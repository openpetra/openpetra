/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Xml;
using System.IO;
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
        doc.Load(AFilename);

        bool firstPropertyGroup = true;

        string xmlns = "http://schemas.microsoft.com/developer/msbuild/2003"; // doc.Attributes["xmlns"].Value;

        foreach (XmlNode child in doc.DocumentElement)
        {
            if (child.Name == "PropertyGroup")
            {
                bool containsTargetPlatform = false;

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
                        child2.InnerText = "v2.0";
                    }
                }

                if (firstPropertyGroup && !containsTargetPlatform)
                {
                    XmlNode targetPlatform = doc.CreateElement("TargetFrameworkVersion", xmlns);
                    targetPlatform.InnerText = "v2.0";
                    child.AppendChild(targetPlatform);
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

                        if ((child2.FirstChild != null) && (child2.FirstChild.Name == "HintPath"))
                        {
                            if (child2.FirstChild.InnerText.Contains("..\\_bin"))
                            {
                                Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                            }
                            else
                            {
                                // more complicated case:
                                // eg for a project in Shared, which refers to ..\..\..\..\Shared\_bin\Server_Client\Debug\Ict.Petra.Shared.MCommon.DataTables.dll
                                string hintPath = child2.FirstChild.InnerText;

                                if (hintPath.StartsWith(".."))
                                {
                                    hintPath = Path.GetFullPath(Path.GetDirectoryName(AFilename) + Path.DirectorySeparatorChar + hintPath);
                                }

                                if ((hintPath.IndexOf("_bin") > -1) && AFilename.StartsWith(hintPath.Substring(0, hintPath.IndexOf("_bin"))))
                                {
                                    Console.WriteLine("PROBLEM: Please fix project reference to " + referencedDll);
                                }
                            }
                        }
                    }
                }
            }
        }

        doc.Save(AFilename + ".new");
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