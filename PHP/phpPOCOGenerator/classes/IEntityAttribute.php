<?php

/**
 * Describes an attribute of an entity.
 * 
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
interface IEntityAttribute {
	/**
	 * Gets the underlying entity.
	 *
	 * @return IEntity The underlying entity.
	 */
	public function getEntity();
	
	/**
	 * Gets the name of that attribute.
	 * 
	 * @return string The name of that attribute.
	 */
	public function getName();
	
	/**
	 * Gets the zero-based index of that attribute.
	 *
	 * @return integer The zero-based index of that attribute.
	 */
	public function getOrdinal();
}
