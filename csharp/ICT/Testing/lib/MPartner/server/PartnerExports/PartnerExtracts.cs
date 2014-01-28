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
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.ImportExport;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Testing.NUnitTools;

namespace Tests.MPartner.Server.PartnerExports
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerExtractsTest
    {
        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("../../log/TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
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
        /// Test importing an extract with FAMILY partners
        /// </summary>
        [Test]
        public void TestImportFamily()
        {
            string testFile = TAppSettingsManager.GetValue("ExtractTest.file", "../../csharp/ICT/Testing/lib/MPartner/SampleData/sampleExtract.ext");
            string SelectedEventCode = TAppSettingsManager.GetValue("ImportPartnerForEventCode", String.Empty);
            StreamReader reader = new StreamReader(testFile, System.Text.Encoding.GetEncoding(1252));

            string[] lines = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n").Split(new char[] { '\n' });
            reader.Close();

            TVerificationResultCollection VerificationResult;
            TPartnerFileImport importer = new TPartnerFileImport();
            PartnerImportExportTDS MainDS = importer.ImportAllData(lines, SelectedEventCode, false, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult);

            foreach (PPartnerRow PartnerRow in MainDS.PPartner.Rows)
            {
                TLogging.Log(PartnerRow.PartnerKey.ToString() + " " + PartnerRow.PartnerShortName);
            }

            // TODO: check if the partners have been imported previously already
            foreach (PPartnerRow PartnerRow in MainDS.PPartner.Rows)
            {
                TLogging.Log(PartnerRow.PartnerKey.ToString() + " " + PartnerRow.PartnerShortName);
            }

            try
            {
                PartnerImportExportTDSAccess.SubmitChanges(MainDS);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                Assert.Fail("See log messages");
            }

            Assert.AreEqual(2, MainDS.PPartner.Rows.Count);
        }
    }
}