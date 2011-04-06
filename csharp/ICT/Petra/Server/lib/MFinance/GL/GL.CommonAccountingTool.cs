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
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Some E-Nums for the CommonAccountingTool i.E. for the transaction property
    /// Sub-System.
    /// (The enum.toString() is used for the database entry so you must not change the
    /// values if you do not want to change the entries.)
    /// </summary>
    public enum CommonAccountingSubSystemsEnum
    {
        /// <summary>
        /// Default - resp Standard value
        /// </summary>
        GL,
        AP,
        GR
    }

    /// <summary>
    /// Some E-Nums for the CommonAccountingTool i.E. for the transaction property
    /// Transaction Type.
    /// (The enum.toString() is used for the database entry so you must not change the
    /// values if you do not want to change the entries.)
    /// </summary>
    public enum CommonAccountingTransactionTypesEnum
    {
        /// <summary>
        /// Default - resp Standard value
        /// </summary>
        STD,
            ALLOC,
            GR,
            INV,
            REALLOC,

        /// <summary>
        /// Used in a revaluation only ...
        /// </summary>
            REVAL
    }

    /// <summary>
    /// Some constants for the journal values to rember that IS_Debit ist true.
    /// </summary>
    public partial class CommonAccountingConstants
    {
        /// <summary>
        /// Sets the transaction to a debit transaction
        /// </summary>
        public const bool IS_DEBIT = true;
        /// <summary>
        /// Sets the transaction to a credit transaction
        /// </summary>
        public const bool IS_CREDIT = false;
    }

    /// <summary>
    /// This Tool creates a batch enables to add a journal and to add transactions to a yournal
    /// All internal "pointers" and control data are set internal and the structure is "read to post".
    /// </summary>
    public partial class CommonAccountingTool
    {
        private GLBatchTDS aBatchTable = null;
        private ABatchRow aBatchRow;
        private AJournalRow journal;

        private GetLedgerInfo getLedgerInfo;
        private GetCurrencyInfo getBaseCurrencyInfo;
        private GetCurrencyInfo getForeignCurrencyInfo = null;
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
        public CommonAccountingTool(int ALedgerNumber,
            string ABatchDescription)
        {
            getLedgerInfo = new GetLedgerInfo(ALedgerNumber);
            CommonAccountingTool_(ABatchDescription);
        }

        /// <summary>
        /// Internaly a GetLedgerInfo-Oject is used. If you have one, reduce the number of not neccessary
        /// database requests and use this constructor ...
        /// </summary>
        /// <param name="ALedgerInfo">The ledger-info object</param>
        /// <param name="ABatchDescription">the description text ...</param>
        public CommonAccountingTool(GetLedgerInfo ALedgerInfo, string ABatchDescription)
        {
            getLedgerInfo = ALedgerInfo;
            CommonAccountingTool_(ABatchDescription);
        }

        private void CommonAccountingTool_(string ABatchDescription)
        {
            aBatchTable = TTransactionWebConnector.CreateABatch(getLedgerInfo.LedgerNumber);
            getBaseCurrencyInfo = new GetCurrencyInfo(getLedgerInfo.BaseCurrency);
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
                    throw new InternalException(
                        "GL.CAT.01",
                        Catalog.GetString("You cannot change the Date after you have created a journal!"));
                }

                blnInitBatchDate = false;
                aBatchRow.DateEffective = value;
            }
        }

        public void AddForeignCurrencyJournal(GetCurrencyInfo AGetCurrencyInfo, decimal AExchangeRateToBase)
        {
            blnJournalIsInForeign = true;
            getForeignCurrencyInfo = AGetCurrencyInfo;
            AddAJournal(AExchangeRateToBase);
        }

        public void AddForeignCurrencyJournal(string ACurrencyCode, decimal AExchangeRateToBase)
        {
            blnJournalIsInForeign = true;
            getForeignCurrencyInfo = new GetCurrencyInfo(ACurrencyCode);
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
                    // This is a hint for the developer only ... !
                    throw new InternalException(
                        "GL.CAT.02",
                        Catalog.GetString("You have to add a journal before you can change the JournalDescription!"));
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
                    // This is a hint for the developer only ... !
                    throw new InternalException(
                        "GL.CAT.03",
                        Catalog.GetString("You have to add a journal before you can change the TransactionTypeCode!"));
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
                    // This is a hint for the developer only ... !
                    throw new InternalException(
                        "GL.CAT.04", "You have to add a journal before you can change the SubSystemCode!");
                }

                journal.SubSystemCode = value.ToString();
            }
        }

        private void AddAJournal(decimal AExchangeRateToBase)
        {
            if (blnInitBatchDate)
            {
                GetAccountingPeriodInfo getAccountingPeriodInfo =
                    new GetAccountingPeriodInfo(getLedgerInfo.LedgerNumber, getLedgerInfo.CurrentPeriod);
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
            journal.JournalPeriod = getLedgerInfo.CurrentPeriod;

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
                // This is a hint for the developer only ... !
                throw new InternalException(
                    "GL.CAT.05", Catalog.GetString("You cannot account foreign currencies in a base journal!"));
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
                // This is a hint for the developer only ... !
                throw new InternalException(
                    "GL.CAT.06", Catalog.GetString("You have to add a journal before you can add a transaction!"));
            }

            if (blnJournalIsInForeign)
            {
                if (ATransActionIsInForeign)
                {
                    GetAccountInfo accountCheck =
                        new GetAccountInfo(null, getLedgerInfo, AAccount);

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
                                    getLedgerInfo.BaseCurrency,
                                    AAccount,
                                    accountCheck.ForeignCurrencyCode,
                                    getForeignCurrencyInfo.CurrencyCode);
                                throw new InternalException("GL.CAT.07", strMessage);
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

            bool blnReturnValue =
                (TTransactionWebConnector.SaveGLBatchTDS(
                     ref aBatchTable, out AVerifications) == TSubmitChangesResult.scrOK);
            blnReturnValue = (GL.WebConnectors.TTransactionWebConnector.PostGLBatch(
                                  aBatchRow.LedgerNumber, aBatchRow.BatchNumber, out AVerifications));
            int returnValue = aBatchRow.BatchNumber;
            // Make shure that this object cannot be used for another posting ...
            aBatchTable = null;
            aBatchRow = null;
            journal = null;
            return returnValue;
        }
    }
}