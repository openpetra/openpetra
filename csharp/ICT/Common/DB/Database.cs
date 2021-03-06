// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2021 by OM International
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
using System.Data.Common;
using System.Data.Odbc;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using Npgsql;
using MySqlConnector;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB.DBCaching;
using Ict.Common.Exceptions;
using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB
{
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
        /// this is for detecting open database connections that should have been closed.
        private static int SNumberConnections = 0; // STATIC_OK: Global

        private const string StrNestedTransactionProblem = "Nested DB Transaction problem details:  *Previously* started " +
                                                           "DB Transaction Properties: Valid: {0}, IsolationLevel: {1}; it got started on Thread {2} in AppDomain '{3}'.  "
                                                           +
                                                           "The attempt to begin a DB Transaction NOW occured on Thread {4} in AppDomain '{5}.'   " +
                                                           "The StackTrace of the *previously* started DB Transaction is as follows:\r\n  PREVIOUS Stracktrace: {6}\r\n  CURRENT Stracktrace: {7}";

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

        /// return the filename or server name of the database
        public String DsnOrServer
        {
            get
            {
                return FDsnOrServer;
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

        /// if we commit or rollback a transaction, make sure we drop the default transaction of the connection as well
        public bool ClearTransaction(TDBTransaction ToClear)
        {
            WaitForCoordinatedDBAccess();

            try
            {
                if (FTransaction != null && ToClear.TransactionIdentifier == FTransaction.TransactionIdentifier)
                {
                    FTransaction = new TDBTransaction();
                    return true;
                }
            }
            finally
            {
                ReleaseCoordinatedDBAccess();
            }

            return false;
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

        #endregion

        /// simple overload for establishing a database connection using the settings from the configuration file
        public void EstablishDBConnection(String AConnectionName = "")
        {
            TDBType DBType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType", "postgresql"));
            string DatabaseHostOrFile = TAppSettingsManager.GetValue("Server.DBHostOrFile", "localhost");
            string DatabasePort = TAppSettingsManager.GetValue("Server.DBPort", "5432");
            string DatabaseName = TAppSettingsManager.GetValue("Server.DBName", "openpetra");
            string DBUsername = TAppSettingsManager.GetValue("Server.DBUserName", "petraserver");
            string DBPassword = TAppSettingsManager.GetValue("Server.DBPassword", string.Empty, false);

            if (DBPassword == "PG_OPENPETRA_DBPWD")
            {
                // get the password from the file ~/.pgpass. This currently only works for PostgreSQL on Linux
                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                           Path.DirectorySeparatorChar + ".pgpass"))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.StartsWith(DatabaseHostOrFile + ":" + DatabasePort + ":" + DatabaseName + ":" + DBUsername + ":")
                            || line.StartsWith("*:" + DatabasePort + ":" + DatabaseName + ":" + DBUsername + ":"))
                        {
                            DBPassword = line.Substring(line.LastIndexOf(':') + 1);
                            break;
                        }
                    }
                }
            }
            
            EstablishDBConnection(DBType,
                DatabaseHostOrFile,
                DatabasePort,
                DatabaseName,
                DBUsername,
                DBPassword,
                "",
                true,
                AConnectionName);
        }

        /// <summary>
        /// Establishes (opens) a DB connection to a specified RDBMS.
        /// </summary>
        /// <param name="ADataBaseType">Type of the RDBMS to connect to.</param>
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
        public void EstablishDBConnection(TDBType ADataBaseType,
            String ADsnOrServer,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString,
            bool AMustCoordinateDBAccess = true,
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

                SNumberConnections++;
                TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_DETAILED_CONN_INFO, "number of database connections " + SNumberConnections.ToString());

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

                        // OnStateChangedHandler does not get called for PostgreSQL when opening the connection
                        FConnectionReady = true;
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
                    TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_DETAILED_CONN_INFO, "    Connecting to database " + ADataBaseType + " " +
                        AConnectionName + " " +
                        GetDBConnectionIdentifierInternal() + ": " +
                        TDBConnection.GetConnectionStringWithHiddenPwd(FConnectionString) + " " +
                        GetThreadAndAppDomainCallInfoForDBConnectionEstablishmentAndDisconnection());

                   try
                    {
                        FSqlConnection.Open();
                    }
                    catch (MySqlConnector.MySqlException)
                    {
                        // Possible cause: MySqlConnector.MySqlException (0x80004005): Too many connections
                        // clear all pools, and then try again. see https://github.com/openpetra/openpetra/issues/557
                        MySqlConnection.ClearAllPools();
                        FSqlConnection.Open();
                    }


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
        /// Closes the DB connection.
        /// </summary>
        /// <param name="ASuppressThreadCompatibilityCheck">Set to true to suppress a check whether the Thread that
        /// calls this Method is the Thread that established the DB Connection.</param>
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
        /// calls this Method is the Thread that established the DB Connection.</param>
        /// <exception cref="EDBConnectionNotAvailableException">Thrown if an attempt is made to close an
        /// already/still closed connection.</exception>
        private void CloseDBConnectionInternal(bool ASuppressThreadCompatibilityCheck = false)
        {
            bool RunningDBTransactionThreadIsCompatible;

            if (ConnectionReady(false))
            {
                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
                {
                    TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_DETAILED_CONN_INFO, "    Closing Database connection..." + GetDBConnectionIdentifier());
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

                        FTransaction.Rollback();

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

                SNumberConnections--;

                // Remark: TDBConnection.CloseDBConnection already calls Dispose() on FSqlConnection so we don't need to do it here
                // and we can simply set it to null.
                FSqlConnection = null;
                FConnectionReady = false;

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
                        if ((FTransaction != null) && FTransaction.Valid && (FTransaction.TransactionIdentifier != ATransaction.TransactionIdentifier))
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
        /// </summary>
        /// <param name="ASqlStatement">SQL statement. Must contain SQL Parameters if
        /// <paramref name="AParametersArray" /> is specified!</param>
        /// <param name="AReadTransaction">Instantiated <see cref="TDBTransaction" /> with the desired
        /// <see cref="IsolationLevel"/>.</param>
        /// <param name="AFillDataTable">Instance of a DataTable. Can be null; in that case a DataTable by the name of
        /// "SelectUsingDataAdapter_DataTable" is created on-the-fly.</param>
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
            ref DataTable AFillDataTable,
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
                AOptionalColumnNameMapping,
                ASelectCommandTimeout,
                AParametersArray,
                ParameterValues,
                false);
        }

        /// <summary>
        /// Executes a SQL Select Statement (1..n times if a Paramterised Query is used) using a <see cref="DbDataAdapter"/>.
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
            TOptionalColumnMappingDelegate AOptionalColumnNameMapping = null,
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
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if the current Thread is the same Thread that the currently running DB Transaction was started
        /// on; if this isn't the case false is returned.</returns>
        /// <exception cref="EDBNullTransactionException"> is thrown if there is no Running DB Transaction to check against.</exception>
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
                    throw new EDBNullTransactionException();
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
        /// asked for with the Argument.
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel"/> that is desired.</param>
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if the <see cref="IsolationLevel"/> of the currently running DB Transaction is acceptable for
        /// what is asked for with the Argument; if this isn't the case false is returned.</returns>
        /// <exception cref="EDBNullTransactionException"> is thrown if there is no Running DB Transaction to check against.</exception>
        public bool CheckRunningDBTransactionIsolationLevelIsCompatible(IsolationLevel ADesiredIsolationLevel,
            bool AMustCoordinateDBAccess = true)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
                if (FTransaction == null)
                {
                    throw new EDBNullTransactionException();
                }

                // Check if the IsolationLevel of the currently running Transaction is acceptable for what is requested with
                // the ADesiredIsolationLevel Argument: if not, this Method returns false!
                if (FTransaction.IsolationLevel < ADesiredIsolationLevel)
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

        /// <summary>
        /// create a basic transaction
        /// </summary>
        /// <returns>The transaction.</returns>
        /// <param name="AIsolationLevel">Isolation level.</param>
        public DbTransaction BeginDbTransaction(IsolationLevel AIsolationLevel)
        {
            return FSqlConnection.BeginTransaction(AIsolationLevel);
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
                if ((FTransaction != null) && (FTransaction.WrappedTransaction != null))
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
                            FTransaction.IsolationLevel, ThreadingHelper.GetThreadIdentifier(
                                FTransaction.ThreadThatTransactionWasStartedOn),
                            FTransaction.AppDomainNameThatTransactionWasStartedIn, ThreadingHelper.GetCurrentThreadIdentifier(),
                            AppDomain.CurrentDomain.FriendlyName,
                            TLogging.StackTraceToText(FTransaction.StackTraceAtPointOfTransactionStart),
                            TLogging.StackTraceToText(new StackTrace(true)));
                        TLogging.Log(NestedTransactionProblemError);

                        throw new EDBTransactionBusyException(
                            "Concurrent DB Transactions are not supported (requested IsolationLevel: " +
                            Enum.GetName(typeof(IsolationLevel), AIsolationLevel) + "): BeginTransaction would overwrite " +
                            "existing DB Transaction - You must use GetNewOrExistingTransaction or " +
                            "ReadTransaction/WriteTransaction!", NestedTransactionProblemError);
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

                        FTransaction = new TDBTransaction(this, AIsolationLevel, ATransactionName);

                        ExtendedLoggingInfoOnHigherDebugLevels(String.Format(
                                "DB Transaction{0} with IsolationLevel '{1}' got started", FTransaction.GetDBTransactionIdentifier(),
                                AIsolationLevel), DBAccess.DB_DEBUGLEVEL_TRANSACTION_DETAIL, true);
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
        /// <para>If there is a current Transaction but it has a different <see cref="IsolationLevel" />, the result
        /// depends on the value of the <paramref name="ATryToEnforceIsolationLevel" />
        /// parameter.</para>
        /// <para>If there is no current Transaction, a new Transaction with the specified <see cref="IsolationLevel" />
        /// is started.</para>
        /// </summary>
        /// <param name="ADesiredIsolationLevel"><see cref="IsolationLevel" /> that is desired</param>
        /// <param name="ANewTransaction">True if a new Transaction was started and is returned,
        /// false if an already existing Transaction is returned</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        /// <returns>Either an existing or a new Transaction that exactly meets the specified IsolationLevel</returns>
        /// <exception cref="EDBTransactionIsolationLevelWrongException">Thrown if the existing Transaction's Isolation Level
        /// hasn't got at least the Isolation Level specified.</exception>
        public TDBTransaction GetNewOrExistingTransaction(IsolationLevel ADesiredIsolationLevel,
            out Boolean ANewTransaction, string ATransactionName = "")
        {
            TDBTransaction TheTransaction;

            ANewTransaction = false;

            WaitForCoordinatedDBAccess();

            try
            {
                TheTransaction = FTransaction;

                FDataBaseRDBMS.AdjustIsolationLevel(ref ADesiredIsolationLevel);

                if ((TheTransaction != null) && TheTransaction.Valid)
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
                            false))
                    {
                        var Exc3 = new
                                   EDBTransactionIsolationLevelTooLowException(
                        "Expected IsolationLevel: at least " + ADesiredIsolationLevel.ToString("G") +
                        " but is: " + TheTransaction.IsolationLevel.ToString("G"));

                        TLogging.Log(Exc3.ToString());

                        throw Exc3;
                    }
                }

                if (TheTransaction == null || !TheTransaction.Valid)
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

                    if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION_DETAIL)
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

            if (FTransaction == null)
            {
                FTransaction = TheTransaction;
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
            // For MySQL we directly run commands against the DB (not using Methods of the TDataBase Class), hence
            // we need to co-ordinate the DB access manually. For PostgreSQL we use Methods of the TDataBase
            // Class that take care of the co-ordination themselves, hence we must not co-ordinate the DB access manually as
            // that would cause a 'deadlock'!
            if (FDbType == TDBType.MySQL)
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
                if (FDbType == TDBType.MySQL)
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
            // For MySQL we directly run commands against the DB (not using Methods of the TDataBase Class), hence
            // we need to co-ordinate the DB access manually. For PostgreSQL we use Methods of the TDataBase
            // Class that take care of the co-ordination themselves, hence we must not co-ordinate the DB access manually as
            // that would cause a 'deadlock'!
            if (FDbType == TDBType.MySQL)
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
                if (FDbType == TDBType.MySQL)
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
            bool Success = false;

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

                        Success = true;
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
                }
            }
            finally
            {
                if (AMustCoordinateDBAccess)
                {
                    ReleaseCoordinatedDBAccess();
                }
            }

            if (Success)
            {
                if (ACommitTransaction)
                {
                    ATransaction.Commit();
                }

                return NumberOfRowsAffected;
            }

            return 0;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes a SQL statement that returns a single result (eg. an SELECT COUNT(*)
        /// SQL command or a call to a Stored Procedure that inserts data and returns
        /// the value of a auto-numbered field). The statement is executed in a
        /// transaction. Suitable for parameterised SQL statements.
        /// </summary>
        /// <param name="ASqlStatement">SQL statement.</param>
        /// <param name="ATransaction">An instantiated <see cref="TDBTransaction" />.</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).</param>
        /// <returns>Single result as object.</returns>
        public object ExecuteScalar(String ASqlStatement,
            TDBTransaction ATransaction,
            DbParameter[] AParametersArray = null)
        {
            return ExecuteScalar(ASqlStatement, ATransaction, true, AParametersArray);
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
        /// <param name="AParametersArray">An array holding 1..n instantiated DbParameters (eg. OdbcParameters)
        /// (including parameter Value).</param>
        /// <returns>Single result as object.</returns>
        private object ExecuteScalar(String ASqlStatement,
            TDBTransaction ATransaction,
            bool AMustCoordinateDBAccess,
            DbParameter[] AParametersArray = null)
        {
            object ReturnValue = null;

            if ((ATransaction == null) || !ATransaction.Valid)
            {
                throw new ArgumentNullException("ATransaction", "We need a valid ATransaction for ExecuteScalar!");
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
        /// <param name="AMustCoordinateDBAccess">Set to true if the Method needs to co-ordinate DB Access on its own,
        /// set to false if the calling Method already takes care of this.</param>
        /// <returns>True if DB connection can accept commands, false if it is busy.</returns>
        private bool ConnectionReady(bool AMustCoordinateDBAccess = true)
        {
            if (AMustCoordinateDBAccess)
            {
                WaitForCoordinatedDBAccess();
            }

            try
            {
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
                PrintContext = "(Context: '" + AContext + " Thread: " + Thread.CurrentThread.ManagedThreadId.ToString() + "')" + Environment.NewLine;
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

            if ((AException.GetType() == typeof(PostgresException)) && (((PostgresException)AException).SqlState == "25P02"))
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

        /// start a read transaction, and then run the code in it
        public void ReadTransaction(ref TDBTransaction ATransaction, Action AEncapsulatedDBAccessCode)
        {
            bool SubmitOK = true;
            TransactionInner(ref ATransaction, IsolationLevel.ReadCommitted, ref SubmitOK, AEncapsulatedDBAccessCode, "ReadTransaction");
        }

        /// start a write transaction, and then run the code in it
        public void WriteTransaction(ref TDBTransaction ATransaction, ref bool ASubmitOK, Action AEncapsulatedDBAccessCode)
        {
            TransactionInner(ref ATransaction, IsolationLevel.Serializable, ref ASubmitOK, AEncapsulatedDBAccessCode, "WriteTransaction");
        }

        private void TransactionInner(ref TDBTransaction ATransaction, IsolationLevel AIsolationLevel, ref bool ASubmitOK, Action AEncapsulatedDBAccessCode, string AContext)
        {
            if (ATransaction == null)
            {
                throw new Exception("TransactionInner: requires at least an empty new TDBTransaction object");
            }

            if (AIsolationLevel != IsolationLevel.Serializable && 
                AIsolationLevel != IsolationLevel.ReadCommitted)
            {
                throw new Exception("TransactionInner: unsupported IsolationLevel " + AIsolationLevel.ToString());
            }

            bool NewTransaction = !ATransaction.Valid;

            if (ATransaction.Valid)
            {
                // check IsolationLevel compatibility
                if (AIsolationLevel == IsolationLevel.Serializable && 
                    ATransaction.IsolationLevel == IsolationLevel.ReadCommitted)
                {
                    throw new Exception("TransactionInner: trying to open a write transaction inside a read transaction");
                }
            }
            else if (FTransaction != null && FTransaction.Valid)
            {
                // can we reuse the existing Transaction of this db connection
                // we don't support nested transactions anyway
                // check IsolationLevel compatibility
                if (AIsolationLevel == IsolationLevel.Serializable && 
                    FTransaction.IsolationLevel == IsolationLevel.ReadCommitted)
                {
                    throw new Exception("TransactionInner: trying to open a write transaction inside an existing read transaction");
                }

                ATransaction.CopyFrom(FTransaction);
                NewTransaction = false;
            }
            else
            {
                ATransaction.BeginTransaction(this, AIsolationLevel, AContext);
                FTransaction = ATransaction;
            }

            bool ExceptionThrown = true;

            try
            {
                // Execute the 'encapsulated C# code section' that the caller 'sends us' in the AEncapsulatedDBAccessCode Action delegate (0..n lines of code!)
                AEncapsulatedDBAccessCode();
                ExceptionThrown = false;
            }
            finally
            {
                // commit or rollback a new transaction
                if (NewTransaction)
                {
                    if (AIsolationLevel == IsolationLevel.Serializable)
                    {
                        if (ASubmitOK && !ExceptionThrown)
                        {
                            ATransaction.Commit();
                        }
                        else
                        {
                            ATransaction.Rollback();
                        }
                    }
                    else if (AIsolationLevel == IsolationLevel.ReadCommitted)
                    {
                        ATransaction.Rollback();
                    }
                }
            }
        }

        #endregion
    }
}
