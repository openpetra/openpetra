/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MFinance.Gift.Data.Access
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
    using Ict.Petra.Shared.MFinance.Gift.Data;
    using Ict.Petra.Shared.MSysMan.Data;
    using Ict.Petra.Shared.MFinance.Account.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MPartner.Mailroom.Data;
    
    
    /// Media"" types of money received.  Eg: Cash, Check Credit Card.
    public class AMethodOfPaymentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AMethodOfPayment";
        
        /// original table name in database
        public const string DBTABLENAME = "a_method_of_payment";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_method_of_payment_code_c"}) + " FROM PUB_a_method_of_payment") 
                            + GenerateOrderByClause(AOrderBy)), AMethodOfPaymentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AMethodOfPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMethodOfPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AMethodOfPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AMethodOfPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_method_of_payment_code_c"}) + " FROM PUB_a_method_of_payment WHERE a_method_of_payment_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMethodOfPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMethodOfPaymentTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMethodOfPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AMethodOfPaymentCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMethodOfPaymentTable AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMethodOfPaymentTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_method_of_payment_code_c"}) + " FROM PUB_a_method_of_payment") 
                            + GenerateWhereClause(AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMethodOfPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMethodOfPaymentTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMethodOfPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMethodOfPaymentTable AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMethodOfPaymentTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMethodOfPaymentTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_method_of_payment", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_method_of_payment WHERE a_method_of_payment_code_c = ?" +
                        "", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_method_of_payment" + GenerateWhereClause(AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaSFile(DataSet ADataSet, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[0].Value = ((object)(AFileName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_method_of_payment_code_c"}) + " FROM PUB_a_method_of_payment WHERE a_process_to_call_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMethodOfPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSFile(DataSet AData, String AFileName, TDBTransaction ATransaction)
        {
            LoadViaSFile(AData, AFileName, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFile(DataSet AData, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFile(AData, AFileName, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFile(out AMethodOfPaymentTable AData, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMethodOfPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSFile(FillDataSet, AFileName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSFile(out AMethodOfPaymentTable AData, String AFileName, TDBTransaction ATransaction)
        {
            LoadViaSFile(out AData, AFileName, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFile(out AMethodOfPaymentTable AData, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFile(out AData, AFileName, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(DataSet ADataSet, SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_method_of_payment", AFieldList, new string[] {
                            "a_method_of_payment_code_c"}) + " FROM PUB_a_method_of_payment, PUB_s_file WHERE a_method_of_payment.a_process_to_" +
                    "call_c = s_file.s_file_name_c") 
                            + GenerateWhereClauseLong("PUB_s_file", SFileTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMethodOfPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, SFileRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, SFileRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(out AMethodOfPaymentTable AData, SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMethodOfPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSFileTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(out AMethodOfPaymentTable AData, SFileRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(out AMethodOfPaymentTable AData, SFileRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSFileTemplate(out AMethodOfPaymentTable AData, SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaSFile(String AFileName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[0].Value = ((object)(AFileName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_method_of_payment WHERE a_process_to_call_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaSFileTemplate(SFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_method_of_payment, PUB_s_file WHERE a_method_of_paymen" +
                        "t.a_process_to_call_c = s_file.s_file_name_c" + GenerateWhereClauseLong("PUB_s_file", SFileTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SFileTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_method_of_payment WHERE a_method_of_payment_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_method_of_payment" + GenerateWhereClause(AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AMethodOfPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), AMethodOfPaymentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), AMethodOfPaymentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AMethodOfPayment", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This is used to track a partner's reason for contacting the organisation/sending money. Divided into Motivation Detail codes.
    public class AMotivationGroupAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationGroup";
        
        /// original table name in database
        public const string DBTABLENAME = "a_motivation_group";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c"}) + " FROM PUB_a_motivation_group") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationGroupTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AMotivationGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationGroupTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c"}) + " FROM PUB_a_motivation_group WHERE a_ledger_number_i = ? AND a_motivation_group_c" +
                    "ode_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationGroupTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationGroupTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationGroupTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationGroupTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c"}) + " FROM PUB_a_motivation_group") 
                            + GenerateWhereClause(AMotivationGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationGroupTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationGroupTable AData, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationGroupTable AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationGroupTable AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationGroupTable AData, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_group", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_group WHERE a_ledger_number_i = ? AND a_mot" +
                        "ivation_group_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_group" + GenerateWhereClause(AMotivationGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c"}) + " FROM PUB_a_motivation_group WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationGroupTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AMotivationGroupTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AMotivationGroupTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AMotivationGroupTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_group", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c"}) + " FROM PUB_a_motivation_group, PUB_a_ledger WHERE a_motivation_group.a_ledger_numb" +
                    "er_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationGroupTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AMotivationGroupTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationGroupTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationGroupTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationGroupTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_group WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_group, PUB_a_ledger WHERE a_motivation_grou" +
                        "p.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_motivation_group WHERE a_ledger_number_i = ? AND a_motivation_g" +
                    "roup_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_motivation_group" + GenerateWhereClause(AMotivationGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AMotivationGroupTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_motivation_group", AMotivationGroupTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_motivation_group", AMotivationGroupTable.GetColumnStringList(), AMotivationGroupTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_motivation_group", AMotivationGroupTable.GetColumnStringList(), AMotivationGroupTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AMotivationGroup", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Used as a subdvision of motivation group. Details of the reason money has been received, where it is going (cost centre and account), and fees to be charged on it.
    public class AMotivationDetailAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationDetail";
        
        /// original table name in database
        public const string DBTABLENAME = "a_motivation_detail";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AMotivationDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_motivation_group_" +
                    "code_c = ? AND a_motivation_detail_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail") 
                            + GenerateWhereClause(AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailTable AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_mo" +
                        "tivation_group_code_c = ? AND a_motivation_detail_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail" + GenerateWhereClause(AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_motivation_group_" +
                    "code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationGroup(FillDataSet, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(out AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroup(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(out AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet ADataSet, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail, PUB_a_motivation_group WHERE a_motivation_detail.a" +
                    "_ledger_number_i = a_motivation_group.a_ledger_number_i AND a_motivation_detail." +
                    "a_motivation_group_code_c = a_motivation_group.a_motivation_group_code_c") 
                            + GenerateWhereClauseLong("PUB_a_motivation_group", AMotivationGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(out AMotivationDetailTable AData, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(out AMotivationDetailTable AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(out AMotivationDetailTable AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(out AMotivationDetailTable AData, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMotivationGroup(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_mo" +
                        "tivation_group_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_a_motivation_group WHERE a_moti" +
                        "vation_detail.a_ledger_number_i = a_motivation_group.a_ledger_number_i AND a_mot" +
                        "ivation_detail.a_motivation_group_code_c = a_motivation_group.a_motivation_group" +
                        "_code_c" + GenerateWhereClauseLong("PUB_a_motivation_group", AMotivationGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMotivationGroupTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_account_code_c = " +
                    "?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AMotivationDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail, PUB_a_account WHERE a_motivation_detail.a_ledger_n" +
                    "umber_i = a_account.a_ledger_number_i AND a_motivation_detail.a_account_code_c =" +
                    " a_account.a_account_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AMotivationDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AMotivationDetailTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AMotivationDetailTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AMotivationDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_ac" +
                        "count_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_a_account WHERE a_motivation_de" +
                        "tail.a_ledger_number_i = a_account.a_ledger_number_i AND a_motivation_detail.a_a" +
                        "ccount_code_c = a_account.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_cost_centre_code_" +
                    "c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AMotivationDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentre(FillDataSet, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AMotivationDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AMotivationDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail, PUB_a_cost_centre WHERE a_motivation_detail.a_ledg" +
                    "er_number_i = a_cost_centre.a_ledger_number_i AND a_motivation_detail.a_cost_cen" +
                    "tre_code_c = a_cost_centre.a_cost_centre_code_c") 
                            + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AMotivationDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentreTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AMotivationDetailTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AMotivationDetailTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AMotivationDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_co" +
                        "st_centre_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_a_cost_centre WHERE a_motivatio" +
                        "n_detail.a_ledger_number_i = a_cost_centre.a_ledger_number_i AND a_motivation_de" +
                        "tail.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c" + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACostCentreTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE p_recipient_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out AMotivationDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AMotivationDetailTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AMotivationDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail, PUB_p_partner WHERE a_motivation_detail.p_recipien" +
                    "t_key_n = p_partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out AMotivationDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AMotivationDetailTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AMotivationDetailTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AMotivationDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE p_recipient_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_p_partner WHERE a_motivation_de" +
                        "tail.p_recipient_key_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AMotivationDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AMotivationDetailTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AMotivationDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + " FROM PUB_a_motivation_detail, PUB_a_ledger WHERE a_motivation_detail.a_ledger_nu" +
                    "mber_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AMotivationDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationDetailTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationDetailTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AMotivationDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_a_ledger WHERE a_motivation_det" +
                        "ail.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + @" FROM PUB_a_motivation_detail, PUB_s_group_motivation WHERE PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = ? AND PUB_s_group_motivation.s_group_unit_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaSGroup(out AMotivationDetailTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSGroup(out AMotivationDetailTable AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroup(out AMotivationDetailTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c"}) + @" FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n") 
                            + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaSGroupTemplate(out AMotivationDetailTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AMotivationDetailTable AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AMotivationDetailTable AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AMotivationDetailTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(@"SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_s_group_motivation WHERE PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = ? AND PUB_s_group_motivation.s_group_unit_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n" + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SGroupTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_motivation_detail WHERE a_ledger_number_i = ? AND a_motivation_" +
                    "group_code_c = ? AND a_motivation_detail_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_motivation_detail" + GenerateWhereClause(AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AMotivationDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), AMotivationDetailTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), AMotivationDetailTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AMotivationDetail", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// motivation details can have several fees
    public class AMotivationDetailFeeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationDetailFee";
        
        /// original table name in database
        public const string DBTABLENAME = "a_motivation_detail_fee";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c",
                            "a_fee_code_c"}) + " FROM PUB_a_motivation_detail_fee") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailFeeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AMotivationDetailFeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailFeeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationDetailFeeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AMotivationDetailFeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AFeeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c",
                            "a_fee_code_c"}) + " FROM PUB_a_motivation_detail_fee WHERE a_ledger_number_i = ? AND a_motivation_gr" +
                    "oup_code_c = ? AND a_motivation_detail_code_c = ? AND a_fee_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailFeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailFeeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c",
                            "a_fee_code_c"}) + " FROM PUB_a_motivation_detail_fee") 
                            + GenerateWhereClause(AMotivationDetailFeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailFeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailFeeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AFeeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee WHERE a_ledger_number_i = ? AND " +
                        "a_motivation_group_code_c = ? AND a_motivation_detail_code_c = ? AND a_fee_code_" +
                        "c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee" + GenerateWhereClause(AMotivationDetailFeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c",
                            "a_fee_code_c"}) + " FROM PUB_a_motivation_detail_fee WHERE a_ledger_number_i = ? AND a_motivation_gr" +
                    "oup_code_c = ? AND a_motivation_detail_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailFeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailFeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetail(FillDataSet, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AMotivationDetailFeeTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail_fee", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_motivation_group_code_c",
                            "a_motivation_detail_code_c",
                            "a_fee_code_c"}) + @" FROM PUB_a_motivation_detail_fee, PUB_a_motivation_detail WHERE a_motivation_detail_fee.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_motivation_detail_fee.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_motivation_detail_fee.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c") 
                            + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AMotivationDetailFeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AMotivationDetailFeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AMotivationDetailFeeTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee WHERE a_ledger_number_i = ? AND " +
                        "a_motivation_group_code_c = ? AND a_motivation_detail_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_motivation_detail_fee, PUB_a_motivation_detail WHERE a_motivation_detail_fee.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_motivation_detail_fee.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_motivation_detail_fee.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c" + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMotivationDetailTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AFeeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_motivation_detail_fee WHERE a_ledger_number_i = ? AND a_motivat" +
                    "ion_group_code_c = ? AND a_motivation_detail_code_c = ? AND a_fee_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_motivation_detail_fee" + GenerateWhereClause(AMotivationDetailFeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AMotivationDetailFeeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_motivation_detail_fee", AMotivationDetailFeeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_motivation_detail_fee", AMotivationDetailFeeTable.GetColumnStringList(), AMotivationDetailFeeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_motivation_detail_fee", AMotivationDetailFeeTable.GetColumnStringList(), AMotivationDetailFeeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AMotivationDetailFee", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Templates of gift batches which can be copied into the gift system.
    public class ARecurringGiftBatchAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGiftBatch";
        
        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift_batch";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ARecurringGiftBatchTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftBatchTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftBatchTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a_batch_number_i" +
                    " = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch") 
                            + GenerateWhereClause(ARecurringGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftBatchTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftBatchTable AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftBatchTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftBatchTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a" +
                        "_batch_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch" + GenerateWhereClause(ARecurringGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch, PUB_a_ledger WHERE a_recurring_gift_batch.a_led" +
                    "ger_number_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out ARecurringGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out ARecurringGiftBatchTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out ARecurringGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out ARecurringGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch, PUB_a_ledger WHERE a_recurring_g" +
                        "ift_batch.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a_bank_account_c" +
                    "ode_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch, PUB_a_account WHERE a_recurring_gift_batch.a_le" +
                    "dger_number_i = a_account.a_ledger_number_i AND a_recurring_gift_batch.a_bank_ac" +
                    "count_code_c = a_account.a_account_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out ARecurringGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out ARecurringGiftBatchTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out ARecurringGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out ARecurringGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a" +
                        "_bank_account_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch, PUB_a_account WHERE a_recurring_" +
                        "gift_batch.a_ledger_number_i = a_account.a_ledger_number_i AND a_recurring_gift_" +
                        "batch.a_bank_account_code_c = a_account.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a_bank_cost_cent" +
                    "re_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentre(FillDataSet, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out ARecurringGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch, PUB_a_cost_centre WHERE a_recurring_gift_batch." +
                    "a_ledger_number_i = a_cost_centre.a_ledger_number_i AND a_recurring_gift_batch.a" +
                    "_bank_cost_centre_c = a_cost_centre.a_cost_centre_code_c") 
                            + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out ARecurringGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentreTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out ARecurringGiftBatchTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out ARecurringGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out ARecurringGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a" +
                        "_bank_cost_centre_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch, PUB_a_cost_centre WHERE a_recurr" +
                        "ing_gift_batch.a_ledger_number_i = a_cost_centre.a_ledger_number_i AND a_recurri" +
                        "ng_gift_batch.a_bank_cost_centre_c = a_cost_centre.a_cost_centre_code_c" + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACostCentreTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch WHERE a_currency_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrency(out ARecurringGiftBatchTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out ARecurringGiftBatchTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out ARecurringGiftBatchTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_recurring_gift_batch, PUB_a_currency WHERE a_recurring_gift_batch.a_c" +
                    "urrency_code_c = a_currency.a_currency_code_c") 
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out ARecurringGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out ARecurringGiftBatchTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out ARecurringGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out ARecurringGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch, PUB_a_currency WHERE a_recurring" +
                        "_gift_batch.a_currency_code_c = a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_recurring_gift_batch WHERE a_ledger_number_i = ? AND a_batch_nu" +
                    "mber_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_recurring_gift_batch" + GenerateWhereClause(ARecurringGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(ARecurringGiftBatchTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_recurring_gift_batch", ARecurringGiftBatchTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_recurring_gift_batch", ARecurringGiftBatchTable.GetColumnStringList(), ARecurringGiftBatchTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_recurring_gift_batch", ARecurringGiftBatchTable.GetColumnStringList(), ARecurringGiftBatchTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ARecurringGiftBatch", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Templates of donor gift information which can be copied into the gift system with recurring gift batches.
    public class ARecurringGiftAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGift";
        
        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ARecurringGiftTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift WHERE a_ledger_number_i = ? AND a_batch_number_i = ? A" +
                    "ND a_gift_transaction_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift") 
                            + GenerateWhereClause(ARecurringGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftTable AData, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftTable AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftTable AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftTable AData, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE a_ledger_number_i = ? AND a_batch" +
                        "_number_i = ? AND a_gift_transaction_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift" + GenerateWhereClause(ARecurringGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift WHERE a_ledger_number_i = ? AND a_batch_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaARecurringGiftBatch(FillDataSet, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(out AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatch(out ARecurringGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(out AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet ADataSet, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift, PUB_a_recurring_gift_batch WHERE a_recurring_gift.a_l" +
                    "edger_number_i = a_recurring_gift_batch.a_ledger_number_i AND a_recurring_gift.a" +
                    "_batch_number_i = a_recurring_gift_batch.a_batch_number_i") 
                            + GenerateWhereClauseLong("PUB_a_recurring_gift_batch", ARecurringGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(out ARecurringGiftTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaARecurringGiftBatchTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(out ARecurringGiftTable AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(out ARecurringGiftTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(out ARecurringGiftTable AData, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaARecurringGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE a_ledger_number_i = ? AND a_batch" +
                        "_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift, PUB_a_recurring_gift_batch WHERE a_rec" +
                        "urring_gift.a_ledger_number_i = a_recurring_gift_batch.a_ledger_number_i AND a_r" +
                        "ecurring_gift.a_batch_number_i = a_recurring_gift_batch.a_batch_number_i" + GenerateWhereClauseLong("PUB_a_recurring_gift_batch", ARecurringGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ARecurringGiftBatchTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet ADataSet, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AMethodOfGivingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift WHERE a_method_of_giving_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out ARecurringGiftTable AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfGiving(FillDataSet, AMethodOfGivingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out ARecurringGiftTable AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(out AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out ARecurringGiftTable AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(out AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift, PUB_a_method_of_giving WHERE a_recurring_gift.a_metho" +
                    "d_of_giving_code_c = a_method_of_giving.a_method_of_giving_code_c") 
                            + GenerateWhereClauseLong("PUB_a_method_of_giving", AMethodOfGivingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out ARecurringGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfGivingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out ARecurringGiftTable AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out ARecurringGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out ARecurringGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AMethodOfGivingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE a_method_of_giving_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift, PUB_a_method_of_giving WHERE a_recurri" +
                        "ng_gift.a_method_of_giving_code_c = a_method_of_giving.a_method_of_giving_code_c" +
                        "" + GenerateWhereClauseLong("PUB_a_method_of_giving", AMethodOfGivingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMethodOfGivingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift WHERE a_method_of_payment_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out ARecurringGiftTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfPayment(FillDataSet, AMethodOfPaymentCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out ARecurringGiftTable AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(out AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out ARecurringGiftTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(out AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift, PUB_a_method_of_payment WHERE a_recurring_gift.a_meth" +
                    "od_of_payment_code_c = a_method_of_payment.a_method_of_payment_code_c") 
                            + GenerateWhereClauseLong("PUB_a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out ARecurringGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfPaymentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out ARecurringGiftTable AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out ARecurringGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out ARecurringGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE a_method_of_payment_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift, PUB_a_method_of_payment WHERE a_recurr" +
                        "ing_gift.a_method_of_payment_code_c = a_method_of_payment.a_method_of_payment_co" +
                        "de_c" + GenerateWhereClauseLong("PUB_a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMethodOfPaymentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift WHERE p_donor_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out ARecurringGiftTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out ARecurringGiftTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out ARecurringGiftTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_recurring_gift, PUB_p_partner WHERE a_recurring_gift.p_donor_key_n = " +
                    "p_partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out ARecurringGiftTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out ARecurringGiftTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out ARecurringGiftTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out ARecurringGiftTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift WHERE p_donor_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift, PUB_p_partner WHERE a_recurring_gift.p" +
                        "_donor_key_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_recurring_gift WHERE a_ledger_number_i = ? AND a_batch_number_i" +
                    " = ? AND a_gift_transaction_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_recurring_gift" + GenerateWhereClause(ARecurringGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(ARecurringGiftTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_recurring_gift", ARecurringGiftTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_recurring_gift", ARecurringGiftTable.GetColumnStringList(), ARecurringGiftTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_recurring_gift", ARecurringGiftTable.GetColumnStringList(), ARecurringGiftTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ARecurringGift", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Store recipient information for the recurring gift.
    public class ARecurringGiftDetailAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGiftDetail";
        
        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift_detail";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ARecurringGiftDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out ARecurringGiftDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND a_batch_number_" +
                    "i = ? AND a_gift_transaction_number_i = ? AND a_detail_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail") 
                            + GenerateWhereClause(ARecurringGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND " +
                        "a_batch_number_i = ? AND a_gift_transaction_number_i = ? AND a_detail_number_i =" +
                        " ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail" + GenerateWhereClause(ARecurringGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND a_batch_number_" +
                    "i = ? AND a_gift_transaction_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaARecurringGift(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGift(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet ADataSet, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + @" FROM PUB_a_recurring_gift_detail, PUB_a_recurring_gift WHERE a_recurring_gift_detail.a_ledger_number_i = a_recurring_gift.a_ledger_number_i AND a_recurring_gift_detail.a_batch_number_i = a_recurring_gift.a_batch_number_i AND a_recurring_gift_detail.a_gift_transaction_number_i = a_recurring_gift.a_gift_transaction_number_i") 
                            + GenerateWhereClauseLong("PUB_a_recurring_gift", ARecurringGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaARecurringGiftTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaARecurringGiftTemplate(out ARecurringGiftDetailTable AData, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaARecurringGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND " +
                        "a_batch_number_i = ? AND a_gift_transaction_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_recurring_gift_detail, PUB_a_recurring_gift WHERE a_recurring_gift_detail.a_ledger_number_i = a_recurring_gift.a_ledger_number_i AND a_recurring_gift_detail.a_batch_number_i = a_recurring_gift.a_batch_number_i AND a_recurring_gift_detail.a_gift_transaction_number_i = a_recurring_gift.a_gift_transaction_number_i" + GenerateWhereClauseLong("PUB_a_recurring_gift", ARecurringGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ARecurringGiftTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND a_motivation_gr" +
                    "oup_code_c = ? AND a_motivation_detail_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetail(FillDataSet, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out ARecurringGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + @" FROM PUB_a_recurring_gift_detail, PUB_a_motivation_detail WHERE a_recurring_gift_detail.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_recurring_gift_detail.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_recurring_gift_detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c") 
                            + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out ARecurringGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out ARecurringGiftDetailTable AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out ARecurringGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out ARecurringGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND " +
                        "a_motivation_group_code_c = ? AND a_motivation_detail_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_recurring_gift_detail, PUB_a_motivation_detail WHERE a_recurring_gift_detail.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_recurring_gift_detail.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_recurring_gift_detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c" + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMotivationDetailTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE p_recipient_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out ARecurringGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientKey(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out ARecurringGiftDetailTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out ARecurringGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail, PUB_p_partner WHERE a_recurring_gift_detail.p_" +
                    "recipient_key_n = p_partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientKeyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE p_recipient_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail, PUB_p_partner WHERE a_recurring" +
                        "_gift_detail.p_recipient_key_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 50);
            ParametersArray[0].Value = ((object)(AMailingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE p_mailing_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out ARecurringGiftDetailTable AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPMailing(FillDataSet, AMailingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out ARecurringGiftDetailTable AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadViaPMailing(out AData, AMailingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out ARecurringGiftDetailTable AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailing(out AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail, PUB_p_mailing WHERE a_recurring_gift_detail.p_" +
                    "mailing_code_c = p_mailing.p_mailing_code_c") 
                            + GenerateWhereClauseLong("PUB_p_mailing", PMailingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out ARecurringGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPMailingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out ARecurringGiftDetailTable AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out ARecurringGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out ARecurringGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 50);
            ParametersArray[0].Value = ((object)(AMailingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE p_mailing_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail, PUB_p_mailing WHERE a_recurring" +
                        "_gift_detail.p_mailing_code_c = p_mailing.p_mailing_code_c" + GenerateWhereClauseLong("PUB_p_mailing", PMailingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PMailingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail WHERE a_recipient_ledger_number_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out ARecurringGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientLedgerNumber(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out ARecurringGiftDetailTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out ARecurringGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_recurring_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_recurring_gift_detail, PUB_p_partner WHERE a_recurring_gift_detail.a_" +
                    "recipient_ledger_number_n = p_partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ARecurringGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ARecurringGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientLedgerNumberTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out ARecurringGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail WHERE a_recipient_ledger_number_" +
                        "n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail, PUB_p_partner WHERE a_recurring" +
                        "_gift_detail.a_recipient_ledger_number_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_recurring_gift_detail WHERE a_ledger_number_i = ? AND a_batch_n" +
                    "umber_i = ? AND a_gift_transaction_number_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_recurring_gift_detail" + GenerateWhereClause(ARecurringGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(ARecurringGiftDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_recurring_gift_detail", ARecurringGiftDetailTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_recurring_gift_detail", ARecurringGiftDetailTable.GetColumnStringList(), ARecurringGiftDetailTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_recurring_gift_detail", ARecurringGiftDetailTable.GetColumnStringList(), ARecurringGiftDetailTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ARecurringGiftDetail", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Information describing groups (batches) of gifts.
    public class AGiftBatchAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AGiftBatch";
        
        /// original table name in database
        public const string DBTABLENAME = "a_gift_batch";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AGiftBatchTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftBatchTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftBatchTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_batch_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftBatchTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch") 
                            + GenerateWhereClause(AGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftBatchTable AData, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftBatchTable AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftBatchTable AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftBatchTable AData, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_batch_num" +
                        "ber_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch" + GenerateWhereClause(AGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AGiftBatchTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AGiftBatchTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AGiftBatchTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch, PUB_a_ledger WHERE a_gift_batch.a_ledger_number_i = a_led" +
                    "ger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AGiftBatchTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AGiftBatchTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch, PUB_a_ledger WHERE a_gift_batch.a_ledger_n" +
                        "umber_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_bank_account_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AGiftBatchTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch, PUB_a_account WHERE a_gift_batch.a_ledger_number_i = a_ac" +
                    "count.a_ledger_number_i AND a_gift_batch.a_bank_account_code_c = a_account.a_acc" +
                    "ount_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AGiftBatchTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AGiftBatchTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_bank_acco" +
                        "unt_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch, PUB_a_account WHERE a_gift_batch.a_ledger_" +
                        "number_i = a_account.a_ledger_number_i AND a_gift_batch.a_bank_account_code_c = " +
                        "a_account.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_bank_cost_centre_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentre(FillDataSet, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftBatchTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch, PUB_a_cost_centre WHERE a_gift_batch.a_ledger_number_i = " +
                    "a_cost_centre.a_ledger_number_i AND a_gift_batch.a_bank_cost_centre_c = a_cost_c" +
                    "entre.a_cost_centre_code_c") 
                            + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentreTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftBatchTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftBatchTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_bank_cost" +
                        "_centre_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch, PUB_a_cost_centre WHERE a_gift_batch.a_led" +
                        "ger_number_i = a_cost_centre.a_ledger_number_i AND a_gift_batch.a_bank_cost_cent" +
                        "re_c = a_cost_centre.a_cost_centre_code_c" + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACostCentreTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch WHERE a_currency_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrency(out AGiftBatchTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out AGiftBatchTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out AGiftBatchTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_batch", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i"}) + " FROM PUB_a_gift_batch, PUB_a_currency WHERE a_gift_batch.a_currency_code_c = a_c" +
                    "urrency.a_currency_code_c") 
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftBatchTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out AGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftBatchTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AGiftBatchTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AGiftBatchTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch, PUB_a_currency WHERE a_gift_batch.a_curren" +
                        "cy_code_c = a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_gift_batch WHERE a_ledger_number_i = ? AND a_batch_number_i = ?" +
                    "", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_gift_batch" + GenerateWhereClause(AGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AGiftBatchTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_gift_batch", AGiftBatchTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_gift_batch", AGiftBatchTable.GetColumnStringList(), AGiftBatchTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_gift_batch", AGiftBatchTable.GetColumnStringList(), AGiftBatchTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AGiftBatch", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Information on the donor's giving. Points to the gift detail records.
    public class AGiftAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AGift";
        
        /// original table name in database
        public const string DBTABLENAME = "a_gift";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AGiftTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift WHERE a_ledger_number_i = ? AND a_batch_number_i = ? AND a_gift_" +
                    "transaction_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift") 
                            + GenerateWhereClause(AGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftTable AData, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftTable AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftTable AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftTable AData, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift WHERE a_ledger_number_i = ? AND a_batch_number_i " +
                        "= ? AND a_gift_transaction_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift" + GenerateWhereClause(AGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift WHERE a_ledger_number_i = ? AND a_batch_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAGiftBatch(FillDataSet, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(out AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatch(out AGiftTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(out AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet ADataSet, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift, PUB_a_gift_batch WHERE a_gift.a_ledger_number_i = a_gift_batch." +
                    "a_ledger_number_i AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i") 
                            + GenerateWhereClauseLong("PUB_a_gift_batch", AGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(out AGiftTable AData, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAGiftBatchTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(out AGiftTable AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(out AGiftTable AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftBatchTemplate(out AGiftTable AData, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift WHERE a_ledger_number_i = ? AND a_batch_number_i " +
                        "= ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_a_gift_batch WHERE a_gift.a_ledger_number_i " +
                        "= a_gift_batch.a_ledger_number_i AND a_gift.a_batch_number_i = a_gift_batch.a_ba" +
                        "tch_number_i" + GenerateWhereClauseLong("PUB_a_gift_batch", AGiftBatchTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AGiftBatchTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet ADataSet, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AMethodOfGivingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift WHERE a_method_of_giving_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out AGiftTable AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfGiving(FillDataSet, AMethodOfGivingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out AGiftTable AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(out AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGiving(out AGiftTable AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(out AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift, PUB_a_method_of_giving WHERE a_gift.a_method_of_giving_code_c =" +
                    " a_method_of_giving.a_method_of_giving_code_c") 
                            + GenerateWhereClauseLong("PUB_a_method_of_giving", AMethodOfGivingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out AGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfGivingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out AGiftTable AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out AGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(out AGiftTable AData, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AMethodOfGivingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift WHERE a_method_of_giving_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_a_method_of_giving WHERE a_gift.a_method_of_" +
                        "giving_code_c = a_method_of_giving.a_method_of_giving_code_c" + GenerateWhereClauseLong("PUB_a_method_of_giving", AMethodOfGivingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMethodOfGivingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift WHERE a_method_of_payment_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out AGiftTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfPayment(FillDataSet, AMethodOfPaymentCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out AGiftTable AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(out AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPayment(out AGiftTable AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(out AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift, PUB_a_method_of_payment WHERE a_gift.a_method_of_payment_code_c" +
                    " = a_method_of_payment.a_method_of_payment_code_c") 
                            + GenerateWhereClauseLong("PUB_a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out AGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMethodOfPaymentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out AGiftTable AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out AGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(out AGiftTable AData, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AMethodOfPaymentCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift WHERE a_method_of_payment_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_a_method_of_payment WHERE a_gift.a_method_of" +
                        "_payment_code_c = a_method_of_payment.a_method_of_payment_code_c" + GenerateWhereClauseLong("PUB_a_method_of_payment", AMethodOfPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMethodOfPaymentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift WHERE p_donor_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out AGiftTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AGiftTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AGiftTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + " FROM PUB_a_gift, PUB_p_partner WHERE a_gift.p_donor_key_n = p_partner.p_partner_" +
                    "key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out AGiftTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AGiftTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AGiftTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AGiftTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift WHERE p_donor_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_p_partner WHERE a_gift.p_donor_key_n = p_par" +
                        "tner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + @" FROM PUB_a_gift, PUB_s_group_gift WHERE PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = ? AND PUB_s_group_gift.s_group_unit_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaSGroup(out AGiftTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSGroup(out AGiftTable AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroup(out AGiftTable AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(out AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i"}) + @" FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n") 
                            + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaSGroupTemplate(out AGiftTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AGiftTable AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AGiftTable AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSGroupTemplate(out AGiftTable AData, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(@"SELECT COUNT(*) FROM PUB_a_gift, PUB_s_group_gift WHERE PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = ? AND PUB_s_group_gift.s_group_unit_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n" + GenerateWhereClauseLong("PUB_s_group", SGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SGroupTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_gift WHERE a_ledger_number_i = ? AND a_batch_number_i = ? AND a" +
                    "_gift_transaction_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_gift" + GenerateWhereClause(AGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AGiftTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_gift", AGiftTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_gift", AGiftTable.GetColumnStringList(), AGiftTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_gift", AGiftTable.GetColumnStringList(), AGiftTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AGift", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    public class AGiftDetailAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AGiftDetail";
        
        /// original table name in database
        public const string DBTABLENAME = "a_gift_detail";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AGiftDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AGiftDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_batch_number_i = ? AND " +
                    "a_gift_transaction_number_i = ? AND a_detail_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail") 
                            + GenerateWhereClause(AGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftDetailTable AData, AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftDetailTable AData, AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftDetailTable AData, AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AGiftDetailTable AData, AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_batch_nu" +
                        "mber_i = ? AND a_gift_transaction_number_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail" + GenerateWhereClause(AGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAGift(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_batch_number_i = ? AND " +
                    "a_gift_transaction_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaAGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGift(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAGift(FillDataSet, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAGift(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaAGift(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGift(out AGiftDetailTable AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGift(out AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet ADataSet, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail, PUB_a_gift WHERE a_gift_detail.a_ledger_number_i = a_gif" +
                    "t.a_ledger_number_i AND a_gift_detail.a_batch_number_i = a_gift.a_batch_number_i" +
                    " AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_numbe" +
                    "r_i") 
                            + GenerateWhereClauseLong("PUB_a_gift", AGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(out AGiftDetailTable AData, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAGiftTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(out AGiftDetailTable AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(out AGiftDetailTable AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAGiftTemplate(out AGiftDetailTable AData, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_batch_nu" +
                        "mber_i = ? AND a_gift_transaction_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAGiftTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_a_gift WHERE a_gift_detail.a_ledger_number_i = a_gift.a_ledger_number_i AND a_gift_detail.a_batch_number_i = a_gift.a_batch_number_i AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i" + GenerateWhereClauseLong("PUB_a_gift", AGiftTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AGiftTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_motivation_group_code_c" +
                    " = ? AND a_motivation_detail_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetail(FillDataSet, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetail(out AGiftDetailTable AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(out AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + @" FROM PUB_a_gift_detail, PUB_a_motivation_detail WHERE a_gift_detail.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_gift_detail.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_gift_detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c") 
                            + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAMotivationDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AGiftDetailTable AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(out AGiftDetailTable AData, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AMotivationGroupCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AMotivationDetailCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_motivati" +
                        "on_group_code_c = ? AND a_motivation_detail_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_a_motivation_detail WHERE a_gift_detail.a_ledger_number_i = a_motivation_detail.a_ledger_number_i AND a_gift_detail.a_motivation_group_code_c = a_motivation_detail.a_motivation_group_code_c AND a_gift_detail.a_motivation_detail_code_c = a_motivation_detail.a_motivation_detail_code_c" + GenerateWhereClauseLong("PUB_a_motivation_detail", AMotivationDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AMotivationDetailTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE p_recipient_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out AGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientKey(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out AGiftDetailTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKey(out AGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail, PUB_p_partner WHERE a_gift_detail.p_recipient_key_n = p_" +
                    "partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientKeyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE p_recipient_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_p_partner WHERE a_gift_detail.p_recip" +
                        "ient_key_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 50);
            ParametersArray[0].Value = ((object)(AMailingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE p_mailing_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out AGiftDetailTable AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPMailing(FillDataSet, AMailingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out AGiftDetailTable AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadViaPMailing(out AData, AMailingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailing(out AGiftDetailTable AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailing(out AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail, PUB_p_mailing WHERE a_gift_detail.p_mailing_code_c = p_m" +
                    "ailing.p_mailing_code_c") 
                            + GenerateWhereClauseLong("PUB_p_mailing", PMailingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out AGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPMailingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out AGiftDetailTable AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out AGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPMailingTemplate(out AGiftDetailTable AData, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 50);
            ParametersArray[0].Value = ((object)(AMailingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE p_mailing_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_p_mailing WHERE a_gift_detail.p_maili" +
                        "ng_code_c = p_mailing.p_mailing_code_c" + GenerateWhereClauseLong("PUB_p_mailing", PMailingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PMailingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE a_recipient_ledger_number_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out AGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientLedgerNumber(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out AGiftDetailTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(out AGiftDetailTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail, PUB_p_partner WHERE a_gift_detail.a_recipient_ledger_num" +
                    "ber_n = p_partner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerRecipientLedgerNumberTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(out AGiftDetailTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE a_recipient_ledger_number_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_p_partner WHERE a_gift_detail.a_recip" +
                        "ient_ledger_number_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_cost_centre_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentre(FillDataSet, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AGiftDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_batch_number_i",
                            "a_gift_transaction_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_gift_detail, PUB_a_cost_centre WHERE a_gift_detail.a_ledger_number_i " +
                    "= a_cost_centre.a_ledger_number_i AND a_gift_detail.a_cost_centre_code_c = a_cos" +
                    "t_centre.a_cost_centre_code_c") 
                            + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AGiftDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AGiftDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentreTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftDetailTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftDetailTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AGiftDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[1].Value = ((object)(ACostCentreCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_cost_cen" +
                        "tre_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail, PUB_a_cost_centre WHERE a_gift_detail.a_l" +
                        "edger_number_i = a_cost_centre.a_ledger_number_i AND a_gift_detail.a_cost_centre" +
                        "_code_c = a_cost_centre.a_cost_centre_code_c" + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACostCentreTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ABatchNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AGiftTransactionNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(ADetailNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_gift_detail WHERE a_ledger_number_i = ? AND a_batch_number_i = " +
                    "? AND a_gift_transaction_number_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_gift_detail" + GenerateWhereClause(AGiftDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AGiftDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_gift_detail", AGiftDetailTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_gift_detail", AGiftDetailTable.GetColumnStringList(), AGiftDetailTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_gift_detail", AGiftDetailTable.GetColumnStringList(), AGiftDetailTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AGiftDetail", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}
