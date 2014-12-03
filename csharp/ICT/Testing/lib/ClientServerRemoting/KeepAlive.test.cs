//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.IO;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework.Constraints;

namespace Ict.Testing.ClientServerRemoting
{
    ///  This will test what happens if the client disappears / fails to send keepalive signals
    [TestFixture]
    public class TTestKeepAlive
    {
        /// init the test
        [SetUp]
        public void Init()
        {
            Catalog.Init();
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestClient.config");
            CommonNUnitFunctions.InitRootPath();

            // just to be sure the server is not running
            CommonNUnitFunctions.nant("stopServer", false);
        }

        /// <summary>
        /// clean up
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // clean up
//            CommonNUnitFunctions.nant("stopServer", false);
        }

        /// <summary>
        /// test keep alive signal
        /// </summary>
        [Test]
        [Ignore("We know that this doesn't work yet! (According to Timo)")]
        public void TestKeepAliveWithoutTimeout()
        {
            CommonNUnitFunctions.StartOpenPetraServer("-D:Server.ClientKeepAliveTimeoutAfterXSeconds_LAN=10");

            try
            {
                // check number of connected clients
                string connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("* no connected Clients *", connectedClientsMessage, "there should not be any clients connected at the moment");

                TPetraConnector.Connect("../../etc/TestClient.config", "<add key=\"ServerObjectKeepAliveIntervalInSeconds\" value=\"5\"/>");

                connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("Connected since", connectedClientsMessage, "there should be one client connected");

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(15.0).TotalMilliseconds));

                connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("Connected since", connectedClientsMessage, "the client should still be connected, no timeout");

                TPetraConnector.Disconnect();

                connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("* no connected Clients *", connectedClientsMessage, "there should not be any clients connected anymore");
            }
            finally
            {
                // clean up
                CommonNUnitFunctions.nant("stopServer", false);
            }
        }

        /// <summary>
        /// test keep alive signal
        /// </summary>
        [Test]
        [Ignore("We know that this doesn't work yet! (According to Timo)")]
        public void TestKeepAliveWithTimeout()
        {
            // TODORemoting
            // set keep alive signal on client and server in a way that the server thinks the client has gone away
            CommonNUnitFunctions.StartOpenPetraServer("-D:Server.ClientKeepAliveTimeoutAfterXSeconds_LAN=10");

            try
            {
                TPetraConnector.Connect("../../etc/TestClient.config", "<add key=\"ServerObjectKeepAliveIntervalInSeconds\" value=\"20\"/>");

                string connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("Connected since", connectedClientsMessage, "there should be one client connected");

                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(15.0).TotalMilliseconds));

                connectedClientsMessage = CommonNUnitFunctions.OpenPetraServerAdminConsole("ConnectedClients");
                StringAssert.Contains("* no connected Clients *",
                    connectedClientsMessage,
                    "there should not be any clients connected anymore after the client got disconnected due to timeout");
            }
            finally
            {
                // clean up
                CommonNUnitFunctions.nant("stopServer", false);
            }
        }
    }
}