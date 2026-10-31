'use strict';
const express = require('express');
const createController = require('../controllers/settingsController');
function settingsRoutes(dependencies) {
  const r = express.Router();
  const controller = createController(dependencies);
  r.get('/api/settings', controller.getSettings);
  r.put('/api/settings', controller.updateSettings);
  return r;
}
module.exports = settingsRoutes;
