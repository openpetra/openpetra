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
        private bool FGiftSelectedForDeletion = false;
        AGiftRow FGift = null;
        string FFilterAllDetailsOfGift = string.Empty;
        DataView FGiftDetailView = null;

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewMode;
            }
        }

        private void InitialiseControls()
        {
            //Fix to length of field
            txtDetailReference.MaxLength = 20;

            //Fix a layering issue
            txtField.SendToBack();

            //Changing this will stop taborder issues
            sptTransactions.TabStop = false;

            txtDetailRecipientKey.PartnerClass = "WORKER,UNIT,FAMILY";
        }

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        public void LoadGifts(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            Console.WriteLine("LoadGifts");
            DateTime dtStart = DateTime.Now;

            bool firstLoad = (FLedgerNumber == -1);

            if (firstLoad)
            {
                InitialiseControls();
            }

            //Enable buttons accordingly
            btnDelete.Enabled = (!FPetraUtilsObject.DetailProtectedMode && !ViewMode && ABatchStatus == MFinanceConstants.BATCH_UNPOSTED);
            btnDeleteAll.Enabled = (!FPetraUtilsObject.DetailProtectedMode && !ViewMode && ABatchStatus == MFinanceConstants.BATCH_UNPOSTED);
            btnNewDetail.Enabled = (!FPetraUtilsObject.DetailProtectedMode && !ViewMode && ABatchStatus == MFinanceConstants.BATCH_UNPOSTED);
            btnNewGift.Enabled = (!FPetraUtilsObject.DetailProtectedMode && !ViewMode && ABatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FBatchStatus == ABatchStatus))
            {
                //Same as previously selected
                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (GetSelectedRowIndex() > 0))
                {
                    if (FGLEffectivePeriodChanged)
                    {
                        FGLEffectivePeriodChanged = false;
                        GetSelectedDetailRow().DateEntered = FBatchRow.GlEffectiveDate;
                        dtpDateEntered.Date = FBatchRow.GlEffectiveDate;
                    }

                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (FExchangeRateToBase != GetBatchRow().ExchangeRateToBase))
                {
                    UpdateBaseAmount(false);
                }

                Console.WriteLine("LoadGifts - Quick exit  {0} ms", (DateTime.Now - dtStart).TotalMilliseconds);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            SuspendLayout();

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchStatus = ABatchStatus;
            FBatchRow = GetBatchRow();
            UpdateBatchStatus();

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;

            // if this form is readonly, then we need all codes, because old codes might have been used
            if (firstLoad || (FActiveOnly != this.Enabled))
            {
                FActiveOnly = this.Enabled;

                Console.WriteLine("Populating ComboBoxes  {0} ms", ((DateTime.Now - dtStart).TotalMilliseconds));
                TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMotivationDetailList(ref cmbDetailMotivationDetailCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, FActiveOnly);
                TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, FActiveOnly);
                TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, FActiveOnly);
                //TFinanceControls.InitialiseKeyMinList(ref cmbMinistry, (Int64)0);

                //TODO            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);
                //TODO            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
            }

            SetGiftDetailDefaultView();

            // only load from server if there are no transactions loaded yet for this batch
            // otherwise we would overwrite transactions that have already been modified
            if (FMainDS.AGiftDetail.DefaultView.Count == 0)
            {
                Console.WriteLine("Loading transactions...  {0}", ((DateTime.Now - dtStart).TotalMilliseconds));
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ALedgerNumber, ABatchNumber));
            }

            ShowData();
            //ShowDetails();

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);

            SelectRowInGrid(1);

            UpdateRecordNumberDisplay();
            UpdateTotals();
            UpdateControlsProtection();

            ResumeLayout();
            Console.WriteLine("LoadGifts completed  {0}", ((DateTime.Now - dtStart).TotalMilliseconds));
            this.Cursor = Cursors.Default;
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
            giftDetailRow.RecipientDescription = APartnerShortName;

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

            if ((txtDetailRecipientKey.Text == string.Empty) || (Convert.ToInt64(txtDetailRecipientKey.Text) == 0))
            {
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
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
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if (txn.NumberValueDecimal == null)
            {
                return;
            }

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
                txtGiftTotal.NumberValueDecimal = 0;
                txtBatchTotal.NumberValueDecimal = 0;

                //If all details have been deleted
                if ((FLedgerNumber != -1) && (grdDetails.Rows.Count == 1))
                {
                    ((TFrmGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
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

//christiank: when you have a moFBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED &&
                txtGiftTotal.NumberValueDecimal = sum;
                txtGiftTotal.CurrencyCode = txtDetailGiftTransactionAmount.CurrencyCode;
                txtGiftTotal.ReadOnly = true;
                //this is here because at the moment the generator does not generate this
                txtBatchTotal.NumberValueDecimal = sumBatch;
                //Now we look at the batch and update the batch data
                ((TFrmGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(sumBatch, FBatchRow.BatchNumber);
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
            FGiftDetailView.Sort = AGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftTransactionAmount, GetBatchRow().CurrencyCode);

            if (FGiftDetailView.Count == 1)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete gift no. {1} from Gift Batch no. {2}?" +
                        "\n\r\n\r" + "     From:  {3}" +
                        "\n\r" + "         To:  {4}" +
                        "\n\r" + "Amount:  {5}"),
                    ARowToDelete.DetailNumber,
                    ARowToDelete.GiftTransactionNumber,
                    ARowToDelete.BatchNumber,
                    ARowToDelete.DonorName,
                    ARowToDelete.RecipientDescription,
                    formattedDetailAmount);
            }
            else if (FGiftDetailView.Count > 1)
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Are you sure you want to delete detail line: {0} from gift no. {1} in Gift Batch no. {2}?" +
                            "\n\r\n\r" + "     From:  {3}" +
                            "\n\r" + "         To:  {4}" +
                            "\n\r" + "Amount:  {5}"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber,
                        ARowToDelete.DonorName,
                        ARowToDelete.RecipientDescription,
                        formattedDetailAmount);
            }
            else //this should never happen
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Gift gift no. {1} in Batch no. {2} has no detail rows in the Gift Detail table!"),
                        ARowToDelete.DetailNumber,
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                allowDeletion = false;
            }

            return allowDeletion;
        }

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
            string completionMessage = string.Empty;

            if ((FPreviouslySelectedDetailRow == null) || (FBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                ||
                (MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all gifts from batch ({0}).\n\nDo you really want to delete all?"),
                         FBatchNumber),
                     Catalog.GetString("Confirm Delete All"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                    completionMessage = String.Format(Catalog.GetString("All gifts and details cancelled successfully."),
                        FPreviouslySelectedDetailRow.BatchNumber);

                    //Load all journals for current Batch
                    //clear any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection();

                    //Clear gifts and details etc for current Batch
                    FMainDS.AGiftDetail.Clear();
                    FMainDS.AGift.Clear();

                    //Load tables afresh
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(FLedgerNumber, FBatchNumber));

                    //Delete gift details
                    for (int i = FMainDS.AGiftDetail.Count - 1; i >= 0; i--)
                    {
                        FMainDS.AGiftDetail[i].Delete();
                    }

                    //Delete gifts
                    for (int i = FMainDS.AGift.Count - 1; i >= 0; i--)
                    {
                        FMainDS.AGift[i].Delete();
                    }

                    FBatchRow.BatchTotal = 0;

                    FPetraUtilsObject.HasChanges = true;

                    // save first, then post
                    if (!((TFrmGiftBatch)ParentForm).SaveChanges())
                    {
                        SelectRowInGrid(1);

                        // saving failed, therefore do not try to cancel
                        MessageBox.Show(Catalog.GetString("The emptied batch failed to save!"));
                    }
                    else
                    {
                        MessageBox.Show(completionMessage,
                            "All Gifts Deleted.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    completionMessage = ex.Message;
                    MessageBox.Show(ex.Message,
                        "Deletion Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    //Return FMainDS to original state
                    FMainDS.RejectChanges();
                }
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                UpdateControlsProtection();
            }

            UpdateRecordNumberDisplay();
        }

        private bool DeleteRowManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;
            string originatingDetailRef = string.Empty;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return deletionSuccessful;
            }

            if ((ARowToDelete.RowState != DataRowState.Added) && !((TFrmGiftBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current gift detail!");
                return deletionSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();

            int selectedDetailNumber = ARowToDelete.DetailNumber;
            int giftToDeleteTransNo = 0;
            int currentBatchNumber = 0;
            string filterAllGiftsOfBatch = String.Empty;
            string filterAllGiftDetailsOfBatch = String.Empty;

            try
            {
                if (ARowToDelete.ModifiedDetailKey != null)
                {
                    originatingDetailRef = ARowToDelete.ModifiedDetailKey;
                }

                //If deleting a detail row as opposed to a gift header
                if (FGiftDetailView.Count > 1)
                {
                    ARowToDelete.Delete();

                    FGiftSelectedForDeletion = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        if (row.DetailNumber > selectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;

                    FPetraUtilsObject.SetChangedFlag();
                }
                else
                {
                    ARowToDelete.Delete();

                    giftToDeleteTransNo = FGift.GiftTransactionNumber;
                    currentBatchNumber = FGift.BatchNumber;

                    filterAllGiftDetailsOfBatch = String.Format("{0}={1}",
                        AGiftDetailTable.GetBatchNumberDBName(),
                        currentBatchNumber);

                    DataView giftDetailView = new DataView(FMainDS.AGiftDetail);
                    giftDetailView.RowFilter = filterAllGiftDetailsOfBatch;

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        if (row.GiftTransactionNumber > giftToDeleteTransNo)
                        {
                            row.GiftTransactionNumber--;
                        }
                    }

                    filterAllGiftsOfBatch = String.Format("{0}={1}",
                        AGiftTable.GetBatchNumberDBName(),
                        currentBatchNumber);

                    DataView giftView = new DataView(FMainDS.AGift);
                    giftView.RowFilter = filterAllGiftsOfBatch;
                    giftView.Sort = AGiftTable.GetGiftTransactionNumberDBName();

                    AGiftRow giftRowToReceive = null;
                    AGiftRow giftRowToCopyDown = null;

                    AGiftRow giftRowCurrent = null;

                    int currentGiftTransNo = 0;

                    foreach (DataRowView gv in giftView)
                    {
                        giftRowCurrent = (AGiftRow)gv.Row;

                        currentGiftTransNo = giftRowCurrent.GiftTransactionNumber;

                        if (currentGiftTransNo > giftToDeleteTransNo)
                        {
                            giftRowToCopyDown = giftRowCurrent;

                            //Copy column values down
                            for (int j = 3; j < giftRowToCopyDown.Table.Columns.Count; j++)
                            {
                                //Update all columns except the pk fields that remain the same
                                giftRowToReceive[j] = giftRowToCopyDown[j];
                            }
                        }

                        if (currentGiftTransNo == giftView.Count)
                        {
                            //Mark last record for deletion
                            giftRowCurrent.GiftStatus = MFinanceConstants.MARKED_FOR_DELETION;
                        }

                        //Will always be previous row
                        giftRowToReceive = giftRowCurrent;
                    }

                    FPreviouslySelectedDetailRow = null;

                    FPetraUtilsObject.SetChangedFlag();

                    FGiftSelectedForDeletion = true;
                }

                //Check if deleting a reversed gift detail
                if (originatingDetailRef.StartsWith("|"))
                {
                    bool ok = TRemote.MFinance.Gift.WebConnectors.ReversedGiftReset(FLedgerNumber, originatingDetailRef);

                    if (!ok)
                    {
                        throw new Exception("Error in trying to reset Modified Detail field of the originating gift detail.");
                    }
                }

                //Try to save changes
                if (((TFrmGiftBatch) this.ParentForm).SaveChanges())
                {
                    //Reload from server
                    FMainDS.AGiftDetail.Clear();
                    FMainDS.AGift.Clear();

                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(FLedgerNumber, FBatchNumber));
                }
                else
                {
                    throw new Exception("Unable to save after deleting a gift!");
                }

                ACompletionMessage = Catalog.GetString("Gift row deleted successfully!");

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Gift Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS = (GiftBatchTDS)FTempDS.Copy();
            }
            finally
            {
                SetGiftDetailDefaultView();
            }

            UpdateRecordNumberDisplay();

            return deletionSuccessful;
        }

        private void PostDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                if (FGiftSelectedForDeletion)
                {
                    FGiftSelectedForDeletion = false;

                    SetBatchLastGiftNumber();

                    UpdateControlsProtection();

                    if (!pnlDetails.Enabled)
                    {
                        ClearControls();
                    }
                }

                UpdateTotals();

                ((TFrmGiftBatch) this.ParentForm).SaveChanges();

                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (!AAllowDeletion && (ACompletionMessage.Length > 0))
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SetBatchLastGiftNumber()
        {
            DataView dv = new DataView(FMainDS.AGift);

            dv.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                FBatchNumber);

            dv.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            if (dv.Count > 0)
            {
                AGiftRow transRow = (AGiftRow)dv[0].Row;
                FBatchRow.LastGiftNumber = transRow.GiftTransactionNumber;
            }
            else
            {
                FBatchRow.LastGiftNumber = 0;
            }
        }

        private void ClearGiftDetailDefaultView()
        {
            FFilterPanelControls.SetBaseFilter(String.Empty, true);
        }

        private void SetGiftDetailDefaultView()
        {
            if (FBatchNumber != -1)
            {
                ClearGiftDetailDefaultView();

                string rowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
                FFilterPanelControls.SetBaseFilter(rowFilter, true);

                FMainDS.AGiftDetail.DefaultView.Sort = string.Format("{0} DESC, {1} ASC",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                if (grdDetails.DataSource != null)
                {
                    ApplyFilter();
                    UpdateRecordNumberDisplay();
                    SetRecordNumberDisplayProperties();
                }
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

            UpdateRecordNumberDisplay();
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
                RetrieveMotivationDetailCostCentreCode();
                newRow.CostCentreCode = txtDetailCostCentreCode.Text;

                FMainDS.AGiftDetail.Rows.Add(newRow);
            }

            return newRow;
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
            AGiftDetailRow giftDetailRow = NewGiftDetail((GiftBatchTDSAGiftDetailRow)FPreviouslySelectedDetailRow);

            if (giftDetailRow != null)
            {
                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);

                RetrieveMotivationDetailAccountCode();
                txtDetailRecipientKey.Focus();
            }

            UpdateRecordNumberDisplay();
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
                newRow.DateEntered = giftRow.DateEntered;
                cmbDetailMotivationGroupCode.SelectedIndex = 0;
                newRow.MotivationGroupCode = cmbDetailMotivationGroupCode.GetSelectedString();
                newRow.MotivationDetailCode = cmbDetailMotivationDetailCode.GetSelectedString();
                RetrieveMotivationDetailCostCentreCode();
                newRow.CostCentreCode = txtDetailCostCentreCode.Text;

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

            if (FBatchRow != null)
            {
                txtDetailGiftTransactionAmount.CurrencyCode = FBatchRow.CurrencyCode;
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

            if ((Convert.ToInt64(txtDetailRecipientKey.Text) == 0) && (cmbDetailMotivationGroupCode.SelectedIndex == -1))
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

            if ((Convert.ToInt64(txtDetailRecipientKey.Text) == 0) && (cmbDetailMotivationGroupCode.SelectedIndex == -1))
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
            txtDetailGiftTransactionAmount.CurrencyCode = ACurrencyCode;
            txtGiftTotal.CurrencyCode = ACurrencyCode;
            txtBatchTotal.CurrencyCode = ACurrencyCode;
            txtHashTotal.CurrencyCode = ACurrencyCode;
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
        /// set the Hash Total symbols for the currency field from outside
        /// </summary>
        public void UpdateHashTotal(Decimal AHashTotal)
        {
            txtHashTotal.NumberValueDecimal = AHashTotal;
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

            btnDelete.Enabled = ((grdDetails.Rows.Count > 1) && !PnlDetailsProtected);
            btnDeleteAll.Enabled = ((grdDetails.Rows.Count > 1) && !PnlDetailsProtected);
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

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }

        /// <summary>
        /// Refresh the dataset for this form
        /// </summary>
        public void RefreshAll()
        {
            Console.WriteLine("RefreshAll()");

            if ((FMainDS != null) && (FMainDS.AGiftDetail != null))
            {
                FMainDS.AGiftDetail.Rows.Clear();
            }

            FBatchRow = GetBatchRow();

            if (FBatchRow != null)
            {
                LoadGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, FBatchRow.BatchStatus);
            }
        }

        private void ReverseGift(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("Reverse Gift");
        }

        /// <summary>
        /// show the form for the gift reversal/adjustment
        /// </summary>
        /// <param name="AFunctionName">Which function shall be called on the server</param>
        public void ShowRevertAdjustForm(String AFunctionName)
        {
            bool reverseWholeBatch = (AFunctionName == "Reverse Gift Batch");

            AGiftBatchRow giftBatch = ((TFrmGiftBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();

            if (giftBatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift Batch to Reverse."));
                return;
            }

            if (!giftBatch.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED))
            {
                MessageBox.Show(Catalog.GetString("This function is only possible when the selected batch is already posted."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Please save first and than try again!"));
                return;
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a Gift to Reverse."));
                return;
            }

            if (reverseWholeBatch && (FBatchNumber != giftBatch.BatchNumber))
            {
                LoadGifts(giftBatch.LedgerNumber, giftBatch.BatchNumber, MFinanceConstants.BATCH_POSTED);
            }

            TFrmGiftRevertAdjust revertForm = new TFrmGiftRevertAdjust(FPetraUtilsObject.GetForm());

            try
            {
                ParentForm.ShowInTaskbar = false;
                revertForm.LedgerNumber = FLedgerNumber;
                revertForm.Text = AFunctionName;

                revertForm.AddParam("Function", AFunctionName.Replace(" ", string.Empty));

                if (reverseWholeBatch)
                {
                    revertForm.GiftMainDS = FMainDS;
                }

//                revertForm.GiftBatchRow = giftBatch;   // TODO Decide whether to remove altogether

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

        /// <summary>
        /// Reverse the whole gift batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReverseGiftBatch(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("Reverse Gift Batch");
        }

        private void ReverseGiftDetail(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("Reverse Gift Detail");
        }

        private void AdjustGift(System.Object sender, System.EventArgs e)
        {
            ShowRevertAdjustForm("Adjust Gift");
        }

        /// <summary>
        /// update the transaction DateEntered from outside
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
                //This code runs when the gift amount is updated
                if (FExchangeRateToBase != GetBatchRow().ExchangeRateToBase)
                {
                    FExchangeRateToBase = GetBatchRow().ExchangeRateToBase;
                }

                FPreviouslySelectedDetailRow.GiftAmount = (decimal)txtDetailGiftTransactionAmount.NumberValueDecimal * FExchangeRateToBase;

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
                    SelectRowInGrid(counter + 1);
                    break;
                }
            }
        }

        private void RunOnceOnParentActivationManual()
        {
            AutoSizeGrid();
        }

        /// <summary>
        /// AutoSize the grid columns (call this after the window has been restored to normal size after being maximized)
        /// </summary>
        public void AutoSizeGrid()
        {
            //TODO: Using this manual code until we can do something better
            //      Autosizing all the columns is very time consuming when there are many rows
            foreach (SourceGrid.DataGridColumn column in grdDetails.Columns)
            {
                column.Width = 100;
                column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch;
            }

            grdDetails.Columns[0].Width = 60;
            grdDetails.Columns[1].Width = 60;
            grdDetails.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.Default;
            grdDetails.Columns[3].Width = 50;
            grdDetails.Columns[4].Width = 25;
            grdDetails.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.Default;

            grdDetails.AutoStretchColumnsToFitWidth = true;
            grdDetails.Rows.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdDetails.AutoSizeCells();
            grdDetails.ShowCell(FPrevRowChangedRow);
        }
    }
}