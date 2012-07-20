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
    /// query finds all family records for person records given in a base extract
    /// </summary>
    public class QueryFamilyExtractForPersons : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all partners living in a given city
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <param name="AExtractId"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults, out int AExtractId)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractFamilyForPersons.sql");

            // create a new object of this class and control extract calculation from base class
            QueryFamilyExtractForPersons ExtractQuery = new QueryFamilyExtractForPersons();

            return ExtractQuery.CalculateExtractInternal(AParameters, SqlStmt, AResults, out AExtractId);
        }

        /// <summary>
        /// retrieve parameters from client sent in AParameters and build up AParameterList to run SQL query
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="ASqlStmt"></param>
        /// <param name="ASQLParameterList"></param>
        protected override void RetrieveParameters(TParameterList AParameters, ref string ASqlStmt, ref List <OdbcParameter>ASQLParameterList)
        {
            // now add parameters to sql parameter list
            ASQLParameterList.Add(new OdbcParameter("base_extract", OdbcType.Int)
                {
                    Value = AParameters.Get("param_base_extract").ToString()
                });
        }
    }
}