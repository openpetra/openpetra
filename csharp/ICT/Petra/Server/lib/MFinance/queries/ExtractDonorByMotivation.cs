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
    public class QueryDonorByMotivation : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all donors with gifts for selected motivation details
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Gift.Queries.ExtractDonorByMotivation.sql");

            // create a new object of this class and control extract calculation from base class
            QueryDonorByMotivation ExtractQuery = new QueryDonorByMotivation();

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
            bool ReceiptLetterFrequencySet;
            string ReceiptLetterFrequency;
            bool MailingCodeSet;
            string MailingCode;


            // add ledger number
            ASQLParameterList.Add(new OdbcParameter("param_ledger_number_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_ledger_number").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_ledger_number", OdbcType.Int)
                {
                    Value = AParameters.Get("param_ledger_number").ToInt()
                });

            // add mailing code
            if (AParameters.Get("param_mailing_code").IsZeroOrNull()
                || (AParameters.Get("param_mailing_code").ToString() == ""))
            {
                MailingCodeSet = false;
                MailingCode = "";
            }
            else
            {
                MailingCodeSet = true;
                MailingCode = AParameters.Get("param_mailing_code").ToString();
            }

            ASQLParameterList.Add(new OdbcParameter("param_mailing_code_unset", OdbcType.Bit)
                {
                    Value = !MailingCodeSet
                });
            ASQLParameterList.Add(new OdbcParameter("param_mailing_code", OdbcType.VarChar)
                {
                    Value = MailingCode
                });

            // Motivation Group and Detail Code in Pairs. First value is group code, second is detail code.
            List <String>param_motivation_detail = new List <String>(AParameters.Get("param_motivation_detail").ToString().Split(','));

            int Index = 0;
            String Group_Detail_Pairs = "";

            foreach (String KeyPart in param_motivation_detail)
            {
                if (Index % 2 == 0)
                {
                    if (Group_Detail_Pairs.Length > 0)
                    {
                        Group_Detail_Pairs += ",";
                    }

                    // even Index: Group Code
                    Group_Detail_Pairs += "('" + KeyPart + "','";
                }
                else
                {
                    // odd Index: Detail Code
                    Group_Detail_Pairs += KeyPart + "')";
                }

                // increase Index for next element
                Index += 1;
            }

            ASqlStmt = ASqlStmt.Replace("##motivation_group_detail_pairs##", Group_Detail_Pairs);

            // Date from and to
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

            // New Donors only
            ASQLParameterList.Add(new OdbcParameter("param_new_donors_only", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_new_donors_only").ToBool()
                });

            // Receipt each gift
            ASQLParameterList.Add(new OdbcParameter("param_receipt_each_gift_only", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_receipt_each_gift_only").ToBool()
                });

            // Active Partners only, Families only and Exclude No Solicitations
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

            // Receipt Letter Frequency
            if (AParameters.Get("param_receipt_letter_frequency").IsZeroOrNull()
                || (AParameters.Get("param_receipt_letter_frequency").ToString() == ""))
            {
                ReceiptLetterFrequencySet = false;
                ReceiptLetterFrequency = "";
            }
            else
            {
                ReceiptLetterFrequencySet = true;
                ReceiptLetterFrequency = AParameters.Get("param_receipt_letter_frequency").ToString();
            }

            ASQLParameterList.Add(new OdbcParameter("param_receipt_letter_frequency_set", OdbcType.Bit)
                {
                    Value = ReceiptLetterFrequencySet
                });
            ASQLParameterList.Add(new OdbcParameter("param_receipt_letter_frequency", OdbcType.VarChar)
                {
                    Value = ReceiptLetterFrequency
                });
        }
    }
}