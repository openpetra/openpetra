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

                parserOld.SupportPetra2xLegacyStandard = true;

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
            if ((oldTable == null) && (newTable.strName != "p_postcode_region_range"))
            {
                return;
            }

            try
            {
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile + ".d.gz",
                    oldTable.grpTableField.Count);

                FileStream outStream = File.Create(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar +
                    newTable.strName + ".sql.gz");
                Stream gzoStream = new GZipOutputStream(outStream);
                StreamWriter MyWriter = new StreamWriter(gzoStream, Encoding.UTF8);

                FileStream outStreamTest = File.Create(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar +
                    newTable.strName + "_test.sql.gz");
                Stream gzoStreamTest = new GZipOutputStream(outStreamTest);
                StreamWriter MyWriterTest = new StreamWriter(gzoStreamTest, Encoding.UTF8);

                string rowCountDir = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                     Path.DirectorySeparatorChar + "_row_count.txt";
                StreamWriter MyWriterCount;

                if (File.Exists(rowCountDir))
                {
                    MyWriterCount = File.AppendText(rowCountDir);
                }
                else
                {
                    FileStream outStreamCount = File.Create(rowCountDir);
                    MyWriterCount = new StreamWriter(outStreamCount);
                }

                MyWriter.WriteLine("COPY " + newTable.strName + " FROM stdin;");

                int ProcessedRows = TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);

                ProcessedRows += MoveTables(newTable.strName, dumpFile, MyWriter, MyWriterTest, newTable);

                MyWriter.WriteLine("\\.");
                MyWriter.WriteLine();

                MyWriterTest.WriteLine();

                MyWriterCount.WriteLine(newTable.strName);
                MyWriterCount.WriteLine(ProcessedRows);

                MyWriter.Close();
                MyWriterTest.Close();
                MyWriterCount.Close();

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

        private int MoveTables(string ANewTableName, string dumpFile, StreamWriter MyWriter, StreamWriter MyWriterTest, TTable newTable)
        {
            int ProcessedRows = 0;

            if (ANewTableName == "a_batch")
            {
                TLogging.Log("a_this_year_old_batch");
                TTable oldTable = storeOld.GetTable("a_this_year_old_batch");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_batch") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);

                TLogging.Log("a_previous_year_batch");
                oldTable = storeOld.GetTable("a_previous_year_batch");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_batch") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);
            }
            else if (ANewTableName == "a_journal")
            {
                TLogging.Log("a_this_year_old_journal");
                TTable oldTable = storeOld.GetTable("a_this_year_old_journal");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_journal") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);

                TLogging.Log("a_previous_year_journal");
                oldTable = storeOld.GetTable("a_previous_year_journal");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_journal") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);
            }
            else if (ANewTableName == "a_transaction")
            {
                TLogging.Log("a_this_year_old_transaction");
                TTable oldTable = storeOld.GetTable("a_this_year_old_transaction");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_this_year_old_transaction") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);

                TLogging.Log("a_previous_year_transaction");
                oldTable = storeOld.GetTable("a_previous_year_transaction");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_previous_year_transaction") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);
            }
            else if (ANewTableName == "a_trans_anal_attrib")
            {
                TLogging.Log("a_thisyearold_trans_anal_attrib");
                TTable oldTable = storeOld.GetTable("a_thisyearold_trans_anal_attrib");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_thisyearold_trans_anal_attrib") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);

                TLogging.Log("a_prev_year_trans_anal_attrib");
                oldTable = storeOld.GetTable("a_prev_year_trans_anal_attrib");
                Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_prev_year_trans_anal_attrib") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);
            }
            else if (ANewTableName == "a_corporate_exchange_rate")
            {
                TLogging.Log("a_prev_year_corp_ex_rate");
                TTable oldTable = storeOld.GetTable("a_prev_year_corp_ex_rate");
                TParseProgressCSV Parser = new TParseProgressCSV(
                    dumpFile.Replace(ANewTableName, "a_prev_year_corp_ex_rate") + ".d.gz",
                    oldTable.grpTableField.Count);
                ProcessedRows += TFixData.MigrateData(Parser, MyWriter, MyWriterTest, oldTable, newTable);
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

            CreatePostcodeRegionRangeTable();

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

        // creates p_postcode_region_range.d.gz and populates with data from p_postcode_region.d.gz
        private void CreatePostcodeRegionRangeTable()
        {
            string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_postcode_region";
            FileInfo FileName = new FileInfo(dumpFile + ".d.gz");

            // new file is not needed if p_postcode_region.d.gz does not exist or is empty
            if (!File.Exists(dumpFile + ".d.gz") || (FileName.Length == 0))
            {
                return;
            }

            Encoding ProgressFileEncoding;
            string ProgressCodepage = TAppSettingsManager.GetValue("CodePage", Environment.GetEnvironmentVariable("PROGRESS_CP"));

            try
            {
                ProgressFileEncoding = Encoding.GetEncoding(Convert.ToInt32(ProgressCodepage));
            }
            catch
            {
                ProgressFileEncoding = Encoding.GetEncoding(ProgressCodepage);
            }

            try
            {
                string FilePath = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar +
                                  "p_postcode_region" + ".d.gz";
                Stream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                GZipInputStream gzipStream = new GZipInputStream(fs);
                StreamReader MyReader = new StreamReader(gzipStream, ProgressFileEncoding);

                FileStream outStream = File.Create(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar +
                    "p_postcode_region_range" + ".d.gz");
                Stream gzoStream = new GZipOutputStream(outStream);
                StreamWriter MyWriter = new StreamWriter(gzoStream, Encoding.UTF8);

                char[] block = new char[10000];
                int count = 0;

                // copy entire contents of p_postcode_region.d.gz to p_postcode_region_range.d.gz
                while ((count = MyReader.ReadBlock(block, 0, block.Length)) != 0)
                {
                    MyWriter.Write(block, 0, count);
                }

                MyWriter.Close();
            }
            catch (Exception e)
            {
                TLogging.Log("Memory usage: " + (GC.GetTotalMemory(false) / 1024 / 1024).ToString() + " MB");
                TLogging.Log("WARNING Problems processing file " + "p_postcode_region_range" + ": " + e.ToString());
            }

            storeOld.AddTable(storeNew.GetTable("p_postcode_region_range"));
        }

        /// <summary>
        /// if ATableName is empty: collect all sql.gz files and concatenate them to one, and also the sequence file
        /// otherwise just make that one table loadable by adding the PSQL Header
        /// </summary>
        public void CreateNewSQLFile(string ATableName)
        {
            GetStoreNew();

            //create test file
            TLogging.Log("creating _loadTest.sql.gz file...");

            using (FileStream outStream = File.Create(
                       TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                       Path.DirectorySeparatorChar + "_loadTest.sql.gz"))
            {
                using (Stream gzoStream = new GZipOutputStream(outStream))
                {
                    StreamWriter sw = new StreamWriter(gzoStream);

                    WritePSQLHeader(sw);

                    if (ATableName.Length > 0)
                    {
                        string fileName = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                          Path.DirectorySeparatorChar + ATableName + "_test.sql.gz";

                        if (File.Exists(fileName))
                        {
                            System.IO.Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                            GZipInputStream gzipStream = new GZipInputStream(fs);
                            StreamReader sr = new StreamReader(gzipStream);

                            sw.Write(sr.ReadToEnd());

                            sr.Close();
                        }
                    }
                    else
                    {
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
                                              Path.DirectorySeparatorChar + newTable.strName + "_test.sql.gz";

                            if (File.Exists(fileName))
                            {
                                System.IO.Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                                GZipInputStream gzipStream = new GZipInputStream(fs);
                                StreamReader sr = new StreamReader(gzipStream);

                                char[] block = new char[100000];
                                int count = 0;

                                while ((count = sr.ReadBlock(block, 0, block.Length)) != 0)
                                {
                                    sw.Write(block, 0, count);
                                }

                                sr.Close();
                            }
                        }
                    }

                    sw.Close();
                }
            }

            TLogging.Log("Success: finished writing the file _loadTest.sql.gz");
            TLogging.Log("creating _load.sql.gz file...");

            using (FileStream outStream = File.Create(
                       TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                       Path.DirectorySeparatorChar + "_load.sql.gz"))
            {
                using (Stream gzoStream = new GZipOutputStream(outStream))
                {
                    StreamWriter sw = new StreamWriter(gzoStream);

                    WritePSQLHeader(sw);

                    if (ATableName.Length > 0)
                    {
                        string fileName = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                          Path.DirectorySeparatorChar + ATableName + ".sql.gz";

                        if (File.Exists(fileName))
                        {
                            System.IO.Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                            GZipInputStream gzipStream = new GZipInputStream(fs);
                            StreamReader sr = new StreamReader(gzipStream);

                            sw.Write(sr.ReadToEnd());

                            sr.Close();
                        }
                    }
                    else
                    {
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

                        // Load Tables inside a single transaction
                        sw.WriteLine("BEGIN;");
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

                                char[] block = new char[100000];
                                int count = 0;

                                while ((count = sr.ReadBlock(block, 0, block.Length)) != 0)
                                {
                                    sw.Write(block, 0, count);
                                }

                                sr.Close();
                            }
                        }

                        sw.WriteLine("COMMIT;");
                    }

                    sw.Close();
                }
            }

            TLogging.Log("Success: finished writing the file _load.sql.gz");
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
                string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ledger.d.gz";

                if (!File.Exists(dumpFile))
                {
                    TRunProgress.RunProgress("fulldump23.r", "", TLogging.GetLogFileName());
                }
            }
            else
            {
                string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + ATableName + ".d.gz";

                if (!File.Exists(dumpFile))
                {
                    TRunProgress.RunProgress("fulldump23.r", ATableName, TLogging.GetLogFileName());
                }
            }

            TLogging.Log("Success: finished exporting the data");
        }
    }
}