//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Npgsql;

namespace Ict.Common.Data.Testing
{
    ///  This is a testing program for Ict.Common.Data.dll
    [TestFixture]
    public class TTestCommonData
    {
        /// Init the test
        [SetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
        }

        /// clean up after the test
        [TearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// update an existing record. testing the modificationID timestamp
        /// </summary>
        [Test]
        public void UpdateRecord()
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            ALedgerAccess.LoadAll(MainDS, null);

            MainDS.ALedger[0].LastGiftBatchNumber++;

            AGiftBatchRow batch = MainDS.AGiftBatch.NewRowTyped();
            batch.LedgerNumber = MainDS.ALedger[0].LedgerNumber;
            batch.BatchNumber = MainDS.ALedger[0].LastGiftBatchNumber;
            batch.BankAccountCode = "6000";
            batch.BatchYear = 0;
            batch.BatchPeriod = 1;
            batch.CurrencyCode = "EUR";
            batch.BatchDescription = "test";
            batch.BankCostCentre = (MainDS.ALedger[0].LedgerNumber * 100).ToString("0000");
            batch.LastGiftNumber = 0;
            MainDS.AGiftBatch.Rows.Add(batch);

            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult result = GiftBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "SubmitChanges should be ok");
            Assert.AreEqual(false, VerificationResult.HasCriticalErrors, "there are critical errors");
            MainDS.AcceptChanges();

            MainDS.AGiftBatch[0].BatchDescription = "test2";
            result = GiftBatchTDSAccess.SubmitChanges(MainDS, out VerificationResult);
            Assert.AreEqual(TSubmitChangesResult.scrOK, result, "SubmitChanges of update should be ok");
            Assert.AreEqual(false, VerificationResult.HasCriticalErrors, "there are critical errors in UPDATE");
        }

#if EXPERIMENT_NOT_FINISHED
        [Test]
        public void TestModifyGiftBatch()
        {
            TDBTransaction t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            GiftBatchTDS MainDS;

            ALedgerAccess.LoadAll(MainDS, t);

            MainDS.ALedger[0].LastGiftBatchNumber++;

            AGiftBatchRow batch = MainDS.AGiftBatch.NewRowTyped();
            batch.LedgerNumber = MainDS.ALedger[0].LedgerNumber;
            batch.BatchNumber = MainDS.ALedger[0].LastGiftBatchNumber;
            batch.BankAccountCode = "6000";
            batch.BatchYear = 1;
            batch.BatchPeriod = 1;
            batch.CurrencyCode = "EUR";
            batch.BankCostCentre = MainDS.ALedger[0].LedgerNumber.ToString() + "00";
            batch.LastGiftNumber = 2;
            MainDS.AGiftBatch.Rows.Add(batch);

            AGiftRow gift = MainDS.AGift.NewRowTyped();
            gift.LedgerNumber = batch.LedgerNumber;
            gift.BatchNumber = batch.BatchNumber;
            gift.GiftTransactionNumber = 1;
            MainDS.AGift.Rows.Add(gift);

            gift = MainDS.AGift.NewRowTyped();
            gift.LedgerNumber = batch.LedgerNumber;
            gift.BatchNumber = batch.BatchNumber;
            gift.GiftTransactionNumber = 2;
            gift.LastDetailNumber = 1;
            MainDS.AGift.Rows.Add(gift);

            AGiftDetailRow giftdetail = MainDS.AGiftDetail.NewRowTyped();
            giftdetail.LedgerNumber = gift.LedgerNumber;
            giftdetail.BatchNumber = gift.BatchNumber;
            giftdetail.GiftTransactionNumber = gift.GiftTransactionNumber;
            giftdetail.DetailNumber = 1;
            giftdetail.MotivationGroupCode = "GIFT";
            giftdetail.MotivationDetailCode = "SUPPORT";
            MainDS.AGiftDetail.Rows.Add(giftdetail);

            MainDS.SubmitChanges(t);
            DBAccess.GDBAccessObj.CommitTransaction();

            // now delete the first gift, and fix the gift detail of the second gift
            t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            MainDS.AGift.Rows.RemoveAt(0);
            MainDS.AGift[0].GiftTransactionNumber = 1;
            MainDS.AGiftDetail[0].GiftTransactionNumber = 1;
            MainDS.AGiftBatch[0].LastGiftNumber = 1;

            MainDS.SubmitChanges(t);
            DBAccess.GDBAccessObj.CommitTransaction();
        }
#endif

    }
}