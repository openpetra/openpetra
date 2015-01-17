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
using System.Collections.Specialized;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using Ict.Tools.DBXML;
using Ict.Common;

namespace GenerateSQL
{
    /// <summary>
    /// this class will create a Petra database with Sqlite
    /// and will allow to load data from CSV files that are in Postgresql COPY format
    /// </summary>
    public class TSQLiteWriter
    {
        /// <summary>
        /// create an Sqlite database and create all tables and sequences and indexes
        /// </summary>
        /// <param name="ADataDefinition"></param>
        /// <param name="ADBFilename"></param>
        /// <param name="ADBPwd"></param>
        /// <returns></returns>
        static public bool CreateDatabase(TDataDefinitionStore ADataDefinition, string ADBFilename, string ADBPwd)
        {
            if (System.IO.File.Exists(ADBFilename))
            {
                throw new Exception("cannot overwrite existing file " + ADBFilename);
            }

            System.Console.WriteLine("Writing file to {0}...", ADBFilename);

            // see also tutorial http://Sqlite.phxsoftware.com/forums/p/130/452.aspx#452

            // sqlite on Windows does not support encryption with a password
            // System.EntryPointNotFoundException: sqlite3_key
            ADBPwd = string.Empty;

            SqliteConnection conn = new SqliteConnection("Data Source=" + ADBFilename + (ADBPwd.Length > 0 ? ";Password=" + ADBPwd : ""));
            conn.Open();

            foreach (TTable table in ADataDefinition.GetTables())
            {
                // see http://www.Sqlite.org/lang_createtable.html
                string createStmt = "CREATE TABLE " + table.strName + " (";
                bool firstField = true;

                foreach (TTableField field in table.grpTableField)
                {
                    createStmt += TWriteSQL.WriteField(TWriteSQL.eDatabaseType.Sqlite, table, field, firstField, false);
                    firstField = false;
                }

                if (table.HasPrimaryKey() && !createStmt.Contains("PRIMARY KEY AUTOINCREMENT"))
                {
                    createStmt += ", PRIMARY KEY (";
                    bool firstPrimaryKeyColumn = true;

                    foreach (string primaryKeyColumnName in table.GetPrimaryKey().strThisFields)
                    {
                        if (!firstPrimaryKeyColumn)
                        {
                            createStmt += ",";
                        }

                        createStmt += primaryKeyColumnName;
                        firstPrimaryKeyColumn = false;
                    }

                    createStmt += ")";
                }

                createStmt += AddForeignKeys(table);

                createStmt += ");";


                SqliteCommand cmd = new SqliteCommand(createStmt, conn);
                cmd.ExecuteNonQuery();
            }

            // sequence workaround
            // see http://www.Sqlite.org/faq.html#q1 AUTOINCREMENT
            foreach (TSequence seq in ADataDefinition.GetSequences())
            {
                string createStmt = "CREATE TABLE " + seq.strName + " (sequence INTEGER PRIMARY KEY AUTOINCREMENT, dummy INTEGER);";
                SqliteCommand cmd = new SqliteCommand(createStmt, conn);
                cmd.ExecuteNonQuery();
                createStmt = "INSERT INTO " + seq.strName + " VALUES(NULL, -1);";
                cmd = new SqliteCommand(createStmt, conn);
                cmd.ExecuteNonQuery();
            }

            conn.Close();

            return true;
        }

        static private string AddForeignKeys(TTable ATable)
        {
            string createStmt = string.Empty;

            if (ATable.HasForeignKey())
            {
                foreach (TConstraint constr in ATable.grpConstraint)
                {
                    if (constr.strType == "foreignkey")
                    {
                        createStmt += ", FOREIGN KEY(";
                        bool firstColumn = true;

                        foreach (string columnName in constr.strThisFields)
                        {
                            if (!firstColumn)
                            {
                                createStmt += ",";
                            }

                            createStmt += columnName;
                            firstColumn = false;
                        }

                        createStmt += ") REFERENCES " + constr.strOtherTable + "(";

                        firstColumn = true;

                        foreach (string columnName in constr.strOtherFields)
                        {
                            if (!firstColumn)
                            {
                                createStmt += ",";
                            }

                            createStmt += columnName;
                            firstColumn = false;
                        }

                        createStmt += ")";
                    }
                }
            }

            return createStmt;
        }

