//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmMaritalStatusSetup
    {
        private void NewRowManual(ref PtMaritalStatusRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PtMaritalStatus.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PtMaritalStatus.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.Code = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtMaritalStatus();
        }

        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailAssignableDate.Enabled = !chkDetailAssignableFlag.Checked;

            if (chkDetailAssignableFlag.Checked)
            {
                dtpDetailAssignableDate.Date = null;

                // Hide any shown Data Validation ToolTip as the Data Validation ToolTip for an
                // empty Unassignable Date might otherwise be left shown
                FPetraUtilsObject.ValidationToolTip.RemoveAll();
            }
            else
            {
                dtpDetailAssignableDate.Date = DateTime.Now.Date;
            }
        }

        private void ValidateDataDetailsManual(PtMaritalStatusRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateMaritalStatus(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    PtMaritalStatusTable.ColumnCodeId,
                    PtMaritalStatusTable.ColumnDescriptionId,
                    PtMaritalStatusTable.ColumnAssignableFlagId,
                    PtMaritalStatusTable.ColumnAssignableDateId,
                    PtMaritalStatusTable.ColumnDeletableFlagId
                });
        }
    }
}