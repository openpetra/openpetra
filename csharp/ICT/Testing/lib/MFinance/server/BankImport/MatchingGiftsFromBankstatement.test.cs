//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
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
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.ClientPlugins.BankStatementImport.BankImportFromCSV;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.ImportExport.WebConnectors;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Testing.NUnitTools;

namespace Tests.MFinance.Server.BankImport
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TBankImportGiftMatching
    {
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
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
        /// This will test for a situation where people have several gifts but no split gifts
        /// </summary>
        [Test]
        public void TestMultipleGifts()
        {
            // import the test gift batch, and post it
            TGiftImporting importer = new TGiftImporting();

            string dirTestData = "../../csharp/ICT/Testing/lib/MFinance/server/BankImport/TestData/";
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

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Failed to import gift batch: " + VerificationResult.BuildVerificationResultString());

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            // import the test csv file, will already do the training
            TBankStatementImport import = new TBankStatementImport();

            testFile = dirTestData + "BankStatement.csv";
            sr = new StreamReader(testFile);
            FileContent = sr.ReadToEnd();
            sr.Close();
            FileContent = FileContent.Replace("30.09.2010", "30.09." + DateTime.Now.Year.ToString("0000"));

            Int32 StatementKey;
            BankImportTDS BankImportDS = import.ImportBankStatementNonInteractive(
                FLedgerNumber,
                "6200",
                ";",
                "DMY",
                TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN,
                "unused,DateEffective,Description,Amount,Currency",
                "BankStatementSeptember.csv",
                FileContent);
            Assert.AreNotEqual(null, BankImportDS, "valid bank import dataset september");

            Assert.AreEqual(TSubmitChangesResult.scrOK, TBankImportWebConnector.StoreNewBankStatement(
                    BankImportDS,
                    out StatementKey,
                    out VerificationResult), "save statement September");

            // revert the gift batch, so that the training does not get confused if the test is run again;
            // the training needs only one gift batch for that date
            Hashtable requestParams = new Hashtable();
            requestParams.Add("Function", "ReverseGiftBatch");
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("BatchNumber", BatchNumber);
            requestParams.Add("GiftDetailNumber", -1);
            requestParams.Add("GiftNumber", -1);
            requestParams.Add("NewBatchSelected", false);
            requestParams.Add("GlEffectiveDate", new DateTime(DateTime.Now.Year, 09, 30));

            Assert.AreEqual(true, TAdjustmentWebConnector.GiftRevertAdjust(requestParams, out VerificationResult), "reversing the gift batch");

            if (VerificationResult.HasCriticalErrors)
            {
                Assert.Fail("Gift Batch was not reverted: " + VerificationResult.BuildVerificationResultString());
            }

            // duplicate the bank import file, for the next month
            FileContent = FileContent.Replace("30.09." + DateTime.Now.Year.ToString("0000"),
                "30.10." + DateTime.Now.Year.ToString("0000"));

            BankImportDS = import.ImportBankStatementNonInteractive(
                FLedgerNumber,
                "6200",
                ";",
                "DMY",
                TDlgSelectCSVSeparator.NUMBERFORMAT_EUROPEAN,
                "unused,DateEffective,Description,Amount,Currency",
                "BankStatementOcotober.csv",
                FileContent);
            Assert.AreNotEqual(null, BankImportDS, "valid bank import dataset october");

            Assert.AreEqual(TSubmitChangesResult.scrOK, TBankImportWebConnector.StoreNewBankStatement(
                    BankImportDS,
                    out StatementKey,
                    out VerificationResult), "save statement October");

            // create gift batch from imported statement
            BankImportTDS FMainDS =
                TBankImportWebConnector.GetBankStatementTransactionsAndMatches(
                    StatementKey, FLedgerNumber);

            Int32 GiftBatchNumber = TBankImportWebConnector.CreateGiftBatch(FMainDS,
                FLedgerNumber,
                StatementKey,
                -1,
                out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                Assert.Fail("cannot create gift batch from bank statement: " + VerificationResult.BuildVerificationResultString());
            }

            // check if the gift batch is correct
            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.LoadGiftBatchData(FLedgerNumber, GiftBatchNumber);

            // since we are not able to match the split gift, only 2 gifts should be matched.
            // TODO: allow 2 gifts to be merged in OpenPetra, even when they come separate on the bank statement.
            //           then 4 gifts could be matched.
            Assert.AreEqual(2, GiftDS.AGift.Rows.Count, "expected two matched gifts");
        }
    }
}