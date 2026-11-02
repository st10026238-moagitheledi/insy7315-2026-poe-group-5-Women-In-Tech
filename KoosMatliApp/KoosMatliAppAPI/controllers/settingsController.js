'use strict';
const { schemas, readRecord, writeRecord, value } = require('../config/contracts');
const { normalize } = require('../middleware/localization');
const { bad } = require('./responseHelpers');
function createSettingsController({ db }) {
  const getSettings = async (req, res) => {
    const s = await db.collection('userSettings').doc(req.user.uid).get(); res.json(readRecord('userSettings', '', s.exists ? s.data() : {})); //(Google Cloud, 2026a).
  };
  const updateSettings = async (req, res) => {
    const language = normalize(value(req.body, 'language', schemas.userSettings.language)); if (!language) return bad(res, req, 'InvalidLanguage');
    const supplied = value(req.body, 'theme', 'System');
    const theme = ['System', 'Light', 'Dark'].find(t => typeof supplied === 'string' && t.toLowerCase() === supplied.toLowerCase());
    if (!theme) return bad(res, req, 'InvalidTheme');
    const notificationsEnabled = value(req.body, 'notificationsEnabled', true), confirmSensitiveActions = value(req.body, 'confirmSensitiveActions', true);
    if (typeof notificationsEnabled !== 'boolean' || typeof confirmSensitiveActions !== 'boolean') return bad(res, req);
    const ref = db.collection('userSettings').doc(req.user.uid);
    await ref.set(writeRecord('userSettings', { language, theme, notificationsEnabled, confirmSensitiveActions }), { merge: true }); //(Google Cloud, 2026b).
    res.json(readRecord('userSettings', '', (await ref.get()).data()));
  };
  return { getSettings, updateSettings };
}
module.exports = createSettingsController;

/*Reference List

Google Cloud, "Manage Data in Cloud Firestore (Document References and Security)," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/firestore/manage-data/add-data

Google Cloud, "Set Data in Cloud Firestore (Merge Options)," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/firestore/manage-data/add-data#set_a_document
*/