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
    <title>Online Registration Backend</title>
    
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
        var submitValue = function (grid) {
            grid.submitData(false);
        };

        function ShowTabPanel(panelName)
        {
            Ext.getCmp('TabPanelApplication').unhideTabStripItem(panelName);
        }

        function HideTabPanel(panelName)
        {
            Ext.getCmp('TabPanelApplication').hideTabStripItem(panelName);
            Ext.getCmp('TabPanelApplication').setActiveTab(0);
        }

        function SetDateFormat(format)
        {
            Ext.getCmp('DateOfArrival').format = format;
            Ext.getCmp('DateOfDeparture').format = format;
            Ext.getCmp('GenAppDate').format = format;
            Ext.getCmp('DateOfBirth').format = format;
        }

        function SetActiveMedicalIncident(panelId)
        {
            var myTabPanel = Ext.getCmp('MedicalPanel');
            var tabs = myTabPanel.find( 'title', 'incident ' + (panelId + 1));
            myTabPanel.setActiveTab( tabs[ 0 ] );
        }

        var MedicalIncidentToDelete = -1;
        
        function ReallyDeleteMedicalIncident(btn)
        {
            if (btn == 'yes')
            {
                tab = document.getElementById('TabMedicalIncident' + MedicalIncidentToDelete);
                tab.parentNode.removeChild(tab);
            }
        }

        function DeleteMedicalIncident(RowId)
        {
            MedicalIncidentToDelete = RowId;
            Ext.MessageBox.confirm('Confirm', 'Are you sure you want to delete this incident?', ReallyDeleteMedicalIncident);
        }

