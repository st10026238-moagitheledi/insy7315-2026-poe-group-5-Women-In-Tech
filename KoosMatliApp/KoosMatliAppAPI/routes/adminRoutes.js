'use strict';

const express = require('express');
const authenticate = require('../middleware/authenticate');
const authorize = require('../middleware/authorize');
const legacy = require('../controllers/adminController');
const createSchoolAdminController = require('../controllers/schoolAdminController');
const { Store } = require('../config/firestoreStore');
const { db } = require('../config/firebase');
const { permission } = require('../middleware/adminPermissions');
const router = express.Router();
router.use(authenticate, authorize('admin'));
router.get('/dashboard', permission('dashboard.read'), async (req, res) => {

  let counts;
  await createSchoolAdminController(new Store(db)).getDashboard(req, { json: data => { counts = data; } }); //(Fowler, 2002).
  const json = res.json.bind(res); //(ExpressJS, 2026a).
  res.json = data => json({ ...data, ...counts });
  return legacy.getDashboard(req, res);
});

router.post('/accounts', permission('roles.manage'), legacy.createStaffAccount);
router.post('/users', (req, res, next) => {
  const body = req.body || {};
  if (!Object.hasOwn(body, 'password') && !Object.hasOwn(body, 'displayName')) return next('router'); //(ExpressJS, 2026b).
 
  return permission('roles.manage')(req, res, () => legacy.createStaffAccount(req, res)); //(Open Web Application Security Project [OWASP], 2026).
});
module.exports = router;

/*Reference List

M. Fowler, Patterns of Enterprise Application Architecture, Addison-Wesley Professional, 2002.

ExpressJS Documentation, "Express Routing and Response Methods," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/guide/routing.html

Open Web Application Security Project (OWASP), "Access Control and Authorization Architecture," OWASP Foundation, 2026. [Online]. Available: https://owasp.org/www-community/Access_Control
*/
