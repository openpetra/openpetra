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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Printing;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.NewDonorSubscriptions
{
    partial class TFrmMainForm
    {
        private void InitializeManualCode()
        {
            try
            {
                TGetData.InitDBConnection();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, Catalog.GetString("Error connecting to Petra 2.x"));
            }

            dtpStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpEndDate.Value = DateTime.Now;
        }

        private void FilterChanged(System.Object sender, EventArgs e)
        {
        }

        private void GenerateLetters(System.Object sender, EventArgs e)
        {
            FMainDS.AGift.Rows.Clear();

            TGetData.GetNewDonorSubscriptions(ref FMainDS, dtpStartDate.Value, dtpEndDate.Value);

            // TODO: for some reason, the columns' initialisation in the constructor does not have any effect; need to do here again???
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Donor", FMainDS.AGift.ColumnDonorKey);
            grdDetails.AddTextColumn("DonorShortName", FMainDS.AGift.ColumnDonorShortName);
            grdDetails.AddTextColumn("Recipient", FMainDS.AGift.ColumnRecipientDescription);
            grdDetails.AddTextColumn("Subscription Start", FMainDS.AGift.ColumnDateOfSubscriptionStart);
            grdDetails.AddTextColumn("Date Gift", FMainDS.AGift.ColumnDateOfFirstGift);

            FMainDS.AGift.DefaultView.Sort = NewDonorTDSAGiftTable.GetDonorShortNameDBName();

            FMainDS.AGift.DefaultView.RowFilter = "ValidAddress = true";

            FMainDS.AGift.DefaultView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGift.DefaultView);

            PrintLetters();
        }

        private Int32 FNumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;

        private void PrintLetters()
        {
            List <string>Letters = TGetData.PrepareLetters(ref FMainDS);

            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = printDocument.PrinterSettings.IsValid;

            if (printerInstalled)
            {
                string AllLetters = String.Empty;

                foreach (string letter in Letters)
                {
                    if (AllLetters.Length > 0)
                    {
                        // AllLetters += "<div style=\"page-break-before: always;\"/>";
                        string body = letter.Substring(letter.IndexOf("<body"));
                        body = body.Substring(0, body.IndexOf("</html"));
                        AllLetters += body;
                    }
                    else
                    {
                        // without closing html
                        AllLetters += letter.Substring(0, letter.IndexOf("</html"));
                    }
                }

                if (AllLetters.Length > 0)
                {
                    AllLetters += "</html>";
                }

                string letterTemplateFilename = TAppSettingsManager.GetValueStatic("LetterTemplate.File");
                FGfxPrinter = new TGfxPrinter(printDocument);
                try
                {
                    TPrinterHtml htmlPrinter = new TPrinterHtml(AllLetters,
                        System.IO.Path.GetDirectoryName(letterTemplateFilename),
                        FGfxPrinter);
                    FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);
                    this.ppvLetters.InvalidatePreview();
                    this.ppvLetters.Document = FGfxPrinter.Document;
                    this.ppvLetters.Zoom = 1;
                    FGfxPrinter.Document.EndPrint += new PrintEventHandler(this.EndPrint);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void EndPrint(object ASender, PrintEventArgs AEv)
        {
            FNumberOfPages = FGfxPrinter.NumberOfPages;
            RefreshPagePosition();
        }
    }
}