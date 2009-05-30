/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MHospitality.Data.Access
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
    using Ict.Petra.Shared.MHospitality.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MConference.Data;
    using Ict.Petra.Shared.MFinance.AR.Data;
    
    
    /// Details of building used for accomodation at a conference
    public class PcBuildingAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcBuilding";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_building";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c"}) + " FROM PUB_pc_building") 
                            + GenerateOrderByClause(AOrderBy)), PcBuildingTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcBuildingTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcBuildingTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcBuildingTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcBuildingTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c"}) + " FROM PUB_pc_building WHERE p_venue_key_n = ? AND pc_building_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcBuildingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcBuildingTable AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcBuildingTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AVenueKey, ABuildingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcBuildingTable AData, Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcBuildingTable AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c"}) + " FROM PUB_pc_building") 
                            + GenerateWhereClause(PcBuildingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcBuildingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcBuildingTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_building", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_building WHERE p_venue_key_n = ? AND pc_building_code" +
                        "_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_building" + GenerateWhereClause(PcBuildingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c"}) + " FROM PUB_pc_building WHERE p_venue_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcBuildingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPVenue(out PcBuildingTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcBuildingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenue(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcBuildingTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenue(out PcBuildingTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_building", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c"}) + " FROM PUB_pc_building, PUB_p_venue WHERE pc_building.p_venue_key_n = p_venue.p_pa" +
                    "rtner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcBuildingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcBuildingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenueTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_building WHERE p_venue_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_building, PUB_p_venue WHERE pc_building.p_venue_key_n" +
                        " = p_venue.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_venue", PVenueTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PVenueTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_building WHERE p_venue_key_n = ? AND pc_building_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_building" + GenerateWhereClause(PcBuildingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcBuildingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_building", PcBuildingTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_building", PcBuildingTable.GetColumnStringList(), PcBuildingTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_building", PcBuildingTable.GetColumnStringList(), PcBuildingTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcBuilding", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Details of rooms used for accommodation at a conference
    public class PcRoomAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcRoom";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_room";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + " FROM PUB_pc_room") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcRoomTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + " FROM PUB_pc_room WHERE p_venue_key_n = ? AND pc_building_code_c = ? AND pc_room_" +
                    "number_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + " FROM PUB_pc_room") 
                            + GenerateWhereClause(PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room WHERE p_venue_key_n = ? AND pc_building_code_c =" +
                        " ? AND pc_room_number_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room" + GenerateWhereClause(PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + " FROM PUB_pc_room WHERE p_venue_key_n = ? AND pc_building_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(DataSet AData, Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            LoadViaPcBuilding(AData, AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(DataSet AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuilding(AData, AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcBuilding(FillDataSet, AVenueKey, ABuildingCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            LoadViaPcBuilding(out AData, AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuilding(out PcRoomTable AData, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuilding(out AData, AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(DataSet ADataSet, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + " FROM PUB_pc_room, PUB_pc_building WHERE pc_room.p_venue_key_n = pc_building.p_ve" +
                    "nue_key_n AND pc_room.pc_building_code_c = pc_building.pc_building_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_building", PcBuildingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(DataSet AData, PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(DataSet AData, PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcBuildingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcBuilding(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room WHERE p_venue_key_n = ? AND pc_building_code_c =" +
                        " ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcBuildingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_building WHERE pc_room.p_venue_key_n = p" +
                        "c_building.p_venue_key_n AND pc_room.pc_building_code_c = pc_building.pc_buildin" +
                        "g_code_c" + GenerateWhereClauseLong("PUB_pc_building", PcBuildingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcBuildingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPcRoomAttributeType(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + @" FROM PUB_pc_room, PUB_pc_room_attribute WHERE PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAttributeType(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomTable AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(out AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(out AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet ADataSet, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c"}) + @" FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAttributeTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(@"SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_room_attribute WHERE PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c" + GenerateWhereClauseLong("PUB_pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomAttributeTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_room WHERE p_venue_key_n = ? AND pc_building_code_c = ? AND pc" +
                    "_room_number_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_room" + GenerateWhereClause(PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcRoomTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_room", PcRoomTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_room", PcRoomTable.GetColumnStringList(), PcRoomTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_room", PcRoomTable.GetColumnStringList(), PcRoomTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcRoom", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Links rooms to attendees of a conference or a booking in the hospitality module
    public class PcRoomAllocAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcRoomAlloc";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_room_alloc";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcRoomAllocTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAllocTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAllocTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc WHERE pc_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAllocTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAllocTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAllocTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc") 
                            + GenerateWhereClause(PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc WHERE pc_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc" + GenerateWhereClause(PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
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
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc WHERE pc_conference_key_n = ? AND p_partner_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcAttendee(out PcRoomAllocTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendee(FillDataSet, AConferenceKey, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcRoomAllocTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendee(out PcRoomAllocTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc, PUB_pc_attendee WHERE pc_room_alloc.pc_conference_key_n " +
                    "= pc_attendee.pc_conference_key_n AND pc_room_alloc.p_partner_key_n = pc_attende" +
                    "e.p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcAttendeeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc WHERE pc_conference_key_n = ? AND p_partne" +
                        "r_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_pc_attendee WHERE pc_room_alloc.pc_co" +
                        "nference_key_n = pc_attendee.pc_conference_key_n AND pc_room_alloc.p_partner_key" +
                        "_n = pc_attendee.p_partner_key_n" + GenerateWhereClauseLong("PUB_pc_attendee", PcAttendeeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcAttendeeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc WHERE p_venue_key_n = ? AND pc_building_code_c = ? AND pc" +
                    "_room_number_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAllocTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoom(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAllocTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAllocTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc, PUB_pc_room WHERE pc_room_alloc.p_venue_key_n = pc_room." +
                    "p_venue_key_n AND pc_room_alloc.pc_building_code_c = pc_room.pc_building_code_c " +
                    "AND pc_room_alloc.pc_room_number_c = pc_room.pc_room_number_c") 
                            + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc WHERE p_venue_key_n = ? AND pc_building_co" +
                        "de_c = ? AND pc_room_number_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_pc_room WHERE pc_room_alloc.p_venue_k" +
                        "ey_n = pc_room.p_venue_key_n AND pc_room_alloc.pc_building_code_c = pc_room.pc_b" +
                        "uilding_code_c AND pc_room_alloc.pc_room_number_c = pc_room.pc_room_number_c" + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPhBooking(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc, PUB_ph_room_booking WHERE PUB_ph_room_booking.ph_room_al" +
                    "loc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i " +
                    "= ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PcRoomAllocTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPhBooking(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PcRoomAllocTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PcRoomAllocTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet ADataSet, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, new string[] {
                            "pc_key_i"}) + " FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE PUB_ph_room_bo" +
                    "oking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.p" +
                    "h_booking_key_i = PUB_ph_booking.ph_key_i") 
                            + GenerateWhereClauseLong("PUB_ph_booking", PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAllocTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPhBookingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_ph_room_booking WHERE PUB_ph_room_boo" +
                        "king.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph" +
                        "_booking_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE" +
                        " PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph" +
                        "_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i" + GenerateWhereClauseLong("PUB_ph_booking", PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PhBookingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_room_alloc WHERE pc_key_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_room_alloc" + GenerateWhereClause(PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcRoomAllocTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        ((PcRoomAllocRow)(TheRow)).Key = ((Int32)(DBAccess.GDBAccessObj.GetNextSequenceValue("seq_room_alloc", ATransaction)));
                        TTypedDataAccess.InsertRow("pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), PcRoomAllocTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), PcRoomAllocTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcRoomAlloc", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Contains type of attributes that can be assigned to a room
    public class PcRoomAttributeTypeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcRoomAttributeType";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_room_attribute_type";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_code_c"}) + " FROM PUB_pc_room_attribute_type") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcRoomAttributeTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAttributeTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAttributeTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "pc_code_c"}) + " FROM PUB_pc_room_attribute_type WHERE pc_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTypeTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTypeTable AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTypeTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "pc_code_c"}) + " FROM PUB_pc_room_attribute_type") 
                            + GenerateWhereClause(PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute_type", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute_type WHERE pc_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute_type" + GenerateWhereClause(PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPcRoom(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room_attribute_type", AFieldList, new string[] {
                            "pc_code_c"}) + @" FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute WHERE PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = ? AND PUB_pc_room_attribute.pc_building_code_c = ? AND PUB_pc_room_attribute.pc_room_number_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTypeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoom(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTypeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTypeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_attribute_type", AFieldList, new string[] {
                            "pc_code_c"}) + @" FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c") 
                            + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(@"SELECT COUNT(*) FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute WHERE PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = ? AND PUB_pc_room_attribute.pc_building_code_c = ? AND PUB_pc_room_attribute.pc_room_number_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar((@"SELECT COUNT(*) FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c" + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_room_attribute_type WHERE pc_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_room_attribute_type" + GenerateWhereClause(PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcRoomAttributeTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), PcRoomAttributeTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), PcRoomAttributeTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcRoomAttributeType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Attributes assigned to rooms used for accommodation at a conference
    public class PcRoomAttributeAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PcRoomAttribute";
        
        /// original table name in database
        public const string DBTABLENAME = "pc_room_attribute";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcRoomAttributeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAttributeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PcRoomAttributeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[3].Value = ((object)(ARoomAttrTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute WHERE p_venue_key_n = ? AND pc_building_code_c = ? AN" +
                    "D pc_room_number_c = ? AND pc_room_attr_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute") 
                            + GenerateWhereClause(PcRoomAttributeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcRoomAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, PcRoomAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, PcRoomAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[3].Value = ((object)(ARoomAttrTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute WHERE p_venue_key_n = ? AND pc_buildin" +
                        "g_code_c = ? AND pc_room_number_c = ? AND pc_room_attr_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute" + GenerateWhereClause(PcRoomAttributeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute WHERE p_venue_key_n = ? AND pc_building_code_c = ? AN" +
                    "D pc_room_number_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(DataSet AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoom(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoom(out PcRoomAttributeTable AData, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoom(out AData, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_attribute", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute, PUB_pc_room WHERE pc_room_attribute.p_venue_key_n = " +
                    "pc_room.p_venue_key_n AND pc_room_attribute.pc_building_code_c = pc_room.pc_buil" +
                    "ding_code_c AND pc_room_attribute.pc_room_number_c = pc_room.pc_room_number_c") 
                            + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute WHERE p_venue_key_n = ? AND pc_buildin" +
                        "g_code_c = ? AND pc_room_number_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute, PUB_pc_room WHERE pc_room_attribute.p" +
                        "_venue_key_n = pc_room.p_venue_key_n AND pc_room_attribute.pc_building_code_c = " +
                        "pc_room.pc_building_code_c AND pc_room_attribute.pc_room_number_c = pc_room.pc_r" +
                        "oom_number_c" + GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute WHERE pc_room_attr_type_code_c = ?") 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomAttributeTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAttributeType(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomAttributeTable AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(out AData, ACode, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeType(out PcRoomAttributeTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeType(out AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet ADataSet, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_attribute", AFieldList, new string[] {
                            "p_venue_key_n",
                            "pc_building_code_c",
                            "pc_room_number_c",
                            "pc_room_attr_type_code_c"}) + " FROM PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE pc_room_attribute.p" +
                    "c_room_attr_type_code_c = pc_room_attribute_type.pc_code_c") 
                            + GenerateWhereClauseLong("PUB_pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PcRoomAttributeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAttributeTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute WHERE pc_room_attr_type_code_c = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE pc_r" +
                        "oom_attribute.pc_room_attr_type_code_c = pc_room_attribute_type.pc_code_c" + GenerateWhereClauseLong("PUB_pc_room_attribute_type", PcRoomAttributeTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomAttributeTypeTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AVenueKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[1].Value = ((object)(ABuildingCode));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[2].Value = ((object)(ARoomNumber));
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[3].Value = ((object)(ARoomAttrTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_pc_room_attribute WHERE p_venue_key_n = ? AND pc_building_code_c " +
                    "= ? AND pc_room_number_c = ? AND pc_room_attr_type_code_c = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_pc_room_attribute" + GenerateWhereClause(PcRoomAttributeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PcRoomAttributeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("pc_room_attribute", PcRoomAttributeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("pc_room_attribute", PcRoomAttributeTable.GetColumnStringList(), PcRoomAttributeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("pc_room_attribute", PcRoomAttributeTable.GetColumnStringList(), PcRoomAttributeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PcRoomAttribute", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// Links room allocations and a booking
    public class PhRoomBookingAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PhRoomBooking";
        
        /// original table name in database
        public const string DBTABLENAME = "ph_room_booking";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking") 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PhRoomBookingTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PhRoomBookingTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PhRoomBookingTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ABookingKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ARoomAllocKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking WHERE ph_booking_key_i = ? AND ph_room_alloc_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ABookingKey, ARoomAllocKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ABookingKey, ARoomAllocKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhRoomBookingTable AData, Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ABookingKey, ARoomAllocKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhRoomBookingTable AData, Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ABookingKey, ARoomAllocKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhRoomBookingTable AData, Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ABookingKey, ARoomAllocKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking") 
                            + GenerateWhereClause(PhRoomBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PhRoomBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PhRoomBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, PhRoomBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, PhRoomBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_room_booking", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ABookingKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ARoomAllocKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_room_booking WHERE ph_booking_key_i = ? AND ph_room_a" +
                        "lloc_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_room_booking" + GenerateWhereClause(PhRoomBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPhBooking(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking WHERE ph_booking_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PhRoomBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPhBooking(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PhRoomBookingTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBooking(out PhRoomBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBooking(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet ADataSet, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_room_booking", AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking, PUB_ph_booking WHERE ph_room_booking.ph_booking_key_i " +
                    "= ph_booking.ph_key_i") 
                            + GenerateWhereClauseLong("PUB_ph_booking", PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPhBookingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_room_booking WHERE ph_booking_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_room_booking, PUB_ph_booking WHERE ph_room_booking.ph" +
                        "_booking_key_i = ph_booking.ph_key_i" + GenerateWhereClauseLong("PUB_ph_booking", PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PhBookingTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking WHERE ph_room_alloc_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhRoomBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAlloc(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhRoomBookingTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhRoomBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet ADataSet, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_room_booking", AFieldList, new string[] {
                            "ph_booking_key_i",
                            "ph_room_alloc_key_i"}) + " FROM PUB_ph_room_booking, PUB_pc_room_alloc WHERE ph_room_booking.ph_room_alloc_" +
                    "key_i = pc_room_alloc.pc_key_i") 
                            + GenerateWhereClauseLong("PUB_pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhRoomBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhRoomBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAllocTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_room_booking WHERE ph_room_alloc_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_room_booking, PUB_pc_room_alloc WHERE ph_room_booking" +
                        ".ph_room_alloc_key_i = pc_room_alloc.pc_key_i" + GenerateWhereClauseLong("PUB_pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomAllocTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(ABookingKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = ((object)(ARoomAllocKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_ph_room_booking WHERE ph_booking_key_i = ? AND ph_room_alloc_key_" +
                    "i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_ph_room_booking" + GenerateWhereClause(PhRoomBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PhRoomBookingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        TTypedDataAccess.InsertRow("ph_room_booking", PhRoomBookingTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("ph_room_booking", PhRoomBookingTable.GetColumnStringList(), PhRoomBookingTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("ph_room_booking", PhRoomBookingTable.GetColumnStringList(), PhRoomBookingTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PhRoomBooking", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
    
    /// make sure charging works for a group or an individual; this summarises all the hospitality services that have to be paid for; also useful for planning meals in the kitchen and room preparation
    public class PhBookingAccess : TTypedDataAccess
    {
        
        /// CamelCase version of table name
        public const string DATATABLENAME = "PhBooking";
        
        /// original table name in database
        public const string DBTABLENAME = "ph_booking";
        
        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking") 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PhBookingTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadAll(out PhBookingTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadAll(out PhBookingTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking WHERE ph_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhBookingTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadByPrimaryKey(out PhBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking") 
                            + GenerateWhereClause(PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking", ATransaction, false));
        }
        
        /// this method is called by all overloads
        public static int CountByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking WHERE ph_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// this method is called by all overloads
        public static int CountUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking" + GenerateWhereClause(PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }
        
        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking WHERE p_contact_key_n = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartner(out PhBookingTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out PhBookingTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartner(out PhBookingTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking, PUB_p_partner WHERE ph_booking.p_contact_key_n = p_partner." +
                    "p_partner_key_n") 
                            + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking WHERE p_contact_key_n = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking, PUB_p_partner WHERE ph_booking.p_contact_key" +
                        "_n = p_partner.p_partner_key_n" + GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPartnerTable.GetPrimKeyColumnOrdList())));
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
                            "ph_key_i"}) + " FROM PUB_ph_booking WHERE a_ledger_number_for_invoice_i = ? AND a_ar_invoice_key" +
                    "_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaAArInvoice(out PhBookingTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoice(FillDataSet, ALedgerNumber, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAArInvoice(out PhBookingTable AData, Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAArInvoice(out PhBookingTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoice(out AData, ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking, PUB_a_ar_invoice WHERE ph_booking.a_ledger_number_for_invoi" +
                    "ce_i = a_ar_invoice.a_ledger_number_i AND ph_booking.a_ar_invoice_key_i = a_ar_i" +
                    "nvoice.a_key_i") 
                            + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
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
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaAArInvoiceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking WHERE a_ledger_number_for_invoice_i = ? AND a" +
                        "_ar_invoice_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking, PUB_a_ar_invoice WHERE ph_booking.a_ledger_n" +
                        "umber_for_invoice_i = a_ar_invoice.a_ledger_number_i AND ph_booking.a_ar_invoice" +
                        "_key_i = a_ar_invoice.a_key_i" + GenerateWhereClauseLong("PUB_a_ar_invoice", AArInvoiceTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, AArInvoiceTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated LoadViaLinkTable
        public static void LoadViaPcRoomAlloc(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking, PUB_ph_room_booking WHERE PUB_ph_room_booking.ph_booking_ke" +
                    "y_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = ?") 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAlloc(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhBookingTable AData, Int32 AKey, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(out AData, AKey, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAlloc(out PhBookingTable AData, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAlloc(out AData, AKey, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet ADataSet, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, new string[] {
                            "ph_key_i"}) + " FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE PUB_ph_room_bo" +
                    "oking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room" +
                    "_alloc_key_i = PUB_pc_room_alloc.pc_key_i") 
                            + GenerateWhereClauseLong("PUB_pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                            + GenerateOrderByClause(AOrderBy)), PhBookingTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAllocTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }
        
        /// auto generated CountViaLinkTable
        public static int CountViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking, PUB_ph_room_booking WHERE PUB_ph_room_bookin" +
                        "g.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_all" +
                        "oc_key_i = ?", ATransaction, false, ParametersArray));
        }
        
        /// auto generated
        public static int CountViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE" +
                        " PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_" +
                        "booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i" + GenerateWhereClauseLong("PUB_pc_room_alloc", PcRoomAllocTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PcRoomAllocTable.GetPrimKeyColumnOrdList())));
        }
        
        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_ph_booking WHERE ph_key_i = ?", ATransaction, false, ParametersArray);
        }
        
        /// auto generated
        public static void DeleteUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_ph_booking" + GenerateWhereClause(PhBookingTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }
        
        /// auto generated
        public static bool SubmitChanges(PhBookingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
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
                        ((PhBookingRow)(TheRow)).Key = ((Int32)(DBAccess.GDBAccessObj.GetNextSequenceValue("seq_booking", ATransaction)));
                        TTypedDataAccess.InsertRow("ph_booking", PhBookingTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("ph_booking", PhBookingTable.GetColumnStringList(), PhBookingTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("ph_booking", PhBookingTable.GetColumnStringList(), PhBookingTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PhBooking", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}
