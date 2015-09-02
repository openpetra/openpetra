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
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmRecurringGLBatch
    {
        /// this window contains 3 tabs
        public enum eGLTabs
        {
            /// list of batches
            RecurringBatches,

            /// list of journals
            RecurringJournals,

            /// list of transactions
            RecurringTransactions,

            /// None
            None
        };

        /// GL contains 3 levels
        public enum eGLLevel
        {
            /// batch level
            Batch,

            /// journal level
            Journal,

            /// transaction level
            Transaction,

            /// analysis attribute level
            Analysis
        };

        private eGLTabs FPreviouslySelectedTab = eGLTabs.None;
        private Int32 FLedgerNumber = -1;
        private Int32 FStandardTabIndex = 0;
        private bool FChangesDetected = false;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

                // Setting the ledger number of the batch control will automatically load the current financial year batches
                ucoRecurringBatches.LedgerNumber = value;

                //ucoRecurringBatches.LoadBatches(FLedgerNumber);

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
            tabRecurringGLBatch.GotFocus += new EventHandler(tabRecurringGLBatch_GotFocus);
            this.tpgRecurringJournals.Enabled = false;
            this.tpgRecurringTransactions.Enabled = false;
        }

        void tabRecurringGLBatch_GotFocus(object sender, EventArgs e)
        {
            FPetraUtilsObject.WriteToStatusBar(Catalog.GetString(
                    "Use the left or right arrow keys to switch between Batches, Journals and Transactions"));
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

        private void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (!ValidateAllData(false, TErrorProcessingMode.Epm_All))
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }

            //Before the TabSelectionChanged event occurs, changes are incorrectly detected on Journal controls
            // TODO: find cause but use this field for now
            FChangesDetected = FPetraUtilsObject.HasChanges;
        }

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        /// <param name="AAllowRepeatEvent"></param>
        public void SelectTab(eGLTabs ATab, bool AAllowRepeatEvent = false)
        {
            //Between the tab changing and seleted events changes are incorrectly detected on Journal controls
            // TODO: find cause but use this field for now
            if (!FChangesDetected && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
            else if (FChangesDetected && !FPetraUtilsObject.HasChanges)
            {
                FChangesDetected = false;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (ATab == eGLTabs.RecurringBatches)
                {
                    if ((FPreviouslySelectedTab == eGLTabs.RecurringBatches) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    FPreviouslySelectedTab = eGLTabs.RecurringBatches;

                    this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringBatches;
                    this.tpgRecurringJournals.Enabled = (ucoRecurringBatches.GetSelectedDetailRow() != null);
                    this.tabRecurringGLBatch.TabStop = this.tpgRecurringJournals.Enabled;

                    ucoRecurringBatches.AutoEnableTransTabForBatch();
                    ucoRecurringBatches.SetInitialFocus();
                }
                else if (ATab == eGLTabs.RecurringJournals)
                {
                    if ((FPreviouslySelectedTab == eGLTabs.RecurringJournals) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    if (this.tpgRecurringJournals.Enabled)
                    {
                        FPreviouslySelectedTab = eGLTabs.RecurringJournals;

                        this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringJournals;

                        LoadJournals(ucoRecurringBatches.GetSelectedDetailRow().BatchNumber);

                        this.tpgRecurringTransactions.Enabled = (ucoRecurringJournals.GetSelectedDetailRow() != null);

                        this.ucoRecurringJournals.UpdateHeaderTotals(ucoRecurringBatches.GetSelectedDetailRow());
                    }
                }
                else if (ATab == eGLTabs.RecurringTransactions)
                {
                    if ((FPreviouslySelectedTab == eGLTabs.RecurringTransactions) && !AAllowRepeatEvent)
                    {
                        //Repeat event
                        return;
                    }

                    if (this.tpgRecurringTransactions.Enabled)
                    {
                        bool batchWasPreviousTab = (FPreviouslySelectedTab == eGLTabs.RecurringBatches);
                        FPreviouslySelectedTab = eGLTabs.RecurringTransactions;

                        // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                        this.tabRecurringGLBatch.SelectedTab = this.tpgRecurringTransactions;

                        if (batchWasPreviousTab)
                        {
                            //This only happens when the user clicks from Batch to Transactions,
                            //  which is only allowed when one journal exists

                            //Need to make sure that the Journal is loaded
                            LoadJournals(ucoRecurringBatches.GetSelectedDetailRow().BatchNumber);
                        }

                        LoadTransactions(ucoRecurringJournals.GetSelectedDetailRow().BatchNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().JournalNumber,
                            ucoRecurringJournals.GetSelectedDetailRow().TransactionCurrency,
                            batchWasPreviousTab);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Refresh();
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
        /// Load Journals for current Batch
        /// </summary>
        /// <param name="ABatchNumber"></param>
        public void LoadJournals(Int32 ABatchNumber)
        {
            this.ucoRecurringJournals.LoadJournals(FLedgerNumber, ABatchNumber);
        }

        /// <summary>
        /// Load Transactions for current Batch and journal
        /// </summary>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionCurrency"></param>
        /// <param name="AFromBatchTab"></param>
        public void LoadTransactions(Int32 ABatchNumber,
            Int32 AJournalNumber,
            String ATransactionCurrency,
            bool AFromBatchTab)
        {
            try
            {
                this.ucoRecurringTransactions.LoadTransactions(FLedgerNumber,
                    ABatchNumber,
                    AJournalNumber,
                    ATransactionCurrency,
                    AFromBatchTab);
            }
            catch (Exception ex)
            {
                string methodName = Utilities.GetMethodName(true);

                string errorMsg = String.Format(Catalog.GetString("Unexpected error in {0}!{1}{1}Try closing this form and restarting OpenPetra."),
                    methodName,
                    Environment.NewLine);

                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}{1}{1} - {3}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message,
                        ex.InnerException.Message));

                MessageBox.Show(errorMsg, methodName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Only check relevant tables.
            List <string>TablesToCheck = new List <string>();
            TablesToCheck.Add(FMainDS.ARecurringBatch.TableName);
            TablesToCheck.Add(FMainDS.ARecurringJournal.TableName);
            TablesToCheck.Add(FMainDS.ARecurringTransaction.TableName);
            TablesToCheck.Add(FMainDS.ARecurringTransAnalAttrib.TableName);

            List <Tuple <string, int>>TableAndCountList = new List <Tuple <string, int>>();
            int AllChangesCount = 0;

            if (FMainDS.HasChanges())
            {
                // Work out how many changes in each table
                foreach (DataTable dt in FMainDS.GetChanges().Tables)
                {
                    string currentTableName = dt.TableName;

                    if ((dt != null)
                        && TablesToCheck.Contains(currentTableName)
                        && (dt.Rows.Count > 0))
                    {
                        int tableChangesCount = 0;

                        DataTable dtChanges = dt.GetChanges();

                        foreach (DataRow dr in dtChanges.Rows)
                        {
                            if (DataUtilities.DataRowColumnsHaveChanged(dr))
                            {
                                tableChangesCount++;
                                AllChangesCount++;
                            }
                        }

                        if (tableChangesCount > 0)
                        {
                            TableAndCountList.Add(new Tuple <string, int>(currentTableName, tableChangesCount));
                        }
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
                    strBatches = String.Format("{0} recurring {1}",
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

                AMessage += Environment.NewLine + Catalog.GetString("(some of the changes may include related background items)");
                AMessage += Environment.NewLine;
                AMessage += String.Format(TFrmPetraEditUtils.StrConsequenceIfNotSaved, Environment.NewLine);
            }

            return AllChangesCount;
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