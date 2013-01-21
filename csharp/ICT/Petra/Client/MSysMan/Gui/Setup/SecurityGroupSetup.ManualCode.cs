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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.Interfaces.MCommon;

namespace Ict.Petra.Client.MSysMan.Gui.Setup
{
    public partial class TFrmSecurityGroupSetup
    {
        private void NewRowManual(ref SGroupRow ARow)
        {
            // Deal with primary key.  GroupId is unique.  UnitKey is always 0
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.SGroup.Rows.Find(new object[] { newName, 0 }) != null)
            {
                while (FMainDS.SGroup.Rows.Find(new object[] { newName + countNewDetail.ToString(), 0 }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.GroupId = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewSGroup();
        }
    }
}