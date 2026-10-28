const express = require("express");
const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");
const { getDashboard } = require("../controllers/parentController");

const router = express.Router();

router.use(authenticate, authorize("parent"));

router.get("/dashboard", getDashboard);

module.exports = router;
