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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.DataStore
{
    /// <summary>
    /// produces an ordered list of tables, ordered by foreign key dependancies
    /// </summary>
    public class TGenerateTableList
    {
        /// <summary>
        /// write an ordered list of tables, ordered by foreign key dependancies
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="AFilename"></param>
        public static void WriteTableList(TDataDefinitionStore AStore, string AFilename)
        {
            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            ProcessTemplate Template = new ProcessTemplate(templateDir + Path.DirectorySeparatorChar +
                "ORM" + Path.DirectorySeparatorChar +
                "TableList.cs");

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(templateDir));

            List <TTable>tables = AStore.GetTables();

            tables = TTableSort.TopologicalSort(AStore, tables);

            string namesCodelet = string.Empty;
            string namesCodeletCustomReport = string.Empty;

            foreach (TTable t in tables)
            {
                namesCodelet += "list.Add(\"" + t.strName + "\");" + Environment.NewLine;

                if (t.AvailableForCustomReport)
                {
                    namesCodeletCustomReport += "list.Add(\"" + t.strName + "\");" + Environment.NewLine;
                }
            }

            Template.AddToCodelet("DBTableNames", namesCodelet);
            Template.AddToCodelet("DBTableNamesAvailableForCustomReport", namesCodeletCustomReport);

            List <TSequence>Sequences = AStore.GetSequences();

            namesCodelet = string.Empty;

            foreach (TSequence s in Sequences)
            {
                namesCodelet += "list.Add(\"" + s.strName + "\");" + Environment.NewLine;
            }

            Template.AddToCodelet("DBSequenceNames", namesCodelet);

            Template.FinishWriting(AFilename, ".cs", true);
        }

        /// <summary>
        /// write the file clean.sql that removes all data from the database, for easy resetting of the database with clean test data
        /// </summary>
        /// <param name="AStore"></param>
        /// <param name="AFilename"></param>
        public static void WriteDBClean(TDataDefinitionStore AStore, string AFilename)
        {
            StreamWriter sw = new StreamWriter(AFilename + ".new");

            sw.WriteLine("-- Generated with nant generateORMTables");
            List <TTable>tables = AStore.GetTables();
            tables = TTableSort.TopologicalSort(AStore, tables);
            tables.Reverse();

            foreach (TTable t in tables)
            {
                sw.WriteLine("DELETE FROM " + t.strName + ";");
            }

            sw.Close();

            TTextFile.UpdateFile(AFilename);
        }
    }
}