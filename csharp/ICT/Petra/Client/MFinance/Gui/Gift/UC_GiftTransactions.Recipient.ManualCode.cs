//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
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
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic static class that handles the interactions between controls in the recipient section of
    /// the gift transaction on a details panel.
    /// </summary>
    public static class TUC_GiftTransactions_Recipient
    {
        #region Initialisation

        /// <summary>
        /// Manage the overlay
        /// </summary>
        public static void SetTextBoxOverlayOnKeyMinistryCombo(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, bool AActiveOnly,
            TCmbAutoPopulated ACmbKeyMinistries, TCmbAutoPopulated ACmbMotivationDetailCode, TextBox ATxtDetailRecipientKeyMinistry,
            ref string AMotivationDetail, bool AInEditModeFlag, bool ABatchUnpostedFlag, bool AReadComboValue = false)
        {
            ResetMotivationDetailCodeFilter(ACmbMotivationDetailCode, ref AMotivationDetail, AActiveOnly);

            // Always enabled initially. Combobox may be diabled later once populated.
            ACmbKeyMinistries.Enabled = true;

            ATxtDetailRecipientKeyMinistry.Visible = true;
            ATxtDetailRecipientKeyMinistry.BringToFront();
            ATxtDetailRecipientKeyMinistry.Parent.Refresh();

            if (AReadComboValue)
            {
                ReconcileKeyMinistryFromCombo(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag,
                    ABatchUnpostedFlag);
            }
            else
            {
                ReconcileKeyMinistryFromTextbox(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag,
                    ABatchUnpostedFlag);
            }
        }

        #endregion

        #region ShowDetails

        /// <summary>
        /// Call from ShowDetailsManual
        /// </summary>
        public static bool OnStartShowDetailsManual(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationDetailCode, TextBox ATxtDetailRecipientKeyMinistry, ref string AMotivationDetail, bool AActiveOnly,
            bool ATransactionsLoadedFlag, bool AInEditModeFlag, bool ABatchUnpostedFlag)
        {
            if (!ATxtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow, AActiveOnly, ACmbKeyMinistries, ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry, ref AMotivationDetail, AInEditModeFlag, ABatchUnpostedFlag, true);
            }
            else if (!ATransactionsLoadedFlag)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow, AActiveOnly, ACmbKeyMinistries, ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry, ref AMotivationDetail, AInEditModeFlag, ABatchUnpostedFlag);
            }

            if (ACurrentDetailRow == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Call from ShowDetailsManual after ACurrentDetailRow is known to be non-NULL
        /// </summary>
        public static void FinishShowDetailsManual(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey, TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode, TextBox ATxtDetailAccountCode, ref string AMotivationGroup, ref string AMotivationDetail,
            out bool? AEnableRecipientHistory, out bool ? AEnableRecipientGiftDestination)
        {
            AEnableRecipientGiftDestination = null;
            AEnableRecipientHistory = null;

            //Record current values for motivation
            AMotivationGroup = ACurrentDetailRow.MotivationGroupCode;
            AMotivationDetail = ACurrentDetailRow.MotivationDetailCode;

            if (ACurrentDetailRow.IsCostCentreCodeNull())
            {
                ATxtDetailCostCentreCode.Text = string.Empty;
            }
            else
            {
                ATxtDetailCostCentreCode.Text = ACurrentDetailRow.CostCentreCode;
            }

            if (ACurrentDetailRow.IsAccountCodeNull())
            {
                ATxtDetailAccountCode.Text = string.Empty;
            }
            else
            {
                ATxtDetailAccountCode.Text = ACurrentDetailRow.AccountCode;
            }

            if (ACurrentDetailRow.IsRecipientKeyNull())
            {
                ATxtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                UpdateRecipientKeyText(0, ACurrentDetailRow, ACmbMotivationDetailCode);
            }
            else
            {
                ATxtDetailRecipientKey.Text = String.Format("{0:0000000000}", ACurrentDetailRow.RecipientKey);
                UpdateRecipientKeyText(ACurrentDetailRow.RecipientKey, ACurrentDetailRow, ACmbMotivationDetailCode);
            }

            if (Convert.ToInt64(ATxtDetailRecipientKey.Text) == 0)
            {
                AEnableRecipientHistory = false;
                OnRecipientPartnerClassChanged(null, ATxtDetailRecipientKey, AtxtDetailRecipientLedgerNumber, out AEnableRecipientGiftDestination);
            }
            else
            {
                AEnableRecipientHistory = true;

                if (Convert.ToInt64(AtxtDetailRecipientLedgerNumber.Text) == 0)
                {
                    OnRecipientPartnerClassChanged(null, ATxtDetailRecipientKey, AtxtDetailRecipientLedgerNumber, out AEnableRecipientGiftDestination);
                }
            }

            //if (ACurrentDetailRow.IsRecipientLedgerNumberNull())
            //{
            //    AtxtDetailRecipientLedgerNumber.Text = String.Format("{0:0000000000}", 0);
            //}
            //else
            //{
            //    AtxtDetailRecipientLedgerNumber.Text = String.Format("{0:0000000000}", ACurrentDetailRow.RecipientLedgerNumber);
            //}
        }

        #endregion

        #region Main public change event handlers

        /// <summary>
        /// Call when the Motivation Detail changes
        /// </summary>
        public static void OnMotivationDetailChanged(GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            Int64 ARecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            TextBox ATxtDeductibleAccount,
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewGiftFlag,
            bool AInEditModeFlag,
            bool ABatchUnpostedFlag,
            bool ATaxDeductiblePercentageEnabledFlag,
            out bool ADoTaxUpdate)
        {
            ADoTaxUpdate = false;

            if (!ABatchUnpostedFlag || !AInEditModeFlag || ATxtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            Int64 MotivationRecipientKey = 0;
            AMotivationDetail = ACmbMotivationDetailCode.GetSelectedString();

            if (AMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, AMotivationGroup, AMotivationDetail });

                ACmbMotivationDetailCode.RefreshLabel();

                if (motivationDetail != null)
                {
                    RetrieveMotivationDetailAccountCode(AMainDS, ALedgerNumber, ATxtDetailAccountCode, ATxtDeductibleAccount,
                        AMotivationGroup, AMotivationDetail, ATaxDeductiblePercentageEnabledFlag);

                    MotivationRecipientKey = motivationDetail.RecipientKey;

                    // set tax deductible checkbox if motivation detail has been changed by the user (i.e. not a row change)
                    if (!APetraUtilsObject.SuppressChangeDetection || ARecipientKeyChangingFlag)
                    {
                        AChkDetailTaxDeductible.Checked = motivationDetail.TaxDeductible;
                    }

                    if (ATaxDeductiblePercentageEnabledFlag)
                    {
                        if (string.IsNullOrEmpty(motivationDetail.TaxDeductibleAccount))
                        {
                            MessageBox.Show(Catalog.GetString("This Motivation Detail does not have an associated Tax Deductible Account. " +
                                    "This can be added in Finance / Setup / Motivation Details.\n\n" +
                                    "Unless this is changed it will be impossible to assign a Tax Deductible Percentage to this gift."),
                                Catalog.GetString("Incomplete Motivation Detail"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        ADoTaxUpdate = true;
                    }
                }
                else
                {
                    AChkDetailTaxDeductible.Checked = false;
                }
            }

            if (!ACreatingNewGiftFlag && (MotivationRecipientKey > 0))
            {
                AMotivationDetailChangedFlag = true;
                PopulateKeyMinistry(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    MotivationRecipientKey);
                AMotivationDetailChangedFlag = false;
            }
            else if (ARecipientKey == 0)
            {
                UpdateRecipientKeyText(0, ACurrentDetailRow, ACmbMotivationDetailCode);
            }

            RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
        }

        /// <summary>
        /// Call when the recipient key changes
        /// </summary>
        public static void OnRecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection,
            GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            ref TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationGroupCode,
            TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            TextBox ATxtDeductibleAccount,
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            bool AShowingDetailsFlag,
            ref bool ARecipientKeyChangingFlag,
            bool AInKeyMinistryChangingFlag,
            bool AInEditModeFlag,
            bool ABatchUnpostedFlag,
            bool AMotivationDetailChangedFlag,
            bool ATaxDeductiblePercentageEnabledFlag,
            bool AActiveOnly,
            out bool? AEnableRecipientHistory,
            out bool ADoValidateGiftDestination,
            out bool ADoTaxUpdate)
        {
            AEnableRecipientHistory = null;
            ADoValidateGiftDestination = false;
            ADoTaxUpdate = false;

            if (ARecipientKeyChangingFlag || APetraUtilsObject.SuppressChangeDetection || AShowingDetailsFlag)
            {
                return;
            }

            ARecipientKeyChangingFlag = true;
            ATxtDetailRecipientKeyMinistry.Text = string.Empty;

            try
            {
                ATxtDetailRecipientKey.Text = APartnerKey.ToString();
                ACurrentDetailRow.RecipientKey = Convert.ToInt64(APartnerKey);
                ACurrentDetailRow.RecipientDescription = APartnerShortName;

                if (!AMotivationDetailChangedFlag && TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetail(
                        APartnerKey, ref AMotivationGroup, ref AMotivationDetail))
                {
                    if (AMotivationGroup != ACmbMotivationGroupCode.GetSelectedString())
                    {
                    	// note - this will also update the Motivation Detail
                        ACmbMotivationGroupCode.SetSelectedString(AMotivationGroup);
                    }
                    else if (AMotivationDetail != ACmbMotivationDetailCode.GetSelectedString())
                    {
                        ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
                    }
                    
                    ACurrentDetailRow.MotivationGroupCode = AMotivationGroup;
                    ACurrentDetailRow.MotivationDetailCode = AMotivationDetail;
                }

                APetraUtilsObject.SuppressChangeDetection = true;

                //Set RecipientLedgerNumber
                if (APartnerKey > 0)
                {
                    ACurrentDetailRow.RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey, null);
                }
                else
                {
                    ACurrentDetailRow.RecipientLedgerNumber = 0;
                }

                if (APartnerKey > 0)
                {
                    RetrieveRecipientCostCentreCode(APartnerKey,
                        ACurrentDetailRow,
                        AMainDS,
                        AtxtDetailRecipientLedgerNumber,
                        ALedgerNumber,
                        ATxtDetailCostCentreCode,
                        AInKeyMinistryChangingFlag);
                    AEnableRecipientHistory = true;
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey, ACurrentDetailRow, ACmbMotivationDetailCode);
                    RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
                    AEnableRecipientHistory = false;
                }

                if (!AInKeyMinistryChangingFlag)
                {
                    GetRecipientData(ACurrentDetailRow,
                        APartnerKey,
                        ref ACmbKeyMinistries,
                        ATxtDetailRecipientKey,
                        ref AtxtDetailRecipientLedgerNumber);
                    ADoValidateGiftDestination = true;
                }

                if (ATaxDeductiblePercentageEnabledFlag)
                {
                    ADoTaxUpdate = true;
                }
            }
            finally
            {
                ARecipientKeyChangingFlag = false;
                ReconcileKeyMinistryFromCombo(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag,
                    ABatchUnpostedFlag);
                APetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        /// <summary>
        /// Modifies menu items depending on the Recipeint's Partner class
        /// </summary>
        public static void OnRecipientPartnerClassChanged(TPartnerClass? APartnerClass, TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber, out bool ? AEnableRecipientGiftDestination)
        {
            AEnableRecipientGiftDestination = null;

            string ItemText = Catalog.GetString("Open Recipient Gift Destination");

            if ((APartnerClass == TPartnerClass.UNIT) || (APartnerClass == null))
            {
                ATxtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
                AtxtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, false);
                AEnableRecipientGiftDestination = false;
            }
            else if (APartnerClass == TPartnerClass.FAMILY)
            {
                ATxtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
                AtxtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, true);
                AEnableRecipientGiftDestination = true;
            }
        }

        /// <summary>
        /// Call when the motivation group changes
        /// </summary>
        public static void OnMotivationGroupChanged(GiftBatchTDSAGiftDetailRow AGiftBatchDetail,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationGroupCode,
            ref TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            Int64 ARecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            TextBox ATxtDeductibleAccount,
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ACreatingNewGiftFlag,
            bool ARecipientKeyChangingFlag,
            bool AInEditModeFlag,
            bool ABatchUnpostedFlag,
            bool ATaxDeductiblePercentageEnabledFlag,
            out bool ADoTaxUpdate)
        {
            if (!ABatchUnpostedFlag || APetraUtilsObject.SuppressChangeDetection || !AInEditModeFlag || ATxtDetailRecipientKeyMinistry.Visible)
            {
                ADoTaxUpdate = false;
                return;
            }

            AMotivationGroup = ACmbMotivationGroupCode.GetSelectedString();
            
            if (!ARecipientKeyChangingFlag)
            {
            	AMotivationDetail = string.Empty;
            }

            ApplyMotivationDetailCodeFilter(AGiftBatchDetail,
                AMainDS,
                ALedgerNumber,
                APetraUtilsObject,
                ACmbKeyMinistries,
                ref ACmbMotivationDetailCode,
                ATxtDetailRecipientKey,
                ARecipientKey,
                AtxtDetailRecipientLedgerNumber,
                ATxtDetailCostCentreCode,
                ATxtDetailAccountCode,
                ATxtDetailRecipientKeyMinistry,
                AChkDetailTaxDeductible,
                ATxtDeductibleAccount,
                AMotivationGroup,
                ref AMotivationDetail,
                ref AMotivationDetailChangedFlag,
                AActiveOnly,
                ARecipientKeyChangingFlag,
                ACreatingNewGiftFlag,
                AInEditModeFlag,
                ABatchUnpostedFlag,
                ATaxDeductiblePercentageEnabledFlag,
                out ADoTaxUpdate);
        }

        /// <summary>
        /// Call when the key ministry changes
        /// </summary>
        public static void OnKeyMinistryChanged(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries, TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey, TextBox ATxtDetailRecipientKeyMinistry,
            bool ARecipientKeyChangingFlag, ref bool AInKeyMinistryChangingFlag)
        {
            if ((ACurrentDetailRow == null) || AInKeyMinistryChangingFlag || ARecipientKeyChangingFlag
                || APetraUtilsObject.SuppressChangeDetection || ATxtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            string KeyMinistry = ACmbKeyMinistries.GetSelectedDescription();
            string RecipientKey = ACmbKeyMinistries.GetSelectedInt64().ToString();

            try
            {
                AInKeyMinistryChangingFlag = true;

                if (ACmbKeyMinistries.Count == 0)
                {
                    ACmbKeyMinistries.SelectedIndex = -1;

                    if (ATxtDetailRecipientKeyMinistry.Text != string.Empty)
                    {
                        ATxtDetailRecipientKeyMinistry.Text = string.Empty;
                    }
                }
                else
                {
                	// if key ministry has actually changed
                    if ((ATxtDetailRecipientKeyMinistry.Text != KeyMinistry)
                        || (ACurrentDetailRow.RecipientKeyMinistry != KeyMinistry))
                    {
                        ATxtDetailRecipientKeyMinistry.Text = KeyMinistry;
                        ACurrentDetailRow.RecipientKeyMinistry = KeyMinistry;
                    }

                	if (Convert.ToInt64(ATxtDetailRecipientKey.Text) != Convert.ToInt64(RecipientKey))
                    {
                        ATxtDetailRecipientKey.Text = RecipientKey;
                    }
                }
            }
            finally
            {
                AInKeyMinistryChangingFlag = false;
            }
        }

        #endregion

        #region Other public methods

        /// <summary>
        /// UpdateRecipientKeyText
        /// </summary>
        public static void UpdateRecipientKeyText(Int64 APartnerKey,
            GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbMotivationDetailCode)
        {
            if ((APartnerKey == 0) && (ACurrentDetailRow != null))
            {
                ACurrentDetailRow.RecipientDescription = ACmbMotivationDetailCode.GetSelectedString();
            }
        }

        /// <summary>
        /// SetKeyMinistryTextBoxInvisible
        /// </summary>
        public static void SetKeyMinistryTextBoxInvisible(GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries,
            ref TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            Int64 ARecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            TextBox ATxtDeductibleAccount,
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewGiftFlag,
            bool AInEditModeFlag,
            bool ABatchUnpostedFlag,
            bool ATaxDeductiblePercentageEnabledFlag,
            out bool ADoTaxUpdate)
        {
            if (ATxtDetailRecipientKeyMinistry.Visible)
            {
                ApplyMotivationDetailCodeFilter(ACurrentDetailRow,
                    AMainDS,
                    ALedgerNumber,
                    APetraUtilsObject,
                    ACmbKeyMinistries,
                    ref ACmbMotivationDetailCode,
                    ATxtDetailRecipientKey,
                    ARecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    ATxtDetailCostCentreCode,
                    ATxtDetailAccountCode,
                    ATxtDetailRecipientKeyMinistry,
                    AChkDetailTaxDeductible,
                    ATxtDeductibleAccount,
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    AActiveOnly,
                    ARecipientKeyChangingFlag,
                    ACreatingNewGiftFlag,
                    AInEditModeFlag,
                    ABatchUnpostedFlag,
                    ATaxDeductiblePercentageEnabledFlag,
                    out ADoTaxUpdate);

                PopulateKeyMinistry(ACurrentDetailRow, ACmbKeyMinistries, ATxtDetailRecipientKey, AtxtDetailRecipientLedgerNumber);

                ReconcileKeyMinistryFromTextbox(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag,
                    ABatchUnpostedFlag);

                //hide the overlay box during editing
                ATxtDetailRecipientKeyMinistry.Visible = false;
            }
            else
            {
                ADoTaxUpdate = false;
            }
        }

        /// <summary>
        /// OnEndEditMode
        /// </summary>
        public static void OnEndEditMode(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationDetailCode, TextBox ATxtDetailRecipientKeyMinistry,
            ref string AMotivationDetail, bool AActiveOnly, bool AInEditModeFlag, bool ABatchUnpostedFlag)
        {
            if (!ATxtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow,
                    AActiveOnly,
                    ACmbKeyMinistries,
                    ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry,
                    ref AMotivationDetail,
                    AInEditModeFlag,
                    ABatchUnpostedFlag);
            }
        }

        /// <summary>
        /// GetRecipientData
        /// </summary>
        public static void GetRecipientData(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, long APartnerKey, ref TCmbAutoPopulated ACmbKeyMinistries,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey, ref TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber)
        {
            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(ATxtDetailRecipientKey.Text);
            }

            TFinanceControls.GetRecipientData(ref ACmbKeyMinistries, ref AtxtDetailRecipientLedgerNumber, APartnerKey, true);

            // enable / disable combo box depending on whether it contains any key ministries
            if (ACmbKeyMinistries.Table.Rows.Count == 0)
            {
                ACmbKeyMinistries.Enabled = false;
            }
            else
            {
                ACmbKeyMinistries.Enabled = true;
            }

            // if recipient is a family then we will need to find their Gift Destination
            if ((ATxtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY) && (APartnerKey != 0))
            {
                AtxtDetailRecipientLedgerNumber.Text = TRemote.MFinance.Gift.WebConnectors.GetGiftDestinationForRecipient(APartnerKey,
                    ACurrentDetailRow.DateEntered).ToString();
            }
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public static void ReconcileKeyMinistryFromCombo(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbKeyMinistries,
            TextBox ATxtDetailRecipientKeyMinistry, bool AInEditModeFlag, bool ABatchUnpostedFlag)
        {
            string KeyMinistry = string.Empty;
            bool EmptyRow = (ACurrentDetailRow == null);

            if (ABatchUnpostedFlag && AInEditModeFlag)
            {
                if (!EmptyRow && (ACmbKeyMinistries.SelectedIndex > -1))
                {
                    KeyMinistry = ACmbKeyMinistries.GetSelectedDescription();
                }

                ATxtDetailRecipientKeyMinistry.Text = KeyMinistry;
            }
        }

        /// <summary>
        /// UpdateAllRecipientDescriptions
        /// </summary>
        public static void UpdateAllRecipientDescriptions(Int32 ABatchNumber, GiftBatchTDS AMainDS)
        {
            DataView giftDetailView = new DataView(AMainDS.AGiftDetail);

            giftDetailView.RowFilter = String.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView rv in giftDetailView)
            {
                GiftBatchTDSAGiftDetailRow row = (GiftBatchTDSAGiftDetailRow)rv.Row;

                if ((row.RecipientKey == 0) && (row.RecipientDescription != row.MotivationDetailCode))
                {
                    row.RecipientDescription = row.MotivationDetailCode;
                }
            }
        }

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        public static void RetrieveMotivationDetailAccountCode(GiftBatchTDS AMainDS, Int32 ALedgerNumber, TextBox ATxtDetailAccountCode,
            TextBox ATxtDeductibleAccount, string AMotivationGroup, string AMotivationDetail, bool ATaxDeductiblePercentageEnabledFlag)
        {
            string AcctCode = string.Empty;
            string TaxDeductibleAcctCode = string.Empty;

            if (AMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, AMotivationGroup, AMotivationDetail });

                if (motivationDetail != null)
                {
                    AcctCode = motivationDetail.AccountCode;

                    if (ATaxDeductiblePercentageEnabledFlag)
                    {
                        TaxDeductibleAcctCode = motivationDetail.TaxDeductibleAccount;
                    }
                }
            }

            ATxtDetailAccountCode.Text = AcctCode;
            ATxtDeductibleAccount.Text = TaxDeductibleAcctCode;
        }

        /// <summary>
        /// RetrieveMotivationDetailCostCentreCode
        /// </summary>
        public static void RetrieveMotivationDetailCostCentreCode(GiftBatchTDS AMainDS, Int32 ALedgerNumber, TextBox ATxtDetailCostCentreCode,
            string AMotivationGroup, string AMotivationDetail)
        {
            string CostCentreCode = string.Empty;

            if (AMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, AMotivationGroup, AMotivationDetail });

                if (motivationDetail != null)
                {
                    CostCentreCode = motivationDetail.CostCentreCode.ToString();
                }
            }

            ATxtDetailCostCentreCode.Text = CostCentreCode;
        }

        /// <summary>
        /// FindCostCentreCodeForRecipient
        /// </summary>
        public static void FindCostCentreCodeForRecipient(AGiftDetailRow ARow, Int64 APartnerKey, GiftBatchTDS AMainDS, Int32 ALedgerNumber,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber, bool AShowError = false)
        {
            if (ARow == null)
            {
                return;
            }

            string CurrentCostCentreCode = ARow.CostCentreCode;
            string NewCostCentreCode = string.Empty;

            Int64 RecipientField = Convert.ToInt64(AtxtDetailRecipientLedgerNumber.Text);

            string MotivationGroup = ARow.MotivationGroupCode;
            string MotivationDetail = ARow.MotivationDetailCode;

            Int64 RecipientLedgerNumber = ARow.RecipientLedgerNumber;

            Int64 LedgerPartnerKey = AMainDS.ALedger[0].PartnerKey;

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
                    ALedgerNumber,
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
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, MotivationGroup, MotivationDetail });

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
                        ALedgerNumber);

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

        #endregion

        #region Private methods

        /// <summary>
        /// RetrieveRecipientCostCentreCode
        /// </summary>
        private static void RetrieveRecipientCostCentreCode(Int64 APartnerKey,
            GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            Int32 ALedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            bool AInKeyMinistryChangingFlag)
        {
            if (AInKeyMinistryChangingFlag || (ACurrentDetailRow == null))
            {
                return;
            }

            FindCostCentreCodeForRecipient(ACurrentDetailRow, APartnerKey, AMainDS, ALedgerNumber, AtxtDetailRecipientLedgerNumber, false);

            ATxtDetailCostCentreCode.Text = ACurrentDetailRow.CostCentreCode;
        }

        /// <summary>
        /// ResetMotivationDetailCodeFilter
        /// </summary>
        private static void ResetMotivationDetailCodeFilter(TCmbAutoPopulated ACmbMotivationDetailCode,
            ref string AMotivationDetail,
            bool AActiveOnly)
        {
            if ((ACmbMotivationDetailCode.Count == 0) && (ACmbMotivationDetailCode.Filter != null)
                && (!ACmbMotivationDetailCode.Filter.Contains("1 = 2")))
            {
                AMotivationDetail = string.Empty;
                ACmbMotivationDetailCode.RefreshLabel();

                if (AActiveOnly)
                {
                    //This is needed as the code in TFinanceControls.ChangeFilterMotivationDetailList looks for presence of the active only prefix
                    ACmbMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true And 1 = 2";
                }
                else
                {
                    ACmbMotivationDetailCode.Filter = "1 = 2";
                }

                return;
            }

            if (ACmbMotivationDetailCode.Count > 0)
            {
                AMotivationDetail = ACmbMotivationDetailCode.GetSelectedString();
            }

            if (AActiveOnly)
            {
                ACmbMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true";
            }
            else
            {
                ACmbMotivationDetailCode.Filter = string.Empty;
            }

            ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
            ACmbMotivationDetailCode.RefreshLabel();
        }

        /// <summary>
        /// ApplyMotivationDetailCodeFilter
        /// </summary>
        private static void ApplyMotivationDetailCodeFilter(GiftBatchTDSAGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries,
            ref TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            Int64 ARecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            TextBox ATxtDeductibleAccount,
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewGiftFlag,
            bool AInEditModeFlag,
            bool ABatchUnpostedFlag,
            bool ATaxDeductiblePercentageEnabledFlag,
            out bool ADoTaxUpdate)
        {
            //FMotivationbDetail will change by next process
            string motivationDetail = AMotivationDetail;

            ResetMotivationDetailCodeFilter(ACmbMotivationDetailCode, ref AMotivationDetail, AActiveOnly);
            TFinanceControls.ChangeFilterMotivationDetailList(ref ACmbMotivationDetailCode, AMotivationGroup);
            AMotivationDetail = motivationDetail;

            if (AMotivationDetail.Length > 0)
            {
                ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
                ACmbMotivationDetailCode.Text = AMotivationDetail;
                ADoTaxUpdate = false;
            }
            else if (ACmbMotivationDetailCode.Count > 0)
            {
                ACmbMotivationDetailCode.SelectedIndex = 0;

                //Force refresh of label
                OnMotivationDetailChanged(ACurrentDetailRow,
                    AMainDS,
                    ALedgerNumber,
                    APetraUtilsObject,
                    ACmbKeyMinistries,
                    ACmbMotivationDetailCode,
                    ATxtDetailRecipientKey,
                    ARecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    ATxtDetailCostCentreCode,
                    ATxtDetailAccountCode,
                    ATxtDetailRecipientKeyMinistry,
                    AChkDetailTaxDeductible,
                    ATxtDeductibleAccount,
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    ARecipientKeyChangingFlag,
                    ACreatingNewGiftFlag,
                    AInEditModeFlag,
                    ABatchUnpostedFlag,
                    ATaxDeductiblePercentageEnabledFlag,
                    out ADoTaxUpdate);
            }
            else
            {
                ACmbMotivationDetailCode.SelectedIndex = -1;
                //Force refresh of label
                OnMotivationDetailChanged(ACurrentDetailRow,
                    AMainDS,
                    ALedgerNumber,
                    APetraUtilsObject,
                    ACmbKeyMinistries,
                    ACmbMotivationDetailCode,
                    ATxtDetailRecipientKey,
                    ARecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    ATxtDetailCostCentreCode,
                    ATxtDetailAccountCode,
                    ATxtDetailRecipientKeyMinistry,
                    AChkDetailTaxDeductible,
                    ATxtDeductibleAccount,
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    ARecipientKeyChangingFlag,
                    ACreatingNewGiftFlag,
                    AInEditModeFlag,
                    ABatchUnpostedFlag,
                    ATaxDeductiblePercentageEnabledFlag,
                    out ADoTaxUpdate);
            }

            RetrieveMotivationDetailAccountCode(AMainDS, ALedgerNumber, ATxtDetailAccountCode, ATxtDeductibleAccount,
                AMotivationGroup, AMotivationDetail, ATaxDeductiblePercentageEnabledFlag);

            if ((ATxtDetailRecipientKey.Text == string.Empty) || (Convert.ToInt64(ATxtDetailRecipientKey.Text) == 0))
            {
                ATxtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
            }
        }

        /// <summary>
        /// PopulateKeyMinistry
        /// </summary>
        private static void PopulateKeyMinistry(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbKeyMinistries,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey, TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber, long APartnerKey = 0)
        {
            ACmbKeyMinistries.Clear();

            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(ATxtDetailRecipientKey.Text);

                if (APartnerKey == 0)
                {
                    return;
                }
            }
            
            // Create a copy of AtxtDetailRecipientLedgerNumber as we do not want to actually change this combobox.
            // Otherwise it is possible that the form will display data that is not actually in the database.
            TtxtAutoPopulatedButtonLabel temp = new TtxtAutoPopulatedButtonLabel();
            temp.Text = AtxtDetailRecipientLedgerNumber.Text;
  
            GetRecipientData(ACurrentDetailRow, APartnerKey, ref ACmbKeyMinistries, ATxtDetailRecipientKey, ref temp);
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        private static void ReconcileKeyMinistryFromTextbox(GiftBatchTDSAGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbKeyMinistries,
            TextBox ATxtDetailRecipientKeyMinistry, bool AInEditModeFlag, bool ABatchUnpostedFlag)
        {
            if (ABatchUnpostedFlag && AInEditModeFlag)
            {
                bool IsEmptyRow = (ACurrentDetailRow == null);

                if (!IsEmptyRow && (ATxtDetailRecipientKeyMinistry.Text.Length > 0))
                {
                    ACmbKeyMinistries.SetSelectedString(ATxtDetailRecipientKeyMinistry.Text);
                }
                else
                {
                    ACmbKeyMinistries.SelectedIndex = -1;
                }
            }
        }

        #endregion
    }
}