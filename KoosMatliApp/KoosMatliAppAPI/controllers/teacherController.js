const { auth, db } = require("../config/firebase");
const { FieldValue } = require("firebase-admin/firestore");


// Convert Firestore documents into normal JSON objects.
function documentsToArray(snapshot) {
    return snapshot.docs.map((doc) => ({
        ...doc.data(),
        id: doc.id
    }));
}
//(Express.js, 2026)

// Convert Firestore values into safe text.
function toText(value) {
    if (
        value === null ||
        value === undefined
    ) {
        return "";
    }

    if (typeof value === "string") {
        return value.trim();
    }

    if (
        typeof value === "number" ||
        typeof value === "boolean"
    ) {
        return String(value);
    }

    if (Array.isArray(value)) {
        return value
            .map(toText)
            .filter(Boolean)
            .join(", ");
    }

    if (typeof value === "object") {
        const preferredValue =
            value.name ??
            value.title ??
            value.label ??
            value.code ??
            value.id ??
            value.value;

        if (
            preferredValue !== undefined &&
            preferredValue !== value
        ) {
            return toText(
                preferredValue);
        }
    }

    return "";
}


// Convert class documents into the shape used by the app.
function normalizeClassDocument(doc) {
    const data =
        doc.data() || {};


    const learnerCountValue =
        data.learnerCount ??
        data.learnersCount ??
        data.numberOfLearners ??
        0; //(Express.js, 2026)


    const learnerCount =
        Number(
            learnerCountValue);


    return {
        id:
            String(doc.id),

        classId:
            toText(
                data.classId) ||
            String(doc.id),

        className:
            toText(
                data.className) ||
            toText(
                data.name) ||
            toText(
                data.code),

        name:
            toText(
                data.name),

        code:
            toText(
                data.code),

        grade:
            toText(
                data.grade),

        subject:
            toText(
                data.subject),

        schedule:
            toText(
                data.schedule),

        learnerCount:
            Number.isFinite(
                learnerCount)
                ? learnerCount
                : 0,

        isActive:
            data.isActive !== false
    };
}


// Get the signed-in teacher's Firestore profile.
async function getTeacherProfile(uid) {
    const teacherDoc = await db
        .collection("users")
        .doc(uid)
        .get();

    if (!teacherDoc.exists) {
        return null;
    }

    return {
        ...teacherDoc.data(),
        id: teacherDoc.id
    };
}


// Convert assigned class values into comparable text.
function getAssignmentValue(assignment) {
    if (typeof assignment === "string") {
        return assignment
            .trim()
            .toLowerCase();
    }

    if (assignment && typeof assignment === "object") {
        const value =
            assignment.id ||
            assignment.classId ||
            assignment.className ||
            assignment.name ||
            assignment.code ||
            ""; //(Express.js, 2026)

        return String(value)
            .trim()
            .toLowerCase();
    }

    return "";
}


// Check if a class belongs to the teacher.
function isAssignedClass(classItem, assignedClasses) {
    const assignedValues = assignedClasses
        .map(getAssignmentValue)
        .filter(Boolean);

    if (assignedValues.length === 0) {
        return false;
    }

    const possibleValues = [
        classItem.id,
        classItem.classId,
        classItem.className,
        classItem.name,
        classItem.code
    ]
        .filter(Boolean)
        .map((value) =>
            String(value)
                .trim()
                .toLowerCase()
        );

    return possibleValues.some((value) =>
        assignedValues.includes(value)
    );
}


//Teacher Dashboard

async function getDashboard(req, res) {
    try {
        const teacher =
            await getTeacherProfile(req.user.uid);

        if (!teacher) {
            return res.status(404).json({
                success: false,
                message: "Teacher profile not found."
            });
        }

        return res.status(200).json({
            success: true,
            message:
                "Teacher dashboard retrieved successfully.",

            teacher: {
                uid: req.user.uid,

                displayName:
                    teacher.displayName || "",

                email:
                    teacher.email ||
                    req.user.email ||
                    "",

                role:
                    teacher.role || "teacher",

                staffId:
                    teacher.staffId || null,

                assignedClasses:
                    teacher.assignedClasses || []
            }
        });
    } //(Express.js, 2026)
    catch (error) {
        console.error(
            "Teacher dashboard error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve the teacher dashboard."
        });
    }
}


