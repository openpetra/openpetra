//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MFinance.AP.WebConnectors
{
    /// <summary>
    /// Web connector for dealing with AP Remittances
    /// </summary>
    public class TRemittanceWebConnector
    {
        /// <summary>
        /// create the Remittance Advice Form Data for Templater documents
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean CreateRemittanceAdviceFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            List <int>APaymentNumberList,
            Int32 ALedgerNumber,
            out List <TFormData>AFormDataList)
        {
            AFormDataList = new List <TFormData>();
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Creating Remittance Advice"));
            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Starting ..."), 10.0m);

            int counter = 0;

            foreach (int paymentNumber in APaymentNumberList)
            {
                counter++;

                // Get the details for this payment
                AccountsPayableTDS paymentDetails = TAPTransactionWebConnector.LoadAPPayment(ALedgerNumber, paymentNumber);

                if (paymentDetails.PPartner.Rows.Count == 0) // unable to load this partner..
                {
                    continue;
                }

                if (paymentDetails.AApPayment.Rows.Count == 0) // unable to load this payment..
                {
                    continue;
                }

                decimal progress = counter / APaymentNumberList.Count * 100.0m;
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString(string.Format("Printing payment {0} ...", paymentNumber)),
                    progress);

                TFormDataPartner formData = new TFormDataPartner();
                TDBTransaction ReadTransaction = null;

                DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                    delegate
                    {
                        // Deal with the supplier part
                        Int64 supplierKey = paymentDetails.PPartner[0].PartnerKey;

                        TFormLettersWebConnector.FillFormDataFromPartner(supplierKey, formData, AFormLetterFinanceInfo);

                        // Deal with the top-level details of the Payment in the ApPayment table.  This will only have one row
                        //  but may contain information about multiple invoices (documents)
                        formData.PaymentNumber = paymentNumber;

                        formData.PaymentDate = paymentDetails.AApPayment[0].PaymentDate.HasValue ?
                                               paymentDetails.AApPayment[0].PaymentDate.Value.ToString("dd MMMM yyyy") : string.Empty;
                        formData.TotalPayment = paymentDetails.AApPayment[0].Amount.ToString(
                            "N2");

                        formData.OurReference =
                            (paymentDetails.AApSupplier.Rows.Count > 0) ? paymentDetails.AApSupplier[0].OurReference : string.Empty;

                        string currencyCode = string.Empty;

                        // A payment may be made up of multiple invoices
                        foreach (AApDocumentRow documentRow in paymentDetails.AApDocument.Rows)
                        {
                            TFormDataAPPayment invoiceDetails = new TFormDataAPPayment();
                            invoiceDetails.InvoiceNumber = documentRow.DocumentCode;
                            invoiceDetails.InvoiceDate = documentRow.DateIssued.ToString("dd MMMM yyyy");
                            invoiceDetails.InvoiceAmount = documentRow.TotalAmount.ToString(
                                "N2");
                            invoiceDetails.InvoiceCurrencyCode =
                                (paymentDetails.AApSupplier.Rows.Count > 0) ? paymentDetails.AApSupplier[0].CurrencyCode : string.Empty;
                            currencyCode = invoiceDetails.InvoiceCurrencyCode;

                            invoiceDetails.PaymentAmount = string.Empty;

                            // An invoice in this payment may only be part-paid, so how much of the invoice amount was paid in this payment??
                            foreach (AApDocumentPaymentRow documentPayment in paymentDetails.AApDocumentPayment.Rows)
                            {
                                if ((documentPayment.ApDocumentId == documentRow.ApDocumentId) && (documentPayment.PaymentNumber == paymentNumber))
                                {
                                    invoiceDetails.PaymentAmount = documentPayment.Amount.ToString("N2");
                                }
                            }

                            formData.AddPayment(invoiceDetails);
                        }

                        formData.CurrencyCode = currencyCode;
                    });

                AFormDataList.Add(formData);
            }

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }

        /// <summary>
        /// create the Cheque Printing Form Data for Templater documents
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean CreateChequeFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            List <int>APaymentNumberList,
            Int32 ALedgerNumber,
            out List <TFormData>AFormDataList)
        {
            AFormDataList = new List <TFormData>();
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Creating Cheque"));
            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Starting ..."), 10.0m);

            int counter = 0;

            foreach (int paymentNumber in APaymentNumberList)
            {
                counter++;

                AccountsPayableTDS paymentDetails = TAPTransactionWebConnector.LoadAPPayment(ALedgerNumber, paymentNumber);

                if (paymentDetails.PPartner.Rows.Count == 0) // unable to load this partner..
                {
                    continue;
                }

                if (paymentDetails.AApPayment.Rows.Count == 0) // unable to load this payment..
                {
                    continue;
                }

                decimal progress = counter / APaymentNumberList.Count * 100.0m;
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString(string.Format("Printing cheque for payment number {0} ...", paymentNumber)),
                    progress);

                TFormDataPartner formData = new TFormDataPartner();
                TDBTransaction ReadTransaction = null;

                DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                    delegate
                    {
                        Int64 supplierKey = paymentDetails.PPartner[0].PartnerKey;

                        TFormLettersWebConnector.FillFormDataFromPartner(supplierKey, formData, AFormLetterFinanceInfo);

                        formData.ChequeDate = DateTime.Today.ToString("dd MMM yyyy");
                        formData.ChequeAmountInWords = paymentDetails.AApPayment[0].ChequeAmountInWords;
                        formData.ChequeAmountToPay = paymentDetails.AApPayment[0].Amount.ToString("n2");
                        formData.ChequeNumber = paymentDetails.AApPayment[0].ChequeNumber.ToString("D6");
                    });

                AFormDataList.Add(formData);
            }

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }
    }
}
