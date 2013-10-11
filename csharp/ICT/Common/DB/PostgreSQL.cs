//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Data.Odbc;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using Npgsql;
using NpgsqlTypes;

using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB
{
    /// <summary>
    /// Allows access to PostgreSQL databases using the 'Npgsql' .NET Data Provider.
    /// </summary>
    public class TPostgreSQL : IDataBaseRDBMS
    {
#if WITH_POSTGRESQL_LOGGING
        private const string NPGSQL_LOGFILENAME = "U:\\tmp\\Npgsql.log";
#endif

        /// <summary>
        /// Creates a PostgreSQL connection using the 'Npgsql' .NET Data Provider.
        /// This works on Windows and on Linux.
        /// </summary>
        /// <param name="AServer">Database Server.</param>
        /// <param name="APort">Port that the DB server is running on.</param>
        /// <param name="ADatabaseName">Name of the database that we want to connect to.</param>
        /// <param name="AUsername">Username for opening the PostgreSQL connection.</param>
        /// <param name="APassword">Password for opening the PostgreSQL connection.</param>
        /// <param name="AConnectionString">Connection string; if it is not empty, it will
        /// overrule the previous parameters.</param>
        /// <param name="AStateChangeEventHandler">Event Handler for connection state changes
        /// (NOTE: This doesn't work yet with the Npgsql driver - see code comments in this Methods'
        /// source code)!</param>
        /// <returns>
        /// Instantiated NpgsqlConnection, but not opened yet (null if connection could not be established).
        /// </returns>
        public DbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            ArrayList ExceptionList;
            NpgsqlConnection TheConnection = null;

#if EXTREME_DEBUGGING
            NpgsqlEventLog.Level = LogLevel.Debug;
            NpgsqlEventLog.LogName = "NpgsqlTests.LogFile";
            NpgsqlEventLog.EchoMessages = false;
#endif

            if (String.IsNullOrEmpty(AConnectionString))
            {
                if (String.IsNullOrEmpty(AUsername))
                {
                    throw new ArgumentException("AUsername", "AUsername must not be null or an empty string!");
                }

                if (String.IsNullOrEmpty(APassword))
                {
                    throw new ArgumentException("APassword", "APassword must not be null or an empty string!");
                }

                // TODO: Make 'ConnectionLifeTime' and 'CommandTimeout' configurable somehow. That would allow
                // us to cater better for server environments where the server is quite busy and the RDBMS could
                // therefore be slow to respond! See https://tracker.openpetra.org/view.php?id=2330.
                AConnectionString = "Server=" + AServer + ";Port=" + APort + ";User Id=" + AUsername +
                                    ";Database=" + ADatabaseName + ";ConnectionLifeTime=60;CommandTimeout=3600;Password=";
            }

            try
            {
                // TLogging.Log('Full ConnectionString (with Password!): ''' + ConnectionString + '''');
                // TLogging.Log('Full ConnectionString (with Password!): ''' + ConnectionString + '''');
                // TLogging.Log('ConnectionStringBuilder.ToString (with Password!): ''' + ConnectionStringBuilder.ToString + '''');
                // Now try to connect to the DB
                TheConnection = new NpgsqlConnection();
                TheConnection.ConnectionString = AConnectionString + APassword + ";";
            }
            catch (Exception exp)
            {
                ExceptionList = new ArrayList();
                ExceptionList.Add((("Error establishing a DB connection to: " + AConnectionString) + Environment.NewLine));
                ExceptionList.Add((("Exception thrown :- " + exp.ToString()) + Environment.NewLine));
                TLogging.Log(ExceptionList, true);
            }

            if (TheConnection != null)
            {
                // Somehow the StateChange Event is never fired for an NpgsqlConnection, although it is documented.
                // As a result of that we cannot rely on the FConnectionReady variable to contain valid values for
                // NpgsqlConnection. Therefore I (ChristianK) wrote a wrapper routine, ConnectionReady, which
                // handles this difference. FConnectionReady must therefore never be inquired directly, but only
                // through calling ConnectionReady()!
                // (As of Npgsql 2.0.11.92 the Event still isn't raised)

                // TODO: need to test this again
                ((NpgsqlConnection)TheConnection).StateChange += AStateChangeEventHandler;
            }

            return TheConnection;
        }

        /// <summary>
        /// Initialises the connection after it was opened. Doesn't do anything with PostgreSQL!
        /// </summary>
        /// <param name="AConnection">DB Connection.</param>
        public void InitConnection(DbConnection AConnection)
        {
        }

        /// <summary>
        /// Formats an error message if the Exception is of Type 'NpgsqlException'.
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>True if this is an NpgsqlException.</returns>
        public bool LogException(Exception AException, ref string AErrorMessage)
        {
            if (AException is NpgsqlException)
            {
                for (int Counter = 0; Counter <= ((NpgsqlException)AException).Errors.Count - 1; Counter += 1)
                {
                    AErrorMessage = AErrorMessage +
                                    "Index #" + Counter.ToString() + Environment.NewLine +
                                    "Message: " + ((NpgsqlException)AException)[Counter].Message + Environment.NewLine +
                                    "Detail: " + ((NpgsqlException)AException)[Counter].Detail.ToString() + Environment.NewLine +
                                    "Where: " + ((NpgsqlException)AException)[Counter].Where + Environment.NewLine +
                                    "SQL: " + ((NpgsqlException)AException)[Counter].ErrorSql + Environment.NewLine +
                                    "Position in SQL: " + ((NpgsqlException)AException)[Counter].Position + Environment.NewLine;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Formats a SQL query so that it works for PostgreSQL.
        /// See also the comments for TDataBase.FormatQueryRDBMSSpecific.
        /// </summary>
        /// <param name="ASqlQuery">SQL Query.</param>
        /// <returns>Formatted SQL Query.</returns>
        public String FormatQueryRDBMSSpecific(String ASqlQuery)
        {
            string ReturnValue = ASqlQuery;

            ReturnValue = ReturnValue.Replace("PUB_", "public.");
            ReturnValue = ReturnValue.Replace("PUB.", "public.");
            ReturnValue = ReturnValue.Replace("pub_", "public.");
            ReturnValue = ReturnValue.Replace("pub.", "public.");
            ReturnValue = ReturnValue.Replace("\"", "'");

            // INSERT INTO table () VALUES
            ReturnValue = ReturnValue.Replace("() VALUES", " VALUES");

            Match m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");

            while (m.Success)
            {
                // needs to be 'yyyy-MM-dd'
                ReturnValue = ReturnValue.Replace("#" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "#",
                    "'" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "'");
                m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");
            }

            // PostgreSQL's 'LIKE' command is case-sensitive, but we prefer case insensitive search
            ReturnValue = ReturnValue.Replace("LIKE", "ILIKE");

            // to avoid Npgsql.NpgsqlException:
            // operator does not exist: boolean = integer
            // Hint: No operator matches the given name and argument type(s). You might need to add explicit type casts.
            ReturnValue = ReturnValue.Replace("_l = 1", "_l = true");
            ReturnValue = ReturnValue.Replace("_l = 0", "_l = false");

            // Get the correct function for DAYOFYEAR
            while (ReturnValue.Contains("DAYOFYEAR("))
            {
                ReturnValue = ReplaceDayOfYear(ReturnValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts an Array of DbParameter (eg. OdbcParameter) to an Array
        /// of NpgsqlParameter. If the Parameters don't have a name yet, they
        /// are given one because PostgreSQL needs named Parameters.
        /// <para>Furthermore, the parameter placeholders '?' in the the passed in
        /// <paramref name="ASqlStatement" /> are replaced with PostgreSQL
        /// ':paramX' placeholders (where 'paramX' is the name of the Parameter).</para>
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement. It will be converted!</param>
        /// <returns>Array of NpgsqlParameter (converted from <paramref name="AParameterArray" />.</returns>
        public DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement)
        {
            NpgsqlParameter[] ReturnValue = new NpgsqlParameter[AParameterArray.Length];
            OdbcParameter[] AParameterArrayOdbc;
            string ParamName = "";

            if (!(AParameterArray is NpgsqlParameter[]))
            {
                AParameterArrayOdbc = (OdbcParameter[])AParameterArray;

                // Parameter Type change and Parameter Name assignment
                for (int Counter = 0; Counter < AParameterArray.Length; Counter++)
                {
                    ParamName = AParameterArrayOdbc[Counter].ParameterName;

                    if (ParamName == "")
                    {
                        ParamName = "param" + Counter.ToString();
#if DEBUGMODE_TODO
                        if (FDebugLevel >= DBAccess.DB_DEBUGLEVEL_TRACE)
                        {
                            TLogging.Log("Assigned Parameter Name '" + ParamName + "' for Parameter with Value '" +
                                AParameterArrayOdbc[Counter].Value.ToString() + "'.");
                        }
#endif
                    }

                    switch (AParameterArrayOdbc[Counter].OdbcType)
                    {
                        case OdbcType.Decimal:

                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Numeric, AParameterArrayOdbc[Counter].Size);

                            break;

                        case OdbcType.VarChar:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Varchar, AParameterArrayOdbc[Counter].Size);

                            break;

                        case OdbcType.Bit:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Boolean);

                            break;

                        case OdbcType.Date:

                            if (AParameterArrayOdbc[Counter].Value != DBNull.Value)
                            {
                                DateTime TmpDate = (DateTime)AParameterArrayOdbc[Counter].Value;

                                if ((TmpDate.Hour == 0)
                                    && (TmpDate.Minute == 0)
                                    && (TmpDate.Second == 0)
                                    && (TmpDate.Millisecond == 0))
                                {
                                    ReturnValue[Counter] = new NpgsqlParameter(
                                        ParamName,
                                        NpgsqlDbType.Date);
                                }
                                else
                                {
                                    ReturnValue[Counter] = new NpgsqlParameter(
                                        ParamName,
                                        NpgsqlDbType.Timestamp);
                                }
                            }
                            else
                            {
                                ReturnValue[Counter] = new NpgsqlParameter(
                                    ParamName,
                                    NpgsqlDbType.Date);
                            }

                            break;

                        case OdbcType.DateTime:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Timestamp);

                            break;

                        case OdbcType.Int:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Integer);

                            break;

                        case OdbcType.BigInt:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Bigint);

                            break;

                        default:
                            ReturnValue[Counter] = new NpgsqlParameter(
                            ParamName,
                            NpgsqlDbType.Integer);

                            break;
                    }

                    ReturnValue[Counter].Value = AParameterArrayOdbc[Counter].Value;
                }
            }
            else
            {
                ReturnValue = (NpgsqlParameter[])AParameterArray;
            }

            if (ASqlStatement.Contains("?"))
            {
                StringBuilder SqlStatementBuilder = new StringBuilder();
                int QMarkPos = 0;
                int LastQMarkPos = -1;
                int ParamCounter = 0;

                /* SQL Syntax change from ODBC style to PostgreSQL style: Replace '?' with
                 * ':xxx' (where xxx is the name of the Parameter).
                 */
                while ((QMarkPos = ASqlStatement.IndexOf("?", QMarkPos + 1)) > 0)
                {
                    SqlStatementBuilder.Append(ASqlStatement.Substring(
                            LastQMarkPos + 1, QMarkPos - LastQMarkPos - 1));
                    SqlStatementBuilder.Append(":").Append(ReturnValue[ParamCounter].ParameterName);

                    ParamCounter++;
                    LastQMarkPos = QMarkPos;
                }

                SqlStatementBuilder.Append(ASqlStatement.Substring(
                        LastQMarkPos + 1, ASqlStatement.Length - LastQMarkPos - 1));

                // TLogging.Log("old statement: " + ASqlStatement);

                ASqlStatement = SqlStatementBuilder.ToString();

                // TLogging.Log("new statement: " + ASqlStatement);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Creates a DbCommand object.
        /// This formats the sql query for PostgreSQL, and transforms the parameters.
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns>Instantiated NpgsqlCommand.</returns>
        public DbCommand NewCommand(ref string ACommandText, DbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction)
        {
            DbCommand ObjReturn = null;

            ACommandText = FormatQueryRDBMSSpecific(ACommandText);

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Query formatted for PostgreSQL: " + ACommandText);
            }

#if WITH_POSTGRESQL_LOGGING
            // TODO?
            if (FDebugLevel >= DB_DEBUGLEVEL_TRACE)
            {
                NpgsqlEventLog.Level = LogLevel.Debug;
                NpgsqlEventLog.LogName = NPGSQL_LOGFILENAME;
                TLogging.Log("NpgsqlEventLog.LogName: " + NpgsqlEventLog.LogName);
            }
#endif
            NpgsqlParameter[] NpgsqlParametersArray = null;

            if ((AParametersArray != null)
                && (AParametersArray.Length > 0))
            {
                // Check for characters that indicate a parameter in query text
                if (ACommandText.IndexOf('?') == -1)
                {
                    throw new EDBParameterisedQueryMissingParameterPlaceholdersException(
                        "Question marks (?) must be present in query text if Parameters are passed in");
                }

                if (AParametersArray != null)
                {
                    NpgsqlParametersArray = (NpgsqlParameter[])ConvertOdbcParameters(AParametersArray, ref ACommandText);
                }
            }

            ObjReturn = new NpgsqlCommand(ACommandText, (NpgsqlConnection)AConnection);

            if (NpgsqlParametersArray != null)
            {
                // add parameters
                foreach (DbParameter param in NpgsqlParametersArray)
                {
                    ObjReturn.Parameters.Add(param);
                }
            }

            return ObjReturn;
        }

        /// <summary>
        /// Creates a DbDataAdapter for PostgreSQL.
        /// </summary>
        /// <remarks>
        /// <b>Important:</b> Since an object that derives from DbDataAdapter is returned you ought to
        /// <em>call .Dispose()</em> on the returned object to release its resouces! (DbDataAdapter inherits
        /// from DataAdapter which itself inherits from Component, which implements IDisposable!)
        /// </remarks>
        /// <returns>Instantiated NpgsqlDataAdapter.</returns>
        public DbDataAdapter NewAdapter()
        {
            DbDataAdapter TheAdapter = new NpgsqlDataAdapter();

            return TheAdapter;
        }

        /// <summary>
        /// Fills a DbDataAdapter that was created with the <see cref="NewAdapter" /> Method.
        /// </summary>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataSet"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        /// <param name="ADataTableName"></param>
        public void FillAdapter(DbDataAdapter TheAdapter,
            ref DataSet AFillDataSet,
            Int32 AStartRecord,
            Int32 AMaxRecords,
            string ADataTableName)
        {
            if ((AStartRecord > 0) && (AMaxRecords > 0))
            {
                ((NpgsqlDataAdapter)TheAdapter).Fill(AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
            }
            else
            {
                if (ADataTableName == String.Empty)
                {
                    ((NpgsqlDataAdapter)TheAdapter).Fill(AFillDataSet);
                }
                else
                {
                    ((NpgsqlDataAdapter)TheAdapter).Fill(AFillDataSet, ADataTableName);
                }
            }
        }

        /// <summary>
        /// Fills a DbDataAdapter that was created with the <see cref="NewAdapter" /> Method.
        /// </summary>
        /// <remarks>Overload of FillAdapter, just for one table.</remarks>
        /// <param name="TheAdapter"></param>
        /// <param name="AFillDataTable"></param>
        /// <param name="AStartRecord"></param>
        /// <param name="AMaxRecords"></param>
        public void FillAdapter(DbDataAdapter TheAdapter,
            ref DataTable AFillDataTable,
            Int32 AStartRecord,
            Int32 AMaxRecords)
        {
            if ((AStartRecord > 0) && (AMaxRecords > 0))
            {
                ((NpgsqlDataAdapter)TheAdapter).Fill(AStartRecord, AMaxRecords, AFillDataTable);
            }
            else
            {
                ((NpgsqlDataAdapter)TheAdapter).Fill(AFillDataTable);
            }
        }

        /// <summary>
        /// Some RDMBS's have some problems with certain Isolation Levels - not so PostgreSQL.
        /// </summary>
        /// <param name="AIsolationLevel">Isolation Level.</param>
        /// <returns>True if Isolation Level was modified.</returns>
        public bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel)
        {
            // All isolation levels work fine for PostgreSQL (though internally the Npgsql driver only
            // supports ReadCommited and Serializable [see Npgsql driver source code: Method
            // 'internal NpgsqlTransaction(NpgsqlConnection conn, IsolationLevel isolation)' in NpgsqlTransaction.cs]!)
            return false;
        }

        /// <summary>
        /// Returns the next sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">Database object that can be used for querying.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase)
        {
            // TODO problem: sequence should be committed? separate transaction?
            // see also http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=44
            // or use locking? see also http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=50
            return Convert.ToInt64(ADatabase.ExecuteScalar("SELECT NEXTVAL('" + ASequenceName + "')", ATransaction));
        }

        /// <summary>
        /// Returns the current sequence value for the given Sequence from the DB.
        /// </summary>
        /// <param name="ASequenceName">Name of the Sequence.</param>
        /// <param name="ATransaction">An instantiated Transaction in which the Query
        /// to the DB will be enlisted.</param>
        /// <param name="ADatabase">Database object that can be used for querying.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetCurrentSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase)
        {
            return Convert.ToInt64(ADatabase.ExecuteScalar("SELECT last_value FROM " + ASequenceName + "", ATransaction));
        }

        /// <summary>
        /// Restart a sequence with the given value.
        /// </summary>
        public void RestartSequence(String ASequenceName,
            TDBTransaction ATransaction,
            TDataBase ADatabase,
            Int64 ARestartValue)
        {
            ADatabase.ExecuteScalar(
                "SELECT pg_catalog.setval('" + ASequenceName + "', " + ARestartValue.ToString() + ", true);", ATransaction);
        }

        /// <summary>
        /// Replace DAYOFYEAR(p_param) with to_char(p_param, 'DDD').
        /// Replace DAYOFYEAR('2010-01-30') with to_char(to_date('2010-01-30', 'YYYY-MM-DD'), 'DDD').
        /// </summary>
        /// <param name="ASqlCommand"></param>
        /// <returns></returns>
        private String ReplaceDayOfYear(String ASqlCommand)
        {
            int StartIndex = ASqlCommand.IndexOf("DAYOFYEAR(");
            int EndBracketIndex = ASqlCommand.IndexOf(')', StartIndex + 10);

            if ((StartIndex < 0) || (EndBracketIndex < 0))
            {
                TLogging.Log("Cant convert DAYOFYEAR() function to PostgreSQL to_char() function with this sql command:");
                TLogging.Log(ASqlCommand);
                return ASqlCommand;
            }

            String ReplacedDate = "";

            if ((ASqlCommand.Length >= StartIndex + 22)
                && (ASqlCommand[StartIndex + 10] == '\'')
                && (ASqlCommand[StartIndex + 21] == '\''))
            {
                // We have a date string
                ReplacedDate = "to_date(" + ASqlCommand.Substring(StartIndex + 10, 12) +
                               ", 'YYYY-MM-DD')";
            }
            else
            {
                int ParameterLength = EndBracketIndex - StartIndex - 10;

                ReplacedDate = ASqlCommand.Substring(StartIndex + 10, ParameterLength);
            }

            ReplacedDate = ReplacedDate + ", 'DDD'";

            return ASqlCommand.Substring(0, StartIndex) + "to_char(" + ReplacedDate + ASqlCommand.Substring(EndBracketIndex);
        }

        /// Updating of a Postgresql database has not been implemented yet, need to do this still manually
        public void UpdateDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion,
            string AHostOrFile, string ADatabasePort, string ADatabaseName, string AUsername, string APassword)
        {
            throw new EDBUnsupportedDBUpgradeException(
                "Cannot connect to old database, please restore the latest clean demo database or run nant patchDatabase");
        }
    }
}