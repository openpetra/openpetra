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

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmAddresseeTypeSetup
    {
        private void NewRowManual(ref PAddresseeTypeRow ARow)
        {
            string newName = Catalog.GetString("NEWTYPE");
            Int32 countNewDetail = 0;

            if (FMainDS.PAddresseeType.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PAddresseeType.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.AddresseeTypeCode = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPAddresseeType();
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1, 2 },
                new int[]
                {
                    PAddresseeTypeTable.ColumnAddresseeTypeCodeId,
                    PAddresseeTypeTable.ColumnDescriptionId,
                    PAddresseeTypeTable.ColumnDeletableId,
                });
        }
    }
}