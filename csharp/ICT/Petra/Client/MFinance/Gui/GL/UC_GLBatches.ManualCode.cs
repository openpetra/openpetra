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
    public partial class TUC_GLBatches
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FSelectedBatchNumber = -1;
        private string FStatusFilter = "1 = 1";
        private string FPeriodFilter = "1 = 1";
        private DateTime StartDateCurrentPeriod;
        private DateTime EndDateLastForwardingPeriod;
        private string FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_EDITING;
        private Int32 FSelectedYear;
        private Int32 FSelectedPeriod = -1;
        private string FPeriodText = String.Empty;
        private DateTime FDefaultDate;

        private bool FSuppressRefreshFilter = false;
        private bool FSuppressRefreshPeriods = false;
        private DateTime FCurrentEffectiveDate;
        private bool FBatchesLoaded = false;

        TCmbAutoComplete FcmbYearFilter = null;
        TCmbAutoComplete FcmbPeriodFilter = null;
        RadioButton FrbtEditing = null;
        RadioButton FrbtPosting = null;
        RadioButton FrbtAll = null;

        private void InitialiseControls()
        {
            FcmbYearFilter = (TCmbAutoComplete)FFilterPanelControls.FindControlByName("cmbYearFilter");
            FcmbPeriodFilter = (TCmbAutoComplete)FFilterPanelControls.FindControlByName("cmbPeriodFilter");
            FrbtEditing = (RadioButton)FFilterPanelControls.FindControlByName("rbtEditing");
            FrbtPosting = (RadioButton)FFilterPanelControls.FindControlByName("rbtPosting");
            FrbtAll = (RadioButton)FFilterPanelControls.FindControlByName("rbtAll");
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FBatchesLoaded = false;
            InitialiseControls();

            FLedgerNumber = ALedgerNumber;

            FrbtEditing.Checked = true;

            // This will populate the periods combos without firing off cascading events
            FSuppressRefreshPeriods = true;
            TFinanceControls.InitialiseAvailableFinancialYearsList(ref FcmbYearFilter, FLedgerNumber); //.InitialiseAvailableGiftYearsList(ref FcmbYearFilter, FLedgerNumber);
            FSuppressRefreshPeriods = false;

            // Now we can set the period part of the filter
            RefreshPeriods(null, null);

            // this will load the batches from the server
            //RefreshFilter(null, null);
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

            //Set the valid date range label
            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber,
                out StartDateCurrentPeriod,
                out EndDateLastForwardingPeriod,
                out FDefaultDate);

            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                StringHelper.DateToLocalizedString(StartDateCurrentPeriod, false, false),
                StringHelper.DateToLocalizedString(EndDateLastForwardingPeriod, false, false));

            //Set sort order
            FMainDS.ABatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ABatchTable.GetLedgerNumberDBName(),
                ABatchTable.GetBatchNumberDBName()
                );

            FBatchesLoaded = true;
        }

        /// Reset the control
        public void ClearCurrentSelection()
        {
            if (FPetraUtilsObject.HasChanges)
            {
                GetDataFromControls();
            }

            this.FPreviouslySelectedDetailRow = null;
            ShowData();
        }

        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS BatchFMainDS()
        {
            return FMainDS;
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

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
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

            if (FPreviouslySelectedDetailRow == null)
            {
                if (((TFrmGLBatch) this.ParentForm) != null)
                {
                    ((TFrmGLBatch) this.ParentForm).DisableJournals();
                }
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
                FValidationControlsDict);

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

            //This line stops the date changing when a new row is selected if AllowVerification=False
            //dtpDetailDateEffective.AllowVerification = !FPetraUtilsObject.DetailProtectedMode;

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

        private void ClearDetailControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            txtDetailBatchDescription.Text = string.Empty;
            txtDetailBatchControlTotal.NumberValueDecimal = 0;
            dtpDetailDateEffective.Date = FDefaultDate;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (!FrbtEditing.Checked)
            {
                FrbtEditing.Checked = true;
            }
            else if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            //Set year and period to correct value
            if (FcmbYearFilter.SelectedIndex != 0)
            {
                FcmbYearFilter.SelectedIndex = 0;
            }
            else if (FcmbPeriodFilter.SelectedIndex != 1)
            {
                FcmbPeriodFilter.SelectedIndex = 1;
            }

            string rowFilter = String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);
            FFilterPanelControls.SetBaseFilter(rowFilter, (FSelectedPeriod == 0) && (FCurrentBatchViewOption == MFinanceConstants.GIFT_BATCH_VIEW_ALL));
            ApplyFilter();

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

        private bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            bool RetVal;

            RetVal = TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);

            return RetVal;
        }

        private void UpdateBatchPeriod(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            bool UpdateTransactionDates = false;

            try
            {
                Int32 periodNumber = 0;
                Int32 yearNumber = 0;
                DateTime dateValue;
                string aDate = dtpDetailDateEffective.Date.ToString();

                if (DateTime.TryParse(aDate, out dateValue))
                {
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

                            if (FcmbYearFilter.SelectedIndex != 0)
                            {
                                FcmbYearFilter.SelectedIndex = 0;
                                FcmbPeriodFilter.SelectedIndex = 1;
                                dtpDetailDateEffective.Date = dateValue;
                                dtpDetailDateEffective.Focus();
                            }
                            else if (FcmbPeriodFilter.SelectedIndex != 1)
                            {
                                FcmbPeriodFilter.SelectedIndex = 1;
                                dtpDetailDateEffective.Date = dateValue;
                                dtpDetailDateEffective.Focus();
                            }
                        }
                    }

                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().UpdateTransactionAmounts("BATCH", UpdateTransactionDates);
                    FPetraUtilsObject.HasChanges = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        /// <summary>
        /// UpdateTotals
        /// </summary>
        public void UpdateTotals()
        {
            //Below not needed as yet
            if (FPreviouslySelectedDetailRow != null)
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                GLRoutines.UpdateTotalsOfBatch(ref FMainDS, FPreviouslySelectedDetailRow);
                txtDetailBatchControlTotal.NumberValueDecimal = FPreviouslySelectedDetailRow.BatchControlTotal;
                FPetraUtilsObject.EnableDataChangedEvent();
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
            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;
            DateTime DateForReverseBatch = dtpDetailDateEffective.Date.Value;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                TRemote.MFinance.GL.WebConnectors.GetCurrentPostingRangeDates(FMainDS.ALedger[0].LedgerNumber,
                    out StartDateCurrentPeriod,
                    out EndDateLastForwardingPeriod);

                if ((DateForReverseBatch.Date < StartDateCurrentPeriod) || (DateForReverseBatch.Date > EndDateLastForwardingPeriod))
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The Reverse Date is outside the periods available for reversing. We will set the posting date to the first possible date, {0}."),
                            StartDateCurrentPeriod));
                    DateForReverseBatch = StartDateCurrentPeriod;
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

                        LoadBatches(FLedgerNumber);

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
                        SetRecordNumberDisplayProperties();
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

            //TODO: Correct this if needed
            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;

            TRemote.MFinance.GL.WebConnectors.GetCurrentPostingRangeDates(FMainDS.ALedger[0].LedgerNumber,
                out StartDateCurrentPeriod,
                out EndDateLastForwardingPeriod);

            if ((dtpDetailDateEffective.Date < StartDateCurrentPeriod) || (dtpDetailDateEffective.Date > EndDateLastForwardingPeriod))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The Date Effective is outside the periods available for posting. Enter a date between {0:d} and {1:d}."),
                        StartDateCurrentPeriod,
                        EndDateLastForwardingPeriod));

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

                    LoadBatches(FLedgerNumber);

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
                    SetRecordNumberDisplayProperties();
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

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void RefreshPeriods(Object sender, EventArgs e)
        {
            int NewYearSelected;
            bool IncludeCurrentAndForwardingItem = true;

            if (FSuppressRefreshPeriods)
            {
                return;
            }

            FSuppressRefreshFilter = true;

            NewYearSelected = FcmbYearFilter.GetSelectedInt32();

            if (FSelectedYear == NewYearSelected)
            {
                FSuppressRefreshFilter = false;
                return;
            }

            FSelectedYear = NewYearSelected;

            if (sender is TCmbAutoComplete)
            {
                FPetraUtilsObject.ClearControl(FcmbPeriodFilter);
            }

            FSuppressRefreshFilter = false;

            //Determine whether or not to include the "Current and forwarding periods" item in the period combo
            if (FMainDS.ALedger.Rows.Count == 1)
            {
                IncludeCurrentAndForwardingItem = (FSelectedYear == FMainDS.ALedger[0].CurrentFinancialYear);
            }

            if (sender != null)
            {
                RefreshFilter(sender, e);
            }

            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref FcmbPeriodFilter,
                FLedgerNumber,
                FSelectedYear,
                0,
                IncludeCurrentAndForwardingItem);
        }

        void RefreshFilter(Object sender, EventArgs e)
        {
            int batchNumber = 0;

            if (FSuppressRefreshFilter
                || (FPetraUtilsObject == null)
                || FPetraUtilsObject.SuppressChangeDetection
                || ((sender != null) && sender is RadioButton && (((RadioButton)sender).Checked == false)))
            {
                return;
            }

            if ((sender != null) && sender is RadioButton)
            {
                //Avoid repeat events
                RadioButton rbt = (RadioButton)sender;

                if (rbt.Name.Contains(FCurrentBatchViewOption))
                {
                    return;
                }
            }

            if (sender is TCmbAutoComplete)
            {
                if (FucoFilterAndFind.CanIgnoreChangeEvent)
                {
                    return;
                }

                int newYear = FcmbYearFilter.GetSelectedInt32();
                int newPeriod = FcmbPeriodFilter.GetSelectedInt32();
                string newPeriodText = FcmbPeriodFilter.Text;

                if (FSelectedYear == newYear)
                {
                    if ((newPeriod == -1) && (newPeriodText != String.Empty))
                    {
                        Console.WriteLine("Skipping period {0} periodText {1}", newPeriod, newPeriodText);
                        return;
                    }

                    if ((newPeriod == FSelectedPeriod) && (newPeriodText == FPeriodText))
                    {
                        Console.WriteLine("Skipping period {0} periodText {1}", newPeriod, newPeriodText);
                        return;
                    }
                }

                Console.WriteLine("Using period {0} periodText {1}", newPeriod, newPeriodText);
            }

            //Record the current batch
            if (FPreviouslySelectedDetailRow != null)
            {
                batchNumber = FPreviouslySelectedDetailRow.BatchNumber;
            }

            ClearCurrentSelection();

            FSelectedYear = FcmbYearFilter.GetSelectedInt32();
            FSelectedPeriod = FcmbPeriodFilter.GetSelectedInt32();
            FPeriodText = FcmbPeriodFilter.Text;

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            int CurrentLedgerYear = LedgerRow.CurrentFinancialYear;
            int CurrentLedgerPeriod = LedgerRow.CurrentPeriod;

            if (FSelectedYear == -1)
            {
                FPeriodFilter = String.Format(
                    "{0} = {1}",
                    ABatchTable.GetBatchYearDBName(), CurrentLedgerYear);
            }
            else
            {
                FPeriodFilter = String.Format(
                    "{0} = {1}",
                    ABatchTable.GetBatchYearDBName(), FSelectedYear);

                if (FSelectedPeriod == -2)  //All periods for year
                {
                    //Nothing to add to filter
                }
                else if (FSelectedPeriod == 0)
                {
                    FPeriodFilter += String.Format(
                        " AND {0} >= {1}",
                        ABatchTable.GetBatchPeriodDBName(), CurrentLedgerPeriod);
                }
                else if (FSelectedPeriod > 0)
                {
                    FPeriodFilter += String.Format(
                        " AND {0} = {1}",
                        ABatchTable.GetBatchPeriodDBName(), FSelectedPeriod);
                }
            }

            Console.WriteLine(" ** " + FPeriodFilter);

            if (FrbtEditing.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_EDITING;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfEditing, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
                btnNew.Enabled = true;
            }
            else if (FrbtPosting.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_POSTING;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfReadyForPosting, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = String.Format("({0} = '{1}') AND ({2} = {3}) AND ({2} <> 0) AND (({4} = 0) OR ({4} = {2}))",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED,
                    ABatchTable.GetBatchCreditTotalDBName(),
                    ABatchTable.GetBatchDebitTotalDBName(),
                    ABatchTable.GetBatchControlTotalDBName());
            }
            else //(FrbtAll.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_ALL;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = "1 = 1";
                btnNew.Enabled = true;
            }

            RefreshGridData(batchNumber, (sender is TCmbAutoComplete));

            UpdateChangeableStatus();

            UpdateRecordNumberDisplay();
            Console.WriteLine("RefreshFilter - finished");
        }

        private void RefreshGridData(int ABatchNumber, bool ANoFocusChange, bool ASelectOnly = false)
        {
            string RowFilter = string.Empty;

            if (!ASelectOnly)
            {
                RowFilter = String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

                FFilterPanelControls.SetBaseFilter(RowFilter, (FSelectedPeriod == -1)
                    && (FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_ALL));
                ApplyFilter();
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
        /// Set focus to the gid controltab
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }

        private void ImportFromSpreadSheet(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import transactions from spreadsheet file");
            dialog.Filter = Catalog.GetString("Spreadsheet files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string directory = Path.GetDirectoryName(dialog.FileName);
                string[] ymlFiles = Directory.GetFiles(directory, "*.yml");
                string definitionFileName = String.Empty;

                if (ymlFiles.Length == 1)
                {
                    definitionFileName = ymlFiles[0];
                }
                else
                {
                    // show another open file dialog for the description file
                    OpenFileDialog dialogDefinitionFile = new OpenFileDialog();
                    dialogDefinitionFile.Title = Catalog.GetString("Please select a yml file that describes the content of the spreadsheet");
                    dialogDefinitionFile.Filter = Catalog.GetString("Data description files (*.yml)|*.yml");

                    if (dialogDefinitionFile.ShowDialog() == DialogResult.OK)
                    {
                        definitionFileName = dialogDefinitionFile.FileName;
                    }
                }

                if (File.Exists(definitionFileName))
                {
                    TYml2Xml parser = new TYml2Xml(definitionFileName);
                    XmlDocument dataDescription = parser.ParseYML2XML();
                    XmlNode RootNode = TXMLParser.FindNodeRecursive(dataDescription.DocumentElement, "RootNode");
                    Int32 FirstTransactionRow = TXMLParser.GetIntAttribute(RootNode, "FirstTransactionRow");
                    string DefaultCostCentre = TXMLParser.GetAttribute(RootNode, "CostCentre");

                    CreateNewABatch();
                    GLBatchTDSAJournalRow NewJournal = FMainDS.AJournal.NewRowTyped(true);
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().NewRowManual(ref NewJournal);
                    FMainDS.AJournal.Rows.Add(NewJournal);
                    NewJournal.TransactionCurrency = TXMLParser.GetAttribute(RootNode, "Currency");

                    if (Path.GetExtension(dialog.FileName).ToLower() == ".csv")
                    {
                        CreateBatchFromCSVFile(dialog.FileName,
                            RootNode,
                            NewJournal,
                            FirstTransactionRow,
                            DefaultCostCentre);
                    }
                }
            }
        }

        /// load transactions from a CSV file into the currently selected batch
        private void CreateBatchFromCSVFile(string ADataFilename,
            XmlNode ARootNode,
            AJournalRow ARefJournalRow,
            Int32 AFirstTransactionRow,
            string ADefaultCostCentre)
        {
            StreamReader dataFile = new StreamReader(ADataFilename, System.Text.Encoding.Default);

            XmlNode ColumnsNode = TXMLParser.GetChild(ARootNode, "Columns");
            string Separator = TXMLParser.GetAttribute(ARootNode, "Separator");
            string DateFormat = TXMLParser.GetAttribute(ARootNode, "DateFormat");
            string ThousandsSeparator = TXMLParser.GetAttribute(ARootNode, "ThousandsSeparator");
            string DecimalSeparator = TXMLParser.GetAttribute(ARootNode, "DecimalSeparator");
            Int32 lineCounter;

            // read headers
            for (lineCounter = 0; lineCounter < AFirstTransactionRow - 1; lineCounter++)
            {
                dataFile.ReadLine();
            }

            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;
            DateTime LatestTransactionDate = DateTime.MinValue;

            do
            {
                string line = dataFile.ReadLine();
                lineCounter++;

                GLBatchTDSATransactionRow NewTransaction = FMainDS.ATransaction.NewRowTyped(true);
                ((TFrmGLBatch)ParentForm).GetTransactionsControl().NewRowManual(ref NewTransaction, ARefJournalRow);
                FMainDS.ATransaction.Rows.Add(NewTransaction);

                foreach (XmlNode ColumnNode in ColumnsNode.ChildNodes)
                {
                    string Value = StringHelper.GetNextCSV(ref line, Separator);
                    string UseAs = TXMLParser.GetAttribute(ColumnNode, "UseAs");

                    if (UseAs.ToLower() == "reference")
                    {
                        NewTransaction.Reference = Value;
                    }
                    else if (UseAs.ToLower() == "narrative")
                    {
                        NewTransaction.Narrative = Value;
                    }
                    else if (UseAs.ToLower() == "dateeffective")
                    {
                        NewTransaction.SetTransactionDateNull();

                        if (Value.Trim().ToString().Length > 0)
                        {
                            try
                            {
                                NewTransaction.TransactionDate = XmlConvert.ToDateTime(Value, DateFormat);

                                if (NewTransaction.TransactionDate > LatestTransactionDate)
                                {
                                    LatestTransactionDate = NewTransaction.TransactionDate;
                                }
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show(Catalog.GetString("Problem with date in row " + lineCounter.ToString() + " Error: " + exp.Message));
                            }
                        }
                    }
                    else if (UseAs.ToLower() == "account")
                    {
                        if (Value.Length > 0)
                        {
                            if (Value.Contains(" "))
                            {
                                // cut off currency code; should have been defined in the data description file, for the whole batch
                                Value = Value.Substring(0, Value.IndexOf(" ") - 1);
                            }

                            Value = Value.Replace(ThousandsSeparator, "");
                            Value = Value.Replace(DecimalSeparator, ".");

                            NewTransaction.TransactionAmount = Convert.ToDecimal(Value, System.Globalization.CultureInfo.InvariantCulture);
                            NewTransaction.CostCentreCode = ADefaultCostCentre;
                            NewTransaction.AccountCode = ColumnNode.Name;
                            NewTransaction.DebitCreditIndicator = true;

                            if (TXMLParser.HasAttribute(ColumnNode,
                                    "CreditDebit") && (TXMLParser.GetAttribute(ColumnNode, "CreditDebit").ToLower() == "credit"))
                            {
                                NewTransaction.DebitCreditIndicator = false;
                            }

                            if (NewTransaction.TransactionAmount < 0)
                            {
                                NewTransaction.TransactionAmount *= -1.0M;
                                NewTransaction.DebitCreditIndicator = !NewTransaction.DebitCreditIndicator;
                            }

                            if (TXMLParser.HasAttribute(ColumnNode, "AccountCode"))
                            {
                                NewTransaction.AccountCode = TXMLParser.GetAttribute(ColumnNode, "AccountCode");
                            }

                            if (NewTransaction.DebitCreditIndicator)
                            {
                                sumDebits += NewTransaction.TransactionAmount;
                            }
                            else if (!NewTransaction.DebitCreditIndicator)
                            {
                                sumCredits += NewTransaction.TransactionAmount;
                            }
                        }
                    }
                }

                if (!NewTransaction.IsTransactionDateNull())
                {
                    NewTransaction.AmountInBaseCurrency = GLRoutines.Multiply(NewTransaction.TransactionAmount,
                        TExchangeRateCache.GetDailyExchangeRate(
                            ARefJournalRow.TransactionCurrency,
                            FMainDS.ALedger[0].BaseCurrency,
                            NewTransaction.TransactionDate));
                    //
                    // The International currency calculation is changed to "Base -> International", because it's likely
                    // we won't have a "Transaction -> International" conversion rate defined.
                    //
                    NewTransaction.AmountInIntlCurrency = GLRoutines.Multiply(NewTransaction.AmountInBaseCurrency,
                        TExchangeRateCache.GetDailyExchangeRate(
                            FMainDS.ALedger[0].BaseCurrency,
                            FMainDS.ALedger[0].IntlCurrency,
                            NewTransaction.TransactionDate));
                }
            } while (!dataFile.EndOfStream);

            // create a balancing transaction; not sure if this is needed at all???
            if (Convert.ToDecimal(sumCredits - sumDebits) != 0)
            {
                GLBatchTDSATransactionRow BalancingTransaction = FMainDS.ATransaction.NewRowTyped(true);
                ((TFrmGLBatch)ParentForm).GetTransactionsControl().NewRowManual(ref BalancingTransaction, ARefJournalRow);
                FMainDS.ATransaction.Rows.Add(BalancingTransaction);

                BalancingTransaction.TransactionDate = LatestTransactionDate;
                BalancingTransaction.DebitCreditIndicator = true;
                BalancingTransaction.TransactionAmount = sumCredits - sumDebits;

                if (BalancingTransaction.TransactionAmount < 0)
                {
                    BalancingTransaction.TransactionAmount *= -1;
                    BalancingTransaction.DebitCreditIndicator = !BalancingTransaction.DebitCreditIndicator;
                }

                if (BalancingTransaction.DebitCreditIndicator)
                {
                    sumDebits += BalancingTransaction.TransactionAmount;
                }
                else
                {
                    sumCredits += BalancingTransaction.TransactionAmount;
                }

                BalancingTransaction.AmountInIntlCurrency = GLRoutines.Multiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].IntlCurrency,
                        BalancingTransaction.TransactionDate));
                BalancingTransaction.AmountInBaseCurrency = GLRoutines.Multiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].BaseCurrency,
                        BalancingTransaction.TransactionDate));
                BalancingTransaction.Narrative = Catalog.GetString("Automatically generated balancing transaction");
                BalancingTransaction.CostCentreCode = TXMLParser.GetAttribute(ARootNode, "CashCostCentre");
                BalancingTransaction.AccountCode = TXMLParser.GetAttribute(ARootNode, "CashAccount");
            }

            ARefJournalRow.JournalCreditTotal = sumCredits;
            ARefJournalRow.JournalDebitTotal = sumDebits;
            ARefJournalRow.JournalDescription = Path.GetFileNameWithoutExtension(ADataFilename);

            dtpDetailDateEffective.Date = LatestTransactionDate;
            txtDetailBatchDescription.Text = Path.GetFileNameWithoutExtension(ADataFilename);
            ABatchRow RefBatch = (ABatchRow)FMainDS.ABatch.Rows[FMainDS.ABatch.Rows.Count - 1];
            RefBatch.BatchCreditTotal = sumCredits;
            RefBatch.BatchDebitTotal = sumDebits;
            // todo RefBatch.BatchControlTotal = sumCredits  - sumDebits;
            // csv !
        }

        private void ImportBatches(object sender, EventArgs e)
        {
            ImportBatches();
        }

        private void ImportFromClipboard(object sender, EventArgs e)
        {
            ImportFromClipboard();
        }

        private void ExportBatches(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please save changed Data before the Export!"),
                    Catalog.GetString("Export Error"));
                return;
            }

            TFrmGLBatchExport gl = new TFrmGLBatchExport(FPetraUtilsObject.GetForm());
            gl.LedgerNumber = FLedgerNumber;
            gl.MainDS = FMainDS;
            gl.Show();
        }

        private void ToggleOptionButtonCheckedEvent(bool AToggleOn)
        {
            if (AToggleOn)
            {
                FrbtEditing.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                FrbtAll.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                FrbtPosting.CheckedChanged += new System.EventHandler(this.RefreshFilter);
            }
            else
            {
                FrbtEditing.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                FrbtAll.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                FrbtPosting.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
            }
        }

        /// <summary>
        /// A simple flag used to indicate that the form has been shown for the first time
        /// </summary>
        private bool FInitialFocusActionComplete = false;

        /// <summary>
        /// Sets the initial focus to the grid or the New button depending on the row count
        /// </summary>
        public void SetInitialFocus()
        {
            if (FInitialFocusActionComplete)
            {
                return;
            }

            if (grdDetails.CanFocus)
            {
                //Set filter to current and forwarding
                if (FcmbPeriodFilter.Items.Count > 0)
                {
                    FcmbPeriodFilter.SelectedIndex = 1;
                }

                if (grdDetails.Rows.Count < 2)
                {
                    btnNew.Focus();
                }
                else
                {
                    grdDetails.Focus();
                }

                FInitialFocusActionComplete = true;
            }
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DoubleClickHeaderCell += new TDoubleClickHeaderCellEventHandler(grdDetails_DoubleClickHeaderCell);
            grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowJournalTab);
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

            AutoSizeGrid();
        }

        /// <summary>
        /// Fired when the user double clicks a header cell.  We use this to autoSize the specified column.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void grdDetails_DoubleClickHeaderCell(object Sender, SourceGrid.ColumnEventArgs e)
        {
            if ((grdDetails.Columns[e.Column].AutoSizeMode & SourceGrid.AutoSizeMode.EnableAutoSize) == SourceGrid.AutoSizeMode.None)
            {
                grdDetails.Columns[e.Column].AutoSizeMode |= SourceGrid.AutoSizeMode.EnableAutoSize;
                grdDetails.AutoSizeCells(new SourceGrid.Range(1, e.Column, grdDetails.Rows.Count - 1, e.Column));
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                AutoSizeGrid();
            }
        }

        /// <summary>
        /// AutoSize the grid columns (call this after the window has been restored to normal size after being maximized)
        /// </summary>
        public void AutoSizeGrid()
        {
            //TODO: Using this manual code until we can do something better
            //      Autosizing all the columns is very time consuming when there are many rows
            foreach (SourceGrid.DataGridColumn column in grdDetails.Columns)
            {
                column.Width = 100;
                column.AutoSizeMode = SourceGrid.AutoSizeMode.EnableStretch;
            }

            grdDetails.Columns[0].Width = 80;
            grdDetails.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.Default;

            grdDetails.AutoStretchColumnsToFitWidth = true;
            grdDetails.Rows.AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdDetails.AutoSizeCells();
            grdDetails.ShowCell(FPrevRowChangedRow);
        }

        private void CreateFilterFindPanelsManual()
        {
            ((Label)FFindPanelControls.FindControlByName("lblBatchNumber")).Text = "Batch number";
        }
    }
}