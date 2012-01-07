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
    /// this report is queries the database for partners that are subscribed to certain publications and also filters the data by other criteria
    /// </summary>
    public class QueryPartnerBySubscription
    {
        /// <summary>
        /// calculate an extract from a report: all partners that are subscribed to certain publications and other criteria
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            // get the partner keys from the database
            try
            {
	            Boolean ReturnValue = false;
                Boolean NewTransaction;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerBySubscription.sql");

//SqlStmt = "SELECT DISTINCT pub_p_partner.p_partner_key_n, pub_p_partner.p_partner_short_name_c FROM pub_p_partner, pub_p_subscription"
//+ " WHERE pub_p_subscription.p_publication_code_c IN ('INDEED','NB-WIM')"
//+ " AND pub_p_partner.p_partner_key_n = pub_p_subscription.p_partner_key_n"
//+ " ORDER BY pub_p_partner.p_partner_short_name_c";
       	
                OdbcParameter[] parameters = new OdbcParameter[1];
                parameters[0] = new OdbcParameter("publication", OdbcType.VarChar);
                parameters[0].Value = AParameters.Get("param_explicit_publication");

                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", Transaction, parameters);
//DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", Transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                // TODO: we might need to add this functionality to TExtractsHandling.CreateExtractFromListOfPartnerKeys as well???
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return false;
                }

                TLogging.Log("preparing the extract", TLoggingType.ToStatusBar);

                TVerificationResultCollection VerificationResult;
                int NewExtractID;

                // create an extract with the given name in the parameters
                ReturnValue = TExtractsHandling.CreateExtractFromListOfPartnerKeys(
                    AParameters.Get("nameOfExtract").ToString(),
                    AParameters.Get("descriptionOfExtract").ToString(),
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