//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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
using System.IO;
using Ict.Common;
using Ict.Common.DB;

namespace GenerateSQL
{
    /// <summary>
    /// initialise the database
    /// </summary>
    public class TLoadMysql
    {
        /// load data from csv files and sql statements
        public static bool LoadData(string AHostname, string ADatabaseName, string AUsername, string APassword, string ALoadSQLFileName)
        {
            StreamReader sr = null;
            TDBTransaction WriteTransaction = null;
            bool SubmissionResult = false;

            DBAccess.GDBAccessObj = new TDataBase(TDBType.MySQL);
            try
            {
                DBAccess.GDBAccessObj.EstablishDBConnection(TDBType.MySQL, AHostname, "", ADatabaseName, AUsername, APassword, "",
                    "GenerateSQL.TLoadMysql.LoadData DB Connection");
                sr = new StreamReader(ALoadSQLFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref WriteTransaction,
                ref SubmissionResult,
                delegate
                {
                    // one command per line.
                    // file is in postgresql syntax
                    // either COPY FROM or INSERT

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (line.Trim().ToUpper().StartsWith("INSERT"))
                        {
                            DBAccess.GDBAccessObj.ExecuteNonQuery(line, WriteTransaction);
                        }
                        else if (line.Trim().ToUpper().StartsWith("COPY"))
                        {
                            // pgsql: COPY p_language FROM 'c:/p_language.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
                            // mysql: LOAD DATA LOCAL INFILE 'c:/p_language.csv' INTO TABLE p_language FIELDS TERMINATED BY ',' ENCLOSED BY '"' ESCAPED BY '"';

                            // need to fix the NULL value from ? to NULL
                            string DataFilename = line.Substring(line.IndexOf("'") + 1);
                            DataFilename = DataFilename.Substring(0, DataFilename.IndexOf("'"));
                            string TableName = line.Substring(line.IndexOf("COPY ") + 5);
                            TableName = TableName.Substring(0, TableName.IndexOf(" "));

                            StreamReader sData = new StreamReader(DataFilename);
                            StreamWriter sDataWriter = new StreamWriter(DataFilename + ".local");
                            bool firstRow = true;

                            while (!sData.EndOfStream)
                            {
                                string CSVDataQuestionMark = sData.ReadLine().Trim();
                                string CSVDataNULL = string.Empty;

                                while (CSVDataQuestionMark.Length > 0)
                                {
                                    bool quotedValue = CSVDataQuestionMark.StartsWith("\"");
                                    string value = StringHelper.GetNextCSV(ref CSVDataQuestionMark, ",");

                                    if (value == "?")
                                    {
                                        value = "NULL";
                                    }

                                    // if true or false is written in quotes, do not convert to integer. needed for a_account_property
                                    if ((!quotedValue && (value == "false")) || (value == "no"))
                                    {
                                        value = "0";
                                    }

                                    if ((!quotedValue && (value == "true")) || (value == "yes"))
                                    {
                                        value = "1";
                                    }

                                    CSVDataNULL = StringHelper.AddCSV(CSVDataNULL, value);
                                }

                                if (CSVDataNULL.Length > 0)
                                {
                                    if (firstRow)
                                    {
                                        firstRow = false;
                                    }
                                    else
                                    {
                                        sDataWriter.WriteLine();
                                    }

                                    sDataWriter.Write(CSVDataNULL);
                                }
                            }

                            sData.Close();
                            sDataWriter.Close();

                            // see also http://dev.mysql.com/doc/refman/5.1/en/insert-speed.html
                            string stmt = String.Format(
                                "LOAD DATA LOCAL INFILE '{0}' INTO TABLE {1} FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' ESCAPED BY '\"' LINES TERMINATED BY '"
                                +
                                Environment.NewLine + "';",
                                DataFilename + ".local",
                                TableName);

                            DBAccess.GDBAccessObj.ExecuteNonQuery(stmt, WriteTransaction);
                        }
                    }

                    SubmissionResult = true;

                    sr.Close();
                });

            DBAccess.GDBAccessObj.CloseDBConnection();

            return true;
        }
    }
}
