const { db } = require("../config/firebase");
const { Timestamp } = require("firebase-admin/firestore");

const getNextId = async (collectionName) => {
    const snapshot = await db
        .collection(collectionName)
        .get();

    const ids = snapshot.docs
        .map(doc => Number(doc.data().Id) || 0);

    return ids.length > 0
        ? Math.max(...ids) + 1
        : 1;
};

const sendMessage = async (req, res) => {
    try {
        const message = req.body;

        if (
            !message.Name ||
            !message.Email ||
            !message.Subject ||
            !message.Message
        ) {
            return res.status(400).json({
                message:
                    "Name, email, subject and message are required."
            });
        }

        const id = await getNextId("contactMessages");

        const submittedAt = new Date();

        const data = {
            Id: id,
            Name: message.Name,
            Email: message.Email,
            Subject: message.Subject,
            Message: message.Message,
            SubmittedAt: Timestamp.fromDate(submittedAt),
            IsRead: false
        };

        await db
            .collection("contactMessages")
            .doc(id.toString())
            .set(data);

        res.json({
            message: "Your message has been sent successfully."
        });

    } catch (error) {
        console.error(error);

        res.status(500).json({
            message: "Failed to send your message."
        });
    }
};

module.exports = {
    sendMessage
};