//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, Tim Ingham, ChristianK (refactoring, '3-As')
//
// Copyright 2004-2019 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.AP.WebConnectors;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;

namespace Ict.Testing.Petra.Server.MFinance.AP
{
    /// <summary>
    /// a couple of tests for AP
    /// </summary>
    [TestFixture]
    public class TestAP
    {
        private const int FLedgerNumber = 43;

        /// <summary>
        /// Holds various bits of information about AP.
        /// </summary>
        private struct AAPInfos
        {
            public DateTime PeriodStartDate;
            public DateTime PeriodEndDate;
            public string ForexGainsLossesAccount;
            public string BankAccount;
            public string CurrencyCode;
            public string ApAccountCode;
            public string CostCentreCode;
            public AccountsPayableTDS ApDS;
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [OneTimeTearDown]
        public void TearDownTest()
        {
            TPetraServerConnector.Disconnect();
        }

        // using the sample supplier from the demo database
        const long SUPPLIER_PARTNER_KEY = 43005002;
        const long SUPPLIER_FOREIGN_PARTNER_KEY = 43005003;

        /// <summary>
        /// Posts a document and pays it. (The supplier DOESN'T work with foreign currency.)
        /// </summary>
        [Test]
        public void SimpleDocument_ExpectPostingAndPayingWorking()
        {
            //
            // Arrange
            //
            decimal Amount = 399.0m;
            decimal APAccountBalanceBefore;
            decimal ABankAccountBefore;
            decimal AExpAccountBefore;
            TVerificationResultCollection VerificationResult;

            List <int>DocumentIDs;
            int PaymentNumber;
            AAPInfos APInfos;

            CommonNUnitFunctions.ResetDatabase();
            TPetraServerConnector.Connect();

            TDataBase db = DBAccess.Connect("SimpleDocument_ExpectPostingAndPayingWorking");
            TDBTransaction transaction = db.BeginTransaction(IsolationLevel.Serializable);

            APInfos = PostSimpleAPDocument(Amount, "Test", "Detail Item", out APAccountBalanceBefore, out ABankAccountBefore,
                out AExpAccountBefore, out DocumentIDs, db);

            transaction.Commit();
            transaction = db.BeginTransaction(IsolationLevel.Serializable);

            //
            // Act: Pay the AP document
            //
            VerificationResult = PayAPDocument(APInfos.ApDS.AApDocument[0].ApDocumentId, Amount,
                APInfos.BankAccount, APInfos.CurrencyCode, APInfos.PeriodEndDate, out PaymentNumber, null, db);
            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            transaction.Commit();

            // Save the current amount on the AP account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode, db).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode, db).YtdActual;

            //
            // Primary Assert: Paying OK?
            //
            // Check the amount on the AP account
            Assert.AreEqual(0.0m, APAccountBalanceAfter - APAccountBalanceBefore, "after paying the invoice, the AP account should be cleared");
            Assert.AreEqual((-1.0m) * Amount, BankAccountAfter - ABankAccountBefore, "after paying the invoice, the bank account should be credited");
        }

        /// <summary>
        /// Posts a document and pays it. The supplier works with foreign currency.
        /// </summary>
        [Test]
        public void ForeignCurrencySupplier_ExpectDocumentPostingAndPayingWorking()
        {
            //
            // Arrange
            //
            int PaymentNumber;

            List <int>DocumentIDs;
            decimal APAccountBalanceBefore;
            decimal BankAccountBefore;
            decimal RevalAccountBefore;

            CommonNUnitFunctions.ResetDatabase();
            TPetraServerConnector.Connect();

            TDataBase db = DBAccess.Connect("ForeignCurrencySupplier_ExpectDocumentPostingPayingAndReversingWorking");
            TDBTransaction transaction = db.BeginTransaction(IsolationLevel.Serializable);

            // Act and Assert: both encapsulated in the method that gets called!
            PostAndPayForeignSupplierAPDocument("Test", out PaymentNumber, out DocumentIDs,
                out APAccountBalanceBefore, out BankAccountBefore, out RevalAccountBefore, db);

            transaction.Commit();
        }

