//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using Ict.Common.DB.DBCaching;
using Ict.Common.IO;

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
        /// specified <see cref="IsolationLevel" />  <em>exactly</em>.
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
        /// <summary>DebugLevel for logging the SQL code from DB queries</summary>
        public const Int32 DB_DEBUGLEVEL_QUERY = 3;

        /// <summary>DebugLevel for logging the SQL code from DB queries</summary>
        public const Int32 DB_DEBUGLEVEL_TRANSACTION = 10;

        /// <summary>DebugLevel for logging results from DB queries: is 6 (was 4 before)</summary>
        public const Int32 DB_DEBUGLEVEL_RESULT = 6;

        /// <summary>DebugLevel for tracing (most verbose log output): is 10 (was 4 before)</summary>
        public const Int32 DB_DEBUGLEVEL_TRACE = 10;

        /// <summary>
        /// store the current object for access to the database
        /// </summary>
        private static TDataBase MGDBAccessObj = null;

        /// <summary>
        /// delegate for setting the database object for this current session
        /// </summary>
        public delegate void DBAccessObjectSetter(TDataBase ADatabaseForUser);
        /// <summary>
        /// delegate for getting the database object for this current session
        /// </summary>
        public delegate TDataBase DBAccessObjectGetter();

        private static DBAccessObjectSetter MGDBAccessObjDelegateSet = null;
        private static DBAccessObjectGetter MGDBAccessObjDelegateGet = null;

        /// we cannot have a reference to System.Web for Session here, so we use a delegate
        public static void SetFunctionForRetrievingCurrentObjectFromWebSession(
            DBAccessObjectSetter setter,
            DBAccessObjectGetter getter)
        {
            MGDBAccessObjDelegateSet = setter;
            MGDBAccessObjDelegateGet = getter;
        }

        /// <summary>Global Object in which the Application can store a reference to an Instance of
        /// <see cref="TDataBase" /></summary>
        public static TDataBase GDBAccessObj
        {
            set
            {
                if (MGDBAccessObjDelegateSet == null)
                {
                    MGDBAccessObj = value;
                }
                else
                {
                    MGDBAccessObjDelegateSet(value);
                }
            }
            get
            {
                if (MGDBAccessObjDelegateGet == null)
                {
                    return MGDBAccessObj;
                }
                else
                {
                    return MGDBAccessObjDelegateGet();
                }
            }
        }
    }

    /// <summary>
    /// every database system that works for OpenPetra has to implement these functions
    /// </summary>
    public interface IDataBaseRDBMS
    {
        /// <summary>
        /// Create a connection, but not opening it yet
        /// </summary>
        /// <param name="AServer"></param>
        /// <param name="APort"></param>
        /// <param name="ADatabaseName"></param>
        /// <param name="AUsername"></param>
        /// <param name="APassword"></param>
        /// <param name="AConnectionString"></param>
        /// <param name="AStateChangeEventHandler"></param>
        /// <returns></returns>
        IDbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler);

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
        /// create a IDbCommand object
        /// this formats the sql query for the database, and transforms the parameters
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        IDbCommand NewCommand(ref string ACommandText, IDbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction);

        /// <summary>
        /// create an IDbDataAdapter
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter NewAdapter();

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName);

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataTable"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        void FillAdapter(IDbDataAdapter TheAdapter,
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
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection);

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection);

        /// <summary>
        /// restart a sequence with the given value
        /// </summary>
        void RestartSequence(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection, Int64 ARestartValue);

        /// update a database when starting the OpenPetra server. otherwise throw an exception
        void UpdateDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion,
            string AHostOrFile, string ADatabasePort, string ADatabaseName, string AUsername, string APassword);
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
    ///   returned when you call IDbDataAdapter.FillSchema. This is true even when
    ///   executing SQL batch statements from which multiple DataTable objects would
    ///   be expected! TODO: this comment needs revising, with native drivers
    /// </summary>
    public class TDataBase
    {
        /// <summary>References the DBConnection instance</summary>
        private TDBConnection FDBConnectionInstance;

        /// <summary>References an open DB connection</summary>
        private IDbConnection FSqlConnection;

        /// <summary>References the type of RDBMS that we are currently connected to</summary>
        private TDBType FDbType;

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

        /// <summary> this is a reference to the specific database functions which can be different for each RDBMS</summary>
        private IDataBaseRDBMS FDataBaseRDBMS;

        /// <summary>Tracks the last DB action; is updated with every creation of a Command.</summary>
        private DateTime FLastDBAction;

        /// <summary>References the current Transaction, if there is any.</summary>
        private IDbTransaction FTransaction;

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

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// The Database type will be specified only when one of the <c>EstablishDBConnection</c>
        /// Methods gets called
        /// </summary>
        public TDataBase() : base()
        {
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
                return ConnectionReady();
            }
        }

        /// <summary>Tells when the last Database action was carried out by the caller.</summary>
        public DateTime LastDBAction
        {
            get
            {
                return FLastDBAction;
            }
        }

        /// <summary>
        /// The current Transaction, if there is any.
        /// </summary>
        public TDBTransaction Transaction
        {
            get
            {
                if (FTransaction == null)
                {
                    return null;
                }
                else
                {
                    return new TDBTransaction(FTransaction, FSqlConnection);
                }
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
                return FUserID;
            }

            set
            {
                FUserID = value;
            }
        }

        #endregion

        private static bool FCheckedDatabaseVersion = false;

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
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown when a connection cannot be established</exception>
        public void EstablishDBConnection(TDBType ADataBaseType,
            String ADsnOrServer,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString)
        {
            FDbType = ADataBaseType;
            FDsnOrServer = ADsnOrServer;
            FDBPort = ADBPort;
            FDatabaseName = ADatabaseName;
            FUsername = AUsername;
            FPassword = APassword;
            FConnectionString = AConnectionString;

            if (FDbType == TDBType.PostgreSQL)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TPostgreSQL();
            }
            else if (FDbType == TDBType.MySQL)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TMySQL();
            }
            else if (FDbType == TDBType.SQLite)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TSQLite();
            }
            else if (FDbType == TDBType.ProgressODBC)
            {
                FDataBaseRDBMS = (IDataBaseRDBMS) new TProgressODBC();
            }

            if (ConnectionReady())
            {
                TLogging.Log("Error establishing connection to Database Server: connection is already open!");
                throw new EDBConnectionNotAvailableException(
                    FSqlConnection != null ? FSqlConnection.State.ToString("G") : "FSqlConnection is null");
            }

            TDBConnection CurrentConnectionInstance;

            if (FSqlConnection == null)
            {
                FDBConnectionInstance = TDBConnection.GetInstance();
                CurrentConnectionInstance = FDBConnectionInstance;

                FSqlConnection = CurrentConnectionInstance.GetConnection(
                    FDataBaseRDBMS,
                    ADsnOrServer,
                    ADBPort,
                    ADatabaseName,
                    AUsername,
                    ref APassword,
                    AConnectionString,
                    new StateChangeEventHandler(this.OnStateChangedHandler));

                if (FSqlConnection == null)
                {
                    throw new EDBConnectionNotEstablishedException();
                }
            }
            else
            {
                CurrentConnectionInstance = FDBConnectionInstance;
            }

            try
            {
                // always log to console and log file, which database we are connecting to.
                // see https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=156
                TLogging.Log("    Connecting to database " + ADataBaseType + ": " + CurrentConnectionInstance.GetConnectionString());

                FSqlConnection.Open();

                FLastDBAction = DateTime.Now;
            }
            catch (Exception exp)
            {
                FSqlConnection = null;

                LogException(exp,
                    String.Format("Exception occured while establishing a connection to Database Server. DB Type: {0}", FDbType));

                throw new EDBConnectionNotEstablishedException(CurrentConnectionInstance.GetConnectionString() + ' ' + exp.ToString());
            }

            // only check database version once when working with multiple connections
            if (!FCheckedDatabaseVersion)
            {
                CheckDatabaseVersion();
                FCheckedDatabaseVersion = true;
            }
        }

        /// <summary>
        /// Application and Database should have the same version, otherwise all sorts of things can go wrong.
        /// this is specific to the OpenPetra database, for all other databases it will just ignore the database version check
        /// </summary>
        private void CheckDatabaseVersion()
        {
            if (TAppSettingsManager.GetValue("action", string.Empty, false) == "patchDatabase")
            {
                // we want to upgrade the database, so don't check for the database version
                return;
            }

            string DBPatchVersion;
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            try
            {
                // now check if the database is uptodate; otherwise run db patch against it
                DBPatchVersion =
                    Convert.ToString(DBAccess.GDBAccessObj.ExecuteScalar(
                            "SELECT s_default_value_c FROM PUB_s_system_defaults WHERE s_default_code_c = 'CurrentDatabaseVersion'",
                            transaction));
            }
            catch (Exception)
            {
                // this can happen when connecting to an old Petra 2.x database, or a completely different database
                return;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            TFileVersionInfo dbversion = new TFileVersionInfo(DBPatchVersion);
            TFileVersionInfo serverExeInfo = new TFileVersionInfo(TFileVersionInfo.GetApplicationVersion());

            if (dbversion.CompareWithoutPrivatePart(serverExeInfo) < 0)
            {
                // for a proper server, the patchtool should have already updated the database

                // for standalone versions, we update the database on the fly when starting the server
                FDataBaseRDBMS.UpdateDatabase(dbversion, serverExeInfo, FDsnOrServer, FDBPort, FDatabaseName, FUsername, FPassword);
            }
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        public void CloseDBConnection()
        {
            if ((FSqlConnection != null) && (FSqlConnection.State != ConnectionState.Closed))
            {
                CloseDBConnectionInternal(FDbType);
            }
        }

        /// <summary>
        /// Closes the DB connection.
        /// </summary>
        /// <param name="ADbType">The Type of DB whose Connection should be closed</param>
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionNotEstablishedException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        private void CloseDBConnectionInternal(TDBType ADbType)
        {
            if (ConnectionReady())
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("  Closing Database connection...");
                }

                if (FTransaction != null)
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log("TDataBase.CloseDBConnectionInternal: before calling this.RollbackTransaction",
                            TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }

                    this.RollbackTransaction();

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log("TDataBase.CloseDBConnectionInternal: after calling this.RollbackTransaction",
                            TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: before calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain: "
                        +
                        AppDomain.CurrentDomain.ToString(),
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FDBConnectionInstance.CloseDBConnection(FSqlConnection);
                FSqlConnection = null;

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: closed DB Connection.");
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "TDataBase.CloseDBConnectionInternal: after calling FDBConnectionInstance.CloseODBCConnection(FConnection) in AppDomain: "
                        +
                        AppDomain.CurrentDomain.ToString(),
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FLastDBAction = DateTime.Now;
            }
            else
            {
                throw new EDBConnectionNotAvailableException();
            }
        }

        /// <summary>
        /// Call this Method to make the next Command that is sent to the DB
        /// a 'Prepared' command.
        /// </summary>
        /// <remarks><see cref="PrepareNextCommand" /> lets you optimise the performance of
        /// frequently used queries. What a RDBMS basically does with a 'Prepared' SQL Command is
        /// that it 'caches' the query plan so that it's used in subsequent calls.
        /// Not supported by all RDBMS, but should just silently fail in case a RDBMS doesn't
        /// support it. PostgreSQL definitely supports it.</remarks>
        /// <returns>void</returns>
        public void PrepareNextCommand()
        {
            FPrepareNextCommand = true;
        }

        /// <summary>
        /// Call this Method to set a timeout (in seconds) for the next Command that is sent to the
        /// DB that is different from the default timeout for a Command (eg. 20s for a
        /// NpgsqlCommand).
        /// </summary>
        /// <returns>void</returns>
        public void SetTimeoutForNextCommand(int ATimeoutInSec)
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
        /// Returns an IDbCommand for a given command text in the context of a
        /// DB transaction. Suitable for parameterised SQL statements.
        /// Allows the passing in of Parameters for the SQL statement
        /// </summary>
        /// <remarks>This function does not execute the Command, it just creates it!</remarks>
        /// <param name="ACommandText">Command Text</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />, or nil if the command
        /// should not be enlisted in a transaction.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameter
        /// (including Parameter Value)</param>
        /// <returns>Instantiated IDbCommand
        /// </returns>
        public IDbCommand Command(String ACommandText, TDBTransaction ATransaction, DbParameter[] AParametersArray)
        {
            IDbCommand ObjReturn = null;

            if (AParametersArray == null)
            {
                AParametersArray = new OdbcParameter[0];
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".Command()...");
            }

            if (!HasAccess(ACommandText))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            try
            {
                /* Preprocess ACommandText for `IN (?)' syntax */
                PreProcessCommand(ref ACommandText, ref AParametersArray);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".Command: now getting IDbCommand(" + ACommandText + ")...");
                }

                ObjReturn = FDataBaseRDBMS.NewCommand(ref ACommandText, FSqlConnection, AParametersArray, ATransaction);

                // enlist this command in a DB transaction (does not happen if ATransaction is null)
                if (ATransaction != null)
                {
                    ObjReturn.Transaction = ATransaction.WrappedTransaction;
                }

                // if this is a call to Stored Procedure: set command type accordingly
                if (ACommandText.ToUpper().StartsWith("CALL"))
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

                throw;
            }

            FLastDBAction = DateTime.Now;
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
            DataSet InputDataSet;
            DataSet ObjReturn;

            ObjReturn = null;
            InputDataSet = new DataSet();
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
            DataSet ObjReturn;

            if (AFillDataSet == null)
            {
                throw new ArgumentNullException("AFillDataSet", "AFillDataSet must not be null!");
            }

            if (ADataTableName == "")
            {
                throw new ArgumentException("ADataTableName", "A name for the DataTable must be submitted!");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".Select()", ASqlStatement, AParametersArray);
            }

            ObjReturn = null;

            try
            {
                IDbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, AParametersArray);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: now filling IDbDataAdapter('" + ADataTableName) + "')..."));
                }

                FDataBaseRDBMS.FillAdapter(TheAdapter, ref AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: finished filling IDbDataAdapter(DataTable '" +
                                   ADataTableName) + "'). DT Row Count: " + AFillDataSet.Tables[ADataTableName].Rows.Count.ToString()));
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
            DataSet ObjReturn;

            if (AFillDataSet == null)
            {
                throw new ArgumentNullException("AFillDataSet", "AFillDataSet must not be null!");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".Select()", ASqlStatement, AParametersArray);
            }

            ObjReturn = null;

            try
            {
                IDbDataAdapter TheAdapter = SelectDA(ASqlStatement, AReadTransaction, AParametersArray);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: now filling IDbDataAdapter('" + ADataTempTableName) + "')..."));
                }

                //Make sure that any previous temp table of the same name is removed first!
                if (AFillDataSet.Tables.Contains(ADataTempTableName))
                {
                    AFillDataSet.Tables.Remove(ADataTempTableName);
                }

                AFillDataSet.Tables.Add(ADataTempTableName);

                FDataBaseRDBMS.FillAdapter(TheAdapter, ref AFillDataSet, AStartRecord, AMaxRecords, ADataTempTableName);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: finished filling IDbDataAdapter(DataTable '" +
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
        /// Returns an <see cref="IDbDataAdapter" /> (eg. <see cref="OdbcDataAdapter" />, NpgsqlDataAdapter) for a given SQL statement.
        /// The SQL statement is executed in the given transaction context (which should
        /// have the desired <see cref="IsolationLevel" />). Suitable for parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel" /></param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Instantiated <see cref="IDbDataAdapter" />
        /// </returns>
        public IDbDataAdapter SelectDA(String ASqlStatement, TDBTransaction AReadTransaction, DbParameter[] AParametersArray = null)
        {
            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Entering " + this.GetType().FullName + ".SelectDA()...");
            }

            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log(this.GetType().FullName + ".SelectDA: now opening IDbDataAdapter(" + ASqlStatement + ")...");
            }

            if (AParametersArray == null)
            {
                AParametersArray = new OdbcParameter[0];
            }

            IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
            TheAdapter.SelectCommand = FDataBaseRDBMS.NewCommand(ref ASqlStatement, FSqlConnection, AParametersArray, AReadTransaction);
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
            return SelectDTInternal(ASqlStatement, ADataTableName, AReadTransaction, new OdbcParameter[0]);
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
                AReadTransaction, AParametersArray);
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
        /// <returns>Instantiated <see cref="DataTable" /></returns>
        private DataTable SelectDTInternal(String ASqlStatement,
            String ADataTableName,
            TDBTransaction AReadTransaction,
            DbParameter[] AParametersArray)
        {
            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".SelectDTInternal()", ASqlStatement, AParametersArray);
            }

            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            DataTable ObjReturn = new DataTable(ADataTableName);
            try
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".SelectDTInternal: now opening IDbDataAdapter(" + ASqlStatement + ")...");
                }

                IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
                TheAdapter.SelectCommand = Command(ASqlStatement, AReadTransaction, AParametersArray);
                FDataBaseRDBMS.FillAdapter(TheAdapter, ref ObjReturn, 0, 0);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".SelectDTInternal: finished filling IDbDataAdapter(DataTable " +
                                   ADataTableName) + "). DT Row Count: " + ObjReturn.Rows.Count.ToString()));
                }
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
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
            DbParameter[] AParametersArray,
            int AStartRecord, int AMaxRecords)
        {
            if (!HasAccess(ASqlStatement))
            {
                throw new Exception("Security Violation: Access Permission failed");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".SelectDT()", ASqlStatement, AParametersArray);
            }

            try
            {
                IDbDataAdapter TheAdapter = FDataBaseRDBMS.NewAdapter();
                TheAdapter.SelectCommand = Command(ASqlStatement, AReadTransaction, AParametersArray);
                FDataBaseRDBMS.FillAdapter(TheAdapter, ref ATypedDataTable, AStartRecord, AMaxRecords);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(((this.GetType().FullName + ".Select: finished filling IDbDataAdapter(DataTable '" +
                                   ATypedDataTable.TableName) + "'). DT Row Count: " + ATypedDataTable.Rows.Count.ToString()));
#if WITH_POSTGRESQL_LOGGING
                    NpgsqlEventLog.Level = LogLevel.None;
#endif
                }
            }
            catch (Exception exp)
            {
                LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error fetching records.");
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


        #region Transactions

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified.
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <returns>Started Transaction (null if an error occured)
        /// </returns>
        public TDBTransaction BeginTransaction(Int16 ARetryAfterXSecWhenUnsuccessful)
        {
            TDBTransaction ReturnValue;

            try
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "Trying to open a DB Transaction... (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                }

                FTransaction = FSqlConnection.BeginTransaction();

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    TLogging.Log("DB Transaction started (in Appdomain " + AppDomain.CurrentDomain.ToString() + " ).");
                }
            }
            catch (System.InvalidOperationException exp)
            {
                // System.InvalidOperationException is thrown when a transaction is currently active. Parallel transactions are not supported!
                // Retry again if programmer wants that
                if (ARetryAfterXSecWhenUnsuccessful != -1)
                {
                    Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                    /*
                     * Retry again to begin a transaction.
                     * Note: if this fails again, an Exception is thrown as if there was
                     * no ARetryAfterXSecWhenUnsuccessful specfied!
                     */
                    ReturnValue = BeginTransaction(-1);
                    return ReturnValue;
                }
                else
                {
                    throw new EDBTransactionBusyException("", exp);
                }
            }
            catch (Exception exp)
            {
                if ((FSqlConnection.State == ConnectionState.Broken) || (FSqlConnection.State == ConnectionState.Closed))
                {
                    TLogging.Log(exp.Message);
                    TLogging.Log("Connection State: " + FSqlConnection.State.ToString("G"));

                    if (FSqlConnection.State == ConnectionState.Broken)
                    {
                        FSqlConnection.Close();
                    }

                    FSqlConnection = null;
                    EstablishDBConnection(FDbType, FDsnOrServer, FDBPort, FDatabaseName, FUsername, FPassword, FConnectionString);
                    return BeginTransaction(ARetryAfterXSecWhenUnsuccessful);
                }

                LogExceptionAndThrow(exp, "Error creating Transaction - Server-side error.");
            }

            FLastDBAction = DateTime.Now;
            return new TDBTransaction(FTransaction, FSqlConnection);
        }

        /// <summary>
        /// Starts a Transaction on the current DB connection.
        /// </summary>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction()
        {
            return BeginTransaction(-1);
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection. Allows a retry timeout to be specified.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /></param>
        /// <param name="ARetryAfterXSecWhenUnsuccessful">Allows a retry timeout to be specified.
        /// This is to be able to mitigate the problem of wanting to start a DB
        /// Transaction while another one is still running (gives time for the
        /// currently running DB Transaction to be finished).</param>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel, Int16 ARetryAfterXSecWhenUnsuccessful)
        {
            TDBTransaction ReturnValue;

            if (FDataBaseRDBMS == null)
            {
                throw new Exception("DBAccess BeginTransaction: FDataBaseRDBMS is null");
            }

            FDataBaseRDBMS.AdjustIsolationLevel(ref AIsolationLevel);

            try
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(
                        "Trying to open an DB Transaction with IsolationLevel '" + AIsolationLevel.ToString() +
                        "... (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                }

                FTransaction = FSqlConnection.BeginTransaction(AIsolationLevel);

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    TLogging.Log(
                        "DB Transaction with IsolationLevel '" + AIsolationLevel.ToString() + "' started (in Appdomain " +
                        AppDomain.CurrentDomain.ToString() + " ).");
                    TLogging.Log("Start of stack trace.->");
                    TLogging.LogStackTrace(TLoggingType.ToLogfile);
                    TLogging.Log("<- End of stack trace");
                }
            }
            catch (System.InvalidOperationException exp)
            {
                // System.InvalidOperationException is thrown when a transaction is currently active. Parallel transactions are not supported!
                // Retry again if programmer wants that
                if (ARetryAfterXSecWhenUnsuccessful != -1)
                {
                    Thread.Sleep(ARetryAfterXSecWhenUnsuccessful * 1000);

                    /*
                     * Retry again to begin a transaction.
                     * Note: if this fails again, an Exception is thrown as if there was
                     * no ARetryAfterXSecWhenUnsuccessful specfied!
                     */
                    ReturnValue = BeginTransaction(AIsolationLevel, -1);
                    return ReturnValue;
                }
                else
                {
                    throw new EDBTransactionBusyException("IsolationLevel: " + Enum.GetName(typeof(IsolationLevel), AIsolationLevel), exp);
                }
            }
            catch (Exception exp)
            {
                if ((FSqlConnection == null) || (FSqlConnection.State == ConnectionState.Broken) || (FSqlConnection.State == ConnectionState.Closed))
                {
                    // reconnect to the database
                    TLogging.Log(exp.Message);

                    if (FSqlConnection == null)
                    {
                        TLogging.Log("FSqlConnection is null");
                    }
                    else
                    {
                        TLogging.Log("Connection State: " + FSqlConnection.State.ToString("G"));

                        if (FSqlConnection.State == ConnectionState.Broken)
                        {
                            FSqlConnection.Close();
                        }

                        FSqlConnection = null;
                    }

                    try
                    {
                        EstablishDBConnection(FDbType, FDsnOrServer, FDBPort, FDatabaseName, FUsername, FPassword, FConnectionString);
                    }
                    catch (Exception e2)
                    {
                        TLogging.Log("Another Exception while trying to establish the connection: " + e2.Message);
                        throw;
                    }

                    return BeginTransaction(AIsolationLevel, ARetryAfterXSecWhenUnsuccessful);
                }

                LogExceptionAndThrow(exp, "Error creating Transaction - Server-side error.");
            }

            FLastDBAction = DateTime.Now;
            return new TDBTransaction(FTransaction, FSqlConnection);
        }

        /// <summary>
        /// Starts a Transaction with a defined <see cref="IsolationLevel" /> on the current DB
        /// connection.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /></param>
        /// <returns>Started Transaction (null if an error occured)</returns>
        public TDBTransaction BeginTransaction(IsolationLevel AIsolationLevel)
        {
            return BeginTransaction(AIsolationLevel, -1);
        }

        /// <summary>
        /// Commits a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void CommitTransaction()
        {
            if (FTransaction != null)
            {
                String msg = "";

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    msg = "DB Transaction with IsolationLevel '" + FTransaction.IsolationLevel.ToString() + "' committed (in Appdomain " +
                          AppDomain.CurrentDomain.ToString() + " ).";
                }

                FTransaction.Commit();

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    TLogging.Log(msg);
                }
            }

            FTransaction = null;
        }

        /// <summary>
        /// Rolls back a running Transaction on the current DB connection.
        /// </summary>
        /// <returns>void</returns>
        public void RollbackTransaction()
        {
            String msg = "";

            if (FTransaction == null)
            {
                return;
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
            {
                msg = "DB Transaction with IsolationLevel '" + FTransaction.IsolationLevel.ToString() + "' rolled back (in Appdomain " +
                      AppDomain.CurrentDomain.ToString() + " ).";
            }

            FTransaction.Rollback();

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
            {
                TLogging.Log(msg);
            }

            FTransaction = null;
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
        /// <returns>Either an existing or a new Transaction that exactly meets the specified <see cref="IsolationLevel" /></returns>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel, out Boolean ANewTransaction)
        {
            return GetNewOrExistingTransaction(ADesiredIsolationLevel, TEnforceIsolationLevel.eilExact, out ANewTransaction);
        }

        /// <summary>
        /// for debugging purposes, get the isolation level of the current transaction
        /// </summary>
        /// <returns>Isolation.Undefined if no transaction is open</returns>
        public IsolationLevel GetIsolationLevel()
        {
            if (this.Transaction != null)
            {
                return this.Transaction.IsolationLevel;
            }

            return IsolationLevel.Unspecified;
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
            out Boolean ANewTransaction)
        {
            TDBTransaction TheTransaction;

            ANewTransaction = false;
            TheTransaction = this.Transaction;

            FDataBaseRDBMS.AdjustIsolationLevel(ref ADesiredIsolationLevel);

            if (TheTransaction != null)
            {
                // Check if the IsolationLevel of the existing Transaction is acceptable
                if ((ATryToEnforceIsolationLevel == TEnforceIsolationLevel.eilExact)
                    && (TheTransaction.IsolationLevel != ADesiredIsolationLevel)
                    || ((ATryToEnforceIsolationLevel == TEnforceIsolationLevel.eilMinimum)
                        && (TheTransaction.IsolationLevel < ADesiredIsolationLevel)))
                {
                    switch (ATryToEnforceIsolationLevel)
                    {
                        case TEnforceIsolationLevel.eilExact:
                            throw new EDBTransactionIsolationLevelWrongException("Expected IsolationLevel: " +
                            ADesiredIsolationLevel.ToString("G") + " but is: " + TheTransaction.IsolationLevel.ToString("G"));

                        case TEnforceIsolationLevel.eilMinimum:
                            throw new EDBTransactionIsolationLevelTooLowException(
                            "Expected IsolationLevel: at least " + ADesiredIsolationLevel.ToString("G") +
                            " but is: " + TheTransaction.IsolationLevel.ToString("G"));
                    }
                }
            }

            if (TheTransaction == null)
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    Console.WriteLine("GetNewOrExistingTransaction: creating new transaction. IsolationLevel: " + ADesiredIsolationLevel.ToString());
                }

                TheTransaction = BeginTransaction(ADesiredIsolationLevel);
                ANewTransaction = true;
            }
            else
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    Console.WriteLine(
                        "GetNewOrExistingTransaction: using existing transaction. IsolationLevel: " + TheTransaction.IsolationLevel.ToString());
                }
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
            return FDataBaseRDBMS.GetNextSequenceValue(ASequenceName, ATransaction, this, FSqlConnection);
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
            return FDataBaseRDBMS.GetCurrentSequenceValue(ASequenceName, ATransaction, this, FSqlConnection);
        }

        /// <summary>
        /// restart a sequence with the given value
        /// </summary>
        public void RestartSequence(String ASequenceName, TDBTransaction ATransaction, Int64 ARestartValue)
        {
            FDataBaseRDBMS.RestartSequence(ASequenceName, ATransaction, this, FSqlConnection, ARestartValue);
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes a SQL statement that does not give back any results (eg. an UPDATE
        /// SQL command). The statement is executed in a transaction. Suitable for
        /// parameterised SQL statements.
        ///
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)
        /// </param>
        /// <returns>Number of Rows affected</returns>
        public int ExecuteNonQuery(String ASqlStatement,
            TDBTransaction ATransaction,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            IDbCommand TransactionCommand = null;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".ExecuteNonQuery()", ASqlStatement, AParametersArray);
            }

            if (ConnectionReady())
            {
                TransactionCommand = Command(ASqlStatement, ATransaction, AParametersArray);

                if (TransactionCommand == null)
                {
                    // should never get here
                    return 0;
                }

                try
                {
                    int NumberOfRowsAffected = TransactionCommand.ExecuteNonQuery();

                    TransactionCommand.Dispose();

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log("Number of rows affected: " + NumberOfRowsAffected.ToString());
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
                    }

                    return NumberOfRowsAffected;
                }
                catch (Exception exp)
                {
                    LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error executing non-query SQL statement.");
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }

            return 0;
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. The transaction's <see cref="IsolationLevel" />
        /// will be <see cref="IsolationLevel.ReadCommitted" />.
        /// Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object
        /// </param>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable)
        {
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(IsolationLevel.ReadCommitted);
                ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction, true);
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
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
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />  of the transaction
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, IsolationLevel AIsolationLevel)
        {
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(AIsolationLevel);
                ExecuteNonQueryBatch(AStatementHashTable, EnclosingTransaction, true);
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }
        }

        /// <summary>
        /// Executes 1..n SQL statements in a batch (in one go). The statements are
        /// executed in a transaction - if one statement results in an Exception, all
        /// statements executed so far are rolled back. Suitable for parameterised SQL
        /// statements.
        ///
        /// </summary>
        /// <param name="AStatementHashTable">A HashTable. Key: a unique identifier;
        /// Value: an instantiated <see cref="TSQLBatchStatementEntry" /> object</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">On successful execution of all statements the
        /// transaction is committed if set to true, otherwise the transaction is not
        /// committed (useful when the caller wants to do further things in the same
        /// transaction).
        /// </param>
        /// <returns>void</returns>
        public void ExecuteNonQueryBatch(Hashtable AStatementHashTable, TDBTransaction ATransaction, bool ACommitTransaction = false)
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
                throw new ArgumentException("AStatementHashTable", "ArrayList containing TSQLBatchStatementEntry objects must not be empty!");
            }

            if (ATransaction == null)
            {
                throw new ArgumentNullException("ATransaction", "This method must be called with an initialized transaction!");
            }

            if (ConnectionReady())
            {
                // TransactionCommand := nil;
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
                            BatchStatementEntryValue.Parameters);
                        SqlCommandNumber = SqlCommandNumber + 1;
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
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
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
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
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" /> of the transaction</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Single result as object
        /// </returns>
        public object ExecuteScalar(String ASqlStatement, IsolationLevel AIsolationLevel, DbParameter[] AParametersArray = null)
        {
            object ReturnValue = null;
            TDBTransaction EnclosingTransaction;

            if (ConnectionReady())
            {
                EnclosingTransaction = BeginTransaction(AIsolationLevel);

                try
                {
                    ReturnValue = ExecuteScalar(ASqlStatement, EnclosingTransaction, AParametersArray);
                }
                catch (Exception)
                {
                    // Exception logging occurs already  inside ExecuteScalar, so we don't need to do it here!
                    throw;
                }
                finally
                {
                    CommitTransaction();
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" /></param>
        /// <param name="ACommitTransaction">The transaction is committed if set to true,
        /// otherwise the transaction is not committed (useful when the caller wants to
        /// do further things in the same transaction).</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value)</param>
        /// <returns>Single result as TObject
        /// </returns>
        public object ExecuteScalar(String ASqlStatement,
            TDBTransaction ATransaction = null,
            DbParameter[] AParametersArray = null,
            bool ACommitTransaction = false)
        {
            object ReturnValue = null;
            IDbCommand TransactionCommand = null;

            if ((ATransaction == null) && (ACommitTransaction == true))
            {
                throw new ArgumentNullException("ACommitTransaction", "ACommitTransaction cannot be set to true when ATransaction is null!");
            }

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                LogSqlStatement(this.GetType().FullName + ".ExecuteScalar()", ASqlStatement, AParametersArray);
            }

            if (ConnectionReady())
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now opening Command(" + ASqlStatement + ")...");
                }

                TransactionCommand = Command(ASqlStatement, ATransaction, AParametersArray);

                if (TransactionCommand == null)
                {
                    // should never get here
                    return null;
                }

                try
                {
                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".ExecuteScalar: now calling Command.ExecuteScalar...");
                    }

                    ReturnValue = TransactionCommand.ExecuteScalar();
                    TransactionCommand.Dispose();

                    if (ReturnValue == null)
                    {
                        throw new Exception("Execute Scalar returned no value");
                    }

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log(this.GetType().FullName + ".ExecuteScalar: finished calling Command.ExecuteScalar");
                    }

                    if (ACommitTransaction)
                    {
                        CommitTransaction();
                    }
                }
                catch (Exception exp)
                {
                    LogExceptionAndThrow(exp, ASqlStatement, AParametersArray, "Error executing scalar SQL statement.");
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_RESULT)
                {
                    TLogging.Log("Result from ExecuteScalar is " + ReturnValue.ToString() + " " + ReturnValue.GetType().ToString());
                }
            }
            else
            {
                throw new EDBConnectionNotAvailableException(FSqlConnection.State.ToString("G"));
            }

            return ReturnValue;
        }

        #endregion

        /// <summary>
        /// read an sql statement from file and remove the comments
        /// </summary>
        public static string ReadSqlFile(string ASqlFilename)
        {
            return ReadSqlFile(ASqlFilename, null);
        }

        /// <summary>
        /// read an sql statement from file and remove the comments
        /// </summary>
        /// <param name="ASqlFilename"></param>
        /// <param name="ADefines">Defines to be set in the sql statement</param>
        /// <returns></returns>
        public static string ReadSqlFile(string ASqlFilename, SortedList <string, string>ADefines)
        {
            ASqlFilename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                           Path.DirectorySeparatorChar +
                           ASqlFilename;

            // Console.WriteLine("reading " + ASqlFilename);
            StreamReader reader = new StreamReader(ASqlFilename);
            string line = null;
            string stmt = "";

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
        ///   Expand IList items in a parameter list so that `IN (?)' syntax works.
        /// </summary>
        static private void PreProcessCommand(ref String ACommandText, ref DbParameter[] AParametersArray)
        {
            /* Check if there are any parameters which need `IN (?)' expansion. */
            Boolean INExpansionNeeded = false;

            if (AParametersArray != null)
            {
                foreach (OdbcParameter param in AParametersArray)
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

                IEnumerator <OdbcParameter>ParametersEnumerator = ((IEnumerable <OdbcParameter> )AParametersArray).GetEnumerator();

                foreach (String SqlPart in ACommandText.Split(new Char[] { '?' }))
                {
                    NewCommandText += SqlPart;

                    if (!ParametersEnumerator.MoveNext())
                    {
                        /* We're at the end of the string/parameter array */
                        continue;
                    }

                    OdbcParameter param = ParametersEnumerator.Current;

                    if (param.Value is TDbListParameterValue)
                    {
                        Boolean first = true;

                        foreach (OdbcParameter subparam in (TDbListParameterValue)param.Value)
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

                ACommandText = NewCommandText;
                AParametersArray = NewParametersArray.ToArray();

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("PreProcessCommand(): Performed `column IN (?)' expansion, result follows:");
                    LogSqlStatement("PreProcessCommand()", ACommandText, AParametersArray);
                }
            }
        }

        private bool FConnectionReady = false;

        /// <summary>
        /// Tells whether the DB connection is ready to accept commands
        /// or whether it is busy.
        /// </summary>
        /// <returns>True if DB connection can accept commands, false if
        /// it is busy</returns>
        private bool ConnectionReady()
        {
            if (FDbType == TDBType.PostgreSQL)
            {
                // TODO: change when OnStateChangedHandler works for postgresql
                return FSqlConnection != null && FSqlConnection.State == ConnectionState.Open;
            }

            return FConnectionReady;
        }

        /// <summary>
        /// Updates the FConnectionReady variable with the current ConnectionState.
        /// </summary>
        /// <remarks>
        /// <em>WARNING:</em> This doesn't work with NpgsqlConnection because it never raises the
        /// Event. Therefore the FConnectionReady variable must
        /// never be inquired directly, but only through calling ConnectionReady()!
        /// TODO: revise this comment with more recent Npgsql release
        /// </remarks>
        /// <param name="ASender">Sending object</param>
        /// <param name="AArgs">StateChange EventArgs</param>
        private void OnStateChangedHandler(object ASender, StateChangeEventArgs AArgs)
        {
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
        /// <returns></returns>
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
        /// Logs the contents of a DataTable
        /// </summary>
        /// <param name="tab">The DataTable whose contents should be logged
        /// </param>
        /// <returns>void</returns>
        public static void LogTable(DataTable tab)
        {
            String line;

            line = "";

            foreach (DataColumn column in tab.Columns)
            {
                line = line + ' ' + column.ColumnName;
            }

            TLogging.Log(line);

            int MaxRows = 10;

            foreach (DataRow row in tab.Rows)
            {
                line = "";

                foreach (DataColumn column in tab.Columns)
                {
                    line = line + ' ' + row[column].ToString();
                }

                if ((MaxRows > 0) || (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE))
                {
                    MaxRows--;
                    TLogging.Log(line);
                }
                else
                {
                    break;
                }
            }

            if (MaxRows == 0)
            {
                TLogging.Log("more rows have been skipped...");
            }
        }

        /// <summary>
        /// For debugging purposes.
        /// Formats the sql query so that it is easily readable
        /// (mainly inserting line breaks before AND)
        ///
        /// </summary>
        /// <param name="s">the sql statement that should be formatted</param>
        /// <returns>s the formatted sql statement
        /// </returns>
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
                PrintContext = "(Context '" + AContext + "')" + Environment.NewLine;
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
                    else
                    {
                        TLogging.Log(
                            "Parameter: " + Counter.ToString() + ' ' + Parameter.Value.ToString() + ' ' + Parameter.Value.GetType().ToString() +
                            ' ' +
                            Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString());
                    }

                    Counter = Counter + 1;
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
        /// <param name="AParametersArray">the parameters for the query</param>
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
        /// <param name="AParametersArray">the parameters for the query</param>
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
        /// <param name="AParametersArray">the parameters for the query</param>
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

            if (ASqlStatement != String.Empty)
            {
                ASqlStatement = FDataBaseRDBMS.FormatQueryRDBMSSpecific(ASqlStatement);

                FormattedSqlStatement = "The SQL Statement was: " + Environment.NewLine +
                                        ASqlStatement + Environment.NewLine;

                if (AParametersArray != null)
                {
                    Int32 Counter = 1;

                    foreach (OdbcParameter Parameter in AParametersArray)
                    {
                        if (Parameter.Value == System.DBNull.Value)
                        {
                            FormattedSqlStatement +=
                                "Parameter: " + Counter.ToString() + " DBNull" + ' ' + Parameter.Value.GetType().ToString() + ' ' +
                                Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) +
                                Environment.NewLine;
                        }
                        else
                        {
                            FormattedSqlStatement +=
                                "Parameter: " + Counter.ToString() + ' ' + Parameter.Value.ToString() + ' ' +
                                Parameter.Value.GetType().ToString() +
                                ' ' +
                                Enum.GetName(typeof(System.Data.Odbc.OdbcType), Parameter.OdbcType) + ' ' + Parameter.Size.ToString() +
                                Environment.NewLine;
                        }

                        Counter = Counter + 1;
                    }
                }
            }

            FDataBaseRDBMS.LogException(AException, ref ErrorMessage);

            TLogging.Log(AContext + Environment.NewLine +
                FormattedSqlStatement +
                "Possible cause: " + AException.ToString() + Environment.NewLine + ErrorMessage);

            TLogging.LogStackTrace(TLoggingType.ToLogfile);

            if (AThrowExceptionAfterLogging)
            {
                throw AException;
            }
        }
    }

    #region TSQLBatchStatementEntry

    /// <summary>
    /// Represents the Value of an entry in a HashTable for use in calls to one of the
    /// <c>ExecuteNonQueryBatch</c> Methods.
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
        /// SQL Statement for one Batch Entry
        /// </summary>
        public String SQLStatement
        {
            get
            {
                return FSQLStatement;
            }
        }

        /// <summary>
        /// Parameters for a Batch Entry (optional)
        /// </summary>
        public DbParameter[] Parameters
        {
            get
            {
                return FParametersArray;
            }
        }


        /// <summary>
        /// Initialises the internal variables that hold the Batch Statment Entry
        /// parameters.
        /// </summary>
        /// <param name="ASQLStatement">SQL Statement for one Batch Entry</param>
        /// <param name="AParametersArray">Parameters for the SQL Statement (can be null)</param>
        /// <returns>void</returns>
        public TSQLBatchStatementEntry(String ASQLStatement, DbParameter[] AParametersArray)
        {
            FSQLStatement = ASQLStatement;
            FParametersArray = AParametersArray;
        }

        #endregion
    }


    /// <summary>
    /// A generic Class for managing all kinds of ADO.NET Database Transactions -
    /// to be used instead of concrete ADO.NET Transaction objects, eg. <see cref="OdbcTransaction" />
    /// or NpgsqlTransaction.
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
    public class TDBTransaction : object
    {
        /// <summary>Holds the <see cref="IsolationLevel" /> of the Transaction</summary>
        private System.Data.IsolationLevel FIsolationLevel;

        /// <summary>Holds the Database connection to which the Transaction belongs.</summary>
        private IDbConnection FConnection;

        /// <summary>Holds the actual IDbTransaction.</summary>
        private IDbTransaction FWrappedTransaction;

        /// <summary>
        /// Database connection to which the Transaction belongs
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return FConnection;
            }
        }

        /// <summary>
        /// <see cref="IsolationLevel" /> of the Transaction
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                return FIsolationLevel;
            }
        }

        /// <summary>
        /// The actual IDbTransaction.
        /// <para><em><b>WARNING:</b> do not do anything
        /// with this Object other than inspecting it; the correct
        /// working of Transactions in the <see cref="TDataBase" />
        /// Object relies on the fact that it manages everything about
        /// a Transaction!!!</em>
        /// </para>
        /// </summary>
        public IDbTransaction WrappedTransaction
        {
            get
            {
                return FWrappedTransaction;
            }
        }

        /// <summary>
        /// Constructor for a <see cref="TDBTransaction" /> Object.
        /// </summary>
        /// <param name="ATransaction">The concrete IDbTransaction Object that <see cref="TDBTransaction" /> should represent</param>
        /// <param name="AConnection"></param>
        public TDBTransaction(IDbTransaction ATransaction, IDbConnection AConnection)
        {
            FWrappedTransaction = ATransaction;

            // somehow, this line does not work for Progress, gives segmentation fault
            //FConnection = ATransaction.Connection;
            FConnection = AConnection;
            FIsolationLevel = ATransaction.IsolationLevel;
        }
    }

    /// <summary>
    ///   A list of parameters which should be expanded into an `IN (?)'
    ///   context.
    /// </summary>
    /// <example>
    ///   Simply use the following style in your .sql file:
    ///   <code>
    ///     SELECT * FROM table WHERE column IN (?)
    ///   </code>
    ///
    ///     Then, to test if <c>column</c> is the string <c>"First"</c>,
    ///     <c>"Second"</c>, or <c>"Third"</c>, set the <c>OdbcParameter.Value</c>
    ///     property to a <c>TDbListParameterValue</c> instance. You
    ///     can use the
    ///     <c>TDbListParameterValue.OdbcListParameterValue()</c>
    ///     function to produce an <c>OdbcParameter</c> with an
    ///     appropriate <c>Value</c> property.
    ///   <code>
    ///     OdbcParameter[] parameters = new OdbcParamter[]
    ///     {
    ///         TDbListParameterValue(param_grdCommitmentStatusChoices", OdbcType.NChar,
    ///             new String[] { "First", "Second", "Third" }),
    ///     };
    ///   </code>
    /// </example>
    public class TDbListParameterValue : IEnumerable <OdbcParameter>
    {
        private IEnumerable SubValues;

        /// <summary>
        ///   The OdbcParameter from which sub-parameters are Clone()d.
        /// </summary>
        public OdbcParameter OdbcParam;

        /// <summary>
        ///   Create a list parameter, such as is used for `column IN (?)' in
        ///   SQL queries, from any IEnumerable object.
        /// </summary>
        /// <param name="name">The ParameterName to use when creating OdbcParameters</param>
        /// <param name="type">The OdbcType of the produced OdbcParameters</param>
        /// <param name="value">An enumerable collection of objects.
        ///   If there are no objects in the enumeration, then the resulting
        ///   query will look like <c>column IN (NULL)</c> because
        ///   <c>column IN ()</c> is invalid. To avoid the case where
        ///   the query should not match any rows and <c>column</c>
        ///   may be NULL, use an expression like <c>(? AND column IN (?)</c>.
        ///   Set the first parameter to FALSE if the list is empty and
        ///   TRUE otherwise so that the prepared statement remains both
        ///   syntactically and semantically valid.</param>
        public TDbListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            OdbcParam = new OdbcParameter(name, type);
            SubValues = value;
        }

        IEnumerator <OdbcParameter>IEnumerable <OdbcParameter> .GetEnumerator()
        {
            UInt32 i = 0;

            foreach (Object value in SubValues)
            {
                OdbcParameter SubParameter = (OdbcParameter)((ICloneable)OdbcParam).Clone();
                SubParameter.Value = value;

                if (SubParameter.ParameterName != null)
                {
                    SubParameter.ParameterName += "_" + (i++);
                }

                yield return SubParameter;
            }
        }

        /// <summary>
        ///   Get the generic IEnumerator over the sub OdbcParameters.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable <OdbcParameter> ) this).GetEnumerator();
        }

        /// <summary>
        ///   Represent this list of parameters as a string, using
        ///   each value's <c>ToString()</c> method.
        /// </summary>
        public override String ToString()
        {
            return "[" + String.Join(",", SubValues.Cast <Object>()) + "]";
        }

        /// <summary>
        ///   Convenience method for creating an OdbcParameter with an
        ///   appropriate <c>TDbListParameterValue</c> as a value.
        /// </summary>
        public static OdbcParameter OdbcListParameterValue(String name, OdbcType type, IEnumerable value)
        {
            return new OdbcParameter(name, type)
                   {
                       Value = new TDbListParameterValue(name, type, value)
                   };
        }
    }
}