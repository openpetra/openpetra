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
using Ict.Testing.NUnitPetraServer;
using Tests.MPartner.shared.CreateTestPartnerData;
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
    public class TPartnerEditBankingDetailsTest
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
        /// create a new partner and play around with banking details
        /// </summary>
        [Test]
        public void TestBankingDetails()
        {
            TPartnerEditUIConnector connector = new TPartnerEditUIConnector();

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow PartnerRow = TCreateTestPartnerData.CreateNewFamilyPartner(MainDS);

            TCreateTestPartnerData.CreateNewLocation(PartnerRow.PartnerKey, MainDS);

            DataSet ResponseDS = new PartnerEditTDS();
            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving. Please check the logs");
            }

            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "TPartnerEditUIConnector SubmitChanges return value");

            connector = new TPartnerEditUIConnector(PartnerRow.PartnerKey);

            // add a banking detail
            PartnerEditTDSPBankingDetailsRow bankingDetailsRow = MainDS.PBankingDetails.NewRowTyped(true);
            bankingDetailsRow.AccountName = "account of " + PartnerRow.PartnerShortName;
            bankingDetailsRow.BankAccountNumber = new Random().Next().ToString();
            bankingDetailsRow.BankingDetailsKey = (MainDS.PBankingDetails.Count + 1) * -1;
            bankingDetailsRow.BankKey = 43005004;
            bankingDetailsRow.MainAccount = true;
            bankingDetailsRow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            MainDS.PBankingDetails.Rows.Add(bankingDetailsRow);

            PPartnerBankingDetailsRow partnerBankingDetails = MainDS.PPartnerBankingDetails.NewRowTyped(true);
            partnerBankingDetails.PartnerKey = PartnerRow.PartnerKey;
            partnerBankingDetails.BankingDetailsKey = bankingDetailsRow.BankingDetailsKey;
            MainDS.PPartnerBankingDetails.Rows.Add(partnerBankingDetails);

            result = connector.SubmitChanges(ref MainDS, ref ResponseDS, out VerificationResult);

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving 2. Please check the logs");
            }

            foreach (DataTable t in MainDS.Tables)
            {
                if ((t == MainDS.PBankingDetails)
                    || (t == MainDS.PPartnerBankingDetails)
                    || (t == MainDS.PDataLabelValuePartner))
                {
                    int NumRows = t.Rows.Count;

                    for (int RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
                    {
                        DataRow InspectDR = t.Rows[RowIndex];

                        // delete all added Rows.
                        if (InspectDR.RowState == DataRowState.Added)
                        {
                            InspectDR.Delete();
                        }
                    }
                }
            }

            MainDS.AcceptChanges();

            Assert.AreEqual(1, PBankingDetailsUsageAccess.CountViaPPartner(PartnerRow.PartnerKey, null), "count of main accounts for partner");

            // add another account
            bankingDetailsRow = MainDS.PBankingDetails.NewRowTyped(true);
            bankingDetailsRow.AccountName = "2nd account of " + PartnerRow.PartnerShortName;
            bankingDetailsRow.BankAccountNumber = new Random().Next().ToString();
            bankingDetailsRow.BankingDetailsKey = (MainDS.PBankingDetails.Count + 1) * -1;
            bankingDetailsRow.BankKey = 43005004;
            bankingDetailsRow.MainAccount = false;
            bankingDetailsRow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            MainDS.PBankingDetails.Rows.Add(bankingDetailsRow);

            partnerBankingDetails = MainDS.PPartnerBankingDetails.NewRowTyped(true);
            partnerBankingDetails.PartnerKey = PartnerRow.PartnerKey;
            partnerBankingDetails.BankingDetailsKey = bankingDetailsRow.BankingDetailsKey;
            MainDS.PPartnerBankingDetails.Rows.Add(partnerBankingDetails);

            PartnerEditTDS ChangedDS = MainDS.GetChangesTyped(true);
            result = connector.SubmitChanges(ref ChangedDS, ref ResponseDS, out VerificationResult);
            MainDS.Merge(ChangedDS);

            foreach (DataTable t in MainDS.Tables)
            {
                if ((t == MainDS.PBankingDetails)
                    || (t == MainDS.PPartnerBankingDetails)
                    || (t == MainDS.PDataLabelValuePartner))
                {
                    int NumRows = t.Rows.Count;

                    for (int RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
                    {
                        DataRow InspectDR = t.Rows[RowIndex];

                        // delete all added Rows.
                        if (InspectDR.RowState == DataRowState.Added)
                        {
                            InspectDR.Delete();
                        }
                    }
                }
            }

            MainDS.AcceptChanges();

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving 3. Please check the logs");
            }

            // now delete the main bank account
            PartnerEditTDSPBankingDetailsRow toDelete = null;

            foreach (PartnerEditTDSPBankingDetailsRow row in MainDS.PBankingDetails.Rows)
            {
                if (row.MainAccount)
                {
                    toDelete = row;
                    break;
                }
            }

            Assert.IsNotNull(toDelete, "cannot find main account");
            Assert.AreEqual(true, toDelete.MainAccount, "should be the main account");
            MainDS.PPartnerBankingDetails.Rows.Find(new object[] { PartnerRow.PartnerKey, toDelete.BankingDetailsKey }).Delete();
            toDelete.Delete();

            ChangedDS = MainDS.GetChangesTyped(true);
            result = connector.SubmitChanges(ref ChangedDS, ref ResponseDS, out VerificationResult);

            Assert.AreEqual(1, VerificationResult.Count, "should fail because we have no main account anymore");
            Assert.AreEqual("One Bank Account of a Partner must be set as the 'Main Account'. Please select the record that should become the 'Main Account' and choose 'Set Main Account'.",
                VerificationResult[0].ResultText,
                "should fail because we have no main account anymore");

            PartnerEditTDSPBankingDetailsRow otherAccount = null;

            foreach (PartnerEditTDSPBankingDetailsRow row in MainDS.PBankingDetails.Rows)
            {
                if ((row.RowState != DataRowState.Deleted) && !row.MainAccount)
                {
                    otherAccount = row;
                    break;
                }
            }

            otherAccount.MainAccount = true;

            ChangedDS = MainDS.GetChangesTyped(true);
            result = connector.SubmitChanges(ref ChangedDS, ref ResponseDS, out VerificationResult);

            MainDS.Merge(ChangedDS);
            MainDS.AcceptChanges();

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving 4. Please check the logs");
            }

            // now delete the last remaining bank account
            toDelete = MainDS.PBankingDetails[0];
            Assert.AreEqual(true, toDelete.MainAccount);
            MainDS.PPartnerBankingDetails.Rows.Find(new object[] { PartnerRow.PartnerKey, toDelete.BankingDetailsKey }).Delete();
            toDelete.Delete();
            ChangedDS = MainDS.GetChangesTyped(true);
            result = connector.SubmitChanges(ref ChangedDS, ref ResponseDS, out VerificationResult);
            MainDS.Merge(ChangedDS);
            MainDS.AcceptChanges();

            if (VerificationResult.HasCriticalErrors)
            {
                TLogging.Log(VerificationResult.BuildVerificationResultString());
                Assert.Fail("There was a critical error when saving 5. Please check the logs");
            }
        }
    }
}