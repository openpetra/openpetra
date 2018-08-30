//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2004-2018 by OM International
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
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Xml;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

using Ict.Common;
using Ict.Common.Printing;

namespace Ict.Common.Printing.Testing
{
    ///  This is a test for printing to PDF
    [TestFixture]
    public class TTestPrintingPDF
    {
        /// init
        [SetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestServer.config");
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Client.DebugLevel", 0);
        }

        /// test the PDF Printer
        [Test]
        public void TestPDFPrinter()
        {
            const string FileName = "../../csharp/ICT/Testing/lib/Common/Printing/test.html";
            string TextToPrint;

            if (!File.Exists(FileName))
            {
                TextToPrint = "<html><body>" + String.Format("Cannot find file {0}", FileName) + "</body></html>";
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                TextToPrint = r.ReadToEnd();
                r.Close();
            }

            TPdfPrinter pdfPrinter = new TPdfPrinter(TPdfPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(TextToPrint,
                String.Empty,
                pdfPrinter);

            pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            pdfPrinter.SavePDF("test.pdf");
        }
    }
}
