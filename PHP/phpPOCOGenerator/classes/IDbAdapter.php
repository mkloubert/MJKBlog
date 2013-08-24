<?php

/**
 * Describes 
 * 
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
interface IDbAdapter {
	/**
	 * Closes the underlying connection of that adapter.
	 */
	public function close();
	
	/**
	 * Gets the underlying configuration data array.
	 * 
	 * @return array The underlying configuration data.
	 */
	public function getConfig();
	
	/**
	 * Reads the list of entites.
	 *
	 * @return array The list of entities.
	 */
	public function getEntities();
	
	/**
	 * Reads the list of attributes of an entity.
	 * 
	 * @param IEntity $entity The underlying entity.
	 *
	 * @return array The list of entity attributes.
	 */
	public function getEntityAttributes(IEntity $entity);
}
