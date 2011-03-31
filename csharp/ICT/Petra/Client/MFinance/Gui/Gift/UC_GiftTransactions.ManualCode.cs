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

using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int64 FLastDonor = -1;

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
            btnDeleteDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode;
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
            TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, ActiveOnly);
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);
            TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, ActiveOnly);
            //TFinanceControls.InitialiseKeyMinList(ref cmbMinistry, (Int64)0);


            //TODO            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);
            //TODO            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);

            ShowData();
        }

        bool FinRecipientKeyChanging = false;
        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            String strMotivationGroup;
            String strMotivationDetail;

            if (FinRecipientKeyChanging | FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FinRecipientKeyChanging = true;
            try
            {
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

                if (!FInKeyMinistryChanging)
                {
                    //...this does not work as expected, because the timer fires valuechanged event after this value is reset
                    TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, APartnerKey);
                }
            }
            finally
            {
                FinRecipientKeyChanging = false;
            }
        }

        private void DonorKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            // At the moment this event is thrown twice
            // We want to deal only on manual entered changes, not on selections changes
            if (FPetraUtilsObject.SuppressChangeDetection)
            {
                FLastDonor = APartnerKey;
            }
            else
            {
                if (APartnerKey != FLastDonor)
                {
                    GLSetupTDS PartnerDS = TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey);

                    if (PartnerDS.PPartner.Rows.Count > 0)
                    {
                        PPartnerRow pr = PartnerDS.PPartner[0];
                        chkDetailConfidentialGiftFlag.Checked = pr.AnonymousDonor;
                    }

                    FLastDonor = APartnerKey;

                    foreach (GiftBatchTDSAGiftDetailRow giftDetail in FMainDS.AGiftDetail.Rows)
                    {
                        if (giftDetail.BatchNumber.Equals(FBatchNumber)
                            && giftDetail.GiftTransactionNumber.Equals(FPreviouslySelectedDetailRow.GiftTransactionNumber))
                        {
                            giftDetail.DonorKey = APartnerKey;
                            giftDetail.DonorName = APartnerShortName;
                        }
                    }
                }
            }
        }

        bool FInKeyMinistryChanging = false;
        private void KeyMinistryChanged(object sender, EventArgs e)
        {
            if (FInKeyMinistryChanging || FinRecipientKeyChanging || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FInKeyMinistryChanging = true;
            try
            {
                Object val = cmbMinistry.SelectedValueCell;

                if (val != null)
                {
                    Int64 rcp = (Int64)val;

                    txtDetailRecipientKey.Text = String.Format("{0:0000000000}", rcp);
                }
            }
            finally
            {
                FInKeyMinistryChanging = false;
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

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            Decimal sum = 0;
            Decimal sumBatch = 0;

            if (FPreviouslySelectedDetailRow == null)
            {
                txtGiftTotal.Text = "";
                return;
            }

            Int32 GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

            foreach (AGiftDetailRow gdr in FMainDS.AGiftDetail.Rows)
            {
                if ((gdr.BatchNumber == FBatchNumber) && (gdr.LedgerNumber == FLedgerNumber))
                {
                    if (gdr.GiftTransactionNumber == GiftNumber)
                    {
                        if (FPreviouslySelectedDetailRow.DetailNumber == gdr.DetailNumber)
                        {
                            sum += Convert.ToDecimal(txtDetailGiftTransactionAmount.NumberValueDecimal);
                            sumBatch += Convert.ToDecimal(txtDetailGiftTransactionAmount.NumberValueDecimal);
                        }
                        else
                        {
                            sum += gdr.GiftTransactionAmount;
                            sumBatch += gdr.GiftTransactionAmount;
                        }
                    }
                    else
                    {
                        sumBatch += gdr.GiftTransactionAmount;
                    }
                }
            }

            txtGiftTotal.NumberValueDecimal = sum;
            txtGiftTotal.CurrencySymbol = txtDetailGiftTransactionAmount.CurrencySymbol;
            txtGiftTotal.ReadOnly = true;
            //this is here because at the moment the generator does not generate this
            txtBatchTotal.NumberValueDecimal = sumBatch;
            //Now we look at the batch and update the batch data
            AGiftBatchRow batch = GetBatchRow();
            batch.BatchTotal = sumBatch;
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

        private void ReverseGift(System.Object sender, System.EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please select a Gift to Reverse."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Please save first and than try again!"));
                return;
            }

            TFrmGiftRevertAdjust revertForm = new TFrmGiftRevertAdjust(this.Handle);
            //revertForm.MainDS = FMainDS;
            //revertForm.GiftDetailRow = FPreviouslySelectedDetailRow;
            revertForm.Show();
        }

        private void ReverseGiftDetail(System.Object sender, System.EventArgs e)
        {
            //TODO
        }

        private void AdjustGift(System.Object sender, System.EventArgs e)
        {
            //TODO
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

            if (BatchHasMethodOfPayment())
            {
                giftRow.MethodOfPaymentCode = GetMethodOfPaymentFromBatch();
            }

            FMainDS.AGift.Rows.Add(giftRow);

            GiftBatchTDSAGiftDetailRow newRow = FMainDS.AGiftDetail.NewRowTyped(true);
            newRow.LedgerNumber = batchRow.LedgerNumber;
            newRow.BatchNumber = batchRow.BatchNumber;
            newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
            newRow.DetailNumber = 1;
            newRow.DateEntered = giftRow.DateEntered;
            newRow.DonorKey = 0;
            newRow.CommentOneType = "Both";
            newRow.CommentTwoType = "Both";
            newRow.CommentThreeType = "Both";
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
            newRow.CommentOneType = "Both";
            newRow.CommentTwoType = "Both";
            newRow.CommentThreeType = "Both";


            return newRow;
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();

            AGiftBatchRow batchRow = GetBatchRow();

            if (batchRow != null)
            {
                txtDetailGiftTransactionAmount.CurrencySymbol = batchRow.CurrencyCode;
                txtBatchStatus.Text = batchRow.BatchStatus;
            }

            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfGivingCode, Catalog.GetString("Enter method of giving"));
            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfPaymentCode, Catalog.GetString("Enter the method of payment"));
            FPetraUtilsObject.SetStatusBarText(txtDetailReference, Catalog.GetString("Enter a reference code."));
            FPetraUtilsObject.SetStatusBarText(cmbDetailReceiptLetterCode, Catalog.GetString("Select the receipt letter code"));
        }

        private void ShowDetailsManual(AGiftDetailRow ARow)
        {
            // show cost centre
            MotivationDetailChanged(null, null);

            if (ARow == null)
            {
                return;
            }

            TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, ARow.RecipientKey);

            dtpDateEntered.Date = ((GiftBatchTDSAGiftDetailRow)ARow).DateEntered;
            txtDetailDonorKey.Text = ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey.ToString();


            UpdateControlsProtection(ARow);

            ShowDetailsForGift(ARow);

            UpdateTotals();
        }

        void ShowDetailsForGift(AGiftDetailRow ARow)
        {
            // this is a special case - normally these lines would be produced by the generator
            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if (giftRow.IsMethodOfGivingCodeNull())
            {
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfGivingCode.SetSelectedString(giftRow.MethodOfGivingCode);
            }

            if (giftRow.IsMethodOfPaymentCodeNull())
            {
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(giftRow.MethodOfPaymentCode);
            }

            if (giftRow.IsReferenceNull())
            {
                txtDetailReference.Text = String.Empty;
            }
            else
            {
                txtDetailReference.Text = giftRow.Reference;
            }

            if (giftRow.IsReceiptLetterCodeNull())
            {
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailReceiptLetterCode.SetSelectedString(giftRow.ReceiptLetterCode);
            }
        }

        /// <summary>
        /// set the currency symbols for the currency field from outside
        /// </summary>
        public void UpdateCurrencySymbols(String ACurrencyCode)
        {
            txtDetailGiftTransactionAmount.CurrencySymbol = ACurrencyCode;
            txtGiftTotal.CurrencySymbol = ACurrencyCode;
            txtBatchTotal.CurrencySymbol = ACurrencyCode;
            txtHashTotal.CurrencySymbol = ACurrencyCode;
        }

        /// <summary>
        /// set the Hash Total symbols for the currency field from outside
        /// </summary>
        public void UpdateHashTotal(Decimal AHashTotal)
        {
            txtHashTotal.NumberValueDecimal = AHashTotal;
        }

        /// <summary>
        /// update the Batch Status from outside
        /// </summary>
        public void UpdateBatchStatus(String ABatchStatus)
        {
            txtBatchStatus.Text = ABatchStatus;
        }

        /// <summary>
        /// set the correct protection from outside
        /// </summary>
        public void UpdateControlsProtection()
        {
            UpdateControlsProtection(FPreviouslySelectedDetailRow);
        }

        private void UpdateControlsProtection(AGiftDetailRow ARow)
        {
            bool firstIsEnabled = (ARow != null) && (ARow.DetailNumber == 1);

            dtpDateEntered.Enabled = firstIsEnabled;
            txtDetailDonorKey.Enabled = firstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = firstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = firstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = firstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = firstIsEnabled;
            PnlDetailsProtected = (ARow != null) && (ARow.GiftTransactionAmount < 0)
                                  && // taken from old petra
                                  (GetGiftRow(ARow.GiftTransactionNumber).ReceiptNumber != 0);

            //	&& (ARow.ReceiptNumber) This is not
        }

        private Boolean BatchHasMethodOfPayment()
        {
            String batchMop =
                GetMethodOfPaymentFromBatch();

            return batchMop != null && batchMop.Length > 0;
        }

        private String GetMethodOfPaymentFromBatch()
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
        }

        private void GetDetailDataFromControlsManual(AGiftDetailRow ARow)
        {
            ARow.CostCentreCode = txtDetailCostCentreCode.Text;

            if (ARow.DetailNumber != 1)
            {
                return;
            }

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
            giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
            giftRow.DateEntered = dtpDateEntered.Date.Value;
//
//            foreach (GiftBatchTDSAGiftDetailRow giftDetail in FMainDS.AGiftDetail.Rows)
//            {
//                if (giftDetail.BatchNumber.Equals(FBatchNumber)
//                    && giftDetail.GiftTransactionNumber.Equals(ARow.GiftTransactionNumber))
//                {
//                    giftDetail.DateEntered = giftRow.DateEntered;
//                    giftDetail.DonorKey = giftRow.DonorKey;
//                    // this does not work
//                    //giftDetail.DonorName = txtDetailDonorKey.LabelText;
//                }
//            }

            //  join by hand
            if (cmbDetailMethodOfGivingCode.SelectedIndex == -1)
            {
                giftRow.SetMethodOfGivingCodeNull();
            }
            else
            {
                giftRow.MethodOfGivingCode = cmbDetailMethodOfGivingCode.GetSelectedString();
            }

            if (cmbDetailMethodOfPaymentCode.SelectedIndex == -1)
            {
                giftRow.SetMethodOfPaymentCodeNull();
            }
            else
            {
                giftRow.MethodOfPaymentCode = cmbDetailMethodOfPaymentCode.GetSelectedString();
            }

            if (txtDetailReference.Text.Length == 0)
            {
                giftRow.SetReferenceNull();
            }
            else
            {
                giftRow.Reference = txtDetailReference.Text;
            }

            if (cmbDetailReceiptLetterCode.SelectedIndex == -1)
            {
                giftRow.SetReceiptLetterCodeNull();
            }
            else
            {
                giftRow.ReceiptLetterCode = cmbDetailReceiptLetterCode.GetSelectedString();
            }
        }
    }
}