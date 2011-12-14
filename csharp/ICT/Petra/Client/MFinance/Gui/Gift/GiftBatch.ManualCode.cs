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

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Gift.Data;

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
        /// show the actual data of the database after server has changed data
        /// </summary>
        public void RefreshAll()
        {
            ucoBatches.RefreshAll();
        }

        private void InitializeManualCode()
        {
            this.tpgTransactions.Enabled = false;
        }

        private int standardTabIndex = 0;
        private void TFrmGiftBatch_Load(object sender, EventArgs e)
        {
            FPetraUtilsObject.TFrmPetra_Load(sender, e);

            tabGiftBatch.SelectedIndex = standardTabIndex;
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
        /// find a special gift detail
        /// </summary>
        public void FindGiftDetail(AGiftDetailRow gdr)
        {
            ucoBatches.SelectBatchNumber(gdr.BatchNumber);
            ucoTransactions.SelectGiftDetailNumber(gdr.GiftTransactionNumber, gdr.DetailNumber);
            standardTabIndex = 1;     // later we switch to the detail tab
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

        private void ValidateDataManual()
        {
            AGiftBatchRow InspectRow;
            AGiftDetailRow InspectRow2;
            object ValidationContext;

            for (int Counter = 0; Counter < FMainDS.AGiftBatch.Rows.Count; Counter++)
            {
                InspectRow = (AGiftBatchRow)FMainDS.AGiftBatch.Rows[Counter];

                DataColumn ValidationColumn;

                // 'Batch Description' must not be empty
                ValidationColumn = InspectRow.Table.Columns[AGiftBatchTable.ColumnBatchDescriptionId];
				ValidationContext = InspectRow.BatchNumber;
                FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                    TStringChecks.StringMustNotBeEmpty(InspectRow.BatchDescription,
                        "Batch Description for Batch Number " + ValidationContext.ToString(),
                        ValidationContext, ValidationColumn, ucoBatches), ValidationColumn, ValidationContext, true);
                
				// 'Exchange Rate' must be greater than 0
                ValidationColumn = InspectRow.Table.Columns[AGiftBatchTable.ColumnExchangeRateToBaseId];
				ValidationContext = InspectRow.BatchNumber;
                FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                    TNumericalChecks.IsPositiveDecimal(InspectRow.ExchangeRateToBase,
                        "Exchange Rate for Batch Number " + ValidationContext.ToString(),
                        ValidationContext, ValidationColumn, ucoBatches), ValidationColumn, ValidationContext, true);
            }

            for (int Counter = 0; Counter < FMainDS.AGiftDetail.Rows.Count; Counter++)
            {
                InspectRow2 = (AGiftDetailRow)FMainDS.AGiftDetail.Rows[Counter];

                DataColumn ValidationColumn;

                ValidationColumn = InspectRow2.Table.Columns[AGiftDetailTable.ColumnGiftCommentOneId];
				ValidationContext = InspectRow2.BatchNumber.ToString() + ";" + InspectRow2.GiftTransactionNumber.ToString();
                // 'Gift Comment One' must not be empty
                FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                    TStringChecks.StringMustNotBeEmpty(InspectRow2.GiftCommentOne,
                        String.Format("Gift Comment One for Batch Number {0}, Gift Transaction Number {1}",
                	                  InspectRow2.BatchNumber, InspectRow2.GiftTransactionNumber),
                        ValidationContext, ValidationColumn, ucoTransactions), ValidationColumn, ValidationContext, true);
            }
        }
    }
}