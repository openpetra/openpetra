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
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmRecurringGLBatch
    {
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

        private eGLTabs FPreviousTab = eGLTabs.RecurringBatches;
        private Int32 FLedgerNumber = -1;
        private Int32 standardTabIndex = 0;
        private bool FWindowIsMaximized = false;

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

                ucoRecurringJournals.WorkAroundInitialization();
                ucoRecurringTransactions.WorkAroundInitialization();
            }
        }

        private void TFrmRecurringGLBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabRecurringGLBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null);

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
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals(bool AEnable = true)
        {
            this.tabRecurringGLBatch.TabStop = AEnable;

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
            this.tabRecurringGLBatch.TabStop = false;

            if (this.tpgJournals.Enabled)
            {
                this.tpgJournals.Enabled = false;
                this.Refresh();
            }
        }

        /// <summary>
        /// enable the transactions tab
        /// </summary>
        public void EnableTransactions(bool AEnable = true)
        {
            if (this.tpgTransactions.Enabled != AEnable)
            {
                this.tpgTransactions.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the transactions tab
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
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (ATab == eGLTabs.RecurringBatches)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgBatches;
                    this.tpgJournals.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);
                    this.tabRecurringGLBatch.TabStop = this.tpgJournals.Enabled;

                    if (this.tpgTransactions.Enabled)
                    {
                        this.ucoRecurringTransactions.CancelChangesToFixedBatches();
                        this.ucoRecurringJournals.CancelChangesToFixedBatches();
                        ucoRecurringBatches.AutoEnableTransTabForBatch();
                    }

                    ucoRecurringBatches.SetInitialFocus();
                    FPreviousTab = eGLTabs.RecurringBatches;
                }
                else if (ucoRecurringBatches.GetSelectedDetailRow() != null && ATab == eGLTabs.RecurringJournals)
                {
                    if (this.tpgJournals.Enabled)
                    {
                        this.tabRecurringGLBatch.SelectedTab = this.tpgJournals;

                        this.ucoRecurringJournals.LoadJournals(FLedgerNumber,
                            ucoRecurringBatches.GetSelectedDetailRow().BatchNumber);

                        this.tpgTransactions.Enabled = (ucoRecurringJournals.GetSelectedDetailRow() != null);

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
                                ucoRecurringBatches.GetSelectedDetailRow().BatchNumber);
                        }

                        if (this.ucoRecurringTransactions.LoadTransactions(
                                FLedgerNumber,
                                ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                                ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                                ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                                fromBatchTab))
                        {
                            ucoRecurringTransactions.AutoSizeGrid();
                        }

                        FPreviousTab = eGLTabs.RecurringTransactions;
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

        private void SelectTabManual(int ASelectedTabIndex)
        {
            switch (ASelectedTabIndex)
            {
                case (int)eGLTabs.RecurringBatches:
                    SelectTab(eGLTabs.RecurringBatches);
                    break;

                case (int)eGLTabs.RecurringJournals:
                    SelectTab(eGLTabs.RecurringJournals);
                    break;

                default: //(ASelectedTabIndex == (int)eGLTabs.RecurringTransactions)
                    SelectTab(eGLTabs.RecurringTransactions);
                    break;
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

        private void RunOnceOnActivationManual()
        {
            this.Resize += new EventHandler(TFrmGLBatch_Resize);
        }

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


        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        /// Special Handlers for menus and command keys for our user controls

        private void MniFilterFind_Click(object sender, EventArgs e)
        {
            if (tabRecurringGLBatch.SelectedTab == tpgBatches)
            {
                ucoRecurringBatches.MniFilterFind_Click(sender, e);
            }
            else if (tabRecurringGLBatch.SelectedTab == tpgJournals)
            {
                ucoRecurringJournals.MniFilterFind_Click(sender, e);
            }
            else if (tabRecurringGLBatch.SelectedTab == tpgTransactions)
            {
                ucoRecurringTransactions.MniFilterFind_Click(sender, e);
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

            if ((tabRecurringGLBatch.SelectedTab == tpgBatches) && (ucoRecurringBatches.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabRecurringGLBatch.SelectedTab == tpgJournals) && (ucoRecurringJournals.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabRecurringGLBatch.SelectedTab == tpgTransactions) && (ucoRecurringTransactions.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}