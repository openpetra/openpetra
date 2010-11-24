<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="PetraServerWebService" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="PetraWebService" %>

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

    private object[] TestData
    {
        get
        {
            DateTime now = DateTime.Now;
            
            return new object[]
            {
                new object[] { "3m Co", 71.72, 0.02, 0.03, now },
                new object[] { "Alcoa Inc", 29.01, 0.42, 1.47, now },
                new object[] { "Altria Group Inc", 83.81, 0.28, 0.34, now },
                new object[] { "American Express Company", 52.55, 0.01, 0.02, now },
                new object[] { "American International Group, Inc.", 64.13, 0.31, 0.49, now },
                new object[] { "AT&T Inc.", 31.61, -0.48, -1.54, now },
                new object[] { "Boeing Co.", 75.43, 0.53, 0.71, now },
                new object[] { "Caterpillar Inc.", 67.27, 0.92, 1.39, now },
                new object[] { "Citigroup, Inc.", 49.37, 0.02, 0.04, now },
                new object[] { "E.I. du Pont de Nemours and Company", 40.48, 0.51, 1.28, now },
                new object[] { "Exxon Mobil Corp", 68.1, -0.43, -0.64, now },
                new object[] { "General Electric Company", 34.14, -0.08, -0.23, now },
                new object[] { "General Motors Corporation", 30.27, 1.09, 3.74, now },
                new object[] { "Hewlett-Packard Co.", 36.53, -0.03, -0.08, now },
                new object[] { "Honeywell Intl Inc", 38.77, 0.05, 0.13, now },
                new object[] { "Intel Corporation", 19.88, 0.31, 1.58, now },
                new object[] { "International Business Machines", 81.41, 0.44, 0.54, now },
                new object[] { "Johnson & Johnson", 64.72, 0.06, 0.09, now },
                new object[] { "JP Morgan & Chase & Co", 45.73, 0.07, 0.15, now },
                new object[] { "McDonald\"s Corporation", 36.76, 0.86, 2.40, now },
                new object[] { "Merck & Co., Inc.", 40.96, 0.41, 1.01, now },
                new object[] { "Microsoft Corporation", 25.84, 0.14, 0.54, now },
                new object[] { "Pfizer Inc", 27.96, 0.4, 1.45, now },
                new object[] { "The Coca-Cola Company", 45.07, 0.26, 0.58, now },
                new object[] { "The Home Depot, Inc.", 34.64, 0.35, 1.02, now },
                new object[] { "The Procter & Gamble Company", 61.91, 0.01, 0.02, now },
                new object[] { "United Technologies Corporation", 63.26, 0.55, 0.88, now },
                new object[] { "Verizon Communications", 35.57, 0.39, 1.11, now },
                new object[] { "Wal-Mart Stores, Inc.", 45.45, 0.73, 1.63, now }
            };
        }
    }
    
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        // TODO: load data from WebConnector
        
        this.Store1.DataSource = this.TestData;
        this.Store1.DataBind();
    }

    protected void Logout_Click(object sender, DirectEventArgs e)
    {
        // Logout from Authenticated Session
        TOpenPetraOrg myServer = new TOpenPetraOrg();
        myServer.Logout();
        this.Response.Redirect("Default.aspx");
    }

    [DirectMethod]
    public Customer AddCustomer()
    {
        Customer customer = new Customer();

        customer.ID = 99;
        customer.FirstName = this.txtFirstName.Text;
        customer.LastName = this.txtLastName.Text;
        customer.Company = this.txtCompany.Text;
        customer.Country = new Country(this.cmbCountry.SelectedItem.Value);
        customer.Premium = this.chkPremium.Checked;
        customer.DateCreated = DateTime.Now;

        return customer;
    }
    
    // Define Customer Class
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public Country Country { get; set; }
        public bool Premium { get; set; }
        public DateTime DateCreated { get; set; }
    }

    // Define Country Class
    public class Country
    {
        public Country(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
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
                <ext:DesktopModule ModuleID="DesktopModule2" WindowID="winCompany" AutoRun="true">
                    <Launcher ID="Launcher2" runat="server" Text="Registrations" Icon="Lorry" />
                </ext:DesktopModule>
            </Modules>  
            
            <Shortcuts>
                <ext:DesktopShortcut ModuleID="DesktopModule2" Text="Registrations" IconCls="shortcut-icon icon-grid48" />
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
                                    <ext:MenuItem Text="Registrations" Icon="Lorry">
                                        <Listeners>
                                            <Click Handler="#{winCompany}.show();" />
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
                        <ext:RecordField Name="company" />
                        <ext:RecordField Name="price" Type="Float" />
                        <ext:RecordField Name="change" Type="Float" />
                        <ext:RecordField Name="pctChange" Type="Float" />
                        <ext:RecordField Name="lastChange" Type="Date" DateFormat="yyyy-MM-ddTHH:mm:ss" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>
        
        <ext:DesktopWindow 
            ID="winCustomer" 
            runat="server" 
            Title="Add Customer" 
            InitCenter="false"
            Icon="User" 
            Padding="5"
            Width="350"
            Height="200"
            PageX="100" 
            PageY="25"
            Layout="Form">
            <Items>
                <ext:TextField ID="txtFirstName" runat="server" FieldLabel="First Name" Text="Steve" AnchorHorizontal="100%" />
                <ext:TextField ID="txtLastName" runat="server" FieldLabel="Last Name" Text="Caballero" AnchorHorizontal="100%" />
                <ext:TextField ID="txtCompany" runat="server" FieldLabel="Company" Text="Awesome Industries" AnchorHorizontal="100%" />
                <ext:ComboBox ID="cmbCountry" runat="server" FieldLabel="Country" AnchorHorizontal="100%">
                    <SelectedItem Value="United States" />
                    <Items>
                        <ext:ListItem Text="Australia" />
                        <ext:ListItem Text="Canada" />
                        <ext:ListItem Text="Great Britian" />
                        <ext:ListItem Text="Japan" />
                        <ext:ListItem Text="United States" />
                    </Items>
                </ext:ComboBox>
                <ext:Checkbox ID="chkPremium" runat="server" FieldLabel="Premium Member" Checked="true" AnchorHorizontal="100%" />
            </Items>
            <Buttons>
                <ext:Button ID="btnSaveCustomer" runat="server" Text="Save" Icon="Disk">
                    <Listeners>
                        <Click Handler="Ext.net.DirectMethods.AddCustomer({
                            success: function (customer) {
                                var template = 'ID: {0}{7} Name: {1} {2}{7} Company: {3}{7} Country: {4}{7} Premium Member: {5}{7} Date Created: {6}{7}',
                                    msg = String.format(template, 
                                            customer.ID, 
                                            customer.FirstName, 
                                            customer.LastName, 
                                            customer.Company, 
                                            customer.Country.Name, 
                                            customer.Premium, 
                                            customer.DateCreated,
                                            '&lt;br /&gt;&lt;br /&gt;');
                                
                                Ext.Msg.alert('Customer Saved', msg);
                            }
                        });" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:DesktopWindow>
        
        <ext:DesktopWindow 
            ID="winCompany" 
            runat="server" 
            InitCenter="false"
            Title="Registrations" 
            Icon="Lorry"             
            Width="550"
            Height="320"
            PageX="200" 
            PageY="125"
            Layout="Fit">
            <TopBar>
                <ext:Toolbar ID="ToolBar1" runat="server">
                    <Items>
                        <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                            <Listeners>
                                <Click Handler="#{GridPanel1}.save();" />
                            </Listeners>
                        </ext:Button>
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
                    Border="false"
                    AutoExpandColumn="Company">
                    <ColumnModel runat="server">
                        <Columns>
                            <ext:Column ColumnID="Company" Header="Company" Width="160" DataIndex="company" />
                            <ext:Column Header="Price" Width="75" DataIndex="price">
                                <Renderer Format="UsMoney" />
                            </ext:Column>
                            <ext:Column Header="Change" Width="75" DataIndex="change">
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header="Change" Width="75" DataIndex="pctChange">
                                <Renderer Fn="pctChange" />
                            </ext:Column>
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
