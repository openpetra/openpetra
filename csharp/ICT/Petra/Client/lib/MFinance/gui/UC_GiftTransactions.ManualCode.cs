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
using System.Data;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public void LoadGifts(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            if ((FLedgerNumber != -1) && (FBatchNumber != -1))
            {
                GetDataFromControls();
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;

            FPreviouslySelectedDetailRow = null;

            DataView view = new DataView(FMainDS.AGiftDetail);

            // only load from server if there are no transactions loaded yet for this batch
            // otherwise we would overwrite transactions that have already been modified
            view.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + "=" + FBatchNumber.ToString();

            if (view.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ALedgerNumber, ABatchNumber));
            }

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, ActiveOnly);
            TFinanceControls.InitialiseMotivationDetailList(ref cmbDetailMotivationDetailCode, FLedgerNumber, ActiveOnly);

//TODO            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);
//TODO            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);

            ShowData();
        }

        private void FilterMotivationDetail(object sender, EventArgs e)
        {
            TFinanceControls.ChangeFilterMotivationDetailList(ref cmbDetailMotivationDetailCode, cmbDetailMotivationGroupCode.GetSelectedString());
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, cmbDetailMotivationGroupCode.GetSelectedString(), cmbDetailMotivationDetailCode.GetSelectedString() });

            if (motivationDetail != null)
            {
                txtDetailAccountCode.Text = motivationDetail.AccountCode;

                // TODO: calculation of cost centre also depends on the recipient partner key; can be a field key or ministry key, or determined by pm_staff_data: foreign cost centre
                txtDetailCostCentreCode.Text = motivationDetail.CostCentreCode;
            }
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// get the details of the current batch
        /// </summary>
        /// <returns></returns>
        private AGiftBatchRow GetBatchRow()
        {
            return (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
        }

        /// <summary>
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private AGiftRow GetGiftRow(Int32 AGiftTransactionNumber)
        {
            return (AGiftRow)FMainDS.AGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, AGiftTransactionNumber });
        }

        /// <summary>
        /// add a new transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewAGiftDetail();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the batch.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow"></param>
        private void NewRowManual(ref AGiftDetailRow ANewRow)
        {
            AGiftBatchRow batchRow = GetBatchRow();

            // TODO: deal properly with gift details and split gifts etc
            AGiftRow giftRow = FMainDS.AGift.NewRowTyped(true);

            giftRow.LedgerNumber = batchRow.LedgerNumber;
            giftRow.BatchNumber = batchRow.BatchNumber;
            giftRow.GiftTransactionNumber = batchRow.LastGiftNumber + 1;
            FMainDS.AGift.Rows.Add(giftRow);

            ANewRow.LedgerNumber = batchRow.LedgerNumber;
            ANewRow.BatchNumber = batchRow.BatchNumber;
            ANewRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;

            // TODO: use previous gifts of donor?
            // ANewRow.MotivationGroupCode = "GIFT";
            // ANewRow.MotivationDetailCode = "SUPPORT";
            batchRow.LastGiftNumber++;
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();

            if (GetBatchRow() != null)
            {
                txtCurrencyCode.Text = GetBatchRow().CurrencyCode;
            }
        }

        private void ShowDetailsManual(AGiftDetailRow ARow)
        {
            // show cost centre
            MotivationDetailChanged(null, null);

            dtpDateEntered.Value = ((GiftBatchTDSAGiftDetailRow)ARow).DateEntered;
            txtDetailDonorKey.Text = String.Format("{0:0000000000}", ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey);
        }

        private void GetDetailDataFromControlsManual(AGiftDetailRow ARow)
        {
            ARow.CostCentreCode = txtDetailCostCentreCode.Text;

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
            giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
            giftRow.DateEntered = dtpDateEntered.Value;

            ((GiftBatchTDSAGiftDetailRow)ARow).DateEntered = giftRow.DateEntered;
            ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey = giftRow.DonorKey;
            ((GiftBatchTDSAGiftDetailRow)ARow).DonorName = txtDetailDonorKey.LabelText;
        }
    }
}