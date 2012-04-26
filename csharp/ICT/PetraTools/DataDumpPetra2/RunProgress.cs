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
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// run a progress program
    /// </summary>
    public class TRunProgress
    {
        /// <summary>
        /// run a .r file in Progress with the given parameters
        /// </summary>
        public static bool RunProgress(String AProgram, String AParameters, String AOutput)
        {
            string petraHome = Environment.GetEnvironmentVariable("PETRA_HOME");

            if ((petraHome == null) || (petraHome.Length == 0))
            {
                throw new Exception("need to run petraenv.sh first");
            }

            System.Diagnostics.Process ProgressProcess = new System.Diagnostics.Process();
            ProgressProcess.EnableRaisingEvents = false;

            if (Utilities.DetermineExecutingOS() == TExecutingOSEnum.eosLinux)
            {
                ProgressProcess.StartInfo.FileName = "sh";         // . petra23env.sh should have been called

                ProgressProcess.StartInfo.Arguments = "-c \"_progres -1 -b -pf " + petraHome + "/etc/batch.pf -yy 1980 -yr4def -p ./" +
                                                      AProgram;

                if (AParameters.Length > 0)
                {
                    ProgressProcess.StartInfo.Arguments += " -param " + AParameters;
                }

                ProgressProcess.StartInfo.Arguments += " | cat ";

                ProgressProcess.StartInfo.Arguments += "\"";
            }
            else         // windows
            {
                string ProgressExe = TAppSettingsManager.GetValue("Progress4GL.Executable", "_progres.exe");
                ProgressExe = ProgressExe.Replace("prowin32.exe", "_progres.exe");

                ProgressProcess.StartInfo.FileName = ProgressExe;

                ProgressProcess.StartInfo.Arguments =
                    TAppSettingsManager.GetValue("Progress4GL.Parameters").Replace("petra23.pf", "petra23-single.pf") +
                    " -b -p " + AProgram;

                if (AParameters.Length > 0)
                {
                    ProgressProcess.StartInfo.Arguments += " -param " + AParameters;
                }
            }

            ProgressProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            ProgressProcess.EnableRaisingEvents = true;
            ProgressProcess.StartInfo.UseShellExecute = false;
            ProgressProcess.StartInfo.RedirectStandardOutput = true;

            if ((!ProgressProcess.Start()))
            {
                return false;
            }

            string output = ProgressProcess.StandardOutput.ReadToEnd();

            if (output.Contains("in use in multi-user mode. (276)"))
            {
                throw new Exception("Please stop the Petra 2.x database by running sysadm petra23 stop");
            }

            if (AOutput.Length > 0)
            {
                StreamWriter sw = new StreamWriter(AOutput, true);
                sw.WriteLine(output);
                sw.Close();
            }

            while ((!ProgressProcess.HasExited))
            {
                Thread.Sleep(500);
            }

            ProgressProcess.Close();
            return true;
        }
    }
}