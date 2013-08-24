<?php

/**
 * A MySQLi based database adapter.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
class DbAdapter_MySQLi extends DbAdapterBase {
	/**
	 * The current MySQLi connection.
	 * 
	 * @var mysqli
	 */
	private $_conn;
	
	
	/**
	 * Initializes a new instance of that class.
	 *
	 * @param array $conf The configuration data for that instance.
	 */
    public function __construct(array $conf = array()) {
    	if (!isset($conf['host'])) {
    		$conf['host'] = '127.0.0.1';
    	}
    	
    	if (!isset($conf['port'])) {
    		$conf['port'] = 3306;
    	}
    	
    	if (!isset($conf['db'])) {
    		$conf['db'] = 'test';
    	}
    	
    	if (!isset($conf['user'])) {
    		$conf['user'] = 'root';
    	}
    	
    	if (!isset($conf['password'])) {
    		$conf['password'] = null;
    	}
    	
    	if (!isset($conf['socket'])) {
    		$conf['socket'] = null;
    	}
    	
    	parent::__construct($conf);
    	
    	$this->_conn = new mysqli($conf['host'],
    			                  $conf['user'], $conf['password'],
    			                  $conf['db'],
    			                  $conf['port'],
                                  $conf['socket']);
    }
    
    /**
     * (non-PHPdoc)
     * @see IDbAdapter::close()
     */
    public function close() {
    	if (!is_null($this->_conn)) {
    		$this->_conn->close();
    		
    		$this->_conn = null;
    		return true;
    	}
    	
    	return false;
    }
    
    /**
     * Gets the underlying MySQLi connection.
     * 
     * @return mysqli The underlying connection or (null)
     *                if no connection is available anymore.
     */
    public function getConnection() {
    	return $this->_conn;
    }
    
    /**
     * (non-PHPdoc)
     * @see IDbAdapter::getEntities()
     */
    public function getEntities() {
    	$result = array();
    	
    	$res = $this->getConnection()->query('SHOW TABLES;');
    	while ($row = $res->fetch_row()) {
    		$newEntity = new Entity(
    			$this,
    			array(
    			    'name' => trim($row[0]),
    			)
    		);
    		
    		$result[] = $newEntity;
    	}
    	$res->close();
    	
    	return $result;
    }
    
    /**
     * (non-PHPdoc)
     * @see IDbAdapter::getEntityAttributes()
     */
    public function getEntityAttributes(IEntity $entity) {
    	$result = array();
    	 
    	$ord = 0;
    	$res = $this->getConnection()->query(sprintf('DESCRIBE %s;',
    	                                             trim($entity->getName())));
    	while ($row = $res->fetch_assoc()) {
    		$type = trim(strtolower($row['Type']));
    		
    		$phpType = 'mixed';
    		if ((0 === strpos($type, 'varchar')) ||
    	        (0 === strpos($type, 'text'))) {
    			
    			$phpType = 'string';
    		}
    		else if (0 === strpos($type, 'bit')) {
    			$phpType = 'boolean';
    		}
    		
    		$newAttrib = new EntityAttribute(
    			$entity,
    			array(
    			    'name'    => trim($row['Field']),
    				'ordinal' => $ord++,
    				'phpType' => $phpType,
    				'type'    => $type,
    			)
    		);
    		
    		$result[] = $newAttrib;
    	}
    	$res->close();
    	 
    	return $result;
    }
}
