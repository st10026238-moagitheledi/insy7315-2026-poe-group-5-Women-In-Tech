'use strict';

const { initializeApp, applicationDefault, getApps } = require('firebase-admin/app');
const { getFirestore } = require('firebase-admin/firestore');
const { getAuth } = require('firebase-admin/auth');
const { getStorage } = require('firebase-admin/storage');
const { readConfig } = require('./settings');
const config = readConfig();
const firebaseApp = getApps()[0] || initializeApp({ projectId: config.projectId, credential: applicationDefault() });
module.exports = { firebaseApp, db: getFirestore(firebaseApp), auth: getAuth(firebaseApp), storage: getStorage(firebaseApp) };
