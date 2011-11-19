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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// create a map that tells which namespaces are defined in which directory/dll
    /// </summary>
    [TaskName("GenerateNamespaceMap")]
    public class GenerateNamespaceMap : NAnt.Core.Task
    {
        string DATE_TIME_STRING = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        private string FNamespaceMapFilename = null;
        /// <summary>
        /// path of the file, where the namespace map will be saved and read from
        /// </summary>
        [TaskAttribute("NamespaceMapFilename", Required = true)]
        public string NamespaceMapFilename {
            get
            {
                return FNamespaceMapFilename;
            }
            set
            {
                FNamespaceMapFilename = value;
            }
        }

        private string FDependencyMapFilename = null;
        /// <summary>
        /// path of the file, where the dependancy map will be saved and read from
        /// </summary>
        [TaskAttribute("DependencyMapFilename", Required = true)]
        public string DependencyMapFilename {
            get
            {
                return FDependencyMapFilename;
            }
            set
            {
                FDependencyMapFilename = value;
            }
        }
        
        private string FCodeRootDir = null;
        /// <summary>
        /// should point to csharp directory
        /// </summary>
        [TaskAttribute("CodeRootDir", Required = true)]
        public string CodeRootDir {
            get
            {
                return FCodeRootDir;
            }
            set
            {
                FCodeRootDir = value;
            }
        }
        
        /// <summary>
        /// create namespace map
        /// </summary>
        protected override void ExecuteTask()
        {
            Dictionary <string, string> map = new Dictionary<string, string>();
            Dictionary <string, TDetailsOfDll> UsingMap = new Dictionary<string, TDetailsOfDll>();
                
            string[] csfiles = Directory.GetFiles(FCodeRootDir, "*.cs", SearchOption.AllDirectories);
            foreach (string csfile in csfiles)
            {
                if (!csfile.Contains("ThirdParty"))
                {
                    ParseCSFile(map, UsingMap, csfile);
                }
            }
            
            WriteMap(FNamespaceMapFilename, map);
            
            WriteMap(FDependencyMapFilename, UsingNamespaceMapToDll(map, UsingMap));
        }
        
        private string ParseCSFile(Dictionary <string, string> NamespaceMap, Dictionary<string, TDetailsOfDll> UsingNamespaces, string filename)
        {
            StreamReader sr = new StreamReader(filename);

            string DllName = 
                Path.GetDirectoryName(filename).Substring(FCodeRootDir.Length + 1).
                    Replace(Path.DirectorySeparatorChar, '.');
            
            if (DllName.StartsWith("ICT."))
            {
                DllName = "Ict." + DllName.Substring("ICT.".Length);
            }
            
            DllName = DllName.Replace("Ict.PetraTools.", "Ict.Tools.");

            TDetailsOfDll DetailsOfDll;
            if (UsingNamespaces.ContainsKey(DllName))
            {
                DetailsOfDll = UsingNamespaces[DllName];
            }
            else
            {
                DetailsOfDll = new TDetailsOfDll();
                UsingNamespaces.Add(DllName, DetailsOfDll);
            }

            try
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Trim().StartsWith("namespace "))
                    {
                        string Namespace = line.Substring("namespace".Length).Trim(new char[]{' ', '\t', '\n', '\r', ';'});
                        if (Namespace != string.Empty)
                        {
                            if (!NamespaceMap.ContainsKey(Namespace))
                            {
                                NamespaceMap.Add(Namespace, DllName);
                            }
                            else
                            {
                                if (NamespaceMap[Namespace] != DllName)
                                {
                                    throw new Exception(
                                        string.Format("Error: GenerateNamespaceMap: a namespace cannot exist in two different directories! {0} exists in {1} and {2}",
                                                      Namespace, NamespaceMap[Namespace], DllName));
                                }
                                
                            }
                        }
                    }
                    else if (line.Trim().StartsWith("using "))
                    {
                        string Namespace = line.Substring("using".Length).Trim(new char[]{' ', '\t', '\n', '\r', ';'});
                        if (Namespace != string.Empty && !Namespace.Contains(" "))
                        {
                            if (!DetailsOfDll.UsedNamespaces.Contains(Namespace))
                            {
                                DetailsOfDll.UsedNamespaces.Add(Namespace);
                            }
                        }
                    }
                    else if (line.Contains("static void Main("))
                    {
                        DetailsOfDll.OutputType = "exe";
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            
            return string.Empty;
        }
            
        /// <summary>
        /// now reduce the namespace names to dll names
        /// </summary>
        /// <param name="NamespaceMap"></param>
        /// <param name="UsingNamespaces"></param>
        /// <returns></returns>
        private Dictionary<string, TDetailsOfDll> UsingNamespaceMapToDll(Dictionary <string, string> NamespaceMap, Dictionary<string, TDetailsOfDll> UsingNamespaces)
        {
            Dictionary<string, TDetailsOfDll> result = new Dictionary<string, TDetailsOfDll>();
            
            foreach (string key in UsingNamespaces.Keys)
            {
                TDetailsOfDll DetailsOfDll = new TDetailsOfDll();
                DetailsOfDll.OutputType = UsingNamespaces[key].OutputType;
                result.Add(key, DetailsOfDll);

                foreach(string usingNamespace in UsingNamespaces[key].UsedNamespaces)
                {
                    if (!NamespaceMap.ContainsKey(usingNamespace))
                    {
                        if (usingNamespace.StartsWith("Ict."))
                        {
                            Console.WriteLine("Warning: missing dllname for namespace " + usingNamespace);
                        }
                        else
                        {
                            NamespaceMap.Add(usingNamespace, usingNamespace);
                        }
                    }
                    
                    if (NamespaceMap.ContainsKey(usingNamespace))
                    {
                        string dllname = NamespaceMap[usingNamespace];
                        
                        if (dllname != key && !DetailsOfDll.ReferencedDlls.Contains(dllname))
                        {
                            DetailsOfDll.ReferencedDlls.Add(dllname);
                        }
                    }
                }
            }

            return result;
        }
        
        private void WriteMap(string filename, Dictionary<string, string> map)
        {
            // If the directory does not exist, we have to create it
            string dirname = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# Generated with GenerateNamespaceMap at " + DATE_TIME_STRING);

            foreach (string key in map.Keys)
            {
                sw.WriteLine(key + "=" + map[key]);
            }

            sw.Close();
        }

        private void WriteMap(string filename, Dictionary<string, TDetailsOfDll> map)
        {
            // If the directory does not exist, we have to create it
            string dirname = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# Generated with GenerateNamespaceMap at " + DATE_TIME_STRING);

            foreach (string key in map.Keys)
            {
                sw.WriteLine(key + "," + map[key].OutputType);
                foreach(string referencedDll in map[key].ReferencedDlls)
                {
                    sw.Write("  ");
                    sw.WriteLine(referencedDll);
                }
            }

            sw.Close();
        }
    }
    
    /// <summary>
    /// this is used to store details of a cs project
    /// </summary>
    public class TDetailsOfDll
    {
        /// <summary>
        /// library or exe
        /// </summary>
        public string OutputType = "library";
        
        /// <summary>
        /// this dll is using the following namespaces
        /// </summary>
        public List<string> UsedNamespaces = new List<string>();
        
        /// <summary>
        /// this dll is referencing the following assemblies
        /// </summary>
        public List<string> ReferencedDlls = new List<string>();
    }
}