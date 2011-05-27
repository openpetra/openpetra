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
                xtype: 'fileuploadfield',
                id: 'form-file',
                emptyText: 'Select an image',
                hideLabel: true,
                buttonOnly: true,
                fieldLabel: 'TODO',
                name: 'photo-path',
{#IFDEF UPLOADBUTTONLABEL}
                buttonText: MainForm.{#UPLOADBUTTONLABEL},
{#ENDIF UPLOADBUTTONLABEL}
{#IFNDEF UPLOADBUTTONLABEL}                
                buttonCfg: {
                    iconCls: 'upload-icon'
                },
{#ENDIFN UPLOADBUTTONLABEL}
                listeners: {
                            'fileselected': function(fb, v){
                                if(UploadForm.getForm().isValid()){
                                    UploadForm.getForm().submit({
                                        url: 'upload.aspx',
                                        waitMsg: 'Uploading your photo...',
                                        success: function(fp, o){
                                            var imgPhoto = Ext.get('photoPreview');
                                            imgPhoto.dom.src = 'upload.aspx?image-id=' + o.result.file;
                                            var imgID = Ext.get('hidImageID');
                                            imgID.set({value: o.result.file});
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

{##ASSISTANTPAGEWITHUPLOADSHOW}
var myDivDestination = Ext.get('photoPreview');
var myDivToMove = Ext.get('uploadDiv');
myDivToMove.setStyle('z-index', '20000');
myDivToMove.setStyle('visibility', 'visible');
myDivToMove.moveTo(myDivDestination.getX() + myDivDestination.getWidth() + 10, myDivDestination.getY());

{##ASSISTANTPAGEWITHUPLOADVALID}
// use the little trick of setting the z-index so that the message box does not get displayed when the form is shown the first time
if (Ext.get('hidImageID').getValue().length == 0 && Ext.get('hidImageID').getStyle('z-index') != 'auto')
{
    Ext.Msg.show({
        title: MainForm.{#MISSINGUPLOADTITLE},
        msg: MainForm.{#MISSINGUPLOADMESSAGE},
        modal: true,
        icon: Ext.Msg.ERROR,
        buttons: Ext.Msg.OK
    });
    return false;
}
Ext.get('hidImageID').setStyle('z-index', '20000');

return (Ext.get('hidImageID').getValue().length != 0);

{##ASSISTANTPAGEWITHUPLOADHIDE}
var myDivToMove = Ext.get('uploadDiv');
myDivToMove.setStyle('visibility', 'hidden');
