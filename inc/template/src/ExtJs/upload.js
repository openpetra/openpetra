{##UPLOADFORMDEFINITION}
var UploadForm = null;

TUploadForm = Ext.extend(Ext.FormPanel, {
    initComponent : function(config) {
        Ext.apply(this, {
            fileUpload: true,
{#IFNDEF UPLOADBUTTONLABEL}
            width: 24,
{#ENDIFN UPLOADBUTTONLABEL}
{#IFDEF UPLOADBUTTONLABEL}
            width: 200,
{#ENDIF UPLOADBUTTONLABEL}
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
                xtype: 'filefield',
                id: 'form-file',
                emptyText: 'Select an image',
                hideLabel: true,
                buttonOnly: true,
                name: 'photo-path',
                width: 20,
                fieldStyle : 'visibility:hidden;width:0px;height:0px;margin:0px',
                // allowBlank is true to avoid invalid flag on Internet Explorer
                allowBlank: true,
{#IFDEF UPLOADBUTTONLABEL}
                buttonText: {#FORMNAME}.{#UPLOADBUTTONLABEL},
{#ENDIF UPLOADBUTTONLABEL}
{#IFNDEF UPLOADBUTTONLABEL}                
                buttonText: '',
                buttonConfig: {
                    iconCls: 'upload-icon'
                },
{#ENDIFN UPLOADBUTTONLABEL}
                listeners: {
                            'change': function(field, value, eOpts){
                                if(field.value.length > 0) {
                                    UploadForm.getForm().submit({
                                        url: 'upload.aspx',
                                        waitMsg: 'Uploading your photo...',
                                        success: function(fp, o){
                                            var imgPhoto = Ext.get('photoPreview');
                                            imgPhoto.dom.src = 'upload.aspx?image-id=' + o.result.file;
                                            var imgID = Ext.getCmp('hidImageID');
                                            imgID.setValue(o.result.file);
                                        },
                                        failure: function(fp, o){
                                                Ext.Msg.show({
                                                    title: 'Problems with upload',
                                                    msg: o.result.msg,
                                                    modal: true,
                                                    icon: Ext.Msg.ERROR,
                                                    buttons: Ext.Msg.OK
                                                });
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
else if (Ext.getCmp('hidImageID').getValue().length == 0)
{
  Ext.Msg.show({
      title: 'Please upload photo',
      msg: 'Please upload photo',
      modal: true,
      icon: Ext.Msg.ERROR,
      buttons: Ext.Msg.OK
  });
}

{##ASSISTANTPAGEWITHUPLOADSHOW}
var myDivDestination = Ext.get('uploadDiv');
var myDivToMove = Ext.get('tmpUploadDiv');
myDivToMove.setStyle('visibility', 'visible');
myDivDestination.insertFirst(myDivToMove);

{##ASSISTANTPAGEWITHUPLOADVALID}
// does not seem to work:
// var valid = this.callParent(arguments);

// therefore copying the content of isValid (http://docs.sencha.com/ext-js/4-0/source/Basic.html#Ext-form-Basic-method-isValid)
var me = this, invalid;                    
me.batchLayouts(function() {
        invalid = me.getFields().filterBy(function(field) {
            return !field.validate();
        });
    });
var valid = (invalid.length == 0);

if (Ext.getCmp('hidImageID').getValue().length == 0)
{
    Ext.Msg.show({
        title: {#FORMNAME}.{#MISSINGUPLOADTITLE},
        msg: {#FORMNAME}.{#MISSINGUPLOADMESSAGE},
        modal: true,
        icon: Ext.Msg.ERROR,
        buttons: Ext.Msg.OK
    });
    return false;
}

return valid;

{##ASSISTANTPAGEWITHUPLOADHIDE}
var myDivToMove = Ext.get('uploadDiv');
myDivToMove.setStyle('visibility', 'hidden');
