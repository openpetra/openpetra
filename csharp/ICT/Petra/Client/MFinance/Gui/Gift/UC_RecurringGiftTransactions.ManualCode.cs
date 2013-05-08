//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections;
using System.Collections.Specialized;


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
    public partial class TUC_RecurringGiftTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private string FBatchMethodOfPayment = string.Empty;
        private Int64 FLastDonor = -1;
        private bool FActiveOnly = true;
        private ARecurringGiftBatchRow FBatchRow = null;


        private void InitialiseControls()
        {
            txtDetailReference.MaxLength = 20;
        }

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AFromTabClick">Indicates if called from a click on a tab or from grid doubleclick</param>
        public void LoadGifts(Int32 ALedgerNumber, Int32 ABatchNumber, bool AFromTabClick = true)
        {
            bool firstLoad = (FLedgerNumber == -1);

            if (firstLoad)
            {
                InitialiseControls();
            }

            //Enable buttons accordingly
            btnDeleteDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            btnNewDetail.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            btnNewGift.Enabled = !FPetraUtilsObject.DetailProtectedMode;

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber))
            {
                //Same as previously selected
                if (grdDetails.SelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());

                    if (AFromTabClick)
                    {
                        grdDetails.Focus();
                    }
                }

                UpdateControlsProtection();

                return;
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchRow = GetBatchRow();

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringGiftDetail.DefaultView);

            FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = ARecurringGiftDetailTable.GetBatchNumberDBName() + "=" + FBatchNumber.ToString();

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
            if (FMainDS.ARecurringGiftDetail.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(ALedgerNumber, ABatchNumber));
            }

            FMainDS.ARecurringGiftDetail.DefaultView.Sort = string.Format("{0} ASC, {1} ASC",
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                AGiftDetailTable.GetDetailNumberDBName());

            ShowData();
            ShowDetails();

            if (AFromTabClick)
            {
                grdDetails.Focus();
            }

            //pnlDetails.Enabled = (grdDetails.Rows.Count > 1); //this.PnlDetailsProtected;

            UpdateTotals();
            UpdateControlsProtection();
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

            try
            {
                FinRecipientKeyChanging = true;
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
                    //...this does not work as expected, because the timer fires valuechanged event after this value is reset
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

                    foreach (GiftBatchTDSARecurringGiftDetailRow giftDetail in FMainDS.ARecurringGiftDetail.Rows)
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
                Int64 rcp = cmbMinistry.GetSelectedInt64();

                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", rcp);
            }
            finally
            {
                FInKeyMinistryChanging = false;
            }
        }

        private void FilterMotivationDetail(object sender, EventArgs e)
        {
            TFinanceControls.ChangeFilterMotivationDetailList(ref cmbDetailMotivationDetailCode, cmbDetailMotivationGroupCode.GetSelectedString());
            RetrieveMotivationDetailAccountCode();

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                RetrieveMotivationDetailCostCentreCode();
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
            AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                new object[] { FLedgerNumber, cmbDetailMotivationGroupCode.GetSelectedString(), cmbDetailMotivationDetailCode.GetSelectedString() });

            if (motivationDetail != null)
            {
                RetrieveMotivationDetailAccountCode();

                // TODO: calculation of cost centre also depends on the recipient partner key; can be a field key or ministry key, or determined by pm_staff_data: foreign cost centre
                if (motivationDetail.CostCentreCode.EndsWith("S"))
                {
                    // work around if we have selected the cost centre already in bank import
                    // TODO: allow to select the cost centre here, which reports to the motivation cost centre
                    //txtDetailCostCentreCode.Text =
                }
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
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            Decimal sum = 0;
            Decimal sumBatch = 0;

            if (FPreviouslySelectedDetailRow == null)
            {
                txtGiftTotal.Text = "";
                txtBatchTotal.NumberValueDecimal = 0;

                //If all details have been deleted
                if ((FLedgerNumber != -1) && (grdDetails.Rows.Count == 1))
                {
                    if ((FBatchRow != null) && (FBatchRow.BatchTotal != 0))
                    {
                        FBatchRow.BeginEdit();
                        FBatchRow.BatchTotal = 0;
                        FBatchRow.EndEdit();
                    }
                }

                return;
            }

            Int32 GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

            foreach (ARecurringGiftDetailRow gdr in FMainDS.ARecurringGiftDetail.Rows)
            {
                if (gdr.RowState != DataRowState.Deleted)
                {
                    if ((gdr.BatchNumber == FBatchNumber) && (gdr.LedgerNumber == FLedgerNumber))
                    {
                        if (gdr.GiftTransactionNumber == GiftNumber)
                        {
                            if (FPreviouslySelectedDetailRow.DetailNumber == gdr.DetailNumber)
                            {
                                sum += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                                sumBatch += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                            }
                            else
                            {
                                sum += gdr.GiftAmount;
                                sumBatch += gdr.GiftAmount;
                            }
                        }
                        else
                        {
                            sumBatch += gdr.GiftAmount;
                        }
                    }
                }
            }

            txtGiftTotal.NumberValueDecimal = sum;
            txtGiftTotal.CurrencySymbol = txtDetailGiftAmount.CurrencySymbol;
            txtGiftTotal.ReadOnly = true;
            //this is here because at the moment the generator does not generate this
            txtBatchTotal.NumberValueDecimal = sumBatch;
            //Now we look at the batch and update the batch data
            FBatchRow.BatchTotal = sumBatch;
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
        private ARecurringGiftBatchRow GetBatchRow()
        {
            return (ARecurringGiftBatchRow)FMainDS.ARecurringGiftBatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
        }

        /// <summary>
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private ARecurringGiftRow GetGiftRow(Int32 ARecurringGiftTransactionNumber)
        {
            return (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, ARecurringGiftTransactionNumber });
        }

        /// <summary>
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private GiftBatchTDSARecurringGiftDetailRow GetGiftDetailRow(Int32 AGiftTransactionNumber, Int32 AGiftDetailNumber)
        {
            return (GiftBatchTDSARecurringGiftDetailRow)FMainDS.ARecurringGiftDetail.Rows.Find(new object[] { FLedgerNumber, FBatchNumber,
                                                                                                              AGiftTransactionNumber,
                                                                                                              AGiftDetailNumber });
        }

        /// <summary>
        /// delete a gift detail, and if it is the last detail, delete the whole gift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDetail(System.Object sender, EventArgs e)
        {
            DeleteARecurringGiftDetail();
        }

        ARecurringGiftRow FGift = null;
        string FFilterAllDetailsOfGift = string.Empty;
        DataView FGiftDetailView = null;

        private bool PreDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            FGift = GetGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
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
                    ARowToDelete.GiftAmount.ToString("C"));
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
                        ARowToDelete.GiftAmount.ToString("C"));
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

        private bool DeleteRowManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ACompletionMessage)
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
//                string filterAllDetailsOfBatch = String.Format("{0}={1}",
//                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
//                    oldBatchNumber);
//
//                giftDetailView.RowFilter = filterAllDetailsOfBatch;
//
//                foreach (DataRowView rv in giftDetailView)
//                {
//                    GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;
//
//                    if (row.GiftTransactionNumber > oldGiftNumber)
//                    {
//                        row.GiftTransactionNumber--;
//                    }
//                }
//                GetBatchRow().LastGiftNumber--;
//#endif
                }
                else
                {
                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        if (row.DetailNumber > selectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;
                }

                ACompletionMessage = Catalog.GetString("Recurring Gift row deleted successfully!");
                deleteSuccessful = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Error in trying to delete the current Recurring Gift!" + "\n\r\n\r" + "Error: {0}"),
                        ex.Message),
                    "Delete Row Error");
            }

            return deleteSuccessful;
        }

        private void PostDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete,
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

                txtBatchTotal.NumberValueDecimal = 0;
                txtDetailDonorKey.Text = string.Empty;
                chkDetailActive.Checked = false;
                txtDetailReference.Clear();
                txtGiftTotal.NumberValueDecimal = 0;
                txtDetailGiftAmount.NumberValueDecimal = 0;
                dtpStartDonations.Clear();
                dtpEndDonations.Clear();
                txtDetailRecipientKey.Text = string.Empty;
                txtField.Text = string.Empty;
                txtDetailAccountCode.Clear();
                txtDetailGiftCommentOne.Clear();
                txtDetailGiftCommentTwo.Clear();
                txtDetailGiftCommentThree.Clear();
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
                cmbDetailMotivationGroupCode.SelectedIndex = -1;
                cmbDetailMotivationDetailCode.SelectedIndex = -1;
                cmbDetailCommentOneType.SelectedIndex = -1;
                cmbDetailCommentTwoType.SelectedIndex = -1;
                cmbDetailCommentThreeType.SelectedIndex = -1;
                cmbDetailMailingCode.SelectedIndex = -1;
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
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
            ARecurringGiftDetailRow RecurringGiftDetailRow = NewGift(); // returns ARecurringGiftDetailRow

            if (RecurringGiftDetailRow != null)
            {
                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringGiftDetail.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1);
                //int newRowIndex = FMainDS.ARecurringGiftDetail.Rows.Count - 1;

                //SelectDetailRowByDataTableIndex(newRowIndex);
                //InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

                //FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                //ShowDetails(FPreviouslySelectedDetailRow);

                //GetDetailsFromControls(FPreviouslySelectedDetailRow, true);

                ////Need to redo this just in case the sorting is not on primary key
                //SelectDetailRowByDataTableIndex(newRowIndex);
            }
        }

        /// <summary>
        /// add a new gift detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGiftDetail(System.Object sender, EventArgs e)
        {
            // this is coded manually, to use the correct gift record

            //If grid is empty call NewGift() instead
            if (grdDetails.Rows.Count == 1)
            {
                NewGift(sender, e);
                return;
            }

            // we create the table locally, no dataset
            ARecurringGiftDetailRow ARecurringGiftDetailRow = NewGiftDetail(
                (GiftBatchTDSARecurringGiftDetailRow)FPreviouslySelectedDetailRow);

            if (ARecurringGiftDetailRow != null)
            {
                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringGiftDetail.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1);
                //int newRowIndex = FMainDS.ARecurringGiftDetail.Rows.Count - 1;

                //SelectDetailRowByDataTableIndex(newRowIndex);

                //InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

                //FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                //ShowDetails(FPreviouslySelectedDetailRow);

                //GetDetailsFromControls(FPreviouslySelectedDetailRow, true);

                ////Need to redo this just in case the sorting is not on primary key
                //SelectDetailRowByDataTableIndex(newRowIndex);

                RetrieveMotivationDetailAccountCode();
                txtDetailGiftAmount.Focus();
            }
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the batch.lastTransactionNumber is updated
        /// </summary>
        private ARecurringGiftDetailRow NewGift()
        {
            ARecurringGiftRow giftRow = FMainDS.ARecurringGift.NewRowTyped(true);

            giftRow.Active = true;

            giftRow.LedgerNumber = FBatchRow.LedgerNumber;
            giftRow.BatchNumber = FBatchRow.BatchNumber;
            giftRow.GiftTransactionNumber = FBatchRow.LastGiftNumber + 1;
            FBatchRow.LastGiftNumber++;
            giftRow.LastDetailNumber = 1;

            if (BatchHasMethodOfPayment())
            {
                giftRow.MethodOfPaymentCode = GetMethodOfPaymentFromBatch();
            }

            FMainDS.ARecurringGift.Rows.Add(giftRow);

            GiftBatchTDSARecurringGiftDetailRow newRow = FMainDS.ARecurringGiftDetail.NewRowTyped(true);
            newRow.LedgerNumber = FBatchRow.LedgerNumber;
            newRow.BatchNumber = FBatchRow.BatchNumber;
            newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
            newRow.DetailNumber = 1;
            //newRow.DateEntered = giftRow.DateEntered;
            newRow.DonorKey = 0;
            newRow.MotivationGroupCode = "GIFT";
            newRow.MotivationDetailCode = "SUPPORT";
            newRow.CommentOneType = "Both";
            newRow.CommentTwoType = "Both";
            newRow.CommentThreeType = "Both";
            FMainDS.ARecurringGiftDetail.Rows.Add(newRow);

            return newRow;
        }

        /// <summary>
        /// add another gift detail to an existing gift
        /// </summary>
        private ARecurringGiftDetailRow NewGiftDetail(GiftBatchTDSARecurringGiftDetailRow ACurrentRow)
        {
            if (ACurrentRow == null)
            {
                return NewGift();
            }

            // find gift row
            ARecurringGiftRow giftRow = GetGiftRow(ACurrentRow.GiftTransactionNumber);

            giftRow.LastDetailNumber++;

            GiftBatchTDSARecurringGiftDetailRow newRow = FMainDS.ARecurringGiftDetail.NewRowTyped(true);
            newRow.LedgerNumber = giftRow.LedgerNumber;
            newRow.BatchNumber = giftRow.BatchNumber;
            newRow.GiftTransactionNumber = giftRow.GiftTransactionNumber;
            newRow.DetailNumber = giftRow.LastDetailNumber;
            newRow.DonorKey = ACurrentRow.DonorKey;
            newRow.DonorName = ACurrentRow.DonorName;
            newRow.DateEntered = ACurrentRow.DateEntered;
            newRow.MotivationGroupCode = "GIFT";
            newRow.MotivationDetailCode = "SUPPORT";
            newRow.CommentOneType = "Both";
            newRow.CommentTwoType = "Both";
            newRow.CommentThreeType = "Both";
            FMainDS.ARecurringGiftDetail.Rows.Add(newRow);

            // TODO: use previous gifts of donor?

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
                txtDetailGiftAmount.CurrencySymbol = FBatchRow.CurrencyCode;
            }

            if (grdDetails.Rows.Count == 1)
            {
                txtBatchTotal.NumberValueDecimal = 0;
                ClearControls();
            }
            else
            {
                //----Set Cost Centre Code
                long PartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

                TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, PartnerKey);

                long FieldNumber = Convert.ToInt64(txtField.Text);

                txtDetailCostCentreCode.Text = TRemote.MFinance.Gift.WebConnectors.IdentifyPartnerCostCentre(FLedgerNumber, FieldNumber);
            }

            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfGivingCode, Catalog.GetString("Enter method of giving"));
            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfPaymentCode, Catalog.GetString("Enter the method of payment"));
            FPetraUtilsObject.SetStatusBarText(txtDetailReference, Catalog.GetString("Enter a reference code."));
            FPetraUtilsObject.SetStatusBarText(cmbDetailReceiptLetterCode, Catalog.GetString("Select the receipt letter code"));
        }

        private void ShowDetailsManual(ARecurringGiftDetailRow ARow)
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
            txtDetailDonorKey.Text = ((GiftBatchTDSARecurringGiftDetailRow)ARow).DonorKey.ToString();

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                txtDetailCostCentreCode.Text = string.Empty;
            }

            UpdateControlsProtection(ARow);

            ShowDetailsForGift(ARow);

            UpdateTotals();
        }

        void ShowDetailsForGift(ARecurringGiftDetailRow ARow)
        {
            // this is a special case - normally these lines would be produced by the generator
            ARecurringGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                txtDetailCostCentreCode.Text = string.Empty;
            }

            if (giftRow.IsActiveNull())
            {
                chkDetailActive.Checked = false;
            }
            else
            {
                chkDetailActive.Checked = giftRow.Active;
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
            txtDetailGiftAmount.CurrencySymbol = ACurrencyCode;
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

            if (!((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().FSelectedBatchMethodOfPayment;

            ledgerNumber = FBatchRow.LedgerNumber;
            batchNumber = FBatchRow.BatchNumber;

            if (FMainDS.ARecurringGift.Rows.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(ledgerNumber, batchNumber));
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
            foreach (ARecurringGiftRow recurringGiftRow in FMainDS.ARecurringGift.Rows)
            {
                if (recurringGiftRow.BatchNumber.Equals(batchNumber) && recurringGiftRow.LedgerNumber.Equals(ledgerNumber)
                    && (recurringGiftRow.MethodOfPaymentCode != FBatchMethodOfPayment))
                {
                    recurringGiftRow.MethodOfPaymentCode = FBatchMethodOfPayment;
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

        private void UpdateControlsProtection(ARecurringGiftDetailRow ARow)
        {
            bool firstIsEnabled = (ARow != null) && (ARow.DetailNumber == 1);

            chkDetailActive.Enabled = firstIsEnabled;
            txtDetailDonorKey.Enabled = firstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = firstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = firstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = firstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = firstIsEnabled;

            if (FBatchRow == null)
            {
                FBatchRow = GetBatchRow();
            }

            pnlDetails.Enabled = (ARow != null);
        }

        private Boolean BatchHasMethodOfPayment()
        {
            String batchMop = GetMethodOfPaymentFromBatch();

            return batchMop != null && batchMop.Length > 0;
        }

        private String GetMethodOfPaymentFromBatch()
        {
            return ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
        }

        private void GetDetailDataFromControlsManual(ARecurringGiftDetailRow ARow)
        {
            //ARow.CostCentreCode = txtDetailCostCentreCode.Text;  //Not present in recurring gifts details table

            if (ARow.DetailNumber != 1)
            {
                return;
            }

            ARecurringGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            if (giftRow != null)
            {
                giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
                giftRow.Active = chkDetailActive.Checked;

                GiftBatchTDSARecurringGiftDetailRow giftDetailRow = GetGiftDetailRow(ARow.GiftTransactionNumber, ARow.DetailNumber);
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

//			giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
            //giftRow.DateEntered = dtpDateEntered.Date.Value;
//
//            foreach (GiftBatchTDSARecurringGiftDetailRow giftDetail in FMainDS.ARecurringGiftDetail.Rows)
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

//            giftRow.Active = chkDetailActive.Checked;
//
//            if (cmbDetailMethodOfGivingCode.SelectedIndex == -1)
//            {
//                giftRow.SetMethodOfGivingCodeNull();
//            }
//            else
//            {
//                giftRow.MethodOfGivingCode = cmbDetailMethodOfGivingCode.GetSelectedString();
//            }
//
//            if (cmbDetailMethodOfPaymentCode.SelectedIndex == -1)
//            {
//                giftRow.SetMethodOfPaymentCodeNull();
//            }
//            else
//            {
//                giftRow.MethodOfPaymentCode = cmbDetailMethodOfPaymentCode.GetSelectedString();
//            }
//
//            if (txtDetailReference.Text.Length == 0)
//            {
//                giftRow.SetReferenceNull();
//            }
//            else
//            {
//                giftRow.Reference = txtDetailReference.Text;
//            }
//
//            if (cmbDetailReceiptLetterCode.SelectedIndex == -1)
//            {
//                giftRow.SetReceiptLetterCodeNull();
//            }
//            else
//            {
//                giftRow.ReceiptLetterCode = cmbDetailReceiptLetterCode.GetSelectedString();
//            }
        }

        private void ValidateDataDetailsManual(ARecurringGiftDetailRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateRecurringGiftDetailManual(this, ARow, ref VerificationResultCollection,
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
    }
}