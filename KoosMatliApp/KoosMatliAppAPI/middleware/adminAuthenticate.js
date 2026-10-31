'use strict';

function authenticate(auth) {
  return async (req, res, next) => {
    const token = /^Bearer\s+(.+)$/i.exec(req.headers.authorization || '')?.[1]?.trim();
    if (!token) return res.set('WWW-Authenticate', 'Bearer').status(401).json({ message: req.t('Unauthorized') });
    try { req.user = await auth.verifyIdToken(token, true); }
    catch (e) {
      if (!String(e.code || '').startsWith('auth/')) return next(e);
      return res.set('WWW-Authenticate', 'Bearer').status(401).json({ message: req.t('Unauthorized') });
    }
    next();
  };
}

module.exports = authenticate;
