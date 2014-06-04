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

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MCommon;
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
        private string FBatchMethodOfPayment = string.Empty;
        private Int64 FLastDonor = -1;
        private bool FActiveOnly = true;
        private bool FGiftSelectedForDeletion = false;
        private bool FSuppressListChanged = false;
        private bool FInRecipientKeyChanging = false;
        private bool FInKeyMinistryChanging = false;
        private bool FInEditMode = false;
        private bool FShowingDetails = false;

        private ARecurringGiftRow FGift = null;
        private string FMotivationGroup = string.Empty;
        private string FMotivationDetail = string.Empty;
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


        private void InitialiseControls()
        {
            //Fix to length of field
            txtDetailReference.MaxLength = 20;

            //Fix a layering issue
            txtField.SendToBack();

            //Changing this will stop taborder issues
            sptTransactions.TabStop = false;

            txtDetailRecipientKey.PartnerClass = "WORKER,UNIT,FAMILY";

            //Set initial width of this textbox
            cmbMinistry.ComboBoxWidth = 250;
            cmbMinistry.AttachedLabel.Visible = false;

            //Setup hidden text boxes used to speed up reading transactions
            SetupComboTextBoxOverlayControls();
        }

        private void SetupComboTextBoxOverlayControls()
        {
            txtRecipientKeyMinistry.TabStop = false;
            txtRecipientKeyMinistry.BorderStyle = BorderStyle.None;
            txtRecipientKeyMinistry.Top = cmbMinistry.Top + 3;
            txtRecipientKeyMinistry.Left += 3;
            txtRecipientKeyMinistry.Width = cmbMinistry.ComboBoxWidth - 21;

            txtRecipientKeyMinistry.Click += new EventHandler(SetFocusToKeyMinistryCombo);
            txtRecipientKeyMinistry.Enter += new EventHandler(SetFocusToKeyMinistryCombo);
            txtRecipientKeyMinistry.KeyDown += new KeyEventHandler(OverlayTextBox_KeyDown);
            txtRecipientKeyMinistry.KeyPress += new KeyPressEventHandler(OverlayTextBox_KeyPress);

            pnlDetails.Enter += new EventHandler(BeginEditMode);
            pnlDetails.Leave += new EventHandler(EndEditMode);

            SetTextBoxOverlayOnKeyMinistryCombo();
        }

        private void SetFocusToKeyMinistryCombo(object sender, EventArgs e)
        {
            cmbMinistry.Focus();
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
            SetKeyMinistryTextBoxInvisible(null, null);
        }

        private void EndEditMode(object sender, EventArgs e)
        {
            FInEditMode = false;

            if (!txtRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo();
            }
        }

        private void SetTextBoxOverlayOnKeyMinistryCombo()
        {
            ResetMotivationDetailCodeFilter();

            txtRecipientKeyMinistry.Visible = true;
            txtRecipientKeyMinistry.BringToFront();
        }

        private void SetKeyMinistryTextBoxInvisible(object sender, EventArgs e)
        {
            if (txtRecipientKeyMinistry.Visible)
            {
                ApplyMotivationDetailCodeFilter();

                PopulateKeyMinistry();

                //hide the overlay box during editing
                txtRecipientKeyMinistry.Visible = false;
            }
        }

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns>True if new gift transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadGifts(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            FBatchRow = GetCurrentBatchRow();

            if ((FBatchRow == null) && (GetAnyBatchRow(ABatchNumber) == null))
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
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FPreviouslySelectedDetailRow != null))
            {
                //Same as previously selected
                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                SetTextBoxOverlayOnKeyMinistryCombo();

                return false;
            }

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
                EnsureGiftDataPresent(ALedgerNumber, ABatchNumber);
            }

            // Now we set the full filter
            ApplyFilter();
            UpdateRecordNumberDisplay();
            SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            FSuppressListChanged = false;
            grdDetails.ResumeLayout();

            FTransactionsLoaded = true;
            UpdateTotals();

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
            DataView TransDV = new DataView(FMainDS.ARecurringGiftDetail);

            TransDV.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            if (TransDV.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(ALedgerNumber, ABatchNumber));

                UpdateAllRecipientDescriptions(ABatchNumber);

                //TODO: apply below
                //((TFrmRecurringGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
            }

            return TransDV.Count > 0;
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
                    row.RecipientDescription = row.MotivationDetailCode;
                }
            }
        }

        private string FindCostCentreCodeForRecipient(ARecurringGiftDetailRow ARow, Int64 APartnerKey, bool AShowError = false)
        {
            if (ARow == null)
            {
                return string.Empty;
            }

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

            return NewCostCentreCode;
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
            txtRecipientKeyMinistry.Text = string.Empty;

            try
            {
                FPreviouslySelectedDetailRow.ReceiptNumber = Convert.ToInt32(APartnerKey);
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

                TLogging.Log(string.Format("GetRecipientFundNumber for {0} is {1}",
                        APartnerKey,
                        FPreviouslySelectedDetailRow.RecipientLedgerNumber));

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
                }

                if (APartnerKey > 0)
                {
                    RetrieveRecipientCostCentreCode(APartnerKey);
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey);
                    RetrieveMotivationDetailCostCentreCode();
                }
            }
            finally
            {
                FInRecipientKeyChanging = false;
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private void UpdateRecipientKeyText(Int64 APartnerKey)
        {
            if (APartnerKey == 0)
            {
                if (FPreviouslySelectedDetailRow != null)
                {
                    FPreviouslySelectedDetailRow.RecipientDescription = cmbDetailMotivationDetailCode.GetSelectedString();
                }
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
            else if (FShowingDetails)
            {
                return;
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
            if (FInKeyMinistryChanging || FInRecipientKeyChanging || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FInKeyMinistryChanging = true;

            try
            {
                if (cmbMinistry.Count == 0)
                {
                    cmbMinistry.SelectedIndex = -1;
                    txtRecipientKeyMinistry.Text = string.Empty;
                }
                else
                {
                    txtRecipientKeyMinistry.Text = cmbMinistry.GetSelectedDescription();
                    txtDetailRecipientKey.Text = cmbMinistry.GetSelectedInt64().ToString();
                }
            }
            finally
            {
                FInKeyMinistryChanging = false;
            }
        }

        /// <summary>
        /// Called on TextChanged event for combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.SuppressChangeDetection || !FInEditMode || txtRecipientKeyMinistry.Visible)
            {
                return;
            }

            FMotivationGroup = cmbDetailMotivationGroupCode.GetSelectedString();
            FMotivationDetail = string.Empty;

            ApplyMotivationDetailCodeFilter();
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

            txtCostCentreCode.Text = CostCentreCode;
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

            txtCostCentreCode.Text = FindCostCentreCodeForRecipient(FPreviouslySelectedDetailRow, APartnerKey, false);
        }

        private bool UpdateCostCentreCodeForRecipients(out string AFailedUpdates,
            Int32 AGiftTransactionNumber = 0,
            Int32 AGiftDetailNumber = 0)
        {
            AFailedUpdates = string.Empty;

            if ((FMainDS.ARecurringGiftBatch.Count == 0) || (FMainDS.ARecurringGift.Count == 0))
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
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftTransactionNumber,
                    ARecurringGiftDetailTable.GetDetailNumberDBName(),
                    AGiftDetailNumber);
            }
            else
            {
                RowFilterForGifts = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);
            }

            DataView giftRowsView = new DataView(FMainDS.ARecurringGiftDetail);
            giftRowsView.RowFilter = RowFilterForGifts;

            foreach (DataRowView dvRows in giftRowsView)
            {
                ARecurringGiftDetailRow giftDetailRow = (ARecurringGiftDetailRow)dvRows.Row;

                ARecurringGiftRow giftRow = GetGiftRow(giftDetailRow.GiftTransactionNumber);

                CurrentCostCentreCode = String.Empty; //giftDetailRow.CostCentreCode;
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
                    txtCostCentreCode.Text = NewCostCentreCode;
                    //giftDetailRow.CostCentreCode = NewCostCentreCode;
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
            if (!FInEditMode || txtRecipientKeyMinistry.Visible)
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
            cmbMinistry.Clear();

            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

                if (APartnerKey == 0)
                {
                    return;
                }
            }

            GetRecipientData(APartnerKey);

            //long FieldNumber = Convert.ToInt64(txtField.Text);
            //txtCostCentreCode.Text = TRemote.MFinance.Gift.WebConnectors.IdentifyPartnerCostCentre(FLedgerNumber, FieldNumber);
        }

        private void GetRecipientData(long APartnerKey)
        {
            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);
            }

            TFinanceControls.GetRecipientData(ref cmbMinistry, ref txtField, APartnerKey, true);
        }

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            TTxtNumericTextBox txn = (TTxtNumericTextBox)sender;

            if ((GetCurrentBatchRow() == null) || (txn.NumberValueDecimal == null))
            {
                return;
            }
            else
            {
                UpdateTotals();
            }
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
                    ((TFrmRecurringGiftBatch)this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
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
                if (FBatchRow != null && FTransactionsLoaded)
                {
                    ((TFrmRecurringGiftBatch)this.ParentForm).GetBatchControl().UpdateBatchTotal(sumBatch, FBatchRow.BatchNumber);
                }
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
        private ARecurringGiftBatchRow GetCurrentBatchRow()
        {
            return (ARecurringGiftBatchRow)FMainDS.ARecurringGiftBatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
        }

        /// <summary>
        /// get the details of any loaded batch
        /// </summary>
        /// <returns></returns>
        private ARecurringGiftBatchRow GetAnyBatchRow(Int32 ABatchNumber)
        {
            return ((TFrmRecurringGiftBatch)ParentForm).GetBatchControl().GetAnyBatchRow(ABatchNumber);
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
            FGiftDetailView.Sort = ARecurringGiftDetailTable.GetDetailNumberDBName() + " ASC";
            String formattedDetailAmount = StringHelper.FormatUsingCurrencyCode(ARowToDelete.GiftAmount, GetCurrentBatchRow().CurrencyCode);

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
            bool deletionSuccessful = false;
            string originatingDetailRef = string.Empty;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return deletionSuccessful;
            }

            if ((ARowToDelete.RowState != DataRowState.Added) && !((TFrmRecurringGiftBatch)this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current gift detail!");
                return deletionSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();

            int selectedDetailNumber = ARowToDelete.DetailNumber;
            int giftToDeleteTransNo = 0;
            string filterAllGiftsOfBatch = String.Empty;
            string filterAllGiftDetailsOfBatch = String.Empty;

            int detailRowCount = FGiftDetailView.Count;

            try
            {
                //Delete current detail row
                ARowToDelete.Delete();

                //If there existed (before the delete row above) more than one detail row, then no need to delete gift header row
                if (detailRowCount > 1)
                {
                    FGiftSelectedForDeletion = false;

                    foreach (DataRowView rv in FGiftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

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
                        ARecurringGiftDetailTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftDetailView = new DataView(FMainDS.ARecurringGiftDetail);
                    giftDetailView.RowFilter = filterAllGiftDetailsOfBatch;
                    giftDetailView.Sort = String.Format("{0} ASC", ARecurringGiftDetailTable.GetGiftTransactionNumberDBName());

                    foreach (DataRowView rv in giftDetailView)
                    {
                        GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                        row.GiftTransactionNumber--;
                    }

                    //Cannot delete the gift row, just copy the data of rows above down by 1 row
                    // and then mark the top row for deletion
                    //In other words, bubble the gift row to be deleted to the top
                    filterAllGiftsOfBatch = String.Format("{0}={1} And {2}>={3}",
                        ARecurringGiftTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringGiftTable.GetGiftTransactionNumberDBName(),
                        giftToDeleteTransNo);

                    DataView giftView = new DataView(FMainDS.ARecurringGift);
                    giftView.RowFilter = filterAllGiftsOfBatch;
                    giftView.Sort = String.Format("{0} ASC", ARecurringGiftTable.GetGiftTransactionNumberDBName());

                    ARecurringGiftRow giftRowToReceive = null;
                    ARecurringGiftRow giftRowToCopyDown = null;
                    ARecurringGiftRow giftRowCurrent = null;

                    int currentGiftTransNo = 0;

                    foreach (DataRowView gv in giftView)
                    {
                        giftRowCurrent = (ARecurringGiftRow)gv.Row;

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
                if (((TFrmRecurringGiftBatch)this.ParentForm).SaveChanges())
                {
                    //Clear current batch's gift data and reload from server
                    ClearCurrentBatchGiftData(FBatchNumber);
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(FLedgerNumber, FBatchNumber));

                    //TODO
                    //((TFrmRecurringGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
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
                ApplyFilter();
            }

            UpdateRecordNumberDisplay();

            return deletionSuccessful;
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

                ((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges();

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
        private void ClearCurrentBatchGiftData(Int32 ABatchNumber)
        {
            //Copy the current dataset
            GiftBatchTDS FTempDS = (GiftBatchTDS)FMainDS.Copy();

            //Remove current batch gift data
            DataView giftDetailView = new DataView(FTempDS.ARecurringGiftDetail);

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

            DataView giftView = new DataView(FTempDS.ARecurringGift);

            giftView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            giftView.Sort = String.Format("{0} DESC",
                ARecurringGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView dr in giftView)
            {
                dr.Delete();
            }

            FTempDS.AcceptChanges();

            //Clear all gift data from Main dataset gift tables
            FMainDS.ARecurringGiftDetail.Clear();
            FMainDS.ARecurringGift.Clear();

            //Bring data back in from other batches if it exists
            if (FTempDS.ARecurringGift.Count > 0)
            {
                FMainDS.ARecurringGift.Merge(FTempDS.ARecurringGift);
                FMainDS.ARecurringGiftDetail.Merge(FTempDS.ARecurringGiftDetail);
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
                FFilterPanelControls.SetBaseFilter(rowFilter, true);
                FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = rowFilter;
                FCurrentActiveFilter = rowFilter;
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
                txtCostCentreCode.Text = string.Empty;
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
            ARecurringGiftRow CurrentGiftRow = null;
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
                    ARecurringGiftRow giftRow = FMainDS.ARecurringGift.NewRowTyped(true);

                    giftRow.LedgerNumber = FBatchRow.LedgerNumber;
                    giftRow.BatchNumber = FBatchRow.BatchNumber;
                    giftRow.GiftTransactionNumber = ++FBatchRow.LastGiftNumber;
                    giftRow.MethodOfPaymentCode = FBatchRow.MethodOfPaymentCode;
                    giftRow.LastDetailNumber = 1;

                    FMainDS.ARecurringGift.Rows.Add(giftRow);

                    CurrentGiftRow = giftRow;
                }
                else
                {
                    CurrentGiftRow = GetGiftRow(FPreviouslySelectedDetailRow.GiftTransactionNumber);
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
                }

                FMainDS.ARecurringGiftDetail.Rows.Add(newRow);

                FPetraUtilsObject.SetChangedFlag();

                if (!SelectDetailRowByDataTableIndex(FMainDS.ARecurringGiftDetail.Rows.Count - 1))
                {
                    if (FCurrentActiveFilter != FFilterPanelControls.BaseFilter)
                    {
                        MessageBox.Show(
                            MCommonResourcestrings.StrNewRecordIsFiltered,
                            MCommonResourcestrings.StrAddNewRecordTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FFilterPanelControls.ClearAllDiscretionaryFilters();

                        if (FucoFilterAndFind.ShowApplyFilterButton != TUcoFilterAndFind.FilterContext.None)
                        {
                            ApplyFilter();
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

                //Set the default motivation Group. This needs to happen after focus has returned
                //  to the pnlDetails to ensure FInEditMode is correct.
                cmbDetailMotivationGroupCode.SelectedIndex = 0;
                UpdateRecipientKeyText(0);
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
                txtDetailGiftAmount.CurrencyCode = FBatchRow.CurrencyCode;
            }

            if (grdDetails.Rows.Count == 1)
            {
                txtBatchTotal.NumberValueDecimal = 0;
                ClearControls();
            }

            if ((Convert.ToInt64(txtDetailRecipientKey.Text) == 0) && (cmbDetailMotivationGroupCode.SelectedIndex == -1))
            {
                txtCostCentreCode.Text = string.Empty;
            }

            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfGivingCode, Catalog.GetString("Enter method of giving"));
            FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfPaymentCode, Catalog.GetString("Enter the method of payment"));
            FPetraUtilsObject.SetStatusBarText(txtDetailReference, Catalog.GetString("Enter a reference code."));
            FPetraUtilsObject.SetStatusBarText(cmbDetailReceiptLetterCode, Catalog.GetString("Select the receipt letter code"));
        }

        private void ShowDetailsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            if (!txtRecipientKeyMinistry.Visible)
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

                if (ARow.IsRecipientKeyMinistryNull())
                {
                    txtRecipientKeyMinistry.Text = string.Empty;
                }
                else
                {
                    txtRecipientKeyMinistry.Text = ARow.RecipientKeyMinistry;
                }

                //Show gift table values
                ARecurringGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);
                ShowDetailsForGift(giftRow);

                //if (ARow.IsCostCentreCodeNull())
                //{
                //    txtDetailCostCentreCode.Text = string.Empty;
                //}
                //else
                //{
                //    txtDetailCostCentreCode.Text = ARow.CostCentreCode;
                //}

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
                }
                else
                {
                    txtField.Text = ARow.RecipientField.ToString();
                }

                UpdateControlsProtection(ARow);

                FShowingDetails = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
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

            FBatchRow = GetCurrentBatchRow();

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

                //TODO
                //((TFrmGiftBatch)ParentForm).ProcessRecipientCostCentreCodeUpdateErrors(false);
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
            bool pnlDetailsEnabledState = false;

            chkDetailActive.Enabled = firstIsEnabled;
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
                PnlDetailsProtected = true;
            }
            else
            {
                PnlDetailsProtected = (ARow != null && ARow.GiftAmount < 0);    // taken from old petra
            }

            pnlDetailsEnabledState = (!PnlDetailsProtected && grdDetails.Rows.Count > 1);
            pnlDetails.Enabled = pnlDetailsEnabledState;

            btnDelete.Enabled = pnlDetailsEnabledState;
            btnNewDetail.Enabled = !PnlDetailsProtected;
            btnNewGift.Enabled = !PnlDetailsProtected;
        }

        private Boolean BatchHasMethodOfPayment()
        {
            String batchMop = GetMethodOfPaymentFromBatch();

            return batchMop != null && batchMop.Length > 0;
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

            if (txtRecipientKeyMinistry.Text.Length == 0)
            {
                ARow.SetRecipientKeyMinistryNull();
            }
            else
            {
                ARow.RecipientKeyMinistry = txtRecipientKeyMinistry.Text;
            }

            //Handle gift table fields for first detail only
            if (ARow.DetailNumber == 1)
            {
                ARecurringGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

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
        }

        private void ValidateDataDetailsManual(ARecurringGiftDetailRow ARow)
        {
            if ((ARow == null) || (GetCurrentBatchRow() == null) || (GetCurrentBatchRow().BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateRecurringGiftDetailManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            ARecurringGiftRow giftRow = GetGiftRow(ARow.GiftTransactionNumber);

            TSharedFinanceValidation_Gift.ValidateRecurringGiftManual(this,
                giftRow,
                ref VerificationResultCollection,
                FValidationControlsDict);
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
            if ((FMainDS != null) && (FMainDS.ARecurringGiftDetail != null))
            {
                FMainDS.ARecurringGiftDetail.Rows.Clear();
            }

            FBatchRow = GetCurrentBatchRow();

            if (FBatchRow != null)
            {
                LoadGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber);
            }
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChanged && (grdDetails.Rows.Count > 1))
            {
                AutoSizeGrid();
            }
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

            Console.WriteLine("Done AutoSizeGrid() on {0} rows", grdDetails.Rows.Count);
        }
    }
}