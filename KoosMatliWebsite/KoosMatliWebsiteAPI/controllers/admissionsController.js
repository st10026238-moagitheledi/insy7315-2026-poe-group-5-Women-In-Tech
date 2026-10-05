const { db } = require("../config/firebase");

const collectionName = "admissions";

const mapAdmission = (document) => {
    const data = document.data();

    return {
        Id: data.Id || 0,
        Title: data.Title || "",
        Description: data.Description || "",
        Requirements: data.Requirements || "",
        ApplicationInstructions:
            data.ApplicationInstructions || "",
        ApplicationLink:
            data.ApplicationLink || "",
        IsActive: data.IsActive !== false
    };
};

const getAdmissions = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const admissions = snapshot.docs
            .map(mapAdmission)
            .filter(admission => admission.IsActive)
            .sort((a, b) => a.Id - b.Id);

        res.json(admissions);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve admissions."
        });
    }
};

const getAdmission = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message:
                    `Admission with ID ${id} was not found.`
            });
        }

        const admission = mapAdmission(document);

        if (!admission.IsActive) {
            return res.status(404).json({
                message:
                    `Admission with ID ${id} is not active.`
            });
        }

        res.json(admission);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve admission."
        });
    }
};

const createAdmission = async (req, res) => {
    try {
        const admission = req.body;

        const snapshot = await db
            .collection(collectionName)
            .get();

        const ids = snapshot.docs
            .map(doc => Number(doc.data().Id) || 0);

        const nextId =
            ids.length > 0
                ? Math.max(...ids) + 1
                : 1;

        admission.Id = nextId;

        await db
            .collection(collectionName)
            .doc(nextId.toString())
            .set(admission);

        res.status(201).json(admission);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create admission."
        });
    }
};

const updateAdmission = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const admission = req.body;

        if (id !== Number(admission.Id)) {
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
                message:
                    `Admission with ID ${id} was not found.`
            });
        }

        await documentReference.set(admission);

        res.json({
            message: "Admission updated successfully.",
            admission
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update admission."
        });
    }
};

const deleteAdmission = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message:
                    `Admission with ID ${id} was not found.`
            });
        }

        await documentReference.delete();

        res.json({
            message: "Admission deleted successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete admission."
        });
    }
};

module.exports = {
    getAdmissions,
    getAdmission,
    createAdmission,
    updateAdmission,
    deleteAdmission
};