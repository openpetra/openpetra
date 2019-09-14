//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Collections;
using System.Threading;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Data;
using Ict.Petra.Server.MReporting.WebConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Tests.MReporting.Tools
{
    /// tools for testing the finance reports
    public class TReportTestingTools
    {
        /// <summary>
        /// setup a ledger with simple test data
        /// </summary>
        public static int SetupTestLedgerWithPostedBatches()
        {
            // create a new ledger
            int LedgerNumber = CommonNUnitFunctions.CreateNewLedger();

            // post a sample batch
            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(LedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();
            commonAccountingTool.JournalDescription = "Test Data accounts";
            string strAccountBank = "6000";
            string StandardCostCentre = TGLTransactionWebConnector.GetStandardCostCentre(LedgerNumber);

            // Accounting of a start balance
            commonAccountingTool.AddBaseCurrencyTransaction(
                "6200", StandardCostCentre, "Start Balance", "Debit", MFinanceConstants.IS_DEBIT, 40);
            commonAccountingTool.AddBaseCurrencyTransaction(
                "9700", StandardCostCentre, "Start Balance", "Credit", MFinanceConstants.IS_CREDIT, 40);
            // Accounting of some gifts ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, StandardCostCentre, "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);
            commonAccountingTool.AddBaseCurrencyTransaction(
                "0100", StandardCostCentre, "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);
            // Accounting of some expense ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, StandardCostCentre, "Expense Example", "Credit", MFinanceConstants.IS_CREDIT, 20);
            commonAccountingTool.AddBaseCurrencyTransaction(
                "4200", StandardCostCentre, "Expense Example", "Debit", MFinanceConstants.IS_DEBIT, 20);
            commonAccountingTool.CloseSaveAndPost(); // returns true if posting seemed to work

            return LedgerNumber;
        }

        /// <summary>
        /// calculate the report and save the result and returned parameters to file
        /// </summary>
        public static void CalculateReport(string AReportParameterJsonFile, string AReportResultFile, TParameterList ASpecificParameters, int ALedgerNumber = -1)
        {
            // important: otherwise month names are in different language, etc
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);

            TParameterList Parameters = new TParameterList();

            if (AReportParameterJsonFile.IndexOf(".json") == -1)
            {
                throw new Exception("invalid report name, should end with .json");
            }

            string resultFile = AReportResultFile;
            Parameters.Load(AReportParameterJsonFile);

            if (ALedgerNumber != -1)
            {
                Parameters.Add("param_ledger_number_i", ALedgerNumber);
            }

            Parameters.Add(ASpecificParameters);

            string ReportID = TReportGeneratorWebConnector.Create();
            TReportGeneratorWebConnector.Start(ReportID, Parameters.ToDataTable());

            while (!TReportGeneratorWebConnector.Progress(ReportID).JobFinished)
            {
                Thread.Sleep(500);
            }

            Assert.IsTrue(TReportGeneratorWebConnector.GetSuccess(ReportID), "Report did not run successfully");

            using (StreamWriter sw = new StreamWriter(resultFile))
            {
                sw.Write(TReportGeneratorWebConnector.DownloadHTML(ReportID));
            }
        }

        /// <summary>
        /// compare the written result and parameter files with the files approved by a domain expert
        /// </summary>
        public static void TestResult(string AResultsFile, int ALedgerNumber = -1)
        {
            if (AResultsFile.IndexOf(".Results.html") == -1)
            {
                throw new Exception("invalid file name, should end with .Results.html");
            }

            string resultFile = AResultsFile;
            string resultExpectedFile = AResultsFile.Replace(".Results.html", ".Results.Expected.html");

            SortedList <string, string>ToReplace = new SortedList <string, string>();
            ToReplace.Add("{ledgernumber}", ALedgerNumber.ToString());
            ToReplace.Add("{Today}", DateTime.Today.ToString("yyyy-MM-dd"));

            int currentYear = DateTime.Today.Year;
            ToReplace.Add("{ThisYear}", currentYear.ToString());
            ToReplace.Add("{PreviousYear}", (currentYear - 1).ToString());

            if (ALedgerNumber != -1)
            {
                TLedgerInfo ledger = new TLedgerInfo(ALedgerNumber);
                ToReplace.Add("{CurrentPeriod}", ledger.CurrentPeriod.ToString());
            }

            Assert.True(TTextFile.SameContent(resultFile, resultExpectedFile, true, ToReplace, true),
                "the file " + resultFile + " should have the same content as " + resultExpectedFile);

            File.Delete(resultFile);
        }
    }
}
