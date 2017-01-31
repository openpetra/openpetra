//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLBatches
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FSelectedBatchNumber = -1;
        private DateTime FDefaultDate = DateTime.Today;
        private bool FInitialFocusActionComplete = false;

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        /// <summary>
        /// FInactiveValuesWarningOnGLSubmitting takes User Setting for Warn on GL Posting
        /// </summary>
        public bool FInactiveValuesWarningOnGLSubmitting = false;

        /// <summary>
        ///List of all batches and whether or not the user has been warned of the presence
        /// of inactive fields on saving.
        /// </summary>
        public Dictionary <int, bool>FRecurringBatchesVerifiedOnSavingDict = new Dictionary <int, bool>();

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private void InitialiseControls()
        {
            //Leave in for future use
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            InitialiseControls();

            FLedgerNumber = ALedgerNumber;

            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll));

            btnNew.Enabled = true;

            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringBatch.DefaultView);

            if (grdDetails.Rows.Count > 1)
            {
                SelectRowInGrid(1);
                ((TFrmRecurringGLBatch) this.ParentForm).EnableJournals();
            }
            else
            {
                ClearControls();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
            }

            //Load all analysis attribute values
            ShowData();

            UpdateChangeableStatus();
            UpdateRecordNumberDisplay();
            SetAccountCostCentreTableVariables();
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
                LoadJournalsForCurrentRecurringBatch();

                EnableTransTab = (FMainDS.ARecurringJournal.DefaultView.Count > 0);
            }

            ((TFrmRecurringGLBatch) this.ParentForm).EnableTransactions(EnableTransTab);
        }

        private void LoadJournalsForCurrentRecurringBatch()
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            //Current Batch number
            Int32 BatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            if (FMainDS.ARecurringJournal != null)
            {
                FMainDS.ARecurringJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    BatchNumber);

                if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
                {
                    ((TFrmRecurringGLBatch) this.ParentForm).LoadJournals(FPreviouslySelectedDetailRow);
                }
            }
        }

        private void ShowDataManual()
        {
            if (FLedgerNumber == -1)
            {
                EnableButtonControl(false);
            }
        }

        private void UpdateChangeableStatus()
        {
            Boolean Submitable = ((FPreviouslySelectedDetailRow != null)
                                  && (grdDetails.Rows.Count > 1));

            FPetraUtilsObject.EnableAction("actSubmitBatch", Submitable);
            FPetraUtilsObject.EnableAction("actDelete", Submitable);
            pnlDetails.Enabled = Submitable;
            pnlDetailsProtected = !Submitable;

            if ((FPreviouslySelectedDetailRow == null) && (((TFrmRecurringGLBatch) this.ParentForm) != null))
            {
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
            }
        }

        /// <summary>
        /// Call ShowDetails() from outside form
        /// </summary>
        public void ShowDetailsRefresh()
        {
            ShowDetails();
        }

        /// <summary>
        /// Checks various things on the form before saving
        /// </summary>
        public void CheckBeforeSaving()
        {
            UpdateRecurringBatchDictionary();
        }

        /// <summary>
        /// Update the dictionary that stores all unposted batches
        ///  and whether or not they have been warned about inactive
        ///   fields
        /// </summary>
        /// <param name="ABatchNumberToExclude"></param>
        public void UpdateRecurringBatchDictionary(int ABatchNumberToExclude = 0)
        {
            if (ABatchNumberToExclude > 0)
            {
                FRecurringBatchesVerifiedOnSavingDict.Remove(ABatchNumberToExclude);
            }

            DataView BatchDV = new DataView(FMainDS.ARecurringBatch);

            foreach (DataRowView bRV in BatchDV)
            {
                ARecurringBatchRow br = (ARecurringBatchRow)bRV.Row;

                int currentBatch = br.BatchNumber;

                if ((currentBatch != ABatchNumberToExclude) && !FRecurringBatchesVerifiedOnSavingDict.ContainsKey(currentBatch))
                {
                    FRecurringBatchesVerifiedOnSavingDict.Add(br.BatchNumber, false);
                }
            }
        }

        /// <summary>
        /// Get current Batch row
        /// </summary>
        /// <returns></returns>
        public ARecurringBatchRow GetCurrentBatchRow()
        {
            return (ARecurringBatchRow) this.GetSelectedDetailRow();
        }

        private ARecurringJournalRow GetCurrentJournalRow()
        {
            return (ARecurringJournalRow)((TFrmRecurringGLBatch) this.ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        private void ValidateDataDetailsManual(ARecurringBatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            ParseHashTotal(ARow);

            TSharedFinanceValidation_GL.ValidateRecurringGLBatchManual(this,
                ARow,
                ref VerificationResultCollection,
                FValidationControlsDict);

            //TODO: remove this once database definition is set for Batch Description to be NOT NULL
            // Description is mandatory then make sure it is set
            if (txtDetailBatchDescription.Text.Length == 0)
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringBatchTable.ColumnBatchDescriptionId];
                ValidationContext = String.Format("Recurring Batch number {0}",
                    ARow.BatchNumber);

                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.BatchDescription,
                    "Description of " + ValidationContext,
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }

        private void ParseHashTotal(ARecurringBatchRow ARow)
        {
            if ((txtDetailBatchControlTotal.NumberValueDecimal == null) || !txtDetailBatchControlTotal.NumberValueDecimal.HasValue)
            {
                bool prev = FPetraUtilsObject.SuppressChangeDetection;
                FPetraUtilsObject.SuppressChangeDetection = true;
                txtDetailBatchControlTotal.NumberValueDecimal = 0m;
                FPetraUtilsObject.SuppressChangeDetection = prev;
            }

            if (ARow.BatchControlTotal != txtDetailBatchControlTotal.NumberValueDecimal.Value)
            {
                ARow.BatchControlTotal = txtDetailBatchControlTotal.NumberValueDecimal.Value;
            }
        }

        private void ShowDetailsManual(ARecurringBatchRow ARow)
        {
            AutoEnableTransTabForBatch();
            grdDetails.TabStop = (ARow != null);

            if (ARow == null)
            {
                pnlDetails.Enabled = false;
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
                EnableButtonControl(false);
                ClearControls();
                return;
            }

            FPetraUtilsObject.DetailProtectedMode = false;

            FSelectedBatchNumber = ARow.BatchNumber;

            UpdateChangeableStatus();
            ((TFrmRecurringGLBatch) this.ParentForm).EnableJournals();
        }

        /// <summary>
        /// This routine is called by a double click on a batch row, which means: Open the
        /// Journal Tab of this batch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowJournalTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TGLBatchEnums.eGLTabs.Journals);
        }

        /// <summary>
        /// Controls the enabled status of the Delete and Submit buttons
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

            btnSubmitBatch.Enabled = AEnable;
            btnDelete.Enabled = AEnable;
        }

        /// <summary>
        /// Undo all changes to the specified batch ready to cancel it.
        ///  This avoids unecessary validation errors when cancelling.
        /// </summary>
        /// <param name="ABatchToDelete"></param>
        /// <param name="ARedisplay"></param>
        public void PrepareBatchDataForDeleting(Int32 ABatchToDelete, Boolean ARedisplay)
        {
            //This code will only be called when the Batch tab is active.

            DataView GLBatchDV = new DataView(FMainDS.ARecurringBatch);
            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
            DataView TransAnalDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            GLBatchDV.RowFilter = String.Format("{0}={1}",
                ARecurringBatchTable.GetBatchNumberDBName(),
                ABatchToDelete);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ABatchToDelete);

            TransDV.RowFilter = String.Format("{0}={1}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchToDelete);

            TransAnalDV.RowFilter = String.Format("{0}={1}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ABatchToDelete);

            //Work from lowest level up
            if (TransAnalDV.Count > 0)
            {
                TransAnalDV.Sort = String.Format("{0}, {1}, {2}",
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                foreach (DataRowView drv in TransAnalDV)
                {
                    ARecurringTransAnalAttribRow transAnalRow = (ARecurringTransAnalAttribRow)drv.Row;

                    if (transAnalRow.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (transAnalRow.RowState != DataRowState.Unchanged)
                    {
                        transAnalRow.RejectChanges();
                    }
                }
            }

            if (TransDV.Count > 0)
            {
                TransDV.Sort = String.Format("{0}, {1}",
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                foreach (DataRowView drv in TransDV)
                {
                    ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

                    if (transRow.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (transRow.RowState != DataRowState.Unchanged)
                    {
                        transRow.RejectChanges();
                    }
                }
            }

            if (JournalDV.Count > 0)
            {
                JournalDV.Sort = String.Format("{0}", ARecurringJournalTable.GetJournalNumberDBName());

                foreach (DataRowView drv in JournalDV)
                {
                    ARecurringJournalRow journalRow = (ARecurringJournalRow)drv.Row;

                    if (journalRow.RowState == DataRowState.Added)
                    {
                        //Do nothing
                    }
                    else if (journalRow.RowState != DataRowState.Unchanged)
                    {
                        journalRow.RejectChanges();
                    }
                }
            }

            if (GLBatchDV.Count > 0)
            {
                ARecurringBatchRow batchRow = (ARecurringBatchRow)GLBatchDV[0].Row;

                //No need to check for Added state as new batches are always saved
                // on creation

                if (batchRow.RowState != DataRowState.Unchanged)
                {
                    batchRow.RejectChanges();
                }

                if (ARedisplay)
                {
                    ShowDetails(batchRow);
                }
            }

            if (TransDV.Count == 0)
            {
                //Load all related data for batch ready to delete/cancel
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndRelatedTablesForBatch(FLedgerNumber, ABatchToDelete));
            }
        }

        private bool SaveOutstandingChanges()
        {
            bool RetVal = true;

            try
            {
                if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
                {
                    RetVal = false;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (CreateNewARecurringBatch())
            {
                if (!EnsureNewBatchIsVisible())
                {
                    return;
                }

                pnlDetails.Enabled = true;
                EnableButtonControl(true);
            }

            Int32 YearNumber;
            Int32 PeriodNumber;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out YearNumber, out PeriodNumber))
            {
                FPreviouslySelectedDetailRow.BatchPeriod = PeriodNumber;
            }

            FPreviouslySelectedDetailRow.DateEffective = FDefaultDate;
            FSelectedBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            UpdateRecordNumberDisplay();

            //Automatically save changes
            ((TFrmRecurringGLBatch) this.ParentForm).SaveChanges();

            //Enable the Journals if not already enabled
            ((TFrmRecurringGLBatch)ParentForm).EnableJournals();
        }

        private bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            return TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);
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
            FPetraUtilsObject.SuppressChangeDetection = true;
            txtDetailBatchDescription.Text = string.Empty;
            txtDetailBatchControlTotal.NumberValueDecimal = 0;
            FPetraUtilsObject.SuppressChangeDetection = false;
        }

        private int GetDataTableRowIndexByPrimaryKeys(int ALedgerNumber, int ABatchNumber)
        {
            int rowPos = 0;
            bool batchFound = false;

            foreach (DataRowView rowView in FMainDS.ARecurringBatch.DefaultView)
            {
                ARecurringBatchRow row = (ARecurringBatchRow)rowView.Row;

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
            if (FInitialFocusActionComplete)
            {
                return;
            }

            if (grdDetails.Rows.Count <= 1)
            {
                btnNew.Focus();
            }
            else if (grdDetails.CanFocus)
            {
                grdDetails.Focus();
            }

            FInitialFocusActionComplete = true;
        }

        private void RunOnceOnParentActivationManual()
        {
            try
            {
                ParentForm.Cursor = Cursors.WaitCursor;

                grdDetails.DoubleClickCell += new TDoubleClickCellEventHandler(this.ShowJournalTab);
                grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);

                txtDetailBatchControlTotal.CurrencyCode = TTxtCurrencyTextBox.CURRENCY_STANDARD_2_DP;

                LoadBatches(FLedgerNumber);
                SetInitialFocus();

                //Always warn until an option is added if required
                FInactiveValuesWarningOnGLSubmitting = true;
                // Keep this code for future option:
                // TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GL_WARN_OF_INACTIVE_VALUES_ON_SUBMITTING, true);
            }
            finally
            {
                ParentForm.Cursor = Cursors.Default;
            }
        }

        private void AutoResizeGrid()
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            AutoResizeGrid();
        }

        private void FilterToggledManual(bool AIsCollapsed)
        {
            AutoResizeGrid();
        }

        private void SubmitBatch(System.Object sender, EventArgs e)
        {
            if ((GetSelectedRowIndex() < 0) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("Please select a Recurring Batch before submitting!"));
                return;
            }

            TFrmRecurringGLBatch MainForm = (TFrmRecurringGLBatch)ParentForm;
            TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());
            TFrmRecurringGLBatchSubmit SubmitForm = null;

            bool LoadDialogVisible = false;

            try
            {
                Cursor = Cursors.WaitCursor;
                MainForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.SUBMITTING;

                dlgStatus.Show();
                LoadDialogVisible = true;
                dlgStatus.Heading = String.Format(Catalog.GetString("Recurring GL Batch {0}"), FSelectedBatchNumber);
                dlgStatus.CurrentStatus = Catalog.GetString("Loading journals and transactions ready for submitting...");

                if (!LoadAllBatchData())
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(Catalog.GetString("The Recurring GL Batch is empty!"),
                        Catalog.GetString("Submit GL Batch"),
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    dlgStatus.Close();
                    LoadDialogVisible = false;
                    return;
                }

                dlgStatus.Close();
                LoadDialogVisible = false;

                //Make sure that all control data is in dataset
                MainForm.GetLatestControlData();

                //Save and check for inactive values
                if (FPetraUtilsObject.HasChanges)
                {
                    //Keep this conditional check separate from the one above so that it only gets called
                    // when necessary and doesn't result in the executon of the same method
                    if (!MainForm.SaveChangesManual(MainForm.FCurrentGLBatchAction))
                    {
                        return;
                    }
                }
                else
                {
                    //This has to be called here because if there are no changes then the DataSavingValidating
                    // method which calls the method below, will not run.
                    if (!MainForm.GetTransactionsControl().AllowInactiveFieldValues(FLedgerNumber,
                            FSelectedBatchNumber, MainForm.FCurrentGLBatchAction))
                    {
                        return;
                    }
                }

                if ((FPreviouslySelectedDetailRow.BatchControlTotal != 0)
                    && (FPreviouslySelectedDetailRow.BatchDebitTotal != FPreviouslySelectedDetailRow.BatchControlTotal))
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The recurring gl batch total ({0}) for batch {1} does not equal the hash total ({2})."),
                            FPreviouslySelectedDetailRow.BatchDebitTotal,
                            FPreviouslySelectedDetailRow.BatchNumber,
                            FPreviouslySelectedDetailRow.BatchControlTotal));

                    txtDetailBatchControlTotal.Focus();
                    txtDetailBatchControlTotal.SelectAll();
                    return;
                }

                SubmitForm = new TFrmRecurringGLBatchSubmit(FPetraUtilsObject.GetForm());

                ParentForm.ShowInTaskbar = false;

                GLBatchTDS submitRecurringDS = (GLBatchTDS)FMainDS.Clone();
                int currentBatch = FPreviouslySelectedDetailRow.BatchNumber;

                submitRecurringDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatchAndRelatedTables(FLedgerNumber, FSelectedBatchNumber));

                SubmitForm.SubmitMainDS = submitRecurringDS;
                SubmitForm.ShowDialog();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (LoadDialogVisible)
                {
                    dlgStatus.Close();
                    LoadDialogVisible = false;
                }

                if (SubmitForm != null)
                {
                    SubmitForm.Dispose();
                    ParentForm.ShowInTaskbar = true;
                }

                MainForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
                Cursor = Cursors.Default;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then submit
                if (!((TFrmRecurringGLBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString(
                            "The recurring batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please fix the batch first and then submit it."),
                        Catalog.GetString("Submit Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private Boolean LoadAllBatchData()
        {
            bool RetVal = false;

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);

            bool NoJournalRows = true;
            bool NoTransRows = true;

            try
            {
                // now load journals/transactions for this batch, if necessary, so we know if exchange rate needs to be set in case of different currency
                JournalDV.RowFilter = String.Format("{0}={1}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber);

                TransDV.RowFilter = String.Format("{0}={1}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FSelectedBatchNumber);

                if (JournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndRelatedTablesForBatch(FLedgerNumber, FSelectedBatchNumber));
                }
                else if (TransDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionAndRelatedTablesForBatch(FLedgerNumber,
                            FSelectedBatchNumber));
                }

                NoJournalRows = (JournalDV.Count == 0);
                NoTransRows = (TransDV.Count == 0);

                if (NoJournalRows && NoTransRows)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString("The recurring gl batch {0} is empty. Do you still want to submit?"),
                                FPreviouslySelectedDetailRow.BatchNumber),
                            Catalog.GetString("Submit Empty Batch"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return RetVal;
                    }
                }
                else if (!NoJournalRows && NoTransRows)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString(
                                    "The recurring gl batch {0} contains empty journals. Do you still want to submit?"),
                                FPreviouslySelectedDetailRow.BatchNumber),
                            Catalog.GetString("Submit Empty Journals"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return RetVal;
                    }
                }
                else if (NoJournalRows && !NoTransRows)
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The recurring gl batch {0} contains orphaned transactions. PLEASE DELETE THE RECURRING BATCH AND RECREATE!"),
                            FPreviouslySelectedDetailRow.BatchNumber),
                        Catalog.GetString("Orphaned Data"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return RetVal;
                }

                RetVal = true;
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        private void SetAccountCostCentreTableVariables()
        {
            //Populate CostCentreList variable
            DataTable CostCentreListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref CostCentreListTable, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)CostCentreListTable;

            //Populate AccountList variable
            DataTable AccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref AccountListTable, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)AccountListTable;
        }

        private bool EnsureNewBatchIsVisible()
        {
            // Can we see the new row, bearing in mind we have filtering that the standard filter code does not know about?
            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
            Int32 RowNumberGrid =
                DataUtilities.GetDataViewIndexByDataTableIndex(dv, FMainDS.ARecurringBatch, FMainDS.ARecurringBatch.Rows.Count - 1) + 1;

            if (RowNumberGrid < 1)
            {
                MessageBox.Show(
                    Catalog.GetString(
                        "The new row has been added but the filter may be preventing it from being displayed. The filter will be reset."),
                    Catalog.GetString("New Batch"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                FFilterAndFindObject.FilterPanelControls.ClearAllDiscretionaryFilters();

                if (SelectDetailRowByDataTableIndex(FMainDS.ARecurringBatch.Rows.Count - 1))
                {
                    // Good - we found the row so now we need to do the other stuff to the new record
                    txtDetailBatchDescription.Text = MCommonResourcestrings.StrPleaseEnterDescription;
                    txtDetailBatchDescription.Focus();
                }
                else
                {
                    // This is not supposed to happen!!
                    MessageBox.Show(
                        Catalog.GetString(
                            "The filter was reset but unexpectedly the new batch is not in the list. Please close the screen and do not save changes."),
                        Catalog.GetString("New Batch"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            return true;
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
        }

        private bool PreDeleteManual(ARecurringBatchRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            if (FPreviouslySelectedDetailRow != null)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete recurring Batch {0}?"),
                    ARowToDelete.BatchNumber);
            }

            return allowDeletion;
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringBatchRow ARowToDelete, ref string ACompletionMessage)
        {
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return false;
            }

            //Notify of deletion process
            TFrmRecurringGLBatch MainForm = (TFrmRecurringGLBatch) this.ParentForm;

            int CurrentBatchNumber = ARowToDelete.BatchNumber;
            bool CurrentBatchJournalsLoadedAndCurrent = false;
            bool CurrentBatchTransactionsLoadedAndCurrent = false;

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = null;

            //Reject any changes which may fail validation
            ARowToDelete.RejectChanges();
            ShowDetails(ARowToDelete);

            try
            {
                this.Cursor = Cursors.WaitCursor;
                MainForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.DELETING;

                //Backup the changes to allow rollback
                BackupMainDS = (GLBatchTDS)FMainDS.GetChangesTyped(false);

                //Don't run an inactive fields check on this batch
                MainForm.GetBatchControl().UpdateRecurringBatchDictionary(CurrentBatchNumber);

                //Check if current batch journals and transactions are loaded and being viewed in their tab
                CurrentBatchJournalsLoadedAndCurrent = (MainForm.GetJournalsControl().FBatchNumber == CurrentBatchNumber);
                CurrentBatchTransactionsLoadedAndCurrent = (MainForm.GetTransactionsControl().FBatchNumber == CurrentBatchNumber);

                //Save and check for inactive values
                FPetraUtilsObject.SetChangedFlag();

                if (!MainForm.SaveChangesManual(MainForm.FCurrentGLBatchAction, !CurrentBatchJournalsLoadedAndCurrent,
                        !CurrentBatchTransactionsLoadedAndCurrent))
                {
                    MainForm.GetBatchControl().UpdateRecurringBatchDictionary();

                    string msg = String.Format(Catalog.GetString("Recurring Batch {0} has not been deleted."),
                        CurrentBatchNumber);

                    MessageBox.Show(msg,
                        Catalog.GetString("Recurring GL Batch Deletion"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return false;
                }

                //Remove any changes to current batch that may cause validation issues
                PrepareBatchDataForDeleting(CurrentBatchNumber, true);

                //Clear the journal and trans tabs if current batch data is displayed there
                if (CurrentBatchJournalsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    MainForm.GetJournalsControl().ClearCurrentSelection(CurrentBatchNumber);
                }

                if (CurrentBatchTransactionsLoadedAndCurrent)
                {
                    //Clear any transactions currently being edited in the Transaction Tab
                    MainForm.GetTransactionsControl().ClearCurrentSelection(CurrentBatchNumber);
                }

                //Delete transactions
                MainForm.GetTransactionsControl().DeleteRecurringTransactionData(CurrentBatchNumber);
                //Delete Journals
                MainForm.GetJournalsControl().DeleteRecurringJournalData(CurrentBatchNumber);

                // Delete the recurring batch row.
                ARowToDelete.Delete();

                ACompletionMessage = String.Format(Catalog.GetString("Recurring Batch no.: {0} deleted successfully."),
                    CurrentBatchNumber);

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                MainForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;

                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            UpdateRecordNumberDisplay();

            return DeletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ARecurringBatchRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                if (ADeletionPerformed && (ACompletionMessage.Length > 0))
                {
                    UpdateChangeableStatus();

                    if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
                    {
                        ClearControls();
                    }

                    FMyForm.SaveChangesManual();

                    //message to user
                    MessageBox.Show(ACompletionMessage,
                        "Deletion Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                ((TFrmRecurringGLBatch)ParentForm).EnableJournals((grdDetails.Rows.Count > 1));
                SetInitialFocus();
            }
            finally
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
            }
        }

        private void RevertDataSet(GLBatchTDS AMainDS, GLBatchTDS ABackupDS, int ASelectRowInGrid = 0)
        {
            if ((ABackupDS != null) && (AMainDS != null))
            {
                AMainDS.RejectChanges();
                AMainDS.Merge(ABackupDS);

                if (ASelectRowInGrid > 0)
                {
                    SelectRowInGrid(ASelectRowInGrid);
                }
            }
        }

        private void UpdateLedgerTableSettings()
        {
            int LedgerLastRecurringGLBatchNumber = 0;

            //Update the last recurring GL batch number
            DataView RecurringGLBatchDV = new DataView(FMainDS.ARecurringBatch);

            RecurringGLBatchDV.RowFilter = string.Empty;
            RecurringGLBatchDV.Sort = string.Format("{0} DESC",
                ARecurringBatchTable.GetBatchNumberDBName());

            //Recurring batch numbers can be reused so reset current highest number
            if (RecurringGLBatchDV.Count > 0)
            {
                LedgerLastRecurringGLBatchNumber = (int)(RecurringGLBatchDV[0][ARecurringBatchTable.GetBatchNumberDBName()]);
            }

            if (FMainDS.ALedger[0].LastRecurringBatchNumber != LedgerLastRecurringGLBatchNumber)
            {
                FMainDS.ALedger[0].LastRecurringBatchNumber = LedgerLastRecurringGLBatchNumber;
            }
        }
    }
}