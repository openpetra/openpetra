<?php

function showError($url)
{
return 'Something changed with Sourceforge Hosted Apps and the content cannot be loaded from there. Please try again later, or go directly to the Sourceforge project page: <a href="http://sf.openpetra.org">OpenPetra.org at Sourceforge</a><br/><a href='.$url.'>Direct link</a>';
}

function endsWith( $str, $sub ) {
   return ( substr( $str, strlen( $str ) - strlen( $sub ) ) === $sub );
}

// returns true if there has been a problem reading from wordpress
function failureWebsite($pageTxt)
{//echo substr(trim($pageTxt), strlen(trim($pageTxt)) - 10);
//echo $pageTxt;
//if (!isset($_GET['dontfail']))
//return true;
  return ($pageTxt === false 
     || strlen(trim($pageTxt)) == 0 
     || ( !endsWith(trim(strtoupper($pageTxt)), '</HTML>') && !endsWith(trim(strtoupper($pageTxt)), '</BODY>'))); 
     // || (strpos($pageTxt, 'warning') > 0));
}


function getContent(&$contents, $startContentStr, $endContentStr)
{
  if ($startContentStr == '^')
  {
    $startContentStr = '';
    $contentPos = 0;
  }
  else
  {
    $contentPos = strpos($contents, $startContentStr);
    //if ($contentPos === false) die('Problem reading page '.$startContentStr);
    if ($contentPos === false) 
    {
      $contents = '';
      return '';
    }
  }
  $start = $contentPos + strlen($startContentStr);
  if ($endContentStr == '^')
  {
    $end = strlen($contents);
  }
  else
  {
    $end = strpos($contents, $endContentStr, $start);
  }
  $result = substr($contents, $start, $end - $start);
  $contents = substr($contents, $end);
  return $result;
}

function FixDateLanguage($postTxt)
{
    global $lang;
    if ($lang == 'de')
    {
        $postTxt = str_replace('Monday', 'Montag', $postTxt);
        $postTxt = str_replace('Tuesday', 'Dienstag', $postTxt);
        $postTxt = str_replace('Wednesday', 'Mittwoch', $postTxt);
        $postTxt = str_replace('Thursday', 'Donnerstag', $postTxt);
        $postTxt = str_replace('Friday', 'Freitag', $postTxt);
        $postTxt = str_replace('Saturday', 'Samstag', $postTxt);
        $postTxt = str_replace('Sunday', 'Sonntag', $postTxt);
        $postTxt = str_replace('January', 'Januar', $postTxt);
        $postTxt = str_replace('February', 'Februar', $postTxt);
        $postTxt = str_replace('March', 'M&auml;rz', $postTxt);
        $postTxt = str_replace('May', 'Mai', $postTxt);
        $postTxt = str_replace('June', 'Juni', $postTxt);
        $postTxt = str_replace('July', 'Juli', $postTxt);
        $postTxt = str_replace('October', 'Oktober', $postTxt);
        $postTxt = str_replace('December', 'Dezember', $postTxt);
    }
    return $postTxt;
}

// from http://www.softarea51.com/tutorials/parse_rss_with_php.html
function ParseRSSFeed($xmltext)
{
        //$doc = new DOMDocument();
        //$doc->load($url);
        $xmltext = str_replace('&', '&amp;', $xmltext);
        $doc = DOMDocument::loadXML(str_replace('content:encoded', 'content', $xmltext));
	$arrFeeds = array();
	foreach ($doc->getElementsByTagName('item') as $node) {
  	    $itemRSS = array ( 
		'title' => utf8_decode($node->getElementsByTagName('title')->item(0)->nodeValue),
		'desc' => $node->getElementsByTagName('description')->item(0)->nodeValue,
		'link' => $node->getElementsByTagName('link')->item(0)->nodeValue,
	        'date' => strtotime($node->getElementsByTagName('pubDate')->item(0)->nodeValue),
		'content' => $node->getElementsByTagName('content')->item(0)->nodeValue
	    );
	    array_push($arrFeeds, $itemRSS);
	}
	return $arrFeeds;
}

function GetPostsInCategory($url, $category, $numberPostsFullLength)
{
        global $lang;
	
	// does not make a change for name of day?
	setlocale(LC_ALL, $lang);
        
	$pageTxt = curl_get_file_contents($url."/category/$category/feed/");
        
	// parse rss feed; instead of trying to get the HTML; will fail on formatting the date anyways
	$feeds = ParseRSSFeed($pageTxt);
	
	$result = "";

	foreach ($feeds as $feed)
	{
	   $counterPosts++;

	   $result .= '<div class="item"><h3><a href="'.$feed['link'].
	      '" rel="bookmark" title="Permanent Link to'.$feed['title'].'">'.$feed['title'].'</a></h3'."\n";
	   $result .= '<small>'.FixDateLanguage(strftime('%A, %d %B %Y', $feed['date'])).'</small>'."\n";

	   if ($counterPosts > $numberPostsFullLength)
	   {
	   	// just show the title and the date
	   }
	   else
	   {
                 $result .= '<div class="entry">'.$feed['content'].'</div>'."\n";
	   }

	   $result .= "</div>";
	}
	return $result;
}

// it seems file_get_contents does not return the full page
function curl_get_file_contents($URL)
{
    $c = curl_init();
    curl_setopt($c, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($c, CURLOPT_URL, $URL);
    $contents = curl_exec($c);
    curl_close($c);
    if ($contents) return $contents;
    else return FALSE;
}

/// return value between first h2 tag
function GetTitleFromPost($content)
{
    return getContent($content, '<h2>', '</h2>');
}

/// remove superflous divs from the end
function fixDivs($result)
{
    $opendiv = substr_count($result, '<div');
    $closediv = substr_count($result, '</div');
    for ($count = 0; $count < $closediv - $opendiv; $count++)
    {
        $pos = strrpos($result, '</div>');
        $result = substr($result, 0, $pos);
    }
    return $result;
}

/// return value between first h2 tag
function GetContentFromPost($content)
{
    $result = getContent($content, '<div class="entry">', '^');
    $result = fixDivs($result);
   
    return $result;
}

function GetPageContent($url, $page, $category, $post)
{
global $lang;
// todo: post: display just one post; also need to change the permanent link in the title

    if (!isset($page) || strlen($page) == 0)
    {
       $result = '';
    }
    else
    {
       $pageTxt = curl_get_file_contents($url.'/'.$page.'/');
       if (failureWebsite($pageTxt))
       {
	    return showError($url.'/'.$page.'/');
       }

       $result = getContent($pageTxt, '<div id="content" class="narrowcolumn" role="main">', '<div id="sidebar"');
       $result = fixDivs($result);
    }

    if (isset($category))
    {
        $numberPostsFullLength = 3;
        $result .= GetPostsInCategory($url, $category, $numberPostsFullLength);
    }
    $result = str_replace('/apps/wordpress', 'http://sourceforge.net/apps/wordpress', $result);
    $result = str_replace('http://sourceforge.nethttp://sourceforge.net', 'http://sourceforge.net', $result);
    $result = str_replace('index.php?', 'index.php?lang='.$lang.'&', $result);
    return $result;
}
?>
