//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Data.SQLite;
using System.IO;
using Ict.Tools.DBXML;
using Ict.Common;

namespace GenerateSQL
{
/// <summary>
/// this class will create a Petra database with sqlite
/// and will allow to load data from CSV files that are in Postgresql COPY format
/// </summary>
public class TSQLiteWriter
{
    /// <summary>
    /// create an sqlite database and create all tables and sequences and indexes
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

        // see also tutorial http://sqlite.phxsoftware.com/forums/p/130/452.aspx#452
        SQLiteConnection conn = new SQLiteConnection("Data Source=" + ADBFilename + (ADBPwd.Length > 0 ? ";Password=" + ADBPwd : ""));
        conn.Open();

        foreach (TTable table in ADataDefinition.GetTables())
        {
            // see http://www.sqlite.org/lang_createtable.html
            string createStmt = "CREATE TABLE " + table.strName + " (";
            bool firstField = true;

            foreach (TTableField field in table.grpTableField.List)
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

            createStmt += ");";

            // TODO: primary key
            // TODO: foreign key

            SQLiteCommand cmd = new SQLiteCommand(createStmt, conn);
            cmd.ExecuteNonQuery();
        }

        // sequence workaround
        // see http://www.sqlite.org/faq.html#q1 AUTOINCREMENT
        foreach (TSequence seq in ADataDefinition.GetSequences())
        {
            string createStmt = "CREATE TABLE " + seq.strName + " (sequence INTEGER PRIMARY KEY AUTOINCREMENT, dummy INTEGER);";
            SQLiteCommand cmd = new SQLiteCommand(createStmt, conn);
            cmd.ExecuteNonQuery();
            createStmt = "INSERT INTO " + seq.strName + " VALUES(NULL, -1);";
            cmd = new SQLiteCommand(createStmt, conn);
            cmd.ExecuteNonQuery();
        }

        return true;
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
        // see tutorial for fast bulk loads: http://sqlite.phxsoftware.com/forums/t/134.aspx
        SQLiteConnection conn = new SQLiteConnection("Data Source=" + ADBFilename +
            (ADBPwd.Length > 0 ? ";Password=" + ADBPwd : ""));

        conn.Open();

        StreamReader reader = new StreamReader(ASqlfile);
        string line = null;

        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);

            if (line.ToUpper().StartsWith("DELETE FROM "))
            {
                RunCommand(conn, line);
            }
            else if (line.ToUpper().StartsWith("INSERT INTO "))
            {
                line = line.Replace("true", "1");
                line = line.Replace("false", "0");
                RunCommand(conn, line);
            }
            else if (line.ToUpper().StartsWith("COPY "))
            {
                string tablename = StringHelper.GetCSVValue(line.Replace(" ", ","), 1);
                LoadData(ADataDefinition, conn, APath, tablename);
            }
        }

        return true;
    }

    static private void RunCommand(SQLiteConnection conn, string deletestmt)
    {
        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = deletestmt;
                cmd.ExecuteNonQuery();
            }

            dbTrans.Commit();
        }
    }

    /// <summary>
    /// load data from a CSV file in Postgresql COPY format
    /// </summary>
    /// <param name="ADataDefinition"></param>
    /// <param name="conn"></param>
    /// <param name="APath"></param>
    /// <param name="ATablename"></param>
    /// <returns></returns>
    static private bool LoadData(TDataDefinitionStore ADataDefinition, SQLiteConnection conn, string APath, string ATablename)
    {
        using (SQLiteTransaction dbTrans = conn.BeginTransaction())
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                // prepare the statement
                string stmt = "INSERT INTO " + ATablename + " (";
                TTable table = ADataDefinition.GetTable(ATablename);
                bool first = true;

                foreach (TTableField field in table.grpTableField.List)
                {
                    if (!first)
                    {
                        stmt += ",";
                    }

                    first = false;

                    stmt += field.strName;

                    SQLiteParameter param = cmd.CreateParameter();
                    cmd.Parameters.Add(param);
                }

                stmt += ") VALUES (";
                first = true;

                for (int count = 0; count < table.grpTableField.List.Count; count++)
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

                // load the data from the text file
                StreamReader reader = new StreamReader(APath + Path.DirectorySeparatorChar + ATablename + ".csv");
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    int count = 0;

                    foreach (TTableField field in table.grpTableField.List)
                    {
                        Object val = StringHelper.GetNextCSV(ref line, ",");

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
                                val = new DateTime(Convert.ToInt16(val.ToString().Substring(0, 3)),
                                    Convert.ToInt16(val.ToString().Substring(4, 2)),
                                    Convert.ToInt16(val.ToString().Substring(6, 2)));
                            }
                        }
                        else if (field.strType == "bit")
                        {
                            val = (val.ToString() == "true");
                        }

                        cmd.Parameters[count].Value = val;
                        count++;
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            dbTrans.Commit();
        }

        return true;
    }
}
}