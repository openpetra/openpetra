<%@ Page Language="C#"
    Inherits="Ict.Petra.WebServer.MConference.TCreateGiftBatch"
    src="CreateGiftBatch.aspx.cs" %>

<%@ Register assembly="Ext.Net" namespace="Ext.Net" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>Create GiftBatch for payment</title>
<script type="text/javascript">
        function HideDialog() {
            parent.winCreateGiftBatch.hide();
            CreateGiftBatchForm.getForm().reset();
        };

        function DownloadGiftTransactions() {
            
        };
</script>        
    
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" CleanResourceUrl="false" />

        <ext:FormPanel 
            ID="CreateGiftBatchForm" 
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
                <ext:TextArea ID="Payments" DataIndex="Payments" runat="server" FieldLabel="Payments" Height="360"/>
                <ext:Label runat="server" Text="TODO Help"/>
                <ext:TextArea ID="GiftTransactions" runat="server" FieldLabel="Gift Transactions" Height="160"/>
            </Items>
            <Buttons>
                <ext:Button ID="SubmitButton" runat="server" Text="Create Gift Batch">
                    <DirectEvents>
                        <Click OnEvent="CreateGiftBatch">
                            <EventMask ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="#{CreateGiftBatchForm}.getForm().getValues(false)" Mode="Raw" Encode="true" />
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
