//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.DB.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// todoComment
    /// </summary>
    public delegate void TApplicationShutdownCallback();

    internal static class TExceptionHandlingCommon
    {
        public static void ProcessEDBAccessLackingCoordinationExc(EDBAccessLackingCoordinationException AException)
        {
            string Reason = String.Empty;

            //Would normally use the code below but cannot due to circular referencing.
            //Form MainMenuForm = TFormsList.GFormsList.MainMenuForm;

            if (Application.OpenForms.Count != 0)              // in the Main Menu Form Test this will be false...
            {
                Form MainMenuForm = Application.OpenForms[0];  // This gets the first ever opened Form, which is the Main Menu

                // Ensure MessageBox is shown on the UI Thread!
                if (MainMenuForm.InvokeRequired)
                {
                    MainMenuForm.Invoke((MethodInvoker) delegate
                        {
                            TServerBusyHelperGui.ShowDBAccessLackingActionNotPossibleDialog(
                                AException, out Reason);
                        });
                }
                else
                {
                    TServerBusyHelperGui.ShowDBAccessLackingActionNotPossibleDialog(
                        AException, out Reason);
                }
            }
            else
            {
                TServerBusyHelperGui.ShowDBAccessLackingActionNotPossibleDialog(
                    AException, out Reason);
            }

            if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
            {
                TLogging.Log(String.Format(Catalog.GetString(
                            TLogging.LOG_PREFIX_INFO + "The OpenPetra Server was too busy to perform the requested action. (Reason: {0})"),
                        Reason));
                TLogging.Log(AException.StackTrace);
            }
        }
    }

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
        /// Handler for Exceptions of Type <see cref="ESecurityAccessDeniedException" />.
        /// </summary>
        public static Action <ESecurityAccessDeniedException, Type>ProcessSecurityAccessDeniedException;

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
            string FunctionalityNotImplementedMsg = AppCoreResourcestrings.StrFunctionalityNotAvailableYet;
            string Reason = String.Empty;
            Exception TheException;

            TheException = ((Exception)AEventArgs.ExceptionObject);

            // 'Unwrap' the Exception if it is contained inside a TargetInvocationException
            if ((TheException is TargetInvocationException)
                && (TheException.InnerException != null))
            {
                TheException = TheException.InnerException;
            }

            if (TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionClientSide(TheException))
            {
                TExceptionHelper.ShowExceptionCausedByUnavailableDBConnectionMessage(false);

                return;
            }

            if (TheException is NotImplementedException)
            {
                if (TheException.Message != String.Empty)
                {
                    FunctionalityNotImplementedMsg = TheException.Message;
                }

                TLogging.Log(FunctionalityNotImplementedMsg);
                TLogging.Log(TheException.StackTrace);

                MessageBox.Show(FunctionalityNotImplementedMsg, AppCoreResourcestrings.StrFunctionalityNotAvailableYetTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if ((TheException is EOPDBException)
                     && ((TheException.InnerException != null)
                         && (TheException.InnerException is EDBAccessLackingCoordinationException)))
            {
                TExceptionHandlingCommon.ProcessEDBAccessLackingCoordinationExc((EDBAccessLackingCoordinationException)TheException.InnerException);
            }
            else if (TheException is EDBAccessLackingCoordinationException)
            {
                TExceptionHandlingCommon.ProcessEDBAccessLackingCoordinationExc((EDBAccessLackingCoordinationException)TheException);
            }
            else if (TheException is ECachedDataTableLoadingRetryGotCancelledException)
            {
                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                {
                    TLogging.Log(Catalog.GetString(
                            TLogging.LOG_PREFIX_INFO +
                            "The OpenPetra Server was too busy to retrieve the data for a Cacheable DataTable and the user cancelled the loading after the retry attempts were exhausted."));
                    TLogging.Log(TheException.StackTrace);
                }

                TServerBusyHelperGui.ShowLoadingOfDataGotCancelledDialog();
            }
            else if (TheException is ESecurityAccessDeniedException)
            {
                if (ProcessSecurityAccessDeniedException != null)
                {
                    ProcessSecurityAccessDeniedException((ESecurityAccessDeniedException)TheException, ASender.GetType());
                }
                else
                {
                    MessageBox.Show(
                        "Unhandled Thread Exception Handler: encountered ESecurityAccessDeniedException, but Delegate " +
                        "'ProcessSecurityAccessDeniedException' isn't set up - which is a mistake that needs to be corrected." +
                        Environment.NewLine +
                        "Message of the ProcessSecurityAccessDeniedException instance:" + Environment.NewLine +
                        ProcessSecurityAccessDeniedException.ToString());
                }
            }
            else
            {
                //      MessageBox.Show("UnhandledExceptionHandler  Unhandled Exception: \r\n\r\n" +
                //                      TheException.ToString() + "\r\n\r\n"+
                //      "IsTerminating: " + AEventArgs.IsTerminating.ToString());

                LogException(TheException,
                    "Reported by UnhandledExceptionHandler: (Application is terminating: " + AEventArgs.IsTerminating.ToString() + ')');
                UEDialogue = new TUnhandledExceptionForm();

                UEDialogue.NonRecoverable = AEventArgs.IsTerminating;
                UEDialogue.TheException = TheException;

                //Would normally use the code below but cannot due to circular referencing.
                //Form MainMenuForm = TFormsList.GFormsList.MainMenuForm;

                if (Application.OpenForms.Count != 0)              // in the Main Menu Form Test this will be false...
                {
                    Form MainMenuForm = Application.OpenForms[0];  // This gets the first ever opened Form, which is the Main Menu

                    // Ensure UEDialogue is shown on the UI Thread!
                    if (MainMenuForm.InvokeRequired)
                    {
                        MainMenuForm.Invoke((MethodInvoker) delegate
                            {
                                UEDialogue.ShowDialog();
                            });
                    }
                    else
                    {
                        UEDialogue.ShowDialog();
                    }
                }
                else
                {
                    UEDialogue.ShowDialog();
                }
            }
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TUnhandledThreadExceptionHandler : object
    {
        /// <summary>
        /// Handler for Exceptions of Type <see cref="ESecurityAccessDeniedException" />.
        /// </summary>
        public static Action <ESecurityAccessDeniedException, Type>ProcessSecurityAccessDeniedException;

        #region TUnhandledThreadExceptionHandler

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ASender"></param>
        /// <param name="AEventArgs"></param>
        public void OnThreadException(object ASender, ThreadExceptionEventArgs AEventArgs)
        {
            TUnhandledExceptionForm UEDialogue;
            string FunctionalityNotImplementedMsg = AppCoreResourcestrings.StrFunctionalityNotAvailableYet;
            string Reason = String.Empty;
            Exception TheException = ((Exception)AEventArgs.Exception);

            // 'Unwrap' the Exception if it is contained inside a TargetInvocationException
            if ((TheException is TargetInvocationException)
                && (TheException.InnerException != null))
            {
                TheException = TheException.InnerException;
            }

            if (TExceptionHelper.IsExceptionCausedByUnavailableDBConnectionClientSide(TheException))
            {
                TExceptionHelper.ShowExceptionCausedByUnavailableDBConnectionMessage(false);

                return;
            }

            if (TheException is NotImplementedException)
            {
                if (TheException.Message != String.Empty)
                {
                    FunctionalityNotImplementedMsg = TheException.Message;
                }

                TLogging.Log(FunctionalityNotImplementedMsg);
                TLogging.Log(TheException.StackTrace);

                MessageBox.Show(FunctionalityNotImplementedMsg, AppCoreResourcestrings.StrFunctionalityNotAvailableYetTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if ((TheException is EOPDBException)
                     && ((TheException.InnerException != null)
                         && (TheException.InnerException is EDBAccessLackingCoordinationException)))
            {
                TExceptionHandlingCommon.ProcessEDBAccessLackingCoordinationExc((EDBAccessLackingCoordinationException)TheException.InnerException);
            }
            else if (TheException is EDBAccessLackingCoordinationException)
            {
                TExceptionHandlingCommon.ProcessEDBAccessLackingCoordinationExc((EDBAccessLackingCoordinationException)TheException);
            }
            else if (TheException is ECachedDataTableLoadingRetryGotCancelledException)
            {
                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_COORDINATED_DB_ACCESS)
                {
                    TLogging.Log(Catalog.GetString(
                            TLogging.LOG_PREFIX_INFO +
                            "The OpenPetra Server was too busy to retrieve the data for a Cacheable DataTable and the user cancelled the loading after the retry attempts were exhausted."));
                    TLogging.Log(TheException.StackTrace);
                }

                TServerBusyHelperGui.ShowLoadingOfDataGotCancelledDialog();
            }
            else if (TheException is ESecurityAccessDeniedException)
            {
                if (ProcessSecurityAccessDeniedException != null)
                {
                    ProcessSecurityAccessDeniedException((ESecurityAccessDeniedException)TheException, ASender.GetType());
                }
                else
                {
                    MessageBox.Show(
                        "Unhandled Thread Exception Handler: encountered ESecurityAccessDeniedException, but Delegate " +
                        "'ProcessSecurityAccessDeniedException' isn't set up - which is a mistake that needs to be corrected." +
                        Environment.NewLine +
                        "Message of the ProcessSecurityAccessDeniedException instance:" + Environment.NewLine +
                        ProcessSecurityAccessDeniedException.ToString());
                }
            }
            else
            {
                //                MessageBox.Show(
                //                    "TUnhandledThreadExceptionHandler.OnThreadException  Unhandled Exception: \r\n\r\n" + TheException.ToString());

                ExceptionHandling.LogException(TheException, "Reported by TUnhandledThreadExceptionHandler.OnThreadException");
                UEDialogue = new TUnhandledExceptionForm();

                UEDialogue.NonRecoverable = false;
                UEDialogue.TheException = TheException;

                //Would normally use the code below but cannot due to circular referencing.
                //Form MainMenuForm = TFormsList.GFormsList.MainMenuForm;

                if (Application.OpenForms.Count != 0)              // in the Main Menu Form Test this will be false...
                {
                    Form MainMenuForm = Application.OpenForms[0];  // This gets the first ever opened Form, which is the Main Menu

                    // Ensure UEDialogue is shown on the UI Thread!
                    if (MainMenuForm.InvokeRequired)
                    {
                        MainMenuForm.Invoke((MethodInvoker) delegate
                            {
                                UEDialogue.ShowDialog();
                            });
                    }
                    else
                    {
                        UEDialogue.ShowDialog();
                    }
                }
                else
                {
                    UEDialogue.ShowDialog();
                }
            }
        }

        #endregion
    }
}