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
using Ict.Petra.Client.CommonForms;
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

        private GiftBatchTDS FViewModeTDS;
        private int standardTabIndex = 0;
        private bool FNewDonorWarning = true;
        private bool FWarnAboutMissingIntlExchangeRate = false;
        // changed gift records
        GiftBatchTDSAGiftDetailTable FGiftDetailTable = null;

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

        private void FPetraUtilsObject_DataSavingStarted_NewDonorWarning()
        {
            if (FNewDonorWarning)
            {
                if (FMainDS.GetChangesTyped(false) == null)
                {
                    FGiftDetailTable = null;
                    return;
                }

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

            return MessageProcessed;
        }

        #endregion
    }
}