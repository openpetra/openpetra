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
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.DotNet.Tasks;

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

                CscTask csc = new CscTask();
                this.CopyTo(csc);

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
                csc.NoConfig = true;

                String FrameworkDLLPath = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(System.Type)).Location);

                // csc.References = new NAnt.DotNet.Types.AssemblyFileSet();

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
                                //csc.ResourcesList.Add( Add(ItemNode.Attributes["Include"].Value);
                            }
                        }
                    }
                }

                csc.Execute();
            }
            else
            {
                throw new Exception("task CompileProject did not get enough parameters to run!");
            }
        }
    }
}