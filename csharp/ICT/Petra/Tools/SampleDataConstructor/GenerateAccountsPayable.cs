//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MFinance.AP.WebConnectors;

namespace Ict.Petra.Tools.SampleDataConstructor
{
    /// <summary>
    /// tools for generating and posting and paying invoices
    /// </summary>
    public class SampleDataAccountsPayable
    {
        /// LedgerNumber to be set from outside
        public static int FLedgerNumber = 43;

        /// <summary>
        /// generate the invoices from a text file that was generated with Benerator
        /// </summary>
        /// <param name="AInputBeneratorFile"></param>
        /// <param name="AYear">eg. 2013</param>
        public static void GenerateInvoices(string AInputBeneratorFile, int AYear)
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml(AInputBeneratorFile, ",");

            XmlNode RecordNode = doc.FirstChild.NextSibling.FirstChild;

            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            // get a list of potential suppliers
            string sqlGetSupplierPartnerKeys =
                "SELECT PUB_a_ap_supplier.p_partner_key_n, PUB_a_ap_supplier.a_currency_code_c " +
                "FROM PUB_p_organisation, PUB_a_ap_supplier WHERE PUB_a_ap_supplier.p_partner_key_n = PUB_p_organisation.p_partner_key_n";
            DataTable SupplierKeys = DBAccess.GDBAccessObj.SelectDT(sqlGetSupplierPartnerKeys, "keys", null);

            // get a list of potential expense account codes
            string sqlGetExpenseAccountCodes = "SELECT a_account_code_c FROM PUB_a_account WHERE a_ledger_number_i = " +
                                               FLedgerNumber.ToString() +
                                               " AND a_account_type_c = 'Expense' AND a_account_active_flag_l = true AND a_posting_status_l = true";
            DataTable AccountCodes = DBAccess.GDBAccessObj.SelectDT(sqlGetExpenseAccountCodes, "codes", null);

            while (RecordNode != null)
            {
                int supplierID = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "Supplier")) % SupplierKeys.Rows.Count;
                Int64 SupplierKey = Convert.ToInt64(SupplierKeys.Rows[supplierID].ItemArray[0]);
                String CurrencyCode = Convert.ToString(SupplierKeys.Rows[supplierID].ItemArray[1]);
                AApDocumentRow invoiceRow = MainDS.AApDocument.NewRowTyped(true);

                invoiceRow.LedgerNumber = FLedgerNumber;
                invoiceRow.ApDocumentId = (Int32)TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_ap_document);
                invoiceRow.ApNumber = invoiceRow.ApDocumentId;
                invoiceRow.DocumentCode = invoiceRow.ApDocumentId.ToString();
                invoiceRow.PartnerKey = SupplierKey;
                invoiceRow.Reference = "something";
                invoiceRow.DateIssued = Convert.ToDateTime(TXMLParser.GetAttribute(RecordNode, "DateIssued"));
                invoiceRow.DateIssued = new DateTime(AYear, invoiceRow.DateIssued.Month, invoiceRow.DateIssued.Day);
                invoiceRow.DateEntered = invoiceRow.DateIssued;
                invoiceRow.TotalAmount = Convert.ToDecimal(TXMLParser.GetAttribute(RecordNode, "Amount")) / 100.0m;
                invoiceRow.CurrencyCode = CurrencyCode;
                invoiceRow.ApAccount = "9100";
                invoiceRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
                invoiceRow.LastDetailNumber = 1;

                // TODO reasonable exchange rate for non base currency. need to check currency of supplier
                invoiceRow.ExchangeRateToBase = 1.0m;

                MainDS.AApDocument.Rows.Add(invoiceRow);

                AApDocumentDetailRow detailRow = MainDS.AApDocumentDetail.NewRowTyped(true);
                detailRow.ApDocumentId = invoiceRow.ApDocumentId;
                detailRow.DetailNumber = 1;
                detailRow.LedgerNumber = invoiceRow.LedgerNumber;
                detailRow.CostCentreCode = (FLedgerNumber * 100).ToString("0000");
                int accountID = Convert.ToInt32(TXMLParser.GetAttribute(RecordNode, "ExpenseAccount")) % AccountCodes.Rows.Count;
                detailRow.AccountCode = AccountCodes.Rows[accountID].ItemArray[0].ToString();
                detailRow.Amount = invoiceRow.TotalAmount;

                MainDS.AApDocumentDetail.Rows.Add(detailRow);

                RecordNode = RecordNode.NextSibling;
            }

            TVerificationResultCollection VerificationResult;
            AccountsPayableTDSAccess.SubmitChanges(MainDS, out VerificationResult);

            if (VerificationResult.HasCriticalOrNonCriticalErrors)
            {
                throw new Exception(VerificationResult.BuildVerificationResultString());
            }
        }

        /// <summary>
        /// post and pay all invoices in the given period, but leave some (or none) unposted
        /// </summary>
        public static bool PostAndPayInvoices(int AYear, int APeriod, int ALeaveInvoicesUnposted = 0)
        {
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            string sqlLoadDocuments =
                "SELECT * FROM PUB_a_ap_document WHERE a_ledger_number_i = ? AND a_date_issued_d >= ? AND a_date_issued_d <= ? AND a_document_status_c='APPROVED'";

            DateTime PeriodStartDate, PeriodEndDate;

            TFinancialYear.GetStartAndEndDateOfPeriod(FLedgerNumber, APeriod, out PeriodStartDate, out PeriodEndDate, null);

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter;
            parameter = new OdbcParameter("ledgernumber", OdbcType.Int);
            parameter.Value = FLedgerNumber;
            parameters.Add(parameter);
            parameter = new OdbcParameter("startDate", OdbcType.DateTime);
            parameter.Value = PeriodStartDate;
            parameters.Add(parameter);
            parameter = new OdbcParameter("endDate", OdbcType.DateTime);
            parameter.Value = PeriodEndDate;
            parameters.Add(parameter);

            DBAccess.GDBAccessObj.SelectDT(MainDS.AApDocument, sqlLoadDocuments, null, parameters.ToArray(), -1, -1);

            int countUnPosted = MainDS.AApDocument.Count;

            List <int>DocumentIdsToPost = new List <int>();

            foreach (AApDocumentRow invoice in MainDS.AApDocument.Rows)
            {
                if (countUnPosted <= ALeaveInvoicesUnposted)
                {
                    break;
                }

                DocumentIdsToPost.Add(invoice.ApDocumentId);

                countUnPosted--;
            }

            TVerificationResultCollection VerificationResult;

            if (!TAPTransactionWebConnector.PostAPDocuments(FLedgerNumber, DocumentIdsToPost, PeriodEndDate, false, out VerificationResult))
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                return false;
            }

            // TODO pay the invoices as well

            return true;
        }
    }
}