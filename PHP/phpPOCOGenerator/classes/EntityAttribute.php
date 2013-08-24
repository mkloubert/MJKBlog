<?php

/**
 * A common entity attribute.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
final class EntityAttribute implements IEntityAttribute {
	private $_entity;
	private $_name;
	private $_ordinal;
	
	/**
	 * Initializes a new instance of that class.
	 *
	 * @param IEntity $entity The underlying entity.
	 * @param array $conf The configuration data for that instance.
	 */
	public function __construct(IEntity $entity,
                                array $conf = array()) {
		
		$this->_entity = $entity;
		
		if (isset($conf['name'])) {
			$this->_name = $conf['name'];
		}
		
		if (isset($conf['ordinal'])) {
			$this->_ordinal = $conf['ordinal'];
		}
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntityAttribute::getEntity()
	 */
	public function getEntity() {
		return $this->_entity;
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntityAttribute::getName()
	 */
	public function getName() {
		return $this->_name;
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IEntityAttribute::getOrdinal()
	 */
	public function getOrdinal() {
		return $this->_ordinal;
	}
}
