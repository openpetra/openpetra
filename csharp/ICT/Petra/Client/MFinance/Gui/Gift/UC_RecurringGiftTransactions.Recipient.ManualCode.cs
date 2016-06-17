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
#region usings

using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles the interactions between controls in the recipient section of
    /// the Recurring Gift transaction on a details panel.
    /// </summary>
    public partial class TUC_RecurringGiftTransactions
    {
        #region Initialisation

        private void SetFocusToKeyMinistryCombo(object sender, EventArgs e)
        {
            cmbKeyMinistries.Focus();
        }

        #endregion

        #region change event handlers

        private void KeyMinistryChanged(object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow == null)
                || FKeyMinistryChangedInProcess
                || FRecipientKeyChangedInProcess
                || FPetraUtilsObject.SuppressChangeDetection
                || txtDetailRecipientKeyMinistry.Visible)
            {
                return;
            }

            string KeyMinistry = cmbKeyMinistries.GetSelectedDescription();
            string RecipientKey = cmbKeyMinistries.GetSelectedInt64().ToString();

            try
            {
                FKeyMinistryChangedInProcess = true;

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
                FKeyMinistryChangedInProcess = false;
            }
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

            OnRecipientKeyChanged(APartnerKey, APartnerShortName, AValidSelection, out DoValidateGiftDestination);

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

        private void OnRecipientKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection,
            out bool ADoValidateRecurringGiftDestination)
        {
            ADoValidateRecurringGiftDestination = false;

            if (FRecipientKeyChangedInProcess
                || FPetraUtilsObject.SuppressChangeDetection
                || FShowDetailsInProcess)
            {
                return;
            }

            Int64 RecipientLedgerNumber = 0;
            FPreviouslySelectedDetailRow.BeginEdit();

            // get the recipient ledger number
            if (APartnerKey > 0)
            {
                RecipientLedgerNumber = TRemote.MFinance.Gift.WebConnectors.GetRecipientFundNumber(APartnerKey,
                    DateTime.Now);
            }

            FRecipientKeyChangedInProcess = true;
            txtDetailRecipientKeyMinistry.Text = string.Empty;

            try
            {
                FPreviouslySelectedDetailRow.RecipientKey = APartnerKey;
                FPreviouslySelectedDetailRow.RecipientDescription = APartnerShortName;
                FPreviouslySelectedDetailRow.RecipientClass = txtDetailRecipientKey.CurrentPartnerClass.ToString();

                FPetraUtilsObject.SuppressChangeDetection = true;

                //Set RecipientLedgerNumber
                FPreviouslySelectedDetailRow.RecipientLedgerNumber = RecipientLedgerNumber;

                if (!FKeyMinistryChangedInProcess)
                {
                    GetRecipientData(APartnerKey, FMotivationDetailHasChangedFlag);
                    ADoValidateRecurringGiftDestination = true;
                }

                FPetraUtilsObject.SuppressChangeDetection = false;

                // do not want to update motivation comboboxes if recipient key is being changed due to a new Recurring Gift or the motivation detail being changed
                if (!FMotivationDetailHasChangedFlag && !FNewGiftInProcess
                    && TRemote.MFinance.Gift.WebConnectors.GetMotivationGroupAndDetailForPartner(APartnerKey, ref FMotivationGroup,
                        ref FMotivationDetail))
                {
                    if (cmbDetailMotivationGroupCode.GetSelectedString() != FMotivationGroup)
                    {
                        // note - this will also update the Motivation Detail
                        cmbDetailMotivationGroupCode.SetSelectedString(FMotivationGroup, -1);
                    }

                    if (cmbMotivationDetailCode.GetSelectedString() != FMotivationDetail)
                    {
                        cmbMotivationDetailCode.SetSelectedString(FMotivationDetail, -1);
                    }

                    FPreviouslySelectedDetailRow.MotivationGroupCode = FMotivationGroup;
                    FPreviouslySelectedDetailRow.MotivationDetailCode = FMotivationDetail;
                }

                FPetraUtilsObject.SuppressChangeDetection = true;

                if (APartnerKey > 0)
                {
                    ApplyRecipientCostCentreCode();
                }
                else
                {
                    UpdateRecipientKeyText(APartnerKey, FPreviouslySelectedDetailRow, FMotivationGroup, FMotivationDetail);

                    DisplayMotivationDetailCostCentreCode();
                }
            }
            finally
            {
                FRecipientKeyChangedInProcess = false;
                ReconcileKeyMinistryFromCombo(FPreviouslySelectedDetailRow);
                FPreviouslySelectedDetailRow.EndEdit();
                FPetraUtilsObject.SuppressChangeDetection = false;
            }
        }

        private void RecipientLedgerNumberChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (FPetraUtilsObject.SuppressChangeDetection
                || FShowDetailsInProcess
                || FRecipientKeyChangedInProcess)
            {
                return;
            }

            string NewCCCode = string.Empty;

            // it is possible that there are no active motivation details and so AMotivationDetail is blank
            if (!string.IsNullOrEmpty(FPreviouslySelectedDetailRow.MotivationDetailCode))
            {
                bool partnerIsMissingLink = false;

                NewCCCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(FLedgerNumber,
                    FPreviouslySelectedDetailRow.RecipientKey,
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                    DateTime.Now,
                    FPreviouslySelectedDetailRow.MotivationGroupCode,
                    FPreviouslySelectedDetailRow.MotivationDetailCode,
                    out partnerIsMissingLink);
            }

            if (txtDetailCostCentreCode.Text != NewCCCode)
            {
                txtDetailCostCentreCode.Text = NewCCCode;
            }
        }

        /// <summary>
        /// modifies menu items depending on the Recipient's Partner class
        /// </summary>
        /// <param name="APartnerClass"></param>
        private void RecipientPartnerClassChanged(TPartnerClass ? APartnerClass)
        {
            bool? DoEnableRecipientGiftDestination;

            OnRecipientPartnerClassChanged(APartnerClass, out DoEnableRecipientGiftDestination);

            if (DoEnableRecipientGiftDestination.HasValue)
            {
                mniRecipientGiftDestination.Enabled = DoEnableRecipientGiftDestination.Value;
            }
        }

        /// <summary>
        /// Modifies menu items depending on the Recipeint's Partner class
        /// </summary>
        private void OnRecipientPartnerClassChanged(TPartnerClass? APartnerClass,
            out bool ? AEnableRecipientRecurringGiftDestination)
        {
            AEnableRecipientRecurringGiftDestination = null;

            string ItemText = Catalog.GetString("Open Recipient Recurring Gift Destination");

            if ((APartnerClass == TPartnerClass.UNIT) || (APartnerClass == null))
            {
                txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, false);
                txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, false);
                AEnableRecipientRecurringGiftDestination = false;
            }
            else if (APartnerClass == TPartnerClass.FAMILY)
            {
                txtDetailRecipientKey.CustomContextMenuItemsVisibility(ItemText, true);
                txtDetailRecipientLedgerNumber.CustomContextMenuItemsVisibility(ItemText, true);
                AEnableRecipientRecurringGiftDestination = true;
            }
        }

        #endregion change event handlers

        #region control handling

        private void ReconcileKeyMinistryFromCombo(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow)
        {
            if (FInEditModeFlag)
            {
                string keyMinistry = string.Empty;
                bool isEmptyDetailRow = (ACurrentDetailRow == null);

                if (!isEmptyDetailRow && (cmbKeyMinistries.SelectedIndex > -1))
                {
                    keyMinistry = cmbKeyMinistries.GetSelectedDescription();
                }

                txtDetailRecipientKeyMinistry.Text = keyMinistry;
            }
        }

        private void ReconcileKeyMinistryFromTextbox(GiftBatchTDSARecurringGiftDetailRow ACurrentDetailRow)
        {
            if (FInEditModeFlag)
            {
                bool isEmptyDetailRow = (ACurrentDetailRow == null);
                string keyMinistry = txtDetailRecipientKeyMinistry.Text;

                if (!isEmptyDetailRow && (keyMinistry.Length > 0))
                {
                    cmbKeyMinistries.SetSelectedString(keyMinistry);
                }
                else
                {
                    cmbKeyMinistries.SelectedIndex = -1;
                }
            }
        }

        private void PopulateKeyMinistry(Int64 APartnerKey, bool AMotivationDetailChangedFlag)
        {
            ClearKeyMinistries();

            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);

                if (APartnerKey == 0)
                {
                    cmbKeyMinistries.Enabled = false;
                    return;
                }
            }

            GetRecipientData(APartnerKey, AMotivationDetailChangedFlag);
        }

        private void ClearKeyMinistries()
        {
            cmbKeyMinistries.SelectedIndex = -1;
            cmbKeyMinistries.Clear();
        }

        #endregion control handling

        #region data handling

        private void UpdateRecipientKeyText(Int64 APartnerKey,
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

        private void GetRecipientData(long APartnerKey, bool AMotivationDetailChangedFlag)
        {
            if (APartnerKey == 0)
            {
                APartnerKey = Convert.ToInt64(txtDetailRecipientKey.Text);
            }

            // If this method has been called as a result of a change in motivation detail then txtDetailRecipientKey has not yet been set...
            // but we do know that the recipient must be a Unit.

            // if Family Recipient
            if (!AMotivationDetailChangedFlag && (txtDetailRecipientKey.CurrentPartnerClass == TPartnerClass.FAMILY))
            {
                txtDetailRecipientLedgerNumber.Text = FPreviouslySelectedDetailRow.RecipientLedgerNumber.ToString();
                ClearKeyMinistries();
                cmbKeyMinistries.Enabled = false;
            }
            // if Unit Recipient
            else
            {
                //At this point, only active KeyMinistries are allowed in a live Recurring Gift
                bool activeOnly = true;
                TFinanceControls.GetRecipientData(ref cmbKeyMinistries, ref txtDetailRecipientLedgerNumber, APartnerKey, activeOnly);

                // enable / disable combo box depending on whether it contains any key ministries
                cmbKeyMinistries.Enabled = ((cmbKeyMinistries.Table != null) && (cmbKeyMinistries.Table.Rows.Count > 0));
            }
        }

        private void UpdateAllRecipientDescriptions(Int32 ABatchNumber)
        {
            DataView RecurringGiftDetailView = new DataView(FMainDS.ARecurringGiftDetail);

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

        private void ApplyRecipientCostCentreCode()
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            bool PartnerIsMissingLink = false;

            try
            {
                string newCostCentreCode = TRemote.MFinance.Gift.WebConnectors.RetrieveCostCentreCodeForRecipient(
                    FPreviouslySelectedDetailRow.LedgerNumber,
                    FPreviouslySelectedDetailRow.RecipientKey,
                    FPreviouslySelectedDetailRow.RecipientLedgerNumber,
                    DateTime.Now,
                    FPreviouslySelectedDetailRow.MotivationGroupCode,
                    FPreviouslySelectedDetailRow.MotivationDetailCode,
                    out PartnerIsMissingLink);

                if (FPreviouslySelectedDetailRow.CostCentreCode != newCostCentreCode)
                {
                    FPreviouslySelectedDetailRow.CostCentreCode = newCostCentreCode;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (txtDetailCostCentreCode.Text != FPreviouslySelectedDetailRow.CostCentreCode)
            {
                txtDetailCostCentreCode.Text = FPreviouslySelectedDetailRow.CostCentreCode;
            }
        }

        #endregion data handling
    }
}
