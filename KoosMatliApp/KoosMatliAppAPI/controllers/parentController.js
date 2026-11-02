const { db } = require("../config/firebase");


function documentsToArray(snapshot) {
    return snapshot.docs.map((doc) => ({
        id: doc.id,
        ...doc.data()
    }));
}


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

    return {
        id: childDoc.id,
        ...childDoc.data()
    };
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
            const dateA = new Date(a.createdAt || a.date || 0).getTime();
            const dateB = new Date(b.createdAt || b.date || 0).getTime();
            return dateB - dateA;
        })
        .slice(0, limit);
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

            if (!item.date) {
                return true;
            }

            const eventDate = new Date(item.date);
            return !Number.isNaN(eventDate.getTime()) &&
                eventDate >= today;
        })
        .sort((a, b) =>
            new Date(a.date || 0) - new Date(b.date || 0)
        )
        .slice(0, limit);
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
                displayName: parent.displayName || req.user.displayName,
                email: parent.email || req.user.email,
                role: req.user.role
            },
            children: documentsToArray(childrenSnapshot),
            announcements,
            upcomingEvents: events
        });
    } catch (error) {
        console.error("Parent dashboard error:", error.message);

        return res.status(500).json({
            success: false,
            message: "Unable to retrieve the parent dashboard."
        });
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

        return res.json({
            success: true,
            children: documentsToArray(snapshot)
        });
    } catch (error) {
        console.error("Get children error:", error.message);
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
        console.error("Announcements error:", error.message);
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
            timetables: documentsToArray(snapshot)
        });
    } catch (error) {
        console.error("Timetable error:", error.message);
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
            homework: documentsToArray(snapshot)
        });
    } catch (error) {
        console.error("Homework error:", error.message);
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
        console.error("Events error:", error.message);
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
            academicRecords: documentsToArray(snapshot)
        });
    } catch (error) {
        console.error("Academic information error:", error.message);
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
                ? { id: accountDoc.id, ...accountDoc.data() }
                : null,
            message: accountDoc.exists
                ? "Fee account retrieved."
                : "No fee account has been created for this learner yet."
        });
    } catch (error) {
        console.error("School fees error:", error.message);
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
                new Date(b.paymentDate || b.createdAt || 0) -
                new Date(a.paymentDate || a.createdAt || 0)
            );

        return res.json({
            success: true,
            child,
            transactions
        });
    } catch (error) {
        console.error("Fee transactions error:", error.message);
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
                new Date(b.paymentDate || b.createdAt || 0) -
                new Date(a.paymentDate || a.createdAt || 0)
            );

        return res.json({
            success: true,
            child,
            feeAccount: accountDoc.exists
                ? { id: accountDoc.id, ...accountDoc.data() }
                : null,
            transactions
        });
    } catch (error) {
        console.error("Fee statement error:", error.message);
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
        console.error("Parent profile error:", error.message);
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

        // Only allow these fields to be edited by the parent.
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
        console.error("Update parent profile error:", error.message);
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
