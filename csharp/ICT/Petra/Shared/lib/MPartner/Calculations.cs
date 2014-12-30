//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Text;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain calculations - specific for the Partner Module.
    /// </summary>
    /// <remarks>There are two further parts of this Partial Class, Calculations.Address.cs and
    /// Calculations.ContactDetails.cs that hold only Methods that are to do with the
    /// 'Addresses' and 'Contact Detail'-related implementations!</remarks>
    public partial class Calculations
    {
        /// <summary>
        /// format the shortname for a partner in a standardized way
        /// </summary>
        /// <param name="AName">surname of partner</param>
        /// <param name="ATitle">title</param>
        /// <param name="AFirstName">first name</param>
        /// <param name="AMiddleName">middle name</param>
        /// <returns>formatted shortname</returns>
        public static String DeterminePartnerShortName(String AName, String ATitle, String AFirstName, String AMiddleName)
        {
            String ShortName = "";

            try
            {
                if (AName.Trim().Length > 0)
                {
                    ShortName = AName.Trim();
                }

                if (AFirstName.Trim().Length > 0)
                {
                    ShortName = ShortName + ", " + AFirstName.Trim();
                }

                if (AMiddleName.Trim().Length > 0)
                {
                    ShortName = ShortName + ' ' + AMiddleName.Trim().Substring(0, 1);
                }

                if (ATitle.Trim().Length > 0)
                {
                    ShortName = ShortName + ", " + ATitle.Trim();
                }

                if (ShortName.Length == 0)
                {
                    ShortName = StrNoNameInfoAvailable;
                }
                else
                {
                    if (ShortName.Length > PPartnerTable.GetPartnerShortNameLength())
                    {
                        ShortName = ShortName.Substring(0, PPartnerTable.GetPartnerShortNameLength());
                    }
                }
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception occured in DeterminePartnerShortName (" + AName + "): " + Exp.ToString());
            }
            return ShortName;
        }

        /// <summary>
        /// overload for DeterminePartnerShortName, no middle name
        /// </summary>
        /// <param name="AName">surname</param>
        /// <param name="ATitle">title</param>
        /// <param name="AFirstName">firstname</param>
        /// <returns></returns>
        public static String DeterminePartnerShortName(String AName, String ATitle, String AFirstName)
        {
            return DeterminePartnerShortName(AName, ATitle, AFirstName, "");
        }

        /// <summary>
        /// overload for DeterminePartnerShortName, no middle name and no first name
        /// </summary>
        /// <param name="AName">surname</param>
        /// <param name="ATitle">title</param>
        /// <returns></returns>
        public static String DeterminePartnerShortName(String AName, String ATitle)
        {
            return DeterminePartnerShortName(AName, ATitle, "", "");
        }

        /// <summary>
        /// overload for DeterminePartnerShortName, no title, firstname and middle name
        /// </summary>
        /// <param name="AName">surname</param>
        /// <returns></returns>
        public static String DeterminePartnerShortName(String AName)
        {
            return DeterminePartnerShortName(AName, "", "", "");
        }

        /// <summary>
        /// Count the subscriptions
        /// </summary>
        /// <param name="ATable">table with contacts</param>
        /// <param name="ATotalContacts">returns the total number of contacts</param>
        public static void CalculateTabCountsContacts(PContactLogTable ATable, out Int32 ATotalContacts)
        {
            // Inspect only CurrentRows (this excludes Deleted DataRows)
            ATotalContacts = new DataView(ATable, "", "", DataViewRowState.CurrentRows).Count;
        }

        /// <summary>
        /// convert shortname from Lastname, firstname, title to another shortname format
        /// TODO: use partner key to get to the full name, resolve issues with couples that have different family names etc
        /// </summary>
        public static string FormatShortName(string AShortname, eShortNameFormat AFormat)
        {
            if (AShortname.Length == 0)
            {
                return "";
            }

            StringCollection names = StringHelper.StrSplit(AShortname, ",");

            string resultValue = "";

            if (AFormat == eShortNameFormat.eShortname)
            {
                return AShortname;
            }
            else if (AFormat == eShortNameFormat.eReverseShortname)
            {
                foreach (string name in names)
                {
                    if (resultValue.Length > 0)
                    {
                        resultValue = " " + resultValue;
                    }

                    resultValue = name + resultValue;
                }

                return resultValue;
            }
            else if (AFormat == eShortNameFormat.eOnlyTitle)
            {
                // organisations&churches have no title, therefore we need to check if there are more than 2 names
                if (names.Count > 2)
                {
                    return names[names.Count - 1];
                }

                // eg. Mustermann, Family
                if (names.Count > 1)
                {
                    return names[1];
                }
            }
            else if (AFormat == eShortNameFormat.eOnlySurname)
            {
                return names[0];
            }
            else if (AFormat == eShortNameFormat.eOnlyFirstname)
            {
                if (names.Count > 1)
                {
                    return names[1];
                }
            }
            else if (AFormat == eShortNameFormat.eReverseWithoutTitle)
            {
                if (names.Count > 1)
                {
                    // remove the title
                    names.RemoveAt(names.Count - 1);
                }

                foreach (string name in names)
                {
                    if (resultValue.Length > 0)
                    {
                        resultValue = " " + resultValue;
                    }

                    resultValue = name + resultValue;
                }

                return resultValue;
            }
            else if (AFormat == eShortNameFormat.eReverseLastnameInitialsOnly)
            {
                if (names.Count > 1)
                {
                    // remove the title
                    names.RemoveAt(names.Count - 1);
                }

                if (names.Count > 1)
                {
                    return names[1] + " " + names[0].Substring(0, 1) + ".";
                }

                return names[0].Substring(0, 1) + ".";
            }

            return "";
        }

        /// format a formal greeting for the given partner short name. this formal greeting can be used in a letter
        public static string FormalGreeting(string APartnerShortName)
        {
            // TODO: use formal greetings p_formality from database, etc
            string title = Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlyTitle);

            if ((StringHelper.ContainsI(title, Catalog.GetString("Mr"))
                 && StringHelper.ContainsI(title, Catalog.GetString("Mrs")))
                || StringHelper.ContainsI(title, Catalog.GetString("Family")))
            {
                return String.Format(Catalog.GetString("Dear {0} {1}{#PLURAL}").Replace("{#PLURAL}", ""),
                    title,
                    Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname));
            }
            else if (StringHelper.ContainsI(title, Catalog.GetString("Mr")))
            {
                return String.Format(Catalog.GetString("Dear {0} {1}{#MALE}").Replace("{#MALE}", ""),
                    title,
                    Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname));
            }
            else if (StringHelper.ContainsI(title, Catalog.GetString("Mrs"))
                     || StringHelper.ContainsI(title, Catalog.GetString("Ms"))
                     || StringHelper.ContainsI(title, Catalog.GetString("Miss")))
            {
                return String.Format(Catalog.GetString("Dear {0} {1}{#FEMALE}").Replace("{#FEMALE}", ""),
                    title,
                    Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname));
            }
            else if (title.Length == 0)
            {
                // for organisations
                return Catalog.GetString("Dear Sir or Madam");
            }
            else
            {
                // unrecognised title
                return String.Format(Catalog.GetString("Dear {0} {1}{#NOGENDER}").Replace("{#NOGENDER}", ""),
                    title,
                    Calculations.FormatShortName(APartnerShortName, eShortNameFormat.eOnlySurname));
            }
        }

        /// <summary>
        /// Calculates the age in years at the current date.
        /// </summary>
        /// <param name="ABirthday">The birthday from which to calculate the current age</param>
        /// <returns>The age in years</returns>
        public static int CalculateAge(DateTime ABirthday)
        {
            return CalculateAge(ABirthday, DateTime.Now);
        }

        /// <summary>
        /// Calculates the age in years at a given date.
        /// </summary>
        /// <param name="ABirthday">The birthday from which to calculate the age</param>
        /// <param name="ACalculationDate">The date against which the birthday should be calculated</param>
        /// <returns>The age in years</returns>
        public static int CalculateAge(DateTime ABirthday, DateTime ACalculationDate)
        {
            int years = ACalculationDate.Year - ABirthday.Year;

            // subtract another year if we're before the birthday in the current year
            if ((ACalculationDate.Month < ABirthday.Month)
                || ((ACalculationDate.Month == ABirthday.Month) && (ACalculationDate.Day < ABirthday.Day)))
            {
                years--;
            }

            return years;
        }

        #region CalculateTabCounts

        /// <summary>
        /// count the available current addresses and the total number of addresses
        /// </summary>
        /// <param name="ATable">table with locations</param>
        /// <param name="ATotalAddresses">returns the total number of address</param>
        /// <param name="ACurrentAddresses">returns the number of current addresses</param>
        public static void CalculateTabCountsAddresses(PPartnerLocationTable ATable, out Int32 ATotalAddresses, out Int32 ACurrentAddresses)
        {
            DataView TmpDV;

            // Inspect only CurrentRows (this excludes Deleted DataRows)
            TmpDV = new DataView(ATable, "", "", DataViewRowState.CurrentRows);
            ATotalAddresses = TmpDV.Count;

            if ((ATotalAddresses == 1) && (((PPartnerLocationRow)TmpDV[0].Row).LocationKey == 0))
            {
                // In case the only Address is linked to Location 0: we don't have a
                // Current Address, because this signalises that there is no valid address.
                // MessageBox.Show('The last Address is the ''No Address on file'' Address!');
                ACurrentAddresses = 0;
            }
            else
            {
                // MessageBox.Show('Query: ' + '((' + PPartnerLocationTable.GetDateEffectiveDBName + ' <= #'
                // + DateTime.Now.Date.ToString('MM/dd/yyyy') + '# OR ' +
                // PPartnerLocationTable.GetDateEffectiveDBName + ' IS NULL) AND (' +
                // PPartnerLocationTable.GetDateGoodUntilDBName + ' >= #'
                // + DateTime.Now.Date.ToString('MM/dd/yyyy') + '# OR ' +
                // PPartnerLocationTable.GetDateGoodUntilDBName + ' IS NULL))');
                ACurrentAddresses = DetermineCurrentAddresses(ATable).Count;
            }

            // MessageBox.Show('ACurrentAddresses: ' + ACurrentAddresses.ToString);
        }

        /// <summary>
        /// Count the Partner Contact Details.
        /// </summary>
        /// <param name="ATable">Table with Partner Contact Details. This will be the PPartnerAttribute Table.</param>
        /// <param name="ATotalPartnerContactDetails">returns the total number of Partner Contact Details.</param>
        /// <param name="AActivePartnerContactDetails">returns the number of current Partner Contact Details.</param>
        public static void CalculateTabCountsPartnerContactDetails(PartnerEditTDSPPartnerAttributeTable ATable,
            out Int32 ATotalPartnerContactDetails, out Int32 AActivePartnerContactDetails)
        {
            // Inspect only CurrentRows (this excludes Deleted DataRows)
            ATotalPartnerContactDetails = new DataView(ATable,
                "PartnerContactDetail = true", "", DataViewRowState.CurrentRows).Count;

            // Inspect only CurrentRows (this excludes Deleted DataRows)
            AActivePartnerContactDetails = new DataView(ATable,
                "PartnerContactDetail = true AND " +
                PPartnerAttributeTable.GetCurrentDBName() + " = true", "",
                DataViewRowState.CurrentRows).Count;
        }

        /// <summary>
        /// Count the subscriptions
        /// </summary>
        /// <param name="ATable">table with subscriptions</param>
        /// <param name="ATotalSubscriptions">returns the total number of subscriptions</param>
        /// <param name="AActiveSubscriptions">returns the number of active subscriptions</param>
        public static void CalculateTabCountsSubscriptions(PSubscriptionTable ATable, out Int32 ATotalSubscriptions, out Int32 AActiveSubscriptions)
        {
            // Inspect only CurrentRows (this excludes Deleted DataRows)
            ATotalSubscriptions = new DataView(ATable, "", "", DataViewRowState.CurrentRows).Count;

            // Inspect only CurrentRows (this excludes Deleted DataRows)
            AActiveSubscriptions = new DataView(ATable,
                PSubscriptionTable.GetSubscriptionStatusDBName() + " <> '" + MPartnerConstants.SUBSCRIPTIONS_STATUS_CANCELLED + "' AND " +
                PSubscriptionTable.GetSubscriptionStatusDBName() + " <> '" + MPartnerConstants.SUBSCRIPTIONS_STATUS_EXPIRED + "'", "",
                DataViewRowState.CurrentRows).Count;
        }

        /// <summary>
        /// Count the relationships
        /// </summary>
        /// <param name="ATable">table with subscriptions</param>
        /// <param name="ATotalRelationships">returns the total number of relationships</param>
        public static void CalculateTabCountsPartnerRelationships(PPartnerRelationshipTable ATable, out Int32 ATotalRelationships)
        {
            // Inspect only CurrentRows (this excludes Deleted DataRows)
            ATotalRelationships = new DataView(ATable, "", "", DataViewRowState.CurrentRows).Count;
        }

        #endregion
    }
}