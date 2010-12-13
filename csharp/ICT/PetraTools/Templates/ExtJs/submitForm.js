var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}

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
                        Ext.Msg.show({
                            title: {#FORMNAME}.{#REQUESTSUCCESSTITLE},
                            msg: {#FORMNAME}.{#REQUESTSUCCESSMESSAGE},
                            modal: true,
                            icon: Ext.Msg.INFO,
                            buttons: Ext.Msg.OK
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
        }
    }    
{#ENDIF REQUESTURL}
}

{##UPLOADFORMDEFINITION}
TUploadForm = Ext.extend(Ext.FormPanel, {
    initComponent : function(config) {
        Ext.apply(this, {
            fileUpload: true,
            width: 24,
            height: 22,
            frame: false,
            header: false,
            border: false,
            bodyStyle: 'padding: 0px 0px 0 0px;',
            labelWidth: 0,
            defaults: {
                allowBlank: false,
                msgTarget: 'side'
            },
            items: [{
                xtype: 'fileuploadfield',
                id: 'form-file',
                emptyText: 'Select an image',
                hideLabel: true,
                buttonOnly: true,
                fieldLabel: 'TODO',
                name: 'photo-path',
                buttonText: 'Click here to upload photo',
                buttonCfg: {
                    iconCls: 'upload-icon'
                },
                listeners: {
                            'fileselected': function(fb, v){
                                if(UploadForm.getForm().isValid()){
                                    UploadForm.getForm().submit({
                                        url: 'upload.aspx',
                                        waitMsg: 'Uploading your photo...',
                                        success: function(fp, o){
                                            var imgPhoto = Ext.get('photoPreview');
                                            imgPhoto.dom.src = 'upload.aspx?image-id=' + o.result.file;
                                            var imgID = {#FORMNAME}.getForm().findField('hidImageID');
                                            imgID.setValue(o.result.file);
                                        }
                                    });
                                }
                            }
                        }
                
            }]
        });
        TUploadForm.superclass.initComponent.apply(this, arguments);
    }
});

{##VALIDUPLOADCHECK}
else if ({#FORMNAME}.getForm().findField('hidImageID').getValue().length == 0)
{
  Ext.Msg.show({
      title: 'Please upload photo',
      msg: 'Please upload photo',
      modal: true,
      icon: Ext.Msg.ERROR,
      buttons: Ext.Msg.OK
  });
}
