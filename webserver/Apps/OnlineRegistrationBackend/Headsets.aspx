<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.THeadsetManagementUI"
    src="Headsets.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Manage the headsets</title>
<script type="text/javascript">
        function HideDialog() {
            parent.winHeadsets.hide();
            HeadsetsForm.getForm().reset();
        };
</script>        
    
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false"/>
		
        <ext:Store ID="SessionsStore" runat="server" OnRefreshData="Sessions_Refresh">
            <Reader>
                <ext:ArrayReader>
                    <Fields>
                        <ext:RecordField Name="SessionName" />
                    </Fields>
                </ext:ArrayReader>
            </Reader>
        </ext:Store>
		
		<ext:FormPanel 
            ID="HeadsetsForm" 
            runat="server"
            Region="Center"
            Width="800"
            Frame="false"
            AutoHeight="true"
            MonitorValid="true"
            PaddingSummary="10px 10px 0 10px"
            LabelWidth="100">                
            <Defaults>
                <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                <ext:Parameter Name="allowBlank" Value="false" Mode="Raw" />
                <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
            </Defaults>
            <Items>
			  <ext:TabPanel ID="TabHeadsets" runat="server" Height="800" Width="600">   
				<Items>
					<ext:Panel ID="TabEditMessageToHomeOfficeReps" runat="server" Title="Edit Message" AutoScroll="true">
					  <Content>
						  <ext:TextArea ID="txtMessageToHomeOfficeReps" 
							DataIndex="txtMessageToHomeOfficeReps" 
							runat="server" 
							Height="360" 
							Width="500"/>
  						  <ext:Button ID="SaveMessage" runat="server" Text="Save">
							<DirectEvents>
								<Click OnEvent="SaveMessageToReps">
									<EventMask ShowMask="true" />
									<ExtraParams>
										<ext:Parameter Name="Values" Value="#{HeadsetsForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
				      </Content>
					</ext:Panel>
					<ext:Panel ID="TabCreateSession" runat="server" Title="Add Session" AutoScroll="true">
					  <Content>
						<ext:TextField ID="txtNewSession" runat="server" FieldLabel="Add new session" EmptyText="eg. Friday July 6 Morning session" Width="300"/>
						<ext:Button ID="AddSession" runat="server" Text="Add Session">
							<DirectEvents>
								<Click OnEvent="AddNewSession">
									<EventMask ShowMask="true" />
									<ExtraParams>
										<ext:Parameter Name="Values" Value="#{HeadsetsForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						</ext:Button>
				      </Content>
					</ext:Panel>
					<ext:Panel ID="TabRentedOutHeadsets" runat="server" Title="Headsets rented out" AutoScroll="true">
					  <Content>
					     <table><tr><td>
						  <ext:ComboBox 
							ID="CurrentSessionRentedOut"
							runat="server" 
							FieldLabel="Select session for which the headsets are rented out" 
							Editable="false"
							ForceSelection="true"
							EmptyText="current session"
							Mode="Local"
							Width="350"
							LabelWidth="150"
							StoreID="SessionsStore"
							DisplayField="SessionName"
							ValueField="SessionName"
							SelectOnFocus="true"
							SelectedIndex = "0">
  						  </ext:ComboBox>
						  </td></tr><tr><td>
						  <ext:Label runat="server" Text="Please paste from Excel or Text file. One partner key per row. "/>
						  </td></tr><tr><td>
						  <ext:TextArea ID="PartnerKeysRentedOut" 
							DataIndex="PartnerKeysRentedOut" 
							runat="server" 
							Height="360" 
							Width="500"/>
						  </td></tr><tr><td>
						  <ext:Button ID="ImportButtonRentedOut" runat="server" Text="Headsets rented out">
							<DirectEvents>
								<Click OnEvent="ImportHeadsetKeysRentedOut">
									<EventMask ShowMask="true" />
									<ExtraParams>
										<ext:Parameter Name="Values" Value="#{HeadsetsForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						  </ext:Button>
						  </td></tr></table>
				      </Content>
					</ext:Panel>
					<ext:Panel ID="TabReturnedHeadsets" runat="server" Title="Returned Headsets" AutoScroll="true">
					  <Content>
					     <table><tr><td>
						  <ext:ComboBox 
							ID="CurrentSessionReturned"
							runat="server" 
							FieldLabel="Select session for which the headsets are returned" 
							Editable="false"
							ForceSelection="true"
							EmptyText="current session"
							Mode="Local"
							Width="350"
							LabelWidth="150"
							StoreID="SessionsStore"
							DisplayField="SessionName"
							ValueField="SessionName"
							SelectOnFocus="true"
							SelectedIndex = "0">
  						  </ext:ComboBox>
						  </td></tr><tr><td>
						  <ext:Label runat="server" Text="Please paste from Excel or Text file. One partner key per row. "/>
						  </td></tr><tr><td>
						  <ext:TextArea ID="PartnerKeysReturned" 
							DataIndex="PartnerKeysReturned" 
							runat="server" 
							Height="360" 
							Width="500"/>
						  </td></tr><tr><td>
						  <ext:Button ID="ImportButtonReturned" runat="server" Text="Returned Headsets">
							<DirectEvents>
								<Click OnEvent="ImportHeadsetKeysReturned">
									<EventMask ShowMask="true" />
									<ExtraParams>
										<ext:Parameter Name="Values" Value="#{HeadsetsForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
									</ExtraParams>
								</Click>
							</DirectEvents>
						  </ext:Button>
						  </td></tr></table>
				      </Content>
					</ext:Panel>
					<ext:Panel ID="TabReporting" runat="server" Title="Reporting" AutoScroll="true">
					  <Content>
					     <table>
						  <tr><td>
						  <ext:TextArea ID="txtReadMessageToHomeOfficeReps" 
							runat="server" 
							ReadOnly=true
							Height="360" 
							Width="500"/>
						  </td></tr>
						  <tr><td>
						  <ext:ComboBox 
							ID="ReportSelectedSession"
							runat="server" 
							FieldLabel="Select session for which the reports should be run" 
							Editable="false"
							ForceSelection="true"
							EmptyText="select session"
							Mode="Local"
							Width="350"
							LabelWidth="150"
							StoreID="SessionsStore"
							DisplayField="SessionName"
							ValueField="SessionName"
							SelectOnFocus="true"
							SelectedIndex = "0">
  						  </ext:ComboBox>
						  </td></tr>
						  <tr><td>
						  <ext:Button ID="btnReportUnreturnedHeadsets" runat="server" Text="Show headsets per session (unreturned and statistics per country)">
							<DirectEvents>
								<Click OnEvent="ReportHeadsetsPerSession" IsUpload="true"/>
							</DirectEvents>
						  </ext:Button>
						  </td></tr>
						  <tr><td>
						  <ext:Button ID="btnStatistics" runat="server" Text="Show Statistics">
							<DirectEvents>
								<Click OnEvent="ReportStatistics" IsUpload="true"/>
							</DirectEvents>
						  </ext:Button>
						  </td></tr>
						  </table>
				      </Content>
					</ext:Panel>
				</Items>
		    </ext:TabPanel>
            </Items>
        </ext:FormPanel>

    </form>
</body>
</html>
