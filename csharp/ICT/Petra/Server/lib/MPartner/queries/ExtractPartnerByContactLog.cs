//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
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
    /// Backend for creating an Extract based off of Contact Logs
    /// </summary>
    public class QueryPartnerByContactLog : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners who have a Contact Log meeting selected criteria
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByContactLog.sql");
            QueryPartnerByContactLog ExtractQuery = new QueryPartnerByContactLog();
            
            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults);
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASQLParameterList"></param>S
        protected override void RetrieveParameters(TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASQLParameterList)
        {
            ASQLParameterList.Add(new OdbcParameter("param_has_contactor", OdbcType.Bit)
            {
                Value = !string.IsNullOrWhiteSpace(AParameters.Get("param_contactor").ToString())
            });

            ASQLParameterList.Add(new OdbcParameter("param_contactor", OdbcType.VarChar)
            {
                Value = AParameters.Get("param_contactor").ToString()
            });

            ASQLParameterList.Add(new OdbcParameter("param_has_contact_code", OdbcType.Bit)
            {
                Value = !string.IsNullOrWhiteSpace(AParameters.Get("param_contact_code").ToString())
            });

            ASQLParameterList.Add(new OdbcParameter("param_contact_code", OdbcType.VarChar)
            {
                Value = AParameters.Get("param_contact_code").ToString()
            });

            ASQLParameterList.Add(new OdbcParameter("param_has_mailing_code", OdbcType.Bit)
            {
                Value = !string.IsNullOrWhiteSpace(AParameters.Get("param_mailing_code").ToString())
            });

            ASQLParameterList.Add(new OdbcParameter("param_mailing_code", OdbcType.VarChar)
            {
                Value = AParameters.Get("param_mailing_code").ToString()
            });

            List<String> param_contact_attributes = new List<String>();
            foreach (TVariant choice in AParameters.Get("param_contact_attributes").ToComposite())
            {
                if(choice.ToString().Length > 0)
                    param_contact_attributes.Add(choice.ToString());
            }

            ASQLParameterList.Add(new OdbcParameter("param_has_contact_attributes", OdbcType.Bit)
            {
                Value = param_contact_attributes.Any()
            });

            ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_explicit_publication",
                OdbcType.VarChar,
                param_contact_attributes));

            ASQLParameterList.Add(new OdbcParameter("param_has_date_from", OdbcType.Bit)
            {
                Value = AParameters.Get("param_date_from") != null && AParameters.Get("param_date_from").ToDate() > DateTime.MinValue
            });

            ASQLParameterList.Add(new OdbcParameter("param_date_from", OdbcType.Date)
            {
                Value = AParameters.Get("param_date_from").ToDate()
            });

            ASQLParameterList.Add(new OdbcParameter("param_has_date_to", OdbcType.Bit)
            {
                Value = AParameters.Get("param_date_to") != null && AParameters.Get("param_date_to").ToDate() > DateTime.MinValue
            });

            ASQLParameterList.Add(new OdbcParameter("param_date_to", OdbcType.Date)
            {
                Value = AParameters.Get("param_date_to").ToDate()
            });



            ASQLParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
            {
                Value = AParameters.Get("param_active").ToBool()
            });
            ASQLParameterList.Add(new OdbcParameter("param_families_only", OdbcType.Bit)
            {
                Value = AParameters.Get("param_families_only").ToBool()
            });
            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
            {
                Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
            });
        }
    }
}