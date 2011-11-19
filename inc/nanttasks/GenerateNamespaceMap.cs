//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, thiasg
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
            
            string[] csfiles = Directory.GetFiles(FCodeRootDir, "*.cs", SearchOption.AllDirectories);
            foreach (string csfile in csfiles)
            {
                string Namespace = GetNamespaceFromCSFile(csfile);
                
                if (Namespace != string.Empty)
                {
                    string DllName = 
                        Path.GetDirectoryName(csfile).Substring(FCodeRootDir.Length + 1).
                            Replace(Path.DirectorySeparatorChar, '.');
                    
                    if (DllName.StartsWith("ICT."))
                    {
                        DllName = "Ict." + DllName.Substring("ICT.".Length);
                    }
                    
                    DllName = DllName.Replace("Ict.PetraTools.", "Ict.Tools.");
                    
                    if (!map.ContainsKey(Namespace))
                    {
                        map.Add(Namespace, DllName);
                    }
                    else
                    {
                        if (map[Namespace] != DllName)
                        {
                            throw new Exception(
                                string.Format("Error: GenerateNamespaceMap: a namespace cannot exist in two different directories! {0} exists in {1} and {2}",
                                              Namespace, map[Namespace], DllName));
                        }
                        
                    }
                }
            }
            
            WriteMap(FNamespaceMapFilename, map);
        }
        
        private string GetNamespaceFromCSFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            
            try
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Trim().StartsWith("namespace"))
                    {
                        sr.Close();
                        return line.Substring("namespace".Length).Trim(new char[]{' ', '\t', '\n', '\r', ';'});
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            
            return string.Empty;
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
    }
}