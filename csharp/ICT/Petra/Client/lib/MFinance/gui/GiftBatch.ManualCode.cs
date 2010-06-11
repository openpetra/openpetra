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
            }
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
            // Switch to Tab. This ensures that FUcoTransations is existant (it gets dynamically loaded)
            tabGiftBatch.SelectedIndex = tpgTransactions.TabIndex;

            this.FUcoTransactions.LoadGifts(ALedgerNumber, ABatchNumber);
        }

        /// <summary>
        /// this should be called when all data is reloaded after posting
        /// </summary>
        public void ClearCurrentSelections()
        {
            if (this.FUcoBatches != null)
            {
                this.FUcoBatches.ClearCurrentSelection();
            }

            if (this.FUcoTransactions != null)
            {
                this.FUcoTransactions.ClearCurrentSelection();
            }
        }

        public void EnableTransactionsTab()
        {
            this.tpgTransactions.Enabled = true;
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
                    LoadTransactions(FUcoBatches.PreviouslySelectedDetailRow.LedgerNumber,
                        FUcoBatches.PreviouslySelectedDetailRow.BatchNumber);

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
                    FUcoBatches.LoadBatches(FLedgerNumber);
                }
            }

            if (ATabPageEventArgs.Tab == tpgTransactions)
            {
                SelectTab(eGiftTabs.Transactions);
            }
        }
    }
}