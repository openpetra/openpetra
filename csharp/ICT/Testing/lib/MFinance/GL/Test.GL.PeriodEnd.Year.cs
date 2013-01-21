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
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;

using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Test of the GL.PeriodEnd.Year routines ...
    /// </summary>
    [TestFixture]
    public class TestGLPeriodicEndYear
    {
        private const int intLedgerNumber = 43;

        /// <summary>
        /// Test_YearEnd
        /// </summary>
        [Test]
        [Ignore("still fails and needs a review")]
        public void Test_YearEnd()
        {
            CommonNUnitFunctions.ResetDatabase();
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end.sql");
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end-account-property.sql");

            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(intLedgerNumber, "NUNIT");
            commonAccountingTool.AddBaseCurrencyJournal();
            commonAccountingTool.JournalDescription = "Test Data accounts";
            string strAccountGift = "0200";
            string strAccountBank = "6200";
            string strAccountExpense = "4100";

            // Accounting of some gifts ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4301", "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4302", "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 200);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4303", "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 300);

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountGift, "4301", "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountGift, "4302", "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 200);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountGift, "4303", "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 300);


            // Accounting of some expenses ...

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountExpense, "4301", "Expense Example", "Debit", MFinanceConstants.IS_DEBIT, 150);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountExpense, "4302", "Expense Example", "Debit", MFinanceConstants.IS_DEBIT, 150);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountExpense, "4303", "Expense Example", "Debit", MFinanceConstants.IS_DEBIT, 200);

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4301", "Expense Example", "Credit", MFinanceConstants.IS_CREDIT, 150);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4302", "Expense Example", "Credit", MFinanceConstants.IS_CREDIT, 150);
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "4303", "Expense Example", "Credit", MFinanceConstants.IS_CREDIT, 200);

            commonAccountingTool.CloseSaveAndPost();


            TVerificationResultCollection verificationResult = new TVerificationResultCollection();

            TCarryForward carryForward;

            bool blnLoop = true;

            while (blnLoop)
            {
                carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));

                if (carryForward.GetPeriodType == TCarryForwardENum.Year)
                {
                    blnLoop = false;
                }
                else
                {
                    carryForward.SetNextPeriod();
                }
            }

            TReallocation reallocation = new TReallocation(new TLedgerInfo(intLedgerNumber));
            reallocation.VerificationResultCollection = verificationResult;
            reallocation.IsInInfoMode = false;
            Assert.AreEqual(6, reallocation.JobSize, "Check the number of reallocation jobs ...");
            reallocation.RunEndOfPeriodOperation();

            reallocation = new TReallocation(new TLedgerInfo(intLedgerNumber));
            reallocation.VerificationResultCollection = verificationResult;
            reallocation.IsInInfoMode = true;
            Assert.AreEqual(0, reallocation.JobSize, "Check the number of reallocation jobs ...");

            int intYear = 0;
            CheckGLMEntry(intLedgerNumber, intYear, strAccountBank,
                -50, 0, 50, 0, 100, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountExpense,
                0, -150, 0, -150, 0, -200);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountGift,
                0, -100, 0, -200, 0, -300);

            TGlmNewYearInit glmNewYearInit = new TGlmNewYearInit(intLedgerNumber, intYear);
            glmNewYearInit.VerificationResultCollection = verificationResult;
            glmNewYearInit.IsInInfoMode = false;
            Assert.AreEqual(10, glmNewYearInit.JobSize, "Check the number of reallocation jobs ...");
            glmNewYearInit.RunEndOfPeriodOperation();
            glmNewYearInit = new TGlmNewYearInit(intLedgerNumber, intYear);
            glmNewYearInit.VerificationResultCollection = verificationResult;
            glmNewYearInit.IsInInfoMode = true;
            Assert.AreEqual(0, glmNewYearInit.JobSize, "Check the number of reallocation jobs ...");

            ++intYear;
            CheckGLMEntry(intLedgerNumber, intYear, strAccountBank,
                -50, 0, 50, 0, 100, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountExpense,
                0, 0, 0, 0, 0, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountGift,
                0, 0, 0, 0, 0, 0);

            TGlmInfo glmInfo = new TGlmInfo(intLedgerNumber, intYear, "8200");
            glmInfo.Reset();
            glmInfo.MoveNext();

            Assert.AreEqual(100, glmInfo.YtdActualBase);
            Assert.AreEqual(0, glmInfo.ClosingPeriodActualBase);
        }

        void CheckGLMEntry(int ALedgerNumber, int AYear, string AAccount,
            decimal cc1Base, decimal cc1Closing,
            decimal cc2Base, decimal cc2Closing,
            decimal cc3Base, decimal cc3Closing)
        {
            TGlmInfo glmInfo = new TGlmInfo(ALedgerNumber, AYear, AAccount);

            glmInfo.Reset();
            int intCnt = 0;
            bool blnFnd1 = false;
            bool blnFnd2 = false;
            bool blnFnd3 = false;

            TCacheable cache = new Ict.Petra.Server.MFinance.Cacheable.TCacheable();
            Type dummy;
            ACostCentreTable costcentres = (ACostCentreTable)cache.GetCacheableTable(TCacheableFinanceTablesEnum.CostCentreList,
                string.Empty,
                false,
                ALedgerNumber,
                out dummy);

            while (glmInfo.MoveNext())
            {
                TLogging.Log("glmInfo.CostCentreCode: " + glmInfo.CostCentreCode);

                if (glmInfo.CostCentreCode.Equals("4301"))
                {
                    Assert.AreEqual(cc1Base, glmInfo.YtdActualBase);
                    Assert.AreEqual(cc1Closing, glmInfo.ClosingPeriodActualBase);
                    blnFnd1 = true;
                }

                if (glmInfo.CostCentreCode.Equals("4302"))
                {
                    Assert.AreEqual(cc2Base, glmInfo.YtdActualBase);
                    Assert.AreEqual(cc2Closing, glmInfo.ClosingPeriodActualBase);
                    blnFnd2 = true;
                }

                if (glmInfo.CostCentreCode.Equals("4303"))
                {
                    Assert.AreEqual(cc3Base, glmInfo.YtdActualBase);
                    Assert.AreEqual(cc3Closing, glmInfo.ClosingPeriodActualBase);
                    blnFnd3 = true;
                }

                if (((ACostCentreRow)costcentres.Rows.Find(new object[] { ALedgerNumber, glmInfo.CostCentreCode })).PostingCostCentreFlag)
                {
                    ++intCnt;
                }
            }

            Assert.AreEqual(3, intCnt, "3 posting cost centres ...");
            Assert.IsTrue(blnFnd1);
            Assert.IsTrue(blnFnd2);
            Assert.IsTrue(blnFnd3);
        }

        /// <summary>
        /// Test of TAccountPeriodToNewYear
        /// </summary>
        [Test]
        public void Test_TAccountPeriodToNewYear()
        {
            CommonNUnitFunctions.ResetDatabase();

            TVerificationResultCollection verificationResult = new TVerificationResultCollection();

            int intLedgerNumber2010 = 2010;

            // create new ledger 2010 which is in year 2010
            TGLSetupWebConnector.CreateNewLedger(intLedgerNumber2010, "NUnit test 2010", "99", "EUR", "USD", new DateTime(2010,
                    1,
                    1), 12, 1, 8, out verificationResult);

            // We are in 2010 and this and 2011 is not a leap year
            TAccountPeriodToNewYear accountPeriodToNewYear =
                new TAccountPeriodToNewYear(intLedgerNumber2010, 2010);
            accountPeriodToNewYear.VerificationResultCollection = verificationResult;
            accountPeriodToNewYear.IsInInfoMode = false;

            // RunEndOfPeriodOperation ...
            Assert.AreEqual(12, accountPeriodToNewYear.JobSize, "JobSize before switching to 2011");
            accountPeriodToNewYear.RunEndOfPeriodOperation();

            // JobSize-Check ...
            TAccountPeriodToNewYear accountPeriodToNewYear2 =
                new TAccountPeriodToNewYear(intLedgerNumber2010, 2010);
            accountPeriodToNewYear2.IsInInfoMode = false;
            Assert.AreEqual(0, accountPeriodToNewYear2.JobSize, "JobSize after switching to 2011");

            TAccountPeriodInfo accountPeriodInfo = new TAccountPeriodInfo(intLedgerNumber2010);
            accountPeriodInfo.AccountingPeriodNumber = 2;
            Assert.AreEqual(2011, accountPeriodInfo.PeriodStartDate.Year, "Test of the year");
            Assert.AreEqual(28, accountPeriodInfo.PeriodEndDate.Day, "Test of the Feb. 28th");

            // Switch to 2012 - this is a leap year ...
            accountPeriodToNewYear = new TAccountPeriodToNewYear(intLedgerNumber2010, 2011);
            accountPeriodToNewYear.IsInInfoMode = false;
            Assert.AreEqual(12, accountPeriodToNewYear.JobSize, "JobSize before switching to 2012");
            accountPeriodToNewYear.RunEndOfPeriodOperation();
            Assert.AreEqual(0, accountPeriodToNewYear.JobSize, "JobSize after switching to 2012");

            accountPeriodInfo = new TAccountPeriodInfo(intLedgerNumber2010);
            accountPeriodInfo.AccountingPeriodNumber = 2;
            Assert.AreEqual(29, accountPeriodInfo.PeriodEndDate.Day, "Test of the Feb. 29th");
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
            //ResetDatabase();
            System.Diagnostics.Debug.WriteLine("Init: " + this.ToString());
        }

        /// <summary>
        /// TearDown the test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            TPetraServerConnector.Disconnect();
            System.Diagnostics.Debug.WriteLine("TearDown: " + this.ToString());
        }

        private const string strTestDataBatchDescription = "TestGLPeriodicEndMonth-TESTDATA";

        private void LoadTestTata_GetBatchInfo()
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ABatchRow template = new ABatchTable().NewRowTyped(false);

            template.BatchDescription = strTestDataBatchDescription;
            ABatchTable batches = ABatchAccess.LoadUsingTemplate(template, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();

            if (batches.Rows.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-batch-data.sql");
            }
        }

        private void UnloadTestData_GetBatchInfo()
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[0].Value = strTestDataBatchDescription;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "DELETE FROM PUB_" + ABatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + ABatchTable.GetBatchDescriptionDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }
}