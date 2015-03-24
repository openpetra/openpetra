//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
    ///  This will test how a client connects, what happens if the password is wrong, what if the user gets retired, etc
    [TestFixture]
    public class TTestLogin
    {
        /// init the test
        [SetUp]
        public void Init()
        {
            Catalog.Init();
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestServer.config");
            CommonNUnitFunctions.InitRootPath();
            CommonNUnitFunctions.StartOpenPetraServer();
        }

        /// <summary>
        /// clean up
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // clean up
            CommonNUnitFunctions.nant("stopServer", false);
        }

        /// <summary>
        /// test how authentication fails and succeeds
        /// </summary>
        [Test]
        public void TestAuthentication()
        {
            // read original config file for client
            StreamReader sr = new StreamReader("../../etc/TestClient.config");
            string config = sr.ReadToEnd();

            sr.Close();
            string tempConfigFile = Path.GetTempFileName();

            // Testing successful login
            Assert.AreEqual(eLoginEnum.eLoginSucceeded, TPetraConnector.Connect("../../etc/TestClient.config", false), "should connect fine");
            TPetraConnector.Disconnect();

            // pass invalid password
            StreamWriter sw = new StreamWriter(tempConfigFile);
            sw.Write(config.Replace("key=\"AutoLoginPasswd\" value=\"demo\"",
                    "key=\"AutoLoginPasswd\" value=\"demoFalse\""));
            sw.Close();

            Assert.AreEqual(eLoginEnum.eLoginAuthenticationFailed, TPetraConnector.Connect(tempConfigFile, false), "should fail on wrong password");

            // pass invalid user
            sw = new StreamWriter(tempConfigFile);
            sw.Write(config.Replace("key=\"AutoLogin\" value=\"demo\"",
                    "key=\"AutoLogin\" value=\"demoNonExisting\""));
            sw.Close();
            Assert.AreEqual(eLoginEnum.eLoginAuthenticationFailed, TPetraConnector.Connect(tempConfigFile, false), "should fail on wrong user");

            // clean up
            File.Delete(tempConfigFile);
        }

        /// <summary>
        /// version of server binaries must match the version of the client binaries
        /// </summary>
        [Test]
        public void TestVersionMismatch()
        {
            StreamReader sr = new StreamReader("../../delivery/bin/version.txt");
            string strServerVersion = sr.ReadLine();
            TFileVersionInfo ServerVersion = new TFileVersionInfo(strServerVersion);

            sr.Close();

            Assert.AreEqual(eLoginEnum.eLoginSucceeded, TPetraConnector.Connect(
                    "../../etc/TestClient.config", false), "connecting with the same version number");
            TPetraConnector.Disconnect();

            TFileVersionInfo.FManualApplicationVersion =
                new TFileVersionInfo(new Version(ServerVersion.FileMajorPart, ServerVersion.FileMinorPart, ServerVersion.FileBuildPart,
                        ServerVersion.FilePrivatePart + 1));
            Assert.AreEqual(eLoginEnum.eLoginVersionMismatch, TPetraConnector.Connect(
                    "../../etc/TestClient.config", false), "client is too new, only change in private part");

            int FileMajorPart = ServerVersion.FileMajorPart;
            int FileMinorPart = ServerVersion.FileMinorPart;
            int FileBuildPart = ServerVersion.FileBuildPart - 1;
            int FilePrivatePart = ServerVersion.FilePrivatePart;

            if (FileBuildPart < 0)
            {
                FileMinorPart -= 1;
                FileBuildPart = 0;
            }
            else if (FileMinorPart < 0)
            {
                FileMajorPart -= 1;
                FileMinorPart = 0;
            }

            TFileVersionInfo.FManualApplicationVersion =
                new TFileVersionInfo(new Version(FileMajorPart, FileMinorPart, FileBuildPart,
                        FilePrivatePart));
            Assert.AreEqual(eLoginEnum.eLoginVersionMismatch, TPetraConnector.Connect("../../etc/TestClient.config", false), "client is too old");

            TFileVersionInfo.FManualApplicationVersion =
                new TFileVersionInfo(new Version(ServerVersion.FileMajorPart, ServerVersion.FileMinorPart, ServerVersion.FileBuildPart + 1,
                        ServerVersion.FilePrivatePart));
            Assert.AreEqual(eLoginEnum.eLoginVersionMismatch, TPetraConnector.Connect("../../etc/TestClient.config", false), "client is too new");

            TFileVersionInfo.FManualApplicationVersion = new TFileVersionInfo(new Version());
            Assert.AreEqual(eLoginEnum.eLoginVersionMismatch, TPetraConnector.Connect(
                    "../../etc/TestClient.config", false), "version 0.0.0.0 should not be accepted");

            // reset values
            TFileVersionInfo.FManualApplicationVersion = ServerVersion;
        }

        /// <summary>
        /// test how a user gets retired after too many failed login attempts
        /// </summary>
        [Test]
        public void TestRetiredUser()
        {
            // TODORemoting create dummy user
            // Test: first login successful
            // several login failures
            // login returns eLoginEnum.eLoginUserIsRetired
        }
    }
}