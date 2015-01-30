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
using System.Collections.Generic;
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
    /// This will test the generation of a gift receipt for a single gift on the server
    [TestFixture]
    public class TGiftSingleGiftReceiptTest
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

        /// <summary>
        /// print single gift receipt
        /// </summary>
        //[Test]
        public void TestSingleReceipt()
        {
            CommonNUnitFunctions.ResetDatabase();

            // import a test gift batch
            TVerificationResultCollection VerificationResult;

            if (!TGiftAnnualReceiptTest.ImportAndPostGiftBatch(FLedgerNumber, out VerificationResult))
            {
                Assert.Fail("ImportAndPostGiftBatch failed: " + VerificationResult.BuildVerificationResultString());
            }

            string formletterTemplateFile = TAppSettingsManager.GetValue("ReceiptTemplate.file",
                "../../csharp/ICT/Testing/lib/MFinance/SampleData/SingleGiftReceiptTemplate.html");
            Encoding encodingOfHTMLfile = TTextFile.GetFileEncoding(formletterTemplateFile);
            StreamReader sr = new StreamReader(formletterTemplateFile, encodingOfHTMLfile, false);
            string FileContent = sr.ReadToEnd();

            sr.Close();

            string formletterExpectedFile = TAppSettingsManager.GetValue("ReceiptExptected.file",
                "../../csharp/ICT/Testing/lib/MFinance/SampleData/SingleGiftReceiptExpected.html");

            Catalog.Init("de-DE", "de-DE");

            TLanguageCulture.SetLanguageAndCulture("de-DE", "de-DE");

            string receipts =
                TReceiptingWebConnector.CreateAnnualGiftReceipts(FLedgerNumber,
                    new DateTime(DateTime.Today.Year, 1, 1), new DateTime(DateTime.Today.Year, 12, 31), FileContent);
            StreamWriter sw = new StreamWriter(formletterExpectedFile + ".new", false, encodingOfHTMLfile);
            sw.WriteLine(receipts);
            sw.WriteLine();
            sw.Close();

            SortedList <string, string>ToReplace = new SortedList <string, string>();
            ToReplace.Add("#TODAY#", DateTime.Now.ToString("d. MMMM yyyy"));
            ToReplace.Add("#THISYEAR#", DateTime.Today.Year.ToString());

            Assert.IsTrue(
                TTextFile.SameContent(formletterExpectedFile, formletterExpectedFile + ".new", true, ToReplace, true),
                "receipt was not printed as expected, check " + formletterExpectedFile + ".new");

            File.Delete(formletterExpectedFile + ".new");
        }
    }
}