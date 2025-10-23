import { initializeToastNotifications } from './toast-notifications.js';
import {initializeAssignToTaskModal, initializeTodoListModal, initializeTodoTaskModal} from './modal-handlers.js';
import {setupPasswordToggle} from "./password-toggle.js";
import {initializeCommentsHandlers} from "./comments.js";
document.addEventListener("DOMContentLoaded", function () {
    initializeToastNotifications();
    initializeTodoTaskModal();
    initializeTodoListModal();
    initializeAssignToTaskModal();
    setupPasswordToggle("toggle-password", "password-field", "toggle-password-icon");
    initializeCommentsHandlers();
});