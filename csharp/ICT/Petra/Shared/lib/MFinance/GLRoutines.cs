//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in AMainDS.ATransaction.DefaultView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                // recalculate the amount in base currency

                if (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                {
                    r.AmountInBaseCurrency = r.TransactionAmount / ACurrentJournal.ExchangeRateToBase;
                }

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
                throw new Exception(String.Format("Batch {0} Journal {1} has invalid exchange rate to base",
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

                if (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                {
                    r.AmountInBaseCurrency = r.TransactionAmount / ACurrentJournal.ExchangeRateToBase;
                }

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
            string origTransactionFilter = AMainDS.ATransaction.DefaultView.RowFilter;
            string origJournalFilter = AMainDS.AJournal.DefaultView.RowFilter;

            ACurrentBatch.BatchDebitTotal = 0.0m;
            ACurrentBatch.BatchCreditTotal = 0.0m;

            AMainDS.AJournal.DefaultView.RowFilter =
                AJournalTable.GetBatchNumberDBName() + " = " + ACurrentBatch.BatchNumber.ToString();

            foreach (DataRowView journalview in AMainDS.AJournal.DefaultView)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                AMainDS.ATransaction.DefaultView.RowFilter =
                    ATransactionTable.GetBatchNumberDBName() + " = " + journalrow.BatchNumber.ToString() + " and " +
                    ATransactionTable.GetJournalNumberDBName() + " = " + journalrow.JournalNumber.ToString();

                UpdateTotalsOfJournal(ref AMainDS, journalrow);

                ACurrentBatch.BatchDebitTotal += journalrow.JournalDebitTotal;
                ACurrentBatch.BatchCreditTotal += journalrow.JournalCreditTotal;
            }

            AMainDS.ATransaction.DefaultView.RowFilter = origTransactionFilter;
            AMainDS.AJournal.DefaultView.RowFilter = origJournalFilter;
        }
    }
}