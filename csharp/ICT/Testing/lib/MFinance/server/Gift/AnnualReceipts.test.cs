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
using System.Text;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Common.Data;
using Ict.Petra.Server.App.Core;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the generation of the annual gift receipts on the server
    [TestFixture]
    public class TGiftAnnualReceiptTest
    {
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        private bool ImportAndPostGiftBatch(out TVerificationResultCollection VerificationResult)
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");

            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);

            int BatchNumber = importer.GetLastGiftBatchNumber();

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// print annual receipt
        /// </summary>
        [Test]
        public void TestAnnualReceipt()
        {
            CommonNUnitFunctions.ResetDatabase();

            // import a test gift batch
            TVerificationResultCollection VerificationResult;

            if (!ImportAndPostGiftBatch(out VerificationResult))
            {
                Assert.Fail("ImportAndPostGiftBatch failed: " + VerificationResult.BuildVerificationResultString());
            }

            // TODO test reversed gifts

            string formletterTemplateFile = TAppSettingsManager.GetValue("ReceiptTemplate.file",
                "../../csharp/ICT/Testing/lib/MFinance/SampleData/AnnualReceiptTemplate.html");
            Encoding encodingOfHTMLfile = TTextFile.GetFileEncoding(formletterTemplateFile);
            StreamReader sr = new StreamReader(formletterTemplateFile, encodingOfHTMLfile, false);
            string FileContent = sr.ReadToEnd();

            sr.Close();

            string formletterExpectedFile = TAppSettingsManager.GetValue("ReceiptExptected.file",
                "../../csharp/ICT/Testing/lib/MFinance/SampleData/AnnualReceiptExpected.html");

            Catalog.Init("de-DE", "de-DE");
            sr = new StreamReader(formletterExpectedFile, encodingOfHTMLfile, false);
            string ExpectedFormletterContent = sr.ReadToEnd().
                                               Replace("#TODAY#", DateTime.Now.ToString("d. MMMM yyyy")).
                                               Replace("#THISYEAR#", DateTime.Today.Year.ToString());
            sr.Close();

            StreamWriter sw = new StreamWriter(formletterExpectedFile + ".updated", false, encodingOfHTMLfile);
            sw.WriteLine(ExpectedFormletterContent);
            sw.Close();

            TLanguageCulture.SetLanguageAndCulture("de-DE", "de-DE");

            string receipts =
                TReceiptingWebConnector.CreateAnnualGiftReceipts(FLedgerNumber,
                    new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, 12, 31), FileContent);
            sw = new StreamWriter(formletterExpectedFile + ".new", false, encodingOfHTMLfile);
            sw.WriteLine(receipts);
            sw.WriteLine();
            sw.Close();

            Assert.IsTrue(
                TTextFile.SameContent(formletterExpectedFile + ".updated", formletterExpectedFile + ".new"),
                "receipt was not printed as expected, check " + formletterExpectedFile + ".new");

            File.Delete(formletterExpectedFile + ".new");
            File.Delete(formletterExpectedFile + ".updated");
        }
    }
}