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
        /// Update the specified Batch's LastJournal number. Assumes all necessary data is loaded for Batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatchRow"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateBatchLastJournal(ref GLBatchTDS AMainDS,
            ref ABatchRow ACurrentBatchRow)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                TLogging.Log(String.Format("Function:{0} - Tried to update totals for non-Unposted Batch:{1}",
                        Utilities.GetMethodName(true),
                        ACurrentBatchRow.BatchNumber));
                return false;
            }

            #endregion Validate Arguments

            bool RowUpdated = false;
            int ActualLastJournalNumber = 0;

            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatchRow.BatchNumber);

            //Highest Journal number first
            JournalDV.Sort = String.Format("{0} DESC",
                AJournalTable.GetJournalNumberDBName());

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)journalView.Row;

                //Run once only
                if (ActualLastJournalNumber == 0)
                {
                    ActualLastJournalNumber = journalRow.JournalNumber;
                }

                if (UpdateJournalLastTransaction(ref AMainDS, ref journalRow))
                {
                    RowUpdated = true;
                }
            }

            if (ACurrentBatchRow.LastJournal != ActualLastJournalNumber)
            {
                ACurrentBatchRow.LastJournal = ActualLastJournalNumber;
                RowUpdated = true;
            }

            return RowUpdated;
        }

        /// <summary>
        /// Update the specified Journal's LastTransaction number. Assumes all necessary data is loaded for Journal
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <returns>false if no change to journal totals</returns>
        public static bool UpdateJournalLastTransaction(ref GLBatchTDS AMainDS,
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
            else if (ACurrentJournal.JournalStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                TLogging.Log(String.Format("Function:{0} - Tried to update totals for non-Unposted Batch:{1} and Journal:{2}",
                        Utilities.GetMethodName(true),
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
                return false;
            }

            #endregion Validate Arguments

            bool RowUpdated = false;

            int ActualLastTransNumber = 0;

            DataView TransDV = new DataView(AMainDS.ATransaction);
            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            TransDV.Sort = String.Format("{0} DESC",
                ATransactionTable.GetTransactionNumberDBName());

            foreach (DataRowView drv in TransDV)
            {
                ATransactionRow transRow = (ATransactionRow)drv.Row;

                //Run once only
                ActualLastTransNumber = transRow.TransactionNumber;
                break;
            }

            if (ACurrentJournal.LastTransactionNumber != ActualLastTransNumber)
            {
                ACurrentJournal.LastTransactionNumber = ActualLastTransNumber;
                RowUpdated = true;
            }

            return RowUpdated;
        }

        /// <summary>
        /// Update the specified Recurring Batch's LastJournal number. Assumes all necessary data is loaded for Batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatchRow"></param>
        /// <param name="AIncludeJournals"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateRecurringBatchLastJournal(ref GLBatchTDS AMainDS,
            ref ARecurringBatchRow ACurrentBatchRow, Boolean AIncludeJournals = false)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool RowUpdated = false;
            int ActualLastJournalNumber = 0;

            DataView JournalDV = new DataView(AMainDS.ARecurringJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatchRow.BatchNumber);

            //Highest Journal number first
            JournalDV.Sort = String.Format("{0} DESC",
                ARecurringJournalTable.GetJournalNumberDBName());

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)journalView.Row;

                //Run once only
                if (ActualLastJournalNumber == 0)
                {
                    ActualLastJournalNumber = journalRow.JournalNumber;
                }

                if (AIncludeJournals && UpdateRecurringJournalLastTransaction(ref AMainDS, ref journalRow))
                {
                    RowUpdated = true;
                }
            }

            if (ACurrentBatchRow.LastJournal != ActualLastJournalNumber)
            {
                ACurrentBatchRow.LastJournal = ActualLastJournalNumber;
                RowUpdated = true;
            }

            return RowUpdated;
        }

        /// <summary>
        /// Update the specified recurring Journal's LastTransaction number. Assumes all necessary data is loaded for Journal
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        /// <returns>false if no change to journal totals</returns>
        public static bool UpdateRecurringJournalLastTransaction(ref GLBatchTDS AMainDS,
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

            bool RowUpdated = false;

            int ActualLastTransNumber = 0;

            DataView TransDV = new DataView(AMainDS.ARecurringTransaction);

            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            TransDV.Sort = String.Format("{0} DESC",
                ARecurringTransactionTable.GetTransactionNumberDBName());

            foreach (DataRowView drv in TransDV)
            {
                ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

                //Run once only
                ActualLastTransNumber = transRow.TransactionNumber;
                break;
            }

            if (ACurrentJournal.LastTransactionNumber != ActualLastTransNumber)
            {
                ACurrentJournal.LastTransactionNumber = ActualLastTransNumber;
                RowUpdated = true;
            }

            return RowUpdated;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        ///   This assumes that all relevant data has already been loaded
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatchRow"></param>
        /// <param name="ACurrentJournalNumber"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateBatchTotals(ref GLBatchTDS AMainDS,
            ref ABatchRow ACurrentBatchRow, Int32 ACurrentJournalNumber = 0)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                TLogging.Log(String.Format("Function:{0} - Tried to update totals for non-Unposted Batch:{1}",
                        Utilities.GetMethodName(true),
                        ACurrentBatchRow.BatchNumber));
                return false;
            }

            #endregion Validate Arguments

            bool AmountsUpdated = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatchRow.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)journalView.Row;

                if (((ACurrentJournalNumber > 0) && (ACurrentJournalNumber == journalRow.JournalNumber))
                    || (ACurrentJournalNumber == 0))
                {
                    if ((UpdateJournalTotals(ref AMainDS, ref journalRow)))
                    {
                        AmountsUpdated = true;
                    }
                }

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatchRow.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatchRow.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatchRow.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatchRow.BatchCreditTotal = BatchCreditTotal;
                AmountsUpdated = true;
            }

            return AmountsUpdated;
        }

        private static bool UpdateJournalTotals(ref GLBatchTDS AMainDS,
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
            else if (ACurrentJournal.JournalStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                TLogging.Log(String.Format("Function:{0} - Tried to update totals for non-Unposted Batch:{1} and Journal:{2}",
                        Utilities.GetMethodName(true),
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
                return false;
            }

            #endregion Validate Arguments

            bool AmountsUpdated = false;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (ACurrentJournal.IsJournalDebitTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalDebitTotalBaseId])
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
                AmountsUpdated = true;
            }

            if (ACurrentJournal.IsJournalCreditTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSAJournalTable.ColumnJournalCreditTotalBaseId])
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
                AmountsUpdated = true;
            }

            DataView TransDV = new DataView(AMainDS.ATransaction);
            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            if ((TransDV.Count == 0) && (ACurrentJournal.LastTransactionNumber > 0))
            {
                //do not update totals as no transactions loaded as yet so no need to update journal total
                return AmountsUpdated;
            }

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView drv in TransDV)
            {
                ATransactionRow transRow = (ATransactionRow)drv.Row;

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
                AmountsUpdated = true;
            }

            return AmountsUpdated;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatchRow"></param>
        /// <param name="ACurrentJournalNumber"></param>
        /// <returns>false if no change to batch totals</returns>
        public static bool UpdateRecurringBatchTotals(ref GLBatchTDS AMainDS,
            ref ARecurringBatchRow ACurrentBatchRow, Int32 ACurrentJournalNumber = 0)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatchRow == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch data row is null!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            bool AmountsUpdated = false;

            decimal BatchDebitTotal = 0.0m;
            decimal BatchCreditTotal = 0.0m;

            DataView JournalDV = new DataView(AMainDS.ARecurringJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ACurrentBatchRow.BatchNumber);

            foreach (DataRowView journalView in JournalDV)
            {
                GLBatchTDSARecurringJournalRow journalRow = (GLBatchTDSARecurringJournalRow)journalView.Row;

                if (((ACurrentJournalNumber > 0) && (ACurrentJournalNumber == journalRow.JournalNumber))
                    || (ACurrentJournalNumber == 0))
                {
                    if ((UpdateRecurringJournalTotals(ref AMainDS, ref journalRow)))
                    {
                        AmountsUpdated = true;
                    }
                }

                BatchDebitTotal += journalRow.JournalDebitTotal;
                BatchCreditTotal += journalRow.JournalCreditTotal;
            }

            if ((ACurrentBatchRow.BatchDebitTotal != BatchDebitTotal)
                || (ACurrentBatchRow.BatchCreditTotal != BatchCreditTotal))
            {
                ACurrentBatchRow.BatchDebitTotal = BatchDebitTotal;
                ACurrentBatchRow.BatchCreditTotal = BatchCreditTotal;
                AmountsUpdated = true;
            }

            return AmountsUpdated;
        }

        private static bool UpdateRecurringJournalTotals(ref GLBatchTDS AMainDS,
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

            bool AmountsUpdated = false;

            decimal JournalDebitTotal = 0.0M;
            decimal JournalDebitTotalBase = 0.0M;
            decimal JournalCreditTotal = 0.0M;
            decimal JournalCreditTotalBase = 0.0M;

            if (ACurrentJournal.IsJournalDebitTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalDebitTotalBaseId])
            {
                ACurrentJournal.JournalDebitTotalBase = 0;
                AmountsUpdated = true;
            }

            if (ACurrentJournal.IsJournalCreditTotalBaseNull()) //DBNull.Value.Equals(ACurrentJournal[GLBatchTDSARecurringJournalTable.ColumnJournalCreditTotalBaseId])
            {
                ACurrentJournal.JournalCreditTotalBase = 0;
                AmountsUpdated = true;
            }

            DataView TransDV = new DataView(AMainDS.ARecurringTransaction);
            TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            if ((TransDV.Count == 0) && (ACurrentJournal.LastTransactionNumber > 0))
            {
                //do not update totals as no transactions loaded as yet so no need to update journal total
                return AmountsUpdated;
            }

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView drv in TransDV)
            {
                ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

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
                AmountsUpdated = true;
            }

            return AmountsUpdated;
        }

        /// <summary>
        /// Return a GL Dataset with data only for the specified batch, other data stripped
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public static GLBatchTDS SingleBatchOnlyDataSet(ref GLBatchTDS AMainDS, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            return ReduceGLDataSet(ref AMainDS, ALedgerNumber, ABatchNumber, true);
        }

        /// <summary>
        /// Return a GL Dataset with data for the specified batch removed
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        public static GLBatchTDS RemoveSingleBatchFromDataSet(ref GLBatchTDS AMainDS, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            return ReduceGLDataSet(ref AMainDS, ALedgerNumber, ABatchNumber, false);
        }

        private static GLBatchTDS ReduceGLDataSet(ref GLBatchTDS AMainDS, Int32 ALedgerNumber, Int32 ABatchNumber, bool AKeepThisBatchOnly = true)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS ReducedDS = new GLBatchTDS();
            ReducedDS.Merge(AMainDS);

            DataView BatchDV = new DataView(ReducedDS.ABatch);
            DataView JournalDV = new DataView(ReducedDS.AJournal);
            DataView TransactionDV = new DataView(ReducedDS.ATransaction);
            DataView AnalysisAttribDV = new DataView(ReducedDS.ATransAnalAttrib);
            //Recurring
            DataView RecurringBatchDV = new DataView(ReducedDS.ARecurringBatch);
            DataView RecurringJournalDV = new DataView(ReducedDS.ARecurringJournal);
            DataView RecurringTransactionDV = new DataView(ReducedDS.ARecurringTransaction);
            DataView RecurringAnalysisAttribDV = new DataView(ReducedDS.ARecurringTransAnalAttrib);

            string CommonRowFilter = StandardRowFilterByLedgerAndBatch(ALedgerNumber, ABatchNumber, !AKeepThisBatchOnly);

            AnalysisAttribDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in AnalysisAttribDV)
            {
                drv.Delete();
            }

            TransactionDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in TransactionDV)
            {
                drv.Delete();
            }

            JournalDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in JournalDV)
            {
                drv.Delete();
            }

            BatchDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in BatchDV)
            {
                drv.Delete();
            }

            //Recurring
            RecurringAnalysisAttribDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in RecurringAnalysisAttribDV)
            {
                drv.Delete();
            }

            RecurringTransactionDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in RecurringTransactionDV)
            {
                drv.Delete();
            }

            RecurringJournalDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in RecurringJournalDV)
            {
                drv.Delete();
            }

            RecurringBatchDV.RowFilter = CommonRowFilter;

            foreach (DataRowView drv in RecurringBatchDV)
            {
                drv.Delete();
            }

            ReducedDS.AcceptChanges();

            return ReducedDS;
        }

        private static string StandardRowFilterByLedgerAndBatch(Int32 ALedgerNumber, Int32 ABatchNumber, Boolean AThisBatchExclusively = true)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            string LedgerFieldName = ABatchTable.GetLedgerNumberDBName();
            string BatchFieldName = ABatchTable.GetBatchNumberDBName();

            string RowFilter = String.Format("{0}={1} And {2}" + (AThisBatchExclusively ? "=" : "<>") + "{3}",
                LedgerFieldName,
                ALedgerNumber,
                BatchFieldName,
                ABatchNumber);

            return RowFilter;
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