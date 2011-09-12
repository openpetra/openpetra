//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, morayh
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
            out TVerificationResultCollection AVerificationResult,
            TDBTransaction ATransaction)
        {
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ATransaction);

            if (LedgerTable.Count != 1)
            {
                throw new Exception("TFinancialYearWebConnector.CreateGLMYear: Cannot find ledger " + ALedgerNumber.ToString());
            }

            ALedgerRow Ledger = LedgerTable[0];
#if NOTNEEDED
            if (AGeneralLedgerMasterAccess.Exists(ALedgerNumber, Ledger.CurrentFinancialYear, AAccountCode, ACostCentreCode, ATransaction))
            {
                throw new Exception("TFinancialYearWebConnector.CreateGLMYear: There is already a GLM Master record for " +
                    ALedgerNumber.ToString() + "/" +
                    Ledger.CurrentFinancialYear.ToString() + "/" +
                    AAccountCode + "/" + ACostCentreCode);
            }
#endif
            AGeneralLedgerMasterTable GLMTable = new AGeneralLedgerMasterTable();
            AGeneralLedgerMasterRow GLMRow = GLMTable.NewRowTyped();

            // row.GlmSequence will be set by SubmitChanges
            GLMRow.GlmSequence = -1;
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

            ALedgerAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, Transaction);

            ABatchAccess.LoadByPrimaryKey(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            AJournalAccess.LoadViaABatch(ADataSet, ALedgerNumber, ABatchNumber, Transaction);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                ATransactionAccess.LoadViaAJournal(ADataSet, journal.LedgerNumber,
                    journal.BatchNumber,
                    journal.JournalNumber,
                    Transaction);
            }

            // load the atransanalattrib
            foreach (ATransactionRow trans in ADataSet.ATransaction.Rows)
            {
                ATransAnalAttribAccess.LoadViaATransaction(ADataSet,
                    trans.LedgerNumber,
                    trans.BatchNumber,
                    trans.JournalNumber,
                    trans.TransactionNumber,
                    Transaction);
            }

            // load all accounts of ledger, because we need them later for the account hierarchy tree for summarisation
            AAccountAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            // TODO: use cached table?
            AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(ADataSet, ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, Transaction);

            // TODO: use cached table?
            ACostCentreAccess.LoadViaALedger(ADataSet, ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return true;
        }

        /// <summary>
        /// runs validations on batch, journals and transactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool ValidateBatchAndTransactions(ref GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            ABatchRow Batch = ADataSet.ABatch[0];

            if ((Batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED) || (Batch.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It has status {0}"), Batch.BatchStatus),
                        TResultSeverity.Resv_Critical));
            }

            // calculate the base currency amounts for each transaction, using the exchange rate from the journals.
            // calculate the credit and debit totals
            GLRoutines.UpdateTotalsOfBatch(ref ADataSet, Batch);

            if (Convert.ToDecimal(Batch.BatchCreditTotal) != Convert.ToDecimal(Batch.BatchDebitTotal))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("It does not balance: Debit is {0}, Credit is {1}"), Batch.BatchDebitTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }
            else if (Batch.BatchCreditTotal == 0)
            {
//                AVerifications.Add(new TVerificationResult(
//                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
//                        Catalog.GetString("It has no monetary value. Please cancel it or add meaningful transactions."),
//                        TResultSeverity.Resv_Critical));
            }
            else if ((Batch.BatchControlTotal != 0) && (Convert.ToDecimal(Batch.BatchControlTotal) != Convert.ToDecimal(Batch.BatchCreditTotal)))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                        String.Format(Catalog.GetString("The control total {0} does not fit the Credit/Debit Total {1}."), Batch.BatchControlTotal,
                            Batch.BatchCreditTotal),
                        TResultSeverity.Resv_Critical));
            }

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!TFinancialYear.IsValidPostingPeriod(Batch.LedgerNumber, Batch.DateEffective, out DateEffectivePeriodNumber,
                    out DateEffectiveYearNumber,
                    Transaction))
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
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            DataView accountView = new DataView(ADataSet.AAccount);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                journal.DateEffective = Batch.DateEffective;
                journal.JournalPeriod = Batch.BatchPeriod;

                journal.JournalCreditTotal = 0.0M;
                journal.JournalDebitTotal = 0.0M;
                DataView TransactionsByJournal = ADataSet.ATransaction.DefaultView;
                TransactionsByJournal.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                foreach (DataRowView transRowView in TransactionsByJournal)
                {
                    ATransactionRow trans = (ATransactionRow)transRowView.Row;

                    if (trans.DebitCreditIndicator)
                    {
                        journal.JournalDebitTotal += trans.TransactionAmount;
                    }
                    else
                    {
                        journal.JournalCreditTotal += trans.TransactionAmount;
                    }
                }

                if (Convert.ToDecimal(journal.JournalCreditTotal) != Convert.ToDecimal(journal.JournalDebitTotal))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The journal {0} does not balance: Debit is {1}, Credit is {2}"), journal.JournalNumber,
                                journal.JournalDebitTotal, journal.JournalCreditTotal),
                            TResultSeverity.Resv_Critical));
                }

                DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);
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
                        accountView.RowFilter = AAccountTable.GetAccountCodeDBName() + " = '" + transaction.AccountCode + "'";

                        if (accountView.Count != 1)
                        {
                            // should not get here
                            throw new Exception("ValidateBatchAndTransactions: Cannot find account " + transaction.AccountCode);
                        }

                        AAccountRow Account = (AAccountRow)accountView[0].Row;

                        if (Account.ForeignCurrencyFlag && (journal.TransactionCurrency != Account.ForeignCurrencyCode))
                        {
                            AVerifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
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
                                String.Format(Catalog.GetString("Cannot post Batch {0} in Ledger {1}"), ABatchNumber, ALedgerNumber),
                                String.Format(Catalog.GetString("Transaction {0} in Journal {1} has invalid base transaction amount of 0."),
                                    transaction.TransactionNumber, transaction.JournalNumber),
                                TResultSeverity.Resv_Critical));
                    }
                }
            }

            return !AVerifications.HasCriticalError();
        }

        /// <summary>
        /// validate the attributes of the tarnsactions
        /// some things are even modified, eg. batch period etc from date effective
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool ValidateAnalysisAttributes(ref GLBatchTDS ADataSet,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            // fetch all the analysis tables

            GLSetupTDS analysisDS = new GLSetupTDS();
            AAnalysisTypeAccess.LoadAll(analysisDS, null);
            AFreeformAnalysisAccess.LoadViaALedger(analysisDS, ALedgerNumber, null);
            AAnalysisAttributeAccess.LoadViaALedger(analysisDS, ALedgerNumber, null);

            foreach (AJournalRow journal in ADataSet.AJournal.Rows)
            {
                if (journal.BatchNumber.Equals(ABatchNumber))
                {
                    DataView TransactionsOfJournalView = new DataView(ADataSet.ATransaction);
                    TransactionsOfJournalView.RowFilter = ATransactionTable.GetJournalNumberDBName() + " = " + journal.JournalNumber.ToString();

                    foreach (DataRowView transRowView in TransactionsOfJournalView)
                    {
                        ATransactionRow trans = (ATransactionRow)transRowView.Row;
                        // 1. check that all atransanalattrib records are there for all analattributes entries
                        DataView ANView = analysisDS.AAnalysisAttribute.DefaultView;
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
                                    AFreeformAnalysisRow afaRow = (AFreeformAnalysisRow)analysisDS.AFreeformAnalysis.Rows.Find(
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

            return !AVerifications.HasCriticalError();
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
        /// <param name="MainDS">only contains the batch to post, and its journals and transactions</param>
        /// <returns>a list with the sums for each costcentre/account combination</returns>
        private static SortedList <string, TAmount>MarkAsPostedAndCollectData(ref GLBatchTDS MainDS)
        {
            SortedList <string, TAmount>PostingLevel = new SortedList <string, TAmount>();
            DataView accountView = new DataView(MainDS.AAccount);

            foreach (AJournalRow journal in MainDS.AJournal.Rows)
            {
                DataView myView = new DataView(MainDS.ATransaction);
                myView.Sort = ATransactionTable.GetJournalNumberDBName();

                foreach (DataRowView transactionview in myView.FindRows(journal.JournalNumber))
                {
                    ATransactionRow transaction = (ATransactionRow)transactionview.Row;
                    transaction.TransactionStatus = true;

                    // get the account that this transaction is writing to
                    accountView.RowFilter = AAccountTable.GetAccountCodeDBName() + " = '" + transaction.AccountCode + "'";
                    AAccountRow Account = (AAccountRow)accountView[0].Row;

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

                    if (!PostingLevel.ContainsKey(key))
                    {
                        PostingLevel.Add(key, new TAmount());
                    }

                    PostingLevel[key].baseAmount += SignBaseAmount;

                    // Only foreign currency accounts store a value in the transaction currency
                    if (Account.ForeignCurrencyFlag)
                    {
                        PostingLevel[key].transAmount += SignTransAmount;
                    }
                }

                journal.JournalStatus = MFinanceConstants.BATCH_POSTED;
            }

            MainDS.ABatch[0].BatchStatus = MFinanceConstants.BATCH_POSTED;

            return PostingLevel;
        }

        /// <summary>
        /// Calculate the summarization trees for each posting account and each
        /// posting cost centre. The result of the union of these trees,
        /// excluding the base posting/posting combination, is the set of
        /// accounts that receive the summary data.
        /// </summary>
        /// <param name="APostingLevel"></param>
        /// <param name="AAccountTree"></param>
        /// <param name="ACostCentreTree"></param>
        /// <param name="AAccountView"></param>
        /// <param name="AAccountHierarchyDetailView"></param>
        /// <param name="ACostCentreView"></param>
        /// <returns></returns>
        private static bool CalculateTrees(ref SortedList <string, TAmount>APostingLevel,
            out SortedList <string, TAccountTreeElement>AAccountTree,
            out SortedList <string, string>ACostCentreTree,
            DataView AAccountView,
            DataView AAccountHierarchyDetailView,
            DataView ACostCentreView)
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

                AAccountView.RowFilter = AAccountTable.GetAccountCodeDBName() + "='" + AccountCode + "'";
                AAccountRow Account = (AAccountRow)AAccountView[0].Row;
                bool DebitCreditIndicator = Account.DebitCreditIndicator;
                AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, AccountCode),
                    new TAccountTreeElement(false, Account.ForeignCurrencyFlag));

                AAccountHierarchyDetailView.RowFilter = AAccountHierarchyDetailTable.GetReportingAccountCodeDBName() + "='" + AccountCode + "'";

                while (AAccountHierarchyDetailView.Count > 0)
                {
                    AAccountHierarchyDetailRow HierarchyDetail = (AAccountHierarchyDetailRow)AAccountHierarchyDetailView[0].Row;
                    AAccountView.RowFilter = AAccountTable.GetAccountCodeDBName() + "='" + HierarchyDetail.AccountCodeToReportTo + "'";

                    if (AAccountView.Count == 0)
                    {
                        // current account is BAL SHT, and it reports nowhere (account with name = ledgernumber does not exist)
                        break;
                    }

                    Account = (AAccountRow)AAccountView[0].Row;
                    AAccountTree.Add(TAccountTreeElement.MakeKey(AccountCode, HierarchyDetail.AccountCodeToReportTo),
                        new TAccountTreeElement(DebitCreditIndicator != Account.DebitCreditIndicator, Account.ForeignCurrencyFlag));
                    AAccountHierarchyDetailView.RowFilter = AAccountHierarchyDetailTable.GetReportingAccountCodeDBName() + "=" +
                                                            "'" + HierarchyDetail.AccountCodeToReportTo + "'";
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

                ACostCentreView.RowFilter = ACostCentreTable.GetCostCentreCodeDBName() + "='" + CostCentreCode + "'";
                ACostCentreRow CostCentre = (ACostCentreRow)ACostCentreView[0].Row;

                while (!CostCentre.IsCostCentreToReportToNull())
                {
                    ACostCentreTree.Add(CostCentreCode + ":" + CostCentre.CostCentreToReportTo,
                        CostCentreCode + ":" + CostCentre.CostCentreToReportTo);

                    ACostCentreView.RowFilter = ACostCentreTable.GetCostCentreCodeDBName() + "='" + CostCentre.CostCentreToReportTo + "'";
                    CostCentre = (ACostCentreRow)ACostCentreView[0].Row;
                }
            }

            return true;
        }

        /// <summary>
        /// for each posting level, propagate the value upwards through both the account and the cost centre hierarchy in glm master;
        /// also propagate the value from the posting period through the following periods;
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="APostingLevel"></param>
        /// <param name="AAccountTree"></param>
        /// <param name="ACostCentreTree"></param>
        /// <returns></returns>
        private static bool SummarizeData(
            ref GLBatchTDS AMainDS,
            ref SortedList <string, TAmount>APostingLevel,
            ref SortedList <string, TAccountTreeElement>AAccountTree,
            ref SortedList <string, string>ACostCentreTree)
        {
            Int32 TempGLMSequence = -1;
            Int32 FromPeriod = AMainDS.ABatch[0].BatchPeriod;

            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                FromPeriod++;
            }

            DataView GLMMasterView = AMainDS.AGeneralLedgerMaster.DefaultView;
            DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;

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
                                GLMMasterView.RowFilter = AGeneralLedgerMasterTable.GetAccountCodeDBName() + "='" + AccountCodeToReportTo +
                                                          "' and " +
                                                          AGeneralLedgerMasterTable.GetCostCentreCodeDBName() + "='" + CostCentreCodeToReportTo + "'";
                                AGeneralLedgerMasterRow GlmRow;

                                if (GLMMasterView.Count == 0)
                                {
                                    TempGLMSequence--;
                                    GlmRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();
                                    GlmRow.LedgerNumber = -1;
                                    GlmRow.Year = -1;
                                    GlmRow.AccountCode = AccountCodeToReportTo;
                                    GlmRow.CostCentreCode = CostCentreCodeToReportTo;
                                    GlmRow.GlmSequence = TempGLMSequence;
                                    AMainDS.AGeneralLedgerMaster.Rows.Add(GlmRow);
                                }
                                else
                                {
                                    GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[0].Row;
                                }

                                GlmRow.YtdActualBase += SignBaseAmount;

                                if (AccountTreeElement.Foreign)
                                {
                                    GlmRow.YtdActualForeign += SignTransAmount;
                                }

                                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    GlmRow.ClosingPeriodActualBase += SignBaseAmount;
                                }

                                // Add the period data from the posting level to the summary levels
                                for (Int32 PeriodCount = FromPeriod;
                                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                                     PeriodCount++)
                                {
                                    GLMPeriodView.RowFilter = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "=" +
                                                              TempGLMSequence.ToString() + " and " +
                                                              AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName() + "=" + PeriodCount.ToString();
                                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                                    if (GLMPeriodView.Count == 0)
                                    {
                                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                                        GlmPeriodRow.GlmSequence = TempGLMSequence;
                                        GlmPeriodRow.PeriodNumber = PeriodCount;
                                        AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(GlmPeriodRow);
                                    }
                                    else
                                    {
                                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[0].Row;
                                    }

                                    GlmPeriodRow.ActualBase += SignBaseAmount;

                                    if (AccountTreeElement.Foreign)
                                    {
                                        GlmPeriodRow.ActualForeign += SignTransAmount;
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
        /// <param name="AMainDS"></param>
        /// <param name="APostingLevel"></param>
        /// <returns></returns>
        private static bool SummarizeDataSimple(
            ref GLBatchTDS AMainDS,
            ref SortedList <string, TAmount>APostingLevel)
        {
            Int32 TempGLMSequence = AMainDS.AGeneralLedgerMaster.Count * (-1) - 1;
            Int32 FromPeriod = AMainDS.ABatch[0].BatchPeriod;

            if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
            {
                // If the year end close is running, then we are posting the year end
                // reallocations.  These appear as part of the final period, but
                // should only be written to the forward periods.
                // In year end, a_current_period_i = a_number_of_accounting_periods_i = a_batch_period_i.
                FromPeriod++;
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

                AMainDS.AAccount.DefaultView.RowFilter = String.Format("{0}='{1}'", AAccountTable.GetAccountCodeDBName(), AccountCode);
                bool isForeignAccount = ((AAccountRow)AMainDS.AAccount.DefaultView[0].Row).ForeignCurrencyFlag;

                TLogging.Log(AccountCode + " " + CostCentreCode);
                TAmount PostingLevelElement = APostingLevel[PostingLevelKey];

                // Find the posting level, creating it if it does not already exist.
                int GLMMasterIndex = GLMMasterView.Find(new object[] { AccountCode, CostCentreCode });
                AGeneralLedgerMasterRow GlmRow;

                if (GLMMasterIndex == -1)
                {
                    TempGLMSequence--;
                    GlmRow = AMainDS.AGeneralLedgerMaster.NewRowTyped();
                    GlmRow.LedgerNumber = -1;
                    GlmRow.Year = -1;
                    GlmRow.AccountCode = AccountCode;
                    GlmRow.CostCentreCode = CostCentreCode;
                    GlmRow.GlmSequence = TempGLMSequence;
                    AMainDS.AGeneralLedgerMaster.Rows.Add(GlmRow);
                }
                else
                {
                    GlmRow = (AGeneralLedgerMasterRow)GLMMasterView[GLMMasterIndex].Row;
                }

                GlmRow.YtdActualBase += PostingLevelElement.baseAmount;

                if (isForeignAccount)
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
                for (Int32 PeriodCount = FromPeriod;
                     PeriodCount <= AMainDS.ALedger[0].NumberOfAccountingPeriods + AMainDS.ALedger[0].NumberFwdPostingPeriods;
                     PeriodCount++)
                {
                    int GLMPeriodIndex = GLMPeriodView.Find(new object[] { TempGLMSequence, PeriodCount });
                    AGeneralLedgerMasterPeriodRow GlmPeriodRow;

                    if (GLMPeriodIndex == -1)
                    {
                        TempGLMSequence--;
                        GlmPeriodRow = AMainDS.AGeneralLedgerMasterPeriod.NewRowTyped();
                        GlmPeriodRow.GlmSequence = TempGLMSequence;
                        GlmPeriodRow.PeriodNumber = PeriodCount;
                        AMainDS.AGeneralLedgerMasterPeriod.Rows.Add(GlmPeriodRow);
                    }
                    else
                    {
                        GlmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodView[GLMPeriodIndex].Row;
                    }

                    GlmPeriodRow.ActualBase += PostingLevelElement.baseAmount;

                    if (isForeignAccount)
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

        /// <summary>
        /// write all changes to the database; on failure the whole transaction is rolled back
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        private static bool SubmitChanges(ref GLBatchTDS AMainDS, out TVerificationResultCollection AVerifications)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                ABatchAccess.SubmitChanges(AMainDS.ABatch, Transaction, out AVerifications);

                if (!AVerifications.HasCriticalError())
                {
                    AJournalAccess.SubmitChanges(AMainDS.AJournal, Transaction, out AVerifications);

                    if (!AVerifications.HasCriticalError())
                    {
                        ATransactionAccess.SubmitChanges(AMainDS.ATransaction, Transaction, out AVerifications);

                        if (!AVerifications.HasCriticalError())
                        {
                            // write glm master and periods
                            foreach (AGeneralLedgerMasterRow glmMaster in AMainDS.AGeneralLedgerMaster.Rows)
                            {
                                AGeneralLedgerMasterTable DBMasterTable =
                                    AGeneralLedgerMasterAccess.LoadByUniqueKey(AMainDS.ALedger[0].LedgerNumber,
                                        AMainDS.ALedger[0].CurrentFinancialYear,
                                        glmMaster.AccountCode,
                                        glmMaster.CostCentreCode,
                                        Transaction);

                                if (DBMasterTable.Count == 0)
                                {
                                    int NewGlmSequence = CreateGLMYear(AMainDS.ALedger[0].LedgerNumber,
                                        glmMaster.AccountCode,
                                        glmMaster.CostCentreCode,
                                        out AVerifications,
                                        Transaction);

                                    if (AVerifications.HasCriticalError())
                                    {
                                        DBAccess.GDBAccessObj.RollbackTransaction();
                                        return false;
                                    }

                                    DBMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(NewGlmSequence, Transaction);
                                }

                                AGeneralLedgerMasterRow DBMasterRow = DBMasterTable[0];
                                DBMasterRow.YtdActualBase += glmMaster.YtdActualBase;

                                if (!glmMaster.IsYtdActualForeignNull())
                                {
                                    if (DBMasterRow.IsYtdActualForeignNull())
                                    {
                                        DBMasterRow.YtdActualForeign = glmMaster.YtdActualForeign;
                                    }
                                    else
                                    {
                                        DBMasterRow.YtdActualForeign += glmMaster.YtdActualForeign;
                                    }
                                }

                                if (AMainDS.ALedger[0].ProvisionalYearEndFlag)
                                {
                                    DBMasterRow.ClosingPeriodActualBase += glmMaster.ClosingPeriodActualBase;
                                }

                                AGeneralLedgerMasterAccess.SubmitChanges(DBMasterTable, Transaction, out AVerifications);

                                if (AVerifications.HasCriticalError())
                                {
                                    DBAccess.GDBAccessObj.RollbackTransaction();
                                    return false;
                                }

                                DataView GLMPeriodView = AMainDS.AGeneralLedgerMasterPeriod.DefaultView;
                                GLMPeriodView.RowFilter = AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName() + "=" +
                                                          glmMaster.GlmSequence.ToString();

                                foreach (DataRowView GLMPeriodRowView in GLMPeriodView)
                                {
                                    AGeneralLedgerMasterPeriodRow glmPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodRowView.Row;

                                    AGeneralLedgerMasterPeriodTable DBPeriodTable =
                                        AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(DBMasterRow.GlmSequence,
                                            glmPeriodRow.PeriodNumber,
                                            Transaction);
                                    AGeneralLedgerMasterPeriodRow DBPeriodRow = DBPeriodTable[0];
                                    DBPeriodRow.ActualBase += glmPeriodRow.ActualBase;

                                    if (!glmPeriodRow.IsActualForeignNull())
                                    {
                                        if (DBPeriodRow.IsActualForeignNull())
                                        {
                                            DBPeriodRow.ActualForeign = glmPeriodRow.ActualForeign;
                                        }
                                        else
                                        {
                                            DBPeriodRow.ActualForeign += glmPeriodRow.ActualForeign;
                                        }
                                    }

                                    AGeneralLedgerMasterPeriodAccess.SubmitChanges(DBPeriodTable, Transaction, out AVerifications);

                                    if (AVerifications.HasCriticalError())
                                    {
                                        DBAccess.GDBAccessObj.RollbackTransaction();
                                        return false;
                                    }
                                }
                            }

                            // TODO: write glm and glmperiod tables only at the very end! otherwise too many times calls to submitchanges
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AVerifications = new TVerificationResultCollection();

                AVerifications.Add(new TVerificationResult("Exception in GL.Posting.cs " + Environment.NewLine +
                        "[" + e.Source +
                        "]" + Environment.NewLine + e.ToString(),
                        "Message: " + e.Message,
                        TResultSeverity.Resv_Critical));

                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
            }

            if (AVerifications.HasCriticalError())
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                return false;
            }

            DBAccess.GDBAccessObj.CommitTransaction();
            AMainDS.AcceptChanges();
            return true;
        }

        /// <summary>
        /// prepare posting a GL Batch, without saving to database yet.
        /// This is called by the actual PostGLBatch routine, but also by the routine for testing what would happen to the balances.
        /// </summary>
        public static bool PostGLBatchInternal(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications,
            out GLBatchTDS AMainDS)
        {
            // get the data from the database into the MainDS
            if (!LoadData(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            // first validate Batch, and Transactions; check credit/debit totals; check currency, etc
            if (!ValidateBatchAndTransactions(ref AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            if (!ValidateAnalysisAttributes(ref AMainDS, ALedgerNumber, ABatchNumber, out AVerifications))
            {
                return false;
            }

            // post each journal, each transaction; add sums for costcentre/account combinations
            SortedList <string, TAmount>PostingLevel = MarkAsPostedAndCollectData(ref AMainDS);

// we need the tree, because of the cost centre tree, which is not calculated by the balance sheet and other reports
//#if OLD_POSTING_WITH_TREE
            // key is PostingAccount, the value TAccountTreeElement describes the parent account and other details of the relation
            SortedList <string, TAccountTreeElement>AccountTree;

            // key is the PostingCostCentre, the value is the parent Cost Centre
            SortedList <string, string>CostCentreTree;

            // TODO Can anything of this be done in StoredProcedures? Only SQLite here?

            // this was in Petra 2.x; takes a lot of time, which the reports could do better
            CalculateTrees(ref PostingLevel, out AccountTree, out CostCentreTree,
                AMainDS.AAccount.DefaultView,
                AMainDS.AAccountHierarchyDetail.DefaultView,
                AMainDS.ACostCentre.DefaultView);

            SummarizeData(ref AMainDS, ref PostingLevel, ref AccountTree, ref CostCentreTree);
//#endif

            SummarizeDataSimple(ref AMainDS, ref PostingLevel);

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
            GLBatchTDS MainDS;

            if (PostGLBatchInternal(ALedgerNumber, ABatchNumber, out AVerifications, out MainDS))
            {
                return SubmitChanges(ref MainDS, out AVerifications);
            }

            return false;
        }

        /// <summary>
        /// cancel a GL Batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool CancelGLBatch(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();

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

            DBAccess.GDBAccessObj.RollbackTransaction();


            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
            NewRow.LedgerNumber = ALedgerNumber;
            MainDS.ALedger[0].LastBatchNumber++;
            NewRow.BatchNumber = MainDS.ALedger[0].LastBatchNumber;
            NewRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
            MainDS.ABatch.Rows.Add(NewRow);

            TVerificationResultCollection VerificationResult;

            if (GLBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
            {
                MainDS.AcceptChanges();
            }

            return MainDS;
        }
    }
}