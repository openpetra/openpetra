var partnerdata = null;

Ext.onReady(function() {
    /// validation method for checking a password has been entered twice correctly, and is at least 5 characters
    Ext.apply(Ext.form.VTypes, {
       password: function(value, field)
       {
          var f;

          if ((f = partnerdata.getForm().findField(field.otherPasswordField)))
          {
             this.passwordText = partnerdata.strErrorPasswordNoMatch;
             if (f.getValue().length > 0 && value != f.getValue())
             {
                return false;
             }
          }

          this.passwordText = partnerdata.strErrorPasswordLength;

          var hasLength = (value.length >= 5);

          return (hasLength);
       },

       passwordText: '',
    });
});

Ext.onReady(function() {
    /// validation method for checking if a checkbox has been ticked
    Ext.form.Checkbox.prototype.validate = function()
    {
        if (this.vtype == "forcetick")
        {
            if (!this.checked)
            {
                Ext.form.Field.prototype.markInvalid.call(this, partnerdata.strErrorCheckboxRequired);
                return false;
            }
            else
            {
                Ext.form.Field.prototype.clearInvalid.call(this);
            }
        }

        return true;
    };
});

function XmlExtractJSONResponse(response)
{
    var xml = response.responseXML;
    var stringDataNode = xml.getElementsByTagName('string')[0];
    if(stringDataNode){
        jsonString = stringDataNode.firstChild.data;
        jsonData = Ext.util.JSON.decode(jsonString);
        return jsonData;
    }
}

