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
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.Interfaces.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLBatches
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FSelectedBatchNumber = -1;
        private string FStatusFilter = "1 = 1";
        private string FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_EDITING;
        private DateTime FDefaultDate = DateTime.Today;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            rbtEditing.Checked = true;

            FPetraUtilsObject.DisableDataChangedEvent();
            FPetraUtilsObject.EnableDataChangedEvent();

            // this will load the batches from the server
            RefreshFilter(null, null);

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableJournals();
            }
            else
            {
                ClearControls();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
            }

            ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
            ((TFrmRecurringGLBatch) this.ParentForm).DisableAttributes();

            ShowData();

            //Set sort order
            FMainDS.ARecurringBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ARecurringBatchTable.GetLedgerNumberDBName(),
                ARecurringBatchTable.GetBatchNumberDBName()
                );

            grdDetails.Focus();
        }


        /// reset the control
        public void ClearCurrentSelection()
        {
            GetDataFromControls();
            this.FPreviouslySelectedDetailRow = null;
            ShowData();
        }

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
            }
            else
            {
                EnableButtonControl(false);
            }
        }

        private void UpdateChangeableStatus()
        {
            Boolean allowSubmit = (FPreviouslySelectedDetailRow != null)
                                   && FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED;

            FPetraUtilsObject.EnableAction("actSubmitBatch", allowSubmit);
            FPetraUtilsObject.EnableAction("actDelete", allowSubmit);
            pnlDetails.Enabled = allowSubmit;
            pnlDetailsProtected = !allowSubmit;

            if (FPreviouslySelectedDetailRow == null)
            {
                // in the very first run ParentForm is null. Therefore
                // the exception handler has been included.
                try
                {
                    ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                }
                catch (Exception)
                {
                }
            }
        }

        private void ValidateDataDetailsManual(ARecurringBatchRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            ParseHashTotal(ARow);

            TSharedFinanceValidation_GL.ValidateRecurringGLBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ParseHashTotal(ARecurringBatchRow ARow)
        {
            decimal correctHashValue = 0;
            string hashTotal = txtDetailBatchControlTotal.Text.Trim();
            decimal hashDecimalVal;

            if (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            if ((hashTotal == null) || (hashTotal.Length == 0))
            {
                correctHashValue = 0m;
            }
            else
            {
                if (!Decimal.TryParse(hashTotal, out hashDecimalVal))
                {
                    correctHashValue = 0m;
                }
                else
                {
                    correctHashValue = hashDecimalVal;
                }
            }

            if (ARow.BatchControlTotal != correctHashValue)
            {
                ARow.BatchControlTotal = correctHashValue;
                txtDetailBatchControlTotal.NumberValueDecimal = correctHashValue;
            }
        }

        private void ShowDetailsManual(ARecurringBatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                 || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED));

            FSelectedBatchNumber = ARow.BatchNumber;

            UpdateChangeableStatus();
        }

        /// <summary>
        /// This routine is called by a double click on a batch row, which means: Open the
        /// Journal Tab of this batch.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowJournalTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGLBatch)ParentForm).SelectTab(TFrmRecurringGLBatch.eGLTabs.RecurringJournals);
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

        private void ClearDetailControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            txtDetailBatchDescription.Text = string.Empty;
            txtDetailBatchControlTotal.NumberValueDecimal = 0;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            //TODO Int32 yearNumber = 0;
            //TODO Int32 periodNumber = 0;

            if (!rbtEditing.Checked)
            {
                rbtEditing.Checked = true;
            }
            //TODO else if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            //TODO {
            //TODO     return;
            //TODO }

            //FPreviouslySelectedDetailRow = null;

            FPetraUtilsObject.VerificationResultCollection.Clear();
            
            pnlDetails.Enabled = true;

            //ClearDetailControls();

            EnableButtonControl(true);

            //grdDetails.DataSource = null;

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateARecurringBatch(FLedgerNumber));

            ARecurringBatchRow newBatchRow = (ARecurringBatchRow)FMainDS.ARecurringBatch.Rows[FMainDS.ARecurringBatch.Rows.Count - 1];

            newBatchRow.DateEffective = FDefaultDate;

            //TODO if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            //TODO {
            //TODO     newBatchRow.BatchPeriod = periodNumber;
            //TODO }

            SelectDetailRowByDataTableIndex(FMainDS.ARecurringBatch.Rows.Count - 1);

            FPreviouslySelectedDetailRow.DateEffective = FDefaultDate;

            FSelectedBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            FPreviouslySelectedDetailRow.BatchDescription = "Please enter a batch description";
            txtDetailBatchDescription.Text = "Please enter a batch description";
            txtDetailBatchDescription.Focus();

            //TODO ((TFrmRecurringGLBatch)ParentForm).SaveChanges();

            //Enable the Journals if not already enabled
            ((TFrmRecurringGLBatch)ParentForm).EnableJournals();
        }

        private void UpdateJournalTransEffectiveDate(bool ASetJournalDateOnly)
        {
            DateTime batchEffectiveDate = FDefaultDate;
            Int32 activeJournalNumber = 0;
            Int32 activeTransNumber = 0;
            Int32 activeTransJournalNumber = 0;

            bool activeJournalUpdated = false;
            bool activeTransUpdated = false;

            //Current Batch number
            Int32 batchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            FMainDS.ARecurringJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                ATransactionTable.GetBatchNumberDBName(),
                batchNumber);

            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(FLedgerNumber, batchNumber));
            }

            activeJournalNumber = ((TFrmRecurringGLBatch) this.ParentForm).GetJournalsControl().ActiveJournalNumber(FLedgerNumber, batchNumber);
            activeTransNumber = ((TFrmRecurringGLBatch) this.ParentForm).GetTransactionsControl().ActiveTransactionNumber(FLedgerNumber,
                batchNumber,
                ref activeTransJournalNumber);

            foreach (DataRowView v in FMainDS.ARecurringJournal.DefaultView)
            {
                ARecurringJournalRow r = (ARecurringJournalRow)v.Row;

                if (ASetJournalDateOnly)
                {
                    if ((activeJournalNumber > 0) && !activeJournalUpdated && (r.JournalNumber == activeJournalNumber))
                    {
                        ((TFrmRecurringGLBatch) this.ParentForm).GetJournalsControl().UpdateEffectiveDateForCurrentRow(batchEffectiveDate);
                        activeJournalUpdated = true;
                    }

                    r.BeginEdit();
                    r.DateEffective = batchEffectiveDate;
                    r.EndEdit();
                }
                else
                {
                    FMainDS.ARecurringTransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        batchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        r.JournalNumber);

                    if (FMainDS.ARecurringTransaction.DefaultView.Count == 0)
                    {
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionWithAttributes(FLedgerNumber, batchNumber, r.JournalNumber));
                    }

                    foreach (DataRowView w in FMainDS.ARecurringTransaction.DefaultView)
                    {
                        ARecurringTransactionRow t = (ARecurringTransactionRow)w.Row;

                        if ((activeTransNumber > 0) && !activeTransUpdated && (r.JournalNumber == activeTransJournalNumber)
                            && (t.TransactionNumber == activeTransNumber))
                        {
                            ((TFrmRecurringGLBatch) this.ParentForm).GetTransactionsControl().UpdateEffectiveDateForCurrentRow(batchEffectiveDate);
                            activeTransUpdated = true;
                        }

                        t.BeginEdit();
                        t.TransactionDate = batchEffectiveDate;
                        t.EndEdit();
                    }
                }
            }

            FPetraUtilsObject.HasChanges = true;
        }

        private bool GetAccountingsYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            return TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(ARecurringBatchRow ARowToDelete, ref string ADeletionQuestion)
        {
            if ((grdDetails.SelectedRowIndex() == -1) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("No Recurring GL Batch is selected to delete."),
                    Catalog.GetString("Deleting Recurring GL Batch"));
                return false;
            }
            else
            {
                // ask if the user really wants to cancel the batch
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete Recurring GL Batch no: {0} ?"),
                    ARowToDelete.BatchNumber);
                return true;
            }
        }

        private void DeleteRecord(System.Object sender, EventArgs e)
        {
            this.DeleteARecurringBatch();
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringBatchRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            int batchNumber = ARowToDelete.BatchNumber;

            try
            {
                // Delete on client side data through views that is already loaded. Data that is not 
                // loaded yet will be deleted with cascading delete on server side so we don't have
                // to worry about this here.

                ACompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} deleted successfully."),
                    batchNumber);

                // Delete the associated recurring transaction analysis attributes
                DataView viewRecurringTransAnalAttrib = new DataView(FMainDS.ARecurringTransAnalAttrib);
                viewRecurringTransAnalAttrib.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransAnalAttribTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    batchNumber);

                foreach (DataRowView row in viewRecurringTransAnalAttrib)
                {
                    row.Delete();
                }

                // Delete the associated recurring transactions
                DataView viewRecurringTransaction = new DataView(FMainDS.ARecurringTransaction);
                viewRecurringTransaction.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringTransactionTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    batchNumber);

                foreach (DataRowView row in viewRecurringTransaction)
                {
                    row.Delete();
                }

                // Delete the associated recurring journals
                DataView viewRecurringJournal = new DataView(FMainDS.ARecurringJournal);
                viewRecurringJournal.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringJournalTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    batchNumber);

                foreach (DataRowView row in viewRecurringJournal)
                {
                    row.Delete();
                }
                
                // Delete the recurring batch row.
                ARowToDelete.Delete();

                FPreviouslySelectedDetailRow = null;

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
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
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                //MessageBox.Show(ACompletionMessage,
                //    "Deletion completed",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Information);

                if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
                {
                    ClearControls();
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else if (!ADeletionPerformed)
            {
                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch)ParentForm).EnableJournals();
            }
            else
            {
                ((TFrmRecurringGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();
                ((TFrmRecurringGLBatch)ParentForm).DisableJournals();
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
            //txtDetailBatchControlTotal.NumberValueDecimal = FPreviouslySelectedDetailRow.BatchControlTotal;
        }

        private bool SaveBatchForSubmitting()
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmRecurringGLBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The recurring batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then submit it!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void SubmitBatch(System.Object sender, EventArgs e)
        {
            // TODO: display progress of posting
            //TVerificationResultCollection Verifications;

            if (!SaveBatchForSubmitting())
            {
                return;
            }
            
            //TODO
            
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

        private void ToggleOptionButtonCheckedEvent(bool AToggleOn)
        {
            if (AToggleOn)
            {
                rbtEditing.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtSubmitting.CheckedChanged += new System.EventHandler(this.RefreshFilter);
            }
            else
            {
                rbtEditing.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtSubmitting.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
            }
        }

        void RefreshFilter(Object sender, EventArgs e)
        {
            int batchNumber = 0;
            int newRowToSelectAfterFilter = 1;
            bool senderIsRadioButton = (sender is RadioButton);

            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }
            else if ((sender != null) && senderIsRadioButton)
            {
                //Avoid repeat events
                RadioButton rbt = (RadioButton)sender;

                if (rbt.Name.Contains(FCurrentBatchViewOption))
                {
                    return;
                }
            }

            //Record the current batch
            if (FPreviouslySelectedDetailRow != null)
            {
                batchNumber = FPreviouslySelectedDetailRow.BatchNumber;

                if (FPreviouslySelectedDetailRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
                {
                    FPetraUtilsObject.DisableSaveButton();
                }
            }

            if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                if (senderIsRadioButton)
                {
                    //Need to cancel the change of option button
                    if ((FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_EDITING) && (rbtEditing.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtEditing.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                    else if ((FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_ALL) && (rbtAll.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtAll.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                    else if ((FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_POSTING) && (rbtSubmitting.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtSubmitting.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                }

                return;
            }

            ClearCurrentSelection();

            if (rbtEditing.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_EDITING;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfEditing));
                FStatusFilter = String.Format("{0} = '{1}'",
                    ARecurringBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
                btnNew.Enabled = true;
            }
            else if (rbtSubmitting.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_POSTING;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfReadyForPosting));
                FStatusFilter = String.Format("({0} = '{1}') AND ({2} = {3}) AND ({2} <> 0) AND (({4} = 0) OR ({4} = {2}))",
                    ARecurringBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED,
                    ARecurringBatchTable.GetBatchCreditTotalDBName(),
                    ARecurringBatchTable.GetBatchDebitTotalDBName(),
                    ARecurringBatchTable.GetBatchControlTotalDBName());
            }
            else //(rbtAll.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_ALL;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll));
                FStatusFilter = "1 = 1";
                btnNew.Enabled = true;
            }

            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringBatch.DefaultView);

            FMainDS.ARecurringBatch.DefaultView.RowFilter =
                String.Format("({0})", FStatusFilter);

            if (grdDetails.Rows.Count < 2)
            {
                ClearDetailControls();
                pnlDetails.Enabled = false;
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableAttributes();
            }
            else
            {
                //Select same row after refilter
                if (batchNumber > 0)
                {
                    newRowToSelectAfterFilter = GetDataTableRowIndexByPrimaryKeys(FLedgerNumber, batchNumber);
                }

                SelectRowInGrid(newRowToSelectAfterFilter);

                UpdateChangeableStatus();
                ((TFrmRecurringGLBatch) this.ParentForm).EnableJournals();
            }
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
        /// Set focus to the gid controltab
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }
    }
}