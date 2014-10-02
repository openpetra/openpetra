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
using Ict.Common;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    class TestOperation : AbstractPeriodEndOperation
    {
        int intCount;
        int intJobCount;
        int intOperationCount;

        public TestOperation(int ACount)
        {
            intCount = ACount;
            intOperationCount = 0;
        }

        public override AbstractPeriodEndOperation GetActualizedClone()
        {
            return new TestOperation(intCount + 1);
        }

        public override int GetJobSize()
        {
            return intJobCount;
        }

        public void SetJobSize(int ASize)
        {
            intJobCount = ASize;
        }

        public int GetOperationCount()
        {
            return intOperationCount;
        }

        public override void RunOperation()
        {
            Assert.AreEqual(1, intCount);
            ++intOperationCount;
            intJobCount = 0;
        }
    }

    class TestOperations : TPeriodEndOperations
    {
        /// <summary></summary>
        public void Test1(TVerificationResultCollection tvr)
        {
            FverificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = true;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }

        /// <summary></summary>
        public void Test2(TVerificationResultCollection tvr)
        {
            FverificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = false;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }

        /// <summary></summary>
        public override void SetNextPeriod(TCarryForward carryForward)
        {
        }
    }


    /// <summary>
    /// Test of the GL.PeriodEnd.Year routines ...
    /// </summary>
    [TestFixture]
    public partial class TestGLPeriodicEnd
    {
        private int intLedgerNumber = 43;

        /// <summary>
        /// Some very basic tests of TPeriodEndOperations and AbstractPeriodEndOperation
        /// </summary>
        [Test]
        public void Test_AbstractPeriodEndOperation()
        {
            TVerificationResultCollection tvr = new TVerificationResultCollection();
            TestOperations periodEndOperations = new TestOperations();

            periodEndOperations.Test1(tvr);
            periodEndOperations.Test2(tvr);
        }

        /// <summary>
        /// Carry Forward manages the switch form Month to Month including year end ...
        /// </summary>
        [Test]
        public void Test_TCarryForward()
        {
            intLedgerNumber = CommonNUnitFunctions.CreateNewLedger();
            TCarryForward carryForward;

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                TLedgerInfo ledgerInfo = new TLedgerInfo(intLedgerNumber);
                Assert.AreEqual(i, ledgerInfo.CurrentPeriod, "Current period should be " + i.ToString());
                carryForward = new TCarryForward(ledgerInfo);
                Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month,
                    "Month: " + i.ToString());
                carryForward.SetNextPeriod();
            }

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Year, "Next Year");
            carryForward.SetNextPeriod();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month, "Next Month");
            carryForward.SetNextPeriod();
        }

        /// <summary>
        /// Test_TCarryForwardYear
        /// </summary>
        [Test]
        public void Test_TCarryForwardYear()
        {
            intLedgerNumber = CommonNUnitFunctions.CreateNewLedger();
            TCarryForward carryForward = null;

            int CurrentYear = new TAccountPeriodInfo(intLedgerNumber, 1).PeriodStartDate.Year;
            Assert.AreEqual(DateTime.Now.Year, CurrentYear, "new ledger should be in current year");

            TLedgerInfo ledgerInfo = null;

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                ledgerInfo = new TLedgerInfo(intLedgerNumber);
                Assert.AreEqual(i, ledgerInfo.CurrentPeriod, "Current period should be " + i.ToString());
                carryForward = new TCarryForward(ledgerInfo);
                Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month,
                    "Month: " + i.ToString());
                carryForward.SetNextPeriod();
            }

            ledgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(true, ledgerInfo.ProvisionalYearEndFlag, "provisional year end flag should be set");
            Assert.AreEqual(false, ledgerInfo.YearEndFlag, "year end has not been run yet");
            Assert.AreEqual(TYearEndProcessStatus.RESET_STATUS,
                (TYearEndProcessStatus)ledgerInfo.YearEndProcessStatus,
                "year end process status should be still on RESET");

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            carryForward.SetNextPeriod();

            TLedgerInfo LedgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(1, LedgerInfo.CurrentFinancialYear, "after year end, we are in a new financial year");
            Assert.AreEqual(1, LedgerInfo.CurrentPeriod, "after year end, we are in Period 1");
            Assert.AreEqual(true, LedgerInfo.YearEndFlag, "after year end, year end flag should be set, because it has been run");
            Assert.AreEqual(false, LedgerInfo.ProvisionalYearEndFlag, "after year end, provisional year end flag should not be set");
            Assert.AreEqual(TYearEndProcessStatus.RESET_STATUS,
                (TYearEndProcessStatus)LedgerInfo.YearEndProcessStatus,
                "after year end, year end process status should be RESET");

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            carryForward.SetNextPeriod();
            LedgerInfo = new TLedgerInfo(intLedgerNumber);
            Assert.AreEqual(2, LedgerInfo.CurrentPeriod, "new month, period 2");

            TAccountPeriodInfo periodInfo = new TAccountPeriodInfo(intLedgerNumber, 1);
            Assert.AreEqual(new DateTime(CurrentYear + 1,
                    1,
                    1), periodInfo.PeriodStartDate, "new Calendar should start with January 1st of next year");
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
    }
}