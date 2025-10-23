export function setupPasswordToggle(toggleBtnId, fieldId, iconId) {
    const toggleBtn = document.getElementById(toggleBtnId);
    const passwordField = document.getElementById(fieldId);
    const icon = document.getElementById(iconId);

    if (toggleBtn && passwordField && icon) {
        toggleBtn.addEventListener("click", function () {
            // Перевіряємо поточний тип поля
            const type = passwordField.getAttribute("type") === "password" ? "text" : "password";
            passwordField.setAttribute("type", type);

            // Змінюємо іконку
            if (type === "password") {
                icon.classList.remove("bi-eye");
                icon.classList.add("bi-eye-slash");
            } else {
                icon.classList.remove("bi-eye-slash");
                icon.classList.add("bi-eye");
            }
        });
    }
}