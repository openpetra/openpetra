//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MPartner.Gui;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        private string FBatchMethodOfPayment = string.Empty;
        private Int64 FLastDonor = -1;
        private bool FActiveOnly = true;
        private bool FGiftSelectedForDeletion = false;
        private bool FSuppressListChanged = false;
        private bool FInRecipientKeyChanging = false;
        private bool FInKeyMinistryChanging = false;
        private bool FMotivationDetailChanged = false;
        private bool FCreatingNewGift = false;
        private bool FInEditMode = false;
        private bool FShowingDetails = false;
        private bool FTaxDeductiblePercentageEnabled = false;
        private ToolTip FDonorInfoToolTip = new ToolTip();

        private AGiftRow FGift = null;
        private string FMotivationGroup = string.Empty;
        private string FMotivationDetail = string.Empty;
        string FFilterAllDetailsOfGift = string.Empty;
        private DataView FGiftDetailView = null;

        private string FBatchStatus = string.Empty;
        private bool FBatchUnposted = false;
        private string FBatchCurrencyCode = string.Empty;
        private decimal FBatchExchangeRateToBase = 1.0m;
        private bool FGLEffectivePeriodChanged = false;

        private List <Int64>FNewDonorsList = new List <long>();

        /// <summary>
        /// The current Ledger number
        /// </summary>
        public Int32 FLedgerNumber = -1;

        /// <summary>
        /// The current Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;

        /// <summary>
        /// Points to the current active Batch
        /// </summary>
        public AGiftBatchRow FBatchRow = null;

        /// <summary>
        /// Specifies that initial transactions have loaded into the dataset
        /// </summary>
        public bool FTransactionsLoaded = false;

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewMode;
            }
        }

        /// <summary>
        /// List of partner keys of first time donors with a gift to be saved
        /// </summary>
        public List <Int64>NewDonorsList
        {
            get
            {
                return FNewDonorsList;
            }
            set
            {
                FNewDonorsList = value;
            }
        }

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            ReconcileKeyMinistryFromCombo();
        }

        private void PreProcessCommandKey()
        {
            ReconcileKeyMinistryFromCombo();
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            // should Tax Deductibility Percentage be enabled? (specifically for OM Switzerland)
            FTaxDeductiblePercentageEnabled = Convert.ToBoolean(
                TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));
        }

        private void InitialiseControls()
        {
            //Fix to length of field
            txtDetailReference.MaxLength = 20;

            //Fix a layering issue
            txtDetailRecipientLedgerNumber.SendToBack();

            //Changing this will stop taborder issues
            sptTransactions.TabStop = false;

            SetupTextBoxMenuItems();
            txtDetailRecipientKey.PartnerClass = "WORKER,UNIT,FAMILY";

            //Event fires when the recipient key is changed and the new partner has a different Partner Class
            txtDetailRecipientKey.PartnerClassChanged += RecipientPartnerClassChanged;

            //Set initial width of this textbox
            cmbKeyMinistries.ComboBoxWidth = 250;
            cmbKeyMinistries.AttachedLabel.Visible = false;

            //Setup hidden text boxes used to speed up reading transactions
            SetupComboTextBoxOverlayControls();

            //Make TextBox look like a label
            txtDonorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtDonorInfo.Font = TAppSettingsManager.GetDefaultBoldFont();

            if (FTaxDeductiblePercentageEnabled)
            {
                // set up Tax Deductibility Percentage (specifically for OM Switzerland)
                SetupTaxDeductibilityControls();
            }

            chkDetailChargeFlag.Enabled = UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3);
        }

        private void SetupTextBoxMenuItems()
        {
            List <Tuple <string, EventHandler>>ItemList = new List <Tuple <string, EventHandler>>();

            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Donor History"), OpenDonorHistory));
            txtDetailDonorKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient History"), OpenRecipientHistory));
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtDetailRecipientKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtDetailRecipientLedgerNumber.AddCustomContextMenuItems(ItemList);
        }

        private void SetupComboTextBoxOverlayControls()
        {
            txtDetailRecipientKeyMinistry.TabStop = false;
            txtDetailRecipientKeyMinistry.BorderStyle = BorderStyle.None;
            txtDetailRecipientKeyMinistry.Top = cmbKeyMinistries.Top + 3;
            txtDetailRecipientKeyMinistry.Left += 3;
            txtDetailRecipientKeyMinistry.Width = cmbKeyMinistries.ComboBoxWidth - 21;

            txtDetailRecipientKeyMinistry.Click += new EventHandler(SetFocusToKeyMinistryCombo);
            txtDetailRecipientKeyMinistry.Enter += new EventHandler(SetFocusToKeyMinistryCombo);
            txtDetailRecipientKeyMinistry.KeyDown += new KeyEventHandler(OverlayTextBox_KeyDown);
            txtDetailRecipientKeyMinistry.KeyPress += new KeyPressEventHandler(OverlayTextBox_KeyPress);

            pnlDetails.Enter += new EventHandler(BeginEditMode);
            pnlDetails.Leave += new EventHandler(EndEditMode);

            TUC_GiftTransactions_Recipient.SetTextBoxOverlayOnKeyMinistryCombo(FPreviouslySelectedDetailRow, FActiveOnly, cmbKeyMinistries,
                cmbDetailMotivationDetailCode, txtDetailRecipientKeyMinistry, ref FMotivationDetail, FInEditMode, FBatchUnposted);
        }

        private void SetFocusToKeyMinistryCombo(object sender, EventArgs e)
        {
            cmbKeyMinistries.Focus();
        }

        private void OverlayTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void OverlayTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void BeginEditMode(object sender, EventArgs e)
        {
            FInEditMode = true;

            bool DoTaxUpdate;
            TUC_GiftTransactions_Recipient.SetKeyMinistryTextBoxInvisible(FPreviouslySelectedDetailRow, FMainDS, FLedgerNumber, FPetraUtilsObject,
                cmbKeyMinistries, ref cmbDetailMotivationDetailCode, txtDetailRecipientKey, txtDetailRecipientLedgerNumber, txtDetailCostCentreCode,
                txtDetailAccountCode, txtDetailRecipientKeyMinistry, chkDetailTaxDeductible, txtDeductibleAccount,
                FMotivationGroup, ref FMotivationDetail, ref FMotivationDetailChanged, FActiveOnly,
                FInRecipientKeyChanging, FCreatingNewGift, FInEditMode, FBatchUnposted, FTaxDeductiblePercentageEnabled, out DoTaxUpdate);

            if (DoTaxUpdate)
            {
                UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), FInRecipientKeyChanging);
                EnableOrDiasbleTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
            }
        }

        private void EndEditMode(object sender, EventArgs e)
        {
            FInEditMode = false;
            TUC_GiftTransactions_Recipient.OnEndEditMode(FPreviouslySelectedDetailRow, cmbKeyMinistries, cmbDetailMotivationDetailCode,
                txtDetailRecipientKeyMinistry, ref FMotivationDetail, FActiveOnly, FInEditMode, FBatchUnposted);
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public void ReconcileKeyMinistryFromCombo()
        {
            TUC_GiftTransactions_Recipient.ReconcileKeyMinistryFromCombo(FPreviouslySelectedDetailRow,
                cmbKeyMinistries,
                txtDetailRecipientKeyMinistry,
                FInEditMode,
                FBatchUnposted);
        }

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <returns>True if new gift transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadGifts(Int32 ALedgerNumber, Int32 ABatchNumber, string ABatchStatus)
        {
            FBatchRow = GetCurrentBatchRow();

            if ((FBatchRow == null) && (GetAnyBatchRow(ABatchNumber) == null))
            {
                MessageBox.Show(String.Format("Cannot load transactions for Gift Batch {0} as the batch is not currently loaded!", ABatchNumber));
                return false;
            }

            //Reset Batch method of payment variable
            FBatchMethodOfPayment = ((TFrmGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;

            bool firstLoad = (FLedgerNumber == -1);

            if (firstLoad)
            {
                InitialiseControls();
            }

            UpdateCurrencySymbols(FBatchRow.CurrencyCode);

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FBatchStatus == ABatchStatus))
            {
                //Same as previously selected
                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED) && (GetSelectedRowIndex() > 0))
                {
                    if (FGLEffectivePeriodChanged)
                    {
                        //Just in case for the currently selected row, the date field has not been updated
                        FGLEffectivePeriodChanged = false;
                        GetSelectedDetailRow().DateEntered = FBatchRow.GlEffectiveDate;
                        dtpDateEntered.Date = FBatchRow.GlEffectiveDate;
                    }

                    //Same as previously selected
                    if (GetSelectedRowIndex() > 0)
                    {
                        GetDetailsFromControls(GetSelectedDetailRow());
                    }
                }

                UpdateControlsProtection();

                if ((ABatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                    && ((FBatchCurrencyCode != FBatchRow.CurrencyCode) || (FBatchExchangeRateToBase != FBatchRow.ExchangeRateToBase)))
                {
                    UpdateBaseAmount(false);
                }

                return false;
            }

            grdDetails.SuspendLayout();
            FSuppressListChanged = true;

            //Read key fields
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchCurrencyCode = FBatchRow.CurrencyCode;

            //Process Batch status
            FBatchStatus = ABatchStatus;
            FBatchUnposted = (FBatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;

            // if this form is readonly, then we need all codes, because old codes might have been used
            if (firstLoad || (FActiveOnly != this.Enabled))
            {
                FActiveOnly = this.Enabled;

                TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMotivationDetailList(ref cmbDetailMotivationDetailCode, FLedgerNumber, FActiveOnly);
                TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, FActiveOnly);
                TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, FActiveOnly);
                TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, FActiveOnly);
            }

            // This sets the incomplete filter but does check the panel enabled state
            ShowData();

            // This sets the main part of the filter but excluding the additional items set by the user GUI
            // It gets the right sort order
            SetGiftDetailDefaultView();

            // only load from server if there are no transactions loaded yet for this batch
            // otherwise we would overwrite transactions that have already been modified
            if (FMainDS.AGiftDetail.DefaultView.Count == 0)
            {
                LoadGiftDataForBatch(ALedgerNumber, ABatchNumber);
            }

            //Check if need to update batch period in each gift
            ((TFrmGiftBatch)ParentForm).GetBatchControl().UpdateBatchPeriod();

            // Now we set the full filter
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            FSuppressListChanged = false;
            grdDetails.ResumeLayout();

            UpdateTotals();

            if ((FPreviouslySelectedDetailRow != null) && (FBatchStatus == MFinanceConstants.BATCH_UNPOSTED))
            {
                TUC_GiftTransactions_Recipient.GetRecipientData(FPreviouslySelectedDetailRow, FPreviouslySelectedDetailRow.RecipientKey,
                    ref cmbKeyMinistries, txtDetailRecipientKey, ref txtDetailRecipientLedgerNumber);
            }

            FTransactionsLoaded = true;
            return true;
        }

        /// <summary>
        /// Ensure the data is loaded for the specified batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>If transactions exist</returns>
        private bool LoadGiftDataForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool RetVal = ((TFrmGiftBatch)ParentForm).EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);

            TUC_GiftTransactions_Recipient.UpdateAllRecipientDescriptions(ABatchNumber, FMainDS);

            ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);

            return RetVal;
        }

        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            bool? DoEnableRecipientHistory;
            bool DoValidateGiftDestination;
            bool DoTaxUpdate;

            TUC_GiftTransactions_Recipient.OnRecipientKeyChanged(APartnerKey, APartnerShortName, AValidSelection, FPreviouslySelectedDetailRow,
                FMainDS, FLedgerNumber, FPetraUtilsObject, ref cmbKeyMinistries, cmbDetailMotivationGroupCode, cmbDetailMotivationDetailCode,
                txtDetailRecipientKey, txtDetailRecipientLedgerNumber, txtDetailCostCentreCode, txtDetailRecipientKeyMinistry, chkDetailTaxDeductible,
                ref FMotivationGroup, ref FMotivationDetail, FShowingDetails, ref FInRecipientKeyChanging, FInKeyMinistryChanging, FInEditMode,
                FBatchUnposted, FMotivationDetailChanged, FTaxDeductiblePercentageEnabled, 
                out DoEnableRecipientHistory, out DoValidateGiftDestination, out DoTaxUpdate);

            if (DoTaxUpdate)
            {
                EnableOrDiasbleTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
                UpdateTaxDeductiblePct(APartnerKey, true);
            }

            if (DoValidateGiftDestination)
            {
                FPartnerShortName = APartnerShortName;

                //Thread only invokes ValidateGiftDestination once Partner Short Name has been updated.
                // Otherwise the Gift Destination screen is displayed and then the screen focus moves to this screen again
                // when the Partner Short Name is updated.
                new Thread(ValidateGiftDestinationThread).Start();
            }

            if (DoEnableRecipientHistory.HasValue)
            {
                mniRecipientHistory.Enabled = DoEnableRecipientHistory.Value;
            }
        }

        // used for ValidateGiftDestinationThread
        private string FPartnerShortName = "";
        private delegate void SimpleDelegate();

        private void ValidateGiftDestinationThread()
        {
            // Check whether this thread should still execute
            while (txtDetailRecipientKey.LabelText != FPartnerShortName)
            {
                // Wait and see if anything has changed
                Thread.Sleep(10);
            }

            Invoke(new SimpleDelegate(ValidateGiftDestination));
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
            else if (FShowingDetails || (APartnerKey == 0))
            {
                mniDonorHistory.Enabled = false;
                txtDonorInfo.Text = "";
                return;
            }
            else
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    if (APartnerKey != FLastDonor)
                    {
                        PPartnerTable PartnerDT = TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey);

                        if (PartnerDT.Rows.Count > 0)
                        {
                            PPartnerRow pr = PartnerDT[0];
                            chkDetailConfidentialGiftFlag.Checked = pr.AnonymousDonor;

                            // add row to dataset to access receipt frequency info for donors
                            FMainDS.DonorPartners.Merge(PartnerDT);
                        }

                        foreach (GiftBatchTDSAGiftDetailRow giftDetail in FMainDS.AGiftDetail.Rows)
                        {
                            if (giftDetail.BatchNumber.Equals(FBatchNumber)
                                && giftDetail.GiftTransactionNumber.Equals(FPreviouslySelectedDetailRow.GiftTransactionNumber))
                            {
                                giftDetail.DonorKey = APartnerKey;
                                giftDetail.DonorName = APartnerShortName;
                            }
                        }

                        AutoPopulateGiftDetail(APartnerKey);

                        mniDonorHistory.Enabled = true;
                    }

                    ShowDonorInfo(APartnerKey);

                    FLastDonor = APartnerKey;
                }
                finally
                {
                    Cursor = Cursors.Default;
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

            if (txt.Name.Contains("One"))
            {
                if (txtValue == String.Empty)
                {
                    cmbDetailCommentOneType.SelectedIndex = -1;
                }
                else if (cmbDetailCommentOneType.SelectedIndex == -1)
                {
                    cmbDetailCommentOneType.SetSelectedString("Both");
                }
            }
            else if (txt.Name.Contains("Two"))
            {
                if (txtValue == String.Empty)
                {
                    cmbDetailCommentTwoType.SelectedIndex = -1;
                }
                else if (cmbDetailCommentTwoType.SelectedIndex == -1)
                {
                    cmbDetailCommentTwoType.SetSelectedString("Both");
                }
            }
            else if (txt.Name.Contains("Three"))
            {
                if (txtValue == String.Empty)
                {
                    cmbDetailCommentThreeType.SelectedIndex = -1;
                }
                else if (cmbDetailCommentThreeType.SelectedIndex == -1)
                {
                    cmbDetailCommentThreeType.SetSelectedString("Both");
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

        private void KeyMinistryChanged(object sender, EventArgs e)
        {
            TUC_GiftTransactions_Recipient.OnKeyMinistryChanged(FPreviouslySelectedDetailRow, FPetraUtilsObject, cmbKeyMinistries,
                txtDetailRecipientKey, txtDetailRecipientKeyMinistry, FInRecipientKeyChanging, ref FInKeyMinistryChanging);
        }

        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            bool DoTaxUpdate;

            TUC_GiftTransactions_Recipient.OnMotivationGroupChanged(FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                cmbKeyMinistries,
                cmbDetailMotivationGroupCode,
                ref cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                txtDeductibleAccount,
                ref FMotivationGroup,
                ref FMotivationDetail,
                ref FMotivationDetailChanged,
                FActiveOnly,
                FCreatingNewGift,
                FInRecipientKeyChanging,
                FInEditMode,
                FBatchUnposted,
                FTaxDeductiblePercentageEnabled,
                out DoTaxUpdate);

            if (DoTaxUpdate)
            {
                UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), FInRecipientKeyChanging);
                EnableOrDiasbleTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
            }

            ValidateGiftDestination();
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            bool DoTaxUpdate;

            TUC_GiftTransactions_Recipient.OnMotivationDetailChanged(FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                cmbKeyMinistries,
                cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                txtDeductibleAccount,
                FMotivationGroup,
                ref FMotivationDetail,
                ref FMotivationDetailChanged,
                FInRecipientKeyChanging,
                FCreatingNewGift,
                FInEditMode,
                FBatchUnposted,
                FTaxDeductiblePercentageEnabled,
                out DoTaxUpdate);

            if (DoTaxUpdate)
            {
                bool DeductiblePercentageEnabled = txtDeductiblePercentage.Enabled;
                EnableOrDiasbleTaxDeductibilityPct(chkDetailTaxDeductible.Checked);

                // if txtDeductiblePercentage has been enabled or disabled then update the percentage
                if (DeductiblePercentageEnabled != txtDeductiblePercentage.Enabled)
                {
                    UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), FInRecipientKeyChanging);
                }
            }
        }

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if ((GetCurrentBatchRow() == null) || (txn.NumberValueDecimal == null))
            {
                return;
            }

            if (FTaxDeductiblePercentageEnabled)
            {
                // this will also call UpdateBaseAmount
                UpdateTaxDeductibilityAmounts(sender, e);
            }
            else if ((FPreviouslySelectedDetailRow != null) && (GetCurrentBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
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
                if ((FLedgerNumber != -1) && (FBatchRow != null) && (grdDetails.Rows.Count == 1))
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
        private AGiftBatchRow GetCurrentBatchRow()
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().GetCurrentBatchRow();
        }

        /// <summary>
        /// get the details of any loaded batch
        /// </summary>
        /// <returns></returns>
        private AGiftBatchRow GetAnyBatchRow(Int32 ABatchNumber)
        {
            return ((TFrmGiftBatch)ParentForm).GetBatchControl().GetAnyBatchRow(ABatchNumber);
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
        /// Clear the gift data of the current batch without marking records for delete
        /// </summary>
        private bool RefreshCurrentBatchGiftData(Int32 ABatchNumber)
        {
            bool RetVal = false;

            //Copy and backup the current dataset
            GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();

            TempDS.Merge(FMainDS);

            GiftBatchTDS BackupDS = (GiftBatchTDS)FMainDS.Copy();
            BackupDS.Merge(FMainDS);

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Remove current batch gift data
                DataView giftDetailView = new DataView(TempDS.AGiftDetail);

                giftDetailView.RowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView dr in giftDetailView)
                {
                    dr.Delete();
                }

                DataView giftView = new DataView(TempDS.AGift);

                giftView.RowFilter = String.Format("{0}={1}",
                    AGiftTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftView.Sort = String.Format("{0} DESC",
                    AGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView dr in giftView)
                {
                    dr.Delete();
                }

                TempDS.AcceptChanges();

                //Clear all gift data from Main dataset gift tables
                FMainDS.AGiftDetail.Clear();
                FMainDS.AGift.Clear();

                //Bring data back in from other batches if it exists
                if (TempDS.AGift.Count > 0)
                {
                    FMainDS.AGift.Merge(TempDS.AGift);
                    FMainDS.AGiftDetail.Merge(TempDS.AGiftDetail);
                }

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(FLedgerNumber, ABatchNumber));

                RetVal = true;
            }
            catch (Exception ex)
            {
                FMainDS.Merge(BackupDS);

                string errMsg = Catalog.GetString("Error trying to clear current Batch data: /n/r/n/r" + ex.Message);
                MessageBox.Show(errMsg, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return RetVal;
        }

        private void SetBatchLastGiftNumber()
        {
            DataView dv = new DataView(FMainDS.AGift);

            dv.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                FBatchNumber);

            dv.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            dv.RowStateFilter = DataViewRowState.CurrentRows;

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

        private void SetGiftDetailDefaultView()
        {
            if (FBatchNumber != -1)
            {
                string rowFilter = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FMainDS.AGiftDetail.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.AGiftDetail.DefaultView.Sort = string.Format("{0} DESC, {1}",
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetDetailNumberDBName());
            }
        }

        private void ClearControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                txtDetailDonorKey.Text = string.Empty;
                txtDetailReference.Clear();
                txtGiftTotal.NumberValueDecimal = 0;
                txtDetailGiftTransactionAmount.NumberValueDecimal = 0;
                txtDetailRecipientKey.Text = string.Empty;
                txtDetailRecipientLedgerNumber.Text = string.Empty;
                txtDetailAccountCode.Clear();
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
                cmbDetailMotivationGroupCode.SelectedIndex = -1;
                cmbDetailMotivationDetailCode.SelectedIndex = -1;
                txtDetailGiftCommentOne.Clear();
                txtDetailGiftCommentTwo.Clear();
                txtDetailGiftCommentThree.Clear();
                cmbDetailCommentOneType.SelectedIndex = -1;
                cmbDetailCommentTwoType.SelectedIndex = -1;
                cmbDetailCommentThreeType.SelectedIndex = -1;
                cmbDetailMailingCode.SelectedIndex = -1;
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
                cmbKeyMinistries.SelectedIndex = -1;
                txtDetailCostCentreCode.Text = string.Empty;

                dtpDateEntered.Clear();
            }
            finally
            {
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
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
                txtBatchStatus.Text = FBatchStatus;
            }

            if (grdDetails.Rows.Count == 1)
            {
                txtBatchTotal.NumberValueDecimal = 0;
                ClearControls();
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

        private void ShowDetailsManual(GiftBatchTDSAGiftDetailRow ARow)
        {
            if (TUC_GiftTransactions_Recipient.OnStartShowDetailsManual(ARow, cmbKeyMinistries, cmbDetailMotivationDetailCode,
                    txtDetailRecipientKeyMinistry, ref FMotivationDetail, FActiveOnly, FTransactionsLoaded, FInEditMode, FBatchUnposted))
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    FShowingDetails = true;

                    bool? DoEnableRecipientHistory;
                    bool? DoEnableRecipientGiftDestination;
                    TUC_GiftTransactions_Recipient.FinishShowDetailsManual(ARow,
                        cmbDetailMotivationDetailCode,
                        txtDetailRecipientKey,
                        txtDetailRecipientLedgerNumber,
                        txtDetailCostCentreCode,
                        txtDetailAccountCode,
                        ref FMotivationGroup,
                        ref FMotivationDetail,
                        out DoEnableRecipientHistory,
                        out DoEnableRecipientGiftDestination);

                    if (DoEnableRecipientHistory.HasValue)
                    {
                        mniRecipientHistory.Enabled = DoEnableRecipientHistory.Value;
                    }

                    if (DoEnableRecipientGiftDestination.HasValue)
                    {
                        mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
                    }

                    //Show gift table values
                    AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
                    ShowDetailsForGift(giftRow);

                ShowDonorInfo(Convert.ToInt64(txtDetailDonorKey.Text));

                    UpdateControlsProtection(ARow);

                    if (FTaxDeductiblePercentageEnabled)
                    {
                        ShowTaxDeductibleManual(ARow);
                    }
                }
                finally
                {
                    FShowingDetails = false;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// displays information about the donor
        /// </summary>
        /// <param name="APartnerKey"></param>
        private void ShowDonorInfo(long APartnerKey)
        {
            string DonorInfo = string.Empty;

            try
            {
                if (APartnerKey == 0)
                {
                    return;
                }

                // find PPartnerRow from dataset
                PPartnerRow DonorRow = (PPartnerRow)FMainDS.DonorPartners.Rows.Find(new object[] { APartnerKey });

                // if PPartnerRow cannot be found, load it from db
                if ((DonorRow == null) || (DonorRow[PPartnerTable.GetReceiptEachGiftDBName()] == DBNull.Value))
                {
                    PPartnerTable PartnerTable = TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey);

                    if ((PartnerTable == null) || (PartnerTable.Rows.Count == 0))
                    {
                        // invalid partner
                        return;
                    }

                    DonorRow = PartnerTable[0];
                }

                // get donor's banking details
                AGiftRow GiftRow = (AGiftRow)FMainDS.AGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber,
                                                                                    FPreviouslySelectedDetailRow.GiftTransactionNumber });
                PBankingDetailsTable BankingDetailsTable = TRemote.MFinance.Gift.WebConnectors.GetDonorBankingDetails(APartnerKey,
                    GiftRow.BankingDetailsKey);
                PBankingDetailsRow BankingDetailsRow = null;

                // set donor info text
                if ((BankingDetailsTable != null) && (BankingDetailsTable.Rows.Count > 0))
                {
                    BankingDetailsRow = BankingDetailsTable[0];
                }

                if ((BankingDetailsRow != null) && !string.IsNullOrEmpty(BankingDetailsRow.BankAccountNumber))
                {
                    DonorInfo = Catalog.GetString("Bank Account: ") + BankingDetailsRow.BankAccountNumber;
                }

                if (DonorRow.ReceiptEachGift)
                {
                    if (DonorInfo != string.Empty)
                    {
                        DonorInfo += "; ";
                    }

                    DonorInfo += "*" + Catalog.GetString("Receipt Each Gift") + "*";
                }

                if (!string.IsNullOrEmpty(DonorRow.ReceiptLetterFrequency))
                {
                    if (DonorInfo != string.Empty)
                    {
                        DonorInfo += "; ";
                    }

                    DonorInfo += DonorRow.ReceiptLetterFrequency + " " + Catalog.GetString("Receipt");
                }

                if (DonorRow.AnonymousDonor)
                {
                    if (DonorInfo != string.Empty)
                    {
                        DonorInfo += "; ";
                    }

                    DonorInfo += Catalog.GetString("Anonymous");
                }

                if ((BankingDetailsRow != null) && !string.IsNullOrEmpty(BankingDetailsRow.Comment))
                {
                    if (DonorInfo != string.Empty)
                    {
                        DonorInfo += "; ";
                    }

                    DonorInfo += BankingDetailsRow.Comment;
                }

                if (!string.IsNullOrEmpty(DonorRow.FinanceComment))
                {
                    if (DonorInfo != string.Empty)
                    {
                        DonorInfo += "; ";
                    }

                    DonorInfo += DonorRow.FinanceComment;
                }
            }
            finally
            {
                // shorten text if it is too long to display on screen
                if (DonorInfo.Length >= 65)
                {
                    txtDonorInfo.Text = DonorInfo.Substring(0, 62) + "...";
                }
                else
                {
                    txtDonorInfo.Text = DonorInfo;
                }

                FDonorInfoToolTip.SetToolTip(txtDonorInfo, DonorInfo);
                FPetraUtilsObject.SetStatusBarText(txtDonorInfo, DonorInfo);
            }
        }

        private void ShowDetailsForGift(AGiftRow ACurrentGiftRow)
        {
            //Set GiftRow controls
            dtpDateEntered.Date = ACurrentGiftRow.DateEntered;

            txtDetailDonorKey.Text = ACurrentGiftRow.DonorKey.ToString();

            if (ACurrentGiftRow.IsMethodOfGivingCodeNull())
            {
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfGivingCode.SetSelectedString(ACurrentGiftRow.MethodOfGivingCode);
            }

            if (ACurrentGiftRow.IsMethodOfPaymentCodeNull())
            {
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(ACurrentGiftRow.MethodOfPaymentCode);
            }

            if (ACurrentGiftRow.IsReferenceNull())
            {
                txtDetailReference.Text = String.Empty;
            }
            else
            {
                txtDetailReference.Text = ACurrentGiftRow.Reference;
            }

            if (ACurrentGiftRow.IsReceiptLetterCodeNull())
            {
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailReceiptLetterCode.SetSelectedString(ACurrentGiftRow.ReceiptLetterCode);
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

            FBatchRow = GetCurrentBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmGiftBatch) this.ParentForm).GetBatchControl().MethodOfPaymentCode;

            ledgerNumber = FBatchRow.LedgerNumber;
            batchNumber = FBatchRow.BatchNumber;

            if (FMainDS.AGift.Rows.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ledgerNumber, batchNumber));

                ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
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
            bool pnlDetailsEnabledState = false;

            dtpDateEntered.Enabled = firstIsEnabled;
            txtDetailDonorKey.Enabled = firstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = firstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = firstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = firstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = firstIsEnabled;

            if (FBatchRow == null)
            {
                FBatchRow = GetCurrentBatchRow();
            }

            if (ARow == null)
            {
                PnlDetailsProtected = (ViewMode
                                       || !FBatchUnposted
                                       );
            }
            else
            {
                PnlDetailsProtected = (ViewMode
                                       || !FBatchUnposted
                                       || (ARow.GiftTransactionAmount < 0 && GetGiftRow(ARow.GiftTransactionNumber).ReceiptNumber != 0)
                                       );    // taken from old petra
            }

            pnlDetailsEnabledState = (!PnlDetailsProtected && grdDetails.Rows.Count > 1);
            pnlDetails.Enabled = pnlDetailsEnabledState;

            btnDelete.Enabled = pnlDetailsEnabledState;
            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
            btnNewDetail.Enabled = !PnlDetailsProtected;
            btnNewGift.Enabled = !PnlDetailsProtected;
        }

        private Boolean BatchHasMethodOfPayment()
        {
            String BatchMop = GetMethodOfPaymentFromBatch();

            return BatchMop != null && BatchMop.Length > 0;
        }

        private String GetMethodOfPaymentFromBatch()
        {
            if (FBatchMethodOfPayment == string.Empty)
            {
                FBatchMethodOfPayment = ((TFrmGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
            }

            return FBatchMethodOfPayment;
        }

        private void GetDetailDataFromControlsManual(GiftBatchTDSAGiftDetailRow ARow)
        {
            if (txtDetailCostCentreCode.Text.Length == 0)
            {
                ARow.SetCostCentreCodeNull();
            }
            else
            {
                ARow.CostCentreCode = txtDetailCostCentreCode.Text;
            }

            if (txtDetailAccountCode.Text.Length == 0)
            {
                ARow.SetAccountCodeNull();
            }
            else
            {
                ARow.AccountCode = txtDetailAccountCode.Text;
            }

            if (ARow.IsRecipientKeyNull())
            {
                ARow.SetRecipientDescriptionNull();
            }
            else
            {
                TUC_GiftTransactions_Recipient.UpdateRecipientKeyText(ARow.RecipientKey, ARow, cmbDetailMotivationDetailCode);
            }

            if (txtDetailRecipientLedgerNumber.Text.Length == 0)
            {
                ARow.SetRecipientFieldNull();
                ARow.SetRecipientLedgerNumberNull();
            }
            else
            {
                ARow.RecipientField = Convert.ToInt64(txtDetailRecipientLedgerNumber.Text);
                ARow.RecipientLedgerNumber = ARow.RecipientField;
            }

            //Handle gift table fields for first detail only
            if (ARow.DetailNumber == 1)
            {
                AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

                giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);
                giftRow.DateEntered = (dtpDateEntered.Date.HasValue ? dtpDateEntered.Date.Value : FBatchRow.GlEffectiveDate);

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

            if (FTaxDeductiblePercentageEnabled)
            {
                GetTaxDeductibleDataFromControlsManual(ref ARow);
            }
        }

        private void ValidateDataDetailsManual(GiftBatchTDSAGiftDetailRow ARow)
        {
            if ((ARow == null) || (GetCurrentBatchRow() == null) || (GetCurrentBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                || (GetCurrentBatchRow().BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateGiftDetailManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, FPreviouslySelectedDetailRow.RecipientField);

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            TSharedFinanceValidation_Gift.ValidateGiftManual(this,
                giftRow,
                FBatchRow.BatchYear,
                FBatchRow.BatchPeriod,
                dtpDateEntered,
                ref VerificationResultCollection,
                FValidationControlsDict);

            if (FTaxDeductiblePercentageEnabled)
            {
                ValidateTaxDeductiblePct(ARow, ref VerificationResultCollection);
            }
        }

        private void ValidateGiftDestination()
        {
            // if no gift destination exists for parter then give the user the option to open Gift Destination maintenance screen
            if ((FPreviouslySelectedDetailRow != null) 
                && (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) == 0) 
                && (FPreviouslySelectedDetailRow.RecipientKey != 0)
                && (cmbDetailMotivationGroupCode.GetSelectedString() == MFinanceConstants.MOTIVATION_GROUP_GIFT)
                && (MessageBox.Show(Catalog.GetString("No valid Gift Destination exists for ") +
                        FPreviouslySelectedDetailRow.RecipientDescription +
                        " (" + FPreviouslySelectedDetailRow.RecipientKey + ").\n\n" +
                        string.Format(Catalog.GetString("A Gift Destination will need to be assigned to this Partner before" +
                                " this gift can be saved with the Motivation Group '{0}'." +
                                " Would you like to do this now?"), MFinanceConstants.MOTIVATION_GROUP_GIFT),
                        Catalog.GetString("No valid Gift Destination"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                OpenGiftDestination(this, null);
            }
        }

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                grdDetails.Focus();
            }
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

            FBatchRow = GetCurrentBatchRow();

            if (FBatchRow != null)
            {
                LoadGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, FBatchRow.BatchStatus);
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChanged && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }

            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
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

            if (!((TFrmGiftBatch)ParentForm).SaveChanges())
            {
                return;
            }

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

                //revertForm.GiftBatchRow = giftBatch;   // TODO Decide whether to remove altogether

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

            if (ABatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            ledgerNumber = ABatchRow.LedgerNumber;
            batchNumber = ABatchRow.BatchNumber;
            batchEffectiveDate = ABatchRow.GlEffectiveDate;

            DataView giftDataView = new DataView(FMainDS.AGift);

            giftDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                AGiftTable.GetLedgerNumberDBName(),
                ledgerNumber,
                AGiftTable.GetBatchNumberDBName(),
                batchNumber);

            if (giftDataView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ledgerNumber, batchNumber));

                ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
            }
            else if ((FPreviouslySelectedDetailRow != null) && (FBatchNumber == batchNumber))
            {
                //Rows already active in transaction tab. Need to set current row as code below will not update currently selected row
                FGLEffectivePeriodChanged = true;
                GetSelectedDetailRow().DateEntered = batchEffectiveDate;
            }

            //Update all gift rows in this batch
            foreach (DataRowView dv in giftDataView)
            {
                AGiftRow giftRow = (AGiftRow)dv.Row;
                giftRow.DateEntered = batchEffectiveDate;
            }

            //If current row exists then refresh details
            if (FGLEffectivePeriodChanged)
            {
                ShowDetails();
            }
        }

        /// <summary>
        /// Update the transaction base amount calculation
        /// </summary>
        /// <param name="AUpdateCurrentRowOnly"></param>
        public void UpdateBaseAmount(Boolean AUpdateCurrentRowOnly)
        {
            Int32 LedgerNumber;
            Int32 CurrentBatchNumber;

            DateTime BatchEffectiveDate;

            decimal BatchExchangeRateToBase = 0;
            string BatchCurrencyCode = string.Empty;
            decimal IntlToBaseCurrencyExchRate = 0;
            bool IsTransactionInIntlCurrency;

            string LedgerBaseCurrency = string.Empty;
            string LedgerIntlCurrency = string.Empty;

            bool TransactionsFromCurrentBatch = false;

            AGiftBatchRow CurrentBatchRow = GetCurrentBatchRow();

            if (!(((TFrmGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
                || (CurrentBatchRow == null)
                || (CurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            BatchCurrencyCode = CurrentBatchRow.CurrencyCode;
            BatchExchangeRateToBase = CurrentBatchRow.ExchangeRateToBase;

            if ((FBatchRow != null)
                && (CurrentBatchRow.LedgerNumber == FBatchRow.LedgerNumber)
                && (CurrentBatchRow.BatchNumber == FBatchRow.BatchNumber))
            {
                TransactionsFromCurrentBatch = true;
                FBatchCurrencyCode = BatchCurrencyCode;
                FBatchExchangeRateToBase = BatchExchangeRateToBase;
            }

            LedgerNumber = CurrentBatchRow.LedgerNumber;
            CurrentBatchNumber = CurrentBatchRow.BatchNumber;

            BatchEffectiveDate = CurrentBatchRow.GlEffectiveDate;
            LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;

            IntlToBaseCurrencyExchRate = ((TFrmGiftBatch)ParentForm).InternationalCurrencyExchangeRate(CurrentBatchRow,
                out IsTransactionInIntlCurrency);

            if (!LoadGiftDataForBatch(LedgerNumber, CurrentBatchNumber))
            {
                //No transactions exist to process or corporate exchange rate not found
                return;
            }

            //If only updating the currency active row
            if (AUpdateCurrentRowOnly && (FPreviouslySelectedDetailRow != null))
            {
                FPreviouslySelectedDetailRow.GiftAmount = GLRoutines.Divide((decimal)txtDetailGiftTransactionAmount.NumberValueDecimal,
                    BatchExchangeRateToBase);

                if (!IsTransactionInIntlCurrency)
                {
                    FPreviouslySelectedDetailRow.GiftAmountIntl = (IntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(
                        FPreviouslySelectedDetailRow.GiftAmount,
                        IntlToBaseCurrencyExchRate);
                }
                else
                {
                    FPreviouslySelectedDetailRow.GiftAmountIntl = FPreviouslySelectedDetailRow.GiftTransactionAmount;
                }
            }
            else
            {
                if (TransactionsFromCurrentBatch && (FPreviouslySelectedDetailRow != null))
                {
                    //Rows already active in transaction tab. Need to set current row as code further below will not update selected row
                    FPreviouslySelectedDetailRow.GiftAmount = GLRoutines.Divide(FPreviouslySelectedDetailRow.GiftTransactionAmount,
                        BatchExchangeRateToBase);

                    if (!IsTransactionInIntlCurrency)
                    {
                        FPreviouslySelectedDetailRow.GiftAmountIntl = (IntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(
                            FPreviouslySelectedDetailRow.GiftAmount,
                            IntlToBaseCurrencyExchRate);
                    }
                    else
                    {
                        FPreviouslySelectedDetailRow.GiftAmountIntl = FPreviouslySelectedDetailRow.GiftTransactionAmount;
                    }
                }

                //Update all transactions
                RecalcTransactionsCurrencyAmounts(CurrentBatchRow, IntlToBaseCurrencyExchRate, IsTransactionInIntlCurrency);
            }

            if (FTaxDeductiblePercentageEnabled)
            {
                UpdateTaxDeductibiltyBaseAmounts(CurrentBatchRow, AUpdateCurrentRowOnly, IsTransactionInIntlCurrency, BatchExchangeRateToBase,
                    IntlToBaseCurrencyExchRate, TransactionsFromCurrentBatch);
            }
        }

        /// <summary>
        /// Update all single batch transactions currency amounts fields
        /// </summary>
        /// <param name="ABatchRow"></param>
        /// <param name="AIntlToBaseCurrencyExchRate"></param>
        /// <param name="ATransactionInIntlCurrency"></param>
        public void RecalcTransactionsCurrencyAmounts(AGiftBatchRow ABatchRow,
            Decimal AIntlToBaseCurrencyExchRate,
            Boolean ATransactionInIntlCurrency)
        {
            int LedgerNumber = ABatchRow.LedgerNumber;
            int BatchNumber = ABatchRow.BatchNumber;
            decimal BatchExchangeRateToBase = ABatchRow.ExchangeRateToBase;

            LoadGiftDataForBatch(LedgerNumber, BatchNumber);

            DataView transDV = new DataView(FMainDS.AGiftDetail);
            transDV.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drvTrans in transDV)
            {
                AGiftDetailRow gdr = (AGiftDetailRow)drvTrans.Row;

                gdr.GiftAmount = GLRoutines.Divide(gdr.GiftTransactionAmount, BatchExchangeRateToBase);

                if (!ATransactionInIntlCurrency)
                {
                    gdr.GiftAmountIntl = (AIntlToBaseCurrencyExchRate == 0) ? 0 : GLRoutines.Divide(gdr.GiftAmount, AIntlToBaseCurrencyExchRate);
                }
                else
                {
                    gdr.GiftAmountIntl = gdr.GiftTransactionAmount;
                }
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

        // auto populate recipient info using the donor's last gift
        private void AutoPopulateGiftDetail(long APartnerKey)
        {
            AGiftTable GiftTable = new AGiftTable();
            GiftBatchTDSAGiftDetailTable GiftDetailTable = new GiftBatchTDSAGiftDetailTable();

            // check if the donor has another gift in this same batch
            foreach (AGiftRow GiftRow in FMainDS.AGift.Rows)
            {
                if ((GiftRow.DonorKey == APartnerKey)
                    && (GiftRow.GiftTransactionNumber != GetSelectedDetailRow().GiftTransactionNumber))
                {
                    GiftTable.Rows.Add((object[])GiftRow.ItemArray.Clone());
                }
            }

            // if the donor does have another gift then get the AGiftDetail records for the most recent gift
            if (GiftTable.Rows.Count > 0)
            {
                // find the most recent gift (probably the last gift in the table)
                AGiftRow LatestGiftRow = (AGiftRow)GiftTable.Rows[GiftTable.Rows.Count - 1];

                for (int i = GiftTable.Rows.Count - 2; i >= 0; i--)
                {
                    if (LatestGiftRow.DateEntered < ((AGiftRow)GiftTable.Rows[i]).DateEntered)
                    {
                        LatestGiftRow = (AGiftRow)GiftTable.Rows[i];
                    }
                }

                foreach (AGiftDetailRow GiftDetailRow in FMainDS.AGiftDetail.Rows)
                {
                    if ((GiftDetailRow.LedgerNumber == LatestGiftRow.LedgerNumber)
                        && (GiftDetailRow.BatchNumber == LatestGiftRow.BatchNumber)
                        && (GiftDetailRow.GiftTransactionNumber == LatestGiftRow.GiftTransactionNumber))
                    {
                        GiftDetailTable.Rows.Add((object[])GiftDetailRow.ItemArray.Clone());
                    }
                }
            }
            else
            {
                // if the donor does not have another gift in this gift batch then search the database for
                // the last gift from this donor
                GiftDetailTable = TRemote.MFinance.Gift.WebConnectors.LoadDonorLastGift(APartnerKey);
            }

            // if this is the donor's first ever gift
            if ((GiftDetailTable == null) || (GiftDetailTable.Rows.Count == 0))
            {
                // set FirstTimeGift field in AGift to true
                GiftBatchTDSAGiftDetailRow CurrentDetail = GetSelectedDetailRow();
                AGiftRow CurrentGift = (AGiftRow)FMainDS.AGift.Rows.Find(
                    new object[] { CurrentDetail.LedgerNumber, CurrentDetail.BatchNumber, CurrentDetail.GiftTransactionNumber });
                CurrentGift.FirstTimeGift = true;

                // add donor key to list so that new donor warning can be shown
                if (!FNewDonorsList.Contains(APartnerKey))
                {
                    FNewDonorsList.Add(APartnerKey);
                }
            }

            bool SplitGift = false;

            // if the last gift was a split gift (multiple details) then ask the user if they would like this new gift to also be split
            if ((GiftDetailTable != null) && (GiftDetailTable.Rows.Count > 1))
            {
                string Message = string.Format(Catalog.GetString(
                        "The last gift from this donor was a split gift.{0}{0}Here are the details:{0}"), "\n");
                int DetailNumber = 1;

                foreach (GiftBatchTDSAGiftDetailRow Row in GiftDetailTable.Rows)
                {
                    Message += DetailNumber + ")  ";

                    if (Row.RecipientKey > 0)
                    {
                        Message +=
                            string.Format(Catalog.GetString("Recipient: {0} ({1});  Motivation Group: {2};  Motivation Detail: {3};  Amount: {4}"),
                                Row.RecipientDescription, Row.RecipientKey, Row.MotivationGroupCode, Row.MotivationDetailCode,
                                StringHelper.FormatUsingCurrencyCode(Row.GiftTransactionAmount, GetCurrentBatchRow().CurrencyCode) +
                                " " + FBatchRow.CurrencyCode) +
                            "\n";
                    }

                    DetailNumber++;
                }

                Message += "\n" + Catalog.GetString("Do you want to create the same split gift again?");

                SplitGift = MessageBox.Show(Message, Catalog.GetString("Create Split Gift"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes;
            }

            if ((GiftDetailTable != null) && (GiftDetailTable.Rows.Count > 0))
            {
                int CurrentTransaction = 0;

                while (true)
                {
                    // populate gift detail
                    txtDetailRecipientKey.Text = String.Format("{0:0000000000}", GiftDetailTable[CurrentTransaction].RecipientKey);
                    cmbDetailMotivationGroupCode.SetSelectedString(GiftDetailTable[CurrentTransaction].MotivationGroupCode);
                    cmbDetailMotivationDetailCode.SetSelectedString(GiftDetailTable[CurrentTransaction].MotivationDetailCode);
                    txtDetailGiftCommentOne.Text = GiftDetailTable[CurrentTransaction].GiftCommentOne;
                    cmbDetailCommentOneType.SetSelectedString(GiftDetailTable[CurrentTransaction].CommentOneType, -1);
                    txtDetailGiftCommentTwo.Text = GiftDetailTable[CurrentTransaction].GiftCommentTwo;
                    cmbDetailCommentTwoType.SetSelectedString(GiftDetailTable[CurrentTransaction].CommentTwoType, -1);
                    txtDetailGiftCommentThree.Text = GiftDetailTable[CurrentTransaction].GiftCommentThree;
                    cmbDetailCommentThreeType.SetSelectedString(GiftDetailTable[CurrentTransaction].CommentThreeType, -1);
                    chkDetailConfidentialGiftFlag.Checked = GiftDetailTable[CurrentTransaction].ConfidentialGiftFlag;
                    chkDetailChargeFlag.Checked = GiftDetailTable[CurrentTransaction].ChargeFlag;
                    chkDetailTaxDeductible.Checked = GiftDetailTable[CurrentTransaction].TaxDeductible;
                    cmbDetailMailingCode.SetSelectedString(GiftDetailTable[CurrentTransaction].MailingCode, -1);
                    KeyMinistryChanged(this, null);

                    if (SplitGift)
                    {
                        // only populate amount if a split gift
                        txtDetailGiftTransactionAmount.NumberValueDecimal = GiftDetailTable[CurrentTransaction].GiftTransactionAmount;
                        CurrentTransaction++;

                        // if there are more details that are part of this gift
                        if (CurrentTransaction < GiftDetailTable.Rows.Count)
                        {
                            // clear previous validation errors.
                            // otherwise we get an error if the user has changed the control immediately after changing the donor key.
                            FPetraUtilsObject.VerificationResultCollection.Clear();

                            // create a new gift detail
                            CreateANewGift(false);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void OpenDonorHistory(System.Object sender, EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(true,
                Convert.ToInt64(txtDetailDonorKey.Text),
                FPetraUtilsObject.GetForm());
        }

        private void OpenRecipientHistory(System.Object sender, EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(false,
                Convert.ToInt64(txtDetailRecipientKey.Text),
                FPetraUtilsObject.GetForm());
        }

        private void OpenGiftDestination(System.Object sender, EventArgs e)
        {
            if (txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY)
            {
                TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(
                    FPetraUtilsObject.GetForm(), FPreviouslySelectedDetailRow.RecipientKey);
                GiftDestinationForm.Show();
            }
        }

        private void ToggleTaxDeductible(Object sender, EventArgs e)
        {
            if (FTaxDeductiblePercentageEnabled)
            {
                EnableOrDiasbleTaxDeductibilityPct(chkDetailTaxDeductible.Checked);
                UpdateTaxDeductiblePct(Convert.ToInt64(txtDetailRecipientKey.Text), false);
            }
        }

        // modifies menu items depending on the Recipeint's Partner class
        private void RecipientPartnerClassChanged(TPartnerClass ? APartnerClass)
        {
            bool? DoEnableRecipientGiftDestination;

            TUC_GiftTransactions_Recipient.OnRecipientPartnerClassChanged(APartnerClass, txtDetailRecipientKey, txtDetailRecipientLedgerNumber,
                out DoEnableRecipientGiftDestination);

            if (DoEnableRecipientGiftDestination.HasValue)
            {
                mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
            }

            //string ItemText = Catalog.GetString("Open Recipient Gift Destination");

            //if ((APartnerClass == TPartnerClass.UNIT) || (APartnerClass == null))
            //{
            //    txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
            //    txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, false);
            //    mniRecipientGiftDestination.Enabled = false;
            //}
            //else if (APartnerClass == TPartnerClass.FAMILY)
            //{
            //    txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
            //    txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, true);
            //    mniRecipientGiftDestination.Enabled = true;
            //}
        }

        /// <summary>
        /// Update Gift Destination based on a broadcast message
        /// </summary>
        /// <param name="AFormsMessage"></param>
        public void ProcessGiftDetainationBroadcastMessage(TFormsMessage AFormsMessage)
        {
            if (Convert.ToInt64(txtDetailRecipientKey.Text) == ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).PartnerKey)
            {
                txtDetailRecipientLedgerNumber.Text = "0";

                foreach (PPartnerGiftDestinationRow Row in ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).
                         GiftDestinationTable.Rows)
                {
                    DateTime GiftDate = FPreviouslySelectedDetailRow.DateEntered;

                    // check if record is active for the Gift Date
                    if ((Row.DateEffective <= GiftDate)
                        && ((Row.DateExpires >= GiftDate) || Row.IsDateExpiresNull())
                        && (Row.DateEffective != Row.DateExpires))
                    {
                        txtDetailRecipientLedgerNumber.Text = Row.FieldKey.ToString();
                    }
                }
            }
        }
    }
}