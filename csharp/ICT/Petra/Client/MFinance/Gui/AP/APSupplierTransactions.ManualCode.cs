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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Shared.Interfaces.MFinance;
using System.Threading;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;

//using System;
//using System.Drawing;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//using System.Resources;
//using System.Collections.Specialized;
//using GNU.Gettext;
//using Ict.Common;
//using Ict.Common.Data;
//using Ict.Common.Verification;
//using Ict.Common.Controls;
//using Ict.Petra.Shared;
//using Ict.Petra.Shared.MFinance.AP.Data;
//using Ict.Petra.Shared.MPartner.Partner.Data;
//using Ict.Petra.Client.App.Core;
//using Ict.Petra.Client.App.Core.RemoteObjects;
//using Ict.Petra.Client.CommonForms;
//using Ict.Petra.Client.CommonControls;
//using Ict.Petra.Client.MCommon;
//using Ict.Petra.Client.MFinance.Gui.GL;
//using Ict.Petra.Client.MFinance.Gui.Setup;
//using Ict.Petra.Shared.Interfaces.MFinance;
//using System.Threading;
//using Ict.Common.Conversion;
//using Ict.Petra.Client.MReporting.Gui;
//using Ict.Petra.Client.MReporting.Gui.MFinance;
//using Ict.Petra.Client.MFinance.Logic;
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

        private Boolean FRequireApprovalBeforePosting = false;

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;


        private String FTypeFilter = "";    // filter which types of transactions are shown
        private String FStatusFilter = "";  // filter the status of invoices
        private String FHistoryFilter = "";  // filter the status of history
        private string FAgedOlderThan;

        private TSgrdDataGridPaged grdDetails;
        private int FPrevRowChangedRow = -1;
        private DataRow FPreviouslySelectedDetailRow = null;

        private void InitializeManualCode()
        {
            grdDetails = grdResult;

            grdResult.MouseClick += new MouseEventHandler(grdResult_Click);
            grdResult.DataPageLoaded += new TDataPageLoadedEventHandler(grdResult_DataPageLoaded);
            grdResult.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(grdResult_SelectionChanged);

            FPetraUtilsObject.SetStatusBarText(grdDetails,
                Catalog.GetString("Use the navigation keys to select a transaction.  Double-click to view the details"));
            FPetraUtilsObject.SetStatusBarText(btnAddTaggedToPayment, Catalog.GetString("Click to pay the tagged items"));
            FPetraUtilsObject.SetStatusBarText(btnApproveTagged, Catalog.GetString("Click to approve the tagged items"));
            FPetraUtilsObject.SetStatusBarText(btnPostTagged, Catalog.GetString("Click to post the tagged items"));
            FPetraUtilsObject.SetStatusBarText(btnReloadList, Catalog.GetString("Click to re-load the transactions list"));
            FPetraUtilsObject.SetStatusBarText(btnTagAll, Catalog.GetString("Click to tag all the displayed items"));
            FPetraUtilsObject.SetStatusBarText(btnUntagAll, Catalog.GetString("Click to un-tag all the displayed items"));
            FPetraUtilsObject.SetStatusBarText(chkToggleFilter, Catalog.GetString("Click to show/hide the Filter/Find panel"));
        }

        private void grdResult_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            FPrevRowChangedRow = grdResult.Selection.ActivePosition.Row;
            SetEnabledStates();
        }

        private void SetEnabledStates()
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                string status = SelectedGridRow[0]["Status"].ToString();

                // Payments can be reversed as can posted invoices
                ActionEnabledEvent(null, new ActionEventArgs("actReverseSelected", (status == "POSTED") || (status.Length == 0)));
                ActionEnabledEvent(null, new ActionEventArgs("actDeleteSelected", (status == "OPEN") || (status == "APPROVED")));
            }
            else
            {
                ActionEnabledEvent(null, new ActionEventArgs("actReverseSelected", false));
                ActionEnabledEvent(null, new ActionEventArgs("actDeleteSelected", false));
            }
        }

        /// <summary>
        /// Method required by IGridBase.
        /// </summary>
        public void SelectRowInGrid(int ARowNumber)
        {
            if (ARowNumber >= grdDetails.Rows.Count)
            {
                ARowNumber = grdDetails.Rows.Count - 1;
            }

            if ((ARowNumber < 1) && (grdDetails.Rows.Count > 1))
            {
                ARowNumber = 1;
            }

            // Note:  We need to be sure to focus column 1 in this case because sometimes column 0 is not visible!!
            grdDetails.Selection.Focus(new SourceGrid.Position(ARowNumber, 1), true);
            FPrevRowChangedRow = ARowNumber;
        }

        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdDetails.DataSource != null)
            {
                int totalTableRecords = grdResult.TotalRecords;
                int totalGridRecords = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;

                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount) + String.Format(" ({0})", totalTableRecords);

                SetRecordNumberDisplayProperties();
                UpdateDisplayedBalance();
            }
        }

        private void ApplyFilterManual(ref string AFilter)
        {
            if (FPagedDataTable != null)
            {
                FPagedDataTable.DefaultView.RowFilter = AFilter;
            }

            bool gotRows = (grdDetails.Rows.Count > 1);
            bool canApprove = ((RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForApproval")).Checked && gotRows;
            bool canPost = ((RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForPosting")).Checked && gotRows;
            bool canPay = ((RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForPaying")).Checked && gotRows;

            bool canTag = canApprove || canPost || canPay;

            ActionEnabledEvent(null, new ActionEventArgs("actOpenTagged", canTag));
            ActionEnabledEvent(null, new ActionEventArgs("actApproveTagged", canApprove));
            ActionEnabledEvent(null, new ActionEventArgs("actPostTagged", canPost));
            ActionEnabledEvent(null, new ActionEventArgs("actAddTaggedToPayment", canPay));
            ActionEnabledEvent(null, new ActionEventArgs("actTagAll", canTag));
            ActionEnabledEvent(null, new ActionEventArgs("actUntagAll", canTag));

            grdDetails.Columns[0].Visible = canTag;

            if (canTag)
            {
                grdDetails.ShowCell(new SourceGrid.Position(grdDetails.Selection.ActivePosition.Row, 0), true);
            }
        }

        private bool IsMatchingRowManual(DataRow ARow)
        {
            string transactionType = ((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbTransactionType")).Text;

            if (transactionType != String.Empty)
            {
                if (!ARow["Type"].ToString().Contains(transactionType))
                {
                    return false;
                }
            }

            string status = ((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbStatus")).Text;

            if (status != String.Empty)
            {
                if (!ARow["Status"].ToString().Contains(status))
                {
                    return false;
                }
            }

            DateTime dt;
            TtxtPetraDate fromDate = (TtxtPetraDate)FFilterAndFindObject.FindPanelControls.FindControlByName("dtpDate-1");

            if ((fromDate.Text != String.Empty) && DateTime.TryParse(fromDate.Text, out dt))
            {
                if (Convert.ToDateTime(ARow["Date"]) < dt.Date)
                {
                    return false;
                }
            }

            TtxtPetraDate toDate = (TtxtPetraDate)FFilterAndFindObject.FindPanelControls.FindControlByName("dtpDate-2");

            if ((toDate.Text != String.Empty) && DateTime.TryParse(toDate.Text, out dt))
            {
                if (Convert.ToDateTime(ARow["Date"]) > dt.Date)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Load the supplier and all the transactions (invoices and payments) that relate to it.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        public void LoadSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            this.Cursor = Cursors.WaitCursor;

            FLedgerNumber = ALedgerNumber;
            FPartnerKey = APartnerKey;
            FMainDS = TRemote.MFinance.AP.WebConnectors.LoadAApSupplier(ALedgerNumber, APartnerKey);

            FSupplierRow = FMainDS.AApSupplier[0];

            // Get our AP ledger settings and enable/disable the corresponding search option on the filter panel
            TFrmLedgerSettingsDialog settings = new TFrmLedgerSettingsDialog(this, ALedgerNumber);
            FRequireApprovalBeforePosting = settings.APRequiresApprovalBeforePosting;
            Control rbtForApproval = FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForApproval");
            rbtForApproval.Enabled = FRequireApprovalBeforePosting;

            //
            // Transactions older than
            DateTime AgedOlderThan = DateTime.Now;

            if (!FSupplierRow.IsPreferredScreenDisplayNull())
            {
                AgedOlderThan = AgedOlderThan.AddMonths(0 - FSupplierRow.PreferredScreenDisplay);
            }

            FAgedOlderThan = AgedOlderThan.ToString("u");

            txtSupplierName.Text = FMainDS.PPartner[0].PartnerShortName;
            txtSupplierCurrency.Text = FSupplierRow.CurrencyCode;
            FFindObject = TRemote.MFinance.AP.UIConnectors.Find();

            FFindObject.FindSupplierTransactions(FLedgerNumber, FPartnerKey);

            // Start thread that checks for the end of the search operation on the PetraServer
            FKeepUpSearchFinishedCheck = true;
            Thread FinishedCheckThread = new Thread(new ThreadStart(SearchFinishedCheckThread));
            FinishedCheckThread.Start();

            this.Text = Catalog.GetString("Supplier Transactions") + " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private void grdResult_DataPageLoaded(object Sender, TDataPageLoadEventArgs e)
        {
            // This is where we end up after querying the database and loading the first data into the grid
            // We are back in our main thread here
            this.Cursor = Cursors.Default;
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
            TAsyncExecProgressState ThreadStatus;

            // Check whether this thread should still execute
            while (FKeepUpSearchFinishedCheck)
            {
                // Wait and see if anything has changed
                Thread.Sleep(200);

                try
                {
                    /* The next line of code calls a function on the PetraServer
                     * > causes a bit of data traffic everytime! */
                    ThreadStatus = FFindObject.AsyncExecProgress.ProgressState;
                }
                catch (NullReferenceException)
                {
                    // The form is closing on the main thread ...
                    return;         // end this thread
                }
                catch (Exception)
                {
                    throw;
                }

                switch (ThreadStatus)
                {
                    case TAsyncExecProgressState.Aeps_Finished:
                        FKeepUpSearchFinishedCheck = false;

                        try
                        {
                            // see also http://stackoverflow.com/questions/6184/how-do-i-make-event-callbacks-into-my-win-forms-thread-safe
                            if (InvokeRequired)
                            {
                                Invoke(new SimpleDelegate(FinishThread));
                            }
                            else
                            {
                                FinishThread();
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            // Another exception that can be caused when the main screen is closed while running this thread
                            return;
                        }

                        break;

                    case TAsyncExecProgressState.Aeps_Stopped:
                        FKeepUpSearchFinishedCheck = false;
                        return;
                }

                // Loop again while FKeepUpSearchFinishedCheck is true ...
            }
        }

        private void FinishThread()
        {
            // Fetch the first page of data
            try
            {
                grdResult.MinimumPageSize = 200;
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
            UpdateRowFilter();
            string currentFilter = FFilterAndFindObject.CurrentActiveFilter;
            ApplyFilterManual(ref currentFilter);

            if (grdResult.TotalPages > 0)
            {
                //
                // I don't want to do either of these things, if I'm being called from a child form
                // that's just been saved..
                //              grdResult.BringToFront();
                //              grdResult.Focus();

                if (grdResult.TotalPages > 1)
                {
                    grdResult.LoadAllDataPages();
                }

                // Highlight first Row
                SelectRowInGrid(1);
            }

            grdResult.AutoResizeGrid();

            UpdateSupplierBalance();
            UpdateDisplayedBalance();
            UpdateRecordNumberDisplay();
            RefreshSumTagged(null, null);

            ActionEnabledEvent(null, new ActionEventArgs("cndSelectedSupplier", grdResult.TotalPages > 0));
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
            ResultsTable.Columns.Add("Tagged", typeof(bool));

            foreach (DataRow Row in ResultsTable.Rows)
            {
                Row["Tagged"] = false;

                if ((Row["Type"].ToString() == "Invoice") && (Row["CreditNote"].Equals(true)))
                {
                    Row["Type"] = "Credit Note";
                }

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

            return ResultsTable;
        }

        private void InitialiseGrid()
        {
            grdResult.Columns.Clear();
            grdResult.AddCheckBoxColumn("", FPagedDataTable.Columns["Tagged"], -1, false);
//          grdResult.AddTextColumn("AP#", FPagedDataTable.Columns["ApNum"]);
            grdResult.AddTextColumn("Inv#", FPagedDataTable.Columns["InvNum"]);
            grdResult.AddTextColumn("Type", FPagedDataTable.Columns["Type"]);
            grdResult.AddCurrencyColumn("Amount", FPagedDataTable.Columns["Amount"]);
            grdResult.AddCurrencyColumn("Outstanding", FPagedDataTable.Columns["OutstandingAmount"]);
            grdResult.AddTextColumn("Currency", FPagedDataTable.Columns["Currency"]);
//          grdResult.AddTextColumn("Discount", FPagedDataTable.Columns["DiscountMsg"]);
            grdResult.AddTextColumn("Status", FPagedDataTable.Columns["Status"]);
            grdResult.AddDateColumn("Date", FPagedDataTable.Columns["Date"]);
        }

        private void UpdateDisplayedBalance()
        {
            DevAge.ComponentModel.BoundDataView dv = (DevAge.ComponentModel.BoundDataView)grdResult.DataSource;
            txtFilteredBalance.Text = UpdateBalance(dv.DataView).ToString("n2") + " " + FSupplierRow.CurrencyCode;
        }

        private void UpdateSupplierBalance()
        {
            DataView dv = new DataView(FPagedDataTable);

            txtSupplierBalance.Text = UpdateBalance(dv).ToString("n2") + " " + FSupplierRow.CurrencyCode;
        }

        private Decimal UpdateBalance(DataView ADataView)
        {
            Decimal balance = 0.0m;

            if (FPagedDataTable != null)
            {
                foreach (DataRowView rv in ADataView)
                {
                    DataRow Row = rv.Row;

                    if ((Row["Currency"].ToString() == txtSupplierCurrency.Text) && (Row["OutstandingAmount"].GetType() == typeof(Decimal)))
                    {
                        if (Row["CreditNote"].Equals(true))  // Payments also carry this "Credit note" label
                        {
                            balance -= (Decimal)Row["OutstandingAmount"];
                        }
                        else
                        {
                            balance += (Decimal)Row["OutstandingAmount"];
                        }
                    }
                }
            }

            return balance;
        }

        private void UpdateRowFilter()
        {
            if (FPagedDataTable != null)
            {
                string filter = String.Format("(Currency='{0}')", txtSupplierCurrency.Text);
                string filterJoint = " AND ";

                if ((FStatusFilter.Length > 0) && (filter.Length > 0))
                {
                    filter += filterJoint;
                }

                filter += FStatusFilter;

                if ((FTypeFilter.Length > 0) && (filter.Length > 0))
                {
                    filter += filterJoint;
                }

                filter += FTypeFilter;

                if ((FHistoryFilter.Length > 0) && (filter.Length > 0))
                {
                    filter += filterJoint;
                }

                filter += FHistoryFilter;

                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(filter, filter.Length == 0);
            }
        }

        /// <summary>
        /// This will re-draw the form, so that any data changes are shown.
        /// </summary>
        public void Reload()
        {
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

            FPrevRowChangedRow = grdResult.Selection.ActivePosition.Row;

            Decimal TotalSelected = 0;
            int TaggedItemCount = 0;

            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                DataRow Row = rv.Row;

                if (Row["Tagged"].Equals(true))
                {
                    TaggedItemCount++;

                    if (Row["Type"].ToString() != "Payment")
                    {
                        // If it's a credit note, I'll subract it
                        // If it's an invoice, I'll add it!
                        //
                        if (Row["CreditNote"].Equals(true))
                        {
                            TotalSelected -= (Decimal)(Row["OutstandingAmount"]);
                        }
                        else
                        {
                            TotalSelected += (Decimal)(Row["OutstandingAmount"]);
                        }
                    }
                }
            }

            txtTaggedBalance.Text = TotalSelected.ToString("n2") + " " + FSupplierRow.CurrencyCode;
            txtTaggedCount.Text = TaggedItemCount.ToString();
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
            FStatusFilter = String.Empty;
            string filterJoint = " AND ";

            String SelectedItem = ((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbStatus")).Text;

            if (SelectedItem != String.Empty)
            {
                FStatusFilter = "(Status='" + SelectedItem + "')";
            }

            RadioButton rbtForApproval = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForApproval");

            if (rbtForApproval.Checked)
            {
                if (FStatusFilter != String.Empty)
                {
                    FStatusFilter += filterJoint;
                }

                FStatusFilter += ("(Status='OPEN')");
            }

            RadioButton rbtForPosting = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForPosting");

            if (rbtForPosting.Checked)
            {
                if (FStatusFilter != String.Empty)
                {
                    FStatusFilter += filterJoint;
                }

                if (FRequireApprovalBeforePosting)
                {
                    FStatusFilter += ("(Status='APPROVED')");
                }
                else
                {
                    FStatusFilter += ("(Status='OPEN' OR Status='APPROVED')");
                }
            }

            RadioButton rbtForPaying = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtForPaying");

            if (rbtForPaying.Checked)
            {
                if (FStatusFilter != String.Empty)
                {
                    FStatusFilter += filterJoint;
                }

                FStatusFilter += ("(Status='POSTED' OR Status='PARTPAID')");
            }

            UpdateRowFilter();
        }

        private void TypeFilterChange(object sender, EventArgs e)
        {
            FTypeFilter = "";

            String SelectedItem = ((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbTransactionType")).Text;

            if (SelectedItem != String.Empty)
            {
                FTypeFilter = "Type='" + SelectedItem + "'";
            }

            UpdateRowFilter();
        }

        private void HistoryFilterChange(object sender, EventArgs e)
        {
            FHistoryFilter = String.Empty;

            RadioButton rbtRecent = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtRecent");

            if (rbtRecent.Checked)
            {
                FHistoryFilter = ("(Date >'" + FAgedOlderThan + "')");
            }

            RadioButton rbtQuarter = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtLastQuarter");

            if (rbtQuarter.Checked)
            {
                if (FHistoryFilter != "")
                {
                    FHistoryFilter += " AND ";
                }

                FHistoryFilter += ("(Date > #" + DateTime.Now.AddMonths(-3).ToString("d", System.Globalization.CultureInfo.InvariantCulture) + "#)");
            }

            RadioButton rbtHalf = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtLastSixMonths");

            if (rbtHalf.Checked)
            {
                if (FHistoryFilter != "")
                {
                    FHistoryFilter += " AND ";
                }

                FHistoryFilter += ("(Date > #" + DateTime.Now.AddMonths(-6).ToString("d", System.Globalization.CultureInfo.InvariantCulture) + "#)");
            }

            RadioButton rbtYear = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtLastYear");

            if (rbtYear.Checked)
            {
                if (FHistoryFilter != "")
                {
                    FHistoryFilter += " AND ";
                }

                FHistoryFilter += ("(Date > #" + DateTime.Now.AddMonths(-12).ToString("d", System.Globalization.CultureInfo.InvariantCulture) + "#)");
            }

            UpdateRowFilter();
        }

        private void TagAll(object sender, EventArgs e)
        {
            // Untag everything
            UntagAll(null, null);

            // Now tag all the rows in the current view
            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                rv.Row["Tagged"] = true;
            }

            RefreshSumTagged(null, null);
        }

        private void UntagAll(object sender, EventArgs e)
        {
            // We do this for all tags in the complete data table
            foreach (DataRow Row in FPagedDataTable.Rows)
            {
                Row["Tagged"] = false;
            }

            RefreshSumTagged(null, null);
        }

        // Opens an individual document or payment
        private void OpenADocumentOrPayment(DataRowView ADataRow)
        {
            if (ADataRow["Status"].ToString().Length > 0) // invoices have status, and payments don't.
            {
                Int32 DocumentId = Convert.ToInt32(ADataRow["ApDocumentId"]);
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

                if (frm.LoadAApDocument(FLedgerNumber, DocumentId))
                {
                    frm.Show();
                }
            }
            else
            {
                Int32 PaymentNumber = Convert.ToInt32(ADataRow["ApNum"]);
                TFrmAPPayment frm = new TFrmAPPayment(this);
                frm.ReloadPayment(FLedgerNumber, PaymentNumber);
                frm.Show();
            }
        }

        // Opens all tagged documents
        private void OpenTaggedDocuments(System.Object sender, EventArgs args)
        {
            if (FPagedDataTable.DefaultView.Count > 0)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (DataRowView rv in FPagedDataTable.DefaultView)
                {
                    if (rv.Row["Tagged"].Equals(true))
                    {
                        OpenADocumentOrPayment(rv);
                    }
                }

                this.Cursor = Cursors.Default;
            }
        }

        // Open the highlighted transaction - called by menu or grid click etc
        private void OpenSelectedTransaction(System.Object sender, EventArgs args)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                this.Cursor = Cursors.WaitCursor;
                OpenADocumentOrPayment(SelectedGridRow[0]);
                this.Cursor = Cursors.Default;
            }
        }

        private void DeleteSelected(object sender, EventArgs e)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                // I can only delete invoices that are not posted already.
                // This method is only enabled when the grid shows items for Posting
                List <int>DeleteTheseDocs = new List <int>();

                string status = SelectedGridRow[0]["Status"].ToString();

                if ((status == "OPEN") || (status == "APPROVED"))
                {
                    Int32 DocumentId = Convert.ToInt32(SelectedGridRow[0]["ApDocumentId"]);
                    DeleteTheseDocs.Add(DocumentId);

                    this.Cursor = Cursors.WaitCursor;
                    TRemote.MFinance.AP.WebConnectors.DeleteAPDocuments(FLedgerNumber, DeleteTheseDocs);
                    this.Cursor = Cursors.Default;
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
                        Int32 DocumentId = Convert.ToInt32(SelectedGridRow[0]["ApDocumentId"]);
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
                            System.Windows.Forms.MessageBox.Show("Invoice reversed to Approved status.", Catalog.GetString("Reversal"));
                            Reload();
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
                        MessageBox.Show("Can't reverse a paid invoice. Reverse the payment instead.", "Reverse");
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
            this.Cursor = Cursors.WaitCursor;

            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, false);
            frm.Show();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Create a new credit note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCreditNote(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, true);
            frm.Show();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Approve all tagged documents
        /// Uses static functions from TFrmAPEditDocument??
        /// Not yet implemented
        /// </summary>
        private void ApproveTaggedDocuments(object sender, EventArgs e)
        {
            string MsgTitle = Catalog.GetString("Document Approval");

            List <Int32>TaggedDocuments = new List <Int32>();

            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                if ((rv.Row["Tagged"].Equals(true)) && (rv.Row["Status"].ToString().Length > 0)   // Invoices have status, Payments don't.
                    && (rv.Row["Status"].ToString() == "OPEN")
                    && (rv.Row["Currency"].ToString() == txtSupplierCurrency.Text)
                    )
                {
                    TaggedDocuments.Add((int)rv.Row["ApDocumentId"]);
                }
            }

            if (TaggedDocuments.Count > 0)
            {
                string msg = String.Format(Catalog.GetString(
                        "Are you sure that you want to approve the {0} tagged document(s)?"), TaggedDocuments.Count);

                if (MessageBox.Show(msg, MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                TVerificationResultCollection verificationResult;

                if (TRemote.MFinance.AP.WebConnectors.ApproveAPDocuments(FLedgerNumber, TaggedDocuments, out verificationResult))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(Catalog.GetString("The tagged documents have been approved successfully!"), MsgTitle);
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(verificationResult.BuildVerificationResultString(), MsgTitle);
                }
            }
            else
            {
                MessageBox.Show(Catalog.GetString("There we no tagged documents to be approved."), MsgTitle);
            }
        }

        /// <summary>
        /// Post all tagged documents in one GL Batch
        /// Uses static functions from TFrmAPEditDocument
        /// </summary>
        private void PostTaggedDocuments(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();
            AccountsPayableTDS TempDS = new AccountsPayableTDS();

            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                if ((rv.Row["Tagged"].Equals(true)) && (rv.Row["Status"].ToString().Length > 0)   // Invoices have status, Payments don't.
                    && ("|POSTED|PARTPAID|PAID".IndexOf("|" + rv.Row["Status"].ToString()) < 0)
                    && (rv.Row["Currency"].ToString() == txtSupplierCurrency.Text)
                    )
                {
                    Int32 DocumentId = Convert.ToInt32(rv.Row["ApDocumentId"]);
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

            if (TFrmAPEditDocument.PostApDocumentList(TempDS, FLedgerNumber, TaggedDocuments, this))
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

            foreach (DataRowView rv in FPagedDataTable.DefaultView)
            {
                if (
                    (rv.Row["Tagged"].Equals(true))
                    && (rv.Row["Currency"].ToString() == txtSupplierCurrency.Text)
                    && ("|POSTED|PARTPAID|".IndexOf("|" + rv.Row["Status"].ToString()) >= 0)
                    )
                {
                    Int32 DocumentId = Convert.ToInt32(rv.Row["ApDocumentId"]);
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

            if (frm.AddDocumentsToPayment(TempDS, FLedgerNumber, TaggedDocuments))
            {
                frm.Show();
            }
        }

        private void PaymentReport(object sender, EventArgs e)
        {
            Int32 PaymentNumStart = -1;
            Int32 PaymentNumEnd = -1;

            DataRowView[] SelectedGridRows = grdResult.SelectedDataRowsAsDataRowView;

            foreach (DataRowView RowView in SelectedGridRows)
            {
                DataRow Row = RowView.Row;
                Int32 PaymentNum = Convert.ToInt32(Row["ApNum"]);

                if ((PaymentNumStart == -1) || (PaymentNum < PaymentNumStart))
                {
                    PaymentNumStart = PaymentNum;
                }

                if (PaymentNum > PaymentNumEnd)
                {
                    PaymentNumEnd = PaymentNum;
                }
            }

            TFrmAP_PaymentReport reporter = new TFrmAP_PaymentReport(this);
            reporter.LedgerNumber = FLedgerNumber;
            reporter.SetPaymentNumber(PaymentNumStart, PaymentNumEnd);
            reporter.Show();
        }

        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.L | Keys.Control))
            {
                grdDetails.Focus();
                return true;
            }

            if (keyData == (Keys.Home | Keys.Control))
            {
                SelectRowInGrid(1);
                return true;
            }

            if (keyData == ((Keys.Up | Keys.Control)))
            {
                SelectRowInGrid(FPrevRowChangedRow - 1);
                return true;
            }

            if (keyData == (Keys.Down | Keys.Control))
            {
                SelectRowInGrid(FPrevRowChangedRow + 1);
                return true;
            }

            if (keyData == ((Keys.End | Keys.Control)))
            {
                SelectRowInGrid(grdDetails.Rows.Count - 1);
                return true;
            }

            return false;
        }

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>The Partner Edit 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcAPTransactionChanged)
            {
                // The message is relevant if the supplier name is empty (=any supplier) or our specific supplier
                if ((((TFormsMessage.FormsMessageAP)AFormsMessage.MessageObject).SupplierName == this.lblSupplierName.Text)
                    || (((TFormsMessage.FormsMessageAP)AFormsMessage.MessageObject).SupplierName == String.Empty))
                {
                    // Reload the screen data
                    Reload();
                }

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}