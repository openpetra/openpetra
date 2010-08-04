Ext.onReady(function(){
    Ext.QuickTips.init();

    // use our own blank image to avoid a call home
    // Ext.BLANK_IMAGE_URL = 'resources/images/default/s.gif';
    
    Ext.form.Field.prototype.msgTarget = 'side';

    Ext.apply(Ext.form.VTypes, {
       password: function(value, field)
       {
          var form = field.findParentByType('form'); 
          var f; 
          if (form && form.getForm() && (f = form.getForm().findField(field.otherPasswordField))) 
          {
             this.passwordText = 'Confirmation does not match your first password entry.';
             if (f.getValue().length > 0 && value != f.getValue())
             {
                return false;
             }
          }
     
          this.passwordText = 'Passwords must be at least 5 characters';
     
          var hasLength = (value.length >= 5);
     
          return (hasLength);
       },
     
       passwordText: 'Passwords must be at least 5 characters, containing either a number, or a valid special character (!@#$%^&*()-_=+)',
    });    
    
    var bd = Ext.getBody();
    
    var nameAndCompany = {
    items:[{    
        layout:'column',
        border:false,
        items: [{
            columnWidth:.5,
            layout: 'form',
            border:false,
            items: [{
                xtype: 'textfield',
                fieldLabel: 'Vorname',
                allowBlank: false,
                emptyText:'Den Vornamen eingeben',
                name: 'firstName',
                anchor: '95%'
                }]
            },{
            columnWidth:.5,
            layout: 'form',
            labelWidth: 80,
            border:false,
            items: [{
                xtype: 'textfield',
                fieldLabel: 'Nachname',
                allowBlank: false,
                name: 'lastname',
                anchor: '95%'
                }]
            }]
        },{
        layout: 'form',
        border:false,
        items: [{
    
                xtype: 'textfield',
                fieldLabel: 'Stra&szlig;e + Hausnummer',
                allowBlank: false,
                emptyText: 'Straßennamen eingeben',
                name: 'street',
                anchor: '95%'
            }]
        },{
        layout:'column',
        border:false,
        items: [{
            columnWidth:.4,
            layout: 'form',
            border:false,
            items: [{
                xtype: 'numberfield',
                fieldLabel: 'PLZ und Stadt',
                allowBlank: false,
                maxLength: 5,
                minLength: 5,
                emptyText:'PLZ eingeben',
                name: 'postcode',
                anchor: '95%'
                }]
            },{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
                xtype: 'textfield',
                hideLabel:true,
                emptyText:'Stadt eingeben',
                allowBlank: false,
                name: 'city',
                anchor: '95%'
                }]
            }]
        },{
        layout:'column',
        border:false,
        items: [{
            columnWidth:.5,
            layout: 'form',
            border:false,
            items: [{
                xtype: 'textfield',
                vtype: 'email',
                fieldLabel: 'Email Adresse',
                allowBlank: false,
                emptyText:'Email Adresse',
                name: 'email',
                anchor: '95%'
                },{
                xtype: 'textfield',
                inputType: 'password',
                fieldLabel: 'Passwort (für zukünftiges Bearbeiten)',
                name: 'password',
                minLength: 5,
                anchor: '95%'
                },{
                xtype: 'textfield',
                vtype: 'password',
                inputType: 'password',
                fieldLabel: 'Passwort (Wiederholung)',
                name: 'passwordConfirm',
                otherPasswordField: 'password',
                anchor: '95%'
                }]
            }]
        }]
    }
    
    var contactForm = new Ext.FormPanel({
        frame: true,
        // monitorValid:true,
        title: 'Anmeldebogen f&uuml;r das Einf&uuml;hrungsseminar',
        bodyStyle: 'padding:5px',
        width: 650,
        labelWidth: 140,
        items: [{
            bodyStyle: {
            margin: '0px 0px 15px 0px'
        },
        items: nameAndCompany
        }],
    buttons: [{
        text: 'Save',
        formBind: true,
        handler: function () {
            // to display missing/invalid fields
            if (!contactForm.getForm().isValid())
            {
                Ext.Msg.show({
                    title: 'Status',
                    msg: 'Fehler! Bitte prüfen Sie Ihre Eingaben.',
                    modal: true,
                    icon: Ext.Msg.ERROR,
                    buttons: Ext.Msg.OK
                });
            }
            else
            {
                Ext.MessageBox.wait('Daten werden gesendet', 'Bitte warten');
                //Ext.MessageBox.hide();
                
                //Ext.MessageBox.alert('Data',Ext.encode(contactForm.getForm().getValues()));
                Ext.Ajax.request({
                    url: '/server.asmx/DataImportFromForm',
                    params:{
                        AFormID: 'EFSAnmeldung',
                        AJSONFormData: Ext.encode(contactForm.getForm().getValues())
                    },
                    success: function () {
                        Ext.Msg.show({
                            title: 'Status',
                            msg: 'Daten erfolgreich gesendet.',
                            modal: true,
                            icon: Ext.Msg.INFO,
                            buttons: Ext.Msg.OK
                        });
                    },
                    failure: function () {
                        Ext.Msg.show({
                            title: 'Status',
                            msg: 'Fehler! Der Server konnte Ihre Eingaben nicht verarbeiten.',
                            modal: true,
                            icon: Ext.Msg.ERROR,
                            buttons: Ext.Msg.OK
                        });
                    }
                });
            }
          }    
        },
        {
            text: 'Cancel'
        }]
    });
    
    
    contactForm.render(document.body);    

})