//Teacher Profile

async function getProfile(req, res) {
    try {
        const teacher =
            await getTeacherProfile(req.user.uid);

        if (!teacher) {
            return res.status(404).json({
                success: false,
                message: "Teacher profile not found."
            });
        }

        return res.status(200).json({
            success: true,

            profile: {
                uid: req.user.uid,

                displayName:
                    teacher.displayName || "",

                email:
                    teacher.email ||
                    req.user.email ||
                    "",

                phoneNumber:
                    teacher.phoneNumber || "",

                staffId:
                    teacher.staffId || "",

                role:
                    teacher.role || "teacher",

                assignedClasses:
                    teacher.assignedClasses || [],

                photoUrl:
                    teacher.photoUrl || ""
            }
        });
    }
    catch (error) {
        console.error(
            "Teacher profile error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve your profile."
        });
    }
}


async function updateProfile(req, res) {
    try {
        const teacherRef =
            db.collection("users")
                .doc(req.user.uid);

        const teacherDoc =
            await teacherRef.get();

        if (!teacherDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Teacher profile not found."
            });
        }

        const updates = {};


        // Update display name.
        if (typeof req.body.displayName === "string") {
            const displayName =
                req.body.displayName.trim();

            if (
                !displayName ||
                displayName.length > 100
            ) {
                return res.status(400).json({
                    success: false,
                    message:
                        "Name must contain 1 to 100 characters."
                });
            }

            updates.displayName = displayName;
        }


        // Update phone number.
        if (typeof req.body.phoneNumber === "string") {
            const phoneNumber =
                req.body.phoneNumber.trim();

            if (phoneNumber.length > 25) {
                return res.status(400).json({
                    success: false,
                    message:
                        "Phone number is too long."
                });
            }

            updates.phoneNumber = phoneNumber;
        }


        if (Object.keys(updates).length === 0) {
            return res.status(400).json({
                success: false,
                message:
                    "Provide a valid displayName or phoneNumber."
            });
        }


        // Update Firestore.
        await teacherRef.update(updates);


        // Keep Firebase Authentication name in sync.
        if (updates.displayName) {
            await auth.updateUser(
                req.user.uid,
                {
                    displayName:
                        updates.displayName
                }
            );
        }


        const updatedTeacher =
            await teacherRef.get();

        const profile =
            updatedTeacher.data();


        return res.status(200).json({
            success: true,

            message:
                "Teacher profile updated successfully.",

            profile: {
                uid: req.user.uid,

                displayName:
                    profile.displayName || "",

                email:
                    profile.email ||
                    req.user.email ||
                    "",

                phoneNumber:
                    profile.phoneNumber || "",

                staffId:
                    profile.staffId || "",

                role:
                    profile.role || "teacher",

                assignedClasses:
                    profile.assignedClasses || [],

                photoUrl:
                    profile.photoUrl || ""
            }
        }); //(Express.js, 2026)
    }
    catch (error) {
        console.error(
            "Update teacher profile error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to update your profile."
        });
    }
}


//Teacher Classes

async function getClasses(req, res) {
    try {
        const teacher =
            await getTeacherProfile(req.user.uid);

        if (!teacher) {
            return res.status(404).json({
                success: false,
                message: "Teacher profile not found."
            });
        }


        const snapshot =
            await db
                .collection("schoolClasses")
                .get();


        const allClasses =
            snapshot.docs
                .map(
                    normalizeClassDocument)
                .filter((item) =>
                    item.isActive !== false
                );


        const assignedClasses =
            Array.isArray(teacher.assignedClasses)
                ? teacher.assignedClasses
                : [];


        const myClasses =
            allClasses.filter((classItem) =>
                isAssignedClass(
                    classItem,
                    assignedClasses
                )
            );


        return res.status(200).json({
            success: true,
            assignedClasses,
            myClasses,
            allClasses //(Express.js, 2026)
        });
    }
    catch (error) {
        console.error(
            "Teacher classes error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve classes."
        });
    }
}


