const { auth, db } = require("../config/firebase");

async function authenticate(req, res, next) {
    try {
        const authHeader = req.headers.authorization;

        if (!authHeader || !authHeader.startsWith("Bearer ")) {
            return res.status(401).json({
                success: false,
                message: "Authentication token is required."
            });
        }

        const token = authHeader.split(" ")[1];

        const decodedToken = await auth.verifyIdToken(token);
        const userDoc = await db.collection("users").doc(decodedToken.uid).get();

        if (!userDoc.exists) {
            return res.status(403).json({
                success: false,
                message: "User profile has not been configured."
            });
        }

        const userData = userDoc.data();

        if (userData.isActive === false) {
            return res.status(403).json({
                success: false,
                message: "Your account has been deactivated."
            });
        }

        req.user = {
            uid: decodedToken.uid,
            email: decodedToken.email || null,
            role: userData.role,
            displayName: userData.displayName || "",
            isActive: userData.isActive !== false
        };

        next();
    } catch (error) {
        console.error("Authentication error:", error.message);

        return res.status(401).json({
            success: false,
            message: "Invalid or expired authentication token."
        });
    }
}

module.exports = authenticate;
