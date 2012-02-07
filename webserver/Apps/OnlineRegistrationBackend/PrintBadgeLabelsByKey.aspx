<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.TPrintBadgeLabelsByKeyUI"
    src="PrintBadgeLabelsByKey.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Print badge labels by key</title>
<script type="text/javascript">
        function HideDialog() {
            parent.winPrintBadgeLabelsByKey.hide();
            BadgeForm.getForm().reset();
        };
        
        function Download(id, outputname) {
        	document.getElementById('DownloadPDFFrame').contentWindow.location="downloadOnce.aspx?id=" + id + "&filename="+outputname+"&directory=badges";
        };
</script>        
    
</head>
<body>
	<iframe id="DownloadPDFFrame" style="display:none"></iframe>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false"/>
		<ext:FormPanel 
            ID="BadgeForm" 
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
                <ext:TextArea ID="RegistrationsKeys" DataIndex="RegistrationsKeys" runat="server" FieldLabel="Paste here from Excel" Height="360" Width="500"/>
                <ext:Label runat="server" Text="Please paste from Excel. Column1 is partnerkey or personkey "/>
            </Items>
            <Buttons>
                <ext:Button ID="ImportButton" runat="server" Text="Print Badge Labels">
                    <DirectEvents>
                        <Click OnEvent="PrintBadgeLabelsByKey">
                            <EventMask ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="#{BadgeForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
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
