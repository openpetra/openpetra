<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MSysMan.TChangePassword"
    src="ChangePassword.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Change Password</title>
<script type="text/javascript">
        function HidePasswordWindow() {
            parent.winChangePassword.hide();
            ChangePasswordForm.getForm().reset();
        };
</script>        
    
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false" />

        <ext:FormPanel 
            ID="ChangePasswordForm" 
            runat="server"
            Region="Center"
            Frame="false"
            AutoHeight="true"
            MonitorValid="true"
            PaddingSummary="10px 10px 0 10px"
            LabelWidth="150">                
            <Defaults>
                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
            </Defaults>
            <Items>
                <ext:TextField ID="OldPassword" DataIndex="OldPassword" runat="server" FieldLabel="Old Password" InputType="Password"/>
                <ext:TextField ID="NewPassword1" DataIndex="NewPassword1" runat="server" FieldLabel="New Password" InputType="Password"/>
                <ext:TextField ID="NewPassword2" DataIndex="NewPassword2" runat="server" FieldLabel="New Password (second time)" InputType="Password"/>
                <ext:Label runat="server" Text="Please use a secure password. At least 8 characters, and a mix of digits and letters."/>
            </Items>
            <Buttons>
                <ext:Button ID="ChangePasswordButton" runat="server" Text="Change Password">
                    <DirectEvents>
                        <Click OnEvent="ChangePassword">
                            <EventMask ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="#{ChangePasswordForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" Text="Cancel">
                    <Listeners>
                        <Click Handler="HidePasswordWindow();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </form>
</body>
</html>
