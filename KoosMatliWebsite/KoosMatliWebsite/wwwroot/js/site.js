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