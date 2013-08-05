//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPostcodeRangeSetup
    {
        private void NewRowManual(ref PPostcodeRangeRow ARow)
        {
            string NewName = Catalog.GetString("NEWRANGE");
            int CountNewDetail = 0;

            if (FMainDS.PPostcodeRange.Rows.Find(new object[] { NewName }) != null)
            {
                while (FMainDS.PPostcodeRange.Rows.Find(new object[] { NewName + CountNewDetail.ToString() }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.Range = NewName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPostcodeRange();
        }

        private void ValidateDataDetailsManual(PPostcodeRangeRow ARow)
        {
            /*TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
             *
             * TSharedPartnerValidation_Partner.ValidatePostcodeRangesSetup(this, ARow, ref VerificationResultCollection,
             *  FPetraUtilsObject.ValidationControlsDict);*/
        }
    }
}