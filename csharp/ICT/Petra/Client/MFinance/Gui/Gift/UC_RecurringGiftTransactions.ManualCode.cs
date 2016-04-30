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
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
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
    public partial class TUC_RecurringGiftTransactions
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
        private ToolTip FDonorInfoToolTip = new ToolTip();

        private ARecurringGiftRow FGift = null;
        private string FMotivationGroup = string.Empty;
        private string FMotivationDetail = string.Empty;
        private bool FMotivationDetailChangedFlag = false;
        private bool FCreatingNewGiftFlag = false;
        private string FFilterAllDetailsOfGift = string.Empty;
        private DataView FGiftDetailView = null;

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
        public ARecurringGiftBatchRow FBatchRow = null;

        /// <summary>
        /// Specifies that initial transactions have loaded into the dataset
        /// </summary>
        public bool FTransactionsLoaded = false;


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

            cmbDetailMotivationDetailCode.Width += 20;

            //Fix a layering issue
            txtDetailRecipientLedgerNumber.SendToBack();

            //Changing this will stop taborder issues
            sptTransactions.TabStop = false;

            SetupTextBoxMenuItems();
            txtDetailRecipientKey.PartnerClass = "WORKER,UNIT,FAMILY";

            //Event fires when the recipient key is changed and the new partner has a different Partner Class
            txtDetailRecipientKey.PartnerClassChanged += RecipientPartnerClassChanged;

            //Set initial width of this textbox
            cmbKeyMinistries.ComboBoxWidth = 300;
            cmbKeyMinistries.AttachedLabel.Visible = false;

            //Setup hidden text boxes used to speed up reading transactions
            SetupComboTextBoxOverlayControls();

            //Make TextBox look like a label
            txtDonorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtDonorInfo.Font = TAppSettingsManager.GetDefaultBoldFont();
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

            // Always enabled initially. Combobox may be diabled later once populated.
            cmbKeyMinistries.Enabled = true;

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
        /// Deal with case when user clicks on a control
        /// that does not result in a lost focus, e.g. menu
        /// </summary>
        public void ReconcileKeyMinistryFromCombo()
        {
            if (FInEditMode)
            {
                string KeyMinistry = string.Empty;
                bool EmptyRow = (FPreviouslySelectedDetailRow == null);

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
            if (FInEditMode)
            {
                bool EmptyRow = (FPreviouslySelectedDetailRow == null);
                string KeyMinistry = txtDetailRecipientKeyMinistry.Text;

                if (!EmptyRow && (KeyMinistry.Length > 0))
                {
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
        /// <returns>True if new gift transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadRecurringGifts(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            FBatchRow = GetRecurringBatchRow();

            if ((FBatchRow == null) && (GetAnyRecurringBatchRow(ABatchNumber) == null))
            {
                MessageBox.Show(String.Format("Cannot load transactions for Gift Batch {0} as the batch is not currently loaded!", ABatchNumber));
                return false;
            }

            //Reset Batch method of payment variable
            FBatchMethodOfPayment = ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;

            bool firstLoad = (FLedgerNumber == -1);

            if (firstLoad)
            {
                InitialiseControls();
            }

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber))
            {
                //Same as previously selected
                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                //TODO check if next line is needed.
                //SetTextBoxOverlayOnKeyMinistryCombo();

                return false;
            }

            //New set of transactions to be loaded
            FTransactionsLoaded = false;

            grdDetails.SuspendLayout();
            FSuppressListChanged = true;

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;

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
            if (FMainDS.ARecurringGiftDetail.DefaultView.Count == 0)
            {
                LoadGiftDataForBatch(ALedgerNumber, ABatchNumber);
            }

            // Now we set the full filter
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            FSuppressListChanged = false;
            grdDetails.ResumeLayout();

            UpdateTotals();

            if ((FPreviouslySelectedDetailRow != null))
            {
                bool disableSave = (FBatchRow.RowState == DataRowState.Unchanged && !FPetraUtilsObject.HasChanges);

                if (disableSave && FPetraUtilsObject.HasChanges && !DataUtilities.DataRowColumnsHaveChanged(FBatchRow))
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
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
            bool RetVal = ((TFrmRecurringGiftBatch)ParentForm).EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);

            UpdateAllRecipientDescriptions(ABatchNumber);

            return RetVal;
        }

        private void UpdateAllRecipientDescriptions(Int32 ABatchNumber)
        {
            DataView giftDetailView = new DataView(FMainDS.ARecurringGiftDetail);

            giftDetailView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView rv in giftDetailView)
            {
                GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                if (row.RecipientKey == 0)
                {
                    if (row.MotivationGroupCode != MFinanceConstants.MOTIVATION_GROUP_GIFT)
                    {
                        row.RecipientDescription = row.MotivationDetailCode;
                    }
                    else
                    {
                        row.RecipientDescription = string.Empty;
                    }
                }
            }
        }

        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (!AValidSelection)
            {
                // if partner is not valid then we treat this change as if the partner key is 0
                APartnerKey = 0;
            }

            mniRecipientHistory.Enabled = APartnerKey != 0;

            if (FInRecipientKeyChanging || FPetraUtilsObject.SuppressChangeDetection || FShowingDetails)
            {
                return;
            }

            FInRecipientKeyChanging = true;
            txtDetailRecipientKeyMinistry.Text = string.Empty;
            bool DoValidateGiftDestination = false;

            try
            {
                FPreviouslySelectedDetailRow.RecipientKey = APartnerKey;
                FPreviouslySelectedDetailRow.RecipientDescription = APartnerShortName;
                FPreviouslySelectedDetailRow.RecipientClass = txtDetailRecipientKey.CurrentPartnerClass.ToString();

                FPetraUtilsObject.SuppressChangeDetection = true;

                //Set RecipientLedgerNumber
                if (APartnerKey > 0)
                {
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber =
                        TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey, DateTime.Today);
                }
                else
                {
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber = 0;
                }

                if (!FInKeyMinistryChanging)
                {
                    GetRecipientData(APartnerKey);

                    DoValidateGiftDestination = true;
                }

                FPetraUtilsObject.SuppressChangeDetection = false;

                // do not want to update motivation comboboxes if recipient key is being changed due to a new gift or the motivation detail being changed
                if (!FMotivationDetailChangedFlag //&& !ACreatingNewGiftFlag
                    && TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetail(
                        APartnerKey, ref FMotivationGroup, ref FMotivationDetail))
                {
                    if (FMotivationGroup != cmbDetailMotivationGroupCode.GetSelectedString())
                    {
                        // note - this will also update the Motivation Detail
                        cmbDetailMotivationGroupCode.SetSelectedString(FMotivationGroup);
                    }

                    if (FMotivationDetail != cmbDetailMotivationDetailCode.GetSelectedString())
                    {
                        cmbDetailMotivationDetailCode.SetSelectedString(FMotivationDetail);
                    }

                    FPreviouslySelectedDetailRow.MotivationGroupCode = FMotivationGroup;
                    FPreviouslySelectedDetailRow.MotivationDetailCode = FMotivationDetail;
                }

                FPetraUtilsObject.SuppressChangeDetection = true;

                if (APartnerKey > 0)
                {
                    RetrieveRecipientCostCentreCode(APartnerKey);
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey);
                    RetrieveMotivationDetailCostCentreCode();
                }

                if (DoValidateGiftDestination)
                {
                    FPartnerShortName = APartnerShortName;

                    //Thread only invokes ValidateGiftDestination once Partner Short Name has been updated.
                    // Otherwise the Gift Destination screen is displayed and then the screen focus moves to this screen again
                    // when the Partner Short Name is updated.
                    new Thread(ValidateRecipientLedgerNumberThread).Start();
                }
            }
            finally
            {
                FInRecipientKeyChanging = false;
                ReconcileKeyMinistryFromCombo();
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private void RecipientLedgerNumberChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (FInRecipientKeyChanging || FPetraUtilsObject.SuppressChangeDetection || FShowingDetails)
            {
                return;
            }

            string NewCCCode = string.Empty;

            // it is possible that there are no active motivation details and so AMotivationDetail is blank
            if (!string.IsNullOrEmpty(FPreviouslySelectedDetailRow.MotivationDetailCode))
            {
                NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FLedgerNumber,
                    FPreviouslySelectedDetailRow.RecipientKey,
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                    FPreviouslySelectedDetailRow.DateEntered,
                    FPreviouslySelectedDetailRow.MotivationGroupCode,
                    FPreviouslySelectedDetailRow.MotivationDetailCode);
            }

            if (txtDetailCostCentreCode.Text != NewCCCode)
            {
                txtDetailCostCentreCode.Text = NewCCCode;
            }
        }

        private void UpdateRecipientKeyText(Int64 APartnerKey)
        {
            if ((APartnerKey == 0) && (FPreviouslySelectedDetailRow != null))
            {
                FPreviouslySelectedDetailRow.RecipientDescription = cmbDetailMotivationDetailCode.GetSelectedString();

                if (FMotivationGroup != MFinanceConstants.MOTIVATION_GROUP_GIFT)
                {
                    FPreviouslySelectedDetailRow.RecipientDescription = FMotivationDetail;
                }
                else
                {
                    FPreviouslySelectedDetailRow.RecipientDescription = string.Empty;
                }
            }
        }

        // used for ValidateGiftDestinationThread
        private string FPartnerShortName = "";
        private delegate void SimpleDelegate();

        private void ValidateRecipientLedgerNumberThread()
        {
            // wait until the label and the partner class have been updated for txtDetailRecipientKey
            while (txtDetailRecipientKey.LabelText != FPartnerShortName
                   || (txtDetailRecipientKey.CurrentPartnerClass == null && Convert.ToInt32(txtDetailRecipientKey.Text) > 0))
            {
                Thread.Sleep(10);
            }

            Invoke(new SimpleDelegate(ValidateRecipientLedgerNumber));
        }

        private void DonorKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            //An invalid donor number can stop deletion of a new row, so need to stop invalid entries
            if (!AValidSelection && (APartnerKey != 0))
            {
                txtDetailDonorKey.Text = String.Format("{0:0000000000}", 0);
                return;
            }

            // At the moment this event is thrown twice
            // We want to deal only on manual entered changes, i.e. not on selections changes, and on non-zero keys
            if (FPetraUtilsObject.SuppressChangeDetection || (APartnerKey == 0))
            {
                // FLastDonor should be the last donor key that has been entered for a gift (not 0)
                if (APartnerKey != 0)
                {
                    FLastDonor = APartnerKey;
                    mniDonorHistory.Enabled = true;
                }
                else
                {
                    mniDonorHistory.Enabled = false;
                    txtDonorInfo.Text = "";

                    if (FCreatingNewGiftFlag)
                    {
                        FLastDonor = 0;
                    }
                }
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

                        ShowDonorInfo(APartnerKey);

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
            if ((FPreviouslySelectedDetailRow == null) || FInKeyMinistryChanging || FInRecipientKeyChanging
                || FPetraUtilsObject.SuppressChangeDetection || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            string KeyMinistry = cmbKeyMinistries.GetSelectedDescription();
            string RecipientKey = cmbKeyMinistries.GetSelectedInt64().ToString();

            try
            {
                FInKeyMinistryChanging = true;

                if (cmbKeyMinistries.Count == 0)
                {
                    cmbKeyMinistries.SelectedIndex = -1;

                    if (txtDetailRecipientKeyMinistry.Text != string.Empty)
                    {
                        txtDetailRecipientKeyMinistry.Text = string.Empty;
                    }
                }
                else
                {
                    // if key ministry has actually changed
                    if ((txtDetailRecipientKeyMinistry.Text != KeyMinistry)
                        || (FPreviouslySelectedDetailRow.RecipientKeyMinistry != KeyMinistry))
                    {
                        txtDetailRecipientKeyMinistry.Text = KeyMinistry;
                        FPreviouslySelectedDetailRow.RecipientKeyMinistry = KeyMinistry;
                    }

                    if (Convert.ToInt64(txtDetailRecipientKey.Text) != Convert.ToInt64(RecipientKey))
                    {
                        txtDetailRecipientKey.Text = RecipientKey;
                    }
                }
            }
            finally
            {
                FInKeyMinistryChanging = false;
            }
        }

        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || !FInEditMode || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            FMotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();

            if (!FInRecipientKeyChanging)
            {
                FMotivationDetail = string.Empty;
            }

            ApplyMotivationDetailCodeFilter();

            if (!FInRecipientKeyChanging)
            {
                ValidateRecipientLedgerNumber();
            }
        }

        private void ApplyMotivationDetailCodeFilter()
        {
            //FMotivationbDetail will change by next process
            string motivationDetail = FMotivationDetail;

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

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        private void RetrieveMotivationDetailAccountCode(AMotivationDetailRow AMotivationDetail)
        {
            if (AMotivationDetail != null)
            {
                if (txtDetailAccountCode.Text != AMotivationDetail.AccountCode)
                {
                    txtDetailAccountCode.Text = AMotivationDetail.AccountCode;
                }
            }
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
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            string NewCostCentreCode = string.Empty;

            try
            {
                NewCostCentreCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FPreviouslySelectedDetailRow.LedgerNumber,
                    FPreviouslySelectedDetailRow.RecipientKey,
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                    FPreviouslySelectedDetailRow.DateEntered,
                    FPreviouslySelectedDetailRow.MotivationGroupCode,
                    FPreviouslySelectedDetailRow.MotivationDetailCode);

                if (FPreviouslySelectedDetailRow.CostCentreCode != NewCostCentreCode)
                {
                    FPreviouslySelectedDetailRow.CostCentreCode = NewCostCentreCode;
                }
            }
            catch (Exception ex)
            {
                string errMsg = Catalog.GetString("Error accessing Cost Centre Code:" + Environment.NewLine + ex.Message);

                MessageBox.Show(errMsg, "Cost Centre Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (txtDetailCostCentreCode.Text != NewCostCentreCode)
            {
                txtDetailCostCentreCode.Text = NewCostCentreCode;
            }
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            if (!FInEditMode || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            Int64 MotivationRecipientKey = 0;
            FMotivationDetail = cmbDetailMotivationDetailCode.GetSelectedString();

            if (FMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                    new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

                cmbDetailMotivationDetailCode.RefreshLabel();

                if (motivationDetail != null)
                {
                    RetrieveMotivationDetailAccountCode(motivationDetail);

                    MotivationRecipientKey = motivationDetail.RecipientKey;

                    // if motivation detail has AutoPopDesc set to true and has not already been autopoulated for this detail
                    if (motivationDetail.Autopopdesc && (txtDetailGiftCommentOne.Text != motivationDetail.MotivationDetailDesc))
                    {
                        // autopopulate comment one with the motivation detail description
                        AutoPopulateCommentOne(motivationDetail.MotivationDetailDesc);
                    }

                    // set tax deductible checkbox if motivation detail has been changed by the user (i.e. not a row change)
                    if (!FPetraUtilsObject.SuppressChangeDetection || FInRecipientKeyChanging)
                    {
                        chkDetailTaxDeductible.Checked = motivationDetail.TaxDeductible;
                    }
                }
            }
            else
            {
                chkDetailTaxDeductible.Checked = false;
            }

            Int64 partnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

            if (!FCreatingNewGiftFlag && (MotivationRecipientKey > 0))
            {
                FMotivationDetailChangedFlag = true;
                PopulateKeyMinistry(MotivationRecipientKey);
                FMotivationDetailChangedFlag = false;
            }
            else if (partnerKey == 0)
            {
                UpdateRecipientKeyText(0);
            }

            if (partnerKey == 0)
            {
                RetrieveMotivationDetailCostCentreCode();
            }
            else
            {
                string NewCCCode = string.Empty;

                // it is possible that there are no active motivation details and so AMotivationDetail is blank
                if (!string.IsNullOrEmpty(FMotivationDetail))
                {
                    NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FLedgerNumber,
                        partnerKey,
                        FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                        FPreviouslySelectedDetailRow.DateEntered,
                        FMotivationGroup,
                        FMotivationDetail);
                }

                if (txtDetailCostCentreCode.Text != NewCCCode)
                {
                    txtDetailCostCentreCode.Text = NewCCCode;
                }
            }
        }

        private void AutoPopulateCommentOne(string AAutoPopComment)
        {
            if (string.IsNullOrEmpty(txtDetailGiftCommentOne.Text))
            {
                txtDetailGiftCommentOne.Text = AAutoPopComment;
                cmbDetailCommentOneType.SetSelectedString("Both", -1);
            }
            else if (string.IsNullOrEmpty(txtDetailGiftCommentTwo.Text))
            {
                txtDetailGiftCommentTwo.Text = txtDetailGiftCommentOne.Text;
                cmbDetailCommentTwoType.SetSelectedString(cmbDetailCommentOneType.GetSelectedString(), -1);
                txtDetailGiftCommentOne.Text = AAutoPopComment;
                cmbDetailCommentOneType.SetSelectedString("Both", -1);
            }
            else if (string.IsNullOrEmpty(txtDetailGiftCommentThree.Text))
            {
                txtDetailGiftCommentThree.Text = txtDetailGiftCommentOne.Text;
                cmbDetailCommentThreeType.SetSelectedString(cmbDetailCommentOneType.GetSelectedString(), -1);
                txtDetailGiftCommentOne.Text = AAutoPopComment;
                cmbDetailCommentOneType.SetSelectedString("Both", -1);
            }
            else
            {
                if (MessageBox.Show(string.Format(Catalog.GetString(
                                "This Motivation Detail is set to auto populate a gift comment field, but all the comment fields are currently full."
                                +
                                " Do you want to overwrite Comment 1?{0}{0}" +
                                "'No' will keep the current comment,{0}" +
                                "'Yes' will copy Comment 1 to the clipboard and replace it with the automated comment '{1}'"),
                            "\n", AAutoPopComment),
                        Catalog.GetString("Auto Populate Gift Comment"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    Clipboard.SetText(txtDetailGiftCommentOne.Text);
                    txtDetailGiftCommentOne.Text = AAutoPopComment;
                    cmbDetailCommentOneType.SetSelectedString("Both", -1);
                }
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

            // If this method has been called as a result of a change in motivation detail then txtDetailRecipientKey has not yet been set...
            // but we do know that the recipient must be a Unit.

            // if Family Recipient
            if (!FMotivationDetailChangedFlag && (txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY))
            {
                txtDetailRecipientLedgerNumber.Text = FPreviouslySelectedDetailRow.RecipientLedgerNumber.ToString();
                cmbKeyMinistries.Clear();
                cmbKeyMinistries.Enabled = false;
            }
            // if Unit Recipient
            else
            {
                //At this point, only active KeyMinistries are allowed in a live gift
                bool activeOnly = true;
                TFinanceControls.GetRecipientData(ref cmbKeyMinistries, ref txtDetailRecipientLedgerNumber, APartnerKey, activeOnly);

                // enable / disable combo box depending on whether it contains any key ministries
                if ((cmbKeyMinistries.Table == null) || (cmbKeyMinistries.Table.Rows.Count == 0))
                {
                    cmbKeyMinistries.Enabled = false;
                }
                else
                {
                    cmbKeyMinistries.Enabled = true;
                }
            }
        }

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if ((GetRecurringBatchRow() == null) || (txn.NumberValueDecimal == null))
            {
                return;
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
                    ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
                }
            }
            else
            {
                GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

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

                //FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED &&
                txtGiftTotal.NumberValueDecimal = sum;
                txtGiftTotal.CurrencyCode = txtDetailGiftAmount.CurrencyCode;
                txtGiftTotal.ReadOnly = true;
                //this is here because at the moment the generator does not generate this
                txtBatchTotal.NumberValueDecimal = sumBatch;

                //Now we look at the batch and update the batch data
                if ((FBatchRow != null) && FTransactionsLoaded)
                {
                    ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(sumBatch, FBatchRow.BatchNumber);
                }
            }

            if (disableSaveButton && FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        /// <summary>
        /// reset the control
        /// </summary>
        /// <param name="AResetFBatchNumber"></param>
        public void ClearCurrentSelection(bool AResetFBatchNumber = true)
        {
            this.FPreviouslySelectedDetailRow = null;

            if (AResetFBatchNumber)
            {
                FBatchNumber = -1;
            }
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
        private ARecurringGiftBatchRow GetRecurringBatchRow()
        {
            return ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// get the details of any loaded batch
        /// </summary>
        /// <returns></returns>
        private ARecurringGiftBatchRow GetAnyRecurringBatchRow(Int32 ABatchNumber)
        {
            return ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().GetAnyRecurringBatchRow(ABatchNumber);
        }

        /// <summary>
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private ARecurringGiftRow GetRecurringGiftRow(Int32 ARecurringGiftTransactionNumber)
        {
            return (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, ARecurringGiftTransactionNumber });
        }

        /// <summary>
        /// get the details of the current gift
        /// </summary>
        /// <returns></returns>
        private GiftBatchTDSARecurringGiftDetailRow GetRecurringGiftDetailRow(Int32 ARecurringGiftTransactionNumber, Int32 ARecurringGiftDetailNumber)
        {
            return (GiftBatchTDSARecurringGiftDetailRow)FMainDS.ARecurringGiftDetail.Rows.Find(new object[] { FLedgerNumber, FBatchNumber,
                                                                                                              ARecurringGiftTransactionNumber,
                                                                                                              ARecurringGiftDetailNumber });
        }

        private bool PreDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            FGift = GetRecurringGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
            FFilterAllDetailsOfGift = String.Format("{0}={1} and {2}={3}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                FPreviouslySelectedDetailRow.BatchNumber,
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                FPreviouslySelectedDetailRow.GiftTransactionNumber);

            FGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
            FGiftDetailView.RowFilter = FFilterAllDetailsOfGift;
            FGiftDetailView.Sort = ARecurringGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftAmount, GetRecurringBatchRow().CurrencyCode);

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
                    formattedDetailAmount);
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
                        formattedDetailAmount);
            }
            else //this should never happen
            {
                ADeletionQuestion =
                    String.Format(Catalog.GetString("Recurring Gift no. {0} in Gift Batch no. {1} has no detail rows in the Gift Detail table!"),
                        ARowToDelete.GiftTransactionNumber,
                        ARowToDelete.BatchNumber);
                allowDeletion = false;
            }

            return allowDeletion;
        }

        private bool DeleteRowManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew)
            {
                try
                {
                    // temporarily disable  New Donor Warning
                    ((TFrmRecurringGiftBatch) this.ParentForm).NewDonorWarning = false;

                    //Return modified row to last saved state to avoid validation failures
                    ARowToDelete.RejectChanges();
                    ShowDetails(ARowToDelete);

                    if (!((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("Error in trying to save prior to deleting current gift detail!"),
                            Catalog.GetString("Deletion Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return DeletionSuccessful;
                    }
                }
                finally
                {
                    ((TFrmRecurringGiftBatch) this.ParentForm).NewDonorWarning = true;
                }
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS BackupMainDS = (GiftBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            //To be used later....Pass copy to delete method.
            //GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();
            //TempDS.Merge(FMainDS);

            int SelectedDetailNumber = ARowToDelete.DetailNumber;
            int GiftToDeleteTransNo = 0;
            string FilterAllGiftsOfBatch = String.Empty;
            string FilterAllGiftDetailsOfBatch = String.Empty;

            int DetailRowCount = FGiftDetailView.Count;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Speeds up deletion of larger gift sets
                FMainDS.EnforceConstraints = false;

                //Delete current detail row
                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete gift header row
                if (DetailRowCount > 1)
                {
                    FGiftSelectedForDeletion = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        if (row.DetailNumber > SelectedDetailNumber)
                        {
                            row.DetailNumber--;
                        }
                    }

                    FGift.LastDetailNumber--;

                    FPetraUtilsObject.SetChangedFlag();
                }
                else
                {
                    GiftToDeleteTransNo = FGift.GiftTransactionNumber;

                    TLogging.Log("Delete recurring row: " + GiftToDeleteTransNo.ToString());

                    // Reduce all Gift Detail row Transaction numbers by 1 if they are greater then gift to be deleted
                    FilterAllGiftDetailsOfBatch = String.Format("{0}={1} And {2}>{3}",
                        ARecurringGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                        GiftToDeleteTransNo);

                    DataView giftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
                    giftDetailView.RowFilter = FilterAllGiftDetailsOfBatch;
                    giftDetailView.Sort = String.Format("{0} ASC", ARecurringGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the gift row to be deleted to the top
                    FilterAllGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        ARecurringGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftTable.GetGiftTransactionNumberDBName(),
                        GiftToDeleteTransNo);

                    DataView giftView = new DataView(FMainDS.ARecurringGift);
                    giftView.RowFilter = FilterAllGiftsOfBatch;
                    giftView.Sort = String.Format("{0} ASC", ARecurringGiftTable.GetGiftTransactionNumberDBName());

                    ARecurringGiftRow giftRowToReceive = null;
                    ARecurringGiftRow giftRowToCopyDown = null;
                    ARecurringGiftRow giftRowCurrent = null;

                    int currentGiftTransNo = 0;

                    foreach (DataRowView gv in giftView)
                    {
                        giftRowCurrent = (ARecurringGiftRow)gv.Row;

                        currentGiftTransNo = giftRowCurrent.GiftTransactionNumber;

                        if (currentGiftTransNo > GiftToDeleteTransNo)
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
                            giftRowCurrent.ChargeStatus = MFinanceConstants.MARKED_FOR_DELETION;
                        }

                        //Will always be previous row
                        giftRowToReceive = giftRowCurrent;
                    }

                    FPreviouslySelectedDetailRow = null;

                    FPetraUtilsObject.SetChangedFlag();

                    FGiftSelectedForDeletion = true;

                    FBatchRow.LastGiftNumber--;
                }

                //Try to save changes
                if (((TFrmRecurringGiftBatch) this.ParentForm).SaveChangesManual())
                {
                    //Clear current batch's gift data and reload from server
                    RefreshCurrentRecurringBatchGiftData(FBatchNumber);
                }
                else
                {
                    throw new Exception("Unable to save after deleting a recurring gift!");
                }

                ACompletionMessage = Catalog.GetString("Recurring gift row deleted successfully!");

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Gift Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS.Merge(BackupMainDS);
            }
            finally
            {
                FMainDS.EnforceConstraints = true;
                SetGiftDetailDefaultView();
                FFilterAndFindObject.ApplyFilter();
                this.Cursor = Cursors.Default;
            }

            UpdateRecordNumberDisplay();

            return DeletionSuccessful;
        }

        private void PostDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete,
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

                ((TFrmRecurringGiftBatch) this.ParentForm).SaveChangesManual();

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

        private void DeleteAllGifts(System.Object sender, EventArgs e)
        {
            string completionMessage = string.Empty;
            int BatchNumberToClear = FBatchNumber;

            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            else if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
            {
                MessageBox.Show(Catalog.GetString("Please remove the filter before attempting to delete all gifts in this recurring batch."),
                    Catalog.GetString("Delete All Gifts"));

                return;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString(
                            "You have chosen to delete all gifts from recurring batch ({0}).{1}{1}Are you sure you want to delete all?"),
                        BatchNumberToClear,
                        Environment.NewLine),
                    Catalog.GetString("Confirm Delete All"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                //Backup the Dataset for reversion purposes
                GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();
                FTempDS.Merge(FMainDS);

                try
                {
                    //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                    completionMessage = String.Format(Catalog.GetString("All gifts and details deleted successfully."),
                        FPreviouslySelectedDetailRow.BatchNumber);

                    //clear any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection(false);

                    //Clear out the gift data for the current batch without marking the records for deletion
                    //  and then reload from server
                    RefreshCurrentRecurringBatchGiftData(BatchNumberToClear);

                    //Now delete all gift data for current batch
                    DeleteCurrentRecurringBatchGiftData(BatchNumberToClear);

                    FBatchRow.BatchTotal = 0;

                    // Be sure to set the last gift number in the parent table before saving all the changes
                    SetBatchLastGiftNumber();

                    FPetraUtilsObject.SetChangedFlag();

                    // save
                    if (((TFrmRecurringGiftBatch)ParentForm).SaveChangesManual())
                    {
                        MessageBox.Show(completionMessage,
                            "All Gifts Deleted.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        SelectRowInGrid(1);

                        // saving failed, therefore do not try to cancel
                        MessageBox.Show(Catalog.GetString("The emptied recurring batch failed to save!"));
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
                    FMainDS.Merge(FTempDS);
                }
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                UpdateControlsProtection();
            }

            UpdateRecordNumberDisplay();
        }

        /// <summary>
        /// Clear the gift data of the current batch without marking records for delete
        /// </summary>
        private bool RefreshCurrentRecurringBatchGiftData(Int32 ABatchNumber)
        {
            bool RetVal = false;

            //Copy the current dataset
            GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();

            TempDS.Merge(FMainDS);

            //Backup the current dataset
            GiftBatchTDS BackupDS = (GiftBatchTDS)FMainDS.Copy();
            BackupDS.Merge(FMainDS);

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Remove current batch gift data
                DataView giftDetailView = new DataView(TempDS.ARecurringGiftDetail);

                giftDetailView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    ARecurringGiftDetailTable.GetDetailNumberDBName());

                foreach (DataRowView dr in giftDetailView)
                {
                    dr.Delete();
                }

                DataView giftView = new DataView(TempDS.ARecurringGift);

                giftView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    ABatchNumber);

                giftView.Sort = String.Format("{0} DESC",
                    ARecurringGiftTable.GetGiftTransactionNumberDBName());

                foreach (DataRowView dr in giftView)
                {
                    dr.Delete();
                }

                TempDS.AcceptChanges();

                //Clear all gift data from Main dataset gift tables
                FMainDS.ARecurringGiftDetail.Clear();
                FMainDS.ARecurringGift.Clear();

                //Bring data back in from other batches if it exists
                if (TempDS.ARecurringGift.Count > 0)
                {
                    FMainDS.ARecurringGift.Merge(TempDS.ARecurringGift);
                    FMainDS.ARecurringGiftDetail.Merge(TempDS.ARecurringGiftDetail);
                }

                //Reload batch contents from server
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringGiftTransactionsForBatch(FLedgerNumber, ABatchNumber));

                RetVal = true;
            }
            catch (Exception ex)
            {
                string errMsg = Catalog.GetString("Error trying to clear current recurring batch data: /n/r/n/r" + ex.Message);
                MessageBox.Show(errMsg, "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Undo changes
                FMainDS.Merge(BackupDS);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return RetVal;
        }

        /// <summary>
        /// Delete all batch gifts and details
        /// </summary>
        /// <param name="ABatchNumber"></param>
        public void DeleteCurrentRecurringBatchGiftData(Int32 ABatchNumber)
        {
            DataView giftDetailView = new DataView(FMainDS.ARecurringGiftDetail);

            giftDetailView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftDetailView.Sort = String.Format("{0} DESC, {1} DESC",
                ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                ARecurringGiftDetailTable.GetDetailNumberDBName());

            foreach (DataRowView dr in giftDetailView)
            {
                dr.Delete();
            }

            DataView giftView = new DataView(FMainDS.ARecurringGift);

            giftView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftView.Sort = String.Format("{0} DESC",
                ARecurringGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in giftView)
            {
                dr.Delete();
            }
        }

        private void SetBatchLastGiftNumber()
        {
            DataView dv = new DataView(FMainDS.ARecurringGift);

            dv.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                FBatchNumber);

            dv.Sort = String.Format("{0} DESC",
                ARecurringGiftTable.GetGiftTransactionNumberDBName());

            dv.RowStateFilter = DataViewRowState.CurrentRows;

            if (dv.Count > 0)
            {
                ARecurringGiftRow transRow = (ARecurringGiftRow)dv[0].Row;
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
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.ARecurringGiftDetail.DefaultView.Sort = string.Format("{0}, {1}",
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    ARecurringGiftDetailTable.GetDetailNumberDBName());
            }
        }

        private void ClearControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                //txtBatchTotal.NumberValueDecimal = 0;
                txtDetailDonorKey.Text = string.Empty;
                txtDetailReference.Clear();
                txtGiftTotal.NumberValueDecimal = 0;
                txtDetailGiftAmount.NumberValueDecimal = 0;
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
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
                cmbKeyMinistries.SelectedIndex = -1;
                txtDetailCostCentreCode.Text = string.Empty;

                chkDetailActive.Checked = false;
                dtpStartDonations.Clear();
                dtpEndDonations.Clear();
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
            FCreatingNewGiftFlag = true;

            ARecurringGiftRow CurrentGiftRow = null;
            bool IsEmptyGrid = (grdDetails.Rows.Count == 1);

            if (ValidateAllData(true, TErrorProcessingMode.Epm_IgnoreNonCritical))
            {
                if (!ACompletelyNewGift)  //i.e. a gift detail
                {
                    ACompletelyNewGift = IsEmptyGrid;
                }

                if (ACompletelyNewGift)
                {
                    //Run this if a new gift is requested or required.

                    // we create the table locally, no dataset
                    ARecurringGiftRow giftRow = FMainDS.ARecurringGift.NewRowTyped(true);

                    giftRow.LedgerNumber = FBatchRow.LedgerNumber;
                    giftRow.BatchNumber = FBatchRow.BatchNumber;
                    giftRow.GiftTransactionNumber = ++FBatchRow.LastGiftNumber;
                    giftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                    giftRow.LastDetailNumber = 1;

                    FMainDS.ARecurringGift.Rows.Add(giftRow);

                    CurrentGiftRow = giftRow;

                    mniDonorHistory.Enabled = false;
                }
                else
                {
                    CurrentGiftRow = GetRecurringGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
                    CurrentGiftRow.LastDetailNumber++;
                }

                //New gifts will require a new detail anyway, so this code always runs
                GiftBatchTDSARecurringGiftDetailRow newRow = FMainDS.ARecurringGiftDetail.NewRowTyped(true);

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

                newRow.DateEntered = DateTime.Now;

                FMainDS.ARecurringGiftDetail.Rows.Add(newRow);

                FPetraUtilsObject.SetChangedFlag();

                if (!SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1))
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

                UpdateRecipientKeyText(0);
                cmbKeyMinistries.Clear();
                mniRecipientHistory.Enabled = false;
            }

            FCreatingNewGiftFlag = false;
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
                txtDetailGiftAmount.CurrencyCode = FBatchRow.CurrencyCode;
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

        private void ShowDetailsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
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
                ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);
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

                if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
                {
                    RecipientPartnerClassChanged(null);
                }

                if (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) == 0)
                {
                    RecipientPartnerClassChanged(null);
                }

                ShowDonorInfo(Convert.ToInt64(txtDetailDonorKey.Text));

                UpdateControlsProtection(ARow);
            }
            finally
            {
                FShowingDetails = false;
                this.Cursor = Cursors.Default;
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
                ARecurringGiftRow GiftRow = (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(
                    new object[] { FLedgerNumber, FBatchNumber, FPreviouslySelectedDetailRow.GiftTransactionNumber });

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

        private void ShowDetailsForGift(ARecurringGiftRow ACurrentGiftRow)
        {
            //Set GiftRow controls
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
            txtDetailGiftAmount.CurrencyCode = ACurrencyCode;
            txtGiftTotal.CurrencyCode = ACurrencyCode;
            txtBatchTotal.CurrencyCode = ACurrencyCode;
            txtHashTotal.CurrencyCode = ACurrencyCode;
        }

        /// <summary>
        /// update the transaction method payment from outside
        /// </summary>
        public void UpdateMethodOfPayment(bool ACalledLocally)
        {
            Int32 LedgerNumber;
            Int32 BatchNumber;

            if (ACalledLocally)
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment);
                return;
            }

            if (!((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().FBatchLoaded)
            {
                return;
            }

            FBatchRow = GetRecurringBatchRow();

            if (FBatchRow == null)
            {
                FBatchRow = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().GetSelectedDetailRow();
            }

            FBatchMethodOfPayment = ((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().MethodOfPaymentCode;

            LedgerNumber = FBatchRow.LedgerNumber;
            BatchNumber = FBatchRow.BatchNumber;

            if (!LoadGiftDataForBatch(LedgerNumber, BatchNumber))
            {
                return;
            }

            if ((FLedgerNumber == LedgerNumber) || (FBatchNumber == BatchNumber))
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
                if (recurringGiftRow.BatchNumber.Equals(BatchNumber) && recurringGiftRow.LedgerNumber.Equals(LedgerNumber)
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
            bool pnlDetailsEnabledState = false;

            chkDetailActive.Enabled = firstIsEnabled;
            txtDetailDonorKey.Enabled = firstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = firstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = firstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = firstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = firstIsEnabled;

            if (FBatchRow == null)
            {
                FBatchRow = GetRecurringBatchRow();
            }

            if (ARow == null)
            {
                PnlDetailsProtected = true;
            }
            else
            {
                PnlDetailsProtected = (ARow != null
                                       && ARow.GiftAmount < 0
                                       );     // taken from old petra
            }

            pnlDetailsEnabledState = (!PnlDetailsProtected && grdDetails.Rows.Count > 1);
            pnlDetails.Enabled = pnlDetailsEnabledState;

            btnDelete.Enabled = pnlDetailsEnabledState;
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
                FBatchMethodOfPayment = ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().MethodOfPaymentCode;
            }

            return FBatchMethodOfPayment;
        }

        private void GetDetailDataFromControlsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            //Handle gift table fields for first detail only
            if (ARow.DetailNumber == 1)
            {
                ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);

                giftRow.DonorKey = Convert.ToInt64(txtDetailDonorKey.Text);

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
        }

        private void ValidateDataDetailsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            FBatchRow = GetRecurringBatchRow();

            if ((ARow == null) || (FBatchRow == null) || (FBatchRow.BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateRecurringGiftDetailManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, Convert.ToInt64(txtDetailRecipientLedgerNumber.Text));

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);

            TSharedFinanceValidation_Gift.ValidateRecurringGiftManual(this,
                giftRow,
                ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ValidateRecipientLedgerNumber()
        {
            // if no gift destination exists for Family parter then give the user the option to open Gift Destination maintenance screen
            if (FInEditMode
                && (FPreviouslySelectedDetailRow != null)
                && (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) == 0)
                && (FPreviouslySelectedDetailRow.RecipientKey != 0)
                && (cmbDetailMotivationGroupCode.GetSelectedString() == MFinanceConstants.MOTIVATION_GROUP_GIFT))
            {
                if ((txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY)
                    && (MessageBox.Show(Catalog.GetString("No valid Gift Destination exists for ") +
                            FPreviouslySelectedDetailRow.RecipientDescription +
                            " (" + FPreviouslySelectedDetailRow.RecipientKey.ToString("0000000000") + ").\n\n" +
                            string.Format(Catalog.GetString("A Gift Destination will need to be assigned to this Partner before" +
                                    " this gift can be saved with the Motivation Group '{0}'." +
                                    " Would you like to do this now?"), MFinanceConstants.MOTIVATION_GROUP_GIFT),
                            Catalog.GetString("No Valid Gift Destination"),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    OpenGiftDestination(this, null);
                }
                // if no recipient ledger number for Unit partner
                else if ((txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.UNIT)
                         && (MessageBox.Show(string.Format(Catalog.GetString(
                                         "The Unit Partner {0} has not been allocated a Parent Field that can receive gifts. " +
                                         "This will need to be changed before this gift can be saved with the Motivation Group '{1}'.\n\n" +
                                         "Would you like to do this now?"),
                                     "'" + FPreviouslySelectedDetailRow.RecipientDescription + "' (" +
                                     FPreviouslySelectedDetailRow.RecipientKey.ToString("0000000000") +
                                     ")",
                                     MFinanceConstants.MOTIVATION_GROUP_GIFT),
                                 Catalog.GetString("Problem with Unit's Parent Field"),
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());
                    frm.SetParameters(TScreenMode.smEdit,
                        FPreviouslySelectedDetailRow.RecipientKey,
                        Ict.Petra.Shared.MPartner.TPartnerEditTabPageEnum.petpDetails);
                    frm.Show();
                }
            }
        }

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                grdDetails.AutoResizeGrid();
                grdDetails.Focus();
            }
        }

        /// <summary>
        /// Refresh the dataset for this form
        /// </summary>
        public void RefreshData()
        {
            if ((FMainDS != null) && (FMainDS.ARecurringGiftDetail != null))
            {
                FMainDS.ARecurringGiftDetail.Rows.Clear();
            }

            FBatchRow = GetRecurringBatchRow();

            if (FBatchRow != null)
            {
                LoadRecurringGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber);
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChanged && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
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
        private void RecipientPartnerClassChanged(TPartnerClass ? APartnerClass)
        {
            string ItemText = Catalog.GetString("Open Recipient Gift Destination");

            if ((APartnerClass == TPartnerClass.UNIT) || (APartnerClass == null))
            {
                txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
                txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, false);
                mniRecipientGiftDestination.Enabled = false;
            }
            else if (APartnerClass == TPartnerClass.FAMILY)
            {
                txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
                txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, true);
                mniRecipientGiftDestination.Enabled = true;
            }
        }

        /// <summary>
        /// Update Gift Destination based on a broadcast message
        /// </summary>
        /// <param name="AFormsMessage"></param>
        public void ProcessGiftDetainationBroadcastMessage(TFormsMessage AFormsMessage)
        {
            // for some reason it is possible that this method can be called even if the parent form has been closed
            if (((TFrmRecurringGiftBatch)ParentForm) == null)
            {
                return;
            }

            // update dataset from controls
            GetDataFromControls();

            // loop through every gift detail currently in the dataset
            foreach (ARecurringGiftDetailRow DetailRow in FMainDS.ARecurringGiftDetail.Rows)
            {
                if (DetailRow.RecipientKey == ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).PartnerKey)
                {
                    DetailRow.RecipientLedgerNumber = 0;

                    foreach (PPartnerGiftDestinationRow Row in ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).
                             GiftDestinationTable.Rows)
                    {
                        // check if record is active for the Gift Date
                        if ((Row.DateEffective <= DateTime.Today)
                            && ((Row.DateExpires >= DateTime.Today) || Row.IsDateExpiresNull())
                            && (Row.DateEffective != Row.DateExpires))
                        {
                            DetailRow.RecipientLedgerNumber = Row.FieldKey;
                        }
                    }

                    // update control if updated gift is currently being displayed
                    if (!string.IsNullOrEmpty(txtDetailRecipientKey.Text) && (Convert.ToInt64(txtDetailRecipientKey.Text) == DetailRow.RecipientKey))
                    {
                        txtDetailRecipientLedgerNumber.Text = DetailRow.RecipientLedgerNumber.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Update Unit recipient based on a broadcast message
        /// </summary>
        /// <param name="AFormsMessage"></param>
        public void ProcessUnitHierarchyBroadcastMessage(TFormsMessage AFormsMessage)
        {
            // for some reason it is possible that this method can be called even if the parent form has been closed
            if (((TFrmRecurringGiftBatch)ParentForm) == null)
            {
                return;
            }

            if (txtDetailRecipientKey.CurrentPartnerClass != TPartnerClass.UNIT)
            {
                return;
            }

            List <Tuple <string, Int64,
                         Int64>>UnitHierarchyChanges =
                ((TFormsMessage.FormsMessageUnitHierarchy)AFormsMessage.MessageObject).UnitHierarchyChanges;

            // loop backwards as the most recent (and accurate) change will be at the end
            for (int i = UnitHierarchyChanges.Count - 1; i >= 0; i--)
            {
                if (UnitHierarchyChanges[i].Item2 == Convert.ToInt64(txtDetailRecipientKey.Text))
                {
                    GetRecipientData(Convert.ToInt64(txtDetailRecipientKey.Text));
                    break;
                }
            }
        }
    }
}