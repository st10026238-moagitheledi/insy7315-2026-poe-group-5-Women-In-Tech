const express = require("express");
const cors = require("cors");
require("dotenv").config();

require("./config/firebase");

const aboutRoutes = require("./routes/aboutRoutes");
const staffRoutes = require("./routes/staffRoutes");
const newsRoutes = require("./routes/newsRoutes");
const eventsRoutes = require("./routes/eventsRoutes");
const galleryRoutes = require("./routes/galleryRoutes");
const admissionsRoutes = require("./routes/admissionsRoutes");
const contactRoutes = require("./routes/contactRoutes");
const applicationsRoutes = require("./routes/applicationsRoutes");


const app = express();

const PORT = process.env.PORT || 5000;


// =========================
// MIDDLEWARE
// =========================

app.use(cors());
app.use(express.json());


// =========================
// BASIC ROUTES
// =========================

app.get("/", (req, res) => {
    res.json({
        success: true,
        message: "Koos Matli Primary School API is running."
    });
});

app.get("/health", (req, res) => {
    res.json({
        success: true,
        status: "healthy"
    });
});


// =========================
// API ROUTES
// =========================

app.use("/api/about", aboutRoutes);

app.use("/api/staffmembers", staffRoutes);

app.use("/api/news", newsRoutes);

app.use("/api/events", eventsRoutes);

app.use("/api/gallery", galleryRoutes);

app.use("/api/admissions", admissionsRoutes);

app.use("/api/contact", contactRoutes);

app.use("/api/Applications", applicationsRoutes);


// =========================
// START SERVER
// =========================

app.listen(PORT, () => {
    console.log(`Koos Matli API running on port ${PORT}`);
});