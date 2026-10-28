require("dotenv").config();

const express = require("express");
const cors = require("cors");
const helmet = require("helmet");

// Initialise Firebase before registering routes.
require("./config/firebase");

const app = express();
const PORT = process.env.PORT || 5000;

app.use(helmet());
app.use(cors({
    origin: process.env.CLIENT_ORIGIN
        ? process.env.CLIENT_ORIGIN.split(",")
        : false
}));
app.use(express.json({ limit: "1mb" }));

app.get("/", (req, res) => {
    res.json({
        success: true,
        message: "Koos Matli Primary School API is running."
    });
});

app.get("/api/health", async (req, res) => {
    try {
        const { db } = require("./config/firebase");

        // Verify that Firestore can be reached.
        await db.listCollections();

        res.status(200).json({
            success: true,
            api: "online",
            database: "connected"
        });
    } catch (error) {
        console.error("Health check failed:", error.message);

        res.status(503).json({
            success: false,
            api: "online",
            database: "unavailable"
        });
    }
});

app.use((req, res) => {
    res.status(404).json({
        success: false,
        message: "Endpoint not found."
    });
});

app.listen(PORT, () => {
    console.log(`Koos Matli API running on port ${PORT}`);
});