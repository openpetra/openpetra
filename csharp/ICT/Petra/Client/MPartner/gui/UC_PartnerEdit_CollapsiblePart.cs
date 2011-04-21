//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Globalization;
using Ict.Common;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MPartner;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Verification;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for the collapsible upper part of the Partner Edit screen.
    /// </summary>
    public partial class TUC_PartnerEdit_CollapsiblePart : TGrpCollapsible
    {
        /// <summary>todoComment</summary>
        public const String StrUnrecognisedPartnerClass = "Unrecognised Partner Class '";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion1 = "Partner Status change from '{0}' to '{1}':" + "\r\n" +
                                                                   "Should Petra apply this change to all Family Members of this Family?";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion2 = "The Family has the following Family Members:";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotion3 = "(Choose 'Cancel' to cancel the change of the Partner Status" + "\r\n" +
                                                                   "for this Partner).";

        /// <summary>todoComment</summary>
        public const String StrStatusCodeFamilyMembersPromotionTitle = "Promote Partner Status Change to All Family Members?";

        private PartnerEditTDS FMainDS;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private String FPartnerClass;
        private DataView FPartnerDefaultView;
        private TDelegateMaintainWorkerField FDelegateMaintainWorkerField;

        /// <summary>Used for keeping track of data verification errors</summary>
        private TVerificationResultCollection FVerificationResultCollection;

        /// <summary>todoComment</summary>
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

        /// <summary>todoComment</summary>
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

        /// <summary>
        /// This Event is thrown when the 'main data' of a DataTable for a certain
        /// PartnerClass has changed.
        ///
        /// </summary>
        public event TPartnerClassMainDataChangedHandler PartnerClassMainDataChanged;

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerEdit_CollapsiblePart() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblStatusUpdated.Text = Catalog.GetString("Status Updated:");
            this.lblPersonGender.Text = Catalog.GetString("&Gender:");
            this.lblTitleNamePerson.Text = Catalog.GetString("Title/Na&me:");
            this.lblPartnerStatus.Text = Catalog.GetString("Partner &Status:");
            this.lblAddresseeType.Text = Catalog.GetString("Addressee Type:");
            this.chkNoSolicitations.Text = Catalog.GetString("No Solicitations");
            this.txtPartnerKey.Text = Catalog.GetString("0000000000");
            this.lblPartnerClass.Text = Catalog.GetString("Class:");
            this.lblPartnerKey.Text = Catalog.GetString("Key:");
            this.lblPersonPanel.Text = Catalog.GetString("PERSON Panel");
            this.lblTitleNameFamily.Text = Catalog.GetString("Title/Na&me:");
            this.lblFamilyPanel.Text = Catalog.GetString("FAMILY Panel");
            this.lblOtherPanel.Text = Catalog.GetString("OTHER Panel");
            this.lblName.Text = Catalog.GetString("Na&me:");
            this.btnEditWorkerField.Text = Catalog.GetString("     &Field...");
            this.txtLastGiftDate.Text = Catalog.GetString("01-JAN-9999");
            this.txtLastGiftDetails.Text = Catalog.GetString("Currency + Amount, Given To");
            this.txtLastContactDate.Text = Catalog.GetString("01-JAN-9999");
            this.lblLastGiftDate.Text = Catalog.GetString("Last Gift:");
            this.lblLastContactDate.Text = Catalog.GetString("Last Contact:");
            #endregion

            // I18N: assign proper font which helps to read asian characters
            txtFamilyTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtFamilyFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtFamilyFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonTitle.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonFirstName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonMiddleName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPersonFamilyName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtOtherName.Font = TAppSettingsManager.GetDefaultBoldFont();
        }

        private TFrmPetraEditUtils FPetraUtilsObject;

        /// <summary>
        /// this provides general functionality for edit screens
        /// </summary>
        public TFrmPetraEditUtils PetraUtilsObject
        {
            get
            {
                return FPetraUtilsObject;
            }
            set
            {
                FPetraUtilsObject = value;
                FPetraUtilsObject.SetStatusBarText(this.btnEditWorkerField, "Select " + "Worker Field");
#if TODO
                FPetraUtilsObject.SetStatusBarText(chkNoSolicitations, PPartnerTable.GetNoSolicitationsHelp());
                FPetraUtilsObject.SetStatusBarText(cmbAddresseeType, PPartnerTable.GetAddresseeTypeCodeHelp());
                FPetraUtilsObject.SetStatusBarText(cmbPartnerStatus, PPartnerTable.GetStatusCodeHelp());
                FPetraUtilsObject.SetStatusBarText(txtPersonTitle, PPersonTable.GetTitleHelp());
                FPetraUtilsObject.SetStatusBarText(txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                FPetraUtilsObject.SetStatusBarText(txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
                FPetraUtilsObject.SetStatusBarText(cmbPersonGender, PPersonTable.GetGenderHelp());
                FPetraUtilsObject.SetStatusBarText(txtFamilyTitle, PFamilyTable.GetTitleHelp());
                FPetraUtilsObject.SetStatusBarText(txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtOtherName, PChurchTable.GetChurchNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtOtherName, POrganisationTable.GetOrganisationNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtOtherName, PUnitTable.GetUnitNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtOtherName, PBankTable.GetBranchNameHelp());
                FPetraUtilsObject.SetStatusBarText(txtOtherName, PVenueTable.GetVenueNameHelp());
#endif
            }
        }

        /// <summary>
        /// needed for generated code
        /// </summary>
        public void InitUserControl()
        {
        }

        private Boolean PartnerStatusCodeChangePromotion(DataColumnChangeEventArgs e)
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

            // Retrieve Family Members from the PetraServer
            FamilyMembersDT = FPartnerEditUIConnector.GetDataFamilyMembers(FMainDS.PPartner[0].PartnerKey, "");
            FamilyMembersDV = new DataView(FamilyMembersDT, "", PPersonTable.GetFamilyIdDBName() + " ASC", DataViewRowState.CurrentRows);

            // Build a formatted String of Family Members' PartnerKeys and ShortNames
            for (Counter = 0; Counter <= FamilyMembersDV.Count - 1; Counter += 1)
            {
                FamilyMembersDR = (PartnerEditTDSFamilyMembersRow)FamilyMembersDV[Counter].Row;
                FamilyMembersText = FamilyMembersText + "   " + StringHelper.FormatStrToPartnerKeyString(FamilyMembersDR.PartnerKey.ToString()) +
                                    "   " + FamilyMembersDR.PartnerShortName + Environment.NewLine;
            }

            // If there are Family Members, ...
            if (FamilyMembersText != "")
            {
                // show MessageBox with Family Members to the user, asking whether to promote.
                FamilyMembersResult =
                    MessageBox.Show(String.Format(StrStatusCodeFamilyMembersPromotion1,
                            ((PPartnerRow)e.Row).StatusCode,
                            e.ProposedValue) + Environment.NewLine + Environment.NewLine + StrStatusCodeFamilyMembersPromotion2 +
                        Environment.NewLine +
                        FamilyMembersText + Environment.NewLine + StrStatusCodeFamilyMembersPromotion3, StrStatusCodeFamilyMembersPromotionTitle,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                // Check User's response
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

                        // no promotion wanted > nothing to do
                        // (StatusCode will be changed only for the Family)
                        break;

                    case System.Windows.Forms.DialogResult.Cancel:
                        ReturnValue = false;
                        break;
                }
            }
            else
            {
            }

            // no promotion needed since there are no Family Members
            // (StatusCode will be changed only for the Family)
            return ReturnValue;
        }

        #region Public Methods

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AMainDS"></param>
        public void PerformDataBinding(PartnerEditTDS AMainDS)
        {
            FMainDS = AMainDS;
            PerformDataBinding();

            // Extender Provider
            this.expStringLengthCheckCollapsiblePart.RetrieveTextboxes(this);
        }

        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControlGroup(pnlPartner);
                CustomEnablingDisabling.DisableControlGroup(pnlRightSide);
            }

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_person
                        CustomEnablingDisabling.DisableControlGroup(pnlPerson);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.FAMILY:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PFamilyTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_family
                        CustomEnablingDisabling.DisableControlGroup(pnlFamily);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.CHURCH:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PChurchTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_church
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.ORGANISATION:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, POrganisationTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_organisation
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.UNIT:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PUnitTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_unit
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.BANK:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PBankTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_bank
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                case TPartnerClass.VENUE:

                    if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PVenueTable.GetTableDBName()))
                    {
                        // need to disable all Fields that are DataBound to p_venue
                        CustomEnablingDisabling.DisableControlGroup(pnlOther);
                        cmbAddresseeType.Focus();
                    }

                    break;

                default:
                    MessageBox.Show(StrUnrecognisedPartnerClass + FPartnerClass + "'!");
                    break;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void PerformDataBinding()
        {
            Binding DateFormatBinding;

            FPartnerDefaultView = FMainDS.PPartner.DefaultView;

            // Perform DataBinding
            txtPartnerKey.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerKeyDBName());
            txtPartnerClass.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerClassDBName());
            chkNoSolicitations.DataBindings.Add("Checked", FMainDS.PPartner, PPartnerTable.GetNoSolicitationsDBName());
            txtLastGiftDetails.DataBindings.Add("Text", FMainDS.MiscellaneousData, PartnerEditTDSMiscellaneousDataTable.GetLastGiftInfoDBName());

            // DataBind Date fields
            DateFormatBinding = new Binding("Text", FMainDS.PPartner, PPartnerTable.GetStatusChangeDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtStatusChange.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FMainDS,
                PartnerEditTDSMiscellaneousDataTable.GetTableName() + '.' + PartnerEditTDSMiscellaneousDataTable.GetLastGiftDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtLastGiftDate.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FMainDS,
                PartnerEditTDSMiscellaneousDataTable.GetTableName() + '.' + PartnerEditTDSMiscellaneousDataTable.GetLastContactDateDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            txtLastContactDate.DataBindings.Add(DateFormatBinding);

            // DataBind AutoPopulatingComboBoxes
            cmbAddresseeType.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetAddresseeTypeCodeDBName());
            cmbPartnerStatus.PerformDataBinding(FPartnerDefaultView, PPartnerTable.GetStatusCodeDBName());

            FMainDS.PPartner.ColumnChanging += new DataColumnChangeEventHandler(this.OnPartnerDataColumnChanging);
            #region Bind and show fields according to Partner Class
            FPartnerClass = FMainDS.PPartner[0].PartnerClass.ToString();

            switch (SharedTypes.PartnerClassStringToEnum(FPartnerClass))
            {
                case TPartnerClass.PERSON:
                    txtPersonTitle.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetTitleDBName());
                    txtPersonFirstName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetFirstNameDBName());
                    txtPersonMiddleName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetMiddleName1DBName());
                    txtPersonFamilyName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetFamilyNameDBName());
                    cmbPersonGender.PerformDataBinding(FMainDS.PPerson, PPersonTable.GetGenderDBName());

                    // The following is commented out because currently the Text is set with a call to SetWorkerFieldText from PartnerEdit.pas
                    // txtWorkerField.DataBindings.Add('Text', FMainDS.PPerson, FMainDS.PPerson.GetUnitNameDBName);
                    pnlPerson.Visible = true;
                    pnlWorkerField.Visible = true;
                    txtPartnerClass.BackColor = System.Drawing.Color.Yellow;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtPersonTitle, PPersonTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtPersonFirstName, PPersonTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtPersonMiddleName, PPersonTable.GetMiddleName1Help());
                    tipMain.SetToolTip(this.txtPersonFamilyName, PPersonTable.GetFamilyNameHelp());
