//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MCommon.queries
{
    /// <summary>
    /// base class for extract queries on server side
    /// </summary>
    public abstract class ExtractQueryBase
    {
        /// <summary>
        /// Extracts who need special treatment need to set this to true. It is needed if there is more
        /// than one query involved in running the extract.
        /// </summary>
        protected bool FSpecialTreatment = false;

        /// <summary>
        /// calculate an extract from a report: all partners of a given type (or selection of multiple types)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        protected bool CalculateExtractInternal(TParameterList AParameters, string ASqlStmt, TResultList AResults)
        {
            int ExtractId;

            return CalculateExtractInternal(AParameters, ASqlStmt, AResults, out ExtractId);
        }

        /// <summary>
        /// calculate an extract from a report: all partners of a given type (or selection of multiple types)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="AResults"></param>
        /// <param name="AExtractId"></param>
        /// <returns></returns>
        protected bool CalculateExtractInternal(TParameterList AParameters, string ASqlStmt, TResultList AResults, out int AExtractId)
        {
            Boolean ReturnValue = false;
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            bool AddressFilterAdded;

            int ExtractId = -1;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOK,
                delegate
                {
                    // get the partner keys from the database
                    if (FSpecialTreatment)
                    {
                        ReturnValue = RunSpecialTreatment(AParameters, Transaction, out ExtractId);
                    }
                    else
                    {
                        // call to derived class to retrieve parameters specific for extract
                        RetrieveParameters(AParameters, ref ASqlStmt, ref SqlParameterList);

                        // add address filter information to sql statement and parameter list
                        AddressFilterAdded = AddAddressFilter(AParameters, ref ASqlStmt, ref SqlParameterList);

                        // now run the database query
                        TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                        DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(ASqlStmt, "partners", Transaction,
                            SqlParameterList.ToArray());

                        // filter data by postcode (if applicable)
                        PostcodeFilter(ref partnerkeys, ref AddressFilterAdded, AParameters, Transaction);

                        // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                        // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
                        if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                        {
                            ReturnValue = false;
                        }
                        else
                        {
                            TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

                            // create an extract with the given name in the parameters
                            ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                                AParameters.Get("param_extract_name").ToString(),
                                AParameters.Get("param_extract_description").ToString(),
                                out ExtractId,
                                partnerkeys,
                                0,
                                AddressFilterAdded,
                                true);
                        }
                    }

                    if (ReturnValue)
                    {
                        SubmissionOK = true;
                    }
                });

            AExtractId = ExtractId;

            return ReturnValue;
        }

        /// <summary>
        /// Filter data by postcode (if applicable)
        /// </summary>
        /// <param name="APartnerkeys"></param>
        /// <param name="AAddressFilterAdded"></param>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        public static void PostcodeFilter(ref DataTable APartnerkeys,
            ref bool AAddressFilterAdded,
            TParameterList AParameters,
            TDBTransaction ATransaction)
        {
            // if filter exists
            if ((AParameters.Exists("param_region") && !string.IsNullOrEmpty(AParameters.Get("param_region").ToString()))
                || (AParameters.Exists("param_postcode_from") && !string.IsNullOrEmpty(AParameters.Get("param_postcode_from").ToString()))
                || (AParameters.Exists("param_postcode_to") && !string.IsNullOrEmpty(AParameters.Get("param_postcode_to").ToString())))
            {
                DataTable partnerkeysCopy = APartnerkeys.Copy();
                int i = 0;

                foreach (DataRow Row in partnerkeysCopy.Rows)
                {
                    // get postcode for current partner's location
                    PLocationRow LocationRow = (PLocationRow)PLocationAccess.LoadByPrimaryKey(
                        Convert.ToInt64(Row["p_site_key_n"]), Convert.ToInt32(Row["p_location_key_i"]),
                        ATransaction)[0];

                    if (!AddressMeetsPostCodeCriteriaOrEmpty(LocationRow.PostalCode,
                            AParameters.Get("param_region").ToString(),
                            AParameters.Get("param_postcode_from").ToString(),
                            AParameters.Get("param_postcode_to").ToString()))
                    {
                        // remove record if it is excluded by the filter
                        APartnerkeys.Rows.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                AAddressFilterAdded = true;
            }
        }

        /// <summary>
        /// extend query statement and query parameter list by address filter information given in extract parameters
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="AOdbcParameterList"></param>
        /// <returns>true if address tables and fields were added</returns>
        protected static bool AddAddressFilter(TParameterList AParameters, ref string ASqlStmt,
            ref List <OdbcParameter>AOdbcParameterList)
        {
            string WhereClause = "";
            string TableNames = "";
            string FieldNames = "";
            string OrderByClause = "";
            string StringValue;
            DateTime DateValue;
            bool LocationTableNeeded = false;
            bool PartnerLocationTableNeeded = false;
            bool AddressFilterAdded = false;

            // add check for mailing addresses only
            if (AParameters.Exists("param_mailing_addresses_only"))
            {
                if (AParameters.Get("param_mailing_addresses_only").ToBool())
                {
                    WhereClause = WhereClause + " AND pub_p_partner_location.p_send_mail_l";
                    PartnerLocationTableNeeded = true;
                }
            }

            // add city statement (allow any city that begins with search string)
            if (AParameters.Exists("param_city"))
            {
                StringValue = AParameters.Get("param_city").ToString();

                if ((StringValue.Trim().Length > 0) && (StringValue != "*"))
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_city", OdbcType.VarChar)
                        {
                            Value = StringValue + "%"
                        });
                    WhereClause = WhereClause + " AND pub_p_location.p_city_c LIKE ?";
                    LocationTableNeeded = true;
                }
            }

            // add county statement (allow any county that begins with search string)
            if (AParameters.Exists("param_county"))
            {
                StringValue = AParameters.Get("param_county").ToString();

                if ((StringValue.Trim().Length > 0) && (StringValue != "*"))
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_county", OdbcType.VarChar)
                        {
                            Value = StringValue + "%"
                        });
                    WhereClause = WhereClause + " AND pub_p_location.p_county_c LIKE ?";
                    LocationTableNeeded = true;
                }
            }

            // add statement for country
            if (AParameters.Exists("param_country"))
            {
                StringValue = AParameters.Get("param_country").ToString();

                if (StringValue.Trim().Length > 0)
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_country", OdbcType.VarChar)
                        {
                            Value = StringValue
                        });
                    WhereClause = WhereClause + " AND pub_p_location.p_country_code_c = ?";
                    LocationTableNeeded = true;
                }
            }

            // postcode filter will be applied after the data is obtained
            if (AParameters.Exists("param_region") || AParameters.Exists("param_postcode_from") || AParameters.Exists("param_postcode_to"))
            {
                LocationTableNeeded = true;
            }

            // add date clause if address should only be valid at a certain date
            if (AParameters.Exists("param_only_addresses_valid_on")
                && (AParameters.Get("param_only_addresses_valid_on").ToBool()))
            {
                if (AParameters.Exists("param_address_date_valid_on")
                    && !AParameters.Get("param_address_date_valid_on").IsZeroOrNull())
                {
                    DateValue = AParameters.Get("param_address_date_valid_on").ToDate();
                }
                else
                {
                    // if date not given then use "Today"
                    DateValue = DateTime.Today;
                }

                AOdbcParameterList.Add(new OdbcParameter("param_address_date_valid_on_1", OdbcType.Date)
                    {
                        Value = DateValue
                    });
                AOdbcParameterList.Add(new OdbcParameter("param_address_date_valid_on_2", OdbcType.Date)
                    {
                        Value = DateValue
                    });
                AOdbcParameterList.Add(new OdbcParameter("param_address_date_valid_on_3", OdbcType.Date)
                    {
                        Value = DateValue
                    });

                WhereClause = WhereClause +
                              " AND (   (    pub_p_partner_location.p_date_effective_d <= ?" +
                              "      AND pub_p_partner_location.p_date_good_until_d IS NULL)" +
                              "  OR (    pub_p_partner_location.p_date_effective_d <= ?" +
                              "      AND pub_p_partner_location.p_date_good_until_d >= ?))";
                LocationTableNeeded = true;
            }
            else
            {
                // if not valid on certain date then check if date range is filled
                if (AParameters.Exists("param_address_start_from")
                    && !AParameters.Get("param_address_start_from").IsZeroOrNull())
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_address_start_from", OdbcType.Date)
                        {
                            Value = AParameters.Get("param_address_start_from").ToDate()
                        });

                    WhereClause = WhereClause +
                                  " AND pub_p_partner_location.p_date_effective_d >= ?";
                    LocationTableNeeded = true;
                }

                if (AParameters.Exists("param_address_start_to")
                    && !AParameters.Get("param_address_start_to").IsZeroOrNull())
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_address_start_to", OdbcType.Date)
                        {
                            Value = AParameters.Get("param_address_start_to").ToDate()
                        });

                    WhereClause = WhereClause +
                                  " AND pub_p_partner_location.p_date_effective_d <= ?";
                    LocationTableNeeded = true;
                }

                if (AParameters.Exists("param_address_end_from")
                    && !AParameters.Get("param_address_end_from").IsZeroOrNull())
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_address_end_from", OdbcType.Date)
                        {
                            Value = AParameters.Get("param_address_end_from").ToDate()
                        });

                    WhereClause = WhereClause +
                                  " AND (    pub_p_partner_location.p_date_good_until_d IS NOT NULL" +
                                  "      AND pub_p_partner_location.p_date_good_until_d >= ?)";
                    LocationTableNeeded = true;
                }

                if (AParameters.Exists("param_address_end_to")
                    && !AParameters.Get("param_address_end_to").IsZeroOrNull())
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_address_end_to", OdbcType.Date)
                        {
                            Value = AParameters.Get("param_address_end_to").ToDate()
                        });

                    WhereClause = WhereClause +
                                  " AND (    pub_p_partner_location.p_date_good_until_d IS NOT NULL" +
                                  "      AND pub_p_partner_location.p_date_good_until_d <= ?)";
                    LocationTableNeeded = true;
                }
            }

            // add statement for location type
            if (AParameters.Exists("param_location_type"))
            {
                StringValue = AParameters.Get("param_location_type").ToString();

                if (StringValue.Trim().Length > 0)
                {
                    List <String>param_location_type = new List <String>();

                    foreach (TVariant choice in AParameters.Get("param_location_type").ToComposite())
                    {
                        param_location_type.Add(choice.ToString());
                    }

                    AOdbcParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_location_type",
                            OdbcType.VarChar,
                            param_location_type));
                    WhereClause = WhereClause + " AND pub_p_partner_location.p_location_type_c IN (?)";
                    PartnerLocationTableNeeded = true;
                }
            }

            // if location table is needed then automatically partner location table is needed as well
            if (LocationTableNeeded)
            {
                FieldNames = ", pub_p_partner_location.p_site_key_n, pub_p_partner_location.p_location_key_i ";
                TableNames = TableNames + ", pub_p_location, pub_p_partner_location";

                WhereClause = " AND pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n" +
                              " AND pub_p_location.p_site_key_n = pub_p_partner_location.p_site_key_n" +
                              " AND pub_p_location.p_location_key_i = pub_p_partner_location.p_location_key_i" +
                              WhereClause;

                OrderByClause = ", pub_p_partner.p_partner_key_n";
            }
            else if (PartnerLocationTableNeeded)
            {
                FieldNames = ", pub_p_partner_location.p_site_key_n, pub_p_partner_location.p_location_key_i ";
                TableNames = TableNames + ", pub_p_partner_location";

                WhereClause = " AND pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n" +
                              WhereClause;

                OrderByClause = ", pub_p_partner.p_partner_key_n";
            }

            // Set information if address filter was set. It is not enough to just check if extra fields or
            // clauses were built but filter fields to be replaced also need to exist.
            if ((ASqlStmt.Contains("##address_filter_fields##")
                 || ASqlStmt.Contains("##address_filter_tables##")
                 || ASqlStmt.Contains("##address_filter_where_clause##")
                 || ASqlStmt.Contains("##address_filter_order_by_clause##"))
                && ((TableNames.Length > 0)
                    || (WhereClause.Length > 0)))
            {
                AddressFilterAdded = true;
            }
            else
            {
                AddressFilterAdded = false;
            }

            ASqlStmt = ASqlStmt.Replace("##address_filter_fields##", FieldNames);
            ASqlStmt = ASqlStmt.Replace("##address_filter_tables##", TableNames);
            ASqlStmt = ASqlStmt.Replace("##address_filter_where_clause##", WhereClause);
            ASqlStmt = ASqlStmt.Replace("##address_filter_order_by_clause##", OrderByClause);

            return AddressFilterAdded;
        }

        /// <summary>
        /// this will return true if the best address is in the given postal region, postcode range and county it will not reset the data returned by best address
        /// </summary>
        /// <returns>void</returns>
        public static bool AddressMeetsPostCodeCriteriaOrEmpty(string APostcode,
            String APostalRegion,
            String APostCodeFrom,
            String APostCodeTo)
        {
            bool ReturnValue = false;

            if ((APostalRegion == "") && (APostCodeFrom == "") && (APostCodeTo == ""))
            {
                return true;
            }

            // Filter by region
            if (APostalRegion.Length > 0)
            {
                if ((APostcode != "") && PostcodeInRegion(APostcode, APostalRegion))
                {
                    ReturnValue = true;
                }
                else
                {
                    return false;
                }
            }

            // filter by specified postcode range
            if ((APostCodeFrom.Length > 0) && (APostCodeTo.Length > 0))
            {
                if ((APostcode != "") && (ComparePostcodes(APostCodeFrom.ToLower(),
                                              APostcode.ToLower()) <= 0) && (ComparePostcodes(APostCodeTo.ToLower(), APostcode.ToLower()) >= 0))
                {
                    ReturnValue = true;
                }
                else
                {
                    return false;
                }
            }
            else if (APostCodeFrom.Length > 0)
            {
                if ((APostcode != "") && (ComparePostcodes(APostCodeFrom.ToLower(),
                                              APostcode.ToLower()) <= 0))
                {
                    ReturnValue = true;
                }
                else
                {
                    return false;
                }
            }
            else if (APostCodeTo.Length > 0)
            {
                if ((APostcode != "") && (ComparePostcodes(APostCodeTo.ToLower(), APostcode.ToLower()) >= 0))
                {
                    ReturnValue = true;
                }
                else
                {
                    return false;
                }
            }

            return ReturnValue;
        }

        private static string FPostalRegion = "";
        private static PPostcodeRangeTable FPostcodeRangeTable = null;

        // check if a postcode is contained within a region
        private static bool PostcodeInRegion(string APostcode, string APostalRegion)
        {
            // Regions datatable should only be loaded once per extract generation
            if (FPostalRegion != APostalRegion)
            {
                TDBTransaction ReadTransaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                    delegate
                    {
                        FPostcodeRangeTable = PPostcodeRangeAccess.LoadViaPPostcodeRegion(APostalRegion, ReadTransaction);
                        FPostalRegion = APostalRegion;
                    });
            }

            foreach (PPostcodeRangeRow Row in FPostcodeRangeTable.Rows)
            {
                if ((ComparePostcodes(Row.From.ToLower(), APostcode.ToLower()) <= 0)
                    && (ComparePostcodes(Row.To.ToLower(), APostcode.ToLower()) >= 0))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Compares two postcodes (universally) and determines which is greater.
        /// </summary>
        /// <param name="APostcodeA"></param>
        /// <param name="APostcodeB"></param>
        /// <returns>-1 if APostcodeB is greater, 0 if equal, 1 if APostcodeA is greater</returns>
        private static int ComparePostcodes(string APostcodeA, string APostcodeB)
        {
            int ReturnValue = 0;

            //
            // if postcode only contains numbers
            //

            int PostcodeLettersA = Regex.Matches(APostcodeA, @"[a-zA-Z]").Count;
            int PostcodeLettersB = Regex.Matches(APostcodeB, @"[a-zA-Z]").Count;

            if ((PostcodeLettersA == 0) && (PostcodeLettersB == 0))
            {
                Int64 PostcodeNumberA = Convert.ToInt64(APostcodeA.Replace(" ", "").Replace("-", ""));
                Int64 PostcodeNumberB = Convert.ToInt64(APostcodeB.Replace(" ", "").Replace("-", ""));

                return PostcodeNumberA.CompareTo(PostcodeNumberB);
            }

            //
            // if postcode contains letters as well
            //

            // if postcode contains a space or a hyphen then use recursion to compare both halves
            int SpaceIndexA = APostcodeA.IndexOf(' ');
            int SpaceIndexB = APostcodeB.IndexOf(' ');
            int HyphenIndexA = APostcodeA.IndexOf('-');
            int HyphenIndexB = APostcodeB.IndexOf('-');

            if ((SpaceIndexA != -1) || (SpaceIndexB != -1))
            {
                if (SpaceIndexA == -1)
                {
                    SpaceIndexA = APostcodeA.Length;
                }

                if (SpaceIndexB == -1)
                {
                    SpaceIndexB = APostcodeB.Length;
                }

                int CompareSubstring = ComparePostcodes(APostcodeA.Substring(0, SpaceIndexA), APostcodeB.Substring(0, SpaceIndexB));

                if (CompareSubstring == 0)
                {
                    if (SpaceIndexA + 1 >= APostcodeA.Length)
                    {
                        return -1;
                    }
                    else if (SpaceIndexB + 1 >= APostcodeB.Length)
                    {
                        return 1;
                    }

                    return ComparePostcodes(APostcodeA.Substring(SpaceIndexA + 1), APostcodeB.Substring(SpaceIndexB + 1));
                }
                else
                {
                    return CompareSubstring;
                }
            }

            if ((HyphenIndexA != -1) || (HyphenIndexB != -1))
            {
                if (HyphenIndexA == -1)
                {
                    HyphenIndexA = APostcodeA.Length;
                }

                if (HyphenIndexB == -1)
                {
                    HyphenIndexB = APostcodeB.Length;
                }

                int CompareSubstring = ComparePostcodes(APostcodeA.Substring(0, HyphenIndexA), APostcodeB.Substring(0, HyphenIndexB));

                if (CompareSubstring == 0)
                {
                    if (HyphenIndexA + 1 >= APostcodeA.Length)
                    {
                        return -1;
                    }
                    else if (HyphenIndexB + 1 >= APostcodeB.Length)
                    {
                        return 1;
                    }

                    return ComparePostcodes(APostcodeA.Substring(HyphenIndexA + 1), APostcodeB.Substring(HyphenIndexB + 1));
                }
                else
                {
                    return CompareSubstring;
                }
            }

            // change postcodes to uppercase
            APostcodeA = APostcodeA.ToUpper();
            APostcodeB = APostcodeB.ToUpper();

            while (true)
            {
                if ((APostcodeA.Length == 0) && (APostcodeB.Length == 0))
                {
                    return 0;
                }
                else if (APostcodeA.Length == 0)
                {
                    return -1;
                }
                else if (APostcodeB.Length == 0)
                {
                    return 1;
                }

                int NumberIndexA = APostcodeA.IndexOfAny("0123456789".ToCharArray());
                int NumberIndexB = APostcodeB.IndexOfAny("0123456789".ToCharArray());
                int NumberLengthA = 0;
                int NumberLengthB = 0;
                int LetterIndexA = APostcodeA.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
                int LetterIndexB = APostcodeB.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
                int LetterLengthA = 0;
                int LetterLengthB = 0;

                // if one postcode starts with a number but the other starts with a letter
                if ((NumberIndexA == 0) && (NumberIndexB != 0))
                {
                    return -1;
                }
                else if ((NumberIndexA != 0) && (NumberIndexB == 0))
                {
                    return 1;
                }

                // if both postcodes begin with a number
                if ((NumberIndexA == 0) && (NumberIndexB == 0))
                {
                    // find character indexs and lengths
                    if (LetterIndexA == -1)
                    {
                        NumberLengthA = APostcodeA.Length;
                    }
                    else
                    {
                        NumberLengthA = LetterIndexA;
                    }

                    if (LetterIndexB == -1)
                    {
                        NumberLengthB = APostcodeA.Length;
                    }
                    else
                    {
                        NumberLengthB = LetterIndexB;
                    }

                    // compare the beginning number
                    ReturnValue =
                        Convert.ToInt64(APostcodeA.Substring(0, NumberLengthA)).CompareTo(Convert.ToInt64(APostcodeB.Substring(0, NumberLengthB)));

                    if (ReturnValue != 0)
                    {
                        return ReturnValue;
                    }

                    // if numbers are equal...
                    for (int i = LetterIndexA; i < APostcodeA.Length; i++)
                    {
                        if (char.IsNumber(APostcodeA[i]))
                        {
                            break;
                        }

                        LetterLengthA++;
                    }

                    for (int i = LetterIndexB; i < APostcodeB.Length; i++)
                    {
                        if (char.IsNumber(APostcodeB[i]))
                        {
                            break;
                        }

                        LetterLengthB++;
                    }

                    // compare letters
                    ReturnValue = APostcodeA.Substring(LetterIndexA, LetterLengthA).CompareTo(APostcodeB.Substring(LetterIndexB, LetterLengthB));

                    if (ReturnValue != 0)
                    {
                        return ReturnValue;
                    }

                    // if letters are equal...
                    APostcodeA = APostcodeA.Substring(NumberLengthA + LetterLengthA);
                    APostcodeB = APostcodeB.Substring(NumberLengthB + LetterLengthB);
                }
                // if bothe postcodes start with a letter
                else if ((LetterIndexA == 0) && (LetterIndexB == 0))
                {
                    // find character indexs and lengths
                    if (NumberIndexA == -1)
                    {
                        LetterLengthA = APostcodeA.Length;
                    }
                    else
                    {
                        LetterLengthA = NumberIndexA;
                    }

                    if (NumberIndexB == -1)
                    {
                        LetterLengthB = APostcodeB.Length;
                    }
                    else
                    {
                        LetterLengthB = NumberIndexB;
                    }

                    // compare the beginning letters
                    ReturnValue = APostcodeA.Substring(0, LetterLengthA).CompareTo(APostcodeB.Substring(0, LetterLengthB));

                    if (ReturnValue != 0)
                    {
                        return ReturnValue;
                    }

                    // if letters are equal...
                    for (int i = NumberIndexA; i < APostcodeA.Length; i++)
                    {
                        if (!char.IsNumber(APostcodeA[i]))
                        {
                            break;
                        }

                        NumberLengthA++;
                    }

                    for (int i = NumberIndexB; i < APostcodeB.Length; i++)
                    {
                        if (!char.IsNumber(APostcodeB[i]))
                        {
                            break;
                        }

                        NumberLengthB++;
                    }

                    // compare number
                    ReturnValue =
                        Convert.ToInt64(APostcodeA.Substring(NumberIndexA,
                                NumberLengthA)).CompareTo(Convert.ToInt64(APostcodeB.Substring(NumberIndexB, NumberLengthB)));

                    if (ReturnValue != 0)
                    {
                        return ReturnValue;
                    }

                    // if numbers are equal...
                    APostcodeA = APostcodeA.Substring(NumberLengthA + LetterLengthA);
                    APostcodeB = APostcodeB.Substring(NumberLengthB + LetterLengthB);
                }
            }
        }

        /// <summary>
        /// This method needs to be implemented by extracts that can't follow the default processing with just
        /// one query.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AExtractId"></param>
        protected virtual bool RunSpecialTreatment(TParameterList AParameters, TDBTransaction ATransaction, out int AExtractId)
        {
            AExtractId = -1;
            return true;
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASqlParameterList"></param>
        protected abstract void RetrieveParameters (TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASqlParameterList);
    }
}