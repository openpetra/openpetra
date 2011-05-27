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
using System.IO;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Plugins.Finance.SwiftParser;
using Ict.Petra.Client.App.Core.RemoteObjects;
using GNU.Gettext;

namespace Plugin.BankImportFromMT940
{
    /// <summary>
    /// import a bank statement from a MT940 Swift file
    /// </summary>
    public class TBankStatementImport : IImportBankStatement
    {
        /// <summary>
        /// the file extensions that should be used for the file open dialog
        /// </summary>
        public string GetFileFilter()
        {
            return "MT940 Datei (*.sta)|*.sta";
        }

        /// <summary>
        /// should return the text for the filter for AEpTransactionTable to get all the gifts, by transaction type
        /// </summary>
        /// <returns></returns>
        public string GetFilterGifts()
        {
            string typeName = AEpTransactionTable.GetTransactionTypeCodeDBName();

            // TODO: match GL transactions as well; independent of amount
            return typeName + " = '052' OR " + typeName + " = '051' OR " + typeName + " = '053' OR " + typeName + " = '067' OR " + typeName +
                   " = '068' OR " + typeName + " = '069'";
        }

        /// <summary>
        /// asks the user to open a csv file and imports the contents according to the config file
        /// </summary>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <returns></returns>
        public bool ImportBankStatement(out Int32 AStatementKey)
        {
            AStatementKey = -1;

            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString("bank statement MT940 (*.sta)|*.sta");
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Please select the bank statement to import");

            if (DialogOpen.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            string BankStatementFilename = DialogOpen.FileName;

            BankImportTDS MainDS = new BankImportTDS();

            decimal StartBalance, EndBalance;
            DateTime DateEffective;
            string BankName;

            if (ImportFromFile(BankStatementFilename,
                    ref MainDS,
                    out StartBalance,
                    out EndBalance,
                    out DateEffective,
                    out BankName) && (MainDS.AEpStatement.Count > 0))
            {
                foreach (AEpStatementRow stmt in MainDS.AEpStatement.Rows)
                {
                    MainDS.AEpTransaction.DefaultView.RowFilter =
                        String.Format("{0}={1}",
                            AEpTransactionTable.GetStatementKeyDBName(),
                            stmt.StatementKey);

                    DateTime latestDate = DateTime.MinValue;

                    foreach (DataRowView v in MainDS.AEpTransaction.DefaultView)
                    {
                        AEpTransactionRow tr = (AEpTransactionRow)v.Row;

                        if (tr.DateEffective > latestDate)
                        {
                            latestDate = tr.DateEffective;
                        }
                    }

                    stmt.Date = latestDate;
                }

                TVerificationResultCollection VerificationResult;
                TLogging.Log("writing to db");

                AEpStatementTable refStmt = MainDS.AEpStatement;

                if (TRemote.MFinance.ImportExport.WebConnectors.StoreNewBankStatement(ref refStmt,
                        MainDS.AEpTransaction,
                        out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    AStatementKey = refStmt[0].StatementKey;
                    return AStatementKey != -1;
                }
            }

            return false;
        }

        /// <summary>
        /// open the file and return a typed datatable
        /// </summary>
        public bool ImportFromFile(string AFilename,
            ref BankImportTDS AMainDS,
            out decimal AStartBalance,
            out decimal AEndBalance,
            out DateTime ADateEffective,
            out string ABankName)
        {
            TSwiftParser parser = new TSwiftParser();

            AStartBalance = -1;
            AEndBalance = -1;
            ABankName = "";
            ADateEffective = DateTime.MinValue;

            parser.ProcessFile(AFilename);

            Int32 statementCounter = 0;
            TLogging.Log(parser.statements.Count.ToString());

            // TODO: support several statements per file?
            foreach (TStatement stmt in parser.statements)
            {
                Int32 transactionCounter = 0;

                foreach (TTransaction tr in stmt.transactions)
                {
                    BankImportTDSAEpTransactionRow row = AMainDS.AEpTransaction.NewRowTyped();

                    row.StatementKey = (statementCounter + 1) * -1;
                    row.Order = transactionCounter;
                    row.DetailKey = -1;
                    row.AccountName = tr.partnerName;

                    if ((tr.accountCode != null) && tr.accountCode.StartsWith("0"))
                    {
                        // cut off leading zeros
                        row.BankAccountNumber = Convert.ToInt64(tr.accountCode).ToString();
                    }
                    else
                    {
                        // TODO: this could be IBAN/BIC, if it starts with a letter
                        row.BankAccountNumber = tr.accountCode;
                    }

                    if ((tr.bankCode != null) && tr.bankCode.StartsWith("0"))
                    {
                        row.BranchCode = Convert.ToInt64(tr.bankCode).ToString();
                    }
                    else
                    {
                        row.BranchCode = tr.bankCode;
                    }

                    row.DateEffective = tr.valueDate;
                    row.TransactionAmount = tr.amount;
                    row.Description = tr.description;
                    row.TransactionTypeCode = tr.typecode;

                    AMainDS.AEpTransaction.Rows.Add(row);

                    transactionCounter++;
                }

                ABankName = stmt.bankCode;

                // TODO; use BLZ List?
                // see http://www.bundesbank.de/zahlungsverkehr/zahlungsverkehr_bankleitzahlen_download.php

                string[] bankAccountData = TAppSettingsManager.GetValue("BankAccounts").Split(new char[] { ',' });

                for (Int32 bankCounter = 0; bankCounter < bankAccountData.Length / 3; bankCounter++)
                {
                    if (bankAccountData[bankCounter * 3 + 0] == ABankName)
                    {
                        ABankName = bankAccountData[bankCounter * 3 + 1];
                    }
                }

                if (statementCounter == 0)
                {
                    AStartBalance = stmt.startBalance;
                }

                if (statementCounter == parser.statements.Count - 1)
                {
                    AEndBalance = stmt.endBalance;
                    ADateEffective = stmt.date;
                }

                statementCounter++;

                // TODO: don't support several bank statements per file at the moment
                break;
            }

            if (parser.statements.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show(Catalog.GetString("We don't support several bank statements per file at the moment"));
            }

            // sort by amount, and by accountname; this is the order of the paper statements and attachments
            AMainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetTransactionAmountDBName() + "," +
                                                      BankImportTDSAEpTransactionTable.GetOrderDBName();
            AMainDS.AEpTransaction.DefaultView.RowFilter = "";
            Int32 countOrderOnStatement = 1;

            foreach (DataRowView rv in AMainDS.AEpTransaction.DefaultView)
            {
                BankImportTDSAEpTransactionRow row = (BankImportTDSAEpTransactionRow)rv.Row;

                if (row.TransactionAmount < 0)
                {
                    // TODO: sort by absolute amount, ignoring debit/credit?
                    row.NumberOnPaperStatement = 1000;
                }
                else
                {
                    row.NumberOnPaperStatement = countOrderOnStatement;
                    countOrderOnStatement++;
                }
            }

            statementCounter = 0;

            foreach (TStatement stmt in parser.statements)
            {
                AEpStatementRow epstmt = AMainDS.AEpStatement.NewRowTyped();
                epstmt.StatementKey = (statementCounter + 1) * -1;
                epstmt.Date = stmt.date;
                epstmt.CurrencyCode = stmt.currency;
                epstmt.Filename = AFilename;

                if (AFilename.Length > AEpStatementTable.GetFilenameLength())
                {
                    // use the last number of characters of the path and filename
                    // dangerous: Proficash always gives the same name?
                    epstmt.Filename = AFilename.Substring(AFilename.Length - AEpStatementTable.GetFilenameLength());
                }

                epstmt.EndBalance = stmt.endBalance;

                AMainDS.AEpStatement.Rows.Add(epstmt);

                statementCounter++;
            }

            return true;
        }

        /// create the output directories if they don't exist yet
        static private void CreateDirectories(string AOutputPath, string[] ABankAccountData)
        {
            if (!Directory.Exists(AOutputPath))
            {
                Directory.CreateDirectory(AOutputPath);
            }

            if (!Directory.Exists(AOutputPath + Path.DirectorySeparatorChar + "imported"))
            {
                Directory.CreateDirectory(AOutputPath + Path.DirectorySeparatorChar + "imported");
            }

            for (Int32 bankCounter = 0; bankCounter < ABankAccountData.Length / 3; bankCounter++)
            {
                string legalEntityPath = AOutputPath + Path.DirectorySeparatorChar + ABankAccountData[bankCounter * 3 + 2];

                if (!Directory.Exists(legalEntityPath))
                {
                    Directory.CreateDirectory(legalEntityPath);
                }

                if (!Directory.Exists(legalEntityPath + Path.DirectorySeparatorChar + "imported"))
                {
                    Directory.CreateDirectory(legalEntityPath + Path.DirectorySeparatorChar + "imported");
                }
            }
        }

        /// check for STA files in RawMT940.Path
        /// there are files from several banks, possibly for several legal entities
        /// one file can contain several bank statements from several days
        /// split the files into one file per statement, and move the file to a separate directory for each legal entity
        static public void SplitFilesAndMove()
        {
            // BankAccounts contains a comma separated list of bank accounts, each with bank account number, bank id, name for legal entity
            string[] bankAccountData = TAppSettingsManager.GetValue("BankAccounts").Split(new char[] { ',' });
            string RawPath = TAppSettingsManager.GetValue("RawMT940.Path");
            string OutputPath = TAppSettingsManager.GetValue("MT940.Output.Path");
            string[] RawSTAFiles = Directory.GetFiles(RawPath, "*.sta");

            CreateDirectories(OutputPath, bankAccountData);

            foreach (string RawFile in RawSTAFiles)
            {
                TSwiftParser Parser = new TSwiftParser();
                Parser.ProcessFile(RawFile);

                if (Parser.statements.Count > 0)
                {
                    bool filesWereSplit = false;
                    DateTime lastDate = DateTime.MinValue;

                    foreach (TStatement stmt in Parser.statements)
                    {
                        for (Int32 bankCounter = 0; bankCounter < bankAccountData.Length / 3; bankCounter++)
                        {
                            if (bankAccountData[bankCounter * 3 + 0] == stmt.accountCode)
                            {
                                lastDate = stmt.date;
                                string newfilename = OutputPath + Path.DirectorySeparatorChar +
                                                     bankAccountData[bankCounter * 3 + 2] + Path.DirectorySeparatorChar +
                                                     bankAccountData[bankCounter * 3 + 1] + "_" +
                                                     lastDate.ToString("yyyyMMdd") + ".sta";
                                TSwiftParser.DumpMT940File(newfilename,
                                    stmt);
                                filesWereSplit = true;
                            }
                        }
                    }

                    if (filesWereSplit)
                    {
                        // move original file to imported folder
                        // don't repeat the date in the filename
                        string Backupfilename = Path.GetFileName(RawFile);

                        if (!Backupfilename.StartsWith(lastDate.ToString("yyyyMMdd")))
                        {
                            Backupfilename = lastDate + Backupfilename;
                        }

                        string BackupName = OutputPath + Path.DirectorySeparatorChar + "imported" + Path.DirectorySeparatorChar + Backupfilename;
                        int CountBackup = 0;

                        while (File.Exists(BackupName))
                        {
                            BackupName = OutputPath + Path.DirectorySeparatorChar + "imported" +
                                         Path.DirectorySeparatorChar +
                                         Path.GetFileNameWithoutExtension(Backupfilename) + "_" + CountBackup.ToString() + ".sta";
                            CountBackup++;
                        }

                        File.Move(RawFile, BackupName);
                    }
                }
            }
        }
    }
}