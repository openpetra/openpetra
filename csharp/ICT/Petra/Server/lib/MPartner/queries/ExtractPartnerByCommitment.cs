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

                List<String> param_grdCommitmentStatusChoices = new List<String>();
                foreach (TVariant choice in AParameters.Get("param_grdCommitmentStatusChoices").ToComposite())
                    param_grdCommitmentStatusChoices.Add(choice.ToString());

                OdbcParameter[] parameters = new OdbcParameter[]
                {
                    new OdbcParameter("param_dtpStartTimeFrom_unset", OdbcType.Bit) { Value = AParameters.Get("param_dtpStartDateFrom").IsZeroOrNull() },
                    new OdbcParameter("param_dtpStartTimeFrom", OdbcType.Date) { Value = AParameters.Get("param_dtpStartDateFrom").ToDate() },
                    new OdbcParameter("param_dtpStartTimeTo_unset", OdbcType.Bit) { Value = AParameters.Get("param_dtpStartDateTo").IsZeroOrNull() },
                    new OdbcParameter("param_dtpStartTimeTo", OdbcType.Date) { Value = AParameters.Get("param_dtpStartDateTo").ToDate() },
                    new OdbcParameter("param_dtpEndTimeFrom_unset", OdbcType.Bit) { Value = AParameters.Get("param_dtpEndDateFrom").IsZeroOrNull() },
                    new OdbcParameter("param_dtpEndTimeFrom", OdbcType.Date) { Value = AParameters.Get("param_dtpEndDateFrom").ToDate() },
                    new OdbcParameter("param_dtpEndTimeTo_unset", OdbcType.Bit) { Value = AParameters.Get("param_dtpEndDateTo").IsZeroOrNull() },
                    new OdbcParameter("param_dtpEndTimeTo", OdbcType.Date) { Value = AParameters.Get("param_dtpEndDateTo").ToDate() },
                    new OdbcParameter("param_dtpDateValidOn_unset", OdbcType.Bit) { Value = AParameters.Get("param_dtpDateValidOn").IsZeroOrNull() },
                    new OdbcParameter("param_dtpDateValidOn_a", OdbcType.Date) { Value = AParameters.Get("param_dtpDateValidOn").ToDate() },
                    new OdbcParameter("param_dtpDateValidOn_b", OdbcType.Date) { Value = AParameters.Get("param_dtpDateValidOn").ToDate() },

                    new OdbcParameter("param_txtFieldSending_unset", OdbcType.Bit) { Value = AParameters.Get("param_txtFieldSending").IsZeroOrNull() },
                    new OdbcParameter("param_txtFieldSending", OdbcType.Int) { Value = AParameters.Get("param_txtFieldSending").ToInt32() },
                    new OdbcParameter("param_txtFieldReceiving_unset", OdbcType.Bit) { Value = AParameters.Get("param_txtFieldReceiving").IsZeroOrNull() },
                    new OdbcParameter("param_txtFieldReceiving", OdbcType.Int) { Value = AParameters.Get("param_txtFieldReceiving").ToInt32() },

                    new OdbcParameter("param_chkCommitmentStatus_not", OdbcType.Bit) { Value = !AParameters.Get("param_chkCommitmentStatus").ToBool() },
                    TDbListParameterValue.OdbcListParameterValue("param_grdCommitmentStatusChoices",
                                                                 OdbcType.NChar,
                                                                 param_grdCommitmentStatusChoices),
                    new OdbcParameter("param_chkCommitmentStatusOthers", OdbcType.Bit) { Value = AParameters.Get("param_chkCommitmentStatusOthers").ToBool() },

                    new OdbcParameter("param_chkPartnerActive_not", OdbcType.Bit) { Value = !AParameters.Get("param_chkPartnerActive").ToBool() },
/*                    new OdbcParameter("param_chkMailable_not", OdbcType.Bit) { Value = !AParameters.Get("param_chkMailable").ToBool() },*/
                    new OdbcParameter("param_chkRespectNoSolicitors_not", OdbcType.Bit) { Value = !AParameters.Get("param_chkRespectNoSolicitors").ToBool() },
                };

                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", Transaction, parameters);

                if (NewTransaction)
                    DBAccess.GDBAccessObj.RollbackTransaction();

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                // TODO: we might need to add this functionality to TExtractsHandling.ExtractFromListOfPartnerKeys as well???
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                    return false;

                TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

                TVerificationResultCollection VerificationResult;
                int NewExtractID;

                // create an extract with the given name in the parameters
                ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                    AParameters.Get("param_txtExtractName").ToString(),
                    AParameters.Get("param_txtExtractDescription").ToString(),
                    out NewExtractID,
                    out VerificationResult,
                    partnerkeys,
                    0);

                if (ReturnValue)
                    DBAccess.GDBAccessObj.CommitTransaction();
                else
                    DBAccess.GDBAccessObj.RollbackTransaction();

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
