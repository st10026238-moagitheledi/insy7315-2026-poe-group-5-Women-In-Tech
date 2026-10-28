const express = require("express");
const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");
const {
    getDashboard,
    createStaffAccount
} = require("../controllers/adminController");

const router = express.Router();

router.use(authenticate, authorize("admin"));

router.get("/dashboard", getDashboard);
router.post("/users", createStaffAccount);

module.exports = router;
