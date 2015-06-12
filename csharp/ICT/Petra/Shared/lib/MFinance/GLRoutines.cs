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
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
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
        /// <param name="AJournalTotalsUpdated"></param>
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSAJournalRow ACurrentJournal, out bool AJournalTotalsUpdated)
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

            AJournalTotalsUpdated = false;

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

                AJournalTotalsUpdated = true;
            }

            if (ACurrentJournal.LastTransactionNumber != LastTransactionNumber)
            {
                ACurrentJournal.LastTransactionNumber = LastTransactionNumber;

                AJournalTotalsUpdated = true;
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

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            DataView TransDataView = new DataView(AMainDS.ARecurringTransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in TransDataView)
            {
                ARecurringTransactionRow r = (ARecurringTransactionRow)v.Row;

                if (r.DebitCreditIndicator)
                {
                    JournalDebitTotal += r.TransactionAmount;
                    JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    JournalCreditTotal += r.TransactionAmount;
                    JournalCreditTotalBase += r.AmountInBaseCurrency;
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
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// ALSO - since the ExchangeRateToBase field is no longer used here, the code that asserts it to be valid is commented out.
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <param name="AJournalTotalsUpdated"></param>
        public static void UpdateTotalsOfRecurringJournal(ref GLBatchTDS AMainDS,
            ref GLBatchTDSARecurringJournalRow ACurrentJournal, out bool AJournalTotalsUpdated)
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

            AJournalTotalsUpdated = false;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalDebitTotalBaseId]))
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
            }

            if (DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalCreditTotalBaseId]))
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
            }

            DataView TransDataView = new DataView(AMainDS.ARecurringTransaction);

            TransDataView.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in TransDataView)
            {
                ARecurringTransactionRow r = (ARecurringTransactionRow)v.Row;

                if (r.DebitCreditIndicator)
                {
                    JournalDebitTotal += r.TransactionAmount;
                    JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    JournalCreditTotal += r.TransactionAmount;
                    JournalCreditTotalBase += r.AmountInBaseCurrency;
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
                AJournalTotalsUpdated = true;
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

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
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

            AAmountsChanged = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.AJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
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
            else if (ACurrentJournal == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The Journal data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Save original values before updating
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
        ///   Assumes that all transactions for the journal are already loaded.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        /// <param name="ACurrentJournal"></param>
        public static bool UpdateTotalsOfRecurringBatchForJournal(ref GLBatchTDS AMainDS,
            ARecurringBatchRow ACurrentBatch, GLBatchTDSARecurringJournalRow ACurrentJournal)
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
            else if (ACurrentJournal == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Journal data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Save current values
            decimal JournalDebitTotal = ACurrentJournal.JournalDebitTotal;
            decimal JournalCreditTotal = ACurrentJournal.JournalCreditTotal;

            bool JournalUpdated;

            UpdateTotalsOfRecurringJournal(ref AMainDS, ref ACurrentJournal, out JournalUpdated);

            if (JournalUpdated)
            {
                //Subtract old amounts
                ACurrentBatch.BatchDebitTotal -= JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal -= JournalCreditTotal;
                //Add updated Journals amounts
                ACurrentBatch.BatchDebitTotal += ACurrentJournal.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += ACurrentJournal.JournalCreditTotal;
            }

            return JournalUpdated;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        public static void UpdateTotalsOfRecurringBatch(ref GLBatchTDS AMainDS,
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

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.ARecurringJournal);
            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)journalView.Row;

                UpdateTotalsOfRecurringJournal(ref AMainDS, ref journalRow);

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