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
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Common.Data;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TGiftBatchTest
    {
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("TestServer.log");
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

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber.ToString());
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "European");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and return a valid batch number");

            if (TTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
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

            GiftBatchTDS MainDS = new GiftBatchTDS();

            AFeesPayableAccess.LoadViaALedger(FLedgerNumber, Transaction);
            AFeesReceivableAccess.LoadViaALedger(FLedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();

            Assert.AreEqual(0.12m, TTransactionWebConnector.CalculateAdminFee(MainDS, FLedgerNumber, "ICT", 12.21m), "expect 1%");
        }
    }
}