'use strict';
const emailValid = s => typeof s === 'string' && /^[^\s@<>]+@[^\s@<>]+\.[^\s@<>]+$/.test(s);
function bad(res, req, message = 'InvalidInput', status = 400) { return res.status(status).json(req.t(message)); }
function recordOr404(req, res, item) { return item ? res.json(item) : bad(res, req, 'NotFound', 404); }
module.exports = { emailValid, bad, recordOr404 };
