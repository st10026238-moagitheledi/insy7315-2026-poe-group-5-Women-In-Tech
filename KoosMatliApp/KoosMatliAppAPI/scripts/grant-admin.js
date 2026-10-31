'use strict';
require('dotenv').config({ quiet: true });
const { readConfig } = require('../config/settings');
const initializeFirebase = () => require('../config/firebase');
async function main() {
  const uid = process.argv[2]?.trim();
  if (!uid || process.argv.length !== 3) throw new Error('Run: npm run grant-admin -- YOUR_FIREBASE_UID');
  const { auth } = initializeFirebase(readConfig());
  const account = await auth.getUser(uid);
  await auth.setCustomUserClaims(uid, { ...account.customClaims, admin: true });
  console.log(`Administrator permission assigned to ${account.email || account.uid}. Sign in again to receive the updated permission.`);
}
main().catch(error => { console.error(error.code || error.message); process.exitCode = 1; });
