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
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPartnerStatusSetup
    {
        private void NewRowManual(ref PPartnerStatusRow ARow)
        {
            string newName = Catalog.GetString("NEWSTATUS");
            Int32 countNewDetail = 0;

            if (FMainDS.PPartnerStatus.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PPartnerStatus.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.StatusCode = newName;

            // New rows normally don't contain Partner Statuses that denote 'Active' Partners
            ARow.PartnerIsActive = false;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPartnerStatus();
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    PPartnerStatusTable.ColumnPartnerStatusDescriptionId,
                    PPartnerStatusTable.ColumnPartnerStatusDescriptionId,
                    PPartnerStatusTable.ColumnDeletableId
                });
        }
    }
}