const { db } = require("../config/firebase");

// Convert Firestore documents into JSON-friendly objects.
function documentsToArray(snapshot) {
    return snapshot.docs.map((doc) => ({
        id: doc.id,
        ...doc.data()
    }));
}

// Convert Firestore Timestamp values into ISO strings for JSON responses.
function serializeValue(value) {
    if (value == null) return value;

    if (value instanceof Date) {
        return value.toISOString();
    }

    if (
        typeof value.toDate === "function" &&
        typeof value.toMillis === "function"
    ) {
        return value.toDate().toISOString();
    }

    if (Array.isArray(value)) {
        return value.map(serializeValue);
    }

    if (typeof value === "object") {
        const result = {};

        for (const [key, item] of Object.entries(value)) {
            result[key] = serializeValue(item);
        }

        return result;
    }

    return value;
}

// Return a consistent learner profile.
function serializeChild(id, data = {}) {
    return serializeValue({
        id,
        firstName: data.firstName || "",
        lastName: data.lastName || "",
        fullName:
            data.fullName ||
            [data.firstName, data.lastName]
                .filter(Boolean)
                .join(" "),
        grade: data.grade || data.gradeName || "",
        className:
            data.className ||
            data.classroom ||
            data.classCode ||
            "",
        admissionNumber:
            data.admissionNumber ||
            data.learnerNumber ||
            data.studentNumber ||
            "",
        dateOfBirth: data.dateOfBirth || data.dob || "",
        gender: data.gender || data.sex || "",
        learnerContactNumber: data.learnerContactNumber || "",
        learnerEmail: data.learnerEmail || "",
        address: data.address || "",
        parentName: data.parentName || "",
        parentContactNumber:
            data.parentContactNumber ||
            data.parentPhoneNumber ||
            "",
        parentEmail: data.parentEmail || "",
        guardianName: data.guardianName || "",
        guardianContactNumber:
            data.guardianContactNumber ||
            data.guardianPhoneNumber ||
            "",
        guardianEmail: data.guardianEmail || "",
        emergencyContactName: data.emergencyContactName || "",
        emergencyContactNumber: data.emergencyContactNumber || "",
        medicalInformation: data.medicalInformation || "",
        allergies: data.allergies || "",
        status: data.status || data.learnerStatus ||
            (data.isActive === false ? "Inactive" : "Active"),
        isActive: data.isActive !== false,
        createdAt: data.createdAt || null,
        linkedAt: data.linkedAt || null
    });
}

// Return the signed-in parent's profile.
async function getParentProfile(uid) {
    const parentDoc = await db.collection("users").doc(uid).get();

    if (!parentDoc.exists) {
        return null;
    }

    return parentDoc.data();
}

// Check that the requested learner belongs to this parent.
async function getLinkedChild(parentUid, childId) {
    const childDoc = await db
        .collection("users")
        .doc(parentUid)
        .collection("children")
        .doc(childId)
        .get();

    if (!childDoc.exists) {
        return null;
    }

    return serializeChild(childDoc.id, childDoc.data());
}

// Return published announcements, newest first.
async function getPublishedAnnouncements(limit = 50) {
    const snapshot = await db.collection("announcements").get();

    return documentsToArray(snapshot)
        .filter((item) =>
            item.isPublished !== false &&
            item.status !== "draft"
        )
        .sort((a, b) => {
            const dateA = toDate(a.createdAt || a.date)?.getTime() || 0;
            const dateB = toDate(b.createdAt || b.date)?.getTime() || 0;
            return dateB - dateA;
        })
        .slice(0, limit)
        .map((item) => serializeValue(item));
}

// Convert common date values to JavaScript Date.
function toDate(value) {
    if (!value) return null;

    if (value instanceof Date) return value;

    if (typeof value.toDate === "function") {
        return value.toDate();
    }

    if (typeof value === "object" && value.seconds != null) {
        return new Date(value.seconds * 1000);
    }

    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
}

// Return upcoming school events.
async function getUpcomingEvents(limit = 50) {
    const snapshot = await db.collection("events").get();
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return documentsToArray(snapshot)
        .filter((item) => {
            if (item.isPublished === false || item.status === "draft") {
                return false;
            }

            if (!item.date) return true;

            const eventDate = toDate(item.date);

            return eventDate && eventDate >= today;
        })
        .sort((a, b) =>
            (toDate(a.date)?.getTime() || 0) -
            (toDate(b.date)?.getTime() || 0)
        )
        .slice(0, limit)
        .map((item) => serializeValue(item));
}

