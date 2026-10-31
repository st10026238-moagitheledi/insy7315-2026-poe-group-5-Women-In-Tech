'use strict';

const { readRecord, value, text, date } = require('../config/contracts');
const { DuplicateProfileError } = require('../config/firestoreStore');
const { bad, recordOr404, emailValid } = require('./responseHelpers');
function createAdminController(store) {

  const getDashboard = async (req, res) => {
    const [users, staff, apps] = await Promise.all([store.schoolUsers(), store.all('staffMembers'), store.all('applications')]);
    const teachers = new Set([
      ...users.filter(u => u.role === 'Teacher').map(u => u.email?.trim().toLowerCase() || `user:${u.id}`),
      ...staff.filter(s => s.position?.toLowerCase() === 'teacher').map(s => s.email?.trim().toLowerCase() || `staff:${s.id}`)
    ]);
    res.json({ totalLearners: users.filter(u => u.role === 'Learner').length, totalTeachers: teachers.size, totalParents: users.filter(u => u.role === 'Parent').length, newApplications: apps.filter(a => a.status.toLowerCase() === 'new').length, pendingApplications: apps.filter(a => a.status.toLowerCase() === 'pending').length, announcementsSent: 0 });
  };
  const getUsers = async (req, res) => res.json(await store.schoolUsers());
  const getUsersByRole = async (req, res) => res.json((await store.schoolUsers()).filter(u => u.role.toLowerCase() === req.params.role.toLowerCase()));
  const getUser = async (req, res) => recordOr404(req, res, await store.get('users', req.params.id));
  const createUser = async (req, res) => {
    const firstName = text(req.body, 'firstName'), lastName = text(req.body, 'lastName'), email = text(req.body, 'email');
    if (!firstName) return bad(res, req, 'First name is required.');
    if (!lastName) return bad(res, req, 'Surname is required.');
    if (!email) return bad(res, req, 'Email is required.');
    if (!text(req.body, 'role')) return bad(res, req, 'A role is required.');
    const role = ['Teacher', 'Parent', 'Learner'].find(r => r.toLowerCase() === text(req.body, 'role').toLowerCase());
    if (!role) return bad(res, req, 'Role must be Teacher, Parent or Learner.');
    if (!emailValid(email)) return bad(res, req);
    if ((await store.schoolUsers()).some(u => u.email.toLowerCase() === email.toLowerCase())) return bad(res, req, 'A user with this email already exists.', 409);
    const user = { id: 0, firstName, lastName, email, phoneNumber: text(req.body, 'phoneNumber'), role, fullName: `${firstName} ${lastName}` };
    try { await store.add('users', user, email); }
    catch (e) { if (e instanceof DuplicateProfileError) return bad(res, req, 'A user with this email already exists.', 409); throw e; }
    res.location(`/api/admin/users/${user.id}`).status(201).json(user);
  };
  const getApplications = async (req, res) => res.json(await store.all('applications'));
  const getApplication = async (req, res) => recordOr404(req, res, await store.get('applications', req.params.id));
  const updateApplicationStatus = async (req, res) => {
    const supplied = text(req.body, 'status'); if (!supplied) return bad(res, req, 'A status is required.');
    const status = ['New', 'Pending', 'Approved', 'Rejected'].find(s => s.toLowerCase() === supplied.toLowerCase());
    if (!status) return bad(res, req, 'InvalidStatus');
    return await store.updateStatus(req.params.id, status) ? res.sendStatus(204) : bad(res, req, 'NotFound', 404);
  };
  const getStaff = async (req, res) => res.json(await store.all('staffMembers'));
  const getClasses = async (req, res) => res.json(await store.all('schoolClasses'));
  const getSubjects = async (req, res) => res.json(await store.all('subjects'));
  const getStaffMember = async (req, res) => recordOr404(req, res, await store.get('staffMembers', req.params.id));
  const getSchoolDetails = async (req, res) => recordOr404(req, res, await store.first('schoolDetails'));
  const updateSchoolDetails = async (req, res) => {
    if (!text(req.body, 'schoolName')) return bad(res, req, 'A school name is required.');
    const details = readRecord('schoolDetails', 1, req.body); details.id = (await store.first('schoolDetails'))?.id || 1;
    for (const key of Object.keys(details)) if (typeof details[key] === 'string') details[key] = details[key].trim();
    await store.save('schoolDetails', details); res.json(details);
  };
  const getSchoolDates = async (req, res) => res.json((await store.all('schoolDates')).sort((a, b) => a.date.localeCompare(b.date)));
  const addSchoolDate = async (req, res) => {
    if (!text(req.body, 'title')) return bad(res, req, 'A title is required.');
    const d = date(value(req.body, 'date')); if (!d || d.getUTCFullYear() < 2) return bad(res, req, 'A valid date is required.');
    const record = { id: 0, title: text(req.body, 'title'), date: d.toISOString(), description: text(req.body, 'description') };
    await store.add('schoolDates', record); res.location(`/api/admin/school/dates?id=${record.id}`).status(201).json(record);
  };
  const getCalendarEvents = async (req, res) => res.json((await store.all('calendarEvents')).sort((a, b) => a.start.localeCompare(b.start)));
  const addCalendarEvent = async (req, res) => {
    if (!text(req.body, 'title')) return bad(res, req, 'An event title is required.');
    const start = date(value(req.body, 'start')), endValue = value(req.body, 'end'), end = date(endValue);
    if (!start || start.getUTCFullYear() < 2) return bad(res, req, 'A valid event start date and time is required.');
    if (endValue != null && !end) return bad(res, req);
    if (end && end < start) return bad(res, req, 'The event end must be after its start.');
    const record = { id: 0, title: text(req.body, 'title'), start: start.toISOString(), end: end?.toISOString() ?? null, description: text(req.body, 'description') };
    await store.add('calendarEvents', record); res.location(`/api/admin/school/calendar?id=${record.id}`).status(201).json(record);
  };
  return { getDashboard, getUsers, getUsersByRole, getUser, createUser, getApplications, getApplication, updateApplicationStatus, getStaff, getClasses, getSubjects, getStaffMember, getSchoolDetails, updateSchoolDetails, getSchoolDates, addSchoolDate, getCalendarEvents, addCalendarEvent };
}
module.exports = createAdminController;
