'use strict';
const express = require('express');
const { admin, permission } = require('../middleware/adminPermissions');
const { bad } = require('../controllers/responseHelpers');
const createAdminController = require('../controllers/schoolAdminController');
function adminRoutes(store) {
  const r = express.Router(); //(ExpressJS, 2026a).
  const controller = createAdminController(store);
  r.use(admin);
  r.param('id', (req, res, next, id) => /^\d+$/.test(id) && Number(id) > 0 && Number(id) <= 2147483647 ? next() : bad(res, req, 'NotFound', 404));
  r.get('/dashboard', permission('dashboard.read'), controller.getDashboard);
  r.get('/users', permission('users.read'), controller.getUsers); //(Open Web Application Security Project [OWASP], 2026).
  r.get('/users/role/:role', permission('users.read'), controller.getUsersByRole);
  r.get('/users/:id', permission('users.read'), controller.getUser);
  r.post('/users', permission('users.write'), controller.createUser);
  r.get('/applications', permission('applications.read'), controller.getApplications);
  r.get('/applications/:id', permission('applications.read'), controller.getApplication);
  r.put('/applications/:id/status', permission('applications.write'), controller.updateApplicationStatus);
  r.get('/staff', permission('staff.read'), controller.getStaff);
  r.get('/staff/classes', permission('staff.read'), controller.getClasses);
  r.get('/staff/subjects', permission('staff.read'), controller.getSubjects);
  r.get('/staff/:id', permission('staff.read'), controller.getStaffMember);
  r.get('/school/details', permission('school.read'), controller.getSchoolDetails);
  r.put('/school/details', permission('school.write'), controller.updateSchoolDetails);
  r.get('/school/dates', permission('school.read'), controller.getSchoolDates);
  r.post('/school/dates', permission('school.write'), controller.addSchoolDate);
  r.get('/school/calendar', permission('school.read'), controller.getCalendarEvents);
  r.post('/school/calendar', permission('school.write'), controller.addCalendarEvent);
  return r;
}
module.exports = adminRoutes;

/*Refernce List

ExpressJS Documentation, "Route Parameters and router.param() API Reference," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/5x/api.html#router.param

Open Web Application Security Project (OWASP), "Principle of Least Privilege and Authorization Controls," OWASP Foundation, 2026. [Online]. Available: https://owasp.org/www-community/Access_Control

Node.js Foundation, "Safe Integer Limits and Numeric Validation in JavaScript," Node.js API Reference, 2026. [Online]. Available: https://nodejs.org/en/docs/guides
*/