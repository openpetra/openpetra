// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//   awebster
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Owf.Controls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using System.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByRelationship
    {
        private void InitializeManualCode()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = "p_relation_description_c";//PTypeTable.GetTypeDescriptionDBName();
            string ValueMember = "p_relation_name_c";//PTypeTable.GetTypeCodeDBName();

            DataTable table = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList);
            DataView view = new DataView(table);

            view.Sort = ValueMember;

            var newTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            newTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
            
            clbIncludeRelationships.SpecialKeys = (SourceGrid.GridSpecialKeys)(
                SourceGrid.GridSpecialKeys.Arrows |
                SourceGrid.GridSpecialKeys.PageDownUp |
                SourceGrid.GridSpecialKeys.Enter |
                SourceGrid.GridSpecialKeys.Escape |
                SourceGrid.GridSpecialKeys.Control | 
                SourceGrid.GridSpecialKeys.Shift);
            clbIncludeRelationships.Columns.Clear();
            clbIncludeRelationships.AddCheckBoxColumn("", newTable.Columns[CheckedMember], 17, false);
            clbIncludeRelationships.AddTextColumn(Catalog.GetString("Type Code"), newTable.Columns[ValueMember], 100);
            clbIncludeRelationships.AddTextColumn(Catalog.GetString("Type Description"), newTable.Columns[DisplayMember], 320);
            clbIncludeRelationships.DataBindGrid(newTable, ValueMember, CheckedMember, ValueMember, false, true, false);
        }

    }
}