//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Common;
using NUnit.Extensions.Forms;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// TestCommonAccountingTool
    /// </summary>
    [TestFixture]
    public class TestCommonAccountingTool
    {
        int LedgerNumber = 43;


        /// <summary>
        /// This routine tests the TLedgerInitFlagHandler completely. It's the routine
        /// which writes "boolean" values to a data base table. The class TGet_GLM_Info is
        /// tested indirect too.
        ///
        /// Be careful by changing this routine. The behaviour has been compared to this of
        /// petra and it has been verfied that the same database entries of the table
        /// a_transaction has been created and the same changes in glm.
        /// </summary>
        [Test]
        public void Test_01_BaseCurrencyAccounting()
        {
            // <summary>
            // 6000 is defined as debit Account and so an accounting in "debit direction" is
            // added as a positive value to GLM.
            // 9800 is defined as credit Account and so an accounting in "credit direction" is
            // added as a positive value to GLM.
            // </summary>

            string strAccountStart = "6000";
            string strAccountEnd = "9800";
            string strCostCentre = "4300";

            // Get the glm-values before and after the test and taking the differences enables
            // to run the test several times
            TGet_GLM_Info getGLM_InfoBeforeStart = new TGet_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            TGet_GLM_Info getGLM_InfoBeforeEnd = new TGet_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit-Test-10", "NUNIT",
                MFinanceConstants.IS_DEBIT, 10);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit-Test 10", "NUNIT",
                MFinanceConstants.IS_CREDIT, 10);
            commonAccountingTool.CloseSaveAndPost(); // returns intBatchNumber

            TGet_GLM_Info getGLM_InfoAfterStart = new TGet_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            TGet_GLM_Info getGLM_InfoAfterEnd = new TGet_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            // strAccountStart is a debit account -> in this case "+"
            Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual + 10, getGLM_InfoAfterStart.YtdActual,
                "Check if 10 has been accounted to " + strAccountStart);
            // strAccountEnd is a credit acount -> in this case "+" too!
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual + 10, getGLM_InfoAfterEnd.YtdActual,
                "Check if 10 has been accounted to " + strAccountEnd);

            commonAccountingTool =
                new TCommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit-Test-5", "NUNIT",
                MFinanceConstants.IS_CREDIT, 5);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit-Test 5", "NUNIT",
                MFinanceConstants.IS_DEBIT, 5);
            commonAccountingTool.CloseSaveAndPost(); // returns intBatchNumber

            getGLM_InfoAfterStart = new TGet_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            getGLM_InfoAfterEnd = new TGet_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);
            // now both directions are "-" and so the difference is reduced to 5
            Assert.AreEqual(getGLM_InfoBeforeStart.YtdActual + 5, getGLM_InfoAfterStart.YtdActual,
                "Check if 10 has been accounted");
            // strAccountEnd is a credit acount -> in this case "+" too!
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdActual + 5, getGLM_InfoAfterEnd.YtdActual,
                "Check if 10 has been accounted");
        }

        private void PrepareTestCaseData()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            // Check if some special test data are available - otherwise load ...
            bool AccountTestCasesAvailable = AAccountAccess.Exists(LedgerNumber, "6001", Transaction);
            bool CostCentreTestCasesAvailable = ACostCentreAccess.Exists(LedgerNumber, "4301", Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            if (!AccountTestCasesAvailable)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-account-data.sql", LedgerNumber);
            }

            if (!CostCentreTestCasesAvailable)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-costcentre-data.sql", LedgerNumber);
            }
        }

        /// <summary>
        /// Tests the foreign Currency part of the TCommonAccountingTool.
        /// </summary>
        [Test]
        public void Test_02_ForeignCurrencyAccounting()
        {
            string strAccountStart = "6001";      // Use an foreign currency account in GBP only
            string strAccountEnd = "9800";        // Use a base currency account only
            string strCostCentre = "4300";

            PrepareTestCaseData();

            // Get the glm-values before and after the test and taking the differences enables
            // to run the test several times
            TGet_GLM_Info getGLM_InfoBeforeStart = new TGet_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            TGet_GLM_Info getGLM_InfoBeforeEnd = new TGet_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(LedgerNumber, "NUNIT");

            decimal ExchangeRateEurToGBP = 0.85m;
            decimal AmountInGBP = 100.0m;
            decimal AmountInEUR = (1.0m / ExchangeRateEurToGBP) * AmountInGBP;
            commonAccountingTool.AddForeignCurrencyJournal("GBP", ExchangeRateEurToGBP);

            commonAccountingTool.AddForeignCurrencyTransaction(
                strAccountStart, strCostCentre, "Debit GBP 100", "NUNIT",
                MFinanceConstants.IS_DEBIT, AmountInEUR, AmountInGBP);
            commonAccountingTool.AddForeignCurrencyTransaction(
                strAccountEnd, strCostCentre, "Credit GBP 100", "NUNIT",
                MFinanceConstants.IS_CREDIT, AmountInEUR, AmountInGBP);

            commonAccountingTool.CloseSaveAndPost(); // returns intBatchNumber

            TGet_GLM_Info getGLM_InfoAfterStart = new TGet_GLM_Info(LedgerNumber, strAccountStart, strCostCentre);
            TGet_GLM_Info getGLM_InfoAfterEnd = new TGet_GLM_Info(LedgerNumber, strAccountEnd, strCostCentre);

            Assert.AreEqual(Math.Round(getGLM_InfoBeforeStart.YtdActual + AmountInEUR, 2), Math.Round(getGLM_InfoAfterStart.YtdActual, 2),
                "Check if base currency has been accounted to " + strAccountStart);
            Assert.AreEqual(Math.Round(getGLM_InfoBeforeEnd.YtdActual + AmountInEUR, 2), Math.Round(getGLM_InfoAfterEnd.YtdActual, 2),
                "Check if base currency has been accounted to " + strAccountEnd);

            Assert.AreEqual(getGLM_InfoBeforeStart.YtdForeign + AmountInGBP, getGLM_InfoAfterStart.YtdForeign,
                "Check if foreign currency has been accounted");
            Assert.AreEqual(getGLM_InfoBeforeEnd.YtdForeign, getGLM_InfoAfterEnd.YtdForeign,
                "Check if nothing foreign has been accounted on the non foreign currency account");
        }

        /// <summary>
        /// Check an spefic error ...
        /// </summary>
        [Test]
        public void Test_03_ForeignCurrencyAccountingWithWrongForeignValue()
        {
            string strAccountStart = "6001";      // Use an foreign currency account in GBP only
            string strAccountEnd = "9800";        // Use a base currency account only
            string strCostCentre = "4300";

            PrepareTestCaseData();

            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(LedgerNumber, "NUNIT");
            try
            {
                commonAccountingTool.AddForeignCurrencyJournal("JPY", 0.3m);
                commonAccountingTool.AddForeignCurrencyTransaction(
                    strAccountStart, strCostCentre, "Credit GBP 100", "NUNIT",
                    MFinanceConstants.IS_DEBIT, 100, 333.33m);
                commonAccountingTool.AddForeignCurrencyTransaction(
                    strAccountEnd, strCostCentre, "Debit GBP 100", "NUNIT",
                    MFinanceConstants.IS_CREDIT, 100, 333.33m);
                Assert.Fail("Exception does not appear!");
            }
            catch (EVerificationException)
            {
                // Exception was thrown, which is expected
                // Assert.Pass will throw an exception NUnit.Framework.SuccessException and fail the test???
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                Assert.Fail("Wrong exception thrown");
            }
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
    }
}