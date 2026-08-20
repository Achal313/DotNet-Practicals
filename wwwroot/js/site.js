document.addEventListener("DOMContentLoaded", function () {

    const themeButton = document.getElementById("themeToggle");

    const savedTheme = localStorage.getItem("theme");

    if (savedTheme === "dark") {
        document.body.classList.add("dark-theme");

        if (themeButton) {
            themeButton.innerHTML = "☀️ Light";
        }
    }

    if (themeButton) {

        themeButton.addEventListener("click", function () {

            document.body.classList.toggle("dark-theme");

            const darkMode =
                document.body.classList.contains("dark-theme");

            if (darkMode) {

                localStorage.setItem("theme", "dark");

                themeButton.innerHTML = "☀️ Light";

            } else {

                localStorage.setItem("theme", "light");

                themeButton.innerHTML = "🌙 Dark";

            }

        });

    }

});