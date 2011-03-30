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

namespace Ict.Testing.Petra.Server.MFinance.GL
{
    [TestFixture]
    public partial class TestGLRevaluation : CommonNUnitFunctions
    {
        [Test]
        /// <summary>
        /// Test for the internal format converter.
        /// </summary>
        public void Test_01_FormatConverter()
        {
            Assert.AreEqual(2, new FormatConverter(",>>>,>>9.99").digits, "Number of digits: 2");
            Assert.AreEqual(1, new FormatConverter(",>>>,>>9.9").digits, "Number of digits: 1");
            Assert.AreEqual(0, new FormatConverter(",>>>,>>9").digits, "Number of digits: 0");
            try
            {
                decimal d = new FormatConverter("nonsens").digits;
                Assert.Fail("No InternalException thrown");
            }
            catch (InternalException internalException)
            {
                Assert.AreEqual("GetCurrencyInfo.02", internalException.ErrorCode, "Wrong Error Code");
            }
            catch (Exception)
            {
                Assert.Fail("Other than InternalException thrown");
            }
        }

        /// <summary>
        /// Test of the internal routine GetCurrencyInfo
        /// </summary>
        [Test]
        public void Test_02_GetCurrencyInfo()
        {
            Assert.AreEqual(2, new GetCurrencyInfo("EUR").digits, "Number of digits: 2");
            try
            {
                decimal d = new GetCurrencyInfo("JPN").digits;
                Assert.Fail("No InternalException thrown");
            }
            catch (InternalException internalException)
            {
                Assert.AreEqual("GetCurrencyInfo.01", internalException.ErrorCode, "Wrong Error Code");
            }
            catch (Exception)
            {
                Assert.Fail("Other than InternalException thrown");
            }

            try
            {
                decimal d = new GetCurrencyInfo("DMG").digits;
                Assert.Fail("No InternalException thrown");
            }
            catch (InternalException internalException)
            {
                if (internalException.ErrorCode.Equals("GetCurrencyInfo.01"))
                {
                    Assert.Fail("Test Data are not loaded correctly");
                }
                else if (internalException.ErrorCode.Equals("GetCurrencyInfo.02"))
                {
                    Assert.Pass("DMG-Test ok");
                }
                else
                {
                    Assert.AreEqual("GetCurrencyInfo.02",
                        internalException.ErrorCode,
                        "Wrong Error Code");
                }
            }
            catch (Exception)
            {
                Assert.Fail("Other than InternalException thrown");
            }
        }

        [Test]
        [TestCase(10, 100, 2, 2, -40)]
        // "-40" means: 40 Currency Units are missing for the correct ratio
        [TestCase(200, 100, 2, 2, 150)]
        [TestCase(50, 100, 2, 2, 0)]
        [TestCase(50, 100, 1.123456789, 2, -39.01)]
        [TestCase(50, 100, 1.123456789, 4, -39.011)]
        [TestCase(50, 100, 1.123456789, 6, -39.010989)]
        public void Test_05_AccountDelta(decimal AAmountInBaseCurency,
            decimal AAmountInForeignCurrency,
            decimal AExchangeRate, int ACurrencyDigits, decimal AExpectedResult)
        {
            Assert.AreEqual(AExpectedResult, CLSRevaluation.AccountDelta(
                    AAmountInBaseCurency, AAmountInForeignCurrency, AExchangeRate, ACurrencyDigits));
        }
    }
}