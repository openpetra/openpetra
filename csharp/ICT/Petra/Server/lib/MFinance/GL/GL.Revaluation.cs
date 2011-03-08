//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;

using Ict.Petra.Server.MFinance.GL.WebConnectors;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    /// <summary>
    /// Description of GL_Revaluation.
    /// </summary>
    public class TGLRevaluation
    {
        /// <summary>
        /// Main Revalutate Routine!
        /// A single call of this routine creates a batch, a journal and a twin set of transactions
        /// for each account number - cost center combination which holds a foreign currency value
        /// </summary>
        /// <param name="ALedgerNum">Number of the Ledger to be revaluated</param>
        /// <param name="ABaseCurrencyType">Type of the base currency EUR USD or else</param>
        /// <param name="ARevaluationAccount">Offset Account for the revaluation</param>
        /// <param name="ARevaluationCostCenter">Cost Center for the revaluation</param>
        /// <param name="AForeignCurrency">Types (Array) of the foreign currency account</param>
        /// <param name="ANewExchangeRate">Array of the exchange rates</param>
        /// <returns></returns>

        public static Boolean Revaluate(int ALedgerNum,
            string ABaseCurrencyType,
            string ARevaluationAccount,
            string ARevaluationCostCenter,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate)
        {

        	DateTime dte = new GetAccountingPeriodInfo(ALedgerNum).GetDatePeriodStart(12);
            System.Diagnostics.Debug.WriteLine(dte.ToLongDateString());


            AAccountTable accountTable =
                AAccountAccess.LoadViaALedger(ALedgerNum, null);
            AGeneralLedgerMasterTable generalLedgerMasterTable =
                AGeneralLedgerMasterAccess.LoadViaALedger(ALedgerNum, null);

            for (int iCnt = 0; iCnt < accountTable.Rows.Count; ++iCnt)
            {
                AAccountRow accountRow = (AAccountRow)accountTable[iCnt];

                // Account shall be active
                if (accountRow.AccountActiveFlag)
                {
                    // Account shall hold foreign Currency values
                    if (accountRow.ForeignCurrencyFlag)
                    {
                        string strAccountCode = accountRow.AccountCode;

                        for (int kCnt = 0; kCnt < AForeignCurrency.Length; ++kCnt)
                        {
                            // AForeignCurrency[] and ANewExchangeRate[] shall support a value
                            // for this account resp. for the currency of the account
                            if (accountRow.ForeignCurrencyCode.Equals(AForeignCurrency[kCnt]))
                            {
                                for (int jCnt = 0; jCnt < generalLedgerMasterTable.Rows.Count; ++jCnt)
                                {
                                    AGeneralLedgerMasterRow generalLedgerMasterRow =
                                        (AGeneralLedgerMasterRow)generalLedgerMasterTable[jCnt];

                                    // generalLedgerMaster shall support Entries for this account
                                    if (generalLedgerMasterRow.AccountCode.Equals(strAccountCode))
                                    {
                                        RevaluateAccount(ALedgerNum, ABaseCurrencyType,
                                            ARevaluationAccount, ARevaluationCostCenter,
                                            AForeignCurrency[kCnt], ANewExchangeRate[kCnt],
                                            accountRow.AccountCode);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        private static void RevaluateAccount(int ALedgerNum,
            string ABaseCurrencyType,
            string ARevaluationAccount,
            string ARevaluationCostCenter,
            string AForeignCurrency,
            decimal ANewExchangeRate,
            string ARelevantAccount)
        {
            AGeneralLedgerMasterTable generalLedgerMasterTable =
                AGeneralLedgerMasterAccess.LoadViaAAccount(ALedgerNum, ARelevantAccount, null);

            for (int iCnt = 0; iCnt < generalLedgerMasterTable.Rows.Count; ++iCnt)
            {
                AGeneralLedgerMasterRow generalLedgerMasterRow =
                    (AGeneralLedgerMasterRow)generalLedgerMasterTable[iCnt];

                // Ytd ... shall not be zero otherwise a revaluation is senseless
                if (generalLedgerMasterRow.YtdActualForeign != 0)
                {
                    // Now we have the relevant Cost Center ...
                    //RevaluateCostCenter(
                    System.Diagnostics.Debug.WriteLine("Cost Center   : " + generalLedgerMasterRow.CostCentreCode);
                }
            }
        }

        private static void RevaluateCostCenter(int ALedgerNum,
            string ABaseCurrencyType,
            string ARevaluationAccount,
            string ARevaluationCostCenter,
            string AForeignCurrency,
            decimal ANewExchangeRate,
            string ARelevantAccount,
            string ACostCenter)
        {
        }

        private static void CreateBatch(int ALedgerNumber)
        {
            GLBatchTDS GLDataset = TTransactionWebConnector.CreateABatch(ALedgerNumber);
            ABatchRow batch = GLDataset.ABatch[0];

            batch.BatchDescription = Catalog.GetString("Period end revaluations");
            //batch.DateEffective = giftbatch.GlEffectiveDate;

            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // #######################################################
            AJournalRow journal = GLDataset.AJournal.NewRowTyped();
            journal.LedgerNumber = batch.LedgerNumber;
            journal.BatchNumber = batch.BatchNumber;
            journal.JournalNumber = 1;
            journal.DateEffective = batch.DateEffective;
            //journal.JournalPeriod = giftbatch.BatchPeriod;
            //journal.TransactionCurrency = giftbatch.CurrencyCode;
            journal.JournalDescription = batch.BatchDescription;
            journal.TransactionTypeCode = MFinanceConstants.TRANSACTION_FX_REVAL;
            journal.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;

            journal.ExchangeRateToBase = 1.0M;
            GLDataset.AJournal.Rows.Add(journal);
            // #######################################################

            ATransactionRow transaction = null;

            transaction = GLDataset.ATransaction.NewRowTyped();
            transaction.LedgerNumber = journal.LedgerNumber;
            transaction.BatchNumber = journal.BatchNumber;
            transaction.JournalNumber = journal.JournalNumber;
            transaction.TransactionNumber = ++journal.LastTransactionNumber;
            //transaction.AccountCode = giftdetail.AccountCode;
            //transaction.CostCentreCode = giftdetail.CostCentreCode;
            //transaction.Narrative = "GB - Gift Batch " + giftbatch.BatchNumber.ToString();
            //transaction.Reference = "GB" + giftbatch.BatchNumber.ToString();
            transaction.DebitCreditIndicator = false;
            transaction.TransactionAmount = 0;
            transaction.AmountInBaseCurrency = 0;
            //transaction.TransactionDate = giftbatch.GlEffectiveDate;

            GLDataset.ATransaction.Rows.Add(transaction);
        }

        /// <summary>
        /// This example shows how to invoke the Revaluate Method.
        /// </summary>
        public TGLRevaluation()
        {
            string[] currencies = new string[2];
            currencies[0] = "GBP";
            currencies[1] = "YEN";
            decimal[] rates = new decimal[2];
            rates[0] = 1.234m;
            rates[1] = 2.345m;
            Revaluate(43, "EUR", "5300", "3700", currencies, rates);
        }
    }

    public class GetAccountingPeriodInfo
    {
        private AAccountingPeriodTable periodTable;

        public GetAccountingPeriodInfo(int ALedgerNumber)
        {
            periodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, null);
        }

        public DateTime GetEffectiveDateOfPeriod(int APeriodNum)
        {
            try
            {
                AAccountingPeriodRow periodRow =
                    (AAccountingPeriodRow)periodTable[APeriodNum];
                return periodRow.EffectiveDate;
            }
            catch (System.IndexOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
        
        public DateTime GetDatePeriodEnd(int APeriodNum)
        {
            try
            {
                AAccountingPeriodRow periodRow =
                    (AAccountingPeriodRow)periodTable[APeriodNum];
                return periodRow.PeriodEndDate;
            }
            catch (System.IndexOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }

        public DateTime GetDatePeriodStart(int APeriodNum)
        {
            try
            {
                AAccountingPeriodRow periodRow =
                    (AAccountingPeriodRow)periodTable[APeriodNum];
                return periodRow.PeriodStartDate;
            }
            catch (System.IndexOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }
    }
}


//				System.Diagnostics.Debug.WriteLine("#########################################################");
//				System.Diagnostics.Debug.WriteLine("Ledger        : " + ALedgerNum);
//				System.Diagnostics.Debug.WriteLine("Account       : " + ARelevantAccount);
//				System.Diagnostics.Debug.WriteLine("Cost Center   : " + generalLedgerMasterRow.CostCentreCode);
//				System.Diagnostics.Debug.WriteLine("Base Currency : " +
//				                                   generalLedgerMasterRow.YtdActualBase + "[" + ABaseCurrencyType + "]");
//				System.Diagnostics.Debug.WriteLine("For. Currency : " +
//				                                   generalLedgerMasterRow.YtdActualForeign + "[" + AForeignCurrency + "]");
//				System.Diagnostics.Debug.WriteLine("Revaluation   : " + ARevaluationCostCenter + ":" + ARevaluationAccount);
//				System.Diagnostics.Debug.WriteLine("Exchange-Rate : " + ANewExchangeRate.ToString());
//				System.Diagnostics.Debug.WriteLine("#########################################################");