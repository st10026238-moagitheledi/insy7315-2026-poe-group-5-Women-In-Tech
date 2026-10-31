'use strict';
const { dictionaries } = require('../middleware/localization');
function createSystemController() {
  const getStatus = (req, res) => res.json({ status: 'API running', languages: '/api/localization/languages' });
  const getLanguages = (req, res) => res.json([{ code: 'en-ZA', name: 'English' }, { code: 'tn-ZA', name: 'Setswana' }, { code: 'nso-ZA', name: 'Sepedi' }]);
  const getResources = (req, res) => res.json({ culture: req.culture, strings: { ...dictionaries['en-ZA'], ...dictionaries[req.culture] } });
  return { getStatus, getLanguages, getResources };
}
module.exports = createSystemController;
