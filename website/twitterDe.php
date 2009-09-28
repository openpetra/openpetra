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
    $url = 'http://twitter.com/statuses/user_timeline.xml?screen_name=' . $user;
    if($since > 0) {
        $url .= '&since_id=' . $since;
    }

    $data = false;
    $path_to_file = CACHE_PATH_TO.DIRECTORY_SEPARATOR.$user.'.'.CACHE_EXTENSION;
    $limit_in_hours=0.5;
    if(!file_exists($path_to_file) || filemtime($path_to_file) <  time() - $limit_in_hours * 60 * 60)
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
           $updates[] = array($id, $text, $date);
        }
    }
    return $updates;
}

function get_relative_date ( $timestamp ) 
{ 

   // calculate the difference in seconds
   $timediff = time () - $timestamp ;
   $hourofday = date('G', $timestamp);
   // only exact to an hour
   $timediff /= 60*60;
   if ($timediff < 1) 
   {
      return "vor ca. einer Stunde";
   }
   if ($timediff <  date('G', time()))
   {
      return "vor ca. ".floor($timediff)." Stunden";
   }
   $timediff -= $hourofday;
   $timediff /= 24;
   if ($timediff<1)
   {
      return "gestern ".($hourofday < 12?'Vormittag':($hourofday < 18?'Nachmittag':'Abend'));
   }
   $timediff++;
   return "vor ".floor($timediff)." Tagen am ".($hourofday < 12?'Vormittag':($hourofday < 18?'Nachmittag':'Abend'));
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
       $msg .= '<div class="tweetdate">'.get_relative_date( $tweed[2]).'</div><div class="tweettext">';
       $msg .= $tweed[1].'</div>'."\n";
     }
     $count++;
  }
  return $msg;
}
?>

<br/><br/>
<div class="twitter">Die neuesten Nachrichten auf <a href="http://www.twitter.com/openpetraorg">Twitter</a>:
<?php echo getTweets('openpetraorgde', 5); ?>
<!-- Follow us on Twitter: <br/><a href="http://www.twitter.com/openpetraorgde"><img src="img/twitter.jpg" border="0"/></a>-->
</div>
