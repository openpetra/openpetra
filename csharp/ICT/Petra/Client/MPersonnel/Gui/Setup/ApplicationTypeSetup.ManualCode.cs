//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmApplicationTypeSetup
    {
        private void InitializeManualCode()
        {
            // the column for "Application for" must show a different text than in the db field
            String Event = Catalog.GetString("Event");
            String Field = Catalog.GetString("Field");

            DataColumn TableColumn = new DataColumn();

            TableColumn.DataType = System.Type.GetType("System.String");
            TableColumn.ColumnName = "ApplicationFor";
            TableColumn.Expression = "IIF(" + PtApplicationTypeTable.GetAppFormTypeDBName() +
                                     "='LONG FORM','" + Field + "','" + Event + "')";
            FMainDS.PtApplicationType.Columns.Add(TableColumn);

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Application Type", FMainDS.PtApplicationType.ColumnAppTypeName);
            grdDetails.AddTextColumn("Description", FMainDS.PtApplicationType.ColumnAppTypeDescr);
            grdDetails.AddTextColumn("Application for", FMainDS.PtApplicationType.Columns["ApplicationFor"]);
            grdDetails.AddCheckBoxColumn("Unassignable?", FMainDS.PtApplicationType.ColumnUnassignableFlag);
            grdDetails.AddDateColumn("Unassignable Date", FMainDS.PtApplicationType.ColumnUnassignableDate);
            grdDetails.AddCheckBoxColumn("Deletable", FMainDS.PtApplicationType.ColumnDeletableFlag);
        }

        private void RunOnceOnActivationManual()
        {
            chkDetailDeletableFlag.Enabled = false;
        }

        private void NewRowManual(ref PtApplicationTypeRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PtApplicationType.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PtApplicationType.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.AppTypeName = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtApplicationType();
        }

        private void GetDetailDataFromControlsManual(PtApplicationTypeRow ARow)
        {
            if (rbtField.Checked)
            {
                ARow.AppFormType = "LONG FORM";
            }
            else
            {
                ARow.AppFormType = "SHORT FORM";
            }
        }

        private void ShowDetailsManual(PtApplicationTypeRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            if (ARow.AppFormType == "SHORT FORM")
            {
                rbtEvent.Checked = true;
                rbtField.Checked = false;
            }
            else if (ARow.AppFormType == "LONG FORM")
            {
                rbtEvent.Checked = false;
                rbtField.Checked = true;
            }

            // once record is saved then application form type cannot be changed any more
            rgrDetailAppFormType.Enabled = (ARow.RowState == DataRowState.Added);
        }

        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailUnassignableDate.Enabled = chkDetailUnassignableFlag.Checked;

            if (!chkDetailUnassignableFlag.Checked)
            {
                dtpDetailUnassignableDate.Date = null;
            }
            else
            {
                dtpDetailUnassignableDate.Date = DateTime.Now.Date;
            }
        }

        private void ValidateDataDetailsManual(PtApplicationTypeRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateApplicationType(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            // The filter string will be something like pt_app_form_type_c LIKE '%Field%'
            // We need to alter that - Event is SHORT FORM and Field is LONG FORM
            AFilterString = AFilterString.Replace("'%Event%'", "'%SHORT%'");
            AFilterString = AFilterString.Replace("'%Field%'", "'%LONG%'");
        }
    }
}