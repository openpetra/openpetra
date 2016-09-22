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
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Extracts;

namespace Ict.Petra.Server.MPartner.queries
{
    /// <summary>
    /// this creates the query for extracts of partners by special types
    /// </summary>
    public class QueryPartnerByGeneralCriteria : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners of a given type (or selection of multiple types)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByGeneralCriteria.sql");

            // create a new object of this class and control extract calculation from base class
            QueryPartnerByGeneralCriteria ExtractQuery = new QueryPartnerByGeneralCriteria();

            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults);
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASQLParameterList"></param>
        protected override void RetrieveParameters(TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASQLParameterList)
        {
            string WhereClause = "";
            string TableNames = "";
            string StringValue;

            // add parameters to sql parameter list

            // Partner Class
            ASQLParameterList.Add(new OdbcParameter("param_partner_class_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_partner_class").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_partner_class", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_partner_class").ToString()
                });

            // Partner Status
            ASQLParameterList.Add(new OdbcParameter("param_partner_status_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_partner_status").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_partner_status", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_partner_status").ToString()
                });

            // Language Code
            ASQLParameterList.Add(new OdbcParameter("param_language_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_language").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_language", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_language").ToString()
                });

            // Active Partners and No Solicitations
            //ASQLParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
            //    {
            //        Value = AParameters.Get("param_active").ToBool()
            //    });
            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });

            // User that created/modified the record
            ASQLParameterList.Add(new OdbcParameter("param_user_created_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_user_created").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_user_created", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_user_created").ToString()
                });
            ASQLParameterList.Add(new OdbcParameter("param_user_modified_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_user_modified").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_user_modified", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_user_modified").ToString()
                });

            // Date range for creation and modification of record
            ASQLParameterList.Add(new OdbcParameter("param_date_created_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_created_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_created_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_created_from").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_created_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_created_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_created_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_created_to").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_modified_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_modified_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_modified_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_modified_from").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_modified_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_modified_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_modified_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_modified_to").ToDate()
                });

            // add statement for church denomination
            if (AParameters.Exists("param_denomination"))
            {
                StringValue = AParameters.Get("param_denomination").ToString();

                if (StringValue.Trim().Length > 0)
                {
                    ASQLParameterList.Add(new OdbcParameter("param_denomination", OdbcType.VarChar)
                        {
                            Value = StringValue
                        });
                    TableNames = ", pub_p_church";
                    WhereClause = " AND pub_p_church.p_partner_key_n = pub_p_partner.p_partner_key_n" +
                                  " AND pub_p_church.p_denomination_code_c = ?";
                }
            }

            // add statement for organisation business code
            if (AParameters.Exists("param_business"))
            {
                StringValue = AParameters.Get("param_business").ToString();

                if (StringValue.Trim().Length > 0)
                {
                    ASQLParameterList.Add(new OdbcParameter("param_business", OdbcType.VarChar)
                        {
                            Value = StringValue
                        });
                    TableNames = ", pub_p_organisation";
                    WhereClause = " AND pub_p_organisation.p_partner_key_n = pub_p_partner.p_partner_key_n" +
                                  " AND pub_p_organisation.p_business_code_c = ?";
                }
            }

            ASqlStmt = ASqlStmt.Replace("##partner_specific_tables##", TableNames);
            ASqlStmt = ASqlStmt.Replace("##partner_specific_where_clause##", WhereClause);
        }
    }
}