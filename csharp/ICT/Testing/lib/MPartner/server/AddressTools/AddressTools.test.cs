//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
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
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Tests.MPartner.shared.CreateTestPartnerData;


namespace Tests.MPartner.Server.AddressTools
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public partial class TAddressToolsTest
    {
        private PartnerEditTDS MainDS = new PartnerEditTDS();
        private long TestPartnerKey;
        private int LocationKey = 1000;

        #region Init and TearDown

        /// <summary>
        /// Open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");

            CommonNUnitFunctions.ResetDatabase();
            PPartnerRow Partner1 = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            TestPartnerKey = Partner1.PartnerKey;

            //Guard assert: ensure database has been properly reset
            TDBTransaction ATransaction = null;
            DataTable ResultTable = null;
            string Query = string.Format("SELECT * FROM p_location WHERE p_site_key_n = {0} AND p_location_key_i = {1}",
                DomainManager.GSiteKey,
                LocationKey);
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ATransaction,
                delegate
                {
                    ResultTable = DBAccess.GetDBAccessObj(ATransaction).SelectDT(Query, "CheckLocations", ATransaction);
                });
            Assert.AreEqual(0, ResultTable.Rows.Count);
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        #endregion

        /// <summary>
        /// Test the address tools
        /// </summary>
        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test1_ExpiredNonMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            SaveChanges();


            ActAndAssert("Best expired non-mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test2_FutureNonMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            CreateFutureAddresses(false);
            SaveChanges();

            ActAndAssert("Best future non-mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test3_CurrentNonMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            CreateFutureAddresses(false);
            CreateCurrentAddresses(false);
            SaveChanges();

            ActAndAssert("Best current non-mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test4_ExpiredMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            CreateFutureAddresses(false);
            CreateCurrentAddresses(false);
            CreateExpiredAddresses(true);
            SaveChanges();

            ActAndAssert("Best expired mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test5_FutureMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            CreateFutureAddresses(false);
            CreateCurrentAddresses(false);
            CreateExpiredAddresses(true);
            CreateFutureAddresses(true);
            SaveChanges();

            ActAndAssert("Best future mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test6_CurrentMailAddress()
        {
            // Arrange
            CreateExpiredAddresses(false);
            CreateFutureAddresses(false);
            CreateCurrentAddresses(false);
            CreateExpiredAddresses(true);
            CreateFutureAddresses(true);
            CreateCurrentAddresses(true);
            SaveChanges();

            ActAndAssert("Best current mail address");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void TestEqualDates()
        {
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-1), null, true, "First created", MainDS);
            SaveChanges();
            System.Threading.Thread.Sleep(5000);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-1), null, true, "Second created", MainDS);
            SaveChanges();

            ActAndAssert("First created");
        }

        /// <summary>
        /// Run the queries and check the results
        /// </summary>
        /// <param name="StreetLine"></param>
        public void ActAndAssert(string StreetLine)
        {
            string ACountryName;
            PLocationTable AAddress = null;
            TDBTransaction ATransaction = null;
            DataTable DonorAddresses = null;

            // Act
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ATransaction,
                delegate
                {
                    TAddressTools.GetBestAddress(TestPartnerKey, out AAddress, out ACountryName, ATransaction);
                    DonorAddresses = TAddressTools.GetBestAddressForPartners(TestPartnerKey.ToString(), ATransaction);
                });

            // Assert
            Assert.AreEqual(1, AAddress.Rows.Count, "GetBestAddress returned wrong number of addresses");
            Assert.AreEqual(1, DonorAddresses.Rows.Count, "GetBestAddressForPartners returned wrong number of addresses");
            Assert.AreEqual(TestPartnerKey + " " + StreetLine,
                ((PLocationRow)AAddress.Rows[0]).StreetName,
                "GetBestAddress returned unexpected address");
            Assert.AreEqual(((PLocationRow)AAddress.Rows[0]).LocationKey, DonorAddresses.Rows[0]["p_location_key_i"]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Mailing"></param>
        public void CreateExpiredAddresses(bool Mailing)
        {
            string Suffix = Mailing ? "mail address" : "non-mail address";

            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-18), DateTime.Today.AddMonths(
                    -17), Mailing, "Rubbish expired " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-2), DateTime.Today.AddMonths(-1), Mailing, "Best expired " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(-2), Mailing, "Poor expired " + Suffix, MainDS);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Mailing"></param>
        public void CreateFutureAddresses(bool Mailing)
        {
            string Suffix = Mailing ? "mail address" : "non-mail address";

            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(+18), null, Mailing, "Rubbish future " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(+1), null, Mailing, "Best future " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(+6), null, Mailing, "Poor expired " + Suffix, MainDS);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Mailing"></param>
        public void CreateCurrentAddresses(bool Mailing)
        {
            string Suffix = Mailing ? "mail address" : "non-mail address";

            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-18), null, Mailing, "Rubbish current " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-2), null, Mailing, "Best current " + Suffix, MainDS);
            CreateNewLocation(TestPartnerKey, DateTime.Today.AddMonths(-3), null, Mailing, "Poor current " + Suffix, MainDS);
        }

        /// <summary>
        /// Sets up test location rows
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="DateEffective"></param>
        /// <param name="GoodUntil"></param>
        /// <param name="Mailing"></param>
        /// <param name="Street"></param>
        /// <param name="AMainDS"></param>
        public void CreateNewLocation(Int64 APartnerKey,
            DateTime DateEffective,
            DateTime? GoodUntil,
            bool Mailing,
            string Street,
            PartnerEditTDS AMainDS)
        {
            // avoid duplicate addresses: StreetName contains the partner key
            PLocationRow LocationRow = AMainDS.PLocation.NewRowTyped();

            LocationRow.SiteKey = DomainManager.GSiteKey;
            LocationRow.LocationKey = LocationKey++;
            LocationRow.StreetName = APartnerKey.ToString() + " " + Street;
            LocationRow.PostalCode = "LO2 2CX";
            LocationRow.City = "London";
            LocationRow.CountryCode = "99";
            AMainDS.PLocation.Rows.Add(LocationRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.PartnerKey = APartnerKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            PartnerLocationRow.DateEffective = DateEffective;
            PartnerLocationRow.DateGoodUntil = GoodUntil;
            PartnerLocationRow.SendMail = Mailing;
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveChanges()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result = UIConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult, "There was a critical error when saving:");
        }
    }
}