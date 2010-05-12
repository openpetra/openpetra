// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//

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

namespace Ict.Petra.Server.MHospitality.Data.Access
{

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
        public static PcBuildingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcBuildingTable.TableId) + " FROM PUB_pc_building" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadByPrimaryKey(PcBuildingTable.TableId, Data, new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadUsingTemplate(PcBuildingTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadUsingTemplate(PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadUsingTemplate(PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadUsingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadUsingTemplate(PcBuildingTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcBuildingTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcBuildingTable Data = new PcBuildingTable();
            LoadViaForeignKey(PcBuildingTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcBuildingTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcRoomTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcRoomTable.TableId) + " FROM PUB_pc_room" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadByPrimaryKey(PcRoomTable.TableId, Data, new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadUsingTemplate(PcRoomTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadUsingTemplate(PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadUsingTemplate(PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadUsingTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadUsingTemplate(PcRoomTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcBuilding(Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, Data, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                new System.Object[2]{AVenueKey, ABuildingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuilding(Int64 AVenueKey, String ABuildingCode, TDBTransaction ATransaction)
        {
            return LoadViaPcBuilding(AVenueKey, ABuildingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuilding(Int64 AVenueKey, String ABuildingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcBuilding(AVenueKey, ABuildingCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcBuildingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, Data, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuildingTemplate(PcBuildingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcBuildingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuildingTemplate(PcBuildingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcBuildingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuildingTemplate(PcBuildingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcBuildingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcBuildingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PcBuildingTable.TableId, Data, new string[2]{"p_venue_key_n", "pc_building_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuildingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcBuildingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcBuildingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcBuildingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomTable Data = new PcRoomTable();
            LoadViaForeignKey(PcRoomTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcRoomAttributeType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomTable Data = new PcRoomTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAttributeType(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeType(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeType(ACode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomTable Data = new PcRoomTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAttributeTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomTable Data = new PcRoomTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAttributeTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcRoomAllocTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcRoomAllocTable.TableId) + " FROM PUB_pc_room_alloc" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadByPrimaryKey(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadByPrimaryKey(PcRoomAllocTable.TableId, Data, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadByPrimaryKey(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadUsingTemplate(PcRoomAllocTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadUsingTemplate(PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadUsingTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadUsingTemplate(PcRoomAllocTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAllocTable Data = new PcRoomAllocTable();
            LoadViaForeignKey(PcRoomAllocTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPhBooking(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAllocTable Data = new PcRoomAllocTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPhBooking(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaPhBooking(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBooking(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBooking(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAllocTable Data = new PcRoomAllocTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPhBookingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAllocTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAllocTable Data = new PcRoomAllocTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPhBookingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAllocTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_room_alloc", "pc_key_i");
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
        public static PcRoomAttributeTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcRoomAttributeTypeTable.TableId) + " FROM PUB_pc_room_attribute_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            LoadByPrimaryKey(PcRoomAttributeTypeTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadUsingTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            LoadUsingTemplate(PcRoomAttributeTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoom(FillDataSet, AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcRoomAttributeTypeTable Data = new PcRoomAttributeTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTypeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcRoomAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcRoomAttributeTable.TableId) + " FROM PUB_pc_room_attribute" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadByPrimaryKey(PcRoomAttributeTable.TableId, Data, new System.Object[4]{AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadByPrimaryKey(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, String ARoomAttrTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AVenueKey, ABuildingCode, ARoomNumber, ARoomAttrTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadUsingTemplate(PcRoomAttributeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadUsingTemplate(PcRoomAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadUsingTemplate(PcRoomAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadUsingTemplate(PcRoomAttributeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                new System.Object[3]{AVenueKey, ABuildingCode, ARoomNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoom(Int64 AVenueKey, String ABuildingCode, String ARoomNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoom(AVenueKey, ABuildingCode, ARoomNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(PcRoomRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomTable.TableId, Data, new string[3]{"p_venue_key_n", "pc_building_code_c", "pc_room_number_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoomAttributeType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, Data, new string[1]{"pc_room_attr_type_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeType(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeType(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeType(ACode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, Data, new string[1]{"pc_room_attr_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(PcRoomAttributeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcRoomAttributeTable Data = new PcRoomAttributeTable();
            LoadViaForeignKey(PcRoomAttributeTable.TableId, PcRoomAttributeTypeTable.TableId, Data, new string[1]{"pc_room_attr_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcRoomAttributeTable LoadViaPcRoomAttributeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAttributeTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PhRoomBookingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PhRoomBookingTable.TableId) + " FROM PUB_ph_room_booking" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadByPrimaryKey(PhRoomBookingTable.TableId, Data, new System.Object[2]{ABookingKey, ARoomAllocKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ABookingKey, ARoomAllocKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadByPrimaryKey(Int32 ABookingKey, Int32 ARoomAllocKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ABookingKey, ARoomAllocKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadUsingTemplate(PhRoomBookingTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadUsingTemplate(PhRoomBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadUsingTemplate(PhRoomBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadUsingTemplate(PhRoomBookingTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPhBooking(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, Data, new string[1]{"ph_booking_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBooking(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaPhBooking(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBooking(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBooking(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, Data, new string[1]{"ph_booking_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBookingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PhBookingTable.TableId, Data, new string[1]{"ph_booking_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPhBookingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPhBookingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPcRoomAlloc(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, Data, new string[1]{"ph_room_alloc_key_i"},
                new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAlloc(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAlloc(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAlloc(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, Data, new string[1]{"ph_room_alloc_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhRoomBookingTable Data = new PhRoomBookingTable();
            LoadViaForeignKey(PhRoomBookingTable.TableId, PcRoomAllocTable.TableId, Data, new string[1]{"ph_room_alloc_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhRoomBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PhBookingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PhBookingTable.TableId) + " FROM PUB_ph_booking" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadByPrimaryKey(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadByPrimaryKey(PhBookingTable.TableId, Data, new System.Object[1]{AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadByPrimaryKey(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadByPrimaryKey(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadUsingTemplate(PhBookingTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadUsingTemplate(PhBookingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadUsingTemplate(PhBookingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadUsingTemplate(PhBookingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadUsingTemplate(PhBookingTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_contact_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_contact_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_contact_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                new System.Object[2]{ALedgerNumber, AKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoice(Int32 ALedgerNumber, Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoice(ALedgerNumber, AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoiceTemplate(AArInvoiceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PhBookingTable Data = new PhBookingTable();
            LoadViaForeignKey(PhBookingTable.TableId, AArInvoiceTable.TableId, Data, new string[2]{"a_ledger_number_for_invoice_i", "a_ar_invoice_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaAArInvoiceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAArInvoiceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPcRoomAlloc(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PhBookingTable Data = new PhBookingTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAlloc(FillDataSet, AKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAlloc(Int32 AKey, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAlloc(AKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAlloc(Int32 AKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAlloc(AKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PhBookingTable Data = new PhBookingTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAllocTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAllocTemplate(PcRoomAllocRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PhBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PhBookingTable Data = new PhBookingTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcRoomAllocTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PhBookingTable LoadViaPcRoomAllocTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcRoomAllocTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_booking", "ph_key_i");
        }
    }
}
