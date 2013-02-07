//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
//
// Copyright 2004-2011 by OM International
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// provides methods for exporting a batch
    /// </summary>
    public class TGLExporting
    {
        private const String quote = "\"";
        private const String summarizedData = "Summarised Transaction Data";
        StringWriter FStringWriter;
        String FDelimiter;
        Int32 FLedgerNumber;
        String FDateFormatString;
        CultureInfo FCultureInfo;
        bool FSummary;
        bool FUseBaseCurrency;
        String FBaseCurrency;
        DateTime FDateForSummary;
        bool FTransactionsOnly;
        bool FDontSummarize;
        String FDontSummarizeAccount;
        GLBatchTDS FMainDS;


        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches"></param>
        /// <param name="requestParams"></param>
        /// <param name="exportString"></param>
        /// <returns>false if batch does not exist at all</returns>
        public bool ExportAllGLBatchData(ref ArrayList batches, Hashtable requestParams, out String exportString)
        {
            FStringWriter = new StringWriter();
            FMainDS = new GLBatchTDS();
            FDelimiter = (String)requestParams["Delimiter"];
            FLedgerNumber = (Int32)requestParams["ALedgerNumber"];
            FDateFormatString = (String)requestParams["DateFormatString"];
            FSummary = (bool)requestParams["Summary"];
            FUseBaseCurrency = (bool)requestParams["bUseBaseCurrency"];
            FBaseCurrency = (String)requestParams["BaseCurrency"];
            FDateForSummary = (DateTime)requestParams["DateForSummary"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            FCultureInfo = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FTransactionsOnly = (bool)requestParams["TransactionsOnly"];
            FDontSummarize = (bool)requestParams["bDontSummarize"];
            FDontSummarizeAccount = (String)requestParams["DontSummarizeAccount"];

            SortedDictionary <String, AJournalSummaryRow>sdSummary = new SortedDictionary <String, AJournalSummaryRow>();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            UInt32 TProgressCounter = 0;
            UInt32 TProgressJournalCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Exporting GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Retrieving records"),
                10);

            while (batches.Count > 0)
            {
                Int32 ABatchNumber = (Int32)batches[0];
                ABatchAccess.LoadByPrimaryKey(FMainDS, FLedgerNumber, ABatchNumber, Transaction);
                AJournalAccess.LoadViaABatch(FMainDS, FLedgerNumber, ABatchNumber, Transaction);

                foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                {
                    if (journal.BatchNumber.Equals(ABatchNumber) && journal.LedgerNumber.Equals(FLedgerNumber))
                    {
                        ATransactionAccess.LoadViaAJournal(FMainDS, journal.LedgerNumber,
                            journal.BatchNumber,
                            journal.JournalNumber,
                            Transaction);
                    }
                }

                foreach (ATransactionRow trans in FMainDS.ATransaction.Rows)
                {
                    if (trans.BatchNumber.Equals(ABatchNumber) && trans.LedgerNumber.Equals(FLedgerNumber))
                    {
                        ATransAnalAttribAccess.LoadViaATransaction(FMainDS, trans.LedgerNumber,
                            trans.BatchNumber,
                            trans.JournalNumber,
                            trans.TransactionNumber,
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
                TProgressCounter = 0;

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    String.Format(Catalog.GetString("Batch {0}"), batch.BatchNumber),
                    20);

                if (!FTransactionsOnly & !FSummary)
                {
                    WriteBatchLine(batch);
                }

                //foreach (AJournalRow journal in journalDS.AJournal.Rows)
                foreach (AJournalRow journal in FMainDS.AJournal.Rows)
                {
                    if (journal.BatchNumber.Equals(batch.BatchNumber) && journal.LedgerNumber.Equals(batch.LedgerNumber))
                    {
                        TProgressJournalCounter = 0;

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            String.Format(Catalog.GetString("Batch {0}, Journal {1}"), batch.BatchNumber, journal.JournalNumber),
                            (TProgressCounter / 25 + 4) * 5 >90 ? 90 : (TProgressCounter /  25 + 4) * 5);

                        if (FSummary)
                        {
                            String mapCurrency = FUseBaseCurrency ? FBaseCurrency : journal.TransactionCurrency;
                            decimal mapExchangeRateToBase = FUseBaseCurrency ? 1 : journal.ExchangeRateToBase;

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
                            if (!FTransactionsOnly)
                            {
                                WriteJournalLine(journal);
                            }
                        }

                        FMainDS.ATransaction.DefaultView.Sort = ATransactionTable.GetTransactionNumberDBName();
                        FMainDS.ATransaction.DefaultView.RowFilter =
                            String.Format("{0}={1} and {2}={3} and {4}={5}",
                                ATransactionTable.GetLedgerNumberDBName(),
                                journal.LedgerNumber,
                                ATransactionTable.GetBatchNumberDBName(),
                                journal.BatchNumber,
                                ATransactionTable.GetJournalNumberDBName(),
                                journal.JournalNumber);

                        foreach (DataRowView dv in FMainDS.ATransaction.DefaultView)
                        {
                            TProgressJournalCounter++;

                            if (++TProgressCounter % 25 == 0)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    String.Format(Catalog.GetString("Batch {0}, Journal {1} - {2}"), batch.BatchNumber, journal.JournalNumber, TProgressJournalCounter),
                                    (TProgressCounter / 25 + 4) * 5 > 90 ? 90 : (TProgressCounter / 25 + 4) * 5);
                            }

                            ATransactionRow transaction = (ATransactionRow)dv.Row;

                            if (FSummary)
                            {
                                ATransactionSummaryRow transactionSummary;
                                counter++;
                                String DictionaryKey = transaction.CostCentreCode + ";" + transaction.AccountCode;
                                int signum = transaction.DebitCreditIndicator ? 1 : -1;
                                bool bDontSummarizeAccount = FDontSummarize && FDontSummarizeAccount != null && FDontSummarizeAccount.Length > 0
                                                             && transaction.AccountCode.Equals(FDontSummarizeAccount);

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
                                WriteTransactionLine(transaction);
                            }
                        }
                    }
                }
            }

            if (FSummary)
            {
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Summarizing"),
                    95);

                //To simplify matters this is always written even if there are no batches
                if (!FTransactionsOnly)
                {
                    // no batch summary line if only transactions are to be exported
                    WriteBatchSummaryLine();
                }

                foreach (KeyValuePair <string, AJournalSummaryRow>kvp in sdSummary)
                {
                    if (!FTransactionsOnly)
                    {
                        // no journal summary line if only transactions are to be exported
                        WriteJournalSummaryLine(kvp.Value);
                    }

                    foreach (KeyValuePair <string, ATransactionSummaryRow>kvpt in kvp.Value.TransactionSummaries)
                    {
                        WriteTransactionSummaryLine(kvpt.Value);
                    }
                }
            }

            exportString = FStringWriter.ToString();

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("GL batch export successful"),
                100);

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }

        void WriteBatchSummaryLine()
        {
            WriteStringQuoted("B");
            WriteStringQuoted(summarizedData);
            WriteCurrency(0);
            WriteLineDate(FDateForSummary);
        }

        void WriteBatchLine(ABatchRow batch)
        {
            WriteStringQuoted("B");
            WriteStringQuoted(batch.BatchDescription);
            WriteCurrency(batch.BatchControlTotal);
            WriteLineDate(batch.DateEffective);
        }

        void WriteJournalSummaryLine(AJournalSummaryRow journalSummary)
        {
            WriteStringQuoted("J");
            WriteStringQuoted(summarizedData);
            WriteStringQuoted("GL");
            WriteStringQuoted("STD");
            WriteStringQuoted(journalSummary.TransactionCurrency);
            WriteGeneralNumber(journalSummary.ExchangeRateToBase);             // format ok ???
            WriteLineDate(FDateForSummary);
        }

        void WriteJournalLine(AJournalRow journal)
        {
            WriteStringQuoted("J");
            WriteStringQuoted(journal.JournalDescription);
            WriteStringQuoted(journal.SubSystemCode);
            WriteStringQuoted(journal.TransactionTypeCode);

            if (FUseBaseCurrency)
            {
                WriteStringQuoted(FBaseCurrency);
                WriteGeneralNumber(1);
            }
            else
            {
                WriteStringQuoted(journal.TransactionCurrency);
                WriteGeneralNumber(journal.ExchangeRateToBase);
            }

            WriteLineDate(journal.DateEffective);
        }

        void WriteTransactionLine(ATransactionRow transaction)
        {
            if (!FTransactionsOnly)
            {
                WriteStringQuoted("T");
            }

            WriteStringQuoted(transaction.CostCentreCode);
            WriteStringQuoted(transaction.AccountCode);
            WriteStringQuoted(transaction.Narrative);
            WriteStringQuoted(transaction.Reference);
            WriteDate(transaction.TransactionDate);
            decimal amount = (FUseBaseCurrency) ? transaction.AmountInBaseCurrency : transaction.TransactionAmount;

            if (transaction.DebitCreditIndicator)
            {
                WriteCurrency(amount);
                WriteCurrency(0);
            }
            else
            {
                WriteCurrency(0);
                WriteCurrency(amount);
            }

            WriteAnalysisAttributesSuffix(transaction);
        }

        static int maxNumValuesExport = 5;
        void WriteAnalysisAttributesSuffix(ATransactionRow transaction)
        {
            FMainDS.ATransAnalAttrib.DefaultView.Sort = ATransAnalAttribTable.GetAnalysisTypeCodeDBName();
            FMainDS.ATransAnalAttrib.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}={3} and {4}={5} and {6}={7}",
                    ATransAnalAttribTable.GetLedgerNumberDBName(),
                    transaction.LedgerNumber,
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    transaction.BatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    transaction.JournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    transaction.TransactionNumber);


            DataView anaView = FMainDS.ATransAnalAttrib.DefaultView;

            for (int i = 1; i <= maxNumValuesExport; i++)
            {
                if (i <= anaView.Count)
                {
                    ATransAnalAttribRow ar = (ATransAnalAttribRow)anaView[i - 1].Row;
                    WriteStringQuoted(ar.AnalysisTypeCode, false);
                    WriteStringQuoted(ar.AnalysisAttributeValue, (i == maxNumValuesExport));
                }
                else
                {
                    WriteStringQuoted("", false);
                    WriteStringQuoted("", (i == maxNumValuesExport));
                }
            }
        }

        void WriteTransactionSummaryLine(ATransactionSummaryRow transactionSummary)
        {
            if (!FTransactionsOnly)
            {
                WriteStringQuoted("T");
            }

            WriteStringQuoted(transactionSummary.CostCentreCode);
            WriteStringQuoted(transactionSummary.AccountCode);
            WriteStringQuoted(transactionSummary.Narrative);
            WriteStringQuoted(transactionSummary.Reference);
            WriteDate(FDateForSummary);
            decimal amount = (FUseBaseCurrency) ? transactionSummary.AmountInBaseCurrency : transactionSummary.TransactionAmount;

            if (amount > 0)
            {
                WriteCurrency(amount);
                WriteLineCurrency(0);
            }
            else
            {
                WriteCurrency(0);
                WriteLineCurrency(-amount);
            }
        }

        void WriteDelimiter(bool bLineEnd)
        {
            if (bLineEnd)
            {
                FStringWriter.WriteLine();
            }
            else
            {
                FStringWriter.Write(FDelimiter);
            }
        }

        void WriteStringQuoted(String theString, bool bLineEnd)
        {
            FStringWriter.Write(quote);
            FStringWriter.Write(theString);
            FStringWriter.Write(quote);
            WriteDelimiter(bLineEnd);
        }

        void WriteCurrency(decimal currencyField, bool bLineEnd)
        {
            Int64 integerNumber = Convert.ToInt64(currencyField);

            if (Convert.ToDecimal(integerNumber) == currencyField)
            {
                FStringWriter.Write(String.Format("{0:d}", integerNumber));
            }
            else
            {
                FStringWriter.Write(String.Format(FCultureInfo, "{0:###########0.00}", currencyField));
            }

            WriteDelimiter(bLineEnd);
        }

        void WriteGeneralNumber(decimal generalNumberField, bool bLineEnd)
        {
            FStringWriter.Write(String.Format(FCultureInfo, "{0:g}", generalNumberField));
            WriteDelimiter(bLineEnd);
        }

        void WriteDate(DateTime dateField, bool bLineEnd)
        {
            FStringWriter.Write(dateField.ToString(FDateFormatString));
            WriteDelimiter(bLineEnd);
        }

        void WriteStringQuoted(String theString)
        {
            WriteStringQuoted(theString, false);
        }

        void WriteCurrency(decimal currencyField)
        {
            WriteCurrency(currencyField, false);
        }

        void WriteGeneralNumber(decimal generalNumberField)
        {
            WriteGeneralNumber(generalNumberField, false);
        }

        void WriteDate(DateTime dateField)
        {
            WriteDate(dateField, false);
        }

        void WriteLineStringQuoted(String theString)
        {
            WriteStringQuoted(theString, true);
        }

        void WriteLineCurrency(decimal currencyField)
        {
            WriteCurrency(currencyField, true);
        }

        void WriteLineGeneralNumber(decimal generalNumberField)
        {
            WriteGeneralNumber(generalNumberField, true);
        }

        void WriteLineDate(DateTime dateField)
        {
            WriteDate(dateField, true);
        }
    }
    /// <summary>
    /// provides the outer structure for summarizing journals
    /// </summary>
    public class AJournalSummaryRow
    {
        private String transactionCurrency;
        private decimal exchangeRateToBase;
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
        public decimal ExchangeRateToBase {
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

        private decimal transactionAmount;

        /// <summary>
        /// Transaction Amount (may be negative!!!)
        /// </summary>
        public decimal TransactionAmount {
            get
            {
                return transactionAmount;
            }
            set
            {
                transactionAmount = value;
            }
        }
        private decimal amountInBaseCurrency;

        /// <summary>
        /// Amount in Base currency (may be negative!!!)
        /// </summary>
        public decimal AmountInBaseCurrency {
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