//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
                           // Open new DB Connection
                           TestDBInstance = DBAccess.Connect(String.Format(TestConnectionName, AThreadNumber));

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

                           TestTransactionInstance.Commit();

                           TestDBInstance.CloseDBConnection();
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
                    ReturnValue = DBAccess.Connect(String.Format(TestConnectionName, 1));

                    if (AFurtherDBAccessCode != null)
                    {
                        // Execute any 'encapsulated C# code section' that the caller 'sends us' in the AFurtherDBAccessCode
                        // Action delegate (0..n lines of code!)
                        AFurtherDBAccessCode();
                    }
                });

            // Establish a new DB Connection on TestingThread1 and execute any 'encapsulated C# code section' that the
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

            // Attempt to establish a new, separate DB Connection
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
                           TDataBaseInstance = DBAccess.Connect(String.Format(TestConnectionName, AThreadNumber));

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
            ATestDBInstance1.CloseDBConnection();
            ATestDBInstance2.CloseDBConnection();
        }

        private Thread GetTestingThread(string ATestName, ThreadStart AThreadStart)
        {
            Thread TestingThread;

            TestingThread = new Thread(AThreadStart);
            TestingThread.Name = ATestName;

            return TestingThread;
        }

        private bool CallGetNewOrExistingTransaction(out TDBTransaction ATransaction, IsolationLevel AIsolationLevel = IsolationLevel.ReadCommitted,
            string ATransactionName = "")
        {
            bool NewTrans;

            TDataBase db = DBAccess.Connect("CallGetNewOrExistingTransaction");
            ATransaction = db.GetNewOrExistingTransaction(AIsolationLevel, out NewTrans, ATransactionName);

            return NewTrans;
        }

        #endregion
    }
}
