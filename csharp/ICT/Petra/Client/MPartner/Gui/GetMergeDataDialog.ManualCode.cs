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
using System.Collections.Generic;
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
            btnOK.Text = "OK (" + clbRecords.CheckedItemsCount + ")";
        }

        /// <summary>
        /// Populates the grid for the dialog for selecting Addresses to be merged.
        /// </summary>
        public bool InitializeAddressGrid(long APartnerKey)
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

            if ((FMainDS != null) && (FMainDS.PLocation != null) && (FMainDS.PLocation.Rows.Count > 0))
            {
                DataView MyDataView = FMainDS.PLocation.DefaultView;

                FDataTable = MyDataView.ToTable(true, new string[] { Address1, Street2, Address3, City, LocationKey, SiteKey });
                FDataTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
                FDataTable.Columns.Add(LocationType, typeof(string));

                for (int Counter = 0; Counter < FMainDS.PLocation.Rows.Count; ++Counter)
                {
                    FDataTable.Rows[Counter][LocationType] = FMainDS.PPartnerLocation.Rows[Counter][LocationType];
                }

                clbRecords.Columns.Clear();
                clbRecords.AddCheckBoxColumn("", FDataTable.Columns[CheckedMember], 17, false);
                clbRecords.AddTextColumn("Address-1", FDataTable.Columns[Address1]);
                clbRecords.AddTextColumn("Street-2", FDataTable.Columns[Street2]);
                clbRecords.AddTextColumn("Address-3", FDataTable.Columns[Address3]);
                clbRecords.AddTextColumn("City", FDataTable.Columns[City]);
                clbRecords.AddTextColumn("Location Type", FDataTable.Columns[LocationType]);
                clbRecords.ValueChanged += new EventHandler(OnCheckboxChange);

                clbRecords.DataBindGrid(FDataTable, Address1, CheckedMember, Address1, false, true, false);
                clbRecords.SetCheckedStringList("");

                clbRecords.AutoResizeGrid();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Populates the grid for the dialog for selecting Addresses to be merged.
        /// </summary>
        public bool InitializeContactDetailGrid(long AFromPartnerKey, long AToPartnerKey)
        {
            // set text for label
            lblInfo.Text = Catalog.GetString(
                "The following contact details exist for the Partner being merged. Select the contact details to be transferred.") +
                           "\n\n" + Catalog.GetString("Any contact details which are not selected will be deleted!");

            string CheckedMember = "CHECKED";
            string ContactCategory = PartnerEditTDSPPartnerAttributeTable.GetCategoryCodeDBName();
            string ContactType = PPartnerAttributeTable.GetAttributeTypeDBName();
            string Sequence = PPartnerAttributeTable.GetSequenceDBName();
            string Value = PPartnerAttributeTable.GetValueDBName();
            string Primary = PPartnerAttributeTable.GetPrimaryDBName();
            string Business = PPartnerAttributeTable.GetSpecialisedDBName();
            string Current = PPartnerAttributeTable.GetCurrentDBName();
            string NoLongerCurrentFrom = PPartnerAttributeTable.GetNoLongerCurrentFromDBName();
            string Comment = PPartnerAttributeTable.GetCommentDBName();


            PartnerEditTDSPPartnerAttributeTable ContactDetails =
                TRemote.MPartner.Partner.WebConnectors.GetPartnerContactDetails(AFromPartnerKey, AToPartnerKey);

            if ((ContactDetails != null) && (ContactDetails.Rows.Count > 0))
            {
                DataView MyDataView = ContactDetails.DefaultView;

                FDataTable = MyDataView.ToTable(true,
                    new string[] { ContactCategory, ContactType, Sequence, Value, Primary, Business, Current, NoLongerCurrentFrom, Comment });
                FDataTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

                clbRecords.Columns.Clear();
                clbRecords.AddCheckBoxColumn("", FDataTable.Columns[CheckedMember], 17, false);
                clbRecords.AddTextColumn("Category", FDataTable.Columns[ContactCategory]);
                clbRecords.AddTextColumn("Type", FDataTable.Columns[ContactType]);
                clbRecords.AddTextColumn("Value", FDataTable.Columns[Value]);
                clbRecords.AddCheckBoxColumn("Primary", FDataTable.Columns[Primary], true);
                clbRecords.AddCheckBoxColumn("Business", FDataTable.Columns[Business], true);
                clbRecords.AddCheckBoxColumn("Current", FDataTable.Columns[Current], true);
                clbRecords.AddDateColumn("No Longer Current From", FDataTable.Columns[NoLongerCurrentFrom]);
                clbRecords.AddTextColumn("Comment", FDataTable.Columns[Comment]);
                clbRecords.ValueChanged += new EventHandler(OnCheckboxChange);

                clbRecords.DataBindGrid(FDataTable, ContactCategory + ", " + ContactType + ", " + Value, CheckedMember, Value, false, true, false);
                clbRecords.SetCheckedStringList("");

                clbRecords.AutoResizeGrid();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Populates the grid for the dialog for selecting which bank account should be the main one.
        /// </summary>
        public bool InitializeBankAccountGrid(long AFromPartnerKey, long AToPartnerKey)
        {
            // set text for label
            lblInfo.Text = Catalog.GetString("Please choose the Bank Account that should become the Main Account for the merged Partner.");

            PBankingDetailsTable BankingDetailsTable = TRemote.MPartner.Partner.WebConnectors.GetPartnerBankingDetails(
                AFromPartnerKey, AToPartnerKey);

            if ((BankingDetailsTable != null) && (BankingDetailsTable.Rows.Count > 0))
            {
                clbRecords.Columns.Clear();
                clbRecords.AddTextColumn("Account Name", BankingDetailsTable.ColumnAccountName);
                clbRecords.AddTextColumn("Account Number", BankingDetailsTable.ColumnBankAccountNumber);
                clbRecords.AddTextColumn("IBAN", BankingDetailsTable.ColumnBankingDetailsKey);

                DataView MyDataView = BankingDetailsTable.DefaultView;
                MyDataView.AllowNew = false;
                clbRecords.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

                clbRecords.AutoResizeGrid();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the LocationKeys for all the rows selected in the grid.
        /// </summary>
        public int[] GetSelectedLocationKeys()
        {
            int[] LocationKeys = new int[clbRecords.CheckedItemsCount];
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
            long[] SiteKeys = new long[clbRecords.CheckedItemsCount];
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
        /// Returns the attribute types and sequences for all the contact details selected in the grid.
        /// </summary>
        public List <string[]>GetContactDetails()
        {
            List <string[]>ContactDetails = new List <string[]>();
            int i = 0;

            foreach (DataRow Row in FDataTable.Rows)
            {
                if (Convert.ToBoolean(Row["Checked"]) == true)
                {
                    ContactDetails.Add(new string[] { Row[PPartnerAttributeTable.GetAttributeTypeDBName()].ToString(),
                                                      Row[PPartnerAttributeTable.GetSequenceDBName()].ToString() });
                    i++;
                }
            }

            return ContactDetails;
        }

        /// <summary>
        /// Returns the BankingDetailsKey for the selected row in the grid.
        /// </summary>
        public int GetSelectedBankAccount()
        {
            DataRowView[] SelectedRow = clbRecords.SelectedDataRowsAsDataRowView;
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
        /// <param name="AMergeAction">Determines the type of data the dialog will display.</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if Addresses were found and accepted by the user, otherwise false.</returns>
        public static bool OpenModalForm(long AFromPartnerKey, long AToPartnerKey, TMergeActionEnum AMergeAction, Form AParentForm)
        {
            DialogResult dlgResult;

            TFrmGetMergeDataDialog SelectDialog = new TFrmGetMergeDataDialog(AParentForm);
            bool RecordsExist = true;

            if (AMergeAction == TMergeActionEnum.ADDRESS)
            {
                RecordsExist = SelectDialog.InitializeAddressGrid(AFromPartnerKey);
            }
            else if (AMergeAction == TMergeActionEnum.CONTACTDETAIL)
            {
                RecordsExist = SelectDialog.InitializeContactDetailGrid(AFromPartnerKey, AToPartnerKey);
            }
            else if (AMergeAction == TMergeActionEnum.BANKACCOUNT)
            {
                SelectDialog.InitializeBankAccountGrid(AFromPartnerKey, AToPartnerKey);
            }

            if (!RecordsExist)
            {
                return true;
            }

            dlgResult = SelectDialog.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                if (AMergeAction == TMergeActionEnum.ADDRESS)
                {
                    TFrmMergePartnersDialog.LocationKeys = SelectDialog.GetSelectedLocationKeys();
                    TFrmMergePartnersDialog.SiteKeys = SelectDialog.GetSelectedSiteKeys();
                }

                if (AMergeAction == TMergeActionEnum.CONTACTDETAIL)
                {
                    TFrmMergePartnersDialog.ContactDetails = SelectDialog.GetContactDetails();
                }
                else if (AMergeAction == TMergeActionEnum.BANKACCOUNT)
                {
                    TFrmMergePartnersDialog.MainBankingDetailsKey = SelectDialog.GetSelectedBankAccount();
                }

                return true;
            }

            return false;
        }
    }
}