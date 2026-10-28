const express = require("express");
const {
    registerParent,
    login,
    forgotPassword,
    getCurrentUser
} = require("../controllers/authController");
const authenticate = require("../middleware/authenticate");

const router = express.Router();

router.post("/register-parent", registerParent);
router.post("/login", login);
router.post("/forgot-password", forgotPassword);
router.get("/me", authenticate, getCurrentUser);

module.exports = router;
