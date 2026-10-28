const express = require("express");
const authenticate = require("../middleware/authenticate");

const router = express.Router();

// GET /api/auth/me
// Returns the authenticated user's profile.
router.get("/me", authenticate, (req, res) => {
    return res.status(200).json({
        success: true,
        user: req.user
    });
});

module.exports = router;
