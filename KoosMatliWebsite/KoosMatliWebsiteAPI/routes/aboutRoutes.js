const express = require("express");

const router = express.Router();

const {
    getSchoolInformation,
    createSchoolInformation,
    updateSchoolInformation
} = require("../controllers/aboutController");

router.get("/", getSchoolInformation);

router.post("/", createSchoolInformation);

router.put("/:id", updateSchoolInformation);

module.exports = router;