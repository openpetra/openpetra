//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmRecurringGiftBatch
    {
        private Int32 FLedgerNumber;
        private int standardTabIndex = 0;
        private bool FWindowIsMaximized = false;


        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                ucoRecurringBatches.LoadBatches(FLedgerNumber);

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                //Enable below if want code to run before standard Save() is executed
                //FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
            }
        }

        /// <summary>
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void RefreshAll()
        {
            ucoRecurringBatches.RefreshAll();
        }

        // Before the dataset is saved, check for correlation between batch and transactions
        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            ucoRecurringBatches.CheckBeforeSavingBatch();
        }

        private void InitializeManualCode()
        {
            tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            this.tpgRecurringTransactions.Enabled = false;

            //FPetraUtilsObject.OnDataSavingStart
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
                    ucoRecurringBatches.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGiftTabs.Transactions:
                    ucoRecurringTransactions.MniFilterFind_Click(sender, e);
                    break;
            }
        }

        private void TFrmGiftBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGiftBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null);

            this.Shown += delegate
            {
                // This will ensure the grid gets the focus when the screen is shown for the first time
                ucoRecurringBatches.SetInitialFocus();
            };
        }

        private void RunOnceOnActivationManual()
        {
            ucoRecurringBatches.Focus();
            HookupAllInContainer(ucoRecurringBatches);
            HookupAllInContainer(ucoRecurringTransactions);
        }

        /// <summary>
        /// activate the transactions tab and load the gift transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>True if new transactions were actually loaded, False if transactions have already been loaded for the ledger/batch</returns>
        public bool LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            return this.ucoRecurringTransactions.LoadGifts(ALedgerNumber, ABatchNumber);
        }

        /// <summary>
        /// this should be called when all data is reloaded after posting
        /// </summary>
        public void ClearCurrentSelections()
        {
            if (this.ucoRecurringBatches != null)
            {
                this.ucoRecurringBatches.ClearCurrentSelection();
            }

            if (this.ucoRecurringTransactions != null)
            {
                this.ucoRecurringTransactions.ClearCurrentSelection();
            }
        }

        /// enable the transaction tab page
        public void EnableTransactionsTab(bool AEnable = true)
        {
            this.tpgRecurringTransactions.Enabled = AEnable;
        }

        /// enable the transaction tab page
        public void DisableTransactionsTab()
        {
            this.tpgRecurringTransactions.Enabled = false;
        }

        /// <summary>
        /// directly access the batches control
        /// </summary>
        public TUC_RecurringGiftBatches GetBatchControl()
        {
            return ucoRecurringBatches;
        }

        /// <summary>
        /// directly access the transactions control
        /// </summary>
        public TUC_RecurringGiftTransactions GetTransactionsControl()
        {
            return ucoRecurringTransactions;
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
                this.tabGiftBatch.SelectedTab = this.tpgRecurringBatches;
                this.tpgRecurringTransactions.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);
                this.ucoRecurringBatches.SetFocusToGrid();
            }
            else if (ATab == eGiftTabs.Transactions)
            {
                if (this.tpgRecurringTransactions.Enabled)
                {
                    // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                    this.tabGiftBatch.SelectedTab = this.tpgRecurringTransactions;

                    ARecurringGiftBatchRow SelectedRow = ucoRecurringBatches.GetSelectedDetailRow();

                    // If there's only one GiftBatch row, I'll not require that the user has selected it!
                    if (FMainDS.ARecurringGiftBatch.Rows.Count == 1)
                    {
                        SelectedRow = FMainDS.ARecurringGiftBatch[0];
                    }

                    if (SelectedRow != null)
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;

                            LoadTransactions(SelectedRow.LedgerNumber, SelectedRow.BatchNumber);
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }

                    this.ucoRecurringTransactions.FocusGrid();
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

            if ((tabGiftBatch.SelectedTab == tpgRecurringBatches) && (ucoRecurringBatches.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabGiftBatch.SelectedTab == tpgRecurringTransactions) && (ucoRecurringTransactions.ProcessParentCmdKey(ref msg, keyData)))
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
    }
}