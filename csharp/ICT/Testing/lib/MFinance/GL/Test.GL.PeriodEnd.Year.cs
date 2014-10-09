//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2014 by OM International
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
        private int intLedgerNumber = 43;

        /// <summary>
        /// Test_YearEnd
        /// </summary>
        [Test]
        public void Test_YearEnd()
        {
            intLedgerNumber = CommonNUnitFunctions.CreateNewLedger();

            TLedgerInfo LedgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(0, LedgerInfo.CurrentFinancialYear, "Before YearEnd, we should be in year 0");

            TAccountPeriodInfo periodInfo = new TAccountPeriodInfo(intLedgerNumber, 1);
            Assert.AreEqual(new DateTime(DateTime.Now.Year,
                    1,
                    1), periodInfo.PeriodStartDate, "Calendar from base database should start with January 1st of this year");

            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end.sql", intLedgerNumber);
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end-account-property.sql",
                intLedgerNumber);

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

            commonAccountingTool.CloseSaveAndPost(); // returns true if posting seemed to work


            TVerificationResultCollection verificationResult = new TVerificationResultCollection();

            bool blnLoop = true;

            while (blnLoop)
            {

                if (LedgerInfo.ProvisionalYearEndFlag)
                {
                    blnLoop = false;
                }
                else
                {
                    TVerificationResultCollection VerificationResult;
                    TPeriodIntervalConnector.TPeriodMonthEnd(intLedgerNumber, false, out VerificationResult);
                    CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                        "Running MonthEnd gave critical error");
                }
            }

            // check before year end that income and expense accounts are not 0
            int intYear = 0;
            CheckGLMEntry(intLedgerNumber, intYear, strAccountBank,
                -50, 0, 50, 0, 100, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountExpense,
                150, 0, 150, 0, 200, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountGift,
                100, 0, 200, 0, 300, 0);

            // test that we cannot post to period 12 anymore, all periods are closed?
            LedgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(true, LedgerInfo.ProvisionalYearEndFlag, "Provisional YearEnd flag should be set");
            Assert.AreEqual(TYearEndProcessStatus.RESET_STATUS,
                (TYearEndProcessStatus)LedgerInfo.YearEndProcessStatus,
                "YearEnd process status should be still on RESET");

            TReallocation reallocation = new TReallocation(LedgerInfo);
            reallocation.VerificationResultCollection = verificationResult;
            reallocation.IsInInfoMode = false;
            Assert.AreEqual(6, reallocation.GetJobSize(), "Six reallocation jobs expected");
            reallocation.RunOperation();

            //
            // Now if I try to do the same thing again, it should find there's nothing to do:

            reallocation = new TReallocation(LedgerInfo);
            reallocation.VerificationResultCollection = verificationResult;
            reallocation.IsInInfoMode = true;
            Assert.AreEqual(0, reallocation.GetJobSize(), "After TReallocation, all reallocation jobs should be clear");

            // check amounts after reallocation
            CheckGLMEntry(intLedgerNumber, intYear, strAccountBank,
                -50, 0, 50, 0, 100, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountExpense,
                0, -150, 0, -150, 0, -200);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountGift,
                0, -100, 0, -200, 0, -300);

            // first run in info mode
            TPeriodIntervalConnector.TPeriodYearEnd(intLedgerNumber, true, out verificationResult);
            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(verificationResult,
                "YearEnd test should not have critical errors");

            // now run for real
            TPeriodIntervalConnector.TPeriodYearEnd(intLedgerNumber, false, out verificationResult);
            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(verificationResult,
                "YearEnd should not have critical errors");

            ++intYear;
            // check after year end that income and expense accounts are 0, bank account remains
            CheckGLMEntry(intLedgerNumber, intYear, strAccountBank,
                -50, 0, 50, 0, 100, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountExpense,
                0, 0, 0, 0, 0, 0);
            CheckGLMEntry(intLedgerNumber, intYear, strAccountGift,
                0, 0, 0, 0, 0, 0);

            // also check the glm period records
            CheckGLMPeriodEntry(intLedgerNumber, intYear, 1, strAccountBank,
                -50, 50, 100);
            CheckGLMPeriodEntry(intLedgerNumber, intYear, 1, strAccountExpense,
                0, 0, 0);
            CheckGLMPeriodEntry(intLedgerNumber, intYear, 1, strAccountGift,
                0, 0, 0);

            // 8200 is the account that the expenses and income from last year is moved to
            TGlmInfo glmInfo = new TGlmInfo(intLedgerNumber, intYear, "8200");
            glmInfo.Reset();
            glmInfo.MoveNext();

            Assert.AreEqual(100, glmInfo.YtdActualBase);
            Assert.AreEqual(0, glmInfo.ClosingPeriodActualBase);

            LedgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(1, LedgerInfo.CurrentFinancialYear, "After YearEnd, we are in a new financial year");
            Assert.AreEqual(1, LedgerInfo.CurrentPeriod, "After YearEnd, we are in Period 1");
            Assert.AreEqual(false, LedgerInfo.ProvisionalYearEndFlag, "After YearEnd, ProvisionalYearEnd flag should not be set");
            Assert.AreEqual(TYearEndProcessStatus.RESET_STATUS,
                (TYearEndProcessStatus)LedgerInfo.YearEndProcessStatus,
                "after year end, year end process status should be RESET");

            periodInfo = new TAccountPeriodInfo(intLedgerNumber, 1);
            Assert.AreEqual(new DateTime(DateTime.Now.Year + 1,
                    1,
                    1), periodInfo.PeriodStartDate, "new Calendar should start with January 1st of next year");
        }

        /// <summary>
        /// test 2 consecutive year ends
        /// </summary>
        [Test]
        public void Test_2YearEnds()
        {
            intLedgerNumber = CommonNUnitFunctions.CreateNewLedger();
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end.sql", intLedgerNumber);
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\test-sql\\gl-test-year-end-account-property.sql",
                intLedgerNumber);
            TLedgerInfo LedgerInfo = new TLedgerInfo(intLedgerNumber);
            for (int countYear = 0; countYear < 2; countYear++)
            {
                TLogging.Log("preparing year number " + countYear.ToString());

                // accounting one gift
                string strAccountGift = "0200";
                string strAccountBank = "6200";
                TCommonAccountingTool commonAccountingTool =
                    new TCommonAccountingTool(intLedgerNumber, "NUNIT");
                commonAccountingTool.AddBaseCurrencyJournal();
                commonAccountingTool.JournalDescription = "Test Data accounts";
                commonAccountingTool.AddBaseCurrencyTransaction(
                    strAccountBank, "4301", "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);
                commonAccountingTool.AddBaseCurrencyTransaction(
                    strAccountGift, "4301", "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);
                Boolean PostedOk = commonAccountingTool.CloseSaveAndPost(); // returns true if posting seemed to work
                Assert.AreEqual(true, PostedOk, "Test batch can't be posted");

                bool blnLoop = true;

                while (blnLoop)
                {
//                  System.Windows.Forms.MessageBox.Show(LedgerInfo.CurrentPeriod.ToString(), "MonthEnd Period");

                    if (LedgerInfo.ProvisionalYearEndFlag)
                    {
                        blnLoop = false;
                    }
                    else
                    {
                        TVerificationResultCollection VerificationResult;
                        TPeriodIntervalConnector.TPeriodMonthEnd(intLedgerNumber, false, out VerificationResult);
                        CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                            "MonthEnd gave critical error at Period" + LedgerInfo.CurrentPeriod + ":\r\n");
                    }
                }

                TLogging.Log("Closing year number " + countYear.ToString());
                TReallocation reallocation = new TReallocation(LedgerInfo);
                TVerificationResultCollection verificationResult = new TVerificationResultCollection();
                reallocation.VerificationResultCollection = verificationResult;
                reallocation.IsInInfoMode = false;
                Assert.AreEqual(1, reallocation.GetJobSize(), "Check 1 reallocation job is required");
                reallocation.RunOperation();

                TYearEnd YearEndOperator = new TYearEnd(LedgerInfo);
                TGlmNewYearInit glmNewYearInit = new TGlmNewYearInit(LedgerInfo, countYear, YearEndOperator);
                glmNewYearInit.VerificationResultCollection = verificationResult;
                glmNewYearInit.IsInInfoMode = false;
                Assert.Greater(glmNewYearInit.GetJobSize(), 0, "Check that NewYearInit has work to do");
                glmNewYearInit.RunOperation();
            }

            Assert.AreEqual(2, LedgerInfo.CurrentFinancialYear, "After YearEnd, Ledger is in year 2");

            TAccountPeriodInfo periodInfo = new TAccountPeriodInfo(intLedgerNumber, 1);
            Assert.AreEqual(new DateTime(DateTime.Now.Year + 2,
                    1,
                    1), periodInfo.PeriodStartDate, "new Calendar should start with January 1st of next year");
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
//              TLogging.Log("glmInfo.CostCentreCode: " + glmInfo.CostCentreCode);

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

        void CheckGLMPeriodEntry(int ALedgerNumber, int AYear, int APeriodNr, string AAccount,
            decimal cc1Base,
            decimal cc2Base,
            decimal cc3Base)
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
//              TLogging.Log("glmInfo.CostCentreCode: " + glmInfo.CostCentreCode);

                TGlmpInfo glmpInfo = new TGlmpInfo(-1, -1, glmInfo.GlmSequence, APeriodNr);

                Assert.AreEqual(true,
                    glmpInfo.RowExists,
                    "we cannot find a glm period record for " + glmInfo.AccountCode + " / " + glmInfo.CostCentreCode);

                if (glmInfo.CostCentreCode.Equals("4301"))
                {
                    Assert.AreEqual(cc1Base, glmpInfo.ActualBase);
                    blnFnd1 = true;
                }

                if (glmInfo.CostCentreCode.Equals("4302"))
                {
                    Assert.AreEqual(cc2Base, glmpInfo.ActualBase);
                    blnFnd2 = true;
                }

                if (glmInfo.CostCentreCode.Equals("4303"))
                {
                    Assert.AreEqual(cc3Base, glmpInfo.ActualBase);
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
            // create new ledger which is in year 2010
            int intLedgerNumber2010 = CommonNUnitFunctions.CreateNewLedger(new DateTime(2010, 1, 1));

            // We are in 2010 and this and 2011 is not a leap year
            TVerificationResultCollection verificationResult = new TVerificationResultCollection();
            TAccountPeriodToNewYear accountPeriodToNewYear = new TAccountPeriodToNewYear(intLedgerNumber2010);

            accountPeriodToNewYear.VerificationResultCollection = verificationResult;
            accountPeriodToNewYear.IsInInfoMode = false;

            // RunEndOfPeriodOperation ...
            accountPeriodToNewYear.RunOperation();

            TAccountPeriodInfo accountPeriodInfo = new TAccountPeriodInfo(intLedgerNumber2010);
            accountPeriodInfo.AccountingPeriodNumber = 2;
            Assert.AreEqual(2011, accountPeriodInfo.PeriodStartDate.Year, "Test of the year");
            Assert.AreEqual(28, accountPeriodInfo.PeriodEndDate.Day, "Test of the Feb. 28th");

            // Switch to 2012 - this is a leap year ...
            accountPeriodToNewYear = new TAccountPeriodToNewYear(intLedgerNumber2010);
            accountPeriodToNewYear.IsInInfoMode = false;
            accountPeriodToNewYear.RunOperation();

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
                    "test-sql\\gl-test-batch-data.sql", intLedgerNumber);
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