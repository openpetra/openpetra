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
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private string FTransactionCurrency = string.Empty;
        private string FBatchStatus = string.Empty;
        private string FJournalStatus = string.Empty;

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
            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionCurrency == AForeignCurrencyName) && (FBatchStatus == ABatchStatus) && (FJournalStatus == AJournalStatus))
            {
                //Same as previously selected
                if ((GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (grdDetails.SelectedRowIndex() > 0))
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

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            FMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                FJournalNumber);

            FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} ASC",
                ATransactionTable.GetTransactionNumberDBName()
                );

            if (FMainDS.ATransaction.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransaction(ALedgerNumber, ABatchNumber, AJournalNumber));
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

            this.CreateNewATransaction();
            ProcessAnalysisAttributes();

            if (pnlDetails.Enabled == false)
            {
                pnlDetails.Enabled = true;
            }

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
            //GLBatchTDSATransactionRow prevRow = GetSelectedDetailRow();

            if (ARefJournalRow == null)
            {
                ARefJournalRow = GetJournalRow();
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ARefJournalRow.LastTransactionNumber + 1;
            ANewRow.TransactionDate = GetBatchRow().DateEffective;
            ARefJournalRow.LastTransactionNumber++;

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
                }

                FTransactionCurrency = TransactionCurrency;
            }

            // Create Text description of Anal Attribs for each transaction..

            foreach (GLBatchTDSATransactionRow TransactionRow in FMainDS.ATransaction.Rows)
            {
                if (TransactionRow.RowState != DataRowState.Deleted)
                {
                    ((TFrmGLBatch)ParentForm).LoadAttributes(
                        TransactionRow.LedgerNumber,
                        TransactionRow.BatchNumber,
                        TransactionRow.JournalNumber,
                        TransactionRow.TransactionNumber
                        );


                    string strAnalAttr = "";
                    FMainDS.ATransAnalAttrib.DefaultView.RowFilter =
                        String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                            ATransAnalAttribTable.GetLedgerNumberDBName(), TransactionRow.LedgerNumber,
                            ATransAnalAttribTable.GetBatchNumberDBName(), TransactionRow.BatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(), TransactionRow.JournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(), TransactionRow.TransactionNumber);

                    foreach (DataRowView rv in FMainDS.ATransAnalAttrib.DefaultView)
                    {
                        ATransAnalAttribRow Row = (ATransAnalAttribRow)rv.Row;

                        if (strAnalAttr.Length > 0)
                        {
                            strAnalAttr += ", ";
                        }

                        strAnalAttr += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                    }

                    TransactionRow.AnalysisAttributes = strAnalAttr;
                }
            }

            UpdateChangeableStatus();
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
        }

        private void GetDetailDataFromControlsManual(ATransactionRow ARow)
        {
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
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            SourceGrid.RowEventArgs egrid = new SourceGrid.RowEventArgs(-10);
            FocusedRowChanged(sender, egrid);
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

            this.btnRemove.Enabled = changeable;
            this.btnNew.Enabled = changeable;
            pnlDetails.Enabled = changeable;
            pnlDetailsProtected = !changeable;
        }

        /// <summary>
        /// remove transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RemoveRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete this transaction ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.TransactionNumber), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = grdDetails.SelectedRowIndex();

                ((TFrmGLBatch)ParentForm).GetAttributesControl().DeleteTransactionAttributes(FPreviouslySelectedDetailRow);
                FPreviouslySelectedDetailRow.Delete();

                FPreviouslySelectedDetailRow = null;

                FPetraUtilsObject.SetChangedFlag();

                SelectRowInGrid(rowIndex);

                if (grdDetails.Rows.Count < 2)
                {
                    ClearControls();
                    ((TFrmGLBatch)ParentForm).DisableAttributes();
                    pnlDetails.Enabled = false;
                }

                UpdateTotals();
            }
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
            ProcessAnalysisAttributes();
        }

        private void ProcessAnalysisAttributes()
        {
            ((TFrmGLBatch)ParentForm).GetAttributesControl().CheckAnalysisAttributes(cmbDetailAccountCode.GetSelectedString());
        }
    }
}