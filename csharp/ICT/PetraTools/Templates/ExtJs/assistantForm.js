var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}

{#FORMTYPE} = Ext.extend(Ext.ux.Wiz, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: {#FORMFRAME},
            header: {#FORMHEADER},
            closable: false,
            // monitorValid:true,
            fileUpload: {#CONTAINSFILEUPLOAD},
            title: ' ',
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
                    
            headerConfig : {
                title : this.{#FORMCAPTION}
            },
            
            cardPanelConfig : {
                defaults : {
                    baseCls    : 'x-small-editor',
                    bodyStyle  : 'padding:40px 15px 5px 120px;background-color:#F6F6F6;',
                    border     : false    
                }
            },

            {#BUTTONS}

            cards : [{#FORMITEMSDEFINITION}]
        });
        {#FORMTYPE}.superclass.initComponent.apply(this, arguments);
    }
});

{##ASSISTANTPAGEDEFINITION}
new Ext.ux.Wiz.Card({
    title : this.{#LABEL},
    monitorValid : true,
    defaults     : {
        labelStyle : 'font-size:11px'
    },
    items : [{#ITEMS}]
})

{##SUBMITBUTTONDEFINITION}
onFinish: function() {
    Ext.MessageBox.wait(this.{#SENDINGDATAMESSAGE}, this.{#SENDINGDATATITLE});

    Ext.Ajax.request({
        url: '/server.asmx/{#REQUESTURL}',
        params:{
            {#REQUESTPARAMETERS}
            AJSONFormData: Ext.encode(this.getWizardData())
        },
        success: function (response) {
            jsonData = XmlExtractJSONResponse(response);
            if (jsonData.failure == true)
            {
                  Ext.Msg.show({
                      title: {#FORMNAME}.{#REQUESTFAILURETITLE},
                      msg: {#FORMNAME}.{#REQUESTFAILUREMESSAGE} + "<br/>" + jsonData.data.result,
                      modal: true,
                      icon: Ext.Msg.ERROR,
                      buttons: Ext.Msg.OK
                  });
            }
            else
            {
                Ext.Msg.show({
                    title: MainForm.btnConfirmREQUESTSUCCESSTITLE,
                    msg: MainForm.btnConfirmREQUESTSUCCESSMESSAGE,
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK,
                    fn: function(btn, text){
                      location.href='{#REDIRECTURLONSUCCESS}';
                    }
                });
            }
        },
        failure: function () {
            Ext.Msg.show({
                title: {#FORMNAME}.{#REQUESTFAILURETITLE},
                msg: {#FORMNAME}.{#REQUESTFAILUREMESSAGE},
                modal: true,
                icon: Ext.Msg.ERROR,
                buttons: Ext.Msg.OK
            });
        }
    });
},
