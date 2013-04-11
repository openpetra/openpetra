//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.IO;
//using System.Reflection;
//using NamespaceHierarchy;
using Ict.Common;
using Ict.Common.IO;
//using Ict.Tools.CodeGeneration;
//using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.ReferenceCountConnectors
{
    /// <summary>
    /// this class auto-generates the connectors for record reference counting functionality
    /// </summary>
    class TCreateReferenceCountConnectors
    {
        private int FTotalCacheable = 0;
        private int FTotalNonCacheable = 0;
        private int FTotalConnectors = 0;

        private Boolean CreateConnectors(String AOutputPath, String AModulePath, String ATemplateDir)
        {
            // Work out the module name from the module path
            string[] items = AModulePath.Split(new char[] { Path.DirectorySeparatorChar });

            if (items.Length == 0)
            {
                // the -inputclient command line parameter must be wrong
                return false;
            }

            // Module name is e.g. MCommon, MPartner etc
            string moduleName = items[items.Length - 1];

            // Work out the actual folder/file for the output file
            String OutputFolder = AOutputPath + Path.DirectorySeparatorChar + "lib" +
                                  Path.DirectorySeparatorChar + moduleName +
                                  Path.DirectorySeparatorChar + "web";

            if (!Directory.Exists(OutputFolder))
            {
                // The -outputserver command line parameter must be wrong
                return false;
            }

            String OutputFile = OutputFolder + Path.DirectorySeparatorChar + "ReferenceCount-generated.cs";
            Console.WriteLine("working on " + OutputFile);

            // Where is the template?
            String templateFilename = ATemplateDir +
                                      Path.DirectorySeparatorChar + "ORM" +
                                      Path.DirectorySeparatorChar + "ReferenceCountWebConnector.cs";

            if (!File.Exists(templateFilename))
            {
                // The -templatedir command line parameter must have been wrong
                return false;
            }

            // Open the template
            ProcessTemplate Template = new ProcessTemplate(templateFilename);

            // now we need to remove the leading 'M' from the module name
            moduleName = moduleName.Substring(1);
            string className = "T" + moduleName + "ReferenceCountWebConnector";

            TLogging.Log("Starting connector for " + className + Environment.NewLine);

            int cacheableCount = 0;
            int nonCacheableCount = 0;

            // load default header with license and copyright
            Template.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(ATemplateDir));
            Template.SetCodelet("TOPLEVELMODULE", moduleName);

            Template.SetCodelet("CLASSNAME", className);
            Template.SetCodelet("CACHEABLETABLECASES", string.Empty);
            Template.SetCodelet("CACHEABLETABLENAME", string.Empty);
            Template.SetCodelet("CACHEABLETABLECASE", string.Empty);
            Template.SetCodelet("CACHEABLETABLELISTNAME", string.Empty);
            Template.SetCodelet("TABLESIF", string.Empty);
            Template.SetCodelet("TABLESELSEIF", string.Empty);
            Template.SetCodelet("TABLESELSE", string.Empty);
            Template.SetCodelet("TABLENAME", string.Empty);

            // Find all the YAML files in the client module folder
            string[] clientFiles = Directory.GetFiles(AModulePath, "*.yaml", SearchOption.AllDirectories);

            foreach (String fn in clientFiles)
            {
                XmlDocument doc = TYml2Xml.CreateXmlDocument();
                SortedList sortedNodes = null;
                TCodeStorage codeStorage = new TCodeStorage(doc, sortedNodes);
                TParseYAMLFormsDefinition yamlParser = new TParseYAMLFormsDefinition(ref codeStorage);
                yamlParser.LoadRecursively(fn, null);

                string attDetailTableName = codeStorage.GetAttribute("DetailTable");
                string attCacheableListName = codeStorage.GetAttribute("CacheableTable");

                if (codeStorage.FControlList.ContainsKey("btnDelete") && (attDetailTableName != String.Empty))
                {
                    if (attCacheableListName != String.Empty)
                    {
                        ProcessTemplate snippet = Template.GetSnippet("CACHEABLETABLECASE");
                        snippet.SetCodelet("CACHEABLETABLENAME", attDetailTableName);
                        snippet.SetCodelet("CACHEABLETABLELISTNAME", attCacheableListName);
                        Template.InsertSnippet("CACHEABLETABLECASES", snippet);

                        TLogging.Log("Creating cacheable reference count connector for " + attCacheableListName);
                        cacheableCount++;
                    }
                    else
                    {
                        ProcessTemplate snippet = null;

                        if (nonCacheableCount == 0)
                        {
                            snippet = Template.GetSnippet("TABLEIF");
                            snippet.SetCodelet("TABLENAME", attDetailTableName);
                            Template.InsertSnippet("TABLESIF", snippet);
                        }
                        else
                        {
                            snippet = Template.GetSnippet("TABLEELSEIF");
                            snippet.SetCodelet("TABLENAME", attDetailTableName);
                            Template.InsertSnippet("TABLESELSEIF", snippet);
                        }

                        TLogging.Log("Creating non-cacheable reference count connector for " + attDetailTableName);
                        nonCacheableCount++;
                    }
                }
            }

            // Now we finish off the template content depending on how many entries we made
            if ((nonCacheableCount == 0) && (cacheableCount > 0))
            {
                ProcessTemplate snippet = Template.GetSnippet("TABLENONE");
                Template.InsertSnippet("TABLESELSE", snippet);
            }

            if (nonCacheableCount > 0)
            {
                ProcessTemplate snippet = Template.GetSnippet("TABLEELSE");
                Template.InsertSnippet("TABLESELSE", snippet);
            }

            if ((cacheableCount > 0) || (nonCacheableCount > 0))
            {
                TLogging.Log("Finishing connector for " + className + Environment.NewLine + Environment.NewLine);
                Template.FinishWriting(OutputFile, ".cs", true);

                FTotalCacheable += cacheableCount;
                FTotalNonCacheable += nonCacheableCount;
                FTotalConnectors++;
            }

            return true;
        }

        /// <summary>
        /// Main class method to create all the auto-generated files in server\lib\web folders
        /// </summary>
        /// <param name="AOutputPath">Absolute or relative path to ICT/Petra/server</param>
        /// <param name="AClientPath">Absolute or relative path to ICT/Petra/client</param>
        /// <param name="ATemplateDir">Absolute or relative path to inc/template/src</param>
        /// <returns>True if successful or False if an error is detected</returns>
        public bool CreateFiles(String AOutputPath, String AClientPath, String ATemplateDir)
        {
            bool returnValue = true;

            string[] allDirs = Directory.GetDirectories(Path.GetFullPath(AClientPath), "M*", SearchOption.TopDirectoryOnly);

            if (allDirs.Length == 0)
            {
                // the command line parameter must be wrong
                returnValue = false;
            }

            foreach (string dirName in allDirs)
            {
                if (!CreateConnectors(AOutputPath, dirName, ATemplateDir))
                {
                    returnValue = false;
                }
            }

            TLogging.Log("*** Total cacheable tables: " + FTotalCacheable.ToString());
            TLogging.Log("*** Total non-cacheable tables: " + FTotalNonCacheable.ToString());
            TLogging.Log("*** Total connectors: " + FTotalConnectors.ToString());

            return returnValue;
        }
    }
}