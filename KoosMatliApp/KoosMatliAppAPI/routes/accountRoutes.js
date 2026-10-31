'use strict';
const express = require('express');
const createController = require('../controllers/accountController');
function accountRoutes(dependencies) {
  const r = express.Router();
  const controller = createController(dependencies);
  r.post('/api/account/password-reset', controller.resetPassword);
  return r;
}
module.exports = accountRoutes;
