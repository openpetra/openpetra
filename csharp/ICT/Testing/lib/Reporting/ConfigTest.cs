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
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ict.Common;

namespace Tests.Reporting
{
    /// This is a test for the reports.
    /// It runs as a NUnit test, and the login is defined in the config file.
    [TestFixture]
    public class TConfigTest : System.Object
    {
        /// <summary>
        /// ...
        /// </summary>
        [SetUp]
        public void Init()
        {
            new TAppSettingsManager("../../etc/TestClient.config");

            new TLogging("../../log/test.log");
        }

        /// <summary>
        /// ...
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// ...
        /// </summary>
        [Test]
        public void CheckConfigFile()
        {
            // This test is about testing if the correct configuration file is loaded, for NUnit tests
            // But it seems, the connection with the server based on the configuration file works anyways, even if
            //  the System.Configuration.ConfigurationManager.AppSettings does not use the custom config file

            // see also http://nunit.com/blogs/?p=9, How NUnit Finds Config Files
            TLogging.Log(TAppSettingsManager.ConfigFileName);
            Assert.AreEqual("demo", TAppSettingsManager.GetValue("AutoLogin"), "retrieving value from TAppSettingsManager");

            // does not seem to help:
            // System.Configuration.ConfigurationFileMap fileMap = new ConfigurationFileMap(TAppSettingsManager.ConfigFileName);
            // System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
            // Assert.AreEqual("demo", System.Configuration.ConfigurationManager.AppSettings["AutoLogin"], "retrieving value from .net ConfigurationManager");
        }
    }
}