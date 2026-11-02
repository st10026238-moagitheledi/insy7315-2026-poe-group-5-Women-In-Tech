'use strict';
// In-memory test adapter only. Production always uses Firebase Admin Firestore.
const { createApp } = require('../config/adminApp');
const { mkdtemp, rm } = require('node:fs/promises');
const { tmpdir } = require('node:os');
const path = require('node:path');
class MemoryDb {
  constructor(seed = {}) { this.rows = new Map(Object.entries(structuredClone(seed))); this.serial = Promise.resolve(); this.sequence = 0; }
  doc(key) {
    return { key, id: key.split('/').at(-1), get: async () => this.snapshot(key), set: async (data, options) => { this.rows.set(key, options?.merge ? { ...this.rows.get(key), ...data } : structuredClone(data)); }, delete: async () => this.rows.delete(key) };
  }
  snapshot(key) { return { id: key.split('/').at(-1), exists: this.rows.has(key), data: () => structuredClone(this.rows.get(key)) }; }
  collection(name) {
    return { doc: id => this.doc(`${name}/${id}`), get: async () => ({ docs: [...this.rows.keys()].filter(k => k.startsWith(name + '/') && k.split('/').length === 2).map(k => this.snapshot(k)) }), add: async data => { const ref = this.doc(`${name}/auto-${++this.sequence}`); await ref.set(data); return ref; } };
  }
  runTransaction(callback) {
    const task = this.serial.then(async () => {
      const pending = []; let writing = false;
      const result = await callback({ get: async ref => { if (writing) throw new Error('Firestore requires all reads before writes'); return this.snapshot(ref.key); }, set: (ref, data) => { writing = true; pending.push([ref.key, data]); }, create: (ref, data) => { writing = true; if (this.rows.has(ref.key)) throw new Error('Already exists'); pending.push([ref.key, data]); }, update: (ref, data) => { writing = true; pending.push([ref.key, { ...this.rows.get(ref.key), ...data }]); } });
      for (const [key, data] of pending) this.rows.set(key, structuredClone(data)); return result;
    }); this.serial = task.catch(() => {}); return task;
  }
}
function fakeAuth() {
  const accounts = new Map([
    ['owner', { uid: 'owner', email: 'owner@example.com', displayName: 'Owner', phoneNumber: '', customClaims: { admin: true }, providerData: [{ providerId: 'password' }], emailVerified: true, disabled: false, metadata: { creationTime: '2026-01-01T00:00:00Z', lastSignInTime: '2026-10-31T00:00:00Z' } }],
    ['target', { uid: 'target', email: 'teacher@example.com', displayName: 'Teacher Test', customClaims: { retained: 'keep' }, providerData: [], metadata: {} }],
    ['otherOwner', { uid: 'otherOwner', email: 'other@example.com', customClaims: { admin: true }, metadata: {} }]
  ]);
  const revoked = [], calls = [];
  return {
    accounts, revoked, calls,
    verifyIdToken: async (token, checkRevoked) => {
      calls.push({ token, checkRevoked });
      const now = Math.floor(Date.now() / 1000);
      if (token === 'owner-token') return { uid: 'owner', admin: true, auth_time: now };
      if (token === 'stale-token') return { uid: 'owner', admin: true, auth_time: now - 600 };
      if (token === 'parent-token') return { uid: 'parent', role: 'Parent' };
      if (token === 'scoped-token') return { uid: 'scoped', admin: true, accessManaged: true, permissions: ['dashboard.read', 'applications.read'] };
      if (token === 'manager-token') return { uid: 'manager', admin: true, accessManaged: true, permissions: ['dashboard.read', 'roles.manage', 'users.read'] };
      const error = new Error('bad token'); error.code = 'auth/id-token-revoked'; throw error;
    },
    getUser: async uid => { if (!accounts.has(uid)) { const e = new Error('missing'); e.code = 'auth/user-not-found'; throw e; } return structuredClone(accounts.get(uid)); },
    updateUser: async (uid, changes) => { Object.assign(accounts.get(uid), changes); return accounts.get(uid); },
    listUsers: async () => ({ users: [...accounts.values()].map(a => structuredClone(a)) }),
    setCustomUserClaims: async (uid, claims) => { accounts.get(uid).customClaims = structuredClone(claims); },
    revokeRefreshTokens: async uid => { revoked.push(uid); }
  };
}
async function harness(t, seed = {}, extra = {}) {
  const db = new MemoryDb(seed), auth = fakeAuth(), uploadDir = await mkdtemp(path.join(tmpdir(), 'koos-admin-test-'));
  const config = { uploadDir, allowedOrigins: ['https://allowed.example'], webApiKey: 'test-only-key' };
  const sent = [];
  const app = createApp({ db, auth, config, fetchImpl: async (url, options) => { sent.push({ url, options }); return { ok: true }; }, logger: { error() {} }, ...extra });
  const server = app.listen(0, '127.0.0.1'); await new Promise(resolve => server.once('listening', resolve));
  t.after(async () => { server.closeAllConnections(); await new Promise(resolve => server.close(resolve)); await rm(uploadDir, { recursive: true, force: true }); });
  async function request(route, { token = 'owner-token', method = 'GET', body, headers = {}, raw } = {}) {
    const response = await fetch(`http://127.0.0.1:${server.address().port}${route}`, { method, headers: { ...(token ? { Authorization: `Bearer ${token}` } : {}), ...(body !== undefined ? { 'Content-Type': 'application/json' } : {}), ...headers }, body: raw ?? (body !== undefined ? JSON.stringify(body) : undefined) });
    const bytes = Buffer.from(await response.arrayBuffer()), txt = bytes.toString(); let data; try { data = JSON.parse(txt); } catch { data = txt; }
    return { status: response.status, headers: response.headers, data, bytes };
  }
  return { db, auth, config, uploadDir, request, sent };
}
module.exports = { MemoryDb, fakeAuth, harness };
