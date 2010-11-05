<?php
    $nav = $page;
    
    // define default pages
    if ($page == 'product')
    {
        $page = 'german-site/about-openpetraorg-de';
    }
    if ($page == 'tech')
    {
        $page = 'german-site/technical-details-de';
    }
    if ($page == 'community')
    {
        $page = 'german-site/user-community-support-de';
    }
    if ($page == 'news')
    {
        $category = 'weekly-de';
    }

    if ($page == 'german-site/about-openpetraorg-de'
        || $page == 'german-site/screenshots-de'
        || $page == 'german-site/organisation-benefits-de'
        || $page == 'german-site/licensing-de'
        || $page == 'german-site/features-of-openpetra-de'
        || $page == 'german-site/history-of-petra-de'
        || $page == 'german-site/history-of-openpetraorg-de'
        || $page == 'german-site/roadmap-de')
    {
	$nav = "product";
    }
    else if ($page == 'flyer')
    {
        $nav = 'flyer';
        $page = 'german-site/flyer-de';
    }
    else if ($page == 'german-site/developers-joining-de'
        || $page == 'german-site/developers-get-started-de'
        || $page == 'german-site/technical-details-de'
        || $page == 'german-site/developers-documentation-de'
        || $page == 'german-site/developers-forum-de')
    {
        $nav = "tech";
    }
    else if ($page == 'german-site/user-manuals-de'
        || $page == 'german-site/contact-us-de'
        || $page == 'shortcuts'
        || $page == 'german-site/vote-for-ideas-de'
        || $page == 'german-site/user-community-support-de'
        || $page == 'german-site/organisation-support-de'
        || $page == 'german-site/organisation-join-de')
    {
        $nav = "community";
    }
    else if ($category == 'weekly-de'
        || $page == 'german-site/download-openpetraorg-de'
        || $page == 'twitter'
        || $page == 'rssfeed'
        || $page == 'mailinglist')
    {
        $nav = "news";
    }
?>

<div id="navigation-wrapper">
	<div id="navigation-wrapper-2">
		<div class="center-wrapper">
   	                        <div class="flags">
			            <a href="index.php?lang=de"><img src="flags/de.png" border="0"/></a>
   			            <a href="index.php?lang=en"><img src="flags/gb.png" border="0"/></a>
			        </div>
	
			<div id="navigation">

				<ul class="tabbed">
					<li <?php echo ($page == 'front'? 'class="current_page_item"':'');?>><a href="index.php?lang=de">Start</a></li>
					<li <?php echo ($page == 'german-site/download-openpetraorg-de'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=german-site/download-openpetraorg-de&category=releases-de">Download</a></li>
					<li <?php echo ($nav == 'product'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=product">Produkt</a></li>
					<li <?php echo ($nav == 'news' && $page != 'german-site/download-openpetraorg-de'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=news">Neuigkeiten</a></li>
					<li <?php echo ($nav == 'community'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=community">Benutzergemeinschaft</a></li>
					<li <?php echo ($nav == 'tech'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=tech">Technische Ecke</a></li>
					<li <?php echo ($nav == 'flyer'? 'class="current_page_item"':'');?>><a href="index.php?lang=de&page=flyer">Handzettel</a></li>
				</ul>

				<div class="clearer">&nbsp;</div>

			</div>

		</div>
	</div>
</div>
