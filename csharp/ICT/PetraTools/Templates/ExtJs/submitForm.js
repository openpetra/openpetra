var {#FORMNAME} = null;

{#IFDEF PASSWORDTWICE}
Ext.onReady(function() {
    /// validation method for checking a password has been entered twice correctly, and is at least 5 characters
    Ext.apply(Ext.form.VTypes, {
       password: function(value, field)
       {
          var f; 

          if ((f = {#FORMNAME}.getForm().findField(field.otherPasswordField))) 
          {
             this.passwordText = {#FORMNAME}.strErrorPasswordNoMatch;
             if (f.getValue().length > 0 && value != f.getValue())
             {
                return false;
             }
          }
     
          this.passwordText = {#FORMNAME}.strErrorPasswordLength;
     
          var hasLength = (value.length >= 5);
     
          return (hasLength);
       },
     
       passwordText: '',
    });
});
{#ENDIF PASSWORDTWICE}

{#IFDEF FORCECHECKBOX}
Ext.onReady(function() {
    /// validation method for checking if a checkbox has been ticked
    Ext.form.Checkbox.prototype.validate = function()
    {
        if (this.vtype == "forcetick")
        {
            if (!this.checked) 
            {
                Ext.form.Field.prototype.markInvalid.call(this, {#FORMNAME}.strErrorCheckboxRequired); 
                return false;
            }
            else 
            {
                Ext.form.Field.prototype.clearInvalid.call(this);
            }
        }

        return true;
    };    
});
{#ENDIF FORCECHECKBOX}

{#IFDEF REQUESTPARAMETERS}
function XmlExtractJSONResponse(response)
{
    var xml = response.responseXML;
    var stringDataNode = xml.getElementsByTagName('string')[0];
    if(stringDataNode){
        jsonString = stringDataNode.firstChild.data;
        jsonData = Ext.util.JSON.decode(jsonString);
        return jsonData;
    }
}
{#ENDIF REQUESTPARAMETERS}

{#IFDEF ASSISTANT}
// This is a fake CardLayout navigation function.  A real implementation would
// likely be more sophisticated, with logic to validate navigation flow.  It will
// be assigned next as the handling function for the buttons in the CardLayout example.
var cardNav = function(incr){
    var l = Ext.getCmp('card-assistant-panel').getLayout();
    var i = l.activeItem.id.split('card-')[1];
    var next = parseInt(i) + incr;
    l.setActiveItem(next);
    Ext.getCmp('card-prev').setDisabled(next==0);
    Ext.getCmp('card-next').setDisabled(next==2);
};
{#ENDIF ASSISTANT}

{#FORMNAME}Form = Ext.extend(Ext.FormPanel, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: {#FORMFRAME},
            header: {#FORMHEADER},
            // monitorValid:true,
            fileUpload: {#CONTAINSFILEUPLOAD},
            title: this.{#FORMCAPTION},
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
            items: [{#FORMITEMSDEFINITION}],
            buttons: [{#BUTTONS}]
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

{##ASSISTANTDEFINITION}
{
    id: 'card-assistant-panel',
    title: this.{#LABEL},
    header: {#ASSISTANTHEADER},
    layout:'card',
    activeItem: 0,
    bodyStyle: 'padding:15px',
    defaults: {border:false},
    bbar: ['->', {
        id: 'card-prev',
        text: '&laquo; Previous',
        handler: cardNav.createDelegate(this, [-1]),
        disabled: true
    },{
        id: 'card-next',
        text: 'Next &raquo;',
        handler: cardNav.createDelegate(this, [1])
    }],
    items: [{#ITEMS}]
}

{##ASSISTANTPAGEDEFINITION}
{
    id: 'card-{#PAGENUMBER}',
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
                success: function (response) {
                    jsonData = XmlExtractJSONResponse(response);
                    if (jsonData.failure == true)
                    {
                          Ext.Msg.show({
                              title: partnerdata.btnSaveREQUESTFAILURETITLE,
                              msg: partnerdata.btnSaveREQUESTFAILUREMESSAGE + "<br/>" + jsonData.data.result,
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

{##LANGUAGEFILE}
if({#FORMNAME}Form) {
    Ext.apply({#FORMNAME}Form.prototype, {
    {#RESOURCESTRINGS}
   });
}
