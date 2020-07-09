//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       cjaekel, timop
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
    /// Utilities for GDPR
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
                DataConsentTDS LastEntry;

                // Note: this could be much simplyfied, and i mean a lot, llike cobining it to one sql
                // but for now, we keep it by using already present functions.

                // address
                if (
                    PartnerTable.Columns.Contains("p_street_name_c") ||
                    PartnerTable.Columns.Contains("p_city_c") ||
                    PartnerTable.Columns.Contains("p_postal_code_c") ||
                    PartnerTable.Columns.Contains("p_country_code_c")
                )
                {
                    LastEntry = TDataHistoryWebConnector.LastKnownEntry(PartnerKey, "address");
                    if (LastEntry.PConsentHistory.Count == 0 || !LastEntry.PConsentHistory[0].AllowedPurposes.Split(',').ToList().Contains(CheckConsentCode))
                    {
                        if (PartnerTable.Columns.Contains("p_street_name_c"))
                        {
                            Row["p_street_name_c"] = "";
                        }

                        if (PartnerTable.Columns.Contains("p_city_c"))
                        {
                            Row["p_city_c"] = "";
                        }

                        if (PartnerTable.Columns.Contains("p_postal_code_c"))
                        {
                            Row["p_postal_code_c"] = "";
                        }

                        if (PartnerTable.Columns.Contains("p_country_code_c"))
                        {
                            Row["p_country_code_c"] = "";
                        }
                    }

                }

                // phone mobile
                if (PartnerTable.Columns.Contains("Mobile"))
                {
                    LastEntry = TDataHistoryWebConnector.LastKnownEntry(PartnerKey, "phone mobile");
                    if (LastEntry.PConsentHistory.Count == 0 || !LastEntry.PConsentHistory[0].AllowedPurposes.Split(',').ToList().Contains(CheckConsentCode))
                    {
                        Row["Mobile"] = "";
                    }
                }

                // phone landline
                if (PartnerTable.Columns.Contains("Phone"))
                {
                    LastEntry = TDataHistoryWebConnector.LastKnownEntry(PartnerKey, "phone landline");
                    if (LastEntry.PConsentHistory.Count == 0 || !LastEntry.PConsentHistory[0].AllowedPurposes.Split(',').ToList().Contains(CheckConsentCode))
                    {
                        Row["Phone"] = "";
                    }
                }

                // email address
                if (PartnerTable.Columns.Contains("EMail"))
                {
                    LastEntry = TDataHistoryWebConnector.LastKnownEntry(PartnerKey, "email address");
                    if (LastEntry.PConsentHistory.Count == 0 || !LastEntry.PConsentHistory[0].AllowedPurposes.Split(',').ToList().Contains(CheckConsentCode))
                    {
                        Row["EMail"] = "";
                    }
                }
            }



            return PartnerTable;
        }
    }
}
