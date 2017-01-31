//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       binki
//
// Copyright 2004-2012 by OM International
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

namespace Ict.Petra.Server.MPersonnel.queries
{
    /// <summary>
    /// find all partners that match a commitment
    /// </summary>
    public class QueryPartnerByCommitment : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// Calculate an extract of partners with commitments.
        /// </summary>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByCommitment.sql");

            // create a new object of this class and control extract calculation from base class
            QueryPartnerByCommitment ExtractQuery = new QueryPartnerByCommitment();

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
            // prepare list of commitment types
            List <String>param_commitment_status_choices = new List <String>();

            foreach (TVariant choice in AParameters.Get("param_commitment_status_choices").ToComposite())
            {
                param_commitment_status_choices.Add(choice.ToString());
            }

            // now add parameters to sql parameter list
            ASQLParameterList.Add(new OdbcParameter("param_start_date_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_start_date_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_start_date_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_start_date_from").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_start_date_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_start_date_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_start_date_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_start_date_to").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_end_date_from_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_end_date_from").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_end_date_from", OdbcType.Date)
                {
                    Value = AParameters.Get("param_end_date_from").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_end_date_to_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_end_date_to").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_end_date_to", OdbcType.Date)
                {
                    Value = AParameters.Get("param_end_date_to").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_valid_on_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_date_valid_on").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_valid_on_a", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_valid_on").ToDate()
                });
            ASQLParameterList.Add(new OdbcParameter("param_date_valid_on_b", OdbcType.Date)
                {
                    Value = AParameters.Get("param_date_valid_on").ToDate()
                });

            ASQLParameterList.Add(new OdbcParameter("param_field_sending_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_field_sending").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_field_sending", OdbcType.Int)
                {
                    Value = AParameters.Get("param_field_sending").ToInt32()
                });
            ASQLParameterList.Add(new OdbcParameter("param_field_receiving_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_field_receiving").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_field_receiving", OdbcType.Int)
                {
                    Value = AParameters.Get("param_field_receiving").ToInt32()
                });

            ASQLParameterList.Add(new OdbcParameter("param_consider_commitment_status_not", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_all_partners").ToBool()
                });
            ASQLParameterList.Add(TDbListParameterValue.OdbcListParameterValue("param_commitment_status_choices",
                    OdbcType.VarChar,
                    param_commitment_status_choices));
            ASQLParameterList.Add(new OdbcParameter("param_include_no_commitment_status", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_include_no_commitment_status").ToBool()
                });

            ASQLParameterList.Add(new OdbcParameter("param_active_not", OdbcType.Bit)
                {
                    Value = !AParameters.Get("param_active").ToBool()
                });

            ASQLParameterList.Add(new OdbcParameter("param_exclude_no_solicitations_not", OdbcType.Bit)
                {
                    Value = !AParameters.Get("param_exclude_no_solicitations").ToBool()
                });
        }
    }
}