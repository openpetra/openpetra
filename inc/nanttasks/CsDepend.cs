using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;

namespace Ict.Tools.NAntTasks
{
    [TaskName("CsDepend")]
    public class CsDependClass : NAnt.Core.Task 
    {


        const string NANT_FILE_REFS_DEPS_TEMPLATE =
@"<?xml version=""1.0""?>
<project name=""OpenPetra-csharp-{0}-refs-deps-generated"">

{1}

</project>
"; 

        const string NANT_FILE_TARGET_DEPENDS_TEMPLATE =
@"
<target name=""nant-subcall"" depends=""{0}"">
</target>
"; 

        const string NANT_FILE_TARGET_TEMPLATE =
@"
<target name=""{0}"" depends=""{1}"" description=""calls target from property target for {0}"">
  <nant inheritall=""false"" target=""${{target}}"" buildfile=""{2}"" />
</target>
";


        const string NANT_BUILD_FILE_TEMPLATE =
@"<?xml version=""1.0""?>
<project name=""OpenPetra-csharp-{0}-build-generated"">
<property name=""Namespace"" value=""{0}"" />

<property name=""RunCsDepend"" value=""false"" />

<include buildfile=""{2}""/> 

<patternset id=""references"">{1}
  <patternset refid=""3rdPartyPattern"" />
</patternset>

</project>
";
        const string NANT_REFERENCE_TEMPLATE =
@"<patternset id=""references"">{1}
  <patternset refid=""3rdPartyPattern"" />
</patternset>

<target name=""{0}"" depends=""{2}"" description=""calls target from property target"">
  <!-- RunCsDepend was already executed and the file imported. Now we can do the real task -->
  <property name=""RunCsDepend"" value=""false"" overwrite=""true"" />
  <property name=""target"" value=""compile"" overwrite=""false"" />
  <call target=""${{target}}"" cascade=""false"" />
</target>
";
        const string NANT_FILE_REFERENCE_INC_TEMPLATE = @"
  <include name=""{0}.dll""/>";

/*        const string NANT_FILE_REFERENCE_CSPROJ_TEMPLATE = @"
<Reference Include=""{0}"" />";
        */
        static Regex usingRegex = new Regex("^\\s*using\\s*([a-zA-Z.]+);");
        static Regex absoluteRefRegex = new Regex(@"[^a-zA-Z.]+(Ict\.[a-zA-Z.]+)\.[a-zA-Z]+[^a-zA-Z.]+");
        static Regex ignoreLineCommentRegex = new Regex(@"^(?<noncomment>.*?)//");
        static Regex replaceStringConstantsRegex = new Regex(@"""([^""]*?)""");
        static Regex namespaceRegex = new Regex("^\\s*namespace\\s+([a-zA-Z.]+)");
        static Regex exceptionRegex = new Regex("^\\s*([a-zA-Z.]+)\\s*=\\s*([a-zA-Z.]+)");

        // Holds t
        private Hashtable _depsNameSpace = new Hashtable();
        // Holds the exception form namespace to directory/dll
        private Hashtable _nameSpaceExceptions = new Hashtable(); 
        
        private FileSet _sources = new FileSet();
        /// <summary>
        /// The set of source files for checking for dependencies.
        /// </summary>
        [BuildElement("sources", Required=true)]
        public FileSet Sources {
            get { return _sources; }
            set { _sources = value; }
        }
        private string _nsExceptions = String.Empty;
        [TaskAttribute("ns-exceptions", Required = false)]
        public string nsException {
            get
            {
                return _nsExceptions;
            }
            set
            {
                _nsExceptions = value;
            }
        }
        private string _nsDefault = String.Empty;
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
        [TaskAttribute("basedir", Required = true)]
        public string basedir {
            get
            {
                return _basedir;
            }
            set
            {
                _basedir= value;
            }
        }

        private string _refsfile = String.Empty;
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

        private string _buildincfile = String.Empty;
        [TaskAttribute("buildincfile", Required = true)]
        public string buildincfile {
            get {
                return _buildincfile;
            }
            set {
                _buildincfile = value;
            }
        }

        protected override void ExecuteTask() {
            Log(Level.Info, "Running for namespace " + nsDefault + " ...");
            if (Sources.BaseDirectory == null) {   
                Sources.BaseDirectory = new DirectoryInfo(Project.BaseDirectory);
            }
        
            // Read in exception list
            ReadExceptionMap(nsException);
            
            // Now check all sourcefiles
            foreach (string fileName in Sources.FileNames) {
                ProcessFile(fileName);
            }
            WriteNantDepFile();
        }

