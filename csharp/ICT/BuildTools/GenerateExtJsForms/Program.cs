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
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using Ict.Common;
using Ict.Common.IO; // Implicit referemce
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.ExtJs;

namespace Ict.Tools.GenerateExtJsForms
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
            List <string>FilesToProcess = new List <string>();
            List <string>AbstractFiles = new List <string>();

            foreach (string file in System.IO.Directory.GetFiles(ADirName, "*.yaml"))
            {
                string baseyaml;

                if (TYml2Xml.ReadHeader(file, out baseyaml))
                {
                    if (!AbstractFiles.Contains(baseyaml))
                    {
                        AbstractFiles.Add(baseyaml);

                        if (FilesToProcess.Contains(baseyaml))
                        {
                            FilesToProcess.Remove(baseyaml);
                        }
                    }

                    if (!AbstractFiles.Contains(Path.GetFileName(file)) && !FilesToProcess.Contains(Path.GetFileName(file)))
                    {
                        FilesToProcess.Add(Path.GetFileName(file));
                    }
                }
            }

            foreach (string file in FilesToProcess)
            {
                Console.WriteLine("working on " + file);
                ProcessFile(ADirName + Path.DirectorySeparatorChar + file, ASelectedLocalisation);
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
                new TAppSettingsManager(false);

                if (Directory.Exists(TAppSettingsManager.GetValue("logPath")))
                {
                    new TLogging(TAppSettingsManager.GetValue("logPath") + "/generateextjsforms.log");
                }
                else
                {
                    new TLogging("generateextjsforms.log");
                }

                TLogging.DebugLevel = TAppSettingsManager.GetInt16("DebugLevel", 0);

                if (!TAppSettingsManager.HasValue("ymlfile"))
                {
                    Console.WriteLine("call: GenerateExtJsForms -ymlfile:c:\\test.yaml -petraxml:petra.xml -localisation:en");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadLine();
                    Environment.Exit(-1);
                    return;
                }

                // calculate ICTPath from ymlfile path
                string fullYmlfilePath = Path.GetFullPath(TAppSettingsManager.GetValue("ymlfile")).Replace("\\", "/");

                string SelectedLocalisation = null;         // none selected by default

                if (TAppSettingsManager.HasValue("localisation"))
                {
                    SelectedLocalisation = TAppSettingsManager.GetValue("localisation");
                }

                CSParser.ICTPath = fullYmlfilePath.Substring(0, fullYmlfilePath.IndexOf("csharp/ICT") + "csharp/ICT".Length);

                TDataBinding.FPetraXMLStore = new TDataDefinitionStore();
                Console.WriteLine("parsing " + TAppSettingsManager.GetValue("petraxml", true));
                TDataDefinitionParser parser = new TDataDefinitionParser(TAppSettingsManager.GetValue("petraxml", true));
                parser.ParseDocument(ref TDataBinding.FPetraXMLStore, true, true);

                string ymlfileParam = TAppSettingsManager.GetValue("ymlfile", true);

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
                if ((e.GetType() != typeof(System.Exception)) || (TLogging.DebugLevel > 0))
                {
                    Console.WriteLine(e.StackTrace);
                }

                Environment.Exit(-1);
            }
        }
    }
}