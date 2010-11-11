var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}

{#FORMTYPE} = Ext.extend(Ext.ux.Wiz, {
    {#RESOURCESTRINGS}
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {    
            frame: {#FORMFRAME},
            header: {#FORMHEADER},
            closable: false,
            // monitorValid:true,
            fileUpload: {#CONTAINSFILEUPLOAD},
            title: ' ',
            bodyStyle: 'padding:5px',
            width: {#FORMWIDTH},
            labelWidth: {#LABELWIDTH},
                    
            headerConfig : {
                title : this.{#FORMCAPTION}
            },
            
            cardPanelConfig : {
                defaults : {
                    baseCls    : 'x-small-editor',
                    bodyStyle  : 'padding:40px 15px 5px 120px;background-color:#F6F6F6;',
                    border     : false    
                }
            },   
               
            cards : [{#FORMITEMSDEFINITION}]
        });
        {#FORMTYPE}.superclass.initComponent.apply(this, arguments);
    }
});

{##ASSISTANTPAGEDEFINITION}
new Ext.ux.Wiz.Card({
    title : this.{#LABEL},
    monitorValid : true,
    defaults     : {
        labelStyle : 'font-size:11px'
    },
    items : [{#ITEMS}]
})