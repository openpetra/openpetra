//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Resources;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Tasks;
using NAnt.Core.Types;
using NAnt.DotNet.Tasks;
using NAnt.DotNet.Types;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// compile a project from a csproj file (in vscode format)
    /// </summary>
    [TaskName("CompileProject")]
    public class CompileProject : NAnt.Core.Task
    {
        private string FCSProjFile = null;
        /// <summary>
        /// the SharpDevelop project file that should be compiled
        /// </summary>
        [TaskAttribute("CSProjFile", Required = false)]
        public string CSProjFile {
            set
            {
                FCSProjFile = value;
            }
            get
            {
                return FCSProjFile;
            }
        }

        private string FCSFile = null;
        /// <summary>
        /// the csharp file that should be compiled.
        /// this will require the namespace map and the project map to resolve the correct csproj file
        /// </summary>
        [TaskAttribute("CSFile", Required = false)]
        public string CSFile {
            set
            {
                FCSFile = value;
            }
            get
            {
                return FCSFile;
            }
        }

        private string FProjectName = null;
        /// <summary>
        /// the project name that should be compiled.
        /// </summary>
        [TaskAttribute("ProjectName", Required = false)]
        public string ProjectName {
            set
            {
                FProjectName = value;
            }
            get
            {
                return FProjectName;
            }
        }

        private string FCodeRootDir = null;
        /// <summary>
        /// should point to csharp directory
        /// </summary>
        [TaskAttribute("CodeRootDir", Required = false)]
        public string CodeRootDir {
            set
            {
                FCodeRootDir = value;
            }
            get
            {
                return FCodeRootDir;
            }
        }

        private bool FUseCSC = false;
        /// <summary>
        /// should we use the csc task, or generate a mini solution and use msbuild for that?
        /// </summary>
        [TaskAttribute("UseCSC", Required = false)]
        public bool UseCSC {
            set
            {
                FUseCSC = value;
            }
            get
            {
                return FUseCSC;
            }
        }

        private bool FOnlyOnce = false;
        /// <summary>
        /// if this is set to true, the project is skipped if the deliverable dll or exe file already exists
        /// </summary>
        [TaskAttribute("OnlyOnce", Required = false)]
        public bool OnlyOnce {
            set
            {
                FOnlyOnce = value;
            }
            get
            {
                return FOnlyOnce;
            }
        }

        protected void RunCscTask()
        {
            CscTask csc = new CscTask();

            this.CopyTo(csc);

            if (this.Project.PlatformName == "unix")
            {
                // on Windows this is csc, but on Mono on Linux or Mac we need mcs
                csc.ExeName = "mcs";
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(FCSProjFile);

            XmlNode propertyGroup = doc.DocumentElement.FirstChild;
            Dictionary <string, string>mainProperties = new Dictionary <string, string>();

            foreach (XmlNode propNode in propertyGroup.ChildNodes)
            {
                mainProperties.Add(propNode.Name, propNode.InnerText);
            }

            string OutputFile = Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" + mainProperties["OutputPath"].Replace("\\", "/"));

            OutputFile += "/" + mainProperties["AssemblyName"];

            if (mainProperties["OutputType"].ToLower() == "library")
            {
                OutputFile += ".dll";
            }
            else
            {
                OutputFile += ".exe";
            }

            csc.OutputFile = new FileInfo(OutputFile);
            csc.DocFile = new FileInfo(mainProperties["DocumentationFile"]);
            csc.OutputTarget = mainProperties["OutputType"];

            // needed because of sqlite3.dll, when compiling on Linux for Windows
            if (this.Project.PlatformName == "unix")
            {
                csc.Platform = "x86";
            }

            csc.Define = "DEBUGMODE";

            String FrameworkDLLPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(System.Type)).Location);

            foreach (XmlNode ProjectNodeChild in doc.DocumentElement)
            {
                if (ProjectNodeChild.Name == "ItemGroup")
                {
                    foreach (XmlNode ItemNode in ProjectNodeChild)
                    {
                        if (ItemNode.Name == "Reference")
                        {
                            if (ItemNode.HasChildNodes && (ItemNode.ChildNodes[0].Name == "HintPath"))
                            {
                                csc.References.AsIs.Add(ItemNode.ChildNodes[0].InnerText);
                            }
                            else
                            {
                                // .net dlls
                                csc.References.AsIs.Add(
                                    FrameworkDLLPath + Path.DirectorySeparatorChar +
                                    ItemNode.Attributes["Include"].Value + ".dll");
                            }
                        }
                        else if (ItemNode.Name == "ProjectReference")
                        {
                            string ReferencedProjectName = ItemNode.ChildNodes[1].InnerText;
                            csc.References.AsIs.Add(
                                Path.GetDirectoryName(OutputFile) + Path.DirectorySeparatorChar +
                                ReferencedProjectName + ".dll");
                        }
                        else if (ItemNode.Name == "Compile")
                        {
                            csc.Sources.AsIs.Add(Path.GetDirectoryName(FCSProjFile) + Path.DirectorySeparatorChar +
                                ItemNode.Attributes["Include"].Value);
                        }
                        else if (ItemNode.Name == "EmbeddedResource")
                        {
                            ResourceFileSet fs = new ResourceFileSet();
                            fs.AsIs.Add(ItemNode.Attributes["Include"].Value);
                            csc.ResourcesList.Add(fs);
                        }
                    }
                }
            }

            csc.Execute();
        }

        private string GetNamespaceAndClass(string ACSFile)
        {
            string result = string.Empty;

            StreamReader sr = new StreamReader(ACSFile);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();

                if (line.StartsWith("namespace "))
                {
                    result = line.Substring("namespace ".Length);
                }

                if (line.Contains("public ") && line.Contains(" class "))
                {
                    string classname = line.Substring(line.IndexOf(" class ") + " class ".Length);

                    if (classname.IndexOfAny(new char[] { ' ', ':', '{' }) != -1)
                    {
                        classname = classname.Substring(0, classname.IndexOfAny(new char[] { ' ', ':', '{' }));
                    }

                    result += "." + classname;

                    return result;
                }
            }

            return Path.GetFileNameWithoutExtension(ACSFile);
        }

        private bool CompileHere()
        {
            if (OnlyOnce && (File.Exists("delivery/bin/" + Path.GetFileName(FCSProjFile).Replace(".csproj", ".dll")) ||
                  File.Exists("delivery/bin/" + Path.GetFileName(FCSProjFile).Replace(".csproj", ".exe"))))
            {
                Console.WriteLine("Skipping " + FCSProjFile);
                return true;
            }

            Console.WriteLine("Compiling " + FCSProjFile);

            XmlDocument doc = new XmlDocument();

            doc.Load(FCSProjFile);

            XmlNode propertyGroup = doc.DocumentElement.FirstChild;
            Dictionary <string, string>mainProperties = new Dictionary <string, string>();

            List <String>src = new List <string>();

            foreach (XmlNode propNode in propertyGroup.ChildNodes)
            {
                mainProperties.Add(propNode.Name, propNode.InnerText);
            }

            CSharpCodeProvider csc = new CSharpCodeProvider(
                new Dictionary <string, string>() {
                    { "CompilerVersion", "v4.0" }
                });
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = false;
            parameters.CompilerOptions = string.Empty;

            string OutputFile = Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" + mainProperties["OutputPath"].Replace("\\", "/"));

            OutputFile += "/" + mainProperties["AssemblyName"];

            if (mainProperties["OutputType"].ToLower() == "library")
            {
                parameters.GenerateExecutable = false;
                OutputFile += ".dll";
            }
            else if (mainProperties["OutputType"].ToLower() == "winexe")
            {
                parameters.GenerateExecutable = true;
                parameters.CompilerOptions += " /target:winexe";
                OutputFile += ".exe";
            }
            else
            {
                parameters.GenerateExecutable = true;
                OutputFile += ".exe";
            }

            if (File.Exists(OutputFile))
            {
                // if compilation fails, we want compileProject -D:onlyonce=true to pick up this project
                File.Delete(OutputFile);
            }

            // needed because of sqlite3.dll, when compiling on Linux for Windows
            if (this.Project.PlatformName == "unix")
            {
                parameters.CompilerOptions += " /platform:x86";
            }

            parameters.OutputAssembly = OutputFile;
            parameters.WarningLevel = 4;

            if (mainProperties.ContainsKey("ApplicationManifest") && (mainProperties["ApplicationManifest"].Length > 0))
            {
                if (!this.Project.RuntimeFramework.Name.StartsWith("mono"))
                {
                    // we cannot include the manifest when compiling on Mono
                    parameters.CompilerOptions += " /win32manifest:\"APPMANIFEST\"";
                }
            }

            parameters.CompilerOptions += " /define:DEBUGMODE /doc:\"XMLOUTPUTFILE.xml\"";

            if (this.Project.PlatformName == "unix")
            {
                // command line options use - instead of /, eg. /define or -define
                parameters.CompilerOptions = parameters.CompilerOptions.Replace("/", "-");
            }

            // insert the path to the xml output file after the command line options thing has been done
            parameters.CompilerOptions = parameters.CompilerOptions.Replace("XMLOUTPUTFILE", OutputFile.Replace("\\", "/"));

            if (mainProperties.ContainsKey("ApplicationManifest") && (mainProperties["ApplicationManifest"].Length > 0))
            {
                parameters.CompilerOptions =
                    parameters.CompilerOptions.Replace("APPMANIFEST",
                        Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" + mainProperties["ApplicationManifest"].Replace("\\", "/")));
            }

            String FrameworkDLLPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(System.Type)).Location);

            foreach (XmlNode ProjectNodeChild in doc.DocumentElement)
            {
                if (ProjectNodeChild.Name == "ItemGroup")
                {
                    foreach (XmlNode ItemNode in ProjectNodeChild)
                    {
                        if (ItemNode.Name == "Reference")
                        {
                            if (ItemNode.HasChildNodes && (ItemNode.ChildNodes[0].Name == "HintPath"))
                            {
                                parameters.ReferencedAssemblies.Add(Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" +
                                        ItemNode.ChildNodes[0].InnerText.Replace("\\", "/")));
                            }
                            else
                            {
                                // .net dlls
                                parameters.ReferencedAssemblies.Add(
                                    FrameworkDLLPath + Path.DirectorySeparatorChar +
                                    ItemNode.Attributes["Include"].Value + ".dll");
                            }
                        }
                        else if (ItemNode.Name == "ProjectReference")
                        {
                            string ReferencedProjectName = ItemNode.ChildNodes[1].InnerText;
                            parameters.ReferencedAssemblies.Add(
                                Path.GetFullPath(Path.GetDirectoryName(OutputFile) + "/" +
                                    ReferencedProjectName.Replace("\\", "/") + ".dll"));
                        }
                        else if (ItemNode.Name == "Compile")
                        {
                            src.Add(Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" +
                                    ItemNode.Attributes["Include"].Value.Replace("\\", "/")));
                        }
                        else if (ItemNode.Name == "EmbeddedResource")
                        {
                            string ResourceXFile = ItemNode.Attributes["Include"].Value;

                            if (ResourceXFile.StartsWith(".."))
                            {
                                ResourceXFile = Path.GetFullPath(Path.GetDirectoryName(FCSProjFile) + "/" +
                                    ResourceXFile.Replace("\\", "/"));
                            }

                            parameters.EmbeddedResources.Add(ResourceXFile);
                        }
                    }
                }
            }

            CompilerResults results = csc.CompileAssemblyFromFile(parameters, src.ToArray());

            bool result = true;

            foreach (CompilerError error in results.Errors)
            {
                Console.WriteLine(error.ToString());

                if (!error.IsWarning)
                {
                    result = false;
                }
            }

            if (!result)
            {
                FailTask fail = new FailTask();
                this.CopyTo(fail);
                fail.Message = "compiler error(s)";
                fail.Execute();
            }

            return result;
        }

        private string CalculateSrcPathForProject(string AProjectName)
        {
            return FCodeRootDir + Path.DirectorySeparatorChar +
                AProjectName.
                Replace("Ict.Tools.", "ICT.BuildTools.").
                Replace("Ict.", "ICT.").
                Replace('.', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// compile the project
        /// </summary>
        protected override void ExecuteTask()
        {
            if ((FCSFile != null) && (FCodeRootDir != null))
            {
                // find the correct project file
                FCSProjFile = FCodeRootDir + "/" +
                              GenerateNamespaceMap.GetProjectNameFromCSFile(FCSFile, FCodeRootDir) +
                              ".csproj";
            }

            if ((FProjectName != null) && (FCodeRootDir != null))
            {
                // calculate project file from the project name
                FCSProjFile = CalculateSrcPathForProject(FProjectName) + "/" + FProjectName + ".csproj";
            }

            if (FCSProjFile != null)
            {
                // Console.WriteLine("compiling " + Path.GetFileNameWithoutExtension(FCSProjFile));

                // could call msbuild or xbuild with the project files as parameter
                // OR: process csproj file, and call csc task directly. might avoid some warnings, and work around xbuild issues

                // this is the fastest option. resources are compiled quickly, and only the required dll is compiled
                CompileHere();
            }
            else
            {
                throw new Exception("task CompileProject did not get enough parameters to run!");
            }
        }
    }
}
