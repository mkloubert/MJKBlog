<?php

/**
 * A MySQLi based database adapter.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
class DbAdapter_SQLSrv extends DbAdapterBase {
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
    		$conf['host'] = 'localhost\SQLEXPRESS';
    	}
    	
    	$connectionInfo = array();

    	if (isset($conf['user'])) {
    		$connectionInfo['UID'] = $conf['user'];
    	}
    	
    	if (isset($conf['password'])) {
    		$connectionInfo['PWD'] = $conf['password'];
    	}
    	
    	if (isset($conf['db'])) {
    		$connectionInfo['Database'] = $conf['db'];
    	}
    	
    	parent::__construct($conf);
    	
    	$this->_conn = sqlsrv_connect($conf['host'],
    			                      $connectionInfo); 	
    }
    
    /**
     * (non-PHPdoc)
     * @see IDbAdapter::close()
     */
    public function close() {
    	if (!is_null($this->_conn)) {
    		sqlsrv_close($this->_conn);
    		$this->_conn = null;
    		
    		return true;
    	}
    	
    	return false;
    }
    
    /**
     * Gets the underlying MySQLi connection.
     * 
     * @return resource The underlying connection or (null)
     *                  if no connection is available anymore.
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
    	
    	$res = sqlsrv_query($this->_conn, 'SELECT name ' . 
    	                                  'FROM sys.Tables ' . 
    	                                  "WHERE name <> N'sysdiagrams'; ");
    	while ($row = sqlsrv_fetch_array($res, SQLSRV_FETCH_ASSOC)) {
    		$newEntity = new Entity(
    			$this,
    			array(
    			    'name' => trim($row['name']),
    			)
    		);
    		
    		$result[] = $newEntity;
    	}
    	sqlsrv_free_stmt($res);
    	
    	return $result;
    }
    
    /**
     * (non-PHPdoc)
     * @see IDbAdapter::getEntityAttributes()
     */
    public function getEntityAttributes(IEntity $entity) {
    	$result = array();

    	$res = sqlsrv_query($this->_conn,
    			            sprintf('SELECT COLUMN_NAME, ORDINAL_POSITION, DATA_TYPE ' . 
    			            		'FROM INFORMATION_SCHEMA.COLUMNS ' . 
    			            		'WHERE TABLE_NAME = N\'%s\'; ',
    	                            trim($entity->getName())));
    	while ($row = sqlsrv_fetch_array($res, SQLSRV_FETCH_ASSOC)) {
    		$type = trim(strtolower($row['DATA_TYPE']));
    		
    		$phpType = 'mixed';
    		if (0 === strpos($type, 'nvarchar')) {
    			$phpType = 'string';
    		}
    		
    		$newAttrib = new EntityAttribute(
    			$entity,
    			array(
    			    'name'    => trim($row['COLUMN_NAME']),
    				'ordinal' => intval($row['ORDINAL_POSITION']) - 1,
    				'phpType' => $phpType,
    				'type'    => $type,
    			)
    		);
    		
    		$result[] = $newAttrib;
    	}
    	sqlsrv_free_stmt($res);
    	 
    	return $result;
    }
}
