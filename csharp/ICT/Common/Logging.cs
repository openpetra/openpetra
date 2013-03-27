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
using System.Collections;
using System.IO;
using System.Diagnostics;


namespace Ict.Common
{
    /// <summary>
    /// Logging Type describes the destination of the logging messages
    /// </summary>
    public enum TLoggingType
    {
        /// <summary>
        /// to the console so that you can follow while program is running
        /// </summary>
        ToConsole = 1,

        /// <summary>
        /// to log file for later analysis
        /// </summary>
        ToLogfile = 2,

        /// <summary>
        /// Statusbar is the alternative for Forms where the user does not have a console
        /// </summary>
        ToStatusBar = 4
    };

    /// <summary>
    /// The TLogging class provides general logging functionality.
    /// Logging output can currently go to the Console, to a file or to both at the same time.
    /// </summary>
    public class TLogging
    {
        /// <summary>
        /// the debuglevel that is required for stacktrace to be printed;
        /// this is related to the mono bug described in the code
        /// </summary>
        public const int DEBUGLEVEL_TRACE = 10;

        /// <summary>
        /// the debuglevel that is required for saving some detailed log files for the reporting
        /// </summary>
        public const int DEBUGLEVEL_REPORTING = 5;

        /// <summary>
        /// some log messages will be only displayed at a certain DebugLevel
        /// </summary>
        public static int DebugLevel = 0;

        /// <summary>DL is a abbreviated synonym for DebugLevel (more convenient)</summary>
        public static int DL
        {
            get
            {
                return DebugLevel;
            }
        }


        /// <summary>
        /// this is the default prefix for the username
        /// </summary>
        public const string DEFAULTUSERNAMEPREFIX = "MiB";

        /// <summary>
        /// this is used for statusbar updates
        /// </summary>
        public delegate void TStatusCallbackProcedure(string msg);

        private static TLogWriter ULogWriter = null;
        private static String ULogFileName;
        private static String UUserNamePrefix = DEFAULTUSERNAMEPREFIX;

        /// <summary>
        /// This can provide information about the context of the program situation when a log message is displayed.
        /// Use SetContext for setting and resetting the context information.
        ///
        /// </summary>
        private static String Context;

        /// <summary>
        /// This is a procedure that is called with the text as a parameter. It can be used to update a status bar.
        /// </summary>
        private static TStatusCallbackProcedure StatusBarProcedure;

        /// <summary>
        /// This is variable indicates if StatusBarProcedure is set to a valid value.
        /// </summary>
        private static bool StatusBarProcedureValid;

        /// <summary>
        /// property for the prefix that describes the
        /// </summary>
        public static string UserNamePrefix
        {
            get
            {
                return UUserNamePrefix;
            }

            set
            {
                UUserNamePrefix = value;
                ULogWriter.LogtextPrefix = UUserNamePrefix;
            }
        }


        #region TLogging

        /// <summary>
        /// Creates a Console-only logger.
        /// </summary>
        public TLogging()
        {
            TLogging.Context = "";
            TLogging.StatusBarProcedure = null;
            StatusBarProcedureValid = false;
        }

