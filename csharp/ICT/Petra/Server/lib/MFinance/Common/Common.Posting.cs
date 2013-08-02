//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh, christophert
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
using System.Data.Odbc;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// provides methods for posting a batch
    /// </summary>
    public class TGLPosting
    {
        private const int POSTING_LOGLEVEL = 1;

        /// <summary>
        /// creates the rows for the whole current year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns>The new glm sequence, which is negative until SubmitChanges</returns>
        private static Int32 CreateGLMYear(
            ref GLPostingTDS AMainDS,
            Int32 ALedgerNumber,
            string AAccountCode,
            string ACostCentreCode)
        {
            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = Ledger.CurrentFinancialYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// creates the rows for the specified year in AGeneralLedgerMaster and AGeneralLedgerMasterPeriod for an Account/CostCentre combination
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns> The GLM Sequence</returns>
        public static Int32 CreateGLMYear(
            ref GLPostingTDS AMainDS,
            Int32 ALedgerNumber,
            int AYear,
            string AAccountCode,
            string ACostCentreCode)
        {
            ALedgerRow Ledger = AMainDS.ALedger[0];

            AGeneralLedgerMasterRow GLMRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = (AMainDS.AGeneralLedgerMaster.Rows.Count * -1) - 1;
            GLMRow.LedgerNumber = ALedgerNumber;
            GLMRow.Year = AYear;
            GLMRow.AccountCode = AAccountCode;
            GLMRow.CostCentreCode = ACostCentreCode;

            AMainDS.AGeneralLedgerMaster.Rows.Add(GLMRow);

            for (int PeriodCount = 1; PeriodCount < Ledger.NumberOfAccountingPeriods + Ledger.NumberFwdPostingPeriods + 1; PeriodCount++)
            {
                AGeneralLedgerMasterPeriodRow PeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                PeriodRow.GlmSequence = GLMRow.GlmSequence;
                PeriodRow.PeriodNumber = PeriodCount;
                AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(PeriodRow);
            }

            return GLMRow.GlmSequence;
        }

        /// <summary>
        /// load the batch and all associated tables into the typed dataset
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns>false if batch does not exist at all</returns>
        private static bool LoadData(out GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            ADataSet = new GLBatchTDS();

            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            if (!ABatchAccess.Exists(ALedgerNumber, ABatchNumber, Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot access Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        Catalog.GetString("Batch not found."),
                        TResultSeverity.Resv_Critical));

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                return false;
            }

            ALedgerAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, Transaction);

            ABatchAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            AJournalAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            ATransactionAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            ATransAnalAttribAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return true;
        }

        /// <summary>
        /// load the tables that are needed for posting
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns>false if batch does not exist at all</returns>
        private static bool LoadDataForPosting(out GLPostingTDS ADataSet,
            Int32 ALedgerNumber)
        {
            ADataSet = new GLPostingTDS();

            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            ALedgerAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, Transaction);

            // load all accounts of ledger, because we need them later for the account hierarchy tree for summarisation
            AAccountAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            // TODO: use cached table?
            AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(ADataSet, ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, Transaction);

            // TODO: use cached table?
            ACostCentreAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            AAnalysisTypeAccess.LoadAll(ADataSet, Transaction);
            AFreeformAnalysisAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);
            AAnalysisAttributeAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return true;
        }

        /// <summary>
        /// Load all GLM and GLMPeriod records for the batch period and the following periods, since that will avoid loading them one by one during submitchanges.
        /// this is called after ValidateBatchAndTransactions, because the BatchYear and BatchPeriod are validated and recalculated there
        /// </summary>
        private static void LoadGLMData(ref GLPostingTDS ADataSet, Int32 ALedgerNumber, ABatchRow ABatchToPost)
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            AGeneralLedgerMasterRow GLMTemplateRow = ADataSet.AGeneralLedgerMaster.NewRowTyped(false);

            GLMTemplateRow.LedgerNumber = ALedgerNumber;
            GLMTemplateRow.Year = ABatchToPost.BatchYear;
            AGeneralLedgerMasterAccess.LoadUsingTemplate(ADataSet, GLMTemplateRow, Transaction);

            string query = "SELECT PUB_a_general_ledger_master_period.* " +
                           "FROM PUB_a_general_ledger_master, PUB_a_general_ledger_master_period " +
                           "WHERE PUB_a_general_ledger_master.a_ledger_number_i = ? " +
                           "AND PUB_a_general_ledger_master.a_year_i = ? " +
                           "AND PUB_a_general_ledger_master_period.a_glm_sequence_i = PUB_a_general_ledger_master.a_glm_sequence_i " +
                           "AND PUB_a_general_ledger_master_period.a_period_number_i >= ?";

            List <OdbcParameter>parameters = new List <OdbcParameter>();

            OdbcParameter parameter = new OdbcParameter("ledgernumber", OdbcType.Int);
            parameter.Value = ALedgerNumber;
            parameters.Add(parameter);
            parameter = new OdbcParameter("year", OdbcType.Int);
            parameter.Value = ABatchToPost.BatchYear;
            parameters.Add(parameter);
            parameter = new OdbcParameter("period", OdbcType.Int);
            parameter.Value = ABatchToPost.BatchPeriod;
            parameters.Add(parameter);
            DBAccess.GDBAccessObj.Select(ADataSet,
                query,
                ADataSet.AGeneralLedgerMasterPeriod.TableName, Transaction, parameters.ToArray());

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }

        /// <summary>
        /// runs validations on batch, journals and transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        private static bool ValidateBatchAndTransactions(ref GLBatchTDS ADataSet,
            GLPostingTDS APostingDS,
            Int32 ALedgerNumber,
            ABatchRow ABatchToPost,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();

            if ((ABatchToPost.BatchStatus == MFinanceConstants.BATCH_CANCELLED) || (ABatchToPost.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), ABatchToPost.BatchStatus),
                        TResultSeverity.Resv_Critical));
            }

            // Calculate the base currency amounts for each transaction, using the exchange rate from the journals.
            // erm - this is done already? I don't want to do it here, since my journal may contain forex-reval elements.

            // Calculate the credit and debit totals
            GLRoutines.UpdateTotalsOfBatch(ref ADataSet, ABatchToPost);

            if (ABatchToPost.BatchCreditTotal != ABatchToPost.BatchDebitTotal)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It does not balance: Debit is {0}, Credit is {1}"), ABatchToPost.BatchDebitTotal,
                            ABatchToPost.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }
            else if ((ABatchToPost.BatchCreditTotal == 0) && ((ADataSet.AJournal.Rows.Count == 0) || (ADataSet.ATransaction.Rows.Count == 0)))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        Catalog.GetString("The batch has no monetary value. Please cancel it or add transactions."),
                        TResultSeverity.Resv_Critical));
            }
            else if ((ABatchToPost.BatchControlTotal != 0)
                     && (ABatchToPost.BatchControlTotal != ABatchToPost.BatchCreditTotal))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The control total {0:n2} does not fit the Credit/Debit Total {1:n2}."),
                            ABatchToPost.BatchControlTotal,
                            ABatchToPost.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;

            bool NewTransaction = false;
            //TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            if (!TFinancialYear.IsValidPostingPeriod(ABatchToPost.LedgerNumber, ABatchToPost.DateEffective, out DateEffectivePeriodNumber,
                    out DateEffectiveYearNumber,
                    Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                            ABatchToPost.DateEffective),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                // just make sure that the correct BatchPeriod is used
                ABatchToPost.BatchPeriod = DateEffectivePeriodNumber;
                ABatchToPost.BatchYear = DateEffectiveYearNumber;
            }

            // check that all transactions are inside the same period as the GL date effective of the batch
            DateTime PostingPeriodStartDate, PostingPeriodEndDate;
            TFinancialYear.GetStartAndEndDateOfPeriod(ABatchToPost.LedgerNumber,
                DateEffectivePeriodNumber,
                out PostingPeriodStartDate,
                out PostingPeriodEndDate,
                Transaction);

            foreach (ATransactionRow transRow in ADataSet.ATransaction.Rows)
            {
                if ((transRow.BatchNumber == ABatchToPost.BatchNumber)
                    && (transRow.TransactionDate < PostingPeriodStartDate) || (transRow.TransactionDate > PostingPeriodEndDate))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                            String.Format(
                                "invalid transaction date for transaction {0} in Batch {1} Journal {2}: {3:d-MMM-yyyy} must be inside period {4} ({5:d-MMM-yyyy} till {6:d-MMM-yyyy})",
                                transRow.TransactionNumber, transRow.BatchNumber, transRow.JournalNumber,
                                transRow.TransactionDate,
                                DateEffectivePeriodNumber,
                                PostingPeriodStartDate,
                                PostingPeriodEndDate),
                            TResultSeverity.Resv_Critical));
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                journal.DateEffective = ABatchToPost.DateEffective;
                journal.JournalPeriod = ABatchToPost.BatchPeriod;

                if (journal.JournalCreditTotal != journal.JournalDebitTotal)
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The journal {0} does not balance: Debit is {1}, Credit is {2}"),
                                journal.JournalNumber,
                                journal.JournalDebitTotal, journal.JournalCreditTotal),
                            TResultSeverity.Resv_Critical));
                }

                TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                foreach (DataRowView TransactionViewRow in TransactionsOfJournalView)
                {
                    ATransactionRow transaction = (ATransactionRow)TransactionViewRow.Row;

                    // check that transactions on foreign currency accounts are using the correct currency
                    // (fx reval transactions are an exception because they are posted in base currency)
                    if (!((transaction.Reference == CommonAccountingTransactionTypesEnum.REVAL.ToString())
                          && (journal.TransactionTypeCode == CommonAccountingTransactionTypesEnum.REVAL.ToString())))
                    {
                        // get the account that this transaction is writing to
                        AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, transaction.AccountCode });

                        if (Account == null)
                        {
                            // should not get here
                            throw new Exception("ValidateBatchAndTransactions: Cannot find account " + transaction.AccountCode);
                        }

                        if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency != Account.ForeignCurrencyCode))
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                    String.Format(Catalog.GetString(
                                            "Transaction {0} in Journal {1} with currency {2} does not fit the foreign currency {3} of account {4}."),
                                        transaction.TransactionNumber, transaction.JournalNumber, journal.TransactionCurrency,
                                        Account.ForeignCurrencyCode,
                                        transaction.AccountCode),
                                    TResultSeverity.Resv_Critical));
                        }
                    }

                    if ((transaction.AmountInBaseCurrency == 0) && (transaction.TransactionAmount != 0))
                    {
                        AVerifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchToPost.BatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("Transaction {0} in Journal {1} has invalid base transaction amount of 0."),
                                    transaction.TransactionNumber, transaction.JournalNumber),
                                TResultSeverity.Resv_Critical));
                    }
                }
            }

            return !AVerifications.HasCriticalErrors;
        }

        /// <summary>
        /// validate the attributes of the transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        private static bool ValidateAnalysisAttributes(ref GLBatchTDS ADataSet,
            GLPostingTDS APostingDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();

            DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                if (journal.BatchNumber.Equals(ABatchNumber))
                {
                    TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                    foreach (DataRowView transRowView in TransactionsOfJournalView)
                    {
                        ATransactionRow trans = (ATransactionRow)transRowView.Row;
                        // 1. check that all atransanalattrib records are there for all analattributes entries
                        DataView ANView = APostingDS.AAnalysisAttribute.DefaultView;
                        ANView.RowFilter = String.Format("{0} = '{1}' AND {2} = true",
                            AAnalysisAttributeTable.GetAccountCodeDBName(),
                            trans.AccountCode, AAnalysisAttributeTable.GetActiveDBName());
                        int i = 0;

                        while (i < ANView.Count)
                        {
                            AAnalysisAttributeRow attributeRow = (AAnalysisAttributeRow)ANView[i].Row;


                            ATransAnalAttribRow aTransAttribRow =
                                (ATransAnalAttribRow)ADataSet.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber,
                                                                                                        trans.JournalNumber,
                                                                                                        trans.TransactionNumber,
                                                                                                        attributeRow.AnalysisTypeCode });

                            if (aTransAttribRow == null)
                            {
                                AVerifications.Add(new TVerificationResult(
                                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                        String.Format(Catalog.GetString(
                                                "Missing attributes record for journal #{0} transaction #{1}  and TypeCode {2}"),
                                            trans.JournalNumber,
                                            trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                        TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                String v = aTransAttribRow.AnalysisAttributeValue;

                                if ((v == null) || (v.Length == 0))
                                {
                                    AVerifications.Add(new TVerificationResult(
                                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                            String.Format(Catalog.GetString("Missing values at journal #{0} transaction #{1}  and TypeCode {2}"),
                                                trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                            TResultSeverity.Resv_Critical));
                                }
                                else
                                {
                                    AFreeformAnalysisRow afaRow = (AFreeformAnalysisRow)APostingDS.AFreeformAnalysis.Rows.Find(
                                        new Object[] { ALedgerNumber, attributeRow.AnalysisTypeCode, v });

                                    if (afaRow == null)
                                    {
                                        // this would cause a constraint error and is only possible in a development/sqlite environment
                                        AVerifications.Add(new TVerificationResult(
                                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                                String.Format(Catalog.GetString("Invalid values at journal #{0} transaction #{1}  and TypeCode {2}"),
                                                    trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                                TResultSeverity.Resv_Critical));
                                    }
                                    else
                                    {
                                        if (!afaRow.Active)
                                        {
                                            AVerifications.Add(new TVerificationResult(
                                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber,
                                                        ALedgerNumber),
                                                    String.Format(Catalog.GetString(
                                                            "Value {0} not active at journal #{1} transaction #{2}  and TypeCode {3}"), v,
                                                        trans.JournalNumber, trans.TransactionNumber, attributeRow.AnalysisTypeCode),
                                                    TResultSeverity.Resv_Critical));
                                        }
                                    }
                                }
                            }

                            i++;
                        }
                    }
                }
            }

            return !AVerifications.HasCriticalErrors;
        }

        /// Helper class for storing the amounts of a batch at posting level for account/costcentre combinations
        private class TAmount
        {
            /// amount in the base currency of the ledger
            public decimal baseAmount = 0.0M;

            /// amount in transaction currency; only for foreign currency accounts
            public decimal transAmount = 0.0M;

            /// generate a key for the account/costcentre combination
            public static string MakeKey(string AccountCode, string CostCentreCode)
            {
                return AccountCode + ":" + CostCentreCode;
            }

            /// get the account code from the key
            public static string GetAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the cost centre code from the key
            public static string GetCostCentreCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// Helper class for managing the account hierarchy for posting the batch
        private class TAccountTreeElement
        {
            /// Constructor
            public TAccountTreeElement(bool AInvert, bool AForeign)
            {
                Invert = AInvert;
                Foreign = AForeign;
            }

            /// is the debit credit indicator different of the reporting account to the parent account
            public bool Invert = false;

            /// is this account a foreign currency account
            public bool Foreign = false;

            /// generate a key for the reporting account/parent account combination
            public static string MakeKey(string ReportingAccountCode, string AccountCodeReportTo)
            {
                return ReportingAccountCode + ":" + AccountCodeReportTo;
            }

            /// get the reporting account code from the key
            public static string GetReportingAccountCode(string key)
            {
                return key.Split(':')[0];
            }

            /// get the parent account code from the key
            public static string GetAccountReportToCode(string key)
            {
                return key.Split(':')[1];
            }
        }

        /// <summary>
        /// mark each journal, each transaction as being posted;
        /// add sums for costcentre/account combinations
        /// </summary>
        /// <param name="MainDS">can contain several batches and journals and transactions</param>
        /// <param name="APostingDS"></param>
        /// <param name="APostingLevel">the balance changes at the posting level</param>
        /// <param name="ABatchToPost">the batch to post</param>
        /// <returns>a list with the sums for each costcentre/account combination</returns>
        private static SortedList <string, TAmount>MarkAsPostedAndCollectData(GLBatchTDS MainDS,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel, ABatchRow ABatchToPost)
        {
            DataView myView = new DataView(MainDS.ATransaction);

            myView.Sort = ATransactionTable.GetJournalNumberDBName();

            foreach (AJournalRow journal in MainDS.AJournal.Rows)
            {
                if (journal.BatchNumber != ABatchToPost.BatchNumber)
                {
                    continue;
                }

                foreach (DataRowView transactionview in myView.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;

                    if (transaction.BatchNumber != ABatchToPost.BatchNumber)
                    {
                        continue;
                    }

                    transaction.TransactionStatus = true;

                    // get the account that this transaction is writing to
                    AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { transaction.LedgerNumber, transaction.AccountCode });

                    // Set the sign of the amounts according to the debit/credit indicator
                    decimal SignBaseAmount = transaction.AmountInBaseCurrency;
                    decimal SignTransAmount = transaction.TransactionAmount;

                    if (Account.DebitCreditIndicator != transaction.DebitCreditIndicator)
                    {
                        SignBaseAmount *= -1.0M;
                        SignTransAmount *= -1.0M;
                    }

                    // TODO: do we need to check for base currency corrections?
                    // or do we get rid of these problems by not having international currency?

                    string key = TAmount.MakeKey(transaction.AccountCode, transaction.CostCentreCode);

                    if (!APostingLevel.ContainsKey(key))
                    {
                        APostingLevel.Add(key, new TAmount());
                    }

                    APostingLevel[key].baseAmount += SignBaseAmount;

                    // Only foreign currency accounts store a value in the transaction currency,
                    // if the transaction was actually in the foreign currency.

                    if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency == Account.ForeignCurrencyCode))
                    {
                        APostingLevel[key].transAmount += SignTransAmount;
                    }
                }

                journal.JournalStatus = MFinanceConstants.BATCH_POSTED;
            }

            ABatchToPost.BatchStatus = MFinanceConstants.BATCH_POSTED;

            return APostingLevel;
        }

        /// <summary>
        /// Calculate the summarization trees for each posting account and each
        /// posting cost centre. The result of the union of these trees,
        /// excluding the base posting/posting combination, is the set of
        /// accounts that receive the summary data.
        /// </summary>
        private static bool CalculateTrees(
            Int32 ALedgerNumber,
            ref SortedList <string, TAmount>APostingLevel,
            out SortedList <string, TAccountTreeElement>AAccountTree,
            out SortedList <string, string>ACostCentreTree,
            GLPostingTDS APostingDS)
        {
            // get all accounts that each posting level account is directly or indirectly posting to
            AAccountTree = new SortedList <string, TAccountTreeElement>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);

                // only once for each account, even though there might be several entries for one account in APostingLevel because of different costcentres
                if (AAccountTree.ContainsKey(TAccountTreeElement.MakeKey(AccountCode, AccountCode)))
                {
                    continue;
                }

                AAccountRow Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });
                bool DebitCreditIndicator = Account.DebitCreditIndicator;
                AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, AccountCode),
                    new TAccountTreeElement(false, Account.ForeignCurrencyFlag));

                AAccountHierarchyDetailRow HierarchyDetail =
                    (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, AccountCode });

                while (HierarchyDetail != null)
                {
                    Account = (AAccountRow)APostingDS.AAccount.Rows.Find(new object[] { ALedgerNumber, HierarchyDetail.AccountCodeToReportTo });

                    if (Account == null)
                    {
                        // current account is BAL SHT, and it reports nowhere (account with name = ledgernumber does not exist)
                        break;
                    }

                    AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, HierarchyDetail.AccountCodeToReportTo),
                        new TAccountTreeElement(DebitCreditIndicator != Account.DebitCreditIndicator, Account.ForeignCurrencyFlag));

                    HierarchyDetail = (AAccountHierarchyDetailRow)APostingDS.AAccountHierarchyDetail.Rows.Find(
                        new object[] { ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, HierarchyDetail.AccountCodeToReportTo });
                }
            }

            ACostCentreTree = new SortedList <string, string>();

            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                // only once for each cost centre
                if (ACostCentreTree.ContainsKey(CostCentreCode + ":" + CostCentreCode))
                {
                    continue;
                }

                ACostCentreTree.Add(CostCentreCode + ":" + CostCentreCode,
                    CostCentreCode + ":" + CostCentreCode);

                ACostCentreRow CostCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentreCode });

                while (!CostCentre.IsCostCentreToReportToNull())
                {
                    ACostCentreTree.Add(CostCentreCode + ":" + CostCentre.CostCentreToReportTo,
                        CostCentreCode + ":" + CostCentre.CostCentreToReportTo);

                    CostCentre = (ACostCentreRow)APostingDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentre.CostCentreToReportTo });
                }
            }

            return true;
        }

        /// <summary>
        /// for each posting level, propagate the value upwards through both the account and the cost centre hierarchy in glm master;
        /// also propagate the value from the posting period through the following periods;
        /// </summary>
        private static bool SummarizeData(
            GLPostingTDS APostingDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel,
            ref SortedList <string, TAccountTreeElement>AAccountTree,
            ref SortedList <string, string>ACostCentreTree)
        {
            if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                AFromPeriod++;
            }

            DataView GLMMasterView = APostingDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = APostingDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Combine the summarization trees for both the account and the cost centre.
                foreach (string AccountTreeKey in AAccountTree.Keys)
                {
                    if (TAccountTreeElement.GetReportingAccountCode(AccountTreeKey) == AccountCode)
                    {
                        string AccountCodeToReportTo = TAccountTreeElement.GetAccountReportToCode(AccountTreeKey);
                        TAccountTreeElement AccountTreeElement = AAccountTree[AccountTreeKey];

                        foreach (string CostCentreKey in ACostCentreTree.Keys)
                        {
                            if (CostCentreKey.StartsWith(CostCentreCode + ":"))
                            {
                                string CostCentreCodeToReportTo = CostCentreKey.Split(':')[1];
                                decimal SignBaseAmount = PostingLevelElement.baseAmount;
                                decimal SignTransAmount = PostingLevelElement.transAmount;

                                // Set the sign of the amounts according to the debit/credit indicator
                                if (AccountTreeElement.Invert)
                                {
                                    SignBaseAmount *= -1;
                                    SignTransAmount *= -1;
                                }

                                // Find the summary level, creating it if it does not already exist.
                                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });
                                AGeneralLedgerMasterRow GlmRow;

                                if (GLMMasterIndex == -1)
                                {
                                    CreateGLMYear(
                                        ref APostingDS,
                                        APostingDS.ALedger[0].LedgerNumber,
                                        AccountCodeToReportTo,
                                        CostCentreCodeToReportTo);

                                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCodeToReportTo, CostCentreCodeToReportTo });
                                }

                                GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                                GlmRow.YtdActualBase += SignBaseAmount;

                                if (AccountTreeElement.Foreign)
                                {
                                    if (GlmRow.IsYtdActualForeignNull())
                                    {
                                        GlmRow.YtdActualForeign = SignTransAmount;
                                    }
                                    else
                                    {
                                        GlmRow.YtdActualForeign += SignTransAmount;
                                    }
                                }

                                if (APostingDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    GlmRow.ClosingPeriodActualBase += SignBaseAmount;
                                }

                                // Add the period data from the posting level to the summary levels
                                for (Int32 PeriodCount = AFromPeriod;
                                     PeriodCount <= APostingDS.ALedger[0].NumberOfAccountingPeriods + APostingDS.ALedger[0].NumberFwdPostingPeriods;
                                     PeriodCount++)
                                {
                                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                                    if (GLMPeriodIndex == -1)
                                    {
                                        GlmPeriodRow = APostingDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                                        GlmPeriodRow.PeriodNumber = PeriodCount;
                                    }
                                    else
                                    {
                                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                                    }

                                    GlmPeriodRow.ActualBase += SignBaseAmount;

                                    if (AccountTreeElement.Foreign)
                                    {
                                        if (GlmPeriodRow.IsActualForeignNull())
                                        {
                                            GlmPeriodRow.ActualForeign = SignTransAmount;
                                        }
                                        else
                                        {
                                            GlmPeriodRow.ActualForeign += SignTransAmount;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// on the posting level propagate the value from the posting period through the following periods;
        /// in this version of SummarizeData, there is no calculation of summary accounts/cost centres, since that can be done by the reports
        /// </summary>
        private static bool SummarizeDataSimple(
            Int32 ALedgerNumber,
            GLPostingTDS AMainDS,
            Int32 AFromPeriod,
            ref SortedList <string, TAmount>APostingLevel)
        {
            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                AFromPeriod++;
            }

            DataView GLMMasterView = AMainDS.AGeneralLedgerMaster.DefaultView;
            GLMMasterView.Sort = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "," + AGeneralLedgerMasterTable.GetCostCentreCodeDBName();
            DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;
            GLMPeriodView.Sort = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "," + AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName();

            // Loop through the posting data collected earlier.  Summarize it to a
            // temporary table, which is much faster than finding and updating records
            // in the glm tables multiple times.  WriteData will write it to the real
            // tables in a single pass.
            foreach (string PostingLevelKey in APostingLevel.Keys)
            {
                string AccountCode = TAmount.GetAccountCode(PostingLevelKey);
                string CostCentreCode = TAmount.GetCostCentreCode(PostingLevelKey);

                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Find the posting level, creating it if it does not already exist.
                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                AGeneralLedgerMasterRow GlmRow;

                if (GLMMasterIndex == -1)
                {
                    CreateGLMYear(
                        ref AMainDS,
                        ALedgerNumber,
                        AccountCode,
                        CostCentreCode);

                    GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                }

                GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;

                GlmRow.YtdActualBase += PostingLevelElement.baseAmount;

                AAccountRow account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

                if (account.ForeignCurrencyFlag)
                {
                    if (GlmRow.IsYtdActualForeignNull())
                    {
                        GlmRow.YtdActualForeign = PostingLevelElement.transAmount;
                    }
                    else
                    {
                        GlmRow.YtdActualForeign += PostingLevelElement.transAmount;
                    }
                }

                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                {
                    GlmRow.ClosingPeriodActualBase += PostingLevelElement.baseAmount;
                } // Last use of GlmRow in this routine ...

                // propagate the data through the following periods
                for (Int32 PeriodCount = AFromPeriod;
                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                     PeriodCount++)
                {
                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { GlmRow.GlmSequence, PeriodCount });
                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                    if (GLMPeriodIndex == -1)
                    {
                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                        GlmPeriodRow.GlmSequence = GlmRow.GlmSequence;
                        GlmPeriodRow.PeriodNumber = PeriodCount;
                    }
                    else
                    {
                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                    }

                    GlmPeriodRow.ActualBase += PostingLevelElement.baseAmount;

                    if (account.ForeignCurrencyFlag)
                    {
                        if (GlmPeriodRow.IsActualForeignNull())
                        {
                            GlmPeriodRow.ActualForeign = PostingLevelElement.transAmount;
                        }
                        else
                        {
                            GlmPeriodRow.ActualForeign += PostingLevelElement.transAmount;
                        }
                    }
                }
            }

            GLMMasterView.Sort = "";
            GLMPeriodView.Sort = "";

            return true;
        }

        private static void SummarizeInternal(Int32 ALedgerNumber,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel,
            Int32 AFromPeriod,
            bool ACalculatePostingTree)
        {
            // we need the tree, because of the cost centre tree, which is not calculated by the balance sheet and other reports.
            // for testing the balances, we don't need to calculate the whole tree
            if (ACalculatePostingTree)
            {
                if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
                {
                    TLogging.Log("Posting: CalculateTrees...");
                }

                // key is PostingAccount, the value TAccountTreeElement describes the parent account and other details of the relation
                SortedList <string, TAccountTreeElement>AccountTree;

                // key is the PostingCostCentre, the value is the parent Cost Centre
                SortedList <string, string>CostCentreTree;

                // TODO Can anything of this be done in StoredProcedures? Only SQLite here?

                // this was in Petra 2.x; takes a lot of time, which the reports could do better
                // TODO: can we just calculate the cost centre tree, since that is needed for Balance Sheet,
                // but avoid calculating the whole account tree?
                CalculateTrees(ALedgerNumber, ref APostingLevel, out AccountTree, out CostCentreTree, APostingDS);

                if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
                {
                    TLogging.Log("Posting: SummarizeData...");
                }

                SummarizeData(APostingDS, AFromPeriod, ref APostingLevel, ref AccountTree, ref CostCentreTree);
            }
            else
            {
                if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
                {
                    TLogging.Log("Posting: SummarizeDataSimple...");
                }

                SummarizeDataSimple(ALedgerNumber, APostingDS, AFromPeriod, ref APostingLevel);
            }
        }

        /// <summary>
        /// write all changes to the database; on failure the whole transaction is rolled back
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool SubmitChanges(GLPostingTDS AMainDS, out TVerificationResultCollection AVerifications)
        {
            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: SubmitChanges...");
            }

            GLPostingTDSAccess.SubmitChanges(AMainDS.GetChangesTyped(true), out AVerifications);

            if (AVerifications.HasCriticalErrors)
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Finished...");
            }

            return true;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            List <Int32>BatchNumbers = new List <int>();
            BatchNumbers.Add(ABatchNumber);
            return PostGLBatches(ALedgerNumber, BatchNumbers, out AVerifications);
        }

        /// <summary>
        /// post several GL Batches at once
        /// </summary>
        public static bool PostGLBatches(Int32 ALedgerNumber, List <Int32>ABatchNumbers, out TVerificationResultCollection AVerifications)
        {
            // TODO: get a lock on this ledger, no one else is allowed to change anything.

            GLPostingTDS PostingDS;

            LoadDataForPosting(out PostingDS, ALedgerNumber);

            SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

            Int32 BatchPeriod = -1;

            foreach (Int32 BatchNumber in ABatchNumbers)
            {
                if (!PostGLBatchPrepare(ALedgerNumber, BatchNumber, out AVerifications, PostingDS, PostingLevel, ref BatchPeriod))
                {
                    return false;
                }
            }

            SummarizeInternal(ALedgerNumber, PostingDS, PostingLevel, BatchPeriod, true);

            PostingDS.ThrowAwayAfterSubmitChanges = true;

            bool result = SubmitChanges(PostingDS, out AVerifications);

            // TODO: release the lock

            return result;
        }

        /// <summary>
        /// only used for precalculating the new balances before the user actually posts the batch
        /// </summary>
        public static bool TestPostGLBatch(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            out GLPostingTDS APostingDS,
            ref Int32 ABatchPeriod)
        {
            SortedList <string, TAmount>PostingLevel = new SortedList <string, TGLPosting.TAmount>();

            LoadDataForPosting(out APostingDS, ALedgerNumber);

            if (PostGLBatchPrepare(ALedgerNumber, ABatchNumber, out AVerifications, APostingDS, PostingLevel, ref ABatchPeriod))
            {
                SummarizeInternal(ALedgerNumber, APostingDS, PostingLevel, ABatchPeriod, false);
                return true;
            }

            return false;
        }

        /// <summary>
        /// prepare posting a GL Batch, without saving to database yet.
        /// This is called by the actual PostGLBatch routine, but also by the routine for testing what would happen to the balances.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber">Batch to post</param>
        /// <param name="AVerifications"></param>
        /// <param name="APostingDS">account, costcentre, hierarchy tables</param>
        /// <param name="APostingLevel">collected new balances</param>
        /// <param name="ABatchPeriod">make sure that all batches that are posted together, are from the same period</param>
        /// <returns></returns>
        private static bool PostGLBatchPrepare(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            GLPostingTDS APostingDS,
            SortedList <string, TAmount>APostingLevel,
            ref Int32 ABatchPeriod)
        {
            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: LoadData...");
            }

            // get the data from the database into the MainDS
            GLBatchTDS BatchDS;

            if (!LoadData(out BatchDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Validation...");
            }

            ABatchRow BatchToPost =
                ((ABatchRow)BatchDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber }));

            if (ABatchPeriod == -1)
            {
                ABatchPeriod = BatchToPost.BatchPeriod;
            }
            else if (ABatchPeriod != BatchToPost.BatchPeriod)
            {
                AVerifications = new TVerificationResultCollection();
                AVerifications.Add(new TVerificationResult(
                        Catalog.GetString("Cannot post Batches from different periods at once!"),
                        Catalog.GetString("Batches from more than one period."),
                        TResultSeverity.Resv_Critical));
                return false;
            }

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            if (!ValidateBatchAndTransactions(ref BatchDS, APostingDS, ALedgerNumber, BatchToPost, out AVerifications))
            {
                return false;
            }

            if (!ValidateAnalysisAttributes(ref BatchDS, APostingDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Load GLM Data...");
            }

            // TODO
            LoadGLMData(ref APostingDS, ALedgerNumber, BatchToPost);

            if (TLogging.DebugLevel >= POSTING_LOGLEVEL)
            {
                TLogging.Log("Posting: Mark as posted and collect data...");
            }

            // post each journal, each transaction; add sums for costcentre/account combinations
            MarkAsPostedAndCollectData(BatchDS, APostingDS, APostingLevel, BatchToPost);

            if (GLBatchTDSAccess.SubmitChanges(BatchDS, out AVerifications) == TSubmitChangesResult.scrOK)
            {
                // if posting goes wrong later, the transation will be rolled back
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tell me whether this Batch can be cancelled
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool GLBatchCanBeCancelled(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            // get the data from the database into the MainDS
            if (!LoadData(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            ABatchRow Batch = AMainDS.ABatch[0];

            if (Batch.BatchStatus == MFinanceConstants.BATCH_POSTED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
                return false;
            }

            if (Batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED)
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot cancel Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It was already cancelled.")),
                        TResultSeverity.Resv_Critical));
                return false;
            }

            return true;
        }

        /// <summary>
        /// If a Batch has been created then found to be not required, it can be deleted here.
        /// (This was added for ICH and Stewardship calculations, which can otherwise leave empty batches in the ledger.)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        public static bool DeleteGLBatch(
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            GLBatchTDS TempTDS;

            if (!GLBatchCanBeCancelled(out TempTDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }
            else
            {
                ABatchRow BatchRow = TempTDS.ABatch[0];

                //
                // If I'm deleting the most recent entry (which is almost certainly the case)
                // I can wind back the Ledger's LastBatchNumber so as not to leave a gap.
                //
                if (BatchRow.BatchNumber == TempTDS.ALedger[0].LastBatchNumber)
                {
                    TempTDS.ALedger[0].LastBatchNumber--;
                }

                BatchRow.Delete();
                //
                // If this batch has journals and transactions, they need to be deleted too,
                // along with any trans_anal_attrib records.
                //
                // The call to GLBatchCanBeCancelled will have loaded all these records for me.

                TempTDS.AJournal.Rows.Clear();
                TempTDS.ATransaction.Rows.Clear();
                TempTDS.ATransAnalAttrib.Rows.Clear();

                return GLBatchTDSAccess.SubmitChanges(TempTDS, out AVerifications) == TSubmitChangesResult.scrOK;
            }
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber, Boolean ACommitTransaction = true)
        {
            bool NewTransactionStarted = false;

            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                MainDS = new GLBatchTDS();

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransactionStarted);

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                NewRow.LedgerNumber = ALedgerNumber;
                MainDS.ALedger[0].LastBatchNumber++;
                NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                NewRow.BatchYear = MainDS.ALedger[0].CurrentFinancialYear;
                MainDS.ABatch.Rows.Add(NewRow);

                if (GLBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    MainDS.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }
            finally
            {
                if (NewTransactionStarted && ACommitTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            return MainDS;
        }

        /// <summary>
        /// create a new batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchDescription"></param>
        /// <param name="ABatchControlTotal"></param>
        /// <param name="ADateEffective"></param>
        /// <returns></returns>
        public static GLBatchTDS CreateABatch(
            Int32 ALedgerNumber,
            string ABatchDescription,
            decimal ABatchControlTotal,
            DateTime ADateEffective)
        {
            bool NewTransactionStarted = false;

            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                MainDS = new GLBatchTDS();

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransactionStarted);

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
                NewRow.LedgerNumber = ALedgerNumber;
                MainDS.ALedger[0].LastBatchNumber++;
                NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
                NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                NewRow.BatchYear = MainDS.ALedger[0].CurrentFinancialYear;

                int FinancialYear, FinancialPeriod;

                if (ADateEffective != default(DateTime))
                {
                    TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref ADateEffective, out FinancialYear, out FinancialPeriod, null, false);
                    NewRow.DateEffective = ADateEffective;
                    NewRow.BatchPeriod = FinancialPeriod;
                    NewRow.BatchYear = FinancialYear;
                }

                NewRow.BatchDescription = ABatchDescription;
                NewRow.BatchControlTotal = ABatchControlTotal;
                MainDS.ABatch.Rows.Add(NewRow);

                if (GLBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    MainDS.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }
            finally
            {
                if (NewTransactionStarted)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            return MainDS;
        }

        /// <summary>
        /// create a new recurring batch.
        /// it is already stored to the database, to avoid problems with LastBatchNumber
        /// </summary>
        public static GLBatchTDS CreateARecurringBatch(Int32 ALedgerNumber)
        {
            bool NewTransactionStarted = false;

            GLBatchTDS MainDS = null;

            //Error handling
            string ErrorContext = "Create a recurring Batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                MainDS = new GLBatchTDS();

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransactionStarted);

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                ARecurringBatchRow NewRow = MainDS.ARecurringBatch.NewRowTyped(true);
                NewRow.LedgerNumber = ALedgerNumber;
                MainDS.ALedger[0].LastRecurringBatchNumber++;
                NewRow.BatchNumber = MainDS.ALedger[0].LastRecurringBatchNumber;
                MainDS.ARecurringBatch.Rows.Add(NewRow);

                if (GLBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    MainDS.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a recurring batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }
            finally
            {
                if (NewTransactionStarted)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            return MainDS;
        }

        /// <summary>
        /// Create a new journal as per gl1120.i
        /// </summary>
        public static bool CreateAJournal(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber, Int32 ABatchNumber, Int32 ALastJournalNumber,
            string AJournalDescription, string ACurrency, decimal AXRateToBase,
            DateTime ADateEffective, Int32 APeriodNumber, out Int32 AJournalNumber)
        {
            bool CreationSuccessful = false;

            AJournalNumber = 0;

            //Error handling
            string ErrorContext = "Create a Journal";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = AMainDS.AJournal.NewRowTyped();
                JournalRow.LedgerNumber = ALedgerNumber;
                JournalRow.BatchNumber = ABatchNumber;
                AJournalNumber = ALastJournalNumber + 1;
                JournalRow.JournalNumber = AJournalNumber;
                JournalRow.JournalDescription = AJournalDescription;
                JournalRow.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                JournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
                JournalRow.TransactionCurrency = ACurrency;
                JournalRow.ExchangeRateToBase = AXRateToBase;
                JournalRow.DateEffective = ADateEffective;
                JournalRow.JournalPeriod = APeriodNumber;
                AMainDS.AJournal.Rows.Add(JournalRow);

                //Update the Last Journal
                ABatchRow BatchRow = (ABatchRow)AMainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber });
                BatchRow.LastJournal = AJournalNumber;

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }

        /// <summary>
        /// create a record for a_transaction
        /// </summary>
        public static bool CreateATransaction(
            GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string ANarrative,
            string AAccountCode,
            string ACostCentreCode,
            decimal ATransAmount,
            DateTime ATransDate,
            bool ADebCredIndicator,
            string AReference,
            bool ASystemGenerated,
            decimal ABaseAmount,
            out int ATransactionNumber)
        {
            bool CreationSuccessful = false;

            ATransactionNumber = 0;

            //Error handling
            string ErrorContext = "Create a Transaction";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;
            TVerificationResultCollection VerificationResult = null;

            try
            {
                AJournalRow JournalRow = (AJournalRow)AMainDS.AJournal.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber });

                //Increment the LastTransactionNumber
                JournalRow.LastTransactionNumber++;
                ATransactionNumber = JournalRow.LastTransactionNumber;

                ATransactionRow TransactionRow = AMainDS.ATransaction.NewRowTyped();

                TransactionRow.LedgerNumber = ALedgerNumber;
                TransactionRow.BatchNumber = ABatchNumber;
                TransactionRow.JournalNumber = AJournalNumber;
                TransactionRow.TransactionNumber = ATransactionNumber;
                TransactionRow.Narrative = ANarrative;
                TransactionRow.Reference = AReference;
                TransactionRow.AccountCode = AAccountCode;
                TransactionRow.CostCentreCode = ACostCentreCode;
                TransactionRow.DebitCreditIndicator = ADebCredIndicator;
                TransactionRow.SystemGenerated = ASystemGenerated;
                TransactionRow.AmountInBaseCurrency = ABaseAmount;
                TransactionRow.TransactionAmount = ATransAmount;
                TransactionRow.TransactionDate = ATransDate;

                AMainDS.ATransaction.Rows.Add(TransactionRow);

                CreationSuccessful = true;
            }
            catch (Exception ex)
            {
                ErrorMessage =
                    String.Format(Catalog.GetString("Unknown error while creating a batch for Ledger: {0}." +
                            Environment.NewLine + Environment.NewLine + ex.ToString()),
                        ALedgerNumber);
                ErrorType = TResultSeverity.Resv_Critical;
                VerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            return CreationSuccessful;
        }
    }
}