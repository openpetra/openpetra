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
    public partial class TUC_RecurringGiftTransactions
    {
        /// <summary>
        /// Creates a new Recurring Gift or Recurring Gift detail depending upon the parameter
        /// </summary>
        /// <param name="ACompletelyNewRecurringGift"></param>
        private void CreateANewRecurringGift(bool ACompletelyNewRecurringGift)
        {
            ARecurringGiftRow CurrentRecurringGiftRow = null;
            bool IsEmptyGrid = (grdDetails.Rows.Count == 1);
            bool HasChanges = FPetraUtilsObject.HasChanges;
            bool SelectEndRow = false;

            bool FPrevRowIsNull = (FPreviouslySelectedDetailRow == null);
            bool CopyDetails = false;

            bool AutoSaveSuccessful = FAutoSave && HasChanges && ((TFrmRecurringGiftBatch)ParentForm).SaveChangesManual();

            FCreatingNewGift = true;

            try
            {
                //May need to copy values down if a new detail row inside current Recurring Gift
                int RecurringGiftTransactionNumber = 0;
                string donorName = string.Empty;
                string donorClass = string.Empty;
                bool confidentialRecurringGiftFlag = false;
                bool chargeFlag = false;
                bool taxDeductible = false;
                string motivationGroupCode = string.Empty;
                string motivationDetailCode = string.Empty;

                if (AutoSaveSuccessful || ((!FAutoSave || !HasChanges) && ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical)))
                {
                    if (!ACompletelyNewRecurringGift)      //i.e. a RecurringGift detail
                    {
                        ACompletelyNewRecurringGift = IsEmptyGrid;
                    }

                    CopyDetails = (!ACompletelyNewRecurringGift && !FPrevRowIsNull);

                    if (CopyDetails)
                    {
                        //Allow for possibility that FPrev... may have some null column values
                        RecurringGiftTransactionNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;
                        donorName = FPreviouslySelectedDetailRow.IsDonorNameNull() ? string.Empty : FPreviouslySelectedDetailRow.DonorName;
                        donorClass = FPreviouslySelectedDetailRow.IsDonorClassNull() ? string.Empty : FPreviouslySelectedDetailRow.DonorClass;
                        confidentialRecurringGiftFlag =
                            FPreviouslySelectedDetailRow.IsConfidentialGiftFlagNull() ? false : FPreviouslySelectedDetailRow.ConfidentialGiftFlag;
                        chargeFlag = FPreviouslySelectedDetailRow.IsChargeFlagNull() ? true : FPreviouslySelectedDetailRow.ChargeFlag;
                        taxDeductible = FPreviouslySelectedDetailRow.IsTaxDeductibleNull() ? true : FPreviouslySelectedDetailRow.TaxDeductible;
                        motivationGroupCode =
                            FPreviouslySelectedDetailRow.IsMotivationGroupCodeNull() ? string.Empty : FPreviouslySelectedDetailRow.
                            MotivationGroupCode;
                        motivationDetailCode =
                            FPreviouslySelectedDetailRow.IsMotivationDetailCodeNull() ? string.Empty : FPreviouslySelectedDetailRow.
                            MotivationDetailCode;
                    }

                    //Set previous row to Null.
                    FPreviouslySelectedDetailRow = null;

                    if (ACompletelyNewRecurringGift)
                    {
                        //Run this if a new Recurring Gift is requested or required.
                        SelectEndRow = true;

                        // we create the row locally, no dataset
                        ARecurringGiftRow recurringGiftRow = FMainDS.ARecurringGift.NewRowTyped(true);

                        recurringGiftRow.LedgerNumber = FBatchRow.LedgerNumber;
                        recurringGiftRow.BatchNumber = FBatchRow.BatchNumber;
                        recurringGiftRow.GiftTransactionNumber = ++FBatchRow.LastGiftNumber;
                        recurringGiftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                        recurringGiftRow.LastDetailNumber = 1;

                        FMainDS.ARecurringGift.Rows.Add(recurringGiftRow);

                        CurrentRecurringGiftRow = recurringGiftRow;

                        mniDonorHistory.Enabled = false;

                        //Reset textboxes to zero
                        txtGiftTotal.NumberValueDecimal = 0;
                    }
                    else
                    {
                        CurrentRecurringGiftRow = GetRecurringGiftRow(RecurringGiftTransactionNumber);
                        CurrentRecurringGiftRow.LastDetailNumber++;

                        //If adding detail to current last Recurring Gift, then new detail will be bottom row in grid
                        if (FBatchRow.LastGiftNumber == RecurringGiftTransactionNumber)
                        {
                            SelectEndRow = true;
                        }
                    }

                    //New Recurring Gifts will require a new detail anyway, so this code always runs
                    GiftBatchTDSARecurringGiftDetailRow newRow = FMainDS.ARecurringGiftDetail.NewRowTyped(true);

                    newRow.LedgerNumber = FBatchRow.LedgerNumber;
                    newRow.BatchNumber = FBatchRow.BatchNumber;
                    newRow.GiftTransactionNumber = CurrentRecurringGiftRow.GiftTransactionNumber;
                    newRow.DetailNumber = CurrentRecurringGiftRow.LastDetailNumber;
                    newRow.MethodOfPaymentCode = CurrentRecurringGiftRow.MethodOfPaymentCode;
                    newRow.MethodOfGivingCode = CurrentRecurringGiftRow.MethodOfGivingCode;
                    newRow.DonorKey = CurrentRecurringGiftRow.DonorKey;
                    newRow.DateEntered = DateTime.Now;

                    if (CopyDetails)
                    {
                        newRow.DonorName = donorName;
                        newRow.DonorClass = donorClass;
                        newRow.ConfidentialGiftFlag = confidentialRecurringGiftFlag;
                        newRow.ChargeFlag = chargeFlag;
                        newRow.TaxDeductible = taxDeductible;
                        newRow.MotivationGroupCode = motivationGroupCode;
                        newRow.MotivationDetailCode = motivationDetailCode;

                        // set the auto-populate comment if needed
                        AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                            new object[] { FLedgerNumber, newRow.MotivationGroupCode, newRow.MotivationDetailCode });

                        if ((motivationDetail != null) && motivationDetail.Autopopdesc)
                        {
                            newRow.GiftCommentOne = motivationDetail.MotivationDetailDesc;
                        }
                    }
                    else
                    {
                        newRow.MotivationGroupCode = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                        newRow.MotivationDetailCode = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                    }

                    FMainDS.ARecurringGiftDetail.Rows.Add(newRow);

                    FPetraUtilsObject.SetChangedFlag();

                    if (!SelectEndRow && !SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1))
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

                            SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1);
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
                    if (ACompletelyNewRecurringGift)
                    {
                        txtDetailDonorKey.Focus();
                    }
                    else
                    {
                        txtDetailRecipientKey.Focus();
                    }

                    //FPreviouslySelectedDetailRow should now be pointing to the newly added row
                    TUC_RecurringGiftTransactions_Recipient.UpdateRecipientKeyText(0,
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
        /// add a new Recurring Gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGift(System.Object sender, EventArgs e)
        {
            CreateANewRecurringGift(true);
        }

        /// <summary>
        /// add a new Recurring Gift detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            CreateANewRecurringGift(false);
        }
    }
}