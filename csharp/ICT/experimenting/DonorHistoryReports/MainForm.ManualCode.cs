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
using System.IO;

namespace Ict.Petra.Client.MFinance.Gui.DonorHistoryReports
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

            TAppSettingsManager settings = new TAppSettingsManager();
            dtpStartDate.Value = DateTime.Now.AddMonths(-1 * settings.GetInt32("DefaultNumberOfMonths"));
            dtpEndDate.Value = DateTime.Now;
            txtMinimumAmount.Text = settings.GetValue("DefaultMinimumAmount");
            txtMaximumAmount.Text = settings.GetValue("DefaultMaximumAmount");
            txtMinimumCount.Text = settings.GetValue("DefaultMinimumCount");
            txtMaximumCount.Text = settings.GetValue("DefaultMaximumCount");
        }

        private void FilterChanged(System.Object sender, EventArgs e)
        {
        }

        private void GenerateLetters(System.Object sender, EventArgs e)
        {
            FMainDS.Gift.Rows.Clear();

            TGetData.GetDonorHistory(ref FMainDS,
                dtpStartDate.Value,
                dtpEndDate.Value,
                Convert.ToDecimal(txtMinimumAmount.Text),
                Convert.ToDecimal(txtMaximumAmount.Text),
                Convert.ToInt32(txtMinimumCount.Text),
                Convert.ToInt32(txtMaximumCount.Text),
                chkProjects.Checked,
                chkSupport.Checked,
                chkFamily.Checked,
                chkChurch.Checked,
                chkOrganisation.Checked);

            // TODO: for some reason, the columns' initialisation in the constructor does not have any effect; need to do here again???
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Donor", FMainDS.Gift.ColumnDonorKey);
            grdDetails.AddTextColumn("DonorShortName", FMainDS.Gift.ColumnDonorShortName);

            FMainDS.Donor.DefaultView.Sort = DonorHistoryTDSDonorTable.GetDonorShortNameDBName();

            FMainDS.Donor.DefaultView.RowFilter = "ValidAddress = true";

            FMainDS.Donor.DefaultView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.Donor.DefaultView);

            // TODO: double click on grid, should show gifts that are part of the query

//            PrintLetters();
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

        private void ExportCSV(object ASender, EventArgs AEv)
        {
            // TODO export Donor Table
//              Ict.Common.IO.TImportExportDialogs.ExportWithDialog(

            TAppSettingsManager settings = new TAppSettingsManager();
            StreamWriter sw = new StreamWriter(settings.GetValue("OutputCSV.File"));

            FMainDS.Donor.DefaultView.RowFilter = "ValidAddress = true";

            foreach (DataRowView rv in FMainDS.Donor.DefaultView)
            {
                DonorHistoryTDSDonorRow row = (DonorHistoryTDSDonorRow)rv.Row;
                string line = "";
                line = StringHelper.AddCSV(line, row.DonorKey.ToString());
                line = StringHelper.AddCSV(line, row.DonorShortName);
                line = StringHelper.AddCSV(line, row.GiftTotalCount.ToString());
                line = StringHelper.AddCSV(line, row.GiftTotalAmount.ToString());
                line = StringHelper.AddCSV(line, row.Locality.ToString());
                line = StringHelper.AddCSV(line, row.StreetName.ToString());
                line = StringHelper.AddCSV(line, row.Address3.ToString());
                line = StringHelper.AddCSV(line, row.Building1.ToString());
                line = StringHelper.AddCSV(line, row.Building2.ToString());
                line = StringHelper.AddCSV(line, row.PostalCode.ToString());
                line = StringHelper.AddCSV(line, row.City.ToString());
                line = StringHelper.AddCSV(line, row.CountryCode.ToString());
                line = StringHelper.AddCSV(line, row.CountryName.ToString());
                sw.WriteLine(line);
            }

            sw.Close();
        }
    }
}