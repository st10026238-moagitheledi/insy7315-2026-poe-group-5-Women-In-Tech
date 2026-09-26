const mobileMenuButton =
    document.getElementById("mobileMenuButton");

const mobileNav =
    document.getElementById("mobileNav");


if (mobileMenuButton && mobileNav) {

    mobileMenuButton.addEventListener("click", function () {

        mobileNav.classList.toggle("active");

    });

}