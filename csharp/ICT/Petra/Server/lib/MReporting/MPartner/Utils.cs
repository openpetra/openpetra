//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Remoting.Server;
using System.IO;
using HtmlAgilityPack;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using System.Linq;

namespace Ict.Petra.Server.MReporting.MPartner
{
    public class Utils {

        /// <summary>
        /// Given the DataSet of a Report query this will check if there currently last known entry,
        /// has the consent to use this data in this report, if not it will be replaced by a empty string 
        /// </summary>
        public static DataTable PartnerRemoveUnconsentReportData(DataTable PartnerTable, string CheckConsentCode) {

            // check if its allowed that this data type is showed in report
            foreach (DataRow Row in PartnerTable.Rows)
            {
                Int64 PartnerKey = Int64.Parse(Row["PartnerKey"].ToString());

                // address
                DataConsentTDS LastEntry = TDataHistoryWebConnector.LastKnownEntry(PartnerKey, "address");
                if (LastEntry.PDataHistory.Count == 0 || !LastEntry.PDataHistory[0].AllowedPurposes.Split(',').ToList().Contains(CheckConsentCode))
                {
                    Row["p_street_name_c"] = "";
                    Row["p_city_c"] = "";
                    Row["p_postal_code_c"] = "";
                    Row["p_country_code_c"] = "";
                }
            }



            return PartnerTable;
        }
    }
}