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
{#IFDEF HTML}
    html: '{#HTML}',
{#ENDIF HTML}
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

{##FILEUPLOADDEFINITION}
{
    xtype: 'fileuploadfield',
    id: '{#ITEMNAME}-file',
    emptyText: this.{#HELP},
    fieldLabel: this.{#LABEL},
    name: '{#ITEMNAME}',
    buttonText: '',
    buttonCfg: {
        iconCls: 'upload-icon'
    }
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
{#IFDEF HIDELABEL}
    hideLabel: {#HIDELABEL},
{#ENDIF HIDELABEL}
    fieldLabel: this.{#LABEL},
{#IFDEF COLUMNWIDTH}
    columnWidth: {#COLUMNWIDTH},
{#ENDIF COLUMNWIDTH}
    name: '{#ITEMNAME}',
    anchor: '{#ANCHOR}'
}

{##COMBOBOXDEFINITION}
new Ext.form.ComboBox({
    fieldLabel: this.{#LABEL},
    hiddenName:'{#ITEMNAME}',
    store: new Ext.data.ArrayStore({
        fields: ['value', 'display'],
        data : {#OPTIONALVALUESARRAY}
    }),
    valueField:'value',
    displayField:'display',
    typeAhead: true,
    mode: 'local',
    triggerAction: 'all',
    emptyText: this.{#HELP},
{#IFDEF ALLOWBLANK}
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    allowBlank: false,
{#ENDIFN ALLOWBLANK}
    selectOnFocus:true,
    width:190
})
