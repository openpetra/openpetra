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
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLBatch
    {
        /// this window contains 3 tabs
        public enum eGLTabs
        {
            /// list of batches
            Batches,

            /// list of journals
            Journals,

            /// list of transactions
            Transactions
        };

        private eGLTabs FPreviousTab = eGLTabs.Batches;
        private Int32 FLedgerNumber = -1;
        private Int32 FStandardTabIndex = 0;
        private bool FLoadForImport = false;
        private bool FWarnAboutMissingIntlExchangeRate = true;

        // Variables that are used to select a specific batch on startup
        private Int32 FInitialBatchYear = -1;
        private Int32 FInitialBatchPeriod = -1;
        private Int32 FInitialBatchNumber = -1;
        private Int32 FInitialJournalNumber = -1;
        private Boolean FInitialBatchFound = false;

        private Boolean FLatestSaveIncludedForex = false;

        /// <summary>
        /// specify to load and import batches
        /// </summary>
        public Boolean LoadForImport
        {
            set
            {
                FLoadForImport = value;
            }
            get
            {
                return FLoadForImport;
            }
        }

        /// <summary>
        /// Set this property if you want to load the screen with an initial Year/Batch/Journal
        /// </summary>
        public Int32 InitialBatchYear
        {
            set
            {
                FInitialBatchYear = value;
            }
            get
            {
                return FInitialBatchYear;
            }
        }

        /// <summary>
        /// Set this property if you want to load the screen with an initial Year/Batch/Journal
        /// </summary>
        public Int32 InitialBatchPeriod
        {
            set
            {
                FInitialBatchPeriod = value;
            }
            get
            {
                return FInitialBatchPeriod;
            }
        }

        /// <summary>
        /// Set this property if you want to load the screen with an initial Year/Batch/Journal
        /// </summary>
        public Int32 InitialBatchNumber
        {
            set
            {
                FInitialBatchNumber = value;
            }
            get
            {
                return FInitialBatchNumber;
            }
        }

        /// <summary>
        /// Set this property if you want to load the screen with an initial Year/Batch/Journal
        /// </summary>
        public Int32 InitialJournalNumber
        {
            set
            {
                FInitialJournalNumber = value;
            }
            get
            {
                return FInitialJournalNumber;
            }
        }

        /* Be sure to leave the LedgerNumber property in this position in the code
         * AFTER the ones above.   Otherwise the code will not work that opens this
         * screen at a pre-defined batch/journal.*/

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                // Setting the ledger number of the batch control will automatically load the current financial year batches
                ucoBatches.LedgerNumber = value;

                ucoJournals.WorkAroundInitialization();
                ucoTransactions.WorkAroundInitialization();
            }
        }

        /// <summary>
        /// Flag that is relevant when the screen is loaded with an initial Batch/Journal specified.
        /// </summary>
        public Boolean InitialBatchFound
        {
            set
            {
                FInitialBatchFound = value;
            }
            get
            {
                return FInitialBatchFound;
            }
        }

        private void TFrmGLBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGLBatch.SelectedIndex = FStandardTabIndex;
            TabSelectionChanged(null, null);

            this.Shown += delegate
            {
                // This will ensure the grid gets the focus when the screen is shown for the first time
                ucoBatches.SetInitialFocus();
            };
        }

        private void InitializeManualCode()
        {
            tabGLBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
            this.tpgJournals.Enabled = false;
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals(Boolean AEnable = true)
        {
            this.tabGLBatch.TabStop = AEnable;

            if (this.tpgJournals.Enabled != AEnable)
            {
                this.tpgJournals.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the journal tab if we have no active batch
        /// </summary>
        public void DisableJournals()
        {
            this.tabGLBatch.TabStop = false;

            if (this.tpgJournals.Enabled)
            {
                this.tpgJournals.Enabled = false;
                this.Refresh();
            }
        }

        /// <summary>
        /// Control transactions tab enabled status. This can do both ways
        /// </summary>
        public void EnableTransactions(Boolean AEnable = true)
        {
            if (this.tpgTransactions.Enabled != AEnable)
            {
                this.tpgTransactions.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void DisableTransactions()
        {
            if (this.tpgTransactions.Enabled)
            {
                this.tpgTransactions.Enabled = false;
                this.Refresh();
            }
        }

        /// <summary>
        /// Print or reprint the posting report for this batch.
        /// </summary>
        public static void PrintPostingRegister(Int32 ALedgerNumber, Int32 ABatchNumber, Boolean AEditTemplate = false)
        {
            FastReportsWrapper ReportingEngine = new FastReportsWrapper("Batch Posting Register");

            if (!ReportingEngine.LoadedOK)
            {
                ReportingEngine.ShowErrorPopup();
                return;
            }

            GLBatchTDS BatchTDS = TRemote.MFinance.GL.WebConnectors.LoadABatchAndContent(ALedgerNumber, ABatchNumber);
            TRptCalculator Calc = new TRptCalculator();
            ALedgerRow LedgerRow = BatchTDS.ALedger[0];

            //Call RegisterData to give the data to the template
            ReportingEngine.RegisterData(BatchTDS.ABatch, "ABatch");
            ReportingEngine.RegisterData(BatchTDS.AJournal, "AJournal");
            ReportingEngine.RegisterData(BatchTDS.ATransaction, "ATransaction");
            ReportingEngine.RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList,
                    ALedgerNumber), "AAccount");
            ReportingEngine.RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                    ALedgerNumber), "ACostCentre");

            Calc.AddParameter("param_batch_number_i", ABatchNumber);
            Calc.AddParameter("param_ledger_number_i", ALedgerNumber);
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(ALedgerNumber);
            Calc.AddStringParameter("param_ledger_name", LedgerName);

            if (AEditTemplate)
            {
                ReportingEngine.DesignReport(Calc);
            }
            else
            {
                ReportingEngine.GenerateReport(Calc);
            }
        }

        /// <summary>
        /// Print out the selected batch using FastReports template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilePrint(object sender, EventArgs e)
        {
            if (!this.tpgJournals.Enabled)
            {
                MessageBox.Show(Catalog.GetString("No Batch is selected"), Catalog.GetString("Batch Posting Register"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ABatchRow BatchRow = ucoBatches.GetSelectedDetailRow();
            PrintPostingRegister(FLedgerNumber, BatchRow.BatchNumber, ModifierKeys.HasFlag(Keys.Control));
        }

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (ATab == eGLTabs.Batches)
                {
                    this.tabGLBatch.SelectedTab = this.tpgBatches;
                    this.tpgJournals.Enabled = (ucoBatches.GetSelectedDetailRow() != null);
                    this.tabGLBatch.TabStop = this.tpgJournals.Enabled;

                    if (this.tpgTransactions.Enabled)
                    {
                        this.ucoTransactions.CancelChangesToFixedBatches();
                        this.ucoJournals.CancelChangesToFixedBatches();
                    }

                    ucoBatches.AutoEnableTransTabForBatch();
                    ucoBatches.SetInitialFocus();
                    FPreviousTab = eGLTabs.Batches;
                }
                else if ((ucoBatches.GetSelectedDetailRow() != null) && (ATab == eGLTabs.Journals))
                {
                    if (this.tpgJournals.Enabled)
                    {
                        this.tabGLBatch.SelectedTab = this.tpgJournals;

                        this.ucoJournals.LoadJournals(FLedgerNumber,
                            ucoBatches.GetSelectedDetailRow().BatchNumber,
                            ucoBatches.GetSelectedDetailRow().BatchStatus);

                        this.tpgTransactions.Enabled =
                            (ucoJournals.GetSelectedDetailRow() != null && ucoJournals.GetSelectedDetailRow().JournalStatus !=
                             MFinanceConstants.BATCH_CANCELLED);

                        this.ucoJournals.UpdateHeaderTotals(ucoBatches.GetSelectedDetailRow());

                        FPreviousTab = eGLTabs.Journals;
                    }
                }
                else if (ATab == eGLTabs.Transactions)
                {
                    if (this.tpgTransactions.Enabled)
                    {
                        // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                        this.tabGLBatch.SelectedTab = this.tpgTransactions;

                        bool fromBatchTab = false;

                        if (FPreviousTab == eGLTabs.Batches)
                        {
                            fromBatchTab = true;
                            //This only happens when the user clicks from Batch to Transactions,
                            //  which is only allowed when one journal exists

                            //Need to make sure that the Journal is loaded
                            this.ucoJournals.LoadJournals(FLedgerNumber,
                                ucoBatches.GetSelectedDetailRow().BatchNumber,
                                ucoBatches.GetSelectedDetailRow().BatchStatus);
                        }

                        this.ucoTransactions.LoadTransactions(
                            FLedgerNumber,
                            ucoJournals.GetSelectedDetailRow().BatchNumber,
                            ucoJournals.GetSelectedDetailRow().JournalNumber,
                            ucoJournals.GetSelectedDetailRow().TransactionCurrency,
                            fromBatchTab,
                            ucoBatches.GetSelectedDetailRow().BatchStatus,
                            ucoJournals.GetSelectedDetailRow().JournalStatus);

                        FPreviousTab = eGLTabs.Transactions;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(false, true))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
        }

        private void SelectTabManual(Int32 ASelectedTabIndex)
        {
            switch (ASelectedTabIndex)
            {
                case (int)eGLTabs.Batches:
                    SelectTab(eGLTabs.Batches);
                    break;

                case (int)eGLTabs.Journals:
                    SelectTab(eGLTabs.Journals);
                    break;

                default: //(ASelectedTabIndex == (int)eGLTabs.Transactions)
                    SelectTab(eGLTabs.Transactions);
                    break;
            }
        }

        /// <summary>
        /// Set up the screen to highlight this batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ABatchYear"></param>
        /// <param name="ABatchPeriod"></param>
        public void ShowDetailsOfOneBatch(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, int ABatchYear, int ABatchPeriod)
        {
            FLedgerNumber = ALedgerNumber;
            InitialBatchNumber = ABatchNumber;
            InitialJournalNumber = AJournalNumber;

            // filter will show this year and period
            FInitialBatchYear = ABatchYear;
            FInitialBatchPeriod = ABatchPeriod;

            Show();
        }

        /// <summary>
        /// directly access the batches control
        /// </summary>
        public TUC_GLBatches GetBatchControl()
        {
            return ucoBatches;
        }

        /// <summary>
        /// directly access the journals control
        /// </summary>
        public TUC_GLJournals GetJournalsControl()
        {
            return ucoJournals;
        }

        /// <summary>
        /// directly access the transactions control
        /// </summary>
        public TUC_GLTransactions GetTransactionsControl()
        {
            return ucoTransactions;
        }

        private void RunOnceOnActivationManual()
        {
        }

        /// <summary>
        /// Uses the current Batch effective date to return the
        ///   corporate exchange rate value
        /// </summary>
        /// <returns></returns>
        public decimal GetInternationalCurrencyExchangeRate()
        {
            decimal IntlToBaseCurrencyExchRate = 0;

            ABatchRow BatchRow = ucoBatches.GetSelectedDetailRow();

            if (BatchRow == null)
            {
                return IntlToBaseCurrencyExchRate;
            }

            return GetInternationalCurrencyExchangeRate(BatchRow.DateEffective);
        }

        /// <summary>
        /// Returns the corporate exchange rate for a given batch row
        ///  and specifies whether or not the transaction is in International
        ///  currency
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        /// <returns></returns>
        private decimal GetInternationalCurrencyExchangeRate(DateTime AEffectiveDate)
        {
            DateTime StartOfMonth = new DateTime(AEffectiveDate.Year, AEffectiveDate.Month, 1);
            string LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            string LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            decimal IntlToBaseCurrencyExchRate = 0;

            if (FLedgerNumber != -1)
            {
                if (LedgerBaseCurrency == LedgerIntlCurrency)
                {
                    IntlToBaseCurrencyExchRate = 1;
                }
                else
                {
                    IntlToBaseCurrencyExchRate = TRemote.MFinance.GL.WebConnectors.GetCorporateExchangeRate(LedgerBaseCurrency,
                        LedgerIntlCurrency,
                        StartOfMonth,
                        AEffectiveDate);

                    if ((IntlToBaseCurrencyExchRate == 0) && FWarnAboutMissingIntlExchangeRate)
                    {
                        FWarnAboutMissingIntlExchangeRate = false;

                        string IntlRateErrorMessage =
                            String.Format(Catalog.GetString("No Corporate Exchange rate exists for {0} to {1} for the month: {2:MMMM yyyy}!"),
                                LedgerBaseCurrency,
                                LedgerIntlCurrency,
                                AEffectiveDate);

                        MessageBox.Show(IntlRateErrorMessage, "Lookup Corporate Exchange Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            return IntlToBaseCurrencyExchRate;
        }

        private int GetChangedRecordCountManual(out string AMessage)
        {
            // For GL Batch we will get some mix of batches, journals and transactions
            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int allChangesCount = 0;

            // Work out how many changes in each table
            foreach (DataTable dt in FMainDS.Tables)
            {
                if (dt != null)
                {
                    int tableChangesCount = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr.RowState != DataRowState.Unchanged)
                        {
                            tableChangesCount++;
                            allChangesCount++;
                        }
                    }

                    if (tableChangesCount > 0)
                    {
                        TableAndCountList.Add(new Tuple <string, int>(dt.TableName, tableChangesCount));
                    }
                }
            }

            // Now build up a sensible message
            AMessage = String.Empty;

            if (TableAndCountList.Count > 0)
            {
                int nBatches = 0;
                int nJournals = 0;
                int nTransactions = 0;

                foreach (Tuple <string, int>TableAndCount in TableAndCountList)
                {
                    if (TableAndCount.Item1.Equals(ABatchTable.GetTableName()))
                    {
                        nBatches = TableAndCount.Item2;
                    }
                    else if (TableAndCount.Item1.Equals(AJournalTable.GetTableName()))
                    {
                        nJournals = TableAndCount.Item2;
                    }
                    else if (TableAndCount.Item2 > nTransactions)
                    {
                        nTransactions = TableAndCount.Item2;
                    }
                }

                AMessage = Catalog.GetString("    You have made changes to ");
                string strBatches = String.Empty;
                string strJournals = String.Empty;
                string strTransactions = String.Empty;

                if (nBatches > 0)
                {
                    strBatches = String.Format("{0} {1}",
                        nBatches,
                        Catalog.GetPluralString("batch", "batches", nBatches));
                }

                if (nJournals > 0)
                {
                    strJournals = String.Format("{0} {1}",
                        nJournals,
                        Catalog.GetPluralString("journal", "journals", nJournals));
                }

                if (nTransactions > 0)
                {
                    strTransactions = String.Format("{0} {1}",
                        nTransactions,
                        Catalog.GetPluralString("transaction", "transactions", nTransactions));
                }

                bool bGotAll = (nBatches > 0) && (nJournals > 0) && (nTransactions > 0);

                if (nBatches > 0)
                {
                    AMessage += strBatches;
                }

                if (nJournals > 0)
                {
                    if (bGotAll)
                    {
                        AMessage += ", ";
                    }
                    else if (nBatches > 0)
                    {
                        AMessage += " and ";
                    }

                    AMessage += strJournals;
                }

                if (nTransactions > 0)
                {
                    if ((nBatches > 0) || (nJournals > 0))
                    {
                        AMessage += " and ";
                    }

                    AMessage += strTransactions;
                }

                AMessage += Environment.NewLine;
                AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
            }

            return allChangesCount;
        }

        // This manual method lets us peek at the data that is about to be saved...
        // The data has already been collected from the contols and validated and there is definitely something to save...
        private TSubmitChangesResult StoreManualCode(ref GLBatchTDS SubmitDS, out TVerificationResultCollection VerificationResult)
        {
            FLatestSaveIncludedForex = false;

            if (SubmitDS.AJournal != null)
            {
                // Check whether we are saving any rows that are in foreign currency
                DataView dv = new DataView(SubmitDS.AJournal,
                    String.Format("{0}<>{1}", AJournalTable.GetTransactionCurrencyDBName(), AJournalTable.GetBaseCurrencyDBName()),
                    String.Empty, DataViewRowState.CurrentRows);
                FLatestSaveIncludedForex = (dv.Count > 0);
            }

            // Now do the standard call to save the changes
            return TRemote.MFinance.GL.WebConnectors.SaveGLBatchTDS(ref SubmitDS, out VerificationResult);
        }

        private void FPetraUtilsObject_DataSaved(object sender, TDataSavedEventArgs e)
        {
            if (e.Success && FLatestSaveIncludedForex)
            {
                // Notify the exchange rate screen, if it is there
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGLOrGiftBatchSaved, this.ToString());
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
        }

        /// <summary>
        /// find a special gift detail
        /// </summary>
        public void FindGLTransaction(int ABatchNumber, int AJournalNumber, int ATransactionNumber)
        {
            ucoTransactions.SelectTransactionNumber(ATransactionNumber);
            FStandardTabIndex = 2;     // later we switch to the transaction tab
        }

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        /// Special Handlers for menus and command keys for our user controls

        private void MniFilterFind_Click(object sender, EventArgs e)
        {
            if (tabGLBatch.SelectedTab == tpgBatches)
            {
                ucoBatches.MniFilterFind_Click(sender, e);
            }
            else if (tabGLBatch.SelectedTab == tpgJournals)
            {
                ucoJournals.MniFilterFind_Click(sender, e);
            }
            else if (tabGLBatch.SelectedTab == tpgTransactions)
            {
                ucoTransactions.MniFilterFind_Click(sender, e);
            }
        }

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.S | Keys.Control))
            {
                if (FPetraUtilsObject.HasChanges)
                {
                    SaveChanges();
                }

                return true;
            }

            if ((tabGLBatch.SelectedTab == tpgBatches) && (ucoBatches.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabGLBatch.SelectedTab == tpgJournals) && (ucoJournals.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabGLBatch.SelectedTab == tpgTransactions) && (ucoTransactions.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}