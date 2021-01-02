//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Tools.DBXML;

namespace GenerateSQL
{
    /// <summary>
    /// initialise the database
    /// </summary>
    public class TLoadMysql
    {
        /// load data from csv files and sql statements
        public static bool ExecuteLoadScript(TDataDefinitionStore ADataDefinition, string AHostname, string ADatabaseName, string AUsername, string APassword, string ALoadSQLFileName)
        {
            StreamReader sr = null;
            TDBTransaction WriteTransaction = new TDBTransaction();
            bool SubmissionResult = false;

            TDataBase DBAccessObj = new TDataBase(TDBType.MySQL);
            try
            {
                DBAccessObj.EstablishDBConnection(TDBType.MySQL, AHostname, "", ADatabaseName, AUsername, APassword, "",
                    true,
                    "GenerateSQL.TLoadMysql.LoadData DB Connection");
                sr = new StreamReader(ALoadSQLFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw e;
            }

            // one command per line.
            // file is in postgresql syntax
            // either COPY FROM or INSERT

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                while (!line.Trim().StartsWith("--") && !line.Trim().EndsWith(";") && !sr.EndOfStream)
                {
                    string templine = sr.ReadLine();

                    if (!templine.StartsWith("--"))
                    {
                        line += " " + templine;
                    }
                }

                if (line.Trim().ToUpper().StartsWith("INSERT") || line.Trim().ToUpper().StartsWith("UPDATE"))
                {
                    DBAccessObj.WriteTransaction(ref WriteTransaction,
                        ref SubmissionResult,
                        delegate
                        {
                            DBAccessObj.ExecuteNonQuery(line, WriteTransaction);
                            SubmissionResult = true;
                        });
                }
                else if (line.Trim().ToUpper().StartsWith("COPY"))
                {
                    // pgsql: COPY p_language FROM 'c:/p_language.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
                    // mysql: LOAD DATA LOCAL INFILE 'c:/p_language.csv' INTO TABLE p_language FIELDS TERMINATED BY ',' ENCLOSED BY '"' ESCAPED BY '"';
                    // But MySQL 8 makes it quite difficult for security reasons, to use LOAD DATA LOCAL INFILE.
                    // So we parse the file and load the data directly. It is not a huge performance loss.
                    DBAccessObj.WriteTransaction(ref WriteTransaction,
                        ref SubmissionResult,
                        delegate
                        {
                            string tablename = StringHelper.GetCSVValue(line.Trim().Replace(" ", ","), 1);
                            LoadData(DBAccessObj, ADataDefinition, Path.GetDirectoryName(ALoadSQLFileName), tablename);
                            SubmissionResult = true;
                        });
                }
            }

            sr.Close();

            DBAccessObj.CloseDBConnection();

            return true;
        }

        /// <summary>
        /// load data from a CSV file in Postgresql COPY format
        /// </summary>
        static private bool LoadData(TDataBase DBAccessObj, TDataDefinitionStore ADataDefinition, string APath, string ATablename)
        {
            TTable table = ADataDefinition.GetTable(ATablename);

            // prepare the statement
            string sqlStmt = table.PrepareSQLInsertStatement();

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
                List <OdbcParameter> Parameters = table.PrepareParametersInsertStatement(line);

                if (DBAccessObj.ExecuteNonQuery(sqlStmt, DBAccessObj.Transaction, Parameters.ToArray()) == 0)
                {
                    throw new Exception("failed to import line for table " + ATablename);
                }
            }

            return true;
        }
    }
}