        /// <summary>
        /// Posts a document and pays it, then reverses it. The supplier works with foreign currency.
        /// </summary>
        [Test]
        public void ForeignCurrencySupplier_ExpectDocumentPostingPayingAndReversingWorking()
        {
            //
            // Arrange
            //
            decimal APAccountBalanceBefore;
            decimal BankAccountBefore;
            decimal RevalAccountBefore;
            TVerificationResultCollection VerificationResult;
            int PaymentNumber;

            List <int>DocumentIDs;
            AAPInfos APInfos;

            CommonNUnitFunctions.ResetDatabase();
            TPetraServerConnector.Connect();

            TDataBase db = DBAccess.Connect("ForeignCurrencySupplier_ExpectDocumentPostingPayingAndReversingWorking");
            TDBTransaction transaction = db.BeginTransaction(IsolationLevel.Serializable);

            // Post and pay a document with a foreign currency supplier
            APInfos = PostAndPayForeignSupplierAPDocument("Test Reverse", out PaymentNumber, out DocumentIDs,
                out APAccountBalanceBefore, out BankAccountBefore, out RevalAccountBefore, db);

            transaction.Commit();
            transaction = db.BeginTransaction(IsolationLevel.Serializable);

            //
            // Act: Immediately "un-pay" and "un-post" this invoice!
            //
            VerificationResult = ReversePayment(PaymentNumber, APInfos.PeriodEndDate, DocumentIDs, APInfos.ApDS, db);
            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            transaction.Commit();
            transaction = db.BeginTransaction(IsolationLevel.Serializable);

            // Save the current amount on the AP account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode, db).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode, db).YtdForeign;
            decimal RevalAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount, APInfos.CostCentreCode, db).YtdActual;

            //
            // Primary Assert: Reversal OK?
            //
            // Now I can see whether anything is left over by all these
            // various transactions, that should have added up to 0.
            // Check the amount on the AP account
            Assert.AreEqual(APAccountBalanceBefore, APAccountBalanceAfter, "After paying then reversing, the AP account should be as before.");
            Assert.AreEqual(BankAccountBefore, BankAccountAfter, "After paying then reversing, the Bank account should be as before.");
            Assert.AreEqual(
                Math.Round(RevalAccountAfter, 2),
                Math.Round(RevalAccountBefore, 2),
                "After paying then reversing, the Forex Gains/Losses Account account should be as before.");

            transaction.Commit();
        }

        #region Helper Methods

        private AAPInfos APTestsArrangement(Int64 APartnerKey, decimal AAmount, decimal? AExchangeRatePosting,
            string ADocumentCode, string ANarrative, TDataBase ADataBase)
        {
            AAPInfos APInfos = new AAPInfos();

            TVerificationResultCollection VerificationResult = CreateAPDocument(APartnerKey,
                AAmount,
                AExchangeRatePosting,
                ADocumentCode,
                ANarrative,
                out APInfos.ApDS, ADataBase);

            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            GetLedgerInfo(out APInfos.PeriodStartDate, out APInfos.PeriodEndDate, out APInfos.ForexGainsLossesAccount, ADataBase);

            SetupSupplierAndDocumentInfo(APInfos.ApDS,
                out APInfos.BankAccount,
                out APInfos.CurrencyCode,
                out APInfos.ApAccountCode,
                out APInfos.CostCentreCode);

            return APInfos;
        }

        private void GetLedgerInfo(out DateTime APeriodStartDate, out DateTime APeriodEndDate,
            out string AForexGainsLossesAccount, TDataBase ADataBase)
        {
            ALedgerTable LedgerTable = null;

            TDataBase db = DBAccess.Connect("GetLedgerInfo", ADataBase);

            TDBTransaction Transaction = new TDBTransaction();

            DateTime PeriodStartDate = DateTime.Today;
            DateTime PeriodEndDate = DateTime.Today;
            string ForexGainsLossesAccount = String.Empty;
            
            db.ReadTransaction(ref Transaction,
                delegate
                {
                    LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, Transaction);

                    ForexGainsLossesAccount = LedgerTable[0].ForexGainsLossesAccount;

                    TFinancialYear.GetStartAndEndDateOfPeriod(FLedgerNumber, LedgerTable[0].CurrentPeriod, out PeriodStartDate, out PeriodEndDate, Transaction);
                });

