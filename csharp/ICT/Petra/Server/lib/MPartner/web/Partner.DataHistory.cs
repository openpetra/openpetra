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
using Ict.Common.DB;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ict.Petra.Server.App.Core.Security;
using Ict.Common;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors {

    public class TDataHistoryWebConnector {

        /// <summary>
        /// Returns all consents entrys for a given partner 
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static DataConsetTDS GetHistory(
            Int64 APartnerKey
        )
        {
            return new DataConsetTDS();
        }

        [RequireModulePermission("PTNRUSER")]
        public static DataConsetTDS GetConsentChannelAndPurpose() {
            TDBTransaction T = new TDBTransaction();
            TDataBase DB = DBAccess.Connect("Get Consent Channel + Purpose");
            DataConsetTDS Set = new DataConsetTDS();

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
                DataConsetTDS Set = new DataConsetTDS();

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
                    PDataHistoryPermissionRow NewPermRow = Set.PDataHistoryPermission.NewRowTyped();

                    NewPermRow.PurposeCode = AllowedPuroseCode;
                    NewPermRow.DataHistoryEntry = -1;

                    Set.PDataHistoryPermission.Rows.Add(NewPermRow);
                }

                DataConsetTDSAccess.SubmitChanges(Set);
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
