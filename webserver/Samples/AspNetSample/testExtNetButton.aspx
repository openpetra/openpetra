<%@ Page Language="C#"%>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<script runat="server">
        protected void Button_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Alert("Server Time", DateTime.Now.ToLongTimeString()).Show();
        }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>My Ext.Net Sample</title>
</head>
<body>

    <ext:ResourceManager ID="ResourceManager1" runat="server" CleanResourceUrl="false" />
   
    <h2>My Test Button</h2>
       
    <ext:Button
        Text="My Test"
        runat="server">
        <DirectEvents>
            <Click OnEvent="Button_Click" Failure="alert('ClickEvent failure: ' + response.responseText)"/>
        </DirectEvents>
    </ext:Button>
</body>
</html>
