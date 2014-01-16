//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Runtime.Serialization;
using Ict.Common.Exceptions;

namespace Ict.Common.DB
{
    #region TDBConnection

    /// <summary>
    /// Contains functions that handle the creation of a DB connection.
    /// </summary>
    /// <remarks>
    /// This is a sealed class so that inheritance of the TDBConnection class
    /// can be prevented (to avoid Class inheritance). Call <see cref="GetInstance" />
    /// to get an Instance of this Class!
    /// This class is kind of 'low-level' - it is not intended to be
    /// instantiated except through the TDataBase.EstablishDBConnection procedures!!!
    /// The TDataBase class is the only class that a developer needs to deal with
    /// when accessing DB's!
    /// </remarks>
    internal sealed class TDBConnection : object
    {
        /// <summary>Used internally to make sure that only one instance of
        /// <see cref="TDBConnection" /> is created.</summary>
        /// <seealso cref="GetInstance" />
        private static TDBConnection FSingletonConnector;

        /// <summary>Used internally to build the Connection String.</summary>
        private static string FConnectionString;

        /// <summary>
        /// Returns an instance of <see cref="TDBConnection" />.
        /// <para>
        /// <em>This method is the only way to get an Instance of
        /// <see cref="TDBConnection" /> because the Class is <b>sealed</b>
        /// (therefore not allowing the creation of an Instance of it using a Constructor!)</em>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Before an Instance of <see cref="TDBConnection" /> is created,
        /// a check is performed whether an instance has already been created by
        /// the calling class. If this is the case, then the the same instance is returned,
        /// otherwise a new instance of <see cref="TDBConnection" /> is created and returned.
        /// </remarks>
        /// <returns>An instance of the <see cref="TDBConnection" /> Class.
        /// </returns>
        public static TDBConnection GetInstance()
        {
            // Support multithreaded applications through "Double checked locking"
            // pattern which avoids locking every time the method is invoked.
            if (FSingletonConnector == null)
            {
                FSingletonConnector = new TDBConnection();
            }

            return FSingletonConnector;
        }

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
        public DbConnection GetConnection(IDataBaseRDBMS ADataBaseRDBMS,
            String AServer,
            String APort,
            String ADatabaseName,
            String AUsername,
            ref String APassword,
            String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            FConnectionString = AConnectionString;

            return ADataBaseRDBMS.GetConnection(AServer, APort,
                ADatabaseName,
                AUsername, ref APassword,
                ref FConnectionString,
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
        /// <returns>void</returns>
        /// <exception cref="EDBConnectionAlreadyClosedException">When trying to close an
        /// already/still closed DB connection.</exception>
        public void CloseDBConnection(IDbConnection AConnection)
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

                    // TLogging.Log("Database connection closed.");
                }
                catch (Exception exp)
                {
                    TLogging.Log("Error closing Database connection!" + "Exception: " + exp.ToString());
                    throw;
                }
            }
            else
            {
                throw new EDBConnectionAlreadyClosedException();
            }
        }

        /// <summary>
        /// Returns the Connection String that is used to connect to the DataBase that
        /// TDBConnection is pointing to.
        /// </summary>
        /// <returns>Connection string - including Password marked as hidden
        /// </returns>
        public String GetConnectionString()
        {
            return FConnectionString + "*hidden*";
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