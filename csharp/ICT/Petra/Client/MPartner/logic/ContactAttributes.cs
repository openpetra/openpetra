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
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// </summary>
    public static class ContactAttributesLogic
    {
        /// <summary>
        /// Setups the contact attributes grid (used on the following screens: Contact Log tab, Contacts with Partners and Extract by Contact Log.
        /// </summary>
        /// <param name="AGrid">Grid to be set up</param>
        /// <param name="AAttributes">Attributes to be included in the grid</param>
        /// <param name="AIncludeDescription">Include columns that display the attribute descriptions</param>
        /// <param name="AContactLogIDFilter">Filter grid to only show rows for given contact log if</param>
        public static DataView SetupContactAttributesGrid(ref TSgrdDataGridPaged AGrid,
            DataTable AAttributes,
            bool AIncludeDescription,
            Int64 AContactLogIDFilter = -1)
        {
            DataView ContactAttributeTableDV =
                TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeList).DefaultView;

            ContactAttributeTableDV.Sort = PContactAttributeTable.GetContactAttributeCodeDBName() + " ASC";

            DataView ContactAttributeDetailTableDV =
                TDataCache.TMPartner.GetCacheableMailingTable(TCacheableMailingTablesEnum.ContactAttributeDetailList).DefaultView;
            ContactAttributeDetailTableDV.Sort = PContactAttributeDetailTable.GetContactAttributeCodeDBName() + " ASC, " +
                                                 PContactAttributeDetailTable.GetContactAttrDetailCodeDBName() + " ASC";

            DataTable DT = new PPartnerContactAttributeTable();

            if ((AAttributes != null) && (AAttributes.Rows.Count > 0))
            {
                DT = AAttributes.Copy();
            }

            if (AIncludeDescription)
            {
                DT.Columns.Add("AttributeDescription", System.Type.GetType("System.String"));
                DT.Columns.Add("AttributeDetailDescription", System.Type.GetType("System.String"));

                // add descriptions to new table
                foreach (DataRow Row in DT.Rows)
                {
                    if (Row.RowState != DataRowState.Deleted)
                    {
                        Row["AttributeDescription"] = GetContactAttributeDesciption(
                            Row[PPartnerContactAttributeTable.GetContactAttributeCodeDBName()].ToString(), ContactAttributeTableDV);
                        Row["AttributeDetailDescription"] = GetContactAttributeDetailDesciption(
                            Row[PPartnerContactAttributeTable.GetContactAttributeCodeDBName()].ToString(),
                            Row[PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName()].ToString(), ContactAttributeDetailTableDV);
                    }
                }
            }

            AGrid.Columns.Clear();
            AGrid.AddTextColumn("Attribute Code", DT.Columns[PPartnerContactAttributeTable.GetContactAttributeCodeDBName()]);

            if (AIncludeDescription)
            {
                AGrid.AddTextColumn("Description", DT.Columns["AttributeDescription"]);
            }

            AGrid.AddTextColumn("Detail Code", DT.Columns[PPartnerContactAttributeTable.GetContactAttrDetailCodeDBName()]);

            if (AIncludeDescription)
            {
                AGrid.AddTextColumn("Description", DT.Columns["AttributeDetailDescription"]);
            }

            DataView GridTableDV = DT.DefaultView;
            GridTableDV.AllowNew = false;
            GridTableDV.AllowEdit = false;
            GridTableDV.AllowDelete = false;

            if (AContactLogIDFilter != -1)
            {
                GridTableDV.RowFilter = PPartnerContactAttributeTable.GetContactIdDBName() + " = " + AContactLogIDFilter;
            }

            // DataBind the DataGrid
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(GridTableDV);

            AGrid.AutoResizeGrid();

            return GridTableDV;
        }

        /// <summary>
        /// Gets a contact attribute's desciption.
        /// </summary>
        /// <param name="AContactAttributeCode">The contact attribute's Code</param>
        /// <param name="AContactAttributeTableDV">Table containing all ContactAttributes</param>
        /// <returns></returns>
        public static string GetContactAttributeDesciption(string AContactAttributeCode, DataView AContactAttributeTableDV)
        {
            string ReturnValue = "";

            int TypeDescriptionInCachePosition = AContactAttributeTableDV.Find(AContactAttributeCode);

            if (TypeDescriptionInCachePosition != -1)
            {
                ReturnValue = AContactAttributeTableDV
                              [TypeDescriptionInCachePosition][PContactAttributeTable.GetContactAttributeDescrDBName()].ToString();

                // If this attribute is inactive, show it.
                if (!Convert.ToBoolean(AContactAttributeTableDV[TypeDescriptionInCachePosition][PContactAttributeTable.GetActiveDBName()]))
                {
                    ReturnValue = ReturnValue + MCommonResourcestrings.StrGenericInactiveCode;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets a contact attribute detail's desciption.
        /// </summary>
        /// <param name="AContactAttributeCode">The contact attribute's Code</param>
        /// <param name="AContactAttributeDetailCode">The contact attribute detail's Code</param>
        /// <param name="AContactAttributeDetailTableDV">Table containing all ContactAttributeDetails</param>
        /// <returns></returns>
        public static string GetContactAttributeDetailDesciption(string AContactAttributeCode,
            string AContactAttributeDetailCode,
            DataView AContactAttributeDetailTableDV)
        {
            string ReturnValue = "";

            int TypeDescriptionInCachePosition = AContactAttributeDetailTableDV.Find(new object[] { AContactAttributeCode,
                                                                                                    AContactAttributeDetailCode });

            if (TypeDescriptionInCachePosition != -1)
            {
                ReturnValue = AContactAttributeDetailTableDV
                              [TypeDescriptionInCachePosition][PContactAttributeDetailTable.GetContactAttrDetailDescrDBName()].ToString();

                // If this Type is inactive, show it.
                if (!Convert.ToBoolean(AContactAttributeDetailTableDV[TypeDescriptionInCachePosition][PContactAttributeDetailTable.GetActiveDBName()]))
                {
                    ReturnValue = ReturnValue + MCommonResourcestrings.StrGenericInactiveCode;
                }
            }

            return ReturnValue;
        }
    }
}