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
        /// </summary>
        public void ImportBatches()
        {
            bool ok = false;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.FileName = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "import.csv");

            dialog.Title = Catalog.GetString("Import batches from csv file");
            dialog.Filter = Catalog.GetString("GL Batches files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                if (!fileCanOpen)
                {
                    MessageBox.Show(Catalog.GetString("Unable to open file."),
                        Catalog.GetString("Batch Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }

                FdlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                    requestParams.Add("NewLine", Environment.NewLine);


                    TVerificationResultCollection AMessages = new TVerificationResultCollection();
                    string importString = File.ReadAllText(dialog.FileName);

                    Thread ImportThread = new Thread(() => ImportGLBatches(
                            requestParams,
                            importString,
                            out AMessages,
                            out ok));

                    using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                    {
                        ImportDialog.ShowDialog();
                    }

                    ShowMessages(AMessages);
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Batch Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SaveUserDefaults(dialog, impOptions);
                    FMyUserControl.ReloadBatches();
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, T for transaction
        /// </summary>
        public void ImportTransactions(ABatchRow ACurrentBatchRow, AJournalRow ACurrentJournalRow)
        {
            bool ok = false;

            if (FPetraUtilsObject.HasChanges && !FMyForm.SaveChanges())
            {
                return;
            }

            if ((ACurrentBatchRow == null)
                || (ACurrentJournalRow == null)
                || (ACurrentJournalRow.JournalStatus != MFinanceConstants.BATCH_UNPOSTED)
                || (ACurrentJournalRow.LastTransactionNumber > 0))
            {
                MessageBox.Show(Catalog.GetString("Please select an empty unposted journal to import transactions"), "Import GL Transactions");
                return;
            }

            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.FileName = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "import.csv");

            dialog.Title = Catalog.GetString("Import batches from csv file");
            dialog.Filter = Catalog.GetString("GL Transactions files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                if (!fileCanOpen)
                {
                    MessageBox.Show(Catalog.GetString("Unable to open file."),
                        Catalog.GetString("Transaction Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }

                FdlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                    requestParams.Add("NewLine", Environment.NewLine);

                    TVerificationResultCollection AMessages = new TVerificationResultCollection();
                    string importString = File.ReadAllText(dialog.FileName);

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

                    ShowMessages(AMessages);
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Transactions Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SaveUserDefaults(dialog, impOptions);
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(FLedgerNumber,
                            ACurrentBatchRow.BatchNumber, ACurrentJournalRow.JournalNumber));

                    //Update totals and set Journal last transaction number
                    GLRoutines.UpdateTotalsOfBatch(ref FMainDS, ACurrentBatchRow);
                    FMyForm.GetTransactionsControl().SelectRow(1);
                    FMyForm.SaveChanges();
                }
            }
        }

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction.
        /// This particular functions allows to paste batches from Excel/LibreOfficeCalc via clipboard
        /// </summary>
        public void ImportFromClipboard()
        {
            bool ok = false;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string clipboardData = Clipboard.GetText(TextDataFormat.Text);

            if ((clipboardData == null) || (clipboardData.Length == 0))
            {
                MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                    Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");
            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            FdlgSeparator = new TDlgSelectCSVSeparator(false);
            FdlgSeparator.SelectedSeparator = "\t";
            FdlgSeparator.CSVData = clipboardData;
            FdlgSeparator.DateFormat = dateFormatString;

            if (impOptions.Length > 1)
            {
                FdlgSeparator.NumberFormat = impOptions.Substring(1);
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
                        clipboardData,
                        out AMessages,
                        out ok));

                using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                {
                    ImportDialog.ShowDialog();
                }

                ShowMessages(AMessages);
            }

            if (ok)
            {
                MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                    Catalog.GetString("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SaveUserDefaults(null, impOptions);
                FMyUserControl.ReloadBatches();
                FPetraUtilsObject.DisableSaveButton();
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
                    NewTransaction.AmountInBaseCurrency = GLRoutines.Multiply(NewTransaction.TransactionAmount,
                        TExchangeRateCache.GetDailyExchangeRate(
                            ARefJournalRow.TransactionCurrency,
                            FMainDS.ALedger[0].BaseCurrency,
                            NewTransaction.TransactionDate));
                    //
                    // The International currency calculation is changed to "Base -> International", because it's likely
                    // we won't have a "Transaction -> International" conversion rate defined.
                    //
                    NewTransaction.AmountInIntlCurrency = GLRoutines.Multiply(NewTransaction.AmountInBaseCurrency,
                        TExchangeRateCache.GetDailyExchangeRate(
                            FMainDS.ALedger[0].BaseCurrency,
                            FMainDS.ALedger[0].IntlCurrency,
                            NewTransaction.TransactionDate));
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

                BalancingTransaction.AmountInIntlCurrency = GLRoutines.Multiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].IntlCurrency,
                        BalancingTransaction.TransactionDate));
                BalancingTransaction.AmountInBaseCurrency = GLRoutines.Multiply(BalancingTransaction.TransactionAmount,
                    TExchangeRateCache.GetDailyExchangeRate(
                        ARefJournalRow.TransactionCurrency,
                        FMainDS.ALedger[0].BaseCurrency,
                        BalancingTransaction.TransactionDate));
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

            GLBatchTDS MainDS = StripMainDataSetForTransImport(ABatchNumber, AJournalNumber);

            ImportIsSuccessful = TRemote.MFinance.GL.WebConnectors.ImportGLTransactions(
                ARequestParams,
                AImportString,
                ref MainDS,
                out AResultMessages);

            ok = ImportIsSuccessful;
            AMessages = AResultMessages;
        }

        private GLBatchTDS StripMainDataSetForTransImport(int ABatchNumber, int AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();

            MainDS.Merge(FMainDS.ABatch);
            MainDS.Merge(FMainDS.AJournal);

            DataView JournalDV = new DataView(MainDS.AJournal);

            JournalDV.RowFilter = String.Format("({0}={1} And {2}<>{3}) Or ({0}<>{1})",
                AJournalTable.GetBatchNumberDBName(),
                ABatchNumber,
                AJournalTable.GetJournalNumberDBName(),
                AJournalNumber);

            JournalDV.Sort = String.Format("{0} DESC, {1} DESC",
                AJournalTable.GetBatchNumberDBName(),
                AJournalTable.GetJournalNumberDBName());

            foreach (DataRowView drv in JournalDV)
            {
                AJournalRow jr = (AJournalRow)drv.Row;
                jr.Delete();
            }

            DataView BatchDV = new DataView(MainDS.ABatch);

            BatchDV.RowFilter = String.Format("{0}<>{1}",
                ABatchTable.GetBatchNumberDBName(),
                ABatchNumber);

            BatchDV.Sort = String.Format("{0} DESC",
                ABatchTable.GetBatchNumberDBName());

            foreach (DataRowView drv in BatchDV)
            {
                ABatchRow br = (ABatchRow)drv.Row;
                br.Delete();
            }

            MainDS.AcceptChanges();

            return MainDS;
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