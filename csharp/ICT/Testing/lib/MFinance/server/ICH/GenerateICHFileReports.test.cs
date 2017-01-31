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
using System.Data.Odbc;
using System.Collections.Generic;
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
                    "test-sql\\gl-test-feesreceivable-data.sql");
            }
        }

#if disabled_because_none_of_this_tests_any_real_code
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
            string FileName = TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar + "Test.csv";
            bool SendEmail = false;

            TGenFilesReports.GenerateStewardshipFile(FLedgerNumber,
                PeriodNumber,
                ICHProcessingNumber,
                CurrencyType,
                FileName,
                SendEmail,
                out VerificationResults);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "Performing Stewardship File Generation Failed!");
        }

        /// <summary>
        /// Test the generation of the email to send to ICH from a given centre.
        /// marked as Excplicit, so that this test will not be run on Jenkins, due to email configuration issues.
        /// </summary>
        [Test, Explicit]
        public void TestGenerateICHEmail()
        {
            TVerificationResultCollection VerificationResults = new TVerificationResultCollection();

            int PeriodNumber = 5;
            int ICHProcessingNumber = 1;
            int CurrencyType = 1; //base
            string FileName = TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar + "TestGenerateICHEmail.csv";
            bool SendEmail = true;

            // make sure there is a valid email destination
            if (TGenFilesReports.GetICHEmailAddress(null).Length == 0)
            {
                string sqlStatement =
                    String.Format("INSERT INTO PUB_{0}({1},{2},{3},{4}) VALUES (?,?,?,?)",
                        AEmailDestinationTable.GetTableDBName(),
                        AEmailDestinationTable.GetFileCodeDBName(),
                        AEmailDestinationTable.GetConditionalValueDBName(),
                        AEmailDestinationTable.GetPartnerKeyDBName(),
                        AEmailDestinationTable.GetEmailAddressDBName());

                OdbcParameter parameter;

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                parameter = new OdbcParameter("name", OdbcType.VarChar);
                parameter.Value = MFinanceConstants.EMAIL_FILE_CODE_STEWARDSHIP;
                parameters.Add(parameter);
                parameter = new OdbcParameter("condition", OdbcType.VarChar);
                parameter.Value = Convert.ToInt64(MFinanceConstants.ICH_COST_CENTRE) / 100;
                parameters.Add(parameter);
                parameter = new OdbcParameter("partnerkey", OdbcType.Int);
                parameter.Value = Convert.ToInt64(MFinanceConstants.ICH_COST_CENTRE) * 10000;
                parameters.Add(parameter);
                parameter = new OdbcParameter("email", OdbcType.VarChar);
                parameter.Value = TAppSettingsManager.GetValue("ClearingHouse.EmailAddress");
                parameters.Add(parameter);

                bool SubmissionOK = true;
                TDBTransaction DBTransaction = null;
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref DBTransaction, ref SubmissionOK,
                    delegate
                    {
                        DBAccess.GDBAccessObj.ExecuteNonQuery(sqlStatement, DBTransaction, parameters.ToArray());
                        DBAccess.GDBAccessObj.CommitTransaction();
                    });
            }

            TGenFilesReports.GenerateStewardshipFile(FLedgerNumber,
                PeriodNumber,
                ICHProcessingNumber,
                CurrencyType,
                FileName,
                SendEmail,
                out VerificationResults);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResults,
                "Performing ICH Email File Generation Failed!");
        }
#endif

    }
}