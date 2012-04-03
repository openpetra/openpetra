//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.ICH.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Common.Data;

namespace Tests.MFinance.Server.ICH
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TStewardshipCalculationTest
    {
        Int32 FLedgerNumber = -1;

        const string MainFeesPayableCode = "ICT";
        const string MainFeesReceivableCode = "HO_ADMIN";

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
        public int ImportAndPostGiftBatch(DateTime AGiftDateEffective)
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            sr.Close();

            FileContent = FileContent.Replace("2010-09-30", AGiftDateEffective.ToString("yyyy-MM-dd"));

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult;

            importer.ImportGiftBatches(parameters, FileContent, out VerificationResult);

            Assert.IsFalse(
                VerificationResult.HasCriticalErrors, "error when importing gift batch: " + VerificationResult.BuildVerificationResultString());

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and return a valid batch number");

            if (!TTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted");
            }

            return BatchNumber;
        }

//        /// <summary>
//        /// Test that the valid posting period code works
//        /// </summary>
//        [Test]
//        public void TestIsValidPostingPeriod()
//        {
//              int DateEffectivePeriodNumber;
//              int DateEffectiveYearNumber;
//
//              bool NewTransaction = false;
//
//              TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
//
//			Assert.IsTrue(TFinancialYear.IsValidPostingPeriod(FLedgerNumber, Convert.ToDateTime("15-Sep-2011"), out DateEffectivePeriodNumber, out DateEffectiveYearNumber, Transaction),"Period is not valid");
//
//              if (NewTransaction)
//              {
//                      DBAccess.GDBAccessObj.RollbackTransaction();
//              }
//        }

        /// <summary>
        /// this function will import admin fees if there are no admin fees in the database yet
        /// </summary>
        private void ImportAdminFees()
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            AFeesPayableRow template = new AFeesPayableTable().NewRowTyped(false);

            template.LedgerNumber = FLedgerNumber;
            template.FeeCode = MainFeesPayableCode;

            AFeesPayableTable FeesPayableTable = AFeesPayableAccess.LoadUsingTemplate(template, Transaction);

            if (FeesPayableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feespayable-data.sql");
            }

            AFeesReceivableRow template1 = new AFeesReceivableTable().NewRowTyped(false);

            template.LedgerNumber = FLedgerNumber;
            template.FeeCode = MainFeesReceivableCode;

            AFeesReceivableTable FeesReceivableTable = AFeesReceivableAccess.LoadUsingTemplate(template1, Transaction);

            if (FeesReceivableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feesreceivable-data.sql");
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        /// <summary>
        /// This will test the admin fee processer
        /// </summary>
        [Test]
        public void TestProcessAdminFees()
        {
            ImportAdminFees();

            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            TVerificationResultCollection VerificationResults = null;

            GiftBatchTDS MainDS = new GiftBatchTDS();

            AFeesPayableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
            AFeesReceivableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            //TODO If this first one works, try different permatations for Assert.AreEqual
            // Test also for exception handling
            Assert.AreEqual(-12m, TTransactionWebConnector.CalculateAdminFee(MainDS,
                    FLedgerNumber,
                    "GIF",
                    -200m,
                    out VerificationResults), "expect 12");
        }

        /// <summary>
        /// this test loads the sample partners, imports a gift batch, and posts it, and then runs a stewardship calculation
        /// </summary>
        [Test]
        public void TestPerformStewardshipCalculation()
        {
            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            Int32 PeriodNumber = 5;

            // run possibly empty stewardship calculation, to process all gifts that do not belong to this test
            TStewardshipCalculationWebConnector.PerformStewardshipCalculation(FLedgerNumber,
                PeriodNumber, out VerificationResults);
            Assert.IsFalse(VerificationResults.HasCriticalErrors, "Performing initial Stewardship Calculation Failed! " +
                VerificationResults.BuildVerificationResultString());

            // import new gift batch. use proper period and date effective
            DateTime PeriodStartDate, PeriodEndDate;
            TFinancialYear.GetStartAndEndDateOfPeriod(FLedgerNumber, PeriodNumber, out PeriodStartDate, out PeriodEndDate, null);
            int GiftBatchNumber = ImportAndPostGiftBatch(PeriodStartDate);

            // make sure we have some admin fees
            ImportAdminFees();

            TStewardshipCalculationWebConnector.PerformStewardshipCalculation(FLedgerNumber,
                PeriodNumber, out VerificationResults);
            Assert.IsFalse(VerificationResults.HasCriticalErrors, "Performing Stewardship Calculation Failed!" +
                VerificationResults.BuildVerificationResultString());

            // analyse the admin fee batch
            // TODO check transactions

            // analyse the stewardship batch
            // TODO check transactions
        }
    }
}