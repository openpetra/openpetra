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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MPartner.Extracts;

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
            Boolean NewTransaction;
            TDBTransaction Transaction;
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();
            bool AddressFilterAdded;

            AExtractId = -1;

            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
            
            // get the partner keys from the database
            try
            {

                if (FSpecialTreatment)
                {
                    ReturnValue = RunSpecialTreatment(AParameters, Transaction, out AExtractId);
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

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }

                    // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                    // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
                    if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                    {
                        return false;
                    }

                    TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

                    // create an extract with the given name in the parameters
                    ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                        AParameters.Get("param_extract_name").ToString(),
                        AParameters.Get("param_extract_description").ToString(),
                        out AExtractId,
                        partnerkeys,
                        0,
                        AddressFilterAdded,
                        true);
                }

                if (ReturnValue)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                return ReturnValue;
            }
            catch (Exception Exc) 
            {
                TLogging.Log("An Exception occured in CalculateExtractInternal:" + Environment.NewLine + Exc.ToString());
                
                if (NewTransaction)
                {                
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
                
                throw;
            }                        
        }

        /// <summary>
        /// extend query statement and query parameter list by address filter information given in extract parameters
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="AOdbcParameterList"></param>
        /// <returns>true if address tables and fields were added</returns>
        protected bool AddAddressFilter(TParameterList AParameters, ref string ASqlStmt,
            ref List <OdbcParameter>AOdbcParameterList)
        {
            string WhereClause = "";
            string TableNames = "";
            string FieldNames = "";
            string OrderByClause = "";
            string StringValue;
            DateTime DateValue;
            string PostCodeFrom = "";
            string PostCodeTo = "";
            bool LocationTableNeeded = false;
            bool PartnerLocationTableNeeded = false;
            bool RegionTableNeeded = false;
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

            // add statement for postcode from and to
            if (AParameters.Exists("param_postcode_from"))
            {
                PostCodeFrom = AParameters.Get("param_postcode_from").ToString().Trim();
            }

            if (AParameters.Exists("param_postcode_to"))
            {
                PostCodeFrom = AParameters.Get("param_postcode_to").ToString().Trim();
            }

            if ((PostCodeFrom.Length > 0)
                && (PostCodeTo.Length > 0))
            {
                // both "from" and "to" field are filled
                AOdbcParameterList.Add(new OdbcParameter("param_postcode_from", OdbcType.VarChar)
                    {
                        Value = PostCodeFrom
                    });
                AOdbcParameterList.Add(new OdbcParameter("param_postcode_to", OdbcType.VarChar)
                    {
                        Value = PostCodeTo
                    });
                WhereClause = WhereClause + " AND (pub_p_location.p_postal_code_c >= ? AND pub_p_location.p_postal_code_c <= ?)";
                LocationTableNeeded = true;
            }
            else if ((PostCodeFrom.Length > 0)
                     && (PostCodeTo.Length == 0))
            {
                // "from" field is filled but "to" field is empty
                AOdbcParameterList.Add(new OdbcParameter("param_postcode_from", OdbcType.VarChar)
                    {
                        Value = PostCodeFrom + "%"
                    });
                WhereClause = WhereClause + " AND pub_p_location.p_postal_code_c LIKE ?";
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

            // add statement for region
            if (AParameters.Exists("param_region"))
            {
                StringValue = AParameters.Get("param_region").ToString();

                if (StringValue.Trim().Length > 0)
                {
                    AOdbcParameterList.Add(new OdbcParameter("param_region", OdbcType.VarChar)
                        {
                            Value = StringValue
                        });
                    WhereClause = WhereClause +
                                  " AND (    pub_p_postcode_range.p_from_c <= pub_p_location.p_postal_code_c" +
                                  " AND pub_p_postcode_range.p_to_c   >= pub_p_location.p_postal_code_c)" +
                                  " AND (    pub_p_postcode_region.p_range_c  = pub_p_postcode_range.p_range_c" +
                                  " AND pub_p_postcode_region.p_region_c = ?)";
                    RegionTableNeeded = true;
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

            if (RegionTableNeeded)
            {
                TableNames = TableNames + ", pub_p_postcode_region, pub_p_postcode_range";
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