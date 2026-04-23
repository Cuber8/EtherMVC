// Script.js - Backend UX Handler
// Manages client-side interactions and communication with backend

class EtherUX {
    constructor() {
        this.apiEndpoint = window.location.origin;
        this.requestTimeout = 5000;
        this.cache = new Map();
        this.listeners = {};
        console.log("[EtherUX] Initializing backend UX handler...");
        this.init();
    }

    /**
     * Initialize UX handler
     */
    init() {
        this.setupEventListeners();
        this.setupErrorHandling();
        console.log("[EtherUX] UX handler initialized");
    }

    /**
     * Setup global event listeners
     */
    setupEventListeners() {
        // Handle form submissions
        document.addEventListener('submit', (e) => {
            if (e.target.dataset.etherForm) {
                e.preventDefault();
                this.handleFormSubmit(e.target);
            }
        });

        // Handle data-ether attributes
        document.addEventListener('click', (e) => {
            const etherAction = e.target.dataset.etherAction;
            if (etherAction) {
                this.executeAction(etherAction, e.target);
            }
        });
    }

    /**
     * Setup global error handling
     */
    setupErrorHandling() {
        window.addEventListener('error', (e) => {
            console.error("[EtherUX] Global error:", e.error);
            this.displayError(e.error?.message || "An error occurred");
        });

        window.addEventListener('unhandledrejection', (e) => {
            console.error("[EtherUX] Unhandled promise rejection:", e.reason);
            this.displayError(e.reason?.message || "An error occurred");
        });
    }

    /**
     * Make API request
     */
    async request(method, endpoint, data = null) {
        try {
            const url = `${this.apiEndpoint}${endpoint}`;
            
            const options = {
                method: method.toUpperCase(),
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                timeout: this.requestTimeout
            };

            if (data) {
                options.body = JSON.stringify(data);
            }

            const response = await fetch(url, options);
            
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            return await response.json();
        } catch (error) {
            console.error("[EtherUX] Request error:", error);
            throw error;
        }
    }

    /**
     * GET request
     */
    async get(endpoint) {
        return this.request('GET', endpoint);
    }

    /**
     * POST request
     */
    async post(endpoint, data) {
        return this.request('POST', endpoint, data);
    }

    /**
     * PUT request
     */
    async put(endpoint, data) {
        return this.request('PUT', endpoint, data);
    }

    /**
     * DELETE request
     */
    async delete(endpoint) {
        return this.request('DELETE', endpoint);
    }

    /**
     * Handle form submission
     */
    async handleFormSubmit(form) {
        try {
            const formData = new FormData(form);
            const data = Object.fromEntries(formData);
            const endpoint = form.action || form.dataset.etherEndpoint;
            const method = form.method || 'POST';

            console.log(`[EtherUX] Submitting form to ${method} ${endpoint}`);

            const result = await this.request(method, endpoint, data);
            
            if (result.success) {
                this.displaySuccess(result.message || "Request successful");
                this.emit('form-submit-success', result);
            } else {
                this.displayError(result.message || "Request failed");
                this.emit('form-submit-error', result);
            }

            return result;
        } catch (error) {
            console.error("[EtherUX] Form submission error:", error);
            this.displayError(error.message);
            this.emit('form-submit-error', error);
        }
    }

    /**
     * Execute action
     */
    async executeAction(action, element) {
        try {
            const [method, endpoint] = action.split(':');
            const params = element.dataset.params ? JSON.parse(element.dataset.params) : {};

            console.log(`[EtherUX] Executing action: ${method} ${endpoint}`);

            const result = await this.request(method, endpoint, params);
            this.emit(`action-${action}`, result);

            return result;
        } catch (error) {
            console.error("[EtherUX] Action execution error:", error);
            this.displayError(error.message);
        }
    }

    /**
     * Display success message
     */
    displaySuccess(message) {
        console.log(`[EtherUX] Success: ${message}`);
        
        const notification = document.createElement('div');
        notification.className = 'ether-notification success';
        notification.textContent = message;
        document.body.appendChild(notification);

        setTimeout(() => notification.remove(), 3000);
    }

    /**
     * Display error message
     */
    displayError(message) {
        console.error(`[EtherUX] Error: ${message}`);
        
        const notification = document.createElement('div');
        notification.className = 'ether-notification error';
        notification.textContent = message;
        document.body.appendChild(notification);

        setTimeout(() => notification.remove(), 5000);
    }

    /**
     * Emit custom event
     */
    emit(eventName, detail) {
        const event = new CustomEvent(eventName, { detail });
        document.dispatchEvent(event);

        if (this.listeners[eventName]) {
            this.listeners[eventName].forEach(callback => callback(detail));
        }
    }

    /**
     * Listen to custom event
     */
    on(eventName, callback) {
        if (!this.listeners[eventName]) {
            this.listeners[eventName] = [];
        }
        this.listeners[eventName].push(callback);
    }

    /**
     * Cache data
     */
    setCache(key, value, ttl = 3600000) {
        this.cache.set(key, {
            value: value,
            expires: Date.now() + ttl
        });
    }

    /**
     * Get cached data
     */
    getCache(key) {
        const item = this.cache.get(key);
        
        if (!item) return null;
        
        if (item.expires < Date.now()) {
            this.cache.delete(key);
            return null;
        }

        return item.value;
    }

    /**
     * Clear cache
     */
    clearCache() {
        this.cache.clear();
    }
}

// Initialize on page load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.EtherUX = new EtherUX();
    });
} else {
    window.EtherUX = new EtherUX();
}
