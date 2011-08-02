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
using Ict.Petra.Shared.MFinance;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;


        //ForeignCurrencyCalculationss foreignCurrencyCalculations;


        /// <summary>
        /// Exchange rate for the forreign currency will be stored here after it is read from the
        /// Journal tab. The "do not use" value is zero.
        /// </summary>
        private decimal exchangeRateForeign = 0m;

        /// <summary>
        /// Dito the exchnage rate for the international currency ...
        /// Actualy the value is irrelevant becaus the international currency is only
        /// to be used to translate a national currencey report into an international
        /// readable and comparable form. So the reports are created in local currency
        /// values and the are "transcalculated" to international currency.
        /// </summary>
        private decimal exchangeRateInternational = 1m;

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AForeignCurrencyName"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, String AForeignCurrencyName)
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

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                true, false, ActiveOnly, false, AForeignCurrencyName);
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
//            txtCreditTotalAmount.NumberValueDecimal = journal.JournalCreditTotal;
//            txtDebitTotalAmount.NumberValueDecimal = journal.JournalDebitTotal;
//            txtCreditTotalAmountBase.NumberValueDecimal = journal.JournalCreditTotal *
//                                                          TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
//                FMainDS.ALedger[0].BaseCurrency,
//                dtpDetailTransactionDate.Date.Value);
//            txtDebitTotalAmountBase.NumberValueDecimal = journal.JournalDebitTotal *
//                                                         TExchangeRateCache.GetDailyExchangeRate(journal.TransactionCurrency,
//                FMainDS.ALedger[0].BaseCurrency,
//                dtpDetailTransactionDate.Date.Value);

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

            if ((oldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount))
                || (oldDebitCreditIndicator != ARow.DebitCreditIndicator))
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
                ARow.AmountInBaseCurrency = ARow.TransactionAmount / exchangeRateForeign;
                ARow.AmountInIntlCurrency = ARow.TransactionAmount / exchangeRateInternational;
            }

            // transactions are filtered for this journal; add up the total amounts
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;
            decimal sumDebitsBase = 0.0M;
            decimal sumCreditsBase = 0.0M;

            foreach (DataRowView v in FMainDS.ATransaction.DefaultView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                if (r.DebitCreditIndicator)
                {
                    sumDebits += r.TransactionAmount;
                    sumDebitsBase += r.AmountInBaseCurrency;
                }
                else
                {
                    sumCredits += r.TransactionAmount;
                    sumCreditsBase += r.AmountInBaseCurrency;
                }
            }

            if (FMainDS.ATransaction.Rows.Count == 0)
            {
                journal.JournalStatus = MFinanceConstants.BATCH_UNPOSTED;
            }
            else
            {
                journal.JournalStatus = MFinanceConstants.BATCH_HAS_TRANSACTIONS;
            }

            txtCreditTotalAmount.NumberValueDecimal = sumCredits;
            txtDebitTotalAmount.NumberValueDecimal = sumDebits;
            txtCreditTotalAmountBase.NumberValueDecimal = sumCreditsBase;
            txtDebitTotalAmountBase.NumberValueDecimal = sumDebitsBase;

            journal.JournalDebitTotal = sumDebitsBase;
            journal.JournalCreditTotal = sumCreditsBase;

            ((TFrmGLBatch)ParentForm).GetJournalsControl().UpdateTotals(GetBatchRow());
            ((TFrmGLBatch)ParentForm).GetBatchControl().UpdateTotals();
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

        private void UpdateBaseAndTotals(System.Object sender, EventArgs e)
        {
            try
            {
                AJournalRow journal = GetJournalRow();
                exchangeRateForeign = journal.ExchangeRateToBase;
            }
            catch (Exception)
            {
                exchangeRateForeign = 0.0M;
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
                    AJournalRow journal = GetJournalRow();
                    journal.JournalStatus = MFinanceConstants.BATCH_UNPOSTED;
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

        /// <summary>
        /// The FMainDS-Contol is only usable after the LedgerNumber has been set externaly.
        /// In this case some "default"-Settings are to be done.
        /// </summary>
        public void FMainDS_ALedgerIsValidNow()
        {
        }

        private void ProcessAnalysisAttibutes()
        {
            ((TFrmGLBatch)ParentForm).GetAttributesControl().CheckAnalysisAttributes((String)cmbDetailAccountCode.SelectedValue);
        }
    }
}