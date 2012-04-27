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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// The TPGE_PartnerStatusComboBox class is a 'value editor' that provides a
    /// ComboBox-like Editor for Partner Statuses in a PropertyGrid.
    ///
    /// The entries for the Partner Statuses list come from a DataTable in the
    /// Client DataCache and exclude some statuses.
    ///
    /// @Comment Uses the TUITE_ListBox class to actually draw the DropDown.
    /// </summary>
    public class TPGE_PartnerStatusComboBox : TUITE_ListBox
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TPGE_PartnerStatusComboBox()
            : base()
        {
            ArrayList ListItemsArray;
            DataTable DataCache_PartnerStatusListTable;

            System.Int16 PartnerStatusCounter;
            string StatusCode;
            ListItemsArray = new ArrayList();
            ListItemsArray.Add("ALL");
            DataCache_PartnerStatusListTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerStatusList);

            for (PartnerStatusCounter = 0; PartnerStatusCounter <= DataCache_PartnerStatusListTable.Rows.Count - 1; PartnerStatusCounter += 1)
            {
                StatusCode = DataCache_PartnerStatusListTable.Rows[PartnerStatusCounter]["p_status_code_c"].ToString();

                if ((StatusCode != "MERGED") && (StatusCode != "DELETED") && (StatusCode != "PRIVATE"))
                {
                    ListItemsArray.Add(StatusCode);
                }
            }

            DrawDropDown(ListItemsArray);
        }
    }
}