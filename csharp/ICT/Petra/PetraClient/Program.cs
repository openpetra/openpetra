//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Threading;
using System.Windows.Forms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.PetraClient;

namespace PetraClient
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThreadAttribute]
        private static void Main(string[] args)
        {
            TUnhandledThreadExceptionHandler UnhandledThreadExceptionHandler;

            // Set up Handlers for 'UnhandledException'
            // Note: BOTH handlers are needed for a WinForms Application!!!
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandling.UnhandledExceptionHandler);
            UnhandledThreadExceptionHandler = new TUnhandledThreadExceptionHandler();

            Application.ThreadException += new ThreadExceptionEventHandler(UnhandledThreadExceptionHandler.OnThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TPetraClientMain.StartUp();
        }
    }
}