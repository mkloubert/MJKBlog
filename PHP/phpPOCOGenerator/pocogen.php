<?php

// s. http://blog.marcel-kloubert.de/


define('DIR_ROOT'   , './'                 , true);
define('DIR_CLASSES', DIR_ROOT . 'classes/', true);
define('DIR_PLUGINS', DIR_ROOT . 'plugins/', true);

define('FILE_DEBUG_SCRIPT', DIR_ROOT . 'debug.php', true);

/**
 * The class autoloader.
 * 
 * @param string $className The name of the class to load.
 */
function __autoload($className) {
    require_once DIR_CLASSES .
                 str_replace('_', '/', $className) .
                 '.php';
}

/**
 * Comparer function to sort nameable objects by name with ignoring
 * case.
 * 
 * @param mixed $x The left object.
 * @param mixed $y The right object.
 * 
 * @return number The compare value.
 * @see strcmp
 */
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


// DEBUG mode?
define('DEBUG_MODE', true, true);
if (DEBUG_MODE &&
    file_exists(FILE_DEBUG_SCRIPT)) {
    
    require_once FILE_DEBUG_SCRIPT;
}

// include database adapter class
if (!isset($dbAdapterName)) {
    $dbAdapterName = 'MySQL';    //TODO: read from command line arguments
}
require_once DIR_PLUGINS . sprintf('db.%s.php',
                                   trim(strtolower($dbAdapterName)));

// configuration for included database adapter
if (!isset($dbAdapterConf)) {
    $dbAdapterConf = array();    //TODO: read from command line arguments
}
$dbAdapter = eval(sprintf('return new DbAdapter_%s($dbAdapterConf);',
                          $dbAdapterName));


// output directory
define('DIR_OUT', DIR_ROOT . 'out/', true);    //TODO: read from command line arguments
if (!file_exists(DIR_OUT)) {
    mkdir(DIR_OUT);
}


// get entities and sort them by name
$entities = $dbAdapter->getEntities();
usort($entities, 'nameableObjectCmp');

foreach ($entities as $e) {
    $outFile = DIR_OUT . sprintf('poco.%s.php', $e->getName());
    if (file_exists($outFile)) {
    	// remove existing file
        unlink($outFile);
    }
    
    $fh = fopen($outFile, 'w');
    $writer = new StreamWriter($fh);

    $entityName      = $e->getName();
    $entityClassName = sprintf('POCO_%s',
                               trim($entityName));
    
    echo $entityName . PHP_EOL;
    
    // PHP header
    $writer->writeLine('<?php')
           ->writeLine();
    
    // class documentation
    $writer->writeLine('/**')
           ->writeLineFormat(' * A POCO for the \'%s\' entity.',
                             $entityName)
           ->writeLine(' *')
           ->writeLine(' * @link http://blog.marcel-kloubert.de/ Generated with phpPOCOGenerator by Marcel J. Kloubert')
           ->writeLine(' */');
    
    // class definition
    $writer->writeLineFormat('class %s {',
                             $entityClassName);
    
    // $_attributes
    $writer->writeLine('    /**')
           ->writeLine('     * Stores all attributes and their values of that POCO.')
           ->writeLine('     * ')
           ->writeLine('     * @var array')
           ->writeLine('     */')
           ->writeLine('    protected $_attributes = array();')
           ->writeLine();
    
    // constructor
    $writer->writeLine()
           ->writeLine('    /**')
           ->writeLine('     * Initializes a new instance of that class.')
           ->writeLine('     * ')
           ->writeLine('     * @param array $attribs The list of attributes and values to initialize.')
           ->writeLine('     */')
           ->writeLine('    public function __construct(array $attribs = array()) {')
           ->writeLine('        foreach ($attribs as $name => $value) {')
           ->writeLine('            $this->_attributes[$name] = $value;')
           ->writeLine('        }')
           ->writeLine('    }')
           ->writeLine();
    
    // get attributes of current entity and sort them by name
    $attribs = $e->getAttributes();
    usort($attribs, 'nameableObjectCmp');
    
    // getters
    foreach ($attribs as $a) {
    	$phpType = trim($a->getPhpType());
    	$attribName = trim($a->getName());
    	$ordinal = $a->getOrdinal();
    	
    	echo sprintf("\t[%s] %s" . PHP_EOL,
                     $ordinal,
    			     $attribName);
        
        // GETter documentation
        $writer->writeLine()
               ->writeLine('    /**')
               ->writeLineFormat('     * Gets the current value of \'%s\' attribute.',
                                 $attribName)
               ->writeLine('     * ')
               ->writeLineFormat('     * @return %s The current value.',
                                 $phpType)
               ->writeLine('     */');
        
        // GETter definition
        $writer->writeLineFormat('    public function get_%s() {',
                                 $attribName)
               ->writeLineFormat('        return $this->_attributes[\'%s\'];',
                                 $attribName)
               ->writeLine('    }');
    }
    
    // issetters
    foreach ($attribs as $a) {
        $phpType = trim($a->getPhpType());
        $attribName = trim($a->getName());
        
        // ISSETter documentation
        $writer->writeLine()
               ->writeLine('    /**')
               ->writeLineFormat('     * Checks if the \'%s\' attribute is set / defined or not.',
                                 $attribName)
               ->writeLine('     * ')
               ->writeLineFormat('     * @return boolean Is set / defined (true) or not (false).',
                                 $phpType)
               ->writeLine('     */');
        
        // ISSETter definition
        $writer->writeLineFormat('    public function isset_%s() {',
                                 $attribName)
                ->writeLineFormat('        return isset($this->_attributes[\'%s\']);',
                                  $attribName)
                ->writeLine('    }');
    }
    
    // setters
    foreach ($attribs as $a) {
        $phpType = trim($a->getPhpType());
        $attribName = trim($a->getName());
        
        // SETter documentation
        $writer->writeLine()
               ->writeLine('    /**')
               ->writeLineFormat('     * Sets the current value of \'%s\' attribute.',
                                 $attribName)
               ->writeLine('     * ')
               ->writeLineFormat('     * @param %s $value The new value.',
               		             $phpType)
               ->writeLine('     * ')
               ->writeLineFormat('     * @return %s That POCO instance.',
                                 $entityClassName)
               ->writeLine('     */');
        
        // SETter definition
        $writer->writeLineFormat('    public function set_%s($value) {',
                                 $attribName)
               ->writeLineFormat('        $this->_attributes[\'%s\'] = $value;',
                                 $attribName)
               ->writeLineFormat('        return $this;',
                                 $attribName)
               ->writeLine('    }');
    }
    
    $writer->writeLine('}');
    
    $writer->close();
}

$dbAdapter->close();
