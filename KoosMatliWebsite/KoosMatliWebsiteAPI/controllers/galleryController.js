const { db } = require("../config/firebase");
const { Timestamp } = require("firebase-admin/firestore");

const collectionName = "gallery";

const mapGalleryItem = (document) => {
    const data = document.data();

    return {
        Id: data.Id || 0,
        Title: data.Title || "",
        Description: data.Description || "",
        ImageUrl: data.ImageUrl || "",
        DateAdded: data.DateAdded
            ? data.DateAdded.toDate()
            : null,
        Category: data.Category || "general",
        IsPublished: data.IsPublished !== false
    };
};

const getGallery = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const gallery = snapshot.docs
            .map(mapGalleryItem)
            .filter(item => item.IsPublished)
            .sort(
                (a, b) =>
                    new Date(b.DateAdded) -
                    new Date(a.DateAdded)
            );

        res.json(gallery);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve gallery."
        });
    }
};

const getGalleryItem = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message:
                    `Gallery item with ID ${id} was not found.`
            });
        }

        res.json(mapGalleryItem(document));

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve gallery item."
        });
    }
};

const createGalleryItem = async (req, res) => {
    try {
        const item = req.body;

        const snapshot = await db
            .collection(collectionName)
            .get();

        const ids = snapshot.docs
            .map(doc => Number(doc.data().Id) || 0);

        const nextId =
            ids.length > 0
                ? Math.max(...ids) + 1
                : 1;

        item.Id = nextId;

        const data = {
            ...item
        };

        if (item.DateAdded) {
            data.DateAdded = Timestamp.fromDate(
                new Date(item.DateAdded)
            );
        }

        await db
            .collection(collectionName)
            .doc(nextId.toString())
            .set(data);

        res.status(201).json(item);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create gallery item."
        });
    }
};

const updateGalleryItem = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const item = req.body;

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Gallery item was not found."
            });
        }

        const data = {
            ...item
        };

        if (item.DateAdded) {
            data.DateAdded = Timestamp.fromDate(
                new Date(item.DateAdded)
            );
        }

        await documentReference.set(data);

        res.json({
            message: "Gallery item updated successfully.",
            galleryItem: item
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update gallery item."
        });
    }
};

const deleteGalleryItem = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Gallery item was not found."
            });
        }

        await documentReference.delete();

        res.json({
            message: "Gallery item deleted successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete gallery item."
        });
    }
};

module.exports = {
    getGallery,
    getGalleryItem,
    createGalleryItem,
    updateGalleryItem,
    deleteGalleryItem
};