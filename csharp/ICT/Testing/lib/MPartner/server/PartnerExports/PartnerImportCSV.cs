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
using System.Xml;
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
using Ict.Petra.Server.MPartner.ImportExport.WebConnectors;
using Ict.Testing.NUnitTools;

namespace Tests.MPartner.Server.PartnerExports
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerImportCSVTest
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
        /// Test importing a CSV file with partners
        /// </summary>
        [Test]
        public void TestImportCSV()
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml("../../demodata/partners/samplePartnerImport.csv", ";");
            TVerificationResultCollection VerificationResult = null;

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFile(TXMLParser.XmlToString(doc), out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.IsFalse(VerificationResult.HasCriticalErrors, "there was an error importing the csv file");
            }

            // there should be 2 partners imported
            Assert.AreEqual(2, MainDS.PPartner.Rows.Count);
        }

        /// <summary>
        /// Test importing a CSV file with partners, the lines are split for inheritance
        /// </summary>
        [Test]
        [Ignore("this does not work at all")]
        public void TestImportCSV2()
        {
            XmlDocument doc = TCsv2Xml.ParseCSV2Xml("../../demodata/partners/samplefilepartnerimport2.csv", ",");
            TVerificationResultCollection VerificationResult = null;

            Console.WriteLine(TXMLParser.XmlToString(doc));
            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFile(TXMLParser.XmlToString(doc), out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.IsFalse(VerificationResult.HasCriticalErrors, "there was an error importing the csv file");
            }

            foreach (PFamilyRow f in MainDS.PFamily.Rows)
            {
                Console.WriteLine("Family name : " + f.FamilyName);
            }

            // we are currently ignoring UNIT and ORGANISATION partners, only importing the 7 FAMILY partners.
            // due to the strange format of the file, each row is imported as a separate partner, ending up with 27 invalid partners
            Assert.AreEqual(7, MainDS.PPartner.Rows.Count);
        }
    }
}