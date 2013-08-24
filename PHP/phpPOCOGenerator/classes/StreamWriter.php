<?php

/**
 * Class that helps to write text to a stream similar to
 * .NET's StreamWriter
 * 
 * @author Marcel Joachim Kloubert <blog.marcel-kloubert.de>
 * @link http://msdn.microsoft.com/de-de/library/system.io.streamwriter.aspx MSDN: System.IO.StreamWriter
 */
final class StreamWriter {
	private $_handle;
	
	
	/**
	 * Initializes a new instance of that class.
	 *
	 * @param resource $handle The underyling file handler.
	 */
	public function __construct($handle) {
		$this->_handle = $handle;
	}
	
	
	/**
	 * Closes the underlying stream.
	 * 
	 * @return StreamWriter That instance.
	 */
	public function close() {
		fclose($this->_handle);
		return $this;
	}
	
	/**
	 * Writes a string to the underlying stream.
	 * 
	 * @param string $str The string / data to write.
	 * 
	 * @return StreamWriter That instance.
	 */
	public function write($str) {
		fwrite($this->_handle, $str);
		return $this;
	}
	
	/**
	 * Writes a string to the underlying stream
	 * and additionally adds a newline character.
	 *
	 * @param string $str The string / data to write.
	 *
	 * @return StreamWriter That instance.
	 */
	public function writeLine($str = '') {
		return $this->write($str)
		            ->write("\n");
	}
	
	/**
	 * Writes a format string to the underlying stream.
	 *
	 * @param string $format The format string.
	 * @param mixed $args
	 * @param mixed $_
	 *
	 * @return StreamWriter That instance.
	 * @see sprintf
	 */
	public function writeFormat() {
		$args = func_get_args();
		$str = call_user_func_array('sprintf', $args);

		return $this->write($str);
	}
	
	/**
	 * Writes a format string to the underlying stream.
	 * and additionally adds a newline character.
	 *
	 * @param string $format The format string.
	 * @param mixed $args
	 * @param mixed $_
	 *
	 * @return StreamWriter That instance.
	 * @see sprintf
	 */
	public function writeLineFormat() {
		$args = func_get_args();
		call_user_func_array(array($this, 'writeFormat'), $args);
		
		return $this->writeLine();
	}
}
