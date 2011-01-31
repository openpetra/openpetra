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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;

namespace FixProjectFiles
{
/// <summary>
/// fix the project GUIDs and references, using the GUID from the sln files
/// </summary>
public class TFixProjectReferences : TCSProjTools
{
    /// <summary>
    /// all the projects GUIDs loaded from the solution files
    /// </summary>
    private SortedList <string, string>FProjectGUIDs = new SortedList <string, string>();

    protected override bool ProcessProjectDetails(string AProjectName, string ARelativePath, string AProjectGUID, string ASolutionFile)
    {
        if (!FProjectGUIDs.ContainsKey(AProjectName))
        {
            FProjectGUIDs.Add(AProjectName, AProjectGUID);
            return true;
        }
        else
        {
            // some projects are part of several solutions; just make sure they have the same GUID
            if (FProjectGUIDs[AProjectName] != AProjectGUID)
            {
                throw new Exception("problem: wrong GUID for " + AProjectName + " in " + ASolutionFile);
            }

            // not a new project, don't run operations on it twice
            return false;
        }
    }

    /// <summary>
    /// fix the GUID and all the assembly references of the given file;
    /// using data from FProjectGUIDs
    /// </summary>
    /// <param name="AFilename">the project file to fix</param>
    public void FixProjectFile(string AFilename)
    {
        XmlDocument doc = new XmlDocument();

        Console.WriteLine(AFilename);
        StreamReader sr = new StreamReader(AFilename);
        string text = sr.ReadToEnd();
        sr.Close();
        doc.LoadXml(text);

        // load from string, to avoid exception about missing BOM
        //doc.Load(AFilename);

        bool firstPropertyGroup = true;

        string xmlns = "http://schemas.microsoft.com/developer/msbuild/2003"; // doc.Attributes["xmlns"].Value;
        string BaseIntermediateOutputPath = String.Empty;

        foreach (XmlNode child in doc.DocumentElement)
        {
            if (child.Name == "PropertyGroup")
            {
                bool containsTargetPlatform = false;
                bool containsIntermediateOutputPath = false;
                string OutputPath = String.Empty;

                foreach (XmlNode child2 in child.ChildNodes)
                {
                    if (child2.Name == "ProjectGuid")
                    {
                        if (child2.InnerText != FProjectGUIDs[System.IO.Path.GetFileNameWithoutExtension(AFilename)])
                        {
                            Console.WriteLine("fixing " + child2.InnerText);
                            child2.InnerText = FProjectGUIDs[System.IO.Path.GetFileNameWithoutExtension(AFilename)];
                        }
                    }

                    if (child2.Name == "TargetFrameworkVersion")
                    {
                        containsTargetPlatform = true;

                        if (TXMLParser.GetChild(child, "ApplicationManifest") != null)
                        {
                            // to avoid problems with Win7 thinking the program program requires elevated privileges
                            // see also http://laputa.sharpdevelop.net/EmbeddingManifests.aspx
                            // we need that at least for the PatchTool
                            child2.InnerText = "v3.5";
                        }
                        else
                        {
                            child2.InnerText = "v2.0";
                        }
                    }

                    if (child2.Name == "IntermediateOutputPath")
                    {
                        containsIntermediateOutputPath = true;

                        // it seems we get problems with "unreferenced" resource files.
                        // looking at such a dll with Reflector, there is a strange relative path to the resource file.
                        // the trailing backslash prevents this
                        if (!child2.InnerText.EndsWith("\\") && AFilename.Contains("/Client/"))
                        {
                            child2.InnerText = child2.InnerText.Substring(0, child2.InnerText.Length - 1) + "\\";
                        }
                    }

                    if ((child2.Name == "BaseIntermediateOutputPath") || (child2.Name == "IntermediateOutputPath"))
                    {
                        child2.InnerText = child2.InnerText.Replace("/", "\\");
                        child2.InnerText = child2.InnerText.Replace("Objcode", "ObjCode");
                        child2.InnerText = child2.InnerText.Replace("objcode", "ObjCode");
                        child2.InnerText = child2.InnerText.Replace("ObjCode\\ObjCode", "ObjCode");

                        if (!child2.InnerText.EndsWith("\\"))
                        {
                            child2.InnerText += "\\";
                        }

                        if (!child2.InnerText.EndsWith("\\ObjCode\\"))
                        {
                            if (child2.InnerText.EndsWith("Debug\\obj\\"))
                            {
                                child2.InnerText = child2.InnerText.Replace("Debug\\obj\\", "ObjCode\\");
                            }

                            if (child2.InnerText.EndsWith("Debug\\"))
                            {
                                child2.InnerText = child2.InnerText.Replace("Debug\\", "ObjCode\\");
                            }

                            if (child2.InnerText.EndsWith("Release\\"))
                            {
                                child2.InnerText = child2.InnerText.Replace("Release\\", "ObjCode\\");
                            }

                            if (child2.InnerText.EndsWith("_bin\\"))
                            {
                                child2.InnerText = child2.InnerText.Replace("_bin\\", "_bin\\ObjCode\\");
                            }

                            if (child2.InnerText.EndsWith("Server_Client\\"))
                            {
                                child2.InnerText = child2.InnerText.Replace("Server_Client\\", "Server_Client\\ObjCode\\");
                            }
                        }

                        if (!child2.InnerText.EndsWith("\\ObjCode\\"))
                        {
                            Console.WriteLine("WARNING: " + child2.Name + " is non standard: " + child2.InnerText);
                        }

                        if (child2.Name == "BaseIntermediateOutputPath")
                        {
                            BaseIntermediateOutputPath = child2.InnerText.Replace("/", "\\");
                        }
                        else
                        {
                            if (child2.InnerText.Replace("Debug", "ObjCode").Replace("Release", "ObjCode").Replace("/",
                                    "\\") != BaseIntermediateOutputPath)
                            {
                                Console.WriteLine("ERROR: Problem with " + child2.Name + " " + child2.InnerText + " in " +
                                    TXMLParser.GetAttribute(child, "Condition"));
                            }
                        }
                    }

                    if (child2.Name == "OutputPath")
                    {
                        child2.InnerText = child2.InnerText.Replace("/", "\\");

                        if (!child2.InnerText.EndsWith("\\"))
                        {
                            child2.InnerText += "\\";
                        }

                        OutputPath = child2.InnerText;

                        if (OutputPath.Replace("Debug", "ObjCode").Replace("Release", "ObjCode").Replace("/", "\\") != BaseIntermediateOutputPath)
                        {
                            Console.WriteLine("ERROR: Problem with " + child2.Name + " " + OutputPath + " in " +
                                TXMLParser.GetAttribute(child, "Condition"));
                        }
                    }

                    if (child2.Name == "SourceAnalysisOverrideSettingsFile")
                    {
                        child.RemoveChild(child2);
                    }
                }

                if (!containsIntermediateOutputPath && TXMLParser.GetAttribute(child, "Condition").Contains("Configuration"))
                {
                    Console.WriteLine("Fixing: PropertyGroup " + TXMLParser.GetAttribute(child,
                            "Condition") + " does not contain IntermediateOutputPath");
                    XmlNode newIntermediateOutputPath = child.OwnerDocument.CreateElement("IntermediateOutputPath", xmlns);
                    newIntermediateOutputPath.InnerText = OutputPath.Replace("Debug", "ObjCode").Replace("Release", "ObjCode");
                    child.AppendChild(newIntermediateOutputPath);
                }

                if (firstPropertyGroup && !containsTargetPlatform)
                {
                    XmlNode targetPlatform = doc.CreateElement("TargetFrameworkVersion", xmlns);
                    targetPlatform.InnerText = "v2.0";
                    child.AppendChild(targetPlatform);
                }

                if (TXMLParser.GetAttribute(child, "Condition") == " '$(Configuration)' == 'Debug' ")
                {
                    XmlNode constants = TXMLParser.GetChild(child, "DefineConstants");

                    if (constants == null)
                    {
                        constants = doc.CreateElement("DefineConstants", xmlns);
                    }

                    constants.InnerText = "DEBUG;TRACE;DEBUGMODE";
                }

                firstPropertyGroup = false;
            }

            if (child.Name == "Import")
            {
                if (child.Attributes["Project"].Value.Contains("SharpDevelopBinPath"))
                {
                    Console.WriteLine(
                        "PROBLEM: please use MSBuildBinPath Microsoft.CSharp.Targets instead of SharpDevelopBinPath) SharpDevelop.Build.CSharp.targets");
                }
            }

            if (child.Name == "ItemGroup")
            {
                foreach (XmlNode child2 in child.ChildNodes)
                {
                    if (child2.Name == "ProjectReference")
                    {
                        foreach (XmlNode child3 in child2.ChildNodes)
                        {
                            if (child3.Name == "Project")
                            {
                                string referencedProject =
                                    System.IO.Path.GetFileNameWithoutExtension(child2.Attributes["Include"].Value.Replace('\\',
                                            Path.DirectorySeparatorChar));

                                if (!FProjectGUIDs.ContainsKey(referencedProject))
                                {
                                    Console.WriteLine("missing referencedProject " + referencedProject);
                                }

                                if (!File.Exists(Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar + child2.Attributes["Include"].Value))
                                {
                                    Console.WriteLine(Path.GetFullPath(Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar +
                                            child2.Attributes["Include"].Value) + " does not exist");
                                    throw new Exception("problem with referenced dll");
                                }

                                if (child3.InnerText != FProjectGUIDs[referencedProject])
                                {
                                    Console.WriteLine("fixing " + child3.InnerText);
                                    child3.InnerText = FProjectGUIDs[referencedProject];
                                }
                            }
                        }
                    }
                    else if (child2.Name == "Reference")
                    {
                        // check whether we should have a ProjectReference instead to help with the build order
                        string referencedDll = child2.Attributes["Include"].Value;

                        // remove any specific version information. eg <Reference Include="Ict.Common, Version=0.0.9.0, Culture=neutral, PublicKeyToken=null">
                        if (referencedDll.Contains(","))
                        {
                            referencedDll = referencedDll.Substring(0, referencedDll.IndexOf(","));
                            child2.Attributes["Include"].Value = referencedDll;
                        }

                        if (referencedDll == "Mono.Posix")
                        {
                            child2.Attributes["Include"].Value = "GNU.Gettext";
                            referencedDll = child2.Attributes["Include"].Value;
                        }

                        if ((child2.FirstChild != null) && (child2.FirstChild.Name == "HintPath"))
                        {
                            string hintPath = child2.FirstChild.InnerText;

                            if (hintPath.Contains("Mono.Posix.dll"))
                            {
                                hintPath = hintPath.Replace("Mono\\Mono.Posix.dll", "GNU\\GNU.Gettext.dll").
                                           Replace("Mono/Mono.Posix.dll", "GNU/GNU.Gettext.dll");
                                child2.FirstChild.InnerText = hintPath;
                            }

                            if (hintPath.Contains("csharp"))
                            {
                                Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                                throw new Exception("no absolute paths, no paths containing csharp or openpetraorg");
                            }
                            else if (hintPath.Contains("..\\_bin")
                                     || hintPath.Contains("\\csharp\\ICT\\"))
                            {
                                Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                            }
                            else
                            {
                                // more complicated case:
                                // eg for a project in Shared, which refers to ..\..\..\..\Shared\_bin\Server_Client\Debug\Ict.Petra.Shared.MCommon.DataTables.dll

                                if (hintPath.StartsWith(".."))
                                {
                                    hintPath = Path.GetFullPath(Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar + hintPath);
                                }

                                if ((hintPath.IndexOf("_bin") > -1) && AFilename.StartsWith(hintPath.Substring(0, hintPath.IndexOf("_bin"))))
                                {
                                    Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                                }
                            }

                            // check if the path exists
                            // this currently does not work before any dlls have been compiled
                            if (!Directory.Exists(Path.GetDirectoryName(hintPath)))
                            {
                                Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                                Console.WriteLine("directory does not exist: " + Path.GetDirectoryName(Path.GetFullPath(hintPath)));
//                                throw new Exception("path error");
                            }
                        }
                    }
                    else if (child2.Name == "Compile")
                    {
                        // check if the file actually exists, with the same case sensitive name
                        string sourceFile =
                            Path.GetFullPath(Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar + child2.Attributes["Include"].Value);

                        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(sourceFile));
                        FileInfo[] files = di.GetFiles();

                        bool found = false;

                        foreach (FileInfo f in files)
                        {
                            if (f.Name == Path.GetFileName(sourceFile))
                            {
                                found = true;
                            }
                            else if (f.Name.ToLower() == Path.GetFileName(sourceFile).ToLower())
                            {
                                throw new Exception("problem with case sensitivity of source file " + child2.Attributes["Include"].Value);
                            }
                        }

                        if (!found)
                        {
                            throw new Exception("cannot find file " + child2.Attributes["Include"].Value + " looking in " + sourceFile);
                        }
                    }
                }
            }
        }

