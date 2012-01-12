//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
    /// this report is quite simple, and should be used as an example for more complex reports and extracts
    /// </summary>
    public class QueryPartnerBySpecialType
    {
        /// <summary>
        /// calculate an extract from a report: all partners of a given type
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            // get the partner keys from the database
            try
            {
                TLogging.Log("Calculate PartnerBySpecialTypeExtract....!!!ARW!!!"); // ARW
                TLogging.Log( "ARW LOG: Name of the SPECIAL TYPE Extract: " + AParameters.Get("nameOfExtract"));
                TLogging.Log( "ARW LOG: Description of the SPECIAL TYPE Extract: " + AParameters.Get("descriptionOfExtract"));
                TLogging.Log( "ARW LOG: Parameters received: \n\t" + 
                             AParameters.Get("param_city") + "\n\t" +
                             AParameters.Get("param_from") + "\n\t" +
                             AParameters.Get("param_to") + "\n\t" +
                             AParameters.Get("param_region") + "\n\t" +                    
                             AParameters.Get("param_country") + "\n\t" +
                             AParameters.Get("param_dateSet") + "\n\t" +
                             AParameters.Get("param_active") + "\n\t" +
                             AParameters.Get("param_mailingAddressesOnly") + "\n\t" +                             
                             AParameters.Get("param_familiesOnly") + "\n\t" +
                             AParameters.Get("param_excludeNoSolicitations") + "\n\t" +
                             AParameters.Get("param_explicit_specialtypes"));
                Boolean ReturnValue = false;
                Boolean NewTransaction;
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);
                string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractByPartnerSpecialType.sql");
                // reading in parameters and getting them ready to be inserted into the query
                // currently, query is for extract by city
                OdbcParameter[] parameters = new OdbcParameter[4];
                parameters[0] = new OdbcParameter("param_explicit_specialtypes", OdbcType.VarChar);
                parameters[0].Value = AParameters.Get("param_explicit_specialtypes").ToBool();
//                parameters[1] = new OdbcParameter("param_dateFieldsIncluded", OdbcType.Bit); // field dates included?
//                parameters[1].Value = false;
//                parameters[1] = new OdbcParameter("Date", OdbcType.Date);
//                parameters[1].Value = AParameters.Get("param_dateSet").ToDate();
                parameters[1] =  new OdbcParameter("param_active", OdbcType.Bit);
                parameters[1].Value = AParameters.Get("param_active").ToBool();
                parameters[2] = new OdbcParameter("param_familiesOnly", OdbcType.Bit);
                parameters[2].Value = AParameters.Get("param_familiesOnly").ToBool();
                parameters[3] = new OdbcParameter("param_excludeNoSolicitations", OdbcType.Bit);
                parameters[3].Value = AParameters.Get("param_excludeNoSolicitations").ToBool();
                
                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
                TLogging.Log("getting the data from the database");
                DataTable partnerkeys = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "partners", Transaction, parameters);

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