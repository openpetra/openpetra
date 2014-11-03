//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Specialized;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MCommon.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Server.MPartner.Partner.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MPartner.Partner sub-namespace
    /// that can be cached on the Client side.
    /// </summary>
    public partial class TPartnerCacheable
    {
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Wrapper for other GetCacheableTable method
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <returns>DataTable</returns>
        public DataTable GetCacheableTable(TCacheablePartnerTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
        }

        /// <summary>
        /// Overload of <see cref="M:GetCacheableTable(TCacheablePartnerTablesEnum, string, bool, out System.Type)" />. See description there.
        /// </summary>
        /// <remarks>Can be used with Delegate TGetCacheableDataTableFromCache.</remarks>
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>The specified Cacheable DataTable is returned if the string matches a Cacheable DataTable,
        /// otherwise <see cref="String.Empty" />.</returns>
        public DataTable GetCacheableTable(string ACacheableTable, out System.Type AType)
        {
            return GetCacheableTable((TCacheablePartnerTablesEnum)Enum.Parse(typeof(TCacheablePartnerTablesEnum), ACacheableTable),
                String.Empty, false, out AType);
        }

        private DataTable GetCountyListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            return DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + PLocationTable.GetCountryCodeDBName() + ", " +
                PLocationTable.GetCountyDBName() + " FROM PUB." +
                PLocationTable.GetTableDBName(), ATableName, AReadTransaction);
        }

        private DataTable GetFoundationOwnerListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            // Used in Foundation Details screen.
            SUserTable TmpUserTable = new SUserTable();

            TmpUserTable = (SUserTable)DBAccess.GDBAccessObj.SelectDT(TmpUserTable, "SELECT " + SUserTable.GetPartnerKeyDBName() + ',' +
                SUserTable.GetUserIdDBName() + ',' +
                SUserTable.GetFirstNameDBName() + ',' +
                SUserTable.GetLastNameDBName() + ' ' +
                "FROM PUB_" + SUserTable.GetTableDBName() + ' ' +
                "WHERE " + SUserTable.GetPartnerKeyDBName() + " <> 0 " +
                "AND " + SUserTable.GetUserIdDBName() +
                " IN (SELECT " + SUserModuleAccessPermissionTable.GetUserIdDBName() + ' ' +
                "FROM PUB_" + SUserModuleAccessPermissionTable.GetTableDBName() + ' ' +
                "WHERE " + SUserModuleAccessPermissionTable.GetModuleIdDBName() +
                " = 'DEVUSER')" + "AND " + SUserTable.GetRetiredDBName() +
                " = FALSE", AReadTransaction, null, -1, -1);
            SUserRow EmptyDR = TmpUserTable.NewRowTyped(false);
            EmptyDR.PartnerKey = 0;
            EmptyDR.UserId = "";
            TmpUserTable.Rows.InsertAt(EmptyDR, 0);
            return TmpUserTable;
        }

        private DataTable GetInstalledSitesListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            // Used eg. in New Partner Dialog.
            StringCollection RequiredColumns = new StringCollection();

            RequiredColumns.Add(PPartnerLedgerTable.GetPartnerKeyDBName());
            PPartnerLedgerTable TmpInstalledSitesDT = PPartnerLedgerAccess.LoadAll(RequiredColumns, AReadTransaction, null, 0, 0);

            if (TmpInstalledSitesDT.Rows.Count != 0)
            {
                TmpInstalledSitesDT.Columns.Remove(PPartnerLedgerTable.GetLastPartnerIdDBName());
                TmpInstalledSitesDT.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), System.Type.GetType("System.String"));
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());

                for (int Counter = 0; Counter <= TmpInstalledSitesDT.Rows.Count - 1; Counter += 1)
                {
                    PPartnerTable PartnerDT = PPartnerAccess.LoadByPrimaryKey(
                        TmpInstalledSitesDT[Counter].PartnerKey,
                        RequiredColumns,
                        AReadTransaction,
                        null,
                        0,
                        0);
                    TmpInstalledSitesDT[Counter][PPartnerTable.GetPartnerShortNameDBName()] = PartnerDT[0].PartnerShortName;
                }
            }

            return TmpInstalledSitesDT;
        }

        private DataTable GetCountryListFromExistingLocationsTable(TDBTransaction AReadTransaction, string ATableName)
        {
            // Used eg. in Report Gift Data Export for finding donors.
            return DBAccess.GDBAccessObj.SelectDT("SELECT DISTINCT " + "PUB." + PCountryTable.GetTableDBName() + '.' +
                PCountryTable.GetCountryCodeDBName() + ", " +
                PCountryTable.GetCountryNameDBName() + " FROM PUB." +
                PCountryTable.GetTableDBName() + " c, PUB." +
                PLocationTable.GetTableDBName() + " l " +
                " WHERE " + PLocationTable.GetCountyDBName() + " IS NOT NULL AND NOT " +
                PLocationTable.GetCountyDBName() + " = ''" +
                " AND c." + PCountryTable.GetCountryCodeDBName() + " = l." +
                PLocationTable.GetCountryCodeDBName(), ATableName, AReadTransaction);
        }

        private DataTable GetDataLabelsForPartnerClassesListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            const string PARTNERCLASSCOL = "PartnerClass";
            const string DLAVAILCOL = "DataLabelsAvailable";

            DataTable TmpTable;
            DataRow NewDR;
            TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;

            // Create our custom Cacheable DataTable on-the-fly
            TmpTable = new DataTable(ATableName);
            DataColumn PKColumn = new DataColumn(PARTNERCLASSCOL, System.Type.GetType("System.String"));
            TmpTable.Columns.Add(PKColumn);
            TmpTable.Columns.Add(new DataColumn(DLAVAILCOL, System.Type.GetType("System.Boolean")));
            TmpTable.PrimaryKey = new DataColumn[] {
                PKColumn
            };

            /*
             * Create an Instance of TOfficeSpecificDataLabelsUIConnector - PartnerKey and DataLabelUse are not important here
             * because we only call Method 'CountLabelUse', which doesn't rely on any of them.
             */
            OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(0,
                TOfficeSpecificDataLabelUseEnum.Family);

            // DataLabels available for PERSONs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.PERSON);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for FAMILYs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.FAMILY);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for CHURCHes?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.CHURCH);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for ORGANISATIONs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for UNITs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.UNIT);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for BANKs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.BANK);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            // DataLabels available for VENUEs?
            NewDR = TmpTable.NewRow();
            NewDR[PARTNERCLASSCOL] = SharedTypes.PartnerClassEnumToString(TPartnerClass.VENUE);
            NewDR[DLAVAILCOL] =
                (OfficeSpecificDataLabelsUIConnector.CountLabelUse(NewDR[PARTNERCLASSCOL].ToString(), AReadTransaction) != 0);
            TmpTable.Rows.Add(NewDR);

            return TmpTable;
        }

        private DataTable GetContactCategoryListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            PPartnerAttributeCategoryRow template = new PPartnerAttributeCategoryTable().NewRowTyped(false);

            template.PartnerContactCategory = true;
            template.SystemCategory = false;

            return PPartnerAttributeCategoryAccess.LoadUsingTemplate(template, null, null, AReadTransaction,
                StringHelper.InitStrArr(new String[] { "ORDER BY", PPartnerAttributeCategoryTable.GetIndexDBName() + " ASC" }), 0, 0);
        }

        private DataTable GetContactTypeListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            PPartnerAttributeCategoryRow template = new PPartnerAttributeCategoryTable().NewRowTyped(false);

            template.PartnerContactCategory = true;
            template.SystemCategory = false;

            return PPartnerAttributeTypeAccess.LoadViaPPartnerAttributeCategoryTemplate(template, null, null, AReadTransaction,
                StringHelper.InitStrArr(new String[] { "ORDER BY", PPartnerAttributeTypeTable.GetTableDBName() + "." +
                                                       PPartnerAttributeTypeTable.GetIndexDBName() + " ASC" }), 0, 0);
        }

        private DataTable GetPartnerAttributeSystemCategoryTypeListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            PPartnerAttributeCategoryRow template = new PPartnerAttributeCategoryTable().NewRowTyped(false);

            template.SystemCategory = true;

            return PPartnerAttributeTypeAccess.LoadViaPPartnerAttributeCategoryTemplate(template, null, null, AReadTransaction,
                StringHelper.InitStrArr(new String[] { "ORDER BY", PPartnerAttributeTypeTable.GetTableDBName() + "." +
                                                       PPartnerAttributeTypeTable.GetIndexDBName() + " ASC" }), 0, 0);
        }

        private DataTable GetPartnerAttributeSystemCategoryListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            PPartnerAttributeCategoryRow template = new PPartnerAttributeCategoryTable().NewRowTyped(false);

            template.SystemCategory = true;

            return PPartnerAttributeCategoryAccess.LoadUsingTemplate(template, null, null, AReadTransaction);
        }
    }
}