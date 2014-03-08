//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmPositionSetup
    {
        private void RunOnceOnActivationManual()
        {
            chkDetailDeletableFlag.Enabled = false;
        }

        private void NewRowManual(ref PtPositionRow ARow)
        {
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            // Note from AlanP on 8 March 2014:
            // The PK needs to include a Unit Type Code but we do not have an interface for that (yet?).
            // Multiple codes exist in the standard database.
            // But all positions use the 'O' code, which means 'Other'
            // (see Mantis case 2793)

            string positionScope = "O";

            if (FMainDS.PtPosition.Rows.Find(new object[] { newName, positionScope }) != null)
            {
                while (FMainDS.PtPosition.Rows.Find(new object[] { newName + countNewDetail.ToString(), positionScope }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.PositionName = newName;
            ARow.PositionScope = positionScope;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtPosition();
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

        private void ValidateDataDetailsManual(PtPositionRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidatePositions(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }
    }
}