partnerdataForm = Ext.extend(Ext.FormPanel, {
    pnlContentFORMCAPTION:'Application form',
    cmpRegistrationLABEL:'Registration',
    lblRegistration1LABEL:'I hereby register for the event',
    lblRegistration1HELP:'',
    cmbEventLABEL:'Event',
    cmbEventHELP:'Select an event',
    cmbEventOPTION0:'March 2010',
    cmbEventOPTION1:'June 2010',
    cmbEventOPTION2:'October 2010',
    lblRegistration2LABEL:'and allow my address to be published',
    lblRegistration2HELP:'',
    lblRegistration3LABEL:'to other participants for setting up shared travel.',
    lblRegistration3HELP:'',
    chkAgreementLABEL:'I have seen the <a href="test.html">conditions of xyz for events</a> and will later print and sign the PDF',
    chkAgreementHELP:'',
    strErrorCheckboxRequired:'You have to tick this and sign the document later',
    txtFirstNameLABEL:'First name',
    txtFirstNameHELP:'Please enter first name',
    txtLastNameLABEL:'Last Name',
    txtLastNameHELP:'',
    txtStreetLABEL:'Street',
    txtStreetHELP:'Streetname and house number',
    txtPostcodeLABEL:'Postcode and City',
    txtPostcodeHELP:'Postcode',
    txtCityLABEL:'City',
    txtCityHELP:'City',
    txtPhoneLABEL:'Phone',
    txtPhoneHELP:'',
    txtEmailLABEL:'Email',
    txtEmailHELP:'',
    txtPasswordLABEL:'Password',
    txtPasswordHELP:'',
    txtPasswordConfirmLABEL:'Password Confirm',
    txtPasswordConfirmHELP:'',
    strErrorPasswordLength:'Passwords must be at least 5 characters',
    strErrorPasswordNoMatch:'Confirmation does not match your first password entry.',
    rgrEmploymentStatusLABEL:'Employment Status',
    rbtPupilLABEL:'Pupil',
    rbtPupilHELP:'',
    rgrEmploymentStatusLABEL:'Employment Status',
    rbtStudentLABEL:'Student',
    rbtStudentHELP:'',
    rbtUnemployedLABEL:'Unemployed',
    rbtUnemployedHELP:'',
    rbtEmployedLABEL:'Employed',
    rbtEmployedHELP:'',
    txtProfessionLABEL:'Degree/Profession',
    txtProfessionHELP:'Enter your school degree or profession',
    grpBankDetailsLABEL:'Bank Details',
    cmpDirectDebitPermissionLABEL:'Direct Debit Permission',
    lblReasonBankDetails1LABEL:'Hereby I give permission to xyz to do a direct debit of',
    lblReasonBankDetails1HELP:'',
    txtConferenceFeeLABEL:'Conference Fee',
    txtConferenceFeeHELP:'',
    lblReasonBankDetails2LABEL:'Euro from this bank account:',
    lblReasonBankDetails2HELP:'',
    txtKontoinhaberLABEL:'Kontoinhaber',
    txtKontoinhaberHELP:'',
    txtKontonummerLABEL:'Kontonummer',
    txtKontonummerHELP:'',
    txtIbanLABEL:'Iban',
    txtIbanHELP:'',
    txtKreditinstitutLABEL:'Kreditinstitut',
    txtKreditinstitutHELP:'',
    txtBankOrtLABEL:'Bank Ort',
    txtBankOrtHELP:'',
    lblBankDetailsSignatureLABEL:'After submitting the form, you will print a PDF, and you (and for minors also their parents) have to sign this',
    lblBankDetailsSignatureHELP:'',
    btnSaveLABEL:'Save',
    btnSaveHELP:'',
    btnSaveVALIDATIONERRORTITLE:'Input Error',
    btnSaveVALIDATIONERRORMESSAGE:'Please check the flagged controls!',
    btnSaveSENDINGDATATITLE:'Please wait',
    btnSaveSENDINGDATAMESSAGE:'Data are being sent to the server',
    btnSaveREQUESTSUCCESSTITLE:'Success',
    btnSaveREQUESTSUCCESSMESSAGE:'Your application has been successful. You will receive an email with a PDF file. Please print the PDF file, sign it, and send it to us via post',
    btnSaveREQUESTFAILURETITLE:'Failure',
    btnSaveREQUESTFAILUREMESSAGE:'Something did not work on the server.',
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
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'compositefield',
                                                fieldLabel: this.cmpRegistrationLABEL,
                                                hideLabel: true,
                                                items: [{
                                                            xtype: 'displayfield',
                                                            hideLabel: true,
                                                            value: this.lblRegistration1LABEL
                                                        },
                                                        new Ext.form.ComboBox({
                                                            fieldLabel: this.cmbEventLABEL,
                                                            hiddenName:'cmbEvent',
                                                            store: new Ext.data.ArrayStore({
                                                                fields: ['value', 'display'],
                                                                data : [[this.cmbEventOPTION0, this.cmbEventOPTION0], [this.cmbEventOPTION1, this.cmbEventOPTION1], [this.cmbEventOPTION2, this.cmbEventOPTION2]]
                                                            }),
                                                            valueField:'value',
                                                            displayField:'display',
                                                            typeAhead: true,
                                                            mode: 'local',
                                                            triggerAction: 'all',
                                                            emptyText: this.cmbEventHELP,
                                                            allowBlank: false,
                                                            selectOnFocus:true,
                                                            width:190
                                                        }),
                                                        {
                                                            xtype: 'displayfield',
                                                            hideLabel: true,
                                                            value: this.lblRegistration2LABEL
                                                        }
                                                        ]
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'displayfield',
                                                hideLabel: true,
                                                value: this.lblRegistration3LABEL
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'checkbox',
                                                vtype: 'forcetick',
                                                boxLabel: this.chkAgreementLABEL,
                                                hideLabel: true,
                                                fieldLabel: this.strEmpty,
                                                name: 'chkAgreement',
                                                anchor: '96.25%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:0.5,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtFirstNameLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtFirstNameHELP,
                                                name: 'txtFirstName',
                                                anchor: '95%'
                                            }
                                            ]
                                },
                                {
                                    columnWidth:0.5,
                                    labelWidth: 80,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtLastNameLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtLastNameHELP,
                                                name: 'txtLastName',
                                                anchor: '95%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtStreetLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtStreetHELP,
                                                name: 'txtStreet',
                                                anchor: '97.5%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:.4,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'numberfield',
                                                fieldLabel: this.txtPostcodeLABEL,
                                                allowBlank: false,
                                                minLength: 5,
                                                maxLength: 5,
                                                emptyText: this.txtPostcodeHELP,
                                                name: 'txtPostcode',
                                                anchor: '94%'
                                            }
                                            ]
                                },
                                {
                                    columnWidth:.6,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtCityLABEL,
                                                hideLabel: true,
                                                allowBlank: false,
                                                emptyText: this.txtCityHELP,
                                                name: 'txtCity',
                                                anchor: '96.25%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:.6,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtPhoneLABEL,
                                                allowBlank: true,
                                                emptyText: this.txtPhoneHELP,
                                                name: 'txtPhone',
                                                anchor: '94%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:.6,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                vtype: 'email',
                                                fieldLabel: this.txtEmailLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtEmailHELP,
                                                name: 'txtEmail',
                                                anchor: '94%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:.6,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                inputType: 'password',
                                                fieldLabel: this.txtPasswordLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtPasswordHELP,
                                                name: 'txtPassword',
                                                anchor: '94%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
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
                                                fieldLabel: this.txtPasswordConfirmLABEL,
                                                allowBlank: false,
                                                otherPasswordField: 'txtPassword',
                                                emptyText: this.txtPasswordConfirmHELP,
                                                name: 'txtPasswordConfirm',
                                                anchor: '94%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    xtype: 'fieldset',
                                    columnWidth: 1.0,
                                    border:false,
                                    items: [{
                                                xtype: 'radio',
                                                boxLabel: this.rbtPupilLABEL,
                                                fieldLabel: this.rgrEmploymentStatusLABEL,
                                                name: 'rgrEmploymentStatus',
                                                anchor: '97.5%'
                                            },
                                            {
                                                xtype: 'radio',
                                                checked: true,
                                                boxLabel: this.rbtStudentLABEL,
                                                fieldLabel: this.strEmpty,
                                                name: 'rgrEmploymentStatus',
                                                anchor: '97.5%'
                                            },
                                            {
                                                xtype: 'radio',
                                                boxLabel: this.rbtUnemployedLABEL,
                                                fieldLabel: this.strEmpty,
                                                name: 'rgrEmploymentStatus',
                                                anchor: '97.5%'
                                            },
                                            {
                                                xtype: 'radio',
                                                boxLabel: this.rbtEmployedLABEL,
                                                fieldLabel: this.strEmpty,
                                                name: 'rgrEmploymentStatus',
                                                anchor: '97.5%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'textfield',
                                                fieldLabel: this.txtProfessionLABEL,
                                                allowBlank: false,
                                                emptyText: this.txtProfessionHELP,
                                                name: 'txtProfession',
                                                anchor: '97.5%'
                                            }
                                            ]
                                }
                                ]
                    },
                    {
                        layout:'column',
                        border:false,
                        items: [{
                                    columnWidth:1,
                                    layout: 'form',
                                    border:false,
                                    items: [{
                                                xtype: 'fieldset',
                                                title: this.grpBankDetailsLABEL,
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
                                                                                    fieldLabel: this.cmpDirectDebitPermissionLABEL,
                                                                                    hideLabel: true,
                                                                                    items: [{
                                                                                                xtype: 'displayfield',
                                                                                                hideLabel: true,
                                                                                                value: this.lblReasonBankDetails1LABEL
                                                                                            },
                                                                                            {
                                                                                                xtype: 'numberfield',
                                                                                                fieldLabel: this.txtConferenceFeeLABEL,
                                                                                                allowBlank: false,
                                                                                                emptyText: this.txtConferenceFeeHELP,
                                                                                                name: 'txtConferenceFee',
                                                                                                anchor: '95%'
                                                                                            },
                                                                                            {
                                                                                                xtype: 'displayfield',
                                                                                                hideLabel: true,
                                                                                                value: this.lblReasonBankDetails2LABEL
                                                                                            }
                                                                                            ]
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'textfield',
                                                                                    fieldLabel: this.txtKontoinhaberLABEL,
                                                                                    allowBlank: false,
                                                                                    emptyText: this.txtKontoinhaberHELP,
                                                                                    name: 'txtKontoinhaber',
                                                                                    anchor: '97.5%'
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'textfield',
                                                                                    fieldLabel: this.txtKontonummerLABEL,
                                                                                    allowBlank: false,
                                                                                    emptyText: this.txtKontonummerHELP,
                                                                                    name: 'txtKontonummer',
                                                                                    anchor: '97.5%'
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'textfield',
                                                                                    fieldLabel: this.txtIbanLABEL,
                                                                                    allowBlank: false,
                                                                                    emptyText: this.txtIbanHELP,
                                                                                    name: 'txtIban',
                                                                                    anchor: '97.5%'
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'textfield',
                                                                                    fieldLabel: this.txtKreditinstitutLABEL,
                                                                                    allowBlank: false,
                                                                                    emptyText: this.txtKreditinstitutHELP,
                                                                                    name: 'txtKreditinstitut',
                                                                                    anchor: '97.5%'
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'textfield',
                                                                                    fieldLabel: this.txtBankOrtLABEL,
                                                                                    allowBlank: false,
                                                                                    emptyText: this.txtBankOrtHELP,
                                                                                    name: 'txtBankOrt',
                                                                                    anchor: '97.5%'
                                                                                }
                                                                                ]
                                                                    }
                                                                    ]
                                                        },
                                                        {
                                                            layout:'column',
                                                            border:false,
                                                            items: [{
                                                                        columnWidth:1,
                                                                        layout: 'form',
                                                                        border:false,
                                                                        items: [{
                                                                                    xtype: 'displayfield',
                                                                                    hideLabel: true,
                                                                                    value: this.lblBankDetailsSignatureLABEL
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
                    ],
            buttons: [{
                          text: this.btnSaveLABEL
                          ,formBind: true,
                          handler: function () {
                              // to display missing/invalid fields
                              if (!partnerdata.getForm().isValid())
                              {
                                  Ext.Msg.show({
                                      title: partnerdata.btnSaveVALIDATIONERRORTITLE,
                                      msg: partnerdata.btnSaveVALIDATIONERRORMESSAGE,
                                      modal: true,
                                      icon: Ext.Msg.ERROR,
                                      buttons: Ext.Msg.OK
                                  });
                              }
                              else
                              {
                                  Ext.MessageBox.wait(partnerdata.btnSaveSENDINGDATAMESSAGE, partnerdata.btnSaveSENDINGDATATITLE);

                                  Ext.Ajax.request({
                                      url: '/server.asmx/DataImportFromForm',
                                      params:{
                                          AFormID: 'EFSAnmeldung',
                                          AJSONFormData: Ext.encode(partnerdata.getForm().getValues())
                                      },
                                      success: function (response) {
                                          jsonData = XmlExtractJSONResponse(response);
                                          if (jsonData.failure == true)
                                          {
                                                Ext.Msg.show({
                                                    title: partnerdata.btnSaveREQUESTFAILURETITLE,
                                                    msg: partnerdata.btnSaveREQUESTFAILUREMESSAGE + "<br/>" + jsonData.data.result,
                                                    modal: true,
                                                    icon: Ext.Msg.ERROR,
                                                    buttons: Ext.Msg.OK
                                                });
                                          }
                                          else
                                          {
                                              Ext.Msg.show({
                                                  title: partnerdata.btnSaveREQUESTSUCCESSTITLE,
                                                  msg: partnerdata.btnSaveREQUESTSUCCESSMESSAGE,
                                                  modal: true,
                                                  icon: Ext.Msg.INFO,
                                                  buttons: Ext.Msg.OK
                                              });
                                          }
                                      },
                                      failure: function () {
                                          Ext.Msg.show({
                                              title: partnerdata.btnSaveREQUESTFAILURETITLE,
                                              msg: partnerdata.btnSaveREQUESTFAILUREMESSAGE,
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
                          text: this.btnCancelLABEL
                      }
                      ]
        });
        partnerdataForm.superclass.initComponent.apply(this, arguments);
    }
});
