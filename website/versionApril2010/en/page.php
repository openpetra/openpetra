<div class="content" id="content-two-columns">

    <div id="main-wrapper">
        <div id="main">

            <div class="post">


                <?php
                    include "wordpress.php";
                    $content = GetPageContent("http://sourceforge.net/apps/wordpress/openpetraorg", $page, $category, $post);
                    if (isset($category))
                    {
                        echo $content;
                    }
                    else
                    {
                        $title = GetTitleFromPost($content);
                        $content = GetContentFromPost($content);
                ?>

                        <div class="post-title"><h2><?php echo $title; ?></h2></div>

                        <div class="post-body">
                        <?php echo $content; ?>
                        </div>
                <?php
                    }
                ?>
                
            </div>

        </div>
    </div>

    <div id="sidebar-wrapper">
        <div id="sidebar">

            <div class="box">

<?php
    
    if ($nav == 'product' || $nav == 'flyer')
    {
?>
                <div class="box-title">Product Details</div>

                <div class="box-content">
                    <ul class="nice-list">
                        <li><a href="index.php?lang=en&page=about-openpetraorg">What is it and what can it do?</a></li>
                        <li><a href="index.php?lang=en&page=features-of-openpetra">Features available</a></li>
                        <li><a href="index.php?lang=en&page=organisation-benefits">Benefits of OpenPetra.org</a></li>
                        <li><a href="index.php?lang=en&page=screenshots">A few screenshots</a></li>
                        <li><a href="index.php?lang=en&page=licensing">Licensing</a></li>
                        <li><a href="index.php?lang=en&page=history-of-petra">History of Petra</a></li>
                        <li><a href="index.php?lang=en&page=history-of-openpetraorg">History of OpenPetra.org</a></li>
                        <li><a href="index.php?lang=en&page=roadmap">Roadmap for OpenPetra</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'tech')
    {
?>
                <div class="box-title">Tech Corner</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=en&page=technical-details">Technical details</a></li>
                    <li><a href="index.php?lang=en&page=developers-joining">Join the effort</a></li>
                    <li><a href="index.php?lang=en&page=developers-get-started">Getting started</a></li>
                    <li><a href="index.php?lang=en&page=developers-documentation">Documentation</a></li>
                    <li><a href="index.php?lang=en&page=developers-forum">Discussions in Forums</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'community')
    {
?>
                <div class="box-title">Community</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=en&page=user-manuals">Manuals</a></li>
                    <li><a href="index.php?lang=en&page=vote-for-ideas">Vote for ideas</a></li>
                    <li><a href="index.php?lang=en&page=user-community-support">Community Support</a></li>
                    <li><a href="index.php?lang=en&page=organisation-support">Corporate Support</a></li>
                    <li><a href="index.php?lang=en&page=organisation-join">Join the effort</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'news')
    {
?>
                <div class="box-title">News</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=en&category=weekly-en">Project Blog</a></li>
                    <li><a href="index.php?lang=en&page=download-openpetraorg&category=releases-en">Release Notes</a></li>
                    <li><a href="index.php?lang=en&page=twitter">Twitter Updates</a></li>
                    <li><a href="index.php?lang=en&page=rssfeed">RSS Feed</a></li>
                    <li><a href="index.php?lang=en&page=mailinglist">Mailing List</a></li>
                    </ul>
                </div>

<?php } ?>
            </div>

        </div>
    </div>

    <div class="clearer">&nbsp;</div>

</div>
