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
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Printing;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Common.Data;

namespace Tests.Common.Printing
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        const string FileName = "../../csharp/ICT/Testing/exe/Printing/test.html";

        /// constructor
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            if (!File.Exists(FileName))
            {
                txtHTMLText.Text = "<html><body>" + String.Format("Cannot find file {0}", FileName) + "</body></html>";
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                txtHTMLText.Text = r.ReadToEnd();
                r.Close();
            }

            cmbZoom.SelectedIndex = 1;

            //TbbPreviewClick(null, null);
        }

        private Int32 NumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;

        private void PrintDocument_EndPrint(System.Object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            NumberOfPages = FGfxPrinter.NumberOfPages;
        }

        void TbbSavePDFClick(object sender, EventArgs e)
        {
            PrintDocument doc = new PrintDocument();

            TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(txtHTMLText.Text,
                String.Empty,
                pdfPrinter);

            pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            pdfPrinter.SavePDF("test.pdf");

            System.Diagnostics.Process.Start(Path.GetFullPath("test.pdf"));
        }

        void TbbPrintPDFToScreenClick(object sender, EventArgs e)
        {
            PrintDocument doc = new PrintDocument();

            TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(txtHTMLText.Text,
                String.Empty,
                pdfPrinter);

            pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            FGfxPrinter = pdfPrinter;

            doc.EndPrint += new PrintEventHandler(this.PrintDocument_EndPrint);

            printPreviewControl1.Document = doc;
            printPreviewControl1.InvalidatePreview();

            printPreviewControl1.Rows = 1;
        }

        void TbbPreviewClick(object sender, EventArgs e)
        {
            PrintDocument doc = new PrintDocument();

            FGfxPrinter = new TGfxPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(txtHTMLText.Text,
                String.Empty,
                FGfxPrinter);
            FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            doc.EndPrint += new PrintEventHandler(this.PrintDocument_EndPrint);

            printPreviewControl1.Document = doc;
            printPreviewControl1.InvalidatePreview();

            printPreviewControl1.Rows = 1;
        }

        void CmbZoomSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbZoom.SelectedIndex == 0)
            {
                // 100%
                printPreviewControl1.Zoom = 1;
            }
            else if (cmbZoom.SelectedIndex == 1)
            {
                // 75%
                printPreviewControl1.Zoom = 0.75;
            }
            else if (cmbZoom.SelectedIndex == 2)
            {
                // 50%
                printPreviewControl1.Zoom = 0.50;
            }
            else if (cmbZoom.SelectedIndex == 3)
            {
                // fit page
                printPreviewControl1.AutoZoom = true;
            }
        }

        void TbbSaveTestTextClick(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(FileName);

            sw.WriteLine(txtHTMLText.Text);
            sw.Close();
        }

        void TbbPreviousPageClick(object sender, EventArgs e)
        {
            if (printPreviewControl1.StartPage > 0)
            {
                printPreviewControl1.StartPage--;
            }
        }

        void TbbNextPageClick(object sender, EventArgs e)
        {
            if (printPreviewControl1.StartPage < NumberOfPages)
            {
                printPreviewControl1.StartPage++;
            }
        }

        void TbbImportReportBinaryFileClick(object sender, EventArgs e)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = "Binary Report File (*.bin)|*.bin";
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = "Open Binary Report File";

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                TResultList Results = new TResultList();
                TParameterList Parameters;
                Results.ReadBinaryFile(DialogOpen.FileName, out Parameters);

                PrintDocument doc = new PrintDocument();

                TPetraIdentity PetraIdentity = new TPetraIdentity(
                    "TESTUSER", "", "", "", "", DateTime.MinValue,
                    DateTime.MinValue, DateTime.MinValue, 0, -1, -1, false,
                    false);

                UserInfo.GUserInfo = new TPetraPrincipal(PetraIdentity, null);

                FGfxPrinter = new TGfxPrinter(doc, TGfxPrinter.ePrinterBehaviour.eReport);
                new TReportPrinterLayout(Results, Parameters, FGfxPrinter, true);
                printPreviewControl1.Document = doc;
                doc.EndPrint += new PrintEventHandler(this.PrintDocument_EndPrint);
                printPreviewControl1.InvalidatePreview();
            }
        }
    }
}