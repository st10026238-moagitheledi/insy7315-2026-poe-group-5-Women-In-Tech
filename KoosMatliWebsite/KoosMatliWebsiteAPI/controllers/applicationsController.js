const { db } = require("../config/firebase");
const { Timestamp } = require("firebase-admin/firestore");

const collectionName = "applications";

const getNextId = async () => {
    const snapshot = await db
        .collection(collectionName)
        .get();

    const ids = snapshot.docs
        .map(doc => Number(doc.data().Id) || 0);

    return ids.length > 0
        ? Math.max(...ids) + 1
        : 1;
};

const mapApplication = (document) => {
    const data = document.data();

    return {
        Id: data.Id || 0,
        ParentFirstName: data.ParentFirstName || "",
        ParentLastName: data.ParentLastName || "",
        ParentEmail: data.ParentEmail || "",
        ParentPhone: data.ParentPhone || "",
        RelationshipToLearner:
            data.RelationshipToLearner || "",
        LearnerFirstName:
            data.LearnerFirstName || "",
        LearnerLastName:
            data.LearnerLastName || "",
        LearnerDateOfBirth:
            data.LearnerDateOfBirth
                ? data.LearnerDateOfBirth.toDate()
                : null,
        GradeApplyingFor:
            data.GradeApplyingFor || "",
        PreviousSchool:
            data.PreviousSchool || "",
        HomeAddress:
            data.HomeAddress || "",
        AdditionalInformation:
            data.AdditionalInformation || "",
        Status:
            data.Status || "Pending",
        DateSubmitted:
            data.DateSubmitted
                ? data.DateSubmitted.toDate()
                : null
    };
};

const getApplications = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const applications = snapshot.docs
            .map(mapApplication)
            .sort(
                (a, b) =>
                    new Date(b.DateSubmitted) -
                    new Date(a.DateSubmitted)
            );

        res.json(applications);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve applications."
        });
    }
};

const getApplication = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message: "Application not found."
            });
        }

        res.json(mapApplication(document));

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve application."
        });
    }
};

const createApplication = async (req, res) => {
    try {
        const application = req.body;

        const id = await getNextId();

        application.Id = id;
        application.Status = "Pending";

        const dateSubmitted = new Date();

        application.DateSubmitted = dateSubmitted;

        const data = {
            ...application,
            LearnerDateOfBirth:
                application.LearnerDateOfBirth
                    ? Timestamp.fromDate(
                        new Date(
                            application.LearnerDateOfBirth
                        )
                    )
                    : null,
            DateSubmitted:
                Timestamp.fromDate(dateSubmitted)
        };

        await db
            .collection(collectionName)
            .doc(id.toString())
            .set(data);

        res.status(201).json(application);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create application."
        });
    }
};

const updateApplication = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const application = req.body;

        if (id !== Number(application.Id)) {
            return res.status(400).json({
                message:
                    "The ID in the URL does not match the ID in the request."
            });
        }

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Application not found."
            });
        }

        const existingApplication =
            existing.data();

        const data = {
            ...application,
            DateSubmitted:
                existingApplication.DateSubmitted
        };

        if (application.LearnerDateOfBirth) {
            data.LearnerDateOfBirth =
                Timestamp.fromDate(
                    new Date(
                        application.LearnerDateOfBirth
                    )
                );
        }

        await documentReference.set(data);

        res.status(204).send();

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update application."
        });
    }
};

const deleteApplication = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Application not found."
            });
        }

        await documentReference.delete();

        res.status(204).send();

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete application."
        });
    }
};

module.exports = {
    getApplications,
    getApplication,
    createApplication,
    updateApplication,
    deleteApplication
};