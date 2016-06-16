//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
#region usings

using System;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        #region motivation group changed

        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            bool DoTaxUpdate;

            if (!FBatchUnpostedFlag
                || FPetraUtilsObject.SuppressChangeDetection
                || !FInEditModeFlag
                || txtDetailRecipientKeyMinistry.Visible)
            {
                DoTaxUpdate = false;
            }
            else
            {
                try
                {
                    FMotivationGroupChangedInProcess = true;

                    int originalMotivationDetailIndex = cmbMotivationDetailCode.SelectedIndex;

                    //Record new motivation group
                    FMotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
                    txtDetailMotivationDetailCode.Text = string.Empty;
                    txtMotivationDetailDesc.Text = string.Empty;

                    if (!FRecipientKeyChangedInProcess)
                    {
                        FMotivationDetail = string.Empty;
                    }

                    ApplyMotivationDetailCodeFilter(out DoTaxUpdate);

                    //The change event for the detail combo doesn't always fire as it should
                    if ((cmbMotivationDetailCode.Count > 1) && (originalMotivationDetailIndex > 0))
                    {
                        MotivationDetailChanged(null, null);
                    }
                }
                finally
                {
                    FMotivationGroupChangedInProcess = false;
                }
            }

            if (DoTaxUpdate)
            {
                EnableTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
                UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), FRecipientKeyChangedInProcess);
            }

            if (!FRecipientKeyChangedInProcess)
            {
                ValidateRecipientLedgerNumber();
            }
        }

        #endregion motivation group changed

        #region motivation detail control handling

        private void SetFocusToMotivationDetailCombo(object sender, EventArgs e)
        {
            cmbMotivationDetailCode.Focus();
        }

        private void ApplyMotivationDetailCodeFilter(out bool ADoTaxUpdate)
        {
            ADoTaxUpdate = false;

            //Refilter the combo with suppressed changes
            TFinanceControls.ChangeMotivationDetailListFilter(ref cmbMotivationDetailCode, FMotivationGroup, -1, FPetraUtilsObject);

            Int32 NumberOfFilteredMotivationDetails = cmbMotivationDetailCode.Count;

            if (FMotivationGroupChangedInProcess && (NumberOfFilteredMotivationDetails > 0))
            {
                //Highlight first item after refilter
                cmbMotivationDetailCode.SelectedIndex = 0;
                FMotivationDetail = cmbMotivationDetailCode.GetSelectedString();
                txtDetailMotivationDetailCode.Text = FMotivationDetail;
                txtMotivationDetailDesc.Text = cmbMotivationDetailCode.GetSelectedDescription();
            }
            else if (NumberOfFilteredMotivationDetails > 0)
            {
                cmbMotivationDetailCode.SetSelectedString(FMotivationDetail, -1);
                txtMotivationDetailDesc.Text = cmbMotivationDetailCode.GetSelectedDescription();
                ADoTaxUpdate = false;
            }
            else
            {
                //No motivation details added to this motivation group as yet
                cmbMotivationDetailCode.SelectedIndex = -1;
                txtDetailMotivationDetailCode.Text = string.Empty;
                txtMotivationDetailDesc.Text = string.Empty;
                FMotivationDetail = string.Empty;
            }

            //The following code will handle an empty motivation details combo box
            DisplayMotivationDetailAccountCode();

            if ((txtDetailRecipientKey.Text == string.Empty) || (Convert.ToInt64(txtDetailRecipientKey.Text) == 0))
            {
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                DisplayMotivationDetailCostCentreCode();
            }
        }

        private void ResetMotivationDetailCodeFilter(bool AShowGiftDetail)
        {
            bool AlreadySuppressingChanges = FPetraUtilsObject.SuppressChangeDetection;

            try
            {
                if (!AlreadySuppressingChanges)
                {
                    FPetraUtilsObject.SuppressChangeDetection = true;
                }

                //If motivation detail combobox is empty and with a valid filter
                if ((cmbMotivationDetailCode.Count == 0) && (cmbMotivationDetailCode.Filter != null)
                    && (!cmbMotivationDetailCode.Filter.Contains("1 = 2")))
                {
                    FMotivationDetail = string.Empty;
                    txtDetailMotivationDetailCode.Text = string.Empty;
                    txtMotivationDetailDesc.Text = string.Empty;

                    if (AShowGiftDetail)
                    {
                        //This is needed as the code in TFinanceControls.ChangeFilterMotivationDetailList looks for presence of the active only prefix
                        cmbMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true And 1 = 2";
                    }
                    else
                    {
                        cmbMotivationDetailCode.Filter = "1 = 2";
                    }

                    return;
                }

                if (AShowGiftDetail)
                {
                    cmbMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true";
                }
                else
                {
                    cmbMotivationDetailCode.Filter = string.Empty;
                }

                //After removing the filter, reselect correct value
                cmbMotivationDetailCode.SetSelectedString(FMotivationDetail);
            }
            finally
            {
                if (!AlreadySuppressingChanges)
                {
                    FPetraUtilsObject.SuppressChangeDetection = false;
                }
            }
        }

        private void ReconcileMotivationDetailFromCombo(GiftBatchTDSAGiftDetailRow ACurrentDetailRow)
        {
            if (FBatchUnpostedFlag && FInEditModeFlag)
            {
                string motivationDetail = string.Empty;
                string motivationDetailDesc = string.Empty;

                bool isEmptyDetailRow = (ACurrentDetailRow == null);

                if (!isEmptyDetailRow && (cmbMotivationDetailCode.SelectedIndex > -1))
                {
                    motivationDetail = cmbMotivationDetailCode.GetSelectedString();
                    motivationDetailDesc = cmbMotivationDetailCode.GetSelectedDescription();
                }

                FMotivationDetail = motivationDetail;
                txtDetailMotivationDetailCode.Text = motivationDetail;
                txtMotivationDetailDesc.Text = motivationDetailDesc;
            }
        }

        private void ReconcileMotivationDetailFromTextbox(GiftBatchTDSAGiftDetailRow ACurrentDetailRow)
        {
            if (FBatchUnpostedFlag && FInEditModeFlag)
            {
                bool isEmptyDetailRow = (ACurrentDetailRow == null);
                string motivationDetail = txtDetailMotivationDetailCode.Text;

                if (!isEmptyDetailRow && (motivationDetail.Length > 0))
                {
                    cmbMotivationDetailCode.SetSelectedString(motivationDetail);
                }
                else
                {
                    cmbMotivationDetailCode.SelectedIndex = -1;
                }

                txtMotivationDetailDesc.Text = cmbMotivationDetailCode.GetSelectedDescription();
            }
        }

        #endregion motivation detail control handling

        #region motivation detail changed

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            if (FMotivationDetailChangedInProcess
                || FPetraUtilsObject.SuppressChangeDetection
                || !FInEditModeFlag
                || !FBatchUnpostedFlag)
            {
                return;
            }

            try
            {
                FMotivationDetailChangedInProcess = true;

                bool doTaxUpdate;
                string currentMotivationDetailCode = cmbMotivationDetailCode.GetSelectedString();
                string prevAutoPopComment = FAutoPopComment;

                //Assign new value from the combobox
                FMotivationDetail = cmbMotivationDetailCode.GetSelectedString();
                txtMotivationDetailDesc.Text = cmbMotivationDetailCode.GetSelectedDescription();

                if ((txtDetailMotivationDetailCode.Text != FMotivationDetail)
                    || (FPreviouslySelectedDetailRow.MotivationDetailCode != FMotivationDetail))
                {
                    txtDetailMotivationDetailCode.Text = FMotivationDetail;
                    FPreviouslySelectedDetailRow.MotivationDetailCode = FMotivationDetail;
                }

                ProcessMotivationDetailRow(out doTaxUpdate);

                if (doTaxUpdate)
                {
                    bool DeductiblePercentageEnabled = txtDeductiblePercentage.Enabled;
                    EnableTaxDeductibilityPct(chkDetailTaxDeductible.Checked);

                    // if txtDeductiblePercentage has been enabled or disabled then update the percentage
                    if (DeductiblePercentageEnabled != txtDeductiblePercentage.Enabled)
                    {
                        UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), FRecipientKeyChangedInProcess);
                        UpdateTaxDeductibilityAmounts(null, null);
                    }
                }

                // if the previous motivation detail had a AutoPopDesc set to true then remove this comment for this detail
                if (!txtDetailRecipientKeyMinistry.Visible && !string.IsNullOrEmpty(prevAutoPopComment))
                {
                    RemoveAutoPopulatedComment(prevAutoPopComment);
                }

                // if motivation detail has AutoPopDesc set to true and has not already been autopoulated for this detail
                if (!txtDetailRecipientKeyMinistry.Visible
                    && !string.IsNullOrEmpty(FAutoPopComment) && (txtDetailGiftCommentOne.Text != FAutoPopComment))
                {
                    // autopopulate comment one with the motivation detail description
                    AutoPopulateCommentOne(FAutoPopComment);
                }
            }
            finally
            {
                FMotivationDetailChangedInProcess = false;
            }
        }

        private void ProcessMotivationDetailRow(out bool ADoTaxUpdate)
        {
            Int64 MotivationRecipientKey = 0;

            ADoTaxUpdate = false;

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = GetCurrentMotivationDetailRow();

                if (motivationDetail != null)
                {
                    DisplayMotivationDetailAccountCode(motivationDetail);

                    MotivationRecipientKey = motivationDetail.RecipientKey;

                    // if motivation detail autopopulation is set to true
                    if (motivationDetail.Autopopdesc)
                    {
                        FAutoPopComment = motivationDetail.MotivationDetailDesc;
                    }
                    else
                    {
                        FAutoPopComment = null;
                    }

                    // set tax deductible checkbox if motivation detail has been changed by the user (i.e. not a row change)
                    if (!FPetraUtilsObject.SuppressChangeDetection || FRecipientKeyChangedInProcess)
                    {
                        if (chkDetailTaxDeductible.Checked != motivationDetail.TaxDeductible)
                        {
                            chkDetailTaxDeductible.Checked = motivationDetail.TaxDeductible;
                        }
                    }

                    if (FSETUseTaxDeductiblePercentageFlag)
                    {
                        if (string.IsNullOrEmpty(motivationDetail.TaxDeductibleAccountCode))
                        {
                            MessageBox.Show(Catalog.GetString("This Motivation Detail does not have an associated Tax Deductible Account. " +
                                    "This can be added in Finance / Setup / Motivation Details.\n\n" +
                                    "Unless this is changed it will be impossible to assign a Tax Deductible Percentage to this gift."),
                                Catalog.GetString("Incomplete Motivation Detail"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        ADoTaxUpdate = true;
                    }
                }
                else
                {
                    chkDetailTaxDeductible.Checked = false;
                }
            }

            if (!FNewGiftInProcess && !FAutoPopulatingGiftInProcess && (MotivationRecipientKey > 0))
            {
                FMotivationDetailHasChangedFlag = true;
                PopulateKeyMinistry(MotivationRecipientKey, FMotivationDetailHasChangedFlag);
                FMotivationDetailHasChangedFlag = false;
            }
            else if (FRecipientKey == 0)
            {
                UpdateRecipientKeyText(0, FPreviouslySelectedDetailRow, FMotivationGroup, FMotivationDetail);
            }

            if (FRecipientKey == 0)
            {
                DisplayMotivationDetailCostCentreCode();
            }
            else
            {
                string NewCCCode = string.Empty;

                // it is possible that there are no active motivation details and so AMotivationDetail is blank
                if (!string.IsNullOrEmpty(FMotivationDetail))
                {
                    bool partnerIsMissingLink = false;

                    NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FLedgerNumber,
                        FRecipientKey,
                        FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                        FPreviouslySelectedDetailRow.DateEntered,
                        FMotivationGroup,
                        FMotivationDetail,
                        out partnerIsMissingLink);
                }

                if (txtDetailCostCentreCode.Text != NewCCCode)
                {
                    txtDetailCostCentreCode.Text = NewCCCode;
                }
            }
        }

        #endregion motivation detail changed

        #region motivation detail data

        private AMotivationDetailRow GetCurrentMotivationDetailRow()
        {
            AMotivationDetailRow MotivationDetailRow = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

            return MotivationDetailRow;
        }

        private void DisplayMotivationDetailAccountCode(AMotivationDetailRow AMotivationDetail = null)
        {
            string AcctCode = string.Empty;
            string TaxDeductibleAcctCode = string.Empty;

            if (FMotivationDetail.Length > 0)
            {
                if (AMotivationDetail == null)
                {
                    AMotivationDetail = GetCurrentMotivationDetailRow();
                }

                if (AMotivationDetail != null)
                {
                    AcctCode = AMotivationDetail.AccountCode;

                    if (FSETUseTaxDeductiblePercentageFlag)
                    {
                        TaxDeductibleAcctCode = AMotivationDetail.TaxDeductibleAccountCode;
                    }
                }
            }

            if (txtDetailAccountCode.Text != AcctCode)
            {
                txtDetailAccountCode.Text = AcctCode;
            }

            if (FSETUseTaxDeductiblePercentageFlag && (txtDeductibleAccount.Text != TaxDeductibleAcctCode))
            {
                txtDeductibleAccount.Text = TaxDeductibleAcctCode;
            }
        }

        private void DisplayMotivationDetailCostCentreCode()
        {
            string CostCentreCode = string.Empty;

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = GetCurrentMotivationDetailRow();

                if (motivationDetail != null)
                {
                    CostCentreCode = motivationDetail.CostCentreCode.ToString();
                }
            }

            if (txtDetailCostCentreCode.Text != CostCentreCode)
            {
                txtDetailCostCentreCode.Text = CostCentreCode;
            }
        }

        #endregion motivation detail data
    }
}