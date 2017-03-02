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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Xml;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.DB.DBCaching;
using Ict.Common.DB.Exceptions;
using Ict.Common.IO;
using Npgsql;
using System.Diagnostics;
using Ict.Common.Session;

namespace Ict.Common.DB
{
    /// <summary>
    /// <see cref="IsolationLevel" /> that needs to be enforced when requesting a
    /// DB Transaction with Methods
    /// <see cref="M:DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, out Boolean)" /> and
    /// <see cref="M:DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out Boolean)" />.
    /// </summary>
    public enum TEnforceIsolationLevel
    {
        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match the
        /// specified <see cref="IsolationLevel" /> <em>exactly</em>.
        /// </summary>
        eilExact,

        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match or
        /// exceed the specified <see cref="IsolationLevel" />.
        /// </summary>
        eilMinimum
    }

    /// <summary>
    /// Contains some Constants and a Global Variable for use with Database Access.
    /// </summary>
    public class DBAccess
    {
        /// <summary>DebugLevel for logging of detailed DB Establishing/DB Disconnection information.</summary>
        public const Int32 DB_DEBUGLEVEL_DETAILED_CONN_INFO = 2;

        /// <summary>DebugLevel for logging the SQL code from DB queries.</summary>
        public const Int32 DB_DEBUGLEVEL_QUERY = 3;

        /// <summary>DebugLevel for logging information about DB Transactions.</summary>
        public const Int32 DB_DEBUGLEVEL_TRANSACTION = 10;

        /// <summary>DebugLevel for logging results from DB queries: is 6 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_RESULT = 6;

        /// <summary>DebugLevel for tracing (very verbose log output): is 10 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_TRACE = 10;

        /// <summary>DebugLevel for dumping stacktraces when Thread-safe access to the TDataBase Class is requested/released (extremely verbose log output): is 11</summary>
        public const Int32 DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES = 11;

        /// <summary>
        /// <em>Global Object</em> in which the Application can store a reference to a single instance of
        /// <see cref="TDataBase" />.
        /// <para>
        /// In every Clients AppDomain this holds the one instance that is responsible for the 'always-open' DB Connection for the
        /// Client that gets established automatically on Client connection and which gets closed automatically on Client
        /// disconnection/crash.
        /// </para>
        /// <para>
        /// <em>IMPORTANT: Do not close and re-open the DB Connection that <em>this</em> instance of
        /// <see cref="TDataBase" /> provides in a Client AppDomain as the majority of program code that access the DB
        /// relies on it being <em>open all the time!!!</em></em>
        /// </para>
        /// </summary>
        public static TDataBase GDBAccessObj
        {
            set
            {
                TSession.SetVariable("DBAccessObj", value);
            }
            get
            {
                return (TDataBase)TSession.GetVariable("DBAccessObj");
            }
        }

        /// <summary>
        /// Gets the <see cref="TDataBase"/> instance that <paramref name="ATransaction"/> got started with, or
        /// the 'globally available' <see cref="DBAccess.GDBAccessObj" /> instance in case <paramref name="ATransaction"/>
        /// is null.
        /// <para>
        /// This is needed for some generated code to be able to work with a (custom) reference to a TDataBase instance, is
        /// helpful for static Methods (which might not have a means to get a reference to a custom <see cref="TDataBase" />
        /// instance easily), and can be neat for other scenarios, too.
        /// </para>
        /// </summary>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or null.</param>
        /// <returns><see cref="TDataBase"/> instance that <paramref name="ATransaction"/> got started with, or
        /// the 'globally available' <see cref="DBAccess.GDBAccessObj" /> instance in case <paramref name="ATransaction"/>
        /// is null.
        /// </returns>
        public static TDataBase GetDBAccessObj(TDBTransaction ATransaction)
        {
            return ATransaction != null ? ATransaction.DataBaseObj : GDBAccessObj;
        }

        /// <summary>
        /// Gets the <see cref="TDataBase"/> instance that gets passed in with Argument <paramref name="ADataBase"/>, or
        /// the 'globally available' <see cref="DBAccess.GDBAccessObj"/> instance in case <paramref name="ADataBase"/>
        /// is null.
        /// <para>
        /// This is needed for some generated code to be able to work with a (custom) reference to a TDataBase instance, is
        /// helpful for static Methods (which might not have a means to get a reference to a custom <see cref="TDataBase" />
        /// instance easily), and can be neat for other scenarios, too.
        /// </para>
        /// </summary>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null.</param>
        /// <returns><see cref="TDataBase"/> instance that got passed in with Argument <paramref name="ADataBase"/>, or
        /// the 'globally available' <see cref="DBAccess.GDBAccessObj"/> instance in case <paramref name="ADataBase"/>
        /// is null.
        /// </returns>
        public static TDataBase GetDBAccessObj(TDataBase ADataBase)
        {
            return ADataBase ?? GDBAccessObj;
        }

        /// <summary>
        /// Creates a new <see cref="TDataBase"/> instance and opens a DB Connection on it.
        /// </summary>
        /// <param name="AConnectionName">Name of the DB Connection (optional). It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).</param>
        /// <returns>New <see cref="TDataBase"/> instance with an open DB Connection.</returns>
        public static TDataBase SimpleEstablishDBConnection(string AConnectionName)
        {
            TDataBase DBAccessObj = new TDataBase();

            DBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                TSrvSetting.PostgreSQLServer,
                TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername,
                TSrvSetting.DBPassword,
                "",
                AConnectionName);

            return DBAccessObj;
        }

        /// <summary>
        /// Checks if the 'globally available' DBAccess.GDBAccessObj is idle, i.e. that it is currently
        /// not running any DB Transactions.
        /// </summary>
        /// <returns>True if the 'globally available' DBAccess.GDBAccessObj is idle, i.e. that it is
        /// currently not running any DB Transactions., otherwise false.</returns>
        public static bool IsGDBAccessObjIdle()
        {
            return IsDBAccessObjIdle(DBAccess.GDBAccessObj);
        }

        /// <summary>
        /// Checks if the <see cref="TDataBase"/> object passed in with <paramref name="ADBAccessObj"/>
        /// is idle, i.e. that it is currently not running any DB Transactions.
        /// </summary>
        /// <returns>True if the the <see cref="TDataBase"/> object passed in with <paramref name="ADBAccessObj"/>
        /// is idle, i.e. that it is currently not running any DB Transactions, otherwise false.</returns>
        public static bool IsDBAccessObjIdle(TDataBase ADBAccessObj)
        {
            bool ReturnValue = false;
            bool LockObtained = false;

            try
            {
                // Obtain a 'lock' on ADBAccessObj to ensure thread safety
                ADBAccessObj.WaitForCoordinatedDBAccess();

                LockObtained = true;
            }
            catch (EDBCoordinatedDBAccessWaitingTimeExceededException)
            {
                // DELIBERATE 'swallowing' of this particular Exception as we are dealing with the consequences correctly here!
                // We can get that particular Exception if the DB connection in ADBAccessObj
                // is performing longer-running queries and we for that reason run into a timeout when trying to obtain the
                // 'lock' on ADBAccessObj.
            }

            try
            {
                // Check if there is a DB Transaction running on the ADBAccessObj instance
                if (LockObtained
                    && (ADBAccessObj.TransactionNonThreadSafe == null))
                {
                    ReturnValue = true;
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("DBAccess.IsDBAccessObjIdle encountered an Exception: " + Exc.ToString());

                throw;
            }
            finally
            {
                if (LockObtained)
                {
                    // Release the 'lock' on the ADBAccessObj that we obtained earlier to allow other threads again
                    ADBAccessObj.ReleaseCoordinatedDBAccess();
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Begins a DB Transaction on a <see cref="TDataBase"/> instance that has currently not got a DB Transaction
        /// running and hence can be used to start a DB Transaction. If the <see cref="DBAccess.GDBAccessObj"/> instance
        /// has currently not got a DB Transaction running then this will be used and a new DB Transaction will be started
        /// on that, otherwise a separate <see cref="TDataBase" /> instance will be created and a separate DB Connection
        /// will be started on that, on which the new DB Transaction will be started on. (This process is completely
        /// thread-safe!)
        /// </summary>
        /// <remarks><em>Important:</em> YOU are responsible to close a DB Connection that was started inside this Method!
        /// The <paramref name="ANewDBConnectionEstablished"/> Argument will be true in that case, and you must call
        /// <see cref="TDataBase.CloseDBConnection"/> on the <see cref="TDataBase"/> instance that the DB Transaction
        /// was started on - which is returned in <paramref name="ADBAccessObj"/>!!! Make sure you are using a try-finally
        /// clause to ensure that the closing will always happen when necessary!</remarks>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="ADBAccessObj">The <see cref="TDataBase"/> instance that the DB Transaction was started on.
        /// This will be the <see cref="DBAccess.GDBAccessObj"/> instance if
        /// <paramref name="ANewDBConnectionEstablished"/> is false, otherwise a separate <see cref="TDataBase" />
        /// instance!</param>
        /// <param name="ANewDBConnectionEstablished">Will be true only if a separate <see cref="TDataBase" /> instance
        /// was created and the DB Transaction got started on that, otherwise it will be false.</param>
        /// <param name="ANameForANewDBConnection">Name of the DB Connection, should a new one be established
        /// (default = "").</param>
        /// <param name="ATransactionName">Name of the DB Transaction (default = "").</param>
        /// <returns>A new DB Transaction that was started on a <see cref="TDataBase"/> instance that didn't have a
        /// DB Transaction running at the time of calling this Method.</returns>
        public static TDBTransaction BeginTransactionOnIdleDBAccessObj(IsolationLevel AIsolationLevel,
            out TDataBase ADBAccessObj, out bool ANewDBConnectionEstablished, string ANameForANewDBConnection = "", string ATransactionName = "")
        {
            bool LockObtained = false;
            bool LockAlreadyReleasedEarly = false;

            try
            {
                // Obtain a 'lock' on the DBAccess.GDBAccessObj to ensure thread safety
                DBAccess.GDBAccessObj.WaitForCoordinatedDBAccess();

                LockObtained = true;
            }
            catch (EDBCoordinatedDBAccessWaitingTimeExceededException)
            {
                // DELIBERATE 'swallowing' of this particular Exception as we are dealing with the consequences correctly here!
                // We can get that particular Exception if the 'globally available' DB connection in DBAccess.GDBAccessObj
                // is performing longer-running queries and we for that reason run into a timeout when trying to obtain the
                // 'lock' on DBAccess.GDBAccessObj.
            }

            try
            {
                // Check if there is a DB Transaction running on the DBAccess.GDBAccessObj instance
                if (LockObtained
                    && (DBAccess.GDBAccessObj.TransactionNonThreadSafe == null))
                {
                    // No DB Transaction running - we can use this DBAccess.GDBAccessObj instance and its DB Connection for
                    // the starting of the new DB Transaction!
                    ANewDBConnectionEstablished = false;

                    ADBAccessObj = GDBAccessObj;
                }
                else
                {
                    if (LockObtained)
                    {
                        // Release the 'lock' on the DBAccess.GDBAccessObj that we obtained earlier to allow other threads again
                        DBAccess.GDBAccessObj.ReleaseCoordinatedDBAccess();
                        LockAlreadyReleasedEarly = true;
                    }

                    if (TSrvSetting.RDMBSType == TDBType.SQLite)
                    {
                        // there is an issue with locked database file. therefore trying to avoid multiple database connections
                        // see https://github.com/openpetra/openpetra/issues/182
                        bool NewTransaction;
                        ANewDBConnectionEstablished = false;
                        ADBAccessObj = DBAccess.GDBAccessObj;
                        TDBTransaction Transaction = ADBAccessObj.GetNewOrExistingTransaction(AIsolationLevel,
                            TEnforceIsolationLevel.eilMinimum,
                            out NewTransaction);
                        return Transaction;
                    }

                    // There is a DB Transaction running on the DBAccess.GDBAccessObj instance = we need to create a separate
                    // TDataBase instance...
                    ADBAccessObj = new TDataBase();

                    // ,.. and establish a separate DB Connection on that separate TDataBase instance.
                    try
                    {
                        ADBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                            TSrvSetting.PostgreSQLServer,
                            TSrvSetting.PostgreSQLServerPort,
                            TSrvSetting.PostgreSQLDatabaseName,
                            TSrvSetting.DBUsername,
                            TSrvSetting.DBPassword,
                            "",
                            ANameForANewDBConnection);

                        ANewDBConnectionEstablished = true;
                    }
                    catch (Exception Exc)
                    {
                        // The 'EDBConnectionNotEstablishedException' Exception will be picked up by the FirstChanceHandler and
                        // the DB reconnection mechanism will jump into gear, hence we are not logging this Exception to keep
                        // the log file clean (we are not 'swallowing it', though!!!)
                        if (!(Exc is EDBConnectionNotEstablishedException))
                        {
                            TLogging.Log("DBAccess.BeginTransactionOnIdleDBAccessObj encountered an Exception while establishing " +
                                "a Database Connection:" + Exc.ToString());
                        }

                        throw;
                    }
                }

                // Begin the DB Transaction - on whatever TDataBase instance we determined above!
                return ADBAccessObj.BeginTransaction(AIsolationLevel, false, ATransactionName : ATransactionName);
            }
            catch (Exception Exc)
            {
                // The 'EDBConnectionNotEstablishedException' and 'EDBConnectionBrokenException' Exceptions will be picked up
                // by the FirstChanceHandler and the DB reconnection mechanism will jump into gear, hence we are not logging
                // this Exception to keep the log file clean (we are not 'swallowing it', though!!!)
                if (!(Exc is EDBConnectionNotEstablishedException)
                    && !(Exc is EDBConnectionBrokenException))
                {
                    TLogging.Log("DBAccess.BeginTransactionOnIdleDBAccessObj encountered an Exception: " + Exc.ToString());
                }

                throw;
            }
            finally
            {
                if (LockObtained
                    && (!LockAlreadyReleasedEarly))
                {
                    // Release the 'lock' on the DBAccess.GDBAccessObj that we obtained earlier to allow other threads again
                    DBAccess.GDBAccessObj.ReleaseCoordinatedDBAccess();
                }
            }
        }

        #region AutoTransactions

        /// <summary>
        /// Starts a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
        /// hence can be used to start a DB Transaction and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction gets rolled back and the
        /// DB Connection gets closed if a separate DB Connection got indeed opened, otherwise the DB Connection is left open.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="AContext">Context in which the Method runs (passed as Name to a newly established DB Connection
        /// (if one indeed needs to be established) and as Name to the DB Transaction, too.</param>
        /// <param name="AReadTransaction">DB Transaction that got started inside this Method. <em>WARNING: </em>
        /// This gets rolled back automatically after the C# Delegate in <paramref name="AEncapsulatedDBAccessCode"/>
        /// got executed!</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void SimpleAutoReadTransactionWrapper(IsolationLevel AIsolationLevel, string AContext,
            out TDBTransaction AReadTransaction, Action AEncapsulatedDBAccessCode)
        {
            if (TSrvSetting.RDMBSType == TDBType.SQLite && DBAccess.GDBAccessObj != null)
            {
                // there is an issue with locked database file. therefore trying to avoid multiple database connections
                // see https://github.com/openpetra/openpetra/issues/182

                bool NewTransaction;
                AReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(AIsolationLevel,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    AEncapsulatedDBAccessCode();
                }
                catch (Exception)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        NewTransaction = false;
                        TLogging.LogAtLevel(7, "SimpleAutoReadTransactionWrapper: rolled back own transaction.");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "SimpleAutoReadTransactionWrapper: committed own transaction.");
                    }
                }
                return;
            }

            TDataBase DBConnectionObj;
            bool SeparateDBConnectionEstablished;

            // Start a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
            // hence can be used to start a DB Transaction.
            AReadTransaction = DBAccess.BeginTransactionOnIdleDBAccessObj(AIsolationLevel, out DBConnectionObj,
                out SeparateDBConnectionEstablished, AContext + " DB Connection", AContext + " DB Transaction");

