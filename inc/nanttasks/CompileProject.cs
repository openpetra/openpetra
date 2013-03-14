//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
    /// compile a project from a csproj file (in SharpDevelop4 format)
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

        private string FProjectFilesDir = null;
        /// <summary>
        /// should point to the directory that contains the sharpdevelop project files
        /// </summary>
        [TaskAttribute("ProjectFilesDir", Required = false)]
        public string ProjectFilesDir {
            set
            {
                FProjectFilesDir = value;
            }
            get
            {
                return FProjectFilesDir;
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

        protected void RunCscTask()
        {
            CscTask csc = new CscTask();

            this.CopyTo(csc);

            if (this.Project.PlatformName == "unix")
            {
                // on Windows this is csc, but on Mono on Linux or Mac we need mcs
                cd.ExeName = "mcs";
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(FCSProjFile);

            XmlNode propertyGroup = doc.DocumentElement.FirstChild;
            Dictionary <string, string>mainProperties = new Dictionary <string, string>();

            foreach (XmlNode propNode in propertyGroup.ChildNodes)
            {
                mainProperties.Add(propNode.Name, propNode.InnerText);
            }

            string OutputFile = mainProperties["OutputPath"];

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
                            csc.Sources.AsIs.Add(ItemNode.Attributes["Include"].Value);
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

            string OutputFile = mainProperties["OutputPath"];

            OutputFile += "/" + mainProperties["AssemblyName"];

            if (mainProperties["OutputType"].ToLower() == "library")
            {
                parameters.GenerateExecutable = false;
                OutputFile += ".dll";
            }
            else
            {
                parameters.GenerateExecutable = true;
                OutputFile += ".exe";
            }

            parameters.OutputAssembly = OutputFile;
            parameters.WarningLevel = 4;

            parameters.CompilerOptions = "/define:DEBUGMODE /doc:\"" + OutputFile + ".xml\"";

            if (this.Project.PlatformName == "unix")
            {
                parameters.CompilerOptions = parameters.CompilerOptions.Replace("/", "-");
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
                                parameters.ReferencedAssemblies.Add(ItemNode.ChildNodes[0].InnerText);
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
                                Path.GetDirectoryName(OutputFile) + Path.DirectorySeparatorChar +
                                ReferencedProjectName + ".dll");
                        }
                        else if (ItemNode.Name == "Compile")
                        {
                            src.Add(ItemNode.Attributes["Include"].Value);
                        }
                        else if (ItemNode.Name == "EmbeddedResource")
                        {
                            string ResourceXFile = ItemNode.Attributes["Include"].Value;

                            if (ResourceXFile.EndsWith(".resx"))
                            {
                                string NamespaceAndClass = Path.GetFileNameWithoutExtension(ResourceXFile);

                                if (ItemNode.HasChildNodes && (ItemNode.FirstChild.Name == "DependentUpon"))
                                {
                                    NamespaceAndClass = GetNamespaceAndClass(ItemNode.FirstChild.InnerText);
                                }

                                //"../../../../tmp/" +
                                string ResourcesFile = NamespaceAndClass + ".resources";

                                if (File.Exists(ResourceXFile))
                                {
                                    Environment.CurrentDirectory = Path.GetDirectoryName(ResourceXFile);

                                    ResXResourceReader ResXReader = new ResXResourceReader(ResourceXFile);
                                    FileStream fs = new FileStream(ResourcesFile, FileMode.OpenOrCreate, FileAccess.Write);
                                    IResourceWriter writer = new ResourceWriter(fs);

                                    foreach (DictionaryEntry d in ResXReader)
                                    {
                                        writer.AddResource(d.Key.ToString(), d.Value);
                                    }

                                    writer.Close();

                                    parameters.EmbeddedResources.Add(ResourcesFile);
                                }
                            }
                            else
                            {
                                parameters.EmbeddedResources.Add(ResourceXFile);
                            }
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

        protected void RunSolutionTask()
        {
            // create a copy of OpenPetra.sln, and remove all projects but FCSProjFile
            StreamReader sr = new StreamReader(FProjectFilesDir + Path.DirectorySeparatorChar + "OpenPetra.sln");
            StreamWriter sw = new StreamWriter(FProjectFilesDir + Path.DirectorySeparatorChar + "OpenPetra.Mini.sln");

            string projFile = Path.GetFileName(FCSProjFile).ToLower();
            bool foundProject = false;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.StartsWith("Project("))
                {
                    if (line.ToLower().Contains(projFile))
                    {
                        sw.WriteLine(line);
                        sw.WriteLine("EndProject");
                        foundProject = true;
                    }
                }
                else if (!line.StartsWith("EndProject"))
                {
                    sw.WriteLine(line);
                }
            }

            sr.Close();
            sw.Close();

            if (!foundProject)
            {
                throw new Exception(
                    "cannot compile " + projFile + " because it cannot be found in " + FProjectFilesDir + Path.DirectorySeparatorChar +
                    "OpenPetra.sln");
            }

            // compile solution OpenPetra.Mini.sln
            CompileSolution sln = new CompileSolution();
            this.CopyTo(sln);

            sln.SolutionFile = FProjectFilesDir + Path.DirectorySeparatorChar + "OpenPetra.Mini.sln";

            sln.Execute();
        }

        /// <summary>
        /// compile the project
        /// </summary>
        protected override void ExecuteTask()
        {
            if ((FCSFile != null) && (FCodeRootDir != null) && (FProjectFilesDir != null))
            {
                // find the correct project file
                FCSProjFile = FProjectFilesDir + "/" +
                              GenerateNamespaceMap.GetProjectNameFromCSFile(FCSFile, FCodeRootDir) +
                              ".csproj";
            }

            if (FCSProjFile != null)
            {
                // Console.WriteLine("compiling " + Path.GetFileNameWithoutExtension(FCSProjFile));

                // could call msbuild or xbuild with the project files as parameter
                // OR: process csproj file, and call csc task directly. might avoid some warnings, and work around xbuild issues

                // this is the fastest option. resources are compiled quickly, and only the required dll is compiled
                CompileHere();

                // if (FUseCSC)
                // {
                //     RunCscTask();
                // }
                // else
                // {
                //     RunSolutionTask();
                // }
            }
            else
            {
                throw new Exception("task CompileProject did not get enough parameters to run!");
            }
        }
    }
}