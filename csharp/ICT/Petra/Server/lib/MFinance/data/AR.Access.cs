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
namespace Ict.Petra.Server.MFinance.AR.Data.Access
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
        public static ATaxTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTypeTable Data = new ATaxTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ATaxTypeTable.TableId) + " FROM PUB_a_tax_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadByPrimaryKey(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTypeTable Data = new ATaxTypeTable();
            LoadByPrimaryKey(ATaxTypeTable.TableId, Data, new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadByPrimaryKey(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadByPrimaryKey(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTypeTable Data = new ATaxTypeTable();
            LoadUsingTemplate(ATaxTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadUsingTemplate(ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTypeTable Data = new ATaxTypeTable();
            LoadUsingTemplate(ATaxTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ATaxTypeTable Data = new ATaxTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ATaxTypeTable Data = new ATaxTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTypeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ATaxTypeTable Data = new ATaxTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaALedgerTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTypeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ATaxTableTable.TableId) + " FROM PUB_a_tax_table" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadByPrimaryKey(ATaxTableTable.TableId, Data, new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadUsingTemplate(ATaxTableTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadUsingTemplate(ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadUsingTemplate(ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadUsingTemplate(ATaxTableTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static ATaxTableTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ATaxTableTable Data = new ATaxTableTable();
            LoadViaForeignKey(ATaxTableTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ATaxTableTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArCategoryTable Data = new AArCategoryTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArCategoryTable.TableId) + " FROM PUB_a_ar_category" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadByPrimaryKey(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArCategoryTable Data = new AArCategoryTable();
            LoadByPrimaryKey(AArCategoryTable.TableId, Data, new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadByPrimaryKey(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadByPrimaryKey(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArCategoryTable Data = new AArCategoryTable();
            LoadUsingTemplate(AArCategoryTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadUsingTemplate(AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadUsingTemplate(AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArCategoryTable Data = new AArCategoryTable();
            LoadUsingTemplate(AArCategoryTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArCategoryTable Data = new AArCategoryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArCategoryTable Data = new AArCategoryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArCategoryTable Data = new AArCategoryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArArticleTable.TableId) + " FROM PUB_a_ar_article" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadByPrimaryKey(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadByPrimaryKey(AArArticleTable.TableId, Data, new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadByPrimaryKey(String AArArticleCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadByPrimaryKey(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArArticleCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadUsingTemplate(AArArticleTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadUsingTemplate(AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadUsingTemplate(AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadUsingTemplate(AArArticleTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticleTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticleTable Data = new AArArticleTable();
            LoadViaForeignKey(AArArticleTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticleTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArArticlePriceTable.TableId) + " FROM PUB_a_ar_article_price" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadByPrimaryKey(AArArticlePriceTable.TableId, Data, new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadUsingTemplate(AArArticlePriceTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadUsingTemplate(AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadUsingTemplate(AArArticlePriceTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArArticlePriceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArArticlePriceTable Data = new AArArticlePriceTable();
            LoadViaForeignKey(AArArticlePriceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArArticlePriceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArDiscountTable.TableId) + " FROM PUB_a_ar_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadByPrimaryKey(AArDiscountTable.TableId, Data, new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadUsingTemplate(AArDiscountTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadUsingTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadUsingTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadUsingTemplate(AArDiscountTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaPType(String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, Data, new string[1]{"p_partner_type_code_c"},
                new System.Object[1]{ATypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaPType(String ATypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPType(ATypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaPType(String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPType(ATypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaPTypeTemplate(PTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, Data, new string[1]{"p_partner_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaPTypeTemplate(PTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaPTypeTemplate(PTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaPTypeTemplate(PTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaPTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, PTypeTable.TableId, Data, new string[1]{"p_partner_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaPTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaPTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountTable Data = new AArDiscountTable();
            LoadViaForeignKey(AArDiscountTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArCategory(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArCategoryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArCategoryTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoice(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoiceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoiceTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoiceDetail(FillDataSet, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetail(ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetail(ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoiceDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArDiscountTable Data = new AArDiscountTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArInvoiceDetailTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArDiscountPerCategoryTable.TableId) + " FROM PUB_a_ar_discount_per_category" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadByPrimaryKey(AArDiscountPerCategoryTable.TableId, Data, new System.Object[2]{AArCategoryCode, AArDiscountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, AArDiscountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, AArDiscountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadUsingTemplate(AArDiscountPerCategoryTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDiscountPerCategoryTable Data = new AArDiscountPerCategoryTable();
            LoadViaForeignKey(AArDiscountPerCategoryTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDiscountPerCategoryTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArDefaultDiscountTable.TableId) + " FROM PUB_a_ar_default_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadByPrimaryKey(AArDefaultDiscountTable.TableId, Data, new System.Object[3]{AArCategoryCode, AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AArCategoryCode, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadUsingTemplate(AArDefaultDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadUsingTemplate(AArDefaultDiscountTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                new System.Object[1]{AArCategoryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategory(String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategory(AArCategoryCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArCategoryTable.TableId, Data, new string[1]{"a_ar_category_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArCategoryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArCategoryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArDefaultDiscountTable Data = new AArDefaultDiscountTable();
            LoadViaForeignKey(AArDefaultDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArDefaultDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArInvoiceTable.TableId) + " FROM PUB_a_ar_invoice" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadByPrimaryKey(AArInvoiceTable.TableId, Data, new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadUsingTemplate(AArInvoiceTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadUsingTemplate(AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadUsingTemplate(AArInvoiceTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_special_tax_type_code_c", "a_special_tax_rate_code_c", "a_special_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_special_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_special_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_special_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceTable Data = new AArInvoiceTable();
            LoadViaForeignKey(AArInvoiceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceTable Data = new AArInvoiceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceTable Data = new AArInvoiceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceTable Data = new AArInvoiceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArInvoiceDetailTable.TableId) + " FROM PUB_a_ar_invoice_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadByPrimaryKey(AArInvoiceDetailTable.TableId, Data, new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadUsingTemplate(AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadUsingTemplate(AArInvoiceDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                new System.Object[4]{ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTable(ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTableTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTableTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTableTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                new System.Object[1]{ATaxTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxType(String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxType(ATaxTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ATaxTypeTable.TableId, Data, new string[1]{"a_tax_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaATaxTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaATaxTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                new System.Object[1]{AArArticleCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticle(String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticle(AArArticleCode, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticleTable.TableId, Data, new string[1]{"a_ar_article_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticleTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticlePrice(String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, Data, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                new System.Object[2]{AArArticleCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePrice(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePrice(AArArticleCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePrice(String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePrice(AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, Data, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePriceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePriceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePriceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            LoadViaForeignKey(AArInvoiceDetailTable.TableId, AArArticlePriceTable.TableId, Data, new string[2]{"a_ar_article_code_c", "a_ar_article_price_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePriceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArArticlePriceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArArticlePriceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AArInvoiceDetailTable Data = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaAArDiscountTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArInvoiceDiscountTable.TableId) + " FROM PUB_a_ar_invoice_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadByPrimaryKey(AArInvoiceDiscountTable.TableId, Data, new System.Object[4]{ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadUsingTemplate(AArInvoiceDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadUsingTemplate(AArInvoiceDiscountTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDiscountTable Data = new AArInvoiceDiscountTable();
            LoadViaForeignKey(AArInvoiceDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AArInvoiceDetailDiscountTable.TableId) + " FROM PUB_a_ar_invoice_detail_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadByPrimaryKey(AArInvoiceDetailDiscountTable.TableId, Data, new System.Object[5]{ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadUsingTemplate(AArInvoiceDetailDiscountTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AInvoiceKey, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetail(ALedgerNumber, AInvoiceKey, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetail(ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArInvoiceDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArInvoiceDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                new System.Object[2]{AArDiscountCode, AArDateValidFrom}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscount(AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AArInvoiceDetailDiscountTable Data = new AArInvoiceDetailDiscountTable();
            LoadViaForeignKey(AArInvoiceDetailDiscountTable.TableId, AArDiscountTable.TableId, Data, new string[2]{"a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AArInvoiceDetailDiscountTable LoadViaAArDiscountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArDiscountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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