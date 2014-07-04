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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftBatch
    {
        private Int32 FLedgerNumber;
        private Boolean FViewMode = false;
        private bool FWindowIsMaximized = false;

        private GiftBatchTDS FViewModeTDS;
        private int standardTabIndex = 0;
        private bool FNewDonorWarning = true;

        // changed gift records
        private GiftBatchTDSAGiftDetailTable FGiftDetailTable = null;


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
        /// International To Base Exchange Rate
        /// </summary>
        public decimal FInternationalToBaseExchangeRate;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                ucoBatches.LoadBatches(FLedgerNumber);

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
            ucoBatches.RefreshAll();
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

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
        	if (FNewDonorWarning)
            {
            	FPetraUtilsObject_DataSaved_NewDonorWarning(Sender, e);
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
                    ucoTransactions.ReconcileKeyMinistryControls();
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

        private void FPetraUtilsObject_DataSavingStarted_NewDonorWarning()
        {
            if (FNewDonorWarning)
            {
                // add changed gift records to datatable
                GetDataFromControls();
                FGiftDetailTable = FMainDS.GetChangesTyped(false).AGiftDetail;
            }
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
                        && NewDonorsList.Contains(Row.DonorKey))
                    {
                        if (MessageBox.Show(string.Format(Catalog.GetString(
                                        "{0} ({1}) is a new Donor.{2}Do you want to add subscriptions for them?{2}" +
                                        "(Note: this message can be disabled by selecting from the menu File then New Donor Warning.)"),
                                    Row.DonorName, Row.DonorKey, "\n\n"),
                                Catalog.GetString("New Donor"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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

        private void RunOnceOnActivationManual()
        {
            ucoBatches.Focus();
            HookupAllInContainer(ucoBatches);
            HookupAllInContainer(ucoTransactions);
            this.Resize += new EventHandler(TFrmGiftBatch_Resize);
        }

        void TFrmGiftBatch_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                // set the flag that we are maximized
                FWindowIsMaximized = true;

                if (tabGiftBatch.SelectedTab == this.tpgBatches)
                {
                    ucoTransactions.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing transactions");
                }
                else
                {
                    ucoBatches.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing batches");
                }
            }
            else if (FWindowIsMaximized && (this.WindowState == FormWindowState.Normal))
            {
                // we have been maximized but now are normal.  In this case we need to re-autosize the cells because otherwise they are still 'stretched'.
                ucoBatches.AutoSizeGrid();
                ucoTransactions.AutoSizeGrid();
                FWindowIsMaximized = false;
                Console.WriteLine("Normal - autosizing both");
            }
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
                    // But we need this to be set before calling ucoTransactions.AutoSizeGrid() because that only works once the page is actually loaded.
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

                            if (LoadTransactions(SelectedRow.LedgerNumber, SelectedRow.BatchNumber,
                                    SelectedRow.BatchStatus))
                            {
                                // We will only call this on the first time through (if we are called twice the second time will not actually load new transactions)
                                ucoTransactions.AutoSizeGrid();
                            }
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
                    SaveChanges();
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
        /// Check for any errors
        /// </summary>
        /// <param name="AShowMessage"></param>
        public void ProcessRecipientCostCentreCodeUpdateErrors(bool AShowMessage = true)
        {
            //Process update errors
            if (FMainDS.Tables.Contains("AUpdateErrors"))
            {
                //TODO remove this code when the worker field issue is sorted out
                AShowMessage = false;

                //--------------------------------------------------------------
                if (AShowMessage)
                {
                    string loadErrors = FMainDS.Tables["AUpdateErrors"].Rows[0].ItemArray[0].ToString();

                    MessageBox.Show(String.Format("Errors occurred in updating gift data:{0}{0}{1}",
                            Environment.NewLine,
                            loadErrors), "Update Gift Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                FMainDS.Tables.Remove("AUpdateErrors");
            }
        }

        /// <summary>
        /// Set up the screen to show only this batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void ShowDetailsOfOneBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            FLedgerNumber = ALedgerNumber;
            ucoBatches.LoadOneBatch(ALedgerNumber, ABatchNumber);
            Show();
        }

        /// <summary>
        /// Returns the corporate exchange rate for a given batch row
        ///  and specifies whether or not the transaction is in International
        ///  currency
        /// </summary>
        /// <param name="ABatchRow"></param>
        /// <param name="AIsTransactionInIntlCurrency"></param>
        /// <returns></returns>
        public decimal InternationalCurrencyExchangeRate(AGiftBatchRow ABatchRow,
            out bool AIsTransactionInIntlCurrency)
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

                if (IntlToBaseCurrencyExchRate == 0)
                {
                    string IntlRateErrorMessage = String.Format("No corporate exchange rate exists for {0} to {1} for the date: {2}!",
                        LedgerBaseCurrency,
                        LedgerIntlCurrency,
                        BatchEffectiveDate);

                    MessageBox.Show(IntlRateErrorMessage, "Lookup Corporate Exchange Rate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return IntlToBaseCurrencyExchRate;
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

        private void mniNewDonorWarning_Click(Object sender, EventArgs e)
        {
            // toggle menu tick
            mniNewDonorWarning.Checked = !mniNewDonorWarning.Checked;

            FNewDonorWarning = mniNewDonorWarning.Checked;

            // change user default
            TUserDefaults.SetDefault(TUserDefaults.FINANCE_NEW_DONOR_WARNING, FNewDonorWarning);
        }
    }
}