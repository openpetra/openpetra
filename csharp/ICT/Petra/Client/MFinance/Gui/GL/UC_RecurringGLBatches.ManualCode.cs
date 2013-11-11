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
using Ict.Common.Data;
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
        private DateTime FDefaultDate = DateTime.Today;
        private GLSetupTDS FCacheDS;
        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;


        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            FPetraUtilsObject.DisableDataChangedEvent();
            FPetraUtilsObject.EnableDataChangedEvent();

            // this will load the batches from the server
            RefreshFilter(null, null);

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableJournals();
                EnableTransactionTabForBatch();
            }
            else
            {
                ClearControls();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
            }

            //Load all analysis attribute values
            if (FCacheDS == null)
            {
                FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber);
            }

            ShowData();

            //Set sort order
            FMainDS.ARecurringBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                ARecurringBatchTable.GetLedgerNumberDBName(),
                ARecurringBatchTable.GetBatchNumberDBName()
                );

            UpdateRecordNumberDisplay();

            grdDetails.Focus();

            SelectRowInGrid(1);

            SetAccountCostCentreTableVariables();
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            GetDataFromControls();
            this.FPreviouslySelectedDetailRow = null;
            ShowData();
        }

        /// <summary>
        /// Returns FMainDS
        /// </summary>
        /// <returns></returns>
        public GLBatchTDS RecurringBatchFMainDS()
        {
            return FMainDS;
        }

        /// <summary>
        /// Enable the transaction tab
        /// </summary>
        public void EnableTransactionTabForBatch()
        {
            bool enable = false;

            //If a single journal exists and it is not status=Cancelled then enable transactions tab
            if ((FPreviouslySelectedDetailRow != null) && (FPreviouslySelectedDetailRow.LastJournal == 1))
            {
                LoadJournalsForCurrentBatch();

                ARecurringJournalRow rJ = (ARecurringJournalRow)FMainDS.ARecurringJournal.DefaultView[0].Row;

                enable = (rJ.JournalStatus != MFinanceConstants.BATCH_CANCELLED);
            }

            if (enable)
            {
                ((TFrmRecurringGLBatch) this.ParentForm).EnableTransactions();
            }
            else
            {
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
            }
        }

        private void LoadJournalsForCurrentBatch()
        {
            //Current Batch number
            Int32 batchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            FMainDS.ARecurringJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                batchNumber);

            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(FLedgerNumber, batchNumber));
            }
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
                                  && (FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED)
                                  && (grdDetails.Rows.Count > 1);

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
            decimal correctHashValue = 0m;

            if (ARow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                return;
            }

            if ((txtDetailBatchControlTotal.NumberValueDecimal == null) || !txtDetailBatchControlTotal.NumberValueDecimal.HasValue)
            {
                correctHashValue = 0m;
            }
            else
            {
                correctHashValue = txtDetailBatchControlTotal.NumberValueDecimal.Value;
            }

            txtDetailBatchControlTotal.NumberValueDecimal = correctHashValue;
            ARow.BatchControlTotal = correctHashValue;
        }

        private void ShowDetailsManual(ARecurringBatchRow ARow)
        {
            EnableTransactionTabForBatch();

            if (ARow == null)
            {
                return;
            }

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                 || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED));

            FSelectedBatchNumber = ARow.BatchNumber;

            UpdateChangeableStatus();

            if (FPetraUtilsObject.HasChanges)
            {
                //May need this
                //((TFrmRecurringGLBatch) this.ParentForm).SaveChanges();
            }
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

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            Int32 yearNumber = 0;
            Int32 periodNumber = 0;

            if (FPetraUtilsObject.HasChanges && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            FPetraUtilsObject.VerificationResultCollection.Clear();

            pnlDetails.Enabled = true;

            EnableButtonControl(true);

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateARecurringBatch(FLedgerNumber));

            ARecurringBatchRow newBatchRow = (ARecurringBatchRow)FMainDS.ARecurringBatch.Rows[FMainDS.ARecurringBatch.Rows.Count - 1];

            newBatchRow.DateEffective = FDefaultDate;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            {
                newBatchRow.BatchPeriod = periodNumber;
            }

            SelectDetailRowByDataTableIndex(FMainDS.ARecurringBatch.Rows.Count - 1);

            FPreviouslySelectedDetailRow.DateEffective = FDefaultDate;


            FSelectedBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            string enterMsg = Catalog.GetString("Please enter a batch description");
            FPreviouslySelectedDetailRow.BatchDescription = enterMsg;
            txtDetailBatchDescription.Text = enterMsg;
            txtDetailBatchDescription.Focus();

            UpdateRecordNumberDisplay();

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
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber, batchNumber, r.JournalNumber));
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

        private bool GetAccountingYearPeriodByDate(Int32 ALedgerNumber, DateTime ADate, out Int32 AYear, out Int32 APeriod)
        {
            return TRemote.MFinance.GL.WebConnectors.GetAccountingYearPeriodByDate(ALedgerNumber, ADate, out AYear, out APeriod);
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
            int batchNumber = ARowToDelete.BatchNumber;

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

            UpdateRecordNumberDisplay();

            return true;
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
                MessageBox.Show(ACompletionMessage, Catalog.GetString("Deletion Completed"));
            }

            UpdateChangeableStatus();

            if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
            {
                ClearControls();
            }

            UpdateBatchHeaderTotals();

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
        public void UpdateBatchHeaderTotals()
        {
            //Below not needed as yet
            if (FPreviouslySelectedDetailRow != null)
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                GLRoutines.UpdateTotalsOfRecurringBatch(ref FMainDS, FPreviouslySelectedDetailRow);
                txtDetailBatchControlTotal.NumberValueDecimal = FPreviouslySelectedDetailRow.BatchControlTotal;
                FPetraUtilsObject.EnableDataChangedEvent();
            }
        }

        private void SubmitBatch(System.Object sender, EventArgs e)
        {
            Boolean SubmitCancelled = false;
            Int32 NumberOfNonBaseCurrencyJournals = 0;
            DateTime DateEffective = DateTime.Today;
            Decimal ExchangeRateToBase;

            if (FPetraUtilsObject.HasChanges)
            {
                // ask user if he wants to save as otherwise process cannot continue
                if (MessageBox.Show(Catalog.GetString("Changes need to be saved in order to submit a batch!") + Environment.NewLine +
                        Catalog.GetString("Do you want to save and continue submitting?"),
                        Catalog.GetString("Changes not saved"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                // save first, then submit
                if (!((TFrmRecurringGLBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString(
                            "The recurring batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then submit it!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (FPreviouslySelectedDetailRow == null)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please select a Batch before submitting."));
                return;
            }

            // now load journals/transactions for this batch, if necessary, so we know if exchange rate needs to be set in case of different currency
            FMainDS.ARecurringJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                FSelectedBatchNumber);
            FMainDS.ARecurringTransaction.DefaultView.RowFilter = String.Format("{0}={1}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                FSelectedBatchNumber);
            FMainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                FSelectedBatchNumber);

            if (FMainDS.ARecurringJournal.DefaultView.Count == 0)
            {
                //Make sure all data is loaded for batch
                //clear any journals from other batches
                FMainDS.ARecurringTransAnalAttrib.Clear();
                FMainDS.ARecurringTransaction.Clear();
                FMainDS.ARecurringJournal.Clear();
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndContent(FLedgerNumber, FSelectedBatchNumber));
            }
            else if (FMainDS.ARecurringTransaction.DefaultView.Count == 0)
            {
                FMainDS.ARecurringTransAnalAttrib.Clear();
                FMainDS.ARecurringTransaction.Clear();
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatchAndContent(FLedgerNumber, FSelectedBatchNumber));
            }

            //Reset row filter
            FMainDS.ARecurringJournal.DefaultView.RowFilter = string.Empty;
            FMainDS.ARecurringTransaction.DefaultView.RowFilter = string.Empty;
            FMainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = string.Empty;

            bool inactiveCodefound = false;

            // check how many journals have currency different from base currency
            // check for inactive accounts or cost centres

            foreach (ARecurringJournalRow JournalRow in FMainDS.ARecurringJournal.Rows)
            {
                if ((JournalRow.BatchNumber == FSelectedBatchNumber)
                    && (JournalRow.TransactionCurrency != ((ALedgerRow)FMainDS.ALedger.Rows[0]).BaseCurrency))
                {
                    NumberOfNonBaseCurrencyJournals++;
                }
            }

            foreach (ARecurringTransactionRow transRow in FMainDS.ARecurringTransaction.Rows)
            {
                if (!AccountIsActive(transRow.AccountCode) || !CostCentreIsActive(transRow.CostCentreCode))
                {
                    inactiveCodefound = true;

                    MessageBox.Show(String.Format(Catalog.GetString(
                                "Recurring batch no. {0} cannot be submitted because transaction {1} in Journal {2} contains an inactive account or cost centre code"),
                            FSelectedBatchNumber,
                            transRow.JournalNumber,
                            transRow.TransactionNumber),
                        Catalog.GetString("Inactive Account/Cost Centre Code"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
                }
            }

            foreach (ARecurringTransAnalAttribRow analAttribRow in FMainDS.ARecurringTransAnalAttrib.Rows)
            {
                if (!AnalysisCodeIsActive(analAttribRow.AccountCode,
                        analAttribRow.AnalysisTypeCode)
                    || !AnalysisAttributeValueIsActive(analAttribRow.AnalysisTypeCode, analAttribRow.AnalysisAttributeValue))
                {
                    inactiveCodefound = true;

                    MessageBox.Show(String.Format(Catalog.GetString(
                                "Recurring batch no. {0} cannot be submitted because transaction {1} in Journal {2} contains an inactive analysis code|value"),
                            FSelectedBatchNumber,
                            analAttribRow.JournalNumber,
                            analAttribRow.TransactionNumber),
                        Catalog.GetString("Inactive Account/Cost Centre Code"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
                }
            }

            if (inactiveCodefound)
            {
                return;
            }

            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("ABatchNumber", FSelectedBatchNumber);

            TFrmRecurringGLBatchSubmit submitForm = new TFrmRecurringGLBatchSubmit(FPetraUtilsObject.GetForm());
            try
            {
                ParentForm.ShowInTaskbar = false;
                submitForm.MainDS = FMainDS;
                submitForm.BatchRow = FPreviouslySelectedDetailRow;

                if (NumberOfNonBaseCurrencyJournals == 0)
                {
                    submitForm.ShowDialog();

                    if (submitForm.GetResult(out DateEffective))
                    {
                        requestParams.Add("AEffectiveDate", DateEffective);
                    }
                    else
                    {
                        SubmitCancelled = true;
                    }

                    // set exchange rate to base to 1 as default if no journals with other currencies exist
                    foreach (ARecurringJournalRow JournalRow in FMainDS.ARecurringJournal.Rows)
                    {
                        requestParams.Add("AExchangeRateToBaseForJournal" + JournalRow.JournalNumber.ToString(), 1.0);
                    }
                }
                else
                {
                    // make sure dialogs for journal rows are displayed in sequential order -> new to use view
                    DataView JournalView = new DataView(FMainDS.ARecurringJournal);
                    JournalView.Sort = ARecurringJournalTable.GetJournalNumberDBName();
                    Boolean FirstJournal = true;

                    foreach (DataRowView rowView in JournalView)
                    {
                        ARecurringJournalRow JournalRow = (ARecurringJournalRow)rowView.Row;

                        if (JournalRow.TransactionCurrency != ((ALedgerRow)FMainDS.ALedger.Rows[0]).BaseCurrency)
                        {
                            submitForm.JournalRow = JournalRow;

                            if (!FirstJournal)
                            {
                                submitForm.SetDateEffectiveReadOnly();
                            }

                            submitForm.ShowDialog();

                            if (submitForm.GetResult(out DateEffective, out ExchangeRateToBase))
                            {
                                requestParams.Add("AExchangeRateToBaseForJournal" + JournalRow.JournalNumber.ToString(), ExchangeRateToBase);
                            }
                            else
                            {
                                SubmitCancelled = true;
                                break;
                            }

                            FirstJournal = false;
                        }
                        else
                        {
                            requestParams.Add("AExchangeRateToBaseForJournal" + JournalRow.JournalNumber.ToString(), 1);
                        }
                    }

                    requestParams.Add("AEffectiveDate", DateEffective);
                }
            }
            finally
            {
                submitForm.Dispose();
                ParentForm.ShowInTaskbar = true;
            }

            if (!SubmitCancelled)
            {
                TVerificationResultCollection AMessages;

                Boolean submitOK = TRemote.MFinance.GL.WebConnectors.SubmitRecurringGLBatch(requestParams, out AMessages);

                if (submitOK)
                {
                    MessageBox.Show(Catalog.GetString("Your recurring batch was submitted successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(Catalog.GetString("Submitting the batch failed!") +
                            Environment.NewLine +
                            Catalog.GetString("Reasons:"), AMessages));
                }
            }
        }

        private void SetAccountCostCentreTableVariables()
        {
            //Populate CostCentreList variable
            DataTable costCentreList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref costCentreList, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)costCentreList;

            //Populate AccountList variable
            DataTable accountList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref accountList, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)accountList;
        }

        private bool AnalysisCodeIsActive(String AAccountCode, String AAnalysisCode = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAccountCode == string.Empty))
            {
                return retVal;
            }

            string originalRowFilter = FCacheDS.AAnalysisAttribute.DefaultView.RowFilter;
            FCacheDS.AAnalysisAttribute.DefaultView.RowFilter = string.Empty;

            FCacheDS.AAnalysisAttribute.DefaultView.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}=true",
                AAnalysisAttributeTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                AAccountCode,
                AAnalysisAttributeTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AAnalysisAttributeTable.GetActiveDBName());

            retVal = (FCacheDS.AAnalysisAttribute.DefaultView.Count > 0);

            FCacheDS.AAnalysisAttribute.DefaultView.RowFilter = originalRowFilter;

            return retVal;
        }

        private bool AnalysisAttributeValueIsActive(String AAnalysisCode = "", String AAnalysisAttributeValue = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAnalysisAttributeValue == string.Empty))
            {
                return retVal;
            }

            string originalRowFilter = FCacheDS.AFreeformAnalysis.DefaultView.RowFilter;
            FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = string.Empty;

            FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AFreeformAnalysisTable.GetAnalysisValueDBName(),
                AAnalysisAttributeValue,
                AFreeformAnalysisTable.GetActiveDBName());

            retVal = (FCacheDS.AFreeformAnalysis.DefaultView.Count > 0);

            FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = originalRowFilter;

            return retVal;
        }

        private bool AccountIsActive(string AAccountCode)
        {
            bool retVal = true;

            AAccountRow currentAccountRow = null;

            if (FAccountTable != null)
            {
                currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });
            }

            if (currentAccountRow != null)
            {
                retVal = currentAccountRow.AccountActiveFlag;
            }

            return retVal;
        }

        private bool CostCentreIsActive(string ACostCentreCode)
        {
            bool retVal = true;

            ACostCentreRow currentCostCentreRow = null;

            if (FCostCentreTable != null)
            {
                currentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });
            }

            if (currentCostCentreRow != null)
            {
                retVal = currentCostCentreRow.CostCentreActiveFlag;
            }

            return retVal;
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

        void RefreshFilter(Object sender, EventArgs e)
        {
            int newRowToSelectAfterFilter = 1;

            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringBatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll));
            btnNew.Enabled = true;

            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringBatch.DefaultView);

            UpdateChangeableStatus();

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableJournals();
                ((TFrmRecurringGLBatch) this.ParentForm).DisableTransactions();
            }
            else
            {
                SelectRowInGrid(newRowToSelectAfterFilter);

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