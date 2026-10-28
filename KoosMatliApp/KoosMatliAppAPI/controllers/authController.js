const { auth, db } = require("../config/firebase");

const VALID_ROLES = ["parent", "teacher", "admin"];

// POST /api/auth/register-parent
// Public registration is restricted to parent accounts only.
async function registerParent(req, res) {
    let createdUser = null;

    try {
        const { displayName, email, password } = req.body;

        // Validate required fields.
        if (
            typeof displayName !== "string" ||
            typeof email !== "string" ||
            typeof password !== "string" ||
            !displayName.trim() ||
            !email.trim() ||
            !password
        ) {
            return res.status(400).json({
                success: false,
                message: "Name, email and password are required."
            });
        }

        if (password.length < 8) {
            return res.status(400).json({
                success: false,
                message: "Password must contain at least 8 characters."
            });
        }

        const cleanName = displayName.trim();
        const cleanEmail = email.trim().toLowerCase();

        // Create the Firebase Authentication account.
        createdUser = await auth.createUser({
            displayName: cleanName,
            email: cleanEmail,
            password
        });

        try {
            // Always create a parent profile in Firestore.
            await db.collection("users").doc(createdUser.uid).create({
                uid: createdUser.uid,
                displayName: cleanName,
                email: cleanEmail,
                role: "parent",
                isActive: true,
                createdAt: new Date().toISOString()
            });
        } catch (error) {
            // Roll back the Auth account if profile creation fails.
            await auth.deleteUser(createdUser.uid).catch((rollbackError) => {
                console.error(
                    "Registration rollback failed:",
                    rollbackError.message
                );
            });

            throw error;
        }

        return res.status(201).json({
            success: true,
            message: "Parent account created successfully. Please sign in.",
            user: {
                uid: createdUser.uid,
                displayName: cleanName,
                email: cleanEmail,
                role: "parent"
            }
        });

    } catch (error) {
        console.error(
            "Registration error:",
            error.code || error.message
        );

        if (error.code === "auth/email-already-exists") {
            return res.status(409).json({
                success: false,
                message: "An account with this email already exists."
            });
        }

        if (error.code === "auth/invalid-email") {
            return res.status(400).json({
                success: false,
                message: "Please provide a valid email address."
            });
        }

        if (
            error.code === "auth/invalid-password" ||
            error.code === "auth/weak-password"
        ) {
            return res.status(400).json({
                success: false,
                message: "The password does not meet requirements."
            });
        }

        return res.status(500).json({
            success: false,
            message: "Unable to register the account."
        });
    }
}

// POST /api/auth/login
async function login(req, res) {
    try {
        const { email, password } = req.body;

        if (
            typeof email !== "string" ||
            typeof password !== "string" ||
            !email.trim() ||
            !password
        ) {
            return res.status(400).json({
                success: false,
                message: "Email and password are required."
            });
        }

        const apiKey = process.env.FIREBASE_WEB_API_KEY;

        if (!apiKey) {
            console.error("FIREBASE_WEB_API_KEY is not configured.");

            return res.status(500).json({
                success: false,
                message: "Authentication is not configured."
            });
        }

        const firebaseResponse = await fetch(
            `https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=${apiKey}`,
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    email: email.trim().toLowerCase(),
                    password,
                    returnSecureToken: true
                })
            }
        );

        const data = await firebaseResponse.json();

        if (!firebaseResponse.ok) {
            const code = data?.error?.message;

            if (
                [
                    "EMAIL_NOT_FOUND",
                    "INVALID_PASSWORD",
                    "INVALID_LOGIN_CREDENTIALS",
                    "USER_DISABLED"
                ].includes(code)
            ) {
                return res.status(401).json({
                    success: false,
                    message: "Invalid email or password."
                });
            }

            console.error("Firebase login error:", code);

            return res.status(400).json({
                success: false,
                message: "Unable to sign in. Check your Firebase configuration."
            });
        }

        const userDoc = await db.collection("users").doc(data.localId).get();

        if (!userDoc.exists) {
            return res.status(403).json({
                success: false,
                message: "Your application profile has not been configured."
            });
        }

        const profile = userDoc.data();

        if (!VALID_ROLES.includes(profile.role)) {
            return res.status(403).json({
                success: false,
                message: "Your account has an invalid role. Contact an administrator."
            });
        }

        if (profile.isActive === false) {
            return res.status(403).json({
                success: false,
                message: "Your account has been deactivated."
            });
        }

        return res.status(200).json({
            success: true,
            message: "Login successful.",
            idToken: data.idToken,
            refreshToken: data.refreshToken,
            expiresIn: data.expiresIn,
            user: {
                uid: data.localId,
                email: profile.email || data.email,
                displayName: profile.displayName || "",
                role: profile.role,
                isActive: profile.isActive !== false
            }
        });
    } catch (error) {
        console.error("Login error:", error.message);

        return res.status(500).json({
            success: false,
            message: "An unexpected error occurred during login."
        });
    }
}

//POST /api/auth/forgot-password
async function forgotPassword(req, res) {
    try {
        const { email } = req.body;

        if (typeof email !== "string" || !email.trim()) {
            return res.status(400).json({
                success: false,
                message: "Email address is required."
            });
        }

        const cleanEmail = email.trim().toLowerCase();
        const apiKey = process.env.FIREBASE_WEB_API_KEY;

        if (!apiKey) {
            console.error("FIREBASE_WEB_API_KEY is not configured.");

            return res.status(500).json({
                success: false,
                message: "Password reset is currently unavailable."
            });
        }

        const firebaseResponse = await fetch(
            `https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=${apiKey}`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    requestType: "PASSWORD_RESET",
                    email: cleanEmail
                })
            }
        );

        const data = await firebaseResponse.json();

        if (!firebaseResponse.ok) {
            const errorCode = data?.error?.message;

            // Do not reveal whether an account exists.
            if (
                errorCode === "EMAIL_NOT_FOUND" ||
                errorCode === "INVALID_EMAIL"
            ) {
                return res.status(200).json({
                    success: true,
                    message:
                        "If an account exists for that email, password reset instructions will be sent."
                });
            }

            console.error("Password reset error:", errorCode);

            return res.status(400).json({
                success: false,
                message:
                    "Unable to process the password reset request. Please try again later."
            });
        }

        return res.status(200).json({
            success: true,
            message:
                "If an account exists for that email, password reset instructions will be sent."
        });
    } catch (error) {
        console.error("Forgot password error:", error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to process the password reset request. Please try again later."
        });
    }
}

// GET /api/auth/me
function getCurrentUser(req, res) {
    return res.status(200).json({
        success: true,
        user: req.user
    });
}

module.exports = {
    registerParent,
    login,
    forgotPassword,
    getCurrentUser
};
