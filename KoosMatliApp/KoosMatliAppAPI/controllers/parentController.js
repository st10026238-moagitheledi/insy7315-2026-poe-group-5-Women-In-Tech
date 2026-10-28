const { db } = require("../config/firebase");

async function getDashboard(req, res) {
    try {
        const parentRef = db.collection("users").doc(req.user.uid);
        const parentDoc = await parentRef.get();

        if (!parentDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Parent profile not found."
            });
        }

        const childrenSnapshot = await parentRef
            .collection("children")
            .get();

        const children = childrenSnapshot.docs.map((doc) => ({
            id: doc.id,
            ...doc.data()
        }));

        return res.status(200).json({
            success: true,
            parent: {
                uid: req.user.uid,
                displayName: req.user.displayName,
                email: req.user.email,
                role: req.user.role
            },
            children
        });
    } catch (error) {
        console.error("Parent dashboard error:", error.message);

        return res.status(500).json({
            success: false,
            message: "Unable to retrieve the parent dashboard."
        });
    }
}

module.exports = { getDashboard };
