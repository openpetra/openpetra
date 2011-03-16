<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.TPageOnlineApplication"
    src="Desktop.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                        <ext:RecordField Name="FirstName" />
                        <ext:RecordField Name="FamilyName" />
                        <ext:RecordField Name="Gender"/>
                        <ext:RecordField Name="DateOfBirth" Type="Date"/>
                        <ext:RecordField Name="ApplicationDate" Type="Date"/>
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
            ID="winUpload" 
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
                                            Success="#{winUpload}.hide(); #{UploadForm}.getForm().reset();"
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
                                        <Click Handler="#{winUpload}.hide();#{UploadForm}.getForm().reset();" />
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
                <ext:FormPanel 
                    ID="FormPanelTop" 
                    runat="server" 
                    Region="North"
                    Split="true"
                    Margins="0 5 5 5"
                    Title="" 
                    Height="120">
                    <Items>
                        <ext:ComboBox 
                            ID="FilterStatus"
                            runat="server" 
                            FieldLabel="Filter by Application Status" 
                            Editable="false"
                            ForceSelection="true"
                            EmptyText="Filter by..."
                            Mode="Local"
                            SelectOnFocus="true"
                            SelectedIndex = "2">
                            <Items>
                                <ext:ListItem Text="All" Value="all" />
                                <ext:ListItem Text="Accepted" Value="accepted" />
                                <ext:ListItem Text="On Hold" Value="on hold" />
                                <ext:ListItem Text="Cancelled" Value="cancelled" />
                            </Items>                        
                            <DirectEvents>
                                <Select OnEvent="ChangeFilter"/>
                            </DirectEvents>
                        </ext:ComboBox>
                        <ext:Button ID="btnDownloadPetra" runat="server" Text="Download for Petra" Icon="PageAttach">
                            <Listeners>
                                <Click Handler="submitValue(#{GridPanel1});" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnBatchAccept" runat="server" Text="Accept Many Applicants">
                            <Listeners>
                                <Click Handler="#{AcceptForm}.getForm().reset();#{winAcceptMany}.show();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:FormPanel>
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
                            <ext:Column Header="Person Key" Width="80" DataIndex="PartnerKey"/>
                            <ext:Column ColumnID="FamilyName" Header="Family Name" Width="90" DataIndex="FamilyName" />
                            <ext:Column Header="First Name" Width="90" DataIndex="FirstName"/>
                            <ext:Column Header="Gender" Width="90" DataIndex="Gender"/>
                            <ext:DateColumn Header="Date of Birth" Width="100" DataIndex="DateOfBirth" Format="dd-MMM-yyyy"/>
                            <ext:DateColumn Header="Date Applied" Width="100" DataIndex="ApplicationDate" Format="dd-MMM-yyyy"/>
                            <ext:Column Header="Application Status" Width="100" DataIndex="ApplicationStatus"/>
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
                        <ext:Container runat="server" Layout="Column" Height="400">
                        <Items>
                        <ext:Container runat="server" LabelAlign="Left" Layout="Form" ColumnWidth=".7">
                            <Items>
                                <ext:TextField ID="PartnerKey" runat="server" FieldLabel="Partner Key" DataIndex="PartnerKey" ReadOnly="true"/>
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
                        <ext:DateField ID="DateOfBirth" runat="server" FieldLabel="Date of Birth" DataIndex="DateOfBirth" Format="dd-MMM-yyyy"/>
                        <ext:DateField ID="GenAppDate" runat="server" FieldLabel="Date of Application" DataIndex="GenAppDate" Format="dd-MMM-yyyy" />
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
                            </Items>
                        </ext:Container>
                        <ext:Container runat="server" LabelAlign="Top" Layout="Form" ColumnWidth=".3">
                            <Items>
                                <ext:Image ID="Image1" runat="server"
                                        Width="100"
                                        ImageUrl="../../img/default_blank.gif"
                                    >
                                    <Listeners>
                                    </Listeners>
                                </ext:Image>   
                                <ext:Button ID="btnUpload" runat="server" Text="Upload new Photo" Icon="Disk">
                                    <Listeners>
                                        <Click Handler="#{winUpload}.show();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Container>
                        
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:DesktopWindow>
        
    </form>
</body>
</html>
