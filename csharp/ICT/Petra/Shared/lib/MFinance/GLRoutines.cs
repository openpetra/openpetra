//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// useful routines that are used on both server and client
    /// </summary>
    public class GLRoutines
    {
        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            GLBatchTDSAJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            if ((ACurrentJournal.ExchangeRateToBase == 0.0m)
                && (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString()))
            {
                throw new Exception(String.Format("Batch {0} Journal {1} has invalid exchange rate to base",
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
            }

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            DataView trnsDataView = new DataView(AMainDS.ATransaction);

            trnsDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in trnsDataView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                if (r.DebitCreditIndicator)
                {
                    ACurrentJournal.JournalDebitTotal += r.TransactionAmount;
                    ACurrentJournal.JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    ACurrentJournal.JournalCreditTotal += r.TransactionAmount;
                    ACurrentJournal.JournalCreditTotalBase += r.AmountInBaseCurrency;
                }
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfRecurringJournal(ref GLBatchTDS AMainDS,
            GLBatchTDSARecurringJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            if ((ACurrentJournal.ExchangeRateToBase == 0.0m)
                && (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString()))
            {
                throw new Exception(String.Format("Recurring Batch {0} Journal {1} has invalid exchange rate to base",
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
            }

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in AMainDS.ARecurringTransaction.DefaultView)
            {
                ARecurringTransactionRow r = (ARecurringTransactionRow)v.Row;

                // recalculate the amount in base currency

//                if (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
//                {
//                    r.AmountInBaseCurrency = Divide(r.TransactionAmount, ACurrentJournal.ExchangeRateToBase);
//                }

                if (r.DebitCreditIndicator)
                {
                    ACurrentJournal.JournalDebitTotal += r.TransactionAmount;
                    ACurrentJournal.JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    ACurrentJournal.JournalCreditTotal += r.TransactionAmount;
                    ACurrentJournal.JournalCreditTotalBase += r.AmountInBaseCurrency;
                }
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        public static void UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch)
        {
            ACurrentBatch.BatchDebitTotal = 0.0m;
            ACurrentBatch.BatchCreditTotal = 0.0m;

            DataView jnlDataView = new DataView(AMainDS.AJournal);
            jnlDataView.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalview in jnlDataView)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                UpdateTotalsOfJournal(ref AMainDS, journalrow);

                ACurrentBatch.BatchDebitTotal += journalrow.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += journalrow.JournalCreditTotal;
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        ///   Assumes that all transactions for the journal are already loaded.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfBatchForJournal(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch, GLBatchTDSAJournalRow ACurrentJournal)
        {
            //Subtract existing journal totals amounts
            ACurrentBatch.BatchDebitTotal -= ACurrentJournal.JournalDebitTotal;
            ACurrentBatch.BatchCreditTotal -= ACurrentJournal.JournalCreditTotal;

            UpdateTotalsOfJournal(ref AMainDS, ACurrentJournal);

            //Add updated Journals amounts
            ACurrentBatch.BatchDebitTotal += ACurrentJournal.JournalDebitTotal;
            ACurrentBatch.BatchCreditTotal += ACurrentJournal.JournalCreditTotal;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        public static void UpdateTotalsOfRecurringBatch(ref GLBatchTDS AMainDS,
            ARecurringBatchRow ACurrentBatch)
        {
            string origTransactionFilter = AMainDS.ARecurringTransaction.DefaultView.RowFilter;
            string origJournalFilter = AMainDS.ARecurringJournal.DefaultView.RowFilter;

            ACurrentBatch.BatchDebitTotal = 0.0m;
            ACurrentBatch.BatchCreditTotal = 0.0m;

            AMainDS.ARecurringJournal.DefaultView.RowFilter =
                ARecurringJournalTable.GetBatchNumberDBName() + " = " + ACurrentBatch.BatchNumber.ToString();

            foreach (DataRowView journalview in AMainDS.ARecurringJournal.DefaultView)
            {
                GLBatchTDSARecurringJournalRow journalrow = (GLBatchTDSARecurringJournalRow)journalview.Row;

                AMainDS.ARecurringTransaction.DefaultView.RowFilter =
                    ARecurringTransactionTable.GetBatchNumberDBName() + " = " + journalrow.BatchNumber.ToString() + " and " +
                    ARecurringTransactionTable.GetJournalNumberDBName() + " = " + journalrow.JournalNumber.ToString();

                UpdateTotalsOfRecurringJournal(ref AMainDS, journalrow);

                ACurrentBatch.BatchDebitTotal += journalrow.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += journalrow.JournalCreditTotal;
            }

            AMainDS.ARecurringTransaction.DefaultView.RowFilter = origTransactionFilter;
            AMainDS.ARecurringJournal.DefaultView.RowFilter = origJournalFilter;
        }

        private static int DECIMALS = 10;

        /// <summary>
        /// use this method to calculate the new amount, using an exchange rate.
        /// This will round the result to the defined limit of decimal places
        /// </summary>
        public static decimal Multiply(decimal AAmount, decimal AExchangeRate)
        {
            decimal Result = AAmount * AExchangeRate;

            return Math.Round(Result, DECIMALS);
        }

        /// <summary>
        /// use this method to calculate the new amount, using an exchange rate.
        /// This will round the result to the defined limit of decimal places
        /// </summary>
        public static decimal Divide(decimal AAmount, decimal AExchangeRate)
        {
            decimal Result = AAmount / AExchangeRate;

            return Math.Round(Result, DECIMALS);
        }
    }
}