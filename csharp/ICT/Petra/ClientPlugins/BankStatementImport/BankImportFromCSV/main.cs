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
using System.Data;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using GNU.Gettext;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.ClientPlugins.BankStatementImport.BankImportFromCSV
{
    /// <summary>
    /// import a bank statement from a CSV file
    /// </summary>
    public class TBankStatementImport : IImportBankStatement
    {
        /// <summary>
        /// asks the user to open a csv file and imports the contents according to the config file
        /// </summary>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <returns></returns>
        public bool ImportBankStatement(out Int32 AStatementKey, Int32 ALedgerNumber, string ABankAccountCode)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();

            DialogOpen.Filter = Catalog.GetString("bank statement (*.csv)|*.csv");
            DialogOpen.RestoreDirectory = true;
            DialogOpen.Title = Catalog.GetString("Please select the bank statement to import");

            if (DialogOpen.ShowDialog() != DialogResult.OK)
            {
                AStatementKey = -1;
                return false;
            }

            string BankStatementFilename = DialogOpen.FileName;

            TDlgSelectCSVSeparator DlgSeparator = new TDlgSelectCSVSeparator(false);
            DlgSeparator.CSVFileName = BankStatementFilename;
            String dateFormatString = TUserDefaults.GetStringDefault("BankimportCSVDateFormat", "MDY");
            String impOptions = TUserDefaults.GetStringDefault("BankimportCSVNumberFormat", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

            DlgSeparator.DateFormat = dateFormatString;

            if (impOptions.Length > 1)
            {
                DlgSeparator.NumberFormat = impOptions.Substring(1);
            }

            DlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

            if (DlgSeparator.ShowDialog() != DialogResult.OK)
            {
                AStatementKey = -1;
                return false;
            }

            TUserDefaults.SetDefault("BankimportCSVDateFormat", DlgSeparator.DateFormat);
            TUserDefaults.SetDefault("BankimportCSVNumberFormat", DlgSeparator.SelectedSeparator + DlgSeparator.NumberFormat);

            StreamReader dataFile = new StreamReader(BankStatementFilename, TTextFile.GetFileEncoding(BankStatementFilename), false);
            string StatementData = dataFile.ReadToEnd();
            dataFile.Close();

            BankImportTDS MainDS = ImportBankStatementNonInteractive(
                ALedgerNumber,
                ABankAccountCode,
                DlgSeparator.SelectedSeparator,
                DlgSeparator.DateFormat,
                DlgSeparator.NumberFormat,
                TUserDefaults.GetStringDefault(
                    "BankimportCSVColumnsUsage",
                    "unused,DateEffective,Description,Amount,Currency"),
                BankStatementFilename,
                StatementData);

            if (MainDS != null)
            {
                TVerificationResultCollection VerificationResult;

                if (TRemote.MFinance.ImportExport.WebConnectors.StoreNewBankStatement(
                        MainDS,
                        out AStatementKey,
                        out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    return AStatementKey != -1;
                }
            }

            AStatementKey = -1;
            return false;
        }

        /// <summary>
        /// this non interactive function can be used from the unit tests
        /// </summary>
        public BankImportTDS ImportBankStatementNonInteractive(Int32 ALedgerNumber,
            string ABankAccountCode,
            string ASeparator,
            string ADateFormat,
            string ANumberFormat,
            string AColumnsUsage,
            string ABankStatementFilename,
            string AStatementData)
        {
            Int32 FirstTransactionRow = 0;
            string DateFormat = (ADateFormat == "MDY" ? "M/d/yyyy" : "d.M.yyyy");
            string ThousandsSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "," : ".");
            string DecimalSeparator = (ANumberFormat == TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN ? "." : ",");

            string[] StatementData = AStatementData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // skip headers
            Int32 lineCounter = FirstTransactionRow;

            // TODO: support splitting a file by month?
            // at the moment this only works for files that are already split by month
            // TODO: check if this statement has already been imported, by the stmt.Filename; delete old statement
            BankImportTDS MainDS = new BankImportTDS();
            AEpStatementRow stmt = MainDS.AEpStatement.NewRowTyped();
            stmt.StatementKey = -1;

            // TODO: depending on the path of BankStatementFilename you could determine between several bank accounts
            // TODO: BankAccountKey should be NOT NULL. for the moment not time to implement
            // stmt.BankAccountKey = Convert.ToInt64(TXMLParser.GetAttribute(RootNode, "BankAccountKey"));
            stmt.Filename = ABankStatementFilename;

            if (stmt.Filename.Length > AEpStatementTable.GetFilenameLength())
            {
                // use the last number of characters of the path and filename
                stmt.Filename = ABankStatementFilename.Substring(ABankStatementFilename.Length - AEpStatementTable.GetFilenameLength());
            }

            stmt.LedgerNumber = ALedgerNumber;
            stmt.CurrencyCode = string.Empty;
            stmt.BankAccountCode = ABankAccountCode;
            MainDS.AEpStatement.Rows.Add(stmt);

            DateTime latestDate = DateTime.MinValue;

            Int32 rowCount = 0;

            // TODO would need to allow the user to change the order&meaning of columns
            string[] ColumnsUsage = AColumnsUsage.Split(new char[] { ',' });

            while (lineCounter < StatementData.Length)
            {
                string line = StatementData[lineCounter];

                lineCounter++;
                rowCount++;

                AEpTransactionRow row = MainDS.AEpTransaction.NewRowTyped();
                row.StatementKey = stmt.StatementKey;
                row.Order = rowCount;
                row.NumberOnPaperStatement = row.Order;

                foreach (string UseAs in ColumnsUsage)
                {
                    string Value = StringHelper.GetNextCSV(ref line, ASeparator);

                    if (UseAs.ToLower() == "dateeffective")
                    {
                        try
                        {
                            row.DateEffective = XmlConvert.ToDateTime(Value, DateFormat);
                        }
                        catch (Exception)
                        {
                            TLogging.Log("Problem with date effective: " + Value + " (Format: " + DateFormat + ")");
                        }

                        if (row.DateEffective > latestDate)
                        {
                            latestDate = row.DateEffective;
                        }
                    }
                    else if (UseAs.ToLower() == "accountname")
                    {
                        row.AccountName = Value;
                    }
                    else if (UseAs.ToLower() == "description")
                    {
                        // remove everything after DTA; it is not relevant and confused matching
                        if (Value.IndexOf(" DTA ") > 0)
                        {
                            Value = Value.Substring(0, Value.IndexOf(" DTA "));
                        }

                        row.Description = Value;
                    }
                    else if (UseAs.ToLower() == "amount")
                    {
                        if (Value.Contains(" "))
                        {
                            // cut off currency code; should have been defined in the data description file, for the whole batch
                            Value = Value.Substring(0, Value.IndexOf(" ") - 1);
                        }

                        Value = Value.Replace(ThousandsSeparator, "");
                        Value = Value.Replace(DecimalSeparator, ".");

                        row.TransactionAmount = Convert.ToDecimal(Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (UseAs.ToLower() == "currency")
                    {
                        if (stmt.CurrencyCode == string.Empty)
                        {
                            stmt.CurrencyCode = Value.ToUpper();
                        }
                        else if (stmt.CurrencyCode != Value.ToUpper())
                        {
                            throw new Exception("cannot mix several currencies in the same bank statement file");
                        }
                    }
                }

                // all transactions with positive amount can be donations
                if (row.TransactionAmount > 0)
                {
                    row.TransactionTypeCode = MFinanceConstants.BANK_STMT_POTENTIAL_GIFT;
                }

                MainDS.AEpTransaction.Rows.Add(row);
            }

            stmt.Date = latestDate;

            return MainDS;
        }
    }
}