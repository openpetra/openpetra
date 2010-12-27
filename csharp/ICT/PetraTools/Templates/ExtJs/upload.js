{##UPLOADFORMDEFINITION}
var UploadForm = null;

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
                                            var imgID = Ext.get('hidImageID');
                                            imgID.value = o.result.file;
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

{##ASSISTANTPAGEWITHUPLOAD}
onShow: function() {
    Ext.ux.Wiz.Card.superclass.onShow.call(this);
    var myDivDestination = Ext.get('photoPreview');
    var myDivToMove = Ext.get('uploadDiv');
    myDivToMove.setStyle('z-index', '20000');
    myDivToMove.setStyle('visibility', 'visible');
    myDivToMove.moveTo(myDivDestination.getX() + myDivDestination.getWidth() + 10, myDivDestination.getY());
},
isValid: function() {
    // use the little trick of setting the z-index so that the message box does not get displayed when the form is shown the first time
    if (Ext.get('hidImageID').value == undefined && Ext.get('hidImageID').getStyle('z-index') != 'auto')
    {
        Ext.Msg.show({
            title: 'Please upload photo',
            msg: 'Please upload photo',
            modal: true,
            icon: Ext.Msg.ERROR,
            buttons: Ext.Msg.OK
        });
        return false;
    }
    Ext.get('hidImageID').setStyle('z-index', '20000');

    return (Ext.get('hidImageID').value != undefined);
},
onHide: function() {
    Ext.ux.Wiz.Card.superclass.onHide.call(this);
    var myDivToMove = Ext.get('uploadDiv');
    myDivToMove.setStyle('visibility', 'hidden');
},
