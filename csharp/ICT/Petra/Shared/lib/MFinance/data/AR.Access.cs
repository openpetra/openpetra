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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_tax_type_code_c"}) + " FROM PUB_a_tax_type")
                            + GenerateOrderByClause(AOrderBy)), ATaxTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_tax_type_code_c"}) + " FROM PUB_a_tax_type WHERE a_tax_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), ATaxTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ATaxTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_tax_type_code_c"}) + " FROM PUB_a_tax_type")
                            + GenerateWhereClause(ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ATaxTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_type", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_type WHERE a_tax_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_type" + GenerateWhereClause(ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_tax_type", AFieldList, new string[] {
                            "a_tax_type_code_c"}) + " FROM PUB_a_tax_type, PUB_a_tax_table WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = ?")
                            + GenerateOrderByClause(AOrderBy)), ATaxTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_tax_type", AFieldList, new string[] {
                            "a_tax_type_code_c"}) + " FROM PUB_a_tax_type, PUB_a_tax_table, PUB_a_ledger WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c AND PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i")
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ATaxTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_tax_table", ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_tax_type WHERE a_tax_type_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_tax_type" + GenerateWhereClause(ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(ATaxTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_tax_type", ATaxTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_tax_type", ATaxTypeTable.GetColumnStringList(), ATaxTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_tax_type", ATaxTypeTable.GetColumnStringList(), ATaxTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ATaxType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table")
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table WHERE a_ledger_number_i = ? AND a_tax_type_code_c = ? AND a_tax_rate_code_c = ? AND a_tax_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table")
                            + GenerateWhereClause(ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_table", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_table WHERE a_ledger_number_i = ? AND a_tax_type_code_c = ? AND a_tax_rate_code_c = ? AND a_tax_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_table" + GenerateWhereClause(ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table WHERE a_ledger_number_i = ?")
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_tax_table", AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table, PUB_a_ledger WHERE " +
                            "PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i")
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_table WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_table, PUB_a_ledger WHERE " +
                "PUB_a_tax_table.a_ledger_number_i = PUB_a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table WHERE a_tax_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxType(FillDataSet, ATaxTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_tax_table", AFieldList, new string[] {
                            "a_ledger_number_i", "a_tax_type_code_c", "a_tax_rate_code_c", "a_tax_valid_from_d"}) + " FROM PUB_a_tax_table, PUB_a_tax_type WHERE " +
                            "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c")
                            + GenerateWhereClauseLong("PUB_a_tax_type", ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ATaxTableTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new ATaxTableTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_tax_table WHERE a_tax_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_tax_table, PUB_a_tax_type WHERE " +
                "PUB_a_tax_table.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c" + GenerateWhereClauseLong("PUB_a_tax_type", ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ATaxTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_tax_table WHERE a_ledger_number_i = ? AND a_tax_type_code_c = ? AND a_tax_rate_code_c = ? AND a_tax_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_tax_table" + GenerateWhereClause(ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(ATaxTableTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_tax_table", ATaxTableTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_tax_table", ATaxTableTable.GetColumnStringList(), ATaxTableTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_tax_table", ATaxTableTable.GetColumnStringList(), ATaxTableTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ATaxTable", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c"}) + " FROM PUB_a_ar_category")
                            + GenerateOrderByClause(AOrderBy)), AArCategoryTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c"}) + " FROM PUB_a_ar_category WHERE a_ar_category_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArCategoryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c"}) + " FROM PUB_a_ar_category")
                            + GenerateWhereClause(AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArCategoryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_category", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_category WHERE a_ar_category_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_category" + GenerateWhereClause(AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_category", AFieldList, new string[] {
                            "a_ar_category_code_c"}) + " FROM PUB_a_ar_category, PUB_a_ar_default_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = ? AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArCategoryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_category", AFieldList, new string[] {
                            "a_ar_category_code_c"}) + " FROM PUB_a_ar_category, PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c AND PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArCategoryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_category WHERE a_ar_category_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_category" + GenerateWhereClause(AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArCategoryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_category", AArCategoryTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_category", AArCategoryTable.GetColumnStringList(), AArCategoryTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_category", AArCategoryTable.GetColumnStringList(), AArCategoryTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArCategory", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article")
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article WHERE a_ar_article_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArArticleCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article")
                            + GenerateWhereClause(AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArArticleCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article WHERE a_ar_article_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article" + GenerateWhereClause(AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article WHERE a_ar_category_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategory(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_article", AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_article.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategoryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article WHERE a_ar_category_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article, PUB_a_ar_category WHERE " +
                "PUB_a_ar_article.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c" + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArCategoryTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaATaxType(DataSet ADataSet, String ATaxTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article WHERE a_tax_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxType(FillDataSet, ATaxTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_article", AFieldList, new string[] {
                            "a_ar_article_code_c"}) + " FROM PUB_a_ar_article, PUB_a_tax_type WHERE " +
                            "PUB_a_ar_article.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c")
                            + GenerateWhereClauseLong("PUB_a_tax_type", ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticleTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticleTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaATaxType(String ATaxTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ATaxTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article WHERE a_tax_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaATaxTypeTemplate(ATaxTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article, PUB_a_tax_type WHERE " +
                "PUB_a_ar_article.a_tax_type_code_c = PUB_a_tax_type.a_tax_type_code_c" + GenerateWhereClauseLong("PUB_a_tax_type", ATaxTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ATaxTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArArticleCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_article WHERE a_ar_article_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_article" + GenerateWhereClause(AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArArticleTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_article", AArArticleTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_article", AArArticleTable.GetColumnStringList(), AArArticleTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_article", AArArticleTable.GetColumnStringList(), AArArticleTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArArticle", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price")
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price WHERE a_ar_article_code_c = ? AND a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price")
                            + GenerateWhereClause(AArArticlePriceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article_price", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article_price WHERE a_ar_article_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article_price" + GenerateWhereClause(AArArticlePriceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price WHERE a_ar_article_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticle(FillDataSet, AArArticleCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_article_price", AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price, PUB_a_ar_article WHERE " +
                            "PUB_a_ar_article_price.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticleTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article_price WHERE a_ar_article_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article_price, PUB_a_ar_article WHERE " +
                "PUB_a_ar_article_price.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c" + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArArticleTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price WHERE a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_article_price", AFieldList, new string[] {
                            "a_ar_article_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_article_price, PUB_a_currency WHERE " +
                            "PUB_a_ar_article_price.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArArticlePriceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArArticlePriceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_article_price WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_article_price, PUB_a_currency WHERE " +
                "PUB_a_ar_article_price.a_currency_code_c = PUB_a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_article_price WHERE a_ar_article_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_article_price" + GenerateWhereClause(AArArticlePriceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArArticlePriceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_article_price", AArArticlePriceTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_article_price", AArArticlePriceTable.GetColumnStringList(), AArArticlePriceTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_article_price", AArArticlePriceTable.GetColumnStringList(), AArArticlePriceTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArArticlePrice", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount")
                            + GenerateWhereClause(AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount" + GenerateWhereClause(AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount WHERE a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_currency WHERE " +
                            "PUB_a_ar_discount.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_currency WHERE " +
                "PUB_a_ar_discount.a_currency_code_c = PUB_a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaPType(DataSet ADataSet, String ATypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ATypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount WHERE p_partner_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPType(FillDataSet, ATypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_p_type WHERE " +
                            "PUB_a_ar_discount.p_partner_type_code_c = PUB_p_type.p_type_code_c")
                            + GenerateWhereClauseLong("PUB_p_type", PTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaPType(String ATypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ATypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount WHERE p_partner_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPTypeTemplate(PTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_p_type WHERE " +
                "PUB_a_ar_discount.p_partner_type_code_c = PUB_p_type.p_type_code_c" + GenerateWhereClauseLong("PUB_p_type", PTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount WHERE a_ar_article_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticle(FillDataSet, AArArticleCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_article WHERE " +
                            "PUB_a_ar_discount.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticleTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount WHERE a_ar_article_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount, PUB_a_ar_article WHERE " +
                "PUB_a_ar_discount.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c" + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArArticleTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_default_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_default_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArCategoryTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_discount.a_invoice_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i")
                            + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceTable.GetPrimKeyColumnOrdList())));
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = ? AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = ? AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount", AFieldList, new string[] {
                            "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_discount, PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d AND PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i")
                            + GenerateWhereClauseLong("PUB_a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceDetailTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_discount WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_discount" + GenerateWhereClause(AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_discount", AArDiscountTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_discount", AArDiscountTable.GetColumnStringList(), AArDiscountTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_discount", AArDiscountTable.GetColumnStringList(), AArDiscountTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArDiscount", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArCategoryCode, AArDiscountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category")
                            + GenerateWhereClause(AArDiscountPerCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category" + GenerateWhereClause(AArDiscountPerCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category WHERE a_ar_category_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategory(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount_per_category", AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_discount_per_category.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategoryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category WHERE a_ar_category_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category, PUB_a_ar_category WHERE " +
                "PUB_a_ar_discount_per_category.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c" + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArCategoryTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_discount_per_category", AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c"}) + " FROM PUB_a_ar_discount_per_category, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_discount_per_category.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_discount_per_category.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDiscountPerCategoryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDiscountPerCategoryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_discount_per_category, PUB_a_ar_discount WHERE " +
                "PUB_a_ar_discount_per_category.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_discount_per_category.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_discount_per_category WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDiscountPerCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_discount_per_category" + GenerateWhereClause(AArDiscountPerCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArDiscountPerCategoryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_discount_per_category", AArDiscountPerCategoryTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_discount_per_category", AArDiscountPerCategoryTable.GetColumnStringList(), AArDiscountPerCategoryTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_discount_per_category", AArDiscountPerCategoryTable.GetColumnStringList(), AArDiscountPerCategoryTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArDiscountPerCategory", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount")
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[2].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AArCategoryCode, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount")
                            + GenerateWhereClause(AArDefaultDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_default_discount", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[2].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_default_discount WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_default_discount" + GenerateWhereClause(AArDefaultDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArCategory(DataSet ADataSet, String AArCategoryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount WHERE a_ar_category_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategory(FillDataSet, AArCategoryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_default_discount", AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                            "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArCategoryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArCategory(String AArCategoryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_default_discount WHERE a_ar_category_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArCategoryTemplate(AArCategoryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_default_discount, PUB_a_ar_category WHERE " +
                "PUB_a_ar_default_discount.a_ar_category_code_c = PUB_a_ar_category.a_ar_category_code_c" + GenerateWhereClauseLong("PUB_a_ar_category", AArCategoryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArCategoryTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_default_discount", AFieldList, new string[] {
                            "a_ar_category_code_c", "a_ar_discount_code_c", "a_ar_date_valid_from_d"}) + " FROM PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArDefaultDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArDefaultDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_default_discount WHERE a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_default_discount, PUB_a_ar_discount WHERE " +
                "PUB_a_ar_default_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_default_discount.a_ar_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AArCategoryCode, String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArCategoryCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[1].Value = ((object)(AArDiscountCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[2].Value = ((object)(AArDateValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_default_discount WHERE a_ar_category_code_c = ? AND a_ar_discount_code_c = ? AND a_ar_date_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArDefaultDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_default_discount" + GenerateWhereClause(AArDefaultDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArDefaultDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_default_discount", AArDefaultDiscountTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_default_discount", AArDefaultDiscountTable.GetColumnStringList(), AArDefaultDiscountTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_default_discount", AArDefaultDiscountTable.GetColumnStringList(), AArDefaultDiscountTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArDefaultDiscount", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice WHERE a_ledger_number_i = ? AND a_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice")
                            + GenerateWhereClause(AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice WHERE a_ledger_number_i = ? AND a_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice" + GenerateWhereClause(AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice WHERE p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice, PUB_p_partner WHERE " +
                            "PUB_a_ar_invoice.p_partner_key_n = PUB_p_partner.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_p_partner WHERE " +
                "PUB_a_ar_invoice.p_partner_key_n = PUB_p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet ADataSet, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice WHERE a_ledger_number_i = ? AND a_special_tax_type_code_c = ? AND a_special_tax_rate_code_c = ? AND a_special_tax_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTable(FillDataSet, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice, PUB_a_tax_table WHERE " +
                            "PUB_a_ar_invoice.a_ledger_number_i = PUB_a_tax_table.a_ledger_number_i AND PUB_a_ar_invoice.a_special_tax_type_code_c = PUB_a_tax_table.a_tax_type_code_c AND PUB_a_ar_invoice.a_special_tax_rate_code_c = PUB_a_tax_table.a_tax_rate_code_c AND PUB_a_ar_invoice.a_special_tax_valid_from_d = PUB_a_tax_table.a_tax_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_tax_table", ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTableTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice WHERE a_ledger_number_i = ? AND a_special_tax_type_code_c = ? AND a_special_tax_rate_code_c = ? AND a_special_tax_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_a_tax_table WHERE " +
                "PUB_a_ar_invoice.a_ledger_number_i = PUB_a_tax_table.a_ledger_number_i AND PUB_a_ar_invoice.a_special_tax_type_code_c = PUB_a_tax_table.a_tax_type_code_c AND PUB_a_ar_invoice.a_special_tax_rate_code_c = PUB_a_tax_table.a_tax_rate_code_c AND PUB_a_ar_invoice.a_special_tax_valid_from_d = PUB_a_tax_table.a_tax_valid_from_d" + GenerateWhereClauseLong("PUB_a_tax_table", ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ATaxTableTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice WHERE a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice, PUB_a_currency WHERE " +
                            "PUB_a_ar_invoice.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice, PUB_a_currency WHERE " +
                "PUB_a_ar_invoice.a_currency_code_c = PUB_a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice", AFieldList, new string[] {
                            "a_ledger_number_i", "a_key_i"}) + " FROM PUB_a_ar_invoice, PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i AND PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_invoice_discount", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_invoice WHERE a_ledger_number_i = ? AND a_key_i = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_invoice" + GenerateWhereClause(AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        ((AArInvoiceRow)(TheRow)).Key = ((Int32)(DBAccess.GDBAccessObj.GetNextSequenceValue("seq_ar_invoice", ATransaction)));
                        TTypedDataAccess.InsertRow("a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_invoice", AArInvoiceTable.GetColumnStringList(), AArInvoiceTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_invoice", AArInvoiceTable.GetColumnStringList(), AArInvoiceTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArInvoice", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail")
                            + GenerateWhereClause(AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail" + GenerateWhereClause(AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_invoice_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoice(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice WHERE " +
                            "PUB_a_ar_invoice_detail.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_detail.a_invoice_key_i = PUB_a_ar_invoice.a_key_i")
                            + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_invoice_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice WHERE " +
                "PUB_a_ar_invoice_detail.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_detail.a_invoice_key_i = PUB_a_ar_invoice.a_key_i" + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaATaxTable(DataSet ADataSet, Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_tax_type_code_c = ? AND a_tax_rate_code_c = ? AND a_tax_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTable(FillDataSet, ALedgerNumber, ATaxTypeCode, ATaxRateCode, ATaxValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_tax_table WHERE " +
                            "PUB_a_ar_invoice_detail.a_ledger_number_i = PUB_a_tax_table.a_ledger_number_i AND PUB_a_ar_invoice_detail.a_tax_type_code_c = PUB_a_tax_table.a_tax_type_code_c AND PUB_a_ar_invoice_detail.a_tax_rate_code_c = PUB_a_tax_table.a_tax_rate_code_c AND PUB_a_ar_invoice_detail.a_tax_valid_from_d = PUB_a_tax_table.a_tax_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_tax_table", ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaATaxTableTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaATaxTable(Int32 ALedgerNumber, String ATaxTypeCode, String ATaxRateCode, System.DateTime ATaxValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ATaxTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ATaxRateCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(ATaxValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_tax_type_code_c = ? AND a_tax_rate_code_c = ? AND a_tax_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaATaxTableTemplate(ATaxTableRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_tax_table WHERE " +
                "PUB_a_ar_invoice_detail.a_ledger_number_i = PUB_a_tax_table.a_ledger_number_i AND PUB_a_ar_invoice_detail.a_tax_type_code_c = PUB_a_tax_table.a_tax_type_code_c AND PUB_a_ar_invoice_detail.a_tax_rate_code_c = PUB_a_tax_table.a_tax_rate_code_c AND PUB_a_ar_invoice_detail.a_tax_valid_from_d = PUB_a_tax_table.a_tax_valid_from_d" + GenerateWhereClauseLong("PUB_a_tax_table", ATaxTableTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ATaxTableTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_currency WHERE " +
                            "PUB_a_ar_invoice_detail.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_currency WHERE " +
                "PUB_a_ar_invoice_detail.a_currency_code_c = PUB_a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArArticle(DataSet ADataSet, String AArArticleCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_ar_article_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticle(FillDataSet, AArArticleCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_ar_article WHERE " +
                            "PUB_a_ar_invoice_detail.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c")
                            + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticleTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArArticle(String AArArticleCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_ar_article_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArArticleTemplate(AArArticleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_article WHERE " +
                "PUB_a_ar_invoice_detail.a_ar_article_code_c = PUB_a_ar_article.a_ar_article_code_c" + GenerateWhereClauseLong("PUB_a_ar_article", AArArticleTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArArticleTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArArticlePrice(DataSet ADataSet, String AArArticleCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail WHERE a_ar_article_code_c = ? AND a_ar_article_price_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticlePrice(FillDataSet, AArArticleCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_ar_article_price WHERE " +
                            "PUB_a_ar_invoice_detail.a_ar_article_code_c = PUB_a_ar_article_price.a_ar_article_code_c AND PUB_a_ar_invoice_detail.a_ar_article_price_d = PUB_a_ar_article_price.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_article_price", AArArticlePriceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArArticlePriceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArArticlePrice(String AArArticleCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArArticleCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail WHERE a_ar_article_code_c = ? AND a_ar_article_price_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArArticlePriceTemplate(AArArticlePriceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail, PUB_a_ar_article_price WHERE " +
                "PUB_a_ar_invoice_detail.a_ar_article_code_c = PUB_a_ar_article_price.a_ar_article_code_c AND PUB_a_ar_invoice_detail.a_ar_article_price_d = PUB_a_ar_article_price.a_ar_date_valid_from_d" + GenerateWhereClauseLong("PUB_a_ar_article_price", AArArticlePriceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArArticlePriceTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = ? AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i"}) + " FROM PUB_a_ar_invoice_detail, PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i AND PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
                        GenerateWhereClauseLong("PUB_a_ar_invoice_detail_discount", AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_invoice_detail WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_invoice_detail" + GenerateWhereClause(AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), AArInvoiceDetailTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), AArInvoiceDetailTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArInvoiceDetail", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[2].Value = ((object)(AArDiscountCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(AArDiscountDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AInvoiceKey, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount")
                            + GenerateWhereClause(AArInvoiceDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[2].Value = ((object)(AArDiscountCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(AArDiscountDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount" + GenerateWhereClause(AArInvoiceDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoice(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_discount", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                            "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i")
                            + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount, PUB_a_ar_invoice WHERE " +
                "PUB_a_ar_invoice_discount.a_ledger_number_i = PUB_a_ar_invoice.a_ledger_number_i AND PUB_a_ar_invoice_discount.a_invoice_key_i = PUB_a_ar_invoice.a_key_i" + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount WHERE a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_discount", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount WHERE a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_discount, PUB_a_ar_discount WHERE " +
                "PUB_a_ar_invoice_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[2].Value = ((object)(AArDiscountCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[3].Value = ((object)(AArDiscountDateValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_invoice_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_invoice_discount" + GenerateWhereClause(AArInvoiceDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_invoice_discount", AArInvoiceDiscountTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_invoice_discount", AArInvoiceDiscountTable.GetColumnStringList(), AArInvoiceDiscountTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_invoice_discount", AArInvoiceDiscountTable.GetColumnStringList(), AArInvoiceDiscountTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArInvoiceDiscount", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[3].Value = ((object)(AArDiscountCode));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[4].Value = ((object)(AArDiscountDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AInvoiceKey, ADetailNumber, AArDiscountCode, AArDiscountDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount")
                            + GenerateWhereClause(AArInvoiceDetailDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[3].Value = ((object)(AArDiscountCode));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[4].Value = ((object)(AArDiscountDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount" + GenerateWhereClause(AArInvoiceDetailDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// auto generated
        public static void LoadViaAArInvoiceDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceDetail(FillDataSet, ALedgerNumber, AInvoiceKey, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail_discount", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i")
                            + GenerateWhereClauseLong("PUB_a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArInvoiceDetail(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArInvoiceDetailTemplate(AArInvoiceDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount, PUB_a_ar_invoice_detail WHERE " +
                "PUB_a_ar_invoice_detail_discount.a_ledger_number_i = PUB_a_ar_invoice_detail.a_ledger_number_i AND PUB_a_ar_invoice_detail_discount.a_invoice_key_i = PUB_a_ar_invoice_detail.a_invoice_key_i AND PUB_a_ar_invoice_detail_discount.a_detail_number_i = PUB_a_ar_invoice_detail.a_detail_number_i" + GenerateWhereClauseLong("PUB_a_ar_invoice_detail", AArInvoiceDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceDetailTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void LoadViaAArDiscount(DataSet ADataSet, String AArDiscountCode, System.DateTime AArDateValidFrom, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount WHERE a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?")
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscount(FillDataSet, AArDiscountCode, AArDateValidFrom, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ar_invoice_detail_discount", AFieldList, new string[] {
                            "a_ledger_number_i", "a_invoice_key_i", "a_detail_number_i", "a_ar_discount_code_c", "a_ar_discount_date_valid_from_d"}) + " FROM PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                            "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d")
                            + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AArInvoiceDetailDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
            DataSet FillDataSet = new DataSet();
            AData = new AArInvoiceDetailDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArDiscountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
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
        public static int CountViaAArDiscount(String AArDiscountCode, System.DateTime AArDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[0].Value = ((object)(AArDiscountCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AArDateValidFrom));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount WHERE a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaAArDiscountTemplate(AArDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ar_invoice_detail_discount, PUB_a_ar_discount WHERE " +
                "PUB_a_ar_invoice_detail_discount.a_ar_discount_code_c = PUB_a_ar_discount.a_ar_discount_code_c AND PUB_a_ar_invoice_detail_discount.a_ar_discount_date_valid_from_d = PUB_a_ar_discount.a_ar_date_valid_from_d" + GenerateWhereClauseLong("PUB_a_ar_discount", AArDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArDiscountTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AInvoiceKey, Int32 ADetailNumber, String AArDiscountCode, System.DateTime AArDiscountDateValidFrom, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AInvoiceKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 100);
            ParametersArray[3].Value = ((object)(AArDiscountCode));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[4].Value = ((object)(AArDiscountDateValidFrom));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ar_invoice_detail_discount WHERE a_ledger_number_i = ? AND a_invoice_key_i = ? AND a_detail_number_i = ? AND a_ar_discount_code_c = ? AND a_ar_discount_date_valid_from_d = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AArInvoiceDetailDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ar_invoice_detail_discount" + GenerateWhereClause(AArInvoiceDetailDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static bool SubmitChanges(AArInvoiceDetailDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ar_invoice_detail_discount", AArInvoiceDetailDiscountTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ar_invoice_detail_discount", AArInvoiceDetailDiscountTable.GetColumnStringList(), AArInvoiceDetailDiscountTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ar_invoice_detail_discount", AArInvoiceDetailDiscountTable.GetColumnStringList(), AArInvoiceDetailDiscountTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AArInvoiceDetailDiscount", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}