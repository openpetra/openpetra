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
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;

namespace Tests.MPartner.Server.PartnerEdit
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerEditTest
    {
        private bool[] Categories;
        
        /// <summary>
        /// use automatic property to avoid compiler warning about unused variable FServerManager
        /// </summary>
        private TServerManager FServerManager {
            get; set;
        }

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            new TLogging("../../log/TestServer.log");
            FServerManager = TPetraServerConnector.Connect("../../etc/TestServer.config");
            
            Categories = new bool[20];

            for (int i = 0; i < 20; i++)
            {
                Categories[i] = true;
            }
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// create a new partner
        public static PPartnerRow CreateNewPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            // get a new partner key
            Int64 newPartnerKey = -1;

            do
            {
                newPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, newPartnerKey, ref newPartnerKey);
                PartnerRow.PartnerKey = newPartnerKey;
            } while (newPartnerKey == -1);

            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            AMainDS.PPartner.Rows.Add(PartnerRow);

            TLogging.Log("Creating new partner: " + PartnerRow.PartnerKey.ToString());

            return PartnerRow;
        }

        /// create a new family
        public static PPartnerRow CreateNewFamilyPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestPartner, Mr";

            PFamilyRow FamilyRow = AMainDS.PFamily.NewRowTyped();
            FamilyRow.PartnerKey = PartnerRow.PartnerKey;
            FamilyRow.FamilyName = PartnerRow.PartnerKey.ToString();
            FamilyRow.FirstName = "TestPartner";
            FamilyRow.Title = "Mr";
            AMainDS.PFamily.Rows.Add(FamilyRow);

            return PartnerRow;
        }

        /// create a new person
        public static PPersonRow CreateNewPerson(PartnerEditTDS AMainDS,
            TPartnerEditUIConnector AConnector,
            Int64 AFamilyKey,
            Int32 ALocationKey,
            string AFirstName,
            string ATitle,
            int AFamilyID)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            PartnerRow.PartnerShortName = AFamilyKey.ToString() + ", " + AFirstName + ", " + ATitle;

            PPersonRow PersonRow = AMainDS.PPerson.NewRowTyped();
            PersonRow.PartnerKey = PartnerRow.PartnerKey;
            PersonRow.FamilyKey = AFamilyKey;
            PersonRow.FamilyName = AFamilyKey.ToString();
            PersonRow.FirstName = AFirstName;
            PersonRow.FamilyId = AFamilyID;
            PersonRow.Title = ATitle;
            AMainDS.PPerson.Rows.Add(PersonRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = ALocationKey;
            PartnerLocationRow.TelephoneNumber = PersonRow.PartnerKey.ToString();
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            return PersonRow;
        }

        /// create a new unit
        public static PPartnerRow CreateNewUnitPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestUnit";

            PUnitRow UnitRow = AMainDS.PUnit.NewRowTyped();
            UnitRow.PartnerKey = PartnerRow.PartnerKey;
            UnitRow.UnitName = "TestUnit";
            AMainDS.PUnit.Rows.Add(UnitRow);

            return PartnerRow;
        }

        /// create a new organisation
        public static PPartnerRow CreateNewOrganisationPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_ORGANISATION;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestOrganisation";

            POrganisationRow OrganisationRow = AMainDS.POrganisation.NewRowTyped();
            OrganisationRow.PartnerKey = PartnerRow.PartnerKey;
            OrganisationRow.OrganisationName = "TestOrganisation";
            AMainDS.POrganisation.Rows.Add(OrganisationRow);

            return PartnerRow;
        }

        /// create a new church
        public static PPartnerRow CreateNewChurchPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;

            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            // make sure denomation "UNKNOWN" exists as this is the default value
            if (!PDenominationAccess.Exists("UNKNOWN", DBAccess.GDBAccessObj.Transaction))
            {
                PDenominationTable DenominationTable = new PDenominationTable();
                PDenominationRow DenominationRow = DenominationTable.NewRowTyped();
                DenominationRow.DenominationCode = "UNKNOWN";
                DenominationRow.DenominationName = "Unknown";
                DenominationTable.Rows.Add(DenominationRow);
                PDenominationAccess.SubmitChanges(DenominationTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult);
            }

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_CHURCH;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestChurch";

            PChurchRow ChurchRow = AMainDS.PChurch.NewRowTyped();
            ChurchRow.PartnerKey = PartnerRow.PartnerKey;
            ChurchRow.ChurchName = "TestChurch";
            ChurchRow.DenominationCode = "UNKNOWN";
            AMainDS.PChurch.Rows.Add(ChurchRow);

            return PartnerRow;
        }

        /// create a new bank
        public static PPartnerRow CreateNewBankPartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_BANK;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestBank";

            PBankRow BankRow = AMainDS.PBank.NewRowTyped();
            BankRow.PartnerKey = PartnerRow.PartnerKey;
            BankRow.BranchName = "TestBank";
            AMainDS.PBank.Rows.Add(BankRow);

            return PartnerRow;
        }

        /// create a new venue
        public static PPartnerRow CreateNewVenuePartner(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, AConnector);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_VENUE;
            PartnerRow.PartnerShortName = PartnerRow.PartnerKey.ToString() + ", TestVenue";

            PVenueRow VenueRow = AMainDS.PVenue.NewRowTyped();
            VenueRow.PartnerKey = PartnerRow.PartnerKey;
            VenueRow.VenueCode = "TEST" + PartnerRow.PartnerKey.ToString();
            VenueRow.VenueName = "TestVenue" + PartnerRow.PartnerKey.ToString();
            AMainDS.PVenue.Rows.Add(VenueRow);

            return PartnerRow;
        }

        /// create a new family with persons
        public static void CreateFamilyWithPersonRecords(PartnerEditTDS AMainDS, TPartnerEditUIConnector AConnector)
        {
            PPartnerRow PartnerRow = CreateNewFamilyPartner(AMainDS, AConnector);

            CreateNewLocation(PartnerRow.PartnerKey, AMainDS);

            CreateNewPerson(AMainDS,
                AConnector,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Adam",
                "Mr",
                0);
            CreateNewPerson(AMainDS,
                AConnector,
                PartnerRow.PartnerKey,
                AMainDS.PLocation[0].LocationKey,
                "Eve",
                "Mrs",
                1);
        }

        /// create a new location
        public static void CreateNewLocation(Int64 APartnerKey, PartnerEditTDS AMainDS)
        {
            // avoid duplicate addresses: StreetName contains the partner key
            PLocationRow LocationRow = AMainDS.PLocation.NewRowTyped();

            LocationRow.SiteKey = DomainManager.GSiteKey;
            LocationRow.LocationKey = -1;
            LocationRow.StreetName = APartnerKey.ToString() + " Nowhere Lane";
            LocationRow.PostalCode = "LO2 2CX";
            LocationRow.City = "London";
            LocationRow.CountryCode = "99";
            AMainDS.PLocation.Rows.Add(LocationRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.PartnerKey = APartnerKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            PartnerLocationRow.TelephoneNumber = APartnerKey.ToString();
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        /// <summary>
        /// create a new partner and save it with a new location
        /// </summary>
        [Test]
        public void TestSaveNewPartnerWithLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "TPartnerEditUIConnector SubmitChanges return value");

            // check the location key for this partner. should not be negative
            Assert.AreEqual(1, MainDS.PPartnerLocation.Rows.Count, "TPartnerEditUIConnector SubmitChanges returns one location");
            Assert.Greater(MainDS.PPartnerLocation[0].LocationKey, 0, "TPartnerEditUIConnector SubmitChanges returns valid location key");
        }

        /// <summary>
        /// new partner with location 0.
        /// first save the partner with location 0, then add a new location, and save again
        /// </summary>
        [Test]
        public void TestNewPartnerWithLocation0()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();

            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = 0;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Create a partner with location 0");

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            // remove location 0, same is done in csharp\ICT\Petra\Client\MCommon\logic\UC_PartnerAddresses.cs TUCPartnerAddressesLogic::AddRecord
            // Check if record with PartnerLocation.LocationKey = 0 is around > delete it
            DataRow PartnerLocationRecordZero =
                MainDS.PPartnerLocation.Rows.Find(new object[] { PartnerRow.PartnerKey, DomainManager.GSiteKey, 0 });

            if (PartnerLocationRecordZero != null)
            {
                DataRow LocationRecordZero = MainDS.PLocation.Rows.Find(new object[] { DomainManager.GSiteKey, 0 });

                if (LocationRecordZero != null)
                {
                    LocationRecordZero.Delete();
                }

                PartnerLocationRecordZero.Delete();
            }

            ResponseDS = new PartnerEditTDS();
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Replace location 0 of partner");

            Assert.AreEqual(1, MainDS.PPartnerLocation.Rows.Count, "the partner should only have one location in the dataset");

            // get all addresses of the partner
            PPartnerLocationTable testPartnerLocations = PPartnerLocationAccess.LoadViaPPartner(PartnerRow.PartnerKey, null);
            Assert.AreEqual(1, testPartnerLocations.Rows.Count, "the partner should only have one location");
            Assert.Greater(testPartnerLocations[0].LocationKey, 0, "TPartnerEditUIConnector SubmitChanges returns valid location key");
        }

        /// <summary>
        /// create a new partner and save it with an existing location
        /// </summary>
        [Test]
        public void TestSaveNewPartnerWithExistingLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "saving the first partner with a location");

            Int32 LocationKey = MainDS.PLocation[0].LocationKey;

            MainDS = new PartnerEditTDS();

            PartnerRow = CreateNewFamilyPartner(MainDS, connector);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = LocationKey;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            PPartnerTable PartnerAtAddress = PPartnerAccess.LoadViaPLocation(
                DomainManager.GSiteKey, LocationKey, null);

            Assert.AreEqual(2, PartnerAtAddress.Rows.Count, "there should be two partners at this location");
        }

        /// <summary>
        /// add a new location for a family, and propagate this to the members of the family
        /// </summary>
        [Test]
        public void TestFamilyPropagateNewLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            CreateFamilyWithPersonRecords(MainDS, connector);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            // now change on partner location. should ask about everyone else
            // it seems, the change must be to PLocation. In Petra 2.3, changes to the PartnerLocation are not propagated
            // MainDS.PPartnerLocation[0].DateGoodUntil = new DateTime(2011, 01, 01);

            Assert.AreEqual(1, MainDS.PLocation.Rows.Count, "there should be only one address for the whole family");

            MainDS.PLocation[0].County = "different";

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrInfoNeeded,
                result,
                "should ask if the partner locations of the other members of the family should be changed as well");

            // TODO: simulate the dialog where the user selects which people to propagate the address change for.

            // TODO: what about replacing the whole address?
            // TODO: adding a new location for all members of the family?
        }

        /// <summary>
        /// modify a location that is used by several partners, but only modify the location for this partner.
        /// need to create a new location
        /// </summary>
        [Test]
        public void TestModifyLocationCreateNew()
        {
            // TODO
        }

        /// <summary>
        /// delete a location
        /// </summary>
        [Test]
        public void TestRemoveLocationFromSeveralPartners()
        {
            // TODO
        }

        /// <summary>
        /// when adding a new location to a family, all the partners involved should be updated p_partner.s_date_modified_d.
        /// even if the partner is not part of the dataset yet.
        /// </summary>
        [Test]
        public void TestChangeLocationUpdatesDateModifiedOfAllPartners()
        {
            // TODO
        }

        /// <summary>
        /// check if changing the family will change the family id of all other members of the family as well
        /// </summary>
        [Test]
        public void TestChangeFamilyID()
        {
            // TODO
        }

        /// <summary>
        /// check if deleting of families works
        /// </summary>
        [Test]
        public void TestDeleteFamily()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow FamilyPartnerRow;
            PFamilyRow FamilyRow;
            PPersonRow PersonRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            FamilyPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Create family record");

            // check if Family partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(FamilyPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // add a person to the family which means the family is not allowed to be deleted any longer
            FamilyRow = (PFamilyRow)MainDS.PFamily.Rows[0];
            FamilyRow.FamilyMembers = true;
            CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create new location");

            PartnerEditTDS PersonDS = new PartnerEditTDS();
            PersonRow = CreateNewPerson(PersonDS, connector, FamilyPartnerRow.PartnerKey,
                MainDS.PLocation[0].LocationKey, "Adam", "Mr", 0);
            PersonRow.FamilyKey = FamilyPartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref PersonDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create person record");

            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(FamilyPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);


            // create new family and create subscription given as gift from this family: not allowed to be deleted
            FamilyPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PPublicationTable PublicationTable = PPublicationAccess.LoadByPrimaryKey("TESTPUBLICATION", DBAccess.GDBAccessObj.Transaction);

            if (PublicationTable.Count == 0)
            {
                // first check if frequency "Annual" exists and if not then create it
                if (!AFrequencyAccess.Exists("Annual", DBAccess.GDBAccessObj.Transaction))
                {
                    // set up details (e.g. bank account) for this Bank so deletion is not allowed
                    AFrequencyTable FrequencyTable = new AFrequencyTable();
                    AFrequencyRow FrequencyRow = FrequencyTable.NewRowTyped();
                    FrequencyRow.FrequencyCode = "Annual";
                    FrequencyRow.FrequencyDescription = "Annual Frequency";
                    FrequencyTable.Rows.Add(FrequencyRow);

                    Assert.IsTrue(AFrequencyAccess.SubmitChanges(FrequencyTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult));
                }

                // now add the publication "TESTPUBLICATION"
                PPublicationRow PublicationRow = PublicationTable.NewRowTyped();
                PublicationRow.PublicationCode = "TESTPUBLICATION";
                PublicationRow.FrequencyCode = "Annual";
                PublicationTable.Rows.Add(PublicationRow);

                Assert.IsTrue(PPublicationAccess.SubmitChanges(PublicationTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult));
            }

            // make sure that "reason subscription given" exists
            if (!PReasonSubscriptionGivenAccess.Exists("FREE", DBAccess.GDBAccessObj.Transaction))
            {
                // set up details (e.g. bank account) for this Bank so deletion is not allowed
                PReasonSubscriptionGivenTable ReasonTable = new PReasonSubscriptionGivenTable();
                PReasonSubscriptionGivenRow ReasonRow = ReasonTable.NewRowTyped();
                ReasonRow.Code = "FREE";
                ReasonRow.Description = "Free Subscription";
                ReasonTable.Rows.Add(ReasonRow);

                Assert.IsTrue(PReasonSubscriptionGivenAccess.SubmitChanges(ReasonTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult));
            }

            // now add the publication "TESTPUBLICATION" to the first family record and indicate it was a gift from newly created family record
            PSubscriptionRow SubscriptionRow = MainDS.PSubscription.NewRowTyped();
            SubscriptionRow.PublicationCode = "TESTPUBLICATION";
            SubscriptionRow.PartnerKey = FamilyRow.PartnerKey; // link subscription with original family
            SubscriptionRow.GiftFromKey = FamilyPartnerRow.PartnerKey; // indicate that subscription is a gift from newly created family
            SubscriptionRow.ReasonSubsGivenCode = "FREE";
            MainDS.PSubscription.Rows.Add(SubscriptionRow);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "add publication to family record");

            // this should now not be allowed since partner record has a subscription linked to it
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(FamilyPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of Family partner
            FamilyPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PartnerKey = FamilyPartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create family record");

            // check if Family record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Family record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of persons works
        /// </summary>
        [Test]
        public void TestDeletePerson()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow FamilyPartnerRow;
            PPartnerRow UnitPartnerRow;
            PPersonRow PersonRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            // create new family, location and person
            FamilyPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            PersonRow = CreateNewPerson(MainDS,
                connector,
                FamilyPartnerRow.PartnerKey,
                MainDS.PLocation[0].LocationKey,
                "Mike",
                "Mr",
                0);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create family and person record");

            // check if Family partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(PersonRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // add a commitment for the person which means the person is not allowed to be deleted any longer
            UnitPartnerRow = CreateNewUnitPartner(MainDS, connector);
            PmStaffDataTable CommitmentTable = new PmStaffDataTable();
            PmStaffDataRow CommitmentRow = CommitmentTable.NewRowTyped();
            CommitmentRow.Key = Convert.ToInt32(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_staff_data));
            CommitmentRow.PartnerKey = PersonRow.PartnerKey;
            CommitmentRow.StartOfCommitment = DateTime.Today.Date;
            CommitmentRow.EndOfCommitment = DateTime.Today.AddDays(90).Date;
            CommitmentRow.OfficeRecruitedBy = UnitPartnerRow.PartnerKey;
            CommitmentRow.HomeOffice = UnitPartnerRow.PartnerKey;
            CommitmentRow.ReceivingField = UnitPartnerRow.PartnerKey;
            CommitmentTable.Rows.Add(CommitmentRow);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create unit to be used in commitment");
            Assert.IsTrue(PmStaffDataAccess.SubmitChanges(CommitmentTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult));

            // this should now not be allowed since person record has a commitment linked to it
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(PersonRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of Person partner
            FamilyPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            PersonRow = CreateNewPerson(MainDS,
                connector,
                FamilyPartnerRow.PartnerKey,
                MainDS.PLocation[0].LocationKey,
                "Mary",
                "Mrs",
                0);
            PartnerKey = PersonRow.PartnerKey;

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create family and person record to be deleted");

            // check if Family record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Family record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of units works
        /// </summary>
        [Test]
        public void TestDeleteUnit()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow UnitPartnerRow;
            PUnitRow UnitRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            UnitPartnerRow = CreateNewUnitPartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            // check if Unit partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(UnitPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // set unit type to Key Ministry which means it is not allowed to be deleted any longer
            UnitRow = (PUnitRow)MainDS.PUnit.Rows[0];
            UnitRow.UnitTypeCode = "KEY-MIN";
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "set unit type to KEY-MIN");

            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(UnitPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of Unit partner
            UnitPartnerRow = CreateNewUnitPartner(MainDS, connector);
            PartnerKey = UnitPartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create unit record for deletion");

            // check if Unit record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Unit record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of churches works
        /// </summary>
        [Test]
        public void TestDeleteChurch()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow ChurchPartnerRow;
            PPartnerRow PartnerRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            ChurchPartnerRow = CreateNewChurchPartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create church record");

            // check if church partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(ChurchPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // create family partner and relationship to church partner
            PartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PPartnerRelationshipRow RelationshipRow = MainDS.PPartnerRelationship.NewRowTyped();

            RelationshipRow.PartnerKey = ChurchPartnerRow.PartnerKey;
            RelationshipRow.RelationName = "SUPPCHURCH";
            RelationshipRow.RelationKey = PartnerRow.PartnerKey;

            MainDS.PPartnerRelationship.Rows.Add(RelationshipRow);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "add relationship record to church record");

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            // now deletion must not be possible since relationship as SUPPCHURCH exists
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(ChurchPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of church partner
            ChurchPartnerRow = CreateNewChurchPartner(MainDS, connector);
            PartnerKey = ChurchPartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create church record for deletion");

            // check if church record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that church record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of organisations works
        /// </summary>
        [Test]
        public void TestDeleteOrganisation()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow OrganisationPartnerRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            OrganisationPartnerRow = CreateNewOrganisationPartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create organisation record");

            // check if organisation partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(OrganisationPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // now test actual deletion of Organisation partner
            PartnerKey = OrganisationPartnerRow.PartnerKey;
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Organisation record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of banks works
        /// </summary>
        [Test]
        public void TestDeleteBank()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow BankPartnerRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            BankPartnerRow = CreateNewBankPartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create bank record");

            // check if Bank partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(BankPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // set up details (e.g. bank account) for this Bank so deletion is not allowed
            PBankingDetailsTable BankingDetailsTable = new PBankingDetailsTable();
            PBankingDetailsRow BankingDetailsRow = BankingDetailsTable.NewRowTyped();
            BankingDetailsRow.BankKey = BankPartnerRow.PartnerKey;
            BankingDetailsRow.BankingType = 0;
            BankingDetailsRow.BankingDetailsKey = Convert.ToInt32(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_bank_details));
            BankingDetailsTable.Rows.Add(BankingDetailsRow);

            Assert.IsTrue(PBankingDetailsAccess.SubmitChanges(BankingDetailsTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult));

            // now deletion must not be possible since a bank account is set up for the bank
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(BankPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of venue partner
            BankPartnerRow = CreateNewBankPartner(MainDS, connector);
            PartnerKey = BankPartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create bank partner for deletion");

            // check if Venue record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Bank record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// check if deleting of venues works
        /// </summary>
        [Test]
        public void TestDeleteVenue()
        {
            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;
            String TextMessage;
            Boolean CanDeletePartner;
            PPartnerRow VenuePartnerRow;
            TSubmitChangesResult result;
            Int64 PartnerKey;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            VenuePartnerRow = CreateNewVenuePartner(MainDS, connector);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create venue record");

            // check if Venue partner can be deleted (still needs to be possible at this point)
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(VenuePartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(CanDeletePartner);

            // set up buildings for this venue so deletion is not allowed
            PcBuildingTable BuildingTable = new PcBuildingTable();
            PcBuildingRow BuildingRow = BuildingTable.NewRowTyped();
            BuildingRow.VenueKey = VenuePartnerRow.PartnerKey;
            BuildingRow.BuildingCode = "Test";
            BuildingTable.Rows.Add(BuildingRow);

            PcBuildingAccess.SubmitChanges(BuildingTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult);

            // now deletion must not be possible since a building is linked to the venue
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(VenuePartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of venue partner
            VenuePartnerRow = CreateNewVenuePartner(MainDS, connector);
            PartnerKey = VenuePartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create venue record for deletion");

            // check if Venue record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Venue record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }

        /// <summary>
        /// create two new unit partners and merge them
        /// </summary>
        [Test]
        public void TestMergeTwoUnits()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;
            long TestPartnerKey;

            // create two new Unit Partners
            PPartnerRow FromPartnerRow = CreateNewUnitPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewUnitPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PUnitRow FromUnitRow = (PUnitRow) MainDS.PUnit.Rows.Find( new object[] { FromPartnerKey });
            PUnitRow ToUnitRow = (PUnitRow) MainDS.PUnit.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromUnitRow.Maximum = 3;
            ToUnitRow.Maximum = 6;

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // create a record to test PUnit references are changed
            PPartnerRow TestPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            TestPartnerKey = TestPartnerRow.PartnerKey;
            PFamilyRow TestFamilyPartnerRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { TestPartnerKey });
            TestFamilyPartnerRow.FieldKey = FromPartnerKey;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.UNIT, TPartnerClass.UNIT, null, null, -1, Categories);

            Assert.AreEqual(true, result, "Merge two Unit partners");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PUnit.Merge(PUnitAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            TestPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { TestPartnerKey });
            FromUnitRow = (PUnitRow) MainDS.PUnit.Rows.Find(new object[] { FromPartnerKey });
            ToUnitRow = (PUnitRow) MainDS.PUnit.Rows.Find(new object[] { ToPartnerKey });
            TestFamilyPartnerRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { TestPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestUnit", ToPartnerRow.PartnerShortName, "merge two Units");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Units");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Units");
            Assert.AreEqual(9, ToUnitRow.Maximum, "merge two Units");
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Units");
            Assert.AreEqual(ToPartnerKey, TestFamilyPartnerRow.FieldKey, "merge two Units");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(TestPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new unit partners and merge them
        /// </summary>
        [Test]
        public void TestMergeTwoChurches()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create two new Church Partners
            PPartnerRow FromPartnerRow = CreateNewChurchPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewChurchPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PChurchRow FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find( new object[] { FromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromChurchRow.PrayerGroup = true;
            ToChurchRow.PrayerGroup = false;

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.CHURCH, TPartnerClass.CHURCH, null, null, -1, Categories);

            Assert.AreEqual(true, result, "Merge two Church partners");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { FromPartnerKey });
            ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge two Church partners");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Church partners");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Church partners");
            Assert.AreEqual(true, ToChurchRow.PrayerGroup, "merge two Church partners");
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Church partners");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new partners and merge them
        /// </summary>
        [Test]
        public void TestMergeChurchToOrganisation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create one new Church Partner and one new Organisation Partner
            PPartnerRow FromPartnerRow = CreateNewChurchPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewOrganisationPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PChurchRow FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find( new object[] { FromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromChurchRow.ContactPartnerKey = ToPartnerKey;
            ToOrganisationRow.ContactPartnerKey = 0;//.ToInt64(System.DBNull);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.CHURCH, TPartnerClass.ORGANISATION, null, null, -1, Categories);

            Assert.AreEqual(true, result, "merge Church to organisation");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { FromPartnerKey });
            ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge Church to organisation");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge Church to organisation");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge Church to organisation");
            Assert.AreEqual(FromChurchRow.ContactPartnerKey, ToOrganisationRow.ContactPartnerKey, "merge Church to organisation");
            Assert.IsNotNull(MergeTable.Rows[0], "merge Church to organisation");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new partners and merge them
        /// </summary>
        [Test]
        public void TestMergeChurchToFamily()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create one new Church Partner and one new Family Partner
            PPartnerRow FromPartnerRow = CreateNewChurchPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PChurchRow FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find( new object[] { FromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToFamilyRow.FamilyName = "";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.CHURCH, TPartnerClass.FAMILY, null, null, -1, Categories);

            Assert.AreEqual(true, result, "merge Church to Family");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { FromPartnerKey });
            ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge Church to Family");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge Church to Family");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge Church to Family");
            Assert.AreEqual(FromChurchRow.ChurchName, ToFamilyRow.FamilyName, "merge Church to Family");
            Assert.IsNotNull(MergeTable.Rows[0], "merge Church to Family");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new venue partners and merge them
        /// </summary>
        [Test]
        public void TestMergeTwoVenues()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create two new Venue Partners
            PPartnerRow FromPartnerRow = CreateNewVenuePartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewVenuePartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PVenueRow FromVenueRow = (PVenueRow) MainDS.PVenue.Rows.Find( new object[] { FromPartnerKey });
            PVenueRow ToVenueRow = (PVenueRow) MainDS.PVenue.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToVenueRow.VenueCode = "";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.VENUE, TPartnerClass.VENUE, null, null, -1, Categories);

            Assert.AreEqual(true, result, "merge two Venues");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PVenue.Merge(PVenueAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromVenueRow = (PVenueRow) MainDS.PVenue.Rows.Find(new object[] { FromPartnerKey });
            ToVenueRow = (PVenueRow) MainDS.PVenue.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestVenue", ToPartnerRow.PartnerShortName, "merge two Venues");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Venues");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Venues");
            Assert.AreEqual('V' + ToPartnerKey.ToString().Substring(1), ToVenueRow.VenueCode, "merge two Venues");
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Venues");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new family partners and merge them
        /// </summary>
        [Test]
        public void TestMergeTwoFamilies()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create two new Family Partners
            PPartnerRow FromPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PFamilyRow FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { FromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromFamilyRow.FirstName = "Chang";
            ToFamilyRow.FirstName = "Eng";
            ToFamilyRow.FamilyName = "";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.FAMILY, TPartnerClass.FAMILY, null, null, -1, Categories);

            Assert.AreEqual(true, result, "Merge two family partners");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { FromPartnerKey });
            ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge two families");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two families");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two families");
            Assert.AreEqual(FromFamilyRow.FamilyName, ToFamilyRow.FamilyName, "merge two families");
            Assert.IsNotNull(MergeTable.Rows[0], "merge two families");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new partners and merge them
        /// </summary>
        [Test]
        public void TestMergeFamilyToOrganisation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create one new Family Partner and one new Organisation Partner
            PPartnerRow FromPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewOrganisationPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PFamilyRow FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { FromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToOrganisationRow.OrganisationName = "";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.FAMILY, TPartnerClass.ORGANISATION, null, null, -1, Categories);

            Assert.AreEqual(true, result, "merge family to organisation");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { FromPartnerKey });
            ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge family to organisation");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge family to organisation");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge family to organisation");
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToOrganisationRow.OrganisationName, "merge family to organisation");
            Assert.IsNotNull(MergeTable.Rows[0], "merge family to organisation");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new person partners and merge them
        /// </summary>
        [Test]
        public void TestMergeFamilyToChurch()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FromPartnerKey;
            long ToPartnerKey;

            // create one new Family Partner and one new Church Partner
            PPartnerRow FromPartnerRow = CreateNewFamilyPartner(MainDS, connector);
            PPartnerRow ToPartnerRow = CreateNewChurchPartner(MainDS, connector);
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PFamilyRow FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { FromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToChurchRow.ChurchName = "";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.FAMILY, TPartnerClass.CHURCH, null, null, -1, Categories);

            Assert.AreEqual(true, result, "merge family to church");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { FromPartnerKey });
            ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge family to church");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge family to church");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge family to church");
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToChurchRow.ChurchName, "merge family to church");
            Assert.IsNotNull(MergeTable.Rows[0], "merge family to church");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }

        /// <summary>
        /// create two new Person partners from the same family and merge them
        /// </summary>
        [Test]
        public void TestMergeTwoPersonsFromSameFamily()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            long FamilyPartnerKey;
            long FromPartnerKey;
            long ToPartnerKey;

            // create two new Person Partners, one family and one location
            CreateFamilyWithPersonRecords(MainDS, connector);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            PLocationRow LocationRow = (PLocationRow) MainDS.PLocation.Rows[0];
            FamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            FromPartnerKey = FromPartnerRow.PartnerKey;
            ToPartnerKey = ToPartnerRow.PartnerKey;
            PFamilyRow FamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { FamilyPartnerKey });
            PPersonRow FromPersonRow = (PPersonRow) MainDS.PPerson.Rows.Find( new object[] { FromPartnerKey });
            PPersonRow ToPersonRow = (PPersonRow) MainDS.PPerson.Rows.Find( new object[] { ToPartnerKey });
            
            // modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToPersonRow.FirstName = "";
            FromPersonRow.Gender = "MALE";
            ToPersonRow.Gender = "UNKNOWN";

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            // submit new records to database
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            /****** Test that the merge does not throw any errors ******/
            
            long[] SiteKey = new long[1];
            int[] LocationKey = new int[1];
            SiteKey[0] = LocationRow.SiteKey;
            LocationKey[0] = LocationRow.LocationKey;
            
            // merge the two partners
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey,
                    TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKey, LocationKey, -1, Categories);

            Assert.AreEqual(true, result, "Merge two Person partners");

            /****** Test that the data is merged correctly ******/
            
            // read Partners from the databse after they have been merged
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(FromPartnerRow.PartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { FromPartnerKey });
            ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows.Find(new object[] { ToPartnerKey });
            FromPersonRow = (PPersonRow) MainDS.PPerson.Rows.Find(new object[] { FromPartnerKey });
            ToPersonRow = (PPersonRow) MainDS.PPerson.Rows.Find(new object[] { ToPartnerKey });
            
            // check partners have been merged correctly
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToPartnerRow.PartnerShortName, "merge two Persons");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual(FromPersonRow.Gender, ToPersonRow.Gender, "merge two Persons");
            Assert.AreEqual(FromPersonRow.FamilyName, ToPersonRow.FamilyName, "merge two Persons");
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Persons");
            
            // delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult); // TODO this fails because FamilyKey is null (I think)
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
            
            connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
        }
    }
}