//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.IO;

using Ict.Common;
using GNU.Gettext;
using System.Windows.Forms;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TApplicationShutdownCallback();

    /// <summary>
    /// contains procedures for structured Exception handling. They are
    /// intended to be used as 'last resort' in case an Exception that was thrown
    /// anywhere in the Application wasn't caught anywhere.
    /// </summary>
    public class ExceptionHandling
    {
        /// <summary>This is set by method "PetraClientMain.StartUp" in the PetraClient.exe Assembly.</summary>
        public static TApplicationShutdownCallback GApplicationShutdownCallback;

        /// <summary>
        /// log file name (should really be set in the config file)
        /// </summary>
        public const String FALLBACK_LOGFILE_NAME = "PetraClient.log";

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AException"></param>
        /// <param name="ALogText"></param>
        public static void LogException(Exception AException, String ALogText)
        {
            String LogentryText;

            LogentryText = "OpenPetra Client Caught an Unhandled Exception.\r\n" + ALogText + "\r\n" + AException.ToString();

            // Check if logging is already initialised; if not, initialise it with a fallback log file
            if (TLogging.GetLogFileName() == "")
            {
                new TLogging(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + FALLBACK_LOGFILE_NAME);
            }

            TLogging.Log(LogentryText);
            TLogging.LogStackTrace(TLoggingType.ToLogfile);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        public static void UnhandledExceptionHandler(object ASender, UnhandledExceptionEventArgs AEventArgs)
        {
            TUnhandledExceptionForm UEDialogue;

            //      MessageBox.Show("UnhandledExceptionHandler  Unhandled Exception: \r\n\r\n" +
            //                      ((Exception)(AEventArgs.ExceptionObject)).ToString() + "\r\n\r\n"+
            //      "IsTerminating: " + AEventArgs.IsTerminating.ToString());

            LogException((Exception)AEventArgs.ExceptionObject,
                "Reported by UnhandledExceptionHandler: (Application is terminating: " + AEventArgs.IsTerminating.ToString() + ')');
            UEDialogue = new TUnhandledExceptionForm();

            UEDialogue.NonRecoverable = AEventArgs.IsTerminating;
            UEDialogue.TheException = (Exception)AEventArgs.ExceptionObject;
            UEDialogue.ShowDialog();
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TUnhandledThreadExceptionHandler : object
    {
        #region TUnhandledThreadExceptionHandler

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        public void OnThreadException(object ASender, ThreadExceptionEventArgs AEventArgs)
        {
            TUnhandledExceptionForm UEDialogue;
            string FunctionalityNotImplementedMsg = Catalog.GetString("This functionality is not yet implemented in OpenPetra.");

            if (!(AEventArgs.Exception is NotImplementedException))
            {
//                MessageBox.Show(
//                    "TUnhandledThreadExceptionHandler.OnThreadException  Unhandled Exception: \r\n\r\n" + AEventArgs.Exception.ToString());

                ExceptionHandling.LogException(AEventArgs.Exception, "Reported by TUnhandledThreadExceptionHandler.OnThreadException");
                UEDialogue = new TUnhandledExceptionForm();

                UEDialogue.NonRecoverable = false;
                UEDialogue.TheException = AEventArgs.Exception;
                UEDialogue.ShowDialog();
            }
            else
            {
                if(AEventArgs.Exception.Message != String.Empty)
                {
                    FunctionalityNotImplementedMsg = AEventArgs.Exception.Message;
                }
                TLogging.Log(FunctionalityNotImplementedMsg);
                TLogging.Log(AEventArgs.Exception.StackTrace);
                
                MessageBox.Show(Catalog.GetString(FunctionalityNotImplementedMsg),
                    Catalog.GetString("Not Yet Implemented in OpenPetra"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
    }
}