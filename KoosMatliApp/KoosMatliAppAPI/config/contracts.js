'use strict';
const schemas = {
  applications: { id: 0, applicationNumber: '', learnerFirstName: '', learnerLastName: '', dateOfBirth: '0001-01-01T00:00:00Z', gradeApplyingFor: '', guardianName: '', guardianEmail: '', guardianPhone: '', dateSubmitted: '0001-01-01T00:00:00Z', status: 'New' },
  staffMembers: { id: 0, firstName: '', lastName: '', email: '', position: '', subjects: [], classes: [] },
  schoolClasses: { id: 0, className: '', grade: '', teacherName: '', learnerCount: 0 },
  subjects: { id: 0, name: '', grade: '', teacherNames: [] },
  schoolDates: { id: 0, title: '', date: '0001-01-01T00:00:00Z', description: '' },
  schoolDetails: { id: 0, schoolName: '', address: '', phoneNumber: '', email: '', principalName: '' },
  calendarEvents: { id: 0, title: '', start: '0001-01-01T00:00:00Z', end: null, description: '' },
  schoolDocuments: { id: 0, title: '', fileName: '', fileUrl: '', uploadedDate: '0001-01-01T00:00:00Z', category: '' },
  userSettings: { displayName: '', phoneNumber: '', confirmSensitiveActions: true, language: 'en-ZA', theme: 'System', notificationsEnabled: true }
};
const cap = s => s[0].toUpperCase() + s.slice(1);
function value(o, key, fallback) { return o?.[key] ?? o?.[cap(key)] ?? fallback; }
function text(o, key) { const v = value(o, key, ''); return typeof v === 'string' ? v.trim() : ''; }
function date(v) {
  if (v == null || v === '') return null;
  if (typeof v.toDate === 'function') v = v.toDate();

  if (typeof v === 'string' && /^\d{4}-\d\d-\d\dT/.test(v) && !/(Z|[+-]\d\d:\d\d)$/i.test(v)) v += 'Z';
  const d = new Date(v); return Number.isFinite(d.getTime()) ? d : null;
}
function numericId(id) { return /^\d+$/.test(String(id)) && Number(id) <= 2147483647 ? Number(id) : 0; }
function readUser(id, fields) {
  let firstName = text(fields, 'firstName'), lastName = text(fields, 'lastName');
  if (!firstName && !lastName) {
    const name = text(fields, 'displayName').split(/\s+/); firstName = name.shift() || ''; lastName = name.join(' ');
  }
  const role = ['Admin', 'Teacher', 'Parent', 'Learner'].find(r => r.toLowerCase() === text(fields, 'role').toLowerCase()) || '';
  return { id: numericId(id), firstName, lastName, email: text(fields, 'email'), phoneNumber: text(fields, 'phoneNumber'), role, fullName: `${firstName} ${lastName}` };
}
function readRecord(collection, id, fields) {
  if (collection === 'users') return readUser(id, fields);
  const result = {};
  for (const [key, fallback] of Object.entries(schemas[collection] || {})) {
    const v = value(fields, key, fallback);
    result[key] = ['date', 'dateOfBirth', 'dateSubmitted', 'start', 'end', 'uploadedDate'].includes(key)
      ? date(v)?.toISOString() ?? fallback : v;
  }
  if ('id' in result && !result.id) result.id = numericId(id);
  if (collection === 'applications') result.learnerFullName = `${result.learnerFirstName} ${result.learnerLastName}`;
  if (collection === 'staffMembers') result.fullName = `${result.firstName} ${result.lastName}`;
  return result;
}
function writeRecord(collection, record) {
  if (collection === 'users') return { Id: record.id, firstName: record.firstName, lastName: record.lastName, displayName: `${record.firstName} ${record.lastName}`.trim(), email: record.email, phoneNumber: record.phoneNumber, role: record.role.toLowerCase() };
  const result = {};
  for (const key of Object.keys(schemas[collection] || {})) {
    if (record[key] !== undefined) result[cap(key)] = ['date', 'dateOfBirth', 'dateSubmitted', 'start', 'end', 'uploadedDate'].includes(key) ? date(record[key]) : record[key];
  }
  return result;
}
function mergeLearners(profiles, learners) {
  const emails = new Set(profiles.filter(u => u.role === 'Learner' && u.email).map(u => u.email.toLowerCase()));
  return [...profiles, ...learners.filter(u => !u.email || (!emails.has(u.email.toLowerCase()) && emails.add(u.email.toLowerCase())))];
}
function readEvent(id, fields) {
  let start = date(value(fields, 'start')), end = date(value(fields, 'end'));
  const time = text(fields, 'time');
  if (!start) {
    start = date(value(fields, 'date'));
    if (!start) throw new Error(`Event ${id} has no valid date`);
    const times = time.split(/[-–—]/).map(x => x.trim());
    const valid = t => /^\d{1,2}:\d{2}(:\d{2})?$/.test(t || '') && Number(t.split(':')[0]) < 24 && Number(t.split(':')[1]) < 60;
    if (valid(times[0])) {
      const day = new Date(start.getTime() + 7200000).toISOString().slice(0, 10);
      const at = t => new Date(`${day}T${t.padStart(t.length === 4 ? 5 : t.length, '0')}+02:00`);
      start = at(times[0]);
      if (valid(times[1])) { end = at(times[1]); if (end < start) end = new Date(end.getTime() + 86400000); }
    }
  }
  return { id: numericId(id), title: text(fields, 'title'), start: start.toISOString(), end: end?.toISOString() ?? null, description: [text(fields, 'description'), text(fields, 'location'), time].filter(Boolean).join('\n') };
}
function eventFields(record) {
  const local = new Date(date(record.start).getTime() + 7200000).toISOString();
  const end = date(record.end), endLocal = end && new Date(end.getTime() + 7200000).toISOString();
  return { Id: record.id, Title: record.title, Description: record.description, Date: new Date(`${local.slice(0, 10)}T00:00:00+02:00`), Time: local.slice(11, 16) + (endLocal ? ` - ${endLocal.slice(11, 16)}` : ''), Start: date(record.start), End: end, Category: 'General', Location: 'Koos Matli Primary School', IsPublished: true };
}
module.exports = { schemas, value, text, date, numericId, readUser, readRecord, writeRecord, mergeLearners, readEvent, eventFields };
