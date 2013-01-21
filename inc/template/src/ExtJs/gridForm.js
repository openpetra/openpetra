var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}

{#FORMTYPE} = Ext.extend(Ext.FormPanel, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: {#FORMFRAME},
            header: {#FORMHEADER},
            // monitorValid:true,
            fileUpload: false,
            title: this.{#FORMCAPTION},
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
            items: [{#FORMITEMSDEFINITION}]
        });
        {#FORMTYPE}.superclass.initComponent.apply(this, arguments);
    }
});