            // Automatic handling of a Read-only DB Transaction - and also the closing the DB Connection if one was
            // established in the call above!
            DBAccess.AutoReadTransaction(ref AReadTransaction, SeparateDBConnectionEstablished, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// Starts a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
        /// hence can be used to start a DB Transaction and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction either gets committed or
        /// rolled back (depending on the value of <paramref name="ASubmissionOK"/>) and the DB Connection gets closed
        /// if a separate DB Connection got indeed opened, otherwise the DB Connection is left open.
        /// </summary>
        /// <param name="AContext">Context in which the Method runs (passed as Name to a newly established DB Connection
        /// (if one indeed needs to be established) and as Name to the DB Transaction, too.</param>
        /// <param name="ATransaction">DB Transaction that got started inside this Method. <em>WARNING: </em>
        /// This either gets rolled back / gets committed automatically after the C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/> got executed - depending on the value of the
        /// <paramref name="ASubmissionOK"/> flag!</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) gets issued.
        /// </param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void SimpleAutoTransactionWrapper(string AContext, out TDBTransaction ATransaction,
            ref bool ASubmissionOK,
            Action AEncapsulatedDBAccessCode)
        {
            TDataBase DBConnectionObj;
            bool SeparateDBConnectionEstablished;

            // Start a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
            // hence can be used to start a DB Transaction.
            ATransaction = DBAccess.BeginTransactionOnIdleDBAccessObj(IsolationLevel.ReadCommitted, out DBConnectionObj,
                out SeparateDBConnectionEstablished, AContext + " DB Connection", AContext + " DB Transaction");

            // Automatic handling of a Read-only DB Transaction - and also the closing the DB Connection if one was
            // established in the call above!
            DBAccess.AutoTransaction(ref ATransaction, SeparateDBConnectionEstablished, ref ASubmissionOK,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// Starts a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
        /// hence can be used to start a DB Transaction and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction either gets committed or
        /// rolled back (depending on the value of <paramref name="ASubmitChangesResult"/>) and the DB Connection
        /// gets closed if a separate DB Connection got indeed opened, otherwise the DB Connection is left open.
        /// </summary>
        /// <param name="AContext">Context in which the Method runs (passed as Name to a newly established DB Connection
        /// (if one indeed needs to be established) and as Name to the DB Transaction, too.</param>
        /// <param name="ATransaction">DB Transaction that got started inside this Method. <em>WARNING: </em>
        /// This either gets rolled back / gets committed automatically after the C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/> got executed - depending on the value of
        /// <paramref name="ASubmitChangesResult"/>!</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when true) or Rollback (when false) gets issued.
        /// </param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void SimpleAutoTransactionWrapper(string AContext, out TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult,
            Action AEncapsulatedDBAccessCode)
        {
            TDataBase DBConnectionObj;
            bool SeparateDBConnectionEstablished;

            // Start a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
            // hence can be used to start a DB Transaction.
            ATransaction = DBAccess.BeginTransactionOnIdleDBAccessObj(IsolationLevel.ReadCommitted, out DBConnectionObj,
                out SeparateDBConnectionEstablished, AContext + " DB Connection", AContext + " DB Transaction");

            // Automatic handling of a Read-only DB Transaction - and also the closing the DB Connection if one was
            // established in the call above!
            DBAccess.AutoTransaction(ref ATransaction, SeparateDBConnectionEstablished, ref ASubmitChangesResult,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// Starts a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
        /// hence can be used to start a DB Transaction and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction either gets committed or
        /// rolled back (depending on the value of <paramref name="ASubmissionOK"/>) and the DB Connection gets closed
        /// if a separate DB Connection got indeed opened, otherwise the DB Connection is left open.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="AContext">Context in which the Method runs (passed as Name to a newly established DB Connection
        /// (if one indeed needs to be established) and as Name to the DB Transaction, too.</param>
        /// <param name="ATransaction">DB Transaction that got started inside this Method. <em>WARNING: </em>
        /// This either gets rolled back / gets committed automatically after the C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/> got executed - depending on the value of the
        /// <paramref name="ASubmissionOK"/> flag!</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) gets issued.
        /// </param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void SimpleAutoTransactionWrapper(IsolationLevel AIsolationLevel, string AContext,
            out TDBTransaction ATransaction, ref bool ASubmissionOK,
            Action AEncapsulatedDBAccessCode)
        {
            TDataBase DBConnectionObj;
            bool SeparateDBConnectionEstablished;

            // Start a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
            // hence can be used to start a DB Transaction.
            ATransaction = DBAccess.BeginTransactionOnIdleDBAccessObj(AIsolationLevel, out DBConnectionObj,
                out SeparateDBConnectionEstablished, AContext + " DB Connection", AContext + " DB Transaction");

            // Automatic handling of a Read-only DB Transaction - and also the closing the DB Connection if one was
            // established in the call above!
            DBAccess.AutoTransaction(ref ATransaction, SeparateDBConnectionEstablished, ref ASubmissionOK,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// Starts a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
        /// hence can be used to start a DB Transaction and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction either gets committed or
        /// rolled back (depending on the value of <paramref name="ASubmitChangesResult"/>) and the DB Connection
        /// gets closed if a separate DB Connection got indeed opened, otherwise the DB Connection is left open.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="AContext">Context in which the Method runs (passed as Name to a newly established DB Connection
        /// (if one indeed needs to be established) and as Name to the DB Transaction, too.</param>
        /// <param name="ATransaction">DB Transaction that got started inside this Method. <em>WARNING: </em>
        /// This either gets rolled back / gets committed automatically after the C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/> got executed - depending on the value of
        /// <paramref name="ASubmitChangesResult"/>!</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when true) or Rollback (when false) gets issued.
        /// </param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void SimpleAutoTransactionWrapper(IsolationLevel AIsolationLevel, string AContext,
            out TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult,
            Action AEncapsulatedDBAccessCode)
        {
            TDataBase DBConnectionObj;
            bool SeparateDBConnectionEstablished;

            // Start a DB Transaction on a TDataBase instance that has currently not got a DB Transaction running and
            // hence can be used to start a DB Transaction.
            ATransaction = DBAccess.BeginTransactionOnIdleDBAccessObj(AIsolationLevel, out DBConnectionObj,
                out SeparateDBConnectionEstablished, AContext + " DB Connection", AContext + " DB Transaction");

            // Automatic handling of a Read-only DB Transaction - and also the closing the DB Connection if one was
            // established in the call above!
            DBAccess.AutoTransaction(ref ATransaction, SeparateDBConnectionEstablished, ref ASubmitChangesResult,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not! Also, this Method closes the DB Connection when
        /// <paramref name="ACloseSeparateDBConnection"/> is true!
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="ACloseSeparateDBConnection">Set to true if the DB Connection that
        /// <paramref name="ATransaction"/> got started on should be closed.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void AutoReadTransaction(ref TDBTransaction ATransaction, bool ACloseSeparateDBConnection,
            Action AEncapsulatedDBAccessCode)
        {
            try
            {
                ATransaction.DataBaseObj.AutoReadTransaction(ref ATransaction, AEncapsulatedDBAccessCode);
            }
            finally
            {
                if (ACloseSeparateDBConnection)
                {
                    ATransaction.DataBaseObj.CloseDBConnection();
                }
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/> and handles the Committing / Rolling Back
        /// of that DB Transaction automatically, depending whether an Exception occured (Rollback always issued!)
        /// and on the value of <paramref name="ASubmissionOK"/>. Also, this Method closes the DB Connection when
        /// <paramref name="ACloseSeparateDBConnection"/> is true!
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="ACloseSeparateDBConnection">Set to true if the DB Connection that
        /// <paramref name="ATransaction"/> got started on should be closed.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void AutoTransaction(ref TDBTransaction ATransaction, bool ACloseSeparateDBConnection,
            ref bool ASubmissionOK, Action AEncapsulatedDBAccessCode)
        {
            try
            {
                ATransaction.DataBaseObj.AutoTransaction(ref ATransaction, ref ASubmissionOK,
                    AEncapsulatedDBAccessCode);
            }
            finally
            {
                if (ACloseSeparateDBConnection)
                {
                    ATransaction.DataBaseObj.CloseDBConnection();
                }
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/> and handles the Committing / Rolling Back
        /// of that DB Transaction automatically, depending whether an Exception occured (Rollback always issued!)
        /// and on the value of <paramref name="ASubmitChangesResult"/>. Also, this Method closes the DB Connection when
        /// <paramref name="ACloseSeparateDBConnection"/> is true!
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="ACloseSeparateDBConnection">Set to true if the DB Connection that
        /// <paramref name="ATransaction"/> got started on should be closed.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void AutoTransaction(ref TDBTransaction ATransaction, bool ACloseSeparateDBConnection,
            ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            try
            {
                ATransaction.DataBaseObj.AutoTransaction(ref ATransaction, ref ASubmitChangesResult,
                    AEncapsulatedDBAccessCode);
            }
            finally
            {
                if (ACloseSeparateDBConnection)
                {
                    ATransaction.DataBaseObj.CloseDBConnection();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// every database system that works for OpenPetra has to implement these functions
    /// </summary>
    public interface IDataBaseRDBMS
    {
        /// <summary>
        /// Creates a connection to a RDBMS, but does not open it yet.
        /// </summary>
        /// <param name="AServer"></param>
        /// <param name="APort"></param>
        /// <param name="ADatabaseName"></param>
        /// <param name="AUsername"></param>
        /// <param name="APassword"></param>
        /// <param name="AConnectionString"></param>
        /// <param name="AStateChangeEventHandler"></param>
        /// <returns>Instantiated object (derived from <see cref="DbConnection" />) - its actual Type depends
        /// on the RDBMS that we are connected to at runtime!</returns>
        DbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler);

        /// init the connection after it was opened
        void InitConnection(DbConnection AConnection);

        /// <summary>
        /// this is for special Exceptions that are specific to the database
        /// they are converted to a string message for logging
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns></returns>
        bool LogException(Exception AException, ref string AErrorMessage);

        /// <summary>
        /// Formats a SQL query for a specific RDBMS.
        /// Put the Schema specifier in front of table names! Format: PUB_*
        /// (eg. PUB_p_partner).
        /// </summary>
        /// <remarks>
        /// Always use ANSI SQL-92 commands that are understood by all RDBMS
        /// systems that should be supported - this does no 'translation' of the
        /// SQL commands!
        /// </remarks>
        /// <param name="ASqlQuery">SQL query</param>
        /// <returns>SQL query that is formatted for a specific RDBMS.
        /// </returns>
        String FormatQueryRDBMSSpecific(String ASqlQuery);

        /// <summary>
        /// convert the ODBC
        /// </summary>
        /// <param name="AParameterArray"></param>
        /// <param name="ASqlStatement"></param>
        /// <returns></returns>
        DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement);

        /// <summary>
        /// Creates a <see cref="DbCommand" /> object.
        /// This formats the sql query for the database, and transforms the parameters.
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns>Instantiated object (derived from <see cref="DbCommand" />) - its actual Type depends
        /// on the RDBMS that we are connected to at runtime!</returns>
        DbCommand NewCommand(ref string ACommandText, DbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction);

        /// <summary>
        /// Creates a <see cref="DbDataAdapter" /> object.
        /// </summary>
        /// <returns>Instantiated object (derived from <see cref="DbDataAdapter" />) - its actual Type depends
        /// on the RDBMS that we are connected to at runtime!</returns>
        DbDataAdapter NewAdapter();

        /// <summary>
        /// Fills a DbDataAdapter that was created with the <see cref="NewAdapter" /> Method.
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        void FillAdapter(DbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName);

        /// <summary>
        /// Fills a DbDataAdapter that was created with the <see cref="NewAdapter" /> Method.
        /// </summary>
        /// <remarks>Overload of FillAdapter, just for one table.</remarks>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataTable"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        void FillAdapter(DbDataAdapter TheAdapter,
            ref DataTable AFillDataTable,
            Int32 AStartRecord,
            Int32 AMaxRecords);

        /// <summary>
        /// some databases have some problems with certain Isolation levels
        /// </summary>
        /// <param name="AIsolationLevel"></param>
        /// <returns>true if isolation level was modified</returns>
        bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel);

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase);

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase);

        /// <summary>
        /// restart a sequence with the given value
        /// </summary>
        void RestartSequence(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, Int64 ARestartValue);

        /// <summary>
        /// Clears (empties) the Connection Pool that a RDBMS driver provides for all connections *that were created using
        /// the Connection String that is associated with <paramref name="ADBConnection"/>*. In case an RDBMS type doesn't
        /// provide a Connection Pool nothing is done and the Method returns no error.
        /// </summary>
        /// <remarks>
        /// THERE IS NORMALLY NO NEED TO EXECUTE THIS METHOD - IN FACT THIS METHOD SHOULD NOT GET CALLED as it will have a
        /// negative performance impact when subsequent DB Connections are opened! Use this Method only for 'unit-testing'
        /// DB Connection-related issues (such as that DB Connections are really closed when they ought to be).
        /// </remarks>
        void ClearConnectionPool(DbConnection ADBConnection);
    }

    /// <summary>
    /// Contains functions that open and close the connection to the DB, allow
    /// execution of SQL statements and creation of DB Transactions.
    /// It is designed to support connections to different kinds of databases;
    /// there needs to be an implementation of the interface IDataBaseRDBMS to support an RDBMS.
    ///
    /// Always use ANSI SQL-92 commands that are understood by all RDBMS
    ///   systems that should be supported - TDataBase does no 'translation' of the
    ///   SQL commands!
    ///   The TDataBase class is the only Class that a developer needs to deal with
    ///   when accessing DB's! (The TDBConnection class is a 'low-level' class that
    ///   is intended to be used only by the TDataBase class.)
    ///   Due to the limitations of native ODBC drivers, only one DataTable is ever
    ///   returned when you call DbDataAdapter.FillSchema. This is true even when
    ///   executing SQL batch statements from which multiple DataTable objects would
    ///   be expected! TODO: this comment needs revising, with native drivers
    /// </summary>
    public class TDataBase
    {
        private const string StrNestedTransactionProblem = "Nested DB Transaction problem details:  *Previously* started " +
                                                           "DB Transaction Properties: Valid: {0}, IsolationLevel: {1}, Reused: {2}; it got started on Thread {3} in AppDomain '{4}'.  "
                                                           +
                                                           "The attempt to begin a DB Transaction NOW occured on Thread {5} in AppDomain '{6}.'   " +
                                                           "The StackTrace of the *previously* started DB Transaction is as follows:\r\n  PREVIOUS Stracktrace: {7}\r\n  CURRENT Stracktrace: {8}";

        /// <summary>An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies a DB Connection once it
        /// gets created. It is used for internal 'sanity checks'. It also gets logged and hence can aid debugging (also useful for
        /// Unit Testing).</summary>
        private System.Guid FConnectionIdentifier;

        /// <summary>References an (open) DB connection.</summary>
        private DbConnection FSqlConnection;

        /// <summary>Waiting time for 'Coordinated' (=Thread-safe) DB Access (in milliseconds).</summary>
        private int FWaitingTimeForCoordinatedDBAccess;

        /// <summary>
        /// Ensures that no concurrent requests are sent to the RDBMS (which could otherwise happen through client- and/or
        /// server-side multi-threading)! (Semaphores allow one to limit the number of Threads that can access a resource
        /// concurrently.)
        /// </summary>
        private SemaphoreSlim FCoordinatedDBAccess = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Tells whether the DB connection is ready to accept commands or whether it is busy.
        /// </summary>
        /// <remarks>The FConnectionReady variable must never be inquired directly, but only through
        /// calling ConnectionReady()! (See remarks for <see cref="OnStateChangedHandler" />.)</remarks>
        private bool FConnectionReady = false;

        /// <summary>References the type of RDBMS that we are currently connected to.</summary>
        private TDBType FDbType;

        /// <summary>Used for guaranteeing Thread Safety when acquiring a DB Connection.</summary>
        private readonly object FSqlConnectionAcquirationLock = new object();

        /// store credentials to be able to login again after closed db connection
        private string FDsnOrServer;
        /// store credentials to be able to login again after closed db connection
        private string FDBPort;
        /// store credentials to be able to login again after closed db connection
        private string FDatabaseName;
        /// store credentials to be able to login again after closed db connection
        private string FUsername;
        /// store credentials to be able to login again after closed db connection
        private string FPassword;
        /// store credentials to be able to login again after closed db connection
        private string FConnectionString;
        /// Name of the DB connection (optional). It gets logged and hence can aid debugging (also useful for Unit Testing).
        private string FConnectionName;
        /// <summary>Thread that the (most recent) DB Connection was established on.</summary>
        private Thread FThreadThatConnectionWasEstablishedOn;

        /// <summary>Reference to the specific database functions which can be different for each RDBMS.</summary>
        private IDataBaseRDBMS FDataBaseRDBMS;

        /// <summary>Tracks the last DB action. Gets updated with every creation of a Command and through various other
        /// DB actions.</summary>
        private DateTime FLastDBAction;

        /// <summary>References the current Transaction, if there is any.</summary>
        private TDBTransaction FTransaction;

        /// <summary>Tells whether the next Command that is sent to the DB should be a 'prepared' Command.</summary>
        /// <remarks>Automatically reset to false once the Command has been executed against the DB!</remarks>
        private bool FPrepareNextCommand = false;

        /// <summary>Sets a timeout (in seconds) for the next Command that is sent to the
        /// DB that is different from the default timeout for a Command (eg. 20s for a
        /// NpgsqlCommand).</summary>
        /// <remarks>Automatically reset to -1 once the Command has been executed against the DB!</remarks>
        private int FTimeoutForNextCommand = -1;

        /// <summary>
        /// this is different from the SQL user name, which is usually the same for the whole server.
        /// This is specific for the user id from table s_user
        /// </summary>
        private string FUserID = string.Empty;

        private static bool FCheckedDatabaseVersion = false;

        /// <summary>
        /// Delegate that can optionally be passed to Method <see cref="SelectUsingDataAdapterMulti"/>. It will get called
        /// every 'n' records (where n is specified with the "AProgressUpdateEveryNRecs" Argument of that Method) while multiple
        /// Parameterised Query executions take place.
        /// </summary>
        /// <param name="ANumberOfProcessedParameterVariations"></param>
        public delegate bool MultipleParamQueryProgressUpdateDelegate(Int32 ANumberOfProcessedParameterVariations);

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// The Database type will be specified only when one of the <c>EstablishDBConnection</c>
        /// Methods gets called
        /// </summary>
        public TDataBase() : base()
        {
            FWaitingTimeForCoordinatedDBAccess = System.Convert.ToInt32(
                TAppSettingsManager.GetValue("Server.DBWaitingTimeForCoordinatedDBAccess", "3000"));

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES)
            {
                TLogging.Log(String.Format("Server.DBWaitingTimeForCoordinatedDBAccess (in milliseconds): {0}",
                        FWaitingTimeForCoordinatedDBAccess));
            }
        }

        /// <summary>
        /// Constructor that specifies which Database type will be used with
        /// this Instance of <see cref="TDataBase" />.
        /// </summary>
        /// <param name="ADBType">Type of RDBMS (Relational Database Management System)</param>
        public TDataBase(TDBType ADBType) : base()
        {
            FDbType = ADBType;
        }

        #endregion

        #region Properties

        /// <summary>Returns the type of the RDBMS that the current Instance of
        /// <see cref="TDataBase" /> is connect to.</summary>
        public String DBType
        {
            get
            {
                return FDbType.ToString("G");
            }
        }

        /// <summary>Tells whether it's save to execute any SQL command on the DB. It is
        /// updated when the DB connection's State changes.</summary>
        public bool ConnectionOK
        {
            get
            {
                return ConnectionReady(true);
            }
        }

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies a DB Connection once it got created.
        /// It is used for internal 'sanity checks'. It also gets logged and hence can aid debugging (also useful for
        /// Unit Testing).
        /// </summary>
        public System.Guid ConnectionIdentifier
        {
            get
            {
                return FConnectionIdentifier;
            }
        }

        /// <summary>
        /// Thread that the (most recent) DB Connection was established on.
        /// </summary>
        internal Thread ThreadThatConnectionWasEstablishedOn
        {
            get
            {
                return FThreadThatConnectionWasEstablishedOn;
            }
        }

        /// <summary>
        /// Name of the DB connection (optional). It gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        public string ConnectionName
        {
            get
            {
                return FConnectionName;
            }
        }

        /// <summary>Tells when the last Database action was carried out by the caller.</summary>
        public DateTime LastDBAction
        {
            get
            {
                WaitForCoordinatedDBAccess();

                try
                {
                    return FLastDBAction;
                }
                finally
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>Waiting time for 'Co-ordinated' (=Thread-safe) DB Access (in milliseconds).</summary>
        /// <remarks>Gets set from server.config file setting 'Server.DBWaitingTimeForCoordinatedDBAccess'. If that isn't
        /// present, then '3000' is the default (=3 seconds).</remarks>
        public int WaitingTimeForCoordinatedDBAccess
        {
            get
            {
                return FWaitingTimeForCoordinatedDBAccess;
            }
        }

        /// <summary>
        /// The current Transaction, if there is any.
        /// </summary>
        public TDBTransaction Transaction
        {
            get
            {
                WaitForCoordinatedDBAccess();

                try
                {
                    return FTransaction;
                }
                finally
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// The current Transaction, if there is any.  <em>WARNING: Must only ever be inquired from a code block that
        /// called <see cref="WaitForCoordinatedDBAccess"/> as otherwise the value is unreliable!!!</em>
        /// </summary>
        internal TDBTransaction TransactionNonThreadSafe
        {
            get
            {
                return FTransaction;
            }
        }

        /// <summary>
        /// store the value of the current s_user.
        /// not to be confused with the sql user
        /// </summary>
        public string UserID
        {
            get
            {
                WaitForCoordinatedDBAccess();

                try
                {
                    return FUserID;
                }
                finally
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            set
            {
                WaitForCoordinatedDBAccess();

                try
                {
                    FUserID = value;
                }
                finally
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        #endregion

        /// <summary>
        /// Establishes (opens) a DB connection to a specified RDBMS.
        /// </summary>
        /// <param name="ADataBaseType">Type of the RDBMS to connect to. At the moment only PostgreSQL is officially supported.</param>
        /// <param name="ADsnOrServer">In case of an ODBC Connection: DSN (Data Source Name). In case of a PostgreSQL connection: Server.</param>
        /// <param name="ADBPort">In case of a PostgreSQL connection: port that the db server is running on.</param>
        /// <param name="ADatabaseName">the database to connect to</param>
        /// <param name="AUsername">User which should be used for connecting to the DB server</param>
        /// <param name="APassword">Password of the User which should be used for connecting to the DB server</param>
        /// <param name="AConnectionString">If this is not empty, it is prefered over the Dsn and Username and Password</param>
        /// <param name="AConnectionName">Name of the DB Connection (optional). It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).</param>
        /// <exception cref="EDBConnectionNotAvailableException">Thrown when a connection cannot be established</exception>
        public void EstablishDBConnection(TDBType ADataBaseType,
            String ADsnOrServer,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString,
            String AConnectionName = "")
        {
            EstablishDBConnection(ADataBaseType, ADsnOrServer, ADBPort, ADatabaseName, AUsername, APassword,
                AConnectionString, true, AConnectionName);
        }

        /// <summary>
        /// Establishes (opens) a DB connection to a specified RDBMS.
        /// </summary>
        /// <param name="ADataBaseType">Type of the RDBMS to connect to. At the moment only PostgreSQL is officially supported.</param>
        /// <param name="ADsnOrServer">In case of an ODBC Connection: DSN (Data Source Name). In case of a PostgreSQL connection: Server.</param>
        /// <param name="ADBPort">In case of a PostgreSQL connection: port that the db server is running on.</param>
        /// <param name="ADatabaseName">the database to connect to</param>
        /// <param name="AUsername">User which should be used for connecting to the DB server</param>
        /// <param name="APassword">Password of the User which should be used for connecting to the DB server</param>
        /// <param name="AConnectionString">If this is not empty, it is prefered over the Dsn and Username and Password</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="AConnectionName">Name of the DB Connection (optional). It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).</param>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown when a connection cannot be established</exception>
        private void EstablishDBConnection(TDBType ADataBaseType,
            String ADsnOrServer,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString,
            bool AMustCoordinateDBAccess,
            String AConnectionName = "")
        {
            bool ExceptionCausedByUnavailableDBConn;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                FDbType = ADataBaseType;
                FDsnOrServer = ADsnOrServer;
                FDBPort = ADBPort;
                FDatabaseName = ADatabaseName;
                FUsername = AUsername;
                FPassword = APassword;
                FConnectionString = AConnectionString;
                FConnectionName = AConnectionName;

                switch (FDbType)
                {
                    case TDBType.PostgreSQL:
                        FDataBaseRDBMS = (IDataBaseRDBMS) new TPostgreSQL();
                        break;

                    case TDBType.MySQL:
                        FDataBaseRDBMS = (IDataBaseRDBMS) new TMySQL();
                        break;

                    case TDBType.SQLite:
                        FDataBaseRDBMS = (IDataBaseRDBMS) new TSQLite();
                        break;

                    case TDBType.ProgressODBC:
                        FDataBaseRDBMS = (IDataBaseRDBMS) new TProgressODBC();
                        break;

                    default:
                        throw new ArgumentException(String.Format("EstablishDBConnection cannot deal with ADataBaseType " + "" +
                            "'{0}' that got passed in", ADataBaseType));
                }

                if (ConnectionReady(false))
                {
                    TLogging.Log("Error establishing connection to Database Server" +
                        GetDBConnectionIdentifierInternal() + ": connection is already open!");
                    throw new EDBConnectionIsAlreadyOpenException(
                        FSqlConnection != null ? "Connection State: " + FSqlConnection.State.ToString("G") : "FSqlConnection is null");
                }

                // We take out a 'lock' on the following code sequence because only *this* guarantees complete thread safety -
                // though we have a call to 'WaitForCoordinatedDBAccess' above this could be either not working for some
                // reason - and also this check can get by-passed by passing in 'false' for the 'AMustCoordinateDBAccess'
                // Argument.
                lock (FSqlConnectionAcquirationLock)
                {
                    if (FSqlConnection == null)
                    {
                        FConnectionIdentifier = System.Guid.NewGuid();

                        FSqlConnection = TDBConnection.GetConnection(
                            FDataBaseRDBMS,
                            ADsnOrServer,
                            ADBPort,
                            ADatabaseName,
                            AUsername,
                            ref APassword,
                            ref FConnectionString,
                            new StateChangeEventHandler(this.OnStateChangedHandler));

                        if (FSqlConnection == null)
                        {
                            throw new EDBConnectionNotEstablishedException();
                        }
                    }
                    else
                    {
                        throw new EDBConnectionWasAlreadyEstablishedException();
                    }
                }

                try
                {
                    // always log to console and log file, which database we are connecting to.
                    // see https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=156
                    TLogging.Log("    Connecting to database " + ADataBaseType +
                        GetDBConnectionIdentifierInternal() + ": " +
                        TDBConnection.GetConnectionStringWithHiddenPwd(FConnectionString) + " " +
                        GetThreadAndAppDomainCallInfoForDBConnectionEstablishmentAndDisconnection());

                    FSqlConnection.Open();
                    FDataBaseRDBMS.InitConnection(FSqlConnection);

                    FThreadThatConnectionWasEstablishedOn = Thread.CurrentThread;
                    FLastDBAction = DateTime.Now;
                }
                catch (Exception Exc)
                {
                    if (FSqlConnection != null)
                    {
                        FSqlConnection.Dispose();
                    }

                    FSqlConnection = null;

                    ExceptionCausedByUnavailableDBConn = TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionServerSide(Exc);

                    if (!ExceptionCausedByUnavailableDBConn)
                    {
                        LogException(Exc,
                            String.Format("Exception occured while establishing a connection to the Database (Server). DB Type: {0}", FDbType));
                    }
                    else
                    {
                        TLogging.Log("    FAILED to establish DB Connection because the DB (Server) is unavailable.");
                    }

                    throw new EDBConnectionNotEstablishedException(TDBConnection.GetConnectionStringWithHiddenPwd(
                            FConnectionString) + ' ' + Exc.ToString(), Exc, !ExceptionCausedByUnavailableDBConn);
                }

                // only check database version once when working with multiple connections
                if (!FCheckedDatabaseVersion)
                {
                    CheckDatabaseVersion();
                    FCheckedDatabaseVersion = true;
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Application and Database should have the same version, otherwise all sorts of things can go wrong.
        /// this is specific to the OpenPetra database, for all other databases it will just ignore the database version check
        /// </summary>
        private void CheckDatabaseVersion()
        {
            TDBTransaction ReadTransaction = null;
            DataTable Tbl = null;

            if (TAppSettingsManager.GetValue("action", string.Empty, false) == "patchDatabase")
            {
                // we want to upgrade the database, so don't check for the database version
                return;
            }

            BeginAutoReadTransaction(IsolationLevel.ReadCommitted, -1, ref ReadTransaction, false,
                delegate
                {
                    // now check if the database is 'up to date'; otherwise run db patch against it
                    Tbl = SelectDTInternal(
                        "SELECT s_default_value_c FROM PUB_s_system_defaults WHERE s_default_code_c = 'CurrentDatabaseVersion'",
                        "Temp", ReadTransaction, new OdbcParameter[0], false);
                });

            if (Tbl.Rows.Count == 0)
            {
                return;
            }
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <param name="ASuppressThreadCompatibilityCheck">Set to true to suppress a check whether the Thread that
        /// calls this Method is the Thread that established the DB Connection. <em>WARNING:
        /// To be set to true only by 1) Method 'Ict.Petra.Server.App.Core.CloseDBConnection()' because there it
        /// will occur if not set to true because the Client Disconnection occurs on a separately started Thread, and
        /// that Thread will be different from the Thread that established the 'globally available' DB Connection
        /// (DBAccess.GDBAccessObj) for the Client's AppDomain; 2) Method 'CloseDBPollingConnection' of TServerManager
        /// as this will be called when the Server is shutting down and at this stage we want to unconditionally
        /// close the Server's DB Polling Connection; 3) Method 'EstablishDBPollingConnection' of TServerManager
        /// because there we want to at least try to close a broken DB Connection and that happens on a different
        /// Thread!!!!</em></param>
        /// <exception cref="EDBConnectionNotAvailableException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        public void CloseDBConnection(bool ASuppressThreadCompatibilityCheck = false)
        {
            WaitForCoordinatedDBAccess();

            try
            {
                if ((FSqlConnection != null) && (FSqlConnection.State != ConnectionState.Closed))
                {
                    CloseDBConnectionInternal(ASuppressThreadCompatibilityCheck);
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <param name="ASuppressThreadCompatibilityCheck">Set to true to suppress a check whether the Thread that
        /// calls this Method is the Thread that established the DB Connection. <em>WARNING:
        /// To be set to true only by 1) Method 'Ict.Petra.Server.App.Core.CloseDBConnection()' because there it
        /// will occur if not set to true because the Client Disconnection occurs on a separately started Thread, and
        /// that Thread will be different from the Thread that established the 'globally available' DB Connection
        /// (DBAccess.GDBAccessObj) for the Client's AppDomain; 2) Method 'CloseDBPollingConnection' of TServerManager
        /// as this will be called when the Server is shutting down and at this stage we want to unconditionally
        /// close the Server's DB Polling Connection; 3) Method 'EstablishDBPollingConnection' of TServerManager
        /// because there we want to at least try to close a broken DB Connection and that happens on a different
        /// Thread!!!!</em></param>
        /// <exception cref="EDBConnectionNotAvailableException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        private void CloseDBConnectionInternal(bool ASuppressThreadCompatibilityCheck = false)
        {
            bool RunningDBTransactionThreadIsCompatible;

            if (ConnectionReady(false))
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("    Closing Database connection..." + GetDBConnectionIdentifier());
                }

                if (!ASuppressThreadCompatibilityCheck)
                {
                    if (!CheckEstablishedDBConnectionThreadIsCompatible(false))
                    {
                        var Exc1 =
                            new EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException(
                                "TDataBase.CloseDBConnectionInternal would close established DB Connection that got " +
                                "established on a different Thread and this isn't supported (ADO.NET provider isn't thread-safe!)",
                                ThreadingHelper.GetThreadIdentifier(ThreadThatConnectionWasEstablishedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                        TLogging.Log(Exc1.ToString());

                        throw Exc1;
                    }
                }

                // If a DB Transaction is still open and it hasn't been committed or rolled back yet
                // then issue a Rollback (this is cleaner than just closing the DB Transaction).
                // Note: DB Transactions must be rolled back (and get committed) *only* in the Thread in which they were
                // created (the RollbackTransaction Method enforces this)!
                if ((FTransaction != null)
                    && (FTransaction.Valid))
                {
                    RunningDBTransactionThreadIsCompatible = CheckRunningDBTransactionThreadIsCompatible(false);

                    if (!ASuppressThreadCompatibilityCheck)
                    {
                        // Multi-threading 'Sanity Check':
                        // Check if the current Thread is the same Thread that the current Transaction was started on:
                        // if not, throw Exception!
                        if (!RunningDBTransactionThreadIsCompatible)
                        {
                            var Exc2 =
                                new EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(
                                    "TDataBase.CloseDBConnectionInternal would roll back still running DB Transaction that got " +
                                    "started on a different Thread and this isn't supported (ADO.NET provider isn't thread-safe!)",
                                    ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                    ThreadingHelper.GetCurrentThreadIdentifier());

                            TLogging.Log(Exc2.ToString());

                            throw Exc2;
                        }
                    }

                    // We are rolling back a running DB Transaction only if it was established on the same Thread, otherwise
                    // we leave it alone and just close the DB Connection. Skipping the roll-back of a DB Transaction should
                    // be fine when we are closing the DB Connection anyway!
                    // (This guards against getting a EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException
                    // thrown from the RollbackTransaction Method.)
                    // Situation in which such a constellation occurs: if a user had started 'some process in OpenPetra' that
                    // runs for some time and then closes the Client without stopping that process first. Example: XML Reports!
                    if (RunningDBTransactionThreadIsCompatible)
                    {
                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log("TDataBase.CloseDBConnectionInternal:" + GetDBConnectionIdentifier() +
                                " before calling this.RollbackTransaction", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }

                        this.RollbackTransaction(false);

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log("TDataBase.CloseDBConnectionInternal:" + GetDBConnectionIdentifier() +
                                " after calling this.RollbackTransaction", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal:" + GetDBConnectionIdentifier() +
                        " before calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain '" +
                        AppDomain.CurrentDomain.FriendlyName + "'",
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                TDBConnection.CloseDBConnection(FSqlConnection, GetDBConnectionIdentifierInternal());

                // Remark: TDBConnection.CloseDBConnection already calls Dispose() on FSqlConnection so we don't need to do it here
                // and we can simply set it to null.
                FSqlConnection = null;

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: closed DB Connection." + GetDBConnectionIdentifier());
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: after calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain '"
                        +
                        AppDomain.CurrentDomain.FriendlyName + "'" + GetDBConnectionIdentifier(),
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FLastDBAction = DateTime.Now;
                FDatabaseName = null;
            }
            else
            {
                throw new EDBConnectionNotAvailableException();
            }
        }

        /// <summary>
        /// Returns the number of open DB Connections that the RDBMS reports.
        /// </summary>
        /// <param name="ADBType">The RDBMS which should return the number of open DB Connections.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /> object, or null (default = null).
        /// If an instance gets passed in then the Method executes DB commands with the <see cref="TDataBase"/> instance
        /// that <paramref name="ATransaction"/> got started with.</param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null).
        /// If <paramref name="ATransaction"/> is null and null gets passed for <paramref name="ADataBase" />, too,
        /// then the Method executes DB commands with the 'globally available' <see cref="DBAccess.GDBAccessObj" />
        /// instance, otherwise with the instance that gets passed in with  <paramref name="ADataBase" /> if
        /// <paramref name="ATransaction"/> is null. (The DB Connection instance of <paramref name="ATransaction" />
        /// takes precedece over any DB Connection instance that might get passed with <paramref name="ADataBase" />.)</param>
        /// <remarks>At present the implementation only works for the PostgreSQL RDBMS!!!</remarks>
        /// <returns>Number of open DB Connections that the PostgreSQL RDBMS reports, or -1 for any other RDBMS.</returns>
        public static int GetNumberOfDBConnections(TDBType ADBType, TDBTransaction ATransaction = null,
            TDataBase ADataBase = null)
        {
            int ReturnValue = -1;

            string SqlQueryStr;

            if (ADBType == TDBType.PostgreSQL)
            {
                SqlQueryStr = String.Format("SELECT SUM(numbackends) FROM pg_stat_database where datname = '{0}'",
                    TAppSettingsManager.GetValue("Server.DBName"));

                if (ATransaction == null)
                {
                    TDataBase db = DBAccess.GetDBAccessObj(ADataBase);

                    if (db != null)
                    {
                        ReturnValue = Convert.ToInt32(db.ExecuteScalar(
                            SqlQueryStr, IsolationLevel.Serializable));
                    }
                }
                else
                {
                    TDataBase db = DBAccess.GetDBAccessObj(ATransaction);

                    if (db != null)
                    {
                        ReturnValue = Convert.ToInt32(db.ExecuteScalar(
                            SqlQueryStr, ATransaction));
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns a string containing the ConnectionIdentifier. If a ConnectionName was assigned when the DB connection
        /// was opened it is included, too.
        /// </summary>
        /// <returns>String containing the ConnectionIdentifier. If a ConnectionName was assigned when the DB connection
        /// was opened it is included, too.
        /// </returns>
        public string GetDBConnectionIdentifier()
        {
            return ((TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE) ? " (Conn.Identifier: " + ConnectionIdentifier + ")" : String.Empty) +
                   (FConnectionName != String.Empty ? String.Format(" (Connection Name: {0})", FConnectionName) : "");
        }

        /// <summary>
        /// Same as <see cref="GetDBConnectionIdentifier"/> but only returns a string in case a certain DebugLevel is set.
        /// </summary>
        /// <returns>Same as <see cref="GetDBConnectionIdentifier"/> but only returns a string in case a certain
        /// DebugLevel is set.</returns>
        private string GetDBConnectionIdentifierInternal()
        {
            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_DETAILED_CONN_INFO)
            {
                return GetDBConnectionIdentifier();
            }

            return String.Empty;
        }

        /// <summary>
        /// Returns a standardised string containing the name of <paramref name="AConnectionName"/>.
        /// </summary>
        /// <returns>String containing the name of <paramref name="AConnectionName"/>, or <see cref="String.Empty"/>
        /// if no name was assigned.
        /// </returns>
        public static string GetDBConnectionName(string AConnectionName)
        {
            return AConnectionName == null ? "" : String.Format("  (Connection Name: {0})", AConnectionName);
        }

        private void ExtendedLoggingInfoOnHigherDebugLevels(string ALogMessage,
            int AMinimumDebugLevel = DBAccess.DB_DEBUGLEVEL_TRACE, bool AIncludeStackTrace = false)
        {
            if (TLogging.DL < AMinimumDebugLevel)
            {
                return;
            }

            TLogging.Log(ALogMessage + String.Format(" (Logged on Thread {0} in AppDomain '{1}')",
                    ThreadingHelper.GetCurrentThreadIdentifier(), AppDomain.CurrentDomain.FriendlyName) +
                (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES ?
                 GetDBConnectionIdentifier() : "."));

            if (AIncludeStackTrace)
            {
                TLogging.Log("Start of stack trace ->");
                TLogging.LogStackTrace(TLoggingType.ToLogfile);
                TLogging.Log("<- End of stack trace");
            }
        }

        /// <summary>
        /// Returns information about the current Thread and the current AppDomain that can be useful
        /// for logging/debugging of DB Connection Establishment and DB Disconnection.
        /// </summary>
        /// <returns>Information about the current Thread and the current AppDomain in form of a
        /// formatted string.</returns>
        public static string GetThreadAndAppDomainCallInfoForDBConnectionEstablishmentAndDisconnection()
        {
            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_DETAILED_CONN_INFO)
            {
                return Utilities.GetThreadAndAppDomainCallInfo();
            }

            return String.Empty;
        }

        /// <summary>
        /// Clears (empties) *all* the Connection Pools a RDBMS driver provides (irrespecive of the Connection String that is
        /// associated with any connection).  In case an RDBMS type doesn't provide a Connection Pool nothing is done and the
        /// Method returns no error.
        /// </summary>
        /// <remarks>
        /// THERE IS NORMALLY NO NEED TO EXECUTE THIS METHOD - IN FACT THIS METHOD SHOULD NOT GET CALLED as it will have a
        /// negative performance impact when subsequent DB Connections are opened! Use this Method only for 'unit-testing'
        /// DB Connection-related issues (such as that DB Connections are really closed when they ought to be).
        /// </remarks>
        public static void ClearAllConnectionPools()
        {
            TDBConnection.ClearAllConnectionPools();

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("    TDataBase.ClearAllConnectionPools got called.");
            }
        }

        /// <summary>
        /// Clears (empties) the Connection Pool that a RDBMS driver provides. In case an RDBMS type doesn't provide a
        /// Connection Pool nothing is done and the Method returns no error.
        /// </summary>
        /// <remarks>
        /// THERE IS NORMALLY NO NEED TO EXECUTE THIS METHOD - IN FACT THIS METHOD SHOULD NOT GET CALLED as it will have a
        /// negative performance impact when subsequent DB Connections are opened! Use this Method only for 'unit-testing'
        /// DB Connection-related issues (such as that DB Connections are really closed when they ought to be).
        /// </remarks>
        public void ClearConnectionPool()
        {
            TDBConnection.ClearConnectionPool(FDataBaseRDBMS, FSqlConnection);

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("    TDataBase.ClearConnectionPool got called.");
            }
        }

        /// <summary>
        /// Gets the number of open DB connections. This Method is a convenient combination of the
        /// <see cref="ClearAllConnectionPools()" /> and <see cref="GetNumberOfDBConnections" /> Methods to get the
        /// number of currently open DB connections on a RDBMS (using just the latter Method on its own is not reliable
        /// because of connection pooling).
        /// </summary>
        /// <param name="ADBType">Type of RDBMS (Relational Database Management System)</param>
        /// <returns>Number of open DB Connections that the PostgreSQL RDBMS reports, or -1 for any other RDBMS.</returns>
        public static int ClearConnectionPoolAndGetNumberOfDBConnections(TDBType ADBType)
        {
            int ReturnValue = -1;

            // Very important: Ensure that all the ConnectionPools are cleared so we can correctly determine the number of
            // open DB Connections!
            TDataBase.ClearAllConnectionPools();

            // Find out how many DB Connections are open.
            // Note: ReturnValue will be -1 for any RDBMS other than PostgreSQL as getting the number of open DB Connections
            // is currently only implemented for PostgreSQL!
            ReturnValue = TDataBase.GetNumberOfDBConnections(ADBType);

            return ReturnValue;
        }

        /// <summary>
        /// Call this Method to make the next Command that is sent to the DB a 'Prepared' command.
        /// </summary>
        /// <remarks>
        /// <para><see cref="PrepareNextCommand" /> lets you optimise the performance of
        /// frequently used queries. What a RDBMS basically does with a 'Prepared' SQL Command is
        /// that it 'caches' the query plan so that it's used in subsequent calls.
        /// Not supported by all RDBMS, but should just silently fail in case a RDBMS doesn't
        /// support it. PostgreSQL definitely supports it.
        /// </para>
        /// <para><em>IMPORTANT:</em> In the light of co-ordinated DB Access and the possibility of multiple Threads trying to
        /// access the DB at the same time this Method <em>MUST</em> only be called in an area of code that is protected by a
        /// WaitForCoordinatedDBAccess() ... ReleaseCoordinatedDBAccess() 'pair' and that 'protection' needs to include the
        /// execution of the Command (ie. by calling DbDataAdapter.Fill) - otherwise the 'preparation' might well be issued
        /// for the wrong Command! An example where this is done correctly can be seen in the
        /// <see cref="SelectUsingDataAdapter"/> Method.
        /// </para>
        /// </remarks>
        /// <returns>void</returns>
        private void PrepareNextCommand()
        {
            FPrepareNextCommand = true;
        }

        /// <summary>
        /// Call this Method to set a timeout (in seconds) for the next Command that is sent to the
        /// DB that is different from the default timeout for a Command (eg. 20s for a
        /// NpgsqlCommand).
        /// </summary>
        /// <remarks>
        /// <para><em>IMPORTANT:</em> In the light of co-ordinated DB Access and the possibility of multiple Threads trying to
        /// access the DB at the same time this Method <em>MUST</em> only be called in an area of code that is protected by a
        /// WaitForCoordinatedDBAccess() ... ReleaseCoordinatedDBAccess() 'pair' and that 'protection' needs to include the
        /// execution of the Command (ie. by calling DbDataAdapter.Fill) - otherwise the Timeout might well be issued
        /// for the wrong Command! An example where this is done correctly can be seen in the
        /// <see cref="SelectUsingDataAdapter"/> Method.
        /// </para>
        /// </remarks>
        /// <param name="ATimeoutInSec">Timeout (in seconds) for the next Command that is sent to the DB.</param>
        /// <returns>void</returns>
        private void SetTimeoutForNextCommand(int ATimeoutInSec)
        {
            FTimeoutForNextCommand = ATimeoutInSec;
        }

        /// <summary>
        /// Means of getting Cache objects.
        /// </summary>
        /// <returns>A new Instance of an <see cref="TSQLCache" /> Object.</returns>
        public TSQLCache GetCache()
        {
            return new TSQLCache();
        }

        #region Command

        /// <summary>
        /// Returns a DbCommand for a given command text in the context of a
        /// DB transaction. Suitable for parameterised SQL statements.
        /// Allows the passing in of Parameters for the SQL statement
        /// </summary>
        /// <remarks>
        /// <b>Important:</b> Since an object that derives from <see cref="DbCommand" /> is returned you ought to
        /// <em>call .Dispose()</em> on the returned object to release its resouces! (<see cref="DbCommand" /> inherits
        /// from <see cref="System.ComponentModel.Component" />, which implements <see cref="IDisposable" />!)
        /// </remarks>
        /// <param name="ACommandText">Command Text</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or null if the command
        /// should not be enlisted in a transaction.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameter
        /// (including Parameter Value)</param>
        /// <returns>Instantiated object (derived from DbCommand) - its actual Type depends
        /// on the RDBMS that we are connected to at runtime!</returns>
        public DbCommand Command(String ACommandText, TDBTransaction ATransaction, DbParameter[] AParametersArray)
        {
            return Command(ACommandText, ATransaction, true, AParametersArray);
        }

        /// <summary>
        /// Returns a DbCommand for a given command text in the context of a
        /// DB transaction. Suitable for parameterised SQL statements.
        /// Allows the passing in of Parameters for the SQL statement
        /// </summary>
        /// <remarks>
        /// <b>Important:</b> Since an object that derives from <see cref="DbCommand" /> is returned you ought to
        /// <em>call .Dispose()</em> on the returned object to release its resouces! (<see cref="DbCommand" /> inherits
        /// from <see cref="System.ComponentModel.Component" />, which implements <see cref="IDisposable" />!)
        /// </remarks>
        /// <param name="ACommandText">Command Text</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or null if the command
        /// should not be enlisted in a transaction.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameter
        /// (including Parameter Value)</param>
        /// <returns>Instantiated object (derived from DbCommand) - its actual Type depends
        /// on the RDBMS that we are connected to at runtime!</returns>
        private DbCommand Command(String ACommandText, TDBTransaction ATransaction, bool AMustCoordinateDBAccess,
            DbParameter[] AParametersArray)
        {
            DbCommand ObjReturn = null;

            if (AParametersArray == null)
            {
                AParametersArray = new OdbcParameter[0];
            }

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".Command()..." +
                    (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES ?
                     " on Thread " + ThreadingHelper.GetCurrentThreadIdentifier() + " in AppDomain '" +
                     AppDomain.CurrentDomain.FriendlyName + "' " +
                     (ATransaction != null ? ATransaction.GetDBTransactionIdentifier() : "") + GetDBConnectionIdentifier() : ""));
            }

            try
            {
                if (!HasAccess(ACommandText))
                {
                    throw new EAccessDeniedException("Security Violation: Access Permission failed");
                }

                try
                {
                    /* Preprocess ACommandText for `IN (?)' syntax */
                    PreProcessCommand(ref ACommandText, ref AParametersArray);

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".Command: now getting DbCommand(" + ACommandText + ")...");
                    }

                    ObjReturn = FDataBaseRDBMS.NewCommand(ref ACommandText, FSqlConnection, AParametersArray, ATransaction);

                    // Enlist this command in a DB transaction (does not happen if ATransaction is null)
                    if (ATransaction != null)
                    {
                        // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet:
                        // if not, throw Exception!
                        if (!ATransaction.Valid)
                        {
                            var Exc1 = new EDBAttemptingToUseTransactionThatIsInvalidException(
                                "TDataBase.Command called with a DB Transaction that isn't valid",
                                ThreadingHelper.GetThreadIdentifier(ATransaction.ThreadThatTransactionWasStartedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                            TLogging.Log(Exc1.ToString());

                            throw Exc1;
                        }

                        //
                        // Multi-threading and multi-connection 'Sanity Checks'
                        //

                        // Check if the current Thread is the same Thread that ATransaction was started on:
                        // if not, throw Exception!
                        if (!CheckDBTransactionThreadIsCompatible(ATransaction))
                        {
                            var Exc2 = new
                                       EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException(
                                "TDataBase.Command would yield Command on a Thread that is different from the Thread that " +
                                "started the Transaction that got passed in",
                                ThreadingHelper.GetThreadIdentifier(ATransaction.ThreadThatTransactionWasStartedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                            TLogging.Log(Exc2.ToString());

                            throw Exc2;
                        }

                        // Check whether the current DB Connection is different from the DB Connection that the Transaction
                        // that got passed in with ATransaction got started against: if this is so, throw Exception!
                        // Reason for that: Running DB Commands on one DB Connection with a DB Transaction that was started
                        // against another DB Connection isn't supported (because each DB Command is always bound to the
                        // DB Connection that creates it)!
                        if (ConnectionIdentifier != ATransaction.ConnectionIdentifier)
                        {
                            var Exc3 = new
                                       EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException(
                                "TDataBase.Command would yield Command on a Connection that is different from the Connection " +
                                "that the Transaction that got passed in was started against",
                                ATransaction.ConnectionIdentifier.ToString(), ConnectionIdentifier.ToString());

                            TLogging.Log(Exc3.ToString());

                            throw Exc3;
                        }

                        // Check whether the TransactionIdentifier of the current Transaction of TDataBase is different from
                        // the TransactionIdentifier of the Transaction that got passed in with ATransaction: if this is so,
                        // throw Exception!
                        // Reason for that: Running DB Commands on a DB Transaction other than the currently running DB
                        // Transaction is not supported (because parallel DB Transactions are not supported)!
                        if (FTransaction.TransactionIdentifier != ATransaction.TransactionIdentifier)
                        {
                            var Exc4 = new
                                       EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException(
                                "TDataBase.Command would yield Command that would be enlisted in a different DB Transaction " +
                                "(the one that got passed in) than the current DB Transaction",
                                ATransaction.TransactionIdentifier.ToString(), FTransaction.TransactionIdentifier.ToString());

                            TLogging.Log(Exc4.ToString());

                            throw Exc4;
                        }

                        ObjReturn.Transaction = ATransaction.WrappedTransaction;
                    }

                    // if this is a call to Stored Procedure: set command type accordingly
                    if (ACommandText.StartsWith("CALL", true, null))
                    {
                        ObjReturn.CommandType = CommandType.StoredProcedure;
                    }

                    if (FPrepareNextCommand)
                    {
                        ObjReturn.Prepare();
                        FPrepareNextCommand = false;

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log(this.GetType().FullName + ".Command: will 'Prepare' this Command.");
                        }
                    }

                    if (FTimeoutForNextCommand != -1)
                    {
                        /*
                         * Tricky bit: we need to create a new Object (of Type String) that is disassociated
                         * with FTimeoutForNextCommand, because FTimeoutForNextCommand is reset in the next statement!
                         */
                        ObjReturn.CommandTimeout = Convert.ToInt32(FTimeoutForNextCommand.ToString());
                        FTimeoutForNextCommand = -1;

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log(
                                this.GetType().FullName + ".Command: set Timeout for this Command to " + ObjReturn.CommandTimeout.ToString() + ".");
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogExceptionAndThrow(exp, ACommandText, AParametersArray, "Error creating Command. The command was: ");
                }

                FLastDBAction = DateTime.Now;
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return ObjReturn;
        }

        #endregion

        #region Select

        /// <summary>
        /// Returns a <see cref="DataSet" /> containing a <see cref="DataTable" /> with the result of a given SQL
        /// statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="DataSet" /></returns>
        public DataSet Select(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray = null)
        {
            DataSet InputDataSet = new DataSet();
            DataSet ObjReturn;

            ObjReturn = Select(InputDataSet, ASqlStatement, ADataTableName, AReadTransaction, AParametersArray);

            InputDataSet.Dispose();

            return ObjReturn;
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a  given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <param name="AMaxRecords">Maximum number of records that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            System.Int32 AStartRecord,
            System.Int32 AMaxRecords)
        {
            return Select(AFillDataSet, ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0], AStartRecord, AMaxRecords);
        }

        /// <summary>
        /// Puts a <see cref="DataTable" /> with the result of a given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <param name="AMaxRecords">Maximum number of records that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet Select(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray = null,
            System.Int32 AStartRecord = 0,
            System.Int32 AMaxRecords = 0)
        {
            DataSet ObjReturn = null;

            if (AFillDataSet == null)
            {
                throw new ArgumentNullException("AFillDataSet", "AFillDataSet must not be null!");
            }

            if (ADataTableName == String.Empty)
            {
                throw new ArgumentException("A name for the DataTable must be submitted!", "ADataTableName");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".Select()", ASqlStatement, AParametersArray);
            }

            WaitForCoordinatedDBAccess();

            try
            {
                using (DbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, false, AParametersArray))
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(((this.GetType().FullName + ".Select: now filling DbDataAdapter('" + ADataTableName) + "')..."));
                    }

                    using (TheAdapter.SelectCommand)
                    {
                        FDataBaseRDBMS.FillAdapter(TheAdapter, ref AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: finished filling DbDataAdapter(DataTable '" +
                                   ADataTableName) + "'). DT Row Count: " + AFillDataSet.Tables[ADataTableName].Rows.Count.ToString()));
#if WITH_POSTGRESQL_LOGGING
                    NpgsqlEventLog.Level = LogLevel.None;
#endif
                }

                ObjReturn = AFillDataSet;
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB40001 exception in TDatabase.Select", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB23505 exception in TDatabase.Select", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (AFillDataSet.Tables[ADataTableName] != null)
                {
                    DataRow[] BadRows = AFillDataSet.Tables[ADataTableName].GetErrors();

                    if (BadRows.Length > 0)
                    {
                        TLogging.Log("Errors reported in " + ADataTableName + " rows:");

                        foreach (DataRow BadRow in BadRows)
                        {
                            TLogging.Log(BadRow.RowError);
                        }
                    }
                }

                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                if ((ObjReturn != null) && (ObjReturn.Tables[ADataTableName] != null))
                {
                    LogTable(ObjReturn.Tables[ADataTableName]);
                }
            }

            return ObjReturn;
        }

        /// <summary>
        /// Puts a temp <see cref="DataTable" /> with the result of a given SQL statement into an existing
        /// <see cref="DataSet" />.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AFillDataSet">Existing <see cref="DataSet" /></param>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTempTableName">Name that the temp <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <param name="AStartRecord">Start record that should be returned</param>
        /// <param name="AMaxRecords">Maximum number of records that should be returned</param>
        /// <returns>Existing <see cref="DataSet" />, additionally containing the new <see cref="DataTable" /></returns>
        public DataSet SelectToTempTable(DataSet AFillDataSet,
            String ASqlStatement,
            String ADataTempTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray,
            System.Int32 AStartRecord,
            System.Int32 AMaxRecords)
        {
            DataSet ObjReturn = null;

            if (AFillDataSet == null)
            {
                throw new ArgumentNullException("AFillDataSet", "AFillDataSet must not be null!");
            }

            if (ADataTempTableName == String.Empty)
            {
                throw new ArgumentException("ADataTempTableName", "A name for the temporary DataTable must be submitted!");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".SelectToTempTable()", ASqlStatement, AParametersArray);
            }

            WaitForCoordinatedDBAccess();

            try
            {
                using (DbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, false, AParametersArray))
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(((this.GetType().FullName + ".SelectToTempTable: now filling DbDataAdapter('" + ADataTempTableName) + "')..."));
                    }

                    //Make sure that any previous temp table of the same name is removed first!
                    if (AFillDataSet.Tables.Contains(ADataTempTableName))
                    {
                        AFillDataSet.Tables.Remove(ADataTempTableName);
                    }

                    AFillDataSet.Tables.Add(ADataTempTableName);

                    using (TheAdapter.SelectCommand)
                    {
                        FDataBaseRDBMS.FillAdapter(TheAdapter, ref AFillDataSet, AStartRecord, AMaxRecords, ADataTempTableName);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".SelectToTempTable: finished filling DbDataAdapter(DataTable '" +
                                   ADataTempTableName) + "'). DT Row Count: " + AFillDataSet.Tables[ADataTempTableName].Rows.Count.ToString()));
#if WITH_POSTGRESQL_LOGGING
                    NpgsqlEventLog.Level = LogLevel.None;
#endif
                }

                ObjReturn = AFillDataSet;
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                if ((ObjReturn != null) && (ObjReturn.Tables[ADataTempTableName] != null))
                {
                    LogTable(ObjReturn.Tables[ADataTempTableName]);
                }
            }

            return ObjReturn;
        }

        #endregion

        #region SelectDA

        /// <summary>
        /// Delegate for optional Column Mappings for use with <see cref="SelectUsingDataAdapter"/>.
        /// </summary>
        /// <param name="AColumNameMappingEnumerator">Enumerator for the Column Mappings.</param>
        /// <returns>Column Mappings string.</returns>
        public delegate string TOptionalColumnMappingDelegate(ref IDictionaryEnumerator AColumNameMappingEnumerator);

        /// <summary>
        /// Executes a SQL Select Statement (once) using a <see cref="DbDataAdapter"/>.
        /// <para><em>Speciality:</em> The execution of the query can be cancelled at any time using the
        /// <see cref="TDataAdapterCanceller.CancelFillOperation"/> Method of the
        /// <see cref="TDataAdapterCanceller"/> instance that gets returned in Argument
        /// <paramref name="ADataAdapterCanceller"/>!
        /// </para>
        /// </summary>
        /// <param name="ASqlStatement">SQL statement. Must contain SQL Parameters if
        /// <paramref name="AParametersArray" /> is specified!</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel"/>.</param>
        /// <param name="AFillDataTable">Instance of a DataTable. Can be null; in that case a DataTable by the name of
        /// "SelectUsingDataAdapter_DataTable" is created on-the-fly.</param>
        /// <param name="ADataAdapterCanceller">An instance of the <see cref="TDataAdapterCanceller"/> Class. Call the
        /// <see cref="TDataAdapterCanceller.CancelFillOperation"/> Method to cancel the execution of the query.</param>
        /// <param name="AOptionalColumnNameMapping">Supply a Delegate to create a mapping between the names of the fields
        /// in the DB and how they should be named in the resulting DataTable. (Optional - pass null for this Argument to not
        /// do that).</param>
        /// <param name="ASelectCommandTimeout">Set a timeout (in seconds) for the Select Command that is different
        /// from the default timeout for a Command (eg. 20s for a NpgsqlCommand). (Optional.)</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value) for a Parameterised Query (Optional.)</param>
        /// <returns>The number of Rows successfully added or refreshed in the DataTable passed in using
        /// <paramref name="AFillDataTable"/> (=return value of calling DbDataAdapter.Fill) - or -1 in case
        /// the creation of the internally used DataAdapter failed (should not happen).</returns>
        public int SelectUsingDataAdapter(String ASqlStatement, TDBTransaction AReadTransaction,
            ref DataTable AFillDataTable, out TDataAdapterCanceller ADataAdapterCanceller,
            TOptionalColumnMappingDelegate AOptionalColumnNameMapping = null,
            int ASelectCommandTimeout = -1, DbParameter[] AParametersArray = null)
        {
            List <object[]>ParameterValues = null;
            List <object>ObjectValues = null;

            if (AParametersArray != null)
            {
                ParameterValues = new List <object[]>(1);
                ObjectValues = new List <object>();

                for (int Counter = 0; Counter < AParametersArray.Length; Counter++)
                {
                    ObjectValues.Add(AParametersArray[Counter].Value);
                }

                ParameterValues.Add(ObjectValues.ToArray());
            }

            return SelectUsingDataAdapterMulti(ASqlStatement,
                AReadTransaction,
                ref AFillDataTable,
                out ADataAdapterCanceller,
                AOptionalColumnNameMapping,
                ASelectCommandTimeout,
                AParametersArray,
                ParameterValues,
                false);
        }

        /// <summary>
        /// Executes a SQL Select Statement (1..n times if a Paramterised Query is used) using a <see cref="DbDataAdapter"/>.
        /// <para><em>Speciality:</em> The execution of the query can be cancelled at any time using the
        /// <see cref="TDataAdapterCanceller.CancelFillOperation"/> Method of the
        /// <see cref="TDataAdapterCanceller"/> instance that gets returned in Argument
        /// <paramref name="ADataAdapterCanceller"/>!
        /// </para>
        /// <para>
        /// In case <paramref name="AParameterValues"/> holds more than one entry then the same parameterised query will be
        /// executed as many times as there are entries. The resulting data of all query executions gets appended to
        /// <paramref name="AFillDataTable"/>!
        /// </para>
        /// </summary>
        /// <param name="ASqlStatement">SQL statement. Must contain SQL Parameters if
        /// <paramref name="AParameterDefinitions"/> is specified!</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel"/>.</param>
        /// <param name="AFillDataTable">Instance of a DataTable. Can be null; in that case a DataTable by the name of
        /// "SelectUsingDataAdapter_DataTable" is created on-the-fly.</param>
        /// <param name="ADataAdapterCanceller">An instance of the <see cref="TDataAdapterCanceller"/> Class. Call the
        /// <see cref="TDataAdapterCanceller.CancelFillOperation"/> Method to cancel the execution of the query.</param>
        /// <param name="AOptionalColumnNameMapping">Supply a Delegate to create a mapping between the names of the fields
        /// in the DB and how they should be named in the resulting DataTable. (Optional - pass null for this Argument to not
        /// do that).</param>
        /// <param name="ASelectCommandTimeout">Set a timeout (in seconds) for the Select Command that is different
        /// from the default timeout for a Command (eg. 20s for a NpgsqlCommand). (Optional.)</param>
        /// <param name="AParameterDefinitions">Instantiated DbParameters (eg. OdbcParameters) for a Parameterised Query.
        /// Only the the Types of the Parameters are relevant, the Values will be <em>ignored</em> (these will need to be passed
        /// in via the <paramref name="AParameterValues"/> Argument!!! (Optional.)</param>
        /// <param name="AParameterValues">A List of Type object[] that contains 1..n Parameter Values in each array for
        /// 1..n executions (=number of List entries) of the <em>same</em> Parameterised Query using the internal DataAdapter.</param>
        /// <param name="APrepareSelectCommand">Set to true to 'Prepare' the Select Command in the RDBMS (if the RDBMS supports
        /// that). Because this only makes sense if <paramref name="AParameterValues"/> holds more than one entry it will be
        /// ignored if <paramref name="AParameterValues"/> is null or holds only one entry! (Optional, Default=false.).</param>
        /// <param name="AProgressUpdateEveryNRecs">Specifies the interval in which
        /// the Delegate passed in in Argument <paramref name="AMultipleParamQueryProgressUpdateCallback"/> will get called
        /// (Optional if that Delegate isn't passed in./)</param>
        /// <param name="AMultipleParamQueryProgressUpdateCallback">The Delegate that should be called every
        /// '<paramref name="AProgressUpdateEveryNRecs"/>' records while multiple Parameterised Query executions take place.
        /// This makes sense only when <paramref name="AParameterValues"/> holds more than one Item. (Optional.)</param>
        /// <returns>The number of Rows successfully added or refreshed in the DataTable passed in using
        /// <paramref name="AFillDataTable"/> (=return value of calling DbDataAdapter.Fill) - or -1 in case
        /// the creation of the internally used DataAdapter failed (should not happen). In case a Parameterised Query got
        /// executed several times (with multiple Parameter Values) then the return value is the added-up number of the calls
        /// to DbDataAdapter.Fill!</returns>
        public int SelectUsingDataAdapterMulti(String ASqlStatement, TDBTransaction AReadTransaction, ref DataTable AFillDataTable,
            out TDataAdapterCanceller ADataAdapterCanceller, TOptionalColumnMappingDelegate AOptionalColumnNameMapping = null,
            int ASelectCommandTimeout = -1, DbParameter[] AParameterDefinitions = null, List <object[]>AParameterValues = null,
            bool APrepareSelectCommand = false,
            int AProgressUpdateEveryNRecs = 0, MultipleParamQueryProgressUpdateDelegate AMultipleParamQueryProgressUpdateCallback = null)
        {
            int ReturnValue = 0;
            bool userCancel = false;
            DbDataAdapter SelectDataAdapter;
            IDictionaryEnumerator ColumNameMappingEnumerator = null;
            string MappingsString;

            OdbcParameter[] QueryParameters = null;
            bool ExecuteProgressUpdateCallback = (AMultipleParamQueryProgressUpdateCallback != null)
                                                 && (AProgressUpdateEveryNRecs != 0);

            AFillDataTable = AFillDataTable ?? new DataTable("SelectUsingDataAdapter_DataTable");
            ADataAdapterCanceller = null;

            ASqlStatement = FDataBaseRDBMS.FormatQueryRDBMSSpecific(ASqlStatement);

            WaitForCoordinatedDBAccess();

            try
            {
                if (AParameterDefinitions != null)
                {
                    #region Validate Arguments

                    if (AParameterValues == null)
                    {
                        throw new ArgumentNullException("AParameterValues", "AParameterValues must not be null if " +
                            "AParameterDefinitions is specified");
                    }

                    if (AParameterDefinitions.Length != AParameterValues[0].Length)
                    {
                        throw new ArgumentException(String.Format(
                                "ValidateSeparateOdbcQueryParametersAndValues - Item count mismatch: AParameterDefinitions holds {0} " +
                                "items but AParameterValues holds {1}; in order for this Method to work they must hold the same number of items",
                                AParameterDefinitions.Length, AParameterValues[0].Length));
                    }

                    #endregion

                    QueryParameters = new OdbcParameter[AParameterDefinitions.Length];

                    for (int Counter = 0; Counter < AParameterDefinitions.Length; Counter++)
                    {
                        QueryParameters[Counter] = (OdbcParameter)AParameterDefinitions[Counter];
                    }
                }

                if (APrepareSelectCommand
                    && (AParameterValues != null)
                    && (AParameterValues.Count > 1))
                {
                    PrepareNextCommand();
                }

                if (ASelectCommandTimeout != -1)
                {
                    SetTimeoutForNextCommand(ASelectCommandTimeout);
                }

                if (AParameterValues != null)
                {
                    string CommandText = ASqlStatement;
                    DbParameter[] ConvertedParameters = FDataBaseRDBMS.ConvertOdbcParameters(QueryParameters, ref CommandText);

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        LogSqlStatement(this.GetType().FullName + ".SelectUsingDataAdapter()", CommandText, QueryParameters);
                    }

                    SelectDataAdapter = (DbDataAdapter)SelectDA(CommandText, AReadTransaction,
                        false, ConvertedParameters);
                }
                else
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        LogSqlStatement(this.GetType().FullName + ".SelectUsingDataAdapter()", ASqlStatement, null);
                    }

                    SelectDataAdapter = (DbDataAdapter)SelectDA(ASqlStatement, AReadTransaction,
                        false);
                }

                if (SelectDataAdapter != null)
                {
                    if (AOptionalColumnNameMapping != null)
                    {
                        MappingsString = AOptionalColumnNameMapping(ref ColumNameMappingEnumerator);

                        if (ColumNameMappingEnumerator != null)
                        {
                            DataTableMapping AliasNames;

                            AliasNames = SelectDataAdapter.TableMappings.Add(MappingsString, MappingsString);

                            while (ColumNameMappingEnumerator.MoveNext())
                            {
                                AliasNames.ColumnMappings.Add(ColumNameMappingEnumerator.Key.ToString(), ColumNameMappingEnumerator.Value.ToString());
                            }
                        }
                    }

                    ADataAdapterCanceller = new TDataAdapterCanceller(SelectDataAdapter);

                    if (AParameterValues != null)
                    {
                        // AParameterValues can hold parameters for 1..n executions of the same query with differing Parameter Values.
                        // Iterate over the AParameterValues entries and assign the individual Parameter Values of a given entry to
                        // the SelectDataAdapter.SelectCommand.Parameters' Values, then execute SelectDataAdapter.Fill.
                        // Thus the resulting DataRows of all the iterations will all be appended to AFillDataTable.
                        for (int OuterCounter = 0; OuterCounter < AParameterValues.Count && !userCancel; OuterCounter++)
                        {
                            for (int InnerCounter = 0; InnerCounter < AParameterValues[OuterCounter].Length && !userCancel; InnerCounter++)
                            {
                                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
                                {
                                    TLogging.Log("Adding parameter " + InnerCounter.ToString() + ": " +
                                        SelectDataAdapter.SelectCommand.Parameters[InnerCounter].ParameterName +
                                        " (" + SelectDataAdapter.SelectCommand.Parameters[InnerCounter].DbType.ToString() + ") value: " +
                                        AParameterValues[OuterCounter][InnerCounter].ToString() +
                                        " (" + AParameterValues[OuterCounter][InnerCounter].GetType().ToString() + ")");
                                }

                                SelectDataAdapter.SelectCommand.Parameters[InnerCounter].Value = AParameterValues[OuterCounter][InnerCounter];
                            }

                            ReturnValue += SelectDataAdapter.Fill(AFillDataTable);

                            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                            {
                                TLogging.Log(((this.GetType().FullName + ".SelectUsingDataAdapter: finished filling DbDataAdapter(DataTable " +
                                               AFillDataTable.TableName) + "). DT Row Count: " + AFillDataTable.Rows.Count.ToString() +
                                              "; Parameter iteration count: " + OuterCounter.ToString()));
                            }

                            if (ExecuteProgressUpdateCallback
                                && ((OuterCounter + 1) % AProgressUpdateEveryNRecs == 0))
                            {
                                userCancel = AMultipleParamQueryProgressUpdateCallback(OuterCounter + 1);
                            }
                        }
                    }
                    else
                    {
                        ReturnValue = SelectDataAdapter.Fill(AFillDataTable);

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log(((this.GetType().FullName + ".SelectUsingDataAdapter: finished filling DbDataAdapter(DataTable " +
                                           AFillDataTable.TableName) + "). DT Row Count: " + AFillDataTable.Rows.Count.ToString()));
                        }
                    }
                }
                else
                {
                    // Should not happen, but if it does then let the caller know that!
                    ReturnValue = -1;
                }
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB40001 exception in TDatabase.SelectUsingDataAdapter",
                        exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB23505 exception in TDatabase.SelectUsingDataAdapter",
                        exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }

                throw;
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                LogTable(AFillDataTable);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns a <see cref="DbDataAdapter" /> (eg. <see cref="OdbcDataAdapter" />, NpgsqlDataAdapter) for a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <remarks>
        /// <b>Important:</b> Since an object that derives from <see cref="DbDataAdapter" /> is returned you ought to
        /// <em>call .Dispose()</em> on the returned object to release its resouces! (<see cref="DbDataAdapter" /> inherits
        /// from <see cref="DataAdapter" /> which itself inherits from <see cref="System.ComponentModel.Component" />, which
        /// implements <see cref="IDisposable" />.
        /// <p><b>ALSO</b>, the returned object contains an instance of DbCommand in its SelectCommand Property which itself
        /// inherits from <see cref="System.ComponentModel.Component" />, which implements <see cref="IDisposable" /> so you
        /// ought to <em>call .Dispose()</em> on the object held in the SelectCommand Property to release its resouces, too!).</p>
        /// </remarks>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>
        /// Instantiated object (derived from <see cref="DbDataAdapter" />. It contains an instantiated object derived from
        /// <see cref="DbCommand" /> in its SelectCommand Property, too! The Type of both the instantiated object and the object
        /// held in the SelectCommand Property depend on the RDBMS that we are connected to at runtime!
        /// </returns>
        private DbDataAdapter SelectDA(String ASqlStatement, TDBTransaction AReadTransaction, bool AMustCoordinateDBAccess,
            DbParameter[] AParametersArray = null)
        {
            DbCommand TheCommand;
            DbDataAdapter TheAdapter;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".SelectDA()...");
            }

            try
            {
                TheCommand = Command(ASqlStatement, AReadTransaction, false, AParametersArray);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".SelectDA: now creating DbDataAdapter(" + ASqlStatement + ")...");
                }

                TheAdapter = FDataBaseRDBMS.NewAdapter();

                TheAdapter.SelectCommand = TheCommand;
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB40001 exception in TDatabase.SelectDA", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB23505 exception in TDatabase.SelectDA", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }

                throw;
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return TheAdapter;
        }

        #endregion

        #region SelectDT

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <returns>Instantiated DataTable</returns>
        public DataTable SelectDT(String ASqlStatement, String ADataTableName, TDBTransaction AReadTransaction)
        {
            return SelectDTInternal(ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0], true);
        }

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Not suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="DataTable" /></returns>
        public DataTable SelectDT(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
            return SelectDTInternal(ASqlStatement, ADataTableName,
                AReadTransaction, AParametersArray, true);
        }

