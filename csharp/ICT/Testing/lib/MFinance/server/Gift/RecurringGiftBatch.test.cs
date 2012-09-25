//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Common.Data;
using Ict.Testing.NUnitTools;

namespace Tests.MFinance.Server.Gift
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TRecurringGiftBatchTest
    {
        Int32 FLedgerNumber = -1;
		GiftBatchTDS FMainDS = null;
      	int FRecurringBatchNumberToDelete = 2;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            //new TLogging("TestServer.log");
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            FLedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
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
        /// This will delete a specified recurring gift batch, and attempt to save it
        /// </summary>
        [Test]
        public void TestDeleteGiftBatch()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            
            FMainDS = new GiftBatchTDS();

            ARecurringGiftBatchAccess.LoadByPrimaryKey(FMainDS, FLedgerNumber, FRecurringBatchNumberToDelete, Transaction);

            ARecurringGiftAccess.LoadViaARecurringGiftBatch(FMainDS, FLedgerNumber, FRecurringBatchNumberToDelete,Transaction);
            
            ARecurringGiftDetailAccess.LoadViaARecurringGiftBatch(FMainDS, FLedgerNumber, FRecurringBatchNumberToDelete, Transaction);
            
            DBAccess.GDBAccessObj.RollbackTransaction();

            // Delete the associated recurring gift detail rows.
			DataView viewGiftDetail = new DataView(FMainDS.ARecurringGiftDetail);
			viewGiftDetail.RowFilter = string.Empty;

			foreach (DataRowView row in viewGiftDetail)
			{
				row.Delete();
			}
            
            // Delete the associated recurring gift rows.
			DataView viewGift = new DataView(FMainDS.ARecurringGift);
			viewGift.RowFilter = string.Empty;

			foreach (DataRowView row in viewGift)
			{
				row.Delete();
			}

			// Delete the recurring batch row.
			FMainDS.AGiftBatch.Rows[0].Delete();
			
			Assert.Throws(Is.InstanceOf(typeof(Exception)), new TestDelegate(SaveChanges));                    

        }
      
        private void SaveChanges()
        {
        	FMainDS.AcceptChanges();
        }

    }
}
