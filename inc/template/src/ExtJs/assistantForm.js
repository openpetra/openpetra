var {#FORMNAME} = null;

{#INCLUDE functions.js}
{#INCLUDE controls.js}
{#INCLUDE upload.js}

Ext.define('{#FORMTYPE}', {
    extend: 'Ext.Panel',
    id:'card-assistant-panel',
    layout:'card',
    activeItem: 0,
    bodyStyle: 'padding:15px',
    defaults: {border:false},
    {#RESOURCESTRINGS}
    strEmpty:'',
    
    /// get the data as a structured object
    getAssistantData: function () 
    {
        var layout = this.getLayout();
        var formValues = {};
        var cards = layout.getLayoutItems();

        for (var i = 0, len = cards.length; i < len; i++) 
        {
            var f = cards[i].getForm();
            if (f) 
            {
                formValues[cards[i].id] = f.getValues(false);
            }
            else
            {
                formValues[cards[i].id] = {};
            }
        }

        return formValues;
    },

    {#BUTTONS}

    /// navigate across the pages
    cardNav: function(incr){
        var layout = this.getLayout();
        var i = layout.activeItem.id.split('card-')[1];
        var next = parseInt(i, 10) + incr;
        
        f = layout.activeItem.getForm();
        if (f.isValid()) {
            layout.setActiveItem(next);
            Ext.getCmp('card-prev').setDisabled(!layout.getPrev());
            Ext.getCmp('card-next').setDisabled(!layout.getNext());
            if (!layout.getNext())
            {
                Ext.getCmp('card-finish').show();
                Ext.getCmp('card-next').hide();
            }
            else
            {
                Ext.getCmp('card-finish').hide();
                Ext.getCmp('card-next').show();
            }

            Ext.getCmp('card-assistant-panel').setTitle(this.{#FORMCAPTION} + 
                ': ' + 
                this.{#FORMCAPTION}Page + ' ' + (next + 1) + ' ' +
                this.{#FORMCAPTION}Of + ' ' + layout.getLayoutItems().length + ': ' + layout.activeItem.title);
        }
    },

    /// constructor creates the pages
    /// this is necessary for the localisation to work with the class variables
    constructor: function(owner, config) {
        var me = this;

        me.callParent();

        me.title = this.{#FORMCAPTION};
        
        {#FORMITEMSDEFINITION};
        
        var toolbar = Ext.create('Ext.toolbar.Toolbar',
            {
                xtype: 'toolbar',
                dock: 'bottom',
                valign: 'right',
            });
        toolbar.add('->');
        toolbar.add({
            id: 'card-prev',
            text: '&laquo; Previous',
            handler: Ext.Function.bind(this.cardNav, this, [-1]),
            disabled: true
        });
        toolbar.add({
            id: 'card-next',
            text: 'Next &raquo;',
            handler: Ext.Function.bind(this.cardNav, this, [1])
        });
        toolbar.add({
            id: 'card-finish',
            text: 'Finish',
            hidden: true,
            handler: Ext.Function.bind(this.cardFinish, this)
        });

        me.dockedItems.add(toolbar);
    },
    
    // items will be added in the constructor, otherwise the localisation would not work
    items: []
});

{#UPLOADFORM}

{##ASSISTANTPAGEDEFINITION}
me.items.add(Ext.create('Ext.FormPanel',
    {
        id: 'card-{#PAGENUMBER}',
        border: false,
        title: this.{#LABEL},
        preventHeader: true,
        onShow: function() {
            Ext.form.Panel.superclass.onShow.call(this);
{#IFDEF ONSHOW}
            {#ONSHOW}
{#ENDIF ONSHOW}
            window.scrollTo(0,0);
        },
{#IFDEF ISVALID}
        isValid: function() {
            {#ISVALID}
        },
{#ENDIF ISVALID}
{#IFDEF ONHIDE}
        onHide: function() {
            Ext.form.Panel.superclass.onHide.call(this);
            {#ONHIDE}
        },
{#ENDIF ONHIDE}
        {#CUSTOMFUNCTIONS}
        labelWidth: {#LABELWIDTH},
        defaults     : {
            labelStyle : 'font-size:11px'
        },
        items : [{#ITEMS}]
    }));

{##SUBMITBUTTONDEFINITION}
cardFinish: function() {
    var layout = this.getLayout();
    f = layout.activeItem.getForm();
    if (!f.isValid())
    {
        return;
    }

    Ext.MessageBox.wait(this.{#SENDINGDATAMESSAGE}, this.{#SENDINGDATATITLE});

    Ext.Ajax.request({
        url: '/server.asmx/{#REQUESTURL}',
        params:{
            {#REQUESTPARAMETERS}
            AJSONFormData: Ext.encode(this.getAssistantData())
        },
        success: function (response) {
            jsonData = XmlExtractJSONResponse(response);
            if (jsonData.failure == true)
            {
                  Ext.Msg.show({
                      title: {#FORMNAME}.{#REQUESTFAILURETITLE},
                      msg: {#FORMNAME}.{#REQUESTFAILUREMESSAGE} + "<br/>" + jsonData.data.result,
                      modal: true,
                      icon: Ext.Msg.ERROR,
                      buttons: Ext.Msg.OK
                  });
            }
            else
            {
{#IFDEF REQUESTSUCCESSMESSAGE}
                Ext.Msg.show({
                    title: {#FORMNAME}.{#REQUESTSUCCESSTITLE},
                    msg: {#FORMNAME}.{#REQUESTSUCCESSMESSAGE},
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK,
                    fn: function(btn, text){
                        {#REDIRECTONSUCCESS}
                    }
                });
{#ENDIF REQUESTSUCCESSMESSAGE}
{#IFNDEF REQUESTSUCCESSMESSAGE}
                {#REDIRECTONSUCCESS}
{#ENDIFN REQUESTSUCCESSMESSAGE}
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
},

{##REDIRECTONSUCCESS}
{#IFNDEF REDIRECTDOWNLOAD}
{#IFDEF REDIRECTURLONSUCCESS}
location.href={#FORMNAME}.{#REDIRECTURLONSUCCESS};
{#ENDIF REDIRECTURLONSUCCESS}
{#ENDIFN REDIRECTDOWNLOAD}
{#IFDEF REDIRECTDOWNLOAD}
location.href={#FORMNAME}.{#REDIRECTURLONSUCCESS} + '?download=' + {#REDIRECTDOWNLOAD};
{#ENDIF REDIRECTDOWNLOAD}
