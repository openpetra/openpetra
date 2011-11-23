//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonControls;
using GNU.Gettext;
using Ict.Common;


namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerEdit_TopPart
    {
        #region Fields

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private String FPartnerClass;

        /// <summary>Used for keeping track of data verification errors</summary>
        private TVerificationResultCollection FVerificationResultCollection;

        // <summary>
        // Delegate for telling the Partner Edit screen that the 'Worker Field...' button has been clicked.
        // </summary>
        // <remarks>The Partner Edit screen acts on that Delegate and opens the corresponding screen.</remarks>
        // TODO private TDelegateMaintainWorkerField FDelegateMaintainWorkerField;
        #endregion

        #region Events

        /// <summary>
        /// This Event is thrown when the 'main data' of a DataTable for a certain
        /// PartnerClass has changed.
        /// </summary>
        public event TPartnerClassMainDataChangedHandler PartnerClassMainDataChanged;

        #endregion

        #region Properties

        /// <summary>UIConnector that the screen uses</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>Holds verification results.</summary>
        public TVerificationResultCollection VerificationResultCollection
        {
            get
            {
                return FVerificationResultCollection;
            }

            set
            {
                FVerificationResultCollection = value;
            }
        }

        #endregion


        #region Public Methods

        /// arrange the panels and controls according to the partner class
        public void InitialiseUserControl()
        {
            #region Show fields according to Partner Class

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    pnlPerson.Visible = true;
                    pnlWorkerField.Visible = true;
                    pnlSpacer.Visible = false;
                    txtPartnerClass.BackColor = System.Drawing.Color.Yellow;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtPersonTitle, PPersonTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    tipMain.SetToolTip(this.txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
#endif
                    txtPersonTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonMiddleName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    this.cmbPersonGender.SelectedValueChanged += new System.EventHandler(this.CmbPersonGender_SelectedValueChanged);

                    break;

                case TPartnerClass.FAMILY:
                    pnlFamily.Visible = true;
                    pnlWorkerField.Visible = true;
                    pnlSpacer.Visible = false;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());
#endif
                    txtFamilyTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.CHURCH:
                    pnlChurch.Visible = true;
                    pnlOther.Visible = true;

                    txtChurchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.ORGANISATION:
                    pnlOrganisation.Visible = true;
                    pnlOther.Visible = true;

                    txtOrganisationName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.UNIT:
                    pnlUnit.Visible = true;
                    pnlOther.Visible = true;

                    txtUnitName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
//                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(this.OnUnitDataColumnChanging);

                    break;

                case TPartnerClass.BANK:
                    pnlBank.Visible = true;
                    pnlOther.Visible = true;

                    txtBranchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                case TPartnerClass.VENUE:
                    pnlVenue.Visible = true;
                    pnlOther.Visible = true;

                    txtVenueName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    break;

                default:
                    MessageBox.Show(String.Format(Catalog.GetString("Unrecognised Partner Class '{0}'!"), FPartnerClass));
                    break;
            }

            #endregion
        }

        /// <summary>
        /// Shows the data that is in FMainDS
        /// </summary>
        public void ShowData()
        {
            FPartnerClass = FMainDS.PPartner[0].PartnerClass.ToString();

            ShowData(FMainDS.PPartner[0]);

// TODO            SetupBtnCreated();
            SetupChkNoSolicitations();
            ApplySecurity();
        }

        /// <summary>
        /// Retrieves data that is in the Controls and puts it into the Tables in FMainDS
        /// </summary>
        public void GetDataFromControls()
        {
            GetDataFromControls(FMainDS.PPartner[0]);

            GetDataFromControlsExtra(FMainDS.PPartner[0]);
        }

        /// <summary>
        /// Initialises Delegate Function to handle click on the "Worker Field..." button.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateMaintainWorkerField(TDelegateMaintainWorkerField ADelegateFunction)
        {
            // TODO FDelegateMaintainWorkerField = ADelegateFunction;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AIncludePartnerClass"></param>
        /// <returns></returns>
        public String PartnerQuickInfo(Boolean AIncludePartnerClass)
        {
            String TmpString;

            TmpString = txtPartnerKey.Text + "   ";

            if (!FMainDS.PPartner[0].IsPartnerShortNameNull())
            {
                TmpString = TmpString + FMainDS.PPartner[0].PartnerShortName;
            }

            if (AIncludePartnerClass)
            {
                TmpString = TmpString + "   [" + FPartnerClass.ToString() + ']';
            }

            return TmpString;
        }

        /// <summary>
        /// Sets the Text of the Worker Field.
        /// </summary>
        /// <param name="AWorkerField">Worker Field.</param>
        public void SetWorkerFieldText(String AWorkerField)
        {
            txtWorkerField.Text = AWorkerField;
        }

        #endregion

        #region Private Methods

        private void ShowDataManual(PPartnerRow ARow)
        {
        	// Ensure that the PartnerKey, which is the PrimaryKey of the p_partner Table, is always read-only
        	// (Read-Only gets set to false in ShowData() for a NEW Partner because it is the PrimaryKey).
        	txtPartnerKey.ReadOnly = true;
        }
        
        private void GetDataFromControlsExtra(PPartnerRow ARow)
        {
            /*
             * Extra logic is needed for FAMILY and PERSON Partners because ARow.NoSolicitations is overwritten in
             * the auto-generated GetDataFromControls Method by the value of chkOtherNoSolicitations.Checked!
             */
            if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY))
            {
                ARow.NoSolicitations = chkFamilyNoSolicitations.Checked;
            }
            else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                ARow.NoSolicitations = chkPersonNoSolicitations.Checked;
            }
        }

        /// <summary>
        /// Sets the background colour of the CheckBox depending on whether it is
        /// Checked or not.
        /// </summary>
        /// <returns>void</returns>
        private void SetupChkNoSolicitations()
        {
            CheckBox ChkNoSolicitations;

            #region Choose CheckBox according to Partner Class

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    ChkNoSolicitations = chkPersonNoSolicitations;
                    break;

                case TPartnerClass.FAMILY:
                    ChkNoSolicitations = chkFamilyNoSolicitations;
                    break;

                case TPartnerClass.CHURCH:
                case TPartnerClass.ORGANISATION:
                case TPartnerClass.UNIT:
                case TPartnerClass.BANK:
                    ChkNoSolicitations = chkOtherNoSolicitations;
                    break;

                default:
                    ChkNoSolicitations = chkOtherNoSolicitations;
                    break;
            }

            #endregion

            if (ChkNoSolicitations.Checked)
            {
                ChkNoSolicitations.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                ChkNoSolicitations.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner. This continues in the switch statments!
                CustomEnablingDisabling.DisableControlGroup(pnlRight);
            }

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlPerson, cmbPersonAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlPerson, chkPersonNoSolicitations);

                        // need to disable all Fields that are DataBound to p_person
                        CustomEnablingDisabling.DisableControlGroup(pnlPerson);

                        cmbPersonAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.FAMILY:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PFamilyTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlFamily, cmbFamilyAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlFamily, chkFamilyNoSolicitations);

                        // need to disable all Fields that are DataBound to p_family
                        CustomEnablingDisabling.DisableControlGroup(pnlFamily);

                        cmbFamilyAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.CHURCH:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PChurchTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_church
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.ORGANISATION:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_organisation
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.UNIT:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_unit
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.BANK:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_bank
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.VENUE:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PVenueTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_venue
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                default:
                    MessageBox.Show(String.Format(Catalog.GetString("Unrecognised Partner Class '{0}'!"), FPartnerClass));
                    break;
            }
        }

        #endregion


        #region Event handlers


        private void OnAnyDataColumnChanging(System.Object sender, EventArgs e)
        {
            TPartnerClassMainDataChangedEventArgs EventFireArgs;

            /* messagebox.show('Column_Changing Event: Column=' + e.Column.ColumnName + */
            /* '; Column content=' + e.Row[e.Column.ColumnName].ToString + */
            /* '; ' + e.ProposedValue.ToString); */
            /* MessageBox.Show('PartnerClass: ' + FPartnerClass.ToString); */
            EventFireArgs = new TPartnerClassMainDataChangedEventArgs();
            EventFireArgs.PartnerClass = FPartnerClass;

            if (FPartnerClass == "PERSON")
            {
                if ((sender == txtPersonTitle)
                    || (sender == txtPersonFirstName)
                    || (sender == txtPersonMiddleName) || (sender == txtPersonFamilyName))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtPersonFamilyName.Text,
                        txtPersonTitle.Text,
                        txtPersonFirstName.Text,
                        txtPersonMiddleName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "FAMILY")
            {
                if ((sender == txtFamilyTitle) || (sender == txtFamilyFirstName)
                    || (sender == txtFamilyFamilyName))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtFamilyFamilyName.Text,
                        txtFamilyTitle.Text,
                        txtFamilyFirstName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "CHURCH")
            {
                if (sender == txtChurchName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtChurchName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                if (sender == txtOrganisationName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOrganisationName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                if (sender == txtUnitName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtUnitName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "BANK")
            {
                if (sender == txtBranchName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtBranchName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "VENUE")
            {
                if (sender == txtVenueName)
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtVenueName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
        }

        private void CmbPersonGender_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            if (cmbPersonGender.GetSelectedString() == "Female")
            {
                cmbPersonAddresseeTypeCode.SetSelectedString(SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcFEMALE));
            }
            else if (cmbPersonGender.GetSelectedString() == "Male")
            {
                cmbPersonAddresseeTypeCode.SetSelectedString(SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcMALE));
            }

//            /*
//             * Also assign the value directly to the databound data field!
//             * Strangely enough, this is necessary for the case if the user doesn't TAB out
//             * of cmbPersonGender, but uses the mouse to select anything else on the screen
//             * *except* cmbAddresseeType!
//             */
//            FMainDS.PPartner[0].AddresseeTypeCode = cmbAddresseeType.SelectedItem.ToString();
        }

        private void UpdateNoSolicitationsColouring(System.Object sender, System.EventArgs e)
        {
            SetupChkNoSolicitations();
        }

        #endregion


        #region Custom Events

        private void OnPartnerClassMainDataChanged(TPartnerClassMainDataChangedEventArgs e)
        {
            /* MessageBox.Show('OnPartnerClassMainDataChanged. e.PartnerClass: ' + e.PartnerClass.ToString); */
            if (PartnerClassMainDataChanged != null)
            {
                PartnerClassMainDataChanged(this, e);
            }
        }

        #endregion


        #region Actions

        private void MaintainWorkerField(System.Object sender, System.EventArgs e)
        {
            //this may be temporary used to have an access point for this dialog
            TFrmPersonnelStaffData staffDataForm = new TFrmPersonnelStaffData(FPetraUtilsObject.GetForm());

            staffDataForm.PartnerKey = ((TFrmPartnerEdit)ParentForm).PartnerKey;
            staffDataForm.Show();

/*
 * #if TODO
 *          if (this.FDelegateMaintainWorkerField != null)
 *          {
 *              try
 *              {
 *                  this.FDelegateMaintainWorkerField();
 *              }
 *              finally
 *              {
 *                  throw new EVerificationMissing(Catalog.GetString("this.FDelegateGetPartnerShortName could not be called!"));
 *              }
 *          }
 * #endif
 */
        }

        #endregion
    }
}