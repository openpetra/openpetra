//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ict.Tools.DevelopersAssistant
{
    /************************************************************************************************************************************
     *
     * This class handles the low level stuff involved in calling Windows to start or stop Processes.
     * All the methods and properties in this class are static, so we never instantiate this class.
     *
     * *********************************************************************************************************************************/

    class NantExecutor
    {
        // We use PostMessage to minimise the server window
        [return : MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const UInt32 WM_SYSCOMMAND = 0x0112;
        private const UInt32 SC_MINIMIZE = 0xF020;


        // This is the processID for the cmd window that is hosting the server process (rather than the server ProcessID itself)
        // We need to keep this so that when we have successfully stopped the server we can kill its containing cmd window
        private static int _serverProcessID = 0;
        private static bool _serverProcessIdIsCmdWindow = false;

        /// <summary>
        /// Checks if the server console process is running and, if necessary, determines the ProcessID of the containing command window
        /// </summary>
        /// <returns>True if the server is running</returns>
        public static bool IsServerRunning()
        {
            Process[] allProcesses = Process.GetProcessesByName("PetraServerConsole");
            bool bIsRunning = allProcesses.Length > 0;

            if (bIsRunning && (_serverProcessID == 0))
            {
                // Theoretically there could be more than one server console running.  If there are we will just work with the first one
                //  and then work out which cmd window is associated with it
                DateTime dtServerStart = allProcesses[0].StartTime;

                // Now go round all cmd.exe processes to find the one that started just before
                // We assume that it will be within 5 seconds
                Process[] cmdProcesses = Process.GetProcessesByName("cmd");

                if (cmdProcesses.Length > 0)
                {
                    TimeSpan tsSmallest = TimeSpan.FromDays(36500);      // assume the smallest time span so far is 100 years!

                    for (int i = 0; i < cmdProcesses.Length; i++)
                    {
                        TimeSpan ts = dtServerStart - cmdProcesses[i].StartTime;

                        if (ts >= TimeSpan.Zero)
                        {
                            // This cmd window did start before the server console process
                            if (ts < tsSmallest)
                            {
                                _serverProcessID = cmdProcesses[i].Id;
                                _serverProcessIdIsCmdWindow = true;
                                tsSmallest = ts;
                            }
                        }
                    }
                }
                else
                {
                    _serverProcessID = allProcesses[0].Id;
                    _serverProcessIdIsCmdWindow = false;
                }
            }

            if (!bIsRunning)
            {
                _serverProcessID = 0;
            }

            return bIsRunning;
        }

        /// <summary>
        /// Starts the server and stores the ProcessID of the command window that hosts it
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the server is to be started</param>
        /// <param name="StartMinimized">Set to True if the server window is to be minimized at startup</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool StartServer(string BranchLocation, bool StartMinimized)
        {
            _serverProcessID = 0;
            bool bRet = LaunchExe("nant.bat", "startPetraServer -logfile:opda.txt", BranchLocation);

            if (bRet)
            {
                // Just wait for things to settle
                System.Threading.Thread.Sleep(2000);

                // Call IsServerRunning - this will work out the value for _serverProcessID
                IsServerRunning();

                if (_serverProcessID == 0)
                {
                    OutputText.AppendText(OutputText.OutputStream.Both,
                        "\r\nThe startServer task was executed but the Assistant failed to figure out the cmd window ID\r\n");
                }
                else if (StartMinimized)
                {
                    PostMessage(new HandleRef(null, Process.GetProcessById(_serverProcessID).MainWindowHandle), WM_SYSCOMMAND, new IntPtr(
                            SC_MINIMIZE), new IntPtr(0));
                }
            }

            return bRet;
        }

        /// <summary>
        /// Stops the server process and closes the command window that hosted it.  (In exceptional circumstances it may not be possible to close the DOS window)
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the server is to be stopped</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool StopServer(string BranchLocation)
        {
            // Launch the stop task
            bool bRet = LaunchExe("nant.bat", "stopPetraServer -logfile:opda.txt", BranchLocation);

            if (bRet && _serverProcessIdIsCmdWindow)
            {
                // That command window will have closed but we will be left with the cmd window that hosted the server
                // We find all cmd windows that are running:  if we know the ProcessID that we started we can just kill that one
                // Otherwise, if there is only one running now we can close it
                // Failing that we do not know which command window to close, so we have to leave it
                Process[] curProcesses = Process.GetProcessesByName("cmd");
                bool bFound = false;

                if (_serverProcessID > 0)
                {
                    foreach (Process p in curProcesses)
                    {
                        if (p.Id == _serverProcessID)
                        {
                            p.Kill();
                            bFound = true;
                        }
                    }
                }
                else if (curProcesses.Length == 1)
                {
                    // There is only one cmd window, so we can close it anyway
                    curProcesses[0].Kill();
                    bFound = true;
                }

                if (!bFound)
                {
                    OutputText.AppendText(OutputText.OutputStream.Both,
                        String.Format("\r\nFailed to find ProcessID {0} so cmd window was not closed\r\n", _serverProcessID));
                }
            }

            _serverProcessID = 0;

            return bRet;
        }

        /// <summary>
        /// Runs a typical Nant target with no parameters
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the task is to be run</param>
        /// <param name="NantTarget">The target name, eg compile, generateSolution</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool RunGenericNantTarget(string BranchLocation, string NantTarget)
        {
            return LaunchExe("nant.bat", String.Format("{0}  -logfile:opda.txt", NantTarget), BranchLocation);
        }

        /// <summary>
        /// Specific call to run the generateWinform task
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the task is to be run</param>
        /// <param name="YAMLPath">The sub-path to the YAML file, eg MPartner\Gui\Setup\myForm.yaml</param>
        /// <param name="AndCompile">Boolean indicating if the compiler is to be invoked after generating the form</param>
        /// <param name="AndStartClient">Boolean indicating if the client should be started after compilation</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool RunGenerateWinform(string BranchLocation, string YAMLPath, bool AndCompile, bool AndStartClient)
        {
            string initialDir = System.IO.Path.Combine(BranchLocation, "csharp\\ICT\\Petra\\Client");

            if (AndCompile && AndStartClient)
            {
                return LaunchExe("nant.bat", String.Format("generateWinform  startPetraClient  -D:file={0}  -logfile:opda.txt", YAMLPath), initialDir);
            }
            else if (AndCompile)
            {
                return LaunchExe("nant.bat", String.Format("generateWinform  -D:file={0}  -logfile:opda.txt", YAMLPath), initialDir);
            }
            else
            {
                return LaunchExe("nant.bat", String.Format("generateWinform  -D:file={0}  -D:donotcompile=true  -logfile:opda.txt",
                        YAMLPath), initialDir);
            }
        }

        /// <summary>
        /// Specific call to run the previewWinform task
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the task is to be run</param>
        /// <param name="YAMLPath">The sub-path to the YAML file, eg MPartner\Gui\Setup\myForm.yaml</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool RunPreviewWinform(string BranchLocation, string YAMLPath)
        {
            string initialDir = System.IO.Path.Combine(BranchLocation, "csharp\\ICT\\Petra\\Client");

            // This is one of the tasks where we don't wait for exit - so we write a dummy output file
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(initialDir, "opda.txt")))
            {
                sw.WriteLine("This task does not generate a log file.");
                sw.Close();
            }

            return LaunchExe("nant.bat", String.Format("previewWinform  -D:file={0}", YAMLPath), initialDir, false);
        }

        //  Helper function to launch an executable file.
        //  Returns true if the executable is launched successfully.
        private static bool LaunchExe(string ExeName, string Params, string StartDirectory, bool WaitForExit = true)
        {
            bool ret = true;
            ProcessStartInfo si = new ProcessStartInfo(ExeName, Params);

            si.WorkingDirectory = StartDirectory;
            si.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                Process p = Process.Start(si);

                if (WaitForExit)
                {
                    p.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                OutputText.AppendText(OutputText.OutputStream.Both,
                    String.Format("\r\nError!!! An exception occurred when launching '{0}'.  The exception message was '{1}'.\r\n", ExeName,
                        ex.Message));
                ret = false;
            }
            return ret;
        }
    }
}