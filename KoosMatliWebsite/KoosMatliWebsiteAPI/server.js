const express = require("express");
const cors = require("cors");
require("dotenv").config();

require("./config/firebase");

const aboutRoutes = require("./routes/aboutRoutes");
const staffRoutes = require("./routes/staffRoutes");
const newsRoutes = require("./routes/newsRoutes");
const eventsRoutes = require("./routes/eventsRoutes");
const galleryRoutes = require("./routes/galleryRoutes");
const contactRoutes = require("./routes/contactRoutes");
const applicationsRoutes = require("./routes/applicationsRoutes");


const app = express();

const PORT = process.env.PORT || 5000;

app.use(cors());
app.use(express.json());

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

app.use("/api/about", aboutRoutes);

app.listen(PORT, () => {
    console.log(`Koos Matli API running on port ${PORT}`);
});