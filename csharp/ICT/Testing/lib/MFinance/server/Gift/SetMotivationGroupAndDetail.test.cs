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
//
using System;
using Ict.Common;
using Ict.Testing.NUnitPetraServer;
using NUnit.Framework;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;


namespace Tests.MFinance.Server.Gift
{
    /// The Webconnector class TSetMotivationGroupAndDetail is tested
    [TestFixture]
    public class SetMotivationGroupAndDetailTest
    {
        const string SMTH = "GIFT"; // "SMTH"; // Means Something
        const string KMIN = "KEYMIN"; // Special Result ...

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
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
        /// Test for the value 0 -> There exist no partner with that ID.
        /// </summary>

        [Test]
        public void Test_nullPartner()
        {
            Int64 partnerKey = 0;
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = SMTH;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsFalse(partnerKeyIsValid, "Check if partnerKey=0 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(SMTH, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        /// Test for the value 1234567 -> There exist no partner with that ID.
        /// </summary>

        [Test]
        public void Test_invalidPartner()
        {
            Int64 partnerKey = 1234567;
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = SMTH;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsFalse(partnerKeyIsValid, "Check if partnerKey=1234567 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(SMTH, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Person()
        {
            Int64 partnerKey = 43005003;      // Valid Number for Pope, Dahlia
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = SMTH;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, "Check if partnerKey=43005003 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(SMTH, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Unit_WithoutKeyMin()
        {
            Int64 partnerKey = 43000000;      // Valid Number for the unit Germany
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = SMTH;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, "Check if partnerKey=43005003 does exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(SMTH, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Unit_WithKeyMin()
        {
            Int64 partnerKey = 73000000; //1800504;      // Valid Number for the unit ""Save The Forest""
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = KMIN;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, "Check if partnerKey=73000000 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(KMIN, motivationDetail, "motivationDetail must be changed to " + KMIN);
        }
    }
}