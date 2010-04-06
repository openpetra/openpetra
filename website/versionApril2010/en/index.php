<?php include "header.php"; ?>

<?php 
$page = $_GET['page'];
$category = $_GET['category'];

if (!isset($page) && !isset($category))
{ 
    $page = 'front';
}
?>

<?php include "navigation.php"; ?>

<div id="content-wrapper">
	<div class="center-wrapper">
		
		<div class="content">

        <?php 
        if ($page == 'front')
        {
            include "front.php";
        }
        else
        {
            include "page.php";
        }
        ?>

		</div>

	</div>
</div>

<?php include "footer.php"; ?>
