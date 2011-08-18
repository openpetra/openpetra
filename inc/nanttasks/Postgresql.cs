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
using NAnt.Core;
using NAnt.Core.Attributes;
using System;
using System.Diagnostics;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// run commands against a MySQL database
    /// </summary>
    [TaskName("psql")]
    public class PsqlTask : NAnt.Core.Task
    {
        private string FPsqlExecutable;

        /// <summary>
        /// path to the psql executable
        /// </summary>
        [TaskAttribute("exe", Required = true)]
        public string PsqlExecutable {
            get
            {
                return FPsqlExecutable;
            }
            set
            {
                FPsqlExecutable = value;
            }
        }

        private string FDatabase;

        /// <summary>
        /// name of the database
        /// </summary>
        [TaskAttribute("database", Required = true)]
        public string Database {
            get
            {
                return FDatabase;
            }
            set
            {
                FDatabase = value;
            }
        }

        private string FSQLCommand = String.Empty;

        /// <summary>
        /// the sql command that should be executed
        /// </summary>
        [TaskAttribute("sqlcommand", Required = false)]
        public string SQLCommand {
            get
            {
                return FSQLCommand;
            }
            set
            {
                FSQLCommand = value;
            }
        }

        private string FSQLFile = String.Empty;

        /// <summary>
        /// name of the file that contains sql statements that should be executed
        /// </summary>
        [TaskAttribute("sqlfile", Required = false)]
        public string SQLFile {
            get
            {
                return FSQLFile;
            }
            set
            {
                FSQLFile = value;
            }
        }

        private string FOutputFile = String.Empty;

        /// <summary>
        /// name of the file that contains sql statements that should be executed
        /// </summary>
        [TaskAttribute("outputfile", Required = false)]
        public string OutputFile {
            get
            {
                return FOutputFile;
            }
            set
            {
                FOutputFile = value;
            }
        }

        private string FPassword = String.Empty;

        /// <summary>
        /// the password of the database user
        /// </summary>
        [TaskAttribute("password", Required = false)]
        public string Password {
            get
            {
                return FPassword;
            }
            set
            {
                FPassword = value;
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
            process.StartInfo.FileName = FPsqlExecutable;

            Environment.SetEnvironmentVariable("PGPASSWORD", FPassword, EnvironmentVariableTarget.Process);

            if (FSQLCommand.Length > 0)
            {
                process.StartInfo.Arguments = "-c \"" + FSQLCommand + "\"";
                Log(Level.Info, FSQLCommand);
            }
            else if (FSQLFile.Length > 0)
            {
                process.StartInfo.Arguments = "-f \"" + FSQLFile + "\"";
                Log(Level.Info, "Load sql commands from file: " + FSQLFile);
            }

            if (FOutputFile.Length > 0)
            {
                process.StartInfo.Arguments += " -t -o \"" + FOutputFile + "\"";
            }

            process.StartInfo.Arguments += " " + FDatabase;

            string SuperUser = string.Empty;

            if (NAnt.Core.PlatformHelper.IsUnix)
            {
                SuperUser = "postgres";
            }

            if (SuperUser.Length > 0)
            {
                process.StartInfo.FileName = "sudo";
                process.StartInfo.Arguments = "-u " + SuperUser + " " + FPsqlExecutable + " " + process.StartInfo.Arguments;
            }

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

            foreach (string line in output)
            {
                if (!(line.Trim().StartsWith("INSERT") || line.Trim().StartsWith("GRANT") || line.Trim().StartsWith("COPY")
                      || line.Trim().StartsWith("DELETE")))
                {
                    Console.WriteLine(line);
                }

                if (line.Contains(" error at"))
                {
                    Failure = true;
                }
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