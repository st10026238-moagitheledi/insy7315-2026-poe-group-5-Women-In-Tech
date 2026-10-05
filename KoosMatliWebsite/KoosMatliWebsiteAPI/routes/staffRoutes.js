const express = require("express");

const router = express.Router();

const {
    getStaffMembers,
    getStaffMember,
    createStaffMember,
    updateStaffMember,
    deleteStaffMember
} = require("../controllers/staffController");

router.get("/", getStaffMembers);

router.get("/:id", getStaffMember);

router.post("/", createStaffMember);

router.put("/:id", updateStaffMember);

router.delete("/:id", deleteStaffMember);

module.exports = router;