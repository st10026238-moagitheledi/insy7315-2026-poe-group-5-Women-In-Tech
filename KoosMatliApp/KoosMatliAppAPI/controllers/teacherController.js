const { db } = require("../config/firebase");

async function getDashboard(req, res) {
    try {
        const teacherDoc = await db
            .collection("users")
            .doc(req.user.uid)
            .get();

        if (!teacherDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Teacher profile not found."
            });
        }

        const teacher = teacherDoc.data();

        return res.status(200).json({
            success: true,
            message: "Teacher dashboard retrieved successfully.",
            teacher: {
                uid: req.user.uid,
                displayName: teacher.displayName || "",
                email: teacher.email || req.user.email,
                role: teacher.role,
                staffId: teacher.staffId || null,
                assignedClasses: teacher.assignedClasses || []
            }
        });
    } catch (error) {
        console.error("Teacher dashboard error:", error.message);

        return res.status(500).json({
            success: false,
            message: "Unable to retrieve the teacher dashboard."
        });
    }
}

module.exports = { getDashboard };
