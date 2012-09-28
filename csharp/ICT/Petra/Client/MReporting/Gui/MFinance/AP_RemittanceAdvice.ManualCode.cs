//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;
using System.Windows.Forms;
using Ict.Common.Printing;
using Ict.Petra.Shared.MFinance.AP.Data;
using System.IO;
using Ict.Petra.Shared.MPartner;
using System.Threading;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAP_RemittanceAdvice
    {
        private Int32 FLedgerNumber;
        private Int32 FNumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;
        private String FHtmlDoc = "";

        private void RunOnceOnActivationManual()
        {
            btnPDF.Visible = false;
            btnCopy.Visible = false;
        }

        /// <summary>
        /// The main screen will call this on creation, to give me my Ledger Number.
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="APaymentNum"></param>
        /// <param name="ALedgerNumber"></param>
        public void PrintRemittanceAdvice(Int32 APaymentNum, Int32 ALedgerNumber)
        {
            txtPaymentNum.NumberValueInt = APaymentNum;
            AccountsPayableTDS PaymentDetails = TRemote.MFinance.AP.WebConnectors.LoadAPPayment(ALedgerNumber, APaymentNum);

            if (PaymentDetails.AApPayment.Rows.Count == 0) // unable to load this payment..
            {
                lblLoadStatus.Text = String.Format(Catalog.GetString("Error - can't load Payment number {0}."), APaymentNum);
                return;
            }

            SortedList <string, List <string>>FormValues = new SortedList <string, List <string>>();

            //
            // load my own country code, so I don't print it on letters to my own country.
            //
            string LocalCountryCode = TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetCountryCodeFromSiteLedger();

            //
            // These are the fields that I will pull out of the TDS...
            //
            FormValues.Add("SupplierName", new List <string>());
            FormValues.Add("SupplierAddress", new List <string>());
            FormValues.Add("PaymentDate", new List <string>());
            FormValues.Add("OurReference", new List <string>());
            FormValues.Add("InvoiceDate", new List <string>());
            FormValues.Add("InvoiceNumber", new List <string>());
            FormValues.Add("InvoiceAmount", new List <string>());
            FormValues.Add("TotalPayment", new List <string>());

            FormValues["SupplierName"].Add(PaymentDetails.PPartner[0].PartnerShortName);

            if (PaymentDetails.PLocation[0].CountryCode == LocalCountryCode)
            {
                PaymentDetails.PLocation[0].CountryCode = ""; // Don't print country code it it's the same as my own.
            }

            FormValues["SupplierAddress"].Add(Calculations.DetermineLocationString(PaymentDetails.PLocation[0],
                    Calculations.TPartnerLocationFormatEnum.plfHtmlLineBreak));

            String DatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern;
            DatePattern = "dd MMMM yyyy"; // The long pattern above is no good in UK, although it might be OK in other cultures...

            FormValues["PaymentDate"].Add(PaymentDetails.AApPayment[0].PaymentDate.Value.ToString(DatePattern));
            FormValues["OurReference"].Add(PaymentDetails.AApSupplier[0].OurReference);

            foreach (AApDocumentRow Row in PaymentDetails.AApDocument.Rows)
            {
                FormValues["InvoiceDate"].Add(Row.DateIssued.ToString(DatePattern));
                FormValues["InvoiceNumber"].Add(Row.DocumentCode);
                FormValues["InvoiceAmount"].Add(Row.TotalAmount.ToString("n2") + " " + PaymentDetails.AApSupplier[0].CurrencyCode);
            }

            FormValues["TotalPayment"].Add(PaymentDetails.AApPayment[0].Amount.ToString("n2") + " " + PaymentDetails.AApSupplier[0].CurrencyCode);

            String TemplateFilename = TAppSettingsManager.GetValue("Formletters.Path") + "\\ApRemittanceAdvice.html";

            if (!File.Exists(TemplateFilename))
            {
                lblLoadStatus.Text = String.Format(Catalog.GetString("Error - unable to load HTML template from {0}"), TemplateFilename);
                return;
            }

            FHtmlDoc = TFormLettersTools.PrintSimpleHTMLLetter(TemplateFilename, FormValues);

            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = printDocument.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                lblLoadStatus.Text = Catalog.GetString("There is no printer, so printing is not possible");
                return;
            }

            FGfxPrinter = new TGfxPrinter(printDocument, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            try
            {
                TPrinterHtml htmlPrinter = new TPrinterHtml(FHtmlDoc,
                    TAppSettingsManager.GetValue("Formletters.Path"),
                    FGfxPrinter);
                FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);
                this.ppvLetters.InvalidatePreview();
                this.ppvLetters.Document = FGfxPrinter.Document;
                this.ppvLetters.Zoom = 1;
                // GfxPrinter.Document.EndPrint += new PrintEventHandler(this.EndPrint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnPDF.Visible = true;
            btnCopy.Visible = true;

            lblLoadStatus.Text = "";
        }

        /// <summary>
        ///
        /// </summary>
        public void BtnLoad_Click(object sender, EventArgs e)
        {
            if (txtPaymentNum.NumberValueInt.HasValue)
            {
                lblLoadStatus.Text = "";
                PrintRemittanceAdvice(txtPaymentNum.NumberValueInt.Value, FLedgerNumber);
            }
        }

        /// <summary>
        /// Generate PDF document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnPDF_Click(object sender, EventArgs e)
        {
            string PDFPath = TFormLettersTools.GeneratePDFFromHTML(FHtmlDoc,
                TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                "reports");

            lblLoadStatus.Text = Catalog.GetString("Generated PDF Document: ") + PDFPath;
        }

        /// <summary>
        /// Copy HTML text to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnCopy_Click(object sender, EventArgs e)
        {
            Ict.Common.Utilities.CopyHtmlToClipboard(FHtmlDoc);
            lblLoadStatus.Text = Catalog.GetString("Text copied to Clipboard.");
        }
    }
}