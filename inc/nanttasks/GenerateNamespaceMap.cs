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

        private string FNamespaceMap3rdParty = null;
        /// <summary>
        /// path of the file, where the namespace map for the 3rd Party and .net libraries is stored
        /// </summary>
        [TaskAttribute("NamespaceMap3rdParty", Required = true)]
        public string NamespaceMap3rdParty {
            get
            {
                return FNamespaceMap3rdParty;
            }
            set
            {
                FNamespaceMap3rdParty = value;
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
            Dictionary <string, string>map = new Dictionary <string, string>();
            Dictionary <string, TDetailsOfDll>UsingMap = new Dictionary <string, TDetailsOfDll>();

            string[] csfiles = Directory.GetFiles(FCodeRootDir, "*.cs", SearchOption.AllDirectories);

            foreach (string csfile in csfiles)
            {
                if (!csfile.Contains("ThirdParty"))
                {
                    ParseCSFile(map, UsingMap, csfile);
                }
            }

            WriteMap(FNamespaceMapFilename, map);

            Dictionary <string, string>ThirdPartyMap = ReadMap(FNamespaceMap3rdParty);

            WriteMap(FDependencyMapFilename, UsingNamespaceMapToDll(map, ThirdPartyMap, UsingMap));
        }

        private string ParseCSFile(Dictionary <string, string>NamespaceMap, Dictionary <string, TDetailsOfDll>UsingNamespaces, string filename)
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
                DetailsOfDll.UsedNamespaces.Add("System");
                DetailsOfDll.UsedNamespaces.Add("System.Xml");
                UsingNamespaces.Add(DllName, DetailsOfDll);
            }

            try
            {
                string LastNamespace = string.Empty;
                bool ReferencesWinForms = false;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.Trim().StartsWith("namespace "))
                    {
                        string Namespace = line.Substring("namespace".Length).Trim(new char[] { ' ', '\t', '\n', '\r', ';' });

                        if (Namespace != string.Empty)
                        {
                            LastNamespace = Namespace;

                            if (!NamespaceMap.ContainsKey(Namespace))
                            {
                                NamespaceMap.Add(Namespace, DllName);
                            }
                            else
                            {
                                if (NamespaceMap[Namespace] != DllName)
                                {
                                    throw new Exception(
                                        string.Format(
                                            "Error: GenerateNamespaceMap: a namespace cannot exist in two different directories! {0} exists in {1} and {2}",
                                            Namespace, NamespaceMap[Namespace], DllName));
                                }
                            }
                        }
                    }
                    else if (line.Trim().StartsWith("using "))
                    {
                        string Namespace = line.Substring("using".Length);
                        int indexComment = Namespace.IndexOf("//");

                        if (indexComment != -1)
                        {
                            // eg. // Implicit reference
                            Namespace = Namespace.Substring(0, indexComment);
                        }

                        Namespace = Namespace.Trim(new char[] { ' ', '\t', '\n', '\r', ';' });

                        if ((Namespace != string.Empty) && !Namespace.Contains(" "))
                        {
                            if (Namespace == "System.Windows.Forms")
                            {
                                ReferencesWinForms = true;
                            }

                            if (!DetailsOfDll.UsedNamespaces.Contains(Namespace))
                            {
                                DetailsOfDll.UsedNamespaces.Add(Namespace);
                            }
                        }
                    }
                    else if (line.Contains("static void Main("))
                    {
                        if (ReferencesWinForms)
                        {
                            DetailsOfDll.OutputType = "winexe";
                        }
                        else
                        {
                            DetailsOfDll.OutputType = "exe";
                        }

                        DetailsOfDll.OutputName = LastNamespace;
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
        private Dictionary <string, TDetailsOfDll>UsingNamespaceMapToDll(
            Dictionary <string, string>NamespaceMap,
            Dictionary <string, string>Namespace3rdPartyMap,
            Dictionary <string, TDetailsOfDll>UsingNamespaces)
        {
            Dictionary <string, TDetailsOfDll>result = new Dictionary <string, TDetailsOfDll>();

            foreach (string key in UsingNamespaces.Keys)
            {
                TDetailsOfDll DetailsOfDll = new TDetailsOfDll();
                DetailsOfDll.OutputType = UsingNamespaces[key].OutputType;
                DetailsOfDll.OutputName = UsingNamespaces[key].OutputName;
                result.Add(key, DetailsOfDll);

                foreach (string usingNamespace in UsingNamespaces[key].UsedNamespaces)
                {
                    if (!NamespaceMap.ContainsKey(usingNamespace))
                    {
                        // check the 3rd party namespace map to find the correct DLL
                        bool found = false;

                        foreach (string namespace3rdParty in Namespace3rdPartyMap.Keys)
                        {
                            if ((namespace3rdParty == usingNamespace)
                                || (namespace3rdParty.EndsWith("*") && usingNamespace.StartsWith(namespace3rdParty.Replace("*", ""))))
                            {
                                NamespaceMap.Add(usingNamespace, Namespace3rdPartyMap[namespace3rdParty]);
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("Warning: we do not know in which dll the namespace " + usingNamespace + " is defined");
                        }
                    }

                    if (NamespaceMap.ContainsKey(usingNamespace))
                    {
                        string dllname = NamespaceMap[usingNamespace];

                        if ((dllname != key) && !DetailsOfDll.ReferencedDlls.Contains(dllname))
                        {
                            DetailsOfDll.ReferencedDlls.Add(dllname);
                        }
                    }
                }
            }

            return result;
        }

        private Dictionary <string, string>ReadMap(string filename)
        {
            Dictionary <string, string>Result = new Dictionary <string, string>();

            StreamReader sr = new StreamReader(filename);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line[0] != '#')
                {
                    string[] values = line.Split(new char[] { '=' });
                    Result.Add(values[0], values[1]);
                }
            }

            sr.Close();

            return Result;
        }

        private void WriteMap(string filename, Dictionary <string, string>map)
        {
            // If the directory does not exist, we have to create it
            string dirname = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# Generated with GenerateNamespaceMap at " + DATE_TIME_STRING);

            List <string>sortedNamespaces = new List <string>(map.Keys);
            sortedNamespaces.Sort();

            foreach (string key in sortedNamespaces)
            {
                sw.WriteLine(key + "=" + map[key]);
            }

            sw.Close();
        }

        private void WriteMap(string filename, Dictionary <string, TDetailsOfDll>map)
        {
            // If the directory does not exist, we have to create it
            string dirname = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# Generated with GenerateNamespaceMap at " + DATE_TIME_STRING);

            List <string>sortedDlls = new List <string>(map.Keys);
            sortedDlls.Sort();

            foreach (string key in sortedDlls)
            {
                sw.WriteLine(key + "," + map[key].OutputType + "," + map[key].OutputName);

                List <string>sortedReferences = new List <string>(map[key].ReferencedDlls);
                sortedReferences.Sort();

                foreach (string referencedDll in sortedReferences)
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
        /// sometimes an exe has a different name than the directory structure. using the namespace of the main class
        /// </summary>
        public string OutputName = String.Empty;

        /// <summary>
        /// this dll is using the following namespaces
        /// </summary>
        public List <string>UsedNamespaces = new List <string>();

        /// <summary>
        /// this dll is referencing the following assemblies
        /// </summary>
        public List <string>ReferencedDlls = new List <string>();
    }
}