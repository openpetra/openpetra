//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, peters
//
// Copyright 2004-2015 by OM International
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

using NUnit.Framework;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;

using Tests.MPartner.shared.CreateTestPartnerData;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TGiftBatchTest
    {
        Int32 FLedgerNumber = -1;

        const string MainFeesPayableCode = "GIF2";
        const string MainFeesReceivableCode = "HO_ADMIN2";

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
            TLogging.Log("Ledger Number = " + FLedgerNumber);
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
        /// This will import a test gift batch, and post it.
        /// </summary>
        [Test]
        public void TestPostGiftBatch()
        {
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());
            FileContent = FileContent.Replace("{thisyear}", DateTime.Today.Year.ToString());

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult = null;
            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber;

            if (!importer.ImportGiftBatches(parameters, FileContent, out NeedRecipientLedgerNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not imported: " + VerificationResult.BuildVerificationResultString());
            }

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and returned a valid batch number");

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }
        }

        /// <summary>
        /// This will import a test gift batch, and check if the cost centre will be fixed according to the motivation detail
        /// </summary>
        [Test]
        public void TestPostGiftBatchWithMotivationDetailCostCentre()
        {
            // import a gift batch, that we will modify later
            TGiftImporting importer = new TGiftImporting();

            string testFile = TAppSettingsManager.GetValue("GiftBatch.file", "../../csharp/ICT/Testing/lib/MFinance/SampleData/sampleGiftBatch.csv");
            StreamReader sr = new StreamReader(testFile);
            string FileContent = sr.ReadToEnd();

            FileContent = FileContent.Replace("{ledgernumber}", FLedgerNumber.ToString());
            FileContent = FileContent.Replace("{thisyear}", DateTime.Today.Year.ToString());

            sr.Close();

            Hashtable parameters = new Hashtable();
            parameters.Add("Delimiter", ",");
            parameters.Add("ALedgerNumber", FLedgerNumber);
            parameters.Add("DateFormatString", "yyyy-MM-dd");
            parameters.Add("NumberFormat", "American");
            parameters.Add("NewLine", Environment.NewLine);

            TVerificationResultCollection VerificationResult = null;
            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber;

            if (!importer.ImportGiftBatches(parameters, FileContent, out NeedRecipientLedgerNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not imported: " + VerificationResult.BuildVerificationResultString());
            }

            int BatchNumber = importer.GetLastGiftBatchNumber();

            Assert.AreNotEqual(-1, BatchNumber, "Should have imported the gift batch and returned a valid batch number");

            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            // create a new costcentre
            const string newCostCentre = "100001";
            if (!ACostCentreAccess.Exists(FLedgerNumber, newCostCentre, Transaction))
            {
                ACostCentreTable cc = new ACostCentreTable();
                ACostCentreRow ccrow = cc.NewRowTyped();
                ccrow.LedgerNumber = FLedgerNumber;
                ccrow.CostCentreCode = newCostCentre;
                ccrow.CostCentreName = newCostCentre;
                ccrow.PostingCostCentreFlag = true;
                ccrow.CostCentreToReportTo = (FLedgerNumber*100).ToString("0000");
                ccrow.CostCentreType = MFinanceConstants.LOCAL_CC_TYPE;
                cc.Rows.Add(ccrow);
                ACostCentreAccess.SubmitChanges(cc, Transaction, UserInfo.GUserInfo.UserID);
            }

            // create a new motivation detail
            if (!AMotivationDetailAccess.Exists(FLedgerNumber, MFinanceConstants.MOTIVATION_GROUP_GIFT, newCostCentre, Transaction))
            {
                AMotivationDetailTable mot = new AMotivationDetailTable();
                AMotivationDetailRow motrow = mot.NewRowTyped();
                motrow.LedgerNumber = FLedgerNumber;
                motrow.MotivationGroupCode = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                motrow.MotivationDetailCode = newCostCentre;
                motrow.AccountCode = "0100";
                motrow.CostCentreCode = newCostCentre;
                motrow.MotivationDetailDesc = newCostCentre;
                mot.Rows.Add(motrow);
                AMotivationDetailAccess.SubmitChanges(mot, Transaction, UserInfo.GUserInfo.UserID);
            }

            // modify the gift batch with that motivation detail, but with wrong costcentre
            AGiftDetailTable giftdetails = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, BatchNumber, 1, 1, null, Transaction);
            giftdetails[0].MotivationDetailCode = newCostCentre;
            giftdetails[0].RecipientKey = 0;
            Assert.AreNotEqual(newCostCentre, giftdetails[0].CostCentreCode, "cost centre code should not match the one defined by the motivation detail");
            AGiftDetailAccess.SubmitChanges(giftdetails, Transaction);

            DBAccess.GDBAccessObj.CommitTransaction();

            if (!TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, BatchNumber, out VerificationResult))
            {
                Assert.Fail("Gift Batch was not posted: " + VerificationResult.BuildVerificationResultString());
            }

            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
            giftdetails = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, BatchNumber, 1, 1, null, Transaction);
            Assert.AreEqual(newCostCentre, giftdetails[0].CostCentreCode, "cost centre code should have been adjusted because of the motivation detail");
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// This will test the admin fee processer
        /// </summary>
        [Test]
        public void TestProcessAdminFees()
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            TVerificationResultCollection VerficationResults = null;

            AFeesPayableTable FeesPayableTable = null;
            AFeesReceivableTable FeesReceivableTable = null;

            try
            {
                AFeesPayableRow template = new AFeesPayableTable().NewRowTyped(false);

                template.LedgerNumber = FLedgerNumber;
                template.FeeCode = MainFeesPayableCode;

                FeesPayableTable = AFeesPayableAccess.LoadUsingTemplate(template, Transaction);

                AFeesReceivableRow template1 = new AFeesReceivableTable().NewRowTyped(false);

                template1.LedgerNumber = FLedgerNumber;
                template1.FeeCode = MainFeesReceivableCode;

                FeesReceivableTable = AFeesReceivableAccess.LoadUsingTemplate(template1, Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            if (FeesPayableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feespayable-data.sql", FLedgerNumber);
            }

            if (FeesReceivableTable.Count == 0)
            {
                CommonNUnitFunctions.LoadTestDataBase("csharp\\ICT\\Testing\\lib\\MFinance\\GL\\" +
                    "test-sql\\gl-test-feesreceivable-data.sql", FLedgerNumber);
            }

            GiftBatchTDS MainDS = new GiftBatchTDS();

            //Reset
            NewTransaction = false;
            Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                AFeesPayableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
                AFeesReceivableAccess.LoadViaALedger(MainDS, FLedgerNumber, Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            //TODO If this first one works, try different permatations for Assert.AreEqual
            // Test also for exception handling
            Assert.AreEqual(12m, TGiftTransactionWebConnector.CalculateAdminFee(MainDS,
                    FLedgerNumber,
                    MainFeesPayableCode,
                    100m,
                    out VerficationResults), "admin fee fixed 12% of 100 expect 12");
        }

        /// <summary>
        /// This will test the admin fee processer
        /// </summary>
        [Test]
        public void TestGetRecipientFundNumber()
        {
            Int64 PartnerKey = 73000000;
            Int64 RecipientLedgerNumber = 0;

            //bool NewTransaction = false;

            //TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                RecipientLedgerNumber = TGiftTransactionWebConnector.GetRecipientFundNumber(PartnerKey);
            }
            catch (Exception)
            {
                throw;
            }
            //finally
            //{
            //if (NewTransaction)
            //{
            //    DBAccess.GDBAccessObj.RollbackTransaction();
            //}
            //}

            //TODO the value to check for needs to be updated oncw workwer field is implemented.
            //TODO If this first one works, try different permatations for Assert.AreEqual
            // Test also for exception handling
            Assert.AreEqual(73000000, RecipientLedgerNumber,
                String.Format("Expected Recipient Ledger Number: {0} but got {1}", 73000000, RecipientLedgerNumber));
        }

        /// <summary>
        /// This will test that the correct Recipient Field, Cost Centre and Account are used for a gift when posting a batch.
        /// Two gifts are tested. One positive and one negative. Only the positive gift should be updated.
        /// The Negative gift should be unchanged.
        /// </summary>
        [Test]
        public void TestBatchPostingRecalculations()
        {
            TVerificationResultCollection VerificationResult;

            Int64 RecipientKey;
            Int64 RealRecipientLedgerNumber;
            Int64 FalseRecipientLedgerNumber;
            const string REALCOSTCENTRECODE = "4300";
            const string FALSECOSTCENTRECODE = "3500";
            const string ACCOUNTCODE = "0100";

            Int32 GiftBatchNumber;

            //
            // Arrange: Create all data needed for this test (Gift Details have 'fake' RecipientLedgerNumber and CostCentreCode)
            //
            TestBatchPostingRecalculations_Arrange(out RecipientKey, out RealRecipientLedgerNumber,
                out FalseRecipientLedgerNumber, REALCOSTCENTRECODE, FALSECOSTCENTRECODE, out GiftBatchNumber);

            //
            // Act: Post the batch
            //
            bool result = TGiftTransactionWebConnector.PostGiftBatch(FLedgerNumber, GiftBatchNumber, out VerificationResult);

            //
            // Assert
            //

            // Initial Assert: Tests that the post returns positive
            Assert.AreEqual(true,
                result,
                "TestBatchPostingRecalculations fail: Posting GiftBatch failed: " + VerificationResult.BuildVerificationResultString());

            // Primary Assert: Chaeck that the gifts have the correct RecipientLedgerNumber, CostCentreCode and Account
            TDBTransaction Transaction = null;
            AGiftDetailRow PositiveGiftDetailRow = null;
            AGiftDetailRow NegativeGiftDetailRow = null;
            ATransactionRow TransactionRow = null;
            Int32 GLBatchNumber = -1;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PositiveGiftDetailRow = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, GiftBatchNumber, 1, 1, Transaction)[0];
                    NegativeGiftDetailRow = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, GiftBatchNumber, 2, 1, Transaction)[0];

                    GLBatchNumber = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, Transaction)[0].LastBatchNumber;
                    TransactionRow = ATransactionAccess.LoadByPrimaryKey(FLedgerNumber, GLBatchNumber, 1, 1, Transaction)[0];
                });

            Assert.IsNotNull(PositiveGiftDetailRow, "TestBatchPostingRecalculations fail: Obtaining PositiveGiftDetailRow from database failed");
            Assert.IsNotNull(NegativeGiftDetailRow, "TestBatchPostingRecalculations fail: Obtaining NegativeGiftDetailRow from database failed");
            Assert.IsNotNull(TransactionRow, "TestBatchPostingRecalculations fail: Obtaining Transaction from database failed");

            Assert.AreEqual(RealRecipientLedgerNumber, PositiveGiftDetailRow.RecipientLedgerNumber,
                "TestBatchPostingRecalculations fail: RecipientLedgerNumber for PositiveGiftDetailRow is incorrect");
            Assert.AreEqual(FalseRecipientLedgerNumber, NegativeGiftDetailRow.RecipientLedgerNumber,
                "TestBatchPostingRecalculations fail: RecipientLedgerNumber for NegativeGiftDetailRow is incorrect");
            Assert.AreEqual(REALCOSTCENTRECODE, PositiveGiftDetailRow.CostCentreCode,
                "TestBatchPostingRecalculations fail: CostCentreCode for PositiveGiftDetailRow is incorrect");
            Assert.AreEqual(FALSECOSTCENTRECODE, NegativeGiftDetailRow.CostCentreCode,
                "TestBatchPostingRecalculations fail: CostCentreCode for NegativeGiftDetailRow is incorrect");
            Assert.AreEqual(ACCOUNTCODE, TransactionRow.AccountCode,
                "TestBatchPostingRecalculations fail: AccountCode for PositiveGiftDetailRow is incorrect");

            // Cleanup: Delete test records

            bool SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    AProcessedFeeAccess.DeleteUsingTemplate(
                        new TSearchCriteria[] { new TSearchCriteria("a_ledger_number_i", FLedgerNumber),
                                                new TSearchCriteria("a_batch_number_i", GiftBatchNumber) },
                        Transaction);

                    AGiftDetailAccess.DeleteRow(AGiftDetailTable.TableId, PositiveGiftDetailRow, Transaction);
                    AGiftDetailAccess.DeleteRow(AGiftDetailTable.TableId, NegativeGiftDetailRow, Transaction);
                });

            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, RealRecipientLedgerNumber, out PartnerCostCentreTbl);
            PartnerCostCentreTbl.Rows[0]["IsLinked"] = '0';
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            TPartnerWebConnector.DeletePartner(RecipientKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(RealRecipientLedgerNumber, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FalseRecipientLedgerNumber, out VerificationResult);
        }

        /// <summary>
        /// This will test that the correct Recipient Field and Cost Centre are used for a gift when loading a batch
        /// </summary>
        [Test]
        public void TestBatchLoadingRecalculations()
        {
            TVerificationResultCollection VerificationResult;

            Int64 RecipientKey;
            Int64 RealRecipientLedgerNumber;
            Int64 FalseRecipientLedgerNumber;
            const string REALCOSTCENTRECODE = "4300";
            const string FALSECOSTCENTRECODE = "3500";
            Int32 GiftBatchNumber;

            //
            // Arrange: Create all data needed for this test (Gift Detail has a 'fake' RecipientLedgerNumber and CostCentreCode)
            //
            TestBatchPostingRecalculations_Arrange(out RecipientKey, out RealRecipientLedgerNumber,
                out FalseRecipientLedgerNumber, REALCOSTCENTRECODE, FALSECOSTCENTRECODE, out GiftBatchNumber);

            //
            // Act: Load the batch
            //
            GiftBatchTDS GiftBatchDS = TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData(FLedgerNumber, GiftBatchNumber);

            //
            // Assert
            //

            // Initial Assert: Tests that the load returns results
            Assert.IsNotNull(GiftBatchDS, "TestBatchLoadingRecalculations fail: Loading GiftBatch failed");
            Assert.IsNotNull(GiftBatchDS.AGiftDetail, "TestBatchLoadingRecalculations fail: Loading GiftBatch failed");

            // Primary Assert: Chaeck that the gift has the correct RecipientLedgerNumber and CostCentreCode
            TDBTransaction Transaction = null;
            AGiftDetailRow PositiveGiftDetailRow = null;
            AGiftDetailRow NegativeGiftDetailRow = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    PositiveGiftDetailRow = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, GiftBatchNumber, 1, 1, Transaction)[0];
                    NegativeGiftDetailRow = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, GiftBatchNumber, 2, 1, Transaction)[0];
                });

            Assert.IsNotNull(PositiveGiftDetailRow, "TestBatchPostingRecalculations fail: Obtaining PositiveGiftDetailRow from database failed");
            Assert.IsNotNull(NegativeGiftDetailRow, "TestBatchPostingRecalculations fail: Obtaining NegativeGiftDetailRow from database failed");

            Assert.AreEqual(RealRecipientLedgerNumber, PositiveGiftDetailRow.RecipientLedgerNumber,
                "TestBatchPostingRecalculations fail: RecipientLedgerNumber for PositiveGiftDetailRow is incorrect");
            Assert.AreEqual(FalseRecipientLedgerNumber, NegativeGiftDetailRow.RecipientLedgerNumber,
                "TestBatchPostingRecalculations fail: RecipientLedgerNumber for NegativeGiftDetailRow is incorrect");
            Assert.AreEqual(REALCOSTCENTRECODE, PositiveGiftDetailRow.CostCentreCode,
                "TestBatchPostingRecalculations fail: CostCentreCode for PositiveGiftDetailRow is incorrect");
            Assert.AreEqual(FALSECOSTCENTRECODE, NegativeGiftDetailRow.CostCentreCode,
                "TestBatchPostingRecalculations fail: CostCentreCode for NegativeGiftDetailRow is incorrect");

            // Cleanup: Delete test records

            bool SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    AGiftDetailAccess.DeleteRow(AGiftDetailTable.TableId, PositiveGiftDetailRow, Transaction);
                    AGiftDetailAccess.DeleteRow(AGiftDetailTable.TableId, NegativeGiftDetailRow, Transaction);
                });

            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, RealRecipientLedgerNumber, out PartnerCostCentreTbl);
            PartnerCostCentreTbl.Rows[0]["IsLinked"] = '0';
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            TPartnerWebConnector.DeletePartner(RecipientKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(RealRecipientLedgerNumber, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FalseRecipientLedgerNumber, out VerificationResult);
        }

        /// <summary>
        /// Creates data needed to test posting recalculations
        /// </summary>
        /// <param name="ARecipientKey">Partner Key of the recipient.</param>
        /// <param name="ARealRecipientLedgerNumber">What the RecipientLedgerNumber should be.</param>
        /// <param name="AFalseRecipientLedgerNumber">What the RecipientLedgerNumber is.</param>
        /// <param name="ARealCostCentreCode">What the CostCentreCode should be.</param>
        /// <param name="AFalseCostCentreCode">What the CostCentreCode is.</param>
        /// <param name="AGiftBatchNumber">Batch Number.</param>
        private void TestBatchPostingRecalculations_Arrange(out long ARecipientKey,
            out long ARealRecipientLedgerNumber,
            out long AFalseRecipientLedgerNumber,
            string ARealCostCentreCode,
            string AFalseCostCentreCode,
            out Int32 AGiftBatchNumber)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            TPartnerEditUIConnector PartnerEditUIUIConnector = new TPartnerEditUIConnector();

            GiftBatchTDS MainDS = new GiftBatchTDS();
            PartnerEditTDS PartnerEditDS = new PartnerEditTDS();

            // this is a family partner in the test database
            const Int64 DONORKEY = 43005001;

            // create a new recipient
            TCreateTestPartnerData.CreateNewFamilyPartner(PartnerEditDS);
            ARecipientKey = PartnerEditDS.PFamily[0].PartnerKey;

            // create two new Unit partners
            TCreateTestPartnerData.CreateNewUnitPartner(PartnerEditDS);
            TCreateTestPartnerData.CreateNewUnitPartner(PartnerEditDS);
            AFalseRecipientLedgerNumber = PartnerEditDS.PPartner[0].PartnerKey;
            ARealRecipientLedgerNumber = PartnerEditDS.PPartner[1].PartnerKey;

            PPartnerTypeRow PartnerTypeRow = PartnerEditDS.PPartnerType.NewRowTyped();
            PartnerTypeRow.PartnerKey = ARealRecipientLedgerNumber;
            PartnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_COSTCENTRE;
            PartnerEditDS.PPartnerType.Rows.Add(PartnerTypeRow);

            // create a Gift Destination for family
            PPartnerGiftDestinationRow GiftDestination = PartnerEditDS.PPartnerGiftDestination.NewRowTyped(true);

            GiftDestination.Key = TPartnerDataReaderWebConnector.GetNewKeyForPartnerGiftDestination();
            GiftDestination.PartnerKey = ARecipientKey;
            GiftDestination.DateEffective = new DateTime(2011, 01, 01);
            GiftDestination.FieldKey = ARealRecipientLedgerNumber;

            PartnerEditDS.PPartnerGiftDestination.Rows.Add(GiftDestination);

            // Guard Assertions
            Assert.That(PartnerEditDS.PFamily[0], Is.Not.Null);
            Assert.That(PartnerEditDS.PPartner[0], Is.Not.Null);
            Assert.That(PartnerEditDS.PPartner[1], Is.Not.Null);

            // Submit the new PartnerEditTDS records to the database
            ResponseDS = new PartnerEditTDS();
            Result = PartnerEditUIUIConnector.SubmitChanges(ref PartnerEditDS, ref ResponseDS, out VerificationResult);

            // Guard Assertion
            Assert.That(Result, Is.EqualTo(
                    TSubmitChangesResult.scrOK), "SubmitChanges for PartnerEditDS failed: " + VerificationResult.BuildVerificationResultString());

            // link unit to Cost Centre
            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, 0, out PartnerCostCentreTbl);
            DataRow PartnerCostCentreRow = PartnerCostCentreTbl.NewRow();
            PartnerCostCentreRow["PartnerKey"] = ARealRecipientLedgerNumber;
            PartnerCostCentreRow["IsLinked"] = ARealCostCentreCode;
            PartnerCostCentreTbl.Rows.Add(PartnerCostCentreRow);
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            // create a new Gift Batch
            MainDS = TGiftTransactionWebConnector.CreateAGiftBatch(FLedgerNumber);
            AGiftBatchNumber = MainDS.AGiftBatch[0].BatchNumber;

            // create two new gifts
            AGiftRow GiftRow = MainDS.AGift.NewRowTyped(true);

            GiftRow.LedgerNumber = FLedgerNumber;
            GiftRow.BatchNumber = AGiftBatchNumber;
            GiftRow.DonorKey = DONORKEY;
            GiftRow.GiftTransactionNumber = 1;
            GiftRow.LastDetailNumber = 1;

            MainDS.AGift.Rows.Add(GiftRow);

            MainDS.AGiftBatch[0].LastGiftNumber++;

            GiftRow = MainDS.AGift.NewRowTyped(true);

            GiftRow.LedgerNumber = FLedgerNumber;
            GiftRow.BatchNumber = AGiftBatchNumber;
            GiftRow.DonorKey = DONORKEY;
            GiftRow.GiftTransactionNumber = 2;
            GiftRow.LastDetailNumber = 1;

            MainDS.AGift.Rows.Add(GiftRow);

            MainDS.AGiftBatch[0].LastGiftNumber++;

            // create a new GiftDetail with a positive amount
            AGiftDetailRow GiftDetail = MainDS.AGiftDetail.NewRowTyped(true);

            GiftDetail.LedgerNumber = FLedgerNumber;
            GiftDetail.BatchNumber = AGiftBatchNumber;
            GiftDetail.GiftTransactionNumber = 1;
            GiftDetail.DetailNumber = 1;
            GiftDetail.RecipientLedgerNumber = AFalseRecipientLedgerNumber;
            GiftDetail.GiftAmount = 100;
            GiftDetail.MotivationGroupCode = "GIFT";
            GiftDetail.MotivationDetailCode = "SUPPORT";
            GiftDetail.RecipientKey = ARecipientKey;
            GiftDetail.CostCentreCode = AFalseCostCentreCode;
            GiftDetail.GiftTransactionAmount = 100;

            MainDS.AGiftDetail.Rows.Add(GiftDetail);

            // create a new GiftDetail with a negative amount
            GiftDetail = MainDS.AGiftDetail.NewRowTyped(true);

            GiftDetail.LedgerNumber = FLedgerNumber;
            GiftDetail.BatchNumber = AGiftBatchNumber;
            GiftDetail.GiftTransactionNumber = 2;
            GiftDetail.DetailNumber = 1;
            GiftDetail.RecipientLedgerNumber = AFalseRecipientLedgerNumber;
            GiftDetail.GiftAmount = -100;
            GiftDetail.MotivationGroupCode = "GIFT";
            GiftDetail.MotivationDetailCode = "SUPPORT";
            GiftDetail.RecipientKey = ARecipientKey;
            GiftDetail.CostCentreCode = AFalseCostCentreCode;
            GiftDetail.GiftTransactionAmount = -100;

            MainDS.AGiftDetail.Rows.Add(GiftDetail);

            // Submit the new GiftBatchTDS records to the database
            Result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref MainDS, out VerificationResult);

            // Guard Assertion
            Assert.That(Result, Is.EqualTo(
                    TSubmitChangesResult.scrOK), "SaveGiftBatchTDS failed: " + VerificationResult.BuildVerificationResultString());
        }

        /// <summary>
        /// This will test that the correct Recipient Field is used for a gift when submitting a recurring batch.
        /// </summary>
        [Test]
        public void TestRecurringBatchSubmitRecalculations()
        {
            TVerificationResultCollection VerificationResult;

            Int64 RecipientKey;
            Int64 RealRecipientLedgerNumber;
            Int64 FalseRecipientLedgerNumber;
            Int32 RecurringGiftBatchNumber;
            Int32 GiftBatchNumber = -1;

            //
            // Arrange: Create all data needed for this test (Recurring Gift Detail will have 'fake' RecipientLedgerNumber)
            //
            TestRecurringBatchSubmitRecalculations_Arrange(out RecipientKey, out RealRecipientLedgerNumber,
                out FalseRecipientLedgerNumber, out RecurringGiftBatchNumber);

            //
            // Act: Submit the batch
            //

            Hashtable requestParams = new Hashtable();
            requestParams.Add("ALedgerNumber", FLedgerNumber);
            requestParams.Add("ABatchNumber", RecurringGiftBatchNumber);
            requestParams.Add("AEffectiveDate", DateTime.Today);
            requestParams.Add("AExchangeRateToBase", (decimal)1);
            requestParams.Add("AExchangeRateIntlToBase", (decimal)1);

            int NewBatchNo = 0;
            TGiftTransactionWebConnector.SubmitRecurringGiftBatch(requestParams, out NewBatchNo);

            //
            // Assert
            //

            // Primary Assert: Chaeck that the RecurringGiftDetail and the newly created GiftDetail have the correct RecipientLedgerNumber
            TDBTransaction Transaction = null;
            ARecurringGiftDetailRow RecurringGiftDetailRow = null;
            AGiftDetailRow GiftDetailRow = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    RecurringGiftDetailRow =
                        ARecurringGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, RecurringGiftBatchNumber, 1, 1, Transaction)[0];

                    GiftBatchNumber = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, Transaction)[0].LastGiftBatchNumber;
                    GiftDetailRow = AGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, GiftBatchNumber, 1, 1, Transaction)[0];
                });

            Assert.IsNotNull(
                RecurringGiftDetailRow, "TestRecurringBatchSubmitRecalculations fail: Obtaining RecurringGiftDetailRow from database failed");
            Assert.IsNotNull(
                GiftDetailRow, "TestRecurringBatchSubmitRecalculations fail: Obtaining GiftDetailRow from database failed");

            Assert.AreEqual(RealRecipientLedgerNumber, RecurringGiftDetailRow.RecipientLedgerNumber,
                "TestRecurringBatchSubmitRecalculations fail: RecipientLedgerNumber for RecurringGiftDetailRow is incorrect");
            Assert.AreEqual(RealRecipientLedgerNumber, GiftDetailRow.RecipientLedgerNumber,
                "TestRecurringBatchSubmitRecalculations fail: RecipientLedgerNumber for GiftDetailRow is incorrect");

            // Cleanup: Delete test records

            bool SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    AGiftDetailAccess.DeleteRow(AGiftDetailTable.TableId, GiftDetailRow, Transaction);
                    ARecurringGiftDetailAccess.DeleteRow(ARecurringGiftDetailTable.TableId, RecurringGiftDetailRow, Transaction);
                });

            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, RealRecipientLedgerNumber, out PartnerCostCentreTbl);
            PartnerCostCentreTbl.Rows[0]["IsLinked"] = '0';
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            TPartnerWebConnector.DeletePartner(RecipientKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(RealRecipientLedgerNumber, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FalseRecipientLedgerNumber, out VerificationResult);
        }

        /// <summary>
        /// This will test that the correct Recipient Field is used for a recurring gift when loading a recurring batch
        /// </summary>
        [Test]
        public void TestRecurringBatchLoadingRecalculations()
        {
            TVerificationResultCollection VerificationResult;

            Int64 RecipientKey;
            Int64 RealRecipientLedgerNumber;
            Int64 FalseRecipientLedgerNumber;
            Int32 RecurringGiftBatchNumber;

            //
            // Arrange: Create all data needed for this test (Recurring Gift Detail will have 'fake' RecipientLedgerNumber)
            //
            TestRecurringBatchSubmitRecalculations_Arrange(out RecipientKey, out RealRecipientLedgerNumber,
                out FalseRecipientLedgerNumber, out RecurringGiftBatchNumber);

            //
            // Act: Load the batch
            //
            GiftBatchTDS GiftBatchDS = TGiftTransactionWebConnector.LoadARecurringGiftBatchAndRelatedData(FLedgerNumber, RecurringGiftBatchNumber);

            //
            // Assert
            //

            // Initial Assert: Tests that the load returns results
            Assert.IsNotNull(GiftBatchDS, "TestRecurringBatchLoadingRecalculations fail: Loading GiftBatch failed");
            Assert.IsNotNull(GiftBatchDS.ARecurringGiftDetail, "TestRecurringBatchLoadingRecalculations fail: Loading GiftBatch failed");

            // Primary Assert: Chaeck that the gift has the correct RecipientLedgerNumber
            TDBTransaction Transaction = null;
            ARecurringGiftDetailRow RecurringGiftDetailRow = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    RecurringGiftDetailRow =
                        ARecurringGiftDetailAccess.LoadByPrimaryKey(FLedgerNumber, RecurringGiftBatchNumber, 1, 1, Transaction)[0];
                });

            Assert.IsNotNull(
                RecurringGiftDetailRow, "TestRecurringBatchLoadingRecalculations fail: Obtaining RecurringGiftDetailRow from database failed");

            Assert.AreEqual(RealRecipientLedgerNumber, RecurringGiftDetailRow.RecipientLedgerNumber,
                "TestRecurringBatchLoadingRecalculations fail: RecipientLedgerNumber for RecurringGiftDetailRow is incorrect");

            // Cleanup: Delete test records

            bool SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    ARecurringGiftDetailAccess.DeleteRow(ARecurringGiftDetailTable.TableId, RecurringGiftDetailRow, Transaction);
                });

            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, RealRecipientLedgerNumber, out PartnerCostCentreTbl);
            PartnerCostCentreTbl.Rows[0]["IsLinked"] = '0';
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            TPartnerWebConnector.DeletePartner(RecipientKey, out VerificationResult);
            TPartnerWebConnector.DeletePartner(RealRecipientLedgerNumber, out VerificationResult);
            TPartnerWebConnector.DeletePartner(FalseRecipientLedgerNumber, out VerificationResult);
        }

        /// <summary>
        /// Creates data needed to test posting recalculations
        /// </summary>
        /// <param name="ARecipientKey">Partner Key of the recipient.</param>
        /// <param name="ARealRecipientLedgerNumber">What the RecipientLedgerNumber should be.</param>
        /// <param name="AFalseRecipientLedgerNumber">What the RecipientLedgerNumber is.</param>
        /// <param name="ARecurringGiftBatchNumber">Batch Number.</param>
        private void TestRecurringBatchSubmitRecalculations_Arrange(out long ARecipientKey,
            out long ARealRecipientLedgerNumber,
            out long AFalseRecipientLedgerNumber,
            out Int32 ARecurringGiftBatchNumber)
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult Result;
            DataSet ResponseDS;
            TPartnerEditUIConnector PartnerEditUIUIConnector = new TPartnerEditUIConnector();

            GiftBatchTDS MainDS = new GiftBatchTDS();
            PartnerEditTDS PartnerEditDS = new PartnerEditTDS();

            // this is a family partner in the test database
            const Int64 DONORKEY = 43005001;


            // create a new recipient
            TCreateTestPartnerData.CreateNewFamilyPartner(PartnerEditDS);
            ARecipientKey = PartnerEditDS.PFamily[0].PartnerKey;

            // create two new Unit partners
            TCreateTestPartnerData.CreateNewUnitPartner(PartnerEditDS);
            TCreateTestPartnerData.CreateNewUnitPartner(PartnerEditDS);
            AFalseRecipientLedgerNumber = PartnerEditDS.PPartner[0].PartnerKey;
            ARealRecipientLedgerNumber = PartnerEditDS.PPartner[1].PartnerKey;

            PPartnerTypeRow PartnerTypeRow = PartnerEditDS.PPartnerType.NewRowTyped();
            PartnerTypeRow.PartnerKey = ARealRecipientLedgerNumber;
            PartnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_COSTCENTRE;
            PartnerEditDS.PPartnerType.Rows.Add(PartnerTypeRow);

            // create a Gift Destination for family
            PPartnerGiftDestinationRow GiftDestination = PartnerEditDS.PPartnerGiftDestination.NewRowTyped(true);

            GiftDestination.Key = TPartnerDataReaderWebConnector.GetNewKeyForPartnerGiftDestination();
            GiftDestination.PartnerKey = ARecipientKey;
            GiftDestination.DateEffective = new DateTime(2011, 01, 01);
            GiftDestination.FieldKey = ARealRecipientLedgerNumber;

            PartnerEditDS.PPartnerGiftDestination.Rows.Add(GiftDestination);

            // Guard Assertions
            Assert.That(PartnerEditDS.PFamily[0], Is.Not.Null);
            Assert.That(PartnerEditDS.PPartner[0], Is.Not.Null);
            Assert.That(PartnerEditDS.PPartner[1], Is.Not.Null);

            // Submit the new PartnerEditTDS records to the database
            ResponseDS = new PartnerEditTDS();
            Result = PartnerEditUIUIConnector.SubmitChanges(ref PartnerEditDS, ref ResponseDS, out VerificationResult);

            // Guard Assertion
            Assert.That(Result, Is.EqualTo(
                    TSubmitChangesResult.scrOK), "SubmitChanges for PartnerEditDS failed: " + VerificationResult.BuildVerificationResultString());

            // link unit to Cost Centre
            DataTable PartnerCostCentreTbl = null;
            TGLSetupWebConnector.LoadCostCentrePartnerLinks(FLedgerNumber, 0, out PartnerCostCentreTbl);
            DataRow PartnerCostCentreRow = PartnerCostCentreTbl.NewRow();
            PartnerCostCentreRow["PartnerKey"] = ARealRecipientLedgerNumber;
            PartnerCostCentreRow["IsLinked"] = 4300;
            PartnerCostCentreTbl.Rows.Add(PartnerCostCentreRow);
            TGLSetupWebConnector.SaveCostCentrePartnerLinks(FLedgerNumber, PartnerCostCentreTbl);

            // create a new Recurring Gift Batch
            MainDS = TGiftTransactionWebConnector.CreateARecurringGiftBatch(FLedgerNumber);
            ARecurringGiftBatchNumber = MainDS.ARecurringGiftBatch[0].BatchNumber;

            // create a new recurring gifts
            ARecurringGiftRow RecurringGiftRow = MainDS.ARecurringGift.NewRowTyped(true);

            RecurringGiftRow.LedgerNumber = FLedgerNumber;
            RecurringGiftRow.BatchNumber = ARecurringGiftBatchNumber;
            RecurringGiftRow.DonorKey = DONORKEY;
            RecurringGiftRow.GiftTransactionNumber = 1;
            RecurringGiftRow.LastDetailNumber = 1;

            MainDS.ARecurringGift.Rows.Add(RecurringGiftRow);

            // create a new RecurringGiftDetail
            ARecurringGiftDetailRow RecurringGiftDetail = MainDS.ARecurringGiftDetail.NewRowTyped(true);

            RecurringGiftDetail.LedgerNumber = FLedgerNumber;
            RecurringGiftDetail.BatchNumber = ARecurringGiftBatchNumber;
            RecurringGiftDetail.GiftTransactionNumber = 1;
            RecurringGiftDetail.DetailNumber = 1;
            RecurringGiftDetail.RecipientLedgerNumber = AFalseRecipientLedgerNumber;
            RecurringGiftDetail.GiftAmount = 100;
            RecurringGiftDetail.MotivationGroupCode = "GIFT";
            RecurringGiftDetail.MotivationDetailCode = "SUPPORT";
            RecurringGiftDetail.RecipientKey = ARecipientKey;

            MainDS.ARecurringGiftDetail.Rows.Add(RecurringGiftDetail);

            // Submit the new GiftBatchTDS records to the database
            Result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref MainDS, out VerificationResult);

            // Guard Assertion
            Assert.That(Result, Is.EqualTo(
                    TSubmitChangesResult.scrOK), "SaveGiftBatchTDS failed: " + VerificationResult.BuildVerificationResultString());
        }

        #region Test Argument Validation

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.PostGiftBatch
        /// </summary>
        [Test]
        public void TestPostGiftBatchArgumentValidation()
        {
            TVerificationResultCollection VerificationResult = null;

            string Message = "Validation failed for posting a Gift Batch with ledger number less than 1.";

            // Post a Gift Batch with ledger number less than 1
            try
            {
                TGiftTransactionWebConnector.PostGiftBatch(-1, 1, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidLedgerNumberException e)
            {
                Assert.AreEqual(-1, e.LedgerNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for posting a Gift Batch with batch number less than 1.";

            // Post a Gift Batch with batch number less than 1
            try
            {
                TGiftTransactionWebConnector.PostGiftBatch(1, -1, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidBatchNumberException e)
            {
                Assert.AreEqual(1, e.LedgerNumber, Message);
                Assert.AreEqual(-1, e.BatchNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.PostGiftBatches
        /// </summary>
        [Test]
        public void TestPostGiftBatchesArgumentValidation()
        {
            TVerificationResultCollection VerificationResult = null;

            List <Int32>BatchNumbers = new List <int>();

            string Message = "Validation failed for posting a Gift Batch with ledger number less than 1.";

            // Post a Gift Batch with ledger number less than 1
            try
            {
                TGiftTransactionWebConnector.PostGiftBatches(-1, BatchNumbers, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidLedgerNumberException e)
            {
                Assert.AreEqual(-1, e.LedgerNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for posting a Gift Batch with batch number less than 1.";
            BatchNumbers.Add(-1);

            // Post a Gift Batch with batch number less than 1
            try
            {
                TGiftTransactionWebConnector.PostGiftBatches(1, BatchNumbers, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidBatchNumberException e)
            {
                Assert.AreEqual(1, e.LedgerNumber, Message);
                Assert.AreEqual(-1, e.BatchNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.PrepareGiftBatchForPosting
        /// </summary>
        [Test]
        public void TestPrepareGiftBatchForPostingArgumentValidation()
        {
            TVerificationResultCollection VerificationResult = null;
            TDBTransaction Transaction = null;

            string Message = "Validation failed for PrepareGiftBatchForPosting with ledger number less than 1.";

            // Prepare Gift Batch For Posting with ledger number less than 1
            try
            {
                TGiftTransactionWebConnector.PrepareGiftBatchForPosting(-1, 1, ref Transaction, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidLedgerNumberException e)
            {
                Assert.AreEqual(-1, e.LedgerNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for PrepareGiftBatchForPosting with batch number less than 1.";

            // Prepare Gift Batch For Posting with batch number less than 1
            try
            {
                TGiftTransactionWebConnector.PrepareGiftBatchForPosting(1, -1, ref Transaction, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidBatchNumberException e)
            {
                Assert.AreEqual(1, e.LedgerNumber, Message);
                Assert.AreEqual(-1, e.BatchNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for PrepareGiftBatchForPosting with null transaction.";

            // Prepare Gift Batch For Posting with null transaction
            try
            {
                TGiftTransactionWebConnector.PrepareGiftBatchForPosting(1, 1, ref Transaction, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemDBTransactionNullException e)
            {
                Assert.AreEqual("Function:Prepare Gift Batch For Posting - Database Transaction must not be NULL!", e.Message,
                    Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData
        /// </summary>
        [Test]
        public void TestLoadAGiftBatchAndRelatedDataArgumentValidation()
        {
            bool ChangesToCommit;
            TDBTransaction Transaction = null;

            // Load Gift Batch with ledger number less than 1
            string Message = "Validation failed for LoadAGiftBatchAndRelatedData with ledger number less than 1.";

            try
            {
                TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData(-1, 1, Transaction, out ChangesToCommit);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidLedgerNumberException e)
            {
                Assert.AreEqual(-1, e.LedgerNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for LoadAGiftBatchAndRelatedData with batch number less than 1.";

            // Load Gift Batch with batch number less than 1
            try
            {
                TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData(1, -1, Transaction, out ChangesToCommit);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidBatchNumberException e)
            {
                Assert.AreEqual(1, e.LedgerNumber, Message);
                Assert.AreEqual(-1, e.BatchNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for LoadAGiftBatchAndRelatedData with null transaction.";

            // Load Gift Batch with null transaction
            try
            {
                TGiftTransactionWebConnector.LoadAGiftBatchAndRelatedData(1, 1, Transaction, out ChangesToCommit);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemDBTransactionNullException e)
            {
                Assert.AreEqual("Function:Load A Gift Batch And Related Data - Database Transaction must not be NULL!", e.Message,
                    Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.CalculateAdminFee
        /// </summary>
        [Test]
        public void TestCalculateAdminFeeArgumentValidation()
        {
            TVerificationResultCollection VerificationResult = null;

            string Message = "Validation failed for CalculateAdminFee with null dataset.";

            // Calculate admin fee with null dataset
            try
            {
                TGiftTransactionWebConnector.CalculateAdminFee(null, -1, "", 0, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemDataObjectNullOrEmptyException e)
            {
                Assert.AreEqual("Function:Calculate Admin Fee - The Gift Batch dataset is null!", e.Message, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            GiftBatchTDS MainDS = new GiftBatchTDS();
            Message = "Validation failed for CalculateAdminFee with ledger number less than 1.";

            // Calculate admin fee with ledger number less than 1
            try
            {
                TGiftTransactionWebConnector.CalculateAdminFee(MainDS, -1, "", 0, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemInvalidLedgerNumberException e)
            {
                Assert.AreEqual(-1, e.LedgerNumber, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            Message = "Validation failed for CalculateAdminFee with blank fee code.";

            // Calculate admin fee with blank admin fee
            try
            {
                TGiftTransactionWebConnector.CalculateAdminFee(MainDS, 1, "", 0, out VerificationResult);
                Assert.Fail(Message);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Function:Calculate Admin Fee - The Fee code is empty!", e.Message, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        /// <summary>
        /// This will test argument validation for TGiftTransactionWebConnector.AddToFeeTotals
        /// </summary>
        [Test]
        public void TestAddToFeeTotalsArgumentValidation()
        {
            string Message = "Validation failed for AddToFeeTotals with null dataset.";

            // AddToFeeTotals with null dataset
            try
            {
                TGiftTransactionWebConnector.AddToFeeTotals(FLedgerNumber, null, null, "", 0, 0);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemDataObjectNullOrEmptyException e)
            {
                Assert.AreEqual("Function:Add To Fee Totals - The Gift Batch dataset is null!", e.Message, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            GiftBatchTDS MainDS = new GiftBatchTDS();
            Message = "Validation failed for AddToFeeTotals with null datarow.";

            // AddToFeeTotals with null datarow
            try
            {
                TGiftTransactionWebConnector.AddToFeeTotals(FLedgerNumber, MainDS, null, "", 0, 0);
                Assert.Fail(Message);
            }
            catch (EFinanceSystemDataObjectNullOrEmptyException e)
            {
                Assert.AreEqual("Function:Add To Fee Totals - The Gift Detail row is null!", e.Message, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }

            AGiftDetailRow GiftDetailRow = MainDS.AGiftDetail.NewRowTyped();
            Message = "Validation failed for AddToFeeTotals with blank fee code.";

            // AddToFeeTotals with blank admin fee
            try
            {
                TGiftTransactionWebConnector.AddToFeeTotals(FLedgerNumber, MainDS, GiftDetailRow, "", 0, 0);
                Assert.Fail(Message);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Function:Add To Fee Totals - The Fee code is empty!", e.Message, Message);
            }
            catch
            {
                Assert.Fail(Message);
            }
        }

        #endregion
    }
}