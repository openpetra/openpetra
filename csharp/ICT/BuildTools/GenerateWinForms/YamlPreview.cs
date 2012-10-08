//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Resources;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.CodeGeneration.Winforms;

namespace Ict.Tools.GenerateWinForms
{
    /// <summary>
    /// Description of YamlPreview.
    /// </summary>
    public partial class TFrmYamlPreview : Form
    {
        string FFilename;
        string FSelectedLocalisation;

        /// <summary>
        /// constructor
        /// </summary>
        public TFrmYamlPreview(string filename, string ASelectedLocalisation)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            FFilename = filename;
            FSelectedLocalisation = ASelectedLocalisation;
        }

        private static void ProcessFile(string filename, string ASelectedLocalisation)
        {
            TProcessYAMLForms processor = new TProcessYAMLForms(filename, ASelectedLocalisation);

            // report is at the moment the only real different type of screen,
            // because it uses different controls
            // otherwise, the Template attribute is also quite important, because it determines which code is written
            processor.AddWriter("navigation", typeof(TWinFormsWriter));
            processor.AddWriter("edit", typeof(TWinFormsWriter));
            processor.AddWriter("dialog", typeof(TWinFormsWriter));
            processor.AddWriter("report", typeof(TWinFormsWriter));
            processor.AddWriter("browse", typeof(TWinFormsWriter));

            processor.ProcessDocument();
        }

        CompilerResults CompileForm(string src, string NamespaceAndClass)
        {
            CSharpCodeProvider csc = new CSharpCodeProvider(
                new Dictionary <string, string>() {
                    { "CompilerVersion", "v4.0" }
                });
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            string binDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // reference all dlls in the bin directory
            string[] dllnames = Directory.GetFiles(binDirectory, "*.dll");

            foreach (string dllName in dllnames)
            {
                if (!dllName.EndsWith("sqlite3.dll"))
                {
                    parameters.ReferencedAssemblies.Add(dllName);
                }
            }

            string ResourceXFile = FFilename.Replace(".yaml", "-generated.resx");
            //"../../../../tmp/" +
            string ResourcesFile = NamespaceAndClass + ".resources";

            if (File.Exists(ResourceXFile))
            {
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

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, src);

            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    TLogging.Log(error.ToString());
                }
            }

            return results;
        }

        Form WindowToOpen;

        void ShowWindow()
        {
            WindowToOpen.ShowDialog();
        }

        bool ShowScreen(Assembly AAssembly, string strTypeName)
        {
            try
            {
                Type type = AAssembly.GetType(strTypeName);

                if (type == null)
                {
                    TLogging.Log("cannot find type " + strTypeName);

                    foreach (Type t in AAssembly.GetTypes())
                    {
                        TLogging.Log(" found " + t.ToString());
                    }

                    return false;
                }

                Control obj = (Control)Activator.CreateInstance(type);

                if (obj is Form)
                {
                    WindowToOpen = (Form)obj;

                    Thread t = new Thread(new ThreadStart(ShowWindow));
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                }
                else
                {
                    obj.Dock = DockStyle.Fill;

                    pnlWindow.Controls.Clear();
                    pnlWindow.Controls.Add(obj);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }

            return false;
        }

        void btnPreviewClick(object sender, EventArgs e)
        {
            TParseYAMLFormsDefinition.ClearCachedYamlFiles();

            // generate the code
            TFrmYamlPreview.ProcessFile(FFilename, FSelectedLocalisation);

            // load the designer code
            StreamReader sr = new StreamReader(FFilename.Replace(".yaml", "-generated.Designer.cs"));
            StringBuilder DesignerCode = new StringBuilder();

            string Namespace = string.Empty;
            string ClassName = "T" + Path.GetFileNameWithoutExtension(FFilename);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                if (line.StartsWith("namespace "))
                {
                    Namespace = line.Substring("namespace ".Length);
                }

                if (line.Contains("+= new "))
                {
                    // ignore event handlers
                }
                else if (line.Contains(".ListTable = "))
                {
                    // ignore TtxtAutoPopulatedButtonLabel.set_ListTable
                }
                else if (line.Contains("partial class"))
                {
                    ClassName = line.Substring("    partial class ".Length);

                    if (Path.GetFileNameWithoutExtension(FFilename).StartsWith("UC"))
                    {
                        line = line.Replace("partial", string.Empty) + ": UserControl {";
                    }
                    else
                    {
                        line = line.Replace("partial", string.Empty) + ": Form {";
                    }

                    DesignerCode.Append(line).Append(Environment.NewLine);
                    line = "public " + ClassName + "() : base()     { InitializeComponent(); }";
                    DesignerCode.Append(line).Append(Environment.NewLine);
                    // read opening curly bracket
                    line = sr.ReadLine();
                }
                else
                {
                    DesignerCode.Append(line).Append(Environment.NewLine);
                }
            }

            sr.Close();

            if (TLogging.DebugLevel > 0)
            {
                StreamWriter sw = new StreamWriter("../../../../log/tempPreviewWinforms.cs");
                sw.WriteLine(DesignerCode.ToString());
                sw.Close();
            }

            // compile the designer code
            CompilerResults results = CompileForm(DesignerCode.ToString(), Namespace + "." + ClassName);

            if (results.Errors.HasErrors)
            {
                TLogging.Log(results.Errors.ToString());
                return;
            }

            // open the form
            ShowScreen(results.CompiledAssembly, Namespace + "." + ClassName);
        }

        bool RunOnlyOnce = true;

        void TFrmYamlPreviewActivated(object sender, EventArgs e)
        {
            if (RunOnlyOnce)
            {
                RunOnlyOnce = false;
                btnPreviewClick(null, null);
            }
        }
    }
}