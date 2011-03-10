//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       <please insert your name>
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
//
using System;
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using Ict.Common;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MFinance; 

namespace Tests.MFinance.GL
{
    /// TODO write your comment here
    [TestFixture]
    public class TRevaluationTests
    {
    	int fLedgerNumber;
        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../../../../etc/TestServer.config");
            fLedgerNumber = 43;
        }

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        [Test]
        public void T01_GetCurrencyInfo()
        {
        	Assert.AreEqual(2, new GetCurrencyInfo("USD").digits, "GetCurrencyInfo of USD");
        	Assert.AreEqual(0, new GetCurrencyInfo("JPY").digits, "GetCurrencyInfo of Japanese Jen");
        }

        [Test]
        public void T02_GetAccountingPeriodInfo()
        {
        	// 1 = I assume that allways a first record exists ...
        	Assert.AreNotEqual(DateTime.MinValue, 
        	                   new GetAccountingPeriodInfo(fLedgerNumber).GetDatePeriodStart(1), 
        	                   "This value should not be DateTime.MinValue because this is an error value");
        	Assert.AreNotEqual(DateTime.MinValue, 
        	                   new GetAccountingPeriodInfo(fLedgerNumber).GetDatePeriodEnd(1), 
        	                   "This value should not be DateTime.MinValue because this is an error value");
        	Assert.AreNotEqual(DateTime.MinValue, 
        	                   new GetAccountingPeriodInfo(fLedgerNumber).GetEffectiveDateOfPeriod(1), 
        	                   "This value should not be DateTime.MinValue because this is an error value");
        }

        
        // GetAccountingPeriodInfo(intLedgerNum).GetEffectiveDateOfPeriod(..)
        
        /// <summary>
        /// Some test, please add comment
        /// </summary>
        [Test]
        public void T03_Revaluation()
        {
            string[] currencies = new string[2];
            currencies[0] = "GBP";
            currencies[1] = "YEN";
            decimal[] rates = new decimal[2];
            rates[0] = 1.234m;
            rates[1] = 2.345m;
            TRevaluationWebConnector.Revaluate(43, "EUR",
                "5300", "3700",
                currencies, rates);
        }
    }
}