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
using System.Linq;
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
        /// <param name="AForeignCurrency">Types (Array) of the foreign currency account</param>
        /// <param name="ANewExchangeRate">Array of the exchange rates</param>
        /// <param name="ACostCentre">Which Cost Centre should win / lose money in this process</param>
        /// <param name="AVerificationResult">A TVerificationResultCollection for possible error messages</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool Revaluate(
            int ALedgerNum,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            String ACostCentre,
            out TVerificationResultCollection AVerificationResult)
        {
            CLSRevaluation revaluation = new CLSRevaluation(ALedgerNum,
                AForeignCurrency, ANewExchangeRate, ACostCentre);

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
        String F_CostCentre;

        private string F_BaseCurrency;
        private int F_BaseCurrencyDigits;
        private string F_RevaluationAccCode;
        private int F_FinancialYear;
        private GLBatchTDS F_GLDataset = null;
        private ABatchRow F_batch;
        private AJournalRow F_journal;


        string strStatusContent = Catalog.GetString("Revaluation");

        private TVerificationResultCollection FVerificationCollection;
        private TResultSeverity F_resultSeverity;


        /// <summary>
        /// Constructor to initialize a variable set.
        /// </summary>
        public CLSRevaluation(int ALedgerNum,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            String ACostCentre)
        {
            F_LedgerNum = ALedgerNum;
            F_CurrencyCode = AForeignCurrency.Distinct().ToArray();
            F_ExchangeRate = ANewExchangeRate;
            F_CostCentre = ACostCentre;
            FVerificationCollection = new TVerificationResultCollection();
            F_resultSeverity = TResultSeverity.Resv_Noncritical;
        }

        /// <summary>
        ///
        /// </summary>
        public TVerificationResultCollection VerificationResultCollection {
            get
            {
                return FVerificationCollection;
            }
        }


        /// <summary>
        /// Run the revaluation and set the flag for the ledger
        /// </summary>
        public Boolean RunRevaluation()
        {
            try
            {
                TLedgerInfo gli = new TLedgerInfo(F_LedgerNum);
                F_BaseCurrency = gli.BaseCurrency;
                F_BaseCurrencyDigits = new TCurrencyInfo(F_BaseCurrency).digits;
                F_RevaluationAccCode = gli.RevaluationAccount;
                F_FinancialYear = gli.CurrentFinancialYear;
                F_AccountingPeriod = gli.CurrentPeriod;

                if (!RunRevaluationIntern())
                {
                    return false;
                }

                if (F_resultSeverity != TResultSeverity.Resv_Critical)
                {
                    new TLedgerInitFlagHandler(F_LedgerNum,
                        TLedgerInitFlagEnum.Revaluation).Flag = true;
                }
            }
            catch (EVerificationException terminate)
            {
                FVerificationCollection = terminate.ResultCollection();
            }
            return F_resultSeverity == TResultSeverity.Resv_Critical;
        }

        private Boolean RunRevaluationIntern()
        {
            AAccountTable AccountTable = new AAccountTable();
            AGeneralLedgerMasterTable GlmTable = new AGeneralLedgerMasterTable();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    AAccountRow accountTemplate = (AAccountRow)AccountTable.NewRowTyped(false);

                    accountTemplate.LedgerNumber = F_LedgerNum;
                    accountTemplate.AccountActiveFlag = true;
                    accountTemplate.ForeignCurrencyFlag = true;
                    AccountTable = AAccountAccess.LoadUsingTemplate(accountTemplate, Transaction);

                    AGeneralLedgerMasterRow glmTemplate = (AGeneralLedgerMasterRow)GlmTable.NewRowTyped(false);
                    glmTemplate.LedgerNumber = F_LedgerNum;
                    glmTemplate.Year = F_FinancialYear;
                    GlmTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(glmTemplate, Transaction);
                });

            if (AccountTable.Rows.Count == 0) // not using any foreign accounts?
            {
                FVerificationCollection.Add(new TVerificationResult(
                        strStatusContent, Catalog.GetString("No foreign currency accounts are used in this ledger."), TResultSeverity.Resv_Status));
                return true;
            }

            for (int iCnt = 0; iCnt < AccountTable.Rows.Count; ++iCnt)
            {
                AAccountRow accountRow = (AAccountRow)AccountTable[iCnt];

                for (int kCnt = 0; kCnt < F_CurrencyCode.Length; ++kCnt)
                {
                    // AForeignCurrency[] and ANewExchangeRate[] shall support a value
                    // for this account resp. for the currency of the account
                    if (accountRow.ForeignCurrencyCode.Equals(F_CurrencyCode[kCnt]))
                    {
                        GlmTable.DefaultView.RowFilter = "a_account_code_c = '" + accountRow.AccountCode + "'";

                        if (GlmTable.DefaultView.Count > 0)
                        {
                            RevaluateAccount(GlmTable.DefaultView, F_ExchangeRate[kCnt], F_CurrencyCode[kCnt]);
                        }
                    }
                }
            }

            return CloseRevaluationAccountingBatch();
        }

        private void RevaluateAccount(DataView GLMView, decimal AExchangeRate, string ACurrencyCode)
        {
            foreach (DataRowView RowView in GLMView)
            {
                AGeneralLedgerMasterRow glmRow = (AGeneralLedgerMasterRow)RowView.Row;
                ACostCentreTable tempTbl = null;
                AGeneralLedgerMasterPeriodTable glmpTbl = null;

                TDBTransaction transaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                    delegate
                    {
                        tempTbl = ACostCentreAccess.LoadByPrimaryKey(F_LedgerNum, glmRow.CostCentreCode, transaction);
                        glmpTbl = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(glmRow.GlmSequence, F_AccountingPeriod, transaction);
                    });

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
                        RevaluateCostCentre(glmRow.AccountCode, glmRow.CostCentreCode, delta, AExchangeRate, ACurrencyCode);
                    }
                    else
                    {
                        string strMessage = String.Format(
                            Catalog.GetString("The account {1}:{0} does not require revaluation."),
                            glmRow.AccountCode,
                            glmRow.CostCentreCode,
                            AExchangeRate);
                        FVerificationCollection.Add(new TVerificationResult(
                                strStatusContent, strMessage, TResultSeverity.Resv_Status));
                    }
                }
                catch (EVerificationException terminate)
                {
                    FVerificationCollection = terminate.ResultCollection();
                }
            } // foreach

        }

        private void RevaluateCostCentre(string ARelevantAccount, string ACostCentre, decimal Adelta, decimal AExchangeRate, string ACurrencyCode)
        {
            // In the very first run Batch and Journal shall be created ...
            if (F_GLDataset == null)
            {
                InitBatchAndJournal(AExchangeRate, ACurrencyCode);
            }

            string strMessage;
            bool blnDebitFlag;

            if (Adelta > 0)
            {
                strMessage = Catalog.GetString("Gain on foreign account {0}, cost centre {1}. Reval Rate {2:G6}");
                blnDebitFlag = true;
            }
            else
            {
                strMessage = Catalog.GetString("Loss on foreign account {0}, cost centre {1}. Reval Rate {2:G6}");
                blnDebitFlag = false;
            }

            Adelta = Math.Abs(Adelta);

            strMessage = String.Format(strMessage, ARelevantAccount, ACostCentre, AExchangeRate);

            CreateTransaction(strMessage, F_RevaluationAccCode, !blnDebitFlag, F_CostCentre, Adelta);
            CreateTransaction(strMessage, ARelevantAccount, blnDebitFlag, ACostCentre, Adelta);
            F_journal.JournalDebitTotal += Adelta;
            F_journal.JournalCreditTotal += Adelta;
        }

        private void InitBatchAndJournal(decimal AExchangeRate, string ACurrencyCode)
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
            F_journal.ExchangeRateTime = 14400;             // revaluations are typically later than 'new rates'
            F_journal.JournalPeriod = F_batch.BatchPeriod;
            F_journal.TransactionCurrency = F_BaseCurrency;
            F_journal.JournalDescription = F_batch.BatchDescription;
            F_journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            F_journal.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            F_journal.LastTransactionNumber = 0;
            F_journal.DateOfEntry = DateTime.Now;
            F_journal.ExchangeRateToBase = 1.0M;
            F_GLDataset.AJournal.Rows.Add(F_journal);

            ARevaluationRow revalRow = F_GLDataset.ARevaluation.NewRowTyped();
            revalRow.LedgerNumber = F_journal.LedgerNumber;
            revalRow.BatchNumber = F_journal.BatchNumber;
            revalRow.JournalNumber = F_journal.JournalNumber;
            revalRow.ExchangeRateToBase = AExchangeRate;
            revalRow.RevaluationCurrency = ACurrencyCode;
            F_GLDataset.ARevaluation.Rows.Add(revalRow);
        }

        private void CreateTransaction(string AMessage, string AAccount,
            bool ADebitFlag, string ACostCentre, decimal Aamount)
        {
            ATransactionRow TransactionRow = null;

            TransactionRow = F_GLDataset.ATransaction.NewRowTyped();
            TransactionRow.LedgerNumber = F_journal.LedgerNumber;
            TransactionRow.BatchNumber = F_journal.BatchNumber;
            TransactionRow.JournalNumber = F_journal.JournalNumber;
            TransactionRow.TransactionNumber = ++F_journal.LastTransactionNumber;
            TransactionRow.AccountCode = AAccount;
            TransactionRow.CostCentreCode = ACostCentre;
            TransactionRow.Narrative = AMessage;
            TransactionRow.Reference = "FX REVAL";
            TransactionRow.DebitCreditIndicator = ADebitFlag;
            TransactionRow.AmountInBaseCurrency = Aamount;
            TransactionRow.TransactionAmount = Aamount;
            TransactionRow.TransactionDate = F_batch.DateEffective;

            F_GLDataset.ATransaction.Rows.Add(TransactionRow);
        }

        private Boolean CloseRevaluationAccountingBatch()
        {
            Boolean blnReturnValue = false;

            if (F_GLDataset != null)
            {
                F_batch.BatchCreditTotal = F_journal.JournalCreditTotal;
                F_batch.BatchDebitTotal = F_journal.JournalDebitTotal;
                TVerificationResultCollection AVerifications;
                blnReturnValue = (TGLTransactionWebConnector.SaveGLBatchTDS(
                                      ref F_GLDataset, out AVerifications) == TSubmitChangesResult.scrOK);

                if (!blnReturnValue)
                {
                    return false;
                }

                F_GLDataset.AcceptChanges();

                blnReturnValue = (TGLTransactionWebConnector.PostGLBatch(
                                      F_batch.LedgerNumber, F_batch.BatchNumber, out AVerifications));
            }

            return blnReturnValue;
        }

        private void AddVerificationResultMessage(
            string AResultContext, string AResultText, string ALocalCode, TResultSeverity AResultSeverity)
        {
            FVerificationCollection.Add(new TVerificationResult(
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
        /// <param name="AExchangeRate">The exchange rate which shall be realized after the accounting has been done</param>
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