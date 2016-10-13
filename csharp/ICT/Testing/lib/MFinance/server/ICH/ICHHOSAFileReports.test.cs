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
using System.Collections.Generic;

namespace Tests.MFinance.Server.ICH
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TICHHOSAFileReportsTest
    {
        Int32 FLedgerNumber = -1;

        const string MainFeesPayableCode = "GIF";
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
        /// Test whether the code opens a text file and replaces the first line
        ///  with the specified string
        /// </summary>
        [Test]
        public void TestFileHeaderReplace()
        {
            string fileName = TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar + "TestGenHOSAFileHeaderReplace.csv";
            int PeriodNumber = 4;
            string StandardCostCentre = "4300";
            string CostCentre = "78";
            string Currency = "USD";

            StreamWriter sw = new StreamWriter(fileName);

            sw.WriteLine("/** Header **,4,4300,78,18/01/2013,USD");
            sw.Close();

            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            string TableForExportHeader = "/** Header **" + "," +
                                          PeriodNumber.ToString() + "," +
                                          StandardCostCentre + "," +
                                          CostCentre + "," +
                                          DateTime.Today.ToShortDateString() + "," +
                                          Currency;

            TGenHOSAFilesReportsWebConnector.ReplaceHeaderInFile(fileName, TableForExportHeader, ref VerificationResults);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "Header Replacement in File Failed!");

            File.Delete(fileName);
        }

        /// <summary>
        /// Test generation of HOSA files
        /// </summary>
        [Test, Explicit]
        public void TestGenerateHOSAFiles()
        {
            int LedgerNumber = FLedgerNumber;
            int PeriodNumber = 4;
            int IchNumber = 1;
            string CostCentre = "73";
            String CurrencySelect = MFinanceConstants.CURRENCY_BASE;
            string FileName = TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar + "TestGenHOSAFile.csv";
            TVerificationResultCollection VerificationResults;

            TGenHOSAFilesReportsWebConnector.GenerateHOSAFiles(LedgerNumber,
                PeriodNumber,
                IchNumber,
                CostCentre,
                CurrencySelect,
                FileName,
                out VerificationResults);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "HOSA File Generation Failed!");

            Assert.IsTrue(File.Exists(FileName),
                "HOSA File did not create!");

            File.Delete(FileName);
        }

        /* Not used
         *
         * /// <summary>
         * /// Test generation of HOSA reports
         * /// </summary>
         * [Test]
         * public void TestGenerateHOSAReports()
         * {
         *  int LedgerNumber = FLedgerNumber;
         *  int PeriodNumber = 4;
         *  int IchNumber = 1;
         *  string Currency = "USD";
         *  TVerificationResultCollection VerificationResults;
         *
         *  TGenHOSAFilesReportsWebConnector.GenerateHOSAReports(LedgerNumber, PeriodNumber, IchNumber, Currency, out VerificationResults);
         *
         *  CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
         *      "Performing HOSA Report Generation Failed!");
         * }
         */

        /// <summary>
        /// Test the exporting of gifts as part of the HOSA process
        /// </summary>
        [Test]
        public void TestExportGifts()
        {
            int LedgerNumber = FLedgerNumber;
            string CostCentre = "7300";
            string AcctCode = "0200";
            string MonthName = "January";
            int PeriodNumber = 1;
            DateTime PeriodStartDate = new DateTime(DateTime.Today.Year, 1, 1);
            DateTime PeriodEndDate = new DateTime(DateTime.Today.Year, 1, 31);
            string Base = MFinanceConstants.CURRENCY_BASE;
            int IchNumber = 0;
            DataTable TableForExport = new DataTable();

            bool NewTransaction = false;

            // otherwise period 1 might have been closed already
            CommonNUnitFunctions.ResetDatabase();

            // need to create gifts first
            TStewardshipCalculationTest.ImportAndPostGiftBatch(PeriodEndDate);

            //Perform stewardship calculation
            TVerificationResultCollection VerificationResults;
            List <Int32>glBatchNumbers;

            TStewardshipCalculationWebConnector.PerformStewardshipCalculation(FLedgerNumber,
                PeriodNumber,
                out glBatchNumbers,
                out VerificationResults);

            VerificationResults = new TVerificationResultCollection();

            //Create DataTable to receive exported transactions
            TableForExport.Columns.Add("CostCentre", typeof(string));
            TableForExport.Columns.Add("Account", typeof(string));
            TableForExport.Columns.Add("LedgerMonth", typeof(string));
            TableForExport.Columns.Add("ICHPeriod", typeof(string));
            TableForExport.Columns.Add("Date", typeof(DateTime));
            TableForExport.Columns.Add("IndividualDebitTotal", typeof(decimal));
            TableForExport.Columns.Add("IndividualCreditTotal", typeof(decimal));

            TGenHOSAFilesReportsWebConnector.ExportGifts(LedgerNumber,
                CostCentre,
                AcctCode,
                MonthName,
                PeriodNumber,
                PeriodStartDate,
                PeriodEndDate,
                Base,
                IchNumber,
                TableForExport,
                VerificationResults);

            TableForExport.AcceptChanges();

            DataRow[] DR = TableForExport.Select();

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "HOSA - Performing Export of gifts Failed!");

            Assert.IsTrue((DR.Length > 0),
                "HOSA - Performing Export of gifts Failed to return any rows!");

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }
    }
}