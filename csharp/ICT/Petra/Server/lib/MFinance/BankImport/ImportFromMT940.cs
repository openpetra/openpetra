//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, jonass
//
// Copyright 2004-2021 by OM International
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
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// import a bank statement from a MT940 Swift file
    /// </summary>
    public class TBankStatementImportMT940
    {
        /// <summary>
        /// import one MT940 file, split into multiple statements per year
        /// </summary>
        static public bool ImportFromFile(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            string AFileName,
            string AFileContent,
            bool AParsePreviousYear,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            TSwiftParser parser = new TSwiftParser();

            parser.ProcessFileContent(AFileContent);

            BankImportTDS MainDS = new BankImportTDS();
            Int32 statementCounter = MainDS.AEpStatement.Rows.Count;

            foreach (TStatement stmt in parser.statements)
            {
                Int32 transactionCounter = 0;

                foreach (TTransaction tr in stmt.transactions)
                {
                    BankImportTDSAEpTransactionRow row = MainDS.AEpTransaction.NewRowTyped();

                    row.StatementKey = (statementCounter + 1) * -1;
                    row.Order = transactionCounter;
                    row.DetailKey = -1;
                    row.AccountName = tr.partnerName;

                    if ((tr.accountCode != null) && Regex.IsMatch(tr.accountCode, "^[A-Z]"))
                    {
                        // this is an iban
                        row.Iban = tr.accountCode;
                        row.Bic = tr.bankCode;
                        row.BranchCode = tr.accountCode.Substring(4, 8).TrimStart(new char[] { '0' });
                        row.BankAccountNumber = tr.accountCode.Substring(12).TrimStart(new char[] { '0' });
                    }
                    else if (tr.accountCode != null)
                    {
                        row.BankAccountNumber = tr.accountCode.TrimStart(new char[] { '0' });
                        row.BranchCode = tr.bankCode == null ? string.Empty : tr.bankCode.TrimStart(new char[] { '0' });
                        row.Iban = string.Empty;
                        row.Bic = string.Empty;
                    }

                    row.DateEffective = tr.valueDate;
                    row.TransactionAmount = tr.amount;
                    row.Description = tr.description;
                    row.TransactionTypeCode = tr.typecode;

                    // see the codes: http://www.hettwer-beratung.de/sepa-spezialwissen/sepa-technische-anforderungen/sepa-gesch%C3%A4ftsvorfallcodes-gvc-mt-940/
                    if ((row.TransactionTypeCode == "052")
                        || (row.TransactionTypeCode == "051")
                        || (row.TransactionTypeCode == "053")
                        || (row.TransactionTypeCode == "067")
                        || (row.TransactionTypeCode == "068")
                        || (row.TransactionTypeCode == "069")
                        || (row.TransactionTypeCode == "119") /* Einzelbuchung Spende (Purpose: CHAR) */
                        || (row.TransactionTypeCode == "152") /* SEPA Credit Transfer Einzelbuchung Dauerauftrag */
                        || (row.TransactionTypeCode == "166") /* SEPA Credit Transfer */
                        || (row.TransactionTypeCode == "169") /* SEPA Credit Transfer Donation */
                        )
                    {
                        row.TransactionTypeCode += MFinanceConstants.BANK_STMT_POTENTIAL_GIFT;
                    }

                    MainDS.AEpTransaction.Rows.Add(row);

                    transactionCounter++;
                }

                AEpStatementRow epstmt = MainDS.AEpStatement.NewRowTyped();
                epstmt.StatementKey = (statementCounter + 1) * -1;
                epstmt.LedgerNumber = ALedgerNumber;
                epstmt.Date = stmt.date;
                epstmt.CurrencyCode = stmt.currency;
                epstmt.Filename = AFileName;
                epstmt.BankAccountCode = ABankAccountCode;
                epstmt.IdFromBank = stmt.id;

                if (AFileName.Length > AEpStatementTable.GetFilenameLength())
                {
                    epstmt.Filename =
                        TAppSettingsManager.GetValue("BankNameFor" + stmt.bankCode + "/" + stmt.accountCode,
                            stmt.bankCode + "/" + stmt.accountCode, true);
                }

                epstmt.StartBalance = stmt.startBalance;
                epstmt.EndBalance = stmt.endBalance;

                MainDS.AEpStatement.Rows.Add(epstmt);

                // sort by amount, and by accountname; this is the order of the paper statements and attachments
                MainDS.AEpTransaction.DefaultView.Sort = BankImportTDSAEpTransactionTable.GetTransactionAmountDBName() + "," +
                                                          BankImportTDSAEpTransactionTable.GetOrderDBName();
                MainDS.AEpTransaction.DefaultView.RowFilter = BankImportTDSAEpTransactionTable.GetStatementKeyDBName() + "=" +
                                                               epstmt.StatementKey.ToString();

                // starting with the most negative amount, which should be the last in the order on the statement
                Int32 countOrderOnStatement = MainDS.AEpTransaction.DefaultView.Count;
                bool countingNegative = true;

                foreach (DataRowView rv in MainDS.AEpTransaction.DefaultView)
                {
                    BankImportTDSAEpTransactionRow row = (BankImportTDSAEpTransactionRow)rv.Row;

                    if ((row.TransactionAmount > 0) && countingNegative)
                    {
                        countingNegative = false;
                        countOrderOnStatement = 1;
                    }

                    if (countingNegative)
                    {
                        row.NumberOnPaperStatement = countOrderOnStatement;
                        countOrderOnStatement--;
                    }
                    else
                    {
                        row.NumberOnPaperStatement = countOrderOnStatement;
                        countOrderOnStatement++;
                    }
                }

                statementCounter++;
            }

            if (TBankStatementImport.StoreNewBankStatement(
                    MainDS,
                    out AStatementKey) == TSubmitChangesResult.scrOK)
            {
                return true;
            }

            return false;
        }
    }
}
