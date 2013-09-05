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
using System.Threading;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Data;
using Ict.Petra.Server.MReporting.UIConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.Interfaces;

namespace Tests.MFinance.Server.Reporting
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TBalanceSheetTest
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
        /// Test the standard balance sheet
        /// </summary>
        [Test]
        public void TestBalanceSheet()
        {
            // create a new ledger
            FLedgerNumber = CommonNUnitFunctions.CreateNewLedger();

            // post a sample batch
            TCommonAccountingTool commonAccountingTool =
                new TCommonAccountingTool(FLedgerNumber, "NUNIT");

            commonAccountingTool.AddBaseCurrencyJournal();
            commonAccountingTool.JournalDescription = "Test Data accounts";
            string strAccountBank = "6000";
            string StandardCostCentre = TGLTransactionWebConnector.GetStandardCostCentre(FLedgerNumber);
            // Accounting of some gifts ...
            commonAccountingTool.AddBaseCurrencyTransaction(
                strAccountBank, StandardCostCentre, "Gift Example", "Debit", MFinanceConstants.IS_DEBIT, 100);
            commonAccountingTool.AddBaseCurrencyTransaction(
                "0100", StandardCostCentre, "Gift Example", "Credit", MFinanceConstants.IS_CREDIT, 100);
            commonAccountingTool.CloseSaveAndPost();

            TReportGeneratorUIConnector ReportGenerator = new TReportGeneratorUIConnector();
            TParameterList Parameters = new TParameterList();
            string testFile = "../../csharp/ICT/Testing/lib/MFinance/server/Reporting/TestData/BalanceSheetDetail.xml";
            string resultFile = testFile.Replace(".xml", ".Results.xml");
            Parameters.Load(testFile);
            Parameters.Add("param_ledger_number_i", FLedgerNumber);
            Parameters.Add("param_start_period_i", 1);
            Parameters.Add("param_end_period_i", 1);

            ReportGenerator.Start(Parameters.ToDataTable());

            while (ReportGenerator.AsyncExecProgressServerSide.ProgressState == TAsyncExecProgressState.Aeps_Executing)
            {
                Thread.Sleep(500);
            }

            Assert.IsTrue(ReportGenerator.GetSuccess(), "Report did not run successfully");
            TResultList Results = new TResultList();
            TLogging.Log(ReportGenerator.GetResult().Rows.Count.ToString());
            Results.LoadFromDataTable(ReportGenerator.GetResult());
            Parameters.LoadFromDataTable(ReportGenerator.GetParameter());

            Results.WriteCSV(Parameters, resultFile, ",", true, false);
        }
    }
}