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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLBatch
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                ucoBatches.LoadBatches(FLedgerNumber);

                ucoJournals.WorkAroundInitialization();
                ucoTransactions.WorkAroundInitialization();
            }
        }

        /// <summary>
        /// Handles the click event for filter/find.
        /// </summary>
        /// <param name="sender">Pass this on to the user control.</param>
        /// <param name="e">Not evaluated.</param>
        public void mniFilterFind_Click(object sender, System.EventArgs e)
        {
            switch (tabGLBatch.SelectedIndex)
            {
                case (int)eGLTabs.Batches:
                    ucoBatches.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGLTabs.Journals:
                    ucoJournals.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGLTabs.Transactions:
                    ucoTransactions.MniFilterFind_Click(sender, e);
                    break;
            }
        }

        private int standardTabIndex = 0;

        private void TFrmGLBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGLBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null); //tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            this.Shown += delegate
            {
                // This will ensure the grid gets the focus when the screen is shown for the first time
                ucoBatches.SetInitialFocus();
            };
        }

        private void InitializeManualCode()
        {
            tabGLBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            this.tpgJournals.Enabled = false;
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// Load the journals for the current batch in the background
        /// </summary>
        public void LoadJournals()
        {
            int batchNumber = ucoBatches.GetSelectedDetailRow().BatchNumber;

            FMainDS.AJournal.DefaultView.RowFilter = string.Format("{0} = {1}",
                AJournalTable.GetBatchNumberDBName(),
                batchNumber);

            // only load from server if there are no journals loaded yet for this batch
            // otherwise we would overwrite journals that have already been modified
            if (FMainDS.AJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(FLedgerNumber, batchNumber));
            }
        }

        /// <summary>
        /// activate the journal tab and load the journals of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            this.tpgJournals.Enabled = true;
            DisableTransactions();
            this.ucoJournals.LoadJournals(ALedgerNumber, ABatchNumber);
        }

        /// <summary>
        /// activate the transaction tab and load the transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AForeignCurrencyName"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, String AForeignCurrencyName)
        {
            this.tpgTransactions.Enabled = true;
            this.ucoTransactions.LoadTransactions(ALedgerNumber, ABatchNumber, AJournalNumber, AForeignCurrencyName);
            this.Refresh();
        }

        /// <summary>
        /// Unload transactions from the form
        /// </summary>
        public void UnloadTransactions()
        {
            this.ucoTransactions.UnloadTransactions();
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void DisableTransactions()
        {
            this.tpgTransactions.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void EnableTransactions(bool AEnable = true)
        {
            this.tpgTransactions.Enabled = AEnable;
            this.Refresh();
        }

        /// <summary>
        /// disable the journal tab if we have no active batch
        /// </summary>
        public void DisableJournals()
        {
            this.tabGLBatch.TabStop = false;

            this.tpgJournals.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals()
        {
            this.tabGLBatch.TabStop = true;

            if (!this.tpgJournals.Enabled)
            {
                this.tpgJournals.Enabled = true;
                this.Refresh();
            }
        }

        /// this window contains 4 tabs
        public enum eGLTabs
        {
            /// list of batches
            Batches,

            /// list of journals
            Journals,

            /// list of transactions
            Transactions
        };


        //Might need this later
        private eGLTabs FPreviousTab = eGLTabs.Batches;

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            if (ATab == eGLTabs.Batches)
            {
                this.tabGLBatch.SelectedTab = this.tpgBatches;
                this.tpgJournals.Enabled = (ucoBatches.GetSelectedDetailRow() != null);
                this.tabGLBatch.TabStop = this.tpgJournals.Enabled;

                if (this.tpgTransactions.Enabled)
                {
                    this.ucoTransactions.CancelChangesToFixedBatches();
                    this.ucoJournals.CancelChangesToFixedBatches();
                    ucoBatches.EnableTransactionTabForBatch();
                }

                ucoBatches.SetInitialFocus();
                FPreviousTab = eGLTabs.Batches;
            }
            else if (ATab == eGLTabs.Journals)
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

                    this.tabGLBatch.SelectedTab = this.tpgTransactions;

                    this.ucoTransactions.LoadTransactions(
                        FLedgerNumber,
                        ucoJournals.GetSelectedDetailRow().BatchNumber,
                        ucoJournals.GetSelectedDetailRow().JournalNumber,
                        ucoJournals.GetSelectedDetailRow().TransactionCurrency,
                        ucoBatches.GetSelectedDetailRow().BatchStatus,
                        ucoJournals.GetSelectedDetailRow().JournalStatus,
                        fromBatchTab);

                    FPreviousTab = eGLTabs.Transactions;
                }
            }

            this.Refresh();
        }

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(true, true))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
        }

        private void SelectTabManual(int ASelectedTabIndex)
        {
            if (ASelectedTabIndex == (int)eGLTabs.Batches)
            {
                SelectTab(eGLTabs.Batches);
            }
            else if (ASelectedTabIndex == (int)eGLTabs.Journals)
            {
                SelectTab(eGLTabs.Journals);
            }
            else //(ASelectedTabIndex == (int)eGLTabs.Transactions)
            {
                SelectTab(eGLTabs.Transactions);
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
    }
}