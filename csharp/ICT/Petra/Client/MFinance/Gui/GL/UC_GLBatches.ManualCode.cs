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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        private Int32 FLedgerNumber = -1;
        private Int32 FSelectedBatchNumber = -1;
        private string FStatusFilter = "1 = 1";
        private string FPeriodFilter = "1 = 1";

        private DateTime DefaultDate;
        private DateTime StartDateCurrentPeriod;
        private DateTime EndDateLastForwardingPeriod;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            rbtEditing.Checked = true;

            FPetraUtilsObject.DisableDataChangedEvent();
            TFinanceControls.InitialiseAvailableGiftYearsList(ref cmbYearFilter, FLedgerNumber);
            FPetraUtilsObject.EnableDataChangedEvent();

            // this will load the batches from the server
            RefreshFilter(null, null);

            ((TFrmGLBatch) this.ParentForm).DisableJournals();
            ((TFrmGLBatch) this.ParentForm).DisableTransactions();
            ((TFrmGLBatch) this.ParentForm).DisableAttributes();

            ShowData();

            //TODO: not necessary for posted batches
            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber,
                out StartDateCurrentPeriod,
                out EndDateLastForwardingPeriod,
                out DefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                StringHelper.DateToLocalizedString(StartDateCurrentPeriod, false, false),
                StringHelper.DateToLocalizedString(EndDateLastForwardingPeriod, false, false));
            //dtpDetailDateEffective.SetMaximalDate(EndDateLastForwardingPeriod);
            //dtpDetailDateEffective.SetMinimalDate(StartDateCurrentPeriod);
            //txtDetailBatchControlTotal.Enabled = false;
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
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                txtDetailBatchControlTotal.CurrencySymbol = ledger.BaseCurrency;
            }
            else
            {
                EnableButtonControl(false);
            }
        }

        private void UpdateChangeableStatus(bool batchRowIsSelected)
        {
            Boolean postable = batchRowIsSelected
                               && FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED;

            FPetraUtilsObject.EnableAction("actPostBatch", postable);
            FPetraUtilsObject.EnableAction("actTestPostBatch", postable);
            FPetraUtilsObject.EnableAction("actCancel", postable);
            pnlDetails.Enabled = postable;
            pnlDetailsProtected = !postable;

            if (!batchRowIsSelected)
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

        private void ValidateDataManual(ABatchRow ARow)
        {
        }

        private void ValidateDataDetailsManual(ABatchRow ARow)
        {
        }

        private void ShowDetailsManual(ABatchRow ARow)
        {
            UpdateChangeableStatus(ARow != null);

            if (ARow != null)
            {
                FPetraUtilsObject.DetailProtectedMode =
                    (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED)
                     || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED));

                dtpDetailDateEffective.AllowVerification = !FPetraUtilsObject.DetailProtectedMode;

                ((TFrmGLBatch)ParentForm).LoadJournals(
                    ARow.LedgerNumber,
                    ARow.BatchNumber);

                FSelectedBatchNumber = ARow.BatchNumber;
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
            txtDetailBatchDescription.Text = string.Empty;
            txtDetailBatchControlTotal.NumberValueDecimal = 0;
            dtpDetailDateEffective.Date = DateTime.Today;
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (!rbtEditing.Checked)
            {
                rbtEditing.Checked = true;
            }

            ClearDetailControls();
            EnableButtonControl(true);

            grdDetails.DataSource = null;

            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateABatch(FLedgerNumber));

            ((ABatchRow)FMainDS.ABatch.Rows[FMainDS.ABatch.Rows.Count - 1]).DateEffective = DefaultDate;

            FPetraUtilsObject.SetChangedFlag();

            // BoundDataView bdv = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);
            //bdv

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ABatch.DefaultView);

            //grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.ABatch.Rows.Count - 1);
            InvokeFocusedRowChanged(grdDetails.SelectedRowIndex());

            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            FSelectedBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;

            //FCurrentRow = FMainDS.ABatch.Rows.Count - 1;

            txtDetailBatchDescription.Text = "PLEASE ENTER DESCRIPTION";
            txtDetailBatchDescription.Focus();

            ((TFrmGLBatch)ParentForm).SaveChanges();
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

                if (!TRemote.MFinance.GL.WebConnectors.CancelGLBatch(out mergeDS, FLedgerNumber, FSelectedBatchNumber, out Verifications))
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
                    GetSelectedDetailRow().BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                    //FPreviouslySelectedDetailRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;
                    grdDetails.Refresh();

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

                    //If some row(s) still exist after deletion
                    if (grdDetails.Rows.Count > 1)
                    {
                        //If last row just deleted, select row at old position - 1
                        if (newCurrentRowPos == grdDetails.Rows.Count)
                        {
                            newCurrentRowPos--;
                        }
                    }
                    else
                    {
                        EnableButtonControl(false);
                        ClearDetailControls();

                        newCurrentRowPos = 0;
                    }

                    //Select and call the event that doesn't occur automatically
                    InvokeFocusedRowChanged(newCurrentRowPos);
                    //!!DO NOT USE THIS: grdDetails.Selection.FocusRow(0);


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
            txtDetailBatchControlTotal.NumberValueDecimal =
                FPreviouslySelectedDetailRow.BatchRunningTotal;
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
                            "The Date Effective is outside the allowable period range. Enter a date betweenn {0:d} and {1:d}."),
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
                    LoadBatches(FLedgerNumber);

                    if (grdDetails.Rows.Count > 1)
                    {
                        //If last row just deleted, select row at old position - 1
                        if (newCurrentRowPos == grdDetails.Rows.Count)
                        {
                            newCurrentRowPos--;
                        }

                        grdDetails.Selection.ResetSelection(false);
                        grdDetails.SelectRowInGrid(newCurrentRowPos);
                        FPreviouslySelectedDetailRow = GetSelectedDetailRow();

                        ShowDetails(FPreviouslySelectedDetailRow);
                    }
                    else
                    {
                        FPreviouslySelectedDetailRow = null;
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

        /// <summary>
        ///  The changes of the radio button are handled.
        /// </summary>
        /// <param name="sender">It should be a radio button</param>
        /// <param name="e"></param>
        private void ChangeBatchFilter(System.Object sender, System.EventArgs e)
        {
            // Each radio button click invokes this routine twice, on run is done for the
            // unchecked button an one is done for the checked one.
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                if (radioButton.Checked)
                {
                    int rowIndex = CurrentRowIndex();

                    RefreshFilter(null, null);
                    // TODO Select the actual row again in updated
                    SelectByIndex(rowIndex);
                    // UpdateChangeableStatus();

                    bool enablePosting = (radioButton.Text == "Editing" && grdDetails.Rows.Count > 1);
                    EnableButtonControl(enablePosting);
                }
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

//        / <summary>
//        / This routine is invoked if no Batch-Row has been selected. The idea was to
//        / select the "old row again" defined by row index but in this case
//        / the list of batches ist filtered. So the row must not exist any more.
//        / </summary>
//        / <param name="rowIndex">Index of a previosly selected row and -1 defines no row.</param>
//        private void SelectByIndex(int rowIndex)
//        {
//            // In the very first call FPetraUtilsObject does not exists
//            // Thererfore try-catch ..
//            try
//            {
//                FPetraUtilsObject.DisableDataChangedEvent();
//                txtDetailBatchControlTotal.Text = "";
//                txtDetailBatchDescription.Text = "";
//                dtpDetailDateEffective.Text = "";
//                UpdateChangeableStatus(false);
//                FPetraUtilsObject.EnableDataChangedEvent();
////                if (rowIndex >=   grdDetails.Rows)
////                {
////                    rowIndex = rowIndex--;
////                }
//                grdDetails.Selection.SelectRow(rowIndex,true);
//            }
//            catch (Exception)
//            {
//            }
//            ;
//        }

        void RefreshFilter(Object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            ClearCurrentSelection();

            Int32 SelectedYear = cmbYearFilter.GetSelectedInt32();
            Int32 SelectedPeriod = cmbPeriodFilter.GetSelectedInt32();

            FPeriodFilter = String.Format(
                "{0} = {1} AND ",
                ABatchTable.GetBatchYearDBName(), SelectedYear);

            if (SelectedPeriod == 0)
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
                    ABatchTable.GetBatchPeriodDBName(), SelectedPeriod);
            }

            if (rbtEditing.Checked)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfEditing, SelectedYear,
                        SelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
                btnNew.Enabled = true;
            }
            else if (rbtAll.Checked)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll, SelectedYear,
                        SelectedPeriod));
                FStatusFilter = "1 = 1";
                btnNew.Enabled = true;
            }
            else //(rbtPosting.Checked)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfReadyForPosting, SelectedYear,
                        SelectedPeriod));
                FStatusFilter = String.Format("({0} = '{1}') AND ({2} = {3}) AND ({2} <> 0) AND (({4} = 0) OR ({4} = {2} ))",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED,
                    ABatchTable.GetBatchCreditTotalDBName(),
                    ABatchTable.GetBatchDebitTotalDBName(),
                    ABatchTable.GetBatchControlTotalDBName());
                btnNew.Enabled = false;
            }

            FMainDS.ABatch.DefaultView.RowFilter =
                String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

            if (grdDetails.Rows.Count < 2)
            {
                ClearDetailControls();
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
    }
}