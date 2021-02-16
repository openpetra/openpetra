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

            // there should be 4 partners imported
            Assert.AreEqual(4, MainDS.PPartner.Rows.Count);
            // there should be 3 families imported
            Assert.AreEqual(3, MainDS.PFamily.Rows.Count);
            // there should be 1 organisation imported
            Assert.AreEqual(1, MainDS.POrganisation.Rows.Count);
        }

        /// <summary>
        /// Test importing a CSV file with partners, with unknown column
        /// </summary>
        [Test]
        public void TestImportCSVUnknownColumn()
        {
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../csharp/ICT/Testing/lib/MPartner/SampleData/samplePartnerImport_unknown_column.csv"))
            {
                doc = sr.ReadToEnd();
            }

            TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "DMY", ";", out VerificationResult);

            Assert.IsNotNull(VerificationResult, "Expected to get errors");
            Assert.AreEqual(1, VerificationResult.Count, "there should be one error");
            Assert.AreEqual("Unknown Column(s): Test2", VerificationResult[0].ResultText, "VerificationResult message");
        }

        /// <summary>
        /// Test importing a CSV file with partners, without Name
        /// </summary>
        [Test]
        public void TestImportCSVWithoutName()
        {
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../csharp/ICT/Testing/lib/MPartner/SampleData/samplePartnerImport_invalid.csv"))
            {
                doc = sr.ReadToEnd();
            }

            TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "DMY", ";", out VerificationResult);

            Assert.IsNotNull(VerificationResult, "Expected to get errors");
            Assert.AreEqual(2, VerificationResult.Count, "there should be two errors");
            Assert.AreEqual("Missing Firstname or family name in line 2",
                VerificationResult[0].ResultText, "VerificationResult message");
            Assert.AreEqual("We need either a valid address, phone number, email address or IBAN in line 4",
                VerificationResult[1].ResultText, "VerificationResult message");
        }

        /// <summary>
        /// Test importing a CSV file with partners using dates with dmy format
        /// </summary>
        [Test]
        public void TestImportCSV_Dates_DMY()
        {
            TVerificationResultCollection VerificationResult = null;
            string doc = String.Empty;

            using (StreamReader sr = new StreamReader("../../csharp/ICT/Testing/lib/MPartner/SampleData/samplePartnerImport_dates_dmy.csv"))
            {
                doc = sr.ReadToEnd();
            }

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "dmy", ";", out VerificationResult);

            if (VerificationResult != null)
            {
                if (VerificationResult.HasCriticalErrors)
                    Assert.AreEqual(String.Empty, VerificationResult.BuildVerificationResultString(), "there was an error importing the csv file");
            }

            // there should be 2 partners imported (2 x family)
            Assert.AreEqual(2, MainDS.PPartner.Rows.Count, "Wrong number of partners");
            Assert.AreEqual(2, MainDS.PFamily.Rows.Count, "Wrong number of families");

            Assert.AreEqual(new DateTime(1979,8,19), MainDS.PFamily[0].DateOfBirth, "date of birth is wrong!");

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

            using (StreamReader sr = new StreamReader("../../csharp/ICT/Testing/lib/MPartner/SampleData/samplePartnerImport_dates_mdy.csv"))
            {
                doc = sr.ReadToEnd();
            }

            PartnerImportExportTDS MainDS = TImportExportWebConnector.ImportFromCSVFileReturnDataSet(doc, "mdy", ";", out VerificationResult);

            if (VerificationResult != null)
            {
                if (VerificationResult.HasCriticalErrors)
                    Assert.AreEqual(String.Empty, VerificationResult.BuildVerificationResultString(), "there was an error importing the csv file");
            }

            // there should be 2 partners imported (2 x family)
            Assert.AreEqual(2, MainDS.PPartner.Rows.Count);
            Assert.AreEqual(2, MainDS.PFamily.Rows.Count);

            Assert.AreEqual(new DateTime(1979,8,19), MainDS.PFamily[0].DateOfBirth, "date of birth is wrong!");

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
