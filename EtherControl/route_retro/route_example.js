// route_example.js - Example Route for Retro Version
// This demonstrates how to define routes in the traditional MVC pattern

const ControllerExample = require('../Controller_retro/Controller_example.js');

class RouteExample {
    constructor() {
        this.controller = new ControllerExample();
        this.routes = [];
        this.setupRoutes();
    }

    /**
     * Setup all routes
     */
    setupRoutes() {
        console.log("[RouteExample] Setting up routes...");

        // GET /api/users
        this.addRoute('GET', '/api/users', async (request) => {
            return await this.controller.getUsers(request);
        });

        // POST /api/users
        this.addRoute('POST', '/api/users', async (request) => {
            return await this.controller.createUser(request);
        });

        // PUT /api/users/:id
        this.addRoute('PUT', '/api/users/:id', async (request) => {
            return await this.controller.updateUser(request);
        });

        // DELETE /api/users/:id
        this.addRoute('DELETE', '/api/users/:id', async (request) => {
            return await this.controller.deleteUser(request);
        });

        // GET /dashboard
        this.addRoute('GET', '/dashboard', async (request) => {
            return await this.controller.getDashboard(request);
        });

        console.log("[RouteExample] Routes setup complete");
    }

    /**
     * Add a route
     */
    addRoute(method, path, handler) {
        this.routes.push({
            method: method.toUpperCase(),
            path: path,
            handler: handler,
            created: new Date().toISOString()
        });

        console.log(`[RouteExample] Route registered: ${method.toUpperCase()} ${path}`);
    }

    /**
     * Match and execute route
     */
    async matchRoute(method, path, request = {}) {
        try {
            for (const route of this.routes) {
                if (this.isPathMatch(route.path, path) && route.method === method.toUpperCase()) {
                    console.log(`[RouteExample] Matched route: ${method} ${path}`);
                    return await route.handler(request);
                }
            }

            return {
                success: false,
                error: `Route not found: ${method} ${path}`
            };
        } catch (error) {
            console.error("[RouteExample] Route matching error:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Check if path matches route pattern
     */
    isPathMatch(pattern, path) {
        const patternParts = pattern.split('/').filter(p => p);
        const pathParts = path.split('/').filter(p => p);

        if (patternParts.length !== pathParts.length) {
            return false;
        }

        for (let i = 0; i < patternParts.length; i++) {
            const pattern = patternParts[i];
            const pathPart = pathParts[i];

            if (!pattern.startsWith(':') && pattern !== pathPart) {
                return false;
            }
        }

        return true;
    }

    /**
     * Get all routes
     */
    getRoutes() {
        return this.routes.map(r => ({
            method: r.method,
            path: r.path,
            created: r.created
        }));
    }

    /**
     * Display all routes
     */
    displayRoutes() {
        console.log("\n=== Registered Routes ===");
        this.routes.forEach(route => {
            console.log(`${route.method.padEnd(6)} ${route.path}`);
        });
        console.log("========================\n");
    }
}

// Export for Node.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = RouteExample;
}
