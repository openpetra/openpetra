//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmGetMergeDataDialog
    {
        private PartnerEditTDS FMainDS;
        private DataTable FDataTable;

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCheckboxChange(Object sender, EventArgs e)
        {
            btnOK.Text = "OK (" + clbAddress.CheckedItemsCount + ")";
        }

        /// <summary>
        /// Populates the grid for the dialog for selecting Addresses to be merged.
        /// </summary>
        public void InitializeAddressGrid(long APartnerKey)
        {
            // set text for label
            lblInfo.Text = Catalog.GetString("The following addresses exist for the Partner being merged. Select the addresses to be transferred.") +
                           "\n\n" + Catalog.GetString("Any addresses which are not selected will be deleted!");

            string CheckedMember = "CHECKED";
            string Address1 = PLocationTable.GetLocalityDBName();
            string Street2 = PLocationTable.GetStreetNameDBName();
            string Address3 = PLocationTable.GetAddress3DBName();
            string City = PLocationTable.GetCityDBName();
            string LocationKey = PLocationTable.GetLocationKeyDBName();
            string SiteKey = PLocationTable.GetSiteKeyDBName();
            string LocationType = PPartnerLocationTable.GetLocationTypeDBName();


            FMainDS = TRemote.MPartner.Partner.WebConnectors.GetPartnerDetails(APartnerKey, true, false, false);
            DataView MyDataView = FMainDS.PLocation.DefaultView;

            FDataTable = MyDataView.ToTable(true, new string[] { Address1, Street2, Address3, City, LocationKey, SiteKey });
            FDataTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
            FDataTable.Columns.Add(LocationType, typeof(string));

            for (int Counter = 0; Counter < FMainDS.PLocation.Rows.Count; ++Counter)
            {
                FDataTable.Rows[Counter][LocationType] = FMainDS.PPartnerLocation.Rows[Counter][LocationType];
            }

            clbAddress.Columns.Clear();
            clbAddress.AddCheckBoxColumn("Select", FDataTable.Columns[CheckedMember], 17, false);
            clbAddress.AddTextColumn("Address-1", FDataTable.Columns[Address1]);
            clbAddress.AddTextColumn("Street-2", FDataTable.Columns[Street2]);
            clbAddress.AddTextColumn("Address-3", FDataTable.Columns[Address3]);
            clbAddress.AddTextColumn("City", FDataTable.Columns[City]);
            clbAddress.AddTextColumn("Location Type", FDataTable.Columns[LocationType]);
            clbAddress.ValueChanged += new EventHandler(OnCheckboxChange);

            clbAddress.DataBindGrid(FDataTable, Address1, CheckedMember, Address1, false, true, false);
            clbAddress.SetCheckedStringList("");
        }

        /// <summary>
        /// Populates the grid for the dialog for selecting which bank account should be the main one.
        /// </summary>
        public void InitializeBankAccountGrid(long AFromPartnerKey, long AToPartnerKey)
        {
            // set text for label
            lblInfo.Text = Catalog.GetString("Please choose the Bank Account that should become the Main Account for the merged Partner.");

            PBankingDetailsTable BankingDetailsTable = TRemote.MPartner.Partner.WebConnectors.GetPartnerBankingDetails(
                AFromPartnerKey, AToPartnerKey);

            clbAddress.Columns.Clear();
            clbAddress.AddTextColumn("Account Name", BankingDetailsTable.ColumnAccountName);
            clbAddress.AddTextColumn("Account Number", BankingDetailsTable.ColumnBankAccountNumber);
            clbAddress.AddTextColumn("IBAN", BankingDetailsTable.ColumnBankingDetailsKey);

            DataView MyDataView = BankingDetailsTable.DefaultView;
            MyDataView.AllowNew = false;
            clbAddress.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        /// <summary>
        /// Returns the LocationKeys for all the rows selected in the grid.
        /// </summary>
        public int[] GetSelectedLocationKeys()
        {
            int[] LocationKeys = new int[clbAddress.CheckedItemsCount];
            int i = 0;

            foreach (DataRow Row in FDataTable.Rows)
            {
                if (Convert.ToBoolean(Row["Checked"]) == true)
                {
                    LocationKeys[i] = Convert.ToInt32(Row[PLocationTable.GetLocationKeyDBName()]);
                    i++;
                }
            }

            return LocationKeys;
        }

        /// <summary>
        /// Returns the SiteKeys for all the rows selected in the grid.
        /// </summary>
        public long[] GetSelectedSiteKeys()
        {
            long[] SiteKeys = new long[clbAddress.CheckedItemsCount];
            int i = 0;

            foreach (DataRow Row in FDataTable.Rows)
            {
                if (Convert.ToBoolean(Row["Checked"]) == true)
                {
                    SiteKeys[i] = Convert.ToInt64(Row[PLocationTable.GetSiteKeyDBName()]);
                    i++;
                }
            }

            return SiteKeys;
        }

        /// <summary>
        /// Returns the BankingDetailsKey for the selected row in the grid.
        /// </summary>
        public int GetSelectedBankAccount()
        {
            DataRowView[] SelectedRow = clbAddress.SelectedDataRowsAsDataRowView;
            PBankingDetailsRow Row = (PBankingDetailsRow)SelectedRow[0].Row;

            return Row.BankingDetailsKey;
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Merge Select Dialog
    /// </summary>
    public static class TGetMergeDataManager
    {
        /// <summary>
        /// Opens a Modal instance of the SelectAddresses Dialog
        /// </summary>
        /// <param name="AFromPartnerKey">Pass in the From Partner's Key.</param>
        /// <param name="AToPartnerKey">Pass in the To Partner's Key.</param>
        /// <param name="ADataType">Determines the type of data the dialog will display.</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if Addresses were found and accepted by the user, otherwise false.</returns>
        public static bool OpenModalForm(long AFromPartnerKey, long AToPartnerKey, string ADataType, Form AParentForm)
        {
            DialogResult dlgResult;

            TFrmGetMergeDataDialog SelectDialog = new TFrmGetMergeDataDialog(AParentForm);

            if (ADataType == "ADDRESS")
            {
                SelectDialog.InitializeAddressGrid(AFromPartnerKey);
            }
            else if (ADataType == "BANKACCOUNT")
            {
                SelectDialog.InitializeBankAccountGrid(AFromPartnerKey, AToPartnerKey);
            }

            dlgResult = SelectDialog.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                if (ADataType == "ADDRESS")
                {
                    TFrmMergePartnersDialog.LocationKeys = SelectDialog.GetSelectedLocationKeys();
                    TFrmMergePartnersDialog.SiteKeys = SelectDialog.GetSelectedSiteKeys();
                }
                else if (ADataType == "BANKACCOUNT")
                {
                    TFrmMergePartnersDialog.MainBankingDetailsKey = SelectDialog.GetSelectedBankAccount();
                }

                return true;
            }

            return false;
        }
    }
}