//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2015 by OM International
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
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Server.MPartner.Mailroom.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client
    ///
    /// </summary>
    public class TAddressDumplicateWebConnector
    {
        /// <summary>
        /// Finds potential duplicate addresses
        /// </summary>
        /// <param name="ADuplicateAddresses">Custom table containing duplicates</param>
        /// <param name="AExactMatchNumber">True if address numbers should have an exact match</param>
        [RequireModulePermission("PTNRUSER")]
        public static void FindAddressDuplicates(ref DataTable ADuplicateAddresses, bool AExactMatchNumber)
        {
            TDBTransaction Transaction = null;
            DataTable ReturnTable = ADuplicateAddresses.Clone();
            PLocationTable Locations = new PLocationTable();

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Checking for duplicate addresses"));

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    // get all locations from database,
                    // except for those without a Locality, Street Name and Address as these are too vague to make a match
                    string Query = "SELECT p_location.* FROM p_location" +
                                   " WHERE (p_location.p_locality_c is NOT NULL AND p_location.p_locality_c <> '')" +
                                   " OR (p_location.p_street_name_c is NOT NULL AND p_location.p_street_name_c <> '')" +
                                   " OR (p_location.p_address_3_c is NOT NULL AND p_location.p_address_3_c <> '')";

                    DBAccess.GDBAccessObj.SelectDT(Locations, Query, Transaction);

                    // create a list of tables grouped by country codes
                    List <DataTable>LocationDataTables = Locations.AsEnumerable()
                                                         .GroupBy(row => row[PLocationTable.GetCountryCodeDBName()])
                                                         .Select(g => g.CopyToDataTable())
                                                         .ToList();

                    DataTable BlankCountryLocations = Locations.Clone();

                    // create another table that contains all locations without a valid country code
                    for (int i = 0; i < LocationDataTables.Count; i++)
                    {
                        // this helps the time left feature to be more accurate from the start
                        LocationDataTables[i].DefaultView.Sort = PLocationTable.GetPostalCodeDBName() + " DESC";
                        LocationDataTables[i] = LocationDataTables[i].DefaultView.ToTable();

                        if (string.IsNullOrEmpty(LocationDataTables[i].Rows[0]["p_country_code_c"].ToString())
                            || (LocationDataTables[i].Rows[0]["p_country_code_c"].ToString() == "99"))
                        {
                            foreach (DataRow Row in LocationDataTables[i].Rows)
                            {
                                BlankCountryLocations.Rows.Add((object[])Row.ItemArray.Clone());
                            }
                        }
                    }

                    Int64 TotalCalculations = 0;
                    Int64 CompletedCalculations = 0;
                    decimal PercentageCompleted = 0;

                    // calculate number of calculations required for this check
                    for (int i = 0; i < LocationDataTables.Count; i++)
                    {
                        if (LocationDataTables[i].Rows.Count > 0)
                        {
                            TotalCalculations += ((Int64)LocationDataTables[i].Rows.Count) * ((Int64)LocationDataTables[i].Rows.Count - 1) / 2;

                            // if not table containing invalid country codes
                            if (!string.IsNullOrEmpty(LocationDataTables[i].Rows[0]["p_country_code_c"].ToString())
                                && (LocationDataTables[i].Rows[0]["p_country_code_c"].ToString() != "99"))
                            {
                                TotalCalculations += BlankCountryLocations.Rows.Count;
                            }
                        }
                    }

                    Int64 TimeLeft;
                    int MinutesLeft;
                    int SecondsLeft;
                    Stopwatch time = Stopwatch.StartNew();

                    // begin search for possible duplicates
                    foreach (DataTable LocationCountry in LocationDataTables)
                    {
                        if (LocationCountry.Rows.Count <= 0)
                        {
                            continue;
                        }

                        for (int i = 0; i < LocationCountry.Rows.Count && ReturnTable.Rows.Count < 500; i++)
                        {
                            string AAddress = null;
                            string[] AAddressArray = null;

                            for (int j = i + 1; j < LocationCountry.Rows.Count; j++)
                            {
                                // check if two rows are a possible duplicate
                                if (PossibleMatch(LocationCountry.Rows[i], ref AAddress, ref AAddressArray, LocationCountry.Rows[j],
                                        AExactMatchNumber))
                                {
                                    ReturnTable.Rows.Add(new object[] {
                                            LocationCountry.Rows[i][PLocationTable.GetSiteKeyDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetLocationKeyDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetLocalityDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetStreetNameDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetAddress3DBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetCityDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetCountyDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetPostalCodeDBName()],
                                            LocationCountry.Rows[i][PLocationTable.GetCountryCodeDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetSiteKeyDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetLocationKeyDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetLocalityDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetStreetNameDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetAddress3DBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetCityDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetCountyDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetPostalCodeDBName()],
                                            LocationCountry.Rows[j][PLocationTable.GetCountryCodeDBName()]
                                        });
                                }

                                CompletedCalculations++;
                            }

                            // if not table containing invalid country codes
                            if (!string.IsNullOrEmpty(LocationCountry.Rows[0]["p_country_code_c"].ToString())
                                && (LocationCountry.Rows[0]["p_country_code_c"].ToString() != "99"))
                            {
                                // compare with locations with invalid country codes
                                for (int j = 0; j < BlankCountryLocations.Rows.Count; j++)
                                {
                                    if (PossibleMatch(LocationCountry.Rows[i], ref AAddress, ref AAddressArray, BlankCountryLocations.Rows[j],
                                            AExactMatchNumber))
                                    {
                                        ReturnTable.Rows.Add(new object[] {
                                                LocationCountry.Rows[i][PLocationTable.GetSiteKeyDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetLocationKeyDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetLocalityDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetStreetNameDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetAddress3DBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetCityDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetCountyDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetPostalCodeDBName()],
                                                LocationCountry.Rows[i][PLocationTable.GetCountryCodeDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetSiteKeyDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetLocationKeyDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetLocalityDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetStreetNameDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetAddress3DBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetCityDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetCountyDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetPostalCodeDBName()],
                                                BlankCountryLocations.Rows[j][PLocationTable.GetCountryCodeDBName()]
                                            });
                                    }

                                    CompletedCalculations++;
                                }
                            }

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                            {
                                break;
                            }

                            // estimate the remaining time
                            PercentageCompleted = decimal.Divide(CompletedCalculations * 100, TotalCalculations);
                            TimeLeft = (Int64)(time.ElapsedMilliseconds * ((100 / PercentageCompleted) - 1));
                            MinutesLeft = (int)TimeLeft / 60000;

                            string OutputMessage = string.Format(Catalog.GetString("Completed: {0}%"), Math.Round(PercentageCompleted, 1));

                            // only show estimated time left if at least 0.1% complete
                            if (PercentageCompleted >= (decimal)0.1)
                            {
                                // only show seconds if less than 10 minutes remaining
                                if (MinutesLeft < 10)
                                {
                                    SecondsLeft = (int)(TimeLeft % 60000) / 1000;

                                    OutputMessage += string.Format(Catalog.GetPluralString(" (approx. {0} minute and {1} seconds remaining)",
                                            " (approx. {0} minutes and {1} seconds remaining)", MinutesLeft, true),
                                        MinutesLeft, SecondsLeft);
                                }
                                else
                                {
                                    OutputMessage += string.Format(Catalog.GetString(" (approx. {0} minutes remaining)"),
                                        MinutesLeft);
                                }
                            }

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                OutputMessage,
                                PercentageCompleted);
                        }
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            ADuplicateAddresses = ReturnTable.Copy();
        }

        private static bool PossibleMatch(DataRow AAddressARow,
            ref string AAddressA,
            ref string[] AAddressAArray,
            DataRow AAddressBRow,
            bool AExactMatchNumber)
        {
            bool ReturnValue = true;
            bool MatchingPostCodes = false;
            string AddressB = null;

            // if addresses have two different postcodes then discount immediately
            if (!string.IsNullOrEmpty(AAddressARow[PLocationTable.GetPostalCodeDBName()].ToString())
                && !string.IsNullOrEmpty(AAddressBRow[PLocationTable.GetPostalCodeDBName()].ToString()))
            {
                if (AAddressARow[PLocationTable.GetPostalCodeDBName()].ToString().ToUpper() !=
                    AAddressBRow[PLocationTable.GetPostalCodeDBName()].ToString().ToUpper())
                {
                    return false;
                }

                MatchingPostCodes = true;
            }

            // if this is the first time this address has got this far...
            if (string.IsNullOrEmpty(AAddressA))
            {
                AAddressA = string.Join("", AAddressARow[PLocationTable.GetLocalityDBName()].ToString(),
                    AAddressARow[PLocationTable.GetStreetNameDBName()].ToString(), AAddressARow[PLocationTable.GetAddress3DBName()].ToString());

                // remove punctuation characters and replace with a space; lower case
                AAddressA = Regex.Replace(AAddressA, @"[^\p{L}\p{N}]+", " ", RegexOptions.Compiled).ToLower();

                // make sure there is a space between all letters and numbers (i.e. caused by a typo)
                AAddressA = Regex.Replace(AAddressA, "(?<=[0-9])(?=[A-Za-z])|(?<=[A-Za-z])(?=[0-9])", " ");
                AAddressAArray = AAddressA.Split(' ');
            }

            AddressB = string.Join("", AAddressBRow[PLocationTable.GetLocalityDBName()].ToString(),
                AAddressBRow[PLocationTable.GetStreetNameDBName()].ToString(), AAddressBRow[PLocationTable.GetAddress3DBName()].ToString());

            // remove punctuation characters and replace with a space; lower case
            AddressB = Regex.Replace(AddressB, @"[^\p{L}\p{N}]+", " ", RegexOptions.Compiled).ToLower();

            // make sure there is a space between all letters and numbers (i.e. caused by a typo)
            AddressB = Regex.Replace(AddressB, "(?<=[0-9])(?=[A-Za-z])|(?<=[A-Za-z])(?=[0-9])", " ");

            if (string.IsNullOrWhiteSpace(AAddressA)) // if addressA is blank
            {
                // match if addressB is also blank or postcodes match
                if (string.IsNullOrWhiteSpace(AddressB) || MatchingPostCodes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            String[] AddressBArray = AddressB.Split(' ');

            bool PossibleMatch = false;
            bool NumbersMatch = false;

            // compare AddressA to AddressB
            foreach (string Item1 in AAddressAArray)
            {
                if (string.IsNullOrEmpty(Item1))
                {
                    continue;
                }

                if (IsDigitsOnly(Item1)) // if word is a number
                {
                    // if this word is a number but the 2nd address contains no numbers
                    if (!AddressB.Any(char.IsDigit))
                    {
                        if (MatchingPostCodes) // if matching postcodes continue
                        {
                            PossibleMatch = true;
                            NumbersMatch = true;
                        }
                    }
                    else
                    {
                        foreach (string Item2 in AddressBArray)
                        {
                            // Only compare two items if second item is also a number.
                            if (IsDigitsOnly(Item2)
                                && ((AExactMatchNumber && (Item1 == Item2))
                                    || (!AExactMatchNumber && (ComputeDamerauLevenshteinDistance(Item1, Item2) <= 1))))
                            {
                                PossibleMatch = true;
                                NumbersMatch = true;
                                break;
                            }
                        }
                    }
                }
                else if (AddressBArray.Contains(Item1))
                {
                    // if 2nd address contains this word then success
                    PossibleMatch = true;
                }
                else
                {
                    foreach (string Item2 in AddressBArray)
                    {
                        // calculate max allowed distance to get from first item to the second
                        decimal Distance = 0;

                        if (AAddressAArray.Count() > 1)
                        {
                            Distance = Math.Min(Math.Floor((decimal)Math.Min(Item1.Length, Item2.Length) / 2), 2);
                        }

                        // if two words have a similar length then use the Levenshtein Distance algorithm to determine how alike they are.
                        // If they have a distance less than or equal to 'Distance' then success.
                        if (!IsDigitsOnly(Item2)
                            && (Math.Abs(Item1.Length - Item2.Length) <= Math.Min(Math.Min(Item1.Length, Item2.Length), 2))
                            && (ComputeDamerauLevenshteinDistance(Item1, Item2) <= Distance))
                        {
                            PossibleMatch = true;
                            break;
                        }
                    }
                }

                ReturnValue = PossibleMatch;

                if (!ReturnValue)
                {
                    break;
                }

                PossibleMatch = false;
            }

            // If a match has not been made but the numbers in the two addresses do agree then try again but the other way around.
            // I.e. compare AddressB to AddressA
            // This means that abbreviations and names that can be spelt as one or two words are still matched
            // and optional address elaments are ignored if only in one address.
            if (!ReturnValue && NumbersMatch)
            {
                foreach (string Item1 in AddressBArray)
                {
                    // do not need to compare numbers again
                    if (string.IsNullOrEmpty(Item1) || IsDigitsOnly(Item1))
                    {
                        continue;
                    }

                    if (AAddressAArray.Contains(Item1))
                    {
                        // if 2nd address contains this word then success
                        PossibleMatch = true;
                    }
                    else
                    {
                        foreach (string Item2 in AAddressAArray)
                        {
                            // calculate max allowed distance to get from first item to the second
                            decimal Distance = 0;

                            if (AAddressAArray.Count() > 1)
                            {
                                Distance = Math.Min(Math.Floor((decimal)Math.Min(Item1.Length, Item2.Length) / 2), 2);
                            }

                            // if two words have a similar length then use the Levenshtein Distance algorithm to determine how alike they are.
                            // If they have a distance less than or equal to 'Distance' then success.
                            if (!IsDigitsOnly(Item2)
                                && (Math.Abs(Item1.Length - Item2.Length) <= Math.Min(Math.Min(Item1.Length, Item2.Length), 2))
                                && (ComputeDamerauLevenshteinDistance(Item1, Item2) <= Distance))
                            {
                                PossibleMatch = true;
                                break;
                            }
                        }
                    }

                    ReturnValue = PossibleMatch;

                    if (!ReturnValue)
                    {
                        break;
                    }

                    PossibleMatch = false;
                }
            }

            return ReturnValue;
        }

        // checks if a string is a number
        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if ((c < '0') || (c > '9'))
                {
                    return false;
                }
            }

            return true;
        }

        // measures the difference between two strings using the Damerau–Levenshtein Distance algorethm
        private static int ComputeDamerauLevenshteinDistance(string word1, string word2)
        {
            int word1Length = word1.Length;
            int word2Length = word2.Length;

            int[, ] matrix = new int[word1Length + 1, word2Length + 1];

            for (int i = 0; i <= word1Length; matrix[i, 0] = i++)
            {
                ;
            }

            for (int i = 0; i <= word2Length; matrix[0, i] = i++)
            {
                ;
            }

            //Let's fill up the matrix
            for (int i = 1; i <= word1Length; i++)
            {
                for (int a = 1; a <= word2Length; a++)
                {
                    int cost = (word2[a - 1] == word1[i - 1]) ? 0 : 1;

                    matrix[i, a] = Math.Min(
                        Math.Min(
                            matrix[i - 1, a] + 1, //delete
                            matrix[i, a - 1] + 1), //insert
                        matrix[i - 1, a - 1] + cost     //substitute
                        );

                    //Transposition using Damerau–Levenshtein distance
                    if ((i > 1) && (a > 1) && (word1[i - 1] == word2[a - 2]) && (word1[i - 2] == word2[a - 1]))
                    {
                        matrix[i, a] = Math.Min(
                            matrix[i, a],
                            matrix[i - 2, a - 2] + cost // transposition
                            );
                    }
                }
            }

            return matrix[word1Length, word2Length];
        }
    }
}