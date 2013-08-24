<?php

/**
 * A common entity.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
final class Entity implements IEntity {
	private $_dbAdapter;
	private $_name;
	
	/**
	 * Initializes a new instance of that class.
	 * 
	 * @param IDbAdapter $dbAdapter The underlying database adapter.
	 * @param array $conf The configuration data for that instance.
	 */
	public function __construct(IDbAdapter $dbAdapter,
                                array $conf = array()) {
		
		$this->_dbAdapter = $dbAdapter;
		
		if (isset($conf['name'])) {
			$this->_name = $conf['name'];
		}
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntity::getAdapter()
	 */
	public function getAdapter() {
		return $this->_dbAdapter;
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntity::getAttributes()
	 */
	public function getAttributes() {
		return $this->getAdapter()
		            ->getEntityAttributes($this);
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntity::getName()
	 */
	public function getName() {
		return $this->_name;
	}
}
