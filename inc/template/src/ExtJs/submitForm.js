var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}
{#INCLUDE upload.js}

{#FORMTYPE} = Ext.extend(Ext.FormPanel, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: {#FORMFRAME},
            header: {#FORMHEADER},
            // monitorValid:true,
            fileUpload: false,
            title: this.{#FORMCAPTION},
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
            items: [{#FORMITEMSDEFINITION}],
            buttons: [{#BUTTONS}]
        });
        {#FORMTYPE}.superclass.initComponent.apply(this, arguments);
    }
});

{#UPLOADFORM}

{##SUBMITBUTTONDEFINITION}
{
    text: this.{#LABEL}
{#IFDEF REQUESTURL}
    ,formBind: true,
    handler: function () {
        // to display missing/invalid fields
        if (!{#FORMNAME}.getForm().isValid())
        {
            Ext.Msg.show({
                title: {#FORMNAME}.{#VALIDATIONERRORTITLE},
                msg: {#FORMNAME}.{#VALIDATIONERRORMESSAGE},
                modal: true,
                icon: Ext.Msg.ERROR,
                buttons: Ext.Msg.OK
            });
        }
        {#CHECKFORVALIDUPLOAD}
        else
        {
            Ext.MessageBox.wait({#FORMNAME}.{#SENDINGDATAMESSAGE}, {#FORMNAME}.{#SENDINGDATATITLE});

            Ext.Ajax.request({
                url: '/server.asmx/{#REQUESTURL}',
                params:{
                    {#REQUESTPARAMETERS}
                    AJSONFormData: Ext.encode({#FORMNAME}.getForm().getValues())
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
{#IFDEF REQUESTSUCCESSMESSAGE}
                        Ext.Msg.show({
                            title: {#FORMNAME}.{#REQUESTSUCCESSTITLE},
                            msg: {#FORMNAME}.{#REQUESTSUCCESSMESSAGE},
                            modal: true,
                            icon: Ext.Msg.INFO,
                            buttons: Ext.Msg.OK,
                            fn: function(btn, text){
                                {#REDIRECTONSUCCESS}
                            }
                        });
{#ENDIF REQUESTSUCCESSMESSAGE}
{#IFNDEF REQUESTSUCCESSMESSAGE}
                        {#REDIRECTONSUCCESS}
{#ENDIFN REQUESTSUCCESSMESSAGE}
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
        }
    }    
{#ENDIF REQUESTURL}
}

{##REDIRECTONSUCCESS}
{#IFNDEF REDIRECTDOWNLOAD}
{#IFDEF REDIRECTURLONSUCCESS}
                      location.href={#FORMNAME}.{#REDIRECTURLONSUCCESS};
{#ENDIF REDIRECTURLONSUCCESS}
{#ENDIFN REDIRECTDOWNLOAD}
{#IFDEF REDIRECTDOWNLOAD}
                      location.href={#FORMNAME}.{#REDIRECTURLONSUCCESS} + '?download=' + {#REDIRECTDOWNLOAD};
{#ENDIF REDIRECTDOWNLOAD}
