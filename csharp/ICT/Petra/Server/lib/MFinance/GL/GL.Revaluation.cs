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
        /// <param name="AAccoutingPeriod">Number of the accounting period
        /// (other form of the date)</param>
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
        private int intLedgerNum;
        private int intAccountingPeriod;
        private string[] strArrForeignCurrencyType;
        private decimal[] decArrExchangeRate;

        private string strBaseCurrencyType;
        private string strRevaluationAccount;

        private int intPtrToForeignData;

        decimal decDelta;

        private GLBatchTDS GLDataset = null;
        private ABatchRow batch;
        private AJournalRow journal;


        string strStatusContent = Catalog.GetString("Revaluation ...");

        TVerificationResultCollection verificationCollection = new TVerificationResultCollection();
        TResultSeverity resultSeverity = TResultSeverity.Resv_Noncritical;


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
            intLedgerNum = ALedgerNum;
            intAccountingPeriod = AAccoutingPeriod;
            strArrForeignCurrencyType = AForeignCurrency;
            decArrExchangeRate = ANewExchangeRate;
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
        /// run the revaluation and set the flag for the ledger
        /// </summary>
        public bool RunRevaluation()
        {
            try
            {
                TLedgerInfo gli = new TLedgerInfo(intLedgerNum);
                strBaseCurrencyType = gli.BaseCurrency;
                strRevaluationAccount = gli.RevaluationAccount;
                RunRevaluationIntern();

                if (resultSeverity != TResultSeverity.Resv_Critical)
                {
                    new TLedgerInitFlagHandler(intLedgerNum,
                        TLedgerInitFlagEnum.Revaluation).Flag = true;
                }
            }
            catch (TVerificationException terminate)
            {
                verificationCollection = terminate.ResultCollection();
            }
            return resultSeverity == TResultSeverity.Resv_Critical;
        }

        private void RunRevaluationIntern()
        {
            AAccountTable accountTable =
                AAccountAccess.LoadViaALedger(intLedgerNum, null);
            AGeneralLedgerMasterTable generalLedgerMasterTable =
                AGeneralLedgerMasterAccess.LoadViaALedger(intLedgerNum, null);

            if (accountTable.Rows.Count == 0)
            {
                TVerificationException terminate = new TVerificationException(
                    Catalog.GetString(Catalog.GetString(
                            "No Entries in GeneralLedgerMasterTable")));
                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "001";
                throw terminate;
            }

            for (int iCnt = 0; iCnt < accountTable.Rows.Count; ++iCnt)
            {
                AAccountRow accountRow = (AAccountRow)accountTable[iCnt];

                // Account shall be active
                if (accountRow.AccountActiveFlag)
                {
                    // Account shall hold foreign Currency values
                    if (accountRow.ForeignCurrencyFlag)
                    {
                        for (int kCnt = 0; kCnt < strArrForeignCurrencyType.Length; ++kCnt)
                        {
                            intPtrToForeignData = kCnt;
                            bool blnFoundGlmEntry = false;

                            // AForeignCurrency[] and ANewExchangeRate[] shall support a value
                            // for this account resp. for the currency of the account
                            if (accountRow.ForeignCurrencyCode.Equals(strArrForeignCurrencyType[kCnt]))
                            {
                                for (int jCnt = 0; jCnt < generalLedgerMasterTable.Rows.Count; ++jCnt)
                                {
                                    AGeneralLedgerMasterRow generalLedgerMasterRow =
                                        (AGeneralLedgerMasterRow)generalLedgerMasterTable[jCnt];

                                    // generalLedgerMaster shall support Entries for this account
                                    if (generalLedgerMasterRow.AccountCode.Equals(accountRow.AccountCode))
                                    {
                                        // Account is localized ...
                                        RevaluateAccount(accountRow.AccountCode);
                                        blnFoundGlmEntry = true;
                                    }
                                }
                            }

                            if (!blnFoundGlmEntry)
                            {
                                string strMessage = Catalog.GetString(
                                    "The account {0} has no glm-entry and a revaluation is not necessary");
                                strMessage = String.Format(strMessage, accountRow.AccountCode);
                                verificationCollection.Add(new TVerificationResult(
                                        strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                            }
                        }
                    }
                    else
                    {
                        string strMessage = Catalog.GetString(
                            "The account {0} is not defined as foreign and a revaluation is not possible");
                        strMessage = String.Format(strMessage, accountRow.AccountCode);
                        verificationCollection.Add(new TVerificationResult(
                                strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                    }
                }
                else
                {
                    string strMessage = Catalog.GetString(
                        "The account {0} is not active and a revaluation is not possible");
                    strMessage = String.Format(strMessage, accountRow.AccountCode);
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                }
            }
        }

        private void RevaluateAccount(string ARelevantAccount)
        {
            AGeneralLedgerMasterTable generalLedgerMasterTable =
                AGeneralLedgerMasterAccess.LoadViaAAccount(intLedgerNum, ARelevantAccount, null);

            for (int iCnt = 0; iCnt < generalLedgerMasterTable.Rows.Count; ++iCnt)
            {
                AGeneralLedgerMasterRow generalLedgerMasterRow =
                    (AGeneralLedgerMasterRow)generalLedgerMasterTable[iCnt];

                try
                {
                    int intNoOfForeignDigts = new TCurrencyInfo(strBaseCurrencyType).digits;
                    decDelta = AccountDelta(generalLedgerMasterRow.YtdActualBase,
                        generalLedgerMasterRow.YtdActualForeign,
                        decArrExchangeRate[intPtrToForeignData],
                        intNoOfForeignDigts);

                    if (decDelta != 0)
                    {
                        // Now we have the relevant Cost Centre ...
                        RevaluateCostCentre(ARelevantAccount, generalLedgerMasterRow.CostCentreCode);
                    }
                    else
                    {
                        string strMessage = Catalog.GetString(
                            "The account {1}:{0} was allread valuated to {2}");
                        strMessage = String.Format(strMessage, ARelevantAccount,
                            generalLedgerMasterRow.CostCentreCode,
                            decArrExchangeRate[intPtrToForeignData]);
                        verificationCollection.Add(new TVerificationResult(
                                strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                    }
                }
                catch (TVerificationException terminate)
                {
                    verificationCollection = terminate.ResultCollection();
                }
                catch (DivideByZeroException)
                {
                    string strMessage = Catalog.GetString(
                        "DivideByZeroException");
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                }
                catch (OverflowException)
                {
                    string strMessage = Catalog.GetString(
                        "OverflowException");
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
                }
            }

            CloseRevaluationAccountingBatch();
        }

        private void RevaluateCostCentre(string ARelevantAccount, string ACostCentre)
        {
            // In the very first run Batch and Journal shall be created ...
            if (GLDataset == null)
            {
                InitBatchAndJournal();
            }

            string strMsgGain =
                Catalog.GetString("Gain on foreign account {0}, cost centre {1}");
            string strMsgLoss =
                Catalog.GetString("Loss on foreign account {0}, cost centre {1}");
            string strMessage;
            bool blnDebitFlag;

            if (decDelta > 0)
            {
                strMessage = strMsgGain;
                blnDebitFlag = true;
            }
            else
            {
                strMessage = strMsgLoss;
                blnDebitFlag = false;
            }

            decDelta = Math.Abs(decDelta);

            strMessage = String.Format(strMessage, ARelevantAccount, ACostCentre);

            CreateTransaction(strMessage, strRevaluationAccount, !blnDebitFlag, ACostCentre);
            CreateTransaction(strMessage, ARelevantAccount, blnDebitFlag, ACostCentre);
            journal.JournalDebitTotal = journal.JournalDebitTotal + decDelta;
            journal.JournalCreditTotal = journal.JournalCreditTotal + decDelta;
        }

        private void InitBatchAndJournal()
        {
            GLDataset = TGLPosting.CreateABatch(intLedgerNum);
            batch = GLDataset.ABatch[0];
            batch.BatchDescription = Catalog.GetString("Period end revaluations");

            TAccountPeriodInfo accountingPeriodInfo = new TAccountPeriodInfo(intLedgerNum);
            accountingPeriodInfo.AccountingPeriodNumber = intAccountingPeriod;
            batch.DateEffective = accountingPeriodInfo.PeriodEndDate;

            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            journal = GLDataset.AJournal.NewRowTyped();
            journal.LedgerNumber = batch.LedgerNumber;
            journal.BatchNumber = batch.BatchNumber;
            journal.JournalNumber = 1;
            journal.DateEffective = batch.DateEffective;
            journal.JournalPeriod = intAccountingPeriod;
            journal.TransactionCurrency = strBaseCurrencyType;
            journal.JournalDescription = batch.BatchDescription;
            journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            journal.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;
            journal.ExchangeRateToBase = 1.0M;
            GLDataset.AJournal.Rows.Add(journal);
        }

        private void CreateTransaction(string AMessage, string AAccount,
            bool ADebitFlag, string ACostCenter)
        {
            ATransactionRow transaction = null;

            transaction = GLDataset.ATransaction.NewRowTyped();
            transaction.LedgerNumber = journal.LedgerNumber;
            transaction.BatchNumber = journal.BatchNumber;
            transaction.JournalNumber = journal.JournalNumber;
            transaction.TransactionNumber = ++journal.LastTransactionNumber;
            transaction.AccountCode = AAccount;
            transaction.CostCentreCode = ACostCenter;
            transaction.Narrative = AMessage;
            transaction.Reference = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            transaction.DebitCreditIndicator = ADebitFlag;
            transaction.AmountInBaseCurrency = decDelta;
            transaction.TransactionAmount = 2;
            transaction.TransactionDate = batch.DateEffective;

            GLDataset.ATransaction.Rows.Add(transaction);
        }

        private void CloseRevaluationAccountingBatch()
        {
            if (GLDataset != null)
            {
                TVerificationResultCollection AVerifications;
                bool blnReturnValue = (TGLTransactionWebConnector.SaveGLBatchTDS(
                                           ref GLDataset, out AVerifications) == TSubmitChangesResult.scrOK);

                if (blnReturnValue)
                {
                    //blnVerificationCollectionContainsData = true;
                }

                blnReturnValue = (TGLTransactionWebConnector.PostGLBatch(
                                      batch.LedgerNumber, batch.BatchNumber, out AVerifications));
            }
        }

        private void AddVerificationResultMessage(
            string AResultContext, string AResultText, string ALocalCode, TResultSeverity AResultSeverity)
        {
            verificationCollection.Add(new TVerificationResult(
                    AResultContext, AResultText, "REVAL", "REVAL:" + ALocalCode, AResultSeverity));

            if (AResultSeverity == TResultSeverity.Resv_Critical)
            {
                resultSeverity = TResultSeverity.Resv_Critical;
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
            return AAmountInBaseCurency -
                   Math.Round((AAmountInForeignCurrency / AExchangeRate), ACurrencyDigits);
        }
    }
}