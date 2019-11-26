//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
using System.Data.Common;
using Ict.Common.DB.Exceptions;
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
using Ict.Common.Exceptions;
using Npgsql;
using NpgsqlTypes;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

namespace Ict.Common.DB.Testing
{
    /// This is a testing program for Ict.Common.DB.dll
    /// Please note that this is a Partial Class which continues with Tests in the file test.Multithreading.cs!
    [TestFixture]
    public partial class TTestCommonDB
    {
        private const string DefaultDBConnName = "Default NUnit TTestCommonDB DB Connection";
        private TDBType FDBType;
        private TDataBase FDataBase = null;
        Int32 FProgressUpdateCounter;
        Int32 FProgressUpdateNumber;

        /// init
        [SetUp]
        public void Init()
        {
            new TLogging("../../log/test.log");
            new TAppSettingsManager("../../etc/TestServer.config");
            FDBType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType"));

            FDataBase = DBAccess.Connect(DefaultDBConnName);

            // Reset some Fields for every Test
            FTestDBInstance1 = null;
            FTestDBInstance2 = null;
            FTestCallDBCommand1 = null;
            FTestCallDBCommand2 = null;
            FTestingThread1 = null;
            FTestingThread2 = null;
            FTestingThread1NewTransaction = false;
            FTestingThread2NewTransaction = false;
            FTestingThread1Exception = null;
            FTestingThread2Exception = null;
            FEstablishedDBConnectionSignalDBConn1 = null;
            FEstablishedDBConnectionSignalDBConn2 = null;
            FCloseDBConnectionSignalDBConn1 = null;
            FCloseDBConnectionSignalDBConn2 = null;
            FDBConnectionClosedSignalDBConn1 = null;
            FDBConnectionClosedSignalDBConn2 = null;
            FGotNewOrExistingDBTransactionSignal1 = null;
            FGotNewOrExistingDBTransactionSignal2 = null;
            FRollbackDBTransactionSignal1 = null;
            FRollbackDBTransactionSignal2 = null;
            FDBTransactionRolledbackSignal1 = null;
            FDBTransactionRolledbackSignal2 = null;
            FProgressUpdateCounter = 0;
            FProgressUpdateNumber = 0;
        }

        /// tear down
        [TearDown]
        public void TearDown()
        {
            FDataBase.CloseDBConnection();
        }

        /// <summary>
        /// this method will try to create a gift for a non existing gift batch
        /// </summary>
        private void EnforceForeignKeyConstraint()
        {
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = true;
            TDataBase db = DBAccess.Connect("Test", FDataBase);
            string sql = "INSERT INTO a_gift(a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i) " +
                         "VALUES(43, 99999999, 1)";

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    db.ExecuteNonQuery(sql, t);
                });

