//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using Ict.Common;
using Ict.Petra.Shared;

namespace Ict.Petra.Shared.Testing
{
    ///  This is a testing program for Ict.Petra.Shared.dll
    [TestFixture]
    public class TTestShared
    {
        /// init the test
        [SetUp]
        public void Init()
        {
            new TLogging("test.log");
        }

        /// test conversions
        [Test]
        public void TestConversions_DateTimeToInt32AndViceVersa()
        {
            DateTime InitialDate;
            DateTime ResultDate;
            DateTime ExpectedDate;
            Int32 TimePart;

            // Comparison #1: with a time of 01:10:12
            InitialDate = new DateTime(2004, 1, 1, 01, 10, 12);

            TimePart = Conversions.DateTimeToInt32Time(InitialDate);

            ResultDate = Conversions.Int32TimeToDateTime(TimePart);
            ExpectedDate = DateTime.Now.Date + new TimeSpan(01, 10, 12);

            Assert.AreEqual(ExpectedDate, ResultDate, "Comparison #1 of ExpectedDate and ResultDate");


            // Comparison #1: with a time of 23:56:59
            InitialDate = new DateTime(2004, 1, 1, 23, 56, 59);

            TimePart = Conversions.DateTimeToInt32Time(InitialDate);

            ResultDate = Conversions.Int32TimeToDateTime(TimePart);
            ExpectedDate = DateTime.Now.Date + new TimeSpan(23, 56, 59);

            Assert.AreEqual(ExpectedDate, ResultDate, "Comparison #2 of ExpectedDate and ResultDate");
        }
    }
}