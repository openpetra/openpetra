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
using System.IO;
using System.Collections;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Common.Verification;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Test of the GL.PeriodEnd.Month routines ...
    /// </summary>
    [TestFixture]
    public class TestGLPeriodicEndMonth
    {
        private int FLedgerNumber;
        private TLedgerInfo FledgerInfo;

        /// <summary>
        ///
        /// </summary>
        public TestGLPeriodicEndMonth()
        {
            FLedgerNumber = 43;
            FledgerInfo = new TLedgerInfo(FLedgerNumber);
        }

        /// <summary>
        /// Tests if unposted batches are detected correctly
        /// </summary>
        [Test]
        public void Test_PEMM_02_UnpostedBatches()
        {
            // System.Diagnostics.Debug.WriteLine(
            UnloadTestData_GetBatchInfo();
            Assert.AreEqual(0, new GetBatchInfo(
                    FLedgerNumber, FledgerInfo.CurrentPeriod).NumberOfBatches, "No unposted batch shall be found");

            LoadTestData_GetBatchInfo();

            Assert.AreEqual(2, new GetBatchInfo(
                    FLedgerNumber, FledgerInfo.CurrentPeriod).NumberOfBatches, "Two of the four batches shall be found");
            //UnloadTestData_GetBatchInfo();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, true, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_06.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Critical,
                        "Value shall be of type critical ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message hase been shown");
            Assert.IsTrue(blnHasErrors, "This is a Critical Message");
            UnloadTestData_GetBatchInfo();
        }

        /// <summary>
        /// Tests if suspended accounts are detected correctly
        /// </summary>
        [Test]
        public void Test_PEMM_03_SuspensedAccounts()
        {
            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(FLedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();
            commonAccountingTool.JournalDescription = "Test Data accounts";
            string strAccountBank = "6000";
            // Accounting of some gifts ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, "7300", "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);
            commonAccountingTool.AddBaseCurrencyTransaction(
                "0100", "7300", "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);
            commonAccountingTool.CloseSaveAndPost(); // returns true if posting seemed to work

            new ChangeSuspenseAccount(FLedgerNumber, strAccountBank).Suspense();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, false, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_07.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.AreEqual(TResultSeverity.Resv_Status, verificationResult[i].ResultSeverity,
                        "Value shall be status only ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message has been shown");
            Assert.IsFalse(blnHasErrors, "there should not be an error for closing the first period");

            int periodCounter = 2;

            while (!blnHasErrors && periodCounter < 12)
            {
                blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                    FLedgerNumber, false, out verificationResult);

                Assert.IsFalse(blnHasErrors, "there was an error closing period " + periodCounter.ToString());
                periodCounter++;
                Assert.AreEqual(periodCounter, new TLedgerInfo(FLedgerNumber).CurrentPeriod, "should be in new period");
            }

            blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, false, out verificationResult);

            blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_07.ToString())
                    && (TResultSeverity.Resv_Critical == verificationResult[i].ResultSeverity))
                {
                    blnStatusArrived = true;
                }
            }

            Assert.IsTrue(blnStatusArrived, "there should be  a critical error PEEC_07 for the suspense account");
            Assert.IsTrue(blnHasErrors, "there should be an error because we cannot close the last period due to suspense account with a balance");

            new ChangeSuspenseAccount(FLedgerNumber, strAccountBank).Unsuspense();
        }

        private void ImportGiftBatch()
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());
            FileContent = FileContent.Replace("{thisyear}-01-01", DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd"));

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);
            Assert.True(TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult),
                "Failed to import the test gift batch.  The file contains critical error(s).");
        }

        /// <summary>
        /// Test for unposted gift batches ...
        /// </summary>
        [Test]
        public void Test_PEMM_04_UnpostedGifts()
        {
            ImportGiftBatch();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, true, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_08.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Critical,
                        "Value shall be of type critical ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Message has not been shown");
            Assert.IsTrue(blnHasErrors, "This is a Critical Message");
        }

        /// <summary>
        /// Check for the revaluation status ...
        /// </summary>
        [Test]
        public void Test_PEMM_05_Revaluation()
        {
            FLedgerNumber = CommonNUnitFunctions.CreateNewLedger();
            // load foreign currency account 6001
            CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                "test-sql\\gl-test-account-data.sql", FLedgerNumber);

            // post a batch for foreign currency account 6001
            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(FLedgerNumber, "NUNIT");
            commonAccountingTool.AddForeignCurrencyJournal("GBP", 1.1m);
            commonAccountingTool.JournalDescription = "Test foreign currency account";
            string strAccountGift = "0200";
            string strAccountBank = "6001";

            // Accounting of some gifts ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, (FLedgerNumber * 100).ToString(), "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);

            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountGift, (FLedgerNumber * 100).ToString(), "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);

            commonAccountingTool.CloseSaveAndPost(); // returns true if posting seemed to work


            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, true, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_05.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Critical,
                        "we need a critical error: need to run revaluation first ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message has been shown");
            Assert.IsTrue(blnHasErrors, "should fail because revaluation needs to be run first");

            // run revaluation
            blnHasErrors = TRevaluationWebConnector.Revaluate(FLedgerNumber, new TLedgerInfo(
                    FLedgerNumber).CurrentPeriod, new string[] { "GBP" }, new decimal[] { 1.2m }, out verificationResult);

            TLogging.Log(verificationResult.BuildVerificationResultString());
            Assert.IsFalse(blnHasErrors, "Problem running the revaluation");

            blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                FLedgerNumber, true, out verificationResult);
            Assert.IsFalse(blnHasErrors, "should now be able to close the month now that the revaluation has been run");
        }

        /// <summary>
        /// Move to the next month
        /// </summary>
        [Test]
        public void Test_SwitchToNextMonth()
        {
            FLedgerNumber = CommonNUnitFunctions.CreateNewLedger();
            TLedgerInfo ledgerInfo = new TLedgerInfo(FLedgerNumber);
            int counter = 0;

            do
            {
                Int32 CurrentPeriod = ledgerInfo.CurrentPeriod;
                ++counter;
                Assert.Greater(20, counter, "Too many loops");

                // Set revaluation flag ...
                new TLedgerInitFlagHandler(FLedgerNumber,
                    TLedgerInitFlagEnum.Revaluation).Flag = true;

                // Run MonthEnd ...
                TVerificationResultCollection verificationResult;
                bool blnHasErrors = TPeriodIntervalConnector.TPeriodMonthEnd(
                    FLedgerNumber, false, out verificationResult);

                if (!ledgerInfo.ProvisionalYearEndFlag)
                {
                    Assert.AreEqual(CurrentPeriod + 1,
                        ledgerInfo.CurrentPeriod, "Period increment");
                }

                Assert.IsFalse(blnHasErrors, "Month end without any error");
                System.Diagnostics.Debug.WriteLine("Counter: " + counter.ToString());
            } while (!ledgerInfo.ProvisionalYearEndFlag);
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
            FLedgerNumber = CommonNUnitFunctions.CreateNewLedger();

            // add costcentre 7300 for gift batch
            ACostCentreTable CostCentres = new ACostCentreTable();
            ACostCentreRow CCRow = CostCentres.NewRowTyped();
            CCRow.LedgerNumber = FLedgerNumber;
            CCRow.CostCentreCode = "7300";
            CCRow.CostCentreName = "7300";
            CCRow.CostCentreType = MFinanceConstants.FOREIGN_CC_TYPE;
            CCRow.CostCentreToReportTo = MFinanceConstants.INTER_LEDGER_HEADING;
            CCRow.PostingCostCentreFlag = true;
            CCRow.CostCentreActiveFlag = true;
            CostCentres.Rows.Add(CCRow);

            ACostCentreAccess.SubmitChanges(CostCentres, null);

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

        private void LoadTestData_GetBatchInfo()
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ABatchRow template = new ABatchTable().NewRowTyped(false);

            template.LedgerNumber = FLedgerNumber;
            template.BatchDescription = strTestDataBatchDescription;
            ABatchTable batches = ABatchAccess.LoadUsingTemplate(template, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();

            if (batches.Rows.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-batch-data.sql", FLedgerNumber);
            }
        }

        private void UnloadTestData_GetBatchInfo()
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = FLedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[1].Value = strTestDataBatchDescription;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "DELETE FROM PUB_" + ABatchTable.GetTableDBName() + " ";
            strSQL += "WHERE " + ABatchTable.GetLedgerNumberDBName() + " = ? " +
                      "AND " + ABatchTable.GetBatchDescriptionDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }

    class ChangeSuspenseAccount
    {
        int ledgerNumber;
        string strAcount;
        public ChangeSuspenseAccount(int ALedgerNumber, string AAccount)
        {
            ledgerNumber = ALedgerNumber;
            strAcount = AAccount;
        }

        public void Suspense()
        {
            try
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = ledgerNumber;
                ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
                ParametersArray[1].Value = strAcount;

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                string strSQL = "INSERT INTO PUB_" + ASuspenseAccountTable.GetTableDBName() + " ";
                strSQL += "(" + ASuspenseAccountTable.GetLedgerNumberDBName();
                strSQL += "," + ASuspenseAccountTable.GetSuspenseAccountCodeDBName() + ") ";
                strSQL += "VALUES ( ? , ? )";

                DBAccess.GDBAccessObj.ExecuteNonQuery(strSQL, transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception)
            {
                Assert.Fail("No database access to run the test");
            }
        }

        public void Unsuspense()
        {
            try
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = ledgerNumber;
                ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
                ParametersArray[1].Value = strAcount;

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                string strSQL = "DELETE FROM PUB_" + ASuspenseAccountTable.GetTableDBName() + " ";
                strSQL += "WHERE " + ASuspenseAccountTable.GetLedgerNumberDBName() + " = ? ";
                strSQL += "AND " + ASuspenseAccountTable.GetSuspenseAccountCodeDBName() + " = ? ";

                DBAccess.GDBAccessObj.ExecuteNonQuery(strSQL, transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception)
            {
                Assert.Fail("No database access to run the test");
            }
        }
    }
}