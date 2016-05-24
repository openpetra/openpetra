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
using System;
using System.Windows.Forms;

using Ict.Common.Verification;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        /// <summary>
        /// Creates a new gift or gift detail depending upon the parameter
        /// </summary>
        /// <param name="ACompletelyNewGift"></param>
        private void CreateANewGift(bool ACompletelyNewGift)
        {
            AGiftRow CurrentGiftRow = null;
            bool IsEmptyGrid = (grdDetails.Rows.Count == 1);
            bool HasChanges = FPetraUtilsObject.HasChanges;
            bool SelectEndRow = false;

            bool AutoSaveSuccessful = FAutoSave && HasChanges && ((TFrmGiftBatch)ParentForm).SaveChangesManual();

            FCreatingNewGift = true;

            try
            {
                if (AutoSaveSuccessful || ((!FAutoSave || !HasChanges) && ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical)))
                {
                    if (!ACompletelyNewGift)      //i.e. a gift detail
                    {
                        ACompletelyNewGift = IsEmptyGrid;
                    }

                    if (ACompletelyNewGift)
                    {
                        //Run this if a new gift is requested or required.
                        SelectEndRow = true;

                        // we create the table locally, no dataset
                        AGiftRow giftRow = FMainDS.AGift.NewRowTyped(true);

                        giftRow.DateEntered = FBatchRow.GlEffectiveDate;
                        giftRow.LedgerNumber = FBatchRow.LedgerNumber;
                        giftRow.BatchNumber = FBatchRow.BatchNumber;
                        giftRow.GiftTransactionNumber = ++FBatchRow.LastGiftNumber;
                        giftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                        giftRow.LastDetailNumber = 1;

                        FMainDS.AGift.Rows.Add(giftRow);

                        CurrentGiftRow = giftRow;

                        mniDonorHistory.Enabled = false;
                    }
                    else
                    {
                        CurrentGiftRow = GetGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
                        CurrentGiftRow.LastDetailNumber++;

                        //If adding detail to current last gift, then new detail will be bottom row in grid
                        if (FBatchRow.LastGiftNumber == FPreviouslySelectedDetailRow.GiftTransactionNumber)
                        {
                            SelectEndRow = true;
                        }
                    }

                    //New gifts will require a new detail anyway, so this code always runs
                    GiftBatchTDSAGiftDetailRow newRow = FMainDS.AGiftDetail.NewRowTyped(true);

                    newRow.LedgerNumber = FBatchRow.LedgerNumber;
                    newRow.BatchNumber = FBatchRow.BatchNumber;
                    newRow.GiftTransactionNumber = CurrentGiftRow.GiftTransactionNumber;
                    newRow.DetailNumber = CurrentGiftRow.LastDetailNumber;
                    newRow.MethodOfPaymentCode = CurrentGiftRow.MethodOfPaymentCode;
                    newRow.MethodOfGivingCode = CurrentGiftRow.MethodOfGivingCode;
                    newRow.DonorKey = CurrentGiftRow.DonorKey;

                    if (!ACompletelyNewGift && (FPreviouslySelectedDetailRow != null))
                    {
                        newRow.DonorName = FPreviouslySelectedDetailRow.DonorName;
                        newRow.DonorClass = FPreviouslySelectedDetailRow.DonorClass;
                        newRow.ConfidentialGiftFlag = FPreviouslySelectedDetailRow.ConfidentialGiftFlag;
                        newRow.ChargeFlag = FPreviouslySelectedDetailRow.ChargeFlag;
                        newRow.TaxDeductible = FPreviouslySelectedDetailRow.TaxDeductible;
                        newRow.MotivationGroupCode = FPreviouslySelectedDetailRow.MotivationGroupCode;
                        newRow.MotivationDetailCode = FPreviouslySelectedDetailRow.MotivationDetailCode;
                    }
                    else
                    {
                        newRow.MotivationGroupCode = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                        newRow.MotivationDetailCode = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                    }

                    newRow.ReceiptPrinted = false;
                    newRow.ReceiptNumber = 0;
                    newRow.DateEntered = CurrentGiftRow.DateEntered;

                    if (FTaxDeductiblePercentageEnabled)
                    {
                        newRow.TaxDeductiblePct = 100;
                    }

                    FMainDS.AGiftDetail.Rows.Add(newRow);

                    FPetraUtilsObject.SetChangedFlag();

                    if (!SelectEndRow && !SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1))
                    {
                        if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
                        {
                            MessageBox.Show(
                                MCommonResourcestrings.StrNewRecordIsFiltered,
                                MCommonResourcestrings.StrAddNewRecordTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                            if (FFilterAndFindObject.FilterFindPanel.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                            {
                                FFilterAndFindObject.ApplyFilter();
                            }

                            SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);
                        }
                    }

                    btnDeleteAll.Enabled = btnDelete.Enabled;
                    UpdateRecordNumberDisplay();
                    FLastDonor = -1;

                    //Select end row
                    if (SelectEndRow)
                    {
                        grdDetails.SelectRowInGrid(grdDetails.Rows.Count - 1);
                    }

                    //Focus accordingly
                    if (ACompletelyNewGift)
                    {
                        txtDetailDonorKey.Focus();
                    }
                    else
                    {
                        txtDetailRecipientKey.Focus();
                    }

                    TUC_GiftTransactions_Recipient.UpdateRecipientKeyText(0,
                        FPreviouslySelectedDetailRow,
                        cmbDetailMotivationGroupCode.GetSelectedString(),
                        cmbDetailMotivationDetailCode.GetSelectedString());
                    cmbKeyMinistries.Clear();
                    mniRecipientHistory.Enabled = false;
                }
            }
            finally
            {
                FCreatingNewGift = false;

                if (AutoSaveSuccessful)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                }
            }
        }

        /// <summary>
        /// add a new gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGift(System.Object sender, EventArgs e)
        {
            CreateANewGift(true);
        }

        /// <summary>
        /// add a new gift detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            CreateANewGift(false);
        }
    }
}