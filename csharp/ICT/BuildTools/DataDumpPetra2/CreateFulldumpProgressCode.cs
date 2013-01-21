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
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// Create a Progress .p file for dumping all data to CSV files;
    /// does not do any upgrading or checking of data
    /// </summary>
    public class TCreateFulldumpProgressCode
    {
        private TDataDefinitionStore storeOld;

        private void DumpTable(ref StreamWriter sw, TTable oldTable)
        {
            // check for parameter, only dump table if parameter empty or equals the table name
            sw.WriteLine("IF WantThisTable(\"" + oldTable.strName + "\") THEN DO:");

            sw.WriteLine("OUTPUT STREAM OutFile TO fulldump/" + oldTable.strName + ".d.");
            sw.WriteLine("REPEAT FOR " + oldTable.strName + ':');

            sw.WriteLine("    FIND NEXT " + oldTable.strName + " NO-LOCK.");
            sw.WriteLine("    EXPORT STREAM OutFile DELIMITER \" \" ");

            foreach (TTableField field in oldTable.grpTableField)
            {
                if (field.strName == "s_modification_id_t")
                {
                    // improve readability, safe space
                    sw.Write(" ? ");
                }
                else
                {
                    sw.Write(" " + field.strName + " ");
                }
            }

            sw.WriteLine(".");
            sw.WriteLine("END.");
            sw.WriteLine("PUT STREAM OutFile UNFORMATTED '.'.");
            sw.WriteLine("OUTPUT STREAM OutFile CLOSE.");

            // now if we are on Linux, gzip that file
            sw.WriteLine("RUN ZipThisTable(\"" + oldTable.strName + "\").");
            sw.WriteLine("END.");
        }

        private void DumpSequences(ref StreamWriter sw)
        {
            sw.WriteLine("IF pv_tablename_c EQ \"\" OR pv_tablename_c EQ \"sequences\" THEN DO:");
            sw.WriteLine("OUTPUT STREAM OutFile TO fulldump/_seqvals.d.");
            List <TSequence>oldSequences = storeOld.GetSequences();

            foreach (TSequence oldSequence in oldSequences)
            {
                sw.WriteLine("    EXPORT STREAM OutFile DELIMITER \" \" 0 '" + oldSequence.strName + "' CURRENT-VALUE(" +
                    oldSequence.strName + ").");
            }

            sw.WriteLine("PUT STREAM OutFile UNFORMATTED '.'.");

            sw.WriteLine("END.");
        }

        /// <summary>
        /// create a Progress .p program for dumping the Progress data to CSV files
        /// </summary>
        public void GenerateFulldumpCode()
        {
            string PetraOldPath = TAppSettingsManager.GetValue("oldpetraxml", "petra23.xml");

            TDataDefinitionParser parserOld = new TDataDefinitionParser(PetraOldPath);

            parserOld.SupportPetra2xLegacyStandard = true;

            storeOld = new TDataDefinitionStore();

            System.Console.WriteLine("Reading 2.x xml file {0}...", PetraOldPath);

            if (!parserOld.ParseDocument(ref storeOld, false, true))
            {
                return;
            }

            string OutputFile = "fulldump23.p";

            System.Console.WriteLine("Writing file to {0}...", OutputFile);
            StreamWriter progressWriter = new StreamWriter(OutputFile);

            // print file header
            progressWriter.WriteLine("/* Generated with Ict.Tools.DataDumpPetra2x.exe */");

            System.Resources.ResourceManager RM = new System.Resources.ResourceManager("Ict.Tools.DataDumpPetra2.templateCode",
                System.Reflection.Assembly.GetExecutingAssembly());
            progressWriter.WriteLine(RM.GetString("progress_dump_functions"));

            progressWriter.WriteLine();

            List <TTable>oldTables = storeOld.GetTables();

            foreach (TTable oldTable in oldTables)
            {
                DumpTable(ref progressWriter, oldTable);
            }

            DumpSequences(ref progressWriter);
            progressWriter.WriteLine();
            progressWriter.Close();
            System.Console.WriteLine("Success: file written: {0}", Path.GetFullPath(OutputFile));
        }
    }
}