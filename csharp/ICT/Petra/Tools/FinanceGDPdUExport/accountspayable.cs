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
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportAccountsPayable
    {
        /// <summary>
        /// export all posted invoices of other suppliers in this year
        /// </summary>
        public static void Export(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            Int32 AFinancialYear,
            string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "accountspayable.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // get all posted or paid ap_documents by their date issued
            string sql =
                String.Format(
                    "SELECT * FROM PUB_{0} " +
                    "WHERE {1} = {2} AND " +
                    "({3} = '{4}' OR {3} = '{5}' OR {3} = '{6}') AND " +
                    "{7} >= ? AND {7} <= ?",
                    AApDocumentTable.GetTableDBName(),
                    AApDocumentTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AApDocumentTable.GetDocumentStatusDBName(),
                    MFinanceConstants.AP_DOCUMENT_POSTED,
                    MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID,
                    MFinanceConstants.AP_DOCUMENT_PAID,
                    AApDocumentTable.GetDateIssuedDBName());

            List <OdbcParameter>Parameters = new List <OdbcParameter>();
            OdbcParameter param = new OdbcParameter("startdate", OdbcType.DateTime);
            param.Value = TAccountingPeriodsWebConnector.GetPeriodStartDate(ALedgerNumber, AFinancialYear, 0, 1);
            Parameters.Add(param);
            param = new OdbcParameter("enddate", OdbcType.DateTime);
            param.Value = TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber, AFinancialYear, 0, 12);
            Parameters.Add(param);

            AApDocumentTable apDocuments = new AApDocumentTable();
            DBAccess.GDBAccessObj.SelectDT(apDocuments, sql, Transaction, Parameters.ToArray(), 0, 0);

            // get all ap details
            sql =
                String.Format(
                    "SELECT Detail.* FROM PUB_{0} AS Doc, PUB_{8} AS Detail " +
                    "WHERE Doc.{1} = {2} AND " +
                    "({3} = '{4}' OR {3} = '{5}' OR {3} = '{6}') AND " +
                    "{7} >= ? AND {7} <= ? AND " +
                    "Detail.{1} = Doc.{1} AND Detail.{9} = Doc.{9} AND " +
                    "Detail.{10} IN ({11})",
                    AApDocumentTable.GetTableDBName(),
                    AApDocumentTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AApDocumentTable.GetDocumentStatusDBName(),
                    MFinanceConstants.AP_DOCUMENT_POSTED,
                    MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID,
                    MFinanceConstants.AP_DOCUMENT_PAID,
                    AApDocumentTable.GetDateIssuedDBName(),
                    AApDocumentDetailTable.GetTableDBName(),
                    AApDocumentTable.GetApDocumentIdDBName(),
                    AApDocumentDetailTable.GetCostCentreCodeDBName(),
                    "'" + ACostCentres.Replace(",", "','") + "'");

            AApDocumentDetailTable apDetails = new AApDocumentDetailTable();
            DBAccess.GDBAccessObj.SelectDT(apDetails, sql, Transaction, Parameters.ToArray(), 0, 0);

            apDetails.DefaultView.Sort = AApDocumentDetailTable.GetApDocumentIdDBName();

            // get all ap payments
            sql =
                String.Format(
                    "SELECT DP.{0}, DP.{1}, P.{2} AS {3}, DP.{7}, DP.{15} FROM PUB_{4} AS Doc, PUB_{5} AS DP, PUB_{6} AS P " +
                    "WHERE Doc.{7} = {8} AND " +
                    "({9} = '{10}' OR {9} = '{11}' OR {9} = '{12}') AND " +
                    "{13} >= ? AND {13} <= ? AND " +
                    "DP.{7} = Doc.{7} AND DP.{14} = Doc.{14} AND " +
                    "P.{7} = Doc.{7} AND P.{15} = DP.{15}",

                    AApDocumentPaymentTable.GetApDocumentIdDBName(),
                    AApDocumentPaymentTable.GetAmountDBName(),
                    AApPaymentTable.GetPaymentDateDBName(),
                    AApDocumentPaymentTable.GetDateCreatedDBName(),
                    AApDocumentTable.GetTableDBName(),
                    AApDocumentPaymentTable.GetTableDBName(),
                    AApPaymentTable.GetTableDBName(),

                    AApDocumentTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AApDocumentTable.GetDocumentStatusDBName(),
                    MFinanceConstants.AP_DOCUMENT_POSTED,
                    MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID,
                    MFinanceConstants.AP_DOCUMENT_PAID,
                    AApDocumentTable.GetDateIssuedDBName(),

                    AApDocumentTable.GetApDocumentIdDBName(),
                    AApPaymentTable.GetPaymentNumberDBName());

            AApDocumentPaymentTable apPayments = new AApDocumentPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(apPayments, sql, Transaction, Parameters.ToArray(), 0, 0);

            apPayments.DefaultView.Sort = AApDocumentPaymentTable.GetApDocumentIdDBName();

            // get the analysis attributes for the taxes
            sql =
                String.Format(
                    "SELECT Attrib.* FROM PUB_{0} AS Doc, PUB_{8} AS Attrib " +
                    "WHERE Doc.{1} = {2} AND " +
                    "({3} = '{4}' OR {3} = '{5}' OR {3} = '{6}') AND " +
                    "{7} >= ? AND {7} <= ? AND " +
                    "Attrib.{1} = Doc.{1} AND Attrib.{9} = Doc.{9}",
                    AApDocumentTable.GetTableDBName(),
                    AApDocumentTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AApDocumentTable.GetDocumentStatusDBName(),
                    MFinanceConstants.AP_DOCUMENT_POSTED,
                    MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID,
                    MFinanceConstants.AP_DOCUMENT_PAID,
                    AApDocumentTable.GetDateIssuedDBName(),
                    AApAnalAttribTable.GetTableDBName(),
                    AApDocumentTable.GetApDocumentIdDBName());

            AApAnalAttribTable apAttrib = new AApAnalAttribTable();
            DBAccess.GDBAccessObj.SelectDT(apAttrib, sql, Transaction, Parameters.ToArray(), 0, 0);

            apAttrib.DefaultView.Sort = AApAnalAttribTable.GetApDocumentIdDBName() + "," + AApAnalAttribTable.GetDetailNumberDBName();

            foreach (AApDocumentRow doc in apDocuments.Rows)
            {
                DataRowView[] detailsRV = apDetails.DefaultView.FindRows(doc.ApDocumentId);

                foreach (DataRowView rv in detailsRV)
                {
                    AApDocumentDetailRow detail = (AApDocumentDetailRow)rv.Row;

                    if (doc.CreditNoteFlag)
                    {
                        detail.Amount *= -1.0m;
                    }

                    StringBuilder attributes = new StringBuilder();

                    DataRowView[] attribs = apAttrib.DefaultView.FindRows(new object[] { doc.ApDocumentId, detail.DetailNumber });

                    decimal TaxOnExpense = 0.0m;

                    foreach (DataRowView rvAttrib in attribs)
                    {
                        AApAnalAttribRow attribRow = (AApAnalAttribRow)rvAttrib.Row;

                        attributes.Append(attribRow.AnalysisAttributeValue);

                        if (attribRow.AnalysisAttributeValue == "v19")
                        {
                            TaxOnExpense = detail.Amount * 0.19m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "v7")
                        {
                            TaxOnExpense = detail.Amount * 0.07m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "70v7")
                        {
                            TaxOnExpense = detail.Amount * 0.7m * 0.07m;
                        }
                        else if (attribRow.AnalysisAttributeValue == "70v19")
                        {
                            TaxOnExpense = detail.Amount * 0.7m * 0.19m;
                        }
                    }

                    DataRowView[] payments = apPayments.DefaultView.FindRows(doc.ApDocumentId);

                    string DatePaid = string.Empty;

                    if (payments.Length > 1)
                    {
                        DatePaid = "Several Payments: ";

                        foreach (DataRowView rvPayment in payments)
                        {
                            AApDocumentPaymentRow payment = ((AApDocumentPaymentRow)rvPayment.Row);
                            DatePaid += payment.DateCreated.Value.ToString("yyyyMMdd") + " ";
                            DatePaid += payment.PaymentNumber.ToString() + "; ";
                        }

                        // for gdpdu, only write dates to this column
                        DatePaid = string.Empty;
                    }
                    else if (payments.Length == 1)
                    {
                        AApDocumentPaymentRow payment = ((AApDocumentPaymentRow)payments[0].Row);
                        DatePaid = payment.DateCreated.Value.ToString("yyyyMMdd");
                    }

                    sb.Append(StringHelper.StrMerge(
                            new string[] {
                                doc.ApNumber.ToString(),
                                detail.DetailNumber.ToString(),
                                doc.DateIssued.ToString("yyyyMMdd"),
                                DatePaid,
                                doc.PartnerKey.ToString(),
                                detail.CostCentreCode,
                                detail.AccountCode,
                                String.Format("{0:N}", detail.Amount),
                                detail.Narrative,
                                attributes.ToString(),
                                String.Format("{0:N}", TaxOnExpense)
                            }, ACSVSeparator));
                    sb.Append(ANewLine);
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}