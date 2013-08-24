<?php

// s. http://blog.marcel-kloubert.de/


define('DIR_ROOT'   , './'                 , true);
define('DIR_CLASSES', DIR_ROOT . 'classes/', true);
define('DIR_PLUGINS', DIR_ROOT . 'plugins/', true);

define('FILE_DEBUG_SCRIPT', DIR_ROOT . 'debug.php', true);

function __autoload($className) {
	require_once DIR_CLASSES .
                 str_replace('_', '/', $className) .
                 '.php';
}

function nameableObjectCmp($x, $y) {
	$strX = null;
	if (!is_null($x)) {
		$strX = trim(strtolower($x->getName()));
	}
	
	$strY = null;
	if (!is_null($y)) {
		$strY = trim(strtolower($y->getName()));
	}
	
	return strcmp($strX, $strY);
}



define('DEBUG_MODE', true, true);
if (DEBUG_MODE &&
    file_exists(FILE_DEBUG_SCRIPT)) {
	
	require_once FILE_DEBUG_SCRIPT;
}


if (!isset($dbAdapterName)) {
	$dbAdapterName = 'MySQL';    //TODO: read from command line arguments
}
require_once DIR_PLUGINS . sprintf('db.%s.php',
                                   trim(strtolower($dbAdapterName)));

if (!isset($dbAdapterConf)) {
	$dbAdapterConf = array();    //TODO: read from command line arguments
}
$dbAdapter = eval(sprintf('return new DbAdapter_%s($dbAdapterConf);',
                          $dbAdapterName));


define('DIR_OUT', DIR_ROOT . 'out/', true);    //TODO: read from command line arguments
if (!file_exists(DIR_OUT)) {
	mkdir(DIR_OUT);
}


$entities = $dbAdapter->getEntities();
usort($entities, 'nameableObjectCmp');

foreach ($entities as $e) {
	$outFile = DIR_OUT . sprintf('poco.%s.php', $e->getName());
	if (file_exists($outFile)) {
		unlink($outFile);
	}
	
	$fh = fopen($outFile, 'w');

	$entityName = $e->getName();
	$entityClassName = sprintf('POCO_%s',
	                           trim($entityName));
	
	echo $e->getName() . "\r\n";
	
	fwrite($fh, "<?php\n");
	fwrite($fh, "\n");
	fwrite($fh, "/**\n");
	fwrite($fh, sprintf(' * A POCO for the \'%s\' entity.' . "\n",
	                    $entityName));
	fwrite($fh, " * \n");
	fwrite($fh, " * @link http://blog.marcel-kloubert.de/ Generated with phpPOCOGenerator by Marcel J. Kloubert\n");
	fwrite($fh, " */\n");
	fwrite($fh, sprintf("class %s {\n",
	                    $entityClassName));
	
	fwrite($fh, "    /**\n");
	fwrite($fh, "     * Stores all attributes and their values of that POCO.\n");
	fwrite($fh, "     */\n");
	fwrite($fh, '    protected $_attributes = array();' . "\n");
	fwrite($fh, "\n");
	fwrite($fh, "\n");
	
	fwrite($fh, "    /**\n");
	fwrite($fh, "     * Initializes a new instance of that class.\n");
	fwrite($fh, "     * \n");
	fwrite($fh, '     * @param array $attribs The list of attributes and values to initialize.' . "\n");
	fwrite($fh, "     */\n");
    fwrite($fh, '    public function __construct(array $attribs = array()) {' . "\n");
	fwrite($fh, '        foreach ($attribs as $name => $value) {' . "\n");
	fwrite($fh, '            $this->_attributes[$name] = $value;' . "\n");
	fwrite($fh, '        }' . "\n");
	fwrite($fh, '    }' . "\n");
	fwrite($fh, "\n");
	
	$attribs = $e->getAttributes();
	usort($attribs, 'nameableObjectCmp');
	
	// getters
	foreach ($attribs as $a) {
		$phpType = trim($a->getPhpType());
		
		fwrite($fh, "\n");
		fwrite($fh, "    /**\n");
		fwrite($fh, sprintf('     * Gets the current value of \'%s\' attribute.' . "\n",
	                        $a->getName()));
		fwrite($fh, "     * \n");
		fwrite($fh, sprintf('     * @return %s The current value.' . "\n",
		                    $phpType));
		fwrite($fh, "     */\n");
		
		fwrite($fh, sprintf('    public function get_%s() {' . "\n",
	                        $a->getName()));
		
		fwrite($fh, sprintf('        return $this->_attributes[\'%s\'];' . "\n",
	                        $a->getName()));
		
		fwrite($fh, "    }\n");
	}
	
	// issetters
	foreach ($attribs as $a) {
		$phpType = trim($a->getPhpType());
	
		fwrite($fh, "\n");
		fwrite($fh, "    /**\n");
		fwrite($fh, sprintf('     * Checks if the \'%s\' attribute is set / defined or not.' . "\n",
		                    $a->getName()));
		fwrite($fh, "     * \n");
		fwrite($fh, "     * @return boolean Is set / defined (true) or not (false).\n");
		fwrite($fh, "     */\n");
	
		fwrite($fh, sprintf('    public function isset_%s() {' . "\n",
		                    $a->getName()));
	
		fwrite($fh, sprintf('        return isset($this->_attributes[\'%s\']);' . "\n",
		                    $a->getName()));
	
		fwrite($fh, "    }\n");
	}
	
	// setters
	foreach ($attribs as $a) {
		fwrite($fh, "\n");
		fwrite($fh, "    /**\n");
		fwrite($fh, sprintf('     * Sets the current value of \'%s\' attribute.' . "\n",
		$a->getName()));
		fwrite($fh, "     * \n");
		fwrite($fh, sprintf('     * @return %s That POCO instance.' . "\n",
	                        $entityClassName));
		fwrite($fh, "     */\n");
		fwrite($fh, sprintf('    public function set_%s($value) {' . "\n",
		                    $a->getName()));
		
		fwrite($fh, sprintf('        $this->_attributes[\'%s\'] = $value;' . "\n",
	                        $a->getName()));
		fwrite($fh, '        return $this;' . "\n");
		
		fwrite($fh, "    }\n");
	}
	
	fwrite($fh, "}\n");
	
	fclose($fh);
}

$dbAdapter->close();
