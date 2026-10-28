require("dotenv").config();

const express = require("express");
const cors = require("cors");
const helmet = require("helmet");

// Initialise Firebase before loading the routes.
require("./config/firebase");

// Import route groups.
const authRoutes = require("./routes/authRoutes");
const parentRoutes = require("./routes/parentRoutes");
const teacherRoutes = require("./routes/teacherRoutes");
const adminRoutes = require("./routes/adminRoutes");

const app = express();
const PORT = process.env.PORT || 5000;

app.use(helmet());

const allowedOrigins = process.env.CLIENT_ORIGIN
    ? process.env.CLIENT_ORIGIN.split(",").map((origin) => origin.trim())
    : [];

app.use(cors({
    origin: (origin, callback) => {
        // Allow requests without an Origin header, such as local API tools.
        if (!origin || allowedOrigins.includes(origin)) {
            return callback(null, true);
        }

        return callback(new Error("Origin not allowed by CORS."));
    }
}));

app.use(express.json({ limit: "1mb" }));

// API root.
app.get("/", (req, res) => {
    res.status(200).json({
        success: true,
        message: "Koos Matli Primary School API is running."
    });
});

// Health check.
app.get("/api/health", async (req, res) => {
    try {
        const { db } = require("./config/firebase");

        await db.listCollections();

        return res.status(200).json({
            success: true,
            api: "online",
            database: "connected"
        });
    } catch (error) {
        console.error("Health check failed:", error.message);

        return res.status(503).json({
            success: false,
            api: "online",
            database: "unavailable"
        });
    }
});

// Shared authentication.
app.use("/api/auth", authRoutes);

// Role-specific routes.
app.use("/api/parent", parentRoutes);
app.use("/api/teacher", teacherRoutes);
app.use("/api/admin", adminRoutes);

// Unknown endpoint handler.
app.use((req, res) => {
    res.status(404).json({
        success: false,
        message: "Endpoint not found."
    });
});

// General error handler.
app.use((error, req, res, next) => {
    console.error("API error:", error.message);

    if (res.headersSent) {
        return next(error);
    }

    if (error.message === "Origin not allowed by CORS.") {
        return res.status(403).json({
            success: false,
            message: "This origin is not allowed to access the API."
        });
    }

    return res.status(500).json({
        success: false,
        message: "An unexpected server error occurred."
    });
});

app.listen(PORT, () => {
    console.log(`Koos Matli API running on port ${PORT}`);
});
