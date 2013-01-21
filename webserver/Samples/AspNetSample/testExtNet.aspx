<%@ Page Language="C#"
    Inherits="MyButtonTest.TMyButtonTest"
    src="testExtNet.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>My Ext.Net Sample</title>
</head>
<body>

    <ext:ResourceManager runat="server"/>
   
    <h2>My Test Button</h2> 
       
    <ext:Button
        Text="My Test"
        runat="server">
        <DirectEvents>
            <Click OnEvent="Button_Click" Failure="alert(response.responseText)"/>
        </DirectEvents>
    </ext:Button>
</body>
</html>
