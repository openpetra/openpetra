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
using System.Data;
using System.Data.Odbc;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.App.ClientDomain;

namespace Ict.Petra.Server.MPartner
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
            out string AEmailAddress,
            TDBTransaction ATransaction)
        {
            AEmailAddress = "";
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
                    if (!row.IsEmailAddressNull())
                    {
                        AEmailAddress = row.EmailAddress;
                    }

                    // we also want the post address, need to load the p_location table:
                    AAddress = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, ATransaction);

                    if (CountryTable.DefaultView.Find(AAddress[0].CountryCode) == -1)
                    {
                        CountryTable.Merge(PCountryAccess.LoadByPrimaryKey(AAddress[0].CountryCode, ATransaction));
                    }

                    ACountryNameLocal = CountryTable[CountryTable.DefaultView.Find(AAddress[0].CountryCode)].CountryNameLocal;
                }
            }

            return AAddress != null;
        }

        /// <summary>
        /// get the best postal address of the partners and return in the result table;
        /// you have to check the ValidAddress flag on the result table
        /// </summary>
        public static BestAddressTDSLocationTable AddPostalAddress(DataTable APartnerTable,
            DataColumn APartnerKeyColumn,
            bool AIgnoreForeignAddresses,
            TDBTransaction ATransaction)
        {
            BestAddressTDSLocationTable ResultTable = new BestAddressTDSLocationTable();

            string LocalCountryCode = TAddressTools.GetLocalCountryCode(ATransaction);

            foreach (DataRow partnerRow in APartnerTable.Rows)
            {
                if (partnerRow[APartnerKeyColumn] != DBNull.Value)
                {
                    PLocationTable Address;
                    string CountryNameLocal;
                    string EmailAddress;
                    Int64 PartnerKey = Convert.ToInt64(partnerRow[APartnerKeyColumn]);
                    GetBestAddress(PartnerKey, out Address, out CountryNameLocal, out EmailAddress, ATransaction);

                    if (AIgnoreForeignAddresses)
                    {
                        // ignore all recipients outside of the country. they will receive a PDF anyways
                        if (!Address[0].IsCountryCodeNull() && (Address[0].CountryCode != LocalCountryCode))
                        {
                            continue;
                        }
                    }

                    BestAddressTDSLocationRow row = ResultTable.NewRowTyped();
                    row.PartnerKey = PartnerKey;
                    ResultTable.Rows.Add(row);

                    if (EmailAddress.Length > 0)
                    {
                        row.EmailAddress = EmailAddress;
                    }

                    row.ValidAddress = (Address != null);

                    if (Address == null)
                    {
                        // no best address; only report if emailAddress is empty as well???
                        continue;
                    }

                    row.LocationKey = Address[0].LocationKey;

                    if (!Address[0].IsLocalityNull())
                    {
                        row.Locality = Address[0].Locality;
                    }

                    if (!Address[0].IsStreetNameNull())
                    {
                        row.StreetName = Address[0].StreetName;
                    }

                    if (!Address[0].IsBuilding1Null())
                    {
                        row.Building1 = Address[0].Building1;
                    }

                    if (!Address[0].IsBuilding2Null())
                    {
                        row.Building2 = Address[0].Building2;
                    }

                    if (!Address[0].IsAddress3Null())
                    {
                        row.Address3 = Address[0].Address3;
                    }

                    if (!Address[0].IsCountryCodeNull())
                    {
                        row.CountryCode = Address[0].CountryCode;
                    }

                    row.CountryName = CountryNameLocal;

                    if (!Address[0].IsPostalCodeNull())
                    {
                        row.PostalCode = Address[0].PostalCode;
                    }

                    if (!Address[0].IsCityNull())
                    {
                        row.City = Address[0].City;
                    }
                }
            }

            return ResultTable;
        }

        /// <summary>
        /// return the country code for this installation of OpenPetra.
        /// using the SiteKey to determine the country
        /// </summary>
        public static string GetLocalCountryCode(TDBTransaction ATransaction)
        {
            if (DomainManager.GSiteKey > 0)
            {
                ALedgerTable ledgerTable = ALedgerAccess.LoadByPrimaryKey((int)(DomainManager.GSiteKey / 1000000), ATransaction);

                if (ledgerTable.Rows.Count == 1)
                {
                    return ledgerTable[0].CountryCode;
                }
            }

            // no sitekey, therefore return invalid country code
            return "99";
        }
    }
}