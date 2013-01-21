//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, jomammele
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
using System.Threading;
using System.Diagnostics;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;
using GenerateI18N;

namespace Ict.Tools.GenerateI18N
{
    class Program
    {
        private static void ParseWithGettext(string AGettextApp, string APoFile, string AListOfFilesToParse)
        {
            System.Diagnostics.Process GettextProcess;
            GettextProcess = new System.Diagnostics.Process();
            GettextProcess.EnableRaisingEvents = false;
            GettextProcess.StartInfo.FileName = AGettextApp;
            GettextProcess.StartInfo.Arguments = String.Format(
                "-j --add-comments=/ --add-location --from-code=UTF-8 {0} -o \"{1}\"",
                AListOfFilesToParse, APoFile);
            GettextProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            GettextProcess.EnableRaisingEvents = true;
            TLogging.Log("ParseWithGettext() Arguments: " + GettextProcess.StartInfo.Arguments);
            try
            {
                if (!GettextProcess.Start())
                {
                    throw new Exception("cannot start gettext");
                }
            }
            catch (Exception)
            {
                TLogging.Log("Cannot start external gettext program. Is it on the path?");
                TLogging.Log("Arguments: " + GettextProcess.StartInfo.Arguments);
                throw new Exception("Problem running gettext");
            }

            while ((!GettextProcess.HasExited))
            {
                Thread.Sleep(100);
            }
        }

        public static void Main(string[] args)
        {
            new TAppSettingsManager(false);

            //enable execution without nant -> debugging easier
            bool independentmode = false;

            if (independentmode)
            {
                TLogging.Log("started in independent-mode");
            }

            try
            {
                if ((TAppSettingsManager.HasValue("do") && (TAppSettingsManager.GetValue("do") == "removeDoNotTranslate")) || independentmode)
                {
                    string doNotTranslatePath;
                    string poFilePath;

                    if (independentmode)
                    {
                        doNotTranslatePath = "E:\\openpetra\\bzr\\work-690\\i18n\\doNotTranslate.po";
                        poFilePath = "E:\\openpetra\\bzr\\work-690\\i18n\\template.pot";
                    }
                    else
                    {
                        doNotTranslatePath = TAppSettingsManager.GetValue("dntFile");
                        poFilePath = TAppSettingsManager.GetValue("poFile");
                    }

                    // remove all strings from po file that are listed in the "Do Not Translate" file
                    TDropUnwantedStrings.RemoveUnwantedStringsFromTranslation(doNotTranslatePath, poFilePath);
                }
                else if (TAppSettingsManager.HasValue("do") && (TAppSettingsManager.GetValue("do") == "yamlfiles"))
                {
                    GenerateYamlFiles.WriteYamlFiles(
                        TAppSettingsManager.GetValue("language"),
                        TAppSettingsManager.GetValue("path"),
                        TAppSettingsManager.GetValue("pofile"));
                }
                else if (TAppSettingsManager.HasValue("file"))
                {
                    TGenerateCatalogStrings.Execute(TAppSettingsManager.GetValue("file"), null, null);
                }
                else if (TAppSettingsManager.HasValue("filelist"))
                {
                    TDataDefinitionStore store = new TDataDefinitionStore();
                    Console.WriteLine("parsing " + TAppSettingsManager.GetValue("petraxml", true));
                    TDataDefinitionParser parser = new TDataDefinitionParser(TAppSettingsManager.GetValue("petraxml", true));
                    parser.ParseDocument(ref store, true, true);

                    string CollectedStringsFilename = TAppSettingsManager.GetValue("tmpPath") +
                                                      Path.DirectorySeparatorChar +
                                                      "GenerateI18N.CollectedGettext.cs";
                    StreamWriter writerCollectedStringsFile = new StreamWriter(CollectedStringsFilename);

                    string GettextApp = TAppSettingsManager.GetValue("gettext");

                    string filesToParseWithGettext = string.Empty;
                    StreamReader readerFilelist = new StreamReader(TAppSettingsManager.GetValue("filelist"));

                    while (!readerFilelist.EndOfStream)
                    {
                        string pathCodeFile = readerFilelist.ReadLine().Trim();
                        string ext = Path.GetExtension(pathCodeFile);

                        if (".cs" == ext)
                        {
                            if (TGenerateCatalogStrings.Execute(pathCodeFile, store, writerCollectedStringsFile))
                            {
                                filesToParseWithGettext += "\"" + pathCodeFile + "\" ";

                                if (filesToParseWithGettext.Length > 1500)
                                {
                                    ParseWithGettext(GettextApp, TAppSettingsManager.GetValue("poFile"), filesToParseWithGettext);
                                    filesToParseWithGettext = string.Empty;
                                }
                            }
                        }
                        else if (".yml" == ext)
                        {
                            TGenerateCatalogStrings.AddTranslationUINavigation(pathCodeFile, writerCollectedStringsFile);
                        }
                        else
                        {
                            Console.WriteLine("the file " + pathCodeFile + " has an unknown extension! File ignored!");
                        }
                    }

                    if (filesToParseWithGettext.Length > 0)
                    {
                        ParseWithGettext(GettextApp, TAppSettingsManager.GetValue("poFile"), filesToParseWithGettext);
                    }

                    writerCollectedStringsFile.Close();

                    // delete the file if it is empty
                    if (File.ReadAllText(CollectedStringsFilename).Length == 0)
                    {
                        File.Delete(CollectedStringsFilename);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.Exit(-1);
            }
        }
    }
}