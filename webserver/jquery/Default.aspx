<!DOCTYPE HTML>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title>Login to OpenPetra</title>
<link rel="stylesheet" href="jQueryDesktop/css/desktop.css" />
</head>
<body>
<style>
    label, input { display:block; }
    input.text { margin-bottom:12px; width:95%; padding: .4em; }
    fieldset { padding-top:20px; padding-left:20px; border:0; margin:0px; }
    .textbox input { 
        font:13px Arial, Helvetica, sans-serif;
        padding:6px 6px 4px;
        width:200px;
    }
    .button {
        cursor:pointer;
        display:inline-block;
        padding:6px 6px 4px;
        margin-top:10px;
        font:12px; 
        width:214px;
    }
    #window_login {
        width:300px;
        height:240px
    }
    #window_login .window_aside { width:0px; }
    #window_login .window_main { margin:0px; }
</style>
<div class="abs" id="wrapper">
  <div class="abs" id="desktop">
    <div id="window_login" class="abs window">
      <div class="abs window_inner">
        <div class="window_top">
          <span class="float_left">
            Login to OpenPetra
          </span>
        </div>
        <div class="abs window_content">
          <div class="window_aside">
          </div>
          <div class="window_main">
              <form method="post" action="#">
                <fieldset class="textbox">
                  <label class="username">
                    <span>Username</span>
                    <input id="username" name="username" value="" type="text" autocomplete="on" placeholder="Username">
                    </label>
                    
                    <label class="password">
                    <span>Password</span>
                    <input id="password" name="password" value="" type="password" placeholder="Password">
                    </label>
                    
                    <button id="btnLogin" class="submit button" type="button">Sign in</button>
                  </fieldset>
              </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js"></script>
<script type="text/javascript" src="jQueryDesktop/js/jquery.desktop.js"></script>
<script type="text/javascript" src="js/json2.js"></script> <!-- TODO minify https://raw.github.com/douglascrockford/JSON-js/master/json2.js -->
<script type="text/javascript" src="js/login.js"></script>
</body>
</html>