//All Learners

async function getLearners(req, res) {
    try {
        const snapshot =
            await db
                .collection("learners")
                .get();


        let learners =
            documentsToArray(snapshot)
                .filter((learner) =>
                    learner.isActive !== false
                );


        const requestedClass =
            typeof req.query.className === "string"
                ? req.query.className
                    .trim()
                    .toLowerCase()
                : "";


        if (requestedClass) {
            learners =
                learners.filter((learner) =>
                    String(
                        learner.className || ""
                    )
                        .trim()
                        .toLowerCase() ===
                    requestedClass
                );
        } //(Express.js, 2026)


        learners.sort((a, b) => {
            const surnameA =
                String(a.lastName || "");

            const surnameB =
                String(b.lastName || "");

            return surnameA.localeCompare(
                surnameB
            );
        });


        return res.status(200).json({
            success: true,
            learners
        });
    }
    catch (error) {
        console.error(
            "Teacher learners error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve learners."
        });
    }
}


//Class Learners

async function getClassLearners(req, res) {
    try {
        const classId =
            String(
                req.params.classId || ""
            ).trim();


        if (!classId) {
            return res.status(400).json({
                success: false,
                message:
                    "A class ID is required."
            });
        }


        const classDoc =
            await db
                .collection("schoolClasses")
                .doc(classId)
                .get();


        const classData =
            classDoc.exists
                ? normalizeClassDocument(
                    classDoc)
                : null;


        const identifiers = [
            classId,
            classData?.className,
            classData?.name,
            classData?.code
        ]
            .filter(Boolean)
            .map((value) =>
                String(value)
                    .trim()
                    .toLowerCase()
            );


        const snapshot =
            await db
                .collection("learners")
                .get();


        const learners =
            documentsToArray(snapshot)
                .filter((learner) => {

                    if (
                        learner.isActive === false
                    ) {
                        return false;
                    }


                    const learnerValues = [
                        learner.classId,
                        learner.className
                    ]
                        .filter(Boolean)
                        .map((value) =>
                            String(value)
                                .trim()
                                .toLowerCase()
                        );


                    return learnerValues.some(
                        (value) =>
                            identifiers.includes(
                                value
                            )
                    );
                }); //(Express.js, 2026)


        return res.status(200).json({
            success: true,
            class: classData,
            learners
        });
    }
    catch (error) {
        console.error(
            "Class learners error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve learners for this class."
        });
    }
}


//Add Learner

async function createLearner(req, res) {
    try {
        const {
            firstName,
            lastName,
            grade,
            className,
            admissionNumber
        } = req.body;


        if (
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
                    "First name, surname, grade and admission number are required."
            });
        }


        // Prevent duplicate admission numbers.
        const existingSnapshot =
            await db
                .collection("learners")
                .where(
                    "admissionNumber",
                    "==",
                    admissionNumber.trim()
                )
                .limit(1)
                .get();


        if (!existingSnapshot.empty) {
            return res.status(409).json({
                success: false,
                message:
                    "A learner with this admission number already exists."
            });
        } //(Express.js, 2026)


        const learnerRef =
            db.collection("learners")
                .doc();


        const learner = {
            firstName:
                firstName.trim(),

            lastName:
                lastName.trim(),

            grade:
                grade.trim(),

            className:
                typeof className === "string"
                    ? className.trim()
                    : "",

            admissionNumber:
                admissionNumber.trim(),

            isActive: true,

            createdAt:
                FieldValue.serverTimestamp(),

            createdBy:
                req.user.uid
        };


        await learnerRef.set(learner);


        return res.status(201).json({
            success: true,

            message:
                "Learner added successfully.",

            learner: {
                id: learnerRef.id,

                firstName:
                    learner.firstName,

                lastName:
                    learner.lastName,

                grade:
                    learner.grade,

                className:
                    learner.className,

                admissionNumber:
                    learner.admissionNumber,

                isActive: true,

                createdAt:
                    new Date().toISOString(),

                createdBy:
                    req.user.uid
            }
        });
    }
    catch (error) {
        console.error(
            "Create learner error:",
            error.message
        ); //(Express.js, 2026)

        return res.status(500).json({
            success: false,
            message:
                "Unable to add the learner."
        });
    }
}


