<?php $lang='en'; include "wordpress.php"; ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN"
"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html>

<head>
<meta http-equiv="content-type" content="text/html; charset=iso-8859-1"/>
<meta name="description" content="OpenPetra.org is a free Open Source software for running the administration of a non-profit organisation."/>
<meta name="keywords" content="ERP, charity, non-profit, NGO, open source, administration, accounting, personnel, human resources, conference, management, reporting, CRM"/> 
<meta name="author" content="OpenPetra Core Development team"/> 
<link rel="stylesheet" type="text/css" href="default.css" media="screen"/>
<link href="http://sourceforge.net/apps/wordpress/openpetraorg/category/all-news-en/feed/atom/" rel="alternate" type="application/atom+xml" title="Atom feed: News from the OpenPetra.org project" />
<link href="http://sourceforge.net/apps/wordpress/openpetraorg/category/all-news-en/feed/" rel="alternate" type="application/rss+xml" title="RSS feed: News from the OpenPetra.org project" />
<title>OpenPetra.org</title>
</head>

<body>

<div class="container">
	
	<div class="main">

		<div class="header">
		
			<div class="title">
				<h1><a href="index.php">OpenPetra.org</a></h1>
                <h2>Free Administration Software for Non-Profit Organisations</h2>
<!--               <div class="donate"><a href="http://sourceforge.net/donate/index.php?group_id=260632"><img src="http://images.sourceforge.net/images/project-support.jpg" width="88" height="32" border="0" alt="Support OpenPetra.org" /></a></div> -->
			</div>
                   <div class="flags">
		      <a href="index.php?lang=de"><img src="flags/de.png" border="0"/></a>
		      <a href="index.php?lang=en"><img src="flags/gb.png" border="0"/></a>
		   </div>

		</div>
	<div class="sidenav">
            <?php include "navigationLeft.php"?>
          </div>	
	 <div class="sidenav2">
	 <?php include "navigationRight.php"?>
	 </div>
		<div class='content'>
			<?php 
            if (!isset($_GET['page']) && !isset($_GET['category']) && !isset($_GET['post']))
            {
                // get the latest news from the project blog
		$category = 'all-news-en';
		$page = 'about-openpetraorg';
		//$category= 'weekly';
            }
            else
            {
                $page = $_GET['page'];
                $category = $_GET['category'];
	        $post = $_GET['post']; 
	    }

	    if (isset($category))
	    {
	        echo "Stay uptodate by suscribing to the <a href='http://sourceforge.net/apps/wordpress/openpetraorg/category/all-news-en/feed/'>RSS feed</a> or 
		the <a href='https://lists.sourceforge.net/lists/listinfo/openpetraorg-weekly'>mailing list</a>, 
		or follow us on <a href='http://www.twitter.com/openpetraorg'>Twitter</a>!";
	    }
            
	    // get the page from the wiki
            $content = GetPageContent("http://sourceforge.net/apps/wordpress/openpetraorg", $page, $category, $post);
            echo $content;
            ?>
		</div>


         <div class="clearer"><span></span></div>


	</div>

	<div class="footer">&copy; 2009 <a href="index.php">OpenPetra.org</a>. Valid <a href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a> &amp; <a href="http://validator.w3.org/check?uri=referer">XHTML</a>. Template design by <a href="http://templates.arcsin.se">Arcsin</a>. 
	Project hosted at &nbsp;
		<a href="http://sourceforge.net/projects/openpetraorg"><img src="http://sflogo.sourceforge.net/sflogo.php?group_id=260632&amp;type=9" width="80" height="15" alt="Get OpenPetra.org at SourceForge.net. Fast, secure and Free Open Source software downloads" /></a>
	<br/>Using ReallySimpleContentCache script by <a href="http://www.ibrow.com">Rob Searles</a>.
	</div>

</div>

</body>

</html>
