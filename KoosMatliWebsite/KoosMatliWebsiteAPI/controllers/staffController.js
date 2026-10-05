const { db } = require("../config/firebase");

const collectionName = "staffMembers";

const mapStaffMember = (document) => {
    const data = document.data();

    return {
        Id: data.Id || 0,
        FullName: data.FullName || "",
        Position: data.Position || "",
        Department: data.Department || "",
        Category: data.Category || "",
        Biography: data.Biography || "",
        ImageUrl: data.ImageUrl || "",
        Email: data.Email || "",
        IsActive: data.IsActive !== false
    };
};

const getStaffMembers = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const staffMembers = snapshot.docs
            .map(mapStaffMember)
            .sort((a, b) => a.Id - b.Id);

        res.json(staffMembers);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve staff members."
        });
    }
};

const getStaffMember = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message:
                    `Staff member with ID ${id} was not found.`
            });
        }

        res.json(mapStaffMember(document));

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve staff member."
        });
    }
};

const createStaffMember = async (req, res) => {
    try {
        const staffMember = req.body;

        const documentReference = db
            .collection(collectionName)
            .doc(staffMember.Id.toString());

        const existing = await documentReference.get();

        if (existing.exists) {
            return res.status(409).json({
                message:
                    `A staff member with ID ${staffMember.Id} already exists.`
            });
        }

        await documentReference.set(staffMember);

        res.status(201).json(staffMember);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create staff member."
        });
    }
};

const updateStaffMember = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const staffMember = req.body;

        if (id !== Number(staffMember.Id)) {
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
                    `Staff member with ID ${id} was not found.`
            });
        }

        await documentReference.set(staffMember);

        res.json({
            message: "Staff member updated successfully.",
            staffMember
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update staff member."
        });
    }
};

const deleteStaffMember = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message:
                    `Staff member with ID ${id} was not found.`
            });
        }

        await documentReference.delete();

        res.json({
            message: "Staff member deleted successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete staff member."
        });
    }
};

module.exports = {
    getStaffMembers,
    getStaffMember,
    createStaffMember,
    updateStaffMember,
    deleteStaffMember
};