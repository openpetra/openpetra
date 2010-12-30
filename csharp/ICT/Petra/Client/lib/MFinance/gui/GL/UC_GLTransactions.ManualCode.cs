//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;


        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            if (FBatchNumber != -1)
            {
                GetDataFromControls();
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            btnNew.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            btnRemove.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            FPreviouslySelectedDetailRow = null;

            DataView view = new DataView(FMainDS.ATransaction);
            view.RowStateFilter = DataViewRowState.CurrentRows | DataViewRowState.Deleted;

            // only load from server if there are no transactions loaded yet for this journal
            // otherwise we would overwrite transactions that have already been modified
            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(AJournalTable.TableId), ",");

            if (view.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber }) == -1)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransaction(ALedgerNumber, ABatchNumber, AJournalNumber));
            }

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);

            ShowData();
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private AJournalRow GetJournalRow()
        {
            return (AJournalRow)FMainDS.AJournal.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber });
        }

        private ABatchRow GetBatchRow()
        {
            return (ABatchRow)FMainDS.ABatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewATransaction();
            ProcessAnalysisAttibutes();
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
            ANewRow.TransactionNumber = ARefJournalRow.LastTransactionNumber + 1;
            ARefJournalRow.LastTransactionNumber++;
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
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();
            txtJournalNumber.Text = FJournalNumber.ToString();

            if (FMainDS.ALedger.Count == 1)
            {
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
            }
        }

        private void ShowDetailsManual(ATransactionRow ARow)
        {
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

            AJournalRow journal = GetJournalRow();
            txtCreditTotalAmount.NumberValueDecimal = journal.JournalCreditTotal;
            txtDebitTotalAmount.NumberValueDecimal = journal.JournalDebitTotal;
            txtCreditTotalAmountBase.NumberValueDecimal = journal.JournalCreditTotal *
                                                          TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
                FMainDS.ALedger[0].BaseCurrency,
                dtpDetailTransactionDate.Date.Value);
            txtDebitTotalAmountBase.NumberValueDecimal = journal.JournalDebitTotal *
                                                         TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
                FMainDS.ALedger[0].BaseCurrency,
                dtpDetailTransactionDate.Date.Value);

            if (ARow == null)
            {
                ((TFrmGLBatch)ParentForm).DisableAttributes();
            }
            else
            {
                ((TFrmGLBatch)ParentForm).LoadAttributes(
                    ARow.LedgerNumber,
                    ARow.BatchNumber,
                    ARow.JournalNumber,
                    ARow.TransactionNumber
                    );
            }
        }

        private void GetDetailDataFromControlsManual(ATransactionRow ARow)
        {
            Decimal oldTransactionAmount = ARow.TransactionAmount;
            bool oldDebitCreditIndicator = ARow.DebitCreditIndicator;

            ARow.DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator)
            {
                ARow.TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);
            }
            else
            {
                ARow.TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

            if ((oldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount)) || (oldDebitCreditIndicator != ARow.DebitCreditIndicator))
            {
                UpdateTotals(ARow);
            }
        }

        /// <summary>
        /// update amount in other currencies (optional) and recalculate all totals for current batch and journal
        /// </summary>
        /// <param name="ARow"></param>
        public void UpdateTotals(ATransactionRow ARow)
        {
            AJournalRow journal = GetJournalRow();

            if (ARow != null)
            {
                ARow.AmountInBaseCurrency = TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
                    FMainDS.ALedger[0].BaseCurrency,
                    ARow.TransactionDate) *
                                            ARow.TransactionAmount;
                ARow.AmountInIntlCurrency = TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
                    FMainDS.ALedger[0].IntlCurrency,
                    ARow.TransactionDate) *
                                            ARow.TransactionAmount;
            }

            // transactions are filtered for this journal; add up the total amounts
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            foreach (DataRowView v in FMainDS.ATransaction.DefaultView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                if (r.DebitCreditIndicator)
                {
                    sumDebits += r.TransactionAmount;
                }
                else
                {
                    sumCredits += r.TransactionAmount;
                }
            }

            journal.JournalCreditTotal = sumCredits;
            journal.JournalDebitTotal = sumDebits;

            sumDebits = 0.0M;
            sumCredits = 0.0M;

            foreach (DataRowView v in FMainDS.AJournal.DefaultView)
            {
                AJournalRow r = (AJournalRow)v.Row;
                sumDebits += r.JournalDebitTotal;
                sumCredits += r.JournalCreditTotal;
            }

            ABatchRow batch = GetBatchRow();
            batch.BatchCreditTotal = sumCredits;
            batch.BatchDebitTotal = sumDebits;
            ((TFrmGLBatch)ParentForm).GetJournalsControl().UpdateTotals(batch);
            // TODO: Batch.BatchRunningTotal
        }

        private void UpdateBaseAndTotals(System.Object sender, EventArgs e)
        {
            try
            {
                AJournalRow journal = GetJournalRow();

                txtDebitAmountBase.NumberValueDecimal =
                    (TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency, FMainDS.ALedger[0].BaseCurrency,
                         dtpDetailTransactionDate.Date.Value) * txtDebitAmount.NumberValueDecimal.Value);
                txtCreditAmountBase.NumberValueDecimal =
                    (TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency, FMainDS.ALedger[0].BaseCurrency,
                         dtpDetailTransactionDate.Date.Value) * txtCreditAmount.NumberValueDecimal.Value);
            }
            catch (Exception)
            {
            }
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
                                "You have choosen to delete this transaction ({0}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.TransactionNumber), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = grdDetails.Selection.GetSelectionRegion().GetRowsIndex()[0];
                ((TFrmGLBatch)ParentForm).GetAttributesControl().DeleteTransactionAttributes(FPreviouslySelectedDetailRow);
                FPreviouslySelectedDetailRow.Delete();
                UpdateTotals(null);
                FPetraUtilsObject.SetChangedFlag();

                if (rowIndex == grdDetails.Rows.Count)
                {
                    rowIndex--;
                }

                if (grdDetails.Rows.Count > 1)
                {
                    grdDetails.Selection.SelectRow(rowIndex, true);
                    FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                    ShowDetails(FPreviouslySelectedDetailRow);
                }
                else
                {
                    FPreviouslySelectedDetailRow = null;
                }
            }
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            ProcessAnalysisAttibutes();
        }

        private void ProcessAnalysisAttibutes()
        {
            ((TFrmGLBatch)ParentForm).GetAttributesControl().CheckAnalysisAttributes((String)cmbDetailAccountCode.SelectedValue);
        }
    }
}