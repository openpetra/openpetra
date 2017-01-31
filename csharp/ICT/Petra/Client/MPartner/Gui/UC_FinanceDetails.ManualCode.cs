//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       joachimm, timop, peters
//
// Copyright 2004-2013 by OM International
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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_FinanceDetails
    {
        #region Properties

        /// <summary>Contains a list of all Partners who share the selected bank account</summary>
        private PPartnerTable FAccountSharedWith = new PPartnerTable();

        /// <summary>Dataset containg all PBank records for all banks and their locations</summary>
        private BankTDS FBankDataset;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private bool FTaxDeductiblePercentageEnabled = false;
        private bool FGovIdEnabled = false;

        private bool FFirstTime = true;

        private bool FValidateBankingDetailsExtra = false;

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

        /// <summary>
        /// run extra validation checks
        /// </summary>
        public bool ValidateBankingDetailsExtra
        {
            set
            {
                FValidateBankingDetailsExtra = value;
            }
        }

        /// <summary>an event that will reload the grid after saving</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        #region Setup

        /// <summary>
        /// load the data for this control
        /// </summary>
        public void PreInitUserControl(PartnerEditTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            // if partner is of class FAMILY or class UNIT, enable grpRecipientGiftReceipting
            grpRecipientGiftReceipting.Enabled = (FMainDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY
                                                  || FMainDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT);
        }

        private void InitializeManualCode()
        {
            // remove labels from control
            cmbBankCode.RemoveDescriptionLabel();

            // add image to 'Share' button
            btnShare.Image = btnNew.Image;

            // add event to txtBankKey that is fired when the dataset is changed in the FindBank dialog
            txtBankKey.DatasetChanged += new TDelegateDatasetChanged(this.DatasetChanged);

            // add event which will populate the bank combo boxes when 'Finance details' tab is shown for the first time
            pnlDetails.VisibleChanged += new EventHandler(pnlDetails_VisibleChanged);

            // set up Tax Deductibility (specifically for OM Switzerland)
            FTaxDeductiblePercentageEnabled = FPartnerEditUIConnector.IsTaxDeductiblePercentageEnabled();

            // set up access to Government Id (e.g. bPK) when needed
            FGovIdEnabled = FPartnerEditUIConnector.IsGovIdEnabled();

            grpRecipientGiftReceipting.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

            grpOther.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

            if (FGovIdEnabled)
            {
                lblGovId.Text = FPartnerEditUIConnector.GetGovIdLabel() + ":";
                pnlMiscSettings.Height += 36;
                pnlLeftMiscSettings.Height += 36;
                pnlRightMiscSettings.Height += 26;

                if (FTaxDeductiblePercentageEnabled)
                {
                    grpOther.Top -= 13;
                    grpRecipientGiftReceipting.Top -= 13;
                    grpOther.Height += 35;
                    pnlRightMiscSettings.Height += 5;
                    grpOther.Width += pnlRightMiscSettings.Width - grpOther.Width - 5;
                    grpRecipientGiftReceipting.Width += pnlRightMiscSettings.Width - grpRecipientGiftReceipting.Width - 5;
                }
            }
            else
            {
                pnlLeftMiscSettings.Controls.Remove(this.grpGovId);

                if (FTaxDeductiblePercentageEnabled)
                {
                    grpOther.Width += pnlRightMiscSettings.Width - grpOther.Width - 5;
                    grpRecipientGiftReceipting.Width += pnlRightMiscSettings.Width - grpRecipientGiftReceipting.Width - 5;
                }
            }

            if (FTaxDeductiblePercentageEnabled)
            {
                chkLimitTaxDeductibility.Visible = true;
                pnlTaxDeductible.Visible = true;
                pnlMiscSettings.Height += 27;
                grpRecipientGiftReceipting.Height += 27;
                pnlLeftMiscSettings.Height += 27;
                pnlRightMiscSettings.Height += 27;
                grpOther.Location = new System.Drawing.Point(grpOther.Location.X, grpOther.Location.Y + 27);

                dtpTaxDeductibleValidFrom.AllowEmpty = false;

                // default is 100%
                txtTaxDeductiblePercentage.NumberValueDecimal = 100;

                // user can only change the tax deductible percentage if they have access to the finance module
                if (!UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1))
                {
                    chkLimitTaxDeductibility.CheckedChanged -= new System.EventHandler(this.ChkLimitTaxDeductibility_Change);
                    chkLimitTaxDeductibility.Enabled = false;
                    txtTaxDeductiblePercentage.Enabled = false;
                    dtpTaxDeductibleValidFrom.Enabled = false;

                    ToolTip toolTip = new ToolTip();
                    toolTip.ShowAlways = true;
                    string Caption = Catalog.GetString("Only users with access to the Finance module can edit this control.");
                    toolTip.SetToolTip(lblLimitTaxDeductibility, Caption);
                    toolTip.SetToolTip(lblTaxDeductiblePercentage, Caption);
                    toolTip.SetToolTip(lblTaxDeductibleValidFrom, Caption);
                }
            }
            else
            {
                grpOther.Dock = DockStyle.Fill;

                //Remove RecipientGiftReceipting
                grpRecipientGiftReceipting.Controls.Remove(this.lblLimitTaxDeductibility);
                grpRecipientGiftReceipting.Visible = false;
                pnlRightMiscSettings.Controls.Remove(grpRecipientGiftReceipting);

                //Reset and rescale the Finance Comment
                //grpOther.Top = grpRecipientGiftReceipting.Top + 10;
                pnlRightMiscSettings.Height = pnlLeftMiscSettings.Height - 19;
                pnlRightMiscSettings.Top += 3;

                // make sure we have enough space
                if (FGovIdEnabled)
                {
                    pnlMiscSettings.Height += 27;
                    pnlLeftMiscSettings.Height += 27;
                    pnlRightMiscSettings.Height += 32;
                    grpOther.Height += 27;
                }
            }
        }

        /// <summary>
        /// Loads PBankingDetails Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            // Make sure that Typed DataTables are already there at Client side
            if (FMainDS.PBankingDetails == null)
            {
                FMainDS.Tables.Add(new PartnerEditTDSPBankingDetailsTable());
                FMainDS.Tables.Add(new PPartnerBankingDetailsTable());
                FMainDS.InitVars();
            }

            if (TClientSettings.DelayedDataLoading
                && ((FMainDS.PBankingDetails == null) || (FMainDS.PBankingDetails.Rows.Count == 0)))
            {
                FMainDS.Merge(FPartnerEditUIConnector.GetBankingDetails());

                // Make DataRows unchanged
                if (FMainDS.PBankingDetails.Rows.Count > 0)
                {
                    if (FMainDS.PBankingDetails.Rows[0].RowState != DataRowState.Added)
                    {
                        FMainDS.PBankingDetails.AcceptChanges();
                    }
                }
            }

            return FMainDS.PBankingDetails.Rows.Count != 0;
        }

        bool FComboBoxesCreated = false;

        private void PopulateComboBoxes()
        {
            // For some reason this method enables the save button

            // temporarily remove the event that enables the save button when data is changed
            FPetraUtilsObject.ActionEnablingEvent -= ((TFrmPartnerEdit)FPetraUtilsObject.GetForm()).ActionEnabledEvent;
            bool HasChanges = FPetraUtilsObject.HasChanges;

            // temporily remove events from comboboxes
            cmbBankName.SelectedValueChanged -= new System.EventHandler(this.BankNameChanged);
            cmbBankCode.SelectedValueChanged -= new System.EventHandler(this.BankCodeChanged);

            // load bank records
            if (FBankDataset == null)
            {
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords();
                txtBankKey.DataSet = FBankDataset;
            }

            // create new datatable without any partnerkey duplicates (same bank with different locations)
            PBankTable ComboboxTable = new PBankTable();
            bool CreateInactiveCode = false;

            foreach (BankTDSPBankRow Row in FBankDataset.PBank.Rows)
            {
                if (!ComboboxTable.Rows.Contains(Row.PartnerKey))
                {
                    PBankRow AddRow = (PBankRow)ComboboxTable.NewRow();
                    AddRow.PartnerKey = Row.PartnerKey;
                    AddRow.BranchName = Row.BranchName;
                    AddRow.BranchCode = Row.BranchCode;
                    ComboboxTable.Rows.Add(AddRow);

                    if (Row.BranchCode == SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ")
                    {
                        CreateInactiveCode = true;
                    }
                }
            }

            // add empty row
            DataRow emptyRow = ComboboxTable.NewRow();
            emptyRow[PBankTable.ColumnPartnerKeyId] = -1;
            emptyRow[PBankTable.ColumnBranchNameId] = Catalog.GetString("");
            emptyRow[PBankTable.ColumnBranchCodeId] = Catalog.GetString("");
            ComboboxTable.Rows.Add(emptyRow);

            if (CreateInactiveCode)
            {
                // add inactive row
                emptyRow = ComboboxTable.NewRow();
                emptyRow[PBankTable.ColumnPartnerKeyId] = -2;
                emptyRow[PBankTable.ColumnBranchNameId] = Catalog.GetString("");
                emptyRow[PBankTable.ColumnBranchCodeId] = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ";
                ComboboxTable.Rows.Add(emptyRow);
            }

            // populate the bank name combo box
            cmbBankName.InitialiseUserControl(ComboboxTable,
                PBankTable.GetPartnerKeyDBName(),
                PBankTable.GetBranchNameDBName(),
                PBankTable.GetBranchCodeDBName(),
                null);

            cmbBankName.AppearanceSetup(new int[] { 230, 160 }, -1);
            cmbBankName.Filter = PBankTable.GetBranchNameDBName() + " <> '' OR " +
                                 PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = ''";

            cmbBankName.SelectedValueChanged += new System.EventHandler(this.BankNameChanged);

            // populate the bank code combo box
            cmbBankCode.InitialiseUserControl(ComboboxTable,
                PBankTable.GetBranchCodeDBName(),
                PBankTable.GetPartnerKeyDBName(),
                null);

            cmbBankCode.AppearanceSetup(new int[] { 210 }, -1);
            // filter rows that are blank or <INACTIVE>
            cmbBankCode.Filter = "(" + PBankTable.GetBranchCodeDBName() + " <> '' AND " + PBankTable.GetBranchCodeDBName() + " <> '" +
                                 SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ') " +
                                 "OR (" + PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = '') " +
                                 "OR (" + PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = '" +
                                 SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ')";
            cmbBankCode.SelectedValueChanged += new System.EventHandler(this.BankCodeChanged);

            FComboBoxesCreated = true;

            if ((FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.BankKey != 0)
                && (((FCurrentBankRow == null) || (FPreviouslySelectedDetailRow.BankKey != FCurrentBankRow.PartnerKey))))
            {
                PartnerKeyChanged(FPreviouslySelectedDetailRow.BankKey, "", true);
            }

            FPetraUtilsObject.ActionEnablingEvent += ((TFrmPartnerEdit)FPetraUtilsObject.GetForm()).ActionEnabledEvent;
            FPetraUtilsObject.HasChanges = HasChanges;
        }

        #endregion

        #region Helper methods

        private void ShowDataManual()
        {
            if (FTaxDeductiblePercentageEnabled)
            {
                if ((FMainDS.PPartnerTaxDeductiblePct != null) && (FMainDS.PPartnerTaxDeductiblePct.Rows.Count > 0))
                {
                    chkLimitTaxDeductibility.Checked = true;
                    txtTaxDeductiblePercentage.NumberValueDecimal =
                        ((PPartnerTaxDeductiblePctRow)FMainDS.PPartnerTaxDeductiblePct.Rows[0]).PercentageTaxDeductible;
                    dtpTaxDeductibleValidFrom.Text = ((PPartnerTaxDeductiblePctRow)FMainDS.PPartnerTaxDeductiblePct.Rows[0]).DateValidFrom.ToString();
                }
            }

            if (FGovIdEnabled)
            {
                String GovIdKeyName = TSystemDefaults.GetStringDefault(
                    SharedConstants.SYSDEFAULT_GOVID_DB_KEY_NAME, "");

                txtGovId.Text = "";

                if (FMainDS.PTax != null)
                {
                    DataView TaxView = new DataView(FMainDS.PTax);

                    TaxView.RowFilter = String.Format("{0}='{1}'",
                        PTaxTable.GetTaxTypeDBName(),
                        GovIdKeyName);

                    if (TaxView.Count > 0)
                    {
                        // take first row with tax type filtered
                        txtGovId.Text = ((PTaxRow)FMainDS.PTax.Rows[0]).TaxRef;
                    }
                }
            }

            if (grdDetails.Rows.Count > 1)
            {
                btnSetMainAccount.Enabled = true;
                pnlDetails.Visible = true;
            }

            // modify events on first run only
            if (FFirstTime)
            {
                grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(grdDetails_FocusRowLeaving);
                grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(grdDetails_FocusRowLeavingManual);

                FFirstTime = false;
            }

            grdDetails.AutoResizeGrid();
        }

        private void ShowDetailsManual(PBankingDetailsRow ARow)
        {
            if (ARow != null)
            {
                btnDelete.Enabled = true;
                pnlDetails.Visible = true;

                // set chkSavingsAccount
                if (ARow.BankingType == MPartnerConstants.BANKINGTYPE_SAVINGSACCOUNT)
                {
                    chkSavingsAccount.Checked = true;
                }
                else
                {
                    chkSavingsAccount.Checked = false;
                }

                // BankKey will be 0 for a new bank account
                if (ARow.BankKey == 0)
                {
                    cmbBankName.SetSelectedString("");
                    cmbBankCode.SetSelectedString("");
                }
                else if (FComboBoxesCreated && ((FCurrentBankRow == null) || (ARow.BankKey != FCurrentBankRow.PartnerKey)))
                {
                    PartnerKeyChanged(ARow.BankKey, "", true);
                }
            }

            if (FPreviouslySelectedDetailRow != null)
            {
                // Find any Partners that share this bank account
                FAccountSharedWith = TRemote.MPartner.Partner.WebConnectors.SharedBankAccountPartners(FPreviouslySelectedDetailRow.BankingDetailsKey,
                    FMainDS.PPartner[0].PartnerKey);
            }

            InitAccountSharedWithGrid();

            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        // Initialise the grid to display partners who share the selected account
        private void InitAccountSharedWithGrid()
        {
            grdAccountSharedWith.Columns.Clear();

            if ((FAccountSharedWith == null) || (FAccountSharedWith.Rows.Count == 0))
            {
                grdAccountSharedWith.Enabled = false;
                lblAccountSharedWith.Enabled = false;
            }
            else
            {
                grdAccountSharedWith.Enabled = true;
                lblAccountSharedWith.Enabled = true;

                grdAccountSharedWith.AddTextColumn(Catalog.GetString("Partner Name"), FAccountSharedWith.ColumnPartnerShortName, 179);

                DataView MyDataView = FAccountSharedWith.DefaultView;
                MyDataView.Sort = "p_partner_key_n ASC";
                MyDataView.AllowNew = false;
                grdAccountSharedWith.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        /// Performs necessary actions after the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet.
        /// New rows with negative id numbers in the primary key have been removed, and replaced with the saved rows.
        /// </summary>
        public void RefreshRecordsAfterMerge()
        {
            int CurrentSelectedRowIndex = GetSelectedRowIndex();

            FPreviouslySelectedDetailRow = null;
            grdDetails.Selection.ResetSelection(false);
            ShowData();

            // reselect the previously selected row
            SelectRowInGrid(CurrentSelectedRowIndex);
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// GetDataFromControls for PPartner table.
        /// </summary>
        /// <remarks>This allows PPartner data to be saved even if the partner has no bank accounts.</remarks>
        /// <returns>True if successful.</returns>
        public bool GetPartnerDataFromControls()
        {
            try
            {
                // GetDataFromControls for PPartner table
                FMainDS.PPartner[0].ReceiptLetterFrequency = cmbReceiptLetterFrequency.GetSelectedString();
                FMainDS.PPartner[0].ReceiptEachGift = chkReceiptEachGift.Checked;
                FMainDS.PPartner[0].AnonymousDonor = chkAnonymousDonor.Checked;
                //Email gift statement removed from UI as probably not needed anylonger
                //FMainDS.PPartner[0].EmailGiftStatement = chkEmailGiftStatement.Checked;
                FMainDS.PPartner[0].FinanceComment = txtFinanceComment.Text;

                if (FTaxDeductiblePercentageEnabled)
                {
                    if (chkLimitTaxDeductibility.Checked)
                    {
                        if (!dtpTaxDeductibleValidFrom.ValidDate(false))
                        {
                            ValidateValidFromDate();
                            return false;
                        }

                        if (FMainDS.PPartnerTaxDeductiblePct == null)
                        {
                            FMainDS.Tables.Add(new PPartnerTaxDeductiblePctTable());
                            FMainDS.InitVars();
                        }

                        bool CreateNewRow = true;

                        // check if previous row can be edited otherwise create a new row
                        foreach (PPartnerTaxDeductiblePctRow Row in FMainDS.PPartnerTaxDeductiblePct.Rows)
                        {
                            if (Row.RowState != DataRowState.Deleted)
                            {
                                if (Row.DateValidFrom == dtpTaxDeductibleValidFrom.Date.Value)
                                {
                                    Row.DateValidFrom = dtpTaxDeductibleValidFrom.Date.Value;
                                    Row.PercentageTaxDeductible = (decimal)txtTaxDeductiblePercentage.NumberValueDecimal;
                                    CreateNewRow = false;
                                }
                                else
                                {
                                    Row.Delete();
                                }
                            }
                        }

                        if (CreateNewRow)
                        {
                            PPartnerTaxDeductiblePctRow NewRow = FMainDS.PPartnerTaxDeductiblePct.NewRowTyped(true);
                            NewRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                            NewRow.DateValidFrom = dtpTaxDeductibleValidFrom.Date.Value;
                            NewRow.PercentageTaxDeductible = (decimal)txtTaxDeductiblePercentage.NumberValueDecimal;
                            FMainDS.PPartnerTaxDeductiblePct.Rows.Add(NewRow);
                        }
                    }
                    else
                    {
                        if ((FMainDS.PPartnerTaxDeductiblePct != null) && (FMainDS.PPartnerTaxDeductiblePct.Count > 0))
                        {
                            // every row in table should be deleted
                            foreach (PPartnerTaxDeductiblePctRow Row in FMainDS.PPartnerTaxDeductiblePct.Rows)
                            {
                                if (Row.RowState != DataRowState.Deleted)
                                {
                                    Row.Delete();
                                }
                            }
                        }
                    }
                }

                if (FGovIdEnabled)
                {
                    String GovIdKeyName = TSystemDefaults.GetStringDefault(
                        SharedConstants.SYSDEFAULT_GOVID_DB_KEY_NAME, "");
                    PTaxRow taxRow;

                    if (FMainDS.PTax == null)
                    {
                        FMainDS.Tables.Add(new PTaxTable());
                        FMainDS.InitVars();
                    }

                    DataView TaxView = new DataView(FMainDS.PTax);

                    TaxView.RowFilter = String.Format("{0}='{1}'",
                        PTaxTable.GetTaxTypeDBName(),
                        GovIdKeyName);

                    if (TaxView.Count == 0)
                    {
                        taxRow = FMainDS.PTax.NewRowTyped();
                        taxRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                        taxRow.TaxType = GovIdKeyName;
                        taxRow.TaxRef = txtGovId.Text;
                        FMainDS.PTax.Rows.Add(taxRow);
                    }
                    else
                    {
                        // take first row with tax type filtered
                        taxRow = (PTaxRow)FMainDS.PTax.Rows[0];
                        taxRow.TaxRef = txtGovId.Text;
                    }
                }
            }
            catch (ConstraintException)
            {
                return false;
            }

            return true;
        }

        private BankTDSPBankRow GetCurrentRow(long APartnerKey)
        {
            BankTDSPBankRow ReturnValue = null;

            // Multiple rows could have the same partner keys but different locaitons.
            // We just want the first row.
            foreach (BankTDSPBankRow Row in FBankDataset.PBank.Rows)
            {
                if (Row.PartnerKey == APartnerKey)
                {
                    ReturnValue = Row;
                    break;
                }
            }

            return ReturnValue;
        }

        #endregion

        #region ActionHandling

        /// <summary>
        /// The currently selected account's PBank row
        /// </summary>
        private BankTDSPBankRow FCurrentBankRow;

        // called when FindBank dialog is accepted
        private void PartnerKeyChanged(long APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (!FComboBoxesCreated)
            {
                return;
            }

            FCurrentBankRow = GetCurrentRow(APartnerKey);

            // change the BankName combo (if it was not the control used to change the bank)
            if ((FCurrentBankRow != null) && (cmbBankName.GetSelectedString() != FCurrentBankRow.PartnerKey.ToString()))
            {
                // temporarily remove event
                cmbBankName.SelectedValueChanged -= BankNameChanged;

                cmbBankName.SetSelectedString(FCurrentBankRow.BranchName);

                // If other banks have the same name then we must iterate through all banks to select the one we want
                while (cmbBankName.GetSelectedString() != FCurrentBankRow.BranchName
                       && cmbBankName.GetSelectedDescription() != FCurrentBankRow.BranchCode)
                {
                    cmbBankName.cmbCombobox.SelectedIndex += 1;
                }

                cmbBankName.SelectedValueChanged += new System.EventHandler(this.BankNameChanged);
            }
            else if ((FCurrentBankRow == null) && (cmbBankName.GetSelectedString() != ""))
            {
                // temporarily remove event
                cmbBankName.SelectedValueChanged -= BankNameChanged;

                cmbBankName.SetSelectedString("");

                cmbBankName.SelectedValueChanged += new System.EventHandler(this.BankNameChanged);
            }

            // change the BankCode combo (if it was not the control used to change the bank)
            if ((FCurrentBankRow != null) && (cmbBankCode.GetSelectedString() != FCurrentBankRow.BranchCode))
            {
                cmbBankCode.SetSelectedString(FCurrentBankRow.BranchCode);
            }
            else if ((FCurrentBankRow == null) && (cmbBankCode.GetSelectedString() != ""))
            {
                // temporarily remove event
                cmbBankCode.SelectedValueChanged -= BankCodeChanged;

                cmbBankCode.SetSelectedString("");

                cmbBankCode.SelectedValueChanged += new System.EventHandler(this.BankCodeChanged);
            }

            // change the bank info
            if ((FCurrentBankRow != null) && (APartnerKey != 0) && (APartnerKey != -1))
            {
                lblBicSwiftCode.Text = Catalog.GetString("BIC/SWIFT Code: ") + FCurrentBankRow.Bic;
                lblCountry.Text = Catalog.GetString("Country: ");

                if (!string.IsNullOrEmpty(FCurrentBankRow.CountryCode))
                {
                    lblCountry.Text += FCurrentBankRow.CountryCode;
                }
                else
                {
                    lblCountry.Text += Catalog.GetString("No Valid Address On File");
                }
            }
            else
            {
                lblBicSwiftCode.Text = "BIC/SWIFT Code: ";
                lblCountry.Text = "Country: ";
            }
        }

        // called when text box is left. Updates comboboxes faster than ValueChanged event
        private void PartnerKeyChanged(System.Object sender, EventArgs e)
        {
            // this if stops a crash when screen is closed causing this event to be fired
            if (txtBankKey.Text != "")
            {
                PartnerKeyChanged(Convert.ToInt64(txtBankKey.Text), "", true);
            }
        }

        // when cmbBankName is changed
        private void BankNameChanged(System.Object sender, EventArgs e)
        {
            // if a blank name has just been selected
            if (string.IsNullOrEmpty(cmbBankName.GetSelectedString())
                && ((FCurrentBankRow == null) || string.IsNullOrEmpty(FCurrentBankRow.BranchName)))
            {
                txtBankKey.Text = "0";
            }
            // cmbBankName.ContainsFocus is needed because the combobox automatically changes the selection
            // to the first row with that name when the focus is left. This was a problem with multiple banks with the same name.
            else if (((FCurrentBankRow == null) || (FCurrentBankRow.PartnerKey.ToString() != cmbBankName.GetSelectedString()))
                     && (cmbBankName.GetSelectedString() != "")
                     && cmbBankName.ContainsFocus)
            {
                FCurrentBankRow = GetCurrentRow(Convert.ToInt64(cmbBankName.GetSelectedString()));

                // update partner key in txtBankKey
                if (FCurrentBankRow != null)
                {
                    txtBankKey.Text = FCurrentBankRow.PartnerKey.ToString();
                    PartnerKeyChanged(FCurrentBankRow.PartnerKey, "", true);
                }
                else
                {
                    txtBankKey.Text = "0";
                    PartnerKeyChanged(0, "", true);
                }
            }
        }

        // when cmbBankCode is changed
        private void BankCodeChanged(System.Object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbBankCode.GetSelectedString()) && (FCurrentBankRow == null))
            {
                return;
            }
            else if ((string.IsNullOrEmpty(cmbBankCode.GetSelectedString()) && !string.IsNullOrEmpty(FCurrentBankRow.BranchCode))
                     || ((cmbBankCode.GetSelectedString() == SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ")
                         && ((FCurrentBankRow == null) || (FCurrentBankRow.BranchCode != SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " "))))
            {
                // if "<INACTIVE>" has been selected change it to blank
                cmbBankCode.SelectedIndex = -1;
                txtBankKey.Text = "0";
            }
            else if ((FCurrentBankRow == null) || (FCurrentBankRow.BranchCode != cmbBankCode.GetSelectedString()))
            {
                FCurrentBankRow = GetCurrentRow(Convert.ToInt64(cmbBankCode.GetSelectedDescription()));

                // update partner key in txtBankKey
                txtBankKey.Text = FCurrentBankRow.PartnerKey.ToString();
                PartnerKeyChanged(FCurrentBankRow.PartnerKey, "", true);
            }
        }

        private void SavingsAccount_Click(System.Object sender, EventArgs e)
        {
            if (chkSavingsAccount.Checked)
            {
                FPreviouslySelectedDetailRow.BankingType = MPartnerConstants.BANKINGTYPE_SAVINGSACCOUNT;
            }
            else
            {
                FPreviouslySelectedDetailRow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            }
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (FBankDataset == null)
            {
                Cursor.Current = Cursors.WaitCursor;

                // load bank records
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords();
                txtBankKey.DataSet = FBankDataset;

                // populate the comboboxes for Bank Name and Bank Code
                PopulateComboBoxes();

                Cursor.Current = Cursors.Default;
            }

            FValidateBankingDetailsExtra = true;
            this.CreateNewPBankingDetails();
        }

        private void NewRowManual(ref PartnerEditTDSPBankingDetailsRow ARow)
        {
            ARow.BankingDetailsKey = (FMainDS.PBankingDetails.Rows.Count + 1) * -1;
            ARow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            ARow.BankKey = 0;

            // automatically set to main account if it is the only account
            ARow.MainAccount = (grdDetails.Rows.Count == 1);

            PPartnerBankingDetailsRow partnerBankingDetails = FMainDS.PPartnerBankingDetails.NewRowTyped();
            partnerBankingDetails.BankingDetailsKey = ARow.BankingDetailsKey;
            partnerBankingDetails.PartnerKey = FMainDS.PPartner[0].PartnerKey;
            FMainDS.PPartnerBankingDetails.Rows.Add(partnerBankingDetails);

            btnSetMainAccount.Enabled = true;
        }

        /// <summary>
        /// Share an existing bank account of another partner
        /// </summary>
        private void ShareExistingBankAccount(System.Object sender, EventArgs e)
        {
            FValidateBankingDetailsExtra = true;

            // first validate the currently selected row (if it exists)
            if (!ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                return;
            }

            PPartnerBankingDetailsRow NewRow;

            long PartnerKey = 0;
            string PartnerShortName;
            TPartnerClass? PartnerClass;
            int BankingDetailsKey;

            DataRow[] ExistingPartnerDataRows;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenPartnerFindByBankDetailsScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenPartnerFindByBankDetailsScreen.Invoke
                        ("",
                        out PartnerKey,
                        out PartnerShortName,
                        out PartnerClass,
                        out BankingDetailsKey,
                        this.ParentForm);

                    if ((PartnerKey != -1) && (BankingDetailsKey != -1))
                    {
                        ExistingPartnerDataRows = FMainDS.PPartnerBankingDetails.Select(
                            PPartnerBankingDetailsTable.GetPartnerKeyDBName() + " = " + FMainDS.PPartner[0].PartnerKey.ToString() +
                            " AND " + PPartnerBankingDetailsTable.GetBankingDetailsKeyDBName() + " = " + BankingDetailsKey.ToString());

                        if (ExistingPartnerDataRows.Length > 0)
                        {
                            // check if partner already exists in extract
                            MessageBox.Show(Catalog.GetString("The selected bank account already exists for this partner"),
                                Catalog.GetString("Add Bank Account to partner"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return;
                        }

                        // add bank account
                        NewRow = FMainDS.PPartnerBankingDetails.NewRowTyped();
                        NewRow.PartnerKey = FMainDS.PPartner[0].PartnerKey;
                        NewRow.BankingDetailsKey = BankingDetailsKey;
                        FMainDS.PPartnerBankingDetails.Rows.Add(NewRow);

                        // get the PBankingDetailsRow that corresponds to the PPartnerBankingDetailsRow NewRow
                        PBankingDetailsTable SharedBankingDetailsTable = TRemote.MPartner.Partner.WebConnectors.GetBankingDetailsRow(
                            BankingDetailsKey);

                        if (SharedBankingDetailsTable == null)
                        {
                            throw new Exception();
                        }

                        FMainDS.PBankingDetails.Merge(SharedBankingDetailsTable);

                        PartnerEditTDSPBankingDetailsRow SharedRow = (PartnerEditTDSPBankingDetailsRow)FMainDS.PBankingDetails.Rows.Find(
                            BankingDetailsKey);

                        // automatically set to main account if it is the only account
                        SharedRow.MainAccount = (grdDetails.Rows.Count == 2);

                        btnSetMainAccount.Enabled = true;

                        // enable save button on screen
                        FPetraUtilsObject.SetChangedFlag();

                        // select the added bank account in the grid so the user can see the change
                        SelectDetailRowByDataTableIndex(FMainDS.PBankingDetails.Rows.Count - 1);

                        UpdateRecordNumberDisplay();
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling PartnerFindScreen Delegate!",
                        exp);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void OpenSharingPartner(System.Object sender, EventArgs e)
        {
            if ((FAccountSharedWith != null) && (FAccountSharedWith.Rows.Count > 0)
                && (grdAccountSharedWith.SelectedDataRows != null) && (grdAccountSharedWith.SelectedDataRows.Length > 0))
            {
                long SharingPartnerKey =
                    Convert.ToInt64(((DataRowView)grdAccountSharedWith.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerKeyDBName()]);

                // Open the selected partner's Partner Edit screen at Personnel Applications
                TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

                frm.SetParameters(TScreenMode.smEdit, SharingPartnerKey, TPartnerEditTabPageEnum.petpFinanceDetails);
                frm.Show();
            }
        }

        // Called when the dataset is changed in the FindBank dialog
        private void DatasetChanged(DataSet ADataset)
        {
            FBankDataset = (BankTDS)ADataset;
            PopulateComboBoxes();
        }

        // set the main account flag, remove that flag from the other accounts (p_banking_details_usage)
        private void SetMainAccount(System.Object Sender, EventArgs e)
        {
            foreach (PartnerEditTDSPBankingDetailsRow r in FMainDS.PBankingDetails.Rows)
            {
                if ((r.RowState != DataRowState.Deleted) && (r != FPreviouslySelectedDetailRow) && r.MainAccount)
                {
                    r.MainAccount = false;
                    FPetraUtilsObject.SetChangedFlag();
                }
            }

            if (!FPreviouslySelectedDetailRow.MainAccount)
            {
                FPreviouslySelectedDetailRow.MainAccount = true;
                FPetraUtilsObject.SetChangedFlag();
            }

            // MainAccount PBankingDetailsUsage is processed on the server side!!!
        }

        // copy the partner's name to the account name
        private void CopyPartnerName(System.Object Sender, EventArgs e)
        {
            if (MessageBox.Show(Catalog.GetString("Be aware that the Account Name field needs to hold the proper name " +
                        "of the Bank Account as assigned by the bank."),
                    Catalog.GetString("Account Name vs. Partner Name"),
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                return;
            }

            PPartnerRow PartnerRow = (PPartnerRow)FMainDS.PPartner.Rows[0];

            if (PartnerRow.PartnerClass == "PERSON")
            {
                PPersonRow PersonRow = (PPersonRow)FMainDS.PPerson.Rows[0];
                txtAccountName.Text = PersonRow.FirstName;

                if ((PersonRow.MiddleName1 != null) && (PersonRow.MiddleName1.Length > 0)
                    && (txtAccountName.Text.Length > 0))
                {
                    txtAccountName.Text += " " + PersonRow.MiddleName1;
                }
                else
                {
                    txtAccountName.Text += PersonRow.MiddleName1;
                }

                if ((PersonRow.FamilyName != null) && (PersonRow.FamilyName.Length > 0)
                    && (txtAccountName.Text.Length > 0))
                {
                    txtAccountName.Text += " " + PersonRow.FamilyName;
                }
                else
                {
                    txtAccountName.Text += PersonRow.FamilyName;
                }
            }
            else if (PartnerRow.PartnerClass == "FAMILY")
            {
                PFamilyRow FamilyRow = (PFamilyRow)FMainDS.PFamily.Rows[0];
                txtAccountName.Text = FamilyRow.FirstName;

                if (txtAccountName.Text.Length > 0)
                {
                    txtAccountName.Text += " " + FamilyRow.FamilyName;
                }
                else
                {
                    txtAccountName.Text += FamilyRow.FamilyName;
                }
            }
            else
            {
                txtAccountName.Text = PartnerRow.PartnerShortName;
            }
        }

        private PBankingDetailsRow LastRowChecked = null;

        // When a bank account is edited, check if it is shared with any other partners. If it is, display a message informing the user.
        private void CheckIfRowIsShared(System.Object Sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow != LastRowChecked) && (FAccountSharedWith.Rows.Count > 0))
            {
                string EditQuestion = "";

                if (FAccountSharedWith.Rows.Count == 1)
                {
                    EditQuestion = Catalog.GetString("This bank account is currently shared with the following Partner:\n");
                }
                else if (FAccountSharedWith.Rows.Count > 1)
                {
                    EditQuestion = Catalog.GetString("This bank account is currently shared with the following Partners:\n");
                }

                for (int i = 0; i < FAccountSharedWith.Rows.Count; i++)
                {
                    // do not allow more than 5 partners to be display. Otherwise message box becomes to long.
                    if (i == 5)
                    {
                        int Remaining = FAccountSharedWith.Rows.Count - i;

                        if (Remaining == 1)
                        {
                            EditQuestion += "\n..." + Catalog.GetString("and 1 other Partner.");
                        }
                        else if (Remaining > 1)
                        {
                            EditQuestion += "\n..." + string.Format(Catalog.GetString("and {0} other Partners."), Remaining);
                        }

                        break;
                    }

                    PPartnerRow Row = (PPartnerRow)FAccountSharedWith.Rows[i];
                    EditQuestion += "\n" + Row.PartnerShortName + " [" + Row.PartnerKey + "]";
                }

                if (FAccountSharedWith.Rows.Count == 1)
                {
                    EditQuestion += Catalog.GetString("\n\nChanges to the Bank Account details here will take effect on the other partner's too.");
                }
                else if (FAccountSharedWith.Rows.Count > 1)
                {
                    EditQuestion += Catalog.GetString("\n\nChanges to the Bank Account details here will take effect on the other partners' too.");
                }

                MessageBox.Show(EditQuestion,
                    Catalog.GetString("Bank Account is used by another Partner"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            LastRowChecked = FPreviouslySelectedDetailRow;
        }

        private void pnlDetails_VisibleChanged(System.Object sender, System.EventArgs e)
        {
            // if FindByBankDetails tab is selected and there is at least 1 row in the grid
            if (pnlDetails.Visible && (FBankDataset == null) && (grdDetails.Rows.Count > 1))
            {
                Cursor.Current = Cursors.WaitCursor;

                // load bank records
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords();
                txtBankKey.DataSet = FBankDataset;

                // populate the comboboxes for Bank Name and Bank Code
                PopulateComboBoxes();

                Cursor.Current = Cursors.Default;
            }
        }

        private void grdDetails_FocusRowLeavingManual(object sender, SourceGrid.RowCancelEventArgs e)
        {
            FValidateBankingDetailsExtra = true;
            grdDetails_FocusRowLeaving(sender, e);
        }

        private void ChkLimitTaxDeductibility_Change(System.Object sender, System.EventArgs e)
        {
            txtTaxDeductiblePercentage.Enabled = chkLimitTaxDeductibility.Checked;
            dtpTaxDeductibleValidFrom.Enabled = chkLimitTaxDeductibility.Checked;

            if (dtpTaxDeductibleValidFrom.Enabled && string.IsNullOrEmpty(dtpTaxDeductibleValidFrom.Text))
            {
                dtpTaxDeductibleValidFrom.Text = DateTime.Today.ToString();
            }
        }

        #endregion

        #region Deletion

        private bool PreDeleteManual(PartnerEditTDSPBankingDetailsRow ARowToDelete, ref String ADeletionQuestion)
        {
            ADeletionQuestion = "";

            // additional message if the bank account to be deleted is shared with one or more other Partners
            if (FAccountSharedWith.Rows.Count > 0)
            {
                if (FAccountSharedWith.Rows.Count == 1)
                {
                    ADeletionQuestion = Catalog.GetString("This bank account is currently shared with the following Partner:\n");
                }
                else if (FAccountSharedWith.Rows.Count > 1)
                {
                    ADeletionQuestion = Catalog.GetString("This bank account is currently shared with the following Partners:\n");
                }

                for (int i = 0; i < FAccountSharedWith.Rows.Count; i++)
                {
                    // do not allow more than 5 partners to be display. Otherwise message box becomes to long.
                    if (i == 5)
                    {
                        int Remaining = FAccountSharedWith.Rows.Count - i;

                        if (Remaining == 1)
                        {
                            ADeletionQuestion += "\n..." + Catalog.GetString("and 1 other Partner.");
                        }
                        else if (Remaining > 1)
                        {
                            ADeletionQuestion += "\n..." + string.Format(Catalog.GetString("and {0} other Partners."), Remaining);
                        }

                        break;
                    }

                    PPartnerRow Row = (PPartnerRow)FAccountSharedWith.Rows[i];
                    ADeletionQuestion += "\n" + Row.PartnerShortName + " [" + Row.PartnerKey + "]";
                }

                if (FAccountSharedWith.Rows.Count == 1)
                {
                    ADeletionQuestion += Catalog.GetString("\n\nThe bank account will not be removed from this other partner.\n\n");
                }
                else if (FAccountSharedWith.Rows.Count > 1)
                {
                    ADeletionQuestion += Catalog.GetString("\n\nThe bank account will not be removed from these other partners.\n\n");
                }
            }

            ADeletionQuestion += Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine,
                lblAccountName.Text,
                txtAccountName.Text);

            return true;
        }

        private bool DeleteRowManual(PartnerEditTDSPBankingDetailsRow ARowToDelete, ref String ACompletionMessage)
        {
            ACompletionMessage = String.Empty;

            // if there are 2 records in grid but one is deleted... set one remaining record as Main Account
            if (ARowToDelete.MainAccount && (grdDetails.Rows.Count == 3))
            {
                foreach (DataRow Row in FMainDS.PBankingDetails.Rows)
                {
                    PartnerEditTDSPBankingDetailsRow BankingDetailsRow = (PartnerEditTDSPBankingDetailsRow)Row;

                    if ((Row.RowState != DataRowState.Deleted)
                        && (BankingDetailsRow.BankingDetailsKey != ARowToDelete.BankingDetailsKey))
                    {
                        BankingDetailsRow.MainAccount = true;
                        break;
                    }
                }
            }

            FMainDS.PPartnerBankingDetails.DefaultView.Sort = PPartnerBankingDetailsTable.GetBankingDetailsKeyDBName();
            FMainDS.PPartnerBankingDetails.DefaultView.FindRows(ARowToDelete.BankingDetailsKey)[0].Row.Delete();

            // if bank account is a 'Main' account then a record in PBankingDetailsUsage will also need deleted.
            if (FMainDS.PBankingDetailsUsage != null)
            {
                FMainDS.PBankingDetailsUsage.DefaultView.Sort = PBankingDetailsUsageTable.GetBankingDetailsKeyDBName();
                DataRowView[] RowsToDelete = FMainDS.PBankingDetailsUsage.DefaultView.FindRows(ARowToDelete.BankingDetailsKey);

                foreach (DataRowView Row in RowsToDelete)
                {
                    Row.Delete();
                }
            }

            // only delete PBankingDetailsRow if it is not shared with any other Partners
            if (FAccountSharedWith.Rows.Count == 0)
            {
                ARowToDelete.Delete();
            }
            else
            {
                FMainDS.PBankingDetails.Rows.Remove(ARowToDelete);
            }

            return true;
        }

        private void PostDeleteManual(PartnerEditTDSPBankingDetailsRow ARowToDelete,
            Boolean AAllowDeletion,
            Boolean ADeletionPerformed,
            String ACompletionMessage)
        {
            if (grdDetails.Rows.Count <= 1)
            {
                // disable buttons and make details panel invisible if no record in grid (first row for headings)
                btnSetMainAccount.Enabled = false;
                pnlDetails.Visible = false;
            }

            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        #endregion

        #region Validation

        private void ValidateDataDetailsManual(PBankingDetailsRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            if (FTaxDeductiblePercentageEnabled && chkLimitTaxDeductibility.Checked)
            {
                // validate dtpTaxDeductibleValidFrom
                ValidateValidFromDate();
            }

            // obtain the bank's country code (if it exists)
            string CountryCode = "";

            if (lblCountry.Text.Length > 8)
            {
                CountryCode = lblCountry.Text.Substring(9);
            }

            // validate bank account details
            TSharedPartnerValidation_Partner.ValidateBankingDetails(this,
                ARow,
                FMainDS.PBankingDetails,
                CountryCode,
                ref VerificationResultCollection,
                FValidationControlsDict);

            // extra validation
            if (FValidateBankingDetailsExtra)
            {
                TSharedPartnerValidation_Partner.ValidateBankingDetailsExtra(this,
                    ARow,
                    ref VerificationResultCollection,
                    FValidationControlsDict);

                FValidateBankingDetailsExtra = false;
            }
        }

        /// <summary>
        /// Adds validation for dtpTaxDeductibleValidFrom
        /// </summary>
        /// <returns>Returns false if validation error</returns>
        private bool ValidateValidFromDate()
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TScreenVerificationResult VerificationResult = null;
            DataColumn ValidationColumn = FMainDS.PPartnerTaxDeductiblePct.ColumnDateValidFrom;
            bool ReturnValue = true;

            // validate dtpTaxDeductibleValidFrom
            if (!dtpTaxDeductibleValidFrom.ValidDate(false))
            {
                VerificationResult = new TScreenVerificationResult(dtpTaxDeductibleValidFrom.DateVerificationResult,
                    ValidationColumn,
                    dtpTaxDeductibleValidFrom);
                VerificationResult.OverrideResultContext(this);

                ReturnValue = false;
            }

            VerificationResultCollection.Remove(ValidationColumn);
            VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);

            return ReturnValue;
        }

        #endregion

        #region Menu and command key handlers for our user controls

        ///////////////////////////////////////////////////////////////////////////////
        //// Special Handlers for menus and command keys for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            return false;
        }

        private void FocusFirstEditableControlManual()
        {
            this.cmbReceiptLetterFrequency.Focus();
        }

        #endregion
    }
}