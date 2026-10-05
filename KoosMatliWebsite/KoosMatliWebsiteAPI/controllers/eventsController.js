const { db } = require("../config/firebase");
const { Timestamp } = require("firebase-admin/firestore");

const collectionName = "events";

const mapEvent = (document) => {
    const data = document.data();

    return {
        Id: data.Id || 0,
        Title: data.Title || "",
        Description: data.Description || "",
        Date: data.Date
            ? data.Date.toDate()
            : null,
        Category: data.Category || "",
        Time: data.Time || "",
        Location: data.Location || "",
        IsPublished: data.IsPublished !== false
    };
};

const getEvents = async (req, res) => {
    try {
        const snapshot = await db
            .collection(collectionName)
            .get();

        const events = snapshot.docs
            .map(mapEvent)
            .filter(event => event.IsPublished)
            .sort(
                (a, b) =>
                    new Date(a.Date) -
                    new Date(b.Date)
            );

        res.json(events);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve events."
        });
    }
};

const getEvent = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const document = await db
            .collection(collectionName)
            .doc(id.toString())
            .get();

        if (!document.exists) {
            return res.status(404).json({
                message: `Event with ID ${id} was not found.`
            });
        }

        res.json(mapEvent(document));

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to retrieve event."
        });
    }
};

const createEvent = async (req, res) => {
    try {
        const event = req.body;

        const snapshot = await db
            .collection(collectionName)
            .get();

        const ids = snapshot.docs
            .map(doc => Number(doc.data().Id) || 0);

        const nextId =
            ids.length > 0
                ? Math.max(...ids) + 1
                : 1;

        event.Id = nextId;

        const data = {
            ...event
        };

        if (event.Date) {
            data.Date = Timestamp.fromDate(
                new Date(event.Date)
            );
        }

        await db
            .collection(collectionName)
            .doc(nextId.toString())
            .set(data);

        res.status(201).json(event);

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to create event."
        });
    }
};

const updateEvent = async (req, res) => {
    try {
        const id = Number(req.params.id);
        const event = req.body;

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Event was not found."
            });
        }

        const data = {
            ...event
        };

        if (event.Date) {
            data.Date = Timestamp.fromDate(
                new Date(event.Date)
            );
        }

        await documentReference.set(data);

        res.json({
            message: "Event updated successfully.",
            event
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to update event."
        });
    }
};

const deleteEvent = async (req, res) => {
    try {
        const id = Number(req.params.id);

        const documentReference = db
            .collection(collectionName)
            .doc(id.toString());

        const existing = await documentReference.get();

        if (!existing.exists) {
            return res.status(404).json({
                message: "Event was not found."
            });
        }

        await documentReference.delete();

        res.json({
            message: "Event deleted successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to delete event."
        });
    }
};

module.exports = {
    getEvents,
    getEvent,
    createEvent,
    updateEvent,
    deleteEvent
};