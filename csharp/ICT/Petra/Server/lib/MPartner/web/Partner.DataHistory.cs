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
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors {

    public class TDataHistoryWebConnector {

        public static DataConsetTDS Get(
            Int64 APartnerKey
        )
        {
            return new DataConsetTDS();
        }

        /// <summary>
        /// suppost to be called by SavePartner
        /// it will update the DataHistory entrys with new data
        /// and the users given: type, value
        /// channel, permissions and date 
        /// </summary>
        /// <returns></returns>
        public static void RegisterChanges(
            List<string> JsonChanges
        ) {
            if (JsonChanges.Count == 0) { return; }

            foreach (string JsonObjectString in JsonChanges) {
                DataHistoryChange ChangeObject = JsonConvert.DeserializeObject<DataHistoryChange>(JsonObjectString);

            }

            return;
        }
    }
    /// <summary>
    /// dummy object to parse data into
    /// </summary>
    public class DataHistoryChange {
        public string PartnerKey { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ChannelCode { get; set; }
        public string Permissions { get; set; }
    }
}
