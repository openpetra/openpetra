//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared.Interfaces.MFinance;
using System.Threading;
using Ict.Common.Conversion;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPSupplierTransactions
    {
        private Int32 FLedgerNumber = -1;
        private Int64 FPartnerKey = -1;
        private AApSupplierRow FSupplierRow = null;
        private IAPUIConnectorsFind FFindObject = null;
        private bool FKeepUpSearchFinishedCheck = false;
        AccountsPayableTDS FMainDS = new AccountsPayableTDS();

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;


        private String FTypeFilter = "";    // filter which types of transactions are shown
        private String FStatusFilter = "";  // filter the status of invoices
        private int[] ColumnWidth =
        {
            20, 70, 90, 90, 90, 100, 110
        };
        private string FAgedOlderThan;

        /// <summary>
        /// Load the supplier and all the transactions (invoices and payments) that relate to it.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        public void LoadSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            FLedgerNumber = ALedgerNumber;
            FPartnerKey = APartnerKey;
            FMainDS = TRemote.MFinance.AP.WebConnectors.LoadAApSupplier(ALedgerNumber, APartnerKey);

            FSupplierRow = FMainDS.AApSupplier[0];

            //
            // Transactions older than
            DateTime AgedOlderThan = DateTime.Now;
            AgedOlderThan = AgedOlderThan.AddMonths(0 - FSupplierRow.PreferredScreenDisplay);
            FAgedOlderThan = AgedOlderThan.ToString("u");

            txtSupplierName.Text = FMainDS.PPartner[0].PartnerShortName;
            txtSupplierCurrency.Text = FSupplierRow.CurrencyCode;
            FFindObject = TRemote.MFinance.AP.UIConnectors.Find();

            FFindObject.FindSupplierTransactions(FLedgerNumber, FPartnerKey);

            // Start thread that checks for the end of the search operation on the PetraServer
            FKeepUpSearchFinishedCheck = true;
            Thread FinishedCheckThread = new Thread(new ThreadStart(SearchFinishedCheckThread));
            FinishedCheckThread.Start();

            grdResult.MouseClick += new MouseEventHandler(grdResult_Click);
            this.Text = Catalog.GetString("Supplier Transactions") + " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private delegate void SimpleDelegate();

        /// <summary>
        /// Thread for the search operation. Monitor's the Server System.Object's
        /// AsyncExecProgress.ProgressState and invokes UI updates from that.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SearchFinishedCheckThread()
        {
            // Check whether this thread should still execute
            while (FKeepUpSearchFinishedCheck)
            {
                /* The next line of code calls a function on the PetraServer
                 * > causes a bit of data traffic everytime! */
                switch (FFindObject.AsyncExecProgress.ProgressState)
                {
                    case TAsyncExecProgressState.Aeps_Finished:
                        FKeepUpSearchFinishedCheck = false;

                        // see also http://stackoverflow.com/questions/6184/how-do-i-make-event-callbacks-into-my-win-forms-thread-safe
                        if (InvokeRequired)
                        {
                            Invoke(new SimpleDelegate(FinishThread));
                        }
                        else
                        {
                            FinishThread();
                        }

                        break;

                    case TAsyncExecProgressState.Aeps_Stopped:
                        FKeepUpSearchFinishedCheck = false;
                        return;
                }

                // Sleep a bit, then loop...
                Thread.Sleep(200);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ANeededPage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        private DataTable GetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages)
        {
            ATotalRecords = 0;
            ATotalPages = 0;
            DataTable ResultsTable = new DataTable();

            ResultsTable = FFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
//          ResultsTable.Columns.Add("DiscountMsg", typeof(string));
            ResultsTable.Columns.Add("Type", typeof(string));
            ResultsTable.Columns.Add("Tagged", typeof(bool));

            foreach (DataRow Row in ResultsTable.Rows)
            {
                Row["Tagged"] = false;

                if (Row["CreditNote"].Equals(true))  // Payments also carry this "Credit note" label
                {
//                  Row["DiscountMsg"] = "";

                    if (Row["Status"].ToString().Length > 0) // Credit notes have Status, but payments don't.
                    {
                        Row["Type"] = "Credit Note";
                    }
                    else
                    {
                        Row["Type"] = "Payment";
                    }
                }
                else
                {
                    Row["Type"] = "Invoice";

/*
 *                  Int32 DiscountDays = (Int32)Row["DiscountDays"];
 *
 *                  if (DiscountDays > 0)
 *                  {
 *                      DateTime DiscountUntil = (DateTime)Row["Date"];
 *                      DiscountUntil = DiscountUntil.AddDays(DiscountDays);
 *                      Row["DiscountMsg"] =
 *                          String.Format("{0:n0}% until {1}", (Decimal)Row["DiscountPercent"], TDate.DateTimeToLongDateString2(DiscountUntil));
 *                  }
 *                  else
 *                  {
 *                      Row["DiscountMsg"] = "None";
 *                  }
 */
                }
            }

            return ResultsTable;
        }

        private void InitialiseGrid()
        {
            grdResult.Columns.Clear();
            grdResult.AddCheckBoxColumn("", FPagedDataTable.Columns["Tagged"], 20, false);
//          grdResult.AddTextColumn("AP#", FPagedDataTable.Columns["ApNum"], 50);
            grdResult.AddTextColumn("Inv#", FPagedDataTable.Columns["InvNum"], 70);
            grdResult.AddTextColumn("Type", FPagedDataTable.Columns["Type"], 90);
            grdResult.AddCurrencyColumn("Amount", FPagedDataTable.Columns["Amount"], 2);
            grdResult.AddTextColumn("Currency", FPagedDataTable.Columns["Currency"], 90);
//          grdResult.AddTextColumn("Discount", FPagedDataTable.Columns["DiscountMsg"], 150);
            grdResult.AddTextColumn("Status", FPagedDataTable.Columns["Status"], 100);
            grdResult.AddDateColumn("Date", FPagedDataTable.Columns["Date"]);

            // Although "Ordinary" text columns have had a width specified above, this loop overwrites
            // the widths with values that survive the user pressing the "Reload" button.
            for (int i = 0; i < ColumnWidth.Length; i++)
            {
                grdResult.Columns[i].Width = ColumnWidth[i];
            }
        }

        private void UpdateDisplayedBalance()
        {
            Decimal SumDisplayed = 0.0m;

            if (FPagedDataTable != null)
            {
                foreach (DataRowView rv in FPagedDataTable.DefaultView)
                {
                    DataRow Row = rv.Row;

                    if ((Row["Currency"].ToString() == txtSupplierCurrency.Text) && (Row["Amount"].GetType() == typeof(Decimal)))
                    {
                        if (Row["CreditNote"].Equals(true))  // Payments also carry this "Credit note" label
                        {
                            SumDisplayed -= (Decimal)Row["Amount"];
                        }
                        else
                        {
                            SumDisplayed += (Decimal)Row["Amount"];
                        }
                    }
                }
            }

            txtDisplayedBalance.Text = SumDisplayed.ToString("n2");
        }

        private void UpdateRowFilter()
        {
            if (FPagedDataTable != null)
            {
                String FilterJoint = "";

                if ((FTypeFilter.Length > 0) && (FStatusFilter.Length > 0))
                {
                    FilterJoint = " AND ";
                }

                FPagedDataTable.DefaultView.RowFilter = FTypeFilter + FilterJoint + FStatusFilter;
                UpdateDisplayedBalance();
            }
        }

        private void FinishThread()
        {
            // Fetch the first page of data
            try
            {
                FPagedDataTable = grdResult.LoadFirstDataPage(@GetDataPagedResult);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
            InitialiseGrid();
            DataView myDataView = FPagedDataTable.DefaultView;
            myDataView.AllowNew = false;

            grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdResult.Visible = true;

            if (grdResult.TotalPages > 0)
            {
                //
                // I don't want to do either of these things, if I'm being called from a child form
                // that's just been saved..
//              grdResult.BringToFront();
//              grdResult.Focus();

                // Highlight first Row
                grdResult.Selection.SelectRow(1, true);

                // Make the Grid respond on updown keys
                UpdateDisplayedBalance();
                RefreshSumTagged(null, null);
            }

            ActionEnabledEvent(null, new ActionEventArgs("cndSelectedSupplier", grdResult.TotalPages > 0));
        }

        /// <summary>
        /// This will re-draw the form, so that any data changes are shown.
        /// </summary>
        public void Reload()
        {
            for (int i = 0; i < grdResult.Columns.Count; i++)
            {
                ColumnWidth[i] = grdResult.Columns[i].Width;
            }

            LoadSupplier(FLedgerNumber, FPartnerKey);
        }

        private void DoRefreshButton(Object Sender, EventArgs e)
        {
            Reload();
        }

        // Called from a timer, below, so that the default processing of
        // the grid control completes before I get called.
        private void RefreshSumTagged(Object Sender, EventArgs e)
        {
            // If I was called from a timer, kill that now:
            if (Sender != null)
            {
                ((System.Windows.Forms.Timer)Sender).Stop();
            }

            // if there's no results table yet, I can't do this...
            if (FPagedDataTable == null)
            {
                return;
            }

            bool TaggedInvoicesPostable = false;
            bool TaggedInvoicesPayable = false;
            Decimal TotalSelected = 0;
            bool ListHasItems = false;

            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                DataRow Row = rv.Row;
                Boolean IsMyCurrency = (Row["Currency"].ToString() == txtSupplierCurrency.Text);

                if (IsMyCurrency)
                {
                    ListHasItems = true;
                }

                if (IsMyCurrency && Row["Tagged"].Equals(true))
                {
                    // If it's a payment or a credit note, I'll subract it
                    // If it's an invoice, I'll add it!
                    // (The payment of a credit note appears on this list as a negative number, so it's always subracted.)
                    //
                    if (Row["CreditNote"].Equals(true))  // Payments also carry this "Credit note" label
                    {
                        TotalSelected -= (Decimal)(Row["Amount"]);
                    }
                    else
                    {
                        TotalSelected += (Decimal)(Row["Amount"]);
                    }

                    //
                    // While I'm in this loop, I'll also check whether to enable the "Pay" and "Post" buttons.
                    //
                    if ("|POSTED|PARTPAID|".IndexOf("|" + Row["Status"].ToString()) >= 0)
                    {
                        TaggedInvoicesPayable = true;
                    }

                    if ("|POSTED|PARTPAID|PAID|".IndexOf(Row["Status"].ToString()) < 0)
                    {
                        TaggedInvoicesPostable = true;
                    }
                }
            }

            txtSumOfTagged.Text = TotalSelected.ToString("n2") + " " + FSupplierRow.CurrencyCode;

            ActionEnabledEvent(null, new ActionEventArgs("actAddTaggedToPayment", TaggedInvoicesPayable));
            ActionEnabledEvent(null, new ActionEventArgs("actPostTagged", TaggedInvoicesPostable));
            ActionEnabledEvent(null, new ActionEventArgs("actTagAllPostable", ListHasItems));
            ActionEnabledEvent(null, new ActionEventArgs("actTagAllPayable", ListHasItems));
            ActionEnabledEvent(null, new ActionEventArgs("actUntagAll", ListHasItems));
        }

        private void grdResult_Click(object sender, EventArgs e)
        {
            // I want to update the total tagged field,
            // but it needs to be performed AFTER the default processing so I'm using a timer.
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

            timer.Tick += new EventHandler(RefreshSumTagged);
            timer.Interval = 100;
            timer.Start();
        }

        private void StatusFilterChange(object sender, EventArgs e)
        {
            String SelectedItem = cmbStatus.SelectedItem.ToString();

            if (SelectedItem == "All")
            {
                FStatusFilter = "";
            }
            else
            {
                FStatusFilter = "(Status = '' OR Status='" + SelectedItem + "')";
            }

            if (chkHideAgedTransactions.Checked)
            {
                if (FStatusFilter != "")
                {
                    FStatusFilter += " AND ";
                }
                FStatusFilter += ("(Date >'" + FAgedOlderThan + "')");
            }
            UpdateRowFilter();
        }

        private void TypeFilterChange(object sender, EventArgs e)
        {
            String SelectedItem = cmbType.SelectedItem.ToString();

            if (SelectedItem == "All")
            {
                FTypeFilter = "";
            }
            else
            {
                FTypeFilter = "Type='" + SelectedItem + "'";
            }

            UpdateRowFilter();
        }

        private void TagAllPostable(object sender, EventArgs e)
        {
            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                DataRow Row = rv.Row;

                if ((Row["Currency"].ToString() == txtSupplierCurrency.Text) && (Row["Status"].ToString().Length > 0)
                    && ("|POSTED|PARTPAID|PAID|".IndexOf("|" + Row["Status"].ToString()) < 0))
                {
                    Row["Tagged"] = true;
                }
            }

            RefreshSumTagged(null, null);
        }

        private void TagAllPayable(object sender, EventArgs e)
        {
            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                DataRow Row = rv.Row;

                if ((Row["Currency"].ToString() == txtSupplierCurrency.Text) && (Row["Status"].ToString().Length > 0)
                    && ("|POSTED|PARTPAID|".IndexOf("|" + Row["Status"].ToString()) >= 0))
                {
                    Row["Tagged"] = true;
                }
            }

            RefreshSumTagged(null, null);
        }

        private void UntagAll(object sender, EventArgs e)
        {
            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                DataRow Row = rv.Row;
                Row["Tagged"] = false;
            }

            RefreshSumTagged(null, null);
        }

        private void OpenSelectedTransaction(System.Object sender, EventArgs args)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                if (SelectedGridRow[0]["Status"].ToString().Length > 0) // invoices have status, and payments don't.
                {
                    Int32 DocumentId = Convert.ToInt32(SelectedGridRow[0]["DocumentId"]);
                    TFrmAPEditDocument frm = new TFrmAPEditDocument(this);
                    frm.LoadAApDocument(FLedgerNumber, DocumentId);
                    frm.Show();
                }
                else
                {
                    Int32 PaymentNumber = Convert.ToInt32(SelectedGridRow[0]["ApNum"]);
                    TFrmAPPayment frm = new TFrmAPPayment(this);
                    frm.ReloadPayment(FLedgerNumber, PaymentNumber);
                    frm.Show();
                }
            }
        }

        private void ReverseSelected(object sender, EventArgs e)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                if (SelectedGridRow[0]["Status"].ToString().Length > 0) // invoices have status, and payments don't.
                {  // Reverse invoice to a previous (unposted) state
                    string barstatus = "|" + SelectedGridRow[0]["Status"].ToString();

                    if (barstatus == "|POSTED")
                    {
                        TVerificationResultCollection Verifications;
                        Int32 DocumentId = Convert.ToInt32(SelectedGridRow[0]["DocumentId"]);
                        List <Int32>ApDocumentIds = new List <Int32>();
                        ApDocumentIds.Add(DocumentId);

                        TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                            FLedgerNumber,
                            Catalog.GetString("Select reversal date"),
                            Catalog.GetString("The date effective for this reversal") + ":");

                        if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
                        {
                            MessageBox.Show(Catalog.GetString("Reversal was cancelled."), Catalog.GetString(
                                    "No Success"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        DateTime PostingDate = dateEffectiveDialog.SelectedDate;

                        if (TRemote.MFinance.AP.WebConnectors.PostAPDocuments(
                                FLedgerNumber,
                                ApDocumentIds,
                                PostingDate,
                                true,
                                out Verifications))
                        {
                            System.Windows.Forms.MessageBox.Show("Ivoice reversed to Approved status.", Catalog.GetString("Reversal"));
                            return;
                        }
                        else
                        {
                            string ErrorMessages = String.Empty;

                            foreach (TVerificationResult verif in Verifications)
                            {
                                ErrorMessages += "[" + verif.ResultContext + "] " +
                                                 verif.ResultTextCaption + ": " +
                                                 verif.ResultText + Environment.NewLine;
                            }

                            System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Reversal"));
                        }

                        return;
                    } // reverse posted invoice

                    if ("|PAID|PARTPAID".IndexOf(barstatus) >= 0)
                    {
                        MessageBox.Show("Can't reverse paid a invoice. Reverse the payment instead.", "Reverse");
                    }
                }
                else // Reverse payment
                {
                    Int32 PaymentNum = (Int32)SelectedGridRow[0]["ApNum"];
                    TFrmAPPayment PaymentScreen = new TFrmAPPayment(this);
                    PaymentScreen.ReversePayment(FLedgerNumber, PaymentNum);
                }
            }
        }

        /// <summary>
        /// Create a new invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInvoice(object sender, EventArgs e)
        {
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, false);
            frm.Show();
        }

        /// <summary>
        /// Create a new credit note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCreditNote(object sender, EventArgs e)
        {
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, true);
            frm.Show();
        }

        /// <summary>
        /// Post all tagged documents in one GL Batch
        /// Uses static functions from TFrmAPEditDocument
        /// </summary>
        private void PostTaggedDocuments(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();
            AccountsPayableTDS TempDS = new AccountsPayableTDS();

            foreach (DataRow row in FPagedDataTable.Rows)
            {
                if ((row["Tagged"].Equals(true)) && (row["Status"].ToString().Length > 0)   // Invoices have status, Payments don't.
                    && ("|POSTED|PARTPAID|PAID".IndexOf("|" + row["Status"].ToString()) < 0)
                    && (row["Currency"].ToString() == txtSupplierCurrency.Text)
                    )
                {
                    Int32 DocumentId = Convert.ToInt32(row["DocumentId"]);
                    TempDS.Merge(TRemote.MFinance.AP.WebConnectors.LoadAApDocument(FLedgerNumber, DocumentId));

                    // I've loaded this record in my DS, but I was not given a handle to it, so I need to find it!
                    TempDS.AApDocument.DefaultView.Sort = "a_ap_document_id_i";
                    Int32 Idx = TempDS.AApDocument.DefaultView.Find(DocumentId);
                    AApDocumentRow DocumentRow = TempDS.AApDocument[Idx];

                    if (TFrmAPEditDocument.ApDocumentCanPost(TempDS, DocumentRow))
                    {
                        TaggedDocuments.Add(DocumentId);
                    }
                }
            }

            if (TaggedDocuments.Count == 0)
            {
                return;
            }

            if (TFrmAPEditDocument.PostApDocumentList(TempDS, FLedgerNumber, TaggedDocuments))
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP documents have been posted successfully!"));

                // TODO: show posting register of GL Batch?

                LoadSupplier(FLedgerNumber, FPartnerKey);
            }
        }

        /// Add all selected invoices to the payment list and show that list so that the user can make the payment
        private void AddTaggedToPayment(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();
            AccountsPayableTDS TempDS = new AccountsPayableTDS();

            foreach (DataRow row in FPagedDataTable.Rows)
            {
                if ((row["Tagged"].Equals(true)) && (!row["InvNum"].Equals("Payment")) && (row["Currency"].ToString() == txtSupplierCurrency.Text))
                {
                    // TODO: only use tagged rows that can be paid
                    Int32 DocumentId = Convert.ToInt32(row["DocumentId"]);
                    TempDS.Merge(TRemote.MFinance.AP.WebConnectors.LoadAApDocument(FLedgerNumber, DocumentId));

                    // I've loaded this record in my DS, but I was not given a handle to it, so I need to find it!
                    TempDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();
                    Int32 Idx = TempDS.AApDocument.DefaultView.Find(DocumentId);
                    AApDocumentRow DocumentRow = TempDS.AApDocument[Idx];

                    if ("|POSTED|PARTPAID|".IndexOf("|" + DocumentRow["a_document_status_c"].ToString()) >= 0)
                    {
                        TaggedDocuments.Add(DocumentId);
                    }
                }
            }

            if (TaggedDocuments.Count == 0)
            {
                return;
            }

            TFrmAPPayment frm = new TFrmAPPayment(this);

            frm.AddDocumentsToPayment(TempDS, FLedgerNumber, TaggedDocuments);

            frm.Show();
        }

        private void PaymentReport(object sender, EventArgs e)
        {
            TFrmAP_PaymentReport reporter = new TFrmAP_PaymentReport(this);

            reporter.Show();
        }
    }
}