            AForexGainsLossesAccount = ForexGainsLossesAccount;
            APeriodStartDate = PeriodStartDate;
            APeriodEndDate = PeriodEndDate;
        }

        private void SetupSupplierAndDocumentInfo(AccountsPayableTDS AMainDS, out string ABankAccount, out string ACurrencyCode,
            out string AApAccountCode, out string ACostCentreCode)
        {
            ABankAccount = AMainDS.AApSupplier[0].DefaultBankAccount;
            ACurrencyCode = AMainDS.AApDocument[0].CurrencyCode;
            AApAccountCode = AMainDS.AApDocument[0].ApAccount;
            ACostCentreCode = AMainDS.AApDocumentDetail[0].CostCentreCode;
        }

        /// <summary>
        /// Creates a AP document for the supplier specified with APartnerKey.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AAmount"></param>
        /// <param name="AExchangeRatePosting"></param>
        /// <param name="ADocumentCode"></param>
        /// <param name="ANarrative"></param>
        /// <param name="AMainDS"></param>
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        private TVerificationResultCollection CreateAPDocument(Int64 APartnerKey, decimal AAmount, decimal? AExchangeRatePosting,
            string ADocumentCode, string ANarrative, out AccountsPayableTDS AMainDS, TDataBase ADataBase)
        {
            string AssertFailMessage = "Problems saving AP document: ";
            TSubmitChangesResult SubmRes;
            TVerificationResultCollection VerificationResult;

            TDataBase db = DBAccess.Connect("CreateAPDocument", ADataBase);

            AMainDS = TAPTransactionWebConnector.CreateAApDocument(FLedgerNumber, APartnerKey, false, db);
            AccountsPayableTDS MainDS = AMainDS;

            TDBTransaction Transaction = new TDBTransaction();
            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    AApSupplierAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                });

            AMainDS.AApDocument[0].DocumentCode = ADocumentCode + DateTime.Now.Ticks.ToString();

            AMainDS.Merge(TAPTransactionWebConnector.CreateAApDocumentDetail(
                    FLedgerNumber,
                    AMainDS.AApDocument[0].ApDocumentId,
                    AMainDS.AApSupplier[0].DefaultExpAccount,
                    AMainDS.AApSupplier[0].DefaultCostCentre,
                    AAmount,
                    AMainDS.AApDocument[0].LastDetailNumber + 1));

            AMainDS.AApDocument[0].LastDetailNumber++;
            AMainDS.AApDocument[0].TotalAmount = AAmount;
            AMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
            AMainDS.AApDocumentDetail[0].Narrative = ANarrative;

            if (AExchangeRatePosting.HasValue)
            {
                AMainDS.AApDocument[0].ExchangeRateToBase = AExchangeRatePosting.Value;
            }

            SubmRes = TAPTransactionWebConnector.SaveAApDocument(ref AMainDS, out VerificationResult, db);

            if (SubmRes != TSubmitChangesResult.scrOK)
            {
                Assert.Fail(AssertFailMessage + String.Format(" - (SaveAApDocument return value: {0}) - ", SubmRes) +
                    VerificationResult.BuildVerificationResultString());
            }

            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult, AssertFailMessage);   // Guard Assert

            return VerificationResult;
        }

        private TVerificationResultCollection PostAPDocument(AccountsPayableTDS AMainDS, DateTime APostingDate,
            ref List <int>ADocumentIds, bool AReversal = false, TDataBase ADataBase = null)
        {
            string AssertFailMessage = AReversal ? "Failed to post AP document reversal: " : "Problems posting AP document: ";
            TVerificationResultCollection VerificationResult;

            if (!AReversal)
            {
                ADocumentIds.Add(AMainDS.AApDocument[0].ApDocumentId);
            }

            Int32 glBatchNumber;

            if (!TAPTransactionWebConnector.PostAPDocuments(FLedgerNumber,
                    ADocumentIds,
                    APostingDate,
                    AReversal,
                    out glBatchNumber,
                    out VerificationResult,
                    ADataBase))
            {
                Assert.Fail(AssertFailMessage +
                    VerificationResult.BuildVerificationResultString());
            }

            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult, AssertFailMessage);   // Guard Assert

            return VerificationResult;
        }

        private AAPInfos PostSimpleAPDocument(decimal AAmount, string ADocumentCode, string ANarrative,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal AExpAccountBefore,
            out List <int>ADocumentIds, TDataBase ADataBase)
        {
            TVerificationResultCollection VerificationResult;

            ADocumentIds = new List <int>();

            AAPInfos APInfos = APTestsArrangement(SUPPLIER_PARTNER_KEY, AAmount, null, ADocumentCode, ANarrative, ADataBase);

            // Save the current amount on the AP account
            AAPAccountBalanceBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode, ADataBase).YtdActual;
            ABankAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode, ADataBase).YtdActual;

            AExpAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.CostCentreCode, ADataBase).YtdActual;

            VerificationResult = PostAPDocument(APInfos.ApDS, APInfos.PeriodStartDate, ref ADocumentIds, false, ADataBase);
            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            //
            // Guard Assert: Posting OK?
            //
            decimal ExpAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.ApDS.AApSupplier[0].DefaultCostCentre, ADataBase).YtdActual;

            Assert.AreEqual(AAmount, ExpAccountAfter - AExpAccountBefore, "after posting the invoice, the expense account should be debited");

            return APInfos;
        }

        private AAPInfos PostForeignSupplierAPDocument(decimal AAmount, decimal AExchangeRatePosting, string ADocumentCode, string ANarrative,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal AExpAccountBefore,
            out decimal ARevalAccountBefore, out List <int>ADocumentIds, TDataBase ADataBase)
        {
            TVerificationResultCollection VerificationResult;

            ADocumentIds = new List <int>();

            AAPInfos APInfos = APTestsArrangement(SUPPLIER_FOREIGN_PARTNER_KEY, AAmount, AExchangeRatePosting, ADocumentCode, ANarrative, ADataBase);

            // Save the current amount on the AP account
            AAPAccountBalanceBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode, ADataBase).YtdActual;
            ABankAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode, ADataBase).YtdForeign;

            AExpAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.CostCentreCode, ADataBase).YtdActual;
            ARevalAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount,
                APInfos.CostCentreCode, ADataBase).YtdActual;

            VerificationResult = PostAPDocument(APInfos.ApDS, APInfos.PeriodStartDate, ref ADocumentIds, false, ADataBase);
            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            //
            // Guard Assert: Posting OK?
            //
            decimal ExpAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.ApDS.AApSupplier[0].DefaultCostCentre, ADataBase).YtdActual;

            Assert.AreEqual(Math.Round(AAmount / AExchangeRatePosting, 2), Math.Round(ExpAccountAfter - AExpAccountBefore,
                    2), "after posting the invoice, the expense account should be debited the amount in base currency (Exchange Rate is " +
                AExchangeRatePosting + ")");

            return APInfos;
        }

        private AAPInfos PostAndPayForeignSupplierAPDocument(string ADocumentCode, out int APaymentNumber, out List <int>ADocumentIDs,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal ARevalAccountBefore, TDataBase ADataBase)
        {
            decimal Amount = 100.0m;
            decimal ExchangeRatePosting = 1.2m;
            decimal ExchangeRatePayment = 1.1m;
            decimal ExpAccountBefore;
            TVerificationResultCollection VerificationResult;
            AAPInfos APInfos;

            APInfos = PostForeignSupplierAPDocument(Amount, ExchangeRatePosting, ADocumentCode, "Detail Item",
                out AAPAccountBalanceBefore, out ABankAccountBefore, out ExpAccountBefore, out ARevalAccountBefore, out ADocumentIDs, ADataBase);

            //
            // Pay the AP document
            //
            VerificationResult = PayAPDocument(APInfos.ApDS.AApDocument[0].ApDocumentId, Amount,
                APInfos.BankAccount, APInfos.CurrencyCode, APInfos.PeriodEndDate, out APaymentNumber, ExchangeRatePayment, ADataBase);
            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult);   // Guard Assert

            // Save the current amount on the AP account and Bank Account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode, ADataBase).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode, ADataBase).YtdForeign;

            //
            // Guard Assert: Paying OK?
            //
            // Check the amount on the AP account
            Assert.AreEqual(0.0m, APAccountBalanceAfter - AAPAccountBalanceBefore, "after paying the invoice, the AP account should be cleared");
            Assert.AreEqual((-1.0m) * Amount, BankAccountAfter - ABankAccountBefore, "after paying the invoice, the bank account should be credited");

            decimal RevalAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount, APInfos.CostCentreCode, ADataBase).YtdActual;

            Assert.AreEqual(
                Math.Round((Amount / ExchangeRatePayment) - (Amount / ExchangeRatePosting), 2),
                Math.Round((RevalAccountAfter - ARevalAccountBefore), 2),
                "after paying the invoice, the revaluation account should be credited with the forex gain");

            return APInfos;
        }

        private TVerificationResultCollection PayAPDocument(int AApDocumentId, decimal AAmount, string ABankAccount,
            string ACurrencyCode, DateTime APeriodEndDate, out int APaymentNumber, decimal? AExchangeRatePayment = null, TDataBase ADataBase = null)
        {
            string AssertFailMessage = "Problems paying AP document: ";
            TVerificationResultCollection VerificationResult;
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApPaymentRow Payment = MainDS.AApPayment.NewRowTyped();

            Payment.LedgerNumber = FLedgerNumber;
            Payment.PaymentNumber = -1;
            Payment.Amount = AAmount;
            Payment.BankAccount = ABankAccount;
            Payment.CurrencyCode = ACurrencyCode;

            if (AExchangeRatePayment.HasValue)
            {
                Payment.ExchangeRateToBase = AExchangeRatePayment.Value;
            }

            MainDS.AApPayment.Rows.Add(Payment);

            AApDocumentPaymentRow DocPayment = MainDS.AApDocumentPayment.NewRowTyped();
            DocPayment.LedgerNumber = FLedgerNumber;
            DocPayment.ApDocumentId = AApDocumentId;
            DocPayment.Amount = AAmount;
            DocPayment.PaymentNumber = Payment.PaymentNumber;
            MainDS.AApDocumentPayment.Rows.Add(DocPayment);
            Int32 glBatchNumber;
            AccountsPayableTDSAApPaymentTable newPayments;

            if (!TAPTransactionWebConnector.PostAPPayments(ref MainDS, APeriodEndDate,
                    out glBatchNumber,
                    out newPayments,
                    out VerificationResult,
                    ADataBase))
            {
                Assert.Fail(AssertFailMessage +
                    VerificationResult.BuildVerificationResultString());
            }

            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult, AssertFailMessage);   // Guard Assert

            APaymentNumber = DocPayment.PaymentNumber;

            return VerificationResult;
        }

        private TVerificationResultCollection ReversePayment(int APaymentNumber, DateTime APeriodEndDate,
            List <int>ADocumentIds, AccountsPayableTDS AApDS, TDataBase ADataBase)
        {
            string AssertFailMessage = "Failed to reverse AP payment: ";
            TVerificationResultCollection VerificationResult;

            List <Int32>glBatchNumbers;

            // "Un-pay" the specified invoice
            if (!TAPTransactionWebConnector.ReversePayment(FLedgerNumber,
                    APaymentNumber,
                    APeriodEndDate,
                    out glBatchNumbers,
                    out VerificationResult, ADataBase))
            {
                Assert.Fail(AssertFailMessage +
                    VerificationResult.BuildVerificationResultString());
            }

            CommonNUnitFunctions.EnsureNullOrEmptyVerificationResult(VerificationResult, AssertFailMessage);   // Guard Assert

            // "Un-post" the specified invoice - returning it to "Approved" status!
            ADocumentIds[0] += 2; // The invoice I posted was reversed, and a duplicate now exists with an Id 2 greater than the original.

            return PostAPDocument(AApDS, APeriodEndDate, ref ADocumentIds, true, ADataBase);
        }

        #endregion
    }
}
