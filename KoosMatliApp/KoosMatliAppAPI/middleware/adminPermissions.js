'use strict';

const permissionCodes = ['dashboard.read', 'users.read', 'users.write', 'applications.read', 'applications.write', 'staff.read', 'school.read', 'school.write', 'documents.read', 'documents.write', 'backups.export', 'roles.manage'];
const isAdmin = c => c.admin === true || c.role === 'Admin';
const isOwner = account => account?.customClaims?.admin === true && account.customClaims?.accessManaged !== true;
const roleOf = account => isOwner(account) ? 'Admin' : account?.customClaims?.role || 'Learner';
const permissionsOf = account => isOwner(account) ? [...permissionCodes] : Array.isArray(account.customClaims?.permissions) ? account.customClaims.permissions : [];
function admin(req, res, next) { return isAdmin(req.user) ? next() : res.status(403).json({ message: req.t('Forbidden') }); }
function permission(code) {
  return (req, res, next) => req.user.accessManaged === true && !req.user.permissions?.includes(code)
    ? res.status(403).json({ message: req.t('Forbidden') }) : next();
}
module.exports = { admin, permission, permissionCodes, isAdmin, isOwner, roleOf, permissionsOf };
