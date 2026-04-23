// index.jsx - Main View Renderer
// Displays information from the selected page and setup

class EtherView {
    constructor(config = {}) {
        this.config = config;
        this.currentPage = null;
        this.components = new Map();
        this.state = {};
        console.log("[EtherView] Initializing Ether View renderer...");
    }

    /**
     * Render a page
     */
    renderPage(pageName, data = {}) {
        try {
            console.log(`[EtherView] Rendering page: ${pageName}`);
            
            const pageElement = document.getElementById('app') || document.body;
            
            const html = this.buildHTML(pageName, data);
            pageElement.innerHTML = html;
            
            this.currentPage = pageName;
            this.state = data;
            
            // Load page-specific scripts
            this.loadPageScripts(pageName);
            
            return true;
        } catch (error) {
            console.error(`[EtherView] Render error: ${error.message}`);
            return false;
        }
    }

    /**
     * Build HTML for page
     */
    buildHTML(pageName, data) {
        let html = '<div class="ether-page">';
        
        // Add layout
        html += this.getLayout('basic');
        
        // Add page content
        html += `<div class="page-content" data-page="${pageName}">`;
        html += this.renderPageContent(pageName, data);
        html += '</div>';
        
        html += '</div>';
        return html;
    }

    /**
     * Get layout template
     */
    getLayout(layoutName) {
        const layouts = {
            basic: `
                <header class="ether-header">
                    <nav class="navbar">
                        <div class="nav-container">
                            <div class="nav-brand">EtherMVC</div>
                            <ul class="nav-menu">
                                <li><a href="/">Home</a></li>
                                <li><a href="/about">About</a></li>
                                <li><a href="/contact">Contact</a></li>
                            </ul>
                        </div>
                    </nav>
                </header>
                <aside class="sidebar">
                    <div class="sidebar-content">
                        <h3>Navigation</h3>
                        <ul>
                            <li><a href="/dashboard">Dashboard</a></li>
                            <li><a href="/settings">Settings</a></li>
                            <li><a href="/help">Help</a></li>
                        </ul>
                    </div>
                </aside>
                <main class="main-content">
            `
        };
        
        return layouts[layoutName] || '';
    }

    /**
     * Render page content
     */
    renderPageContent(pageName, data) {
        const pages = {
            home: `
                <h1>Welcome to EtherMVC</h1>
                <p>A secure framework for building websites and applications.</p>
                <button class="btn btn-primary">Get Started</button>
            `,
            dashboard: `
                <h1>Dashboard</h1>
                <div class="dashboard-grid">
                    <div class="card">
                        <h3>Statistics</h3>
                        <p>Your application statistics will appear here.</p>
                    </div>
                    <div class="card">
                        <h3>Recent Activity</h3>
                        <p>Recent activities will be displayed here.</p>
                    </div>
                </div>
            `,
            example: `
                <h1>Example Page</h1>
                <div class="example-form">
                    <form data-ether-form>
                        <div class="form-group">
                            <label for="name">Name</label>
                            <input type="text" id="name" name="name" required>
                        </div>
                        <div class="form-group">
                            <label for="email">Email</label>
                            <input type="email" id="email" name="email" required>
                        </div>
                        <button type="submit" class="btn btn-primary">Submit</button>
                    </form>
                </div>
            `,
            notfound: `
                <h1>Page Not Found</h1>
                <p>The page you're looking for doesn't exist.</p>
                <a href="/" class="btn btn-primary">Go Home</a>
            `
        };

        return pages[pageName] || pages.notfound;
    }

    /**
     * Load page-specific scripts
     */
    loadPageScripts(pageName) {
        // Initialize page-specific functionality
        const scripts = {
            home: () => this.initHomePage(),
            dashboard: () => this.initDashboard(),
            example: () => this.initExamplePage()
        };

        if (scripts[pageName]) {
            scripts[pageName]();
        }
    }

    /**
     * Initialize home page
     */
    initHomePage() {
        const button = document.querySelector('.btn-primary');
        if (button) {
            button.addEventListener('click', () => {
                console.log("[EtherView] Getting started clicked");
            });
        }
    }

    /**
     * Initialize dashboard
     */
    initDashboard() {
        console.log("[EtherView] Dashboard initialized");
    }

    /**
     * Initialize example page
     */
    initExamplePage() {
        console.log("[EtherView] Example page initialized");
    }

    /**
     * Register custom component
     */
    registerComponent(name, renderFunction) {
        this.components.set(name, renderFunction);
    }

    /**
     * Get current page
     */
    getCurrentPage() {
        return this.currentPage;
    }

    /**
     * Get state
     */
    getState() {
        return { ...this.state };
    }

    /**
     * Update state
     */
    updateState(newState) {
        this.state = { ...this.state, ...newState };
    }
}

// Initialize on page load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.EtherView = new EtherView();
    });
} else {
    window.EtherView = new EtherView();
}
