'use strict';

const { createHash } = require('node:crypto');
const { readRecord, writeRecord, mergeLearners, readEvent, eventFields } = require('./contracts');
class DuplicateProfileError extends Error {}
class Store {
  constructor(db) { this.db = db; }
  async all(collection) {
    const snapshot = await this.db.collection(collection).get();
    let rows = snapshot.docs.filter(d => collection !== 'users' || d.data().isActive !== false).map(d => readRecord(collection, d.id, d.data()));
    if (collection === 'staffMembers') {
      const emails = new Set(rows.filter(r => r.email).map(r => r.email.toLowerCase()));
      for (const u of await this.all('users')) if (u.role === 'Teacher' && (!u.email || !emails.has(u.email.toLowerCase()))) {
        rows.push({ id: 0, firstName: u.firstName, lastName: u.lastName, email: u.email, fullName: u.fullName, position: 'Teacher', subjects: [], classes: [] });
        if (u.email) emails.add(u.email.toLowerCase());
      }
    }
    if (collection === 'calendarEvents') {
      const shared = await this.db.collection('events').get(); rows.push(...shared.docs.map(d => readEvent(d.id, d.data())));
    }
    return rows;
  }
  async schoolUsers() {
    const [profiles, snapshot] = await Promise.all([this.all('users'), this.db.collection('learners').get()]);

    return mergeLearners(profiles, snapshot.docs.filter(d => d.data().isActive !== false).map(d => ({ ...readRecord('users', d.id, d.data()), role: 'Learner' })));
  }
  async get(collection, id) {
    const snap = await this.db.collection(collection).doc(String(id)).get(); return snap.exists ? readRecord(collection, snap.id, snap.data()) : null;
  }
  async first(collection) { return (await this.all(collection)).sort((a, b) => a.id - b.id)[0] || null; }
  async add(collection, record, uniqueValue) {
    const original = collection; if (collection === 'calendarEvents') collection = 'events';
    const counter = this.db.collection('_metadata').doc(`counter_${collection}`);
    const unique = uniqueValue == null ? null : this.db.collection('_unique').doc(`${collection}_${createHash('sha256').update(uniqueValue.toUpperCase()).digest('hex').toUpperCase()}`);
    await this.db.runTransaction(async tx => {
      const snap = await tx.get(counter); let id = snap.exists ? Number(snap.data().value) : 0;
      if (!Number.isSafeInteger(id) || id < 0) throw new Error('Invalid collection counter');
      if (unique && (await tx.get(unique)).exists) throw new DuplicateProfileError();
      let ref;
      do { if (++id > 2147483647) throw new Error('Integer IDs exhausted'); ref = this.db.collection(collection).doc(String(id)); } while ((await tx.get(ref)).exists);
      const saved = { ...record, id };
      let fields = collection === 'events' ? eventFields(saved) : writeRecord(original, saved);
      if (original === 'users') fields = { ...fields, isActive: true, createdAt: new Date().toISOString() };
      tx.set(counter, { value: id }); tx.create(ref, fields); if (unique) tx.create(unique, { Id: id }); record.id = id;
    });
    return record;
  }
  async save(collection, record) {
    if (!Number.isInteger(record.id) || record.id < 1) throw new Error('A positive ID is required');
    await this.db.collection(collection).doc(String(record.id)).set(writeRecord(collection, record), { merge: true });
  }
  async updateStatus(id, status) {
    const ref = this.db.collection('applications').doc(String(id));
    return this.db.runTransaction(async tx => { if (!(await tx.get(ref)).exists) return false; tx.update(ref, { Status: status }); return true; });
  }
}
module.exports = { Store, DuplicateProfileError };
