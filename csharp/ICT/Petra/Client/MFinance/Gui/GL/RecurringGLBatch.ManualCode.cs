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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmRecurringGLBatch
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

                ucoRecurringBatches.LoadBatches(FLedgerNumber);

                ucoRecurringTransactions.WorkAroundInitialization();
            }
        }

        private int standardTabIndex = 0;

        private void TFrmRecurringGLBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabRecurringGLBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null); //tabRecurringGLBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            this.Shown += delegate
            {
                // This will ensure the grid gets the focus when the screen is shown for the first time
                ucoRecurringBatches.SetInitialFocus();
            };
        }

        private void InitializeManualCode()
        {
            tabRecurringGLBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            this.tpgJournals.Enabled = false;
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// Load the journals for the current batch in the background
        /// </summary>
        public void LoadJournals()
        {
            int batchNumber = ucoRecurringBatches.GetSelectedDetailRow().BatchNumber;

            FMainDS.ARecurringJournal.DefaultView.RowFilter = string.Format("{0} = {1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                batchNumber);

            // only load from server if there are no journals loaded yet for this batch
            // otherwise we would overwrite journals that have already been modified
            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(FLedgerNumber, batchNumber));
            }
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
            this.tabRecurringGLBatch.TabStop = false;

            this.tpgJournals.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals()
        {
            this.tabRecurringGLBatch.TabStop = true;

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
            RecurringBatches,

            /// list of journals
            RecurringJournals,

            /// list of transactions
            RecurringTransactions,
        };

        //Might need this later
        private eGLTabs FPreviousTab = eGLTabs.RecurringBatches;

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            this.Cursor = Cursors.WaitCursor;

            if (ATab == eGLTabs.RecurringBatches)
            {
                this.tabRecurringGLBatch.SelectedTab = this.tpgBatches;
                this.tpgJournals.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);

                if (this.tpgTransactions.Enabled)
                {
                    this.ucoRecurringTransactions.CancelChangesToFixedBatches();
                    this.ucoRecurringJournals.CancelChangesToFixedBatches();
                    ucoRecurringBatches.EnableTransactionTabForBatch();
                }

                ucoRecurringBatches.SetInitialFocus();
                FPreviousTab = eGLTabs.RecurringBatches;
            }
            else if (ATab == eGLTabs.RecurringJournals)
            {
                if (this.tpgJournals.Enabled)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgJournals;

                    this.ucoRecurringJournals.LoadJournals(FLedgerNumber,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchNumber,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchStatus);

                    this.tpgTransactions.Enabled =
                        (ucoRecurringJournals.GetSelectedDetailRow() != null && ucoRecurringJournals.GetSelectedDetailRow().JournalStatus !=
                         MFinanceConstants.BATCH_CANCELLED);

                    this.ucoRecurringJournals.UpdateHeaderTotals(ucoRecurringBatches.GetSelectedDetailRow());

                    FPreviousTab = eGLTabs.RecurringJournals;
                }
            }
            else if (ATab == eGLTabs.RecurringTransactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                    // But we need this to be set before calling ucoTransactions.AutoSizeGrid() because that only works once the page is actually loaded.
                    this.tabRecurringGLBatch.SelectedTab = this.tpgTransactions;

                    bool fromBatchTab = false;

                    if (FPreviousTab == eGLTabs.RecurringBatches)
                    {
                        fromBatchTab = true;
                        //This only happens when the user clicks from Batch to Transactions,
                        //  which is only allowed when one journal exists

                        //Need to make sure that the Journal is loaded
                        this.ucoRecurringJournals.LoadJournals(FLedgerNumber,
                            ucoRecurringBatches.GetSelectedDetailRow().BatchNumber,
                            ucoRecurringBatches.GetSelectedDetailRow().BatchStatus);
                    }

                    if (this.ucoRecurringTransactions.LoadTransactions(
                            FLedgerNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                            ucoRecurringBatches.GetSelectedDetailRow().BatchStatus,
                            ucoRecurringJournals.GetSelectedDetailRow().JournalStatus,
                            fromBatchTab))
                    {
                        ucoRecurringTransactions.AutoSizeGrid();
                    }

                    FPreviousTab = eGLTabs.RecurringTransactions;
                }
            }

            this.Cursor = Cursors.Default;
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
            if (ASelectedTabIndex == (int)eGLTabs.RecurringBatches)
            {
                SelectTab(eGLTabs.RecurringBatches);
            }
            else if (ASelectedTabIndex == (int)eGLTabs.RecurringJournals)
            {
                SelectTab(eGLTabs.RecurringJournals);
            }
            else //(ASelectedTabIndex == (int)eGLTabs.RecurringTransactions)
            {
                SelectTab(eGLTabs.RecurringTransactions);
            }
        }

        /// <summary>
        /// directly access the batches control
        /// </summary>
        public TUC_RecurringGLBatches GetBatchControl()
        {
            return ucoRecurringBatches;
        }

        /// <summary>
        /// directly access the journals control
        /// </summary>
        public TUC_RecurringGLJournals GetJournalsControl()
        {
            return ucoRecurringJournals;
        }

        /// <summary>
        /// directly access the transactions control
        /// </summary>
        public TUC_RecurringGLTransactions GetTransactionsControl()
        {
            return ucoRecurringTransactions;
        }

        /// <summary>
        /// Shows the Filter/Find UserControl and switches to the Find Tab.
        /// </summary>
        /// <param name="sender">Not evaluated.</param>
        /// <param name="e">Not evaluated.</param>
        public void mniFilterFind_Click(object sender, System.EventArgs e)
        {
            switch (tabRecurringGLBatch.SelectedIndex)
            {
                case (int)eGLTabs.RecurringBatches:
                    ucoRecurringBatches.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGLTabs.RecurringJournals:
                    ucoRecurringJournals.MniFilterFind_Click(sender, e);
                    break;

                case (int)eGLTabs.RecurringTransactions:
                    ucoRecurringTransactions.MniFilterFind_Click(sender, e);
                    break;
            }
        }

        private void RunOnceOnActivationManual()
        {
            this.Resize += new EventHandler(TFrmGLBatch_Resize);
        }

        private bool FWindowIsMaximized = false;
        void TFrmGLBatch_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                // set the flag that we are maximized
                FWindowIsMaximized = true;

                if (tabRecurringGLBatch.SelectedTab == this.tpgBatches)
                {
                    ucoRecurringTransactions.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing transactions");
                }
                else if (tabRecurringGLBatch.SelectedTab == this.tpgTransactions)
                {
                    ucoRecurringBatches.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing batches");
                }
                else
                {
                    ucoRecurringBatches.AutoSizeGrid();
                    ucoRecurringTransactions.AutoSizeGrid();
                }
            }
            else if (FWindowIsMaximized && (this.WindowState == FormWindowState.Normal))
            {
                // we have been maximized but now are normal.  In this case we need to re-autosize the cells because otherwise they are still 'stretched'.
                ucoRecurringBatches.AutoSizeGrid();
                ucoRecurringTransactions.AutoSizeGrid();
                FWindowIsMaximized = false;
                Console.WriteLine("Normal - autosizing both");
            }
        }
    }
}