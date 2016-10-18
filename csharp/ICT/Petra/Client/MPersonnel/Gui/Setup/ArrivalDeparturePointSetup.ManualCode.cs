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
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmArrivalDeparturePointSetup
    {
        private void NewRowManual(ref PtArrivalPointRow ARow)
        {
            // Deal with primary key.  AssignmentTypeCode is unique and is 8 characters.
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.PtArrivalPoint.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PtArrivalPoint.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.Code = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtArrivalPoint();
        }

        private void EnableDisableUnassignableDate(Object sender, EventArgs e)
        {
            dtpDetailUnassignableDate.Enabled = chkDetailUnassignableFlag.Checked;

            if (!chkDetailUnassignableFlag.Checked)
            {
                dtpDetailUnassignableDate.Date = null;

                // Hide any shown Data Validation ToolTip as the Data Validation ToolTip for an
                // empty Unassignable Date might otherwise be left shown
                FPetraUtilsObject.ValidationToolTip.RemoveAll();
            }
            else
            {
                dtpDetailUnassignableDate.Date = DateTime.Now.Date;
            }
        }

        private void ValidateDataDetailsManual(PtArrivalPointRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateArrivalDeparturePoint(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    PtArrivalPointTable.ColumnCodeId,
                    PtArrivalPointTable.ColumnDescriptionId,
                    PtArrivalPointTable.ColumnUnassignableFlagId,
                    PtArrivalPointTable.ColumnUnassignableDateId,
                    PtArrivalPointTable.ColumnDeletableFlagId
                });
        }
    }
}