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
using System.Collections;
using System.IO;

namespace Ict.Common
{
    /// <summary>
    /// The TLogWriter class writes arbitrary strings to a specified log file,
    /// prefixing them with date and time and optionally also with a prefix.
    /// Part of the logging framework for ICT Applications.
    /// </summary>
    public class TLogWriter
    {
        private static string ULogFileName = "";
        private static string ULogtextPrefix = "";
        private static bool USuppressDateAndTime = false;
        private String FLogFileErrorMsg;
        private bool FCanWriteLogFile;

        /// <summary>
        /// this text is always printed in front of each line in the logging
        /// can include the current time and user etc
        /// </summary>
        public string LogtextPrefix
        {
            get
            {
                if (ULogtextPrefix == "")
                {
                    return ULogtextPrefix;
                }
                else
                {
                    return ULogtextPrefix.Substring(2, ULogtextPrefix.Length - 2);
                }
            }

            set
            {
                if (value != "")
                {
                    ULogtextPrefix = " [" + value + ']';
                }
                else
                {
                    ULogtextPrefix = "";
                }
            }
        }

        /// <summary>
        /// Set to true to suppress the logging of date and time in log files (default= false).
        /// </summary>
        public bool SuppressDateAndTime
        {
            get
            {
                return USuppressDateAndTime;
            }

            set
            {
                USuppressDateAndTime = value;
            }
        }

        #region TLogWriter

        /// <summary>
        /// constructor that tells where to write the logfile
        /// </summary>
        /// <param name="LogfileName">where to write the logfile</param>
        public TLogWriter(string LogfileName)
        {
            if (LogfileName.Length > 0)
            {
                LogfileName = Path.GetFullPath(LogfileName);
            }
				
            ULogFileName = LogfileName;

            // Test whether I can write to this file.
            FileStream temp = null;
            try
            {
                FCanWriteLogFile = true;
                FLogFileErrorMsg = "Log file is " + ULogFileName;

				// Test wether there was an write access today, if not rotate filenames
				if(NeedToRotateFiles(ULogFileName)){
					RotateFiles(ULogFileName,LogfileName);
				}

                temp = new FileStream(ULogFileName, FileMode.OpenOrCreate, FileAccess.Write);
            }
            catch (Exception e)
            {
                FLogFileErrorMsg = "Error opening log file " + ULogFileName + ": " + e.Message;
                FCanWriteLogFile = false;
            }
            finally
            {
                if (temp != null)
                {
                    temp.Close();
                }
            }
        }

		/// <summary>
		/// Rotates the logfiles names in the following way.
		/// Example: the current log file for today is PetraClient.log, 
		/// the logfile from yesterday PetraClient-01.log, 
		/// the day before yesterday PetraClient-02.log, and files older than 6 days are deleted.
		/// 
		/// When it comes to rotate the logfiles, the number of each logfile is increased
		/// </summary>
		/// <param name="ULogFileName">Full Path including filename</param>
		/// <param name="LogFileName"> The name of the Logfile</param> 
		private void RotateFiles(string ULogFileName,string LogFileName){

			string LogfilePath = Path.GetDirectoryName (ULogFileName);
			string Extention = Path.GetExtension(LogFileName);
			string LogFileNameWithoutExtention = Path.GetFileNameWithoutExtension (LogFileName);

			for (int i = 6; i > 0; i--) {
				string NameToRotate = LogFileNameWithoutExtention + "-0" + i + Extention;
				string UOldFile = Path.Combine (LogfilePath, NameToRotate);

				if(File.Exists (UOldFile)){
					if (6 == i) {
						File.Delete (UOldFile);
					} 

					else {
						string NewName = LogFileNameWithoutExtention + "-0" + (i+1) + Extention;
						string UNewFile = Path.Combine (LogfilePath, NewName);

						File.Move (UOldFile, UNewFile);
					}
				}
			}

			///<description>change the newest logfile to -01.log</description>
			string Name = LogFileNameWithoutExtention + "-01" + Extention;
			string NewFile = Path.Combine (LogfilePath, Name);

			File.Move (ULogFileName, NewFile);
		}

