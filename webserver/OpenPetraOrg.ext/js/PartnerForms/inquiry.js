var inquiry = null;
inquiryForm = Ext.extend(Ext.FormPanel, {
    pnlContentFORMCAPTION:'Application form',
    txtFullNameLABEL:'Full Name',
    txtFullNameHELP:'',
    txtEinsatzortLABEL:'Einsatzort',
    txtEinsatzortHELP:'',
    dtpEinsatzbeginnLABEL:'Einsatzbeginn',
    dtpEinsatzbeginnHELP:'',
    dtpEinsatzdauerLABEL:'Einsatzdauer',
    dtpEinsatzdauerHELP:'',
    txtLongTermPlansLABEL:'Long Term Plans',
    txtLongTermPlansHELP:'',
    chkGlobalActionLABEL:'Global Action',
    chkGlobalActionHELP:'',
    chkGlobalServiceLABEL:'Global Service',
    chkGlobalServiceHELP:'',
    chkFsjLABEL:'Fsj',
    chkFsjHELP:'',
    chkImAuslandLABEL:'Im Ausland',
    chkImAuslandHELP:'',
    chkImInlandLABEL:'Im Inland',
    chkImInlandHELP:'',
    chkAlsErsatzzumZivildienstLABEL:'Als Ersatzzum Zivildienst',
    chkAlsErsatzzumZivildienstHELP:'',
    chkAfdiaLABEL:'Afdia',
    chkAfdiaHELP:'',
    chkSozialversicherungspflichtigeAnstellungLABEL:'Sozialversicherungspflichtige Anstellung',
    chkSozialversicherungspflichtigeAnstellungHELP:'',
    chkRequestmoreInformationaboutLABEL:'Requestmore Informationabout',
    chkRequestmoreInformationaboutHELP:'',
    txtTextLABEL:'Text',
    txtTextHELP:'',
    chkAmregisteredfortheEFSLABEL:'AmregisteredfortheEFS',
    chkAmregisteredfortheEFSHELP:'',
    txtWhatareyoulookingforLABEL:'Whatareyoulookingfor',
    txtWhatareyoulookingforHELP:'',
    txtVisionLABEL:'Vision',
    txtVisionHELP:'',
    txtTeamLABEL:'Team',
    txtTeamHELP:'',
    txtExpectationsLABEL:'Expectations',
    txtExpectationsHELP:'',
    btnSaveLABEL:'Save',
    btnSaveHELP:'',
    btnCancelLABEL:'Cancel',
    btnCancelHELP:'',
    strEmpty:'',
    initComponent : function(config) {
        Ext.apply(this, {
            frame: true,
            // monitorValid:true,
            title: this.pnlContentFORMCAPTION,
            bodyStyle: 'padding:5px',
            width: 650,
            labelWidth: 140,
            items: [{
                bodyStyle: {
                margin: '0px 0px 15px 0px'
            },
            items: [{
    layout:'column',
    border:false,
    items: [{
    columnWidth:1,
    layout: 'form',
    border:false,
    items: [{
    xtype: 'textfield',
    fieldLabel: this.txtFullNameLABEL,
    allowBlank: false,
    emptyText: this.txtFullNameHELP,
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
    fieldLabel: this.txtEinsatzortLABEL,
    allowBlank: false,
    emptyText: this.txtEinsatzortHELP,
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
    fieldLabel: this.dtpEinsatzbeginnLABEL,
    allowBlank: false,
    format: 'd.m.Y',
    boxMaxWidth: 175,
    emptyText: this.dtpEinsatzbeginnHELP,
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
    fieldLabel: this.dtpEinsatzdauerLABEL,
    allowBlank: false,
    format: 'd.m.Y',
    boxMaxWidth: 175,
    emptyText: this.dtpEinsatzdauerHELP,
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
    fieldLabel: this.txtLongTermPlansLABEL,
    allowBlank: false,
    emptyText: this.txtLongTermPlansHELP,
    name: 'txtLongTermPlans',
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
    fieldLabel: this.chkGlobalActionLABEL,
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
    fieldLabel: this.chkGlobalServiceLABEL,
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
    fieldLabel: this.chkFsjLABEL,
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
    fieldLabel: this.chkImAuslandLABEL,
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
    fieldLabel: this.chkImInlandLABEL,
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
    fieldLabel: this.chkAlsErsatzzumZivildienstLABEL,
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
    fieldLabel: this.chkAfdiaLABEL,
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
    fieldLabel: this.chkSozialversicherungspflichtigeAnstellungLABEL,
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
    fieldLabel: this.chkRequestmoreInformationaboutLABEL,
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
    fieldLabel: this.txtTextLABEL,
    allowBlank: false,
    emptyText: this.txtTextHELP,
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
    fieldLabel: this.chkAmregisteredfortheEFSLABEL,
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
    fieldLabel: this.txtWhatareyoulookingforLABEL,
    allowBlank: false,
    emptyText: this.txtWhatareyoulookingforHELP,
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
    fieldLabel: this.txtVisionLABEL,
    allowBlank: false,
    emptyText: this.txtVisionHELP,
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
    fieldLabel: this.txtTeamLABEL,
    allowBlank: false,
    emptyText: this.txtTeamHELP,
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
    fieldLabel: this.txtExpectationsLABEL,
    allowBlank: false,
    emptyText: this.txtExpectationsHELP,
    name: 'txtExpectations',
    anchor: '97.5%'
    }
]
    }
]
}
]
            }],
        buttons: [{
text: this.btnSaveLABEL
}
,{
text: this.btnCancelLABEL
}

        ]
        });
        inquiryForm.superclass.initComponent.apply(this, arguments);
    }
});
