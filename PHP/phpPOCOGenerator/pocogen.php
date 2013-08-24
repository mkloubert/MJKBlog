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

foreach ($dbAdapter->getEntities() as $e) {
	echo $e->getName() . "\r\n";
	
	foreach ($e->getAttributes() as $a) {
		echo "\t" . '[' . $a->getOrdinal() . '] ' . $a->getName() . "\r\n";
	}
}

$dbAdapter->close();
