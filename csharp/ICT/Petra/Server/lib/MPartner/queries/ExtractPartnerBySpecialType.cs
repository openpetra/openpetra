//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Andrew Webster <arw7@students.calvin.edu>
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
    public class QueryPartnerBySpecialType : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners of a given type (or selection of multiple types)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractByPartnerSpecialType.sql");

            // create a new object of this class and control extract calculation from base class
            QueryPartnerBySpecialType ExtractQuery = new QueryPartnerBySpecialType();

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
            ICollection <String>param_explicit_specialtypes;

            // prepare list of special types
            param_explicit_specialtypes = AParameters.Get("param_explicit_specialtypes").ToString().Split(new Char[] { ',', });

            if (param_explicit_specialtypes.Count == 0)
            {
                throw new NoNullAllowedException("At least one option must be checked.");
            }

            // now add parameters to sql parameter list
            ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("specialtype", OdbcType.VarChar, param_explicit_specialtypes));
            ASQLParameterList.Add(new OdbcParameter("param_dateFieldsIncluded", OdbcType.Bit)
                {
                    Value = !AParameters.Get("param_date_set").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("Date", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_set").ToDate()
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