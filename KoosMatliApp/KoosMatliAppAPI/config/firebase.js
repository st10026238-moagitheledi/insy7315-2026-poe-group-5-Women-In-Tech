'use strict';

const { initializeApp, applicationDefault, getApps } = require('firebase-admin/app');
const { getFirestore } = require('firebase-admin/firestore');
const { getAuth } = require('firebase-admin/auth');
const { getStorage } = require('firebase-admin/storage');
const { readConfig } = require('./settings');
const config = readConfig();
const firebaseApp = getApps()[0] || initializeApp({ projectId: config.projectId, credential: applicationDefault() }); //(Google Cloud, 2026a).
module.exports = { firebaseApp, db: getFirestore(firebaseApp), auth: getAuth(firebaseApp), storage: getStorage(firebaseApp) }; //(Firebase Documentation, 2026).

/*Reference List

Google Cloud, "Initialize Firebase Admin SDK," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/admin/setup

Google Cloud, "Cloud Firestore Node.js Admin SDK Reference," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/reference/admin/node/firebase-admin.firestore
*/