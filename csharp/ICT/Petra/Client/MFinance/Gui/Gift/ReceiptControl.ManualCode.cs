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
                txtLedger.Text = FLedgerNumber.ToString();
            }
        }

        private void InitializeManualCode()
        {
        }

        private void LoadUnreceiptedGifts()
        {
            FGiftTbl = TRemote.MFinance.Gift.WebConnectors.GetUnreceiptedGifts(FLedgerNumber);
            FGiftTbl.Columns.Add("Selected", typeof(bool));
            FGiftTbl.Columns.Add("Print", typeof(bool));
            FGiftTbl.Columns.Add("Remove", typeof(bool));
            foreach (DataRow Row in FGiftTbl.Rows)
            {
                Row["Selected"] = false;
                Row["Print"] = false;
                Row["Remove"] = false;
            }

            BoundDataView GiftsView = new BoundDataView(FGiftTbl.DefaultView);
            GiftsView.AllowNew = false;

            grdDetails.DataSource = GiftsView;
            grdDetails.Columns.Clear();
            grdDetails.AddCheckBoxColumn("Sel", FGiftTbl.Columns["Selected"], 25, false);
            grdDetails.AddTextColumn("Recpt#", FGiftTbl.Columns["ReceiptNumber"],60);
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

        private void OnBtnPrint(Object sender, EventArgs e)
        {
            foreach (DataRow Row in FGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    Row["Print"] = true;
                }
            }

            //
            // The HTML string returned here may be several complete HTML documents, with "|PageBreak|" between each one.
            string HtmlDoc = TRemote.MFinance.Gift.WebConnectors.PrintOrRemoveReceipts(FLedgerNumber, FGiftTbl, "GiftReceipt.en-GB.html");
            System.Drawing.Printing.PrintDocument Printer = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = Printer.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                MessageBox.Show(Catalog.GetString("No printer is installed, so printing is not possible"));
                return;
            }

            string[] HtmlPages = HtmlDoc.Split(new string[] { "|PageBreak|" }, StringSplitOptions.None);

            try
            {
                foreach (string htmlPage in HtmlPages)
                {
                    System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                    TGfxPrinter GfxPrinter = new TGfxPrinter(printDocument, TGfxPrinter.ePrinterBehaviour.eFormLetter);
                    TPrinterHtml htmlPrinter = new TPrinterHtml(htmlPage,
                        TAppSettingsManager.GetValue("Formletters.Path"),
                        GfxPrinter);
                    GfxPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);
                    GfxPrinter.Document.Print();
                }

                if (MessageBox.Show(
                    Catalog.GetString("Please check that the receipts printed correctly.\r\nThe selected gifts will now be marked as receipted."),
                    Catalog.GetString("Receipt Printing"),
                    MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }

                //
                // These receipts that have been printed can now be marked in the database, so that they won't be printed again..
                //
                foreach (DataRow Row in FGiftTbl.Rows)
                {
                    Row["Print"] = false;
                }
                OnBtnRemove(sender, e);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Catalog.GetString("Receipt Printing"));
            }

            
        }

        private void OnBtnRemove(Object sender, EventArgs e)
        {
             foreach (DataRow Row in FGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    Row["Remove"] = true;
                }
            }
            TRemote.MFinance.Gift.WebConnectors.PrintOrRemoveReceipts(FLedgerNumber, FGiftTbl, "");
           LoadUnreceiptedGifts();
        }

        private void OnBtnClose(Object sender, EventArgs e)
        {
            this.Close();
        }
    }
}