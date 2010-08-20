var {#FORMNAME} = null;
{#FORMNAME}Form = Ext.extend(Ext.FormPanel, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: true,
            // monitorValid:true,
            title: this.{#FORMCAPTION},
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
    title: this.{#LABEL},
    autoHeight: true,
    items: [{#ITEMS}]
    }

{##COMPOSITEDEFINITION}
{
    xtype: 'compositefield',
    fieldLabel: this.{#LABEL},
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
    fieldLabel: this.{#LABEL},
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
    emptyText: this.{#HELP},
    name: '{#ITEMNAME}',
    anchor: '{#ANCHOR}'
    }

{##LABELDEFINITION}
{
    xtype: '{#XTYPE}',
    hideLabel: true,
    value: this.{#LABEL}
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
    boxLabel: this.{#BOXLABEL},
{#ENDIF BOXLABEL}
    fieldLabel: this.{#LABEL},
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
    else
    {
        Ext.MessageBox.wait({#FORMNAME}.{#SENDINGDATAMESSAGE}, {#FORMNAME}.{#SENDINGDATATITLE});

        Ext.Ajax.request({
            url: '/server.asmx/{#REQUESTURL}',
            params:{
                {#REQUESTPARAMETERS}
                AJSONFormData: Ext.encode({#FORMNAME}.getForm().getValues())
            },
            success: function () {
                Ext.Msg.show({
                    title: {#FORMNAME}.{#REQUESTSUCCESSTITLE},
                    msg: {#FORMNAME}.{#REQUESTSUCCESSMESSAGE},
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK
                });
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

{##LANGUAGEFILE}
if({#FORMNAME}Form) {
    Ext.apply({#FORMNAME}Form.prototype, {
    {#RESOURCESTRINGS}
   });
}