//Announcements

async function getAnnouncements(req, res) {
    try {
        const snapshot =
            await db
                .collection("announcements")
                .get();


        const announcements =
            documentsToArray(snapshot)
                .filter((item) =>
                    item.status !== "deleted"
                );


        return res.status(200).json({
            success: true,
            announcements
        });
    }
    catch (error) {
        console.error(
            "Teacher announcements error:",
            error.message
        );

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve announcements."
        });
    }
}


//Create Announcement

async function createAnnouncement(req, res) {
    try {
        const {
            title, //(Express.js, 2026)
            message,
            category,
            audience,
            grade,
            subject,
            eventDate
        } = req.body;


        if (
            typeof title !== "string" ||
            typeof message !== "string" ||
            !title.trim() ||
            !message.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Announcement title and message are required."
            });
        }


        const announcementRef =
            db.collection("announcements")
                .doc();


        const createdAt =
            new Date().toISOString();


        const announcement = {
            title:
                title.trim(),

            message:
                message.trim(),

            category:
                typeof category === "string"
                    ? category.trim()
                    : "General",

            audience:
                typeof audience === "string"
                    ? audience.trim()
                    : "All Classes",

            grade:
                typeof grade === "string"
                    ? grade.trim()
                    : "",

            subject:
                typeof subject === "string"
                    ? subject.trim()
                    : "",

            eventDate:
                typeof eventDate === "string"
                    ? eventDate
                    : "",

            status:
                "published",

            isPublished:
                true,

            createdAt,

            createdBy:
                req.user.uid,

            authorName:
                req.user.displayName ||
                "Teacher"
        };


        await announcementRef.set(
            announcement);


        return res.status(201).json({
            success: true,

            message:
                "Announcement published successfully.",

            announcement: {
                id:
                    announcementRef.id,

                ...announcement
            }
        });
    }
    catch (error) {
        console.error(
            "Create announcement error:", //(Express.js, 2026)
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to publish the announcement."
        });
    }
}


// Update Learner

async function updateLearner(req, res) {
    try {
        const learnerId =
            String(req.params.learnerId || "").trim();

        if (!learnerId) {
            return res.status(400).json({
                success: false,
                message: "A learner ID is required."
            });
        }

        const learnerRef =
            db.collection("learners")
                .doc(learnerId);

        const learnerDoc =
            await learnerRef.get();

        if (!learnerDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Learner not found."
            });
        }

        const {
            firstName,
            lastName,
            grade,
            className,
            admissionNumber
        } = req.body;


        if (
            typeof firstName !== "string" ||
            typeof lastName !== "string" ||
            typeof admissionNumber !== "string" ||
            !firstName.trim() ||
            !lastName.trim() ||
            !admissionNumber.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "First name, surname and admission number are required."
            });
        }


        const duplicateSnapshot =
            await db
                .collection("learners")
                .where(
                    "admissionNumber",
                    "==",
                    admissionNumber.trim())
                .get();


        const duplicate =
            duplicateSnapshot.docs.find(
                (doc) => doc.id !== learnerId);


        if (duplicate) {
            return res.status(409).json({
                success: false,
                message:
                    "Another learner already uses this admission number."
            });
        }


        const updates = {
            firstName:
                firstName.trim(),

            lastName:
                lastName.trim(),

            admissionNumber:
                admissionNumber.trim(),

            updatedAt:
                FieldValue.serverTimestamp(),

            updatedBy:
                req.user.uid
        };


        if (typeof grade === "string") {
            updates.grade =
                grade.trim();
        }


        if (typeof className === "string") {
            updates.className =
                className.trim();
        }


        await learnerRef.update(
            updates);


        const updatedDoc =
            await learnerRef.get();


        return res.status(200).json({
            success: true,

            message:
                "Learner updated successfully.",

            learner: {
                id:
                    updatedDoc.id,

                ...updatedDoc.data()
            } //(Express.js, 2026)
        });
    }
    catch (error) {
        console.error(
            "Update learner error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to update the learner."
        });
    }
}


