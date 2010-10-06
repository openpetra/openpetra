//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// provides methods for exporting a batch
    /// </summary>
    public class TGLExporting
    {
        private const String quote = "\"";
        private const String summarizedData = "Summarised Transaction Data";

        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches"></param>
        /// <param name="requestParams"></param>
        /// <param name="exportString"></param>
        /// <returns>false if batch does not exist at all</returns>
        public static bool ExportAllGLBatchData(ref ArrayList batches, Hashtable requestParams, out String exportString)
        {
            StringWriter sw = new StringWriter();
            StringBuilder line = new StringBuilder();
            GLBatchTDS FMainDS = new GLBatchTDS();
            String Delimiter = (String)requestParams["Delimiter"];
            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            String dateFormatString = (String)requestParams["DateFormatString"];
            bool Summary = (bool)requestParams["Summary"];
            bool bUseBaseCurrency = (bool)requestParams["bUseBaseCurrency"];
            String BaseCurrency = (String)requestParams["BaseCurrency"];
            DateTime DateForSummary = (DateTime)requestParams["DateForSummary"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            CultureInfo ci = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            bool TransactionsOnly = (bool)requestParams["TransactionsOnly"];
            bool bDontSummarize = (bool)requestParams["bDontSummarize"];
            String DontSummarizeAccount = (String)requestParams["DontSummarizeAccount"];

            SortedDictionary <String, AJournalSummaryRow>sdSummary = new SortedDictionary <String, AJournalSummaryRow>();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            while (batches.Count > 0)
            {
                Int32 ABatchNumber = (Int32)batches[0];
                ABatchAccess.LoadByPrimaryKey(FMainDS, FLedgerNumber, ABatchNumber, Transaction);
                AJournalAccess.LoadViaABatch(FMainDS, FLedgerNumber, ABatchNumber, Transaction);

                foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                {
                    if (journal.BatchNumber.Equals(ABatchNumber) && journal.LedgerNumber.Equals(ALedgerNumber))
                    {
                        ATransactionAccess.LoadViaAJournal(FMainDS, journal.LedgerNumber,
                            journal.BatchNumber,
                            journal.JournalNumber,
                            Transaction);
                    }
                }

                batches.RemoveAt(0);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            UInt32 counter = 0;
            AJournalSummaryRow journalSummary = null;

            foreach (ABatchRow batch in FMainDS.ABatch.Rows)
            {
                if (!TransactionsOnly & !Summary)
                {
                    WriteBatchLine(ref sw, ref line, Delimiter, dateFormatString, ci, batch);
                }

                //foreach (AJournalRow journal in journalDS.AJournal.Rows)
                foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                {
                    if (journal.BatchNumber.Equals(batch.BatchNumber) && journal.LedgerNumber.Equals(batch.LedgerNumber))
                    {
                        if (Summary)
                        {
                            String mapCurrency = bUseBaseCurrency ? BaseCurrency : journal.TransactionCurrency;
                            double mapExchangeRateToBase = bUseBaseCurrency ? 1 : journal.ExchangeRateToBase;

                            if (!sdSummary.TryGetValue(mapCurrency, out journalSummary))
                            {
                                journalSummary = new AJournalSummaryRow();
                                sdSummary.Add(mapCurrency, journalSummary);
                            }

                            //overwrite always because we want to have the last
                            journalSummary.ExchangeRateToBase = mapExchangeRateToBase;
                            journalSummary.TransactionCurrency = mapCurrency;
                        }
                        else
                        {
                            if (!TransactionsOnly)
                            {
                                WriteJournalLine(ref sw, ref line, Delimiter, dateFormatString, ci, journal, bUseBaseCurrency, BaseCurrency);
                            }
                        }

                        //foreach (ATransactionRow transaction in journalDS.ATransaction.Rows)
                        foreach (ATransactionRow transaction in FMainDS.ATransaction.Rows)
                        {
                            if (transaction.JournalNumber.Equals(journal.JournalNumber) && transaction.BatchNumber.Equals(journal.BatchNumber)
                                && transaction.LedgerNumber.Equals(journal.LedgerNumber))
                            {
                                if (Summary)
                                {
                                    ATransactionSummaryRow transactionSummary;
                                    counter++;
                                    String DictionaryKey = transaction.CostCentreCode + ";" + transaction.AccountCode;
                                    int signum = transaction.DebitCreditIndicator ? 1 : -1;
                                    bool bDontSummarizeAccount = DontSummarizeAccount != null && DontSummarizeAccount.Length > 0
                                                                 && transaction.AccountCode.Equals(DontSummarizeAccount);

                                    if (bDontSummarizeAccount)
                                    {
                                        DictionaryKey += ";" + counter.ToString("X");
                                    }

                                    if (journalSummary.TransactionSummaries.TryGetValue(DictionaryKey, out transactionSummary))
                                    {
                                        transactionSummary.TransactionAmount += signum * transaction.TransactionAmount;
                                        transactionSummary.AmountInBaseCurrency += signum * transaction.AmountInBaseCurrency;
                                    }
                                    else
                                    {
                                        transactionSummary = new ATransactionSummaryRow();
                                        transactionSummary.CostCentreCode = transaction.CostCentreCode;
                                        transactionSummary.AccountCode = transaction.AccountCode;
                                        transactionSummary.TransactionAmount = signum * transaction.TransactionAmount;
                                        transactionSummary.AmountInBaseCurrency = signum * transaction.AmountInBaseCurrency;

                                        if (bDontSummarizeAccount)
                                        {
                                            transactionSummary.Narrative = transaction.Narrative;
                                            transactionSummary.Reference = transaction.Reference;
                                        }
                                        else
                                        {
                                            transactionSummary.Narrative = summarizedData;
                                            transactionSummary.Reference = "";
                                        }

                                        journalSummary.TransactionSummaries.Add(DictionaryKey, transactionSummary);
                                    }
                                }
                                else
                                {
                                    WriteTransactionLine(ref sw,
                                        ref line,
                                        Delimiter,
                                        dateFormatString,
                                        ci,
                                        TransactionsOnly,
                                        quote,
                                        transaction,
                                        bUseBaseCurrency);
                                }
                            }
                        }
                    }
                }
            }

            if (Summary)
            {
                //To simplify matters this is always written even if there are no batches
                WriteBatchSummaryLine(ref sw, ref line, Delimiter, DateForSummary, dateFormatString);

                foreach (KeyValuePair <string, AJournalSummaryRow>kvp in sdSummary)
                {
                    WriteJournalSummaryLine(ref sw,
                        ref line,
                        Delimiter,
                        DateForSummary,
                        dateFormatString,
                        ci,
                        kvp.Value,
                        bUseBaseCurrency,
                        BaseCurrency);

                    foreach (KeyValuePair <string, ATransactionSummaryRow>kvpt in kvp.Value.TransactionSummaries)
                    {
                        WriteTransactionSummaryLine(ref sw,
                            ref line,
                            Delimiter,
                            DateForSummary,
                            dateFormatString,
                            ci,
                            TransactionsOnly,
                            quote,
                            kvpt.Value,
                            bUseBaseCurrency);
                    }
                }
            }

            exportString = sw.ToString();
            return true;
        }

        static void WriteBatchSummaryLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            DateTime DateForSummary,
            String dateFormatString)
        {
            line.Append(quote);
            line.Append("B");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(summarizedData);
            line.Append(quote);
            line.Append(Delimiter);
            line.Append("0");
            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, DateForSummary);
            sw.WriteLine(line);
            line.Length = 0;
        }

        static void WriteBatchLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            String dateFormatString,
            CultureInfo ci,
            ABatchRow batch)
        {
            line.Append(quote);
            line.Append("B");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(batch.BatchDescription.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.AppendFormat(ci, "{0:f}", batch.BatchControlTotal);
            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, batch.DateEffective);
            sw.WriteLine(line);
            line.Length = 0;
        }

        static void WriteJournalSummaryLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            DateTime DateForSummary,
            String dateFormatString,
            CultureInfo ci,
            AJournalSummaryRow journalSummary,
            bool useBaseCurrency,
            String BaseCurrency)
        {
            line.Append(quote);
            line.Append("J");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(summarizedData);
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append("GL");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append("STD");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(journalSummary.TransactionCurrency.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.AppendFormat(ci, "{0:f}", journalSummary.ExchangeRateToBase);
            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, DateForSummary);
            sw.WriteLine(line);
            line.Length = 0;
        }

        static void WriteJournalLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            String dateFormatString,
            CultureInfo ci,
            AJournalRow journal,
            bool useBaseCurrency,
            String BaseCurrency)
        {
            line.Append(quote);
            line.Append("J");
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(journal.JournalDescription.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(journal.SubSystemCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(journal.TransactionTypeCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);

            if (useBaseCurrency)
            {
                line.Append(BaseCurrency);
                line.Append(quote);
                line.Append(Delimiter);
                line.Append("1");
            }
            else
            {
                line.Append(journal.TransactionCurrency.ToString());
                line.Append(quote);
                line.Append(Delimiter);
                line.AppendFormat(ci, "{0:f}", journal.ExchangeRateToBase);
            }

            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, journal.DateEffective);
            sw.WriteLine(line);
            line.Length = 0;
        }

        static void WriteTransactionLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            String dateFormatString,
            CultureInfo ci,
            bool TransactionsOnly,
            String quote,
            ATransactionRow transaction,
            bool useBaseCurrency)
        {
            if (!TransactionsOnly)
            {
                line.Append(quote);
                line.Append("T");
                line.Append(quote);
                line.Append(Delimiter);
            }

            line.Append(quote);
            line.Append(transaction.CostCentreCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transaction.AccountCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transaction.Narrative.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transaction.Reference.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, transaction.TransactionDate);
            line.Append(Delimiter);
            double amount = (useBaseCurrency) ? transaction.AmountInBaseCurrency : transaction.TransactionAmount;

            if (transaction.DebitCreditIndicator)
            {
                line.AppendFormat(ci, "{0:f}", amount);
                line.Append(Delimiter);
                line.Append("0");
            }
            else
            {
                line.Append("0");
                line.Append(Delimiter);
                line.AppendFormat(ci, "{0:f}", amount);
            }

            for (int i = 1; i < 10; i++)
            {
                line.Append(Delimiter);
                line.Append(quote);
                line.Append(quote);
            }

            sw.WriteLine(line);
            line.Length = 0;
        }

        static void WriteTransactionSummaryLine(ref StringWriter sw,
            ref StringBuilder line,
            String Delimiter,
            DateTime DateForSummary,
            String dateFormatString,
            CultureInfo ci,
            bool TransactionsOnly,
            String quote,
            ATransactionSummaryRow transactionSummary,
            bool useBaseCurrency)
        {
            if (!TransactionsOnly)
            {
                line.Append(quote);
                line.Append("T");
                line.Append(quote);
                line.Append(Delimiter);
            }

            line.Append(quote);
            line.Append(transactionSummary.CostCentreCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transactionSummary.AccountCode.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transactionSummary.Narrative.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.Append(quote);
            line.Append(transactionSummary.Reference.ToString());
            line.Append(quote);
            line.Append(Delimiter);
            line.AppendFormat(dateFormatString, DateForSummary);
            line.Append(Delimiter);
            double amount = (useBaseCurrency) ? transactionSummary.AmountInBaseCurrency : transactionSummary.TransactionAmount;

            if (amount > 0)
            {
                line.AppendFormat(ci, "{0:f}", amount);
                line.Append(Delimiter);
                line.Append("0");
            }
            else
            {
                line.Append("0");
                line.Append(Delimiter);
                line.AppendFormat(ci, "{0:f}", -amount);
            }

            sw.WriteLine(line);
            line.Length = 0;
        }
    }
    /// <summary>
    /// provides the outer structure for summarizing journals
    /// </summary>
    public class AJournalSummaryRow
    {
        private String transactionCurrency;
        private double exchangeRateToBase;
        private SortedDictionary <String, ATransactionSummaryRow>transactionSummaries = new SortedDictionary <String, ATransactionSummaryRow>();
        /// <summary>A SortedDictinary contains the subordinate Summary Rows.</summary>
        public SortedDictionary <string, ATransactionSummaryRow>TransactionSummaries {
            get
            {
                return transactionSummaries;
            }
            set
            {
                transactionSummaries = value;
            }
        }

        /// <summary>
        /// Exchange Rate
        /// </summary>
        public double ExchangeRateToBase {
            get
            {
                return exchangeRateToBase;
            }
            set
            {
                exchangeRateToBase = value;
            }
        }

        /// <summary>
        /// Transaction Currency
        /// </summary>
        public string TransactionCurrency {
            get
            {
                return transactionCurrency;
            }
            set
            {
                transactionCurrency = value;
            }
        }
    }
    /// <summary>
    /// provides the inner structure for summarising transactions
    /// </summary>
    public class ATransactionSummaryRow
    {
        private String costCentreCode;

        /// <summary>
        /// Cost Centre
        /// </summary>
        public string CostCentreCode {
            get
            {
                return costCentreCode;
            }
            set
            {
                costCentreCode = value;
            }
        }
        private String accountCode;

        /// <summary>
        /// Account Code
        /// </summary>
        public string AccountCode {
            get
            {
                return accountCode;
            }
            set
            {
                accountCode = value;
            }
        }
        private String narrative;

        /// <summary>
        /// Narrative
        /// </summary>
        public string Narrative {
            get
            {
                return narrative;
            }
            set
            {
                narrative = value;
            }
        }

        private String reference;

        /// <summary>
        /// Reference
        /// </summary>
        public string Reference {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }

        private double transactionAmount;

        /// <summary>
        /// Transaction Amount (may be negative!!!)
        /// </summary>
        public double TransactionAmount {
            get
            {
                return transactionAmount;
            }
            set
            {
                transactionAmount = value;
            }
        }
        private double amountInBaseCurrency;

        /// <summary>
        /// Amount in Base currency (may be negative!!!)
        /// </summary>
        public double AmountInBaseCurrency {
            get
            {
                return amountInBaseCurrency;
            }
            set
            {
                amountInBaseCurrency = value;
            }
        }
    }
}