            // UNDO the test
            t = new TDBTransaction();
            SubmissionOK = true;
            sql = "DELETE FROM a_gift" +
                  " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999 AND a_gift_transaction_number_i = 1";
            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    db.ExecuteNonQuery(sql, t);
                });
        }

        /// test the order of statements in a transaction
        [Test]
        public void TestForeignKeyConstraints()
        {
            // see http://nunit.net/blogs/?p=63, we expect an exception to be thrown
            // also http://nunit.org/index.php?p=exceptionAsserts&r=2.5
            Assert.Throws <EOPDBException>(new TestDelegate(EnforceForeignKeyConstraint));
        }

        /// <summary>
        /// this method will try to create a gift for a new gift batch before creating the gift batch.
        /// if the constraints would be checked only when committing the transaction, everything would be fine.
        /// but usually you get a violation of foreign key constraint a_gift_fk1
        /// </summary>
        private void WrongOrderSqlStatements()
        {
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = true;
            TDataBase db = DBAccess.Connect("Test", FDataBase);

            // setup test scenario: a gift batch, with 2 gifts, each with 2 gift details
            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    string sql = "INSERT INTO a_gift(a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i) " +
                                 "VALUES(43, 99999999, 1)";
                    db.ExecuteNonQuery(sql,
                        t);
                    sql =
                        "INSERT INTO a_gift_batch(a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_bank_account_code_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c) "
                        +
                        "VALUES(43, 99999999, 'Test', '6000', 1, 'EUR', '4300')";
                    db.ExecuteNonQuery(sql, t);
                });

            // UNDO the test
            t = null;
            SubmissionOK = true;
            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    string sql = "DELETE FROM a_gift" +
                                 " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999 AND a_gift_transaction_number_i = 1";
                    db.ExecuteNonQuery(sql, t);
                    sql = "DELETE FROM a_gift_batch" +
                          " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999";
                    db.ExecuteNonQuery(sql, t);
                });
        }

        /// test the order of statements in a transaction
        [Test]
        public void TestOrderStatementsInTransaction()
        {
            // see http://nunit.net/blogs/?p=63, we expect an exception to be thrown
            // also http://nunit.org/index.php?p=exceptionAsserts&r=2.5

            Assert.Throws <EOPDBException>(new TestDelegate(WrongOrderSqlStatements));
        }

        /// <summary>
        /// insert multiple rows in one statement.
        /// </summary>
        [Test]
        public void TestInsertMultipleRows()
        {
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = true;
            string sql;
            TDataBase db = DBAccess.Connect("TestInsertMultipleRows", FDataBase);

            db.WriteTransaction(
                ref t,
                ref SubmissionOK,
                delegate
                {
                    sql =
                        "INSERT INTO a_gift_batch(a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_bank_account_code_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d) "
                        +
                        "VALUES (43, 990, 'TEST', '6000', 1, 'EUR', '4300', CURRENT_DATE()),(43, 991, 'TEST', '6000', 1, 'EUR', '4300', CURRENT_DATE())";
                    db.ExecuteNonQuery(sql, t);
                });

            // UNDO the test
            t = new TDBTransaction();
            SubmissionOK = true;
            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    sql = "DELETE FROM a_gift_batch" +
                          " WHERE a_ledger_number_i = 43 AND (a_batch_number_i = 990 or a_batch_number_i = 991)";
                    db.ExecuteNonQuery(sql, t);
                });
        }

        /// test sequences
        [Test]
        public void TestSequence()
        {
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("TestInsertMultipleRows", FDataBase);
            bool SubmissionOK = true;

            db.WriteTransaction(ref t,
                ref SubmissionOK,
                delegate
                {
                    Int64 PreviousSequence = db.GetNextSequenceValue("seq_statement_number", t);
                    Int64 NextSequence = db.GetNextSequenceValue("seq_statement_number", t);

                    Assert.AreEqual(PreviousSequence + 1, NextSequence, "next sequence is one more than previous sequence");
                    Int64 CurrentSequence = db.GetCurrentSequenceValue("seq_statement_number", t);
                    Assert.AreEqual(CurrentSequence, NextSequence, "current sequence value should be the last used sequence value");
                    db.RestartSequence("seq_statement_number", t, CurrentSequence);
                    Int64 CurrentSequenceAfterReset = db.GetCurrentSequenceValue("seq_statement_number", t);
                    Assert.AreEqual(CurrentSequence, CurrentSequenceAfterReset, "after reset we want the same current sequence");
                    Int64 NextSequenceAfterReset = db.GetNextSequenceValue("seq_statement_number", t);
                    Assert.AreEqual(CurrentSequence + 1, NextSequenceAfterReset,
                        "after reset we don't want the previous last sequence number to be repeated");
                });
        }

        /// test timestamp
        [Test]
        public void TestTimeStamp()
        {
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("Test", FDataBase);
            bool SubmissionOK = true;

            db.WriteTransaction(ref t,
                ref SubmissionOK,
                delegate
                {
                    string countSql = "SELECT COUNT(*) FROM PUB_s_system_defaults";
                    int count = Convert.ToInt32(db.ExecuteScalar(countSql, t));
                    string code = "test" + (count + 1).ToString();

                    string insertSql = String.Format(
                        "INSERT INTO PUB_s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c, s_modification_id_t) VALUES('{0}', '{1}','{2}',NOW())",
                        code,
                        "test",
                        "test");

                    Assert.AreEqual(1, db.ExecuteNonQuery(insertSql, t));

                    string getTimeStampSql = String.Format(
                        "SELECT s_modification_id_t FROM PUB_s_system_defaults WHERE s_default_code_c = '{0}'",
                        code);
                    DateTime timestamp = Convert.ToDateTime(db.ExecuteScalar(getTimeStampSql, t));

                    string updateSql = String.Format(
                        "UPDATE PUB_s_system_defaults set s_modification_id_t = NOW(), s_default_description_c = '{0}' where s_default_code_c = '{1}' AND s_modification_id_t = ?",
                        "test2",
                        code);

                    OdbcParameter param = new OdbcParameter("timestamp", OdbcType.DateTime);
                    param.Value = timestamp;

                    Assert.AreEqual(1, db.ExecuteNonQuery(updateSql, t, new OdbcParameter[] { param }), "update by timestamp");
                });
        }

        /// <summary>
        /// Test LIKE -> ILIKE substitution in PostgreSQL
        /// </summary>
        [Test]
        public void TestPostgreSQL_ILIKE()
        {
            TPostgreSQL psql = new TPostgreSQL();

            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc and 'xyz'"), "abc and 'xyz'", "No like");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc like 'xyz'"), "abc like 'xyz'", "lower case like");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc LIKE 'xyz'"), "abc ILIKE 'xyz'", "Like before quotes");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc 'LIKE xyz'"), "abc 'LIKE xyz'", "Like inside quotes");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific(
                    "abc 'LIKE xyz' LIKE 'pqr'"), "abc 'LIKE xyz' ILIKE 'pqr'", "Like both inside and outside quotes");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc AND 'def' LIKE 'xyz'"), "abc AND 'def' ILIKE 'xyz'", "Like between two sets of quotes");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("'abc' LIKE 'xyz'"), "'abc' ILIKE 'xyz'", "Quote at start and end");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("LIKE 'xyz'"), "ILIKE 'xyz'", "Like at start");
            Assert.AreEqual(psql.FormatQueryRDBMSSpecific("abc ILIKE 'xyz'"), "abc ILIKE 'xyz'", "there is already an ILIKE");
        }

        #region GNoETransaction_throws_proper_Exception

        /// <summary>
        /// This Test asserts that GetNewOrExistingTransaction will throw the proper Exceptions once it gets called and
        /// a DB Transaction is running and the minimum <see cref="IsolationLevel"/> that is asked for by the caller
        /// cannot be met.
        /// </summary>
        [Test]
        public void TestDBAccess_GNoETransaction_throws_proper_ExceptionOnWrongMinimumIsolationLevel()
        {
            bool NewTrans;
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            //
            // Arrange
            //

            TDBTransaction transaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            //
            // Act and Assert
            //
            Assert.Throws <EDBTransactionIsolationLevelTooLowException>(() =>
                                                                        db.GetNewOrExistingTransaction(
                                                                            IsolationLevel.Serializable,
                                                                            out NewTrans), "GetNewOrExistingTransaction didn't throw expected " +
                                                                        "Exception (EDBTransactionIsolationLevelTooLowException) on asking for a minimum IsolationLevel which "
                                                                        +
                                                                        "cannot be met");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            transaction.Rollback();
        }

        #endregion

        #region CheckRunningDBTransactionIsolationLevelIsCompatible

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsolationLevelIsCompatible when there is no current DB
        /// Transaction throws EDBNullTransactionException.
        /// </summary>
        [Test]
        public void TestDBAccess_CheckRunningDBTransactionIsolationLevelIsCompatible_ExpectEDBNullTransactionException()
        {
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            // Guard Assert
            Assert.IsNull(db.Transaction);


            // Act/Assert
            Assert.Catch <EDBNullTransactionException>(delegate
                                                       {
                                                           db.CheckRunningDBTransactionIsolationLevelIsCompatible(
                                                               IsolationLevel.ReadCommitted);
                                                       },
                                                       "Calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for an exact " +
                                                       "IsolationLevel that is exactly what the running DB Transaction has got did not throw EDBNullTransactionException");
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsolationLevelIsCompatible returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_CheckRunningDBTransactionIsolationLevelIsCompatible_ExpectTrue2()
        {
            bool NewTrans;
            bool Result;
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            //
            // Arrange
            //

            TDBTransaction transaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            // Act
            Result = db.CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel.ReadCommitted);

            // Primary Assert
            Assert.IsTrue(Result, "Calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for an exact " +
                "IsolationLevel that is exactly what the running DB Transaction has got did not return true");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            transaction.Rollback();
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for a minimum
        /// IsolationLevel that isn't met by the running DB Transactions' IsolationLevel returns false.
        /// </summary>
        [Test]
        public void TestDBAccess_CheckRunningDBTransactionIsolationLevelIsCompatible_WithMinimumIsolationLevel_ExpectFalse()
        {
            bool NewTrans;
            bool Result;
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            //
            // Arrange
            //

            TDBTransaction transaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            // Act
            Result = db.CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel.Serializable);

            // Primary Assert
            Assert.IsFalse(Result, "Calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for a minimum " +
                "IsolationLevel that isn't met by the running DB Transactions' IsolationLevel did not return false");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            transaction.Rollback();
        }

        /// <summary>
        /// This Test asserts that calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for a minimum
        /// IsolationLevel that is met by the running DB Transactions' IsolationLevel returns true.
        /// </summary>
        [Test]
        public void TestDBAccess_CheckRunningDBTransactionIsolationLevelIsCompatible_WithMinimumIsolationLevel_ExpectTrue()
        {
            bool NewTrans;
            bool Result;
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            //
            // Arrange
            //

            TDBTransaction transaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTrans);

            // Guard Assert: Check that the new DB Transaction has been taken out
            Assert.That(NewTrans, Is.True);

            // Act
            Result = db.CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel.ReadCommitted);

            // Primary Assert
            Assert.IsTrue(Result, "Calling CheckRunningDBTransactionIsolationLevelIsCompatible and asking for a minimum " +
                "IsolationLevel that is met by the running DB Transactions' IsolationLevel did not return true");

            // Roll back the one DB Transaction that has been requested (this would happen automatically on DB closing, but
            // it's better to do this explicitly here so it is clear it isn't forgotten about.
            transaction.Rollback();
        }

        #endregion

        /// <summary>
        /// Tests that TDataBase.ExecuteNonQuery is working fine when a previous call to TDataBase.ExecuteNonQuery
        /// raised an Exception in the RDBMS driver.
        /// </summary>
        /// <remarks>
        /// Similar tests for other public Methods of the TDataBase class could be written to ensure that that class itself
        /// plus the DB connection aren't left in some 'troublesome state' after RDBMS-level Exceptions have been raised.
        /// </remarks>
        [Test]
        public void TestDBAccess_working_after_ExecuteNonQuery_threw_DBLevel_Exception()
        {
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = true;
            string sql;
            TDataBase db = DBAccess.Connect("TestDBAccess", FDataBase);

            try
            {
                TLogging.Log("Attempting to run BREAKING INSERT command using ExecuteNonQuery...");

                // Arrange #1

                // p_type_description_c must not be null, hence an Exception will be thrown when that SQL command is executed
                sql =
                    "INSERT INTO p_type(p_type_code_c, p_type_description_c) " +
                    "VALUES ('TEST_EXECUTENONQUERY', NULL)";

                t = new TDBTransaction();
                db.WriteTransaction(ref t, ref SubmissionOK,
                    delegate
                    {
                        // Act #1

                        // We expect that ExecuteNonQuery will throw a not-null constraint exception - and this is *what we want*!
                        db.ExecuteNonQuery(sql, t);
                    });
            }
            catch (EOPDBException)
            {
                // That is the result we want so we can continue.  The transaction will have auto-rolled back
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                TLogging.Log("Running WORKING INSERT command using ExecuteNonQuery...");

                // Arrange #2
                sql =
                    "INSERT INTO p_type(p_type_code_c, p_type_description_c) " +
                    "VALUES ('TEST_EXECUTENONQUERY', 'Test should be fine')";

                t = new TDBTransaction();
                db.WriteTransaction(ref t, ref SubmissionOK,
                    delegate
                    {
                        // Act #2 AND Assert

                        // We expect that ExecuteNonQuery *will work* after the previous execution threw an Exception and the
                        // Transaction it was enlisted it was rolled back.
                        // Should it throw an Exception of Type 'System.InvalidOperationException' then the likely cause for
                        // that would be that the underlying IDbCommand Object that is used by ExecuteNonQuery was not correctly
                        // disposed of!
                        Assert.DoesNotThrow(delegate { db.ExecuteNonQuery(sql, t); },
                            "No Exception should have been thrown by the call to the ExecuteNonQuery Method, but an Exception WAS thrown!");
                    });
            }
            catch (Exception)
            {
                throw;
            }

            // UNDO the test
            t = new TDBTransaction();
            SubmissionOK = true;
            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    sql = "DELETE FROM p_type" +
                          " WHERE p_type_code_c = 'TEST_EXECUTENONQUERY' AND p_type_description_c = NULL";
                    db.ExecuteNonQuery(sql, t);
                    sql = "DELETE FROM p_type" +
                          " WHERE p_type_code_c = 'TEST_EXECUTENONQUERY' AND p_type_description_c = 'Test should be fine'";
                    db.ExecuteNonQuery(sql, t);
                });
        }
