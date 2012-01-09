//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Conversion;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPMain
    {
        private IAPUIConnectorsFind FSupplierFindObject = null;
        private bool FKeepUpSearchFinishedCheck = false;
        private bool FSearchForSuppliers = false;
        private DataTable FInvoiceTable;
        private ALedgerRow FLedgerInfo;



        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;

        private Int32 FLedgerNumber;

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FSupplierFindObject = TRemote.MFinance.AP.UIConnectors.Find();
                ALedgerTable Tbl = FSupplierFindObject.GetLedgerInfo(FLedgerNumber);
                FLedgerInfo = Tbl[0];
            }
        }

        private String GetLedgerCurrency(Int32 ALedgerNumber)
        {
            return FLedgerInfo.BaseCurrency;
        }

        /// <summary>
        /// Search button was clicked
        /// </summary>
        public void DoSearch(object sender, EventArgs e)
        {
            if (FKeepUpSearchFinishedCheck)
            {
                // don't run several searches at the same time
                return;
            }

            FSearchForSuppliers = tpgSuppliers.Visible;

            DataTable CriteriaTable = new DataTable();
            CriteriaTable.Columns.Add("LedgerNumber", typeof(Int32));
            CriteriaTable.Columns.Add("SupplierId", typeof(string));

            decimal DaysPlus = -1;

            if (chkDueFuture.Checked)  // Calculate the future date to send to the server
            {
                DaysPlus = nudNumberTimeUnits.Value;

                if (cmbTimeUnit.SelectedText == "Months")
                {
                    DaysPlus *= 31;
                }
                else if (cmbTimeUnit.SelectedText == "Weeks")
                {
                    DaysPlus *= 7;
                }
            }
            else if (chkDueToday.Checked)
            {
                DaysPlus = 0;
            }

            CriteriaTable.Columns.Add("DaysPlus", typeof(decimal));
            DataRow row = CriteriaTable.NewRow();
            row["DaysPlus"] = DaysPlus;
            row["SupplierId"] = cmbSupplierCode.Text;
            row["LedgerNumber"] = FLedgerNumber;
            CriteriaTable.Rows.Add(row);

            // Start the asynchronous search operation on the PetraServer
            if (FSearchForSuppliers)
            {
                FSupplierFindObject.FindSupplier(CriteriaTable);
            }
            else
            {
                FSupplierFindObject.FindInvoices(CriteriaTable);
            }

            // Start thread that checks for the end of the search operation on the PetraServer
            FKeepUpSearchFinishedCheck = true;
            Thread FinishedCheckThread = new Thread(new ThreadStart(SearchFinishedCheckThread));
            FinishedCheckThread.Start();
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
                switch (FSupplierFindObject.AsyncExecProgress.ProgressState)
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
                        EnableDisableUI(true);
                        return;
                }

                // Sleep for some time. After that, this function is called again automatically.
                Thread.Sleep(200);
            }

            EnableDisableUI(true);
        }

        private void InitialiseGrid()
        {
            if (FSearchForSuppliers)
            {
                grdSupplierResult.Columns.Clear();
                grdSupplierResult.AddTextColumn("Supplier Key", FPagedDataTable.Columns[0], 90);
                grdSupplierResult.AddTextColumn("Supplier Name", FPagedDataTable.Columns[1], 250);
                grdSupplierResult.AddTextColumn("Currency", FPagedDataTable.Columns[2], 85);
            }
            else
            {
                grdInvoiceResult.Columns.Clear();
                grdInvoiceResult.AddCheckBoxColumn("", FPagedDataTable.Columns["Selected"],25,false);
                grdInvoiceResult.AddTextColumn("AP#", FPagedDataTable.Columns[0], 55);
                grdInvoiceResult.AddTextColumn("Inv#", FPagedDataTable.Columns[1], 90);
                grdInvoiceResult.AddTextColumn("Supplier", FPagedDataTable.Columns[2], 240);
                grdInvoiceResult.AddCurrencyColumn("Amount", FPagedDataTable.Columns[4],2);
                grdInvoiceResult.AddTextColumn("Currency", FPagedDataTable.Columns[3], 70);
                grdInvoiceResult.AddDateColumn("Due Date", FPagedDataTable.Columns[7]);
                grdInvoiceResult.AddTextColumn("Status", FPagedDataTable.Columns[5], 100);
                grdInvoiceResult.AddDateColumn("Issued", FPagedDataTable.Columns[6]);
                grdInvoiceResult.AddTextColumn("Discount", FPagedDataTable.Columns["DiscountMsg"], 150);

                grdInvoiceResult.Columns[4].Width = 90;  // Only the text columns can have their widths set while
                grdInvoiceResult.Columns[6].Width = 110; // they're being added. For these currency and date columns,
                grdInvoiceResult.Columns[8].Width = 110; // I need to set the width afterwards. (THIS WILL GO WONKY IF EXTRA FIELDS ARE ADDED ABOVE.)

                grdInvoiceResult.Click += new EventHandler(grdInvoiceResult_Click);
            }
        }

        void grdInvoiceResult_Click(object sender, EventArgs e) // this is OK, but it should be performed AFTER the default processing!
        {
            // Add up all the selected Items  ** I can only sum items that are in my currency! **
            String MyCurrency = GetLedgerCurrency(FLedgerNumber);

            Decimal TotalSelected = 0;
            foreach (DataRow Row in FInvoiceTable.Rows)
            {
                if ((Row["Selected"].Equals(true)) && (Row["Currency"].Equals(MyCurrency)))
                {
                    TotalSelected += (Decimal)(Row["a_total_amount_n"]);
                }
            }
            txtSumTagged.Text = TotalSelected.ToString(); // This needs formatting.
        }

        private void FinishThread()
        {
            // Fetch the first page of data
            try
            {
                if (FSearchForSuppliers)
                {
                    FPagedDataTable = grdSupplierResult.LoadFirstDataPage(@GetDataPagedResult);
                }
                else
                {
                    FPagedDataTable = grdInvoiceResult.LoadFirstDataPage(@GetDataPagedResult);
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
            InitialiseGrid();
            DataView myDataView = FPagedDataTable.DefaultView;
            myDataView.AllowNew = false;

            if (FSearchForSuppliers)
            {
                grdSupplierResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdSupplierResult.Visible = true;

                if (grdSupplierResult.TotalPages > 0)
                {
                    grdSupplierResult.BringToFront();

                    // Highlight first Row
                    grdSupplierResult.Selection.SelectRow(1, true);

                    // Make the Grid respond on updown keys
                    grdSupplierResult.Focus();
                }

                ActionEnabledEvent(null, new ActionEventArgs("cndSelectedSupplier", grdSupplierResult.TotalPages > 0));
            }
            else
            {
                grdInvoiceResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdInvoiceResult.Visible = true;

                if (grdInvoiceResult.TotalPages > 0)
                {
                    grdInvoiceResult.BringToFront();

                    // Highlight first Row
                    grdInvoiceResult.Selection.SelectRow(1, true);

                    // Make the Grid respond on updown keys
                    grdInvoiceResult.Focus();
                }

//              ActionEnabledEvent(null, new ActionEventArgs("cndSelectedDocument", grdInvoiceResult.TotalPages > 0));
            }
        }

        private void InitializeManualCode()
        {
            this.cmbSupplierCurrency.cmbCombobox.TextChanged += new System.EventHandler(this.SetSupplierFilters);
        }

        private void EnableDisableUI(bool AEnable)
        {
            // TODO: autogenerate?
        }

        /// <summary>
        /// todoComment
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

            if (FSupplierFindObject != null)
            {
                FInvoiceTable = FSupplierFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);

                if (!FSearchForSuppliers)
                {
                    FInvoiceTable.Columns.Add("DiscountMsg");
                    DataColumn Checkboxcolumn = new DataColumn("Selected", typeof(bool));
                    FInvoiceTable.Columns.Add(Checkboxcolumn);

                    foreach (DataRow Row in FInvoiceTable.Rows)
                    {
                        Row["DiscountMsg"] = "None";
                        Row["Selected"] = false;

                        if ((Row[8].GetType() == typeof(Decimal))
                          && (Row[9].GetType() == typeof(DateTime)))
                        {
                            Decimal DiscountPercent = (Decimal)Row[8];
                            DateTime DiscountUntil = (DateTime)Row[9];

                            if (DiscountUntil > DateTime.Now)
                            {
                                Row["DiscountMsg"] =
                                    String.Format("{0:n0}% until {1}", DiscountPercent, TDate.DateTimeToLongDateString2(DiscountUntil));
                            }
                            else
                            {
                                Row["DiscountMsg"] = "Expired";
                            }
                        }
                    }
                }

                return FInvoiceTable;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// get the partner key of the currently selected supplier in the grid
        /// </summary>
        /// <returns></returns>
        private Int64 GetCurrentlySelectedSupplier()
        {
            DataRowView[] SelectedGridRow = grdSupplierResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                Int64 SupplierKey = Convert.ToInt64(SelectedGridRow[0][0]);
                return SupplierKey;
            }

            return -1;
        }

        private Int32 GetCurrentlySelectedInvoice()
        {
            DataRowView[] SelectedGridRow = grdInvoiceResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                Int32 InvoiceNum = Convert.ToInt32(SelectedGridRow[0]["a_ap_number_i"]);
                return InvoiceNum;
            }

            return -1;
        }

        /// <summary>
        /// open the transactions of the selected supplier
        /// </summary>
        public void SupplierTransactions(object sender, EventArgs e)
        {
            TFrmAPSupplierTransactions frm = new TFrmAPSupplierTransactions(this);

            frm.LoadSupplier(FLedgerNumber, GetCurrentlySelectedSupplier());
            frm.Show();
        }


        /// <summary>
        /// Open the selected invoice
        /// </summary>
        public void ShowInvoice(object sender, EventArgs e)
        {
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

            frm.LoadAApDocument(FLedgerNumber, GetCurrentlySelectedInvoice());
            frm.Show();
        }


        /// <summary>
        /// create a new supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewSupplier(object sender, EventArgs e)
        {
            Int64 PartnerKey = -1;
            String ResultStringLbl;
            TLocationPK ResultLocationPK;

            // the user has to select an existing partner to make that partner a supplier
            if (TPartnerFindScreenManager.OpenModalForm("ORGANISATION,FAMILY,CHURCH",
                    out PartnerKey,
                    out ResultStringLbl,
                    out ResultLocationPK,
                    this))
            {
                TFrmAPEditSupplier frm = new TFrmAPEditSupplier(this);
                frm.LedgerNumber = FLedgerNumber;
                frm.CreateNewSupplier(PartnerKey);
                frm.Show();
            }
        }

        /// <summary>
        /// edit an existing supplier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditSupplier(object sender, EventArgs e)
        {
            Int64 PartnerKey = GetCurrentlySelectedSupplier();

            if (PartnerKey != -1)
            {
                TFrmAPEditSupplier frm = new TFrmAPEditSupplier(this);
                frm.LedgerNumber = FLedgerNumber;
                frm.EditSupplier(PartnerKey);
                frm.Show();
            }
        }

        /// <summary>
        /// create a new invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInvoice(object sender, EventArgs e)
        {
            Int64 PartnerKey = GetCurrentlySelectedSupplier();

            if (PartnerKey != -1)
            {
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

                frm.CreateAApDocument(FLedgerNumber, PartnerKey, false);
                frm.Show();
            }
        }

        /// <summary>
        /// create a new credit note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCreditNote(object sender, EventArgs e)
        {
            Int64 PartnerKey = GetCurrentlySelectedSupplier();

            if (PartnerKey != -1)
            {
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

                frm.CreateAApDocument(FLedgerNumber, PartnerKey, true);
                frm.Show();
            }
        }

        private void SupplierOutstandingOpt(object sender, EventArgs e)
        {
        }

        private void SetDueFilters(object sender, EventArgs e)
        {
            bool CanShow =
                (chkDueToday.CheckState == CheckState.Checked)
                || (chkOverdue.CheckState == CheckState.Checked)
                || (chkDueFuture.CheckState == CheckState.Checked);

            if (!CanShow)
            {
                chkShowOutstandingAmounts.CheckState = CheckState.Unchecked;
            }

            chkShowOutstandingAmounts.Enabled = CanShow;
        }

        private void SetSupplierFilters(object sender, EventArgs e)
        {
            if (FPagedDataTable != null)
            {
                String CurrencyRowFilter = "";
                String ActiveRowFilter = "";

                String CurrencyCode = cmbSupplierCurrency.cmbCombobox.Text;

                if (CurrencyCode != "")
                {
                    CurrencyRowFilter = String.Format("{0}='{1}'", AApSupplierTable.GetCurrencyCodeDBName(), CurrencyCode);
                }

                if (chkHideInactiveSuppliers.CheckState == CheckState.Checked)
                {
                    ActiveRowFilter = String.Format("{0}='ACTIVE'", PPartnerTable.GetStatusCodeDBName());
                }

                String RowFilter = CurrencyRowFilter;

                if ((CurrencyRowFilter != "") && (ActiveRowFilter != ""))
                {
                    RowFilter += " AND ";
                }

                RowFilter += ActiveRowFilter;
                FPagedDataTable.DefaultView.RowFilter = RowFilter;
            }
        }
    }
}

/* cut from APMain.yaml:

    Menu:
        mniFile:
            mniReports: {Label=&Reports}
            mniReprintPaymentReport: {Label=Reprint Pa&yment Report}
            mniSeparator: {Label=-}
            mniImport: {Label=&Import}
            mniExport: {Label=&Export}
            mniSeparator: {Label=-}
            mniDefaults: {Label=AP &Defaults}
            mniSeparator: {Label=-}

*/
