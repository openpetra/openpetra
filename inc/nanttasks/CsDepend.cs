//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       thiasg, timop
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
    /// Runs CsDepend
    /// Uses following heuristics:
    ///   * One Directory with sourcefiles is one assembly
    ///   * using are used for finding needed namespaces
    ///   * namespace is used to namespaces provided by assembly
    ///   * If a namespace is used, then all assemblies providing this namespace are referenced
    ///   * If a full qualified access is found, then the longest exising namespace is used as referenc
    /// </summary>
    [TaskName("CsDepend")]
    public class CsDependClass : NAnt.Core.Task
    {
        string DATE_TIME_STRING = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        const string NANT_FILE_REFS_DEPS_TEMPLATE = "NANT_FILE_REFS_DEPS_TEMPLATE";
        const string NANT_FILE_TARGET_DEPENDS_TEMPLATE = "NANT_FILE_TARGET_DEPENDS_TEMPLATE";
        const string NANT_FILE_TARGET_TEMPLATE = "NANT_FILE_TARGET_TEMPLATE";
        const string NANT_BUILD_FILE_TEMPLATE = "NANT_BUILD_FILE_TEMPLATE";
        const string NANT_REFERENCE_PATTERNSET_TEMPLATE = "NANT_REFERENCE_PATTERNSET_TEMPLATE";
        const string NANT_REFERENCE_TEMPLATE = "NANT_REFERENCE_TEMPLATE";
        const string NANT_FILE_REFERENCE_INC_TEMPLATE = "NANT_FILE_REFERENCE_INC_TEMPLATE";

        static Regex usingRegex = new Regex(@"^\s*using\s*([a-zA-Z.]+);");
        static Regex absoluteRefRegex = new Regex(@"[^a-zA-Z.]+(Ict\.[a-zA-Z.]+)\.[a-zA-Z]+[^a-zA-Z.]+");
        static Regex ignoreLineCommentRegex = new Regex(@"^(?<noncomment>.*?)//");
        static Regex replaceStringConstantsRegex = new Regex(@"""([^""]*?)""");
        static Regex namespaceRegex = new Regex(@"^\s*namespace\s+([a-zA-Z.]+)");
        static Regex exceptionRegex = new Regex(@"^\s*([a-zA-Z.]+)\s*=\s*([a-zA-Z.0-9\-]+)");

        /// <summary>
        /// the template snippets
        /// </summary>
        private SortedList <String, String>FSnippets;
        /// <summary>
        /// map namespace to assembly
        /// </summary>
        private Hashtable _namespace2assembly = new Hashtable();
        /// <summary>
        /// map assembly to uuid for project file
        /// </summary>
        private Hashtable _assembly2uuid = new Hashtable();
        /// <summary>
        /// Stores assembly specific data
        /// </summary>
        class AssemblyData
        {
            /// <summary>
            /// The directory, where the source files of the assembly are located
            /// </summary>
            public string directory;
            /// <summary>
            /// The name of the assembly
            /// </summary>
            public string name;
            Hashtable _usings = new Hashtable();
            Hashtable _refs = new Hashtable();
            Hashtable _providesNS = new Hashtable();
            public AssemblyData(string dirname, string assemblyname)
            {
                directory = dirname;
                name = assemblyname;
            }

            private void _AddDict(Hashtable map, string name)
            {
                if (!map.Contains(name))
                {
                    map.Add(name, name);
                }
            }

            /// <summary>
            /// Add the given name to the list of provided namespaces
            /// </summary>
            /// <param name="name">Provided namespace</param>
            public void AddNamespace(string name)
            {
                _AddDict(_providesNS, name);
            }

            /// <summary>
            /// Add the name, which is used by a using
            /// </summary>
            /// <param name="name">the name of the using</param>
            public void AddUsing(string name)
            {
                _AddDict(_usings, name);
            }

            /// <summary>
            /// Add a reference, which is referred to by a full qualified access
            /// </summary>
            /// <param name="name"></param>
            public void AddReference(string name)
            {
                _AddDict(_refs, name);
            }

            /// <summary>
            /// Returns all usings as an array.
            /// </summary>
            /// <returns>ArrayList of strings with namespaces by using</returns>
            public ArrayList GetUsings()
            {
                return new ArrayList(_usings.Keys);
            }

            /// <summary>
            /// Returns an arraylist of the references
            /// </summary>
            /// <returns>ArrayList of strings</returns>
            public ArrayList GetReference()
            {
                return new ArrayList(_refs.Keys);
            }
        }
        /// <summary>
        /// maps the computed assemblyname to the directory, where the src files are located
        /// </summary>
        private Hashtable _assembly2data = new Hashtable();

        private FileSet _sources = new FileSet();
        /// <summary>
        /// The set of source files for checking for dependencies.
        /// </summary>
        [BuildElement("sources", Required = true)]
        public FileSet Sources {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
            }
        }
        private string _nsDir = null;
        /// <summary>
        /// Directory, where the namespace map will be saved and read from
        /// </summary>
        [TaskAttribute("ns-map-dir", Required = false)]
        public string nsDir {
            get
            {
                return _nsDir;
            }
            set
            {
                _nsDir = value;
            }
        }
        private string _nsDefault = String.Empty;
        /// <summary>
        /// The namespace used for this directory and all sub-directories
        /// </summary>
        [TaskAttribute("ns-default", Required = true)]
        public string nsDefault {
            get
            {
                return _nsDefault;
            }
            set
            {
                _nsDefault = value;
            }
        }

        private string _basedir = String.Empty;
        /// <summary>
        /// The directory where the original nant build file was called
        /// </summary>
        [TaskAttribute("basedir", Required = true)]
        public string basedir {
            get
            {
                return _basedir;
            }
            set
            {
                _basedir = value;
            }
        }

        private string _outputtype = "library";
        /// <summary>
        /// The directory where the original nant build file was called
        /// </summary>
        [TaskAttribute("outputtype", Required = false)]
        public string outputtype {
            get
            {
                return _outputtype;
            }
            set
            {
                _outputtype = value;
            }
        }

        private string _refsfile = String.Empty;
        /// <summary>
        /// Where the nant buildfile, which should be include, will be written to
        /// </summary>
        [TaskAttribute("referencesfile", Required = true)]
        public string refsfile {
            get
            {
                return _refsfile;
            }
            set
            {
                _refsfile = value;
            }
        }

        private string _uuidfile = String.Empty;
        /// <summary>
        /// Where the project uuid map should be stored
        /// </summary>
        [TaskAttribute("uuidfile", Required = true)]
        public string uuidfile {
            get
            {
                return _uuidfile;
            }
            set
            {
                _uuidfile = value;
            }
        }

        private string _templatefile = String.Empty;
        /// <summary>
        /// The template file that is used for the build files, which are in xml.
        /// This should be a full qualified filename.
        /// </summary>
        [TaskAttribute("templatefile", Required = false)]
        public string templatefile {
            get
            {
                return _templatefile;
            }
            set
            {
                _templatefile = value;
            }
        }

        private string _additionalRefs = String.Empty;
        /// <summary>
        /// The buildfile, which should be included into the generated buildfiles.
        /// This should be a full qualified filename.
        /// </summary>
        [TaskAttribute("additionalRefs", Required = false)]
        public string additionalRefs {
            get
            {
                return _additionalRefs;
            }
            set
            {
                _additionalRefs = value;
            }
        }
        private string _buildincfile = String.Empty;
        /// <summary>
        /// The buildfile, which should be included into the generated buildfiles.
        /// This should be a full qualified filename.
        /// </summary>
        [TaskAttribute("buildincfile", Required = true)]
        public string buildincfile {
            get
            {
                return _buildincfile;
            }
            set
            {
                _buildincfile = value;
            }
        }
        /// <summary>
        /// Execute the task of finding the dependencies
        /// </summary>
        protected override void ExecuteTask()
        {
            Log(Level.Info, "Running for namespace " + nsDefault + " ...");

            // Load templates for the xml files
            LoadTemplates(templatefile);

            if (Sources.BaseDirectory == null)
            {
                Sources.BaseDirectory = new DirectoryInfo(Project.BaseDirectory);
            }

            // Check all sourcefiles
            foreach (string fileName in Sources.FileNames)
            {
                ProcessFile(fileName);
            }

            // Write map, which namespace is provided by what assembly
            WriteNamespaceMap();

            // Read in Additional Namespace Maps
            ReadNamespaceMaps();

            // Handle full qualified access and put it to usings
            AddRefsToUsings();

            // Read in assembly to uuid for project file Map
            if (File.Exists(_uuidfile))
            {
                Log(Level.Debug, "Read uuidfile " + _uuidfile + " ...");
                ReadMap(uuidfile, _assembly2uuid);
            }

            // Write the nant files for each directory
            WriteNantFiles();

            // Write uuid map, if updated to project file Map
            Log(Level.Debug, "Write uuidfile " + _uuidfile + " ...");
            WriteMap(_uuidfile, _assembly2uuid);
        }

        /// <summary>
        /// Iterate through all assemblies and check, which assembly needs to be referenced for
        /// full qualified access.
        /// The corresponding using is added to list in the AssemblyData instance.
        /// </summary>
        private void AddRefsToUsings()
        {
            foreach (AssemblyData assembly in _assembly2data.Values)
            {
                Log(Level.Debug, "Search reference for assembly '" + assembly.name + "'");

                foreach (string refToCheck in assembly.GetReference())
                {
                    string reference = refToCheck;
                    Log(Level.Debug, "Search reference '" + refToCheck + "'");

                    while (!_namespace2assembly.Contains(reference))
                    {
                        int idx = reference.LastIndexOf('.');

                        if (-1 == idx)
                        {
                            reference = null;
                            break;
                        }
                        else
                        {
                            reference = reference.Substring(0, idx);
                            Log(Level.Debug, "Next ref to test'" + reference + "'");
                        }
                    }

                    if (reference == null)
                    {
                        Log(Level.Warning, "No assembly found for '" + reference + "'");
                    }
                    else
                    {
                        Log(Level.Debug, "Assembly for reference'" + refToCheck + "' is in namespace '" + reference + "'");
                        assembly.AddUsing(reference); // Add the reference to using
                    }
                }
            }
        }

        /// <summary>
        /// Write the nant files
        /// </summary>
        private void WriteNantFiles()
        {
            Log(Level.Debug, "WriteNantFiles called (" + _assembly2data.Count.ToString() + " assemblies)");
            string targets = String.Empty;
            bool referenceAdded = false;
            ArrayList knownTargets = new ArrayList();
            string additionalRefsString = String.Empty;

            foreach (string addRef in _additionalRefs.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                additionalRefsString += string.Format(FSnippets[NANT_REFERENCE_PATTERNSET_TEMPLATE], addRef.Trim());
            }

            foreach (AssemblyData assembly in _assembly2data.Values)
            {
                Log(Level.Debug, "Processing assembly '" + assembly.name + "'");
                // Get all references we need
                Hashtable refsList = new Hashtable(); // All references we know about
                Hashtable pkgRefsList = new Hashtable(); // All references in our package

                foreach (string used in assembly.GetUsings())
                {
                    if (!_namespace2assembly.Contains(used))
                    {
                        if (used.StartsWith("Ict."))
                        {
                            Log(Level.Warning, "Namespace '" + used + "' not found in any assembly! Ignored!");
                        }

                        continue;
                    }

                    Log(Level.Debug, "Process using: " + used);

                    foreach (string usedAssembly in ((Hashtable)_namespace2assembly[used]).Keys)
                    {
                        Log(Level.Debug, "Process used assembly: " + usedAssembly);

                        if (usedAssembly != assembly.name)   // Do not add ourselfs
                        {
                            AddToDict(refsList, usedAssembly, usedAssembly);

                            if (usedAssembly.StartsWith(nsDefault))
                            {
                                AddToDict(pkgRefsList, usedAssembly, usedAssembly);
                            }
                        }
                    }
                }

                // Build the necessary strings
                string refs = String.Empty;

                foreach (string used in ToSortedArray(refsList.Keys))
                {
                    refs += string.Format(FSnippets[NANT_FILE_REFERENCE_INC_TEMPLATE], used);
                }

                string depstring = string.Join(",", (string[])ToSortedArray(pkgRefsList.Keys).ToArray(typeof(string)));

                string originalOutputType = _outputtype;

                // if there is a file called OutputType, then use the contained value
                if (File.Exists(assembly.directory + "/OutputType.cfg"))
                {
                    using (StreamReader sr = new StreamReader(assembly.directory + "/OutputType.cfg"))
                    {
                        _outputtype = sr.ReadLine();
                        sr.Close();
                    }
                }

                if (nsDefault == assembly.name)   // This one is written at the end
                {   // Add the references at the beginning of the targets
                    targets = string.Format(FSnippets[NANT_REFERENCE_TEMPLATE], assembly.name, refs + additionalRefsString,
                        depstring, GetUUID(assembly.name), _outputtype) + targets;
                    referenceAdded = true;
                }
                else
                {
                    // Write nant build file
                    String filename = Path.Combine(assembly.directory, assembly.name + "-generated.build");
                    Log(Level.Debug, "Write filename: " + filename);
                    StreamWriter sw = new StreamWriter(filename);
                    sw.Write(string.Format(FSnippets[NANT_BUILD_FILE_TEMPLATE], DATE_TIME_STRING,
                            assembly.name, refs + additionalRefsString, buildincfile,
                            GetUUID(assembly.name), _outputtype));
                    sw.Close();
                    // Add a target for compiling this namespace
                    targets += string.Format(FSnippets[NANT_FILE_TARGET_TEMPLATE], assembly.name, depstring, filename);
                }

                _outputtype = originalOutputType;

                knownTargets.Add(assembly.name);
            }

            // Write the included nant file
            knownTargets.Sort();
            targets += string.Format(FSnippets[NANT_FILE_TARGET_DEPENDS_TEMPLATE],
                string.Join(",", (string[])knownTargets.ToArray(typeof(string))));

            if (!referenceAdded)   // references were not added, but nant needs them
            {
                targets = Environment.NewLine + @"  <patternset id=""references""/>" + Environment.NewLine + targets;
            }

            Log(Level.Debug, "Write reference nantfile: " + refsfile);
            StreamWriter swRefs = new StreamWriter(refsfile);
            swRefs.Write(string.Format(FSnippets[NANT_FILE_REFS_DEPS_TEMPLATE], DATE_TIME_STRING, nsDefault, targets));
            swRefs.Close();
        }

        /// <summary>
        /// load the templates for the xml build files from a template file
        /// </summary>
        /// <param name="ATemplateFilePath">file to load the templates from</param>
        private void LoadTemplates(string ATemplateFilePath)
        {
            // using the idea from file csharp\ICT\PetraTools\CodeGeneration\ProcessTemplate.cs
            StreamReader r = File.OpenText(ATemplateFilePath);
            string TemplateCode = string.Empty;

            while (!r.EndOfStream)
            {
                string line = r.ReadLine().TrimEnd(new char[] { '\r', '\t', ' ', '\n' }).Replace("\t", "    ");
                TemplateCode += line + Environment.NewLine;
            }

            r.Close();

            // get the snippets
            FSnippets = new SortedList <String, String>();
            string[] snippets = Regex.Split(TemplateCode, "{##");

            for (int counter = 1; counter < snippets.Length; counter++)
            {
                string snippetName = snippets[counter].Substring(0, snippets[counter].IndexOf("}"));

                // exclude first newline
                string snippetText = snippets[counter].Substring(snippets[counter].IndexOf(Environment.NewLine) + Environment.NewLine.Length);

                // remove all whitespaces from the end, but keep one line ending for ENDIF etc
                snippetText = snippetText.TrimEnd(new char[] { '\n', '\r', ' ', '\t' }) + Environment.NewLine;

                FSnippets.Add(snippetName, snippetText);
            }
        }

        /// <summary>
        /// Take an ICollection, put it into an ArrayList, sort it and return it.
        /// </summary>
        /// <param name="col">The collection to sort</param>
        /// <returns>An sorted ArrayList</returns>
        private ArrayList ToSortedArray(ICollection col)
        {
            ArrayList rc = new ArrayList(col);

            rc.Sort();
            return rc;
        }

        private void WriteMap(string filename, Hashtable map)
        {
            // If the directory does not exist, we have to create it
            string dirname = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# Generated with CsDepend at " + DATE_TIME_STRING);

            foreach (string key in ToSortedArray(map.Keys))
            {
                foreach (string name in ToSortedArray(((Hashtable)map[key]).Values))
                {
                    sw.WriteLine(key + "=" + name);
                }
            }

            sw.Close();
        }

        /// <summary>
        /// Reads the filename in the format
        ///   key=val
        /// into the given hash table
        /// </summary>
        /// <param name="filename">file to open for reading</param>
        /// <param name="map">The map to write</param>
        private void ReadMap(string filename, Hashtable map)
        {
            StreamReader sr = new StreamReader(filename);

            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                Match match = exceptionRegex.Match(line);

                if (match.Success && (match.Groups.Count > 2))
                {
                    string key = match.Groups[1].ToString();
                    string val = match.Groups[2].ToString();
                    Log(Level.Debug, "Found map entry: " + key + "=" + val);
                    AddToDict(map, key, val);
                }
            }

            sr.Close();
        }

        /// <summary>
        /// Reads in the name space maps for all other assemblies
        /// </summary>
        private void ReadNamespaceMaps()
        {
            if (null == _nsDir)
            {
                return; // Not directory for namespace map. Abort.
            }

            string[] filePaths = Directory.GetFiles(_nsDir, "*.namespace-map");

            foreach (string filename in filePaths)
            {
                if (Path.GetFileNameWithoutExtension(filename) == nsDefault)
                {
                    continue; // We have read this file allready
                }

                // Read in the file
                Log(Level.Debug, "Read namespace map:" + filename);
                ReadMap(filename, _namespace2assembly);
            }
        }

        private string GetUUID(string assemblyname)
        {
            if (!_assembly2uuid.Contains(assemblyname))
            {
                Log(Level.Debug, "UUID for " + assemblyname + " not found!");
                AddToDict(_assembly2uuid, assemblyname, Guid.NewGuid().ToString("D").ToUpper());
            }

            IEnumerable uuids = ((Hashtable)(_assembly2uuid[assemblyname])).Keys;
            IEnumerator uuidsIter = uuids.GetEnumerator();
            uuidsIter.MoveNext();
            return (string)(uuidsIter.Current);
        }

        /// <summary>
        /// Writes a map with the known namespaces to assembly names into a file
        /// </summary>
        private void WriteNamespaceMap()
        {
            if (null == _nsDir)
            {
                return; // Not directory for namespace map. Abort.
            }

            string filename = Path.Combine(_nsDir, nsDefault + ".namespace-map");
            WriteMap(filename, _namespace2assembly);
        }

        /// <summary>
        /// Add the key to the map with the value. The map is interpreted as key => map of values
        /// </summary>
        /// <param name="map">The hashtable to add to</param>
        /// <param name="key">The string used as key</param>
        /// <param name="value">The string used as value</param>
        static private void AddToDict(Hashtable map, string key, string value)
        {
            Hashtable items;

            if (map.Contains(key))
            {
                items = (Hashtable)map[key];
            }
            else
            {
                items = new Hashtable();
                map.Add(key, items);
            }

            if (!items.Contains(value))
            {
                items.Add(value, value);
            }
        }

        /// <summary>
        /// Reads the given filename and analyzes it.
        /// Defined namespaces are added to _namespace2assembly map
        /// Used usings are found
        /// Used full qualified references are found
        /// </summary>
        /// <param name="filename"></param>
        private void ProcessFile(string filename)
        {
            string fileNamespace = null;
            // Get the assembly name derived from the directory
            string dirname = Directory.GetParent(filename).FullName;
            string assemblyname = dirname.Substring(basedir.Length);

            assemblyname = assemblyname.Replace(Path.AltDirectorySeparatorChar, '.');
            assemblyname = assemblyname.Replace(Path.DirectorySeparatorChar, '.');
            assemblyname = nsDefault + assemblyname;
            Log(Level.Debug, "ProcessFile:" + filename + " Assemblyname:" + assemblyname);
            AssemblyData assembly;

            if (_assembly2data.Contains(assemblyname))
            {
                assembly = (AssemblyData)_assembly2data[assemblyname];

                if (assembly.directory != dirname)
                {
                    Log(Level.Error, "The assembly '" + assemblyname + "' is in two directories ('" +
                        assembly.directory + "' and '" + dirname + "'!");
                }
            }
            else
            {
                assembly = new AssemblyData(dirname, assemblyname);
                _assembly2data.Add(assemblyname, assembly);
            }

            StreamReader sr = new StreamReader(filename);

            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                // Ignore Line comment
                Match matchComment = ignoreLineCommentRegex.Match(line);

                if (matchComment.Success && (matchComment.Groups.Count > 1))
                {
                    line = matchComment.Groups["noncomment"].ToString();
                }

                // Ignore string constants
                line = replaceStringConstantsRegex.Replace(line, "");
                // Check for using
                Match match = usingRegex.Match(line);

                if (match.Success && (match.Groups.Count > 1))
                {
                    string import = match.Groups[1].ToString();
                    Log(Level.Debug, "Found using: " + import);
                    assembly.AddUsing(import);
                }
                else
                {
                    // Check for namespace
                    Match nsMatch = namespaceRegex.Match(line);

                    if (nsMatch.Success && (nsMatch.Groups.Count > 1))
                    {
                        fileNamespace = nsMatch.Groups[1].ToString();
                        Log(Level.Debug, "Found namespace: " + fileNamespace);
                        // Add the namespace to the map
                        AddToDict(_namespace2assembly, fileNamespace, assemblyname);
                        assembly.AddNamespace(fileNamespace);
                    }
                    else
                    {
                        // Check for full qualified accesses
                        MatchCollection mc = absoluteRefRegex.Matches(line);

                        foreach (Match refMatch in mc)
                        {
                            if (refMatch.Success && (refMatch.Groups.Count > 1))
                            {
                                string refstring = refMatch.Groups[1].ToString();
                                Log(Level.Debug, "Found refMatch: " + refstring + " (" + line + ")");
                                assembly.AddReference(refstring);
                            }
                        }
                    }
                }
            }

            sr.Close();
            Log(Level.Debug, "File done: " + filename);
        }
    }
}