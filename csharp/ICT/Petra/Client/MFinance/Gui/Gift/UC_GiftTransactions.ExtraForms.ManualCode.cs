//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
#region usings

using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MPartner.Gui;

using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        #region gift reverse/adjust form
        /// <summary>
        /// Reverse the whole gift batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReverseGiftBatch(System.Object sender, System.EventArgs e)
        {
            // This will throw an exception if insufficient permissions
            TSecurityChecks.CheckUserModulePermissions("FINANCE-2", "ReverseGiftBatch [raised by Client Proxy for ModuleAccessManager]");

            ShowRevertAdjustForm(GiftAdjustmentFunctionEnum.ReverseGiftBatch);
        }

        private void ReverseGift(System.Object sender, System.EventArgs e)
        {
            // This will throw an exception if insufficient permissions
            TSecurityChecks.CheckUserModulePermissions("FINANCE-2", "ReverseGift [raised by Client Proxy for ModuleAccessManager]");

            ShowRevertAdjustForm(GiftAdjustmentFunctionEnum.ReverseGift);
        }

        /// <summary>
        /// show the form for the gift reversal/adjustment
        /// </summary>
        /// <param name="AFunctionName">Which function shall be called on the server</param>
        private void ShowRevertAdjustForm(GiftAdjustmentFunctionEnum AFunctionName)
        {
            TFrmGiftBatch ParentGiftBatchForm = (TFrmGiftBatch)ParentForm;
            bool ReverseWholeBatch = (AFunctionName == GiftAdjustmentFunctionEnum.ReverseGiftBatch);
            bool AdjustGift = (AFunctionName == GiftAdjustmentFunctionEnum.AdjustGift);

            if (!ParentGiftBatchForm.SaveChangesManual())
            {
                return;
            }

            ParentGiftBatchForm.Cursor = Cursors.WaitCursor;

            AGiftBatchRow giftBatch = ((TFrmGiftBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
            int BatchNumber = giftBatch.BatchNumber;

            if (giftBatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift Batch to Reverse."));
                ParentGiftBatchForm.Cursor = Cursors.Default;
                return;
            }

            if (!giftBatch.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED))
            {
                MessageBox.Show(Catalog.GetString("This function is only possible when the selected batch is already posted."));
                ParentGiftBatchForm.Cursor = Cursors.Default;
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Please save first and than try again!"));
                ParentGiftBatchForm.Cursor = Cursors.Default;
                return;
            }

            if (ReverseWholeBatch && (FBatchNumber != BatchNumber))
            {
                ParentGiftBatchForm.SelectTab(TFrmGiftBatch.eGiftTabs.Transactions, true);
                ParentGiftBatchForm.SelectTab(TFrmGiftBatch.eGiftTabs.Batches);
                ParentGiftBatchForm.Cursor = Cursors.WaitCursor;
            }

            if (!ReverseWholeBatch && (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift to Adjust/Reverse."));
                ParentGiftBatchForm.Cursor = Cursors.Default;
                return;
            }

            TFrmGiftRevertAdjust revertForm = new TFrmGiftRevertAdjust(FPetraUtilsObject.GetForm());

            if (AdjustGift)
            {
                if (FSETUseTaxDeductiblePercentageFlag)
                {
                    revertForm.CheckTaxDeductPctChange = true;
                }

                revertForm.CheckGiftDestinationChange = true;
            }

            try
            {
                ParentForm.ShowInTaskbar = false;
                revertForm.LedgerNumber = FLedgerNumber;
                revertForm.CurrencyCode = giftBatch.CurrencyCode;

                // put spaces inbetween words
                revertForm.Text = Regex.Replace(AFunctionName.ToString(), "([a-z])([A-Z])", @"$1 $2");

                revertForm.AddParam("Function", AFunctionName);
                revertForm.AddParam("BatchNumber", giftBatch.BatchNumber);

                if (AdjustGift)
                {
                    int workingTransactionNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;
                    int workingDetailNumber = FPreviouslySelectedDetailRow.DetailNumber;
                    revertForm.GiftDetailRow = (AGiftDetailRow)FMainDS.AGiftDetail.Rows.Find(
                        new object[] { giftBatch.LedgerNumber, giftBatch.BatchNumber, workingTransactionNumber, workingDetailNumber });
                }

                if (ReverseWholeBatch)
                {
                    revertForm.GetGiftsForReverseAdjust(); // Added Feb '17 Tim Ingham - previously, reversing a whole batch didn't work.
                }

                if (!revertForm.IsDisposed && (revertForm.ShowDialog() == DialogResult.OK))
                {
                    ParentGiftBatchForm.Cursor = Cursors.WaitCursor;

                    if ((revertForm.AdjustmentBatchNumber > 0) && (revertForm.AdjustmentBatchNumber != giftBatch.BatchNumber))
                    {
                        // select the relevant batch
                        ParentGiftBatchForm.InitialBatchNumber = revertForm.AdjustmentBatchNumber;
                    }

                    ParentGiftBatchForm.RefreshAll();
                }
            }
            finally
            {
                ParentGiftBatchForm.Cursor = Cursors.WaitCursor;
                revertForm.Dispose();
                ParentForm.ShowInTaskbar = true;
                ParentGiftBatchForm.Cursor = Cursors.Default;
            }

            if (AdjustGift && (ParentGiftBatchForm.ActiveTab() == TFrmGiftBatch.eGiftTabs.Transactions))
            {
                //Select first row for adjusting, i.e. first +ve amount
                foreach (DataRowView drv in FMainDS.AGiftDetail.DefaultView)
                {
                    AGiftDetailRow gdr = (AGiftDetailRow)drv.Row;

                    if (gdr.GiftTransactionAmount > 0)
                    {
                        grdDetails.SelectRowInGrid(grdDetails.Rows.DataSourceRowToIndex(drv) + 1);
                    }
                }
            }
        }

        private void ReverseGiftDetail(System.Object sender, System.EventArgs e)
        {
            // This will throw an exception if insufficient permissions
            TSecurityChecks.CheckUserModulePermissions("FINANCE-2", "ReverseGiftDetail [raised by Client Proxy for ModuleAccessManager]");

            ShowRevertAdjustForm(GiftAdjustmentFunctionEnum.ReverseGiftDetail);
        }

        private void AdjustGift(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm(GiftAdjustmentFunctionEnum.AdjustGift);
        }

        private Boolean IsAdjustmentToReversedDetail(AGiftDetailRow ARow)
        {
            // This method returns true if the gift detail row is an unposted adjustement to a reversed gift detail
            // We use the LinkToPreviousGift property to discover if this gift is associated with the previous gift row.
            // Then we check if that row has a modifiedDetail flag sets for its first detail.
            if ((ARow == null) || (FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if ((giftRow != null) && (giftRow.LinkToPreviousGift == true))
            {
                if (ARow.GiftTransactionNumber > 1)
                {
                    AGiftDetailRow prevDetailRow = (AGiftDetailRow)FMainDS.AGiftDetail.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.BatchNumber, ARow.GiftTransactionNumber - 1, 1 });

                    return prevDetailRow.ModifiedDetail == true;
                }
            }

            return false;
        }

        #endregion gift reverse/adjust form

        #region open donor history

        private void OpenDonorHistory(System.Object sender, EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(true,
                Convert.ToInt64(txtDetailDonorKey.Text),
                FPetraUtilsObject.GetForm());
        }

        #endregion open donor history

        #region open donor finance details

        private void OpenDonorFinanceDetails(System.Object sender, EventArgs e)
        {
            TFrmPartnerEdit frmPartner = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frmPartner.SetParameters(TScreenMode.smEdit,
                FPreviouslySelectedDetailRow.DonorKey,
                Shared.MPartner.TPartnerEditTabPageEnum.petpFinanceDetails);
            frmPartner.Show();
        }

        #endregion open donor finance details

        #region open recipient history

        private void OpenRecipientHistory(System.Object sender, EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(false,
                Convert.ToInt64(txtDetailRecipientKey.Text),
                FPetraUtilsObject.GetForm());
        }

        #endregion open recipient history

        #region open gift destination

        private void OpenGiftDestination(System.Object sender, EventArgs e)
        {
            if (txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY)
            {
                TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(
                    FPetraUtilsObject.GetForm(), FPreviouslySelectedDetailRow.RecipientKey);
                GiftDestinationForm.Show();
            }
        }

        #endregion open gift destination
    }
}