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

                decimal progress = counter / APaymentNumberList.Count * 100.0m;
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString(string.Format("Printing payment {0} ...", paymentNumber)),
                    progress);

                CreateFormDataInternal(ALedgerNumber, paymentNumber, AFormLetterFinanceInfo, AFormDataList, false);
            }

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }

        /// <summary>
        /// Create the Remittance Advice Form Data AND optionally the Cheque Data for Templater documents
        /// This method allows the client to print cheque counterfoil information that includes remittance info
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean CreateRemittanceAdviceAndChequeFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            ref AccountsPayableTDSAApPaymentTable APaymentTable,
            Int32 ALedgerNumber,
            bool AIncludeChequeFormData,
            out List <TFormData>AFormDataList)
        {
            AFormDataList = new List <TFormData>();
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Creating Remittance Advice"));
            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Starting ..."), 10.0m);

            // Print in payment number order
            APaymentTable.DefaultView.Sort = string.Format("{0} ASC", AccountsPayableTDSAApPaymentTable.GetPaymentNumberDBName());

            for (int i = 0; i < APaymentTable.DefaultView.Count; i++)
            {
                AccountsPayableTDSAApPaymentRow row = (AccountsPayableTDSAApPaymentRow)APaymentTable.DefaultView[i].Row;
                int paymentNumber = row.PaymentNumber;

                decimal progress = i / APaymentTable.DefaultView.Count * 100.0m;
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString(string.Format("Printing payment {0} ...", paymentNumber)),
                    progress);

                CreateFormDataInternal(ALedgerNumber, paymentNumber, AFormLetterFinanceInfo, AFormDataList,
                    AIncludeChequeFormData, row.ChequeNumber, row.ChequeAmountInWords);
            }

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }

        private static void CreateFormDataInternal(Int32 ALedgerNumber, int APaymentNumber,
            TFormLetterFinanceInfo AFormLetterFinanceInfo, List <TFormData>AFormDataList, bool AIncludeChequeFormData,
            int AChequeNumber = 0, string AChequeAmountInWords = "")
        {
            // Get all the details for this payment.  This creates a transaction.
            AccountsPayableTDS paymentDetails = TAPTransactionWebConnector.LoadAPPayment(ALedgerNumber, APaymentNumber);

            if (paymentDetails.PPartner.Rows.Count == 0) // unable to load this partner..
            {
                return;
            }

            if (paymentDetails.AApPayment.Rows.Count == 0) // unable to load this payment..
            {
                return;
            }

            if (AIncludeChequeFormData && paymentDetails.AApPayment[0].PrintCheque && (paymentDetails.AApPayment[0].Amount <= 0.0m))
            {
                // Cannot print cheques unless they are for a positive amount
                return;
            }

            TFormDataPartner formData = new TFormDataPartner();

            // Deal with the supplier part
            Int64 supplierKey = paymentDetails.PPartner[0].PartnerKey;

            TFormLettersWebConnector.FillFormDataFromPartner(supplierKey, formData, AFormLetterFinanceInfo);

            // Deal with the top-level details of the Payment in the ApPayment table.  This will only have one row
            //  but may contain information about multiple invoices (documents)
            formData.PaymentNumber = paymentDetails.AApPayment[0].PaymentNumber;

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
                    if ((documentPayment.ApDocumentId == documentRow.ApDocumentId)
                        && (documentPayment.PaymentNumber == paymentDetails.AApPayment[0].PaymentNumber))
                    {
                        invoiceDetails.PaymentAmount = documentPayment.Amount.ToString("N2");
                    }
                }

                formData.AddPayment(invoiceDetails);
            }

            formData.CurrencyCode = currencyCode;

            if (AIncludeChequeFormData && paymentDetails.AApPayment[0].PrintCheque)
            {
                formData.ChequeDate = DateTime.Today.ToString("dd MMM yyyy");
                formData.ChequeAmountToPay = paymentDetails.AApPayment[0].Amount.ToString("n2");
                formData.ChequeAmountInWords = AChequeAmountInWords;
                formData.ChequeNumber = AChequeNumber.ToString("D6");
            }

            AFormDataList.Add(formData);
        }
    }
}
