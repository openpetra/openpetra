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
using System.Globalization;
using Mono.Unix;
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
        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import batches from spreadsheet file");
            dialog.Filter = Catalog.GetString("GL Batches files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(false);
                dlgSeparator.CSVFileName = dialog.FileName;

                if (dlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    CultureInfo culture = new CultureInfo("en-GB");
                    culture.DateTimeFormat.ShortDatePattern = dlgSeparator.DateFormat;

                    StreamReader sr = new StreamReader(dialog.FileName);
                    ABatchRow NewBatch = null;
                    AJournalRow NewJournal = null;
                    string Message = Catalog.GetString("Parsing first line");
                    Int32 RowNumber = 0;

                    try
                    {
                        while (!sr.EndOfStream)
                        {
                            string Line = sr.ReadLine();
                            RowNumber++;

                            // skip empty lines and commented lines
                            if ((Line.Trim().Length > 0) && !Line.StartsWith("/*") && !Line.StartsWith("#"))
                            {
                                string RowType = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);

                                if (RowType == "B")
                                {
                                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.CreateABatch(FLedgerNumber));
                                    NewBatch = FMainDS.ABatch[FMainDS.ABatch.Count - 1];
                                    NewJournal = null;

                                    NewBatch.BatchDescription = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the hash value of the batch");
                                    NewBatch.BatchControlTotal = Convert.ToDouble(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator));
                                    Message = Catalog.GetString("Parsing the date effective of the batch");
                                    NewBatch.DateEffective = Convert.ToDateTime(StringHelper.GetNextCSV(ref Line,
                                            dlgSeparator.SelectedSeparator), culture);
                                }
                                else if (RowType == "J")
                                {
                                    if (NewBatch == null)
                                    {
                                        Message = Catalog.GetString("Expected a Batch line, but found a Journal");
                                        throw new Exception();
                                    }

                                    NewJournal = FMainDS.AJournal.NewRowTyped(true);
                                    ((TFrmGLBatch)ParentForm).GetJournalsControl().NewRowManual(ref NewJournal);
                                    FMainDS.AJournal.Rows.Add(NewJournal);

                                    NewJournal.JournalDescription = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the sub system code of the journal");
                                    NewJournal.SubSystemCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    // TODO test if SubSystemCode exists in cached table
                                    Message = Catalog.GetString("Parsing the transaction type of the journal");
                                    NewJournal.TransactionTypeCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    // TODO test if TransactionTypeCode exists in cached table
                                    Message = Catalog.GetString("Parsing the currency of the journal");
                                    NewJournal.TransactionCurrency = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    // TODO test if Currency exists in cached table
                                    Message = Catalog.GetString("Parsing the exchange rate of the journal");
                                    NewJournal.ExchangeRateToBase = Convert.ToDouble(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator));
                                    Message = Catalog.GetString("Parsing the date effective of the journal");
                                    NewJournal.DateEffective = Convert.ToDateTime(StringHelper.GetNextCSV(ref Line,
                                            dlgSeparator.SelectedSeparator), culture);
                                }
                                else if (RowType == "T")
                                {
                                    if (NewJournal == null)
                                    {
                                        Message = Catalog.GetString("Expected a Journal or Batch line, but found a Transaction");
                                        throw new Exception();
                                    }

                                    GLBatchTDSATransactionRow NewTransaction = FMainDS.ATransaction.NewRowTyped(true);
                                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().NewRowManual(ref NewTransaction, NewJournal);
                                    FMainDS.ATransaction.Rows.Add(NewTransaction);

                                    Message = Catalog.GetString("Parsing the cost centre of the transaction");
                                    NewTransaction.CostCentreCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    // TODO check if cost centre exists, and is a posting costcentre.
                                    // TODO check if cost centre is active. ask user if he wants to use an inactive cost centre
                                    Message = Catalog.GetString("Parsing the account code of the transaction");
                                    NewTransaction.AccountCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    // TODO check if account exists, and is a posting account.
                                    // TODO check if account is active. ask user if he wants to use an inactive account
                                    Message = Catalog.GetString("Parsing the narrative of the transaction");
                                    NewTransaction.Narrative = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the reference of the transaction");
                                    NewTransaction.Reference = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the transaction date");
                                    NewTransaction.TransactionDate =
                                        Convert.ToDateTime(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator), culture);

                                    Message = Catalog.GetString("Parsing the debit amount of the transaction");
                                    string DebitAmountString = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Double DebitAmount = DebitAmountString.Trim().Length == 0 ? 0.0 : Convert.ToDouble(DebitAmountString);
                                    Message = Catalog.GetString("Parsing the credit amount of the transaction");
                                    string CreditAmountString = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Double CreditAmount = DebitAmountString.Trim().Length == 0 ? 0.0 : Convert.ToDouble(CreditAmountString);

                                    if ((DebitAmount == 0) && (CreditAmount == 0))
                                    {
                                        Message = Catalog.GetString("Either the debit amount or the debit amount must be greater than 0.");
                                    }

                                    if ((DebitAmount != 0) && (CreditAmount != 0))
                                    {
                                        Message = Catalog.GetString("You can not have a value for both debit and credit amount");
                                    }

                                    if (DebitAmount != 0)
                                    {
                                        NewTransaction.DebitCreditIndicator = true;
                                        NewTransaction.TransactionAmount = DebitAmount;
                                        NewJournal.JournalDebitTotal += DebitAmount;
                                        NewBatch.BatchDebitTotal += DebitAmount;
                                        //NewBatch.BatchControlTotal += DebitAmount;
                                        NewBatch.BatchRunningTotal += DebitAmount;
                                    }
                                    else
                                    {
                                        NewTransaction.DebitCreditIndicator = false;
                                        NewTransaction.TransactionAmount = CreditAmount;
                                        NewJournal.JournalCreditTotal += CreditAmount;
                                        NewBatch.BatchCreditTotal += CreditAmount;
                                    }

                                    // TODO analysis attributes

                                    // TODO If this is a fund transfer to a foreign cost centre, check whether there are Key Ministries available for it.
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        sr.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            String.Format(Catalog.GetString("There is a problem parsing the file in row {0}. "), RowNumber) +
                            Environment.NewLine +
                            Message,
                            Catalog.GetString("Error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sr.Close();
                        return;
                    }
                }
            }
        }
    }
}