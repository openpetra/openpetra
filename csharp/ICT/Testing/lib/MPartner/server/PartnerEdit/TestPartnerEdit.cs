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
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Tests.MPartner.shared.CreateTestPartnerData;
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
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
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
using Ict.Testing.NUnitTools;

namespace Tests.MPartner.Server.PartnerEdit
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerEditTest
    {
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
        /// create a new partner and save it with a new location
        /// </summary>
        [Test]
        public void TestSaveNewPartnerWithLocation()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);

            TCreateTestPartnerData.CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

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

            PPartnerRow PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();

            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = 0;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "Create a partner with location 0");

            TCreateTestPartnerData.CreateNewLocation(PartnerRow.PartnerKey, MainDS);

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

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

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

            PPartnerRow PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);

            TCreateTestPartnerData.CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "saving the first partner with a location");

            Int32 LocationKey = MainDS.PLocation[0].LocationKey;

            MainDS = new PartnerEditTDS();

            PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);

            PPartnerLocationRow PartnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = DomainManager.GSiteKey;
            PartnerLocationRow.PartnerKey = PartnerRow.PartnerKey;
            PartnerLocationRow.LocationKey = LocationKey;
            PartnerLocationRow.TelephoneNumber = PartnerRow.PartnerKey.ToString();
            MainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

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

            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

            // now change on partner location. should ask about everyone else
            // it seems, the change must be to PLocation. In Petra 2.3, changes to the PartnerLocation are not propagated
            // MainDS.PPartnerLocation[0].DateGoodUntil = new DateTime(2011, 01, 01);

            Assert.AreEqual(1, MainDS.PLocation.Rows.Count, "there should be only one address for the whole family");

            MainDS.PLocation[0].County = "different";

            ResponseDS = new PartnerEditTDS();

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

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

            FamilyPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
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
            TCreateTestPartnerData.CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create new location");

            PartnerEditTDS PersonDS = new PartnerEditTDS();
            PersonRow = TCreateTestPartnerData.CreateNewPerson(PersonDS, FamilyPartnerRow.PartnerKey,
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
            FamilyPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
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

                    AFrequencyAccess.SubmitChanges(FrequencyTable, DBAccess.GDBAccessObj.Transaction);
                }

                // now add the publication "TESTPUBLICATION"
                PPublicationRow PublicationRow = PublicationTable.NewRowTyped();
                PublicationRow.PublicationCode = "TESTPUBLICATION";
                PublicationRow.FrequencyCode = "Annual";
                PublicationTable.Rows.Add(PublicationRow);

                PPublicationAccess.SubmitChanges(PublicationTable, DBAccess.GDBAccessObj.Transaction);
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

                PReasonSubscriptionGivenAccess.SubmitChanges(ReasonTable, DBAccess.GDBAccessObj.Transaction);
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
            FamilyPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
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
            FamilyPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            TCreateTestPartnerData.CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            PersonRow = TCreateTestPartnerData.CreateNewPerson(MainDS,
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
            UnitPartnerRow = TCreateTestPartnerData.CreateNewUnitPartner(MainDS);
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
            PmStaffDataAccess.SubmitChanges(CommitmentTable, DBAccess.GDBAccessObj.Transaction);

            // this should now not be allowed since person record has a commitment linked to it
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(PersonRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of Person partner
            FamilyPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            TCreateTestPartnerData.CreateNewLocation(FamilyPartnerRow.PartnerKey, MainDS);
            PersonRow = TCreateTestPartnerData.CreateNewPerson(MainDS,
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

            UnitPartnerRow = TCreateTestPartnerData.CreateNewUnitPartner(MainDS);
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
            UnitRow.UnitTypeCode = MPartnerConstants.UNIT_TYPE_KEYMIN;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "set unit type to " + MPartnerConstants.UNIT_TYPE_KEYMIN);

            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(UnitPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of Unit partner
            UnitPartnerRow = TCreateTestPartnerData.CreateNewUnitPartner(MainDS);
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

            ChurchPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
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
            PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            PPartnerRelationshipRow RelationshipRow = MainDS.PPartnerRelationship.NewRowTyped();

            RelationshipRow.PartnerKey = ChurchPartnerRow.PartnerKey;
            RelationshipRow.RelationName = "SUPPCHURCH";
            RelationshipRow.RelationKey = PartnerRow.PartnerKey;

            MainDS.PPartnerRelationship.Rows.Add(RelationshipRow);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "add relationship record to church record");

            CommonNUnitFunctions.EnsureNullOrOnlyNonCriticalVerificationResults(VerificationResult,
                "There was a critical error when saving:");

            // now deletion must not be possible since relationship as SUPPCHURCH exists
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(ChurchPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of church partner
            ChurchPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
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

            OrganisationPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
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

            BankPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
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

            PBankingDetailsAccess.SubmitChanges(BankingDetailsTable, DBAccess.GDBAccessObj.Transaction);

            // now deletion must not be possible since a bank account is set up for the bank
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(BankPartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of venue partner
            BankPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
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

            VenuePartnerRow = TCreateTestPartnerData.CreateNewVenuePartner(MainDS);
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

            PcBuildingAccess.SubmitChanges(BuildingTable, DBAccess.GDBAccessObj.Transaction);

            // now deletion must not be possible since a building is linked to the venue
            CanDeletePartner = TPartnerWebConnector.CanPartnerBeDeleted(VenuePartnerRow.PartnerKey, out TextMessage);

            if (TextMessage.Length > 0)
            {
                TLogging.Log(TextMessage);
            }

            Assert.IsTrue(!CanDeletePartner);

            // now test actual deletion of venue partner
            VenuePartnerRow = TCreateTestPartnerData.CreateNewVenuePartner(MainDS);
            PartnerKey = VenuePartnerRow.PartnerKey;
            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "create venue record for deletion");

            // check if Venue record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult));

            // check that Venue record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(PartnerKey));
        }
    }
}