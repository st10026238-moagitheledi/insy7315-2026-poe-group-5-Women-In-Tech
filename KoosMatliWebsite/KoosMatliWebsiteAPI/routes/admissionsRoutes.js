const express = require("express");

const router = express.Router();

const {
    getAdmissions,
    getAdmission,
    createAdmission,
    updateAdmission,
    deleteAdmission
} = require("../controllers/admissionsController");

router.get("/", getAdmissions);

router.get("/:id", getAdmission);

router.post("/", createAdmission);

router.put("/:id", updateAdmission);

router.delete("/:id", deleteAdmission);

module.exports = router;