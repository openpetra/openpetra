
/* ************************************************************************

#asset(openpetraorg/*)

************************************************************************ */

/**
 * This is the main application class of OpenPetra.org
 */
qx.Class.define("openpetraorg.Application",
{
  extend : qx.application.Standalone,
  statics : {
    DESKTOP: null,
    WEBSERVICE: null
  },
  members :
  {
    /**
     * This method contains the initial application code and gets called 
     * during startup of the application
     */
    main : function()
    {
      // Call super class
      this.base(arguments);

      // Enable logging in debug variant
      if (qx.core.Variant.isSet("qx.debug", "on"))
      {
        // support native logging capabilities, e.g. Firebug for Firefox
        qx.log.appender.Native;
        // support additional cross-browser console. Press F7 to toggle visibility
        qx.log.appender.Console;
      }

      /*
      -------------------------------------------------------------------------
        Below is your actual application code...
      -------------------------------------------------------------------------
      */
      var hostname = document.location.host;
      if (hostname == null || hostname == undefined || hostname == "")
      {
        hostname = "localhost";
      }

      openpetraorg.Application.WEBSERVICE = new openpetraorg.soap.Client("http://"+hostname+"/server.asmx");
      
      var windowManager = new qx.ui.window.Manager(); 
      openpetraorg.Application.DESKTOP = new qx.ui.window.Desktop(windowManager);
      openpetraorg.Application.DESKTOP.set({decorator: "main", backgroundColor: "background-pane"});     
      this.getRoot().add(openpetraorg.Application.DESKTOP, {height: "100%", width: "100%"});

      // show modal login dialog
      var loginDialog = new openpetraorg.MCommon.LoginDialog();
      openpetraorg.Application.DESKTOP.add(loginDialog, {left: "30%", top: "30%"});

      loginDialog.open();
    }
  }
});
