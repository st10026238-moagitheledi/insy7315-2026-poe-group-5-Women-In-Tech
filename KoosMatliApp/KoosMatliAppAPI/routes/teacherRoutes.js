const express = require("express");

const authenticate =
    require("../middleware/authenticate");

const authorize =
    require("../middleware/authorize"); //(Express.js, 2026)

const {
    getDashboard,
    getProfile,
    updateProfile,
    getClasses,
    getLearners,
    getClassLearners,
    createLearner,
    updateLearner,
    deleteLearner,
    getAnnouncements,
    createAnnouncement,
    updateAnnouncement,
    deleteAnnouncement,
    getHomework,
    createHomework,
    getSchedule,
    createScheduleItem,
    updateScheduleItem,
    deleteScheduleItem,
    getAttendance,
    saveAttendance
} = require("../controllers/teacherController");


const router = express.Router();


router.use(
    authenticate,
    authorize("teacher") //(Express.js, 2026)
);


router.get(
    "/dashboard",
    getDashboard
);


router.get(
    "/profile",
    getProfile
);

router.patch(
    "/profile",
    updateProfile
);


router.get(
    "/classes",
    getClasses
);


router.get(
    "/learners",
    getLearners
);

router.get(
    "/classes/:classId/learners",
    getClassLearners //(Express.js, 2026)
);

router.post(
    "/learners",
    createLearner
);

router.patch(
    "/learners/:learnerId",
    updateLearner
);

router.delete(
    "/learners/:learnerId",
    deleteLearner
);


router.get(
    "/announcements",
    getAnnouncements
);

router.post(
    "/announcements",
    createAnnouncement //(Express.js, 2026)
);

router.patch(
    "/announcements/:announcementId",
    updateAnnouncement
);

router.delete(
    "/announcements/:announcementId",
    deleteAnnouncement
);


router.get(
    "/homework",
    getHomework
);

router.post(
    "/homework",
    createHomework
);


router.get(
    "/schedule",
    getSchedule
);

router.post(
    "/schedule",
    createScheduleItem
);

router.patch(
    "/schedule/:scheduleId",
    updateScheduleItem
); //(Express.js, 2026)

router.delete(
    "/schedule/:scheduleId",
    deleteScheduleItem
);


router.get(
    "/classes/:classId/attendance",
    getAttendance
);

router.post(
    "/attendance",
    saveAttendance
);


module.exports = router; //(Express.js, 2026)

//Reference List:
//1. Express.js, 2026. Routing. [online] Available at: https://expressjs.com/en/guide/routing/ [Accessed 31 October 2026].
