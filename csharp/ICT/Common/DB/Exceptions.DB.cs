//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Common.DB.Exceptions
{
    #region EDBConnectionNotEstablishedException

    /// <summary>
    /// Thrown if an attempt to create a DB connection failed.
    /// </summary>
    [Serializable()]
    public class EDBConnectionNotEstablishedException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBConnectionNotEstablishedException() : base()
        {
            TLogging.Log("Error establishing ODBC Database connection. Please check the connection parameters.");
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="msg">Exception message</param>
        public EDBConnectionNotEstablishedException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// Use this to pass on the ConnectionString with the Exception and to log the Exception
        /// </summary>
        /// <param name="AConnectionString">ODBC connection string</param>
        /// <param name="AException">Original exception
        /// </param>
        public EDBConnectionNotEstablishedException(string AConnectionString, Exception AException) : base(AConnectionString, AException)
        {
            string ErrorString;

            ErrorString = ((("Error opening ODBC Database connection. The connection string is [ " + AConnectionString) + " ]."));
            Console.WriteLine();
            TLogging.Log(ErrorString);
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBConnectionNotEstablishedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDBConnectionNotAvailableException

    /// <summary>
    /// Thrown if the DB connection is not able to execute any SQL commands.
    /// </summary>
    [Serializable()]
    public class EDBConnectionNotAvailableException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBConnectionNotAvailableException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on information about the Connection with the Exception
        /// </summary>
        /// <param name="AConnectionInfo">ConnectionState (as String) of the Database
        /// connection.
        /// </param>
        public EDBConnectionNotAvailableException(String AConnectionInfo) : base("DB Connection Status: " + AConnectionInfo)
        {
        }

        /// <summary>
        /// Use this to pass on information about the Connection with the Exception
        /// </summary>
        /// <param name="AConnectionInfo">ConnectionState (as String) of the Database
        /// connection.
        /// </param>
        /// <param name="AException">Original exception
        /// </param>
        public EDBConnectionNotAvailableException(String AConnectionInfo, Exception AException) : base("DB Connection Status: " +
                                                                                                      AConnectionInfo,
                                                                                                      AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBConnectionNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDbExecuteNonQueryBatchException

    /// <summary>
    /// Thrown by ExecuteNonQueryBatch if an DB Exception occurs while executing SQL
    /// commands.
    /// </summary>
    [Serializable()]
    public class EDBExecuteNonQueryBatchException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBExecuteNonQueryBatchException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBExecuteNonQueryBatchException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on Batch Command Information with the Exception
        /// </summary>
        /// <param name="ABatchCommandInfo">SQL statement and batch entry number where the
        /// error occured</param>
        /// <param name="AException">Original exception
        /// </param>
        public EDBExecuteNonQueryBatchException(string ABatchCommandInfo, Exception AException) : base(ABatchCommandInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBExecuteNonQueryBatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDBParameterisedQueryMissingParameterPlaceholdersException

    /// <summary>
    /// Thrown if a SQL command should execute a parameterised query, but parameter
    /// placeholders were missing in the query string.
    /// </summary>
    [Serializable()]
    public class EDBParameterisedQueryMissingParameterPlaceholdersException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBParameterisedQueryMissingParameterPlaceholdersException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBParameterisedQueryMissingParameterPlaceholdersException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBParameterisedQueryMissingParameterPlaceholdersException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBParameterisedQueryMissingParameterPlaceholdersException(SerializationInfo info, StreamingContext context) : base(info,
                                                                                                                                  context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDBTransactionIsolationLevelTooLowException

    /// <summary>
    /// Can be thrown if code wants to use a Transaction with a certain <see cref="IsolationLevel" />,
    /// but the Transaction it is using has an <see cref="IsolationLevel" /> that is lower than it
    /// expects.
    /// </summary>
    [Serializable()]
    public class EDBTransactionIsolationLevelTooLowException : ApplicationException
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EDBTransactionIsolationLevelTooLowException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBTransactionIsolationLevelTooLowException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBTransactionIsolationLevelTooLowException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBTransactionIsolationLevelTooLowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDBTransactionIsolationLevelWrongException

    /// <summary>
    /// Can be thrown if code wants to use a Transaction with a certain <see cref="IsolationLevel" />,
    /// but the Transaction it is using has a different <see cref="IsolationLevel" /> than it expects.
    /// </summary>
    [Serializable()]
    public class EDBTransactionIsolationLevelWrongException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBTransactionIsolationLevelWrongException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBTransactionIsolationLevelWrongException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBTransactionIsolationLevelWrongException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBTransactionIsolationLevelWrongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EDBTransactionMissingException

    /// <summary>
    /// Can be thrown if code wants to run an SQL query without a transaction.
    /// This would give problems with the Progress SQL engine, with locking.
    /// Progress SQL requires all SQL queries to be in a transaction.
    /// </summary>
    [Serializable()]
    public class EDBTransactionMissingException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBTransactionMissingException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBTransactionMissingException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBTransactionMissingException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBTransactionMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
    #region EDBTransactionBusyException

    /// <summary>
    /// Is thrown if a caller wants to start a new Transaction by calling
    /// BeginTransaction, but a Transaction is currently executing (parallel
    /// Transactions are not supported by ADO.NET!).
    /// </summary>
    [Serializable()]
    public class EDBTransactionBusyException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBTransactionBusyException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBTransactionBusyException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBTransactionBusyException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBTransactionBusyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
    #region EDBSyncUnknownParameterTypeException

    /// <summary>
    /// Is thrown during the write to the Sync table, where the ODBC parameters need to be written to a string for the PostgreSQL database
    /// </summary>
    [Serializable()]
    public class EDBSyncUnknownParameterTypeException : ApplicationException
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EDBSyncUnknownParameterTypeException() : base()
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        public EDBSyncUnknownParameterTypeException(String AInfo) : base(AInfo)
        {
        }

        /// <summary>
        /// Use this to pass on a message with the Exception
        /// </summary>
        /// <param name="AInfo">Exception message</param>
        /// <param name="AException">Original exception</param>
        public EDBSyncUnknownParameterTypeException(String AInfo, Exception AException) : base(AInfo, AException)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public EDBSyncUnknownParameterTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the
        /// serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
}