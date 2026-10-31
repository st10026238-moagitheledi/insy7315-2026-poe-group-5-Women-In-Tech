const { auth, db } = require("../config/firebase");

async function getDashboard(req, res) {
    try {
        const [usersSnapshot, applicationsSnapshot, announcementsSnapshot] =
            await Promise.all([
                db.collection("users").get(),
                db.collection("applications").get(),
                db.collection("announcements").get()
            ]);

        const users = usersSnapshot.docs.map((doc) => doc.data());

        const countRole = (role) =>
            users.filter((user) => user.role === role).length;

        return res.status(200).json({
            success: true,
            message: "Admin dashboard retrieved successfully.",
            admin: {
                uid: req.user.uid,
                displayName: req.user.displayName,
                email: req.user.email,
                role: req.user.role
            },
            statistics: {
                parents: countRole("parent"),
                teachers: countRole("teacher"),
                admins: countRole("admin"),
                totalUsers: users.length,
                applications: applicationsSnapshot.size,
                announcements: announcementsSnapshot.size
            }
        });
    } catch (error) {
        console.error("Admin dashboard error:", error.message);

        return res.status(500).json({
            success: false,
            message: "Unable to retrieve the admin dashboard."
        });
    }
}

async function createStaffAccount(req, res) {
    let createdUser = null;

    try {
        const {
            displayName,
            email,
            password,
            role,
            staffId,
            assignedClasses
        } = req.body;

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
                message: "Name, email and temporary password are required."
            });
        }

        if (password.length < 8) {
            return res.status(400).json({
                success: false,
                message: "Password must contain at least 8 characters."
            });
        }

        if (!["teacher", "admin"].includes(role)) {
            return res.status(400).json({
                success: false,
                message: "Only teacher or admin accounts can be created here."
            });
        }

        if (
            role === "teacher" &&
            assignedClasses !== undefined &&
            !Array.isArray(assignedClasses)
        ) {
            return res.status(400).json({
                success: false,
                message: "assignedClasses must be an array."
            });
        }

        const cleanName = displayName.trim();
        const cleanEmail = email.trim().toLowerCase();

        createdUser = await auth.createUser({
            displayName: cleanName,
            email: cleanEmail,
            password,
            emailVerified: false,
            disabled: false
        });

        const profile = {
            uid: createdUser.uid,
            displayName: cleanName,
            email: cleanEmail,
            role,
            isActive: true,
            createdAt: new Date().toISOString(),
            createdBy: req.user.uid,
            mustChangePassword: true
        };

        if (typeof staffId === "string" && staffId.trim()) {
            profile.staffId = staffId.trim();
        }

        if (role === "teacher") {
            profile.assignedClasses = Array.isArray(assignedClasses)
                ? assignedClasses
                : [];
        }

        try {
            await db.collection("users").doc(createdUser.uid).create(profile);
        } catch (error) {
            await auth.deleteUser(createdUser.uid).catch((rollbackError) => {
                console.error("Staff account rollback failed:", rollbackError.message);
            });

            throw error;
        }

        return res.status(201).json({
            success: true,
            message: `${role === "teacher" ? "Teacher" : "Admin"} account created successfully.`,
            user: {
                uid: createdUser.uid,
                displayName: cleanName,
                email: cleanEmail,
                role,
                staffId: profile.staffId || null,
                mustChangePassword: true
            }
        });
    } catch (error) {
        console.error("Staff account creation error:", error.code || error.message);

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
                message: "The temporary password does not meet requirements."
            });
        }

        return res.status(500).json({
            success: false,
            message: "Unable to create the staff account."
        });
    }
}

module.exports = {
    getDashboard,
    createStaffAccount
};
