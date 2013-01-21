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
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ict.Tools.DevelopersAssistant
{
    static class Program
    {
        /// <summary>
        /// The title of the application main window
        /// </summary>
        public const string APP_TITLE = "OpenPetra Developer's Assistant";
        /// <summary>
        /// The user-message id that we pass to start the server
        /// </summary>
        public static uint UM_START_SERVER = 0x2100;
        /// <summary>
        /// The user-message id that we pass to stop the server
        /// </summary>
        public static uint UM_STOP_SERVER = 0x2101;
        /// <summary>
        /// The static command line arguments object
        /// </summary>
        public static CommandArgs cmdLine = null;

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [return : MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            cmdLine = new CommandArgs(args);

            IntPtr hWnd = FindWindow(null, APP_TITLE);

            if (hWnd == IntPtr.Zero)
            {
                // This is the only running process of our name

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                // There must be another window already open
                // We need to send an appropriatemessage to the other app and just quit our new instance
                if (cmdLine.StopServer)
                {
                    PostMessage(hWnd, UM_STOP_SERVER, IntPtr.Zero, IntPtr.Zero);
                }
                else if (cmdLine.StartServer)
                {
                    PostMessage(hWnd, UM_START_SERVER, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }
    }
}