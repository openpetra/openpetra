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
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;
using System.Diagnostics;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        private Int32 FLedgerNumber;
        private Int32 FSelectedBatchNumber;
        private TFinanceBatchFilterEnum FLoadedData = TFinanceBatchFilterEnum.fbfNone;
        private DateTime DefaultDate;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
            ((TFrmGLBatch)ParentForm).GetAttributesControl().LedgerNumber = ALedgerNumber;

            rbtEditing.Checked = true;

            // this will load the batches from the server
            SetBatchFilter();

            ShowData();

            //TODO: not necessary for posted batches
            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;
            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber, out StartDateCurrentPeriod, out EndDateLastForwardingPeriod, out DefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                StartDateCurrentPeriod.ToShortDateString(), EndDateLastForwardingPeriod.ToShortDateString());
        }

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private void ShowDetailsManual(ABatchRow ARow)
        {
            UpdateChangeableStatus();
            FPetraUtilsObject.DetailProtectedMode = (ARow.BatchStatus.Equals("Posted") || ARow.BatchStatus.Equals("Cancelled"));
            ((TFrmGLBatch)ParentForm).LoadJournals(
                ARow.LedgerNumber,
                ARow.BatchNumber);
            FSelectedBatchNumber = ARow.BatchNumber;
        }

        private void ShowJournalTab(Object sender, EventArgs e)
        {
            ((TFrmGLBatch)ParentForm).SelectTab(TFrmGLBatch.eGLTabs.Journals);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewABatch();

            dtpDetailDateEffective.Date = DefaultDate;

            // TODO: this.dtpDateCantBeBeyond.Value = AAccountingPeriod[ALedger.CurrentPeriod + ALedger.ForwardingPostingPeriods].EndOfPeriod

            // TODO: on change of FMainDS.ABatch[GetSelectedDetailDataTableIndex()].DateEffective
            // also change FMainDS.ABatch[GetSelectedDetailDataTableIndex()].BatchPeriod
            // and control this.dtpDateCantBeBeyond
        }

        /// <summary>
        /// cancel a batch (there is no deletion of batches)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                ||
                (MessageBox.Show(String.Format(Catalog.GetString("You have choosen to cancel this batch ({0}).\n\nDo you really want to cancel it?"),
                         FSelectedBatchNumber),
                     Catalog.GetString("Confirm Cancel"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                TVerificationResultCollection Verifications;
                GLBatchTDS mergeDS;
                //save the position of the actual row
                int rowIndex = CurrentRowIndex();

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
                    FPreviouslySelectedDetailRow.BatchStatus = MFinanceConstants.BATCH_CANCELLED;

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

                    MessageBox.Show(Catalog.GetString("The batch has been cancelled successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //((TFrmGLBatch)ParentForm).GetJournalsControl().ClearCurrentSelection();
                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                    FPetraUtilsObject.SetChangedFlag();

                    SelectByIndex(rowIndex);
                }

                UpdateChangeableStatus();
            }
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            // TODO: show VerificationResult
            // TODO: display progress of posting
            TVerificationResultCollection Verifications;

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmGLBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then post it!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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

                    // TODO: refresh the grid, to reflect that the batch has been posted
                    LoadBatches(FLedgerNumber);
                }
            }
        }

        private void ChangeBatchFilter(System.Object sender, System.EventArgs e)
        {
            int rowIndex = CurrentRowIndex();

            SetBatchFilter();
            // TODO Select the actual row again in updated
            SelectByIndex(rowIndex);
            UpdateChangeableStatus();
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

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void SetBatchFilter()
        {
            if (FMainDS == null)
            {
                return;
            }

            // load data from database, if it has not been loaded yet
            if (rbtAll.Checked && ((FLoadedData & TFinanceBatchFilterEnum.fbfAll) == 0))
            {
                // TODO: more criteria: period, etc
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfAll));
                FLoadedData = TFinanceBatchFilterEnum.fbfAll;
            }
            else if (rbtEditing.Checked && ((FLoadedData & TFinanceBatchFilterEnum.fbfEditing) == 0))
            {
                // TODO: more criteria: period, etc
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfEditing));
                FLoadedData |= TFinanceBatchFilterEnum.fbfEditing;
            }
            else if (rbtPosting.Checked && ((FLoadedData & TFinanceBatchFilterEnum.fbfReadyForPosting) == 0))
            {
                // TODO: more criteria: period, etc
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(FLedgerNumber, TFinanceBatchFilterEnum.fbfReadyForPosting));
                FLoadedData |= TFinanceBatchFilterEnum.fbfReadyForPosting;
            }

            if (rbtAll.Checked)
            {
                FMainDS.ABatch.DefaultView.RowFilter = "";
            }
            else if (rbtEditing.Checked)
            {
                FMainDS.ABatch.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
            }
            else if (rbtPosting.Checked)
            {
                FMainDS.ABatch.DefaultView.RowFilter = String.Format("({0} = '{1}') AND ({2} = {3}) AND ({2} <> 0) AND (({4} = 0) OR ({4} = {2} ))",
                    ABatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED,
                    ABatchTable.GetBatchCreditTotalDBName(),
                    ABatchTable.GetBatchDebitTotalDBName(),
                    ABatchTable.GetBatchControlTotalDBName());
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
                    AJournalRow NewJournal = FMainDS.AJournal.NewRowTyped(true);
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
                    NewTransaction.AmountInIntlCurrency = NewTransaction.TransactionAmount * TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].IntlCurrency,
                        NewTransaction.TransactionDate);
                    NewTransaction.AmountInBaseCurrency = NewTransaction.TransactionAmount * TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].BaseCurrency,
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

            // TODO: RefBatch.BatchRunningTotal
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null)
                                 && (FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            this.btnCancel.Enabled = changeable;
            this.btnPostBatch.Enabled = changeable;
            pnlDetails.Enabled = changeable;
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

            TFrmGLBatchExport gl = new TFrmGLBatchExport(this.Handle);
            gl.LedgerNumber = FLedgerNumber;
            gl.MainDS = FMainDS;
            gl.Show();
        }
    }
}