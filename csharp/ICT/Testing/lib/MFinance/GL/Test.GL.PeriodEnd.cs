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
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Common;

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

        public override int JobSize {
            get
            {
                return intJobCount;
            }
        }

        public void SetJobSize(int ASize)
        {
            intJobCount = ASize;
        }

        public int GetOperationCount()
        {
            return intOperationCount;
        }

        public override void RunEndOfPeriodOperation()
        {
            Assert.AreEqual(1, intCount);
            ++intOperationCount;
            intJobCount = 0;
        }
    }

    class TestOperations : TPeriodEndOperations
    {
        public void Test1(TVerificationResultCollection tvr)
        {
            verificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = true;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }

        public void Test2(TVerificationResultCollection tvr)
        {
            verificationResults = tvr;
            TestOperation testOperation = new TestOperation(1);
            testOperation.SetJobSize(12);
            testOperation.IsInInfoMode = false;
            RunPeriodEndSequence(testOperation, "Message");
            Assert.AreEqual(1, testOperation.GetOperationCount());
        }
    }


    /// <summary>
    /// Test of the GL.PeriodEnd.Year routines ...
    /// </summary>
    [TestFixture]
    public partial class TestGLPeriodicEnd
    {
        private const int intLedgerNumber = 43;

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
            CommonNUnitFunctions.ResetDatabase();
            TCarryForward carryForward;

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
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
            CommonNUnitFunctions.ResetDatabase();
            TCarryForward carryForward = null;
            TVerificationResultCollection tvr = new TVerificationResultCollection();

            for (int i = 1; i < 13; ++i)  // 12 Months
            {
                carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
                Assert.AreEqual(carryForward.GetPeriodType, TCarryForwardENum.Month,
                    "Month: " + i.ToString());
                carryForward.SetNextPeriod();
            }

            Assert.AreEqual(DateTime.Now.Year, carryForward.Year, "Standard");
            TAccountPeriodToNewYear accountPeriodToNewYear =
                new TAccountPeriodToNewYear(intLedgerNumber, DateTime.Now.Year);
            accountPeriodToNewYear.IsInInfoMode = false;
            accountPeriodToNewYear.VerificationResultCollection = tvr;
            accountPeriodToNewYear.RunEndOfPeriodOperation();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            Assert.AreEqual(DateTime.Now.Year, carryForward.Year, "Non standard ...");
            carryForward.SetNextPeriod();

            carryForward = new TCarryForward(new TLedgerInfo(intLedgerNumber));
            carryForward.SetNextPeriod();

            TLedgerInitFlagHandler ledgerInitFlag =
                new TLedgerInitFlagHandler(intLedgerNumber, TLedgerInitFlagEnum.ActualYear);
            ledgerInitFlag.AddMarker(DateTime.Now.Year.ToString());
            Assert.IsFalse(ledgerInitFlag.Flag, "Should be deleted ...");
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
    }
}