{#FORMNAME}Form = Ext.extend(Ext.FormPanel, {
    {#RESOURCESTRINGS}
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: true,
            // monitorValid:true,
            title: {#FORMCAPTION},
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
            items: [{
                bodyStyle: {
                margin: '0px 0px 15px 0px'
            },
            items: [{#FORMITEMSDEFINITION}]
            }],
        buttons: [{#BUTTONS}
        ]
        });
        {#FORMNAME}Form.superclass.initComponent.apply(this, arguments);
    }
});
    
{##ROWDEFINITION}
{
    layout:'column',
    border:false,
    items: [{#ITEMS}]
}

{##CELLDEFINITION}
{
    columnWidth:{#COLUMNWIDTH},
{#IFDEF LABELWIDTH}
    labelWidth: {#LABELWIDTH},
{#ENDIF LABELWIDTH}
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

{##GROUPBOXDEFINITION}
{
    xtype: 'fieldset',
    title: {#LABEL},
    autoHeight: true,
    items: [{#ITEMS}]
    }

{##COMPOSITEDEFINITION}
{
    xtype: 'compositefield',
    fieldLabel: {#LABEL},
{#IFDEF HIDELABEL}
    hideLabel: {#HIDELABEL},
{#ENDIF HIDELABEL}
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
    fieldLabel: {#LABEL},
{#IFDEF HIDELABEL}
    hideLabel: {#HIDELABEL},
{#ENDIF HIDELABEL}
{#IFDEF ALLOWBLANK}
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    allowBlank: false,
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
    width: {#WIDTH},
{#ENDIF WIDTH}
    {#CUSTOMATTRIBUTES}
    emptyText: {#HELP},
    name: '{#ITEMNAME}',
    anchor: '{#ANCHOR}'
    }

{##LABELDEFINITION}
{
    xtype: '{#XTYPE}',
    hideLabel: true,
    value: {#LABEL}
}

{##CHECKBOXDEFINITION}
{
    xtype: '{#XTYPE}',
{#IFDEF VTYPE}
    vtype: '{#VTYPE}',
{#ENDIF VTYPE}
{#IFDEF CHECKED}
    checked: {#CHECKED},
{#ENDIF CHECKED}
{#IFDEF BOXLABEL}
    boxLabel: {#BOXLABEL},
{#ENDIF BOXLABEL}
    fieldLabel: {#LABEL},
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
    anchor: '{#ANCHOR}'
    }

{##SUBMITBUTTONDEFINITION}
{
text: {#LABEL}
{#IFDEF REQUESTURL}
,formBind: true,
handler: function () {
    // to display missing/invalid fields
    if (!{#FORMNAME}.getForm().isValid())
    {
        Ext.Msg.show({
            title: {#VALIDATIONERRORTITLE},
            msg: {#VALIDATIONERRORMESSAGE},
            modal: true,
            icon: Ext.Msg.ERROR,
            buttons: Ext.Msg.OK
        });
    }
    else
    {
        Ext.MessageBox.wait({#SENDINGDATAMESSAGE}, {#SENDINGDATATITLE});

        Ext.Ajax.request({
            url: '/server.asmx/{#REQUESTURL}',
            params:{
                {#REQUESTPARAMETERS}
                AJSONFormData: Ext.encode({#FORMNAME}.getForm().getValues())
            },
            success: function () {
                Ext.Msg.show({
                    title: {#REQUESTSUCCESSTITLE},
                    msg: {#REQUESTSUCCESSMESSAGE},
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK
                });
            },
            failure: function () {
                Ext.Msg.show({
                    title: {#REQUESTFAILURETITLE},
                    msg: {#REQUESTFAILUREMESSAGE},
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

{##LANGUAGEFILE}
if({#FORMNAME}Form) {
    Ext.apply({#FORMNAME}Form.prototype, {
    {#RESOURCESTRINGS}
   });
}