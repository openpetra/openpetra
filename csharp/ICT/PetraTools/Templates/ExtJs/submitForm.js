Ext.onReady(function(){
    Ext.QuickTips.init();

    // use our own blank image to avoid a call home
    // Ext.BLANK_IMAGE_URL = 'resources/images/default/s.gif';
    
    Ext.form.Field.prototype.msgTarget = 'side';

    CreateCustomValidationTypePassword();

    var ItemsOnForm = {
    items:[
        {#FORMITEMSDEFINITION}
        ]}
    
    var {#FORMNAME} = new Ext.FormPanel({
        frame: true,
        // monitorValid:true,
        title: '{#FORMNAME}',
        bodyStyle: 'padding:5px',
        width: {#FORMWIDTH},
        labelWidth: {#LABELWIDTH},
        items: [{
            bodyStyle: {
            margin: '0px 0px 15px 0px'
        },
        items: ItemsOnForm
        }],
    buttons: [{#BUTTONS}]
    });
    
    {#FORMNAME}.render('{#FORMNAME}');
})

{##ROWDEFINITION}
{
    layout:'column',
    border:false,
    items: [{#ITEMS}]
}

{##CELLDEFINITION}
{
    columnWidth:{#COLUMNWIDTH},
    layout: 'form',
    border:false,
    items: [{#ITEM}]
    }

{##RADIOGROUPDEFINITION}
{
    xtype: 'fieldset',
    columnWidth: 1.0,
    border:false,
    items: [{#ITEMS}]
    }
    
{##ITEMDEFINITION}
{
    xtype: '{#XTYPE}',
{#IFDEF VTYPE}
    vtype: '{#VTYPE}',
{#ENDIF VTYPE}    
{#IFDEF INPUTTYPE}
    inputType: '{#INPUTTYPE}',
{#ENDIF INPUTTYPE}    
    fieldLabel: '{#LABEL}',
{#IFDEF ALLOWBLANK}
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    allowBlank: true,
{#ENDIFN ALLOWBLANK}
{#IFDEF OTHERPASSWORDFIELD}
    otherPasswordField: '{#OTHERPASSWORDFIELD}',
{#ENDIF OTHERPASSWORDFIELD}
{#IFDEF MINLENGTH}
    minLength: {#MINLENGTH},
{#ENDIF MINLENGTH}    
{#IFDEF MAXLENGTH}
    maxLength: {#MAXLENGTH},
{#ENDIF MAXLENGTH}
{#IFDEF COLUMNWIDTH}
    columnWidth: {#COLUMNWIDTH},
{#ENDIF COLUMNWIDTH}
{#IFDEF WIDTH}
    Width: {#WIDTH},
{#ENDIF WIDTH}
    emptyText:'{#HELP}',
    name: '{#ITEMNAME}',
    anchor: '95%'
    }

{##CHECKBOXDEFINITION}
{
    xtype: '{#XTYPE}',
{#IFDEF VTYPE}
    vtype: '{#VTYPE}',
{#ENDIF VTYPE}
{#IFDEF BOXLABEL}
    boxLabel: '{#BOXLABEL}',
{#ENDIF BOXLABEL}
    fieldLabel: '{#LABEL}',
{#IFDEF ALLOWBLANK}
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    allowBlank: true,
{#ENDIFN ALLOWBLANK}
{#IFDEF COLUMNWIDTH}
    columnWidth: {#COLUMNWIDTH},
{#ENDIF COLUMNWIDTH}
    name: '{#ITEMNAME}',
    anchor: '95%'
    }

{##SUBMITBUTTONDEFINITION}
{
text: '{#BUTTONTEXT}',
formBind: true,
handler: function () {
    // to display missing/invalid fields
    if (!contactForm.getForm().isValid())
    {
        Ext.Msg.show({
            title: '{#VALIDATIONERRORTITLE}',
            msg: '{#VALIDATIONERRORMESSAGE}',
            modal: true,
            icon: Ext.Msg.ERROR,
            buttons: Ext.Msg.OK
        });
    }
    else
    {
        Ext.MessageBox.wait('{#SENDINGDATAMESSAGE}', '{#SENDINGDATATITLE}');

        Ext.Ajax.request({
            url: '/server.asmx/{#REQUESTURL}',
            params:{
                {#REQUESTPARAMETERS},
                AJSONFormData: Ext.encode({#FORMNAME}.getForm().getValues())
            },
            success: function () {
                Ext.Msg.show({
                    title: '{#REQUESTSUCCESSTITLE}',
                    msg: '{#REQUESTSUCCESSMESSAGE}',
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK
                });
            },
            failure: function () {
                Ext.Msg.show({
                    title: '{#REQUESTFAILURETITLE}',
                    msg: '{#REQUESTFAILUREMESSAGE}',
                    modal: true,
                    icon: Ext.Msg.ERROR,
                    buttons: Ext.Msg.OK
                });
            }
        });
    }
  }    
}
