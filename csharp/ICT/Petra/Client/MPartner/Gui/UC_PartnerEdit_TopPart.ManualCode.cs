//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MPartner.Verification;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Shared.MPartner.Partner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>Delegate declaration</summary>
    public delegate void TDelegateMaintainGiftDestination();

    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TPartnerClassMainDataChangedEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String PartnerClass;
    }

    /// <summary>Event handler declaration</summary>
    public delegate void TPartnerClassMainDataChangedHandler(System.Object Sender, TPartnerClassMainDataChangedEventArgs e);


    public partial class TUC_PartnerEdit_TopPart
    {
        #region Fields

        private System.Windows.Forms.ToolTip FTipMain;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private String FPartnerClass;

        // <summary>
        // Delegate for telling the Partner Edit screen that the 'Gift Destination' button has been clicked.
        // </summary>
        // <remarks>The Partner Edit screen acts on that Delegate and opens the corresponding screen.</remarks>
        private TDelegateMaintainGiftDestination FDelegateMaintainGiftDestination;

        private bool FIgnorePartnerStatusChange = true;

        /// <summary>See call to Method <see cref="TbtnCreatedHelper.AddModifiedCreatedButtonToContainerControl"/>.</summary>
        private TbtnCreated btnCreatedModifiedOverall = new TbtnCreated();

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
                return FPetraUtilsObject.VerificationResultCollection;
            }

            set
            {
                FPetraUtilsObject.VerificationResultCollection = value;
            }
        }

        #endregion


        #region Public Methods

        /// arrange the panels and controls according to the partner class
        public void InitialiseUserControl()
        {
            FIgnorePartnerStatusChange = false;

            // Set up ToolTip
            this.components = new System.ComponentModel.Container();
            FTipMain = new System.Windows.Forms.ToolTip(this.components);
            FTipMain.AutoPopDelay = 4000;
            FTipMain.InitialDelay = 500;
            FTipMain.ReshowDelay = 100;

            BuildValidationControlsDict();

            // Ensure that the 'Gift Destination' Panel (and thus the Button on it) always comes last in the Tab Order
            // (well, that is last before the 'Created/Modified' Button, which has got TabIndex 999).
            pnlGiftDestination.TabIndex = 998;

            txtGiftDestination.Top -= 2;

            #region Show fields according to Partner Class

            txtPartnerKey.PartnerClass = FPartnerClass;

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    pnlPerson.Visible = true;
                    pnlGiftDestination.Visible = true;
                    pnlPerson2ndLine.Visible = true;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
                    FTipMain.SetToolTip(this.txtPersonTitle, PPersonTable.GetTitleHelp());
                    FTipMain.SetToolTip(this.txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    FTipMain.SetToolTip(this.txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    FTipMain.SetToolTip(this.txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());

                    txtPersonTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonMiddleName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtPersonFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    this.cmbPersonGender.SelectedValueChanged += new System.EventHandler(this.CmbPersonGender_SelectedValueChanged);

                    txtPartnerClass.BackColor = TCommonControlsHelper.PartnerClassPERSONColour;

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtPersonTitle.SelectionStart = 0;

                    break;

                case TPartnerClass.FAMILY:
                    pnlFamily.Visible = true;
                    pnlGiftDestination.Visible = true;
                    pnlFamily2ndLine.Visible = true;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
                    FTipMain.SetToolTip(this.txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    FTipMain.SetToolTip(this.txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    FTipMain.SetToolTip(this.txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());

                    txtFamilyTitle.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFirstName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    txtFamilyFamilyName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtFamilyTitle.SelectionStart = 0;

                    break;

                case TPartnerClass.CHURCH:
                    pnlChurch.Visible = true;
                    pnlOther2ndLine.Visible = true;

                    txtChurchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtChurchName.SelectionStart = 0;

                    break;

                case TPartnerClass.ORGANISATION:
                    pnlOrganisation.Visible = true;
                    pnlOther2ndLine.Visible = true;

                    txtOrganisationName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtOrganisationName.SelectionStart = 0;

                    break;

                case TPartnerClass.UNIT:
                    pnlUnit.Visible = true;
                    pnlOther2ndLine.Visible = true;

                    txtUnitName.TextChanged += new EventHandler(OnAnyDataColumnChanging);
                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(OnUnitDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtUnitName.SelectionStart = 0;

                    break;

                case TPartnerClass.BANK:
                    pnlBank.Visible = true;
                    pnlOther2ndLine.Visible = true;

                    txtBranchName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtBranchName.SelectionStart = 0;

                    break;

                case TPartnerClass.VENUE:
                    pnlVenue.Visible = true;
                    pnlOther2ndLine.Visible = true;

                    txtVenueName.TextChanged += new EventHandler(OnAnyDataColumnChanging);

                    // Ensure that the Text in the first TextBox isn't all selected when the Form is brought up
                    txtVenueName.SelectionStart = 0;

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

            SetupBtnCreated();
            SetupChkNoSolicitations();
            ApplySecurity();
        }

        /// <summary>
        /// Updates the 'Status Update' Date TextBox to reflect today's date if the 'Partner Status' was just changed
        /// </summary>
        public void UpdateStatusUpdatedDate()
        {
            if (FMainDS.PPartner[0].IsStatusChangeNull())
            {
                dtpStatusUpdated.Date = null;
            }
            else
            {
                dtpStatusUpdated.Date = FMainDS.PPartner[0].StatusChange;
            }
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
        /// Initialises Delegate Function to handle click on the "Gift Destination" button.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateMaintainGiftDestination(TDelegateMaintainGiftDestination ADelegateFunction)
        {
            FDelegateMaintainGiftDestination = ADelegateFunction;
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
        /// Sets the Text of the Gift Destination.
        /// </summary>
        /// <param name="AGiftDestination">Gift Destination.</param>
        public void SetGiftDestinationText(String AGiftDestination)
        {
            txtGiftDestination.Text = AGiftDestination;
            FPetraUtilsObject.SetStatusBarText(txtGiftDestination, AGiftDestination);
        }

        /// <summary>
        /// Sets the Partner's PartnerStatus to 'ACTIVE'.
        /// </summary>
        public void SetPartnerStatusToActive()
        {
            cmbPartnerStatus.SetSelectedString(SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE));
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
                ARow.AddresseeTypeCode = cmbFamilyAddresseeTypeCode.GetSelectedString();
            }
            else if (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON))
            {
                ARow.NoSolicitations = chkPersonNoSolicitations.Checked;
                ARow.AddresseeTypeCode = cmbPersonAddresseeTypeCode.GetSelectedString();
            }
        }

        /// <summary>
        /// Sets up the Button that holds the Created and Modified information.
        /// Since there are always two Tables involved in the data that is displayed in
        /// this UserControl, DateCreated and CreatedBy are taken from the table where
        /// DateCreated is earlier, and DateModified and ModifiedBy are taken from the
        /// table where DateModified is later.
        /// </summary>
        private void SetupBtnCreated()
        {
            DateTime DateCreatedPPartner = DateTime.Now;
            DateTime DateModifiedPPartner = DateTime.Now;
            DateTime DateCreatedPartnerClassDependent = DateTime.Now;
            DateTime DateModifiedPartnerClassDependent = DateTime.Now;
            String CreatedByPartnerClassDependent = "";
            String ModifiedByPartnerClassDependent = "";

            // Manually add button for the modified/created information that was present in Petra 2.x's Partner Edit's 
            // 'Collapsible Part', but was missing from OpenPetra's 'Top Part' because the WinForms Generator doesn't have 
            // a built-in support for the creation of those buttons yet (Bug #1782).
            TbtnCreatedHelper.AddModifiedCreatedButtonToContainerControl(ref btnCreatedModifiedOverall, grpCollapsible, 
                ACustomYLocation: 0);
            FPetraUtilsObject.SetStatusBarText(btnCreatedModifiedOverall, ApplWideResourcestrings.StrBtnCreatedUpdatedStatusBarText);

            #region Determine DateCreated, DateModified, CreatedBy and ModifiedBy according to PartnerClass

            switch (SharedTypes.PartnerClassStringToEnum(FMainDS.PPartner[0].PartnerClass))
            {
                case TPartnerClass.PERSON:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PPerson.ColumnDateCreated, FMainDS.PPerson[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PPerson.ColumnDateModified, FMainDS.PPerson[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PPerson.ColumnCreatedBy, FMainDS.PPerson[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PPerson.ColumnModifiedBy, FMainDS.PPerson[0]);
                    break;

                case TPartnerClass.FAMILY:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PFamily.ColumnDateCreated, FMainDS.PFamily[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PFamily.ColumnDateModified, FMainDS.PFamily[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PFamily.ColumnCreatedBy, FMainDS.PFamily[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PFamily.ColumnModifiedBy, FMainDS.PFamily[0]);
                    break;

                case TPartnerClass.CHURCH:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PChurch.ColumnDateCreated, FMainDS.PChurch[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PChurch.ColumnDateModified, FMainDS.PChurch[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PChurch.ColumnCreatedBy, FMainDS.PChurch[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PChurch.ColumnModifiedBy, FMainDS.PChurch[0]);
                    break;

                case TPartnerClass.ORGANISATION:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.POrganisation.ColumnDateCreated,
                    FMainDS.POrganisation[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.POrganisation.ColumnDateModified,
                    FMainDS.POrganisation[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.POrganisation.ColumnCreatedBy,
                    FMainDS.POrganisation[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.POrganisation.ColumnModifiedBy,
                    FMainDS.POrganisation[0]);
                    break;

                case TPartnerClass.BANK:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PBank.ColumnDateCreated, FMainDS.PBank[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PBank.ColumnDateModified, FMainDS.PBank[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PBank.ColumnCreatedBy, FMainDS.PBank[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PBank.ColumnModifiedBy, FMainDS.PBank[0]);
                    break;

                case TPartnerClass.UNIT:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PUnit.ColumnDateCreated, FMainDS.PUnit[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PUnit.ColumnDateModified, FMainDS.PUnit[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PUnit.ColumnCreatedBy, FMainDS.PUnit[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PUnit.ColumnModifiedBy, FMainDS.PUnit[0]);
                    break;

                case TPartnerClass.VENUE:
                    DateCreatedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PVenue.ColumnDateCreated, FMainDS.PVenue[0]);
                    DateModifiedPartnerClassDependent = TSaveConvert.DateColumnToDate(FMainDS.PVenue.ColumnDateModified, FMainDS.PVenue[0]);
                    CreatedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PVenue.ColumnCreatedBy, FMainDS.PVenue[0]);
                    ModifiedByPartnerClassDependent = TSaveConvert.StringColumnToString(FMainDS.PVenue.ColumnModifiedBy, FMainDS.PVenue[0]);
                    break;
            }

            #endregion

            /*
             * Decide on which DateCreated and CreatedBy to display:
             * If PPartner DateCreated is earlier, take them from PPartner, otherwise from
             * the Table according to PartnerClass.
             */
            DateCreatedPPartner = TSaveConvert.DateColumnToDate(FMainDS.PPartner.ColumnDateCreated, FMainDS.PPartner[0]);

            if (DateCreatedPartnerClassDependent != DateTime.MinValue)
            {
                if ((DateCreatedPPartner < DateCreatedPartnerClassDependent) && (DateCreatedPPartner != DateTime.MinValue))
                {
                    btnCreatedModifiedOverall.DateCreated = DateCreatedPPartner;
                    btnCreatedModifiedOverall.CreatedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnCreatedBy, FMainDS.PPartner[0]);
                }
                else
                {
                    btnCreatedModifiedOverall.DateCreated = DateCreatedPartnerClassDependent;
                    btnCreatedModifiedOverall.CreatedBy = CreatedByPartnerClassDependent;
                }
            }
            else
            {
                btnCreatedModifiedOverall.DateCreated = DateCreatedPPartner;
                btnCreatedModifiedOverall.CreatedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnCreatedBy, FMainDS.PPartner[0]);
            }

            /*
             * Decide on which DateModified and ModifiedBy to display:
             * If PPartner DateModified is later, take them from PPartner, otherwise from
             * the Table according to PartnerClass.
             */
            DateModifiedPPartner = TSaveConvert.DateColumnToDate(FMainDS.PPartner.ColumnDateModified, FMainDS.PPartner[0]);

            if (DateModifiedPartnerClassDependent != DateTime.MinValue)
            {
                if ((DateModifiedPPartner > DateModifiedPartnerClassDependent) && (DateModifiedPPartner != DateTime.MinValue))
                {
                    btnCreatedModifiedOverall.DateModified = DateModifiedPPartner;
                    btnCreatedModifiedOverall.ModifiedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnModifiedBy, FMainDS.PPartner[0]);
                }
                else
                {
                    btnCreatedModifiedOverall.DateModified = DateModifiedPartnerClassDependent;
                    btnCreatedModifiedOverall.ModifiedBy = ModifiedByPartnerClassDependent;
                }
            }
            else
            {
                btnCreatedModifiedOverall.DateModified = DateModifiedPPartner;
                btnCreatedModifiedOverall.ModifiedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnModifiedBy, FMainDS.PPartner[0]);
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
                // timop: I have disabled all controls. usually you have p_partner modify permissions, or none
                CustomEnablingDisabling.DisableControlGroup(pnlContent);
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
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_church
                        CustomEnablingDisabling.DisableControlGroup(pnlOther2ndLine);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.ORGANISATION:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_organisation
                        CustomEnablingDisabling.DisableControlGroup(pnlOther2ndLine);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.UNIT:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_unit
                        CustomEnablingDisabling.DisableControlGroup(pnlOther2ndLine);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.BANK:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_bank
                        CustomEnablingDisabling.DisableControlGroup(pnlOther2ndLine);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                case TPartnerClass.VENUE:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PVenueTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_partner
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, cmbOtherAddresseeTypeCode);
                        CustomEnablingDisabling.DisableControl(pnlOther2ndLine, chkOtherNoSolicitations);

                        // need to disable all Fields that are DataBound to p_venue
                        CustomEnablingDisabling.DisableControlGroup(pnlOther2ndLine);

                        cmbOtherAddresseeTypeCode.Focus();
                    }

                    break;

                default:
                    MessageBox.Show(String.Format(Catalog.GetString("Unrecognised Partner Class '{0}'!"), FPartnerClass));
                    break;
            }
        }

        private Boolean PartnerStatusCodeChangePromotion(string ANewPartnerStatusCode)
        {
            Boolean ReturnValue;
            String FamilyMembersText;
            PartnerEditTDSFamilyMembersTable FamilyMembersDT;
            Int32 Counter;
            Int32 Counter2;
            PartnerEditTDSFamilyMembersRow FamilyMembersDR;
            PartnerEditTDSFamilyMembersInfoForStatusChangeRow FamilyMembersInfoDR;
            DialogResult FamilyMembersResult;
            DataView FamilyMembersDV;

            ReturnValue = true;
            FamilyMembersText = "";

            /* Retrieve Family Members from the PetraServer */
            FamilyMembersDT = FPartnerEditUIConnector.GetDataFamilyMembers(FMainDS.PPartner[0].PartnerKey, "");
            FamilyMembersDV = new DataView(FamilyMembersDT, "", PPersonTable.GetFamilyIdDBName() + " ASC", DataViewRowState.CurrentRows);

            /* Build a formatted String of Family Members' PartnerKeys and ShortNames */
            for (Counter = 0; Counter <= FamilyMembersDV.Count - 1; Counter += 1)
            {
                FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter].Row;
                FamilyMembersText = FamilyMembersText + "   " + StringHelper.FormatStrToPartnerKeyString(FamilyMembersDR.PartnerKey.ToString()) +
                                    "   " + FamilyMembersDR.PartnerShortName + Environment.NewLine;
            }

            /* If there are Family Members, ... */
            if (FamilyMembersText != "")
            {
                /* show MessageBox with Family Members to the user, asking whether to promote. */
                FamilyMembersResult =
                    MessageBox.Show(
                        String.Format(
                            Catalog.GetString("Partner Status change to '{0}': \r\n" +
                                "Should OpenPetra apply this change to all Family Members of this Family?"),
                            ANewPartnerStatusCode) + Environment.NewLine + Environment.NewLine +
                        Catalog.GetString("The Family has the following Family Members:") + Environment.NewLine +
                        FamilyMembersText + Environment.NewLine +
                        Catalog.GetString("(Choose 'Cancel' to cancel the change of the Partner Status\r\n" +
                            "for this Partner)."),
                        Catalog.GetString("Promote Partner Status Change to All Family Members?"),
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                /* Check User's response */
                switch (FamilyMembersResult)
                {
                    case System.Windows.Forms.DialogResult.Yes:

                        /*
                         * User wants to promote the Partner StatusCode change to Family
                         * Members: add new DataTable for that purpose if it doesn't exist yet.
                         */
                        if (FMainDS.FamilyMembersInfoForStatusChange == null)
                        {
                            FMainDS.Tables.Add(new PartnerEditTDSFamilyMembersInfoForStatusChangeTable());
                            FMainDS.InitVars();
                        }

                        /*
                         * Remove any existing DataRows so we start from a 'clean slate'
                         * (the user could change the Partner StatusCode more than once...)
                         */
                        FMainDS.FamilyMembersInfoForStatusChange.Rows.Clear();

                        /*
                         * Add the PartnerKeys of the Family Members that we have just displayed
                         * to the user to the DataTable.
                         *
                         * Note: This DataTable will be sent to the PetraServer when the user
                         * saves the Partner. The UIConnector will pick it up and process it.
                         */
                        for (Counter2 = 0; Counter2 <= FamilyMembersDV.Count - 1; Counter2 += 1)
                        {
                            FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter2].Row;
                            FamilyMembersInfoDR = FMainDS.FamilyMembersInfoForStatusChange.NewRowTyped(false);
                            FamilyMembersInfoDR.PartnerKey = FamilyMembersDR.PartnerKey;
                            FMainDS.FamilyMembersInfoForStatusChange.Rows.Add(FamilyMembersInfoDR);
                        }

                        break;

                    case System.Windows.Forms.DialogResult.No:

                        /* no promotion wanted > nothing to do */
                        /* (StatusCode will be changed only for the Family) */
                        break;

                    case System.Windows.Forms.DialogResult.Cancel:
                        ReturnValue = false;
                        break;
                }
            }
            else
            {
            }

            /* no promotion needed since there are no Family Members */
            /* (StatusCode will be changed only for the Family) */
            return ReturnValue;
        }

        #endregion


        #region Event handlers


        private void OnAnyDataColumnChanging(System.Object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            // return if control has not actually been changed (i.e. CTRL+A will unneccessarily fire this event)
            if (ctrl.GetType() == typeof(TextBox))
            {
                if ((((TextBox)ctrl).SelectedText == ctrl.Text) && (ctrl.Text != ""))
                {
                    return;
                }
            }

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

        private void ValidateDataManual(PPartnerRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidatePartnerManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);

            if (FPartnerClass == "PERSON")
            {
                PPersonValidation.Validate(this, FMainDS.PPerson[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "FAMILY")
            {
                PFamilyValidation.Validate(this, FMainDS.PFamily[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "CHURCH")
            {
                PChurchValidation.Validate(this, FMainDS.PChurch[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                POrganisationValidation.Validate(this, FMainDS.POrganisation[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "UNIT")
            {
                PUnitValidation.Validate(this, FMainDS.PUnit[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "BANK")
            {
                PBankValidation.Validate(this, FMainDS.PBank[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else if (FPartnerClass == "VENUE")
            {
                PVenueValidation.Validate(this, FMainDS.PVenue[0], ref VerificationResultCollection,
                    FValidationControlsDict);
            }
        }

        private void PartnerStatusCodeChangePromotion(System.Object sender, EventArgs e)
        {
            string PartnerStatus = cmbPartnerStatus.GetSelectedString();

            // Business Rule: if the Partner's StatusCode changes, give the user the
            // option to promote the change to all Family Members (if the Partner is
            // a FAMILY and has Family Members).
            if ((FMainDS != null)
                && (!FIgnorePartnerStatusChange)
                && (FPartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY)))
            {
                if (PartnerStatus != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED))
                {
                    if (PartnerStatusCodeChangePromotion(PartnerStatus))
                    {
                        // Set the StatusChange date (this would be done on the server side
                        // automatically, but we want to display it now for immediate user feedback)
                        FMainDS.PPartner[0].StatusChange = DateTime.Today;
                    }
                    else
                    {
                        // User wants to cancel the change of the Partner StatusCode
                        // Undo the change in the DataColumn
                        FIgnorePartnerStatusChange = true;

                        UndoData(FMainDS.PPartner[0], cmbPartnerStatus);
                        cmbPartnerStatus.SelectNextControl(cmbPartnerStatus, true, true, true, true);

                        FIgnorePartnerStatusChange = false;
                    }
                }
            }
        }

        /// <summary>
        /// TODO: Replace this with the Data Validation Framework - once it supports user interaction as needed
        /// in this case (=asking the user to make a decision).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnitDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyUnitData(e, FMainDS, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.ResultCode != PetraErrorCodes.ERR_UNITNAMECHANGEUNDONE)
                    {
                        TMessages.MsgVerificationError(VerificationResultReturned, this.GetType());

                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FPetraUtilsObject.VerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName, DataRowVersion.Original];

                        // need to assign this to make the change actually visible...
                        txtUnitName.Text = e.ProposedValue.ToString();
// TODO                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
// TODO                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FPetraUtilsObject.VerificationResultCollection.Contains(e.Column))
                    {
                        FPetraUtilsObject.VerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
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

        private void MaintainGiftDestination(System.Object sender, System.EventArgs e)
        {
            if (this.FDelegateMaintainGiftDestination != null)
            {
                this.FDelegateMaintainGiftDestination();
            }
            else
            {
                throw new EVerificationMissing("FDelegateMaintainGiftDestination");
            }
        }

        #endregion

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        //// Special Handlers for menus and command keys for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.E | Keys.Control | Keys.Shift))
            {
                if (pnlPerson.Visible)
                {
                    txtPersonTitle.Focus();
                    return true;
                }
                else if (pnlFamily.Visible)
                {
                    txtFamilyTitle.Focus();
                    return true;
                }
                else if (pnlChurch.Visible)
                {
                    txtChurchName.Focus();
                    return true;
                }
                else if (pnlOrganisation.Visible)
                {
                    txtOrganisationName.Focus();
                    return true;
                }
                else if (pnlUnit.Visible)
                {
                    txtUnitName.Focus();
                    return true;
                }
                else if (pnlBank.Visible)
                {
                    txtBranchName.Focus();
                    return true;
                }
                else if (pnlVenue.Visible)
                {
                    txtVenueName.Focus();
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}