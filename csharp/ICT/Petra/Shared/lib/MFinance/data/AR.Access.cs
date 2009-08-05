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
namespace Ict.Petra.Shared.MFinance.AR.Data.Access
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
    using Ict.Petra.Shared.MFinance.AR.Data;
    using Ict.Petra.Shared.MFinance.Account.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;

    /// used for invoicing
    public class ATaxTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ATaxType";

        /// original table name in database
        public const string DBTABLENAME = "a_tax_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ATaxTypeTable.TableId) + " FROM PUB_a_tax_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATaxTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ATaxTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTypeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, ATaxTypeTable.TableId) + " FROM PUB_a_tax_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out ATaxTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out ATaxTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ATaxTypeTable.TableId, ADataSet, new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTypeTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTypeTable();
            LoadByPrimaryKey(ATaxTypeTable.TableId, AData, new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTypeTable AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTypeTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ATaxTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTypeTable();
            LoadUsingTemplate(ATaxTypeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ATaxTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTypeTable();
            LoadUsingTemplate(ATaxTypeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return Exists(ATaxTypeTable.TableId, new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATaxTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ATaxTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATaxTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ATaxTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_tax_type", AFieldList, ATaxTypeTable.TableId) +
                            " FROM PUB_a_tax_type, PUB_a_tax_table WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATaxTypeTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTypeTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTypeTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTypeTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_tax_type", AFieldList, ATaxTypeTable.TableId) +
                            " FROM PUB_a_tax_type, PUB_a_tax_table, PUB_a_ledger WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i") +
                            GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATaxTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(ALedgerTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_tax_type", AFieldList, ATaxTypeTable.TableId) +
                            " FROM PUB_a_tax_type, PUB_a_tax_table, PUB_a_ledger WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i") +
                            GenerateWhereClauseLong("PUB_a_ledger", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATaxTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(ATaxTypeTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_type, PUB_a_tax_table WHERE " +
                        "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_type, PUB_a_tax_table, PUB_a_ledger WHERE " +
                        "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i" +
                        GenerateWhereClauseLong("PUB_a_tax_table", ATaxTypeTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(ALedgerTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_type, PUB_a_tax_table, PUB_a_ledger WHERE " +
                        "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i" +
                        GenerateWhereClauseLong("PUB_a_tax_table", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(ATaxTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ATaxTypeTable.TableId, new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ATaxTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ATaxTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ATaxTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATaxTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This is used by the invoicing
    public class ATaxTableAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ATaxTable";

        /// original table name in database
        public const string DBTABLENAME = "a_tax_table";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ATaxTableTable.TableId) + " FROM PUB_a_tax_table") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ATaxTableTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ATaxTableTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, ATaxTableTable.TableId) + " FROM PUB_a_tax_table" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out ATaxTableTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out ATaxTableTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ATaxTableTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTableTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadByPrimaryKey(ATaxTableTable.TableId, AData, new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTableTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ATaxTableTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ATaxTableTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadUsingTemplate(ATaxTableTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ATaxTableTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadUsingTemplate(ATaxTableTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_table", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return Exists(ATaxTableTable.TableId, new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_table" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATaxTableTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ATaxTableTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_table" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ATaxTableTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ATaxTableTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTableTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTableTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out ATaxTableTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out ATaxTableTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(out ATaxTableTable AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out ATaxTableTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out ATaxTableTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ATaxTableTable.TableId, new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ATaxTableTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ATaxTableTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ATaxTableTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATaxTableTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// there are several categories that are can use invoicing: catering, hospitality, store and fees
    public class AArCategoryAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArCategory";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_category";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArCategoryTable.TableId) + " FROM PUB_a_ar_category") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArCategoryTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArCategoryTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArCategoryTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArCategoryTable.TableId) + " FROM PUB_a_ar_category" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArCategoryTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArCategoryTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArCategoryTable.TableId, ADataSet, new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArCategoryTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArCategoryTable();
            LoadByPrimaryKey(AArCategoryTable.TableId, AData, new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArCategoryTable AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArCategoryTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArCategoryTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArCategoryTable();
            LoadUsingTemplate(AArCategoryTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArCategoryTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArCategoryTable();
            LoadUsingTemplate(AArCategoryTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_category", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return Exists(AArCategoryTable.TableId, new System.Object[1]{AArCategoryCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_category" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArCategoryTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArCategoryTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_category" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArCategoryTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArCategoryTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_category", AFieldList, AArCategoryTable.TableId) +
                            " FROM PUB_a_ar_category, PUB_a_ar_default_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = ? AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArCategoryTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_category", AFieldList, AArCategoryTable.TableId) +
                            " FROM PUB_a_ar_category, PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArCategoryTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_category", AFieldList, AArCategoryTable.TableId) +
                            " FROM PUB_a_ar_category, PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArCategoryTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArCategoryTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_category, PUB_a_ar_default_discount WHERE " +
                        "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = ? AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_category, PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", AArCategoryTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArDiscountTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_category, PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArCategoryTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArCategoryTable.TableId, new System.Object[1]{AArCategoryCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArCategoryTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArCategoryTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArCategoryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArCategoryTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines an item that can be sold or a service that can be charged for; this can be used for catering, hospitality, store and fees; it can describe a specific book, or a group of equally priced books
    public class AArArticleAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArArticle";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_article";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArArticleTable.TableId) + " FROM PUB_a_ar_article") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArArticleTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArArticleTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArArticleTable.TableId) + " FROM PUB_a_ar_article" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArArticleTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArArticleTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArArticleTable.TableId, ADataSet, new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticleTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadByPrimaryKey(AArArticleTable.TableId, AData, new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticleTable AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticleTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArArticleTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadUsingTemplate(AArArticleTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArArticleTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadUsingTemplate(AArArticleTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArArticleCode, TDBTransaction ATransaction)
        {
            return Exists(AArArticleTable.TableId, new System.Object[1]{AArArticleCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArArticleTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArArticleTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArArticleTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArArticleTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArArticleTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArArticleTable AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArArticleTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArArticleTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArArticleTable AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArArticleTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArArticleTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArArticleCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArArticleTable.TableId, new System.Object[1]{AArArticleCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArArticleTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArArticleTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArArticleTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArArticleTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// assign a price to an article, which can be updated by time
    public class AArArticlePriceAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArArticlePrice";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_article_price";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArArticlePriceTable.TableId) + " FROM PUB_a_ar_article_price") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArArticlePriceTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArArticlePriceTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArArticlePriceTable.TableId) + " FROM PUB_a_ar_article_price" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArArticlePriceTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArArticlePriceTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArArticlePriceTable.TableId, ADataSet, new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticlePriceTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadByPrimaryKey(AArArticlePriceTable.TableId, AData, new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticlePriceTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArArticlePriceTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArArticlePriceTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadUsingTemplate(AArArticlePriceTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArArticlePriceTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadUsingTemplate(AArArticlePriceTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article_price", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return Exists(AArArticlePriceTable.TableId, new System.Object[2]{AArArticleCode, AArDateValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article_price" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArArticlePriceTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArArticlePriceTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article_price" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArArticlePriceTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArArticlePriceTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArArticlePriceTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArArticlePriceTable AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArArticlePriceTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrency(out AArArticlePriceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArArticlePriceTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArArticlePriceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArArticlePriceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArArticlePriceTable.TableId, new System.Object[2]{AArArticleCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArArticlePriceTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArArticlePriceTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArArticlePriceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArArticlePriceTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines a discount that depends on other conditions or can just be assigned to an invoice or article
    public class AArDiscountAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArDiscount";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_discount";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArDiscountTable.TableId) + " FROM PUB_a_ar_discount") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArDiscountTable.TableId) + " FROM PUB_a_ar_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArDiscountTable.TableId, ADataSet, new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadByPrimaryKey(AArDiscountTable.TableId, AData, new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDiscountTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadUsingTemplate(AArDiscountTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDiscountTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadUsingTemplate(AArDiscountTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return Exists(AArDiscountTable.TableId, new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDiscountTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArDiscountTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDiscountTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrency(out AArDiscountTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArDiscountTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArDiscountTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPType(DataSet ADataSet, String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, ADataSet, new string[1]{"p_partner_type_code_c"},
                new System.Object[1]{ATypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPType(DataSet AData, String ATypeCode, TDBTransaction ATransaction)
        {
            LoadViaPType(AData, ATypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPType(DataSet AData, String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPType(AData, ATypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPType(out AArDiscountTable AData, String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, AData, new string[1]{"p_partner_type_code_c"},
                new System.Object[1]{ATypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPType(out AArDiscountTable AData, String ATypeCode, TDBTransaction ATransaction)
        {
            LoadViaPType(out AData, ATypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPType(out AArDiscountTable AData, String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPType(out AData, ATypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet ADataSet, PTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, ADataSet, new string[1]{"p_partner_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet AData, PTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet AData, PTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, PTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, AData, new string[1]{"p_partner_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, PTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, PTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, PTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, ADataSet, new string[1]{"p_partner_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, AData, new string[1]{"p_partner_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPTypeTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPType(String ATypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, new string[1]{"p_partner_type_code_c"},
                new System.Object[1]{ATypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPTypeTemplate(PTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, new string[1]{"p_partner_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, new string[1]{"p_partner_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArDiscountTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArDiscountTable AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArDiscountTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_default_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategory(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountTable AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c") +
                            GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArCategoryTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategoryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c") +
                            GenerateWhereClauseLong("PUB_a_ar_category", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategoryTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_default_discount WHERE " +
                        "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                        "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c" +
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArCategoryTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                        "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c" +
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_discount.a_invoice_key_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoice(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i") +
                            GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArInvoiceTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i") +
                            GenerateWhereClauseLong("PUB_a_ar_invoice", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount WHERE " +
                        "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_discount.a_invoice_key_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                        "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArInvoiceTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                        "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArInvoiceDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = ? AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceDetail(FillDataSet, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet ADataSet, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i") +
                            GenerateWhereClauseLong("PUB_a_ar_invoice_detail", AArInvoiceDetailTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArInvoiceDetailTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, AArDiscountTable.TableId) +
                            " FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i") +
                            GenerateWhereClauseLong("PUB_a_ar_invoice_detail", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceDetailTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = ? AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArInvoiceDetailTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArDiscountTable.TableId, new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDiscountTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDiscountTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArDiscountTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines which discount applies to which category to limit the options in the UI
    public class AArDiscountPerCategoryAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArDiscountPerCategory";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_discount_per_category";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArDiscountPerCategoryTable.TableId) + " FROM PUB_a_ar_discount_per_category") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDiscountPerCategoryTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArDiscountPerCategoryTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArDiscountPerCategoryTable.TableId) + " FROM PUB_a_ar_discount_per_category" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArDiscountPerCategoryTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArDiscountPerCategoryTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArDiscountPerCategoryTable.TableId, ADataSet, new System.Object[2]{AArCategoryCode, AArDiscountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, AArDiscountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, AArDiscountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountPerCategoryTable AData, String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadByPrimaryKey(AArDiscountPerCategoryTable.TableId, AData, new System.Object[2]{AArCategoryCode, AArDiscountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountPerCategoryTable AData, String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, AArDiscountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDiscountPerCategoryTable AData, String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, AArDiscountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDiscountPerCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDiscountPerCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, AArDiscountPerCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, AArDiscountPerCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            return Exists(AArDiscountPerCategoryTable.TableId, new System.Object[2]{AArCategoryCode, AArDiscountCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDiscountPerCategoryTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArDiscountPerCategoryTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDiscountPerCategoryTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArDiscountPerCategoryTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountPerCategoryTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountPerCategoryTable AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDiscountPerCategoryTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDiscountPerCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDiscountPerCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDiscountPerCategoryTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDiscountPerCategoryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArDiscountPerCategoryTable.TableId, new System.Object[2]{AArCategoryCode, AArDiscountCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDiscountPerCategoryTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDiscountPerCategoryTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArDiscountPerCategoryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArDiscountPerCategoryTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines which discounts should be applied by default during a certain event or time period to articles from a certain category
    public class AArDefaultDiscountAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArDefaultDiscount";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_default_discount";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArDefaultDiscountTable.TableId) + " FROM PUB_a_ar_default_discount") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArDefaultDiscountTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArDefaultDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArDefaultDiscountTable.TableId) + " FROM PUB_a_ar_default_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArDefaultDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArDefaultDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArDefaultDiscountTable.TableId, ADataSet, new System.Object[3]{AArCategoryCode, AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AArCategoryCode, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDefaultDiscountTable AData, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadByPrimaryKey(AArDefaultDiscountTable.TableId, AData, new System.Object[3]{AArCategoryCode, AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDefaultDiscountTable AData, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArDefaultDiscountTable AData, String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AArCategoryCode, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDefaultDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArDefaultDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, AArDefaultDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, AArDefaultDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_default_discount", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return Exists(AArDefaultDiscountTable.TableId, new System.Object[3]{AArCategoryCode, AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_default_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDefaultDiscountTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArDefaultDiscountTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_default_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArDefaultDiscountTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArDefaultDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDefaultDiscountTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDefaultDiscountTable AData, String AArCategoryCode, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategory(out AArDefaultDiscountTable AData, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategory(out AData, AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, ADataSet, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, AData, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArCategoryTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArCategoryTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDefaultDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDefaultDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArDefaultDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArDefaultDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArDefaultDiscountTable.TableId, new System.Object[3]{AArCategoryCode, AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDefaultDiscountTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArDefaultDiscountTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArDefaultDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArDefaultDiscountTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// the invoice (which is also an offer at a certain stage)
    public class AArInvoiceAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArInvoice";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_invoice";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArInvoiceTable.TableId) + " FROM PUB_a_ar_invoice") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArInvoiceTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArInvoiceTable.TableId) + " FROM PUB_a_ar_invoice" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArInvoiceTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadByPrimaryKey(AArInvoiceTable.TableId, AData, new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceTable AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadUsingTemplate(AArInvoiceTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadUsingTemplate(AArInvoiceTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return Exists(AArInvoiceTable.TableId, new System.Object[2]{ALedgerNumber, AKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArInvoiceTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArInvoiceTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out AArInvoiceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(out AArInvoiceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(out AArInvoiceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet ADataSet, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet ADataSet, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_special_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_special_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceTable AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_special_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_special_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_special_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_special_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_special_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_special_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_special_tax_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrency(out AArInvoiceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArInvoiceTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArInvoiceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, AArInvoiceTable.TableId) +
                            " FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, AArInvoiceTable.TableId) +
                            " FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, AArInvoiceTable.TableId) +
                            " FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArInvoiceTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount WHERE " +
                        "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", AArInvoiceTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArDiscountTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArInvoiceTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArInvoiceTable.TableId, new System.Object[2]{ALedgerNumber, AKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArInvoiceTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_ar_invoice", "a_key_i");
        }
    }

    /// an invoice consists of one or more details
    public class AArInvoiceDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArInvoiceDetail";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_invoice_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArInvoiceDetailTable.TableId) + " FROM PUB_a_ar_invoice_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArInvoiceDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArInvoiceDetailTable.TableId) + " FROM PUB_a_ar_invoice_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArInvoiceDetailTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadByPrimaryKey(AArInvoiceDetailTable.TableId, AData, new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return Exists(AArInvoiceDetailTable.TableId, new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArInvoiceDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArInvoiceDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet ADataSet, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTable(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTable(out AData, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet ADataSet, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, AData, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTableTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTableTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AArInvoiceDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceDetailTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceDetailTable AData, String ATaxTypeCode, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxType(out AArInvoiceDetailTable AData, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxType(out AData, ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, ADataSet, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, AData, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaATaxTypeTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaATaxTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrency(out AArInvoiceDetailTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArInvoiceDetailTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out AArInvoiceDetailTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArInvoiceDetailTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArInvoiceDetailTable AData, String AArArticleCode, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticle(out AArInvoiceDetailTable AData, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticle(out AData, AArArticleCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, ADataSet, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, AData, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticleTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticleTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(DataSet ADataSet, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, ADataSet, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(DataSet AData, String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePrice(AData, AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(DataSet AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePrice(AData, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(out AArInvoiceDetailTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, AData, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(out AArInvoiceDetailTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePrice(out AData, AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(out AArInvoiceDetailTable AData, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePrice(out AData, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet ADataSet, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, ADataSet, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet AData, AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet AData, AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, AData, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, ADataSet, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, AData, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArArticlePriceTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArArticlePriceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArArticlePrice(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                new System.Object[2]{AArArticleCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArArticlePriceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, AArInvoiceDetailTable.TableId) +
                            " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDetailTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, AArInvoiceDetailTable.TableId) +
                            " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDetailTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArDiscountTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, AArInvoiceDetailTable.TableId) +
                            " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d") +
                            GenerateWhereClauseLong("PUB_a_ar_discount", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDetailTable.TableId), ATransaction,
                            GetParametersForWhereClause(AArInvoiceDetailTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", AArInvoiceDetailTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(AArDiscountTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                        "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" +
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AArInvoiceDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArInvoiceDetailTable.TableId, new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArInvoiceDetailTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines which discounts apply directly to the invoice rather than the invoice items; this can depend on the customer etc
    public class AArInvoiceDiscountAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArInvoiceDiscount";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_invoice_discount";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArInvoiceDiscountTable.TableId) + " FROM PUB_a_ar_invoice_discount") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDiscountTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArInvoiceDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArInvoiceDiscountTable.TableId) + " FROM PUB_a_ar_invoice_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArInvoiceDiscountTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadByPrimaryKey(AArInvoiceDiscountTable.TableId, AData, new System.Object[4]{ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, AArInvoiceDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, AArInvoiceDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            return Exists(AArInvoiceDiscountTable.TableId, new System.Object[4]{ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDiscountTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArInvoiceDiscountTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDiscountTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArInvoiceDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoice(out AArInvoiceDiscountTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArInvoiceDiscountTable.TableId, new System.Object[4]{ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDiscountTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDiscountTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArInvoiceDiscountTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// defines which discounts apply one invoice item
    public class AArInvoiceDetailDiscountAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AArInvoiceDetailDiscount";

        /// original table name in database
        public const string DBTABLENAME = "a_ar_invoice_detail_discount";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AArInvoiceDetailDiscountTable.TableId) + " FROM PUB_a_ar_invoice_detail_discount") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AArInvoiceDetailDiscountTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AArInvoiceDetailDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AArInvoiceDetailDiscountTable.TableId) + " FROM PUB_a_ar_invoice_detail_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDetailDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AArInvoiceDetailDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AArInvoiceDetailDiscountTable.TableId, ADataSet, new System.Object[5]{ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadByPrimaryKey(AArInvoiceDetailDiscountTable.TableId, AData, new System.Object[5]{ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDetailDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AArInvoiceDetailDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            return Exists(AArInvoiceDetailDiscountTable.TableId, new System.Object[5]{ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDetailDiscountTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AArInvoiceDetailDiscountTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AArInvoiceDetailDiscountTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AArInvoiceDetailDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(out AArInvoiceDetailDiscountTable AData, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetail(out AData, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet ADataSet, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetailTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceDetailTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscount(out AArInvoiceDetailDiscountTable AData, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscount(out AData, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, ADataSet, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, AData, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArDiscountTemplate(out AArInvoiceDetailDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArDiscountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AArInvoiceDetailDiscountTable.TableId, new System.Object[5]{ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDetailDiscountTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AArInvoiceDetailDiscountTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDetailDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AArInvoiceDetailDiscountTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}