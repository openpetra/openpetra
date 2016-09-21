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
using System.IO;
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
            DateTime? dtServerStart = null;

            if (bIsRunning && (_serverProcessID == 0))
            {
                // Theoretically there could be more than one server console running.  If there are we will just work with the first one
                //  and then work out which cmd window is associated with it
                for (int i = 0; i < allProcesses.Length; i++)
                {
                    try
                    {
                        dtServerStart = allProcesses[0].StartTime;
                    }
                    catch (Exception)
                    {
                        // We did not have access permission for this process
                    }

                    if (dtServerStart.HasValue)
                    {
                        break;
                    }
                }

                if (dtServerStart.HasValue)
                {
                    // Now go round all cmd.exe processes to find the one that started just before
                    // We assume that it will be within 5 seconds
                    Process[] cmdProcesses = Process.GetProcessesByName("cmd");

                    if (cmdProcesses.Length > 0)
                    {
                        TimeSpan tsSmallest = TimeSpan.FromDays(36500);      // assume the smallest time span so far is 100 years!

                        for (int i = 0; i < cmdProcesses.Length; i++)
                        {
                            try
                            {
                                // If this command window is an administrator's one, we will get access denied when we ask for the start time
                                // But that's ok - it won't be one we are interested in anyway!  So we can just try the next one.
                                TimeSpan ts = dtServerStart.Value - cmdProcesses[i].StartTime;

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
                            catch (Exception)
                            {
                                // We did not have access permission for this process
                            }
                        }
                    }
                    else
                    {
                        _serverProcessID = allProcesses[0].Id;
                        _serverProcessIdIsCmdWindow = false;
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
            if ((NantTarget == "test-main-navigation-screens-core") && !IsServerRunning())
            {
                // Final chance to start the server for any task that requires it
                StartServer(BranchLocation, true);
            }

            PlatformID platform = Environment.OSVersion.Platform;

            // Unix does not support the redirect character, so we use the logfile command line - but this gives less output
            // Also (and this is interesting!), if we use the redirect character when the task is to start the client we get a problem in that
            //  while the login screen is displayed nant has 'finished' so we appear to return from launching the exe,
            //  but part of nant is still running - the part that is outputting to opda.txt.  So if you then call nant again,
            //  for example to start the server while the login screen is displayed, it doesn't work (nant cannot have two concurrent tasks).
            // Also, for the same reason, it is not possible to start two clients using the redirect character.
            // HOWEVER, it is often important in other scenarios to use the redirect because it gives ALL the output and sometimes we need that
            //  so that we can parse the output and look for key strings.  So we need to pick and choose carefully...
            if ((platform == PlatformID.MacOSX) || (platform == PlatformID.Unix) || (NantTarget == NantTask.TaskItem.startPetraClient.ToString()))
            {
                return LaunchExe("nant.bat", String.Format("{0}  -logfile:opda.txt", NantTarget), BranchLocation);
            }
            else
            {
                return LaunchExe("nant.bat", String.Format("{0} > opda.txt", NantTarget), BranchLocation);
            }
        }

        /// <summary>
        /// Specific call to run the generateWinform task
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the task is to be run</param>
        /// <param name="YAMLPath">The sub-path to the YAML file, eg MPartner\Gui\Setup\myForm.yaml</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool RunGenerateWinform(string BranchLocation, string YAMLPath)
        {
            // Prior to v1.0.3 we used one of three options to deal with generating a Win Form - using a different nant call to handle the check box options on the GUI
            // From v1.0.3 this method just runs nant to generatWinform with no compile
            //  The check box options are dealt with by creating different 'sequences' because that way we can 'abort' after an error.
            // This call was used to compile AND startClient
            //     return LaunchExe("nant.bat", String.Format("generateWinform  startPetraClient  -D:file={0}  -logfile:opda.txt", YAMLPath), initialDir);
            // This call was used to compile
            //     return LaunchExe("nant.bat", String.Format("generateWinform  -D:file={0}  -logfile:opda.txt", YAMLPath), initialDir);

            string initialDir = System.IO.Path.Combine(BranchLocation, "csharp\\ICT\\Petra\\Client");

            return LaunchExe("nant.bat", String.Format("generateWinformNoCompile  -D:file={0}  -logfile:opda.txt",
                    YAMLPath), initialDir);
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

        /// <summary>
        /// Specific call to run the previewWinform task
        /// </summary>
        /// <param name="BranchLocation">The path to the openPetra branch for which the task is to be run</param>
        /// <param name="CommandArgs">The admin console command args (if any)</param>
        /// <returns>True if nant.bat was launched successfully.  Check the log file to see if the command actually succeeded.</returns>
        public static bool RunServerAdminConsole(string BranchLocation, string CommandArgs)
        {
            string initialDir = System.IO.Path.Combine(BranchLocation, "delivery", "bin");

            // We have to ensure there is a config file
            string cfgFile = initialDir + "\\PetraServerAdminConsole.exe.config";

            if (!System.IO.File.Exists(cfgFile))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(cfgFile))
                {
                    sw.WriteLine("<?xml version=\"1.0\"?>");
                    sw.WriteLine("<configuration>");
                    sw.WriteLine(" <appSettings>");
                    sw.WriteLine("  <add key=\"Server.Port\" value=\"9000\" />");
                    sw.WriteLine(" </appSettings>");
                    sw.WriteLine("</configuration>");
                    sw.Close();
                }
            }

            // Start the server
            if (!IsServerRunning())
            {
                StartServer(BranchLocation, true);
            }

            // This is one of the tasks where we don't wait for exit - so we write a dummy output file
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.Combine(BranchLocation, "opda.txt"), true))
            {
                sw.WriteLine("The task to launch the server Admin Console does not generate a log file.");
                sw.Close();
            }

            bool bWaitForExit = (CommandArgs == null);
            bool bHideWindow = (CommandArgs != null);
            return LaunchExe("PetraServerAdminConsole.exe", CommandArgs, initialDir, bWaitForExit, bHideWindow, false);
        }

        //  Helper function to launch an executable file.
        //  Returns true if the executable is launched successfully.
        private static bool LaunchExe(string ExeName,
            string Params,
            string StartDirectory,
            bool WaitForExit = true,
            bool HideWindow = true,
            bool UseShellExecute = true)
        {
            bool ret = true;
            ProcessStartInfo si = new ProcessStartInfo(ExeName, Params);

            si.WorkingDirectory = StartDirectory;
            si.WindowStyle = (HideWindow) ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal;
            si.UseShellExecute = UseShellExecute;

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