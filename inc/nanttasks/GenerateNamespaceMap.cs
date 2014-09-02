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

        private bool FShowWarnings = true;
        /// <summary>
        /// show warnings when namespace seems to be missing and cannot be resolved.
        /// it only makes sense to show warnings, after all winforms have been generated
        /// </summary>
        [TaskAttribute("ShowWarnings", Required = false)]
        public bool ShowWarnings {
            set
            {
                FShowWarnings = value;
            }
        }

        private List <string>FLimitToNamespaces = new List <string>();
        /// <summary>
        /// set this if you only want to generate the project mapping for some namespaces (eg. Ict.Common and Ict.Tools*)
        /// </summary>
        [TaskAttribute("LimitToNamespaces", Required = false)]
        public string LimitToNamespaces {
            set
            {
                FLimitToNamespaces = new List <string>(value.Split(new char[] { ',' }));
            }
        }

        /// <summary>
        /// create namespace map
        /// </summary>
        protected override void ExecuteTask()
        {
            Dictionary <string, string>NamespaceMap = new Dictionary <string, string>();
            Dictionary <string, TDetailsOfDll>UsingMap = new Dictionary <string, TDetailsOfDll>();

            string[] csfiles = Directory.GetFiles(FCodeRootDir, "*.cs", SearchOption.AllDirectories);

            foreach (string csfile in csfiles)
            {
                if (csfile.Contains("ThirdParty"))
                {
                    continue;
                }

                ParseCSFile(NamespaceMap, UsingMap, csfile);
            }

            // fix all namespaces where the dll/exe is called differently than the namespace calculated from the directory
            bool change = true;

            while (change)
            {
                change = false;

                foreach (string key in NamespaceMap.Keys)
                {
                    if (UsingMap.ContainsKey(NamespaceMap[key])
                        && (UsingMap[NamespaceMap[key]].OutputName.Length > 0)
                        && (NamespaceMap[key] != UsingMap[NamespaceMap[key]].OutputName))
                    {
                        NamespaceMap[key] = UsingMap[NamespaceMap[key]].OutputName;
                        change = true;
                        break;
                    }
                }
            }

            Dictionary <string, string>ThirdPartyMap = ReadMap(FNamespaceMap3rdParty);
            Dictionary <string, TDetailsOfDll>dllMap = UsingNamespaceMapToDll(NamespaceMap, ThirdPartyMap, UsingMap, FShowWarnings);

            WriteNamespaceMap(FNamespaceMapFilename, NamespaceMap);

            WriteProjectMap(FDependencyMapFilename, dllMap);
        }

        private bool IgnoreNamespace(string ANamespace)
        {
            if (FLimitToNamespaces.Count > 0)
            {
                foreach (string name in FLimitToNamespaces)
                {
                    if ((name == ANamespace)
                        || (name.EndsWith("*") && ANamespace.StartsWith(name.Substring(0, name.Length - 1))))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// get the project name, without extension and without path, of a file.
        /// using the path of the file
        public static string GetProjectNameFromCSFile(string filename, string ACodeRootDir)
        {
            string DllName = Path.GetDirectoryName(filename);

            if (DllName.Contains(ACodeRootDir))
            {
                DllName = DllName.Substring(ACodeRootDir.Length + 1);
            }

            DllName = DllName.Replace('/', '.').Replace('\\', '.');

            if (DllName.StartsWith("ICT."))
            {
                DllName = "Ict." + DllName.Substring("ICT.".Length);
            }

            DllName = DllName.Replace("Ict.BuildTools.", "Ict.Tools.");

            return DllName;
        }

        private string ParseCSFile(Dictionary <string, string>NamespaceMap, Dictionary <string, TDetailsOfDll>UsingNamespaces, string filename)
        {
            StreamReader sr = new StreamReader(filename);

            string DllName = GetProjectNameFromCSFile(filename, FCodeRootDir);

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

                            if (Namespace.StartsWith("System.Web") && !Path.GetDirectoryName(filename).EndsWith("WebService"))
                            {
                                Console.WriteLine(
                                    "Warning: we should not reference System.Web since that is not part of the client profile of .net 4.0! in " +
                                    filename);
                            }

                            if (Namespace.StartsWith("System.Runtime.Caching")
                                && !Path.GetDirectoryName(filename).EndsWith("WebService"))
                            {
                                Console.WriteLine(
                                    "Warning: we should not reference System.Runtime.Caching since that is not part of the client profile of .net 4.0! in "
                                    +
                                    filename);
                            }

                            if (Namespace.StartsWith("Ict.Petra.Server")
                                && Path.GetDirectoryName(filename).Replace("\\", "/").Contains("ICT/Petra/Client"))
                            {
                                Console.WriteLine(
                                    "Warning: we must not reference a Server namespace (" + Namespace + ") from the client side in "
                                    +
                                    filename);
                            }

                            if (Namespace.StartsWith("Ict.Petra.Server")
                                && Path.GetDirectoryName(filename).Replace("\\", "/").Contains("ICT/Petra/Shared"))
                            {
                                Console.WriteLine(
                                    "Warning: we must not reference a Server namespace (" + Namespace + ") from the shared directory in "
                                    +
                                    filename);
                            }

                            if (Namespace.StartsWith("Ict.Petra.Client")
                                && Path.GetDirectoryName(filename).Replace("\\", "/").Contains("ICT/Petra/Shared"))
                            {
                                Console.WriteLine(
                                    "Warning: we must not reference a Client namespace (" + Namespace + ") from the shared directory in "
                                    +
                                    filename);
                            }

                            if (Namespace.StartsWith("Ict.Petra.Client")
                                && Path.GetDirectoryName(filename).Replace("\\", "/").Contains("ICT/Petra/Server"))
                            {
                                Console.WriteLine(
                                    "Warning: we must not reference a Client namespace (" + Namespace + ") from the server directory in "
                                    +
                                    filename);
                            }

                            if (!DetailsOfDll.UsedNamespaces.Contains(Namespace))
                            {
                                DetailsOfDll.UsedNamespaces.Add(Namespace);
                            }
                        }
                    }
                    else if (line.Trim().StartsWith("// generateNamespaceMap-Link-Extra-DLL "))
                    {
                        string Namespace = line.Substring("// generateNamespaceMap-Link-Extra-DLL ".Length);
                        int indexComment = Namespace.IndexOf("//");

                        if (indexComment != -1)
                        {
                            // eg. // Implicit reference
                            Namespace = Namespace.Substring(0, indexComment);
                        }

                        Namespace = Namespace.Trim(new char[] { ' ', '\t', '\n', '\r', ';' });
//Console.WriteLine("Encountered 'generateNamespaceMap-Link-Extra-DLL ': Namespace = " +Namespace);
                        if ((Namespace != string.Empty) && !Namespace.Contains(" "))
                        {
                            if (!NamespaceMap.ContainsKey(Namespace))
                            {
                                NamespaceMap.Add(Namespace, Namespace + ".dll");
                            }
                            
                            if (!DetailsOfDll.UsedNamespaces.Contains(Namespace))
                            {
//Console.WriteLine("Added 'Extra DLL': " + Namespace);                                
                                DetailsOfDll.UsedNamespaces.Add(Namespace);
                            }                            
                        }
                    }
                    else if (line.Contains("static void Main(") || line.Contains("static int Main("))
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
            Dictionary <string, TDetailsOfDll>UsingNamespaces,
            bool AShowWarnings)
        {
            Dictionary <string, TDetailsOfDll>result = new Dictionary <string, TDetailsOfDll>();

            foreach (string key in UsingNamespaces.Keys)
            {
                if (IgnoreNamespace(key))
                {
                    continue;
                }

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

                        if (!found && AShowWarnings)
                        {
                            Console.WriteLine("Warning: we do not know in which dll the namespace " + usingNamespace + " is defined");
                        }
                    }

                    if (NamespaceMap.ContainsKey(usingNamespace))
                    {
                        string dllname = NamespaceMap[usingNamespace];

                        if ((dllname != key) && !DetailsOfDll.ReferencedDlls.Contains(dllname) && (DetailsOfDll.OutputName != dllname))
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

        private void WriteNamespaceMap(string filename, Dictionary <string, string>map)
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

        private void WriteProjectMap(string filename, Dictionary <string, TDetailsOfDll>map)
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