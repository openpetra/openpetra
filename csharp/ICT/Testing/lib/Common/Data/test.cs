//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Data.Odbc;
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
            TDBTransaction ReadTransaction = null;
            GiftBatchTDS MainDS = new GiftBatchTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    ALedgerAccess.LoadAll(MainDS, ReadTransaction);
                });

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
            batch.HashTotal = 83;
            MainDS.AGiftBatch.Rows.Add(batch);

            GiftBatchTDSAccess.SubmitChanges(MainDS);
            MainDS.AcceptChanges();

            MainDS.AGiftBatch[0].BatchDescription = "test2";
            GiftBatchTDSAccess.SubmitChanges(MainDS);


            TDBTransaction transaction = null;
            AGiftBatchTable batches = null;
            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref transaction,
                delegate
                {
                    batches = AGiftBatchAccess.LoadByPrimaryKey(batch.LedgerNumber, batch.BatchNumber, transaction);
                });

            // some problems with sqlite and datagrid
            Assert.AreEqual(typeof(decimal), batches[0][AGiftBatchTable.ColumnHashTotalId].GetType(), "type decimal");
            Assert.AreEqual(83.0m, batches[0].HashTotal, "gift batch hashtotal does not equal");
        }

        /// <summary>
        /// investigating the speed of loading a table.
        /// in the end, in my specific case, it was a DataView that slowed down the load, and also the merge
        /// </summary>
        [Test, Explicit]
        public void SpeedTestLoadIntoTypedTable()
        {
            TDBTransaction ReadTransaction = null;
            DateTime before = DateTime.Now;
            DateTime after = DateTime.Now;
            GiftBatchTDS ds = new GiftBatchTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    string sql = "SELECT PUB_a_gift_detail.*, false AS AlreadyMatched, PUB_a_gift_batch.a_batch_status_c AS BatchStatus " +
                                 "FROM PUB_a_gift_batch, PUB_a_gift_detail " +
                                 "WHERE PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i";

                    before = DateTime.Now;
                    DataTable untyped = DBAccess.GDBAccessObj.SelectDT(sql, "test", ReadTransaction);
                    after = DateTime.Now;

                    TLogging.Log(String.Format("loading all {0} gift details into an untyped table took {1} milliseconds",
                            untyped.Rows.Count,
                            (after.Subtract(before)).TotalMilliseconds));

                    GiftBatchTDSAGiftDetailTable typed = new GiftBatchTDSAGiftDetailTable();
                    before = DateTime.Now;
                    DBAccess.GDBAccessObj.SelectDT(typed, sql, ReadTransaction, new OdbcParameter[0], 0, 0);
                    after = DateTime.Now;

                    TLogging.Log(String.Format("loading all {0} gift details into a typed table took {1} milliseconds",
                            typed.Rows.Count,
                            (after.Subtract(before)).TotalMilliseconds));

                    AMotivationDetailAccess.LoadAll(ds, ReadTransaction);

                    before = DateTime.Now;
                    DBAccess.GDBAccessObj.Select(ds, sql, ds.AGiftDetail.TableName, ReadTransaction);
                    after = DateTime.Now;
                });

            TLogging.Log(String.Format("loading all {0} gift details into a typed dataset took {1} milliseconds",
                    ds.AGiftDetail.Rows.Count,
                    (after.Subtract(before)).TotalMilliseconds));

            before = DateTime.Now;
            GiftBatchTDS ds2 = new GiftBatchTDS();
            ds2.Merge(ds.AGiftDetail);
            after = DateTime.Now;

            TLogging.Log(String.Format("merging typed table into other dataset took {0} milliseconds",
                    (after.Subtract(before)).TotalMilliseconds));
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