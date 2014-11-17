//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;
using System.IO;
using System.Data;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Interface used by logic objects in order to access selected public methods in the TUC_GLBatches class
    /// </summary>
    public interface IUC_GLBatches
    {
        /// <summary>
        /// Load the batches for the current financial year (used in particular when the screen starts up).
        /// </summary>
        void UpdateDisplay();

        /// <summary>
        /// Reload the batches
        /// </summary>
        void ReloadBatches();

        /// <summary>
        /// Create a New Batch
        /// </summary>
        bool CreateNewABatch();
    }

    public partial class TUC_GLBatches : IUC_GLBatches
    {
        private Int32 FLedgerNumber = -1;

        // Logic objects
        private TUC_GLBatches_LoadAndFilter FLoadAndFilterLogicObject = null;
        private TUC_GLBatches_Import FImportLogicObject = null;
        private TUC_GLBatches_Cancel FCancelLogicObject = null;
        private TUC_GLBatches_Post FPostLogicObject = null;
        private TUC_GLBatches_Reverse FReverseLogicObject = null;

        private DateTime FDefaultDate;
        private bool FBatchesLoaded = false;

        //Date related
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;
        private DateTime FCurrentEffectiveDate;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                InitialiseLogicObjects();
                InitialiseLedgerControls();

                LoadBatchesForCurrentYear();
            }
        }

        private void InitialiseLogicObjects()
        {
            FLoadAndFilterLogicObject = new TUC_GLBatches_LoadAndFilter(FLedgerNumber, FMainDS, FFilterAndFindObject);
            FImportLogicObject = new TUC_GLBatches_Import(FPetraUtilsObject, FLedgerNumber, FMainDS, this);
            FCancelLogicObject = new TUC_GLBatches_Cancel(FPetraUtilsObject, FLedgerNumber, FMainDS);
            FPostLogicObject = new TUC_GLBatches_Post(FPetraUtilsObject, FLedgerNumber, FMainDS, this);
            FReverseLogicObject = new TUC_GLBatches_Reverse(FPetraUtilsObject, FLedgerNumber, FMainDS, this);
        }

        private void InitialiseLedgerControls()
        {
            //Set the valid date range label
            TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                out FStartDateCurrentPeriod,
                out FEndDateLastForwardingPeriod,
                out FDefaultDate);

            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                StringHelper.DateToLocalizedString(FStartDateCurrentPeriod, false, false),
                StringHelper.DateToLocalizedString(FEndDateLastForwardingPeriod, false, false));

            // Get the current year/period and pass on to the filter logic object
            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            FLoadAndFilterLogicObject.CurrentLedgerYear = LedgerRow.CurrentFinancialYear;
            FLoadAndFilterLogicObject.CurrentLedgerPeriod = LedgerRow.CurrentPeriod;
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public void LoadBatchesForCurrentYear()
        {
            FBatchesLoaded = false;

            // Set up for current year with current and forwarding periods (on initial load this will already be set so will not fire a change)
            FLoadAndFilterLogicObject.YearIndex = 0;
            FLoadAndFilterLogicObject.PeriodIndex = 0;

            // This call will get the first year's data and update the display.
            // Note: If the first year data has already been loaded once there will be no trip to the server to get any updates.
            //        if you know updates are available, you need to merge them afterwards or clear the data table first
            UpdateDisplay();

            if (((TFrmGLBatch) this.ParentForm).LoadForImport)
            {
                // We have been launched from the Import Batches main menu screen as opposed to the regular GL Batches menu
                // Call the logic object to import:  this will request a CSV file and merge the batches on the server.
                // Finally it will call back to ReloadBatches() in this class, which merges the server data into FMainDS and selects the first row
                FImportLogicObject.ImportBatches(TUC_GLBatches_Import.TImportDataSourceEnum.FromFile);

                // Reset the flag
                ((TFrmGLBatch) this.ParentForm).LoadForImport = false;
            }

            FBatchesLoaded = true;
        }

        /// <summary>
        /// Updates the data display.  Call this after the DataSet has changed.
        /// </summary>
        public void UpdateDisplay()
        {
            Cursor prevCursor = ParentForm.Cursor;

            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;

                // Remember our current row position
                int nCurrentRowIndex = GetSelectedRowIndex();

                // This single call will fire the event that loads data and populates the grid
                FFilterAndFindObject.ApplyFilter();

                // Now we can select the row index we had before (if it exists)
                SelectRowInGrid(nCurrentRowIndex);
                UpdateRecordNumberDisplay();
            }
            finally
            {
                ParentForm.Cursor = prevCursor;
            }
        }

        /// <summary>
        /// Enable the transaction tab
        /// </summary>
        public void AutoEnableTransTabForBatch()
        {
            bool EnableTransTab = false;

            //If a single journal exists and it is not status=Cancelled then enable transactions tab
            if ((FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.LastJournal == 1))
            {
                LoadJournalsForCurrentBatch();

                if (FMainDS.AJournal.DefaultView.Count > 0)
                {
                    AJournalRow rJ = (AJournalRow)FMainDS.AJournal.DefaultView[0].Row;

                    EnableTransTab = (rJ.JournalStatus != MFinanceConstants.BATCH_CANCELLED);
                }
            }

            ((TFrmGLBatch) this.ParentForm).EnableTransactions(EnableTransTab);
        }

        private void LoadJournalsForCurrentBatch()
        {
            //Current Batch number
            Int32 BatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            if (FMainDS.AJournal != null)
            {
                FMainDS.AJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                    ATransactionTable.GetBatchNumberDBName(),
                    BatchNumber);

                if (FMainDS.AJournal.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(FLedgerNumber, BatchNumber));
                }
            }
        }

        private void ShowDataManual()
        {
            // AlanP: Can this happen?
            if (FLedgerNumber == -1)
            {
                EnableButtonControl(false);
            }
        }

        private void UpdateChangeableStatus()
        {
            FPetraUtilsObject.EnableAction("actReverseBatch", (FPreviouslySelectedDetailRow != null)
                && FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_POSTED);

            Boolean postable = (FPreviouslySelectedDetailRow != null)
                               && FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED;

            FPetraUtilsObject.EnableAction("actPostBatch", postable);
            FPetraUtilsObject.EnableAction("actTestPostBatch", postable);
            FPetraUtilsObject.EnableAction("actCancel", postable);
            pnlDetails.Enabled = postable;
            pnlDetailsProtected = !postable;

            if ((FPreviouslySelectedDetailRow == null) && (((TFrmGLBatch) this.ParentForm) != null))
            {
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
            }
        }

        private void ValidateDataDetailsManual(ABatchRow ARow)
        {
            if ((ARow == null) || (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            ParseHashTotal(ARow);

            TSharedFinanceValidation_GL.ValidateGLBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict, FStartDateCurrentPeriod, FEndDateLastForwardingPeriod);

            //TODO: remove this once database definition is set for Batch Description to be NOT NULL
            // Description is mandatory then make sure it is set
            if (txtDetailBatchDescription.Text.Length == 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ABatchTable.ColumnBatchDescriptionId];
                ValidationContext = String.Format("Batch number {0}",
                    ARow.BatchNumber);

                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.BatchDescription,
                    "Description of " + ValidationContext,
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }

        private void ParseHashTotal(ABatchRow ARow)
        {
            decimal CorrectHashValue = 0m;

            if (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            if ((txtDetailBatchControlTotal.NumberValueDecimal == null) || !txtDetailBatchControlTotal.NumberValueDecimal.HasValue)
            {
                CorrectHashValue = 0m;
            }
            else
            {
                CorrectHashValue = txtDetailBatchControlTotal.NumberValueDecimal.Value;
            }

            // AlanP: is this another case of needing to check for a real change??
            txtDetailBatchControlTotal.NumberValueDecimal = CorrectHashValue;
            ARow.BatchControlTotal = CorrectHashValue;
        }

        private void ShowDetailsManual(ABatchRow ARow)
        {
            AutoEnableTransTabForBatch();
            grdDetails.TabStop = (ARow != null);

            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
                ((TFrmGLBatch) this.ParentForm).DisableTransactions();
                EnableButtonControl(false);
                ClearDetailControls();
                return;
            }

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                 || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED));

            FCurrentEffectiveDate = ARow.DateEffective;

            UpdateBatchPeriod(null, null);

            UpdateChangeableStatus();
            ((TFrmGLBatch) this.ParentForm).EnableJournals();
        }

        /// <summary>
        /// This routine is called by a double click on a batch row, which means: Open the
        /// Journal Tab of this batch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowJournalTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Journals);
        }

        /// <summary>
        /// Controls the enabled status of the Cancel, Test and Post buttons
        /// </summary>
        /// <param name="AEnable"></param>
        private void EnableButtonControl(bool AEnable)
        {
            if (AEnable)
            {
                if (!pnlDetails.Enabled)
                {
                    pnlDetails.Enabled = true;
                }
            }

            btnPostBatch.Enabled = AEnable;
            btnTestPostBatch.Enabled = AEnable;
            btnCancel.Enabled = AEnable;
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (!SaveChangesAndResetFilter())
            {
                return;
            }

            // AlanP:  review this
            //string rowFilter = String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);
            //FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, (FSelectedPeriod == 0)
            //    && (FCurrentBatchViewOption == MFinanceConstants.GIFT_BATCH_VIEW_ALL));
            //FFilterAndFindObject.ApplyFilter();

            CreateNewABatch();

            pnlDetails.Enabled = true;
            EnableButtonControl(true);

            ABatchRow newBatchRow = GetSelectedDetailRow();
            Int32 yearNumber = 0;
            Int32 periodNumber = 0;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            {
                newBatchRow.BatchPeriod = periodNumber;
            }

            newBatchRow.DateEffective = FDefaultDate;
            dtpDetailDateEffective.Date = FDefaultDate;

            //Needed as GL batches can not be deleted
            ((TFrmGLBatch)ParentForm).SaveChanges();

            //Enable the Journals if not already enabled
            ((TFrmGLBatch)ParentForm).EnableJournals();
        }

        private bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            return TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);
        }

        /// <summary>
        /// Cancel a batch (there is no deletion of batches)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelRow(System.Object sender, EventArgs e)
        {
            int newCurrentRowPos = grdDetails.GetFirstHighlightedRowIndex();

            if (FCancelLogicObject.CancelBatch(FPreviouslySelectedDetailRow, txtDetailBatchDescription))
            {
                SelectRowInGrid(newCurrentRowPos);
            }

            //If some row(s) still exist after deletion
            if (grdDetails.Rows.Count < 2)
            {
                EnableButtonControl(false);
                ClearDetailControls();

                ((TFrmGLBatch)ParentForm).DisableJournals();
                ((TFrmGLBatch)ParentForm).DisableTransactions();
            }
        }

        private void ClearControls()
        {
            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                txtDetailBatchDescription.Clear();
                txtDetailBatchControlTotal.NumberValueDecimal = 0;
            }
            finally
            {
                FPetraUtilsObject.EnableDataChangedEvent();
            }
        }

        private void ClearDetailControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            txtDetailBatchDescription.Text = string.Empty;
            txtDetailBatchControlTotal.NumberValueDecimal = 0;
            dtpDetailDateEffective.Date = FDefaultDate;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        private void UpdateBatchPeriod(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            bool UpdateTransactionDates = false;

            Int32 periodNumber = 0;
            Int32 yearNumber = 0;
            string aDate = string.Empty;
            DateTime dateValue;

            try
            {
                aDate = dtpDetailDateEffective.Date.ToString();

                if (DateTime.TryParse(aDate, out dateValue))
                {
                    if ((dateValue < FStartDateCurrentPeriod) || (dateValue > FEndDateLastForwardingPeriod))
                    {
                        return;
                    }

                    //GetDetailsFromControls will do this automatically if the user tabs
                    //  passed the last control, but not if they clik on another control
                    if (FCurrentEffectiveDate != dateValue)
                    {
                        FCurrentEffectiveDate = dateValue;
                        FPreviouslySelectedDetailRow.DateEffective = dateValue;
                    }
                    else
                    {
                        return;
                    }

                    //Check if new date is in a different Batch period to the current one
                    if (GetAccountingYearPeriodByDate(FLedgerNumber, dateValue, out yearNumber, out periodNumber))
                    {
                        if (periodNumber != FPreviouslySelectedDetailRow.BatchPeriod)
                        {
                            FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;

                            //Update the Transaction effective dates
                            UpdateTransactionDates = true;

                            if (FLoadAndFilterLogicObject.YearIndex != 0)
                            {
                                FLoadAndFilterLogicObject.YearIndex = 0;
                                FLoadAndFilterLogicObject.PeriodIndex = 1;
                                dtpDetailDateEffective.Date = dateValue;
                                dtpDetailDateEffective.Focus();
                            }
                            else if (FLoadAndFilterLogicObject.PeriodIndex != 1)
                            {
                                FLoadAndFilterLogicObject.PeriodIndex = 1;
                                dtpDetailDateEffective.Date = dateValue;
                                dtpDetailDateEffective.Focus();
                            }
                        }
                    }

                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionTotals("BATCH", UpdateTransactionDates);
                    FPetraUtilsObject.SetChangedFlag();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReverseBatch(System.Object sender, EventArgs e)
        {
            //get index position of row to post
            int newCurrentRowPos = GetSelectedRowIndex();

            if (FReverseLogicObject.ReverseBatch(FPreviouslySelectedDetailRow, dtpDetailDateEffective.Date.Value, FStartDateCurrentPeriod,
                    FEndDateLastForwardingPeriod))
            {
                // AlanP - commenting out most of this because it should be unnecessary - or should move to ShowDetailsManual()
                //Select unposted batch row in same index position as batch just posted
                //grdDetails.DataSource = null;
                //grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

                if (grdDetails.Rows.Count > 1)
                {
                    //Needed because posting process forces grid events which sets FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent
                    // such that a removal of a row is not detected
                    SelectRowInGrid(newCurrentRowPos);
                }
                else
                {
                    EnableButtonControl(false);
                    ClearDetailControls();
                    btnNew.Focus();
                    pnlDetails.Enabled = false;
                }

                UpdateRecordNumberDisplay();
                FFilterAndFindObject.SetRecordNumberDisplayProperties();
            }
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            //get index position of row to post
            int newCurrentRowPos = GetSelectedRowIndex();

            if (FPostLogicObject.PostBatch(FPreviouslySelectedDetailRow, dtpDetailDateEffective.Date.Value, FStartDateCurrentPeriod,
                    FEndDateLastForwardingPeriod))
            {
                // AlanP - commenting out most of this because it should be unnecessary - or should move to ShowDetailsManual()
                ////Select unposted batch row in same index position as batch just posted
                //grdDetails.DataSource = null;
                //grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

                if (grdDetails.Rows.Count > 1)
                {
                    //Needed because posting process forces grid events which sets FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent
                    // such that a removal of a row is not detected
                    SelectRowInGrid(newCurrentRowPos);
                }
                else
                {
                    EnableButtonControl(false);
                    ClearDetailControls();
                    btnNew.Focus();
                    pnlDetails.Enabled = false;
                }

                UpdateRecordNumberDisplay();
                FFilterAndFindObject.SetRecordNumberDisplayProperties();
            }
        }

        /// <summary>
        /// this function calculates the balances of the accounts involved, if this batch would be posted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestPostBatch(System.Object sender, EventArgs e)
        {
            FPostLogicObject.TestPostBatch(FPreviouslySelectedDetailRow);
        }

        private void RefreshGridData(int ABatchNumber, bool ANoFocusChange, bool ASelectOnly = false)
        {
            //string RowFilter = string.Empty;

            if (!ASelectOnly)
            {
                //RowFilter = String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

                //// AlanP: review this
                //FFilterAndFindObject.FilterPanelControls.SetBaseFilter(RowFilter, (FSelectedPeriod == -1)
                //    && (FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_ALL));
                FFilterAndFindObject.ApplyFilter();
            }

            if (grdDetails.Rows.Count < 2)
            {
                ShowDetails(null);
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
                ((TFrmGLBatch) this.ParentForm).DisableTransactions();
            }
            else if (FBatchesLoaded == true)
            {
                //Select same row after refilter
                int newRowToSelectAfterFilter =
                    (ABatchNumber > 0) ? GetDataTableRowIndexByPrimaryKeys(FLedgerNumber, ABatchNumber) : FPrevRowChangedRow;

                if (ANoFocusChange)
                {
                    SelectRowInGrid(newRowToSelectAfterFilter);
                    //grdDetails.SelectRowWithoutFocus(newRowToSelectAfterFilter);
                }
                else
                {
                    SelectRowInGrid(newRowToSelectAfterFilter);
                }
            }
        }

        private int GetDataTableRowIndexByPrimaryKeys(int ALedgerNumber, int ABatchNumber)
        {
            int rowPos = 0;
            bool batchFound = false;

            foreach (DataRowView rowView in FMainDS.ABatch.DefaultView)
            {
                ABatchRow row = (ABatchRow)rowView.Row;

                if ((row.LedgerNumber == ALedgerNumber) && (row.BatchNumber == ABatchNumber))
                {
                    batchFound = true;
                    break;
                }

                rowPos++;
            }

            if (!batchFound)
            {
                rowPos = 0;
            }

            //remember grid is out of sync with DataView by 1 because of grid header rows
            return rowPos + 1;
        }

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            if (grdDetails.Rows.Count <= 1)
            {
                btnNew.Focus();
            }
            else
            {
                grdDetails.Focus();
            }
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowJournalTab);
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            SetInitialFocus();
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }
        }

        private void ImportFromSpreadSheet(object sender, EventArgs e)
        {
            string CSVDataFileName;
            DateTime LatestTransactionDate;

            if (FImportLogicObject.ImportFromSpreadsheet(out CSVDataFileName, out LatestTransactionDate))
            {
                dtpDetailDateEffective.Date = LatestTransactionDate;
                txtDetailBatchDescription.Text = Path.GetFileNameWithoutExtension(CSVDataFileName);
            }
        }

        /// <summary>
        /// ImportBatches called from button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImportBatchesFromFile(object sender, EventArgs e)
        {
            if (!SaveChangesAndResetFilter())
            {
                return;
            }

            FImportLogicObject.ImportBatches(TUC_GLBatches_Import.TImportDataSourceEnum.FromFile);
        }

        private void ImportBatchesFromClipboard(object sender, EventArgs e)
        {
            if (!SaveChangesAndResetFilter())
            {
                return;
            }

            FImportLogicObject.ImportBatches(TUC_GLBatches_Import.TImportDataSourceEnum.FromClipboard);
        }

        /// <summary>
        /// Public method called from the transactions tab
        /// </summary>
        public void ImportTransactions(TUC_GLBatches_Import.TImportDataSourceEnum AImportDataSource)
        {
            FImportLogicObject.ImportTransactions(FPreviouslySelectedDetailRow, GetCurrentJournal(), AImportDataSource);
        }

        private void ExportBatches(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please correct and save changed data before the export!"),
                    Catalog.GetString("Export Error"));
                return;
            }

            TFrmGLBatchExport gl = new TFrmGLBatchExport(FPetraUtilsObject.GetForm());
            gl.LedgerNumber = FLedgerNumber;
            gl.MainDS = FMainDS;
            gl.Show();
        }

        private void CreateFilterFindPanelsManual()
        {
            ((Label)FFilterAndFindObject.FindPanelControls.FindControlByName("lblBatchNumber")).Text = "Batch number";
        }

        private AJournalRow GetCurrentJournal()
        {
            return (AJournalRow)((TFrmGLBatch) this.ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// Reload batches after an import
        /// </summary>
        public void ReloadBatches()
        {
            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, FLoadAndFilterLogicObject.DatabaseYear,
                    FLoadAndFilterLogicObject.DatabasePeriod));

            grdDetails.SelectRowInGrid(1);
        }

        private bool SaveChangesAndResetFilter()
        {
            bool RetVal = true;

            try
            {
                if (!FLoadAndFilterLogicObject.StatusEditing)
                {
                    FLoadAndFilterLogicObject.StatusEditing = true;
                }

                if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
                {
                    RetVal = false;
                }
                else
                {
                    //Set year and period to correct value
                    FLoadAndFilterLogicObject.YearIndex = 0;
                    FLoadAndFilterLogicObject.PeriodIndex = 0;

                    FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            if (FLoadAndFilterLogicObject != null)
            {
                FLoadAndFilterLogicObject.ApplyFilterManual(ref AFilterString);
            }
        }
    }
}