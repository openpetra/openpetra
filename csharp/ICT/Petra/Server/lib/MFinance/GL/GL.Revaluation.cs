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
        /// A single call of this routine creates a batch, a journal and 2 transactions
        /// for each account number/cost center combination that holds a foreign currency value
        /// </summary>
        /// <param name="ALedgerNum">Number of the Ledger to be revaluated</param>
        /// <param name="AForeignAccount">Account Codes (Array) of the selected foreign currency accounts</param>
        /// <param name="AForeignCurrency">Array of the 'foreign' currencies</param>
        /// <param name="ANewExchangeRate">Matching array of the exchange rates</param>
        /// <param name="ACostCentre">Which Cost Centre should win / lose money in this process</param>
        /// <param name="glBatchNumber">If a batch was generated, the caller should print it.</param>
        /// <param name="AVerificationResult">A TVerificationResultCollection for possible error messages</param>
        /// <returns>true if a forex batch was posted.</returns>
        [RequireModulePermission("FINANCE-2")]
        public static bool Revaluate(
            int ALedgerNum,
            string[] AForeignAccount,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            String ACostCentre,
            out Int32 glBatchNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            CLSRevaluation revaluation = new CLSRevaluation(ALedgerNum,
                AForeignAccount, AForeignCurrency, ANewExchangeRate, ACostCentre);

            bool blnReturn = revaluation.RunRevaluation(out glBatchNumber);

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
        private string[] F_ForeignAccount;
        private string[] F_ForeignCurrency;
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

        /// <summary>
        /// Constructor to initialize a variable set.
        /// </summary>
        public CLSRevaluation(int ALedgerNum,
            string[] AForeignAccount,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            String ACostCentre)
        {
            F_LedgerNum = ALedgerNum;
            F_ForeignAccount = AForeignAccount;
            F_ForeignCurrency = AForeignCurrency;
            F_ExchangeRate = ANewExchangeRate;
            F_CostCentre = ACostCentre;
            FVerificationCollection = new TVerificationResultCollection();
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
        /// Returns true if a Reval batch was posted.
        /// </summary>
        public Boolean RunRevaluation(out Int32 glBatchNumber)
        {
            glBatchNumber = -1;
            try
            {
                TLedgerInfo ledger = new TLedgerInfo(F_LedgerNum);
                F_BaseCurrency = ledger.BaseCurrency;
                F_BaseCurrencyDigits = new TCurrencyInfo(F_BaseCurrency).digits;
                F_RevaluationAccCode = ledger.RevaluationAccount;
                F_FinancialYear = ledger.CurrentFinancialYear;
                F_AccountingPeriod = ledger.CurrentPeriod;

                TDBTransaction Transaction = null;

                AGeneralLedgerMasterTable GlmTable = new AGeneralLedgerMasterTable();
                AGeneralLedgerMasterRow glmTemplate = (AGeneralLedgerMasterRow)GlmTable.NewRowTyped(false);
                Boolean transactionsWereCreated = false;

                glmTemplate.LedgerNumber = F_LedgerNum;
                glmTemplate.Year = F_FinancialYear;

                for (Int32 i = 0; i < F_ForeignAccount.Length; i++)
                {
                    glmTemplate.AccountCode = F_ForeignAccount[i];
                    DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                        ref Transaction,
                        delegate
                        {
                            GlmTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(glmTemplate, Transaction);
                        });

                    if (GlmTable.Rows.Count > 0)
                    {
                        transactionsWereCreated |= RevaluateAccount(GlmTable, F_ExchangeRate[i], F_ForeignCurrency[i]);
                    }
                }

                Boolean batchPostedOK = true;

                if (transactionsWereCreated)
                {
                    batchPostedOK = CloseRevaluationAccountingBatch(out glBatchNumber);
                }

                if (batchPostedOK)
                {
                    if (!transactionsWereCreated) // If no transactions were needed, I'll just advise the user:
                    {
                        FVerificationCollection.Add(new TVerificationResult(
                                "Post Forex Batch",
                                "Exchange rates are unchanged - no revaluation was required.",
                                TResultSeverity.Resv_Status));
                    }

                    for (Int32 i = 0; i < F_ForeignAccount.Length; i++)
                    {
                        TLedgerInitFlag.RemoveFlagComponent(F_LedgerNum, MFinanceConstants.LEDGER_INIT_FLAG_REVAL, F_ForeignAccount[i]);
                    }
                }
                else
                {
                    FVerificationCollection.Add(new TVerificationResult(
                            "Post Forex Batch",
                            "The Revaluation Batch could not be posted.",
                            TResultSeverity.Resv_Critical));
                }

                return batchPostedOK;
            }
            catch (EVerificationException terminate)
            {
                FVerificationCollection = terminate.ResultCollection();
                return false;
            }
        } // Run Revaluation

        private Boolean RevaluateAccount(AGeneralLedgerMasterTable AGlmTbl, decimal AExchangeRate, string ACurrencyCode)
        {
            Boolean transactionsWereCreated = false;

            foreach (AGeneralLedgerMasterRow glmRow in AGlmTbl.Rows)
            {
                AGeneralLedgerMasterPeriodTable glmpTbl = null;

                TDBTransaction transaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                    delegate
                    {
                        glmpTbl = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(glmRow.GlmSequence, F_AccountingPeriod, transaction);
                    });

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
                        transactionsWereCreated = true;
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
                        TLedgerInitFlag.RemoveFlagComponent(F_LedgerNum, MFinanceConstants.LEDGER_INIT_FLAG_REVAL, glmRow.AccountCode);
                    }
                }
                catch (EVerificationException terminate)
                {
                    FVerificationCollection = terminate.ResultCollection();
                }
            } // foreach

            return transactionsWereCreated;
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
            TransactionRow.SystemGenerated = false;  // Setting true will prohibit reversal of this batch

            F_GLDataset.ATransaction.Rows.Add(TransactionRow);
        }

        /// Returns true if it seems to be OK.
        private Boolean CloseRevaluationAccountingBatch(out Int32 glBatchNumber)
        {
            Boolean blnReturnValue = false;

            glBatchNumber = -1;

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

                if (blnReturnValue)
                {
                    glBatchNumber = F_batch.BatchNumber;
                }
            }

            return blnReturnValue;
        } // Close Revaluation Accounting Batch

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