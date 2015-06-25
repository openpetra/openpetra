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
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// useful routines that are used on both server and client
    /// </summary>
    public class GLRoutines
    {
        private const int DECIMALS = 10;

        /// <summary>
        /// Update the totals for the current journal (no exchange rate calculation at this point)
        ///   and set LastTransactionNumber
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <returns>false if no change to journal totals</returns>
        public static bool UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSAJournalRow ACurrentJournal)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentJournal == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Journal row does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            int LastTransactionNumber = 0;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (ACurrentJournal.IsJournalDebitTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalDebitTotalBaseId])
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
                RetVal = true;
            }

            if (ACurrentJournal.IsJournalCreditTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalCreditTotalBaseId])
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
                RetVal = true;
            }

            DataView TransDataView = new DataView(AMainDS.ATransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            if (TransDataView.Count == 0)
            {
                //do not update totals as nor transactions loaded as yet so no need to update journal total
                return RetVal;
            }

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
                RetVal = true;
            }

            if (ACurrentJournal.LastTransactionNumber != LastTransactionNumber)
            {
                ACurrentJournal.LastTransactionNumber = LastTransactionNumber;
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// ALSO - since the ExchangeRateToBase field is no longer used here, the code that asserts it to be valid is commented out.
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <returns>false if no change to journal totals</returns>
        public static bool UpdateTotalsOfRecurringJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSARecurringJournalRow ACurrentJournal)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentJournal == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Journal row does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            int LastTransactionNumber = 0;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (ACurrentJournal.IsJournalDebitTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalDebitTotalBaseId])
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
                RetVal = true;
            }

            if (ACurrentJournal.IsJournalCreditTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalCreditTotalBaseId])
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
                RetVal = true;
            }

            DataView TransDataView = new DataView(AMainDS.ARecurringTransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            if (TransDataView.Count == 0)
            {
                return RetVal;
            }

            TransDataView.Sort = string.Format("{0} DESC",
                ARecurringTransactionTable.GetTransactionNumberDBName());

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView drv in TransDataView)
            {
                ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

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
                RetVal = true;
            }

            if (ACurrentJournal.LastTransactionNumber != LastTransactionNumber)
            {
                ACurrentJournal.LastTransactionNumber = LastTransactionNumber;
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatch == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)journalView.Row;

                if (UpdateTotalsOfJournal(ref AMainDS, ref journalRow))
                {
                    RetVal = true;
                }

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatch.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatch.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatch.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatch.BatchCreditTotal = BatchCreditTotal;
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateTotalsOfRecurringBatch(ref GLBatchTDS AMainDS,
            ARecurringBatchRow ACurrentBatch)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatch == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RetVal = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.ARecurringJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)journalView.Row;

                if (UpdateTotalsOfRecurringJournal(ref AMainDS, ref journalRow))
                {
                    RetVal = true;
                }

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatch.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatch.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatch.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatch.BatchCreditTotal = BatchCreditTotal;
                RetVal = true;
            }

            return RetVal;
        }

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