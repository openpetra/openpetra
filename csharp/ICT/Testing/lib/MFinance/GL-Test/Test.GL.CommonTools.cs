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
using NUnit.Framework;
using Ict.Testing.NUnitForms;
using Ict.Petra.Server.MFinance.GL;


using Ict.Common;


namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Tests for some common Tools.
    /// </summary>
    [TestFixture]
    public partial class TestGLCommonTools : CommonNUnitFunctions
    {
        int LedgerNumber = 43;
        /// <summary>
        /// This routine tests the TLedgerInitFlagHandler completely. It's the routine
        /// which writes "boolean" values to a data base table.
        /// </summary>
        [Test]
        public void Test_01_TLedgerInitFlagHandler()
        {
            bool blnOld = new TLedgerInitFlagHandler(43, LegerInitFlag.Revaluation).Flag;

            new TLedgerInitFlagHandler(LedgerNumber, LegerInitFlag.Revaluation).Flag = true;
            Assert.IsTrue(new TLedgerInitFlagHandler(
                    LedgerNumber, LegerInitFlag.Revaluation).Flag, "Flag was set a line before");
            new TLedgerInitFlagHandler(LedgerNumber, LegerInitFlag.Revaluation).Flag = true;
            Assert.IsTrue(new TLedgerInitFlagHandler(
                    LedgerNumber, LegerInitFlag.Revaluation).Flag, "Flag was set a line before");
            new TLedgerInitFlagHandler(LedgerNumber, LegerInitFlag.Revaluation).Flag = false;
            Assert.IsFalse(new TLedgerInitFlagHandler(
                    LedgerNumber, LegerInitFlag.Revaluation).Flag, "Flag was reset a line before");
            new TLedgerInitFlagHandler(LedgerNumber, LegerInitFlag.Revaluation).Flag = false;
            Assert.IsFalse(new TLedgerInitFlagHandler(
                    LedgerNumber, LegerInitFlag.Revaluation).Flag, "Flag was reset a line before");
            new TLedgerInitFlagHandler(LedgerNumber, LegerInitFlag.Revaluation).Flag = blnOld;
        }

        /// <summary>
        /// Test of the GetLedgerInfo Routine...
        /// </summary>
        [Test]
        public void Test_02_GetLedgerInfo()
        {
            Assert.AreEqual("EUR", new GetLedgerInfo(LedgerNumber).BaseCurrency,
                String.Format("Base Currency of {0} shall be EUR", LedgerNumber));
            Assert.AreEqual("5003", new GetLedgerInfo(LedgerNumber).RevaluationAccount,
                String.Format("Revaluation Account of {0} shall be 5003", LedgerNumber));
        }

        [Test]
        public void Test_03_GetAccountingPeriodInfo()
        {
            GetAccountingPeriodInfo getAPI = new GetAccountingPeriodInfo(LedgerNumber);

            Assert.AreNotEqual(DateTime.MinValue, getAPI.GetDatePeriodEnd(1),
                "DateTime.MinValue is an error representative");
            Assert.AreNotEqual(DateTime.MinValue, getAPI.GetDatePeriodStart(1),
                "DateTime.MinValue is an error representative");
            Assert.AreNotEqual(DateTime.MinValue, getAPI.GetEffectiveDateOfPeriod(1),
                "DateTime.MinValue is an error representative");

            Assert.AreEqual(DateTime.MinValue, getAPI.GetDatePeriodEnd(33),
                "DateTime.MinValue is an error representative");
            Assert.AreEqual(DateTime.MinValue, getAPI.GetDatePeriodStart(33),
                "DateTime.MinValue is an error representative");
            Assert.AreEqual(DateTime.MinValue, getAPI.GetEffectiveDateOfPeriod(33),
                "DateTime.MinValue is an error representative");

            Assert.IsTrue(TryGetAccountPeriodInfo(LedgerNumber, 1),
                "This request shall pass");
            Assert.IsFalse(TryGetAccountPeriodInfo(LedgerNumber, 100),
                "This request shall fail (period does not exist)");
        }

        private bool TryGetAccountPeriodInfo(int ALedgerNum, int APeriodNum)
        {
            try
            {
                GetAccountingPeriodInfo getAPI = new GetAccountingPeriodInfo(ALedgerNum, APeriodNum);
                DateTime date = getAPI.EffectiveDate;
                return date != null;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
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
    }
}