        /// <summary>
        /// load statements from an sql file that we use for PostgreSQL
        /// </summary>
        /// <param name="ADataDefinition"></param>
        /// <param name="ADBFilename"></param>
        /// <param name="APath"></param>
        /// <param name="ASqlfile"></param>
        /// <param name="ADBPwd"></param>
        /// <returns></returns>
        static public bool ExecuteLoadScript(TDataDefinitionStore ADataDefinition, string ADBFilename, string APath, string ASqlfile, string ADBPwd)
        {
            // see tutorial for fast bulk loads: http://Sqlite.phxsoftware.com/forums/t/134.aspx

            // sqlite on Windows does not support encryption with a password
            // System.EntryPointNotFoundException: sqlite3_key
            ADBPwd = string.Empty;

            TLogging.Log("Connecting to sqlite database " + ADBFilename);
            TLogging.Log("Loading file " + ASqlfile);

            if (!File.Exists(ADBFilename))
            {
                TLogging.Log("File does not exist: " + ADBFilename);
                return false;
            }

            SqliteConnection conn;

            try
            {
                conn = new SqliteConnection("Data Source=" + ADBFilename +
                    (ADBPwd.Length > 0 ? ";Password=" + ADBPwd : ""));

                conn.Open();
            }
            catch (Exception e)
            {
                TLogging.Log("cannot open file " + ADBFilename);
                TLogging.Log(e.ToString());
                return false;
            }

            StreamReader reader = new StreamReader(ASqlfile);
            string line = null;

            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);

                if (line.ToUpper().StartsWith("DELETE FROM "))
                {
                    RunCommand(conn, line);
                }
                else if (line.ToUpper().StartsWith("INSERT INTO ") || line.ToUpper().StartsWith("UPDATE "))
                {
                    line = line.Replace("true", "1");
                    line = line.Replace("false", "0");
                    RunCommand(conn, line);
                }
                else if (line.ToUpper().StartsWith("COPY ") && line.ToUpper().Contains("FROM STDIN"))
                {
                    string tablename = StringHelper.GetCSVValue(line.Replace(" ", ","), 1);

                    string columnnames = line.Substring(line.IndexOf("(") + 1);
                    columnnames = columnnames.Substring(0, columnnames.IndexOf(")"));

                    LoadDataFromReader(ADataDefinition, conn, reader, tablename, StringHelper.GetCSVList(columnnames, ",", true));
                }
                else if (line.ToUpper().StartsWith("COPY "))
                {
                    string tablename = StringHelper.GetCSVValue(line.Replace(" ", ","), 1);
                    LoadData(ADataDefinition, conn, APath, tablename);
                }
                else if (line.ToUpper().StartsWith("SELECT NEXTVAL("))
                {
                    string SequenceName = line.Substring(line.IndexOf("'") + 1);
                    SequenceName = SequenceName.Substring(0, SequenceName.IndexOf("'"));
                    RunCommand(conn, "INSERT INTO " + SequenceName + " VALUES(NULL, -1);");
                }
                else if (!line.StartsWith("--") && (line.Trim().Length > 0))
                {
                    throw new Exception("unknown command " + line);
                }
            }

            conn.Close();

