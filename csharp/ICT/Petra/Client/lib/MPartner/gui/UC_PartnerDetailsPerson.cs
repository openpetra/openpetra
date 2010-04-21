/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using System.Globalization;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl for editing Partner Details for a Partner of Partner Class PERSON.
    /// </summary>
    public partial class TUC_PartnerDetailsPerson : System.Windows.Forms.UserControl, IPetraEditUserControl
    {
        /// <summary>todoComment</summary>
        protected PartnerEditTDS FMainDS;
        private System.Object FSelectedAcquisitionCode;

        /// <summary>todoComment</summary>
        public TexpTextBoxStringLengthCheck expStringLengthCheckPerson;

        /// <summary>todoComment</summary>
        public PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerDetailsPerson() : base ()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.grpMisc.Text = Catalog.GetString("Miscellaneous");
            this.txtOccupationCode.ButtonText = Catalog.GetString("&Occupation...");
            this.lblAcquisitionCode.Text = Catalog.GetString("&Acquisition Code:");
            this.lblLanguageCode.Text = Catalog.GetString("Lang&uage Code:");
            this.lblMaritalStatusComment.Text = Catalog.GetString("Marital Status Comment:");
            this.lblMaritalStatusSince.Text = Catalog.GetString("Marital Status Si&nce:");
            this.lblMaritalStatus.Text = Catalog.GetString("Mari&tal Status:");
            this.lblAcademicTitle.Text = Catalog.GetString("A&cademic Title:");
            this.lblDateOfBirth.Text = Catalog.GetString("&Date of Birth:");
            this.lblDecorations.Text = Catalog.GetString("Dec&orations:");
            this.grpNames.Text = Catalog.GetString("Names");
            this.lblLocalName.Text = Catalog.GetString("&Local Name:");
            this.lblPreferredName.Text = Catalog.GetString("&Preferred Name:");
            this.lblPreviousName.Text = Catalog.GetString("P&revious Name:");
            this.grpBelieverSince.Text = Catalog.GetString("Believer since");
            this.lblBelieverSince.Text = Catalog.GetString("&Year:");
            this.lblComment.Text = Catalog.GetString("Comment:");
            #endregion

            // I18N: assign proper font which helps to read asian characters
            txtLocalName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPreviousName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtPreferredName.Font = TAppSettingsManager.GetDefaultBoldFont();
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
            }
        }

        #region Public methods

        /// <summary>
        /// todoComment
        /// </summary>
        public void InitialiseUserControl()
        {
            Binding DateFormatBinding;
            Binding NullableNumberFormatBinding;

            // Names GroupBox
            txtPreferredName.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetPreferedNameDBName());
            txtPreviousName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPreviousNameDBName());
            txtLocalName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameLocDBName());
            txtDecorations.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetDecorationsDBName());
            txtAcademicTitle.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetAcademicTitleDBName());
            txtOccupationCode.PerformDataBinding(FMainDS.PPerson.DefaultView, PPersonTable.GetOccupationCodeDBName());

            // Miscellaneous GroupBox
            DateFormatBinding = new Binding("Text", FMainDS.PPerson, PPersonTable.GetDateOfBirthDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            txtDateOfBirth.DataBindings.Add(DateFormatBinding);
            DateFormatBinding = new Binding("Text", FMainDS.PPerson, PPersonTable.GetMaritalStatusSinceDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            txtMaritalStatusSince.DataBindings.Add(DateFormatBinding);

            // DataBind AutoPopulatingComboBoxes
            cmbAcquisitionCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetAcquisitionCodeDBName());
            cmbMaritalStatus.PerformDataBinding(FMainDS.PPerson, PPersonTable.GetMaritalStatusDBName());
            cmbLanguageCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetLanguageCodeDBName());

            // Believer since GroupBox
            this.txtBelieverComment.DataBindings.Add("Text", FMainDS.PPerson, PPersonTable.GetBelieverSinceCommentDBName());
            NullableNumberFormatBinding = new Binding("Text", FMainDS.PPerson, PPersonTable.GetBelieverSinceYearDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber_2);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtBelieverSince.DataBindings.Add(NullableNumberFormatBinding);
            this.txtMaritalStatusComment.DataBindings.Add("Text", FMainDS,
                PPersonTable.GetTableName() + "." + PFamilyTable.GetMaritalStatusCommentDBName());
            btnCreatedPerson.UpdateFields(FMainDS.PPerson);

            // Extender Provider
            this.expStringLengthCheckPerson.RetrieveTextboxes(this);

            // Set StatusBar Texts
