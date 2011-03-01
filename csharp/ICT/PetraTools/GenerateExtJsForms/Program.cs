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
using Ict.Common.IO; // Implicit referemce
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.ExtJs;

namespace GenerateExtJsForms
{
class Program
{
    private static void ProcessFile(string filename, string ASelectedLocalisation)
    {
        if (ASelectedLocalisation == null)
        {
            // check for all existing localisations
            foreach (string file in System.IO.Directory.GetFiles(
                         Path.GetDirectoryName(filename),
                         "*.yaml"))
            {
                if (!file.EndsWith(Path.GetFileName(filename))
                    && Path.GetFileName(file).StartsWith(Path.GetFileNameWithoutExtension(filename)))
                {
                    ASelectedLocalisation = Path.GetExtension(Path.GetFileNameWithoutExtension(file)).Substring(1);

                    TProcessYAMLForms processorLocalized = new TProcessYAMLForms(file, null);

                    processorLocalized.AddWriter("SubmitForm", typeof(TExtJsFormsWriter));

                    processorLocalized.ProcessDocument();
                }
            }

            if (ASelectedLocalisation != null)
            {
                // do not generate the root yaml file
                return;
            }
        }

        // by default, just generate the form for one (or default) localisation
        TProcessYAMLForms processor = new TProcessYAMLForms(filename, ASelectedLocalisation);

        processor.AddWriter("SubmitForm", typeof(TExtJsFormsWriter));

        processor.ProcessDocument();
    }

    private static void ProcessDirectory(string ADirName, string ASelectedLocalisation)
    {
        foreach (string file in System.IO.Directory.GetFiles(ADirName, "*.yaml"))
        {
            // reset the dataset each time to force reload
            TDataBinding.FDatasetTables = null;

            // only look for main files, not language specific files (*.XY.yaml or *.xy-xy.yaml")
            if ((file[file.Length - 8] != '.') && (file[file.Length - 8] != '-'))
            {
                Console.WriteLine("working on " + file);
                ProcessFile(file, ASelectedLocalisation);
            }
        }

        foreach (string subdir in System.IO.Directory.GetDirectories(ADirName))
        {
            ProcessDirectory(subdir, ASelectedLocalisation);
        }
    }

    public static void Main(string[] args)
    {
        try
        {
            TAppSettingsManager opts = new TAppSettingsManager(false);

            if (Directory.Exists("log"))
            {
                new TLogging("log/generateextjsforms.log");
            }
            else
            {
                new TLogging("generateextjsforms.log");
            }

            if (!opts.HasValue("ymlfile"))
            {
                Console.WriteLine("call: GenerateExtJsForms -ymlfile:c:\\test.yaml -petraxml:petra.xml -localisation:en");
                Console.Write("Press any key to continue . . . ");
                Console.ReadLine();
                Environment.Exit(-1);
                return;
            }

            // calculate ICTPath from ymlfile path
            string fullYmlfilePath = Path.GetFullPath(opts.GetValue("ymlfile")).Replace("\\", "/");

            if (!fullYmlfilePath.Contains("webserver/"))
            {
                Console.WriteLine("ymlfile must be below the webserver directory");
            }

            string SelectedLocalisation = null;             // none selected by default

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
                ProcessDirectory(ymlfileParam, SelectedLocalisation);
            }
            else
            {
                ProcessFile(ymlfileParam, SelectedLocalisation);
            }

            // TODO: generate localised versions
            // TODO: generate minified version. either using YUICompressor, or simple string operation?
            // Or should the xsp server do that? generate js files on the fly? figure out the language of the client? cache files?
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