        /// <summary>
        /// Returns a <see cref="DataTable" /> filled with the result of a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ADataTableName">Name that the <see cref="DataTable" /> should get</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>Instantiated <see cref="DataTable" /></returns>
        private DataTable SelectDTInternal(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray,
            bool AMustCoordinateDBAccess)
        {
            DataTable ObjReturn;

            if (ADataTableName == String.Empty)
            {
                throw new ArgumentException("A name for the DataTable must be submitted!", "ADataTableName");
            }

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".SelectDTInternal()", ASqlStatement, AParametersArray);
            }

            ObjReturn = new DataTable(ADataTableName);

            try
            {
                using (DbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, false,
                           AParametersArray))
                {
                    using (TheAdapter.SelectCommand)
                    {
                        FDataBaseRDBMS.FillAdapter(TheAdapter, ref ObjReturn, 0, 0);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".SelectDTInternal: finished filling DbDataAdapter(DataTable " +
                                   ADataTableName) + "). DT Row Count: " + ObjReturn.Rows.Count.ToString()));
                }
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB40001 exception in TDatabase.SelectDTInternal",
                        exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB23505 exception in TDatabase.SelectDTInternal",
                        exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }

                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                LogTable(ObjReturn);
            }

            return ObjReturn;
        }

        /// <summary>
        /// this loads the result into a typed datatable
        /// </summary>
        /// <param name="ATypedDataTable">this needs to be an object of the typed datatable</param>
        /// <param name="ASqlStatement"></param>
        /// <param name="AReadTransaction"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="AStartRecord">does not have any effect yet</param>
        /// <param name="AMaxRecords">not implemented yet</param>
        /// <returns></returns>
        public DataTable SelectDT(DataTable ATypedDataTable, String ASqlStatement,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray = null,
            int AStartRecord = 0, int AMaxRecords = 0)
        {
            if (ATypedDataTable == null)
            {
                throw new ArgumentException("ATypedDataTable must not be null", "ATypedDataTable");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".SelectDT()", ASqlStatement, AParametersArray);
            }

            WaitForCoordinatedDBAccess();

            try
            {
                using (DbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, false, AParametersArray))
                {
                    using (TheAdapter.SelectCommand)
                    {
                        FDataBaseRDBMS.FillAdapter(TheAdapter, ref ATypedDataTable, AStartRecord, AMaxRecords);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".SelectDT: finished filling DbDataAdapter(DataTable '" +
                                   ATypedDataTable.TableName) + "'). DT Row Count: " + ATypedDataTable.Rows.Count.ToString()));
