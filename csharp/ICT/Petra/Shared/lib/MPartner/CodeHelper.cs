//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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

using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Gets descriptions for codes in the Partner Module.
    /// </summary>
    public class PartnerCodeHelper
    {
        /// <summary>
        /// Returns the description of a Marital Status Code.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <param name="AMaritalStatusCode">Marital Status Code.</param>
        /// <returns>The description of a Marital Status Code, or empty string if the Marital Status Code could not be identified.</returns>
        public static string GetMaritalStatusDescription(TGetCacheableDataTableFromCache ACacheRetriever, string AMaritalStatusCode)
        {
            DataTable CachedDT;
            DataRow FoundDR;
            string ReturnValue = "";
            Type tmp;

            CachedDT = ACacheRetriever(Enum.GetName(typeof(TCacheablePartnerTablesEnum), TCacheablePartnerTablesEnum.MaritalStatusList), out tmp);
            FoundDR = CachedDT.Rows.Find(new object[] { AMaritalStatusCode });

            if (FoundDR != null)
            {
                ReturnValue = FoundDR[PtMaritalStatusTable.ColumnDescriptionId].ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Update extra location specific fields in table APartnerLocationTable
        /// </summary>
        /// <param name="ALocationDT">Table containing location records to be used to update APartnerLocation records.</param>
        /// <param name="APartnerLocationDT">Table to be updated with Location specific information.</param>
        /// <param name="AMakePLocationRecordUnchanged">Set to true to make the PLocation Record unchanged
        /// (by calling .AcceptChanges() on it).</param>
        public static void SyncPartnerEditTDSPartnerLocation(PLocationTable ALocationDT, PartnerEditTDSPPartnerLocationTable APartnerLocationDT,
            bool AMakePLocationRecordUnchanged = false)
        {
            foreach (PartnerEditTDSPPartnerLocationRow PartnerLocationRow in APartnerLocationDT.Rows)
            {
                SyncPartnerEditTDSPartnerLocation(ALocationDT, PartnerLocationRow, AMakePLocationRecordUnchanged);
            }
        }

        /// <summary>
        /// Update extra location specific fields in DataRow APartnerLocationDR
        /// </summary>
        /// <param name="ALocationDT">Table containing location records to be used to update APartnerLocation records.</param>
        /// <param name="APartnerLocationDR">Single DataRow to be updated with Location specific information.</param>
        /// <param name="AMakePLocationRecordUnchanged">Set to true to make the PLocation Record unchanged
        /// (by calling .AcceptChanges() on it).</param>
        public static void SyncPartnerEditTDSPartnerLocation(PLocationTable ALocationDT, PartnerEditTDSPPartnerLocationRow APartnerLocationDR,
            bool AMakePLocationRecordUnchanged = false)
        {
            DataRow Row;
            PLocationRow LocationRow;

            if (APartnerLocationDR.RowState != DataRowState.Deleted)
            {
                Row = ALocationDT.Rows.Find(new Object[] { APartnerLocationDR.SiteKey, APartnerLocationDR.LocationKey });

                if (Row != null)
                {
                    LocationRow = (PLocationRow)Row;

                    APartnerLocationDR.LocationLocality = LocationRow.Locality;
                    APartnerLocationDR.LocationStreetName = LocationRow.StreetName;
                    APartnerLocationDR.LocationAddress3 = LocationRow.Address3;
                    APartnerLocationDR.LocationCity = LocationRow.City;
                    APartnerLocationDR.LocationCounty = LocationRow.County;
                    APartnerLocationDR.LocationPostalCode = LocationRow.PostalCode;
                    APartnerLocationDR.LocationCountryCode = LocationRow.CountryCode;

                    APartnerLocationDR.LocationCreatedBy = LocationRow.CreatedBy;

                    if (!LocationRow.IsDateCreatedNull())
                    {
                        APartnerLocationDR.LocationDateCreated = (DateTime)LocationRow.DateCreated;
                    }

                    APartnerLocationDR.LocationModifiedBy = LocationRow.ModifiedBy;

                    if (!LocationRow.IsDateModifiedNull())
                    {
                        APartnerLocationDR.LocationDateModified = (DateTime)LocationRow.DateModified;
                    }

                    if (AMakePLocationRecordUnchanged)
                    {
                        Row.AcceptChanges();
                    }
                }
            }
        }
    }
}