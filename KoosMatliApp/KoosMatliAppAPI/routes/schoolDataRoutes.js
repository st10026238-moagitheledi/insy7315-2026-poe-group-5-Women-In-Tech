const express = require("express");

const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");

const {
    linkChildToParent
} = require("../controllers/schoolDataController");

const router = express.Router();

// Every route in this file requires an authenticated admin.
router.use(authenticate, authorize("admin"));

router.post(
    "/parents/:parentUid/children",
    linkChildToParent
);

module.exports = router;