#endif
                    FMainDS.PPerson.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    this.cmbPersonGender.SelectedValueChanged += new System.EventHandler(this.CmbPersonGender_SelectedValueChanged);
                    break;

                case TPartnerClass.FAMILY:
                    txtFamilyTitle.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetTitleDBName());
                    txtFamilyFirstName.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetFirstNameDBName());
                    txtFamilyFamilyName.DataBindings.Add("Text", FMainDS.PFamily, PFamilyTable.GetFamilyNameDBName());

                    // The following is commented out because currently the Text is set with a call to SetWorkerFieldText from PartnerEdit.pas
                    // txtWorkerField.DataBindings.Add('Text', FMainDS.PFamily, FMainDS.PFamily.GetUnitNameDBName);
                    pnlFamily.Visible = true;
                    pnlWorkerField.Visible = true;

                    // Set ToolTips in addition to StatusBar texts for fields to make it clearer what to fill in there...
#if TODO
                    tipMain.SetToolTip(this.txtFamilyTitle, PFamilyTable.GetTitleHelp());
                    tipMain.SetToolTip(this.txtFamilyFirstName, PFamilyTable.GetFirstNameHelp());
                    tipMain.SetToolTip(this.txtFamilyFamilyName, PFamilyTable.GetFamilyNameHelp());
