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
using System.Text.RegularExpressions;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
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
        /// Main Revalutate Routine!
        /// A single call of this routine creates a batch, a journal and a twin set of transactions
        /// for each account number - cost center combination which holds a foreign currency value
        /// </summary>
        /// <param name="ALedgerNum">Number of the Ledger to be revaluated</param>
        /// <param name="AAccoutingPeriod">Number of the accouting perdiod
        /// (other form of the date)</param>
        /// <param name="ARevaluationCostCenter">Cost Center for the revaluation</param>
        /// <param name="AForeignCurrency">Types (Array) of the foreign currency account</param>
        /// <param name="ANewExchangeRate">Array of the exchange rates</param>
        /// <param name="AVerificationResult">A TVerificationResultCollection for possibly error messages</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool Revaluate(
            int ALedgerNum,
            int AAccoutingPeriod,
            string ARevaluationCostCenter,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate,
            out TVerificationResultCollection AVerificationResult)
        {
            CLSRevaluation revaluation = new CLSRevaluation(ALedgerNum, AAccoutingPeriod,
                ARevaluationCostCenter,
                AForeignCurrency, ANewExchangeRate);

            bool blnReturn = revaluation.RunRevaluation();

            AVerificationResult = revaluation.GetVerificationResultCollection;
            return blnReturn;
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Main Revaluation Class. The routine is to complex for a linear program.
    /// </summary>
    public class CLSRevaluation
    {
        private int intLedgerNum;
        private int intAccountingPeriod;
        private string strRevaluationCostCenter;
        private string[] strArrForeignCurrencyType;
        private decimal[] decArrExchangeRate;

        private string strBaseCurrencyType;
        private string strRevaluationAccount;

        private int intPtrToForeignData;


        decimal decAccActForeign;
        decimal decAccActBase;

        decimal decAccActBaseRequired;

        decimal decDelta;

        private GLBatchTDS GLDataset = null;
        private ABatchRow batch;
        private AJournalRow journal;


        string strStatusContent = Catalog.GetString("Revaluation ...");

        TVerificationResultCollection verificationCollection = new TVerificationResultCollection();
        TResultSeverity resultSeverity = TResultSeverity.Resv_Noncritical;
        private bool blnVerificationCollectionContainsData = false;


        /// <summary>
        /// Constructor to initialize a variable set.
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AAccoutingPeriod"></param>
        /// <param name="ARevaluationCostCenter"></param>
        /// <param name="AForeignCurrency"></param>
        /// <param name="ANewExchangeRate"></param>
        public CLSRevaluation(int ALedgerNum,
            int AAccoutingPeriod,
            string ARevaluationCostCenter,
            string[] AForeignCurrency,
            decimal[] ANewExchangeRate)
        {
            intLedgerNum = ALedgerNum;
            intAccountingPeriod = AAccoutingPeriod;
            strRevaluationCostCenter = ARevaluationCostCenter;
            strArrForeignCurrencyType = AForeignCurrency;
            decArrExchangeRate = ANewExchangeRate;
            blnVerificationCollectionContainsData = false;
        }

        public TVerificationResultCollection GetVerificationResultCollection {
            get
            {
                return verificationCollection;
            }
        }

        public bool RunRevaluation()
        {
            try
            {
                GetLedgerInfo gli = new GetLedgerInfo(intLedgerNum);
                strBaseCurrencyType = gli.BaseCurrency;
                strRevaluationAccount = gli.RevaluationAccount;
                RunRevaluationIntern();
            }
            catch (InternalException ex)
            {
                AddVerificationResultMessage("LedgerInfo invalid",
                    ex.Message, "REVAL.02",
                    TResultSeverity.Resv_Critical);
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
                throw new InternalException("001", Catalog.GetString(
                        "No Entries in GeneralLedgerMasterTable"));
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
                    int intNoOfForeignDigts = new GetCurrencyInfo(strBaseCurrencyType).digits;
                    decDelta = AccountDelta(generalLedgerMasterRow.YtdActualBase,
                        generalLedgerMasterRow.YtdActualForeign,
                        decArrExchangeRate[intPtrToForeignData],
                        intNoOfForeignDigts);

                    if (decDelta != 0)
                    {
                        // Now we have the relevant Cost Center ...
                        RevaluateCostCenter(ARelevantAccount, generalLedgerMasterRow.CostCentreCode);
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
                catch (InternalException internalException)
                {
                    string strMessage = "{0}:[{1}:{2}] {3}";
                    strMessage = String.Format(strMessage, internalException.ErrorCode,
                        ARelevantAccount,
                        generalLedgerMasterRow.CostCentreCode,
                        internalException.Message);
                    verificationCollection.Add(new TVerificationResult(
                            strStatusContent, strMessage, TResultSeverity.Resv_Noncritical));
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

        private void RevaluateCostCenter(string ARelevantAccount, string ACostCenter)
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

            strMessage = String.Format(strMessage, ARelevantAccount, ACostCenter);

            CreateTransaction(strMessage, strRevaluationAccount, !blnDebitFlag, ACostCenter);
            CreateTransaction(strMessage, ARelevantAccount, blnDebitFlag, ACostCenter);
            journal.JournalDebitTotal = journal.JournalDebitTotal + decDelta;
            journal.JournalCreditTotal = journal.JournalCreditTotal + decDelta;
        }

        private void InitBatchAndJournal()
        {
            GLDataset = TTransactionWebConnector.CreateABatch(intLedgerNum);
            batch = GLDataset.ABatch[0];
            batch.BatchDescription = Catalog.GetString("Period end revaluations");
            batch.DateEffective = new
                                  GetAccountingPeriodInfo(intLedgerNum).GetDatePeriodEnd(intAccountingPeriod);
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            journal = GLDataset.AJournal.NewRowTyped();
            journal.LedgerNumber = batch.LedgerNumber;
            journal.BatchNumber = batch.BatchNumber;
            journal.JournalNumber = 1;
            journal.DateEffective = batch.DateEffective;
            journal.JournalPeriod = intAccountingPeriod;
            journal.TransactionCurrency = strBaseCurrencyType;
            journal.JournalDescription = batch.BatchDescription;
            journal.TransactionTypeCode = MFinanceConstants.TRANSACTION_REVAL;
            journal.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;
            journal.ExchangeRateToBase = 1.0M;
            GLDataset.AJournal.Rows.Add(journal);
        }

        private void CreateTransaction(string AMessage, string AAccount,
            bool ADebitFlag, string ACostCenter)
        {
            batch.BatchStatus = MFinanceConstants.BATCH_HAS_TRANSACTIONS;

            ATransactionRow transaction = null;

            transaction = GLDataset.ATransaction.NewRowTyped();
            transaction.LedgerNumber = journal.LedgerNumber;
            transaction.BatchNumber = journal.BatchNumber;
            transaction.JournalNumber = journal.JournalNumber;
            transaction.TransactionNumber = ++journal.LastTransactionNumber;
            transaction.AccountCode = AAccount;
            transaction.CostCentreCode = ACostCenter;
            transaction.Narrative = AMessage;
            transaction.Reference = MFinanceConstants.TRANSACTION_FX_REVAL;
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
                bool blnReturnValue = (TTransactionWebConnector.SaveGLBatchTDS(
                                           ref GLDataset, out AVerifications) == TSubmitChangesResult.scrOK);

                if (blnReturnValue)
                {
                    blnVerificationCollectionContainsData = true;
                }

                ;
                blnReturnValue = (GL.WebConnectors.TTransactionWebConnector.PostGLBatch(
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

    /// <summary>
    /// Gets the specific date informations of an accounting intervall.
    /// </summary>
    public class GetAccountingPeriodInfo
    {
        private AAccountingPeriodTable periodTable = null;

        /// <summary>
        /// Constructor needs a valid ledger number.
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        public GetAccountingPeriodInfo(int ALedgerNumber)
        {
            periodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, null);
        }

        /// <summary>
        /// Selects to correct AAccountingPeriodRow or - in case of an error -
        /// it sets to null
        /// </summary>
        /// <param name="APeriodNum">Number of the requested period</param>
        /// <returns></returns>
        private AAccountingPeriodRow GetRowOfPeriod(int APeriodNum)
        {
            if (periodTable != null)
            {
                if (periodTable.Rows.Count != 0)
                {
                    for (int i = 0; i < periodTable.Rows.Count; ++i)
                    {
                        AAccountingPeriodRow periodRow =
                            (AAccountingPeriodRow)periodTable[i];

                        if (periodRow.AccountingPeriodNumber == APeriodNum)
                        {
                            return periodRow;
                        }
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the effective date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetEffectiveDateOfPeriod(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.EffectiveDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Reads the end date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetDatePeriodEnd(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.PeriodEndDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Reads the start date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetDatePeriodStart(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.PeriodStartDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }

    /// <summary>
    /// This exception shall handle the internal errors of type critcal.
    /// </summary>
    public class InternalException : SystemException
    {
        string strErrorCode;
        public InternalException(string errorCode, string message)
            : base(message)
        {
            strErrorCode = errorCode;
        }

        public string ErrorCode
        {
            get
            {
                return strErrorCode;
            }
        }
    }

    /// <summary>
    /// Get currency info is intended to be used to get some some specific infos
    /// using the old petra data base entries. GetCurrencyInfon is designed to get
    /// a rough set of information which shall be used for foreign currency
    /// calculations and presentations. In normal cases open petra uses the
    /// user defined localisation ie the presentation and rounding rules.
    /// But if you have to work with JPY (Japanese Yen) you have to know that
    /// you have to round to 0 digits even if your user settings have selected the
    /// USD and two digits rounding.
    ///
    /// The routine works error regressive that means that invalid data (currency codes)
    /// and damaged format strings will result in 2 digit rounding as a default.
    /// </summary>
    public class GetCurrencyInfo
    {
        private ACurrencyTable currencyTable = null;
        private ACurrencyRow currencyRow = null;

        /// <summary>
        /// Constructor which automatically loads one CurrencyTable Entry defined
        /// by the parameter.
        /// </summary>
        /// <param name="ACurrencyCode">Three digit description to define the
        /// currency.</param>
        public GetCurrencyInfo(string ACurrencyCode)
        {
            currencyTable = ACurrencyAccess.LoadByPrimaryKey(ACurrencyCode, null);

            if (currencyTable.Rows.Count == 1)
            {
                currencyRow = (ACurrencyRow)currencyTable[0];
            }
            else
            {
                throw new InternalException("GetCurrencyInfo.01",
                    Catalog.GetString(String.Format(
                            "There exists no account for the Currency code {0}",
                            ACurrencyCode)));
            }
        }

        /// <summary>
        /// Calculates the number of digits by reading the row.DisplayFormat
        /// Entry of the currency table and convert the old petra string to an
        /// integer response.
        /// </summary>
        public int digits
        {
            get
            {
                return new FormatConverter(currencyRow.DisplayFormat).digits;
            }
        }
    }

    /// <summary>
    /// This class is a local Format converter <br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.99").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.9").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9").digits.ToString());<br />
    /// The result is 2,1 and 0 digits ..
    /// </summary>
    public class FormatConverter
    {
        string sRegex;
        Regex reg;
        MatchCollection matchCollection;
        int intDigits;
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                throw new InternalException("GetCurrencyInfo.02",
                    String.Format("The regular expression {0} does not fit for a match in {1}",
                        sRegex, strFormat));
            }

            intDigits = (matchCollection[0].Value).Length - 3;

            if (intDigits == -1)
            {
                intDigits = 0;
            }

            if (intDigits < -1)
            {
                intDigits = 2;
            }
        }

        /// <summary>
        /// Property to report the number of digits
        /// </summary>
        public int digits
        {
            get
            {
                return intDigits;
            }
        }
    }
}