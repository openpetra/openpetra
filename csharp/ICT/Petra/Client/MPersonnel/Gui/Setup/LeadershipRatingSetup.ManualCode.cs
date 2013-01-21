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
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon.Validation;

namespace Ict.Petra.Client.MPersonnel.Gui.Setup
{
    public partial class TFrmLeadershipRatingSetup
    {
        private void NewRowManual(ref PtLeadershipRatingRow ARow)
        {
            // Deal with primary key.  Code is unique and is a single character.
            // Default suggestion is 0..9, A..Z
            char code = '0';

            if (FMainDS.PtLeadershipRating.Rows.Find(new object[] { code }) != null)
            {
                while (FMainDS.PtLeadershipRating.Rows.Find(new object[] { code }) != null)
                {
                    if (code == '9')
                    {
                        code = 'A';
                    }
                    else
                    {
                        code++;
                    }
                }
            }

            ARow.Code = code.ToString();
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPtLeadershipRating();
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

        private void ValidateDataDetailsManual(PtLeadershipRatingRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedValidation_CacheableDataTables.ValidateLeadershipRating(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }
    }
}