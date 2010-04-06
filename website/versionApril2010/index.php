<?php 
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

$lang = $_GET['lang'];

if (!isset($lang))
{ 
    $lang = GetBestLanguage("");
}

// todo: use ReallySimpleContentCache

if ($lang == 'de')
{
    include "de/index.php";
}
else
{
    include "en/index.php";
}

?>