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
#region usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MPartner.Gui;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Validation;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftTransactions
    {
        #region public fields

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
        public bool FGiftTransactionsLoadedFlag = false;

        #endregion public fields

        #region private fields

        //Key values
        private string FBatchCurrencyCode = string.Empty;
        private string FBatchMethodOfPayment = string.Empty;
        //private decimal FBatchExchangeRateToBase = 1.0m;
        private ARecurringGiftRow FGift = null;
        private Int32 FCurrentGiftInBatch = 0;
        private Int64 FLastDonor = -1;
        private DataView FGiftDetailView = null;
        private Int64 FRecipientKey = 0;
        private string FMotivationGroup = string.Empty;
        private string FMotivationDetail = string.Empty;
        private string FAutoPopComment = string.Empty;


        //Flags
        private bool FShowStatusDialogOnLoadFlag = true;
        private bool FInEditModeFlag = false;
        private bool FActiveOnlyFlag = false;
        private bool FGiftSelectedForDeletionFlag = false;
        private bool FSuppressListChangedFlag = false;
        private bool FMotivationDetailHasChangedFlag = false;

        //Process indicators
        private bool FRecipientKeyChangedInProcess = false;
        private bool FKeyMinistryChangedInProcess = false;
        private bool FMotivationGroupChangedInProcess = false;
        private bool FMotivationDetailChangedInProcess = false;
        private bool FNewGiftInProcess = false;
        private bool FAutoPopulatingGiftInProcess = false;
        private bool FGiftAmountChangedInProcess = false;

        private ToolTip FDonorInfoToolTip = new ToolTip();
        private string FFilterAllDetailsOfGift = string.Empty;

        //List new donors
        private List <Int64>FNewDonorsList = new List <long>();

        // used for ValidateGiftDestinationThread
        private string FPartnerShortName = "";
        private delegate void SimpleDelegate();

        #endregion private fields

        #region system settings

        //System Defaults - FSET
        // Specifies if Donor zero is allowed
        // This value is system wide but can be over-ruled by FINANCE-3 level user
        private bool FSETDonorZeroIsValidFlag = false;
        // Specifies if Recipient zero is allowed
        // This value is system wide but can be over-ruled by FINANCE-3 level user
        private bool FSETRecipientZeroIsValidFlag = false;

        #endregion system settings

        #region user settings

        //User Defaults - FSET
        private bool FSETNewDonorAlertFlag = true;
        private bool FSETAutoCopyIncludeMailingCodeFlag = false;
        private bool FSETAutoCopyIncludeCommentsFlag = false;
        private bool FSETAutoSaveFlag = false;

        #endregion user settings

        #region public properties

        /// <summary>
        /// Sets a flag to show the status dialog when transactions are loaded
        /// </summary>
        public Boolean ShowStatusDialogOnLoad
        {
            set
            {
                FShowStatusDialogOnLoadFlag = value;
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

        #endregion public properties

        #region keyboard handler methods

        private void PreProcessCommandKey()
        {
            ReconcileFloatingTextboxesFromCombos();
        }

        /// <summary>
        /// This implements keyboard shortcuts to match Petra 2.x
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="keyData">The key data</param>
        /// <returns></returns>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.A | Keys.Alt))
            {
                txtDetailGiftAmount.Focus();
                return true;
            }

            if (keyData == (Keys.M | Keys.Alt))
            {
                bool comment1 = txtDetailGiftCommentOne.Focused;
                bool comment2 = txtDetailGiftCommentTwo.Focused;
                bool comment3 = txtDetailGiftCommentThree.Focused;

                if (!comment1 && !comment2 && !comment3)
                {
                    txtDetailGiftCommentOne.Focus();
                    return true;
                }

                if (comment1)
                {
                    txtDetailGiftCommentTwo.Focus();
                    return true;
                }

                if (comment2)
                {
                    txtDetailGiftCommentThree.Focus();
                    return true;
                }

                if (comment3)
                {
                    txtDetailGiftCommentOne.Focus();
                    return true;
                }
            }

            if (keyData == (Keys.V | Keys.Alt))
            {
                cmbDetailMotivationGroupCode.Focus();
                return true;
            }

            if (keyData == (Keys.D | Keys.Alt))
            {
                txtDetailDonorKey.PerformButtonClick();
                return true;
            }

            return false;
        }

        #endregion keyboard handler methods

        #region form setup methods

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            // read user defaults
            FSETNewDonorAlertFlag = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_NEW_DONOR_ALERT, true);
            FSETDonorZeroIsValidFlag = ((TFrmRecurringGiftBatch)ParentForm).FDonorZeroIsValid;
            FSETAutoCopyIncludeMailingCodeFlag = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_MAILING_CODE, false);
            FSETAutoCopyIncludeCommentsFlag = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_COMMENTS, false);
            FSETRecipientZeroIsValidFlag = ((TFrmRecurringGiftBatch)ParentForm).FRecipientZeroIsValid;
            FSETAutoSaveFlag = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_SAVE, false);
            //FWarnOfInactiveValuesOnSubmitting = ((TFrmRecurringGiftBatch)ParentForm).FWarnOfInactiveValuesOnPosting;
        }

        private void InitialiseControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                //Fix to length of field
                txtDetailReference.MaxLength = 20;

                cmbMotivationDetailCode.Width += 20;

                //Fix a layering issue
                txtDetailRecipientLedgerNumber.SendToBack();

                //Changing this will stop taborder issues
                sptTransactions.TabStop = false;

                SetupTextBoxMenuItems();

                txtDetailDonorKey.PartnerClass = "DONOR";
                txtDetailRecipientKey.PartnerClass = "WORKER,UNIT,FAMILY";

                //Event fires when the recipient key is changed and the new partner has a different Partner Class
                txtDetailRecipientKey.PartnerClassChanged += RecipientPartnerClassChanged;

                //Set initial width of this textbox
                cmbKeyMinistries.ComboBoxWidth = 300;
                cmbKeyMinistries.AttachedLabel.Visible = false;

                TextBox motivationDetailComboLabel = cmbMotivationDetailCode.AttachedLabel;
                motivationDetailComboLabel.Visible = false;
                txtMotivationDetailDesc.BorderStyle = BorderStyle.None;
                txtMotivationDetailDesc.Font = motivationDetailComboLabel.Font;
                txtMotivationDetailDesc.Left = cmbMotivationDetailCode.Left + 106;
                txtMotivationDetailDesc.Top = cmbMotivationDetailCode.Top + 5;
                txtMotivationDetailDesc.Width = motivationDetailComboLabel.Width;
                txtMotivationDetailDesc.Text = string.Empty;
                txtMotivationDetailDesc.BringToFront();

                //Setup hidden text boxes used to speed up reading transactions
                SetupFloatingTextboxes();

                //Add events to parent panel control to identify edit mode
                pnlDetails.Enter += new EventHandler(BeginEditMode);
                pnlDetails.Leave += new EventHandler(EndEditMode);

                //Make TextBox look like a label
                txtDonorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
                txtDonorInfo.Font = TAppSettingsManager.GetDefaultBoldFont();

                //Methods of giving/payment need to allow a blank line
                cmbDetailMethodOfGivingCode.AllowDbNull = true;
                cmbDetailMethodOfGivingCode.NullValueDescription = string.Empty;
                cmbDetailMethodOfPaymentCode.AllowDbNull = true;
                cmbDetailMethodOfPaymentCode.NullValueDescription = string.Empty;

                // set tooltip
                grdDetails.SetHeaderTooltip(4, Catalog.GetString("Confidential"));

                chkDetailChargeFlag.Enabled = UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3);
            }
            finally
            {
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private void SetupTextBoxMenuItems()
        {
            List <Tuple <string, EventHandler>>ItemList = new List <Tuple <string, EventHandler>>();

            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Donor History"), OpenDonorHistory));
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Donor Finance Details"), OpenDonorFinanceDetails));
            txtDetailDonorKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient History"), OpenRecipientHistory));
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtDetailRecipientKey.AddCustomContextMenuItems(ItemList);

            ItemList.Clear();
            ItemList.Add(new Tuple <string, EventHandler>(Catalog.GetString("Open Recipient Gift Destination"), OpenGiftDestination));
            txtDetailRecipientLedgerNumber.AddCustomContextMenuItems(ItemList);
        }

        private void SetupFloatingTextboxes()
        {
            //Key ministry textbox properties
            txtDetailRecipientKeyMinistry.TabStop = false;
            txtDetailRecipientKeyMinistry.BorderStyle = BorderStyle.None;
            txtDetailRecipientKeyMinistry.Top = cmbKeyMinistries.Top + 3;
            txtDetailRecipientKeyMinistry.Left += 3;
            txtDetailRecipientKeyMinistry.Width = cmbKeyMinistries.ComboBoxWidth - 21;
            //Events
            txtDetailRecipientKeyMinistry.Click += new EventHandler(SetFocusToKeyMinistryCombo);
            txtDetailRecipientKeyMinistry.Enter += new EventHandler(SetFocusToKeyMinistryCombo);
            txtDetailRecipientKeyMinistry.KeyDown += new KeyEventHandler(FloatingTextBox_KeyDown);
            txtDetailRecipientKeyMinistry.KeyPress += new KeyPressEventHandler(FloatingTextBox_KeyPress);

            //Motivation detail textbox properties
            txtDetailMotivationDetailCode.TabStop = false;
            txtDetailMotivationDetailCode.BorderStyle = BorderStyle.None;
            txtDetailMotivationDetailCode.Top = cmbMotivationDetailCode.Top + 3;
            txtDetailMotivationDetailCode.Left += 3;
            txtDetailMotivationDetailCode.Width = cmbMotivationDetailCode.ComboBoxWidth - 21;
            //Events
            txtDetailMotivationDetailCode.Click += new EventHandler(SetFocusToMotivationDetailCombo);
            txtDetailMotivationDetailCode.Enter += new EventHandler(SetFocusToMotivationDetailCombo);
            txtDetailMotivationDetailCode.KeyDown += new KeyEventHandler(FloatingTextBox_KeyDown);
            txtDetailMotivationDetailCode.KeyPress += new KeyPressEventHandler(FloatingTextBox_KeyPress);

            bool ShowGiftDetail = GiftDetailRowIsEditable(FPreviouslySelectedDetailRow);

            ShowFloatingTextBoxes(FPreviouslySelectedDetailRow, ShowGiftDetail);
        }

        #endregion form setup methods

        #region controls handling methods

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widths needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
        }

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                // NOTE: AlanP: RestoreAdditionalWindowPositionProperties must be called when the screen is fully shown,
                // especially if Panel2 has a scrollbar that is only showing part of the panel.
                // We have to make this call after Windows has set the scroll position because this changes the splitter distance.
                // See https://tracker.openpetra.org/view.php?id=4936
                FPetraUtilsObject.RestoreAdditionalWindowPositionProperties();

                grdDetails.AutoResizeGrid();
                grdDetails.Focus();
            }
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
            bool FirstIsEnabled = ((ARow != null) && (ARow.DetailNumber == 1));
            bool pnlDetailsEnabledState = false;

            chkDetailActive.Enabled = FirstIsEnabled;
            txtDetailDonorKey.Enabled = FirstIsEnabled;
            cmbDetailMethodOfGivingCode.Enabled = FirstIsEnabled;

            cmbDetailMethodOfPaymentCode.Enabled = FirstIsEnabled && !BatchHasMethodOfPayment();
            txtDetailReference.Enabled = FirstIsEnabled;
            cmbDetailReceiptLetterCode.Enabled = FirstIsEnabled;

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
                // taken from old petra
                PnlDetailsProtected = (ARow.GiftAmount < 0);
            }

            pnlDetailsEnabledState = (!PnlDetailsProtected && grdDetails.Rows.Count > 1);
            pnlDetails.Enabled = pnlDetailsEnabledState;

            btnDelete.Enabled = pnlDetailsEnabledState;
            btnDeleteAll.Enabled = btnDelete.Enabled;

            btnNewDetail.Enabled = true;
            btnNewGift.Enabled = true;

            mniDonorFinanceDetails.Enabled = (ARow != null);
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

        private void ClearControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                txtDetailDonorKey.Text = string.Empty;
                txtDetailReference.Clear();
                txtGiftTotal.NumberValueDecimal = 0;
                txtDetailGiftAmount.NumberValueDecimal = 0;
                txtDetailRecipientKey.Text = string.Empty;
                txtDetailRecipientLedgerNumber.Text = string.Empty;
                txtDetailAccountCode.Clear();
                cmbDetailReceiptLetterCode.SelectedIndex = -1;
                cmbDetailMotivationGroupCode.SelectedIndex = -1;
                txtDetailMotivationDetailCode.Text = string.Empty;
                txtMotivationDetailDesc.Text = string.Empty;
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
                cmbMotivationDetailCode.SelectedIndex = -1;
                txtDetailCostCentreCode.Text = string.Empty;

                chkDetailActive.Checked = false;
                dtpDetailStartDonations.Clear();
                dtpDetailEndDonations.Clear();
            }
            finally
            {
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private bool FloatingTextboxesAreVisible()
        {
            return txtDetailRecipientKeyMinistry.Visible == true;
        }

        private void HideFloatingTextBoxes()
        {
            if (FloatingTextboxesAreVisible())
            {
                //Motivation Detail is populted, just filter it
                ApplyMotivationDetailCodeFilter();
                //Repopulate the keymin combo
                PopulateKeyMinistry(0, false);

                //Write bound textbox values to comboboxes
                ReconcileCombosFromFloatingTextboxes(FPreviouslySelectedDetailRow);

                //hide the overlay box during editing
                txtDetailRecipientKeyMinistry.Visible = false;
                txtDetailMotivationDetailCode.Visible = false;
            }
        }

        /// <summary>
        /// Manage the overlay
        /// </summary>
        private void ShowFloatingTextBoxes(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            bool AShowGiftDetail,
            bool AReadComboValue = false)
        {
            ResetMotivationDetailCodeFilter(AShowGiftDetail);

            // Always enabled initially. Combobox may be diabled later once populated.
            cmbKeyMinistries.Enabled = true;
            cmbMotivationDetailCode.Enabled = true;

            txtDetailRecipientKeyMinistry.Visible = true;
            txtDetailRecipientKeyMinistry.BringToFront();
            txtDetailRecipientKeyMinistry.Parent.Refresh();

            txtDetailMotivationDetailCode.Visible = true;
            txtDetailMotivationDetailCode.BringToFront();
            txtDetailMotivationDetailCode.Parent.Refresh();

            if (AReadComboValue)
            {
                ReconcileFloatingTextboxesFromCombos(ACurrentDetailRow);
            }
            else
            {
                ReconcileCombosFromFloatingTextboxes(ACurrentDetailRow);
            }
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public void ReconcileFloatingTextboxesFromCombos(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow = null)
        {
            if (ACurrentDetailRow == null)
            {
                ACurrentDetailRow = FPreviouslySelectedDetailRow;
            }

            ReconcileKeyMinistryFromCombo(ACurrentDetailRow);
            ReconcileMotivationDetailFromCombo(ACurrentDetailRow);
        }

        private void ReconcileCombosFromFloatingTextboxes(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow)
        {
            ReconcileKeyMinistryFromTextbox(ACurrentDetailRow);
            ReconcileMotivationDetailFromTextbox(ACurrentDetailRow);
        }

        private void FloatingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void FloatingTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #endregion controls handling methods

        #region show details

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
            bool ShowGiftDetail;

            //TODO: Remove?
            if (ARow == null)
            {
                FMotivationGroup = string.Empty;
                FMotivationDetail = string.Empty;
                return;
            }

            FMotivationGroup = ARow.MotivationGroupCode;
            FMotivationDetail = ARow.MotivationDetailCode;

            // set FAutoPopComment if needed
            AMotivationDetailRow MotivationDetailRow = GetCurrentMotivationDetailRow();

            ShowGiftDetail = GiftDetailRowIsEditable(FPreviouslySelectedDetailRow);

            if (!FloatingTextboxesAreVisible())
            {
                ShowFloatingTextBoxes(ARow, ShowGiftDetail, true);
            }
            else if (!FGiftTransactionsLoadedFlag)
            {
                ShowFloatingTextBoxes(ARow, ShowGiftDetail, false);
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool? DoEnableRecipientGiftDestination;
                FinishShowDetailsManual(ARow, out DoEnableRecipientGiftDestination);

                if (DoEnableRecipientGiftDestination.HasValue)
                {
                    mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
                }

                if (MotivationDetailRow != null)
                {
                    //Set the associated combobox textbox
                    txtMotivationDetailDesc.Text = MotivationDetailRow.MotivationDetailDesc;

                    // if motivation detail autopopulation is set to true
                    if (MotivationDetailRow.Autopopdesc)
                    {
                        FAutoPopComment = MotivationDetailRow.MotivationDetailDesc;
                    }
                    else
                    {
                        FAutoPopComment = null;
                    }
                }

                //Show gift table values
                ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);
                ShowDetailsForGift(giftRow);

                ShowDonorInfo(Convert.ToInt64(txtDetailDonorKey.Text));

                UpdateControlsProtection(ARow);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void FinishShowDetailsManual(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            out bool ? AEnableRecipientRecurringGiftDestination)
        {
            AEnableRecipientRecurringGiftDestination = null;

            if (ACurrentDetailRow.IsCostCentreCodeNull())
            {
                txtDetailCostCentreCode.Text = string.Empty;
            }
            else
            {
                txtDetailCostCentreCode.Text = ACurrentDetailRow.CostCentreCode;
            }

            if (ACurrentDetailRow.IsAccountCodeNull())
            {
                txtDetailAccountCode.Text = string.Empty;
            }
            else
            {
                txtDetailAccountCode.Text = ACurrentDetailRow.AccountCode;
            }

            if (ACurrentDetailRow.IsRecipientKeyNull())
            {
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                UpdateRecipientKeyText(0, ACurrentDetailRow, FMotivationGroup, FMotivationDetail);
            }
            else
            {
                txtDetailRecipientKey.Text = String.Format("{0:0000000000}", ACurrentDetailRow.RecipientKey);
                UpdateRecipientKeyText(ACurrentDetailRow.RecipientKey, ACurrentDetailRow, FMotivationGroup, FMotivationDetail);
            }

            if (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
            {
                OnRecipientPartnerClassChanged(null, out AEnableRecipientRecurringGiftDestination);
            }

            if (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) == 0)
            {
                OnRecipientPartnerClassChanged(txtDetailRecipientKey.CurrentPartnerClass, out AEnableRecipientRecurringGiftDestination);
            }

            if (ACurrentDetailRow.IsCommentOneTypeNull() && !ACurrentDetailRow.IsGiftCommentOneNull())
            {
                cmbDetailCommentOneType.SetSelectedString("Both");
            }

            if (ACurrentDetailRow.IsCommentTwoTypeNull() && !ACurrentDetailRow.IsGiftCommentTwoNull())
            {
                cmbDetailCommentTwoType.SetSelectedString("Both");
            }

            if (ACurrentDetailRow.IsCommentThreeTypeNull() && !ACurrentDetailRow.IsGiftCommentThreeNull())
            {
                cmbDetailCommentThreeType.SetSelectedString("Both");
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

            if (FCurrentGiftInBatch != ACurrentGiftRow.GiftTransactionNumber)
            {
                //New gift is selected so update the totals
                FCurrentGiftInBatch = ACurrentGiftRow.GiftTransactionNumber;
                UpdateTotals();
            }
        }

        private void ShowDonorInfo(long APartnerKey)
        {
            string DonorInfo = string.Empty;

            if (APartnerKey == 0)
            {
                return;
            }

            try
            {
                PPartnerRow DonorRow = RetrieveDonorRow(APartnerKey);

                if (DonorRow == null)
                {
                    string errMsg = String.Format(Catalog.GetString("Partner Key:'{0}' cannot be found in the Partner table!"),
                        APartnerKey);
                    MessageBox.Show(errMsg, Catalog.GetString("Show Donor Information"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                // get donor's banking details
                ARecurringGiftRow GiftRow = (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(new object[] { FLedgerNumber,
                                                                                                               FBatchNumber,
                                                                                                               FPreviouslySelectedDetailRow.
                                                                                                               GiftTransactionNumber });

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

        #endregion show details

        #region details editing

        private bool GiftDetailRowIsEditable(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            if (ARow != null)
            {
                if (!FActiveOnlyFlag || (ARow.GiftAmount < 0))
                {
                    return false;
                }

                return true;
            }

            return FActiveOnlyFlag;
        }

        private void BeginEditMode(object sender, EventArgs e)
        {
            bool DisableSave = (FBatchRow.RowState == DataRowState.Unchanged && !FPetraUtilsObject.HasChanges);

            FInEditModeFlag = true;

            //Make sure we are not in SuppressChanges mode
            FPetraUtilsObject.SuppressChangeDetection = false;

            HideFloatingTextBoxes();

            //On populating key ministry
            if (DisableSave && FPetraUtilsObject.HasChanges && !DataUtilities.DataRowColumnsHaveChanged(FBatchRow))
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        private void EndEditMode(object sender, EventArgs e)
        {
            FInEditModeFlag = false;

            bool ShowGiftDetail = GiftDetailRowIsEditable(FPreviouslySelectedDetailRow);

            if (!FloatingTextboxesAreVisible())
            {
                ShowFloatingTextBoxes(FPreviouslySelectedDetailRow, ShowGiftDetail, false);
            }
        }

        #endregion details editing

        #region delete gifts

        private bool PreDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            return OnPreDeleteManual(ARowToDelete, ref ADeletionQuestion);
        }

        private bool DeleteRowManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete, ref string ACompletionMessage)
        {
            return OnDeleteRowManual(ARowToDelete, ref ACompletionMessage);
        }

        private void PostDeleteManual(GiftBatchTDSARecurringGiftDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            OnPostDeleteManual(ARowToDelete, AAllowDeletion, ADeletionPerformed, ACompletionMessage);
        }

        #endregion delete gifts

        #region event handlers

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

        private void GiftDetailAmountChanged(object sender, EventArgs e)
        {
            if (FShowDetailsInProcess || (GetRecurringBatchRow() == null))
            {
                return;
            }

            if (txtDetailGiftAmount.NumberValueDecimal == null)
            {
                return;
            }
            else if (txtDetailGiftAmount.NumberValueDecimal.HasValue)
            {
                FGiftAmountChangedInProcess = true;
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChangedFlag && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();

                // Once we have auto-sized once and there are more than 8 rows we don't auto-size any more (unless we load data again)
                FSuppressListChangedFlag = (grdDetails.Rows.Count > 8);
            }

            btnDeleteAll.Enabled = btnDelete.Enabled;
        }

        #endregion event handlers

        #region saving methods

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            ReconcileFloatingTextboxesFromCombos();
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

                if (chkDetailActive.Checked.Equals(null))
                {
                    giftRow.SetActiveNull();
                }
                else
                {
                    giftRow.Active = chkDetailActive.Checked;
                }
            }

            //The next two are read-only fields populated by lookups based on other control values
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
                UpdateRecipientKeyText(ARow.RecipientKey,
                    ARow,
                    cmbDetailMotivationGroupCode.GetSelectedString(),
                    cmbMotivationDetailCode.GetSelectedString());
            }

            if (txtDetailRecipientLedgerNumber.Text.Length == 0)
            {
                ARow.SetRecipientFieldNull();
                ARow.SetRecipientLedgerNumberNull();
            }
            else
            {
                ARow.RecipientLedgerNumber = Convert.ToInt64(txtDetailRecipientLedgerNumber.Text);
                ARow.RecipientField = ARow.RecipientLedgerNumber;
            }

            if (string.IsNullOrEmpty(ARow.GiftCommentOne))
            {
                ARow.CommentOneType = null;
            }

            if (string.IsNullOrEmpty(ARow.GiftCommentTwo))
            {
                ARow.CommentTwoType = null;
            }

            if (string.IsNullOrEmpty(ARow.GiftCommentThree))
            {
                ARow.CommentThreeType = null;
            }
        }

        #endregion saving methods

        #region data validation

        private void ValidateDataDetailsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            //Ensure FBatchRow is correct, as this gets called from the batch tab validation
            FBatchRow = GetRecurringBatchRow();

            if ((ARow == null)
                || (ARow.RowState == DataRowState.Deleted)
                || (FBatchRow == null)
                || (FBatchRow.BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            //Process amounts, including taxes and currencies
            if (FGiftAmountChangedInProcess)
            {
                ProcessGiftAmountChange();
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //Validate gift level first
            ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            TSharedFinanceValidation_Gift.ValidateRecurringGiftManual(this,
                giftRow,
                ref VerificationResultCollection,
                FValidationControlsDict,
                FSETDonorZeroIsValidFlag);

            //Validate gift detail level
            TSharedFinanceValidation_Gift.ValidateRecurringGiftDetailManual(this,
                ARow,
                ref VerificationResultCollection,
                FValidationControlsDict,
                -1,
                FSETRecipientZeroIsValidFlag);

            //Recipient field
            if ((ARow[GiftBatchTDSARecurringGiftDetailTable.GetRecipientFieldDBName()] != DBNull.Value)
                && (ARow.RecipientField != 0)
                && (ARow.RecipientKey != 0)
                && ((int)ARow.RecipientField / 1000000 != ARow.LedgerNumber))
            {
                TVerificationResultCollection TempVerificationResultCollection;

                if (!TRemote.MFinance.Gift.WebConnectors.IsRecipientLedgerNumberSetupForILT(FLedgerNumber, FPreviouslySelectedDetailRow.RecipientKey,
                        Convert.ToInt64(txtDetailRecipientLedgerNumber.Text), out TempVerificationResultCollection))
                {
                    DataColumn ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnRecipientLedgerNumberId];
                    object ValidationContext = String.Format("Recurring Batch Number {0} (transaction:{1} detail:{2})",
                        ARow.BatchNumber,
                        ARow.GiftTransactionNumber,
                        ARow.DetailNumber);

                    TValidationControlsData ValidationControlsData;

                    if (FValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                    {
                        TVerificationResult VerificationResult = new TVerificationResult(this,
                            ValidationContext.ToString() + ": " + TempVerificationResultCollection[0].ResultText,
                            PetraErrorCodes.ERR_RECIPIENTFIELD_NOT_ILT, TResultSeverity.Resv_Critical);

                        VerificationResultCollection.Auto_Add_Or_AddOrRemove(this,
                            new TScreenVerificationResult(VerificationResult, ValidationColumn, ValidationControlsData.ValidationControl),
                            ValidationColumn, true);
                    }
                }
            }
        }

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

        private void ValidateRecipientLedgerNumber()
        {
            if (!FInEditModeFlag || (FPreviouslySelectedDetailRow.RecipientKey == 0))
            {
                return;
            }
            // if no gift destination exists for Family parter then give the user the option to open Gift Destination maintenance screen
            else if ((FPreviouslySelectedDetailRow != null)
                     && (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) == 0)
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
            // if recipient ledger number belongs to a different ledger then check that it is set up for inter-ledger transfers
            else if ((FPreviouslySelectedDetailRow != null)
                     && (Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) != 0)
                     && ((int)Convert.ToInt64(txtDetailRecipientLedgerNumber.Text) / 1000000 != FLedgerNumber))
            {
                TVerificationResultCollection VerificationResults;

                if (!TRemote.MFinance.Gift.WebConnectors.IsRecipientLedgerNumberSetupForILT(
                        FLedgerNumber, FPreviouslySelectedDetailRow.RecipientKey, Convert.ToInt64(txtDetailRecipientLedgerNumber.Text),
                        out VerificationResults))
                {
                    MessageBox.Show(VerificationResults.BuildVerificationResultString(), Catalog.GetString("Invalid Data Entered"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion data validation

        #region process broadcast messages

        /// <summary>
        /// Update Gift Destination based on a broadcast message
        /// </summary>
        /// <param name="AFormsMessage"></param>
        public void ProcessGiftDestinationBroadcastMessage(TFormsMessage AFormsMessage)
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
                    DateTime GiftDate = DateTime.Today;

                    foreach (PPartnerGiftDestinationRow Row in ((TFormsMessage.FormsMessageGiftDestination)AFormsMessage.MessageObject).
                             GiftDestinationTable.Rows)
                    {
                        // check if record is active for the Gift Date
                        if ((Row.DateEffective <= GiftDate)
                            && ((Row.DateExpires >= GiftDate) || Row.IsDateExpiresNull())
                            && (Row.DateEffective != Row.DateExpires))
                        {
                            DetailRow.RecipientLedgerNumber = Row.FieldKey;
                        }
                    }

                    // update control if updated gift is currently being displayed
                    if (!string.IsNullOrEmpty(txtDetailRecipientKey.Text)
                        && (Convert.ToInt64(txtDetailRecipientKey.Text) == DetailRow.RecipientKey))
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
                    GetRecipientData(Convert.ToInt64(txtDetailRecipientKey.Text), false);

                    break;
                }
            }
        }

        #endregion process broadcast messages
    }
}