// Remove Learner

async function deleteLearner(req, res) {
    try {
        const learnerId =
            String(req.params.learnerId || "").trim();

        if (!learnerId) {
            return res.status(400).json({
                success: false,
                message:
                    "A learner ID is required."
            });
        }


        const learnerRef =
            db.collection("learners")
                .doc(learnerId);


        const learnerDoc =
            await learnerRef.get();


        if (!learnerDoc.exists) {
            return res.status(404).json({
                success: false,
                message:
                    "Learner not found."
            });
        }

        await learnerRef.update({
            isActive: false,
            deletedAt:
                FieldValue.serverTimestamp(),

            deletedBy:
                req.user.uid
        });


        return res.status(200).json({
            success: true,
            message:
                "Learner removed successfully."
        });
    }
    catch (error) {
        console.error(
            "Delete learner error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to remove the learner."
        });
    }
} //(Express.js, 2026)


// Update Announcement

async function updateAnnouncement(req, res) {
    try {
        const announcementId =
            String(
                req.params.announcementId || ""
            ).trim();


        if (!announcementId) {
            return res.status(400).json({
                success: false,
                message:
                    "An announcement ID is required."
            });
        }


        const announcementRef =
            db.collection("announcements")
                .doc(announcementId);


        const announcementDoc =
            await announcementRef.get();


        if (!announcementDoc.exists) {
            return res.status(404).json({
                success: false,
                message:
                    "Announcement not found."
            });
        }


        const {
            title,
            message,
            category,
            audience,
            grade,
            subject,
            eventDate
        } = req.body;


        if (
            typeof title !== "string" ||
            typeof message !== "string" ||
            !title.trim() ||
            !message.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Announcement title and message are required."
            });
        }


        const updates = {
            title:
                title.trim(),

            message:
                message.trim(),

            category:
                typeof category === "string"
                    ? category.trim()
                    : "General",

            audience:
                typeof audience === "string"
                    ? audience.trim()
                    : "All Classes",

            grade:
                typeof grade === "string"
                    ? grade.trim()
                    : "",

            subject:
                typeof subject === "string"
                    ? subject.trim()
                    : "",

            eventDate:
                typeof eventDate === "string"
                    ? eventDate
                    : "",

            updatedAt:
                new Date().toISOString(),

            updatedBy:
                req.user.uid
        };


        await announcementRef.update(
            updates);


        const updatedDoc =
            await announcementRef.get(); //(Express.js, 2026)


        return res.status(200).json({
            success: true,

            message:
                "Announcement updated successfully.",

            announcement: {
                id:
                    updatedDoc.id,

                ...updatedDoc.data()
            }
        });
    }
    catch (error) {
        console.error(
            "Update announcement error:",
            error.message
        );


        return res.status(500).json({
            success: false,
            message:
                "Unable to update the announcement."
        });
    }
}


//Delete Announcement

async function deleteAnnouncement(req, res) {
    try {
        const announcementId =
            String(
                req.params.announcementId || ""
            ).trim();


        if (!announcementId) {
            return res.status(400).json({
                success: false,
                message:
                    "An announcement ID is required."
            });
        }


        const announcementRef =
            db.collection("announcements")
                .doc(announcementId);


        const announcementDoc =
            await announcementRef.get();


        if (!announcementDoc.exists) {
            return res.status(404).json({
                success: false,
                message:
                    "Announcement not found."
            });
        } //(Express.js, 2026)


        await announcementRef.update({
            status:
                "deleted",

            isPublished:
                false,

            deletedAt:
                new Date().toISOString(),

            deletedBy:
                req.user.uid
        });


        return res.status(200).json({
            success: true,
            message:
                "Announcement deleted successfully."
        });
    }
    catch (error) {
        console.error(
            "Delete announcement error:",
            error.message
        );


        return res.status(500).json({
            success: false,
            message:
                "Unable to delete the announcement."
        });
    }
}


