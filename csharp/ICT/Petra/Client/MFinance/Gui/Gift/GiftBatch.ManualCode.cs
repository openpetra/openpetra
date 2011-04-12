//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftBatch
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
                ucoBatches.LoadBatches(FLedgerNumber);
            }
        }
        /// <summary>
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void refresh()
        {
            ucoBatches.refresh();
        }

        private void InitializeManualCode()
        {
            this.tpgTransactions.Enabled = false;

            if (FTabPageEvent == null)
            {
                FTabPageEvent += this.TabPageEventHandler;
            }
        }

        private void TFrmGiftBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGiftBatch.SelectedIndex = 0;
            TabSelectionChanged(null, null);
        }

        /// <summary>
        /// activate the transactions tab and load the gift transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            this.tpgTransactions.Enabled = true;
            FPetraUtilsObject.DisableDataChangedEvent();
            this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber);
            FPetraUtilsObject.EnableDataChangedEvent();
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
        public void EnableTransactionsTab()
        {
            this.tpgTransactions.Enabled = true;
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
            return ucoTransactions;
        }

        /// this window contains 2 tabs
        public enum eGiftTabs
        {
            /// list of batches
            Batches,

            /// list of transactions
            Transactions
        };

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGiftTabs ATab)
        {
            if (ATab == eGiftTabs.Batches)
            {
                this.tabGiftBatch.SelectedTab = this.tpgBatches;
            }
            else if (ATab == eGiftTabs.Transactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    LoadTransactions(ucoBatches.GetSelectedDetailRow().LedgerNumber,
                        ucoBatches.GetSelectedDetailRow().BatchNumber);
                    this.tabGiftBatch.SelectedTab = this.tpgTransactions;
                }
            }
        }

        private void TabPageEventHandler(object sender, TTabPageEventArgs ATabPageEventArgs)
        {
            if (ATabPageEventArgs.Event == "InitialActivation")
            {
                if (ATabPageEventArgs.Tab == tpgBatches)
                {
                    ucoBatches.LoadBatches(FLedgerNumber);
                }
            }

            if (ATabPageEventArgs.Tab == tpgTransactions)
            {
                SelectTab(eGiftTabs.Transactions);
            }
        }
    }
}