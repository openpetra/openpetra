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
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonDialogs;
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
        private bool FShowStatusDialogOnLoad = true;
        private string FBatchCurrencyCode = string.Empty;
        private string FBatchMethodOfPayment = string.Empty;

        private ARecurringGiftRow FGift = null;
        private Int64 FLastDonor = -1;
        private bool FActiveOnly = false;
        private bool FGiftSelectedForDeletion = false;
        private bool FSuppressListChanged = false;
        private bool FInRecipientKeyChanging = false;
        private bool FInKeyMinistryChanging = false;
        private bool FCreatingNewGift = false;
        private bool FAutoPopulatingGift = false;
        private bool FInEditMode = false;
        private ToolTip FDonorInfoToolTip = new ToolTip();

        private string FMotivationGroup = string.Empty;
        private string FMotivationDetail = string.Empty;
        private bool FMotivationDetailChanged = false;

        private string FAutoPopComment = string.Empty;
        private string FFilterAllDetailsOfGift = string.Empty;
        private DataView FGiftDetailView = null;
        private Int64 FRecipientKey = 0;
        private bool FGiftAmountChanged = false;
        private Int32 FCurrentGiftInBatch = 0;

        //System Defaults
        // Specifies if Donor zero is allowed
        // This value is system wide but can be over-ruled by FINANCE-3 level user
        private bool FDonorZeroIsValid = false;
        // Specifies if Recipient zero is allowed
        // This value is system wide but can be over-ruled by FINANCE-3 level user
        private bool FRecipientZeroIsValid = false;

        //User Defaults
        private bool FNewDonorAlert = true;
        private bool FAutoCopyIncludeMailingCode = false;
        private bool FAutoCopyIncludeComments = false;
        private bool FAutoSave = false;
        //private bool FWarnOfInactiveValuesOnPosting = false;

        //List new donors
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
        public ARecurringGiftBatchRow FBatchRow = null;

        /// <summary>
        /// Specifies that initial transactions have loaded into the dataset
        /// </summary>
        public bool FGiftTransactionsLoaded = false;

        /// <summary>
        /// Sets a flag to show the status dialog when transactions are loaded
        /// </summary>
        public Boolean ShowStatusDialogOnLoad
        {
            set
            {
                FShowStatusDialogOnLoad = value;
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

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            // read user defaults
            FNewDonorAlert = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_NEW_DONOR_ALERT, true);
            FDonorZeroIsValid = ((TFrmRecurringGiftBatch)ParentForm).FDonorZeroIsValid;
            FAutoCopyIncludeMailingCode = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_MAILING_CODE, false);
            FAutoCopyIncludeComments = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_COMMENTS, false);
            FRecipientZeroIsValid = ((TFrmRecurringGiftBatch)ParentForm).FRecipientZeroIsValid;
            FAutoSave = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_SAVE, false);
            //FWarnOfInactiveValuesOnSubmitting = ((TFrmRecurringGiftBatch)ParentForm).FWarnOfInactiveValuesOnPosting;
        }

        private void InitialiseControls()
        {
            try
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                //Fix to length of field
                txtDetailReference.MaxLength = 20;

                cmbDetailMotivationDetailCode.Width += 20;

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

                //Setup hidden text boxes used to speed up reading transactions
                SetupComboTextBoxOverlayControls();

                //Make TextBox look like a label
                txtDonorInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
                txtDonorInfo.Font = TAppSettingsManager.GetDefaultBoldFont();

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

            bool ShowGiftDetail = EditableGiftDetail(FPreviouslySelectedDetailRow);

            TUC_RecurringGiftTransactions_Recipient.SetTextBoxOverlayOnKeyMinistryCombo(FPreviouslySelectedDetailRow,
                ShowGiftDetail,
                cmbKeyMinistries,
                cmbDetailMotivationDetailCode,
                txtDetailRecipientKeyMinistry,
                ref FMotivationDetail,
                FInEditMode);
        }

        private bool EditableGiftDetail(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            if (ARow != null)
            {
                if (!FActiveOnly || (ARow.GiftAmount < 0))
                {
                    return false;
                }

                return true;
            }

            return FActiveOnly;
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
            bool DisableSave = (FBatchRow.RowState == DataRowState.Unchanged && !FPetraUtilsObject.HasChanges);

            FInEditMode = true;

            TUC_RecurringGiftTransactions_Recipient.SetKeyMinistryTextBoxInvisible(FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                cmbKeyMinistries,
                ref cmbDetailMotivationGroupCode,
                ref cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                FRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                FMotivationGroup,
                ref FMotivationDetail,
                ref FMotivationDetailChanged,
                FActiveOnly,
                FInRecipientKeyChanging,
                FCreatingNewGift,
                FInEditMode,
                ref FAutoPopComment);

            //On populating key ministry
            if (DisableSave && FPetraUtilsObject.HasChanges && !DataUtilities.DataRowColumnsHaveChanged(FBatchRow))
            {
                FPetraUtilsObject.DisableSaveButton();
            }
        }

        private void EndEditMode(object sender, EventArgs e)
        {
            FInEditMode = false;

            bool ShowGiftDetail = EditableGiftDetail(FPreviouslySelectedDetailRow);

            TUC_RecurringGiftTransactions_Recipient.OnEndEditMode(FPreviouslySelectedDetailRow,
                cmbKeyMinistries,
                cmbDetailMotivationGroupCode,
                cmbDetailMotivationDetailCode,
                txtDetailRecipientKeyMinistry,
                ref FMotivationGroup,
                ref FMotivationDetail,
                ShowGiftDetail,
                FInEditMode,
                FPetraUtilsObject);
        }

        /// <summary>
        /// Deal with case when user clicks on a control
        /// that does not result in a lost focus, e.g. menu
        /// </summary>
        public void ReconcileKeyMinistryFromCombo()
        {
            TUC_RecurringGiftTransactions_Recipient.ReconcileKeyMinistryFromCombo(FPreviouslySelectedDetailRow,
                cmbKeyMinistries,
                txtDetailRecipientKeyMinistry,
                FInEditMode);
        }

        /// <summary>
        /// load the gifts into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AForceLoadFromServer">Set to true to get data from the server even though it is apparently the current batch number and status</param>
        /// <returns>True if new gift transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadRecurringGifts(Int32 ALedgerNumber, Int32 ABatchNumber, bool AForceLoadFromServer = false)
        {
            //Set key flags
            bool FirstGiftTransLoad = (FLedgerNumber == -1);
            bool SameCurrentBatch = ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && !AForceLoadFromServer);

            FBatchRow = GetRecurringBatchRow();

            if ((FBatchRow == null) && (GetAnyRecurringBatchRow(ABatchNumber) == null))
            {
                MessageBox.Show(String.Format("Cannot load transactions for Recurring Gift Batch {0} as the batch is not currently loaded!",
                        ABatchNumber));
                return false;
            }

            //Set key values from Batch
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FBatchCurrencyCode = FBatchRow.CurrencyCode;
            FBatchMethodOfPayment = FBatchRow.MethodOfPaymentCode;

            if (FirstGiftTransLoad)
            {
                InitialiseControls();
            }

            UpdateCurrencySymbols(FBatchCurrencyCode);

            //Check if the same batch is selected, so no need to apply filter
            if (SameCurrentBatch)
            {
                //Same as previously selected
                if (GetSelectedRowIndex() > 0)
                {
                    GetDetailsFromControls(GetSelectedDetailRow());
                }

                UpdateControlsProtection();

                return false;
            }

            //New Batch
            FCurrentGiftInBatch = 0;

            //New set of transactions to be loaded
            TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());

            if (FShowStatusDialogOnLoad == true)
            {
                dlgStatus.Show();
                FShowStatusDialogOnLoad = false;
                dlgStatus.Heading = String.Format(Catalog.GetString("Recurring Batch {0}"), ABatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading transactions ...");
            }

            FGiftTransactionsLoaded = false;
            FSuppressListChanged = false;

            //Apply new filter
            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;

            // if this form is readonly, then we need all codes, because old codes might have been used
            if (FirstGiftTransLoad)
            {
                //Recurring, like posted, shows all historical data
                FActiveOnly = false;
                dlgStatus.CurrentStatus = Catalog.GetString("Initialising controls ...");

                try
                {
                    //Without this, the Save button enables even for Posted batches!
                    FPetraUtilsObject.SuppressChangeDetection = true;

                    TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, FActiveOnly);
                    TFinanceControls.InitialiseMotivationDetailList(ref cmbDetailMotivationDetailCode, FLedgerNumber, FActiveOnly);
                    TFinanceControls.InitialiseMethodOfGivingCodeList(ref cmbDetailMethodOfGivingCode, FActiveOnly);
                    TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, FActiveOnly);
                    TFinanceControls.InitialisePMailingList(ref cmbDetailMailingCode, FActiveOnly);
                }
                finally
                {
                    FPetraUtilsObject.SuppressChangeDetection = false;
                }
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
                dlgStatus.CurrentStatus = Catalog.GetString("Requesting transactions from server ...");
                //Load all partners in Batch
                FMainDS.DonorPartners.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAllPartnerDataForBatch(ALedgerNumber, ABatchNumber)); //LoadAllPartnerDataForBatch();
                //Include Donor fields
                LoadGiftDataForBatch(ALedgerNumber, ABatchNumber);
            }

            // Now we set the full filter
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

            SelectRowInGrid(1);

            UpdateControlsProtection();

            dlgStatus.CurrentStatus = Catalog.GetString("Updating totals for the batch ...");
            UpdateTotals();

            if ((FPreviouslySelectedDetailRow != null))
            {
                bool disableSave = (FBatchRow.RowState == DataRowState.Unchanged && !FPetraUtilsObject.HasChanges);

                if (disableSave && FPetraUtilsObject.HasChanges && !DataUtilities.DataRowColumnsHaveChanged(FBatchRow))
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }

            FGiftTransactionsLoaded = true;
            dlgStatus.Close();

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

            TUC_RecurringGiftTransactions_Recipient.UpdateAllRecipientDescriptions(ABatchNumber, FMainDS);

            UpdateAllDonorNames(ABatchNumber);

            return RetVal;
        }

        /// <summary>
        /// Update all donor names in gift details table
        /// </summary>
        /// <param name="ABatchNumber"></param>
        private void UpdateAllDonorNames(Int32 ABatchNumber)
        {
            Dictionary <Int32, Int64>GiftsDict = new Dictionary <Int32, Int64>();
            Dictionary <Int64, string>DonorsDict = new Dictionary <Int64, string>();

            DataView GiftDV = new DataView(FMainDS.ARecurringGift);

            GiftDV.RowFilter = string.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            GiftDV.Sort = string.Format("{0} ASC", ARecurringGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView drv in GiftDV)
            {
                ARecurringGiftRow gr = (ARecurringGiftRow)drv.Row;

                Int64 donorKey = gr.DonorKey;

                GiftsDict.Add(gr.GiftTransactionNumber, donorKey);

                if (!DonorsDict.ContainsKey(donorKey))
                {
                    if (donorKey != 0)
                    {
                        PPartnerRow pr = RetrieveDonorRow(donorKey);

                        if (pr != null)
                        {
                            DonorsDict.Add(donorKey, pr.PartnerShortName);
                        }
                    }
                    else
                    {
                        DonorsDict.Add(0, "");
                    }
                }
            }

            //Add donor info to gift details
            DataView GiftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);

            GiftDetailDV.RowFilter = string.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            GiftDetailDV.Sort = string.Format("{0} ASC", ARecurringGiftDetailTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView drv in GiftDetailDV)
            {
                GiftBatchTDSARecurringGiftDetailRow giftDetail = (GiftBatchTDSARecurringGiftDetailRow)drv.Row;

                Int64 donorKey = GiftsDict[giftDetail.GiftTransactionNumber];

                giftDetail.DonorKey = donorKey;
                giftDetail.DonorName = DonorsDict[donorKey];
            }
        }

        private PPartnerRow RetrieveDonorRow(long APartnerKey)
        {
            if (APartnerKey == 0)
            {
                return null;
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
                    return null;
                }
                else
                {
                    FMainDS.DonorPartners.Merge(PartnerTable);
                }

                DonorRow = PartnerTable[0];
            }

            return DonorRow;
        }

        private void RecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            bool DoValidateGiftDestination;

            if (!AValidSelection)
            {
                if (APartnerKey != 0)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("Recipient number {0} could not be found!"),
                            APartnerKey));
                    txtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                    return;
                }
            }

            FRecipientKey = APartnerKey;

            TUC_RecurringGiftTransactions_Recipient.OnRecipientKeyChanged(APartnerKey,
                APartnerShortName,
                AValidSelection,
                FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                ref cmbKeyMinistries,
                cmbDetailMotivationGroupCode,
                cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                ref FMotivationGroup,
                ref FMotivationDetail,
                FShowDetailsInProcess,
                ref FInRecipientKeyChanging,
                FInKeyMinistryChanging,
                FInEditMode,
                FMotivationDetailChanged,
                FCreatingNewGift,
                FActiveOnly,
                out DoValidateGiftDestination);

            if (DoValidateGiftDestination)
            {
                FPartnerShortName = APartnerShortName;

                //Thread only invokes ValidateGiftDestination once Partner Short Name has been updated.
                // Otherwise the Gift Destination screen is displayed and then the screen focus moves to this screen again
                // when the Partner Short Name is updated.
                new Thread(ValidateRecipientLedgerNumberThread).Start();
            }

            mniRecipientHistory.Enabled = APartnerKey != 0;
        }

        private void RecipientLedgerNumberChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            TUC_RecurringGiftTransactions_Recipient.OnRecipientLedgerNumberChanged(FLedgerNumber,
                FPreviouslySelectedDetailRow,
                FPetraUtilsObject,
                txtDetailCostCentreCode,
                FInRecipientKeyChanging,
                FShowDetailsInProcess);
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
            if (FAutoPopulatingGift)
            {
                return;
            }
            else if (!AValidSelection && (APartnerKey != 0))
            {
                //An invalid donor number can stop deletion of a new row, so need to stop invalid entries
                MessageBox.Show(String.Format(Catalog.GetString("Donor number {0} could not be found!"),
                        APartnerKey));
                txtDetailDonorKey.Text = String.Format("{0:0000000000}", 0);
                return;
            }
            // At the moment this event is thrown twice
            // We want to deal only on manual entered changes, i.e. not on selections changes, and on non-zero keys
            else if (FPetraUtilsObject.SuppressChangeDetection || (APartnerKey == 0))
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

                    if (FCreatingNewGift)
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

                    // this is a different donor
                    if (APartnerKey != FLastDonor)
                    {
                        PPartnerRow pr = RetrieveDonorRow(APartnerKey);

                        if (pr == null)
                        {
                            string errMsg = String.Format(Catalog.GetString("Partner Key:'{0} - {1}' cannot be found in the Partner table!"),
                                APartnerKey,
                                APartnerShortName);
                            MessageBox.Show(errMsg, Catalog.GetString("Donor Changed"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //An invalid donor number can stop deletion of a new row, so need to stop invalid entries
                            txtDetailDonorKey.Text = String.Format("{0:0000000000}", 0);
                            return;
                        }

                        chkDetailConfidentialGiftFlag.Checked = pr.AnonymousDonor;

                        Int32 giftTransactionNo = FPreviouslySelectedDetailRow.GiftTransactionNumber;

                        DataView giftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);

                        giftDetailDV.RowFilter = string.Format("{0}={1} And {2}={3}",
                            ARecurringGiftDetailTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            ARecurringGiftDetailTable.GetGiftTransactionNumberDBName(),
                            giftTransactionNo);

                        foreach (DataRowView drv in giftDetailDV)
                        {
                            GiftBatchTDSARecurringGiftDetailRow giftDetail = (GiftBatchTDSARecurringGiftDetailRow)drv.Row;

                            giftDetail.DonorKey = APartnerKey;
                            giftDetail.DonorName = APartnerShortName;
                            giftDetail.DonorClass = pr.PartnerClass;
                        }

                        //Point to current gift row and specify as not a new donor
                        FGift = GetRecurringGiftRow(giftTransactionNo);

                        //Only autopopulate if this is a donor selection on a clean gift,
                        //  i.e. determine this is not a donor change where other changes have been made
                        //Sometimes you want to just change the donor without changing what already has been entered
                        //  e.g. when you realise you have entered the wrong donor after entering the correct recipient data
                        if (!TRemote.MFinance.Gift.WebConnectors.DonorHasGiven(FLedgerNumber, APartnerKey))
                        {
                            // add donor key to list so that new donor warning can be shown
                            if (!FNewDonorsList.Contains(APartnerKey))
                            {
                                FNewDonorsList.Add(APartnerKey);
                            }
                        }
                        else if ((giftDetailDV.Count == 1)
                                 && (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
                                 && (txtDetailGiftAmount.NumberValueDecimal.Value == 0))
                        {
                            AutoPopulateGiftDetail(APartnerKey, APartnerShortName, giftTransactionNo);
                        }

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

        // auto populate recipient info using the donor's last gift
        private void AutoPopulateGiftDetail(Int64 ADonorKey, String APartnerShortName, Int32 ARecurringGiftTransactionNumber)
        {
            FAutoPopulatingGift = true;

            bool IsSplitGift = false;

            DateTime LatestUnpostedGiftDateEntered = new DateTime(1900, 1, 1);

            try
            {
                //Check for Donor in saved gift batches
                // and record most recent date entered
                GiftBatchTDSAGiftDetailTable GiftDetailTable = new GiftBatchTDSAGiftDetailTable();

                //Check for even more recent saved gifts on server (i.e. not necessarily loaded)
                GiftDetailTable = TRemote.MFinance.Gift.WebConnectors.LoadDonorLastPostedGift(ADonorKey, FLedgerNumber, LatestUnpostedGiftDateEntered);

                if (((GiftDetailTable != null) && (GiftDetailTable.Count > 0)))
                {
                    //UnLoaded/Saved gift from donor is more recent
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
                                    StringHelper.FormatUsingCurrencyCode(Row.GiftAmount, GetRecurringBatchRow().CurrencyCode) +
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
                cmbDetailMotivationDetailCode.SetSelectedString(giftDetailRow.MotivationDetailCode);
                chkDetailConfidentialGiftFlag.Checked = giftDetailRow.ConfidentialGiftFlag;
                chkDetailChargeFlag.Checked = giftDetailRow.ChargeFlag;
                cmbDetailMethodOfPaymentCode.SetSelectedString(FBatchMethodOfPayment, -1);
                cmbDetailMethodOfGivingCode.SetSelectedString(giftDetailRow.MethodOfGivingCode, -1);
                chkDetailTaxDeductible.Checked = giftDetailRow.TaxDeductible;

                //Handle mailing code
                if (FAutoCopyIncludeMailingCode)
                {
                    cmbDetailMailingCode.SetSelectedString(giftDetailRow.MailingCode, -1);
                }
                else
                {
                    cmbDetailMailingCode.SelectedIndex = -1;
                }

                //Copy the comments and comment types if required
                if (FAutoCopyIncludeComments)
                {
                    txtDetailGiftCommentOne.Text = giftDetailRow.GiftCommentOne;
                    cmbDetailCommentOneType.SetSelectedString(giftDetailRow.CommentOneType);
                    txtDetailGiftCommentTwo.Text = giftDetailRow.GiftCommentTwo;
                    cmbDetailCommentTwoType.SetSelectedString(giftDetailRow.CommentTwoType);
                    txtDetailGiftCommentThree.Text = giftDetailRow.GiftCommentThree;
                    cmbDetailCommentThreeType.SetSelectedString(giftDetailRow.CommentThreeType);
                }

                //Now process other gift details if they exist
                if (IsSplitGift)
                {
                    //Only copy amount to first row if copying split gifts
                    txtDetailGiftAmount.NumberValueDecimal = giftDetailRow.GiftTransactionAmount;

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
                        detailRow.GiftTransactionNumber = ARecurringGiftTransactionNumber;
                        detailRow.DetailNumber = ++FGift.LastDetailNumber;
                        detailRow.DonorName = APartnerShortName;
                        detailRow.DonorClass = FPreviouslySelectedDetailRow.DonorClass;
                        detailRow.DateEntered = DateTime.Today;
                        detailRow.MethodOfPaymentCode = FPreviouslySelectedDetailRow.MethodOfPaymentCode;

                        if (!FAutoCopyIncludeMailingCode)
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
                                    DateTime.Today);
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
                                            ARecurringGiftTransactionNumber,
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
                                ARecurringGiftTransactionNumber,
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
                                ARecurringGiftTransactionNumber,
                                detailRow.DetailNumber);
                            MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (string.IsNullOrEmpty(detailRow.MotivationDetailCode))
                        {
                            detailRow.MotivationDetailCode = string.Empty;
                            string msg = String.Format(Catalog.GetString("Gift: {0}, Detail: {1} has no Motivation Detail!"),
                                ARecurringGiftTransactionNumber,
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
                            }
                            else
                            {
                                string msg =
                                    String.Format(Catalog.GetString(
                                            "Gift: {0}, Detail: {1} has Motivation Group and Detail codes ('{2} : {3}') not found in the database!"),
                                        ARecurringGiftTransactionNumber,
                                        detailRow.DetailNumber,
                                        motivationGroup,
                                        motivationDetail);
                                MessageBox.Show(msg, Catalog.GetString("Copying Previous Split Gift"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                detailRow.TaxDeductible = false;
                            }
                        }

                        //______________________
                        //Process comments
                        if (!FAutoCopyIncludeComments)
                        {
                            detailRow.CommentOneType = "Both";
                            detailRow.CommentTwoType = "Both";
                            detailRow.CommentThreeType = "Both";
                            detailRow.SetGiftCommentOneNull();
                            detailRow.SetGiftCommentTwoNull();
                            detailRow.SetGiftCommentThreeNull();
                        }

                        detailRow.AcceptChanges();
                        detailRow.SetAdded();
                    }

                    //Add in the new records
                    //  It is OK to merge from a different table structure as long as common fields
                    //  exist as between AGiftDetail and ARecurringGiftDetail
                    FMainDS.ARecurringGiftDetail.Merge(GiftDetailTable);

                    int indexOfLatestRow = FMainDS.ARecurringGiftDetail.Rows.Count - 1;

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

                    cmbKeyMinistries.Clear();
                    mniRecipientHistory.Enabled = false;
                    btnDeleteAll.Enabled = btnDelete.Enabled;
                    UpdateRecordNumberDisplay();
                    FLastDonor = -1;
                }
                else
                {
                    txtDetailGiftAmount.Focus();
                }

                FPetraUtilsObject.SetChangedFlag();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                FAutoPopulatingGift = false;
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
            TUC_RecurringGiftTransactions_Recipient.OnKeyMinistryChanged(FPreviouslySelectedDetailRow,
                FPetraUtilsObject,
                cmbKeyMinistries,
                txtDetailRecipientKey,
                txtDetailRecipientKeyMinistry,
                FInRecipientKeyChanging,
                ref FInKeyMinistryChanging);
        }

        private void MotivationGroupChanged(object sender, EventArgs e)
        {
            TUC_RecurringGiftTransactions_Recipient.OnMotivationGroupChanged(FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                cmbKeyMinistries,
                cmbDetailMotivationGroupCode,
                ref cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                FRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                ref FMotivationGroup,
                ref FMotivationDetail,
                ref FMotivationDetailChanged,
                FActiveOnly,
                FCreatingNewGift,
                FInRecipientKeyChanging,
                FInEditMode,
                ref FAutoPopComment);

            if (!FInRecipientKeyChanging)
            {
                ValidateRecipientLedgerNumber();
            }
        }

        private void MotivationDetailChanged(object sender, EventArgs e)
        {
            string PrevAutoPopComment = FAutoPopComment;

            TUC_RecurringGiftTransactions_Recipient.OnMotivationDetailChanged(FPreviouslySelectedDetailRow,
                FMainDS,
                FLedgerNumber,
                FPetraUtilsObject,
                cmbKeyMinistries,
                cmbDetailMotivationDetailCode,
                txtDetailRecipientKey,
                FRecipientKey,
                txtDetailRecipientLedgerNumber,
                txtDetailCostCentreCode,
                txtDetailAccountCode,
                txtDetailRecipientKeyMinistry,
                chkDetailTaxDeductible,
                FMotivationGroup,
                ref FMotivationDetail,
                ref FMotivationDetailChanged,
                FInRecipientKeyChanging,
                FCreatingNewGift,
                FInEditMode,
                FAutoPopulatingGift,
                ref FAutoPopComment);

            // if the previous motivation detail had a AutoPopDesc set to true then remove this comment for this detail
            if (!txtDetailRecipientKeyMinistry.Visible && !string.IsNullOrEmpty(PrevAutoPopComment))
            {
                UndoAutoPopulateCommentOne(PrevAutoPopComment);
            }

            // if motivation detail has AutoPopDesc set to true and has not already been autopoulated for this detail
            if (!txtDetailRecipientKeyMinistry.Visible
                && !string.IsNullOrEmpty(FAutoPopComment) && (txtDetailGiftCommentOne.Text != FAutoPopComment))
            {
                // autopopulate comment one with the motivation detail description
                AutoPopulateCommentOne(FAutoPopComment);
            }
        }

        private void UndoAutoPopulateCommentOne(string AAutoPopComment)
        {
            if (txtDetailGiftCommentOne.Text == AAutoPopComment)
            {
                txtDetailGiftCommentOne.Text = string.Empty;
            }

            // move any custom comments to fill Comment One
            if (!string.IsNullOrEmpty(txtDetailGiftCommentTwo.Text))
            {
                txtDetailGiftCommentOne.Text = txtDetailGiftCommentTwo.Text;
                cmbDetailCommentOneType.SetSelectedString(cmbDetailCommentTwoType.GetSelectedString(-1));
                txtDetailGiftCommentTwo.Text = "";
                cmbDetailCommentTwoType.SetSelectedString("Both");
            }

            if (!string.IsNullOrEmpty(txtDetailGiftCommentThree.Text))
            {
                txtDetailGiftCommentTwo.Text = txtDetailGiftCommentThree.Text;
                cmbDetailCommentTwoType.SetSelectedString(cmbDetailCommentThreeType.GetSelectedString(-1));
                txtDetailGiftCommentThree.Text = "";
                cmbDetailCommentThreeType.SetSelectedString("Both");
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
                FGiftAmountChanged = true;
            }
        }

        private void ProcessGiftAmountChange()
        {
            FGiftAmountChanged = false;

            if (txtDetailGiftAmount.NumberValueDecimal == null)
            {
                return;
            }

            decimal NewAmount = txtDetailGiftAmount.NumberValueDecimal.Value;

            if (FPreviouslySelectedDetailRow != null)
            {
                FPreviouslySelectedDetailRow.GiftAmount = NewAmount;
            }

            UpdateTotals();
        }

        private void UpdateTotals()
        {
            if ((FPetraUtilsObject == null))
            {
                return;
            }

            Decimal SumTransactions = 0;
            Decimal SumBatch = 0;
            Int32 GiftNumber = 0;

            //Sometimes a change in an unbound textbox causes a data changed condition
            bool SaveButtonWasEnabled = FPetraUtilsObject.HasChanges;
            bool DataChanges = false;

            if (FPreviouslySelectedDetailRow == null)
            {
                if ((txtGiftTotal.NumberValueDecimal.HasValue && (txtGiftTotal.NumberValueDecimal.Value != 0))
                    || (txtBatchTotal.NumberValueDecimal.HasValue && (txtBatchTotal.NumberValueDecimal.Value != 0)))
                {
                    txtGiftTotal.NumberValueDecimal = 0;
                    txtBatchTotal.NumberValueDecimal = 0;
                }

                //If all details have been deleted
                if ((FLedgerNumber != -1) && (FBatchRow != null) && (grdDetails.Rows.Count == 1))
                {
                    //((TFrmRecurringGiftBatch) this.ParentForm).GetBatchControl().UpdateBatchTotal(0, FBatchRow.BatchNumber);
                    //Now we look at the batch and update the batch data
                    if (FBatchRow.BatchTotal != SumBatch)
                    {
                        FBatchRow.BatchTotal = SumBatch;
                        DataChanges = true;
                    }
                }
            }
            else
            {
                GiftNumber = FPreviouslySelectedDetailRow.GiftTransactionNumber;

                DataView giftDetailDV = new DataView(FMainDS.ARecurringGiftDetail);
                giftDetailDV.RowStateFilter = DataViewRowState.CurrentRows;

                giftDetailDV.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    FBatchNumber);

                foreach (DataRowView drv in giftDetailDV)
                {
                    ARecurringGiftDetailRow gdr = (ARecurringGiftDetailRow)drv.Row;

                    if (gdr.GiftTransactionNumber == GiftNumber)
                    {
                        if (FPreviouslySelectedDetailRow.DetailNumber == gdr.DetailNumber)
                        {
                            SumTransactions += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                            SumBatch += Convert.ToDecimal(txtDetailGiftAmount.NumberValueDecimal);
                        }
                        else
                        {
                            SumTransactions += gdr.GiftAmount;
                            SumBatch += gdr.GiftAmount;
                        }
                    }
                    else
                    {
                        SumBatch += gdr.GiftAmount;
                    }
                }

                if ((txtGiftTotal.NumberValueDecimal.HasValue == false) || (txtGiftTotal.NumberValueDecimal.Value != SumTransactions))
                {
                    txtGiftTotal.NumberValueDecimal = SumTransactions;
                }

                txtGiftTotal.CurrencyCode = txtDetailGiftAmount.CurrencyCode;
                txtGiftTotal.ReadOnly = true;

                //Now we look at the batch and update the batch data
                if (FBatchRow.BatchTotal != SumBatch)
                {
                    FBatchRow.BatchTotal = SumBatch;
                    DataChanges = true;
                }
            }

            if (txtBatchTotal.NumberValueDecimal.Value != SumBatch)
            {
                txtBatchTotal.NumberValueDecimal = SumBatch;
            }

            if (!DataChanges && !SaveButtonWasEnabled && FPetraUtilsObject.HasChanges)
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
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widths needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
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

        /// <summary>
        /// Clear the gift data of the current batch without marking records for delete
        /// </summary>
        private bool RefreshCurrentRecurringBatchGiftData(Int32 ABatchNumber,
            bool AAcceptChanges = false,
            bool AHandleDataSetBackup = false)
        {
            bool RetVal = false;

            //Copy and backup the current dataset
            GiftBatchTDS BackupDS = null;
            GiftBatchTDS TempDS = (GiftBatchTDS)FMainDS.Copy();

            TempDS.Merge(FMainDS);

            if (AHandleDataSetBackup)
            {
                BackupDS = (GiftBatchTDS)FMainDS.Copy();
                BackupDS.Merge(FMainDS);
            }

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

                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringGiftTransactionsForBatch(FLedgerNumber, ABatchNumber));

                if (AAcceptChanges)
                {
                    FMainDS.AcceptChanges();
                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                //If not revert on error then calling method will
                if (AHandleDataSetBackup)
                {
                    RevertDataSet(FMainDS, BackupDS);
                }

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return RetVal;
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

                FMainDS.ARecurringGift.DefaultView.RowFilter = String.Format("{0}={1}",
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    FBatchNumber);
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

                chkDetailActive.Checked = false;
                dtpDetailStartDonations.Clear();
                dtpDetailEndDonations.Clear();
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
            //TODO: Remove?
            if (ARow == null)
            {
                UpdateControlsProtection(ARow);
                return;
            }

            bool ShowGiftDetail = EditableGiftDetail(FPreviouslySelectedDetailRow);

            if (TUC_RecurringGiftTransactions_Recipient.OnStartShowDetailsManual(ARow,
                    cmbKeyMinistries,
                    cmbDetailMotivationGroupCode,
                    cmbDetailMotivationDetailCode,
                    txtDetailRecipientKeyMinistry,
                    ref FMotivationGroup,
                    ref FMotivationDetail,
                    ShowGiftDetail,
                    FGiftTransactionsLoaded,
                    FInEditMode))
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    bool? DoEnableRecipientGiftDestination;
                    TUC_RecurringGiftTransactions_Recipient.FinishShowDetailsManual(ARow,
                        cmbDetailMotivationDetailCode,
                        txtDetailRecipientKey,
                        txtDetailRecipientLedgerNumber,
                        txtDetailCostCentreCode,
                        txtDetailAccountCode,
                        cmbDetailCommentOneType,
                        cmbDetailCommentTwoType,
                        cmbDetailCommentThreeType,
                        ref FMotivationGroup,
                        ref FMotivationDetail,
                        out DoEnableRecipientGiftDestination);

                    if (DoEnableRecipientGiftDestination.HasValue)
                    {
                        mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
                    }

                    // set FAutoPopComment if needed
                    AMotivationDetailRow motivationDetail = (AMotivationDetailRow)FMainDS.AMotivationDetail.Rows.Find(
                        new object[] { FLedgerNumber, FMotivationGroup, FMotivationDetail });

                    if (motivationDetail != null)
                    {
                        // if motivation detail autopopulation is set to true
                        if (motivationDetail.Autopopdesc)
                        {
                            FAutoPopComment = motivationDetail.MotivationDetailDesc;
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
        }

        /// <summary>
        /// displays information about the donor
        /// </summary>
        /// <param name="APartnerKey"></param>
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
                ARecurringGiftRow GiftRow = (ARecurringGiftRow)FMainDS.ARecurringGift.Rows.Find(new object[] { FLedgerNumber, FBatchNumber,
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

        /// <summary>
        /// set the currency symbols for the currency field from outside
        /// </summary>
        public void UpdateCurrencySymbols(String ACurrencyCode)
        {
            if (txtDetailGiftAmount.CurrencyCode != ACurrencyCode)
            {
                txtDetailGiftAmount.CurrencyCode = ACurrencyCode;
            }

            if ((txtGiftTotal.CurrencyCode != ACurrencyCode)
                || (txtBatchTotal.CurrencyCode != ACurrencyCode)
                || (txtHashTotal.CurrencyCode != ACurrencyCode))
            {
                txtGiftTotal.CurrencyCode = ACurrencyCode;
                txtBatchTotal.CurrencyCode = ACurrencyCode;
                txtHashTotal.CurrencyCode = ACurrencyCode;
            }
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
                //No transactions exist to process
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
            DataView giftView = new DataView(FMainDS.ARecurringGift);

            giftView.RowStateFilter = DataViewRowState.CurrentRows;
            giftView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftTable.GetBatchNumberDBName(),
                BatchNumber);

            foreach (DataRowView drv in giftView)
            {
                ARecurringGiftRow giftRow = (ARecurringGiftRow)drv.Row;
                giftRow.MethodOfPaymentCode = FBatchMethodOfPayment;
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
            bool FirstIsEnabled = (ARow != null) && (ARow.DetailNumber == 1);
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

                if (chkDetailActive.Checked.Equals(null))
                {
                    giftRow.SetActiveNull();
                }
                else
                {
                    giftRow.Active = chkDetailActive.Checked;
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
                TUC_RecurringGiftTransactions_Recipient.UpdateRecipientKeyText(ARow.RecipientKey,
                    ARow,
                    cmbDetailMotivationGroupCode.GetSelectedString(),
                    cmbDetailMotivationDetailCode.GetSelectedString());
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

        private void ValidateDataDetailsManual(GiftBatchTDSARecurringGiftDetailRow ARow)
        {
            FBatchRow = GetRecurringBatchRow();

            if ((ARow == null)
                || (ARow.RowState == DataRowState.Deleted)
                || (FBatchRow == null)
                || (FBatchRow.BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            //Process amounts, including taxes and currencies
            if (FGiftAmountChanged)
            {
                ProcessGiftAmountChange();
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            //It is necessary to validate the unbound control for date entered. This requires us to pass the control.
            ARecurringGiftRow giftRow = GetRecurringGiftRow(ARow.GiftTransactionNumber);

            TSharedFinanceValidation_Gift.ValidateRecurringGiftManual(this,
                giftRow,
                ref VerificationResultCollection,
                FValidationControlsDict,
                FDonorZeroIsValid);

            //Validate gift detail level
            TSharedFinanceValidation_Gift.ValidateRecurringGiftDetailManual(this,
                ARow,
                ref VerificationResultCollection,
                FValidationControlsDict,
                -1,
                FRecipientZeroIsValid);

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

        private void ValidateRecipientLedgerNumber()
        {
            if (!FInEditMode || (FPreviouslySelectedDetailRow.RecipientKey == 0))
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
        /// Refresh the dataset for this form
        /// </summary>
        public void RefreshData()
        {
            Cursor PrevCursor = ParentForm.Cursor;

            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;

                if ((FMainDS != null) && (FMainDS.ARecurringGiftDetail != null))
                {
                    FMainDS.ARecurringGift.Rows.Clear();
                    FMainDS.ARecurringGiftDetail.Rows.Clear();
                }

                // Get the current batch row from the batch tab
                FBatchRow = GetRecurringBatchRow();

                if (FBatchRow != null)
                {
                    // Be sure to pass the true parameter because we definitely need to update FMainDS.ARecurringGiftDetail as it is now empty!
                    LoadRecurringGifts(FBatchRow.LedgerNumber, FBatchRow.BatchNumber, true);
                }
            }
            finally
            {
                ParentForm.Cursor = PrevCursor;
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChanged && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();

                // Once we have auto-sized once and there are more than 8 rows we don't auto-size any more (unless we load data again)
                FSuppressListChanged = (grdDetails.Rows.Count > 8);
            }
        }

        /// Select a special gift detail number from outside
        public void SelectRecurringGiftDetailNumber(Int32 ARecurringGiftNumber, Int32 ARecurringGiftDetailNumber)
        {
            DataView myView = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView;

            for (int counter = 0; (counter < myView.Count); counter++)
            {
                int myViewGiftNumber = (int)myView[counter][2];
                int myViewGiftDetailNumber = (int)(int)myView[counter][3];

                if ((myViewGiftNumber == ARecurringGiftNumber) && (myViewGiftDetailNumber == ARecurringGiftDetailNumber))
                {
                    SelectRowInGrid(counter + 1);
                    break;
                }
            }
        }

        private void OpenDonorHistory(System.Object sender, EventArgs e)
        {
            TCommonScreensForwarding.OpenDonorRecipientHistoryScreen(true,
                Convert.ToInt64(txtDetailDonorKey.Text),
                FPetraUtilsObject.GetForm());
        }

        private void OpenDonorFinanceDetails(System.Object sender, EventArgs e)
        {
            TFrmPartnerEdit frmPartner = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frmPartner.SetParameters(TScreenMode.smEdit,
                FPreviouslySelectedDetailRow.DonorKey,
                Shared.MPartner.TPartnerEditTabPageEnum.petpFinanceDetails);
            frmPartner.Show();
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
            bool? DoEnableRecipientGiftDestination;

            TUC_RecurringGiftTransactions_Recipient.OnRecipientPartnerClassChanged(APartnerClass,
                txtDetailRecipientKey,
                txtDetailRecipientLedgerNumber,
                out DoEnableRecipientGiftDestination);

            if (DoEnableRecipientGiftDestination.HasValue)
            {
                mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
            }
        }

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
                    TUC_RecurringGiftTransactions_Recipient.GetRecipientData(FPreviouslySelectedDetailRow,
                        Convert.ToInt64(txtDetailRecipientKey.Text),
                        ref cmbKeyMinistries,
                        txtDetailRecipientKey,
                        ref txtDetailRecipientLedgerNumber,
                        false);

                    break;
                }
            }
        }
    }
}