#if disabledAfterRefactoringDBAccess
        /// <summary>
        /// Tests the SelectUsingDataAdapterMulti Method, passing a single SQL Query Parameter for each of two Partners.
        /// Also, the AMultipleParamQueryProgressUpdateCallback Delegate is hooked up and the correct calling of this gets
        /// asserted, too.
        /// </summary>
        [Test]
        public void TestDBAccess_SelectUsingDataAdapterMulti1()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            const string TestReadQuery1 = "SELECT * from p_partner where p_partner_key_n = :APartnerKey;";
            DataTable TmpDT = new DataTable();
            TDataAdapterCanceller TmpDac;
            TDataBase db = DBAccess.Connect("TestDBAccess");

            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            List <object[]>ParameterValuesList = new List <object[]>();

            ParametersArray[0] = new OdbcParameter("APartnerKey", OdbcType.BigInt);
            ParameterValuesList.Add(new object[] { 43005001 });
            ParameterValuesList.Add(new object[] { 43005002 });

            db.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    // Act AND Asserts # 1 - Prepared Parametrised Query

                    Assert.AreEqual(2, db.SelectUsingDataAdapterMulti(TestReadQuery1, ReadTransaction, ref TmpDT, out TmpDac,
                            AParameterDefinitions : ParametersArray, AParameterValues : ParameterValuesList,
                            APrepareSelectCommand : true,
                            AProgressUpdateEveryNRecs : 1, AMultipleParamQueryProgressUpdateCallback : delegate(int AProgressUpdateCounter)
                            {
                                FProgressUpdateCounter++;
                                FProgressUpdateNumber = AProgressUpdateCounter;
                                return false;
                            }),
                        "SelectUsingDataAdapterMulti using a Prepared Command did not yield 2 records, but ought to.");

                    Assert.AreEqual(2, FProgressUpdateCounter,
                        "SelectUsingDataAdapterMulti using a Prepared Command did not yield 2 progress updates, but ought to.");


                    // Act AND Asserts # 2 - Non-Prepared Parametrised Query
                    FProgressUpdateCounter = 0;

                    Assert.AreEqual(2, db.SelectUsingDataAdapterMulti(TestReadQuery1, ReadTransaction, ref TmpDT, out TmpDac,
                            AParameterDefinitions : ParametersArray, AParameterValues : ParameterValuesList,
                            AProgressUpdateEveryNRecs : 2, AMultipleParamQueryProgressUpdateCallback : delegate(int AProgressUpdateCounter)
                            {
                                FProgressUpdateCounter++;
                                FProgressUpdateNumber = AProgressUpdateCounter;
                                return false;
                            }),
                        "SelectUsingDataAdapterMulti using NO Prepared Command did not yield 2 records, but ought to.");

                    Assert.AreEqual(1, FProgressUpdateCounter,
                        "SelectUsingDataAdapterMulti using NO Prepared Command did not yield 1 progress update, but ought to.");
                });
        }

        /// <summary>
        /// Tests the SelectUsingDataAdapterMulti Method, passing two SQL Query Parameters for each of two Partners.
        /// </summary>
        [Test]
        public void TestDBAccess_SelectUsingDataAdapterMulti2()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            const string TestReadQuery1 =
                "SELECT * from p_partner where p_partner_key_n = :APartnerKey and p_partner_short_name_c LIKE :APartnerShortName;";
            DataTable TmpDT = new DataTable();
            TDataAdapterCanceller TmpDac;
            TDataBase db = DBAccess.Connect("TestDBAccess");

            OdbcParameter[] ParametersArray;
            List <object[]>ParameterValuesList = new List <object[]>();

            ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("APartnerKey", OdbcType.BigInt);
            ParametersArray[1] = new OdbcParameter("APartnerShortName", OdbcType.Text);

            ParameterValuesList.Add(new object[] { 43005001, "z%" });
            ParameterValuesList.Add(new object[] { 43005002, "T%" });

            db.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    // Act AND Assert #1 - Prepared Parametrised Query

                    Assert.AreEqual(1, db.SelectUsingDataAdapterMulti(TestReadQuery1, ReadTransaction, ref TmpDT, out TmpDac,
AParameterDefinitions: ParametersArray, AParameterValues : ParameterValuesList, APrepareSelectCommand : true),
                        "SelectUsingDataAdapterMulti using a Prepared Command did not yield 1 records, but ought to.");

                    // Act AND Assert #2 - Non-Prepared Parametrised Query
                    Assert.AreEqual(1, db.SelectUsingDataAdapterMulti(TestReadQuery1, ReadTransaction, ref TmpDT, out TmpDac,
AParameterDefinitions: ParametersArray, AParameterValues : ParameterValuesList),
                        "SelectUsingDataAdapterMulti using NO Prepared Command did not yield 1 records, but ought to.");
                });
        }

        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() can create a new transaction on the default DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_DefaultConnection()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            int Result = 0;


            DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "Test1Transaction",
                AEncapsulatedDBAccessCode : delegate
                {
                    Result =
                        Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar("SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                ReadTransaction));

                    // Is this the expected connection?
                    Assert.AreEqual("Default NUnit TTestCommonDB DB Connection", ReadTransaction.DataBaseObj.ConnectionName);
                });

            // Did we get the expected transaction?
            Assert.AreEqual("Test1Transaction", ReadTransaction.TransactionName);

            // Check we get a result
            Assert.AreEqual(1, Result);

            // Check the transaction is rolled back
            Assert.False(ReadTransaction.Valid);
        }

        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() can create a new transaction on a new DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_NewConnection()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            int Result = 0;

            // Initialize TSrvSetting; needed by DBAccess.Connect()
            var oink = new TSrvSetting();

            Assert.NotNull(oink);

            DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "Test2Transaction",
                ASeparateDBConnection : true,
                AEncapsulatedDBAccessCode : delegate
                {
                    Result =
                        Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar("SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                ReadTransaction));

                    // Is this the expected connection?
                    Assert.AreEqual("Test2Transaction", ReadTransaction.DataBaseObj.ConnectionName);
                });

            // Did we get the expected transaction?
            Assert.AreEqual("Test2Transaction", ReadTransaction.TransactionName);

            // Check we get a result
            Assert.AreEqual(1, Result);

            // Check the transaction is rolled back
            Assert.False(ReadTransaction.Valid);
        }

        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() can join an existing compatible transaction on the default DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_JoinExistingTransaction()
        {
            TDataBase db = DBAccess.Connect("Test");
            TDBTransaction FirstTransaction = db.BeginTransaction(ATransactionName : "FirstTransaction");
            TDBTransaction ReadTransaction = new TDBTransaction();
            int Result = 0;

            DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "SecondTransaction",
                AEncapsulatedDBAccessCode : delegate
                {
                    Result =
                        Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar("SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                ReadTransaction));

                    // Is this the expected connection?
                    Assert.AreEqual("Default NUnit TTestCommonDB DB Connection", ReadTransaction.DataBaseObj.ConnectionName);
                });

            // Did we get the expected transaction?
            Assert.AreEqual("FirstTransaction", ReadTransaction.TransactionName);

            // Check we get a result
            Assert.AreEqual(1, Result);

            // Check the existing transaction we joined is not rolled back
            Assert.True(ReadTransaction.Valid);

            // Clear up the FirstTransaction we Began earlier
            ReadTransaction.Rollback();
        }

        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() will create a new DB connection and transaction if there is an
        /// incompatible transaction already running on the default DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_CantJoinExistingTransaction()
        {
            if (FDBType == TDBType.SQLite)
            {
                // do not run this test with SQLite
                return;
            }

            TDataBase db = DBAccess.Connect("TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_CantJoinExistingTransaction");
            TDBTransaction FirstTransaction = db.BeginTransaction(ATransactionName : "FirstTransaction");
            TDBTransaction ReadTransaction = new TDBTransaction();
            int Result = 0;

            // Initialize TSrvSetting; needed by DBAccess.Connect()
            var oink = new TSrvSetting();

            Assert.NotNull(oink);

            DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "SecondTransaction",
                AIsolationLevel : IsolationLevel.Serializable,
                AEncapsulatedDBAccessCode : delegate
                {
                    Result =
                        Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar("SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                ReadTransaction));

                    // Is this the expected connection?
                    Assert.AreEqual("SecondTransaction", ReadTransaction.DataBaseObj.ConnectionName);
                });

            // Did we get the expected transaction?
            Assert.AreEqual("SecondTransaction", ReadTransaction.TransactionName);

            // Check we get a result
            Assert.AreEqual(1, Result);

            // Check the new transaction is rolled back
            Assert.False(ReadTransaction.Valid);

            // Check the existing transaction is not rolled back
            Assert.True(FirstTransaction.Valid);

            // Clear up the FirstTransaction we Began earlier
            FirstTransaction.Rollback();
        }

        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() will create an new transaction on a particular requested DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_RequestedConnection()
        {
            TDataBase RequestedConnection = EstablishDBConnectionAndReturnIt("New DB Connection");
            TDBTransaction ReadTransaction = new TDBTransaction();

            DBAccess.SimpleAutoDBConnAndReadTransactionSelector(ATransaction : out ReadTransaction, AName : "NewTransaction",
                ADatabase : RequestedConnection,
                AEncapsulatedDBAccessCode : delegate
                {
                    Result =
                        Convert.ToInt32(ReadTransaction.DataBaseObj.ExecuteScalar("SELECT COUNT(*) FROM p_partner WHERE p_partner_key_n = 43005001",
                                ReadTransaction));

                    // Is this the expected connection?
                    Assert.AreEqual("New DB Connection", ReadTransaction.DataBaseObj.ConnectionName);
                });

            // Did we get the expected transaction?
            Assert.AreEqual("NewTransaction", ReadTransaction.TransactionName);

            // Check we get a result
            Assert.AreEqual(1, Result);

            // Check the new transaction is rolled back
            Assert.False(ReadTransaction.Valid);

            RequestedConnection.CloseDBConnection();
        }
