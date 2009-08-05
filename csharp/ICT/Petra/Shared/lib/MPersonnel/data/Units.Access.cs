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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PtPositionTable.TableId) + " FROM PUB_pt_position") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtPositionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PtPositionTable.TableId) + " FROM PUB_pt_position" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PtPositionTable.TableId, ADataSet, new System.Object[2]{APositionName, APositionScope}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadByPrimaryKey(PtPositionTable.TableId, AData, new System.Object[2]{APositionName, APositionScope}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtPositionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadUsingTemplate(PtPositionTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtPositionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadUsingTemplate(PtPositionTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            return Exists(PtPositionTable.TableId, new System.Object[2]{APositionName, APositionScope}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtPositionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PtPositionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtPositionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PtPositionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet ADataSet, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtPositionTable();
            LoadViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PtPositionTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pt_position", AFieldList, PtPositionTable.TableId) +
                            " FROM PUB_pt_position, PUB_um_job WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtPositionTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, PtPositionTable.TableId) +
                            " FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_unit", PUnitTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtPositionTable.TableId), ATransaction,
                            GetParametersForWhereClause(PUnitTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pt_position", AFieldList, PtPositionTable.TableId) +
                            " FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                            "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_unit", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtPositionTable.TableId), ATransaction,
                            GetParametersForWhereClause(PtPositionTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_um_job", PtPositionTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PUnitTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_position, PUB_um_job, PUB_p_unit WHERE " +
                        "PUB_um_job.pt_position_name_c = PUB_pt_position.pt_position_name_c AND PUB_um_job.pt_position_scope_c = PUB_pt_position.pt_position_scope_c AND PUB_um_job.pm_unit_key_n = PUB_p_unit.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_um_job", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PtPositionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String APositionName, String APositionScope, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PtPositionTable.TableId, new System.Object[2]{APositionName, APositionScope}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtPositionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtPositionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PtPositionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PtPositionTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmJobTable.TableId) + " FROM PUB_um_job") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmJobTable.TableId) + " FROM PUB_um_job" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmJobTable.TableId, ADataSet, new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadByPrimaryKey(UmJobTable.TableId, AData, new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadUsingTemplate(UmJobTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadUsingTemplate(UmJobTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            return Exists(UmJobTable.TableId, new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmJobTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtPosition(DataSet ADataSet, String APositionName, String APositionScope, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, ADataSet, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                new System.Object[2]{APositionName, APositionScope}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, AData, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                new System.Object[2]{APositionName, APositionScope}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, ADataSet, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, AData, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, ADataSet, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, AData, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                new System.Object[2]{APositionName, APositionScope}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtPositionTemplate(PtPositionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtPositionTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, PtPositionTable.TableId, new string[2]{"pt_position_name_c", "pt_position_scope_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaUUnitType(DataSet ADataSet, String AUnitTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, ADataSet, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobTable();
            LoadViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, AData, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                new System.Object[1]{AUnitTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(UUnitTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUUnitTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobTable.TableId, UUnitTypeTable.TableId, new string[1]{"pt_position_scope_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AAbilityAreaName));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_requirement WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_ability_area", PtAbilityAreaTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(PtAbilityAreaTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                            "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_ability_area", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_um_job_requirement", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PtAbilityAreaTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_requirement, PUB_pt_ability_area WHERE " +
                        "PUB_um_job_requirement.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_requirement.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_requirement.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_requirement.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_requirement.pt_ability_area_name_c = PUB_pt_ability_area.pt_ability_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_requirement", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c") +
                            GenerateWhereClauseLong("PUB_p_language", PLanguageTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(PLanguageTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                            "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c") +
                            GenerateWhereClauseLong("PUB_p_language", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_um_job_language", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PLanguageTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_language, PUB_p_language WHERE " +
                        "PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_um_job_language", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtQualificationArea(DataSet ADataSet, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AQualificationAreaName));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_qualification WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_qualification_area", PtQualificationAreaTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(PtQualificationAreaTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                            "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_qualification_area", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_um_job_qualification", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PtQualificationAreaTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_qualification, PUB_pt_qualification_area WHERE " +
                        "PUB_um_job_qualification.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_qualification.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_qualification.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_qualification.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_qualification.pt_qualification_area_name_c = PUB_pt_qualification_area.pt_qualification_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_qualification", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AVisionAreaName));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_vision WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_vision_area", PtVisionAreaTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(PtVisionAreaTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                            "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c") +
                            GenerateWhereClauseLong("PUB_pt_vision_area", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_um_job_vision", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PtVisionAreaTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_um_job_vision, PUB_pt_vision_area WHERE " +
                        "PUB_um_job_vision.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_vision.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_vision.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_vision.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_um_job_vision.pt_vision_area_name_c = PUB_pt_vision_area.pt_vision_area_name_c" +
                        GenerateWhereClauseLong("PUB_um_job_vision", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_pm_job_assignment WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                            "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_pm_job_assignment", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPartnerTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_pm_job_assignment, PUB_p_partner WHERE " +
                        "PUB_pm_job_assignment.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_pm_job_assignment.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_pm_job_assignment.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_pm_job_assignment.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_pm_job_assignment.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_job_assignment", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_s_job_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = ? AND PUB_s_job_group.s_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", SGroupTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(SGroupTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_um_job", AFieldList, UmJobTable.TableId) +
                            " FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                            "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_s_job_group", UmJobTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(SGroupTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job, PUB_s_job_group, PUB_s_group WHERE " +
                        "PUB_s_job_group.s_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_s_job_group.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_s_job_group.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_s_job_group.um_job_key_i = PUB_um_job.um_job_key_i AND PUB_s_job_group.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_job_group.s_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_job_group", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(UmJobTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmJobTable.TableId, new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmJobTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmJobTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_job", "um_job_key_i");
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmJobRequirementTable.TableId) + " FROM PUB_um_job_requirement") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobRequirementTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmJobRequirementTable.TableId) + " FROM PUB_um_job_requirement" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmJobRequirementTable.TableId, ADataSet, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadByPrimaryKey(UmJobRequirementTable.TableId, AData, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobRequirementTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadUsingTemplate(UmJobRequirementTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobRequirementTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadUsingTemplate(UmJobRequirementTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            return Exists(UmJobRequirementTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobRequirementTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmJobRequirementTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_requirement" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobRequirementTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmJobRequirementTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet ADataSet, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobRequirementTable();
            LoadViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobRequirementTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmJobRequirementTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AAbilityAreaName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobRequirementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobRequirementTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobRequirementTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmJobRequirementTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmJobRequirementTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmJobLanguageTable.TableId) + " FROM PUB_um_job_language") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobLanguageTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmJobLanguageTable.TableId) + " FROM PUB_um_job_language" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmJobLanguageTable.TableId, ADataSet, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadByPrimaryKey(UmJobLanguageTable.TableId, AData, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobLanguageTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadUsingTemplate(UmJobLanguageTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobLanguageTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadUsingTemplate(UmJobLanguageTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            return Exists(UmJobLanguageTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobLanguageTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmJobLanguageTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobLanguageTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmJobLanguageTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet ADataSet, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobLanguageTable();
            LoadViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String ALanguageCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmJobLanguageTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobLanguageTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobLanguageTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmJobLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmJobLanguageTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmJobQualificationTable.TableId) + " FROM PUB_um_job_qualification") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobQualificationTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmJobQualificationTable.TableId) + " FROM PUB_um_job_qualification" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmJobQualificationTable.TableId, ADataSet, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadByPrimaryKey(UmJobQualificationTable.TableId, AData, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobQualificationTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadUsingTemplate(UmJobQualificationTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobQualificationTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadUsingTemplate(UmJobQualificationTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            return Exists(UmJobQualificationTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobQualificationTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmJobQualificationTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_qualification" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobQualificationTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmJobQualificationTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtQualificationArea(DataSet ADataSet, String AQualificationAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, ADataSet, new string[1]{"pt_qualification_area_name_c"},
                new System.Object[1]{AQualificationAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, AData, new string[1]{"pt_qualification_area_name_c"},
                new System.Object[1]{AQualificationAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, ADataSet, new string[1]{"pt_qualification_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, AData, new string[1]{"pt_qualification_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, ADataSet, new string[1]{"pt_qualification_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, AData, new string[1]{"pt_qualification_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, new string[1]{"pt_qualification_area_name_c"},
                new System.Object[1]{AQualificationAreaName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(PtQualificationAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, new string[1]{"pt_qualification_area_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtQualificationAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationAreaTable.TableId, new string[1]{"pt_qualification_area_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtQualificationLevel(DataSet ADataSet, Int32 AQualificationLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, ADataSet, new string[1]{"pt_qualification_level_i"},
                new System.Object[1]{AQualificationLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, AData, new string[1]{"pt_qualification_level_i"},
                new System.Object[1]{AQualificationLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, ADataSet, new string[1]{"pt_qualification_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, AData, new string[1]{"pt_qualification_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, ADataSet, new string[1]{"pt_qualification_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobQualificationTable();
            LoadViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, AData, new string[1]{"pt_qualification_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, new string[1]{"pt_qualification_level_i"},
                new System.Object[1]{AQualificationLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtQualificationLevelTemplate(PtQualificationLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, new string[1]{"pt_qualification_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtQualificationLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobQualificationTable.TableId, PtQualificationLevelTable.TableId, new string[1]{"pt_qualification_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AQualificationAreaName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmJobQualificationTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AQualificationAreaName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobQualificationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobQualificationTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobQualificationTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmJobQualificationTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmJobQualificationTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmJobVisionTable.TableId) + " FROM PUB_um_job_vision") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmJobVisionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmJobVisionTable.TableId) + " FROM PUB_um_job_vision" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmJobVisionTable.TableId, ADataSet, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadByPrimaryKey(UmJobVisionTable.TableId, AData, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobVisionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadUsingTemplate(UmJobVisionTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmJobVisionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadUsingTemplate(UmJobVisionTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            return Exists(UmJobVisionTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobVisionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmJobVisionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_job_vision" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmJobVisionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmJobVisionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, ADataSet, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, AData, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                new System.Object[4]{AUnitKey, APositionName, APositionScope, AJobKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, UmJobTable.TableId, new string[4]{"pm_unit_key_n", "pt_position_name_c", "pt_position_scope_c", "um_job_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PUnitTable.TableId, new string[1]{"pm_unit_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet ADataSet, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmJobVisionTable();
            LoadViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmJobVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmJobVisionTable.TableId, new System.Object[5]{AUnitKey, APositionName, APositionScope, AJobKey, AVisionAreaName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmJobVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobVisionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmJobVisionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmJobVisionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmJobVisionTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PtAssignmentTypeTable.TableId) + " FROM PUB_pt_assignment_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtAssignmentTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PtAssignmentTypeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PtAssignmentTypeTable.TableId) + " FROM PUB_pt_assignment_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PtAssignmentTypeTable.TableId, ADataSet, new System.Object[1]{AAssignmentTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtAssignmentTypeTable();
            LoadByPrimaryKey(PtAssignmentTypeTable.TableId, AData, new System.Object[1]{AAssignmentTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtAssignmentTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtAssignmentTypeTable();
            LoadUsingTemplate(PtAssignmentTypeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtAssignmentTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtAssignmentTypeTable();
            LoadUsingTemplate(PtAssignmentTypeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PtAssignmentTypeTable.TableId, new System.Object[1]{AAssignmentTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_assignment_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtAssignmentTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PtAssignmentTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_assignment_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtAssignmentTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PtAssignmentTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PtAssignmentTypeTable.TableId, new System.Object[1]{AAssignmentTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtAssignmentTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtAssignmentTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtAssignmentTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PtAssignmentTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PtAssignmentTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PtLeavingCodeTable.TableId) + " FROM PUB_pt_leaving_code") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PtLeavingCodeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PtLeavingCodeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PtLeavingCodeTable.TableId) + " FROM PUB_pt_leaving_code" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PtLeavingCodeTable.TableId, ADataSet, new System.Object[1]{ALeavingCodeInd}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtLeavingCodeTable();
            LoadByPrimaryKey(PtLeavingCodeTable.TableId, AData, new System.Object[1]{ALeavingCodeInd}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtLeavingCodeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtLeavingCodeTable();
            LoadUsingTemplate(PtLeavingCodeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PtLeavingCodeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PtLeavingCodeTable();
            LoadUsingTemplate(PtLeavingCodeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            return Exists(PtLeavingCodeTable.TableId, new System.Object[1]{ALeavingCodeInd}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_leaving_code" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtLeavingCodeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PtLeavingCodeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pt_leaving_code" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PtLeavingCodeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PtLeavingCodeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALeavingCodeInd, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PtLeavingCodeTable.TableId, new System.Object[1]{ALeavingCodeInd}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PtLeavingCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtLeavingCodeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PtLeavingCodeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PtLeavingCodeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PtLeavingCodeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmUnitAbilityTable.TableId) + " FROM PUB_um_unit_ability") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmUnitAbilityTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmUnitAbilityTable.TableId) + " FROM PUB_um_unit_ability" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmUnitAbilityTable.TableId, ADataSet, new System.Object[2]{APartnerKey, AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadByPrimaryKey(UmUnitAbilityTable.TableId, AData, new System.Object[2]{APartnerKey, AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitAbilityTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadUsingTemplate(UmUnitAbilityTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitAbilityTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadUsingTemplate(UmUnitAbilityTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            return Exists(UmUnitAbilityTable.TableId, new System.Object[2]{APartnerKey, AAbilityAreaName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitAbilityTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmUnitAbilityTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_ability" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitAbilityTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmUnitAbilityTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtAbilityArea(DataSet ADataSet, String AAbilityAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, ADataSet, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, AData, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                new System.Object[1]{AAbilityAreaName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(PtAbilityAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityAreaTable.TableId, new string[1]{"pt_ability_area_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtAbilityLevel(DataSet ADataSet, Int32 AAbilityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, ADataSet, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitAbilityTable();
            LoadViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, AData, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                new System.Object[1]{AAbilityLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(PtAbilityLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtAbilityLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitAbilityTable.TableId, PtAbilityLevelTable.TableId, new string[1]{"pt_ability_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AAbilityAreaName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmUnitAbilityTable.TableId, new System.Object[2]{APartnerKey, AAbilityAreaName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitAbilityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitAbilityTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitAbilityTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitAbilityTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmUnitAbilityTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmUnitLanguageTable.TableId) + " FROM PUB_um_unit_language") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmUnitLanguageTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmUnitLanguageTable.TableId) + " FROM PUB_um_unit_language" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmUnitLanguageTable.TableId, ADataSet, new System.Object[3]{APartnerKey, ALanguageCode, ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadByPrimaryKey(UmUnitLanguageTable.TableId, AData, new System.Object[3]{APartnerKey, ALanguageCode, ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitLanguageTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadUsingTemplate(UmUnitLanguageTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitLanguageTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadUsingTemplate(UmUnitLanguageTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            return Exists(UmUnitLanguageTable.TableId, new System.Object[3]{APartnerKey, ALanguageCode, ALanguageLevel}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitLanguageTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmUnitLanguageTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitLanguageTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmUnitLanguageTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, AData, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtLanguageLevel(DataSet ADataSet, Int32 ALanguageLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, ADataSet, new string[1]{"pt_language_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitLanguageTable();
            LoadViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, AData, new string[1]{"pt_language_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                new System.Object[1]{ALanguageLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(PtLanguageLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtLanguageLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitLanguageTable.TableId, PtLanguageLevelTable.TableId, new string[1]{"pt_language_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String ALanguageCode, Int32 ALanguageLevel, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmUnitLanguageTable.TableId, new System.Object[3]{APartnerKey, ALanguageCode, ALanguageLevel}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitLanguageTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitLanguageTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmUnitLanguageTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmUnitVisionTable.TableId) + " FROM PUB_um_unit_vision") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmUnitVisionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmUnitVisionTable.TableId) + " FROM PUB_um_unit_vision" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmUnitVisionTable.TableId, ADataSet, new System.Object[2]{APartnerKey, AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadByPrimaryKey(UmUnitVisionTable.TableId, AData, new System.Object[2]{APartnerKey, AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitVisionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadUsingTemplate(UmUnitVisionTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitVisionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadUsingTemplate(UmUnitVisionTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            return Exists(UmUnitVisionTable.TableId, new System.Object[2]{APartnerKey, AVisionAreaName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitVisionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmUnitVisionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_vision" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitVisionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmUnitVisionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtVisionArea(DataSet ADataSet, String AVisionAreaName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, ADataSet, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, AData, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                new System.Object[1]{AVisionAreaName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(PtVisionAreaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionAreaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionAreaTable.TableId, new string[1]{"pt_vision_area_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPtVisionLevel(DataSet ADataSet, Int32 AVisionLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, ADataSet, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitVisionTable();
            LoadViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, AData, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                new System.Object[1]{AVisionLevel}, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(PtVisionLevelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPtVisionLevelTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitVisionTable.TableId, PtVisionLevelTable.TableId, new string[1]{"pt_vision_level_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AVisionAreaName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmUnitVisionTable.TableId, new System.Object[2]{APartnerKey, AVisionAreaName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitVisionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitVisionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitVisionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitVisionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmUnitVisionTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmUnitCostTable.TableId) + " FROM PUB_um_unit_cost") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmUnitCostTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmUnitCostTable.TableId) + " FROM PUB_um_unit_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmUnitCostTable.TableId, ADataSet, new System.Object[2]{APartnerKey, AValidFromDate}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadByPrimaryKey(UmUnitCostTable.TableId, AData, new System.Object[2]{APartnerKey, AValidFromDate}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitCostTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadUsingTemplate(UmUnitCostTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitCostTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadUsingTemplate(UmUnitCostTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            return Exists(UmUnitCostTable.TableId, new System.Object[2]{APartnerKey, AValidFromDate}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitCostTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmUnitCostTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitCostTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmUnitCostTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitCostTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_local_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_local_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_local_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_local_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_local_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitCostTable();
            LoadViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_local_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, new string[1]{"a_local_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, new string[1]{"a_local_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitCostTable.TableId, ACurrencyTable.TableId, new string[1]{"a_local_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime AValidFromDate, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmUnitCostTable.TableId, new System.Object[2]{APartnerKey, AValidFromDate}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitCostTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitCostTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmUnitCostTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, UmUnitEvaluationTable.TableId) + " FROM PUB_um_unit_evaluation") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(UmUnitEvaluationTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, UmUnitEvaluationTable.TableId) + " FROM PUB_um_unit_evaluation" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(UmUnitEvaluationTable.TableId, ADataSet, new System.Object[3]{APartnerKey, ADateOfEvaluation, AEvaluationNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadByPrimaryKey(UmUnitEvaluationTable.TableId, AData, new System.Object[3]{APartnerKey, ADateOfEvaluation, AEvaluationNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitEvaluationTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadUsingTemplate(UmUnitEvaluationTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(UmUnitEvaluationTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadUsingTemplate(UmUnitEvaluationTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            return Exists(UmUnitEvaluationTable.TableId, new System.Object[3]{APartnerKey, ADateOfEvaluation, AEvaluationNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitEvaluationTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(UmUnitEvaluationTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_um_unit_evaluation" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(UmUnitEvaluationTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(UmUnitEvaluationTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new UmUnitEvaluationTable();
            LoadViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            return CountViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(UmUnitEvaluationTable.TableId, PUnitTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, System.DateTime ADateOfEvaluation, Decimal AEvaluationNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(UmUnitEvaluationTable.TableId, new System.Object[3]{APartnerKey, ADateOfEvaluation, AEvaluationNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(UmUnitEvaluationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitEvaluationTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(UmUnitEvaluationTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(UmUnitEvaluationTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(UmUnitEvaluationTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_pe_evaluation_number", "um_evaluation_number_n");
        }
    }
}