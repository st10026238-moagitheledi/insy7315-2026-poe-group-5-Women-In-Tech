const express = require("express");

const router = express.Router();

const {
    getGallery,
    getGalleryItem,
    createGalleryItem,
    updateGalleryItem,
    deleteGalleryItem
} = require("../controllers/galleryController");

router.get("/", getGallery);

router.get("/:id", getGalleryItem);

router.post("/", createGalleryItem);

router.put("/:id", updateGalleryItem);

router.delete("/:id", deleteGalleryItem);

module.exports = router;