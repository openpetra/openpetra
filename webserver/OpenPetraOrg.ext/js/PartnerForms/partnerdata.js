Ext.onReady(function(){
    Ext.QuickTips.init();

    // use our own blank image to avoid a call home
    // Ext.BLANK_IMAGE_URL = 'resources/images/default/s.gif';

    Ext.form.Field.prototype.msgTarget = 'side';

    CreateCustomValidationTypePassword();

    var ItemsOnForm = {
    items:[
        {
            layout:'column',
            border:false,
            items: [{
            columnWidth:0.5,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'First name',
            allowBlank: false,
            emptyText:'Please enter first name',
            name: 'txtFirstName',
            anchor: '95%'
            }
        ]
            }
        ,{
            columnWidth:0.5,
            labelWidth: 80,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Last Name',
            allowBlank: false,
            emptyText:'',
            name: 'txtLastName',
            anchor: '95%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Street',
            allowBlank: false,
            emptyText:'Streetname and house number',
            name: 'txtStreet',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:.4,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Postcode and City',
            allowBlank: false,
            emptyText:'Postcode',
            name: 'txtPostcode',
            anchor: '94%'
            }
        ]
            }
        ,{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'City',
            hideLabel: true,
            allowBlank: false,
            emptyText:'City',
            name: 'txtCity',
            anchor: '96.25%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Phone',
            allowBlank: true,
            emptyText:'',
            name: 'txtPhone',
            anchor: '94%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            vtype: 'email',
            fieldLabel: 'Email',
            allowBlank: false,
            emptyText:'',
            name: 'txtEmail',
            anchor: '94%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            inputType: 'password',
            fieldLabel: 'Password',
            allowBlank: false,
            emptyText:'',
            name: 'txtPassword',
            anchor: '94%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:.6,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            vtype: 'password',
            inputType: 'password',
            fieldLabel: 'Password Confirm',
            allowBlank: false,
            otherPasswordField: 'txtPassword',
            emptyText:'',
            name: 'txtPasswordConfirm',
            anchor: '94%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            xtype: 'fieldset',
            columnWidth: 1.0,
            border:false,
            items: [{
            xtype: 'radio',
            boxLabel: 'Pupil',
            fieldLabel: 'Employment Status',
            allowBlank: true,
            name: 'rgrEmploymentStatus',
            anchor: '97.5%'
            }
        ,{
            xtype: 'radio',
            checked: true,
            boxLabel: 'Student',
            fieldLabel: '',
            allowBlank: true,
            name: 'rgrEmploymentStatus',
            anchor: '97.5%'
            }
        ,{
            xtype: 'radio',
            boxLabel: 'Unemployed',
            fieldLabel: '',
            allowBlank: true,
            name: 'rgrEmploymentStatus',
            anchor: '97.5%'
            }
        ,{
            xtype: 'radio',
            boxLabel: 'Employed',
            fieldLabel: '',
            allowBlank: true,
            name: 'rgrEmploymentStatus',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Degree/Profession',
            allowBlank: false,
            emptyText:'Enter your school degree or profession',
            name: 'txtProfession',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'fieldset',
            title: 'Bank Details',
            autoHeight: true,
            items: [{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'compositefield',
            fieldLabel: 'Direct Debit Permission',
            hideLabel: true,
            items: [{
            xtype: 'displayfield',
            hideLabel: true,
            value: 'Hereby I give permission to xyz to do a direct debit of'
        }
        ,{
            xtype: 'textfield',
            fieldLabel: 'Conference Fee',
            allowBlank: false,
            emptyText:'',
            name: 'txtConferenceFee',
            anchor: '95%'
            }
        ,{
            xtype: 'displayfield',
            hideLabel: true,
            value: 'Euro from this bank account:'
        }
        ]
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Kontoinhaber',
            allowBlank: false,
            emptyText:'',
            name: 'txtKontoinhaber',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Kontonummer',
            allowBlank: false,
            emptyText:'',
            name: 'txtKontonummer',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Iban',
            allowBlank: false,
            emptyText:'',
            name: 'txtIban',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Kreditinstitut',
            allowBlank: false,
            emptyText:'',
            name: 'txtKreditinstitut',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Bank Ort',
            allowBlank: false,
            emptyText:'',
            name: 'txtBankOrt',
            anchor: '97.5%'
            }
        ]
            }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'displayfield',
            hideLabel: true,
            value: 'After submitting the form, you will print a PDF, and you (and for minors also their parents) have to sign this'
        }
        ]
            }
        ]
        }
        ]
            }
        ]
            }
        ]
        }
        ]}

    var partnerdata = new Ext.FormPanel({
        frame: true,
        // monitorValid:true,
        title: 'partnerdata',
        bodyStyle: 'padding:5px',
        width: 650,
        labelWidth: 140,
        items: [{
            bodyStyle: {
            margin: '0px 0px 15px 0px'
        },
        items: ItemsOnForm
        }],
    buttons: [{
text: 'Save'
,formBind: true,
handler: function () {
    // to display missing/invalid fields
    if (!partnerdata.getForm().isValid())
    {
        Ext.Msg.show({
            title: '"Input Error"',
            msg: '"Please check the flagged controls!"',
            modal: true,
            icon: Ext.Msg.ERROR,
            buttons: Ext.Msg.OK
        });
    }
    else
    {
        Ext.MessageBox.wait('"Data are being sent to the server"', '"Please wait"');

        Ext.Ajax.request({
            url: '/server.asmx/DataImportFromForm',
            params:{
                depth: '0', AFormID: 'EFSAnmeldung',
                AJSONFormData: Ext.encode(partnerdata.getForm().getValues())
            },
            success: function () {
                Ext.Msg.show({
                    title: '"Success"',
                    msg: '"Your application has been successful. You will receive an email with a PDF file. Please print the PDF file, sign it, and send it to us via post"',
                    modal: true,
                    icon: Ext.Msg.INFO,
                    buttons: Ext.Msg.OK
                });
            },
            failure: function () {
                Ext.Msg.show({
                    title: '"Failure"',
                    msg: '"Something did not work on the server."',
                    modal: true,
                    icon: Ext.Msg.ERROR,
                    buttons: Ext.Msg.OK
                });
            }
        });
    }
  }
}
,{
text: 'Cancel'
}
]
    });

    partnerdata.render('partnerdata');
})
