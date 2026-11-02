'use strict';

require('dotenv').config({ quiet: true });
const { readConfig } = require('../config/settings');
const initializeFirebase = () => require('../config/firebase');
const { Store } = require('../config/firestoreStore');
async function main() {
  if (process.env.NODE_ENV === 'production' || process.argv[2] !== '--confirm-demo-write') throw new Error('Development only. Explicitly run npm run seed-demo -- --confirm-demo-write to write marked demo records.');
  const store = new Store(initializeFirebase(readConfig()).db), today = new Date(); today.setUTCHours(0, 0, 0, 0);
  const at = (days, hour = 0) => new Date(today.getTime() + days * 86400000 + hour * 3600000).toISOString();
  async function addMissing(collection, records, key) {
    const keys = new Set((await store.all(collection)).map(r => key(r).toLowerCase()));
    for (const record of records) if (!keys.has(key(record).toLowerCase())) { await store.add(collection, record); keys.add(key(record).toLowerCase()); }
  }
  const apps = [['001', 'Demo Naledi', 'Mokoena', 'Grade 1', 'New', 6], ['002', 'Demo Kabelo', 'Dube', 'Grade 3', 'Pending', 8], ['003', 'Demo Lerato', 'Molefe', 'Grade 5', 'Approved', 10], ['004', 'Demo Thabo', 'Nkosi', 'Grade 2', 'Rejected', 7]].map(([n, first, last, grade, status, age]) => {
    const birth = new Date(today); birth.setUTCFullYear(birth.getUTCFullYear() - age);
    const applicationNumber = `DEMO-APP-${n}`;
    return { id: 0, applicationNumber, learnerFirstName: first, learnerLastName: last, dateOfBirth: birth.toISOString(), gradeApplyingFor: grade, guardianName: `Demo Guardian ${last}`, guardianEmail: applicationNumber.toLowerCase() + '@example.com', guardianPhone: '', dateSubmitted: at(-3), status };
  });
  await addMissing('applications', apps, a => a.applicationNumber);
  await addMissing('staffMembers', [
    { id: 0, firstName: 'Demo Dineo', lastName: 'Molefe', email: 'demo.teacher1@example.com', position: 'Teacher', subjects: ['[DEMO] Mathematics', '[DEMO] English'], classes: ['[DEMO] Grade 3A'] },
    { id: 0, firstName: 'Demo Thabo', lastName: 'Mokoena', email: 'demo.teacher2@example.com', position: 'Teacher', subjects: ['[DEMO] Natural Sciences'], classes: ['[DEMO] Grade 5A'] }
  ], a => a.email);
  await addMissing('schoolClasses', [
    { id: 0, className: '[DEMO] Grade 3A', grade: 'Grade 3', teacherName: 'Demo Dineo Molefe', learnerCount: 0 },
    { id: 0, className: '[DEMO] Grade 5A', grade: 'Grade 5', teacherName: 'Demo Thabo Mokoena', learnerCount: 0 }
  ], a => a.className);
  await addMissing('subjects', [
    { id: 0, name: '[DEMO] Mathematics', grade: 'Grade 3', teacherNames: ['Demo Dineo Molefe'] },
    { id: 0, name: '[DEMO] English', grade: 'Grade 3', teacherNames: ['Demo Dineo Molefe'] },
    { id: 0, name: '[DEMO] Natural Sciences', grade: 'Grade 5', teacherNames: ['Demo Thabo Mokoena'] }
  ], a => a.name + '|' + a.grade);
  await addMissing('schoolDates', [
    { id: 0, title: '[DEMO] Application deadline', date: at(7), description: 'Sample date for testing only.' },
    { id: 0, title: '[DEMO] Parent meeting', date: at(14), description: 'Sample date for testing only.' }
  ], a => a.title);
  await addMissing('calendarEvents', [
    { id: 0, title: '[DEMO] Staff planning', start: at(2, 8), end: at(2, 9), description: 'Sample planning meeting for testing only.' },
    { id: 0, title: '[DEMO] Sports afternoon', start: at(5, 12), end: at(5, 14), description: 'Sample event for testing only.' }
  ], a => a.title);
  console.log('Marked demo records added where missing. Existing records were not overwritten.');
}
main().catch(error => { console.error(error.code || error.message); process.exitCode = 1; });
