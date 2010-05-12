//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Printing;

namespace Tests.Common.Printing
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        const string FileName = "../../Common/Printing/test.html";

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            if (!File.Exists(FileName))
            {
                txtHTMLText.Text = "<html><body>" + String.Format(Catalog.GetString("Cannot find file {0}"), FileName) + "</body></html>";
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                txtHTMLText.Text = r.ReadToEnd();
                r.Close();
            }

            cmbZoom.SelectedIndex = 1;

            TbbPreviewClick(null, null);
        }

        private Int32 NumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;

        private void PrintDocument_EndPrint(System.Object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            NumberOfPages = FGfxPrinter.NumberOfPages;
        }

        void TbbPreviewClick(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = txtHTMLText.Text;

            PrintDocument doc = new PrintDocument();

            FGfxPrinter = new TGfxPrinter(doc);
            TPrinterHtml htmlPrinter = new TPrinterHtml(txtHTMLText.Text,
                String.Empty,
                FGfxPrinter);
            FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter);

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
    }
}