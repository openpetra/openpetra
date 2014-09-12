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
using Tests.MReporting.Tools;

namespace Tests.MFinance.Server.Reporting
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TIncExpStatementTest
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
        /// Test the standard Income and Expenses report
        /// </summary>
        [Test]
        public void TestIncExpStatement()
        {
/*
 * Don't run this test - it's not valid in the FastReports world.
            // create a new ledger
            FLedgerNumber = TReportTestingTools.SetupTestLedgerWithPostedBatches();

            string testFile = "../../csharp/ICT/Testing/lib/MFinance/server/Reporting/TestData/IncExpStmt.xml";

            TParameterList SpecificParameters = new TParameterList();
            SpecificParameters.Add("param_start_period_i", 1);
            SpecificParameters.Add("param_end_period_i", 1);
            SpecificParameters.Add("param_costcentreoptions", "SelectedCostCentres");
            string StandardCostCentre = TGLTransactionWebConnector.GetStandardCostCentre(FLedgerNumber);
            SpecificParameters.Add("param_cost_centre_codes", StandardCostCentre);
            TReportTestingTools.CalculateReport(testFile, SpecificParameters, FLedgerNumber);

            TReportTestingTools.TestResult(testFile, FLedgerNumber);
 */
        }
    }
}