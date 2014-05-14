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
        private Int32 FLedgerNumber = -1;
        private Int32 standardTabIndex = 0;
        private eGLTabs FPreviouslySelectedTab = eGLTabs.Batches;
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

                ucoBatches.LoadBatches(FLedgerNumber);

                ucoJournals.WorkAroundInitialization();
                ucoTransactions.WorkAroundInitialization();
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

        private void TFrmGLBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGLBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null);

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
        /// Enable the journal tab if we have an active batch
        /// </summary>
        public void EnableJournals(Boolean AEnable = true)
        {
            this.tabGLBatch.TabStop = AEnable;

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
            this.tabGLBatch.TabStop = false;

            if (this.tpgJournals.Enabled)
            {
                this.tpgJournals.Enabled = false;
                this.Refresh();
            }
        }

        /// <summary>
        /// Control transactions tab enabled status. This can do both ways
        /// </summary>
        public void EnableTransactions(Boolean AEnable = true)
        {
            if (this.tpgTransactions.Enabled != AEnable)
            {
                this.tpgTransactions.Enabled = AEnable;
                this.Refresh();
            }
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
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
            this.Cursor = Cursors.WaitCursor;

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
                FPreviouslySelectedTab = eGLTabs.Batches;
            }
            else if ((ucoBatches.GetSelectedDetailRow() != null) && (ATab == eGLTabs.Journals))
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

                    FPreviouslySelectedTab = eGLTabs.Journals;
                }
            }
            else if (ATab == eGLTabs.Transactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    // Note!! This call may result in this (SelectTab) method being called again (but no new transactions will be loaded the second time)
                    // But we need this to be set before calling ucoTransactions.AutoSizeGrid() because that only works once the page is actually loaded.
                    this.tabGLBatch.SelectedTab = this.tpgTransactions;

                    bool fromBatchTab = false;

                    if (FPreviouslySelectedTab == eGLTabs.Batches)
                    {
                        fromBatchTab = true;
                        //This only happens when the user clicks from Batch to Transactions,
                        //  which is only allowed when one journal exists

                        //Need to make sure that the Journal is loaded
                        this.ucoJournals.LoadJournals(FLedgerNumber,
                            ucoBatches.GetSelectedDetailRow().BatchNumber,
                            ucoBatches.GetSelectedDetailRow().BatchStatus);
                    }

                    if (this.ucoTransactions.LoadTransactions(
                            FLedgerNumber,
                            ucoJournals.GetSelectedDetailRow().BatchNumber,
                            ucoJournals.GetSelectedDetailRow().JournalNumber,
                            ucoJournals.GetSelectedDetailRow().TransactionCurrency,
                            ucoBatches.GetSelectedDetailRow().BatchStatus,
                            ucoJournals.GetSelectedDetailRow().JournalStatus,
                            fromBatchTab))
                    {
                        ucoTransactions.AutoSizeGrid();
                    }

                    FPreviouslySelectedTab = eGLTabs.Transactions;
                }
            }

            this.Cursor = Cursors.Default;
            this.Refresh();
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

        private void SelectTabManual(Int32 ASelectedTabIndex)
        {
            switch (ASelectedTabIndex)
            {
                case (int)eGLTabs.Batches:
                    SelectTab(eGLTabs.Batches);
                    break;
                case (int)eGLTabs.Journals:
                    SelectTab(eGLTabs.Journals);
                    break;
                default: //(ASelectedTabIndex == (int)eGLTabs.Transactions)
                    SelectTab(eGLTabs.Transactions);
                    break;
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

                if (tabGLBatch.SelectedTab == this.tpgBatches)
                {
                    ucoTransactions.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing transactions");
                }
                else if (tabGLBatch.SelectedTab == this.tpgTransactions)
                {
                    ucoBatches.AutoSizeGrid();
                    Console.WriteLine("Maximised - autosizing batches");
                }
                else
                {
                    ucoBatches.AutoSizeGrid();
                    ucoTransactions.AutoSizeGrid();
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

        /// <summary>
        /// Stores the exchange rate to convert base currency into international currency
        /// </summary>
        public Decimal BaseToIntlExchangeRate(DateTime AEffectiveDate)
        {
            if ((FLedgerNumber == -1) || (FMainDS.ALedger == null) || (FMainDS.ALedger.Count == 0))
            {
                return 0;
            }

            decimal IntlRateToBaseCurrency = 0;
            DateTime StartOfMonthDate = new DateTime(AEffectiveDate.Year, AEffectiveDate.Month, 1);

            try
            {
                // read the exchange rate for international currency calculations
                IntlRateToBaseCurrency = TRemote.MFinance.GL.WebConnectors.GetCorporateExchangeRate(FMainDS.ALedger[0].BaseCurrency,
                    FMainDS.ALedger[0].IntlCurrency,
                    StartOfMonthDate,
                    AEffectiveDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error trying to retrieve International Exchange Rate for Ledger {0} and date {1}. Message: {2}",
                        FLedgerNumber,
                        StartOfMonthDate,
                        ex.Message));
            }

            return IntlRateToBaseCurrency;
        }

    }
}