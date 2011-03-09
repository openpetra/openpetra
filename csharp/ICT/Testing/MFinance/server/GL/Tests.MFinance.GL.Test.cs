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

namespace Tests.MFinance.GL
{
    /// TODO write your comment here
    [TestFixture]
    public class TMyTest
    {
        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../../../../etc/TestServer.config");
        }

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// Some test, please add comment
        /// </summary>
        [Test]
        public void Revaluation()
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