//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
//       timop
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using NUnit.Framework;
using MimeKit;
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

        /// this is used for the unit tests
        private string MailboxAddressListToString(List<MailboxAddress> AList)
        {
            string result = String.Empty;

            foreach (MailboxAddress addr in AList)
            {
                if (result.Length > 0)
                {
                    result += ", ";
                }

                result += addr.ToString();
            }

            return result;
        }

        /// <summary>
        /// A test.
        /// </summary>
        [Test]
        public void Test1_Single_addr_spec()
        {
            string test = "meepo.ratbagger@here.com";
            Assert.AreEqual(test, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// Commas are valid separators for .NET
        /// </summary>
        [Test]
        public void Test2_comma_separated_addr_spec()
        {
            string test = "me@here.com, him@there.com";
            Assert.AreEqual(test, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// Semicolons need to be converted to commas
        /// </summary>
        [Test]
        public void Test3_semicolon_separated_addr_spec()
        {
            string test = "me@here.com; him@there.com";
            string expected = "me@here.com, him@there.com";
            Assert.AreEqual(expected, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// Test when a display-name is added. First comma-separated...
        /// </summary>
        [Test]
        public void Test4_comma_separated_name_addr_quoted()
        {
            string test = "\"Meepo Ratbagger\" <me@here.com>, \"Mr.Oinky\" <him@there.com>";
            Assert.AreEqual(test, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// ...then semicolon-separeted
        /// </summary>
        [Test]
        public void Test5_semicolon_separated_name_addr_quoted()
        {
            string test = "\"Meepo Ratbagger\" <me@here.com>; \"Mr. Oinky\" <him@there.com>";
            string expc = "\"Meepo Ratbagger\" <me@here.com>, \"Mr. Oinky\" <him@there.com>";
            Assert.AreEqual(expc, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// Test that quoted semicolons aren't converted.
        /// </summary>
        [Test]
        public void Test6_semicolon_separated_name_addr_quoted_containing_semicolon()
        {
            string test = "\"Meepo;Ratbagger\" <me@here.com>; \"Mr.;Oinky\" <him@there.com>";
            string expc = "\"Meepo;Ratbagger\" <me@here.com>, \"Mr.;Oinky\" <him@there.com>";
            Assert.AreEqual(expc, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// Test that escaped quotes inside quotes still preserve semicolons as written.
        /// </summary>
        [Test]
        public void Test7_semicolon_separated_name_addr_containing_quoted_quotes()
        {
            string test = "\"test7Meepo; \\\"Meep;\\\" Ratbagger\" <me@here.com>; \"Mr. \\\"Oink;\\\" Oinky\" <him@there.com>";
            string expc = "\"test7Meepo; \\\"Meep;\\\" Ratbagger\" <me@here.com>, \"Mr. \\\"Oink;\\\" Oinky\" <him@there.com>";
            Assert.AreEqual(expc, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }

        /// <summary>
        /// And finally, that backslash-quoted semicolons also aren't converted.
        /// </summary>
        [Test]
        public void Test8_semicolon_separated_name_addr_backslash_quoted_containing_semicolon()
        {
            string test = "Meepo\\;Ratbagger <me@here.com>; Mr.\\;Oinky <him@there.com>";
            string expc = "\"Meepo;Ratbagger\" <me@here.com>, \"Mr.;Oinky\" <him@there.com>";
            Assert.AreEqual(expc, MailboxAddressListToString(TSmtpSender.ConvertAddressList(test)));
        }
    }
}
