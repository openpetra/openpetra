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
using System.Collections.Generic;
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
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.AP.UIConnectors;
using Ict.Petra.Server.MFinance.AP.WebConnectors;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.Interfaces.MPartner;

namespace Tests.MPartner.Server.PartnerMerge
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TPartnerMergeTest
    {
        private bool[] FCategories;
        private bool DifferentFamilies = false;
        
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
            
            FCategories = new bool[20];

            for (int i = 0; i < 20; i++)
            {
                FCategories[i] = true;
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

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class UNIT.
        /// </summary>
        /// <remarks>Creates two new Unit Partners, an auxilary Family Partner, merges the two Units 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoUnits()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long TestPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Unit Partners and a Family Partner
            //
            TestMergeTwoUnits_Arrange(out FromPartnerKey, out ToPartnerKey, out TestPartnerKey, 
                UIConnector);
            
            //
            // Act: Merge the two Unit Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.UNIT, TPartnerClass.UNIT, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Unit Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoUnits_SecondaryAsserts(FromPartnerKey, ToPartnerKey, TestPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(TestPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Unit Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Unit Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Unit Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ATestPartnerKey">Partner Key of the auxilary Family Partner.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoUnits_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long ATestPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Unit Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewUnitPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewUnitPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PUnitRow FromUnitRow = (PUnitRow)MainDS.PUnit.Rows.Find(new object[] { AFromPartnerKey });
            PUnitRow ToUnitRow = (PUnitRow)MainDS.PUnit.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromUnitRow, Is.Not.Null);
            Assert.That(ToUnitRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromUnitRow.Maximum = 3;
            ToUnitRow.Maximum = 6;
            
            // Submit the two new Unit Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Units failed: " + VerificationResult.BuildVerificationResultString());
            
            // Create a Family record to be able to test later that the FieldKey (which is referencing p_unit) is changed
            PPartnerRow TestPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            Assert.That(TestPartnerRow, Is.Not.Null);
            
            ATestPartnerKey = TestPartnerRow.PartnerKey;
            PFamilyRow TestFamilyPartnerRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { ATestPartnerKey });        
            Assert.That(TestFamilyPartnerRow, Is.Not.Null);
            
            TestFamilyPartnerRow.FieldKey = AFromPartnerKey;
            
            // Submit the new Family record to database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for Family failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Unit Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Unit Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ATestPartnerKey">Partner Key of the auxilary Family Partner.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoUnits_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, long ATestPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PUnit.Merge(PUnitAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PPartnerRow TestPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { ATestPartnerKey });
            PUnitRow FromUnitRow = (PUnitRow)MainDS.PUnit.Rows.Find(new object[] { AFromPartnerKey });
            PUnitRow ToUnitRow = (PUnitRow)MainDS.PUnit.Rows.Find(new object[] { AToPartnerKey });
            PFamilyRow TestFamilyPartnerRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { ATestPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(TestPartnerRow, Is.Not.Null);
            Assert.That(FromUnitRow, Is.Not.Null);
            Assert.That(ToUnitRow, Is.Not.Null);
            Assert.That(TestFamilyPartnerRow, Is.Not.Null);            
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Units
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestUnit", ToPartnerRow.PartnerShortName, "merge two Units");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Units");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Units");
            Assert.AreEqual(9, ToUnitRow.Maximum, "merge two Units");
            
            // Checking the Family
            Assert.AreEqual(AToPartnerKey, TestFamilyPartnerRow.FieldKey, "merge two Units");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Units");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class CHURCH.
        /// </summary>
        /// <remarks>Creates two new Church Partners, merges the two Churches 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoChurches()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Church Partners
            //
            TestMergeTwoChurches_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Church Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.CHURCH, TPartnerClass.CHURCH, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Church Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoChurches_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Church Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoChurches_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Church Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromChurchRow.PrayerGroup = true;
            ToChurchRow.PrayerGroup = false;
            
            // Submit the two new Church Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Churches failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoChurches_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Churches
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge two Church partners");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Church partners");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Church partners");
            Assert.AreEqual(true, ToChurchRow.PrayerGroup, "merge two Church partners");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Churches");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class CHURCH to a Partner of Partner Class ORGANISATION.
        /// </summary>
        /// <remarks>Creates one new Church Partner and one new Organisation Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeChurchToOrganisation()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Church Partners
            //
            TestMergeChurchToOrganisation_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Church Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.CHURCH, TPartnerClass.ORGANISATION, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Church to Organisation");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeChurchToOrganisation_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Church Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeChurchToOrganisation_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Church Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromChurchRow.ContactPartnerKey = AToPartnerKey;
            ToOrganisationRow.ContactPartnerKey = 0;
            
            // Submit the two new Church Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for church and organisation failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeChurchToOrganisation_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the church and the Organisation
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge Church to Organisation");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge Church to Organisation");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge Church to Organisation");
            Assert.AreEqual(FromChurchRow.ContactPartnerKey, ToOrganisationRow.ContactPartnerKey, "merge Church to Organisation");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Church to organisation");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class CHURCH to a Partner of Partner Class ORGANISATION.
        /// </summary>
        /// <remarks>Creates one new Church Partner and one new Family Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeChurchToFamily()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Church Partners
            //
            TestMergeChurchToFamily_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Church Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.CHURCH, TPartnerClass.FAMILY, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Church to Family");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeChurchToFamily_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Church Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeChurchToFamily_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Church Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToFamilyRow.FamilyName = "";
            
            // Submit the two new Church Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for church and family failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeChurchToFamily_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PChurchRow FromChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromChurchRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the church and the Family
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestChurch", ToPartnerRow.PartnerShortName, "merge Church to Family");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge Church to Family");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge Church to Family");
            Assert.AreEqual(FromChurchRow.ChurchName, ToFamilyRow.FamilyName, "merge Church to Family");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Church to Family");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class VENUE.
        /// </summary>
        /// <remarks>Creates two new Venue Partners, merges the two Venues 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoVenues()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Venue Partners
            //
            TestMergeTwoVenues_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Venue Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.VENUE, TPartnerClass.VENUE, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Venue Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoVenues_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Venue Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Venue Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Venue Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoVenues_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Venue Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewVenuePartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewVenuePartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PVenueRow FromVenueRow = (PVenueRow)MainDS.PVenue.Rows.Find(new object[] { AFromPartnerKey });
            PVenueRow ToVenueRow = (PVenueRow)MainDS.PVenue.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromVenueRow, Is.Not.Null);
            Assert.That(ToVenueRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToVenueRow.VenueCode = "";
            
            // Submit the two new Venue Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Venues failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Venue Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Venue Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoVenues_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PVenue.Merge(PVenueAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PVenueRow FromVenueRow = (PVenueRow)MainDS.PVenue.Rows.Find(new object[] { AFromPartnerKey });
            PVenueRow ToVenueRow = (PVenueRow)MainDS.PVenue.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromVenueRow, Is.Not.Null);
            Assert.That(ToVenueRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Venues
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestVenue", ToPartnerRow.PartnerShortName, "merge two Venues");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Venues");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Venues");
            Assert.AreEqual('V' + AToPartnerKey.ToString().Substring(1), ToVenueRow.VenueCode, "merge two Venues");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Venues");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class FAMILY.
        /// </summary>
        /// <remarks>Creates two new Family Partners, merges the two Families 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoFamilies()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Family Partners
            //
            TestMergeTwoFamilies_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Family Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.FAMILY, TPartnerClass.FAMILY, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Family Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoFamilies_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Family Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoFamilies_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Family Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromFamilyRow.FirstName = "Chang";
            ToFamilyRow.FirstName = "Eng";
            ToFamilyRow.FamilyName = "";
            
            // Submit the two new Family Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Families failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoFamilies_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Families
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge two families");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two families");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two families");
            Assert.AreEqual(FromFamilyRow.FamilyName, ToFamilyRow.FamilyName, "merge two families");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Families");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class FAMILY to a Partner of Partner Class ORGANISATION.
        /// </summary>
        /// <remarks>Creates one new Family Partner and one new Organisation Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeFamilyToOrganisation()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Family Partners
            //
            TestMergeFamilyToOrganisation_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Family Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.FAMILY, TPartnerClass.ORGANISATION, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Family to Organisation");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeFamilyToOrganisation_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Family Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeFamilyToOrganisation_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Family Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToOrganisationRow.OrganisationName = "";
            
            // Submit the two new Family Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for family and organisation failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeFamilyToOrganisation_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the family and the Organisation
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge family to organisation");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge family to organisation");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge family to organisation");
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToOrganisationRow.OrganisationName, "merge family to organisation");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Family to Organisation");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class FAMILY to a Partner of Partner Class CHURCH.
        /// </summary>
        /// <remarks>Creates one new Family Partner and one new Church Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeFamilyToChurch()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Family Partners
            //
            TestMergeFamilyToChurch_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Family Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.FAMILY, TPartnerClass.CHURCH, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Family to Church");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeFamilyToChurch_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Family Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeFamilyToChurch_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Family Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToChurchRow.ChurchName = "";
            
            // Submit the two new Family Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for family and church failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeFamilyToChurch_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the family and the Church
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestPartner, Mr", ToPartnerRow.PartnerShortName, "merge family to church");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge family to church");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge family to church");
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToChurchRow.ChurchName, "merge family to church");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Family to Church");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class PERSON and from the same Family.
        /// </summary>
        /// <remarks>Creates two new Person Partners and one new Family Partner, merges the two Persons 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoPersonsFromSameFamily()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long FamilyPartnerKey;
            long[] SiteKey;
            int[] LocationKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergeTwoPersonsFromSameFamily_Arrange(out FromPartnerKey, out ToPartnerKey, out FamilyPartnerKey, out SiteKey, out LocationKey, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKey, LocationKey, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoPersonsFromSameFamily_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFamilyPartnerKey">Partner Key of the Family Partner that is in the Partner Merge Test.</param>
        /// <param name="ASiteKey">Site Key of the Location that is in the Partner Merge Test.</param>
        /// <param name="ALocationKey">Location Key of the Location that is in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoPersonsFromSameFamily_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFamilyPartnerKey, 
            out long[] ASiteKey, out int[] ALocationKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // create two new Person Partners, one family and one location
            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            PLocationRow LocationRow = (PLocationRow) MainDS.PLocation.Rows[0];
            
            // Guard Assertions
            Assert.That(FamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(LocationRow, Is.Not.Null);
            
            AFamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PFamilyRow FamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { AFamilyPartnerKey });
            PPersonRow FromPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AFromPartnerKey });
            PPersonRow ToPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AToPartnerKey });
            
            // location data is needed for the merge
            ASiteKey = new long[1];
            ALocationKey = new int[1];
            ASiteKey[0] = LocationRow.SiteKey;
            ALocationKey[0] = LocationRow.LocationKey;

            // Guard Assertions
            Assert.That(FamilyRow, Is.Not.Null);
            Assert.That(FromPersonRow, Is.Not.Null);
            Assert.That(ToPersonRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToPersonRow.FirstName = "";
            FromPersonRow.Gender = "MALE";
            ToPersonRow.Gender = "UNKNOWN";
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons from same Family failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoPersonsFromSameFamily_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PPersonRow FromPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AFromPartnerKey });
            PPersonRow ToPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromPersonRow, Is.Not.Null);
            Assert.That(ToPersonRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToPartnerRow.PartnerShortName, "merge two Persons");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual(FromPersonRow.Gender, ToPersonRow.Gender, "merge two Persons");
            Assert.AreEqual(FromPersonRow.FamilyName, ToPersonRow.FamilyName, "merge two Persons");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Persons");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class PERSON and from different Families.
        /// </summary>
        /// <remarks>Creates two new Person Partners and two new Family Partners, merges the two Persons 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoPersonsFromDifferentFamilies()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long FromFamilyPartnerKey;
            long ToFamilyPartnerKey;
            long[] SiteKey;
            int[] LocationKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergeTwoPersonsFromDifferentFamilies_Arrange(out FromPartnerKey, out ToPartnerKey, out FromFamilyPartnerKey, out ToFamilyPartnerKey,
                out SiteKey, out LocationKey, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKey, LocationKey, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoPersonsFromDifferentFamilies_SecondaryAsserts(FromPartnerKey, ToPartnerKey, FromFamilyPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromFamilyPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToFamilyPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners and two Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFromFamilyPartnerKey">Partner Key of the From Family Partner that is in the Partner Merge Test.</param>
        /// <param name="AToFamilyPartnerKey">Partner Key of the To Family Partner that is in the Partner Merge Test.</param>
        /// <param name="ASiteKey">Site Key of the Location that is in the Partner Merge Test.</param>
        /// <param name="ALocationKey">Location Key of the Location that is in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoPersonsFromDifferentFamilies_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFromFamilyPartnerKey, 
            out long AToFamilyPartnerKey, out long[] ASiteKey, out int[] ALocationKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // create one new Person Partner, one family and one location
            TCreateTestPartnerData.CreateFamilyWithOnePersonRecord(MainDS);
            PPartnerRow FromFamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PLocationRow LocationRow = (PLocationRow) MainDS.PLocation.Rows[0];
            
            // Submit the two new Person Partner records to the database (need to this now or will get error when second location is created)
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // create one new Person Partner, one family and one location
            TCreateTestPartnerData.CreateFamilyWithOnePersonRecord(MainDS);
            PPartnerRow ToFamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[3];
            
            // Guard Assertions
            Assert.That(FromFamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToFamilyPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(LocationRow, Is.Not.Null);
            
            AFromFamilyPartnerKey = FromFamilyPartnerRow.PartnerKey;
            AToFamilyPartnerKey = ToFamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PFamilyRow FromFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { AFromFamilyPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find( new object[] { AToFamilyPartnerKey });
            PPersonRow FromPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AFromPartnerKey });
            PPersonRow ToPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AToPartnerKey });
            
            // location data is needed for the merge
            ASiteKey = new long[1];
            ALocationKey = new int[1];
            ASiteKey[0] = LocationRow.SiteKey;
            ALocationKey[0] = LocationRow.LocationKey;

            // Guard Assertions
            Assert.That(FromFamilyRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);
            Assert.That(FromPersonRow, Is.Not.Null);
            Assert.That(ToPersonRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToPersonRow.FirstName = "";
            FromPersonRow.Gender = "MALE";
            ToPersonRow.Gender = "UNKNOWN";
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons from different Families failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFromFamilyPartnerKey">Partner Key of the From Family Partner that is in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoPersonsFromDifferentFamilies_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, long AFromFamilyPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PPersonRow FromPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AFromPartnerKey });
            PPersonRow ToPersonRow = (PPersonRow)MainDS.PPerson.Rows.Find(new object[] { AToPartnerKey });
            PFamilyRow FromFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AFromFamilyPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromPersonRow, Is.Not.Null);
            Assert.That(ToPersonRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual(FromPartnerRow.PartnerShortName, ToPartnerRow.PartnerShortName, "merge two Persons");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge two Persons");
            Assert.AreEqual(FromPersonRow.Gender, ToPersonRow.Gender, "merge two Persons");
            Assert.AreNotEqual(FromPersonRow.FamilyName, ToPersonRow.FamilyName, "merge two Persons");
            Assert.IsTrue(FromPersonRow.IsFamilyKeyNull(), "merge two Persons");
            Assert.IsFalse(FromFamilyRow.FamilyMembers, "merge two Persons");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Persons");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class ORGANISATION.
        /// </summary>
        /// <remarks>Creates two new Organisation Partners, merges the two Organisations 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoOrganisations()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Organisation Partners
            //
            TestMergeTwoOrganisations_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Organisation Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.ORGANISATION, TPartnerClass.ORGANISATION, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Organisation Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoOrganisations_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Organisation Partners and a Organisation Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoOrganisations_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Organisation Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromOrganisationRow.Religious = true;
            
            // Submit the two new Organisation Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Organisations failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoOrganisations_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Organisations
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestOrganisation", ToPartnerRow.PartnerShortName, "merge two Organisations");
            Assert.IsTrue(FromOrganisationRow.Religious, "merge two Organisations");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Organisations");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class ORGANISATION to a Partner of Partner Class CHURCH.
        /// </summary>
        /// <remarks>Creates one new Organisation Partner and one new Church Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeOrganisationToChurch()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Organisation Partners
            //
            TestMergeOrganisationToChurch_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Organisation Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.ORGANISATION, TPartnerClass.CHURCH, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Organisation to Church");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeOrganisationToChurch_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Organisation Partners and a Organisation Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeOrganisationToChurch_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Organisation Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewChurchPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow)MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToChurchRow.ChurchName = "";
            
            // Submit the two new Organisation Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for organisation and church failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Church Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeOrganisationToChurch_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PChurch.Merge(PChurchAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PChurchRow ToChurchRow = (PChurchRow) MainDS.PChurch.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToChurchRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the organisation and the Church
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestOrganisation", ToPartnerRow.PartnerShortName, "merge organisation to church");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge organisation to church");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge organisation to church");
            Assert.AreEqual(FromOrganisationRow.OrganisationName, ToChurchRow.ChurchName, "merge organisation to church");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Organisation to Church");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class ORGANISATION to a Partner of Partner Class FAMILY.
        /// </summary>
        /// <remarks>Creates one new Organisation Partner and one new Family Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeOrganisationToFamily()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Organisation Partners
            //
            TestMergeOrganisationToFamily_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Organisation Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.ORGANISATION, TPartnerClass.FAMILY, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Organisation to Family");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeOrganisationToFamily_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Organisation Partners and a Organisation Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeOrganisationToFamily_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Organisation Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow)MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToFamilyRow.FamilyName = "";
            
            // Submit the two new Organisation Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for organisation and family failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Family Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeOrganisationToFamily_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PFamily.Merge(PFamilyAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PFamilyRow ToFamilyRow = (PFamilyRow) MainDS.PFamily.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToFamilyRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the organisation and the Family
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestOrganisation", ToPartnerRow.PartnerShortName, "merge organisation to family");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge organisation to family");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge organisation to family");
            Assert.AreEqual(FromOrganisationRow.OrganisationName, ToFamilyRow.FamilyName, "merge organisation to family");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Organisation to Family");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class ORGANISATION to a Partner of Partner Class BANK.
        /// </summary>
        /// <remarks>Creates one new Organisation Partner and one new Bank Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeOrganisationToBank()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Organisation Partners
            //
            TestMergeOrganisationToBank_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Organisation Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.ORGANISATION, TPartnerClass.BANK, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Organisation to Bank");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeOrganisationToBank_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Organisation Partners and a Organisation Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeOrganisationToBank_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Organisation Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PBankRow ToBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToBankRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToBankRow.BranchName = "";
            
            // Submit the two new Organisation Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for organisation and bank failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeOrganisationToBank_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PBank.Merge(PBankAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            POrganisationRow FromOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AFromPartnerKey });
            PBankRow ToBankRow = (PBankRow) MainDS.PBank.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromOrganisationRow, Is.Not.Null);
            Assert.That(ToBankRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the organisation and the Bank
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestOrganisation", ToPartnerRow.PartnerShortName, "merge organisation to bank");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge organisation to bank");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge organisation to bank");
            Assert.AreEqual(FromOrganisationRow.OrganisationName, ToBankRow.BranchName, "merge organisation to bank");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Organisation to Bank");
        }

        /// <summary>
        /// Tests Partner Merge, merging two Partners of Partner Class BANK.
        /// </summary>
        /// <remarks>Creates two new Bank Partners, merges the two Banks 
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeTwoBanks()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            int BankingDetailsKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Bank Partners
            //
            TestMergeTwoBanks_Arrange(out FromPartnerKey, out ToPartnerKey, out BankingDetailsKey, UIConnector);
            
            //
            // Act: Merge the two Bank Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.BANK, TPartnerClass.BANK, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Bank Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeTwoBanks_SecondaryAsserts(FromPartnerKey, ToPartnerKey, BankingDetailsKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Bank Partners and a Bank Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ABankingDetailsKey">BankingDetailsKey for the BankingDetails record being tested</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeTwoBanks_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out int ABankingDetailsKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Bank Partners and a new BankingDetails record
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
            PartnerEditTDSPBankingDetailsRow BankingDetailsRow = TCreateTestPartnerData.CreateNewBankingRecords(FromPartnerRow.PartnerKey, MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(BankingDetailsRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            ABankingDetailsKey = BankingDetailsRow.BankingDetailsKey;
            
            PBankRow FromBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AFromPartnerKey });
            PBankRow ToBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromBankRow, Is.Not.Null);
            Assert.That(ToBankRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            ToBankRow.BranchName = "";
            BankingDetailsRow.BankKey = AFromPartnerKey;
            
            // Submit the two new Bank Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Banks failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ABankingDetailsKey">BankingDetailsKey for the BankingDetails record being tested</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeTwoBanks_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, int ABankingDetailsKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PBank.Merge(PBankAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PBankingDetails.Merge(PBankingDetailsAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PBankRow FromBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AFromPartnerKey });
            PBankRow ToBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AToPartnerKey });
            PBankingDetailsRow BankingDetailsRow = (PBankingDetailsRow)MainDS.PBankingDetails.Rows.Find(new object[] { ABankingDetailsKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromBankRow, Is.Not.Null);
            Assert.That(ToBankRow, Is.Not.Null);
            Assert.That(BankingDetailsRow, Is.Not.Null);
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Banks
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestBank", ToPartnerRow.PartnerShortName, "merge two Banks");
            Assert.AreEqual(FromBankRow.BranchName, ToBankRow.BranchName, "merge two Banks");
            Assert.AreEqual(AToPartnerKey, BankingDetailsRow.BankKey, "merge two Banks");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge two Banks");
        }

        /// <summary>
        /// Tests Partner Merge, merging a Partner of Partner Class BANK to a Partner of Partner Class ORGANISATION.
        /// </summary>
        /// <remarks>Creates one new Bank Partner and one new Organisation Partner, merges the two Partners
        /// and checks that the merge did work.</remarks>
        [Test]
        public void TestMergeBankToOrganisation()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Bank Partners
            //
            TestMergeBankToOrganisation_Arrange(out FromPartnerKey, out ToPartnerKey, UIConnector);
            
            //
            // Act: Merge the two Bank Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.BANK, TPartnerClass.ORGANISATION, null, null, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging Bank to Organisation");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeBankToOrganisation_SecondaryAsserts(FromPartnerKey, ToPartnerKey, ref UIConnector);

            
            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Bank Partners and a Family Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeBankToOrganisation_Arrange(out long AFromPartnerKey, out long AToPartnerKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Create two new Bank Partners
            PPartnerRow FromPartnerRow = TCreateTestPartnerData.CreateNewBankPartner(MainDS);
            PPartnerRow ToPartnerRow = TCreateTestPartnerData.CreateNewOrganisationPartner(MainDS);
            
            // Guard Assertions
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            
            PBankRow FromBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow)MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });

            // Guard Assertions
            Assert.That(FromBankRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);
            
            // Modify records so that they contain different data
            ToPartnerRow.PartnerShortName = "";
            FromBankRow.ContactPartnerKey = AToPartnerKey;
            ToOrganisationRow.ContactPartnerKey = 0;
            
            // Submit the two new Bank Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for bank and organisation failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Bank Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Organisation Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeBankToOrganisation_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PBank.Merge(PBankAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.POrganisation.Merge(POrganisationAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PBankRow FromBankRow = (PBankRow)MainDS.PBank.Rows.Find(new object[] { AFromPartnerKey });
            POrganisationRow ToOrganisationRow = (POrganisationRow) MainDS.POrganisation.Rows.Find(new object[] { AToPartnerKey });
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(FromBankRow, Is.Not.Null);
            Assert.That(ToOrganisationRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the bank and the Organisation
            Assert.AreEqual(FromPartnerRow.PartnerKey.ToString() + ", TestBank", ToPartnerRow.PartnerShortName, "merge Bank to Organisation");
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge Bank to Organisation");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge Bank to Organisation");
            Assert.AreEqual(FromBankRow.ContactPartnerKey, ToOrganisationRow.ContactPartnerKey, "merge Bank to Organisation");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge Bank to organisation");
        }

        /// <summary>
        /// Tests Partner Merge, merging the Gift Info for a Person Partner.
        /// </summary>
        /// <remarks>Creates two new Person Partners and one new Family Partner, merges the two Persons 
        /// and checks that the Gift Info merge worked.</remarks>
        [Test]
        public void TestMergeGiftInfo()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long  FamilyPartnerKey;
            int LedgerNumber;
            int BatchNumber;
            long[] SiteKeys = new long[0];
            int[] LocationKeys = new int[0];
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergeGiftInfo_Arrange(out FromPartnerKey, out ToPartnerKey, out FamilyPartnerKey, out LedgerNumber, out BatchNumber, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKeys, LocationKeys, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeGiftInfo_SecondaryAsserts(FromPartnerKey, ToPartnerKey, LedgerNumber, BatchNumber, ref UIConnector);

            // Cleanup: Delete test records
            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.LoadGiftBatchData(LedgerNumber, BatchNumber);
            GiftDS.AGiftDetail.Rows[0].Delete();
            GiftDS.AGift.Rows[0].Delete();
            GiftDS.AGiftBatch.Rows[0].Delete();
            TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners, a Family Partner and Gift Info for the From Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFamilyPartnerKey">Partner Key of the Family Partner that is in the Partner Merge Test.</param>
        /// <param name="ALedgerNumber">Ledger Number for the GiftBatch that is created for testing.</param>
        /// <param name="ABatchNumber">Batch Number for the GiftBatch that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeGiftInfo_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFamilyPartnerKey, out int ALedgerNumber,
             out int ABatchNumber, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            GiftBatchTDS GiftDS = new GiftBatchTDS();
            
            // create two new Person Partners, one family and GiftInfo for From Partner
            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            AGiftBatchRow GiftBatchRow = TCreateTestPartnerData.CreateNewGiftInfo(FromPartnerRow.PartnerKey, ref GiftDS);
            
            // Guard Assertions
            Assert.That(FamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(GiftBatchRow, Is.Not.Null);
            Assert.AreEqual(1, GiftDS.AGift.Rows.Count);
            Assert.AreEqual(1, GiftDS.AGiftDetail.Rows.Count);
            
            AFamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            ALedgerNumber = GiftBatchRow.LedgerNumber;
            ABatchNumber = GiftBatchRow.BatchNumber;
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new Gift Info records to the database
            Result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for Gift Info failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ALedgerNumber">Ledger Number for the GiftBatch that is created for testing.</param>
        /// <param name="ABatchNumber">Batch Number for the GiftBatch that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeGiftInfo_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, int ALedgerNumber, int ABatchNumber, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            GiftBatchTDS GiftDS = new GiftBatchTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            GiftDS.AGift.Merge(AGiftAccess.LoadViaAGiftBatch(ALedgerNumber, ABatchNumber, DBAccess.GDBAccessObj.Transaction));
            GiftDS.AGiftDetail.Merge(AGiftDetailAccess.LoadViaAGiftBatch(ALedgerNumber, ABatchNumber, DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            AGiftRow GiftRow = (AGiftRow)GiftDS.AGift.Rows[0];
            AGiftDetailRow GiftDetailRow = (AGiftDetailRow)GiftDS.AGiftDetail.Rows[0];
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(GiftRow, Is.Not.Null);
            Assert.That(GiftDetailRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge gift info");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftRow.DonorKey, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftDetailRow.RecipientKey, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftDetailRow.RecipientLedgerNumber, "merge gift info");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge gift info");
        }

        /// <summary>
        /// Tests Partner Merge, merging the Gift Info for a Person Partner.
        /// </summary>
        /// <remarks>Creates two new Person Partners and one new Family Partner, merges the two Persons 
        /// and checks that the Gift Info merge worked.</remarks>
        [Test]
        public void TestMergeRecurringGiftInfo()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long  FamilyPartnerKey;
            int LedgerNumber;
            int BatchNumber;
            long[] SiteKeys = new long[0];
            int[] LocationKeys = new int[0];
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergeRecurringGiftInfo_Arrange(out FromPartnerKey, out ToPartnerKey, out FamilyPartnerKey, out LedgerNumber, out BatchNumber, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKeys, LocationKeys, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeRecurringGiftInfo_SecondaryAsserts(FromPartnerKey, ToPartnerKey, LedgerNumber, BatchNumber, ref UIConnector);

            // Cleanup: Delete test records
            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.LoadRecurringGiftBatchData(LedgerNumber, BatchNumber);
            GiftDS.ARecurringGiftDetail.Rows[0].Delete();
            GiftDS.ARecurringGift.Rows[0].Delete();
            GiftDS.ARecurringGiftBatch.Rows[0].Delete();
            TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners, a Family Partner and Gift Info for the From Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFamilyPartnerKey">Partner Key of the Family Partner that is in the Partner Merge Test.</param>
        /// <param name="ALedgerNumber">Ledger Number for the GiftBatch that is created for testing.</param>
        /// <param name="ABatchNumber">Batch Number for the GiftBatch that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeRecurringGiftInfo_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFamilyPartnerKey, out int ALedgerNumber,
             out int ABatchNumber, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            GiftBatchTDS GiftDS = new GiftBatchTDS();
            
            // create two new Person Partners, one family and GiftInfo for From Partner
            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            ARecurringGiftBatchRow GiftBatchRow = TCreateTestPartnerData.CreateNewRecurringGiftInfo(FromPartnerRow.PartnerKey, ref GiftDS);
            
            // Guard Assertions
            Assert.That(FamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(GiftBatchRow, Is.Not.Null);
            Assert.AreEqual(1, GiftDS.ARecurringGift.Rows.Count);
            Assert.AreEqual(1, GiftDS.ARecurringGiftDetail.Rows.Count);
            
            AFamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            ALedgerNumber = GiftBatchRow.LedgerNumber;
            ABatchNumber = GiftBatchRow.BatchNumber;
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new Gift Info records to the database
            Result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for Recurring Gift Info failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ALedgerNumber">Ledger Number for the GiftBatch that is created for testing.</param>
        /// <param name="ABatchNumber">Batch Number for the GiftBatch that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeRecurringGiftInfo_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, int ALedgerNumber, int ABatchNumber, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            GiftBatchTDS GiftDS = new GiftBatchTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            GiftDS.ARecurringGift.Merge(ARecurringGiftAccess.LoadViaARecurringGiftBatch(ALedgerNumber, ABatchNumber, DBAccess.GDBAccessObj.Transaction));
            GiftDS.ARecurringGiftDetail.Merge(ARecurringGiftDetailAccess.LoadViaARecurringGiftBatch(ALedgerNumber, ABatchNumber, DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            ARecurringGiftRow GiftRow = (ARecurringGiftRow)GiftDS.ARecurringGift.Rows[0];
            ARecurringGiftDetailRow GiftDetailRow = (ARecurringGiftDetailRow)GiftDS.ARecurringGiftDetail.Rows[0];
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(GiftRow, Is.Not.Null);
            Assert.That(GiftDetailRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge gift info");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftRow.DonorKey, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftDetailRow.RecipientKey, "merge gift info");
            Assert.AreEqual(AToPartnerKey, GiftDetailRow.RecipientLedgerNumber, "merge gift info");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge gift info");
        }

        /// <summary>
        /// Tests Partner Merge, merging the Gift Info for a Person Partner.
        /// </summary>
        /// <remarks>Creates two new Person Partners and one new Family Partner, merges the two Persons 
        /// and checks that the AP Info merge worked.</remarks>
        [Test]
        public void TestMergeAPInfo()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long  FamilyPartnerKey;
            int APDocumentID;
            int LedgerNumber;
            long[] SiteKeys = new long[0];
            int[] LocationKeys = new int[0];
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergeAPInfo_Arrange(out FromPartnerKey, out ToPartnerKey, out FamilyPartnerKey, out APDocumentID, out LedgerNumber, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKeys, LocationKeys, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergeAPInfo_SecondaryAsserts(FromPartnerKey, ToPartnerKey, APDocumentID, ref UIConnector);

            // Cleanup: Delete test records
            List<int> DocumentID = new List<int>();
            DocumentID.Add(APDocumentID);
            TAPTransactionWebConnector.DeleteAPDocuments(LedgerNumber, DocumentID, out VerificationResult);
            AccountsPayableTDS APDS = TAPTransactionWebConnector.LoadAApSupplier(LedgerNumber, ToPartnerKey);
            APDS.AApSupplier.Rows[0].Delete();
            AApSupplierAccess.SubmitChanges(APDS.AApSupplier, DBAccess.GDBAccessObj.Transaction, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners, a Family Partner and AP Info for the From Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFamilyPartnerKey">Partner Key of the Family Partner that is in the Partner Merge Test.</param>
        /// <param name="AAPDocumentID">Document ID for APDocument that is created for testing.</param>
        /// <param name="ALedgerNumber">Ledger Number for the GiftBatch that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergeAPInfo_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFamilyPartnerKey, out int AAPDocumentID,
             out int ALedgerNumber, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            AccountsPayableTDS APDS = new AccountsPayableTDS();
            
            // create two new Person Partners, one family and APInfo for From Partner
            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            AApDocumentRow APDocumentRow = TCreateTestPartnerData.CreateNewAPInfo(FromPartnerRow.PartnerKey, ref APDS);
            
            // Guard Assertions
            Assert.That(FamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(APDocumentRow, Is.Not.Null);
            Assert.AreEqual(1, APDS.AApSupplier.Rows.Count);
            
            AFamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            AAPDocumentID = APDocumentRow.ApDocumentId;
            ALedgerNumber = APDocumentRow.LedgerNumber;
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new Supplier record to the database
            TSupplierEditUIConnector Connector = new TSupplierEditUIConnector();
            Result = Connector.SubmitChanges(ref APDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for AP Info failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new Document record to the database
            Result = TAPTransactionWebConnector.SaveAApDocument(ref APDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for AP Info failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AAPDocumentID">Document ID for APDocument that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergeAPInfo_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, int AAPDocumentID, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            AccountsPayableTDS APDS = new AccountsPayableTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            APDS.AApSupplier.Merge(AApSupplierAccess.LoadViaPPartner(AToPartnerKey, DBAccess.GDBAccessObj.Transaction));
            APDS.AApDocument.Merge(AApDocumentAccess.LoadByPrimaryKey(AAPDocumentID, DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            AApSupplierRow SupplierRow = (AApSupplierRow)APDS.AApSupplier.Rows[0];
            AApDocumentRow DocumentRow = (AApDocumentRow)APDS.AApDocument.Rows[0];
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(SupplierRow, Is.Not.Null);
            Assert.That(DocumentRow, Is.Not.Null);        
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge AP info");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge AP info");
            Assert.AreEqual(AToPartnerKey, DocumentRow.PartnerKey, "AP gift info");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge AP info");
        }

        /// <summary>
        /// Tests Partner Merge, merging the PM Data for a Person Partner.
        /// </summary>
        /// <remarks>Creates two new Person Partners and one new Family Partner, merges the two Persons 
        /// and checks that the PM Data merge worked.</remarks>
        [Test]
        public void TestMergePMData()
        {
            long FromPartnerKey;
            long ToPartnerKey;
            long  FamilyPartnerKey;
            long[] SiteKeys = new long[0];
            int[] LocationKeys = new int[0];
            int DataLabelKey;
            TVerificationResultCollection VerificationResult;            
            TPartnerEditUIConnector UIConnector = new TPartnerEditUIConnector();

            //
            // Arrange: Create two Person Partners in one Family Partner with one Location
            //
            TestMergePMData_Arrange(out FromPartnerKey, out ToPartnerKey, out FamilyPartnerKey, out DataLabelKey, UIConnector);
            
            //
            // Act: Merge the two Person Partners!
            //
            bool result = TMergePartnersWebConnector.MergeTwoPartners(FromPartnerKey, ToPartnerKey,
                TPartnerClass.PERSON, TPartnerClass.PERSON, SiteKeys, LocationKeys, -1, FCategories, ref DifferentFamilies);
            
            // 
            // Assert
            //
            
            // Primary Assert: Tests that Partner Merge reports that it was successful! 
            Assert.AreEqual(true, result, "Merging two Person Partners");

            // Secondary Asserts: Test that the data was merged correctly!                      
            TestMergePMData_SecondaryAsserts(FromPartnerKey, ToPartnerKey, DataLabelKey, ref UIConnector);

            // Cleanup: Delete test records
            TPartnerWebConnector.DeletePartner(FromPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(ToPartnerKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FamilyPartnerKey, out VerificationResult);
            PDataLabelTable DataLabelTable = PDataLabelAccess.LoadByPrimaryKey(DataLabelKey, DBAccess.GDBAccessObj.Transaction);
            DataLabelTable.Rows[0].Delete();
            PDataLabelAccess.SubmitChanges(DataLabelTable, DBAccess.GDBAccessObj.Transaction, out VerificationResult);
        }

        /// <summary>
        /// Creates two Person Partners, a Family Partner and AP Info for the From Partner.
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AFamilyPartnerKey">Partner Key of the Family Partner that is in the Partner Merge Test.</param>
        /// <param name="ADataLabelKey">Key for PDataLabel that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        private void TestMergePMData_Arrange(out long AFromPartnerKey, out long AToPartnerKey, out long AFamilyPartnerKey, 
            out int ADataLabelKey, TPartnerEditUIConnector AConnector)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            PartnerEditTDS MainDS = new PartnerEditTDS();
            IndividualDataTDS IndividualDS = new IndividualDataTDS();
            
            // create two new Person Partners, one family and PM Data for both Partners
            TCreateTestPartnerData.CreateFamilyWithTwoPersonRecords(MainDS);
            PPartnerRow FamilyPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[0];
            PPartnerRow FromPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[1];
            PPartnerRow ToPartnerRow = (PPartnerRow) MainDS.PPartner.Rows[2];
            PDataLabelTable DataLabel = TCreateTestPartnerData.CreateNewPMData(FromPartnerRow.PartnerKey, ToPartnerRow.PartnerKey, IndividualDS);
            
            PmPassportDetailsRow row = (PmPassportDetailsRow) IndividualDS.PmPassportDetails.Rows[0];
            
            // Guard Assertions
            Assert.That(FamilyPartnerRow, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(DataLabel, Is.Not.Null);
            Assert.AreEqual(1, IndividualDS.PDataLabelValuePartner.Rows.Count);
            Assert.AreEqual(1, IndividualDS.PmPassportDetails.Rows.Count);
            Assert.AreEqual(2, IndividualDS.PmPersonalData.Rows.Count);
            
            AFamilyPartnerKey = FamilyPartnerRow.PartnerKey;
            AFromPartnerKey = FromPartnerRow.PartnerKey;
            AToPartnerKey = ToPartnerRow.PartnerKey;
            ADataLabelKey = ((PDataLabelRow) DataLabel.Rows[0]).Key;
            
            // Submit the two new Person Partner records to the database
            ResponseDS = new PartnerEditTDS();
            Result = AConnector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for two Persons failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new DataLabel record to the database
            bool BoolResult = PDataLabelAccess.SubmitChanges(DataLabel, DBAccess.GDBAccessObj.Transaction, out VerificationResult);
            
            // Guard Assertion
            Assert.IsTrue(BoolResult, "SubmitChanges for DataLabel failed: " + VerificationResult.BuildVerificationResultString());
            
            // Submit the new Document record to the database
            MainDS.Merge(IndividualDS);
            Result = TIndividualDataWebConnector.SubmitChangesServerSide(ref IndividualDS, ref MainDS, DBAccess.GDBAccessObj.Transaction, out VerificationResult);
            
            // Guard Assertion
            Assert.That(Result, Is.EqualTo(TSubmitChangesResult.scrOK), "SubmitChanges for PM Data failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// Test that the data was merged correctly!
        /// </summary>
        /// <param name="AFromPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="AToPartnerKey">Partner Key of the Person Partner that is the 'From' Partner in the Partner Merge Test.</param>
        /// <param name="ADataLabelKey">Key for PDataLabel that is created for testing.</param>
        /// <param name="AConnector">Instantiated Partner Edit UIConnector.</param>
        void TestMergePMData_SecondaryAsserts(long AFromPartnerKey, long AToPartnerKey, int ADataLabelKey, ref TPartnerEditUIConnector AConnector)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            IndividualDataTDS IndividualDS = new IndividualDataTDS();
            
            // Read Partners from the database after they have been merged            
            MainDS.PPartner.Merge(PPartnerAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            MainDS.PPerson.Merge(PPersonAccess.LoadAll(DBAccess.GDBAccessObj.Transaction));
            IndividualDS.PDataLabelValuePartner.Merge(PDataLabelValuePartnerAccess.LoadViaPDataLabel(ADataLabelKey, DBAccess.GDBAccessObj.Transaction));
            IndividualDS.PmPassportDetails.Merge(PmPassportDetailsAccess.LoadViaPPerson(AToPartnerKey, DBAccess.GDBAccessObj.Transaction));
            IndividualDS.PmPersonalData.Merge(PmPersonalDataAccess.LoadViaPPerson(AToPartnerKey, DBAccess.GDBAccessObj.Transaction));
            PPartnerMergeTable MergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AFromPartnerKey, DBAccess.GDBAccessObj.Transaction);
            
            PPartnerRow FromPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AFromPartnerKey });
            PPartnerRow ToPartnerRow = (PPartnerRow)MainDS.PPartner.Rows.Find(new object[] { AToPartnerKey });
            PDataLabelValuePartnerRow DataLabelValuePartnerRow = (PDataLabelValuePartnerRow)IndividualDS.PDataLabelValuePartner.Rows[0];
            PmPassportDetailsRow PassportDetailsRow = (PmPassportDetailsRow)IndividualDS.PmPassportDetails.Rows[0];
            PmPersonalDataRow PersonalDataRow = (PmPersonalDataRow)IndividualDS.PmPersonalData.Rows[0];
            
            // Check that what we are about to check is there...
            Assert.That(MergeTable, Is.Not.Null);
            Assert.That(FromPartnerRow, Is.Not.Null);
            Assert.That(ToPartnerRow, Is.Not.Null);
            Assert.That(DataLabelValuePartnerRow, Is.Not.Null);
            Assert.That(PassportDetailsRow, Is.Not.Null);
            Assert.That(PersonalDataRow, Is.Not.Null);
            
            //
            // Check that Partners have been merged correctly
            //
            
            // Checking the two Persons
            Assert.AreEqual("MERGED", FromPartnerRow.StatusCode, "merge PM data");
            Assert.AreEqual("ACTIVE", ToPartnerRow.StatusCode, "merge PM data");
            Assert.AreEqual(AToPartnerKey, DataLabelValuePartnerRow.PartnerKey, "merge PM data");
            Assert.AreEqual(175, PersonalDataRow.HeightCm, "merge PM data");
            Assert.AreEqual(95, PersonalDataRow.WeightKg, "merge PM data");
            Assert.IsTrue(PersonalDataRow.InternalDriverLicense, "merge PM data");
            Assert.IsTrue(PersonalDataRow.GenDriverLicense, "merge PM data");
            
            // Checking the MergeTable
            Assert.IsNotNull(MergeTable.Rows[0], "merge PM data");
        }
    }
}