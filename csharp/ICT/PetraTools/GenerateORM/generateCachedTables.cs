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
using System.Collections;
using System.IO;
using System.Xml;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.CachedTables
{
    /// <summary>
    /// creates a file with enums in Shared and one file per submodule in Server for cached tables
    /// </summary>
    public class TGenerateCachedTables
    {
        /// creates a file with enums in Shared and one file per submodule in Server for cached tables
        public static void WriteCachedTables(TDataDefinitionStore AStore,
            string ACacheYamlFilename,
            string ASharedPath,
            string ATemplateDir)
        {
            // Load yaml file with list of tables that should be cached
            TYml2Xml ymlParser = new TYml2Xml(ACacheYamlFilename);
            XmlDocument xmlDoc = ymlParser.ParseYML2XML();

            XmlNode module = xmlDoc.DocumentElement.FirstChild.FirstChild;

            while (module != null)
            {
                XmlNode subModule = module.FirstChild;
                bool severalSubModules = (subModule != null && subModule.NextSibling != null);

                // write the shared file with the enum definitions
                ProcessTemplate SharedTemplate = new ProcessTemplate(ATemplateDir + Path.DirectorySeparatorChar +
                    "ORM" + Path.DirectorySeparatorChar +
                    "Cacheable.Shared.cs");

                SharedTemplate.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(ATemplateDir));
                SharedTemplate.SetCodelet("NAMESPACE", "Ict.Petra.Shared.M" + module.Name);

                while (subModule != null)
                {
                    // write the server file for each submodule
                    ProcessTemplate ServerTemplate = new ProcessTemplate(ATemplateDir + Path.DirectorySeparatorChar +
                        "ORM" + Path.DirectorySeparatorChar +
                        "Cacheable.Server.cs");

                    ServerTemplate.SetCodelet("GPLFILEHEADER", ProcessTemplate.LoadEmptyFileComment(ATemplateDir));
                    ServerTemplate.SetCodelet("NAMESPACE", "Ict.Petra.Server.M" + module.Name + "." + subModule.Name + ".Cacheable");
                    ServerTemplate.SetCodelet("SUBNAMESPACE", "M" + module.Name + "." + subModule.Name);
                    ServerTemplate.SetCodelet("CACHEABLECLASS", "T" + module.Name + "Cacheable");
                    ServerTemplate.SetCodelet("SUBMODULE", subModule.Name);
                    ServerTemplate.SetCodelet("GETCALCULATEDLISTFROMDB", "");
                    ServerTemplate.SetCodelet("LEDGERGETCACHEABLE", "");
                    ServerTemplate.SetCodelet("LEDGERSAVECACHEABLE", "");

                    if (!severalSubModules)
                    {
                        // for MCommon
                        ServerTemplate.SetCodelet("NAMESPACE", "Ict.Petra.Server.M" + module.Name + ".Cacheable");
                        ServerTemplate.SetCodelet("SUBNAMESPACE", "M" + module.Name);
                        ServerTemplate.SetCodelet("CACHEABLECLASS", "TCacheable");
                    }

                    ProcessTemplate snippetSubmodule = SharedTemplate.GetSnippet("SUBMODULEENUM");
                    snippetSubmodule.SetCodelet("SUBMODULE", subModule.Name);
                    snippetSubmodule.SetCodelet("MODULE", module.Name);

                    ProcessTemplate snippetLedgerGetTable = null;
                    ProcessTemplate snippetLedgerSaveTable = null;

                    XmlNode TableOrListElement = subModule.FirstChild;

                    while (TableOrListElement != null)
                    {
                        XmlNode enumElement = TableOrListElement.FirstChild;

                        while (enumElement != null)
                        {
                            bool DependsOnLedger = false;

                            if (TYml2Xml.GetAttributeRecursive(enumElement, "DependsOnLedger") == "true")
                            {
                                if (snippetLedgerGetTable == null)
                                {
                                    snippetLedgerGetTable = ServerTemplate.GetSnippet("LEDGERGETCACHEABLE");
                                    snippetLedgerGetTable.SetCodelet("SUBMODULE", subModule.Name);
                                }

                                if ((snippetLedgerSaveTable == null) && (TableOrListElement.Name == "DatabaseTables"))
                                {
                                    snippetLedgerSaveTable = ServerTemplate.GetSnippet("LEDGERSAVECACHEABLE");
                                    snippetLedgerSaveTable.SetCodelet("SUBMODULE", subModule.Name);
                                }

                                DependsOnLedger = true;
                            }

                            ProcessTemplate snippetElement = SharedTemplate.GetSnippet("ENUMELEMENT");

                            if ((enumElement.NextSibling == null)
                                && ((TableOrListElement.NextSibling == null) || (TableOrListElement.NextSibling.FirstChild == null)))
                            {
                                snippetElement = SharedTemplate.GetSnippet("ENUMELEMENTLAST");
                            }

                            string Comment = TXMLParser.GetAttribute(enumElement, "Comment");

                            if ((Comment.Length == 0) && (TableOrListElement.Name == "DatabaseTables"))
                            {
                                TTable Table = AStore.GetTable(enumElement.Name);

                                if (Table == null)
                                {
                                    throw new Exception("Error: cannot find table " + enumElement.Name + " for caching in module " + module.Name);
                                }

                                Comment = Table.strDescription;
                            }

                            if (Comment.Length == 0)
                            {
                                Comment = "todoComment";
                            }

                            snippetElement.SetCodelet("ENUMCOMMENT", Comment);

                            string enumName = enumElement.Name;

                            if (TXMLParser.HasAttribute(enumElement, "Enum"))
                            {
                                enumName = TXMLParser.GetAttribute(enumElement, "Enum");
                            }
                            else if (TableOrListElement.Name == "DatabaseTables")
                            {
                                string character2 = enumElement.Name.Substring(1, 1);

                                if (character2.ToLower() == character2)
                                {
                                    // this is a table name that has a 2 digit prefix
                                    enumName = enumElement.Name.Substring(2) + "List";
                                }
                                else
                                {
                                    enumName = enumElement.Name.Substring(1) + "List";
                                }
                            }

                            snippetElement.SetCodelet("ENUMNAME", enumName);

                            snippetSubmodule.InsertSnippet("ENUMELEMENTS", snippetElement);

                            if (TableOrListElement.Name == "DatabaseTables")
                            {
                                ProcessTemplate snippetLoadTable = ServerTemplate.GetSnippet("LOADTABLE");

                                if (DependsOnLedger)
                                {
                                    snippetLoadTable = ServerTemplate.GetSnippet("LOADTABLEVIALEDGER");
                                }

                                snippetLoadTable.SetCodelet("ENUMNAME", enumName);
                                snippetLoadTable.SetCodelet("DATATABLENAME", enumElement.Name);

                                if (DependsOnLedger)
                                {
                                    snippetLedgerGetTable.InsertSnippet("LOADTABLESANDLISTS", snippetLoadTable);
                                }
                                else
                                {
                                    ServerTemplate.InsertSnippet("LOADTABLESANDLISTS", snippetLoadTable);
                                }

                                ProcessTemplate snippetSaveTable = ServerTemplate.GetSnippet("SAVETABLE");
                                snippetSaveTable.SetCodelet("ENUMNAME", enumName);
                                snippetSaveTable.SetCodelet("SUBMODULE", subModule.Name);
                                snippetSaveTable.SetCodelet("DATATABLENAME", enumElement.Name);

                                if (DependsOnLedger)
                                {
                                    snippetLedgerSaveTable.InsertSnippet("SAVETABLE", snippetSaveTable);
                                }
                                else
                                {
                                    ServerTemplate.InsertSnippet("SAVETABLE", snippetSaveTable);
                                }
                            }
                            else
                            {
                                ProcessTemplate snippetLoadList = ServerTemplate.GetSnippet("LOADCALCULATEDLIST");

                                if (DependsOnLedger)
                                {
                                    snippetLoadList = ServerTemplate.GetSnippet("LOADCALCULATEDLISTFORLEDGER");
                                }

                                snippetLoadList.SetCodelet("ENUMNAME", enumName);
                                snippetLoadList.SetCodelet("CALCULATEDLISTNAME", enumName);

                                if (DependsOnLedger)
                                {
                                    snippetLedgerGetTable.InsertSnippet("LOADTABLESANDLISTS", snippetLoadList);
                                }
                                else
                                {
                                    ServerTemplate.InsertSnippet("LOADTABLESANDLISTS", snippetLoadList);
                                }

                                ProcessTemplate snippetManualCodeFunction = ServerTemplate.GetSnippet("GETCALCULATEDLISTFROMDB");

                                if (DependsOnLedger)
                                {
                                    snippetManualCodeFunction = ServerTemplate.GetSnippet("GETCALCULATEDLISTLEDGERFROMDB");
                                }

                                snippetManualCodeFunction.SetCodelet("CALCULATEDLISTNAME", enumName);
                                ServerTemplate.InsertSnippet("GETCALCULATEDLISTFROMDB", snippetManualCodeFunction);
                            }

                            enumElement = enumElement.NextSibling;

                            if (enumElement != null)
                            {
                                snippetSubmodule.AddToCodelet("ENUMELEMENTS", Environment.NewLine);
                            }
                        }

                        TableOrListElement = TableOrListElement.NextSibling;
                    }

                    SharedTemplate.InsertSnippet("ENUMS", snippetSubmodule);

                    if (snippetLedgerGetTable != null)
                    {
                        ServerTemplate.InsertSnippet("LEDGERGETCACHEABLE", snippetLedgerGetTable);
                    }

                    if (snippetLedgerSaveTable != null)
                    {
                        ServerTemplate.InsertSnippet("LEDGERSAVECACHEABLE", snippetLedgerSaveTable);
                    }

                    string path = ASharedPath +
                                  Path.DirectorySeparatorChar + ".." +
                                  Path.DirectorySeparatorChar + "Server" +
                                  Path.DirectorySeparatorChar + "lib" +
                                  Path.DirectorySeparatorChar + "M" + module.Name +
                                  Path.DirectorySeparatorChar;

                    if (File.Exists(path + "Cacheable.cs"))
                    {
                        path += "Cacheable.cs";
                    }
                    else
                    {
                        if (File.Exists(path + "data" + Path.DirectorySeparatorChar + subModule.Name + "." + "Cacheable.cs"))
                        {
                            path += "data" + Path.DirectorySeparatorChar + subModule.Name + "." + "Cacheable.cs";
                        }
                        else if (File.Exists(path + "data" + Path.DirectorySeparatorChar + "Cacheable.cs"))
                        {
                            path += "data" + Path.DirectorySeparatorChar + "Cacheable.cs";
                        }
                        else
                        {
                            path += subModule.Name + "." + "Cacheable.cs";
                        }
                    }

                    ServerTemplate.FinishWriting(path, ".cs", true);

                    subModule = subModule.NextSibling;
                }

                SharedTemplate.FinishWriting(ASharedPath +
                    Path.DirectorySeparatorChar + "lib" +
                    Path.DirectorySeparatorChar + "M" + module.Name +
                    Path.DirectorySeparatorChar + "Cacheable.cs",
                    ".cs", true);

                module = module.NextSibling;
            }
        }
    }
}