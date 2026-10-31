'use strict';
const path = require('node:path');
function readConfig(env = process.env) {
  const projectId = (env.FIREBASE_PROJECT_ID || env.Firebase__ProjectId || '').trim();
  if (!projectId || /\s/.test(projectId) || projectId.startsWith('REPLACE_')) throw new Error('Set FIREBASE_PROJECT_ID to the Firebase project ID.');
  const production = env.NODE_ENV === 'production';
  if (production && (env.FIREBASE_AUTH_EMULATOR_HOST || env.FIRESTORE_EMULATOR_HOST)) throw new Error('Firebase emulators are permitted only outside production.');
  const port = Number(env.PORT || 5000);
  if (!Number.isInteger(port) || port < 1 || port > 65535) throw new Error('PORT must be an integer from 1 to 65535.');
  return { projectId, port, host: env.HOST || '0.0.0.0', webApiKey: env.FIREBASE_WEB_API_KEY || env.Firebase__WebApiKey || '', uploadDir: path.resolve(env.UPLOAD_DIR || path.join(__dirname, '..', 'App_Data', 'uploads')), allowedOrigins: (env.CORS_ALLOWED_ORIGINS || '').split(',').map(x => x.trim()).filter(Boolean), httpsPfxPath: env.HTTPS_PFX_PATH || '', httpsPfxPassword: env.HTTPS_PFX_PASSWORD || '' };
}
module.exports = { readConfig };
