const express = require("express");
const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");
const { getDashboard } = require("../controllers/teacherController");

const router = express.Router();

router.use(authenticate, authorize("teacher"));

router.get("/dashboard", getDashboard);

module.exports = router;
