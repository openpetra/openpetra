//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles importing of batches
    /// </summary>
    public class TUC_GLBatches_Import
    {
        /// <summary>
        /// An enumeration of possible data sources for importing batches or transactions
        /// </summary>
        public enum TImportDataSourceEnum
        {
            /// <summary>
            /// Import from a CSV file
            /// </summary>
            FromFile,

            /// <summary>
            /// Import from the clipboard in csv format
            /// </summary>
            FromClipboard
        };

        private TDlgSelectCSVSeparator FdlgSeparator;

        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private IUC_GLBatches FMyUserControl = null;
        private TFrmGLBatch FMyForm = null;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GLBatches_Import(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GLBatchTDS AMainDS, IUC_GLBatches AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FMyUserControl = AUserControl;

            FMyForm = (TFrmGLBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// The code handles importing from file or clipboard
        /// </summary>
        public void ImportBatches(TImportDataSourceEnum AImportDataSource)
        {
            bool ok = false;
            String importString;
            String impOptions;
            OpenFileDialog dialog = null;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.IMPORTING;

                FdlgSeparator = new TDlgSelectCSVSeparator(false);

                if (AImportDataSource == TImportDataSourceEnum.FromClipboard)
                {
                    importString = Clipboard.GetText(TextDataFormat.UnicodeText);

                    if ((importString == null) || (importString.Length == 0))
                    {
                        MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");
                    String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                    FdlgSeparator = new TDlgSelectCSVSeparator(false);
                    FdlgSeparator.SelectedSeparator = "\t";
                    FdlgSeparator.CSVData = importString;
                    FdlgSeparator.DateFormat = dateFormatString;

                    if (impOptions.Length > 1)
                    {
                        FdlgSeparator.NumberFormat = impOptions.Substring(1);
                    }
                }
                else if (AImportDataSource == TImportDataSourceEnum.FromFile)
                {
                    dialog = new OpenFileDialog();

                    string exportPath = TClientSettings.GetExportPath();
                    string fullPath = TUserDefaults.GetStringDefault("Imp Filename",
                        exportPath + Path.DirectorySeparatorChar + "import.csv");
                    TImportExportDialogs.SetOpenFileDialogFilePathAndName(dialog, fullPath, exportPath);

                    dialog.Title = Catalog.GetString("Import Batches from CSV File");
                    dialog.Filter = Catalog.GetString("GL Batch Files (*.csv)|*.csv|Text Files (*.txt)|*.txt");
                    impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

                    // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
                    TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(fullPath));

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                        if (!fileCanOpen)
                        {
                            MessageBox.Show(Catalog.GetString("Unable to open file."),
                                Catalog.GetString("Batch Import"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            return;
                        }

                        importString = File.ReadAllText(dialog.FileName, Encoding.Default);

                        String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                        FdlgSeparator.DateFormat = dateFormatString;

                        if (impOptions.Length > 1)
                        {
                            FdlgSeparator.NumberFormat = impOptions.Substring(1);
                        }

                        FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    // unknown source!!  The following need a value...
                    impOptions = String.Empty;
                    importString = String.Empty;
                }

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                    requestParams.Add("NewLine", Environment.NewLine);


                    TVerificationResultCollection AMessages = new TVerificationResultCollection();

                    Thread ImportThread = new Thread(() => ImportGLBatches(
                            requestParams,
                            importString,
                            out AMessages,
                            out ok));

                    using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                    {
                        ImportDialog.ShowDialog();
                    }

                    // We save the defaults even if ok is false - because the client will probably want to try and import
                    //   the same file again after correcting any errors
                    SaveUserDefaults(dialog, impOptions);
                    ShowMessages(AMessages);
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Batch Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    FMyUserControl.ReloadBatches();
                    FMyForm.GetBatchControl().SelectRowInGrid(1);

                    FPetraUtilsObject.SetChangedFlag();
                    FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction);
                }
            }
            finally
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
            }
        }

        /// <summary>
        /// this supports the transaction export files from Petra 2.x.
        /// Lines do NOT start with T for transaction
        /// </summary>
        public void ImportTransactions(ABatchRow ACurrentBatchRow, AJournalRow ACurrentJournalRow, TImportDataSourceEnum AImportDataSource)
        {
            bool ok = false;
            String importString;
            String impOptions;
            OpenFileDialog dialog = null;

            if (FPetraUtilsObject.HasChanges && !FMyForm.SaveChanges())
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save your changes before Importing new transactions"), Catalog.GetString(
                        "Import GL Transactions"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((ACurrentBatchRow == null)
                || (ACurrentJournalRow == null)
                || (ACurrentJournalRow.JournalStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                MessageBox.Show(Catalog.GetString("Please select an unposted journal to import transactions."),
                    Catalog.GetString("Import GL Transactions"));
                return;
            }

            if (ACurrentJournalRow.LastTransactionNumber > 0)
            {
                if (MessageBox.Show(Catalog.GetString(
                            "The current journal already contains some transactions.  Do you really want to add more transactions to this journal?"),
                        Catalog.GetString("Import GL Transactions"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            FdlgSeparator = new TDlgSelectCSVSeparator(false);

            if (AImportDataSource == TImportDataSourceEnum.FromClipboard)
            {
                importString = Clipboard.GetText(TextDataFormat.UnicodeText);

                if ((importString == null) || (importString.Length == 0))
                {
                    MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");
                String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                FdlgSeparator.SelectedSeparator = "\t";
                FdlgSeparator.CSVData = importString;
                FdlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormat = impOptions.Substring(1);
                }
            }
            else if (AImportDataSource == TImportDataSourceEnum.FromFile)
            {
                dialog = new OpenFileDialog();

                string exportPath = TClientSettings.GetExportPath();
                string fullPath = TUserDefaults.GetStringDefault("Imp Filename",
                    exportPath + Path.DirectorySeparatorChar + "import.csv");
                TImportExportDialogs.SetOpenFileDialogFilePathAndName(dialog, fullPath, exportPath);

                dialog.Title = Catalog.GetString("Import Transactions from CSV File");
                dialog.Filter = Catalog.GetString("GL Transactions files (*.csv)|*.csv|Text Files (*.txt)|*.txt");
                impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

                // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
                TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(fullPath));

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                    if (!fileCanOpen)
                    {
                        MessageBox.Show(Catalog.GetString("Unable to open file."),
                            Catalog.GetString("Transaction Import"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        return;
                    }

                    importString = File.ReadAllText(dialog.FileName, Encoding.Default);

                    String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
                    FdlgSeparator.DateFormat = dateFormatString;

                    if (impOptions.Length > 1)
                    {
                        FdlgSeparator.NumberFormat = impOptions.Substring(1);
                    }

                    FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);
                }
                else
                {
                    return;
                }
            }
            else
            {
                // unknown source!!  The following need a value...
                impOptions = String.Empty;
                importString = String.Empty;
            }

            if (FdlgSeparator.ShowDialog() == DialogResult.OK)
            {
                Hashtable requestParams = new Hashtable();

                requestParams.Add("ALedgerNumber", FLedgerNumber);
                requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                requestParams.Add("NewLine", Environment.NewLine);

                TVerificationResultCollection AMessages = new TVerificationResultCollection();

                Thread ImportThread = new Thread(() => ImportGLTransactions(requestParams,
                        importString,
                        ACurrentBatchRow.BatchNumber,
                        ACurrentJournalRow.JournalNumber,
                        out AMessages,
                        out ok));

                using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                {
                    ImportDialog.ShowDialog();
                }

                // It is important to save user defaults here, even if there were errors
                //   because in that case the user will want to import the same file again after fixing it.
                SaveUserDefaults(dialog, impOptions);
                ShowMessages(AMessages);
            }

            if (ok)
            {
                MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                    Catalog.GetString("Transactions Import"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Update the client side with new information from the server
                FMyForm.Cursor = Cursors.WaitCursor;
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAJournal(FLedgerNumber, ACurrentBatchRow.BatchNumber));
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionAndRelatedTablesForJournal(FLedgerNumber,
                        ACurrentBatchRow.BatchNumber, ACurrentJournalRow.JournalNumber));
                FMainDS.AcceptChanges();
                FMyForm.Cursor = Cursors.Default;

                FMyForm.GetTransactionsControl().SelectRow(1);
            }
        }

        /// <summary>
        /// Import a batch from a spreadsheet
        /// </summary>
        /// <param name="ACSVDataFileName"></param>
        /// <param name="ALatestTransactionDate"></param>
        /// <returns></returns>
        public bool ImportFromSpreadsheet(out string ACSVDataFileName, out DateTime ALatestTransactionDate)
        {
            ALatestTransactionDate = DateTime.MinValue;
            ACSVDataFileName = String.Empty;

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

                    FMyUserControl.CreateNewABatch();
                    GLBatchTDSAJournalRow NewJournal = FMainDS.AJournal.NewRowTyped(true);
                    FMyForm.GetJournalsControl().NewRowManual(ref NewJournal);
                    FMainDS.AJournal.Rows.Add(NewJournal);
                    NewJournal.TransactionCurrency = TXMLParser.GetAttribute(RootNode, "Currency");

                    if (Path.GetExtension(dialog.FileName).ToLower() == ".csv")
                    {
                        ACSVDataFileName = dialog.FileName;

                        CreateBatchFromCSVFile(dialog.FileName,
                            RootNode,
                            NewJournal,
                            FirstTransactionRow,
                            DefaultCostCentre,
                            out ALatestTransactionDate);

                        return true;
                    }
                }
            }

            return false;
        }

        /// load transactions from a CSV file into the currently selected batch
        private void CreateBatchFromCSVFile(string ADataFilename,
            XmlNode ARootNode,
            AJournalRow ARefJournalRow,
            Int32 AFirstTransactionRow,
            string ADefaultCostCentre,
            out DateTime ALatestTransactionDate)
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
            ALatestTransactionDate = DateTime.MinValue;

            do
            {
                string line = dataFile.ReadLine();
                lineCounter++;

                GLBatchTDSATransactionRow NewTransaction = FMainDS.ATransaction.NewRowTyped(true);
                FMyForm.GetTransactionsControl().NewRowManual(ref NewTransaction, ARefJournalRow);
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

                                if (NewTransaction.TransactionDate > ALatestTransactionDate)
                                {
                                    ALatestTransactionDate = NewTransaction.TransactionDate;
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
                    NewTransaction.AmountInBaseCurrency = GLRoutines.CurrencyMultiply(NewTransaction.TransactionAmount,
                        TExchangeRateCache.GetDailyExchangeRate(
                            ARefJournalRow.TransactionCurrency,
                            FMainDS.ALedger[0].BaseCurrency,
                            NewTransaction.TransactionDate,
                            false));
                    //
                    // The International currency calculation is changed to "Base -> International", because it's likely
                    // we won't have a "Transaction -> International" conversion rate defined.
                    //
                    NewTransaction.AmountInIntlCurrency = GLRoutines.CurrencyMultiply(NewTransaction.AmountInBaseCurrency,
                        TExchangeRateCache.GetDailyExchangeRate(
                            FMainDS.ALedger[0].BaseCurrency,
                            FMainDS.ALedger[0].IntlCurrency,
                            NewTransaction.TransactionDate,
                            false));
                }
            } while (!dataFile.EndOfStream);

            // create a balancing transaction; not sure if this is needed at all???
            if (Convert.ToDecimal(sumCredits - sumDebits) != 0)
            {
                GLBatchTDSATransactionRow BalancingTransaction = FMainDS.ATransaction.NewRowTyped(true);
                FMyForm.GetTransactionsControl().NewRowManual(ref BalancingTransaction, ARefJournalRow);
                FMainDS.ATransaction.Rows.Add(BalancingTransaction);

                BalancingTransaction.TransactionDate = ALatestTransactionDate;
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

                BalancingTransaction.AmountInIntlCurrency = GLRoutines.CurrencyMultiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].IntlCurrency,
                        BalancingTransaction.TransactionDate,
                        false));
                BalancingTransaction.AmountInBaseCurrency = GLRoutines.CurrencyMultiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].BaseCurrency,
                        BalancingTransaction.TransactionDate,
                        false));
                BalancingTransaction.Narrative = Catalog.GetString("Automatically generated balancing transaction");
                BalancingTransaction.CostCentreCode = TXMLParser.GetAttribute(ARootNode, "CashCostCentre");
                BalancingTransaction.AccountCode = TXMLParser.GetAttribute(ARootNode, "CashAccount");
            }

            ARefJournalRow.JournalCreditTotal = sumCredits;
            ARefJournalRow.JournalDebitTotal = sumDebits;
            ARefJournalRow.JournalDescription = Path.GetFileNameWithoutExtension(ADataFilename);

            ABatchRow RefBatch = (ABatchRow)FMainDS.ABatch.Rows[FMainDS.ABatch.Rows.Count - 1];
            RefBatch.BatchCreditTotal = sumCredits;
            RefBatch.BatchDebitTotal = sumDebits;
            // todo RefBatch.BatchControlTotal = sumCredits  - sumDebits;
            // csv !
        }

        private void SaveUserDefaults(OpenFileDialog dialog, String impOptions)
        {
            if (dialog != null)
            {
                TUserDefaults.SetDefault("Imp Filename", dialog.FileName);
            }

            impOptions = FdlgSeparator.SelectedSeparator;
            impOptions += FdlgSeparator.NumberFormat;
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", FdlgSeparator.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ImportGLBatches
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="AMessages"></param>
        /// <param name="ok"></param>
        private void ImportGLBatches(
            Hashtable ARequestParams,
            string AImportString,
            out TVerificationResultCollection AMessages,
            out bool ok)
        {
            TVerificationResultCollection AResultMessages;
            bool ImportIsSuccessful;

            ImportIsSuccessful = TRemote.MFinance.GL.WebConnectors.ImportGLBatches(
                ARequestParams,
                AImportString,
                out AResultMessages);

            ok = ImportIsSuccessful;
            AMessages = AResultMessages;
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ImportGLTransactions
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ok"></param>
        private void ImportGLTransactions(
            Hashtable ARequestParams,
            string AImportString,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            out TVerificationResultCollection AMessages,
            out bool ok)
        {
            TVerificationResultCollection AResultMessages;
            bool ImportIsSuccessful;

            ImportIsSuccessful = TRemote.MFinance.GL.WebConnectors.ImportGLTransactions(
                ARequestParams,
                AImportString,
                FLedgerNumber,
                ABatchNumber,
                AJournalNumber,
                out AResultMessages);

            ok = ImportIsSuccessful;
            AMessages = AResultMessages;
        }

        private void ShowMessages(TVerificationResultCollection AMessages)
        {
            StringBuilder ErrorMessages = new StringBuilder();

            if (AMessages.Count > 0)
            {
                foreach (TVerificationResult message in AMessages)
                {
                    ErrorMessages.AppendFormat("[{0}] {1}: {2}{3}", message.ResultContext, message.ResultTextCaption,
                        message.ResultText.Replace(Environment.NewLine, " "), Environment.NewLine);
                }
            }

            if (ErrorMessages.Length > 0)
            {
                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FPetraUtilsObject.GetForm());
                extendedMessageBox.ShowDialog(ErrorMessages.ToString(), Catalog.GetString("Import Errors"), String.Empty,
                    TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiError);
            }
        }
    }
}