//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MConference.Applications;
using Ict.Common.Data;

namespace Tests.MConference.OnlineBackend
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TConferenceOnlineBackendTest
    {
        static Int64 EventPartnerKey;
        static string EventCode;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");

            EventPartnerKey = TAppSettingsManager.GetInt64("ConferenceTool.EventPartnerKey", 1110198);
            EventCode = TAppSettingsManager.GetValue("ConferenceTool.EventCode", "SC001CNGRSS08");
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// refresh attendees.
        /// used this test for debugging
        /// </summary>
        [Test, Explicit]
        public void TestRefreshAttendees()
        {
            TAttendeeManagement.RefreshAttendees(EventPartnerKey);
        }

        /// <summary>
        /// print badges.
        /// used this test for debugging
        /// </summary>
        [Test, Explicit]
        public void TestPrintBadges()
        {
            string PDFPath = TConferenceBadges.PrintBadges(EventPartnerKey,
                EventCode,
                22000000,
                "",
                false,
                false);

            TLogging.Log("TestPrintBadges: check pdf file " + PDFPath);
        }
    }
}