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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftTransactions
    {
        #region create new gift

        private void NewGift(System.Object sender, EventArgs e)
        {
            CreateANewRecurringGift(true);
        }

        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            CreateANewRecurringGift(false);
        }

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

            bool AutoSaveSuccessful = FSETAutoSaveFlag && HasChanges && ((TFrmRecurringGiftBatch)ParentForm).SaveChangesManual();

            FNewGiftInProcess = true;

            try
            {
                //May need to copy values down if a new detail row inside current Recurring Gift
                int recurringGiftTransactionNumber = 0;
                string donorName = string.Empty;
                string donorClass = string.Empty;
                bool confidentialRecurringGiftFlag = false;
                bool chargeFlag = false;
                bool taxDeductible = false;
                string motivationGroupCode = string.Empty;
                string motivationDetailCode = string.Empty;

                if (AutoSaveSuccessful || ((!FSETAutoSaveFlag || !HasChanges) && ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical)))
                {
                    if (!ACompletelyNewRecurringGift)      //i.e. a RecurringGift detail
                    {
                        ACompletelyNewRecurringGift = IsEmptyGrid;
                    }

                    CopyDetails = (!ACompletelyNewRecurringGift && !FPrevRowIsNull);

                    if (CopyDetails)
                    {
                        //Allow for possibility that FPrev... may have some null column values
                        recurringGiftTransactionNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;
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
                        CurrentRecurringGiftRow = GetRecurringGiftRow(recurringGiftTransactionNumber);
                        CurrentRecurringGiftRow.LastDetailNumber++;

                        //If adding detail to current last Recurring Gift, then new detail will be bottom row in grid
                        if (FBatchRow.LastGiftNumber == recurringGiftTransactionNumber)
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

                    cmbMotivationDetailCode.SetSelectedString(newRow.MotivationDetailCode, -1);
                    txtDetailMotivationDetailCode.Text = newRow.MotivationDetailCode;

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
                    UpdateRecipientKeyText(0,
                        FPreviouslySelectedDetailRow,
                        cmbDetailMotivationGroupCode.GetSelectedString(),
                        cmbMotivationDetailCode.GetSelectedString());

                    cmbKeyMinistries.Clear();
                    mniRecipientHistory.Enabled = false;
                }
            }
            finally
            {
                FNewGiftInProcess = false;

                if (AutoSaveSuccessful)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                }
            }
        }

        #endregion create new gift

        #region auto-populate gift

        /// <summary>
        /// auto populate new gift recipient info using the donor's last gift
        /// </summary>
        /// <param name="ADonorKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="ARecurringGiftTransactionNumber"></param>
        private void AutoPopulateGiftDetail(Int64 ADonorKey, String APartnerShortName, Int32 ARecurringGiftTransactionNumber)
        {
            FAutoPopulatingGiftInProcess = true;

            bool IsSplitGift = false;

            DateTime LatestUnpostedGiftDateEntered = new DateTime(1900, 1, 1);

            try
            {
                //Check for Donor in saved gift batches
                // and record most recent date entered
                GiftBatchTDSAGiftDetailTable GiftDetailTable = new GiftBatchTDSAGiftDetailTable();

                //Check for even more recent saved gifts on server (i.e. not necessarily loaded)
                GiftDetailTable = TRemote.MFinance.Gift.WebConnectors.LoadDonorLastPostedGift(ADonorKey, FLedgerNumber, LatestUnpostedGiftDateEntered);

                if (((GiftDetailTable != null) && (GiftDetailTable.Count > 0)))
                {
                    //UnLoaded/Saved gift from donor is more recent
                    IsSplitGift = (GiftDetailTable.Count > 1);
                }
                else
                {
                    //nothing to autocopy
                    return;
                }

                // if the last gift was a split gift (multiple details) then ask the user if they would like this new gift to also be split
                if (IsSplitGift)
                {
                    GiftDetailTable.DefaultView.Sort = GiftBatchTDSAGiftDetailTable.GetDetailNumberDBName() + " ASC";

                    string Message = string.Format(Catalog.GetString(
                            "The last gift from this donor was a split gift.{0}{0}Here are the details:{0}"), "\n");
                    int DetailNumber = 1;

                    foreach (DataRowView dvr in GiftDetailTable.DefaultView)
                    {
                        GiftBatchTDSAGiftDetailRow Row = (GiftBatchTDSAGiftDetailRow)dvr.Row;

                        Message += DetailNumber + ")  ";

                        if (Row.RecipientKey > 0)
                        {
                            Message +=
                                string.Format(Catalog.GetString("Recipient: {0} ({1});  Motivation Group: {2};  Motivation Detail: {3};  Amount: {4}"),
                                    Row.RecipientDescription, Row.RecipientKey, Row.MotivationGroupCode, Row.MotivationDetailCode,
                                    StringHelper.FormatUsingCurrencyCode(Row.GiftAmount, GetRecurringBatchRow().CurrencyCode) +
                                    " " + FBatchRow.CurrencyCode) +
                                "\n";
                        }

                        DetailNumber++;
                    }

                    Message += "\n" + Catalog.GetString("Do you want to create the same split gift again?");

                    if (!(MessageBox.Show(Message, Catalog.GetString(
                                  "Create Split Gift"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                          == DialogResult.Yes))
                    {
                        if (cmbDetailMethodOfGivingCode.CanFocus)
                        {
                            cmbDetailMethodOfGivingCode.Focus();
                        }
                        else if (txtDetailReference.CanFocus)
                        {
                            txtDetailReference.Focus();
                        }

                        return;
                    }
                }

                this.Cursor = Cursors.WaitCursor;

                GiftBatchTDSAGiftDetailRow giftDetailRow = (GiftBatchTDSAGiftDetailRow)GiftDetailTable.DefaultView[0].Row;

                // Handle first row, which is FPreviouslySelectedDetailRow
                txtDetailDonorKey.Text = String.Format("{0:0000000000}", ADonorKey);
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", giftDetailRow.RecipientKey);
                cmbDetailMotivationGroupCode.SetSelectedString(giftDetailRow.MotivationGroupCode);
                txtDetailMotivationDetailCode.Text = giftDetailRow.MotivationDetailCode;
                cmbMotivationDetailCode.SetSelectedString(giftDetailRow.MotivationDetailCode);
                chkDetailConfidentialGiftFlag.Checked = giftDetailRow.ConfidentialGiftFlag;
                chkDetailChargeFlag.Checked = giftDetailRow.ChargeFlag;
                cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment, -1);
                cmbDetailMethodOfGivingCode.SetSelectedString(giftDetailRow.MethodOfGivingCode, -1);
                chkDetailTaxDeductible.Checked = giftDetailRow.TaxDeductible;

                //Handle mailing code
                if (FSETAutoCopyIncludeMailingCodeFlag)
                {
                    cmbDetailMailingCode.SetSelectedString(giftDetailRow.MailingCode, -1);
                }
                else
                {
                    cmbDetailMailingCode.SelectedIndex = -1;
                }

                //Copy the comments and comment types if required
                if (FSETAutoCopyIncludeCommentsFlag)
                {
                    txtDetailGiftCommentOne.Text = giftDetailRow.GiftCommentOne;
                    cmbDetailCommentOneType.SetSelectedString(giftDetailRow.CommentOneType);
                    txtDetailGiftCommentTwo.Text = giftDetailRow.GiftCommentTwo;
                    cmbDetailCommentTwoType.SetSelectedString(giftDetailRow.CommentTwoType);
                    txtDetailGiftCommentThree.Text = giftDetailRow.GiftCommentThree;
                    cmbDetailCommentThreeType.SetSelectedString(giftDetailRow.CommentThreeType);
                }

                //Now process other gift details if they exist
                if (IsSplitGift)
                {
                    //Only copy amount to first row if copying split gifts
                    txtDetailGiftAmount.NumberValueDecimal = giftDetailRow.GiftTransactionAmount;

                    // clear previous validation errors.
                    // otherwise we get an error if the user has changed the control immediately after changing the donor key.
                    FPetraUtilsObject.VerificationResultCollection.Clear();

                    bool SelectEndRow = (FBatchRow.LastGiftNumber == FPreviouslySelectedDetailRow.GiftTransactionNumber);

                    //Just retain other details to add
                    giftDetailRow.Delete();
                    GiftDetailTable.AcceptChanges();

                    foreach (DataRowView drv in GiftDetailTable.DefaultView)
                    {
                        GiftBatchTDSAGiftDetailRow detailRow = (GiftBatchTDSAGiftDetailRow)drv.Row;

                        //______________________
                        //Update basic field values
                        detailRow.LedgerNumber = FLedgerNumber;
                        detailRow.BatchNumber = FBatchNumber;
                        detailRow.GiftTransactionNumber = ARecurringGiftTransactionNumber;
                        detailRow.DetailNumber = ++FGift.LastDetailNumber;
                        detailRow.DonorName = APartnerShortName;
                        detailRow.DonorClass = FPreviouslySelectedDetailRow.DonorClass;
                        detailRow.DateEntered = DateTime.Today;
                        detailRow.MethodOfPaymentCode = FPreviouslySelectedDetailRow.MethodOfPaymentCode;

                        if (!FSETAutoCopyIncludeMailingCodeFlag)
                        {
                            detailRow.MailingCode = string.Empty;
                        }

                        //______________________
                        //process recipient details to get most recent data
                        Int64 partnerKey = detailRow.RecipientKey;
                        string partnerShortName = string.Empty;
                        TPartnerClass partnerClass;
                        bool recipientIsValid = true;

                        if (TServerLookup.TMPartner.GetPartnerShortName(partnerKey, out partnerShortName, out partnerClass))
                        {
                            detailRow.RecipientDescription = partnerShortName;
                            detailRow.RecipientClass = partnerClass.ToString();

                            if (partnerClass == TPartnerClass.FAMILY)
                            {
                                detailRow.RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(partnerKey,
                                    DateTime.Today);
                                detailRow.RecipientField = detailRow.RecipientLedgerNumber;
                                detailRow.RecipientKeyMinistry = string.Empty;
                            }
                            else
                            {
                                //Class - UNIT
                                Int64 field;
                                string keyMinName;

                                recipientIsValid = TFinanceControls.GetRecipientKeyMinData(partnerKey, out field, out keyMinName);

                                detailRow.RecipientLedgerNumber = field;
                                detailRow.RecipientField = field;
                                detailRow.RecipientKeyMinistry = keyMinName;

                                if (!recipientIsValid)
                                {
                                    string msg =
                                        String.Format(Catalog.GetString(
                                                "Gift: {0}, Detail: {1} has a recipient: '{2}-{3}' that is an inactive Key Ministry!"),
                                            ARecurringGiftTransactionNumber,
                                            detailRow.DetailNumber,
                                            partnerKey,
                                            keyMinName);
                                    MessageBox.Show(msg, Catalog.GetString(
                                            "Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            recipientIsValid = false;
                            string msg = String.Format(Catalog.GetString("Gift: {0}, Detail: {1} has an invalid Recipient key: '{2}'!"),
                                ARecurringGiftTransactionNumber,
                                detailRow.DetailNumber,
                                partnerKey);
                            MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        //______________________
                        //Process motivation
                        if (string.IsNullOrEmpty(detailRow.MotivationGroupCode))
                        {
                            detailRow.MotivationGroupCode = string.Empty;
                            string msg = String.Format(Catalog.GetString("Gift: {0}, Detail: {1} has no Motivation Group!"),
                                ARecurringGiftTransactionNumber,
                                detailRow.DetailNumber);
                            MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (string.IsNullOrEmpty(detailRow.MotivationDetailCode))
                        {
                            detailRow.MotivationDetailCode = string.Empty;
                            string msg = String.Format(Catalog.GetString("Gift: {0}, Detail: {1} has no Motivation Detail!"),
                                ARecurringGiftTransactionNumber,
                                detailRow.DetailNumber);
                            MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            AMotivationDetailRow motivationDetailRow = null;
                            string motivationGroup = detailRow.MotivationGroupCode;
                            string motivationDetail = detailRow.MotivationDetailCode;

                            motivationDetailRow = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                                new object[] { FLedgerNumber, motivationGroup, motivationDetail });

                            if (motivationDetailRow != null)
                            {
                                if (partnerKey > 0)
                                {
                                    bool partnerIsMissingLink = false;

                                    detailRow.CostCentreCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FLedgerNumber,
                                        partnerKey,
                                        detailRow.RecipientLedgerNumber,
                                        detailRow.DateEntered,
                                        motivationGroup,
                                        motivationDetail,
                                        out partnerIsMissingLink);
                                }
                                else
                                {
                                    if (motivationGroup != MFinanceConstants.MOTIVATION_GROUP_GIFT)
                                    {
                                        detailRow.RecipientDescription = motivationGroup;
                                    }
                                    else
                                    {
                                        detailRow.RecipientDescription = string.Empty;
                                    }

                                    detailRow.CostCentreCode = motivationDetailRow.CostCentreCode;
                                }

                                detailRow.AccountCode = motivationDetailRow.AccountCode;
                            }
                            else
                            {
                                string msg =
                                    String.Format(Catalog.GetString(
                                            "Gift: {0}, Detail: {1} has Motivation Group and Detail codes ('{2} : {3}') not found in the database!"),
                                        ARecurringGiftTransactionNumber,
                                        detailRow.DetailNumber,
                                        motivationGroup,
                                        motivationDetail);
                                MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                detailRow.TaxDeductible = false;
                            }
                        }

                        //______________________
                        //Process comments
                        if (!FSETAutoCopyIncludeCommentsFlag)
                        {
                            detailRow.SetCommentOneTypeNull();
                            detailRow.SetCommentTwoTypeNull();
                            detailRow.SetCommentThreeTypeNull();
                            detailRow.SetGiftCommentOneNull();
                            detailRow.SetGiftCommentTwoNull();
                            detailRow.SetGiftCommentThreeNull();
                        }

                        detailRow.AcceptChanges();
                        detailRow.SetAdded();
                    }

                    //Add in the new records
                    //  It is OK to merge from a different table structure as long as common fields
                    //  exist as between AGiftDetail and ARecurringGiftDetail
                    FMainDS.ARecurringGiftDetail.Merge(GiftDetailTable);

                    int indexOfLatestRow = FMainDS.ARecurringGiftDetail.Rows.Count - 1;

                    //Select last row added
                    if (SelectEndRow)
                    {
                        grdDetails.SelectRowInGrid(grdDetails.Rows.Count - 1);
                    }
                    else if (!SelectDetailRowByDataTableIndex(indexOfLatestRow))
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

                            SelectDetailRowByDataTableIndex(indexOfLatestRow);
                        }
                    }

                    cmbKeyMinistries.Clear();
                    cmbMotivationDetailCode.Clear();
                    mniRecipientHistory.Enabled = false;
                    btnDeleteAll.Enabled = btnDelete.Enabled;
                    UpdateRecordNumberDisplay();
                    FLastDonor = -1;
                }
                else
                {
                    txtDetailDonorKey.FocusTextBoxPartAfterFindScreenCloses = false;
                    txtDetailGiftAmount.Focus();
                }

                FPetraUtilsObject.SetChangedFlag();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FAutoPopulatingGiftInProcess = false;
            }
        }

        #endregion auto-populate gift
    }
}