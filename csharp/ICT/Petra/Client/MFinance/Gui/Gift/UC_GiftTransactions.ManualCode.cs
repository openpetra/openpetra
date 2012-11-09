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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        /// <summary>
        /// The current Ledger number
        /// </summary>
        public Int32 FLedgerNumber = -1;

        /// <summary>
        /// The current Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;

        private string FBatchStatus = string.Empty;
        private string FBatchMethodOfPayment = string.Empty;
        private decimal FExchangeRateToBase = 1.0m;
        private Int64 FLastDonor = -1;
        private bool FActiveOnly = true;
        private AGiftBatchRow FBatchRow = null;
        private bool FGLEffectivePeriodChanged = false;

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AFromTabClick">Indicates if called from a click on a tab or from grid doubleclick</param>
        public void LoadGifts(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            bool AFromTabClick = true)
        {
            bool firstLoad = (FLedgerNumber == -1);

            //Enable buttons accordingly
            btnDeleteDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode && !ViewMode;
            btnNewDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode && !ViewMode;
            btnNewGift.Enabled = !FPetraUtilsObject.DetailProtectedMode && !ViewMode;

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FBatchStatus == ABatchStatus))
            {
                //Same as previously selected
                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (grdDetails.SelectedRowIndex() > 0))
                {
                    if (FGLEffectivePeriodChanged)
                    {
                        FGLEffectivePeriodChanged = false;
                        GetSelectedDetailRow().DateEntered = FBatchRow.GlEffectiveDate;
                        dtpDateEntered.Date = FBatchRow.GlEffectiveDate;
                    }

                    GetDetailsFromControls(GetSelectedDetailRow());

                    if (AFromTabClick)
                    {
                        grdDetails.Focus();
                    }
                }

                UpdateControlsProtection();

                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (FExchangeRateToBase != GetBatchRow().ExchangeRateToBase))
                {
                    UpdateBaseAmount(false);
                }

                return;
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchStatus = ABatchStatus;
            FBatchRow = GetBatchRow();
            UpdateBatchStatus();

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);

            FMainDS.AGiftDetail.DefaultView.RowFilter = AGiftDetailTable.GetBatchNumberDBName() + "=" + FBatchNumber.ToString();

            // if this form is readonly, then we need all codes, because old codes might have been used
            if (firstLoad || (FActiveOnly != this.Enabled))
            {
                FActiveOnly = this.Enabled;

                TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMotivationDetailList(ref cmbDetailMotivationDetailCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, FActiveOnly);
                TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, FActiveOnly);
                TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, FActiveOnly);
                //TFinanceControls.InitialiseKeyMinList(ref cmbMinistry, (Int64)0);

                //TODO            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);
                //TODO            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
            }

            // only load from server if there are no transactions loaded yet for this batch
            // otherwise we would overwrite transactions that have already been modified
            if (FMainDS.AGiftDetail.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ALedgerNumber, ABatchNumber));
            }

            FMainDS.AGiftDetail.DefaultView.Sort = string.Format("{0} ASC, {1} ASC",
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                AGiftDetailTable.GetDetailNumberDBName());

            ShowData();
            ShowDetails();

            if (AFromTabClick)
            {
                grdDetails.Focus();
            }

            UpdateTotals();
            UpdateControlsProtection();
        }

        /// <summary>
        /// Refresh the dataset for this form
        /// </summary>
        public void RefreshAll()
        {
            if ((FMainDS != null) && (FMainDS.AGiftDetail != null))
            {
                FMainDS.AGiftDetail.Rows.Clear();
            }

            FBatchRow = GetBatchRow();

            LoadGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, FBatchRow.BatchStatus);
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

            GiftBatchTDSAGiftDetailRow giftDetailRow = GetGiftDetailRow(FPreviouslySelectedDetailRow.GiftTransactionNumber,
                FPreviouslySelectedDetailRow.DetailNumber);
            //giftDetailRow.RecipientKey = Convert.ToInt64(txtDetailRecipientKey.Text);
            giftDetailRow.RecipientDescription = APartnerShortName;  //txtDetailRecipientKey.LabelText;

            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

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
                    //
                    TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, APartnerKey);

                    long FieldNumber = Convert.ToInt64(txtField.Text);

                    txtDetailCostCentreCode.Text = TRemote.MFinance.Gift.WebConnectors.IdentifyPartnerCostCentre(FLedgerNumber, FieldNumber);
                }

                if (APartnerKey == 0)
                {
                    RetrieveMotivationDetailCostCentreCode();
                }
            }
            finally
            {
                FinRecipientKeyChanging = false;
                FPetraUtilsObject.SuppressChangeDetection = false;
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
                    PPartnerTable PartnerDT = TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey);

                    if (PartnerDT.Rows.Count > 0)
                    {
                        PPartnerRow pr = PartnerDT[0];
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

        private void DetailCommentChanged(object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            TextBox txt = (TextBox)sender;

            string txtValue = txt.Text;

            if (txtValue == String.Empty)
            {
                if (txt.Name.Contains("One"))
                {
                    if (cmbDetailCommentOneType.SelectedIndex >= 0)
                    {
                        cmbDetailCommentOneType.SelectedIndex = -1;
                    }
                }
                else if (txt.Name.Contains("Two"))
                {
                    if (cmbDetailCommentTwoType.SelectedIndex >= 0)
                    {
                        cmbDetailCommentTwoType.SelectedIndex = -1;
                    }
                }
                else if (txt.Name.Contains("Three"))
                {
                    if (cmbDetailCommentThreeType.SelectedIndex >= 0)
                    {
                        cmbDetailCommentThreeType.SelectedIndex = -1;
                    }
                }
            }
        }

        private void DetailCommentTypeChanged(object sender, EventArgs e)
        {
            //TODO This code is called from the OnLeave event because the underlying
            //    combo control does not detect a value changed when the user tabs to
            //    and clears out the contents. AWAITING FIX to remove this code

            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            TCmbAutoComplete cmb = (TCmbAutoComplete)sender;

            string cmbValue = cmb.GetSelectedString();

            if (cmbValue == String.Empty)
            {
                if (cmb.Name.Contains("One"))
                {
                    if (cmbValue != FPreviouslySelectedDetailRow.CommentOneType)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
                else if (cmb.Name.Contains("Two"))
                {
                    if (cmbValue != FPreviouslySelectedDetailRow.CommentTwoType)
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }
                else if (cmb.Name.Contains("Three"))
                {
                    if (cmbValue != FPreviouslySelectedDetailRow.CommentThreeType)
                    {
                        FPetraUtilsObject.SetChangedFlag();
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
                if (cmbMinistry.Count == 0)
                {
                    cmbMinistry.SelectedIndex = -1;
                }
                else
                {
                    Int64 rcp = cmbMinistry.GetSelectedInt64();

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

            if ((cmbDetailMotivationDetailCode.Count > 0) && (cmbDetailMotivationDetailCode.Text.Trim() == string.Empty))
            {
                cmbDetailMotivationDetailCode.SelectedIndex = 0;
            }

            RetrieveMotivationDetailAccountCode();

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                RetrieveMotivationDetailCostCentreCode();
            }
        }

        /// <summary>
        /// Called on TextChanged event for combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MotivationGroupCodeChanged(object sender, EventArgs e)
        {
            if (cmbDetailMotivationGroupCode.Text.Trim() == string.Empty)
            {
                cmbDetailMotivationGroupCode.SelectedIndex = -1;
                cmbDetailMotivationDetailCode.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Called on TextChanged event for combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MotivationDetailCodeChanged(object sender, EventArgs e)
        {
            if (cmbDetailMotivationDetailCode.Text.Trim() == string.Empty)
            {
                txtDetailAccountCode.Text = string.Empty;
            }
        }

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        private void RetrieveMotivationDetailAccountCode()
        {
            string MotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            string MotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();
            string AcctCode = string.Empty;

            AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, MotivationGroup, MotivationDetail });

            if (motivationDetail != null)
            {
                AcctCode = motivationDetail.AccountCode.ToString();
            }

            txtDetailAccountCode.Text = AcctCode;
        }

        private void RetrieveMotivationDetailCostCentreCode()
        {
            string MotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            string MotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();
            string CostCentreCode = string.Empty;

            AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, MotivationGroup, MotivationDetail });

            if (motivationDetail != null)
            {
                CostCentreCode = motivationDetail.CostCentreCode.ToString();
            }

            txtDetailCostCentreCode.Text = CostCentreCode;
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            string MotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            string MotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();

            AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, MotivationGroup, MotivationDetail });

            if (motivationDetail != null)
            {
                RetrieveMotivationDetailAccountCode();
            }

            long PartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

            if (PartnerKey == 0)
            {
                RetrieveMotivationDetailCostCentreCode();
            }
            else
            {
                TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, PartnerKey);

                long FieldNumber = Convert.ToInt64(txtField.Text);

                txtDetailCostCentreCode.Text = TRemote.MFinance.Gift.WebConnectors.IdentifyPartnerCostCentre(FLedgerNumber, FieldNumber);
            }
        }

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow != null) && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                UpdateBaseAmount(true);
            }

            UpdateTotals();
        }

        private void UpdateTotals()
        {
            Decimal sum = 0;
            Decimal sumBatch = 0;
            Int32 GiftNumber = 0;
            bool disableSaveButton = false;

            if (FPetraUtilsObject == null)
            {
                return;
            }

            //Sometimes a change in this unbound textbox causes a data changed condition
            disableSaveButton = !FPetraUtilsObject.HasChanges;

            if (FPreviouslySelectedDetailRow == null)
            {
                txtGiftTotal.Text = "";
                txtBatchTotal.NumberValueDecimal = 0;

                //If all details have been deleted
                if ((FLedgerNumber != -1) && (grdDetails.Rows.Count == 1))
                {
                    FBatchRow.BatchTotal = 0;
                }
            }
            else
            {
                GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

                foreach (AGiftDetailRow gdr in FMainDS.AGiftDetail.Rows)
                {
                    if (gdr.RowState != DataRowState.Deleted)
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
                }

                txtGiftTotal.NumberValueDecimal = sum;
                txtGiftTotal.CurrencySymbol = txtDetailGiftTransactionAmount.CurrencySymbol;
                txtGiftTotal.ReadOnly = true;
                //this is here because at the moment the generator does not generate this
                txtBatchTotal.NumberValueDecimal = sumBatch;
                //Now we look at the batch and update the batch data
                FBatchRow.BatchTotal = sumBatch;
            }

            if (disableSaveButton && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
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
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().GetCurrentBatchRow();
            //return (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
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
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private GiftBatchTDSAGiftDetailRow GetGiftDetailRow(Int32 AGiftTransactionNumber, Int32 AGiftDetailNumber)
        {
            return (GiftBatchTDSAGiftDetailRow)FMainDS.AGiftDetail.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, AGiftTransactionNumber,
                                                                                            AGiftDetailNumber });
        }

        /// <summary>
        /// delete a gift detail, and if it is the last detail, delete the whole gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDetail(System.Object sender, EventArgs e)
        {
            DeleteAGiftDetail();
        }

        AGiftRow FGift = null;
        string FFilterAllDetailsOfGift = string.Empty;
        DataView FGiftDetailView = null;

        private bool PreDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            FGift = GetGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                AGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.AGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;

            if (FGiftDetailView.Count == 1)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete transaction {1} from Gift Batch no. {2}?" +
                        "\n\r\n\r" + "     From:  {3}" +
                        "\n\r" + "         To:  {4}" +
                        "\n\r" + "Amount:  {5}"),
                    ARowToDelete.DetailNumber,
                    ARowToDelete.GiftTransactionNumber,
                    ARowToDelete.BatchNumber,
                    ARowToDelete.DonorName,
                    ARowToDelete.RecipientDescription,
                    ARowToDelete.GiftTransactionAmount.ToString("C"));
            }
            else if (FGiftDetailView.Count > 1)
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Are you sure you want to delete detail {0} from transaction {1} in Gift Batch no. {2}?" +
                            "\n\r\n\r" + "     From:  {3}" +
                            "\n\r" + "         To:  {4}" +
                            "\n\r" + "Amount:  {5}"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber,
                        ARowToDelete.DonorName,
                        ARowToDelete.RecipientDescription,
                        ARowToDelete.GiftTransactionAmount.ToString("C"));
            }
            else //this should never happen
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Gift transaction {1} in Gift Batch no. {2} has no detail rows in the Gift Detail table!"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                allowDeletion = false;
            }

            return allowDeletion;
        }

        private bool DeleteRowManual(GiftBatchTDSAGiftDetailRow ARowToDelete, out string ACompletionMessage)
        {
            bool deleteSuccessful = false;

            ACompletionMessage = string.Empty;

            int selectedDetailNumber = FPreviouslySelectedDetailRow.DetailNumber;

            try
            {
                FPreviouslySelectedDetailRow.Delete();
                FPreviouslySelectedDetailRow = null;

                if (FGiftDetailView.Count == 0)
                {
                    // TODO int oldGiftNumber = gift.GiftTransactionNumber;
                    // TODO int oldBatchNumber = gift.BatchNumber;

                    FGift.Delete();

                    // TODO we cannot update primary keys easily, therefore we have to do it later on the server side
//#if DISABLED
//	                string filterAllDetailsOfBatch = String.Format("{0}={1}",
//	                    AGiftDetailTable.GetBatchNumberDBName(),
//	                    oldBatchNumber);
//
//	                giftDetailView.RowFilter = filterAllDetailsOfBatch;
//
//	                foreach (DataRowView rv in giftDetailView)
//	                {
//	                    GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;
//
//	                    if (row.GiftTransactionNumber > oldGiftNumber)
//	                    {
//	                        row.GiftTransactionNumber--;
//	                    }
//	                }
//	                GetBatchRow().LastGiftNumber--;
//#endif
                }
                else
                {
                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        if (row.DetailNumber > selectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;
                }

                ACompletionMessage = Catalog.GetString("Gift row deleted successfully!");
                deleteSuccessful = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Error in trying to delete the current gift!" + "\n\r\n\r" + "Error: {0}"),
                        ex.Message),
                    "Delete Row Error");
            }

            return deleteSuccessful;
        }

        private void PostDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            MessageBox.Show(ACompletionMessage);

            if (!pnlDetails.Enabled)
            {
                ClearControls();
            }
        }

        private void ClearControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                txtDetailDonorKey.Text = string.Empty;
                txtDetailReference.Clear();
                dtpDateEntered.Clear();
                txtGiftTotal.NumberValueDecimal = 0;
                txtDetailGiftTransactionAmount.NumberValueDecimal = 0;
                txtDetailRecipientKey.Text = string.Empty;
                txtField.Text = string.Empty;
                txtDetailAccountCode.Clear();
                txtDetailGiftCommentOne.Clear();
                txtDetailGiftCommentTwo.Clear();
                txtDetailGiftCommentThree.Clear();
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
                cmbDetailMotivationGroupCode.SelectedIndex = -1;
                cmbDetailMotivationDetailCode.SelectedIndex = -1;
                cmbDetailCommentOneType.SelectedIndex = -1;
                cmbDetailCommentTwoType.SelectedIndex = -1;
                cmbDetailCommentThreeType.SelectedIndex = -1;
                cmbDetailMailingCode.SelectedIndex = -1;
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
                cmbMinistry.SelectedIndex = -1;
                txtDetailCostCentreCode.Text = string.Empty;
            }
            finally
            {
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
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
            AGiftDetailRow GiftDetailRow = NewGift(); // returns AGiftDetailRow

            if (GiftDetailRow != null)
            {
                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);
            }
        }

        private void ReverseGift(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("ReverseGift");
        }

        /// <summary>
        /// show the form for the gift reversal/adjustment
        /// </summary>
        /// <param name="functionname">Which function shall be called on the server</param>
        public void ShowRevertAdjustForm(String functionname)
        {
            AGiftBatchRow giftBatch = ((TFrmGiftBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();

            if (giftBatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift Batch."));
                return;
            }

            if (!giftBatch.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED))
            {
                MessageBox.Show(Catalog.GetString("This function is only possible when the selected batch is already posted."));
                return;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift to Reverse."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Please save first and than try again!"));
                return;
            }

            TFrmGiftRevertAdjust revertForm = new TFrmGiftRevertAdjust(FPetraUtilsObject.GetForm());
            try
            {
                ParentForm.ShowInTaskbar = false;
                revertForm.LedgerNumber = FLedgerNumber;
                revertForm.AddParam("Function", functionname);
                revertForm.GiftDetailRow = FPreviouslySelectedDetailRow;

                if (revertForm.ShowDialog() == DialogResult.OK)
                {
                    ((TFrmGiftBatch)ParentForm).RefreshAll();
                }
            }
            finally
            {
                revertForm.Dispose();
                ParentForm.ShowInTaskbar = true;
            }
        }

        private void ReverseGiftDetail(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("ReverseGiftDetail");
        }

        private void AdjustGift(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("AdjustGift");
        }

        /// <summary>
        /// add a new gift detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            //If grid is empty call NewGift() instead
            if (grdDetails.Rows.Count == 1)
            {
                NewGift(sender, e);
                return;
            }

            // this is coded manually, to use the correct gift record
            // we create the table locally, no dataset
            AGiftDetailRow GiftDetailRow = NewGiftDetail((GiftBatchTDSAGiftDetailRow)FPreviouslySelectedDetailRow); // returns AGiftDetailRow

            if (GiftDetailRow != null)
            {
                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);
                //int newRowIndex = FMainDS.AGiftDetail.Rows.Count - 1;

                //SelectDetailRowByDataTableIndex(newRowIndex);
                //InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

                //FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                //ShowDetails(FPreviouslySelectedDetailRow);

                //GetDetailsFromControls(FPreviouslySelectedDetailRow, true);

                ////Need to redo this just in case the sorting is not on primary key
                //SelectDetailRowByDataTableIndex(newRowIndex);

                RetrieveMotivationDetailAccountCode();
                txtDetailGiftTransactionAmount.Focus();
            }
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the batch.lastTransactionNumber is updated
        /// </summary>
        private AGiftDetailRow NewGift()
        {
            GiftBatchTDSAGiftDetailRow newRow = null;

            if (ValidateAllData(true, true))
            {
                AGiftRow giftRow = FMainDS.AGift.NewRowTyped(true);

                giftRow.DateEntered = FBatchRow.GlEffectiveDate;
                giftRow.LedgerNumber = FBatchRow.LedgerNumber;
                giftRow.BatchNumber = FBatchRow.BatchNumber;
                giftRow.GiftTransactionNumber = FBatchRow.LastGiftNumber + 1;
                giftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                FBatchRow.LastGiftNumber++;
                giftRow.LastDetailNumber = 1;

                FMainDS.AGift.Rows.Add(giftRow);

                newRow = FMainDS.AGiftDetail.NewRowTyped(true);
                newRow.LedgerNumber = FBatchRow.LedgerNumber;
                newRow.BatchNumber = FBatchRow.BatchNumber;
                newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
                newRow.DetailNumber = 1;
                newRow.DateEntered = giftRow.DateEntered;
                newRow.DonorKey = 0;
                cmbDetailMotivationGroupCode.SelectedIndex = 0;
                newRow.MotivationGroupCode = cmbDetailMotivationGroupCode.GetSelectedString();
                newRow.MotivationDetailCode = cmbDetailMotivationDetailCode.GetSelectedString();
                FMainDS.AGiftDetail.Rows.Add(newRow);
            }

            return newRow;
        }

        /// <summary>
        /// add another gift detail to an existing gift
        /// </summary>
        private AGiftDetailRow NewGiftDetail(GiftBatchTDSAGiftDetailRow ACurrentRow)
        {
            GiftBatchTDSAGiftDetailRow newRow = null;

            if (ValidateAllData(true, true))
            {
                if (ACurrentRow == null)
                {
                    return NewGift();
                }

                // find gift row
                AGiftRow giftRow = GetGiftRow(ACurrentRow.GiftTransactionNumber);

                giftRow.LastDetailNumber++;

                newRow = FMainDS.AGiftDetail.NewRowTyped(true);
                newRow.LedgerNumber = giftRow.LedgerNumber;
                newRow.BatchNumber = giftRow.BatchNumber;
                newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
                newRow.DetailNumber = giftRow.LastDetailNumber;
                newRow.MethodOfPaymentCode = giftRow.MethodOfPaymentCode;
                newRow.MethodOfGivingCode = giftRow.MethodOfGivingCode;
                newRow.DonorKey = ACurrentRow.DonorKey;
                newRow.DonorName = ACurrentRow.DonorName;
                newRow.DateEntered = ACurrentRow.DateEntered;
                cmbDetailMotivationGroupCode.SelectedIndex = 0;
                newRow.MotivationGroupCode = cmbDetailMotivationGroupCode.GetSelectedString();
                newRow.MotivationDetailCode = cmbDetailMotivationDetailCode.GetSelectedString();
                FMainDS.AGiftDetail.Rows.Add(newRow);
            }

            return newRow;
        }

        /// <summary>
        /// show ledger and batch number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();

