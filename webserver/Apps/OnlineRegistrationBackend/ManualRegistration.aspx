<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.TManualRegistrationUI"
    src="ManualRegistration.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Manual Registration</title>
<script type="text/javascript">
        function HideDialog() {
            parent.winManualRegistration.hide();
            ManualRegistrationForm.getForm().reset();
        };
</script>        
    
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false"/>
		<ext:FormPanel 
            ID="ManualRegistrationForm" 
            runat="server"
            Region="Center"
            Width="400"
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
                <ext:TextField ID="FirstName" runat="server" FieldLabel="First Name" DataIndex="FirstName" />
                <ext:TextField ID="LastName" runat="server" FieldLabel="Family Name" DataIndex="LastName" />
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
                    </Items>                        
                </ext:ComboBox>
                <ext:TextField ID="Role" runat="server" FieldLabel="TS-Role" DataIndex="Role" />
                <ext:TextField ID="Email" runat="server" FieldLabel="Email" DataIndex="Email" />
                <ext:TextField ID="Phone" runat="server" FieldLabel="Phone" DataIndex="Phone" />
                <ext:TextField ID="Street" runat="server" FieldLabel="Street" DataIndex="Street" />
                <ext:TextField ID="PostCode" runat="server" FieldLabel="PostCode" DataIndex="PostCode" />
                <ext:TextField ID="City" runat="server" FieldLabel="City" DataIndex="City" />
                <ext:TextField ID="Country" runat="server" FieldLabel="Country Code" DataIndex="Country" />
                <ext:DateField ID="DateOfBirth" runat="server" FieldLabel="Date of Birth" DataIndex="DateOfBirth" Format="dd-MMM-yyyy"/>
                <ext:DateField ID="DateOfArrival" runat="server" FieldLabel="Date of Arrival" DataIndex="DateOfArrival" Format="dd-MMM-yyyy" />
                <ext:DateField ID="DateOfDeparture" runat="server" FieldLabel="Date of Departure" DataIndex="DateOfDeparture" Format="dd-MMM-yyyy" />
                <ext:CheckBox ID="Vegetarian" runat="server" FieldLabel="Vegetarian" DataIndex="Vegetarian" />
                <ext:TextField ID="MedicalNeeds" runat="server" FieldLabel="MedicalNeeds" DataIndex="MedicalNeeds" />
                <ext:TextField ID="RegistrationOffice" runat="server" FieldLabel="Registration Office Number" DataIndex="RegistrationOffice" />
                <ext:TextField ID="PaymentInfo" runat="server" FieldLabel="PaymentInfo" DataIndex="PaymentInfo" />
            </Items>
            <Buttons>
                <ext:Button ID="btnSubmit" runat="server" Text="Submit Manual Registration">
                    <DirectEvents>
                        <Click OnEvent="SubmitManualRegistration">
                            <EventMask ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="#{ManualRegistrationForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
                            </ExtraParams>
                        </Click>
                    </DirectEvents>
                </ext:Button>
                <ext:Button runat="server" Text="Cancel">
                    <Listeners>
                        <Click Handler="HideDialog();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>

    </form>
</body>
</html>
