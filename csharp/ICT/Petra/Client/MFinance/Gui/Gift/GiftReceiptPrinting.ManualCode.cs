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
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Printing;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftReceiptPrinting
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private void InitializeManualCode()
        {
            dtpStartDate.Date = new DateTime(DateTime.Now.Year - 1, 1, 1);
            dtpEndDate.Date = new DateTime(DateTime.Now.Year - 1, 12, 31);
        }

        private Int32 FNumberOfPages = 0;
        private TGfxPrinter FGfxPrinter = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="AllLetters">HTML text (could be several pages)</param>
        /// <param name="APathForImagesBase">Could be null if I'm not printing images!</param>
        public void PreviewOrPrint(String AllLetters, String APathForImagesBase)
        {
            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = printDocument.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                MessageBox.Show(Catalog.GetString("There is no printer, so printing is not possible"));
                return;
            }

            FGfxPrinter = new TGfxPrinter(printDocument, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            try
            {
                TPrinterHtml htmlPrinter = new TPrinterHtml(AllLetters,
                    APathForImagesBase,
                    FGfxPrinter);
                FGfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);
                this.ppvLetters.InvalidatePreview();
                this.ppvLetters.Document = FGfxPrinter.Document;
                this.ppvLetters.Zoom = 1;
                FGfxPrinter.Document.EndPrint += new PrintEventHandler(this.EndPrint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateLetters(System.Object sender, EventArgs e)
        {
            Int64 DonorKey = 0;
            string Extract = null;

            if ((!dtpStartDate.Date.HasValue) || (!dtpEndDate.Date.HasValue))
            {
                MessageBox.Show(Catalog.GetString("Please supply valid Start and End dates."));
                return;
            }

            if (rbtPartner.Checked)
            {
                if (txtDonor.Text == "0000000000")
                {
                    MessageBox.Show(Catalog.GetString("Please select a donor."));
                    return;
                }
                else
                {
                    DonorKey = Convert.ToInt64(txtDonor.Text);
                }
            }

            if (rbtExtract.Checked)
            {
                if (txtExtract.Text == "")
                {
                    MessageBox.Show(Catalog.GetString("No extract name entered!"));
                    return;
                }
                else
                {
                    Extract = txtExtract.Text;
                }
            }

            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString("HTML file (*.html)|*.html;*.htm");
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Select the template for the gift receipt");

            if (DialogOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string letterTemplateFilename = DialogOpen.FileName;

            StreamReader sr = new StreamReader(letterTemplateFilename);

            string htmlTemplate = sr.ReadToEnd();

            sr.Close();

            string AllLetters = TRemote.MFinance.Gift.WebConnectors.CreateAnnualGiftReceipts(FLedgerNumber,
                dtpStartDate.Date.Value,
                dtpEndDate.Date.Value,
                htmlTemplate,
                chkDeceasedDonorsFirst.Checked,
                Extract,
                DonorKey);

            if (AllLetters.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("There are no posted gifts in the date range"));
                return;
            }

            PreviewOrPrint(AllLetters, System.IO.Path.GetDirectoryName(letterTemplateFilename));
        }

        private void EndPrint(object ASender, PrintEventArgs AEv)
        {
            FNumberOfPages = FGfxPrinter.NumberOfPages;

            RefreshPagePosition();
            ttxCurrentPage.Text = "1";
        }
    }
}