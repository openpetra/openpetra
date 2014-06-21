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
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Partner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_FinanceDetails
    {
        #region Properties

        /// <summary>Contains a list of all Partners who share the selected bank account</summary>
        private PPartnerTable AccountSharedWith = new PPartnerTable();

        /// <summary>Dataset containg all PBank records for all banks and their locations</summary>
        private BankTDS FBankDataset;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

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

        /// <summary>an event that will reload the grid after saving</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        #endregion

        #region Setup

        /// <summary>
        /// load the data for this control
        /// </summary>
        public void SpecialInitUserControl(PartnerEditTDS AMainDS)
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

            // change status bar texts
            FPetraUtilsObject.SetStatusBarText(txtBankKey, Catalog.GetString("Select a Bank."));
            FPetraUtilsObject.SetStatusBarText(cmbBankName, Catalog.GetString("Select a Bank Name."));
            FPetraUtilsObject.SetStatusBarText(cmbBankCode, Catalog.GetString("Select a Bank Code."));

            // add event which will populate the bank combo boxes when 'Finance details' tab is shown for the first time
            pnlDetails.VisibleChanged += new EventHandler(pnlDetails_VisibleChanged);
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
            // For some reason this method sets the RowState of one PLocation record and one PPartnerLocation record from MainDS to modified,
            // even if it hasn't been modified. This causes the save button to be enabled even though no data might was changed.
            // There is no clear reason why! To avoid this I record all records that are Unchanged at the start of the method and ensure they are
            // still unchanged at the end of the method.
            List <int>PartnerLocationList = new List <int>();
            List <int>LocationList = new List <int>();

            for (int i = 0; i < FMainDS.PPartnerLocation.Rows.Count; i++)
            {
                if (FMainDS.PPartnerLocation.Rows[i].RowState == DataRowState.Unchanged)
                {
                    PartnerLocationList.Add(i);
                }
            }

            for (int i = 0; i < FMainDS.PLocation.Rows.Count; i++)
            {
                if (FMainDS.PLocation.Rows[i].RowState == DataRowState.Unchanged)
                {
                    LocationList.Add(i);
                }
            }

            // temporarily remove the event that enables the save button when data is changed
            FPetraUtilsObject.ActionEnablingEvent -= ((TFrmPartnerEdit)FPetraUtilsObject.GetForm()).ActionEnabledEvent;

            // temporily remove events from comboboxes
            cmbBankName.SelectedValueChanged -= new System.EventHandler(this.BankNameChanged);
            cmbBankCode.SelectedValueChanged -= new System.EventHandler(this.BankCodeChanged);

            // load bank records
            if (FBankDataset == null)
            {
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords(true);
                txtBankKey.DataSet = FBankDataset;
            }

            // add empty row
            DataRow emptyRow = FBankDataset.PBank.NewRow();
            emptyRow[PBankTable.ColumnPartnerKeyId] = 0;
            emptyRow[PBankTable.ColumnBranchNameId] = Catalog.GetString("");
            emptyRow[PBankTable.ColumnBranchCodeId] = Catalog.GetString("");
            FBankDataset.PBank.Rows.Add(emptyRow);

            // add inactive row
            emptyRow = FBankDataset.PBank.NewRow();
            emptyRow[PBankTable.ColumnPartnerKeyId] = -1;
            emptyRow[PBankTable.ColumnBranchNameId] = Catalog.GetString("");
            emptyRow[PBankTable.ColumnBranchCodeId] = SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ";
            FBankDataset.PBank.Rows.Add(emptyRow);

            cmbBankName.InitialiseUserControl();
            cmbBankCode.InitialiseUserControl();

            // populate the bank name combo box
            cmbBankName.InitialiseUserControl(FBankDataset.PBank,
                PBankTable.GetPartnerKeyDBName(),
                PBankTable.GetBranchNameDBName(),
                PBankTable.GetBranchCodeDBName(),
                null);

            cmbBankName.AppearanceSetup(new int[] { 230, 160 }, -1);
            cmbBankName.Filter = PBankTable.GetBranchNameDBName() + " <> '' OR " +
                                 PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = ''";

            cmbBankName.SelectedValueChanged += new System.EventHandler(this.BankNameChanged);

            // populate the bank code combo box
            cmbBankCode.InitialiseUserControl(FBankDataset.PBank,
                PBankTable.GetBranchCodeDBName(),
                PBankTable.GetPartnerKeyDBName(),
                null);

            cmbBankCode.AppearanceSetup(new int[] { 210 }, -1);
            // filter rows that are blank or <INACTIVE>
            cmbBankCode.Filter = "(" + PBankTable.GetBranchCodeDBName() + " <> '' AND " + PBankTable.GetBranchCodeDBName() + " <> '" + SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ') " +
                                 "OR (" + PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = '') " +
                                 "OR (" + PBankTable.GetBranchNameDBName() + " = '' AND " + PBankTable.GetBranchCodeDBName() + " = '" + SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ')";
            cmbBankCode.SelectedValueChanged += new System.EventHandler(this.BankCodeChanged);

            FComboBoxesCreated = true;

            if ((FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.BankKey != 0)
                && (((FCurrentBankRow == null) || (FPreviouslySelectedDetailRow.BankKey != FCurrentBankRow.PartnerKey))))
            {
                PartnerKeyChanged(FPreviouslySelectedDetailRow.BankKey, "", true);
            }

            FPetraUtilsObject.ActionEnablingEvent += ((TFrmPartnerEdit)FPetraUtilsObject.GetForm()).ActionEnabledEvent;

            // reject any unwanted changes
            foreach (int i in PartnerLocationList)
            {
                FMainDS.PPartnerLocation.Rows[i].RejectChanges();
            }

            foreach (int i in LocationList)
            {
                FMainDS.PLocation.Rows[i].RejectChanges();
            }
        }

        #endregion

        #region Helper methods

        private void ShowDataManual()
        {
            if (grdDetails.Rows.Count > 1)
            {
                btnSetMainAccount.Enabled = true;
                pnlDetails.Visible = true;
            }
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
                AccountSharedWith = TRemote.MPartner.Partner.WebConnectors.SharedBankAccountPartners(FPreviouslySelectedDetailRow.BankingDetailsKey,
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

            grdAccountSharedWith.AddTextColumn(Catalog.GetString("Partner Name"), AccountSharedWith.ColumnPartnerShortName, 179);

            DataView MyDataView = AccountSharedWith.DefaultView;
            MyDataView.Sort = "p_partner_key_n ASC";
            MyDataView.AllowNew = false;
            grdAccountSharedWith.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
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
                FMainDS.PPartner[0].EmailGiftStatement = chkEmailGiftStatement.Checked;
                FMainDS.PPartner[0].FinanceComment = txtFinanceComment.Text;
            }
            catch (ConstraintException)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region ActionHandling

        /// <summary>
        /// The currently selected account's PBank row
        /// </summary>
        private PBankRow FCurrentBankRow;

        // called when FindBank dialog is accepted
        private void PartnerKeyChanged(long APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            if (!FComboBoxesCreated)
            {
                return;
            }

            if (FBankDataset == null)
            {
                // populate the comboboxes for Bank Name and Bank Code if not done already (this should never actually happen!)
                PopulateComboBoxes();
            }

            FCurrentBankRow = (PBankRow)FBankDataset.PBank.Rows.Find(new object[] { APartnerKey });

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
                    cmbBankName.SelectedIndex += 1;
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
                lblCountry.Text = Catalog.GetString("Country: No Valid Address On File");

                foreach (PPartnerLocationRow Row in FBankDataset.PPartnerLocation.Rows)
                {
                    if ((Row.PartnerKey == FCurrentBankRow.PartnerKey)
                        && (Row.IsDateGoodUntilNull() || (Row.DateGoodUntil >= DateTime.Today)))
                    {
                        lblCountry.Text = "Country: " +
                                          ((PLocationRow)FBankDataset.PLocation.Rows.Find(new object[] { Row.SiteKey, Row.LocationKey })).CountryCode;
                        break;
                    }
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
            if ((cmbBankName.GetSelectedString() == "") && (FCurrentBankRow.BranchName != ""))
            {
                txtBankKey.Text = "0";
            }
            // cmbBankName.ContainsFocus is needed because the combobox automatically changes the selection
            // to the first row with that name when the focus is left. This was a problem with multiple banks with the same name.
            else if (((FCurrentBankRow == null) || (FCurrentBankRow.PartnerKey.ToString() != cmbBankName.GetSelectedString()))
                     && (cmbBankName.GetSelectedString() != "")
                     && cmbBankName.ContainsFocus)
            {
                FCurrentBankRow = (PBankRow)FBankDataset.PBank.Rows.Find(new object[] { Convert.ToInt64(cmbBankName.GetSelectedString()) });

                // update partner key in txtBankKey
                txtBankKey.Text = FCurrentBankRow.PartnerKey.ToString();
                PartnerKeyChanged(FCurrentBankRow.PartnerKey, "", true);
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
                     || ((cmbBankCode.GetSelectedString() == SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ") && (FCurrentBankRow.BranchCode != SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " ")))
            {
                // if "<INACTIVE>" has been selected change it to blank
                cmbBankCode.SelectedIndex = -1;
                txtBankKey.Text = "0";
            }
            else if ((FCurrentBankRow == null) || (FCurrentBankRow.BranchCode != cmbBankCode.GetSelectedString()))
            {
                FCurrentBankRow = (PBankRow)FBankDataset.PBank.Rows.Find(new object[] { cmbBankCode.GetSelectedDescription() });

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
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords(true);
                txtBankKey.DataSet = FBankDataset;

                // populate the comboboxes for Bank Name and Bank Code
                Thread NewThread = new Thread(PopulateComboBoxes);
                NewThread.Start();

                Cursor.Current = Cursors.Default;
            }

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
            long SharingPartnerKey = Convert.ToInt64(((DataRowView)grdAccountSharedWith.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerKeyDBName()]);

            // Open the selected partner's Partner Edit screen at Personnel Applications
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, SharingPartnerKey, TPartnerEditTabPageEnum.petpFinanceDetails);
            frm.Show();
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
            if ((FPreviouslySelectedDetailRow != LastRowChecked) && (AccountSharedWith.Rows.Count > 0))
            {
                string EditQuestion = "";

                if (AccountSharedWith.Rows.Count == 1)
                {
                    EditQuestion = Catalog.GetString("This bank account is currently shared with the following Partner:\n");
                }
                else if (AccountSharedWith.Rows.Count > 1)
                {
                    EditQuestion = Catalog.GetString("This bank account is currently shared with the following Partners:\n");
                }

                for (int i = 0; i < AccountSharedWith.Rows.Count; i++)
                {
                    // do not allow more than 5 partners to be display. Otherwise message box becomes to long.
                    if (i == 5)
                    {
                        int Remaining = AccountSharedWith.Rows.Count - i;

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

                    PPartnerRow Row = (PPartnerRow)AccountSharedWith.Rows[i];
                    EditQuestion += "\n" + Row.PartnerShortName + " [" + Row.PartnerKey + "]";
                }

                if (AccountSharedWith.Rows.Count == 1)
                {
                    EditQuestion += Catalog.GetString("\n\nChanges to the Bank Account details here will take effect on the other partner's too.");
                }
                else if (AccountSharedWith.Rows.Count > 1)
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
                FBankDataset = TRemote.MPartner.Partner.WebConnectors.GetPBankRecords(true);
                txtBankKey.DataSet = FBankDataset;

                Cursor.Current = Cursors.Default;

                // populate the comboboxes for Bank Name and Bank Code
                Thread NewThread = new Thread(PopulateComboBoxes);
                NewThread.Start();
            }
        }

        #endregion

        #region Deletion

        private bool PreDeleteManual(PartnerEditTDSPBankingDetailsRow ARowToDelete, ref String ADeletionQuestion)
        {
            ADeletionQuestion = "";

            // additional message if the bank account to be deleted is shared with one or more other Partners
            if (AccountSharedWith.Rows.Count > 0)
            {
                if (AccountSharedWith.Rows.Count == 1)
                {
                    ADeletionQuestion = Catalog.GetString("This bank account is currently shared with the following Partner:\n");
                }
                else if (AccountSharedWith.Rows.Count > 1)
                {
                    ADeletionQuestion = Catalog.GetString("This bank account is currently shared with the following Partners:\n");
                }

                for (int i = 0; i < AccountSharedWith.Rows.Count; i++)
                {
                    // do not allow more than 5 partners to be display. Otherwise message box becomes to long.
                    if (i == 5)
                    {
                        int Remaining = AccountSharedWith.Rows.Count - i;

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

                    PPartnerRow Row = (PPartnerRow)AccountSharedWith.Rows[i];
                    ADeletionQuestion += "\n" + Row.PartnerShortName + " [" + Row.PartnerKey + "]";
                }

                if (AccountSharedWith.Rows.Count == 1)
                {
                    ADeletionQuestion += Catalog.GetString("\n\nThe bank account will not be removed from this other partner.\n\n");
                }
                else if (AccountSharedWith.Rows.Count > 1)
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
            if (AccountSharedWith.Rows.Count == 0)
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
        }

        #endregion
    }
}