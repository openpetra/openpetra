//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.IO;
using System.Diagnostics;

namespace Ict.Tools.NAntTasks
{
    /// <summary>
    /// run commands against a MySQL database
    /// </summary>
    [TaskName("mysql")]
    public class MysqlTask : NAnt.Core.Task
    {
        private string FMysqlExecutable;

        /// <summary>
        /// path to the MySQL executable
        /// </summary>
        [TaskAttribute("exe", Required = true)]
        public string MysqlExecutable {
            get
            {
                return FMysqlExecutable;
            }
            set
            {
                FMysqlExecutable = value;
            }
        }

        private string FDatabase = String.Empty;

        /// <summary>
        /// name of the database
        /// </summary>
        [TaskAttribute("database", Required = false)]
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
        /// name of the file that should receive the output from the MySQL command
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

        private string FUser = String.Empty;

        /// <summary>
        /// the database user
        /// </summary>
        [TaskAttribute("user", Required = false)]
        public string User {
            get
            {
                return FUser;
            }
            set
            {
                FUser = value;
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
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.EnableRaisingEvents = false;
            process.StartInfo.FileName = FMysqlExecutable;

            if (FSQLCommand.Length > 0)
            {
                process.StartInfo.RedirectStandardInput = true;

                // do not print the password
                string SqlPrint = FSQLCommand;

                int pos;

                if ((pos = SqlPrint.IndexOf("IDENTIFIED BY '")) != -1)
                {
                    SqlPrint = SqlPrint.Substring(0, pos) + " IDENTIFIED BY 'xxx" +
                               SqlPrint.Substring(SqlPrint.IndexOf("'", pos + "IDENTIFIED BY '".Length));
                }

                Log(Level.Info, SqlPrint);
            }
            else if (FSQLFile.Length > 0)
            {
                process.StartInfo.RedirectStandardInput = true;
                Log(Level.Info, "Load sql commands from file: " + FSQLFile);
            }

            // add -v before -u if you want to see the sql commands...

            if (FPassword.Length > 0)
            {
                process.StartInfo.Arguments += " --password=" + FPassword;
            }

            if (FUser.Length > 0)
            {
                process.StartInfo.Arguments += " --user=" + FUser;
            }

            if (FDatabase.Length > 0)
            {
                process.StartInfo.Arguments += " --database=" + FDatabase;
            }

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
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

            if ((FSQLCommand.Length > 0) && (process.StandardInput != null))
            {
                process.StandardInput.WriteLine(FSQLCommand);
                process.StandardInput.Close();
            }
            else if (FSQLFile.Length > 0)
            {
                StreamReader sr = new StreamReader(FSQLFile);
                process.StandardInput.Write(sr.ReadToEnd());
                process.StandardInput.Close();
                sr.Close();
            }

            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(500);
            }

            // TODO: what about error output?
            // see also http://csharptest.net/?p=321 about how to use Process class

            string[] output = process.StandardOutput.ReadToEnd().Split('\n');

            foreach (string line in output)
            {
                if (!(line.Trim().StartsWith("INSERT") || line.Trim().StartsWith("GRANT") || line.Trim().StartsWith("COPY")
                      || line.Trim().StartsWith("DELETE")))
                {
                    Console.WriteLine(line);
                }
            }

            if (FOutputFile.Length > 0)
            {
                StreamWriter sw = new StreamWriter(FOutputFile);

                foreach (string line in output)
                {
                    sw.WriteLine(line);
                }

                sw.Close();
            }

            if (FailOnError && (process.ExitCode != 0))
            {
                throw new Exception("Exit Code " + process.ExitCode.ToString() + " shows that something went wrong");
            }
        }
    }
}