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
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common.Verification;

using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


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
    public partial class TestGLPeriodicEndYear : CommonNUnitFunctions
    {
        private const int intLedgerNumber = 43;


        [Test]
        public void Test_YearEndFlagStatus()
        {
            ResetDatabase();
            THandleLedgerInfo ledgerInfo;
            int counter = 0;

            TVerificationResultCollection verificationResult;
            bool blnHaseErrors = TPeriodIntervallConnector.TPeriodYearEndInfo(
                intLedgerNumber, out verificationResult);

            bool messageHasBeenShown;

            Assert.GreaterOrEqual(verificationResult.Count, 1, "At least one message required");
            Assert.IsTrue(blnHaseErrors, "No Year End allowed ...");
            messageHasBeenShown = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                System.Diagnostics.Debug.WriteLine(verificationResult[i].ResultCode.ToString());

                if (verificationResult[i].ResultCode.Equals(TYearEndErrorStatus.PEYM_02.ToString()))
                {
                    messageHasBeenShown = true;
                }
            }

            Assert.IsTrue(messageHasBeenShown, "Correct message ...");

            do
            {
                ++counter;
                Assert.Greater(20, counter, "To many loops");

                // Set revaluation flag ...
                new TLedgerInitFlagHandler(intLedgerNumber,
                    TLedgerInitFlagEnum.Revaluation).Flag = true;
                blnHaseErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
                    intLedgerNumber, out verificationResult);
                ledgerInfo = new THandleLedgerInfo(intLedgerNumber);
            } while (!ledgerInfo.ProvisionalYearEndFlag);

            blnHaseErrors = TPeriodIntervallConnector.TPeriodYearEndInfo(
                intLedgerNumber, out verificationResult);

            Assert.AreEqual(0, verificationResult.Count, "No Error message shall be shown");
            Assert.IsFalse(blnHaseErrors, "Year End allowed ...");
        }

        [Test]
        public void Test_xxx()
        {
//            ResetDatabase();
//            THandleLedgerInfo ledgerInfo;
//            int counter = 0;
//
//            TVerificationResultCollection verificationResult;
//            bool blnHaseErrors = TPeriodIntervallConnector.TPeriodYearEndInfo(
//                intLedgerNumber, out verificationResult);
//
//            bool messageHasBeenShown;
//
//            Assert.GreaterOrEqual(verificationResult.Count, 1, "At least one message required");
//            Assert.IsTrue(blnHaseErrors, "No Year End allowed ...");
//            messageHasBeenShown = false;
//
//            for (int i = 0; i < verificationResult.Count; ++i)
//            {
//                System.Diagnostics.Debug.WriteLine(verificationResult[i].ResultCode.ToString());
//
//                if (verificationResult[i].ResultCode.Equals(TYearEndErrorStatus.PEYM_02.ToString()))
//                {
//                    messageHasBeenShown = true;
//                }
//            }
//
//            Assert.IsTrue(messageHasBeenShown, "Correct message ...");
//
//            do
//            {
//                ++counter;
//                Assert.Greater(20, counter, "To many loops");
//
//                // Set revaluation flag ...
//                new TLedgerInitFlagHandler(intLedgerNumber,
//                    TLedgerInitFlagEnum.Revaluation).Flag = true;
//                blnHaseErrors = TPeriodIntervallConnector.TPeriodMonthEnd(
//                    intLedgerNumber, out verificationResult);
//                ledgerInfo = new THandleLedgerInfo(intLedgerNumber);
//            } while (!ledgerInfo.ProvisionalYearEndFlag);


            TVerificationResultCollection verificationResult;
            bool blnHaseErrors = TPeriodIntervallConnector.TPeriodYearEnd(
                intLedgerNumber, out verificationResult);

            if (verificationResult.Count > 0)
            {
                for (int i = 0; i < verificationResult.Count; ++i)
                {
                    System.Diagnostics.Debug.WriteLine(verificationResult[i].ResultCode);
                }
            }

            Assert.AreEqual(0, verificationResult.Count, "No Error message shall be shown");
            Assert.IsFalse(blnHaseErrors, "Year End allowed ...");
        }

        [TestFixtureSetUp]
        public void Init()
        {
            InitServerConnection();
            //ResetDatabase();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            DisconnectServerConnection();
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
                LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL-Test\\" +
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