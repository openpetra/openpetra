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
    public partial class TUC_GiftTransactions
    {
        #region create new gift

        private void NewGift(System.Object sender, EventArgs e)
        {
            CreateANewGift(true);
        }

        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            CreateANewGift(false);
        }

        /// <summary>
        /// Creates a new Gift or Gift detail depending upon the parameter
        /// </summary>
        /// <param name="ACompletelyNewGift"></param>
        private void CreateANewGift(bool ACompletelyNewGift)
        {
            // Using a button's keyboard shortcut results in a different sequence of Events from clicking it with the mouse. If the current control is in pnlDetails,
            // then when the New or Delete button's processing attempts to save the current record and calls TFrmPetraUtils.ForceOnLeaveForActiveControl(),
            // it inadvertently re-raises the pnlDetails.Enter event which activates BeginEditMode() at a point when it's not supposed to be activated, putting
            // TCmbAutoComplete controls in a state they're not supposed to be in, resulting in a NullReferenceException from FPreviouslySelectedDetailRow
            // in UC_GiftTransactions.Motivation.ManualCode.cs, MotivationDetailChanged().
            // To fix it, put the focus outside pnlDetails, preventing the whole chain of events from happening.
            grdDetails.Focus();

            AGiftRow CurrentGiftRow = null;
            bool IsEmptyGrid = (grdDetails.Rows.Count == 1);
            bool HasChanges = FPetraUtilsObject.HasChanges;
            bool SelectEndRow = false;

            bool FPrevRowIsNull = (FPreviouslySelectedDetailRow == null);
            bool CopyDetails = false;

            bool AutoSaveSuccessful = FSETAutoSaveFlag && HasChanges && ((TFrmGiftBatch)ParentForm).SaveChangesManual();

            FNewGiftInProcess = true;

            try
            {
                //May need to copy values down if a new detail row inside current gift
                int giftTransactionNumber = 0;
                string donorName = string.Empty;
                string donorClass = string.Empty;
                bool confidentialGiftFlag = false;
                bool chargeFlag = false;
                bool taxDeductible = false;
                string motivationGroupCode = string.Empty;
                string motivationDetailCode = string.Empty;

                if (AutoSaveSuccessful || ((!FSETAutoSaveFlag || !HasChanges) && ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical)))
                {
                    if (!ACompletelyNewGift)      //i.e. a gift detail
                    {
                        ACompletelyNewGift = IsEmptyGrid;
                    }

                    CopyDetails = (!ACompletelyNewGift && !FPrevRowIsNull);

                    if (CopyDetails)
                    {
                        //Allow for possibility that FPrev... may have some null column values
                        giftTransactionNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;
                        donorName = FPreviouslySelectedDetailRow.IsDonorNameNull() ? string.Empty : FPreviouslySelectedDetailRow.DonorName;
                        donorClass = FPreviouslySelectedDetailRow.IsDonorClassNull() ? string.Empty : FPreviouslySelectedDetailRow.DonorClass;
                        confidentialGiftFlag =
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

                    if (ACompletelyNewGift)
                    {
                        //Run this if a new gift is requested or required.
                        SelectEndRow = true;

                        // we create the row locally, no dataset
                        AGiftRow giftRow = FMainDS.AGift.NewRowTyped(true);

                        giftRow.LedgerNumber = FBatchRow.LedgerNumber;
                        giftRow.BatchNumber = FBatchRow.BatchNumber;
                        giftRow.GiftTransactionNumber = ++FBatchRow.LastGiftNumber;
                        giftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                        giftRow.LastDetailNumber = 1;
                        giftRow.DateEntered = FBatchRow.GlEffectiveDate;

                        FMainDS.AGift.Rows.Add(giftRow);

                        CurrentGiftRow = giftRow;

                        mniDonorHistory.Enabled = false;

                        //Reset textboxes to zero
                        txtGiftTotal.NumberValueDecimal = 0;
                    }
                    else
                    {
                        CurrentGiftRow = GetGiftRow(giftTransactionNumber);
                        CurrentGiftRow.LastDetailNumber++;

                        //If adding detail to current last gift, then new detail will be bottom row in grid
                        if (FBatchRow.LastGiftNumber == giftTransactionNumber)
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

                    if (CopyDetails)
                    {
                        newRow.DonorName = donorName;
                        newRow.DonorClass = donorClass;
                        newRow.ConfidentialGiftFlag = confidentialGiftFlag;
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

                    newRow.DateEntered = CurrentGiftRow.DateEntered;
                    newRow.ReceiptPrinted = false;
                    newRow.ReceiptNumber = 0;

                    cmbMotivationDetailCode.SetSelectedString(newRow.MotivationDetailCode, -1);
                    txtDetailMotivationDetailCode.Text = newRow.MotivationDetailCode;

                    if (FSETUseTaxDeductiblePercentageFlag)
                    {
                        newRow.TaxDeductiblePct = newRow.TaxDeductible ? 100.0m : 0.0m;

                        //Set unbound textboxes to 0
                        txtTaxDeductAmount.NumberValueDecimal = 0.0m;
                        txtNonDeductAmount.NumberValueDecimal = 0.0m;
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

                    //FPreviouslySelectedDetailRow should now be pointing to the newly added row
                    UpdateRecipientKeyText(0,
                        FPreviouslySelectedDetailRow,
                        cmbDetailMotivationGroupCode.GetSelectedString(),
                        cmbMotivationDetailCode.GetSelectedString());

                    ClearKeyMinistries();
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
        /// <param name="AGiftTransactionNumber"></param>
        private void AutoPopulateGiftDetail(Int64 ADonorKey, String APartnerShortName, Int32 AGiftTransactionNumber)
        {
            FAutoPopulatingGiftInProcess = true;

            bool IsSplitGift = false;

            DateTime LatestUnpostedGiftDateEntered = new DateTime(1900, 1, 1);

            try
            {
                //Check for Donor in loaded gift batches
                // and record most recent date entered
                AGiftTable DonorRecentGiftsTable = new AGiftTable();
                GiftBatchTDSAGiftDetailTable GiftDetailTable = new GiftBatchTDSAGiftDetailTable();

                AGiftRow MostRecentLoadedGiftForDonorRow = null;

                DataView giftDV = new DataView(FMainDS.AGift);

                giftDV.RowStateFilter = DataViewRowState.CurrentRows;

                giftDV.RowFilter = string.Format("{0}={1} And Not ({2}={3} And {4}={5})",
                    AGiftTable.GetDonorKeyDBName(),
                    ADonorKey,
                    AGiftTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    AGiftTable.GetGiftTransactionNumberDBName(),
                    AGiftTransactionNumber);

                giftDV.Sort = String.Format("{0} DESC, {1} DESC",
                    AGiftTable.GetDateEnteredDBName(),
                    AGiftTable.GetGiftTransactionNumberDBName());

                if (giftDV.Count > 0)
                {
                    //Take first row = most recent date entered value
                    MostRecentLoadedGiftForDonorRow = (AGiftRow)giftDV[0].Row;
                    LatestUnpostedGiftDateEntered = MostRecentLoadedGiftForDonorRow.DateEntered;
                    DonorRecentGiftsTable.Rows.Add((object[])MostRecentLoadedGiftForDonorRow.ItemArray.Clone());
                }

                //Check for even more recent saved gifts on server (i.e. not necessarily loaded)
                GiftDetailTable = TRemote.MFinance.Gift.WebConnectors.LoadDonorLastPostedGift(ADonorKey, FLedgerNumber, LatestUnpostedGiftDateEntered);

                if (((GiftDetailTable != null) && (GiftDetailTable.Count > 0)))
                {
                    //UnLoaded/Saved gift from donor is more recent
                    IsSplitGift = (GiftDetailTable.Count > 1);
                }
                else if (MostRecentLoadedGiftForDonorRow != null)
                {
                    //Loaded/unsaved gift from donor is more recent
                    DataView giftDetailDV = new DataView(FMainDS.AGiftDetail);

                    giftDetailDV.RowStateFilter = DataViewRowState.CurrentRows;

                    giftDetailDV.RowFilter = string.Format("{0}={1} And {2}={3}",
                        AGiftDetailTable.GetBatchNumberDBName(),
                        MostRecentLoadedGiftForDonorRow.BatchNumber,
                        AGiftDetailTable.GetGiftTransactionNumberDBName(),
                        MostRecentLoadedGiftForDonorRow.GiftTransactionNumber);

                    foreach (DataRowView drv in giftDetailDV)
                    {
                        GiftBatchTDSAGiftDetailRow gDR = (GiftBatchTDSAGiftDetailRow)drv.Row;
                        GiftDetailTable.Rows.Add((object[])gDR.ItemArray.Clone());
                    }

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
                                    StringHelper.FormatUsingCurrencyCode(Row.GiftTransactionAmount, GetBatchRow().CurrencyCode) +
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

                //Handle tax fields on current row
                if (FSETUseTaxDeductiblePercentageFlag)
                {
                    bool taxDeductible = (giftDetailRow.IsTaxDeductibleNull() ? true : giftDetailRow.TaxDeductible);
                    giftDetailRow.TaxDeductible = taxDeductible;

                    try
                    {
                        FPetraUtilsObject.SuppressChangeDetection = true;
                        chkDetailTaxDeductible.Checked = taxDeductible;
                        EnableTaxDeductibilityPct(taxDeductible);
                    }
                    finally
                    {
                        FPetraUtilsObject.SuppressChangeDetection = false;
                    }

                    if (!IsSplitGift)
                    {
                        //Most commonly not a split gift (?)
                        if (!taxDeductible)
                        {
                            txtDeductiblePercentage.NumberValueDecimal = 0.0m;
                        }

                        txtTaxDeductAmount.NumberValueDecimal = 0.0m;
                        txtNonDeductAmount.NumberValueDecimal = 0.0m;
                    }
                    else
                    {
                        if (taxDeductible)
                        {
                            //In case the tax percentage has changed or null values in amount fields
                            ReconcileTaxDeductibleAmounts(giftDetailRow);
                        }
                        else
                        {
                            //Changing this will update the unbound amount textboxes
                            txtDeductiblePercentage.NumberValueDecimal = 0.0m;
                        }
                    }
                }

                //Process values that are not bound to a control
                giftDetailRow.ReceiptPrinted = false;
                giftDetailRow.ReceiptNumber = 0;

                //Now process other gift details if they exist
                if (IsSplitGift)
                {
                    //Only copy amount to first row if copying split gifts
                    txtDetailGiftTransactionAmount.NumberValueDecimal = giftDetailRow.GiftTransactionAmount;

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
                        detailRow.GiftTransactionNumber = AGiftTransactionNumber;
                        detailRow.DetailNumber = ++FGift.LastDetailNumber;
                        detailRow.DonorName = APartnerShortName;
                        detailRow.DonorClass = FPreviouslySelectedDetailRow.DonorClass;
                        detailRow.DateEntered = FGift.DateEntered;
                        detailRow.MethodOfPaymentCode = FPreviouslySelectedDetailRow.MethodOfPaymentCode;
                        detailRow.ReceiptPrinted = false;
                        detailRow.ReceiptNumber = 0;

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
                                    FGift.DateEntered);
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
                                            AGiftTransactionNumber,
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
                                AGiftTransactionNumber,
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
                                AGiftTransactionNumber,
                                detailRow.DetailNumber);
                            MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (string.IsNullOrEmpty(detailRow.MotivationDetailCode))
                        {
                            detailRow.MotivationDetailCode = string.Empty;
                            string msg = String.Format(Catalog.GetString("Gift: {0}, Detail: {1} has no Motivation Detail!"),
                                AGiftTransactionNumber,
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

                                if (FSETUseTaxDeductiblePercentageFlag && string.IsNullOrEmpty(motivationDetailRow.TaxDeductibleAccountCode))
                                {
                                    detailRow.TaxDeductibleAccountCode = string.Empty;

                                    string msg = String.Format(Catalog.GetString(
                                            "Gift: {0}, Detail: {1} has  Motivation Detail: {2} which has no Tax Deductible Account!" +
                                            "This can be added in Finance / Setup / Motivation Details.{3}{3}" +
                                            "Unless this is changed it will be impossible to assign a Tax Deductible Percentage to this gift."),
                                        AGiftTransactionNumber,
                                        detailRow.DetailNumber,
                                        motivationDetail,
                                        Environment.NewLine);
                                    MessageBox.Show(msg, Catalog.GetString(
                                            "Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    detailRow.TaxDeductibleAccountCode = motivationDetailRow.TaxDeductibleAccountCode;
                                }
                            }
                            else
                            {
                                string msg =
                                    String.Format(Catalog.GetString(
                                            "Gift: {0}, Detail: {1} has Motivation Group and Detail codes ('{2} : {3}') not found in the database!"),
                                        AGiftTransactionNumber,
                                        detailRow.DetailNumber,
                                        motivationGroup,
                                        motivationDetail);
                                MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                detailRow.TaxDeductible = false;
                            }
                        }

                        //______________________
                        //Handle tax fields
                        detailRow.TaxDeductiblePct = RetrieveTaxDeductiblePct((recipientIsValid ? detailRow.RecipientKey : 0),
                            detailRow.TaxDeductible);

                        AGiftDetailRow giftDetails = (AGiftDetailRow)detailRow;
                        TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref giftDetails);

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

                    //Add in the new records (latter two arguments put in to parallel recurring form)
                    FMainDS.AGiftDetail.Merge(GiftDetailTable, false, MissingSchemaAction.Ignore);

                    int indexOfLatestRow = FMainDS.AGiftDetail.Rows.Count - 1;

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

                    ClearKeyMinistries();
                    cmbMotivationDetailCode.Clear();
                    mniRecipientHistory.Enabled = false;
                    btnDeleteAll.Enabled = btnDelete.Enabled;
                    UpdateRecordNumberDisplay();
                    FLastDonor = -1;
                }
                else
                {
                    txtDetailDonorKey.FocusTextBoxPartAfterFindScreenCloses = false;
                    txtDetailGiftTransactionAmount.Focus();
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