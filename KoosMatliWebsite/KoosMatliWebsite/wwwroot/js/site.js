// =========================================================
// MOBILE NAVIGATION
// =========================================================
const mobileMenuButton =
    document.getElementById("mobileMenuButton");

const mobileNav =
    document.getElementById("mobileNav");


if (mobileMenuButton && mobileNav) {

    mobileMenuButton.addEventListener("click", function () {

        mobileNav.classList.toggle("active");

    });

}

// =========================================================
// STAFF FILTERS
// =========================================================
const staffFilters =
    document.querySelectorAll(".staff-filter");

const staffCards =
    document.querySelectorAll(".staff-card");


staffFilters.forEach(function (button) {

    button.addEventListener("click", function () {

        // Remove active state from all buttons
        staffFilters.forEach(function (item) {

            item.classList.remove("active");

        });


        // Activate clicked button
        button.classList.add("active");


        // Get selected category
        const selectedCategory =
            button.dataset.filter;


        // Show/hide cards
        staffCards.forEach(function (card) {

            const cardCategory =
                card.dataset.category;


            if (
                selectedCategory === "all" ||
                cardCategory === selectedCategory
            ) {

                card.style.display = "";

            } else {

                card.style.display = "none";

            }

        });

    });

});

// =========================================================
// NEWS FILTERS
// =========================================================
const newsFilters =
    document.querySelectorAll(".news-filter");

const newsItems =
    document.querySelectorAll(".news-item");

const newsEmpty =
    document.getElementById("newsEmpty");


newsFilters.forEach(function (button) {

    button.addEventListener("click", function () {

        // Remove active state
        newsFilters.forEach(function (item) {

            item.classList.remove("active");

        });


        // Activate selected filter
        button.classList.add("active");


        // Get selected category
        const selectedCategory =
            button.dataset.newsFilter;


        let visibleItems = 0;


        // Filter news
        newsItems.forEach(function (item) {

            const itemCategory =
                item.dataset.newsCategory;


            if (
                selectedCategory === "all" ||
                itemCategory === selectedCategory
            ) {

                item.style.display = "";

                visibleItems++;

            } else {

                item.style.display = "none";

            }

        });


        // Show empty state if necessary
        if (newsEmpty) {

            if (visibleItems === 0) {

                newsEmpty.classList.add("visible");

            } else {

                newsEmpty.classList.remove("visible");

            }

        }

    });

});

// =========================================================
// SCHOOL GALLERY FILTERS
// =========================================================

const galleryFilters =
    document.querySelectorAll(".gallery-filter");

const galleryCards =
    document.querySelectorAll(".gallery-card");

const galleryEmpty =
    document.getElementById("galleryEmpty");


galleryFilters.forEach(function (button) {

    button.addEventListener(
        "click",
        function () {

            // Remove active state
            galleryFilters.forEach(function (item) {

                item.classList.remove("active");

            });


            // Activate clicked filter
            button.classList.add("active");


            // Get selected category
            const selectedCategory =
                button.dataset.galleryFilter;


            let visibleCards = 0;


            // Filter gallery
            galleryCards.forEach(function (card) {

                const cardCategory =
                    card.dataset.galleryCategory;


                if (
                    selectedCategory === "all" ||
                    cardCategory === selectedCategory
                ) {

                    card.style.display = "";

                    visibleCards++;

                } else {

                    card.style.display = "none";

                }

            });


            // Empty state
            if (galleryEmpty) {

                if (visibleCards === 0) {

                    galleryEmpty.classList.add("visible");

                } else {

                    galleryEmpty.classList.remove("visible");

                }

            }

        }
    );

});

// =========================================================
// SCHOOL GALLERY LIGHTBOX
// =========================================================

const galleryLightbox =
    document.getElementById("galleryLightbox");

const galleryLightboxImage =
    document.getElementById("galleryLightboxImage");

const galleryLightboxTitle =
    document.getElementById("galleryLightboxTitle");

const galleryLightboxCategory =
    document.getElementById("galleryLightboxCategory");

const galleryLightboxClose =
    document.getElementById("galleryLightboxClose");

const galleryLightboxPrev =
    document.getElementById("galleryLightboxPrev");

const galleryLightboxNext =
    document.getElementById("galleryLightboxNext");


let currentGalleryIndex = 0;


// =========================================================
// GET VISIBLE GALLERY CARDS
// =========================================================

