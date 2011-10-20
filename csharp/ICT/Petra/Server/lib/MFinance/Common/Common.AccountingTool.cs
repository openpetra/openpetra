//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// This Tool creates a batch enables to add a journal and to add transactions to a yournal
    /// All internal "pointers" and control data are set internal and the structure is "read to post".
    /// </summary>
    public class TCommonAccountingTool
    {
        private GLBatchTDS aBatchTable = null;
        private ABatchRow aBatchRow;
        private AJournalRow journal;

        private TLedgerInfo TLedgerInfo;
        private TCurrencyInfo getBaseCurrencyInfo;
        private TCurrencyInfo getForeignCurrencyInfo = null;
        bool blnJournalIsInForeign;

        private int intJournalCount;

        private bool blnReadyForTransaction;

        // The use of the default value requires an additional database request. So this is done in the
        // "last moment" and only if no other date value is used
        private bool blnInitBatchDate;


        /// <summary>
        /// The constructor creates a base batch and defines the batch parameters. There is only
        /// one batch to account. Use a new object to post another batch.
        /// </summary>
        /// <param name="ALedgerNumber">the ledger number</param>
        /// <param name="ABatchDescription">a batch description text</param>
        public TCommonAccountingTool(int ALedgerNumber,
            string ABatchDescription)
        {
            TLedgerInfo = new TLedgerInfo(ALedgerNumber);
            TCommonAccountingTool_(ABatchDescription);
        }

        /// <summary>
        /// Internaly a TLedgerInfo-Oject is used. If you have one, reduce the number of not neccessary
        /// database requests and use this constructor ...
        /// </summary>
        /// <param name="ALedgerInfo">The ledger-info object</param>
        /// <param name="ABatchDescription">the description text ...</param>
        public TCommonAccountingTool(TLedgerInfo ALedgerInfo, string ABatchDescription)
        {
            TLedgerInfo = ALedgerInfo;
            TCommonAccountingTool_(ABatchDescription);
        }

        private void TCommonAccountingTool_(string ABatchDescription)
        {
            aBatchTable = TGLPosting.CreateABatch(TLedgerInfo.LedgerNumber);
            getBaseCurrencyInfo = new TCurrencyInfo(TLedgerInfo.BaseCurrency);
            aBatchRow = aBatchTable.ABatch[0];
            aBatchRow.BatchDescription = ABatchDescription;
            aBatchRow.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;
            intJournalCount = 0;
            blnReadyForTransaction = false;
            blnInitBatchDate = true;
        }

        /// <summary>
        /// The default parameter for the date is the "effective date" of the accounting interval and the
        /// value is set in the constructor. Here you can change the value, if you need an other day ...
        /// </summary>
        public DateTime DateEffective
        {
            set
            {
                if (blnReadyForTransaction)
                {
                    TVerificationException terminate = new TVerificationException(
                        "You cannot change the Date after you have created a journal!");
                    terminate.Context = "Common Accountig";
                    terminate.ErrorCode = "GL.CAT.01";
                    throw terminate;
                }

                blnInitBatchDate = false;
                aBatchRow.DateEffective = value;
            }
        }

        /// <summary>
        /// Here you can add a Foreign currency Journal
        /// </summary>
        /// <param name="ATCurrencyInfo">A TCurrencyInfo is created extern and
        /// can be included as a parameter</param>
        /// <param name="AExchangeRateToBase"></param>
        public void AddForeignCurrencyJournal(TCurrencyInfo ATCurrencyInfo, decimal AExchangeRateToBase)
        {
            blnJournalIsInForeign = true;
            getForeignCurrencyInfo = ATCurrencyInfo;
            AddAJournal(AExchangeRateToBase);
        }

        /// <summary>
        /// Here you can add a Foreign currency Journal
        /// </summary>
        /// <param name="ACurrencyCode">The currency code is defined by a string and the
        /// TCurrencyInfo object is created internally</param>
        /// <param name="AExchangeRateToBase"></param>
        public void AddForeignCurrencyJournal(string ACurrencyCode, decimal AExchangeRateToBase)
        {
            blnJournalIsInForeign = true;

            if (getForeignCurrencyInfo == null)
            {
                getForeignCurrencyInfo = new TCurrencyInfo(ACurrencyCode);
            }

            AddAJournal(AExchangeRateToBase);
        }

        /// <summary>
        /// A standard-base currency journal does not need any more information
        /// </summary>
        public void AddBaseCurrencyJournal()
        {
            blnJournalIsInForeign = false;
            AddAJournal(1.0m);
        }

        /// <summary>
        /// The journal description text is copied form the batch description text. Here you can change it on the
        /// last added journal.
        /// </summary>
        public string JournalDescription
        {
            set
            {
                if (!blnReadyForTransaction)
                {
                    TVerificationException terminate = new TVerificationException(
                        "You have to add a journal before you can change the JournalDescription!");
                    terminate.Context = "Common Accountig";
                    terminate.ErrorCode = "GL.CAT.02";
                    throw terminate;
                }

                journal.JournalDescription = value;
            }
        }

        /// <summary>
        /// Change the TransactionTypeCode from it's default value ...
        /// </summary>
        public CommonAccountingTransactionTypesEnum TransactionTypeCode
        {
            set
            {
                if (!blnReadyForTransaction)
                {
                    TVerificationException terminate = new TVerificationException(
                        "You have to add a journal before you can change the TransactionTypeCode!");
                    terminate.Context = "Common Accountig";
                    terminate.ErrorCode = "GL.CAT.03";
                    throw terminate;
                }

                journal.TransactionTypeCode = value.ToString();
            }
        }

        /// <summary>
        /// Change the SubSystemCode from it's default value ...
        /// </summary>
        public CommonAccountingSubSystemsEnum SubSystemCode
        {
            set
            {
                if (!blnReadyForTransaction)
                {
                    TVerificationException terminate = new TVerificationException(
                        "You have to add a journal before you can change the SubSystemCode!");
                    terminate.Context = "Common Accountig";
                    terminate.ErrorCode = "GL.CAT.04";
                    throw terminate;
                }

                journal.SubSystemCode = value.ToString();
            }
        }

        private void AddAJournal(decimal AExchangeRateToBase)
        {
            if (blnInitBatchDate)
            {
                TAccountPeriodInfo getAccountingPeriodInfo =
                    new TAccountPeriodInfo(TLedgerInfo.LedgerNumber, TLedgerInfo.CurrentPeriod);
                aBatchRow.DateEffective = getAccountingPeriodInfo.PeriodEndDate;
                blnInitBatchDate = false;
            }

            if (intJournalCount != 0)
            {
                // The checksum of the "last journal" is used to update the checksum of the batch.
                aBatchRow.BatchControlTotal += journal.JournalDebitTotal - journal.JournalCreditTotal;
            }

            ++intJournalCount;
            journal = aBatchTable.AJournal.NewRowTyped();
            journal.LedgerNumber = aBatchRow.LedgerNumber;
            journal.BatchNumber = aBatchRow.BatchNumber;
            journal.JournalNumber = intJournalCount;
            journal.DateEffective = aBatchRow.DateEffective;
            journal.JournalPeriod = TLedgerInfo.CurrentPeriod;

            if (blnJournalIsInForeign)
            {
                journal.TransactionCurrency = getForeignCurrencyInfo.CurrencyCode;
            }
            else
            {
                journal.TransactionCurrency = getBaseCurrencyInfo.CurrencyCode;
            }

            journal.JournalDescription = aBatchRow.BatchDescription;
            journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
            journal.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;
            journal.ExchangeRateToBase = AExchangeRateToBase;
            journal.JournalCreditTotal = 0;
            journal.JournalDebitTotal = 0;
            aBatchTable.AJournal.Rows.Add(journal);
            blnReadyForTransaction = true;
        }

        /// <summary>
        /// A Transaction is added only to the last added batch. This routine can be called
        /// multiple times too even inside of one journal.
        /// </summary>
        /// <param name="AAccount"></param>
        /// <param name="ACostCenter"></param>
        /// <param name="ANarrativeMessage"></param>
        /// <param name="AReferenceMessage"></param>
        /// <param name="AIsDebit"></param>
        /// <param name="AAmountBaseCurrency"></param>
        public void AddBaseCurrencyTransaction(string AAccount,
            string ACostCenter,
            string ANarrativeMessage,
            string AReferenceMessage,
            bool AIsDebit,
            decimal AAmountBaseCurrency)
        {
            AddATransaction(AAccount, ACostCenter, ANarrativeMessage,
                AReferenceMessage, AIsDebit, AAmountBaseCurrency, 0, false);
        }

        /// <summary>
        /// Add a foreign currency transaction ...
        /// </summary>
        /// <param name="AAccount"></param>
        /// <param name="ACostCenter"></param>
        /// <param name="ANarrativeMessage"></param>
        /// <param name="AReferenceMessage"></param>
        /// <param name="AIsDebit"></param>
        /// <param name="AAmountBaseCurrency"></param>
        /// <param name="AAmountForeignCurrency"></param>
        public void AddForeignCurrencyTransaction(string AAccount,
            string ACostCenter,
            string ANarrativeMessage,
            string AReferenceMessage,
            bool AIsDebit,
            decimal AAmountBaseCurrency,
            decimal AAmountForeignCurrency)
        {
            if (!blnJournalIsInForeign)
            {
                TVerificationException terminate = new TVerificationException(
                    Catalog.GetString("You cannot account foreign currencies in a base journal!"));
                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "GL.CAT.05";
                throw terminate;
            }

            AddATransaction(AAccount, ACostCenter, ANarrativeMessage,
                AReferenceMessage, AIsDebit, AAmountBaseCurrency, AAmountForeignCurrency, true);
        }

        private void AddATransaction(string AAccount,
            string ACostCenter,
            string ANarrativeMessage,
            string AReferenceMessage,
            bool AIsDebit,
            decimal AAmountBaseCurrency,
            decimal AAmountForeignCurrency,
            bool ATransActionIsInForeign)
        {
            if (!blnReadyForTransaction)
            {
                TVerificationException terminate = new TVerificationException(
                    Catalog.GetString("You have to add a journal before you can add a transaction!"));
                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "GL.CAT.06";
                throw terminate;
            }

            if (blnJournalIsInForeign)
            {
                if (ATransActionIsInForeign)
                {
                    TAccountInfo accountCheck =
                        new TAccountInfo(TLedgerInfo, AAccount);

                    if (accountCheck.IsValid)
                    {
                        if (accountCheck.ForeignCurrencyFlag)
                        {
                            if (!accountCheck.ForeignCurrencyCode.Equals(this.getForeignCurrencyInfo.CurrencyCode))
                            {
                                // This is a difficult error situation. Someone wants to account
                                // JYP-Currencies on a GBP-account in an EUR ledger.
                                string strMessage = Catalog.GetString("The ledger is defined in {0}, the account {1} is defined in " +
                                    "{2} and you want to account something in {3}?");
                                strMessage = String.Format(strMessage,
                                    TLedgerInfo.BaseCurrency,
                                    AAccount,
                                    accountCheck.ForeignCurrencyCode,
                                    getForeignCurrencyInfo.CurrencyCode);
                                TVerificationException terminate = new TVerificationException(strMessage);
                                terminate.Context = "Common Accountig";
                                terminate.ErrorCode = "GL.CAT.07";
                                throw terminate;
                            }
                        }
                    }
                }
            }

            ATransactionRow transaction = null;

            transaction = aBatchTable.ATransaction.NewRowTyped();
            transaction.LedgerNumber = journal.LedgerNumber;
            transaction.BatchNumber = journal.BatchNumber;
            transaction.JournalNumber = journal.JournalNumber;
            transaction.TransactionNumber = ++journal.LastTransactionNumber;
            transaction.AccountCode = AAccount;
            transaction.CostCentreCode = ACostCenter;
            transaction.Narrative = ANarrativeMessage;
            transaction.Reference = AReferenceMessage;
            transaction.DebitCreditIndicator = AIsDebit;

            if (ATransActionIsInForeign)
            {
                transaction.TransactionAmount = AAmountForeignCurrency;
                transaction.AmountInBaseCurrency = AAmountBaseCurrency;
            }
            else
            {
                transaction.TransactionAmount = AAmountBaseCurrency;
                transaction.AmountInBaseCurrency = AAmountBaseCurrency;
            }

            transaction.TransactionDate = aBatchRow.DateEffective;
            aBatchTable.ATransaction.Rows.Add(transaction);

            if (AIsDebit)
            {
                journal.JournalDebitTotal += AAmountBaseCurrency;
            }
            else
            {
                journal.JournalCreditTotal += AAmountBaseCurrency;
            }
        }

        /// <summary>
        /// Here you can close save and post the batch, the included journal(s) and the
        /// transaction(s).
        /// </summary>
        /// <param name="AVerifications">A TVerificationResultCollection can defined to
        /// accept the error messages and warnings - if necessary.</param>
        /// <returns>The routine writes back the batch number and so you can access to the
        /// batch directly (if necessary)</returns>
        public int CloseSaveAndPost(TVerificationResultCollection AVerifications)
        {
            return CloseSaveAndPost_(AVerifications);
        }

        /// <summary>
        /// The net-syntax checker reqires a clause "using Ict.Common.Verification;" in the routine which
        /// calls CloseSaveAndPost(null). The only way to avoid this is the use of CloseSaveAndPost().
        /// </summary>
        /// <returns></returns>
        public int CloseSaveAndPost()
        {
            return CloseSaveAndPost_(null);
        }

        private int CloseSaveAndPost_(TVerificationResultCollection AVerifications)
        {
            if (intJournalCount != 0)
            {
                // The checksum of the "last journal" is used to update the checksum of the batch.
                aBatchRow.BatchControlTotal += journal.JournalDebitTotal - journal.JournalCreditTotal;
            }

            TSubmitChangesResult submissionResult = GLBatchTDSAccess.SubmitChanges(
                aBatchTable, out AVerifications);

            if (submissionResult != TSubmitChangesResult.scrOK)
            {
                throw new ApplicationException("Batch could not be saved!");
            }

            TGLPosting.PostGLBatch(
                aBatchRow.LedgerNumber, aBatchRow.BatchNumber, out AVerifications);

            int returnValue = aBatchRow.BatchNumber;
            // Make shure that this object cannot be used for another posting ...
            aBatchTable = null;
            aBatchRow = null;
            journal = null;
            return returnValue;
        }
    }
}