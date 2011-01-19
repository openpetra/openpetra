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
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLBatches
    {
        private String FImportMessage;
        private String FImportLine;
        private TDlgSelectCSVSeparator FdlgSeparator;
        private CultureInfo FCultureInfoNumber;
        private CultureInfo FCultureInfoDate;
        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches()
        {
            FCultureInfoDate = new CultureInfo("en-GB");
            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            GLSetupTDS FCacheDS = ((TFrmGLBatch)ParentForm).GetAttributesControl().CacheDS;
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.FileName = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "import.csv");

            dialog.Title = Catalog.GetString("Import batches from spreadsheet file");
            dialog.Filter = Catalog.GetString("GL Batches files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                FdlgSeparator.CSVFileName = dialog.FileName;

                if (dateFormatString.Equals("MDY"))
                {
                    FdlgSeparator.DateFormat = "MM/dd/yyyy";
                }
                else
                {
                    if (dateFormatString.Equals("DMY"))
                    {
                        FdlgSeparator.DateFormat = "dd/MM/yyyy";
                    }
                    else
                    {
                        FdlgSeparator.DateFormat = dateFormatString;
                    }
                }

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormatIndex = impOptions.Substring(1) == "American" ? 0 : 1;
                }

                FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    FCultureInfoDate.DateTimeFormat.ShortDatePattern = FdlgSeparator.DateFormat;
                    FCultureInfoNumber = new CultureInfo(FdlgSeparator.NumberFormatIndex == 0 ? "en-US" : "de-DE");

                    StreamReader sr = new StreamReader(dialog.FileName);

                    ABatchRow NewBatch = null;
                    AJournalRow NewJournal = null;
                    FImportMessage = Catalog.GetString("Parsing first line");
                    Int32 RowNumber = 0;

                    try
                    {
                        while (!sr.EndOfStream)
                        {
                            FImportLine = sr.ReadLine();
                            RowNumber++;

                            // skip empty lines and commented lines
                            if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#")
                                && !FImportLine.StartsWith(","))
                            {
                                string RowType = ImportString("RowType");

                                if (RowType == "B")
                                {
                                    GLBatchTDS NewBatchDS = TRemote.MFinance.GL.WebConnectors.CreateABatch(FLedgerNumber);
                                    Int32 NewBatchNumber = NewBatchDS.ABatch[0].BatchNumber;
                                    FMainDS.Merge(NewBatchDS);

                                    DataView FindView = new DataView(FMainDS.ABatch);
                                    FindView.Sort = ABatchTable.GetLedgerNumberDBName() + "," + ABatchTable.GetBatchNumberDBName();
                                    NewBatch = (ABatchRow)FindView[FindView.Find(new object[] { FLedgerNumber, NewBatchNumber })].Row;
                                    NewJournal = null;

                                    FPetraUtilsObject.SetChangedFlag();

                                    NewBatch.BatchDescription = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    FImportMessage = Catalog.GetString("Parsing the hash value of the batch");
                                    NewBatch.BatchControlTotal =
                                        Convert.ToDecimal(StringHelper.GetNextCSV(ref FImportLine,
                                                FdlgSeparator.SelectedSeparator), FCultureInfoNumber);
                                    string NextString = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    FImportMessage = Catalog.GetString("Parsing the date effective of the batch: " + NextString);
                                    NewBatch.DateEffective = Convert.ToDateTime(NextString, FCultureInfoDate);
                                }
                                else if (RowType == "J")
                                {
                                    if (NewBatch == null)
                                    {
                                        FImportMessage = Catalog.GetString("Expected a Batch line, but found a Journal");
                                        throw new Exception();
                                    }

                                    NewJournal = FMainDS.AJournal.NewRowTyped(true);
                                    ((TFrmGLBatch)ParentForm).GetJournalsControl().NewRowManual(ref NewJournal);
                                    NewJournal.BatchNumber = NewBatch.BatchNumber;
                                    FMainDS.AJournal.Rows.Add(NewJournal);

                                    NewJournal.JournalDescription = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    FImportMessage = Catalog.GetString("Parsing the sub system code of the journal");
                                    NewJournal.SubSystemCode = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    // TODO test if SubSystemCode exists in cached table
                                    FImportMessage = Catalog.GetString("Parsing the transaction type of the journal");
                                    NewJournal.TransactionTypeCode = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    // TODO test if TransactionTypeCode exists in cached table
                                    FImportMessage = Catalog.GetString("Parsing the currency of the journal");
                                    NewJournal.TransactionCurrency = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    // TODO test if Currency exists in cached table
                                    FImportMessage = Catalog.GetString("Parsing the exchange rate of the journal");
                                    NewJournal.ExchangeRateToBase =
                                        Convert.ToDecimal(StringHelper.GetNextCSV(ref FImportLine,
                                                FdlgSeparator.SelectedSeparator), FCultureInfoNumber);
                                    FImportMessage = Catalog.GetString("Parsing the date effective of the journal");
                                    NewJournal.DateEffective = Convert.ToDateTime(StringHelper.GetNextCSV(ref FImportLine,
                                            FdlgSeparator.SelectedSeparator), FCultureInfoDate);
                                }
                                else if (RowType == "T")
                                {
                                    if (NewJournal == null)
                                    {
                                        FImportMessage = Catalog.GetString("Expected a Journal or Batch line, but found a Transaction");
                                        throw new Exception();
                                    }

                                    GLBatchTDSATransactionRow NewTransaction = FMainDS.ATransaction.NewRowTyped(true);
                                    ((TFrmGLBatch)ParentForm).GetTransactionsControl().NewRowManual(ref NewTransaction, NewJournal);
                                    NewTransaction.BatchNumber = NewBatch.BatchNumber;
                                    NewTransaction.JournalNumber = NewJournal.JournalNumber;
                                    FMainDS.ATransaction.Rows.Add(NewTransaction);

                                    FImportMessage = Catalog.GetString("Parsing the cost centre of the transaction");
                                    NewTransaction.CostCentreCode = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    // TODO check if cost centre exists, and is a posting costcentre.
                                    // TODO check if cost centre is active. ask user if he wants to use an inactive cost centre
                                    FImportMessage = Catalog.GetString("Parsing the account code of the transaction");
                                    NewTransaction.AccountCode = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    // TODO check if account exists, and is a posting account.
                                    // TODO check if account is active. ask user if he wants to use an inactive account
                                    FImportMessage = Catalog.GetString("Parsing the narrative of the transaction");
                                    NewTransaction.Narrative = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    FImportMessage = Catalog.GetString("Parsing the reference of the transaction");
                                    NewTransaction.Reference = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    FImportMessage = Catalog.GetString("Parsing the transaction date");
                                    NewTransaction.TransactionDate =
                                        Convert.ToDateTime(StringHelper.GetNextCSV(ref FImportLine,
                                                FdlgSeparator.SelectedSeparator), FCultureInfoDate);

                                    FImportMessage = Catalog.GetString("Parsing the debit amount of the transaction");
                                    string DebitAmountString = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    decimal DebitAmount = DebitAmountString.Trim().Length == 0 ? 0.0M : Convert.ToDecimal(DebitAmountString,
                                        FCultureInfoNumber);
                                    FImportMessage = Catalog.GetString("Parsing the credit amount of the transaction");
                                    string CreditAmountString = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                    decimal CreditAmount = DebitAmountString.Trim().Length == 0 ? 0.0M : Convert.ToDecimal(CreditAmountString,
                                        FCultureInfoNumber);

                                    if ((DebitAmount == 0) && (CreditAmount == 0))
                                    {
                                        FImportMessage = Catalog.GetString("Either the debit amount or the debit amount must be greater than 0.");
                                    }

                                    if ((DebitAmount != 0) && (CreditAmount != 0))
                                    {
                                        FImportMessage = Catalog.GetString("You can not have a value for both debit and credit amount");
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

                                    for (int i = 0; i < 10; i++)
                                    {
                                        FImportMessage = String.Format(Catalog.GetString("Parsing Analysis Type/Value Pair #{0}. "), i);
                                        String type = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
                                        String v = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);

                                        //these data is only be imported if all corresponding values are there in the
                                        if ((type.Length > 0) && (v.Length > 0))
                                        {
                                            DataRow atrow = FCacheDS.AAnalysisType.Rows.Find(new Object[] { type });
                                            DataRow afrow = FCacheDS.AFreeformAnalysis.Rows.Find(new Object[] { NewTransaction.LedgerNumber, type, v });
                                            DataRow anrow =
                                                FCacheDS.AAnalysisAttribute.Rows.Find(new Object[] { NewTransaction.LedgerNumber,
                                                                                                     NewTransaction.AccountCode,
                                                                                                     type });

                                            if ((atrow != null) && (afrow != null) && (anrow != null))
                                            {
                                                ATransAnalAttribRow NewTransAnalAttrib = FMainDS.ATransAnalAttrib.NewRowTyped(true);
                                                ((TFrmGLBatch)ParentForm).GetAttributesControl().NewRowManual(ref NewTransAnalAttrib, NewTransaction);
                                                NewTransAnalAttrib.AnalysisTypeCode = type;
                                                NewTransAnalAttrib.AnalysisAttributeValue = v;
                                                NewTransAnalAttrib.AccountCode = NewTransaction.AccountCode;
                                                FMainDS.ATransAnalAttrib.Rows.Add(NewTransAnalAttrib);
                                            }
                                        }
                                    }

                                    // TODO If this is a fund transfer to a foreign cost centre, check whether there are Key Ministries available for it.
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        SaveUserDefaults(dialog, impOptions);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            String.Format(Catalog.GetString("There is a problem parsing the file in row {0}. "), RowNumber) +
                            Environment.NewLine +
                            FImportMessage + " " + e,
                            Catalog.GetString("Error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
            }
        }

        private String ImportString(String message)
        {
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);

            FImportMessage = Catalog.GetString("Parsing the " + message);
            return sReturn;
        }

        private void SaveUserDefaults(OpenFileDialog dialog, String impOptions)
        {
            TUserDefaults.SetDefault("Imp Filename", dialog.FileName);
            impOptions = FdlgSeparator.SelectedSeparator;
            impOptions += FdlgSeparator.NumberFormatIndex == 0 ? "American" : "European";
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", FdlgSeparator.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }
    }
}