//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Validation;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Controllers;
using System.ComponentModel;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private Int32 FTransactionNumber = -1;
        private string FTransactionCurrency = string.Empty;
        private string FBatchStatus = string.Empty;
        private string FJournalStatus = string.Empty;
		private GLSetupTDS FCacheDS = null;
        private SourceGrid.Cells.Editors.ComboBox FAnalAttribTypeVal;
        private ATransAnalAttribRow FPSAttributesRow = null;
        private bool FAttributesGridEntered = false;

        private ABatchRow FBatchRow = null;

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AForeignCurrencyName"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AJournalStatus"></param>
        public void LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string AForeignCurrencyName,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            string AJournalStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            FBatchRow = GetBatchRow();

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionCurrency == AForeignCurrencyName) && (FBatchStatus == ABatchStatus) && (FJournalStatus == AJournalStatus)
                && (FMainDS.ATransaction.DefaultView.Count > 0))
            {
                //Same as previously selected
                if ((FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (grdDetails.SelectedRowIndex() > 0))
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                return;
            }

            bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != AForeignCurrencyName);

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            FTransactionCurrency = AForeignCurrencyName;
            FBatchStatus = ABatchStatus;
            FJournalStatus = AJournalStatus;

            FPreviouslySelectedDetailRow = null;

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            FMainDS.ATransaction.DefaultView.RowFilter = string.Empty;

            if (FMainDS.ATransaction.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(ALedgerNumber, ABatchNumber, AJournalNumber));
            }
            else
            {
                FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                    ATransactionTable.GetLedgerNumberDBName(),
                    ATransactionTable.GetBatchNumberDBName(),
                    ATransactionTable.GetJournalNumberDBName()
                    );

                if (FMainDS.ATransaction.DefaultView.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber }) == -1)
                {
                    FMainDS.ATransaction.Clear();
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(ALedgerNumber, ABatchNumber, AJournalNumber));
                }
                
                
            }

            FMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} ASC",
                ATransactionTable.GetTransactionNumberDBName()
                );

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);

			//Load all analysis attributes for all transactions in this Journal if necessary
            if (FMainDS.ATransAnalAttrib.DefaultView.Count == 0)
            {
                //Load from Server for the whole journal
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForJournal(FLedgerNumber, FBatchNumber, FJournalNumber));
            }

			grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransAnalAttrib.DefaultView);
			FAnalAttribTypeVal = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
			FAnalAttribTypeVal.EnableEdit = true;
			grdAnalAttributes.AddTextColumn("Value", FMainDS.ATransAnalAttrib.Columns[ATransAnalAttribTable.GetAnalysisAttributeValueDBName()], 100, FAnalAttribTypeVal);
			FAnalAttribTypeVal.Control.SelectedValueChanged += new EventHandler(this.AnalysisAttributeValueChanged);
			grdAnalAttributes.Columns[0].Width = 100;

			//Load all analysis attribute values
			if (FCacheDS == null)
			{
				FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber);
			}


            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            if (requireControlSetup)
            {
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, ActiveOnly, false, AForeignCurrencyName);
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
            }

            ShowData();
            ShowDetails();

            btnNew.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;
            btnRemove.Enabled = !FPetraUtilsObject.DetailProtectedMode && FJournalStatus == MFinanceConstants.BATCH_UNPOSTED;

            //This will update Batch and journal totals
            UpdateTotals();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                pnlDetails.Enabled = false;
            }
        }

        /// <summary>
        /// Unload the currently loaded transactions
        /// </summary>
        public void UnloadTransactions()
        {
            if (FMainDS.ATransaction.DefaultView.Count > 0)
            {
                FPreviouslySelectedDetailRow = null;
                FMainDS.ATransaction.Clear();
                //ClearControls();
            }
        }

        /// <summary>
        /// Update the effective date from outside
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        public void UpdateEffectiveDateForCurrentRow(DateTime AEffectiveDate)
        {
            if ((GetSelectedDetailRow() != null) && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                GetSelectedDetailRow().TransactionDate = AEffectiveDate;
                dtpDetailTransactionDate.Date = AEffectiveDate;
                GetDetailsFromControls(GetSelectedDetailRow());
            }
        }

        /// <summary>
        /// Return the active transaction number and sets the Journal number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        public Int32 ActiveTransactionNumber(Int32 ALedgerNumber, Int32 ABatchNumber, ref Int32 AJournalNumber)
        {
            Int32 activeTrans = 0;

            if (FPreviouslySelectedDetailRow != null)
            {
                activeTrans = FPreviouslySelectedDetailRow.TransactionNumber;
                AJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
            }

            return activeTrans;
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private GLBatchTDSAJournalRow GetJournalRow()
        {
            return ((TFrmGLBatch)ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        private ABatchRow GetBatchRow()
        {
            return ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FMainDS.ATransaction.RejectChanges();
            }
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewATransaction();

            if (pnlDetails.Enabled == false)
            {
                pnlDetails.Enabled = true;
            }

            // make sure analysis attributes are created
            ((TFrmGLBatch) this.ParentForm).EnableAttributes();

            cmbDetailCostCentreCode.Focus();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefJournalRow">this can be null; otherwise this is the journal that the transaction should belong to</param>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow, AJournalRow ARefJournalRow)
        {
            if (ARefJournalRow == null)
            {
                ARefJournalRow = GetJournalRow();
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ++ARefJournalRow.LastTransactionNumber;
            ANewRow.TransactionDate = GetBatchRow().DateEffective;

            if (FPreviouslySelectedDetailRow != null)
            {
                ANewRow.AccountCode = FPreviouslySelectedDetailRow.AccountCode;
                ANewRow.CostCentreCode = FPreviouslySelectedDetailRow.CostCentreCode;

                if (ARefJournalRow.JournalCreditTotal != ARefJournalRow.JournalDebitTotal)
                {
                    ANewRow.Reference = FPreviouslySelectedDetailRow.Reference;
                    ANewRow.Narrative = FPreviouslySelectedDetailRow.Narrative;
                    ANewRow.TransactionDate = FPreviouslySelectedDetailRow.TransactionDate;
                    decimal Difference = ARefJournalRow.JournalDebitTotal - ARefJournalRow.JournalCreditTotal;
                    ANewRow.TransactionAmount = Math.Abs(Difference);
                    ANewRow.DebitCreditIndicator = Difference < 0;
                }
            }

            //If first row added
            if (grdDetails.Rows.Count == 2)
            {
                if (cmbDetailCostCentreCode.Count > 1)
                {
                    cmbDetailCostCentreCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailCostCentreCode.SelectedIndex = -1;
                }

                if (cmbDetailAccountCode.Count > 1)
                {
                    cmbDetailAccountCode.SelectedIndex = 1;
                }
                else
                {
                    cmbDetailAccountCode.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();
                txtJournalNumber.Text = FJournalNumber.ToString();

                string TransactionCurrency = GetJournalRow().TransactionCurrency;
                string BaseCurrency = FMainDS.ALedger[0].BaseCurrency;
                lblBaseCurrency.Text = String.Format(Catalog.GetString("{0} (Base Currency)"), BaseCurrency);
                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), TransactionCurrency);
                txtDebitAmountBase.CurrencySymbol = BaseCurrency;
                txtCreditAmountBase.CurrencySymbol = BaseCurrency;
                txtDebitAmount.CurrencySymbol = TransactionCurrency;
                txtCreditAmount.CurrencySymbol = TransactionCurrency;
                txtCreditTotalAmountBase.CurrencySymbol = BaseCurrency;
                txtDebitTotalAmountBase.CurrencySymbol = BaseCurrency;
                txtCreditTotalAmount.CurrencySymbol = TransactionCurrency;
                txtDebitTotalAmount.CurrencySymbol = TransactionCurrency;

                // foreign currency accounts only get transactions in that currency
                if (FTransactionCurrency != TransactionCurrency)
                {
                    string SelectedAccount = cmbDetailAccountCode.GetSelectedString();

                    // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
                    bool ActiveOnly = this.Enabled;

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, ActiveOnly, false, TransactionCurrency);

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount);

                    if ((GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED) && FPetraUtilsObject.HasChanges)
                    {
                        FPetraUtilsObject.DisableSaveButton();
                    }

                    FTransactionCurrency = TransactionCurrency;
                }
            }

            UpdateChangeableStatus();
        }

        private void ShowAttributesTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Attributes);
        }

        private void ShowDetailsManual(ATransactionRow ARow)
        {
            txtJournalNumber.Text = FJournalNumber.ToString();

            if (ARow == null)
            {
                ((TFrmGLBatch)ParentForm).DisableAttributes();
                return;
            }

            if (ARow.DebitCreditIndicator)
            {
                txtDebitAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
                txtCreditAmountBase.NumberValueDecimal = 0;
                txtDebitAmount.NumberValueDecimal = ARow.TransactionAmount;
                txtCreditAmount.NumberValueDecimal = 0;
            }
            else
            {
                txtDebitAmountBase.NumberValueDecimal = 0;
                txtCreditAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
                txtDebitAmount.NumberValueDecimal = 0;
                txtCreditAmount.NumberValueDecimal = ARow.TransactionAmount;
            }

            if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                UpdateTotals();
            }
            else if (FPetraUtilsObject.HasChanges && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                FPetraUtilsObject.DisableSaveButton();
            }

            if (TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString()))
            {
                ((TFrmGLBatch)ParentForm).EnableAttributes();
            }
            else
            {
                ((TFrmGLBatch)ParentForm).DisableAttributes();
            }
            
            RefreshAnalysisAttributesGrid();
        }

        private void RefreshAnalysisAttributesGrid()
        {
        	//Empty the grid
        	FMainDS.ATransAnalAttrib.DefaultView.RowFilter = "1=2";
        	
        	FPSAttributesRow = null;

        	if (FPreviouslySelectedDetailRow == null)
			{
				FTransactionNumber = -1;
				return;
			}
        	else if (!TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString()))
        	{
        		if (grdAnalAttributes.Enabled)
        		{
        			grdAnalAttributes.Enabled = false;
        		}
        		return;
        	}
        	else
        	{
        		if (!grdAnalAttributes.Enabled)
        		{
        			grdAnalAttributes.Enabled = true;
        		}
        	}
			
			FTransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;

            FMainDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                ATransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ATransAnalAttribTable.GetTransactionNumberDBName(),
                FTransactionNumber);

            FMainDS.ATransAnalAttrib.DefaultView.Sort = String.Format("{0} ASC",
                ATransAnalAttribTable.GetAnalysisTypeCodeDBName()
                );

            FMainDS.ATransAnalAttrib.DefaultView.AllowNew = false;
            
            if (grdAnalAttributes.Rows.Count > 1)
            {
            	grdAnalAttributes.SelectRowInGrid(1, true);
            	FPSAttributesRow = GetSelectedAttributeRow();
            }
        }
        
        private void EnterAnalysisAttributesGrid(System.Object sender, EventArgs e)
        {
			if (FAttributesGridEntered)
			{
				return;
			}
			else
			{
				FAttributesGridEntered = true;
			}

	    	if (grdAnalAttributes.SelectedRowIndex() > 0)
        	{
        		//Ensures that when the user first enters the grid, the combo appears.
        		//grdAnalAttributes.SelectRowInGrid(1);
        		grdAnalAttributes.Selection.Focus(new Position(grdAnalAttributes.SelectedRowIndex(), 1), true);
        	}
	    	else if (grdAnalAttributes.Rows.Count > 1)
	    	{
        		grdAnalAttributes.SelectRowInGrid(1);
        		//grdAnalAttributes.Selection.Focus(new Position(grdAnalAttributes.SelectedRowIndex(), 1), true);
	    	}
        }
        
	    private ATransAnalAttribRow GetSelectedAttributeRow()
	    {
	        DataRowView[] SelectedGridRow = grdAnalAttributes.SelectedDataRowsAsDataRowView;
	
	        if (SelectedGridRow.Length >= 1)
	        {
	            return (ATransAnalAttribRow)SelectedGridRow[0].Row;
	        }
	
	        return null;
	    }

	    
	    private void AddAttribCodeValueComboClick(System.Object sender, EventArgs e)
	    {
	    	if (!FAttributesGridEntered)
	    	{
	    		return;
	    	}
	    	
	    	if (grdAnalAttributes.SelectedRowIndex() > 0)
        	{
        		//Ensures that when the user first enters the grid, the combo appears.
        		grdAnalAttributes.Selection.Focus(new Position(grdAnalAttributes.SelectedRowIndex(), 1), true);
        	}
	    	else if (grdAnalAttributes.Rows.Count > 1)
	    	{
        		grdAnalAttributes.SelectRowInGrid(1);
	    	}
	    }
	    
	    private void AddAttribCodeValueCombo(System.Object sender, SourceGrid.RowEventArgs e)
        {
        	FPSAttributesRow = GetSelectedAttributeRow();
			
			string currentAnalTypeCode = FPSAttributesRow.AnalysisTypeCode;
			
        	FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = string.Empty;
			
			FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0} = '{1}'",
			                                                                 AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
			                                                                 currentAnalTypeCode);

			int analTypeCodeValuesCount = FCacheDS.AFreeformAnalysis.DefaultView.Count;

        	if (analTypeCodeValuesCount == 0)
        	{
        		MessageBox.Show("No analysis attribute type codes present!");
        		return;
        	}
        	
			string[] analTypeValues = new string[analTypeCodeValuesCount];
			
			int counter = 0;
			
			foreach (DataRowView dvr in FCacheDS.AFreeformAnalysis.DefaultView)
			{
				AFreeformAnalysisRow faRow = (AFreeformAnalysisRow)dvr.Row;
				analTypeValues[counter] = faRow.AnalysisValue;
				
				counter++;
			}
			
			//Refresh the combo values
			FAnalAttribTypeVal.StandardValuesExclusive = true;
			FAnalAttribTypeVal.StandardValues = analTypeValues;
            Int32 RowNumber;

            RowNumber = grdAnalAttributes.SelectedRowIndex();
            FAnalAttribTypeVal.EnableEdit = true;
            FAnalAttribTypeVal.EditableMode = EditableMode.Focus;
            grdAnalAttributes.Selection.Focus(new Position(RowNumber, grdAnalAttributes.Columns.Count - 1), true);
        }

	    private void AnalysisAttributeValueChanged(System.Object sender, EventArgs e)
	    {
	    	FPetraUtilsObject.SetChangedFlag();
	    }
	    
        private void GetDetailDataFromControlsManual(ATransactionRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            Decimal oldTransactionAmount = ARow.TransactionAmount;
            bool oldDebitCreditIndicator = ARow.DebitCreditIndicator;

            if (txtDebitAmount.Text.Length == 0)
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }

            if (txtCreditAmount.Text.Length == 0)
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            ARow.DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator)
            {
                ARow.TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);

                if (txtCreditAmount.NumberValueDecimal.Value != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
            else
            {
                ARow.TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

            if ((oldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount))
                || (oldDebitCreditIndicator != ARow.DebitCreditIndicator))
            {
                UpdateTotals();
            }
        }

        private void TransDateChanged(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                DateTime dateValue;

                string aDate = dtpDetailTransactionDate.Date.ToString();

                if (!DateTime.TryParse(aDate, out dateValue))
                {
                    dtpDetailTransactionDate.Date = GetBatchRow().DateEffective;
                }
            }
            catch
            {
                //Do nothing
            }
        }

        /// <summary>
        /// update amount in other currencies (optional) and recalculate all totals for current batch and journal
        /// </summary>
        public void UpdateTotals()
        {
            bool alreadyChanged;

            if ((FJournalNumber != -1))         // && !pnlDetailsProtected)
            {
                GLBatchTDSAJournalRow journal = GetJournalRow();

                GLRoutines.UpdateTotalsOfJournal(ref FMainDS, journal);

                alreadyChanged = FPetraUtilsObject.HasChanges;

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.DisableDataChangedEvent();
                }

                txtCreditTotalAmount.NumberValueDecimal = journal.JournalCreditTotal;
                txtDebitTotalAmount.NumberValueDecimal = journal.JournalDebitTotal;
                txtCreditTotalAmountBase.NumberValueDecimal = journal.JournalCreditTotalBase;
                txtDebitTotalAmountBase.NumberValueDecimal = journal.JournalDebitTotalBase;

                if (FPreviouslySelectedDetailRow != null)
                {
                    if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                    {
                        txtDebitAmountBase.NumberValueDecimal = FPreviouslySelectedDetailRow.AmountInBaseCurrency;
                        txtCreditAmountBase.NumberValueDecimal = 0;
                        txtDebitAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                        txtCreditAmount.NumberValueDecimal = 0;
                    }
                    else
                    {
                        txtDebitAmountBase.NumberValueDecimal = 0;
                        txtCreditAmountBase.NumberValueDecimal = FPreviouslySelectedDetailRow.AmountInBaseCurrency;
                        txtDebitAmount.NumberValueDecimal = 0;
                        txtCreditAmount.NumberValueDecimal = FPreviouslySelectedDetailRow.TransactionAmount;
                    }
                }

                if (!alreadyChanged)
                {
                    FPetraUtilsObject.EnableDataChangedEvent();
                }

                // refresh the currency symbols
                ShowDataManual();

                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().UpdateTotals(GetBatchRow());
                    ((TFrmGLBatch)ParentForm).GetBatchControl().UpdateTotals();
                }

                if (!alreadyChanged && FPetraUtilsObject.HasChanges)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            txtCreditAmount.Validated += new EventHandler(ControlHasChanged);
            txtDebitAmount.Validated += new EventHandler(ControlHasChanged);
            cmbDetailCostCentreCode.Validated += new EventHandler(ControlHasChanged);
            cmbDetailAccountCode.Validated += new EventHandler(ControlHasChanged);
            cmbDetailKeyMinistryKey.Validated += new EventHandler(ControlHasChanged);
            txtDetailNarrative.Validated += new EventHandler(ControlHasChanged);
            txtDetailReference.Validated += new EventHandler(ControlHasChanged);
            dtpDetailTransactionDate.Validated += new EventHandler(ControlHasChanged);
            
            grdAnalAttributes.Enter += new EventHandler(EnterAnalysisAttributesGrid);
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            //TODO: Find out why these were put here as they stop the field updates from working
            //SourceGrid.RowEventArgs egrid = new SourceGrid.RowEventArgs(-10);
            //FocusedRowChanged(sender, egrid);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && (GetBatchRow() != null)
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                                 && (GetJournalRow().JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            // pnlDetailsProtected must be changed first: when the enabled property of the control is changed, the focus changes, which triggers validation
            pnlDetailsProtected = !changeable;
            this.btnRemove.Enabled = changeable;
            this.btnNew.Enabled = changeable;
            pnlDetails.Enabled = changeable;
        }

        /// <summary>
        /// Called from the button press on the sub-form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRecord(System.Object sender, EventArgs e)
        {
            this.DeleteATransaction();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(ATransactionRow ARowToDelete, ref string ADeletionQuestion)
        {
            if ((grdDetails.SelectedRowIndex() == -1) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("No GL Transaction is selected to delete."),
                    Catalog.GetString("Deleting GL Transaction"));
                return false;
            }
            else
            {
                // ask if the user really wants to cancel the transaction
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete GL Transaction no: {0} ?"),
                    ARowToDelete.TransactionNumber);
                return true;
            }
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ATransactionRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
                {
                    ClearControls();
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!ADeletionPerformed)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (grdDetails.Rows.Count <= 1)
            {
                ((TFrmGLBatch)ParentForm).GetAttributesControl().ClearCurrentSelection();
                ((TFrmGLBatch)ParentForm).DisableAttributes();
            }
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ATransactionRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            if (ARowToDelete == null)
            {
                ACompletionMessage = string.Empty;
                return deletionSuccessful;
            }

            int currentBatchNo = ARowToDelete.BatchNumber;
            int currentJournalNo = ARowToDelete.JournalNumber;
            int transactionNumberToDelete = ARowToDelete.TransactionNumber;
            int lastTransactionNumber = GetJournalRow().LastTransactionNumber;

            try
            {
                // Delete on client side data through views that is already loaded. Data that is not
                // loaded yet will be deleted with cascading delete on server side so we don't have
                // to worry about this here.

                ACompletionMessage = String.Format(Catalog.GetString("Transaction no.: {0} deleted successfully."),
                    transactionNumberToDelete);

                // Delete the associated transaction analysis attributes
                //Unload any open attributes in the attributes tab
                ((TFrmGLBatch) this.ParentForm).UnloadAttributes();

                //Load all attributes
                FMainDS.ATransAnalAttrib.Clear();

                for (int i = 1; i <= FMainDS.ATransaction.Count; i++)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttrib(FLedgerNumber, currentBatchNo, currentJournalNo, i));
                }

                //If attributes do exist
                if (FMainDS.ATransAnalAttrib.Count > 0)
                {
                    ((TFrmGLBatch) this.ParentForm).DisableAttributes();

                    //Iterate through higher number attributes and transaction numbers and reduce by one
                    DataView attrView = FMainDS.ATransAnalAttrib.DefaultView;
                    attrView.RowFilter = String.Empty;

                    attrView.Sort = ATransAnalAttribTable.GetTransactionNumberDBName();
                    attrView.RowFilter = String.Format("{0} = {1} AND {2} = {3} AND {4} >= {5}",
                        ATransAnalAttribTable.GetBatchNumberDBName(),
                        currentBatchNo,
                        ATransAnalAttribTable.GetJournalNumberDBName(),
                        currentJournalNo,
                        ATransAnalAttribTable.GetTransactionNumberDBName(),
                        transactionNumberToDelete);

                    ATransAnalAttribRow attrRowCurrent = null;

                    foreach (DataRowView gv in attrView)
                    {
                        attrRowCurrent = (ATransAnalAttribRow)gv.Row;

                        if ((attrRowCurrent.TransactionNumber == transactionNumberToDelete))
                        {
                            attrRowCurrent.Delete();
                        }
                        else
                        {
                            attrRowCurrent.TransactionNumber--;
                        }
                    }
                }

                //Bubble the transaction to delete to the top
                DataView transView = FMainDS.ATransaction.DefaultView;
                transView.RowFilter = String.Empty;

                transView.Sort = ATransactionTable.GetTransactionNumberDBName();
                transView.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    currentBatchNo,
                    ATransactionTable.GetJournalNumberDBName(),
                    currentJournalNo);

                ATransactionRow transRowToReceive = null;
                ATransactionRow transRowToCopyDown = null;
                ATransactionRow transRowCurrent = null;

                int currentTransNo = 0;

                foreach (DataRowView gv in transView)
                {
                    transRowCurrent = (ATransactionRow)gv.Row;

                    currentTransNo = transRowCurrent.TransactionNumber;

                    if (currentTransNo > transactionNumberToDelete)
                    {
                        transRowToCopyDown = transRowCurrent;

                        //Copy column values down
                        for (int j = 4; j < transRowToCopyDown.Table.Columns.Count; j++)
                        {
                            //Update all columns except the pk fields that remain the same
                            transRowToReceive[j] = transRowToCopyDown[j];
                        }
                    }

                    if (currentTransNo == transView.Count)                         //Last row which is the row to be deleted
                    {
                        //Mark last record for deletion
                        transRowCurrent.SubType = MFinanceConstants.MARKED_FOR_DELETION;
                    }

                    //transRowToReceive will become previous row for next recursion
                    transRowToReceive = transRowCurrent;
                }

                FPreviouslySelectedDetailRow = null;
                //grdDetails.DataSource = null;

                FPetraUtilsObject.SetChangedFlag();

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            //Bubble the deleted transaction to the top
            //FPreviouslySelectedDetailRow = MFinanceConstants.MARKED_FOR_DELETION;
            if (!((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Unable to save after deleting a transaction!");
                deletionSuccessful = false;
            }
            else
            {
                //Reload from server
                grdDetails.DataSource = null;
                FMainDS.ATransAnalAttrib.Clear();
                FMainDS.ATransaction.Clear();

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(FLedgerNumber, currentBatchNo, currentJournalNo));
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);

                ResetJournalLastTransNumber();

                MessageBox.Show("Deletion successful!");
            }

            UpdateTotals();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                pnlDetails.Enabled = false;
            }

            return deletionSuccessful;
        }

        private void ResetJournalLastTransNumber()
        {
            string transRowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            //Highest trans number first
            string transSortOrder = ATransactionTable.GetTransactionNumberDBName() + " DESC";

            FMainDS.ATransaction.DefaultView.RowFilter = transRowFilter;
            FMainDS.ATransaction.DefaultView.Sort = transSortOrder;

            if (FMainDS.ATransaction.DefaultView.Count > 0)
            {
                ATransactionRow transRow = (ATransactionRow)FMainDS.ATransaction.DefaultView[0].Row;
                GetJournalRow().LastTransactionNumber = transRow.TransactionNumber;
            }
            else
            {
                GetJournalRow().LastTransactionNumber = 0;
            }

            transSortOrder = ATransactionTable.GetTransactionNumberDBName() + " ASC";
            FMainDS.ATransaction.DefaultView.Sort = transSortOrder;
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        private void ClearControls()
        {
            //Stop data change detection
            FPetraUtilsObject.DisableDataChangedEvent();

            //Clear combos
            cmbDetailAccountCode.SelectedIndex = -1;
            cmbDetailCostCentreCode.SelectedIndex = -1;
            cmbDetailKeyMinistryKey.SelectedIndex = -1;
            //Clear Textboxes
            txtDetailNarrative.Clear();
            txtDetailReference.Clear();
            //Clear Numeric Textboxes
            txtDebitAmount.NumberValueDecimal = 0;
            txtDebitAmountBase.NumberValueDecimal = 0;
            txtCreditAmount.NumberValueDecimal = 0;
            txtCreditAmountBase.NumberValueDecimal = 0;

            //Enable data change detection
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            //CheckTransAnalysisAttributes();

            if (TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString()))
            {
                ((TFrmGLBatch)ParentForm).EnableAttributes();
            }
            else
            {
                ((TFrmGLBatch)ParentForm).DisableAttributes();
            }
            
            RefreshAnalysisAttributesGrid();
        }

        private void CheckTransAnalysisAttributes()
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            //check if the necessary rows for the given account are there, automatically add/update account
            GLSetupTDS glSetupCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber);

            if (glSetupCacheDS == null)
            {
                return;
            }

            //Account Number for AnalysisTable lookup
            int currentTransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            string currentAccountCode = cmbDetailAccountCode.GetSelectedString();

            ATransactionRow currentTransactionRow = FPreviouslySelectedDetailRow;

            //Retrieve the analysis attributes for the supplied account
            DataView analAttrView = glSetupCacheDS.AAnalysisAttribute.DefaultView;
            analAttrView.RowFilter = String.Format("{0} = '{1}'",
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                currentAccountCode);

            if (analAttrView.Count > 0)
            {
                for (int i = 0; i < analAttrView.Count; i++)
                {
                    //Read the Type Code for each attribute row
                    AAnalysisAttributeRow analAttrRow = (AAnalysisAttributeRow)analAttrView[i].Row;
                    string analysisTypeCode = analAttrRow.AnalysisTypeCode;

                    //Check if the attribute type code exists in the Transaction Analysis Attributes table
                    ATransAnalAttribRow transAnalAttrRow =
                        (ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber,
                                                                                               currentTransactionNumber,
                                                                                               analysisTypeCode });

                    if (transAnalAttrRow == null)
                    {
                        //Create a new TypeCode for this account
                        ATransAnalAttribRow newRow = FMainDS.ATransAnalAttrib.NewRowTyped(true);
                        newRow.LedgerNumber = FLedgerNumber;
                        newRow.BatchNumber = FBatchNumber;
                        newRow.JournalNumber = FJournalNumber;
                        newRow.TransactionNumber = currentTransactionNumber;
                        newRow.AnalysisTypeCode = analysisTypeCode;
                        newRow.AccountCode = currentAccountCode;

                        FMainDS.ATransAnalAttrib.Rows.Add(newRow);
                    }
                    else if (transAnalAttrRow.AccountCode != currentAccountCode)
                    {
                        //Check account code is correct
                        transAnalAttrRow.AccountCode = currentAccountCode;
                    }
                }
            }
            else
            {
                //If this account code is used need to delete it from TransAnal table.
                DataView transAnalAttrView = FMainDS.ATransAnalAttrib.DefaultView;
                transAnalAttrView.RowFilter = String.Format("{0}={1} AND {2}={3} AND ({4}={5} OR {6}='{7}')",
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    currentTransactionNumber,
                    ATransAnalAttribTable.GetAccountCodeDBName(),
                    currentTransactionRow.AccountCode);

                foreach (DataRowView dv in transAnalAttrView)
                {
                    ATransAnalAttribRow tr = (ATransAnalAttribRow)dv.Row;

                    tr.Delete();
                }
            }
        }

        private void ValidateDataDetailsManual(ATransactionRow ARow)
        {
            if ((ARow == null) || (GetBatchRow() == null) || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //Local validation
            if ((txtDebitAmount.NumberValueDecimal == 0) && (txtCreditAmount.NumberValueDecimal == 0))
            {
                TSharedFinanceValidation_GL.ValidateGLDetailManual(this, FBatchRow, ARow, txtDebitAmount, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
            else
            {
                TSharedFinanceValidation_GL.ValidateGLDetailManual(this, FBatchRow, ARow, null, ref VerificationResultCollection,
                    FValidationControlsDict);
            }
        }

        /// <summary>
        /// Set focus to the gid controltab
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }
    }
}