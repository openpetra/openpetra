//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using System.Runtime.Serialization;
using Ict.Common.Exceptions;

namespace Ict.Common.DB
{
    #region TDBConnection

    /// <summary>
    /// Contains functions that handle the creation and closing of a DB connection.
    /// </summary>
    /// <remarks>
    /// This class is kind of 'low-level' - it is not intended to be
    /// instantiated except through the TDataBase.EstablishDBConnection procedures!!!
    /// The TDataBase class is the only class that a developer needs to deal with
    /// when accessing DB's!
    /// </remarks>
    internal static class TDBConnection
    {
        /// <summary>
        /// Opens a connection to the specified database
        /// </summary>
        /// <param name="ADataBaseRDBMS">the database functions for the selected type of database</param>
        /// <param name="AServer">The Database Server</param>
        /// <param name="APort">the port that the db server is running on</param>
        /// <param name="ADatabaseName">the database to connect to</param>
        /// <param name="AUsername">The username for opening the connection</param>
        /// <param name="APassword">The password for opening the connection</param>
        /// <param name="AConnectionString">The connection string; if it is not empty, it will overrule the previous parameters</param>
        /// <param name="AStateChangeEventHandler">for connection state changes</param>
        /// <returns>Opened Connection (null if connection could not be established).
        /// </returns>
        public static DbConnection GetConnection(IDataBaseRDBMS ADataBaseRDBMS,
            String AServer,
            String APort,
            String ADatabaseName,
            String AUsername,
            ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            return ADataBaseRDBMS.GetConnection(AServer, APort,
                ADatabaseName,
                AUsername, ref APassword,
                ref AConnectionString,
                AStateChangeEventHandler);
        }

        /// <summary>
        /// Closes a DB connection. Also calls the <c>Dispose()</c> Method on the DB connection object.
        /// </summary>
        /// <remarks>
        /// Although the .NET FCL allows the <see cref="IDbConnection.Close" /> method to be
        /// called even on already closed connections without causing an error,
        /// for the purposes of cleaner application development this method throws
        /// an exception when the caller tries to close an already/still closed
        /// connection.
        /// </remarks>
        /// <param name="AConnection">Open Database connection</param>
        /// <param name="AConnectionName">Name of the DB Connection (can be an empty string).</param>
        /// <exception cref="EDBConnectionAlreadyClosedException">When trying to close an
        /// already/still closed DB connection.</exception>
        public static void CloseDBConnection(IDbConnection AConnection, string AConnectionName)
        {
            if (AConnection == null)
            {
                throw new ArgumentNullException("AConnection", "AConnection must not be null!");
            }

            if (AConnection.State != ConnectionState.Closed)
            {
                try
                {
                    AConnection.Close();
                    AConnection.Dispose();

                    TLogging.Log(
                        "    " +
                        (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE ? "CloseDBConnection:" : "") + 
                            "Database connection closed." + AConnectionName + " " + TDataBase.GetThreadAndAppDomainCallInfoForDBConnectionEstablishmentAndDisconnection());
                }
                catch (Exception exp)
                {
                    TLogging.Log(
                        (TLogging.DL >=
                         DBAccess.DB_DEBUGLEVEL_TRACE ? "CloseDBConnection:" : "") + "Error closing Database connection!" + 
                            AConnectionName + " " + TDataBase.GetThreadAndAppDomainCallInfoForDBConnectionEstablishmentAndDisconnection() +
                            " Exception: " + exp.ToString());
                    throw;
                }
            }
            else
            {
                throw new EDBConnectionAlreadyClosedException();
            }
        }

        /// <summary>
        /// Clears (empties) *all* the Connection Pools that a RDBMS driver provides (irrespecive of the Connection String
        /// that is associated with any connection). In case an RDBMS type doesn't provide a Connection Pool nothing is done
        /// and the Method returns no error.
        /// </summary>
        /// <remarks>
        /// THERE IS NORMALLY NO NEED TO EXECUTE THIS METHOD - IN FACT THIS METHOD SHOULD NOT GET CALLED as it will have a
        /// negative performance impact when subsequent DB Connections are opened! Use this Method only for 'unit-testing'
        /// DB Connection-related issues (such as that DB Connections are really closed when they ought to be).
        /// </remarks>
        public static void ClearAllConnectionPools()
        {
            TPostgreSQL.ClearAllConnectionPools();
        }

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
        public static void ClearConnectionPool(IDataBaseRDBMS ADataBaseRDBMS, DbConnection ADBConnection)
        {
            ADataBaseRDBMS.ClearConnectionPool(ADBConnection);
        }

        /// <summary>
        /// Returns the Connection String that is used to connect to the DataBase that
        /// TDBConnection is pointing to.
        /// </summary>
        /// <param name="AConnectionString">Connection String that the connection got opened with.</param>
        /// <returns>Connection string - including Password marked as hidden.</returns>
        public static String GetConnectionStringWithHiddenPwd(string AConnectionString)
        {
            return AConnectionString + "*hidden*";
        }
    }

    #endregion

    #region EDBConnectionAlreadyClosedException

    /// <summary>
    /// Thrown if an attempt is made to close an already/still closed DB Connection.
    /// </summary>
    [Serializable()]
    public class EDBConnectionAlreadyClosedException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionAlreadyClosedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConnectionAlreadyClosedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionAlreadyClosedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EDBConnectionAlreadyClosedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }
    #endregion
}