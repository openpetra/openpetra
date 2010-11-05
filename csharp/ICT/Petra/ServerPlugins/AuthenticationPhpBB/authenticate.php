<?
// need extension=mcrypt.so in php.ini
define('IN_PHPBB', true);
$phpbb_root_path = (defined('PHPBB_ROOT_PATH')) ? PHPBB_ROOT_PATH : './../';
$phpEx = substr(strrchr(__FILE__, '.'), 1);
include($phpbb_root_path . 'common.' . $phpEx);

include ("../includes/auth/auth_db.php");

// set the path to the secret key text file in variable SecretKeyFile
// make sure the key file cannot be accessed from the outside
// sample content in config-sample.php:
include ("config.php");

class mycrypt
{
var $mykey;

function init($keyfile)
{
  $this->mykey = base64_decode(file_get_contents($keyfile));
}

function encrypt($data, &$iv) {
        $iv_size = mcrypt_get_iv_size(MCRYPT_RIJNDAEL_128, MCRYPT_MODE_CBC);
        //to make things easier, we are reusing the vector
        //$iv = mcrypt_create_iv($iv_size, MCRYPT_RAND);
        $fill = 8 - (strlen($data) % 8);
        if ($fill != 8)
        {
           for ($i = 0; $i < $fill; $i++)
           {
              $data .= chr(0);
           }
        }
        return mcrypt_encrypt (MCRYPT_RIJNDAEL_128, $this->mykey, $data, MCRYPT_MODE_CBC, $iv);
}

function decrypt($data, $iv) {
        return mcrypt_decrypt (MCRYPT_RIJNDAEL_128, $this->mykey, $data, MCRYPT_MODE_CBC, $iv);
}
}
$mycr = new mycrypt();
// make sure the path of secretkey.dat cannot be accessed from the outside
$mycr->init($SecretKeyFile);
$msg = base64_decode($_GET["msg"]);
$iv = base64_decode($_GET["msg2"]);
$clearmsg = $mycr->decrypt($msg, $iv);
$pos = strpos($clearmsg, ";");
$username = substr($clearmsg, 0, $pos);
$pos2 = strpos($clearmsg, ";", $pos + 1);
$password = substr($clearmsg, $pos + 1, $pos2 - $pos - 1);
$result = login_db($username, $password);
//print_r($result);
echo base64_encode($mycr->encrypt( $result['status'] == LOGIN_SUCCESS? 'LOGIN_SUCCESS':($result['status'] == LOGIN_ERROR_ATTEMPTS ? 'LOGIN_ERROR_ATTEMPTS':'LOGIN_FAILURE'), $iv));
/*
$myFile = "$SecretLogFile"; // could set that variable in config.php
$fh = fopen($myFile, 'a') or die("can't open file");
fwrite($fh, $clearmsg."\n");
fwrite($fh, $password."\n");
fwrite($fh, $result['status']."\n");
fclose($fh);
*/
?>
