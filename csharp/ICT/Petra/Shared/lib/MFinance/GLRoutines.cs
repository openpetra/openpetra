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
        /// Update the totals for the current journal (no exchange rate calculation at this point)
        ///   and set LastTransactionNumber
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSAJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            int LastTransactionNumber = 0;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalDebitTotalBaseId]))
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
            }

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalCreditTotalBaseId]))
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
            }

            DataView TransDataView = new DataView(AMainDS.ATransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            TransDataView.Sort = string.Format("{0} DESC",
                ATransactionTable.GetTransactionNumberDBName());

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView drv in TransDataView)
            {
                ATransactionRow transRow = (ATransactionRow)drv.Row;

                //on first recursion
                if (transRow.TransactionNumber > LastTransactionNumber)
                {
                    //Sort order will ensure this is the highest trans number on first pass
                    LastTransactionNumber = transRow.TransactionNumber;
                }

                if (transRow.DebitCreditIndicator)
                {
                    JournalDebitTotal += transRow.TransactionAmount;
                    JournalDebitTotalBase += transRow.AmountInBaseCurrency;
                }
                else
                {
                    JournalCreditTotal += transRow.TransactionAmount;
                    JournalCreditTotalBase += transRow.AmountInBaseCurrency;
                }
            }

            if ((ACurrentJournal.JournalDebitTotal != JournalDebitTotal)
                || (ACurrentJournal.JournalDebitTotalBase != JournalDebitTotalBase)
                || (ACurrentJournal.JournalCreditTotal != JournalCreditTotal)
                || (ACurrentJournal.JournalCreditTotalBase != JournalCreditTotalBase))
            {
                ACurrentJournal.JournalDebitTotal = JournalDebitTotal;
                ACurrentJournal.JournalDebitTotalBase = JournalDebitTotalBase;
                ACurrentJournal.JournalCreditTotal = JournalCreditTotal;
                ACurrentJournal.JournalCreditTotalBase = JournalCreditTotalBase;
            }

            if (ACurrentJournal.LastTransactionNumber != LastTransactionNumber)
            {
                ACurrentJournal.LastTransactionNumber = LastTransactionNumber;
            }
        }

        /// <summary>
        /// Update the totals for the current journal (no exchange rate calculation at this point)
        ///   and set LastTransactionNumber
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <param name="AJournalUpdated"></param>
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSAJournalRow ACurrentJournal, out bool AJournalUpdated)
        {
            AJournalUpdated = false;

            if (ACurrentJournal == null)
            {
                return;
            }

            int LastTransactionNumber = 0;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalDebitTotalBaseId]))
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
            }

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalCreditTotalBaseId]))
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
            }

            DataView TransDataView = new DataView(AMainDS.ATransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            TransDataView.Sort = string.Format("{0} DESC",
                ATransactionTable.GetTransactionNumberDBName());

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView drv in TransDataView)
            {
                ATransactionRow transRow = (ATransactionRow)drv.Row;

                //on first recursion
                if (transRow.TransactionNumber > LastTransactionNumber)
                {
                    //Sort order will ensure this is the highest trans number on first pass
                    LastTransactionNumber = transRow.TransactionNumber;
                }

                if (transRow.DebitCreditIndicator)
                {
                    JournalDebitTotal += transRow.TransactionAmount;
                    JournalDebitTotalBase += transRow.AmountInBaseCurrency;
                }
                else
                {
                    JournalCreditTotal += transRow.TransactionAmount;
                    JournalCreditTotalBase += transRow.AmountInBaseCurrency;
                }
            }

            if ((ACurrentJournal.JournalDebitTotal != JournalDebitTotal)
                || (ACurrentJournal.JournalDebitTotalBase != JournalDebitTotalBase)
                || (ACurrentJournal.JournalCreditTotal != JournalCreditTotal)
                || (ACurrentJournal.JournalCreditTotalBase != JournalCreditTotalBase))
            {
                ACurrentJournal.JournalDebitTotal = JournalDebitTotal;
                ACurrentJournal.JournalDebitTotalBase = JournalDebitTotalBase;
                ACurrentJournal.JournalCreditTotal = JournalCreditTotal;
                ACurrentJournal.JournalCreditTotalBase = JournalCreditTotalBase;

                AJournalUpdated = true;
            }

            if (ACurrentJournal.LastTransactionNumber != LastTransactionNumber)
            {
                ACurrentJournal.LastTransactionNumber = LastTransactionNumber;

                AJournalUpdated = true;
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// ALSO - since the ExchangeRateToBase field is no longer used here, the code that asserts it to be valid is commented out.
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        public static void UpdateTotalsOfRecurringJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSARecurringJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            /* // Since I'm not using ExchangeRateToBase, I don't need to check that it's valid:
             *
             * if ((ACurrentJournal.ExchangeRateToBase == 0.0m)
             *  && (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString()))
             * {
             *  throw new Exception(String.Format("Recurring Batch {0} Journal {1} has invalid exchange rate to base",
             *          ACurrentJournal.BatchNumber,
             *          ACurrentJournal.JournalNumber));
             * }
             */

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            DataView trnsDataView = new DataView(AMainDS.ARecurringTransaction);

            trnsDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in trnsDataView)
            {
                ARecurringTransactionRow r = (ARecurringTransactionRow)v.Row;

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
            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView jnlDataView = new DataView(AMainDS.AJournal);

            jnlDataView.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in jnlDataView)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)journalView.Row;

                UpdateTotalsOfJournal(ref AMainDS, ref journalRow);

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatch.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatch.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatch.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatch.BatchCreditTotal = BatchCreditTotal;
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        /// <param name="AAmountsChanged"></param>
        public static void UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch, out bool AAmountsChanged)
        {
            AAmountsChanged = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView jnlDataView = new DataView(AMainDS.AJournal);
            jnlDataView.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in jnlDataView)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)journalView.Row;

                bool journalUpdated;
                UpdateTotalsOfJournal(ref AMainDS, ref journalRow, out journalUpdated);

                if (journalUpdated && !AAmountsChanged)
                {
                    AAmountsChanged = true;
                }

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatch.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatch.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatch.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatch.BatchCreditTotal = BatchCreditTotal;
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
            //Save current values
            decimal JournalDebitTotal = ACurrentJournal.JournalDebitTotal;
            decimal JournalCreditTotal = ACurrentJournal.JournalCreditTotal;

            bool JournalUpdated;

            UpdateTotalsOfJournal(ref AMainDS, ref ACurrentJournal, out JournalUpdated);

            if (JournalUpdated)
            {
                //Subtract old amounts
                ACurrentBatch.BatchDebitTotal -= JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal -= JournalCreditTotal;
                //Add updated Journals amounts
                ACurrentBatch.BatchDebitTotal += ACurrentJournal.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += ACurrentJournal.JournalCreditTotal;
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        public static void UpdateTotalsOfRecurringBatch(ref GLBatchTDS AMainDS,
            ARecurringBatchRow ACurrentBatch)
        {
            ACurrentBatch.BatchDebitTotal = 0.0m;
            ACurrentBatch.BatchCreditTotal = 0.0m;

            DataView jnlDataView = new DataView(AMainDS.ARecurringJournal);
            jnlDataView.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in jnlDataView)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)journalView.Row;

                UpdateTotalsOfRecurringJournal(ref AMainDS, ref journalRow);

                ACurrentBatch.BatchDebitTotal += journalRow.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += journalRow.JournalCreditTotal;
            }
        }

        private const int DECIMALS = 10;

        /// <summary>
        /// use this method to calculate the new amount, using an exchange rate.
        /// This will round the result to the defined limit of decimal places
        /// </summary>
        public static decimal Multiply(decimal AAmount, decimal AExchangeRate, int ADecimals = DECIMALS)
        {
            decimal Result = AAmount * AExchangeRate;

            return Math.Round(Result, ADecimals);
        }

        /// <summary>
        /// use this method to calculate the new amount, using an exchange rate.
        /// This will round the result to the defined limit of decimal places
        /// </summary>
        public static decimal Divide(decimal AAmount, decimal AExchangeRate, int ADecimals = DECIMALS)
        {
            if (AExchangeRate == 0.0m)
            {
                return 0.0m;
            }

            decimal Result = AAmount / AExchangeRate;

            return Math.Round(Result, ADecimals);
        }
    }
}