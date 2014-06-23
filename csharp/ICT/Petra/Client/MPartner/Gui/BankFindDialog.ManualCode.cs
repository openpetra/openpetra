//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2011 by OM International
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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Description of TFrmBankFindDialog.ManualCode.
    /// </summary>
    public partial class TFrmBankFindDialog
    {
        #region Properties

        /// <summary>
        /// DataTable displayed in the grid
        /// </summary>
        private DataTable FCriteriaData = new DataTable();

        private Int64 FBankPartnerKey;

        /// <summary>
        /// Partner Key of selected Bank
        /// </summary>
        public Int64 BankPartnerKey
        {
            get
            {
                return FBankPartnerKey;
            }
        }

        /// <summary>
        /// Dataset
        /// </summary>
        public BankTDS MainDS
        {
            get
            {
                return FMainDS;
            }
        }

        #endregion

        #region Setup

        private void InitializeManualCode()
        {
            // set status bar text for controls
            FPetraUtilsObject.SetStatusBarText(txtBranchName, Catalog.GetString("Enter a Bank name"));
            FPetraUtilsObject.SetStatusBarText(txtBranchCode, Catalog.GetString("Enter a Bank/Branch Code"));
            FPetraUtilsObject.SetStatusBarText(txtBicCode, Catalog.GetString("Enter a Bank's BIC/SWIFT Code"));
            FPetraUtilsObject.SetStatusBarText(txtCity, Catalog.GetString("Enter a Bank's City"));
            FPetraUtilsObject.SetStatusBarText(txtCountry, Catalog.GetString("Enter a Bank's Country"));
            FPetraUtilsObject.SetStatusBarText(btnClear, Catalog.GetString("Clear the filter text"));
            FPetraUtilsObject.SetStatusBarText(btnEdit, Catalog.GetString("Open the Partner Edit screen for the selected Partner"));
            FPetraUtilsObject.SetStatusBarText(btnAccept, Catalog.GetString("Accept selected Bank"));
            FPetraUtilsObject.SetStatusBarText(btnCancel, Catalog.GetString("Cancel selection"));

            // add additional event to chkbox
            chkShowInactive.CheckedChanged += new System.EventHandler(this.SelectRowInGrid);
        }

        /// <summary>
        /// Load data into grid
        /// </summary>
        /// <param name="AFirstTime">True if being run for the first time</param>
        public void LoadDataGrid(bool AFirstTime)
        {
            // Only call data from server if the dataset is actually empty.
            // (A filled dataset is passed to this screen from the 'Finance Details' tab.)
            if ((FMainDS == null) || (FMainDS.PBank.Rows.Count == 0))
            {
                FMainDS = new BankTDS();
                FMainDS.Merge(TRemote.MPartner.Partner.WebConnectors.GetPBankRecords(true));
            }

            FCriteriaData.Clear();

            if (AFirstTime)
            {
                // setup the grid on first run
                SetupGrid();
            }

            // create a new row for each bank record
            foreach (BankTDSPBankRow BankRow in FMainDS.PBank.Rows)
            {
                if (BankRow.PartnerKey >= 0)
                {
                    DataRow NewBankRow = FCriteriaData.NewRow();
                    NewBankRow[PBankTable.GetPartnerKeyDBName()] = BankRow.PartnerKey;
                    NewBankRow[PLocationTable.GetSiteKeyDBName()] = 0;
                    NewBankRow[PLocationTable.GetLocationKeyDBName()] = 0;
                    NewBankRow[PBankTable.GetBranchNameDBName()] = BankRow.BranchName;

                    if (BankRow.BranchCode.StartsWith(SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS + " "))
                    {
                        NewBankRow[PBankTable.GetBranchCodeDBName()] = BankRow.BranchCode.Substring(11);
                    }
                    else if (BankRow.BranchCode.StartsWith(SharedConstants.INACTIVE_VALUE_WITH_QUALIFIERS))
                    {
                        NewBankRow[PBankTable.GetBranchCodeDBName()] = BankRow.BranchCode.Substring(10);
                    }
                    else
                    {
                        NewBankRow[PBankTable.GetBranchCodeDBName()] = BankRow.BranchCode;
                    }

                    NewBankRow[PBankTable.GetBicDBName()] = BankRow.Bic;
                    NewBankRow[BankTDSPBankTable.GetStatusCodeDBName()] = BankRow.StatusCode;
                    FCriteriaData.Rows.Add(NewBankRow);
                }
            }

            // add location information if it exists
            foreach (PPartnerLocationRow Row in FMainDS.PPartnerLocation.Rows)
            {
                DataRow DRow = FCriteriaData.Rows.Find(new object[] { Row.PartnerKey, 0, 0 });
                PLocationRow LocationRow = (PLocationRow)FMainDS.PLocation.Rows.Find(new object[] { Row.SiteKey, Row.LocationKey });

                if (DRow != null)
                {
                    DRow[PLocationTable.GetSiteKeyDBName()] = Row.SiteKey;
                    DRow[PLocationTable.GetLocationKeyDBName()] = Row.LocationKey;
                    DRow[PLocationTable.GetCityDBName()] = LocationRow.City;
                    DRow[PLocationTable.GetCountryCodeDBName()] = LocationRow.CountryCode;
                }
                // if more than one location exists for a bank create new record/s for the additional location/s
                else
                {
                    BankTDSPBankRow BankRow = (BankTDSPBankRow)FMainDS.PBank.Rows.Find(new object[] { Row.PartnerKey });

                    DataRow NewBankRow = FCriteriaData.NewRow();
                    NewBankRow[PBankTable.GetPartnerKeyDBName()] = BankRow.PartnerKey;
                    NewBankRow[PLocationTable.GetSiteKeyDBName()] = Row.SiteKey;
                    NewBankRow[PLocationTable.GetLocationKeyDBName()] = Row.LocationKey;
                    NewBankRow[PBankTable.GetBranchNameDBName()] = BankRow.BranchName;
                    NewBankRow[PBankTable.GetBranchCodeDBName()] = BankRow.BranchCode;
                    NewBankRow[PBankTable.GetBicDBName()] = BankRow.Bic;
                    NewBankRow[PLocationTable.GetCityDBName()] = LocationRow.City;
                    NewBankRow[PLocationTable.GetCountryCodeDBName()] = LocationRow.CountryCode;
                    NewBankRow[BankTDSPBankTable.GetStatusCodeDBName()] = BankRow.StatusCode;
                    FCriteriaData.Rows.Add(NewBankRow);
                }
            }

            // sort order for grid
            DataView MyDataView = FCriteriaData.DefaultView;
            MyDataView.Sort = "p_branch_name_c ASC";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            SelectRowInGrid();
            UpdateRecordNumberDisplay();
            Filter(this, null);
        }

        private void SetupGrid()
        {
            // create a new table
            FCriteriaData.Columns.Add(PBankTable.GetPartnerKeyDBName(), Type.GetType("System.Int64"));
            FCriteriaData.Columns.Add(PLocationTable.GetSiteKeyDBName(), Type.GetType("System.Int64"));
            FCriteriaData.Columns.Add(PLocationTable.GetLocationKeyDBName(), Type.GetType("System.Int32"));
            FCriteriaData.Columns.Add(PBankTable.GetBranchNameDBName(), Type.GetType("System.String"));
            FCriteriaData.Columns.Add(PBankTable.GetBranchCodeDBName(), Type.GetType("System.String"));
            FCriteriaData.Columns.Add(PBankTable.GetBicDBName(), Type.GetType("System.String"));
            FCriteriaData.Columns.Add(PLocationTable.GetCityDBName(), Type.GetType("System.String"));
            FCriteriaData.Columns.Add(PLocationTable.GetCountryCodeDBName(), Type.GetType("System.String"));
            FCriteriaData.Columns.Add(BankTDSPBankTable.GetStatusCodeDBName(), Type.GetType("System.String"));

            FCriteriaData.PrimaryKey = new DataColumn[] {
                FCriteriaData.Columns[PBankTable.GetPartnerKeyDBName()],
                FCriteriaData.Columns[PLocationTable.GetSiteKeyDBName()], FCriteriaData.Columns[PLocationTable.GetLocationKeyDBName()]
            };
            FCriteriaData.DefaultView.AllowNew = false;

            // add columns to the grid
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Partner Key", FCriteriaData.Columns[PBankTable.GetPartnerKeyDBName()], 80);
            grdDetails.AddTextColumn("Bank Name", FCriteriaData.Columns[PBankTable.GetBranchNameDBName()]);
            grdDetails.AddTextColumn("Bank/Branch Code", FCriteriaData.Columns[PBankTable.GetBranchCodeDBName()], 120);
            grdDetails.AddTextColumn("BIC/SWIFT Code", FCriteriaData.Columns[PBankTable.GetBicDBName()], 110);
            grdDetails.AddTextColumn("Status", FCriteriaData.Columns[BankTDSPBankTable.GetStatusCodeDBName()], 70);
            grdDetails.AddTextColumn("City", FCriteriaData.Columns[PLocationTable.GetCityDBName()], 80);
            grdDetails.AddTextColumn("Country", FCriteriaData.Columns[PLocationTable.GetCountryCodeDBName()], 60);

            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(grdDetails_DoubleClickCell);
            grdDetails.EnterKeyPressed += new TKeyPressedEventHandler(grdDetails_EnterKey);
            grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
        }

        #endregion

        #region Helper methods

        // automatically select the currently selected Bank (if it exists)
        private void SelectRowInGrid()
        {
            int RowPos = 1;

            // if no bank is selected then no row should be selected
            if (FBankPartnerKey == 0)
            {
                btnAccept.Enabled = false;
                btnEdit.Enabled = false;

                return;
            }

            BankTDSPBankRow BankRow = (BankTDSPBankRow)FMainDS.PBank.Rows.Find(new object[] { FBankPartnerKey });

            // if current bank is 'inactive' then make sure chkShowInactive is checked (unchecked by default)
            if ((BankRow.StatusCode != SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                && (chkShowInactive.Checked == false))
            {
                chkShowInactive.Checked = true;
            }

            // look through each row in the grid
            foreach (DataRowView RowView in FCriteriaData.DefaultView)
            {
                // if current grid row is the row we are looking for the select it
                if (Convert.ToInt64(RowView[PBankTable.GetPartnerKeyDBName()]) == FBankPartnerKey)
                {
                    grdDetails.SelectRowWithoutFocus(RowPos);

                    btnAccept.Enabled = true;
                    btnEdit.Enabled = true;

                    return;
                }

                // account for grid rows being filtered by being inactive
                if (chkShowInactive.Checked
                    || (!chkShowInactive.Checked
                        && (RowView[BankTDSPBankTable.GetStatusCodeDBName()].ToString() ==
                            SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))))
                {
                    RowPos++;
                }
            }

            btnAccept.Enabled = false;
            btnEdit.Enabled = false;
        }

        // update the record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdDetails.DataSource != null)
            {
                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
        }

        #endregion

        #region Action Handling

        private void SelectRowInGrid(System.Object sender, EventArgs e)
        {
            SelectRowInGrid();
        }

        private void grdDetails_DoubleClickCell(object Sender, SourceGrid.CellContextEventArgs e)
        {
            Accept(e, null);
        }

        private void grdDetails_EnterKey(object Sender, EventArgs e)
        {
            Accept(e, null);
        }

        /// <summary>
        /// Create a new bank
        /// </summary>
        public void NewBank(System.Object sender, EventArgs e)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smNew, "BANK");
            frm.Show();
        }

        private void Accept(System.Object sender, EventArgs e)
        {
            // only accept if one, and only one, row is selected
            if (grdDetails.SelectedDataRows.Length == 1)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void Cancel(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Filter(System.Object sender, EventArgs e)
        {
            // Apply filter to the grid

            DataView MyDataView = FCriteriaData.DefaultView;

            String Filter = "";

            if (!string.IsNullOrEmpty(txtBranchName.Text))
            {
                Filter += FCriteriaData.Columns[PBankTable.GetBranchNameDBName()] + " LIKE '" + txtBranchName.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtBranchCode.Text))
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FCriteriaData.Columns[PBankTable.GetBranchCodeDBName()] + " LIKE '" + txtBranchCode.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtBicCode.Text))
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FCriteriaData.Columns[PBankTable.GetBicDBName()] + " LIKE '" + txtBicCode.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtCity.Text))
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FCriteriaData.Columns[PLocationTable.GetCityDBName()] + " LIKE '" + txtCity.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtCountry.Text))
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FCriteriaData.Columns[PLocationTable.GetCountryCodeDBName()] + " LIKE '" + txtCountry.Text + "%'";
            }

            if (!chkShowInactive.Checked)
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FCriteriaData.Columns[BankTDSPBankTable.GetStatusCodeDBName()] + " = '" +
                          SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE) + "'";
            }

            MyDataView.RowFilter = Filter;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            UpdateRecordNumberDisplay();
            SelectRowInGrid();
        }

        // A new row is selected in the grid
        private void FocusedRowChanged(System.Object sender, EventArgs e)
        {
            if (grdDetails.SelectedDataRows.Length == 1)
            {
                DataRowView RowDataRowView = (DataRowView)grdDetails.SelectedDataRows[0];

                if (RowDataRowView.Row[BankTDSPBankTable.GetStatusCodeDBName()].ToString() !=
                    SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE))
                {
                    chkShowInactive.Enabled = false;
                }
                else
                {
                    chkShowInactive.Enabled = true;
                }

                // Update property with a new selected bank's partner key
                FBankPartnerKey = Convert.ToInt64(RowDataRowView.Row[BankTDSPBankTable.GetPartnerKeyDBName()]);

                btnAccept.Enabled = true;
                btnEdit.Enabled = true;
            }
        }

        // clear all data in filter
        private void Clear(System.Object sender, EventArgs e)
        {
            txtBranchName.Clear();
            txtBranchCode.Clear();
            txtBicCode.Clear();
            txtCity.Clear();
            txtCountry.Clear();
        }

        // Open Partner Edit screen for the selected bank
        private void EditSelectedPartner(System.Object sender, EventArgs e)
        {
            // Open the selected Bank's Partner Edit screen
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, FBankPartnerKey);
            frm.Show();
        }

        #endregion

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>This screen 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if ((((IFormsMessagePartnerInterface)AFormsMessage.MessageObject).PartnerClass == TPartnerClass.BANK)
                && ((AFormsMessage.MessageClass == TFormsMessageClassEnum.mcNewPartnerSaved)
                    || (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcExistingPartnerSaved)))
            {
                FMainDS = null;
                FBankPartnerKey = ((IFormsMessagePartnerInterface)AFormsMessage.MessageObject).PartnerKey;
                LoadDataGrid(false);

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Used for passing a Partner's partner key to the screen before the screen is actually shown.
        /// </summary>
        public void SetParameters(BankTDS ABankDataset, Int64 ABankKey)
        {
            FMainDS = ABankDataset;
            FBankPartnerKey = ABankKey;
        }

        #endregion
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Find Partner Dialog.
    /// </summary>
    public static class TBankFindDialogManager
    {
        /// <summary>
        /// Opens a Modal instance of the Bank Find Dialog.
        /// </summary>
        /// <param name="ABankDataset">Dataset containing bank data (can be null)</param>
        /// <param name="ABankKey">Matching partner key for selected bank</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if a bank was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(
            ref BankTDS ABankDataset,
            ref Int64 ABankKey,
            Form AParentForm)
        {
            DialogResult dlgResult;

            TFrmBankFindDialog BankFind = new TFrmBankFindDialog(AParentForm);

            BankFind.SetParameters(ABankDataset, ABankKey);
            BankFind.LoadDataGrid(true);

            dlgResult = BankFind.ShowDialog();

            ABankKey = 0;

            if (dlgResult == DialogResult.OK)
            {
                ABankKey = BankFind.BankPartnerKey;
                ABankDataset = BankFind.MainDS;

                return true;
            }

            return false;
        }
    }
}