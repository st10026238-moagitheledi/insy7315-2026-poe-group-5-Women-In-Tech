'use strict';
const express = require('express');
const createController = require('../controllers/settingsController');
function settingsRoutes(dependencies) {
  const r = express.Router(); //(ExpressJS, 2026a).
  const controller = createController(dependencies); //(Node.js Foundation, 2026).
  r.get('/api/settings', controller.getSettings);
  r.put('/api/settings', controller.updateSettings);
  return r;
}
module.exports = settingsRoutes;

/*Reference List

ExpressJS Documentation, "express.Router() API Reference," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/5x/api.html#router

Node.js Foundation, "Modular Design Patterns in Node.js," Node.js Guides, 2026. [Online]. Available: https://nodejs.org/en/docs/guides
*/