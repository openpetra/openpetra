/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
namespace Ict.Petra.Shared.MPersonnel.Units.Data.Access
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    using Ict.Petra.Shared;
    using Ict.Petra.Shared.MPersonnel.Units.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MPersonnel.Personnel.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MSysMan.Data;

    /// This is a listing of the different position which exist within our organisation, e.g. Field Leader, Book Keeper, Computer support.
    public class PtPositionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PtPosition";

        /// original table name in database
        public const string DBTABLENAME = "pt_position";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position")
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtPositionTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PtPositionTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtPositionTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[0].Value = ((object)(APositionName));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(APositionScope));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position WHERE pt_position_name_c = ? AND pt_position_scope_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APositionName, APositionScope, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APositionName, APositionScope, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtPositionTable AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APositionName, APositionScope, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtPositionTable AData, String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APositionName, APositionScope, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtPositionTable AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APositionName, APositionScope, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position")
                            + GenerateWhereClause(PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtPositionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtPositionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, PtPositionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, PtPositionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position")
                            + GenerateWhereClause(PtPositionTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_position", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[0].Value = ((object)(APositionName));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(APositionScope));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_position WHERE pt_position_name_c = ? AND pt_position_scope_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position" + GenerateWhereClause(PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position" + GenerateWhereClause(PtPositionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet ADataSet, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AUnitTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position WHERE pt_position_scope_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet AData, String AUnitTypeCode, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(AData, AUnitTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(AData, AUnitTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(out PtPositionTable AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitType(FillDataSet, AUnitTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitType(out PtPositionTable AData, String AUnitTypeCode, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(out AData, AUnitTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(out PtPositionTable AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(out AData, AUnitTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet ADataSet, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position, PUB_u_unit_type WHERE " +
                            "PUB_pt_position.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c")
                            + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, UUnitTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, UUnitTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, UUnitTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, UUnitTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position, PUB_u_unit_type WHERE " +
                            "PUB_pt_position.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c")
                            + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUUnitType(String AUnitTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AUnitTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_position WHERE pt_position_scope_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position, PUB_u_unit_type WHERE " +
                "PUB_pt_position.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c" + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UUnitTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position, PUB_u_unit_type WHERE " +
                "PUB_pt_position.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c" +
                GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UUnitTypeTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pt_position", AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position, PUB_um_job WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PtPositionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out PtPositionTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PtPositionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, new string[] {
                            "pt_position_name_c", "pt_position_scope_c"}) + " FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PtPositionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtPositionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PtPositionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_position, PUB_um_job WHERE " +
                        "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                        "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_um_job", PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                        "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_um_job", PtPositionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[0].Value = ((object)(APositionName));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(APositionScope));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pt_position WHERE pt_position_name_c = ? AND pt_position_scope_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_position" + GenerateWhereClause(PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_position" +
                GenerateWhereClause(PtPositionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PtPositionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("pt_position", PtPositionTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pt_position", PtPositionTable.GetColumnStringList(), PtPositionTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pt_position", PtPositionTable.GetColumnStringList(), PtPositionTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PtPosition", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// This table contains information concerning jobs within the unit.
    public class UmJobAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmJob";

        /// original table name in database
        public const string DBTABLENAME = "um_job";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmJobTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job")
                            + GenerateWhereClause(UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job")
                            + GenerateWhereClause(UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job" + GenerateWhereClause(UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job" + GenerateWhereClause(UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job WHERE pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job WHERE pm_unit_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_p_unit WHERE " +
                "PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_p_unit WHERE " +
                "PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtPosition(DataSet ADataSet, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[0].Value = ((object)(APositionName));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(APositionScope));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job WHERE pt_position_name_c = ? AND pt_position_scope_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtPosition(DataSet AData, String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            LoadViaPtPosition(AData, APositionName, APositionScope, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPosition(DataSet AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPosition(AData, APositionName, APositionScope, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPosition(out UmJobTable AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtPosition(FillDataSet, APositionName, APositionScope, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtPosition(out UmJobTable AData, String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            LoadViaPtPosition(out AData, APositionName, APositionScope, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPosition(out UmJobTable AData, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPosition(out AData, APositionName, APositionScope, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet ADataSet, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_pt_position WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c")
                            + GenerateWhereClauseLong("PUB_pt_position", PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet AData, PtPositionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet AData, PtPositionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtPositionTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, PtPositionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, PtPositionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, PtPositionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_pt_position WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c")
                            + GenerateWhereClauseLong("PUB_pt_position", PtPositionTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtPositionTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtPositionTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtPositionTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtPosition(String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[0].Value = ((object)(APositionName));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(APositionScope));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job WHERE pt_position_name_c = ? AND pt_position_scope_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtPositionTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_pt_position WHERE " +
                "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c" + GenerateWhereClauseLong("PUB_pt_position", PtPositionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtPositionTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtPositionTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_pt_position WHERE " +
                "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c" +
                GenerateWhereClauseLong("PUB_pt_position", PtPositionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtPositionTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet ADataSet, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AUnitTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job WHERE pt_position_scope_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet AData, String AUnitTypeCode, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(AData, AUnitTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(AData, AUnitTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(out UmJobTable AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitType(FillDataSet, AUnitTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitType(out UmJobTable AData, String AUnitTypeCode, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(out AData, AUnitTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitType(out UmJobTable AData, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitType(out AData, AUnitTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet ADataSet, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_u_unit_type WHERE " +
                            "PUB_um_job.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c")
                            + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, UUnitTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, UUnitTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, UUnitTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, UUnitTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_u_unit_type WHERE " +
                            "PUB_um_job.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c")
                            + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUUnitTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUUnitTypeTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUUnitTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUUnitType(String AUnitTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AUnitTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job WHERE pt_position_scope_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_u_unit_type WHERE " +
                "PUB_um_job.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c" + GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UUnitTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_u_unit_type WHERE " +
                "PUB_um_job.pt_position_scope_c = PUB_u_unit_type.u_unit_type_code_c" +
                GenerateWhereClauseLong("PUB_u_unit_type", UUnitTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UUnitTypeTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_requirement WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityArea(FillDataSet, AAbilityAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobTable AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPtAbilityArea(String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_requirement WHERE " +
                        "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                        "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_requirement", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtAbilityAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                        "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_requirement", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguage(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobTable AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_language WHERE " +
                        "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                        "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_um_job_language", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PLanguageTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                        "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_um_job_language", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtQualificationArea(DataSet ADataSet, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AQualificationAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_qualification WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet AData, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(AData, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(AData, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobTable AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationArea(FillDataSet, AQualificationAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobTable AData, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(out AData, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobTable AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(out AData, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet ADataSet, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, PtQualificationAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, PtQualificationAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, PtQualificationAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, PtQualificationAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPtQualificationArea(String AQualificationAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AQualificationAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_qualification WHERE " +
                        "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                        "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_qualification", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtQualificationAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                        "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_qualification", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_vision WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionArea(FillDataSet, AVisionAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobTable AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPtVisionArea(String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_vision WHERE " +
                        "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                        "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_vision", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtVisionAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                        "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_vision", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_pm_job_assignment WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(out UmJobTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPartner(out UmJobTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(out UmJobTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_pm_job_assignment WHERE " +
                        "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                        "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_job_assignment", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                        "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_job_assignment", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_s_job_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = ? AND PUB_s_job_group.s_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroup(out UmJobTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaSGroup(out UmJobTable AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroup(out UmJobTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n")
                            + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"}) + " FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n")
                            + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroupTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(out UmJobTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job, PUB_s_job_group WHERE " +
                        "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = ? AND PUB_s_job_group.s_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                        "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_job_group", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SGroupTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                        "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_job_group", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_job WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job" + GenerateWhereClause(UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job" +
                GenerateWhereClause(UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmJobTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        ((UmJobRow)(TheRow)).JobKey = ((Int32)(DBAccess.GDBAccessObj.GetNextSequenceValue("seq_job", ATransaction)));
                        TTypedDataAccess.InsertRow("um_job", UmJobTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_job", UmJobTable.GetColumnStringList(), UmJobTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_job", UmJobTable.GetColumnStringList(), UmJobTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmJob", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Lists abilities and experience required for various positions.
    public class UmJobRequirementAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmJobRequirement";

        /// original table name in database
        public const string DBTABLENAME = "um_job_requirement";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobRequirementTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmJobRequirementTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobRequirementTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AAbilityAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_ability_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement")
                            + GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobRequirementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobRequirementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, UmJobRequirementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, UmJobRequirementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement")
                            + GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AAbilityAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_ability_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement" + GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement" + GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobRequirementTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_um_job WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_um_job WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_um_job WHERE " +
                "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i" + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UmJobTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_um_job WHERE " +
                "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i" +
                GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement WHERE pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobRequirementTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobRequirementTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobRequirementTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_p_unit WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_p_unit WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement WHERE pm_unit_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_p_unit WHERE " +
                "PUB_um_job_requirement.pm_unit_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_p_unit WHERE " +
                "PUB_um_job_requirement.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement WHERE pt_ability_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobRequirementTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityArea(FillDataSet, AAbilityAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobRequirementTable AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmJobRequirementTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtAbilityArea(String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement WHERE pt_ability_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                "PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtAbilityAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                "PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" +
                GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtAbilityAreaTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet ADataSet, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AAbilityLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement WHERE pt_ability_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet AData, Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(AData, AAbilityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(AData, AAbilityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmJobRequirementTable AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevel(FillDataSet, AAbilityLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmJobRequirementTable AData, Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(out AData, AAbilityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmJobRequirementTable AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(out AData, AAbilityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet ADataSet, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_pt_ability_level WHERE " +
                            "PUB_um_job_requirement.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i")
                            + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, PtAbilityLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, PtAbilityLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, PtAbilityLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, PtAbilityLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_requirement", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_ability_area_name_c"}) + " FROM PUB_um_job_requirement, PUB_pt_ability_level WHERE " +
                            "PUB_um_job_requirement.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i")
                            + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobRequirementTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobRequirementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmJobRequirementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtAbilityLevel(Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AAbilityLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_requirement WHERE pt_ability_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_pt_ability_level WHERE " +
                "PUB_um_job_requirement.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i" + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtAbilityLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement, PUB_pt_ability_level WHERE " +
                "PUB_um_job_requirement.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i" +
                GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtAbilityLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AAbilityAreaName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_job_requirement WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_ability_area_name_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_requirement" + GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_requirement" +
                GenerateWhereClause(UmJobRequirementTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobRequirementTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmJobRequirementTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_job_requirement", UmJobRequirementTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_job_requirement", UmJobRequirementTable.GetColumnStringList(), UmJobRequirementTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_job_requirement", UmJobRequirementTable.GetColumnStringList(), UmJobRequirementTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmJobRequirement", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Language used on this job.
    public class UmJobLanguageAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmJobLanguage";

        /// original table name in database
        public const string DBTABLENAME = "um_job_language";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmJobLanguageTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[4].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language")
                            + GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, UmJobLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, UmJobLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language")
                            + GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[4].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND p_language_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language" + GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language" + GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_um_job WHERE " +
                "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i" + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UmJobTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_um_job WHERE " +
                "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i" +
                GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language WHERE pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobLanguageTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_p_unit WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_p_unit WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language WHERE pm_unit_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_p_unit WHERE " +
                "PUB_um_job_language.pm_unit_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_p_unit WHERE " +
                "PUB_um_job_language.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language WHERE p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguage(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobLanguageTable AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmJobLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language WHERE p_language_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_p_language WHERE " +
                "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c" + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PLanguageTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_p_language WHERE " +
                "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet ADataSet, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALanguageLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language WHERE pt_language_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet AData, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(AData, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(AData, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmJobLanguageTable AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevel(FillDataSet, ALanguageLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmJobLanguageTable AData, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(out AData, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmJobLanguageTable AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(out AData, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet ADataSet, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_pt_language_level WHERE " +
                            "PUB_um_job_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i")
                            + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, PtLanguageLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, PtLanguageLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, PtLanguageLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_language", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "p_language_code_c"}) + " FROM PUB_um_job_language, PUB_pt_language_level WHERE " +
                            "PUB_um_job_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i")
                            + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmJobLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtLanguageLevel(Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALanguageLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_language WHERE pt_language_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_pt_language_level WHERE " +
                "PUB_um_job_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i" + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtLanguageLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language, PUB_pt_language_level WHERE " +
                "PUB_um_job_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i" +
                GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtLanguageLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[4].Value = ((object)(ALanguageCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_job_language WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND p_language_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_language" + GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_language" +
                GenerateWhereClause(UmJobLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobLanguageTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmJobLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_job_language", UmJobLanguageTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_job_language", UmJobLanguageTable.GetColumnStringList(), UmJobLanguageTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_job_language", UmJobLanguageTable.GetColumnStringList(), UmJobLanguageTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmJobLanguage", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details of qualifications required for individual jobs.
    public class UmJobQualificationAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmJobQualification";

        /// original table name in database
        public const string DBTABLENAME = "um_job_qualification";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobQualificationTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmJobQualificationTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobQualificationTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AQualificationAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_qualification_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification")
                            + GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobQualificationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobQualificationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, UmJobQualificationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, UmJobQualificationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification")
                            + GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AQualificationAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_qualification_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification" + GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification" + GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobQualificationTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_um_job WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_um_job WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_um_job WHERE " +
                "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i" + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UmJobTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_um_job WHERE " +
                "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i" +
                GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification WHERE pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobQualificationTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobQualificationTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobQualificationTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_p_unit WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_p_unit WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification WHERE pm_unit_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_p_unit WHERE " +
                "PUB_um_job_qualification.pm_unit_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_p_unit WHERE " +
                "PUB_um_job_qualification.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet ADataSet, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AQualificationAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification WHERE pt_qualification_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet AData, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(AData, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(AData, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobQualificationTable AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationArea(FillDataSet, AQualificationAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobQualificationTable AData, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(out AData, AQualificationAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(out UmJobQualificationTable AData, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationArea(out AData, AQualificationAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet ADataSet, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, PtQualificationAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, PtQualificationAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, PtQualificationAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, PtQualificationAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationAreaTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtQualificationArea(String AQualificationAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AQualificationAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification WHERE pt_qualification_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                "PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c" + GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtQualificationAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                "PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c" +
                GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtQualificationAreaTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(DataSet ADataSet, Int32 AQualificationLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AQualificationLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification WHERE pt_qualification_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(DataSet AData, Int32 AQualificationLevel, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevel(AData, AQualificationLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(DataSet AData, Int32 AQualificationLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevel(AData, AQualificationLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(out UmJobQualificationTable AData, Int32 AQualificationLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationLevel(FillDataSet, AQualificationLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(out UmJobQualificationTable AData, Int32 AQualificationLevel, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevel(out AData, AQualificationLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(out UmJobQualificationTable AData, Int32 AQualificationLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevel(out AData, AQualificationLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet ADataSet, PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_pt_qualification_level WHERE " +
                            "PUB_um_job_qualification.pt_qualification_level_i = PUB_pt_qualification_level.pt_qualification_level_i")
                            + GenerateWhereClauseLong("PUB_pt_qualification_level", PtQualificationLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet AData, PtQualificationLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet AData, PtQualificationLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, PtQualificationLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, PtQualificationLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_qualification", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_qualification_area_name_c"}) + " FROM PUB_um_job_qualification, PUB_pt_qualification_level WHERE " +
                            "PUB_um_job_qualification.pt_qualification_level_i = PUB_pt_qualification_level.pt_qualification_level_i")
                            + GenerateWhereClauseLong("PUB_pt_qualification_level", PtQualificationLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobQualificationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobQualificationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtQualificationLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevelTemplate(out UmJobQualificationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtQualificationLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtQualificationLevel(Int32 AQualificationLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AQualificationLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_qualification WHERE pt_qualification_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtQualificationLevelTemplate(PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_pt_qualification_level WHERE " +
                "PUB_um_job_qualification.pt_qualification_level_i = PUB_pt_qualification_level.pt_qualification_level_i" + GenerateWhereClauseLong("PUB_pt_qualification_level", PtQualificationLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtQualificationLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtQualificationLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification, PUB_pt_qualification_level WHERE " +
                "PUB_um_job_qualification.pt_qualification_level_i = PUB_pt_qualification_level.pt_qualification_level_i" +
                GenerateWhereClauseLong("PUB_pt_qualification_level", PtQualificationLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtQualificationLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AQualificationAreaName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_job_qualification WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_qualification_area_name_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_qualification" + GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_qualification" +
                GenerateWhereClause(UmJobQualificationTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobQualificationTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmJobQualificationTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_job_qualification", UmJobQualificationTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_job_qualification", UmJobQualificationTable.GetColumnStringList(), UmJobQualificationTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_job_qualification", UmJobQualificationTable.GetColumnStringList(), UmJobQualificationTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmJobQualification", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details regarding the vision associated with various jobs.
    public class UmJobVisionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmJobVision";

        /// original table name in database
        public const string DBTABLENAME = "um_job_vision";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobVisionTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmJobVisionTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmJobVisionTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AVisionAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_vision_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision")
                            + GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobVisionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmJobVisionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, UmJobVisionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, UmJobVisionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision")
                            + GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AVisionAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_vision_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision" + GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision" + GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out UmJobVisionTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_um_job WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_um_job WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_um_job WHERE " +
                "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i" + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UmJobTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_um_job WHERE " +
                "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i" +
                GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision WHERE pm_unit_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobVisionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobVisionTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmJobVisionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_p_unit WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_p_unit WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision WHERE pm_unit_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_p_unit WHERE " +
                "PUB_um_job_vision.pm_unit_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_p_unit WHERE " +
                "PUB_um_job_vision.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision WHERE pt_vision_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobVisionTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionArea(FillDataSet, AVisionAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobVisionTable AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmJobVisionTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtVisionArea(String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision WHERE pt_vision_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                "PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtVisionAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                "PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" +
                GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtVisionAreaTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet ADataSet, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AVisionLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision WHERE pt_vision_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet AData, Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(AData, AVisionLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(AData, AVisionLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmJobVisionTable AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevel(FillDataSet, AVisionLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmJobVisionTable AData, Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(out AData, AVisionLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmJobVisionTable AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(out AData, AVisionLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet ADataSet, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_pt_vision_level WHERE " +
                            "PUB_um_job_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i")
                            + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, PtVisionLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, PtVisionLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, PtVisionLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job_vision", AFieldList, new string[] {
                            "pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i", "pt_vision_area_name_c"}) + " FROM PUB_um_job_vision, PUB_pt_vision_level WHERE " +
                            "PUB_um_job_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i")
                            + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmJobVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmJobVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmJobVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtVisionLevel(Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AVisionLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_job_vision WHERE pt_vision_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_pt_vision_level WHERE " +
                "PUB_um_job_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i" + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtVisionLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision, PUB_pt_vision_level WHERE " +
                "PUB_um_job_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i" +
                GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtVisionLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ParametersArray[4] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[4].Value = ((object)(AVisionAreaName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_job_vision WHERE pm_unit_key_n = ? AND pt_position_name_c = ? AND pt_position_scope_c = ? AND um_job_key_i = ? AND pt_vision_area_name_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_vision" + GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_job_vision" +
                GenerateWhereClause(UmJobVisionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmJobVisionTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmJobVisionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_job_vision", UmJobVisionTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_job_vision", UmJobVisionTable.GetColumnStringList(), UmJobVisionTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_job_vision", UmJobVisionTable.GetColumnStringList(), UmJobVisionTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmJobVision", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Describes whether a person is full-time, part-time, etc.
    public class PtAssignmentTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PtAssignmentType";

        /// original table name in database
        public const string DBTABLENAME = "pt_assignment_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_assignment_type_code_c"}) + " FROM PUB_pt_assignment_type")
                            + GenerateOrderByClause(AOrderBy)), PtAssignmentTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtAssignmentTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtAssignmentTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PtAssignmentTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtAssignmentTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AAssignmentTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 2);
            ParametersArray[0].Value = ((object)(AAssignmentTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_assignment_type_code_c"}) + " FROM PUB_pt_assignment_type WHERE pt_assignment_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PtAssignmentTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAssignmentTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAssignmentTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAssignmentTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtAssignmentTypeTable AData, String AAssignmentTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtAssignmentTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AAssignmentTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtAssignmentTypeTable AData, String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AAssignmentTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtAssignmentTypeTable AData, String AAssignmentTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AAssignmentTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_assignment_type_code_c"}) + " FROM PUB_pt_assignment_type")
                            + GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PtAssignmentTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtAssignmentTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtAssignmentTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtAssignmentTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, PtAssignmentTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, PtAssignmentTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_assignment_type_code_c"}) + " FROM PUB_pt_assignment_type")
                            + GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PtAssignmentTypeTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PtAssignmentTypeTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtAssignmentTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtAssignmentTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_assignment_type", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 2);
            ParametersArray[0].Value = ((object)(AAssignmentTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_assignment_type WHERE pt_assignment_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_assignment_type" + GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_assignment_type" + GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PtAssignmentTypeTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 2);
            ParametersArray[0].Value = ((object)(AAssignmentTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pt_assignment_type WHERE pt_assignment_type_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_assignment_type" + GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_assignment_type" +
                GenerateWhereClause(PtAssignmentTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtAssignmentTypeTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PtAssignmentTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("pt_assignment_type", PtAssignmentTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pt_assignment_type", PtAssignmentTypeTable.GetColumnStringList(), PtAssignmentTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pt_assignment_type", PtAssignmentTypeTable.GetColumnStringList(), PtAssignmentTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PtAssignmentType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// This describes the reason a person left a particular position.
    public class PtLeavingCodeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PtLeavingCode";

        /// original table name in database
        public const string DBTABLENAME = "pt_leaving_code";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_leaving_code_ind_c"}) + " FROM PUB_pt_leaving_code")
                            + GenerateOrderByClause(AOrderBy)), PtLeavingCodeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtLeavingCodeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtLeavingCodeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PtLeavingCodeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PtLeavingCodeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ALeavingCodeInd, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 4);
            ParametersArray[0].Value = ((object)(ALeavingCodeInd));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pt_leaving_code_ind_c"}) + " FROM PUB_pt_leaving_code WHERE pt_leaving_code_ind_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PtLeavingCodeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALeavingCodeInd, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALeavingCodeInd, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALeavingCodeInd, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtLeavingCodeTable AData, String ALeavingCodeInd, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtLeavingCodeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALeavingCodeInd, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtLeavingCodeTable AData, String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALeavingCodeInd, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PtLeavingCodeTable AData, String ALeavingCodeInd, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALeavingCodeInd, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_leaving_code_ind_c"}) + " FROM PUB_pt_leaving_code")
                            + GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PtLeavingCodeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtLeavingCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PtLeavingCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtLeavingCodeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, PtLeavingCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, PtLeavingCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pt_leaving_code_ind_c"}) + " FROM PUB_pt_leaving_code")
                            + GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PtLeavingCodeTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PtLeavingCodeTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PtLeavingCodeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PtLeavingCodeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_leaving_code", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 4);
            ParametersArray[0].Value = ((object)(ALeavingCodeInd));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pt_leaving_code WHERE pt_leaving_code_ind_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_leaving_code" + GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_leaving_code" + GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PtLeavingCodeTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 4);
            ParametersArray[0].Value = ((object)(ALeavingCodeInd));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pt_leaving_code WHERE pt_leaving_code_ind_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_leaving_code" + GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pt_leaving_code" +
                GenerateWhereClause(PtLeavingCodeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtLeavingCodeTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PtLeavingCodeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("pt_leaving_code", PtLeavingCodeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pt_leaving_code", PtLeavingCodeTable.GetColumnStringList(), PtLeavingCodeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pt_leaving_code", PtLeavingCodeTable.GetColumnStringList(), PtLeavingCodeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PtLeavingCode", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details of  the abilities within the unit.
    public class UmUnitAbilityAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmUnitAbility";

        /// original table name in database
        public const string DBTABLENAME = "um_unit_ability";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability")
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitAbilityTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmUnitAbilityTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitAbilityTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AAbilityAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability WHERE p_partner_key_n = ? AND pt_ability_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitAbilityTable AData, Int64 APartnerKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, AAbilityAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitAbilityTable AData, Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitAbilityTable AData, Int64 APartnerKey, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability")
                            + GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitAbilityRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitAbilityRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, UmUnitAbilityRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, UmUnitAbilityRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability")
                            + GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_ability", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AAbilityAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_ability WHERE p_partner_key_n = ? AND pt_ability_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability" + GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability" + GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitAbilityTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitAbilityTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitAbilityTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_p_unit WHERE " +
                            "PUB_um_unit_ability.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_p_unit WHERE " +
                            "PUB_um_unit_ability.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_ability WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_p_unit WHERE " +
                "PUB_um_unit_ability.p_partner_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_p_unit WHERE " +
                "PUB_um_unit_ability.p_partner_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability WHERE pt_ability_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmUnitAbilityTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityArea(FillDataSet, AAbilityAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmUnitAbilityTable AData, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(out UmUnitAbilityTable AData, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityArea(out AData, AAbilityAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_pt_ability_area WHERE " +
                            "PUB_um_unit_ability.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, PtAbilityAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, PtAbilityAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_pt_ability_area WHERE " +
                            "PUB_um_unit_ability.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityAreaTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtAbilityArea(String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_ability WHERE pt_ability_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_pt_ability_area WHERE " +
                "PUB_um_unit_ability.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" + GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtAbilityAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_pt_ability_area WHERE " +
                "PUB_um_unit_ability.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" +
                GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtAbilityAreaTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet ADataSet, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AAbilityLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability WHERE pt_ability_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet AData, Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(AData, AAbilityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(AData, AAbilityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmUnitAbilityTable AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevel(FillDataSet, AAbilityLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmUnitAbilityTable AData, Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(out AData, AAbilityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(out UmUnitAbilityTable AData, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevel(out AData, AAbilityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet ADataSet, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_pt_ability_level WHERE " +
                            "PUB_um_unit_ability.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i")
                            + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, PtAbilityLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, PtAbilityLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, PtAbilityLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, PtAbilityLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_ability", AFieldList, new string[] {
                            "p_partner_key_n", "pt_ability_area_name_c"}) + " FROM PUB_um_unit_ability, PUB_pt_ability_level WHERE " +
                            "PUB_um_unit_ability.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i")
                            + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitAbilityTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitAbilityTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtAbilityLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevelTemplate(out UmUnitAbilityTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtAbilityLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtAbilityLevel(Int32 AAbilityLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AAbilityLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_ability WHERE pt_ability_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_pt_ability_level WHERE " +
                "PUB_um_unit_ability.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i" + GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtAbilityLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability, PUB_pt_ability_level WHERE " +
                "PUB_um_unit_ability.pt_ability_level_i = PUB_pt_ability_level.pt_ability_level_i" +
                GenerateWhereClauseLong("PUB_pt_ability_level", PtAbilityLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtAbilityLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AAbilityAreaName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_ability WHERE p_partner_key_n = ? AND pt_ability_area_name_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_ability" + GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_ability" +
                GenerateWhereClause(UmUnitAbilityTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmUnitAbilityTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitAbilityTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_unit_ability", UmUnitAbilityTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_unit_ability", UmUnitAbilityTable.GetColumnStringList(), UmUnitAbilityTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_unit_ability", UmUnitAbilityTable.GetColumnStringList(), UmUnitAbilityTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmUnitAbility", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details of the language used within this unit.
    public class UmUnitLanguageAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmUnitLanguage";

        /// original table name in database
        public const string DBTABLENAME = "um_unit_language";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language")
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmUnitLanguageTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[1].Value = ((object)(ALanguageCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ALanguageLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language WHERE p_partner_key_n = ? AND p_language_code_c = ? AND pt_language_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, ALanguageCode, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, ALanguageCode, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitLanguageTable AData, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, ALanguageCode, ALanguageLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitLanguageTable AData, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, ALanguageCode, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitLanguageTable AData, Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, ALanguageCode, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language")
                            + GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, UmUnitLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, UmUnitLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language")
                            + GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_language", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[1].Value = ((object)(ALanguageCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ALanguageLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_language WHERE p_partner_key_n = ? AND p_language_code_c = ? AND pt_language_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language" + GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language" + GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitLanguageTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_p_unit WHERE " +
                            "PUB_um_unit_language.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_p_unit WHERE " +
                            "PUB_um_unit_language.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_language WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_p_unit WHERE " +
                "PUB_um_unit_language.p_partner_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_p_unit WHERE " +
                "PUB_um_unit_language.p_partner_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language WHERE p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmUnitLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguage(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmUnitLanguageTable AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out UmUnitLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_p_language WHERE " +
                            "PUB_um_unit_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_p_language WHERE " +
                            "PUB_um_unit_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_language WHERE p_language_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_p_language WHERE " +
                "PUB_um_unit_language.p_language_code_c = PUB_p_language.p_language_code_c" + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PLanguageTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_p_language WHERE " +
                "PUB_um_unit_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet ADataSet, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALanguageLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language WHERE pt_language_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet AData, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(AData, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(AData, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmUnitLanguageTable AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevel(FillDataSet, ALanguageLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmUnitLanguageTable AData, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(out AData, ALanguageLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(out UmUnitLanguageTable AData, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevel(out AData, ALanguageLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet ADataSet, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_pt_language_level WHERE " +
                            "PUB_um_unit_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i")
                            + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, PtLanguageLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, PtLanguageLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, PtLanguageLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_language", AFieldList, new string[] {
                            "p_partner_key_n", "p_language_code_c", "pt_language_level_i"}) + " FROM PUB_um_unit_language, PUB_pt_language_level WHERE " +
                            "PUB_um_unit_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i")
                            + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtLanguageLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevelTemplate(out UmUnitLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtLanguageLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtLanguageLevel(Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALanguageLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_language WHERE pt_language_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_pt_language_level WHERE " +
                "PUB_um_unit_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i" + GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtLanguageLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language, PUB_pt_language_level WHERE " +
                "PUB_um_unit_language.pt_language_level_i = PUB_pt_language_level.pt_language_level_i" +
                GenerateWhereClauseLong("PUB_pt_language_level", PtLanguageLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtLanguageLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[1].Value = ((object)(ALanguageCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ALanguageLevel));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_language WHERE p_partner_key_n = ? AND p_language_code_c = ? AND pt_language_level_i = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_language" + GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_language" +
                GenerateWhereClause(UmUnitLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmUnitLanguageTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_unit_language", UmUnitLanguageTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_unit_language", UmUnitLanguageTable.GetColumnStringList(), UmUnitLanguageTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_unit_language", UmUnitLanguageTable.GetColumnStringList(), UmUnitLanguageTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmUnitLanguage", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details of the vision required on this unit.
    public class UmUnitVisionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmUnitVision";

        /// original table name in database
        public const string DBTABLENAME = "um_unit_vision";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision")
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitVisionTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmUnitVisionTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitVisionTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AVisionAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision WHERE p_partner_key_n = ? AND pt_vision_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitVisionTable AData, Int64 APartnerKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, AVisionAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitVisionTable AData, Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitVisionTable AData, Int64 APartnerKey, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision")
                            + GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitVisionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitVisionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, UmUnitVisionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, UmUnitVisionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision")
                            + GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_vision", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AVisionAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_vision WHERE p_partner_key_n = ? AND pt_vision_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision" + GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision" + GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitVisionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitVisionTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitVisionTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_p_unit WHERE " +
                            "PUB_um_unit_vision.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_p_unit WHERE " +
                            "PUB_um_unit_vision.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_vision WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_p_unit WHERE " +
                "PUB_um_unit_vision.p_partner_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_p_unit WHERE " +
                "PUB_um_unit_vision.p_partner_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision WHERE pt_vision_area_name_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmUnitVisionTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionArea(FillDataSet, AVisionAreaName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmUnitVisionTable AData, String AVisionAreaName, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(out UmUnitVisionTable AData, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionArea(out AData, AVisionAreaName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_unit_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, PtVisionAreaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_unit_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c")
                            + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionAreaTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionAreaTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionAreaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtVisionArea(String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_vision WHERE pt_vision_area_name_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_pt_vision_area WHERE " +
                "PUB_um_unit_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" + GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtVisionAreaTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_pt_vision_area WHERE " +
                "PUB_um_unit_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" +
                GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtVisionAreaTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet ADataSet, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AVisionLevel));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision WHERE pt_vision_level_i = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet AData, Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(AData, AVisionLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(AData, AVisionLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmUnitVisionTable AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevel(FillDataSet, AVisionLevel, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmUnitVisionTable AData, Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(out AData, AVisionLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(out UmUnitVisionTable AData, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevel(out AData, AVisionLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet ADataSet, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_pt_vision_level WHERE " +
                            "PUB_um_unit_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i")
                            + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, PtVisionLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, PtVisionLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevelTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, PtVisionLevelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_vision", AFieldList, new string[] {
                            "p_partner_key_n", "pt_vision_area_name_c"}) + " FROM PUB_um_unit_vision, PUB_pt_vision_level WHERE " +
                            "PUB_um_unit_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i")
                            + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitVisionTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitVisionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPtVisionLevelTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPtVisionLevelTemplate(out UmUnitVisionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPtVisionLevelTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPtVisionLevel(Int32 AVisionLevel, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AVisionLevel));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_vision WHERE pt_vision_level_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_pt_vision_level WHERE " +
                "PUB_um_unit_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i" + GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PtVisionLevelTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision, PUB_pt_vision_level WHERE " +
                "PUB_um_unit_vision.pt_vision_level_i = PUB_pt_vision_level.pt_vision_level_i" +
                GenerateWhereClauseLong("PUB_pt_vision_level", PtVisionLevelTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PtVisionLevelTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AVisionAreaName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_vision WHERE p_partner_key_n = ? AND pt_vision_area_name_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_vision" + GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_vision" +
                GenerateWhereClause(UmUnitVisionTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmUnitVisionTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitVisionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_unit_vision", UmUnitVisionTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_unit_vision", UmUnitVisionTable.GetColumnStringList(), UmUnitVisionTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_unit_vision", UmUnitVisionTable.GetColumnStringList(), UmUnitVisionTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmUnitVision", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details pertaining to the costs of being on in the unit.
    public class UmUnitCostAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmUnitCost";

        /// original table name in database
        public const string DBTABLENAME = "um_unit_cost";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost")
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmUnitCostTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, System.DateTime AValidFromDate, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AValidFromDate));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost WHERE p_partner_key_n = ? AND um_valid_from_date_d = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AValidFromDate, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, System.DateTime AValidFromDate, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AValidFromDate, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitCostTable AData, Int64 APartnerKey, System.DateTime AValidFromDate, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, AValidFromDate, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitCostTable AData, Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AValidFromDate, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitCostTable AData, Int64 APartnerKey, System.DateTime AValidFromDate, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AValidFromDate, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost")
                            + GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, UmUnitCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, UmUnitCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost")
                            + GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitCostTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_cost", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AValidFromDate));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_cost WHERE p_partner_key_n = ? AND um_valid_from_date_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost" + GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost" + GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmUnitCostTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitCostTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_cost", AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost, PUB_p_unit WHERE " +
                            "PUB_um_unit_cost.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_cost", AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost, PUB_p_unit WHERE " +
                            "PUB_um_unit_cost.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitCostTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_cost WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost, PUB_p_unit WHERE " +
                "PUB_um_unit_cost.p_partner_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost, PUB_p_unit WHERE " +
                "PUB_um_unit_cost.p_partner_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost WHERE a_local_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out UmUnitCostTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrency(out UmUnitCostTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out UmUnitCostTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_cost", AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost, PUB_a_currency WHERE " +
                            "PUB_um_unit_cost.a_local_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_cost", AFieldList, new string[] {
                            "p_partner_key_n", "um_valid_from_date_d"}) + " FROM PUB_um_unit_cost, PUB_a_currency WHERE " +
                            "PUB_um_unit_cost.a_local_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitCostTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitCostTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out UmUnitCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_cost WHERE a_local_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost, PUB_a_currency WHERE " +
                "PUB_um_unit_cost.a_local_currency_code_c = PUB_a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost, PUB_a_currency WHERE " +
                "PUB_um_unit_cost.a_local_currency_code_c = PUB_a_currency.a_currency_code_c" +
                GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AValidFromDate));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_cost WHERE p_partner_key_n = ? AND um_valid_from_date_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_cost" + GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_cost" +
                GenerateWhereClause(UmUnitCostTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmUnitCostTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("um_unit_cost", UmUnitCostTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_unit_cost", UmUnitCostTable.GetColumnStringList(), UmUnitCostTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_unit_cost", UmUnitCostTable.GetColumnStringList(), UmUnitCostTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmUnitCost", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Details pertaining to evaluation of the unit.
    public class UmUnitEvaluationAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "UmUnitEvaluation";

        /// original table name in database
        public const string DBTABLENAME = "um_unit_evaluation";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation")
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitEvaluationTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out UmUnitEvaluationTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out UmUnitEvaluationTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(ADateOfEvaluation));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Decimal, 14);
            ParametersArray[2].Value = ((object)(AEvaluationNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation WHERE p_partner_key_n = ? AND um_date_of_evaluation_d = ? AND um_evaluation_number_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, ADateOfEvaluation, AEvaluationNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, ADateOfEvaluation, AEvaluationNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitEvaluationTable AData, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, ADateOfEvaluation, AEvaluationNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitEvaluationTable AData, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, ADateOfEvaluation, AEvaluationNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out UmUnitEvaluationTable AData, Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, ADateOfEvaluation, AEvaluationNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation")
                            + GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitEvaluationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, UmUnitEvaluationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, UmUnitEvaluationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, UmUnitEvaluationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation")
                            + GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitEvaluationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_evaluation", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(ADateOfEvaluation));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Decimal, 14);
            ParametersArray[2].Value = ((object)(AEvaluationNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_evaluation WHERE p_partner_key_n = ? AND um_date_of_evaluation_d = ? AND um_evaluation_number_n = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation" + GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation" + GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new UmUnitEvaluationTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitEvaluationTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitEvaluationTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out UmUnitEvaluationTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_evaluation", AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation, PUB_p_unit WHERE " +
                            "PUB_um_unit_evaluation.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_unit_evaluation", AFieldList, new string[] {
                            "p_partner_key_n", "um_date_of_evaluation_d", "um_evaluation_number_n"}) + " FROM PUB_um_unit_evaluation, PUB_p_unit WHERE " +
                            "PUB_um_unit_evaluation.p_partner_key_n = PUB_p_unit.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), UmUnitEvaluationTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new UmUnitEvaluationTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new UmUnitEvaluationTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out UmUnitEvaluationTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_um_unit_evaluation WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation, PUB_p_unit WHERE " +
                "PUB_um_unit_evaluation.p_partner_key_n = PUB_p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation, PUB_p_unit WHERE " +
                "PUB_um_unit_evaluation.p_partner_key_n = PUB_p_unit.p_partner_key_n" +
                GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PUnitTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(ADateOfEvaluation));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Decimal, 14);
            ParametersArray[2].Value = ((object)(AEvaluationNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_um_unit_evaluation WHERE p_partner_key_n = ? AND um_date_of_evaluation_d = ? AND um_evaluation_number_n = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_evaluation" + GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_um_unit_evaluation" +
                GenerateWhereClause(UmUnitEvaluationTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new UmUnitEvaluationTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitEvaluationTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        ((UmUnitEvaluationRow)(TheRow)).EvaluationNumber = ((Decimal)(DBAccess.GDBAccessObj.GetNextSequenceValue("seq_pe_evaluation_number", ATransaction)));
                        TTypedDataAccess.InsertRow("um_unit_evaluation", UmUnitEvaluationTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("um_unit_evaluation", UmUnitEvaluationTable.GetColumnStringList(), UmUnitEvaluationTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("um_unit_evaluation", UmUnitEvaluationTable.GetColumnStringList(), UmUnitEvaluationTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table UmUnitEvaluation", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}