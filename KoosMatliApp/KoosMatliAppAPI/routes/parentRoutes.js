const express = require("express");
const authenticate = require("../middleware/authenticate");
const authorize = require("../middleware/authorize");

const {
    getDashboard,
    getChildren,
    getAnnouncements,
    getTimetable,
    getHomework,
    getEvents,
    getAcademicInformation,
    getFees,
    getFeeTransactions,
    getFeeStatement,
    getProfile,
    updateProfile
} = require("../controllers/parentController");

const router = express.Router();

// Every route requires a valid token and the parent role.
router.use(authenticate, authorize("parent"));

// 1. Dashboard
router.get("/dashboard", getDashboard);

// 2. Children
router.get("/children", getChildren);

// 3. Announcements
router.get("/announcements", getAnnouncements);

// 4. Timetables
router.get(
    "/children/:childId/timetable",
    getTimetable
);

// 5. Homework and learning resources
router.get(
    "/children/:childId/homework",
    getHomework
);

// 6. School calendar and events
router.get("/events", getEvents);

// 7. Academic information
router.get(
    "/children/:childId/academic",
    getAcademicInformation
);

// 8. School fees and payments
router.get(
    "/children/:childId/fees",
    getFees
);

router.get(
    "/children/:childId/fees/transactions",
    getFeeTransactions
);

router.get(
    "/children/:childId/fees/statement",
    getFeeStatement
);

// 9. Parent profile and settings
router.get("/profile", getProfile);
router.patch("/profile", updateProfile);

module.exports = router;
