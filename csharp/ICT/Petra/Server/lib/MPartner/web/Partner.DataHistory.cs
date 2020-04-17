//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       cjaekel
//
// Copyright 2004-2020 by OM International
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
using System.Data;
using Ict.Common.DB;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ict.Petra.Server.App.Core.Security;
using Ict.Common;
using System.Data.Odbc;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors {

    public class TDataHistoryWebConnector {

        /// <summary>
        /// Returns all unique data types
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static List<string> GetUniqueTypes(
            Int64 APartnerKey
        )
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get get unique data types");
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();
            List<string> Types = new List<string>();

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT DISTINCT `p_type_c` FROM `p_data_history` " +
                    "WHERE `p_partner_key_n` = ?";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.VarChar) { Value = APartnerKey.ToString() });

                DataTable AllTypes = DB.SelectDT(sql, "UniqueTypes", T, SQLParameter.ToArray());
                foreach (DataRow TypeRow in AllTypes.Rows)
                {
                    Types.Add( TypeRow.Field<string>("p_type_c") );
                }
            });

            return Types;
        }


        /// <summary>
        /// Returns the last known entry for a partner and type, could be empty
        /// also returns all consent channel and purposes
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static DataConsentTDS LastKnownEntry(
            Int64 APartnerKey,
            string ADataType
        )
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Last known entry");
            DataConsentTDS Set = new DataConsentTDS();
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT " +
                    "`p_data_history`.*, " +
                    "GROUP_CONCAT(`p_data_history_permission`.`p_purpose_code_c` SEPARATOR ',') AS `AllowedPurposes` " +
                	"FROM `p_data_history` " +
                    "LEFT JOIN `p_data_history_permission` " +
                    "ON `p_data_history`.`p_entry_id_i` = `p_data_history_permission`.`p_data_history_entry_i` " +
                    "AND `p_data_history_permission`.`p_data_chancled_d` IS NULL " +
                    "WHERE `p_data_history`.`p_partner_key_n` = ? " +
                    "AND `p_data_history`.`p_type_c` = ? " +
                    "GROUP BY `p_data_history`.`p_entry_id_i` " +
                    "ORDER BY `p_data_history`.`p_entry_id_i` DESC " +
                    "LIMIT 1";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.VarChar) { Value = APartnerKey.ToString() } );
                SQLParameter.Add(new OdbcParameter("DataType", OdbcType.VarChar) { Value = ADataType } );

                // DB.SelectDT(Set, sql, T, SQLParameter.ToArray(), 0,0);
                DB.SelectDT(Set.PDataHistory, sql, T, SQLParameter.ToArray()); 

                PConsentChannelAccess.LoadAll(Set, T);
                PPurposeAccess.LoadAll(Set, T);

            });

            return Set;
        }

        /// <summary>
        /// Returns history for given data type
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static DataConsentTDS GetHistory(
            Int64 APartnerKey,
            string ADataType
        )
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get data history for partner");
            DataConsentTDS Set = new DataConsentTDS();
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();

            DB.ReadTransaction(ref T, delegate {

                // prepare for one huge cunk sql
                string sql = "" +
                	"SELECT " +
                	"  `pdh`.*, " +
                	"  GROUP_CONCAT(`pdhp`.`p_purpose_code_c` SEPARATOR ',') AS `AllowedPurposes`, " +
                	"  `pdh`.`s_date_created_d` AS `EventDate`, " +
                	"  'ACTIVE' AS `State` " +
                	"FROM `p_data_history` AS `pdh` " +
                	"LEFT JOIN `p_data_history_permission` AS `pdhp` " +
                	"  ON `pdh`.`p_entry_id_i` = `pdhp`.`p_data_history_entry_i` " +
                	"  AND `pdhp`.`p_data_chancled_d` IS NULL " +
                	"WHERE `pdh`.`p_partner_key_n` = ? " +
                	"  AND `pdh`.`p_type_c` = ? " +
                	"GROUP BY `pdh`.`p_entry_id_i` " +
                	"" +
                	"UNION " +
                	"" +
                    "SELECT " +
                    "  `pdh`.*, " +
                    "  GROUP_CONCAT(`pdhp`.`p_purpose_code_c` SEPARATOR ',') AS `AllowedPurposes`, " +
                    "  `pdhp`.`p_data_chancled_d` AS `EventDate`, " +
                    "  'CANCLED' AS `State` " +
                    "FROM `p_data_history` AS `pdh` " +
                    "JOIN `p_data_history_permission` AS `pdhp` " +
                    "  ON `pdh`.`p_entry_id_i` = `pdhp`.`p_data_history_entry_i` " +
                    "  AND `pdhp`.`p_data_chancled_d` IS NOT NULL " +
                    "WHERE `pdh`.`p_partner_key_n` = ? " +
                    "  AND `pdh`.`p_type_c` = ? " +
                    "GROUP BY `pdh`.`p_entry_id_i`, `pdhp`.`p_data_chancled_d` " +
                    "" +
                    "ORDER BY `EventDate` DESC, `p_entry_id_i` DESC";

                // yeah... twice, because why not
                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey.ToString() });
                SQLParameter.Add(new OdbcParameter("DataType", OdbcType.VarChar) { Value = ADataType });
                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey.ToString() });
                SQLParameter.Add(new OdbcParameter("DataType", OdbcType.VarChar) { Value = ADataType });

                Set.PDataHistory.Constraints.Clear(); //mmmm...
                DB.SelectDT(Set.PDataHistory, sql, T, SQLParameter.ToArray());
                PConsentChannelAccess.LoadAll(Set, T);
                PPurposeAccess.LoadAll(Set, T);

            });

            return Set;

        }


        /// <summary>
        /// Lists all consents channels and purposes
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static DataConsentTDS GetConsentChannelAndPurpose() {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Consent Channel + Purpose");
            DataConsentTDS Set = new DataConsentTDS();

            DB.ReadTransaction(ref T, delegate {

                PConsentChannelAccess.LoadAll(Set, T);
                PPurposeAccess.LoadAll(Set, T);

            });

            return Set;
        }


        /// <summary>
        /// suppost to be called by SavePartner
        /// it will update the DataHistory entrys with new data
        /// and the users given: type, value
        /// channel, permissions and date 
        /// </summary>
        /// <returns></returns>
        [NoRemoting]
        public static void RegisterChanges(
            List<string> JsonChanges
        ) {
            if (JsonChanges.Count == 0) { return; }

            // connection objects
            // bool SubmissionOK = false;
            // TDBTransaction T = new TDBTransaction();
            // TDataBase DB = DBAccess.Connect("Inserting DataHistory Changes");

            // base strings


            // data is comming like this: ["{}", "{}"] 
            foreach (string JsonObjectString in JsonChanges) {
                DataHistoryChange ChangeObject = JsonConvert.DeserializeObject<DataHistoryChange>(JsonObjectString);
                DataConsentTDS Set = new DataConsentTDS();

                // generate and add new row for p_data_history
                PDataHistoryRow NewRow = Set.PDataHistory.NewRowTyped();

                NewRow.EntryId = -1;
                NewRow.PartnerKey = ChangeObject.PartnerKey;
                NewRow.Type = ChangeObject.Type;
                NewRow.Value = ChangeObject.Value;
                NewRow.ChannelCode = ChangeObject.ChannelCode;

                Set.PDataHistory.Rows.Add(NewRow);

                // generate and add each row for a allowed porpose in p_data_history_permission
                foreach (string AllowedPuroseCode in ChangeObject.Permissions.Split(',')) {
                    if (AllowedPuroseCode.Trim().Equals("")) { continue; } // catch non permission values
                    PDataHistoryPermissionRow NewPermRow = Set.PDataHistoryPermission.NewRowTyped();

                    NewPermRow.PurposeCode = AllowedPuroseCode;
                    NewPermRow.DataHistoryEntry = -1;

                    Set.PDataHistoryPermission.Rows.Add(NewPermRow);
                }

                DataConsentTDSAccess.SubmitChanges(Set);
            }

            return;
        }
    }
    /// <summary>
    /// dummy object to parse data into
    /// </summary>
    public class DataHistoryChange {
        public long PartnerKey { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ChannelCode { get; set; }
        public string Permissions { get; set; }
    }
}
