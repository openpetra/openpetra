<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Ict.Common" %>
<%
                string ConfigFileName;
                // make sure the correct config file is used
                if (Environment.CommandLine.Contains("/appconfigfile="))
                {
                    // this happens when we use fastcgi-mono-server4
                    ConfigFileName = Environment.CommandLine.Substring(
                        Environment.CommandLine.IndexOf("/appconfigfile=") + "/appconfigfile=".Length);

                    if (ConfigFileName.IndexOf(" ") != -1)
                    {
                        ConfigFileName = ConfigFileName.Substring(0, ConfigFileName.IndexOf(" "));
                    }
                }
                else
                {
                    // this is the normal behaviour when running with local http server
                    ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config";
                }

                new TAppSettingsManager(ConfigFileName);
                string ServerUrl = TAppSettingsManager.GetValue("Server.Url");

                string [] files = System.IO.Directory.GetFiles("../client", "OpenPetraRemoteSetup-*.exe");
                string filename = String.Empty;
                foreach (string f in files)
                {
                    if (f.Length - f.Replace("-", "").Length != 1)
                    {
                        continue;
                    }
                    filename = f.Replace("-", "-" + ServerUrl + "-");
                    if (!System.IO.File.Exists(filename))
                    {
                        System.IO.File.Copy(f, filename);
                        // delete older installers
                        string currentversion = System.IO.Path.GetFileName(f).Substring("OpenPetraRemoteSetup".Length);
                        string [] oldinstallers = System.IO.Directory.GetFiles("../client", "OpenPetraRemoteSetup-*.exe");
                        foreach (string oldinstaller in oldinstallers)
                        {
                            if (!oldinstaller.EndsWith(currentversion))
                            {
                                System.IO.File.Delete(oldinstaller);
                            }
                        }
                    }
                    filename=System.IO.Path.GetFileName(filename);
                }

                string language="en";
                if ((Request.UserLanguages.Length > 1)
                   && Request.UserLanguages[0].StartsWith("de")) {
                    language="de";
                }

%>
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="img/favicon.png">

    <title>OpenPetra</title>

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

<%
      if (ServerUrl.StartsWith("demo."))
      {
%>
        <div class="form-signin">
          <div class="bs-callout bs-callout-warning"><h4>Download</h4>
            <p>Install the Windows client and connect to our Demo OpenPetra server:</p>
               <button id="btnDownload" class="btn btn-lg btn-primary btn-block"
                  onclick="location.href = '/client/<% Response.Write(filename); %>'">
                    Get the Windows Client!
               </button>
          </div>
          <div class="bs-callout bs-callout-warning"><h4>Login information</h4><p>Please login with user <b>demo</b> and password <b>demo</b></p></div>
        </div>

        <form class="form-signin">
          <p>If you want to try something experimental, have a look at this preview of OpenPetra in the webbrowser:</p>
          <input type="text" class="form-control" placeholder="Email address" id="txtEmail">
          <input type="password" class="form-control" placeholder="Password" id="txtPassword">
          <button id="btnLogin" class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>

          <div class="bs-callout bs-callout-warning">
             <h4>Login information</h4>
             <p>Please login with user <b>demo</b> and password <b>demo</b>,
                  or even easier, just click "Sign in"!</p>
          </div>
        </form>

        <div class="form-signin">
          <div class="bs-callout bs-callout-info">
              <h4>You are welcome!</h4>
              <p>Brought to you by <a href="https://www.openpetra.org">OpenPetra.org</a></p>
          </div>
        </div>

<%
      } // if demo
      else
      {
%>
        <div class="form-signin">
          <div class="bs-callout bs-callout-warning"><h4>Download</h4>
            <p>Install the Windows client and connect to your OpenPetra server:</p>
               <button id="btnDownload" class="btn btn-lg btn-primary btn-block"
                  onclick="location.href = '/client/<% Response.Write(filename); %>'">
                    Get the Windows Client!
               </button>
          </div>
        </div>

        <div class="form-signin">
          <div class="bs-callout bs-callout-info">
              <p>Brought to you by <a href="https://www.openpetra.org">OpenPetra.org</a></p>
          </div>
        </div>

        <div class="form-signin">
          <div class="bs-callout bs-callout-info">
<%     if (language == "de")
       {
%>
              <h4>Unterst&uuml;tzung</h4>
              <p>Bei Problemen bitte das <a href="https://forum.openpetra.de">&ouml;ffentliche OpenPetra-Forum</a> besuchen, dort k&ouml;nnen gerne Fragen gestellt werden!</p>
<%
       }
       else
       {
%>
              <h4>Support</h4>
              <p>If you have any issues, please visit <a href="https://forum.openpetra.org">the public OpenPetra forum</a> and share your questions!</p>

<%
       }
%>
          </div>
        </div>
<%
      }
%>
    </div> <!-- /container -->

  </body>
</html>
