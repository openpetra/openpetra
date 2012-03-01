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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPublicationCostSetup
    {
        private void NewRowManual(ref PPublicationCostRow ARow)
        {
            // Deal with primary key.  It is combination of a code and a effective date
            // Start by finding a code that does not have today's date
            Type DataTableType;

            // Load Data
            PPublicationTable allPublications = new PPublicationTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("PublicationList", String.Empty, null, out DataTableType);

            allPublications.Merge(CacheDT);

            bool bFound = false;

            for (int i = 0; i < allPublications.Rows.Count; i++)
            {
                string tryCode = allPublications.Rows[i][0].ToString();

                if (FMainDS.PPublicationCost.Rows.Find(new object[] { tryCode, DateTime.Today }) == null)
                {
                    ARow.PublicationCode = tryCode;
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                // use the first Publication and the first unused date
                string tryCode = allPublications.Rows[0][0].ToString();

                for (int i = 1;; i++)
                {
                    DateTime tryDate = DateTime.Today.AddDays(i);

                    if (FMainDS.PPublicationCost.Rows.Find(new object[] { tryCode, tryDate }) == null)
                    {
                        ARow.PublicationCode = tryCode;
                        ARow.DateEffective = tryDate;
                        break;
                    }
                }
            }
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPublicationCost();
        }
    }
}