//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Testing.NUnitPetraServer;
using NUnit.Framework;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MPartner;
using Tests.MPartner.shared.CreateTestPartnerData;


namespace Tests.MFinance.Server.Gift
{
    /// The Webconnector class TSetMotivationGroupAndDetail is tested
    [TestFixture]
    public class SetMotivationGroupAndDetailTest
    {
        const string SMTH = "GIFT"; // "SMTH"; // Means Something
        const string KMIN = "KEYMIN"; // Special Result ...
        const string SUPT = "SUPPORT"; // Special Result ...

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// <summary>
        /// ...
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// Test for the value 0 -> There exist no partner with that ID.
        /// </summary>

        [Test]
        public void Test_NullPartner()
        {
            Int64 partnerKey = 0;
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = KMIN;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsFalse(partnerKeyIsValid, "Check if partnerKey=0 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(KMIN, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        /// Test for the value 1234567 -> There exist no partner with that ID.
        /// </summary>

        [Test]
        public void Test_InvalidPartner()
        {
            Int64 partnerKey = 1234567;
            bool partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = KMIN;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsFalse(partnerKeyIsValid, "Check if partnerKey=1234567 does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(KMIN, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Person()
        {
            Int64 partnerKey = CreateNewPartnerKey();
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = SUPT;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, "Check if partnerKey=" + partnerKey.ToString() + "  does not exist");
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(SUPT, motivationDetail, "motivationDetail must not be changed");
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Unit_WithoutKeyMin()
        {
            Int64 partnerKey = CreateNewPartnerKeyWithUnit();
            Boolean partnerKeyIsValid;
            String motivationGroup = SMTH;
            String motivationDetail = "Incorrect";

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, String.Format("PartnerKey {0} was created but does not exist!", partnerKey));
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(KMIN, motivationDetail, "motivationDetail must be changed to " + KMIN);

            DeletePartnerKeyWithUnit(partnerKey);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_Unit_WithKeyMin()
        {
            Int64 partnerKey = CreateNewPartnerKeyWithUnit();
            Boolean partnerKeyIsValid;

            String motivationGroup = SMTH;
            String motivationDetail = KMIN;

            partnerKeyIsValid = TGuiTools.GetMotivationGroupAndDetail(
                partnerKey, ref motivationGroup, ref motivationDetail);

            Assert.IsTrue(partnerKeyIsValid, String.Format("PartnerKey {0} was created but does not exist!", partnerKey));
            Assert.AreEqual(SMTH, motivationGroup, "motivationGroup must not be changed");
            Assert.AreEqual(KMIN, motivationDetail, "motivationDetail should not change from " + KMIN);

            DeletePartnerKeyWithUnit(partnerKey);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_GetRecipientLedger()
        {
            Int64 partnerKey = CreateNewPartnerKey();

            long RecipLedgerNumber = TGiftTransactionWebConnector.GetRecipientFundNumber(partnerKey);

            Assert.IsTrue((RecipLedgerNumber != 0), String.Format("PartnerKey {0} has a recipient ledger number of 0!", partnerKey));
        }

        /// <summary>
        ///
        /// </summary>
        //[Test] - TODO - reinstate once worker field is sorted
        private void Test_ZRecipientLedgerEqualsLedgerPartner()
        {
            Int64 partnerKey = CreateNewPartnerKeyWithUnit();
            Int64 ledgerPartnerKey = GetLedgerPartnerKey(43);

            long RecipLedgerNumber = TGiftTransactionWebConnector.GetRecipientFundNumber(partnerKey);

            Assert.AreEqual(RecipLedgerNumber, ledgerPartnerKey,
                String.Format("Expected RecipientLedgerNumber ({0}) to equal Ledger PartnerKey ({1})", RecipLedgerNumber, ledgerPartnerKey));

            DeletePartnerKeyWithUnit(partnerKey);
        }

        /// <summary>
        ///
        /// </summary>
        //[Test] - TODO - reinstate once worker field is sorted
        private void Test_ZCheckCostCentreLinkForRecipient()
        {
            bool Success = false;
            string CostCentreCode = string.Empty;
            Int64 partnerKey = CreateNewPartnerKeyWithUnit();

            Success = TGiftTransactionWebConnector.CheckCostCentreLinkForRecipient(43, partnerKey, out CostCentreCode);

            Assert.IsTrue(Success,
                String.Format("Invalid Ledger number exists for PartnerKey ({0}), returning Cost Centre {1}", partnerKey, CostCentreCode));

            DeletePartnerKeyWithUnit(partnerKey);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void Test_KeyMinistryExists()
        {
            bool KeyMinActive = false;
            bool Success = false;

            Int64 partnerKey = CreateNewPartnerKeyWithUnit();

            Success = TGiftTransactionWebConnector.KeyMinistryExists(partnerKey, out KeyMinActive);

            Assert.IsTrue(Success, String.Format("PartnerKey {0} has has no key ministry!", partnerKey));

            if (Success)
            {
                Assert.IsTrue(KeyMinActive, String.Format("PartnerKey {0} has inactive key ministry!", partnerKey));
            }

            DeletePartnerKeyWithUnit(partnerKey);
        }

        private Int64 CreateNewPartnerKey()
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result;
            DataSet ResponseDS = new PartnerEditTDS();
            Int64 retVal = 0;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow partnerRow = TCreateTestPartnerData.CreateNewPartner(MainDS);

            if (partnerRow != null)
            {
                result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

                if (result == TSubmitChangesResult.scrOK)
                {
                    retVal = partnerRow.PartnerKey;
                }
            }

            return retVal;
        }

        private Int64 CreateNewPartnerKeyWithUnit()
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result;
            DataSet ResponseDS = new PartnerEditTDS();
            Int64 retVal = 0;

            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow UnitPartnerRow = TCreateTestPartnerData.CreateNewUnitPartnerWithTypeCode(MainDS, "KEY-MIN");

            if (UnitPartnerRow != null)
            {
                result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

                if (result == TSubmitChangesResult.scrOK)
                {
                    retVal = UnitPartnerRow.PartnerKey;
                }
            }

            return retVal;
        }

        private void DeletePartnerKeyWithUnit(Int64 APartnerKey)
        {
            TVerificationResultCollection VerificationResult;

            // check if Unit record is being deleted
            Assert.IsTrue(TPartnerWebConnector.DeletePartner(APartnerKey,
                    out VerificationResult), "Error deleting partner-key-with-unit " + APartnerKey.ToString());

            // check that Unit record is really deleted
            Assert.IsTrue(!TPartnerServerLookups.VerifyPartner(APartnerKey),
                "Error. Partner-key-with-unit " + APartnerKey.ToString() + " still exists!");
        }

        private Int64 GetLedgerPartnerKey(Int32 ALedgerNumber)
        {
            Int64 retVal = 0;
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                GiftBatchTDS MainDS = new GiftBatchTDS();

                ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                if (MainDS.ALedger.Count > 0)
                {
                    retVal = MainDS.ALedger[0].PartnerKey;
                }
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

            return retVal;
        }
    }
}