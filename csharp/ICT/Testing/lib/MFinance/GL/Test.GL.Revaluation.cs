//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2011 by OM International
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
    /// <summary>
    /// TestGLRevaluation - math part
    /// </summary>
    [TestFixture]
    public partial class TestGLRevaluation
    {
        /// <summary>
        /// Test_05_AccountDelta
        /// </summary>
        /// <param name="AAmountInBaseCurency"></param>
        /// <param name="AAmountInForeignCurrency"></param>
        /// <param name="AExchangeRate"></param>
        /// <param name="ACurrencyDigits"></param>
        /// <param name="AExpectedResult"></param>
        // "-40" means: 40 Currency Units are missing for the correct ratio
        [TestCase(10, 100, 2, 2, 40)]
        [TestCase(200, 100, 2, 2, -150)]
        [TestCase(50, 100, 2, 2, 0)]
        [TestCase(50, 100, 1.123456789, 2, 39.01)]
        [TestCase(50, 100, 1.123456789, 4, 39.011)]
        [TestCase(50, 100, 1.123456789, 6, 39.010989)]
        public void Test_05_AccountDelta(decimal AAmountInBaseCurency,
            decimal AAmountInForeignCurrency,
            decimal AExchangeRate, int ACurrencyDigits, decimal AExpectedResult)
        {
            Assert.AreEqual(AExpectedResult, CLSRevaluation.AccountDelta(
                    AAmountInBaseCurency, AAmountInForeignCurrency, AExchangeRate, ACurrencyDigits));
        }
    }
}