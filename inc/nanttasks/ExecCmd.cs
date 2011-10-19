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
    /// execute a command on the shell or command line
    /// </summary>
    [TaskName("ExecCmd")]
    public class ExecCmdTask : NAnt.Core.Tasks.ExecTask
    {
        private string FSuperUser = String.Empty;

        /// <summary>
        /// name of the privileged user
        /// </summary>
        [TaskAttribute("superuser", Required = false)]
        public string SuperUser {
            get
            {
                return FSuperUser;
            }
            set
            {
                FSuperUser = value;
            }
        }

        /// <summary>
        /// run the task
        /// </summary>
        protected override void ExecuteTask()
        {
            if (NAnt.Core.PlatformHelper.IsUnix)
            {
                this.FileName = "sh";
                this.CommandLineArguments = "-c" + this.CommandLineArguments.Substring(2);

                if (SuperUser.Length > 0)
                {
                    this.FileName = "sudo";
                    this.CommandLineArguments = "-u " + SuperUser + " " + this.CommandLineArguments.Substring(2);
                }
            }
            else if (NAnt.Core.PlatformHelper.IsWindows)
            {
                this.FileName = "cmd.exe";
                this.CommandLineArguments = "/C" + this.CommandLineArguments.Substring(2);
            }

            base.ExecuteTask();
        }
    }
}