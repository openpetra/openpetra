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
                <div class="box-title">Produktbeschreibung</div>

                <div class="box-content">
                    <ul class="nice-list">
                        <li><a href="index.php?lang=de&page=german-site/about-openpetraorg-de">Was ist es und was macht es?</a></li>
                        <li><a href="index.php?lang=de&page=german-site/features-of-openpetra-de">Produkteigenschaften</a></li>
                        <li><a href="index.php?lang=de&page=german-site/organisation-benefits-de">Vorteile von OpenPetra.org</a></li>
                        <li><a href="index.php?lang=de&page=german-site/screenshots-de">Ein paar Bildschirmfotos</a></li>
                        <li><a href="index.php?lang=de&page=german-site/licensing-de">Lizenzrechtliches</a></li>
                        <li><a href="index.php?lang=de&page=german-site/history-of-petra-de">Geschichte von Petra</a></li>
                        <li><a href="index.php?lang=de&page=german-site/history-of-openpetraorg-de">Geschichte von OpenPetra.org</a></li>
                        <li><a href="index.php?lang=de&page=german-site/roadmap-de">Zeitplan f&uuml;r OpenPetra</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'tech')
    {
?>
                <div class="box-title">Technische Ecke</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=de&page=german-site/technical-details-de">Technische Details</a></li>
                    <li><a href="index.php?lang=de&page=german-site/developers-joining-de">Bei OpenPetra.org mitmachen</a></li>
                    <li><a href="index.php?lang=de&page=german-site/developers-get-started-de">Erste Schritte</a></li>
                    <li><a href="index.php?lang=de&page=german-site/developers-documentation-de">Dokumentation</a></li>
                    <li><a href="index.php?lang=de&page=german-site/developers-forum-de">Entwicklerforum</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'community')
    {
?>
                <div class="box-title">Benutzergemeinschaft</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=de&page=german-site/user-manuals-de">Handb&uuml;cher</a></li>
                    <li><a href="index.php?lang=de&page=german-site/vote-for-ideas-de">F&uuml;r Vorschl&auml;ge stimmen</a></li>
                    <li><a href="index.php?lang=de&page=german-site/user-community-support-de">Gegenseitige Hilfestellung</a></li>
                    <li><a href="index.php?lang=de&page=german-site/organisation-support-de">Hilfestellung f&uuml;r Werke</a></li>
                    <li><a href="index.php?lang=de&page=german-site/organisation-join-de">Bei OpenPetra.org einsteigen</a></li>
                    </ul>
                </div>

<?php }
      else if ($nav == 'news')
    {
?>
                <div class="box-title">Neuigkeiten</div>

                <div class="box-content">
                    <ul class="nice-list">
                    <li><a href="index.php?lang=de&category=weekly-de">Projekt Blog</a></li>
                    <li><a href="index.php?lang=de&page=german-site/download-openpetraorg-de&category=releases-de">Neue Versionen</a></li>
                    <li><a href="index.php?lang=de&page=twitter">Twitter Updates</a></li>
                    <li><a href="index.php?lang=de&page=rssfeed">RSS Feed</a></li>
                    <li><a href="index.php?lang=de&page=mailinglist">Mailing List</a></li>
                    </ul>
                </div>

<?php } ?>
            </div>

        </div>
    </div>

    <div class="clearer">&nbsp;</div>

</div>
