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
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// dump data from the progress database to CSV files and store as SQL COPY commands for PostgreSQL
    /// </summary>
    public class TDumpProgressToPostgresql
    {
        const Int32 MAX_SIZE_D_GZ_SEPARATE_PROCESS = 100000;

        private static TDataDefinitionStore storeOld = null;
        private static TDataDefinitionStore storeNew = null;

        /// <summary>
        /// get the data structure for the version that should be upgraded
        /// </summary>
        public static TDataDefinitionStore GetStoreOld()
        {
            if (storeOld == null)
            {
                string PetraOldPath = TAppSettingsManager.GetValue("oldpetraxml", "petra23.xml");

                TLogging.Log(String.Format("Reading 2.x xml file {0}...", PetraOldPath));
                TDataDefinitionParser parserOld = new TDataDefinitionParser(PetraOldPath);
                storeOld = new TDataDefinitionStore();

                if (!parserOld.ParseDocument(ref storeOld, false, true))
                {
                    return null;
                }
            }

            return storeOld;
        }

        /// <summary>
        /// get the data structure for the new version
        /// </summary>
        public static TDataDefinitionStore GetStoreNew()
        {
            if (storeNew == null)
            {
                string PetraNewPath = TAppSettingsManager.GetValue("newpetraxml", "petra.xml");

                TLogging.Log(String.Format("Reading OpenPetra xml file {0}...", PetraNewPath));

                TDataDefinitionParser parserNew = new TDataDefinitionParser(PetraNewPath, false);
                storeNew = new TDataDefinitionStore();

                if (!parserNew.ParseDocument(ref storeNew, false, true))
                {
                    return null;
                }
            }

            return storeNew;
        }

        private void LoadTable(TTable newTable)
        {
            TLogging.Log(newTable.strName);

            string oldTableName = DataDefinitionDiff.GetOldTableName(newTable.strName);

            TTable oldTable = storeOld.GetTable(oldTableName);

            // if this is a new table in OpenPetra, do not dump anything. the table will be empty in OpenPetra
            if (oldTable == null)
            {
                return;
            }

            // the file has already been stored in fulldump, tablename.d.gz
            string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + oldTableName;

            if (!File.Exists(dumpFile + ".d.gz"))
            {
                TLogging.Log("cannot find file " + dumpFile + ".d.gz");
                return;
            }

            FileInfo info = new FileInfo(dumpFile + ".d.gz");

            if (info.Length == 0)
            {
                // this table should be ignored
                TLogging.Log("ignoring " + dumpFile + ".d.gz");
                return;
            }

            if (TAppSettingsManager.HasValue("table") || !File.Exists(dumpFile + ".sql.gz") || ((new FileInfo(dumpFile + ".sql.gz")).Length == 0))
            {
                if (((long)info.Length > MAX_SIZE_D_GZ_SEPARATE_PROCESS) && !TAppSettingsManager.HasValue("table"))
                {
                    if (TAppSettingsManager.GetValue("IgnoreBigTables", "false", false) == "false")
                    {
                        ProcessAndWritePostgresqlFile(dumpFile, newTable);
                    }
                }
                else
                {
                    ProcessAndWritePostgresqlFile(dumpFile, newTable);
                }
            }
        }

        /// <summary>
        /// process the data from the Progress dump file, so that Postgresql can read the result
        /// </summary>
        public void ProcessAndWritePostgresqlFile(string dumpFile, TTable newTable)
        {
            string oldTableName = DataDefinitionDiff.GetOldTableName(newTable.strName);

            TTable oldTable = storeOld.GetTable(oldTableName);

            // if this is a new table in OpenPetra, do not dump anything. the table will be empty in OpenPetra
            if (oldTable == null)
            {
                return;
            }

            try
            {
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile + ".d.gz",
                    oldTable.grpTableField.Count);

                FileStream outStream = File.Create(dumpFile + ".sql.gz");
                Stream gzoStream = new GZipOutputStream(outStream);
                StreamWriter MyWriter = new StreamWriter(gzoStream, Encoding.UTF8);

                MyWriter.WriteLine("COPY " + newTable.strName + " FROM stdin;");

                int ProcessedRows = TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);

                ProcessedRows += MoveTables(newTable.strName, dumpFile, MyWriter, newTable);

                MyWriter.WriteLine("\\.");
                MyWriter.WriteLine();

                MyWriter.Close();

                TLogging.Log(" after processing file, rows: " + ProcessedRows.ToString());
            }
            catch (Exception e)
            {
                TLogging.Log("Memory usage: " + (GC.GetTotalMemory(false) / 1024 / 1024).ToString() + " MB");
                TLogging.Log("WARNING Problems processing file " + dumpFile + ": " + e.ToString());

                if (File.Exists(dumpFile + ".sql.gz"))
                {
                    File.Delete(dumpFile + ".sql.gz");
                }
            }
        }

        private int MoveTables(string ANewTableName, string dumpFile, StreamWriter MyWriter, TTable newTable)
        {
            int ProcessedRows = 0;

            if (ANewTableName == "a_batch")
            {
                TLogging.Log("a_this_year_old_batch");
                TTable oldTable = storeOld.GetTable("a_this_year_old_batch");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_batch") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);

                TLogging.Log("a_previous_year_batch");
                oldTable = storeOld.GetTable("a_previous_year_batch");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_batch") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);
            }
            else if (ANewTableName == "a_journal")
            {
                TLogging.Log("a_this_year_old_journal");
                TTable oldTable = storeOld.GetTable("a_this_year_old_journal");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_journal") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);

                TLogging.Log("a_previous_year_journal");
                oldTable = storeOld.GetTable("a_previous_year_journal");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_journal") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);
            }
            else if (ANewTableName == "a_transaction")
            {
                TLogging.Log("a_this_year_old_transaction");
                TTable oldTable = storeOld.GetTable("a_this_year_old_transaction");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_transaction") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);

                TLogging.Log("a_previous_year_transaction");
                oldTable = storeOld.GetTable("a_previous_year_transaction");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_transaction") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);
            }
            else if (ANewTableName == "a_trans_anal_attrib")
            {
                TLogging.Log("a_thisyearold_trans_anal_attrib");
                TTable oldTable = storeOld.GetTable("a_thisyearold_trans_anal_attrib");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_thisyearold_trans_anal_attrib") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);

                TLogging.Log("a_prev_year_trans_anal_attrib");
                oldTable = storeOld.GetTable("a_prev_year_trans_anal_attrib");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_prev_year_trans_anal_attrib") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);
            }
            else if (ANewTableName == "a_corporate_exchange_rate")
            {
                TLogging.Log("a_prev_year_corp_ex_rate");
                TTable oldTable = storeOld.GetTable("a_prev_year_corp_ex_rate");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_prev_year_corp_ex_rate") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, oldTable, newTable);
            }

            return ProcessedRows;
        }

        void WritePSQLHeader(StreamWriter sw)
        {
            sw.WriteLine("--");
            sw.WriteLine("-- PostgreSQL database dump");
            sw.WriteLine("--");
            sw.WriteLine();
            sw.WriteLine("SET statement_timeout = 0;");
            sw.WriteLine("SET client_encoding = 'UTF8';");
            sw.WriteLine("SET standard_conforming_strings = off;");
            sw.WriteLine("SET check_function_bodies = false;");
            sw.WriteLine("SET client_min_messages = warning;");
            sw.WriteLine("SET escape_string_warning = off;");
            sw.WriteLine();
            sw.WriteLine("SET search_path = public, pg_catalog;");
            sw.WriteLine();
        }

        /// <summary>
        /// Load the data from the 2.x Petra CSV file, and create psql load file
        /// </summary>
        public void LoadTablesToPostgresql(string ATableName)
        {
            GetStoreOld();
            GetStoreNew();

            DataDefinitionDiff.newVersion = "3.0";
            TTable.GEnabledLoggingMissingFields = false;

            TParseProgressCSV.InitProgressCodePage();

            TSequenceWriter.InitSequences(GetStoreNew().GetSequences());

            if (ATableName.Length == 0)
            {
                List <TTable>newTables = storeNew.GetTables();

                foreach (TTable newTable in newTables)
                {
                    LoadTable(newTable);
                }

                GC.Collect();
            }
            else
            {
                LoadTable(storeNew.GetTable(ATableName));
            }

            // write some tables, if they have been used
            TFinanceAccountsPayableUpgrader.WriteAPDocumentNumberToId();
            TSequenceWriter.WriteSequences();

            TLogging.Log("Success: finished exporting the data");
            TTable.GEnabledLoggingMissingFields = true;
        }

        /// <summary>
        /// collect all sql.gz files and concatenate them to one, and also the sequence file
        /// </summary>
        public void CreateNewSQLFile()
        {
            GetStoreNew();

            TLogging.Log("creating load.sql.gz file...");

            using (FileStream outStream = File.Create(
                       TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                       Path.DirectorySeparatorChar + "load.sql.gz"))
            {
                using (Stream gzoStream = new GZipOutputStream(outStream))
                {
                    StreamWriter sw = new StreamWriter(gzoStream);

                    WritePSQLHeader(sw);

                    // Load Sequences
                    string fileNameSequences = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                               Path.DirectorySeparatorChar + "_Sequences.sql.gz";

                    if (File.Exists(fileNameSequences))
                    {
                        System.IO.Stream fs = new FileStream(fileNameSequences, FileMode.Open, FileAccess.Read);
                        GZipInputStream gzipStream = new GZipInputStream(fs);
                        StreamReader sr = new StreamReader(gzipStream);

                        sw.Write(sr.ReadToEnd());

                        sr.Close();
                    }

                    // Load Tables
                    List <TTable>newTables = storeNew.GetTables();

                    foreach (TTable newTable in newTables)
                    {
                        string fileName = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                          Path.DirectorySeparatorChar + newTable.strName + ".sql.gz";

                        if (File.Exists(fileName))
                        {
                            System.IO.Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                            GZipInputStream gzipStream = new GZipInputStream(fs);
                            StreamReader sr = new StreamReader(gzipStream);

                            sw.Write(sr.ReadToEnd());

                            sr.Close();
                        }
                    }

                    sw.Close();
                }
            }

            TLogging.Log("Success: finished writing the file load.sql.gz");
        }

        /// <summary>
        /// dump one or all tables from Progress into a simple CSV file. still using the old Petra 2.x format
        /// </summary>
        public void DumpTablesToCSV(string ATableName)
        {
            GetStoreOld();

            if (!Directory.Exists(TAppSettingsManager.GetValue("fulldumpPath", "fulldump")))
            {
                Directory.CreateDirectory(TAppSettingsManager.GetValue("fulldumpPath", "fulldump"));
            }

            TLogging.Log("Start: dumping the data");

            if (ATableName.Length == 0)
            {
                TRunProgress.RunProgress("fulldumpPetra23.r", "Sequences", TLogging.GetLogFileName());

                List <TTable>oldTables = storeOld.GetTables();

                foreach (TTable oldTable in oldTables)
                {
                    string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + oldTable.strName;

                    if (!File.Exists(dumpFile + ".d.gz"))
                    {
                        TLogging.Log("Exporting to CSV: table " + oldTable.strName);
                        TRunProgress.RunProgress("fulldumpPetra23.r", oldTable.strName, TLogging.GetLogFileName());

                        if (TLogging.DebugLevel >= 10)
                        {
                            // line numbers from exceptions in the ConvertCSVValues parser will not be completely correct in vi
                            // if we do not call this. reason: some characters (^M is not considered as a newline character in vi, but in Notepad++ and also in StreamReader.ReadLine().
                            // but this routine seems to cost a lot of memory, which makes all the other calculations even slower.
                            // so only switch this on for debugging
                            StringBuilder WholeFile = new StringBuilder();

                            using (StreamReader fileReader = new StreamReader(dumpFile + ".d"))
                            {
                                // avoid single \r and \n newline characters. Convert all to the local line endings.
                                // Fixes problems with line number differences in vi
                                WholeFile.Append(fileReader.ReadToEnd());
                            }

                            WholeFile.Replace("\r\n", "\\r\\n").
                            Replace("\n", Environment.NewLine).
                            Replace("\r", Environment.NewLine).
                            Replace("\\r\\n", Environment.NewLine);

                            using (StreamWriter sw = new StreamWriter(dumpFile + ".d"))
                            {
                                sw.Write(WholeFile.ToString());
                            }

                            WholeFile = null;
                        }

                        // gzip the file, to keep the amount of data small on the harddrive
                        Stream outStream = File.Create(dumpFile + ".d.gz");
                        Stream gzoStream = new GZipOutputStream(outStream);

                        using (Stream inputStream = File.OpenRead(dumpFile + ".d"))
                        {
                            byte[]  dataBuffer = new byte[4096];
                            StreamUtils.Copy(inputStream, gzoStream, dataBuffer);
                        }
                        gzoStream.Close();

                        File.Delete(dumpFile + ".d");
                    }
                }
            }
            else
            {
                TRunProgress.RunProgress("fulldumpPetra23.r", ATableName, TLogging.GetLogFileName());
            }

            TLogging.Log("Success: finished exporting the data");
        }
    }
}