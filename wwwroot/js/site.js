// Toast Notification System
const ToastNotification = {
    container: null,

    init() {
        // Create toast container if it doesn't exist
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.className = 'toast-container';
            document.body.appendChild(this.container);
        }
    },

    show(message, type = 'success', duration = 4000) {
        this.init();

        const toast = document.createElement('div');
        toast.className = `toast-notification ${type}`;

        const icons = {
            success: 'bi-check-circle-fill',
            error: 'bi-x-circle-fill',
            info: 'bi-info-circle-fill',
            warning: 'bi-exclamation-triangle-fill'
        };

        toast.innerHTML = `
            <div class="toast-content">
                <i class="bi ${icons[type]} toast-icon"></i>
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" aria-label="Kapat">
                <i class="bi bi-x"></i>
            </button>
        `;

        this.container.appendChild(toast);

        // Close button handler
        const closeBtn = toast.querySelector('.toast-close');
        closeBtn.addEventListener('click', () => {
            this.remove(toast);
        });

        // Auto-remove after duration
        setTimeout(() => {
            this.remove(toast);
        }, duration);
    },

    remove(toast) {
        toast.classList.add('removing');
        setTimeout(() => {
            toast.remove();
        }, 300); // Match animation duration
    },

    success(message, duration) {
        this.show(message, 'success', duration);
    },

    error(message, duration) {
        this.show(message, 'error', duration);
    },

    info(message, duration) {
        this.show(message, 'info', duration);
    },

    warning(message, duration) {
        this.show(message, 'warning', duration);
    }
};

// Make it globally available
window.Toast = ToastNotification;
