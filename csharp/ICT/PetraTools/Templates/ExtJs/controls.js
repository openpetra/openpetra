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
{#IFDEF DATERANGE}
    minValue: new Date({#MINYEAR},{#MINMONTH}-1,{#MINDAY}),
    maxValue: new Date({#MAXYEAR},{#MAXMONTH}-1,{#MAXDAY}),
    value: new Date({#DEFAULTYEAR},{#DEFAULTMONTH}-1,{#DEFAULTDAY}),
{#ENDIF DATERANGE}
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

{##HIDDENFIELDDEFINITION}
{
    xtype: 'hidden',
    value: {#VALUE},
    name: '{#ITEMNAME}'
}

{##FILEUPLOADDEFINITION}
{
    xtype: 'hidden',
    name: 'hidImageID',
},
{
    columnWidth:1,
    layout: 'form',
    border:false,
    items: [{
                xtype: 'displayfield',
                fieldLabel: this.{#LABEL},
                allowBlank: false,
                width: 300,
                html: '<img id="photoPreview" src="../../img/default_blank.gif" style="width:120px; height:160px; border-style: dotted; border-width: 1px; align: right"></div><div id="uploadDiv2" style="width:30px; height:20px"></div>',
                name: '{#ITEMNAME}',
                anchor: '97.5%'
    }]
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

{##GRIDDEFINITION}
new Ext.grid.GridPanel({
    store: store,
    width: 600,
    height: 500,
    region:'center',
    margins: '0 5 5 5',
    autoExpandColumn: 'name',
    plugins: [editor],
    view: new Ext.grid.GroupingView({
        markDirty: false
    }),

    columns: [
    new Ext.grid.RowNumberer(),
    {
        id: 'name',
        header: 'First Name',
        dataIndex: 'name',
        width: 220,
        sortable: true,
        editor: {
            xtype: 'textfield',
            allowBlank: false
        }
    },{
        header: 'Email',
        dataIndex: 'email',
        width: 150,
        sortable: true,
        editor: {
            xtype: 'textfield',
            allowBlank: false,
            vtype: 'email'
        }
    },{
        xtype: 'datecolumn',
        header: 'Start Date',
        dataIndex: 'start',
        format: 'm/d/Y',
        width: 100,
        sortable: true,
        groupRenderer: Ext.util.Format.dateRenderer('M y'),
        editor: {
            xtype: 'datefield',
            allowBlank: false,
            minValue: '01/01/2006',
            minText: 'Can\'t have a start date before the company existed!',
            maxValue: (new Date()).format('m/d/Y')
        }
    },{
        xtype: 'numbercolumn',
        header: 'Salary',
        dataIndex: 'salary',
        format: '$0,0.00',
        width: 100,
        sortable: true,
        editor: {
            xtype: 'numberfield',
            allowBlank: false,
            minValue: 1,
            maxValue: 150000
        }
    },{
        xtype: 'booleancolumn',
        header: 'Active',
        dataIndex: 'active',
        align: 'center',
        width: 50,
        trueText: 'Yes',
        falseText: 'No',
        editor: {
            xtype: 'checkbox'
        }
    }]
})