// 1. PARENT DASHBOARD
async function getDashboard(req, res) {
    try {
        const parent = await getParentProfile(req.user.uid);

        if (!parent) {
            return res.status(404).json({
                success: false,
                message: "Parent profile not found."
            });
        }

        const childrenSnapshot = await db
            .collection("users")
            .doc(req.user.uid)
            .collection("children")
            .get();

        const [announcements, events] = await Promise.all([
            getPublishedAnnouncements(5),
            getUpcomingEvents(5)
        ]);

        return res.json({
            success: true,
            parent: {
                uid: req.user.uid,
                displayName: parent.displayName || req.user.displayName || "",
                email: parent.email || req.user.email || "",
                role: req.user.role
            },
            children: childrenSnapshot.docs.map((doc) =>
                serializeChild(doc.id, doc.data())
            ),
            announcements,
            upcomingEvents: events
        });
    } catch (error) {
        console.error("Parent dashboard error:", error);

        return serverError(res, "Unable to retrieve the parent dashboard.");
    }
}

// 2. MY CHILDREN
async function getChildren(req, res) {
    try {
        const snapshot = await db
            .collection("users")
            .doc(req.user.uid)
            .collection("children")
            .get();

        const children = snapshot.docs.map((doc) =>
            serializeChild(doc.id, doc.data())
        );

        return res.json({
            success: true,
            count: children.length,
            children
        });
    } catch (error) {
        console.error("Get children error:", error);

        return serverError(res, "Unable to retrieve your children.");
    }
}

// 3. ANNOUNCEMENTS
async function getAnnouncements(req, res) {
    try {
        const announcements = await getPublishedAnnouncements();

        return res.json({
            success: true,
            announcements
        });
    } catch (error) {
        console.error("Announcements error:", error);

        return serverError(res, "Unable to retrieve announcements.");
    }
}

// Shared helper for learner-specific endpoints.
async function requireLinkedChild(req, res) {
    const childId = String(req.params.childId || "").trim();

    if (!childId) {
        res.status(400).json({
            success: false,
            message: "A learner ID is required."
        });
        return null;
    }

    const child = await getLinkedChild(req.user.uid, childId);

    if (!child) {
        res.status(404).json({
            success: false,
            message: "Learner not found or not linked to your account."
        });
        return null;
    }

    return child;
}

// 4. TIMETABLES
async function getTimetable(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const snapshot = await db
            .collection("timetables")
            .where("childId", "==", child.id)
            .get();

        return res.json({
            success: true,
            child,
            timetables: documentsToArray(snapshot).map(serializeValue)
        });
    } catch (error) {
        console.error("Timetable error:", error);
        return serverError(res, "Unable to retrieve the timetable.");
    }
}

// 5. HOMEWORK AND LEARNING RESOURCES
async function getHomework(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const snapshot = await db
            .collection("homework")
            .where("childId", "==", child.id)
            .get();

        return res.json({
            success: true,
            child,
            homework: documentsToArray(snapshot).map(serializeValue)
        });
    } catch (error) {
        console.error("Homework error:", error);
        return serverError(res, "Unable to retrieve homework.");
    }
}

// 6. SCHOOL CALENDAR AND EVENTS
async function getEvents(req, res) {
    try {
        const events = await getUpcomingEvents();

        return res.json({
            success: true,
            events
        });
    } catch (error) {
        console.error("Events error:", error);
        return serverError(res, "Unable to retrieve school events.");
    }
}

// 7. ACADEMIC INFORMATION
async function getAcademicInformation(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const snapshot = await db
            .collection("academicRecords")
            .where("childId", "==", child.id)
            .get();

        return res.json({
            success: true,
            child,
            academicRecords: documentsToArray(snapshot).map(serializeValue)
        });
    } catch (error) {
        console.error("Academic information error:", error);
        return serverError(res, "Unable to retrieve academic information.");
    }
}

// 8. SCHOOL FEES
async function getFees(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const accountDoc = await db
            .collection("feeAccounts")
            .doc(child.id)
            .get();

        return res.json({
            success: true,
            child,
            feeAccount: accountDoc.exists
                ? serializeValue({
                    id: accountDoc.id,
                    ...accountDoc.data()
                })
                : null,
            message: accountDoc.exists
                ? "Fee account retrieved."
                : "No fee account has been created for this learner yet."
        });
    } catch (error) {
        console.error("School fees error:", error);
        return serverError(res, "Unable to retrieve school fees.");
    }
}

