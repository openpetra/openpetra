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
using System.Data;
using Ict.Common.DB;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Common.Remoting.Server;

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

            Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Calculations.DetermineBestAddress(PartnerLocationsDS);

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
        /// <param name="APartnerDetails">if true: Adds partner short name and partner class columns</param>
        /// <returns></returns>
        public static DataTable GetBestAddressForPartners(DataTable Partners,
            int PartnerKeyColumn,
            TDBTransaction ATransaction,
            Boolean APartnerDetails = false)
        {
            List <String>PartnerList = new List <string>();

            foreach (DataRow Partner in Partners.Rows)
            {
                PartnerList.Add(Partner[PartnerKeyColumn].ToString());
            }

            return GetBestAddressForPartners(String.Join(",", PartnerList), ATransaction, APartnerDetails);
        }

        /// <summary>
        /// Given a string list of Partner Keys, return a table with the best address for each partner
        /// </summary>
        /// <param name="DonorList">Comma-separated list of partner keys, or SQL query returning partner keys only</param>
        /// <param name="ATransaction">The current database transaction</param>
        /// <param name="APartnerDetails">if true: Adds partner short name and partner class columns</param>
        /// <returns></returns>
        public static DataTable GetBestAddressForPartners(String DonorList, TDBTransaction ATransaction, Boolean APartnerDetails = false)
        {
            DataTable ResultTable = new DataTable();

            if (DonorList == String.Empty)
            {
                return ResultTable;
            }

            string Query = TDataBase.ReadSqlFile("Partner.CommonAddressTools.GetBestAddress.sql");

            Query = Query.Replace("{DonorList}", DonorList);

            if (APartnerDetails)
            {
                Query = "WITH AddressTable AS (" + Query +
                        ") SELECT DISTINCT AddressTable.*, p_partner.p_partner_short_name_c, p_partner.p_partner_class_c FROM AddressTable JOIN p_partner ON p_partner.p_partner_key_n=AddressTable.p_partner_key_n";
            }

            ResultTable = DBAccess.GetDBAccessObj(ATransaction).SelectDT(Query, "PartnersAddresses", ATransaction);
            return ResultTable;
        }

        /// <summary>
        /// Returns the Table handed in with the best addresses
        /// </summary>
        /// <param name="Partners"></param>
        /// <param name="PartnerKeyColumn"></param>
        /// <param name="ATransaction"></param>
        /// <param name="APartnerDetails"></param>
        /// <returns></returns>
        public static DataTable GetBestAddressForPartnersAsJoinedTable(DataTable Partners,
            int PartnerKeyColumn,
            TDBTransaction ATransaction,
            Boolean APartnerDetails = false)
        {
            DataTable Addresses = GetBestAddressForPartners(Partners, PartnerKeyColumn, ATransaction, APartnerDetails);

            foreach (DataColumn dc in Addresses.Columns)
            {
                Partners.Columns.Add("addr_" + dc.ColumnName);
            }

            DataView dv = Partners.DefaultView;

            foreach (DataRow dr in Addresses.Rows)
            {
                dv.RowFilter = "partnerkey = " + dr[0].ToString();

                for (int i = 0; i < Addresses.Columns.Count; i++)
                {
                    dv[0]["addr_" + Addresses.Columns[i].ColumnName] = dr[i];
                }
            }

            dv.RowFilter = String.Empty;

            return dv.ToTable();
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