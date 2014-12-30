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
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonForms;

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
        private Int32 FStandardTabIndex = 0;

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

            tabRecurringGLBatch.SelectedIndex = FStandardTabIndex;
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
            this.tpgRecurringJournals.Enabled = false;
            this.tpgRecurringTransactions.Enabled = false;
        }

        /// <summary>
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals(bool AEnable = true)
        {
            this.tabRecurringGLBatch.TabStop = AEnable;

            if (this.tpgRecurringJournals.Enabled != AEnable)
            {
                this.tpgRecurringJournals.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the journal tab if we have no active batch
        /// </summary>
        public void DisableJournals()
        {
            this.tabRecurringGLBatch.TabStop = false;

            if (this.tpgRecurringJournals.Enabled)
            {
                this.tpgRecurringJournals.Enabled = false;
                this.Refresh();
            }
        }

        /// <summary>
        /// enable the transactions tab
        /// </summary>
        public void EnableTransactions(bool AEnable = true)
        {
            if (this.tpgRecurringTransactions.Enabled != AEnable)
            {
                this.tpgRecurringTransactions.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the transactions tab
        /// </summary>
        public void DisableTransactions()
        {
            if (this.tpgRecurringTransactions.Enabled)
            {
                this.tpgRecurringTransactions.Enabled = false;
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
                    this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringBatches;
                    this.tpgRecurringJournals.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);
                    this.tabRecurringGLBatch.TabStop = this.tpgRecurringJournals.Enabled;

                    ucoRecurringBatches.AutoEnableTransTabForBatch();
                    ucoRecurringBatches.SetInitialFocus();
                    FPreviousTab = eGLTabs.RecurringBatches;
                }
                else if ((ucoRecurringBatches.GetSelectedDetailRow() != null) && (ATab == eGLTabs.RecurringJournals))
                {
                    if (this.tpgRecurringJournals.Enabled)
                    {
                        this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringJournals;

                        this.ucoRecurringJournals.LoadJournals(FLedgerNumber,
                            ucoRecurringBatches.GetSelectedDetailRow().BatchNumber);

                        this.tpgRecurringTransactions.Enabled = (ucoRecurringJournals.GetSelectedDetailRow() != null);

                        this.ucoRecurringJournals.UpdateHeaderTotals(ucoRecurringBatches.GetSelectedDetailRow());

                        FPreviousTab = eGLTabs.RecurringJournals;
                    }
                }
                else if (ATab == eGLTabs.RecurringTransactions)
                {
                    if (this.tpgRecurringTransactions.Enabled)
                    {
                        // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                        this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringTransactions;

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

                        this.ucoRecurringTransactions.LoadRecurringTransactions(
                            FLedgerNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                            fromBatchTab);

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

        private int GetChangedRecordCountManual(out string AMessage)
        {
            // For GL Batch we will get some mix of batches, journals and transactions
            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int allChangesCount = 0;

            // Work out how many changes in each table
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
                int nBatches = 0;
                int nJournals = 0;
                int nTransactions = 0;

                foreach (Tuple <string, int>TableAndCount in TableAndCountList)
                {
                    if (TableAndCount.Item1.Equals(ARecurringBatchTable.GetTableName()))
                    {
                        nBatches = TableAndCount.Item2;
                    }
                    else if (TableAndCount.Item1.Equals(ARecurringJournalTable.GetTableName()))
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

                AMessage += Environment.NewLine;
                AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
            }

            return allChangesCount;
        }

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        /// Special Handlers for menus and command keys for our user controls

        private void MniFilterFind_Click(object sender, EventArgs e)
        {
            if (tabRecurringGLBatch.SelectedTab == tpgRecurringBatches)
            {
                ucoRecurringBatches.MniFilterFind_Click(sender, e);
            }
            else if (tabRecurringGLBatch.SelectedTab == tpgRecurringJournals)
            {
                ucoRecurringJournals.MniFilterFind_Click(sender, e);
            }
            else if (tabRecurringGLBatch.SelectedTab == tpgRecurringTransactions)
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

            if ((tabRecurringGLBatch.SelectedTab == tpgRecurringBatches) && (ucoRecurringBatches.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabRecurringGLBatch.SelectedTab == tpgRecurringJournals) && (ucoRecurringJournals.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }
            else if ((tabRecurringGLBatch.SelectedTab == tpgRecurringTransactions) && (ucoRecurringTransactions.ProcessParentCmdKey(ref msg, keyData)))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}