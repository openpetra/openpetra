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
using Ict.Petra.Server.MPartner.Extracts;

namespace Ict.Petra.Server.MPartner.queries
{
    /// <summary>
    /// find all partners that match a commitment
    /// </summary>
    public class QueryPartnerByCommitment
    {
        /// <summary>
        /// Calculate an extract of partners with commitments.
        /// </summary>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            // get the partner keys from the database
            try
            {
                Boolean ReturnValue = false;
                Boolean NewTransaction;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                String SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByCommitment.sql");

                List <String>param_commitment_status_choices = new List <String>();

                foreach (TVariant choice in AParameters.Get("param_commitment_status_choices").ToComposite())
                {
                    param_commitment_status_choices.Add(choice.ToString());
                }

                OdbcParameter[] parameters = new OdbcParameter[]
                {
                    new OdbcParameter("param_start_date_from_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_start_date_from").IsZeroOrNull()
                    },
                    new OdbcParameter("param_start_date_from", OdbcType.Date) {
                        Value = AParameters.Get("param_start_date_from").ToDate()
                    },
                    new OdbcParameter("param_start_date_to_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_start_date_to").IsZeroOrNull()
                    },
                    new OdbcParameter("param_start_date_to", OdbcType.Date) {
                        Value = AParameters.Get("param_start_date_to").ToDate()
                    },
                    new OdbcParameter("param_end_date_from_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_end_date_from").IsZeroOrNull()
                    },
                    new OdbcParameter("param_end_date_from", OdbcType.Date) {
                        Value = AParameters.Get("param_end_date_from").ToDate()
                    },
                    new OdbcParameter("param_end_date_to_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_end_date_to").IsZeroOrNull()
                    },
                    new OdbcParameter("param_end_date_to", OdbcType.Date) {
                        Value = AParameters.Get("param_end_date_to").ToDate()
                    },
                    new OdbcParameter("param_date_valid_on_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_date_valid_on").IsZeroOrNull()
                    },
                    new OdbcParameter("param_date_valid_on_a", OdbcType.Date) {
                        Value = AParameters.Get("param_date_valid_on").ToDate()
                    },
                    new OdbcParameter("param_date_valid_on_b", OdbcType.Date) {
                        Value = AParameters.Get("param_date_valid_on").ToDate()
                    },

                    new OdbcParameter("param_field_sending_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_field_sending").IsZeroOrNull()
                    },
                    new OdbcParameter("param_field_sending", OdbcType.Int) {
                        Value = AParameters.Get("param_field_sending").ToInt32()
                    },
                    new OdbcParameter("param_field_receiving_unset", OdbcType.Bit) {
                        Value = AParameters.Get("param_field_receiving").IsZeroOrNull()
                    },
                    new OdbcParameter("param_field_receiving", OdbcType.Int) {
                        Value = AParameters.Get("param_field_receiving").ToInt32()
                    },

                    new OdbcParameter("param_consider_commitment_status_not", OdbcType.Bit) {
                        Value = !AParameters.Get("param_consider_commitment_status").ToBool()
                    },
                    TDbListParameterValue.OdbcListParameterValue("param_commitment_status_choices",
                        OdbcType.VarChar,
                        param_commitment_status_choices),
                    new OdbcParameter("param_include_no_commitment_status", OdbcType.Bit) {
                        Value = AParameters.Get("param_include_no_commitment_status").ToBool()
                    },

                    new OdbcParameter("param_active_not", OdbcType.Bit) {
                        Value = !AParameters.Get("param_active").ToBool()
                    },

/*                    new OdbcParameter("param_mailing_addresses_only_not", OdbcType.Bit) { Value = !AParameters.Get("param_mailing_addresses_only").ToBool() },*/
                    new OdbcParameter("param_exclude_no_solicitations_not", OdbcType.Bit) {
                        Value = !AParameters.Get("param_exclude_no_solicitations").ToBool()
                    },
                };

                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", Transaction, parameters);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                // TODO: we might need to add this functionality to TExtractsHandling.ExtractFromListOfPartnerKeys as well???
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return false;
                }

                TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

                TVerificationResultCollection VerificationResult;
                int NewExtractID;

                // create an extract with the given name in the parameters
                ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                    AParameters.Get("param_extract_name").ToString(),
                    AParameters.Get("param_extract_description").ToString(),
                    out NewExtractID,
                    out VerificationResult,
                    partnerkeys,
                    0);

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
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }
        }
    }
}