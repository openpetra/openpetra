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
    public class TGenerateICHFileReportsTest
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
        /// this test loads the sample partners, imports a gift batch, and posts it, and then runs a stewardship calculation
        /// </summary>
        [Test]
        public void TestGenerateStewardshipFile()
        {
            ImportAdminFees();

            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            int PeriodNumber = 5;
            int ICHProcessingNumber = 1;
            int CurrencyType = 1; //base
            string FileName = @"C:\Test.csv";
            bool SendEmail = true;

            TGenFilesReports.GenerateStewardshipFile(FLedgerNumber,
                PeriodNumber,
                ICHProcessingNumber,
                CurrencyType,
                FileName,
                SendEmail,
                out VerificationResults);

            Assert.IsFalse(VerificationResults.HasCriticalErrors,
                "Performing Stewardship File Generation Failed!" + VerificationResults.BuildVerificationResultString());
        }

        /// <summary>
        /// Test the generation of the email to send to ICH from a given centre
        /// </summary>
        [Test]
        public void TestGenerateICHEmail()
        {
            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            int PeriodNumber = 5;
            int ICHProcessingNumber = 1;
            int CurrencyType = 1; //base
            string FileName = @"C:\TestGenerateICHEmail.csv";
            bool SendEmail = true;

            TGenFilesReports.GenerateStewardshipFile(FLedgerNumber,
                PeriodNumber,
                ICHProcessingNumber,
                CurrencyType,
                FileName,
                SendEmail,
                out VerificationResults);

            Assert.IsFalse(VerificationResults.HasCriticalErrors,
                "Performing ICH Email File Generation Failed!" + VerificationResults.BuildVerificationResultString());
        }
    }
}