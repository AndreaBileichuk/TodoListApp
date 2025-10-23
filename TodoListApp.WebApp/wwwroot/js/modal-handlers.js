export function initializeTodoTaskModal() {
    const taskModalElement = document.getElementById('taskModal');
    if (!taskModalElement) return;

    const modal = new bootstrap.Modal(taskModalElement);
    const modalBody = taskModalElement.querySelector('.modal-body');

    // Edit a task
    document.body.addEventListener('click', async function (event) {
        const editButton = event.target.closest('.edit-task-btn');
        if (!editButton) return;

        event.preventDefault();
        const url = editButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = taskModalElement.querySelector(".modal-title");
            if (modalTitle) {
                modalTitle.innerText = "Edit Task";
            }

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, '#editTaskForm');
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });

    // Create a task
    document.body.addEventListener('click', async function (event) {
        const addButton = event.target.closest('.add-task-btn');
        if (!addButton) return;

        event.preventDefault();
        const url = addButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = taskModalElement.querySelector(".modal-title");
            if (modalTitle) {
                modalTitle.innerText = "Add Task";
            }

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, '#createTaskForm');
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });

    // Delete a task
    document.body.addEventListener('click', async function (event) {
        const addButton = event.target.closest('.delete-task-btn');
        if (!addButton) return;

        event.preventDefault();
        const url = addButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = taskModalElement.querySelector(".modal-title");
            if (modalTitle) {
                modalTitle.innerText = "Delete Task";
            }

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, '#createTaskForm');
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });
}

export function initializeTodoListModal() {
    const todoModalElement = document.getElementById('todoListModal');
    if (!todoModalElement) return;

    const modal = new bootstrap.Modal(todoModalElement);
    const modalBody = todoModalElement.querySelector('.modal-body');

    // Edit a todoList
    document.body.addEventListener('click', async function (event) {
        const editButton = event.target.closest('.edit-todo-list-btn');
        if (!editButton) return;

        event.preventDefault();
        const url = editButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = todoModalElement.querySelector(".modal-title");
            if (modalTitle) {
                modalTitle.innerText = "Edit Task";
            }

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, "#editTodoListForm");
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });

    // Create a todoList
    document.body.addEventListener('click', async function (event) {
        const addButton = event.target.closest('.add-todo-list-btn');
        if (!addButton) return;

        event.preventDefault();
        const url = addButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = todoModalElement.querySelector(".modal-title");
            modalTitle.innerText = "Add Todo";

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, '#createTodoListForm');
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });

    // Delete a todoList
    document.body.addEventListener("click", async function(event) {
        const deleteButton = event.target.closest('.delete-todo-list-btn');
        if(!deleteButton) return;

        event.preventDefault();
        const url = deleteButton.getAttribute("href");

        try {
            const response = await fetch(url);
            const modalTitle = todoModalElement.querySelector(".modal-title");
            modalTitle.innerText = "Delete Todo"

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, '#deleteTodoListForm');
            modal.show();
        } catch(error) {
            console.error("Error loading modal context:", error);
        }
    });
}

export function initializeAssignToTaskModal() {
    const todoModalElement = document.getElementById('taskModal');
    if (!todoModalElement) return;

    const modal = new bootstrap.Modal(todoModalElement);
    const modalBody = todoModalElement.querySelector('.modal-body');

    // Edit a todoList
    document.body.addEventListener('click', async function (event) {
        const assignButton = event.target.closest('.assign-task-btn');
        if (!assignButton) return;

        event.preventDefault();
        const url = assignButton.getAttribute('href');

        try {
            const response = await fetch(url);
            const modalTitle = todoModalElement.querySelector(".modal-title");
            if (modalTitle) {
                modalTitle.innerText = "Assign Task";
            }

            modalBody.innerHTML = await response.text();
            attachFormHandler(modalBody, "#assignTaskToUserForm");
            modal.show();
        } catch (error) {
            console.error('Error loading modal content:', error);
        }
    });
}

function attachFormHandler(modalBody, id) {
    const form = modalBody.querySelector(id);
    if (!form) return;

    form.addEventListener('submit', async function (event) {
        event.preventDefault();

        const formData = new FormData(form);
        const actionUrl = form.getAttribute('action');
        const token = form.querySelector('input[name="__RequestVerificationToken"]').value;

        try {
            const response = await fetch(actionUrl, {
                method: 'POST',
                body: formData,
                headers: {'RequestVerificationToken': token}
            });

            if (response.redirected) {
                window.location.href = response.url;
            } else {
                modalBody.innerHTML = await response.text();
                attachFormHandler(modalBody, id);
            }
        } catch (error) {
            console.error('Form submission error:', error);
        }
    })
}