// Homework

async function getHomework(req, res) {
    try {
        const grade =
            typeof req.query.grade === "string"
                ? req.query.grade.trim().toLowerCase()
                : "";

        const subject =
            typeof req.query.subject === "string"
                ? req.query.subject.trim().toLowerCase()
                : "";


        const snapshot =
            await db
                .collection("homework")
                .get();


        let homework =
            documentsToArray(snapshot)
                .filter(
                    item =>
                        item.status !== "deleted"); //(Express.js, 2026)


        if (grade) {
            homework =
                homework.filter(
                    item =>
                        String(item.grade || "")
                            .trim()
                            .toLowerCase() ===
                        grade);
        }


        if (subject) {
            homework =
                homework.filter(
                    item =>
                        String(item.subject || "")
                            .trim()
                            .toLowerCase() ===
                        subject);
        }


        homework.sort(
            (a, b) =>
                String(a.dueDate || "")
                    .localeCompare(
                        String(b.dueDate || "")));


        return res.status(200).json({
            success: true,
            homework
        });
    }
    catch (error) {
        console.error(
            "Teacher homework error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve homework."
        });
    }
}


async function createHomework(req, res) {
    try {
        const {
            title,
            description,
            dueDate,
            status,
            grade,
            subject,
            attachmentName
        } = req.body; //(Express.js, 2026)


        if (
            typeof title !== "string" ||
            typeof description !== "string" ||
            typeof dueDate !== "string" ||
            typeof grade !== "string" ||
            typeof subject !== "string" ||
            !title.trim() ||
            !description.trim() ||
            !dueDate.trim() ||
            !grade.trim() ||
            !subject.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Title, instructions, due date, grade and subject are required."
            });
        }


        const homeworkRef =
            db.collection("homework")
                .doc();


        const homework = {
            title:
                title.trim(),

            description:
                description.trim(),

            dueDate:
                dueDate.trim(),

            status:
                typeof status === "string"
                    ? status.trim()
                    : "Assigned",

            grade:
                grade.trim(),

            subject:
                subject.trim(),

            attachmentName:
                typeof attachmentName === "string"
                    ? attachmentName.trim()
                    : "",

            createdAt:
                new Date().toISOString(),

            createdBy:
                req.user.uid //(Express.js, 2026)
        };


        await homeworkRef.set(
            homework);


        return res.status(201).json({
            success: true,

            message:
                "Homework added successfully.",

            homework: {
                id:
                    homeworkRef.id,

                ...homework
            }
        });
    }
    catch (error) {
        console.error(
            "Create homework error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to add homework."
        });
    }
}


// Schedule

async function getSchedule(req, res) {
    try {
        const snapshot =
            await db
                .collection("teacherSchedule")
                .where(
                    "createdBy",
                    "==",
                    req.user.uid)
                .get();


        const schedule =
            documentsToArray(snapshot)
                .filter(
                    item =>
                        item.isActive !== false)
                .sort(
                    (a, b) => {
                        const dateCompare =
                            String(a.date || "")
                                .localeCompare(
                                    String(b.date || ""));

                        if (dateCompare !== 0) {
                            return dateCompare;
                        }

                        return String(a.startTime || "")
                            .localeCompare(
                                String(b.startTime || ""));
                    });


        return res.status(200).json({
            success: true,
            schedule
        });
    }
    catch (error) {
        console.error(
            "Teacher schedule error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve the schedule."
        }); //(Express.js, 2026)
    }
}


