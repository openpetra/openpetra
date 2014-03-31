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
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner.Verification;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmFieldOfService
    {
        private Int64 FPartnerKey;

        /// constructor (use this one!)
        public TFrmFieldOfService(Form AParentForm, long APartnerKey) : base()
        {
            FPartnerKey = APartnerKey;

            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PPartnerFieldOfServiceTable.GetPartnerKeyDBName(), FPartnerKey);

            Initialize(AParentForm, Search);
        }

        private void InitializeManualCode()
        {
            txtPartnerKey.Text = FPartnerKey.ToString();

            // display the partner's name
            PPartnerTable Table = TRemote.MPartner.Partner.WebConnectors.GetPartnerDetails(FPartnerKey, false, false, false).PPartner;

            if (Table != null)
            {
                PPartnerRow Row = Table[0];
                txtName.Text = Row.PartnerShortName;
            }

            // manually add a forth column which displays the fields partner short name
            DataColumn FieldName = new DataColumn("FieldName", Type.GetType("System.String"));
            FMainDS.PPartnerFieldOfService.Columns.Add(FieldName);

            grdDetails.Columns.Clear();
            grdDetails.AddDateColumn("Date Effective From", FMainDS.PPartnerFieldOfService.ColumnDateEffective);
            grdDetails.AddDateColumn("Date of Expiry", FMainDS.PPartnerFieldOfService.ColumnDateExpires);
            grdDetails.AddPartnerKeyColumn("Field Key", FMainDS.PPartnerFieldOfService.ColumnFieldKey, 90);
            grdDetails.AddPartnerKeyColumn("Field Name", FieldName);

            // TODO this does not work?!
            //grdDetails.Columns[0].Width = 90;
            //grdDetails.Columns[1].Width = 90;

            foreach (DataRow Row in FMainDS.PPartnerFieldOfService.Rows)
            {
                string PartnerShortName;
                TPartnerClass PartnerClass;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                    Convert.ToInt64(Row[PPartnerFieldOfServiceTable.ColumnFieldKeyId]), out PartnerShortName, out PartnerClass);
                Row["FieldName"] = PartnerShortName;
            }

            // create a new column so we can to sort the table with null DateExpires values at the top
            DataColumn NewColumn = new DataColumn("Order", Type.GetType("System.DateTime"));
            FMainDS.PPartnerFieldOfService.Columns.Add(NewColumn);

            foreach (DataRow Row in FMainDS.PPartnerFieldOfService.Rows)
            {
                if (Row["p_date_expires_d"] == DBNull.Value)
                {
                    Row["Order"] = DateTime.MaxValue;
                }
                else
                {
                    Row["Order"] = Row["p_date_expires_d"];
                }
            }

            DataView myDataView = FMainDS.PPartnerFieldOfService.DefaultView;
            myDataView.Sort = "Order DESC, p_date_effective_d DESC";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
        }

        private void NewRow(Object Sender, EventArgs e)
        {
            if (CreateNewPPartnerFieldOfService())
            {
                txtDetailFieldKey.Focus();
            }
        }

        private void NewRowManual(ref PPartnerFieldOfServiceRow ANewRow)
        {
            int Max = 0;

            foreach (PPartnerFieldOfServiceRow Row in FMainDS.PPartnerFieldOfService.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted) && (Row.Key >= Max))
                {
                    Max = Row.Key + 1;
                }
            }

            ANewRow.Key = Math.Max(Max, TRemote.MPartner.Partner.WebConnectors.GetNewKeyForPartnerFieldOfService());
            ANewRow.PartnerKey = FPartnerKey;
            ANewRow.FieldKey = 0;
            ANewRow["Order"] = DateTime.MaxValue;
        }

        private void DateExpiresEntered(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dtpDetailDateExpires.Text))
            {
                FPreviouslySelectedDetailRow["Order"] = DateTime.MaxValue;
            }
            else
            {
                FPreviouslySelectedDetailRow["Order"] = Convert.ToDateTime(dtpDetailDateExpires.Text);
            }
        }

        private void UpdateFieldName(object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow.RowState != DataRowState.Unchanged)
            {
                string PartnerShortName;
                TPartnerClass PartnerClass;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                    Convert.ToInt64(txtDetailFieldKey.Text), out PartnerShortName, out PartnerClass);

                FPreviouslySelectedDetailRow["FieldName"] = PartnerShortName;
            }
        }

        private void ValidateDataDetailsManual(PPartnerFieldOfServiceRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateFieldOfServiceManual(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }
    }
}