#endif
        /// <summary>
        /// Tests SimpleAutoDBConnAndReadTransactionSelector() will join an existing compatible transaction on a particular
        /// requested DB connection.
        /// </summary>
        [Test]
        public void TestDBAccess_SimpleAutoDBConnAndReadTransactionSelector_RequestedConnectionJoin()
        {
            if (FDBType == TDBType.SQLite)
            {
                // do not run this test with SQLite
                return;
            }

            TDataBase RequestedConnection = DBAccess.Connect("New DB Connection");
            TDBTransaction FirstTransaction = RequestedConnection.BeginTransaction(IsolationLevel.ReadCommitted, -1, "FirstTransaction");
            bool newTransaction;

            TDBTransaction ReadTransaction = RequestedConnection.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                out newTransaction,
                "NewTransaction");

            Assert.AreEqual("New DB Connection", ReadTransaction.DataBaseObj.ConnectionName);

            // Did we get not a new transaction
            Assert.AreEqual(false, newTransaction);

            // Did we get the expected transaction?
            Assert.AreEqual("FirstTransaction", ReadTransaction.TransactionName);

            // Check the transaction we joined is left open
            Assert.True(ReadTransaction.Valid);

            ReadTransaction.Rollback();
            RequestedConnection.CloseDBConnection();
        }
    }
}