		/// <summary>
		/// Cheks if there was an Write Access to the file today.
		/// </summary>
		/// <returns><c>true</c>, if to rotate files was needed (no write access today), <c>false</c> otherwise.</returns>
		/// <param name="ULogFileName">U log file name.</param>
		private bool NeedToRotateFiles(string ULogFileName){
			if (!File.Exists (ULogFileName)) {
				return false;
			}

			FileInfo fileInfo = new FileInfo(ULogFileName);
			DateTime LastWriteTime = fileInfo.LastWriteTime;
			LastWriteTime = LastWriteTime.Date;

			DateTime NowDate = DateTime.Now;
			NowDate = NowDate.Date;

			return !LastWriteTime.Equals (NowDate);
		}

        /// <summary>
        /// the name of the current logfile
        /// </summary>
        /// <returns>the name of the current logfile</returns>
        public static String GetLogFileName()
        {
            return ULogFileName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALogFileMsg"></param>
        /// <returns></returns>
        public bool CanWriteLogFile(out String ALogFileMsg)
        {
            ALogFileMsg = FLogFileErrorMsg;
            return FCanWriteLogFile;
        }

        /// <summary>
        /// Log to file
        /// </summary>
        /// <param name="strFile">filename of logging file</param>
        /// <param name="strMessage">message to log</param>
        public static void Log(string strFile, string strMessage)
        {
            StreamWriter SWriter;
            FileStream FStream;

            try
            {
                FStream = new FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Write);
                SWriter = new StreamWriter(FStream);

                SWriter.BaseStream.Seek(0, SeekOrigin.End);

                if (TLogging.DebugLevel > 0)
                {
                    SWriter.WriteLine(Environment.NewLine +
                        (!USuppressDateAndTime ? DateTime.Now.ToString("dddd, dd-MMM-yyyy, HH:mm:ss.ff") : String.Empty) + "  " + ULogtextPrefix +
                        (!(USuppressDateAndTime && ULogtextPrefix.Length == 0) ? " : " : String.Empty) +
                        strMessage);
                }
                else
                {
                    SWriter.WriteLine(
                        Environment.NewLine + (!USuppressDateAndTime ? DateTime.Now.ToString(
                                                   "dddd, dd-MMM-yyyy, HH:mm:ss.ff") : String.Empty) + ULogtextPrefix +
                        (!(USuppressDateAndTime && ULogtextPrefix.Length == 0) ? " : " : String.Empty) +
                        strMessage);
                }

                SWriter.Flush();
                SWriter.Close();
                FStream.Close();
            }
            catch (Exception e)
            {
                // eg cannot find directory
                Console.WriteLine("TLogWriter:Log was not able to write to the log file");
                Console.WriteLine(e.ToString());

                // do not throw, this causes somehow problems on running nant test on ci-win
                // throw;
            }
        }

        /// <summary>
        /// Log message to the current logfile
        /// </summary>
        /// <param name="strMessage">message to log</param>
        public static void Log(string strMessage)
        {
            Log(ULogFileName, strMessage);
        }

        /// <summary>
        /// log a list of messages
        /// </summary>
        /// <param name="aList">the messages to log</param>
        /// <param name="isException">if this is an exception, the logging output will look a bit different</param>
        public static void Log(ArrayList aList, bool isException)
        {
            System.Int32 IIndex;
            StreamWriter SWriter;
            FileStream FStream;
            string AdditionalInfo;
            AdditionalInfo = "";
            try
            {
                FStream = new FileStream(ULogFileName, FileMode.OpenOrCreate, FileAccess.Write);
                SWriter = new StreamWriter(FStream);
                SWriter.BaseStream.Seek(0, SeekOrigin.End);

                if (isException)
                {
                    AdditionalInfo = "The application has encountered an Exception: The details are as follows:" + Environment.NewLine;
                }

                SWriter.WriteLine((((((Environment.NewLine +
                                       DateTime.Now.ToLongDateString()) + ", ") + DateTime.Now.ToLongTimeString()) + " : ") + AdditionalInfo));
                IIndex = 0;

                while (IIndex <= (aList.Count - 1))
                {
                    SWriter.WriteLine((aList[IIndex].ToString() + Environment.NewLine));
                }

                SWriter.Flush();
                SWriter.Close();
                FStream.Close();
            }
            catch (Exception)
            {
                // eg cannot find directory
                throw;
            }
        }

        #endregion
    }
}