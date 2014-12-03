<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.TMainWindow"
    validateRequest="false"
    src="Main.aspx.cs" %>
<%
    Response.CacheControl = "no-cache";
%>
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="img/favicon.png">

    <title>OpenPetra.js</title>

    <!-- Bootstrap core CSS -->
    <link href="ThirdParty/Bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="ThirdParty/DataTables/css/dataTables.bootstrap.css" rel="stylesheet">
    <link rel="stylesheet" href="ThirdParty/jQueryUI/css/default/jquery-ui.custom.min.css">
    <link href="css/main.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="ThirdParty/html5shiv/html5shiv.js"></script>
      <script src="ThirdParty/Respond/respond.min.js"></script>
    <![endif]-->

    <script type="text/javascript" src="ThirdParty/jQuery/jquery.min.js"></script>
    <script type="text/javascript" src="ThirdParty/jQueryUI/jquery-ui.min.js"></script>
    <script class="include" type="text/javascript" charset="utf-8" src="ThirdParty/DataTables/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="ThirdParty/Bootstrap/js/bootstrap.min.js"></script>
    <script class="include" type="text/javascript" charset="utf-8" src="ThirdParty/DataTables/js/dataTables.bootstrap.js"></script>
    <script type="text/javascript" src="js/main.js"></script>
    <script type="text/javascript" src="js/navigation.aspx"></script>
  </head>

  <body onLoad="init()">

    <!-- Fixed navbar -->
    <div class="navbar navbar-default navbar-fixed-top" id="topnavigation">
      <div class="container-liquid">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <button type="button" class="navbar-brand btn btn-xs visible" data-toggle="offcanvas" id="btnShow"><i class="glyphicon glyphicon-chevron-right"></i></button>
          <button type="button" class="navbar-brand btn btn-xs hidden" data-toggle="offcanvas" id="btnHide"><i class="glyphicon glyphicon-chevron-left"></i></button>
          <a class="navbar-brand" href="#">OpenPetra</a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href='javascript:OpenTab("frmHome", "Home");'>Home</a></li>
            <li><a href="#about">About</a></li>
            <li><a href="#contact">Contact</a></li>
            <li class="dropdown">
              <a href="#" class="dropdown-toggle" data-toggle="dropdown">Dropdown <b class="caret"></b></a>
              <ul class="dropdown-menu">
                <li><a href="#">Action</a></li>
                <li><a href="#">Another action</a></li>
                <li><a href="#">Something else here</a></li>
                <li class="divider"></li>
                <li class="dropdown-header">Nav header</li>
                <li><a href="#">Separated link</a></li>
                <li><a href="#">One more separated link</a></li>
              </ul>
            </li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="#" id="logout">Logout</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>
    
    <div class="container-liquid">
      <div class="row row-offcanvas row-offcanvas-left">
        <div class="sidebar-offcanvas" id="sidebar" role="navigation">
          <div class="sidebar-nav">
            <div class="list-group" id="LeftNavigation">
            </div>
          </div>
        </div>
        <div class="col-xs-12 col-sm-12" id="tabControl" role="main">
          <div class="row">
            <ul class="nav nav-tabs col-xs-10 col-sm-10" id="TabbedWindows">
            </ul>
          </div>
          <div class="row">
            <div  id="containerIFrames">
            </div>
            
          </div>
         </div>
      </div>
    </div> <!-- /container -->
  </body>
</html>
