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
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.BankImport.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.BankImport.WebConnectors;
using Ict.Petra.Server.MFinance.BankImport.Logic;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Testing.NUnitTools;

namespace Ict.Testing.Petra.Server.MFinance.BankImport
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TBankImportGiftMatching
    {
        Int32 FLedgerNumber = -1;
        string dirTestData = "../../csharp/ICT/Testing/lib/MFinance/server/BankImport/";

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
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
        /// This will test for a situation where people have several gifts but no split gifts
        /// </summary>
        [Test]
        public void TestMultipleGifts()
        {
            // import the test gift batch, and post it
            TGiftImporting importer = new TGiftImporting();

            string testFile = dirTestData + "GiftBatch.csv";
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            sr.Close();
            FileContent = FileContent.Replace("2010-09-30", DateTime.Now.Year.ToString("0000") + "-09-30");

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);
            parameters.Add("DatesMayBeIntegers", false);

            TVerificationResultCollection VerificationResult;
            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber;
            bool refreshRequired;

            if (!importer.ImportGiftBatches(parameters, FileContent, out NeedRecipientLedgerNumber, out refreshRequired, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not imported: " + VerificationResult.BuildVerificationResultString());
            }

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Failed to import gift batch: " + VerificationResult.BuildVerificationResultString());

            Int32 generatedGlBatchNumber;

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out generatedGlBatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            // import the test csv file, will already do the training
            testFile = dirTestData + "BankStatement.csv";
            sr = new StreamReader(testFile);
            FileContent = sr.ReadToEnd();
            sr.Close();
            FileContent = FileContent.Replace("30.09.2010", "30.09." + DateTime.Now.Year.ToString("0000"));

            Int32 StatementKey;
            BankImportTDS BankImportDS = TBankStatementImportCSV.ImportBankStatementHelper(
                FLedgerNumber,
                "6200",
                ";",
                "DMY",
                "European",
                "EUR",
                "unused,DateEffective,Description,Amount,Currency",
                "",
                "BankStatementSeptember.csv",
                FileContent);
            Assert.AreNotEqual(null, BankImportDS, "valid bank import dataset september");

            Assert.AreEqual(TSubmitChangesResult.scrOK, TBankStatementImport.StoreNewBankStatement(
                    BankImportDS,
                    out StatementKey), "save statement September");

            // revert the gift batch, so that the training does not get confused if the test is run again;
            // the training needs only one gift batch for that date
            int AdjustmentBatchNumber;
            Assert.AreEqual(true, TAdjustmentWebConnector.GiftRevertAdjust(
                FLedgerNumber,
                BatchNumber,
                -1,
                -1,
                false,
                -1,
                new DateTime(DateTime.Now.Year, 09, 30),
                GiftAdjustmentFunctionEnum.ReverseGiftBatch,
                true,
                0.0m,
                out AdjustmentBatchNumber), "reversing the gift batch");

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, AdjustmentBatchNumber, out generatedGlBatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Reverse Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            // duplicate the bank import file, for the next month
            FileContent = FileContent.Replace("30.09." + DateTime.Now.Year.ToString("0000"),
                "30.10." + DateTime.Now.Year.ToString("0000"));

            BankImportDS = TBankStatementImportCSV.ImportBankStatementHelper(
                FLedgerNumber,
                "6200",
                ";",
                "DMY",
                "European",
                "EUR",
                "unused,DateEffective,Description,Amount,Currency",
                "",
                "BankStatementOctober.csv",
                FileContent);
            Assert.AreNotEqual(null, BankImportDS, "valid bank import dataset october");

            Assert.AreEqual(TSubmitChangesResult.scrOK, TBankStatementImport.StoreNewBankStatement(
                    BankImportDS,
                    out StatementKey), "save statement October");

            // create gift batch from imported statement
            Int32 GiftBatchNumber;
            TBankImportWebConnector.CreateGiftBatch(
                FLedgerNumber,
                StatementKey,
                out VerificationResult,
                out GiftBatchNumber);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "cannot create gift batch from bank statement:");

            // check if the gift batch is correct
            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData(FLedgerNumber, GiftBatchNumber);

            // since we are not able to match the split gifts, only 1 donation should be matched.
            // TODO: allow 2 gifts to be merged in OpenPetra, even when they come separate on the bank statement.
            //           then 4 gifts could be matched.
            Assert.AreEqual(1, GiftDS.AGift.Rows.Count, "expected two matched gifts");
        }


        /// <summary>
        /// Test the import of CAMT file
        /// </summary>
        [Test]
        public void TestImportCAMT()
        {
            // import the test camt file, will already do the training
            string testFile = dirTestData + "camt_testfile.53.xml";
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();
            sr.Close();
            FileContent = FileContent.Replace("2015-09-01", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJ-MM-TT", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJMMTT", DateTime.Now.Year.ToString("0000") + "0901");

            Int32 StatementKey;
            TVerificationResultCollection VerificationResult;
            bool success = TBankStatementImportCAMT.ImportFromFile(
                FLedgerNumber,
                "6200",
                "camt_testfileSeptember.xml",
                FileContent,
                false,
                out StatementKey,
                out VerificationResult);
            Assert.AreEqual(true, success, "valid bank import dataset september");
        }

        /// <summary>
        /// Import a sample CAMT.53 file
        /// </summary>
        [Test]
        public void ImportCAMTFile53()
        {
            TCAMTParser p = new TCAMTParser();

            string testfile = dirTestData + "camt_testfile.53.xml";
            TVerificationResultCollection VerificationResult;
            string FileContent;
            using (StreamReader sr = new StreamReader(testfile))
            {
                FileContent = sr.ReadToEnd();
            }

            FileContent = FileContent.Replace("2015-09-01", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJ-MM-TT", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJMMTT", DateTime.Now.Year.ToString("0000") + "0901");

            p.ProcessFileContent(FileContent, false, out VerificationResult);

            Assert.AreEqual(1, p.statements.Count, "there should be one statement");

            foreach (TStatement stmt in p.statements)
            {
                Assert.AreEqual(2, stmt.transactions.Count, "There should be two transactions");

                foreach (TTransaction tr in stmt.transactions)
                {
                    Assert.AreEqual(new DateTime(DateTime.Now.Year, 9, 1), tr.valueDate, "The date should match");
                }
            }
        }

        /// <summary>
        /// Import a sample CAMT.52 file
        /// </summary>
        [Test]
        public void ImportCAMTFile52()
        {
            TCAMTParser p = new TCAMTParser();

            string testfile = dirTestData + "camt_testfile.52.xml";
            TVerificationResultCollection VerificationResult;
            string FileContent;
            using (StreamReader sr = new StreamReader(testfile))
            {
                FileContent = sr.ReadToEnd();
            }

            FileContent = FileContent.Replace("2015-09-01", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJ-MM-TT", DateTime.Now.Year.ToString("0000") + "-09-01");
            FileContent = FileContent.Replace("JJJJMMTT", DateTime.Now.Year.ToString("0000") + "0901");

            p.ProcessFileContent(FileContent, false, out VerificationResult);

            Assert.AreEqual(1, p.statements.Count, "there should be one statement");

            foreach (TStatement stmt in p.statements)
            {
                Assert.AreEqual(2, stmt.transactions.Count, "There should be two transactions");

                foreach (TTransaction tr in stmt.transactions)
                {
                    Assert.AreEqual(new DateTime(DateTime.Now.Year, 9, 1), tr.valueDate, "The date should match");
                }
            }
        }

        /// <summary>
        /// Import a sample mt940 file
        /// </summary>
        [Test]
        public void ImportMT940File()
        {
            string testfile = dirTestData + "mt940test.sta";
            string FileContent;
            using (StreamReader sr = new StreamReader(testfile))
            {
                FileContent = sr.ReadToEnd();
            }

            TSwiftParser p = new TSwiftParser();
            p.ProcessFileContent(FileContent);

            Assert.AreEqual(2, p.statements.Count, "there should be two statements");
        }
    }
}
