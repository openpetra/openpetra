<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Import Namespace="Ict.Common" %>

<%@ Import Namespace="System.IO" %>

<script runat="server">

string FSelectedCountry = String.Empty;
string FSelectedLanguage = String.Empty;

// in this case, we are using the country of registration to determine the language
private void CalculatePreferredLanguage()
{
    string CountrySelectionPage = "~/index.html?error=PleaseSelectValidCountry";

    FSelectedCountry = HttpContext.Current.Request.Params["country-id"];

    if (FSelectedCountry == null || FSelectedCountry.Length == 0)
    {
        FSelectedCountry = "en-gb";
    }

    string SupportedCountries = "de-at, nl-be, fr-be, cs, fi, fr-fr, de-de, en-ie, it, nl-nl, no, pt, es, sv, fr-ch, de-ch, en-gb";
    if (!StringHelper.StrSplit(SupportedCountries, ",").Contains(FSelectedCountry))
    {
        Response.Redirect(CountrySelectionPage);
    }

    FSelectedLanguage = FSelectedCountry;
    if (FSelectedLanguage.Contains("-"))
    {
        FSelectedLanguage = FSelectedLanguage.Substring(0, FSelectedLanguage.IndexOf("-"));
    }
}


</script>

<%CalculatePreferredLanguage();%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <title>Sample Registration</title>

    <link rel="stylesheet" type="text/css" href="../../resources/css/ext-all.css"/>
    <script type="text/javascript" src="../../js/ext-base.js"></script>
    <script type="text/javascript" src="../../js/ext-all.js"></script>
    <script type="text/javascript" src="http://extjs.cachefly.net/ext-3.2.1/src/locale/ext-lang-<%Response.Write(FSelectedLanguage);%>.js"></script>

    <script type="text/javascript" src="gen/main.js"></script>
    <script type="text/javascript" src="main.<%Response.Write(FSelectedCountry);%>-lang.js"></script>
<script type="text/javascript">
    <!-- 
    Ext.BLANK_IMAGE_URL = '../../img/default_blank.gif';
    Ext.onReady(function() {
        Ext.QuickTips.init();
        Ext.form.Field.prototype.msgTarget = 'side';

        MainForm = new TMainForm();
        MainForm.render('mainFormDiv');
        });
    -->
</script>    
</head>

<body>

This demo shows several controls and their behaviour:

<ul>
<li>Email with Validation</li>
<li>Multi-Line Text</li>
<li>Radio Buttons</li>
<li>TODO: Radio Buttons with dependant controls (see missing country text box below)</li>
<li>Comboboxes, with default value</li>
<li>Date Time Picker, limited to a date range. Format and Help are set specifically in this test, otherwise the format would depend on the localisation that has been chosen</li>
<li>TODO: 3 comboboxes for selecting the date of birth</li>
<li>Show a document in an iframe, in the correct language</li>
<li>Force a ticked checkbox for the rules, but don't force people to be vegetarian.</li>
</ul>

<br/><br/>
Show this form for different languages: <a href="index.aspx?country-id=de-de">german</a>, <a href="index.aspx?country-id=en-gb">english</a>

<br/><br/>

<div id="mainFormDiv"></div>

</body>
</html>
