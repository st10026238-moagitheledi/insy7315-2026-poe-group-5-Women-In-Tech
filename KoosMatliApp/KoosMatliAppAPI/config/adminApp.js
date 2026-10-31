'use strict';
const express = require('express');
const cors = require('cors');
const helmet = require('helmet');
const { randomUUID } = require('node:crypto');
const { Store } = require('./firestoreStore');
const { localization } = require('../middleware/localization');
const authenticate = require('../middleware/adminAuthenticate');
const errorHandler = require('../middleware/errorHandler');
const { bad } = require('../controllers/responseHelpers');
const systemRoutes = require('../routes/systemRoutes');
const settingsRoutes = require('../routes/settingsRoutes');
const accountRoutes = require('../routes/accountRoutes');
const adminRoutes = require('../routes/schoolAdminRoutes');
const featuresRoutes = require('../routes/featuresRoutes');
const documentRoutes = require('../routes/documentsRoutes');
function createApp({ auth, db, store = new Store(db), config, fetchImpl = fetch, logger = console }) {
  const dependencies = { auth, db, store, config, fetchImpl, uploadDir: config.uploadDir };
  const app = express();
  app.disable('x-powered-by');
  app.use(helmet());
  app.use(localization);
  app.use(cors({ origin: config.allowedOrigins || [], exposedHeaders: ['Content-Language'] }));
  app.use(express.json({ limit: '1mb' }));
  app.use((req, res, next) => { req.traceId = randomUUID(); next(); });

  app.use('/', systemRoutes());

  app.use('/api', authenticate(auth));
  app.use('/', settingsRoutes(dependencies));
  app.use('/', accountRoutes(dependencies));
  app.use('/api/admin/features', featuresRoutes(dependencies));
  app.use('/api/admin/documents', documentRoutes(dependencies));
  app.use('/api/admin', adminRoutes(store));

  app.use((req, res) => bad(res, req, 'NotFound', 404));
  app.use(errorHandler(logger));
  return app;
}

module.exports = { createApp };
