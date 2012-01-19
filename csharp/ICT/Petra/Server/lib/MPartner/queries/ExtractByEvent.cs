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
    public class QueryPartnerByEvent
    {
        /// <summary>
        /// calculate an extract from a report: all partners living in a given city
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

                string SqlStmt = TDataBase.ReadSqlFile("Partner.Queries.ExtractPartnerByEvent.sql");

                
                
                
                
                
                
                
                int Index = 0;
                int SizeOfArray;
                String ValueList;
                Int64 ListValueInt;
                String ListValue;
                String ParameterList = "";
                             
                // set array to correct size depending on number of specialtypes selected
                ValueList = AParameters.Get("param_event").ToString();

                SizeOfArray = StringHelper.CountOccurencesOfChar(ValueList,',') + 1;                

                TLogging.Log("SizeOfArray is: " + SizeOfArray);
                
                OdbcParameter[] parameters = new OdbcParameter[SizeOfArray + 7];
                TLogging.Log("parameters[] created...");
                // this *should* always have at least one selection because the client requires it

                ListValue = StringHelper.GetNextCSV(ref ValueList, ",");
                if(ListValue.Length == 0) throw new NoNullAllowedException("At least one option must be checked."); // safety
                Index = 0;
                while (ListValue != "")
                {
                    ListValueInt = Convert.ToInt64(ListValue);
                    parameters[Index] = new OdbcParameter("Event" + Index.ToString(), OdbcType.BigInt);

                    parameters[Index].Value = ListValueInt;
                    Index++;
                    if (ParameterList.Length == 0)

                    {
                        ParameterList = "?";
                    }
                    else
                    {
                        ParameterList = ParameterList + ",?";
                    }
                    
                    ListValue = StringHelper.GetNextCSV(ref ValueList, ",");
                }
                

                SqlStmt = SqlStmt.Replace("##ParameterList##", ParameterList);
                
                // reading in parameters and getting them ready to be inserted into the query
                parameters[Index] = new OdbcParameter("Accepted", OdbcType.Bit);
                parameters[Index].Value = AParameters.Get("param_status_Accepted").ToBool();
                parameters[Index + 1] = new OdbcParameter("Hold", OdbcType.Bit);
                parameters[Index + 1].Value = AParameters.Get("param_status_Hold").ToBool();
                parameters[Index + 2] = new OdbcParameter("Enquiry", OdbcType.Bit);
                parameters[Index + 2].Value = AParameters.Get("param_status_Enquiry").ToBool();
                parameters[Index + 3] = new OdbcParameter("cancelled", OdbcType.Bit);
                parameters[Index + 3].Value = AParameters.Get("param_status_cancelled").ToBool();
                parameters[Index + 4] = new OdbcParameter("Rejected", OdbcType.Bit);;
                parameters[Index + 4].Value = AParameters.Get("param_status_Rejected").ToBool();
                parameters[Index + 5] = new OdbcParameter("cancelled", OdbcType.Bit);
                parameters[Index + 5].Value = AParameters.Get("param_Active_Parteners").ToBool();
                parameters[Index + 6] = new OdbcParameter("Exclude_No_Soliotions", OdbcType.Bit);
                parameters[Index + 6].Value = AParameters.Get("param_Exclude_No_Soliotions").ToBool();
                //parameters[Index + 7] =  new OdbcParameter("Mailing_Addresses_Only", OdbcType.Bit)
                //parameters[Index + 7].Value = AParameters.Get("param_Mailing_Addresses_Only").ToBool();
                
                
                
                //OdbcParameter[] parameters = new OdbcParameter[8];
                //parameters[0] = new OdbcParameter("Event", OdbcType.BigInt);
                parameters[0].Value = AParameters.Get("param_event").ToInt64();
               // parameters[1] = new OdbcParameter("Accepted", OdbcType.Bit);
                //parameters[1].Value = AParameters.Get("param_status_Accepted").ToBool();
                //parameters[2] = new OdbcParameter("Hold", OdbcType.Bit);
                //parameters[2].Value = AParameters.Get("param_status_Hold").ToBool();
               // parameters[3] = new OdbcParameter("Enquiry", OdbcType.Bit);
                //parameters[3].Value = AParameters.Get("param_status_Enquiry").ToBool();
                //parameters[4] = new OdbcParameter("cancelled", OdbcType.Bit);
                //parameters[4].Value = AParameters.Get("param_status_cancelled").ToBool();
                //parameters[5] = new OdbcParameter("Rejected", OdbcType.Bit);
                //parameters[5].Value = AParameters.Get("param_status_Rejected").ToBool(); 
                //parameters[6] = new OdbcParameter("Active_Parteners", OdbcType.Bit);
                //parameters[6].Value = AParameters.Get("param_Active_Parteners").ToBool();
                //parameters[7] = new OdbcParameter("Exclude_No_Soliotions", OdbcType.Bit);
                //parameters[7].Value = AParameters.Get("param_Exclude_No_Soliotions").ToBool();
             // parameters[8] = new OdbcParameter("Mailing_Addresses_Only", OdbcType.Bit);
             // parameters[8].Value = AParameters.Get("param_Mailing_Addresses_Only");
                
                TLogging.Log("getting the data from the database", TLoggingType.ToStatusBar);
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