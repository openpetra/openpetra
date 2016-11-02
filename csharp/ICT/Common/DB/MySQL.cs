//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.Text.RegularExpressions;

using MySql.Data.MySqlClient;

using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB
{
    /// <summary>
    /// Allows access to MySQL databases using the 'MySQL AB ADO.Net Driver for MySQL' .NET Data Provider.
    /// </summary>
    public class TMySQL : IDataBaseRDBMS
    {
        /// <summary>
        /// Creates a MySqlConnection connection using the 'MySQL AB ADO.Net Driver for MySQL' .NET Data Provider.
        /// </summary>
        /// <param name="AServer">Database Server.</param>
        /// <param name="APort">Port that the DB server is running on.</param>
        /// <param name="ADatabaseName">Name of the database that we want to connect to.</param>
        /// <param name="AUsername">Username for opening the MySQL connection.</param>
        /// <param name="APassword">Password for opening the MySQL connection.</param>
        /// <param name="AConnectionString">Connection string; if it is not empty, it will
        /// overrule the previous parameters.</param>
        /// <param name="AStateChangeEventHandler">Event Handler for connection state changes.</param>
        /// <returns>
        /// Instantiated MySqlConnection, but not opened yet (null if connection could not be established).
        /// </returns>
        public DbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            MySqlConnection TheConnection = null;

            if (String.IsNullOrEmpty(AConnectionString))
            {
                if (AUsername == "")
                {
                    throw new ArgumentException("AUsername", "AUsername must not be null or an empty string!");
                }

                AConnectionString = "SERVER=" + AServer + ";" + "DATABASE=" + ADatabaseName + ";" + "UID=" + AUsername + ";" + "PASSWORD=";
            }

            try
            {
                TheConnection = new MySqlConnection(AConnectionString + APassword + ";");
            }
            catch (Exception exp)
            {
                ArrayList ExceptionList = new ArrayList();
                ExceptionList.Add((("Error establishing a DB connection to: " + AConnectionString) + Environment.NewLine));
                ExceptionList.Add((("Exception thrown :- " + exp.ToString()) + Environment.NewLine));
                TLogging.Log(ExceptionList, true);
            }

            if (TheConnection != null)
            {
                // TODO: need to test this
                TheConnection.StateChange += AStateChangeEventHandler;
            }

            return TheConnection;
        }

        /// <summary>
        /// Initialises the connection after it was opened. Doesn't do anything with MySQL!
        /// </summary>
        /// <param name="AConnection">DB Connection.</param>
        public void InitConnection(DbConnection AConnection)
        {
        }

        /// <summary>
        /// Formats an error message if the Exception is of Type 'MySqlException'.
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>True if this is an MySqlException.</returns>
        public bool LogException(Exception AException, ref string AErrorMessage)
        {
            if (AException is MySqlException)
            {
                MySqlException exp = (MySqlException)AException;

                AErrorMessage = AErrorMessage +
                                "Message: " + exp.Message + Environment.NewLine +
                                "MySQL Error Number: " + exp.Number.ToString() + Environment.NewLine;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Formats a SQL query so that it works for MySQL.
        /// See also the comments for TDataBase.FormatQueryRDBMSSpecific.
        /// </summary>
        /// <param name="ASqlQuery">SQL Query.</param>
        /// <returns>Formatted SQL Query.</returns>
        public String FormatQueryRDBMSSpecific(String ASqlQuery)
        {
            string ReturnValue = ASqlQuery;

            ReturnValue = ReturnValue.Replace("PUB_", "");
            ReturnValue = ReturnValue.Replace("PUB.", "");
            ReturnValue = ReturnValue.Replace("pub_", "");
            ReturnValue = ReturnValue.Replace("pub.", "");
            ReturnValue = ReturnValue.Replace("public.", "");

            // replacing the quotes would give trouble with importing initial database, with LOAD FROM
            //ReturnValue = ReturnValue.Replace("\"", "'");

            Match m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");

            while (m.Success)
            {
                // needs to be 'yyyy-MM-dd'
                ReturnValue = ReturnValue.Replace("#" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "#",
                    "'" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "'");
                m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");
            }

            ReturnValue = ReturnValue.Replace("= false", "= 0");
            ReturnValue = ReturnValue.Replace("= true", "= 1");
            ReturnValue = ReturnValue.Replace("=false", "=0");
            ReturnValue = ReturnValue.Replace("=true", "=1");
            ReturnValue = ReturnValue.Replace(" as ", " AS ");
            ReturnValue = ReturnValue.Replace("true AS ", "1 AS ");
            ReturnValue = ReturnValue.Replace("false AS ", "0 AS ");
            ReturnValue = ReturnValue.Replace("SUM (", "SUM(");

            if (ReturnValue.Contains(" AS usage"))
            {
                // it seems that usage is a keyword and cannot be used as an alias
                // this is used in csharp/ICT/Petra/Server/lib/MFinance/Common/Common.CrossLedger.cs
                ReturnValue = ReturnValue.Replace(" AS usage", " AS usage_");
                ReturnValue = ReturnValue.Replace("usage.", "usage_.");
            }

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
        /// are given one because MySQL needs named Parameters.
        /// <para>Furthermore, the parameter placeholders '?' in the the passed in
        /// <paramref name="ASqlStatement" /> are replaced with MySQL
        /// '?paramX' placeholders (where 'paramX' is the name of the Parameter).</para>
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement. It will be converted!</param>
        /// <returns>Array of MysqlParameter (converted from <paramref name="AParameterArray" />.</returns>
        public DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement)
        {
            MySqlParameter[] ReturnValue = new MySqlParameter[AParameterArray.Length];
            OdbcParameter[] AParameterArrayOdbc;
            string ParamName = "";

            if (!(AParameterArray is MySqlParameter[]))
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

                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.Decimal, AParameterArrayOdbc[Counter].Size);

                            break;

                        case OdbcType.VarChar:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.String, AParameterArrayOdbc[Counter].Size);

                            break;

                        case OdbcType.Bit:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.Bit);

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
                                    ReturnValue[Counter] = new MySqlParameter(
                                        ParamName,
                                        MySqlDbType.Date);
                                }
                                else
                                {
                                    ReturnValue[Counter] = new MySqlParameter(
                                        ParamName,
                                        MySqlDbType.Timestamp);
                                }
                            }
                            else
                            {
                                ReturnValue[Counter] = new MySqlParameter(
                                    ParamName,
                                    MySqlDbType.Date);
                            }

                            break;

                        case OdbcType.DateTime:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.DateTime);

                            break;

                        case OdbcType.Int:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.Int32);

                            break;

                        case OdbcType.BigInt:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.Int64);

                            break;

                        default:
                            ReturnValue[Counter] = new MySqlParameter(
                            ParamName,
                            MySqlDbType.Int32);

                            break;
                    }

                    ReturnValue[Counter].Value = AParameterArrayOdbc[Counter].Value;
                }
            }
            else
            {
                ReturnValue = (MySqlParameter[])AParameterArray;
            }

            if (ASqlStatement.Contains("?"))
            {
                string SQLSelectBeforeQMark;
                string SQLSelectAfterQMark;
                string SQLSelectStatementRepl = "";
                int QMarkPos = 0;
                int LastQMarkPos = 0;
                int NextQMarkPos = 0;
                int ParamCounter = 0;

                /* SQL Syntax change from ODBC style to MySQL style: Replace '?' with
                 * '?xxx' (where xxx is the name of the Parameter).
                 */
                while (ASqlStatement.IndexOf("?", QMarkPos + 1) > 0)
                {
                    QMarkPos = ASqlStatement.IndexOf("?", QMarkPos + 1);

                    ////                        TLogging.Log("QMarkPos: " + QMarkPos.ToString() +
                    ////                            "; ParamCounter: " + ParamCounter.ToString() +
                    ////                            "; LastQMarkPos: " + LastQMarkPos.ToString());

                    if (ParamCounter > 0)
                    {
                        SQLSelectBeforeQMark = ASqlStatement.Substring(
                            LastQMarkPos + 1, QMarkPos - LastQMarkPos - 1);
                    }
                    else
                    {
                        SQLSelectBeforeQMark = ASqlStatement.Substring(
                            0, QMarkPos);
                    }

                    NextQMarkPos = ASqlStatement.IndexOf("?", QMarkPos + 1);

                    if (NextQMarkPos > 0)
                    {
                        SQLSelectAfterQMark = "";
                    }
                    else
                    {
                        SQLSelectAfterQMark = ASqlStatement.Substring(
                            QMarkPos + 1, ASqlStatement.Length - QMarkPos - 1);
                    }

                    SQLSelectStatementRepl = SQLSelectStatementRepl +
                                             SQLSelectBeforeQMark +
                                             "?" + ReturnValue[ParamCounter].ParameterName +
                                             SQLSelectAfterQMark;

                    LastQMarkPos = QMarkPos;

                    ////                        TLogging.Log("QMarkPos: " + QMarkPos.ToString() +
                    ////                            "; ParamCounter: " + ParamCounter.ToString() +
                    ////                            "; LastQMarkPos: " + LastQMarkPos.ToString() + Environment.NewLine +
                    ////                            "SQLSelectBeforeQMark: " + SQLSelectBeforeQMark + Environment.NewLine +
                    ////                            "SQLSelectAfterQMark: " + SQLSelectAfterQMark + Environment.NewLine +
                    ////                            "SQLSelectStatementRepl: " + SQLSelectStatementRepl + Environment.NewLine);

                    ParamCounter++;
                }

                ASqlStatement = SQLSelectStatementRepl;

                //                TLogging.Log("new statement: " + SQLSelectStatementRepl);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Creates a DbCommand object.
        /// This formats the sql query for MySQL, and transforms the parameters.
        /// </summary>
        /// <param name="ACommandText"></param>
        /// <param name="AConnection"></param>
        /// <param name="AParametersArray"></param>
        /// <param name="ATransaction"></param>
        /// <returns>Instantiated MySqlCommand.</returns>
        public DbCommand NewCommand(ref string ACommandText, DbConnection AConnection, DbParameter[] AParametersArray, TDBTransaction ATransaction)
        {
            DbCommand ObjReturn = null;

            ACommandText = FormatQueryRDBMSSpecific(ACommandText);

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Query formatted for MySQL: " + ACommandText);
            }

            MySqlParameter[] MySQLParametersArray = null;

            if ((AParametersArray != null)
                && (AParametersArray.Length > 0))
            {
                // Check for characters that indicate a parameter in query text
                if (ACommandText.IndexOf('?') == -1)
                {
                    foreach (DbParameter param in AParametersArray)
                    {
                        if (string.IsNullOrEmpty(param.ParameterName))
                        {
                            throw new EDBParameterisedQueryMissingParameterPlaceholdersException(
                                "Question marks (?) must be present in query text if nameless Parameters are passed in");
                        }
                    }
                }

                if (AParametersArray != null)
                {
                    MySQLParametersArray = (MySqlParameter[])ConvertOdbcParameters(AParametersArray, ref ACommandText);
                }
            }

            ObjReturn = new MySqlCommand(ACommandText, (MySqlConnection)AConnection);

            if (MySQLParametersArray != null)
            {
                // add parameters
                foreach (DbParameter param in MySQLParametersArray)
                {
                    ObjReturn.Parameters.Add(param);
                }
            }

            return ObjReturn;
        }

        /// <summary>
        /// Creates a DbDataAdapter for MySQL.
        /// </summary>
        /// <remarks>
        /// <b>Important:</b> Since an object that derives from DbDataAdapter is returned you ought to
        /// <em>call .Dispose()</em> on the returned object to release its resouces! (DbDataAdapter inherits
        /// from DataAdapter which itself inherits from Component, which implements IDisposable!)
        /// </remarks>
        /// <returns>Instantiated MySqlDataAdapter.</returns>
        public DbDataAdapter NewAdapter()
        {
            DbDataAdapter TheAdapter = new MySqlDataAdapter();

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
            ((MySqlDataAdapter)TheAdapter).Fill(AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
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
            ((MySqlDataAdapter)TheAdapter).Fill(AStartRecord, AMaxRecords, AFillDataTable);
        }

        /// <summary>
        /// Some RDMBS's have some problems with certain Isolation Levels - not so MySQL.
        /// </summary>
        /// <param name="AIsolationLevel">Isolation Level.</param>
        /// <returns>True if Isolation Level was modified.</returns>
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
        /// <param name="ADatabase">Database object that can be used for querying.</param>
        /// <returns>Sequence Value.</returns>
        public System.Int64 GetNextSequenceValue(String ASequenceName, TDBTransaction ATransaction, TDataBase ADatabase)
        {
            string stmt = "INSERT INTO " + ASequenceName + " VALUES(NULL, -1);";

            using (MySqlCommand cmd = new MySqlCommand(stmt, (MySqlConnection)ATransaction.Connection))
            {
                cmd.ExecuteNonQuery();
            }

            return GetCurrentSequenceValue(ASequenceName, ATransaction, ADatabase);
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
            string stmt = "SELECT MAX(sequence) FROM " + ASequenceName + ";";

            using (MySqlCommand cmd = new MySqlCommand(stmt, (MySqlConnection)ATransaction.Connection))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Restart a sequence with the given value.
        /// </summary>
        public void RestartSequence(String ASequenceName,
            TDBTransaction ATransaction,
            TDataBase ADatabase,
            Int64 ARestartValue)
        {
            ADatabase.ExecuteNonQuery("DELETE FROM " + ASequenceName + ";", ATransaction);
            ADatabase.ExecuteNonQuery("INSERT INTO " + ASequenceName + " VALUES(" + ARestartValue.ToString() + ", -1);", ATransaction);
        }

        /// <summary>
        /// Replace DAYOFYEAR(p_param) with DATE_FORMAT(p_param, %j).
        /// </summary>
        /// <param name="ASqlCommand"></param>
        /// <returns></returns>
        private String ReplaceDayOfYear(String ASqlCommand)
        {
            int StartIndex = ASqlCommand.IndexOf("DAYOFYEAR(");
            int EndBracketIndex = ASqlCommand.IndexOf(')', StartIndex + 10);

            if ((StartIndex < 0) || (EndBracketIndex < 0))
            {
                TLogging.Log("Cant convert DAYOFYEAR() function to MySQL DATE_FORMAT() function with this sql command:");
                TLogging.Log(ASqlCommand);
                return ASqlCommand;
            }

            int ParameterLength = EndBracketIndex - StartIndex - 10;

            String ReplacedDate = ASqlCommand.Substring(StartIndex + 10, ParameterLength) + ", %j";

            return ASqlCommand.Substring(0, StartIndex) + "DATE_FORMAT(" + ReplacedDate + ASqlCommand.Substring(EndBracketIndex);
        }

        /// Updating of a MySQL database has not been implemented yet, need to do this still manually
        public void UpdateDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion,
            string AHostOrFile, string ADatabasePort, string ADatabaseName, string AUsername, string APassword)
        {
            throw new EDBUnsupportedDBUpgradeException(
                "Cannot connect to old database, please restore the latest clean demo database or run nant patchDatabase");
        }

        /// <summary>
        /// Clearing of all Connection Pools is not yet implemented for MySQL...!
        /// </summary>
        public void ClearAllConnectionPools()
        {
            // We don't do anything here: Clearing of all Connection Pools is not yet implemented for MySQL...!
        }

        /// <summary>
        /// Clearing of a Connection Pool is not yet implemented for MySQL...!
        /// </summary>
        public void ClearConnectionPool(DbConnection ADBConnection)
        {
            // We don't do anything here: Clearing of a Connection Pool is not yet implemented for MySQL...!
        }
    }
}
