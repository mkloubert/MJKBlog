<?php

/**
 * A basic database adapter.
 *
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 */
abstract class DbAdapterBase implements IDbAdapter {
	private $_config;
	
	/**
	 * Initializes a new instance of that class.
	 *
	 * @param array $conf The configuration data for that instance.
	 */
	protected function __construct(array $conf = array()) {
		$this->_config = $conf;
	}
	
	/**
	 * (non-PHPdoc)
	 * @see IDbAdapter::getConfig()
	 */
	public function getConfig() {
		return $this->_config;
	}
}
