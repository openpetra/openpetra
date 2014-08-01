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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
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
        private bool FInEditMode = false;
        private bool FShowingDetails = false;

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
        
        // this should be updated each time  txtField is updated to prevent problems at validation
        private Int64 FCorrespondingRecipientKeyToField = 0;

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
        }

        private void InitialiseControls()
        {
            //Fix to length of field
            txtDetailReference.MaxLength = 20;

            //Fix a layering issue
            txtField.SendToBack();

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
            txtGiftReceipting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtGiftReceipting.Font = TAppSettingsManager.GetDefaultBoldFont();
        }

        private void SetupTextBoxMenuItems()
        {
            List <Tuple <string, EventHandler>>ItemList = new List <Tuple <string, EventHandler>>();

            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Donor History"), OpenDonorHistory));
            txtDetailDonorKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient History"), OpenRecipientHistory));
            ItemList.Add(new Tuple<string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtDetailRecipientKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple<string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtField.AddCustomContextMenuItems(ItemList);
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

            SetTextBoxOverlayOnKeyMinistryCombo();
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
            SetKeyMinistryTextBoxInvisible();
        }

        private void EndEditMode(object sender, EventArgs e)
        {
            FInEditMode = false;

            if (!txtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo();
            }
        }

        private void SetTextBoxOverlayOnKeyMinistryCombo(bool AReadComboValue = false)
        {
            ResetMotivationDetailCodeFilter();

            txtDetailRecipientKeyMinistry.Visible = true;
            txtDetailRecipientKeyMinistry.BringToFront();
            txtDetailRecipientKeyMinistry.Parent.Refresh();

            if (AReadComboValue)
            {
                ReconcileKeyMinistryFromCombo();
            }
            else
            {
                ReconcileKeyMinistryFromTextbox();
            }
        }

        private void SetKeyMinistryTextBoxInvisible()
        {
            if (txtDetailRecipientKeyMinistry.Visible)
            {
                ApplyMotivationDetailCodeFilter();

                PopulateKeyMinistry();

                ReconcileKeyMinistryFromTextbox();

                //hide the overlay box during editing
                txtDetailRecipientKeyMinistry.Visible = false;
            }
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public void ReconcileKeyMinistryFromCombo()
        {
            string KeyMinistry = string.Empty;
            bool EmptyRow = (FPreviouslySelectedDetailRow == null);

            if (FBatchUnposted && FInEditMode)
            {
                if (!EmptyRow && (cmbKeyMinistries.SelectedIndex > -1))
                {
                    KeyMinistry = cmbKeyMinistries.GetSelectedDescription();
                }

                txtDetailRecipientKeyMinistry.Text = KeyMinistry;
            }
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public void ReconcileKeyMinistryFromTextbox()
        {
            string KeyMinistry = string.Empty;
            bool EmptyRow = (FPreviouslySelectedDetailRow == null);

            if (FBatchUnposted && FInEditMode)
            {
                if (!EmptyRow && (txtDetailRecipientKeyMinistry.Text.Length > 0))
                {
                    KeyMinistry = txtDetailRecipientKeyMinistry.Text;
                    cmbKeyMinistries.SetSelectedString(KeyMinistry);
                }
                else
                {
                    cmbKeyMinistries.SelectedIndex = -1;
                }
            }
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
                EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);
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

            FTransactionsLoaded = true;
            return true;
        }

        /// <summary>
        /// Ensure the data is loaded for the specified batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>If transactions exist</returns>
        public Boolean EnsureGiftDataPresent(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            DataView TransDV = new DataView(FMainDS.AGiftDetail);

            TransDV.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            if (TransDV.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadTransactions(ALedgerNumber, ABatchNumber));

                UpdateAllRecipientDescriptions(ABatchNumber);

                ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
            }

            return TransDV.Count > 0;
        }

        private void UpdateAllRecipientDescriptions(Int32 ABatchNumber)
        {
            DataView giftDetailView = new DataView(FMainDS.AGiftDetail);

            giftDetailView.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView rv in giftDetailView)
            {
                GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                if (row.RecipientKey == 0)
                {
                    row.RecipientDescription = row.MotivationDetailCode;
                }
            }
        }

        private void FindCostCentreCodeForRecipient(AGiftDetailRow ARow, Int64 APartnerKey, bool AShowError = false)
        {
            if (ARow == null)
            {
                return;
            }

            string CurrentCostCentreCode = ARow.CostCentreCode;
            string NewCostCentreCode = string.Empty;

            Int64 RecipientField = Convert.ToInt64(txtField.Text);

            string MotivationGroup = ARow.MotivationGroupCode;
            string MotivationDetail = ARow.MotivationDetailCode;

            Int64 RecipientLedgerNumber = ARow.RecipientLedgerNumber;

            Int64 LedgerPartnerKey = FMainDS.ALedger[0].PartnerKey;

            bool KeyMinIsActive = false;
            bool KeyMinExists = TRemote.MFinance.Gift.WebConnectors.KeyMinistryExists(APartnerKey, out KeyMinIsActive);

            string ValidLedgerNumberCostCentreCode;

            string errMsg = string.Empty;

            if (TRemote.MFinance.Gift.WebConnectors.CheckCostCentreDestinationForRecipient(ARow.LedgerNumber, APartnerKey, RecipientField,
                    out ValidLedgerNumberCostCentreCode)
                || TRemote.MFinance.Gift.WebConnectors.CheckCostCentreDestinationForRecipient(ARow.LedgerNumber, RecipientLedgerNumber,
                    RecipientField,
                    out ValidLedgerNumberCostCentreCode))
            {
                NewCostCentreCode = ValidLedgerNumberCostCentreCode;
            }
            else if ((RecipientLedgerNumber != LedgerPartnerKey) && ((MotivationGroup == MFinanceConstants.MOTIVATION_GROUP_GIFT) || KeyMinExists))
            {
                errMsg = String.Format(
                    "Error in extracting Cost Centre Code for Recipient: {0} in Ledger: {1}.{2}{2}(Recipient Ledger Number: {3}, Ledger Partner Key: {4})",
                    APartnerKey,
                    FLedgerNumber,
                    Environment.NewLine,
                    RecipientLedgerNumber,
                    LedgerPartnerKey);

                if (AShowError)
                {
                    MessageBox.Show(errMsg,
                        "Cost Centre Code Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                else
                {
                    TLogging.Log("Cost Centre Code Error: " + errMsg);
                }
            }
            else
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                    new object[] { FLedgerNumber, MotivationGroup, MotivationDetail });

                if (motivationDetail != null)
                {
                    NewCostCentreCode = motivationDetail.CostCentreCode;
                }
                else
                {
                    errMsg = String.Format(
                        "Error in extracting Cost Centre Code for Motivation Group: {0} and Motivation Detail: {1} in Ledger: {2}.",
                        MotivationGroup,
                        MotivationDetail,
                        FLedgerNumber);

                    if (AShowError)
                    {
                        MessageBox.Show(errMsg,
                            "Cost Centre Code Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        TLogging.Log("Cost Centre Code Error: " + errMsg);
                    }
                }
            }

            if (CurrentCostCentreCode != NewCostCentreCode)
            {
                ARow.CostCentreCode = NewCostCentreCode;
            }
        }

        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (FInRecipientKeyChanging || FPetraUtilsObject.SuppressChangeDetection || FShowingDetails)
            {
                return;
            }

            FInRecipientKeyChanging = true;
            txtDetailRecipientKeyMinistry.Text = string.Empty;

            try
            {
                FPreviouslySelectedDetailRow.RecipientKey = Convert.ToInt64(APartnerKey);
                FPreviouslySelectedDetailRow.RecipientDescription = APartnerShortName;

                FPetraUtilsObject.SuppressChangeDetection = true;

                //Set RecipientLedgerNumber
                if (APartnerKey > 0)
                {
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey);
                }
                else
                {
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber = 0;
                }

                if (TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetail(
                        APartnerKey, ref FMotivationGroup, ref FMotivationDetail))
                {
                    if (FMotivationDetail.Equals(MFinanceConstants.GROUP_DETAIL_KEY_MIN))
                    {
                        cmbDetailMotivationDetailCode.SetSelectedString(MFinanceConstants.GROUP_DETAIL_KEY_MIN);
                    }
                }

                if (!FInKeyMinistryChanging)
                {
                    GetRecipientData(APartnerKey);
            		ValidateGiftDestination();
                }

                if (APartnerKey > 0)
                {
                    RetrieveRecipientCostCentreCode(APartnerKey);
                    mniRecipientHistory.Enabled = true;
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey);
                    RetrieveMotivationDetailCostCentreCode();
                    mniRecipientHistory.Enabled = false;
                }
            }
            finally
            {
                FInRecipientKeyChanging = false;
                ReconcileKeyMinistryFromCombo();
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private void UpdateRecipientKeyText(Int64 APartnerKey)
        {
            if ((APartnerKey == 0) && (FPreviouslySelectedDetailRow != null))
            {
                FPreviouslySelectedDetailRow.RecipientDescription = cmbDetailMotivationDetailCode.GetSelectedString();
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
            else if (FShowingDetails || (APartnerKey == 0))
            {
                mniDonorHistory.Enabled = false;
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

                        AutoPopulateGiftDetail(APartnerKey);
                        ShowReceiptFrequency(APartnerKey);

                        mniDonorHistory.Enabled = true;
                    }
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
            string KeyMinistry = cmbKeyMinistries.GetSelectedDescription();
            string RecipientKey = cmbKeyMinistries.GetSelectedInt64().ToString();

            if ((FPreviouslySelectedDetailRow == null) || FInKeyMinistryChanging || FInRecipientKeyChanging
                || FPetraUtilsObject.SuppressChangeDetection || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            try
            {
                FInKeyMinistryChanging = true;

                if (cmbKeyMinistries.Count == 0)
                {
                    cmbKeyMinistries.SelectedIndex = -1;
                    txtDetailRecipientKeyMinistry.Text = string.Empty;
                }
                else
                {
                    txtDetailRecipientKeyMinistry.Text = KeyMinistry;
                    FPreviouslySelectedDetailRow.RecipientKeyMinistry = KeyMinistry;
                    txtDetailRecipientKey.Text = RecipientKey;
                }
            }
            finally
            {
                FInKeyMinistryChanging = false;
            }
        }

        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            if (!FBatchUnposted || FPetraUtilsObject.SuppressChangeDetection || !FInEditMode || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            FMotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            FMotivationDetail = string.Empty;

            ApplyMotivationDetailCodeFilter();
            
            ValidateGiftDestination();
        }

        private void ApplyMotivationDetailCodeFilter()
        {
            //FMotivationbDetail will change by next process
            string motivationDetail = FMotivationDetail;

            ResetMotivationDetailCodeFilter();
            TFinanceControls.ChangeFilterMotivationDetailList(ref cmbDetailMotivationDetailCode, FMotivationGroup);
            FMotivationDetail = motivationDetail;

            if (FMotivationDetail.Length > 0)
            {
                cmbDetailMotivationDetailCode.SetSelectedString(FMotivationDetail);
                cmbDetailMotivationDetailCode.Text = FMotivationDetail;
            }
            else if (cmbDetailMotivationDetailCode.Count > 0)
            {
                cmbDetailMotivationDetailCode.SelectedIndex = 0;

                //Force refresh of label
                MotivationDetailChanged(null, null);
            }
            else
            {
                cmbDetailMotivationDetailCode.SelectedIndex = -1;
                //Force refresh of label
                MotivationDetailChanged(null, null);
            }

            RetrieveMotivationDetailAccountCode();

            if ((txtDetailRecipientKey.Text == string.Empty) || (Convert.ToInt64(txtDetailRecipientKey.Text) == 0))
            {
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                RetrieveMotivationDetailCostCentreCode();
            }
        }

        private void ResetMotivationDetailCodeFilter()
        {
            if ((cmbDetailMotivationDetailCode.Count == 0) && (cmbDetailMotivationDetailCode.Filter != null)
                && (!cmbDetailMotivationDetailCode.Filter.Contains("1 = 2")))
            {
                FMotivationDetail = string.Empty;
                cmbDetailMotivationDetailCode.RefreshLabel();

                if (FActiveOnly)
                {
                    //This is needed as the code in TFinanceControls.ChangeFilterMotivationDetailList looks for presence of the active only prefix
                    cmbDetailMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true And 1 = 2";
                }
                else
                {
                    cmbDetailMotivationDetailCode.Filter = "1 = 2";
                }

                return;
            }

            if (cmbDetailMotivationDetailCode.Count > 0)
            {
                FMotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();
            }

            if (FActiveOnly)
            {
                cmbDetailMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true";
            }
            else
            {
                cmbDetailMotivationDetailCode.Filter = string.Empty;
            }

            cmbDetailMotivationDetailCode.SetSelectedString(FMotivationDetail);
            cmbDetailMotivationDetailCode.RefreshLabel();
        }

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        private void RetrieveMotivationDetailAccountCode()
        {
            string AcctCode = string.Empty;

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                    new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

                if (motivationDetail != null)
                {
                    AcctCode = motivationDetail.AccountCode.ToString();
                }
            }

            txtDetailAccountCode.Text = AcctCode;
        }

        private void RetrieveMotivationDetailCostCentreCode()
        {
            string CostCentreCode = string.Empty;

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                    new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

                if (motivationDetail != null)
                {
                    CostCentreCode = motivationDetail.CostCentreCode.ToString();
                }
            }

            txtDetailCostCentreCode.Text = CostCentreCode;
        }

        private void RetrieveRecipientCostCentreCode(Int64 APartnerKey)
        {
            //string FailedUpdates = string.Empty;

            if (FInKeyMinistryChanging || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            //UpdateCostCentreCodeForRecipients(out FailedUpdates,
            //    FPreviouslySelectedDetailRow.GiftTransactionNumber,
            //    FPreviouslySelectedDetailRow.DetailNumber);

            //((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors();

            FindCostCentreCodeForRecipient(FPreviouslySelectedDetailRow, APartnerKey, false);

            txtDetailCostCentreCode.Text = FPreviouslySelectedDetailRow.CostCentreCode;
        }

        private bool UpdateCostCentreCodeForRecipients(out string AFailedUpdates,
            Int32 AGiftTransactionNumber = 0,
            Int32 AGiftDetailNumber = 0)
        {
            AFailedUpdates = string.Empty;

            if ((FMainDS.AGiftBatch.Count == 0) || (FMainDS.AGift.Count == 0))
            {
                return true;
            }

            Int64 LedgerPartnerKey = FMainDS.ALedger[0].PartnerKey;

            string CurrentCostCentreCode = string.Empty;
            string NewCostCentreCode = string.Empty;

            string MotivationGroup = string.Empty;
            string MotivationDetail = string.Empty;

            Int64 PartnerKey = 0;
            Int64 RecipientFundNumber = 0;

            bool KeyMinIsActive = false;
            bool IsKeyMinistry = false;

            string ValidLedgerNumberCostCentreCode = string.Empty;
            //bool ValidLedgerNumberExists = false;

            string ErrMsg = string.Empty;

            string RowFilterForGifts = string.Empty;

            if (AGiftTransactionNumber > 0)
            {
                RowFilterForGifts = String.Format("{0}={1} And {2}={3} And {4}={5}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftTransactionNumber,
                    AGiftDetailTable.GetDetailNumberDBName(),
                    AGiftDetailNumber);
            }
            else
            {
                RowFilterForGifts = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
            }

            DataView giftRowsView = new DataView(FMainDS.AGiftDetail);
            giftRowsView.RowFilter = RowFilterForGifts;

            foreach (DataRowView dvRows in giftRowsView)
            {
                AGiftDetailRow giftDetailRow = (AGiftDetailRow)dvRows.Row;

                AGiftRow giftRow = GetGiftRow(giftDetailRow.GiftTransactionNumber);

                CurrentCostCentreCode = giftDetailRow.CostCentreCode;
                NewCostCentreCode = CurrentCostCentreCode;

                MotivationGroup = giftDetailRow.MotivationGroupCode;
                MotivationDetail = giftDetailRow.MotivationDetailCode;

                PartnerKey = giftDetailRow.RecipientKey;
                RecipientFundNumber = giftDetailRow.RecipientLedgerNumber;

                KeyMinIsActive = false;
                IsKeyMinistry = TRemote.MFinance.Gift.WebConnectors.KeyMinistryExists(PartnerKey, out KeyMinIsActive);

                //ValidLedgerNumberExists = CheckCostCentreLinkForRecipient(LedgerNumber,
                //    PartnerKey,
                //    out ValidLedgerNumberCostCentreCode);

                Int64 RecipientField = Convert.ToInt64(txtField.Text);

                if (TRemote.MFinance.Gift.WebConnectors.CheckCostCentreDestinationForRecipient(giftRow.LedgerNumber, PartnerKey, RecipientField,
                        out ValidLedgerNumberCostCentreCode)
                    || TRemote.MFinance.Gift.WebConnectors.CheckCostCentreDestinationForRecipient(giftRow.LedgerNumber, RecipientFundNumber,
                        RecipientField,
                        out ValidLedgerNumberCostCentreCode))
                {
                    NewCostCentreCode = ValidLedgerNumberCostCentreCode;
                }
                else if ((RecipientFundNumber != LedgerPartnerKey) && ((MotivationGroup == MFinanceConstants.MOTIVATION_GROUP_GIFT) || IsKeyMinistry))
                {
                    ErrMsg = String.Format(
                        "Error in extracting Cost Centre Code for Recipient: {0} in Ledger: {1}.{2}{2}(Recipient Ledger Number: {3}, Ledger Partner Key: {4})",
                        PartnerKey,
                        FLedgerNumber,
                        Environment.NewLine,
                        RecipientFundNumber,
                        LedgerPartnerKey);

                    TLogging.Log("Cost Centre Code Error: " + ErrMsg);
                }
                else
                {
                    AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                        new object[] { FLedgerNumber, MotivationGroup, MotivationDetail });

                    if (motivationDetail != null)
                    {
                        NewCostCentreCode = motivationDetail.CostCentreCode.ToString();
                    }
                    else
                    {
                        ErrMsg = String.Format(
                            "Error in extracting Cost Centre Code for Motivation Group: {0} and Motivation Detail: {1} in Ledger: {2}.",
                            MotivationGroup,
                            MotivationDetail,
                            FLedgerNumber);

                        TLogging.Log("Cost Centre Code Error: " + ErrMsg);
                    }
                }

                if (CurrentCostCentreCode != NewCostCentreCode)
                {
                    giftDetailRow.CostCentreCode = NewCostCentreCode;
                }

                if (ErrMsg.Length > 0)
                {
                    if (AFailedUpdates.Length > 0)
                    {
                        AFailedUpdates += (Environment.NewLine + Environment.NewLine);
                    }

                    AFailedUpdates += ErrMsg;
                    ErrMsg = string.Empty;
                }
            }

            return AFailedUpdates.Length == 0;
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            if (!FBatchUnposted || !FInEditMode || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            FMotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                    new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

                cmbDetailMotivationDetailCode.RefreshLabel();

                if (motivationDetail != null)
                {
                    RetrieveMotivationDetailAccountCode();
                }
            }

            long PartnerKey = 0;
            Int64.TryParse(txtDetailRecipientKey.Text, out PartnerKey);

            if (PartnerKey > 0)
            {
                PopulateKeyMinistry(PartnerKey);
            }
            else
            {
                RetrieveMotivationDetailCostCentreCode();
                UpdateRecipientKeyText(0);
            }
        }

        private void PopulateKeyMinistry(long APartnerKey = 0)
        {
            cmbKeyMinistries.Clear();

            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

                if (APartnerKey == 0)
                {
                    return;
                }
            }

            GetRecipientData(APartnerKey);
        }

        private void GetRecipientData(long APartnerKey)
        {
            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);
            }

            TFinanceControls.GetRecipientData(ref cmbKeyMinistries, ref txtField, APartnerKey, true);

            if (Convert.ToInt64(txtField.Text) == 0 && APartnerKey != 0)
            {
                txtField.Text = TRemote.MFinance.Gift.WebConnectors.GetGiftDestinationForRecipient(APartnerKey,
                    FPreviouslySelectedDetailRow.DateEntered).ToString();
            }
            
            FCorrespondingRecipientKeyToField = APartnerKey;
        }

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if ((GetCurrentBatchRow() == null) || (txn.NumberValueDecimal == null))
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow != null) && (GetCurrentBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED))
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

        private bool PreDeleteManual(GiftBatchTDSAGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            FGift = GetGiftRow(ARowToDelete.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                AGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.AGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;
            FGiftDetailView.Sort = AGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftTransactionAmount, GetCurrentBatchRow().CurrencyCode);

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
                    String.Format(Catalog.GetString("Gift no. {0} in Batch no. {1} has no detail rows in the Gift Detail table!"),
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                allowDeletion = false;
            }

            return allowDeletion;
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

            // temporarily disable  New Donor Warning
            ((TFrmGiftBatch) this.ParentForm).NewDonorWarning = false;

            if ((ARowToDelete.RowState != DataRowState.Added) && !((TFrmGiftBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current gift detail!");
                return deletionSuccessful;
            }

            ((TFrmGiftBatch) this.ParentForm).NewDonorWarning = true;

            //Backup the Dataset for reversion purposes
            GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();

            int selectedDetailNumber = ARowToDelete.DetailNumber;
            int giftToDeleteTransNo = 0;
            string filterAllGiftsOfBatch = String.Empty;
            string filterAllGiftDetailsOfBatch = String.Empty;

            int detailRowCount = FGiftDetailView.Count;

            try
            {
                if (ARowToDelete.ModifiedDetailKey != null)
                {
                    originatingDetailRef = ARowToDelete.ModifiedDetailKey;
                }

                //Delete current detail row
                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete gift header row
                if (detailRowCount > 1)
                {
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
                    giftToDeleteTransNo = FGift.GiftTransactionNumber;

                    TLogging.Log("Delete row: " + giftToDeleteTransNo.ToString());

                    // Reduce all Gift Detail row Transaction numbers by 1 if they are greater then gift to be deleted
                    filterAllGiftDetailsOfBatch = String.Format("{0}={1} And {2}>{3}",
                        AGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftDetailTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftDetailView = new DataView(FMainDS.AGiftDetail);
                    giftDetailView.RowFilter = filterAllGiftDetailsOfBatch;
                    giftDetailView.Sort = String.Format("{0} ASC", AGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the gift row to be deleted to the top
                    filterAllGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        AGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        AGiftTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftView = new DataView(FMainDS.AGift);
                    giftView.RowFilter = filterAllGiftsOfBatch;
                    giftView.Sort = String.Format("{0} ASC", AGiftTable.GetGiftTransactionNumberDBName());

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
                                if (!giftRowToCopyDown.Table.Columns[j].ColumnName.EndsWith("_text"))
                                {
                                    giftRowToReceive[j] = giftRowToCopyDown[j];
                                }
                            }
                        }

                        if (currentGiftTransNo == FBatchRow.LastGiftNumber)
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

                    FBatchRow.LastGiftNumber--;
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
                    //Clear current batch's gift data and reload from server
                    if (RefreshCurrentBatchGiftData(FBatchNumber))
                    {
                        ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
                    }
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
                FFilterAndFindObject.ApplyFilter();
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

        /// <summary>
        /// Clear the gift data of the current batch without marking records for delete
        /// </summary>
        private bool RefreshCurrentBatchGiftData(Int32 ABatchNumber)
        {
            bool RetVal = false;

            //Copy and backup the current dataset
            GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();
            GiftBatchTDS BackupDS = (GiftBatchTDS)FMainDS.Copy();

            try
            {
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
                FMainDS = BackupDS;

                string errMsg = Catalog.GetString("Error trying to clear current Batch data: /n/r/n/r" + ex.Message);
                MessageBox.Show(errMsg, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                txtField.Text = string.Empty;
                FCorrespondingRecipientKeyToField = 0;
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
        /// Creates a new gift or gift detail depending upon the parameter
        /// </summary>
        /// <param name="ACompletelyNewGift"></param>
        private void CreateANewGift(bool ACompletelyNewGift)
        {
            AGiftRow CurrentGiftRow = null;
            bool IsEmptyGrid = (grdDetails.Rows.Count == 1);

            if (ValidateAllData(true, true))
            {
                if (!ACompletelyNewGift)  //i.e. a gift detail
                {
                    ACompletelyNewGift = IsEmptyGrid;
                }

                if (ACompletelyNewGift)
                {
                    //Run this if a new gift is requested or required.

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
                }

                newRow.DateEntered = CurrentGiftRow.DateEntered;

                FMainDS.AGiftDetail.Rows.Add(newRow);

                FPetraUtilsObject.SetChangedFlag();

                if (!SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1))
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

                btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
                UpdateRecordNumberDisplay();

                //Focus accordingly
                if (ACompletelyNewGift)
                {
                    txtDetailDonorKey.Focus();
                }
                else
                {
                    txtDetailRecipientKey.Focus();
                }

                //Set the default motivation Group. This needs to happen after focus has returned
                //  to the pnlDetails to ensure FInEditMode is correct.
                cmbDetailMotivationGroupCode.SelectedIndex = 0;
                UpdateRecipientKeyText(0);
                cmbKeyMinistries.Clear();
                mniRecipientHistory.Enabled = false;
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
            if (!txtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(true);
            }
            else if (!FTransactionsLoaded)
            {
                SetTextBoxOverlayOnKeyMinistryCombo();
            }

            if (ARow == null)
            {
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                FShowingDetails = true;

                //Record current values for motivation
                FMotivationGroup = ARow.MotivationGroupCode;
                FMotivationDetail = ARow.MotivationDetailCode;

                //Show gift table values
                AGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
                ShowDetailsForGift(giftRow);

                if (ARow.IsCostCentreCodeNull())
                {
                    txtDetailCostCentreCode.Text = string.Empty;
                }
                else
                {
                    txtDetailCostCentreCode.Text = ARow.CostCentreCode;
                }

                if (ARow.IsAccountCodeNull())
                {
                    txtDetailAccountCode.Text = string.Empty;
                }
                else
                {
                    txtDetailAccountCode.Text = ARow.AccountCode;
                }

                if (ARow.IsRecipientKeyNull())
                {
                    txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                    UpdateRecipientKeyText(0);
                }
                else
                {
                    txtDetailRecipientKey.Text = String.Format("{0:0000000000}", ARow.RecipientKey);
                    UpdateRecipientKeyText(ARow.RecipientKey);
                }

                if (ARow.IsRecipientFieldNull())
                {
                    txtField.Text = string.Empty;
                    FCorrespondingRecipientKeyToField = 0;
                }
                else
                {
                    txtField.Text = ARow.RecipientField.ToString();
                    FCorrespondingRecipientKeyToField = ARow.RecipientField;
                }
                
                if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
                {
                    mniRecipientHistory.Enabled = false;
                    RecipientPartnerClassChanged(null);
                }
                else
                {
                	mniRecipientHistory.Enabled = true;
                
	                if (Convert.ToInt64(txtField.Text) == 0)
	                {
	                    RecipientPartnerClassChanged(null);
	                }
                }

                ShowReceiptFrequency(Convert.ToInt64(txtDetailDonorKey.Text));

                UpdateControlsProtection(ARow);
            }
            finally
            {
                FShowingDetails = false;
                this.Cursor = Cursors.Default;
            }
        }

        // dispalays information about the donor's receipt frequency options
        private void ShowReceiptFrequency(long APartnerKey)
        {
            txtGiftReceipting.Text = "";

            if (APartnerKey == 0)
            {
                return;
            }

            // find PPartnerRow from dataset
            PPartnerRow DonorRow = (PPartnerRow)FMainDS.DonorPartners.Rows.Find(new object[] { APartnerKey });

            // if PPartnerRow cannot be found load it from db
            if (DonorRow == null)
            {
                DonorRow = (PPartnerRow)TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey).Rows[0];
            }

            if (DonorRow.ReceiptEachGift)
            {
                txtGiftReceipting.Text = "*" + Catalog.GetString("Receipt Each Gift") + "*";
            }

            if (!string.IsNullOrEmpty(DonorRow.ReceiptLetterFrequency))
            {
                if (DonorRow.ReceiptEachGift)
                {
                    txtGiftReceipting.Text += "; ";
                }

                txtGiftReceipting.Text += DonorRow.ReceiptLetterFrequency + " " + Catalog.GetString("Receipt");
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
                UpdateRecipientKeyText(ARow.RecipientKey);
            }

            if (txtField.Text.Length == 0)
            {
                ARow.SetRecipientFieldNull();
            }
            else
            {
                ARow.RecipientField = Convert.ToInt64(txtField.Text);
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
        }

        private void ValidateDataDetailsManual(AGiftDetailRow ARow)
        {
            if ((ARow == null) || (GetCurrentBatchRow() == null) || (GetCurrentBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                || (GetCurrentBatchRow().BatchNumber != ARow.BatchNumber))
            {
                return;
            }
        	
        	// this happens if validation is called after recipient key has been changed but before RecipientKeyChanged is called,
        	// meaning that the Field has not yet been updated
        	if (ARow.RecipientKey != FCorrespondingRecipientKeyToField)
        	{
        		GetRecipientData(Convert.ToInt64(txtDetailRecipientKey.Text));
        		FPreviouslySelectedDetailRow.RecipientField = Convert.ToInt64(txtField.Text);
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
        }
        
        private void ValidateGiftDestination()
        {
        	// if no gift destination exists for parter then give the user the option to open Gift Destination maintenance screen
        	if (Convert.ToInt64(txtField.Text) == 0 && FPreviouslySelectedDetailRow.RecipientKey != 0
        	    && cmbDetailMotivationGroupCode.GetSelectedString() == MFinanceConstants.MOTIVATION_GROUP_GIFT &&
        	    MessageBox.Show(Catalog.GetString("No valid Gift Destination exists for ")
        	                      	+ FPreviouslySelectedDetailRow.RecipientDescription
        	                      	+ " (" + FPreviouslySelectedDetailRow.RecipientKey + ").\n\n"
        	                      	+ string.Format(Catalog.GetString("A Gift Destination will need to be assigned to this Partner before"
        	                      		+ " this gift can be saved with the Motivation Group '{0}'."
								  	+ " Would you like to do this now?"), MFinanceConstants.MOTIVATION_GROUP_GIFT),
                              	Catalog.GetString("No valid Gift Destination"),
                              	MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

            if (!EnsureGiftDataPresent(LedgerNumber, CurrentBatchNumber))
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

            EnsureGiftDataPresent(LedgerNumber, BatchNumber);

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

                    //clear any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection();

                    //Clear out the gift data for the current batch without marking the records for deletion
                    //  and then reload from server
                    if (RefreshCurrentBatchGiftData(FBatchNumber))
                    {
                        ((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
                    }

                    //Now delete all gift data for current batch
                    DeleteCurrentBatchGiftData(FBatchNumber);

                    FBatchRow.BatchTotal = 0;

                    // Be sure to set the last gift number in the parent table before saving all the changes
                    SetBatchLastGiftNumber();

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

        private void DeleteCurrentBatchGiftData(Int32 ABatchNumber)
        {
            DataView giftDetailView = new DataView(FMainDS.AGiftDetail);

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

            DataView giftView = new DataView(FMainDS.AGift);

            giftView.RowFilter = String.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftView.Sort = String.Format("{0} DESC",
                AGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in giftView)
            {
                dr.Delete();
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
        
        // modifies menu items depending on the Recipeint's Partner class
        private void RecipientPartnerClassChanged(TPartnerClass? APartnerClass)
        {
        	string ItemText = Catalog.GetString("Open Recipient Gift Destination");
        		
        	if (APartnerClass == TPartnerClass.UNIT || APartnerClass == null)
        	{
            	txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
            	txtField.CustomContextMenuItemsVisibility(ItemText, false);
            	mniRecipientGiftDestination.Enabled = false;
        	}
        	else if (APartnerClass == TPartnerClass.FAMILY)
        	{
        		txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
            	txtField.CustomContextMenuItemsVisibility(ItemText, true);
            	mniRecipientGiftDestination.Enabled = true;
        	}
        }
        
        /// <summary>
        /// Update Gift Destination based on a broadcast message
        /// </summary>
        /// <param name="AFormsMessage"></param>
        public void ProcessGiftDetainationBroadcastMessage(TFormsMessage AFormsMessage)
        {
        	if (Convert.ToInt64(txtDetailRecipientKey.Text) == ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).PartnerKey)
            {
                foreach (PPartnerGiftDestinationRow Row in ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).GiftDestinationTable.Rows)
                {
                    // check if record is active for today
                    if ((Row.DateEffective <= DateTime.Today)
                        && ((Row.DateExpires >= DateTime.Today) || Row.IsDateExpiresNull())
                        && (Row.DateEffective != Row.DateExpires))
                    {
                    	txtField.Text = Row.FieldKey.ToString();
                    	FCorrespondingRecipientKeyToField = Row.FieldKey;
                    }
                }
            }
        }
    }
}