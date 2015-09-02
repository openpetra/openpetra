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
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevAge.ComponentModel;

using Ict.Common;
using Ict.Common.Printing;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Gui;
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

        private void PopulateGrid(bool AFirstTime)
        {
            BoundDataView GiftsView = new BoundDataView(FGiftTbl.DefaultView);

            GiftsView.AllowNew = false;
            grdDetails.DataSource = GiftsView;

            if (AFirstTime)
            {
                grdDetails.Columns.Clear();
                grdDetails.AddCheckBoxColumn("Sel", FGiftTbl.Columns["Selected"], 30, false);
                grdDetails.AddTextColumn("Recpt#", FGiftTbl.Columns["ReceiptNumber"]);
                grdDetails.AddDateColumn("Date", FGiftTbl.Columns["DateEntered"]);
                grdDetails.AddTextColumn("Donor", FGiftTbl.Columns["Donor"]);
                grdDetails.AddTextColumn("Batch#", FGiftTbl.Columns["BatchNumber"]);
                grdDetails.AddTextColumn("Ref", FGiftTbl.Columns["Reference"]);
            }

            UpdateRecordNumberDisplay();
        }

        private void RunOnceOnActivationManual()
        {
            grdDetails.CausesValidation = false;
            grdDetails.SelectionMode = SourceGrid.GridSelectionMode.Row;

            // load DataTable
            FGiftTbl = TRemote.MFinance.Gift.WebConnectors.GetUnreceiptedGifts(FLedgerNumber);
            FGiftTbl.Columns.Add("Selected", typeof(bool));

            foreach (DataRow Row in FGiftTbl.Rows)
            {
                Row["Selected"] = false;
            }

            PopulateGrid(true);
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

            CoolPrintPreviewDialog PrintDlg = new CoolPrintPreviewDialog();
            PrintDlg.Document = GfxPrinter.Document;
            PrintDlg.ClientSize = new System.Drawing.Size(500, 720);
            try
            {
                PrintDlg.ShowDialog();
            }
            catch (Exception)  // if the user presses Cancel, an exception may be raised!
            {
            }
        }

        private void OnBtnPrint(Object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument Printer = new System.Drawing.Printing.PrintDocument();
            bool printerInstalled = Printer.PrinterSettings.IsValid;

            if (!printerInstalled)
            {
                MessageBox.Show(Catalog.GetString("No printer is installed, so printing is not possible"),
                    Catalog.GetString("Receipt Printing"));

                this.Cursor = Cursors.Default;
                return;
            }

            DataTable SelectedRecords = GetSelectedRecords();

            if (SelectedRecords.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("No records are selected."),
                    Catalog.GetString("Receipt Printing"));

                this.Cursor = Cursors.Default;
                return;
            }

            OpenFileDialog DialogOpen = new OpenFileDialog();

            if (Directory.Exists(TAppSettingsManager.GetValue("Formletters.Path")))
            {
                DialogOpen.InitialDirectory = TAppSettingsManager.GetValue("Formletters.Path");
            }

            DialogOpen.Filter = Catalog.GetString("HTML file (*.html)|*.html;*.htm");
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Select the template for the gift receipt");

            if (DialogOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string HTMLTemplateFilename = DialogOpen.FileName;

            this.Cursor = Cursors.WaitCursor;

            //
            // The HTML string returned here may be several complete HTML documents, with <body>...</body> for each page.
            string HtmlDoc = TRemote.MFinance.Gift.WebConnectors.PrintReceipts(FLedgerNumber, SelectedRecords, HTMLTemplateFilename);

            if (HtmlDoc == "")
            {
                MessageBox.Show(Catalog.GetString("The server returned no pages to print."),
                    Catalog.GetString("Receipt Printing"));

                this.Cursor = Cursors.Default;
                return;
            }

            String ReceiptedDonorsList = "";
            int NumberOfDonors = 0;

            foreach (DataRow Row in FGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    String DonorName = Row["Donor"].ToString() + "\r\n";

                    if (!ReceiptedDonorsList.Contains(DonorName))
                    {
                        ReceiptedDonorsList += DonorName;
                        NumberOfDonors++;
                    }
                }
            }

            try
            {
                PreviewOrPrint(HtmlDoc);

                string Message = Catalog.GetPluralString("Was a receipt to the following donor printed correctly?",
                    "Were receipts to the following donors printed correctly?", NumberOfDonors);

                if (MessageBox.Show(
                        Message + "\n\n" +
                        ReceiptedDonorsList,
                        Catalog.GetString("Receipt Printing"),
                        MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                //
                // These receipts that have been printed can now be marked in the database, so that they won't be printed again..
                //
                RemoveReceipts(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Catalog.GetString("Receipt Printing"));
            }

            this.Cursor = Cursors.Default;
        }

        private void OnBtnRemove(Object sender, EventArgs e)
        {
            RemoveReceipts();
        }

        private void RemoveReceipts(bool AAskPermission = true)
        {
            DataTable SelectedRecords = GetSelectedRecords();

            if (SelectedRecords.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("No records are selected."),
                    Catalog.GetString("Receipt Printing"));

                this.Cursor = Cursors.Default;
                return;
            }

            if (AAskPermission)
            {
                string Msg = string.Empty;

                if (SelectedRecords.Rows.Count == 1)
                {
                    Msg = Catalog.GetString("Are you sure you want to remove the selected receipt?");
                }
                else
                {
                    Msg = string.Format(Catalog.GetString("Are you sure you want to remove the {0} selected receipts?"), (SelectedRecords.Rows.Count));
                }

                if (MessageBox.Show(Msg,
                        Catalog.GetString("Receipt Printing"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
            }

            this.Cursor = Cursors.WaitCursor;

            try
            {
                TRemote.MFinance.Gift.WebConnectors.MarkReceiptsPrinted(FLedgerNumber, SelectedRecords);

                // remove removed records from datatable
                DataTable TempTable = FGiftTbl.Copy();
                int SelectedIndex = grdDetails.GetFirstHighlightedRowIndex();
                int i = 0;

                foreach (DataRow Row in FGiftTbl.Rows)
                {
                    if (Row["Selected"].Equals(true))
                    {
                        TempTable.Rows.RemoveAt(i);

                        if (i < (SelectedIndex - 1))
                        {
                            SelectedIndex--;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }

                FGiftTbl = TempTable.Copy();

                // if we don't do this then checking a previously deleted and highlighted record will cause an error
                grdDetails.SelectRowInGrid(0);

                // reload the grid
                PopulateGrid(false);

                grdDetails.SelectRowInGrid(SelectedIndex);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                throw ex;
            }

            this.Cursor = Cursors.Default;
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

        /// <summary>
        /// Returns a DataTable containing all the selected records
        /// </summary>
        /// <returns></returns>
        private DataTable GetSelectedRecords()
        {
            DataTable ReturnTable = FGiftTbl.Clone();

            foreach (DataRow Row in FGiftTbl.Rows)
            {
                if (Row["Selected"].Equals(true))
                {
                    DataRow TempRow = ReturnTable.NewRow();
                    TempRow["BatchNumber"] = Row["BatchNumber"];
                    TempRow["TransactionNumber"] = Row["TransactionNumber"];
                    ReturnTable.Rows.Add(TempRow);
                }
            }

            return ReturnTable;
        }

        ///<summary>
        /// Update the text in the button panel indicating details of the record count
        /// </summary>
        private void UpdateRecordNumberDisplay()
        {
            if (grdDetails.DataSource != null)
            {
                int RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
        }
    }
}