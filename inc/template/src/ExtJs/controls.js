{##ROWDEFINITION}
{
    xtype: 'container',
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
    layout: 'anchor',
    border:false,
    items: [{#ITEM}]
}

{##RADIOGROUPDEFINITION}
{
    xtype: 'fieldset',
    columnWidth: 1.0,
    border:false,
    defaults: {
        anchor: '100%',
        hideEmptyLabel: false
    },
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
{#IFDEF REGEX}
    regex: new RegExp("{#REGEX}"),
{#ENDIF REGEX}
{#IFDEF HIDELABEL}
    hideLabel: {#HIDELABEL},
{#ENDIF HIDELABEL}
{#IFDEF ALLOWBLANK}
    fieldLabel: this.{#LABEL},
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    fieldLabel: this.{#LABEL}+' *',
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
{#IFDEF MINYEAR}
    minValue: new Date({#MINYEAR},{#MINMONTH}-1,{#MINDAY}),
{#ENDIF MINYEAR}
{#IFDEF MAXYEAR}
    maxValue: new Date({#MAXYEAR},{#MAXMONTH}-1,{#MAXDAY}),
{#ENDIF MAXYEAR}
{#IFDEF DATEFORMAT}
    format: '{#DATEFORMAT}',
{#ENDIF DATEFORMAT}
{#IFDEF SHOWTODAY}
    showToday: {#SHOWTODAY},
{#ENDIF SHOWTODAY}
{#IFDEF DEFAULTYEAR}
    value: new Date({#DEFAULTYEAR},{#DEFAULTMONTH}-1,{#DEFAULTDAY}),
{#ENDIF DEFAULTYEAR}
{#IFDEF HTML}
    html: '{#HTML}',
{#ENDIF HTML}
    {#CUSTOMATTRIBUTES}
    emptyText: this.{#HELP},
    name: '{#ITEMNAME}',
    id: '{#ITEMID}',
    anchor: '{#ANCHOR}'
}

{##LABELDEFINITION}
{
    xtype: '{#XTYPE}',
    hideLabel: true,
    value: this.{#LABEL}
}

{##INLINEDEFINITION}
{
    xtype: '{#XTYPE}',
    hideLabel: true,
    value: this.{#LABEL}
},
{
    xtype: '{#XTYPE}',
    hideLabel: true,
    html: '<iframe src="' + this.{#ITEMNAME}URL + '" width="100%" height="{#HEIGHT}"><p>' + this.BROWSERMISSINGIFRAMESUPPORT + '<a href="' + this.{#ITEMNAME}URL + '">' + this.{#ITEMNAME}LABEL + '</a></p></iframe><br/><a href="' + this.{#ITEMNAME}URL + '" target="newbigger">' + this.IFRAMEINBIGGERWINDOW + '</a>',
    name: '{#ITEMNAME}',
    id: '{#ITEMNAME}',   
    anchor: '{#ANCHOR}'
}

{##TEXTAREADEFINITION}
{
    xtype: 'displayfield',
    hideLabel: true,
{#IFDEF ALLOWBLANK}
    value: this.{#LABEL}
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    value: this.{#LABEL}+' *'
{#ENDIFN ALLOWBLANK}
},
{
    xtype: 'textarea',
{#IFDEF ALLOWBLANK}
    allowBlank: {#ALLOWBLANK},
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    allowBlank: false,
{#ENDIFN ALLOWBLANK}
{#IFDEF COLUMNWIDTH}
    columnWidth: {#COLUMNWIDTH},
{#ENDIF COLUMNWIDTH}
{#IFDEF WIDTH}
    width: {#WIDTH},
{#ENDIF WIDTH}
    hideLabel: true,
    {#CUSTOMATTRIBUTES}
    emptyText: this.{#HELP},
    name: '{#ITEMNAME}',
    id: '{#ITEMNAME}',   
    anchor: '{#ANCHOR}'
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
    id: 'hidImageID'
},
{
    columnWidth:1,
    layout: 'anchor',
    border:false,
    items: [{
                xtype: 'displayfield',
                fieldLabel: this.{#LABEL}+' *',
                allowBlank: false,
                width: 300,
                html: '<div><img id="photoPreview" src="../../img/default_blank.gif" style="width:120px; height:160px; border-style: dotted; border-width: 1px; align: right"></img><div id="uploadDiv" style="position:relative;top:-160px;left:125px;"/></div>',
                name: '{#ITEMNAME}',
                id: '{#ITEMNAME}',
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
{#IFNDEF HIDELABEL}
    fieldLabel: this.{#LABEL},
{#ENDIFN HIDELABEL}
{#IFDEF COLUMNWIDTH}
    columnWidth: {#COLUMNWIDTH},
{#ENDIF COLUMNWIDTH}
{#IFDEF INPUTVALUE}
    inputValue: '{#INPUTVALUE}',
{#ENDIF INPUTVALUE}
    name: '{#ITEMNAME}',
    id: '{#ITEMID}',
    anchor: '{#ANCHOR}'
}

{##COMBOBOXDEFINITION}
Ext.create('Ext.form.field.ComboBox',
  {
    name:'{#ITEMNAME}',
    store: new Ext.data.ArrayStore({
        fields: ['value', 'display'],
        data : {#OPTIONALVALUESARRAY}
    }),
    valueField:'value',
    displayField:'display',
    typeAhead: true,
    forceSelection: true,
{#IFDEF VALUE}    
    value: '{#VALUE}',
{#ENDIF VALUE}
    mode: 'local',
    triggerAction: 'all',
    emptyText: this.{#HELP},
{#IFDEF ALLOWBLANK}
    fieldLabel: this.{#LABEL},
    allowBlank: true,
{#ENDIF ALLOWBLANK}
{#IFNDEF ALLOWBLANK}
    fieldLabel: this.{#LABEL}+' *',
    allowBlank: false,
{#ENDIFN ALLOWBLANK}
    selectOnFocus:true,
    labelWidth: {#LABELWIDTH},
    width: {#WIDTH} + {#LABELWIDTH}
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
