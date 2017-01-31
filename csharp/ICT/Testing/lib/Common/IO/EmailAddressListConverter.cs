//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
//
// Copyright 2004-2012 by OM International
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
using NUnit.Framework;
using Ict.Common.IO;

namespace Tests.Common.IO
{
    /// <summary>
    /// Tests conversion of email address lists from semicolon-separated to comma-separated.
    /// </summary>
    [TestFixture]
    public class TTestEmailAddressListConverter
    {
        /// <summary>
        /// Perform necessary setup.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
        }

        /// <summary>
        /// Perform any tear-down.
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// A test.
        /// </summary>
        [Test]
        public void Test1_Single_addr_spec()
        {
            Assert.AreEqual("meepo.ratbagger@here.com", TSmtpSender.ConvertAddressList("meepo.ratbagger@here.com"));
        }

        /// <summary>
        /// Commas are valid separators for .NET
        /// </summary>
        [Test]
        public void Test2_comma_separated_addr_spec()
        {
            Assert.AreEqual("me@here.com, him@there.com", TSmtpSender.ConvertAddressList("me@here.com, him@there.com"));
        }

        /// <summary>
        /// Semicolons need to be converted to commas
        /// </summary>
        [Test]
        public void Test3_semicolon_separated_addr_spec()
        {
            Assert.AreEqual("me@here.com, him@there.com", TSmtpSender.ConvertAddressList("me@here.com; him@there.com"));
        }

        /// <summary>
        /// Test when a display-name is added. First comma-separated...
        /// </summary>
        [Test]
        public void Test4_comma_separated_name_addr_quoted()
        {
            Assert.AreEqual("\"Meepo Ratbagger\" <me@here.com>, \"Mr.Oinky\" <him@there.com>",
                TSmtpSender.ConvertAddressList("\"Meepo Ratbagger\" <me@here.com>, \"Mr.Oinky\" <him@there.com>"));
        }

        /// <summary>
        /// ...then semicolon-separeted
        /// </summary>
        [Test]
        public void Test5_semicolon_separated_name_addr_quoted()
        {
            Assert.AreEqual("\"Meepo Ratbagger\" <me@here.com>, \"Mr. Oinky\" <him@there.com>",
                TSmtpSender.ConvertAddressList("\"Meepo Ratbagger\" <me@here.com>; \"Mr. Oinky\" <him@there.com>"));
        }

        /// <summary>
        /// Test that quoted semicolons aren't converted.
        /// </summary>
        [Test]
        public void Test6_semicolon_separated_name_addr_quoted_containing_semicolon()
        {
            Assert.AreEqual("\"Meepo;Ratbagger\" <me@here.com>, \"Mr.;Oinky\" <him@there.com>",
                TSmtpSender.ConvertAddressList("\"Meepo;Ratbagger\" <me@here.com>; \"Mr.;Oinky\" <him@there.com>"));
        }

        /// <summary>
        /// Test that escaped quotes inside quotes still preserve semicolons as written.
        /// </summary>
        [Test]
        public void Test7_semicolon_separated_name_addr_containing_quoted_quotes()
        {
            Assert.AreEqual("\"Meepo; \\\"Meep;\\\" Ratbagger\" <me@here.com>, \"Mr. \\\"Oink;\\\" Oinky\" <him@there.com>",
                TSmtpSender.ConvertAddressList("\"Meepo; \\\"Meep;\\\" Ratbagger\" <me@here.com>; \"Mr. \\\"Oink;\\\" Oinky\" <him@there.com>"));
        }

        /// <summary>
        /// And finally, that backslash-quoted semicolons also aren't converted.
        /// </summary>
        [Test]
        public void Test8_semicolon_separated_name_addr_backslash_quoted_containing_semicolon()
        {
            Assert.AreEqual("Meepo\\;Ratbagger <me@here.com>, Mr.\\;Oinky <him@there.com>",
                TSmtpSender.ConvertAddressList("Meepo\\;Ratbagger <me@here.com>; Mr.\\;Oinky <him@there.com>"));
        }
    }
}