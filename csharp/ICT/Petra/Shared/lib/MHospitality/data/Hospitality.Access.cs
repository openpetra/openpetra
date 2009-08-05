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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcBuildingTable.TableId) + " FROM PUB_pc_building") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcBuildingTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PcBuildingTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcBuildingTable.TableId) + " FROM PUB_pc_building" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PcBuildingTable.TableId, ADataSet, new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcBuildingTable();
            LoadByPrimaryKey(PcBuildingTable.TableId, AData, new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PcBuildingTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcBuildingTable();
            LoadUsingTemplate(PcBuildingTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcBuildingTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcBuildingTable();
            LoadUsingTemplate(PcBuildingTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_building", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            return Exists(PcBuildingTable.TableId, new System.Object[2]{AVenueKey, ABuildingCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_building" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcBuildingTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcBuildingTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_building" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcBuildingTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcBuildingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPVenueTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcBuildingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcBuildingTable.TableId, new System.Object[2]{AVenueKey, ABuildingCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcBuildingTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcBuildingTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcBuildingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcBuildingTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcRoomTable.TableId) + " FROM PUB_pc_room") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PcRoomTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcRoomTable.TableId) + " FROM PUB_pc_room" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PcRoomTable.TableId, ADataSet, new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomTable();
            LoadByPrimaryKey(PcRoomTable.TableId, AData, new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PcRoomTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomTable();
            LoadUsingTemplate(PcRoomTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcRoomTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomTable();
            LoadUsingTemplate(PcRoomTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return Exists(PcRoomTable.TableId, new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcRoomTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcRoomTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcBuilding(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, ADataSet, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, AData, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, ADataSet, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, AData, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcBuildingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, ADataSet, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcBuildingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcBuildingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, AData, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcBuildingTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcBuildingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcBuilding(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                new System.Object[2]{AVenueKey, ABuildingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcBuildingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcBuildingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPVenue(out PcRoomTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcRoomTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcRoomTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPcRoomAttributeType(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room", AFieldList, PcRoomTable.TableId) +
                            " FROM PUB_pc_room, PUB_pc_room_attribute WHERE " +
                            "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room", AFieldList, PcRoomTable.TableId) +
                            " FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE " +
                            "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c") +
                            GenerateWhereClauseLong("PUB_pc_room_attribute_type", PcRoomAttributeTypeTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomAttributeTypeTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room", AFieldList, PcRoomTable.TableId) +
                            " FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE " +
                            "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c") +
                            GenerateWhereClauseLong("PUB_pc_room_attribute_type", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAttributeTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 40);
            ParametersArray[0].Value = ((object)(ACode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_room_attribute WHERE " +
                        "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE " +
                        "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c" +
                        GenerateWhereClauseLong("PUB_pc_room_attribute", PcRoomTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PcRoomAttributeTypeTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room, PUB_pc_room_attribute, PUB_pc_room_attribute_type WHERE " +
                        "PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c AND PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c" +
                        GenerateWhereClauseLong("PUB_pc_room_attribute", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcRoomTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcRoomTable.TableId, new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcRoomTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcRoomTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcRoomAllocTable.TableId) + " FROM PUB_pc_room_alloc") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAllocTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcRoomAllocTable.TableId) + " FROM PUB_pc_room_alloc" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PcRoomAllocTable.TableId, ADataSet, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadByPrimaryKey(PcRoomAllocTable.TableId, AData, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PcRoomAllocTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadUsingTemplate(PcRoomAllocTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcRoomAllocTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadUsingTemplate(PcRoomAllocTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AKey, TDBTransaction ATransaction)
        {
            return Exists(PcRoomAllocTable.TableId, new System.Object[1]{AKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAllocTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcRoomAllocTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAllocTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcRoomAllocTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcConference(out PcRoomAllocTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcRoomAllocTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcRoomAllocTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPerson(out PcRoomAllocTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcRoomAllocTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcRoomAllocTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcRoom(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPhBooking(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, PcRoomAllocTable.TableId) +
                            " FROM PUB_pc_room_alloc, PUB_ph_room_booking WHERE " +
                            "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAllocTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, PcRoomAllocTable.TableId) +
                            " FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE " +
                            "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i") +
                            GenerateWhereClauseLong("PUB_ph_booking", PhBookingTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAllocTable.TableId), ATransaction,
                            GetParametersForWhereClause(PhBookingTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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

        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_alloc", AFieldList, PcRoomAllocTable.TableId) +
                            " FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE " +
                            "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i") +
                            GenerateWhereClauseLong("PUB_ph_booking", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAllocTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomAllocTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAllocTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPhBookingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PcRoomAllocTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_ph_room_booking WHERE " +
                        "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE " +
                        "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i" +
                        GenerateWhereClauseLong("PUB_ph_room_booking", PcRoomAllocTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PhBookingTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_alloc, PUB_ph_room_booking, PUB_ph_booking WHERE " +
                        "PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i AND PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i" +
                        GenerateWhereClauseLong("PUB_ph_room_booking", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcRoomAllocTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcRoomAllocTable.TableId, new System.Object[1]{AKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAllocTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAllocTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcRoomAllocTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcRoomAllocTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_room_alloc", "pc_key_i");
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcRoomAttributeTypeTable.TableId) + " FROM PUB_pc_room_attribute_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAttributeTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTypeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcRoomAttributeTypeTable.TableId) + " FROM PUB_pc_room_attribute_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PcRoomAttributeTypeTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTypeTable();
            LoadByPrimaryKey(PcRoomAttributeTypeTable.TableId, AData, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTypeTable();
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAttributeTypeTable();
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(PcRoomAttributeTypeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAttributeTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcRoomAttributeTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAttributeTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcRoomAttributeTypeTable.TableId, ASearchCriteria)));
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_room_attribute_type", AFieldList, PcRoomAttributeTypeTable.TableId) +
                            " FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute WHERE " +
                            "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = ? AND PUB_pc_room_attribute.pc_building_code_c = ? AND PUB_pc_room_attribute.pc_room_number_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAttributeTypeTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_attribute_type", AFieldList, PcRoomAttributeTypeTable.TableId) +
                            " FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE " +
                            "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c") +
                            GenerateWhereClauseLong("PUB_pc_room", PcRoomTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAttributeTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_room_attribute_type", AFieldList, PcRoomAttributeTypeTable.TableId) +
                            " FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE " +
                            "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c") +
                            GenerateWhereClauseLong("PUB_pc_room", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAttributeTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomAttributeTypeTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute WHERE " +
                        "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = ? AND PUB_pc_room_attribute.pc_building_code_c = ? AND PUB_pc_room_attribute.pc_room_number_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE " +
                        "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c" +
                        GenerateWhereClauseLong("PUB_pc_room_attribute", PcRoomAttributeTypeTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PcRoomTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute_type, PUB_pc_room_attribute, PUB_pc_room WHERE " +
                        "PUB_pc_room_attribute.pc_room_attr_type_code_c = PUB_pc_room_attribute_type.pc_code_c AND PUB_pc_room_attribute.p_venue_key_n = PUB_pc_room.p_venue_key_n AND PUB_pc_room_attribute.pc_building_code_c = PUB_pc_room.pc_building_code_c AND PUB_pc_room_attribute.pc_room_number_c = PUB_pc_room.pc_room_number_c" +
                        GenerateWhereClauseLong("PUB_pc_room_attribute", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcRoomAttributeTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcRoomAttributeTypeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAttributeTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAttributeTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcRoomAttributeTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcRoomAttributeTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcRoomAttributeTable.TableId) + " FROM PUB_pc_room_attribute") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcRoomAttributeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcRoomAttributeTable.TableId) + " FROM PUB_pc_room_attribute" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PcRoomAttributeTable.TableId, ADataSet, new System.Object[4]{AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadByPrimaryKey(PcRoomAttributeTable.TableId, AData, new System.Object[4]{AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PcRoomAttributeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadUsingTemplate(PcRoomAttributeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcRoomAttributeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAttributeTable();
            LoadUsingTemplate(PcRoomAttributeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_room_attribute", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PcRoomAttributeTable.TableId, new System.Object[4]{AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAttributeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcRoomAttributeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_room_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcRoomAttributeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcRoomAttributeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcRoom(DataSet ADataSet, Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcRoomTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, ADataSet, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, AData, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeType(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, ADataSet, new string[1]{"pc_room_attr_type_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, AData, new string[1]{"pc_room_attr_type_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, ADataSet, new string[1]{"pc_room_attr_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, AData, new string[1]{"pc_room_attr_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, ADataSet, new string[1]{"pc_room_attr_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, AData, new string[1]{"pc_room_attr_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAttributeTypeTemplate(out PcRoomAttributeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAttributeTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, new string[1]{"pc_room_attr_type_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, new string[1]{"pc_room_attr_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, new string[1]{"pc_room_attr_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcRoomAttributeTable.TableId, new System.Object[4]{AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAttributeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcRoomAttributeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcRoomAttributeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcRoomAttributeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PhRoomBookingTable.TableId) + " FROM PUB_ph_room_booking") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PhRoomBookingTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PhRoomBookingTable.TableId) + " FROM PUB_ph_room_booking" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PhRoomBookingTable.TableId, ADataSet, new System.Object[2]{ABookingKey, ARoomAllocKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadByPrimaryKey(PhRoomBookingTable.TableId, AData, new System.Object[2]{ABookingKey, ARoomAllocKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PhRoomBookingTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadUsingTemplate(PhRoomBookingTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PhRoomBookingTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhRoomBookingTable();
            LoadUsingTemplate(PhRoomBookingTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_room_booking", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            return Exists(PhRoomBookingTable.TableId, new System.Object[2]{ABookingKey, ARoomAllocKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_room_booking" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PhRoomBookingTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PhRoomBookingTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_room_booking" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PhRoomBookingTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PhRoomBookingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPhBooking(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, ADataSet, new string[1]{"ph_booking_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, AData, new string[1]{"ph_booking_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, ADataSet, new string[1]{"ph_booking_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, AData, new string[1]{"ph_booking_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPhBookingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, ADataSet, new string[1]{"ph_booking_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, AData, new string[1]{"ph_booking_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPhBookingTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPhBookingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, new string[1]{"ph_booking_key_i"},
                new System.Object[1]{AKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, new string[1]{"ph_booking_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, new string[1]{"ph_booking_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcRoomAlloc(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, ADataSet, new string[1]{"ph_room_alloc_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, AData, new string[1]{"ph_room_alloc_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, ADataSet, new string[1]{"ph_room_alloc_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, AData, new string[1]{"ph_room_alloc_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPcRoomAllocTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, ADataSet, new string[1]{"ph_room_alloc_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, AData, new string[1]{"ph_room_alloc_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhRoomBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, new string[1]{"ph_room_alloc_key_i"},
                new System.Object[1]{AKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, new string[1]{"ph_room_alloc_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, new string[1]{"ph_room_alloc_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PhRoomBookingTable.TableId, new System.Object[2]{ABookingKey, ARoomAllocKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PhRoomBookingTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PhRoomBookingTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PhRoomBookingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PhRoomBookingTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PhBookingTable.TableId) + " FROM PUB_ph_booking") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PhBookingTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
            AData = new PhBookingTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PhBookingTable.TableId) + " FROM PUB_ph_booking" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
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
            LoadByPrimaryKey(PhBookingTable.TableId, ADataSet, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhBookingTable();
            LoadByPrimaryKey(PhBookingTable.TableId, AData, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadUsingTemplate(PhBookingTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            AData = new PhBookingTable();
            LoadUsingTemplate(PhBookingTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PhBookingTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadUsingTemplate(PhBookingTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AKey, TDBTransaction ATransaction)
        {
            return Exists(PhBookingTable.TableId, new System.Object[1]{AKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PhBookingTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PhBookingTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PhBookingTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PhBookingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_contact_key_n"},
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
        public static void LoadViaPPartner(out PhBookingTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_contact_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_contact_key_n"},
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
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_contact_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_contact_key_n"},
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
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_contact_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, new string[1]{"p_contact_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, new string[1]{"p_contact_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, new string[1]{"p_contact_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAArInvoice(DataSet ADataSet, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
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
        public static void LoadViaAArInvoice(out PhBookingTable AData, Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
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
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaAArInvoiceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, ADataSet, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
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
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, AData, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAArInvoiceTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAArInvoiceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPcRoomAlloc(DataSet ADataSet, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, PhBookingTable.TableId) +
                            " FROM PUB_ph_booking, PUB_ph_room_booking WHERE " +
                            "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PhBookingTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, PhBookingTable.TableId) +
                            " FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE " +
                            "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i") +
                            GenerateWhereClauseLong("PUB_pc_room_alloc", PcRoomAllocTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PhBookingTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcRoomAllocTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_ph_booking", AFieldList, PhBookingTable.TableId) +
                            " FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE " +
                            "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i") +
                            GenerateWhereClauseLong("PUB_pc_room_alloc", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PhBookingTable.TableId), ATransaction,
                            GetParametersForWhereClause(PhBookingTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PhBookingTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcRoomAllocTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcRoomAllocTemplate(out PhBookingTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcRoomAllocTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_ph_booking, PUB_ph_room_booking WHERE " +
                        "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE " +
                        "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i" +
                        GenerateWhereClauseLong("PUB_ph_room_booking", PhBookingTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PcRoomAllocTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_ph_booking, PUB_ph_room_booking, PUB_pc_room_alloc WHERE " +
                        "PUB_ph_room_booking.ph_booking_key_i = PUB_ph_booking.ph_key_i AND PUB_ph_room_booking.ph_room_alloc_key_i = PUB_pc_room_alloc.pc_key_i" +
                        GenerateWhereClauseLong("PUB_ph_room_booking", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PhBookingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PhBookingTable.TableId, new System.Object[1]{AKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PhBookingTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PhBookingTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PhBookingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PhBookingTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_booking", "ph_key_i");
        }
    }
}