//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
        private const int intLedgerNumber = 43;

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

        /// <summary>
        /// Post a document and pay for it
        /// </summary>
        [Test]
        public void SimpleDocumentPostingAndPayment()
        {
            // Create an AP document for the demo supplier
            AccountsPayableTDS MainDS = TTransactionWebConnector.CreateAApDocument(intLedgerNumber, SUPPLIER_PARTNER_KEY, false);

            AApSupplierAccess.LoadByPrimaryKey(MainDS, SUPPLIER_PARTNER_KEY, null);
            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(intLedgerNumber, null);

            MainDS.AApDocument[0].DocumentCode = "Test" + DateTime.Now.Ticks.ToString();

            MainDS.Merge(TTransactionWebConnector.CreateAApDocumentDetail(
                    intLedgerNumber,
                    MainDS.AApDocument[0].ApDocumentId,
                    MainDS.AApSupplier[0].DefaultExpAccount,
                    MainDS.AApSupplier[0].DefaultCostCentre,
                    399.0m,
                    MainDS.AApDocument[0].LastDetailNumber + 1));

            MainDS.AApDocument[0].LastDetailNumber++;
            MainDS.AApDocument[0].TotalAmount = 399.0m;
            MainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
            MainDS.AApDocumentDetail[0].Narrative = "Test";

            TVerificationResultCollection VerificationResult;

            if (TTransactionWebConnector.SaveAApDocument(ref MainDS, out VerificationResult)
                != TSubmitChangesResult.scrOK)
            {
                Assert.Fail("Problems saving AP document: " +
                    VerificationResult.BuildVerificationResultString());
            }

            DateTime PeriodStartDate, PeriodEndDate;
            TFinancialYear.GetStartAndEndDateOfPeriod(intLedgerNumber, LedgerTable[0].CurrentPeriod, out PeriodStartDate, out PeriodEndDate, null);

            // Post the AP document
            List <int>documentIds = new List <int>();
            documentIds.Add(MainDS.AApDocument[0].ApDocumentId);

            if (!TTransactionWebConnector.PostAPDocuments(intLedgerNumber,
                    documentIds,
                    PeriodStartDate,
                    false, out VerificationResult))
            {
                Assert.Fail("Problems posting AP document: " +
                    VerificationResult.BuildVerificationResultString());
            }

            // Pay the AP document
            int ApDocumentId = MainDS.AApDocument[0].ApDocumentId;
            string BankAccount = MainDS.AApSupplier[0].DefaultBankAccount;

            MainDS = new AccountsPayableTDS();
            AApPaymentRow payment = MainDS.AApPayment.NewRowTyped();
            payment.LedgerNumber = intLedgerNumber;
            payment.PaymentNumber = -1;
            payment.Amount = 399.0m;
            payment.BankAccount = BankAccount;
            MainDS.AApPayment.Rows.Add(payment);

            AApDocumentPaymentRow docPayment = MainDS.AApDocumentPayment.NewRowTyped();
            docPayment.LedgerNumber = intLedgerNumber;
            docPayment.ApDocumentId = ApDocumentId;
            docPayment.Amount = 399.0m;
            docPayment.PaymentNumber = payment.PaymentNumber;
            MainDS.AApDocumentPayment.Rows.Add(docPayment);

            if (!TTransactionWebConnector.PostAPPayments(ref MainDS, PeriodEndDate, out VerificationResult))
            {
                Assert.Fail("Problems paying AP document: " +
                    VerificationResult.BuildVerificationResultString());
            }
        }
    }
}