//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.ComponentModel;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftBatch : IFrmPetraEditManual
    {
        private Int32 FLedgerNumber;
        private Boolean FViewMode = false;

        private GiftBatchTDS FViewModeTDS;
        private int standardTabIndex = 0;
        private bool FNewDonorWarning = true;
        private bool FWarnAboutMissingIntlExchangeRate = false;
        // changed gift records
        GiftBatchTDSAGiftDetailTable FGiftDetailTable = null;

        // Variables that are used to select a specific batch on startup
        private Int32 FInitialBatchNumber = -1;
        private Int32 FInitialBatchYear = -1;

        private Boolean FLatestSaveIncludedForex = false;

        /// ViewMode is a special mode where the whole window with all tabs is in a readonly mode
        public bool ViewMode {
            get
            {
                return FViewMode;
            }
            set
            {
                FViewMode = value;
            }
        }

        /// ViewModeTDS is for injection of the Datasets in the View Mode
        public GiftBatchTDS ViewModeTDS
        {
            get
            {
                return FViewModeTDS;
            }
            set
            {
                FViewModeTDS = value;
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

        /* Be sure to leave the LedgerNumber property in this position in the code
         * AFTER the ones above.   Otherwise the code will not work that opens this
         * screen at a pre-defined batch or in ViewMode.*/

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                // setting the ledger number on the batch screen will automatically trigger loading the batches for the current year
                ucoBatches.LedgerNumber = value;

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                //Enable below if want code to run before standard Save() is executed
                FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
            }
        }

        /// <summary>
        /// Show the user a message when a gift is entered for a new donor
        /// </summary>
        public bool NewDonorWarning
        {
            set
            {
                FNewDonorWarning = value;
            }
        }

        /// <summary>
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void RefreshAll()
        {
            ucoBatches.RefreshAllData();
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
            return SaveChangesManual(TExtraGiftBatchChecks.GiftBatchAction.SAVING);
        }

        /// <summary>
        /// Check for ExWorkers before saving or cancelling
        /// </summary>
        /// <returns>True if Save is successful</returns>
        public bool SaveChangesManual(TExtraGiftBatchChecks.GiftBatchAction AAction)
        {
            GetDataFromControls();

            // first alert the user to any recipients who are Ex-Workers
            if (TExtraGiftBatchChecks.CanContinueWithAnyExWorkers(AAction, FMainDS, FPetraUtilsObject))
            {
                return SaveChanges();
            }

            return false;
        }

        /// <summary>
        /// Checks to be made before saving and posting
        /// </summary>
        /// <param name="APostingGiftDetails">GiftDetails for the batch that is to be posted</param>
        /// <param name="ACancelledDueToExWorkerOrAnonDonor">True if batch posting has been cancelled by the user because of an Ex-Worker recipient
        /// or an anonymous donor for a gift that is not marked as confidential</param>
        /// <returns>True if Save is successful</returns>
        public bool SaveChangesForPosting(DataTable APostingGiftDetails, out bool ACancelledDueToExWorkerOrAnonDonor)
        {
            GetDataFromControls();

            // first alert the user to any recipients who are Ex-Workers
            ACancelledDueToExWorkerOrAnonDonor = !TExtraGiftBatchChecks.CanContinueWithAnyExWorkers(TExtraGiftBatchChecks.GiftBatchAction.POSTING,
                FMainDS,
                FPetraUtilsObject,
                APostingGiftDetails);

            // if save is continuing then alert the user to any gift that are not marked confidential but have an anonymous donor
            if (!ACancelledDueToExWorkerOrAnonDonor)
            {
                ACancelledDueToExWorkerOrAnonDonor = !TExtraGiftBatchChecks.CanContinueWithAnyAnonymousDonors(FMainDS);
            }

            if (!ACancelledDueToExWorkerOrAnonDonor)
            {
                return SaveChanges();
            }

            return false;
        }

        // Before the dataset is saved, check for correlation between batch and transactions
        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            ucoBatches.CheckBeforeSaving();
            ucoTransactions.CheckBeforeSaving();

            if (FNewDonorWarning)
            {
                FPetraUtilsObject_DataSavingStarted_NewDonorWarning();
            }
        }

        // This manual method lets us peek at the data that is about to be saved...
        // The data has already been collected from the contols and validated and there is definitely something to save...
        private TSubmitChangesResult StoreManualCode(ref GiftBatchTDS SubmitDS, out TVerificationResultCollection VerificationResult)
        {
            FLatestSaveIncludedForex = false;

            if (SubmitDS.AGiftBatch != null)
            {
                // Check whether we are saving any rows that are in foreign currency
                DataView dv = new DataView(SubmitDS.AGiftBatch,
                    String.Format("{0}<>'{1}'", AGiftBatchTable.GetCurrencyCodeDBName(), FMainDS.ALedger[0].BaseCurrency),
                    String.Empty, DataViewRowState.CurrentRows);
                FLatestSaveIncludedForex = (dv.Count > 0);
            }

            // Now do the standard call to save the changes
            return TRemote.MFinance.Gift.WebConnectors.SaveGiftBatchTDS(ref SubmitDS, out VerificationResult);
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (FNewDonorWarning)
            {
                FPetraUtilsObject_DataSaved_NewDonorWarning(Sender, e);
            }

            if (e.Success && FLatestSaveIncludedForex)
            {
                // Notify the exchange rate screen, if it is there
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcGLOrGiftBatchSaved, this.ToString());
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
        }

        private void FPetraUtilsObject_DataSavingStarted_NewDonorWarning()
        {
            GetDataFromControls();

            FGiftDetailTable = FMainDS.AGiftDetail.GetChangesTyped();
        }

        private void FPetraUtilsObject_DataSaved_NewDonorWarning(object Sender, TDataSavedEventArgs e)
        {
            // if data successfully saved then look for new donors and warn the user
            if (e.Success && (FGiftDetailTable != null) && FNewDonorWarning)
            {
                // this list contains a list of all new donors that were entered onto form
                List <Int64>NewDonorsList = ucoTransactions.NewDonorsList;

                foreach (GiftBatchTDSAGiftDetailRow Row in FGiftDetailTable.Rows)
                {
                    // check changed data is either added or modified and that it is by a new donor
                    if (((Row.RowState == DataRowState.Added) || (Row.RowState == DataRowState.Modified))
                        && (!Row.IsDonorKeyNull() && NewDonorsList.Contains(Row.DonorKey)))
                    {
                        if (MessageBox.Show(string.Format(Catalog.GetString(
                                        "{0} ({1}) is a new Donor.{2}Do you want to add subscriptions for them?{2}" +
                                        "(Note: this message can be disabled in the 'File' menu by unselecting the 'New Donor Warning' item.)"),
                                    Row.DonorName, Row.DonorKey, "\n\n"),
                                Catalog.GetString("New Donor"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Open the donor's Edit screen so subscriptions can be added
                            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                            frm.SetParameters(TScreenMode.smEdit, Row.DonorKey, TPartnerEditTabPageEnum.petpSubscriptions);
                            frm.ShowDialog();
                        }

                        // ensures message is not displayed twice for one new donor with two gifts
                        NewDonorsList.Remove(Row.DonorKey);
                    }
                }

                ucoTransactions.NewDonorsList.Clear();
            }
        }

        private void InitializeManualCode()
        {
            tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            this.tpgTransactions.Enabled = false;

            // get user default
            FNewDonorWarning = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_NEW_DONOR_WARNING, true);
            mniNewDonorWarning.Checked = FNewDonorWarning;

            // only add this event if the user want a new donor warning (this will still work without the condition)
            if (FNewDonorWarning)
            {
                FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
            }

            mniPrintGiftBatchDetail.Enabled = true;

            // change the event that gets called when 'Save' is clicked (i.e. changed from generated code)
            tbbSave.Click -= FileSave;
            mniFileSave.Click -= FileSave;
            tbbSave.Click += FileSaveManual;
            mniFileSave.Click += FileSaveManual;
        }

        /// <summary>
        /// Handles the click event for filter/find.
        /// </summary>
        /// <param name="sender">Pass this on to the user control.</param>
        /// <param name="e">Not evaluated.</param>
        public void MniFilterFind_Click(object sender, System.EventArgs e)
        {
            switch (tabGiftBatch.SelectedIndex)
            {
                case (int)eGiftTabs.Batches:
                    ucoBatches.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGiftTabs.Transactions:
                    ucoTransactions.ReconcileKeyMinistryFromCombo();
                    ucoTransactions.MniFilterFind_Click(sender, e);
                    break;
            }
        }

        private void TFrmGiftBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGiftBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null); //tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            this.Shown += delegate
            {
                // This will ensure the grid gets the focus when the screen is shown for the first time
                ucoBatches.SetInitialFocus();
            };
        }

        private void RunOnceOnActivationManual()
        {
            ucoBatches.Focus();
            HookupAllInContainer(ucoBatches);
            HookupAllInContainer(ucoTransactions);
        }

        /// <summary>
        /// activate the transactions tab and load the gift transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <returns>True if new transactions were actually loaded, False if transactions have already been loaded for the ledger/batch</returns>
        public bool LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            return this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber, ABatchStatus);
        }

        /// <summary>
        /// this should be called when all data is reloaded after posting
        /// </summary>
        public void ClearCurrentSelections()
        {
            if (this.ucoBatches != null)
            {
                this.ucoBatches.ClearCurrentSelection();
            }

            if (this.ucoTransactions != null)
            {
                this.ucoTransactions.ClearCurrentSelection();
            }
        }

        /// enable the transaction tab page
        public void EnableTransactions(bool AEnable = true)
        {
            this.tpgTransactions.Enabled = AEnable;
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void DisableTransactions()
        {
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// disable the batches tab
        /// </summary>
        public void DisableBatches()
        {
            this.tpgBatches.Enabled = false;
        }

        /// <summary>
        /// directly access the batches control
        /// </summary>
        public TUC_GiftBatches GetBatchControl()
        {
            return ucoBatches;
        }

        /// <summary>
        /// directly access the transactions control
        /// </summary>
        public TUC_GiftTransactions GetTransactionsControl()
        {
            return ucoTransactions;
        }

        /// this window contains 2 tabs
        public enum eGiftTabs
        {
            /// list of batches
            Batches,

            /// list of transactions
            Transactions
        };

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(false, true))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
        }

        private void SelectTabManual(int ASelectedTabIndex)
        {
            if (ASelectedTabIndex == (int)eGiftTabs.Batches)
            {
                SelectTab(eGiftTabs.Batches);
            }
            else
            {
                SelectTab(eGiftTabs.Transactions);
            }
        }

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGiftTabs ATab)
        {
            FPetraUtilsObject.RestoreAdditionalWindowPositionProperties();

            if (ATab == eGiftTabs.Batches)
            {
                this.tabGiftBatch.SelectedTab = this.tpgBatches;
                this.tpgTransactions.Enabled = (ucoBatches.GetSelectedDetailRow() != null);
                this.ucoBatches.SetFocusToGrid();
            }
            else if (ATab == eGiftTabs.Transactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                    this.tabGiftBatch.SelectedTab = this.tpgTransactions;

                    AGiftBatchRow SelectedRow = ucoBatches.GetSelectedDetailRow();

                    // If there's only one GiftBatch row, I'll not require that the user has selected it!
                    if (FMainDS.AGiftBatch.Rows.Count == 1)
                    {
                        SelectedRow = FMainDS.AGiftBatch[0];
                    }

                    if (SelectedRow != null)
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;

                            LoadTransactions(SelectedRow.LedgerNumber,
                                SelectedRow.BatchNumber,
                                SelectedRow.BatchStatus);
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }

                    ucoTransactions.FocusGrid();
                }
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
                    SaveChangesManual();
                }

                return true;
            }

            if ((tabGiftBatch.SelectedTab == tpgBatches) && (ucoBatches.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabGiftBatch.SelectedTab == tpgTransactions) && (ucoTransactions.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Ensure the data is loaded for the specified batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>If transactions exist</returns>
        public Boolean EnsureGiftDataPresent(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            DataView TransDV = new DataView(FMainDS.AGiftDetail);

            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                AGiftDetailTable.GetLedgerNumberDBName(),
                ALedgerNumber,
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            if (TransDV.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadGiftAndTaxDeductDataForBatch(ALedgerNumber, ABatchNumber));
            }

            return TransDV.Count > 0;
        }

        /// <summary>
        /// find a special gift detail
        /// </summary>
        public void FindGiftDetail(AGiftDetailRow gdr)
        {
            ucoBatches.SelectBatchNumber(gdr.BatchNumber);
            ucoTransactions.SelectGiftDetailNumber(gdr.GiftTransactionNumber, gdr.DetailNumber);
            standardTabIndex = 1;     // later we switch to the detail tab
        }

        private int GetChangedRecordCountManual(out string AMessage)
        {
            // For Gift Batch we will
            //  either get a change to N Batches
            //  or get changes to M transactions in N Batches
            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int allChangesCount = 0;

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
                if (TableAndCountList.Count == 1)
                {
                    // Only saving changes to batches
                    Tuple <string, int>TableAndCount = TableAndCountList[0];

                    AMessage = String.Format(Catalog.GetString("    You have made changes to the details of {0} {1}.{2}"),
                        TableAndCount.Item2,
                        Catalog.GetPluralString("batch", "batches", TableAndCount.Item2),
                        Environment.NewLine);
                }
                else
                {
                    // Saving changes to transactions as well
                    int nBatches = 0;
                    int nTransactions = 0;

                    foreach (Tuple <string, int>TableAndCount in TableAndCountList)
                    {
                        if (TableAndCount.Item1.Equals(AGiftBatchTable.GetTableName()))
                        {
                            nBatches = TableAndCount.Item2;
                        }
                        else if (TableAndCount.Item2 > nTransactions)
                        {
                            nTransactions = TableAndCount.Item2;
                        }
                    }

                    if (nBatches == 0)
                    {
                        AMessage = String.Format(Catalog.GetString("    You have made changes to {0} {1}.{2}"),
                            nTransactions,
                            Catalog.GetPluralString("transaction", "transactions", nTransactions),
                            Environment.NewLine);
                    }
                    else
                    {
                        AMessage = String.Format(Catalog.GetString("    You have made changes to {0} {1} and {2} {3}.{4}"),
                            nBatches,
                            Catalog.GetPluralString("batch", "batches", nBatches),
                            nTransactions,
                            Catalog.GetPluralString("transaction", "transactions", nTransactions),
                            Environment.NewLine);
                    }
                }

                AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
            }

            return allChangesCount;
        }

        /// <summary>
        /// Check if batch columns have actually changed
        /// </summary>
        /// <param name="ABatchRow"></param>
        /// <returns></returns>
        public bool BatchColumnsHaveChanged(AGiftBatchRow ABatchRow)
        {
            bool RetVal = false;

            if (ABatchRow.RowState != DataRowState.Unchanged)
            {
                bool columnValueChanged = false;

                for (int i = 0; i < FMainDS.AGiftBatch.Columns.Count; i++)
                {
                    string originalValue = ABatchRow[i, DataRowVersion.Original].ToString();
                    string currentValue = ABatchRow[i, DataRowVersion.Current].ToString();

                    if (originalValue != currentValue)
                    {
                        columnValueChanged = true;
                        break;
                    }
                }

                if (!columnValueChanged)
                {
                    ABatchRow.RejectChanges();
                }

                RetVal = columnValueChanged;
            }

            return RetVal;
        }

        /// <summary>
        /// Set up the screen to show only this batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchYear"></param>
        /// <param name="ABatchPeriod"></param>
        public void ShowDetailsOfOneBatch(Int32 ALedgerNumber, Int32 ABatchNumber, int ABatchYear, int ABatchPeriod)
        {
            FLedgerNumber = ALedgerNumber;
            ucoBatches.LoadOneBatch(ALedgerNumber, ABatchNumber, ABatchYear, ABatchPeriod);
            Show();
        }

        /// <summary>
        /// Returns the corporate exchange rate for a given batch row
        ///  and specifies whether or not the transaction is in International
        ///  currency
        /// </summary>
        /// <param name="ABatchRow"></param>
        /// <param name="AIsTransactionInIntlCurrency"></param>
        /// <param name="AAlwaysReportError"></param>
        /// <returns></returns>
        public decimal InternationalCurrencyExchangeRate(AGiftBatchRow ABatchRow,
            out bool AIsTransactionInIntlCurrency, bool AAlwaysReportError = false)
        {
            decimal IntlToBaseCurrencyExchRate = 1;

            AIsTransactionInIntlCurrency = false;

            string BatchCurrencyCode = ABatchRow.CurrencyCode;
            decimal BatchExchangeRateToBase = ABatchRow.ExchangeRateToBase;
            DateTime BatchEffectiveDate = ABatchRow.GlEffectiveDate;
            DateTime StartOfMonth = new DateTime(BatchEffectiveDate.Year, BatchEffectiveDate.Month, 1);
            string LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            string LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            if (LedgerBaseCurrency == LedgerIntlCurrency)
            {
                IntlToBaseCurrencyExchRate = 1;
            }
            else if (BatchCurrencyCode == LedgerIntlCurrency)
            {
                AIsTransactionInIntlCurrency = true;
            }
            else
            {
                IntlToBaseCurrencyExchRate = TRemote.MFinance.GL.WebConnectors.GetCorporateExchangeRate(LedgerBaseCurrency,
                    LedgerIntlCurrency,
                    StartOfMonth,
                    BatchEffectiveDate);

                if ((IntlToBaseCurrencyExchRate == 0) && (FWarnAboutMissingIntlExchangeRate || AAlwaysReportError))
                {
                    FWarnAboutMissingIntlExchangeRate = false;

                    string IntlRateErrorMessage =
                        String.Format(Catalog.GetString("No Corporate Exchange rate exists for {0} to {1} for the month: {2:MMMM yyyy}!"),
                            LedgerBaseCurrency,
                            LedgerIntlCurrency,
                            BatchEffectiveDate);

                    MessageBox.Show(IntlRateErrorMessage, Catalog.GetString(
                            "Lookup Corporate Exchange Rate"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return IntlToBaseCurrencyExchRate;
        }

        private void mniNewDonorWarning_Click(Object sender, EventArgs e)
        {
            // toggle menu tick
            mniNewDonorWarning.Checked = !mniNewDonorWarning.Checked;

            FNewDonorWarning = mniNewDonorWarning.Checked;

            // change user default
            TUserDefaults.SetDefault(TUserDefaults.FINANCE_NEW_DONOR_WARNING, FNewDonorWarning);
        }

        // open screen to print the Gift Batch Detail report
        private void PrintGiftBatchDetail(Object sender, EventArgs e)
        {
            TFrmGiftBatchDetail Report = new TFrmGiftBatchDetail(this);

            Report.LedgerNumber = FLedgerNumber;
            Report.BatchNumber = ucoBatches.FSelectedBatchNumber;
            Report.Show();
        }

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

            // update gift destination
            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcGiftDestinationChanged)
            {
                ucoTransactions.ProcessGiftDetainationBroadcastMessage(AFormsMessage);

                MessageProcessed = true;
            }
            else if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcUnitHierarchyChanged)
            {
                ucoTransactions.ProcessUnitHierarchyBroadcastMessage(AFormsMessage);

                MessageProcessed = true;
            }
            else if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcRefreshGiftBatches)
            {
                this.RefreshAll();

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}