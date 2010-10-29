<?php
define('CACHE_PATH_TO', dirname(__FILE__).DIRECTORY_SEPARATOR.'cache');
define('CACHE_EXTENSION', 'cachetw');

// it seems file_get_contents does not return the full page
function curl_get_file_contents_from_twitter($URL)
{
    // die("getting " . $URL);
    $c = curl_init();
    curl_setopt($c, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($c, CURLOPT_URL, $URL);
    $contents = curl_exec($c);
    curl_close($c);
    if (strlen($contents) > 0) return $contents;
    else return false;
}

function fetch_twitter_feed($user, $since = 0) {
global $reload;
    $url = 'http://twitter.com/statuses/user_timeline.xml?screen_name=' . $user;
    if($since > 0) {
        $url .= '&since_id=' . $since;
    }

    $data = false;
    $path_to_file = CACHE_PATH_TO.DIRECTORY_SEPARATOR.$user.'.'.CACHE_EXTENSION;
    $limit_in_hours=0.5;
    if(!file_exists($path_to_file) || filemtime($path_to_file) <  time() - $limit_in_hours * 60 * 60 || isset($reload))
    {
        // try to get new version
        
    $attempt = 0;
    while ($data === false && $attempt < 10)
    {
	    $data = curl_get_file_contents_from_twitter($url);
            $xmlDoc = new DOMDocument();
            if (@$xmlDoc->loadXML($data) === false)
            {
                  $data = false;
            }
	    $attempt++;
    }
        if (! ($data === false))
	{
	   // store file in cache
	   file_put_contents($path_to_file, $data);
	}
    }
    
    if ($data === false)
    {
       $data = file_get_contents($path_to_file);
    }
    if ($data === false)
    { 
       return false;
    }
//    echo $data;
    $xmlDoc = new DOMDocument();
    if (@$xmlDoc->loadXML($data) === false)
    {
        //echo $data;
	/*
	<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/1999/REC-html401-19991224/strict.dtd">
	<!-- <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
	"http://www.w3.org/TR/html4/strict.dtd"> -->
	<HTML>
	<HEAD>
	<META HTTP-EQUIV="Refresh" CONTENT="0.1">
	<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
	<META HTTP-EQUIV="Expires" CONTENT="-1">
	<TITLE></TITLE>
	</HEAD>
	<BODY><P></BODY>
	</HTML>
        */
	return false;
    }
    $statuses = $xmlDoc->getElementsByTagName('status');
    $updates = array();
    if($statuses->length > 0) {
        foreach($statuses as $status) {
           $id = $status->getElementsByTagName('id')->item(0)->nodeValue;
           $text = $status->getElementsByTagName('text')->item(0)->nodeValue;
           $text = str_replace("https://translations.launchpad.net/openpetraorg", "http://bit.ly/abcEtj", $text);
	   $text = htmlspecialchars($text);
	   while (!(($poshttp = strpos($text, 'http://')) === false))
	   {
	   	$posspace = strpos($text, ' ', $poshttp);
		if ($posspace === false)
		{
		   $posspace = strlen($text);
		}
	        $http = substr($text, $poshttp, $posspace - $poshttp);
		$temphttp = str_replace('http://', 'REPLACEHTTP://', $http);
		$text = str_replace ($http, '<a href="'.$temphttp.'">'.$temphttp.'</a>', $text);
	   }
	   $text = str_replace('REPLACEHTTP://', 'http://', $text);
           $date = strtotime($status->getElementsByTagName('created_at')->item(0)->nodeValue);
	   if ($text[0] != '@')
	   {
               $updates[] = array($id, $text, $date);
	   }
        }
    }
    return $updates;
}

function get_relative_date ( $timestamp ) 
{ 

   // calculate the difference in seconds
   $timediff = time () - $timestamp ;
   $hourofday = date('G', $timestamp);
   $hourtoday = date('G', time());
   // only exact to an hour
   $timediff /= 60*60;
   if ($timediff < 1) 
   {
      return "about an hour ago";
   }
   if ($timediff <  12)
   {
      return "about ".floor($timediff)." hours ago";
   }
   $timediff -= $hourtoday;
   $timediff /= 24;
   if ($timediff<1)
   {
      return "yesterday in the ".($hourofday < 12?'morning':($hourofday < 18?'afternoon':'evening'));
   }
   $timediff++;
   return floor($timediff)." days ago in the ".($hourofday < 12?'morning':($hourofday < 18?'afternoon':'evening'));
}

function getTweets($twittername, $numberTweets)
{
$tweeds = false;
  $tweeds = fetch_twitter_feed($twittername);
  if ($tweeds === false) return 'Twitter feed currently not available...';
  $count = 0;
  $msg = '';
  foreach ($tweeds as $tweed)
  {
     if ($count < $numberTweets)
     {
       //$msg .= '<div class="tweetdate">'.date('d-M-Y g:i A', $tweed[2]).'</div><div class="tweettext">';
       $msg .= '<li><span>'.get_relative_date( $tweed[2]).': </span>';
       $msg .= $tweed[1].'</li>'."\n";
     }
     $count++;
  }
  return $msg;
}
?>
<?php echo getTweets('openpetraorg', 3); ?>
