const { auth, db } = require("../config/firebase");

async function authenticate(req, res, next) {
    try {
        const authHeader = req.headers.authorization;

        if (!authHeader || !authHeader.startsWith("Bearer ")) {
            return res.status(401).json({
                success: false,
                message: "A valid authentication token is required."
            });
        }

        const token = authHeader.substring(7).trim();

        if (!token) {
            return res.status(401).json({
                success: false,
                message: "A valid authentication token is required."
            });
        }

        // true also checks whether the token has been revoked.
        const decodedToken = await auth.verifyIdToken(token, true);

        const userDoc = await db
            .collection("users")
            .doc(decodedToken.uid)
            .get();

        if (!userDoc.exists) {
            return res.status(403).json({
                success: false,
                message: "Your user profile has not been configured."
            });
        }

        const profile = userDoc.data();

        if (profile.isActive === false) {
            return res.status(403).json({
                success: false,
                message: "Your account has been deactivated."
            });
        }

        if (!["parent", "teacher", "admin"].includes(profile.role)) {
            return res.status(403).json({
                success: false,
                message: "Your account has an invalid role."
            });
        }

        req.user = {
            uid: decodedToken.uid,
            email: profile.email || decodedToken.email || null,
            displayName: profile.displayName || "",
            role: profile.role,
            isActive: profile.isActive !== false,
            admin: decodedToken.admin === true,
            accessManaged: decodedToken.accessManaged === true,
            permissions: Array.isArray(decodedToken.permissions) ? decodedToken.permissions : [],
            auth_time: decodedToken.auth_time
        };

        return next();
    } catch (error) {
        console.error("Authentication error:", error.message);

        return res.status(401).json({
            success: false,
            message: "Invalid, expired or revoked authentication token."
        });
    }
}

module.exports = authenticate;
