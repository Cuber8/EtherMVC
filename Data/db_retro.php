<?php
/**
 * db_retro.php - Retro Database Connection Manager
 * Handles MySQL/MariaDB connections for traditional MVC architecture
 * Includes basic security practices
 */

class RetroDatabase {
    private $host;
    private $user;
    private $password;
    private $database;
    private $connection;
    private $lastError;
    private $preparedStatements = [];

    /**
     * Constructor - Initialize database connection
     */
    public function __construct($config = []) {
        $this->host = $config['host'] ?? 'localhost';
        $this->user = $config['user'] ?? 'root';
        $this->password = $config['password'] ?? '';
        $this->database = $config['database'] ?? 'ethermvc';
        
        echo "[RetroDatabase] Initializing MySQL connection...\n";
        $this->connect();
    }

    /**
     * Connect to database
     */
    private function connect() {
        try {
            $this->connection = new mysqli(
                $this->host,
                $this->user,
                $this->password,
                $this->database
            );

            if ($this->connection->connect_error) {
                throw new Exception("Connection failed: " . $this->connection->connect_error);
            }

            $this->connection->set_charset("utf8mb4");
            echo "[RetroDatabase] Connected successfully\n";
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            echo "[RetroDatabase] Error: " . $this->lastError . "\n";
        }
    }

    /**
     * Execute a prepared statement query
     */
    public function query($sql, $params = []) {
        try {
            $stmt = $this->connection->prepare($sql);
            
            if (!$stmt) {
                throw new Exception("Prepare failed: " . $this->connection->error);
            }

            if (!empty($params)) {
                $types = $this->getParameterTypes($params);
                $stmt->bind_param($types, ...$params);
            }

            if (!$stmt->execute()) {
                throw new Exception("Execute failed: " . $stmt->error);
            }

            $result = $stmt->get_result();
            return $result;
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            echo "[RetroDatabase] Query error: " . $this->lastError . "\n";
            return false;
        }
    }

    /**
     * Insert data
     */
    public function insert($table, $data) {
        try {
            $columns = array_keys($data);
            $values = array_values($data);
            $placeholders = array_fill(0, count($columns), '?');

            $sql = "INSERT INTO " . $this->escapeIdentifier($table) . " (" . 
                   implode(',', array_map([$this, 'escapeIdentifier'], $columns)) . 
                   ") VALUES (" . implode(',', $placeholders) . ")";

            $result = $this->query($sql, $values);
            
            if ($result === false) {
                return false;
            }

            return $this->connection->insert_id;
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            return false;
        }
    }

    /**
     * Update data
     */
    public function update($table, $data, $where = '') {
        try {
            $sets = [];
            $values = [];

            foreach ($data as $column => $value) {
                $sets[] = $this->escapeIdentifier($column) . "=?";
                $values[] = $value;
            }

            $sql = "UPDATE " . $this->escapeIdentifier($table) . " SET " . 
                   implode(',', $sets);

            if (!empty($where)) {
                $sql .= " WHERE " . $where;
            }

            return $this->query($sql, $values);
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            return false;
        }
    }

    /**
     * Delete data
     */
    public function delete($table, $where = '') {
        try {
            $sql = "DELETE FROM " . $this->escapeIdentifier($table);

            if (!empty($where)) {
                $sql .= " WHERE " . $where;
            }

            return $this->connection->query($sql);
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            return false;
        }
    }

    /**
     * Select data
     */
    public function select($table, $where = '', $limit = '') {
        try {
            $sql = "SELECT * FROM " . $this->escapeIdentifier($table);

            if (!empty($where)) {
                $sql .= " WHERE " . $where;
            }

            if (!empty($limit)) {
                $sql .= " LIMIT " . intval($limit);
            }

            $result = $this->connection->query($sql);
            $data = [];

            if ($result) {
                while ($row = $result->fetch_assoc()) {
                    $data[] = $row;
                }
            }

            return $data;
        } catch (Exception $e) {
            $this->lastError = $e->getMessage();
            return [];
        }
    }

    /**
     * Escape SQL identifiers (table/column names)
     */
    private function escapeIdentifier($identifier) {
        return "`" . str_replace("`", "``", $identifier) . "`";
    }

    /**
     * Get parameter types for prepared statements
     */
    private function getParameterTypes($params) {
        $types = '';
        foreach ($params as $param) {
            if (is_int($param)) {
                $types .= 'i';
            } elseif (is_float($param)) {
                $types .= 'd';
            } elseif (is_string($param)) {
                $types .= 's';
            } else {
                $types .= 's';
            }
        }
        return $types;
    }

    /**
     * Get last error
     */
    public function getLastError() {
        return $this->lastError;
    }

    /**
     * Close connection
     */
    public function close() {
        if ($this->connection) {
            $this->connection->close();
            echo "[RetroDatabase] Connection closed\n";
        }
    }

    /**
     * Destructor - Close connection
     */
    public function __destruct() {
        $this->close();
    }
}

// Example usage:
/*
$db = new RetroDatabase([
    'host' => 'localhost',
    'user' => 'root',
    'password' => '',
    'database' => 'ethermvc'
]);

$users = $db->select('users', 'active=1', 10);
print_r($users);
*/
?>
