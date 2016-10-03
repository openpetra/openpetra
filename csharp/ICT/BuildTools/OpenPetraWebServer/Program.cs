//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using Ict.Common;

namespace Ict.Tools.OpenPetraWebServer
{
    static class Program
    {
        enum ShowWindowConstants : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        };

        enum RunStyles
        {
            None,
            FullUI,
            SmallUI,
            HelpUI
        };

        private const string APPLICATION_TITLE = "Open Petra Web Server";
        public const string SHUTDOWN_MESSAGE = "Are you sure you want to stop the Open Petra Web Server application?";
        public const string BALLOON_MESSAGE = "The web server is still running in the system tray.";
        public const int HELP_PORT = 57912;
        public const int SERVER_MANAGER_BASE_PORT = 57915;

        private static string[] _commandArgs;
        private static string _Version = "";
        private static string _ApplicationTitleAndVersion = APPLICATION_TITLE;
        private static string _ConfigFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static RunStyles _RunStyle;

        public static string ApplicationTitle {
            get
            {
                return APPLICATION_TITLE;
            }
        }
        public static string ApplicationTitleAndVersion {
            get
            {
                return _ApplicationTitleAndVersion;
            }
        }
        public static string FileVersion {
            get
            {
                return _Version;
            }
        }
        public static string ConfigurationFilePath {
            get
            {
                return _ConfigFilePath;
            }
        }
        public static string CommandLineParams
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (string s in _commandArgs)
                {
                    sb.AppendFormat("{0}  ", s);
                }

