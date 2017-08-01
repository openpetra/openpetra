//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2017 by OM International
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
using System.Threading;

using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB.Testing
{
    /// This is a testing program for Ict.Common.DB.dll
    /// Please note that this is a Partial Class which continues the Tests that are written in the file test.cs!
    [TestFixture]
    public partial class TTestCommonDB
    {
        private const string TestThreadName = "TestThread {0}";
        private const string TestConnectionName = "TestConnection {0}";
        private const string TestTransactionName = "TestTransaction {0}";

        private TDataBase FTestDBInstance1;
        private TDataBase FTestDBInstance2;
        private Action FTestCallDBCommand1;
        private Action FTestCallDBCommand2;
        private Thread FTestingThread1;
        private Thread FTestingThread2;
        private bool FTestingThread1NewTransaction;
        private bool FTestingThread2NewTransaction;
        private Exception FTestingThread1Exception;
        private Exception FTestingThread2Exception;
        private static ManualResetEvent FEstablishedDBConnectionSignalDBConn1;
        private static ManualResetEvent FEstablishedDBConnectionSignalDBConn2;
        private static ManualResetEvent FCloseDBConnectionSignalDBConn1;
        private static ManualResetEvent FCloseDBConnectionSignalDBConn2;
        private static ManualResetEvent FDBConnectionClosedSignalDBConn1;
        private static ManualResetEvent FDBConnectionClosedSignalDBConn2;
        private static ManualResetEvent FGotNewOrExistingDBTransactionSignal1;
        private static ManualResetEvent FGotNewOrExistingDBTransactionSignal2;
        private static ManualResetEvent FRollbackDBTransactionSignal1;
        private static ManualResetEvent FRollbackDBTransactionSignal2;
        private static ManualResetEvent FDBTransactionRolledbackSignal1;
        private static ManualResetEvent FDBTransactionRolledbackSignal2;

        private enum TMethodForReadQueries
        {
            Select,

            SelectDT,

            SelectDT_Utilising_SelectDTInternal,

            SelectUsingDataAdapter,

            ExecuteScalar
        }

        /// <summary>
        /// This Test asserts that if Thread 'A' starts a DB Transaction using GetNewOrExistingTransaction, *another* Thread 'B'
        /// isn't allowed to re-use that ('piggy-back' on) DB Transaction.
        /// Reason for checking that: Running DB Commands on a different Thread than the one that the DB Transaction was started
        /// on isn't supported as the ADO.NET providers (specifically: the PostgreSQL provider, Npgsql) aren't thread-safe!
        /// </summary>
        [TestCase(true)]
        [TestCase(false)]
        public void TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded(
            bool ASimulateLongerRunningThread)
        {
            IAsyncResult TestCallDBCommand1AsyncResult;
            IAsyncResult TestCallDBCommand2AsyncResult;

            TLogging.Log(String.Format("--> Start of Test 'TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded'. " +
                    "ASimulateLongerRunningThread={0}", ASimulateLongerRunningThread));

            //
            // Arrange
            //

            // Create ManualResetEvents for signalling between the Thread this Test runs on and the two
            // DB Transaction-starting-and-rolling-back-Threads
            FGotNewOrExistingDBTransactionSignal1 = new ManualResetEvent(false);
            FGotNewOrExistingDBTransactionSignal2 = new ManualResetEvent(false);
            FRollbackDBTransactionSignal1 = new ManualResetEvent(false);
            FRollbackDBTransactionSignal2 = new ManualResetEvent(false);
            FDBTransactionRolledbackSignal1 = new ManualResetEvent(false);
            FDBTransactionRolledbackSignal2 = new ManualResetEvent(false);

            // Create Action Delegates that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForGNoET(1, ASimulateLongerRunningThread);
            FTestCallDBCommand2 = GetActionDelegateForGNoET(2, false);

            // Create a separate instance of TDataBase and establish a separate DB connection on it
            TDataBase TestDBInstance = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), false);
            DBAccess.GDBAccessObj = TestDBInstance;
            FTestDBInstance1 = TestDBInstance;

            //
            // Act
            //

            // 1st Step: Call the first Delegate set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool).
            // it seems with Mono 4.6, threads are being reused, and therefore we cannot change the name on an existing thread
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand1Callback), FTestCallDBCommand1);

            // 2nd Step: Wait until the first Thread has started the DB Transaction
            while (FTestDBInstance1 == null || FTestDBInstance1.Transaction == null)
            {
                Thread.Sleep(100);
            }

            // Guard Asserts: Check that the new DB Transaction has been taken out properly and that no Exception occurred
            Assert.That(FTestingThread1NewTransaction, Is.True);
            Assert.IsNull(FTestingThread1Exception, "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: " +
                "An Exception was thrown when taking out a DB Transaction by the first testing Thread, but this wasn't expected");

            // 3rd Step: Call the second Delegate set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool).
            TestCallDBCommand2AsyncResult = FTestCallDBCommand2.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand2);

            // 4th Step: Wait until the both Threads have finished their work
            TLogging.Log(
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: Waiting for signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: RECEIVED signal for getting of new DB Transaction 1");
            FGotNewOrExistingDBTransactionSignal1.Dispose();

            FGotNewOrExistingDBTransactionSignal2.WaitOne();
            TLogging.Log(
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: RECEIVED signal for getting of new DB Transaction 2");
            FGotNewOrExistingDBTransactionSignal2.Dispose();

            TLogging.Log("TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: before any Asserts");

            //
            // Assert
            //

            // Primary Asserts: Ensure that an Exception gets thrown and that it is of Type
            // EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException.
            Assert.NotNull(FTestingThread2Exception,
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: Expected Exception " +
                "is null though it shouldn't be");
            Assert.IsInstanceOf <EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException>(
                FTestingThread2Exception,
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: Expected Exception isn't of Type '"
                +
                "EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException' though it must be of this Type");

            // Signal both Threads that we started earlier that they should each roll back their DB Transaction
            FRollbackDBTransactionSignal1.Set();
            FRollbackDBTransactionSignal2.Set();

            // Wait until the first Thread has rolled back its DB Transaction
            TLogging.Log(
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: Waiting for signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded: RECEIVED signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.Dispose();

            TLogging.Log("--> End of Test 'TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded'.");
        }

        /// <summary>
        /// This Test asserts that after a DB Transaction got started with GetNewOrExistingTransaction it gets properly re-used
        /// (piggy-backed on) when another call to GetNewOrExistingTransaction is done on the *same* Thread.
        /// </summary>
        [Test]
        public void TestDBAccess_Multithreading__GNoETransaction_throws_no_Exception()
        {
            bool NewTrans;

            // Arrange
            NewTrans = CallGetNewOrExistingTransaction(ATransactionName : "GNoETransaction_throws_no_Exception 1");

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            // Act AND Primary Assert in one go: Attempt to 'piggy-back' on the DB Transaction that got created --- this must
            // work (and not throw an Exception as in Test Method 'TestDBAccess_Multithreading__GNoETransaction_throws_proper_ExceptionMultiThreaded')
            // as we are doing this from within the same Thread (the main Thread)!
            Assert.DoesNotThrow(delegate { NewTrans = CallGetNewOrExistingTransaction(
ATransactionName: "GNoETransaction_throws_no_Exception 2"); });

            // Guard Assert: Check that no new Transaction has been taken out (just for peace-of-mind).
            Assert.That(NewTrans, Is.False);

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// This Test asserts that (1) two independent DB Connections can be taked out on the same Thread and (2) that after
        /// closing both DB Connections no more open DB Connections get reported by the RDBMS than there were open before the
        /// two independent DB Transactions were openend.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading__TwoDBConnectionsSingleThreaded()
        {
            int InitialNumberOfDBConnections;

            // This test at present can only work out results when connected to PostgreSQL as at present the required Method
            // TDataBase.GetNumberOfDBConnections() only is able to work with PostgreSQL...
            if (FDBType != TDBType.PostgreSQL)
            {
                return;
            }

            TLogging.Log("--> Start of Test 'TestDBAccess_MultiThreading__TwoDBConnectionsSingleThreaded'.");

            // Arrange: Clear Connection Pool and keep record of how many DB Connections are open before we open any of our own in this Method
            InitialNumberOfDBConnections = TDataBase.ClearConnectionPoolAndGetNumberOfDBConnections(FDBType);

            //
            // Act
            //

            // Open two independent DB Connections (also independent of DBAccess.GDBAccessObj which automatically
            // gets created for every Test in this TestFixture!)
            TDataBase TestDBInstance1 = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), true);
            TDataBase TestDBInstance2 = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 2), true);

            // Assert: Happens only in Method CloseTestConnections - a comparison between InitialNumberOfDBConnections and
            // open DB Connections after closing the Test Connections is done there and the Assert will fail if they are not
            // the same.
            CloseTestConnections(TestDBInstance1, TestDBInstance2, InitialNumberOfDBConnections);

            TLogging.Log("<-- End of Test 'TestDBAccess_MultiThreading__TwoDBConnectionsSingleThreaded'.");
        }

        /// <summary>
        /// TODO
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded()
        {
            int InitialNumberOfDBConnections;
            IAsyncResult TestCallDBCommand1AsyncResult;
            IAsyncResult TestCallDBCommand2AsyncResult;

            // This test at present can only work out results when connected to PostgreSQL as at present the required Method
            // TDataBase.GetNumberOfDBConnections() only is able to work with PostgreSQL...
            if (FDBType != TDBType.PostgreSQL)
            {
                return;
            }

            TLogging.Log("--> Start of Test 'TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded'.");

            //
            // Arrange
            //

            // Clear Connection Pool and keep record of how many DB Connections are open before we open any of our own in this Method
            if (FTestDBInstance1 != null)
            {
                FTestDBInstance1.CloseDBConnection();
                FTestDBInstance1 = null;
            }

            if (FTestDBInstance2 != null)
            {
                FTestDBInstance2.CloseDBConnection();
                FTestDBInstance2 = null;
            }

            if (DBAccess.GDBAccessObj != null)
            {
                DBAccess.GDBAccessObj.CloseDBConnection();
                DBAccess.GDBAccessObj = null;
            }

            DBAccess.GDBAccessObj = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), true);
            InitialNumberOfDBConnections = TDataBase.ClearConnectionPoolAndGetNumberOfDBConnections(FDBType);

            // Create ManualResetEvents for signalling between the Thread this Test runs on and the two
            // DB Establishing-and-DB-closing-Threads
            FEstablishedDBConnectionSignalDBConn1 = new ManualResetEvent(false);
            FEstablishedDBConnectionSignalDBConn2 = new ManualResetEvent(false);
            FCloseDBConnectionSignalDBConn1 = new ManualResetEvent(false);
            FCloseDBConnectionSignalDBConn2 = new ManualResetEvent(false);
            FDBConnectionClosedSignalDBConn1 = new ManualResetEvent(false);
            FDBConnectionClosedSignalDBConn2 = new ManualResetEvent(false);

            //
            // Act
            //
            // 1st Step: Create Action Delegates that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForDBConnectionAndDisconnection(1);
            FTestCallDBCommand2 = GetActionDelegateForDBConnectionAndDisconnection(2);

            TLogging.Log(
                "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: Starting two Threads (asynchronously) for the establishment of two DB Connections...");

            // 2nd Step: Call the Delegates set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool) -
            // until the code in those Action Delegates hits the WaitOne() Methods...!
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand1Callback), FTestCallDBCommand1);
            TestCallDBCommand2AsyncResult = FTestCallDBCommand2.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand2);

            // 3rd Step: Wait until the both Threads have established a DB Connection
            TLogging.Log("TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: Waiting for signal for establishment of DB Connections");
            FEstablishedDBConnectionSignalDBConn1.WaitOne();
            TLogging.Log("TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: RECEIVED signal for establishment of DB Connection 1");
            FEstablishedDBConnectionSignalDBConn1.Dispose();

            FEstablishedDBConnectionSignalDBConn2.WaitOne();
            TLogging.Log("TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: RECEIVED signal for establishment of DB Connection 2");
            FEstablishedDBConnectionSignalDBConn2.Dispose();

            //
            // Assert
            //

            // Guard Asserts
            Assert.IsNull(FTestingThread1Exception, "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: " +
                "An Exception was thrown when establishing a DB Connection by the first testing Thread, but this wasn't expected");
            Assert.IsNull(FTestingThread2Exception, "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: " +
                "An Exception was thrown when establishing a DB Connection by the second testing Thread, but this wasn't expected");

            Assert.IsNotNull(FTestDBInstance1, "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: " +
                "New DB Connection hans't been established by the first testing Thread but that was expected");
            Assert.IsNotNull(FTestDBInstance2, "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: " +
                "New DB Connection hans't been established by the second testing Thread but that was expected");

            // Primary Assert
            Assert.AreEqual(InitialNumberOfDBConnections + 2, TDataBase.GetNumberOfDBConnections(FDBType),
                String.Format("After opening two DB Connections in two separate Threads in parallel, PostgreSQL reports that " +
                    "a difference exists between the actual DB connections it has from us:  initially open DB connections + 2: " +
                    "{0}, now open DB Connections: {1}", InitialNumberOfDBConnections + 2,
                    TDataBase.GetNumberOfDBConnections(FDBType)));

            // Guard Assert: ConnectionIdentifiers of first and second TDataBase instances must not be identical
            Assert.AreNotEqual(FTestDBInstance1.ConnectionIdentifier, FTestDBInstance2.ConnectionIdentifier,
                "ConnectionIdentifiers of first and second TDataBase instances are the same, but ought to be different");

            // Signal both Threads that we started earlier that they should each close their DB Connection
            FCloseDBConnectionSignalDBConn1.Set();
            FCloseDBConnectionSignalDBConn2.Set();

            // Wait until the both Threads have each closed their DB Connection
            TLogging.Log(
                "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: Waiting for signal from Thread 1 that DB Connection has been closed");
            FDBConnectionClosedSignalDBConn1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: RECEIVED signal from Thread 1 that DB Connection has been closed");
            FDBConnectionClosedSignalDBConn1.Dispose();

            TLogging.Log(
                "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: Waiting for signal from Thread 2 that DB Connection has been closed");
            FDBConnectionClosedSignalDBConn2.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded: RECEIVED signal from Thread 2 that DB Connection has been closed");
            FDBConnectionClosedSignalDBConn2.Dispose();

            // Guard Assert: happens only in Method CleanUpAndPerformChecksAfterDisconnctingFromDB - a comparison between
            // InitialNumberOfDBConnections and actually open DB Connections after closing the Test Connections is done
            // there and the Assert will fail if they are not the same.
            CleanUpAndPerformChecksAfterDisconnctingFromDB(InitialNumberOfDBConnections);

            TLogging.Log("<-- End of Test 'TestDBAccess_MultiThreading__TwoDBConnectionsMultiThreaded'.");
        }

        /// <summary>
        /// Asserts that running multiple DB Commands on two independent Threads does not raise any Exceptions!
        /// </summary>
        [TestCase(800, true, 500, "Select")]
        [TestCase(700, true, 500, "SelectDT")]
        [TestCase(900, true, 500, "SelectDT_Utilising_SelectDTInternal")]
        [TestCase(1000, true, 500, "SelectUsingDataAdapter")]
        [TestCase(300, true, 500, "ExecuteScalar")]
        [TestCase(500, true, 500, "SelectUsingDataAdapter")]
        [TestCase(600, true, 300, "SelectUsingDataAdapter")]
        [TestCase(400, true, 800, "SelectUsingDataAdapter")]
        [TestCase(1100, true, 1000, "SelectUsingDataAdapter")]
        public void TestDBAccess_MultiThreading__TwoDBCommandsOnTwoConnectionsMultiThreaded(
            int ADelayBetween1stAnd2ndThread, bool AWithSleeping, int APauseTimeMultiplicator, string AMethodForReadQueries)
        {
            const string TestReadQuery1 = "SELECT * from p_partner where p_partner_key_n = {0};";
            const string TestReadQuery2 = "SELECT * from a_batch where a_batch_number_i = {0};";
            const string TestReadQueryExecuteScalar1 = "SELECT COUNT(*) from p_partner where p_partner_key_n = {0};";
            const string TestReadQueryExecuteScalar2 = "SELECT COUNT(*) from a_batch where a_batch_number_i = {0};";
            const string TestUpdateQuery1 = "UPDATE p_partner SET p_partner_short_name_c = 'Test {0}' where p_partner_key_n = {0};";
            const string TestUpdateQuery2 = "UPDATE a_batch SET a_batch_description_c = 'Test {0}' where a_batch_number_i = {0};";
            const int TestQuery1RunCount = 20;
            const int TestQuery2RunCount = 30;

            int InitialNumberOfDBConnections;
            IAsyncResult TestCallDBCommand1AsyncResult;
            IAsyncResult TestCallDBCommand2AsyncResult;
            TMethodForReadQueries MethodForReadQueries = (TMethodForReadQueries)Enum.Parse(
                typeof(TMethodForReadQueries), AMethodForReadQueries);

            // This test at present can only work out results when connected to PostgreSQL as at present the required Method
            // TDataBase.GetNumberOfDBConnections() only is able to work with PostgreSQL...
            if (FDBType != TDBType.PostgreSQL)
            {
                return;
            }

            TLogging.Log(String.Format("--> Start of Test 'TestDBAccess_MultiThreading__TwoDBCommandsOnTwoConnectionsMultiThreaded'. " +
                    "ADelayBetween1stAnd2ndThread={0}, AWithSleeping={1}, APauseTimeMultiplicator={2}, AMethodForReadQueries={3}",
                    ADelayBetween1stAnd2ndThread, AWithSleeping, APauseTimeMultiplicator, AMethodForReadQueries));

            //
            // Arrange
            //

            // 1st Step: Clear Connection Pool and keep record of how many DB Connections are open before we open any of our own in this Method
            InitialNumberOfDBConnections = TDataBase.ClearConnectionPoolAndGetNumberOfDBConnections(FDBType);

            // 2nd Step: Create Action Delegates that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForMultiDBCallsThread(1, TestQuery1RunCount, 43005000, AWithSleeping,
                APauseTimeMultiplicator, TestReadQuery1, TestReadQueryExecuteScalar1, MethodForReadQueries, TestUpdateQuery1);
            FTestCallDBCommand2 = GetActionDelegateForMultiDBCallsThread(2, TestQuery2RunCount, 0, AWithSleeping,
                APauseTimeMultiplicator, TestReadQuery2, TestReadQueryExecuteScalar2, MethodForReadQueries, TestUpdateQuery2);

            //
            // Act
            //

            // Call the Action Delegates that got set up above asynchronously by using BeginInvoke. This will execute all the
            // code defined in the Action Delegates on separate Threads (these Threads come from the .NET ThreadPool).
            // After each Thread is finished with its work it executes a Callback (TestCallDBCommand1Callback and
            // TestCallDBCommand2Callback, respectively).
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand1Callback), FTestCallDBCommand1);
            Thread.Sleep(ADelayBetween1stAnd2ndThread);
            TestCallDBCommand2AsyncResult = FTestCallDBCommand2.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand2);

            // Block execution on the Thread that this Test is running on (which NUNit creates for us) until *both* Threads
            // have finished their work
            TestCallDBCommand1AsyncResult.AsyncWaitHandle.WaitOne();
            TestCallDBCommand2AsyncResult.AsyncWaitHandle.WaitOne();

            //
            // Assert
            //

            // Primary Asserts: Ensure no Exceptions have happened in either Thread!
            Assert.IsNull(FTestingThread1Exception, "TestDBAccess_MultiThreading__TwoDBCommandsOnTwoConnectionsMultiThreaded: Expected no " +
                "Exception on Thread '" + String.Format(TestThreadName, 1) + "' but Exception was thrown");
            Assert.IsNull(FTestingThread2Exception, "TestDBAccess_MultiThreading__TwoDBCommandsOnTwoConnectionsMultiThreaded: Expected no " +
                "Exception on Thread '" + String.Format(TestThreadName, 2) + "' but Exception was thrown");

            // Guard Assert: A comparison between InitialNumberOfDBConnections and open DB Connections after closing the
            // Test Connections is done in the follwoing Method call. The Assert will fail if they are not the same.
            CleanUpAndPerformChecksAfterDisconnctingFromDB(InitialNumberOfDBConnections);

            TLogging.Log("<-- End of Test 'TestDBAccess_MultiThreading__TwoDBCommandsOnTwoConnectionsMultiThreaded'.");
        }

        private Action GetActionDelegateForGNoET(int AThreadNumber, bool ASimulateLongerRunningThread)
        {
            bool NewTransaction = false;

            return () =>
                   {
                       TLogging.Log(String.Format("GetActionDelegateForGNoET: Thread {0} for calling GetNewOrExisting DB Transaction got started.",
                               AThreadNumber));

                       if (AThreadNumber == 1)
                       {
                           FTestingThread1 = Thread.CurrentThread;

                           // threads from the ThreadPool can be reused, and we are not allowed to set the name again in Mono
                           if (FTestingThread1.Name == String.Empty)
                           {
                               // using a Guid to avoid confusion
                               //FTestingThread1.Name = String.Format(TestThreadName, AThreadNumber);
                               FTestingThread1.Name = String.Format(TestThreadName, Guid.NewGuid());
                           }

                           if (FTestDBInstance1 == null)
                           {
                               FTestDBInstance1 = EstablishDBConnectionAndReturnIt("Thread 1", true);
                           }

                           DBAccess.GDBAccessObj = FTestDBInstance1;
                       }
                       else
                       {
                           FTestingThread2 = Thread.CurrentThread;
                           FTestingThread2.Name = String.Format(TestThreadName, AThreadNumber);

                           // reusing test instance from first thread
                           DBAccess.GDBAccessObj = FTestDBInstance1;
                           FTestDBInstance2 = DBAccess.GDBAccessObj;
                       }

                       try
                       {
                           NewTransaction = CallGetNewOrExistingTransaction(
ATransactionName: "GNoETransaction_throws_proper_Exception " + AThreadNumber.ToString());

                           if (AThreadNumber == 1)
                           {
                               FTestingThread1NewTransaction = NewTransaction;
                           }
                           else
                           {
                               FTestingThread2NewTransaction = NewTransaction;
                           }

                           if (ASimulateLongerRunningThread)
                           {
                               using (var taskHandle = new ManualResetEvent(false))
                               {
                                   taskHandle.WaitOne(5000);
                               }
                           }

                           if (AThreadNumber == 1)
                           {
                               // Signal main Test Thread that we have successfully gotten a DB Transaction!
                               FGotNewOrExistingDBTransactionSignal1.Set();

                               // Now wait until main test Thread signals us to roll back the DB Transaction we have established earlier...
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForGNoET: Waiting for signal for rolling back of DB Transaction (Thread {0})", AThreadNumber));
                               FRollbackDBTransactionSignal1.WaitOne();

                               TLogging.Log(String.Format(
                                       "GetActionDelegateForGNoET: RECEIVED signal for rolling back of DB Transaction (Thread {0})", AThreadNumber));
                               FRollbackDBTransactionSignal1.Dispose();

                               // Roll back the DB Transaction that we have established earlier
                               DBAccess.GDBAccessObj.RollbackTransaction();

                               // Signal main Test Thread that we have successfully rolled back a DB Transaction!
                               FDBTransactionRolledbackSignal1.Set();
                           }
                           else
                           {
                               // Signal main Test Thread that we have successfully gotten a DB Transaction!
                               FGotNewOrExistingDBTransactionSignal2.Set();

                               // Now wait until main test Thread signals us to roll back the DB Transaction we have established earlier...
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForGNoET: Waiting for signal for rolling back of DB Transaction (Thread {0})", AThreadNumber));
                               FRollbackDBTransactionSignal2.WaitOne();

                               TLogging.Log(String.Format(
                                       "GetActionDelegateForGNoET: RECEIVED signal for rolling back of DB Transaction (Thread {0})", AThreadNumber));
                               FRollbackDBTransactionSignal2.Dispose();

                               // Roll back the DB Transaction that we have established earlier
                               DBAccess.GDBAccessObj.RollbackTransaction();

                               // Signal main Test Thread that we have successfully rolled back a DB Transaction!
                               FDBTransactionRolledbackSignal2.Set();
                           }

                           TLogging.Log(String.Format("Thread {0} for calling GetNewOrExisting DB Transaction has finished.", AThreadNumber));
                       }
                       catch (EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException Exc)
                       {
                           if (AThreadNumber == 1)
                           {
                               FTestingThread1Exception = Exc;

                               // Signal main Test Thread that we have finished our work here! (The Test Thread will need
                               // to check FTestingThread1Exception to find out the Exception that happened here.)
                               FGotNewOrExistingDBTransactionSignal1.Set();
                           }
                           else
                           {
                               FTestingThread2Exception = Exc;

                               // Signal main Test Thread that we have finished our work here! (The Test Thread will need
                               // to check FTestingThread2Exception to find out the Exception that happened here.)
                               FGotNewOrExistingDBTransactionSignal2.Set();
                           }
                       }
                   };
        }

        #region CheckEstablishedDBConnectionThreadIsCompatible

        /// <summary>
        /// This Test asserts that calling CheckEstablishedDBConnectionThreadIsCompatible from a different Thread than the
        /// Thread that established the DB Connection returns false.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckEstablishedDBConnectionThreadIsCompatible_ExpectFalse()
        {
            TDataBase TestDBInstance;

            //
            // Arrange
            //
            TestDBInstance = GetTestDBConnectionSimpleThreaded();

            // Act and Primary Assert
            Assert.IsFalse(TestDBInstance.CheckEstablishedDBConnectionThreadIsCompatible(),
                "Calling CheckEstablishedDBConnectionThreadIsCompatible from a different Thread than the Thread that " +
                "established the DB Connection Transaction did not return false");
        }

        /// <summary>
        /// This Test asserts that calling CheckEstablishedDBConnectionThreadIsCompatible from the same Thread than the
        /// Thread that started the DB Transaction returns true if there is no open DB Connection.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckEstablishedDBConnectionThreadIsCompatible_ExpectTrue1()
        {
            // Arrange
            CloseTestDBConnection(DBAccess.GDBAccessObj, DefaultDBConnName);

            // Act and Primary Assert
            Assert.IsTrue(DBAccess.GDBAccessObj.CheckEstablishedDBConnectionThreadIsCompatible(),
                "Calling CheckEstablishedDBConnectionThreadIsCompatible from the same Thread than the Thread that " +
                "established the DB Connection and where there is no open DB Connection did not return true");
        }

        /// <summary>
        /// This Test asserts that calling CheckEstablishedDBConnectionThreadIsCompatible from the same Thread than the
        /// Thread that started the DB Transaction returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckEstablishedDBConnectionThreadIsCompatible_ExpectTrue2()
        {
            // Act and Primary Assert
            Assert.IsTrue(DBAccess.GDBAccessObj.CheckEstablishedDBConnectionThreadIsCompatible(),
                "Calling CheckEstablishedDBConnectionThreadIsCompatible from the same Thread than the Thread that " +
                "established the DB Connection did not return true");
        }

        #endregion

        #region CheckDBTransactionThreadIsCompatible

        /// <summary>
        /// This Test asserts that calling CheckDBTransactionThreadIsCompatible from a different Thread than the Thread that
        /// started the DB Transaction returns false.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectFalse()
        {
            TDataBase TestDBInstance;
            TDBTransaction DBTransOnDefaultTDataBaseInstance;
            IAsyncResult TestCallDBCommand1AsyncResult;

            //
            // Arrange
            //

            // Create ManualResetEvents for signalling between the Thread this Test runs on and the two
            // DB Transaction-starting-and-rolling-back-Threads
            FGotNewOrExistingDBTransactionSignal1 = new ManualResetEvent(false);
            FRollbackDBTransactionSignal1 = new ManualResetEvent(false);
            FDBTransactionRolledbackSignal1 = new ManualResetEvent(false);

            // Create Action Delegate that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForGNoET(1, false);

            // Create a separate instance of TDataBase and establish a separate DB connection on it
            TestDBInstance = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), false);
            DBAccess.GDBAccessObj = TestDBInstance;
            FTestDBInstance1 = TestDBInstance;

            //
            // Act
            //

            // 1st Step: Call the Delegate set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool).
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand1);

            // 2nd Step: Wait until the Thread has finished its work
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectFalse: Waiting for signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectFalse: RECEIVED signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.Dispose();

            // Get the DB Transaction that was started on DBAccess.GDBAccessObj
            DBTransOnDefaultTDataBaseInstance = DBAccess.GDBAccessObj.Transaction;

            // Guard Assert
            Assert.IsNotNull(DBTransOnDefaultTDataBaseInstance, "DBTransOnDefaultTDataBaseInstance must not be null");

            // Primary Assert
            Assert.IsFalse(TDataBase.CheckDBTransactionThreadIsCompatible(DBTransOnDefaultTDataBaseInstance),
                "Calling CheckDBTransactionThreadIsCompatible from a different Thread than the Thread that started the DB " +
                "Transaction did not return false");

            // Signal the Thread that we started earlier that it should roll back its DB Transaction
            FRollbackDBTransactionSignal1.Set();

            // Wait until the Thread has rolled back its DB Transaction
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectFalse: Waiting for signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectFalse: RECEIVED signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.Dispose();
        }

        /// <summary>
        /// This Test asserts that calling CheckDBTransactionThreadIsCompatible from the same Thread than the Thread that
        /// started the DB Transaction returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_ExpectTrue()
        {
            bool NewTrans;
            bool Result;

            //
            // Arrange
            //

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            //
            // Act
            //
            Result = TDataBase.CheckDBTransactionThreadIsCompatible(DBAccess.GDBAccessObj.Transaction);

            // Primary Assert
            Assert.IsTrue(Result,
                "Calling CheckDBTransactionThreadIsCompatible from the same Thread than the Thread that started the DB " +
                "Transaction did not return true");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// This Test asserts that calling CheckDBTransactionThreadIsCompatible with ATransaction = null results in an
        /// ArgumentNullException.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckDBTransactionThreadIsCompatible_throws_ArgumentNullException()
        {
            //
            // Act and Assert
            //
            Assert.Throws <ArgumentNullException>(() =>
                                                  TDataBase.CheckDBTransactionThreadIsCompatible(null),
                                                  "CheckRunningDBTransactionThreadIsCompatible didn't throw expected " +
                                                  "Exception (ArgumentNullException) when called when ATransaction is null.");
        }

        #endregion


        #region CheckRunningDBTransactionThreadIsCompatible

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionThreadIsCompatible from a different Thread than the Thread
        /// that started the DB Transaction returns false.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectFalse()
        {
            TDBTransaction DBTransOnDefaultTDataBaseInstance;
            IAsyncResult TestCallDBCommand1AsyncResult;

            //
            // Arrange
            //

            // Create ManualResetEvents for signalling between the Thread this Test runs on and the two
            // DB Transaction-starting-and-rolling-back-Threads
            FGotNewOrExistingDBTransactionSignal1 = new ManualResetEvent(false);
            FRollbackDBTransactionSignal1 = new ManualResetEvent(false);
            FDBTransactionRolledbackSignal1 = new ManualResetEvent(false);

            // Create Action Delegate that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForGNoET(1, false);

            // Create an instance of TDataBase and establish a DB connection on it
            TDataBase TestDBInstance = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), false);
            DBAccess.GDBAccessObj = TestDBInstance;
            FTestDBInstance1 = TestDBInstance;

            //
            // Act
            //

            // 1st Step: Call the Delegate set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool).
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand1);

            // 2nd Step: Wait until the Thread has finished its work
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectFalse: Waiting for signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectFalse: RECEIVED signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.Dispose();

            // Get the DB Transaction that was started on DBAccess.GDBAccessObj
            DBTransOnDefaultTDataBaseInstance = DBAccess.GDBAccessObj.Transaction;

            // Guard Assert
            Assert.IsNotNull(DBTransOnDefaultTDataBaseInstance, "DBTransOnDefaultTDataBaseInstance must not be null");

            // Primary Assert
            Assert.IsFalse(DBAccess.GDBAccessObj.CheckRunningDBTransactionThreadIsCompatible(),
                "Calling CheckRunningDBTransactionThreadIsCompatible from a different Thread than the Thread that started the DB " +
                "Transaction did not return false");

            // Signal the Thread that we started earlier that it should roll back its DB Transaction
            FRollbackDBTransactionSignal1.Set();

            // Wait until the Thread has rolled back its DB Transaction
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectFalse: Waiting for signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectFalse: RECEIVED signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.Dispose();
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionThreadIsCompatible when there is no current DB Transaction
        /// throws EDBNullTransactionException.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectEDBNullTransactionException()
        {
            // Guard Assert
            Assert.IsNull(DBAccess.GDBAccessObj.Transaction);

            // Act and Primary Assert
            Assert.Catch <EDBNullTransactionException>(delegate
                                                       {
                                                           DBAccess.GDBAccessObj.CheckRunningDBTransactionThreadIsCompatible();
                                                       },
                                                       "Calling CheckRunningDBTransactionThreadIsCompatible when there is no current DB Transaction did not throw EDBNullTransactionException");
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionThreadIsCompatible from the same Thread than the Thread
        /// that started the DB Transaction returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionThreadIsCompatible_ExpectTrue2()
        {
            bool NewTrans;
            TDBTransaction DBTransOnDefaultTDataBaseInstance;

            //
            // Arrange
            //

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            //
            // Act
            //

            // Get the DB Transaction that was started on DBAccess.GDBAccessObj
            DBTransOnDefaultTDataBaseInstance = DBAccess.GDBAccessObj.Transaction;

            // Guard Assert
            Assert.IsNotNull(DBTransOnDefaultTDataBaseInstance);

            // Primary Assert
            Assert.IsTrue(DBAccess.GDBAccessObj.CheckRunningDBTransactionThreadIsCompatible(),
                "Calling CheckRunningDBTransactionThreadIsCompatible from the same Thread than the Thread that started the DB " +
                "Transaction did not return true");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        #endregion


        #region CheckRunningDBTransactionIsCompatible

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsCompatible when there is no current DB Transaction
        /// throws EDBNullTransactionException.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectEDBNullTransactionException()
        {
            // Guard Assert
            Assert.IsNull(DBAccess.GDBAccessObj.Transaction);

            // Act and Primary Assert
            Assert.Catch <EDBNullTransactionException>(delegate {
                                                           DBAccess.GDBAccessObj.CheckRunningDBTransactionIsCompatible(IsolationLevel.ReadCommitted,
                                                               TEnforceIsolationLevel.eilExact);
                                                       }, "Calling CheckRunningDBTransactionIsCompatible when there is no current " +
                                                       "DB Transaction did not throw an EDBNullTransactionException");
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsCompatible from the same Thread than the Thread
        /// that started the DB Transaction returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectTrue2()
        {
            bool NewTrans;
            TDBTransaction DBTransOnDefaultTDataBaseInstance;

            //
            // Arrange
            //

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            //
            // Act
            //

            // Get the DB Transaction that was started on DBAccess.GDBAccessObj
            DBTransOnDefaultTDataBaseInstance = DBAccess.GDBAccessObj.Transaction;

            // Guard Assert
            Assert.IsNotNull(DBTransOnDefaultTDataBaseInstance);

            // Primary Assert
            Assert.IsTrue(DBAccess.GDBAccessObj.CheckRunningDBTransactionIsCompatible(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilExact),
                "Calling CheckRunningDBTransactionIsCompatible from the same Thread than the Thread that started the DB " +
                "Transaction did not return true");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsCompatible from a different Thread than the Thread
        /// that started the DB Transaction returns false.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectFalse()
        {
            TDBTransaction DBTransOnDefaultTDataBaseInstance;
            IAsyncResult TestCallDBCommand1AsyncResult;

            //
            // Arrange
            //

            // Create ManualResetEvents for signalling between the Thread this Test runs on and the two
            // DB Transaction-starting-and-rolling-back-Threads
            FGotNewOrExistingDBTransactionSignal1 = new ManualResetEvent(false);
            FRollbackDBTransactionSignal1 = new ManualResetEvent(false);
            FDBTransactionRolledbackSignal1 = new ManualResetEvent(false);

            // Create Action Delegate that we will execute in a new Thread in the next step. Doing this instead of
            // using a simple Thread pattern (GetTestingThread, .Start, .Join) allows us to catch and process Exceptions on
            // another Thread, which otherwise is not possible within a NUnit Test!
            FTestCallDBCommand1 = GetActionDelegateForGNoET(1, false);

            // Create an instance of TDataBase and establish a DB connection on it
            TDataBase TestDBInstance = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, 1), false);
            DBAccess.GDBAccessObj = TestDBInstance;
            FTestDBInstance1 = TestDBInstance;


            //
            // Act
            //

            // 1st Step: Call the Delegate set up in the Arrange section above asynchronously by using BeginInvoke. This
            // will execute all the code defined in the Delegate on a separate Thread (which comes from the .NET ThreadPool).
            TestCallDBCommand1AsyncResult = FTestCallDBCommand1.BeginInvoke(
                new AsyncCallback(TestCallDBCommand2Callback), FTestCallDBCommand1);

            // 2nd Step: Wait until the Thread has finished its work
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectFalse: Waiting for signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectFalse: RECEIVED signal for getting new DB Transaction");
            FGotNewOrExistingDBTransactionSignal1.Dispose();

            // Get the DB Transaction that was started on DBAccess.GDBAccessObj
            DBTransOnDefaultTDataBaseInstance = DBAccess.GDBAccessObj.Transaction;

            // Guard Assert
            Assert.IsNotNull(DBTransOnDefaultTDataBaseInstance, "DBTransOnDefaultTDataBaseInstance must not be null");

            // Primary Assert
            Assert.IsFalse(DBAccess.GDBAccessObj.CheckRunningDBTransactionIsCompatible(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilExact),
                "Calling CheckRunningDBTransactionIsCompatible from a different Thread than the Thread that started the DB " +
                "Transaction did not return false");

            // Signal the Thread that we started earlier that it should roll back its DB Transaction
            FRollbackDBTransactionSignal1.Set();

            // Wait until the Thread has rolled back its DB Transaction
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectFalse: Waiting for signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.WaitOne();
            TLogging.Log(
                "TestDBAccess_MultiThreading_CheckRunningDBTransactionIsCompatible_ExpectFalse: RECEIVED signal that DB Transaction has been rolled back");
            FDBTransactionRolledbackSignal1.Dispose();
        }

        #endregion

        #region SimpleAutoDBConnAndReadTransactionSelector
        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector will create a new DB connection and transaction if the requested DB connection already
        /// has an existing transaction with a compatible IsolationLevel but was started on a different thread.
        /// </summary>
        [Test]
        public void TestDBAccess_MultiThreading_SimpleSelector()
        {
            TDataBase RequestedConnection = EstablishDBConnectionAndReturnIt("New DB Connection");

            RequestedConnection.BeginTransaction(ATransactionName : "First DB Transaction");

            ThreadStart StartConnection = delegate
            {
                TDBTransaction ReadTransaction = null;
                int Result = 0;

                // Need to instantiate a TSrvSetting for DBAccess.SimpleEstablishDBConnection() to create a new connection for us.
                var oink = new TSrvSetting();
                Assert.NotNull(oink);

                // As this transaction request comes from a different thread from the one the existing connection was started on,
                // we can't join it and so must create a new connection on this thread.
                DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "Second DB Transaction",
                    ADatabase : RequestedConnection,
                    AEncapsulatedDBAccessCode : delegate
                    {
                        Result =
                            Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar(
                                    "SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                    ReadTransaction));

                        // Is this the expected connection?
                        Assert.AreEqual("Second DB Transaction", ReadTransaction.DataBaseObj.ConnectionName);
                    });

                //// Check we get a result
                Assert.AreEqual(1, Result);

                //// Check the new transaction is rolled back
                Assert.False(ReadTransaction.Valid);
            };
            Thread FirstThread = new Thread(StartConnection);

            FirstThread.Start();
            FirstThread.Join();


            RequestedConnection.RollbackTransaction();
            RequestedConnection.CloseDBConnection();
        }

        #endregion

        #region Helper Methods

        private Action GetActionDelegateForMultiDBCallsThread(int AThreadNumber, int ANumberOfQueryExecutions,
            int ARepeatCounterStartNumber, bool AWithSleeping, int APauseTimeMultiplicator, string ATestReadQuery,
            string ATestReadQueryExecuteScalar, TMethodForReadQueries AMethodForReadQueries, string ATestUpateQuery)
        {
            TDataBase TestDBInstance;
            TDBTransaction TestTransactionInstance;
            bool IsNewTransaction;
            DateTime NextSleepTime = DateTime.MinValue;
            int PauseTime;
            string Query;
            string TableName;
            DataSet TestDS = null;
            DataTable TestDT = null;
            TDataAdapterCanceller TestDataAdapterCanceller;

            return () =>
                   {
                       // threads from the ThreadPool can be reused, and we are not allowed to set the name again in Mono
                       if (Thread.CurrentThread.Name == String.Empty)
                       {
                           // using a Guid to avoid confusion
                           //Thread.CurrentThread.Name = String.Format(TestThreadName, AThreadNumber);
                           Thread.CurrentThread.Name = String.Format(TestThreadName, Guid.NewGuid());
                       }

                       try
                       {
                           // Open independent DB Connection (independent of DBAccess.GDBAccessObj which automatically
                           // gets created for every Test in this TestFixture!)
                           TestDBInstance = EstablishDBConnectionAndReturnIt(String.Format(TestConnectionName, AThreadNumber), false);

                           // Get a new DB Transaction on DB Connection
                           TestTransactionInstance = TestDBInstance.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out IsNewTransaction,
                               "TwoDBCommandsOnTwoConnections_" + String.Format(TestTransactionName, AThreadNumber));

                           // Note: From hereon this test uses a new Property of a TDBTransaction: TestTransactionInstance.DataBaseObj.
                           // It is the same object and instance as TestDBInstance but can be used anywhere where the TDataBase object
                           // is not readily available, eg. in the static Methods of our Typed DataStore [SubmitChanges et al]!

                           if (AWithSleeping)
                           {
                               NextSleepTime = GetNextSleepTime();
                               //                        TLogging.Log("NextSleepTime: " + NextSleepTime.ToString());
                           }

                           for (int RepeatCounter = ARepeatCounterStartNumber; RepeatCounter < (ARepeatCounterStartNumber +
                                                                                                ANumberOfQueryExecutions); RepeatCounter++)
                           {
                               Query = String.Format(ATestReadQuery, RepeatCounter);
                               TableName = "DataTable_for_" + Query;

                               // Run a query on this DB Connection using the new DB Transaction that got started on this DB Connection
                               switch (AMethodForReadQueries)
                               {
                                   case TMethodForReadQueries.Select:
                                       TestDS = new DataSet();

                                       TestTransactionInstance.DataBaseObj.Select(TestDS, Query, TableName, TestTransactionInstance);

                                       break;

                                   case TMethodForReadQueries.SelectDT:
                                       TestDT = new DataTable(TableName);

                                       TestTransactionInstance.DataBaseObj.SelectDT(TestDT, Query, TestTransactionInstance);

                                       break;

                                   case TMethodForReadQueries.SelectDT_Utilising_SelectDTInternal:

                                       TestTransactionInstance.DataBaseObj.SelectDT(Query, TableName, TestTransactionInstance);

                                       break;

                                   case TMethodForReadQueries.SelectUsingDataAdapter:
                                       TestDT = new DataTable(TableName);

                                       TestTransactionInstance.DataBaseObj.SelectUsingDataAdapter(Query, TestTransactionInstance,
                                       ref TestDT, out TestDataAdapterCanceller);

                                       break;

                                   case TMethodForReadQueries.ExecuteScalar:
                                       // Need to redefine Query as we need to perform a different SQL query when using ExecuteScalar
                                       Query = String.Format(ATestReadQueryExecuteScalar, RepeatCounter);

                                       int Count1 =
                                           Convert.ToInt32(TestTransactionInstance.DataBaseObj.ExecuteScalar(Query, TestTransactionInstance, null));

                                       break;

                                   default:
                                       throw new ArgumentOutOfRangeException();
                               }

                               if (AWithSleeping)
                               {
                                   if (DateTime.Now >= NextSleepTime)
                                   {
                                       using (var taskHandle = new ManualResetEvent(false))
                                       {
                                           PauseTime = RandomNumber(1, 5) * APauseTimeMultiplicator;
                                           //                                    TLogging.Log("PauseTime: " + PauseTime.ToString());

                                           taskHandle.WaitOne(PauseTime);
                                       }
                                   }
                               }

                               //                        if (RepeatCounter1 % 3 == 1)
                               //                        {
                               TestTransactionInstance.DataBaseObj.ExecuteNonQuery(String.Format(ATestUpateQuery, RepeatCounter),
                                   TestTransactionInstance);
                               //                        }

                               if (AWithSleeping)
                               {
                                   NextSleepTime = GetNextSleepTime();
                                   //                            TLogging.Log("NextSleepTime: " + NextSleepTime.ToString());
                               }
                           }

                           TestTransactionInstance.DataBaseObj.CommitTransaction();

                           CloseTestDBConnection(TestTransactionInstance.DataBaseObj, String.Format(TestConnectionName, AThreadNumber), false);
                       }
                       catch (Exception Exc)
                       {
                           if (AThreadNumber == 1)
                           {
                               FTestingThread1Exception = Exc;
                           }
                           else
                           {
                               FTestingThread2Exception = Exc;
                           }
                       }
                   };
        }

        private void TestCallDBCommand1Callback(IAsyncResult ar)
        {
            var TestAction = (Action)ar.AsyncState;

            TestAction.EndInvoke(ar);

            TLogging.Log("FTestCallDBCommand1 Action Delegate (that got spawned on a separate Thread) finished its work.");
        }

        private void TestCallDBCommand2Callback(IAsyncResult ar)
        {
            var TestAction = (Action)ar.AsyncState;

            TestAction.EndInvoke(ar);

            TLogging.Log("FTestCallDBCommand2 Action Delegate (that got spawned on a separate Thread) finished its work.");
        }

        private DateTime GetNextSleepTime()
        {
            return DateTime.Now + new TimeSpan(0, 0, 0, 0, RandomNumber(1, 20));
        }

        /// <summary>
        /// Generates a pseudo-random number in a range between a given minimum and maximum.
        /// </summary>
        /// <param name="AMinimum">Minimum number.</param>
        /// <param name="AMaximum">Maximum number.</param>
        /// <returns></returns>
        private int RandomNumber(int AMinimum, int AMaximum)
        {
            Random random = new Random();

            return random.Next(AMinimum, AMaximum);
        }

        private TDataBase GetTestDBConnectionSimpleThreaded(Action AFurtherDBAccessCode = null)
        {
            TDataBase ReturnValue = null;
            Thread TestingThread;

            TestingThread = GetTestingThread(String.Format(TestThreadName, 1), delegate
                {
                    ReturnValue = CallEstablishDBConnection(String.Format(TestConnectionName, 1));

                    if (AFurtherDBAccessCode != null)
                    {
                        // Execute any 'encapsulated C# code section' that the caller 'sends us' in the AFurtherDBAccessCode
                        // Action delegate (0..n lines of code!)
                        AFurtherDBAccessCode();
                    }
                });

            // Establish a new DB Connection on TestingThread1 (independent of DBAccess.GDBAccessObj which automatically
            // gets created for every Test in this TestFixture!) and execute any 'encapsulated C# code section' that the
            // caller 'sends us' in the AFurtherDBAccessCode Action delegate (0..n lines of code!)
            TestingThread.Start();
            TestingThread.Join();

            // Guard Assert: Check that the new DB Connection has been established
            Assert.NotNull(ReturnValue, "GetTestDBConnection1: " +
                "New DB Connection hans't been established by TestingThread but that was expected");

            return ReturnValue;
        }

        private Action GetActionDelegateForDBConnectionAndDisconnection(int AThreadNumber)
        {
            TDataBase TDataBaseInstance = null;

            // Attempt to establish a new, separate DB Connection (independent of DBAccess.GDBAccessObj which automatically
            // gets created for every Test in this TestFixture!)
            return () =>
                   {
                       if (AThreadNumber == 1)
                       {
                           FTestingThread1 = Thread.CurrentThread;

                           // threads from the ThreadPool can be reused, and we are not allowed to set the name again in Mono
                           if (FTestingThread1.Name == String.Empty)
                           {
                               // using a Guid to avoid confusion
                               //FTestingThread1.Name = String.Format(TestThreadName, AThreadNumber);
                               FTestingThread1.Name = String.Format(TestThreadName, Guid.NewGuid());
                           }
                       }
                       else
                       {
                           FTestingThread2 = Thread.CurrentThread;

                           // threads from the ThreadPool can be reused, and we are not allowed to set the name again in Mono
                           if (FTestingThread2.Name == String.Empty)
                           {
                               // using a Guid to avoid confusion
                               //FTestingThread2.Name = String.Format(TestThreadName, AThreadNumber);
                               FTestingThread2.Name = String.Format(TestThreadName, Guid.NewGuid());
                           }
                       }

                       try
                       {
                           TDataBaseInstance = CallEstablishDBConnection(String.Format(TestConnectionName, AThreadNumber));

                           TLogging.Log(String.Format("GetActionDelegateForDBConnectionAndDisconnection: Established DB Connection (Thread {0})",
                                   AThreadNumber));

                           if (AThreadNumber == 1)
                           {
                               FTestDBInstance1 = TDataBaseInstance;

                               // Signal main Test Thread that we have successfully established a DB Connection!
                               FEstablishedDBConnectionSignalDBConn1.Set();

                               // Now wait until main test Thread signals us to close the DB Connection we have established earlier...
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForDBConnectionAndDisconnection: Waiting for signal for closing of DB Connection (Thread {0})",
                                       AThreadNumber));
                               FCloseDBConnectionSignalDBConn1.WaitOne();
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForDBConnectionAndDisconnection: RECEIVED signal for closing of DB Connection (Thread {0})",
                                       AThreadNumber));
                               FCloseDBConnectionSignalDBConn1.Dispose();

                               // Close the DB Connection we have established earlier
                               FTestDBInstance1.CloseDBConnection();

                               // Signal main Test Thread that we have successfully closed the DB Connection!
                               FDBConnectionClosedSignalDBConn1.Set();
                           }
                           else
                           {
                               FTestDBInstance2 = TDataBaseInstance;

                               // Signal main Test Thread that we have successfully established a DB Connection!
                               FEstablishedDBConnectionSignalDBConn2.Set();

                               // Now wait until main test Thread signals us to close the DB Connection we have established earlier...
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForDBConnectionAndDisconnection: Waiting for signal for closing of DB Connection (Thread {0})",
                                       AThreadNumber));
                               FCloseDBConnectionSignalDBConn2.WaitOne();
                               TLogging.Log(String.Format(
                                       "GetActionDelegateForDBConnectionAndDisconnection: RECEIVED signal for closing of DB Connection (Thread {0})",
                                       AThreadNumber));
                               FCloseDBConnectionSignalDBConn2.Dispose();

                               // Close the DB Connection we have established earlier
                               FTestDBInstance2.CloseDBConnection();

                               // Signal main Test Thread that we have successfully closed the DB Connection!
                               FDBConnectionClosedSignalDBConn2.Set();
                           }
                       }
                       catch (Exception Exc)
                       {
                           if (AThreadNumber == 1)
                           {
                               FTestingThread1Exception = Exc;

                               // Signal main Test Thread that we have finished our work here! (The Test Thread will need
                               // to check FTestingThread1Exception to find out the Exception that happened here.)
                               FEstablishedDBConnectionSignalDBConn1.Set();
                           }
                           else
                           {
                               FTestingThread2Exception = Exc;

                               // Signal main Test Thread that we have finished our work here! (The Test Thread will need
                               // to check FTestingThread2Exception to find out the Exception that happened here.)
                               FEstablishedDBConnectionSignalDBConn2.Set();
                           }
                       }
                   };
        }

        private void CloseTestConnections(TDataBase ATestDBInstance1, TDataBase ATestDBInstance2,
            int AInitialNumberOfDBConnections)
        {
            // Close the two independent DB Connections one after the other.
            // Note: If you look at the Log output you will see that closing these two DB Connections HAS NOT GOT AN
            // IMPACT on the number of open DB Connections that PostgreSQL reports - this is because of the Connection
            // Pooling that is going on and these DB Connections not having been 'released' by the Connection Pool...!
            CloseTestDBConnection(ATestDBInstance1, String.Format(TestConnectionName, 1), true);
            CloseTestDBConnection(ATestDBInstance2, String.Format(TestConnectionName, 2), true);

            CleanUpAndPerformChecksAfterDisconnctingFromDB(AInitialNumberOfDBConnections);
        }

        private void CleanUpAndPerformChecksAfterDisconnctingFromDB(int AInitialNumberOfDBConnections)
        {
            TLogging.Log(
                "  CleanUpAndPerformChecksAfterDisconnctingFromDB: Number of open DB Connections on PostgreSQL BEFORE clearing Connection Pool: " +
                TDataBase.GetNumberOfDBConnections(FDBType));

            TLogging.Log(
                "  CleanUpAndPerformChecksAfterDisconnctingFromDB: Number of open DB Connections on PostgreSQL AFTER  clearing Connection Pool: " +
                TDataBase.ClearConnectionPoolAndGetNumberOfDBConnections(FDBType));

            // The following guard assert at present can only work out results when connected to PostgreSQL as at present the required Method
            // TDataBase.GetNumberOfDBConnections() only is able to work with PostgreSQL...
            if (FDBType == TDBType.PostgreSQL)
            {
                // Guard Assert: PostgreSQL must report no more open DB Connections than before we started opening and closing DB
                // Connections in this Method!
                Assert.AreEqual(AInitialNumberOfDBConnections, TDataBase.GetNumberOfDBConnections(FDBType),
                    String.Format("After opening and closing two DB Connections, PostgreSQL reports that a difference exists " +
                        "between the actual DB connections it has from us:  initially open DB connections: {0}, now open DB Connections: {1}",
                        AInitialNumberOfDBConnections, TDataBase.GetNumberOfDBConnections(FDBType)));
            }
        }

        private Thread GetTestingThread(string ATestName, ThreadStart AThreadStart)
        {
            Thread TestingThread;

            TestingThread = new Thread(AThreadStart);
            TestingThread.Name = ATestName;

            return TestingThread;
        }

        private bool CallGetNewOrExistingTransaction(IsolationLevel AIsolationLevel = IsolationLevel.ReadCommitted,
            string ATransactionName = "")
        {
            bool NewTrans;

            DBAccess.GDBAccessObj.GetNewOrExistingTransaction(AIsolationLevel, out NewTrans, ATransactionName);

            return NewTrans;
        }

        private TDataBase CallEstablishDBConnection(string AConnectionName)
        {
            return EstablishDBConnectionAndReturnIt(AConnectionName, false);
        }

        #endregion
    }
}
