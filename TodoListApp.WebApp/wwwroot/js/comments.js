// File: wwwroot/js/comments-handler.js
import { DefaultAvatar } from './helpers.js'; // Ensure this path and export name are correct

// Function to get AntiForgeryToken
function getAntiForgeryToken() {
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenInput ? tokenInput.value : '';
}

export function initializeCommentsHandlers() {
    const commentsCollapseElement = document.getElementById('collapseComments');

    if (!commentsCollapseElement) {
        return;
    }

    const todoListId = commentsCollapseElement.dataset.listId;
    const getCommentsUrl = commentsCollapseElement.dataset.getCommentsUrl;
    const addCommentUrl = commentsCollapseElement.dataset.addCommentUrl;

    if (!todoListId || !getCommentsUrl || !addCommentUrl) {
        console.error("Missing data attributes (listId, getCommentsUrl, or addCommentUrl) on #collapseComments.");
        return;
    }
    // --- END READ IDs AND URLs ---

    const commentsBody = commentsCollapseElement.querySelector('.comments-body');
    const commentCountBadge = document.getElementById('comment-count-badge');
    const addCommentForm = document.querySelector('.add-comment-form form');
    const toggleIcon = document.querySelector('.comments-header .toggle-icon');
    let commentsLoaded = false;

    // Function to format date
    function formatCommentDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleString('uk-UA', {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    // Function to render a single comment
    function renderComment(comment) {
        const commentAvatar = comment.avatarUrl || DefaultAvatar; // Use imported default
        // Sanitize comment text before rendering to prevent XSS
        const sanitizedText = comment.text.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\n/g, '<br>');
        // Sanitize user name too
        const sanitizedUserName = comment.userName.replace(/</g, "&lt;").replace(/>/g, "&gt;");

        return `
            <div class="comment-item mb-3 pb-3 border-bottom" data-comment-id="${comment.id}">
                <div class="comment-header d-flex align-items-center mb-2">
                    <img src="${commentAvatar}" class="rounded-circle comment-avatar me-2" alt="Avatar">
                    <div>
                        <span class="comment-author fw-semibold small">${sanitizedUserName}</span>
                        <span class="comment-time text-muted small ms-2">${formatCommentDate(comment.createdAt)}</span>
                    </div>
                </div>
                <div class="comment-text small ps-4 ms-3" style="border-left: 2px solid #e9ecef; padding-left: 12px !important;">
                    ${sanitizedText}
                </div>
            </div>`;
    }

    // Function to load and render comments
    async function loadComments() {

        if (commentsLoaded && commentsBody.innerHTML.trim() !== '' && !commentsBody.querySelector('.text-danger')) return;

        commentsBody.innerHTML = '<div class="text-center p-3"><div class="spinner-border spinner-border-sm" role="status"></div> Завантаження...</div>';
        commentsLoaded = false; // Reset loaded flag until successful

        try {
            debugger
            const url = getCommentsUrl;
            const response = await fetch(url);
            if (!response.ok) throw new Error(`HTTP ${response.status} ${response.statusText}`);

            const comments = await response.json();
            commentsLoaded = true; // Set flag on success
            commentsBody.innerHTML = ''; // Clear previous content/spinner
            commentCountBadge.textContent = comments.length;

            if (comments.length === 0) {
                commentsBody.innerHTML = '<p class="text-muted text-center mb-0 small p-3">Коментарів ще немає.</p>';
            } else {
                comments.forEach(comment => {
                    commentsBody.insertAdjacentHTML('beforeend', renderComment(comment));
                });
            }
        } catch (error) {
            console.error('Помилка завантаження коментарів:', error);
            commentsBody.innerHTML = '<p class="text-danger text-center mb-0 small p-3">Не вдалося завантажити коментарі.</p>';
            commentCountBadge.textContent = '⚠';
        }
    }

    // Event Listeners for Bootstrap Collapse
    commentsCollapseElement.addEventListener('show.bs.collapse', function () {
        loadComments(); // Load on first show
        if(toggleIcon) {
            toggleIcon.classList.remove('bi-chevron-down');
            toggleIcon.classList.add('bi-chevron-up');
        }
    });

    commentsCollapseElement.addEventListener('hide.bs.collapse', function () {
        if(toggleIcon) {
            toggleIcon.classList.remove('bi-chevron-up');
            toggleIcon.classList.add('bi-chevron-down');
        }
    });

    // Event Listener for Add Comment Form Submission (AJAX)
    if (addCommentForm) {
        addCommentForm.addEventListener('submit', async function (event) {
            event.preventDefault(); // Prevent standard form submission
            const textArea = addCommentForm.querySelector('textarea[name="commentText"]');
            const submitButton = addCommentForm.querySelector('button[type="submit"]');
            const commentText = textArea.value.trim();
            if (!commentText) return; // Don't submit empty comments

            submitButton.disabled = true;
            submitButton.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Надсилання...';

            try {
                const formData = new FormData(addCommentForm);
                const postUrl = addCommentUrl; // Use the URL from data attribute

                const response = await fetch(postUrl, {
                    method: 'POST',
                    body: formData,
                    headers: { // Need token header when using FormData this way for AntiForgery
                        'RequestVerificationToken': getAntiForgeryToken()
                    }
                });

                if (response.ok) {
                    textArea.value = ''; // Clear textarea
                    commentsLoaded = false; // Reset flag to force reload
                    // Check if collapse is open before reloading, otherwise just reset flag
                    const isCollapsed = !commentsCollapseElement.classList.contains('show');
                    if (!isCollapsed) {
                        await loadComments(); // Reload comments if section is open
                        // Optional: scroll to bottom after reload
                        // commentsBody.scrollTop = commentsBody.scrollHeight;
                    } else {
                        // If closed, just update the badge count (approximate)
                        commentCountBadge.textContent = parseInt(commentCountBadge.textContent || '0') + 1;
                    }
                } else {
                    let errorMsg = response.statusText;
                    try {
                        const contentType = response.headers.get("content-type");
                        if (contentType && contentType.indexOf("application/json") !== -1) {
                            const errorData = await response.json();
                            errorMsg = errorData.message || errorMsg;
                        } else {
                            errorMsg = await response.text();
                        }
                    } catch {}
                    alert(`Не вдалося додати коментар: ${errorMsg}`);
                }
            } catch (error) {
                console.error('Помилка додавання коментаря:', error);
                alert('Сталася помилка під час додавання коментаря.');
            } finally {
                submitButton.disabled = false;
                submitButton.innerHTML = '<i class="bi bi-send"></i> Надіслати';
            }
        });
    }
}