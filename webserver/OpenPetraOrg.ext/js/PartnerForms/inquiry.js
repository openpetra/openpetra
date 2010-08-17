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
            columnWidth:1,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Full Name',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtFullName',
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
            fieldLabel: 'Einsatzort',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtEinsatzort',
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
            xtype: 'datefield',
            fieldLabel: 'Einsatzbeginn',
            allowBlank: true,
            Width: 175,
            emptyText:'TODO',
            name: 'dtpEinsatzbeginn',
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
            xtype: 'datefield',
            fieldLabel: 'Einsatzdauer',
            allowBlank: true,
            Width: 175,
            emptyText:'TODO',
            name: 'dtpEinsatzdauer',
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
            fieldLabel: 'Long- Term Plans',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtLong-TermPlans',
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
            xtype: 'checkbox',
            fieldLabel: 'Global Action',
            allowBlank: true,
            name: 'chkGlobalAction',
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
            xtype: 'checkbox',
            fieldLabel: 'Global Service',
            allowBlank: true,
            name: 'chkGlobalService',
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
            xtype: 'checkbox',
            fieldLabel: 'Fsj',
            allowBlank: true,
            name: 'chkFsj',
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
            xtype: 'checkbox',
            fieldLabel: 'Im Ausland',
            allowBlank: true,
            name: 'chkImAusland',
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
            xtype: 'checkbox',
            fieldLabel: 'Im Inland',
            allowBlank: true,
            name: 'chkImInland',
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
            xtype: 'checkbox',
            fieldLabel: 'Als Ersatzzum Zivildienst',
            allowBlank: true,
            name: 'chkAlsErsatzzumZivildienst',
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
            xtype: 'checkbox',
            fieldLabel: 'Afdia',
            allowBlank: true,
            name: 'chkAfdia',
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
            xtype: 'checkbox',
            fieldLabel: 'Sozialversicherungspflichtige Anstellung',
            allowBlank: true,
            name: 'chkSozialversicherungspflichtigeAnstellung',
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
            columnWidth:0.5,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'checkbox',
            fieldLabel: 'Requestmore Informationabout',
            allowBlank: true,
            name: 'chkRequestmoreInformationabout',
            anchor: '95%'
            }
        ]
            }
        ,{
            columnWidth:0.5,
            layout: 'form',
            border:false,
            items: [{
            xtype: 'textfield',
            fieldLabel: 'Text',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtText',
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
            xtype: 'checkbox',
            fieldLabel: 'AmregisteredfortheEFS',
            allowBlank: true,
            name: 'chkAmregisteredfortheEFS',
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
            fieldLabel: 'Whatareyoulookingfor',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtWhatareyoulookingfor',
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
            fieldLabel: 'Vision',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtVision',
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
            fieldLabel: 'Team',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtTeam',
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
            fieldLabel: 'Expectations',
            allowBlank: true,
            Width: -1,
            emptyText:'TODO',
            name: 'txtExpectations',
            anchor: '95%'
            }
        ]
            }
        ]
        }
        ]}

    var inquiry = new Ext.FormPanel({
        frame: true,
        // monitorValid:true,
        title: 'inquiry',
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

    inquiry.render('inquiry');
})
