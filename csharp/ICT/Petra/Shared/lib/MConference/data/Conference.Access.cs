/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MConference.Data.Access
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
    using Ict.Petra.Shared.MConference.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MCommon.Data;
    
    
    /// Basic details about a conference
    public class PcConferenceAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConference";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_conference";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference") 
                            + GenerateWhereClause(PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference" + GenerateWhereClause(PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_p_unit WHERE pc_conference.pc_conference_key_n = p_u" +
                    "nit.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_p_unit WHERE pc_conference.pc_confere" +
                        "nce_key_n = p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference WHERE a_currency_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_a_currency WHERE pc_conference.a_currency_code_c = a" +
                    "_currency.a_currency_code_c") 
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_a_currency WHERE pc_conference.a_curr" +
                        "ency_code_c = a_currency.a_currency_code_c" + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPcConferenceOptionType(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_pc_conference_option WHERE PUB_pc_conference_option." +
                    "pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conferenc" +
                    "e_option.pc_option_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionType(FillDataSet, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + @" FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_option WHERE PUB_pc_con" +
                        "ference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND P" +
                        "UB_pc_conference_option.pc_option_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c" + GenerateWhereClauseLong("PUB_pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceOptionTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_pc_attendee WHERE PUB_pc_attendee.pc_conference_key_" +
                    "n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = " +
                    "?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE PUB_pc_attendee.pc_c" +
                    "onference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_pa" +
                    "rtner_key_n = PUB_p_person.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_attendee WHERE PUB_pc_attendee.pc_" +
                        "conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_p" +
                        "artner_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE PUB_p" +
                        "c_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_p" +
                        "c_attendee.p_partner_key_n = PUB_p_person.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPersonTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_pc_conference_venue WHERE PUB_pc_conference_venue.pc" +
                    "_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_" +
                    "venue.p_venue_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenue(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, new string[] {
                            "pc_conference_key_n"}) + " FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHERE PUB_pc_confer" +
                    "ence_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_p" +
                    "c_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenueTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_venue WHERE PUB_pc_conf" +
                        "erence_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB" +
                        "_pc_conference_venue.p_venue_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHER" +
                        "E PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_" +
                        "key_n AND PUB_pc_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PVenueTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_conference WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_conference" + GenerateWhereClause(PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcConferenceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_conference", PcConferenceTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_conference", PcConferenceTable.GetColumnStringList(), PcConferenceTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_conference", PcConferenceTable.GetColumnStringList(), PcConferenceTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcConference", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Cost types to be used for conference (extra) charges
    public class PcCostTypeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcCostType";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_cost_type";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_cost_type_code_c"}) + " FROM PUB_pc_cost_type") 
                            + GenerateOrderByClause(AOrderBy)), PcCostTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcCostTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcCostTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcCostTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcCostTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_cost_type_code_c"}) + " FROM PUB_pc_cost_type WHERE pc_cost_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcCostTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcCostTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ACostTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_cost_type_code_c"}) + " FROM PUB_pc_cost_type") 
                            + GenerateWhereClause(PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcCostTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcCostTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_cost_type", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ACostTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_cost_type WHERE pc_cost_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_cost_type" + GenerateWhereClause(PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(String ACostTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_cost_type WHERE pc_cost_type_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_cost_type" + GenerateWhereClause(PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcCostTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_cost_type", PcCostTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_cost_type", PcCostTypeTable.GetColumnStringList(), PcCostTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_cost_type", PcCostTypeTable.GetColumnStringList(), PcCostTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcCostType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Lists types of options that can be used for a conference
    public class PcConferenceOptionTypeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceOptionType";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_conference_option_type";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option_type") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceOptionTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceOptionTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceOptionTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option_type WHERE pc_option_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option_type") 
                            + GenerateWhereClause(PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option_type", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option_type WHERE pc_option_type_code_c = " +
                        "?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option_type" + GenerateWhereClause(PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference_option_type", AFieldList, new string[] {
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option_type, PUB_pc_conference_option WHERE PUB_pc_confer" +
                    "ence_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type" +
                    "_code_c AND PUB_pc_conference_option.pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_option_type", AFieldList, new string[] {
                            "pc_option_type_code_c"}) + @" FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option_type, PUB_pc_conference_option WHER" +
                        "E PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type" +
                        ".pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_conference_option_type WHERE pc_option_type_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_conference_option_type" + GenerateWhereClause(PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcConferenceOptionTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), PcConferenceOptionTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), PcConferenceOptionTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcConferenceOptionType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Lists options that are set for a conference
    public class PcConferenceOptionAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceOption";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_conference_option";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceOptionTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceOptionTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceOptionTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AOptionTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option WHERE pc_conference_key_n = ? AND pc_option_type_c" +
                    "ode_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option") 
                            + GenerateWhereClause(PcConferenceOptionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AOptionTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option WHERE pc_conference_key_n = ? AND p" +
                        "c_option_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option" + GenerateWhereClause(PcConferenceOptionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_option", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option, PUB_pc_conference WHERE pc_conference_option.pc_c" +
                    "onference_key_n = pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option, PUB_pc_conference WHERE pc_confere" +
                        "nce_option.pc_conference_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option WHERE pc_option_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionType(FillDataSet, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_option", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_type_code_c"}) + " FROM PUB_pc_conference_option, PUB_pc_conference_option_type WHERE pc_conference" +
                    "_option.pc_option_type_code_c = pc_conference_option_type.pc_option_type_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceOptionTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option WHERE pc_option_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option, PUB_pc_conference_option_type WHER" +
                        "E pc_conference_option.pc_option_type_code_c = pc_conference_option_type.pc_opti" +
                        "on_type_code_c" + GenerateWhereClauseLong("PUB_pc_conference_option_type", PcConferenceOptionTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceOptionTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AOptionTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_conference_option WHERE pc_conference_key_n = ? AND pc_option_" +
                    "type_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_conference_option" + GenerateWhereClause(PcConferenceOptionTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcConferenceOptionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_conference_option", PcConferenceOptionTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_conference_option", PcConferenceOptionTable.GetColumnStringList(), PcConferenceOptionTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_conference_option", PcConferenceOptionTable.GetColumnStringList(), PcConferenceOptionTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcConferenceOption", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Lists possible criterias that must be met for discounts to be applied
    public class PcDiscountCriteriaAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcDiscountCriteria";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_discount_criteria";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_discount_criteria_code_c"}) + " FROM PUB_pc_discount_criteria") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountCriteriaTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcDiscountCriteriaTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountCriteriaTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcDiscountCriteriaTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcDiscountCriteriaTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ADiscountCriteriaCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_discount_criteria_code_c"}) + " FROM PUB_pc_discount_criteria WHERE pc_discount_criteria_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountCriteriaTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountCriteriaTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ADiscountCriteriaCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_discount_criteria_code_c"}) + " FROM PUB_pc_discount_criteria") 
                            + GenerateWhereClause(PcDiscountCriteriaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountCriteriaTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountCriteriaTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount_criteria", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ADiscountCriteriaCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount_criteria WHERE pc_discount_criteria_code_c =" +
                        " ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount_criteria" + GenerateWhereClause(PcDiscountCriteriaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ADiscountCriteriaCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_discount_criteria WHERE pc_discount_criteria_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_discount_criteria" + GenerateWhereClause(PcDiscountCriteriaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcDiscountCriteriaTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_discount_criteria", PcDiscountCriteriaTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_discount_criteria", PcDiscountCriteriaTable.GetColumnStringList(), PcDiscountCriteriaTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_discount_criteria", PcDiscountCriteriaTable.GetColumnStringList(), PcDiscountCriteriaTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcDiscountCriteria", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Lists optional discounts for a conference
    public class PcDiscountAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcDiscount";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_discount";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ADiscountCriteriaCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[2].Value = ((object)(ACostTypeCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 6);
            ParametersArray[3].Value = ((object)(AValidity));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[4].Value = ((object)(AUpToAge));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount WHERE pc_conference_key_n = ? AND pc_discount_criteria_code" +
                    "_c = ? AND pc_cost_type_code_c = ? AND pc_validity_c = ? AND pc_up_to_age_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount") 
                            + GenerateWhereClause(PcDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ADiscountCriteriaCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[2].Value = ((object)(ACostTypeCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 6);
            ParametersArray[3].Value = ((object)(AValidity));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[4].Value = ((object)(AUpToAge));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount WHERE pc_conference_key_n = ? AND pc_discoun" +
                        "t_criteria_code_c = ? AND pc_cost_type_code_c = ? AND pc_validity_c = ? AND pc_u" +
                        "p_to_age_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount" + GenerateWhereClause(PcDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_discount", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount, PUB_pc_conference WHERE pc_discount.pc_conference_key_n = " +
                    "pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount, PUB_pc_conference WHERE pc_discount.pc_conf" +
                        "erence_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet ADataSet, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ADiscountCriteriaCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount WHERE pc_discount_criteria_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcDiscountCriteria(FillDataSet, ADiscountCriteriaCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(out AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(out AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet ADataSet, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_discount", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount, PUB_pc_discount_criteria WHERE pc_discount.pc_discount_cri" +
                    "teria_code_c = pc_discount_criteria.pc_discount_criteria_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_discount_criteria", PcDiscountCriteriaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcDiscountCriteriaTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcDiscountCriteria(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ADiscountCriteriaCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount WHERE pc_discount_criteria_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount, PUB_pc_discount_criteria WHERE pc_discount." +
                        "pc_discount_criteria_code_c = pc_discount_criteria.pc_discount_criteria_code_c" + GenerateWhereClauseLong("PUB_pc_discount_criteria", PcDiscountCriteriaTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcDiscountCriteriaTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount WHERE pc_cost_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcCostType(FillDataSet, ACostTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_discount", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_discount_criteria_code_c",
                            "pc_cost_type_code_c",
                            "pc_validity_c",
                            "pc_up_to_age_i"}) + " FROM PUB_pc_discount, PUB_pc_cost_type WHERE pc_discount.pc_cost_type_code_c = p" +
                    "c_cost_type.pc_cost_type_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_cost_type", PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcDiscountTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcDiscountTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcCostTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount WHERE pc_cost_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount, PUB_pc_cost_type WHERE pc_discount.pc_cost_" +
                        "type_code_c = pc_cost_type.pc_cost_type_code_c" + GenerateWhereClauseLong("PUB_pc_cost_type", PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcCostTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ADiscountCriteriaCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[2].Value = ((object)(ACostTypeCode));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 6);
            ParametersArray[3].Value = ((object)(AValidity));
            ParametersArray[4] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[4].Value = ((object)(AUpToAge));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_discount WHERE pc_conference_key_n = ? AND pc_discount_criteri" +
                    "a_code_c = ? AND pc_cost_type_code_c = ? AND pc_validity_c = ? AND pc_up_to_age_" +
                    "i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_discount" + GenerateWhereClause(PcDiscountTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_discount", PcDiscountTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_discount", PcDiscountTable.GetColumnStringList(), PcDiscountTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_discount", PcDiscountTable.GetColumnStringList(), PcDiscountTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcDiscount", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Lists the attendees at a conference
    public class PcAttendeeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcAttendee";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_attendee";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee") 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcAttendeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcAttendeeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcAttendeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee WHERE pc_conference_key_n = ? AND p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee") 
                            + GenerateWhereClause(PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee WHERE pc_conference_key_n = ? AND p_partner_" +
                        "key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee" + GenerateWhereClause(PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_attendee", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee, PUB_pc_conference WHERE pc_attendee.pc_conference_key_n = " +
                    "pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee, PUB_pc_conference WHERE pc_attendee.pc_conf" +
                        "erence_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee WHERE p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_attendee", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee, PUB_p_person WHERE pc_attendee.p_partner_key_n = p_person." +
                    "p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee WHERE p_partner_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee, PUB_p_person WHERE pc_attendee.p_partner_ke" +
                        "y_n = p_person.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPersonTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee WHERE pc_home_office_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_attendee", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n"}) + " FROM PUB_pc_attendee, PUB_p_unit WHERE pc_attendee.pc_home_office_key_n = p_unit" +
                    ".p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcAttendeeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcAttendeeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee WHERE pc_home_office_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee, PUB_p_unit WHERE pc_attendee.pc_home_office" +
                        "_key_n = p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_attendee WHERE pc_conference_key_n = ? AND p_partner_key_n = ?" +
                    "", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_attendee" + GenerateWhereClause(PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcAttendeeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_attendee", PcAttendeeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_attendee", PcAttendeeTable.GetColumnStringList(), PcAttendeeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_attendee", PcAttendeeTable.GetColumnStringList(), PcAttendeeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcAttendee", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Charges for the various xyz_tbd options from a conference (currency held in conference master)
    public class PcConferenceCostAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceCost";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_conference_cost";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_days_i"}) + " FROM PUB_pc_conference_cost") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceCostTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceCostTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceCostTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AOptionDays));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_days_i"}) + " FROM PUB_pc_conference_cost WHERE pc_conference_key_n = ? AND pc_option_days_i =" +
                    " ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionDays, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionDays, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceCostTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AOptionDays, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionDays, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionDays, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_days_i"}) + " FROM PUB_pc_conference_cost") 
                            + GenerateWhereClause(PcConferenceCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceCostTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_cost", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AOptionDays));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_cost WHERE pc_conference_key_n = ? AND pc_" +
                        "option_days_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_cost" + GenerateWhereClause(PcConferenceCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_days_i"}) + " FROM PUB_pc_conference_cost WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_cost", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_option_days_i"}) + " FROM PUB_pc_conference_cost, PUB_pc_conference WHERE pc_conference_cost.pc_confe" +
                    "rence_key_n = pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_cost WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_cost, PUB_pc_conference WHERE pc_conferenc" +
                        "e_cost.pc_conference_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(AOptionDays));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_conference_cost WHERE pc_conference_key_n = ? AND pc_option_da" +
                    "ys_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_conference_cost" + GenerateWhereClause(PcConferenceCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcConferenceCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_conference_cost", PcConferenceCostTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_conference_cost", PcConferenceCostTable.GetColumnStringList(), PcConferenceCostTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_conference_cost", PcConferenceCostTable.GetColumnStringList(), PcConferenceCostTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcConferenceCost", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Contains extra conference costs for individual attendees
    public class PcExtraCostAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcExtraCost";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_extra_cost";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost") 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcExtraCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcExtraCostTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcExtraCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AExtraCostKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost WHERE pc_conference_key_n = ? AND p_partner_key_n = ? AND" +
                    " pc_extra_cost_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AExtraCostKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AExtraCostKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost") 
                            + GenerateWhereClause(PcExtraCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcExtraCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcExtraCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AExtraCostKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost WHERE pc_conference_key_n = ? AND p_partne" +
                        "r_key_n = ? AND pc_extra_cost_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost" + GenerateWhereClause(PcExtraCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost WHERE pc_conference_key_n = ? AND p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendee(FillDataSet, AConferenceKey, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_extra_cost", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost, PUB_pc_attendee WHERE pc_extra_cost.pc_conference_key_n " +
                    "= pc_attendee.pc_conference_key_n AND pc_extra_cost.p_partner_key_n = pc_attende" +
                    "e.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendeeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost WHERE pc_conference_key_n = ? AND p_partne" +
                        "r_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost, PUB_pc_attendee WHERE pc_extra_cost.pc_co" +
                        "nference_key_n = pc_attendee.pc_conference_key_n AND pc_extra_cost.p_partner_key" +
                        "_n = pc_attendee.p_partner_key_n" + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcAttendeeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost WHERE pc_cost_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcCostType(FillDataSet, ACostTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_extra_cost", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost, PUB_pc_cost_type WHERE pc_extra_cost.pc_cost_type_code_c" +
                    " = pc_cost_type.pc_cost_type_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_cost_type", PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcCostTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACostTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost WHERE pc_cost_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost, PUB_pc_cost_type WHERE pc_extra_cost.pc_c" +
                        "ost_type_code_c = pc_cost_type.pc_cost_type_code_c" + GenerateWhereClauseLong("PUB_pc_cost_type", PcCostTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcCostTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost WHERE pc_authorising_field_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnit(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_extra_cost", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_extra_cost_key_i"}) + " FROM PUB_pc_extra_cost, PUB_p_unit WHERE pc_extra_cost.pc_authorising_field_n = " +
                    "p_unit.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcExtraCostTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcExtraCostTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPUnitTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost WHERE pc_authorising_field_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost, PUB_p_unit WHERE pc_extra_cost.pc_authori" +
                        "sing_field_n = p_unit.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_unit", PUnitTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PUnitTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ((object)(AExtraCostKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_extra_cost WHERE pc_conference_key_n = ? AND p_partner_key_n =" +
                    " ? AND pc_extra_cost_key_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_extra_cost" + GenerateWhereClause(PcExtraCostTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcExtraCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_extra_cost", PcExtraCostTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_extra_cost", PcExtraCostTable.GetColumnStringList(), PcExtraCostTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_extra_cost", PcExtraCostTable.GetColumnStringList(), PcExtraCostTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcExtraCost", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Discounts and Supplements for early or late registration
    public class PcEarlyLateAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcEarlyLate";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_early_late";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_applicable_d"}) + " FROM PUB_pc_early_late") 
                            + GenerateOrderByClause(AOrderBy)), PcEarlyLateTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcEarlyLateTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcEarlyLateTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcEarlyLateTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcEarlyLateTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AApplicable));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_applicable_d"}) + " FROM PUB_pc_early_late WHERE pc_conference_key_n = ? AND pc_applicable_d = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcEarlyLateTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AApplicable, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AApplicable, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcEarlyLateTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AApplicable, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AApplicable, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AApplicable, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_applicable_d"}) + " FROM PUB_pc_early_late") 
                            + GenerateWhereClause(PcEarlyLateTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcEarlyLateTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcEarlyLateRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcEarlyLateRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcEarlyLateTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_early_late", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AApplicable));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_early_late WHERE pc_conference_key_n = ? AND pc_appli" +
                        "cable_d = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_early_late" + GenerateWhereClause(PcEarlyLateTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_applicable_d"}) + " FROM PUB_pc_early_late WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcEarlyLateTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcEarlyLateTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_early_late", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_applicable_d"}) + " FROM PUB_pc_early_late, PUB_pc_conference WHERE pc_early_late.pc_conference_key_" +
                    "n = pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcEarlyLateTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcEarlyLateTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_early_late WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_early_late, PUB_pc_conference WHERE pc_early_late.pc_" +
                        "conference_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = ((object)(AApplicable));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_early_late WHERE pc_conference_key_n = ? AND pc_applicable_d =" +
                    " ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_early_late" + GenerateWhereClause(PcEarlyLateTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcEarlyLateTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_early_late", PcEarlyLateTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_early_late", PcEarlyLateTable.GetColumnStringList(), PcEarlyLateTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_early_late", PcEarlyLateTable.GetColumnStringList(), PcEarlyLateTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcEarlyLate", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Contains information about which groups individual attendees are assigned to
    public class PcGroupAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcGroup";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_group";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_group_type_c",
                            "pc_group_name_c"}) + " FROM PUB_pc_group") 
                            + GenerateOrderByClause(AOrderBy)), PcGroupTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcGroupTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[2].Value = ((object)(AGroupType));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[3].Value = ((object)(AGroupName));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_group_type_c",
                            "pc_group_name_c"}) + " FROM PUB_pc_group WHERE pc_conference_key_n = ? AND p_partner_key_n = ? AND pc_g" +
                    "roup_type_c = ? AND pc_group_name_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcGroupTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_group_type_c",
                            "pc_group_name_c"}) + " FROM PUB_pc_group") 
                            + GenerateWhereClause(PcGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcGroupTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_group", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[2].Value = ((object)(AGroupType));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[3].Value = ((object)(AGroupName));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_group WHERE pc_conference_key_n = ? AND p_partner_key" +
                        "_n = ? AND pc_group_type_c = ? AND pc_group_name_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_group" + GenerateWhereClause(PcGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_group_type_c",
                            "pc_group_name_c"}) + " FROM PUB_pc_group WHERE pc_conference_key_n = ? AND p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcGroupTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendee(FillDataSet, AConferenceKey, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_group", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_partner_key_n",
                            "pc_group_type_c",
                            "pc_group_name_c"}) + " FROM PUB_pc_group, PUB_pc_attendee WHERE pc_group.pc_conference_key_n = pc_atten" +
                    "dee.pc_conference_key_n AND pc_group.p_partner_key_n = pc_attendee.p_partner_key" +
                    "_n") 
                            + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcGroupTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcGroupTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendeeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_group WHERE pc_conference_key_n = ? AND p_partner_key" +
                        "_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_group, PUB_pc_attendee WHERE pc_group.pc_conference_k" +
                        "ey_n = pc_attendee.pc_conference_key_n AND pc_group.p_partner_key_n = pc_attende" +
                        "e.p_partner_key_n" + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcAttendeeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(APartnerKey));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[2].Value = ((object)(AGroupType));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 80);
            ParametersArray[3].Value = ((object)(AGroupName));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_group WHERE pc_conference_key_n = ? AND p_partner_key_n = ? AN" +
                    "D pc_group_type_c = ? AND pc_group_name_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_group" + GenerateWhereClause(PcGroupTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcGroupTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_group", PcGroupTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_group", PcGroupTable.GetColumnStringList(), PcGroupTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_group", PcGroupTable.GetColumnStringList(), PcGroupTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcGroup", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Xyz_tbd travel supplements (by xyz_tbd ID)
    public class PcSupplementAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcSupplement";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_supplement";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_xyz_tbd_type_c"}) + " FROM PUB_pc_supplement") 
                            + GenerateOrderByClause(AOrderBy)), PcSupplementTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcSupplementTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcSupplementTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcSupplementTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcSupplementTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 12);
            ParametersArray[1].Value = ((object)(AXyzTbdType));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_xyz_tbd_type_c"}) + " FROM PUB_pc_supplement WHERE pc_conference_key_n = ? AND pc_xyz_tbd_type_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcSupplementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AXyzTbdType, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AXyzTbdType, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcSupplementTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AXyzTbdType, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AXyzTbdType, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AXyzTbdType, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_xyz_tbd_type_c"}) + " FROM PUB_pc_supplement") 
                            + GenerateWhereClause(PcSupplementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcSupplementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcSupplementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcSupplementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcSupplementTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_supplement", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 12);
            ParametersArray[1].Value = ((object)(AXyzTbdType));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_supplement WHERE pc_conference_key_n = ? AND pc_xyz_t" +
                        "bd_type_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_supplement" + GenerateWhereClause(PcSupplementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_xyz_tbd_type_c"}) + " FROM PUB_pc_supplement WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcSupplementTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcSupplementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_supplement", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "pc_xyz_tbd_type_c"}) + " FROM PUB_pc_supplement, PUB_pc_conference WHERE pc_supplement.pc_conference_key_" +
                    "n = pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcSupplementTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcSupplementTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_supplement WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_supplement, PUB_pc_conference WHERE pc_supplement.pc_" +
                        "conference_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 12);
            ParametersArray[1].Value = ((object)(AXyzTbdType));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_supplement WHERE pc_conference_key_n = ? AND pc_xyz_tbd_type_c" +
                    " = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_supplement" + GenerateWhereClause(PcSupplementTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcSupplementTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_supplement", PcSupplementTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_supplement", PcSupplementTable.GetColumnStringList(), PcSupplementTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_supplement", PcSupplementTable.GetColumnStringList(), PcSupplementTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcSupplement", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Links venues to conferences
    public class PcConferenceVenueAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceVenue";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_conference_venue";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceVenueTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceVenueTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcConferenceVenueTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AVenueKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue WHERE pc_conference_key_n = ? AND p_venue_key_n = ?" +
                    "") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AVenueKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AVenueKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AConferenceKey, AVenueKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AVenueKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AVenueKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue") 
                            + GenerateWhereClause(PcConferenceVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_venue", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AVenueKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_venue WHERE pc_conference_key_n = ? AND p_" +
                        "venue_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_venue" + GenerateWhereClause(PcConferenceVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue WHERE pc_conference_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_venue", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue, PUB_pc_conference WHERE pc_conference_venue.pc_con" +
                    "ference_key_n = pc_conference.pc_conference_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_venue WHERE pc_conference_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_venue, PUB_pc_conference WHERE pc_conferen" +
                        "ce_venue.pc_conference_key_n = pc_conference.pc_conference_key_n" + GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcConferenceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue WHERE p_venue_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenue(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_venue", AFieldList, new string[] {
                            "pc_conference_key_n",
                            "p_venue_key_n"}) + " FROM PUB_pc_conference_venue, PUB_p_venue WHERE pc_conference_venue.p_venue_key_" +
                    "n = p_venue.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcConferenceVenueTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceVenueTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenueTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_venue WHERE p_venue_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_venue, PUB_p_venue WHERE pc_conference_ven" +
                        "ue.p_venue_key_n = p_venue.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PVenueTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AVenueKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_conference_venue WHERE pc_conference_key_n = ? AND p_venue_key" +
                    "_n = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_conference_venue" + GenerateWhereClause(PcConferenceVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcConferenceVenueTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_conference_venue", PcConferenceVenueTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_conference_venue", PcConferenceVenueTable.GetColumnStringList(), PcConferenceVenueTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_conference_venue", PcConferenceVenueTable.GetColumnStringList(), PcConferenceVenueTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcConferenceVenue", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}
