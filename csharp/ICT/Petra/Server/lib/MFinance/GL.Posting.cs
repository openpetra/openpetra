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
using System.Data;
using System.Data.Odbc;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// provides methods for posting a batch
    /// </summary>
    public class TGLPosting
    {
        /// <summary>
        /// creates the rows for the whole current year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="AVerificationResult"></param>
        /// <param name="ATransaction"></param>
        /// <returns>the glm sequence</returns>
        private static Int32 CreateGLMYear(Int32 ALedgerNumber,
            string AAccountCode,
            string ACostCentreCode,
            TVerificationResultCollection AVerificationResult,
            TDBTransaction ATransaction)
        {
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);

            if (LedgerTable.Count != 1)
            {
                throw new Exception("TFinancialYearWebConnector.CreateGLMYear: Cannot find ledger " + ALedgerNumber.ToString());
            }

            ALedgerRow Ledger = LedgerTable[0];

            if (AGeneralLedgerMasterAccess.Exists(ALedgerNumber, Ledger.CurrentFinancialYear, AAccountCode, ACostCentreCode, ATransaction))
            {
                throw new Exception("TFinancialYearWebConnector.CreateGLMYear: There is already a GLM Master record for " +
                    ALedgerNumber.ToString() + "/" +
                    Ledger.CurrentFinancialYear.ToString() + "/" +
                    AAccountCode + "/" + ACostCentreCode);
            }

            AGeneralLedgerMasterTable GLMTable = new AGeneralLedgerMasterTable();
            AGeneralLedgerMasterRow GLMRow = GLMTable.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = Ledger.CurrentFinancialYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            GLMTable.Rows.Add(GLMRow);
            AGeneralLedgerMasterAccess.SubmitChanges(GLMTable, ATransaction, out AVerificationResult);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodTable PeriodTable = new AGeneralLedgerMasterPeriodTable();
                AGeneralLedgerMasterPeriodRow PeriodRow = PeriodTable.NewRowTyped();
                PeriodRow.GlmSequence = GLMTable[0].GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                PeriodTable.Rows.Add(PeriodRow);
                AGeneralLedgerMasterPeriodAccess.SubmitChanges(PeriodTable, ATransaction, out AVerificationResult);
            }

            return GLMTable[0].GlmSequence;
        }

        /// <summary>
        /// this method loads the batch and the journals and the transactions, also the ledger
        /// runs validations on batch, journals and transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool ValidateBatchAndTransactions(out GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            ADataSet = new GLBatchTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!ABatchAccess.Exists(ALedgerNumber, ABatchNumber, Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        Catalog.GetString("The batch does not exist at all."),
                        TResultSeverity.Resv_Critical));
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }

            ABatchTable BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, Transaction);
            ADataSet.Merge(BatchTable);

            ABatchRow Batch = ADataSet.ABatch[0];

            if ((Batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED) || (Batch.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
            }

            if (Batch.BatchCreditTotal != Batch.BatchDebitTotal)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It does not balance: Debit is {0}, Credit is {1}"), Batch.BatchDebitTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }
            else if (Batch.BatchCreditTotal == 0)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        Catalog.GetString("It has no monetary value. Please cancel it or add meaningful transactions."),
                        TResultSeverity.Resv_Critical));
            }
            else if ((Batch.BatchControlTotal != 0) && (Batch.BatchControlTotal != Batch.BatchCreditTotal))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The control total {0} does not fit the Credit/Debit Total {1}."), Batch.BatchControlTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }

            Int32 DateEffectivePeriodNumber;

            if (!TFinancialYear.IsValidPeriod(Batch.LedgerNumber, Batch.DateEffective, out DateEffectivePeriodNumber, Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                            Batch.DateEffective),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                // just make sure that the correct BatchPeriod is used
                Batch.BatchPeriod = DateEffectivePeriodNumber;

                // TODO: BatchYear?
            }

            AJournalTable JournalTable = AJournalAccess.LoadViaABatch(ALedgerNumber, ABatchNumber, Transaction);
            ADataSet.Merge(JournalTable);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                journal.DateEffective = Batch.DateEffective;
                journal.JournalPeriod = Batch.BatchPeriod;

                // TODO: JournalYear?

                if (journal.JournalCreditTotal != journal.JournalDebitTotal)
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The journal {0} does not balance: Debit is {1}, Credit is {2}"), journal.JournalNumber,
                                journal.JournalDebitTotal, journal.JournalCreditTotal),
                            TResultSeverity.Resv_Critical));
                }

                ATransactionTable TransactionTable = ATransactionAccess.LoadViaAJournal(journal.LedgerNumber,
                    journal.BatchNumber,
                    journal.JournalNumber,
                    Transaction);

                foreach (ATransactionRow transaction in TransactionTable.Rows)
                {
                    // check that transactions on foreign currency accounts are using the correct currency
                    // (fx reval transactions are an exception because they are posted in base currency)
                    if (transaction.Reference != MFinanceConstants.TRANSACTION_FX_REVAL)
                    {
                        string stmt = "SELECT " + AAccountTable.GetForeignCurrencyCodeDBName() +
                                      " FROM " + TTypedDataTable.GetTableNameSQL(AAccountTable.TableId) +
                                      " WHERE " + AAccountTable.GetLedgerNumberDBName() + " = ? AND " +
                                      AAccountTable.GetAccountCodeDBName() + " = ? AND " +
                                      AAccountTable.GetForeignCurrencyFlagDBName() + " = true AND " +
                                      AAccountTable.GetForeignCurrencyCodeDBName() + " <> ?";

                        OdbcParameter paramLedgerNumber = TTypedDataTable.CreateOdbcParameter(AAccountTable.TableId,
                            AAccountTable.ColumnLedgerNumberId);
                        paramLedgerNumber.Value = transaction.LedgerNumber;
                        OdbcParameter paramAccountCode = TTypedDataTable.CreateOdbcParameter(AAccountTable.TableId, AAccountTable.ColumnAccountCodeId);
                        paramAccountCode.Value = transaction.AccountCode;
                        OdbcParameter paramForeignCurrencyCode = TTypedDataTable.CreateOdbcParameter(AAccountTable.TableId,
                            AAccountTable.ColumnForeignCurrencyCodeId);
                        paramForeignCurrencyCode.Value = journal.TransactionCurrency;
                        OdbcParameter[] ParametersArray = new OdbcParameter[] {
                            paramLedgerNumber,
                            paramAccountCode,
                            paramForeignCurrencyCode
                        };
                        DataTable accountForeignCurrency = DBAccess.GDBAccessObj.SelectDT(stmt, "temp", Transaction, ParametersArray);

                        foreach (DataRow account in accountForeignCurrency.Rows)
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Transaction {0} in Journal {1} with currency {2} does not fit the foreign currency {3} of account {4}."),
                                        transaction.TransactionNumber, transaction.JournalNumber, journal.TransactionCurrency, account[0],
                                        transaction.AccountCode),
                                    TResultSeverity.Resv_Critical));
                        }
                    }

                    if ((transaction.AmountInBaseCurrency == 0) && (transaction.TransactionAmount != 0))
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("Transaction {0} in Journal {1} has invalid base transaction amount of 0."),
                                    transaction.TransactionNumber, transaction.JournalNumber),
                                TResultSeverity.Resv_Critical));
                    }
                }

                ADataSet.Merge(TransactionTable);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return !AVerifications.HasCriticalError();
        }

        /// <summary>
        /// mark each journal, each transaction as being posted;
        /// add sums for costcentre/account combinations
        /// </summary>
        /// <param name="MainDS">only contains the batch to post, and its journals and transactions</param>
        private static void MarkAsPostedAndCollectData(ref GLBatchTDS MainDS)
        {
            foreach (AJournalRow journal in MainDS.AJournal.Rows)
            {
                DataView myView = new DataView(MainDS.ATransaction);
                myView.Sort = ATransactionTable.GetJournalNumberDBName();

                foreach (DataRowView transactionview in myView.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;
                    transaction.TransactionStatus = true;

                    // TODO
                }

                journal.JournalStatus = MFinanceConstants.BATCH_POSTED;
            }
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            GLBatchTDS MainDS;

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            // also get the data from the database into the MainDS
            if (!ValidateBatchAndTransactions(out MainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            // post each journal, each transaction; add sums for costcentre/account combinations
            MarkAsPostedAndCollectData(ref MainDS);

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // TODO: what about international currency etc?
            // TODO: CalculateTrees
            // TODO: SummarizeData (optionally using stored procedures?)
            // TODO: WriteData

            return false;
        }
    }
}