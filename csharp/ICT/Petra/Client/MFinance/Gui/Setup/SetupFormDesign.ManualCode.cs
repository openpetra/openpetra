//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupFormDesign
    {
        #region Initialise

        private void InitializeManualCode()
        {
            // The form type code has the entries from Frequency plus an entry 'Standard'
            // The sort is handled automatically on the description column
            DataView dv = (DataView)cmbDetailFormTypeCode.cmbCombobox.DataSource;
            DataRowView rv = dv.AddNew();

            rv[0] = MFinanceConstants.FORM_TYPE_CODE_STANDARD;
            rv[1] = MFinanceConstants.FORM_TYPE_CODE_STANDARD;

            // Our currency is always with 2 decimals
            txtDetailMinimumAmount.AlwaysHideLabel = true;
            txtDetailMinimumAmount.CurrencyCode = TTxtCurrencyTextBox.CURRENCY_STANDARD_2_DP;

            // We like our text in bold font
            cmbDetailFormCode.Font = new System.Drawing.Font(cmbDetailFormCode.Font, System.Drawing.FontStyle.Bold);

            // Set up a Resize event so we can stretch the filename text box
            this.Resize += TFrmSetupFormDesign_Resize;
        }

        void TFrmSetupFormDesign_Resize(object sender, EventArgs e)
        {
            // Resize the filename text box and anchor the Browse button to the right edge
            btnBrowse.Left = this.Width - btnBrowse.Width - 20;
            txtDetailFormFileName.Width = btnBrowse.Left - txtDetailFormFileName.Left - 10;
        }

        #endregion

        #region New record

        private void NewRecord(object Sender, EventArgs e)
        {
            CreateNewAForm();
        }

        private void NewRowManual(ref AFormRow ARow)
        {
            int suffix = 1;

            // Choose a suffix that is not used in any FormCode
            while ((FMainDS.AForm.Rows.Find(new object[] { MFinanceConstants.FORM_CODE_CHEQUE, "NEWFORM" + suffix.ToString() }) != null)
                   || (FMainDS.AForm.Rows.Find(new object[] { MFinanceConstants.FORM_CODE_RECEIPT, "NEWFORM" + suffix.ToString() }) != null)
                   || (FMainDS.AForm.Rows.Find(new object[] { MFinanceConstants.FORM_CODE_REMITTANCE, "NEWFORM" + suffix.ToString() }) != null))
            {
                suffix++;
            }

            ARow.FormName = "NEWFORM" + suffix.ToString();

            ARow.FormCode = String.Empty;
        }

        #endregion

        #region Event Handlers

        private void BrowseFileName(object Sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Word Documents (*.doc, *.dot,*.docx)|*.doc;*.dot;*.docx|Html Documents (*.htm, *.html)|*.htm;*.html|All Files|*.*";
            dlg.CheckFileExists = true;
            dlg.Title = "Select a Document";

            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            txtDetailFormFileName.Text = dlg.FileName;
        }

        private void FormCodeChanged(object Sender, EventArgs e)
        {
            // We only show the 'options' when the form code is 'RECEIPT'.
            bool doShowConditions = cmbDetailFormCode.SelectedIndex == 1;

            pnlConditions.Visible = doShowConditions;
            rgrGiftOptions.Visible = doShowConditions;
            rgrAdjustmentOptions.Visible = doShowConditions;
            chkAlwaysPrintNewDonor.Visible = doShowConditions;

            // We only show 'Standard' unless the form code is RECEIPT
            cmbDetailFormTypeCode.Filter =
                doShowConditions ? String.Empty : String.Format("{0}='{1}'",
                    AFrequencyTable.GetFrequencyCodeDBName(), MFinanceConstants.FORM_TYPE_CODE_STANDARD);

            // Force the user to select a value unless there is only one to choose from
            cmbDetailFormTypeCode.cmbCombobox.SelectedIndex = doShowConditions ? -1 : 0;
        }

        #endregion

        #region Manual method extensions

        private void ShowDetailsManual(AFormRow ARow)
        {
            // Handle the 'Options' which are in a comma separated list
            string s = (ARow == null) ? String.Empty : ARow.Options;

            string giftTypes = StringHelper.GetNextCSV(ref s);
            string adjustments = StringHelper.GetNextCSV(ref s);
            string chkValue = StringHelper.GetNextCSV(ref s);

            if ((String.Compare(MFinanceConstants.FORM_OPTION_ALL, giftTypes, true) == 0)
                || (giftTypes == String.Empty))
            {
                rbtAllGifts.Checked = true;
            }
            else if (String.Compare(MFinanceConstants.FORM_OPTION_GIFTS_ONLY, giftTypes, true) == 0)
            {
                rbtGiftsOnly.Checked = true;
            }
            else if (String.Compare(MFinanceConstants.FORM_OPTION_GIFT_IN_KIND_ONLY, giftTypes, true) == 0)
            {
                rbtGiftInKindOnly.Checked = true;
            }
            else if (String.Compare(MFinanceConstants.FORM_OPTION_OTHER, giftTypes, true) == 0)
            {
                rbtOther.Checked = true;
            }

            if ((String.Compare(MFinanceConstants.FORM_OPTION_ALL, adjustments, true) == 0)
                || (adjustments == String.Empty))
            {
                rbtAll.Checked = true;
            }
            else if (String.Compare(MFinanceConstants.FORM_OPTION_ADJUSTMENTS_ONLY, adjustments, true) == 0)
            {
                rbtAdjustmentsOnly.Checked = true;
            }
            else if (String.Compare(MFinanceConstants.FORM_OPTION_EXCLUDE_ADJUSTMENTS, adjustments, true) == 0)
            {
                rbtExcludeAdjustments.Checked = true;
            }

            chkAlwaysPrintNewDonor.Checked = chkValue == "yes";
        }

        private void GetDetailsFromControlsManual(AFormRow ARow)
        {
            // Handle the 'Options' which are in a comma separated list
            string options = String.Empty;

            if (rbtGiftsOnly.Checked)
            {
                options += (MFinanceConstants.FORM_OPTION_GIFTS_ONLY + ",");
            }
            else if (rbtGiftInKindOnly.Checked)
            {
                options += (MFinanceConstants.FORM_OPTION_GIFT_IN_KIND_ONLY + ",");
            }
            else if (rbtOther.Checked)
            {
                options += (MFinanceConstants.FORM_OPTION_OTHER + ",");
            }
            else
            {
                options += (MFinanceConstants.FORM_OPTION_ALL + ",");
            }

            if (rbtAdjustmentsOnly.Checked)
            {
                options += (MFinanceConstants.FORM_OPTION_ADJUSTMENTS_ONLY + ",");
            }
            else if (rbtExcludeAdjustments.Checked)
            {
                options += (MFinanceConstants.FORM_OPTION_EXCLUDE_ADJUSTMENTS + ",");
            }
            else
            {
                options += (MFinanceConstants.FORM_OPTION_ALL + ",");
            }

            options += (chkAlwaysPrintNewDonor.Checked) ? "yes" : "no";

            ARow.Options = options;
        }

        private void ValidateDataDetailsManual(AFormRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Setup.ValidateFormDesignManual(this,
                ARow,
                ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        #endregion
    }
}