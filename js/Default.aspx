<%@ Page Language="C#" %>
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="img/favicon.png">

    <title>OpenPetra.js Login</title>

    <!-- Bootstrap core CSS -->
    <link href="ThirdParty/Bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="css/login.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="ThirdParty/html5shiv/html5shiv.js"></script>
      <script src="ThirdParty/Respond/respond.min.js"></script>
    <![endif]-->

    <script type="text/javascript" src="ThirdParty/jQuery/jquery.min.js"></script>
    <script type="text/javascript" src="js/login.js"></script>
  </head>

  <body>

    <div class="container">

      <form class="form-signin">
        <h2 class="form-signin-heading">Please sign in to OpenPetra.js</h2>
        <input type="text" class="form-control" placeholder="Email address" autofocus id="txtEmail">
        <input type="password" class="form-control" placeholder="Password" id="txtPassword">
        <button id="btnLogin" class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
      </form>

    </div> <!-- /container -->

  </body>
</html>
