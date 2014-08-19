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
using System.Threading;
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
                    
                    Thread thread = new Thread(SetupComboboxes);
                    thread.Start();
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

        /// <summary>
        /// TODO might need this
        /// </summary>
        /*public AllocationJournalTDS MainDS
        {
            get
            {
            	return FMainDS;
            }
            set
            {
            	FMainDS = value;
            }
        }*/

        private void InitializeManualCode()
        {
            rbtPercentageOption.Checked = true;

            // set currency codes
            txtTotalAmount.CurrencyCode = FTransactionCurrency;
            txtDetailAmount.CurrencyCode = FTransactionCurrency;

            // disallow negative numbers
            txtTotalAmount.NegativeValueAllowed = false;
            txtDetailAmount.NegativeValueAllowed = false;
            txtDetailPercentage.NegativeValueAllowed = false;

            // correct label position which doesn't get moved when using padding
            lblTotalAmount.Location = new System.Drawing.Point(lblTotalAmount.Location.X, txtTotalAmount.Location.Y + 5);

            // correct this radio group hiding another control
            rgrDebitCredit.SendToBack();
            
            // ok button disabled until two allocations are added
            btnOK.Enabled = false;

            // TODO tmp hardcoded test data
            txtTotalAmount.NumberValueDecimal = 100;
            DataRow NewRow = FMainDS.Allocations.NewRow();
            NewRow["Percentage"] = 25.00;
            NewRow["Amount"] = 25.00;
            NewRow["a_cost_centre_code_c"] = "2600";
            NewRow["a_account_code_c"] = "0407";
            FMainDS.Allocations.Rows.Add(NewRow);
            NewRow = FMainDS.Allocations.NewRow();
            NewRow["Percentage"] = 45.00;
            NewRow["Amount"] = 45.00;
            NewRow["a_cost_centre_code_c"] = "8042";
            NewRow["a_account_code_c"] = "9800";
            FMainDS.Allocations.Rows.Add(NewRow);
            btnOK.Enabled = true;
        }
        
        private void SetupComboboxes()
        {
            if (FLedgerNumber != -1)
            {
            	// populate combo boxes
                TFinanceControls.InitialiseCostCentreList(ref cmbFromCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                TFinanceControls.InitialiseAccountList(ref cmbFromAccountCode, FLedgerNumber,
                    true, false, FActiveOnly, false, FTransactionCurrency, true);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, FActiveOnly, false, FTransactionCurrency, true);
            }
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
        	// Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        	FPetraUtilsObject.VerificationResultCollection.Clear();
        	
            FValidateEverything = true;

            if (ValidateAllData(false, true))
            {
            	this.DialogResult = DialogResult.OK;
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
            
	            if (grdDetails.Rows.Count > 2)
	            {
	            	btnOK.Enabled = true;
	            }
            }
        }

        // update allocation percentages or amounts when the total 'from' amount is changed
        private void TotalAmountChanged(Object Sender, EventArgs e)
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

            txtDetailPercentage.NumberValueDecimal = (txtDetailAmount.NumberValueDecimal / txtTotalAmount.NumberValueDecimal) * 100;

            this.txtDetailPercentage.TextChanged += new System.EventHandler(this.PercentageChanged);
        }

        private void PercentageChanged(Object Sender, EventArgs e)
        {
            this.txtDetailAmount.TextChanged -= new System.EventHandler(this.AmountChanged);

            txtDetailAmount.NumberValueDecimal = (txtDetailPercentage.NumberValueDecimal / 100) * txtTotalAmount.NumberValueDecimal;

            this.txtDetailAmount.TextChanged += new System.EventHandler(this.AmountChanged);
        }

        // delete highlighted row/s
        private void DeleteRecord(Object Sender, EventArgs e)
        {
            this.DeleteAllocations();
            
            if (grdDetails.Rows.Count <= 2)
            {
            	btnOK.Enabled = false;
            }
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
                
                btnOK.Enabled = false;
            }
        }

        #endregion

        #region Validation

        private void ValidateDataDetailsManual(AllocationJournalTDSAllocationsRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GL.ValidateAllocationJournalDialog(this, ARow, rbtAmountOption.Checked, txtTotalAmount.NumberValueDecimal,
                                                                        ref VerificationResultCollection, FPetraUtilsObject.ValidationControlsDict);
 
            if (VerificationResultCollection.Count == 0 && FValidateEverything)
            {
                ValidateEverything();
            }
        }

        /// validate all data not in a DataRow
        private void ValidateEverything()
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            TScreenVerificationResult VerificationResult;

            // Validate Reference
            if (!string.IsNullOrEmpty(txtReference.Text) && txtReference.Text.Length > 100)
            {
                // 'Reference' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtReference.Text, 100,
                    "Reference", txtReference), null, txtReference);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtReference, VerificationResult, null);
            }
            else if (string.IsNullOrEmpty(txtReference.Text))
            {
            	// 'Reference' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(txtReference.Text,
                    "Reference", txtReference), null, txtReference);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtReference, VerificationResult, null);
            }
            
            // Validate Narrative
            if (!string.IsNullOrEmpty(txtNarrative.Text) && txtNarrative.Text.Length > 500)
            {
                // 'Narrative' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtNarrative.Text, 500,
                    "Narrative", txtNarrative), null, txtNarrative);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtNarrative, VerificationResult, null);
            }
            else if (string.IsNullOrEmpty(txtNarrative.Text))
            {
            	// 'Narrative' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(txtNarrative.Text,
                    "Narrative", txtNarrative), null, txtNarrative);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtNarrative, VerificationResult, null);
            }
            
            // Validate FromCostCentreCode
            if (string.IsNullOrEmpty(cmbFromCostCentreCode.GetSelectedString()))
            {
            	// 'Cost Centre Code' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(cmbFromCostCentreCode.Text,
                    "Cost Centre Code", cmbFromCostCentreCode), null, cmbFromCostCentreCode);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(cmbFromCostCentreCode, VerificationResult, null);
            }
            
            // Validate FromAccountCode
            if (string.IsNullOrEmpty(cmbFromAccountCode.GetSelectedString()))
            {
            	// 'Account Code' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(cmbFromAccountCode.Text,
                    "Account Code", cmbFromAccountCode), null, cmbFromAccountCode);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(cmbFromAccountCode, VerificationResult, null);
            }
            
            // Validate TotalAmount
            if (Convert.ToDecimal(txtTotalAmount.Text) <= 0)
            {
            	// 'Account Code' must not be empty
            	VerificationResult = new TScreenVerificationResult(TNumericalChecks.IsPositiveDecimal(Convert.ToDecimal(txtTotalAmount.Text),
                    "Amount", txtTotalAmount), null, txtTotalAmount);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtTotalAmount, VerificationResult, null);
            }
            else
            {
            	// Validate Allocations' amounts
            	if (rbtAmountOption.Checked)
            	{
            		decimal TotalAmountInAllocations = 0;
            		
            		foreach (AllocationJournalTDSAllocationsRow Row in FMainDS.Allocations.Rows)
            		{
            			TotalAmountInAllocations += Row.Amount;
            		}
            		
            		if (TotalAmountInAllocations != Convert.ToDecimal(txtTotalAmount.Text))
            		{
            			VerificationResult = new TScreenVerificationResult(this, null,
                           Catalog.GetString("The amounts entered do not match the total amount of the Allocation. Please check the amounts entered."),
                           txtTotalAmount, TResultSeverity.Resv_Critical);

		                // Handle addition/removal to/from TVerificationResultCollection
		                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtTotalAmount, VerificationResult, null);
            		}
            	}
            	// Validate Allocations' percentages
            	else
            	{
            		decimal TotalPercentageInAllocations = 0;
            		
            		foreach (AllocationJournalTDSAllocationsRow Row in FMainDS.Allocations.Rows)
            		{
            			TotalPercentageInAllocations += Row.Percentage;
            		}
            		
            		if (TotalPercentageInAllocations != 100)
            		{
            			VerificationResult = new TScreenVerificationResult(this, null,
                           Catalog.GetString("The percentages entered must add up to 100%."),
                           txtTotalAmount, TResultSeverity.Resv_Critical);

		                // Handle addition/removal to/from TVerificationResultCollection
		                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtDetailPercentage, VerificationResult, null);
            		}
            	}
            }
        }

        #endregion
    }
}