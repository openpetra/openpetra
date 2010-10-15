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
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPMain
    {
        private IAPUIConnectorsFind FSupplierFindObject = null;
        private bool FKeepUpSearchFinishedCheck = false;

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
            }
        }

        /// <summary>
        /// search button was clicked
        /// </summary>
        public void SearchForSupplier(object sender, EventArgs e)
        {
            if (FKeepUpSearchFinishedCheck)
            {
                // don't run several searches at the same time
                return;
            }

            FSupplierFindObject = TRemote.MFinance.AP.UIConnectors.Find();

            DataTable CriteriaTable = new DataTable();

            // TODO: fill this criteria table with generated code? or check for visible controls during runtime?
            CriteriaTable.Columns.Add("SupplierId", typeof(string));
            DataRow row = CriteriaTable.NewRow();
            CriteriaTable.Rows.Add(row);
            row["SupplierId"] = cmbSupplierCode.GetSelectedString();

            // Start the asynchronous search operation on the PetraServer
            FSupplierFindObject.FindSupplier(CriteriaTable);

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
            grdSupplierResult.Columns.Clear();
            grdSupplierResult.AddTextColumn("Partner Key", FPagedDataTable.Columns[0]);
            grdSupplierResult.AddTextColumn("Partner Name", FPagedDataTable.Columns[1]);
        }

        /// make sure that this is called in the normal GUI thread
        private void FinishThread()
        {
            // Fetch the first page of data
            try
            {
                FPagedDataTable = grdSupplierResult.LoadFirstDataPage(@GetDataPagedResult);
            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }
            InitialiseGrid();
            DataView myDataView = FPagedDataTable.DefaultView;
            myDataView.AllowNew = false;
            grdSupplierResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdSupplierResult.AutoSizeCells();
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
                return FSupplierFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
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
                return Convert.ToInt64(SelectedGridRow[0][FPagedDataTable.Columns[0].ColumnName]);
            }

            return -1;
        }

        /// <summary>
        /// open the transactions of the selected supplier
        /// </summary>
        public void SupplierTransactions(object sender, EventArgs e)
        {
            TFrmAPSupplierTransactions frm = new TFrmAPSupplierTransactions(this.Handle);

            frm.LoadSupplier(FLedgerNumber, GetCurrentlySelectedSupplier());
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
                    this.Handle))
            {
                TFrmAPEditSupplier frm = new TFrmAPEditSupplier(this.Handle);
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
                TFrmAPEditSupplier frm = new TFrmAPEditSupplier(this.Handle);
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
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this.Handle);

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
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this.Handle);

                frm.CreateAApDocument(FLedgerNumber, PartnerKey, true);
                frm.Show();
            }
        }
    }
}