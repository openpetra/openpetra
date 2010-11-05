<%@ Page Language="C#" %>

<script runat="server">
private string GetPreferredLanguage()
{
    if (HttpContext.Current.Request.Params["lang"] != null)
    {
        return HttpContext.Current.Request.Params["lang"];
    }
    
    try
    {
        return HttpContext.Current.Request.UserLanguages[0].Trim().Split(new char[]{'-'})[0];
    }
    catch (ArgumentException)
    {
    }
    
    return "en";
}
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <title>Test Form</title>

    <link rel="stylesheet" type="text/css" href="http://extjs.cachefly.net/ext-3.2.1/resources/css/ext-all.css"/>
    <script type="text/javascript" src="http://extjs.cachefly.net/ext-3.2.1/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="http://extjs.cachefly.net/ext-3.2.1/ext-all.js"></script>
    <script type="text/javascript" src="http://extjs.cachefly.net/ext-3.2.1/src/locale/ext-lang-<%Response.Write(GetPreferredLanguage());%>.js"></script>
    
    <script type="text/javascript" src="gen/partnerdata.js"></script>
    <script type="text/javascript" src="partnerdata-lang-<%Response.Write(GetPreferredLanguage());%>.js"></script>
    <script type="text/javascript" src="gen/inquiry.js"></script>

<script type="text/javascript">
    <!-- 
    // Ext.BLANK_IMAGE_URL = '../ext/resources/images/default/s.gif';
    Ext.onReady(function() {
        Ext.QuickTips.init();
        Ext.form.Field.prototype.msgTarget = 'side';

        partnerdata = new partnerdataForm();
        partnerdata.render('partnerdataDiv');

        inquiry = new inquiryForm();
        inquiry.render('inquiryDiv');

        });
    -->
</script>    

    <style type="text/css">
    <!--
    body {
        padding:20px;
        padding-top:32px;
    }
    -->
    </style>    
</head>
<body >

TODO: enter email and password to edit your own data that you have entered previously
<br/> TODO
<div id="partnerdataDiv"></div>
<br/><br/>
<div id="contactform">
</div>
<br/><br/>
<div id="inquiryDiv"></div>


<font size="-2">This form was created with <a href="http://www.openpetra.org">OpenPetra</a>, the free software for the administration of charities.</font>
</body>
</html>
