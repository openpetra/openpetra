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
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmJobAssignmentTypeSetup
    {
        private void NewRowManual(ref PtAssignmentTypeRow ARow)
        {
            // Deal with primary key.  AssignmentTypeCode is unique and is a single character.
            // Default suggestion is A..Z, 0..9
            char code = 'A';

            if (FMainDS.PtAssignmentType.Rows.Find(new object[] { code }) != null)
            {
                while (FMainDS.PtAssignmentType.Rows.Find(new object[] { code }) != null)
                {
                    if (code == 'Z')
                    {
                        code = '0';
                    }
                    else
                    {
                        code++;
                    }
                }
            }

            ARow.AssignmentTypeCode = code.ToString();
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtAssignmentType();
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

        private void ValidateDataDetailsManual(PtAssignmentTypeRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateJobAssignmentTypes(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1, 2, 3, 4 },
                new int[]
                {
                    PtAssignmentTypeTable.ColumnAssignmentTypeCodeId,
                    PtAssignmentTypeTable.ColumnAssignmentCodeDescrId,
                    PtAssignmentTypeTable.ColumnUnassignableFlagId,
                    PtAssignmentTypeTable.ColumnUnassignableDateId,
                    PtAssignmentTypeTable.ColumnDeletableFlagId
                });
        }
    }
}