//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;

using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.MPartner.Common
{
    ///<summary>
    /// useful functions for the address of a partner
    ///</summary>
    public class TAddressTools
    {
        /// find the current best address for the partner
        public static bool GetBestAddress(Int64 APartnerKey,
            out PLocationTable AAddress,
            out string ACountryNameLocal,
            TDBTransaction ATransaction)
        {
            AAddress = null;
            ACountryNameLocal = "";

            DataSet PartnerLocationsDS = new DataSet();

            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            PartnerLocationsDS.Tables.Add(new PCountryTable());
            DataTable PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];
            PCountryTable CountryTable = (PCountryTable)PartnerLocationsDS.Tables[PCountryTable.GetTableName()];
            CountryTable.DefaultView.Sort = PCountryTable.GetCountryCodeDBName();

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, ATransaction);

            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(PartnerLocationsDS);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    // we also want the post address, need to load the p_location table:
                    AAddress = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, ATransaction);

                    // watch out for empty country codes
                    if (AAddress[0].CountryCode.Trim().Length > 0)
                    {
                        if (CountryTable.DefaultView.Find(AAddress[0].CountryCode) == -1)
                        {
                            CountryTable.Merge(PCountryAccess.LoadByPrimaryKey(AAddress[0].CountryCode, ATransaction));
                        }

                        ACountryNameLocal = CountryTable[CountryTable.DefaultView.Find(AAddress[0].CountryCode)].CountryNameLocal;
                    }

                    break;
                }
            }

            return AAddress != null;
        }

        /// <summary>
        /// Given a DataColumn of Partner Keys, return a table with the best address for each partner
        /// </summary>
        /// <param name="Partners">DataTable containing a column of partner keys</param>
        /// <param name="PartnerKeyColumn">Column number in Partners that contains the keys</param>
        /// <param name="ATransaction">The current database transaction</param>
        /// <returns></returns>
        public static DataTable GetBestAddressForPartners(DataTable Partners, int PartnerKeyColumn, TDBTransaction ATransaction)
        {
            List <String>PartnerList = new List <string>();

            foreach (DataRow Partner in Partners.Rows)
            {
                PartnerList.Add(Partner[PartnerKeyColumn].ToString());
            }

            return GetBestAddressForPartners(String.Join(",", PartnerList), ATransaction);
        }

        /// <summary>
        /// Given a string list of Partner Keys, return a table with the best address for each partner
        /// </summary>
        /// <param name="DonorList">Comma-separated list of partner keys, or SQL query returning partner keys only</param>
        /// <param name="ATransaction">The current database transaction</param>
        /// <returns></returns>
        public static DataTable GetBestAddressForPartners(String DonorList, TDBTransaction ATransaction)
        {
            DataTable ResultTable = new DataTable();
            string Query =
                @"
                WITH address_state AS (
                    SELECT
                        p_partner_key_n
                        , p_site_key_n
                        , p_location_key_i
                        , p_date_effective_d
                        , p_date_good_until_d
                        , p_location_type_c
                        , p_send_mail_l
                        -- State order is:
                        --   Current: 1
                        --   Future: 2
                        --   Expired:  3
                        , CASE
                            WHEN p_date_effective_d <= current_date AND (p_date_good_until_d >= current_date OR p_date_good_until_d is null) THEN 1  -- Current
                            WHEN p_date_effective_d > current_date THEN 2                                                                            -- Future
                            ELSE 3                                                                                                                   -- Expired
                        END AS date_state
                    FROM
                      p_partner_location
                    WHERE
                      p_partner_key_n IN ("
                +
                DonorList +
                @"
                      )
                )
                , best_state AS (
                    SELECT
                        *
                        --
                        -- Best selection is:
                        --   Current:  maximum p_date_effective_d   (most recent current address)
                        --   Future:   minimum p_date_effective_d   (soonest-starting future address)
                        --   Expired:  maximum p_date_good_until_d  (most recently active address)
                        , row_number() OVER (PARTITION BY p_partner_key_n ORDER BY p_send_mail_l DESC, date_state
                            , CASE WHEN date_state = 1 THEN p_date_effective_d END DESC
                            , CASE WHEN date_state = 2 THEN p_date_effective_d END
                            , CASE WHEN date_state = 3 THEN p_date_good_until_d END DESC
                        )
                    FROM
                        address_state
                )
                SELECT
                    best_state.p_partner_key_n
                    , best_state.p_date_effective_d
                    , best_state.p_date_good_until_d
                    , best_state.p_location_type_c
                    , best_state.p_send_mail_l
                    , p_location.*
                FROM
                    best_state INNER JOIN p_location USING (p_site_key_n, p_location_key_i)
                WHERE
                    row_number = 1
                ;
            "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ;

            ResultTable = DBAccess.GetDBAccessObj(ATransaction).SelectDT(Query, "PartnersAddresses", ATransaction);
            return ResultTable;
        }

        /// <summary>
        /// Return the country code for this installation of OpenPetra.
        /// using the SiteKey to determine the country
        /// </summary>
        public static string GetCountryCodeFromSiteLedger(TDBTransaction ATransaction)
        {
            string CountryCode = string.Empty;

            if (DomainManager.GSiteKey > 0)
            {
                Int32 ledgerNumber = (Int32)(DomainManager.GSiteKey / 1000000);
                CountryCode = TLedgerInfo.GetLedgerCountryCode(ledgerNumber);
            }

            if (CountryCode.Length == 0)
            {
                // Domain Manager doesn't know my site key?
                CountryCode = "99";
            }

            return CountryCode;
        }

        /// <summary>
        /// Get the printable name for this country
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        public static string GetCountryName(string CountryCode, TDBTransaction ATransaction)
        {
            PCountryTable Tbl = PCountryAccess.LoadByPrimaryKey(CountryCode, ATransaction);

            if (Tbl.Rows.Count > 0)
            {
                return Tbl[0].CountryName;
            }
            else
            {
                return "";
            }
        }
    }
}