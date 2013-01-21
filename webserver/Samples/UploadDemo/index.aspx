<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Import Namespace="Ict.Common" %>

<%@ Import Namespace="System.IO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <title>Sample Registration</title>

    <link rel="stylesheet" type="text/css" href="../../resources/css/ext-all.css"/>
    <script type="text/javascript" src="../../js/ext-base.js"></script>
    <script type="text/javascript" src="../../js/ext-all-debug.js"></script>

    <script type="text/javascript" src="gen/main.js"></script>
    <style type=text/css>
        .upload-icon {
            background: url('../../img/image_add.png') no-repeat 0 0 !important;
        }
    </style>
<script type="text/javascript">
    <!-- 
    Ext.BLANK_IMAGE_URL = '../../img/default_blank.gif';
    Ext.onReady(function() {
        Ext.QuickTips.init();
        Ext.form.Field.prototype.msgTarget = 'side';
        MainForm = new TMainForm();
        MainForm.render('mainFormDiv');
        UploadForm = new TUploadForm();
        UploadForm.render('uploadDiv');
        });
    -->
</script>    
</head>

<body>

<div id="mainFormDiv"></div>

</body>
</html>
