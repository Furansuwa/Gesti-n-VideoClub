// Lógica para el Modo Oscuro
document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById("theme-toggle");
    const themeIcon = themeToggleBtn.querySelector("i");
    const currentTheme = localStorage.getItem("theme");

    // Aplicar tema guardado al cargar
    if (currentTheme === "dark") {
        document.documentElement.setAttribute("data-theme", "dark");
        themeIcon.classList.remove("fa-moon");
        themeIcon.classList.add("fa-sun");
    }

    // Evento al hacer clic
    themeToggleBtn.addEventListener("click", function () {
        let theme = "light";

        if (document.documentElement.getAttribute("data-theme") !== "dark") {
            document.documentElement.setAttribute("data-theme", "dark");
            theme = "dark";
            themeIcon.classList.remove("fa-moon");
            themeIcon.classList.add("fa-sun");
        } else {
            document.documentElement.removeAttribute("data-theme");
            themeIcon.classList.remove("fa-sun");
            themeIcon.classList.add("fa-moon");
        }

        // Guardar preferencia
        localStorage.setItem("theme", theme);
    });
});