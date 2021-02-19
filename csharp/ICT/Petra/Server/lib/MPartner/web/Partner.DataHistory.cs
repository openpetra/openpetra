//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       cjaekel, timop
//
// Copyright 2004-2021 by OM International
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
using Ict.Common.Verification;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors {

    /// get the data history of a partner
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

                string sql = "SELECT DISTINCT `p_type_c` FROM `p_consent_history` " +
                    "WHERE `p_partner_key_n` = ?";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey });

                DataTable AllTypes = DB.SelectDT(sql, "UniqueTypes", T, SQLParameter.ToArray());
                foreach (DataRow TypeRow in AllTypes.Rows)
                {
                    Types.Add( TypeRow.Field<string>("p_type_c") );
                }

                // also get types for existing values of the address that don't have a consent yet
                if (!Types.Contains(MPartnerConstants.CONSENT_TYPE_ADDRESS))
                {
                    Types.Add(MPartnerConstants.CONSENT_TYPE_ADDRESS);
                }

                if (!Types.Contains(MPartnerConstants.CONSENT_TYPE_EMAIL))
                {
                    Types.Add(MPartnerConstants.CONSENT_TYPE_EMAIL);
                }

                if (!Types.Contains(MPartnerConstants.CONSENT_TYPE_LANDLINE))
                {
                    Types.Add(MPartnerConstants.CONSENT_TYPE_LANDLINE);
                }

                if (!Types.Contains(MPartnerConstants.CONSENT_TYPE_MOBILE))
                {
                    Types.Add(MPartnerConstants.CONSENT_TYPE_MOBILE);
                }
            });

            return Types;
        }

        /// <summary>
        /// Do we have any consent at all for this contact.
        /// This method has been copied to Ict.Petra.Server.MFinance.Gift.WebConnectors.TReceiptingWebConnector
        /// to avoid cyclic dependancies.
        /// </summary>
        private static bool UndefinedConsent(Int64 APartnerKey)
        {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Last known entry");
            List<OdbcParameter> SQLParameter = new List<OdbcParameter>();
            bool HasConsent = false;

            DB.ReadTransaction(ref T, delegate {

                string sql = "SELECT " +
                    "COUNT(*)" +
                    "FROM `p_consent_history` " +
                    "WHERE `p_consent_history`.`p_partner_key_n` = ?";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey } );

                HasConsent = (Convert.ToInt32(DB.ExecuteScalar(sql, T, SQLParameter.ToArray())) > 0);
            });

            return !HasConsent;
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
                    "`p_consent_history`.*, " +
                    "GROUP_CONCAT(`p_consent_history_permission`.`p_purpose_code_c` SEPARATOR ',') AS `AllowedPurposes` " +
                    "FROM `p_consent_history` " +
                    "LEFT JOIN `p_consent_history_permission` " +
                    "ON `p_consent_history`.`p_entry_id_i` = `p_consent_history_permission`.`p_consent_history_entry_i` " +
                    "WHERE `p_consent_history`.`p_partner_key_n` = ? " +
                    "AND `p_consent_history`.`p_type_c` = ? " +
                    "GROUP BY `p_consent_history`.`p_entry_id_i` " +
                    "ORDER BY `p_consent_history`.`p_entry_id_i` DESC " +
                    "LIMIT 1";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey } );
                SQLParameter.Add(new OdbcParameter("DataType", OdbcType.VarChar) { Value = ADataType } );

                DB.SelectDT(Set.PConsentHistory, sql, T, SQLParameter.ToArray()); 

                if (Set.PConsentHistory.Count == 0)
                {
                    // there is no consent yet
                    // do we have a value at all?
                    List<string> Subscriptions;
                    List<string> PartnerTypes;
                    string DefaultEmailAddress;
                    string DefaultPhoneMobile;
                    string DefaultPhoneLandline;
                    PartnerEditTDS PartnerDS = TSimplePartnerEditWebConnector.GetPartnerDetails(APartnerKey,
                        out Subscriptions,
                        out PartnerTypes,
                        out DefaultEmailAddress,
                        out DefaultPhoneMobile,
                        out DefaultPhoneLandline);

                    if (ADataType == MPartnerConstants.CONSENT_TYPE_ADDRESS)
                    {
                        // what about new contact?
                        PLocationRow locationRow = null;

                        if (PartnerDS.PLocation.Rows.Count > 0)
                        {
                            locationRow = PartnerDS.PLocation[0];
                        }
                        else
                        {
                            locationRow = PartnerDS.PLocation.NewRowTyped();
                        }

                        PConsentHistoryRow row = Set.PConsentHistory.NewRowTyped();
                        row.EntryId = -1;
                        row.PartnerKey = APartnerKey;
                        row.Type = ADataType;
                        row.Value = locationRow.StreetName + ", " + locationRow.PostalCode + " " + locationRow.City + ", " + locationRow.CountryCode;
                        row.ConsentDate = DateTime.Today;
                        Set.PConsentHistory.Rows.Add(row);
                    }

                    if (ADataType == MPartnerConstants.CONSENT_TYPE_EMAIL)
                    {
                        PConsentHistoryRow row = Set.PConsentHistory.NewRowTyped();
                        row.EntryId = -1;
                        row.PartnerKey = APartnerKey;
                        row.Type = ADataType;
                        row.Value = DefaultEmailAddress;
                        row.ConsentDate = DateTime.Today;
                        Set.PConsentHistory.Rows.Add(row);
                    }

                    if (ADataType == MPartnerConstants.CONSENT_TYPE_LANDLINE)
                    {
                        PConsentHistoryRow row = Set.PConsentHistory.NewRowTyped();
                        row.EntryId = -1;
                        row.PartnerKey = APartnerKey;
                        row.Type = ADataType;
                        row.Value = DefaultPhoneLandline;
                        row.ConsentDate = DateTime.Today;
                        Set.PConsentHistory.Rows.Add(row);
                    }

                    if (ADataType == MPartnerConstants.CONSENT_TYPE_MOBILE)
                    {
                        PConsentHistoryRow row = Set.PConsentHistory.NewRowTyped();
                        row.EntryId = -1;
                        row.PartnerKey = APartnerKey;
                        row.Type = ADataType;
                        row.Value = DefaultPhoneMobile;
                        row.ConsentDate = DateTime.Today;
                        Set.PConsentHistory.Rows.Add(row);
                    }
                }

                PConsentChannelAccess.LoadAll(Set, T);
                PConsentPurposeAccess.LoadAll(Set, T);

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
                    "  `ch`.*, " +
                    "  GROUP_CONCAT(`chp`.`p_purpose_code_c` SEPARATOR ',') AS `AllowedPurposes` " +
                    "FROM `p_consent_history` AS `ch` " +
                    "LEFT JOIN `p_consent_history_permission` AS `chp` " +
                    "  ON `ch`.`p_entry_id_i` = `chp`.`p_consent_history_entry_i` " +
                    "WHERE `ch`.`p_partner_key_n` = ? " +
                    "  AND `ch`.`p_type_c` = ? " +
                    "GROUP BY `ch`.`p_entry_id_i` " +
                    "ORDER BY `ch`.`p_entry_id_i` DESC";

                SQLParameter.Add(new OdbcParameter("PartnerKey", OdbcType.BigInt) { Value = APartnerKey });
                SQLParameter.Add(new OdbcParameter("DataType", OdbcType.VarChar) { Value = ADataType });

                Set.PConsentHistory.Constraints.Clear(); //mmmm...
                DB.SelectDT(Set.PConsentHistory, sql, T, SQLParameter.ToArray());
                PConsentChannelAccess.LoadAll(Set, T);
                PConsentPurposeAccess.LoadAll(Set, T);

            });

            return Set;

        }

        /// <summary>
        /// Edits the selected consent type to be added or removed consent entrys,
        /// only the last entry for a datatype can be edited
        /// and even duh it says edit, it actually creates a new entry in history
        /// 
        /// `AConsentCodes` is a comma separated list of the new allowed consent codes
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool EditHistory(
            Int64 APartnerKey,
            string ADataType,
            string AChannelCode,
            DateTime AConsentDate,
            string AConsentCodes,
            out TVerificationResultCollection AVerificationResult
        )
        {
            AVerificationResult = new TVerificationResultCollection();
            DataConsentTDS LastEntry = LastKnownEntry(APartnerKey, ADataType);
            DataConsentTDSPConsentHistoryRow LastEntryRow = LastEntry.PConsentHistory[0];

            // tried to save with same permissions, we skip these actions and throw a error
            if (LastEntryRow.AllowedPurposes == AConsentCodes && LastEntryRow.ConsentDate == AConsentDate &&
                LastEntryRow.ChannelCode == AChannelCode) {
                AVerificationResult.Add(new TVerificationResult("error", "no_changes", TResultSeverity.Resv_Critical));
                return false; 
            }

            DataHistoryChange ToChange = new DataHistoryChange
            {
                ConsentDate = AConsentDate,
                ChannelCode = AChannelCode,
                PartnerKey = APartnerKey,
                Type = ADataType,
                Value = LastEntryRow.Value,
                Permissions = AConsentCodes
            };

            String ToRegister = JsonConvert.SerializeObject(ToChange);
            List<string> JsonList = new List<string> { ToRegister };
            RegisterChanges(JsonList, new List<string>() { ADataType });
            return true;
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
                PConsentPurposeAccess.LoadAll(Set, T);

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
        public static bool RegisterChanges(
            List<string> JsonChanges,
            List<string> NeededChanges
        ) {
            foreach (string PreCheck in JsonChanges)
            {
                DataHistoryChange PreCheckObj = JsonConvert.DeserializeObject<DataHistoryChange>(PreCheck);
                if (NeededChanges.Contains(PreCheckObj.Type))
                {
                    NeededChanges.Remove(PreCheckObj.Type);
                }
            }

            if (NeededChanges.Count != 0)
            {
                return false;
            }

            // data is comming like this: ["{}", "{}"] 
            foreach (string JsonObjectString in JsonChanges) {
                DataHistoryChange ChangeObject = JsonConvert.DeserializeObject<DataHistoryChange>(JsonObjectString);
                DataConsentTDS Set = new DataConsentTDS();

                // generate and add new row for p_consent_history
                PConsentHistoryRow NewRow = Set.PConsentHistory.NewRowTyped();

                NewRow.EntryId = -1;
                NewRow.PartnerKey = ChangeObject.PartnerKey;
                NewRow.Type = ChangeObject.Type;
                NewRow.Value = ChangeObject.Value;
                NewRow.ConsentDate = ChangeObject.ConsentDate;
                NewRow.ChannelCode = ChangeObject.ChannelCode;

                Set.PConsentHistory.Rows.Add(NewRow);

                // generate and add each row for a allowed purpose in p_consent_history_permission
                foreach (string AllowedPuroseCode in ChangeObject.Permissions.Split(',')) {
                    if (AllowedPuroseCode.Trim().Equals("")) { continue; } // catch non permission values
                    PConsentHistoryPermissionRow NewPermRow = Set.PConsentHistoryPermission.NewRowTyped();

                    NewPermRow.PurposeCode = AllowedPuroseCode;
                    NewPermRow.ConsentHistoryEntry = -1;

                    Set.PConsentHistoryPermission.Rows.Add(NewPermRow);
                }

                DataConsentTDSAccess.SubmitChanges(Set);
            }

            return true;
        }
    }
    /// <summary>
    /// dummy object to parse data into
    /// </summary>
    public class DataHistoryChange {
        /// PartnerKey
        public long PartnerKey { get; set; }
        /// Type
        public string Type { get; set; }
        /// Value
        public string Value { get; set; }
        /// ConsentDate
        public DateTime ConsentDate { get; set; }
        /// ChannelCode
        public string ChannelCode { get; set; }
        /// Permissions
        public string Permissions { get; set; }
    }
}