</script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false" />
    
        <ext:Desktop 
            ID="MyDesktop" 
            runat="server" 
            BackgroundColor="Black" 
            ShortcutTextColor="White" 
            Wallpaper="desktop.jpg">
            <StartButton Text="OpenPetra" IconCls="start-button" />

            <Modules>
                <ext:DesktopModule ModuleID="DesktopModule2" WindowID="winApplications" AutoRun="false">
                    <Launcher ID="Launcher2" runat="server" Text="Applications" Icon="Lorry" />
                </ext:DesktopModule>
                <ext:DesktopModule ModuleID="DesktopModule3" WindowID="winHeadsets" AutoRun="false">
                    <Launcher ID="Launcher4" runat="server" Text="Headsets" Icon="Lorry" />
                </ext:DesktopModule>
                <ext:DesktopModule ModuleID="ChangePassword" WindowID="winChangePassword" AutoRun="false">
                    <Launcher ID="Launcher3" runat="server" Text="Change Password"/>
                </ext:DesktopModule>
            </Modules>  
            
            <Shortcuts>
                <ext:DesktopShortcut ModuleID="DesktopModule2" Text="Applications" IconCls="shortcut-icon icon-grid48" />
                <ext:DesktopShortcut ModuleID="DesktopModule3" Text="Headsets" IconCls="shortcut-icon icon-grid48" />
                <ext:DesktopShortcut ModuleID="ChangePassword" Text="Change Password" IconCls="shortcut-icon icon-grid48" />
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
        
        <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh" OnSubmitData="DownloadPetra">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="PartnerKey" />
                        <ext:RecordField Name="PersonKey" />
                        <ext:RecordField Name="FirstName" />
                        <ext:RecordField Name="FamilyName" />
                        <ext:RecordField Name="Gender"/>
                        <ext:RecordField Name="DateOfBirth" Type="Date"/>
                        <ext:RecordField Name="ApplicationDate" Type="Date"/>
                        <ext:RecordField Name="ApplicationStatus" /> 
                        <ext:RecordField Name="Role" />
                        <ext:RecordField Name="StFgCode" />
                        <ext:RecordField Name="StFgLeader" />
                        <ext:RecordField Name="BadgePrint" />
                        <ext:RecordField Name="FieldCharged" />
                        <ext:RecordField Name="ApplicationKey" />
                        <ext:RecordField Name="DateOfArrival" Type="Date"/>
                        <ext:RecordField Name="DateOfDeparture" Type="Date"/>
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

        <ext:Store ID="StoreServiceTeamJob" runat="server" OnRefreshData="ServiceTeamJobs_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="JobTitle" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>

        <ext:Store ID="StoreRegistrationOffice" runat="server" OnRefreshData="RegistrationOffice_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="PartnerKey" />
                        <ext:RecordField Name="PartnerShortName" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>

        <ext:Store ID="StoreRebukes" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="ID">
                    <Fields>
                        <ext:RecordField Name="ID" Type="Int" />
                        <ext:RecordField Name="When" Type="Date"/>
                        <ext:RecordField Name="Time"/>
                        <ext:RecordField Name="What"/>
                        <ext:RecordField Name="Consequence"/>
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>

        <ext:DesktopWindow 
            ID="winUploadPhoto" 
            runat="server" 
            Title="Upload another photo" 
            Width="500"
            Height="120"
            PageX="200" 
            PageY="125"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:FormPanel 
                            ID="UploadForm" 
                            runat="server"
                            Region="Center"
                            Width="300"
                            Frame="false"
                            AutoHeight="true"
                            MonitorValid="true"
                            PaddingSummary="10px 10px 0 10px"
                            LabelWidth="50">                
                            <Defaults>
                                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                            </Defaults>
                            <Items>
                                <ext:FileUploadField 
                                    ID="FileUploadField1" 
                                    runat="server" 
                                    EmptyText="Select an image"
                                    FieldLabel="Photo"
                                    ButtonText=""
                                    Icon="ImageAdd"
                                    />
                            </Items>
                            <Listeners>
                                <ClientValidation Handler="#{SaveButton}.setDisabled(!valid);" />
                            </Listeners>
                            <Buttons>
                                <ext:Button ID="SaveButton" runat="server" Text="Upload">
                                    <DirectEvents>
                                        <Click 
                                            OnEvent="UploadClick"
                                            Before="if (!#{UploadForm}.getForm().isValid()) { return false; } 
                                                Ext.Msg.wait('Uploading your photo...', 'Uploading');"
                                            Success="#{winUploadPhoto}.hide(); #{UploadForm}.getForm().reset();"
                                            Failure="Ext.Msg.show({ 
                                                title   : 'Error', 
                                                msg     : 'Error during uploading', 
                                                minWidth: 200, 
                                                modal   : true, 
                                                icon    : Ext.Msg.ERROR, 
                                                buttons : Ext.Msg.OK 
                                            });">
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="FormPanel1.getForm().getValues(false)" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel">
                                    <Listeners>
                                        <Click Handler="#{winUploadPhoto}.hide();#{UploadForm}.getForm().reset();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
        
            </Items>
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winUploadPetraFile" 
            runat="server" 
            Title="Upload the result of the Petra import" 
            Width="500"
            Height="120"
            PageX="200" 
            PageY="125"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:FormPanel 
                            ID="PetraUploadForm" 
                            runat="server"
                            Region="Center"
                            Width="300"
                            Frame="false"
                            AutoHeight="true"
                            MonitorValid="true"
                            PaddingSummary="10px 10px 0 10px"
                            LabelWidth="50">                
                            <Defaults>
                                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                            </Defaults>
                            <Items>
                                <ext:FileUploadField 
                                    ID="FileUploadField2" 
                                    runat="server" 
                                    EmptyText="Select a Petra file"
                                    FieldLabel="Petra File"
                                    ButtonText=""
                                    Icon="ImageAdd"
                                    />
                            </Items>
                            <Listeners>
                                <ClientValidation Handler="#{UploadPetraFileButton}.setDisabled(!valid);" />
                            </Listeners>
                            <Buttons>
                                <ext:Button ID="UploadPetraFileButton" runat="server" Text="Upload">
                                    <DirectEvents>
                                        <Click 
                                            OnEvent="UploadPetraImportClick"
                                            Before="if (!#{PetraUploadForm}.getForm().isValid()) { return false; } 
                                                Ext.Msg.wait('Uploading your file...', 'Uploading');"
                                            Success="#{winUploadPetraFile}.hide(); #{PetraUploadForm}.getForm().reset();"
                                            Failure="Ext.Msg.show({ 
                                                title   : 'Error', 
                                                msg     : 'Error during uploading', 
                                                minWidth: 200, 
                                                modal   : true, 
                                                icon    : Ext.Msg.ERROR, 
                                                buttons : Ext.Msg.OK 
                                            });">
                                            <ExtraParams>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel">
                                    <Listeners>
                                        <Click Handler="#{winUploadPetraFile}.hide();#{PetraUploadForm}.getForm().reset();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
        
            </Items>
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winUploadPetraExtract" 
            runat="server" 
            Title="Upload a Petra Extract" 
            Width="500"
            Height="150"
            PageX="200" 
            PageY="125"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:FormPanel 
                            ID="PetraExtractUploadForm" 
                            runat="server"
                            Region="Center"
                            Width="300"
                            Frame="false"
                            AutoHeight="true"
                            MonitorValid="true"
                            PaddingSummary="10px 10px 0 10px"
                            LabelWidth="50">                
                            <Defaults>
                                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                            </Defaults>
                            <Items>
                                <ext:FileUploadField 
                                    ID="FileUploadField3" 
                                    runat="server" 
                                    EmptyText="Select a Petra Extract"
                                    FieldLabel="Petra Extract"
                                    ButtonText=""
                                    Icon="ImageAdd"
                                    />
                                <ext:ComboBox 
                                    ID="FileUploadCodePage3"
                                    runat="server" 
                                    FieldLabel="Code Page" 
                                    Editable="false"
                                    ForceSelection="true"
                                    EmptyText="Select encoding..."
                                    Mode="Local"
                                    Width="260"
                                    LabelWidth="150"
                                    SelectOnFocus="true"
                                    SelectedIndex = "0">
                                    <Items>
                                        <ext:ListItem Text="Western European (1252)" Value="1252" />
                                        <ext:ListItem Text="Central European (1250)" Value="1250" />
                                    </Items>                        
                                </ext:ComboBox>
                            </Items>
                            <Listeners>
                                <ClientValidation Handler="#{UploadPetraExtractButton}.setDisabled(!valid);" />
                            </Listeners>
                            <Buttons>
                                <ext:Button ID="UploadPetraExtractButton" runat="server" Text="Upload">
                                    <DirectEvents>
                                        <Click 
                                            OnEvent="UploadPetraExtractClick"
                                            Before="if (!#{PetraExtractUploadForm}.getForm().isValid()) { return false; } 
                                                Ext.Msg.wait('Uploading your file...', 'Uploading');"
                                            Success="#{winUploadPetraExtract}.hide(); #{PetraExtractUploadForm}.getForm().reset();"
                                            Failure="Ext.Msg.show({ 
                                                title   : 'Error', 
                                                msg     : 'Error during uploading', 
                                                minWidth: 200, 
                                                modal   : true, 
                                                icon    : Ext.Msg.ERROR, 
                                                buttons : Ext.Msg.OK 
                                            });">
                                            <ExtraParams>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel">
                                    <Listeners>
                                        <Click Handler="#{winUploadPetraExtract}.hide();#{PetraExtractUploadForm}.getForm().reset();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
        
            </Items>
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winAcceptMany" 
            runat="server" 
            Title="Accept many applicants by Registration key" 
            Width="500"
            Height="600"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:FormPanel 
                            ID="AcceptForm" 
                            runat="server"
                            Region="Center"
                            Width="300"
                            Frame="false"
                            AutoHeight="true"
                            MonitorValid="true"
                            PaddingSummary="10px 10px 0 10px"
                            LabelWidth="50">                
                            <Defaults>
                                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                            </Defaults>
                            <Items>
                                <ext:TextArea ID="RegistrationsKeys" DataIndex="RegistrationsKeys" runat="server" FieldLabel="Registration Keys" Height="360"/>
                                <ext:Label runat="server" Text="Please enter all keys of applicants that you want to set to application status 'Accepted'. "/>
                                <ext:Label runat="server" Text="You have to enter the registration key that is printed on the PDF, not the partner key from your Petra."/>
                                <ext:Label runat="server" Text="You can leave out the 000400..., so just type for example 12123 or 8651."/>
                                <ext:Label runat="server" Text="One key per line."/>
                            </Items>
                            <Buttons>
                                <ext:Button ID="AcceptButton" runat="server" Text="Accept All">
                                    <DirectEvents>
                                        <Click OnEvent="AcceptManyApplicants">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="#{AcceptForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                    <Listeners>
                                        <Click Handler="#{winAcceptMany}.hide();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel">
                                    <Listeners>
                                        <Click Handler="#{winAcceptMany}.hide();#{AcceptForm}.getForm().reset();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
        
            </Items>
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winJSONApplication" 
            runat="server" 
            Title="Import Application from JSON data" 
            Width="500"
            Height="600"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <TopBar>
            </TopBar>           
            <Items>
                <ext:FormPanel 
                            ID="JSONApplicationForm" 
                            runat="server"
                            Region="Center"
                            Width="300"
                            Frame="false"
                            AutoHeight="true"
                            MonitorValid="true"
                            PaddingSummary="10px 10px 0 10px"
                            LabelWidth="50">
                            <Defaults>
                                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                            </Defaults>
                            <Items>
                                <ext:TextArea ID="JSONData" DataIndex="JSONData" runat="server" FieldLabel="JSON Data" Height="360"/>
                                <ext:Label runat="server" Text="This is only for the data administrator to be used. We can import logged applications again."/>
                            </Items>
                            <Buttons>
                                <ext:Button ID="ImportJSONButton" runat="server" Text="Import Application">
                                    <DirectEvents>
                                        <Click OnEvent="ImportJSONApplication">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="#{JSONApplicationForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                    <Listeners>
                                        <Click Handler="#{winJSONApplication}.hide();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" Text="Cancel">
                                    <Listeners>
                                        <Click Handler="#{winJSONApplication}.hide();#{JSONApplicationForm}.getForm().reset();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:FormPanel>
        
            </Items>
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winChangePassword" 
            runat="server" 
            Title="Change your Password" 
            Width="600"
            Height="208"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="ChangePassword.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winCreateGiftBatch" 
            runat="server" 
            Title="Create Gift Batch with Payments for Participants" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="CreateGiftBatch.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>
        
        <ext:DesktopWindow 
            ID="winPrintBadgeLabelsByKey" 
            runat="server" 
            Title="Print Badge Labels By Key" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="PrintBadgeLabelsByKey.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winImportFellowshipGroups" 
            runat="server" 
            Title="Import Fellowship groups" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="ImportFellowshipGroups.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winHeadsets" 
            runat="server" 
            Title="Manage headsets" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Maximized="true"
            Layout="Border">
            <AutoLoad Url="Headsets.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winManualRegistration" 
            runat="server" 
            Title="Manual Registration" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="ManualRegistration.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

        <ext:DesktopWindow 
            ID="winImportPrintedBadges" 
            runat="server" 
            Title="Import Printed Badges" 
            Width="600"
            Height="608"
            PageX="200" 
            PageY="100"
            Layout="Border">
            <AutoLoad Url="ImportPrintedBadges.aspx" Mode="IFrame" ShowMask="true" />
        </ext:DesktopWindow>

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
                    Region="North"
                    Split="true"
                    Margins="0 5 5 5"
                    Title=""
                    AutoScroll="true"
                    Height="120">
                    <Content>
                        <table><tr><td>
                        <table>
                            <tr><td>
                        <ext:ComboBox 
                            ID="FilterStatus"
                            runat="server" 
                            FieldLabel="Filter by Application Status" 
                            Editable="false"
                            ForceSelection="true"
                            EmptyText="Filter by..."
                            Mode="Local"
                            Width="260"
                            LabelWidth="150"
                            SelectOnFocus="true"
                            SelectedIndex = "2">
                            <Items>
                                <ext:ListItem Text="All" Value="all" />
                                <ext:ListItem Text="Accepted" Value="accepted" />
                                <ext:ListItem Text="On Hold" Value="on hold" />
                                <ext:ListItem Text="Cancelled" Value="cancelled" />
                            </Items>                        
                            <DirectEvents>
                                <Select OnEvent="ChangeFilter">
                                  <EventMask ShowMask="true" Msg="Loading data..." MinDelay="1000" />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>
                        </td>
                        <td><ext:TextField ID="txtSearchApplicant" runat="server" FieldLabel="Search for Applicant" EmptyText="Key, Name, FGroup">
                            <DirectEvents>
                                <Change OnEvent="SearchApplicant"/>
                            </DirectEvents>
                        </ext:TextField>
                        </td>
                        </tr>
                        <tr><td>
                        <ext:ComboBox 
                            ID="FilterRegistrationOffice"
                            runat="server" 
                            FieldLabel="Filter by Registration Office" 
                            Editable="false"
                            ForceSelection="true"
                            EmptyText="Filter by..."
                            Mode="Local"
                            Width="260"
                            LabelWidth="150"
                            StoreID="StoreRegistrationOffice"
                            DisplayField="PartnerShortName"
                            ValueField="PartnerKey"
                            SelectOnFocus="true"
                            SelectedIndex = "0">
                            <DirectEvents>
                                <Select OnEvent="ChangeFilter">
                                  <EventMask ShowMask="true" Msg="Loading data..." MinDelay="1000" />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>
                        </td>
                        <td>
                        <ext:ComboBox 
                            ID="FilterRole"
                            runat="server" 
                            FieldLabel="Filter by Role"
                            Editable="false"
                            ForceSelection="true"
                            EmptyText="Filter by..."
                            Mode="Local"
                            Width="260"
                            LabelWidth="150"
                            StoreID="StoreRole"
                            DisplayField="RoleCode"
                            ValueField="RoleCode"
                            SelectOnFocus="true"
                            SelectedIndex = "0">
                            <DirectEvents>
                                <Select OnEvent="ChangeFilter">
                                  <EventMask ShowMask="true" Msg="Loading data..." MinDelay="1000" />
                                </Select>
                            </DirectEvents>
                        </ext:ComboBox>
                        </td>
                        </tr>
                        </table>
                        </td>
                        <td>
                        <ext:TabPanel ID="TabButtons" runat="server" EnableTabScroll="true" Height="90" Width="400">   
                          <Items>
                            <ext:Panel ID="TabPetra" runat="server" Title="Petra" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                  <td><ext:Button ID="btnDownloadPetra" runat="server" Text="Download for Petra" Icon="PageAttach">
                                    <Listeners>
                                        <Click Handler="submitValue(#{GridPanel1});" />
                                    </Listeners>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnUploadPetraFile" runat="server" Text="Upload imported Petra File" Icon="Disk">
                                      <Listeners>
                                          <Click Handler="#{winUploadPetraFile}.show();" />
                                      </Listeners>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnUploadPetraExtract" runat="server" Text="Upload Petra Extract" Icon="Disk">
                                      <Listeners>
                                          <Click Handler="#{winUploadPetraExtract}.show();" />
                                      </Listeners>
                                  </ext:Button></td>
                                </tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabManualRegistration" runat="server" Title="Manual Registration" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                  <td><ext:Button ID="btnManualRegistration" runat="server" Text="Manual Registration">
                                      <Listeners>
                                          <Click Handler="#{winManualRegistration}.show();" />
                                      </Listeners>
                                  </ext:Button></td>
                                </tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabTopFinance" runat="server" Title="Finance" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                <td><ext:Button ID="btnFinanceReport" runat="server" Text="Print Finance Report">
                                    <DirectEvents>
                                        <Click OnEvent="PrintFinanceReport"/>
                                    </DirectEvents>
                                </ext:Button></td>
                                <td><ext:Button ID="btnCreateGiftBatch" runat="server" Text="Create Gift Batch">
                                    <Listeners>
                                        <Click Handler="#{winCreateGiftBatch}.show();" />
                                    </Listeners>
                                </ext:Button></td>
                                </tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabBoundaries" runat="server" Title="Boundaries" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                <td><ext:DateField ID="dtpRebukesReportForDate" runat="server" FieldLabel="Print for Date" Format="dd-MM-yyyy" width="230"/></td>
                                <td><ext:Button ID="btnRebukesReport" runat="server" Text="Print Rebukes Report">
                                    <DirectEvents>
                                        <Click OnEvent="PrintRebukesReport"/>
                                    </DirectEvents>
                                </ext:Button>
                                </td>
                                </tr>
                                <tr><td>
                                <ext:Button ID="btnFilterByRebukes" runat="server" Text="Filter for all participants with rebukes">
                                    <DirectEvents>
                                        <Click OnEvent="FilterByRebukes"/>
                                    </DirectEvents>
                                </ext:Button>
                                </td><td>
                                <ext:Button ID="btnResetFilterByRebukes" runat="server" Text="Reset Filter">
                                    <DirectEvents>
                                        <Click OnEvent="ClearFilterByRebukes"/>
                                    </DirectEvents>
                                </ext:Button>
                                </td></tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabMedical" runat="server" Title="Medical" AutoScroll="true">
                              <Items>
                                <ext:DateField ID="dtpMedicalReportForDate" runat="server" FieldLabel="Print for Date" Format="dd-MM-yyyy" width="230"/>
                                <ext:Button ID="btnMedicalReport" runat="server" Text="Print Medical Report">
                                    <DirectEvents>
                                        <Click OnEvent="PrintMedicalReport"/>
                                    </DirectEvents>
                                </ext:Button>
                              </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabBadges" runat="server" Title="Badges" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                  <td><ext:Button ID="btnTestPrintBadges" runat="server" Text="Test Badges">
                                        <DirectEvents>
                                            <Click OnEvent="TestPrintBadges"/>
                                        </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnReprintBadges" runat="server" Text="Reprint Badges">
                                      <DirectEvents>
                                          <Click OnEvent="ReprintBadges"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnPrintBadges" runat="server" Text="Print Badges">
                                      <DirectEvents>
                                          <Click OnEvent="PrintBadges"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnPrintBadgeLabels" runat="server" Text="Print Barcode Labels">
                                      <DirectEvents>
                                          <Click OnEvent="PrintBarcodeLabels"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnPrintBadgeLabelsByKey" runat="server" Text="Badge Labels By Key">
                                      <Listeners>
                                          <Click Handler="#{winPrintBadgeLabelsByKey}.show();" />
                                      </Listeners>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnImportPrintedBadges" runat="server" Text="Import Printed Badges">
                                      <Listeners>
                                          <Click Handler="#{winImportPrintedBadges}.show();" />
                                      </Listeners>
                                  </ext:Button></td>
                              </tr>
                              </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabExport" runat="server" Title="Export" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                  <td><ext:Button ID="btnDownloadExcel" runat="server" Text="Export to Excel/Calc" Icon="PageAttach">
                                      <DirectEvents>
                                          <Click OnEvent="DownloadExcel"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnExportTShirtNumbers" runat="server" Text="Get T-Shirt Numbers">
                                      <DirectEvents>
                                          <Click OnEvent="ExportTShirtNumbers" IsUpload="true"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnExcelArrivalRegistration" runat="server" Text="Excel List for Arrival Registration">
                                      <DirectEvents>
                                          <Click OnEvent="ExportArrivalRegistrationList" IsUpload="true"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnPrintArrivalRegistration" runat="server" Text="Print List for Arrival Registration">
                                      <DirectEvents>
                                          <Click OnEvent="PrintArrivalRegistration"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                  <td><ext:Button ID="btnExcelRolesPerCountry" runat="server" Text="Excel List for Roles per Country">
                                      <DirectEvents>
                                          <Click OnEvent="ExportRolesPerCountry" IsUpload="true"/>
                                      </DirectEvents>
                                  </ext:Button></td>
                                </tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabTopGroups" runat="server" Title="Groups" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                <td><ext:Button ID="btnImportFellowshipGroups" runat="server" Text="Import Fellowship Groups">
                                    <Listeners>
                                        <Click Handler="#{winImportFellowshipGroups}.show();" />
                                    </Listeners>
                                </ext:Button></td>
                                <td>
                                <ext:TextField ID="MaxGroupMembers" runat="server" FieldLabel="Max. Group Members" DataIndex="MaxGroupMembers"/>
                                <ext:Button ID="btnCalculateFellowshipGroups" runat="server" Text="Calculate Fellowship Groups">
                                    <DirectEvents>
                                        <Click OnEvent="CalculateFellowshipGroups" />
                                    </DirectEvents>
                                </ext:Button>
                                </td>
                                </tr>
                                </table>
                              </Content>
                            </ext:Panel>
                            <ext:Panel ID="TabTODO" runat="server" Title="TODO" AutoScroll="true">
                              <Content>
                                <table>
                                <tr>
                                <td><ext:Button ID="btnBatchAccept" runat="server" Text="Accept Many Applicants">
                                    <Listeners>
                                        <Click Handler="#{AcceptForm}.getForm().reset();#{winAcceptMany}.show();" />
                                    </Listeners>
                                </ext:Button></td>
                                <td><ext:Button ID="btnJSONApplication" runat="server" Text="Direct JSON" Icon="Disk">
                                    <Listeners>
                                        <Click Handler="#{JSONApplicationForm}.getForm().reset();#{winJSONApplication}.show();" />
                                    </Listeners>
                                </ext:Button></td>
                                <td><ext:Button ID="btnLoadRefreshApplicants" runat="server" Text="Refresh Applicants">
                                    <DirectEvents>
                                        <Click OnEvent="LoadRefreshApplicants">
                                            <EventMask ShowMask="true" Msg="Refreshing Applicants..." MinDelay="1000" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button></td>
                                <td><ext:Button ID="btnFixArrivalDepartureDates" runat="server" Text="Fix Arrival Departure Dates">
                                    <DirectEvents>
                                        <Click OnEvent="FixArrivalDepartureDates" />
                                    </DirectEvents>
                                </ext:Button></td>
                                </tr></table>
                                </Content>
                            </ext:Panel>
                           </Items>
                        </ext:TabPanel>
                        </td></tr></table>
                    </Content>
                </ext:Panel>
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
                            <ext:RowNumbererColumn />
                            <ext:Column Header="Registration Key" Width="80" DataIndex="PartnerKey"/>
                            <ext:Column Header="Person Key" Width="80" DataIndex="PersonKey"/>
                            <ext:Column ColumnID="FamilyName" Header="Family Name" Width="90" DataIndex="FamilyName" />
                            <ext:Column Header="First Name" Width="90" DataIndex="FirstName"/>
                            <ext:Column Header="Gender" Width="90" DataIndex="Gender"/>
                            <ext:DateColumn Header="Date of Birth" Width="100" DataIndex="DateOfBirth" Format="dd-MMM-yyyy"/>
                            <ext:DateColumn Header="Date Applied" Width="100" DataIndex="ApplicationDate" Format="dd-MMM-yyyy"/>
                            <ext:Column Header="Application Status" Width="100" DataIndex="ApplicationStatus"/>
                            <ext:Column Header="Role" Width="120" DataIndex="Role"/>
                            <ext:Column Header="FGroup" Width="120" DataIndex="StFgCode"/>
                            <ext:Column Header="FGLeader" Width="120" DataIndex="StFgLeader"/>
                            <ext:DateColumn Header="Badge Printed" Width="100" DataIndex="BadgePrint" Format="dd-MMM-yyyy"/>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" SingleSelect="true">
                            <DirectEvents>
                                <RowSelect OnEvent="RowSelect" Buffer="100">
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{FormPanel1}" />
                                    <ExtraParams>
                                        <ext:Parameter Name="PartnerKey" Value="this.getSelected().data['PartnerKey']" Mode="Raw" />
                                        <ext:Parameter Name="ApplicationKey" Value="this.getSelected().data['ApplicationKey']" Mode="Raw" />
                                    </ExtraParams>
                                </RowSelect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <LoadMask ShowMask="true" />
                    <Plugins>
                        <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                            <Filters>
                                <ext:NumericFilter DataIndex="PartnerKey" />
                                <ext:NumericFilter DataIndex="PersonKey" />
                                <ext:StringFilter DataIndex="FamilyName" />
                                <ext:StringFilter DataIndex="FirstName" />
                                <ext:StringFilter DataIndex="StFgCode" />
                            </Filters>
                        </ext:GridFilters>
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="100" StoreID="Store1" />
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
                    Layout="Border"
                    Icon="User">
                    <Items>
                        <ext:Toolbar ID="ToolBar1" runat="server" Region="North" AutoHeight="true">
                            <Items>
                                <ext:Button ID="btnSave" runat="server" Text="Save" Icon="Disk">
                                    <DirectEvents>
                                        <Click OnEvent="SaveApplication">
                                            <EventMask ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="FormPanel1.getForm().getValues(false)" Mode="Raw" Encode="true" />
                                                <ext:Parameter Name="RebukeValues" Value="Ext.get('GridPanelRebukes') != null?#{GridPanelRebukes}.getRowsValues({selectedOnly : false}):''" Mode="Raw" Encode="true"/>
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnReprintPDF" runat="server" Text="Reprint PDF" Icon="Printer">
                                    <DirectEvents>
                                        <Click OnEvent="ReprintPDF" IsUpload="true"/>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnReprintBadge" runat="server" Text="Reprint Badge" Icon="Printer">
                                    <DirectEvents>
                                        <Click OnEvent="ReprintBadge" IsUpload="true"/>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:Button ID="btnPrintMedicalReport" runat="server" Text="Print Report" Icon="Printer">
                                    <DirectEvents>
                                        <Click OnEvent="PrintMedicalReportForParticipant" IsUpload="true"/>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                        <ext:TabPanel ID="TabPanelApplication" runat="server" Region="Center" EnableTabScroll="true">   
                          <Items>
                            <ext:Panel ID="TabApplicantDetails" runat="server" Title="Applicant Details" AutoScroll="true">
                              <Items>
                                <ext:BorderLayout runat="server">
                                  <Center>
                                    <ext:Container runat="server" LabelAlign="Left" Layout="Form">
                                      <Items>
                                        <ext:TextField ID="PartnerKey" runat="server" FieldLabel="Registration Key" DataIndex="PartnerKey" ReadOnly="true"/>
                                        <ext:TextField ID="PersonKey" runat="server" FieldLabel="Petra Person Key" DataIndex="PersonKey" ReadOnly="true"/>
                                        <ext:TextField ID="FirstName" runat="server" FieldLabel="First Name" DataIndex="FirstName" />
                                        <ext:TextField ID="FamilyName" runat="server" FieldLabel="Family Name" DataIndex="FamilyName" />
                                        <ext:ComboBox 
                                            ID="Gender"
                                            runat="server" 
                                            FieldLabel="Gender" 
                                            DataIndex="Gender"
                                            Editable="false"
                                            TypeAhead="true" 
                                            ForceSelection="true"
                                            EmptyText="Select a gender..."
                                            Mode="Local"
                                            Resizable="true"
                                            SelectOnFocus="true">
                                            <Items>
                                                <ext:ListItem Text="Male" Value="Male" />
                                                <ext:ListItem Text="Female" Value="Female" />
                                                <ext:ListItem Text="Unknown" Value="Unknown" />
                                            </Items>                        
                                        </ext:ComboBox>
                                        <ext:DateField ID="DateOfBirth" runat="server" FieldLabel="Date of Birth" DataIndex="DateOfBirth" Format="dd-MM-yyyy"/>
                                        <ext:DateField ID="GenAppDate" runat="server" FieldLabel="Date of Application" DataIndex="GenAppDate" Format="dd-MM-yyyy" />
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
                                            Width="300"
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
                                        <ext:ComboBox 
                                            ID="TShirtStyle"
                                            runat="server" 
                                            FieldLabel="T-Shirt Style" 
                                            DataIndex="TShirtStyle"
                                            Editable="false"
                                            TypeAhead="true" 
                                            ForceSelection="true"
                                            Mode="Local"
                                            Resizable="true"
                                            SelectOnFocus="true">
                                          <Items>
                                              <ext:ListItem Text="M (Boys Cut)" Value="M (Boys Cut)" />
                                              <ext:ListItem Text="F (Girls Cut)" Value="F (Girls Cut)" />
                                          </Items>   
                                        </ext:ComboBox>
                                        <ext:ComboBox 
                                            ID="TShirtSize"
                                            runat="server" 
                                            FieldLabel="T-Shirt Size" 
                                            DataIndex="TShirtSize"
                                            Editable="false"
                                            TypeAhead="true" 
                                            ForceSelection="true"
                                            Mode="Local"
                                            Resizable="true"
                                            SelectOnFocus="true">
                                          <Items>
                                              <ext:ListItem Text="S (Small)" />
                                              <ext:ListItem Text="M (Medium)" />
                                              <ext:ListItem Text="L (Large)" />
                                              <ext:ListItem Text="XL (Very Large)" />
                                          </Items>   
                                        </ext:ComboBox>
                                        <ext:TextArea
                                            ID="Comment"
                                            runat="Server"
                                            Width="300"
                                            FieldLabel="Comment By Registration Office"
                                            />
                                        <ext:DateField ID="DateOfArrival" runat="server" FieldLabel="Date of Arrival" DataIndex="DateOfArrival" Format="dd-MM-yyyy"/>
                                        <ext:DateField ID="DateOfDeparture" runat="server" FieldLabel="Date of Departure" DataIndex="DateOfDeparture" Format="dd-MM-yyyy"/>
                                      </Items>
                                    </ext:Container>
                                  </Center>
                                  <East>
                                    <ext:Container runat="server" LabelAlign="Top" Layout="Form" ColumnWidth=".3">
                                      <Items>
                                        <ext:Image ID="Image1" runat="server"
                                                Width="100"
                                                Height="133"
                                                ImageUrl="../../img/default_blank.gif"
                                            >
                                            <Listeners>
                                            </Listeners>
                                        </ext:Image>   
                                        <ext:Button ID="btnUpload" runat="server" Text="Upload new Photo" Icon="Disk">
                                            <Listeners>
                                                <Click Handler="#{winUploadPhoto}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                      </Items>
                                    </ext:Container>
                                  </East>
                                </ext:BorderLayout>
                                </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabServiceTeam" runat="server" Title="Service Team" AutoScroll="true">
                              <Items>
                              <ext:Container runat="server" LabelAlign="Left" Layout="Form">
                                <Items>
                                <ext:ComboBox
                                    ID="JobWish1"
                                    runat="server" 
                                    FieldLabel="Job Wish 1"
                                    DataIndex="JobWish1"
                                    Editable="false"
                                    TypeAhead="true"
                                    ForceSelection="true"
                                    Mode="Local"
                                    Resizable="true"
                                    Width="300"
                                    Displayfield="JobTitle"
                                    ValueField="JobTitle"
                                    StoreID="StoreServiceTeamJob"                                    
                                    SelectOnFocus="true">
                                </ext:ComboBox>
                                <ext:ComboBox
                                    ID="JobWish2"
                                    runat="server" 
                                    FieldLabel="Job Wish 2" 
                                    DataIndex="JobWish2"
                                    Editable="false"
                                    TypeAhead="true"
                                    ForceSelection="true"
                                    Mode="Local"
                                    Resizable="true"
                                    Width="300"
                                    DisplayField="JobTitle"
                                    ValueField="JobTitle"
                                    StoreID="StoreServiceTeamJob"                                    
                                    SelectOnFocus="true">
                                </ext:ComboBox>
                                <ext:ComboBox
                                    ID="JobAssigned"
                                    runat="server" 
                                    FieldLabel="Assigned Job" 
                                    DataIndex="JobAssigned"
                                    Editable="false"
                                    TypeAhead="true"
                                    ForceSelection="true"
                                    Mode="Local"
                                    Resizable="true"
                                    Width="300"
                                    DisplayField="JobTitle"
                                    ValueField="JobTitle"
                                    StoreID="StoreServiceTeamJob"                                    
                                    SelectOnFocus="true">
                                </ext:ComboBox>
                                <ext:TextArea ID="CommentRegistrationOfficeReadOnly" runat="server" 
                                    FieldLabel="Comment Registration Office" 
                                    Width="300"
                                    Height="80"
                                    ReadOnly="true"/>
                                </Items>
                              </ext:Container>
                              </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabFinance" runat="server" Title="Finance" AutoScroll="true">
                              <Items>
                              <ext:Container runat="server" LabelAlign="Left" Layout="Form">
                                <Items>
                                <ext:ComboBox 
                                    ID="StFieldCharged"
                                    runat="server" 
                                    FieldLabel="Field Charged" 
                                    Editable="false"
                                    ForceSelection="true"
                                    Mode="Local"
                                    Width="260"
                                    LabelWidth="150"
                                    StoreID="StoreRegistrationOffice"
                                    DisplayField="PartnerShortName"
                                    ValueField="PartnerKey"
                                    SelectOnFocus="true"
                                    SelectedIndex = "0">
                                </ext:ComboBox>
                                <ext:CheckBox
                                    ID="SecondSibling"
                                    runat="Server"
                                    FieldLabel="Reduced price because of sibling"
                                    />
                                <ext:CheckBox
                                    ID="CancelledByFinanceOffice"
                                    runat="Server"
                                    FieldLabel="Cancelled by Finance Office, no payment required"
                                    />
                                </Items>
                              </ext:Container>
                              </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabGroups" runat="server" Title="Group" AutoScroll="true">
                              <Items>
                                    <ext:TextField
                                        ID="GroupWish"
                                        runat="Server"
                                        FieldLabel="Group Wish by participant"
                                        Width="700"
                                        />
                                    <ext:Label runat="server"
                                        HideLabel="true"
                                        Html="Enter the partner keys of the people this person wants to be with in a group. <br/>Add a minus in front to avoid people, eg. -4123123.<br/>Separate the keys with a comma."
                                        Width="500"/>
                                    <ext:TextField
                                        ID="GroupWishKeys"
                                        runat="Server"
                                        FieldLabel="Enforce Groups by key"
                                        Width="700"
                                        />
                                    <ext:TextField
                                        ID="StFgCode"
                                        runat="Server"
                                        FieldLabel="Fellowship Group Code"
                                        />
                                    <ext:CheckBox
                                        ID="StFgLeader"
                                        runat="Server"
                                        LableWidth="200"
                                        FieldLabel="This person is the Fellowship Group Leader"
                                        />
                                    <ext:TextArea
                                        ID="GroupMembers"
                                        runat="Server"
                                        ReadOnly="true"
                                        FieldLabel="Members of this group"
                                        Width="400"
                                        Height="120"
                                        Disabled="true"
                                        />
                                      <ext:Button ID="btnRefreshGroupMembers" runat="server" Text="Refresh">
                                          <DirectEvents>
                                              <Click OnEvent="RefreshGroupMembers"/>
                                          </DirectEvents>
                                      </ext:Button>
                              </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabRawApplicationData" runat="server" Title="Data Entered" AutoScroll="true">
                              <DirectEvents>
                                  <Show OnEvent="ShowRawApplicationData"/>
                              </DirectEvents>
                            </ext:Panel>
                            <ext:Panel ID="TabRebukes" runat="server" Title="Rebukes" AutoScroll="true">
                                <Items>
                              <ext:Button ID="btnNewRebuke" runat="server" Text="New Rebuke">
                                  <DirectEvents>
                                      <Click OnEvent="AddNewRebuke"/>
                                  </DirectEvents>
                              </ext:Button>
                              <ext:GridPanel 
                                  ID="GridPanelRebukes"
                                  runat="server" 
                                  Title="Rebukes" 
                                  StripeRows="true"
                                  TrackMouseOver="true"
                                  Width="800" 
                                  Height="350"
                                  StoreID="StoreRebukes"
                                  AutoExpandColumn="What">
                                  <ColumnModel runat="server">
                                      <Columns>
                                          <ext:DateColumn Header="When" DataIndex="When" Format="dd-MMM-yyyy">
                                              <Editor>
                                                  <ext:DateField runat="server" />
                                              </Editor>
                                          </ext:DateColumn>
                                          <ext:Column ColumnID="Time" Header="Time" DataIndex="Time" Width="100">
                                              <Editor>
                                                  <ext:TextField runat="server" />
                                              </Editor>
                                          </ext:Column>
                                          <ext:Column ColumnID="What" Header="What" DataIndex="What" Width="300">
                                              <Editor>
                                                  <ext:TextField runat="server" />
                                              </Editor>
                                          </ext:Column>
                                          <ext:Column ColumnID="Consequence" Header="Consequence" DataIndex="Consequence" Width="300">
                                              <Editor>
                                                  <ext:TextField runat="server" />
                                              </Editor>
                                          </ext:Column>
                                      </Columns>
                                  </ColumnModel>
                                  <SelectionModel>
                                      <ext:RowSelectionModel runat="server" SingleSelect="true" />
                                  </SelectionModel>
                              </ext:GridPanel>          
                              </Items>
                            </ext:Panel>
                            <ext:Panel ID="TabMedicalInfo" runat="server" Title="Medical Info" AutoScroll="true"/>
                            <ext:Panel ID="TabMedicalLog" runat="server" Title="Medical Log" AutoScroll="true">
                              <Items>
                                <ext:Button ID="btnAddMedicalIncident" runat="server" Text="Add incident" Icon="PageAttach">
                                  <DirectEvents>
                                      <Click OnEvent="AddMedicalIncident"/>
                                  </DirectEvents>
                                </ext:Button>
                                <ext:TabPanel ID="MedicalPanel" runat="server" EnableTabScroll="true" Height="400" Width="800">
                                  <Items>
                                  </Items>
                                </ext:TabPanel>
                              </Items>
                            </ext:Panel>
                          </Items>
                        </ext:TabPanel>
                     </Items>
                </ext:FormPanel>
            </Items>
        </ext:DesktopWindow>
        
    </form>
</body>
</html>
