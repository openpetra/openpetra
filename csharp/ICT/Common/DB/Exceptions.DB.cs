//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using Npgsql;
using Ict.Common.Exceptions;

namespace Ict.Common.DB.Exceptions
{
    #region EDBConnectionNotEstablishedException

    /// <summary>
    /// Thrown if an attempt to create a DB connection failed.
    /// </summary>
    [Serializable()]
    public class EDBConnectionNotEstablishedException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionNotEstablishedException() : base()
        {
            TLogging.Log("Error establishing Database connection. Please check the connection parameters.");
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConnectionNotEstablishedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Use this to pass on the ConnectionString with the Exception and to log the Exception to the log file.
        /// </summary>
        /// <param name="AConnectionString">Database connection string.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        /// <param name="ALogException">Set to false to not log the Exception (default=true).</param>
        public EDBConnectionNotEstablishedException(string AConnectionString, Exception AInnerException,
            bool ALogException = true) : base(AConnectionString, AInnerException)
        {
            if (ALogException)
            {
                string ErrorString = (("Error opening Database Connection. The connection string is [ " + AConnectionString) + " ].");

                Console.WriteLine();
                TLogging.Log(ErrorString);
            }
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
        public EDBConnectionNotEstablishedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBConnectionIsAlreadyOpenException

    /// <summary>
    /// Thrown if an attempt to create a DB connection failed because the DB connection was already established earlier.
    /// </summary>
    [Serializable()]
    public class EDBConnectionIsAlreadyOpenException : EDBConnectionNotEstablishedException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionIsAlreadyOpenException() : base()
        {
            TLogging.Log("Error establishing Database connection. Please check the connection parameters.");
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConnectionIsAlreadyOpenException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Use this to pass on the ConnectionString with the Exception and to log the Exception to the log file.
        /// </summary>
        /// <param name="AConnectionString">Database connection string.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionIsAlreadyOpenException(string AConnectionString, Exception AInnerException) : base(AConnectionString, AInnerException)
        {
            string ErrorString =
                (("Error opening Database Connection as the Database Connection was already opened earlier. The connection string is [ " +
                  AConnectionString) + " ].");

            Console.WriteLine();
            TLogging.Log(ErrorString);
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
        public EDBConnectionIsAlreadyOpenException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBConnectionWasAlreadyEstablishedException

    /// <summary>
    /// Thrown if an attempt to create a DB connection failed because the DB connection was already established earlier.
    /// </summary>
    [Serializable()]
    public class EDBConnectionWasAlreadyEstablishedException : EDBConnectionNotEstablishedException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionWasAlreadyEstablishedException() : base()
        {
            TLogging.Log("Error establishing Database connection. Please check the connection parameters.");
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConnectionWasAlreadyEstablishedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Use this to pass on the ConnectionString with the Exception and to log the Exception to the log file.
        /// </summary>
        /// <param name="AConnectionString">Database connection string.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionWasAlreadyEstablishedException(string AConnectionString, Exception AInnerException) : base(AConnectionString,
                                                                                                                      AInnerException)
        {
            string ErrorString =
                (("Error opening Database Connection as the Database Connection was already established earlier. The connection string is [ " +
                  AConnectionString) + " ].");

            Console.WriteLine();
            TLogging.Log(ErrorString);
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
        public EDBConnectionWasAlreadyEstablishedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBConnectionNotAvailableException

    /// <summary>
    /// Thrown if the DB connection is not able to execute any SQL commands.
    /// </summary>
    [Serializable()]
    public class EDBConnectionNotAvailableException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionNotAvailableException() : base()
        {
        }

        /// <summary>
        /// Passes on information about the Connection.
        /// </summary>
        /// <param name="AConnectionInfo">ConnectionState (as String) of the Database connection.
        /// </param>
        public EDBConnectionNotAvailableException(String AConnectionInfo) : base("DB Connection Status: " + AConnectionInfo)
        {
        }

        /// <summary>
        /// Passes on information about the Connection and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />
        /// </summary>
        /// <param name="AConnectionInfo">ConnectionState (as String) of the Database connection.
        /// </param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionNotAvailableException(String AConnectionInfo, Exception AInnerException)
            : base("DB Connection Status: " + AConnectionInfo, AInnerException)
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
        public EDBConnectionNotAvailableException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBConnectionBrokenException

    /// <summary>
    /// Thrown if the DB Connection was found to be broken in the 'BeginTransaction' Methods
    /// (determined with a call to Method
    /// 'TExceptionHandler.IsExceptionCausedByUnavailableDBConnectionServerSide') AND the
    /// the one retry attempt to re-establish the broken DB Connection in those Methods
    /// didn't overcome the situation.
    /// </summary>
    [Serializable()]
    public class EDBConnectionBrokenException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConnectionBrokenException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConnectionBrokenException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a a reference to the inner <see cref="Exception" />
        /// that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionBrokenException(Exception AInnerException) : base("Database Connection Broken Exception occurred", AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConnectionBrokenException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBConnectionBrokenException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBExecuteNonQueryBatchException

    /// <summary>
    /// Thrown by ExecuteNonQueryBatch if an DB Exception occurs while executing SQL
    /// commands.
    /// </summary>
    [Serializable()]
    public class EDBExecuteNonQueryBatchException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBExecuteNonQueryBatchException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBExecuteNonQueryBatchException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Use this to pass on Batch Command Information and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="ABatchCommandInfo">SQL statement and batch entry number where the error occured.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBExecuteNonQueryBatchException(string ABatchCommandInfo, Exception AInnerException) : base(ABatchCommandInfo, AInnerException)
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
        public EDBExecuteNonQueryBatchException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBParameterisedQueryMissingParameterPlaceholdersException

    /// <summary>
    /// Thrown if a SQL command should execute a parameterised query, but parameter
    /// placeholders were missing in the query string.
    /// </summary>
    [Serializable()]
    public class EDBParameterisedQueryMissingParameterPlaceholdersException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBParameterisedQueryMissingParameterPlaceholdersException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBParameterisedQueryMissingParameterPlaceholdersException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBParameterisedQueryMissingParameterPlaceholdersException(string AMessage, Exception AInnerException) : base(AMessage,
                                                                                                                            AInnerException)
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
        public EDBParameterisedQueryMissingParameterPlaceholdersException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBTransactionIsolationLevelTooLowException

    /// <summary>
    /// Thrown in case code wants to use a Transaction with a certain <see cref="IsolationLevel" />,
    /// but the Transaction it is using has an <see cref="IsolationLevel" /> that is lower than it
    /// expects.
    /// </summary>
    [Serializable()]
    public class EDBTransactionIsolationLevelTooLowException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBTransactionIsolationLevelTooLowException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBTransactionIsolationLevelTooLowException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBTransactionIsolationLevelTooLowException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBTransactionIsolationLevelTooLowException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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


    #region EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException

    /// <summary>
    /// Thrown in case a caller wants to close a DB Connection when there is still a Transaction running - which the act of
    /// closing of the DB Connection would roll back - and that DB Transaction was started on another Thread (running DB
    /// commands on different Threads on the same DB Connection isn't supported as the ADO.NET providers (specifically: the
    /// PostgreSQL provider, Npgsql) aren't thread-safe!).
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException : EDBAccessLackingCoordinationException
    {
        private String FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection;
        private String FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection;

        /// <summary>
        /// ThreadIdentifier of the Thread which established the current DB Connection.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection
        {
            get
            {
                return FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection;
            }

            set
            {
                FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection = value;
            }
        }

        /// <summary>
        /// ThreadIdentifier of the Thread which attempted to close the current DB Connection.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection
        {
            get
            {
                return FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection;
            }

            set
            {
                FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection" /> and
        /// <see cref="ThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection">ThreadIdentifier of the Thread which
        /// established the current DB Connection.</param>
        /// <param name="AThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection">ThreadIdentifier of the Thread which
        /// attempted to close the current DB Connection.</param>
        public EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException(String AMessage,
            String AThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection,
            string AThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection) : base(AMessage)
        {
            FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection = AThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection;
            FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection = AThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException(string AMessage,
            Exception AInnerException) : base(AMessage,
                                             AInnerException)
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
        public EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException(SerializationInfo AInfo,
            StreamingContext AContext) : base(AInfo, AContext)
        {
            FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection = AInfo.GetString(
                "ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection");
            FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection = AInfo.GetString(
                "ThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection");
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

            AInfo.AddValue("ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection",
                FThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection);
            AInfo.AddValue("ThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection",
                FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToCloseDBConnectionThatGotEstablishedOnDifferentThreadException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection != null)
                && (ThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection != null))
            {
                ReturnValue += "  (ThreadIdentifier of the Thread which established the current DB Connection: " +
                               ThreadIdentifierOfThreadWhichEstablishedCurrentDBConnection + "; " +
                               "ThreadIdentifier of the Thread which attempted to close the current DB Connection.: " +
                               FThreadIdentifierOfThreadWhichAttemptedToCloseCurrentDBConnection + ")" + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion


    #region EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException

    /// <summary>
    /// Thrown in case a caller wants to work with a Transaction and that DB Transaction was started on another Thread
    /// (running DB commands on different Threads in the same DB Transaction [and hence on the same DB Connection] isn't supported!).
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException : EDBAccessLackingCoordinationException
    {
        private String FThreadIdentifierOfThreadWhichStartedExistingTransaction;
        private String FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;

        /// <summary>
        /// ThreadIdentifier of the Thread which started the existing Transaction.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichStartedExistingTransaction
        {
            get
            {
                return FThreadIdentifierOfThreadWhichStartedExistingTransaction;
            }

            set
            {
                FThreadIdentifierOfThreadWhichStartedExistingTransaction = value;
            }
        }

        /// <summary>
        /// ThreadIdentifier of the Thread which wants to reuse an existing Transaction.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction
        {
            get
            {
                return FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;
            }

            set
            {
                FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="ThreadIdentifierOfThreadWhichStartedExistingTransaction" /> and
        /// <see cref="ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AThreadIdentifierOfThreadWhichStartedExistingTransaction">ThreadIdentifier of the Thread which
        /// started the existing Transaction.</param>
        /// <param name="AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction">ThreadIdentifier of the Thread which
        /// wants to reuse an existing Transaction.</param>
        public EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(String AMessage,
            String AThreadIdentifierOfThreadWhichStartedExistingTransaction,
            string AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction) : base(AMessage)
        {
            FThreadIdentifierOfThreadWhichStartedExistingTransaction = AThreadIdentifierOfThreadWhichStartedExistingTransaction;
            FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(string AMessage, Exception AInnerException) : base(AMessage,
                                                                                                                                            AInnerException)
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
        public EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException(SerializationInfo AInfo,
            StreamingContext AContext) : base(AInfo, AContext)
        {
            FThreadIdentifierOfThreadWhichStartedExistingTransaction = AInfo.GetString("ThreadIdentifierOfThreadWhichStartedExistingTransaction");
            FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = AInfo.GetString(
                "ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction");
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

            AInfo.AddValue("ThreadIdentifierOfThreadWhichStartedExistingTransaction",
                FThreadIdentifierOfThreadWhichStartedExistingTransaction);
            AInfo.AddValue("ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction",
                FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((ThreadIdentifierOfThreadWhichStartedExistingTransaction != null)
                && (ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction != null))
            {
                ReturnValue += "  (ThreadIdentifier of the Thread that started the existing Transaction: " +
                               ThreadIdentifierOfThreadWhichStartedExistingTransaction + "; " +
                               "ThreadIdentifier of the Thread that wants to use the existing Transaction: " +
                               FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction + ")" + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion

    #region EDBNullTransactionException

    /// <summary>
    /// Thrown if a transaction is expected on a DB connection but the current transaction is null.
    /// </summary>
    [Serializable()]
    public class EDBNullTransactionException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBNullTransactionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance with a specific error message.
        /// </summary>
        /// <param name="AMessage">An error message explaining the reason for this exception.</param>
        public EDBNullTransactionException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance with a specific error message and the inner exception which caused this one.
        /// </summary>
        /// <param name="AMessage">An error message explaining the reason for this exception.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBNullTransactionException(String AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
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
        public EDBNullTransactionException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBAttemptingToUseTransactionThatIsInvalidException

    /// <summary>
    /// Thrown in case a caller wants to use a DB Transaction and that DB Transaction is no longer Valid (i.e. that
    /// DB Transaction has already been Committed or Rolled Back).
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToUseTransactionThatIsInvalidException : EDBAccessLackingCoordinationException
    {
        private String FThreadIdentifierOfThreadWhichStartedExistingTransaction;
        private String FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;

        /// <summary>
        /// ThreadIdentifier of the Thread which started the existing Transaction.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichStartedExistingTransaction
        {
            get
            {
                return FThreadIdentifierOfThreadWhichStartedExistingTransaction;
            }

            set
            {
                FThreadIdentifierOfThreadWhichStartedExistingTransaction = value;
            }
        }

        /// <summary>
        /// ThreadIdentifier of the Thread which wants to reuse an existing Transaction.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction
        {
            get
            {
                return FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;
            }

            set
            {
                FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToUseTransactionThatIsInvalidException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToUseTransactionThatIsInvalidException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="ThreadIdentifierOfThreadWhichStartedExistingTransaction" /> and
        /// <see cref="ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AThreadIdentifierOfThreadWhichStartedExistingTransaction">ThreadIdentifier of the Thread which
        /// started the existing Transaction.</param>
        /// <param name="AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction">ThreadIdentifier of the Thread which
        /// wants to reuse an existing Transaction.</param>
        public EDBAttemptingToUseTransactionThatIsInvalidException(String AMessage,
            String AThreadIdentifierOfThreadWhichStartedExistingTransaction,
            string AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction) : base(AMessage)
        {
            FThreadIdentifierOfThreadWhichStartedExistingTransaction = AThreadIdentifierOfThreadWhichStartedExistingTransaction;
            FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = AThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToUseTransactionThatIsInvalidException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAttemptingToUseTransactionThatIsInvalidException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
            FThreadIdentifierOfThreadWhichStartedExistingTransaction = AInfo.GetString("ThreadIdentifierOfThreadWhichStartedExistingTransaction");
            FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction = AInfo.GetString(
                "ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction");
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

            AInfo.AddValue("ThreadIdentifierOfThreadWhichStartedExistingTransaction",
                FThreadIdentifierOfThreadWhichStartedExistingTransaction);
            AInfo.AddValue("ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction",
                FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToUseTransactionThatIsInvalidException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToUseTransactionThatIsInvalidException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((ThreadIdentifierOfThreadWhichStartedExistingTransaction != null)
                && (ThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction != null))
            {
                ReturnValue += "  (ThreadIdentifier of the Thread that started the existing Transaction: " +
                               ThreadIdentifierOfThreadWhichStartedExistingTransaction + "; " +
                               "ThreadIdentifier of the Thread that wants to reuse an existing Transaction: " +
                               FThreadIdentifierOfThreadWhichWantsToReuseExistingTransaction + ")" + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion

    #region EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException

    /// <summary>
    /// Thrown in case a caller wants to create a Command that would be executed on a Thread that is different from the Thread
    /// that started the DB Transaction that got passed in as an Argument (running DB commands on different
    /// Threads in the same DB Transaction [and hence on the same DB Connection] isn't supported as the ADO.NET providers
    /// [specifically: the PostgreSQL provider, Npgsql] aren't thread-safe!)
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException :
        EDBAttemptingToWorkWithTransactionThatGotStartedOnDifferentThreadException
    {
        private String FThreadIdentifierOfThreadWhichStartedTransaction;
        private String FThreadIdentifierOfThreadWhichWantsToCreateCommand;

        /// <summary>
        /// ThreadIdentifier of the Thread which started the existing Transaction.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichStartedTransaction
        {
            get
            {
                return FThreadIdentifierOfThreadWhichStartedTransaction;
            }

            set
            {
                FThreadIdentifierOfThreadWhichStartedTransaction = value;
            }
        }

        /// <summary>
        /// ThreadIdentifier of the Thread which wants to create a Command using the Transaction that was started on another
        /// Thread.
        /// </summary>
        public String ThreadIdentifierOfThreadWhichWantsToCreateCommand
        {
            get
            {
                return FThreadIdentifierOfThreadWhichWantsToCreateCommand;
            }

            set
            {
                FThreadIdentifierOfThreadWhichWantsToCreateCommand = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException(String AMessage) :
            base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="ThreadIdentifierOfThreadWhichStartedTransaction" /> and
        /// <see cref="ThreadIdentifierOfThreadWhichWantsToCreateCommand" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AThreadIdentifierOfThreadWhichStartedTransaction">ThreadIdentifier of the Thread which
        /// started the existing Transaction.</param>
        /// <param name="AThreadIdentifierOfThreadWhichWantsToCreateCommand">ThreadIdentifier of the Thread which wants
        /// to create a Command using the Transaction that was started on another Thread.</param>
        public EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException(String AMessage,
            String AThreadIdentifierOfThreadWhichStartedTransaction,
            string AThreadIdentifierOfThreadWhichWantsToCreateCommand) : base(AMessage)
        {
            FThreadIdentifierOfThreadWhichStartedTransaction = AThreadIdentifierOfThreadWhichStartedTransaction;
            FThreadIdentifierOfThreadWhichWantsToCreateCommand = AThreadIdentifierOfThreadWhichWantsToCreateCommand;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException(string AMessage,
            Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException(
            SerializationInfo AInfo,
            StreamingContext AContext) : base(AInfo, AContext)
        {
            FThreadIdentifierOfThreadWhichStartedTransaction = AInfo.GetString("ThreadIdentifierOfThreadWhichStartedTransaction");
            FThreadIdentifierOfThreadWhichWantsToCreateCommand = AInfo.GetString("ThreadIdentifierOfThreadWhichWantsToCreateCommand");
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

            AInfo.AddValue("ThreadIdentifierOfThreadWhichStartedTransaction",
                FThreadIdentifierOfThreadWhichStartedTransaction);
            AInfo.AddValue("ThreadIdentifierOfThreadWhichWantsToCreateCommand",
                FThreadIdentifierOfThreadWhichWantsToCreateCommand);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandThatWouldRunCommandOnDifferentThreadThanThreadOfTheTransactionThatGotPassedException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((ThreadIdentifierOfThreadWhichStartedTransaction != null)
                && (ThreadIdentifierOfThreadWhichWantsToCreateCommand != null))
            {
                ReturnValue += "  (ThreadIdentifier of the Thread that started the Transaction: " +
                               ThreadIdentifierOfThreadWhichStartedTransaction + "; " +
                               "ThreadIdentifier of the Thread that wants to create a Command using the Transaction that was started on " +
                               "another Thread: " + FThreadIdentifierOfThreadWhichWantsToCreateCommand + ")" + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion

    #region EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException

    /// <summary>
    /// Thrown in case a caller wants to create a Command that would be enlisted in a DB Transaction that was started against
    /// a DB Connection that is different from the DB Connection of current DB Connection (running DB Commands on one
    /// DB Connection with a DB Transaction that was started against another DB Connection isn't supported [because each
    /// DB Command is bound to a certain DB Connection as the ADO.NET providers {specifically: the PostgreSQL provider,
    /// Npgsql} aren't thread-safe!}]).
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException :
        EDBAccessLackingCoordinationException
    {
        private String FConnectionIdentifierOfConnectionOfPassedInTransaction;
        private String FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance;

        /// <summary>
        /// ConnectionIdentifier of the Connection of the Transaction that got passed in as an Argument.
        /// </summary>
        public String ConnectionIdentifierOfConnectionOfPassedInTransaction
        {
            get
            {
                return FConnectionIdentifierOfConnectionOfPassedInTransaction;
            }

            set
            {
                FConnectionIdentifierOfConnectionOfPassedInTransaction = value;
            }
        }

        /// <summary>
        /// ConnectionIdentifier of the current TDataBase instance.
        /// </summary>
        public String ConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance
        {
            get
            {
                return FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance;
            }

            set
            {
                FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException(String AMessage) :
            base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="ConnectionIdentifierOfConnectionOfPassedInTransaction" /> and
        /// <see cref="ConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AConnectionIdentifierOfConnectionOfPassedInTransaction">ConnectionIdentifier of the Connection of the
        /// Transaction that got passed in as an Argument.</param>
        /// <param name="AConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance">ConnectionIdentifier of the
        /// current TDataBase instance.</param>
        public EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException(String AMessage,
            String AConnectionIdentifierOfConnectionOfPassedInTransaction,
            string AConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance) : base(AMessage)
        {
            FConnectionIdentifierOfConnectionOfPassedInTransaction = AConnectionIdentifierOfConnectionOfPassedInTransaction;
            FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance = AConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException(string AMessage,
            Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException(
            SerializationInfo AInfo,
            StreamingContext AContext) : base(AInfo, AContext)
        {
            FConnectionIdentifierOfConnectionOfPassedInTransaction = AInfo.GetString("ConnectionIdentifierOfConnectionOfPassedInTransaction");
            FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance = AInfo.GetString(
                "ConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance");
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

            AInfo.AddValue("ConnectionIdentifierOfConnectionOfPassedInTransaction",
                FConnectionIdentifierOfConnectionOfPassedInTransaction);
            AInfo.AddValue("ConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance",
                FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandOnDifferentDBConnectionThanTheDBConnectionOfOfTheDBTransactionThatGotPassedException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((ConnectionIdentifierOfConnectionOfPassedInTransaction != null)
                && (ConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance != null))
            {
                ReturnValue += "  (ConnectionIdentifier of the Connection of the Transaction that got passed in as an " +
                               "Argument: " + FConnectionIdentifierOfConnectionOfPassedInTransaction + "; " +
                               "ConnectionIdentifier of the current TDataBase instance: " +
                               FConnectionIdentifierOfConnectionOfCurrentTDataBaseInstance + ")" + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion

    #region EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException

    /// <summary>
    /// Thrown in case a caller wants to create a Command that would be enlisted in a DB Transaction that is different from
    /// the current DB Transaction (running DB Commands on a DB Transaction other than the currently running DB Transaction
    /// is not supported [because parallel DB Transactions are not supported]).
    /// </summary>
    [Serializable()]
    public class EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException :
        EDBAccessLackingCoordinationException
    {
        private String FTransactionIdentifierOfPassedInTransaction;
        private String FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance;

        /// <summary>
        /// TransactionIdentifier of the Transaction that got passed in as an Argument.
        /// </summary>
        public String TransactionIdentifierOfPassedInTransaction
        {
            get
            {
                return FTransactionIdentifierOfPassedInTransaction;
            }

            set
            {
                FTransactionIdentifierOfPassedInTransaction = value;
            }
        }

        /// <summary>
        /// TransactionIdentifier of the current Transaction of the current TDataBase instance (which should create the Command).
        /// </summary>
        public String TransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance
        {
            get
            {
                return FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance;
            }

            set
            {
                FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and
        /// <see cref="TransactionIdentifierOfPassedInTransaction" /> and
        /// <see cref="TransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ATransactionIdentifierOfPassedInTransaction">TransactionIdentifier of the Transaction that
        /// got passed in as an Argument.</param>
        /// <param name="ATransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance">TransactionIdentifier
        /// of the current Transaction of the current TDataBase instance (which should create the Command).</param>
        public EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException(String AMessage,
            String ATransactionIdentifierOfPassedInTransaction,
            string ATransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance) : base(AMessage)
        {
            FTransactionIdentifierOfPassedInTransaction = ATransactionIdentifierOfPassedInTransaction;
            FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance =
                ATransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException(string AMessage,
            Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException(SerializationInfo AInfo,
            StreamingContext AContext) : base(AInfo, AContext)
        {
            FTransactionIdentifierOfPassedInTransaction = AInfo.GetString("TransactionIdentifierOfPassedInTransaction");
            FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance = AInfo.GetString(
                "TransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance");
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

            AInfo.AddValue("TransactionIdentifierOfPassedInTransaction",
                FTransactionIdentifierOfPassedInTransaction);
            AInfo.AddValue("TransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance",
                FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException"/>.
        /// </summary>
        /// <returns>A string representation of the current
        /// <see cref="EDBAttemptingToCreateCommandEnlistedInDifferentDBTransactionThanTheCurrentDBTransactionException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if ((TransactionIdentifierOfPassedInTransaction != null)
                && (TransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance != null))
            {
                ReturnValue += "  (TransactionIdentifier of the Transaction that got passed in as an Argument: " +
                               FTransactionIdentifierOfPassedInTransaction + "; " +
                               "TransactionIdentifier of the current Transaction of the current TDataBase instance (which should create " +
                               "the Command): " + FTransactionIdentifierOfCurrentTransactionOfCurrentTDataBaseInstance + ")" +
                               Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion


    #region EDBTransactionIsolationLevelWrongException

    /// <summary>
    /// Thrown in case code wants to use a Transaction with a certain <see cref="IsolationLevel" />,
    /// but the Transaction it is using has a different <see cref="IsolationLevel" /> than it expects.
    /// </summary>
    [Serializable()]
    public class EDBTransactionIsolationLevelWrongException : EDBAccessLackingCoordinationException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBTransactionIsolationLevelWrongException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBTransactionIsolationLevelWrongException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBTransactionIsolationLevelWrongException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBTransactionIsolationLevelWrongException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBTransactionMissingException

    /// <summary>
    /// Thrown in case code wants to run an SQL query without a transaction.
    /// This would give problems with the Progress SQL engine, with locking.
    /// Progress SQL requires all SQL queries to be in a transaction.
    /// </summary>
    [Serializable()]
    public class EDBTransactionMissingException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBTransactionMissingException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBTransactionMissingException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBTransactionMissingException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBTransactionMissingException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBAccessLackingCoordinationException

    /// <summary>
    /// Thrown in case a caller wants to run an action against the DB, but that isn't possible as another action that was
    /// started earlier prevents that - this means that the DB access isn't co-ordinated (but it would need to be!).
    /// </summary>
    [Serializable()]
    public class EDBAccessLackingCoordinationException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAccessLackingCoordinationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAccessLackingCoordinationException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAccessLackingCoordinationException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAccessLackingCoordinationException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBTransactionBusyException

    /// <summary>
    /// Thrown in case a caller wants to start a new Transaction by calling
    /// BeginTransaction, but a Transaction is currently executing (parallel
    /// Transactions are not supported by ADO.NET!).
    /// </summary>
    [Serializable()]
    public class EDBTransactionBusyException : EDBAccessLackingCoordinationException
    {
        private String FNestedTransactionProblemDetails;

        /// <summary>
        /// Nested Transaction Problem Details.
        /// </summary>
        public String NestedTransactionProblemDetails
        {
            get
            {
                return FNestedTransactionProblemDetails;
            }

            set
            {
                FNestedTransactionProblemDetails = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBTransactionBusyException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBTransactionBusyException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and <see cref="NestedTransactionProblemDetails" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ANestedTransactionProblemDetails">Nested Transaction Problem Details.</param>
        public EDBTransactionBusyException(String AMessage, String ANestedTransactionProblemDetails) : base(AMessage)
        {
            FNestedTransactionProblemDetails = ANestedTransactionProblemDetails;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBTransactionBusyException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBTransactionBusyException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
            FNestedTransactionProblemDetails = AInfo.GetString("NestedTransactionProblemDetails");
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

            AInfo.AddValue("NestedTransactionProblemDetails", FNestedTransactionProblemDetails);

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion

        /// <summary>
        /// Creates and returns a string representation of the current <see cref="EDBTransactionBusyException"/>.
        /// </summary>
        /// <returns>A string representation of the current <see cref="EDBTransactionBusyException"/>.</returns>
        /// <remarks>See https://msdn.microsoft.com/en-us/library/system.exception.tostring(v=vs.100).aspx for the standard
        /// .NET implementation of the System.Exception.ToString() Method.</remarks>
        public override string ToString()
        {
            string ReturnValue;
            string StackTraceStr;

            // Start the ToString() return value as .NET does for the System.Exception.ToString() Method...
            ReturnValue = GetType().FullName + ": " + Message + Environment.NewLine;

            // ...then add our "special information"...
            if (NestedTransactionProblemDetails != null)
            {
                ReturnValue += "  --> " + NestedTransactionProblemDetails + Environment.NewLine;
            }

            // ...and end the ToString() return value as .NET does for the System.Exception.ToString() Method.
            if (InnerException != null)
            {
                ReturnValue += InnerException.ToString() + Environment.NewLine;
            }

            StackTraceStr = Environment.StackTrace;

            if (!String.IsNullOrEmpty(StackTraceStr))
            {
                ReturnValue += "Server stack trace:" + Environment.StackTrace + Environment.NewLine;
            }

            return ReturnValue;
        }
    }

    #endregion

    #region EDBCoordinatedDBAccessWaitingTimeExceededException

    /// <summary>
    /// Thrown when co-ordinated (=Thread-safe) DB Access was requested, but the DB Abstraction layer was busy executing
    /// another request, and the waiting time for the request for which this Exception got thrown was exceeded.
    /// The request for which this Exception got thrown didn't get executed because of this.
    /// </summary>
    [Serializable()]
    public class EDBCoordinatedDBAccessWaitingTimeExceededException : EDBAccessLackingCoordinationException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBCoordinatedDBAccessWaitingTimeExceededException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBCoordinatedDBAccessWaitingTimeExceededException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBCoordinatedDBAccessWaitingTimeExceededException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBCoordinatedDBAccessWaitingTimeExceededException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBAutoServerCallRetriesExceededException

    /// <summary>
    /// Thrown when co-ordinated (=Thread-safe) DB Access was requested, but the DB Abstraction layer was busy executing
    /// another request, and the waiting time for the request for which this Exception got thrown was exceeded.
    /// The request for which this Exception got thrown didn't get executed because of this.
    /// </summary>
    [Serializable()]
    public class EDBAutoServerCallRetriesExceededException : EDBAccessLackingCoordinationException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBAutoServerCallRetriesExceededException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBAutoServerCallRetriesExceededException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBAutoServerCallRetriesExceededException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBAutoServerCallRetriesExceededException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBSyncUnknownParameterTypeException

    /// <summary>
    /// Thrown during the write to the Sync table, where the ODBC parameters need to be written to a string
    /// for the PostgreSQL database.
    /// </summary>
    [Serializable()]
    public class EDBSyncUnknownParameterTypeException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBSyncUnknownParameterTypeException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBSyncUnknownParameterTypeException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBSyncUnknownParameterTypeException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBSyncUnknownParameterTypeException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDBUnsupportedDBUpgradeException

    /// <summary>
    /// Thrown if an upgrade of the DB to a newer version of OpenPetra cannot be done.
    /// </summary>
    [Serializable()]
    public class EDBUnsupportedDBUpgradeException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBUnsupportedDBUpgradeException() : base()
        {
            TLogging.Log("Upgrading the database to a newer version of OpenPetra is not supported.");
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <remarks>The string that is passed in <paramref name="AMessage" /> currently needs
        /// to contain the exact string "Unsupported upgrade" so that the Client can react properly...</remarks>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBUnsupportedDBUpgradeException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <remarks>The string that is passed in <paramref name="AMessage" /> currently needs
        /// to contain the exact string "Unsupported upgrade" so that the Client can react properly...</remarks>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBUnsupportedDBUpgradeException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBUnsupportedDBUpgradeException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EDB40001TransactionSerialisationException

    /// <summary>
    /// Thrown during a serializable transaction if another concurrent transaction has messed with the same records
    /// for the PostgreSQL database.
    /// </summary>
    [Serializable()]
    public class EDBTransactionSerialisationException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBTransactionSerialisationException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBTransactionSerialisationException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and Inner Exception.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException"></param>
        public EDBTransactionSerialisationException(String AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EDBTransactionSerialisationException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region Static Helper Class with methods dealing with DB Exception

    /// <summary>
    /// Static helper class for DB Exceptions
    /// </summary>
    public static class TDBExceptionHelper
    {
        /// <summary>
        /// Tests to see if the exception was raised by Npgsql and is an ERROR 40001.  This is the error raised when serializable transactions collide.
        /// </summary>
        public static bool IsFirstChanceNpgsql40001Exception(Exception AException)
        {
            if (AException is PostgresException)
            {
                PostgresException e = (PostgresException)AException;

                if ((e.SqlState == "40001") && (e.Severity == "ERROR"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tests to see if the exception was raised by Npgsql and is an ERROR 23505 and the constraint is a primary key.
        /// This error is PROBABLY raised when serializable transactions collide.
        /// </summary>
        public static bool IsFirstChanceNpgsql23505Exception(Exception AException)
        {
            if (AException is PostgresException)
            {
                PostgresException e = (PostgresException)AException;

                // AlanP note: We check for these three things...  The error could also be raised by a unique key constraint error that is not a primary key.
                // But we will assume that that is not a transaction collision for now.
                // Note that we may get a pk constraint error that is not a transaction collision but then it would probably be a programming error??
                // Raising this exception will result in a friendly message suggesting there was another user that used this pk at the same time.
                if ((e.SqlState == "23505") && (e.Severity == "ERROR") && (e.ConstraintName.EndsWith("_pk")))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tests to see if the exception looks like it was caused by serializable transactions colliding.
        /// </summary>
        public static bool IsTransactionSerialisationException(Exception AException)
        {
            Exception ex = AException;

            while (ex != null)
            {
                // Check if it is one of our specific exceptions
                if (ex is EDBTransactionSerialisationException)
                {
                    return true;
                }

                ex = ex.InnerException;
            }

            return false;
        }

        #endregion
    }
}
