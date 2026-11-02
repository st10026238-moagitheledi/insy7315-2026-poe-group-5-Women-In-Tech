'use strict';
const express = require('express');
const multer = require('multer');
const { admin, permission } = require('../middleware/adminPermissions');
const { bad } = require('../controllers/responseHelpers');
const createDocumentsController = require('../controllers/documentsController');
function documentRoutes(dependencies) {
  const r = express.Router(); //(ExpressJS, 2026b).
  const controller = createDocumentsController(dependencies);
  r.use(admin);
  r.param('id', (req, res, next, id) => /^\d+$/.test(id) && Number(id) > 0 && Number(id) <= 2147483647 ? next() : bad(res, req, 'NotFound', 404));
  const upload = multer({ storage: multer.memoryStorage(), limits: { fileSize: 10 * 1024 * 1024, files: 1, fields: 1, fieldSize: 2048, parts: 2 } }); //(ExpressJS, 2026a).
  r.get('/', permission('documents.read'), controller.getDocuments);
  r.get('/:id', permission('documents.read'), controller.getDocument); //(Open Web Application Security Project [OWASP], 2026).
  r.post('/', permission('documents.write'), upload.single('file'), controller.uploadDocument);
  r.get('/:id/file', permission('documents.read'), controller.downloadDocument);
  return r;
}
module.exports = documentRoutes;

/*Reference List

ExpressJS Community, "Multer Middleware Documentation for Node.js," ExpressJS Ecosystem, 2026. [Online]. Available: https://github.com/expressjs/multer

Open Web Application Security Project (OWASP), "Unrestricted File Upload and Denial of Service Defenses," OWASP Foundation, 2026. [Online]. Available: https://owasp.org/www-community/vulnerabilities/Unrestricted_File_Upload

ExpressJS Documentation, "Using router.param for Parameter Pre-conditions," ExpressJS, 2026. [Online]. Available: https://expressjs.com/en/5x/api.html#router.param
*/