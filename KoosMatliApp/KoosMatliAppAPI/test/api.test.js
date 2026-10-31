'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const { harness } = require('./helpers');
const { Store, DuplicateProfileError } = require('../config/firestoreStore');
const { readEvent, readUser, eventFields } = require('../config/contracts');
const { validFile } = require('../controllers/documentsController');
const { readConfig } = require('../config/settings');
test('public localization and root, bearer verification, role and scoped permission enforcement', async t => {
  const h = await harness(t);
  assert.equal((await h.request('/', { token: null })).data.status, 'API running');
  assert.equal((await h.request('/api/localization/languages', { token: null })).data.length, 3);
  for (const culture of ['en-ZA', 'tn-ZA', 'nso-ZA']) {
    const r = await h.request('/api/localization/resources', { token: null, headers: { 'Accept-Language': culture } });
    assert.equal(r.data.culture, culture); assert.equal(r.headers.get('content-language'), culture); assert.ok(r.data.strings.Forbidden);
  }
  assert.equal((await h.request('/api/admin/users', { token: null })).status, 401);
  assert.equal((await h.request('/api/admin/users', { token: 'revoked' })).status, 401);
  assert.equal((await h.request('/api/admin/users', { token: 'parent-token' })).status, 403);
  assert.equal((await h.request('/api/admin/users', { token: 'scoped-token' })).status, 403);
  assert.equal((await h.request('/api/admin/applications', { token: 'scoped-token' })).status, 200);
  assert.equal((await h.request('/api/admin/applications/1/status', { token: 'scoped-token', method: 'PUT', body: { status: 'Approved' } })).status, 403);
  assert.ok(h.auth.calls.every(c => c.checkRevoked === true));
  assert.equal((await h.request('/api/admin/users/xyz')).status, 404);
  assert.equal((await h.request('/api/settings', { method: 'PUT', raw: '{bad', headers: { 'Content-Type': 'application/json' } })).status, 400);
});
test('create user persists to Firestore format, GET lists it, duplicate conflicts, learners retained, dashboard agrees', async t => {
  const h = await harness(t, {
    'users/teacher-uid': { displayName: 'Teacher Existing', email: 'teacher@example.com', role: 'teacher', isActive: true },
    'staffMembers/1': { Id: 1, FirstName: 'Teacher', LastName: 'Existing', Email: 'TEACHER@example.com', Position: 'Teacher' },
    'learners/child-uid': { firstName: 'Existing', lastName: 'Learner', isActive: true },
    'users/inactive': { role: 'parent', isActive: false }
  });
  const r = await h.request('/api/admin/users', { method: 'POST', body: { firstName: ' New ', lastName: 'Learner', email: 'new@example.com', role: 'learner' } });
  assert.equal(r.status, 201); assert.equal(r.data.fullName, 'New Learner');
  assert.equal(h.db.rows.get(`users/${r.data.id}`).role, 'learner');
  const users = (await h.request('/api/admin/users')).data;
  assert.equal(users.length, 3); assert.ok(users.some(u => u.email === 'new@example.com'));
  assert.equal((await h.request('/api/admin/users/role/learner')).data.length, 2);
  assert.equal((await h.request(`/api/admin/users/${r.data.id}`)).data.firstName, 'New');
  assert.equal((await h.request('/api/admin/dashboard')).data.totalTeachers, 1);
  assert.equal((await h.request('/api/admin/dashboard')).data.totalLearners, 2);
  assert.equal((await h.request('/api/admin/staff')).data.length, 1);
  assert.equal((await h.request('/api/admin/users', { method: 'POST', body: { firstName: 'Again', lastName: 'Test', email: 'NEW@example.com', role: 'Parent' } })).status, 409);
  assert.equal((await h.request('/api/admin/users', { method: 'POST', body: { firstName: 'A', lastName: 'B', email: 'a@b.com', role: 'Admin' } })).status, 400);
  assert.equal((await h.request('/api/admin/users', { method: 'POST', body: { firstName: 'A', lastName: 'B', email: 'bad', role: 'Teacher' } })).status, 400);
});
test('transaction IDs skip existing records and unique reservations reject concurrent duplicate creates', async t => {
  const h = await harness(t, { 'users/1': { Email: 'old@example.com' } }), store = new Store(h.db);
  const record = email => ({ id: 0, firstName: 'A', lastName: 'B', email, phoneNumber: '', role: 'Parent' });
  const outcomes = await Promise.allSettled([store.add('users', record('a@b.com'), 'a@b.com'), store.add('users', record('A@b.com'), 'A@b.com')]);
  assert.equal(outcomes.filter(o => o.status === 'fulfilled').length, 1);
  assert.ok(outcomes.find(o => o.status === 'rejected').reason instanceof DuplicateProfileError);
  assert.equal(outcomes.find(o => o.status === 'fulfilled').value.id, 2); assert.equal(h.db.rows.get('users/1').Email, 'old@example.com');
});
test('application detail, statuses and dashboard counters retain all other application fields', async t => {
  const h = await harness(t, { 'applications/1': { Id: 1, LearnerFirstName: 'Naledi', LearnerLastName: 'Test', Status: 'New', GuardianEmail: 'keep@example.com' }, 'applications/2': { Id: 2, Status: 'Pending' } });
  assert.equal((await h.request('/api/admin/applications/1')).data.learnerFullName, 'Naledi Test');
  assert.equal((await h.request('/api/admin/dashboard')).data.pendingApplications, 1);
  for (const status of ['Pending', 'Approved', 'Rejected', 'New']) assert.equal((await h.request('/api/admin/applications/1/status', { method: 'PUT', body: { status } })).status, 204);
  assert.equal(h.db.rows.get('applications/1').GuardianEmail, 'keep@example.com');
  assert.equal((await h.request('/api/admin/applications/1/status', { method: 'PUT', body: { status: 'invalid' } })).status, 400);
  assert.equal((await h.request('/api/admin/applications/99/status', { method: 'PUT', body: { status: 'Approved' } })).status, 404);
});
test('school details, dates, classes, subjects and South African event times round-trip', async t => {
  const h = await harness(t, { 'schoolClasses/1': { Id: 1, ClassName: '4A' }, 'subjects/1': { Id: 1, Name: 'English' }, 'events/1': { Date: new Date('2026-10-04T22:00:00Z'), Time: '14:00 - 15:30', Title: 'Revision' }, 'staffMembers/1': { Id: 1, Position: 'Principal' } });
  assert.equal((await h.request('/api/admin/school/details')).status, 404);
  assert.equal((await h.request('/api/admin/school/details', { method: 'PUT', body: { schoolName: 'Koos Matli', address: 'School road' } })).status, 200);
  assert.equal((await h.request('/api/admin/school/details')).data.address, 'School road');
  assert.equal((await h.request('/api/admin/staff/classes')).data[0].className, '4A');
  assert.equal((await h.request('/api/admin/staff/subjects')).data[0].name, 'English');
  assert.equal((await h.request('/api/admin/staff/1')).data.position, 'Principal');
  assert.equal((await h.request('/api/admin/school/dates', { method: 'POST', body: { title: 'Deadline', date: '2026-11-01T00:00:00Z' } })).status, 201);
  assert.equal((await h.request('/api/admin/school/dates')).data.length, 1);
  assert.equal((await h.request('/api/admin/school/calendar')).data[0].start, '2026-10-05T12:00:00.000Z');
  assert.equal((await h.request('/api/admin/school/calendar', { method: 'POST', body: { title: 'Planning', start: '2026-11-02T08:00:00Z', end: '2026-11-02T09:00:00Z' } })).status, 201);
  assert.equal(h.db.rows.get('events/2').Time, '10:00 - 11:00');
  assert.equal((await h.request('/api/admin/school/calendar')).data.length, 2);
  assert.equal((await h.request('/api/admin/school/calendar', { method: 'POST', body: { title: 'Bad', start: '2026-11-02T08:00:00Z', end: '2026-11-01T08:00:00Z' } })).status, 400);
});
test('settings merge preserves profile data and validates themes and languages', async t => {
  const h = await harness(t, { 'userSettings/owner': { DisplayName: 'Keep', PhoneNumber: '123' } });
  for (const language of ['en-ZA', 'tn-ZA', 'nso-ZA']) {
    const r = await h.request('/api/settings', { method: 'PUT', body: { language, theme: 'dark', notificationsEnabled: false, confirmSensitiveActions: true, displayName: 'Do not overwrite' } });
    assert.equal(r.status, 200); assert.equal(r.data.language, language); assert.equal(r.data.theme, 'Dark'); assert.equal(r.data.displayName, 'Keep');
  }
  assert.equal((await h.request('/api/settings')).data.phoneNumber, '123');
  assert.equal((await h.request('/api/settings', { method: 'PUT', body: { language: 'xx' } })).status, 400);
  assert.equal((await h.request('/api/settings', { method: 'PUT', body: { theme: 'pink' } })).status, 400);
});
test('profile updates Auth and matching Firestore profile; email needs recent authentication and revokes sessions', async t => {
  const h = await harness(t, { 'users/owner': { displayName: 'Old', email: 'owner@example.com', role: 'admin' } });
  assert.equal((await h.request('/api/admin/features/profile')).data.role, 'Owner');
  assert.equal((await h.request('/api/admin/features/profile', { method: 'PUT', body: { displayName: 'New Name', email: 'owner@example.com', phoneNumber: '0123456789' } })).status, 200);
  assert.equal(h.auth.accounts.get('owner').displayName, 'New Name'); assert.equal(h.db.rows.get('users/owner').displayName, 'New Name');
  assert.equal((await h.request('/api/admin/features/profile')).data.phoneNumber, '0123456789');
  assert.equal((await h.request('/api/admin/features/profile', { token: 'stale-token', method: 'PUT', body: { displayName: 'New', email: 'changed@example.com' } })).status, 400);
  assert.equal((await h.request('/api/admin/features/profile', { method: 'PUT', body: { displayName: 'New', email: 'changed@example.com' } })).data.emailChanged, true);
  assert.equal(h.auth.accounts.get('owner').emailVerified, false); assert.deepEqual(h.auth.revoked, ['owner']);
  assert.equal((await h.request('/api/admin/features/revoke-sessions', { method: 'POST' })).status, 204);
});
test('access changes protect owner/self, retain unrelated claims, enforce grant ceiling and update profile', async t => {
  const h = await harness(t, { 'users/target': { displayName: 'Teacher Test', email: 'teacher@example.com', role: 'teacher' } });
  assert.ok((await h.request('/api/admin/features/access')).data.some(r => r.uid === 'target' && r.role === 'Teacher'));
  const edit = { uid: 'target', profileId: 0, role: 'Admin', permissions: ['dashboard.read', 'users.write'] };
  assert.equal((await h.request('/api/admin/features/access', { method: 'PUT', body: { ...edit, uid: 'owner' } })).status, 400);
  assert.equal((await h.request('/api/admin/features/access', { method: 'PUT', body: { ...edit, uid: 'otherOwner' } })).status, 400);
  assert.equal((await h.request('/api/admin/features/access', { method: 'PUT', token: 'manager-token', body: edit })).status, 403);
  assert.equal((await h.request('/api/admin/features/access', { method: 'PUT', body: edit })).status, 204);
  const claims = h.auth.accounts.get('target').customClaims;
  assert.equal(claims.retained, 'keep'); assert.equal(claims.accessManaged, true); assert.ok(claims.permissions.includes('users.read'));
  assert.equal(h.db.rows.get('users/target').role, 'admin'); assert.ok(h.auth.revoked.includes('target'));
  assert.ok([...h.db.rows.keys()].some(k => k.startsWith('adminAudit/')));
  assert.equal((await h.request('/api/admin/features/access', { method: 'PUT', body: { ...edit, permissions: ['not-real'] } })).status, 400);
});
test('password reset targets signed-in account only; backup has expected shape and scoped enforcement', async t => {
  const h = await harness(t);
  assert.equal((await h.request('/api/account/password-reset', { method: 'POST', body: { email: 'someone-else@example.com' } })).status, 204);
  assert.equal(JSON.parse(h.sent[0].options.body).email, 'owner@example.com');
  const backup = await h.request('/api/admin/features/backup'); assert.equal(backup.data.formatVersion, 1);
  for (const key of ['users', 'applications', 'staff', 'classes', 'subjects', 'schoolDates', 'calendarEvents', 'documents']) assert.ok(Array.isArray(backup.data[key]));
  assert.equal((await h.request('/api/admin/features/backup', { token: 'scoped-token' })).status, 403);
  h.config.webApiKey = ''; assert.equal((await h.request('/api/account/password-reset', { method: 'POST' })).status, 503);
});
test('document multipart upload/download, signature validation, auth and size limit', async t => {
  const h = await harness(t);
  const form = (content, filename) => { const f = new FormData(); f.append('file', new Blob([content]), filename); f.append('title', 'Test document'); return f; };
  assert.equal((await h.request('/api/admin/documents', { method: 'POST', raw: form('hello', 'notes.txt'), token: null })).status, 401);
  const saved = await h.request('/api/admin/documents', { method: 'POST', raw: form('hello', 'notes.txt') }); assert.equal(saved.status, 201);
  assert.equal((await h.request('/api/admin/documents')).data.length, 1);
  assert.equal((await h.request(`/api/admin/documents/${saved.data.id}`)).data.title, 'Test document');
  const file = await h.request(saved.data.fileUrl); assert.equal(file.status, 200); assert.equal(file.data, 'hello'); assert.ok(file.headers.get('content-disposition').includes('notes.txt'));
  assert.equal((await h.request('/api/admin/documents', { method: 'POST', raw: form('not pdf', 'fake.pdf') })).status, 400);
  assert.equal((await h.request('/api/admin/documents', { method: 'POST', raw: form(Buffer.alloc(10 * 1024 * 1024 + 1, 65), 'big.txt') })).status, 413);
  assert.equal((await h.request('/api/admin/documents')).data.length, 1);
  h.db.rows.get(`schoolDocumentFiles/${saved.data.id}`).StoredName = '../outside';
  assert.equal((await h.request(saved.data.fileUrl)).status, 404);
});
test('mapping, upload validation and production configuration edge cases', async () => {
  assert.equal(readUser('firebase-uid', { displayName: 'Full Name', role: 'teacher' }).role, 'Teacher');
  assert.equal(readEvent('1', { Date: '2026-10-31T22:00:00Z', Time: '23:00 - 01:00' }).end, '2026-11-01T23:00:00.000Z');
  assert.equal(eventFields({ id: 1, title: 'x', description: '', start: '2026-11-01T08:00:00Z', end: null }).Date.toISOString(), '2026-10-31T22:00:00.000Z');
  assert.equal(await validFile(Buffer.from([255, 216, 255]), '.jpg'), true);
  assert.equal(await validFile(Buffer.from([0xff]), '.txt'), false);
  assert.equal(await validFile(Buffer.from('hello\0'), '.txt'), false);
  assert.equal(await validFile(Buffer.from('not zip'), '.docx'), false);
  const fs = require('node:fs/promises'), path = require('node:path');
  assert.equal(await validFile(await fs.readFile(path.join(__dirname, 'fixtures/word.docx')), '.docx'), true);
  assert.equal(await validFile(await fs.readFile(path.join(__dirname, 'fixtures/ordinary.zip')), '.docx'), false);
  assert.throws(() => readConfig({ FIREBASE_PROJECT_ID: 'good-id', NODE_ENV: 'production', FIRESTORE_EMULATOR_HOST: 'localhost:8080' }));
  assert.throws(() => readConfig({ FIREBASE_PROJECT_ID: 'bad id' }));
  assert.equal(readConfig({ FIREBASE_PROJECT_ID: 'good-id' }).port, 5000);
});
