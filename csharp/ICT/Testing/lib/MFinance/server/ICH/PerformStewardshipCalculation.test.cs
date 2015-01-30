//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.ICH;
using Ict.Petra.Server.MFinance.ICH.WebConnectors;
using Ict.Petra.Shared.MFinance;
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
        static Int32 FLedgerNumber = 43;

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
        public static int ImportAndPostGiftBatch(DateTime AGiftDateEffective)
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file",
                CommonNUnitFunctions.rootPath + "/csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");

            TLogging.Log(testFile);

            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            sr.Close();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());
            FileContent = FileContent.Replace("{thisyear}-01-01", AGiftDateEffective.ToString("yyyy-MM-dd"));

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult;
            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber;

            importer.ImportGiftBatches(parameters, FileContent, out NeedRecipientLedgerNumber, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "error when importing gift batch:");

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and return a valid batch number");

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                string VerifResStr;

                if (VerificationResult != null)
                {
                    VerifResStr = ": " + VerificationResult.BuildVerificationResultString();
                }
                else
                {
                    VerifResStr = String.Empty;
                }

                Assert.Fail("Gift Batch was not posted" + VerifResStr);
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
            AFeesPayableTable FeesPayableTable = null;
            AFeesReceivableTable FeesReceivableTable = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    AFeesPayableRow template = new AFeesPayableTable().NewRowTyped(false);
                    template.LedgerNumber = FLedgerNumber;
                    template.FeeCode = MainFeesPayableCode;

                    FeesPayableTable = AFeesPayableAccess.LoadUsingTemplate(template, Transaction);

                    AFeesReceivableRow template1 = new AFeesReceivableTable().NewRowTyped(false);

                    template1.LedgerNumber = FLedgerNumber;
                    template1.FeeCode = MainFeesReceivableCode;

                    FeesReceivableTable = AFeesReceivableAccess.LoadUsingTemplate(template1, Transaction);
                });

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
        }

        /// <summary>
        /// This will test the admin fee processer
        /// </summary>
        [Test]
        public void TestProcessAdminFees()
        {
            ImportAdminFees();

            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    AFeesPayableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
                    AFeesReceivableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
                });

            TVerificationResultCollection VerificationResults = null;

            //TODO If this first one works, try different permatations for Assert.AreEqual
            // Test also for exception handling
            Assert.AreEqual(2m, TGiftTransactionWebConnector.CalculateAdminFee(MainDS,
                    FLedgerNumber,
                    "GIF",
                    200m,
                    out VerificationResults), "expect 1% of 200");
        }

        /// <summary>
        /// this test loads the sample partners, imports a gift batch, and posts it, and then runs a stewardship calculation
        /// </summary>
        [Test]
        public void TestPerformStewardshipCalculation()
        {
            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            Int32 PeriodNumber = 5;
            const string CostCentreGIF = "9500";
            const string CostCentreReceivingField = "7300";

            // run possibly empty stewardship calculation, to process all gifts that do not belong to this test
            TStewardshipCalculationWebConnector.PerformStewardshipCalculation(FLedgerNumber,
                PeriodNumber, out VerificationResults);
            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "Performing initial Stewardship Calculation Failed!");

            // make sure we have some admin fees
            ImportAdminFees();

            decimal AdminGrantIncomeBefore = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ADMIN_FEE_INCOME_ACCT,
                (FLedgerNumber * 100).ToString("0000")).YtdActual;
            decimal GIFBefore = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_SETTLEMENT,
                CostCentreGIF).YtdActual;
            decimal RecipientBefore = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_SETTLEMENT,
                CostCentreReceivingField).YtdActual;
            decimal ClearingHouseBefore = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_ICH,
                (FLedgerNumber * 100).ToString("0000")).YtdActual;

            // import new gift batch. use proper period and date effective
            DateTime PeriodStartDate, PeriodEndDate;
            TFinancialYear.GetStartAndEndDateOfPeriod(FLedgerNumber, PeriodNumber, out PeriodStartDate, out PeriodEndDate, null);
            ImportAndPostGiftBatch(PeriodStartDate);

            TStewardshipCalculationWebConnector.PerformStewardshipCalculation(FLedgerNumber,
                PeriodNumber, out VerificationResults);
            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "Performing Stewardship Calculation Failed!");

            // Home office keeps 1.40 => 4300/3400 Admin Grant income
            decimal AdminGrantIncomeAfter = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ADMIN_FEE_INCOME_ACCT,
                (FLedgerNumber * 100).ToString("0000")).YtdActual;
            Assert.AreEqual(20.0m * 7.0m / 100.0m, AdminGrantIncomeAfter - AdminGrantIncomeBefore,
                "Home office keeps 7% of 20 Euro gift");

            // GIF should get 0.20 => 9500/5601
            decimal GIFAfter = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_SETTLEMENT,
                CostCentreGIF).YtdActual;

            Assert.AreEqual(20.0m / 100.0m, GIFAfter - GIFBefore,
                "GIF should get 1% of 20 Euro gift. Before: " + GIFBefore + ", After: " + GIFAfter);

            // Receiving field should get 20-1.60 = 18.40 => 7300/5601
            decimal RecipientAfter = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_SETTLEMENT,
                CostCentreReceivingField).YtdActual;
            Assert.AreEqual(20.0m * (100.0m - 1.0m - 7.0m) / 100.0m, RecipientAfter - RecipientBefore,
                "Receiving field should get 92% of 20 Euro gift");

            // Clearing House (4300/8500) should receive the money for GIF and receiving field
            decimal ClearingHouseAfter = new TGet_GLM_Info(FLedgerNumber,
                MFinanceConstants.ICH_ACCT_ICH,
                (FLedgerNumber * 100).ToString("0000")).YtdActual;
            Assert.AreEqual(20.0m * (100.0m - 7.0m) / 100.0m, ClearingHouseAfter - ClearingHouseBefore,
                "We have to give everything apart from our 7% to ICH");
        }

//        /// <summary>
//        /// Test the creation of the ICH Stewardship batch
//        /// </summary>
//        [Test]
//        public void TestGenerateICHStewardshipBatch()
//        {
//                      //ImportAdminFees();
//
//              int PeriodNumber = 5;
//
//              TVerificationResultCollection VerificationResults = new TVerificationResultCollection();
//
//              Assert.IsTrue(TStewardshipCalculationWebConnector.GenerateICHStewardshipBatch(FLedgerNumber, PeriodNumber, ref VerificationResults),"Did not create ICH Stewardship Batch");
//        }
    }
}