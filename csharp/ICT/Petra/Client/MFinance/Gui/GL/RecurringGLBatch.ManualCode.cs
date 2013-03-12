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
                ucoRecurringAttributes.LedgerNumber = value;

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
            this.tpgAttributes.Enabled = false;
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
            DisableAttributes();
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
        /// activate the attributes tab and load the attributes of the transaction
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        public void LoadAttributes(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber)
        {
            this.tpgAttributes.Enabled = true;
            this.ucoRecurringAttributes.LoadAttributes(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber);
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
        /// Unload transactions from the form
        /// </summary>
        public void UnloadJournals()
        {
            this.ucoRecurringJournals.UnloadJournals();
        }

        /// <summary>
        /// Enable the attributes tab if we have active transactions
        /// </summary>
        public void EnableAttributes()
        {
            this.tpgAttributes.Enabled = true;
            this.Refresh();
        }

        /// <summary>
        /// disable the attributes tab if we have no active transactions
        /// </summary>
        public void DisableAttributes()
        {
            this.tpgAttributes.Enabled = false;
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

            /// list of attributes
            RecurringAttributes
        };

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
                    //TODO SaveChanges();
                    this.tpgTransactions.Enabled = false;
                }

                this.tpgAttributes.Enabled = false;

                this.ucoRecurringBatches.FocusGrid();
            }
            else if (ATab == eGLTabs.RecurringJournals)
            {
                if (this.tpgJournals.Enabled)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgJournals;

                    this.ucoRecurringJournals.LoadJournals(FLedgerNumber,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchNumber,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchStatus);

                    this.tpgTransactions.Enabled = (ucoRecurringJournals.GetSelectedDetailRow() != null);
                    this.tpgAttributes.Enabled = false;

                    this.ucoRecurringJournals.FocusGrid();
                }
            }
            else if (ATab == eGLTabs.RecurringTransactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgTransactions;
                    this.tpgAttributes.Enabled = true;

                    this.ucoRecurringTransactions.LoadTransactions(
                        FLedgerNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                        ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                        ucoRecurringBatches.GetSelectedDetailRow().BatchStatus,
                        ucoRecurringJournals.GetSelectedDetailRow().JournalStatus);

                    this.tpgAttributes.Enabled = ((ucoRecurringTransactions.GetSelectedDetailRow() != null)
                                                  && TRemote.MFinance.Setup.WebConnectors.HasAccountSetupAnalysisAttributes(FLedgerNumber,
                                                      ucoRecurringTransactions.GetSelectedDetailRow().AccountCode));
                }
            }
            else if (ATab == eGLTabs.RecurringAttributes)
            {
                if (this.tpgAttributes.Enabled)
                {
                    this.tabRecurringGLBatch.SelectedTab = this.tpgAttributes;

                    this.ucoRecurringAttributes.LoadAttributes(
                        FLedgerNumber,
                        ucoRecurringTransactions.GetSelectedDetailRow().BatchNumber,
                        ucoRecurringTransactions.GetSelectedDetailRow().JournalNumber,
                        ucoRecurringTransactions.GetSelectedDetailRow().TransactionNumber
                        );
                }
            }

            this.Refresh();
        }

        void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            //TODO FPetraUtilsObject.VerificationResultCollection.Clear();

            //TODO if (!SaveChanges())
            //TODO {
            //TODO     e.Cancel = true;
            //TODO
            //TODO     FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            //TODO }
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
            else if (ASelectedTabIndex == (int)eGLTabs.RecurringTransactions)
            {
                SelectTab(eGLTabs.RecurringTransactions);
            }
            else  //eGLTabs.RecurringAttributes
            {
                SelectTab(eGLTabs.RecurringAttributes);
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
        /// directly access the attributes control
        /// </summary>
        public TUC_RecurringGLAttributes GetAttributesControl()
        {
            return ucoRecurringAttributes;
        }
    }
}