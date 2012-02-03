<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Petra.Server.app.WebService" %>

<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="PetraWebService" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

string FSelectedCountry = String.Empty;
string FSelectedLanguage = String.Empty;
string FSelectedRole = String.Empty;

// in this case, we are using the country of registration to determine the language
private void CalculatePreferredLanguage()
{
    string CountrySelectionPage = "~/index.html?error=PleaseSelectValidCountry";

    if (HttpContext.Current.Request.Params["country-id"] == null || HttpContext.Current.Request.Params["country-id"].Length == 0)
    {
        Response.Redirect(CountrySelectionPage);
    }
    
    FSelectedCountry = HttpContext.Current.Request.Params["country-id"];
    FSelectedRole = HttpContext.Current.Request.Params["role-id"];
    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Apps/OnlineRegistration/gen/main." + FSelectedCountry + "." + FSelectedRole + ".js"))
    {
        Response.Redirect(CountrySelectionPage);
    }

    FSelectedLanguage = FSelectedCountry;
    if (FSelectedLanguage.Contains("-"))
    {
        FSelectedLanguage = FSelectedLanguage.Substring(0, FSelectedLanguage.IndexOf("-"));
    }

    /// check whether we can get a connection to the database. otherwise the people should not enter all the data for nothing
    try
    {

        TOpenPetraOrg server = new TOpenPetraOrg();
        server.InitServer();
        server.Logout();
    }
    catch (Exception e)
    {
        TLogging.Log("Cannot access website, " + e.Message);
        Response.Redirect(CountrySelectionPage);
    }
}

private string WithValidation()
{
  string NoValidation = HttpContext.Current.Request.Params["validate"];
  if ((NoValidation != null) && (NoValidation == "false"))
  {
    return "false";
  }
  
  return "true";
}

</script>

<%CalculatePreferredLanguage();%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <title>Sample Registration</title>

    <link rel="stylesheet" type="text/css" href="../../resources/css/ext-all.css"/>
    
    <script type="text/javascript" src="../../js/ext-all-debug.js"></script>

    <script type="text/javascript" src="../../js/locale/ext-lang-<%Response.Write(FSelectedLanguage);%>.js"></script>

    <script type="text/javascript" src="gen/main.<%Response.Write(FSelectedCountry + "." + FSelectedRole);%>.js"></script>
    <script type="text/javascript" src="main.<%Response.Write(FSelectedCountry);%>-lang.js"></script>
    <script type="text/javascript" src="main.<%Response.Write(FSelectedCountry + "." + FSelectedRole);%>-lang.js"></script>

    <style type=text/css>
        .upload-icon {
            background: url('../../img/image_add.png') no-repeat 0 0 !important;
        }
    </style>

<script type="text/javascript">
    Ext.BLANK_IMAGE_URL = '../../img/default_blank.gif';
    Ext.onReady(function() {
        Ext.QuickTips.init();
        Ext.form.Field.prototype.msgTarget = 'side';

        MainForm = Ext.create('TMainForm');
        MainForm.validate = <%Response.Write(WithValidation());%>;
        MainForm.render('mainFormDiv');
        // this is required for smaller screens, otherwise the scrollbars don't work properly
        MainForm.setPosition(0,0);
        
        UploadForm = Ext.create('TUploadForm');
        UploadForm.render('tmpUploadDiv');
        
        });
</script>    
</head>

<body>
<div id="mainFormDiv"></div>
<div id="tmpUploadDiv" style="visibility:hidden"></div>
</body>
</html>