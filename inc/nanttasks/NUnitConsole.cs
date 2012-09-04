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
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.NUnit2;
using NAnt.NUnit2.Tasks;
using NAnt.NUnit2.Types;
using NAnt.NUnit.Types;
using System;
using System.IO;
using System.Diagnostics;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// run NUnit tests on the console
    /// </summary>
    [TaskName("NUnitConsole")]
    public class NUnitConsoleTask : NAnt.Core.Task
    {
        private string FAssemblyName = string.Empty;

        /// <summary>
        /// which dll to load and to run the tests contained in it
        /// </summary>
        [TaskAttribute("assemblyname", Required = false)]
        public string AssemblyName {
            get
            {
                return FAssemblyName;
            }
            set
            {
                FAssemblyName = value;
            }
        }

        /// <summary>
        /// run the task
        /// </summary>
        protected override void ExecuteTask()
        {
            if (!PlatformHelper.IsWindows)
            {
                // use the existing NUnit2 task
                NUnit2Task origTask = new NUnit2Task();
                this.CopyTo(origTask);

                NUnit2Test test = new NUnit2Test();
                FormatterElement formatter = new FormatterElement();
                formatter.Type = FormatterType.Plain;
                origTask.FormatterElements.Add(formatter);
                test.AssemblyFile = new FileInfo(FAssemblyName);
                origTask.Tests.Add(test);

                origTask.Execute();

                return;
            }

            bool Failure = false;

            System.Diagnostics.Process process;
            process = new System.Diagnostics.Process();
            process.EnableRaisingEvents = false;

            string exeName = Project.Properties["external.NUnitConsole"];

            if (!exeName.Contains("(x86)"))
            {
                exeName = exeName.Replace("nunit-console-x86.exe", "nunit-console.exe");
            }

            process.StartInfo.FileName = "\"" + exeName + "\"";

            process.StartInfo.Arguments = "\"" + FAssemblyName + "\"";
            process.StartInfo.WorkingDirectory = "\"" + Path.GetDirectoryName(FAssemblyName) + "\"";

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;
            try
            {
                if (!process.Start())
                {
                    throw new Exception("cannot start " + process.StartInfo.FileName);
                }
            }
            catch (Exception exp)
            {
                throw new Exception("cannot start " + process.StartInfo.FileName + Environment.NewLine + exp.Message);
            }

            string[] output = process.StandardError.ReadToEnd().Split('\n');

            foreach (string s in output)
            {
                Console.WriteLine(s);
            }

            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(500);
            }

            if (FailOnError && (process.ExitCode != 0))
            {
                throw new Exception("Exit Code " + process.ExitCode.ToString() + " shows that something went wrong");
            }

            if (FailOnError && Failure)
            {
                throw new Exception("Output shows that something went wrong");
            }
        }
    }
}