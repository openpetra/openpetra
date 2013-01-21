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
    public class QueryDonorByMiscellaneous : Ict.Petra.Server.MCommon.queries.ExtractQueryBase
    {
        /// <summary>
        /// calculate an extract from a report: all donors that have given to particular fields (ledgers)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AResults"></param>
        /// <returns></returns>
        public static bool CalculateExtract(TParameterList AParameters, TResultList AResults)
        {
            string SqlStmt = TDataBase.ReadSqlFile("Gift.Queries.ExtractDonorByMiscellaneous.sql");

            // create a new object of this class and control extract calculation from base class
            QueryDonorByMiscellaneous ExtractQuery = new QueryDonorByMiscellaneous();

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

            // add recipient key
            ASQLParameterList.Add(new OdbcParameter("param_recipient_key_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_recipient_key").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_recipient_key", OdbcType.BigInt)
                {
                    Value = AParameters.Get("param_recipient_key").ToInt64()
                });

            // add mailing code
            if (AParameters.Get("param_mailing_code").IsZeroOrNull()
                || (AParameters.Get("param_mailing_code").ToString() == ""))
            {
                MailingCodeSet = false;
                MailingCode = "";
                ASqlStmt = ASqlStmt.Replace("##equals_or_like_mailing_code##", "=");
            }
            else
            {
                MailingCodeSet = true;
                MailingCode = AParameters.Get("param_mailing_code").ToString();
                MailingCode.Replace('*', '%');

                if (MailingCode.Contains("%"))
                {
                    ASqlStmt = ASqlStmt.Replace("##equals_or_like_mailing_code##", "LIKE");
                }
                else
                {
                    ASqlStmt = ASqlStmt.Replace("##equals_or_like_mailing_code##", "=");
                }
            }

            ASQLParameterList.Add(new OdbcParameter("param_mailing_code_unset", OdbcType.Bit)
                {
                    Value = !MailingCodeSet
                });
            ASQLParameterList.Add(new OdbcParameter("param_mailing_code", OdbcType.VarChar)
                {
                    Value = MailingCode
                });

            // Exclude Motivation Detail 'No Receipt'
            ASQLParameterList.Add(new OdbcParameter("param_exclude_mot_detail_no_receipt", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_exclude_mot_detail_no_receipt").ToBool()
                });

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

            // add Method of Giving
            ASQLParameterList.Add(new OdbcParameter("param_method_of_giving_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_method_of_giving").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_method_of_giving", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_method_of_giving").ToString()
                });

            // add Method of Payment
            ASQLParameterList.Add(new OdbcParameter("param_method_of_payment_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_method_of_payment").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_method_of_payment", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_method_of_payment").ToString()
                });

            // add Reference
            ASQLParameterList.Add(new OdbcParameter("param_reference_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_reference").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_reference", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_reference").ToString()
                });

            // TODO: receipt letter code currently not available --> therefore leave this code snippet empty
            // add receipt letter code
            //ASQLParameterList.Add(new OdbcParameter("param_receipt_letter_code_unset", OdbcType.Bit)
            //    {
            //        Value = AParameters.Get("param_receipt_letter_code").IsZeroOrNull()
            //    });
            //ASQLParameterList.Add(new OdbcParameter("param_receipt_letter_code", OdbcType.VarChar)
            //    {
            //        Value = AParameters.Get("param_receipt_letter_code").ToString()
            //    });
            ASqlStmt = ASqlStmt.Replace("##receipt_letter_code_snippet##", "");
            //ASqlStmt = ASqlStmt.Replace("##receipt_letter_code_snippet##",
            //                            "AND (? OR pub_a_gift.a_receipt_letter_code_c ##equals_or_like_receipt_letter_code## ?)");
            //ASqlStmt = ASqlStmt.Replace("##equals_or_like_receipt_letter_code##", "LIKE");

            // add Gift type
            ASQLParameterList.Add(new OdbcParameter("param_gift_type_unset", OdbcType.Bit)
                {
                    Value = AParameters.Get("param_gift_type").IsZeroOrNull()
                });
            ASQLParameterList.Add(new OdbcParameter("param_gift_type", OdbcType.VarChar)
                {
                    Value = AParameters.Get("param_gift_type").ToString()
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