        string xmlString = TXMLParser.XmlToStringIndented(doc);

        if (!xmlString.Contains("'$(Platform)' == 'x86'")
            || !xmlString.Contains(">x86</Platform>")
            || !xmlString.Contains(">x86</PlatformTarget>"))
        {
            // see also https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=116
            xmlString = xmlString.Replace(">AnyCPU</Platform>", ">x86</Platform>");
            xmlString = xmlString.Replace(">AnyCPU</PlatformTarget>", ">x86</PlatformTarget>");
            xmlString = xmlString.Replace("'$(Platform)' == 'AnyCPU'", "'$(Platform)' == 'x86'");
        }

        // see also discussions at http://community.sharpdevelop.net/forums/p/1225/6962.aspx#6962
        // about UTF8 with BOM, MonoDevelop stores without BOM.
        StreamWriter sw = new StreamWriter(AFilename + ".new", false, new UTF8Encoding(true));
        sw.Write(xmlString);
        sw.Close();

        TTextFile.Unix2Dos(AFilename + ".new");
        TTextFile.UpdateFile(AFilename);
    }

    /// <summary>
    /// fix all project files in the solution
    /// </summary>
    /// <param name="ASolutionFile"></param>
    public void FixAllProjectFiles(string ASolutionFile)
    {
        StringCollection ProjectFiles = LoadGUIDsFromSolution(ASolutionFile);

        foreach (string ProjectFile in ProjectFiles)
        {
            FixProjectFile(ProjectFile);
        }
    }
}
}