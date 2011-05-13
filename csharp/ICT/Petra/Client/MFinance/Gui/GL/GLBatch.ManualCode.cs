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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmGLBatch
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
                ucoAttributes.LedgerNumber = value;

                ucoBatches.FMainDS_ALedgerIsValidNow();
                ucoTransactions.FMainDS_ALedgerIsValidNow();
                ucoJournals.FMainDS_ALedgerIsValidNow();

                ucoJournals.WorkAroundInitialization();
                ucoTransactions.WorkAroundInitialization();
            }
        }

        private void InitializeManualCode()
        {
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
            this.ucoJournals.LoadJournals(ALedgerNumber, ABatchNumber);
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
            this.ucoTransactions.LoadTransactions(ALedgerNumber, ABatchNumber, AJournalNumber, AForeignCurrencyName);
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
            this.ucoAttributes.LoadAttributes(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber);
        }

        /// <summary>
        /// disable the transactions tab if we have no active journal
        /// </summary>
        public void DisableTransactions()
        {
            this.tpgTransactions.Enabled = false;
        }

        /// <summary>
        /// disable the attributes tab if we have no active transactions
        /// </summary>
        public void DisableAttributes()
        {
            this.tpgAttributes.Enabled = false;
        }

        /// <summary>
        /// disable the journal tab if we have no active batch
        /// </summary>
        public void DisableJournals()
        {
            this.tpgJournals.Enabled = false;
        }

        /// this window contains 4 tabs
        public enum eGLTabs
        {
            /// list of batches
            Batches,

            /// list of journals
            Journals,

            /// list of transactions
            Transactions,

            /// list of attributes
            Attributes
        };

        /// <summary>
        /// Switch to the given tab
        /// </summary>
        /// <param name="ATab"></param>
        public void SelectTab(eGLTabs ATab)
        {
            if (ATab == eGLTabs.Batches)
            {
                this.tabGLBatch.SelectedTab = this.tpgBatches;
            }
            else if (ATab == eGLTabs.Journals)
            {
                if (this.tpgJournals.Enabled)
                {
                    this.tabGLBatch.SelectedTab = this.tpgJournals;
                }
            }
            else if (ATab == eGLTabs.Transactions)
            {
                if (this.tpgTransactions.Enabled)
                {
                    this.tabGLBatch.SelectedTab = this.tpgTransactions;
                }
            }
            else if (ATab == eGLTabs.Attributes)
            {
                if (this.tpgAttributes.Enabled)
                {
                    this.tabGLBatch.SelectedTab = this.tpgAttributes;
                }
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

        /// <summary>
        /// directly access the attributes control
        /// </summary>
        public TUC_GLAttributes GetAttributesControl()
        {
            return ucoAttributes;
        }
    }
}