#endif
                    FMainDS.PFamily.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.CHURCH:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PChurch, PChurchTable.GetChurchNameDBName());
                    pnlOther.Visible = true;

                    FMainDS.PChurch.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.ORGANISATION:
                    txtOtherName.DataBindings.Add("Text", FMainDS.POrganisation, POrganisationTable.GetOrganisationNameDBName());
                    pnlOther.Visible = true;

                    FMainDS.POrganisation.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.UNIT:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PUnit, PUnitTable.GetUnitNameDBName());
                    pnlOther.Visible = true;

                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(this.OnUnitDataColumnChanging);
                    FMainDS.PUnit.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.BANK:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PBank, PBankTable.GetBranchNameDBName());
                    pnlOther.Visible = true;

                    FMainDS.PBank.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                case TPartnerClass.VENUE:
                    txtOtherName.DataBindings.Add("Text", FMainDS.PVenue, PVenueTable.GetVenueNameDBName());
                    pnlOther.Visible = true;

                    FMainDS.PVenue.ColumnChanging += new DataColumnChangeEventHandler(this.OnAnyDataColumnChanging);
                    break;

                default:
                    MessageBox.Show(StrUnrecognisedPartnerClass + FPartnerClass + "'!");
                    break;
            }

            #endregion
            SetupBtnCreated();
            SetupChkNoSolicitations();
            ApplySecurity();
        }

        /// <summary>
        /// Initialises Delegate Function to handle click on the "Change Worker Field" button
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateMaintainWorkerField(TDelegateMaintainWorkerField ADelegateFunction)
        {
            FDelegateMaintainWorkerField = ADelegateFunction;
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
        /// todoComment
        /// </summary>
        /// <param name="AWorkerField"></param>
        public void SetWorkerFieldText(String AWorkerField)
        {
            txtWorkerField.Text = AWorkerField;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up the Button that holds the Created and Modified information.
        ///
        /// Since there are always two Tables involved in the data that is displayed in
        /// this UserControl, DateCreated and CreatedBy are taken from the table where
        /// DateCreated is earlier, and DateModified and ModifiedBy are taken from the
        /// table where DateModified is later.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupBtnCreated()
        {
            DateTime DateCreatedPPartner = DateTime.Now;
            DateTime DateModifiedPPartner = DateTime.Now;
            DateTime DateCreatedPartnerClassDependent = DateTime.Now;
            DateTime DateModifiedPartnerClassDependent = DateTime.Now;
            String CreatedByPartnerClassDependent = "";
            String ModifiedByPartnerClassDependent = "";

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

            if ((DateCreatedPPartner < DateCreatedPartnerClassDependent) && (DateCreatedPPartner != DateTime.MinValue))
            {
                btnCreatedOverall.DateCreated = DateCreatedPPartner;
                btnCreatedOverall.CreatedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnCreatedBy, FMainDS.PPartner[0]);
            }
            else
            {
                btnCreatedOverall.DateCreated = DateCreatedPartnerClassDependent;
                btnCreatedOverall.CreatedBy = CreatedByPartnerClassDependent;
            }

            /*
             * Decide on which DateModified and ModifiedBy to display:
             * If PPartner DateModified is later, take them from PPartner, otherwise from
             * the Table according to PartnerClass.
             */
            DateModifiedPPartner = TSaveConvert.DateColumnToDate(FMainDS.PPartner.ColumnDateModified, FMainDS.PPartner[0]);

            if ((DateModifiedPPartner > DateModifiedPartnerClassDependent) && (DateModifiedPPartner != DateTime.MinValue))
            {
                btnCreatedOverall.DateModified = DateModifiedPPartner;
                btnCreatedOverall.ModifiedBy = TSaveConvert.StringColumnToString(FMainDS.PPartner.ColumnModifiedBy, FMainDS.PPartner[0]);
            }
            else
            {
                btnCreatedOverall.DateModified = DateModifiedPartnerClassDependent;
                btnCreatedOverall.ModifiedBy = ModifiedByPartnerClassDependent;
            }
        }

        /// <summary>
        /// Sets the background colour of the CheckBox depending on whether it is
        /// Checked or not.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupChkNoSolicitations()
        {
            if (chkNoSolicitations.Checked)
            {
                chkNoSolicitations.BackColor = System.Drawing.Color.PeachPuff;
            }
            else
            {
                chkNoSolicitations.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        #endregion

        #region Event handlers
        private void CmbPersonGender_SelectedValueChanged(System.Object sender, System.EventArgs e)
        {
            if (cmbPersonGender.SelectedItem.ToString() == "Female")
            {
                cmbAddresseeType.SelectedItem = SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcFEMALE);
            }
            else if (cmbPersonGender.SelectedItem.ToString() == "Male")
            {
                cmbAddresseeType.SelectedItem = SharedTypes.StdAddresseeTypeCodeEnumToString(TStdAddresseeTypeCode.satcMALE);
            }

            /*
             * Also assign the value directly to the databound data field!
             * Strangely enough, this is necessary for the case if the user doesn't TAB out
             * of cmbPersonGender, but uses the mouse to select anything else on the screen
             * *except* cmbAddresseeType!
             */
            FMainDS.PPartner[0].AddresseeTypeCode = cmbAddresseeType.SelectedItem.ToString();
        }

        private void ChkNoSolicitations_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            SetupChkNoSolicitations();
        }

        private void OnAnyDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TPartnerClassMainDataChangedEventArgs EventFireArgs;

            // messagebox.show('Column_Changing Event: Column=' + e.Column.ColumnName +
            // '; Column content=' + e.Row[e.Column.ColumnName].ToString +
            // '; ' + e.ProposedValue.ToString);
            // MessageBox.Show('PartnerClass: ' + FPartnerClass.ToString);
            EventFireArgs = new TPartnerClassMainDataChangedEventArgs();
            EventFireArgs.PartnerClass = FPartnerClass;

            if (FPartnerClass == "PERSON")
            {
                if ((e.Column.ColumnName == PPersonTable.GetTitleDBName()) || (e.Column.ColumnName == PPersonTable.GetFirstNameDBName())
                    || (e.Column.ColumnName == PPersonTable.GetMiddleName1DBName()) || (e.Column.ColumnName == PPersonTable.GetFamilyNameDBName()))
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
                if ((e.Column.ColumnName == PFamilyTable.GetTitleDBName()) || (e.Column.ColumnName == PFamilyTable.GetFirstNameDBName())
                    || (e.Column.ColumnName == PFamilyTable.GetFamilyNameDBName()))
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtFamilyFamilyName.Text,
                        txtFamilyTitle.Text,
                        txtFamilyFirstName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "CHURCH")
            {
                if (e.Column.ColumnName == PChurchTable.GetChurchNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "ORGANISATION")
            {
                if (e.Column.ColumnName == POrganisationTable.GetOrganisationNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "UNIT")
            {
                if (e.Column.ColumnName == PUnitTable.GetUnitNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "BANK")
            {
                if (e.Column.ColumnName == PBankTable.GetBranchNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
            else if (FPartnerClass == "VENUE")
            {
                if (e.Column.ColumnName == PVenueTable.GetVenueNameDBName())
                {
                    FMainDS.PPartner[0].PartnerShortName = Calculations.DeterminePartnerShortName(txtOtherName.Text);
                    OnPartnerClassMainDataChanged(EventFireArgs);
                }
            }
        }

        private void OnPartnerDataColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            TVerificationResult VerificationResultReturned;
            TScreenVerificationResult VerificationResultEntry;
            Control BoundControl;

            // MessageBox.Show('Column ''' + e.Column.ToString + ''' is changing...');
            try
            {
                if (TPartnerVerification.VerifyPartnerData(e, out VerificationResultReturned) == false)
                {
                    if (VerificationResultReturned.ResultCode != ErrorCodes.PETRAERRORCODE_PARTNERSTATUSMERGEDCHANGEUNDONE)
                    {
                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.ResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.ResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.ResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PPartner], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FVerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName];

                        // need to assign this to make the change actually visible...
                        cmbPartnerStatus.SelectedItem = e.ProposedValue.ToString();

                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.ResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.ResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.ResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FPartnerDefaultView], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }

                    // Business Rule: if the Partner's StatusCode changes, give the user the
                    // option to promote the change to all Family Members (if the Partner is
                    // a FAMILY and has Family Members).
                    if (e.Column.ColumnName == PPartnerTable.GetStatusCodeDBName())
                    {
                        if (PartnerStatusCodeChangePromotion(e))
                        {
                            // Set the StatusChange date (this would be done on the server side
                            // automatically, but we want to display it now for immediate user feedback)
                            FMainDS.PPartner[0].StatusChange = DateTime.Today;
                        }
                        else
                        {
                            // User wants to cancel the change of the Partner StatusCode
                            // Undo the change in the DataColumn
                            e.ProposedValue = e.Row[e.Column.ColumnName];

                            // Need to assign this to make the change actually visible...
                            cmbPartnerStatus.SelectedItem = e.ProposedValue.ToString();
                        }
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

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
                    if (VerificationResultReturned.ResultCode != ErrorCodes.PETRAERRORCODE_UNITNAMECHANGEUNDONE)
                    {
                        // TODO 1 ochristiank cUI : Make a message library and call a method there to show verification errors.
                        MessageBox.Show(
                            VerificationResultReturned.ResultText + Environment.NewLine + Environment.NewLine + "Message Number: " +
                            VerificationResultReturned.ResultCode + Environment.NewLine + "Context: " + this.GetType().ToString() +
                            Environment.NewLine +
                            "Release: ",
                            VerificationResultReturned.ResultTextCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                        VerificationResultEntry = new TScreenVerificationResult(this,
                            e.Column,
                            VerificationResultReturned.ResultText,
                            VerificationResultReturned.ResultTextCaption,
                            VerificationResultReturned.ResultCode,
                            BoundControl,
                            VerificationResultReturned.ResultSeverity);
                        FVerificationResultCollection.Add(VerificationResultEntry);

                        // MessageBox.Show('After setting the error: ' + e.ProposedValue.ToString);
                    }
                    else
                    {
                        // undo the change in the DataColumn
                        e.ProposedValue = e.Row[e.Column.ColumnName, DataRowVersion.Original];

                        // need to assign this to make the change actually visible...
                        txtOtherName.Text = e.ProposedValue.ToString();
                        BoundControl = TDataBinding.GetBoundControlForColumn(BindingContext[FMainDS.PUnit], e.Column);

                        // MessageBox.Show('Bound control: ' + BoundControl.ToString);
                        BoundControl.Focus();
                    }
                }
                else
                {
                    if (FVerificationResultCollection.Contains(e.Column))
                    {
                        FVerificationResultCollection.Remove(e.Column);
                    }
                }
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void BtnEditWorkerField_Click(System.Object sender, System.EventArgs e)
        {
            if (this.FDelegateMaintainWorkerField != null)
            {
                try
                {
                    this.FDelegateMaintainWorkerField();
                }
                finally
                {
                }

                // raise EVerificationMissing.Create('this.FDelegateGetPartnerShortName could not be called!');
            }
        }

        /// <summary>
        /// Checks whether there any Tips to show to the User; if there are, they will be
        /// shown.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CheckForUserTips()
        {
// TODO balloontip
#if TODO
            TBalloonTip BalloonTip;

            // Show general Tip about a number of changes to Tab Headers if the Tips for
            // these Tab Headers haven't been shown yet.
            if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersGeneral) == '!')
            {
                if ((TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersAddresses) == '!')
                    && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) == '!')
                    && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersNotes) == '!'))
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersGeneral);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "Petra Version 2.2.7 Change: Counters in Tab Headers",
                        "The counters in the tab headers of the Addresses and Subscription Tabs work different than before." + "\r\n" +
                        "The Notes tab header now also has an indicator that shows whether Partner Notes are entered, or not." + "\r\n" +
                        "Switch to these tabs to see an explanation of the changes.",
                        pnlBalloonTipAnchorTabs);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }
                else
                {
                    // since the precondition was not met, we don't need to show this User Tip anymore
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewTabCountersGeneral);
                }
            }

            // The following Tips are only shown after all User Tips for the Tab Headers
            // have been shown  this is done to prevent those User Tips from coming up at
            // the same time than a User Tip from the Tabs, which wouldn't look nice.
            if ((TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersAddresses) != '!')
                && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersSubscriptions) != '!')
                && (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewTabCountersNotes) != '!'))
            {
                // Show Tip about Video Tutorial.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatPartnerEditVideoTutorial) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatPartnerEditVideoTutorial);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.7: Video Tutorial",
                        "The Video Tutorial that is available for the Partner Edit screen can now be launched directly from the" + "\r\n" +
                        "Partner Edit screen. Choose 'Video Tutorial for Partner Edit screen...' from the 'Help' menu to see the video.",
                        pnlBalloonTipAnchorHelp);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }

                // Show Tip about PartnerStatus promotion for Partner of Class FAMILY.
                // This Tip is only shown after the User Tip for the Video Tutorial has been shown
                // this is done to prevent this User Tip coming up at the same time than the
                // User Tip from the Video Tutorial, which wouldn't look nice.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewPromotePartnerStatusChange) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewPromotePartnerStatusChange);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.8: Change of 'Partner Status' for a FAMILY",
                        "When changing the 'Partner Status' for a FAMILY: if the FAMILY has Family Members, you are now" + "\r\n" +
                        "asked whether this change should be applied to the Partner Statuses of all Family Members." + "\r\n" +
                        "This will help in keeping the Partner Statuses of FAMILYs and their Family Members in sync.",
                        cmbPartnerStatus);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }

                // Show Tip about Deactivate Partner Dialog.
                // This Tip is only shown after the User Tip for the PartnerStatus promotion
                // for Partner of Class FAMILY has been shown  this is done to prevent this
                // User Tip coming up at the same time than the User Tip for the PartnerStatus
                // promotion for Partner of Class FAMILY, which wouldn't look nice.
                if (TUserTips.TMPartner.CheckTipStatus(TMPartnerTips.mpatNewDeactivatePartner) == '!')
                {
                    // Set Tip Status so it doesn't get picked up again!
                    TUserTips.TMPartner.SetTipViewed(TMPartnerTips.mpatNewDeactivatePartner);
                    BalloonTip = new TBalloonTip();
                    BalloonTip.ShowBalloonTipNewFunction(
                        "New in Petra Version 2.2.8: Deactivate Partner Dialog",
                        "This new functionality allows the full 'deactivation' of a Partner in one step instead of three steps:" + "\r\n" +
                        "  1) Sets the 'Partner Status' (eg. to 'INVALID')," + "\r\n" + "  2) Cancels all Subscriptions," + "\r\n" +
                        "  3) Expires all Current Addresses." + "\r\n" +
                        "Each of these steps will be done by default, but you can choose to not to perform a certain step." + "\r\n" +
                        "This functionality will help in doing the necessary steps needed to deactivate a Partner more quickly and consistently." +
                        "\r\n" + "It will be very helpful for processing returned mail." + "\r\n" +
                        "Choose 'Deactivate Partner...' from the 'File' menu to use this new functionality.",
                        pnlBalloonTipAnchorHelp);

                    // Dont' show any more Tips in this instance of the Partner Edit screen
                    return;
                }
            }
#endif
        }

        #endregion

        #region Custom Events
        private void OnPartnerClassMainDataChanged(TPartnerClassMainDataChangedEventArgs e)
        {
            // MessageBox.Show('OnPartnerClassMainDataChanged. e.PartnerClass: ' + e.PartnerClass.ToString);
            if (PartnerClassMainDataChanged != null)
            {
                PartnerClassMainDataChanged(this, e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Event Arguments declaration
    /// </summary>
    public class TPartnerClassMainDataChangedEventArgs : System.EventArgs
    {
        /// <summary>todoComment</summary>
        public String PartnerClass;
    }

    /// <summary>todoComment</summary>
    public delegate void TDelegateMaintainWorkerField();

    /// <summary>Event handler declaration</summary>
    public delegate void TPartnerClassMainDataChangedHandler(System.Object Sender, TPartnerClassMainDataChangedEventArgs e);
}