async function createScheduleItem(req, res) {
    try {
        const {
            title,
            description,
            eventType,
            date,
            startTime,
            endTime,
            grade,
            subject,
            location
        } = req.body;


        if (
            typeof title !== "string" ||
            typeof eventType !== "string" ||
            typeof date !== "string" ||
            typeof startTime !== "string" ||
            typeof endTime !== "string" ||
            typeof location !== "string" ||
            !title.trim() ||
            !eventType.trim() ||
            !date.trim() ||
            !startTime.trim() ||
            !endTime.trim() ||
            !location.trim()
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Title, event type, date, times and location are required."
            });
        } //(Express.js, 2026)


        const scheduleRef =
            db.collection("teacherSchedule")
                .doc();


        const scheduleItem = {
            title:
                title.trim(),

            description:
                typeof description === "string"
                    ? description.trim()
                    : "",

            eventType:
                eventType.trim(),

            date:
                date.trim(),

            startTime:
                startTime.trim(),

            endTime:
                endTime.trim(),

            grade:
                typeof grade === "string"
                    ? grade.trim()
                    : "",

            subject:
                typeof subject === "string"
                    ? subject.trim()
                    : "",

            location:
                location.trim(),

            isActive:
                true,

            createdAt:
                new Date().toISOString(),

            createdBy:
                req.user.uid
        };


        await scheduleRef.set(
            scheduleItem);


        return res.status(201).json({
            success: true,

            message:
                "Schedule item added successfully.",

            scheduleItem: {
                id:
                    scheduleRef.id,

                ...scheduleItem
            }
        });
    }
    catch (error) {
        console.error(
            "Create schedule error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to add the schedule item."
        });
    } //(Express.js, 2026)
}


async function updateScheduleItem(req, res) {
    try {
        const scheduleId =
            String(
                req.params.scheduleId || ""
            ).trim();


        if (!scheduleId) {
            return res.status(400).json({
                success: false,
                message:
                    "A schedule item ID is required."
            });
        }


        const scheduleRef =
            db.collection("teacherSchedule")
                .doc(scheduleId);


        const scheduleDoc =
            await scheduleRef.get();


        if (!scheduleDoc.exists) {
            return res.status(404).json({
                success: false,
                message:
                    "Schedule item not found."
            });
        }


        const existing =
            scheduleDoc.data();


        if (existing.createdBy !==
            req.user.uid) {
            return res.status(403).json({
                success: false,
                message:
                    "You cannot edit this schedule item."
            });
        }


        const {
            title,
            description,
            eventType,
            date,
            startTime,
            endTime,
            grade,
            subject,
            location
        } = req.body; //(Express.js, 2026)


        const updates = {
            title:
                String(title || "").trim(),

            description:
                String(description || "").trim(),

            eventType:
                String(eventType || "").trim(),

            date:
                String(date || "").trim(),

            startTime:
                String(startTime || "").trim(),

            endTime:
                String(endTime || "").trim(),

            grade:
                String(grade || "").trim(),

            subject:
                String(subject || "").trim(),

            location:
                String(location || "").trim(),

            updatedAt:
                new Date().toISOString(),

            updatedBy:
                req.user.uid
        };


        if (
            !updates.title ||
            !updates.eventType ||
            !updates.date ||
            !updates.startTime ||
            !updates.endTime ||
            !updates.location
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Title, event type, date, times and location are required."
            });
        }


        await scheduleRef.update(
            updates); //(Express.js, 2026)
 

        const updatedDoc =
            await scheduleRef.get();


        return res.status(200).json({
            success: true,

            message:
                "Schedule item updated successfully.",

            scheduleItem: {
                id:
                    updatedDoc.id,

                ...updatedDoc.data()
            }
        });
    }
    catch (error) {
        console.error(
            "Update schedule error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to update the schedule item."
        });
    }
}


