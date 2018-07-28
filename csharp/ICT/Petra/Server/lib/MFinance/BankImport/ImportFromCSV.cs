//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Xml;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// import a bank statement from a CSV file
    /// </summary>
    public class TBankStatementImportCSV
    {
        /// <summary>
        /// import the data of a CSV file
        /// </summary>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <param name="ABankStatementFilename"></param>
        /// <param name="ACSVContent"></param>
        /// <param name="ASeparator"></param>
        /// <param name="ADateFormat">DMY or MDY</param>
        /// <param name="ANumberFormat">European or American</param>
        /// <param name="AColumnMeaning"></param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="AVerificationResult"></param>
        public static bool ImportBankStatement(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            string ABankStatementFilename,
            string ACSVContent,
            string ASeparator,
            string ADateFormat,
            string ANumberFormat,
            string AColumnMeaning,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            BankImportTDS MainDS = ImportBankStatementHelper(
                ALedgerNumber,
                ABankAccountCode,
                ASeparator,
                ADateFormat,
                ANumberFormat,
                AColumnMeaning,
                ABankStatementFilename,
                ACSVContent);

            if (MainDS != null)
            {
                if (TBankStatementImport.StoreNewBankStatement(
                        MainDS,
                        out AStatementKey) == TSubmitChangesResult.scrOK)
                {
                    return AStatementKey != -1;
                }
            }

            AStatementKey = -1;
            return false;
        }

        /// <summary>
        /// this can be used from the unit tests
        /// </summary>
        public static BankImportTDS ImportBankStatementHelper(Int32 ALedgerNumber,
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
            string ThousandsSeparator = (ANumberFormat == "American" ? "," : ".");
            string DecimalSeparator = (ANumberFormat == "American" ? "." : ",");

            List <String> StatementData = new List<string>();
            string [] stmtarray = AStatementData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in stmtarray)
            {
                StatementData.Add(line);
            }

            // skip headers
            Int32 lineCounter = FirstTransactionRow;

            // TODO: support splitting a file by month?
            // at the moment this only works for files that are already split by month
            // TODO: check if this statement has already been imported, by the stmt.Filename; delete old statement
            BankImportTDS MainDS = new BankImportTDS();
            AEpStatementRow stmt = MainDS.AEpStatement.NewRowTyped();
            stmt.StatementKey = -1;

            // TODO: BankAccountKey should be NOT NULL. for the moment not time to implement
            // stmt.BankAccountKey = Convert.ToInt64(TXMLParser.GetAttribute(RootNode, "BankAccountKey"));
            stmt.Filename = Path.GetFileName(ABankStatementFilename.Replace('\\', Path.DirectorySeparatorChar));

            // depending on the path of BankStatementFilename you could determine between several bank accounts
            // search all config parameters starting with "BankNameFor", 
            // and see if the rest of the parameter name is part of the filename or path
            StringCollection BankNameForParameters = TAppSettingsManager.GetKeys("BankNameFor");

            foreach(string BankNameForParameter in BankNameForParameters)
            {
                if (stmt.Filename.ToLower().Contains(BankNameForParameter.Substring("BankNameFor".Length).ToLower()))
                {
                    stmt.Filename = TAppSettingsManager.GetValue(BankNameForParameter);
                }
            }

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

            for (; lineCounter < StatementData.Count; lineCounter++)
            {
                string line = StatementData[lineCounter];

                rowCount++;

                AEpTransactionRow row = MainDS.AEpTransaction.NewRowTyped();
                row.StatementKey = stmt.StatementKey;
                row.Order = rowCount;
                row.NumberOnPaperStatement = row.Order;

                foreach (string UseAs in ColumnsUsage)
                {
                    string Value = StringHelper.GetNextCSV(ref line, StatementData, ref lineCounter, ASeparator);

                    if (UseAs.ToLower() == "dateeffective")
                    {
                        if (Value.Length == "dd.mm.yy".Length)
                        {
                            DateFormat = DateFormat.Replace("yyyy", "yy");
                        }

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
                        if (row.AccountName.Length > 0)
                        {
                            row.AccountName += " ";
                        }

                        row.AccountName += Value;
                    }
                    else if (UseAs.ToLower() == "description")
                    {
                        // remove everything after DTA; it is not relevant and confused matching
                        if (Value.IndexOf(" DTA ") > 0)
                        {
                            Value = Value.Substring(0, Value.IndexOf(" DTA "));
                        }

                        if (row.Description.Length > 0)
                        {
                            row.Description += " ";
                        }

                        row.Description += Value;
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
                    else if (UseAs.ToLower() == "directdebiths")
                    {
                        if (Value == "S")
                        {
                            row.TransactionAmount *= -1;
                        }
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
