'use strict';

function errorHandler(logger = console) {
  return (error, req, res, next) => {
    if (res.headersSent) return next(error);
    const code = error.code;
    let status = [4, 14, 'unavailable', 'deadline-exceeded'].includes(code) ? 503 : 500;
    let message = 'ServerError';
    if (error.type === 'entity.parse.failed') { status = 400; message = 'InvalidInput'; }
    if (error.type === 'entity.too.large' || code === 'LIMIT_FILE_SIZE') { status = 413; message = 'Choose a PDF, DOCX, TXT, PNG or JPG file up to 10 MB.'; }
    if (String(code).startsWith('LIMIT_') && code !== 'LIMIT_FILE_SIZE') { status = 400; message = 'InvalidInput'; }
    if (code === 'auth/user-not-found') { status = 404; message = 'NotFound'; }
    // Never log request tokens, credential files, or Firebase REST URLs/API keys.
    if (status >= 500) logger.error(`API failure ${req.traceId || ''}: ${typeof code === 'string' || typeof code === 'number' ? code : error.name}`);
    res.status(status).json({ message: req.t ? req.t(message) : 'The request could not be completed.', ...(status >= 500 ? { traceId: req.traceId } : {}) });
  };
}
module.exports = errorHandler;
