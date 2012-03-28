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

        private TDataDefinitionStore storeOld;
        private TDataDefinitionStore storeNew;
        private Encoding ProgressFileEncoding;

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

            if (!File.Exists(dumpFile + ".sql.gz") || ((new FileInfo(dumpFile + ".sql.gz")).Length == 0))
            {
                if (((long)info.Length > MAX_SIZE_D_GZ_SEPARATE_PROCESS) && !TAppSettingsManager.HasValue("table"))
                {
                    ProcessAndWritePostgresqlFileNewProcess(dumpFile, newTable);
                }
                else
                {
                    ProcessAndWritePostgresqlFile(dumpFile, newTable);
                }
            }
        }

        private void ProcessAndWritePostgresqlFileNewProcess(string dumpFile, TTable newTable)
        {
            if (TAppSettingsManager.GetValue("IgnoreBigTables", "false", false) == "true")
            {
                return;
            }

            TLogging.Log("Special treatment of file " + Path.GetFileName(dumpFile));
            System.Diagnostics.Process ChildProcess = new System.Diagnostics.Process();
            ChildProcess.EnableRaisingEvents = false;

            if (Utilities.DetermineExecutingOS() == TExecutingOSEnum.eosLinux)
            {
                ChildProcess.StartInfo.FileName = "mono";

                ChildProcess.StartInfo.Arguments = "Ict.Tools.DataDumpPetra2.exe ";
            }
            else         // windows
            {
                ChildProcess.StartInfo.FileName = "Ict.Tools.DataDumpPetra2.exe";

                ChildProcess.StartInfo.Arguments = string.Empty;
            }

            ChildProcess.StartInfo.Arguments +=
                " -debuglevel:" + TAppSettingsManager.GetValue("debuglevel", "0") +
                " -table:" + newTable.strName +
                " -newpetraxml:" + TAppSettingsManager.GetValue("newpetraxml", "petra.xml") +
                " -oldpetraxml:" + TAppSettingsManager.GetValue("oldpetraxml", "petra.xml");

            ChildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            ChildProcess.EnableRaisingEvents = true;
            ChildProcess.StartInfo.UseShellExecute = false;

            if (!ChildProcess.Start())
            {
                return;
            }

            Thread.Sleep(500);

            while ((!ChildProcess.HasExited))
            {
                Thread.Sleep(500);
            }

            ChildProcess.Close();
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
                using (System.IO.Stream fs = new FileStream(dumpFile + ".d.gz", FileMode.Open, FileAccess.Read))
                {
                    using (GZipInputStream gzipStream = new GZipInputStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(gzipStream, ProgressFileEncoding))
                        {
                            using (FileStream outStream = File.Create(dumpFile + ".sql.gz"))
                            {
                                using (Stream gzoStream = new GZipOutputStream(outStream))
                                {
                                    using (StreamWriter sw = new StreamWriter(gzoStream))
                                    {
                                        int CountRows = 0;

                                        sw.WriteLine("COPY " + newTable.strName + " FROM stdin;");
                                        Console.WriteLine("COPY " + newTable.strName + " FROM stdin;");

                                        while (!sr.EndOfStream)
                                        {
                                            List <string[]>ParsedValues = TParseProgressCSV.ParseFile(sr,
                                                oldTable.grpTableField.List.Count,
                                                ref CountRows);

                                            List <string[]>DumpValues = TFixData.MigrateData(oldTable, newTable, ParsedValues);

                                            foreach (string[] row in DumpValues)
                                            {
                                                sw.WriteLine(StringHelper.StrMerge(row, '\t').Replace("\\\\N", "\\N").ToString());
                                                Console.WriteLine(StringHelper.StrMerge(row, '\t').Replace("\\\\N", "\\N").ToString());
                                            }
                                        }

                                        sw.Close();

                                        TLogging.Log(" after processing file, rows: " + CountRows.ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                File.Delete(dumpFile + ".d");
            }
            catch (Exception e)
            {
                TLogging.Log((GC.GetTotalMemory(false) / 1024 / 1024).ToString());
                TLogging.Log("WARNING Problems processing file " + dumpFile + ": " + e.ToString());

                if (File.Exists(dumpFile + ".sql.gz"))
                {
                    File.Delete(dumpFile + ".sql.gz");
                }
            }

            Console.WriteLine("\\.");
            Console.WriteLine();
        }

        private static void WriteSequences()
        {
            TLogging.Log("writing the sequences: ...");
// TODO
            string dumpFile = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "initialiseSequences.sql";
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
        /// Load the data from the 2.x Petra CSV file, and create psql load file
        /// </summary>
        public void LoadTablesToPostgresql(string ATableName, String APetraOldPath, String APetraNewPath)
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
                // TODO LoadSequences();

                ArrayList newTables = storeNew.GetTables();

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

            TLogging.Log("Success: finished exporting the data");
            TTable.GEnabledLoggingMissingFields = true;
        }

        /// <summary>
        /// dump one or all tables from Progress into a simple CSV file. still using the old Petra 2.x format
        /// </summary>
        public void DumpTablesToCSV(string ATableName, String APetraOldPath)
        {
            TDataDefinitionParser parserOld = new TDataDefinitionParser(APetraOldPath);

            storeOld = new TDataDefinitionStore();
            TLogging.Log(String.Format("Reading 2.x xml file {0}...", APetraOldPath));

            if (!parserOld.ParseDocument(ref storeOld, false, true))
            {
                return;
            }

            if (!Directory.Exists(TAppSettingsManager.GetValue("fulldumpPath", "fulldump")))
            {
                Directory.CreateDirectory(TAppSettingsManager.GetValue("fulldumpPath", "fulldump"));
            }

            TLogging.Log("Start: dumping the data");

            if (ATableName.Length == 0)
            {
                TRunProgress.RunProgress("fulldumpPetra23.r", "Sequences", TLogging.GetLogFileName());

                ArrayList oldTables = storeOld.GetTables();

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