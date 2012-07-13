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

namespace Ict.Petra.Server.MFinance.queries
{
    /// <summary>
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryRecipientByField : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all recipient that have given to particular fields (ledgers)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Gift.Queries.ExtractRecipientByField.sql");

            // create a new object of this class and control extract calculation from base class
            QueryRecipientByField ExtractQuery = new QueryRecipientByField();

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
            bool AllLedgers;

            ICollection <String>param_ledgers;

            AllLedgers = AParameters.Get("param_all_ledgers").ToBool();

            // now add parameters to sql parameter list
            ASQLParameterList.Add(new OdbcParameter("param_all_ledgers", OdbcType.Bit)
                {
                    Value = AllLedgers
                });

            if (AllLedgers)
            {
                // Add dummy value in case of an empty list so sql query does not fail.
                // This value is irrelevant in this case.
                ASQLParameterList.Add(new OdbcParameter("ledgers", OdbcType.BigInt)
                    {
                        Value = 0
                    });
            }
            else
            {
                // prepare list of ledgers
                param_ledgers = AParameters.Get("param_ledgers").ToString().Split(new Char[] { ',', });
                ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("ledgers", OdbcType.BigInt, param_ledgers));
            }

            ASQLParameterList.Add(new OdbcParameter("param_date_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_from").ToDate()
                });

            ASQLParameterList.Add(new OdbcParameter("param_date_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_to").ToDate()
                });
        }
    }
}