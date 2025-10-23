export function initializeToastNotifications() {
    const successMessage = document.querySelector('[data-success-message]')?.dataset.successMessage;
    const errorMessage = document.querySelector('[data-error-message]')?.dataset.errorMessage;

    if (!successMessage && !errorMessage) return;

    const toastEl = document.getElementById('toastMessage');
    const toastBody = document.getElementById('toastBody');
    const toastTitle = document.getElementById('toastTitle');
    const toastIcon = document.getElementById('toastIcon');

    if (errorMessage) {
        toastEl.classList.remove('text-bg-success');
        toastEl.classList.add('text-bg-danger');
        toastTitle.textContent = 'Error';
        toastBody.textContent = errorMessage;
        toastIcon.className = 'bi bi-x-circle-fill fs-4 me-3';
    } else {
        toastEl.classList.remove('text-bg-danger');
        toastEl.classList.add('text-bg-success');
        toastTitle.textContent = 'Success';
        toastBody.textContent = successMessage;
        toastIcon.className = 'bi bi-check-circle-fill fs-4 me-3';
    }

    const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 4000 });
    toast.show();
}