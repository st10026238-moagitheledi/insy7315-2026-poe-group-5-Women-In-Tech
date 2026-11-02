const express = require("express");

const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");

const {
    linkChildToParent,
    deleteLinkedChild
} = require("../controllers/schoolDataController");

const router = express.Router();

// Protect every route: authenticated administrators only.
router.use(authenticate, authorize("admin"));

// Create a child and link them to a parent.
router.post(
    "/parents/:parentUid/children",
    linkChildToParent
);

// Unlink a child from a parent's account.
router.delete(
    "/parents/:parentUid/children/:childId",
    deleteLinkedChild
);

module.exports = router;
