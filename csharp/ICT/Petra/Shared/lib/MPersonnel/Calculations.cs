//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Shared.MPersonnel
{
    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain calculations - specific for the Personnel Module.
    /// </summary>
    public class Calculations
    {
        /// <summary>Passport expired</summary>
        public static String PASSPORT_EXPIRED = " (" + Catalog.GetString("exp.") + ")";

        /// <summary>Main passport expired</summary>
        public static String PASSPORTMAIN_EXPIRED = " (" + Catalog.GetString("exp.") + " " + Catalog.GetString("MAIN") + "!)";

        /// <summary>
        /// Determines a PERSON's Nationalities (deduced from its passports).
        /// </summary>
        /// <remarks>
        /// <para>Algorithm:</para>
        /// <list type="number">
        ///   <item>Check pm_main_passport_l flag</item>
        ///     <list type="number">
        ///       <item>if null, don't do anything special (Note: Partner Import does not write to this field, it stays null if it isn't found in the import file [which will be the case for Petra 2.x])</item>
        ///       <item>if false, do likewise</item>
        ///       <item>if true, list as FIRST Country</item>
        ///     </list>
        ///   </list>
        /// <item>Order by pm_date_of_issue_d</item>
        ///   <list type="number">
        ///     <item>order Countries by pm_date_of_issue_d DESC, except for the FIRST Country</item>
        ///     <list type="number">
        ///       <item>in case there are several FIRST Countries (which in theory should not happen), they are ordered by pm_date_of_issue_d DESC in themselves at the beginning of the list of Countries.</item>
        ///     </list>
        ///   </list>
        /// <item>Check for passport expiration</item>
        ///   <list type="number">
        ///     <item>If a passport's expiry date is set and it is in the past</item>
        ///     <list type="number">
        ///       <item>show the Country + " (exp.)".</item>
        ///     </list>
        ///   </list>
        /// <item>Elimination of duplicate listings</item>
        ///   <list type="number">
        ///     <item>If a country of issue comes up twice</item>
        ///     <list type="number">
        ///       <item>eliminate the duplicate, except if the Country is listed once by its name and one with the exp. postfix.</item>
        ///     </list>
        ///   </list>
        /// <para>Additionally, a warning text " MAIN!)" to the postfix " (exp." is added if the expired passport is the
        /// 'Main Passport' (pm_main_passport_l=true) of the PERSON.</para>
        /// </remarks>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheableCommonTablesEnum" /> Enum!</param>
        /// <param name="APassportDetailsDT"></param>
        /// <returns></returns>
        public static string DeterminePersonsNationalities(TGetCacheableDataTableFromCache ACacheRetriever, PmPassportDetailsTable APassportDetailsDT)
        {
            HashSet <string>Nationalities = new HashSet <string>();
            string NationalitiesStr = String.Empty;
            PmPassportDetailsRow PassportDR;

            DataView OrderedPassportsDV = new DataView(
                APassportDetailsDT, null, PmPassportDetailsTable.GetDateOfIssueDBName() + " DESC",
                DataViewRowState.CurrentRows);

            // If a Passport's MainPassport flag is set, record it as the first Nationality
            foreach (DataRowView OrderedPassport in OrderedPassportsDV)
            {
                PassportDR = (PmPassportDetailsRow)OrderedPassport.Row;

                if (!PassportDR.IsMainPassportNull()
                    && PassportDR.MainPassport)
                {
                    // Add the Nationality (Note: Duplicates are taken care of automatically as this is a HashSet!)
                    Nationalities.Add(DeterminePassportNationality(ACacheRetriever, PassportDR));

                    // Note: We could leave the loop here if we assume that there is ever only going to be ONE
                    // 'first Nationality', but that assumption could be wrong and we want to make sure to list
                    // all 'first Nationalities', ordered by pm_date_of_issue_d DESC in themselves.
                }
            }

            // Add the rest of the Nationalities (Note: Duplicates are taken care of automatically as this is a HashSet!)
            foreach (DataRowView OrderedPassport in OrderedPassportsDV)
            {
                Nationalities.Add(DeterminePassportNationality(ACacheRetriever, (PmPassportDetailsRow)OrderedPassport.Row));
            }

            // Put all Nationalities in a string
            foreach (string Nationality in Nationalities)
            {
                NationalitiesStr += Nationality + ", ";
            }

            if (NationalitiesStr.Length > 0)
            {
                return NationalitiesStr.Substring(0, NationalitiesStr.Length - 2);  // remove last comma
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Returns the 'nationality' of a passport and marks expired passports up.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheableCommonTablesEnum" /> Enum!</param>
        /// <param name="APassportDR">DataRow containing information about a passport.</param>
        /// <returns>Country name that goes with the passport's Nationality Code, except for the case
        /// where no country name is found for the Nationality Code, then the Nationality Code is returned.
        /// If a passport is expired, the string " (exp.)" (potentially translated) is added as a postfix.</returns>
        public static string DeterminePassportNationality(TGetCacheableDataTableFromCache ACacheRetriever, PmPassportDetailsRow APassportDR)
        {
            string ReturnValue;

            ReturnValue = CommonCodeHelper.GetCountryName(
                @ACacheRetriever, APassportDR.PassportNationalityCode);

            // Fallback: If no country name exists in the Chacheable DataTable, use the Nationality Code.
            if (ReturnValue == String.Empty)
            {
                ReturnValue = APassportDR.PassportNationalityCode;
            }

            if ((APassportDR.DateOfExpiration.HasValue)
                && (APassportDR.DateOfExpiration.Value.Date < DateTime.Now.Date))
            {
                if ((!APassportDR.IsMainPassportNull())
                    && (APassportDR.MainPassport))
                {
                    ReturnValue += PASSPORTMAIN_EXPIRED;
                }
                else
                {
                    ReturnValue += PASSPORT_EXPIRED;
                }
            }

            return ReturnValue;
        }
    }
}