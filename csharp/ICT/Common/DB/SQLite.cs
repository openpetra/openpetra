//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using Mono.Data.Sqlite;
using System.Collections;
using Ict.Common;
using Ict.Common.IO;
using System.Text.RegularExpressions;

namespace Ict.Common.DB
{
    /// <summary>
    /// this class allows access to SQLite databases on Windows and Linux, using Mono.Data.Sqlite
    /// </summary>
    public class TSQLite : IDataBaseRDBMS
    {
        /// <summary>
        /// Create a SQLite connection using Mono.Data.Sqlite.
        /// this works on Windows (with the sqlite3.dll) and on Linux
        /// </summary>
        /// <param name="AServer">The Database file</param>
        /// <param name="APort">the port that the db server is running on</param>
        /// <param name="ADatabaseName">not in use</param>
        /// <param name="AUsername">not in use</param>
        /// <param name="APassword">The password for opening the database</param>
        /// <param name="AConnectionString">not in use</param>
        /// <param name="AStateChangeEventHandler">for connection state changes</param>
        /// <returns>the connection</returns>
        public IDbConnection GetConnection(String AServer, String APort,
            String ADatabaseName,
            String AUsername, ref String APassword,
            ref String AConnectionString,
            StateChangeEventHandler AStateChangeEventHandler)
        {
            ArrayList ExceptionList;

            SqliteConnection TheConnection = null;

            if (!File.Exists(AServer))
            {
                // on Windows, we cannot store the database in userappdata during installation, because
                // the setup has to be run as administrator.
                // therefore the first time the user starts Petra, we need to prepare his environment
                // see also http://www.vincenzo.net/isxkb/index.php?title=Vista_considerations#Best_Practices

                // copy the base database
                string baseDatabase = TAppSettingsManager.GetValue("Server.SQLiteBaseFile");

                if (!Directory.Exists(Path.GetDirectoryName(AServer)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AServer));
                }

                File.Copy(baseDatabase, AServer);
            }

            if (AConnectionString == "")
            {
                AConnectionString = "Data Source=" + Path.GetFullPath(AServer);

                // sqlite on Windows does not support encryption with a password
                // System.EntryPointNotFoundException: sqlite3_key
                APassword = string.Empty;

                if (APassword.Length > 0)
                {
                    AConnectionString += ";Password=";
                }
            }

            if (new TFileVersionInfo(SqliteConnection.SQLiteVersion).Compare(new TFileVersionInfo("3.7.11")) < 0)
            {
                // for insert statements with multiple rows. see http://www.sqlite.org/releaselog/3_7_11.html
                TLogging.Log("OpenPetra requires SQLite >= 3.7.11, but current version is " + SqliteConnection.SQLiteVersion);
                return null;
            }

            try
            {
                // Now try to connect to the DB
                TheConnection = new SqliteConnection(AConnectionString + APassword);
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
                TheConnection.StateChange += AStateChangeEventHandler;
            }

            return TheConnection;
        }

