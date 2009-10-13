/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
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

        private void InitializeManualCode()
        {
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// activate the transactions tab and load the gift transactions of the batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            this.tpgTransactions.Enabled = true;
            this.ucoTransactions.LoadGifts(ALedgerNumber, ABatchNumber);
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
                    this.tabGiftBatch.SelectedTab = this.tpgTransactions;
                }
            }
        }
    }
}