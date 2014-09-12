//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andrewW
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
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryPartnerByRelationship : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners in selected relationships with selected partner
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = "";

            if (AParameters.Get("param_selection").ToString() == "an extract")
            {
                SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractFromExtractByPartnerRelationship.sql");
            }
            else if (AParameters.Get("param_selection").ToString() == "one partner")
            {
                SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractByPartnerRelationship.sql");
            }
            else
            {
                throw new ArgumentException("Must supply an extract or partner key.");
            }

            // create a new object of this class and control extract calculation from base class
            QueryPartnerByRelationship ExtractQuery = new QueryPartnerByRelationship();

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
            List <String>param_relationships = new List <String>();
            List <String>param_reciprocal_relationships = new List <String>();

            foreach (var choice in AParameters.Get("param_explicit_relationships").ToString().Split(','))
            {
                param_relationships.Add(choice);
            }

            foreach (var choice in AParameters.Get("param_reciprocal_relationships").ToString().Split(','))
            {
                param_reciprocal_relationships.Add(choice);
            }

            if ((param_relationships.Count == 0) && (param_reciprocal_relationships.Count == 0))
            {
                throw new NoNullAllowedException("At least one option must be checked.");
            }

            ASQLParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });

            string paramName;

            if (AParameters.Get("param_selection").ToString() == "an extract")
            {
                paramName = "param_extract";
                ASQLParameterList.Add(new OdbcParameter(paramName, OdbcType.VarChar)
                    {
                        Value = AParameters.Get(paramName).ToString()
                    });
                ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_relationships", OdbcType.VarChar, param_relationships));
                ASQLParameterList.Add(new OdbcParameter(paramName, OdbcType.VarChar)
                    {
                        Value = AParameters.Get(paramName)
                    });
                ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_reciprocal_relationships", OdbcType.VarChar,
                        param_reciprocal_relationships));
            }
            else
            {
                paramName = "param_partnerkey";
                ASQLParameterList.Add(new OdbcParameter(paramName, OdbcType.Int)
                    {
                        Value = AParameters.Get(paramName).ToInt32()
                    });
                ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_relationships", OdbcType.VarChar, param_relationships));
                ASQLParameterList.Add(new OdbcParameter(paramName, OdbcType.Int)
                    {
                        Value = AParameters.Get(paramName).ToInt32()
                    });
                ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_reciprocal_relationships", OdbcType.VarChar,
                        param_reciprocal_relationships));
            }

            ASQLParameterList.Add(new OdbcParameter("param_active", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_active").ToBool()
                });
            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_no_solicitations").ToBool()
                });
        }
    }
}