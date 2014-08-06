//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2013 by OM International
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

using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmAllocationJournalDialog
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private bool FActiveOnly = true;
        // TODO tmp hardcoding
        private string FTransactionCurrency = "USD";

        /// <summary>
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                if (FLedgerNumber != value)
                {
                    FLedgerNumber = value;
                    txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                    // populate combo boxes
                    TFinanceControls.InitialiseCostCentreList(ref cmbFromCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                    TFinanceControls.InitialiseAccountList(ref cmbFromAccountCode, FLedgerNumber,
                        true, false, FActiveOnly, false, FTransactionCurrency, true);
                    TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, FActiveOnly, false, FTransactionCurrency, true);

                    // can delete this when not using hardcoded data
                    ShowDetails(FPrevRowChangedRow);
                }
            }
        }

        /// <summary>
        /// </summary>
        public Int32 BatchNumber
        {
            set
            {
                if (FBatchNumber != value)
                {
                    FBatchNumber = value;
                    txtBatchNumber.Text = FBatchNumber.ToString();
                }
            }
        }

        private void InitializeManualCode()
        {
            rbtPercentageOption.Checked = true;

            // set currency codes
            txtFromAmount.CurrencyCode = FTransactionCurrency;
            txtDetailAmount.CurrencyCode = FTransactionCurrency;

            // disallow negative numbers
            txtFromAmount.NegativeValueAllowed = false;
            txtDetailAmount.NegativeValueAllowed = false;
            txtDetailPercentage.NegativeValueAllowed = false;

            // correct label position which doesn't get moved when using padding
            lblFromAmount.Location = new System.Drawing.Point(lblFromAmount.Location.X, txtFromAmount.Location.Y + 5);

            // correct this radio group hiding another control
            rgrDebitCredit.SendToBack();

            // TODO tmp hardcoded test data
            txtFromAmount.NumberValueDecimal = 100;
            DataRow NewRow = FMainDS.Allocations.NewRow();
            NewRow["Percentage"] = 25;
            NewRow["Amount"] = 15.25;
            NewRow["a_cost_centre_code_c"] = "2600";
            NewRow["a_account_code_c"] = "0407";
            FMainDS.Allocations.Rows.Add(NewRow);
        }

        private void NewRowManual(ref AllocationJournalTDSAllocationsRow ANewRow)
        {
            ANewRow.CostCentreCode = System.DBNull.Value.ToString();
            ANewRow.AccountCode = System.DBNull.Value.ToString();
        }

        private void ShowDetailsManual(AllocationJournalTDSAllocationsRow ARow)
        {
            btnDeleteAll.Enabled = pnlDetails.Enabled;
        }

        #region Events

        bool FValidateEverything = false;

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            FValidateEverything = true;

            if (ValidateAllData(false, true))
            {
                Close();
            }

            FValidateEverything = false;
        }

        // This does nothing yet.
        private TSubmitChangesResult StoreManualCode(ref AllocationJournalTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            return TSubmitChangesResult.scrOK;
        }

        private void NewRow(Object Sender, EventArgs e)
        {
            if (CreateNewAllocations())
            {
                txtDetailAmount.NumberValueDecimal = 0;
                txtDetailPercentage.NumberValueDecimal = 0;
            }
        }

        // update allocation percentages or amounts when the total 'from' amount is changed
        private void FromAmountChanged(Object Sender, EventArgs e)
        {
            if (txtDetailAmount.Enabled)
            {
                AmountChanged(Sender, e);
            }
            else
            {
                PercentageChanged(Sender, e);
            }
        }

        // radio selection has changed
        private void AmountPercentageChanged(Object Sender, EventArgs e)
        {
            txtDetailAmount.Enabled = rbtAmountOption.Checked;
            txtDetailPercentage.Enabled = rbtPercentageOption.Checked;
        }

        private void AmountChanged(Object Sender, EventArgs e)
        {
            this.txtDetailPercentage.TextChanged -= new System.EventHandler(this.PercentageChanged);

            txtDetailPercentage.NumberValueDecimal = (txtDetailAmount.NumberValueDecimal / txtFromAmount.NumberValueDecimal) * 100;

            this.txtDetailPercentage.TextChanged += new System.EventHandler(this.PercentageChanged);
        }

        private void PercentageChanged(Object Sender, EventArgs e)
        {
            this.txtDetailAmount.TextChanged -= new System.EventHandler(this.AmountChanged);

            txtDetailAmount.NumberValueDecimal = (txtDetailPercentage.NumberValueDecimal / 100) * txtFromAmount.NumberValueDecimal;

            this.txtDetailAmount.TextChanged += new System.EventHandler(this.AmountChanged);
        }

        // delete highlighted row/s
        private void DeleteRecord(Object Sender, EventArgs e)
        {
            this.DeleteAllocations();
        }

        // delete all rows
        private void DeleteAllAllocations(Object Sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all allocations.\n\nDo you really want to continue?")),
                     Catalog.GetString("Confirm Deletion"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

                for (int i = dv.Count - 1; i >= 0; i--)
                {
                    dv[i].Delete();
                }

                SelectRowInGrid(1);
            }
        }

        #endregion

        #region Validation

        private void ValidateDataDetailsManual(AllocationJournalTDSAllocationsRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            if (TSharedFinanceValidation_GL.ValidateAllocationJournalDialog(this, ARow, rbtAmountOption.Checked, txtFromAmount.NumberValueDecimal,
                    ref VerificationResultCollection, FPetraUtilsObject.ValidationControlsDict)
                && FValidateEverything)
            {
                ValidateEverything();
            }
        }

        // validate all data (even data not in DataRow)
        private void ValidateEverything()
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GL.ValidateAllocationJournalDialogEverything(this, txtReference,
                ref VerificationResultCollection, FPetraUtilsObject.ValidationControlsDict);
        }

        #endregion
    }
}