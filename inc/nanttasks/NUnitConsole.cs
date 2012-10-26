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
        private string FTestCase = string.Empty;

        /// <summary>
        /// which dll to load and to run the tests contained in it
        /// </summary>
        [TaskAttribute("assemblyname", Required = true)]
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
        /// which test to run. if not defined, all tests will be run
        /// </summary>
        [TaskAttribute("testcase", Required = false)]
        public string TestCase {
            get
            {
                return FTestCase;
            }
            set
            {
                FTestCase = value;
            }
        }

        /// <summary>
        /// run the task
        /// </summary>
        protected override void ExecuteTask()
        {
            bool Failure = false;

            System.Diagnostics.Process process;
            process = new System.Diagnostics.Process();
            process.EnableRaisingEvents = false;

            if (!PlatformHelper.IsWindows)
            {
                process.StartInfo.FileName = "mono";
                process.StartInfo.Arguments = Project.Properties["external.NUnitConsole"] + " \"" + FAssemblyName + "\"";
            }
            else
            {
                string exeName = Project.Properties["external.NUnitConsole"];

                if (!exeName.Contains("(x86)"))
                {
                    exeName = exeName.Replace("nunit-console-x86.exe", "nunit-console.exe");
                }

                process.StartInfo.FileName = exeName;

                process.StartInfo.Arguments = "\"" + FAssemblyName + "\"";
            }

            if (FTestCase.Length > 0)
            {
                process.StartInfo.Arguments += " /run=" + FTestCase;
            }

            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(FAssemblyName);

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