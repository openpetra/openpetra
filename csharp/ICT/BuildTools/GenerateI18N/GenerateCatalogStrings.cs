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
using System.Xml;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace GenerateI18N
{
    /// <summary>
    /// Use all Text properties from the designer file and add Catalog.SetString in the constructor
    /// </summary>
    public class TGenerateCatalogStrings
    {
        /// <summary>
        /// read the designer file and add the strings to the main file
        /// </summary>
        /// <param name="AMainFilename"></param>
        /// <param name="ADataDefinitionStore"></param>
        /// <param name="ADbHelpTranslationWriter">dummy cs file that is used to provide the strings to gettext</param>
        /// <returns>true if the file should be parsed for translatable strings</returns>
        public static bool Execute(string AMainFilename, TDataDefinitionStore ADataDefinitionStore, StreamWriter ADbHelpTranslationWriter)
        {
            string DesignerFileName = GetDesignerFilename(AMainFilename);
            StreamReader readerDesignerFile = null;
            StreamWriter writer = null;

            if (AMainFilename.EndsWith(".Designer.cs") || AMainFilename.EndsWith("AssemblyInfo.cs"))
            {
                return false;
            }

            if (File.Exists(Path.GetDirectoryName(AMainFilename) + Path.DirectorySeparatorChar +
                    System.IO.Path.GetFileNameWithoutExtension(AMainFilename.Replace("-generated", string.Empty)) + ".yaml"))
            {
                // do not generate translation code for already generated files;
                // but still let gettext parse this file for Catalog.GetString
                return true;
            }

            if (AMainFilename.Contains("-generated."))
            {
                // do not generate translation code for already generated files
                return false;
            }

            if (File.Exists(DesignerFileName))
            {
                readerDesignerFile = new StreamReader(DesignerFileName);
                writer = new StreamWriter(AMainFilename + ".new");
            }

            StreamReader readerMainFile = new StreamReader(AMainFilename);

            // find the call to InitializeComponent
            string line = "";
            bool ContainsCatalogGetStringCall = false;

            while (!readerMainFile.EndOfStream && !line.Contains("InitializeComponent();"))
            {
                line = readerMainFile.ReadLine();

                CheckLineAndAddDBHelp(line, ADataDefinitionStore, ADbHelpTranslationWriter);

                if (line.Contains("Catalog.GetString"))
                {
                    ContainsCatalogGetStringCall = true;
                }

                if (writer != null)
                {
                    writer.WriteLine(line);
                }
            }

            if (readerDesignerFile == null)
            {
                // this is just a normal code file without designer code

                if (!readerMainFile.EndOfStream)
                {
                    // TODO: be more strict with missing designer file!
                    //readerMainFile.Close();
                    //throw new Exception("the file " + AMainFilename + " should have a designer file!");
                    Console.WriteLine("the file " + AMainFilename + " should have a designer file!");
                }

                readerMainFile.Close();

                return ContainsCatalogGetStringCall;
            }

            if (readerMainFile.EndOfStream)
            {
                readerMainFile.Close();
                readerDesignerFile.Close();
                writer.Close();

                throw new Exception("Problem: cannot find InitializeComponent in " + AMainFilename);
            }

            string identation = "".PadLeft(line.IndexOf("InitializeComponent()"));

            writer.WriteLine(identation + "#region CATALOGI18N");

            // empty line for uncrustify
            writer.WriteLine();
            writer.WriteLine(
                identation + "// this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N");

            // parse the designer files and insert all labels etc into the main file
            while (!readerDesignerFile.EndOfStream)
            {
                string designerLine = readerDesignerFile.ReadLine();

                // catch all .Text = , but also TooltipsText = , but ignore lblSomethingText = new ...
                if (designerLine.Contains("Text = \""))
                {
                    bool trailingColon = false;
                    string content = designerLine.Substring(
                        designerLine.IndexOf("\"") + 1, designerLine.LastIndexOf("\"") - designerLine.IndexOf("\"") - 1);

                    if (content.EndsWith(":"))
                    {
                        trailingColon = true;
                        content = content.Substring(0, content.Length - 1);
                    }

                    // see also FormWriter.cs, SetControlProperty; it also calls ProperI18NCatalogGetString
                    try
                    {
                        if (TFormWriter.ProperI18NCatalogGetString(content))
                        {
                            writer.WriteLine(identation +
                                designerLine.Substring(0, designerLine.IndexOf(" = ")).Trim() +
                                " = Catalog.GetString(\"" + content + "\")" + (trailingColon ? " + \":\"" : string.Empty) + ";");

                            ADbHelpTranslationWriter.WriteLine("Catalog.GetString(\"" + content + "\");");
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "Problem with \\r or \\n")
                        {
                            throw new Exception("Problem with \\r or \\n in file " + DesignerFileName + ": " + designerLine);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            writer.WriteLine(identation + "#endregion");

            readerDesignerFile.Close();

            bool skip = false;

            while (!readerMainFile.EndOfStream)
            {
                line = readerMainFile.ReadLine();

                if (line.Trim().StartsWith("#region CATALOGI18N"))
                {
                    skip = true;
                }

                if (!skip)
                {
                    CheckLineAndAddDBHelp(line, ADataDefinitionStore, ADbHelpTranslationWriter);
                    writer.WriteLine(line);
                }

                if (skip && line.Trim().StartsWith("#endregion"))
                {
                    skip = false;
                }
            }

            writer.Close();
            readerMainFile.Close();

            TTextFile.UpdateFile(AMainFilename, true);

            return true;
        }

        /// <summary>
        /// get the filename of the designer file that belongs to this file;
        /// it does not check if that file actually exists
        /// </summary>
        /// <param name="AFilename"></param>
        /// <returns></returns>
        public static string GetDesignerFilename(string AFilename)
        {
            return Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar + System.IO.Path.GetFileNameWithoutExtension(AFilename) +
                   ".Designer.cs";
        }

        /// <summary>
        /// also check for .SetStatusBarText([...]Table.Get[...]Help: add text from petra.xml to a separate dummy file so that it will be picked up by gettext
        /// </summary>
        /// <param name="ALine"></param>
        /// <param name="store"></param>
        /// <param name="ADbHelpTranslationWriter">dummy cs file that is used to provide the strings to gettext</param>
        private static void CheckLineAndAddDBHelp(string ALine, TDataDefinitionStore store, StreamWriter ADbHelpTranslationWriter)
        {
            Match m = Regex.Match(ALine, ".*SetStatusBarText\\(.*, (.*)Table.Get(.*)Help\\(\\)");

            if (m.Success)
            {
                // eg FPetraUtilsObject.SetStatusBarText(txtPreferredName, PPersonTable.GetPreferedNameHelp());
                string tablename = m.Groups[1].Value;
                string columnname = m.Groups[2].Value;

                if ((store == null) || (ADbHelpTranslationWriter == null))
                {
                    Console.WriteLine("please run the usual nant translation for the whole solution so that the help text for {0}.{1} will be added.",
                        tablename, columnname);
                    return;
                }

                TTable table = store.GetTable(tablename);
                ADbHelpTranslationWriter.WriteLine("Catalog.GetString(\"" + table.GetField(columnname).strHelp + "\");");
            }
        }

        private static string GetLabelOrName(XmlNode ANode)
        {
            return TYml2Xml.HasAttribute(ANode, "Label") ? TYml2Xml.GetAttribute(ANode, "Label") : StringHelper.ReverseUpperCamelCase(ANode.Name);
        }

        /// <summary>
        /// add Catalog.GetString for each label and description in the UINavigation file to the dummy file to prepare the translation files
        /// </summary>
        /// <param name="UINavigationFilename">yml file</param>
        /// <param name="ATranslationWriter">dummy cs file that is used to provide the strings to gettext</param>
        public static void AddTranslationUINavigation(string UINavigationFilename, StreamWriter ATranslationWriter)
        {
            TYml2Xml parser = new TYml2Xml(UINavigationFilename);
            XmlDocument doc = parser.ParseYML2XML();

            XmlNode OpenPetraNode = doc.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            while (DepartmentNode != null)
            {
                ATranslationWriter.WriteLine("Catalog.GetString(\"" + GetLabelOrName(DepartmentNode) + "\");");

                XmlNode ModuleNode = DepartmentNode.FirstChild;

                while (ModuleNode != null)
                {
                    ATranslationWriter.WriteLine("Catalog.GetString(\"" + GetLabelOrName(ModuleNode) + "\");");

                    XmlNode SubModuleNode = ModuleNode.FirstChild;

                    while (SubModuleNode != null)
                    {
                        ATranslationWriter.WriteLine("Catalog.GetString(\"" + GetLabelOrName(SubModuleNode) + "\");");
                        XmlNode TaskGroupNode = SubModuleNode.FirstChild;

                        while (TaskGroupNode != null)
                        {
                            ATranslationWriter.WriteLine("Catalog.GetString(\"" + GetLabelOrName(TaskGroupNode) + "\");");
                            XmlNode TaskNode = TaskGroupNode.FirstChild;

                            while (TaskNode != null)
                            {
                                ATranslationWriter.WriteLine("Catalog.GetString(\"" + GetLabelOrName(TaskNode) + "\");");

                                if (TYml2Xml.HasAttribute(TaskNode, "Description"))
                                {
                                    ATranslationWriter.WriteLine("Catalog.GetString(\"" + TYml2Xml.GetAttribute(TaskNode, "Description") + "\");");
                                }

                                TaskNode = TaskNode.NextSibling;
                            }

                            TaskGroupNode = TaskGroupNode.NextSibling;
                        }

                        SubModuleNode = SubModuleNode.NextSibling;
                    }

                    ModuleNode = ModuleNode.NextSibling;
                }

                DepartmentNode = DepartmentNode.NextSibling;
            }
        }
    }
}