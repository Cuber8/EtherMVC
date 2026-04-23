// Controller_example.js - Example Controller for Retro Version
// This demonstrates how to create controllers in the traditional MVC pattern

class ControllerExample {
    constructor() {
        this.name = "ControllerExample";
        console.log("[ControllerExample] Controller initialized");
    }

    /**
     * Handle GET request - Fetch data
     */
    async getUsers(request) {
        try {
            console.log("[ControllerExample] Getting users...");
            
            // Simulate database query
            const users = [
                { id: 1, name: "John Doe", email: "john@example.com" },
                { id: 2, name: "Jane Smith", email: "jane@example.com" },
                { id: 3, name: "Bob Johnson", email: "bob@example.com" }
            ];

            return {
                success: true,
                data: users,
                message: "Users fetched successfully",
                count: users.length
            };
        } catch (error) {
            console.error("[ControllerExample] Error fetching users:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Handle POST request - Create user
     */
    async createUser(request) {
        try {
            console.log("[ControllerExample] Creating user...");
            
            // Validate input
            if (!request.name || !request.email) {
                return {
                    success: false,
                    error: "Name and email are required"
                };
            }

            // Simulate database insert
            const newUser = {
                id: 4,
                name: request.name,
                email: request.email,
                created: new Date().toISOString()
            };

            return {
                success: true,
                data: newUser,
                message: "User created successfully"
            };
        } catch (error) {
            console.error("[ControllerExample] Error creating user:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Handle PUT request - Update user
     */
    async updateUser(request) {
        try {
            console.log("[ControllerExample] Updating user...");
            
            if (!request.id) {
                return {
                    success: false,
                    error: "User ID is required"
                };
            }

            const updatedUser = {
                id: request.id,
                name: request.name || "Updated User",
                email: request.email || "updated@example.com",
                updated: new Date().toISOString()
            };

            return {
                success: true,
                data: updatedUser,
                message: "User updated successfully"
            };
        } catch (error) {
            console.error("[ControllerExample] Error updating user:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Handle DELETE request - Delete user
     */
    async deleteUser(request) {
        try {
            console.log("[ControllerExample] Deleting user...");
            
            if (!request.id) {
                return {
                    success: false,
                    error: "User ID is required"
                };
            }

            return {
                success: true,
                message: `User ${request.id} deleted successfully`,
                id: request.id
            };
        } catch (error) {
            console.error("[ControllerExample] Error deleting user:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }

    /**
     * Handle dashboard view
     */
    async getDashboard(request) {
        try {
            console.log("[ControllerExample] Loading dashboard...");
            
            const dashboard = {
                title: "Dashboard",
                stats: {
                    totalUsers: 3,
                    activeUsers: 2,
                    newUsersThisWeek: 1
                },
                recentActivity: [
                    "User login",
                    "Data update",
                    "System check"
                ]
            };

            return {
                success: true,
                data: dashboard,
                message: "Dashboard loaded successfully"
            };
        } catch (error) {
            console.error("[ControllerExample] Error loading dashboard:", error);
            return {
                success: false,
                error: error.message
            };
        }
    }
}

// Export for Node.js
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ControllerExample;
}
