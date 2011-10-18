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
using System.IO;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
/// <summary>
/// main class
/// </summary>
    public class TMain
    {
        /// <summary>
        /// main function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            new TAppSettingsManager(false);
            new TLogging("Ict.Tools.DataDumpPetra2.log");

            if (!TAppSettingsManager.HasValue("debuglevel"))
            {
                Console.Error.WriteLine("dumps one single table or all tables from Progress Petra 2.3 into Postgresql SQL load format");
                Console.Error.WriteLine(
                    "usage: Ict.Tools.DataDumpPetra2 -debuglevel:<0..10> -table:<single table or all> -oldpetraxml:<path and filename of old petra.xml> -newpetraxml:<path and filename of petra.xml>");
                Console.Error.WriteLine("will default to processing all tables, and using petra23.xml and petra.xml from the current directory");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("If the file fulldumpOpenPetraCSV.r does not exist yet, the .p file will be written.");
                Console.Error.WriteLine("");
                Console.Error.WriteLine(
                    "You should redirect the output to a file, or even pipe it through gzip. eg. mono Ict.Tools.DataDumpPetra2.exe | iconv --to-code=UTF-8 | gzip > mydump.sql.gz");
                Console.Error.WriteLine("");
            }

            try
            {
                TLogging.DebugLevel = TAppSettingsManager.GetInt16("debuglevel", 0);
                string table = TAppSettingsManager.GetValue("table", "");
                string newxmlfile = TAppSettingsManager.GetValue("newpetraxml", "petra.xml");
                string oldxmlfile = TAppSettingsManager.GetValue("oldpetraxml", "petra23.xml");

                if (!File.Exists("fulldumpOpenPetraCSV.r"))
                {
                    TCreateFulldumpProgressCode CreateProgressCode = new TCreateFulldumpProgressCode();
                    CreateProgressCode.GenerateFulldumpCode(oldxmlfile, newxmlfile, "fulldumpOpenPetraCSV.p");
                    TLogging.Log("Please compile fulldumpOpenPetraCSV.p against a StandAlone Petra 2.3 database (network would take forever),");
                    TLogging.Log("and copy the resulting fulldumpOpenPetraCSV.r into this directory. Then rerun Ict.Tools.DataDumpPetra2.exe.");
                    return;
                }

                TDumpProgressToPostgresql dumper = new TDumpProgressToPostgresql();
                dumper.DumpTables(table, oldxmlfile, newxmlfile);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);

                if (e.InnerException != null)
                {
                    TLogging.Log(e.InnerException.Message);
                }

                TLogging.Log(e.StackTrace);
            }
        }
    }
}