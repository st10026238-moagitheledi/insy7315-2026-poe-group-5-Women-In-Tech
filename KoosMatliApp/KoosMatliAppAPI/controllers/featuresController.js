'use strict';

const { permissionCodes, isOwner, roleOf, permissionsOf } = require('../middleware/adminPermissions');
const { readRecord, value, text } = require('../config/contracts');
const { bad, emailValid } = require('./responseHelpers');
function createFeaturesController({ auth, db, store }) {

  const getProfile = async (req, res) => {
    const user = await auth.getUser(req.user.uid), snap = await db.collection('userSettings').doc(user.uid).get();
    const settings = readRecord('userSettings', '', snap.exists ? snap.data() : {});
    res.json({ uid: user.uid, email: user.email ?? null, displayName: user.displayName || settings.displayName, phoneNumber: settings.phoneNumber || user.phoneNumber || '', emailVerified: user.emailVerified, disabled: user.disabled, createdAt: user.metadata.creationTime ? new Date(user.metadata.creationTime).toISOString() : null, lastSignInAt: user.metadata.lastSignInTime ? new Date(user.metadata.lastSignInTime).toISOString() : null, role: isOwner(user) ? 'Owner' : roleOf(user) });
  };
  const updateProfile = async (req, res) => {
    const displayName = text(req.body, 'displayName'), email = text(req.body, 'email'), phoneNumber = text(req.body, 'phoneNumber');
    if (!displayName || displayName.length > 120 || !emailValid(email) || phoneNumber.length > 30) return bad(res, req);
    const user = await auth.getUser(req.user.uid), changed = (user.email || '').toLowerCase() !== email.toLowerCase();
    const authTime = Number(req.user.auth_time);
    if (changed && (!Number.isFinite(authTime) || Date.now() / 1000 - authTime > 300)) return res.status(400).json({ message: 'Sign in again, then change your email within five minutes.' });
    try { await auth.updateUser(user.uid, { displayName, ...(changed ? { email, emailVerified: false } : {}) }); }
    catch (e) { if (!String(e.code).startsWith('auth/')) throw e; return res.status(400).json({ message: 'Firebase could not update the account. Check that the email is valid and unused.' }); }
    await db.collection('userSettings').doc(user.uid).set({ DisplayName: displayName, PhoneNumber: phoneNumber }, { merge: true });
    const profile = db.collection('users').doc(user.uid);
    if ((await profile.get()).exists) await profile.set({ displayName, email, phoneNumber }, { merge: true });
    if (changed) await auth.revokeRefreshTokens(user.uid);
    res.json({ emailChanged: changed });
  };
  const revokeSessions = async (req, res) => { await auth.revokeRefreshTokens(req.user.uid); res.sendStatus(204); };
  const getAccess = async (req, res) => {
    const profiles = await store.all('users'), accounts = []; let pageToken;
    do { const page = await auth.listUsers(1000, pageToken); accounts.push(...page.users); pageToken = page.pageToken; } while (pageToken);
    const row = (account, profile) => ({ uid: account?.uid || '', profileId: profile?.id || 0, name: profile?.fullName || account?.displayName || account?.email || account?.uid, email: profile?.email || account?.email || '', role: !account || (!isOwner(account) && !account.customClaims?.role) ? profile?.role || roleOf(account) : roleOf(account), permissions: account ? permissionsOf(account) : [], isOwner: account ? isOwner(account) : false });
    const rows = profiles.map(p => row(accounts.find(a => a.email?.toLowerCase() === p.email.toLowerCase()), p));
    for (const account of accounts) if (!profiles.some(p => p.email.toLowerCase() === account.email?.toLowerCase())) rows.push(row(account));
    res.json(rows);
  };
  const updateAccess = async (req, res) => {
    const role = text(req.body, 'role'), requested = value(req.body, 'permissions'), uid = text(req.body, 'uid'), profileId = value(req.body, 'profileId', 0);
    if (!['Admin', 'Teacher', 'Parent', 'Learner'].includes(role) || !Array.isArray(requested) || requested.some(p => !permissionCodes.includes(p)) || !Number.isInteger(profileId) || profileId < 0) return bad(res, req);
    const permissions = role === 'Admin' ? [...new Set([...requested, ...requested.filter(p => p.endsWith('.write')).map(p => p.slice(0, -6) + '.read')])] : [];
    if (role === 'Admin' && !permissions.includes('dashboard.read')) return res.status(400).json({ message: 'An administrator needs Dashboard read permission.' });
    if (req.user.accessManaged === true && permissions.some(p => !req.user.permissions?.includes(p))) return bad(res, req, 'Forbidden', 403);
    const profile = profileId > 0 ? await store.get('users', profileId) : null;
    if (!uid) {
      if (!profile) return bad(res, req, 'NotFound', 404);
      if (role === 'Admin' || requested.length) return res.status(400).json({ message: 'Create a matching Firebase sign-in account before granting application access.' });
    } else {
      if (uid === req.user.uid) return res.status(400).json({ message: 'You cannot change your own access.' });
      const target = await auth.getUser(uid);
      if (isOwner(target)) return res.status(400).json({ message: 'The bootstrap owner is protected. Change owner access only through trusted server setup.' });
      if (profile && profile.email.toLowerCase() !== target.email?.toLowerCase()) return bad(res, req);
      await auth.setCustomUserClaims(uid, { ...target.customClaims, admin: role === 'Admin', role, accessManaged: true, permissions });
      await auth.revokeRefreshTokens(uid);
    }
    if (profile) { profile.role = role; await store.save('users', profile); }
    if (uid) { const ref = db.collection('users').doc(uid); if ((await ref.get()).exists) await ref.set({ role: role.toLowerCase() }, { merge: true }); }
    await db.collection('adminAudit').add({ ActorUid: req.user.uid, TargetUid: uid, ProfileId: profileId, Role: role, Permissions: permissions, At: new Date() });
    res.sendStatus(204);
  };
  const exportBackup = async (req, res) => {
    const result = { formatVersion: 1, applicationName: 'KoosMatli', createdAt: new Date().toISOString() };
    const collections = { users: 'users', applications: 'applications', staff: 'staffMembers', classes: 'schoolClasses', subjects: 'subjects', schoolDates: 'schoolDates', calendarEvents: 'calendarEvents', documents: 'schoolDocuments' };
    for (const [key, collection] of Object.entries(collections)) result[key] = await store.all(collection);
    result.schoolDetails = await store.first('schoolDetails'); res.json(result);
  };
  return { getProfile, updateProfile, revokeSessions, getAccess, updateAccess, exportBackup };
}
module.exports = createFeaturesController;
