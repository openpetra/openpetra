/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MFinance.AP.Data.Access
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
    using Ict.Petra.Shared.MFinance.AP.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MFinance.Account.Data;
    using Ict.Petra.Shared.MSysMan.Data;
    
    
    /// This table defines the concept of a supplier in the AP system and is the centre of the AP system.
    public class AApSupplierAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApSupplier";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_supplier";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier") 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApSupplierTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApSupplierTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApSupplierTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier WHERE p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier") 
                            + GenerateWhereClause(AApSupplierTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_supplier", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_supplier WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_supplier" + GenerateWhereClause(AApSupplierTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier WHERE p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_supplier", AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier, PUB_p_partner WHERE a_ap_supplier.p_partner_key_n = p_pa" +
                    "rtner.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_supplier WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_supplier, PUB_p_partner WHERE a_ap_supplier.p_partn" +
                        "er_key_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier WHERE a_currency_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_supplier", AFieldList, new string[] {
                            "p_partner_key_n"}) + " FROM PUB_a_ap_supplier, PUB_a_currency WHERE a_ap_supplier.a_currency_code_c = a" +
                    "_currency.a_currency_code_c") 
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApSupplierTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApSupplierTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_supplier WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_supplier, PUB_a_currency WHERE a_ap_supplier.a_curr" +
                        "ency_code_c = a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_supplier WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_supplier" + GenerateWhereClause(AApSupplierTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApSupplierTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_supplier", AApSupplierTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_supplier", AApSupplierTable.GetColumnStringList(), AApSupplierTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_supplier", AApSupplierTable.GetColumnStringList(), AApSupplierTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApSupplier", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This is either an invoice or a credit note in the Accounts Payable system.
    public class AApDocumentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocument";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_document";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApDocumentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_ap_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document") 
                            + GenerateWhereClause(AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_ap_numbe" +
                        "r_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document" + GenerateWhereClause(AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document, PUB_a_ledger WHERE a_ap_document.a_ledger_number_i = a_l" +
                    "edger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document, PUB_a_ledger WHERE a_ap_document.a_ledger" +
                        "_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document WHERE p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApSupplier(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet ADataSet, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document, PUB_a_ap_supplier WHERE a_ap_document.p_partner_key_n = " +
                    "a_ap_supplier.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_a_ap_supplier", AApSupplierTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApSupplierTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApSupplier(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApSupplierTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document, PUB_a_ap_supplier WHERE a_ap_document.p_p" +
                        "artner_key_n = a_ap_supplier.p_partner_key_n" + GenerateWhereClauseLong("PUB_a_ap_supplier", AApSupplierTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApSupplierTable.GetPrimKeyColumnOrdList())));
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
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_ap_account_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i"}) + " FROM PUB_a_ap_document, PUB_a_account WHERE a_ap_document.a_ledger_number_i = a_" +
                    "account.a_ledger_number_i AND a_ap_document.a_ap_account_c = a_account.a_account" +
                    "_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_ap_accou" +
                        "nt_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document, PUB_a_account WHERE a_ap_document.a_ledge" +
                        "r_number_i = a_account.a_ledger_number_i AND a_ap_document.a_ap_account_c = a_ac" +
                        "count.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_ap_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_document" + GenerateWhereClause(AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApDocumentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_document", AApDocumentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_document", AApDocumentTable.GetColumnStringList(), AApDocumentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_document", AApDocumentTable.GetColumnStringList(), AApDocumentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApDocument", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This table receives a new entry when a credit note is applied to an invoice. Since the invoices and credit notes share the same table, we need a way to link the two, and this is the role of this table.
    public class ACrdtNoteInvoiceLinkAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "ACrdtNoteInvoiceLink";
        
        /// original table name in database
        public const string DBTABLENAME = "a_crdt_note_invoice_link";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link") 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ACreditNoteNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AInvoiceNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND a_credit_note_" +
                    "number_i = ? AND a_invoice_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link") 
                            + GenerateWhereClause(ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACrdtNoteInvoiceLinkRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ACreditNoteNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AInvoiceNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND" +
                        " a_credit_note_number_i = ? AND a_invoice_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link" + GenerateWhereClause(ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND a_credit_note_" +
                    "number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentCreditNoteNumber(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_crdt_note_invoice_link", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link, PUB_a_ap_document WHERE a_crdt_note_invoice_l" +
                    "ink.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_crdt_note_invoice_" +
                    "link.a_credit_note_number_i = a_ap_document.a_ap_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentCreditNoteNumberTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocumentCreditNoteNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND" +
                        " a_credit_note_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link, PUB_a_ap_document WHERE a_crdt" +
                        "_note_invoice_link.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_crd" +
                        "t_note_invoice_link.a_credit_note_number_i = a_ap_document.a_ap_number_i" + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND a_invoice_numb" +
                    "er_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentInvoiceNumber(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_crdt_note_invoice_link", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_credit_note_number_i",
                            "a_invoice_number_i"}) + " FROM PUB_a_crdt_note_invoice_link, PUB_a_ap_document WHERE a_crdt_note_invoice_l" +
                    "ink.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_crdt_note_invoice_" +
                    "link.a_invoice_number_i = a_ap_document.a_ap_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), ACrdtNoteInvoiceLinkTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACrdtNoteInvoiceLinkTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentInvoiceNumberTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocumentInvoiceNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND" +
                        " a_invoice_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link, PUB_a_ap_document WHERE a_crdt" +
                        "_note_invoice_link.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_crd" +
                        "t_note_invoice_link.a_invoice_number_i = a_ap_document.a_ap_number_i" + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ACreditNoteNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AInvoiceNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_crdt_note_invoice_link WHERE a_ledger_number_i = ? AND a_credit" +
                    "_note_number_i = ? AND a_invoice_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_crdt_note_invoice_link" + GenerateWhereClause(ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(ACrdtNoteInvoiceLinkTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_crdt_note_invoice_link", ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_crdt_note_invoice_link", ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ACrdtNoteInvoiceLinkTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_crdt_note_invoice_link", ACrdtNoteInvoiceLinkTable.GetColumnStringList(), ACrdtNoteInvoiceLinkTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ACrdtNoteInvoiceLink", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// An invoice or credit note consists out of several items, or details. This table contains all these details.
    public class AApDocumentDetailAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocumentDetail";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_document_detail";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApDocumentDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_ap_number_i = ? " +
                    "AND a_detail_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail") 
                            + GenerateWhereClause(AApDocumentDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_a" +
                        "p_number_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail" + GenerateWhereClause(AApDocumentDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail, PUB_a_ledger WHERE a_ap_document_detail.a_ledger_" +
                    "number_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail, PUB_a_ledger WHERE a_ap_document_d" +
                        "etail.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_ap_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocument(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail, PUB_a_ap_document WHERE a_ap_document_detail.a_le" +
                    "dger_number_i = a_ap_document.a_ledger_number_i AND a_ap_document_detail.a_ap_nu" +
                    "mber_i = a_ap_document.a_ap_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_a" +
                        "p_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail, PUB_a_ap_document WHERE a_ap_docum" +
                        "ent_detail.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_ap_document" +
                        "_detail.a_ap_number_i = a_ap_document.a_ap_number_i" + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentTable.GetPrimKeyColumnOrdList())));
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
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_cost_centre_code" +
                    "_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentre(FillDataSet, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail, PUB_a_cost_centre WHERE a_ap_document_detail.a_le" +
                    "dger_number_i = a_cost_centre.a_ledger_number_i AND a_ap_document_detail.a_cost_" +
                    "centre_code_c = a_cost_centre.a_cost_centre_code_c") 
                            + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACostCentreTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_c" +
                        "ost_centre_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail, PUB_a_cost_centre WHERE a_ap_docum" +
                        "ent_detail.a_ledger_number_i = a_cost_centre.a_ledger_number_i AND a_ap_document" +
                        "_detail.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c" + GenerateWhereClauseLong("PUB_a_cost_centre", ACostCentreTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACostCentreTable.GetPrimKeyColumnOrdList())));
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
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_account_code_c =" +
                    " ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_detail", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i"}) + " FROM PUB_a_ap_document_detail, PUB_a_account WHERE a_ap_document_detail.a_ledger" +
                    "_number_i = a_account.a_ledger_number_i AND a_ap_document_detail.a_account_code_" +
                    "c = a_account.a_account_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentDetailTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentDetailTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_a" +
                        "ccount_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail, PUB_a_account WHERE a_ap_document_" +
                        "detail.a_ledger_number_i = a_account.a_ledger_number_i AND a_ap_document_detail." +
                        "a_account_code_c = a_account.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_document_detail WHERE a_ledger_number_i = ? AND a_ap_number_" +
                    "i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_document_detail" + GenerateWhereClause(AApDocumentDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApDocumentDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_document_detail", AApDocumentDetailTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_document_detail", AApDocumentDetailTable.GetColumnStringList(), AApDocumentDetailTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_document_detail", AApDocumentDetailTable.GetColumnStringList(), AApDocumentDetailTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApDocumentDetail", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Records all payments that have been made against an accounts payable detail.
    public class AApPaymentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApPayment";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_payment";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment") 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment WHERE a_ledger_number_i = ? AND a_payment_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment") 
                            + GenerateWhereClause(AApPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment WHERE a_ledger_number_i = ? AND a_payment_n" +
                        "umber_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment" + GenerateWhereClause(AApPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment, PUB_a_ledger WHERE a_ap_payment.a_ledger_number_i = a_led" +
                    "ger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment, PUB_a_ledger WHERE a_ap_payment.a_ledger_n" +
                        "umber_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AUserId));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment WHERE s_user_id_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSUser(FillDataSet, AUserId, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment, PUB_s_user WHERE a_ap_payment.s_user_id_c = s_user.s_user" +
                    "_id_c") 
                            + GenerateWhereClauseLong("PUB_s_user", SUserTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSUserTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AUserId));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment WHERE s_user_id_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment, PUB_s_user WHERE a_ap_payment.s_user_id_c " +
                        "= s_user.s_user_id_c" + GenerateWhereClauseLong("PUB_s_user", SUserTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SUserTable.GetPrimKeyColumnOrdList())));
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
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment WHERE a_ledger_number_i = ? AND a_bank_account_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_payment, PUB_a_account WHERE a_ap_payment.a_ledger_number_i = a_ac" +
                    "count.a_ledger_number_i AND a_ap_payment.a_bank_account_c = a_account.a_account_" +
                    "code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment WHERE a_ledger_number_i = ? AND a_bank_acco" +
                        "unt_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment, PUB_a_account WHERE a_ap_payment.a_ledger_" +
                        "number_i = a_account.a_ledger_number_i AND a_ap_payment.a_bank_account_c = a_acc" +
                        "ount.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_payment WHERE a_ledger_number_i = ? AND a_payment_number_i =" +
                    " ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_payment" + GenerateWhereClause(AApPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_payment", AApPaymentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_payment", AApPaymentTable.GetColumnStringList(), AApPaymentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_payment", AApPaymentTable.GetColumnStringList(), AApPaymentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApPayment", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This table links the different payments to actual invoices and credit notes.
    public class AApDocumentPaymentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocumentPayment";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_document_payment";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_ap_number_i = ?" +
                    " AND a_payment_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment") 
                            + GenerateWhereClause(AApDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "ap_number_i = ? AND a_payment_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment" + GenerateWhereClause(AApDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment, PUB_a_ledger WHERE a_ap_document_payment.a_ledge" +
                    "r_number_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment, PUB_a_ledger WHERE a_ap_document_" +
                        "payment.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_ap_number_i = ?" +
                    "") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocument(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment, PUB_a_ap_document WHERE a_ap_document_payment.a_" +
                    "ledger_number_i = a_ap_document.a_ledger_number_i AND a_ap_document_payment.a_ap" +
                    "_number_i = a_ap_document.a_ap_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "ap_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment, PUB_a_ap_document WHERE a_ap_docu" +
                        "ment_payment.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_ap_docume" +
                        "nt_payment.a_ap_number_i = a_ap_document.a_ap_number_i" + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApPayment(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_payment_number_" +
                    "i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApPayment(FillDataSet, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet ADataSet, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ap_document_payment, PUB_a_ap_payment WHERE a_ap_document_payment.a_l" +
                    "edger_number_i = a_ap_payment.a_ledger_number_i AND a_ap_document_payment.a_paym" +
                    "ent_number_i = a_ap_payment.a_payment_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_payment", AApPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApPaymentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "payment_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApPaymentTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment, PUB_a_ap_payment WHERE a_ap_docum" +
                        "ent_payment.a_ledger_number_i = a_ap_payment.a_ledger_number_i AND a_ap_document" +
                        "_payment.a_payment_number_i = a_ap_payment.a_payment_number_i" + GenerateWhereClauseLong("PUB_a_ap_payment", AApPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApPaymentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_document_payment WHERE a_ledger_number_i = ? AND a_ap_number" +
                    "_i = ? AND a_payment_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_document_payment" + GenerateWhereClause(AApDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApDocumentPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_document_payment", AApDocumentPaymentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_document_payment", AApDocumentPaymentTable.GetColumnStringList(), AApDocumentPaymentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_document_payment", AApDocumentPaymentTable.GetColumnStringList(), AApDocumentPaymentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApDocumentPayment", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This table acts as a queue for electronic payments. If an invoice is paid electronically, the payment is added to this table. A EP program will go through this table paying all entries to GL and moving them to the a_ap_payment table.
    public class AEpPaymentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AEpPayment";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ep_payment";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment") 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AEpPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AEpPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AEpPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment WHERE a_ledger_number_i = ? AND a_payment_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment") 
                            + GenerateWhereClause(AEpPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment WHERE a_ledger_number_i = ? AND a_payment_n" +
                        "umber_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment" + GenerateWhereClause(AEpPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment, PUB_a_ledger WHERE a_ep_payment.a_ledger_number_i = a_led" +
                    "ger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment, PUB_a_ledger WHERE a_ep_payment.a_ledger_n" +
                        "umber_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AUserId));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment WHERE s_user_id_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSUser(FillDataSet, AUserId, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment, PUB_s_user WHERE a_ep_payment.s_user_id_c = s_user.s_user" +
                    "_id_c") 
                            + GenerateWhereClauseLong("PUB_s_user", SUserTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaSUserTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AUserId));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment WHERE s_user_id_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment, PUB_s_user WHERE a_ep_payment.s_user_id_c " +
                        "= s_user.s_user_id_c" + GenerateWhereClauseLong("PUB_s_user", SUserTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, SUserTable.GetPrimKeyColumnOrdList())));
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
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment WHERE a_ledger_number_i = ? AND a_bank_account_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_payment, PUB_a_account WHERE a_ep_payment.a_ledger_number_i = a_ac" +
                    "count.a_ledger_number_i AND a_ep_payment.a_bank_account_c = a_account.a_account_" +
                    "code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment WHERE a_ledger_number_i = ? AND a_bank_acco" +
                        "unt_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment, PUB_a_account WHERE a_ep_payment.a_ledger_" +
                        "number_i = a_account.a_ledger_number_i AND a_ep_payment.a_bank_account_c = a_acc" +
                        "ount.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ep_payment WHERE a_ledger_number_i = ? AND a_payment_number_i =" +
                    " ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ep_payment" + GenerateWhereClause(AEpPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AEpPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ep_payment", AEpPaymentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ep_payment", AEpPaymentTable.GetColumnStringList(), AEpPaymentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ep_payment", AEpPaymentTable.GetColumnStringList(), AEpPaymentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AEpPayment", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// This table links the different EP payments to actual invoices and credit notes.
    public class AEpDocumentPaymentAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AEpDocumentPayment";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ep_document_payment";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment") 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AEpDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AEpDocumentPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AEpDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_ap_number_i = ?" +
                    " AND a_payment_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment") 
                            + GenerateWhereClause(AEpDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "ap_number_i = ? AND a_payment_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment" + GenerateWhereClause(AEpDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedger(FillDataSet, ALedgerNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment, PUB_a_ledger WHERE a_ep_document_payment.a_ledge" +
                    "r_number_i = a_ledger.a_ledger_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaALedgerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment, PUB_a_ledger WHERE a_ep_document_" +
                        "payment.a_ledger_number_i = a_ledger.a_ledger_number_i" + GenerateWhereClauseLong("PUB_a_ledger", ALedgerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ALedgerTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_ap_number_i = ?" +
                    "") 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocument(FillDataSet, ALedgerNumber, AApNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment, PUB_a_ap_document WHERE a_ep_document_payment.a_" +
                    "ledger_number_i = a_ap_document.a_ledger_number_i AND a_ep_document_payment.a_ap" +
                    "_number_i = a_ap_document.a_ap_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "ap_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment, PUB_a_ap_document WHERE a_ep_docu" +
                        "ment_payment.a_ledger_number_i = a_ap_document.a_ledger_number_i AND a_ep_docume" +
                        "nt_payment.a_ap_number_i = a_ap_document.a_ap_number_i" + GenerateWhereClauseLong("PUB_a_ap_document", AApDocumentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_payment_number_" +
                    "i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAEpPayment(FillDataSet, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet ADataSet, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ep_document_payment", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_payment_number_i"}) + " FROM PUB_a_ep_document_payment, PUB_a_ep_payment WHERE a_ep_document_payment.a_l" +
                    "edger_number_i = a_ep_payment.a_ledger_number_i AND a_ep_document_payment.a_paym" +
                    "ent_number_i = a_ep_payment.a_payment_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ep_payment", AEpPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AEpDocumentPaymentTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AEpDocumentPaymentTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAEpPaymentTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAEpPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(APaymentNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_" +
                        "payment_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment, PUB_a_ep_payment WHERE a_ep_docum" +
                        "ent_payment.a_ledger_number_i = a_ep_payment.a_ledger_number_i AND a_ep_document" +
                        "_payment.a_payment_number_i = a_ep_payment.a_payment_number_i" + GenerateWhereClauseLong("PUB_a_ep_payment", AEpPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AEpPaymentTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(APaymentNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ep_document_payment WHERE a_ledger_number_i = ? AND a_ap_number" +
                    "_i = ? AND a_payment_number_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ep_document_payment" + GenerateWhereClause(AEpDocumentPaymentTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AEpDocumentPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ep_document_payment", AEpDocumentPaymentTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ep_document_payment", AEpDocumentPaymentTable.GetColumnStringList(), AEpDocumentPaymentTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ep_document_payment", AEpDocumentPaymentTable.GetColumnStringList(), AEpDocumentPaymentTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AEpDocumentPayment", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Analysis Attributes applied to an AP for posting to the GL.
    public class AApAnalAttribAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "AApAnalAttrib";
        
        /// original table name in database
        public const string DBTABLENAME = "a_ap_anal_attrib";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out AApAnalAttribTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out AApAnalAttribTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out AApAnalAttribTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AAnalysisTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_ap_number_i = ? AND " +
                    "a_detail_number_i = ? AND a_analysis_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib") 
                            + GenerateWhereClause(AApAnalAttribTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApAnalAttribRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApAnalAttribRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AAnalysisTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_ap_nu" +
                        "mber_i = ? AND a_detail_number_i = ? AND a_analysis_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib" + GenerateWhereClause(AApAnalAttribTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_ap_number_i = ? AND " +
                    "a_detail_number_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentDetail(FillDataSet, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(out AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(out AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet ADataSet, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_anal_attrib", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + @" FROM PUB_a_ap_anal_attrib, PUB_a_ap_document_detail WHERE a_ap_anal_attrib.a_ledger_number_i = a_ap_document_detail.a_ledger_number_i AND a_ap_anal_attrib.a_ap_number_i = a_ap_document_detail.a_ap_number_i AND a_ap_anal_attrib.a_detail_number_i = a_ap_document_detail.a_detail_number_i") 
                            + GenerateWhereClauseLong("PUB_a_ap_document_detail", AApDocumentDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAApDocumentDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_ap_nu" +
                        "mber_i = ? AND a_detail_number_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_ap_anal_attrib, PUB_a_ap_document_detail WHERE a_ap_anal_attrib.a_ledger_number_i = a_ap_document_detail.a_ledger_number_i AND a_ap_anal_attrib.a_ap_number_i = a_ap_document_detail.a_ap_number_i AND a_ap_anal_attrib.a_detail_number_i = a_ap_document_detail.a_detail_number_i" + GenerateWhereClauseLong("PUB_a_ap_document_detail", AApDocumentDetailTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AApDocumentDetailTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AAnalysisTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_account_code_c = ? A" +
                    "ND a_analysis_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAnalysisAttribute(FillDataSet, ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(out AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(out AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet ADataSet, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_anal_attrib", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + @" FROM PUB_a_ap_anal_attrib, PUB_a_analysis_attribute WHERE a_ap_anal_attrib.a_ledger_number_i = a_analysis_attribute.a_ledger_number_i AND a_ap_anal_attrib.a_account_code_c = a_analysis_attribute.a_account_code_c AND a_ap_anal_attrib.a_analysis_type_code_c = a_analysis_attribute.a_analysis_type_code_c") 
                            + GenerateWhereClauseLong("PUB_a_analysis_attribute", AAnalysisAttributeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, AAnalysisAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, AAnalysisAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAnalysisAttributeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAAnalysisAttribute(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAccountCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(AAnalysisTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_accou" +
                        "nt_code_c = ? AND a_analysis_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_ap_anal_attrib, PUB_a_analysis_attribute WHERE a_ap_anal_attrib.a_ledger_number_i = a_analysis_attribute.a_ledger_number_i AND a_ap_anal_attrib.a_account_code_c = a_analysis_attribute.a_account_code_c AND a_ap_anal_attrib.a_analysis_type_code_c = a_analysis_attribute.a_analysis_type_code_c" + GenerateWhereClauseLong("PUB_a_analysis_attribute", AAnalysisAttributeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAnalysisAttributeTable.GetPrimKeyColumnOrdList())));
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
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_account_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccount(FillDataSet, ALedgerNumber, AAccountCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_anal_attrib", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib, PUB_a_account WHERE a_ap_anal_attrib.a_ledger_number_" +
                    "i = a_account.a_ledger_number_i AND a_ap_anal_attrib.a_account_code_c = a_accoun" +
                    "t.a_account_code_c") 
                            + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAAccountTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_accou" +
                        "nt_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib, PUB_a_account WHERE a_ap_anal_attrib.a" +
                        "_ledger_number_i = a_account.a_ledger_number_i AND a_ap_anal_attrib.a_account_co" +
                        "de_c = a_account.a_account_code_c" + GenerateWhereClauseLong("PUB_a_account", AAccountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AAccountTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet ADataSet, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAnalysisTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[2].Value = ((object)(AAnalysisValue));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + " FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_analysis_type_code_c" +
                    " = ? AND a_analysis_attribute_value_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAFreeformAnalysis(FillDataSet, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(out AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(out AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet ADataSet, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_ap_anal_attrib", AFieldList, new string[] {
                            "a_ledger_number_i",
                            "a_ap_number_i",
                            "a_detail_number_i",
                            "a_analysis_type_code_c"}) + @" FROM PUB_a_ap_anal_attrib, PUB_a_freeform_analysis WHERE a_ap_anal_attrib.a_ledger_number_i = a_freeform_analysis.a_ledger_number_i AND a_ap_anal_attrib.a_analysis_type_code_c = a_freeform_analysis.a_analysis_type_code_c AND a_ap_anal_attrib.a_analysis_attribute_value_c = a_freeform_analysis.a_analysis_value_c") 
                            + GenerateWhereClauseLong("PUB_a_freeform_analysis", AFreeformAnalysisTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), AApAnalAttribTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, AFreeformAnalysisRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, AFreeformAnalysisRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AApAnalAttribTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAFreeformAnalysisTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaAFreeformAnalysis(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(AAnalysisTypeCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[2].Value = ((object)(AAnalysisValue));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_analy" +
                        "sis_type_code_c = ? AND a_analysis_attribute_value_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_a_ap_anal_attrib, PUB_a_freeform_analysis WHERE a_ap_anal_attrib.a_ledger_number_i = a_freeform_analysis.a_ledger_number_i AND a_ap_anal_attrib.a_analysis_type_code_c = a_freeform_analysis.a_analysis_type_code_c AND a_ap_anal_attrib.a_analysis_attribute_value_c = a_freeform_analysis.a_analysis_value_c" + GenerateWhereClauseLong("PUB_a_freeform_analysis", AFreeformAnalysisTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AFreeformAnalysisTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ALedgerNumber));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AApNumber));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(ADetailNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[3].Value = ((object)(AAnalysisTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_ap_anal_attrib WHERE a_ledger_number_i = ? AND a_ap_number_i = " +
                    "? AND a_detail_number_i = ? AND a_analysis_type_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_ap_anal_attrib" + GenerateWhereClause(AApAnalAttribTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(AApAnalAttribTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("a_ap_anal_attrib", AApAnalAttribTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_ap_anal_attrib", AApAnalAttribTable.GetColumnStringList(), AApAnalAttribTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_ap_anal_attrib", AApAnalAttribTable.GetColumnStringList(), AApAnalAttribTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AApAnalAttrib", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}
