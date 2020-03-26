//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using NUnit.Framework;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Petra.Shared.MSponsorship.Data;
using Ict.Petra.Server.MSponsorship.WebConnectors;

using Tests.MPartner.shared.CreateTestPartnerData;

namespace Tests.MSponsorship.Server.MSponsorship
{
    /// This will runs test to ensure that everything related to child management works as intended
    /// Creating a new child, 
    ///     + adding new reminder, dates, sponsorships and school/family events
    /// 
    /// then selecting all new generated entrys and compare them to the wanted result
    /// then edites them an checks again
    [TestFixture]
    public class TSponsorshipTesting
    {
        Int32 FLedgerNumber = -1;
        Int64 APartnerKey = -1;

        Int32 totalChildsBeforeTest = -1;
        Int32 totalChildsAfterTest = -1;

        String new_child_firstname = "";
        String new_child_lastname = "";
        String new_child_status = "";
        DateTime new_child_birth = new DateTime(2012, 6, 2);

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
            TLogging.Log("Selected Ledger Number = " + FLedgerNumber);
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// arppper for all test because nunit is not liniar 
        /// </summary>
        [Test]
        public void RunTest()
        {
            TLogging.Log("Running: selectAllChildren");
            SelectAllChilds();

            TLogging.Log("Running: createNewChild");
            CreateNewChild();

            TLogging.Log("Running: selectCreatedChild");
            SelectCreatedChild();

            TLogging.Log("Running: createNewReminders");
            CreateNewReminders();

            TLogging.Log("Running: createNewComments");
            CreateNewComments();

            TLogging.Log("Running: createNewSponsorship");
            CreateNewSponsorship();
        }

        /// <summary>
        /// This will select all childs entrys of the webconnector
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_FindChildren
        /// </summary>
        public void SelectAllChilds()
        {


            SponsorshipFindTDSSearchResultTable Result = TSponsorshipWebConnector.FindChildren("", "", "", "", "");

            totalChildsBeforeTest = Result.Rows.Count;
            TLogging.Log("All Childs returns: " + totalChildsBeforeTest);
        }

        /// <summary>
        /// This will select all childs entrys of the webconnector
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild
        /// </summary>
        public void CreateNewChild()
        {
            new_child_firstname = RandomString(15, false);
            new_child_lastname = RandomString(8, false);
            new_child_status = "CHILDREN_HOME";

            TVerificationResultCollection VRC = new TVerificationResultCollection();

            bool success =  TSponsorshipWebConnector.MaintainChild(
                new_child_status,
                new_child_firstname,
                new_child_lastname,
                new_child_birth,
                "",
                false,
                "MALE",
                "",
                APartnerKey,
                FLedgerNumber,
                out VRC
            );

            if (success)
            {
                TLogging.Log("Created new Child with following args:" + VRC.ToString());
            }
            else
            {
                TLogging.Log("Creating new Child fail, all other test not possible");
                Assert.False(true);
            }

        }

        /// <summary>
        /// Trys to select the same child that we just created
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_FindChildren
        /// </summary>
        public void SelectCreatedChild()
        {
            SponsorshipFindTDSSearchResultTable Result = TSponsorshipWebConnector.FindChildren(
                new_child_firstname,
                new_child_lastname,
                "",
                "",
                ""
            );

            // we expect one result only
            Assert.AreEqual(Result.Rows.Count, 1);

            SponsorshipFindTDSSearchResultRow Row = Result[0];
            APartnerKey = Row.PartnerKey;

            TLogging.Log("Found new Child with id: "+APartnerKey);
        }

        /// <summary>
        /// Adds new reminders to the created child
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildReminders
        /// </summary>
        public void CreateNewReminders()
        {

            // SponsorshipFindTDSSearchResultTable Result = TSponsorshipWebConnector.FindChildren("", "", "", "", "");

            TLogging.Log("Created new Reminder:");
        }

        /// <summary>
        /// Adds new School and Family comments to the created child
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildComments
        /// </summary>
        public void CreateNewComments()
        {

            // SponsorshipFindTDSSearchResultTable Result = TSponsorshipWebConnector.FindChildren("", "", "", "", "");

            TLogging.Log("Created new Comments for School:");
            TLogging.Log("Created new Comments for Family:");
        }

        /// <summary>
        /// Creates a new Recurring gift the new child
        /// serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainSponsorshipRecurringGifts
        /// </summary>
        public void CreateNewSponsorship()
        {

            // SponsorshipFindTDSSearchResultTable Result = TSponsorshipWebConnector.FindChildren("", "", "", "", "");

            TLogging.Log("Created new Sponsorship:");
        }

        /// utils



        /// returns random strings
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

    }
}
