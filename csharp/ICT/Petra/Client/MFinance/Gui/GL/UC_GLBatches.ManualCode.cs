//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Xml;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.Interfaces.MFinance;

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
        void LoadBatchesForCurrentYear();

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

        private Int32 FSelectedBatchNumber = -1;
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

                LoadBatchesForCurrentYear();
            }
        }

        private void InitialiseLogicObjects()
        {
            FLoadAndFilterLogicObject = new TUC_GLBatches_LoadAndFilter(FLedgerNumber, FMainDS, FFilterAndFindObject);
            FImportLogicObject = new TUC_GLBatches_Import(FPetraUtilsObject, FLedgerNumber, FMainDS, this);
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public void LoadBatchesForCurrentYear()
        {
            FBatchesLoaded = false;
            InitialiseLogicObjects();

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

            // This single call will fire the event that loads data and populates the grid
            FFilterAndFindObject.ApplyFilter();

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGLBatch) this.ParentForm).EnableJournals();
                AutoEnableTransTabForBatch();
            }
            else
            {
                ClearControls();
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
                ((TFrmGLBatch) this.ParentForm).DisableTransactions();
            }

            ShowData();

            FBatchesLoaded = true;

            if (((TFrmGLBatch) this.ParentForm).LoadForImport)
            {
                FImportLogicObject.ImportBatches();
            }

            UpdateRecordNumberDisplay();
            SelectRowInGrid(1);
        }

        ///// No longer used?  It has disappeared inside the load and filter object
        ///// Reset the control
        //public void ClearCurrentSelection()
        //{
        //    if (FPetraUtilsObject.HasChanges)
        //    {
        //        GetDataFromControls();
        //    }

        //    this.FPreviouslySelectedDetailRow = null;
        //    ShowData();
        //}

        ///// No longer used?
        ///// <summary>
        ///// Returns FMainDS
        ///// </summary>
        ///// <returns></returns>
        //public GLBatchTDS BatchFMainDS()
        //{
        //    return FMainDS;
        //}

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

            FSelectedBatchNumber = ARow.BatchNumber;
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

            FSelectedBatchNumber = newBatchRow.BatchNumber;

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
            if ((FPreviouslySelectedDetailRow == null) || !((TFrmGLBatch)ParentForm).SaveChanges())
            {
                return;
            }

            int newCurrentRowPos = grdDetails.GetFirstHighlightedRowIndex();

            if ((MessageBox.Show(String.Format(Catalog.GetString("You have chosen to cancel this batch ({0}).\n\nDo you really want to cancel it?"),
                         FSelectedBatchNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                try
                {
                    // AlanP: commented out calls that just set FPreviouslySelectedDetailRow to null
                    //Load all journals for current Batch
                    //clear any transactions currently being editied in the Transaction Tab
                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                    //clear any journals currently being editied in the Journals Tab
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();

                    //Clear Journals etc for current Batch
                    FMainDS.ATransAnalAttrib.Clear();
                    FMainDS.ATransaction.Clear();
                    FMainDS.AJournal.Clear();

                    //Load tables afresh
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(FLedgerNumber, FSelectedBatchNumber));
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionForBatch(FLedgerNumber, FSelectedBatchNumber));
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForBatch(FLedgerNumber, FSelectedBatchNumber));

                    //Delete transactions
                    for (int i = FMainDS.ATransAnalAttrib.Count - 1; i >= 0; i--)
                    {
                        FMainDS.ATransAnalAttrib[i].Delete();
                    }

                    for (int i = FMainDS.ATransaction.Count - 1; i >= 0; i--)
                    {
                        FMainDS.ATransaction[i].Delete();
                    }

                    //Update Journal totals and status
                    foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                    {
                        if (journal.BatchNumber == FSelectedBatchNumber)
                        {
                            journal.BeginEdit();
                            journal.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                            journal.JournalCreditTotal = 0;
                            journal.JournalDebitTotal = 0;
                            journal.EndEdit();
                        }
                    }

                    FPreviouslySelectedDetailRow.BeginEdit();

                    //Ensure validation passes
                    if (FPreviouslySelectedDetailRow.BatchDescription.Length == 0)
                    {
                        txtDetailBatchDescription.Text = " ";
                    }

                    FPreviouslySelectedDetailRow.BatchCreditTotal = 0;
                    FPreviouslySelectedDetailRow.BatchDebitTotal = 0;
                    FPreviouslySelectedDetailRow.BatchControlTotal = 0;
                    FPreviouslySelectedDetailRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                    FPreviouslySelectedDetailRow.EndEdit();

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (((TFrmGLBatch)ParentForm).SaveChanges())
                    {
                        //Select and call the event that doesn't occur automatically
                        SelectRowInGrid(newCurrentRowPos);

                        MessageBox.Show(Catalog.GetString("The batch has been cancelled successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The batch has been cancelled but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save the cancellation immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

//                }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        // Not used any more?
        // ALanP: should be able to use standard method
        //private int CurrentRowIndex()
        //{
        //    int rowIndex = -1;

        //    SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

        //    if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
        //    {
        //        rowIndex = selectedRegion.GetRowsIndex()[0];
        //    }

        //    return rowIndex;
        //}

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
                    FPetraUtilsObject.HasChanges = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool SaveBatchForPosting()
        {
            // save first, then post
            if (!((TFrmGLBatch)ParentForm).SaveChanges())
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                    Catalog.GetString("Please first save the batch, and then post it!"),
                    Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ReverseBatch(System.Object sender, EventArgs e)
        {
            TVerificationResultCollection Verifications;

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmGLBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to reverse
                    MessageBox.Show(Catalog.GetString("The batch was not reversed due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then you can reverse it!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //get index position of row to post
            int newCurrentRowPos = GetSelectedRowIndex();

            //TODO: Allow the user in a dialog to specify the reverse date
            DateTime DateForReverseBatch = dtpDetailDateEffective.Date.Value;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if ((DateForReverseBatch.Date < FStartDateCurrentPeriod) || (DateForReverseBatch.Date > FEndDateLastForwardingPeriod))
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The Reverse Date is outside the periods available for reversing. We will set the posting date to the first possible date, {0}."),
                            FStartDateCurrentPeriod));
                    DateForReverseBatch = FStartDateCurrentPeriod;
                }

                if (MessageBox.Show(String.Format(Catalog.GetString("Are you sure you want to reverse batch {0}?"),
                            FSelectedBatchNumber),
                        Catalog.GetString("Question"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    int ReversalGLBatch;

                    if (!TRemote.MFinance.GL.WebConnectors.ReverseBatch(FLedgerNumber, FSelectedBatchNumber,
                            DateForReverseBatch,
                            out ReversalGLBatch,
                            out Verifications))
                    {
                        string ErrorMessages = String.Empty;

                        foreach (TVerificationResult verif in Verifications)
                        {
                            ErrorMessages += "[" + verif.ResultContext + "] " +
                                             verif.ResultTextCaption + ": " +
                                             verif.ResultText + Environment.NewLine;
                        }

                        System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Reversal failed"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            String.Format(Catalog.GetString("A reversal batch has been created, with batch number {0}!"), ReversalGLBatch),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // refresh the grid, to reflect that the batch has been posted
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndContent(FLedgerNumber, ReversalGLBatch));

                        // AlanP: You must not set FPreviouslySelectedDetailRow = null because it is owned by grid events
                        this.FPreviouslySelectedDetailRow = null;
                        ((TFrmGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();
                        ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                        LoadBatchesForCurrentYear();

                        // AlanP - commenting out most of this because it should be unnecessary - or should move to ShowDetailsManual()
                        //Select unposted batch row in same index position as batch just posted
                        grdDetails.DataSource = null;
                        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            // TODO: display progress of posting
            TVerificationResultCollection Verifications;

            if (!SaveBatchForPosting())
            {
                return;
            }

            //get index position of row to post
            int newCurrentRowPos = GetSelectedRowIndex();

            if ((dtpDetailDateEffective.Date < FStartDateCurrentPeriod) || (dtpDetailDateEffective.Date > FEndDateLastForwardingPeriod))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The Date Effective is outside the periods available for posting. Enter a date between {0:d} and {1:d}."),
                        FStartDateCurrentPeriod,
                        FEndDateLastForwardingPeriod));

                return;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString("Are you sure you want to post batch {0}?"),
                        FSelectedBatchNumber),
                    Catalog.GetString("Question"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                if (!TRemote.MFinance.GL.WebConnectors.PostGLBatch(FLedgerNumber, FSelectedBatchNumber, out Verifications))
                {
                    string ErrorMessages = String.Empty;

                    foreach (TVerificationResult verif in Verifications)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    // TODO: print reports on successfully posted batch
                    MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // refresh the grid, to reflect that the batch has been posted
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndContent(FLedgerNumber, FSelectedBatchNumber));

                    // make sure that the current dataset is clean,
                    // otherwise the next save would try to modify the posted batch, even though no values have been changed
                    FMainDS.AcceptChanges();

                    // AlanP: You must not set FPreviouslySelectedDetailRow = null because it is owned by grid events
                    this.FPreviouslySelectedDetailRow = null;
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();
                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                    LoadBatchesForCurrentYear();

                    // AlanP - commenting out most of this because it should be unnecessary - or should move to ShowDetailsManual()
                    ////Select unposted batch row in same index position as batch just posted
                    grdDetails.DataSource = null;
                    grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

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
        }

        /// <summary>
        /// this function calculates the balances of the accounts involved, if this batch would be posted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestPostBatch(System.Object sender, EventArgs e)
        {
            TVerificationResultCollection Verifications;

            if (!SaveBatchForPosting())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            List <TVariant>Result = TRemote.MFinance.GL.WebConnectors.TestPostGLBatch(FLedgerNumber, FSelectedBatchNumber, out Verifications);

            try
            {
                if ((Verifications != null) && (Verifications.Count > 0))
                {
                    string ErrorMessages = string.Empty;

                    foreach (TVerificationResult verif in Verifications)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (Result.Count < 25)
                {
                    string message = string.Empty;

                    foreach (TVariant value in Result)
                    {
                        ArrayList compValues = value.ToComposite();

                        message +=
                            string.Format(
                                Catalog.GetString("{1}/{0} ({3}/{2}) is: {4} and would be: {5}"),
                                ((TVariant)compValues[0]).ToString(),
                                ((TVariant)compValues[2]).ToString(),
                                ((TVariant)compValues[1]).ToString(),
                                ((TVariant)compValues[3]).ToString(),
                                StringHelper.FormatCurrency((TVariant)compValues[4], "currency"),
                                StringHelper.FormatCurrency((TVariant)compValues[5], "currency")) +
                            Environment.NewLine;
                    }

                    MessageBox.Show(message, Catalog.GetString("Result of Test Posting"));
                }
                else
                {
                    // store to CSV file
                    string message = string.Empty;

                    foreach (TVariant value in Result)
                    {
                        ArrayList compValues = value.ToComposite();

                        string[] columns = new string[] {
                            ((TVariant)compValues[0]).ToString(),
                            ((TVariant)compValues[1]).ToString(),
                            ((TVariant)compValues[2]).ToString(),
                            ((TVariant)compValues[3]).ToString(),
                            StringHelper.FormatCurrency((TVariant)compValues[4], "CurrencyCSV"),
                            StringHelper.FormatCurrency((TVariant)compValues[5], "CurrencyCSV")
                        };

                        message += StringHelper.StrMerge(columns,
                            Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator[0]) +
                                   Environment.NewLine;
                    }

                    string CSVFilePath = TClientSettings.PathLog + Path.DirectorySeparatorChar + "Batch" + FSelectedBatchNumber.ToString() +
                                         "_TestPosting.csv";
                    StreamWriter sw = new StreamWriter(CSVFilePath, false, System.Text.Encoding.UTF8);
                    sw.Write(message);
                    sw.Close();

                    MessageBox.Show(
                        String.Format(Catalog.GetString("Please see file {0} for the result of the test posting"), CSVFilePath),
                        Catalog.GetString("Result of Test Posting"));
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
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

        ///// <summary>
        ///// Set focus to the gid controltab
        ///// </summary>
        //public void FocusGrid()
        //{
        //    if ((grdDetails != null) && grdDetails.CanFocus)
        //    {
        //        grdDetails.Focus();
        //    }
        //}

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            //if (FInitialFocusActionComplete)
            //{
            //    return;
            //}

            //if (grdDetails.CanFocus)
            //{
            //Set filter to current and forwarding
            //if (FcmbPeriod.Items.Count > 0)
            //{
            //    FcmbPeriod.SelectedIndex = 1;
            //}

            if (grdDetails.Rows.Count <= 1)
            {
                btnNew.Focus();
            }
            else
            {
                grdDetails.Focus();
            }

            //FInitialFocusActionComplete = true;
            //}
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowJournalTab);
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);
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
        public void ImportBatches(object sender, EventArgs e)
        {
            if (!SaveChangesAndResetFilter())
            {
                return;
            }

            FImportLogicObject.ImportBatches();
        }

        private void ImportFromClipboard(object sender, EventArgs e)
        {
            if (!SaveChangesAndResetFilter())
            {
                return;
            }

            FImportLogicObject.ImportFromClipboard();
        }

        /// <summary>
        /// Public method called from the transactions tab
        /// </summary>
        public void ImportTransactions()
        {
            FImportLogicObject.ImportTransactions(FPreviouslySelectedDetailRow, GetCurrentJournal());
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