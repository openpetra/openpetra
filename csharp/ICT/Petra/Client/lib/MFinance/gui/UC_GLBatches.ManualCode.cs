/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Mono.Unix;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        private Int32 FLedgerNumber;
        private Int32 FSelectedBatchNumber;
        private DateTime DefaultDate;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            // TODO: more criteria: state of batch, period, etc
            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatch(ALedgerNumber));

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

            dtpDetailDateEffective.Value = DefaultDate;

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
            // TODO
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
                        Catalog.GetString("Please first save the batch, and then post it!"));
                    return;
                }
            }

            if (!TRemote.MFinance.GL.WebConnectors.PostGLBatch(FLedgerNumber, FSelectedBatchNumber, out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"));
            }
            else
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"));

                // TODO: refresh the grid, to reflect that the batch has been posted
                LoadBatches(FLedgerNumber);
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

            double sumDebits = 0.0;
            double sumCredits = 0.0;
            DateTime LatestTransactionDate = DateTime.MinValue;

            do
            {
                string line = dataFile.ReadLine();
                lineCounter++;

                ATransactionRow NewTransaction = FMainDS.ATransaction.NewRowTyped(true);
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

                            NewTransaction.TransactionAmount = Convert.ToDouble(Value, System.Globalization.CultureInfo.InvariantCulture);
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
                                NewTransaction.TransactionAmount *= -1.0;
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
                ATransactionRow BalancingTransaction = FMainDS.ATransaction.NewRowTyped(true);
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

            dtpDetailDateEffective.Value = LatestTransactionDate;
            txtDetailBatchDescription.Text = Path.GetFileNameWithoutExtension(ADataFilename);
            ABatchRow RefBatch = (ABatchRow)FMainDS.ABatch.Rows[FMainDS.ABatch.Rows.Count - 1];
            RefBatch.BatchCreditTotal = sumCredits;
            RefBatch.BatchDebitTotal = sumDebits;

            // TODO: RefBatch.BatchRunningTotal
        }
    }
}