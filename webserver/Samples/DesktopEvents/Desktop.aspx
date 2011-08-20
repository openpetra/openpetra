<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.TPageOnlineApplication"
    validateRequest="false"
    src="Desktop.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>OpenPetra TeenStreet Registration Backend</title>    
    
    <style type="text/css">        
        .start-button {
            background-image: url(vista_start_button.gif) !important;
        }
        
        .shortcut-icon {
            width: 48px;
            height: 48px;
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src="window.png", sizingMethod="scale");
        }
        
        .icon-grid48 {
            background-image: url(grid48x48.png) !important;
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src="grid48x48.png", sizingMethod="scale");
        }
        
        .icon-user48 {
            background-image: url(user48x48.png) !important;
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src="user48x48.png", sizingMethod="scale");
        }
        
        .icon-window48 {
            background-image: url(window48x48.png) !important;
            filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src="window48x48.png", sizingMethod="scale");
        }
        
        .desktopEl {
            position: absolute !important;
        }
    </style>
<script type="text/javascript">
        function ShowMask(message)
        {
          Ext.net.Mask.show({msg:message,msgCls:null});
        }
        function TestAlert(message)
        {
          alert(message);
        }

</script>        
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" />
    
        <ext:Desktop 
            ID="MyDesktop" 
            runat="server" 
            BackgroundColor="Black" 
            ShortcutTextColor="White" 
            Wallpaper="desktop.jpg">
            <StartButton Text="OpenPetra" IconCls="start-button" />

            <Modules>
                <ext:DesktopModule ModuleID="DesktopModule2" WindowID="winApplications" AutoRun="true">
                    <Launcher ID="Launcher2" runat="server" Text="Applications" Icon="Lorry" />
                </ext:DesktopModule>
            </Modules>  
            
            <Shortcuts>
                <ext:DesktopShortcut ModuleID="DesktopModule2" Text="Applications" IconCls="shortcut-icon icon-grid48" />
            </Shortcuts>
            
            <StartMenu Width="400" Height="400" ToolsWidth="227" Title="Start Menu">
                <ToolItems>
                    
                    <ext:MenuSeparator />
                    
                </ToolItems>
                
                <Items>
                    <ext:MenuItem ID="MenuItem1" runat="server" Text="All" Icon="Folder" HideOnClick="false">
                        <Menu>
                            <ext:Menu ID="Menu1" runat="server">
                                <Items>
                                    <ext:MenuItem Text="Applications" Icon="Lorry">
                                        <Listeners>
                                            <Click Handler="#{winApplications}.show();" />
                                        </Listeners>
                                    </ext:MenuItem>
                                </Items>
                            </ext:Menu>
                        </Menu>
                    </ext:MenuItem>
                    <ext:MenuSeparator />
                </Items>
            </StartMenu>
        </ext:Desktop>
        
        <ext:DesktopWindow 
            ID="winApplications" 
            runat="server" 
            InitCenter="false"
            Title="Applications" 
            Icon="Lorry"             
            Width="700"
            Height="520"
            PageX="200" 
            PageY="125"
            Maximized="true"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:Panel 
                    ID="FormPanelTop" 
                    runat="server" 
                    Region="Center"
                    Split="true"
                    Margins="0 5 5 5"
                    Title="" 
                    Height="120">
                    <Content>
                        <table>
                        <tr>
                        <td><ext:Button ID="btnTest" runat="server" Text="Test Download">
                            <DirectEvents>
                                <Click OnEvent="TestDownload" IsUpload="true"/>
                            </DirectEvents>
                        </ext:Button></td>
                        <td><ext:Button ID="btnTest2" runat="server" Text="Test Alert">
                            <DirectEvents>
                                <Click OnEvent="TestAlert"/>
                            </DirectEvents>
                        </ext:Button></td>
                        </tr></table>
                    </Content>
                </ext:Panel>
            </Items>
        </ext:DesktopWindow>

    </form>
</body>
</html>