            return true;
        }

        static private void RunCommand(SqliteConnection conn, string deletestmt)
        {
            using (SqliteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = deletestmt;
                    cmd.ExecuteNonQuery();
                }

                dbTrans.Commit();
            }
        }

        static private void PrepareSqlStatement(SqliteCommand cmd, string ATablename, TTable table, StringCollection AColumnNames)
        {
            string stmt = "INSERT INTO " + ATablename + " (";
            bool first = true;

            foreach (string columnname in AColumnNames)
            {
                if (!first)
                {
                    stmt += ",";
                }

                first = false;

                stmt += columnname;

                SqliteParameter param = cmd.CreateParameter();
                cmd.Parameters.Add(param);
            }

            stmt += ") VALUES (";
            first = true;

            for (int count = 0; count < AColumnNames.Count; count++)
            {
                if (!first)
                {
                    stmt += ",";
                }

                first = false;
                stmt += "?";
            }

            stmt += ")";

            cmd.CommandText = stmt;
        }

        static private void ProcessLine(SqliteCommand cmd, string line, TTable table, StringCollection AColumnNames)
        {
            int count = 0;
            string Separator = "\t";

            line = line.Replace("\\N", "?");

            if (!line.Contains(Separator))
            {
                Separator = ",";
            }

            foreach (string columnname in AColumnNames)
            {
                TTableField field = table.GetField(columnname);

                Object val = StringHelper.GetNextCSV(ref line, Separator);

                if (val.ToString() == "?")
                {
                    val = null;
                }
                else if ((field.strType == "date") && (val.ToString().Length != 0))
                {
                    if (val.ToString().Contains("-"))
                    {
                        StringCollection dateString = StringHelper.StrSplit(val.ToString(), "-");
                        val = new DateTime(Convert.ToInt16(dateString[0]),
                            Convert.ToInt16(dateString[1]),
                            Convert.ToInt16(dateString[2]));
                    }
                    else
                    {
                        try
                        {
                            val = new DateTime(Convert.ToInt16(val.ToString().Substring(0, 3)),
                                Convert.ToInt16(val.ToString().Substring(4, 2)),
                                Convert.ToInt16(val.ToString().Substring(6, 2)));
                        }
                        catch (Exception e)
                        {
                            TLogging.Log(e.ToString());
                            throw new Exception("error parsing date time " + val);
                        }
                    }
                }
                else if (field.strType == "bit")
                {
                    val = (val.ToString() == "true") || (val.ToString() == "t");
                }

                cmd.Parameters[count].Value = val;
                count++;
            }
        }

        /// <summary>
        /// load data from a CSV file in Postgresql COPY format
        /// </summary>
        static private bool LoadData(TDataDefinitionStore ADataDefinition, SqliteConnection conn, string APath, string ATablename)
        {
            using (SqliteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    TTable table = ADataDefinition.GetTable(ATablename);

                    StringCollection ColumnNames = new StringCollection();

                    foreach (TTableField f in table.grpTableField)
                    {
                        ColumnNames.Add(f.strName);
                    }

                    // prepare the statement
                    PrepareSqlStatement(cmd, ATablename, table, ColumnNames);

                    // load the data from the text file
                    string filename = APath + Path.DirectorySeparatorChar + ATablename + ".csv";

                    if (File.Exists(filename + ".local"))
                    {
                        filename += ".local";
                    }

                    StreamReader reader = new StreamReader(filename);
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        ProcessLine(cmd, line, table, ColumnNames);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            throw new Exception("failed to import line for table " + ATablename);
                        }
                    }
                }

                dbTrans.Commit();
            }

            return true;
        }

        /// <summary>
        /// load data from the sql file in Postgresql COPY format
        /// </summary>
        static private bool LoadDataFromReader(TDataDefinitionStore ADataDefinition,
            SqliteConnection conn,
            StreamReader sr,
            string ATablename,
            StringCollection AColumnNames)
        {
            using (SqliteTransaction dbTrans = conn.BeginTransaction())
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    TTable table = ADataDefinition.GetTable(ATablename);

                    // prepare the statement
                    PrepareSqlStatement(cmd, ATablename, table, AColumnNames);

                    string line;

                    while ((line = sr.ReadLine()) != "\\.")
                    {
                        ProcessLine(cmd, line, table, AColumnNames);

                        cmd.ExecuteNonQuery();
                    }
                }

                dbTrans.Commit();
            }

            return true;
        }
    }
}