                return sb.ToString().Trim();
            }
        }

        public static uint UM_ACTIVATE_APP = 0x2123;
        public static uint UM_CLOSE_APP = 0x2124;

        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static Form myForm = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            _commandArgs = args;

            new TAppSettingsManager(false);

            // Read the default working parameters using TAppSettings.  If a config file exists, the settings in the file will be used.
            CommandLineArgs commandLineArgs = new CommandLineArgs();

            // Read the actual command line for any further over-rides
            commandLineArgs.ParseCommandLine(args);
            CreateLog(commandLineArgs);

            _RunStyle = RunStyles.None;

            if (commandLineArgs.UseFullUI)
            {
                TLogging.Log("Using the full UI");

                // Find out if there is an existing instance
                Process curProcess = Process.GetCurrentProcess();
                Process[] RunningProcesses = System.Diagnostics.Process.GetProcessesByName(curProcess.ProcessName);

                if (RunningProcesses.Length == 1)
                {
                    // This is the only running process of our name
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
                    _Version = fvi.FileVersion;
                    _ApplicationTitleAndVersion += ("(" + _Version + ")");

                    _ConfigFilePath += @"\OM_International\OpenPetraWebServer.cfg.xml";

                    _RunStyle = RunStyles.FullUI;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    myForm = new MainForm();
                    Application.Run(myForm);
                }
                else
                {
                    // There must be another window in the tray
                    IntPtr hWnd = FindWindow(null, APPLICATION_TITLE);

                    if (hWnd != null)
                    {
                        // We need to activate the other app
                        SendMessage(hWnd, UM_ACTIVATE_APP, IntPtr.Zero, IntPtr.Zero);
                    }

                    TLogging.Log("There is already another process with application title " + APPLICATION_TITLE);
                }
            }
            else
            {
                if (commandLineArgs.IsValid)
                {
                    // Command line is valid so we will use the Small UI
                    // Check if the port is available
                    if (PortIsInUse(commandLineArgs.Port, true))
                    {
                        string msg =
                            "The port you have specified is in use.  Check the System Tray to see if an instance of the web server is already running.";

                        // Show a message box unless -quiet:true
                        if (!commandLineArgs.SuppressStartupMessages)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                msg,
                                APPLICATION_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                        }

                        // Now we just quit without starting anything
                        TLogging.Log(msg);
                    }
                    else
                    {
                        if (commandLineArgs.MaxRuntimeInMinutes > 0)
                        {
                            // start a thread that will stop this server after the given time
                            ThreadStart stopThread = delegate {
                                StopApplication(commandLineArgs.MaxRuntimeInMinutes);
                            };

                            Thread thread = new Thread(stopThread);

                            // The next line is vital!!  It ensures that when the application stops 'normally' the stopThread will terminate.
                            // Otherwise the program will be left in memory
                            thread.IsBackground = true;

                            thread.Start();
                        }

                        // make sure that the bin directory is on the PATH, so that we can find libsodium.dll
                        string path = Environment.GetEnvironmentVariable("PATH");
                        string binDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                        Environment.SetEnvironmentVariable("PATH", path + ";" + binDir);

                        _RunStyle = RunStyles.SmallUI;
                        TLogging.Log("Ict.Tools.OpenPetraWebServer is now running on port " + commandLineArgs.Port.ToString() + "...");
                        TLogging.Log("    Command line was: " + Environment.CommandLine);
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        myForm = new SmallUIForm(commandLineArgs.PhysicalPath,
                            commandLineArgs.VirtualPath,
                            commandLineArgs.Port,
                            commandLineArgs.DefaultPage,
                            commandLineArgs.AcceptRemoteConnection,
                            commandLineArgs.LogPageRequests);
                        Application.Run(myForm);
                    }
                }
                else
                {
                    // Command line is invalid so we will use the Command Line Help form
                    TLogging.Log("The command line arguments don't make sense! " + Environment.CommandLine, TLoggingType.ToConsole);
                    _RunStyle = RunStyles.HelpUI;
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new CommandLineHelp());
                }
            }

            TLogging.Log("Ict.Tools.OpenPetraWebServer has finished");
        }

        static private void CreateLog(CommandLineArgs args)
        {
            if (args.LogfilePath == String.Empty)
            {
                // Create a console-only log
                new TLogging();
            }
            else
            {
                // Create a log file and a console log
                string pathToLogFile;

                if (args.UseFullUI)
                {
                    pathToLogFile = Path.Combine(args.LogfilePath, "Ict.Tools.OpenPetraWebServer.log");
                }
                else
                {
                    pathToLogFile = Path.Combine(args.LogfilePath, "Ict.Tools.OpenPetraWebServer." + args.Port.ToString() + ".log");
                }

                new TLogging(pathToLogFile);
            }

            // Direct the console logging to our server log form
            TLogging.SendConsoleOutputToString(OnNewLoggingMessage);
        }

        static private void StopApplication(Int16 AStopAfterMinutes)
        {
            Thread.Sleep(TimeSpan.FromMinutes(AStopAfterMinutes));
            TLogging.Log("quitting the Webserver due to time out, after " + AStopAfterMinutes.ToString() + " minutes");

            if (myForm != null)
            {
                myForm.Close();
            }

            Environment.Exit(1);
        }

        /// <summary>
        /// Returns true if the specified port is in use;
        /// only checking IPv4 loopback address for now
        /// </summary>
        static public bool PortIsInUse(int PortNumber, bool DisallowHelpPort)
        {
            if ((PortNumber == HELP_PORT) && DisallowHelpPort)
            {
                return true;
            }

            bool inUse = false;

            try
            {
                //Create a socket on the current IPv4 address
                Socket TestSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create an IP end point
                IPEndPoint localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PortNumber);

                // Bind that port
                TestSocket.Bind(localIP);

                // Cleanup
                TestSocket.Close();
            }
            catch (SocketException)
            {
                inUse = true;
            }

            return inUse;
        }

        public static WebSite CreateHelpWebSite()
        {
            WebSite webSite = new WebSite();
            string physicalPath = null;

            string myFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // At runtime the help will be in the folder beneath the runtime
            string tryFolder = Path.Combine(myFolder, "ServerHelp");

            if (Directory.Exists(tryFolder))
            {
                physicalPath = tryFolder;
            }
            else
            {
                // when we run in the debug environment we will be in the bin folder
                // so we need to go up one level (even possibly 2) before going down
                for (int upLevels = 1; upLevels <= 2; upLevels++)
                {
                    tryFolder = myFolder;
                    int pos = tryFolder.LastIndexOf('\\');

                    for (int i = 0; i < 1 && pos >= 0; i++)
                    {
                        tryFolder = tryFolder.Substring(0, pos);
                        pos = tryFolder.LastIndexOf('\\');
                    }

                    tryFolder = Path.Combine(tryFolder, "ServerHelp");

                    if (Directory.Exists(tryFolder))
                    {
                        physicalPath = tryFolder;
                        break;
                    }
                }
            }

            if (physicalPath != null)
            {
                webSite = new WebSite();
                webSite.PhysicalPath = physicalPath;
                webSite.VirtualPath = "/";
                webSite.Port = Program.HELP_PORT;
                webSite.DefaultPage = "home.htm";
            }

            return webSite;
        }

        public static void OnNewLoggingMessage()
        {
            if (myForm != null)
            {
                switch (_RunStyle)
                {
                    case RunStyles.SmallUI:
                        ((SmallUIForm)myForm).OnNewLogMessage();
                        break;

                    case RunStyles.FullUI:
                        ((MainForm)myForm).OnNewLogMessage();
                        break;
                }
            }
        }
    }
}
