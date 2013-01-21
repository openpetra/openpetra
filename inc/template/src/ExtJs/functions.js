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
                this.markInvalid({#FORMNAME}.strErrorCheckboxRequired); 
                return false;
            }
            else 
            {
                this.clearInvalid();
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
        jsonData = Ext.JSON.decode(jsonString);
        return jsonData;
    }
}
{#ENDIF REQUESTPARAMETERS}

{#IFDEF DATAGRID}
var editor = new Ext.ux.grid.RowEditor({
    saveText: 'Update'
});
{#ENDIF DATAGRID}

{##LANGUAGEFILE}
if({#FORMTYPE}) {
    Ext.apply({#FORMTYPE}.prototype, {
    {#RESOURCESTRINGS}
   });
}