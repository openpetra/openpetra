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
using System.IO;
using Ict.Common;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Gui.BankImport;
using Ict.Plugins.Finance.SwiftParser;
using Mono.Unix;

namespace Ict.Plugins.Finance.SwiftParser
{
    /// <summary>
    ///
    /// </summary>
    public class TImportMT940 : IImportBankStatement
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

            return typeName + " = '052' OR " + typeName + " = '051' OR " + typeName + " = '053' OR " + typeName + " = '067' OR " + typeName +
                   " = '068' OR " + typeName + " = '069'";
        }

        /// <summary>
        /// open the file and return a typed datatable
        /// </summary>
        public bool ImportFromFile(string AFilename,
            ref BankImportTDS AMainDS,
            out double AStartBalance,
            out double AEndBalance,
            out string ABankName)
        {
            TSwiftParser parser = new TSwiftParser();

            AStartBalance = -1;
            AEndBalance = -1;
            ABankName = "";

            parser.ProcessFile(AFilename);

            Int32 statementCounter = 0;

            // TODO: support several statements per file?
            foreach (TStatement stmt in parser.statements)
            {
                Int32 transactionCounter = 0;

                foreach (TTransaction tr in stmt.transactions)
                {
                    AEpTransactionRow row = AMainDS.AEpTransaction.NewRowTyped();

                    row.StatementKey = statementCounter;
                    row.Order = transactionCounter;
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
                // TODO: use app setting BankAccounts
                // see http://www.bundesbank.de/zahlungsverkehr/zahlungsverkehr_bankleitzahlen_download.php
                if (ABankName == "52060410")
                {
                    ABankName += " (EKK)";
                }
                else if (ABankName == "67450048")
                {
                    ABankName += " (SPK)";
                }

                if (statementCounter == 0)
                {
                    AStartBalance = stmt.startBalance;
                }

                if (statementCounter == parser.statements.Count - 1)
                {
                    AEndBalance = stmt.endBalance;
                }

                statementCounter++;

                // TODO: don't support several bank statements per file at the moment
                break;
            }

            if (parser.statements.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show(Catalog.GetString("We don't support several bank statements per file at the moment"));
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
            string[] bankAccountData = TAppSettingsManager.GetValueStatic("BankAccounts").Split(new char[] { ',' });
            string RawPath = TAppSettingsManager.GetValueStatic("RawMT940.Path");
            string OutputPath = TAppSettingsManager.GetValueStatic("MT940.Output.Path");
            string[] RawSTAFiles = Directory.GetFiles(RawPath, "*.sta");

            CreateDirectories(OutputPath, bankAccountData);

            foreach (string RawFile in RawSTAFiles)
            {
                TSwiftParser Parser = new TSwiftParser();
                Parser.ProcessFile(RawFile);

                if (Parser.statements.Count > 0)
                {
                    bool filesWereSplit = false;
                    string lastDate = String.Empty;

                    foreach (TStatement stmt in Parser.statements)
                    {
                        for (Int32 bankCounter = 0; bankCounter < bankAccountData.Length / 3; bankCounter++)
                        {
                            if (bankAccountData[bankCounter * 3 + 0] == stmt.accountCode)
                            {
                                string newfilename = Path.GetFileName(RawFile);

                                if (!newfilename.StartsWith(lastDate))
                                {
                                    newfilename = lastDate + "_" + bankAccountData[bankCounter * 3 + 1] + newfilename;
                                }

                                newfilename = OutputPath + Path.DirectorySeparatorChar +
                                              bankAccountData[bankCounter * 3 + 2] + Path.DirectorySeparatorChar + newfilename;
                                TSwiftParser.DumpMT940File(newfilename,
                                    stmt);
                                filesWereSplit = true;
                                lastDate = stmt.date;
                            }
                        }
                    }

                    if (filesWereSplit)
                    {
                        // move original file to imported folder
                        // don't repeat the date in the filename
                        string Backupfilename = Path.GetFileName(RawFile);

                        if (!Backupfilename.StartsWith(lastDate))
                        {
                            Backupfilename = lastDate + Backupfilename;
                        }

                        string BackupName = OutputPath + Path.DirectorySeparatorChar + "imported" + Path.DirectorySeparatorChar + Backupfilename;
                        int CountBackup = 0;

                        while (File.Exists(BackupName))
                        {
                            BackupName = OutputPath + Path.DirectorySeparatorChar + "imported" +
                                         Path.DirectorySeparatorChar +
                                         Path.GetFileNameWithoutExtension(Backupfilename) + CountBackup.ToString() + ".sta";
                            CountBackup++;
                        }

                        File.Move(RawFile, BackupName);
                    }
                }
            }
        }
    }
}