function getVisibleGalleryCards() {

    return Array.from(galleryCards)
        .filter(function (card) {

            return card.style.display !== "none";

        });

}


// =========================================================
// OPEN LIGHTBOX
// =========================================================

function openGalleryLightbox(card) {

    if (!galleryLightbox) {
        return;
    }


    const visibleCards =
        getVisibleGalleryCards();


    currentGalleryIndex =
        visibleCards.indexOf(card);


    updateGalleryLightbox();


    galleryLightbox.classList.add("active");

    galleryLightbox.setAttribute(
        "aria-hidden",
        "false"
    );


    document.body.classList.add(
        "gallery-lightbox-open"
    );

}


// =========================================================
// UPDATE LIGHTBOX
// =========================================================

function updateGalleryLightbox() {

    const visibleCards =
        getVisibleGalleryCards();


    if (
        visibleCards.length === 0 ||
        currentGalleryIndex < 0
    ) {

        return;

    }


    const card =
        visibleCards[currentGalleryIndex];


    const image =
        card.dataset.galleryImage;

    const title =
        card.dataset.galleryTitle;

    const category =
        card.dataset.galleryCategory;


    galleryLightboxImage.src =
        image;


    galleryLightboxImage.alt =
        title;


    galleryLightboxTitle.textContent =
        title;


    galleryLightboxCategory.textContent =
        category.toUpperCase();

}


// =========================================================
// CLOSE LIGHTBOX
// =========================================================

function closeGalleryLightbox() {

    if (!galleryLightbox) {
        return;
    }


    galleryLightbox.classList.remove(
        "active"
    );


    galleryLightbox.setAttribute(
        "aria-hidden",
        "true"
    );


    document.body.classList.remove(
        "gallery-lightbox-open"
    );


    galleryLightboxImage.src = "";

}


// =========================================================
// NEXT IMAGE
// =========================================================

function showNextGalleryImage() {

    const visibleCards =
        getVisibleGalleryCards();


    if (visibleCards.length === 0) {
        return;
    }


    currentGalleryIndex =
        (
            currentGalleryIndex + 1
        ) %
        visibleCards.length;


    updateGalleryLightbox();

}


// =========================================================
// PREVIOUS IMAGE
// =========================================================

function showPreviousGalleryImage() {

    const visibleCards =
        getVisibleGalleryCards();


    if (visibleCards.length === 0) {
        return;
    }


    currentGalleryIndex =
        (
            currentGalleryIndex -
            1 +
            visibleCards.length
        ) %
        visibleCards.length;


    updateGalleryLightbox();

}


// =========================================================
// OPEN IMAGE WHEN CARD IS CLICKED
// =========================================================

galleryCards.forEach(function (card) {

    card.addEventListener(
        "click",
        function () {

            openGalleryLightbox(card);

        }
    );

});


// =========================================================
// CLOSE BUTTON
// =========================================================

if (galleryLightboxClose) {

    galleryLightboxClose.addEventListener(
        "click",
        closeGalleryLightbox
    );

}


// =========================================================
// PREVIOUS BUTTON
// =========================================================

if (galleryLightboxPrev) {

    galleryLightboxPrev.addEventListener(
        "click",
        showPreviousGalleryImage
    );

}


// =========================================================
// NEXT BUTTON
// =========================================================

if (galleryLightboxNext) {

    galleryLightboxNext.addEventListener(
        "click",
        showNextGalleryImage
    );

}


// =========================================================
// CLOSE WHEN BACKDROP IS CLICKED
// =========================================================

const galleryLightboxBackdrop =
    document.querySelector(
        ".gallery-lightbox-backdrop"
    );


if (galleryLightboxBackdrop) {

    galleryLightboxBackdrop.addEventListener(
        "click",
        closeGalleryLightbox
    );

}


// =========================================================
// KEYBOARD CONTROLS
// =========================================================

document.addEventListener(
    "keydown",
    function (event) {

        if (
            !galleryLightbox ||
            !galleryLightbox.classList.contains(
                "active"
            )
        ) {

            return;

        }


        if (event.key === "Escape") {

            closeGalleryLightbox();

        }


        if (event.key === "ArrowRight") {

            showNextGalleryImage();

        }


        if (event.key === "ArrowLeft") {

            showPreviousGalleryImage();

        }

    }
);