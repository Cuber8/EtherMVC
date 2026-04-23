// EtherChemistery.js - Simplified Route & Controller Pattern
// Combines controller and route logic into a single, secure, and easy-to-use pattern

class EtherChemistery {
    constructor() {
        this.routes = new Map();
        this.middleware = [];
        this.errorHandlers = [];
        console.log("[EtherChemistery] Initializing simplified route pattern...");
    }

    /**
     * Define a route with handler
     * @param {string} method - HTTP method (GET, POST, PUT, DELETE)
     * @param {string} path - Route path
     * @param {object} handler - Handler object with validation and execution
     */
    define(method, path, handler) {
        try {
            if (typeof handler !== 'object' || !handler.execute) {
                throw new Error("Handler must be an object with an 'execute' method");
            }

            const routeKey = `${method.toUpperCase()}:${path}`;
            
            if (this.routes.has(routeKey)) {
                console.warn(`[EtherChemistery] Route '${routeKey}' already defined, overwriting...`);
            }

            this.routes.set(routeKey, {
                method: method.toUpperCase(),
                path: path,
                handler: handler,
                created: new Date().toISOString()
            });

            console.log(`[EtherChemistery] Route defined: ${routeKey}`);
            return this;
        } catch (error) {
            console.error(`[EtherChemistery] Error defining route: ${error.message}`);
            return this;
        }
    }

    /**
     * GET route shorthand
     */
    get(path, handler) {
        return this.define('GET', path, handler);
    }

    /**
     * POST route shorthand
     */
    post(path, handler) {
        return this.define('POST', path, handler);
    }

    /**
     * PUT route shorthand
     */
    put(path, handler) {
        return this.define('PUT', path, handler);
    }

    /**
     * DELETE route shorthand
     */
    delete(path, handler) {
        return this.define('DELETE', path, handler);
    }

    /**
     * Add middleware function
     */
    use(middlewareFunction) {
        if (typeof middlewareFunction === 'function') {
            this.middleware.push(middlewareFunction);
            console.log("[EtherChemistery] Middleware added");
        }
        return this;
    }

    /**
     * Add error handler
     */
    onError(errorHandler) {
        if (typeof errorHandler === 'function') {
            this.errorHandlers.push(errorHandler);
            console.log("[EtherChemistery] Error handler registered");
        }
        return this;
    }

    /**
     * Match and execute route
     */
    async match(method, path, request = {}) {
        try {
            const routeKey = `${method.toUpperCase()}:${path}`;
            const route = this.routes.get(routeKey);

            if (!route) {
                throw new Error(`Route not found: ${routeKey}`);
            }

            // Execute middleware
            for (const mw of this.middleware) {
                await mw(request);
            }

            // Execute route handler
            const handler = route.handler;
            
            // Validate request if validator exists
            if (handler.validate && !handler.validate(request)) {
                throw new Error("Request validation failed");
            }

            // Execute handler
            const result = await handler.execute(request);
            
            return {
                success: true,
                data: result,
                timestamp: new Date().toISOString()
            };
        } catch (error) {
            return this.handleError(error, method, path);
        }
    }

    /**
     * Handle errors
     */
    handleError(error, method, path) {
        let handled = false;

        for (const handler of this.errorHandlers) {
            if (handler(error, method, path)) {
                handled = true;
                break;
            }
        }

        if (!handled) {
            console.error(`[EtherChemistery] Unhandled error on ${method} ${path}: ${error.message}`);
        }

        return {
            success: false,
            error: error.message,
            route: `${method}:${path}`,
            timestamp: new Date().toISOString()
        };
    }

    /**
     * Get all defined routes
     */
    getRoutes() {
        const routes = [];
        this.routes.forEach((route) => {
            routes.push({
                method: route.method,
                path: route.path,
                created: route.created
            });
        });
        return routes;
    }

    /**
     * Create a secure handler template
     */
    static createHandler(config) {
        return {
            name: config.name || 'handler',
            description: config.description || '',
            
            validate: function(request) {
                if (config.requiredParams) {
                    for (const param of config.requiredParams) {
                        if (!(param in request)) {
                            console.warn(`[EtherChemistery] Missing required parameter: ${param}`);
                            return false;
                        }
                    }
                }
                return true;
            },

            execute: async function(request) {
                try {
                    return await config.logic(request);
                } catch (error) {
                    console.error(`[EtherChemistery] Handler error: ${error.message}`);
                    throw error;
                }
            }
        };
    }
}

// Export for Node.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = EtherChemistery;
}

// ===== EXAMPLE USAGE =====
/*
const router = new EtherChemistery();

// Add logging middleware
router.use((request) => {
    console.log(`[Request] ${request.method || 'UNKNOWN'} ${request.path || 'UNKNOWN'}`);
});

// Define routes using handlers
router.get('/users', EtherChemistery.createHandler({
    name: 'getUsers',
    description: 'Fetch all users',
    logic: async (request) => {
        return { message: 'Users fetched successfully', users: [] };
    }
}));

router.post('/users', EtherChemistery.createHandler({
    name: 'createUser',
    description: 'Create a new user',
    requiredParams: ['name', 'email'],
    logic: async (request) => {
        return { message: 'User created', id: 1 };
    }
}));

// Error handler
router.onError((error, method, path) => {
    console.error(`[Error] ${method} ${path}: ${error.message}`);
    return true;
});

// Use routes
(async () => {
    const result = await router.match('GET', '/users', {});
    console.log(result);
})();
*/
