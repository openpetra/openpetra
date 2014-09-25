//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       AlanP
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
using NUnit.Framework;
using System.Collections.Generic;
using Ict.Testing.NUnitPetraServer;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
// This test is designed to check the GetAvailableLedgers() method on the server.
// This method returns a ALedgerTable using a SELECT statement that only returns selected columns.
// The tests are designed to check the content of this sub-set of columns
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Tests.MFinance.Server.CrossLedger
{
    /// <summary>
    /// The main test class for Available Ledgers
    /// </summary>
    [TestFixture]
    public class TAvailableLedgersTest
    {
        // Our working data set
        GLSetupTDS FMainDS = null;

        // Class members for our test environment
        int FInitialLedgerCount = -1;
        List <int>FTestLedgerList = new List <int>();
        Boolean FInitSucceeded = false;
        String FInitExceptionMessage = String.Empty;

        /// <summary>
        /// Open database connection and prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");

            // These will be our test ledger numbers
            FTestLedgerList.AddRange(new int[] { 9997, 9998, 9999 });

            // Load existing data
            FMainDS = new GLSetupTDS();

            try
            {
                TDBTransaction transaction = null;
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                delegate
                {
                    ALedgerAccess.LoadAll(FMainDS, transaction);
                });

                // Check that our test rows are not in the database already
                if (!FindTestRows(FTestLedgerList))
                {
                    // Get the initial number of available ledgers
                    FInitialLedgerCount = TGLSetupWebConnector.GetAvailableLedgers().DefaultView.Count;

                    // Add our test rows
                    AddTestRow(FTestLedgerList[0], "NUnitTestLedger1", false);
                    AddTestRow(FTestLedgerList[1], "NUnitTestLedger2", true, "JPY");
                    AddTestRow(FTestLedgerList[2], "NUnitTestLedger2", true);

                    // Save these new rows
                    ALedgerAccess.SubmitChanges(FMainDS.ALedger, null);
                    FMainDS.AcceptChanges();

                    FInitSucceeded = true;
                }
            }
            catch (Exception ex)
            {
                FInitExceptionMessage = ex.Message;
            }
        }

        /// <summary>
        /// Clean up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            // Delete our working test rows
            foreach (int LedgerNum in FTestLedgerList)
            {
                DeleteTestRowIfExists(LedgerNum);
            }

            ALedgerAccess.SubmitChanges(FMainDS.ALedger, null);

            // Disconnect
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// This test ensures the test bench was set up correctly
        /// </summary>
        [Test]
        public void TestInitialisation()
        {
            Assert.IsTrue(FInitSucceeded, "The test bench was not initialised correctly.  The code did not complete.");
            Assert.IsTrue(FInitialLedgerCount >= 0, "The test bench was not initialised correctly.  Failed to establish the initial ledger count.");
            Assert.IsTrue(FInitExceptionMessage == String.Empty, "Test bench init caused an exception: {0}", FInitExceptionMessage);

            Console.WriteLine("TestInitialisation... Initial ledger count is {0}", FInitialLedgerCount.ToString());
        }

        /// <summary>
        /// This will test for the number of ledgers in use
        /// </summary>
        [Test]
        public void TestGetAvailableLedgers()
        {
            DataView ledgerView = TGLSetupWebConnector.GetAvailableLedgers().DefaultView;

            ledgerView.RowFilter = "";     // All ledgers

            int ExpectedCount = FInitialLedgerCount + FTestLedgerList.Count;
            Assert.AreEqual(ExpectedCount, ledgerView.Count, "Expected 'All ledgers' count to be " + ExpectedCount.ToString());

            Console.WriteLine("New 'All' ledger count is {0}", ledgerView.Count.ToString());
        }

        /// <summary>
        /// This will test for the number of ledgers in use
        /// </summary>
        [Test]
        public void TestGetAvailableInUseLedgers()
        {
            DataView ledgerView = TGLSetupWebConnector.GetAvailableLedgers().DefaultView;

            ledgerView.RowFilter = "a_ledger_status_l = 1";     // Only view 'in use' ledgers

            int ExpectedCount = FInitialLedgerCount + FTestLedgerList.Count - 1;
            Assert.AreEqual(ExpectedCount, ledgerView.Count, "Expected 'In Use' ledger count to be " + ExpectedCount.ToString());

            Console.WriteLine("New 'In Use' ledger count is {0}", ledgerView.Count.ToString());
        }

        /// <summary>
        /// This will test for the number of ledgers based in Japanese Yen
        /// </summary>
        [Test]
        public void TestGetAvailableJPYLedgers()
        {
            DataView ledgerView = TGLSetupWebConnector.GetAvailableLedgers().DefaultView;

            ledgerView.RowFilter = "a_base_currency_c = 'JPY'";     // Only view 'in use' ledgers

            int ExpectedCount = 1;
            Assert.AreEqual(ExpectedCount, ledgerView.Count, "Expected 'JPY' ledger count to be " + ExpectedCount.ToString());

            Console.WriteLine("New 'JPY' ledger count is {0}", ledgerView.Count.ToString());
        }

        ///////////////////////////////////////////////////////////////////////////////
        // Private Helper functions used in setup and tear down

        private Boolean FindTestRows(List <Int32>ALedgerNumberList)
        {
            // Check whether our test rows already exist
            foreach (Int32 LedgerNumber in ALedgerNumberList)
            {
                if (FMainDS.ALedger.Rows.Find(LedgerNumber) != null)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddTestRow(Int32 ALedgerNumber, String ALedgerName, Boolean ALedgerStatus, String ALedgerCurrency = null)
        {
            // Create a new row and specify the column values that are part of our test
            ALedgerRow newRow = FMainDS.ALedger.NewRowTyped();

            newRow.LedgerNumber = ALedgerNumber;
            newRow.LedgerName = ALedgerName;
            newRow.LedgerStatus = ALedgerStatus;

            if (ALedgerCurrency != null)
            {
                newRow.BaseCurrency = ALedgerCurrency;
            }

            // These columns are also required
            newRow.PartnerKey = ALedgerNumber * 10000;              // unique index for table
            newRow.ForexGainsLossesAccount = "NUnitForex";          // Not NULL and no default specified

            FMainDS.ALedger.Rows.Add(newRow);
        }

        private void DeleteTestRowIfExists(Int32 ALedgerNumber)
        {
            DataRow deleteRow = FMainDS.ALedger.Rows.Find(new object[] { ALedgerNumber });

            if (deleteRow != null)
            {
                deleteRow.Delete();
            }
        }
    }
}