async function getFeeTransactions(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const snapshot = await db
            .collection("feeTransactions")
            .where("childId", "==", child.id)
            .get();

        const transactions = documentsToArray(snapshot)
            .sort((a, b) =>
                (toDate(b.paymentDate || b.createdAt)?.getTime() || 0) -
                (toDate(a.paymentDate || a.createdAt)?.getTime() || 0)
            )
            .map(serializeValue);

        return res.json({
            success: true,
            child,
            transactions
        });
    } catch (error) {
        console.error("Fee transactions error:", error);
        return serverError(res, "Unable to retrieve payment history.");
    }
}

async function getFeeStatement(req, res) {
    try {
        const child = await requireLinkedChild(req, res);
        if (!child) return;

        const accountDoc = await db
            .collection("feeAccounts")
            .doc(child.id)
            .get();

        const snapshot = await db
            .collection("feeTransactions")
            .where("childId", "==", child.id)
            .get();

        const transactions = documentsToArray(snapshot)
            .sort((a, b) =>
                (toDate(b.paymentDate || b.createdAt)?.getTime() || 0) -
                (toDate(a.paymentDate || a.createdAt)?.getTime() || 0)
            )
            .map(serializeValue);

        return res.json({
            success: true,
            child,
            feeAccount: accountDoc.exists
                ? serializeValue({
                    id: accountDoc.id,
                    ...accountDoc.data()
                })
                : null,
            transactions
        });
    } catch (error) {
        console.error("Fee statement error:", error);
        return serverError(res, "Unable to retrieve the fee statement.");
    }
}

// 9. PARENT PROFILE
async function getProfile(req, res) {
    try {
        const parent = await getParentProfile(req.user.uid);

        if (!parent) {
            return res.status(404).json({
                success: false,
                message: "Parent profile not found."
            });
        }

        return res.json({
            success: true,
            profile: {
                uid: req.user.uid,
                displayName: parent.displayName || "",
                email: parent.email || req.user.email || "",
                phoneNumber: parent.phoneNumber || "",
                role: req.user.role
            }
        });
    } catch (error) {
        console.error("Parent profile error:", error);
        return serverError(res, "Unable to retrieve your profile.");
    }
}

async function updateProfile(req, res) {
    try {
        const parentRef = db.collection("users").doc(req.user.uid);
        const parentDoc = await parentRef.get();

        if (!parentDoc.exists) {
            return res.status(404).json({
                success: false,
                message: "Parent profile not found."
            });
        }

        const updates = {};

        if (typeof req.body.displayName === "string") {
            const displayName = req.body.displayName.trim();

            if (!displayName || displayName.length > 100) {
                return res.status(400).json({
                    success: false,
                    message: "Name must contain 1 to 100 characters."
                });
            }

            updates.displayName = displayName;
        }

        if (typeof req.body.phoneNumber === "string") {
            const phoneNumber = req.body.phoneNumber.trim();

            if (phoneNumber.length > 25) {
                return res.status(400).json({
                    success: false,
                    message: "Phone number is too long."
                });
            }

            updates.phoneNumber = phoneNumber;
        }

        if (Object.keys(updates).length === 0) {
            return res.status(400).json({
                success: false,
                message: "Provide a valid displayName or phoneNumber."
            });
        }

        await parentRef.update(updates);

        return res.json({
            success: true,
            message: "Profile updated successfully.",
            profile: {
                uid: req.user.uid,
                displayName:
                    updates.displayName ?? parentDoc.data().displayName ?? "",
                email: parentDoc.data().email || req.user.email || "",
                phoneNumber:
                    updates.phoneNumber ?? parentDoc.data().phoneNumber ?? "",
                role: req.user.role
            }
        });
    } catch (error) {
        console.error("Update parent profile error:", error);
        return serverError(res, "Unable to update your profile.");
    }
}

function serverError(res, message) {
    return res.status(500).json({
        success: false,
        message
    });
}

module.exports = {
    getDashboard,
    getChildren,
    getAnnouncements,
    getTimetable,
    getHomework,
    getEvents,
    getAcademicInformation,
    getFees,
    getFeeTransactions,
    getFeeStatement,
    getProfile,
    updateProfile
};
