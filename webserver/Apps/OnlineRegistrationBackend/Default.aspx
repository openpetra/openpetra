<%@ Page Language="C#" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Petra.Server.app.WebService" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="PetraWebService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Desktop - Ext.NET Examples</title>
    
    <script runat="server">
        protected void Button1_Click(object sender, DirectEventArgs e)
        {
            // Do some Authentication...
            Session ["UserName"] = this.txtUsername.Text;
            TOpenPetraOrg myServer = new TOpenPetraOrg();
            myServer.Login(this.txtUsername.Text, this.txtPassword.Text);

            if (myServer.IsUserLoggedIn())
            {
                Window1.Close();
                // open application
                Response.Redirect("Desktop.aspx");
            }
            else
            {
                X.Msg.Alert("Error", "Login did not work").Show();
            }
        }
    </script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false" />
        
        <ext:Window 
            ID="Window1" 
            runat="server" 
            Closable="false"
            Resizable="false"
            Height="150" 
            Icon="Lock" 
            Title="Login to OpenPetra Preview"
            Draggable="false"
            Width="350"
            Modal="true"
            Padding="5"
            Layout="Form">
            <Items>
                <ext:TextField 
                    ID="txtUsername" 
                    runat="server" 
                    FieldLabel="Username" 
                    AllowBlank="false"
                    BlankText="Your username is required."
                    Text="Demo"
                    />
                <ext:TextField 
                    ID="txtPassword" 
                    runat="server" 
                    InputType="Password" 
                    FieldLabel="Password" 
                    AllowBlank="false" 
                    BlankText="Your password is required."
                    Text="demo"
                    />
            </Items>
            <Buttons>
                <ext:Button ID="Button1" runat="server" Text="Login" Icon="Accept">
                    <DirectEvents>
                        <Click OnEvent="Button1_Click" Success="">
                            <EventMask ShowMask="true" Msg="Verifying..." MinDelay="1000" />
                        </Click>
                    </DirectEvents>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
    <ext:KeyMap runat="server" Target="={#{Window1}.getBody()}">
        <ext:KeyBinding>
            <Keys>
                <ext:Key Code="ENTER" />
            </Keys>
            <Listeners>
                <Event Handler="e.stopEvent();#{Button1}.fireEvent('click');" />
            </Listeners>
        </ext:KeyBinding>
     </ext:KeyMap>
</body>
</html>