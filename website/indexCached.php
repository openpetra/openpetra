<?php
/**
 * ReallySimpleContentCache - demo file for not so simple useage
 * As the name suggests, this is class is for really simple content caching
 *
 * Copyright 2007 Rob Searles
 * http://www.ibrow.com
 * Please give me credit if you use this. Thanks
 *
 * History:
 * I originally built this for caching XML which had been constructed
 * out of database calls. I had a Flash front end talking to PHP in the
 * backend, the PHP would then go and interrogate the database, construct
 * some more XML which it would return. There were many calls being made to
 * the database, so I decided to cache the final reply XML.
 *
 * It's not bullet proof, and doesn't have a great deal of functionality,
 * but it is a quick and dirty solution to a little scratch I had to itch
 * Play around with it, learn, improve and enjoy. I hope you find it useful
 *
 * copyright 2007 rob searles
 * Licenced under the GNU Lesser General Public License (Version 3)
 * http://www.gnu.org/licenses/lgpl.html
 * addition by Timotheus.Pokorra@ict.om.org: wiki2web.php 
 */
require('ReallySimpleContentCache.php');

function GetBestLanguage($str)
{
	$str=$str?$str:$_SERVER['HTTP_ACCEPT_LANGUAGE'];
	$langs=explode(',',$str);
	foreach ($langs as $lang) 
	{
                // parsing language preference instructions
                // 2_digit_code[-longer_code][;q=coefficient]
                ereg('([a-z]{1,2})(-([a-z0-9]+))?(;q=([0-9\.]+))?',$lang,$found);
                // 2 digit lang code
                $code=$found[1];
		if ($code == "en")
		{
		   return "en";
		}
		else if ($code == "de")
		{
		  return "de";
		}
	}

	return 'en';
}

$scriptUrl = $_SERVER['SCRIPT_URL'];
if (!isset($_SERVER['SCRIPT_URL']) || strlen($_SERVER['SCRIPT_URL']) == 1)
{
	$scriptUrl = '/index.php';
}
$url='http://'.$_SERVER["HTTP_HOST"].$scriptUrl;
if (isset($_SERVER["QUERY_STRING"]) && strlen($_SERVER["QUERY_STRING"]) > 0)
{
  $url.='?'.$_SERVER["QUERY_STRING"];
}

$lang = $_GET['lang'];
if (substr($url, strlen($url) - 3) == '-de')
{
   $lang = 'de';
}
else if (!isset($lang) && strlen($_SERVER["QUERY_STRING"]) > 0)
{
   $lang = 'en';
}
$lang = GetBestLanguage($lang);
$nameIndexOrig = 'indexOrig.php';
if ($lang == 'de')
{
   $nameIndexOrig = 'indexOrigDe.php';
}
$siteid=md5($url.$lang);
$url = str_replace('index.php', $nameIndexOrig, $url);
$url = str_replace('indexCached.php', $nameIndexOrig, $url);
if (strpos($url, 'reload') > 0)
{
   $CacheClear = new ReallySimpleContentCache();
   $CacheClear->clear('*');
}
//echo $url;
//die(phpinfo());
//$content = curl_get_file_contents($url);
//die($content);

// it seems file_get_contents does not return the full page
function curl_get_file_contents($URL)
{
    // die("getting " . $URL);
    $c = curl_init();
    curl_setopt($c, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($c, CURLOPT_URL, $URL);
    $contents = curl_exec($c);
    curl_close($c);
    if ($contents) return $contents;
    else return FALSE;
}



/**
 * Initiate ReallySimpleContentCache class
 */
$Cache = new ReallySimpleContentCache($siteid);
/**
 * Set the ID of this cache file
 */
$Cache->set_id($siteid);
/**
 * Set timeout limit to 0.1 of an hour - er, 6 minutes? set_limit(0.1)
   changed to two hours
 */
$Cache->set_limit(2);
/**
 * Try to get any content from the Cache
 */
$alreadyInCache = $Cache->get();
/**
 * If no content has been given, go off and create some
 */
if(!$alreadyInCache) 
{
//echo "loading $url";
	$content = curl_get_file_contents($url);
	/**
	 * now cache this content
	 */
	$Cache->save($content);
        echo $content;
}
?>
