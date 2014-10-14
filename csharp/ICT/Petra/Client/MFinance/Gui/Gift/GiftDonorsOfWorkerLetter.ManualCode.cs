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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Printing;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Printing;
using Ict.Common.Verification;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    partial class TFrmGiftDonorsOfWorkerLetter
    {
        private Int32 FLedgerNumber;

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private void GenerateLetters(System.Object sender, EventArgs e)
        {
            FMainDS = TRemote.MFinance.Gift.WebConnectors.GetDonorsOfWorker(
                Convert.ToInt64(txtWorkerPartnerKey.Text),
                FLedgerNumber,
                true, true);

            if (FMainDS.BestAddress.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no letters with valid addresses for your current parameters"),
                    Catalog.GetString("Nothing to print"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // TODO: for some reason, the columns' initialisation in the constructor does not have any effect; need to do here again???
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Donor", FMainDS.AGift.ColumnDonorKey);
            grdDetails.AddTextColumn("DonorShortName", FMainDS.AGift.ColumnDonorShortName);
            grdDetails.AddTextColumn("Recipient", FMainDS.AGift.ColumnRecipientDescription);

            FMainDS.AGift.DefaultView.Sort = NewDonorTDSAGiftTable.GetDonorShortNameDBName();

            FMainDS.AGift.DefaultView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGift.DefaultView);

            PrintLetters();
        }

        private Int32 FNumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;

        private void PrintLetters()
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString("HTML file (*.html)|*.html;*.htm");
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Select the template for the letter to the new donors");

            if (DialogOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string letterTemplateFilename = DialogOpen.FileName;

            StreamReader sr = new StreamReader(letterTemplateFilename);

            string htmlTemplate = sr.ReadToEnd();

            sr.Close();

            StringCollection Letters = TRemote.MFinance.Gift.WebConnectors.PrepareNewDonorLetters(ref FMainDS, htmlTemplate);

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

                    FGfxPrinter = new TGfxPrinter(printDocument, TGfxPrinter.ePrinterBehaviour.eFormLetter);
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
        }

        private void EndPrint(object ASender, PrintEventArgs AEv)
        {
            FNumberOfPages = FGfxPrinter.NumberOfPages;
            RefreshPagePosition();
        }

        private void CreateExtract(object ASender, EventArgs AEv)
        {
            TFrmCreateExtract newExtract = new TFrmCreateExtract();

            newExtract.BestAddress = FMainDS.BestAddress;
            newExtract.IncludeNonValidAddresses = false;
            newExtract.ShowDialog();
        }

        private void AddContactHistory(object ASender, EventArgs AEv)
        {
            List <Int64>partnerKeys = new List <long>();

            foreach (BestAddressTDSLocationRow row in FMainDS.BestAddress.Rows)
            {
                if (row.ValidAddress)
                {
                    partnerKeys.Add(row.PartnerKey);
                }
            }

            // No Mailing code, because this is a form letter
            TRemote.MPartner.Partner.WebConnectors.AddContact(partnerKeys,
                DateTime.Today,
                "",
                MPartnerConstants.METHOD_CONTACT_FORMLETTER,
                Catalog.GetString("Letter for donors of worker"),
                SharedConstants.PETRAMODULE_FINANCE1,
                "");

            MessageBox.Show(Catalog.GetString("The partner contacts have been updated successfully!"),
                Catalog.GetString("Success"));
        }
    }
}