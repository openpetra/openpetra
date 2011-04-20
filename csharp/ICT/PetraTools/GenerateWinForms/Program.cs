//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Xml;
using System.Collections.Specialized;
using System.IO;
using Ict.Common;
using Ict.Common.IO; // Implicit reference
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.Winforms;

namespace GenerateWinForms
{
class Program
{
    private static void ProcessFile(string filename, string ASelectedLocalisation)
    {
        TProcessYAMLForms processor = new TProcessYAMLForms(filename, ASelectedLocalisation);

        // report is at the moment the only real different type of screen,
        // because it uses different controls
        // otherwise, the Template attribute is also quite important, because it determines which code is written
        processor.AddWriter("navigation", typeof(TWinFormsWriter));
        processor.AddWriter("edit", typeof(TWinFormsWriter));
        processor.AddWriter("dialog", typeof(TWinFormsWriter));
        processor.AddWriter("report", typeof(TWinFormsWriter));
        processor.AddWriter("browse", typeof(TWinFormsWriter));

        //processor.AddWriter("browse", typeof(TWinFormsWriter));
        // could add instead of TWinformsWriter: TGtkWriter
        processor.ProcessDocument();
    }

    public static void Main(string[] args)
    {
        try
        {
            TAppSettingsManager opts = new TAppSettingsManager(false);

            if (Directory.Exists("log"))
            {
                new TLogging("log/generatewinforms.log");
            }
            else
            {
                new TLogging("generatewinforms.log");
            }

            if (!opts.HasValue("ymlfile"))
            {
                Console.WriteLine("call: GenerateWinForms -ymlfile:c:\\test.yaml -petraxml:petra.xml -localisation:en");
                Console.Write("Press any key to continue . . . ");
                Console.ReadLine();
                Environment.Exit(-1);
                return;
            }

            // calculate ICTPath from ymlfile path
            string fullYmlfilePath = Path.GetFullPath(opts.GetValue("ymlfile")).Replace("\\", "/");

            if (!fullYmlfilePath.Contains("csharp/ICT"))
            {
                Console.WriteLine("ymlfile must be below the csharp/ICT directory");
            }

            string SelectedLocalisation = null;             // none selected by default; winforms autosize works quite well

            if (opts.HasValue("localisation"))
            {
                SelectedLocalisation = opts.GetValue("localisation");
            }

            CSParser.ICTPath = fullYmlfilePath.Substring(0, fullYmlfilePath.IndexOf("csharp/ICT") + "csharp/ICT".Length);

            TDataBinding.FPetraXMLStore = new TDataDefinitionStore();
            Console.WriteLine("parsing " + opts.GetValue("petraxml", true));
            TDataDefinitionParser parser = new TDataDefinitionParser(opts.GetValue("petraxml", true));
            parser.ParseDocument(ref TDataBinding.FPetraXMLStore, true, true);

            string ymlfileParam = opts.GetValue("ymlfile", true);

            if (ymlfileParam.Contains(","))
            {
                StringCollection collection = StringHelper.StrSplit(ymlfileParam, ",");

                foreach (string file in collection)
                {
                    ProcessFile(file, SelectedLocalisation);
                }
            }
            else if (System.IO.Directory.Exists(ymlfileParam))
            {
                foreach (string file in System.IO.Directory.GetFiles(ymlfileParam, "*.yaml"))
                {
                    // reset the dataset each time to force reload
                    TDataBinding.FDatasetTables = null;

                    // only look for main files, not language specific files (*.XY.yaml)
                    if (file[file.Length - 8] != '.')
                    {
                        Console.WriteLine("working on " + file);
                        ProcessFile(file, SelectedLocalisation);
                    }
                }
            }
            else
            {
                ProcessFile(ymlfileParam, SelectedLocalisation);
            }
        }
        catch (Exception e)
        {
            string commandline = "";

            foreach (string s in args)
            {
                commandline += s + " ";
            }

            Console.WriteLine("Problem while processing " + commandline);
            Console.WriteLine(e.GetType().ToString() + ": " + e.Message);

            if (e.InnerException != null)
            {
                Console.WriteLine(e.InnerException.GetType().ToString() + ": " + e.InnerException.Message);
            }

            // do not print a stacktrace for custom generated exception, eg. by the YML parser
            if (e.GetType() != typeof(System.Exception))
            {
                Console.WriteLine(e.StackTrace);
            }

            Environment.Exit(-1);
        }
    }
}
}