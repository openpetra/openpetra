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
            allowBlank: false,
            emptyText:'',
            name: 'txtFullName',
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
            fieldLabel: 'Einsatzort',
            allowBlank: false,
            emptyText:'',
            name: 'txtEinsatzort',
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
            xtype: 'datefield',
            fieldLabel: 'Einsatzbeginn',
            allowBlank: false,
            format: 'd.m.Y',
            boxMaxWidth: 175,
            emptyText:'',
            name: 'dtpEinsatzbeginn',
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
            xtype: 'datefield',
            fieldLabel: 'Einsatzdauer',
            allowBlank: false,
            format: 'd.m.Y',
            boxMaxWidth: 175,
            emptyText:'',
            name: 'dtpEinsatzdauer',
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
            fieldLabel: 'Long- Term Plans',
            allowBlank: false,
            emptyText:'',
            name: 'txtLong-TermPlans',
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
            xtype: 'checkbox',
            fieldLabel: 'Global Action',
            allowBlank: true,
            name: 'chkGlobalAction',
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
            xtype: 'checkbox',
            fieldLabel: 'Global Service',
            allowBlank: true,
            name: 'chkGlobalService',
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
            xtype: 'checkbox',
            fieldLabel: 'Fsj',
            allowBlank: true,
            name: 'chkFsj',
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
            xtype: 'checkbox',
            fieldLabel: 'Im Ausland',
            allowBlank: true,
            name: 'chkImAusland',
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
            xtype: 'checkbox',
            fieldLabel: 'Im Inland',
            allowBlank: true,
            name: 'chkImInland',
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
            xtype: 'checkbox',
            fieldLabel: 'Als Ersatzzum Zivildienst',
            allowBlank: true,
            name: 'chkAlsErsatzzumZivildienst',
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
            xtype: 'checkbox',
            fieldLabel: 'Afdia',
            allowBlank: true,
            name: 'chkAfdia',
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
            xtype: 'checkbox',
            fieldLabel: 'Sozialversicherungspflichtige Anstellung',
            allowBlank: true,
            name: 'chkSozialversicherungspflichtigeAnstellung',
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
            allowBlank: false,
            emptyText:'',
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
            fieldLabel: 'Whatareyoulookingfor',
            allowBlank: false,
            emptyText:'',
            name: 'txtWhatareyoulookingfor',
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
            fieldLabel: 'Vision',
            allowBlank: false,
            emptyText:'',
            name: 'txtVision',
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
            fieldLabel: 'Team',
            allowBlank: false,
            emptyText:'',
            name: 'txtTeam',
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
            fieldLabel: 'Expectations',
            allowBlank: false,
            emptyText:'',
            name: 'txtExpectations',
            anchor: '97.5%'
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
