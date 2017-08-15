//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2017 by OM International
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
using System.Collections.Generic;
using NUnit.Framework;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Testing.NUnitPetraServer;

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    /// <summary>
    /// TestGLCommonTools
    /// </summary>
    [TestFixture]
    public class TestGLCommonTools
    {
        int FLedgerNumber = 43;
        /// <summary>
        /// This routine tests the TLedgerInitFlagHandler completely. It's the routine
        /// which writes "boolean" values to a data base table.
        /// </summary>
        [Test]
        public void Test_01_TLedgerInitFlagHandler()
        {
            bool blnOld = new TLedgerInitFlag(43, "RevalTest").IsSet;

            new TLedgerInitFlag(FLedgerNumber, "RevalTest").IsSet = true;
            Assert.IsTrue(new TLedgerInitFlag(
                    FLedgerNumber, "RevalTest").IsSet, "Flag was set a line before");
            TLedgerInitFlag.SetOrRemoveFlag(FLedgerNumber, "RevalTest", true);
            Assert.IsTrue(new TLedgerInitFlag(
                    FLedgerNumber, "RevalTest").IsSet, "Flag was set a line before");
            new TLedgerInitFlag(FLedgerNumber, "RevalTest").IsSet = false;
            Assert.IsFalse(new TLedgerInitFlag(
                    FLedgerNumber, "RevalTest").IsSet, "Flag was reset a line before");
            TLedgerInitFlag.SetOrRemoveFlag(FLedgerNumber, "RevalTest", false);
            Assert.IsFalse(new TLedgerInitFlag(
                    FLedgerNumber, "RevalTest").IsSet, "Flag was reset a line before");

            TLedgerInitFlag.SetFlagComponent(FLedgerNumber, "RevalTest", "A");
            TLedgerInitFlag.SetFlagComponent(FLedgerNumber, "RevalTest", "B");
            TLedgerInitFlag.SetFlagComponent(FLedgerNumber, "RevalTest", "C");
            TLedgerInitFlag.RemoveFlagComponent(FLedgerNumber, "RevalTest", "B");
            String NewVal = TLedgerInitFlag.GetFlagValue(FLedgerNumber, "RevalTest");
            Assert.IsTrue(NewVal == "A,C", "Flag Value of 'RevalTest' should be 'A,C' but is '" + NewVal + "'");
            new TLedgerInitFlag(FLedgerNumber, "RevalTest").IsSet = blnOld;
        }

        /// <summary>
        /// Test of the TLedgerInfo Routine...
        /// </summary>
        [Test]
        public void Test_02_TLedgerInfo()
        {
            Assert.AreEqual("EUR", new TLedgerInfo(FLedgerNumber).BaseCurrency,
                String.Format("Base Currency of {0} shall be EUR", FLedgerNumber));
            Assert.AreEqual("5003", new TLedgerInfo(FLedgerNumber).RevaluationAccount,
                String.Format("Revaluation Account of {0} shall be 5003", FLedgerNumber));
        }

        /// <summary>
        /// Test_03_TAccountPeriodInfo
        /// </summary>
        [Test]
        public void Test_03_TAccountPeriodInfo()
        {
            TAccountPeriodInfo getAPI = new TAccountPeriodInfo(FLedgerNumber);

            getAPI.AccountingPeriodNumber = 2;

            Assert.AreNotEqual(DateTime.MinValue, getAPI.PeriodStartDate,
                "Simple Date Check");
            Assert.AreNotEqual(DateTime.MinValue, getAPI.PeriodEndDate,
                "Simple Date Check");
        }

        /// <summary>
        /// Test for the internal format converter.
        /// </summary>
        [Test]
        public void Test_04_FormatConverter()
        {
            Assert.AreEqual(2, new FormatConverter(",>>>,>>9.99").digits, "Number of digits: 2");
            Assert.AreEqual(1, new FormatConverter(",>>>,>>9.9").digits, "Number of digits: 1");
            Assert.AreEqual(0, new FormatConverter(",>>>,>>9").digits, "Number of digits: 0");
            try
            {
                new FormatConverter("nonsens");
                Assert.Fail("No InternalException thrown");
            }
            catch (EVerificationException internalException)
            {
                Assert.AreEqual("TCurrencyInfo03", internalException.ErrorCode, "Wrong Error Code");
            }
            catch (Exception)
            {
                Assert.Fail("Other than InternalException thrown");
            }
        }

        private void CreateInvalidCurrency()
        {
            new TCurrencyInfo("JPN");
        }

        /// <summary>
        /// Test of the internal routine TCurrencyInfo
        /// </summary>
        [Test]
        public void Test_05_TCurrencyInfo()
        {
            Assert.AreEqual(2, new TCurrencyInfo("EUR").digits, "Number of digits: 2");

            Assert.Throws <EVerificationException>(CreateInvalidCurrency, "No InternalException thrown");
            try
            {
                CreateInvalidCurrency();
            }
            catch (EVerificationException internalException)
            {
                Assert.AreEqual("TCurrencyInfo02", internalException.ErrorCode, "Wrong Error Code");
            }
            catch (Exception e)
            {
                Assert.Fail("Other than InternalException thrown: " + e.GetType().ToString());
            }
        }

        /// <summary>
        /// CurrencyInfo supports two currencies and the conversion rules incluing the
        /// rouding based on
        /// </summary>
        [Test]
        public void Test_05_TCurrencyInfo_2()
        {
            TCurrencyInfo getCurrencyInfo = new TCurrencyInfo("EUR", "JPY");

            Assert.AreEqual(1.23m, getCurrencyInfo.RoundBaseCurrencyValue(1.23456m), "Round to 2 digits");
            Assert.AreEqual(1.0m, getCurrencyInfo.RoundForeignCurrencyValue(1.23456m), "Round to 0 digits");

            decimal exchangeRate = 1 / 119.7295m;

            Assert.AreEqual(0.84m, getCurrencyInfo.ToBaseValue(100.00m, exchangeRate),
                "Conversion from 100 YEN to 0.83 EUR");
            Assert.AreEqual(11973, getCurrencyInfo.ToForeignValue(100.00m, exchangeRate),
                "Conversion from 100 EUR to 11983 YEN");
            Assert.AreEqual(120, getCurrencyInfo.ToForeignValue(1.00m, exchangeRate),
                "Conversion from 1 EUR to 120 YEN");

            getCurrencyInfo.ForeignCurrencyCode = "GBP";     // Change foreign Currency to Pound ...
            exchangeRate = 1 / 0.8801m;

            Assert.AreEqual(113.62m, getCurrencyInfo.ToBaseValue(100.00m, exchangeRate),
                "Conversion from 100 GBP to 113.62 EUR");
            Assert.AreEqual(88.01m, getCurrencyInfo.ToForeignValue(100.00m, exchangeRate),
                "Conversion from 100 EUR to 88.01 GBP");
        }

        /// <summary>
        /// Test of the Routines HasNoChilds and ChildList
        /// of TGetAccountHierarchyDetailInfo
        /// </summary>
        [Test]
        public void Test_08_TGetAccountHierarchyDetailInfo()
        {
            TGetAccountHierarchyDetailInfo gahdi = new TGetAccountHierarchyDetailInfo(FLedgerNumber);

            Assert.IsTrue(gahdi.HasNoChildren("6800"), "Base Account without children");
            Assert.IsFalse(gahdi.HasNoChildren("6800S"), "Root Account");
            List <String>list = gahdi.GetChildren("7000S");
            Assert.AreEqual(2, list.Count, "Two entries ...");
            Assert.AreEqual("7000", list[0], "7000 is the first account");
            Assert.AreEqual("7010", list[1], "7010 is the second account");
            Assert.AreEqual("7000S", gahdi.GetParentAccount("7010"));

            List <String>list2 = gahdi.GetChildren("ASSETS");
            Assert.AreEqual(41, list2.Count, "Currently 41 child entries");
        }

        /// <summary>
        /// Test_09_TAccountPropertyHandler
        /// </summary>
        [Test]
        public void Test_09_TAccountPropertyHandler()
        {
            TLedgerInfo tHandleLedgerInfo = new TLedgerInfo(FLedgerNumber);
            TAccountInfo tHandleAccountInfo = new TAccountInfo(tHandleLedgerInfo.LedgerNumber);

            tHandleAccountInfo.SetSpecialAccountCode(TAccountPropertyEnum.ICH_ACCT);
            Assert.IsTrue(tHandleAccountInfo.IsValid, "ICH_ACCT shall exist");
            Assert.AreEqual("8500", tHandleAccountInfo.AccountCode);
        }

        private bool TryGetAccountPeriodInfo(int ALedgerNum, int APeriodNum)
        {
            try
            {
                new TAccountPeriodInfo(ALedgerNum, APeriodNum);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
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
