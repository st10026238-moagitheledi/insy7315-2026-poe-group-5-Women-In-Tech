
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
            admissionNumber,
            dateOfBirth,
            gender,
            learnerContactNumber,
            learnerEmail,
            address,
            parentName,
            parentContactNumber,
            parentEmail,
            guardianName,
            guardianContactNumber,
            guardianEmail,
            emergencyContactName,
            emergencyContactNumber,
            medicalInformation,
            allergies
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

        // Confirm that the parent exists and is active.
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
                message:
                    "The selected account is not an active parent account."
            });
        }

        // Check whether the admission number is already registered.
        const existingLearner = await db
            .collection("learners")
            .where("admissionNumber", "==", admissionNumber.trim())
            .limit(1)
            .get();

        if (!existingLearner.empty) {
            return res.status(409).json({
                success: false,
                message: "This admission number is already registered."
            });
        }

        // Validate optional fields and normalize empty strings to "".
        const optionalString = (value, fieldName) => {
            if (value == null || value === "") return "";

            if (typeof value !== "string") {
                throw new Error(`${fieldName} must be a string.`);
            }

            return value.trim();
        };

        const learnerData = {
            firstName: firstName.trim(),
            lastName: lastName.trim(),
            fullName: `${firstName.trim()} ${lastName.trim()}`,
            grade: grade.trim(),
            className: optionalString(className, "Class"),
            admissionNumber: admissionNumber.trim(),

            dateOfBirth: optionalString(dateOfBirth, "Date of birth"),
            gender: optionalString(gender, "Gender"),

            learnerContactNumber: optionalString(
                learnerContactNumber, "Learner contact number"
            ),
            learnerEmail: optionalString(learnerEmail, "Learner email"),
            address: optionalString(address, "Address"),

            parentName: optionalString(parentName, "Parent name"),
            parentContactNumber: optionalString(
                parentContactNumber, "Parent contact number"
            ),
            parentEmail: optionalString(parentEmail, "Parent email"),

            guardianName: optionalString(guardianName, "Guardian name"),
            guardianContactNumber: optionalString(
                guardianContactNumber, "Guardian contact number"
            ),
            guardianEmail: optionalString(guardianEmail, "Guardian email"),

            emergencyContactName: optionalString(
                emergencyContactName, "Emergency contact name"
            ),
            emergencyContactNumber: optionalString(
                emergencyContactNumber, "Emergency contact number"
            ),

            medicalInformation: optionalString(
                medicalInformation, "Medical information"
            ),
            allergies: optionalString(allergies, "Allergies"),

            status: "Active",
            isActive: true,
            createdAt: FieldValue.serverTimestamp(),
            createdBy: req.user.uid
        };

        // Create the learner and link the same profile to the parent.
        const learnerRef = db.collection("learners").doc();
        const childId = learnerRef.id;

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

        if (error.message?.includes("must be a string")) {
            return res.status(400).json({
                success: false,
                message: error.message
            });
        }

        return res.status(500).json({
            success: false,
            message: "Unable to link the learner to the parent."
        });
    }
}

// DELETE /api/school-data/parents/:parentUid/children/:childId
const deleteLinkedChild = async (req, res) => {
    try {
        const { parentUid, childId } = req.params;

        if (!parentUid || !childId) {
            return res.status(400).json({
                success: false,
                message: "Parent ID and child ID are required."
            });
        }

        const parentRef = db.collection("users").doc(parentUid);
        const linkRef = parentRef
            .collection("children")
            .doc(childId);

        const [parentDoc, linkDoc] = await Promise.all([
            parentRef.get(),
            linkRef.get()
        ]);

        if (!parentDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Parent account not found."
            });
        }

        if (parentDoc.data().role !== "parent") {
            return res.status(400).json({
                success: false,
                message: "The specified account is not a parent."
            });
        }

        if (!linkDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "This child is not linked to the parent."
            });
        }

        await linkRef.delete();

        return res.status(200).json({
            success: true,
            message: "Child successfully unlinked from the parent."
        });
    } catch (error) {
        console.error("Error unlinking child:", error);

        return res.status(500).json({
            success: false,
            message: "Failed to unlink child.",
            error: error.message
        });
    }
};

module.exports = {
    linkChildToParent,
    deleteLinkedChild
};
