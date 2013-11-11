//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, Tim Ingham, ChristianK (refactoring, '3-As')
//
// Copyright 2004-2013 by OM International
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
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [TestFixtureTearDown]
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

            APInfos = PostSimpleAPDocument(Amount, "Test", "Detail Item", out APAccountBalanceBefore, out ABankAccountBefore,
                out AExpAccountBefore, out DocumentIDs);

            //
            // Act: Pay the AP document
            //
            VerificationResult = PayAPDocument(APInfos.ApDS.AApDocument[0].ApDocumentId, Amount,
                APInfos.BankAccount, APInfos.CurrencyCode, APInfos.PeriodEndDate, out PaymentNumber);
            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            // Save the current amount on the AP account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode).YtdActual;

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

            // Act and Assert: both encapsulated in the method that gets called!
            PostAndPayForeignSupplierAPDocument("Test", out PaymentNumber, out DocumentIDs,
                out APAccountBalanceBefore, out BankAccountBefore, out RevalAccountBefore);
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

            // Post and pay a document with a foreign currency supplier
            APInfos = PostAndPayForeignSupplierAPDocument("Test Reverse", out PaymentNumber, out DocumentIDs,
                out APAccountBalanceBefore, out BankAccountBefore, out RevalAccountBefore);


            //
            // Act: Immediately "un-pay" and "un-post" this invoice!
            //
            VerificationResult = ReversePayment(PaymentNumber, APInfos.PeriodEndDate, DocumentIDs, APInfos.ApDS);
            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            // Save the current amount on the AP account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode).YtdForeign;
            decimal RevalAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount, APInfos.CostCentreCode).YtdActual;

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
        }

        #region Helper Methods

        private AAPInfos APTestsArrangement(Int64 APartnerKey, decimal AAmount, decimal? AExchangeRatePosting,
            string ADocumentCode, string ANarrative)
        {
            AAPInfos APInfos = new AAPInfos();

            TVerificationResultCollection VerificationResult = CreateAPDocument(APartnerKey,
                AAmount,
                AExchangeRatePosting,
                ADocumentCode,
                ANarrative,
                out APInfos.ApDS);

            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            GetLedgerInfo(out APInfos.PeriodStartDate, out APInfos.PeriodEndDate, out APInfos.ForexGainsLossesAccount);

            SetupSupplierAndDocumentInfo(APInfos.ApDS,
                out APInfos.BankAccount,
                out APInfos.CurrencyCode,
                out APInfos.ApAccountCode,
                out APInfos.CostCentreCode);

            return APInfos;
        }

        private void GetLedgerInfo(out DateTime APeriodStartDate, out DateTime APeriodEndDate,
            out string AForexGainsLossesAccount)
        {
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, null);

            AForexGainsLossesAccount = LedgerTable[0].ForexGainsLossesAccount;

            TFinancialYear.GetStartAndEndDateOfPeriod(FLedgerNumber, LedgerTable[0].CurrentPeriod, out APeriodStartDate, out APeriodEndDate, null);
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
        /// <returns></returns>
        private TVerificationResultCollection CreateAPDocument(Int64 APartnerKey, decimal AAmount, decimal? AExchangeRatePosting,
            string ADocumentCode, string ANarrative, out AccountsPayableTDS AMainDS)
        {
            TVerificationResultCollection VerificationResult;

            AMainDS = TAPTransactionWebConnector.CreateAApDocument(FLedgerNumber, APartnerKey, false);

            AApSupplierAccess.LoadByPrimaryKey(AMainDS, APartnerKey, null);

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

            if (TAPTransactionWebConnector.SaveAApDocument(ref AMainDS, out VerificationResult)
                != TSubmitChangesResult.scrOK)
            {
                Assert.Fail("Problems saving AP document: " +
                    VerificationResult.BuildVerificationResultString());
            }

            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            return VerificationResult;
        }

        private TVerificationResultCollection PostAPDocument(AccountsPayableTDS AMainDS, DateTime APostingDate,
            ref List <int>ADocumentIds, bool AReversal = false)
        {
            string AssertFailMessage = AReversal ? "Failed to post AP document reversal: " : "Problems posting AP document: ";
            TVerificationResultCollection VerificationResult;

            if (!AReversal)
            {
                ADocumentIds.Add(AMainDS.AApDocument[0].ApDocumentId);
            }

            if (!TAPTransactionWebConnector.PostAPDocuments(FLedgerNumber,
                    ADocumentIds,
                    APostingDate,
                    AReversal, out VerificationResult))
            {
                Assert.Fail(AssertFailMessage +
                    VerificationResult.BuildVerificationResultString());
            }

            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            return VerificationResult;
        }

        private AAPInfos PostSimpleAPDocument(decimal AAmount, string ADocumentCode, string ANarrative,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal AExpAccountBefore,
            out List <int>ADocumentIds)
        {
            TVerificationResultCollection VerificationResult;

            ADocumentIds = new List <int>();

            AAPInfos APInfos = APTestsArrangement(SUPPLIER_PARTNER_KEY, AAmount, null, ADocumentCode, ANarrative);

            // Save the current amount on the AP account
            AAPAccountBalanceBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode).YtdActual;
            ABankAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode).YtdActual;

            AExpAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.CostCentreCode).YtdActual;

            VerificationResult = PostAPDocument(APInfos.ApDS, APInfos.PeriodStartDate, ref ADocumentIds);
            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            //
            // Guard Assert: Posting OK?
            //
            decimal ExpAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.ApDS.AApSupplier[0].DefaultCostCentre).YtdActual;

            Assert.AreEqual(AAmount, ExpAccountAfter - AExpAccountBefore, "after posting the invoice, the expense account should be debited");

            return APInfos;
        }

        private AAPInfos PostForeignSupplierAPDocument(decimal AAmount, decimal AExchangeRatePosting, string ADocumentCode, string ANarrative,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal AExpAccountBefore,
            out decimal ARevalAccountBefore, out List <int>ADocumentIds)
        {
            TVerificationResultCollection VerificationResult;

            ADocumentIds = new List <int>();

            AAPInfos APInfos = APTestsArrangement(SUPPLIER_FOREIGN_PARTNER_KEY, AAmount, AExchangeRatePosting, ADocumentCode, ANarrative);

            // Save the current amount on the AP account
            AAPAccountBalanceBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode).YtdActual;
            ABankAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode).YtdForeign;

            AExpAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.CostCentreCode).YtdActual;
            ARevalAccountBefore = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount,
                APInfos.CostCentreCode).YtdActual;

            VerificationResult = PostAPDocument(APInfos.ApDS, APInfos.PeriodStartDate, ref ADocumentIds);
            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            //
            // Guard Assert: Posting OK?
            //
            decimal ExpAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApDS.AApSupplier[0].DefaultExpAccount,
                APInfos.ApDS.AApSupplier[0].DefaultCostCentre).YtdActual;

            Assert.AreEqual(Math.Round(AAmount / AExchangeRatePosting, 2), Math.Round(ExpAccountAfter - AExpAccountBefore,
                    2), "after posting the invoice, the expense account should be debited the amount in base currency");

            return APInfos;
        }

        private AAPInfos PostAndPayForeignSupplierAPDocument(string ADocumentCode, out int APaymentNumber, out List <int>ADocumentIDs,
            out decimal AAPAccountBalanceBefore, out decimal ABankAccountBefore, out decimal ARevalAccountBefore)
        {
            decimal Amount = 100.0m;
            decimal ExchangeRatePosting = 1.2m;
            decimal ExchangeRatePayment = 1.1m;
            decimal ExpAccountBefore;
            TVerificationResultCollection VerificationResult;
            AAPInfos APInfos;

            APInfos = PostForeignSupplierAPDocument(Amount, ExchangeRatePosting, ADocumentCode, "Detail Item",
                out AAPAccountBalanceBefore, out ABankAccountBefore, out ExpAccountBefore, out ARevalAccountBefore, out ADocumentIDs);

            //
            // Pay the AP document
            //
            VerificationResult = PayAPDocument(APInfos.ApDS.AApDocument[0].ApDocumentId, Amount,
                APInfos.BankAccount, APInfos.CurrencyCode, APInfos.PeriodEndDate, out APaymentNumber, ExchangeRatePayment);
            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            // Save the current amount on the AP account and Bank Account
            decimal APAccountBalanceAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ApAccountCode, APInfos.CostCentreCode).YtdActual;
            decimal BankAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.BankAccount, APInfos.CostCentreCode).YtdForeign;

            //
            // Guard Assert: Paying OK?
            //
            // Check the amount on the AP account
            Assert.AreEqual(0.0m, APAccountBalanceAfter - AAPAccountBalanceBefore, "after paying the invoice, the AP account should be cleared");
            Assert.AreEqual((-1.0m) * Amount, BankAccountAfter - ABankAccountBefore, "after paying the invoice, the bank account should be credited");

            decimal RevalAccountAfter = new TGet_GLM_Info(FLedgerNumber,
                APInfos.ForexGainsLossesAccount, APInfos.CostCentreCode).YtdActual;

            Assert.AreEqual(
                Math.Round((Amount / ExchangeRatePayment) - (Amount / ExchangeRatePosting), 2),
                Math.Round((RevalAccountAfter - ARevalAccountBefore), 2),
                "after paying the invoice, the revaluation account should be credited with the forex gain");

            return APInfos;
        }

        private TVerificationResultCollection PayAPDocument(int AApDocumentId, decimal AAmount, string ABankAccount,
            string ACurrencyCode, DateTime APeriodEndDate, out int APaymentNumber, decimal? AExchangeRatePayment = null)
        {
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

            if (!TAPTransactionWebConnector.PostAPPayments(ref MainDS, APeriodEndDate, out VerificationResult))
            {
                Assert.Fail("Problems paying AP document: " +
                    VerificationResult.BuildVerificationResultString());
            }

            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            APaymentNumber = DocPayment.PaymentNumber;

            return VerificationResult;
        }

        private TVerificationResultCollection ReversePayment(int APaymentNumber, DateTime APeriodEndDate,
            List <int>ADocumentIds, AccountsPayableTDS AApDS)
        {
            TVerificationResultCollection VerificationResult;

            // "Un-pay" the specified invoice
            if (!TAPTransactionWebConnector.ReversePayment(FLedgerNumber,
                    APaymentNumber,
                    APeriodEndDate,
                    out VerificationResult))
            {
                Assert.Fail("Failed to reverse AP payment: " +
                    VerificationResult.BuildVerificationResultString());
            }

            Assert.That(VerificationResult, Is.Empty);  // Guard Assert

            // "Un-post" the specified invoice - returning it to "Approved" status!
            ADocumentIds[0] += 2; // The invoice I posted was reversed, and a duplicate now exists with an Id 2 greater than the original.

            return PostAPDocument(AApDS, APeriodEndDate, ref ADocumentIds, true);
        }

        #endregion
    }
}