        /// <summary>
        /// enforce foreign key constraints
        /// </summary>
        public void InitConnection(IDbConnection AConnection)
        {
            string enforceForeignKeyConstraints = "PRAGMA foreign_keys = ON;";

            using (SqliteCommand cmd = new SqliteCommand(enforceForeignKeyConstraints, (SqliteConnection)AConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// format an error message for exception of type SQLiteException
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="AErrorMessage"></param>
        /// <returns>true if this is an SQLiteException</returns>
        public bool LogException(Exception AException, ref string AErrorMessage)
        {
            if (AException is SqliteException)
            {
                AErrorMessage = AErrorMessage + ((SqliteException)AException).ErrorCode.ToString() +
                                Environment.NewLine;
                return true;
            }

            return false;
        }

        /// <summary>
        /// format the sql query so that it works for SQLite
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
            ReturnValue = ReturnValue.Replace("public.", "");
            ReturnValue = ReturnValue.Replace("\"", "'");

            ReturnValue = ReturnValue.Replace("NOW()", "datetime('now')");

            ReturnValue = ReturnValue.Replace("= false", "= 0");
            ReturnValue = ReturnValue.Replace("= true", "= 1");
            ReturnValue = ReturnValue.Replace("=false", "=0");
            ReturnValue = ReturnValue.Replace("=true", "=1");
            ReturnValue = ReturnValue.Replace("=FALSE", "=0");
            ReturnValue = ReturnValue.Replace("=TRUE", "=1");
            ReturnValue = ReturnValue.Replace("= FALSE", "= 0");
            ReturnValue = ReturnValue.Replace("= TRUE", "= 1");
            ReturnValue = ReturnValue.Replace(" as ", " AS ");
            ReturnValue = ReturnValue.Replace("true AS ", "1 AS ");
            ReturnValue = ReturnValue.Replace("false AS ", "0 AS ");
            ReturnValue = ReturnValue.Replace("TRUE AS ", "1 AS ");
            ReturnValue = ReturnValue.Replace("FALSE AS ", "0 AS ");

            // INSERT INTO table () VALUES
            ReturnValue = ReturnValue.Replace("() VALUES", " VALUES");

            Match m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");

            while (m.Success)
            {
                // needs to be 'yyyy-MM-dd 00:00:00'
                // added 00:00:00 time to fix issue https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=11
                ReturnValue = ReturnValue.Replace("#" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + "#",
                    "'" + m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3] + " 00:00:00'");
                m = Regex.Match(ReturnValue, "#([0-9][0-9][0-9][0-9])-([0-9][0-9])-([0-9][0-9])#");
            }

            // LIKE of sqlite is always case insensitive, so no modification needed (eg ILIKE for postgresql)

            // Get the correct function for DAYOFYEAR
            while (ReturnValue.Contains("DAYOFYEAR("))
            {
                ReturnValue = ReplaceDayOfYear(ReturnValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts an Array of DbParameter (eg. OdbcParameter) to an Array
        /// of SQLiteParameter.
        /// </summary>
        /// <param name="AParameterArray">Array of DbParameter that is to be converted.</param>
        /// <param name="ASqlStatement">SQL Statement will stay the same.</param>
        /// <returns>Array of SQLiteParameter (converted from <paramref name="AParameterArray" />.</returns>
        public DbParameter[] ConvertOdbcParameters(DbParameter[] AParameterArray, ref string ASqlStatement)
        {
            SqliteParameter[] ReturnValue = new SqliteParameter[AParameterArray.Length];
            OdbcParameter[] AParameterArrayOdbc;

            if (!(AParameterArray is SqliteParameter[]))
            {
                AParameterArrayOdbc = (OdbcParameter[])AParameterArray;

                for (int Counter = 0; Counter < AParameterArray.Length; Counter++)
                {
                    ReturnValue[Counter] = new SqliteParameter();
                    ReturnValue[Counter].Value = AParameterArrayOdbc[Counter].Value;
                }
            }
            else
            {
                ReturnValue = (SqliteParameter[])AParameterArray;
            }

            return ReturnValue;
        }

        /// <summary>
        /// create a IDbCommand object
        /// this formats the sql query for SQLite, and transforms the parameters
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
                TLogging.Log("Query formatted for SQLite: " + ACommandText);
            }

            SqliteParameter[] SQLiteParametersArray = null;

            if ((AParametersArray != null)
                && (AParametersArray.Length > 0))
            {
                if (AParametersArray != null)
                {
                    SQLiteParametersArray = (SqliteParameter[])ConvertOdbcParameters(AParametersArray, ref ACommandText);
                }
            }

            ObjReturn = ((SqliteConnection)AConnection).CreateCommand();
            ((SqliteCommand)ObjReturn).CommandText = ACommandText;

            if (SQLiteParametersArray != null)
            {
                // add parameters
                foreach (DbParameter param in SQLiteParametersArray)
                {
                    ObjReturn.Parameters.Add(param);
                }
            }

            return ObjReturn;
        }

        /// <summary>
        /// create an IDbDataAdapter for SQLite
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter NewAdapter()
        {
            IDbDataAdapter TheAdapter = new SqliteDataAdapter();

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
            ((SqliteDataAdapter)TheAdapter).Fill(AFillDataSet, AStartRecord, AMaxRecords, ADataTableName);

            TheAdapter.SelectCommand.Dispose();
            ((SqliteDataAdapter)TheAdapter).Dispose();
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
            ((SqliteDataAdapter)TheAdapter).Fill(AFillDataTable);

            TheAdapter.SelectCommand.Dispose();
            ((SqliteDataAdapter)TheAdapter).Dispose();
        }

        /// <summary>
        /// some databases have some problems with certain Isolation levels
        /// </summary>
        /// <param name="AIsolationLevel"></param>
        /// <returns>true if isolation level was modified</returns>
        public bool AdjustIsolationLevel(ref IsolationLevel AIsolationLevel)
        {
            // somehow there is a problem with RepeatableRead
            if ((AIsolationLevel == IsolationLevel.RepeatableRead) || (AIsolationLevel == IsolationLevel.ReadUncommitted))
            {
                AIsolationLevel = IsolationLevel.ReadCommitted;
                return true;
            }

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

            using (SqliteCommand cmd = new SqliteCommand(stmt, (SqliteConnection)AConnection))
            {
                cmd.ExecuteNonQuery();
                return GetCurrentSequenceValue(ASequenceName, ATransaction, ADatabase, AConnection);
            }
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

            using (SqliteCommand cmd = new SqliteCommand(stmt, (SqliteConnection)AConnection))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
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
            ADatabase.ExecuteNonQuery("DELETE FROM " + ASequenceName + ";", ATransaction);
            ADatabase.ExecuteNonQuery("INSERT INTO " + ASequenceName + " VALUES(" + ARestartValue.ToString() + ", -1);", ATransaction);
        }

        /// <summary>
        /// Replace DAYOFYEAR(p_param) with strftime(%j, p_param)
        /// </summary>
        /// <param name="ASqlCommand"></param>
        /// <returns></returns>
        private String ReplaceDayOfYear(String ASqlCommand)
        {
            int StartIndex = ASqlCommand.IndexOf("DAYOFYEAR(");

            if (StartIndex < 0)
            {
                TLogging.Log("Cant convert DAYOFYEAR() function to SQLite strftime() function with this sql command:");
                TLogging.Log(ASqlCommand);
                return ASqlCommand;
            }

            return ASqlCommand.Substring(0, StartIndex) + "strftime(%j, " +
                   ASqlCommand.Substring(StartIndex + 10);
        }

        /// <summary>
        /// For standalone installations, we update the SQLite database on the fly
        /// </summary>
        public void UpdateDatabase(TFileVersionInfo ADBVersion, TFileVersionInfo AExeVersion,
            string AHostOrFile, string ADatabasePort, string ADatabaseName, string AUsername, string APassword)
        {
            // we do not support updating standalone databases at the moment
            if (AExeVersion.FileMajorPart == 0)
            {
                DBAccess.GDBAccessObj.CloseDBConnection();
                throw new Exception(String.Format("Unsupported upgrade: Please rename your file {0} so that we can start with a fresh database!",
                        AHostOrFile));
            }

            string dbpatchfilePath = Path.GetDirectoryName(TAppSettingsManager.GetValue("Server.SQLiteBaseFile"));

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            ADBVersion.FilePrivatePart = 0;
            AExeVersion.FilePrivatePart = 0;

            try
            {
                // run all available patches. for each release there could be a patch file
                string[] sqlFiles = Directory.GetFiles(dbpatchfilePath, "*.sql");

                bool foundUpdate = true;

                // run through all sql files until we have no matching update files anymore
                while (foundUpdate)
                {
                    foundUpdate = false;

                    foreach (string sqlFile in sqlFiles)
                    {
                        if (!sqlFile.EndsWith("pg.sql") && (new TPatchFileVersionInfo(ADBVersion)).PatchApplies(sqlFile, AExeVersion))
                        {
                            foundUpdate = true;
                            StreamReader sr = new StreamReader(sqlFile);

                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine().Trim();

                                if (!line.StartsWith("--"))
                                {
                                    DBAccess.GDBAccessObj.ExecuteNonQuery(line, transaction);
                                }
                            }

                            sr.Close();
                            ADBVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(sqlFile);
                        }
                    }
                }

                if (ADBVersion.Compare(AExeVersion) == 0)
                {
                    // if patches have been applied successfully, update the database version
                    string newVersionSql =
                        String.Format("UPDATE s_system_defaults SET s_default_value_c = '{0}' WHERE s_default_code_c = 'CurrentDatabaseVersion';",
                            AExeVersion.ToStringDotsHyphen());
                    DBAccess.GDBAccessObj.ExecuteNonQuery(newVersionSql, transaction);
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw new Exception(String.Format("Cannot connect to old database (version {0}), there are some missing sql patch files",
                            ADBVersion));
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }
        }
    }
}