#if WITH_POSTGRESQL_LOGGING
                    NpgsqlEventLog.Level = LogLevel.None;
#endif
                }
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB40001 exception in TDatabase.SelectDT", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException("EDB23505 exception in TDatabase.SelectDT", exp);
                    LogException(e, "Error fetching records.");
                    throw e;
                }

                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
            {
                if (ATypedDataTable != null)
                {
                    LogTable(ATypedDataTable);
                }
            }

            return ATypedDataTable;
        }

        #endregion

        #region DB Connection check Methods

        /// <summary>
        /// Checks if the current Thread is the same Thread that the currently established DB Connection was established on.
        /// </summary>
        /// <returns>True if there isn't a currently established DB Connection or if the current Thread is the same Thread
        /// that the currently running established DB Connection was established on; if this isn't the case false is
        /// returned.</returns>
        public bool CheckEstablishedDBConnectionThreadIsCompatible()
        {
            return CheckEstablishedDBConnectionThreadIsCompatible(true);
        }

        /// <summary>
        /// Checks if the current Thread is the same Thread that the currently established DB Connection was established on.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if there isn't a currently established DB Connection or if the current Thread is the same Thread
        /// that the currently running established DB Connection was established on; if this isn't the case false is
        /// returned.</returns>
        private bool CheckEstablishedDBConnectionThreadIsCompatible(bool AMustCoordinateDBAccess)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FSqlConnection == null)
                {
                    return true;
                }

                // Multi-threading 'Sanity Check':
                // Check if the current Thread is the same Thread that the currently established DB Connection was established on:
                // if not, this Method returns false!
                // Reason for that: Running DB Commands on a different Thread than the one that the DB Connection was started
                // on isn't supported as the ADO.NET providers (specifically: the PostgreSQL provider, Npgsql) aren't thread-safe!
                if (ThreadingHelper.GetCurrentThreadIdentifier() != ThreadingHelper.GetThreadIdentifier(
                        ThreadThatConnectionWasEstablishedOn))
                {
                    return false;
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return true;
        }

        #endregion

        #region Transactions

        #region Transaction check Methods

        /// <summary>
        /// Checks if the current Thread is the same Thread that <paramref name="ATransaction"/> was started on.
        /// </summary>
        /// <param name="ATransaction">An instantiated Transaction.</param>
        /// <returns>True if the current Thread is the same Thread that <paramref name="ATransaction"/> was started on;
        /// if this isn't the case false is returned.</returns>
        public static bool CheckDBTransactionThreadIsCompatible(TDBTransaction ATransaction)
        {
            if (ATransaction == null)
            {
                throw new ArgumentNullException("ATransaction", "ATransaction Argument must not be null");
            }

            // Multi-threading 'Sanity Check':
            // Check if the current Thread is the same Thread that ATransaction was started on: if not, this Method returns false!
            // Reason for that: Running DB Commands on a different Thread than the one that the DB Transaction was started
            // on isn't supported as the ADO.NET providers (specifically: the PostgreSQL provider, Npgsql) aren't thread-safe!
            if (ThreadingHelper.GetCurrentThreadIdentifier() != ThreadingHelper.GetThreadIdentifier(
                    ATransaction.ThreadThatTransactionWasStartedOn))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the current Thread is the same Thread that the currently running DB Transaction was started on.
        /// </summary>
        /// <returns>True if there isn't a currently running DB Transaction or if the current Thread is the same Thread
        /// that the currently running DB Transaction was started on; if this isn't the case false is returned.</returns>
        public bool CheckRunningDBTransactionThreadIsCompatible()
        {
            return CheckRunningDBTransactionThreadIsCompatible(true);
        }

        /// <summary>
        /// Checks if the current Thread is the same Thread that the currently running DB Transaction was started on.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if there isn't a currently running DB Transaction or if the current Thread is the same Thread
        /// that the currently running DB Transaction was started on; if this isn't the case false is returned.</returns>
        private bool CheckRunningDBTransactionThreadIsCompatible(bool AMustCoordinateDBAccess)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FTransaction == null)
                {
                    return true;
                }

                // Multi-threading 'Sanity Check':
                // Check if the current Thread is the same Thread that the currently running DB Transaction was started on:
                // if not, this Method returns false!
                // Reason for that: Running DB Commands on a different Thread than the one that the DB Transaction was started
                // on isn't supported as the ADO.NET providers (specifically: the PostgreSQL provider, Npgsql) aren't thread-safe!
                if (ThreadingHelper.GetCurrentThreadIdentifier() != ThreadingHelper.GetThreadIdentifier(
                        FTransaction.ThreadThatTransactionWasStartedOn))
                {
                    return false;
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the <see cref="IsolationLevel"/> of the currently running DB Transaction is acceptable for what is
        /// asked for with the two Arguments.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel"/> that is desired.</param>
        /// <param name="AEnforceIsolationLevel">Pass <see cref="TEnforceIsolationLevel.eilExact"/> if an
        /// exact match of the <see cref="IsolationLevel"/> of the currently running DB Transaction with
        /// <paramref name="ADesiredIsolationLevel"/> is required. Pass <see cref="TEnforceIsolationLevel.eilMinimum"/>
        /// if the minimum <see cref="IsolationLevel"/> specified with <paramref name="ADesiredIsolationLevel"/> is
        /// acceptable.
        /// </param>
        /// <returns>True if there isn't a currently running DB Transaction or if the <see cref="IsolationLevel"/> of the
        /// currently running DB Transaction is acceptable for what is asked for with the two Arguments; if this isn't the case
        /// false is returned.</returns>
        public bool CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel AEnforceIsolationLevel)
        {
            return CheckRunningDBTransactionIsolationLevelIsCompatible(ADesiredIsolationLevel, AEnforceIsolationLevel,
                true);
        }

        /// <summary>
        /// Checks if the <see cref="IsolationLevel"/> of the currently running DB Transaction is acceptable for what is
        /// asked for with the two Arguments.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel"/> that is desired.</param>
        /// <param name="AEnforceIsolationLevel">Pass <see cref="TEnforceIsolationLevel.eilExact"/> if an
        /// exact match of the <see cref="IsolationLevel"/> of the currently running DB Transaction with
        /// <paramref name="ADesiredIsolationLevel"/> is required. Pass <see cref="TEnforceIsolationLevel.eilMinimum"/>
        /// if the minimum <see cref="IsolationLevel"/> specified with <paramref name="ADesiredIsolationLevel"/> is
        /// acceptable.
        /// </param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if there isn't a currently running DB Transaction or if the <see cref="IsolationLevel"/> of the
        /// currently running DB Transaction is acceptable for what is asked for with the two Arguments; if this isn't the case
        /// false is returned.</returns>
        private bool CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel AEnforceIsolationLevel, bool AMustCoordinateDBAccess)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FTransaction == null)
                {
                    return true;
                }

                // Check if the IsolationLevel of the currently running Transaction is acceptable for what is requested with
                // the ADesiredIsolationLevel and AEnforceIsolationLevel Arguments: if not, this Method returns false!
                if ((AEnforceIsolationLevel == TEnforceIsolationLevel.eilExact)
                    && (FTransaction.IsolationLevel != ADesiredIsolationLevel)
                    || ((AEnforceIsolationLevel == TEnforceIsolationLevel.eilMinimum)
                        && (FTransaction.IsolationLevel < ADesiredIsolationLevel)))
                {
                    return false;
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the current Thread is the same Thread that the currently running DB Transaction was started on AND
        /// if the <see cref="IsolationLevel"/> of the currently running DB Transaction is acceptable for what is
        /// asked for with the two Arguments.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel"/> that is desired.</param>
        /// <param name="AEnforceIsolationLevel">Pass <see cref="TEnforceIsolationLevel.eilExact"/> if an
        /// exact match of the <see cref="IsolationLevel"/> of the currently running DB Transaction with
        /// <paramref name="ADesiredIsolationLevel"/> is required. Pass <see cref="TEnforceIsolationLevel.eilMinimum"/>
        /// if the minimum <see cref="IsolationLevel"/> specified with <paramref name="ADesiredIsolationLevel"/> is
        /// acceptable.
        /// </param>
        /// <remarks>Combines calls to the Methods <see cref="CheckRunningDBTransactionThreadIsCompatible()"/> and
        /// <see cref="CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel, TEnforceIsolationLevel)"/>
        /// for convenience!</remarks>
        /// <returns>True if there isn't a currently running DB Transaction. Also returns true if the current Thread is the
        /// same Thread that the currently running DB Transaction was started on AND if the <see cref="IsolationLevel"/>
        /// of the currently running DB Transaction is acceptable for what is asked for with the two Arguments; if this isn't the
        /// case false is returned.</returns>
        public bool CheckRunningDBTransactionIsCompatible(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel AEnforceIsolationLevel)
        {
            if (!(CheckRunningDBTransactionThreadIsCompatible())
                || (!CheckRunningDBTransactionIsolationLevelIsCompatible(ADesiredIsolationLevel,
                        AEnforceIsolationLevel)))
            {
                return false;
            }

            return true;
        }

        #endregion

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <param name="ATransactionName">Name of the DB Transaction  (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <returns>Started Transaction (null if an error occured).</returns>
        public TDBTransaction BeginTransaction(Int16 ARetryAfterXSecWhenUnsuccessful = -1, string ATransactionName = "")
        {
            return BeginTransaction(true, ARetryAfterXSecWhenUnsuccessful, ATransactionName);
        }

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="ADBReconnectionAttempt">This must only be set to anything higher than 0 by the recursive call
        /// inside the Method; set to -1 to suppress an automatic DB reconnection attempt (this automatic behaviour
        /// should only be turned off if there is a very specific reason for that!).</param>
        /// <returns>Started Transaction (null if an error occured).</returns>
        internal TDBTransaction BeginTransaction(bool AMustCoordinateDBAccess,
            Int16 ARetryAfterXSecWhenUnsuccessful = -1, string ATransactionName = "", int ADBReconnectionAttempt = 0)
        {
            string NestedTransactionProblemError;
            bool ExceptionCausedByUnavailableDBConn;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                // Guard against running into a 'Nested' DB Transaction (which are not supported!)
                if (FTransaction != null)
                {
                    // Retry again if programmer wants that
                    if (ARetryAfterXSecWhenUnsuccessful != -1)
                    {
                        Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                        // Retry again to begin a transaction (=RECURSIVE call!).
                        // Note: If this fails again, an Exception is thrown as if there was
                        // no ARetryAfterXSecWhenUnsuccessful specfied!
                        return BeginTransaction(false, -1, ATransactionName);
                    }
                    else
                    {
                        NestedTransactionProblemError = String.Format(StrNestedTransactionProblem, FTransaction.Connection != null,
                            FTransaction.IsolationLevel, FTransaction.Reused, ThreadingHelper.GetThreadIdentifier(
                                FTransaction.ThreadThatTransactionWasStartedOn),
                            FTransaction.AppDomainNameThatTransactionWasStartedIn, ThreadingHelper.GetCurrentThreadIdentifier(),
                            AppDomain.CurrentDomain.FriendlyName,
                            TLogging.StackTraceToText(FTransaction.StackTraceAtPointOfTransactionStart),
                            TLogging.StackTraceToText(new StackTrace(true)));
                        TLogging.Log(NestedTransactionProblemError);

                        throw new EDBTransactionBusyException(
                            "Concurrent DB Transactions are not supported: BeginTransaction would overwrite existing DB Transaction - " +
                            "You must use GetNewOrExistingTransaction, GetNewOrExistingAutoTransaction or " +
                            "GetNewOrExistingAutoReadTransaction!", NestedTransactionProblemError);
                    }
                }

                try
                {
                    if ((FSqlConnection == null)
                        && (ADBReconnectionAttempt > 0))
                    {
                        throw new EDBConnectionBrokenException();
                    }
                    else
                    {
                        ExtendedLoggingInfoOnHigherDebugLevels("Trying to start a DB Transaction... ");

                        FTransaction = new TDBTransaction(FSqlConnection.BeginTransaction(), ConnectionIdentifier, this,
                            false, ATransactionName);

                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "DB Transaction started{0})", FTransaction.GetDBTransactionIdentifier()),
                            DBAccess.DB_DEBUGLEVEL_TRANSACTION, true);
                    }
                }
                catch (Exception Exc)
                {
                    ExceptionCausedByUnavailableDBConn = TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionServerSide(Exc);

                    if ((FSqlConnection == null) || (FSqlConnection.State == ConnectionState.Broken)
                        || (FSqlConnection.State == ConnectionState.Closed)
                        || (ExceptionCausedByUnavailableDBConn && (ADBReconnectionAttempt <= 1)))
                    {
                        //
                        // Attempt to reconnect the database connection (one attempt only!)
                        //

                        if (FSqlConnection != null)
                        {
                            if (!ExceptionCausedByUnavailableDBConn)
                            {
                                TLogging.Log("BeginTransaction encountered an Exception:\r\n" + Exc.ToString());

                                TLogging.Log(
                                    "BeginTransaction: Attempting to reconnect to the database because the DB connection isn't allowing the start of a DB Transaction! (Connection State: "
                                    +
                                    FSqlConnection.State.ToString("G") + ")");

                                if (FSqlConnection.State == ConnectionState.Broken)
                                {
                                    FSqlConnection.Close();
                                }
                            }
                            else
                            {
                                TLogging.Log("BeginTransaction: Attempting to reconnect to the database because the DB connection is broken...");
                            }

                            FSqlConnection.Dispose();
                            FSqlConnection = null;
                        }

                        if (ADBReconnectionAttempt < 1)
                        {
                            ADBReconnectionAttempt++;

                            TLogging.Log(String.Format("BeginTransaction: DB Reconnection attempt #{0} underway...",
                                    ADBReconnectionAttempt));

                            try
                            {
                                EstablishDBConnection(FDbType, FDsnOrServer, FDBPort, FDatabaseName, FUsername, FPassword,
                                    FConnectionString, false);
                            }
                            catch (Exception e2)
                            {
                                ExceptionCausedByUnavailableDBConn = TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionServerSide(e2);

                                if (!ExceptionCausedByUnavailableDBConn)
                                {
                                    LogExceptionAndThrow(e2,
                                        "BeginTransaction: Another Exception occured while trying to establish the connection: " + e2.ToString());
                                }
                                else
                                {
                                    TLogging.Log("    FAILED to begin a DB Transaction because the DB (Server) is unavailable.");
                                }
                            }

                            // Retry again to begin a transaction (=RECURSIVE call!).
                            return BeginTransaction(false, ARetryAfterXSecWhenUnsuccessful, ATransactionName,
                                ADBReconnectionAttempt);
                        }
                    }

                    if (!(Exc is EDBConnectionBrokenException))
                    {
                        LogExceptionAndThrow(Exc, "BeginTransaction: Error creating Transaction - Server-side error.");
                    }
                    else
                    {
                        throw;
                    }
                }

                FLastDBAction = DateTime.Now;

                return FTransaction;
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection. Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <returns>Started Transaction (null if an error occured).</returns>
        public TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful = -1,
            string ATransactionName = "")
        {
            return BeginTransaction(AIsolationLevel, true, ARetryAfterXSecWhenUnsuccessful, ATransactionName);
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection. Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="ADBReconnectionAttempt">This must only be set to anything higher than 0 by the recursive call
        /// inside the Method; set to -1 to suppress an automatic DB reconnection attempt (this automatic behaviour
        /// should only be turned off if there is a very specific reason for that!).</param>
        /// <returns>Started Transaction (null if an error occured).</returns>
        internal TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel, bool AMustCoordinateDBAccess,
            Int16 ARetryAfterXSecWhenUnsuccessful = -1, string ATransactionName = "", int ADBReconnectionAttempt = 0)
        {
            string NestedTransactionProblemError;
            bool ExceptionCausedByUnavailableDBConn;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FDataBaseRDBMS == null)
                {
                    throw new EOPDBException("DBAccess BeginTransaction: FDataBaseRDBMS is null");
                }

                // Guard against running into a 'Nested' DB Transaction (which are not supported!)
                if (FTransaction != null)
                {
                    // Retry again if programmer wants that
                    if (ARetryAfterXSecWhenUnsuccessful != -1)
                    {
                        Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                        // Retry again to begin a transaction (=RECURSIVE call!).
                        // Note: If this fails again, an Exception is thrown as if there was
                        // no ARetryAfterXSecWhenUnsuccessful specfied!
                        return BeginTransaction(AIsolationLevel, false, -1);
                    }
                    else
                    {
                        NestedTransactionProblemError = String.Format(StrNestedTransactionProblem, FTransaction.Connection != null,
                            FTransaction.IsolationLevel, FTransaction.Reused, ThreadingHelper.GetThreadIdentifier(
                                FTransaction.ThreadThatTransactionWasStartedOn),
                            FTransaction.AppDomainNameThatTransactionWasStartedIn, ThreadingHelper.GetCurrentThreadIdentifier(),
                            AppDomain.CurrentDomain.FriendlyName,
                            TLogging.StackTraceToText(FTransaction.StackTraceAtPointOfTransactionStart),
                            TLogging.StackTraceToText(new StackTrace(true)));
                        TLogging.Log(NestedTransactionProblemError);

                        throw new EDBTransactionBusyException(
                            "Concurrent DB Transactions are not supported (requested IsolationLevel: " +
                            Enum.GetName(typeof(IsolationLevel), AIsolationLevel) + "): BeginTransaction would overwrite " +
                            "existing DB Transaction - You must use GetNewOrExistingTransaction, GetNewOrExistingAutoTransaction or " +
                            "GetNewOrExistingAutoReadTransaction!", NestedTransactionProblemError);
                    }
                }

                FDataBaseRDBMS.AdjustIsolationLevel(ref AIsolationLevel);

                try
                {
                    if ((FSqlConnection == null)
                        && (ADBReconnectionAttempt > 0))
                    {
                        throw new EDBConnectionBrokenException();
                    }
                    else
                    {
                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "Trying to start a DB Transaction with IsolationLevel '{0}'...", AIsolationLevel));

                        FTransaction = new TDBTransaction(FSqlConnection.BeginTransaction(AIsolationLevel),
                            ConnectionIdentifier, this, false, ATransactionName);

                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "DB Transaction{0} with IsolationLevel '{1}' got started", FTransaction.GetDBTransactionIdentifier(),
                                AIsolationLevel), DBAccess.DB_DEBUGLEVEL_TRANSACTION, true);
                    }
                }
                catch (Exception Exc)
                {
                    ExceptionCausedByUnavailableDBConn = TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionServerSide(Exc);

                    if ((FSqlConnection == null) || (FSqlConnection.State == ConnectionState.Broken)
                        || (FSqlConnection.State == ConnectionState.Closed)
                        || (ExceptionCausedByUnavailableDBConn && (ADBReconnectionAttempt <= 1)))
                    {
                        //
                        // Attempt to reconnect the database connection (one attempt only!)
                        //

                        if (FSqlConnection != null)
                        {
                            if (!ExceptionCausedByUnavailableDBConn)
                            {
                                TLogging.Log("BeginTransaction encountered an Exception:\r\n" + Exc.ToString());

                                TLogging.Log(
                                    "BeginTransaction: Attempting to reconnect to the database because the DB connection isn't allowing the start of a DB Transaction! (Connection State: "
                                    +
                                    FSqlConnection.State.ToString("G") + ")");

                                if (FSqlConnection.State == ConnectionState.Broken)
                                {
                                    FSqlConnection.Close();
                                }
                            }
                            else
                            {
                                TLogging.Log("BeginTransaction: Attempting to reconnect to the database because the DB connection is broken...");
                            }

                            FSqlConnection.Dispose();
                            FSqlConnection = null;
                        }

                        if (ADBReconnectionAttempt < 1)
                        {
                            ADBReconnectionAttempt++;

                            TLogging.Log(String.Format("BeginTransaction: DB Reconnection attempt #{0} underway...",
                                    ADBReconnectionAttempt));

                            try
                            {
                                EstablishDBConnection(FDbType, FDsnOrServer, FDBPort, FDatabaseName, FUsername, FPassword,
                                    FConnectionString, false);
                            }
                            catch (Exception e2)
                            {
                                ExceptionCausedByUnavailableDBConn = TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionServerSide(e2);

                                if (!ExceptionCausedByUnavailableDBConn)
                                {
                                    LogExceptionAndThrow(e2,
                                        "BeginTransaction: Another Exception occured while trying to establish the connection: " + e2.ToString());
                                }
                                else
                                {
                                    TLogging.Log("    FAILED to begin a DB Transaction because the DB (Server) is unavailable.");
                                }
                            }

                            // Retry again to begin a transaction (=RECURSIVE call!).
                            return BeginTransaction(AIsolationLevel, false, ARetryAfterXSecWhenUnsuccessful, ATransactionName,
                                ADBReconnectionAttempt);
                        }
                    }

                    if (!(Exc is EDBConnectionBrokenException))
                    {
                        LogExceptionAndThrow(Exc, "BeginTransaction: Error creating Transaction - Server-side error.");
                    }
                    else
                    {
                        throw;
                    }
                }

                FLastDBAction = DateTime.Now;

                return FTransaction;
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Commits a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void CommitTransaction()
        {
            CommitTransaction(true);
        }

        /// <summary>
        /// Commits a running Transaction on the current DB connection.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>void</returns>
        private void CommitTransaction(bool AMustCoordinateDBAccess)
        {
            string TransactionIdentifier = null;
            bool TransactionValid = false;
            bool TransactionReused = false;
            IsolationLevel TransactionIsolationLevel = IsolationLevel.Unspecified;
            string ThreadThatTransactionWasStartedOn = null;
            string AppDomainNameThatTransactionWasStartedIn = null;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FTransaction != null)
                {
                    // Attempt to commit the DB Transaction

                    // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet.
                    if (!FTransaction.Valid)
                    {
                        var Exc1 =
                            new EDBAttemptingToUseTransactionThatIsInvalidException(
                                "TDataBase.CommitTransaction called on DB Transaction that isn't valid",
                                ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                        TLogging.Log(Exc1.ToString());

                        throw Exc1;
                    }

                    // Multi-threading 'Sanity Check':
                    // Check if the current Thread is the same Thread that the current Transaction was started on:
                    // if not, throw Exception!
                    if (!CheckRunningDBTransactionThreadIsCompatible(false))
                    {
                        var Exc2 =
                            new EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(
                                "TDataBase.CommitTransaction would commit DB Transaction that got started on a different Thread " +
                                "and this isn't supported (ADO.NET provider isn't thread-safe!)",
                                ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                        TLogging.Log(Exc2.ToString());

                        throw Exc2;
                    }

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                    {
                        // Gather information for logging
                        TransactionIdentifier = FTransaction.GetDBTransactionIdentifier();
                        TransactionValid = FTransaction.Valid;
                        TransactionReused = FTransaction.Reused;
                        TransactionIsolationLevel = FTransaction.IsolationLevel;
                        ThreadThatTransactionWasStartedOn = ThreadingHelper.GetThreadIdentifier(
                            FTransaction.ThreadThatTransactionWasStartedOn);
                        AppDomainNameThatTransactionWasStartedIn = FTransaction.AppDomainNameThatTransactionWasStartedIn;
                    }

                    FTransaction.WrappedTransaction.Commit();

                    // Commit was OK, now clean up.
                    FTransaction.Dispose();
                    FTransaction = null;

                    FLastDBAction = DateTime.Now;

                    ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                            "DB Transaction{0} got committed.  Before that, its DB Transaction Properties were: Valid: {1}, " +
                            "IsolationLevel: {2}, Reused: {3} (it got started on Thread {4} in AppDomain '{5}').",
                            TransactionIdentifier, TransactionValid, TransactionIsolationLevel, TransactionReused,
                            ThreadThatTransactionWasStartedOn, AppDomainNameThatTransactionWasStartedIn),
                        DBAccess.DB_DEBUGLEVEL_TRANSACTION);
                }
            }
            catch (Exception exp)
            {
                if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB40001 exception in TDatabase.CommitTransaction",
                        exp);
                    LogException(e, "Exception while attempting Transaction commit.");
                    throw e;
                }
                else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                {
                    EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                        "EDB23505 exception in TDatabase.CommitTransaction",
                        exp);
                    LogException(e, "Exception while attempting Transaction commit.");
                    throw e;
                }

                LogExceptionAndThrow(exp, "Exception while attempting Transaction commit.");
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Rolls back a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void RollbackTransaction()
        {
            RollbackTransaction(true);
        }

        /// <summary>
        /// Rolls back a running Transaction on the current DB connection.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>void</returns>
        private void RollbackTransaction(bool AMustCoordinateDBAccess)
        {
            string TransactionIdentifier = null;
            bool TransactionValid = false;
            bool TransactionReused = false;
            IsolationLevel TransactionIsolationLevel = IsolationLevel.Unspecified;
            string ThreadThatTransactionWasStartedOn = null;
            string AppDomainNameThatTransactionWasStartedIn = null;

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FTransaction != null)
                {
                    // Attempt to roll back the DB Transaction.
                    try
                    {
                        // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet.
                        if (!FTransaction.Valid)
                        {
                            var Exc1 =
                                new EDBAttemptingToUseTransactionThatIsInvalidException(
                                    "TDataBase.RollbackTransaction called on DB Transaction that isn't valid",
                                    ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                    ThreadingHelper.GetCurrentThreadIdentifier());

                            TLogging.Log(Exc1.ToString());

                            throw Exc1;
                        }

                        // Multi-threading 'Sanity Check':
                        // Check if the current Thread is the same Thread that the current Transaction was started on:
                        // if not, throw Exception!
                        if (!CheckRunningDBTransactionThreadIsCompatible(false))
                        {
                            var Exc2 =
                                new EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(
                                    "TDataBase.RollbackTransaction would roll back DB Transaction that got started on a different Thread " +
                                    "and this isn't supported (ADO.NET provider isn't thread-safe!)",
                                    ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                    ThreadingHelper.GetCurrentThreadIdentifier());

                            TLogging.Log(Exc2.ToString());

                            throw Exc2;
                        }

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                        {
                            // Gather information for logging
                            TransactionIdentifier = FTransaction.GetDBTransactionIdentifier();
                            TransactionValid = FTransaction.Valid;
                            TransactionReused = FTransaction.Reused;
                            TransactionIsolationLevel = FTransaction.IsolationLevel;
                            ThreadThatTransactionWasStartedOn = ThreadingHelper.GetThreadIdentifier(
                                FTransaction.ThreadThatTransactionWasStartedOn);
                            AppDomainNameThatTransactionWasStartedIn = FTransaction.AppDomainNameThatTransactionWasStartedIn;
                        }

                        FTransaction.WrappedTransaction.Rollback();

                        // Rollback was OK, now clean up.
                        FTransaction.Dispose();
                        FTransaction = null;

                        FLastDBAction = DateTime.Now;

                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "DB Transaction{0} got rolled back.  Before that, its DB Transaction Properties were: Valid: {1}, " +
                                "IsolationLevel: {2}, Reused: {3} (it got started on Thread {4} in AppDomain '{5}').", TransactionIdentifier,
                                TransactionValid, TransactionIsolationLevel, TransactionReused, ThreadThatTransactionWasStartedOn,
                                AppDomainNameThatTransactionWasStartedIn),
                            DBAccess.DB_DEBUGLEVEL_TRANSACTION);
                    }
                    catch (Exception Exc)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        //
                        // MSDN says: "Try/Catch exception handling should always be used when rolling back a
                        // transaction. A Rollback generates an InvalidOperationException if the connection is
                        // terminated or if the transaction has already been rolled back on the server."
                        TLogging.Log("Exception while attempting Transaction rollback: " + Exc.ToString());
                    }
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IsolationLevel"/> of the current DB Transaction (or debugging purposes).
        /// </summary>
        /// <returns><see cref="IsolationLevel.Unspecified"/> if no DB Transaction is open.</returns>
        public IsolationLevel GetIsolationLevel()
        {
            WaitForCoordinatedDBAccess();

            try
            {
                if (FTransaction != null)
                {
                    return FTransaction.IsolationLevel;
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            return IsolationLevel.Unspecified;
        }

        /// <summary>
        /// Either starts a new Transaction on the current DB connection or returns
        /// a existing <see cref="TDBTransaction" />. What it does depends on two factors: whether a Transaction
        /// is currently running or not, and if so, whether it meets the specified
        /// <paramref name="ADesiredIsolationLevel" />.
        /// <para>If there is a current Transaction but it has a different <see cref="IsolationLevel" />,
        /// <see cref="EDBTransactionIsolationLevelWrongException" />
        /// is thrown.</para>
        /// <para>If there is no current Transaction, a new Transaction with the specified <see cref="IsolationLevel" />
        /// is started.</para>
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired</param>
        /// <param name="ANewTransaction">True if a new Transaction was started and is returned,
        /// false if an already existing Transaction is returned</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <returns>Either an existing or a new Transaction that exactly meets the specified <see cref="IsolationLevel" /></returns>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel, out Boolean ANewTransaction,
            string ATransactionName = "")
        {
            return GetNewOrExistingTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                out ANewTransaction, ATransactionName);
        }

        /// <summary>
        /// Either starts a new Transaction on the current DB connection or returns
        /// a existing <see cref="TDBTransaction" />. What it does depends on two factors: whether a Transaction
        /// is currently running or not, and if so, whether it meets the specified
        /// <paramref name="ADesiredIsolationLevel" />.
        /// <para>If there is a current Transaction but it has a different <see cref="IsolationLevel" />, the result
        /// depends on the value of the <paramref name="ATryToEnforceIsolationLevel" />
        /// parameter.</para>
        /// <para>If there is no current Transaction, a new Transaction with the specified <see cref="IsolationLevel" />
        /// is started.</para>
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ANewTransaction">True if a new Transaction was started and is returned,
        /// false if an already existing Transaction is returned</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <returns>Either an existing or a new Transaction that exactly meets the specified IsolationLevel</returns>
        /// <exception cref="EDBTransactionIsolationLevelWrongException">Thrown if the ATryToEnforceIsolationLevel Argument is set to
        /// TEnforceIsolationLevel.eilExact and the existing Transactions' IsolationLevel does not
        /// exactly match the IsolationLevel specified with Argument ADesiredIsolationLevel.</exception>
        /// <exception cref="EDBTransactionIsolationLevelTooLowException">Thrown if ATryToEnforceIsolationLevel is set to
        /// eilExact and the existing Transaction's Isolation Level does not exactly match the Isolation Level specified,</exception>
        /// <exception cref="EDBTransactionIsolationLevelWrongException">Thrown if ATryToEnforceIsolationLevel Argument is set to
        /// eilMinimum and the existing Transaction's Isolation Level hasn't got at least the Isolation Level specified.</exception>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel,
            out Boolean ANewTransaction, string ATransactionName = "")
        {
            TDBTransaction TheTransaction;

            ANewTransaction = false;

            WaitForCoordinatedDBAccess();

            try
            {
                TheTransaction = FTransaction;

                FDataBaseRDBMS.AdjustIsolationLevel(ref ADesiredIsolationLevel);

                if (TheTransaction != null)
                {
                    // Multi-threading and multi-connection 'Sanity Check':
                    // Check if the current Thread is the same Thread that the currently running DB Transaction was started on:
                    // if not, throw Exception!
                    if (!CheckRunningDBTransactionThreadIsCompatible(false))
                    {
                        var Exc1 = new EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(
                            "TDataBase.GetNewOrExistingTransaction would yield DB Transaction that got started on a different " +
                            "Thread and this isn't supported (ADO.NET provider isn't thread-safe!)",
                            ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                            ThreadingHelper.GetCurrentThreadIdentifier());

                        TLogging.Log(Exc1.ToString());

                        throw Exc1;
                    }

                    // Check if the IsolationLevel of the currently running DB Transaction is acceptable for what is
                    // asked for with the ADesiredIsolationLevel and ATryToEnforceIsolationLevel Arguments:
                    // if not, throw Exception!
                    if (!CheckRunningDBTransactionIsolationLevelIsCompatible(ADesiredIsolationLevel,
                            ATryToEnforceIsolationLevel, false))
                    {
                        switch (ATryToEnforceIsolationLevel)
                        {
                            case TEnforceIsolationLevel.eilExact:
                                var Exc2 = new
                                           EDBTransactionIsolationLevelWrongException("Expected IsolationLevel: " +
                                ADesiredIsolationLevel.ToString("G") + " but is: " +
                                TheTransaction.IsolationLevel.ToString("G"));

                                TLogging.Log(Exc2.ToString());

                                throw Exc2;

                            case TEnforceIsolationLevel.eilMinimum:
                                var Exc3 = new
                                           EDBTransactionIsolationLevelTooLowException(
                                "Expected IsolationLevel: at least " + ADesiredIsolationLevel.ToString("G") +
                                " but is: " + TheTransaction.IsolationLevel.ToString("G"));

                                TLogging.Log(Exc3.ToString());

                                throw Exc3;
                        }
                    }
                }

                if (TheTransaction == null)
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(String.Format("GetNewOrExistingTransaction: Creating new DB Transaction with IsolationLevel '{0}'{1}.",
                                ADesiredIsolationLevel, GetDBConnectionIdentifier()));
                    }

                    TheTransaction = BeginTransaction(ADesiredIsolationLevel, false, -1, ATransactionName);

                    ANewTransaction = true;
                }
                else
                {
                    // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet.
                    if (!TheTransaction.Valid)
                    {
                        var Exc4 =
                            new EDBAttemptingToUseTransactionThatIsInvalidException(
                                "TDataBase.GetNewOrExistingTransaction would yield DB Transaction that isn't valid",
                                ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                ThreadingHelper.GetCurrentThreadIdentifier());

                        TLogging.Log(Exc4.ToString());

                        throw Exc4;
                    }

                    // Set Flag that indicates that the Transaction has been re-used instead of freshly created! This Flag can be
                    // inquired using the readonly TDBTransaction.Reused Property!
                    TheTransaction.SetTransactionToReused();

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                    {
                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "GetNewOrExistingTransaction: Re-using (='piggy-backing on') existing DB Transaction{0} with IsolationLevel '{1}'{2} "
                                +
                                "(it got started on Thread {3} in AppDomain '{4}').", TheTransaction.GetDBTransactionIdentifier(),
                                TheTransaction.IsolationLevel,
                                GetDBConnectionIdentifier(),
                                ThreadingHelper.GetThreadIdentifier(FTransaction.ThreadThatTransactionWasStartedOn),
                                TheTransaction.AppDomainNameThatTransactionWasStartedIn));
                    }
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            return TheTransaction;
        }

        #endregion

        #region GetNextSequenceValue

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction)
        {
            // For SQLite and MySQL we directly run commands against the DB (not using Methods of the TDataBase Class), hence
            // we need to co-ordinate the DB access manually. For PostgreSQL and Progress we use Methods of the TDataBase
            // Class that take care of the co-ordination themselves, hence we must not co-ordinate the DB access manually as
            // that would cause a 'deadlock'!
            if ((FDbType == TDBType.SQLite)
                || (FDbType == TDBType.MySQL))
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                return FDataBaseRDBMS.GetNextSequenceValue(ASequenceName, ATransaction, this);
            }
            finally
            {
                // (See comment above)
                if ((FDbType == TDBType.SQLite)
                    || (FDbType == TDBType.MySQL))
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction)
        {
            // For SQLite and MySQL we directly run commands against the DB (not using Methods of the TDataBase Class), hence
            // we need to co-ordinate the DB access manually. For PostgreSQL and Progress we use Methods of the TDataBase
            // Class that take care of the co-ordination themselves, hence we must not co-ordinate the DB access manually as
            // that would cause a 'deadlock'!
            if ((FDbType == TDBType.SQLite)
                || (FDbType == TDBType.MySQL))
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                return FDataBaseRDBMS.GetCurrentSequenceValue(ASequenceName, ATransaction, this);
            }
            finally
            {
                // (See comment above)
                if ((FDbType == TDBType.SQLite)
                    || (FDbType == TDBType.MySQL))
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// restart a sequence with the given value
        /// </summary>
        public void RestartSequence(String ASequenceName, TDBTransaction ATransaction, Int64 ARestartValue)
        {
            // No co-ordination of DB access manually using FCoordinatedDBAccess required as all RDBMS's use Methods of the
            // TDataBase Class that take care of the co-ordination themselves...
            FDataBaseRDBMS.RestartSequence(ASequenceName, ATransaction, this, ARestartValue);
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction. Suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).
        /// </param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <returns>Number of Rows affected.</returns>
        public int ExecuteNonQuery(String ASqlStatement,
            TDBTransaction ATransaction,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            return ExecuteNonQuery(ASqlStatement, ATransaction, true, AParametersArray, ACommitTransaction);
        }

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction. Suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).
        /// </param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <returns>Number of Rows affected.</returns>
        private int ExecuteNonQuery(String ASqlStatement,
            TDBTransaction ATransaction,
            bool AMustCoordinateDBAccess,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            int NumberOfRowsAffected = 0;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".ExecuteNonQuery()", ASqlStatement, AParametersArray);
            }

            try
            {
                if (!ConnectionReady(false))
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }

                using (DbCommand TransactionCommand = Command(ASqlStatement, ATransaction, false, AParametersArray))
                {
                    if (TransactionCommand == null)
                    {
                        // should never get here
                        throw new EOPDBException("Failed to create Command object!");
                    }

                    try
                    {
                        NumberOfRowsAffected = TransactionCommand.ExecuteNonQuery();

                        if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log("Number of rows affected: " + NumberOfRowsAffected.ToString());
                        }
                    }
                    catch (Exception exp)
                    {
                        if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                        {
                            EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                                "EDB40001 exception in TDatabase.ExecuteNonQuery",
                                exp);
                            LogException(e, "Error executing non-query SQL statement.");
                            throw e;
                        }
                        else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                        {
                            EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                                "EDB23505 exception in TDatabase.ExecuteNonQuery",
                                exp);
                            LogException(e, "Error executing non-query SQL statement.");
                            throw e;
                        }

                        LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error executing non-query SQL statement.");
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction(false);
                    }

                    return NumberOfRowsAffected;
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. The transaction's <see cref="IsolationLevel" />
        /// will be <see cref="IsolationLevel.ReadCommitted" />.
        /// Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object.
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable)
        {
            WaitForCoordinatedDBAccess();

            try
            {
                if (ConnectionReady(false))
                {
                    using (TDBTransaction EnclosingTransaction = BeginTransaction(IsolationLevel.ReadCommitted, false))
                    {
                        ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction, false, true);
                    }
                }
                else
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. A Transaction with the desired
        /// <see cref="IsolationLevel" /> is automatically created and committed/rolled back.
        /// Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object.</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />  of the transaction.
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, IsolationLevel AIsolationLevel)
        {
            WaitForCoordinatedDBAccess();

            try
            {
                if (ConnectionReady(false))
                {
                    using (TDBTransaction EnclosingTransaction = BeginTransaction(AIsolationLevel, false))
                    {
                        ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction, false, true);
                    }
                }
                else
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. Suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="ACommitTransaction">On successful execution of all statements the
        /// transaction is committed if set to true, otherwise the transaction is not
        /// committed (useful when the caller wants to do further things in the same
        /// transaction).
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, TDBTransaction ATransaction, bool ACommitTransaction = false)
        {
            ExecuteNonQueryBatch(AStatementHashTable, ATransaction, true, ACommitTransaction);
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. Suitable for parameterised SQL
        /// statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="ACommitTransaction">On successful execution of all statements the
        /// transaction is committed if set to true, otherwise the transaction is not
        /// committed (useful when the caller wants to do further things in the same
        /// transaction).
        /// </param>
        /// <returns>void</returns>
        private void ExecuteNonQueryBatch(Hashtable AStatementHashTable, TDBTransaction ATransaction,
            bool AMustCoordinateDBAccess, bool ACommitTransaction)
        {
            int SqlCommandNumber;
            String CurrentBatchEntryKey = "";
            String CurrentBatchEntrySQLStatement = "";
            IDictionaryEnumerator BatchStatementEntryIterator;
            TSQLBatchStatementEntry BatchStatementEntryValue;

            if (AStatementHashTable == null)
            {
                throw new ArgumentNullException("AStatementHashTable", "This method must be called with an initialized HashTable!!");
            }

            if (AStatementHashTable.Count == 0)
            {
                throw new ArgumentException("ArrayList containing TSQLBatchStatementEntry objects must not be empty!", "AStatementHashTable");
            }

            if (ATransaction == null)
            {
                throw new ArgumentNullException("ATransaction", "This method must be called with an initialized transaction!");
            }

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (!ConnectionReady(false))
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }

                SqlCommandNumber = 0;

                try
                {
                    BatchStatementEntryIterator = AStatementHashTable.GetEnumerator();

                    while (BatchStatementEntryIterator.MoveNext())
                    {
                        BatchStatementEntryValue = (TSQLBatchStatementEntry)BatchStatementEntryIterator.Value;
                        CurrentBatchEntryKey = BatchStatementEntryIterator.Key.ToString();
                        CurrentBatchEntrySQLStatement = BatchStatementEntryValue.SQLStatement;

                        ExecuteNonQuery(CurrentBatchEntrySQLStatement, ATransaction,
                            false, BatchStatementEntryValue.Parameters);

                        SqlCommandNumber = SqlCommandNumber + 1;
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction(false);
                    }
                }
                catch (Exception exp)
                {
                    RollbackTransaction();

                    LogException(exp, CurrentBatchEntrySQLStatement,
                        null,
                        "Exception occured while executing AStatementHashTable entry '" +
                        CurrentBatchEntryKey + "' (#" + SqlCommandNumber.ToString() +
                        ")! (The SQL Statement is a non-query SQL statement.)  All SQL statements executed so far were rolled back.");

                    throw new EDBExecuteNonQueryBatchException(
                        "Exception occured while executing AStatementHashTable entry '" + CurrentBatchEntryKey + "' (#" +
                        SqlCommandNumber.ToString() + ")! Non-query SQL statement: [" + CurrentBatchEntrySQLStatement +
                        "]). All SQL statements executed so far were rolled back.",
                        exp);
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction with the desired <see cref="IsolationLevel" /> and
        /// the transaction is automatically committed. Suitable for
        /// parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /> of the transaction.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).</param>
        /// <returns>Single result as object.</returns>
        public object ExecuteScalar(String ASqlStatement, IsolationLevel AIsolationLevel, DbParameter[] AParametersArray = null)
        {
            object ReturnValue = null;

            WaitForCoordinatedDBAccess();

            try
            {
                if (ConnectionReady(false))
                {
                    using (TDBTransaction EnclosingTransaction = BeginTransaction(AIsolationLevel, false))
                    {
                        try
                        {
                            ReturnValue = ExecuteScalar(ASqlStatement, EnclosingTransaction, false, AParametersArray);
                        }
                        finally
                        {
                            CommitTransaction(false);
                        }
                    }
                }
                else
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).</param>
        /// <returns>Single result as object.</returns>
        public object ExecuteScalar(String ASqlStatement,
            TDBTransaction ATransaction,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            return ExecuteScalar(ASqlStatement, ATransaction, true, AParametersArray, ACommitTransaction);
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).</param>
        /// <returns>Single result as object.</returns>
        private object ExecuteScalar(String ASqlStatement,
            TDBTransaction ATransaction,
            bool AMustCoordinateDBAccess,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            object ReturnValue = null;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".ExecuteScalar()", ASqlStatement, AParametersArray);
            }

            try
            {
                if (!ConnectionReady(false))
                {
                    throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now creating Command(" + ASqlStatement + ")...");
                }

                using (DbCommand TransactionCommand = Command(ASqlStatement, ATransaction, false, AParametersArray))
                {
                    if (TransactionCommand == null)
                    {
                        // should never get here
                        throw new EOPDBException("Failed to create Command object!");
                    }

                    try
                    {
                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now calling Command.ExecuteScalar...");
                        }

                        ReturnValue = TransactionCommand.ExecuteScalar();

                        if (ReturnValue == null)
                        {
                            throw new EOPDBException("Execute Scalar returned no value");
                        }

                        if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log(this.GetType().FullName + ".ExecuteScalar: finished calling Command.ExecuteScalar");
                        }
                    }
                    catch (EOPDBException Exc)
                    {
                        LogExceptionAndThrow(Exc, ASqlStatement, AParametersArray, "Error executing scalar SQL statement.");
                    }
                    catch (Exception exp)
                    {
                        if (TDBExceptionHelper.IsFirstChanceNpgsql40001Exception(exp))
                        {
                            EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                                "EDB40001 exception in TDatabase.ExecuteScalar",
                                exp);
                            LogException(e, "Error executing scalar SQL statement.");
                            throw e;
                        }
                        else if (TDBExceptionHelper.IsFirstChanceNpgsql23505Exception(exp))
                        {
                            EDBTransactionSerialisationException e = new EDBTransactionSerialisationException(
                                "EDB23505 exception in TDatabase.ExecuteScalar",
                                exp);
                            LogException(e, "Error executing scalar SQL statement.");
                            throw e;
                        }
                        else if ((!exp.StackTrace.Contains("IsDBConnectionOK"))
                                 && (!exp.StackTrace.Contains("The Connection is broken")))
                        {
                            LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error executing scalar SQL statement.");
                        }
                        else
                        {
                            throw new EOPDBException(exp);
                        }
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction(false);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
                {
                    TLogging.Log("Result from ExecuteScalar is " + ReturnValue.ToString() + " " + ReturnValue.GetType().ToString());
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            return ReturnValue;
        }

        #endregion

        /// <summary>
        /// Reads a SQL statement from file and removes the comments.
        /// </summary>
        /// <param name="ASqlFilename">.</param>
        /// <param name="ADefines">Defines to be set in the SQL statement.</param>
        /// <returns>SQL statement.</returns>
        public static string ReadSqlFile(string ASqlFilename, SortedList <string, string>ADefines = null)
        {
            string line = null;
            string stmt = "";

            ASqlFilename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                           Path.DirectorySeparatorChar +
                           ASqlFilename;

            // Console.WriteLine("reading " + ASqlFilename);
            using (StreamReader reader = new StreamReader(ASqlFilename))
            {
                if (reader == null)
                {
                    throw new Exception("cannot open file " + ASqlFilename);
                }

                Regex DecommenterRegex = new Regex(@"\s--.*");

                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Trim().StartsWith("--"))
                    {
                        stmt += DecommenterRegex.Replace(line.Trim(), "") + Environment.NewLine;
                    }
                }

                reader.Close();
            }

            if (ADefines != null)
            {
                ProcessTemplate template = new ProcessTemplate(null);
                template.FTemplateCode = new StringBuilder(stmt);

                foreach (string define in ADefines.Keys)
                {
                    string enabled = ADefines[define];

                    if (enabled.Length == 0)
                    {
                        enabled = "enabled";
                    }

                    template.SetCodelet(define, enabled);
                }

                return template.FinishWriting(true).Replace(Environment.NewLine, " ");
            }

            return stmt.Replace(Environment.NewLine, " ");
        }

        /// <summary>
        /// Expands IList items in a parameter list so that `IN (?)' syntax works.
        /// </summary>
        static private void PreProcessCommand(ref String ACommandText, ref DbParameter[] AParametersArray)
        {
            /* Check if there are any parameters which need `IN (?)' expansion. */
            Boolean INExpansionNeeded = false;

            if (AParametersArray != null)
            {
                foreach (DbParameter param in AParametersArray)
                {
                    if (param.Value is TDbListParameterValue)
                    {
                        INExpansionNeeded = true;
                        break;
                    }
                }
            }

            /* Perform the `IN (?)' expansion. */
            if (INExpansionNeeded)
            {
                List <OdbcParameter>NewParametersArray = new List <OdbcParameter>();
                String NewCommandText = "";

                using (IEnumerator <OdbcParameter>ParametersEnumerator = ((IEnumerable <OdbcParameter> )AParametersArray).GetEnumerator())
                {
                    foreach (String SqlPart in ACommandText.Split(new Char[] { '?' }))
                    {
                        NewCommandText += SqlPart;

                        if (!ParametersEnumerator.MoveNext())
                        {
                            /* We're at the end of the string/parameter array */
                            continue;
                        }

                        OdbcParameter param = ParametersEnumerator.Current;

                        // Check if param.Value is of Type TDbListParameterValue; ParamValue will be null in case it isn't
                        var ParamValue = param.Value as TDbListParameterValue;

                        if (ParamValue != null)
                        {
                            Boolean first = true;

                            foreach (OdbcParameter subparam in ParamValue)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    NewCommandText += ", ";
                                }

                                NewCommandText += "?";

                                NewParametersArray.Add(subparam);
                            }

                            /* We had an empty list. */
                            if (first)
                            {
                                NewCommandText += "?";

                                /* `column IN ()' is invalid, use `column IN (NULL)' */
                                param.Value = DBNull.Value;
                                NewParametersArray.Add(param);
                            }
                        }
                        else
                        {
                            NewCommandText += "?";
                            NewParametersArray.Add(param);
                        }
                    }

                    /* Catch any leftover parameters? */
                    while (ParametersEnumerator.MoveNext())
                    {
                        NewParametersArray.Add(ParametersEnumerator.Current);
                    }
                }

                ACommandText = NewCommandText;
                AParametersArray = NewParametersArray.ToArray();

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("PreProcessCommand(): Performed `column IN (?)' expansion, result follows:");
                    LogSqlStatement("PreProcessCommand()", ACommandText, AParametersArray);
                }
            }
        }

        /// <summary>
        /// Tells whether the DB connection is ready to accept commands or whether it is busy.
        /// </summary>
        /// <returns>True if DB connection can accept commands, false if it is busy.</returns>
        public bool ConnectionReady()
        {
            return ConnectionReady(true);
        }

        /// <summary>
        /// Tells whether the DB connection is ready to accept commands or whether it is busy.
        /// </summary>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if DB connection can accept commands, false if it is busy.</returns>
        private bool ConnectionReady(bool AMustCoordinateDBAccess)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FDbType == TDBType.PostgreSQL)
                {
                    // TODO: change when OnStateChangedHandler works for postgresql
                    return FSqlConnection != null && FSqlConnection.State == ConnectionState.Open;
                }

                return FConnectionReady;
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }
        }

        /// <summary>
        /// Updates the FConnectionReady variable with the current ConnectionState.
        /// </summary>
        /// <remarks>
        /// <em>WARNING:</em> This doesn't work with NpgsqlConnection because it never raises the
        /// Event. Therefore the FConnectionReady variable must
        /// never be inquired directly, but only through calling ConnectionReady()!
        /// TODO: revise this comment with more recent Npgsql release (as of Npgsql 2.0.11.92 the Event still isn't raised)
        /// </remarks>
        /// <param name="ASender">Sending object.</param>
        /// <param name="AArgs">StateChange EventArgs.</param>
        private void OnStateChangedHandler(object ASender, StateChangeEventArgs AArgs)
        {
            // Important: In this Method we must NOT co-ordinate the DB access manually as that would cause a 'deadlock'!
            switch (AArgs.CurrentState)
            {
                case ConnectionState.Open:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    FConnectionReady = true;
                    break;

                case ConnectionState.Closed:
                case ConnectionState.Connecting:
                case ConnectionState.Broken:
                    FConnectionReady = false;
                    break;

                default:
                    FConnectionReady = false;
                    break;
            }
        }

        /// <summary>
        /// for debugging, export data table to xml (which can be saved as xml, yml, csv)
        /// </summary>
        /// <param name="ATable"></param>
        /// <returns>XmlDocument containing the DataTable.</returns>
        public static XmlDocument DataTableToXml(DataTable ATable)
        {
            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            foreach (DataRow row in ATable.Rows)
            {
                XmlElement node = doc.CreateElement(TYml2Xml.XMLELEMENT);

                foreach (DataColumn column in ATable.Columns)
                {
                    node.SetAttribute(column.ColumnName, row[column].ToString());
                }

                doc.DocumentElement.AppendChild(node);
            }

            return doc;
        }

        /// <summary>
        /// For debugging purposes only.
        /// Logs the contents of a DataTable.
        /// </summary>
        /// <param name="ATable">The DataTable whose contents should be logged.</param>
        /// <returns>void</returns>
        public static void LogTable(DataTable ATable)
        {
            String Line = "";
            int MaxRows = 10;

            foreach (DataColumn column in ATable.Columns)
            {
                Line = Line + ' ' + column.ColumnName;
            }

            TLogging.Log(Line);

            foreach (DataRow row in ATable.Rows)
            {
                Line = "";

                foreach (DataColumn column in ATable.Columns)
                {
                    Line = Line + ' ' + row[column].ToString();
                }

                if ((MaxRows > 0) || (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE))
                {
                    MaxRows--;
                    TLogging.Log(Line);
                }
                else
                {
                    break;
                }
            }

            if (MaxRows == 0)
            {
                TLogging.Log("The DataTable held more rows (" + ATable.Rows.Count + " in total), but they have been skipped...");
            }
        }

        /// <summary>
        /// For debugging purposes.
        /// Formats the sql query so that it is easily readable
        /// (mainly inserting line breaks before AND).
        /// </summary>
        /// <param name="s">The sql statement that should be formatted.</param>
        /// <returns>Formatted sql statement.</returns>
        public static string FormatSQLStatement(string s)
        {
            string ReturnValue;
            char char13 = (char)13;
            char char10 = (char)10;

            ReturnValue = s;

            ReturnValue = ReturnValue.Replace(char13, ' ').Replace(char10, ' ');
            ReturnValue = ReturnValue.Replace(Environment.NewLine, " ");
            ReturnValue = ReturnValue.Replace(" FROM ", Environment.NewLine + "FROM ");
            ReturnValue = ReturnValue.Replace(" WHERE ", Environment.NewLine + "WHERE ");
            ReturnValue = ReturnValue.Replace(" UNION ", Environment.NewLine + "UNION ");
            ReturnValue = ReturnValue.Replace(" AND ", Environment.NewLine + "AND ");
            ReturnValue = ReturnValue.Replace(" OR ", Environment.NewLine + "OR ");
            ReturnValue = ReturnValue.Replace(" GROUP BY ", Environment.NewLine + "GROUP BY ");
            ReturnValue = ReturnValue.Replace(" ORDER BY ", Environment.NewLine + "ORDER BY ");

            return ReturnValue;
        }

        /// <summary>
        /// This Method checks if the current user has enough access rights to execute the query
        /// passed in in Argument <paramref name="ASQLStatement" />.
        /// <para>This Method needs to be implemented by a derived Class, that knows about the
        /// users' access rights. The implementation here simply returns true...</para>
        /// </summary>
        /// <returns>True if the user has access, false if access is denied.
        /// The implementation here simply returns true, though!
        /// </returns>
        public virtual bool HasAccess(String ASQLStatement)
        {
            return true;
        }

        /// <summary>
        /// Logs the SQL statement and the parameters;
        /// use DebugLevel to define behaviour.
        /// </summary>
        /// <param name="ASqlStatement">SQL Statement that should be logged.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement. Can be null.</param>
        /// <returns>void</returns>
        private void LogSqlStatement(String ASqlStatement, DbParameter[] AParametersArray)
        {
            LogSqlStatement("", ASqlStatement, AParametersArray);
        }

        /// <summary>
        /// Logs the SQL statement and the parameters;
        /// use DebugLevel to define behaviour.
        /// </summary>
        /// <param name="AContext">Context in which the logging takes place (eg. Method name).</param>
        /// <param name="ASqlStatement">SQL Statement that should be logged.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement. Can be null.</param>
        /// <returns>void</returns>
        public static void LogSqlStatement(String AContext, String ASqlStatement, DbParameter[] AParametersArray)
        {
            String PrintContext = "";

            if (AContext != String.Empty)
            {
                PrintContext = "(Context: '" + AContext + "')" + Environment.NewLine;
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                TLogging.Log(PrintContext +
                    "The SQL query is: " + Environment.NewLine + FormatSQLStatement(ASqlStatement));
            }

            if ((TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
                && (AParametersArray != null))
            {
                Int32 Counter = 1;

                foreach (OdbcParameter Parameter in AParametersArray)
                {
                    if (Parameter.Value == System.DBNull.Value)
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + " DBNull" + ' ' + Parameter.Value.GetType().ToString() + ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType));
                    }
                    else if (Parameter.Value == null)
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + " " + Parameter.ParameterName + " (no value specified) " +
                            ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString());
                    }
                    else
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + ' ' + Parameter.Value.ToString() + ' ' + Parameter.Value.GetType().ToString() +
                            ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString());
                    }

                    Counter++;
                }
            }
        }

        /// <summary>
        /// Logs an Exception and re-throws it afterwards.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogExceptionAndThrow(Exception AException, string AContext)
        {
            LogException(AException, "", null, AContext, true);
        }

        /// <summary>
        /// Logs an Exception and re-throws it afterwards.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AParametersArray">Parameters for the query.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogExceptionAndThrow(Exception AException, string ASqlStatement, DbParameter[] AParametersArray, string AContext)
        {
            LogException(AException, ASqlStatement, AParametersArray, AContext, true);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogException(Exception AException, string AContext)
        {
            LogException(AException, "", null, AContext, false);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AParametersArray">Parameters for the query.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        private void LogException(Exception AException, string ASqlStatement, DbParameter[] AParametersArray, string AContext)
        {
            LogException(AException, ASqlStatement, AParametersArray, AContext, false);
        }

        /// <summary>
        /// Logs an Exception.
        /// <para>Custom handling of OdbcException and NpgsqlException ensure that
        /// the maximum of information that is available from the DB's is logged.</para>
        /// </summary>
        /// <param name="AException">Exception that should be logged.</param>
        /// <param name="ASqlStatement">SQL Statement that caused the Exception (will be logged).</param>
        /// <param name="AParametersArray">Parameters for the query.</param>
        /// <param name="AContext">Context where the Exception happened
        /// (will be logged). Can be empty.</param>
        /// <param name="AThrowExceptionAfterLogging">If set to true, the Exception that is passed in in Argument
        /// <paramref name="AException" /> will be re-thrown.</param>
        /// <exception cref="Exception">Re-throws the Exception that is passed in in Argument
        /// <paramref name="AException" /> if <paramref name="AThrowExceptionAfterLogging" /> is set to true.</exception>
        private void LogException(Exception AException,
            string ASqlStatement,
            DbParameter[] AParametersArray,
            string AContext,
            bool AThrowExceptionAfterLogging)
        {
            string ErrorMessage = "";
            string FormattedSqlStatement = "";

            if ((AException.GetType() == typeof(NpgsqlException)) && (((NpgsqlException)AException).Code == "25P02"))
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
                {
                    TLogging.Log(
                        TLogging.LOG_PREFIX_INFO + "NpgsqlException with code '25P02' raised: The transaction was cancelled by user command.");
                }

                return;
            }

            if (ASqlStatement != String.Empty)
            {
                ASqlStatement = FDataBaseRDBMS.FormatQueryRDBMSSpecific(ASqlStatement);

                FormattedSqlStatement = "The SQL Statement was: " + Environment.NewLine +
                                        ASqlStatement + Environment.NewLine;

                if (AParametersArray != null && AParametersArray.Length > 0 && AParametersArray[0] is OdbcParameter)
                {
                    Int32 Counter = 1;

                    foreach (OdbcParameter Parameter in AParametersArray)
                    {
                        if (Parameter.Value == System.DBNull.Value)
                        {
                            FormattedSqlStatement +=
                                "Parameter: " + Counter.ToString() + ' ' + Parameter.ParameterName + " DBNull" + ' ' + Parameter.Value.GetType().ToString() + ' ' +
                                Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) +
                                Environment.NewLine;
                        }
                        else if (Parameter.Value == null)
                        {
                            TLogging.Log(
                                "Parameter: " + Counter.ToString() + ' ' + Parameter.ParameterName + " (no value specified) " +
                                ' ' +
                                Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString());
                        }
                        else
                        {
                            FormattedSqlStatement +=
                                "Parameter: " + Counter.ToString() + ' ' + Parameter.ParameterName + ' ' + Parameter.Value.ToString() + ' ' +
                                Parameter.Value.GetType().ToString() +
                                ' ' +
                                Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString() +
                                Environment.NewLine;
                        }

                        Counter++;
                    }
                }
            }

            FDataBaseRDBMS.LogException(AException, ref ErrorMessage);

            TLogging.Log(AContext + Environment.NewLine +
                String.Format("on Thread {0} in AppDomain '{1}'", ThreadingHelper.GetCurrentThreadIdentifier(),
                    AppDomain.CurrentDomain.FriendlyName) +
                (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES ?
                 GetDBConnectionIdentifier() : "") + Environment.NewLine + FormattedSqlStatement +
                "Possible cause: " + AException.ToString() + Environment.NewLine + ErrorMessage);

            TLogging.LogStackTrace(TLoggingType.ToLogfile);

            if (AThrowExceptionAfterLogging)
            {
                if (!String.IsNullOrEmpty(AContext))
                {
                    throw new EOPDBException("[Context: " + AContext + "]", AException);
                }
                else
                {
                    throw new EOPDBException(AException);
                }
            }
        }

        #region CoordinatedDBAccess

        internal void WaitForCoordinatedDBAccess()
        {
            const string StrWaitingMessage =
                "Waiting to obtain Thread-safe access to the Database Abstraction Layer... " + Utilities.StrThreadAndAppDomainCallInfo;
            const string StrWaitingSuccessful = "Obtained Thread-safe access to the Database Abstraction Layer... " +
                                                Utilities.StrThreadAndAppDomainCallInfo;

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES)
            {
                TLogging.Log(String.Format(StrWaitingMessage, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName) + Environment.NewLine +
                    "  StackTrace: " + new StackTrace(true).ToString());
            }
            else if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log(String.Format(StrWaitingMessage, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName));
            }

            if (!FCoordinatedDBAccess.Wait(FWaitingTimeForCoordinatedDBAccess))
            {
                throw new EDBCoordinatedDBAccessWaitingTimeExceededException(
                    String.Format("Failed to obtain co-ordinated " +
                        "(=Thread-safe) access to the Database Abstraction Layer (waiting time [{0} ms] exceeded). " +
                        "(Call performed in Thread {1})!", FWaitingTimeForCoordinatedDBAccess,
                        ThreadingHelper.GetCurrentThreadIdentifier()));
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES)
            {
                TLogging.Log(String.Format(StrWaitingSuccessful, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName) + Environment.NewLine +
                    "  StackTrace: " + new StackTrace(true).ToString());
            }
            else if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log(String.Format(StrWaitingSuccessful, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName));
            }
        }

        internal void ReleaseCoordinatedDBAccess()
        {
            const string StrReleasedCoordinatedDBAccess =
                "Released Thread-safe access to the Database Abstraction Layer. " + Utilities.StrThreadAndAppDomainCallInfo + "...";

            FCoordinatedDBAccess.Release();

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES)
            {
                TLogging.Log(String.Format(
                        StrReleasedCoordinatedDBAccess, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName) + Environment.NewLine +
                    "  StackTrace: " + new StackTrace(true).ToString());
            }
            else if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log(String.Format(StrReleasedCoordinatedDBAccess, ThreadingHelper.GetCurrentThreadIdentifier(),
                        AppDomain.CurrentDomain.FriendlyName));
            }
        }

        #endregion

        #region AutoTransactions

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ref ATransaction, ref ASubmissionOK, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                ref ATransaction, ref ASubmissionOK, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel, ref ATransaction,
                ref ASubmissionOK, true, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel,
                ref ATransaction, ref ASubmissionOK, true, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the values of <paramref name="ASubmissionOK"/>
        /// and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmissionOK"/> is true, or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK, bool ACommitTransaction,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ref ATransaction, ref ASubmissionOK,
                ACommitTransaction, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the values of <paramref name="ASubmissionOK"/>
        /// and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmissionOK"/> is true, or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK, bool ACommitTransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                ref ATransaction, ref ASubmissionOK, ACommitTransaction, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the values of <paramref name="ASubmissionOK"/>
        /// and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmissionOK"/> is true, or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, bool ACommitTransaction,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel, ref ATransaction,
                ref ASubmissionOK, ACommitTransaction, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the values of <paramref name="ASubmissionOK"/>
        /// and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmissionOK"/> is true, or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, bool ACommitTransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            bool NewTransaction;
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = GetNewOrExistingTransaction(ADesiredIsolationLevel,
                ATryToEnforceIsolationLevel, out NewTransaction, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know whether an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmissionOK, NewTransaction && ACommitTransaction);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ref ATransaction, ref ASubmitChangesResult,
                String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                ref ATransaction, ref ASubmitChangesResult, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em> : Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel, ref ATransaction,
                ref ASubmitChangesResult, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em> : Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel,
                ref ATransaction, ref ASubmitChangesResult, true, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/> <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmitChangesResult"/> is <see cref="TSubmitChangesResult.scrOK"/>,
        /// or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, bool ACommitTransaction,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, ref ATransaction, ref ASubmitChangesResult,
                ACommitTransaction, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/> <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmitChangesResult"/> is <see cref="TSubmitChangesResult.scrOK"/>,
        /// or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, bool ACommitTransaction,
            string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                ref ATransaction, ref ASubmitChangesResult, ACommitTransaction, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em> : Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the values of <paramref name="ASubmitChangesResult"/>
        /// and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/> <em>and</em> when
        /// <paramref name="ACommitTransaction"/> is true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Controls whether a Commit is issued when
        /// <paramref name="ASubmitChangesResult"/> is <see cref="TSubmitChangesResult.scrOK"/>,
        /// or whether nothing should happen in that case (also no Rollback!).</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, bool ACommitTransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            bool NewTransaction;
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = GetNewOrExistingTransaction(ADesiredIsolationLevel,
                ATryToEnforceIsolationLevel, out NewTransaction, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmitChangesResult, NewTransaction && ACommitTransaction);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoReadTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoReadTransaction(ADesiredIsolationLevel, ref ATransaction, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// Method with the Argument <paramref name="ADesiredIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoReadTransaction(IsolationLevel ADesiredIsolationLevel,
            ref TDBTransaction ATransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoReadTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact,
                ref ATransaction, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoReadTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction,
            Action AEncapsulatedDBAccessCode)
        {
            GetNewOrExistingAutoReadTransaction(ADesiredIsolationLevel, ATryToEnforceIsolationLevel, ref ATransaction,
                String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// Method with the Arguments <paramref name="ADesiredIsolationLevel"/> and
        /// <paramref name="ATryToEnforceIsolationLevel"/> and returns the DB Transaction that
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/>
        /// either started or simply returned in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATryToEnforceIsolationLevel">Only has an effect if there is an already
        /// existing Transaction. See the 'Exceptions' section for possible Exceptions that may be thrown.
        /// </param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out bool, string)"/> either
        /// started or simply returned.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void GetNewOrExistingAutoReadTransaction(IsolationLevel ADesiredIsolationLevel,
            TEnforceIsolationLevel ATryToEnforceIsolationLevel, ref TDBTransaction ATransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            bool NewTransaction;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = GetNewOrExistingTransaction(ADesiredIsolationLevel,
                ATryToEnforceIsolationLevel, out NewTransaction, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                if (NewTransaction)
                {
                    // A DB Transaction Rollback is issued regardless of whether an unhandled Exception was thrown, or not!
                    //
                    // Reasoning as to why we don't Commit the DB Transaction:
                    // If the DB Transaction that was used for here for reading was re-used and if in earlier code paths data was written
                    // to the DB using that DB Transaction then that data will get Committed here although we are only reading here,
                    // and in doing so we would not know here 'what' we would be unknowingly committing to the DB!
                    RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(ref ATransaction, ref ASubmissionOK, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(-1, ref ATransaction, ref ASubmissionOK, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(Int16 ARetryAfterXSecWhenUnsuccessful, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(ARetryAfterXSecWhenUnsuccessful, ref ATransaction, ref ASubmissionOK, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(Int16 ARetryAfterXSecWhenUnsuccessful, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(ARetryAfterXSecWhenUnsuccessful, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmissionOK);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(ref ATransaction, ref ASubmitChangesResult, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(-1, ref ATransaction, ref ASubmitChangesResult, ATransactionName,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(Int16 ARetryAfterXSecWhenUnsuccessful, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(ARetryAfterXSecWhenUnsuccessful, ref ATransaction, ref ASubmitChangesResult, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(Int16 ARetryAfterXSecWhenUnsuccessful, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(ARetryAfterXSecWhenUnsuccessful, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmitChangesResult);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful) and returns
        /// the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, ref ATransaction, ref ASubmissionOK, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful) and returns
        /// the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            ref bool ASubmissionOK, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, -1, ref ATransaction, ref ASubmissionOK, ATransactionName,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK,
            Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ref ATransaction, ref ASubmissionOK,
                String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, ref bool ASubmissionOK, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmissionOK);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful) and returns
        /// the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, ref ATransaction, ref ASubmitChangesResult, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful) and returns
        /// the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, -1, ref ATransaction, ref ASubmitChangesResult, ATransactionName,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ref ATransaction,
                ref ASubmitChangesResult, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Committing / Rolling Back of the DB Transaction automatically, depending whether an
        /// Exception occured (Rollback always issued!) and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmitChangesResult);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(ref TDBTransaction ATransaction,
            Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(ref ATransaction, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method, supplying -1 for that Methods' 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry
        /// when unsuccessful) and returns the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(ref TDBTransaction ATransaction, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(-1, ref ATransaction, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(ARetryAfterXSecWhenUnsuccessful, ref ATransaction, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(short, string)"/>
        /// Method with the Argument <paramref name="ARetryAfterXSecWhenUnsuccessful"/> and returns
        /// the DB Transaction that <see cref="BeginTransaction(short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(ARetryAfterXSecWhenUnsuccessful, ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // A DB Transaction Rollback is issued regardless of whether an unhandled Exception was thrown, or not!
                //
                // Reasoning as to why we don't Commit the DB Transaction:
                // If the DB Transaction that was used for here for reading was re-used and if in earlier code paths data was written
                // to the DB using that DB Transaction then that data will get Committed here although we are only reading here,
                // and in doing so we would not know here 'what' we would be unknowingly committing to the DB!
                RollbackTransaction();
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful)
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(AIsolationLevel, ref ATransaction, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Argument <paramref name="AIsolationLevel"/> and -1 for that Methods'
        /// 'ARetryAfterXSecWhenUnsuccessful' Argument (meaning: no retry when unsuccessful)
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction,
            string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(AIsolationLevel, -1, ref ATransaction, ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ref ATransaction, String.Empty,
                AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, string ATransactionName, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ref ATransaction, true,
                ATransactionName, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        private void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, bool AMustCoordinateDBAccess, Action AEncapsulatedDBAccessCode)
        {
            BeginAutoReadTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful, ref ATransaction,
                AMustCoordinateDBAccess, String.Empty, AEncapsulatedDBAccessCode);
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Calls the
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// Method with the Arguments <paramref name="AIsolationLevel"/> and
        /// <paramref name="ARetryAfterXSecWhenUnsuccessful"/>
        /// and returns the DB Transaction that
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/>
        /// started in <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="AIsolationLevel"><see cref="IsolationLevel" /> that is desired.</param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified (in seconds).</param>
        /// <param name="ATransaction">The DB Transaction that the Method
        /// <see cref="BeginTransaction(IsolationLevel, short, string)"/> started.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <param name="ATransactionName">Name of the DB Transaction. It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        private void BeginAutoReadTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful,
            ref TDBTransaction ATransaction, bool AMustCoordinateDBAccess, string ATransactionName,
            Action AEncapsulatedDBAccessCode)
        {
            // Execute the Method that we are 'encapsulating' inside the present Method. (The called Method has no 'automaticness'
            // regarding Exception Handling.)
            ATransaction = BeginTransaction(AIsolationLevel, AMustCoordinateDBAccess, ARetryAfterXSecWhenUnsuccessful,
                ATransactionName);

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // A DB Transaction Rollback is issued regardless of whether an unhandled Exception was thrown, or not!
                //
                // Reasoning as to why we don't Commit the DB Transaction:
                // If the DB Transaction that was used for here for reading was re-used and if in earlier code paths data was written
                // to the DB using that DB Transaction then that data will get Committed here although we are only reading here,
                // and in doing so we would not know here 'what' we would be unknowingly committing to the DB!
                RollbackTransaction(AMustCoordinateDBAccess);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/> and handles the Committing / Rolling Back
        /// of that DB Transaction automatically, depending whether an Exception occured (Rollback always issued!)
        /// and on the value of <paramref name="ASubmissionOK"/>.
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void AutoTransaction(ref TDBTransaction ATransaction, ref bool ASubmissionOK, Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmissionOK);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/> and handles the Committing / Rolling Back
        /// of that DB Transaction automatically, depending whether an Exception occured (Rollback always issued!)
        /// and on the value of <paramref name="ASubmitChangesResult"/>.
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void AutoTransaction(ref TDBTransaction ATransaction, ref TSubmitChangesResult ASubmitChangesResult, Action AEncapsulatedDBAccessCode)
        {
            bool ExceptionThrown = true;

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();

                // Once execution gets to here we know that no unhandled Exception was thrown
                ExceptionThrown = false;
            }
            finally
            {
                // We can get to here in two ways:
                //   1) no unhandled Exception was thrown;
                //   2) an unhandled Exception was thrown.

                // The next Method that gets called will know wheter an unhandled Exception has be thrown (or not) by inspecting the
                // 'ExceptionThrown' Variable and will act accordingly!
                AutoTransCommitOrRollback(ExceptionThrown, ASubmitChangesResult);
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Takes an instance of a running DB Transaction
        /// in Argument <paramref name="ATransaction"/>.
        /// Handles the Rolling Back of the DB Transaction automatically - a <em>Rollback is always issued</em>,
        /// whether an Exception occured, or not!
        /// </summary>
        /// <param name="ATransaction">Instance of a running DB Transaction.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public void AutoReadTransaction(ref TDBTransaction ATransaction, Action AEncapsulatedDBAccessCode)
        {
            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();
            }
            finally
            {
                // A DB Transaction Rollback is issued regardless of whether an unhandled Exception was thrown, or not!
                //
                // Reasoning as to why we don't Commit the DB Transaction:
                // If the DB Transaction that was used for here for reading was re-used and if in earlier code paths data was written
                // to the DB using that DB Transaction then that data will get Committed here although we are only reading here,
                // and in doing so we would not know here 'what' we would be unknowingly committing to the DB!
                RollbackTransaction();
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Works on the basis of the currently running DB Transaction
        /// and handles the Committing / Rolling Back of that DB Transaction, depending whether an Exception occured
        /// (indicated with Argument <paramref name="AExceptionThrown"/>) and on the values of
        /// Arguments <paramref name="ASubmissionOK"/> and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="AExceptionThrown">Set to true if an Exception occured.</param>
        /// <param name="ASubmissionOK">Controls whether a Commit (when true) or Rollback (when false) is issued.</param>
        /// <param name="ACommitTransaction">Set this to false if no Exception was thrown, but when the running
        /// DB Transaction should still not get committed.</param>
        private void AutoTransCommitOrRollback(bool AExceptionThrown, bool ASubmissionOK, bool ACommitTransaction = true)
        {
            if ((!AExceptionThrown)
                && ASubmissionOK)
            {
                // While everthing is fine, the calling code might decide not to Commit the DB Transaction!
                if (ACommitTransaction)
                {
                    TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRACE, "AutoTransCommitOrRollback: Commit");

                    CommitTransaction();
                }
            }
            else
            {
                TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRACE, "AutoTransCommitOrRollback: Rollback");

                RollbackTransaction();
            }
        }

        /// <summary>
        /// <em>Automatic Transaction Handling</em>: Works on the basis of the currently running DB Transaction
        /// and handles the Committing / Rolling Back of that DB Transaction, depending whether an Exception occured
        /// (indicated with Argument <paramref name="AExceptionThrown"/>) and on the values of
        /// Arguments <paramref name="ASubmitChangesResult"/> and <paramref name="ACommitTransaction"/>.
        /// </summary>
        /// <param name="AExceptionThrown">Set to true if an Exception occured.</param>
        /// <param name="ASubmitChangesResult">Controls whether a Commit (when it is
        /// <see cref="TSubmitChangesResult.scrOK"/>) or Rollback (when it has a different value) is issued.</param>
        /// <param name="ACommitTransaction">Set this to false if no Exception was thrown, but when the running
        /// DB Transaction should still not get committed.</param>
        private void AutoTransCommitOrRollback(bool AExceptionThrown, TSubmitChangesResult ASubmitChangesResult, bool ACommitTransaction = true)
        {
            if ((!AExceptionThrown)
                && (ASubmitChangesResult == TSubmitChangesResult.scrOK))
            {
                // While everthing is fine, the calling code might decide not to Commit the DB Transaction!
                if (ACommitTransaction)
                {
                    TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRANSACTION, "AutoTransCommitOrRollback: Commit");
                    CommitTransaction();
                }
            }
            else
            {
                TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRANSACTION, "AutoTransCommitOrRollback: Rollback");
                RollbackTransaction();
            }
        }

        #endregion
    }

    #region TDataAdapterCanceller

    /// <summary>
    /// Provides a safe means to cancel the Fill operation of an associated <see cref="DbDataAdapter"/>.
    /// </summary>
    public sealed class TDataAdapterCanceller
    {
        readonly DbDataAdapter FDataAdapter;

        internal TDataAdapterCanceller(DbDataAdapter ADataAdapter)
        {
            FDataAdapter = ADataAdapter;
        }

        /// <summary>
        /// Call this Method to cancel the Fill operation of the associated <see cref="DbDataAdapter"/>.
        /// </summary>
        /// <remarks><em>IMPORTANT:</em> This Method <em>MUST</em> be called on a separate Thread as otherwise the cancellation
        /// will not work correctly (this is an implementation detail of ADO.NET!).</remarks>
        public void CancelFillOperation()
        {
            FDataAdapter.SelectCommand.Cancel();
        }
    }

    #endregion

    #region TSQLBatchStatementEntry

    /// <summary>
    /// Represents the Value of an entry in a HashTable for use in calls to one of the
    /// <c>TDataBase.ExecuteNonQueryBatch</c> Methods.
    /// </summary>
    /// <remarks>Once instantiated, Batch Statment Entry values can
    /// only be read!</remarks>
    public class TSQLBatchStatementEntry
    {
        /// <summary>Holds the SQL Statement for one Batch Statement Entry</summary>
        private string FSQLStatement;

        /// <summary>Holds the Parameters for a Batch Entry (optional)</summary>
        private DbParameter[] FParametersArray;

        /// <summary>
        /// SQL Statement for one Batch Entry.
        /// </summary>
        public String SQLStatement
        {
            get
            {
                return FSQLStatement;
            }
        }

        /// <summary>
        /// Parameters for a Batch Entry (optional).
        /// </summary>
        public DbParameter[] Parameters
        {
            get
            {
                return FParametersArray;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ASQLStatement">SQL Statement for one Batch Entry.</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement (can be null).</param>
        /// <returns>void</returns>
        public TSQLBatchStatementEntry(String ASQLStatement, DbParameter[] AParametersArray)
        {
            FSQLStatement = ASQLStatement;
            FParametersArray = AParametersArray;
        }
    }

    #endregion

    /// <summary>
    /// A generic Class for managing all kinds of ADO.NET Database Transactions -
    /// to be used instead of concrete ADO.NET Transaction objects, eg. <see cref="OdbcTransaction" />
    /// or NpgsqlTransaction, etc. Effectively wraps ADO.NET Transaction objects.
    /// </summary>
    /// <remarks>
    /// <em>IMPORTANT:</em> This Transaction Class does not have Commit or
    /// Rollback methods! This is so that the programmers are forced to use the
    /// CommitTransaction and RollbackTransaction methods of the <see cref="TDataBase" /> Class.
    /// <para>
    /// The reasons for this:
    /// <list type="bullet">
    /// <item><see cref="TDataBase" /> can know whether a Transaction is
    /// running (unbelievably, there is no way to find this out through ADO.NET!)</item>
    /// <item><see cref="TDataBase" /> can log Commits and Rollbacks. Another benefit of using this
    /// Class instead of a concrete implementation of ADO.NET Transaction Classes
    /// (eg. <see cref="OdbcTransaction" />) is that it is not tied to a specific ADO.NET
    /// provider, therefore making it easier to use a different ADO.NET provider than ODBC.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public class TDBTransaction : object, IDisposable
    {
        /// <summary>Holds the DbTransaction that we are wrapping inside this class.</summary>
        private DbTransaction FWrappedTransaction;
        private TDataBase FTDataBaseInstanceThatTransactionBelongsTo;
        private bool FReused = false;
        private Thread FThreadThatTransactionWasStartedOn;
        private AppDomain FAppDomainThatTransactionWasStartedIn;
        private StackTrace FStackTraceAtPointOfTransactionStart;
        private System.Guid FTransactionIdentifier;
        private System.Guid FConnectionIdentifier;
        private string FTransactionName;

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies a DB Transaction once it got created.
        /// It is used for internal 'sanity checks'. It also gets logged and hence can aid debugging (also useful for
        /// Unit Testing).</summary>
        public System.Guid TransactionIdentifier
        {
            get
            {
                return FTransactionIdentifier;
            }
        }

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies the DB Connection that this
        /// DB Transaction is running against. It is used for internal 'sanity checks'. It also gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</summary>
        public System.Guid ConnectionIdentifier
        {
            get
            {
                return FConnectionIdentifier;
            }
        }

        /// <summary>
        /// Instance of <see cref="TDataBase"/> that this Transaction belongs to.
        /// <para>
        /// Can be used e.g. in Static Methods to get the <see cref="TDataBase"/> instance that a <see cref="TDBTransaction"/>
        /// object was instantiated on. This <see cref="TDataBase"/> instance can then be used to execute calls on the proper
        /// <see cref="TDataBase"/> instance without needing to have this instance readily in a given context
        /// (thinking of static Methods in the 'Typed DataStore' here)!
        /// </para>
        /// </summary>
        public TDataBase DataBaseObj
        {
            get
            {
                return FTDataBaseInstanceThatTransactionBelongsTo;
            }
        }

        /// <summary>
        /// Name of the DB Transaction (optional). It gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        public string TransactionName
        {
            get
            {
                return FTransactionName;
            }
        }


        /// <summary>
        /// Database connection to which the Transaction belongs.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return FWrappedTransaction.Connection;
            }
        }

        /// <summary>
        /// <see cref="IsolationLevel" /> of the Transaction.
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                return FWrappedTransaction.IsolationLevel;
            }
        }

        /// <summary>
        /// DbTransaction that is wrapped in an instance of this class, i.e. which the instance of this class represents.
        /// <para><em><b>WARNING:</b> Do not do anything
        /// with this Object other than inspecting it; the correct
        /// working of Transactions in the <see cref="TDataBase" />
        /// Object relies on the fact that <see cref="TDataBase" /> manages <em>everything</em> about
        /// Transactions!!!</em>
        /// </para>
        /// </summary>
        public DbTransaction WrappedTransaction
        {
            get
            {
                return FWrappedTransaction;
            }
        }

        /// <summary>
        /// True if the Transaction has been re-used at least once by way of calling one of the
        /// 'TDataBase.GetNewOrExistingTransaction' or 'TDataBase.GetNewOrExistingAutoTransaction' Methods.
        /// </summary>
        public bool Reused
        {
            get
            {
                return FReused;
            }
        }

        /// <summary>
        /// True if the Transaction hasn't been Committed or Rolled Back, otherwise false.
        /// </summary>
        public bool Valid
        {
            get
            {
                if (FWrappedTransaction != null)
                {
                    return FWrappedTransaction.Connection != null;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Name of the AppDomain that the DB Transaction was started in. It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).
        /// </summary>
        public string AppDomainNameThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn.FriendlyName;
            }
        }

        /// <summary>
        /// AppDomain that the DB Transaction was started in.
        /// </summary>
        internal AppDomain AppDomainThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn;
            }
        }

        /// <summary>
        /// Thread that the DB Transaction was started on. It is used for internal 'sanity checks'. It also gets logged and
        /// hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        internal Thread ThreadThatTransactionWasStartedOn
        {
            get
            {
                return FThreadThatTransactionWasStartedOn;
            }
        }

        /// <summary>
        /// StackTrace at the point of the starting of the DB Transaction. This is kept so it can get logged and hence it
        /// can aid debugging.
        /// </summary>
        internal StackTrace StackTraceAtPointOfTransactionStart
        {
            get
            {
                return FStackTraceAtPointOfTransactionStart;
            }
        }

        /// <summary>
        /// Constructor for a <see cref="TDBTransaction" /> Object.
        /// </summary>
        /// <param name="ATransaction">The concrete <see cref="DbTransaction"/> object that <see cref="TDBTransaction" />
        /// should represent.</param>
        /// <param name="AConnectionIdentifier">ConnectionIdentifier of the DB Connection that this DB Transaction
        /// is running against.</param>
        /// <param name="ATDataBaseInstanceThatTransactionBelongsTo">Instance of <see cref="TDataBase"/> that
        /// owns this <see cref="TDBTransaction"/> instance.</param>
        /// <param name="AReused">Set to true to make the new instance return 'true' right away when its
        /// <see cref="Reused"/> Property gets inquired (default=false).</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        public TDBTransaction(DbTransaction ATransaction,
            System.Guid AConnectionIdentifier,
            TDataBase ATDataBaseInstanceThatTransactionBelongsTo,
            bool AReused = false,
            string ATransactionName = "")
        {
            FTransactionIdentifier = System.Guid.NewGuid();
            FTransactionName = ATransactionName;

            FWrappedTransaction = ATransaction;
            FReused = AReused;

            FTDataBaseInstanceThatTransactionBelongsTo = ATDataBaseInstanceThatTransactionBelongsTo;
            FAppDomainThatTransactionWasStartedIn = AppDomain.CurrentDomain;
            FThreadThatTransactionWasStartedOn = Thread.CurrentThread;
            FStackTraceAtPointOfTransactionStart = new StackTrace(true);
            FConnectionIdentifier = AConnectionIdentifier;
        }

        /// <summary>
        /// This Method must only get called from one of the 'TDataBase.GetNewOrExistingTransaction' Methods!
        /// </summary>
        internal void SetTransactionToReused()
        {
            FReused = true;
        }

        /// <summary>
        /// Returns a string containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too. Gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        /// <returns>String containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too.
        /// </returns>
        public string GetDBTransactionIdentifier()
        {
            return " (Trans.Identifier: " + FTransactionIdentifier + ")" +
                   (FTransactionName != String.Empty ? String.Format(" (Transaction Name: {0})", FTransactionName) : "");
        }

        #region Dispose pattern

        /// <summary>
        /// Releases all resources used by the <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and <see cref="Connection" />).
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the  <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and optionally releases the managed resources of that object.
        /// </summary>
        /// <param name="ADisposing">True to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                if (FWrappedTransaction != null)
                {
                    FWrappedTransaction.Dispose();
                    FWrappedTransaction = null;
                }
            }
        }

        #endregion
    }


    /// <summary>
    /// A list of parameters which should be expanded into an `IN (?)' context.
    /// </summary>
    /// <example>
    /// Simply use the following style in your .sql file:
    /// <code>
    ///   SELECT * FROM table WHERE column IN (?)
    /// </code>
    ///
    /// Then, to test if <c>column</c> is the string <c>"First"</c>,
    /// <c>"Second"</c>, or <c>"Third"</c>, set the <c>OdbcParameter.Value</c>
    /// property to a <c>TDbListParameterValue</c> instance. You
    /// can use the
    /// <c>TDbListParameterValue.OdbcListParameterValue()</c>
    /// function to produce an <c>OdbcParameter</c> with an
    /// appropriate <c>Value</c> property.
    /// <code>
    /// OdbcParameter[] parameters = new OdbcParamter[]
    /// {
    ///     TDbListParameterValue(param_grdCommitmentStatusChoices", OdbcType.NChar,
    ///         new String[] { "First", "Second", "Third" }),
    /// };
    /// </code>
    /// </example>
    public class TDbListParameterValue : IEnumerable <OdbcParameter>
    {
        private IEnumerable SubValues;

        /// <summary>
        /// The OdbcParameter from which sub-parameters are Clone()d.
        /// </summary>
        public OdbcParameter OdbcParam;

        /// <summary>
        /// Create a list parameter, such as is used for 'column IN (?)' in
        /// SQL queries, from any IEnumerable object.
        /// </summary>
        /// <param name="name">The ParameterName to use when creating OdbcParameters.</param>
        /// <param name="type">The OdbcType of the produced OdbcParameters.</param>
        /// <param name="value">An enumerable collection of objects.
        /// If there are no objects in the enumeration, then the resulting
        /// query will look like <c>column IN (NULL)</c> because
        /// <c>column IN ()</c> is invalid. To avoid the case where
        /// the query should not match any rows and <c>column</c>
        /// may be NULL, use an expression like <c>(? AND column IN (?)</c>.
        /// Set the first parameter to FALSE if the list is empty and
        /// TRUE otherwise so that the prepared statement remains both
        /// syntactically and semantically valid.</param>
        public TDbListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            OdbcParam = new OdbcParameter(name, type);
            SubValues = value;
        }

        IEnumerator <OdbcParameter>IEnumerable <OdbcParameter> .GetEnumerator()
        {
            UInt32 Counter = 0;

            foreach (Object value in SubValues)
            {
                OdbcParameter SubParameter = (OdbcParameter)((ICloneable)OdbcParam).Clone();
                SubParameter.Value = value;

                if (SubParameter.ParameterName != null)
                {
                    SubParameter.ParameterName += "_" + (Counter++);
                }

                yield return SubParameter;
            }
        }

        /// <summary>
        /// Get the generic IEnumerator over the sub-OdbcParameters.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable <OdbcParameter> ) this).GetEnumerator();
        }

        /// <summary>
        /// Represent this list of parameters as a string, using
        /// each value's <c>ToString()</c> method.
        /// </summary>
        public override String ToString()
        {
            return "[" + String.Join(",", SubValues.Cast <Object>()) + "]";
        }

        /// <summary>
        /// Convenience method for creating an OdbcParameter with an
        /// appropriate <c>TDbListParameterValue</c> as a value.
        /// </summary>
        public static OdbcParameter OdbcListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            return new OdbcParameter(name, type) {
                       Value = new TDbListParameterValue(name, type, value)
            };
        }
    }
}
