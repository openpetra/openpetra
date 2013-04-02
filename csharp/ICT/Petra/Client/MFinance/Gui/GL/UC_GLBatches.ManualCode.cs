//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
        private Int32 FSelectedPeriod;
        private DateTime FDefaultDate;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            rbtEditing.Checked = true;

            FPetraUtilsObject.DisableDataChangedEvent();
            TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbYearFilter, FLedgerNumber); //.InitialiseAvailableGiftYearsList(ref cmbYearFilter, FLedgerNumber);
            FPetraUtilsObject.EnableDataChangedEvent();

            // this will load the batches from the server
            RefreshFilter(null, null);

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGLBatch) this.ParentForm).EnableJournals();
            }
            else
            {
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
            }

            ((TFrmGLBatch) this.ParentForm).DisableTransactions();

            ShowData();

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
            
            grdDetails.Focus();
        }

        void RefreshPeriods(Object sender, EventArgs e)
        {
            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref cmbPeriodFilter, FLedgerNumber, cmbYearFilter.GetSelectedInt32());
            cmbPeriodFilter.SelectedIndex = 0;
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
        public GLBatchTDS BatchFMainDS()
        {
        	return FMainDS;
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
            Boolean postable = (FPreviouslySelectedDetailRow != null)
                               && FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED;

            FPetraUtilsObject.EnableAction("actPostBatch", postable);
            FPetraUtilsObject.EnableAction("actTestPostBatch", postable);
            FPetraUtilsObject.EnableAction("actCancel", postable);
            pnlDetails.Enabled = postable;
            pnlDetailsProtected = !postable;

            if (FPreviouslySelectedDetailRow == null)
            {
                // in the very first run ParentForm is null. Therefore
                // the exception handler has been included.
                try
                {
                    ((TFrmGLBatch) this.ParentForm).DisableJournals();
                }
                catch (Exception)
                {
                }
            }
        }

        private void ValidateDataDetailsManual(ABatchRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            ParseHashTotal(ARow);

            TSharedFinanceValidation_GL.ValidateGLBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ParseHashTotal(ABatchRow ARow)
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

        private void ShowDetailsManual(ABatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                 || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED));

            dtpDetailDateEffective.AllowVerification = !FPetraUtilsObject.DetailProtectedMode;

            FSelectedBatchNumber = ARow.BatchNumber;

            //Update the batch period if necessary
            UpdateBatchPeriod(null, null);

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
            Int32 yearNumber = 0;
            Int32 periodNumber = 0;

            if (!rbtEditing.Checked)
            {
                rbtEditing.Checked = true;
            }
            else if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                return;
            }

            //Set year and period to correct value
            if (cmbYearFilter.GetSelectedInt32() != 0)
            {
                cmbYearFilter.SelectedIndex = 0;
            }
            else if (cmbPeriodFilter.GetSelectedInt32() != 0)
            {
                cmbPeriodFilter.SelectedIndex = 0;
            }

            //FPreviouslySelectedDetailRow = null;

            pnlDetails.Enabled = true;

            //ClearDetailControls();

            EnableButtonControl(true);

            //grdDetails.DataSource = null;

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateABatch(FLedgerNumber));

            ABatchRow newBatchRow = (ABatchRow)FMainDS.ABatch.Rows[FMainDS.ABatch.Rows.Count - 1];

            newBatchRow.DateEffective = FDefaultDate;

            if (GetAccountingYearPeriodByDate(FLedgerNumber, FDefaultDate, out yearNumber, out periodNumber))
            {
                newBatchRow.BatchPeriod = periodNumber;
            }

            SelectDetailRowByDataTableIndex(FMainDS.ABatch.Rows.Count - 1);

            FPreviouslySelectedDetailRow.DateEffective = FDefaultDate;
            dtpDetailDateEffective.Date = FDefaultDate;

            FSelectedBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            FPreviouslySelectedDetailRow.BatchDescription = "Please enter a batch description";
            txtDetailBatchDescription.Text = "Please enter a batch description";
            txtDetailBatchDescription.Focus();

            ((TFrmGLBatch)ParentForm).SaveChanges();

            //Enable the Journals if not already enabled
            ((TFrmGLBatch)ParentForm).EnableJournals();
        }

        private void UpdateJournalTransEffectiveDate(bool ASetJournalDateOnly)
        {
            DateTime batchEffectiveDate = dtpDetailDateEffective.Date.Value;
            Int32 activeJournalNumber = 0;
            Int32 activeTransNumber = 0;
            Int32 activeTransJournalNumber = 0;

            bool activeJournalUpdated = false;
            bool activeTransUpdated = false;

            //Current Batch number
            Int32 batchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            FMainDS.AJournal.DefaultView.RowFilter = String.Format("{0}={1}",
                ATransactionTable.GetBatchNumberDBName(),
                batchNumber);

            if (FMainDS.AJournal.DefaultView.Count == 0)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(FLedgerNumber, batchNumber));
            }

            activeJournalNumber = ((TFrmGLBatch) this.ParentForm).GetJournalsControl().ActiveJournalNumber(FLedgerNumber, batchNumber);
            activeTransNumber = ((TFrmGLBatch) this.ParentForm).GetTransactionsControl().ActiveTransactionNumber(FLedgerNumber,
                batchNumber,
                ref activeTransJournalNumber);

            foreach (DataRowView v in FMainDS.AJournal.DefaultView)
            {
                AJournalRow r = (AJournalRow)v.Row;

                if (ASetJournalDateOnly)
                {
                    if ((activeJournalNumber > 0) && !activeJournalUpdated && (r.JournalNumber == activeJournalNumber))
                    {
                        ((TFrmGLBatch) this.ParentForm).GetJournalsControl().UpdateEffectiveDateForCurrentRow(batchEffectiveDate);
                        activeJournalUpdated = true;
                    }

                    r.BeginEdit();
                    r.DateEffective = batchEffectiveDate;
                    r.EndEdit();
                }
                else
                {
                    FMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0}={1} and {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        batchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        r.JournalNumber);

                    if (FMainDS.ATransaction.DefaultView.Count == 0)
                    {
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber, batchNumber, r.JournalNumber));
                    }

                    foreach (DataRowView w in FMainDS.ATransaction.DefaultView)
                    {
                        ATransactionRow t = (ATransactionRow)w.Row;

                        if ((activeTransNumber > 0) && !activeTransUpdated && (r.JournalNumber == activeTransJournalNumber)
                            && (t.TransactionNumber == activeTransNumber))
                        {
                            ((TFrmGLBatch) this.ParentForm).GetTransactionsControl().UpdateEffectiveDateForCurrentRow(batchEffectiveDate);
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

        private void UpdateBatchPeriod(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                Int32 periodNumber = 0;
                Int32 yearNumber = 0;
                DateTime dateValue;
                string aDate = dtpDetailDateEffective.Date.ToString();

                if (DateTime.TryParse(aDate, out dateValue))
                {
                    if (FPreviouslySelectedDetailRow.DateEffective != dateValue)
                    {
                        FPreviouslySelectedDetailRow.DateEffective = dateValue;
                        //Update the Transaction effective dates
                        UpdateJournalTransEffectiveDate(true);
                    }

                    if (GetAccountingYearPeriodByDate(FLedgerNumber, dateValue, out yearNumber, out periodNumber))
                    {
                        if (periodNumber != FPreviouslySelectedDetailRow.BatchPeriod)
                        {
                            FPreviouslySelectedDetailRow.BatchPeriod = periodNumber;

                            //Update the Transaction effective dates
                            UpdateJournalTransEffectiveDate(false);

                            if (cmbYearFilter.SelectedIndex != 0)
                            {
                                cmbYearFilter.SelectedIndex = 0;
                            }
                            else if (cmbPeriodFilter.SelectedIndex != 0)
                            {
                                cmbPeriodFilter.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            int newCurrentRowPos = grdDetails.SelectedRowIndex();

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                ||
                (MessageBox.Show(String.Format(Catalog.GetString("You have chosen to cancel this batch ({0}).\n\nDo you really want to cancel it?"),
                         FSelectedBatchNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                TVerificationResultCollection Verifications;
                GLBatchTDS mergeDS;
                //save the position of the actual row
                //int rowIndex = CurrentRowIndex();

                if (!TRemote.MFinance.GL.WebConnectors.GLBatchCanBeCancelled(out mergeDS, FLedgerNumber, FSelectedBatchNumber, out Verifications))
                {
                    string ErrorMessages = String.Empty;

                    foreach (TVerificationResult verif in Verifications)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Cancel batch failed"));
                    return;
                }
                else
                {
                    FMainDS.Merge(mergeDS);
                    //GetSelectedDetailRow().BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                    FPreviouslySelectedDetailRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                    //grdDetails.Refresh();

                    foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                    {
                        if (journal.BatchNumber == FSelectedBatchNumber)
                        {
                            journal.JournalStatus = MFinanceConstants.BATCH_CANCELLED;
                            journal.JournalCreditTotal = 0;
                            journal.JournalDebitTotal = 0;
                        }
                    }

                    FPreviouslySelectedDetailRow.BatchCreditTotal = 0;
                    FPreviouslySelectedDetailRow.BatchDebitTotal = 0;
                    FPreviouslySelectedDetailRow.BatchControlTotal = 0;
                    DataView transactionDV = new DataView(FMainDS.ATransaction, String.Format("{0} = {1}",
                            ATransactionTable.GetBatchNumberDBName(),
                            FSelectedBatchNumber), "", DataViewRowState.CurrentRows);

                    while (transactionDV.Count > 0)
                    {
                        transactionDV[0].Delete();
                    }

                    //Select and call the event that doesn't occur automatically
                    SelectRowInGrid(newCurrentRowPos);

                    //If some row(s) still exist after deletion
                    if (grdDetails.Rows.Count < 2)
                    {
                        EnableButtonControl(false);
                        ClearDetailControls();

                        ((TFrmGLBatch)ParentForm).DisableJournals();
                        ((TFrmGLBatch)ParentForm).DisableTransactions();
                    }

                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (((TFrmGLBatch)ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The batch has been cancelled successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //((TFrmGLBatch)ParentForm).GetJournalsControl() .ClearCurrentSelection();
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The batch has been cancelled but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save the cancellation immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
            if (FPetraUtilsObject.HasChanges)
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
            }

            return true;
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
            int newCurrentRowPos = grdDetails.SelectedRowIndex();

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
                    this.FPreviouslySelectedDetailRow = null;
                    ((TFrmGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();
                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                    LoadBatches(FLedgerNumber);

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
                        pnlDetails.Enabled = false;
                    }
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

            List <TVariant>Result = TRemote.MFinance.GL.WebConnectors.TestPostGLBatch(FLedgerNumber, FSelectedBatchNumber, out Verifications);

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

                    message +=
                        string.Format(
                            "{0},{1},{2},{3},{4},{5}",
                            ((TVariant)compValues[0]).ToString(),
                            ((TVariant)compValues[1]).ToString(),
                            ((TVariant)compValues[2]).ToString(),
                            ((TVariant)compValues[3]).ToString(),
                            StringHelper.FormatCurrency((TVariant)compValues[4], "currency"),
                            StringHelper.FormatCurrency((TVariant)compValues[5], "currency")) +
                        Environment.NewLine;
                }

                string CSVFilePath = TClientSettings.PathLog + Path.DirectorySeparatorChar + "Batch" + FSelectedBatchNumber.ToString() +
                                     "_TestPosting.csv";
                StreamWriter sw = new StreamWriter(CSVFilePath);
                sw.Write(message);
                sw.Close();

                MessageBox.Show(
                    String.Format(Catalog.GetString("Please see file {0} for the result of the test posting"), CSVFilePath),
                    Catalog.GetString("Result of Test Posting"));
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

        /// <summary>
        /// SelectByIndex
        /// </summary>
        /// <param name="rowIndex"></param>
        public void SelectByIndex(int rowIndex)
        {
            // TODO: Alan noted on upgrade that this method could be replaced by SelectRowInGrid(rowIndex, true)
            // This method actually never seems to be called
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                grdDetails.Selection.ResetSelection(false);
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void ToggleOptionButtonCheckedEvent(bool AToggleOn)
        {
            if (AToggleOn)
            {
                rbtEditing.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged += new System.EventHandler(this.RefreshFilter);
                rbtPosting.CheckedChanged += new System.EventHandler(this.RefreshFilter);
            }
            else
            {
                rbtEditing.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtAll.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
                rbtPosting.CheckedChanged -= new System.EventHandler(this.RefreshFilter);
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

            if (FPetraUtilsObject.HasChanges && !((TFrmGLBatch) this.ParentForm).SaveChanges())
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
                    else if ((FCurrentBatchViewOption == MFinanceConstants.GL_BATCH_VIEW_POSTING) && (rbtPosting.Checked == false))
                    {
                        ToggleOptionButtonCheckedEvent(false);
                        rbtPosting.Checked = true;
                        ToggleOptionButtonCheckedEvent(true);
                    }
                }
                else
                {
                    //Reset the combos
                    FPetraUtilsObject.DisableDataChangedEvent();
                    cmbYearFilter.SetSelectedInt32(FSelectedYear);
                    cmbPeriodFilter.SetSelectedInt32(FSelectedPeriod);
                    FPetraUtilsObject.EnableDataChangedEvent();
                }

                return;
            }

            ClearCurrentSelection();

            FSelectedYear = cmbYearFilter.GetSelectedInt32();
            FSelectedPeriod = cmbPeriodFilter.GetSelectedInt32();

            FPeriodFilter = String.Format(
                "{0} = {1} AND ",
                ABatchTable.GetBatchYearDBName(), FSelectedYear);

            if (FSelectedPeriod == 0)
            {
                ALedgerRow Ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                FPeriodFilter += String.Format(
                    "{0} >= {1}",
                    ABatchTable.GetBatchPeriodDBName(), Ledger.CurrentPeriod);
            }
            else
            {
                FPeriodFilter += String.Format(
                    "{0} = {1}",
                    ABatchTable.GetBatchPeriodDBName(), FSelectedPeriod);
            }

            if (rbtEditing.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_EDITING;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfEditing, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
                btnNew.Enabled = true;
            }
            else if (rbtPosting.Checked)
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
            else //(rbtAll.Checked)
            {
                FCurrentBatchViewOption = MFinanceConstants.GL_BATCH_VIEW_ALL;

                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll, FSelectedYear,
                        FSelectedPeriod));
                FStatusFilter = "1 = 1";
                btnNew.Enabled = true;
            }

            FPreviouslySelectedDetailRow = null;
            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

            FMainDS.ABatch.DefaultView.RowFilter =
                String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

            if (grdDetails.Rows.Count < 2)
            {
                ClearDetailControls();
                pnlDetails.Enabled = false;
                ((TFrmGLBatch) this.ParentForm).DisableJournals();
                ((TFrmGLBatch) this.ParentForm).DisableTransactions();
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
                ((TFrmGLBatch) this.ParentForm).EnableJournals();
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
                                MessageBox.Show(Catalog.GetString("Problem with date in row " + lineCounter.ToString() + " Fehler: " + exp.Message));
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
                    NewTransaction.AmountInBaseCurrency = NewTransaction.TransactionAmount * TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].BaseCurrency,
                        NewTransaction.TransactionDate);
                    //
                    // The International currency calculation is changed to "Base -> International", because it's likely
                    // we won't have a "Transaction -> International" conversion rate defined.
                    //
                    NewTransaction.AmountInIntlCurrency = NewTransaction.AmountInBaseCurrency * TExchangeRateCache.GetDailyExchangeRate(
                        FMainDS.ALedger[0].BaseCurrency,
                        FMainDS.ALedger[0].IntlCurrency,
                        NewTransaction.TransactionDate);
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

                BalancingTransaction.AmountInIntlCurrency = BalancingTransaction.TransactionAmount * TExchangeRateCache.GetDailyExchangeRate(
                    ARefJournalRow.TransactionCurrency,
                    FMainDS.ALedger[0].IntlCurrency,
                    BalancingTransaction.TransactionDate);
                BalancingTransaction.AmountInBaseCurrency = BalancingTransaction.TransactionAmount * TExchangeRateCache.GetDailyExchangeRate(
                    ARefJournalRow.TransactionCurrency,
                    FMainDS.ALedger[0].BaseCurrency,
                    BalancingTransaction.TransactionDate);
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