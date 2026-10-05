const { db } = require("../config/firebase");

const collectionName = "schoolInformation";

const getSchoolInformation = async (req, res) => {
    try {
        const document = await db
            .collection(collectionName)
            .doc("main")
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message: "School information was not found."
            });
        }

        const data = document.data();

        res.json({
            Id: data.Id || 0,
            SchoolName: data.SchoolName || "",
            Motto: data.Motto || "",
            Description: data.Description || "",
            History: data.History || "",
            Mission: data.Mission || "",
            Vision: data.Vision || "",
            Address: data.Address || "",
            Phone: data.Phone || "",
            Email: data.Email || "",
            PrincipalName: data.PrincipalName || ""
        });

    } catch (error) {
        console.error("Error getting school information:", error);

        res.status(500).json({
            message: "Failed to retrieve school information."
        });
    }
};

const createSchoolInformation = async (req, res) => {
    try {
        const documentReference = db
            .collection(collectionName)
            .doc("main");

        const existing = await documentReference.get();

        if (existing.exists) {
            return res.status(409).json({
                message:
                    "School information already exists. Use PUT to update it."
            });
        }

        const schoolInformation = req.body;

        await documentReference.set(schoolInformation);

        res.status(201).json({
            message: "School information created successfully.",
            schoolInformation
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create school information."
        });
    }
};

const updateSchoolInformation = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const updatedInformation = req.body;

        if (id !== Number(updatedInformation.Id)) {
            return res.status(400).json({
                message:
                    "The ID in the URL does not match the ID in the request."
            });
        }

        const documentReference = db
            .collection(collectionName)
            .doc("main");

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "School information was not found."
            });
        }

        await documentReference.set(
            updatedInformation,
            { merge: false }
        );

        res.json({
            message: "School information updated successfully.",
            schoolInformation: updatedInformation
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update school information."
        });
    }
};

module.exports = {
    getSchoolInformation,
    createSchoolInformation,
    updateSchoolInformation
};