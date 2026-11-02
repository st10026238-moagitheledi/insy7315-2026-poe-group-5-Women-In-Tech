'use strict';
const express = require('express');
const { admin, permission } = require('../middleware/adminPermissions');
const createFeaturesController = require('../controllers/featuresController');
function featuresRoutes(dependencies) {
  const r = express.Router();
  const controller = createFeaturesController(dependencies);
  r.get('/profile', controller.getProfile);
  r.put('/profile', controller.updateProfile);
  r.post('/revoke-sessions', controller.revokeSessions);
  r.get('/access', admin, permission('roles.manage'), controller.getAccess);
  r.put('/access', admin, permission('roles.manage'), controller.updateAccess);
  r.get('/backup', admin, permission('backups.export'), controller.exportBackup);
  return r;
}
module.exports = featuresRoutes;
