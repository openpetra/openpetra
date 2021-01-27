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
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
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
        [OneTimeSetUp]
        public void Init()
        {
            new TLogging("../../log/TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [OneTimeTearDown]
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
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../demodata/partners/samplePartnerImport.csv"))
            {
                doc = sr.ReadToEnd();
            }

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "DMY", ";", out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.IsFalse(VerificationResult.HasCriticalErrors, "there was an error importing the csv file");
            }

            // there should be 2 partners imported
            Assert.AreEqual(2, MainDS.PPartner.Rows.Count);
        }

        /// <summary>
        /// Test importing a CSV file with partners using dates with dmy format
        /// </summary>
        [Test]
        public void TestImportCSV_Dates_DMY()
        {
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../demodata/partners/samplePartnerImport_dates_dmy.csv"))
            {
                doc = sr.ReadToEnd();
            }

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "dmy", ";", out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.IsFalse(VerificationResult.HasCriticalErrors, "there was an error importing the csv file");
            }

            // there should be 4 partners imported (2 x family + 2 x person)
            Assert.AreEqual(4, MainDS.PPartner.Rows.Count, "Wrong number of partners");
            Assert.AreEqual(2, MainDS.PPerson.Rows.Count, "Wrong number of persons");

            Assert.AreEqual(new DateTime(1979,8,19), MainDS.PPerson[0].DateOfBirth, "date of birth is wrong!");

            // Now try with the wrong date format
            VerificationResult = null;
            MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "mdy", ";", out VerificationResult);

            Assert.IsNotNull(VerificationResult, "Expected to get errors");
            int numErrors = 0;

            for (int i = 0; i < VerificationResult.Count; i++)
            {
                if (VerificationResult[i].ResultSeverity != TResultSeverity.Resv_Status)
                {
                    numErrors++;
                }
            }

            Assert.AreEqual(2, numErrors, "Wrong number of errors");
        }

        /// <summary>
        /// Test importing a CSV file with partners using dates with mdy format
        /// </summary>
        [Test]
        public void TestImportCSV_Dates_MDY()
        {
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../demodata/partners/samplePartnerImport_dates_mdy.csv"))
            {
                doc = sr.ReadToEnd();
            }

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "mdy", ";", out VerificationResult);

            if (VerificationResult != null)
            {
                Assert.IsFalse(VerificationResult.HasCriticalErrors, "there was an error importing the csv file");
            }

            // there should be 4 partners imported (2 x family + 2 x person)
            Assert.AreEqual(4, MainDS.PPartner.Rows.Count);
            Assert.AreEqual(2, MainDS.PPerson.Rows.Count);

            Assert.AreEqual(new DateTime(1979,8,19), MainDS.PPerson[0].DateOfBirth, "date of birth is wrong!");

            // Now try with the wrong date format
            VerificationResult = null;
            MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "dmy", ";", out VerificationResult);

            Assert.IsNotNull(VerificationResult, "Expected to get errors");
            int numErrors = 0;

            for (int i = 0; i < VerificationResult.Count; i++)
            {
                if (VerificationResult[i].ResultSeverity != TResultSeverity.Resv_Status)
                {
                    numErrors++;
                }
            }

            Assert.AreEqual(2, numErrors, "Wrong number of errors");
        }
    }
}