        private void ReadExceptionMap(string exceptionFilename)
        {
            Log(Level.Debug, "ReadExceptionMap:" + exceptionFilename);
            StreamReader sr = new StreamReader(exceptionFilename);
            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                Match match = exceptionRegex.Match(line);
                if (match.Success && match.Groups.Count > 2)
                {
                    string key = match.Groups[1].ToString();
                    string val = match.Groups[2].ToString();
                    Log(Level.Debug, "Found exception: " + key + "=" + val);
                    if (_nameSpaceExceptions.Contains(key)) {
                      Log(Level.Warning, "Duplicate exception entry in file " + exceptionFilename + " for " + key + "! Entry ignored!");
                    } else {
                      _nameSpaceExceptions.Add(key, val);
                    }
                }
              }
            sr.Close();
        }
        
        public static string NamespaceToPath(string ns) {
            // Build path to file. The dots are the directory seperators
            string[] filenameParts = ns.Split('.');
            string filename = null;
            foreach (string part in filenameParts) {
                if (filename == null) {
                    filename = part;
                } else {
                    filename = Path.Combine(filename, part);
                }
            }
            return filename;
        }

        private void WriteNantDepFile()
        {
            ArrayList depsNames = new ArrayList();
            string targets = "";
            bool refsadded = false;
            foreach (string key in _depsNameSpace.Keys) {
                if (key.StartsWith(nsDefault)) {
                    depsNames.Add(key);
                    ArrayList depsArray = new ArrayList(((Hashtable)_depsNameSpace[key]).Values);
                    depsArray.Sort();
                    
                    // Generate Nant file for references
                    string refs = "";
//                    string refsCsproj = "";
                    ArrayList depsTargetArray = new ArrayList();
                    foreach (string dep in depsArray) {
//                        refsCsproj += string.Format(NANT_FILE_REFERENCE_CSPROJ_TEMPLATE, dep);
                        // Remember dep for dependency between targets in nant file
                        if (dep.StartsWith("Ict.") && dep != key) {
                            refs += string.Format(NANT_FILE_REFERENCE_INC_TEMPLATE, dep);
                        }
                        if (dep.StartsWith(nsDefault) && dep != key) {
                            depsTargetArray.Add(dep);
                        }   
                    }
                    // Build the dependency string
                    string depstring = string.Join(",", (string[])depsTargetArray.ToArray(typeof(string)));
                    Log(Level.Debug, "name: " + nsDefault + "   key: " + key);
                    String filename = null;
                    if (key.Length > nsDefault.Length) {
                        filename = key.Substring(nsDefault.Length + 1);
                    }
                    if (!String.IsNullOrEmpty( filename )) {
                        // Build path to file. The dots are the directory seperators
                        filename = NamespaceToPath(filename);
                        filename = Path.Combine(filename, key + "-generated.build");
                        Log(Level.Debug, "Write filename: " + filename);
                        // Write nant build file
                        StreamWriter sw = new StreamWriter(filename);
                        sw.Write(string.Format(NANT_BUILD_FILE_TEMPLATE, key, refs, buildincfile));
                        sw.Close();
                        // Add a target for compiling this namespace
                        targets += string.Format(NANT_FILE_TARGET_TEMPLATE, key, depstring, filename);
                        // Add new traget to all deps
                    } else { // The references file for this namespace
                        // Add the references at the beginning of the targets
                        targets = string.Format(NANT_REFERENCE_TEMPLATE, key, refs, depstring) + targets;
                        refsadded = true;
                    }
                } else {
                  Log(Level.Warning, "Namespace " + key + " not expected! Ignoring!");
                }
            }
            // Add dependency for calling all namespace targets
            depsNames.Sort();
            targets += string.Format(NANT_FILE_TARGET_DEPENDS_TEMPLATE,
                                           string.Join(",", (string[])depsNames.ToArray(typeof(string))));
            if (! refsadded) { // references were not added, but nant need them
                targets = @"
  <patternset id=""references""/>
" + targets;
            }
            Log(Level.Debug, "Write reference nantfile: " + refsfile);
            StreamWriter swRefs = new StreamWriter(refsfile);
            swRefs.Write(string.Format(NANT_FILE_REFS_DEPS_TEMPLATE, nsDefault, targets));
            swRefs.Close();

        }
        private void AddImport(string import, string fileNamespace, Hashtable usingStatements) {
            if (_nameSpaceExceptions.Contains(import)) {
                string old = import;
                import = (string)_nameSpaceExceptions[import];
                Log(Level.Debug, "Namespace exception found: " + old + "=" + import);
            }
            // Ignore duplicate usings. They could occur because of the exception list
            if (!usingStatements.Contains(import)) {
                usingStatements.Add(import, import);
            }
        }
        private void ProcessFile(string filename)
        {
            Log(Level.Debug, "ProcessFile:" + filename);
            StreamReader sr = new StreamReader(filename);
            Hashtable usingStatements = new Hashtable();
            string fileNamespace = null;

            while (sr.Peek() >= 0)
            {
                string line = sr.ReadLine();
                // Ignore Line comment
                Match matchComment = ignoreLineCommentRegex.Match(line);
                if (matchComment.Success && matchComment.Groups.Count > 1) {
                    string old = line;
                    line = matchComment.Groups["noncomment"].ToString();
                    Log(Level.Debug, "Found Comment: " + line + " (" + old + ")");
                }
                // Ignore string constants
                line = replaceStringConstantsRegex.Replace(line, "");
                Match match = usingRegex.Match(line);
                if (match.Success && match.Groups.Count > 1)
                {
                    string import = match.Groups[1].ToString();
                    Log(Level.Debug, "Found using: " + import);
                    AddImport(import, fileNamespace, usingStatements);
                } else  {
                    Match nsMatch = namespaceRegex.Match(line);
                    if (nsMatch.Success && nsMatch.Groups.Count > 1) {
                        fileNamespace = nsMatch.Groups[1].ToString();
                        Log(Level.Debug, "Found namespace: " + fileNamespace);
                        if (_nameSpaceExceptions.Contains(fileNamespace)) { // Apply exception
                            string old = fileNamespace;
                            fileNamespace = (string)_nameSpaceExceptions[fileNamespace];
                            Log(Level.Debug, "Namespace exception found: " + old + "=" + fileNamespace);
                        } 
                        // check, if namespace starts with preset namespace
                        if (! fileNamespace.StartsWith(nsDefault)) {
                            // This namespace should not be in this package!
                            Log(Level.Warning, "File " + filename + " has namespace " + fileNamespace + "! Expected starting with: "+ nsDefault);
                        } else {
                            // Transform namespace name to directory name
                            Log(Level.Debug, "Found namespace to check: " + fileNamespace);
                            Log(Level.Debug, "Filename: " + filename);
                            string dir = basedir;
                            if (fileNamespace.Length > nsDefault.Length) {
                                dir = Path.Combine(dir, NamespaceToPath(fileNamespace.Substring(nsDefault.Length + 1)));
                            }
                            Log(Level.Debug, "Expected dir: " + dir);
                            if (Directory.GetParent(filename).FullName != dir) {
                                Log(Level.Error, "File " + filename + " should be in directory " + dir + "!");
                                // Build namespace exception entry @TODO mgr
                                string origdir = Directory.GetParent(filename).FullName;
                                string nsNew = origdir.Substring(basedir.Length);
                                nsNew = nsNew.Replace(Path.AltDirectorySeparatorChar, '.');
                                nsNew = nsNew.Replace(Path.DirectorySeparatorChar, '.');
                                Log(Level.Debug, "nsNew=" + nsNew);
                                Log(Level.Error, "Please, add: " + fileNamespace + "=" + nsDefault + nsNew);
                            }                                
                        }
                        if (_depsNameSpace.Contains(fileNamespace)) {
                            Log(Level.Debug, "Namespace already found. Add using statements");
                            Hashtable entry = (Hashtable)_depsNameSpace[fileNamespace];
                            foreach (string val in usingStatements.Values) {
                                if (!entry.Contains(val)) {  // Only add new entries
                                    Log(Level.Debug, "New using statement: " + val);
                                    entry.Add(val, val);
                                }
                            }
                        } else {
                            _depsNameSpace.Add(fileNamespace, usingStatements);
                        }
                    } else {
                      MatchCollection mc = absoluteRefRegex.Matches(line);
                      foreach (Match refMatch in mc) {
                          if (refMatch.Success && refMatch.Groups.Count > 1) {
                              string refstring = refMatch.Groups[1].ToString();
                              Log(Level.Debug, "Found refMatch: " + refstring + " (" + line + ")");
                              AddImport(refstring, fileNamespace, usingStatements); // Add, as it would be a using
                          }
                      }
                    }
                }
            }
            sr.Close();
            if (null != fileNamespace) {
                // Just add the refs we have perhaps found later.
                if (_depsNameSpace.Contains(fileNamespace)) {
                    Log(Level.Debug, "Namespace already found. Add using statements");
                    Hashtable entry = (Hashtable)_depsNameSpace[fileNamespace];
                    foreach (string val in usingStatements.Values) {
                        if (!entry.Contains(val)) {  // Only add new entries
                            Log(Level.Debug, "New using statement: " + val);
                            entry.Add(val, val);
                        }
                    }
                } else {
                    _depsNameSpace.Add(fileNamespace, usingStatements);
                }
            }
            Log(Level.Debug, "File done: " + filename);
        }
    }

}