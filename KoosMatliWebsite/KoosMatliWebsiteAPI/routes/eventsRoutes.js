const express = require("express");

const router = express.Router();

const {
    getEvents,
    getEvent,
    createEvent,
    updateEvent,
    deleteEvent
} = require("../controllers/eventsController");

router.get("/", getEvents);

router.get("/:id", getEvent);

router.post("/", createEvent);

router.put("/:id", updateEvent);

router.delete("/:id", deleteEvent);

module.exports = router;