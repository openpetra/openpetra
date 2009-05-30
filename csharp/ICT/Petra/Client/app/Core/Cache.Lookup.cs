/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;

using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Provides Methods that perform a lookup of a Code in a Cacheable DataTable
    /// and return the Description of that code.
    /// </summary>
    public static class Cache_Lookup
    {
        /// <summary>
        /// Lookups available for the Partner Module.
        /// </summary>
        public static class TMPartner
        {
            static private DataTable FCountryListDataCacheDT;
            static private DataTable FLanguageListDataCacheDT;

            /// <summary>
            /// Refreshes the internal cached DataTable from the DataCache.
            /// </summary>
            /// <remarks>Called by TDataCache if the DataTable was
            /// refreshed on the Serverside.</remarks>
            /// <param name="ACacheableTable"></param>
            public static void RefreshCacheablePartnerTable(TCacheablePartnerTablesEnum ACacheableTable)
            {
                switch (ACacheableTable)
                {
                    case TCacheablePartnerTablesEnum.CountryList:
                        FCountryListDataCacheDT = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.CountryList);

                        break;

                    case TCacheablePartnerTablesEnum.LanguageCodeList:
                        FLanguageListDataCacheDT = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LanguageCodeList);

                        break;
                }
            }

            /// <summary>
            /// Determines full Country Name from Country Code.
            /// </summary>
            /// <param name="ACountryCode">Country Code to lookup</param>
            /// <returns>Full Country Name for Country Code; if Country Code isn't found in
            /// the lookup table, the Country Code is returned.</returns>
            public static string DetermineCountryNameFromCode(string ACountryCode)
            {
                PCountryRow FoundDR;
                string ReturnValue;

                if (FCountryListDataCacheDT == null)
                {
                    RefreshCacheablePartnerTable(TCacheablePartnerTablesEnum.CountryList);
                }

                FoundDR = (PCountryRow)FCountryListDataCacheDT.Rows.Find(ACountryCode);

                if (FoundDR != null)
                {
                    // Found in Cache
                    ReturnValue = FoundDR.CountryName;
                }
                else
                {
                    // Value not found in Cache -> just use submitted Code

                    // MessageBox.Show("Value not found in Cache -> using submitted Code!");
                    ReturnValue = ACountryCode;
                }

                return ReturnValue;
            }

            /// <summary>
            /// Determines full Language Name from Language Code.
            /// </summary>
            /// <param name="ALanguageCode">Language Code to lookup</param>
            /// <returns>Full Language Name for Country Code; if Language Code isn't found in
            /// the lookup table, the Language Code is returned.</returns>
            public static string DetermineLanguageNameFromCode(string ALanguageCode)
            {
                PLanguageRow FoundDR;
                string ReturnValue;

                if (FLanguageListDataCacheDT == null)
                {
                    RefreshCacheablePartnerTable(TCacheablePartnerTablesEnum.LanguageCodeList);
                }

                FoundDR = (PLanguageRow)FLanguageListDataCacheDT.Rows.Find(ALanguageCode);

                if (FoundDR != null)
                {
                    // Found in Cache
                    ReturnValue = FoundDR.LanguageDescription;
                }
                else
                {
                    // Value not found in Cache -> just use submitted Code

                    // MessageBox.Show("Value not found in Cache -> using submitted Code!");
                    ReturnValue = ALanguageCode;
                }

                return ReturnValue;
            }
        }
    }
}