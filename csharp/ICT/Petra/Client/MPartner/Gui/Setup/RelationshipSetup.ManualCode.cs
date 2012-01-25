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
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmRelationshipSetup
    {
        private void NewRowManual(ref PRelationRow ARow)
        {
            // Deal with the primary key - we need a unique relation name
            string newName = Catalog.GetString("NEWRELATION");
            Int32 countNewDetail = 0;

            if (FMainDS.PRelation.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PRelation.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.RelationName = newName;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            // Deal with the possibility that we have no relationship categories set up for the Non-Null field for this table
            Type DataTableType;

            // Load Data
            PRelationCategoryTable allCategories = new PRelationCategoryTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("RelationCategoryList", String.Empty, null, out DataTableType);

            allCategories.Merge(CacheDT);

            if (allCategories.Rows.Count == 0)
            {
                string Msg =
                    "Before you attempt to save a New Relationship you should return to the Partner Setup screen and create a new 'Relationship Category'.";
                MessageBox.Show(Msg, "Open Petra Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CreateNewPRelation();
        }
    }
}