//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2015 by OM International
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
using System.Threading;
using System.Windows.Forms;
using SourceGrid;

using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// GL Transaction Detail Find screen.
    /// This screen is based on Partner Find.
    public partial class TFrmGLTransactionFindScreen
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public delegate void TMyUpdateDelegate(System.Object msg);

        private Int32 FLedgerNumber = -1;
        private DataTable FCriteriaDataTable;
        private IFinanceUIConnectorsGLTransactionFind FGLTransactionFindObject;
        string FCurrency = "";

        /// <summary>DataTable that holds all Pages of data (also empty ones that are not retrieved yet!)</summary>
        private DataTable FPagedDataTable;

        /// <summary>Tells the screen whether it should still wait for the Server's result</summary>
        private Boolean FKeepUpSearchFinishedCheck;

        /// <summary>
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                if (FLedgerNumber != value)
                {
                    FLedgerNumber = value;
                    FCurrency = TRemote.MFinance.Common.ServerLookups.WebConnectors.GetLedgerBaseCurrency(FLedgerNumber);

                    cmbLedger.SetSelectedInt32(FLedgerNumber);

                    SetupComboboxes();
                }
            }
        }

        #region Setup methods

        private void InitializeManualCode()
        {
            FGLTransactionFindObject = TRemote.MFinance.Finance.UIConnectors.GLTransactionFind();

            // remove from the combobox all ledger numbers which the user does not have permission to access
            DataView cmbLedgerDataView = (DataView)cmbLedger.cmbCombobox.DataSource;

            for (int i = 0; i < cmbLedgerDataView.Count; i++)
            {
                string LedgerNumberStr;

                // postgresql
                if (cmbLedgerDataView[i].Row[0].GetType().Equals(typeof(int)))
                {
                    LedgerNumberStr = ((int)cmbLedgerDataView[i].Row[0]).ToString("0000");
                }
                else // sqlite
                {
                    LedgerNumberStr = ((Int64)cmbLedgerDataView[i].Row[0]).ToString("0000");
                }

                if (!UserInfo.GUserInfo.IsInModule("LEDGER" + LedgerNumberStr))
                {
                    cmbLedgerDataView.Delete(i);
                    i--;
                }
            }

            // add event to combobox (this is the best moment to do this)
            cmbLedger.SelectedValueChanged += new System.EventHandler(this.OnComboChange);

            // add divider line (can't currently do this in YAML)
            DevAge.Windows.Forms.Line linCriteriaDivider = new DevAge.Windows.Forms.Line();
            linCriteriaDivider.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            linCriteriaDivider.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            linCriteriaDivider.FirstColor = System.Drawing.SystemColors.ControlDark;
            linCriteriaDivider.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            linCriteriaDivider.Location = new System.Drawing.Point(grpFindCriteria.Location.Y + 6, btnSearch.Location.Y - 2);
            linCriteriaDivider.Name = "linCriteriaDivider";
            linCriteriaDivider.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            linCriteriaDivider.Size = new System.Drawing.Size(grpFindCriteria.Size.Width - 12, 2);
            grpFindCriteria.Controls.Add(linCriteriaDivider);

            // pnlBlankSearchResult
            pnlBlankSearchResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            pnlBlankSearchResult.Size = grdResult.Size; lblSearchInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));

            // lblSearchInfo
            this.lblSearchInfo.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            lblSearchInfo.Size = grdResult.Size;
            lblSearchInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // remove '0' from these textboxes
            txtBatchNumber.NumberValueInt = null;
            txtMinimumAmount.NumberValueInt = null;
            txtMaximumAmount.NumberValueInt = null;

            // set button to be on the very right of the screen (can't make this work in YAML)
            btnClear.Location = new System.Drawing.Point(linCriteriaDivider.Location.X + linCriteriaDivider.Width - btnClear.Width,
                btnClear.Location.Y);

            // set to blank initially
            lblRecordCounter.Text = "";

            // event fired on screen close
            this.Closed += new System.EventHandler(this.TFrmGLTransactionFindScreen_Closed);

            // catch enter on all controls, to trigger search or accept (could use this.AcceptButton, but we have several search buttons etc)
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatchEnterKey);

            // catch enter on grid to view the selected transaction
            this.grdResult.EnterKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.BtnView_Click);

            // fix tab order
            int Temp = cmbCostCentre.TabIndex;
            int Temp2 = txtJournalDescription.TabIndex;
            cmbCostCentre.TabIndex = dtpDateFrom.TabIndex;
            dtpDateFrom.TabIndex = dtpDateTo.TabIndex;
            dtpDateTo.TabIndex = txtNarrative.TabIndex;
            txtNarrative.TabIndex = txtBatchDescription.TabIndex;
            txtBatchDescription.TabIndex = txtBatchNumber.TabIndex;
            txtBatchNumber.TabIndex = Temp;
            txtJournalDescription.TabIndex = cmbAccount.TabIndex;
            cmbAccount.TabIndex = Temp2;

            this.ActiveControl = cmbStatus;
        }

        // populate the comboboxes
        private void SetupComboboxes()
        {
            TFinanceControls.InitialiseCostCentreList(ref cmbCostCentre, FLedgerNumber, true, false, false, false);
            cmbCostCentre.ColumnWidthCol2 = 300;
            cmbCostCentre.AttachedLabel.Width = 280;

            TFinanceControls.InitialiseAccountList(ref cmbAccount, FLedgerNumber,
                true, false, false, false, FCurrency, true);
            cmbAccount.ColumnWidthCol2 = 300;
            cmbAccount.AttachedLabel.Width = 280;

            cmbStatus.AddStringItem(Catalog.GetString("All"));
            cmbStatus.AddStringItem(MFinanceConstants.BATCH_POSTED);
            cmbStatus.AddStringItem(MFinanceConstants.BATCH_UNPOSTED);
            cmbStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Sets up paged search result DataTable with the result of the Servers query
        /// and DataBind the DataGrid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupGrid()
        {
            try
            {
                grdResult.Columns.Clear();
                grdResult.AddTextColumn(Catalog.GetString("Batch"), FPagedDataTable.Columns["a_batch_number_i"]);
                grdResult.AddTextColumn(Catalog.GetString("Journal"), FPagedDataTable.Columns["a_journal_number_i"]);
                grdResult.AddTextColumn(Catalog.GetString("Transaction"), FPagedDataTable.Columns["a_transaction_number_i"]);
                grdResult.AddCheckBoxColumn(Catalog.GetString("P"), FPagedDataTable.Columns["a_transaction_status_l"]);
                grdResult.AddTextColumn(Catalog.GetString("Narrative"), FPagedDataTable.Columns["a_narrative_c"]);
                grdResult.AddTextColumn(Catalog.GetString("Cost Centre Code"), FPagedDataTable.Columns["a_cost_centre_code_c"]);
                grdResult.AddTextColumn(Catalog.GetString("Account Code"), FPagedDataTable.Columns["a_account_code_c"]);
                grdResult.AddCurrencyColumn(Catalog.GetString("Amount (" + FCurrency + ")"), FPagedDataTable.Columns["a_amount_in_base_currency_n"]);
                grdResult.AddDateColumn(Catalog.GetString("Transaction Date"), FPagedDataTable.Columns["a_transaction_date_d"]);
                grdResult.AddTextColumn(Catalog.GetString("Batch Description"), FPagedDataTable.Columns["a_batch_description_c"]);
                grdResult.AddTextColumn(Catalog.GetString("Journal Description"), FPagedDataTable.Columns["a_journal_description_c"]);

                this.grdResult.DoubleClickCell += new Ict.Common.Controls.TDoubleClickCellEventHandler(this.GrdResult_DoubleClickCell);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception occured in SetupGrid: " + exp.Message + exp.StackTrace);
            }
        }

        /// <summary>
        /// Sets up the DataBinding of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridDataBinding()
        {
            DataView FindResultPagedDV;

            FindResultPagedDV = FPagedDataTable.DefaultView;
            FindResultPagedDV.AllowNew = false;
            FindResultPagedDV.AllowDelete = false;
            FindResultPagedDV.AllowEdit = false;
            grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(FindResultPagedDV);
        }

        #endregion

        #region Main Methods

        private void GetCriteriaFromControls()
        {
            // create datatable to contain criteria
            FCriteriaDataTable = new DataTable();
            FCriteriaDataTable.Columns.Add(new DataColumn("Ledger", typeof(Int32)));
            FCriteriaDataTable.Columns.Add(new DataColumn("BatchNumber", typeof(Int32)));
            FCriteriaDataTable.Columns.Add(new DataColumn("TransactionStatus", typeof(Int32)));
            FCriteriaDataTable.Columns.Add(new DataColumn("BatchDescription", typeof(string)));
            FCriteriaDataTable.Columns.Add(new DataColumn("JournalDescription", typeof(string)));
            FCriteriaDataTable.Columns.Add(new DataColumn("Narrative", typeof(string)));
            FCriteriaDataTable.Columns.Add(new DataColumn("CostCentreCode", typeof(string)));
            FCriteriaDataTable.Columns.Add(new DataColumn("AccountCode", typeof(string)));
            FCriteriaDataTable.Columns.Add(new DataColumn("From", typeof(DateTime)));
            FCriteriaDataTable.Columns.Add(new DataColumn("To", typeof(DateTime)));
            FCriteriaDataTable.Columns.Add(new DataColumn("MinAmount", typeof(Int64)));
            FCriteriaDataTable.Columns.Add(new DataColumn("MaxAmount", typeof(Int64)));

            // add criteria from controls to datarow
            DataRow CriteriaRow = FCriteriaDataTable.NewRow();
            CriteriaRow["Ledger"] = cmbLedger.GetSelectedInt32();

            if (txtBatchNumber.NumberValueInt != null)
            {
                CriteriaRow["BatchNumber"] = txtBatchNumber.NumberValueInt;
            }

            if (cmbStatus.GetSelectedString() == MFinanceConstants.BATCH_POSTED)
            {
                CriteriaRow["TransactionStatus"] = 1;
            }
            else if (cmbStatus.GetSelectedString() == MFinanceConstants.BATCH_UNPOSTED)
            {
                CriteriaRow["TransactionStatus"] = 0;
            }
            else
            {
                CriteriaRow["TransactionStatus"] = DBNull.Value;
            }

            CriteriaRow["BatchDescription"] = txtBatchDescription.Text;
            CriteriaRow["JournalDescription"] = txtJournalDescription.Text;
            CriteriaRow["Narrative"] = txtNarrative.Text;
            CriteriaRow["CostCentreCode"] = cmbCostCentre.GetSelectedString();
            CriteriaRow["AccountCode"] = cmbAccount.GetSelectedString();

            if (dtpDateFrom.Date != null)
            {
                CriteriaRow["From"] = dtpDateFrom.Date;
            }

            if (dtpDateTo.Date != null)
            {
                CriteriaRow["To"] = dtpDateTo.Date;
            }

            if (txtMinimumAmount.NumberValueInt != null)
            {
                CriteriaRow["MinAmount"] = txtMinimumAmount.NumberValueInt;
            }

            if (txtMaximumAmount.NumberValueInt != null)
            {
                CriteriaRow["MaxAmount"] = txtMaximumAmount.NumberValueInt;
            }

            FCriteriaDataTable.Rows.Add(CriteriaRow);
        }

        /// <summary>
        /// Starts the Search operation.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void PerformSearch()
        {
            Thread FinishedCheckThread;

            // Start the asynchronous search operation on the PetraServer
            FGLTransactionFindObject.PerformSearch(FCriteriaDataTable);

            // Start thread that checks for the end of the search operation on the PetraServer
            FinishedCheckThread = new Thread(new ThreadStart(SearchFinishedCheckThread));
            FinishedCheckThread.Start();
        }

        /// <summary>
        /// Thread for the search operation. Monitor's the Server System.Object's
        /// Progress.ProgressState and invokes UI updates from that.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void SearchFinishedCheckThread()
        {
            TProgressState ProgressState;

            /* Check whether this Thread should still execute */
            while (FKeepUpSearchFinishedCheck)
            {
                try
                {
                    /* The next line of code calls a function on the PetraServer
                     * > causes a bit of data traffic everytime! */
                    ProgressState = FGLTransactionFindObject.Progress;
                }
                catch (System.NullReferenceException)
                {
                    /*
                     * This Exception occurs if the screen has been closed by the user
                     * in the meantime -> don't try to do anything further - it will break!
                     */
                    return;  // Thread ends here!
                }
                catch (Exception)
                {
                    throw;
                }

                if (ProgressState.JobFinished)
                {
                    FKeepUpSearchFinishedCheck = false;

                    // Fetch the first page of data
                    try
                    {
                        // For speed reasons we must add the necessary amount of emtpy Rows only *after* .AutoSizeCells()
                        // has already been run! See XML Comment on the called Method
                        // TSgrdDataGridPaged.LoadFirstDataPage for details!
                        FPagedDataTable = grdResult.LoadFirstDataPage(@GetDataPagedResult, false);
                    }
                    catch (Exception E)
                    {
                        MessageBox.Show(E.ToString());
                    }
                }
                else if (ProgressState.CancelJob)
                {
                    FKeepUpSearchFinishedCheck = false;
                    EnableDisableUI(true);
                    return;
                }

                // Sleep for some time. After that, this function is called again automatically.
                Thread.Sleep(200);
            }

            EnableDisableUI(true);
        }

        /// <summary>
        /// Enables and disables the UI. Invokes setting up of the Grid after a
        /// successful search operation.
        /// </summary>
        /// <returns>void</returns>
        private void EnableDisableUI(System.Object AEnable)
        {
            object[] Args;
            TMyUpdateDelegate MyUpdateDelegate;

            // Since this procedure is called from a separate (background) Thread, it is
            // necessary to execute this procedure in the Thread of the GUI
            if (btnSearch.InvokeRequired)
            {
                Args = new object[1];

                try
                {
                    MyUpdateDelegate = new TMyUpdateDelegate(EnableDisableUI);
                    Args[0] = AEnable;
                    btnSearch.Invoke(MyUpdateDelegate, new object[] { AEnable });
                }
                finally
                {
                    Args = new object[0];
                }
            }
            else
            {
                // Enable/disable according to how the search operation ended
                if (Convert.ToBoolean(AEnable))
                {
                    TProgressState ThreadStatus = FGLTransactionFindObject.Progress;

                    if (ThreadStatus.JobFinished)
                    {
                        // Search operation ended without interruption
                        if (FPagedDataTable.Rows.Count > 0)
                        {
                            btnSearch.Enabled = false;

                            // At least one result was found by the search operation
                            lblSearchInfo.Text = "";

                            //
                            // Setup result DataGrid
                            //
                            if (grdResult.Columns.Count < 1)
                            {
                                SetupGrid();
                            }

                            SetupDataGridDataBinding();
                            grdResult.AutoSizeCells();

                            grdResult.Selection.SelectRow(1, true);

                            // Scroll grid to first line (the grid might have been scrolled before to another position)
                            grdResult.ShowCell(new Position(1, 1), true);

                            // For speed reasons we must add the necessary amount of emtpy Rows only here (after .AutoSizeCells() has already
                            // been run! See XML Comment on the called Method TSgrdDataGridPaged.AddEmptyRows() for details!
                            grdResult.AddEmptyRows();

                            grdResult.BringToFront();

                            // set tooltips
                            grdResult.SetHeaderTooltip(3, MFinanceConstants.BATCH_POSTED);
                            grdResult.SetHeaderTooltip(5, Catalog.GetString("Confidential"));

                            // Make the Grid respond on updown keys
                            grdResult.Focus();

                            // Display the number of found gift details
                            UpdateRecordNumberDisplay();

                            Application.DoEvents();

                            btnSearch.Enabled = true;

                            this.Cursor = Cursors.Default;
                        }
                        else
                        {
                            // Search operation has found nothing
                            this.Cursor = Cursors.Default;
                            lblSearchInfo.Text = Catalog.GetString("No GL Transactions found.");
                            Application.DoEvents();

                            btnSearch.Enabled = true;

                            UpdateRecordNumberDisplay();
                        }
                    }
                    else
                    {
                        // Search operation interrupted by user
                        // used to release server System.Object here
                        // (It isn't currently possible for the user to stop the search. I don't think this functionality is necessary)
                        this.Cursor = Cursors.Default;
                        lblSearchInfo.Text = Catalog.GetString("Search stopped!");

                        btnSearch.Enabled = true;
                        Application.DoEvents();
                    }

                    // enable or disable btnView
                    if (FPagedDataTable.Rows.Count > 0)
                    {
                        btnView.Enabled = true;
                    }
                    else
                    {
                        btnView.Enabled = false;
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ANeededPage"></param>
        /// <param name="APageSize"></param>
        /// <param name="ATotalRecords"></param>
        /// <param name="ATotalPages"></param>
        /// <returns></returns>
        public DataTable GetDataPagedResult(Int16 ANeededPage, Int16 APageSize, out Int32 ATotalRecords, out Int16 ATotalPages)
        {
            DataTable ReturnValue = null;

            ATotalRecords = 0;
            ATotalPages = 0;

            if (FGLTransactionFindObject != null)
            {
                ReturnValue = FGLTransactionFindObject.GetDataPagedResult(ANeededPage, APageSize, out ATotalRecords, out ATotalPages);
            }

            return ReturnValue;
        }

        // update record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdResult.DataSource != null)
            {
                RecordCount = grdResult.TotalRecords;
            }
            else
            {
                RecordCount = 0;
            }

            lblRecordCounter.Text = String.Format(Catalog.GetPluralString("{0} record", "{0} records", RecordCount, true), RecordCount);
        }

        private void TFrmGLTransactionFindScreen_Closed(object sender, EventArgs e)
        {
            FGLTransactionFindObject = null;
        }

        private DataRow GetCurrentDataRow()
        {
            if (grdResult != null)
            {
                DataRowView[] TheDataRowViewArray = grdResult.SelectedDataRowsAsDataRowView;

                if (TheDataRowViewArray.Length > 0)
                {
                    return TheDataRowViewArray[0].Row;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if a combo box's value has been changed while the list is dropped down
        /// and that that combo box still contains the focus.
        /// </summary>
        /// <returns></returns>
        public bool ComboboxDroppedDown()
        {
            if (CostCentreDroppedDown && cmbCostCentre.ContainsFocus)
            {
                CostCentreDroppedDown = false;
                return true;
            }
            else if (AccountDroppedDown && cmbAccount.ContainsFocus)
            {
                AccountDroppedDown = false;
                return true;
            }
            else if (LedgerDroppedDown && cmbLedger.ContainsFocus)
            {
                LedgerDroppedDown = false;
                return true;
            }
            else if (StatusDroppedDown && cmbStatus.ContainsFocus)
            {
                StatusDroppedDown = false;
                return true;
            }

            CostCentreDroppedDown = false;
            AccountDroppedDown = false;
            LedgerDroppedDown = false;
            StatusDroppedDown = false;

            return false;
        }

        #endregion

        #region Events

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;

            GetCriteriaFromControls();

            pnlBlankSearchResult.BringToFront();
            lblSearchInfo.Text = Catalog.GetString("Searching...");

            this.Cursor = Cursors.AppStarting;
            FKeepUpSearchFinishedCheck = true;
            EnableDisableUI(false);
            Application.DoEvents();

            // Clear result table
            try
            {
                // If the scroll bar is scrolled to the bottom of the results table, we will get empty rows in the grid after the next search.
                // I've no idea why this is but automatically scrolling to the the top of the grid prevents it happening.
                grdResult.ShowCell(0);

                if (FPagedDataTable != null)
                {
                    FPagedDataTable.Clear();
                }
            }
            catch (Exception)
            {
                // don't do anything since this happens if the DataTable has no data yet
            }

            // Start asynchronous search operation
            PerformSearch();
        }

        // reset to default values
        private void BtnClear_Click(Object sender, EventArgs e)
        {
            // clear the search criteria
            txtBatchNumber.NumberValueInt = null;
            cmbStatus.SetSelectedString("");
            txtBatchDescription.ResetText();
            txtJournalDescription.ResetText();
            txtNarrative.ResetText();
            cmbCostCentre.SetSelectedString("");
            cmbAccount.SetSelectedString("");
            dtpDateFrom.ResetText();
            dtpDateTo.ResetText();
            txtMinimumAmount.NumberValueInt = null;
            txtMaximumAmount.NumberValueInt = null;

            // clear the grid
            grdResult.DataSource = null;
            FPagedDataTable = null;
            UpdateRecordNumberDisplay();

            // disable View button
            btnView.Enabled = false;
        }

        private void CatchEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!ComboboxDroppedDown())
                {
                    BtnSearch_Click(sender, e);
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            // get the currently selected row
            DataRow CurrentlySelectedRow = GetCurrentDataRow();

            if (CurrentlySelectedRow != null)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    TFrmGLBatch GLb = new TFrmGLBatch(this);
                    GLb.LedgerNumber = FLedgerNumber;

                    GLb.ShowDetailsOfOneBatch(FLedgerNumber, (int)CurrentlySelectedRow["a_batch_number_i"],
                        (int)CurrentlySelectedRow["a_journal_number_i"], (int)CurrentlySelectedRow["a_batch_year_i"],
                        (int)CurrentlySelectedRow["a_batch_period_i"]);

                    GLb.SelectTab(TFrmGLBatch.eGLTabs.Journals);
                    GLb.SelectTab(TFrmGLBatch.eGLTabs.Transactions);
                    GLb.FindGLTransaction((int)CurrentlySelectedRow["a_batch_number_i"],
                        (int)CurrentlySelectedRow["a_journal_number_i"], (int)CurrentlySelectedRow["a_transaction_number_i"]);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void GrdResult_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            BtnView_Click(this, null);
        }

        // These are used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down.
        private bool LedgerDroppedDown = false;
        private bool CostCentreDroppedDown = false;
        private bool AccountDroppedDown = false;
        private bool StatusDroppedDown = false;

        private void OnComboChange(object sender, EventArgs e)
        {
            // if the list is dropped down while the value is changed (not the case when a value from the list is clicked on)
            if (cmbCostCentre.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                CostCentreDroppedDown = true;
            }

            if (cmbAccount.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                AccountDroppedDown = true;
            }

            if (cmbLedger.cmbCombobox.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                LedgerDroppedDown = true;

                LedgerNumber = cmbLedger.GetSelectedInt32();
            }

            if (cmbStatus.DroppedDown)
            {
                // this is used to stop an 'Enter' key press triggering a search while a combo boxes list is dropped down
                StatusDroppedDown = true;
            }
        }

        #endregion
    }
}