#if TODO
            FPetraUtilsObject.SetStatusBarText(txtPreferredName, PPersonTable.GetPreferedNameHelp());
            FPetraUtilsObject.SetStatusBarText(txtPreviousName, PPartnerTable.GetPreviousNameHelp());
            FPetraUtilsObject.SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            FPetraUtilsObject.SetStatusBarText(txtDecorations, PPersonTable.GetDecorationsHelp());
            FPetraUtilsObject.SetStatusBarText(txtAcademicTitle, PPersonTable.GetAcademicTitleHelp());
            FPetraUtilsObject.SetStatusBarText(txtOccupationCode, PPersonTable.GetOccupationCodeHelp());
            FPetraUtilsObject.SetStatusBarText(txtDateOfBirth, PPersonTable.GetDateOfBirthHelp());
            FPetraUtilsObject.SetStatusBarText(txtMaritalStatusSince, PPersonTable.GetMaritalStatusSinceHelp());
            FPetraUtilsObject.SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            FPetraUtilsObject.SetStatusBarText(cmbMaritalStatus, PPersonTable.GetMaritalStatusHelp());
            FPetraUtilsObject.SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            FPetraUtilsObject.SetStatusBarText(txtBelieverSince, PPersonTable.GetBelieverSinceYearHelp());
            FPetraUtilsObject.SetStatusBarText(txtBelieverComment, PPersonTable.GetBelieverSinceCommentHelp());
#endif
            #region Verification
            txtOccupationCode.VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            #endregion
            ApplySecurity();
        }

        #endregion

        #region Helper functions
        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControl(pnlPreferedPreviousName, txtPreviousName);
                CustomEnablingDisabling.DisableControl(pnlLocalName, txtLocalName);
                CustomEnablingDisabling.DisableControl(pnlAcquisition, cmbAcquisitionCode);
                CustomEnablingDisabling.DisableControl(pnlAcquisition, cmbLanguageCode);
            }

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPersonTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_person
                CustomEnablingDisabling.DisableControlGroup(pnlBirthDecoration);
                CustomEnablingDisabling.DisableControlGroup(pnlMaritalAcademic);
                CustomEnablingDisabling.DisableControlGroup(pnlMaritalSince);
                CustomEnablingDisabling.DisableControlGroup(pnlOccupation);
                CustomEnablingDisabling.DisableControlGroup(pnlBelieverSinceYear);
                CustomEnablingDisabling.DisableControl(pnlPreferedPreviousName, txtPreferredName);
            }
        }

        #endregion

        #region Event Handlers
        private void TxtOccupationCode_ClickButton(string LabelStringIn,
            string TextBoxStringIn,
            out string LabelStringOut,
            out string TextBoxStringOut)
        {
// TODO Occupation
            LabelStringOut = null;
            TextBoxStringOut = "";
#if TODO
            TCmdMPartner mCmdMPartner;
            String mOccupationCode;

            // call Progress from here
            mCmdMPartner = new TCmdMPartner();
            mCmdMPartner.OpenOccupationFindScreen(this.ParentForm, out mOccupationCode);

            // I can only return the Occupation Code
            TextBoxStringOut = mOccupationCode;
            LabelStringOut = null;
#endif
        }

        private void TxtBelieverComment_TextChanged(System.Object sender, System.EventArgs e)
        {
            // messagebox.Show('MaxLength: ' + this.txtBelieverComment.MaxLength.ToString + this.txtBelieverComment.Text);
        }

        /// <summary>
        /// checks that the Acquisition Code is valid.
        /// </summary>
        /// <returns>void</returns>
        private void CmbAcquisitionCode_Leave(System.Object sender, System.EventArgs e)
        {
            DataTable DataCacheAcquisitionCodeTable;
            PAcquisitionRow TmpRow;
            DialogResult UseAlthoughUnassignable;

            try
            {
                // check if the publication selected is valid, if not, gives warning.
                DataCacheAcquisitionCodeTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AcquisitionCodeList);
                TmpRow = (PAcquisitionRow)DataCacheAcquisitionCodeTable.Rows.Find(new Object[] { this.cmbAcquisitionCode.SelectedItem.ToString() });

                if (TmpRow != null)
                {
                    if (TmpRow.ValidAcquisition)
                    {
                        FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                    }
                    else
                    {
                        UseAlthoughUnassignable = MessageBox.Show(CommonResourcestrings.StrErrorTheCodeIsNoLongerActive1 + " '" +
                            this.cmbAcquisitionCode.SelectedItem.ToString() + "' " +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive2 + "\r\n" +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive3 + "\r\n" + "\r\n" +
                            "Message Number: " + ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE + "\r\n" +
                            "File Name: " + this.GetType().FullName, "Invalid Data Entered",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                        if (UseAlthoughUnassignable == System.Windows.Forms.DialogResult.No)
                        {
                            // If user selects not to use the publication, the recent publication code is selected.
                            this.cmbAcquisitionCode.SelectedItem = FSelectedAcquisitionCode;
                        }
                        else
                        {
                            FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}