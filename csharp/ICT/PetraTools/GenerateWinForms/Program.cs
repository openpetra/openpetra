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
using Ict.Common.IO; // Implicit reference
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.Winforms;

namespace Ict.Tools.GenerateWinForms
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

        private static void DeleteGeneratedFile(string file, string generatedExtension)
        {
            string path = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file) +
                          generatedExtension;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                new TAppSettingsManager(false);

                if (Directory.Exists("log"))
                {
                    new TLogging("log/generatewinforms.log");
                }
                else
                {
                    new TLogging("generatewinforms.log");
                }

                TLogging.DebugLevel = TAppSettingsManager.GetInt16("DebugLevel", 10);

                if (!TAppSettingsManager.HasValue("op"))
                {
                    Console.WriteLine("call: GenerateWinForms -op:generate -ymlfile:c:\\test.yaml -petraxml:petra.xml -localisation:en");
                    Console.WriteLine("  or: GenerateWinForms -op:generate -ymldir:c:\\myclient -petraxml:petra.xml -localisation:en");
                    Console.WriteLine("  or: GenerateWinForms -op:clean -ymldir:c:\\myclient");
                    Console.Write("Press any key to continue . . . ");
                    Console.ReadLine();
                    Environment.Exit(-1);
                    return;
                }

                // calculate ICTPath from ymlfile path
                string fullYmlfilePath =
                    Path.GetFullPath(TAppSettingsManager.GetValue("ymlfile", TAppSettingsManager.GetValue("ymldir", false))).Replace(
                        "\\",
                        "/");

                if (!fullYmlfilePath.Contains("csharp/ICT"))
                {
                    Console.WriteLine("ymlfile must be below the csharp/ICT directory");
                }

                CSParser.ICTPath = fullYmlfilePath.Substring(0, fullYmlfilePath.IndexOf("csharp/ICT") + "csharp/ICT".Length);

                if (TAppSettingsManager.GetValue("op") == "clean")
                {
                    if (!Directory.Exists(fullYmlfilePath))
                    {
                        throw new Exception("invalid directory " + fullYmlfilePath);
                    }

                    // delete all generated files in the directory
                    foreach (string file in System.IO.Directory.GetFiles(fullYmlfilePath, "*.yaml", SearchOption.AllDirectories))
                    {
                        DeleteGeneratedFile(file, "-generated.cs");
                        DeleteGeneratedFile(file, "-generated.Designer.cs");
                        DeleteGeneratedFile(file, "-generated.resx");
                    }
                }
                else if (TAppSettingsManager.GetValue("op") == "generate")
                {
                    string SelectedLocalisation = null;         // none selected by default; winforms autosize works quite well

                    if (TAppSettingsManager.HasValue("localisation"))
                    {
                        SelectedLocalisation = TAppSettingsManager.GetValue("localisation");
                    }

                    TDataBinding.FPetraXMLStore = new TDataDefinitionStore();
                    Console.WriteLine("parsing " + TAppSettingsManager.GetValue("petraxml", true));
                    TDataDefinitionParser parser = new TDataDefinitionParser(TAppSettingsManager.GetValue("petraxml", true));
                    parser.ParseDocument(ref TDataBinding.FPetraXMLStore, true, true);

                    string ymlfileParam = TAppSettingsManager.GetValue("ymlfile", TAppSettingsManager.GetValue("ymldir", false));

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
                        string[] yamlfiles = System.IO.Directory.GetFiles(ymlfileParam, "*.yaml", SearchOption.AllDirectories);
                        // sort the files so that the deepest files are first processed,
                        // since the files higher up are depending on them
                        // eg. FinanceMain.yaml needs to check for GLBatch-generated.cs

                        List <string>yamlFilesSorted = new List <string>(yamlfiles.Length);

                        foreach (string file in yamlfiles)
                        {
                            yamlFilesSorted.Add(file);
                        }

                        yamlFilesSorted.Sort(new YamlFileOrderComparer());

                        foreach (string file in yamlFilesSorted)
                        {
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


/// <summary>
/// for sorting the yaml files
/// </summary>
    public class YamlFileOrderComparer : IComparer <string>
    {
        /// <summary>
        /// compare two file names; a file in a subdirectory comes before the file in the main directory
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>-1 if node1 depends on node2, +1 if node2 depends on node1, and 0 if they are identical</returns>
        public int Compare(string node1, string node2)
        {
            string path1 = Path.GetDirectoryName(node1);
            string path2 = Path.GetDirectoryName(node2);

            if ((path1.Length > path2.Length) && path1.StartsWith(path2))
            {
                return -1;
            }

            if ((path2.Length > path1.Length) && path2.StartsWith(path1))
            {
                return +1;
            }

            if (path1 == path2)
            {
                return 0;
            }

            return -1;
        }
    }
}