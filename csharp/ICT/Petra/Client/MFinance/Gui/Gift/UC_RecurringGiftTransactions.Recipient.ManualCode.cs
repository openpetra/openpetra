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
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic static class that handles the interactions between controls in the recipient section of
    /// the Recurring Gift transaction on a details panel.
    /// </summary>
    public static class TUC_RecurringGiftTransactions_Recipient
    {
        #region Initialisation

        /// <summary>
        /// Manage the overlay
        /// </summary>
        public static void SetTextBoxOverlayOnKeyMinistryCombo(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            bool AShowRecurringGiftDetail,
            TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationDetailCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            ref string AMotivationDetail,
            bool AInEditModeFlag,
            bool AReadComboValue = false,
            TFrmPetraEditUtils APetraUtilsObject = null)
        {
            ResetMotivationDetailCodeFilter(ACmbMotivationDetailCode, ref AMotivationDetail, AShowRecurringGiftDetail, APetraUtilsObject);

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
                    AInEditModeFlag);
            }
            else
            {
                ReconcileKeyMinistryFromTextbox(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag);
            }
        }

        #endregion

        #region ShowDetails

        /// <summary>
        /// Call from ShowDetailsManual
        /// </summary>
        public static bool OnStartShowDetailsManual(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationGroupCode,
            TCmbAutoPopulated ACmbMotivationDetailCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            bool AShowRecurringGiftDetail,
            bool ATransactionsLoadedFlag,
            bool AInEditModeFlag)
        {
            if (!ATxtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow, AShowRecurringGiftDetail, ACmbKeyMinistries, ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry, ref AMotivationDetail, AInEditModeFlag, true);
            }
            else if (!ATransactionsLoadedFlag)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow, AShowRecurringGiftDetail, ACmbKeyMinistries, ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry, ref AMotivationDetail, AInEditModeFlag);
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
        public static void FinishShowDetailsManual(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow, TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey, TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode, TextBox ATxtDetailAccountCode, Ict.Common.Controls.TCmbAutoComplete ACmbDetailCommentOneType,
            Ict.Common.Controls.TCmbAutoComplete ACmbDetailCommentTwoType, Ict.Common.Controls.TCmbAutoComplete ACmbDetailCommentThreeType,
            ref string AMotivationGroup, ref string AMotivationDetail, out bool ? AEnableRecipientRecurringGiftDestination)
        {
            AEnableRecipientRecurringGiftDestination = null;

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
                UpdateRecipientKeyText(0, ACurrentDetailRow, AMotivationGroup, AMotivationDetail);
            }
            else
            {
                ATxtDetailRecipientKey.Text = String.Format("{0:0000000000}", ACurrentDetailRow.RecipientKey);
                UpdateRecipientKeyText(ACurrentDetailRow.RecipientKey, ACurrentDetailRow, AMotivationGroup, AMotivationDetail);
            }

            if (Convert.ToInt64(ATxtDetailRecipientKey.Text) == 0)
            {
                OnRecipientPartnerClassChanged(null,
                    ATxtDetailRecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    out AEnableRecipientRecurringGiftDestination);
            }

            if (Convert.ToInt64(AtxtDetailRecipientLedgerNumber.Text) == 0)
            {
                OnRecipientPartnerClassChanged(ATxtDetailRecipientKey.CurrentPartnerClass,
                    ATxtDetailRecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    out AEnableRecipientRecurringGiftDestination);
            }

            if (ACurrentDetailRow.IsCommentOneTypeNull())
            {
                ACmbDetailCommentOneType.SetSelectedString("Both");
            }

            if (ACurrentDetailRow.IsCommentTwoTypeNull())
            {
                ACmbDetailCommentTwoType.SetSelectedString("Both");
            }

            if (ACurrentDetailRow.IsCommentThreeTypeNull())
            {
                ACmbDetailCommentThreeType.SetSelectedString("Both");
            }
        }

        #endregion

        #region Main public change event handlers

        /// <summary>
        /// Call when the Motivation Detail changes
        /// </summary>
        public static void OnMotivationDetailChanged(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
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
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewRecurringGiftFlag,
            bool AInEditModeFlag,
            bool AAutoPopulatingRecurringGift,
            ref string AAutoPopComment)
        {
            if (!AInEditModeFlag || ATxtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            Int64 MotivationRecipientKey = 0;
            AMotivationDetail = ACmbMotivationDetailCode.GetSelectedString();

            if (AMotivationDetail.Length > 0)
            {
                ACmbMotivationDetailCode.RefreshLabel();

                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, AMotivationGroup, AMotivationDetail });

                if (motivationDetail != null)
                {
                    RetrieveMotivationDetailAccountCode(motivationDetail,
                        ATxtDetailAccountCode);

                    MotivationRecipientKey = motivationDetail.RecipientKey;

                    // if motivation detail autopopulation is set to true
                    if (motivationDetail.Autopopdesc)
                    {
                        AAutoPopComment = motivationDetail.MotivationDetailDesc;
                    }
                    else
                    {
                        AAutoPopComment = null;
                    }

                    // set tax deductible checkbox if motivation detail has been changed by the user (i.e. not a row change)
                    if (!APetraUtilsObject.SuppressChangeDetection || ARecipientKeyChangingFlag)
                    {
                        if (AChkDetailTaxDeductible.Checked != motivationDetail.TaxDeductible)
                        {
                            AChkDetailTaxDeductible.Checked = motivationDetail.TaxDeductible;
                        }
                    }
                }
                else
                {
                    AChkDetailTaxDeductible.Checked = false;
                }
            }

            if (!ACreatingNewRecurringGiftFlag && !AAutoPopulatingRecurringGift && (MotivationRecipientKey > 0))
            {
                AMotivationDetailChangedFlag = true;
                PopulateKeyMinistry(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKey,
                    AtxtDetailRecipientLedgerNumber,
                    AMotivationDetailChangedFlag,
                    MotivationRecipientKey);
                AMotivationDetailChangedFlag = false;
            }
            else if (ARecipientKey == 0)
            {
                UpdateRecipientKeyText(0, ACurrentDetailRow, AMotivationGroup, AMotivationDetail);
            }

            if (ARecipientKey == 0)
            {
                RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
            }
            else
            {
                string NewCCCode = string.Empty;

                // it is possible that there are no active motivation details and so AMotivationDetail is blank
                if (!string.IsNullOrEmpty(AMotivationDetail))
                {
                    bool partnerIsMissingLink = false;

                    NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(ALedgerNumber,
                        ARecipientKey,
                        ACurrentDetailRow.RecipientLedgerNumber,
                        ACurrentDetailRow.DateEntered,
                        AMotivationGroup,
                        AMotivationDetail,
                        out partnerIsMissingLink);
                }

                if (ATxtDetailCostCentreCode.Text != NewCCCode)
                {
                    ATxtDetailCostCentreCode.Text = NewCCCode;
                }
            }
        }

        /// <summary>
        /// Call when the recipient key changes
        /// </summary>
        public static void OnRecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection,
            GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
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
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            bool AShowingDetailsFlag,
            ref bool ARecipientKeyChangingFlag,
            bool AInKeyMinistryChangingFlag,
            bool AInEditModeFlag,
            bool AMotivationDetailChangedFlag,
            bool ACreatingNewRecurringGiftFlag,
            bool AActiveOnly,
            out bool ADoValidateRecurringGiftDestination)
        {
            ADoValidateRecurringGiftDestination = false;

            if (ARecipientKeyChangingFlag || APetraUtilsObject.SuppressChangeDetection || AShowingDetailsFlag)
            {
                return;
            }

            Int64 RecipientLedgerNumber = 0;
            ACurrentDetailRow.BeginEdit();

            // get the recipient ledger number
            if (APartnerKey > 0)
            {
                RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey,
                    ACurrentDetailRow.DateEntered);
            }

            ARecipientKeyChangingFlag = true;
            ATxtDetailRecipientKeyMinistry.Text = string.Empty;

            try
            {
                ACurrentDetailRow.RecipientKey = APartnerKey;
                ACurrentDetailRow.RecipientDescription = APartnerShortName;
                ACurrentDetailRow.RecipientClass = ATxtDetailRecipientKey.CurrentPartnerClass.ToString();

                APetraUtilsObject.SuppressChangeDetection = true;

                //Set RecipientLedgerNumber
                ACurrentDetailRow.RecipientLedgerNumber = RecipientLedgerNumber;

                if (!AInKeyMinistryChangingFlag)
                {
                    GetRecipientData(ACurrentDetailRow,
                        APartnerKey,
                        ref ACmbKeyMinistries,
                        ATxtDetailRecipientKey,
                        ref AtxtDetailRecipientLedgerNumber,
                        AMotivationDetailChangedFlag);
                    ADoValidateRecurringGiftDestination = true;
                }

                APetraUtilsObject.SuppressChangeDetection = false;

                // do not want to update motivation comboboxes if recipient key is being changed due to a new Recurring Gift or the motivation detail being changed
                if (!AMotivationDetailChangedFlag && !ACreatingNewRecurringGiftFlag
                    && TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetail(APartnerKey, ref AMotivationGroup, ref AMotivationDetail))
                {
                    if (AMotivationGroup != ACmbMotivationGroupCode.GetSelectedString())
                    {
                        // note - this will also update the Motivation Detail
                        ACmbMotivationGroupCode.SetSelectedString(AMotivationGroup);
                    }

                    if (AMotivationDetail != ACmbMotivationDetailCode.GetSelectedString())
                    {
                        ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
                    }

                    ACurrentDetailRow.MotivationGroupCode = AMotivationGroup;
                    ACurrentDetailRow.MotivationDetailCode = AMotivationDetail;
                }

                APetraUtilsObject.SuppressChangeDetection = true;

                if (APartnerKey > 0)
                {
                    RetrieveRecipientCostCentreCode(ACurrentDetailRow, ATxtDetailCostCentreCode);
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey, ACurrentDetailRow, AMotivationGroup, AMotivationDetail);

                    RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
                }
            }
            finally
            {
                ARecipientKeyChangingFlag = false;
                ReconcileKeyMinistryFromCombo(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag);

                ACurrentDetailRow.EndEdit();
                APetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        /// <summary>
        /// Call when the recipient ledger number changes
        /// </summary>
        public static void OnRecipientLedgerNumberChanged(Int32 ALedgerNumber,
            GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TFrmPetraEditUtils APetraUtilsObject,
            TextBox ATxtDetailCostCentreCode,
            bool ARecipientKeyChangingFlag,
            bool AShowingDetailsFlag)
        {
            if (APetraUtilsObject.SuppressChangeDetection || AShowingDetailsFlag || ARecipientKeyChangingFlag)
            {
                return;
            }

            string NewCCCode = string.Empty;

            // it is possible that there are no active motivation details and so AMotivationDetail is blank
            if (!string.IsNullOrEmpty(ACurrentDetailRow.MotivationDetailCode))
            {
                bool partnerIsMissingLink = false;

                NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(ALedgerNumber,
                    ACurrentDetailRow.RecipientKey,
                    ACurrentDetailRow.RecipientLedgerNumber,
                    ACurrentDetailRow.DateEntered,
                    ACurrentDetailRow.MotivationGroupCode,
                    ACurrentDetailRow.MotivationDetailCode,
                    out partnerIsMissingLink);
            }

            if (ATxtDetailCostCentreCode.Text != NewCCCode)
            {
                ATxtDetailCostCentreCode.Text = NewCCCode;
            }
        }

        /// <summary>
        /// Modifies menu items depending on the Recipeint's Partner class
        /// </summary>
        public static void OnRecipientPartnerClassChanged(TPartnerClass? APartnerClass, TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber, out bool ? AEnableRecipientRecurringGiftDestination)
        {
            AEnableRecipientRecurringGiftDestination = null;

            string ItemText = Catalog.GetString("Open Recipient RecurringGift Destination");

            if ((APartnerClass == TPartnerClass.UNIT) || (APartnerClass == null))
            {
                ATxtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
                AtxtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, false);
                AEnableRecipientRecurringGiftDestination = false;
            }
            else if (APartnerClass == TPartnerClass.FAMILY)
            {
                ATxtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
                AtxtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, true);
                AEnableRecipientRecurringGiftDestination = true;
            }
        }

        /// <summary>
        /// Call when the motivation group changes
        /// </summary>
        public static void OnMotivationGroupChanged(GiftBatchTDSARecurringGiftDetailRow ARecurringGiftBatchDetail,
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
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ACreatingNewRecurringGiftFlag,
            bool ARecipientKeyChangingFlag,
            bool AInEditModeFlag,
            ref string AAutoPopComment)
        {
            if (APetraUtilsObject.SuppressChangeDetection || !AInEditModeFlag || ATxtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            AMotivationGroup = ACmbMotivationGroupCode.GetSelectedString();

            if (!ARecipientKeyChangingFlag)
            {
                AMotivationDetail = string.Empty;
            }

            ApplyMotivationDetailCodeFilter(ARecurringGiftBatchDetail,
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
                AMotivationGroup,
                ref AMotivationDetail,
                ref AMotivationDetailChangedFlag,
                AActiveOnly,
                ARecipientKeyChangingFlag,
                ACreatingNewRecurringGiftFlag,
                AInEditModeFlag,
                ref AAutoPopComment);
        }

        /// <summary>
        /// Call when the key ministry changes
        /// </summary>
        public static void OnKeyMinistryChanged(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow, TFrmPetraEditUtils APetraUtilsObject,
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
            GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            string AMotivationGroupCode,
            string AMotivationDetailCode)
        {
            if ((APartnerKey == 0) && (ACurrentDetailRow != null))
            {
                if (AMotivationGroupCode != MFinanceConstants.MOTIVATION_GROUP_GIFT)
                {
                    ACurrentDetailRow.RecipientDescription = AMotivationDetailCode;
                }
                else
                {
                    ACurrentDetailRow.RecipientDescription = string.Empty;
                }
            }
        }

        /// <summary>
        /// SetKeyMinistryTextBoxInvisible
        /// </summary>
        public static void SetKeyMinistryTextBoxInvisible(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TFrmPetraEditUtils APetraUtilsObject,
            TCmbAutoPopulated ACmbKeyMinistries,
            ref TCmbAutoPopulated ACmbMotivationGroupCode,
            ref TCmbAutoPopulated ACmbMotivationDetailCode,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            Int64 ARecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            TextBox ATxtDetailCostCentreCode,
            TextBox ATxtDetailAccountCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            CheckBox AChkDetailTaxDeductible,
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewRecurringGiftFlag,
            bool AInEditModeFlag,
            ref string AAutoPopComment)
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
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    AActiveOnly,
                    ARecipientKeyChangingFlag,
                    ACreatingNewRecurringGiftFlag,
                    AInEditModeFlag,
                    ref AAutoPopComment);

                PopulateKeyMinistry(ACurrentDetailRow, ACmbKeyMinistries, ATxtDetailRecipientKey, AtxtDetailRecipientLedgerNumber, false);

                ReconcileKeyMinistryFromTextbox(ACurrentDetailRow,
                    ACmbKeyMinistries,
                    ATxtDetailRecipientKeyMinistry,
                    AInEditModeFlag);

                //hide the overlay box during editing
                ATxtDetailRecipientKeyMinistry.Visible = false;
            }
        }

        /// <summary>
        /// OnEndEditMode
        /// </summary>
        public static void OnEndEditMode(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbKeyMinistries,
            TCmbAutoPopulated ACmbMotivationGroupCode,
            TCmbAutoPopulated ACmbMotivationDetailCode,
            TextBox ATxtDetailRecipientKeyMinistry,
            ref string AMotivationGroup,
            ref string AMotivationDetail,
            bool AShowRecurringGiftDetail,
            bool AInEditModeFlag,
            TFrmPetraEditUtils APetraUtilsObject)
        {
            if (!ATxtDetailRecipientKeyMinistry.Visible)
            {
                SetTextBoxOverlayOnKeyMinistryCombo(ACurrentDetailRow,
                    AShowRecurringGiftDetail,
                    ACmbKeyMinistries,
                    ACmbMotivationDetailCode,
                    ATxtDetailRecipientKeyMinistry,
                    ref AMotivationDetail,
                    AInEditModeFlag,
                    false,
                    APetraUtilsObject);
            }
        }

        /// <summary>
        /// GetRecipientData
        /// </summary>
        public static void GetRecipientData(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            long APartnerKey,
            ref TCmbAutoPopulated ACmbKeyMinistries,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            ref TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            bool AMotivationDetailChangedFlag)
        {
            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(ATxtDetailRecipientKey.Text);
            }

            // If this method has been called as a result of a change in motivation detail then txtDetailRecipientKey has not yet been set...
            // but we do know that the recipient must be a Unit.

            // if Family Recipient
            if (!AMotivationDetailChangedFlag && (ATxtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY))
            {
                AtxtDetailRecipientLedgerNumber.Text = ACurrentDetailRow.RecipientLedgerNumber.ToString();
                ACmbKeyMinistries.Clear();
                ACmbKeyMinistries.Enabled = false;
            }
            // if Unit Recipient
            else
            {
                //At this point, only active KeyMinistries are allowed in a live Recurring Gift
                bool activeOnly = true;
                TFinanceControls.GetRecipientData(ref ACmbKeyMinistries, ref AtxtDetailRecipientLedgerNumber, APartnerKey, activeOnly);

                // enable / disable combo box depending on whether it contains any key ministries
                if ((ACmbKeyMinistries.Table == null) || (ACmbKeyMinistries.Table.Rows.Count == 0))
                {
                    ACmbKeyMinistries.Enabled = false;
                }
                else
                {
                    ACmbKeyMinistries.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        public static void ReconcileKeyMinistryFromCombo(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbKeyMinistries,
            TextBox ATxtDetailRecipientKeyMinistry,
            bool AInEditModeFlag = true)
        {
            string KeyMinistry = string.Empty;
            bool isEmptyDetailRow = (ACurrentDetailRow == null);

            if (!isEmptyDetailRow && (ACmbKeyMinistries.SelectedIndex > -1))
            {
                KeyMinistry = ACmbKeyMinistries.GetSelectedDescription();
            }

            ATxtDetailRecipientKeyMinistry.Text = KeyMinistry;
        }

        /// <summary>
        /// Keep the combo and textboxes together
        /// </summary>
        private static void ReconcileKeyMinistryFromTextbox(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbKeyMinistries,
            TextBox ATxtDetailRecipientKeyMinistry,
            bool AInEditModeFlag)
        {
            if (AInEditModeFlag)
            {
                bool isEmptyDetailRow = (ACurrentDetailRow == null);
                string keyMinistry = ATxtDetailRecipientKeyMinistry.Text;

                if (!isEmptyDetailRow && (keyMinistry.Length > 0))
                {
                    ACmbKeyMinistries.SetSelectedString(keyMinistry);
                }
                else
                {
                    ACmbKeyMinistries.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// UpdateAllRecipientDescriptions
        /// </summary>
        public static void UpdateAllRecipientDescriptions(Int32 ABatchNumber, GiftBatchTDS AMainDS)
        {
            DataView RecurringGiftDetailView = new DataView(AMainDS.ARecurringGiftDetail);

            RecurringGiftDetailView.RowFilter = String.Format("{0}={1}",
                ARecurringGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView rv in RecurringGiftDetailView)
            {
                GiftBatchTDSARecurringGiftDetailRow row = (GiftBatchTDSARecurringGiftDetailRow)rv.Row;

                if ((row.RecipientKey == 0) && (row.RecipientDescription != row.MotivationDetailCode))
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

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        private static void RetrieveMotivationDetailAccountCode(GiftBatchTDS AMainDS,
            Int32 ALedgerNumber,
            TextBox ATxtDetailAccountCode,
            string AMotivationGroup,
            string AMotivationDetail)
        {
            string AcctCode = string.Empty;

            if (AMotivationDetail.Length > 0)
            {
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, AMotivationGroup, AMotivationDetail });

                if (motivationDetail != null)
                {
                    AcctCode = motivationDetail.AccountCode;
                }
            }

            if (ATxtDetailAccountCode.Text != AcctCode)
            {
                ATxtDetailAccountCode.Text = AcctCode;
            }
        }

        /// <summary>
        /// To be called on the display of a new record
        /// </summary>
        private static void RetrieveMotivationDetailAccountCode(AMotivationDetailRow AMotivationDetail,
            TextBox ATxtDetailAccountCode)
        {
            if (AMotivationDetail != null)
            {
                if (ATxtDetailAccountCode.Text != AMotivationDetail.AccountCode)
                {
                    ATxtDetailAccountCode.Text = AMotivationDetail.AccountCode;
                }
            }
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

            if (ATxtDetailCostCentreCode.Text != CostCentreCode)
            {
                ATxtDetailCostCentreCode.Text = CostCentreCode;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// RetrieveRecipientCostCentreCode
        /// </summary>
        private static void RetrieveRecipientCostCentreCode(GiftBatchTDSARecurringGiftDetailRow ARow, TextBox ATxtDetailCostCentreCode)
        {
            if (ARow == null)
            {
                return;
            }

            bool PartnerIsMissingLink = false;

            try
            {
                string newCostCentreCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(ARow.LedgerNumber,
                    ARow.RecipientKey,
                    ARow.RecipientLedgerNumber,
                    ARow.DateEntered,
                    ARow.MotivationGroupCode,
                    ARow.MotivationDetailCode,
                    out PartnerIsMissingLink);

                if (ARow.CostCentreCode != newCostCentreCode)
                {
                    ARow.CostCentreCode = newCostCentreCode;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (ATxtDetailCostCentreCode.Text != ARow.CostCentreCode)
            {
                ATxtDetailCostCentreCode.Text = ARow.CostCentreCode;
            }
        }

        /// <summary>
        /// ResetMotivationDetailCodeFilter
        /// </summary>
        private static void ResetMotivationDetailCodeFilter(TCmbAutoPopulated ACmbMotivationDetailCode,
            ref string AMotivationDetail,
            bool AShowRecurringGiftDetail,
            TFrmPetraEditUtils APetraUtilsObject = null)
        {
            if ((ACmbMotivationDetailCode.Count == 0) && (ACmbMotivationDetailCode.Filter != null)
                && (!ACmbMotivationDetailCode.Filter.Contains("1 = 2")))
            {
                AMotivationDetail = string.Empty;
                ACmbMotivationDetailCode.RefreshLabel();

                if (AShowRecurringGiftDetail)
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

            if (AShowRecurringGiftDetail)
            {
                ACmbMotivationDetailCode.Filter = AMotivationDetailTable.GetMotivationStatusDBName() + " = true";
            }
            else
            {
                ACmbMotivationDetailCode.Filter = string.Empty;
            }

            //Update the combo
            try
            {
                if (APetraUtilsObject != null)
                {
                    APetraUtilsObject.SuppressChangeDetection = true;
                }

                ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
            }
            finally
            {
                if (APetraUtilsObject != null)
                {
                    APetraUtilsObject.SuppressChangeDetection = false;
                }
            }

            ACmbMotivationDetailCode.RefreshLabel();
        }

        /// <summary>
        /// ApplyMotivationDetailCodeFilter
        /// </summary>
        private static void ApplyMotivationDetailCodeFilter(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
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
            string AMotivationGroup,
            ref string AMotivationDetail,
            ref bool AMotivationDetailChangedFlag,
            bool AActiveOnly,
            bool ARecipientKeyChangingFlag,
            bool ACreatingNewRecurringGiftFlag,
            bool AInEditModeFlag,
            ref string AAutoPopComment)
        {
            //FMotivationbDetail will change by next process
            string motivationDetail = AMotivationDetail;

            TFinanceControls.ChangeFilterMotivationDetailList(ref ACmbMotivationDetailCode, AMotivationGroup);
            AMotivationDetail = motivationDetail;

            if (AMotivationDetail.Length > 0)
            {
                if ((ACmbMotivationDetailCode.GetSelectedString() != null) && (ACmbMotivationDetailCode.GetSelectedString() != AMotivationDetail))
                {
                    ACmbMotivationDetailCode.SetSelectedString(AMotivationDetail);
                }

                ACmbMotivationDetailCode.RefreshLabel();
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
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    ARecipientKeyChangingFlag,
                    ACreatingNewRecurringGiftFlag,
                    AInEditModeFlag,
                    false,
                    ref AAutoPopComment);
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
                    AMotivationGroup,
                    ref AMotivationDetail,
                    ref AMotivationDetailChangedFlag,
                    ARecipientKeyChangingFlag,
                    ACreatingNewRecurringGiftFlag,
                    AInEditModeFlag,
                    false,
                    ref AAutoPopComment);
            }

            RetrieveMotivationDetailAccountCode(AMainDS, ALedgerNumber, ATxtDetailAccountCode,
                AMotivationGroup, AMotivationDetail);

            if ((ATxtDetailRecipientKey.Text == string.Empty) || (Convert.ToInt64(ATxtDetailRecipientKey.Text) == 0))
            {
                ATxtDetailRecipientKey.Text = String.Format("{0:0000000000}", 0);
                RetrieveMotivationDetailCostCentreCode(AMainDS, ALedgerNumber, ATxtDetailCostCentreCode, AMotivationGroup, AMotivationDetail);
            }
        }

        /// <summary>
        /// PopulateKeyMinistry
        /// </summary>
        private static void PopulateKeyMinistry(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow,
            TCmbAutoPopulated ACmbKeyMinistries,
            TtxtAutoPopulatedButtonLabel ATxtDetailRecipientKey,
            TtxtAutoPopulatedButtonLabel AtxtDetailRecipientLedgerNumber,
            bool AMotivationDetailChangedFlag,
            long APartnerKey = 0)
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

            GetRecipientData(ACurrentDetailRow,
                APartnerKey,
                ref ACmbKeyMinistries,
                ATxtDetailRecipientKey,
                ref AtxtDetailRecipientLedgerNumber,
                AMotivationDetailChangedFlag);
        }

        #endregion
    }
}