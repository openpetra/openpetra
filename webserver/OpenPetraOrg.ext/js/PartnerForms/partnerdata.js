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
            allowBlank: true,
            columnWidth: 0.5,
            emptyText:'TODO',
            name: 'txtFirstName',
            anchor: '95%'
            }]
        }
        ,{
        columnWidth:0.5,
        layout: 'form',
        border:false,
        items: [{
            xtype: 'textfield',
            fieldLabel: 'Last Name',
            allowBlank: true,
            columnWidth: 0.5,
            emptyText:'TODO',
            name: 'txtLastName',
            anchor: '95%'
            }]
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
            allowBlank: true,
            columnWidth: 1,
            emptyText:'TODO',
            name: 'txtStreet',
            anchor: '95%'
            }]
        }
        ]
        }
        ,{
            layout:'column',
            border:false,
            items: [{
        columnWidth:0.5,
        layout: 'form',
        border:false,
        items: [{
            xtype: 'textfield',
            fieldLabel: 'Postcode',
            allowBlank: true,
            columnWidth: 0.5,
            emptyText:'TODO',
            name: 'txtPostcode',
            anchor: '95%'
            }]
        }
        ,{
        columnWidth:0.5,
        layout: 'form',
        border:false,
        items: [{
            xtype: 'textfield',
            fieldLabel: 'City',
            allowBlank: true,
            columnWidth: 0.5,
            emptyText:'TODO',
            name: 'txtCity',
            anchor: '95%'
            }]
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
            fieldLabel: 'Phone',
            allowBlank: true,
            columnWidth: 1,
            emptyText:'TODO',
            name: 'txtPhone',
            anchor: '95%'
            }]
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
            fieldLabel: 'Email',
            allowBlank: true,
            columnWidth: 1,
            emptyText:'TODO',
            name: 'txtEmail',
            anchor: '95%'
            }]
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
            fieldLabel: 'Password',
            allowBlank: true,
            columnWidth: 1,
            emptyText:'TODO',
            name: 'txtPassword',
            anchor: '95%'
            }]
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
            fieldLabel: 'Password Confirm',
            allowBlank: true,
            columnWidth: 1,
            emptyText:'TODO',
            name: 'txtPasswordConfirm',
            anchor: '95%'
            }]
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
    buttons: [{text: 'Cancel'}]
    });

    partnerdata.render(document.body);
})
