//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using System.Threading;
using System.IO;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.IO.Testing
{
    ///  This is a test for retrieving data via http with THTTPUtils
    [TestFixture]
    public class TTestHttpClient
    {
        /// init
        [OneTimeSetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestServer.config");
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Client.DebugLevel", 0);
        }

        private void ReadWebsiteWith500Error()
        {
            THTTPUtils.ReadWebsite("http://localhost/api/serverMServerAdmin.asmx/TServerAdminWebConnector_StopServer");
        }

        /// test the THTTPUtils, for 500 error
        [Test]
        public void TestHttpUtils500()
        {
            // throws an exception and shows the 500 error message
            Assert.Throws <System.AggregateException>(ReadWebsiteWith500Error, "No Exception thrown despite 500 HTTP Error Code");
        }

        /// test the THTTPUtils, for retrieving data
        [Test]
        public void TestHttpUtils200()
        {
            string content = THTTPUtils.ReadWebsite("https://www.openpetra.org");
            StringAssert.Contains("Free Administration Software for Non-Profits", content, "should contain the text");
        }
    }
}
