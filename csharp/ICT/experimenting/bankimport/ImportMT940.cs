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
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Gui.BankImport;
using Ict.Plugins.Finance.SwiftParser;

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
        /// open the file and return a typed datatable
        /// </summary>
        public bool ImportFromFile(string AFilename, ref BankImportTDS AMainDS, out double ATotalAmountStatement, out Int32 ANumberAllTransactions)
        {
            TSwiftParser parser = new TSwiftParser();

            parser.ProcessFile(AFilename);

            ATotalAmountStatement = 0.0;
            ANumberAllTransactions = -1;
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

                ATotalAmountStatement = stmt.endBalance - stmt.startBalance;
                ANumberAllTransactions = stmt.transactions.Count;

                statementCounter++;
            }

            return true;
        }
    }
}