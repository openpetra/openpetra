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

            //Need this to allow focus to go to the grid.
            tabRecurringGLBatch.TabStop = false;

            tabRecurringGLBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null); //tabRecurringGLBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            this.ucoRecurringBatches.FocusGrid();
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
        /// activate the journal tab and load the journals of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadJournals(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            this.tpgJournals.Enabled = true;
            DisableTransactions();
            this.ucoRecurringJournals.LoadJournals(ALedgerNumber, ABatchNumber);
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
            this.ucoRecurringTransactions.LoadTransactions(ALedgerNumber, ABatchNumber, AJournalNumber, AForeignCurrencyName);
            this.Refresh();
        }

        /// <summary>
        /// Unload transactions from the form
        /// </summary>
        public void UnloadTransactions()
        {
            this.ucoRecurringTransactions.UnloadTransactions();
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
        public void EnableTransactions()
        {
            this.tpgTransactions.Enabled = true;
            this.Refresh();
        }

        /// <summary>
        /// disable the journal tab if we have no active batch
        /// </summary>
        public void DisableJournals()
        {
            this.tpgJournals.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals()
        {
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
        //private eGLTabs FPreviousTab = eGLTabs.RecurringBatches;
        
        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            if (ATab == eGLTabs.RecurringBatches)
            {
                this.tabRecurringGLBatch.SelectedTab = this.tpgBatches;
                this.tpgJournals.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);

                if (this.tpgTransactions.Enabled)
                {
                    this.ucoRecurringTransactions.CancelChangesToFixedBatches();
                    this.ucoRecurringJournals.CancelChangesToFixedBatches();
                    this.tpgTransactions.Enabled = false;
                }

                this.ucoRecurringBatches.FocusGrid();
                //FPreviousTab = eGLTabs.RecurringBatches;
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
                    	(ucoRecurringJournals.GetSelectedDetailRow() != null);

                    this.ucoRecurringJournals.UpdateHeaderTotals(ucoRecurringBatches.GetSelectedDetailRow());

                    this.ucoRecurringJournals.FocusGrid();
                    //FPreviousTab = eGLTabs.RecurringJournals;
                }
            }
            else if (ATab == eGLTabs.RecurringTransactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgTransactions;

                    this.ucoRecurringTransactions.LoadTransactions(
                        FLedgerNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchStatus,
                        ucoRecurringJournals.GetSelectedDetailRow().JournalStatus);

                    //FPreviousTab = eGLTabs.RecurringTransactions;
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
    }
}