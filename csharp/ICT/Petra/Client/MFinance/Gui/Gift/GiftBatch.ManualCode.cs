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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftBatch
    {
        private Int32 FLedgerNumber;
        private Boolean FViewMode = false;

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
        private GiftBatchTDS FViewModeTDS;
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
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                ucoBatches.LoadBatches(FLedgerNumber);
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
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void RefreshAll()
        {
            ucoBatches.RefreshAll();
        }

        private void InitializeManualCode()
        {
            tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
            this.tpgTransactions.Enabled = false;
        }

        private int standardTabIndex = 0;

        private void TFrmGiftBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGiftBatch.SelectedIndex = standardTabIndex;
            TabSelectionChanged(null, null); //tabGiftBatch.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);
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
        /// <param name="AFromTabClick">Indicates if called from a click on a tab or from grid doubleclick</param>
        public void LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            bool AFromTabClick = true)
        {
            this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber, ABatchStatus, AFromTabClick);
//            try
//            {
//                //this.tpgTransactions.Enabled = true;
//                FPetraUtilsObject.DisableDataChangedEvent();
//                this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber, ABatchStatus, AFromTabClick);
//            }
//            finally
//            {
//                FPetraUtilsObject.EnableDataChangedEvent();
//            }
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
        public void EnableTransactions()
        {
            this.tpgTransactions.Enabled = true;
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void DisableTransactions()
        {
            this.tpgTransactions.Enabled = false;
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
            if (ucoTransactions == null)
            {
            	return null;
            }
            else
            {
            	return ucoTransactions;
            }
        	
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

            if (!SaveChanges())
            {
                e.Cancel = true;

                FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
            }
        }

        bool FChangeTabEventHasRun = false;

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
        /// <param name="AFromTabClick"></param>
        public void SelectTab(eGiftTabs ATab, bool AFromTabClick = true)
        {
            if (FChangeTabEventHasRun && AFromTabClick)
            {
                FChangeTabEventHasRun = false;
                return;
            }
            else
            {
                FChangeTabEventHasRun = !AFromTabClick;
            }

            if (ATab == eGiftTabs.Batches)
            {
                //If from grid double click then invoke tab changed event
                if (!AFromTabClick)
                {
                    this.tabGiftBatch.SelectedTab = this.tpgBatches;
                }

                this.tpgTransactions.Enabled = (ucoBatches.GetSelectedDetailRow() != null);
            }
            else if (ATab == eGiftTabs.Transactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    AGiftBatchRow SelectedRow = ucoBatches.GetSelectedDetailRow();

                    //
                    // If there's only one GiftBatch row, I'll not require that the user has selected it!

                    if (FMainDS.AGiftBatch.Rows.Count == 1)
                    {
                        SelectedRow = FMainDS.AGiftBatch[0];
                    }

                    if (SelectedRow != null)
                    {
                        LoadTransactions(SelectedRow.LedgerNumber,
                            SelectedRow.BatchNumber,
                            SelectedRow.BatchStatus, AFromTabClick);
                    }

                    //If from grid double click then invoke tab changed event
                    if (!AFromTabClick)
                    {
                        this.tabGiftBatch.SelectedTab = this.tpgTransactions;
                    }
                }
            }
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
    }
}