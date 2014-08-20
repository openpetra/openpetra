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
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        private GLBatchTDSAJournalRow FJournal = null;
        private int FNextTransactionNumber = -1;
        
        /// <summary>
        /// The Journal that this allocation will be added to.
        /// </summary>
        public GLBatchTDSAJournalRow Journal
        {
        	set
        	{
        		FJournal = value;

	            // set currency codes
	            txtTotalAmount.CurrencyCode = FJournal.TransactionCurrency;
	            txtDetailTransactionAmount.CurrencyCode = FJournal.TransactionCurrency;
	            
                if (FLedgerNumber != FJournal.LedgerNumber)
                {
                    FLedgerNumber = FJournal.LedgerNumber;
                    txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FJournal.LedgerNumber);
                    
                    Thread thread = new Thread(SetupComboboxes);
                    thread.Start();
                }
                
                txtBatchNumber.Text = FJournal.BatchNumber.ToString();
                
                // LastTransactionNumber + 1 is reserved for 'from' allocation
                FNextTransactionNumber = FJournal.LastTransactionNumber + 2;
        	}
        }

        /// <summary>
        /// </summary>
        public GLBatchTDS MainDS
        {
            get
            {
            	return FMainDS;
            }
        }

        #region Setup
        
        private void InitializeManualCode()
        {
            rbtPercentageOption.Checked = true;

            // disallow negative numbers
            txtTotalAmount.NegativeValueAllowed = false;
            txtDetailTransactionAmount.NegativeValueAllowed = false;
            txtDetailPercentage.NegativeValueAllowed = false;

            // correct label position which doesn't get moved when using padding
            lblTotalAmount.Location = new System.Drawing.Point(lblTotalAmount.Location.X, txtTotalAmount.Location.Y + 5);

            // correct this radio group hiding another control
            rgrDebitCredit.SendToBack();
            
            // ok button disabled until two allocations are added
            btnOK.Enabled = false;
        }
        
        private void RunOnceOnActivationManual()
        {
        	// Stop changes from ever being detected. We do not want to save the data on this screen.
        	FPetraUtilsObject.DisableSaveButton();
            FPetraUtilsObject.UnhookControl(this, true);
        }
        
        private void SetupComboboxes()
        {
            if (FLedgerNumber != -1)
            {
            	FPetraUtilsObject.SuppressChangeDetection = true;
            	
            	// populate combo boxes
                TFinanceControls.InitialiseCostCentreList(ref cmbFromCostCentreCode, FLedgerNumber, true, false, true, false);
                TFinanceControls.InitialiseAccountList(ref cmbFromAccountCode, FLedgerNumber,
                    true, false, true, false, FJournal.TransactionCurrency, true);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, true, false);
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, true, false, FJournal.TransactionCurrency, true);
            }
        }
        
        #endregion

        private void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            ANewRow.LedgerNumber = FJournal.LedgerNumber;
            ANewRow.BatchNumber = FJournal.BatchNumber;
            ANewRow.JournalNumber = FJournal.JournalNumber;
            ANewRow.TransactionNumber = FNextTransactionNumber;
            ANewRow.TransactionDate = FJournal.DateEffective;
            ANewRow.CostCentreCode = System.DBNull.Value.ToString();
            ANewRow.AccountCode = System.DBNull.Value.ToString();
            ANewRow.Percentage = 0;
            FNextTransactionNumber++;
        }

        private void ShowDetailsManual(GLBatchTDSATransactionRow ARow)
        {
            btnDeleteAll.Enabled = pnlDetails.Enabled;
        }

        #region Events

        bool FValidateEverything = false;

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
        	// enables extra validation (i.e. data not in grid)
            FValidateEverything = true;

            if (ValidateAllData(false, true))
            {
            	// Create the transaction to take the given amount OUT of the "allocate from" account & cost centre.
            	GLBatchTDSATransactionRow NewRow = FMainDS.ATransaction.NewRowTyped(true);
            	NewRow.LedgerNumber = FJournal.LedgerNumber;
	            NewRow.BatchNumber = FJournal.BatchNumber;
	            NewRow.JournalNumber = FJournal.JournalNumber;
	            NewRow.TransactionNumber = FJournal.LastTransactionNumber + 1;
	            NewRow.TransactionDate = FJournal.DateEffective;
	            NewRow.CostCentreCode = cmbFromCostCentreCode.GetSelectedString();
	            NewRow.AccountCode = cmbFromAccountCode.GetSelectedString();
	            NewRow.DebitCreditIndicator = rbtDebit.Checked;
	            NewRow.TransactionAmount = Convert.ToDecimal(txtTotalAmount.Text);
	            NewRow.Reference = txtFromReference.Text;
	            
	            if (string.IsNullOrEmpty(txtFromNarrative.Text))
	            {
	            	txtFromNarrative.Text = Catalog.GetString("Allocation") + ": " + NewRow.CostCentreCode + "-" + NewRow.AccountCode;
	            }
	            
	            NewRow.Narrative = txtFromNarrative.Text;
	            
	            // add DebitCreditIndicator, Narrative and Reference to each row in grid
        		DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

                for (int i = dv.Count - 1; i >= 0; i--)
                {
                	GLBatchTDSATransactionRow Row = (GLBatchTDSATransactionRow) dv[i].Row;
                	Row.DebitCreditIndicator = !rbtDebit.Checked;
                	Row.Narrative = txtFromNarrative.Text;
	            	Row.Reference = txtFromReference.Text;
                }
                
	            FMainDS.ATransaction.Rows.Add(NewRow);
            	
            	this.DialogResult = DialogResult.OK;
            	FPetraUtilsObject.DisableSaveButton();
                Close();
            }
        	
            // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        	FPetraUtilsObject.VerificationResultCollection.Clear();

            FValidateEverything = false;
        }

        // This does nothing. We do not actually want to save any data here.
        private TSubmitChangesResult StoreManualCode(ref GLBatchTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            return TSubmitChangesResult.scrOK;
        }

        private void NewRow(Object Sender, EventArgs e)
        {
            if (CreateNewATransaction())
            {
                txtDetailTransactionAmount.NumberValueDecimal = 0;
                txtDetailPercentage.NumberValueDecimal = 0;
            
	            if (grdDetails.Rows.Count > 2)
	            {
	            	btnOK.Enabled = true;
	            }
            }
            
            FPetraUtilsObject.DisableSaveButton();
        }

        // update allocation percentages or amounts when the total 'from' amount is changed
        private void TotalAmountChanged(Object Sender, EventArgs e)
        {
            if (txtDetailTransactionAmount.Enabled)
            {
                UpdatePercentages(grdDetails.Rows.Count <= 2);
            }
            else
            {
            	UpdateAmounts(grdDetails.Rows.Count <= 2);
            }
        }

        // radio selection has changed
        private void AmountPercentageChanged(Object Sender, EventArgs e)
        {
            txtDetailTransactionAmount.Enabled = rbtAmountOption.Checked;
            txtDetailPercentage.Enabled = rbtPercentageOption.Checked;
            TotalAmountChanged(Sender, e);
        }

        private void AmountChanged(Object Sender, EventArgs e)
        {
        	// update percentage for either current row or all rows
        	UpdatePercentages(GetAmountTotal() != Convert.ToDecimal(txtTotalAmount.Text));
        }

        private void PercentageChanged(Object Sender, EventArgs e)
        {
        	// update amount for either current row or all rows
            UpdateAmounts(GetPercentageTotal() != 100);
        }

        // delete highlighted row/s
        private void DeleteRecord(Object Sender, EventArgs e)
        {
            this.DeleteATransaction();
            
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
        
        #region Helper Methods
        
        private void UpdatePercentages(bool ACurrentRowOnly)
        {
            this.txtDetailPercentage.TextChanged -= new System.EventHandler(this.PercentageChanged);
            
            // only currently selected row needs updating
        	if (ACurrentRowOnly)
        	{
        		if (txtTotalAmount.NumberValueDecimal != 0)
        		{
        			txtDetailPercentage.NumberValueDecimal = (txtDetailTransactionAmount.NumberValueDecimal / txtTotalAmount.NumberValueDecimal) * 100;
        		}
        		else
        		{
        			txtDetailPercentage.NumberValueDecimal = 0;
        		}
        	}
        	// all rows need updating
        	else
        	{
        		List<GLBatchTDSATransactionRow> AllocationList = new List<GLBatchTDSATransactionRow>();
        		
        		foreach (GLBatchTDSATransactionRow Row in MainDS.ATransaction.Rows)
        		{
        			if (txtTotalAmount.NumberValueDecimal != 0)
        			{
        				Row.Percentage = decimal.Round((Row.TransactionAmount / ((decimal) txtTotalAmount.NumberValueDecimal)) * 100, 2);
	        		}
	        		else
	        		{
	        			Row.Percentage = 0;
	        		}
	        		
        			AllocationList.Add(Row);
        		}
        		
        		// fix rounding error
        		if (GetAmountTotal() == txtTotalAmount.NumberValueDecimal && GetPercentageTotal() != 100 && txtTotalAmount.NumberValueDecimal != 0)
        		{
        			decimal Difference = 100 - GetPercentageTotal();
        			
        			// sort list by amount sizes
        			AllocationList = AllocationList.OrderByDescending(o=>o.Percentage).ToList();
        			
        			if (Difference < 0)
        			{
        				int Index = 0;
        				
        				while (Difference != 0)
        				{
        					AllocationList[Index].Percentage -= (decimal) 0.01;
        					Difference += (decimal) 0.01;
        					Index++;
        				}
        			}
        			else if (Difference > 0)
        			{
        				int Index = 0;
        				
        				while (Difference != 0)
        				{
        					AllocationList[Index].Percentage += (decimal) 0.01;
        					Difference -= (decimal) 0.01;
        					Index++;
        				}
        			}
        		}
        	}

            this.txtDetailPercentage.TextChanged += new System.EventHandler(this.PercentageChanged);
        }
        
        private void UpdateAmounts(bool ACurrentRowOnly)
        {
            this.txtDetailTransactionAmount.TextChanged -= new System.EventHandler(this.AmountChanged);
            
            // only currently selected row needs updating
        	if (ACurrentRowOnly)
        	{
        		txtDetailTransactionAmount.NumberValueDecimal = (txtDetailPercentage.NumberValueDecimal / 100) * txtTotalAmount.NumberValueDecimal;
        	}
        	// all rows need updating
        	else
        	{
        		List<GLBatchTDSATransactionRow> AllocationList = new List<GLBatchTDSATransactionRow>();
        		
        		foreach (GLBatchTDSATransactionRow Row in MainDS.ATransaction.Rows)
        		{
        			Row.TransactionAmount = decimal.Round((Row.Percentage / 100) * ((decimal) txtTotalAmount.NumberValueDecimal), 2);
        			AllocationList.Add(Row);
        		}
        		
        		// fix rounding error
        		if (GetPercentageTotal() == 100 && GetAmountTotal() != txtTotalAmount.NumberValueDecimal && txtTotalAmount.NumberValueDecimal != 0)
        		{
        			decimal Difference = (decimal) txtTotalAmount.NumberValueDecimal - GetAmountTotal();
        			
        			// sort list by amount sizes
        			AllocationList = AllocationList.OrderByDescending(o=>o.TransactionAmount).ToList();
        			
        			if (Difference < 0)
        			{
        				int Index = 0;
        				
        				while (Difference != 0)
        				{
        					AllocationList[Index].TransactionAmount -= (decimal) 0.01;
        					Difference += (decimal) 0.01;
        					Index++;
        				}
        			}
        			else if (Difference > 0)
        			{
        				int Index = 0;
        				
        				while (Difference != 0)
        				{
        					AllocationList[Index].TransactionAmount += (decimal) 0.01;
        					Difference -= (decimal) 0.01;
        					Index++;
        				}
        			}
        		}
        	}

            this.txtDetailTransactionAmount.TextChanged += new System.EventHandler(this.AmountChanged);
        }
        
        // calculates total amount in rows
        private decimal GetAmountTotal()
        {
    		decimal TotalAmountInAllocations = 0;
            		
    		DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = dv.Count - 1; i >= 0; i--)
            {
            	TotalAmountInAllocations += ((GLBatchTDSATransactionRow) dv[i].Row).TransactionAmount;
            }
            
            return TotalAmountInAllocations;
        }
        
        // calculates total percentage in rows
        private decimal GetPercentageTotal()
        {
    		decimal TotalPercentageInAllocations = 0;
    		
    		DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = dv.Count - 1; i >= 0; i--)
            {
            	TotalPercentageInAllocations += ((GLBatchTDSATransactionRow) dv[i].Row).Percentage;
            }
            
            return TotalPercentageInAllocations;
        }
        
        private bool CanCloseManual()
        {
        	// if 'Cancel' button has been clicked then ask the user if they really want to close the screen.
        	if (FMainDS.HasChanges()
        	   && this.DialogResult != DialogResult.OK
        	   && MessageBox.Show(Catalog.GetString("Are you sure you want to cancel this Allocation?"), 
        		                Catalog.GetString("Allocation Journal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        		                == DialogResult.No)
        	{
        		return false;
        	}
        	
        	return true;
        }
        
        #endregion

        #region Validation

        private void ValidateDataDetailsManual(GLBatchTDSATransactionRow ARow)
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
            if (!string.IsNullOrEmpty(txtFromReference.Text) && txtFromReference.Text.Length > 100)
            {
                // 'Reference' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtFromReference.Text, 100,
                    "Reference", txtFromReference), null, txtFromReference);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromReference, VerificationResult, null);
            }
            else if (string.IsNullOrEmpty(txtFromReference.Text))
            {
            	// 'Reference' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(txtFromReference.Text,
                    "Reference", txtFromReference), null, txtFromReference);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromReference, VerificationResult, null);
            }
            
            // Validate Narrative
            if (!string.IsNullOrEmpty(txtFromNarrative.Text) && txtFromNarrative.Text.Length > 500)
            {
                // 'Narrative' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtFromNarrative.Text, 500,
                    "Narrative", txtFromNarrative), null, txtFromNarrative);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromNarrative, VerificationResult, null);
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
            		if (GetAmountTotal() != Convert.ToDecimal(txtTotalAmount.Text))
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
            		if (GetPercentageTotal() != 100)
            		{
            			VerificationResult = new TScreenVerificationResult(this, null,
                           Catalog.GetString("The percentages entered must add up to 100%."),
                           txtDetailPercentage, TResultSeverity.Resv_Critical);

		                // Handle addition/removal to/from TVerificationResultCollection
		                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtDetailPercentage, VerificationResult, null);
            		}
            	}
            }
            
            if (grdDetails.Rows.Count <=2)
            {
            	VerificationResult = new TScreenVerificationResult(this, null,
                   Catalog.GetString("You must include at least 2 destination allocations."),
                   btnNew, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(btnNew, VerificationResult, null);
            }
            else if (grdDetails.Rows.Count > 11)
            {
            	VerificationResult = new TScreenVerificationResult(this, null,
                   Catalog.GetString("You must include no more than 10 destination allocations."),
                   btnDeleteAllocation, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(btnDeleteAllocation, VerificationResult, null);
            }
        }

        #endregion
    }
}