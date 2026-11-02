'use strict';
// Test preload only. It prevents all live Firebase access in server integration checks.
const { MemoryDb, fakeAuth } = require('./helpers');
const db = new MemoryDb({
  'users/owner': { uid: 'owner', email: 'owner@example.com', displayName: 'Owner', role: 'admin', isActive: true },
  'users/parent': { uid: 'parent', email: 'parent@example.com', role: 'parent', isActive: true },
  'users/scoped': { uid: 'scoped', role: 'admin', isActive: true },
  'users/teacher': { uid: 'teacher', email: 'teacher@example.com', role: 'teacher', isActive: true }
});
const originalCollection = db.collection.bind(db);
db.collection = name => {
  const c = originalCollection(name), get = c.get, doc = c.doc;
  c.get = async () => { const s = await get(); s.size = s.docs.length; return s; };
  c.doc = id => { const r = doc(id); r.create = async data => { if ((await r.get()).exists) throw new Error('exists'); await r.set(data); }; return r; };
  return c;
};
db.listCollections = async () => [];
const auth = fakeAuth(), verify = auth.verifyIdToken;
auth.verifyIdToken = (token, check) => token === 'teacher-token' ? Promise.resolve({ uid: 'teacher', role: 'Teacher' }) : verify(token, check);
auth.createUser = async input => { const user = { ...input, uid: 'created-staff', customClaims: {}, providerData: [], metadata: {} }; auth.accounts.set(user.uid, user); return user; };
auth.deleteUser = async uid => auth.accounts.delete(uid);
const file = require.resolve('../config/firebase');
require.cache[file] = { id: file, filename: file, loaded: true, exports: { auth, db, storage: {}, firebaseApp: {} } };
const originalFetch = global.fetch;
global.fetch = async (url, options) => {
  if (String(url).startsWith('https://identitytoolkit.googleapis.com/')) return { ok: true, json: async () => ({ localId: 'owner', email: 'owner@example.com', idToken: 'owner-token', expiresIn: '3600', refreshToken: 'test-refresh' }) };
  return originalFetch(url, options);
};