async function deleteScheduleItem(req, res) {
    try {
        const scheduleId =
            String(
                req.params.scheduleId || ""
            ).trim();


        const scheduleRef =
            db.collection("teacherSchedule")
                .doc(scheduleId);


        const scheduleDoc =
            await scheduleRef.get();


        if (!scheduleDoc.exists) {
            return res.status(404).json({
                success: false,
                message:
                    "Schedule item not found."
            });
        } //(Express.js, 2026)


        const existing =
            scheduleDoc.data();


        if (existing.createdBy !==
            req.user.uid) {
            return res.status(403).json({
                success: false,
                message:
                    "You cannot delete this schedule item."
            });
        }


        await scheduleRef.update({
            isActive:
                false,

            deletedAt:
                new Date().toISOString(),

            deletedBy:
                req.user.uid
        });


        return res.status(200).json({
            success: true,
            message:
                "Schedule item deleted successfully."
        }); //(Express.js, 2026)
    }
    catch (error) {
        console.error(
            "Delete schedule error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to delete the schedule item."
        });
    }
}


// Attendance

async function getAttendance(req, res) {
    try {
        const classId =
            String(
                req.params.classId || ""
            ).trim();

        const date =
            typeof req.query.date === "string"
                ? req.query.date.trim()
                : "";


        if (!classId ||
            !date) {
            return res.status(400).json({
                success: false,
                message:
                    "Class ID and date are required."
            });
        }


        const attendanceId =
            `${req.user.uid}_${classId}_${date}`;


        const attendanceDoc =
            await db
                .collection("attendance")
                .doc(attendanceId)
                .get();


        if (!attendanceDoc.exists) {
            return res.status(200).json({
                success: true,
                records: []
            });
        }


        return res.status(200).json({
            success: true,
            records:
                attendanceDoc.data().records || []
        });
    }
    catch (error) {
        console.error(
            "Get attendance error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to retrieve attendance."
        }); //(Express.js, 2026)
    }
}


async function saveAttendance(req, res) {
    try {
        const {
            classId,
            grade,
            subject,
            date,
            records
        } = req.body;


        if (
            typeof classId !== "string" ||
            typeof date !== "string" ||
            !classId.trim() ||
            !date.trim() ||
            !Array.isArray(records) ||
            records.length === 0
        ) {
            return res.status(400).json({
                success: false,
                message:
                    "Class, date and attendance records are required."
            });
        } //(Express.js, 2026)


        const cleanRecords =
            records
                .filter(
                    record =>
                        record &&
                        typeof record.learnerId === "string" &&
                        (
                            record.status === "Present" ||
                            record.status === "Absent"
                        ))
                .map(
                    record => ({
                        learnerId:
                            record.learnerId.trim(),

                        status:
                            record.status
                    }));


        if (cleanRecords.length === 0) {
            return res.status(400).json({
                success: false,
                message:
                    "No valid attendance records were supplied."
            });
        }


        const attendanceId =
            `${req.user.uid}_${classId.trim()}_${date.trim()}`;


        await db
            .collection("attendance")
            .doc(attendanceId)
            .set(
                {
                    teacherId:
                        req.user.uid,

                    classId:
                        classId.trim(), //(Express.js, 2026)

                    grade:
                        typeof grade === "string"
                            ? grade.trim()
                            : "",

                    subject:
                        typeof subject === "string"
                            ? subject.trim()
                            : "",

                    date:
                        date.trim(),

                    records:
                        cleanRecords,

                    updatedAt:
                        new Date().toISOString()
                },
                {
                    merge: true
                });


        return res.status(200).json({
            success: true,
            message:
                "Attendance saved successfully."
        });
    }
    catch (error) {
        console.error(
            "Save attendance error:",
            error.message);

        return res.status(500).json({
            success: false,
            message:
                "Unable to save attendance."
        });
    }
}



//Exports

module.exports = {
    getDashboard,
    getProfile,
    updateProfile,
    getClasses,
    getLearners,
    getClassLearners,
    createLearner,
    updateLearner,
    deleteLearner,
    getAnnouncements,
    createAnnouncement,
    updateAnnouncement,
    deleteAnnouncement,
    getHomework,
    createHomework,
    getSchedule,
    createScheduleItem,
    updateScheduleItem,
    deleteScheduleItem,
    getAttendance,
    saveAttendance

}; //(Express.js, 2026)


//Reference List:
//1. Express.js, 2026. Routing. [online] Available at: https://expressjs.com/en/guide/routing/ [Accessed 31 October 2026].
