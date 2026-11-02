'use strict';
const express = require('express');
const createController = require('../controllers/accountController');
function accountRoutes(dependencies) { //(ExpressJS, 2026a).
  const r = express.Router();
  const controller = createController(dependencies);
  r.post('/api/account/password-reset', controller.resetPassword); //(Open Web Application Security Project [OWASP], 2026).
  return r; //(Node.js Foundation, 2026).
}
module.exports = accountRoutes;

/*Reference List
ExpressJS Documentation, "Routing and Router API Reference in Express," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/guide/routing.html

Open Web Application Security Project (OWASP), "Authentication Cheat Sheet - Password Recovery," OWASP Foundation, 2026. [Online]. Available: https://owasp.org/www-project-cheat-sheets/

Node.js Foundation, "Modular Architecture and Design Patterns in Node.js," Node.js Guides, 2026. [Online]. Available: https://nodejs.org/en/docs/guides

*/