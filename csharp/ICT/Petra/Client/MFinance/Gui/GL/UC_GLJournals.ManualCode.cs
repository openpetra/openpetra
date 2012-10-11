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
using System.Drawing;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Gui.Setup;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLJournals
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private string FBatchStatus = string.Empty;

        private const Decimal DEFAULT_CURRENCY_EXCHANGE = 1.0m;

        /// <summary>
        /// load the journals into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            DateTime batchDateEffective;

            //Make sure the current effective date for the Batch is correct
            batchDateEffective = GetBatchRow().DateEffective;

            if ((!dtpDetailDateEffective.Date.HasValue) || (dtpDetailDateEffective.Date.Value != batchDateEffective))
            {
                dtpDetailDateEffective.Date = batchDateEffective;
            }

            //Check if same Journals as previously selected
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FBatchStatus == ABatchStatus))
            {
                if (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                {
                    if (grdDetails.SelectedRowIndex() > 0)
                    {
                        GetDetailsFromControls(GetSelectedDetailRow());
                    }
                }

                return;
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchStatus = ABatchStatus;

            FPreviouslySelectedDetailRow = null;

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AJournal.DefaultView);

            FMainDS.AJournal.DefaultView.RowFilter = string.Format("{0} = {1}",
                AJournalTable.GetBatchNumberDBName(),
                FBatchNumber);

            FMainDS.AJournal.DefaultView.Sort = String.Format("{0} ASC",
                AJournalTable.GetJournalNumberDBName()
                );

            // only load from server if there are no journals loaded yet for this batch
            // otherwise we would overwrite journals that have already been modified
            if (FMainDS.AJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(ALedgerNumber, ABatchNumber));
            }

            ShowData();
            ShowDetails();

            txtDetailExchangeRateToBase.Enabled = false;

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
            }

            txtBatchNumber.Text = FBatchNumber.ToString();

            //This will update Batch totals
            UpdateTotals(GetBatchRow());
            ((TFrmGLBatch)ParentForm).SaveChanges();
        }

        private void RefreshCurrencyAndExchangeRate()
        {
            txtDetailExchangeRateToBase.Text = FPreviouslySelectedDetailRow.ExchangeRateToBase.ToString("0.00000000");
            txtDetailExchangeRateToBase.BackColor =
                (FPreviouslySelectedDetailRow.ExchangeRateToBase == DEFAULT_CURRENCY_EXCHANGE) ? Color.LightPink : Color.Empty;

            // recalculate the base currency amounts for the transactions
            ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTotals();

            btnGetSetExchangeRate.Enabled = (FPreviouslySelectedDetailRow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);
        }

        private void ResetCurrencyExchangeRate(object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.SuppressChangeDetection && (FPreviouslySelectedDetailRow != null)
                && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                FPreviouslySelectedDetailRow.TransactionCurrency = cmbDetailTransactionCurrency.GetSelectedString();

                ABatchRow batchrow = ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();

                FPreviouslySelectedDetailRow.ExchangeRateToBase = TExchangeRateCache.GetDailyExchangeRate(
                    FMainDS.ALedger[0].BaseCurrency,
                    FPreviouslySelectedDetailRow.TransactionCurrency,
                    batchrow.DateEffective);

                RefreshCurrencyAndExchangeRate();
            }
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            btnGetSetExchangeRate.Click += new EventHandler(SetExchangeRateValue);
            cmbDetailTransactionCurrency.SelectedValueChanged +=
                new System.EventHandler(ResetCurrencyExchangeRate);
        }

        private void SetExchangeRateValue(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate setupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            if (setupDailyExchangeRate.ShowDialog(FLedgerNumber, dtpDetailDateEffective.Date.HasValue? dtpDetailDateEffective.Date.Value:DateTime.Today,
                    cmbDetailTransactionCurrency.GetSelectedString(),
                    DEFAULT_CURRENCY_EXCHANGE) == DialogResult.Cancel)
            {
                return;
            }

            if (FPreviouslySelectedDetailRow.ExchangeRateToBase != setupDailyExchangeRate.CurrencyExchangeRate)
            {
                //Enforce save needed condition
                FPetraUtilsObject.SetChangedFlag();
            }

            FPreviouslySelectedDetailRow.ExchangeRateToBase = setupDailyExchangeRate.CurrencyExchangeRate;

            RefreshCurrencyAndExchangeRate();
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();
                txtDebit.CurrencySymbol = FMainDS.ALedger[0].BaseCurrency;
                txtCredit.CurrencySymbol = FMainDS.ALedger[0].BaseCurrency;
                txtControl.CurrencySymbol = FMainDS.ALedger[0].BaseCurrency;
            }

            if (FPreviouslySelectedDetailRow != null)
            {
                txtDebit.NumberValueDecimal = FPreviouslySelectedDetailRow.JournalDebitTotal;
                txtCredit.NumberValueDecimal = FPreviouslySelectedDetailRow.JournalCreditTotal;
                txtControl.NumberValueDecimal =
                    FPreviouslySelectedDetailRow.JournalDebitTotal -
                    FPreviouslySelectedDetailRow.JournalCreditTotal;
            }

            dtpDetailDateEffective.AllowVerification = !FPetraUtilsObject.DetailProtectedMode;

            UpdateChangeableStatus();
        }

        /// <summary>
        /// update the journal header fields from a batch
        /// </summary>
        /// <param name="ABatch"></param>
        public void UpdateTotals(ABatchRow ABatch)
        {
            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            foreach (DataRowView v in FMainDS.AJournal.DefaultView)
            {
                AJournalRow r = (AJournalRow)v.Row;

                sumCredits += r.JournalCreditTotal;
                sumDebits += r.JournalDebitTotal;
            }

            if (ABatch.BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
            {
                ABatch.BatchCreditTotal = sumCredits;
                ABatch.BatchDebitTotal = sumDebits;
                ABatch.BatchRunningTotal = Math.Round(sumDebits - sumCredits, 2);
            }

            txtCurrentPeriod.Text = ABatch.BatchPeriod.ToString();
            txtDebit.NumberValueDecimal = sumDebits;
            txtCredit.NumberValueDecimal = sumCredits;
            txtControl.NumberValueDecimal = ABatch.BatchControlTotal;
        }

        private void ShowDetailsManual(AJournalRow ARow)
        {
            if (ARow == null)
            {
                ((TFrmGLBatch)ParentForm).DisableTransactions();
            }
            else
            {
                ((TFrmGLBatch)ParentForm).EnableTransactions();

                btnGetSetExchangeRate.Enabled = (ARow.TransactionCurrency != FMainDS.ALedger[0].BaseCurrency);

                //Can't cancel an already cancelled row
                btnCancel.Enabled = (ARow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);
                ((TFrmGLBatch)ParentForm).EnableTransactions();

                if (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }

                UpdateChangeableStatus();
            }
        }

        private ABatchRow GetBatchRow()
        {
            return ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// add a new journal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            this.CreateNewAJournal();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGLBatch) this.ParentForm).EnableTransactions();
            }
        }

        /// <summary>
        /// make sure the correct journal number is assigned and the batch.lastJournal is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        public void NewRowManual(ref GLBatchTDSAJournalRow ANewRow)
        {
            DataView view = new DataView(FMainDS.ABatch);

            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(ABatchTable.TableId), ',');
            ABatchRow row = (ABatchRow)view.FindRows(new object[] { FLedgerNumber, FBatchNumber })[0].Row;
            ANewRow.LedgerNumber = row.LedgerNumber;
            ANewRow.BatchNumber = row.BatchNumber;
            ANewRow.JournalNumber = row.LastJournal + 1;

            // manually created journals are all GL
            ANewRow.SubSystemCode = "GL";
            ANewRow.TransactionTypeCode = "STD";

            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
            ANewRow.TransactionCurrency = ledger.BaseCurrency;

            ANewRow.ExchangeRateToBase = 1;
            ANewRow.DateEffective = row.DateEffective;
            ANewRow.JournalPeriod = row.BatchPeriod;
            row.LastJournal++;
        }

        /// initialise some comboboxes
        private void BeforeShowDetailsManual(AJournalRow ARow)
        {
            // SubSystemCode: the user can only select GL, but the system can generate eg. AP journals or GR journals
            this.cmbDetailSubSystemCode.Items.Clear();
            this.cmbDetailSubSystemCode.Items.AddRange(new object[] { ARow.SubSystemCode });

            TFinanceControls.InitialiseTransactionTypeList(ref cmbDetailTransactionTypeCode, FLedgerNumber, ARow.SubSystemCode);
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Transactions);
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && GetBatchRow() != null
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            Boolean journalUpdatable =
                (FPreviouslySelectedDetailRow != null && FPreviouslySelectedDetailRow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);

            this.btnCancel.Enabled = changeable && journalUpdatable;
            this.btnAdd.Enabled = changeable;
            pnlDetails.Enabled = changeable && journalUpdatable;
            pnlDetailsProtected = !changeable;
        }

        /// <summary>
        /// remove journals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            int currentRowIndex = grdDetails.SelectedRowIndex();

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to cancel this journal ({0}).\n\nDo you really want to cancel it?"),
                            FPreviouslySelectedDetailRow.JournalNumber),
                        Catalog.GetString("Confirm Cancel"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                FPreviouslySelectedDetailRow.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                ABatchRow batchrow = ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
                batchrow.BatchCreditTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                batchrow.BatchDebitTotal -= FPreviouslySelectedDetailRow.JournalDebitTotal;

                if (batchrow.BatchControlTotal != 0)
                {
                    batchrow.BatchControlTotal -= FPreviouslySelectedDetailRow.JournalCreditTotal;
                }

                FPreviouslySelectedDetailRow.JournalCreditTotal = 0;
                FPreviouslySelectedDetailRow.JournalDebitTotal = 0;

                foreach (ATransactionRow transaction in FMainDS.ATransaction.Rows)     //alle? ist das richtig?
                {
                    transaction.Delete();
                }

                SelectRowInGrid(currentRowIndex);

                ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                FPetraUtilsObject.SetChangedFlag();
                UpdateChangeableStatus();

                if (grdDetails.Rows.Count < 2)
                {
                    ClearControls();
                }
            }
        }

        private void ClearControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            txtDetailJournalDescription.Clear();
            cmbDetailTransactionTypeCode.SelectedIndex = -1;
            cmbDetailTransactionCurrency.SelectedIndex = -1;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private decimal GetActualExchangeRateForeign()
        {
            return Convert.ToDecimal(txtDetailExchangeRateToBase.Text);
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }
    }
}