        /// <summary>
        /// Creates a logger that can log both to Console or file.
        /// </summary>
        /// <param name="AFileName">File to which the output should be written if logging to
        /// the logfile is requested.</param>
        public TLogging(String AFileName)
        {
            if (Path.GetFullPath(AFileName) == TLogWriter.GetLogFileName())
            {
                return;
            }

            TLogging.Context = "";

            if (ULogWriter == null)
            {
                ULogWriter = new TLogWriter(AFileName);
                ULogFileName = AFileName;
            }
            else
            {
                throw new Exception("TLogging.Create: only use one log file at the time! old name: " +
                    TLogWriter.GetLogFileName() + "; new name: " +
                    Path.GetFullPath(AFileName));
            }

            TLogging.StatusBarProcedure = null;
            StatusBarProcedureValid = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ALogFileMsg"></param>
        /// <returns></returns>
        public bool CanWriteLogFile(out String ALogFileMsg)
        {
            return ULogWriter.CanWriteLogFile(out ALogFileMsg);
        }

        /// <summary>
        /// returns the name of the current log file
        /// </summary>
        /// <returns>the path of the current log file</returns>
        public static String GetLogFileName()
        {
            return System.IO.Path.GetFullPath(ULogFileName);
        }

        /// <summary>
        /// Set the context of the program situation. It is displayed in the next log messages.
        /// </summary>
        /// <param name="context">This will be displayed in the following calls to Log; can be reset with an empty string
        /// </param>
        /// <returns>void</returns>
        public static void SetContext(String context)
        {
            TLogging.Context = context;
        }

        /// <summary>
        /// This sets the procedure that is called with the text as an parameter. It can be used to update a status bar.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void SetStatusBarProcedure(TStatusCallbackProcedure callbackfn)
        {
            TLogging.StatusBarProcedure = callbackfn;
            StatusBarProcedureValid = true;
        }

        /// <summary>
        /// Logs a message. Output goes to both Screen and Logfile.
        ///
        /// </summary>
        /// <param name="Text">Log message</param>
        /// <returns>void</returns>
        public static void Log(string Text)
        {
            if (ULogWriter != null)
            {
                Log(Text, TLoggingType.ToLogfile | TLoggingType.ToConsole);
            }
            else
            {
                Log(Text, TLoggingType.ToConsole);
            }
        }

        /// <summary>
        /// Log if level is this high
        /// </summary>
        /// <param name="Level"></param>
        /// <param name="Text"></param>
        public static void LogAtLevel(Int32 Level, string Text)
        {
            if (TLogging.DebugLevel >= Level)
            {
                TLogging.Log(Text);
            }
        }

        /// <summary>
        /// Log if level is this high. Output destination can be selected with the Loggingtype flag.
        /// </summary>
        /// <param name="ALevel"></param>
        /// <param name="AText"></param>
        /// <param name="ALoggingType"></param>
        public static void LogAtLevel(Int32 ALevel, string AText, TLoggingType ALoggingType)
        {
            if (TLogging.DebugLevel >= ALevel)
            {
                TLogging.Log(AText, ALoggingType);
            }
        }

        /// <summary>
        /// Logs a message. Output destination can be selected with the Loggingtype flag.
        /// </summary>
        /// <param name="Text">Log message</param>
        /// <param name="ALoggingType">Determines the output destination.
        /// Note: More than one output destination can be chosen!</param>
        public static void Log(string Text, TLoggingType ALoggingType)
        {
            if (((ALoggingType & TLoggingType.ToConsole) != 0)
                || ((ALoggingType & TLoggingType.ToLogfile) != 0)
                // only in Debugmode write the messages for the statusbar also on the console (e.g. reporting progress)
                || (((ALoggingType & TLoggingType.ToStatusBar) != 0) && (TLogging.DebugLevel == TLogging.DEBUGLEVEL_TRACE)))
            {
                Console.Error.WriteLine(Utilities.CurrentTime() + "  " + Text);

                if ((TLogging.Context != null) && (TLogging.Context.Length != 0))
                {
                    Console.Error.WriteLine("  Context: " + TLogging.Context);
                }
            }

            if (((ALoggingType & TLoggingType.ToConsole) != 0) || ((ALoggingType & TLoggingType.ToLogfile) != 0)
                || ((ALoggingType & TLoggingType.ToStatusBar) != 0))
            {
                if (TLogging.StatusBarProcedureValid && (Text.IndexOf("SELECT") == -1))
                {
                    // don't print sql statements to the statusbar in debug mode

                    if (TLogging.Context.Length != 0)
                    {
                        Text += "; Context: " + TLogging.Context;
                    }

                    StatusBarProcedure(Text);
                }
            }

            if ((ALoggingType & TLoggingType.ToLogfile) != 0)
            {
                if (ULogWriter != null)
                {
                    TLogWriter.Log(Text);

                    if (TLogging.Context.Length != 0)
                    {
                        TLogWriter.Log("  Context: " + TLogging.Context);
                    }
                }
                else
                {
                    // I found it was better to write the actual logging message,
                    // even if the logwriter is not setup up correctly
                    new TLogging("temp.log");
                    TLogWriter.Log(Text);

                    if (TLogging.Context.Length != 0)
                    {
                        TLogWriter.Log("  Context: " + TLogging.Context);
                    }

                    ULogWriter = null;
                    ULogFileName = null;

                    // now throw an exception, because it is not supposed to work like this
                    throw new TNoLoggingToFile_WrongConstructorUsedException();
                }
            }
        }

        /// <summary>
        /// log the current stack trace; on Mono, that does not fully work
        /// </summary>
        /// <param name="ALoggingtype">destination of logging</param>
        public static void LogStackTrace(TLoggingType ALoggingtype)
        {
            if (Utilities.DetermineExecutingCLR() == TExecutingCLREnum.eclrMono)
            {
                // not printing the stacktrace since that could cause an exception
                return;
            }

            StackTrace st;
            StackFrame sf;
            Int32 Counter;
            String msg;

            st = new StackTrace(true);
            msg = "";

            for (Counter = 0; Counter <= st.FrameCount - 1; Counter += 1)
            {
                sf = st.GetFrame(Counter);

                if ((sf.GetMethod().Name == "WndProc") && (sf.GetFileLineNumber() == 0))
                {
                    break;
                }

                msg = msg + "    at " + sf.GetMethod().ToString();

                if (sf.GetFileLineNumber() != 0)
                {
                    msg = msg + " (" + sf.GetFileName() + ": " + sf.GetFileLineNumber().ToString() + ')';
                }

                msg = msg + Environment.NewLine;
            }

            msg = msg + "in Appdomain " + AppDomain.CurrentDomain.FriendlyName + Environment.NewLine;
            TLogging.Log(msg, ALoggingtype);
        }

        /// <summary>
        /// Logs a number of messages in one go. Output goes to both Screen and Logfile.
        /// </summary>
        /// <param name="aList">An ArrayList containing a number of Log messages</param>
        /// <param name="isException">If set to TRUE, an information which states that all
        /// following Log messages are Exceptions is written before
        /// the Log messages are logged.</param>
        /// <returns>void</returns>
        public static void Log(ArrayList aList, bool isException)
        {
            Log(aList, isException, TLoggingType.ToConsole | TLoggingType.ToLogfile);
        }

        /// <summary>
        /// Logs a number of messages in one go. Output destination can be selected
        /// with the Loggingtype flag.
        /// </summary>
        /// <param name="aList">An ArrayList containing a number of Log messages</param>
        /// <param name="isException">If set to TRUE, an information which states that all
        /// following Log messages are Exceptions is written before
        /// the Log messages are logged.</param>
        /// <param name="Loggingtype">logging destination (eg. console, logfile etc)</param>
        /// <returns>void</returns>
        public static void Log(ArrayList aList, bool isException, TLoggingType Loggingtype)
        {
            string additionalInfo;

            additionalInfo = "";

            if (isException)
            {
                additionalInfo = "The application has encountered an Exception: The details are as follows:" + Environment.NewLine;
            }

            Log(Environment.NewLine +
                DateTime.Now.ToLongDateString() + ", " +
                DateTime.Now.ToLongTimeString() + " : " + additionalInfo,
                Loggingtype);

            for (int Counter = 0; Counter <= (aList.Count - 1); Counter++)
            {
                Log(aList[Counter].ToString(), Loggingtype);
            }
        }
    }
    #endregion

    /// <summary>
    /// Thrown when using the wrong constructor
    /// </summary>
    public class TNoLoggingToFile_WrongConstructorUsedException : ApplicationException
    {
        /// <summary>
        /// This Exception is thrown if the TLogging class was created using the Create()
        /// constructor (without the FileName parameter) and a logging request is made
        /// that would write to a Logfile.
        /// </summary>
        public TNoLoggingToFile_WrongConstructorUsedException()
        {
        }
    }
}