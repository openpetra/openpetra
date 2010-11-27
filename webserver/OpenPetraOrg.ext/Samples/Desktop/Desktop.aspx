<%@ Page Language="C#" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="PetraServerWebService" %>
<%@ Assembly Name="Ict.Petra.Server.MConference" %>
<%@ Assembly Name="Ict.Petra.Shared.MConference.Datasets" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="PetraWebService" %>
<%@ Import Namespace="Ict.Petra.Server.MConference.Applications" %>
<%@ Import Namespace="Ict.Petra.Shared.MConference.Data" %>

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
            RoleData_Refresh(null, null);
            ApplicationStatus_Refresh(null, null);
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

        ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];
        if (CurrentApplicants == null || sender != null)
        {
            CurrentApplicants = TApplicationManagement.GetApplications("SC001CNGRSS08", 43000000);
            Session["CURRENTAPPLICANTS"] = CurrentApplicants;
            this.FormPanel1.SetValues(new {});
            this.FormPanel1.Disabled = true;
        }

        this.Store1.DataSource = DataTableToArray(CurrentApplicants.ApplicationGrid);
        this.Store1.DataBind();
    }

    protected void RoleData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        // TODO: load from database
        this.StoreRole.DataSource = new object[]
        {
            new object[] { "TS-TEEN-A" },
            new object[] { "TS-TEEN-O" },
            new object[] { "TS-SERVE" }
        };
        
        this.StoreRole.DataBind();
    }

    protected void ApplicationStatus_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        // TODO: load from database
        this.StoreApplicationStatus.DataSource = new object[]
        {
            new object[] { "H", "On Hold" },
            new object[] { "A", "Accepted" },
            new object[] { "R", "Rejected" },
            new object[] { "C", "Cancelled" }
        };
        
        this.StoreApplicationStatus.DataBind();
    }

    protected void RowSelect(object sender, DirectEventArgs e)
    {
        Int64 PartnerKey = Convert.ToInt64(e.ExtraParams["PartnerKey"]);

        ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];
        CurrentApplicants.ApplicationGrid.DefaultView.RowFilter = "p_partner_key_n = " + PartnerKey.ToString();

        ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)CurrentApplicants.ApplicationGrid.DefaultView[0].Row;
        Session["CURRENTROW"] = row;

        this.FormPanel1.Disabled = false;

        this.FormPanel1.SetValues(new {
            row.PartnerKey,
            row.FirstName,                          
            row.FamilyName,
            row.Gender,
            row.DateOfBirth,
            row.GenAppDate,
            row.GenApplicationStatus,
            row.StCongressCode
        });
    }

    protected void SaveApplication(object sender, DirectEventArgs e)
    {
        ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];
        
        //Console.WriteLine(e.ExtraParams["Values"]);
        
        Dictionary<string,string> values = JSON.Deserialize<Dictionary<string, string>>(e.ExtraParams["Values"]);
        
        row.FamilyName = values["FamilyName"];
        row.FirstName = values["FirstName"];
        row.Gender = values["Gender"];
        if (values["DateOfBirth"].Length == 0)
        {
            row.SetDateOfBirthNull();
        }
        else
        {
            row.DateOfBirth = Convert.ToDateTime(values["DateOfBirth"]);
        }
        row.GenAppDate = Convert.ToDateTime(values["GenAppDate"]);
        row.StCongressCode = values["StCongressCode_Value"];
        row.GenApplicationStatus = values["GenApplicationStatus_Value"];

        ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];
        if (TApplicationManagement.SaveApplications(ref CurrentApplicants) != TSubmitChangesResult.scrOK)
        {
            X.Msg.Alert("Error", "Saving did not work").Show();            
        }
        
        MyData_Refresh(null, null);
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

        <ext:Store ID="StoreRole" runat="server" OnRefreshData="RoleData_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="RoleCode" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>

        <ext:Store ID="StoreApplicationStatus" runat="server" OnRefreshData="ApplicationStatus_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="StatusCode" />
                        <ext:RecordField Name="StatusDescription" />
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
            Height="520"
            PageX="200" 
            PageY="125"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:GridPanel 
                    ID="GridPanel1" 
                    runat="server" 
                    Region="Center"
                    StoreID="Store1" 
                    StripeRows="true"
                    Border="false"
                    Frame="true">
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
                        <ext:RowSelectionModel runat="server" SingleSelect="true">
                            <DirectEvents>
                                <RowSelect OnEvent="RowSelect" Buffer="100">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{FormPanel1}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="PartnerKey" Value="this.getSelected().data['PartnerKey']" Mode="Raw" />
                                    </ExtraParams>
                                </RowSelect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="10" StoreID="Store1" />
                    </BottomBar>
                </ext:GridPanel>
                <ext:FormPanel 
                    ID="FormPanel1" 
                    runat="server" 
                    Region="South"
                    Split="true"
                    Margins="0 5 5 5"
                    Title="Application Details" 
                    Height="280"
                    Icon="User">
                    <Items>
                        <ext:Toolbar ID="ToolBar1" runat="server">
                            <Items>
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                    <DirectEvents>
                                        <Click OnEvent="SaveApplication">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="FormPanel1.getForm().getValues(false)" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                        <ext:TextField ID="PartnerKey" runat="server" FieldLabel="Partner Key" DataIndex="PartnerKey" ReadOnly="true"/>
                        <ext:TextField ID="FirstName" runat="server" FieldLabel="First Name" DataIndex="FirstName" />
                        <ext:TextField ID="FamilyName" runat="server" FieldLabel="Family Name" DataIndex="FamilyName" />
                        <ext:TextField ID="Gender" runat="server" FieldLabel="Gender" DataIndex="Gender" />
                        <ext:DateField ID="DateOfBirth" runat="server" FieldLabel="Date of Birth" DataIndex="DateOfBirth" Format="yyyy-MM-dd"/>
                        <ext:DateField ID="GenAppDate" runat="server" FieldLabel="Date of Application" DataIndex="GenAppDate" Format="yyyy-MM-dd" />
                        <ext:ComboBox 
                            ID="GenApplicationStatus"
                            runat="server" 
                            FieldLabel="Application Status" 
                            DataIndex="GenApplicationStatus"
                            StoreID="StoreApplicationStatus"
                            Editable="false"
                            TypeAhead="true" 
                            ForceSelection="true"
                            EmptyText="Select a status..."
                            DisplayField="StatusDescription"
                            ValueField="StatusCode"
                            Mode="Local"
                            Resizable="true"
                            SelectOnFocus="true"                            
                        />
                        <ext:ComboBox 
                            ID="StCongressCode"
                            runat="server" 
                            FieldLabel="Role" 
                            DataIndex="StCongressCode"
                            StoreID="StoreRole"
                            Editable="false"
                            TypeAhead="true" 
                            ForceSelection="true"
                            EmptyText="Select a role..."
                            DisplayField="RoleCode"
                            ValueField="RoleCode"
                            Mode="Local"
                            Resizable="true"
                            SelectOnFocus="true"                            
                        />
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:DesktopWindow>
        
    </form>
</body>
</html>
