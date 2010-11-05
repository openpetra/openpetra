<?php
    $nav = $page;
    
    // define default pages
    if ($page == 'product')
    {
        $page = 'about-openpetraorg';
    }
    if ($page == 'tech')
    {
        $page = 'technical-details';
    }
    if ($page == 'community')
    {
        $page = 'user-community-support';
    }
    if ($page == 'news')
    {
        $category = 'weekly-en';
    }

    if ($page == 'about-openpetraorg'
        || $page == 'screenshots'
        || $page == 'organisation-benefits'
        || $page == 'licensing'
        || $page == 'features-of-openpetra'
        || $page == 'history-of-petra'
        || $page == 'history-of-openpetraorg'
        || $page == 'roadmap')
    {
	$nav = "product";
    }
    else if ($page == 'flyer')
    {
        $nav = 'flyer';
    }
    else if ($page == 'developers-joining'
        || $page == 'developers-get-started'
        || $page == 'technical-details'
        || $page == 'developers-documentation'
        || $page == 'developers-forum')
    {
        $nav = "tech";
    }
    else if ($page == 'user-manuals'
        || $page == 'contact-us'
        || $page == 'shortcuts'
        || $page == 'vote-for-ideas'
        || $page == 'user-community-support'
        || $page == 'organisation-support'
        || $page == 'organisation-join')
    {
        $nav = "community";
    }
    else if ($category == 'weekly-en'
        || $page == 'download-openpetraorg'
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
					<li <?php echo ($page == 'front'? 'class="current_page_item"':'');?>><a href="index.php?lang=en">Home</a></li>
					<li <?php echo ($page == 'download-openpetraorg'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=download-openpetraorg&category=releases-en">Download</a></li>
					<li <?php echo ($nav == 'product'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=product">Product Details</a></li>
					<li <?php echo ($nav == 'news' && $page != 'download-openpetraorg'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=news">News</a></li>
					<li <?php echo ($nav == 'community'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=community">Community</a></li>
					<li <?php echo ($nav == 'tech'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=tech">Tech Corner</a></li>
					<li <?php echo ($nav == 'flyer'? 'class="current_page_item"':'');?>><a href="index.php?lang=en&page=flyer">Flyer</a></li>
				</ul>

				<div class="clearer">&nbsp;</div>

			</div>

		</div>
	</div>
</div>
