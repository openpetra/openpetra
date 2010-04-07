                <?php
                    include "wordpress.php";
                    $contentFeatures = GetPageContent("http://sourceforge.net/apps/wordpress/openpetraorg", 'german-site/frontpage-features-de', '', '');
                    $contentFeatures = GetContentFromPost($contentFeatures);
		?>


<div id="featured-wrapper">
    <div id="featured">

        <div class="left" style="width: 430px">

<h2>OpenPetra.org f&uuml;r Sie!</h2>
<br/>
<ul>
<li><b>Einfach zu benutzen, kostenlos, einsatzerprobt...</b><br/>
OpenPetra macht Ihre Mitarbeiter und Spender gl&uuml;cklich!</li>
<li><b>Buchhaltung, Kontaktbetreuung, Personalverwaltung, ...</b><br/>
OpenPetra erledigt alle Verwaltungsaufgaben in Ihrer Organisation</li>
<li><b>Testen und benutzen Sie OpenPetra wie Sie wollen...</b><br/>
OpenPetra l&auml;uft auf einem Laptop, oder im Netzwerk, oder als SaaS</li>
<li><b>Noch in der Entwicklung...</b><br/>
Wir brauchen jetzt Ihre Mithilfe, damit OpenPetra einsatzf&auml;hig wird</li>
</ul>
        
            <p class="large text-center"><a href="index.php?lang=de&page=german-site/screenshots-de">Rundgang</a> <span class="text-separator">|</span> <a href="index.php?lang=de&page=download-openpetraorg&category=releases-en">Download</a> <span class="text-separator">|</span> <a href="index.php?lang=de&page=about-openpetraorg">Mehr Informationen</a></p>

        </div>

        <div class="right">
            <a href="index.php?lang=de&page=german-site/screenshots-de"><img src="img/preview.png" width="420" height="270" alt="" border="0"/></a>
        </div>

        <div class="clearer">&nbsp;</div>

    </div>
</div>

<div id="main">

    <div class="separator-vertical">

        <div class="col2 left">

            <h2 class="left">Produkteigenschaften</h2>
            <div class="right more large"><a href="index.php?lang=de&page=german-site/features-of-openpetra-de">Alle Eigenschaften &#187;</a></div>
            
            <div class="clearer">&nbsp;</div>

	    <?php
		$contentFeatures = str_replace('<h3>', '<div class="content-separator"></div><h3>', $contentFeatures);
		$contentFeatures = str_replace('</p>', '</p><div class="clearer">&nbsp;</div>', $contentFeatures);
                echo  $contentFeatures;
            ?>

        </div>

        <div class="col2 right">

            <h2 class="left">Neueste Neuigkeiten</h2>
            <div class="right more large"><a href="index.php?lang=de&page=news">Mehr Neues &#187;</a></div>

            <div class="clearer">&nbsp;</div>

            <ul class="nice-list">
                <?php include "twitter.php"; ?>
            </ul>


            <h2 class="left">Die Ziele von OpenPetra.org</h2>
            <div class="right more large"><a href="index.php?lang=de&page=organisation-benefits-de">Mehr &#187;</a></div>

            <div class="clearer">&nbsp;</div>
<ul class="nice-list">
    <LI>OpenPetra.org ist einfach zu benutzen, und erfordert nur geringe Schulung f&uuml;r die Benutzer</LI>
    <LI>OpenPetra.org reduziert die Risiken f&uuml;r gemeinn&uuml;tzige Organisationen, wenn es um den Einsatz von Zeit und Geldern f&uuml;r Verwaltungssoftware geht</LI>
    <LI>OpenPetra.org wird in verschiedene Sprachen &uuml;bersetzt, und ist einfach an alle m&ouml;glichen Einsatzsituationen anpassbar</LI>
    <LI>OpenPetra.org er&ouml;ffnet Organisationen gro&szlig;e M&ouml;glichkeiten, und erwartet im Gegenzug, dass Verbesserungen und Erweiterungen wieder ins Projekt fliessen</LI>
</ul>

        </div>

        <div class="clearer">&nbsp;</div>

    </div>

</div>
