const { db } = require("../config/firebase");

const collectionName = "news";

const mapNews = (document) => {
    const data = document.data();

    let publishedDate = null;

    if (data.PublishedDate) {
        publishedDate = data.PublishedDate.toDate();
    }

    return {
        Id: data.Id || 0,
        Title: data.Title || "",
        Content: data.Content || "",
        ImageUrl: data.ImageUrl || "",
        PublishedDate: publishedDate,
        IsPublished: data.IsPublished !== false,
        Category: data.Category || "general"
    };
};

const getNews = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const news = snapshot.docs
            .map(mapNews)
            .filter(news => news.IsPublished)
            .sort(
                (a, b) =>
                    new Date(b.PublishedDate) -
                    new Date(a.PublishedDate)
            );

        res.json(news);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve news."
        });
    }
};

const getNewsById = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message: `News item with ID ${id} was not found.`
            });
        }

        const news = mapNews(document);

        if (!news.IsPublished) {
            return res.status(404).json({
                message: `News item with ID ${id} was not found.`
            });
        }

        res.json(news);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve news item."
        });
    }
};

const createNews = async (req, res) => {
    try {
        const news = req.body;

        const snapshot = await db
            .collection(collectionName)
            .get();

        const ids = snapshot.docs
            .map(doc => Number(doc.data().Id) || 0);

        const nextId =
            ids.length > 0
                ? Math.max(...ids) + 1
                : 1;

        news.Id = nextId;

        const documentReference = db
            .collection(collectionName)
            .doc(nextId.toString());

        if (news.PublishedDate) {
            news.PublishedDate =
                new Date(news.PublishedDate);
        }

        await documentReference.set({
            ...news,
            PublishedDate:
                adminTimestamp(news.PublishedDate)
        });

        res.status(201).json(news);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create news item."
        });
    }
};

const adminTimestamp = (date) => {
    if (!date) {
        return new Date();
    }

    const { Timestamp } = require("firebase-admin/firestore");

    return Timestamp.fromDate(
        new Date(date)
    );
};

const updateNews = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const news = req.body;

        if (id !== Number(news.Id)) {
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
                message: "News item was not found."
            });
        }

        const data = {
            ...news
        };

        if (data.PublishedDate) {
            data.PublishedDate =
                adminTimestamp(data.PublishedDate);
        }

        await documentReference.set(data);

        res.json({
            message: "News item updated successfully.",
            news
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update news item."
        });
    }
};

const deleteNews = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "News item was not found."
            });
        }

        await documentReference.delete();

        res.json({
            message: "News item deleted successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete news item."
        });
    }
};

module.exports = {
    getNews,
    getNewsById,
    createNews,
    updateNews,
    deleteNews
};