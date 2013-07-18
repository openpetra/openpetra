//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Subscription
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary> indicate if the "Edit Issues" button is allowed to be generally be enabled </summary>
        private Boolean FAllowEditIssues = false;

        /// <summary> indicate if controls are filled with a new record </summary>
        private Boolean FInitializationRunning;

        /// <summary>Reference to PubclicationCostTable.</summary>
        private PPublicationCostTable FPublicationCostDT;

        /// <summary>DataRow for the p_subscription record we are working with</summary>
        private PSubscriptionRow FSubscriptionDR = null;

        /// <summary>CachedDataset.TmpCacheDS: DataSet; Currently selected PublicationCode. Won't update automatically!</summary>
        private System.Object FSelectedPublicationCode;

        #region Public Methods

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>dictionary of validation controls in case it is needed for parent control</summary>
        public TValidationControlsDict ValidationControlsDict
        {
            get
            {
                return FValidationControlsDict;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Boolean AllowEditIssues
        {
            get
            {
                return FAllowEditIssues;
            }

            set
            {
                FAllowEditIssues = value;
            }
        }

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        /// <summary>
        /// Display data in control based on data from ARow
        /// </summary>
        /// <param name="ARow"></param>
        public void ShowDetails(PSubscriptionRow ARow)
        {
            FInitializationRunning = true;

            // show controls if not visible yet
            MakeScreenInvisible(false);

            // set member
            FSubscriptionDR = ARow;

            ShowData(ARow);

            // for every record initially disable issue group box, but enable button in the same group box
            if (FAllowEditIssues)
            {
                this.btnEditIssues.Visible = true;
                this.btnEditIssues.Enabled = true;
            }

            EnableDisableIssuesGroupBox(false);

            txtPublicationCost.ReadOnly = true;

            FInitializationRunning = false;
        }

        /// <summary>
        /// Read data from controls into ARow parameter
        /// </summary>
        /// <param name="ARow"></param>
        public void GetDetails(PSubscriptionRow ARow)
        {
            ValidateAllData(false);
            //GetDataFromControls(ARow);
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        public void MakeScreenInvisible(Boolean value)
        {
            /* Set the groupboxes of this UserControl visible or invisible. */
            this.grpMisc.Visible = !value;
            this.grpSubscription.Visible = !value;
            this.grpDates.Visible = !value;
            this.grpIssues.Visible = !value;
        }

        /// <summary>todoComment</summary>
        public void SpecialInitUserControl()
        {
            if (FValidationControlsDict.Count == 0)
            {
                BuildValidationControlsDict();
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            FPublicationCostDT = (PPublicationCostTable)TDataCache.TMPartner.GetCacheableSubscriptionsTable(
                TCacheableSubscriptionsTablesEnum.PublicationCostList);
        }

        private void GetDataFromControlsManual(PSubscriptionRow ARow)
        {
        }

        private PSubscriptionRow GetSelectedMasterRow()
        {
            return FSubscriptionDR;
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// called when combo box value for publication code is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PublicationCodeChanged(object sender, EventArgs e)
        {
            // only react to a changed publication code if there is a record to display
            if (FSubscriptionDR != null)
            {
                UpdatePublicationCost();
                CheckPublicationValidity();
            }
        }

        /// <summary>
        /// called for OnLeave event for publication code combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GratisSubscriptionChanged(object sender, EventArgs e)
        {
            UpdatePublicationCost();
        }

        /// <summary>
        /// called when combo box value for publication status is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PublicationStatusChanged(object sender, EventArgs e)
        {
            if ((cmbPSubscriptionPublicationCode.GetSelectedString() == "")
                || (FSubscriptionDR == null)
                || FInitializationRunning)
            {
                /* don't do his check while a new record is being displayed */
                return;
            }

            if (this.cmbPSubscriptionSubscriptionStatus.GetSelectedString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_GIFT)
            {
                /* GIFT */
                /* enable Gift From partner key text box */
                this.txtPSubscriptionGiftFromKey.Enabled = true;

                /* clear any previously supplied reason for cancellation */
                FSubscriptionDR.SetReasonSubsCancelledCodeNull();
                this.cmbPSubscriptionReasonSubsCancelledCode.cmbCombobox.SelectedValue = "";
                this.cmbPSubscriptionReasonSubsCancelledCode.Enabled = false;

                /* clear any previously supplied Date Ended */
                this.dtpPSubscriptionDateCancelled.Enabled = false;
                this.dtpPSubscriptionDateCancelled.Text = "";
                FSubscriptionDR.SetDateCancelledNull();
            }
            else if ((this.cmbPSubscriptionSubscriptionStatus.GetSelectedString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED)
                     || (this.cmbPSubscriptionSubscriptionStatus.GetSelectedString() == MPartnerConstants.SUBSCRIPTIONS_STATUS_EXPIRED))
            {
                /* CANCELLED or EXPIRED */
                /* Set the DateEnded field to todays date: */
                this.dtpPSubscriptionDateCancelled.Enabled = true;
                dtpPSubscriptionDateCancelled.Text = StringHelper.DateToLocalizedString(DateTime.Now, false);
                FSubscriptionDR.DateCancelled = DateTime.Now.Date;

                /* clear any previously supplied partner key */
                FSubscriptionDR.GiftFromKey = 0;
                txtPSubscriptionGiftFromKey.Text = "0";
                this.txtPSubscriptionGiftFromKey.Enabled = false;

                /* allow them to enter a reason ended */
                this.cmbPSubscriptionReasonSubsCancelledCode.Enabled = true;

                this.cmbPSubscriptionReasonSubsCancelledCode.DropDown();
            }
            else
            {
                /* All other Cases */
                /* clear any previously supplied partner key */
                FSubscriptionDR.GiftFromKey = 0;
                txtPSubscriptionGiftFromKey.Text = "0";
                this.txtPSubscriptionGiftFromKey.Enabled = false;

                /* clear any previously supplied Date Ended */
                FSubscriptionDR.SetDateCancelledNull();
                this.dtpPSubscriptionDateCancelled.Text = "";
                this.dtpPSubscriptionDateCancelled.Enabled = false;

                /* clear any previously supplied reason for cancellation */
                FSubscriptionDR.ReasonSubsCancelledCode = "";
                this.cmbPSubscriptionReasonSubsCancelledCode.cmbCombobox.SelectedValue = "";
                this.cmbPSubscriptionReasonSubsCancelledCode.Enabled = false;
            }
        }

        private void EditIssues(System.Object sender, EventArgs e)
        {
            /* if anwered OK to question below, the Issuesgroupbox screenparts are enabled. */
            if (MessageBox.Show(Catalog.GetString(
                        "Issues data is usually automatically maintained by Petra. Are you sure you want to manually change it?"),
                    Catalog.GetString("Edit Issues"),
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
            {
                EnableDisableIssuesGroupBox(true);
                this.btnEditIssues.Visible = false;
                this.txtPSubscriptionNumberIssuesReceived.Focus();
            }
        }

        private void EnableDisableIssuesGroupBox(Boolean AEnable)
        {
            this.txtPSubscriptionNumberIssuesReceived.Enabled = AEnable;
            this.dtpPSubscriptionFirstIssue.Enabled = AEnable;
            this.dtpPSubscriptionLastIssue.Enabled = AEnable;
        }

        /// <summary>
        /// Sets the Publication cost
        /// </summary>
        /// <returns>void</returns>
        private void SetupPublicationCost(double APublicationCost, String ACurrencyCode)
        {
            this.txtPublicationCost.NumberValueDouble = APublicationCost;
            this.txtPublicationCost.CurrencyCode = ACurrencyCode;
        }

        private void UpdatePublicationCost()
        {
            DataRow[] PublicationCostRows = null;

            /* if the Subscription is free, the cost is always 0! */
            if (this.chkPSubscriptionGratisSubscription.Checked)
            {
                SetupPublicationCost(0, "");
            }
            else
            {
                /* If any Publications */
                if (FSubscriptionDR != null)
                {
                    PublicationCostRows = FPublicationCostDT.Select(
                        PPublicationTable.GetPublicationCodeDBName() + " = '" + this.cmbPSubscriptionPublicationCode.GetSelectedString() + "'");

                    /* if the Subscription has a cost, set it, else set the cost to 0. */
                    if (PublicationCostRows.Length > 0)
                    {
                        SetupPublicationCost((double)((PPublicationCostRow)PublicationCostRows[0]).PublicationCost,
                            ((PPublicationCostRow)PublicationCostRows[0]).CurrencyCode);
                    }
                    else
                    {
                        SetupPublicationCost(0, "");
                    }
                }
                else
                {
                    this.btnEditIssues.Enabled = false;
                }
            }
        }

        private void CheckPublicationValidity()
        {
            DataTable DataCachePublicationListTable;
            PPublicationRow TmpRow;

            if (FInitializationRunning)
            {
                /* don't do his check while a new record is being displayed */
                return;
            }

            try
            {
                /* check if the publication selected is valid, if not, gives warning. */
                DataCachePublicationListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
                TmpRow = (PPublicationRow)DataCachePublicationListTable.Rows.Find(
                    new Object[] { this.cmbPSubscriptionPublicationCode.GetSelectedString() });

                if (TmpRow.ValidPublication)
                {
                    FSelectedPublicationCode = cmbPSubscriptionPublicationCode.cmbCombobox.SelectedValue;
                }
                else
                {
                    if (MessageBox.Show("Please note that Publication '" + this.cmbPSubscriptionPublicationCode.GetSelectedString() +
                            "'\r\nis no longer available." + "\r\n" + "" + "Do you still want to add a subscription for it?",
                            "Create Subscription",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        /* If user selects not to use the publication, the recent publication code is selected. */
                        if (FSelectedPublicationCode != null)
                        {
                            this.cmbPSubscriptionPublicationCode.cmbCombobox.SelectedValue = FSelectedPublicationCode;
                        }
                        else
                        {
                            this.cmbPSubscriptionPublicationCode.cmbCombobox.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        FSelectedPublicationCode = cmbPSubscriptionPublicationCode.cmbCombobox.SelectedValue;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ValidateDataManual(PSubscriptionRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateSubscriptionManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        #endregion
    }
}