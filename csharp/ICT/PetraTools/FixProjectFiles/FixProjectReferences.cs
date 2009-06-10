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

namespace FixProjectFiles
{
	/// <summary>
	/// fix the project GUIDs and references, using the GUID from the sln files
	/// </summary>
	public class TFixProjectReferences
	{
		/// <summary>
		/// all the projects GUIDs loaded from the solution files
		/// </summary>
		private SortedList<string, string> FProjectGUIDs = new SortedList<string, string>();

		/// <summary>
		/// add GUIDs of projects inside the solution;
		/// fills FProjectGUIDs
		/// </summary>
		/// <param name="ASolutionFile"></param>
		/// <returns>a list of paths of the projects that are part of this solution</returns>
		public StringCollection LoadGUIDsFromSolution(string ASolutionFile)
		{
			StringCollection result = new StringCollection();
			StreamReader reader = new StreamReader(ASolutionFile);
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();
				if (line.StartsWith("Project("))
				{
					StringCollection details = StringHelper.StrSplit(line.Substring(line.IndexOf("=") + 1), ",");
					// name of project file (without path, without extension csproj)
					string ProjectName = StringHelper.TrimQuotes(details[0]);
					// relative path to project
					string RelativePath = StringHelper.TrimQuotes(details[1]);
					// GUID of project
					string ProjectGUID = StringHelper.TrimQuotes(details[2]);
					
					if (!FProjectGUIDs.ContainsKey(ProjectName))
					{
						FProjectGUIDs.Add(ProjectName, ProjectGUID);
						result.Add(Path.GetFullPath(Path.GetDirectoryName(ASolutionFile) + Path.DirectorySeparatorChar + RelativePath));
					}
					else
					{
						// some projects are part of several solutions; just make sure they have the same GUID
						if (FProjectGUIDs[ProjectName] != ProjectGUID)
						{
							throw new Exception("problem: wrong GUID for " + ProjectName + " in " + ASolutionFile);
						}
					}
				}
			}
			return result;
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
			foreach (XmlNode child in doc.DocumentElement)
			{
				if (child.Name == "PropertyGroup")
				{
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
					}
				}
				if (child.Name == "ItemGroup")
				{
					foreach(XmlNode child2 in child.ChildNodes)
					{
						if (child2.Name == "ProjectReference")
						{
							foreach(XmlNode child3 in child2.ChildNodes)
							{
								if (child3.Name == "Project")
								{
									string referencedProject = 
										System.IO.Path.GetFileNameWithoutExtension(child2.Attributes["Include"].Value);
									if (child3.InnerText != FProjectGUIDs[referencedProject])
									{
										Console.WriteLine("fixing " + child3.InnerText);
										child3.InnerText = FProjectGUIDs[referencedProject];
									}
								}
							}
						}
					}
				}
			}
			doc.Save(AFilename + ".new");
			Ict.Tools.CodeGeneration.TTextFile.UpdateFile(AFilename);
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