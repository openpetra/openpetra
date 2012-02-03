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
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MPartner.Extracts;

namespace Ict.Petra.Server.MPartner.queries
{
    /// <summary>
    /// contains helper methods that are needed to prepare queries for Extracts
    /// </summary>
    public class TExtractHelper
    {
        /// <summary>
        /// extend query statement and query parameter list by address filter information given in extract parameters
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="AOdbcParameterList"></param>
        /// <param name="AAddressFilterAdded"></param>
        /// <returns></returns>
        public static bool AddAddressFilter(TParameterList AParameters, ref string ASqlStmt, 
                                            ref TSelfExpandingArrayList AOdbcParameterList,
                                            out bool AAddressFilterAdded)
        {
        	string   WhereClause = "";
        	string   TableNames = "";
        	string   StringValue;
        	DateTime DateValue;
        	string   PostCodeFrom = "";
        	string   PostCodeTo   = "";
        	bool     LocationTableNeeded = false;
        	bool     RegionTableNeeded = false;
        	
			// add city statement (allow any city that begins with search string)        	
        	if (AParameters.Exists("param_city"))
        	{
        		StringValue = AParameters.Get("param_city").ToString();
        		if (StringValue.Trim().Length > 0 && StringValue != "*")
        		{
        			AOdbcParameterList.Add (new OdbcParameter("param_city", OdbcType.VarChar) 
        			                        {Value = StringValue + "%"});
        			WhereClause = WhereClause + " AND pub_p_location.p_city_c LIKE ?";
        			LocationTableNeeded = true;
        		}
        	}

        	// add statement for country
        	if (AParameters.Exists("param_country"))
        	{
        		StringValue = AParameters.Get("param_country").ToString();
        		if (StringValue.Trim().Length > 0)
        		{
        			AOdbcParameterList.Add (new OdbcParameter("param_country", OdbcType.VarChar) 
        			                        {Value = StringValue});
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

        	if (   PostCodeFrom.Length > 0
        	    && PostCodeTo.Length > 0)
        	{
        		// both "from" and "to" field are filled
    			AOdbcParameterList.Add (new OdbcParameter("param_postcode_from", OdbcType.VarChar) 
    			                        {Value = PostCodeFrom});
    			AOdbcParameterList.Add (new OdbcParameter("param_postcode_to", OdbcType.VarChar) 
    			                        {Value = PostCodeTo});
    			WhereClause = WhereClause + " AND (pub_p_location.p_postal_code_c >= ? AND pub_p_location.p_postal_code_c <= ?)";
      			LocationTableNeeded = true;
        	}
        	else if (   PostCodeFrom.Length > 0
        	         && PostCodeTo.Length == 0)
        	{
        		// "from" field is filled but "to" field is empty
    			AOdbcParameterList.Add (new OdbcParameter("param_postcode_from", OdbcType.VarChar) 
    			                        {Value = PostCodeFrom + "%"});
    			WhereClause = WhereClause + " AND pub_p_location.p_postal_code_c LIKE ?";
       			LocationTableNeeded = true;
        	}
        	
			// add date clause if address should only be valid at a certain date
        	if (AParameters.Exists("param_only_addresses_valid_on"))
        	{
        		if (AParameters.Get("param_only_addresses_valid_on").ToBool())
        		{
		        	if (   AParameters.Exists("param_address_date_valid_on")
        			    && !AParameters.Get("param_address_date_valid_on").IsZeroOrNull())
		        	{
        				DateValue = AParameters.Get("param_address_date_valid_on").ToDate();
        			}
		        	else
		        	{
		        		// if date not given then use "Today"
		        		DateValue = DateTime.Today;
		        	}
		        	
        			AOdbcParameterList.Add (new OdbcParameter("param_address_date_valid_on_1", OdbcType.Date) 
		        	                        {Value = DateValue});
        			AOdbcParameterList.Add (new OdbcParameter("param_address_date_valid_on_2", OdbcType.Date) 
		        	                        {Value = DateValue});
        			AOdbcParameterList.Add (new OdbcParameter("param_address_date_valid_on_3", OdbcType.Date) 
		        	                        {Value = DateValue});

        			WhereClause = WhereClause +
        				" AND (   (    pub_p_partner_location.p_date_effective_d <= ?" +
        					"      AND pub_p_partner_location.p_date_good_until_d IS NULL)" +
        					"  OR (    pub_p_partner_location.p_date_effective_d <= ?" +
        					"      AND pub_p_partner_location.p_date_good_until_d >= ?))";
        			LocationTableNeeded = true;
        		}
        	}
        	
        	// add statement for region
        	if (AParameters.Exists("param_region"))
        	{
        		StringValue = AParameters.Get("param_region").ToString();
        		if (StringValue.Trim().Length > 0)
        		{
        			AOdbcParameterList.Add (new OdbcParameter("param_region", OdbcType.VarChar) 
        			                        {Value = StringValue});
        			WhereClause = WhereClause + 
						" AND (    pub_p_postcode_range.p_from_c <= pub_p_location.p_postal_code_c" +
	                         " AND pub_p_postcode_range.p_to_c   >= pub_p_location.p_postal_code_c)" +
                		" AND (    pub_p_postcode_region.p_range_c  EQ pub_p_postcode_range.p_range_c" +
	                         " AND pub_p_postcode_region.p_region_c EQ ?)";
        			RegionTableNeeded = true;
        		}
        	}
        	
        	if (LocationTableNeeded)
        	{
        		TableNames = TableNames + ", pub_p_location, pub_p_partner_location";

        		WhereClause = " AND pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n" +
					    	  " AND pub_p_location.p_site_key_n = pub_p_partner_location.p_site_key_n" +
							  " AND pub_p_location.p_location_key_i = pub_p_partner_location.p_location_key_i" +
        					  WhereClause;
        		
        	}
        	
        	if (RegionTableNeeded)
        	{
        		TableNames = TableNames + ", pub_p_postcode_region, pub_p_postcode_range";
        	}

        	ASqlStmt = ASqlStmt.Replace("##address_filter_table_names##", TableNames);
        	ASqlStmt = ASqlStmt.Replace("##address_filter_where_clause##", WhereClause);
        	
        	if (   TableNames.Length > 0
        	    || WhereClause.Length > 0)
        	{
        		AAddressFilterAdded = true;
        	}
        	else
        	{
        		AAddressFilterAdded = false;
        	}
        	
            return true;
        }
        
        /// <summary>
        /// convert array list to array of type OdbcParameter
        /// </summary>
        /// <param name="AOdbcParameterList"></param>
        /// <returns></returns>
        public static OdbcParameter[] ConvertParameterArrayList (TSelfExpandingArrayList AOdbcParameterList)
        {
            OdbcParameter[] parameterArray = new OdbcParameter[AOdbcParameterList.Count];
            int Index = 0;
            foreach (object tempObject in AOdbcParameterList)
            {
                parameterArray[Index] = (OdbcParameter)tempObject;
                Index++;
            }
            
            return parameterArray;
        }
    }
}