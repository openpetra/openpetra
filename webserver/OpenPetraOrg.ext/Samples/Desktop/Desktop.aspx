<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="PetraServerWebService" %>
<%@ Assembly Name="Ict.Petra.Server.MConference" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="PetraWebService" %>
<%@ Import Namespace="Ict.Petra.Server.MConference.Applications" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        // check for valid user
        TOpenPetraOrg myServer = new TOpenPetraOrg();
        if (!myServer.IsUserLoggedIn())
        {
            this.Response.Redirect("Default.aspx");
            return;
        }
        
        if (!X.IsAjaxRequest)
        {
            MyData_Refresh(null, null);
        }
    }

    private object[] DataTableToArray(DataTable ATable)
    {
        ArrayList Result = new ArrayList();
        foreach (DataRow row in ATable.Rows)
        {
            object[]NewRow = new object[ATable.Columns.Count];
            for (int count = 0; count < ATable.Columns.Count; count++)
            {
                NewRow[count] = row[count];
            }
            
            Result.Add(NewRow);
        }
        
        return Result.ToArray();
    }

    /// load data from the database
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        // TODO get the current sitekey of the user
        // TODO offer all available conferences???

        DataTable test = TApplicationManagement.GetApplications("SC001CNGRSS08", 43000000);
        
        this.Store1.DataSource = DataTableToArray(test);
        this.Store1.DataBind();
    }

    protected void Logout_Click(object sender, DirectEventArgs e)
    {
        // Logout from Authenticated Session
        TOpenPetraOrg myServer = new TOpenPetraOrg();
        myServer.Logout();
        this.Response.Redirect("Default.aspx");
    }

</script>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Desktop - Ext.NET Examples</title>    
    
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
        var alignPanels = function () {
            pnlSample.getEl().alignTo(Ext.getBody(), "tr", [-505, 5], false)
        };

        var template = '<span style="color:{0};">{1}</span>';

        var change = function (value) {
            return String.format(template, (value > 0) ? "green" : "red", value);
        };

        var pctChange = function (value) {
            return String.format(template, (value > 0) ? "green" : "red", value + "%");
        };

        var createDynamicWindow = function (app) {
            var desk = app.getDesktop();

            var w = desk.createWindow({
                title  : "Dynamic Web Browser",
                width  : 1000,
                height : 600,
                maximizable : true,
                minimizable : true,
                autoLoad : {
                    url  : "http://ajaxian.com/archives/mad-cool-date-library/",
                    mode : "iframe",
                    showMask : true
                }
            });

            w.center();
            w.show();
        };
    </script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server">
            <Listeners>
                <DocumentReady Handler="alignPanels();" />
                <WindowResize Handler="alignPanels();" />
            </Listeners>
        </ext:ResourceManager>
        
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
                    <ext:MenuItem Text="Logout" Icon="Disconnect">
                        <DirectEvents>
                            <Click OnEvent="Logout_Click">
                                <EventMask ShowMask="true" Msg="Good Bye..." MinDelay="1000" />
                            </Click>
                        </DirectEvents>
                    </ext:MenuItem>
                    
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
        
        <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="PartnerKey" />
                        <ext:RecordField Name="FirstName" />
                        <ext:RecordField Name="FamilyName" />
                        <ext:RecordField Name="Gender"/>
                        <ext:RecordField Name="DateOfBirth" Type="Date" DateFormat="yyyy-MM-dd" />
                        <ext:RecordField Name="ApplicationDate" Type="Date" DateFormat="yyyy-MM-dd" />
                        <ext:RecordField Name="ApplicationStatus" />
                        <ext:RecordField Name="Role" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>
        
        <ext:DesktopWindow 
            ID="winApplications" 
            runat="server" 
            InitCenter="false"
            Title="Applications" 
            Icon="Lorry"             
            Width="700"
            Height="320"
            PageX="200" 
            PageY="125"
            Layout="Fit">
            <TopBar>
                <ext:Toolbar ID="ToolBar1" runat="server">
                    <Items>
                        <ext:Button ID="btnLoad" runat="server" Text="Reload" Icon="ArrowRefresh">
                            <Listeners>
                                <Click Handler="#{GridPanel1}.load();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="extbtnedit" runat="server" Icon="Add">
                            <ToolTips>
                                <ext:ToolTip ID="ToolTip2" Title="Edit Entry" runat="server" Html="Edit" />
                            </ToolTips>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>           
            <Items>
                <ext:GridPanel 
                    ID="GridPanel1" 
                    runat="server" 
                    StoreID="Store1" 
                    StripeRows="true"
                    Border="false">
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:Column Header="Person Key" Width="80" DataIndex="PartnerKey"/>
                            <ext:Column ColumnID="FamilyName" Header="Family Name" Width="90" DataIndex="FamilyName" />
                            <ext:Column Header="First Name" Width="90" DataIndex="FirstName"/>
                            <ext:Column Header="Date of Birth" Width="100" DataIndex="DateOfBirth"/>
                            <ext:Column Header="Gender" Width="90" DataIndex="Gender"/>
                            <ext:Column Header="Date Applied" Width="100" DataIndex="ApplicationDate"/>
                            <ext:Column Header="Application Status" Width="120" DataIndex="ApplicationStatus"/>
                            <ext:Column Header="Role" Width="120" DataIndex="Role"/>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="10" StoreID="Store1" />
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:DesktopWindow>
        
    </form>
</body>
</html>
