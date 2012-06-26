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
    validate: true,
    {#RESOURCESTRINGS}
    nextButtonText:'Next',
    previousButtonText:'Previous',
    cancelButtonText:'Cancel',
    finishButtonText:'Finish',
    stepText:'page {0} of {1}',
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

        // for testing and making screenshots, do not validate
        if (({#FORMNAME}.validate == false) || f.isValid()) 
        {
            layout.setActiveItem(next);
            Ext.getCmp('btn-prev').setDisabled(!layout.getPrev());
            Ext.getCmp('btn-next').setDisabled(!layout.getNext());
            if (!layout.getNext())
            {
                Ext.getCmp('btn-finish').show();
                Ext.getCmp('btn-next').hide();
            }
            else
            {
                Ext.getCmp('btn-finish').hide();
                Ext.getCmp('btn-next').show();
            }

            Ext.getCmp('card-assistant-panel').setTitle(this.{#FORMCAPTION} + 
                ': ' + 
                this.stepText.replace('{0}', (next + 1)).replace('{1}', layout.getLayoutItems().length) + 
                ': ' + layout.activeItem.title);
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
            id: 'btn-cancel',
            text: this.cancelButtonText,
            handler: Ext.Function.bind(this.cardCancel, this),
            disabled: false
        });
        toolbar.add({
            id: 'btn-prev',
            text: '&laquo; ' + this.previousButtonText,
            handler: Ext.Function.bind(this.cardNav, this, [-1]),
            disabled: true
        });
        toolbar.add({
            id: 'btn-next',
            text: this.nextButtonText + ' &raquo;',
            handler: Ext.Function.bind(this.cardNav, this, [1])
        });
        toolbar.add({
            id: 'btn-finish',
            text: this.finishButtonText,
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
            MainForm.setHeight(50+Ext.getCmp('lowest-card-{#PAGENUMBER}').getPosition()[1]);
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
        items : [{#ITEMS}
         ,{xtype: 'label', name: 'lowest-card-{#PAGENUMBER}', id: 'lowest-card-{#PAGENUMBER}'}]
    }));

{##SUBMITBUTTONDEFINITION}
cardCancel: function() {
    Ext.Msg.show({
        title: {#FORMNAME}.{#CANCELQUESTIONTITLE},
        msg: {#FORMNAME}.{#CANCELQUESTIONMESSAGE},
        modal: true,
        icon: Ext.Msg.QUESTION,
        buttons: Ext.Msg.YESNO,
        fn: function(btn, text){
          if(btn === 'yes'){ 
            location.href={#FORMNAME}.{#REDIRECTURLONCANCEL};
          }
        }
    });
},

cardFinish: function() {
    var layout = this.getLayout();
    f = layout.activeItem.getForm();

    if ({#FORMNAME}.validate == false)
    {
        // need to validate all pages now
        var cards = layout.getLayoutItems();

        var invalid = false;
        for (var i = 0, len = cards.length; i < len; i++) 
        {
            var f = cards[i].getForm();
            if (!f.isValid())
            {
                invalid = true;
            }
        }

        if (invalid)
        {
            Ext.Msg.show({
              title: "DEBUGMODE: Missing some data",
              msg: "You are using this form with the checkbox set for quickly clicking through the forms.<br/>Please enter all data, otherwise you cannot submit the application",
              modal: true,
              icon: Ext.Msg.ERROR,
              buttons: Ext.Msg.OK
            });

            return;
        }
    }

    if (!f.isValid())
    {
        return;
    }

    Ext.MessageBox.wait(this.{#SENDINGDATAMESSAGE}, this.{#SENDINGDATATITLE});

    // Extend timeout for all Ext.Ajax.requests to 180 seconds. Default is 30 seconds. 
    Ext.Ajax.timeout = 180000;

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
        failure: function (response, opts) {
            Ext.Msg.show({
                title: {#FORMNAME}.{#REQUESTFAILURETITLE},
                msg: {#FORMNAME}.{#REQUESTFAILUREMESSAGE} + " Statuscode: " + response.status,
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
