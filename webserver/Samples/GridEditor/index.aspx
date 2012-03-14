<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <title>Sample Registration</title>

    <link rel="stylesheet" type="text/css" href="../../resources/css/ext-all.css"/>
    <link rel="stylesheet" type="text/css" href="../../css/grid-examples.css" /> 
    <link rel="stylesheet" type="text/css" href="../../css/ext-ux-RowEditor.css" /> 

	<style type="text/css"> 
		.x-grid3 .x-window-ml{
			padding-left: 0;	
		} 
		.x-grid3 .x-window-mr {
			padding-right: 0;
		} 
		.x-grid3 .x-window-tl {
			padding-left: 0;
		} 
		.x-grid3 .x-window-tr {
			padding-right: 0;
		} 
		.x-grid3 .x-window-tc .x-window-header {
			height: 3px;
			padding:0;
			overflow:hidden;
		} 
		.x-grid3 .x-window-mc {
			border-width: 0;
			background: #cdd9e8;
		} 
		.x-grid3 .x-window-bl {
			padding-left: 0;
		} 
		.x-grid3 .x-window-br {
			padding-right: 0;
		}
		.x-grid3 .x-panel-btns {
			padding:0;
		}
		.x-grid3 .x-panel-btns td.x-toolbar-cell {
			padding:3px 3px 0;
		}
		.x-box-inner {
			zoom:1;
		}
        }        
    </style>     
    <script type="text/javascript" src="../../js/ext-base.js"></script>
    <script type="text/javascript" src="../../js/ext-all.js"></script>
    <script type="text/javascript" src="../../js/ext-ux-RowEditor.js"></script>
    <script type="text/javascript" src="data.js"></script>
    <script type="text/javascript" src="gen/main.js"></script>
<script type="text/javascript">
    <!-- 
    Ext.BLANK_IMAGE_URL = '../../img/default_blank.gif';
    Ext.onReady(function() {
        Ext.QuickTips.init();
        Ext.form.Field.prototype.msgTarget = 'side';
        initData();
        
        // see also http://stackoverflow.com/questions/2559114/ext-roweditor-js-does-not-fire-afteredit-event        
        editor.on("afteredit",function(roweditor,changes,record,index){ alert('Data should now be saved on the server');});
        
        MainForm = new TMainForm();
        MainForm.render('mainFormDiv');
        
        });
    -->
</script>    

</head>

<body>
<div id="mainFormDiv"></div>
</body>
</html>
