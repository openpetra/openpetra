//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Net;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Collections;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

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
            if (AConnectionString == "")
            {
                AConnectionString = "SERVER=" + AServer + ";" + "DATABASE=" + ADatabaseName + ";" + "UID=" + AUsername + ";" + "PASSWORD=";
            }

            MySqlConnection TheConnection = null;

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
        /// format an error message if the exception is of type MySqlException
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>true if this is an NpgsqlException</returns>
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
        /// format the sql query so that it works for MySQL
        /// see also the comments for TDataBase.FormatQueryRDBMSSpecific
        /// </summary>
        /// <param name="ASqlQuery"></param>
        /// <returns></returns>
        public String FormatQueryRDBMSSpecific(String ASqlQuery)
        {
            string ReturnValue = ASqlQuery;

            ReturnValue = ReturnValue.Replace("PUB_", "");
            ReturnValue = ReturnValue.Replace("PUB.", "");
            ReturnValue = ReturnValue.Replace("pub_", "");
            ReturnValue = ReturnValue.Replace("pub.", "");

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

            // Get the correct function for DAYOFYEAR
            while (ReturnValue.Contains("DAYOFYEAR("))
            {
                ReturnValue = ReplaceDayOfYear(ReturnValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// TODOComment
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement that is to be converted.</param>
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
            IDbCommand ObjReturn = null;

            ACommandText = FormatQueryRDBMSSpecific(ACommandText);

            if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRACE)
            {
                TLogging.Log("Query formatted for MySQL: " + ACommandText);
            }

            MySqlParameter[] MySQLParametersArray = null;

            if ((AParametersArray != null)
                && (AParametersArray.Length > 0))
            {
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
        /// create an IDbDataAdapter for MySQL
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter NewAdapter()
        {
            IDbDataAdapter TheAdapter = new MySqlDataAdapter();

            return TheAdapter;
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
            ((MySqlDataAdapter)TheAdapter).Fill(AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);
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
            ((MySqlDataAdapter)TheAdapter).Fill(AFillDataTable);
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
            string stmt = "INSERT INTO " + ASequenceName + " VALUES(NULL, -1);";
            MySqlCommand cmd = new MySqlCommand(stmt, (MySqlConnection)AConnection);

            cmd.ExecuteNonQuery();
            return GetCurrentSequenceValue(ASequenceName, ATransaction, ADatabase, AConnection);
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
            string stmt = "SELECT MAX(sequence) FROM " + ASequenceName + ";";
            MySqlCommand cmd = new MySqlCommand(stmt, (MySqlConnection)AConnection);

            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        /// <summary>
        /// restart a sequence with the given value
        /// </summary>
        public void RestartSequence(String ASequenceName,
            TDBTransaction ATransaction,
            TDataBase ADatabase,
            IDbConnection AConnection,
            Int64 ARestartValue)
        {
            ADatabase.ExecuteNonQuery("DELETE FROM " + ASequenceName + ";", ATransaction, false);
            ADatabase.ExecuteNonQuery("INSERT INTO " + ASequenceName + " VALUES(" + ARestartValue.ToString() + ", -1);", ATransaction, false);
        }

        /// <summary>
        /// Replace DAYOFYEAR(p_param) with DATE_FORMAT(p_param, %j)
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
            throw new Exception(
                "Cannot connect to old database, please restore the latest clean demo database or run nant patchDatabase");
        }
    }
}