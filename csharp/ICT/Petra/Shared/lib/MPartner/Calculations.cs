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
using System.Data;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Text;
using Ict.Common;
using Ict.Common.Data;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain calculations - specific for the Partner Module.
    /// </summary>
    public class Calculations
    {
        #region Resourcestrings

        /// <summary>
        /// message for when no information is available
        /// </summary>
        private static readonly string StrNoNameInfoAvailable = Catalog.GetString("  No name information available");

        #endregion
        /// <summary>
        /// column name for best address
        /// </summary>
        public const String PARTNERLOCATION_BESTADDR_COLUMN = "BestAddress";

        /// <summary>
        /// column name for the location icon
        /// </summary>
        public const String PARTNERLOCATION_ICON_COLUMN = "Icon";

        /// <summary>
        /// Specifies how to format the String that is returned by Method
        /// <see cref="M:Ict.Petra.Shared.MPartner.Calculations.DetermineLocationString(Ict.Petra.Shared.MPartner.Partner.Data.PLocationRow, Ict.Petra.Shared.MPartner.Calculations.TPartnerLocationFormatEnum)" />.
        /// </summary>
        public enum TPartnerLocationFormatEnum
        {
            /// <summary>Return Location Part Strings separated by comma</summary>
            plfCommaSeparated,

            /// <summary>Return Location Part Strings separated by CR+LF</summary>
            plfLineBreakSeparated,

            /// <summary>Return Location Part Strings separated by HTML br element</summary>
            plfHtmlLineBreak
        }

        /// <summary>
        /// check the validity of each location and update the icon for each location (current address, old address, future address)
        /// for the current date
        /// </summary>
        /// <param name="APartnerLocationsDS">the dataset with the locations</param>
        public static void DeterminePartnerLocationsDateStatus(DataSet APartnerLocationsDS)
        {
            DataTable ProcessDT;

            if ((APartnerLocationsDS is PartnerEditTDS)
                || (APartnerLocationsDS.Tables.Contains(TTypedDataTable.GetTableName(PPartnerLocationTable.TableId)) == true))
            {
                ProcessDT = APartnerLocationsDS.Tables[TTypedDataTable.GetTableName(PPartnerLocationTable.TableId)];
            }
            else
            {
                ProcessDT = APartnerLocationsDS.Tables["PartnerLocation"];
            }

            DeterminePartnerLocationsDateStatus(ProcessDT, DateTime.Today);
        }

        /// <summary>
        /// check the validity of each location and update the icon of each location (current address, old address, future address)
        /// </summary>
        /// <param name="APartnerLocationsDT">the datatable to check</param>
        /// <param name="ADateToCheck"></param>
        public static void DeterminePartnerLocationsDateStatus(DataTable APartnerLocationsDT, DateTime ADateToCheck)
        {
            System.DateTime pDateEffective;
            System.DateTime pDateGoodUntil;

            /*
             *  Add custom DataColumn if its not part of the DataTable yet
             */
            if (!APartnerLocationsDT.Columns.Contains(PARTNERLOCATION_ICON_COLUMN))
            {
                APartnerLocationsDT.Columns.Add(new System.Data.DataColumn(PARTNERLOCATION_ICON_COLUMN, typeof(Int32)));
            }

            /*
             * Loop over all DataRows and determine their 'Date Status'. The result is then
             * stored in the 'Icon' DataColumn.
             */
            foreach (DataRow pRow in APartnerLocationsDT.Rows)
            {
                if (pRow.RowState != DataRowState.Deleted)
                {
                    pDateEffective = TSaveConvert.ObjectToDate(pRow[PPartnerLocationTable.GetDateEffectiveDBName()]);
                    pDateGoodUntil = TSaveConvert.ObjectToDate(
                        pRow[PPartnerLocationTable.GetDateGoodUntilDBName()], TNullHandlingEnum.nhReturnHighestDate);

                    // Current Address: Icon = 1,
                    // Future Address:  Icon = 2,
                    // Expired Address: Icon = 3.
                    if ((pDateEffective <= ADateToCheck) && ((pDateGoodUntil >= ADateToCheck) || (pDateGoodUntil == new DateTime(9999, 12, 31))))
                    {
                        pRow[PartnerEditTDSPPartnerLocationTable.GetIconDBName()] = ((object)1);
                    }
                    else if (pDateEffective > ADateToCheck)
                    {
                        pRow[PartnerEditTDSPPartnerLocationTable.GetIconDBName()] = ((object)2);
                    }
                    else
                    {
                        pRow[PartnerEditTDSPPartnerLocationTable.GetIconDBName()] = ((object)3);
                    }
                }
            }
        }

        /// <summary>
        /// Determines which address is the 'Best Address' of a Partner, and marks it in the DataColumn 'BestAddress'.
        /// </summary>
        /// <remarks>There are convenient overloaded server-side Methods, Ict.Petra.Server.MPartner.ServerCalculations.DetermineBestAddress,
        /// which work by specifying the PartnerKey of a Partner in an Argument.</remarks>
        /// <param name="APartnerLocationsDS">Dataset containing the addresses of a Partner.</param>
        /// <returns>A <see cref="TLocationPK" /> which points to the 'Best Address'. If no 'Best Address' was found,
        /// SiteKey and LocationKey of this instance will be both -1.</returns>
        public static TLocationPK DetermineBestAddress(DataSet APartnerLocationsDS)
        {
            DataTable ProcessDT;

            if ((APartnerLocationsDS is PartnerEditTDS)
                || (APartnerLocationsDS.Tables.Contains(TTypedDataTable.GetTableName(PPartnerLocationTable.TableId)) == true))
            {
                ProcessDT = APartnerLocationsDS.Tables[TTypedDataTable.GetTableName(PPartnerLocationTable.TableId)];
            }
            else
            {
                ProcessDT = APartnerLocationsDS.Tables["PartnerLocation"];
            }

            return DetermineBestAddress(ProcessDT);
        }

        /// <summary>
        /// Determines which address is the 'Best Address' of a Partner, and marks it in the DataColumn 'BestAddress'.
        /// </summary>
        /// <remarks>There are convenient overloaded server-side Methods, Ict.Petra.Server.MPartner.ServerCalculations.DetermineBestAddress,
        /// which work by specifying the PartnerKey of a Partner in an Argument.</remarks>
        /// <param name="APartnerLocationsDT">DataTable containing the addresses of a Partner.</param>
        /// <returns>A <see cref="TLocationPK" /> which points to the 'Best Address'. If no 'Best Address' was found,
        /// SiteKey and LocationKey of this instance will be both -1.</returns>
        public static TLocationPK DetermineBestAddress(DataTable APartnerLocationsDT)
        {
            TLocationPK ReturnValue;

            DataRow[] OrderedRows;
            System.Int32 CurrentRow;
            System.Int32 BestRow;
            System.Int16 FirstRowAddrOrder;
            bool FirstRowMailingAddress;
            System.DateTime BestRowDate;
            System.DateTime TempDate;
            CurrentRow = 0;
            BestRow = 0;

            TLogging.LogAtLevel(8, "Calculations.DetermineBestAddress: processing " + APartnerLocationsDT.Rows.Count.ToString() + " rows...");

            if (APartnerLocationsDT == null)
            {
                throw new ArgumentException("Argument APartnerLocationsDT must not be null");
            }

            if (!APartnerLocationsDT.Columns.Contains(PARTNERLOCATION_BESTADDR_COLUMN))
            {
                DeterminePartnerLocationsDateStatus(APartnerLocationsDT, DateTime.Today);
            }

            /*
             *  Add custom DataColumn if its not part of the DataTable yet
             */
            if (!APartnerLocationsDT.Columns.Contains(PARTNERLOCATION_BESTADDR_COLUMN))
            {
                APartnerLocationsDT.Columns.Add(new System.Data.DataColumn(PARTNERLOCATION_BESTADDR_COLUMN, typeof(Boolean)));
            }

            /*
             * Order tables' rows: first all records with p_send_mail_l = true, these are ordered
             * ascending by Icon, then all records with p_send_mail_l = false, these are ordered
             * ascending by Icon.
             */
            OrderedRows = APartnerLocationsDT.Select(APartnerLocationsDT.DefaultView.RowFilter,
                PPartnerLocationTable.GetSendMailDBName() + " DESC, " + PartnerEditTDSPPartnerLocationTable.GetIconDBName() + " ASC",
                DataViewRowState.CurrentRows);

            if (OrderedRows.Length > 1)
            {
                FirstRowAddrOrder = Convert.ToInt16(OrderedRows[0][PartnerEditTDSPPartnerLocationTable.GetIconDBName()]);
                FirstRowMailingAddress = Convert.ToBoolean(OrderedRows[0][PPartnerLocationTable.GetSendMailDBName()]);

                // determine pBestRowDate
                if (FirstRowAddrOrder != 3)
                {
                    BestRowDate = TSaveConvert.ObjectToDate(OrderedRows[CurrentRow][PPartnerLocationTable.GetDateEffectiveDBName()]);
                }
                else
                {
                    BestRowDate = TSaveConvert.ObjectToDate(OrderedRows[CurrentRow][PPartnerLocationTable.GetDateGoodUntilDBName()]);
                }

                // iterate through the sorted rows
                for (CurrentRow = 0; CurrentRow <= OrderedRows.Length - 1; CurrentRow += 1)
                {
                    // reset any row that might have been marked as 'best' before
                    OrderedRows[CurrentRow][PartnerEditTDSPPartnerLocationTable.GetBestAddressDBName()] = ((object)0);

                    // determine pTempDate
                    if (FirstRowAddrOrder != 3)
                    {
                        TempDate = TSaveConvert.ObjectToDate(OrderedRows[CurrentRow][PPartnerLocationTable.GetDateEffectiveDBName()]);
                    }
                    else
                    {
                        TempDate = TSaveConvert.ObjectToDate(OrderedRows[CurrentRow][PPartnerLocationTable.GetDateGoodUntilDBName()]);
                    }

                    // still the same ADDR_ORDER than the ADDR_ORDER of the first row and
                    // still the same Mailing Address than the Mailing Address flag of the first row > proceed
                    if ((Convert.ToInt16(OrderedRows[CurrentRow][PartnerEditTDSPPartnerLocationTable.GetIconDBName()]) == FirstRowAddrOrder)
                        && (Convert.ToBoolean(OrderedRows[CurrentRow][PPartnerLocationTable.GetSendMailDBName()]) == FirstRowMailingAddress))
                    {
                        switch (FirstRowAddrOrder)
                        {
                            case 1:
                            case 3:

                                // find the Row with the highest p_date_effective_d (or p_date_good_until_d) date
                                if (TempDate > BestRowDate)
                                {
                                    BestRowDate = TempDate;
                                    BestRow = CurrentRow;
                                }

                                break;

                            case 2:

                                // find the Row with the lowest p_date_effective_d date
                                if (TempDate < BestRowDate)
                                {
                                    BestRowDate = TempDate;
                                    BestRow = CurrentRow;
                                }

                                break;
                        }
                    }
                }

                // mark the location that was determined to be the 'best'
                OrderedRows[BestRow][PartnerEditTDSPPartnerLocationTable.GetBestAddressDBName()] = ((object)1);
                ReturnValue =
                    new TLocationPK(Convert.ToInt64(OrderedRows[BestRow][PLocationTable.GetSiteKeyDBName()]),
                        Convert.ToInt32(OrderedRows[BestRow][PLocationTable.GetLocationKeyDBName()]));
            }
            else
            {
                if (OrderedRows.Length == 1)
                {
                    // mark the only location to be the 'best'
                    OrderedRows[0][PartnerEditTDSPPartnerLocationTable.GetBestAddressDBName()] = ((object)1);

                    ReturnValue = new TLocationPK(Convert.ToInt64(OrderedRows[0][PLocationTable.GetSiteKeyDBName()]),
                        Convert.ToInt32(OrderedRows[0][PLocationTable.GetLocationKeyDBName()]));
                }
                else
                {
                    ReturnValue = new TLocationPK();
                }
            }

            return ReturnValue;
        }

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
        /// Builds a formatted String out of the data that is contained in a Location.
        /// </summary>
        /// <param name="ALocationDR">DataRow containing the Location data.</param>
        /// <param name="APartnerLocationStringFormat">Specifies how to format the String that is returned.</param>
        /// <returns>Formatted String.</returns>
        public static String DetermineLocationString(PLocationRow ALocationDR,
            TPartnerLocationFormatEnum APartnerLocationStringFormat = TPartnerLocationFormatEnum.plfLineBreakSeparated)
        {
            return DetermineLocationString(ALocationDR.Building1,
                ALocationDR.Building2,
                ALocationDR.Locality,
                ALocationDR.StreetName,
                ALocationDR.Address3,
                ALocationDR.Suburb,
                ALocationDR.City,
                ALocationDR.County,
                ALocationDR.PostalCode,
                ALocationDR.CountryCode,
                APartnerLocationStringFormat);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ABuilding1"></param>
        /// <param name="ABuilding2"></param>
        /// <param name="ALocality"></param>
        /// <param name="AStreetName"></param>
        /// <param name="AAddress3"></param>
        /// <param name="ASuburb"></param>
        /// <param name="ACity"></param>
        /// <param name="ACounty"></param>
        /// <param name="APostalCode"></param>
        /// <param name="ACountryCode"></param>
        /// <returns></returns>
        public static String DetermineLocationString(String ABuilding1,
            String ABuilding2,
            String ALocality,
            String AStreetName,
            String AAddress3,
            String ASuburb,
            String ACity,
            String ACounty,
            String APostalCode,
            String ACountryCode)
        {
            return DetermineLocationString(ABuilding1,
                ABuilding2,
                ALocality,
                AStreetName,
                AAddress3,
                ASuburb,
                ACity,
                ACounty,
                APostalCode,
                ACountryCode,
                TPartnerLocationFormatEnum.plfLineBreakSeparated);
        }

        /// <summary>
        /// Builds a formatted String out of the data that is contained in a Location.
        /// </summary>
        /// <param name="ABuilding1">building name 1</param>
        /// <param name="ABuilding2">building name 2</param>
        /// <param name="ALocality">locality</param>
        /// <param name="AStreetName">street name</param>
        /// <param name="AAddress3">address 3</param>
        /// <param name="ASuburb">suburb</param>
        /// <param name="ACity">city</param>
        /// <param name="ACounty">county</param>
        /// <param name="APostalCode">postal code</param>
        /// <param name="ACountryCode">country code</param>
        /// <param name="PartnerLocationStringFormat">requested format</param>
        /// <returns>formatted string</returns>
        public static String DetermineLocationString(String ABuilding1,
            String ABuilding2,
            String ALocality,
            String AStreetName,
            String AAddress3,
            String ASuburb,
            String ACity,
            String ACounty,
            String APostalCode,
            String ACountryCode,
            TPartnerLocationFormatEnum PartnerLocationStringFormat)
        {
            String ReturnValue;
            String Separator;
            StringBuilder SBuilder;

            switch (PartnerLocationStringFormat)
            {
                case TPartnerLocationFormatEnum.plfCommaSeparated:
                    Separator = ", ";
                    break;

                case TPartnerLocationFormatEnum.plfLineBreakSeparated:
                    Separator = Environment.NewLine;
                    break;

                case TPartnerLocationFormatEnum.plfHtmlLineBreak:
                    Separator = "<br/>";
                    break;

                default:
                    Separator = Environment.NewLine;
                    break;
            }

            SBuilder = new StringBuilder(200);

            if (ABuilding1 != null)
            {
                if (ABuilding1 != "")
                {
                    SBuilder.Append(ABuilding1 + Separator);
                }
            }

            if (ABuilding2 != null)
            {
                if (ABuilding2 != "")
                {
                    SBuilder.Append(ABuilding2 + Separator);
                }
            }

            if (ALocality != null)
            {
                if (ALocality != "")
                {
                    SBuilder.Append(ALocality + Separator);
                }
            }

            if (AStreetName != null)
            {
                if (AStreetName != "")
                {
                    SBuilder.Append(AStreetName + Separator);
                }
            }

            if (AAddress3 != null)
            {
                if (AAddress3 != "")
                {
                    SBuilder.Append(AAddress3 + Separator);
                }
            }

            if (ASuburb != null)
            {
                if (ASuburb != "")
                {
                    SBuilder.Append(ASuburb + Separator);
                }
            }

            if (ACity != null)
            {
                if (ACity != "")
                {
                    SBuilder.Append(ACity + Separator);
                }
            }

            if (ACounty != null)
            {
                if (ACounty != "")
                {
                    SBuilder.Append(ACounty + Separator);
                }
            }

            if (APostalCode != null)
            {
                if (APostalCode != "")
                {
                    SBuilder.Append(APostalCode + Separator);
                }
            }

            if (ACountryCode != null)
            {
                if (ACountryCode != "")
                {
                    SBuilder.Append(ACountryCode + Separator);
                }
            }

            // Get the String that contains the concatenated subStrings
            ReturnValue = SBuilder.ToString();

            // Remove last Separator if the Result has them
            if (ReturnValue.Length > Separator.Length)
            {
                ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - Separator.Length);
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the current address from a location table
        /// </summary>
        /// <param name="ATable">table with locations</param>
        /// <returns>data view containing the current address</returns>
        public static DataView DetermineCurrentAddresses(PPartnerLocationTable ATable)
        {
            // dd/MM/yyyy did not work on Mono on Mac
            // see also http://www.csharp-examples.net/dataview-rowfilter/
            return new DataView(ATable, "((" + PPartnerLocationTable.GetDateEffectiveDBName() + " <= #" + DateTime.Now.Date.ToString(
                    "yyyy-MM-dd") + "# OR " + PPartnerLocationTable.GetDateEffectiveDBName() + " IS NULL) AND (" +
                PPartnerLocationTable.GetDateGoodUntilDBName() + " >= #" + DateTime.Now.Date.ToString(
                    "yyyy-MM-dd") + "# OR " + PPartnerLocationTable.GetDateGoodUntilDBName() + " IS NULL))", "", DataViewRowState.CurrentRows);
        }

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

        /// <summary>
        /// Formats a phone number in international format.
        /// </summary>
        /// <remarks>Example:  Phone number=01234 56789, Extension=77, Country=NL. Result=+31 (0)01234 56789-77.</remarks>
        /// <param name="APhoneNumber">Phone Number.</param>
        /// <param name="APhoneExtension">Phone Extension.</param>
        /// <param name="ACountryCode">Country Code of the Country in which the phone number is registered/to be reached.</param>
        /// <param name="ACacheRetriever">Delegate that returns the a DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MCommon Cache (that is, it needs to work with the <see cref="TCacheableCommonTablesEnum" /> Enum!</param>
        /// <returns></returns>
        public static string FormatIntlPhoneNumber(string APhoneNumber, string APhoneExtension, string ACountryCode,
            TGetCacheableDataTableFromCache ACacheRetriever)
        {
            string IntlTelephoneCode = CommonCodeHelper.GetCountryIntlTelephoneCode(
                ACacheRetriever, ACountryCode);

            if (APhoneExtension != String.Empty)
            {
                APhoneNumber += "-" + APhoneExtension;
            }

            if (IntlTelephoneCode != String.Empty)
            {
                if ((APhoneNumber.StartsWith("0")
                     && (APhoneNumber.Substring(1, 1) != "0")))
                {
                    APhoneNumber = "(0)" + APhoneNumber.Substring(1);
                }

                return "+" + IntlTelephoneCode + " " + APhoneNumber;
            }
            else
            {
                return APhoneNumber;
            }
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
    }
}