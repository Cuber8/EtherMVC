<?php
/**
 * vendor/php/DatabaseUtil.php - Database Utilities
 * Connection pooling and query helpers
 */

class DatabaseUtil {
    private static $connections = [];
    private static $config = [];

    /**
     * Initialize database configuration
     */
    public static function initialize($config) {
        self::$config = $config;
    }

    /**
     * Get database connection
     */
    public static function getConnection($name = 'default') {
        if (!isset(self::$connections[$name])) {
            $cfg = self::$config[$name] ?? self::$config['default'];
            
            try {
                $dsn = sprintf(
                    'mysql:host=%s;port=%d;dbname=%s;charset=utf8mb4',
                    $cfg['host'] ?? 'localhost',
                    $cfg['port'] ?? 3306,
                    $cfg['database'] ?? 'ethermvc'
                );
                
                $pdo = new PDO(
                    $dsn,
                    $cfg['username'] ?? 'root',
                    $cfg['password'] ?? '',
                    [
                        PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
                        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
                        PDO::ATTR_PERSISTENT => false
                    ]
                );
                
                self::$connections[$name] = $pdo;
            } catch (PDOException $e) {
                throw new Exception("Database connection failed: " . $e->getMessage());
            }
        }
        
        return self::$connections[$name];
    }

    /**
     * Execute query
     */
    public static function query($sql, $params = [], $connection = 'default') {
        $pdo = self::getConnection($connection);
        $stmt = $pdo->prepare($sql);
        $stmt->execute($params);
        return $stmt;
    }

    /**
     * Fetch one
     */
    public static function fetchOne($sql, $params = [], $connection = 'default') {
        $stmt = self::query($sql, $params, $connection);
        return $stmt->fetch();
    }

    /**
     * Fetch all
     */
    public static function fetchAll($sql, $params = [], $connection = 'default') {
        $stmt = self::query($sql, $params, $connection);
        return $stmt->fetchAll();
    }

    /**
     * Insert record
     */
    public static function insert($table, $data, $connection = 'default') {
        $columns = array_keys($data);
        $values = array_values($data);
        $placeholders = array_fill(0, count($columns), '?');
        
        $sql = sprintf(
            'INSERT INTO `%s` (%s) VALUES (%s)',
            $table,
            '`' . implode('`, `', $columns) . '`',
            implode(', ', $placeholders)
        );
        
        self::query($sql, $values, $connection);
        
        return self::getConnection($connection)->lastInsertId();
    }

    /**
     * Update record
     */
    public static function update($table, $data, $where, $connection = 'default') {
        $sets = [];
        $values = [];
        
        foreach ($data as $key => $value) {
            $sets[] = "`$key` = ?";
            $values[] = $value;
        }
        
        $whereConditions = [];
        foreach ($where as $key => $value) {
            $whereConditions[] = "`$key` = ?";
            $values[] = $value;
        }
        
        $sql = sprintf(
            'UPDATE `%s` SET %s WHERE %s',
            $table,
            implode(', ', $sets),
            implode(' AND ', $whereConditions)
        );
        
        $stmt = self::query($sql, $values, $connection);
        return $stmt->rowCount();
    }

    /**
     * Delete record
     */
    public static function delete($table, $where, $connection = 'default') {
        $whereConditions = [];
        $values = [];
        
        foreach ($where as $key => $value) {
            $whereConditions[] = "`$key` = ?";
            $values[] = $value;
        }
        
        $sql = sprintf(
            'DELETE FROM `%s` WHERE %s',
            $table,
            implode(' AND ', $whereConditions)
        );
        
        $stmt = self::query($sql, $values, $connection);
        return $stmt->rowCount();
    }
}
?>
