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
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// dump data from the progress database to CSV files and store as SQL COPY commands for PostgreSQL
    /// </summary>
    public class TDumpProgressToPostgresql
    {
        private TDataDefinitionStore storeOld;
        private TDataDefinitionStore storeNew;
        private Encoding ProgressFileEncoding;

        private void DumpTable(TTable newTable)
        {
            TLogging.Log(newTable.strName);

            string oldTableName = DataDefinitionDiff.GetOldTableName(newTable.strName);

            TTable oldTable = storeOld.GetTable(oldTableName);

            // if this is a new table in OpenPetra, do not dump anything. the table will be empty in OpenPetra
            if (oldTable == null)
            {
                return;
            }

            if (!Directory.Exists("fulldump"))
            {
                Directory.CreateDirectory("fulldump");
            }

            // the result will be stored in fulldump
            string dumpFile = "fulldump" + Path.DirectorySeparatorChar + oldTableName + ".d";

            // now run the compiled .r program against the Progress database
            // in debug mode, don't dump the file again
            if ((TLogging.DebugLevel == 0) || !File.Exists(dumpFile))
            {
                TRunProgress.RunProgress("fulldumpOpenPetraCSV.r", oldTableName, TLogging.GetLogFileName());

#if DISABLED
                // line numbers from exceptions in the ConvertCSVValues parser will not be completely correct in vi
                // if we do not call this. reason: some characters (^M is not considered as a newline character in vi, but in Notepad++ and also in StreamReader.ReadLine().
                // but this routine seems to cost a lot of memory, which makes all the other calculations even slower.
                // so only switch this on for debugging
                StringBuilder WholeFile = new StringBuilder();

                using (StreamReader fileReader = new StreamReader(dumpFile))
                {
                    // avoid single \r and \n newline characters. Convert all to the local line endings.
                    // Fixes problems with line number differences in vi
                    WholeFile.Append(fileReader.ReadToEnd());
                }

                WholeFile.Replace("\r\n", "\\r\\n").
                Replace("\n", Environment.NewLine).
                Replace("\r", Environment.NewLine).
                Replace("\\r\\n", Environment.NewLine);

                using (StreamWriter sw = new StreamWriter(dumpFile))
                {
                    sw.Write(WholeFile.ToString());
                }

                WholeFile = null;
#endif
            }

            TLogging.Log("after reading from Progress");

            StringCollection ColumnNames = new StringCollection();

            Console.WriteLine("COPY " + newTable.strName +" FROM stdin;");

            int CountRows = 0;

            using (StreamReader sr = new StreamReader(dumpFile, ProgressFileEncoding))
            {
                while (!sr.EndOfStream)
                {
                    List <string[]>DumpValues = TParseProgressCSV.ParseFile(sr, newTable.grpTableField.List.Count, ref CountRows);

                    TFixData.FixData(oldTableName, ColumnNames, ref DumpValues);

                    foreach (string[] row in DumpValues)
                    {
                        Console.WriteLine(StringHelper.StrMerge(row, '\t').Replace("\\\\N", "\\N").ToString());
                    }
                }
            }

            if (TLogging.DebugLevel == 0)
            {
                File.Delete(dumpFile);
            }

            TLogging.Log(" after processing file, rows: " + CountRows.ToString());

            Console.WriteLine("\\.");
            Console.WriteLine();
        }

        private static void DumpSequences()
        {
            TLogging.Log("dumping the sequences: ...");

            // now run the compiled .r program against the Progress database
            TRunProgress.RunProgress("fulldumpOpenPetraCSV.r", "sequences", TLogging.GetLogFileName());

            string dumpFile = "fulldump" + Path.DirectorySeparatorChar + "initialiseSequences.sql";
            StreamReader reader = new StreamReader(dumpFile);

            while (!reader.EndOfStream)
            {
                Console.WriteLine(reader.ReadLine());
            }

            Console.WriteLine();

            reader.Close();

            TLogging.Log("  sequences are done");

            File.Delete(dumpFile);
        }

        void WritePSQLHeader()
        {
            Console.WriteLine("--");
            Console.WriteLine("-- PostgreSQL database dump");
            Console.WriteLine("--");
            Console.WriteLine();
            Console.WriteLine("SET statement_timeout = 0;");
            Console.WriteLine("SET client_encoding = 'UTF8';");
            Console.WriteLine("SET standard_conforming_strings = off;");
            Console.WriteLine("SET check_function_bodies = false;");
            Console.WriteLine("SET client_min_messages = warning;");
            Console.WriteLine("SET escape_string_warning = off;");
            Console.WriteLine();
            Console.WriteLine("SET search_path = public, pg_catalog;");
            Console.WriteLine();
        }

        /// <summary>
        /// dump one or all tables from Progress, and create a sql load file for PostgreSQL
        /// </summary>
        public void DumpTables(string ATableName, String APetraOldPath, String APetraNewPath)
        {
            TDataDefinitionParser parserOld = new TDataDefinitionParser(APetraOldPath);

            storeOld = new TDataDefinitionStore();
            TDataDefinitionParser parserNew = new TDataDefinitionParser(APetraNewPath, false);
            storeNew = new TDataDefinitionStore();

            TLogging.Log(String.Format("Reading 2.x xml file {0}...", APetraOldPath));

            if (!parserOld.ParseDocument(ref storeOld, false, true))
            {
                return;
            }

            TLogging.Log(String.Format("Reading OpenPetra xml file {0}...", APetraNewPath));

            if (!parserNew.ParseDocument(ref storeNew, false, true))
            {
                return;
            }

            DataDefinitionDiff.newVersion = "3.0";
            TTable.GEnabledLoggingMissingFields = false;

            if (!Directory.Exists("fulldump"))
            {
                Directory.CreateDirectory("fulldump");
            }

            WritePSQLHeader();

            string ProgressCodepage = Environment.GetEnvironmentVariable("PROGRESS_CP");

            try
            {
                ProgressFileEncoding = Encoding.GetEncoding(Convert.ToInt32(ProgressCodepage));
            }
            catch
            {
                ProgressFileEncoding = Encoding.GetEncoding(ProgressCodepage);
            }

            if (ATableName.Length == 0)
            {
                DumpSequences();

                ArrayList newTables = storeNew.GetTables();

                foreach (TTable newTable in newTables)
                {
                    DumpTable(newTable);
                }

                GC.Collect();
            }
            else
            {
                DumpTable(storeNew.GetTable(ATableName));
            }

            TLogging.Log("Success: finished exporting the data");
            TTable.GEnabledLoggingMissingFields = true;
        }
    }
}