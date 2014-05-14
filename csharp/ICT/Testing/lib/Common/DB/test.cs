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
    ///  This is a testing program for Ict.Common.DB.dll
    [TestFixture]
    public class TTestCommonDB
    {
        /// <summary>
        /// modified version taken from Ict.Petra.Server.App.Main::TServerManager
        /// </summary>
        private void EstablishDBConnection()
        {
            TLogging.Log("  Connecting to Database...");

            DBAccess.GDBAccessObj = new TDataBase();
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 10);
            try
            {
                DBAccess.GDBAccessObj.EstablishDBConnection(CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType")),
                    TAppSettingsManager.GetValue("Server.DBHostOrFile"),
                    TAppSettingsManager.GetValue("Server.DBPort"),
                    TAppSettingsManager.GetValue("Server.DBName"),
                    TAppSettingsManager.GetValue("Server.DBUserName"),
                    TAppSettingsManager.GetValue("Server.DBPassword"),
                    "");
            }
            catch (Exception)
            {
                throw;
            }

            TLogging.Log("  Connected to Database.");
        }

        /// init
        [SetUp]
        public void Init()
        {
            new TLogging("test.log");
            new TAppSettingsManager("../../etc/TestServer.config");

            EstablishDBConnection();
        }

        /// tear down
        [TearDown]
        public void TearDown()
        {
            DBAccess.GDBAccessObj.CloseDBConnection();
            TLogging.Log("  Database disconnected.");
        }

        /// <summary>
        /// this method will try to create a gift for a non existing gift batch
        /// </summary>
        private void EnforceForeignKeyConstraint()
        {
            TDBTransaction t;
            string sql;

            try
            {
                t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                sql = "INSERT INTO a_gift(a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i) " +
                      "VALUES(43, 99999999, 1)";
                DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }

            // UNDO the test
            t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            sql = "DELETE FROM a_gift" +
                  " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999 AND a_gift_transaction_number_i = 1";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
            DBAccess.GDBAccessObj.CommitTransaction();
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
            TDBTransaction t;
            string sql;

            try
            {
                // setup test scenario: a gift batch, with 2 gifts, each with 2 gift details
                t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                sql = "INSERT INTO a_gift(a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i) " +
                      "VALUES(43, 99999999, 1)";
                DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
                sql =
                    "INSERT INTO a_gift_batch(a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_bank_account_code_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c) "
                    +
                    "VALUES(43, 99999999, 'Test', '6000', 1, 'EUR', '4300')";
                DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }

            // UNDO the test
            t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            sql = "DELETE FROM a_gift" +
                  " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999 AND a_gift_transaction_number_i = 1";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
            sql = "DELETE FROM a_gift_batch" +
                  " WHERE a_ledger_number_i = 43 AND a_batch_number_i = 99999999";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
            DBAccess.GDBAccessObj.CommitTransaction();
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
        /// works fine with postgresql.
        /// for sqlite, you need version 3.7.11 at least; http://www.sqlite.org/releaselog/3_7_11.html
        /// </summary>
        [Test]
        public void TestInsertMultipleRows()
        {
            TDBTransaction t;
            string sql;

            try
            {
                t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                sql =
                    "INSERT INTO a_gift_batch(a_ledger_number_i, a_batch_number_i, a_batch_description_c, a_bank_account_code_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c) "
                    +
                    "VALUES (43, 990, 'TEST', '6000', 1, 'EUR', '4300'),(43, 991, 'TEST', '6000', 1, 'EUR', '4300')";
                DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }

            // UNDO the test
            t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            sql = "DELETE FROM a_gift_batch" +
                  " WHERE a_ledger_number_i = 43 AND (a_batch_number_i = 990 or a_batch_number_i = 991)";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// test sequences
        [Test]
        public void TestSequence()
        {
            TDBTransaction t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            Int64 PreviousSequence = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_statement_number", t);
            Int64 NextSequence = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_statement_number", t);

            Assert.AreEqual(PreviousSequence + 1, NextSequence, "next sequence is one more than previous sequence");
            Int64 CurrentSequence = DBAccess.GDBAccessObj.GetCurrentSequenceValue("seq_statement_number", t);
            Assert.AreEqual(CurrentSequence, NextSequence, "current sequence value should be the last used sequence value");
            DBAccess.GDBAccessObj.RestartSequence("seq_statement_number", t, CurrentSequence);
            Int64 CurrentSequenceAfterReset = DBAccess.GDBAccessObj.GetCurrentSequenceValue("seq_statement_number", t);
            Assert.AreEqual(CurrentSequence, CurrentSequenceAfterReset, "after reset we want the same current sequence");
            Int64 NextSequenceAfterReset = DBAccess.GDBAccessObj.GetNextSequenceValue("seq_statement_number", t);
            Assert.AreEqual(CurrentSequence + 1, NextSequenceAfterReset, "after reset we don't want the previous last sequence number to be repeated");

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// test timestamp
        [Test]
        public void TestTimeStamp()
        {
            TDBTransaction t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            string countSql = "SELECT COUNT(*) FROM PUB_s_system_defaults";
            int count = Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(countSql, t));
            string code = "test" + (count + 1).ToString();

            string insertSql = String.Format(
                "INSERT INTO PUB_s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c, s_modification_id_t) VALUES('{0}', '{1}','{2}',NOW())",
                code,
                "test",
                "test");

            Assert.AreEqual(1, DBAccess.GDBAccessObj.ExecuteNonQuery(insertSql, t));

            string getTimeStampSql = String.Format(
                "SELECT s_modification_id_t FROM PUB_s_system_defaults WHERE s_default_code_c = '{0}'",
                code);
            DateTime timestamp = Convert.ToDateTime(DBAccess.GDBAccessObj.ExecuteScalar(getTimeStampSql, t));

            string updateSql = String.Format(
                "UPDATE PUB_s_system_defaults set s_modification_id_t = NOW(), s_default_description_c = '{0}' where s_default_code_c = '{1}' AND s_modification_id_t = ?",
                "test2",
                code);

            OdbcParameter param = new OdbcParameter("timestamp", OdbcType.DateTime);
            param.Value = timestamp;

            Assert.AreEqual(1, DBAccess.GDBAccessObj.ExecuteNonQuery(updateSql, t, new OdbcParameter[] { param }), "update by timestamp");

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

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
            TDBTransaction t;
            string sql;

            try
            {
                TLogging.Log("Attempting to run BREAKING INSERT command using ExecuteNonQuery...");

                // Arrange #1

                // p_type_description_c must not be null, hence an Exception will be thrown when that SQL command is executed
                sql =
                    "INSERT INTO p_type(p_type_code_c, p_type_description_c) " +
                    "VALUES ('TEST_EXECUTENONQUERY', NULL)";

                t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                // Act #1

                // We expect that ExecuteNonQuery will throw a not-null constraint exception - and this is *what we want*!
                DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);

                // Should not get here (as an Exception will be raised by ExecuteNonQuery!)
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (EOPDBException)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
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

                t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                // Act #2 AND Assert

                // We expect that ExecuteNonQuery *will work* after the previous execution threw an Exception and the
                // Transaction it was enlisted it was rolled back.
                // Should it throw an Exception of Type 'System.InvalidOperationException' then the likely cause for
                // that would be that the underlying IDbCommand Object that is used by ExecuteNonQuery was not correctly
                // disposed of!
                Assert.DoesNotThrow(delegate { DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t); },
                    "No Exception should have been thrown by the call to the ExecuteNonQuery Method, but an Exception WAS thrown!");

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception)
            {
                throw;
            }

            // UNDO the test
            t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            sql = "DELETE FROM p_type" +
                  " WHERE p_type_code_c = 'TEST_EXECUTENONQUERY' AND p_type_description_c = NULL";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
            sql = "DELETE FROM p_type" +
                  " WHERE p_type_code_c = 'TEST_EXECUTENONQUERY' AND p_type_description_c = 'Test should be fine'";
            DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);

            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }
}