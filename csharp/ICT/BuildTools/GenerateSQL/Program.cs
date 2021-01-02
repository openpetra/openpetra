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
using Ict.Common;
using Ict.Tools.DBXML;
using GenerateSQL;

namespace Ict.Tools.GenerateSQL
{
    class Program
    {
        public static void Main(string[] args)
        {
            TCmdOpts cmdLine = new TCmdOpts();
            string operation, xmlfile, outputfile;

            new TAppSettingsManager(false);
            new TLogging(System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "/../../log/generatesql.log");

            try
            {
                operation = cmdLine.GetOptValue("do");
                xmlfile = cmdLine.GetOptValue("petraxml");
                outputfile = cmdLine.GetOptValue("outputFile");
                TWriteSQL.eDatabaseType dbms = TWriteSQL.StringToDBMS(cmdLine.GetOptValue("dbms"));

                if (operation == "sql")
                {
                    System.Console.WriteLine("Reading xml file {0}...", xmlfile);
                    TDataDefinitionParser parser = new TDataDefinitionParser(xmlfile);
                    TDataDefinitionStore store = new TDataDefinitionStore();

                    if (parser.ParseDocument(ref store))
                    {
                        // create an sql script that will be loaded into the database later
                        TWriteSQL.WriteSQL(store, dbms, outputfile);
                    }
                }

                if (operation == "load")
                {
                    if (dbms == TWriteSQL.eDatabaseType.MySQL)
                    {
                        System.Console.WriteLine("Reading xml file {0}...", xmlfile);
                        TDataDefinitionParser parser = new TDataDefinitionParser(xmlfile);
                        TDataDefinitionStore store = new TDataDefinitionStore();

                        if (parser.ParseDocument(ref store))
                        {
                            // we want to write directly to the database
                            TLoadMysql.ExecuteLoadScript(store, cmdLine.GetOptValue("host"), cmdLine.GetOptValue("database"), cmdLine.GetOptValue("username"),
                                cmdLine.GetOptValue("password"), cmdLine.GetOptValue("sqlfile"));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error " + e.GetType().ToString() + ": " + e.Message);
                System.Console.WriteLine(e.StackTrace);
                System.Console.WriteLine();
                System.Console.WriteLine("generate the SQL files to create the database in SQL");
                System.Console.WriteLine(
                    "usage: GenerateSQL -do:<operation> -dbms:<type of database system> -petraxml:<path and filename of petra.xml> -outputFile:<path and filename of the output file>");
                System.Console.WriteLine("operations available:");
                System.Console.WriteLine("  sql ");
                System.Console.WriteLine("    sample call: ");
                System.Console.WriteLine(
                    "        GenerateSQL -do:sql -dbms:postgresql -petraxml:u:/sql/datadefinition/petra.xml -outputFile:U:/setup/petra0300/petra.sql");
                System.Console.WriteLine("Available database managment systems and their code:");
                System.Console.WriteLine("  mysql (recommended)");
                System.Console.WriteLine("  postgresql (supported)");
                System.Environment.Exit(-1);
            }
        }
    }
}
