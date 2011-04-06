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
using Ict.Petra.Server.MFinance.GL;
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
    [TestFixture]
    public partial class TestGLPeriodicEndMonth : CommonNUnitFunctions
    {
        private const int intLedgerNumber = 43;


        [Test]
        public void Test_PYEF_01()
        {
            new SetLedgerParameter(intLedgerNumber).ProvisionalYearEndFlag = true;
            Assert.True(new GetLedgerInfo(intLedgerNumber).ProvisionalYearEndFlag,
                "Cannot start test because ProvisionalYearEndFlag cannot be changed");

            TVerificationResultCollection verificationResult;
            bool blnResult = TPeriodMonthConnector.TPeriodMonthEndInfo(intLedgerNumber, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals("PYEF-01"))
                {
                    blnStatusArrived = true;
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message PYEF-01 is shown");
            Assert.IsFalse(blnResult, "PYEF-01 is not a Critital Message");

            new SetLedgerParameter(intLedgerNumber).ProvisionalYearEndFlag = false;
        }

        /// <summary>
        /// PYEF-02 is the status for a set of unpostetd batches. This error is non critical,
        /// which means that further tests are done and this status is one of a set of different stati,
        /// but a Period-End-Month calculation is not allowed in this case.
        /// </summary>
        [Test]
        public void Test_PYEF_02()
        {
            GetLedgerInfo ledgerInfo = new GetLedgerInfo(intLedgerNumber);

            // System.Diagnostics.Debug.WriteLine(
            UnloadTestData_GetBatchInfo();
            Assert.AreEqual(0, new GetBatchInfo(
                    intLedgerNumber, ledgerInfo.CurrentPeriod).NumberOfBatches, "No unposted batch shall be found");

            LoadTestTata_GetBatchInfo();

            Assert.AreEqual(2, new GetBatchInfo(
                    intLedgerNumber, ledgerInfo.CurrentPeriod).NumberOfBatches, "Two of the four batches shall be found");
            //UnloadTestData_GetBatchInfo();

            TVerificationResultCollection verificationResult;
            bool blnResult = TPeriodMonthConnector.TPeriodMonthEndInfo(intLedgerNumber, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals("PYEF-02"))
                {
                    blnStatusArrived = true;
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message PYEF-02 is shown");
            Assert.IsFalse(blnResult, "PYEF-02 is not a Critital Message");
            UnloadTestData_GetBatchInfo();
        }

        [Test]
        public void Test_PYEF_03()
        {
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Unsuspense();
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Suspense();

            TVerificationResultCollection verificationResult;
            bool blnResult = TPeriodMonthConnector.TPeriodMonthEndInfo(intLedgerNumber, out verificationResult);
            bool blnStatusArrived = false;

            for (int i = 0; i < verificationResult.Count; ++i)
            {
                if (verificationResult[i].ResultCode.Equals("PYEF-03"))
                {
                    blnStatusArrived = true;
                    Assert.IsTrue(verificationResult[i].ResultSeverity == TResultSeverity.Resv_Status,
                        "Value shall be status only ...");
                }
            }

            Assert.IsTrue(blnStatusArrived, "Status message PYEF-03 is shown");
            Assert.IsFalse(blnResult, "PYEF-03 is not a Critital Message");
            new SetDeleteSuspenseAccount(intLedgerNumber, "6000").Unsuspense();
        }

        [TestFixtureSetUp]
        public void Init()
        {
            InitServerConnection();
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
            }
            ;
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
            }
            ;
        }
    }
}