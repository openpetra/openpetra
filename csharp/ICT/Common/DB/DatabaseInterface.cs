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
using System.Data.Common;

namespace Ict.Common.DB
{
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
}
