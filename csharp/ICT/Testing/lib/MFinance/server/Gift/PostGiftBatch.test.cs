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
using System.IO;
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Common.Data;
using Ict.Testing.NUnitTools;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TGiftBatchTest
    {
        Int32 FLedgerNumber = -1;

        const string MainFeesPayableCode = "GIF2";
        const string MainFeesReceivableCode = "HO_ADMIN2";

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
        /// This will import a test gift batch, and post it.
        /// </summary>
        [Test]
        public void TestPostGiftBatch()
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

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and return a valid batch number");

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted");
            }
        }

        /// <summary>
        /// This will test the admin fee processer
        /// </summary>
        [Test]
        public void TestProcessAdminFees()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            TVerificationResultCollection VerficationResults = null;

            AFeesPayableRow template = new AFeesPayableTable().NewRowTyped(false);

            template.LedgerNumber = FLedgerNumber;
            template.FeeCode = MainFeesPayableCode;

            AFeesPayableTable FeesPayableTable = AFeesPayableAccess.LoadUsingTemplate(template, Transaction);

            AFeesReceivableRow template1 = new AFeesReceivableTable().NewRowTyped(false);

            template1.LedgerNumber = FLedgerNumber;
            template1.FeeCode = MainFeesReceivableCode;

            AFeesReceivableTable FeesReceivableTable = AFeesReceivableAccess.LoadUsingTemplate(template1, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            if (FeesPayableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feespayable-data.sql", FLedgerNumber);
            }

            if (FeesReceivableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feesreceivable-data.sql", FLedgerNumber);
            }

            GiftBatchTDS MainDS = new GiftBatchTDS();

            Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            AFeesPayableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
            AFeesReceivableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            //TODO If this first one works, try different permatations for Assert.AreEqual
            // Test also for exception handling
            Assert.AreEqual(12m, TGiftTransactionWebConnector.CalculateAdminFee(MainDS,
                    FLedgerNumber,
                    MainFeesPayableCode,
                    100m,
                    out VerficationResults), "admin fee fixed 12% of 100 expect 12");
        }
    }
}