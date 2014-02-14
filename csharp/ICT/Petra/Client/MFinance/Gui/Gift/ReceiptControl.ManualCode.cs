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
using DevAge.ComponentModel;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    partial class TFrmReceiptControl
    {
        private Int32 FLedgerNumber;
        private DataTable FGiftTbl;

        /// The ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                txtLedger.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            }
        }

        private void InitializeManualCode()
        {
        }

        private void LoadUnreceiptedGifts()
        {
            FGiftTbl = TRemote.MFinance.Gift.WebConnectors.GetUnreceiptedGifts(FLedgerNumber);
            FGiftTbl.Columns.Add("Selected", typeof(bool));

            foreach (DataRow Row in FGiftTbl.Rows)
            {
                Row["Selected"] = false;
            }

            BoundDataView GiftsView = new BoundDataView(FGiftTbl.DefaultView);
            GiftsView.AllowNew = false;

            grdDetails.DataSource = GiftsView;
            grdDetails.Columns.Clear();
            grdDetails.AddCheckBoxColumn("Sel", FGiftTbl.Columns["Selected"], 30, false);
            grdDetails.AddTextColumn("Recpt#", FGiftTbl.Columns["ReceiptNumber"], 60);
            grdDetails.AddDateColumn("Date", FGiftTbl.Columns["DateEntered"]);
            grdDetails.AddTextColumn("Donor", FGiftTbl.Columns["Donor"], 190);
            grdDetails.AddTextColumn("Batch#", FGiftTbl.Columns["BatchNumber"], 60);
            grdDetails.AddTextColumn("Ref", FGiftTbl.Columns["Reference"], 170);

            // Only the text columns can have their widths set while
            // they're being added.
            // For the currency and date columns,
            // I need to set the width afterwards. (THIS WILL GO WONKY IF EXTRA FIELDS ARE ADDED ABOVE.)

            grdDetails.Columns[2].Width = 100;
            grdDetails.AutoStretchColumnsToFitWidth = false;
        }

        private void RunOnceOnActivationManual()
        {
            grdDetails.CausesValidation = false;
            grdDetails.SelectionMode = SourceGrid.GridSelectionMode.Row;
            LoadUnreceiptedGifts();
        }

        /// <summary>
        /// Use a .NET PrintPreview Dialog to print (or not print) this (possibly multi-page) HTML document.
        /// </summary>
        /// <param name="HtmlPages"></param>
        public static void PreviewOrPrint(String HtmlPages)
        {
            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = printDocument.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                MessageBox.Show(Catalog.GetString("There is no printer, so printing is not possible"));
                return;
            }

            TGfxPrinter GfxPrinter = new TGfxPrinter(printDocument, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(HtmlPages,
                TAppSettingsManager.GetValue("Formletters.Path"),
                GfxPrinter);
            GfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            PrintPreviewDialog PrintDlg = new PrintPreviewDialog();
            PrintDlg.Document = GfxPrinter.Document;
            PrintDlg.ClientSize = new System.Drawing.Size(500, 720);
            PrintDlg.ShowDialog();
        }

        private void OnBtnPrint(Object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument Printer = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = Printer.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                MessageBox.Show(Catalog.GetString("No printer is installed, so printing is not possible"),
                    Catalog.GetString("Receipt Printing"));
                return;
            }

            //
            // The HTML string returned here may be several complete HTML documents, with <body>...</body> for each page.
            string HtmlDoc = TRemote.MFinance.Gift.WebConnectors.PrintReceipts(FGiftTbl);

            if (HtmlDoc == "")
            {
                MessageBox.Show(Catalog.GetString("The server returned no pages to print."),
                    Catalog.GetString("Receipt Printing"));
                return;
            }

            String ReceiptedDonorsList = "";

            foreach (DataRow Row in FGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    String DonorName = Row["Donor"].ToString() + "\r\n";

                    if (!ReceiptedDonorsList.Contains(DonorName))
                    {
                        ReceiptedDonorsList += DonorName;
                    }
                }
            }

            try
            {
                PreviewOrPrint(HtmlDoc);

                if (MessageBox.Show(
                        Catalog.GetString(
                            "Press OK if receipts to these recipients were printed correctly.\r\nThe gifts will be marked as receipted.\r\n") +
                        ReceiptedDonorsList,
                        Catalog.GetString("Receipt Printing"),
                        MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }

                //
                // These receipts that have been printed can now be marked in the database, so that they won't be printed again..
                //
                OnBtnRemove(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Catalog.GetString("Receipt Printing"));
            }
        }

        private void OnBtnRemove(Object sender, EventArgs e)
        {
            TRemote.MFinance.Gift.WebConnectors.MarkReceiptsPrinted(FGiftTbl);

            LoadUnreceiptedGifts();
        }

        private void OnBtnRcptNumber(Object sender, EventArgs e)
        {
            Boolean IntFormatIsOk = false;
            Int32 ReceiptNum;
            Int32 NewReceiptNum = 0;

            do
            {
                ReceiptNum = TRemote.MFinance.Gift.WebConnectors.GetLastReceiptNumber(FLedgerNumber);
                String ReceiptNumString = (ReceiptNum + 1).ToString();
                PetraInputBox input = new PetraInputBox(
                    Catalog.GetString("Receipt Numbers"),
                    Catalog.GetString("The next Gift Receipt will have this number:"),
                    ReceiptNumString, false);
                input.ShowDialog();

                if (input.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                ReceiptNumString = input.GetAnswer();
                try
                {
                    NewReceiptNum = Convert.ToInt32(ReceiptNumString) - 1;
                    IntFormatIsOk = true;
                }
                catch (FormatException)
                {
                    MessageBox.Show(
                        Catalog.GetString("Please enter an integer."),
                        Catalog.GetString("Format Problem")
                        );
                }
            } while (!IntFormatIsOk);

            if (NewReceiptNum < ReceiptNum)
            {
                MessageBox.Show(
                    Catalog.GetString("WARNING - new Receipt Number is less than previous number.\r\nDuplicate Receipt Numbers might be printed."),
                    Catalog.GetString("Receipt Numbers"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );
            }

            TRemote.MFinance.Gift.WebConnectors.SetLastReceiptNumber(FLedgerNumber, NewReceiptNum);
        }

        private void OnBtnClose(Object sender, EventArgs e)
        {
            this.Close();
        }
    }
}