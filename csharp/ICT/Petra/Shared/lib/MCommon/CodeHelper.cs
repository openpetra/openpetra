//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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

using Ict.Common.Data;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// Gets descriptions for codes in the Common Module.
    /// </summary>
    public class CommonCodeHelper
    {
        /// <summary>
        /// Returns the name of a Country.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MCommon Cache (that is, it needs to work with the <see cref="TCacheableCommonTablesEnum" /> Enum!</param>
        /// <param name="ACountryCode">Country Code.</param>
        /// <returns>The description of a Country Code, or empty string if the Country Code could not be identified.</returns>
        public static string GetCountryName(TGetCacheableDataTableFromCache ACacheRetriever, string ACountryCode)
        {
            DataTable CachedDT;
            DataRow FoundDR;
            string ReturnValue = "";
            Type tmp;

            CachedDT = ACacheRetriever(Enum.GetName(typeof(TCacheableCommonTablesEnum), TCacheableCommonTablesEnum.CountryList), out tmp);
            FoundDR = CachedDT.Rows.Find(new object[] { ACountryCode });

            if (FoundDR != null)
            {
                ReturnValue = FoundDR[PCountryTable.ColumnCountryNameId].ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the International Telephone Code of a Country.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MCommon Cache (that is, it needs to work with the <see cref="TCacheableCommonTablesEnum" /> Enum!</param>
        /// <param name="AIntlTelephoneCode">International Telephone Code.</param>
        /// <returns>The description of a Country Code, or empty string if the Country Code could not be identified.</returns>
        public static string GetCountryIntlTelephoneCode(TGetCacheableDataTableFromCache ACacheRetriever, string AIntlTelephoneCode)
        {
            DataTable CachedDT;
            DataRow FoundDR;
            string ReturnValue = "";
            Type tmp;

            CachedDT = ACacheRetriever(Enum.GetName(typeof(TCacheableCommonTablesEnum), TCacheableCommonTablesEnum.CountryList), out tmp);
            FoundDR = CachedDT.Rows.Find(new object[] { AIntlTelephoneCode });

            if (FoundDR != null)
            {
                ReturnValue = FoundDR[PCountryTable.ColumnInternatTelephoneCodeId].ToString();
            }

            return ReturnValue;
        }
    }
}