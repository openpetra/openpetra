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
    /// this report is queries the database for partners that are subscribed to certain publications and also filters the data by other criteria
    /// </summary>
    public class QueryPartnerBySubscription : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners that are subscribed to certain publications and other criteria
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerBySubscription.sql");

            // create a new object of this class and control extract calculation from base class
            QueryPartnerBySubscription ExtractQuery = new QueryPartnerBySubscription();

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
            // prepare list of selected publications
            List <String>param_explicit_publication = new List <String>();

            foreach (TVariant choice in AParameters.Get("param_explicit_publication").ToComposite())
            {
                param_explicit_publication.Add(choice.ToString());
            }

            // now add parameters to sql parameter list
            ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_explicit_publication",
                    OdbcType.VarChar,
                    param_explicit_publication));

            ASQLParameterList.Add(new OdbcParameter("param_free_subscriptions_only", OdbcType.Bit)
            {
                Value = AParameters.Get("param_free_subscriptions_only").ToBool()
            });

            ASQLParameterList.Add(new OdbcParameter("param_include_active_subscriptions_only", OdbcType.Bit)
            {
                Value = AParameters.Get("param_include_active_subscriptions_only").ToBool()
            });

            ASQLParameterList.Add(new OdbcParameter("param_include_active_subscriptions_only", OdbcType.Bit)
            {
                Value = AParameters.Get("param_include_active_subscriptions_only").ToBool()
            });

            ASQLParameterList.Add(new OdbcParameter("param_subscription_status", OdbcType.VarChar)
            {
                Value = AParameters.Get("param_subscription_status").ToString()
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