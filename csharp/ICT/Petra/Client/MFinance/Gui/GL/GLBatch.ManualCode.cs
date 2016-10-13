//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MReporting.Gui.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLBatch
    {
        /// <summary>Store the current action on the batch</summary>
        public TGLBatchEnums.GLBatchAction FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;

        private TGLBatchEnums.eGLTabs FPreviouslySelectedTab = TGLBatchEnums.eGLTabs.None;
        private Int32 FLedgerNumber = -1;
        private Int32 FStandardTabIndex = 0;
        private bool FChangesDetected = false;

        private bool FLoadForImport = false;
        private bool FWarnAboutMissingIntlExchangeRate = true;

        // Variables that are used to select a specific batch on startup
        private Int32 FInitialBatchYear = -1;
        private Int32 FInitialBatchPeriod = -1;
        private Int32 FInitialBatchNumber = -1;
        private Int32 FInitialJournalNumber = -1;
        private Boolean FInitialBatchFound = false;
        private Boolean FenablePostingReport = true;

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
        /// warn the user that corporate exchange rate is missing
        /// </summary>
        public Boolean WarnAboutMissingIntlExchangeRate
        {
            set
            {
                FWarnAboutMissingIntlExchangeRate = value;
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

                FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
                FPetraUtilsObject.DataSavingValidated += new TDataSavingValidatedHandler(FPetraUtilsObject_DataSavingValidated);

                ucoJournals.WorkAroundInitialization();
                ucoTransactions.WorkAroundInitialization();
            }
            get
            {
                return FLedgerNumber;
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

        /// <summary>
        /// If this is false, no report will be generated on Batch posting.
        /// </summary>
        public Boolean EnablePostingReport
        {
            get
            {
                TLogging.Log("GLBatch.ManualCode: Posting Report Enabled: " + FenablePostingReport);
                return FenablePostingReport;
            }
            set
            {
                FenablePostingReport = value;
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
            tabGLBatch.GotFocus += new EventHandler(tabGLBatch_GotFocus);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
            this.tpgJournals.Enabled = false;
            this.tpgTransactions.Enabled = false;

            // change the event that gets called when 'Save' is clicked (i.e. changed from generated code)
            tbbSave.Click -= FileSave;
            mniFileSave.Click -= FileSave;
            tbbSave.Click += FileSaveManual;
            mniFileSave.Click += FileSaveManual;
        }

        private void tabGLBatch_GotFocus(Object Sender, EventArgs e)
        {
            FPetraUtilsObject.WriteToStatusBar(Catalog.GetString(
                    "Use the left or right arrow keys to switch between Batches, Journals and Transactions"));
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

        private void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(false, TErrorProcessingMode.Epm_All))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }

            //Before the TabSelectionChanged event occurs, changes are incorrectly detected on Journal controls
            // TODO: find cause but use this field for now
            FChangesDetected = FPetraUtilsObject.HasChanges;
        }

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        /// <param name="AAllowRepeatEvent"></param>
        public void SelectTab(TGLBatchEnums.eGLTabs ATab, bool AAllowRepeatEvent = false)
        {
            //Between the tab changing and seleted events changes are incorrectly detected on Journal controls
            // TODO: find cause but use this field for now
            if (!FChangesDetected && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
            else if (FChangesDetected && !FPetraUtilsObject.HasChanges)
            {
                FChangesDetected = false;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (ATab == TGLBatchEnums.eGLTabs.Batches)
                {
                    if ((FPreviouslySelectedTab == TGLBatchEnums.eGLTabs.Batches) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    FPreviouslySelectedTab = TGLBatchEnums.eGLTabs.Batches;

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
                }
                else if (ATab == TGLBatchEnums.eGLTabs.Journals)
                {
                    if ((FPreviouslySelectedTab == TGLBatchEnums.eGLTabs.Journals) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    if (this.tpgJournals.Enabled)
                    {
                        FPreviouslySelectedTab = TGLBatchEnums.eGLTabs.Journals;

                        this.tabGLBatch.SelectedTab = this.tpgJournals;

                        LoadJournals(ucoBatches.GetSelectedDetailRow());

                        this.tpgTransactions.Enabled =
                            (ucoJournals.GetSelectedDetailRow() != null && ucoJournals.GetSelectedDetailRow().JournalStatus !=
                             MFinanceConstants.BATCH_CANCELLED);

                        this.ucoJournals.UpdateHeaderTotals(ucoBatches.GetSelectedDetailRow());
                    }
                }
                else if (ATab == TGLBatchEnums.eGLTabs.Transactions)
                {
                    if ((FPreviouslySelectedTab == TGLBatchEnums.eGLTabs.Transactions) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    if (this.tpgTransactions.Enabled)
                    {
                        ABatchRow batchRow = ucoBatches.GetSelectedDetailRow();

                        string loadingMessage = string.Empty;
                        bool batchWasPreviousTab = (FPreviouslySelectedTab == TGLBatchEnums.eGLTabs.Batches);

                        FPreviouslySelectedTab = TGLBatchEnums.eGLTabs.Transactions;

                        // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                        this.tabGLBatch.SelectedTab = this.tpgTransactions;

                        if (batchWasPreviousTab)
                        {
                            //This only happens when the user clicks from Batch to Transactions,
                            //  which is only allowed when one journal exists

                            //Need to make sure that the Journal is loaded
                            LoadJournals(batchRow);
                        }

                        GLBatchTDSAJournalRow journalRow = ucoJournals.GetSelectedDetailRow();

                        if ((batchRow == null) || batchRow.IsBatchStatusNull())
                        {
                            loadingMessage = Catalog.GetString(
                                "There has been a problem loading the batch, please reselect the Batch tab and batch again and then retry.");
                        }
                        else if ((journalRow == null) || journalRow.IsJournalStatusNull())
                        {
                            loadingMessage = Catalog.GetString(
                                "There has been a problem loading the journals, please reselect the Batch tab and then the Journal tab and then retry.");
                        }

                        if (loadingMessage.Length == 0)
                        {
                            LoadTransactions(journalRow.BatchNumber,
                                batchRow.BatchStatus,
                                journalRow.JournalNumber,
                                journalRow.JournalStatus,
                                journalRow.TransactionCurrency,
                                batchWasPreviousTab);

                            //Warn if missing International Exchange Rate
                            WarnAboutMissingIntlExchangeRate = true;
                            GetInternationalCurrencyExchangeRate();
                        }
                        else
                        {
                            MessageBox.Show(loadingMessage,
                                "View Transactions",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Refresh();
            }
        }

        private void SelectTabManual(Int32 ASelectedTabIndex)
        {
            switch (ASelectedTabIndex)
            {
                case (int)TGLBatchEnums.eGLTabs.Batches:
                    SelectTab(TGLBatchEnums.eGLTabs.Batches);
                    break;

                case (int)TGLBatchEnums.eGLTabs.Journals:
                    SelectTab(TGLBatchEnums.eGLTabs.Journals);
                    break;

                default: //(ASelectedTabIndex == (int)TGLBatchEnums.eGLTabs.Transactions)
                    SelectTab(TGLBatchEnums.eGLTabs.Transactions);
                    break;
            }
        }

        /// <summary>
        /// Load Journals for current Batch
        /// </summary>
        /// <param name="ACurrentBatchRow"></param>
        public void LoadJournals(ABatchRow ACurrentBatchRow)
        {
            this.ucoJournals.LoadJournals(ACurrentBatchRow);
        }

        /// <summary>
        /// Load Transactions for current Batch and journal
        /// </summary>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AJournalStatus"></param>
        /// <param name="ATransactionCurrency"></param>
        /// <param name="AFromBatchTab"></param>
        public void LoadTransactions(Int32 ABatchNumber,
            String ABatchStatus,
            Int32 AJournalNumber,
            String AJournalStatus,
            String ATransactionCurrency,
            bool AFromBatchTab)
        {
            try
            {
                this.ucoTransactions.LoadTransactions(FLedgerNumber,
                    ABatchNumber,
                    AJournalNumber,
                    ATransactionCurrency,
                    AFromBatchTab,
                    ABatchStatus,
                    AJournalStatus);
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
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

        private int GetChangedRecordCountManual(out string AMessage)
        {
            // For GL Batch we will get some mix of batches, journals and transactions
            // Only check relevant tables.
            List <string>TablesToCheck = new List <string>();
            TablesToCheck.Add(FMainDS.ABatch.TableName);
            TablesToCheck.Add(FMainDS.AJournal.TableName);
            TablesToCheck.Add(FMainDS.ATransaction.TableName);
            TablesToCheck.Add(FMainDS.ATransAnalAttrib.TableName);

            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int AllChangesCount = 0;

            if (FMainDS.HasChanges())
            {
                // Work out how many changes in each table
                foreach (DataTable dt in FMainDS.GetChanges().Tables)
                {
                    string currentTableName = dt.TableName;

                    if ((dt != null)
                        && TablesToCheck.Contains(currentTableName)
                        && (dt.Rows.Count > 0))
                    {
                        int tableChangesCount = 0;

                        DataTable dtChanges = dt.GetChanges();

                        foreach (DataRow dr in dtChanges.Rows)
                        {
                            if (DataUtilities.DataRowColumnsHaveChanged(dr))
                            {
                                tableChangesCount++;
                                AllChangesCount++;
                            }
                        }

                        if (tableChangesCount > 0)
                        {
                            TableAndCountList.Add(new Tuple <string, int>(currentTableName, tableChangesCount));
                        }
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

                AMessage += Environment.NewLine + Catalog.GetString("(some of the changes may include related background items)");
                AMessage += Environment.NewLine;
                AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
            }

            return AllChangesCount;
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
        /// Uses the current Batch effective date to return the
        ///   corporate exchange rate value
        /// </summary>
        /// <returns></returns>
        public decimal GetInternationalCurrencyExchangeRate()
        {
            string NotUsed = string.Empty;

            ABatchRow BatchRow = ucoBatches.GetSelectedDetailRow();

            if (BatchRow == null)
            {
                return 0;
            }

            return GetInternationalCurrencyExchangeRate(BatchRow.DateEffective, out NotUsed);
        }

        /// <summary>
        /// Returns the corporate exchange rate for a given batch row
        ///  and specifies whether or not the transaction is in International
        ///  currency
        /// </summary>
        /// <param name="AEffectiveDate"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        public decimal GetInternationalCurrencyExchangeRate(DateTime AEffectiveDate, out string AErrorMessage)
        {
            DateTime StartOfMonth = new DateTime(AEffectiveDate.Year, AEffectiveDate.Month, 1);
            string LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            string LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            AErrorMessage = string.Empty;

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

                    // if no rate exists for the choosen period
                    if (IntlToBaseCurrencyExchRate == 0)
                    {
                        AErrorMessage =
                            String.Format(Catalog.GetString("No Corporate Exchange rate exists for {0} to {1} for the month: {2:MMMM yyyy}!"),
                                LedgerBaseCurrency,
                                LedgerIntlCurrency,
                                AEffectiveDate);

                        // show message immediately
                        if (FWarnAboutMissingIntlExchangeRate)
                        {
                            FWarnAboutMissingIntlExchangeRate = false;

                            MessageBox.Show(AErrorMessage, "Lookup Corporate Exchange Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }

            return IntlToBaseCurrencyExchRate;
        }

        // This manual method lets us peek at the data that is about to be saved...
        // The data has already been collected from the contols and validated and there is definitely something to save...
        private TSubmitChangesResult StoreManualCode(ref GLBatchTDS SubmitDS, out TVerificationResultCollection AVerificationResult)
        {
            FLatestSaveIncludedForex = false;

            if (SubmitDS.AJournal != null)
            {
                // Check whether we are saving any rows that are in foreign currency
                foreach (AJournalRow row in SubmitDS.AJournal.Rows)
                {
                    if (row.BaseCurrency != row.TransactionCurrency)
                    {
                        FLatestSaveIncludedForex = true;
                        break;
                    }
                }
            }

            //Do only at posting and when first entering transactions
            //---------------------------------------------------------
            // Check if corporate exchange rate exists for any new batches or modified batches.
            // Note: all previously saved batches will have an exchange rate for that saved batch date as
            // it is impossible to delete a corporate exchange rate if batches exist that need it.
            //string ErrorMessage = ValidateCorporateExchangeRate(SubmitDS);

            //if (!string.IsNullOrEmpty(ErrorMessage))
            //{
            //    AVerificationResult = new TVerificationResultCollection();
            //    TScreenVerificationResult Verification = null;

            //    Verification = new TScreenVerificationResult(
            //        new TVerificationResult(this, ErrorMessage, TResultSeverity.Resv_Noncritical),
            //        null, null);

            //    // Handle addition/removal to/from TVerificationResultCollection
            //    AVerificationResult.Auto_Add_Or_AddOrRemove(this, Verification, null, true);

            //    return TSubmitChangesResult.scrError;
            //}

            // Now do the standard call to save the changes
            return TRemote.MFinance.GL.WebConnectors.SaveGLBatchTDS(ref SubmitDS, out AVerificationResult);
        }

        private void FileSaveManual(object sender, EventArgs e)
        {
            SaveChangesManual();
        }

        /// <summary>
        /// Check for ExWorkers before saving
        /// </summary>
        /// <returns>True if Save is successful</returns>
        public bool SaveChangesManual()
        {
            FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.SAVING;
            return SaveChangesManual(FCurrentGLBatchAction);
        }

        /// <summary>
        /// Save according to current batch action
        /// </summary>
        /// <param name="AAction"></param>
        /// <param name="AGetJournalDataFromControls"></param>
        /// <param name="AGetTransDataFromControls"></param>
        /// <returns>True if Save is successful</returns>
        public bool SaveChangesManual(TGLBatchEnums.GLBatchAction AAction,
            bool AGetJournalDataFromControls = false,
            bool AGetTransDataFromControls = false)
        {
            if (AAction == TGLBatchEnums.GLBatchAction.NONE)
            {
                AAction = TGLBatchEnums.GLBatchAction.SAVING;
                FCurrentGLBatchAction = AAction;
            }

            if (AAction == TGLBatchEnums.GLBatchAction.CANCELLING)
            {
                if (AGetJournalDataFromControls)
                {
                    ucoJournals.GetDataFromControls();
                }

                if (AGetTransDataFromControls)
                {
                    ucoTransactions.GetDataFromControls();
                }
            }
            else if (AAction == TGLBatchEnums.GLBatchAction.CANCELLINGJOURNAL)
            {
                ucoBatches.GetDataFromControls();

                if (AGetTransDataFromControls)
                {
                    ucoTransactions.GetDataFromControls();
                }
            }
            else if ((AAction == TGLBatchEnums.GLBatchAction.DELETINGTRANS)
                     || (AAction == TGLBatchEnums.GLBatchAction.DELETINGALLTRANS))
            {
                ucoBatches.GetDataFromControls();
                ucoJournals.GetDataFromControls();
            }
            else
            {
                GetDataFromControls();
            }

            return SaveChanges();
        }

        // Before the dataset is saved, check for correlation between batch and transactions
        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            ucoBatches.CheckBeforeSaving();
            ucoJournals.CheckBeforeSaving();
            ucoTransactions.CheckBeforeSaving();
        }

        private void FPetraUtilsObject_DataSavingValidated(object Sender, CancelEventArgs e)
        {
            int BatchNumber = GetBatchControl().GetCurrentBatchRow().BatchNumber;
            int JournalNumber = 0;
            int TransactionNumber = 0;

            if (FCurrentGLBatchAction == TGLBatchEnums.GLBatchAction.NONE)
            {
                FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.SAVING;
            }
            else if ((FCurrentGLBatchAction == TGLBatchEnums.GLBatchAction.CANCELLINGJOURNAL)
                     || (FCurrentGLBatchAction == TGLBatchEnums.GLBatchAction.DELETINGALLTRANS))
            {
                JournalNumber = GetJournalsControl().GetCurrentJournalRow().JournalNumber;
            }
            else if (FCurrentGLBatchAction == TGLBatchEnums.GLBatchAction.DELETINGTRANS)
            {
                JournalNumber = GetJournalsControl().GetCurrentJournalRow().JournalNumber;
                TransactionNumber = GetTransactionsControl().GetCurrentTransactionRow().TransactionNumber;
            }

            //Check if the user has made a Bank Cost Centre or Account Code inactive
            // on saving
            if (!ucoTransactions.AllowInactiveFieldValues(FLedgerNumber, BatchNumber, FCurrentGLBatchAction,
                    JournalNumber, TransactionNumber))
            {
                e.Cancel = true;
            }
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
        /// find a specific transaction
        /// </summary>
        public void FindGLTransaction(int ABatchNumber, int AJournalNumber, int ATransactionNumber)
        {
            ucoTransactions.SelectTransactionNumber(ATransactionNumber);
            FStandardTabIndex = 2;     // later we switch to the transaction tab
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

            TFrmBatchPostingRegister ReportGui = new TFrmBatchPostingRegister(this);
//          ReportGui.PrintReportNoUi(FLedgerNumber, BatchRow.BatchNumber); // This alternative doesn't show the UI

            ReportGui.LedgerNumber = FLedgerNumber;
            ReportGui.BatchNumber = BatchRow.BatchNumber;
            ReportGui.Show();
        }

        /// <summary>
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void RefreshAll(bool AIsFromMessage = false)
        {
            ucoBatches.ReloadBatches(AIsFromMessage);
        }

        /// <summary>
        /// Needs to be called prior to posting current batch to ensure all data is up-to-date
        /// </summary>
        public void GetLatestControlData()
        {
            GetDataFromControls();
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

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>The Partner Edit 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcRefreshGLBatches)
            {
                this.RefreshAll(true);

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}