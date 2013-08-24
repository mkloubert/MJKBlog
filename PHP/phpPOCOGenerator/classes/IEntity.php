<?php

/**
 * Describes an entity.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
interface IEntity {
	/**
	 * Gets the underlying database adapter.
	 *
	 * @return IDbAdapter The underlying database adapter.
	 */
	public function getAdapter();
	
	/**
	 * Reads the list of attributes of that entity.
	 *
	 * @return array The list of attributes of that entity.
	 */
	public function getAttributes();
	
	/**
	 * Gets the name of that entity.
	 *
	 * @return string The name of that entity.
	 */
	public function getName();
}
