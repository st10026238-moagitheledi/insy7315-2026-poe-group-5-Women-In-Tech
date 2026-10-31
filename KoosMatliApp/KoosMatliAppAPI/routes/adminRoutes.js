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
  await createSchoolAdminController(new Store(db)).getDashboard(req, { json: data => { counts = data; } });
  const json = res.json.bind(res);
  res.json = data => json({ ...data, ...counts });
  return legacy.getDashboard(req, res);
});

router.post('/accounts', permission('roles.manage'), legacy.createStaffAccount);
router.post('/users', (req, res, next) => {
  const body = req.body || {};
  if (!Object.hasOwn(body, 'password') && !Object.hasOwn(body, 'displayName')) return next('router');
 
  return permission('roles.manage')(req, res, () => legacy.createStaffAccount(req, res));
});
module.exports = router;
