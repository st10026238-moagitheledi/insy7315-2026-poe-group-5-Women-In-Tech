const { db } = require("../config/firebase");
const { FieldValue } = require("firebase-admin/firestore");

async function linkChildToParent(req, res) {
    try {
        const { parentUid } = req.params;

        const {
            firstName,
            lastName,
            grade,
            className,
            admissionNumber
        } = req.body;

        // Validate required fields.
        if (
            !parentUid ||
            typeof firstName !== "string" ||
            typeof lastName !== "string" ||
            typeof grade !== "string" ||
            typeof admissionNumber !== "string" ||
            !firstName.trim() ||
            !lastName.trim() ||
            !grade.trim() ||
            !admissionNumber.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Parent ID, first name, surname, grade and admission number are required."
            });
        }

        // Confirm the parent exists and has the correct role.
        const parentRef = db.collection("users").doc(parentUid);
        const parentSnapshot = await parentRef.get();

        if (!parentSnapshot.exists) {
            return res.status(404).json({
                success: false,
                message: "Parent account was not found."
            });
        }

        const parent = parentSnapshot.data();

        if (parent.role !== "parent" || parent.isActive === false) {
            return res.status(400).json({
                success: false,
                message: "The selected account is not an active parent account."
            });
        }

        // Generate one learner ID used in both collections.
        const learnerRef = db.collection("learners").doc();
        const childId = learnerRef.id;

        const learnerData = {
            firstName: firstName.trim(),
            lastName: lastName.trim(),
            grade: grade.trim(),
            className:
                typeof className === "string" ? className.trim() : "",
            admissionNumber: admissionNumber.trim(),
            isActive: true,
            createdAt: FieldValue.serverTimestamp(),
            createdBy: req.user.uid
        };

        // Create the learner and link them to the verified parent.
        const batch = db.batch();

        batch.set(learnerRef, learnerData);

        batch.set(
            parentRef.collection("children").doc(childId),
            {
                ...learnerData,
                linkedAt: FieldValue.serverTimestamp(),
                linkedBy: req.user.uid
            }
        );

        await batch.commit();

        return res.status(201).json({
            success: true,
            message: "Learner created and linked to the parent successfully.",
            child: {
                id: childId,
                ...learnerData,
                createdAt: null
            }
        });
    } catch (error) {
        console.error("Link learner to parent error:", error);

        return res.status(500).json({
            success: false,
            message: "Unable to link the learner to the parent."
        });
    }
}

module.exports = {
    linkChildToParent
};
