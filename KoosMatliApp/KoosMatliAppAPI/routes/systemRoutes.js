'use strict';
const express = require('express');
const createController = require('../controllers/systemController');
function systemRoutes() {
  const r = express.Router();
  const controller = createController();
  r.get('/', controller.getStatus);
  r.get('/api/localization/languages', controller.getLanguages);
  r.get('/api/localization/resources', controller.getResources);
  return r;
}
module.exports = systemRoutes;
