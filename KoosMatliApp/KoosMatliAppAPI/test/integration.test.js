'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const { spawn } = require('node:child_process');
const { createServer } = require('node:net');
const path = require('node:path');
const fs = require('node:fs');
test('combined server preserves shared login/health, all teacher routes, legacy accounts and admin contracts', async t => {
  const probe = createServer(); await new Promise(resolve => probe.listen(0, '127.0.0.1', resolve)); const port = probe.address().port; await new Promise(resolve => probe.close(resolve));
  const proc = spawn(process.execPath, ['--require', './test/integration-fixture.js', 'server.js'], { cwd: path.resolve(__dirname, '..'), env: { ...process.env, PORT: String(port), FIREBASE_PROJECT_ID: 'demo-combined-test', FIREBASE_WEB_API_KEY: 'test-key', NODE_ENV: 'development' }, stdio: ['ignore', 'pipe', 'pipe'] });
  let logs = ''; proc.stdout.on('data', d => { logs += d; }); proc.stderr.on('data', d => { logs += d; });
  t.after(() => proc.kill());
  const base = `http://127.0.0.1:${port}`;
  let started = false;
  for (let i = 0; i < 100; i++) { try { await fetch(base); started = true; break; } catch { await new Promise(r => setTimeout(r, 30)); } }
  assert.ok(started, logs);
  const req = async (route, method = 'GET', body, token) => {
    const r = await fetch(base + route, { method, headers: { ...(token ? { Authorization: `Bearer ${token}` } : {}), ...(body ? { 'Content-Type': 'application/json' } : {}) }, body: body ? JSON.stringify(body) : undefined });
    return { status: r.status, data: r.status === 204 ? null : await r.json() };
  };
  assert.equal((await req('/')).data.success, true);
  assert.equal((await req('/api/health')).data.database, 'connected');
  const login = await req('/api/auth/login', 'POST', { email: 'owner@example.com', password: 'test-password' });
  assert.equal(login.data.idToken, 'owner-token'); assert.equal(login.data.user.role, 'admin');
  assert.equal((await req('/api/auth/me', 'GET', null, 'owner-token')).status, 200);
  const dashboard = await req('/api/admin/dashboard', 'GET', null, 'owner-token');
  assert.equal(dashboard.status, 200); assert.equal(dashboard.data.success, true); assert.equal(dashboard.data.statistics.parents, 1); assert.equal(dashboard.data.totalTeachers, 1);
  const created = await req('/api/admin/users', 'POST', { firstName: 'New', lastName: 'Parent', email: 'new@example.com', role: 'Parent' }, 'owner-token');
  assert.equal(created.status, 201); assert.equal(created.data.fullName, 'New Parent');
  assert.ok((await req('/api/admin/users', 'GET', null, 'owner-token')).data.some(u => u.email === 'new@example.com'));
  const legacy = await req('/api/admin/users', 'POST', { displayName: 'New Teacher', email: 'newteacher@example.com', password: 'test-pass-123', role: 'teacher' }, 'owner-token');
  assert.equal(legacy.status, 201); assert.equal(legacy.data.success, true); assert.equal(legacy.data.user.uid, 'created-staff');
  assert.equal((await req('/api/admin/accounts', 'POST', { displayName: 'Denied', email: 'denied@example.com', password: 'test-pass-123', role: 'admin' }, 'scoped-token')).status, 403);
  assert.equal((await req('/api/admin/users', 'POST', { displayName: 'Denied', email: 'denied@example.com', password: 'test-pass-123', role: 'admin' }, 'scoped-token')).status, 403);
  assert.equal((await req('/api/admin/users', 'GET', null, 'parent-token')).status, 403);
  const routes = fs.readFileSync(path.join(__dirname, '../routes/teacherRoutes.js'), 'utf8');
  const endpoints = [...routes.matchAll(/router\.(get|post|patch|delete)\(\s*"([^"]+)"/g)];
  assert.equal(endpoints.length, 21);
  for (const [, method, route] of endpoints) {
    const r = await req('/api/teacher' + route.replace(/:[A-Za-z]+/g, 'test'), method.toUpperCase());
    assert.equal(r.status, 401, route);
  }
  assert.equal((await req('/api/parent/dashboard')).status, 401);
  assert.equal((await req('/api/school-data/parents/test/children', 'POST', {})).status, 401);
  assert.equal((await req('/api/settings', 'GET', null, 'owner-token')).status, 200);
  assert.equal((await req('/api/admin/features/access', 'GET', null, 'owner-token')).status, 200);
});