//            AGiftBatchRow FBatchRow = GetBatchRow();

            if (FBatchRow != null)
            {
                txtDetailGiftTransactionAmount.CurrencySymbol = FBatchRow.CurrencyCode;
            }

            if (grdDetails.Rows.Count == 1)
            {
                txtBatchTotal.NumberValueDecimal = 0;
                ClearControls();
            }
            else
            {
                AGiftDetailRow ARow = (AGiftDetailRow)FMainDS.AGiftDetail.Rows[0];
                cmbDetailMotivationGroupCode.SetSelectedString(ARow.MotivationGroupCode);
                cmbDetailMotivationDetailCode.SetSelectedString(ARow.MotivationDetailCode);
                UpdateControlsProtection(ARow);
            }

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                txtDetailCostCentreCode.Text = string.Empty;
            }

            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfGivingCode, Catalog.GetString("Enter method of giving"));
            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfPaymentCode, Catalog.GetString("Enter the method of payment"));
            FPetraUtilsObject.SetStatusBarText(txtDetailReference, Catalog.GetString("Enter a reference code."));
            FPetraUtilsObject.SetStatusBarText(cmbDetailReceiptLetterCode, Catalog.GetString("Select the receipt letter code"));
        }

        private void ShowDetailsManual(AGiftDetailRow ARow)
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();

            if (ARow == null)
            {
                return;
            }

            // show cost centre
            MotivationDetailChanged(null, null);

            TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, ARow.RecipientKey);

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
            dtpDateEntered.Date = giftRow.DateEntered;

            cmbDetailMethodOfPaymentCode.SetSelectedString(giftRow.MethodOfPaymentCode);

            if (((GiftBatchTDSAGiftDetailRow)ARow).IsDonorKeyNull())
            {
                txtDetailDonorKey.Text = "0";
            }
            else
            {
                txtDetailDonorKey.Text = ((GiftBatchTDSAGiftDetailRow)ARow).DonorKey.ToString();
            }

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                txtDetailCostCentreCode.Text = string.Empty;
            }

            UpdateControlsProtection(ARow);

            ShowDetailsForGift(ARow);
        }

        private void ShowDetailsForGift(AGiftDetailRow ARow)
        {
            // this is a special case - normally these lines would be produced by the generator
            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if (cmbMinistry.Count == 0)
            {
                cmbMinistry.SelectedIndex = -1;
                cmbMinistry.Text = string.Empty;
            }

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
        /// update the transaction method payment from outside
        /// </summary>
        public void UpdateMethodOfPayment(bool ACalledLocally)
        {
            Int32 ledgerNumber;
            Int32 batchNumber;

            if (ACalledLocally)
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment);
                return;
            }

            if (!((TFrmGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().FSelectedBatchMethodOfPayment;

            ledgerNumber = FBatchRow.LedgerNumber;
            batchNumber = FBatchRow.BatchNumber;

            if (FMainDS.AGift.Rows.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ledgerNumber, batchNumber));
            }
            else if ((FLedgerNumber == ledgerNumber) || (FBatchNumber == batchNumber))
            {
                //Rows already active in transaction tab. Need to set current row ac code below will not update selected row
                if (FPreviouslySelectedDetailRow != null)
                {
                    FPreviouslySelectedDetailRow.MethodOfPaymentCode = FBatchMethodOfPayment;
                    cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment);
                }
            }

            //Update all transactions
            foreach (AGiftRow giftRow in FMainDS.AGift.Rows)
            {
                if (giftRow.BatchNumber.Equals(batchNumber) && giftRow.LedgerNumber.Equals(ledgerNumber)
                    && (giftRow.MethodOfPaymentCode != FBatchMethodOfPayment))
                {
                    giftRow.MethodOfPaymentCode = FBatchMethodOfPayment;
                }
            }
        }

        /// <summary>
        /// update the transaction method payment from outside
        /// </summary>
        /// <param name="ABatchRow"></param>
        public void UpdateDateEntered(AGiftBatchRow ABatchRow)
        {
            Int32 ledgerNumber;
            Int32 batchNumber;
            DateTime batchEffectiveDate;

            ledgerNumber = ABatchRow.LedgerNumber;
            batchNumber = ABatchRow.BatchNumber;
            batchEffectiveDate = ABatchRow.GlEffectiveDate;

            if (FMainDS.AGift.Rows.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ledgerNumber, batchNumber));
            }
            else if ((FLedgerNumber == ledgerNumber) || (FBatchNumber == batchNumber))
            {
                FGLEffectivePeriodChanged = true;
                //Rows already active in transaction tab. Need to set current row ac code below will not update selected row
                GetSelectedDetailRow().DateEntered = batchEffectiveDate;
            }

            //Update all transactions
            foreach (AGiftRow giftRow in FMainDS.AGift.Rows)
            {
                if (giftRow.BatchNumber.Equals(batchNumber) && giftRow.LedgerNumber.Equals(ledgerNumber))
                {
                    giftRow.DateEntered = batchEffectiveDate;
                }
            }

            if (FGLEffectivePeriodChanged)
            {
                ShowDetails();
            }
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
        public void UpdateBatchStatus()
        {
            //Sometimes a change in this unbound textbox causes a data changed condition
            bool disableSave = !FPetraUtilsObject.HasChanges;

            txtBatchStatus.Text = FBatchStatus;

            if (disableSave && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        /// <summary>
        /// set the correct protection from outside
        /// </summary>
        public void UpdateControlsProtection()
        {
            UpdateControlsProtection(FPreviouslySelectedDetailRow);
        }

        /// <summary>
        /// update the transaction base amount calculation from outside
        /// </summary>
        public void UpdateBaseAmount(bool AUpdateCurrentRowOnly)
        {
            Int32 ledgerNumber;
            Int32 batchNumber;

            if ((((TFrmGiftBatch)ParentForm).GetBatchControl().GetCurrentBatchRow() == null) || (FLedgerNumber == -1)
                || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            if (AUpdateCurrentRowOnly)
            {
                //This happens when the gift amount is updated
                if (FExchangeRateToBase != GetBatchRow().ExchangeRateToBase)
                {
                    FExchangeRateToBase = GetBatchRow().ExchangeRateToBase;
                }

                FPreviouslySelectedDetailRow.GiftAmount = FPreviouslySelectedDetailRow.GiftTransactionAmount * FExchangeRateToBase;
                return;
            }

            if (!((TFrmGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            if (FExchangeRateToBase != FBatchRow.ExchangeRateToBase)
            {
                FExchangeRateToBase = FBatchRow.ExchangeRateToBase;
            }
            else
            {
                return;
            }

            ledgerNumber = FBatchRow.LedgerNumber;
            batchNumber = FBatchRow.BatchNumber;

            if (FMainDS.AGift.Rows.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ledgerNumber, batchNumber));
            }
            else if ((FLedgerNumber == ledgerNumber) || (FBatchNumber == batchNumber))
            {
                //Rows already active in transaction tab. Need to set current row ac code below will not update selected row
                if (FPreviouslySelectedDetailRow != null)
                {
                    FPreviouslySelectedDetailRow.GiftAmount = FPreviouslySelectedDetailRow.GiftTransactionAmount * FExchangeRateToBase;
                }
            }

            //Update all transactions
            foreach (AGiftDetailRow gdr in FMainDS.AGiftDetail.Rows)
            {
                gdr.GiftAmount = gdr.GiftTransactionAmount * FExchangeRateToBase;
            }
        }

        private void UpdateControlsProtection(AGiftDetailRow ARow)
        {
            bool firstIsEnabled = (ARow != null) && (ARow.DetailNumber == 1) && !ViewMode;

            dtpDateEntered.Enabled = firstIsEnabled;
            txtDetailDonorKey.Enabled = firstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = firstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = firstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = firstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = firstIsEnabled;

            if (FBatchRow == null)
            {
                FBatchRow = GetBatchRow();
            }

            if (ARow == null)
            {
                PnlDetailsProtected = true;
            }
            else
            {
                PnlDetailsProtected = (ViewMode
                                       || ((ARow != null) && (ARow.GiftTransactionAmount < 0)
                                           && (GetGiftRow(ARow.GiftTransactionNumber).ReceiptNumber != 0))
                                       || FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED
                                       );    // taken from old petra
            }

            pnlDetails.Enabled = !(PnlDetailsProtected);
        }

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewMode;
            }
        }
        private Boolean BatchHasMethodOfPayment()
        {
            String batchMop = GetMethodOfPaymentFromBatch();

            return batchMop != null && batchMop.Length > 0;
        }

        private String GetMethodOfPaymentFromBatch()
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
        }

        private void GiftDateChanged(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                DateTime dateValue;

                string aDate = dtpDateEntered.Date.ToString();

                if (!DateTime.TryParse(aDate, out dateValue))
                {
                    dtpDateEntered.Date = FBatchRow.GlEffectiveDate;
                }
            }
            catch
            {
                //Do nothing
            }
        }

        private void GetDetailDataFromControlsManual(AGiftDetailRow ARow)
        {
            ARow.CostCentreCode = txtDetailCostCentreCode.Text;

            if (ARow.DetailNumber != 1)
            {
                return;
            }

            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if (giftRow != null)
            {
                giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
                giftRow.DateEntered = (dtpDateEntered.Date.HasValue ? dtpDateEntered.Date.Value : FBatchRow.GlEffectiveDate);

                GiftBatchTDSAGiftDetailRow giftDetailRow = GetGiftDetailRow(ARow.GiftTransactionNumber, ARow.DetailNumber);
                giftDetailRow.RecipientKey = Convert.ToInt64(txtDetailRecipientKey.Text);
                giftDetailRow.RecipientDescription = txtDetailRecipientKey.LabelText;

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

        /// Select a special gift detail number from outside
        public void SelectGiftDetailNumber(Int32 AGiftNumber, Int32 AGiftDetailNumber)
        {
            DataView myView = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView;

            for (int counter = 0; (counter < myView.Count); counter++)
            {
                int myViewGiftNumber = (int)myView[counter][2];
                int myViewGiftDetailNumber = (int)(int)myView[counter][3];

                if ((myViewGiftNumber == AGiftNumber) && (myViewGiftDetailNumber == AGiftDetailNumber))
                {
                    grdDetails.Selection.ResetSelection(false);
                    grdDetails.Selection.SelectRow(counter + 1, true);
                    // scroll to the row
                    grdDetails.ShowCell(new SourceGrid.Position(counter + 1, 0), true);

                    FocusedRowChanged(this, new SourceGrid.RowEventArgs(counter + 1));
                    break;
                }
            }
        }

        private void ValidateDataDetailsManual(AGiftDetailRow ARow)
        {
            if ((ARow == null) || (GetBatchRow() == null) || (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateGiftDetailManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            TSharedFinanceValidation_Gift.ValidateGiftManual(this,
                giftRow,
                FBatchRow.BatchYear,
                FBatchRow.BatchPeriod,
                dtpDateEntered,
                ref VerificationResultCollection,
                FValidationControlsDict);
        }
    }
}