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
        private const int intLedgerNumber = 43;

        /// <summary>
        /// Tests if unposted batches are detected correctly
        /// </summary>
        [Test]
        public void Test_PEMM_02_UnpostedBatches()
        {
            TLedgerInfo ledgerInfo = new TLedgerInfo(intLedgerNumber);

            // System.Diagnostics.Debug.WriteLine(
            UnloadTestData_GetBatchInfo();
            Assert.AreEqual(0, new GetBatchInfo(
                    intLedgerNumber, ledgerInfo.CurrentPeriod).NumberOfBatches, "No unposted batch shall be found");

            LoadTestTata_GetBatchInfo();

            Assert.AreEqual(2, new GetBatchInfo(
                    intLedgerNumber, ledgerInfo.CurrentPeriod).NumberOfBatches, "Two of the four batches shall be found");
            //UnloadTestData_GetBatchInfo();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                intLedgerNumber, true, out verificationResult);
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
            Assert.IsTrue(blnHasErrors, "This is a Critital Message");
            UnloadTestData_GetBatchInfo();
        }

        /// <summary>
        /// Tests if suspended accounts are detected correctly
        /// </summary>
        [Test]
        public void Test_PEMM_03_SuspensedAccounts()
        {
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Suspense();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                intLedgerNumber, true, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_07.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Status,
                        "Value shall be status only ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message has been shown");
            Assert.IsTrue(blnHasErrors, "This is a Critital Message");
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Unsuspense();
        }

        private void ImportGiftBatch()
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", intLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);
        }

        /// <summary>
        /// Test for unposted gift batches ...
        /// </summary>
        [Test]
        public void Test_PEMM_04_UnpostedGifts()
        {
            ImportGiftBatch();

            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                intLedgerNumber, true, out verificationResult);
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
            TVerificationResultCollection verificationResult;
            bool blnHasErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                intLedgerNumber, true, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals(
                        TPeriodEndErrorAndStatusCodes.PEEC_05.ToString()))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Critical,
                        "Value shall be of type critical ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message has been shown");
            Assert.IsTrue(blnHasErrors, "This is a Critital Message");
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Unsuspense();
        }

        /// <summary>
        /// Move to the next month
        /// </summary>
        [Test]
        public void Test_SwitchToNextMonth()
        {
            CommonNUnitFunctions.ResetDatabase();
            TLedgerInfo ledgerInfo1;
            TLedgerInfo ledgerInfo2;
            int counter = 0;

            do
            {
                ++counter;
                Assert.Greater(20, counter, "Too many loops");

                // Set revaluation flag ...
                new TLedgerInitFlagHandler(intLedgerNumber,
                    TLedgerInitFlagEnum.Revaluation).Flag = true;

                ledgerInfo1 = new TLedgerInfo(intLedgerNumber);
                // Period end now shall run ...
                TVerificationResultCollection verificationResult;
                bool blnHasErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                    intLedgerNumber, false, out verificationResult);

                ledgerInfo2 = new TLedgerInfo(intLedgerNumber);

                if (!ledgerInfo2.ProvisionalYearEndFlag)
                {
                    Assert.AreEqual(ledgerInfo1.CurrentPeriod + 1,
                        ledgerInfo2.CurrentPeriod, "counter ok");
                }

                Assert.IsFalse(blnHasErrors, "Month end without any error");
                System.Diagnostics.Debug.WriteLine("Counter: " + counter.ToString());
            } while (!ledgerInfo2.ProvisionalYearEndFlag);
        }

        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect();
            CommonNUnitFunctions.ResetDatabase();
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


    class SetDeleteSuspenseAccount
    {
        int ledgerNumber;
        string strAcount;
        public SetDeleteSuspenseAccount(int ALedgerNumber, string AAccount)
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