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
using Ict.Petra.Server.MReporting.UIConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.Interfaces.MReporting;
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
        public static void CalculateReport(string AReportParameterXmlFile, TParameterList ASpecificParameters, int ALedgerNumber = -1)
        {
            // important: otherwise month names are in different language, etc
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);

            TReportGeneratorUIConnector ReportGenerator = new TReportGeneratorUIConnector();
            TParameterList Parameters = new TParameterList();
            string resultFile = AReportParameterXmlFile.Replace(".xml", ".Results.xml");
            string parameterFile = AReportParameterXmlFile.Replace(".xml", ".Parameters.xml");
            Parameters.Load(AReportParameterXmlFile);

            if (ALedgerNumber != -1)
            {
                Parameters.Add("param_ledger_number_i", ALedgerNumber);
            }

            Parameters.Add(ASpecificParameters);

            ReportGenerator.Start(Parameters.ToDataTable());

            while (!ReportGenerator.Progress.JobFinished)
            {
                Thread.Sleep(500);
            }

            Assert.IsTrue(ReportGenerator.GetSuccess(), "Report did not run successfully");
            TResultList Results = new TResultList();

            Results.LoadFromDataTable(ReportGenerator.GetResult());
            Parameters.LoadFromDataTable(ReportGenerator.GetParameter());

            if (!Parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT1, -1, eParameterFit.eBestFit))
            {
                Parameters.Add("ControlSource", new TVariant("Left1"), ReportingConsts.HEADERPAGELEFT1);
            }

            if (!Parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT2, -1, eParameterFit.eBestFit))
            {
                Parameters.Add("ControlSource", new TVariant("Left2"), ReportingConsts.HEADERPAGELEFT2);
            }

            Parameters.Save(parameterFile, false);
            Results.WriteCSV(Parameters, resultFile, ",", false, false);
        }

        /// <summary>
        /// compare the written result and parameter files with the files approved by a domain expert
        /// </summary>
        public static void TestResult(string AReportParameterXmlFile, int ALedgerNumber = -1)
        {
            string resultFile = AReportParameterXmlFile.Replace(".xml", ".Results.xml");
            string parameterFile = AReportParameterXmlFile.Replace(".xml", ".Parameters.xml");
            string resultExpectedFile = AReportParameterXmlFile.Replace(".xml", ".Results.Expected.xml");
            string parameterExpectedFile = AReportParameterXmlFile.Replace(".xml", ".Parameters.Expected.xml");

            SortedList <string, string>ToReplace = new SortedList <string, string>();
            ToReplace.Add("{ledgernumber}", ALedgerNumber.ToString());
            int currentYear = DateTime.Today.Year;
            ToReplace.Add("{ThisYear}", currentYear.ToString());
            ToReplace.Add("{PreviousYear}", (currentYear - 1).ToString());

            Assert.True(TTextFile.SameContent(resultFile, resultExpectedFile, true, ToReplace, true),
                "the file " + resultFile + " should have the same content as " + resultExpectedFile);

            Assert.True(TTextFile.SameContent(parameterFile, parameterExpectedFile, true, ToReplace, true),
                "the file " + parameterFile + " should have the same content as " + parameterExpectedFile);

            File.Delete(parameterFile);
            File.Delete(resultFile);
        }
    }
}