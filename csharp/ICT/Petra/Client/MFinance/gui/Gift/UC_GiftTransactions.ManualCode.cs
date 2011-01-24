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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
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
            btnDeleteDetail.Enabled = FPetraUtilsObject.DetailProtectedMode;
            btnNewDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            btnNewGift.Enabled = !FPetraUtilsObject.DetailProtectedMode;

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

        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            String strMotivationGroup;
            String strMotivationDetail;

            strMotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            strMotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();

            if (TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetail(
                    APartnerKey, ref strMotivationGroup, ref strMotivationDetail))
            {
                if (strMotivationDetail.Equals(MFinanceConstants.GROUP_DETAIL_KEY_MIN))
                {
                    cmbDetailMotivationDetailCode.SetSelectedString(MFinanceConstants.GROUP_DETAIL_KEY_MIN);
                }
            }
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
                if (motivationDetail.CostCentreCode.EndsWith("S"))
                {
                    // work around if we have selected the cost centre already in bank import
                    // TODO: allow to select the cost centre here, which reports to the motivation cost centre
                    //txtDetailCostCentreCode.Text =
                }
                else
                {
                    txtDetailCostCentreCode.Text = motivationDetail.CostCentreCode;
                }
            }
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widhts needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
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
        /// delete a gift detail, and if it is the last detail, delete the whole gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDetail(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            int oldDetailNumber = FPreviouslySelectedDetailRow.DetailNumber;
            AGiftRow gift = GetGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
            string filterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                AGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);
            FMainDS.AGiftDetail.Rows.Remove(FPreviouslySelectedDetailRow);
            FPreviouslySelectedDetailRow = null;
            DataView giftDetailView = new DataView(FMainDS.AGiftDetail);
            giftDetailView.RowFilter = filterAllDetailsOfGift;

            if (giftDetailView.Count == 0)
            {
                int oldGiftNumber = gift.GiftTransactionNumber;
                int oldBatchNumber = gift.BatchNumber;

                FMainDS.AGift.Rows.Remove(gift);

// we cannot update primary keys easily, therefore we have to do it later on the server side
#if DISABLED
                string filterAllDetailsOfBatch = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    oldBatchNumber);

                giftDetailView.RowFilter = filterAllDetailsOfBatch;

                foreach (DataRowView rv in giftDetailView)
                {
                    GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                    if (row.GiftTransactionNumber > oldGiftNumber)
                    {
                        row.GiftTransactionNumber--;
                    }
                }
                GetBatchRow().LastGiftNumber--;
#endif
            }
            else
            {
                foreach (DataRowView rv in giftDetailView)
                {
                    GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                    if (row.DetailNumber > oldDetailNumber)
                    {
                        row.DetailNumber--;
                    }
                }

                gift.LastDetailNumber--;
            }

            FPetraUtilsObject.SetChangedFlag();

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);
            grdDetails.Refresh();
        }

        /// <summary>
        /// add a new gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGift(System.Object sender, EventArgs e)
        {
            // this is coded manually, to use the correct gift record

            // we create the table locally, no dataset
            AGiftDetailRow NewRow = NewGift();

            FPetraUtilsObject.SetChangedFlag();

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);
            grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);
        }

        /// <summary>
        /// add a new gift detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            // this is coded manually, to use the correct gift record

            // we create the table locally, no dataset
            AGiftDetailRow NewRow = NewGiftDetail((GiftBatchTDSAGiftDetailRow)FPreviouslySelectedDetailRow);

            FPetraUtilsObject.SetChangedFlag();

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);
            grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the batch.lastTransactionNumber is updated
        /// </summary>
        private AGiftDetailRow NewGift()
        {
            AGiftBatchRow batchRow = GetBatchRow();

            AGiftRow giftRow = FMainDS.AGift.NewRowTyped(true);

            giftRow.LedgerNumber = batchRow.LedgerNumber;
            giftRow.BatchNumber = batchRow.BatchNumber;
            giftRow.GiftTransactionNumber = batchRow.LastGiftNumber + 1;
            batchRow.LastGiftNumber++;
            giftRow.LastDetailNumber = 1;
            FMainDS.AGift.Rows.Add(giftRow);

            GiftBatchTDSAGiftDetailRow newRow = FMainDS.AGiftDetail.NewRowTyped(true);
            newRow.LedgerNumber = batchRow.LedgerNumber;
            newRow.BatchNumber = batchRow.BatchNumber;
            newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
            newRow.DetailNumber = 1;
            newRow.DateEntered = giftRow.DateEntered;
            newRow.DonorKey = 0;
            FMainDS.AGiftDetail.Rows.Add(newRow);

            // TODO: use previous gifts of donor?
            //newRow.MotivationGroupCode = "GIFT";
            //newRow.MotivationDetailCode = "SUPPORT";

            return newRow;
        }

        /// <summary>
        /// add another gift detail to an existing gift
        /// </summary>
        private AGiftDetailRow NewGiftDetail(GiftBatchTDSAGiftDetailRow ACurrentRow)
        {
            if (ACurrentRow == null)
            {
                return NewGift();
            }

            // find gift row
            AGiftRow giftRow = GetGiftRow(ACurrentRow.GiftTransactionNumber);

            giftRow.LastDetailNumber++;

            GiftBatchTDSAGiftDetailRow newRow = FMainDS.AGiftDetail.NewRowTyped(true);
            newRow.LedgerNumber = giftRow.LedgerNumber;
            newRow.BatchNumber = giftRow.BatchNumber;
            newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
            newRow.DetailNumber = giftRow.LastDetailNumber;
            newRow.DonorKey = ACurrentRow.DonorKey;
            newRow.DonorName = ACurrentRow.DonorName;
            newRow.DateEntered = ACurrentRow.DateEntered;
            FMainDS.AGiftDetail.Rows.Add(newRow);

            // TODO: use previous gifts of donor?
            // newRow.MotivationGroupCode = "GIFT";
            // newRow.MotivationDetailCode = "SUPPORT";

            return newRow;
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

            dtpDateEntered.Date = ((GiftBatchTDSAGiftDetailRow)ARow).DateEntered;
            txtDetailDonorKey.Text = String.Format("{0:0000000000}", ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey);

            dtpDateEntered.Enabled = (ARow.DetailNumber == 1);
            txtDetailDonorKey.Enabled = (ARow.DetailNumber == 1);
        }

        private void GetDetailDataFromControlsManual(AGiftDetailRow ARow)
        {
            ARow.CostCentreCode = txtDetailCostCentreCode.Text;

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
            giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
            giftRow.DateEntered = dtpDateEntered.Date.Value;

            ((GiftBatchTDSAGiftDetailRow)ARow).DateEntered = giftRow.DateEntered;
            ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey = giftRow.DonorKey;
            ((GiftBatchTDSAGiftDetailRow)ARow).DonorName = txtDetailDonorKey.LabelText;
        }
    }
}