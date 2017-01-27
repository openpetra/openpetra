//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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

namespace Tests.MPartner.Server.Reporting
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerReportsTest
    {
        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
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
        /// Test the partner by special types report
        /// </summary>
        [Test]
        public void TestPartnerBySpecialTypes()
        {
            string testFile = "../../csharp/ICT/Testing/lib/MPartner/server/Reporting/TestData/PartnerBySpecialTypes.Test.xml";

            TParameterList SpecificParameters = new TParameterList();
            SpecificParameters.Add("param_address_date_valid_on", new TVariant(new DateTime(DateTime.Today.Year, 1, 1)));
            SpecificParameters.Add("param_explicit_specialtypes", new TVariant("LEDGER"));

            TReportTestingTools.CalculateReport(testFile, SpecificParameters);

            TReportTestingTools.TestResult(testFile);
        }

        /// <summary>
        /// Test the partner by city report
        /// </summary>
        [Test]
        public void TestPartnerByCity()
        {
            string testFile = "../../csharp/ICT/Testing/lib/MPartner/server/Reporting/TestData/PartnerByCity.Test.xml";

            TParameterList SpecificParameters = new TParameterList();
            SpecificParameters.Add("param_today", new TVariant(DateTime.Today));
            SpecificParameters.Add("param_city", new TVariant("Westhausen"));

            TReportTestingTools.CalculateReport(testFile, SpecificParameters);

            TReportTestingTools.TestResult(testFile);
        }
    }
}