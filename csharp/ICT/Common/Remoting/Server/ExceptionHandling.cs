//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Threading;

using Ict.Common;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Contains procedures for structured Exception handling. They are
    /// intended to be used as 'last resort' in case an Exception that was thrown
    /// anywhere in the Application wasn't caught anywhere.
    /// </summary>
    public class ExceptionHandling
    {
        private const String FALLBACK_LOGFILE_NAME = "PetraServer.log";

        /// <summary>
        /// Logs an Exception and a StackTrace to the Server logfile.
        /// </summary>
        /// <param name="AException">The Exception to log.</param>
        /// <param name="ALogText">Additional Text to be logged.</param>
        /// <returns>void</returns>
        public static void LogException(Exception AException, String ALogText)
        {
            String LogentryText;

            LogentryText = "PetraServer Caught an Unhandled Exception.\r\n" + ALogText + "\r\n" + AException.ToString();

            // Check if logging is already initialised; if not, initialise it with a fallback log file
            if (TLogging.GetLogFileName() == "")
            {
                new TLogging(Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                    Path.DirectorySeparatorChar + FALLBACK_LOGFILE_NAME);
            }

            TLogging.Log(LogentryText);
            TLogging.LogStackTrace(TLoggingType.ToLogfile);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// <para>
        /// No finalizers are executed by the CLR when an unhandled exception occurs, even when
        /// a Handler like this is hooked up to the AppDomain.CurrentDomain.UnhandledException
        /// Event! Also, all threads are silently killed without a chance to execute their
        /// catch/finally blocks to do an orderly shutdown.
        /// To force an 'ordered cooperative shutdown' that overcomes those limitations, we need
        /// to do this from another Thread (MS didn't do their job properly here...).
        /// </para>
        /// <para>
        /// See http://geekswithblogs.net/akraus1/archive/2006/10/30/95435.aspx
        /// for an explanation of how (and why) this overcomes those limitations!
        /// </para>
        /// </remarks>
        /// <param name="ASender">The source of the unhandled exception event.</param>
        /// <param name="AEventArgs">An UnhandledExceptionEventArgs that contains the event data.</param>
        public static void UnhandledExceptionHandler(object ASender, UnhandledExceptionEventArgs AEventArgs)
        {
            LogException((Exception)AEventArgs.ExceptionObject,
                "The PetraServer process will need to be stopped because of the following Unhandled Exception:");

            // Prepare 'cooperative async shutdown' from another thread
            Thread HelperThread = new Thread(delegate()
                {
                    Console.WriteLine("Asynchronous shutdown started");

                    Environment.Exit(1);
                });

            HelperThread.Start();
            HelperThread.Join(); // wait until we have exited
        }
    }
}