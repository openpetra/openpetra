//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
using System;
using System.Data;
using System.Text.RegularExpressions;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Collections.Specialized;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    /// <summary>
    /// Description of GL_Revaluation.
    /// </summary>
    public class TRevaluationWebConnector
    {
        /// <summary>
        /// Main Revaluate Routine!
        /// A single call of this routine creates a batch, a journal and a twin set of transactions
        /// for each account number - cost center combination which holds a foreign currency value
        /// </summary>
        /// <param name="ALedgerNum">Number of the Ledger to be revaluated</param>
        /// <param name="AAccoutingPeriod">Number of the accounting period (other form of the date)</param>
        /// <param name="AForeignCurrency">Types (Array) of the foreign currency account</param>
        /// <param name="ANewExchangeRate">Array of the exchange rates</param>
        /// <param name="AVerificationResult">A TVerificationResultCollection for possible error messages</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool Revaluate(
            int ALedgerNum,
            int AAccoutingPeriod,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            out TVerificationResultCollection AVerificationResult)
        {
            CLSRevaluation revaluation = new CLSRevaluation(ALedgerNum, AAccoutingPeriod,
                AForeignCurrency, ANewExchangeRate);

            bool blnReturn = revaluation.RunRevaluation();

            AVerificationResult = revaluation.VerificationResultCollection;
            return blnReturn;
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Main Revaluation Class. The routine is too complex for a linear program.
    /// </summary>
    public class CLSRevaluation
    {
        private int F_LedgerNum;
        private int F_AccountingPeriod;
        private string[] F_CurrencyCode;
        private decimal[] F_ExchangeRate;

        private string F_BaseCurrency;
        private int F_BaseCurrencyDigits;
        private string F_RevaluationAccCode;
        private int F_FinancialYear;
        private GLBatchTDS F_GLDataset = null;
        private ABatchRow F_batch;
        private AJournalRow F_journal;


        string strStatusContent = Catalog.GetString("Revaluation ...");

        TVerificationResultCollection verificationCollection = new TVerificationResultCollection();
        TResultSeverity F_resultSeverity = TResultSeverity.Resv_Noncritical;


        /// <summary>
        /// Constructor to initialize a variable set.
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AAccoutingPeriod"></param>
        /// <param name="AForeignCurrency"></param>
        /// <param name="ANewExchangeRate"></param>
        public CLSRevaluation(int ALedgerNum,
            int AAccoutingPeriod,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate)
        {
            F_LedgerNum = ALedgerNum;
//            F_AccountingPeriod = AAccoutingPeriod; // Ignoring what the client asked for, I'm going to revaluate the current period!
            F_CurrencyCode = AForeignCurrency;
            F_ExchangeRate = ANewExchangeRate;
        }

        /// <summary>
        ///
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection {
            get
            {
                return verificationCollection;
            }
        }


        /// <summary>
        /// Run the revaluation and set the flag for the ledger
        /// </summary>
        public bool RunRevaluation()
        {
            try
            {
                TLedgerInfo gli = new TLedgerInfo(F_LedgerNum);
                F_BaseCurrency = gli.BaseCurrency;
                F_BaseCurrencyDigits = new TCurrencyInfo(F_BaseCurrency).digits;
                F_RevaluationAccCode = gli.RevaluationAccount;
                F_FinancialYear = gli.CurrentFinancialYear;
                F_AccountingPeriod = gli.CurrentPeriod;
                RunRevaluationIntern();

                if (F_resultSeverity != TResultSeverity.Resv_Critical)
                {
                    new TLedgerInitFlagHandler(F_LedgerNum,
                        TLedgerInitFlagEnum.Revaluation).Flag = true;
                }
            }
            catch (EVerificationException terminate)
            {
                verificationCollection = terminate.ResultCollection();
            }
            return F_resultSeverity == TResultSeverity.Resv_Critical;
        }

        private void RunRevaluationIntern()
        {
            Boolean NewTransaction;
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
            AAccountTable accountTable = new AAccountTable();

            AAccountRow accountTemplate = (AAccountRow)accountTable.NewRowTyped(false);

            accountTemplate.LedgerNumber = F_LedgerNum;
            accountTemplate.AccountActiveFlag = true;
            accountTemplate.ForeignCurrencyFlag = true;
            accountTable = AAccountAccess.LoadUsingTemplate(accountTemplate, DBTransaction);

            AGeneralLedgerMasterTable glmTable = new AGeneralLedgerMasterTable();
            AGeneralLedgerMasterRow glmTemplate = (AGeneralLedgerMasterRow)glmTable.NewRowTyped(false);
            glmTemplate.LedgerNumber = F_LedgerNum;
            glmTemplate.Year = F_FinancialYear;
            glmTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(glmTemplate, DBTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (accountTable.Rows.Count == 0) // not using any foreign accounts?
            {
                return;
            }

            for (int iCnt = 0; iCnt < accountTable.Rows.Count; ++iCnt)
            {
                AAccountRow accountRow = (AAccountRow)accountTable[iCnt];

                for (int kCnt = 0; kCnt < F_CurrencyCode.Length; ++kCnt)
                {
                    // AForeignCurrency[] and ANewExchangeRate[] shall support a value
                    // for this account resp. for the currency of the account
                    if (accountRow.ForeignCurrencyCode.Equals(F_CurrencyCode[kCnt]))
                    {
                        glmTable.DefaultView.RowFilter = "a_account_code_c = '" + accountRow.AccountCode + "'";

                        if (glmTable.DefaultView.Count > 0)
                        {
                            RevaluateAccount(glmTable.DefaultView, F_ExchangeRate[kCnt]);
                        }
                    }
                }
            }

            CloseRevaluationAccountingBatch();
        }

        private void RevaluateAccount(DataView GLMView, decimal AExchangeRate)
        {
            foreach (DataRowView RowView in GLMView)
            {
                AGeneralLedgerMasterRow glmRow = (AGeneralLedgerMasterRow)RowView.Row;
                ACostCentreTable tempTbl = ACostCentreAccess.LoadByPrimaryKey(F_LedgerNum, glmRow.CostCentreCode, null);

                if (tempTbl.Rows.Count == 0)
                {
                    continue; // I really don't expect this, but if it does happen, this will prevent a crash!
                }

                ACostCentreRow tempRow = tempTbl[0];

                if (!tempRow.PostingCostCentreFlag)
                {
                    continue; // I do expect this - many rows are not "posting" cost centres.
                }

                try
                {
                    AGeneralLedgerMasterPeriodTable glmpTbl = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(glmRow.GlmSequence,
                        F_AccountingPeriod,
                        null);

                    if (glmpTbl.Rows.Count == 0)
                    {
                        continue; // I really don't expect this, but if it does happen, this will prevent a crash!
                    }

                    AGeneralLedgerMasterPeriodRow glmpRow = glmpTbl[0];

                    //
                    // If ActualForeign has not been set, I can't allow the ORM to even attempt to access them:
                    // (If ActualForeign is NULL, that's probably a fault, but this has occured in historical data.)

                    if (glmpRow.IsActualBaseNull() || glmpRow.IsActualForeignNull())
                    {
                        continue;
                    }

                    decimal delta = AccountDelta(glmpRow.ActualBase,
                        glmpRow.ActualForeign,
                        AExchangeRate,
                        F_BaseCurrencyDigits);

                    if (delta != 0)
                    {
                        // Now we have the relevant Cost Centre ...
                        RevaluateCostCentre(glmRow.AccountCode, glmRow.CostCentreCode, delta);
                    }
                    else
                    {
                        string strMessage = String.Format(
                            Catalog.GetString("The account {1}:{0} does not require revaluation."),
                            glmRow.AccountCode,
                            glmRow.CostCentreCode,
                            AExchangeRate);
                        verificationCollection.Add(new TVerificationResult(
                                strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                    }
                }
                catch (EVerificationException terminate)
                {
                    verificationCollection = terminate.ResultCollection();
                }
                catch (DivideByZeroException)
                {
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, Catalog.GetString("DivideByZeroException"), TResultSeverity.Resv_Noncritical));
                }
                catch (OverflowException)
                {
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, Catalog.GetString("OverflowException"), TResultSeverity.Resv_Noncritical));
                }
            }
        }

        private void RevaluateCostCentre(string ARelevantAccount, string ACostCentre, decimal Adelta)
        {
            // In the very first run Batch and Journal shall be created ...
            if (F_GLDataset == null)
            {
                InitBatchAndJournal();
            }

            string strMessage;
            bool blnDebitFlag;

            if (Adelta > 0)
            {
                strMessage = Catalog.GetString("Gain on foreign account {0}, cost centre {1}");
                blnDebitFlag = true;
            }
            else
            {
                strMessage = Catalog.GetString("Loss on foreign account {0}, cost centre {1}");
                blnDebitFlag = false;
            }

            Adelta = Math.Abs(Adelta);

            strMessage = String.Format(strMessage, ARelevantAccount, ACostCentre);

            CreateTransaction(strMessage, F_RevaluationAccCode, !blnDebitFlag, ACostCentre, Adelta);
            CreateTransaction(strMessage, ARelevantAccount, blnDebitFlag, ACostCentre, Adelta);
            F_journal.JournalDebitTotal = F_journal.JournalDebitTotal + Adelta;
            F_journal.JournalCreditTotal = F_journal.JournalCreditTotal + Adelta;
        }

        private void InitBatchAndJournal()
        {
            F_GLDataset = TGLPosting.CreateABatch(F_LedgerNum);
            F_batch = F_GLDataset.ABatch[0];
            F_batch.BatchDescription = Catalog.GetString("Period end revaluations");

            TAccountPeriodInfo accountingPeriodInfo = new TAccountPeriodInfo(F_LedgerNum);
            accountingPeriodInfo.AccountingPeriodNumber = F_batch.BatchPeriod;
            F_batch.DateEffective = accountingPeriodInfo.PeriodEndDate;

            F_batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            F_journal = F_GLDataset.AJournal.NewRowTyped();
            F_journal.LedgerNumber = F_batch.LedgerNumber;
            F_journal.BatchNumber = F_batch.BatchNumber;
            F_journal.JournalNumber = 1;
            F_journal.DateEffective = F_batch.DateEffective;
            F_journal.JournalPeriod = F_batch.BatchPeriod;
            F_journal.TransactionCurrency = F_BaseCurrency;
            F_journal.JournalDescription = F_batch.BatchDescription;
            F_journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            F_journal.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            F_journal.LastTransactionNumber = 0;
            F_journal.DateOfEntry = DateTime.Now;
            F_journal.ExchangeRateToBase = 1.0M;
            F_GLDataset.AJournal.Rows.Add(F_journal);
        }

        private void CreateTransaction(string AMessage, string AAccount,
            bool ADebitFlag, string ACostCenter, decimal Aamount)
        {
            ATransactionRow transaction = null;

            transaction = F_GLDataset.ATransaction.NewRowTyped();
            transaction.LedgerNumber = F_journal.LedgerNumber;
            transaction.BatchNumber = F_journal.BatchNumber;
            transaction.JournalNumber = F_journal.JournalNumber;
            transaction.TransactionNumber = ++F_journal.LastTransactionNumber;
            transaction.AccountCode = AAccount;
            transaction.CostCentreCode = ACostCenter;
            transaction.Narrative = AMessage;
            transaction.Reference = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            transaction.DebitCreditIndicator = ADebitFlag;
            transaction.AmountInBaseCurrency = Aamount;
            transaction.TransactionAmount = Aamount;
            transaction.TransactionDate = F_batch.DateEffective;

            F_GLDataset.ATransaction.Rows.Add(transaction);
        }

        private void CloseRevaluationAccountingBatch()
        {
            if (F_GLDataset != null)
            {
                F_batch.BatchCreditTotal = F_journal.JournalCreditTotal;
                F_batch.BatchDebitTotal = F_journal.JournalDebitTotal;
                TVerificationResultCollection AVerifications;
                bool blnReturnValue = (TGLTransactionWebConnector.SaveGLBatchTDS(
                                           ref F_GLDataset, out AVerifications) == TSubmitChangesResult.scrOK);
                F_GLDataset.AcceptChanges();

                if (blnReturnValue)
                {
                    //blnVerificationCollectionContainsData = true;
                }

                blnReturnValue = (TGLTransactionWebConnector.PostGLBatch(
                                      F_batch.LedgerNumber, F_batch.BatchNumber, out AVerifications));
            }
        }

        private void AddVerificationResultMessage(
            string AResultContext, string AResultText, string ALocalCode, TResultSeverity AResultSeverity)
        {
            verificationCollection.Add(new TVerificationResult(
                    AResultContext, AResultText, "REVAL", "REVAL:" + ALocalCode, AResultSeverity));

            if (AResultSeverity == TResultSeverity.Resv_Critical)
            {
                F_resultSeverity = TResultSeverity.Resv_Critical;
            }
        }

        /// <summary>
        /// In order to be able to use a unit test for the calculation, it is public ...
        /// </summary>
        /// <param name="AAmountInBaseCurency">Available account value in base currency units</param>
        /// <param name="AAmountInForeignCurrency">Available account value in foreign currency units</param>
        /// <param name="AExchangeRate">The exchange rate which shall be realized after the
        /// accounting has been done</param>
        /// <param name="ACurrencyDigits">Number of rounding digits</param>
        /// <returns></returns>
        public static decimal AccountDelta(decimal AAmountInBaseCurency,
            decimal AAmountInForeignCurrency,
            decimal AExchangeRate, int ACurrencyDigits)
        {
            return Math.Round((AAmountInForeignCurrency / AExchangeRate), ACurrencyDigits) -
                   AAmountInBaseCurency;
        }
    }
}