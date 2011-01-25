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
using System.IO;
using System.Collections.Specialized;
using System.Xml;
using Ict.Common;

namespace Ict.Tools.CodeGeneration
{
    public class TCSProjTools
    {
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
        /// this is being overwritten in FixProjectReferences
        /// </summary>
        /// <param name="ASolutionFile"></param>
        /// <returns></returns>
        protected virtual bool ProcessProjectDetails(string AProjectName, string ARelativePath, string AProjectGUID, string ASolutionFile)
        {
            return true;
        }

        /// <summary>
        /// add GUIDs of projects inside the solution;
        /// fills FProjectGUIDs
        /// </summary>
        /// <param name="ASolutionFile"></param>
        /// <returns>a list of paths of the projects that are part of this solution</returns>
        public StringCollection LoadGUIDsFromSolution(string ASolutionFile)
        {
throw new NotImplementedException();
            StringCollection result = new StringCollection();
            StreamReader reader = new StreamReader(ASolutionFile);
            StringCollection lines = new StringCollection();
            bool SolutionFixed = false;

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            reader.Close();

            for (Int32 counter = 0; counter < lines.Count; counter++)
            {
                string line = lines[counter];

                if (line.StartsWith("Project("))
                {
                    StringCollection details = StringHelper.StrSplit(line.Substring(line.IndexOf("=") + 1), ",");

                    // name of project file (without path, without extension csproj)
                    string ProjectName = StringHelper.TrimQuotes(details[0]);

                    // relative path to project
                    string RelativePath = StringHelper.TrimQuotes(details[1]).Replace('\\', Path.DirectorySeparatorChar);

                    // GUID of project
                    string ProjectGUID = StringHelper.TrimQuotes(details[2]);

                    if (!ProjectGUID.StartsWith("{"))
                    {
                        SolutionFixed = true;

                        for (Int32 counter2 = 0; counter2 < lines.Count; counter2++)
                        {
                            lines[counter2] = lines[counter2].Replace(ProjectGUID, "{" + ProjectGUID + "}");
                        }

                        ProjectGUID = "{" + ProjectGUID + "}";
                    }

                    if (ProcessProjectDetails(ProjectName, RelativePath, ProjectGUID, ASolutionFile))
                    {
                        result.Add(Path.GetFullPath(Path.GetDirectoryName(ASolutionFile) + Path.DirectorySeparatorChar + RelativePath));
                    }
                }
            }

            if (SolutionFixed)
            {
                StreamWriter sw = new StreamWriter(ASolutionFile);

                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }

                sw.Close();
            }

            return result;
        }

        /// <summary>
        /// get all the code files that belong to the project
        /// </summary>
        /// <param name="AProjectFilename"></param>
        /// <returns>list of full paths to the code files</returns>
        public static StringCollection LoadCodeFilesFromProject(string AProjectFilename)
        {
throw new NotImplementedException();
            StringCollection result = new StringCollection();

            XmlDocument doc = new XmlDocument();

            doc.Load(AProjectFilename);

            foreach (XmlNode child in doc.DocumentElement)
            {
                if (child.Name == "ItemGroup")
                {
                    foreach (XmlNode child2 in child.ChildNodes)
                    {
                        if (child2.Name == "Compile")
                        {
                            result.Add(
                                Path.GetDirectoryName(AProjectFilename) + Path.DirectorySeparatorChar +
                                child2.Attributes["Include"].Value);
                        }
                    }
                }
            }

            return result;
        }
    }
}