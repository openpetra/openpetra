/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2010 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Net;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Collections;
using System.Text.RegularExpressions;

namespace Ict.Common.DB
{
    /// <summary>
    /// this class allows access to MySQL databases
    /// </summary>
    public class TMySQL : IDataBaseRDBMS
    {
        /// <summary>
        /// connect to the database
        /// </summary>
        /// <param name="AServer"></param>
        /// <param name="APort"></param>
        /// <param name="ADatabaseName"></param>
        /// <param name="AUsername"></param>
        /// <param name="APassword"></param>
        /// <param name="AConnectionString"></param>
        /// <param name="AStateChangeEventHandler"></param>
        /// <returns></returns>
        public IDbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            return null;
        }

        /// <summary>
        /// format an error message if the exception is of type MysqlException
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>true if this is an NpgsqlException</returns>
        public bool LogException(Exception AException, ref string AErrorMessage)
        {
            return false;
        }

        /// <summary>
        /// format the sql query so that it works for MySQL
        /// see also the comments for TDataBase.FormatQueryRDBMSSpecific
        /// </summary>
        /// <param name="ASqlQuery"></param>
        /// <returns></returns>
        public String FormatQueryRDBMSSpecific(String ASqlQuery)
        {
            return ASqlQuery;
        }

        /// <summary>
        /// TODOComment
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement that is to be converted.</param>
        /// <returns>Array of MysqlParameter (converted from <paramref name="AParameterArray" />.</returns>
        public DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement)
        {
            return null;
        }

        /// <summary>
        /// create a IDbCommand object
        /// this formats the sql query for MySQL, and transforms the parameters
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        public IDbCommand NewCommand(ref string ACommandText, IDbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction)
        {
            return null;
        }

        /// <summary>
        /// create an IDbDataAdapter for MySQL
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter NewAdapter()
        {
            return null;
        }

        /// <summary>
        /// fill an IDbDataAdapter that was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        public void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName)
        {
        }

        /// <summary>
        /// overload of FillAdapter, just for one table
        /// IDbDataAdapter was created with NewAdapter
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataTable"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public void FillAdapter(IDbDataAdapter TheAdapter,
            ref DataTable AFillDataTable,
            Int32 AStartRecord,
            Int32 AMaxRecords)
        {
        }

        /// <summary>
        /// some databases have some problems with certain Isolation levels
        /// </summary>
        /// <param name="AIsolationLevel"></param>
        /// <returns>true if isolation level was modified</returns>
        public bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel)
        {
            // all isolation levels work fine for MySQL
            return false;
        }

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection)
        {
            return -1;
        }

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">the database object that can be used for querying</param>
        /// <param name="AConnection"></param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase, IDbConnection AConnection)
        {
            return -1;
        }
    }
}